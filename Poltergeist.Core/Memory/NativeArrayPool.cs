// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Code is a modified version of TlsOverPerCoreLockedStacksArrayPool from .NET 5

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Poltergeist.Core.Memory
{
	public static class NativeArrayPool<T> where T : unmanaged
	{
		private const int NumBuckets = 17;
		private const int MaxPerCorePerArraySizeStacks = 64;
		private const int MaxBuffersPerArraySizePerCore = 8;

		private static readonly NativeArray<T> Empty = new(0);

		// ReSharper disable once StaticMemberInGenericType
		private static readonly int[] BucketArraySizes;
		private static readonly PerCoreLockedStacks[] Buckets = new PerCoreLockedStacks[NumBuckets];

		[ThreadStatic]
		private static NativeArray<T>[] _tTlsBuckets;

		static NativeArrayPool()
		{
			int[] sizes = new int[NumBuckets];
			for (int i = 0; i < sizes.Length; i++)
				sizes[i] = Utilities.GetMaxSizeForBucket(i);
			BucketArraySizes = sizes;
		}

		private static PerCoreLockedStacks CreatePerCoreLockedStacks(int bucketIndex)
		{
			PerCoreLockedStacks stacks = new();
			return Interlocked.CompareExchange(ref Buckets[bucketIndex], stacks, null) ?? stacks;
		}

		public static NativeArray<T> Rent(int minimumLength)
		{
			switch (minimumLength)
			{
				case < 0:
					throw new ArgumentOutOfRangeException(nameof(minimumLength));
				case 0:
					return Empty;
			}

			int bucketIndex = Utilities.SelectBucketIndex(minimumLength);

			if (bucketIndex >= Buckets.Length)
				return new NativeArray<T>(minimumLength);

			NativeArray<T>[] tlsBuckets = _tTlsBuckets;
			if (tlsBuckets != null)
			{
				NativeArray<T> buffer = tlsBuckets[bucketIndex];
				if (buffer != null)
				{
					tlsBuckets[bucketIndex] = null;
					return buffer;
				}
			}

			PerCoreLockedStacks bucket = Buckets[bucketIndex];
			if (bucket != null)
			{
				NativeArray<T> buffer = bucket.TryPop();
				if (buffer != null)
					return buffer;
			}

			return new NativeArray<T>(BucketArraySizes[bucketIndex]);
		}

		public static void Return(NativeArray<T> nativeArray, bool clearArray = false)
		{
			if (nativeArray == null)
				throw new ArgumentNullException(nameof(nativeArray));

			int bucketIndex = Utilities.SelectBucketIndex(nativeArray.Count);

			if (bucketIndex < Buckets.Length)
			{
				if (clearArray)
					nativeArray.Clear();

				if (nativeArray.Count != BucketArraySizes[bucketIndex])
					throw new ArgumentException("Memory not from pool", nameof(nativeArray));

				NativeArray<T>[] tlsBuckets = _tTlsBuckets;
				if (tlsBuckets == null)
				{
					_tTlsBuckets = tlsBuckets = new NativeArray<T>[NumBuckets];
					tlsBuckets[bucketIndex] = nativeArray;
				}
				else
				{
					NativeArray<T> prev = tlsBuckets[bucketIndex];
					tlsBuckets[bucketIndex] = nativeArray;

					if (prev != null)
					{
						PerCoreLockedStacks stackBucket = Buckets[bucketIndex] ?? CreatePerCoreLockedStacks(bucketIndex);
						stackBucket.TryPush(prev);
					}
				}
			}
		}

		private sealed class PerCoreLockedStacks
		{
			private readonly LockedStack[] _perCoreStacks;

			public PerCoreLockedStacks()
			{
				LockedStack[] stacks = new LockedStack[System.Math.Min(Environment.ProcessorCount, MaxPerCorePerArraySizeStacks)];
				for (int i = 0; i < stacks.Length; i++)
					stacks[i] = new LockedStack();
				_perCoreStacks = stacks;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void TryPush(NativeArray<T> array)
			{
				LockedStack[] stacks = _perCoreStacks;
				int index = Thread.GetCurrentProcessorId() % stacks.Length;
				for (int i = 0; i < stacks.Length; i++)
				{
					if (stacks[index].TryPush(array))
						return;
					if (++index == stacks.Length)
						index = 0;
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public NativeArray<T> TryPop()
			{
				LockedStack[] stacks = _perCoreStacks;
				int index = Thread.GetCurrentProcessorId() % stacks.Length;
				for (int i = 0; i < stacks.Length; i++)
				{
					NativeArray<T> arr = stacks[index].TryPop();
					if (arr != null)
						return arr;
					if (++index == stacks.Length)
						index = 0;
				}
				return null;
			}
		}

		private sealed class LockedStack
		{
			private readonly NativeArray<T>[] _arrays = new NativeArray<T>[MaxBuffersPerArraySizePerCore];
			private int _count;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool TryPush(NativeArray<T> array)
			{
				bool enqueued = false;
				Monitor.Enter(this);
				if (_count < MaxBuffersPerArraySizePerCore)
				{
					_arrays[_count++] = array;
					enqueued = true;
				}
				Monitor.Exit(this);
				return enqueued;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public NativeArray<T> TryPop()
			{
				NativeArray<T> nativeArray = null;
				Monitor.Enter(this);
				if (_count > 0)
				{
					nativeArray = _arrays[--_count];
					_arrays[_count] = null;
				}
				Monitor.Exit(this);
				return nativeArray;
			}
		}

		private static class Utilities
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static int SelectBucketIndex(int bufferSize) => BitOperations.Log2((uint)bufferSize - 1 | 15) - 3;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static int GetMaxSizeForBucket(int binIndex) => 16 << binIndex;
		}
	}
}
