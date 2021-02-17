using Poltergeist.Core.Collections;
using System.Linq;
using Xunit;

namespace Poltergeist.Core.Tests
{
	public class EnumerableUtilsTests
	{
		[Fact]
		public void JoinTest()
		{
			Assert.True(EnumerableUtils.Join(new[] { 1, 2, 3 }, new[] { 4, 5 }, new[] { 6, 7, 8 }, new[] { 9 }).SequenceEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }));
			Assert.True(EnumerableUtils.Join(new[] { "a", "b", "c" }, new[] { "d", "e" }, new[] { "f", "g", "h" }, new[] { "i" }).SequenceEqual(new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i" }));
		}
	}
}
