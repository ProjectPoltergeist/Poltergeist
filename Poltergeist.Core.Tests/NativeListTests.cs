using Poltergeist.Core.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Poltergeist.Core.Collections;
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

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(1024)]
		public void CreateDataTest(int size)
		{
			int[] data = new int[size];
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

		[Theory]
		[InlineData(1, 0)]
		[InlineData(128, 1024)]
		[InlineData(1024, 128)]
		public void EnsureCapacityTest(int initial, int requested)
		{
			using (NativeList<int> nativeList = new(initial))
			{
				Assert.True(nativeList.Capacity >= initial);
				nativeList.EnsureCapacity(requested);
				Assert.True(nativeList.Capacity >= requested);
			}
		}

		[Fact]
		public void EnsureCapacityDataTest()
		{
			int[] data = new int[1024];
			RandomNumberGenerator.Fill(MemoryMarshal.AsBytes(data.AsSpan()));
			using (NativeList<int> nativeList = new(data))
			{
				AssertUtils.SequenceEqual(data, nativeList);
				nativeList.EnsureCapacity(nativeList.Count * 3);
				AssertUtils.SequenceEqual(data, nativeList);
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(128)]
		public void IterationTest(int length)
		{
			uint[] array = new uint[length];
			array.AsSpan().Fill(uint.MaxValue);
			using (NativeList<uint> nativeList = new(array))
			{
				for (int i = 0; i < nativeList.Count; i++)
					Assert.Equal(uint.MaxValue, nativeList[i]);
				for (uint i = 0; i < nativeList.Count; i++)
					Assert.Equal(uint.MaxValue, nativeList[i]);
				foreach (uint u in nativeList)
					Assert.Equal(uint.MaxValue, u);
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(128)]
		public void IndexerTest(int length)
		{
			using (NativeList<uint> nativeList = new(new uint[length]))
			{
				Assert.Throws<IndexOutOfRangeException>(() => { _ = nativeList[-1]; });
				Assert.Throws<IndexOutOfRangeException>(() => { _ = nativeList[nativeList.Count]; });
				if (nativeList.Count != 0)
				{
					_ = nativeList[0];
					// ReSharper disable once UseIndexFromEndExpression
					_ = nativeList[nativeList.Count - 1];
					_ = nativeList[^1];
				}
				Assert.Throws<IndexOutOfRangeException>(() => { _ = nativeList[(uint)nativeList.Count]; });
				if (nativeList.Count != 0)
				{
					_ = nativeList[0u];
					_ = nativeList[(uint)(nativeList.Count - 1)];
				}
			}
		}

		[Theory]
		[InlineData(new int[0], 512, new[] { 512 })]
		[InlineData(new[] { 256 }, 512, new[] { 256, 512 })]
		[InlineData(new[] { 256, 512, 512 }, 512, new[] { 256, 512, 512, 512 })]
		public void AddTest(int[] initial, int value, int[] expected)
		{
			using (NativeList<int> nativeList = new(initial))
			{
				AssertUtils.SequenceEqual(initial, nativeList);
				nativeList.Add(value);
				AssertUtils.SequenceEqual(expected, nativeList);
			}
			using (NativeList<int> nativeList = new(initial))
			{
				AssertUtils.SequenceEqual(initial, nativeList);
				// ReSharper disable once HeapView.BoxingAllocation
				nativeList.Add((object)value);
				AssertUtils.SequenceEqual(expected, nativeList);
			}
		}

		[Theory]
		[InlineData(new int[0], new[] { 512 }, new[] { 512 })]
		[InlineData(new[] { 256 }, new[] { 512 }, new[] { 256, 512 })]
		[InlineData(new[] { 256, 512, 512 }, new[] { 512 }, new[] { 256, 512, 512, 512 })]
		[InlineData(new int[0], new[] { 128, 512 }, new[] { 128, 512 })]
		[InlineData(new[] { 256 }, new[] { 128, 512 }, new[] { 256, 128, 512 })]
		[InlineData(new[] { 256, 512, 512 }, new[] { 128, 512 }, new[] { 256, 512, 512, 128, 512 })]
		[InlineData(new[] { 256, 512, 512, 64, 32, 1024, 16384, 32768, 2, 4, 16 }, new[] { 128, 512, 2048, 4096, 8 },
			new[] { 256, 512, 512, 64, 32, 1024, 16384, 32768, 2, 4, 16, 128, 512, 2048, 4096, 8 })]
		public void AddRangeTest(int[] initial, int[] values, int[] expected)
		{
			using (NativeList<int> nativeList = new(initial))
			{
				AssertUtils.SequenceEqual(initial, nativeList);
				nativeList.AddRange(values);
				AssertUtils.SequenceEqual(expected, nativeList);
			}
		}

		[Theory]
		[InlineData(new int[0], 512, 0, new[] { 512 })]
		[InlineData(new[] { 256 }, 512, 0, new[] { 512, 256 })]
		[InlineData(new[] { 256, 512, 512 }, 512, 0, new[] { 512, 256, 512, 512 })]
		public void InsertTest(int[] initial, int value, int index, int[] expected)
		{
			using (NativeList<int> nativeList = new(initial))
			{
				AssertUtils.SequenceEqual(initial, nativeList);
				nativeList.Insert(index, value);
				AssertUtils.SequenceEqual(expected, nativeList);
			}
			using (NativeList<int> nativeList = new(initial))
			{
				AssertUtils.SequenceEqual(initial, nativeList);
				// ReSharper disable once HeapView.BoxingAllocation
				nativeList.Insert(index, (object)value);
				AssertUtils.SequenceEqual(expected, nativeList);
			}
		}

		[Theory]
		[InlineData(new int[0], new[] { 512 }, 0, new[] { 512 })]
		[InlineData(new[] { 256 }, new[] { 512 }, 0, new[] { 512, 256 })]
		[InlineData(new[] { 256, 512, 512 }, new[] { 512 }, 0, new[] { 512, 256, 512, 512 })]
		[InlineData(new int[0], new[] { 128, 512 }, 0, new[] { 128, 512 })]
		[InlineData(new[] { 256 }, new[] { 128, 512 }, 0, new[] { 128, 512, 256 })]
		[InlineData(new[] { 256, 512, 512 }, new[] { 128, 512 }, 0, new[] { 128, 512, 256, 512, 512 })]
		[InlineData(new[] { 256, 512, 512, 64, 32, 1024, 16384, 32768, 2, 4, 16 }, new[] { 128, 512, 2048, 4096, 8 }, 0,
			new[] { 128, 512, 2048, 4096, 8, 256, 512, 512, 64, 32, 1024, 16384, 32768, 2, 4, 16 })]
		public void InsertRangeTest(int[] initial, int[] values, int index, int[] expected)
		{
			using (NativeList<int> nativeList = new(initial))
			{
				AssertUtils.SequenceEqual(initial, nativeList);
				nativeList.InsertRange(index, values);
				AssertUtils.SequenceEqual(expected, nativeList);
			}
		}

		[Theory]
		[InlineData(new[] { 256 }, 0, new int[0])]
		[InlineData(new[] { 256, 512, 1024 }, 0, new[] { 512, 1024 })]
		[InlineData(new[] { 256, 512, 1024 }, 1, new[] { 256, 1024 })]
		[InlineData(new[] { 256, 512, 1024 }, 2, new[] { 256, 512 })]
		public void RemoveAtTest(int[] initial, int index, int[] expected)
		{
			using (NativeList<int> nativeList = new(initial))
			{
				AssertUtils.SequenceEqual(initial, nativeList);
				nativeList.RemoveAt(index);
				AssertUtils.SequenceEqual(expected, nativeList);
			}
		}

		[Theory]
		[InlineData(new[] { 256, 512, 512, 128, 512 }, 4, 1, new[] { 256, 512, 512, 128 })]
		[InlineData(new[] { 256, 512, 512, 128, 512 }, 0, 1, new[] { 512, 512, 128, 512 })]
		[InlineData(new[] { 256, 512, 512, 128, 512 }, 4, 2, new[] { 256, 512, 512 })]
		[InlineData(new[] { 256, 512, 512, 128, 512 }, 0, 2, new[] { 512, 128, 512 })]
		[InlineData(new[] { 256, 512, 512, 128, 512 }, 4, 0, new[] { 256, 512, 512, 128, 512 })]
		[InlineData(new[] { 256, 512, 512, 128, 512 }, 0, 0, new[] { 256, 512, 512, 128, 512 })]
		[InlineData(new[] { 256, 512, 512, 64, 32, 1024, 16384, 32768, 2, 4, 16, 128, 512, 2048, 4096, 8 }, 5, 5,
			new[] { 256, 512, 512, 64, 4, 16, 128, 512, 2048, 4096, 8 })]
		public void RemoveRangeTest(int[] initial, int index, int count, int[] expected)
		{
			using (NativeList<int> nativeList = new(initial))
			{
				AssertUtils.SequenceEqual(initial, nativeList);
				nativeList.RemoveRange(index, count);
				Debug.WriteLine(string.Join(' ', nativeList));
				AssertUtils.SequenceEqual(expected, nativeList);
			}
		}

		[Theory]
		[InlineData(new[] { 256 }, 256, new int[0])]
		[InlineData(new[] { 256, 512, 1024 }, 256, new[] { 512, 1024 })]
		[InlineData(new[] { 256, 512, 1024 }, 512, new[] { 256, 1024 })]
		[InlineData(new[] { 256, 512, 1024 }, 1024, new[] { 256, 512 })]
		public void RemoveTest(int[] initial, int value, int[] expected)
		{
			using (NativeList<int> nativeList = new(initial))
			{
				AssertUtils.SequenceEqual(initial, nativeList);
				nativeList.Remove(value);
				AssertUtils.SequenceEqual(expected, nativeList);
			}
			using (NativeList<int> nativeList = new(initial))
			{
				AssertUtils.SequenceEqual(initial, nativeList);
				// ReSharper disable once HeapView.BoxingAllocation
				nativeList.Remove((object)value);
				AssertUtils.SequenceEqual(expected, nativeList);
			}
		}

		[Theory]
		[InlineData(new int[0])]
		[InlineData(new[] { 256 })]
		[InlineData(new[] { 256, 512 })]
		[InlineData(new[] { 256, 512, 1024 })]
		public void ClearTest(int[] initial)
		{
			using (NativeList<int> nativeList = new(initial))
			{
				AssertUtils.SequenceEqual(initial, nativeList);
				nativeList.Clear();
				AssertUtils.SequenceEqual(Array.Empty<int>(), nativeList);
			}
		}

		[Theory]
		[InlineData(new int[0], 512, -1)]
		[InlineData(new[] { 256 }, 256, 0)]
		[InlineData(new[] { 256, 512 }, 256, 0)]
		[InlineData(new[] { 256, 512 }, 512, 1)]
		[InlineData(new[] { 256, 512, 1024 }, 512, 1)]
		[InlineData(new[] { 256, 512, 1024 }, 1024, 2)]
		[InlineData(new[] { 256, 512, 1024 }, 128, -1)]
		public void IndefOfContainsTest(int[] initial, int value, int index)
		{
			using (NativeList<int> nativeList = new(initial))
			{
				AssertUtils.SequenceEqual(initial, nativeList);
				Assert.Equal(index, nativeList.IndexOf(value));
				// ReSharper disable once HeapView.BoxingAllocation
				Assert.Equal(index, nativeList.IndexOf((object)value));
				Assert.Equal(index != -1, nativeList.Contains(value));
				// ReSharper disable once HeapView.BoxingAllocation
				Assert.Equal(index != -1, nativeList.Contains((object)value));
			}
		}

		[Fact]
		public void SpanTest()
		{
			uint[] array = new uint[1024];
			array.AsSpan().Fill(uint.MaxValue);
			using (NativeList<uint> nativeList = new(array))
			{
				Assert.Equal(nativeList.Count, nativeList.AsSpan().Length);
				Assert.Equal(nativeList.Count, nativeList.AsReadOnlySpan().Length);
				AssertUtils.SequenceEqual(nativeList.AsSpan(), new ReadOnlySpan<uint>(nativeList.Data, nativeList.Count));
				AssertUtils.SequenceEqual(nativeList.AsReadOnlySpan(), new ReadOnlySpan<uint>(nativeList.Data, nativeList.Count));
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(128)]
		public void ToArrayListTest(int length)
		{
			uint[] array = new uint[length];
			array.AsSpan().Fill(uint.MaxValue);
			using (NativeList<uint> nativeList = new(array))
			{
				AssertUtils.SequenceEqual(array, nativeList);
				AssertUtils.SequenceEqual(nativeList, nativeList.ToArray());
				AssertUtils.SequenceEqual(nativeList, nativeList.ToList());
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(128)]
		public void EnumerableTest(int length)
		{
			uint[] array = new uint[length];
			array.AsSpan().Fill(uint.MaxValue);
			using (NativeList<uint> nativeList = new(array))
			{
				// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
				Assert.True(nativeList.All(v => v == uint.MaxValue));
				// ReSharper disable once UseCollectionCountProperty
				Assert.Equal(nativeList.Count, nativeList.Count());
			}
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(128)]
		public void CopyToTest(int length)
		{
			uint[] array = new uint[length];
			array.AsSpan().Fill(uint.MaxValue);
			using (NativeList<uint> nativeList = new(array))
			{
				uint[] array1 = new uint[nativeList.Count];
				nativeList.CopyTo(array1);
				AssertUtils.SequenceEqual(nativeList, array1);
				uint[] array2 = new uint[nativeList.Count];
				nativeList.CopyTo(array2, 0);
				AssertUtils.SequenceEqual(nativeList, array2);
				Array array3 = new uint[nativeList.Count];
				nativeList.CopyTo(array3, 0);
				AssertUtils.SequenceEqual(nativeList, array3.ToArray<uint>());
				// ReSharper disable once UseArrayCreationExpression.1
				Array array4 = Array.CreateInstance(typeof(uint), nativeList.Count);
				nativeList.CopyTo(array4, 0);
				AssertUtils.SequenceEqual(nativeList, array4.ToArray<uint>());
				Array array5 = Array.CreateInstance(typeof(uint), new[] { nativeList.Count }, new[] { 0 });
				nativeList.CopyTo(array5, 0);
				AssertUtils.SequenceEqual(nativeList, array5.ToArray<uint>());
				uint[] array6 = new uint[nativeList.Count];
				Assert.True(nativeList.TryCopyTo(array6));
				AssertUtils.SequenceEqual(nativeList, array6);
				uint[] array7 = new uint[nativeList.Count / 2];
				Assert.Equal(nativeList.Count <= array7.Length, nativeList.TryCopyTo(array7));
			}
		}
	}
}
