using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Poltergeist.Core.Math
{
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct Vector4 : IEquatable<Vector4>, IFormattable
	{
		private const float Epsilon = 0.001f;

		public static Vector4 Zero => new(0F, 0F, 0F, 0F);
		public static Vector4 One => new(1F, 1F, 1F, 1F);
		public static Vector4 PositiveInfinity => new(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		public static Vector4 NegativeInfinity => new(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

		public readonly float X;
		public readonly float Y;
		public readonly float Z;
		public readonly float W;

		public Vector4(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 operator -(Vector4 a) => new(-a.X, -a.Y, -a.Z, -a.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 operator +(Vector4 a, Vector4 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 operator -(Vector4 a, Vector4 b) => new(a.X - b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 operator *(Vector4 a, Vector4 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 operator /(Vector4 a, Vector4 b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 operator *(Vector4 a, float d) => new(a.X * d, a.Y * d, a.Z * d, a.W * d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 operator *(float d, Vector4 a) => new(a.X * d, a.Y * d, a.Z * d, a.W * d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 operator /(Vector4 a, float d) => new(a.X / d, a.Y / d, a.Z / d, a.W / d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector4 other)
		{
			return MathF.Abs(X - other.X) <= Epsilon && MathF.Abs(Y - other.Y) <= Epsilon && MathF.Abs(Z - other.Z) <= Epsilon && MathF.Abs(W - other.W) <= Epsilon;
		}

		public override bool Equals(object obj)
		{
			return obj is Vector4 other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(X, Y, Z, W);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector4 left, Vector4 right)
		{
			return left.Equals(right);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector4 left, Vector4 right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"({X}, {Y}, {Z}, {W})";
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return $"({X.ToString(format, formatProvider)}, {Y.ToString(format, formatProvider)}, {Z.ToString(format, formatProvider)}, {W.ToString(format, formatProvider)})";
		}
	}
}
