using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UniRx
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct DictionaryReplaceEvent<TKey, TValue> : IEquatable<DictionaryReplaceEvent<TKey, TValue>>
	{
		public DictionaryReplaceEvent(TKey key, TValue oldValue, TValue newValue)
		{
			this = default(DictionaryReplaceEvent<TKey, TValue>);
			this.Key = key;
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		public TKey Key { get; private set; }

		public TValue OldValue { get; private set; }

		public TValue NewValue { get; private set; }

		public override string ToString()
		{
			return string.Format("Key:{0} OldValue:{1} NewValue:{2}", this.Key, this.OldValue, this.NewValue);
		}

		public override int GetHashCode()
		{
			return EqualityComparer<TKey>.Default.GetHashCode(this.Key) ^ EqualityComparer<TValue>.Default.GetHashCode(this.OldValue) << 2 ^ EqualityComparer<TValue>.Default.GetHashCode(this.NewValue) >> 2;
		}

		public bool Equals(DictionaryReplaceEvent<TKey, TValue> other)
		{
			return EqualityComparer<TKey>.Default.Equals(this.Key, other.Key) && EqualityComparer<TValue>.Default.Equals(this.OldValue, other.OldValue) && EqualityComparer<TValue>.Default.Equals(this.NewValue, other.NewValue);
		}
	}
}
