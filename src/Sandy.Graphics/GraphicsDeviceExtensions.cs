using Pie;
using Sandy.Math;

namespace Sandy.Graphics;

public static class GraphicsDeviceExtensions
{
    public static void ClearColorBuffer(this GraphicsDevice device, Color color)
    {
        device.ClearColorBuffer(color.R, color.G, color.B, color.A);
    }
}