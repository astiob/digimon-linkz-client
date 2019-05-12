using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UniRx
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct CollectionReplaceEvent<T> : IEquatable<CollectionReplaceEvent<T>>
	{
		public CollectionReplaceEvent(int index, T oldValue, T newValue)
		{
			this = default(CollectionReplaceEvent<T>);
			this.Index = index;
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		public int Index { get; private set; }

		public T OldValue { get; private set; }

		public T NewValue { get; private set; }

		public override string ToString()
		{
			return string.Format("Index:{0} OldValue:{1} NewValue:{2}", this.Index, this.OldValue, this.NewValue);
		}

		public override int GetHashCode()
		{
			return this.Index.GetHashCode() ^ EqualityComparer<T>.Default.GetHashCode(this.OldValue) << 2 ^ EqualityComparer<T>.Default.GetHashCode(this.NewValue) >> 2;
		}

		public bool Equals(CollectionReplaceEvent<T> other)
		{
			return this.Index.Equals(other.Index) && EqualityComparer<T>.Default.Equals(this.OldValue, other.OldValue) && EqualityComparer<T>.Default.Equals(this.NewValue, other.NewValue);
		}
	}
}
