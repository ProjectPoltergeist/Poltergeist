using Poltergeist.Core.Memory;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace Poltergeist.Core.Tests
{
	public unsafe class NativeArrayTests
	{
		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(127)]
		[InlineData(1024)]
		public void CreateTest(int size)
		{
			using (NativeArray<int> nativeArray = new(size))
			{
				Assert.True(nativeArray.Data != null);
				Assert.NotNull(nativeArray.Allocator);
				Assert.Equal(size, nativeArray.Count);
				Assert.Equal(size * sizeof(int), nativeArray.ByteSize);
				Assert.True(nativeArray.AlignedSize >= nativeArray.ByteSize);
				// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
				Assert.True(nativeArray.All(v => v == 0));
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(127)]
		[InlineData(1024)]
		public void CreateWithDataTest(int size)
		{
			int[] array = new int[size];
			new Random().NextBytes(MemoryMarshal.AsBytes(array.AsSpan()));
			using (NativeArray<int> nativeArray = new(array))
			{
				Assert.True(nativeArray.Data != null);
				Assert.NotNull(nativeArray.Allocator);
				Assert.Equal(array.Length, nativeArray.Count);
				Assert.Equal(array.Length * sizeof(int), nativeArray.ByteSize);
				Assert.True(nativeArray.AlignedSize >= nativeArray.ByteSize);
				// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
				Assert.True(array.SequenceEqual(nativeArray));
			}
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(int.MinValue)]
		[InlineData(int.MaxValue)]
		public void InvalidLengthTest(int length)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				using (NativeArray<int> _ = new(length)) { }
			});
		}

		[Fact]
		public void SpanTest()
		{
			using (NativeArray<int> nativeArray = new(1024))
			{
				Assert.Equal(nativeArray.Count, nativeArray.AsSpan().Length);
				Assert.Equal(nativeArray.Count, nativeArray.AsReadOnlySpan().Length);
				Assert.True(nativeArray.AsSpan().SequenceEqual(new ReadOnlySpan<int>(nativeArray.Data, nativeArray.Count)));
				Assert.True(nativeArray.AsReadOnlySpan().SequenceEqual(new ReadOnlySpan<int>(nativeArray.Data, nativeArray.Count)));
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		[InlineData(int.MinValue)]
		[InlineData(int.MaxValue)]
		public void FillTest(int value)
		{
			using (NativeArray<int> nativeArray = new(1024, false))
			{
				nativeArray.Fill(value);
				// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
				Assert.True(nativeArray.All(v => v == value));
			}
		}

		[Fact]
		public void ClearTest()
		{
			using (NativeArray<uint> nativeArray = new(1024, false))
			{
				nativeArray.Fill(uint.MaxValue);
				nativeArray.Clear();
				// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
				Assert.True(nativeArray.All(v => v == 0));
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(128)]
		public void EnumerableTest(int length)
		{
			using (NativeArray<uint> nativeArray = new(length))
			{
				nativeArray.Fill(uint.MaxValue);
				// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
				Assert.True(nativeArray.All(v => v == uint.MaxValue));
#pragma warning disable CA1826
				Assert.Equal(nativeArray.Count, nativeArray.Count());
#pragma warning restore CA1826
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(128)]
		public void ToArrayTest(int length)
		{
			using (NativeArray<uint> nativeArray = new(length))
			{
				nativeArray.Fill(uint.MaxValue);
				uint[] array = nativeArray.ToArray();

				Assert.True(nativeArray.SequenceEqual(array));
				Assert.Equal(nativeArray.Count, array.Length);
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(128)]
		public void IterationTest(int length)
		{
			using (NativeArray<uint> nativeArray = new(length))
			{
				for (int i = 0; i < nativeArray.Count; i++)
					nativeArray[i] = uint.MaxValue;
				for (int i = 0; i < nativeArray.Count; i++)
					Assert.Equal(uint.MaxValue, nativeArray[i]);
				foreach (uint u in nativeArray)
					Assert.Equal(uint.MaxValue, u);
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(128)]
		public void IndexerTest(int length)
		{
			using (NativeArray<uint> nativeArray = new(length))
			{
				Assert.Throws<IndexOutOfRangeException>(() => { _ = nativeArray[-1]; });
				Assert.Throws<IndexOutOfRangeException>(() => { _ = nativeArray[nativeArray.Count]; });
				if (nativeArray.Count == 0)
					return;
				_ = nativeArray[0];
				_ = nativeArray[nativeArray.Count - 1];
			}
		}
	}
}
