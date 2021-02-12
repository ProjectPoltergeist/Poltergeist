using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Poltergeist.Core.Math
{
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct Vector3Int : IEquatable<Vector3Int>, IFormattable
	{
		public static Vector3Int Zero => new(0, 0, 0);
		public static Vector3Int One => new(1, 1, 1);
		public static Vector3Int Up => new(0, 1, 0);
		public static Vector3Int Down => new(0, -1, 0);
		public static Vector3Int Left => new(-1, 0, 0);
		public static Vector3Int Right => new(1, 0, 0);

		public readonly int X;
		public readonly int Y;
		public readonly int Z;

		public Vector3Int(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int operator -(Vector3Int a) => new(-a.X, -a.Y, -a.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int operator +(Vector3Int a, Vector3Int b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int operator -(Vector3Int a, Vector3Int b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int operator *(Vector3Int a, Vector3Int b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int operator /(Vector3Int a, Vector3Int b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int operator *(Vector3Int a, int d) => new(a.X * d, a.Y * d, a.Z * d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int operator *(int d, Vector3Int a) => new(a.X * d, a.Y * d, a.Z * d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int operator /(Vector3Int a, int d) => new(a.X / d, a.Y / d, a.Z / d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector3Int other)
		{
			return X == other.X && Y == other.Y && Z == other.Z;
		}

		public override bool Equals(object obj)
		{
			return obj is Vector3Int other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(X, Y, Z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector3Int left, Vector3Int right)
		{
			return left.Equals(right);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3Int left, Vector3Int right)
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
