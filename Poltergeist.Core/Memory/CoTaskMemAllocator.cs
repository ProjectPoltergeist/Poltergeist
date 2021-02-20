using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Poltergeist.Core.Memory
{
	public sealed class CoTaskMemAllocator : INativeAllocator
	{
		private readonly HashSet<IntPtr> _allocatedPointers = new();
		private readonly object _lock = new();

		public IntPtr Allocate(int size)
		{
			if (size < 0)
				throw new ArgumentOutOfRangeException(nameof(size), size, "Allocation size must be positive");
			IntPtr ptr = Marshal.AllocCoTaskMem(size);
			if (ptr == IntPtr.Zero)
				throw new NullReferenceException();
			lock (_lock)
				_allocatedPointers.Add(ptr);
			return ptr;
		}

		public void Free(IntPtr data)
		{
			lock (_lock)
				if (!_allocatedPointers.Remove(data))
					throw new InvalidOperationException("Tried to doublefree or free invalid data");
			Marshal.FreeCoTaskMem(data);
		}
	}
}
