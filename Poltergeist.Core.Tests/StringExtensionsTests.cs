using Poltergeist.Core.Extensions;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Poltergeist.Core.Tests
{
	public class StringExtensionsTests
	{
		[Theory]
		[InlineData("", new[] { "" })]
		[InlineData("ab", new[] { "ab" })]
		[InlineData("a\rb", new[] { "a", "b" })]
		[InlineData("a\nb", new[] { "a", "b" })]
		[InlineData("a\r\nb", new[] { "a", "b" })]
		[InlineData("\r", new[] { "", "" })]
		[InlineData("ab\r", new[] { "ab", "" })]
		[InlineData("\n", new[] { "", "" })]
		[InlineData("ab\n", new[] { "ab", "" })]
		[InlineData("\r\n", new[] { "", "" })]
		[InlineData("ab\r\n", new[] { "ab", "" })]
		[InlineData("a\rb\nc", new[] { "a", "b", "c" })]
		[InlineData("a\nb\rc", new[] { "a", "b", "c" })]
		[InlineData("a\r\nb\rc\nd", new[] { "a", "b", "c", "d" })]
		[InlineData("a\rb\nc\r\n", new[] { "a", "b", "c", "" })]
		[InlineData("a\nb\rc\r\n", new[] { "a", "b", "c", "" })]
		[InlineData("a\r\nb\rc\nd\r\n", new[] { "a", "b", "c", "d", "" })]
		public void SplitTest(string value, string[] expected)
		{
			int i = 0;
			foreach (ReadOnlySpan<char> line in value.AsSpan().SplitLines())
			{
				Assert.True(line.Equals(expected[i], StringComparison.Ordinal), $"\"{expected[i]}\" == \"{line.ToString()}\"");
				i++;
			}
			Assert.Equal(expected.Length, i);
		}
	}
}
