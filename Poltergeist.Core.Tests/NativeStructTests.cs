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
		public unsafe void DataTest(int integer, string utf8String, string wideString, int ptr)
		{
			TestStruct testStruct = new() { integer = integer, utf8String = utf8String, wideString = wideString, ptr = new IntPtr(ptr) };
			using (NativeStruct<TestStruct> nativeStruct = new(testStruct))
			{
				Assert.True(nativeStruct.Data != null);
				Assert.NotNull(nativeStruct.Allocator);
				Assert.Equal(Marshal.SizeOf<TestStruct>(), nativeStruct.Size);
				Assert.Equal(testStruct.integer, *(int*)nativeStruct.Data);
				Assert.Equal(testStruct, Marshal.PtrToStructure<TestStruct>(new IntPtr(nativeStruct.Data)));
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct TestStruct : IEquatable<TestStruct>
		{
			public int integer;
			[MarshalAs(UnmanagedType.LPUTF8Str)]
			public string utf8String;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string wideString;
			public IntPtr ptr;

			public bool Equals(TestStruct other)
			{
				return integer == other.integer && utf8String == other.utf8String && wideString == other.wideString && ptr == other.ptr;
			}

			public override bool Equals(object obj)
			{
				return obj is TestStruct other && Equals(other);
			}

			public override int GetHashCode()
			{
				// ReSharper disable NonReadonlyMemberInGetHashCode
				return HashCode.Combine(integer, utf8String, wideString, ptr);
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
				return $"({integer}) ({utf8String}) ({wideString}) ({ptr})";
				// ReSharper restore HeapView.BoxingAllocation
			}
		}
	}
}
