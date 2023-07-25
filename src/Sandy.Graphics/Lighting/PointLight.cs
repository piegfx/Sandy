using System.Numerics;
using Sandy.Graphics.Renderers.Structs;
using Sandy.Math;

namespace Sandy.Graphics.Lighting;

public struct PointLight : ILight
{
    public Vector3 Position;

    public Color Color;

    // TODO: Replace these with a general range + intensity value instead.
    public float Constant;

    public float Linear;

    public float Quadratic;

    public PointLight()
    {
        Position = Vector3.Zero;
        Color = Color.White;

        Constant = 1.0f;
        Linear = 0.14f;
        Quadratic = 0.07f;
    }

    public LightInfo LightInfo => new LightInfo()
    {
        Type = LightInfo.LightType.Point
    };
}