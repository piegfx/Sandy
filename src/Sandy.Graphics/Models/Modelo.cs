using System.Numerics;
using System.Runtime.InteropServices;

namespace Sandy.Graphics.Models;

public static unsafe class Modelo
{
    public const string LibName = "modelo";
    
    public struct VertexPositionColorTextureNormalTangent
    {
        public Vector3 Position;
        public Vector4 Color;
        public Vector2 TexCoord;
        public Vector3 Normal;
        public Vector3 Tangent;
    }
    
    public struct Mesh
    {
        public VertexPositionColorTextureNormalTangent* Vertices;
        public nuint NumVertices;

        public uint* Indices;
        public nuint NumIndices;

        public ulong Material;
    }

    public struct Scene
    {
        public Mesh* Meshes;
        public nuint NumMeshes;
    }

    [DllImport(LibName, EntryPoint = "mdLoad")]
    public static extern Scene* Load(sbyte* path);
}