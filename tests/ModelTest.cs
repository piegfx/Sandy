using System;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Pie.Windowing;
using Sandy.Framework;
using Sandy.Graphics;
using Sandy.Graphics.Lighting;
using Sandy.Graphics.Materials;
using Sandy.Graphics.Models;
using Sandy.Graphics.Models.Primitives;
using Sandy.Graphics.Renderers;
using Sandy.Graphics.Structs;
using Sandy.Math;

namespace Sandy.Tests;

public class ModelTest : SandyApp
{
    private Renderable[] _renderables;

    private Vector3 _position;
    private Vector3 _rotation;

    private Quaternion _objRot;
    
    protected override unsafe void Initialize()
    {
        base.Initialize();

        //Cube cube = new Cube();
        //_renderable = new Renderable(cube.Vertices, cube.Indices, new Material(Texture2D.White));

        Modelo.Scene* scene;

        //string path = "/home/skye/Documents/Cubebs/IMyDefaultCube2GLTFseparate.gltf";
        string path = "/home/skye/Downloads/Fox.gltf";
        //string path = "/home/skye/Downloads/ionthrusterconcept01.gltf";

        string dir = Path.GetDirectoryName(path);

        fixed (byte* pathPtr = Encoding.Default.GetBytes(path))
            Modelo.Load((sbyte*) pathPtr, Modelo.LoadFlagsGenerateIndices | Modelo.LoadFlagsGenerateNormals, &scene);

        if (scene->NumMeshes == 0)
            throw new Exception("num meshes is 0");

        _renderables = new Renderable[scene->NumMeshes];

        for (nuint i = 0; i < scene->NumMeshes; i++)
        {
            Modelo.Mesh mesh = scene->Meshes[0];
            VertexPositionTextureColorNormalTangent[] vertices =
                new VertexPositionTextureColorNormalTangent[mesh.NumVertices];

            fixed (VertexPositionTextureColorNormalTangent* vPtr = vertices)
                Unsafe.CopyBlock(vPtr, mesh.Vertices, (uint) mesh.NumVertices * VertexPositionTextureColorNormalTangent.SizeInBytes);

            uint[] indices = null;

            if (mesh.Indices != null)
            {
                indices = new uint[mesh.NumIndices];

                fixed (uint* indPtr = indices)
                    Unsafe.CopyBlock(indPtr, mesh.Indices, (uint) mesh.NumIndices * sizeof(uint));
            }

            Material material;

            if (mesh.Material != nuint.MaxValue)
            {
                ref Modelo.Material mat = ref scene->Materials[mesh.Material];
                
                Texture2D albedo = Texture2D.White;

                if (mat.AlbedoTexture != nuint.MaxValue)
                {
                    string texturePath = Marshal.PtrToStringAnsi((nint) scene->Images[mat.AlbedoTexture].Path);

                    albedo = new Texture2D(Path.Combine(dir, texturePath));
                }

                material = new Material(albedo);
            }
            else
            {
                material = new Material(Texture2D.White);
            }

            _renderables[i] = new Renderable(vertices, indices, material);
        }
        
        Modelo.Free(scene);

        _position = new Vector3(0, 0, 2);
        _objRot = Quaternion.Identity;
    }

    protected override void Update(Time time, Input input)
    {
        base.Update(time, input);
        
        /*_objRot *= Quaternion.CreateFromAxisAngle(Vector3.UnitX, 1 * (float) time.DeltaTime.TotalSeconds) *
                     Quaternion.CreateFromAxisAngle(Vector3.UnitY, 0.75f * (float) time.DeltaTime.TotalSeconds) *
                     Quaternion.CreateFromAxisAngle(Vector3.UnitZ, 0.5f * (float) time.DeltaTime.TotalSeconds);*/

        Quaternion rotation = Quaternion.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z);

        Vector3 forward = Vector3.Transform(-Vector3.UnitZ, rotation);
        Vector3 right = Vector3.Transform(Vector3.UnitX, rotation);
        Vector3 up = Vector3.Transform(Vector3.UnitY, rotation);

        float speed = 10 * (float) time.DeltaTime.TotalSeconds;

        if (input.IsKeyDown(Key.W))
            _position += forward * speed;
        if (input.IsKeyDown(Key.S))
            _position -= forward * speed;
        if (input.IsKeyDown(Key.A))
            _position -= right * speed;
        if (input.IsKeyDown(Key.D))
            _position += right * speed;
        if (input.IsKeyDown(Key.Space))
            _position += up * speed;
        if (input.IsKeyDown(Key.C))
            _position -= up * speed;

        const float mouseSpeed = 0.01f;

        _rotation.X -= input.MouseDelta.X * mouseSpeed;
        _rotation.Y -= input.MouseDelta.Y * mouseSpeed;
    }

    protected override void Draw(Time time, Input input)
    {
        base.Draw(time, input);
        
        Renderer.NewFrame();
        
        Renderer.DirectionalLight = new DirectionalLight()
        {
            Color = Color.White,
            Direction = Quaternion.CreateFromYawPitchRoll(MathHelper.PiOver4, -MathF.PI / 2, 0)
        };
        
        for (int i = 0; i < _renderables.Length; i++)
            Renderer.DrawOpaque(_renderables[i], Matrix4x4.CreateFromQuaternion(_objRot));

        Size<int> winSize = Window.FramebufferSize;
        //Vector3 cameraPos = new Vector3(0, 0, 2);
        Vector3 cameraPos = _position;
        Quaternion cameraRot = Quaternion.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z);
        
        Vector3 cameraForward = Vector3.Transform(-Vector3.UnitZ, cameraRot);
        Vector3 cameraUp = Vector3.Transform(Vector3.UnitY, cameraRot);
        
        CameraInfo camera = new CameraInfo()
        {
            Projection = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.ToRadians(75), winSize.Width / (float) winSize.Height, 0.1f, 1000f),
            View = Matrix4x4.CreateLookAt(cameraPos, cameraPos + cameraForward, cameraUp),
            Position = cameraPos,
            ClearColor = new Color(1.0f, 0.5f, 0.25f, 1.0f),
            Viewport = new Rectangle<float>(0, 0, 1, 1)
        };
        
        Renderer.Perform3DPass(camera);
        
        Renderer.EndFrame(false);
    }

    public ModelTest(AppOptions options) : base(options) { }
}