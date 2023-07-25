using System;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.CompilerServices;
using Pie;
using Pie.ShaderCompiler;
using Sandy.Graphics.Exceptions;
using Sandy.Math;

namespace Sandy.Graphics.Renderers;

public sealed class SpriteRenderer : IDisposable
{
    public const uint MaxSprites = 1 << 14;

    private const uint NumVertices = 4;
    private const uint NumIndices = 6;

    private const uint MaxVertices = NumVertices * MaxSprites;
    private const uint MaxIndices = NumIndices * MaxSprites;
    
    private GraphicsDevice _device;

    private SpriteVertex[] _vertices;
    private uint[] _indices;

    private GraphicsBuffer _vertexBuffer;
    private GraphicsBuffer _indexBuffer;

    private GraphicsBuffer _spriteMatricesBuffer;

    private Shader _shader;
    private InputLayout _layout;

    private DepthStencilState _depthStencilState;
    private Pie.RasterizerState _rasterizerState;
    private BlendState _blendState;
    
    // TODO: In-engine sampler states
    private SamplerState _samplerState;

    private Texture2D _currentTexture;
    private uint _currentSprite;

    private bool _hasBegun;

    public bool HasBegun => _hasBegun;

    public SpriteRenderer(GraphicsDevice device)
    {
        _device = device;

        _vertices = new SpriteVertex[MaxVertices];
        _indices = new uint[MaxIndices];

        _vertexBuffer = device.CreateBuffer(BufferType.VertexBuffer, MaxVertices * SpriteVertex.SizeInBytes, true);
        _indexBuffer = device.CreateBuffer(BufferType.IndexBuffer, MaxIndices * sizeof(uint), true);

        _spriteMatricesBuffer = device.CreateBuffer(BufferType.UniformBuffer, Matrix4x4.Identity, true);
        
        Assembly assembly = Assembly.GetExecutingAssembly();
        byte[] vertSpv = EmbeddedResource.Load(assembly, Renderer.ShaderNamespace + ".Sprite.Sprite_vert.spv");
        byte[] fragSpv = EmbeddedResource.Load(assembly, Renderer.ShaderNamespace + ".Sprite.Sprite_frag.spv");

        _shader = device.CreateShader(new[]
        {
            new ShaderAttachment(ShaderStage.Vertex, vertSpv, "VertexShader"),
            new ShaderAttachment(ShaderStage.Fragment, fragSpv, "PixelShader")
        });

        _layout = device.CreateInputLayout(new[]
        {
            new InputLayoutDescription(Format.R32G32_Float, 0, 0, InputType.PerVertex), // position
            new InputLayoutDescription(Format.R32G32_Float, 8, 0, InputType.PerVertex), // worldPos
            new InputLayoutDescription(Format.R32G32_Float, 16, 0, InputType.PerVertex), // texCoord
            new InputLayoutDescription(Format.R32G32B32A32_Float, 24, 0, InputType.PerVertex), // tint
            new InputLayoutDescription(Format.R32_Float, 40, 0, InputType.PerVertex), // rotation
        });

        _depthStencilState = device.CreateDepthStencilState(DepthStencilStateDescription.Disabled);
        _rasterizerState = device.CreateRasterizerState(RasterizerStateDescription.CullClockwise);
        _blendState = device.CreateBlendState(BlendStateDescription.NonPremultiplied);

        _samplerState = device.CreateSamplerState(SamplerStateDescription.LinearClamp);

        _currentTexture = null;
        _currentSprite = 0;
    }

    public void Draw(Texture2D texture, Vector2 position, Rectangle<int>? source, Color tint, float rotation, Vector2 scale, Vector2 origin, Flip flip = Flip.None)
    {
        if (!_hasBegun)
            throw new SpriteSessionException("There is no currently active sprite renderer session.");
        
        if (_currentTexture != texture || _currentSprite >= MaxSprites)
            Flush();

        _currentTexture = texture;

        Size<int> texSize = texture.Size;
        Rectangle<int> srcRect = source ?? new Rectangle<int>(Vector2T<int>.Zero, texSize);

        float texX = srcRect.X / (float) texSize.Width;
        float texY = srcRect.Y / (float) texSize.Height;
        float texW = srcRect.Width / (float) texSize.Width;
        float texH = srcRect.Height / (float) texSize.Height;

        if (texture is RenderTarget2D && _device.Api is GraphicsApi.OpenGL or GraphicsApi.OpenGLES)
            flip ^= Flip.FlipY;

        if ((flip & Flip.FlipX) == Flip.FlipX)
        {
            texW = -texW;
            texX = 1f - texX;
        }

        if ((flip & Flip.FlipY) == Flip.FlipY)
        {
            texH = -texH;
            texY = 1f - texY;
        }

        float x = -origin.X * scale.X;
        float y = -origin.Y * scale.Y;
        float w = srcRect.Width * scale.X;
        float h = srcRect.Height * scale.Y;

        uint currentVertex = _currentSprite * NumVertices;
        uint currentIndex = _currentSprite * NumIndices;

        _vertices[currentVertex + 0] = new SpriteVertex(new Vector2(x, y), position, new Vector2(texX, texY), tint, rotation);
        _vertices[currentVertex + 1] = new SpriteVertex(new Vector2(x + w, y), position, new Vector2(texX + texW, texY), tint, rotation);
        _vertices[currentVertex + 2] = new SpriteVertex(new Vector2(x + w, y + h), position, new Vector2(texX + texW, texY + texH), tint, rotation);
        _vertices[currentVertex + 3] = new SpriteVertex(new Vector2(x, y + h), position, new Vector2(texX, texY + texH), tint, rotation);

        _indices[currentIndex + 0] = 0 + currentVertex;
        _indices[currentIndex + 1] = 1 + currentVertex;
        _indices[currentIndex + 2] = 3 + currentVertex;
        _indices[currentIndex + 3] = 1 + currentVertex;
        _indices[currentIndex + 4] = 2 + currentVertex;
        _indices[currentIndex + 5] = 3 + currentVertex;

        _currentSprite++;
    }

