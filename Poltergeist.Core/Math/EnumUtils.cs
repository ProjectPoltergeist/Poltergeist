using System;
using System.Runtime.CompilerServices;

namespace Poltergeist.Core.Math
{
	public static class EnumUtils<T> where T : unmanaged, Enum
	{
		private static readonly T[] values = Enum.GetValues<T>();
		private static readonly string[] names = Enum.GetNames<T>();

		public static ReadOnlySpan<T> Values => values;
		public static ReadOnlySpan<string> Names => names;
		public static Type UnderlyingType { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = Enum.GetUnderlyingType(typeof(T));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsDefined(T value) => Enum.IsDefined(value);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetName(T value) => Enum.GetName(value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Parse(string value) => Enum.Parse<T>(value);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Parse(string value, bool ignoreCase) => Enum.Parse<T>(value, ignoreCase);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryParse(string value, out T enu) => Enum.TryParse(value, out enu);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryParse(string value, bool ignoreCase, out T enu) => Enum.TryParse(value, ignoreCase, out enu);
#pragma warning disable CA2248
		// ReSharper disable once HeapView.PossibleBoxingAllocation
		// ReSharper disable once HeapView.BoxingAllocation
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasFlag(T value, T flag) => value.HasFlag(flag);
#pragma warning restore CA2248
	}

	public static class EnumUtils
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasFlagF<T>(this T value, T flag) where T : unmanaged, Enum => EnumUtils<T>.HasFlag(value, flag);
	}
}
