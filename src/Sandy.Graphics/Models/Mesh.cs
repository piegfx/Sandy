using Sandy.Graphics.Materials;

namespace Sandy.Graphics.Models;

public class Mesh
{
    public VertexPositionTextureColorNormalTangent[] Vertices;

    public uint[] Indices;

    public Material Material;

    public Mesh(VertexPositionTextureColorNormalTangent[] vertices, uint[] indices, Material material)
    {
        Vertices = vertices;
        Indices = indices;
        Material = material;
    }
}