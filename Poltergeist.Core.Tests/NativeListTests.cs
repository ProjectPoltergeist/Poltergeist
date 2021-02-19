using Poltergeist.Core.Memory;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Xunit;

namespace Poltergeist.Core.Tests
{
	public unsafe class NativeListTests
	{
		[Fact]
		public void CreateTest()
		{
			using (NativeList<int> nativeList = new())
			{
				Assert.NotNull(nativeList.Allocator);
				Assert.Equal(NativeList<int>.DefaultAllocator, nativeList.Allocator);
				Assert.True(nativeList.Data != null);
				Assert.Empty(nativeList);
				Assert.True(nativeList.Capacity > 0);
				Assert.False(nativeList.IsFixedSize);
				Assert.False(nativeList.IsReadOnly);
				Assert.False(nativeList.IsSynchronized);
			}
		}

		[Fact]
		public void CreateDataTest()
		{
			int[] data = new int[1024];
			RandomNumberGenerator.Fill(MemoryMarshal.AsBytes(data.AsSpan()));
			using (NativeList<int> nativeList = new(data))
			{
				Assert.NotNull(nativeList.Allocator);
				Assert.Equal(NativeList<int>.DefaultAllocator, nativeList.Allocator);
				Assert.True(nativeList.Data != null);
				AssertUtils.SequenceEqual(data, nativeList);
				Assert.True(nativeList.Capacity >= data.Length);
				Assert.False(nativeList.IsFixedSize);
				Assert.False(nativeList.IsReadOnly);
				Assert.False(nativeList.IsSynchronized);
			}
		}
	}
}
