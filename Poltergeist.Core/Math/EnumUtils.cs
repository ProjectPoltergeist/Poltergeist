using System;
using System.Runtime.CompilerServices;

namespace Poltergeist.Core.Math
{
	public static class EnumUtils<T> where T : unmanaged, Enum
	{
		private static readonly T[] ValuesCache = Enum.GetValues<T>();
		private static readonly string[] NamesCache = Enum.GetNames<T>();

		public static ReadOnlySpan<T> Values => ValuesCache;
		public static ReadOnlySpan<string> Names => NamesCache;
		public static Type UnderlyingType { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; } = Enum.GetUnderlyingType(typeof(T));

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
