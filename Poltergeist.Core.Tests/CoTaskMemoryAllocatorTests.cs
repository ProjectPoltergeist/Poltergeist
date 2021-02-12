using Poltergeist.Core.Memory;
using System;
using Xunit;

namespace Poltergeist.Core.Tests
{
	public class CoTaskMemoryAllocatorTests
	{
		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(16)]
		[InlineData(1024)]
		public unsafe void AllocationTest(int size)
		{
			CoTaskMemAllocator allocator = new();
			IntPtr data = allocator.Allocate(size);
			try
			{
				Assert.NotEqual(IntPtr.Zero, data);
				byte* bytes = (byte*)data.ToPointer();
				// check if writing to memory doesn't crash
				for (int i = 0; i < size; i++)
				{
					bytes[i] = byte.MaxValue;
				}
				// check if reading memory yields correct values
				for (int i = 0; i < size; i++)
				{
					Assert.Equal(byte.MaxValue, bytes[i]);
				}
			}
			finally
			{
				allocator.Free(data);
			}
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(int.MinValue)]
		public void NegativeSizeTest(int size)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => { new CoTaskMemAllocator().Allocate(size); });
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		[InlineData(1)]
		[InlineData(int.MinValue)]
		[InlineData(int.MaxValue)]
		public void InvalidFreeTest(int data)
		{
			Assert.Throws<InvalidOperationException>(() => { new CoTaskMemAllocator().Free(new IntPtr(data)); });
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(16)]
		[InlineData(1024)]
		public void DoubleFreeTest(int size)
		{
			CoTaskMemAllocator allocator = new();
			IntPtr data = allocator.Allocate(size);
			allocator.Free(data);
			Assert.Throws<InvalidOperationException>(() => { allocator.Free(data); });
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(16)]
		[InlineData(1024)]
		public void WrongAllocatorFreeTest(int size)
		{
			CoTaskMemAllocator allocator = new();
			IntPtr data = allocator.Allocate(size);
			try
			{
				Assert.Throws<InvalidOperationException>(() => { new CoTaskMemAllocator().Free(data); });
			}
			finally
			{
				allocator.Free(data);
			}
		}
	}
}
