using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sandy.Math;

[StructLayout(LayoutKind.Sequential)]
public struct Vector3T<T> : IEquatable<Vector3T<T>> where T : INumber<T>
{
    public static Vector3T<T> Zero => new Vector3T<T>(T.Zero, T.Zero, T.Zero);

    public static Vector3T<T> One => new Vector3T<T>(T.One, T.One, T.One);

    public static Vector3T<T> UnitX => new Vector3T<T>(T.One, T.Zero, T.Zero);

    public static Vector3T<T> UnitY => new Vector3T<T>(T.Zero, T.One, T.Zero);

    public static Vector3T<T> UnitZ => new Vector3T<T>(T.Zero, T.Zero, T.One);

    public T X;
    public T Y;
    public T Z;

    public Vector3T(T scalar)
    {
        X = scalar;
        Y = scalar;
        Z = scalar;
    }

    public Vector3T(T x, T y, T z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3T(Vector2T<T> xy, T z)
    {
        X = xy.X;
        Y = xy.Y;
        Z = z;
    }

    public static Vector3T<T> operator +(Vector3T<T> left, Vector3T<T> right)
    {
        return new Vector3T<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static Vector3T<T> operator -(Vector3T<T> left, Vector3T<T> right)
    {
        return new Vector3T<T>(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }
    
    public static Vector3T<T> operator -(Vector3T<T> vector)
    {
        return new Vector3T<T>(-vector.X, -vector.Y, -vector.Z);
    }

    public static Vector3T<T> operator *(Vector3T<T> left, Vector3T<T> right)
    {
        return new Vector3T<T>(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    public static Vector3T<T> operator *(Vector3T<T> left, T right)
    {
        return new Vector3T<T>(left.X * right, left.Y * right, left.Z * right);
    }

    public static Vector3T<T> operator *(T left, Vector3T<T> right)
    {
        return new Vector3T<T>(left * right.X, left * right.Y, left * right.Z);
    }

    public static Vector3T<T> operator /(Vector3T<T> left, Vector3T<T> right)
    {
        return new Vector3T<T>(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
    }

    public static Vector3T<T> operator /(Vector3T<T> left, T right)
    {
        return new Vector3T<T>(left.X / right, left.Y / right, left.Z / right);
    }

    public static bool operator ==(Vector3T<T> left, Vector3T<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector3T<T> left, Vector3T<T> right)
    {
        return !left.Equals(right);
    }

    public static explicit operator System.Numerics.Vector3(Vector3T<T> vector)
    {
        float x = Convert.ToSingle(vector.X);
        float y = Convert.ToSingle(vector.Y);
        float z = Convert.ToSingle(vector.Z);
        return new Vector3(x, y, z);
    }

    public static explicit operator Vector3T<T>(System.Numerics.Vector3 vector)
    {
        T x = T.CreateChecked(vector.X);
        T y = T.CreateChecked(vector.Y);
        T z = T.CreateChecked(vector.Z);
        return new Vector3T<T>(x, y, z);
    }

    [MethodImpl(Vector3T.Options)]
    public readonly Vector3T<TOther> As<TOther>() where TOther : INumber<TOther>
    {
        TOther x = TOther.CreateChecked(X);
        TOther y = TOther.CreateChecked(Y);
        TOther z = TOther.CreateChecked(Z);
        return new Vector3T<TOther>(x, y, z);
    }

    public override string ToString()
    {
        return $"Vector3T<{typeof(T).FullName}>(X: {X}, Y: {Y}, Z: {Z})";
    }

    public bool Equals(Vector3T<T> other)
    {
        return EqualityComparer<T>.Default.Equals(X, other.X) && EqualityComparer<T>.Default.Equals(Y, other.Y) &&
               EqualityComparer<T>.Default.Equals(Z, other.Z);
    }

    public override bool Equals(object obj)
    {
        return obj is Vector3T<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    #region Swizzle

    public Vector2T<T> XX => new Vector2T<T>(X, X);

    public Vector2T<T> XY => new Vector2T<T>(X, Y);

    public Vector2T<T> XZ => new Vector2T<T>(X, Z);

    public Vector2T<T> YX => new Vector2T<T>(Y, X);

    public Vector2T<T> YY => new Vector2T<T>(Y, Y);

    public Vector2T<T> YZ => new Vector2T<T>(Y, Z);

    public Vector2T<T> ZX => new Vector2T<T>(Z, X);

    public Vector2T<T> ZY => new Vector2T<T>(Z, Y);

    public Vector2T<T> ZZ => new Vector2T<T>(Z, Z);

    public Vector3T<T> XXX => new Vector3T<T>(X, X, X);

    public Vector3T<T> XXY => new Vector3T<T>(X, X, Y);

    public Vector3T<T> XXZ => new Vector3T<T>(X, X, Z);

    public Vector3T<T> XYX => new Vector3T<T>(X, Y, X);

    public Vector3T<T> XYY => new Vector3T<T>(X, Y, Y);

    public Vector3T<T> XYZ => new Vector3T<T>(X, Y, Z);

    public Vector3T<T> XZX => new Vector3T<T>(X, Z, X);

    public Vector3T<T> XZY => new Vector3T<T>(X, Z, Y);

    public Vector3T<T> XZZ => new Vector3T<T>(X, Z, Z);

    public Vector3T<T> YXX => new Vector3T<T>(Y, X, X);

    public Vector3T<T> YXY => new Vector3T<T>(Y, X, Y);

    public Vector3T<T> YXZ => new Vector3T<T>(Y, X, Z);

    public Vector3T<T> YYX => new Vector3T<T>(Y, Y, X);

    public Vector3T<T> YYY => new Vector3T<T>(Y, Y, Y);

    public Vector3T<T> YYZ => new Vector3T<T>(Y, Y, Z);

    public Vector3T<T> YZX => new Vector3T<T>(Y, Z, X);

    public Vector3T<T> YZY => new Vector3T<T>(Y, Z, Y);

    public Vector3T<T> YZZ => new Vector3T<T>(Y, Z, Z);

    public Vector3T<T> ZXX => new Vector3T<T>(Z, X, X);

    public Vector3T<T> ZXY => new Vector3T<T>(Z, X, Y);

    public Vector3T<T> ZXZ => new Vector3T<T>(Z, X, Z);

    public Vector3T<T> ZYX => new Vector3T<T>(Z, Y, X);

    public Vector3T<T> ZYY => new Vector3T<T>(Z, Y, Y);

    public Vector3T<T> ZYZ => new Vector3T<T>(Z, Y, Z);

    public Vector3T<T> ZZX => new Vector3T<T>(Z, Z, X);

    public Vector3T<T> ZZY => new Vector3T<T>(Z, Z, Y);

    public Vector3T<T> ZZZ => new Vector3T<T>(Z, Z, Z);

    public Vector4T<T> XXXX => new Vector4T<T>(X, X, X, X);

    public Vector4T<T> XXXY => new Vector4T<T>(X, X, X, Y);

    public Vector4T<T> XXXZ => new Vector4T<T>(X, X, X, Z);

    public Vector4T<T> XXYX => new Vector4T<T>(X, X, Y, X);

    public Vector4T<T> XXYY => new Vector4T<T>(X, X, Y, Y);

    public Vector4T<T> XXYZ => new Vector4T<T>(X, X, Y, Z);

    public Vector4T<T> XXZX => new Vector4T<T>(X, X, Z, X);

    public Vector4T<T> XXZY => new Vector4T<T>(X, X, Z, Y);

    public Vector4T<T> XXZZ => new Vector4T<T>(X, X, Z, Z);

    public Vector4T<T> XYXX => new Vector4T<T>(X, Y, X, X);

    public Vector4T<T> XYXY => new Vector4T<T>(X, Y, X, Y);

    public Vector4T<T> XYXZ => new Vector4T<T>(X, Y, X, Z);

    public Vector4T<T> XYYX => new Vector4T<T>(X, Y, Y, X);

    public Vector4T<T> XYYY => new Vector4T<T>(X, Y, Y, Y);

    public Vector4T<T> XYYZ => new Vector4T<T>(X, Y, Y, Z);

    public Vector4T<T> XYZX => new Vector4T<T>(X, Y, Z, X);

    public Vector4T<T> XYZY => new Vector4T<T>(X, Y, Z, Y);

    public Vector4T<T> XYZZ => new Vector4T<T>(X, Y, Z, Z);

    public Vector4T<T> XZXX => new Vector4T<T>(X, Z, X, X);

    public Vector4T<T> XZXY => new Vector4T<T>(X, Z, X, Y);

    public Vector4T<T> XZXZ => new Vector4T<T>(X, Z, X, Z);

    public Vector4T<T> XZYX => new Vector4T<T>(X, Z, Y, X);

    public Vector4T<T> XZYY => new Vector4T<T>(X, Z, Y, Y);

    public Vector4T<T> XZYZ => new Vector4T<T>(X, Z, Y, Z);

    public Vector4T<T> XZZX => new Vector4T<T>(X, Z, Z, X);

    public Vector4T<T> XZZY => new Vector4T<T>(X, Z, Z, Y);

    public Vector4T<T> XZZZ => new Vector4T<T>(X, Z, Z, Z);

    public Vector4T<T> YXXX => new Vector4T<T>(Y, X, X, X);

    public Vector4T<T> YXXY => new Vector4T<T>(Y, X, X, Y);

    public Vector4T<T> YXXZ => new Vector4T<T>(Y, X, X, Z);

    public Vector4T<T> YXYX => new Vector4T<T>(Y, X, Y, X);

    public Vector4T<T> YXYY => new Vector4T<T>(Y, X, Y, Y);

    public Vector4T<T> YXYZ => new Vector4T<T>(Y, X, Y, Z);

    public Vector4T<T> YXZX => new Vector4T<T>(Y, X, Z, X);

    public Vector4T<T> YXZY => new Vector4T<T>(Y, X, Z, Y);

    public Vector4T<T> YXZZ => new Vector4T<T>(Y, X, Z, Z);

    public Vector4T<T> YYXX => new Vector4T<T>(Y, Y, X, X);

    public Vector4T<T> YYXY => new Vector4T<T>(Y, Y, X, Y);

    public Vector4T<T> YYXZ => new Vector4T<T>(Y, Y, X, Z);

    public Vector4T<T> YYYX => new Vector4T<T>(Y, Y, Y, X);

    public Vector4T<T> YYYY => new Vector4T<T>(Y, Y, Y, Y);

    public Vector4T<T> YYYZ => new Vector4T<T>(Y, Y, Y, Z);

    public Vector4T<T> YYZX => new Vector4T<T>(Y, Y, Z, X);

    public Vector4T<T> YYZY => new Vector4T<T>(Y, Y, Z, Y);

    public Vector4T<T> YYZZ => new Vector4T<T>(Y, Y, Z, Z);

    public Vector4T<T> YZXX => new Vector4T<T>(Y, Z, X, X);

    public Vector4T<T> YZXY => new Vector4T<T>(Y, Z, X, Y);

    public Vector4T<T> YZXZ => new Vector4T<T>(Y, Z, X, Z);

    public Vector4T<T> YZYX => new Vector4T<T>(Y, Z, Y, X);

    public Vector4T<T> YZYY => new Vector4T<T>(Y, Z, Y, Y);

    public Vector4T<T> YZYZ => new Vector4T<T>(Y, Z, Y, Z);

    public Vector4T<T> YZZX => new Vector4T<T>(Y, Z, Z, X);

    public Vector4T<T> YZZY => new Vector4T<T>(Y, Z, Z, Y);

    public Vector4T<T> YZZZ => new Vector4T<T>(Y, Z, Z, Z);

    public Vector4T<T> ZXXX => new Vector4T<T>(Z, X, X, X);

    public Vector4T<T> ZXXY => new Vector4T<T>(Z, X, X, Y);

    public Vector4T<T> ZXXZ => new Vector4T<T>(Z, X, X, Z);

    public Vector4T<T> ZXYX => new Vector4T<T>(Z, X, Y, X);

    public Vector4T<T> ZXYY => new Vector4T<T>(Z, X, Y, Y);

    public Vector4T<T> ZXYZ => new Vector4T<T>(Z, X, Y, Z);

    public Vector4T<T> ZXZX => new Vector4T<T>(Z, X, Z, X);

    public Vector4T<T> ZXZY => new Vector4T<T>(Z, X, Z, Y);

    public Vector4T<T> ZXZZ => new Vector4T<T>(Z, X, Z, Z);

    public Vector4T<T> ZYXX => new Vector4T<T>(Z, Y, X, X);

    public Vector4T<T> ZYXY => new Vector4T<T>(Z, Y, X, Y);

    public Vector4T<T> ZYXZ => new Vector4T<T>(Z, Y, X, Z);

    public Vector4T<T> ZYYX => new Vector4T<T>(Z, Y, Y, X);

    public Vector4T<T> ZYYY => new Vector4T<T>(Z, Y, Y, Y);

    public Vector4T<T> ZYYZ => new Vector4T<T>(Z, Y, Y, Z);

    public Vector4T<T> ZYZX => new Vector4T<T>(Z, Y, Z, X);

    public Vector4T<T> ZYZY => new Vector4T<T>(Z, Y, Z, Y);

    public Vector4T<T> ZYZZ => new Vector4T<T>(Z, Y, Z, Z);

    public Vector4T<T> ZZXX => new Vector4T<T>(Z, Z, X, X);

    public Vector4T<T> ZZXY => new Vector4T<T>(Z, Z, X, Y);

    public Vector4T<T> ZZXZ => new Vector4T<T>(Z, Z, X, Z);

    public Vector4T<T> ZZYX => new Vector4T<T>(Z, Z, Y, X);

    public Vector4T<T> ZZYY => new Vector4T<T>(Z, Z, Y, Y);

    public Vector4T<T> ZZYZ => new Vector4T<T>(Z, Z, Y, Z);

    public Vector4T<T> ZZZX => new Vector4T<T>(Z, Z, Z, X);

    public Vector4T<T> ZZZY => new Vector4T<T>(Z, Z, Z, Y);

    public Vector4T<T> ZZZZ => new Vector4T<T>(Z, Z, Z, Z);

    #endregion
}

public static class Vector3T
{
    internal const MethodImplOptions Options =
        MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;
    
    [MethodImpl(Options)]
    public static T MagnitudeSquared<T>(in Vector3T<T> vector) where T : INumber<T>
    {
        return Dot(vector, vector);
    }

    [MethodImpl(Options)]
    public static T Magnitude<T>(in Vector3T<T> vector) where T : INumber<T>, IRootFunctions<T>
    {
        return T.Sqrt(MagnitudeSquared(vector));
    }

    [MethodImpl(Options)]
    public static void Normalize<T>(ref Vector3T<T> vector) where T : INumber<T>, IRootFunctions<T>
    {
        T magnitude = Magnitude(vector);
        vector.X /= magnitude;
        vector.Y /= magnitude;
        vector.Z /= magnitude;
    }

    [MethodImpl(Options)]
    public static Vector3T<T> Normalize<T>(Vector3T<T> vector) where T : INumber<T>, IRootFunctions<T>
    {
        Normalize(ref vector);
        return vector;
    }

    [MethodImpl(Options)]
    public static T Dot<T>(in Vector3T<T> a, in Vector3T<T> b) where T : INumber<T>
    {
        return T.CreateChecked(a.X * b.X + a.Y * b.Y + a.Z * b.Z);
    }

    [MethodImpl(Options)]
    public static Vector3T<T> Abs<T>(in Vector3T<T> vector) where T : INumber<T>
    {
        return new Vector3T<T>(T.Abs(vector.X), T.Abs(vector.Y), T.Abs(vector.Z));
    }

    [MethodImpl(Options)]
    public static Vector3T<T> Clamp<T>(in Vector3T<T> vector, in Vector3T<T> min, in Vector3T<T> max)
        where T : INumber<T>
    {
        T x = T.Clamp(vector.X, min.X, max.X);
        T y = T.Clamp(vector.Y, min.Y, max.Y);
        T z = T.Clamp(vector.Z, min.Z, max.Z);
        return new Vector3T<T>(x, y, z);
    }

    [MethodImpl(Options)]
    public static T DistanceSquared<T>(in Vector3T<T> a, in Vector3T<T> b) where T : INumber<T>
    {
        Vector3T<T> res = a - b;
        return Dot(res, res);
    }

    [MethodImpl(Options)]
    public static T Distance<T>(in Vector3T<T> a, in Vector3T<T> b) where T : INumber<T>, IRootFunctions<T>
    {
        return T.Sqrt(DistanceSquared(a, b));
    }

    [MethodImpl(Options)]
    public static Vector3T<T> Lerp<T>(in Vector3T<T> a, in Vector3T<T> b, T amount) where T : INumber<T>
    {
        T x = MathHelper.Lerp(a.X, b.X, amount);
        T y = MathHelper.Lerp(a.Y, b.Y, amount);
        T z = MathHelper.Lerp(a.Z, b.Z, amount);
        return new Vector3T<T>(x, y, z);
    }

    [MethodImpl(Options)]
    public static Vector3T<T> Max<T>(in Vector3T<T> a, in Vector3T<T> b) where T : INumber<T>
    {
        T x = T.Max(a.X, b.X);
        T y = T.Max(a.Y, b.Y);
        T z = T.Max(a.Z, b.Z);
        return new Vector3T<T>(x, y, z);
    }

    [MethodImpl(Options)]
    public static Vector3T<T> Min<T>(in Vector3T<T> a, in Vector3T<T> b) where T : INumber<T>
    {
        T x = T.Min(a.X, b.X);
        T y = T.Min(a.Y, b.Y);
        T z = T.Min(a.Z, b.Z);
        return new Vector3T<T>(x, y, z);
    }

    [MethodImpl(Options)]
    public static Vector3T<T> Reflect<T>(in Vector3T<T> surface, in Vector3T<T> normal) where T : INumber<T>
    {
        return surface - (T.CreateChecked(2) * Dot(surface, normal)) * normal;
    }

    [MethodImpl(Options)]
    public static Vector3T<T> Sqrt<T>(in Vector3T<T> vector) where T : INumber<T>, IRootFunctions<T>
    {
        T x = T.Sqrt(vector.X);
        T y = T.Sqrt(vector.Y);
        T z = T.Sqrt(vector.Z);
        return new Vector3T<T>(x, y, z);
    }

    [MethodImpl(Options)]
    public static Vector3T<T> Cross<T>(in Vector3T<T> a, in Vector3T<T> b) where T : INumber<T>
    {
        return new Vector3T<T>(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
    }

    [MethodImpl(Options)]
    public static Vector3T<T> Transform<T>(in Vector3T<T> vector, in MatrixT<T> matrix) where T : INumber<T>
    {
        throw new NotImplementedException();
    }

    [MethodImpl(Options)]
    public static Vector3T<T> Transform<T>(in Vector3T<T> vector, in QuaternionT<T> quaternion) where T : INumber<T>
    {
        throw new NotImplementedException();
    }
    
    [MethodImpl(Options)]
    public static Vector3T<T> TransformNormal<T>(in Vector3T<T> vector, in MatrixT<T> matrix) where T : INumber<T>
    {
        throw new NotImplementedException();
    }

    [MethodImpl(Options)]
    public static Vector3T<T> TransformNormal<T>(in Vector3T<T> vector, in QuaternionT<T> quaternion) where T : INumber<T>
    {
        throw new NotImplementedException();
    }
}

public static class Vector3TExtensions
{
    [MethodImpl(Vector3T.Options)]
    public static T LengthSquared<T>(this Vector3T<T> vector) where T : INumber<T>
    {
        return Vector3T.MagnitudeSquared(vector);
    }

    [MethodImpl(Vector3T.Options)]
    public static T Length<T>(this Vector3T<T> vector) where T : INumber<T>, IRootFunctions<T>
    {
        return Vector3T.Magnitude(vector);
    }

    [MethodImpl(Vector3T.Options)]
    public static Vector3T<T> Normalize<T>(this Vector3T<T> vector) where T : INumber<T>, IRootFunctions<T>
    {
        return Vector3T.Normalize(vector);
    }
}