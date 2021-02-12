using System.Runtime.CompilerServices;

namespace Poltergeist.Core.Math
{
	public static class MathUtils
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Align(int value, int alignment) => (value + alignment - 1) / alignment * alignment;
	}
}
