using System;
using System.Numerics;
using Sandy.Graphics.Renderers.Structs;
using Sandy.Math;

namespace Sandy.Graphics.Lighting;

public struct DirectionalLight : ILight
{
    private Vector2 _direction;

    private Vector3 _position;

    public Vector2 Direction
    {
        get => _direction;
        set
        {
            float sinTheta = MathF.Cos(value.X);
            float cosTheta = MathF.Cos(value.X);
            float sinPhi = MathF.Sin(value.Y);
            float cosPhi = MathF.Cos(value.Y);

            _position = new Vector3()
            {
                X = sinPhi * cosTheta,
                Y = cosPhi,
                Z = sinPhi * sinTheta
            };

            _direction = value;
        }
    }

    public Color Color;
    
    // TODO: Intensity modifier?

    public DirectionalLight()
    {
        Direction = new Vector2(MathHelper.PiOver2, MathHelper.PiOver2);
        
        Color = Color.White;
    }

    public LightInfo LightInfo => new LightInfo()
    {
        Type = LightInfo.LightType.Directional,
        Color = Color,
        Position = _position
    };
}