    public void DrawRectangle(Vector2 position, Size<float> size, Color color)
    {
        Draw(Texture2D.White, position, null, color, 0, new Vector2(size.Width, size.Height), Vector2.Zero);
    }

    public void DrawBorder(Vector2 position, Size<float> size, float thickness, Color color)
    {
        DrawRectangle(position, new Size<float>(thickness, size.Height), color);
        DrawRectangle(position, new Size<float>(size.Width, thickness), color);
        DrawRectangle(new Vector2(position.X + size.Width - thickness, position.Y), new Size<float>(thickness, size.Height), color);
        DrawRectangle(new Vector2(position.X, position.Y + size.Height - thickness), new Size<float>(size.Width, thickness), color);
    }

    public void Begin()
    {
        if (_hasBegun)
            throw new SpriteSessionException("There is already an active sprite renderer session.");

        _hasBegun = true;
        
        System.Drawing.Size size = Renderer.Instance.Device.Swapchain.Size;

        _device.UpdateBuffer(_spriteMatricesBuffer, 0,
            Matrix4x4.CreateOrthographicOffCenter(0, size.Width, size.Height, 0, -1, 1));
    }

    public void End()
    {
        if (!_hasBegun)
            throw new SpriteSessionException("There is no currently active sprite session to end.");

        _hasBegun = false;

        Flush();
    }

    private void Flush()
    {
        // Don't try and process anything if there is nothing to flush.
        // It can result in weird behaviour.
        if (_currentSprite == 0)
            return;

        MappedSubresource vrt = _device.MapResource(_vertexBuffer, MapMode.Write);
        PieUtils.CopyToUnmanaged(vrt.DataPtr, 0, _currentSprite * NumVertices * SpriteVertex.SizeInBytes, _vertices);
        _device.UnmapResource(_vertexBuffer);

        MappedSubresource idx = _device.MapResource(_indexBuffer, MapMode.Write);
        PieUtils.CopyToUnmanaged(idx.DataPtr, 0, _currentSprite * NumIndices * sizeof(uint), _indices);
        _device.UnmapResource(_indexBuffer);
        
        _device.SetPrimitiveType(PrimitiveType.TriangleList);
        
        _device.SetShader(_shader);
        
        _device.SetUniformBuffer(0, _spriteMatricesBuffer);
        
        _device.SetTexture(1, _currentTexture.PieTexture, _samplerState);
        
        _device.SetDepthStencilState(_depthStencilState);
        _device.SetRasterizerState(_rasterizerState);
        _device.SetBlendState(_blendState);
        
        _device.SetVertexBuffer(0, _vertexBuffer, SpriteVertex.SizeInBytes, _layout);
        _device.SetIndexBuffer(_indexBuffer, IndexType.UInt);
        
        _device.DrawIndexed(_currentSprite * NumIndices);

        _currentSprite = 0;
    }
    
    public void Dispose()
    {
        _vertexBuffer.Dispose();
        _indexBuffer.Dispose();
        _spriteMatricesBuffer.Dispose();
        _shader.Dispose();
        _layout.Dispose();
        _depthStencilState.Dispose();
        _rasterizerState.Dispose();
        _blendState.Dispose();
        _samplerState.Dispose();
    }

    [Flags]
    public enum Flip
    {
        None = 0,
        FlipX = 1 << 0,
        FlipY = 1 << 1,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SpriteVertex
    {
        public Vector2 Position;
        public Vector2 WorldPosition;
        public Vector2 TexCoord;
        public Color Tint;
        public float Rotation;

        public SpriteVertex(Vector2 position, Vector2 worldPosition, Vector2 texCoord, Color tint, float rotation)
        {
            Position = position;
            WorldPosition = worldPosition;
            TexCoord = texCoord;
            Tint = tint;
            Rotation = rotation;
        }

        public const uint SizeInBytes = 44;
    }
}
