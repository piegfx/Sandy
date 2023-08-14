using System.Numerics;
using Sandy.Math;

namespace Sandy.Graphics.Structs;

public struct CameraInfo
{
    public Matrix4x4 Projection;
    public Matrix4x4 View;

    public Vector3 Position;
    
    public Rectangle<float> Viewport;

    public Color? ClearColor;
    public Skybox Skybox;
    

    public CameraInfo(Matrix4x4 projection, Matrix4x4 view, Vector3 position, Rectangle<float> viewport, Color? clearColor, Skybox skybox)
    {
        Projection = projection;
        View = view;

        Position = position;

        Viewport = viewport;
        
        ClearColor = clearColor;
        Skybox = skybox;
    }
}