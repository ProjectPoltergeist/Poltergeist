using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Poltergeist.Core.Math
{
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct Vector3 : IEquatable<Vector3>, IFormattable
	{
		private const float Epsilon = 0.001f;

		public static Vector3 Zero => new(0F, 0F, 0F);
		public static Vector3 One => new(1F, 1F, 1F);
		public static Vector3 Forward => new(0F, 0F, 1F);
		public static Vector3 Back => new(0F, 0F, -1F);
		public static Vector3 Up => new(0F, 1F, 0F);
		public static Vector3 Down => new(0F, -1F, 0F);
		public static Vector3 Left => new(-1F, 0F, 0F);
		public static Vector3 Right => new(1F, 0F, 0F);
		public static Vector3 PositiveInfinity => new(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		public static Vector3 NegativeInfinity => new(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

		public readonly float X;
		public readonly float Y;
		public readonly float Z;

		public Vector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator -(Vector3 a) => new(-a.X, -a.Y, -a.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator -(Vector3 a, Vector3 b) => new(a.X - b.X, a.Y + b.Y, a.Z + b.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator *(Vector3 a, Vector3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator /(Vector3 a, Vector3 b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator *(Vector3 a, float d) => new(a.X * d, a.Y * d, a.Z * d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator *(float d, Vector3 a) => new(a.X * d, a.Y * d, a.Z * d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator /(Vector3 a, float d) => new(a.X / d, a.Y / d, a.Z / d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector3 other)
		{
			return MathF.Abs(X - other.X) <= Epsilon && MathF.Abs(Y - other.Y) <= Epsilon && MathF.Abs(Z - other.Z) <= Epsilon;
		}

		public override bool Equals(object obj)
		{
			return obj is Vector3 other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(X, Y, Z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector3 left, Vector3 right)
		{
			return left.Equals(right);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3 left, Vector3 right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"({X}, {Y}, {Z})";
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return $"({X.ToString(format, formatProvider)}, {Y.ToString(format, formatProvider)}, {Z.ToString(format, formatProvider)})";
		}
	}
}
