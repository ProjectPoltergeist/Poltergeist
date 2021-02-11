using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Poltergeist.Core
{
	public sealed unsafe class NativeArray<T> : MemoryManager<T>, IDisposable, IReadOnlyList<T> where T : unmanaged
	{
		public static readonly int DefaultMemoryAlignment = Math.Max(Math.Max(16, sizeof(T)), MathUtils.Align(Vector<byte>.Count, 16));

		public readonly T* Data;
		public readonly int Count;
		public readonly int ByteSize;

		private readonly object _freeLock = new();
		private readonly bool _zeroOnFree;
		private readonly int _alignedSize;

		private bool _valid;

		int IReadOnlyCollection<T>.Count => Count;

		public NativeArray(int size, bool zeroMemory = true, bool zeroOnFree = false) : this(size, DefaultMemoryAlignment, zeroMemory, zeroOnFree) { }

		public NativeArray(int size, int alignment, bool zeroMemory = true, bool zeroOnFree = false)
		{
			if (size < 0)
				throw new ArgumentOutOfRangeException(nameof(size), size, "Allocation size must be positive");
			if (alignment < sizeof(T))
				throw new ArgumentOutOfRangeException(nameof(alignment), alignment, "Alignment must be at least the element size");
			Count = size;
			ByteSize = size * sizeof(T);
			_alignedSize = ByteSize == 0 ? 0 : MathUtils.Align(ByteSize, alignment);
			Data = (T*)Marshal.AllocCoTaskMem(_alignedSize).ToPointer();
			_valid = Data != null;

			if (_alignedSize == 0)
				return;
			GC.AddMemoryPressure(_alignedSize);
			if (zeroMemory)
				Clear();
			_zeroOnFree = zeroOnFree;
		}

		public T this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (index >= Count)
					ThrowHelper.IndexOutOfRange();
				return Data[index];
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				if (index >= Count)
					ThrowHelper.IndexOutOfRange();
				Data[index] = value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTo(Span<T> destination)
		{
			AsSpan().CopyTo(destination);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryCopyTo(Span<T> destination)
		{
			return AsSpan().TryCopyTo(destination);
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

		private void Free()
		{
			lock (_freeLock)
			{
				if (!_valid)
					return;
				if (_zeroOnFree)
					Clear();
				Marshal.FreeCoTaskMem(new IntPtr(Data));
				if (_alignedSize > 0)
					GC.RemoveMemoryPressure(_alignedSize);
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
			Free();
		}
#pragma warning restore CA2015

		public NativeMemoryEnumerator GetEnumerator() => new(this);
		// ReSharper disable HeapView.BoxingAllocation
		IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		// ReSharper restore HeapView.BoxingAllocation

		public struct NativeMemoryEnumerator : IEnumerator<T>
		{
			private readonly NativeArray<T> _array;
			private int _currentIndex;

			public T Current { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] private set; }
			// ReSharper disable once HeapView.BoxingAllocation
			object IEnumerator.Current => Current;

			public NativeMemoryEnumerator(NativeArray<T> array)
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
