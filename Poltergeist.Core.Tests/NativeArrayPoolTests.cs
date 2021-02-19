using Poltergeist.Core.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Poltergeist.Core.Tests
{
	public class NativeArrayPoolTests
	{
		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(64)]
		[InlineData(1024)]
		public void RentTest(int size)
		{
			NativeArray<int> nativeArray = NativeArrayPool<int>.Rent(size);
			try
			{
				Assert.NotNull(nativeArray);
				Assert.True(nativeArray.Count >= size);
			}
			finally
			{
				NativeArrayPool<int>.Return(nativeArray);
			}
		}

		[Theory]
		[InlineData(0, 10)]
		[InlineData(1, 10)]
		[InlineData(64, 10)]
		[InlineData(1024, 10)]
		public void RentMultipleTest(int size, int count)
		{
			NativeArray<int>[] nativeArrays = new NativeArray<int>[count];
			for (int i = 0; i < count; i++)
				nativeArrays[i] = NativeArrayPool<int>.Rent(size);

			try
			{
				foreach (NativeArray<int> nativeArray in nativeArrays)
				{
					Assert.NotNull(nativeArray);
					Assert.True(nativeArray.Count >= size);
				}
			}
			finally
			{
				foreach (NativeArray<int> nativeArray in nativeArrays)
					NativeArrayPool<int>.Return(nativeArray);
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(64)]
		[InlineData(1024)]
		public void ParallelRentTest(int size)
		{
			Parallel.For(0, Environment.ProcessorCount * 2, (_, _) =>
			{
				NativeArray<int> nativeArray = NativeArrayPool<int>.Rent(size);
				try
				{
					Assert.NotNull(nativeArray);
					Assert.True(nativeArray.Count >= size);
				}
				finally
				{
					NativeArrayPool<int>.Return(nativeArray);
				}
			});
		}

		[Theory]
		[InlineData(5)]
		[InlineData(10)]
		public unsafe void ZeroCacheTest(int count)
		{
			NativeArray<int>[] nativeArrays = new NativeArray<int>[count];
			for (int i = 0; i < count; i++)
				nativeArrays[i] = NativeArrayPool<int>.Rent(0);

			try
			{
				foreach (NativeArray<int> nativeArray in nativeArrays)
				{
					Assert.NotNull(nativeArray);
					Assert.Equal(0, nativeArray.Count);
					Assert.True(nativeArrays[0].Data == nativeArray.Data);
					Assert.Equal(nativeArrays[0], nativeArray);
				}
			}
			finally
			{
				foreach (NativeArray<int> nativeArray in nativeArrays)
					NativeArrayPool<int>.Return(nativeArray);
			}
		}

		[Theory]
		[InlineData(1, 10)]
		[InlineData(64, 10)]
		[InlineData(1024, 10)]
		public unsafe void UniqueTest(int size, int count)
		{
			NativeArray<int>[] nativeArrays = new NativeArray<int>[count];
			for (int i = 0; i < count; i++)
				nativeArrays[i] = NativeArrayPool<int>.Rent(size);

			HashSet<NativeArray<int>> arrays = new();
			HashSet<IntPtr> datas = new();

			try
			{
				foreach (NativeArray<int> nativeArray in nativeArrays)
				{
					Assert.True(arrays.Add(nativeArray));
					Assert.True(datas.Add(new IntPtr(nativeArray.Data)));
				}
			}
			finally
			{
				foreach (NativeArray<int> nativeArray in nativeArrays)
					NativeArrayPool<int>.Return(nativeArray);
			}
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(int.MinValue)]
		public void NegativeTest(int size)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => NativeArrayPool<int>.Rent(size));
		}

		[Fact]
		public void ReturnNullTest()
		{
			Assert.Throws<ArgumentNullException>(() => NativeArrayPool<int>.Return(null));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(10)]
		public void ReturnNotFromPoolTest(int size)
		{
			NativeArray<int> array = new(size);
			try
			{
				Assert.Throws<ArgumentException>(() => NativeArrayPool<int>.Return(array));
			}
			finally
			{
				array.Dispose();
			}
		}
	}
}
