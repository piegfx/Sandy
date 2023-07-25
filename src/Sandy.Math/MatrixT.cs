using System.Numerics;
using System.Runtime.InteropServices;

namespace Sandy.Math;

[StructLayout(LayoutKind.Sequential)]
public struct MatrixT<T> where T : INumber<T>
{
    public static MatrixT<T> Identity =>
        new MatrixT<T>(Vector4T<T>.UnitX, Vector4T<T>.UnitY, Vector4T<T>.UnitZ, Vector4T<T>.UnitW);
    
    public Vector4T<T> Row0;
    public Vector4T<T> Row1;
    public Vector4T<T> Row2;
    public Vector4T<T> Row3;

    public Vector4T<T> Column0
    {
        get => new Vector4T<T>(Row0.X, Row1.X, Row2.X, Row3.X);
        set
        {
            Row0.X = value.X;
            Row1.X = value.Y;
            Row2.X = value.Z;
            Row3.X = value.W;
        }
    }
    
    public Vector4T<T> Column1
    {
        get => new Vector4T<T>(Row0.Y, Row1.Y, Row2.Y, Row3.Y);
        set
        {
            Row0.Y = value.X;
            Row1.Y = value.Y;
            Row2.Y = value.Z;
            Row3.Y = value.W;
        }
    }
    
    public Vector4T<T> Column2
    {
        get => new Vector4T<T>(Row0.Z, Row1.Z, Row2.Z, Row3.Z);
        set
        {
            Row0.Z = value.X;
            Row1.Z = value.Y;
            Row2.Z = value.Z;
            Row3.Z = value.W;
        }
    }
    
    public Vector4T<T> Column3
    {
        get => new Vector4T<T>(Row0.W, Row1.W, Row2.W, Row3.W);
        set
        {
            Row0.W = value.X;
            Row1.W = value.Y;
            Row2.W = value.Z;
            Row3.W = value.W;
        }
    }

    public MatrixT(Vector4T<T> row0, Vector4T<T> row1, Vector4T<T> row2, Vector4T<T> row3)
    {
        Row0 = row0;
        Row1 = row1;
        Row2 = row2;
        Row3 = row3;
    }

    /*public static MatrixT<T> operator *(MatrixT<T> left, MatrixT<T> right)
    {
        
    }*/
}

public static class MatrixT
{
    
}