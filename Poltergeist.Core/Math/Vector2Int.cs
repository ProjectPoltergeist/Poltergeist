using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Poltergeist.Core.Math
{
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct Vector2Int : IEquatable<Vector2Int>, IFormattable
	{
		public static Vector2Int Zero => new(0, 0);
		public static Vector2Int One => new(1, 1);
		public static Vector2Int Up => new(0, 1);
		public static Vector2Int Down => new(0, -1);
		public static Vector2Int Left => new(-1, 0);
		public static Vector2Int Right => new(1, 0);

		public readonly int X;
		public readonly int Y;

		public Vector2Int(int x, int y)
		{
			X = x;
			Y = y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator -(Vector2Int a) => new(-a.X, -a.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new(a.X + b.X, a.Y + b.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new(a.X - b.X, a.Y - b.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator *(Vector2Int a, Vector2Int b) => new(a.X * b.X, a.Y * b.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator /(Vector2Int a, Vector2Int b) => new(a.X / b.X, a.Y / b.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator *(Vector2Int a, int d) => new(a.X * d, a.Y * d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator *(int d, Vector2Int a) => new(a.X * d, a.Y * d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator /(Vector2Int a, int d) => new(a.X / d, a.Y / d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector2Int other)
		{
			return X == other.X && Y == other.Y;
		}

		public override bool Equals(object obj)
		{
			return obj is Vector2Int other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(X, Y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2Int left, Vector2Int right)
		{
			return left.Equals(right);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2Int left, Vector2Int right)
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
