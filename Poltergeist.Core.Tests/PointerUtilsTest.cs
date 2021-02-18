using Poltergeist.Core.Memory;
using System;
using Xunit;

namespace Poltergeist.Core.Tests
{
	public unsafe class PointerUtilsTest
	{
		[Fact]
		public void NullTest()
		{
			Assert.False(PointerUtils.IsReadable((byte*)null));
		}

		[Fact]
		public void NullDoubleTest()
		{
			Assert.False(PointerUtils.IsReadable((byte**)null));
		}

		[Fact]
		public void ValidTest()
		{
			using (NativeArray<byte> nativeArray = new(1))
				Assert.True(PointerUtils.IsReadable(nativeArray.Data));
		}

		[Fact]
		public void ValidDoubleTest()
		{
			using (NativeArray<IntPtr> nativeArray = new(1))
				Assert.True(PointerUtils.IsReadable((byte**)nativeArray.Data));
		}
	}
}
