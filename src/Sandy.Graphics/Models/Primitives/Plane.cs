using System.Numerics;
using Sandy.Math;

namespace Sandy.Graphics.Models.Primitives;

public class Plane : IPrimitive
{
    public VertexPositionTextureColorNormalTangent[] Vertices { get; }
    
    public uint[] Indices { get; }

    public Plane()
    {
        Vertices = new []
        {
            new VertexPositionTextureColorNormalTangent(new Vector3(-0.5f,  0.5f, 0.0f), new Vector2(0, 0), Color.White, -Vector3.UnitZ, Vector3.Zero),
            new VertexPositionTextureColorNormalTangent(new Vector3( 0.5f,  0.5f, 0.0f), new Vector2(1, 0), Color.White, -Vector3.UnitZ, Vector3.Zero),
            new VertexPositionTextureColorNormalTangent(new Vector3( 0.5f, -0.5f, 0.0f), new Vector2(1, 1), Color.White, -Vector3.UnitZ, Vector3.Zero),
            new VertexPositionTextureColorNormalTangent(new Vector3(-0.5f, -0.5f, 0.0f), new Vector2(0, 1), Color.White, -Vector3.UnitZ, Vector3.Zero),
        };

        Indices = new uint[]
        {
            0, 1, 3,
            1, 2, 3
        };
    }
}