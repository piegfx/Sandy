using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;
using Pie;
using Sandy.Graphics.Exceptions;
using Sandy.Graphics.Lighting;
using Sandy.Graphics.Renderers;
using Sandy.Graphics.Renderers.Structs;
using Sandy.Graphics.Structs;
using Sandy.Graphics.Text;
using Sandy.Math;

namespace Sandy.Graphics;

public class Renderer : IDisposable
{
    public event OnLog Log;
    
    private List<(Renderable, Matrix4x4)> _opaques;

    private GraphicsBuffer _cameraMatricesBuffer;
    private GraphicsBuffer _sceneInfoBuffer;

    public GraphicsDevice Device;
    public SpriteRenderer SpriteRenderer;

    public Renderer3D Renderer3D;

    public bool VSync;

    public DirectionalLight? DirectionalLight;

    public RendererSettings Settings;

    public Renderer(GraphicsDevice device, bool vsync = true)
    {
        if (Instance != null)
            throw new MultipleInstanceException("Cannot have multiple render instances!");
        
        Device = device;
        Instance = this;

        Log = delegate { };

        VSync = vsync;
        
        LogMessage(LogType.Info, $"Adapter info:\n    Adapter: {device.Adapter.Name}");
        
        LogMessage(LogType.Debug, "Creating sprite renderer.");
        SpriteRenderer = new SpriteRenderer(device);
        
        LogMessage(LogType.Debug, "Creating renderer impl (deferred renderer).");
        Renderer3D = new DeferredRenderer((Size<int>) device.Swapchain.Size);

        LogMessage(LogType.Debug, "Creating renderer settings.");
        Settings = new RendererSettings(0.03f);

        _opaques = new List<(Renderable, Matrix4x4)>();

        LogMessage(LogType.Debug, "Creating camera matrices buffer.");
        _cameraMatricesBuffer = device.CreateBuffer(BufferType.UniformBuffer, (uint) Unsafe.SizeOf<CameraMatrices>(), true);
        
        LogMessage(LogType.Debug, "Creating scene info buffer.");
        _sceneInfoBuffer = device.CreateBuffer(BufferType.UniformBuffer, (uint) Unsafe.SizeOf<SceneInfo>(), true);
    }

    public void NewFrame()
    {
        _opaques.Clear();
    }

    private (string, Texture2D)[] _renderPassTextures;
    private Font _renderPassFont;
    
    public void EndFrame(bool debugDraw = false)
    {
        Device.SetFramebuffer(null);
        Device.ClearColorBuffer(Color.Black);

        if (debugDraw)
        {
            if (_renderPassTextures == null)
            {
                _renderPassTextures = Renderer3D.GetRenderPassTextures();
            }
            
            if (_renderPassFont == null)
            {
                _renderPassFont = new Font(EmbeddedResource.Load(Assembly.GetExecutingAssembly(),
                    "Sandy.Graphics.Text.Roboto-Regular.ttf"));
            }

            SpriteRenderer.Begin();

            Vector2 pos = Vector2.Zero;
            Vector2 scale = new Vector2(1 / 3f + 0.001f);
            foreach ((string name, Texture2D texture) in _renderPassTextures)
            {
                SpriteRenderer.Flip flip =
                    Device.Api is GraphicsApi.OpenGL or GraphicsApi.OpenGLES && texture is not RenderTarget2D
                        ? SpriteRenderer.Flip.FlipY
                        : SpriteRenderer.Flip.None;

                SpriteRenderer.Draw(texture, pos, null, Color.White, 0, scale, Vector2.Zero, flip);
            
                _renderPassFont.Draw(SpriteRenderer, 20, name, pos, Color.White);
            
                pos.X += (int) (texture.Size.Width * scale.X);

                if (pos.X >= Renderer3D.MainTarget.Size.Width)
                {
                    pos.X = 0;
                    pos.Y += (int) (texture.Size.Height * scale.Y);
                }
            }
        
            SpriteRenderer.End();
        }

        else
        {
            SpriteRenderer.Begin();
            SpriteRenderer.Draw(Renderer3D.MainTarget, Vector2.Zero, null, Color.White, 0, Vector2.One,
                Vector2.Zero);
            SpriteRenderer.End();
        }
    }

    public void DrawOpaque(Renderable renderable, in Matrix4x4 worldMatrix)
    {
        _opaques.Add((renderable, worldMatrix));
    }

    public void Perform3DPass(CameraInfo cameraInfo)
    {
        IEnumerable<(Renderable, Matrix4x4)> opaques = _opaques.OrderBy(tRenderable =>
            Vector3.Distance(tRenderable.Item2.Translation, cameraInfo.Position));

        CameraMatrices matrices = new CameraMatrices()
        {
            Projection = cameraInfo.Projection,
            View = cameraInfo.View,
            Position = new Vector4(cameraInfo.Position, 1.0f)
        };
        
        Device.UpdateBuffer(_cameraMatricesBuffer, 0, matrices);
        Device.SetUniformBuffer(0, _cameraMatricesBuffer);

        SceneInfo scene = new SceneInfo()
        {
            DirectionalLight = DirectionalLight?.LightInfo ?? new LightInfo(),
            AmbientColor = Settings.Ambient
        };
        
        Device.UpdateBuffer(_sceneInfoBuffer, 0, scene);
        Device.SetUniformBuffer(1, _sceneInfoBuffer);
        
        Renderer3D.BeginPass(cameraInfo);
        
        foreach ((Renderable renderable, Matrix4x4 world) in opaques)
            Renderer3D.Draw(renderable, world);

        Renderer3D.EndPass();
    }

    public void Present()
    {
        // TODO: This looks like its a bug in Pie's opengl implementation.
        // Shouldn't need to set framebuffer to null here, if you bypass the renderer and use the graphics device
        // directly, it should render fine. But nothing is rendered. Does not happen under Direct3D.
        Device.SetFramebuffer(null);
        Device.Present(VSync ? 1 : 0);
    }

    public void Resize(Size<int> newSize)
    {
        Device.ResizeSwapchain((System.Drawing.Size) newSize);
        Device.Viewport = new System.Drawing.Rectangle(0, 0, newSize.Width, newSize.Height);
        Renderer3D.Resize(newSize);

        _renderPassTextures = null;
    }

    public void Dispose()
    {
        LogMessage(LogType.Debug, "Disposing camera matrices buffer.");
        _cameraMatricesBuffer.Dispose();
        
        LogMessage(LogType.Debug, "Disposing scene info buffer.");
        _sceneInfoBuffer.Dispose();
        
        LogMessage(LogType.Debug, "Disposing sprite renderer.");
        SpriteRenderer.Dispose();
        
        LogMessage(LogType.Debug, "Disposing device.");
        Device.Dispose();
        
        LogMessage(LogType.Debug, "Renderer disposal complete.");
        Instance = null;
    }

    internal void LogMessage(LogType type, string message) => Log.Invoke(type, message);

    public static Renderer Instance { get; private set; }

    public const string ShaderNamespace = "Sandy.Graphics.Shaders";

    public delegate void OnLog(LogType type, string message);
}