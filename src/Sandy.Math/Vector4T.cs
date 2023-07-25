using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sandy.Math;

[StructLayout(LayoutKind.Sequential)]
public struct Vector4T<T> : IEquatable<Vector4T<T>> where T : INumber<T>
{
    public static Vector4T<T> Zero => new Vector4T<T>(T.Zero, T.Zero, T.Zero, T.Zero);

    public static Vector4T<T> One => new Vector4T<T>(T.One, T.One, T.One, T.Zero);

    public static Vector4T<T> UnitX => new Vector4T<T>(T.One, T.Zero, T.Zero, T.Zero);

    public static Vector4T<T> UnitY => new Vector4T<T>(T.Zero, T.One, T.Zero, T.Zero);

    public static Vector4T<T> UnitZ => new Vector4T<T>(T.Zero, T.Zero, T.One, T.Zero);

    public static Vector4T<T> UnitW => new Vector4T<T>(T.Zero, T.Zero, T.Zero, T.One);

    public T X;
    public T Y;
    public T Z;
    public T W;

    public Vector4T(T scalar)
    {
        X = scalar;
        Y = scalar;
        Z = scalar;
        W = scalar;
    }

    public Vector4T(T x, T y, T z, T w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public Vector4T(Vector2T<T> xy, T z, T w)
    {
        X = xy.X;
        Y = xy.Y;
        Z = z;
        W = w;
    }

    public Vector4T(Vector3T<T> xyz, T w)
    {
        X = xyz.X;
        Y = xyz.Y;
        Z = xyz.Z;
        W = w;
    }

    public static Vector4T<T> operator +(Vector4T<T> left, Vector4T<T> right)
    {
        return new Vector4T<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    }

    public static Vector4T<T> operator -(Vector4T<T> left, Vector4T<T> right)
    {
        return new Vector4T<T>(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
    }
    
    public static Vector4T<T> operator -(Vector4T<T> vector)
    {
        return new Vector4T<T>(-vector.X, -vector.Y, -vector.Z, -vector.W);
    }

    public static Vector4T<T> operator *(Vector4T<T> left, Vector4T<T> right)
    {
        return new Vector4T<T>(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
    }

    public static Vector4T<T> operator *(Vector4T<T> left, T right)
    {
        return new Vector4T<T>(left.X * right, left.Y * right, left.Z * right, left.W * right);
    }

    public static Vector4T<T> operator *(T left, Vector4T<T> right)
    {
        return new Vector4T<T>(left * right.X, left * right.Y, left * right.Z, left * right.W);
    }

    public static Vector4T<T> operator /(Vector4T<T> left, Vector4T<T> right)
    {
        return new Vector4T<T>(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
    }

    public static Vector4T<T> operator /(Vector4T<T> left, T right)
    {
        return new Vector4T<T>(left.X / right, left.Y / right, left.Z / right, left.W / right);
    }

    public static bool operator ==(Vector4T<T> left, Vector4T<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector4T<T> left, Vector4T<T> right)
    {
        return !left.Equals(right);
    }

    public static explicit operator System.Numerics.Vector4(Vector4T<T> vector)
    {
        float x = Convert.ToSingle(vector.X);
        float y = Convert.ToSingle(vector.Y);
        float z = Convert.ToSingle(vector.Z);
        float w = Convert.ToSingle(vector.W);
        return new Vector4(x, y, z, w);
    }

    public static explicit operator Vector4T<T>(System.Numerics.Vector4 vector)
    {
        T x = T.CreateChecked(vector.X);
        T y = T.CreateChecked(vector.Y);
        T z = T.CreateChecked(vector.Z);
        T w = T.CreateChecked(vector.W);
        return new Vector4T<T>(x, y, z, w);
    }

    [MethodImpl(Vector4T.Options)]
    public readonly Vector4T<TOther> As<TOther>() where TOther : INumber<TOther>
    {
        TOther x = TOther.CreateChecked(X);
        TOther y = TOther.CreateChecked(Y);
        TOther z = TOther.CreateChecked(Z);
        TOther w = TOther.CreateChecked(W);
        return new Vector4T<TOther>(x, y, z, w);
    }

    public override string ToString()
    {
        return $"Vector4T<{typeof(T).FullName}>(X: {X}, Y: {Y}, Z: {Z}, W: {W})";
    }

    public bool Equals(Vector4T<T> other)
    {
        return EqualityComparer<T>.Default.Equals(X, other.X) && EqualityComparer<T>.Default.Equals(Y, other.Y) &&
               EqualityComparer<T>.Default.Equals(Z, other.Z) && EqualityComparer<T>.Default.Equals(W, other.W);
    }

    public override bool Equals(object obj)
    {
        return obj is Vector4T<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z, W);
    }

    #region Swizzle

    public Vector2T<T> XX => new Vector2T<T>(X, X);

    public Vector2T<T> XY => new Vector2T<T>(X, Y);

    public Vector2T<T> XZ => new Vector2T<T>(X, Z);

    public Vector2T<T> XW => new Vector2T<T>(X, W);

    public Vector2T<T> YX => new Vector2T<T>(Y, X);

    public Vector2T<T> YY => new Vector2T<T>(Y, Y);

    public Vector2T<T> YZ => new Vector2T<T>(Y, Z);

    public Vector2T<T> YW => new Vector2T<T>(Y, W);

    public Vector2T<T> ZX => new Vector2T<T>(Z, X);

    public Vector2T<T> ZY => new Vector2T<T>(Z, Y);

    public Vector2T<T> ZZ => new Vector2T<T>(Z, Z);

    public Vector2T<T> ZW => new Vector2T<T>(Z, W);

    public Vector2T<T> WX => new Vector2T<T>(W, X);

    public Vector2T<T> WY => new Vector2T<T>(W, Y);

    public Vector2T<T> WZ => new Vector2T<T>(W, Z);

    public Vector2T<T> WW => new Vector2T<T>(W, W);

    public Vector3T<T> XXX => new Vector3T<T>(X, X, X);

    public Vector3T<T> XXY => new Vector3T<T>(X, X, Y);

    public Vector3T<T> XXZ => new Vector3T<T>(X, X, Z);

    public Vector3T<T> XXW => new Vector3T<T>(X, X, W);

    public Vector3T<T> XYX => new Vector3T<T>(X, Y, X);

    public Vector3T<T> XYY => new Vector3T<T>(X, Y, Y);

    public Vector3T<T> XYZ => new Vector3T<T>(X, Y, Z);

    public Vector3T<T> XYW => new Vector3T<T>(X, Y, W);

    public Vector3T<T> XZX => new Vector3T<T>(X, Z, X);

    public Vector3T<T> XZY => new Vector3T<T>(X, Z, Y);

    public Vector3T<T> XZZ => new Vector3T<T>(X, Z, Z);

    public Vector3T<T> XZW => new Vector3T<T>(X, Z, W);

    public Vector3T<T> XWX => new Vector3T<T>(X, W, X);

    public Vector3T<T> XWY => new Vector3T<T>(X, W, Y);

    public Vector3T<T> XWZ => new Vector3T<T>(X, W, Z);

    public Vector3T<T> XWW => new Vector3T<T>(X, W, W);

    public Vector3T<T> YXX => new Vector3T<T>(Y, X, X);

    public Vector3T<T> YXY => new Vector3T<T>(Y, X, Y);

    public Vector3T<T> YXZ => new Vector3T<T>(Y, X, Z);

    public Vector3T<T> YXW => new Vector3T<T>(Y, X, W);

    public Vector3T<T> YYX => new Vector3T<T>(Y, Y, X);

    public Vector3T<T> YYY => new Vector3T<T>(Y, Y, Y);

    public Vector3T<T> YYZ => new Vector3T<T>(Y, Y, Z);

    public Vector3T<T> YYW => new Vector3T<T>(Y, Y, W);

    public Vector3T<T> YZX => new Vector3T<T>(Y, Z, X);

    public Vector3T<T> YZY => new Vector3T<T>(Y, Z, Y);

    public Vector3T<T> YZZ => new Vector3T<T>(Y, Z, Z);

    public Vector3T<T> YZW => new Vector3T<T>(Y, Z, W);

    public Vector3T<T> YWX => new Vector3T<T>(Y, W, X);

    public Vector3T<T> YWY => new Vector3T<T>(Y, W, Y);

    public Vector3T<T> YWZ => new Vector3T<T>(Y, W, Z);

    public Vector3T<T> YWW => new Vector3T<T>(Y, W, W);

    public Vector3T<T> ZXX => new Vector3T<T>(Z, X, X);

    public Vector3T<T> ZXY => new Vector3T<T>(Z, X, Y);

    public Vector3T<T> ZXZ => new Vector3T<T>(Z, X, Z);

    public Vector3T<T> ZXW => new Vector3T<T>(Z, X, W);

    public Vector3T<T> ZYX => new Vector3T<T>(Z, Y, X);

    public Vector3T<T> ZYY => new Vector3T<T>(Z, Y, Y);

    public Vector3T<T> ZYZ => new Vector3T<T>(Z, Y, Z);

    public Vector3T<T> ZYW => new Vector3T<T>(Z, Y, W);

    public Vector3T<T> ZZX => new Vector3T<T>(Z, Z, X);

    public Vector3T<T> ZZY => new Vector3T<T>(Z, Z, Y);

    public Vector3T<T> ZZZ => new Vector3T<T>(Z, Z, Z);

    public Vector3T<T> ZZW => new Vector3T<T>(Z, Z, W);

    public Vector3T<T> ZWX => new Vector3T<T>(Z, W, X);

    public Vector3T<T> ZWY => new Vector3T<T>(Z, W, Y);

    public Vector3T<T> ZWZ => new Vector3T<T>(Z, W, Z);

    public Vector3T<T> ZWW => new Vector3T<T>(Z, W, W);

    public Vector3T<T> WXX => new Vector3T<T>(W, X, X);

    public Vector3T<T> WXY => new Vector3T<T>(W, X, Y);

    public Vector3T<T> WXZ => new Vector3T<T>(W, X, Z);

    public Vector3T<T> WXW => new Vector3T<T>(W, X, W);

    public Vector3T<T> WYX => new Vector3T<T>(W, Y, X);

    public Vector3T<T> WYY => new Vector3T<T>(W, Y, Y);

    public Vector3T<T> WYZ => new Vector3T<T>(W, Y, Z);

    public Vector3T<T> WYW => new Vector3T<T>(W, Y, W);

    public Vector3T<T> WZX => new Vector3T<T>(W, Z, X);

    public Vector3T<T> WZY => new Vector3T<T>(W, Z, Y);

    public Vector3T<T> WZZ => new Vector3T<T>(W, Z, Z);

    public Vector3T<T> WZW => new Vector3T<T>(W, Z, W);

    public Vector3T<T> WWX => new Vector3T<T>(W, W, X);

    public Vector3T<T> WWY => new Vector3T<T>(W, W, Y);

    public Vector3T<T> WWZ => new Vector3T<T>(W, W, Z);

    public Vector3T<T> WWW => new Vector3T<T>(W, W, W);

    public Vector4T<T> XXXX => new Vector4T<T>(X, X, X, X);

    public Vector4T<T> XXXY => new Vector4T<T>(X, X, X, Y);

    public Vector4T<T> XXXZ => new Vector4T<T>(X, X, X, Z);

    public Vector4T<T> XXXW => new Vector4T<T>(X, X, X, W);

    public Vector4T<T> XXYX => new Vector4T<T>(X, X, Y, X);

    public Vector4T<T> XXYY => new Vector4T<T>(X, X, Y, Y);

    public Vector4T<T> XXYZ => new Vector4T<T>(X, X, Y, Z);

    public Vector4T<T> XXYW => new Vector4T<T>(X, X, Y, W);

    public Vector4T<T> XXZX => new Vector4T<T>(X, X, Z, X);

    public Vector4T<T> XXZY => new Vector4T<T>(X, X, Z, Y);

    public Vector4T<T> XXZZ => new Vector4T<T>(X, X, Z, Z);

    public Vector4T<T> XXZW => new Vector4T<T>(X, X, Z, W);

    public Vector4T<T> XXWX => new Vector4T<T>(X, X, W, X);

    public Vector4T<T> XXWY => new Vector4T<T>(X, X, W, Y);

    public Vector4T<T> XXWZ => new Vector4T<T>(X, X, W, Z);

    public Vector4T<T> XXWW => new Vector4T<T>(X, X, W, W);

    public Vector4T<T> XYXX => new Vector4T<T>(X, Y, X, X);

    public Vector4T<T> XYXY => new Vector4T<T>(X, Y, X, Y);

    public Vector4T<T> XYXZ => new Vector4T<T>(X, Y, X, Z);

    public Vector4T<T> XYXW => new Vector4T<T>(X, Y, X, W);

    public Vector4T<T> XYYX => new Vector4T<T>(X, Y, Y, X);

    public Vector4T<T> XYYY => new Vector4T<T>(X, Y, Y, Y);

    public Vector4T<T> XYYZ => new Vector4T<T>(X, Y, Y, Z);

    public Vector4T<T> XYYW => new Vector4T<T>(X, Y, Y, W);

    public Vector4T<T> XYZX => new Vector4T<T>(X, Y, Z, X);

    public Vector4T<T> XYZY => new Vector4T<T>(X, Y, Z, Y);

    public Vector4T<T> XYZZ => new Vector4T<T>(X, Y, Z, Z);

    public Vector4T<T> XYZW => new Vector4T<T>(X, Y, Z, W);

    public Vector4T<T> XYWX => new Vector4T<T>(X, Y, W, X);

    public Vector4T<T> XYWY => new Vector4T<T>(X, Y, W, Y);

    public Vector4T<T> XYWZ => new Vector4T<T>(X, Y, W, Z);

    public Vector4T<T> XYWW => new Vector4T<T>(X, Y, W, W);

    public Vector4T<T> XZXX => new Vector4T<T>(X, Z, X, X);

    public Vector4T<T> XZXY => new Vector4T<T>(X, Z, X, Y);

    public Vector4T<T> XZXZ => new Vector4T<T>(X, Z, X, Z);

    public Vector4T<T> XZXW => new Vector4T<T>(X, Z, X, W);

    public Vector4T<T> XZYX => new Vector4T<T>(X, Z, Y, X);

    public Vector4T<T> XZYY => new Vector4T<T>(X, Z, Y, Y);

    public Vector4T<T> XZYZ => new Vector4T<T>(X, Z, Y, Z);

    public Vector4T<T> XZYW => new Vector4T<T>(X, Z, Y, W);

    public Vector4T<T> XZZX => new Vector4T<T>(X, Z, Z, X);

    public Vector4T<T> XZZY => new Vector4T<T>(X, Z, Z, Y);

    public Vector4T<T> XZZZ => new Vector4T<T>(X, Z, Z, Z);

    public Vector4T<T> XZZW => new Vector4T<T>(X, Z, Z, W);

    public Vector4T<T> XZWX => new Vector4T<T>(X, Z, W, X);

    public Vector4T<T> XZWY => new Vector4T<T>(X, Z, W, Y);

    public Vector4T<T> XZWZ => new Vector4T<T>(X, Z, W, Z);

    public Vector4T<T> XZWW => new Vector4T<T>(X, Z, W, W);

    public Vector4T<T> XWXX => new Vector4T<T>(X, W, X, X);

    public Vector4T<T> XWXY => new Vector4T<T>(X, W, X, Y);

    public Vector4T<T> XWXZ => new Vector4T<T>(X, W, X, Z);

    public Vector4T<T> XWXW => new Vector4T<T>(X, W, X, W);

    public Vector4T<T> XWYX => new Vector4T<T>(X, W, Y, X);

    public Vector4T<T> XWYY => new Vector4T<T>(X, W, Y, Y);

    public Vector4T<T> XWYZ => new Vector4T<T>(X, W, Y, Z);

    public Vector4T<T> XWYW => new Vector4T<T>(X, W, Y, W);

    public Vector4T<T> XWZX => new Vector4T<T>(X, W, Z, X);

    public Vector4T<T> XWZY => new Vector4T<T>(X, W, Z, Y);

    public Vector4T<T> XWZZ => new Vector4T<T>(X, W, Z, Z);

    public Vector4T<T> XWZW => new Vector4T<T>(X, W, Z, W);

    public Vector4T<T> XWWX => new Vector4T<T>(X, W, W, X);

    public Vector4T<T> XWWY => new Vector4T<T>(X, W, W, Y);

    public Vector4T<T> XWWZ => new Vector4T<T>(X, W, W, Z);

    public Vector4T<T> XWWW => new Vector4T<T>(X, W, W, W);

    public Vector4T<T> YXXX => new Vector4T<T>(Y, X, X, X);

    public Vector4T<T> YXXY => new Vector4T<T>(Y, X, X, Y);

    public Vector4T<T> YXXZ => new Vector4T<T>(Y, X, X, Z);

    public Vector4T<T> YXXW => new Vector4T<T>(Y, X, X, W);

    public Vector4T<T> YXYX => new Vector4T<T>(Y, X, Y, X);

    public Vector4T<T> YXYY => new Vector4T<T>(Y, X, Y, Y);

    public Vector4T<T> YXYZ => new Vector4T<T>(Y, X, Y, Z);

    public Vector4T<T> YXYW => new Vector4T<T>(Y, X, Y, W);

    public Vector4T<T> YXZX => new Vector4T<T>(Y, X, Z, X);

    public Vector4T<T> YXZY => new Vector4T<T>(Y, X, Z, Y);

    public Vector4T<T> YXZZ => new Vector4T<T>(Y, X, Z, Z);

    public Vector4T<T> YXZW => new Vector4T<T>(Y, X, Z, W);

    public Vector4T<T> YXWX => new Vector4T<T>(Y, X, W, X);

    public Vector4T<T> YXWY => new Vector4T<T>(Y, X, W, Y);

    public Vector4T<T> YXWZ => new Vector4T<T>(Y, X, W, Z);

    public Vector4T<T> YXWW => new Vector4T<T>(Y, X, W, W);

    public Vector4T<T> YYXX => new Vector4T<T>(Y, Y, X, X);

    public Vector4T<T> YYXY => new Vector4T<T>(Y, Y, X, Y);

    public Vector4T<T> YYXZ => new Vector4T<T>(Y, Y, X, Z);

    public Vector4T<T> YYXW => new Vector4T<T>(Y, Y, X, W);

    public Vector4T<T> YYYX => new Vector4T<T>(Y, Y, Y, X);

    public Vector4T<T> YYYY => new Vector4T<T>(Y, Y, Y, Y);

    public Vector4T<T> YYYZ => new Vector4T<T>(Y, Y, Y, Z);

    public Vector4T<T> YYYW => new Vector4T<T>(Y, Y, Y, W);

    public Vector4T<T> YYZX => new Vector4T<T>(Y, Y, Z, X);

    public Vector4T<T> YYZY => new Vector4T<T>(Y, Y, Z, Y);

    public Vector4T<T> YYZZ => new Vector4T<T>(Y, Y, Z, Z);

    public Vector4T<T> YYZW => new Vector4T<T>(Y, Y, Z, W);

    public Vector4T<T> YYWX => new Vector4T<T>(Y, Y, W, X);

    public Vector4T<T> YYWY => new Vector4T<T>(Y, Y, W, Y);

    public Vector4T<T> YYWZ => new Vector4T<T>(Y, Y, W, Z);

    public Vector4T<T> YYWW => new Vector4T<T>(Y, Y, W, W);

    public Vector4T<T> YZXX => new Vector4T<T>(Y, Z, X, X);

    public Vector4T<T> YZXY => new Vector4T<T>(Y, Z, X, Y);

    public Vector4T<T> YZXZ => new Vector4T<T>(Y, Z, X, Z);

    public Vector4T<T> YZXW => new Vector4T<T>(Y, Z, X, W);

    public Vector4T<T> YZYX => new Vector4T<T>(Y, Z, Y, X);

    public Vector4T<T> YZYY => new Vector4T<T>(Y, Z, Y, Y);

    public Vector4T<T> YZYZ => new Vector4T<T>(Y, Z, Y, Z);

    public Vector4T<T> YZYW => new Vector4T<T>(Y, Z, Y, W);

    public Vector4T<T> YZZX => new Vector4T<T>(Y, Z, Z, X);

    public Vector4T<T> YZZY => new Vector4T<T>(Y, Z, Z, Y);

    public Vector4T<T> YZZZ => new Vector4T<T>(Y, Z, Z, Z);

    public Vector4T<T> YZZW => new Vector4T<T>(Y, Z, Z, W);

    public Vector4T<T> YZWX => new Vector4T<T>(Y, Z, W, X);

    public Vector4T<T> YZWY => new Vector4T<T>(Y, Z, W, Y);

    public Vector4T<T> YZWZ => new Vector4T<T>(Y, Z, W, Z);

    public Vector4T<T> YZWW => new Vector4T<T>(Y, Z, W, W);

    public Vector4T<T> YWXX => new Vector4T<T>(Y, W, X, X);

    public Vector4T<T> YWXY => new Vector4T<T>(Y, W, X, Y);

    public Vector4T<T> YWXZ => new Vector4T<T>(Y, W, X, Z);

    public Vector4T<T> YWXW => new Vector4T<T>(Y, W, X, W);

    public Vector4T<T> YWYX => new Vector4T<T>(Y, W, Y, X);

    public Vector4T<T> YWYY => new Vector4T<T>(Y, W, Y, Y);

    public Vector4T<T> YWYZ => new Vector4T<T>(Y, W, Y, Z);

    public Vector4T<T> YWYW => new Vector4T<T>(Y, W, Y, W);

    public Vector4T<T> YWZX => new Vector4T<T>(Y, W, Z, X);

    public Vector4T<T> YWZY => new Vector4T<T>(Y, W, Z, Y);

    public Vector4T<T> YWZZ => new Vector4T<T>(Y, W, Z, Z);

    public Vector4T<T> YWZW => new Vector4T<T>(Y, W, Z, W);

    public Vector4T<T> YWWX => new Vector4T<T>(Y, W, W, X);

    public Vector4T<T> YWWY => new Vector4T<T>(Y, W, W, Y);

    public Vector4T<T> YWWZ => new Vector4T<T>(Y, W, W, Z);

    public Vector4T<T> YWWW => new Vector4T<T>(Y, W, W, W);

    public Vector4T<T> ZXXX => new Vector4T<T>(Z, X, X, X);

    public Vector4T<T> ZXXY => new Vector4T<T>(Z, X, X, Y);

    public Vector4T<T> ZXXZ => new Vector4T<T>(Z, X, X, Z);

    public Vector4T<T> ZXXW => new Vector4T<T>(Z, X, X, W);

    public Vector4T<T> ZXYX => new Vector4T<T>(Z, X, Y, X);

    public Vector4T<T> ZXYY => new Vector4T<T>(Z, X, Y, Y);

    public Vector4T<T> ZXYZ => new Vector4T<T>(Z, X, Y, Z);

    public Vector4T<T> ZXYW => new Vector4T<T>(Z, X, Y, W);

    public Vector4T<T> ZXZX => new Vector4T<T>(Z, X, Z, X);

    public Vector4T<T> ZXZY => new Vector4T<T>(Z, X, Z, Y);

    public Vector4T<T> ZXZZ => new Vector4T<T>(Z, X, Z, Z);

    public Vector4T<T> ZXZW => new Vector4T<T>(Z, X, Z, W);

    public Vector4T<T> ZXWX => new Vector4T<T>(Z, X, W, X);

    public Vector4T<T> ZXWY => new Vector4T<T>(Z, X, W, Y);

    public Vector4T<T> ZXWZ => new Vector4T<T>(Z, X, W, Z);

    public Vector4T<T> ZXWW => new Vector4T<T>(Z, X, W, W);

    public Vector4T<T> ZYXX => new Vector4T<T>(Z, Y, X, X);

    public Vector4T<T> ZYXY => new Vector4T<T>(Z, Y, X, Y);

    public Vector4T<T> ZYXZ => new Vector4T<T>(Z, Y, X, Z);

    public Vector4T<T> ZYXW => new Vector4T<T>(Z, Y, X, W);

    public Vector4T<T> ZYYX => new Vector4T<T>(Z, Y, Y, X);

    public Vector4T<T> ZYYY => new Vector4T<T>(Z, Y, Y, Y);

    public Vector4T<T> ZYYZ => new Vector4T<T>(Z, Y, Y, Z);

    public Vector4T<T> ZYYW => new Vector4T<T>(Z, Y, Y, W);

    public Vector4T<T> ZYZX => new Vector4T<T>(Z, Y, Z, X);

    public Vector4T<T> ZYZY => new Vector4T<T>(Z, Y, Z, Y);

    public Vector4T<T> ZYZZ => new Vector4T<T>(Z, Y, Z, Z);

    public Vector4T<T> ZYZW => new Vector4T<T>(Z, Y, Z, W);

    public Vector4T<T> ZYWX => new Vector4T<T>(Z, Y, W, X);

    public Vector4T<T> ZYWY => new Vector4T<T>(Z, Y, W, Y);

    public Vector4T<T> ZYWZ => new Vector4T<T>(Z, Y, W, Z);

    public Vector4T<T> ZYWW => new Vector4T<T>(Z, Y, W, W);

    public Vector4T<T> ZZXX => new Vector4T<T>(Z, Z, X, X);

    public Vector4T<T> ZZXY => new Vector4T<T>(Z, Z, X, Y);

    public Vector4T<T> ZZXZ => new Vector4T<T>(Z, Z, X, Z);

    public Vector4T<T> ZZXW => new Vector4T<T>(Z, Z, X, W);

    public Vector4T<T> ZZYX => new Vector4T<T>(Z, Z, Y, X);

    public Vector4T<T> ZZYY => new Vector4T<T>(Z, Z, Y, Y);

    public Vector4T<T> ZZYZ => new Vector4T<T>(Z, Z, Y, Z);

    public Vector4T<T> ZZYW => new Vector4T<T>(Z, Z, Y, W);

    public Vector4T<T> ZZZX => new Vector4T<T>(Z, Z, Z, X);

    public Vector4T<T> ZZZY => new Vector4T<T>(Z, Z, Z, Y);

    public Vector4T<T> ZZZZ => new Vector4T<T>(Z, Z, Z, Z);

    public Vector4T<T> ZZZW => new Vector4T<T>(Z, Z, Z, W);

    public Vector4T<T> ZZWX => new Vector4T<T>(Z, Z, W, X);

    public Vector4T<T> ZZWY => new Vector4T<T>(Z, Z, W, Y);

    public Vector4T<T> ZZWZ => new Vector4T<T>(Z, Z, W, Z);

    public Vector4T<T> ZZWW => new Vector4T<T>(Z, Z, W, W);

    public Vector4T<T> ZWXX => new Vector4T<T>(Z, W, X, X);

    public Vector4T<T> ZWXY => new Vector4T<T>(Z, W, X, Y);

    public Vector4T<T> ZWXZ => new Vector4T<T>(Z, W, X, Z);

    public Vector4T<T> ZWXW => new Vector4T<T>(Z, W, X, W);

    public Vector4T<T> ZWYX => new Vector4T<T>(Z, W, Y, X);

    public Vector4T<T> ZWYY => new Vector4T<T>(Z, W, Y, Y);

    public Vector4T<T> ZWYZ => new Vector4T<T>(Z, W, Y, Z);

    public Vector4T<T> ZWYW => new Vector4T<T>(Z, W, Y, W);

    public Vector4T<T> ZWZX => new Vector4T<T>(Z, W, Z, X);

    public Vector4T<T> ZWZY => new Vector4T<T>(Z, W, Z, Y);

    public Vector4T<T> ZWZZ => new Vector4T<T>(Z, W, Z, Z);

    public Vector4T<T> ZWZW => new Vector4T<T>(Z, W, Z, W);

    public Vector4T<T> ZWWX => new Vector4T<T>(Z, W, W, X);

    public Vector4T<T> ZWWY => new Vector4T<T>(Z, W, W, Y);

    public Vector4T<T> ZWWZ => new Vector4T<T>(Z, W, W, Z);

    public Vector4T<T> ZWWW => new Vector4T<T>(Z, W, W, W);

    public Vector4T<T> WXXX => new Vector4T<T>(W, X, X, X);

    public Vector4T<T> WXXY => new Vector4T<T>(W, X, X, Y);

    public Vector4T<T> WXXZ => new Vector4T<T>(W, X, X, Z);

    public Vector4T<T> WXXW => new Vector4T<T>(W, X, X, W);

    public Vector4T<T> WXYX => new Vector4T<T>(W, X, Y, X);

    public Vector4T<T> WXYY => new Vector4T<T>(W, X, Y, Y);

    public Vector4T<T> WXYZ => new Vector4T<T>(W, X, Y, Z);

    public Vector4T<T> WXYW => new Vector4T<T>(W, X, Y, W);

    public Vector4T<T> WXZX => new Vector4T<T>(W, X, Z, X);

    public Vector4T<T> WXZY => new Vector4T<T>(W, X, Z, Y);

    public Vector4T<T> WXZZ => new Vector4T<T>(W, X, Z, Z);

    public Vector4T<T> WXZW => new Vector4T<T>(W, X, Z, W);

    public Vector4T<T> WXWX => new Vector4T<T>(W, X, W, X);

    public Vector4T<T> WXWY => new Vector4T<T>(W, X, W, Y);

    public Vector4T<T> WXWZ => new Vector4T<T>(W, X, W, Z);

    public Vector4T<T> WXWW => new Vector4T<T>(W, X, W, W);

    public Vector4T<T> WYXX => new Vector4T<T>(W, Y, X, X);

    public Vector4T<T> WYXY => new Vector4T<T>(W, Y, X, Y);

    public Vector4T<T> WYXZ => new Vector4T<T>(W, Y, X, Z);

    public Vector4T<T> WYXW => new Vector4T<T>(W, Y, X, W);

    public Vector4T<T> WYYX => new Vector4T<T>(W, Y, Y, X);

    public Vector4T<T> WYYY => new Vector4T<T>(W, Y, Y, Y);

    public Vector4T<T> WYYZ => new Vector4T<T>(W, Y, Y, Z);

    public Vector4T<T> WYYW => new Vector4T<T>(W, Y, Y, W);

    public Vector4T<T> WYZX => new Vector4T<T>(W, Y, Z, X);

    public Vector4T<T> WYZY => new Vector4T<T>(W, Y, Z, Y);

    public Vector4T<T> WYZZ => new Vector4T<T>(W, Y, Z, Z);

    public Vector4T<T> WYZW => new Vector4T<T>(W, Y, Z, W);

    public Vector4T<T> WYWX => new Vector4T<T>(W, Y, W, X);

    public Vector4T<T> WYWY => new Vector4T<T>(W, Y, W, Y);

    public Vector4T<T> WYWZ => new Vector4T<T>(W, Y, W, Z);

    public Vector4T<T> WYWW => new Vector4T<T>(W, Y, W, W);

    public Vector4T<T> WZXX => new Vector4T<T>(W, Z, X, X);

    public Vector4T<T> WZXY => new Vector4T<T>(W, Z, X, Y);

    public Vector4T<T> WZXZ => new Vector4T<T>(W, Z, X, Z);

    public Vector4T<T> WZXW => new Vector4T<T>(W, Z, X, W);

    public Vector4T<T> WZYX => new Vector4T<T>(W, Z, Y, X);

    public Vector4T<T> WZYY => new Vector4T<T>(W, Z, Y, Y);

    public Vector4T<T> WZYZ => new Vector4T<T>(W, Z, Y, Z);

    public Vector4T<T> WZYW => new Vector4T<T>(W, Z, Y, W);

    public Vector4T<T> WZZX => new Vector4T<T>(W, Z, Z, X);

    public Vector4T<T> WZZY => new Vector4T<T>(W, Z, Z, Y);

    public Vector4T<T> WZZZ => new Vector4T<T>(W, Z, Z, Z);

    public Vector4T<T> WZZW => new Vector4T<T>(W, Z, Z, W);

    public Vector4T<T> WZWX => new Vector4T<T>(W, Z, W, X);

    public Vector4T<T> WZWY => new Vector4T<T>(W, Z, W, Y);

    public Vector4T<T> WZWZ => new Vector4T<T>(W, Z, W, Z);

    public Vector4T<T> WZWW => new Vector4T<T>(W, Z, W, W);

    public Vector4T<T> WWXX => new Vector4T<T>(W, W, X, X);

    public Vector4T<T> WWXY => new Vector4T<T>(W, W, X, Y);

    public Vector4T<T> WWXZ => new Vector4T<T>(W, W, X, Z);

    public Vector4T<T> WWXW => new Vector4T<T>(W, W, X, W);

    public Vector4T<T> WWYX => new Vector4T<T>(W, W, Y, X);

    public Vector4T<T> WWYY => new Vector4T<T>(W, W, Y, Y);

    public Vector4T<T> WWYZ => new Vector4T<T>(W, W, Y, Z);

    public Vector4T<T> WWYW => new Vector4T<T>(W, W, Y, W);

    public Vector4T<T> WWZX => new Vector4T<T>(W, W, Z, X);

    public Vector4T<T> WWZY => new Vector4T<T>(W, W, Z, Y);

    public Vector4T<T> WWZZ => new Vector4T<T>(W, W, Z, Z);

    public Vector4T<T> WWZW => new Vector4T<T>(W, W, Z, W);

    public Vector4T<T> WWWX => new Vector4T<T>(W, W, W, X);

    public Vector4T<T> WWWY => new Vector4T<T>(W, W, W, Y);

    public Vector4T<T> WWWZ => new Vector4T<T>(W, W, W, Z);

    public Vector4T<T> WWWW => new Vector4T<T>(W, W, W, W);

    #endregion
}

public static class Vector4T
{
    internal const MethodImplOptions Options =
        MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;
    
    [MethodImpl(Options)]
    public static T MagnitudeSquared<T>(in Vector4T<T> vector) where T : INumber<T>
    {
        return Dot(vector, vector);
    }

    [MethodImpl(Options)]
    public static T Magnitude<T>(in Vector4T<T> vector) where T : INumber<T>, IRootFunctions<T>
    {
        return T.Sqrt(MagnitudeSquared(vector));
    }

    [MethodImpl(Options)]
    public static void Normalize<T>(ref Vector4T<T> vector) where T : INumber<T>, IRootFunctions<T>
    {
        T magnitude = Magnitude(vector);
        vector.X /= magnitude;
        vector.Y /= magnitude;
        vector.Z /= magnitude;
        vector.W /= magnitude;
    }

    [MethodImpl(Options)]
    public static Vector4T<T> Normalize<T>(Vector4T<T> vector) where T : INumber<T>, IRootFunctions<T>
    {
        Normalize(ref vector);
        return vector;
    }

    [MethodImpl(Options)]
    public static T Dot<T>(in Vector4T<T> a, in Vector4T<T> b) where T : INumber<T>
    {
        return T.CreateChecked(a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W);
    }

    [MethodImpl(Options)]
    public static Vector4T<T> Abs<T>(in Vector4T<T> vector) where T : INumber<T>
    {
        return new Vector4T<T>(T.Abs(vector.X), T.Abs(vector.Y), T.Abs(vector.Z), T.Abs(vector.W));
    }

    [MethodImpl(Options)]
    public static Vector4T<T> Clamp<T>(in Vector4T<T> vector, in Vector4T<T> min, in Vector4T<T> max)
        where T : INumber<T>
    {
        T x = T.Clamp(vector.X, min.X, max.X);
        T y = T.Clamp(vector.Y, min.Y, max.Y);
        T z = T.Clamp(vector.Z, min.Z, max.Z);
        T w = T.Clamp(vector.W, min.W, max.W);
        return new Vector4T<T>(x, y, z, w);
    }

    [MethodImpl(Options)]
    public static T DistanceSquared<T>(in Vector4T<T> a, in Vector4T<T> b) where T : INumber<T>
    {
        Vector4T<T> res = a - b;
        return Dot(res, res);
    }

    [MethodImpl(Options)]
    public static T Distance<T>(in Vector4T<T> a, in Vector4T<T> b) where T : INumber<T>, IRootFunctions<T>
    {
        return T.Sqrt(DistanceSquared(a, b));
    }

    [MethodImpl(Options)]
    public static Vector4T<T> Lerp<T>(in Vector4T<T> a, in Vector4T<T> b, T amount) where T : INumber<T>
    {
        T x = MathHelper.Lerp(a.X, b.X, amount);
        T y = MathHelper.Lerp(a.Y, b.Y, amount);
        T z = MathHelper.Lerp(a.Z, b.Z, amount);
        T w = MathHelper.Lerp(a.W, b.W, amount);
        return new Vector4T<T>(x, y, z, w);
    }

    [MethodImpl(Options)]
    public static Vector4T<T> Max<T>(in Vector4T<T> a, in Vector4T<T> b) where T : INumber<T>
    {
        T x = T.Max(a.X, b.X);
        T y = T.Max(a.Y, b.Y);
        T z = T.Max(a.Z, b.Z);
        T w = T.Max(a.W, b.W);
        return new Vector4T<T>(x, y, z, w);
    }

    [MethodImpl(Options)]
    public static Vector4T<T> Min<T>(in Vector4T<T> a, in Vector4T<T> b) where T : INumber<T>
    {
        T x = T.Min(a.X, b.X);
        T y = T.Min(a.Y, b.Y);
        T z = T.Min(a.Z, b.Z);
        T w = T.Min(a.W, b.W);
        return new Vector4T<T>(x, y, z, w);
    }

    [MethodImpl(Options)]
    public static Vector4T<T> Sqrt<T>(in Vector4T<T> vector) where T : INumber<T>, IRootFunctions<T>
    {
        T x = T.Sqrt(vector.X);
        T y = T.Sqrt(vector.Y);
        T z = T.Sqrt(vector.Z);
        T w = T.Sqrt(vector.W);
        return new Vector4T<T>(x, y, z, w);
    }

    [MethodImpl(Options)]
    public static Vector4T<T> Transform<T>(in Vector4T<T> vector, in MatrixT<T> matrix) where T : INumber<T>
    {
        throw new NotImplementedException();
    }

    [MethodImpl(Options)]
    public static Vector4T<T> Transform<T>(in Vector4T<T> vector, in QuaternionT<T> quaternion) where T : INumber<T>
    {
        throw new NotImplementedException();
    }
    
    [MethodImpl(Options)]
    public static Vector4T<T> TransformNormal<T>(in Vector4T<T> vector, in MatrixT<T> matrix) where T : INumber<T>
    {
        throw new NotImplementedException();
    }

    [MethodImpl(Options)]
    public static Vector4T<T> TransformNormal<T>(in Vector4T<T> vector, in QuaternionT<T> quaternion) where T : INumber<T>
    {
        throw new NotImplementedException();
    }
}

public static class Vector4TExtensions
{
    [MethodImpl(Vector4T.Options)]
    public static T LengthSquared<T>(this Vector4T<T> vector) where T : INumber<T>
    {
        return Vector4T.MagnitudeSquared(vector);
    }

    [MethodImpl(Vector4T.Options)]
    public static T Length<T>(this Vector4T<T> vector) where T : INumber<T>, IRootFunctions<T>
    {
        return Vector4T.Magnitude(vector);
    }

    [MethodImpl(Vector4T.Options)]
    public static Vector4T<T> Normalize<T>(this Vector4T<T> vector) where T : INumber<T>, IRootFunctions<T>
    {
        return Vector4T.Normalize(vector);
    }
}