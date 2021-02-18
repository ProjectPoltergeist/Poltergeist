using Poltergeist.Core.Memory;
using System;
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
		public void Rent(int size)
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
		public void RentMultiple(int size, int count)
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
		public void ParallelRent(int size)
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
	}
}
