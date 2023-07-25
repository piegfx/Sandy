using System;
using System.Numerics;
using Sandy.Graphics.Renderers.Structs;
using Sandy.Math;

namespace Sandy.Graphics.Lighting;

public struct DirectionalLight : ILight
{
    private Quaternion _direction;

    private Vector3 _forward;

    public Quaternion Direction
    {
        get => _direction;
        set
        {
            _forward = Vector3.Transform(-Vector3.UnitZ, value);

            _direction = value;
        }
    }

    public Color Color;
    
    // TODO: Intensity modifier?

    public DirectionalLight()
    {
        Direction = Quaternion.CreateFromAxisAngle(Vector3.UnitX, -MathHelper.PiOver2);
        
        Color = Color.White;
    }

    public LightInfo LightInfo => new LightInfo()
    {
        Type = LightInfo.LightType.Directional,
        Color = Color,
        Position = _forward
    };
}