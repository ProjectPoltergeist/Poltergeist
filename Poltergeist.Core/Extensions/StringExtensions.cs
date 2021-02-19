using System;

namespace Poltergeist.Core.Extensions
{
	public static class StringExtensions
	{
		public static LineSplitEnumerator SplitLines(this ReadOnlySpan<char> str)
		{
			return new(str);
		}

		public ref struct LineSplitEnumerator
		{
			private ReadOnlySpan<char> _data;
			private bool _endOnEmpty;

			public ReadOnlySpan<char> Current { get; private set; }

			public LineSplitEnumerator(ReadOnlySpan<char> data)
			{
				_data = data;
				_endOnEmpty = false;
				Current = default;
			}

			public bool MoveNext()
			{
				if (_data.Length == 0)
				{
					if (_endOnEmpty)
						return false;
					Current = ReadOnlySpan<char>.Empty;
					_endOnEmpty = true;
					return true;
				}

				int index = _data.IndexOfAny('\r', '\n');
				if (index == -1)
				{
					Current = _data;
					_data = ReadOnlySpan<char>.Empty;
					_endOnEmpty = true;
					return true;
				}

				Current = _data.Slice(0, index);
				_data = _data.Slice(index + (_data[index] == '\r' && index < _data.Length - 1 && _data[index + 1] == '\n' ? 2 : 1));
				_endOnEmpty = false;
				return true;
			}

			public LineSplitEnumerator GetEnumerator() => this;
		}
	}
}
