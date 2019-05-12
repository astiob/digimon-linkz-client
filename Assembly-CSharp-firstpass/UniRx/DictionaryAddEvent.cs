using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UniRx
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct DictionaryAddEvent<TKey, TValue> : IEquatable<DictionaryAddEvent<TKey, TValue>>
	{
		public DictionaryAddEvent(TKey key, TValue value)
		{
			this = default(DictionaryAddEvent<TKey, TValue>);
			this.Key = key;
			this.Value = value;
		}

		public TKey Key { get; private set; }

		public TValue Value { get; private set; }

		public override string ToString()
		{
			return string.Format("Key:{0} Value:{1}", this.Key, this.Value);
		}

		public override int GetHashCode()
		{
			return EqualityComparer<TKey>.Default.GetHashCode(this.Key) ^ EqualityComparer<TValue>.Default.GetHashCode(this.Value) << 2;
		}

		public bool Equals(DictionaryAddEvent<TKey, TValue> other)
		{
			return EqualityComparer<TKey>.Default.Equals(this.Key, other.Key) && EqualityComparer<TValue>.Default.Equals(this.Value, other.Value);
		}
	}
}
