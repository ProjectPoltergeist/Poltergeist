using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Poltergeist.Core.Math
{
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct Vector2 : IEquatable<Vector2>, IFormattable
	{
		private const float Epsilon = 0.001f;

		public static Vector2 Zero => new(0f, 0f);
		public static Vector2 One => new(1f, 1f);
		public static Vector2 Up => new(0f, 1f);
		public static Vector2 Down => new(0f, -1f);
		public static Vector2 Left => new(-1f, 0f);
		public static Vector2 Right => new(1f, 0f);
		public static Vector2 PositiveInfinity => new(float.PositiveInfinity, float.PositiveInfinity);
		public static Vector2 NegativeInfinity => new(float.NegativeInfinity, float.NegativeInfinity);

		public readonly float X;
		public readonly float Y;

		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator -(Vector2 a) => new(-a.X, -a.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(Vector2 a, Vector2 b) => new(a.X * b.X, a.Y * b.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator /(Vector2 a, Vector2 b) => new(a.X / b.X, a.Y / b.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(Vector2 a, float d) => new(a.X * d, a.Y * d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(float d, Vector2 a) => new(a.X * d, a.Y * d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator /(Vector2 a, float d) => new(a.X / d, a.Y / d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector2 other)
		{
			return MathF.Abs(X - other.X) <= Epsilon && MathF.Abs(Y - other.Y) <= Epsilon;
		}

		public override bool Equals(object obj)
		{
			return obj is Vector2 other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(X, Y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2 left, Vector2 right)
		{
			return left.Equals(right);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2 left, Vector2 right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"({X}, {Y})";
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return $"({X.ToString(format, formatProvider)}, {Y.ToString(format, formatProvider)})";
		}
	}
}
