using System;
using System.Numerics;
using System.Runtime.CompilerServices;
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
    private Renderable _renderable;

    private Quaternion _rotation;
    
    protected override unsafe void Initialize()
    {
        base.Initialize();

        //Cube cube = new Cube();
        //_renderable = new Renderable(cube.Vertices, cube.Indices, new Material(Texture2D.White));

        Modelo.Scene* scene;

        fixed (byte* path = "/home/skye/Downloads/Cubebs/IMyDefaultCube2GLTFseparate.gltf"u8)
            scene = Modelo.Load((sbyte*) path);

        if (scene->NumMeshes == 0)
            throw new Exception("num meshes is 0");

        Modelo.Mesh mesh = scene->Meshes[0];
        VertexPositionTextureColorNormalTangent[] vertices =
            new VertexPositionTextureColorNormalTangent[mesh.NumVertices];

        for (nuint  i = 0; i < mesh.NumVertices; i++)
        {
            Modelo.VertexPositionColorTextureNormalTangent vpctnt = mesh.Vertices[i];
            
            vertices[i] = new VertexPositionTextureColorNormalTangent()
            {
                Position = vpctnt.Position,
                TexCoord = vpctnt.TexCoord,
                Color = Color.White,
                Normal = vpctnt.Normal,
                Tangent = vpctnt.Tangent
            };
        }

        uint[] indices = new uint[mesh.NumIndices];
        
        fixed (uint* indPtr = indices)
            Unsafe.CopyBlock(indPtr, mesh.Indices, (uint) mesh.NumIndices * sizeof(uint));

        _renderable = new Renderable(vertices, indices, new Material(Texture2D.White));

        _rotation = Quaternion.Identity;
    }

    protected override void Draw(Time time, Input input)
    {
        base.Draw(time, input);
        
        Renderer.NewFrame();
        
        Renderer.DirectionalLight = new DirectionalLight()
        {
            Color = Color.White,
            Direction = Quaternion.Identity
        };
        
        Renderer.DrawOpaque(_renderable, Matrix4x4.CreateFromQuaternion(_rotation));

        Size<int> winSize = Window.FramebufferSize;
        Vector3 cameraPos = new Vector3(0, 0, 2);
        
        CameraInfo camera = new CameraInfo()
        {
            Projection = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.ToRadians(75), winSize.Width / (float) winSize.Height, 0.1f, 100f),
            View = Matrix4x4.CreateLookAt(cameraPos, Vector3.Zero, Vector3.UnitY),
            Position = cameraPos,
            ClearColor = new Color(1.0f, 0.5f, 0.25f, 1.0f),
            Viewport = new Rectangle<float>(0, 0, 1, 1)
        };
        
        Renderer.Perform3DPass(camera);
        
        Renderer.EndFrame();
    }

    public ModelTest(AppOptions options) : base(options) { }
}