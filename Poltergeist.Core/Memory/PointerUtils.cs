using System.Runtime.CompilerServices;

namespace Poltergeist.Core.Memory
{
	public static unsafe class PointerUtils
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static bool IsReadable<T>(T* ptr) where T : unmanaged
		{
			try
			{
				_ = *ptr;
				return true;
			}
			catch
			{
				return false;
			}
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static bool IsReadable<T>(T** ptr) where T : unmanaged
		{
			try
			{
				_ = *ptr;
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
