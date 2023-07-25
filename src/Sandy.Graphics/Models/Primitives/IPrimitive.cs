namespace Sandy.Graphics.Models.Primitives;

public interface IPrimitive
{
    public VertexPositionTextureColorNormalTangent[] Vertices { get; }
    
    public uint[] Indices { get; }
}