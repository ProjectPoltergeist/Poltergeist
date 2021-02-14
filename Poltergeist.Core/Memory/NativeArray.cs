using Poltergeist.Core.Math;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;

// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Poltergeist.Core.Memory
{
	public sealed unsafe class NativeArray<T> : MemoryManager<T>, IReadOnlyList<T>, IDisposable where T : unmanaged
	{
		// ReSharper disable StaticMemberInGenericType
		public static readonly int MaxSize = int.MaxValue / sizeof(T);
		public static readonly int DefaultMemoryAlignment = System.Math.Max(System.Math.Max(16, sizeof(T)), MathUtils.Align(Vector<byte>.Count, 16));
		public static readonly INativeAllocator DefaultAllocator = new CoTaskMemAllocator();
		// ReSharper restore StaticMemberInGenericType

		public readonly T* Data;
		public readonly int Count;
		public readonly int ByteSize;

		internal readonly INativeAllocator Allocator;
		internal readonly int AlignedSize;

		private readonly object _freeLock = new();
		private readonly bool _zeroOnFree;

#if DEBUG
		private readonly string _allocationStacktrace;
#endif

		private bool _valid;

		int IReadOnlyCollection<T>.Count => Count;

		public NativeArray(ReadOnlySpan<T> data, bool zeroOnFree = false, INativeAllocator allocator = null)
			: this(data, DefaultMemoryAlignment, zeroOnFree, allocator)
		{
		}

		public NativeArray(ReadOnlySpan<T> data, int alignment, bool zeroOnFree = false, INativeAllocator allocator = null)
			: this(data.Length, alignment, false, zeroOnFree, allocator)
		{
			data.CopyTo(AsSpan());
		}

		public NativeArray(int size, bool zeroMemory = true, bool zeroOnFree = false, INativeAllocator allocator = null)
			: this(size, DefaultMemoryAlignment, zeroMemory, zeroOnFree, allocator)
		{
		}

		public NativeArray(int size, int alignment, bool zeroMemory = true, bool zeroOnFree = false, INativeAllocator allocator = null)
		{
			if (size < 0)
				throw new ArgumentOutOfRangeException(nameof(size), size, "Allocation size must be positive");
			if (alignment < sizeof(T))
				throw new ArgumentOutOfRangeException(nameof(alignment), alignment, "Alignment must be at least the element size");
			if (size > MaxSize)
				throw new ArgumentOutOfRangeException(nameof(size), size, "Allocation size must be below 2GB");
			Count = size;
			ByteSize = size * sizeof(T);
			Allocator = allocator ?? DefaultAllocator;
			AlignedSize = ByteSize == 0 ? 0 : (int)System.Math.Min((uint)MathUtils.Align(ByteSize, alignment), int.MaxValue);
			Data = (T*)Allocator.Allocate(AlignedSize).ToPointer();
			_valid = Data != null;

			if (AlignedSize == 0)
				return;
			GC.AddMemoryPressure(AlignedSize);
			if (zeroMemory)
				Clear();
			_zeroOnFree = zeroOnFree;

#if DEBUG
			_allocationStacktrace = Environment.StackTrace;
#endif
		}

		public T this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (index >= Count || index < 0)
					ThrowHelper.IndexOutOfRange();
				return Data[index];
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				if (index >= Count || index < 0)
					ThrowHelper.IndexOutOfRange();
				Data[index] = value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTo(Span<T> destination)
		{
			AsReadOnlySpan().CopyTo(destination);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryCopyTo(Span<T> destination)
		{
			return AsReadOnlySpan().TryCopyTo(destination);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Fill(T value)
		{
			AsSpan().Fill(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear()
		{
			AsSpan().Clear();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> AsSpan()
		{
			return new(Data, Count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<T> AsReadOnlySpan()
		{
			return new(Data, Count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] ToArray()
		{
			if (Count == 0)
				return Array.Empty<T>();

			T[] destination = new T[Count];
			CopyTo(destination);
			return destination;
		}

		#region MemoryOwner
		protected override void Dispose(bool disposing) => Dispose();
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override Span<T> GetSpan() => AsSpan();
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override MemoryHandle Pin(int elementIndex = 0) => new(Data + elementIndex, pinnable: this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override void Unpin() { }

		protected override bool TryGetArray(out ArraySegment<T> segment)
		{
			segment = default;
			return false;
		}
		#endregion

		#region IEnumerable
		public NativeArrayEnumerator GetEnumerator() => new(this);
		// ReSharper disable HeapView.BoxingAllocation
		IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		// ReSharper restore HeapView.BoxingAllocation
		#endregion

		#region IDisposeable
		private void Free()
		{
			lock (_freeLock)
			{
				if (!_valid)
					return;
				if (_zeroOnFree)
					Clear();
				Allocator.Free(new IntPtr(Data));
				if (AlignedSize > 0)
					GC.RemoveMemoryPressure(AlignedSize);
				_valid = false;
			}
		}

		public void Dispose()
		{
			Free();
			GC.SuppressFinalize(this);
		}

#pragma warning disable CA2015
		~NativeArray()
		{
#if DEBUG
			Debug.WriteLine($"NativeArray leaked to the GC, this might lead to use after free with spans. Allocation stacktrace: {_allocationStacktrace}");
#endif
			Free();
		}
#pragma warning restore CA2015
		#endregion

		public struct NativeArrayEnumerator : IEnumerator<T>
		{
			private readonly NativeArray<T> _array;
			private int _currentIndex;

			public T Current { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] private set; }
			// ReSharper disable once HeapView.BoxingAllocation
			object IEnumerator.Current => Current;

			internal NativeArrayEnumerator(NativeArray<T> array)
			{
				_array = array;
				_currentIndex = -1;
				Current = default;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				if (++_currentIndex >= _array.Count)
					return false;
				Current = _array.Data[_currentIndex];
				return true;
			}

			public void Reset() => _currentIndex = -1;
			public void Dispose() { }
		}

		[Pure]
		private static class ThrowHelper
		{
			public static void IndexOutOfRange() => throw new IndexOutOfRangeException();
		}
	}
}
