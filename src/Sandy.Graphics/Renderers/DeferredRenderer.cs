using System.Numerics;
using System.Reflection;
using Microsoft.VisualBasic.CompilerServices;
using Pie;
using Pie.ShaderCompiler;
using Sandy.Graphics.Renderers.Structs;
using Sandy.Graphics.Structs;
using Sandy.Math;
using PrimitiveType = Pie.PrimitiveType;
using Texture = Pie.Texture;

namespace Sandy.Graphics.Renderers;

public sealed class DeferredRenderer : Renderer3D
{
    private CameraInfo _camera;

    private DrawInfo _drawInfo;
    private GraphicsBuffer _drawInfoBuffer;

    private Shader _gBufferShader;
    private InputLayout _gBufferLayout;

    private Shader _lightPassShader;

    private DepthStencilState _depthStencilState;
    private BlendState _blendState;
    
    // TODO: Per texture sampler states
    private SamplerState _samplerState;
    
    private Pie.RasterizerState _rasterizerState;

    public override RenderTarget2D MainTarget { get; protected set; }
    
    public Framebuffer GBuffer;
    public Texture AlbedoTexture;
    public Texture FragPosTexture;
    public Texture NormalTexture;
    public Texture MetallicRoughnessTexture;
    
    public Texture DepthTexture;

    private Framebuffer _mainTargetFb;
    private Texture _mainTargetTexture;

    // Used when drawing the G-Buffer to the main target.
    private DepthStencilState _drawDepthStencil;

    public DeferredRenderer(Size<int> size)
    {
        GraphicsDevice device = Renderer.Instance.Device;

        CreateTargets(size, device);

        Renderer.Instance.LogMessage(LogType.Debug, "Creating G-buffer shader.");
        
        Assembly assembly = Assembly.GetExecutingAssembly();
        
        byte[] vertSpv = EmbeddedResource.Load(assembly, Renderer.ShaderNamespace + ".Render.Deferred.GBuffer_vert.spv");
        byte[] fragSpv = EmbeddedResource.Load(assembly, Renderer.ShaderNamespace + ".Render.Deferred.GBuffer_frag.spv");
        _gBufferShader = device.CreateShader(new[]
        {
            new ShaderAttachment(ShaderStage.Vertex, vertSpv, "VertexShader"),
            new ShaderAttachment(ShaderStage.Fragment, fragSpv, "PixelShader")
        });
        
        Renderer.Instance.LogMessage(LogType.Debug, "Creating G-buffer input layout.");

        _gBufferLayout = device.CreateInputLayout(new[]
        {
            new InputLayoutDescription(Format.R32G32B32_Float, 0, 0, InputType.PerVertex), // position
            new InputLayoutDescription(Format.R32G32_Float, 12, 0, InputType.PerVertex), // texCoord
            new InputLayoutDescription(Format.R32G32B32A32_Float, 20, 0, InputType.PerVertex), // color
            new InputLayoutDescription(Format.R32G32B32_Float, 36, 0, InputType.PerVertex), // normal
            new InputLayoutDescription(Format.R32G32B32_Float, 48, 0, InputType.PerVertex), // tangent
        });
        
        Renderer.Instance.LogMessage(LogType.Debug, "Creating light pass shader.");
        vertSpv = EmbeddedResource.Load(assembly, Renderer.ShaderNamespace + ".Render.Deferred.LightPass_vert.spv");
        fragSpv = EmbeddedResource.Load(assembly, Renderer.ShaderNamespace + ".Render.Deferred.LightPass_frag.spv");

        _lightPassShader = device.CreateShader(new[]
        {
            new ShaderAttachment(ShaderStage.Vertex, vertSpv, "VertexShader"),
            new ShaderAttachment(ShaderStage.Fragment, fragSpv, "PixelShader")
        }, new []
        {
            new SpecializationConstant(0, (uint) (device.Api is GraphicsApi.OpenGL or GraphicsApi.OpenGLES ? 1 : 0))
        });

        Renderer.Instance.LogMessage(LogType.Debug, "Creating draw info buffer.");
        _drawInfo = new DrawInfo();
        _drawInfoBuffer = device.CreateBuffer(BufferType.UniformBuffer, _drawInfo, true);
        
        Renderer.Instance.LogMessage(LogType.Debug, "Creating various states.");
        _depthStencilState = device.CreateDepthStencilState(DepthStencilStateDescription.LessEqual);
        _blendState = device.CreateBlendState(BlendStateDescription.Disabled);

        _samplerState = device.CreateSamplerState(SamplerStateDescription.AnisotropicRepeat);
        _rasterizerState = device.CreateRasterizerState(RasterizerStateDescription.CullNone);

        _drawDepthStencil = device.CreateDepthStencilState(DepthStencilStateDescription.Disabled);
    }

    internal override void BeginPass(in CameraInfo cameraInfo)
    {
        _camera = cameraInfo;
        
        GraphicsDevice device = Renderer.Instance.Device;
        device.SetFramebuffer(GBuffer);
        
        device.ClearColorBuffer(Color.Transparent);
        device.ClearDepthStencilBuffer(ClearFlags.Depth, 1.0f, 0);

        device.SetShader(_gBufferShader);

        device.SetDepthStencilState(_depthStencilState);
        device.SetBlendState(_blendState);

        Size<int> size = MainTarget.Size;
        device.Viewport = new System.Drawing.Rectangle(0, 0, size.Width, size.Height);
    }

