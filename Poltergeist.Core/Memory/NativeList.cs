using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Poltergeist.Core.Memory
{
	public sealed unsafe class NativeList<T> : IList<T>, IReadOnlyList<T>, IList, IDisposable where T : unmanaged, IEquatable<T>
	{
		private const int DefaultCapacity = 8;

		// ReSharper disable StaticMemberInGenericType
		public static readonly int MaxSize = int.MaxValue / sizeof(T);
		public static readonly INativeAllocator DefaultAllocator = new CoTaskMemAllocator();
		// ReSharper restore StaticMemberInGenericType

		internal readonly INativeAllocator Allocator;

		private readonly object _freeLock = new();
		private readonly bool _zeroOnFree;

#if DEBUG
		private readonly string _allocationStacktrace;
#endif

		private bool _valid;
		private int _version;

		public T* Data { get; private set; }
		public int Count { get; private set; }
		public int Capacity { get; private set; }

		public bool IsSynchronized => false;
		public object SyncRoot => this;
		public bool IsReadOnly => false;
		public bool IsFixedSize => false;

		public NativeList(ReadOnlySpan<T> data, bool zeroOnFree = false, INativeAllocator allocator = null)
			: this(data.Length, zeroOnFree, allocator)
		{
			data.CopyTo(new Span<T>(Data, Capacity));
		}

		public NativeList(int capacity = DefaultCapacity, bool zeroOnFree = false, INativeAllocator allocator = null)
		{
			if (capacity < 1)
				throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Capacity must be positive");
			if (capacity > MaxSize)
				throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Capacity must be below 2GB");
			Capacity = capacity;
			Allocator = allocator ?? DefaultAllocator;
			Data = (T*)Allocator.Allocate(capacity * sizeof(T)).ToPointer();
			_valid = Data != null;

			GC.AddMemoryPressure(capacity * sizeof(T));
			_zeroOnFree = zeroOnFree;

#if DEBUG
			_allocationStacktrace = Environment.StackTrace;
#endif
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EnsureCapacity(int capacity)
		{
			if (Capacity >= capacity)
				return;
			Resize(capacity);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void Resize(int capacity)
		{
			capacity = System.Math.Max(Capacity * 2, capacity);
			if (capacity > MaxSize || capacity < 1)
				throw new InvalidOperationException();
			int size = capacity * sizeof(T);
			T* newData = (T*)Allocator.Allocate(size).ToPointer();
			GC.AddMemoryPressure(size);
			CopyTo(new Span<T>(newData, Count));
			if (_zeroOnFree)
				AsSpan().Clear();
			Allocator.Free(new IntPtr(Data));
			GC.RemoveMemoryPressure(Capacity * sizeof(T));
			Data = newData;
			Capacity = capacity;
		}

		object IList.this[int index]
		{
			// ReSharper disable once HeapView.BoxingAllocation
			get => this[index];
			set
			{
				if (value is not T v)
					throw new ArgumentException($"Argument is not of type {typeof(T).Name}", nameof(value));
				this[index] = v;
			}
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

		public int Add(object value)
		{
			if (value is not T v)
				throw new ArgumentException($"Argument is not of type {typeof(T).Name}", nameof(value));
			Add(v);
			return Count - 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(T item)
		{
			EnsureCapacity(Count + 1);
			Data[Count++] = item;
			_version++;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddRange(ReadOnlySpan<T> items)
		{
			EnsureCapacity(Count + items.Length);
			items.CopyTo(new Span<T>(Data + Count, Capacity - Count));
			Count += items.Length;
			_version++;
		}

		public void Insert(int index, object value)
		{
			if (value is not T v)
				throw new ArgumentException($"Argument is not of type {typeof(T).Name}", nameof(value));
			Insert(index, v);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Insert(int index, T item)
		{
			if (index >= Count || index < 0)
				ThrowHelper.IndexOutOfRange();
			EnsureCapacity(Count + 1);
			new ReadOnlySpan<T>(Data + index, Count - index).CopyTo(new Span<T>(Data + index + 1, Count - index - 1));
			Data[index] = item;
			Count++;
			_version++;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InsertRange(int index, ReadOnlySpan<T> items)
		{
			if (index >= Count || index < 0)
				ThrowHelper.IndexOutOfRange();
			EnsureCapacity(Count + items.Length);
			new ReadOnlySpan<T>(Data + index, Count - index).CopyTo(new Span<T>(Data + index + items.Length, Count - index - items.Length));
			items.CopyTo(new Span<T>(Data + index, Capacity - index));
			Count += items.Length;
			_version++;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RemoveAt(int index)
		{
			if (index >= Count || index < 0)
				ThrowHelper.IndexOutOfRange();
			_version++;
			if (index == Count - 1)
			{
				Data[--Count] = default;
				return;
			}

			new ReadOnlySpan<T>(Data + index + 1, Count - index - 1).CopyTo(new Span<T>(Data + index, Count - index));
			Data[--Count] = default;
		}

		public void Remove(object value)
		{
			if (value is not T v)
				throw new ArgumentException($"Argument is not of type {typeof(T).Name}", nameof(value));
			Remove(v);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Remove(T item)
		{
			int i = IndexOf(item);
			if (i != -1)
			{
				RemoveAt(i);
				return true;
			}

			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RemoveRange(int index, int count)
		{
			if (index >= Count || index < 0)
				ThrowHelper.IndexOutOfRange();
			if (count < 0 || count > Count - index)
				// ReSharper disable once HeapView.BoxingAllocation
				ThrowHelper.ArgumentOutOfRange("Trying to remove more elements than the collection contains after the index", nameof(count), count);
			new ReadOnlySpan<T>(Data + index + count, Count - index - count).CopyTo(new Span<T>(Data + index, count));
			if (_zeroOnFree)
				new Span<T>(Data + Count - count, count).Clear();
			Count -= count;
			_version++;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear()
		{
			if (_zeroOnFree)
				AsSpan().Clear();
			Count = 0;
		}

		public int IndexOf(object value)
		{
			if (value is not T v)
				throw new ArgumentException($"Argument is not of type {typeof(T).Name}", nameof(value));
			return IndexOf(v);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int IndexOf(T item)
		{
			return AsReadOnlySpan().IndexOf(item);
		}

		public bool Contains(object value)
		{
			if (value is not T v)
				throw new ArgumentException($"Argument is not of type {typeof(T).Name}", nameof(value));
			return Contains(v);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(T item)
		{
			return AsReadOnlySpan().Contains(item);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTo(Array array, int index)
		{
			if (index >= array.Length || index < 0)
				ThrowHelper.IndexOutOfRange();
			if (array is T[] a)
			{
				CopyTo(new Span<T>(a, index, a.Length - index));
				return;
			}

			CopyToSlow(array, index);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void CopyToSlow(Array array, int index)
		{
			if (array.Rank != 1 || array.GetLowerBound(0) != 0)
				throw new ArgumentException("Only copies to single dimensional, zero based arrays are permitted", nameof(array));
			GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
			try
			{
				CopyTo(new Span<T>((T*)handle.AddrOfPinnedObject() + index, array.Length - index));
			}
			finally
			{
				handle.Free();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTo(T[] array, int index)
		{
			if (index >= array.Length || index < 0)
				ThrowHelper.IndexOutOfRange();
			CopyTo(new Span<T>(array, index, array.Length - index));
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public List<T> ToList()
		{
			return new(this);
		}

		// ReSharper disable HeapView.BoxingAllocation
		public NativeListEnumerator GetEnumerator() => new(this);
		IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		// ReSharper restore HeapView.BoxingAllocation

		private void Free()
		{
			lock (_freeLock)
			{
				if (!_valid)
					return;
				if (_zeroOnFree)
					AsSpan().Clear();
				Allocator.Free(new IntPtr(Data));
				GC.RemoveMemoryPressure(Capacity * sizeof(T));
				_valid = false;
			}
		}

		public void Dispose()
		{
			Free();
			GC.SuppressFinalize(this);
		}

		~NativeList()
		{
#if DEBUG
			Debug.WriteLine($"NativeList leaked to the GC, this might lead to use after free with spans. Allocation stacktrace: {_allocationStacktrace}");
#endif
			Free();
		}

		public struct NativeListEnumerator : IEnumerator<T>
		{
			private readonly NativeList<T> _list;
			private readonly int _version;
			private int _currentIndex;

			public T Current { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] private set; }
			// ReSharper disable once HeapView.BoxingAllocation
			object IEnumerator.Current => Current;

			internal NativeListEnumerator(NativeList<T> list)
			{
				_list = list;
				_currentIndex = -1;
				_version = list._version;
				Current = default;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				if (_version != _list._version)
					ThrowHelper.InvalidOperation();
				if (++_currentIndex >= _list.Count)
					return false;
				Current = _list.Data[_currentIndex];
				return true;
			}

			public void Reset()
			{
				if (_version != _list._version)
					ThrowHelper.InvalidOperation();
				_currentIndex = -1;
			}

			public void Dispose() { }
		}

		[Pure]
		private static class ThrowHelper
		{
			public static void IndexOutOfRange() => throw new IndexOutOfRangeException();
			public static void InvalidOperation() => throw new InvalidOperationException();
			public static void ArgumentOutOfRange(string message, string name, object value) => throw new ArgumentOutOfRangeException(name, value, message);
		}
	}
}
