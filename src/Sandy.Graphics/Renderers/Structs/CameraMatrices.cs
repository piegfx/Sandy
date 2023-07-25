using System.Numerics;
using System.Runtime.InteropServices;

namespace Sandy.Graphics.Renderers.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct CameraMatrices
{
    public Matrix4x4 Projection;
    public Matrix4x4 View;

    // Must be a Vector4 to appease the padding requirements.
    // Could I add a spare dummy float at the bottom?
    // Yes!
    // Do I instead want to make this as awkward as possible for the user?
    // Yes!
    public Vector4 Position;
}