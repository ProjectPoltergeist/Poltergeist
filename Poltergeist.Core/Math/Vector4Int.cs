using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Poltergeist.Core.Math
{
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct Vector4Int : IEquatable<Vector4Int>, IFormattable
	{
		public static Vector4Int Zero => new(0, 0, 0, 0);
		public static Vector4Int One => new(1, 1, 1, 1);

		public readonly int X;
		public readonly int Y;
		public readonly int Z;
		public readonly int W;

		public Vector4Int(int x, int y, int z, int w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4Int operator -(Vector4Int a) => new(-a.X, -a.Y, -a.Z, -a.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4Int operator +(Vector4Int a, Vector4Int b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4Int operator -(Vector4Int a, Vector4Int b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4Int operator *(Vector4Int a, Vector4Int b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4Int operator /(Vector4Int a, Vector4Int b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4Int operator *(Vector4Int a, int d) => new(a.X * d, a.Y * d, a.Z * d, a.W * d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4Int operator *(int d, Vector4Int a) => new(a.X * d, a.Y * d, a.Z * d, a.W * d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4Int operator /(Vector4Int a, int d) => new(a.X / d, a.Y / d, a.Z / d, a.W / d);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector4Int other)
		{
			return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
		}

		public override bool Equals(object obj)
		{
			return obj is Vector4Int other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(X, Y, Z, W);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector4Int left, Vector4Int right)
		{
			return left.Equals(right);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector4Int left, Vector4Int right)
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
