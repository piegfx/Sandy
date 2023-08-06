using System.Numerics;
using System.Runtime.InteropServices;
using Sandy.Math;

namespace Sandy.Graphics.Renderers.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct LightInfo
{
    public Color Color;

    public Vector3 Position;
    
    public LightType Type;

    //public float Constant;

    //public float Linear;

    //public float Quadratic;

    //private float _padding;

    public enum LightType : uint
    {
        None,
        Directional,
        Point
    }
}