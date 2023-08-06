using System.Numerics;
using System.Runtime.InteropServices;
using Sandy.Graphics.Materials;

namespace Sandy.Graphics.Models;

public static unsafe class Modelo
{
    public const string LibName = "modelo";

    public const uint LoadFlagsNone = 0;
    public const uint LoadFlagsGenerateIndices = 1 << 0;
    public const uint LoadFlagsGenerateNormals = 1 << 1;

    [StructLayout(LayoutKind.Sequential)]
    public struct Mesh
    {
        public VertexPositionTextureColorNormalTangent* Vertices;
        public nuint NumVertices;

        public uint* Indices;
        public nuint NumIndices;

        public ulong Material;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Material
    {
        public Vector4 AlbedoColor;
        public nuint AlbedoTexture;

        public nuint NormalTexture;

        public float Metallic;
        public nuint MetallicTexture;

        public float Roughness;
        public nuint RoughnessTexture;

        public nuint OcclusionTexture;

        public nuint EmissiveTexture;

        public AlphaMode AlphaMode;
        public float AlphaCutoff;

        public bool DoubleSided;
    }

    public enum ImageDataType
    {
        Unknown,
        Png,
        Jpg,
        Bmp,
        Dds
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Image
    {
        public sbyte* Path;

        public ImageDataType DataType;
        public byte* Data;
        public nuint DataLength;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Scene
    {
        public Mesh* Meshes;
        public nuint NumMeshes;

        public Material* Materials;
        public nuint NumMaterials;

        public Image* Images;
        public nuint NumImages;
    }

    [DllImport(LibName, EntryPoint = "mdLoad")]
    public static extern void Load(sbyte* path, uint flags, Scene** scene);
    
    [DllImport(LibName, EntryPoint = "mdFree")]
    public static extern void Free(Scene* scene);
}