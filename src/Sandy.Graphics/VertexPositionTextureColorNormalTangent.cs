using System.Numerics;
using System.Runtime.InteropServices;
using Sandy.Math;

namespace Sandy.Graphics;

[StructLayout(LayoutKind.Sequential)]
public struct VertexPositionTextureColorNormalTangent
{
    public Vector3 Position;
    public Vector2 TexCoord;
    public Color Color;
    public Vector3 Normal;
    public Vector3 Tangent;

    public VertexPositionTextureColorNormalTangent(Vector3 position, Vector2 texCoord, Color color, Vector3 normal, Vector3 tangent)
    {
        Position = position;
        TexCoord = texCoord;
        Color = color;
        Normal = normal;
        Tangent = tangent;
    }

    public const uint SizeInBytes = 60;
}