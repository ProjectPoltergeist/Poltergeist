﻿using System.Collections.Generic;

namespace Poltergeist.Core.Collections
{
	public static class EnumerableUtils
	{
		public static IEnumerable<T> Join<T>(params IEnumerable<T>[] enumerables)
		{
			foreach (IEnumerable<T> enumerable in enumerables)
				foreach (T element in enumerable)
					yield return element;
		}
	}
}
