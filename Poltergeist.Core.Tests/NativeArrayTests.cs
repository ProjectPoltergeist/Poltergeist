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
			using (NativeArray<int> a = new(size))
			{
				Assert.True(a.Data != null);
				Assert.NotNull(a.Allocator);
				Assert.Equal(size, a.Count);
				Assert.Equal(size * sizeof(int), a.ByteSize);
				Assert.True(a.AlignedSize >= a.ByteSize);
				// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
				Assert.True(a.All(i => i == 0));
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(127)]
		[InlineData(1024)]
		public void CreateWithDataTest(int size)
		{
			int[] arr = new int[size];
			new Random().NextBytes(MemoryMarshal.AsBytes(arr.AsSpan()));
			using (NativeArray<int> a = new(arr))
			{
				Assert.True(a.Data != null);
				Assert.NotNull(a.Allocator);
				Assert.Equal(arr.Length, a.Count);
				Assert.Equal(arr.Length * sizeof(int), a.ByteSize);
				Assert.True(a.AlignedSize >= a.ByteSize);
				// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
				Assert.True(arr.SequenceEqual(a));
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
			using (NativeArray<int> a = new(1024))
			{
				Assert.Equal(a.Count, a.AsSpan().Length);
				Assert.Equal(a.Count, a.AsReadOnlySpan().Length);
				Assert.True(a.AsSpan().SequenceEqual(new ReadOnlySpan<int>(a.Data, a.Count)));
				Assert.True(a.AsReadOnlySpan().SequenceEqual(new ReadOnlySpan<int>(a.Data, a.Count)));
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		[InlineData(int.MinValue)]
		[InlineData(int.MaxValue)]
		public void FillTest(int value)
		{
			using (NativeArray<int> a = new(1024, false))
			{
				a.Fill(value);
				// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
				Assert.True(a.All(i => i == value));
			}
		}

		[Fact]
		public void ClearTest()
		{
			using (NativeArray<uint> a = new(1024, false))
			{
				a.Fill(uint.MaxValue);
				a.Clear();
				// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
				Assert.True(a.All(i => i == 0));
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(128)]
		public void EnumerableTest(int length)
		{
			using (NativeArray<uint> a = new(length))
			{
				a.Fill(uint.MaxValue);
				// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
				Assert.True(a.All(i => i == uint.MaxValue));
#pragma warning disable CA1826
				Assert.Equal(a.Count, a.Count());
#pragma warning restore CA1826
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(128)]
		public void ToArrayTest(int length)
		{
			using (NativeArray<uint> a = new(length))
			{
				a.Fill(uint.MaxValue);
				uint[] ar = a.ToArray();

				Assert.True(a.SequenceEqual(ar));
				Assert.Equal(a.Count, ar.Length);
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(128)]
		public void IterationTest(int length)
		{
			using (NativeArray<uint> a = new(length))
			{
				for (int i = 0; i < a.Count; i++)
					a[i] = uint.MaxValue;
				for (int i = 0; i < a.Count; i++)
					Assert.Equal(uint.MaxValue, a[i]);
				foreach (uint u in a)
					Assert.Equal(uint.MaxValue, u);
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(128)]
		public void IndexerTest(int length)
		{
			using (NativeArray<uint> a = new(length))
			{
				Assert.Throws<IndexOutOfRangeException>(() => { _ = a[-1]; });
				Assert.Throws<IndexOutOfRangeException>(() => { _ = a[a.Count]; });
				if (a.Count == 0)
					return;
				_ = a[0];
				_ = a[a.Count - 1];
			}
		}
	}
}
