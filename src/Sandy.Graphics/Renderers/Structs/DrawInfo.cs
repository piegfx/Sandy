using System.Numerics;
using System.Runtime.InteropServices;

namespace Sandy.Graphics.Renderers.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct DrawInfo
{
    public Matrix4x4 WorldMatrix;
}