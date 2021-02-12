using System;
using System.Runtime.InteropServices;

namespace Poltergeist.Core.Memory
{
	public sealed unsafe class NativeStruct<T> : IDisposable where T : struct
	{
		// ReSharper disable once StaticMemberInGenericType
		public static readonly INativeAllocator DefaultAllocator = new CoTaskMemAllocator();

		public readonly void* Data;
		public readonly int Size;

		internal readonly INativeAllocator Allocator;

		private readonly bool _zeroOnFree;
		private readonly object _disposeLock = new();

		private bool _valid;

		public NativeStruct(T structure, bool zeroOnFree = false, INativeAllocator allocator = null)
		{
			Size = Marshal.SizeOf<T>();

			Allocator = allocator ?? DefaultAllocator;
			IntPtr ptr = Allocator.Allocate(Size);
			GC.AddMemoryPressure(Size);

			Marshal.StructureToPtr(structure, ptr, false);
			Data = ptr.ToPointer();

			_valid = Data != null;
			_zeroOnFree = zeroOnFree;
		}

		private void Free()
		{
			lock (_disposeLock)
			{
				if (!_valid)
					return;
				IntPtr ptr = new(Data);

				Marshal.DestroyStructure<T>(ptr);
				if (_zeroOnFree)
					new Span<byte>(Data, Size).Clear();
				Allocator.Free(ptr);
				GC.RemoveMemoryPressure(Size);

				_valid = false;
			}
		}

		public void Dispose()
		{
			Free();
			GC.SuppressFinalize(this);
		}

		~NativeStruct()
		{
			Free();
		}
	}
}