    internal override void EndPass()
    {
        GraphicsDevice device = Renderer.Instance.Device;
        
        device.SetFramebuffer(MainTarget.PieFramebuffer);
        device.ClearColorBuffer(_camera.ClearColor.Value);

        device.SetRasterizerState(_rasterizerState);
        device.SetDepthStencilState(_drawDepthStencil);
        
        // As the vertices are defined directly in the shader, there is very little do actually do here.
        device.SetPrimitiveType(PrimitiveType.TriangleList);
        device.SetShader(_lightPassShader);
        device.SetTexture(2, AlbedoTexture, _samplerState);
        device.SetTexture(3, FragPosTexture, _samplerState);
        device.SetTexture(4, NormalTexture, _samplerState);
        device.SetTexture(5, MetallicRoughnessTexture, _samplerState);
        device.Draw(6);
    }

    internal override void Draw(Renderable renderable, in Matrix4x4 worldMatrix)
    {
        GraphicsDevice device = Renderer.Instance.Device;
        
        device.SetPrimitiveType(renderable.Material.PrimitiveType);
        
        device.SetRasterizerState(renderable.Material.RasterizerState.PieState);
        device.SetDepthStencilState(_depthStencilState);
        
        _drawInfo.WorldMatrix = worldMatrix;
        device.UpdateBuffer(_drawInfoBuffer, 0, _drawInfo);
        device.SetUniformBuffer(2, _drawInfoBuffer);
        
        device.SetTexture(3, renderable.Material.Albedo.PieTexture, _samplerState);
        device.SetTexture(4, renderable.Material.Normal.PieTexture, _samplerState);
        device.SetTexture(5, renderable.Material.Metallic.PieTexture, _samplerState);
        device.SetTexture(6, renderable.Material.Roughness.PieTexture, _samplerState);
        device.SetTexture(7, renderable.Material.AmbientOcclusion.PieTexture, _samplerState);

        device.SetVertexBuffer(0, renderable.VertexBuffer, VertexPositionTextureColorNormalTangent.SizeInBytes,
            _gBufferLayout);
        device.SetIndexBuffer(renderable.IndexBuffer, IndexType.UInt);
        
        device.DrawIndexed(renderable.NumIndices);
    }

    internal override void Resize(Size<int> newSize)
    {
        Renderer.Instance.LogMessage(LogType.Debug, $"Recreating render targets at size {newSize}.");
        
        DisposeTargets();
        CreateTargets(newSize, Renderer.Instance.Device);
    }

    public override (string, Texture2D)[] GetRenderPassTextures()
    {
        return new []
        {
            ("output", MainTarget),
            ("albedo", new Texture2D(AlbedoTexture)),
            ("fragPos", new Texture2D(FragPosTexture)),
            ("normal", new Texture2D(NormalTexture)),
            ("metallicRoughness", new Texture2D(MetallicRoughnessTexture)),
        };
    }

    private void CreateTargets(Size<int> size, GraphicsDevice device)
    {
        Renderer.Instance.LogMessage(LogType.Debug, "Creating G-buffer.");

        // We can reuse this description many times for all the different textures.
        TextureDescription description = TextureDescription.Texture2D(size.Width, size.Height,
            Format.R32G32B32A32_Float, 1, 1, TextureUsage.Framebuffer | TextureUsage.ShaderResource);

        AlbedoTexture = device.CreateTexture(description);
        FragPosTexture = device.CreateTexture(description);
        NormalTexture = device.CreateTexture(description);
        MetallicRoughnessTexture = device.CreateTexture(description);

        description.Format = Format.D32_Float;
        description.Usage = TextureUsage.Framebuffer;
        DepthTexture = device.CreateTexture(description);

        GBuffer = device.CreateFramebuffer(new[]
        {
            new FramebufferAttachment(AlbedoTexture),
            new FramebufferAttachment(FragPosTexture),
            new FramebufferAttachment(NormalTexture),
            new FramebufferAttachment(MetallicRoughnessTexture),
            
            new FramebufferAttachment(DepthTexture)
        });
        
        Renderer.Instance.LogMessage(LogType.Debug, "Creating main target.");

        _mainTargetTexture = device.CreateTexture(TextureDescription.Texture2D(size.Width, size.Height,
            Format.R8G8B8A8_UNorm, 1, 1, TextureUsage.Framebuffer | TextureUsage.ShaderResource));

        _mainTargetFb = device.CreateFramebuffer(new[]
        {
            new FramebufferAttachment(_mainTargetTexture),
            new FramebufferAttachment(DepthTexture)
        });

        MainTarget = new RenderTarget2D(_mainTargetFb, _mainTargetTexture, DepthTexture);
    }

    private void DisposeTargets()
    {
        Renderer.Instance.LogMessage(LogType.Debug, "Disposing G-buffer.");
        
        GBuffer.Dispose();
        AlbedoTexture.Dispose();
        FragPosTexture.Dispose();
        NormalTexture.Dispose();
        MetallicRoughnessTexture.Dispose();
        DepthTexture.Dispose();
        
        Renderer.Instance.LogMessage(LogType.Debug, "Disposing main targets.");
        _mainTargetFb.Dispose();
        _mainTargetTexture.Dispose();
    }

    public override void Dispose()
    {
        _drawInfoBuffer.Dispose();
        _gBufferShader.Dispose();
        _gBufferLayout.Dispose();
        _lightPassShader.Dispose();
        _depthStencilState.Dispose();
        _blendState.Dispose();
        _samplerState.Dispose();
        _rasterizerState.Dispose();
        DisposeTargets();
        MainTarget.Dispose();
    }
}