using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Poltergeist.Core.Tests
{
	public static class AssertUtils
	{
		public static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
		{
			if (expected is IReadOnlyList<T> list1 && actual is IReadOnlyList<T> list2)
				Assert.Equal(list1.Count, list2.Count);
			// ReSharper disable PossibleMultipleEnumeration
			Assert.Equal(expected.Count(), actual.Count());
			Assert.True(expected.SequenceEqual(actual));
			// ReSharper restore PossibleMultipleEnumeration
		}

		public static void SequenceEqual<T>(ReadOnlySpan<T> expected, ReadOnlySpan<T> actual) where T : IEquatable<T>
		{
			Assert.Equal(expected.Length, actual.Length);
			Assert.True(expected.SequenceEqual(actual));
		}
	}
}
