using System;
using System.Numerics;
using Sandy.Framework;
using Sandy.Graphics;
using Sandy.Graphics.Lighting;
using Sandy.Graphics.Materials;
using Sandy.Graphics.Models;
using Sandy.Graphics.Models.Primitives;
using Sandy.Graphics.Renderers;
using Sandy.Graphics.Structs;
using Sandy.Graphics.Text;
using Sandy.Math;

namespace BasicCube;

// Basic demo of a rotating cube with lighting.
public class MyApp : SandyApp
{
    private Renderable _cube;
    private Quaternion _rotation;
    private Font _font;

    protected override void Initialize()
    {
        base.Initialize();

        // Create our material from the textures.
        Material material = new Material(
            new Texture2D("MetalGrid/metalgrid2_basecolor.png"),
            new Texture2D("MetalGrid/metalgrid2_normal-dx.png"),
            new Texture2D("MetalGrid/metalgrid2_metallic.png"),
            new Texture2D("MetalGrid/metalgrid2_roughness.png"),
            new Texture2D("MetalGrid/metalgrid2_AO.png")
        )
        {
            // Materials default to a counter clockwise rasterizer state, however currently, the cube primitive uses
            // a clockwise rasterizer state.
            // TODO: This needs fixing.
            RasterizerState = RasterizerState.CullClockwise
        };
        
        // Renderables are an object that can be drawn, containing a vertices, indices, and a material.
        Cube cube = new Cube();
        _cube = new Renderable(cube.Vertices, cube.Indices, material);
        
        _rotation = Quaternion.Identity;
        
        // Roboto is built in :)
        _font = Font.Roboto;
    }

    protected override void Update(Time time, Input input)
    {
        base.Update(time, input);

        _rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitX, 1 * (float) time.DeltaTime.TotalSeconds) *
                     Quaternion.CreateFromAxisAngle(Vector3.UnitY, 0.75f * (float) time.DeltaTime.TotalSeconds) *
                     Quaternion.CreateFromAxisAngle(Vector3.UnitZ, 0.5f * (float) time.DeltaTime.TotalSeconds);
    }

    protected override void Draw(Time time, Input input)
    {
        base.Draw(time, input);
        
        // Begin a new frame. This clears everything from the previous frame.
        // Once begun, you should NOT draw anything manually, using the sprite renderer or GraphicsDevice.
        // At best, you will probably see nothing rendered at all.
        Renderer.NewFrame();

        // Create our directional light. You don't have to define this each frame, you can set it in initialization.
        // You can even make it null, if you want to disable the directional light.
        Renderer.DirectionalLight = new DirectionalLight()
        {
            Color = Color.White,
            Direction = Quaternion.Identity
        };

        // Send our cube into the draw cube. It will not actually be drawn yet.
        Renderer.DrawOpaque(_cube, Matrix4x4.CreateFromQuaternion(_rotation));

        Size<int> winSize = Window.FramebufferSize;
        Vector3 cameraPos = new Vector3(0, 0, 2);
        
        // Define our camera info. This is what our 3D pass will use to determine where and how to render.
        CameraInfo camera = new CameraInfo()
        {
            Projection = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.ToRadians(75), winSize.Width / (float) winSize.Height, 0.1f, 100f),
            View = Matrix4x4.CreateLookAt(cameraPos, Vector3.Zero, Vector3.UnitY),
            Position = cameraPos,
            ClearColor = new Color(1.0f, 0.5f, 0.25f, 1.0f),
            Viewport = new Rectangle<float>(0, 0, 1, 1)
        };
        
        // Perform the 3D pass! This will sort the objects, then perform at least one pass, based on the given camera info.
        // If shadows, or transparent objects are involved, multiple passes will be performed.
        // You can perform multiple 3D passes per frame, without needing to redraw everything. The render handles this
        // for you. TODO this does not actually work right now lol
        Renderer.Perform3DPass(camera);
        
        // End the frame. The renderer will do some final post processing, and it is now safe to manually draw things
        // using the sprite renderer or GraphicsDevice if you wish.
        Renderer.EndFrame();

        ref SpriteRenderer sprite = ref Renderer.SpriteRenderer;
        sprite.Begin();
        _font.Draw(sprite, 36, "Hello, this is a spinning cube. This is how you can draw text!", Vector2.Zero, Color.White);
        sprite.End();
    }

    public override void Dispose()
    {
        // For now, the material does not claim ownership over anything.
        // TODO: Change this behaviour/DisposeTextures method?
        _cube.Material.Albedo.Dispose();
        _cube.Material.Normal.Dispose();
        _cube.Material.Metallic.Dispose();
        _cube.Material.Roughness.Dispose();
        _cube.Material.Albedo.Dispose();
        _cube.Material.RasterizerState.Dispose();
        
        base.Dispose();
    }

    public MyApp(AppOptions options) : base(options) { }
}