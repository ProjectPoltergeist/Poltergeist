using System;

namespace Poltergeist.Core.Memory
{
	public interface INativeAllocator
	{
		IntPtr Allocate(int size);
		void Free(IntPtr data);
	}
}
