using Poltergeist.Core.Memory;
using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Poltergeist.Core.Tests
{
	public class NativeStructTests
	{
		[Theory]
		[InlineData(0, null, null, 0)]
		[InlineData(int.MaxValue, "abcde", "fghij", int.MaxValue)]
		[InlineData(-1, "abc\tde", "fgh\tij", -1)]
		public unsafe void DataTest(int i, string a, string b, int p)
		{
			TestStruct ts = new() { a = a, b = b, i = i, p = new IntPtr(p) };
			using (NativeStruct<TestStruct> nativeStruct = new(ts))
			{
				Assert.True(nativeStruct.Data != null);
				Assert.NotNull(nativeStruct.Allocator);
				Assert.Equal(Marshal.SizeOf<TestStruct>(), nativeStruct.Size);
				Assert.Equal(ts.i, *(int*)nativeStruct.Data);
				Assert.Equal(ts, Marshal.PtrToStructure<TestStruct>(new IntPtr(nativeStruct.Data)));
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct TestStruct : IEquatable<TestStruct>
		{
			public int i;
			[MarshalAs(UnmanagedType.LPUTF8Str)]
			public string a;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string b;
			public IntPtr p;

			public bool Equals(TestStruct other)
			{
				return i == other.i && a == other.a && b == other.b && p.Equals(other.p);
			}

			public override bool Equals(object obj)
			{
				return obj is TestStruct other && Equals(other);
			}

			public override int GetHashCode()
			{
				// ReSharper disable NonReadonlyMemberInGetHashCode
				return HashCode.Combine(i, a, b, p);
				// ReSharper restore NonReadonlyMemberInGetHashCode
			}

			public static bool operator ==(TestStruct left, TestStruct right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(TestStruct left, TestStruct right)
			{
				return !left.Equals(right);
			}

			public override string ToString()
			{
				// ReSharper disable HeapView.BoxingAllocation
				return $"({i}) ({a}) ({b}) ({p})";
				// ReSharper restore HeapView.BoxingAllocation
			}
		}
	}
}
