using System.Numerics;
using System.Runtime.InteropServices;

namespace Sandy.Math;

[StructLayout(LayoutKind.Sequential)]
public struct QuaternionT<T> where T : INumber<T>
{
    public static QuaternionT<T> Identity => new QuaternionT<T>(T.Zero, T.Zero, T.Zero, T.One);

    public static QuaternionT<T> Zero => new QuaternionT<T>(T.Zero, T.Zero, T.Zero, T.Zero);

    public T X;
    public T Y;
    public T Z;
    public T W;

    public QuaternionT(T x, T y, T z, T w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }
}