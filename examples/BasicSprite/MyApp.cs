using System;
using System.Numerics;
using Pie.Windowing;
using Sandy.Framework;
using Sandy.Graphics;
using Sandy.Graphics.Renderers;
using Sandy.Math;

namespace BasicSprite;

// Draw a simple rotating texture.
public class MyApp : SandyApp
{
    private Texture2D _texture;
    private float _rotation;
    
    protected override void Initialize()
    {
        base.Initialize();
        
        Window.Resize += WindowOnResize;

        _texture = new Texture2D("awesomeface.png");
    }

    protected override void Update(Time time, Input input)
    {
        base.Update(time, input);

        _rotation += 1 * (float) time.DeltaTime.TotalSeconds;
        _rotation = MathHelper.Wrap(_rotation, 0, MathF.Tau);
        
        if (input.IsKeyPressed(Key.Escape))
            Close();
    }

    protected override void Draw(Time time, Input input)
    {
        base.Draw(time, input);
        
        // For this demo, we bypass the renderer entirely, only using its sprite renderer.
        // As such, there is no need to call NewFrame or EndFrame (doing so may break the application depending on where
        // you call it), and no need to perform any passes.
        // However, because of this, we do need to clear the color buffer manually.

        // TODO: Add a 2D renderer engine
        Renderer.Device.ClearColorBuffer(Color.CornflowerBlue);
        
        Size<int> windowSize = Window.FramebufferSize;
        Size<int> textureSize = _texture.Size;

        ref SpriteRenderer spriteRenderer = ref Renderer.SpriteRenderer;
        spriteRenderer.Begin();
        spriteRenderer.Draw(_texture, new Vector2(windowSize.Width, windowSize.Height) / 2, null, Color.White,
            _rotation, Vector2.One, new Vector2(textureSize.Width, textureSize.Height) / 2);
        spriteRenderer.End();
    }

    public override void Dispose()
    {
        _texture.Dispose();
        
        base.Dispose();
    }

    private void WindowOnResize(Size<int> size)
    {
        Renderer.Resize(size);
    }

    public MyApp(AppOptions options) : base(options) { }
}