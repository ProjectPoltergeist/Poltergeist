using Poltergeist.Core.Math;
using System;
using System.Linq;
using Xunit;

namespace Poltergeist.Core.Tests
{
	public static class EnumUtilsTests
	{
		// ReSharper disable UnusedMember.Local
		private enum TestEnum
		{
			A = 1,
			B = 2,
			C = 4,
			D = A,
			E = B,
			F = 3
		}

		[Flags]
		public enum TestFlags
		{
			A = 1 << 0,
			B = 1 << 1,
			C = 1 << 2,
			D = 1 << 3,
			E = 1 << 4,
			F = 1 << 5
		}
		// ReSharper restore UnusedMember.Local

		[Fact]
		public static void ValuesTest()
		{
			Assert.True(EnumUtils<TestEnum>.Values.ToArray().SequenceEqual(Enum.GetValues<TestEnum>()));
		}

		[Fact]
		public static void NamesTest()
		{
			Assert.True(EnumUtils<TestEnum>.Names.ToArray().SequenceEqual(Enum.GetNames<TestEnum>()));
		}

		[Fact]
		public static void TypeTest()
		{
			Assert.Equal(Enum.GetUnderlyingType(typeof(TestEnum)), EnumUtils<TestEnum>.UnderlyingType);
		}

		[Theory]
		[InlineData((TestFlags)0, TestFlags.C)]
		[InlineData(TestFlags.A, TestFlags.C)]
		[InlineData(TestFlags.C, TestFlags.C)]
		[InlineData(TestFlags.A | TestFlags.B, TestFlags.C)]
		[InlineData(TestFlags.A | TestFlags.C, TestFlags.C)]
		public static void HasFlagTest(TestFlags value, TestFlags flag)
		{
			// ReSharper disable HeapView.BoxingAllocation
			Assert.Equal(value.HasFlag(flag), EnumUtils<TestFlags>.HasFlag(value, flag));
			Assert.Equal(value.HasFlag(flag), value.HasFlagF(flag));
			// ReSharper restore HeapView.BoxingAllocation
		}
	}
}
