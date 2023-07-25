using System.Numerics;
using Sandy.Math;

namespace Sandy.Graphics.Structs;

public struct CameraInfo
{
    public Matrix4x4 Projection;
    public Matrix4x4 View;

    public Vector3 Position;

    public Color? ClearColor;
    
    public Rectangle<float> Viewport;

    public CameraInfo(Matrix4x4 projection, Matrix4x4 view, Vector3 position, Color? clearColor, Rectangle<float> viewport)
    {
        Projection = projection;
        View = view;
        
        Position = position;

        ClearColor = clearColor;

        Viewport = viewport;
    }
}