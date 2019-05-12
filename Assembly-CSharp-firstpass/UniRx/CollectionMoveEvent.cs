using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UniRx
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct CollectionMoveEvent<T> : IEquatable<CollectionMoveEvent<T>>
	{
		public CollectionMoveEvent(int oldIndex, int newIndex, T value)
		{
			this = default(CollectionMoveEvent<T>);
			this.OldIndex = oldIndex;
			this.NewIndex = newIndex;
			this.Value = value;
		}

		public int OldIndex { get; private set; }

		public int NewIndex { get; private set; }

		public T Value { get; private set; }

		public override string ToString()
		{
			return string.Format("OldIndex:{0} NewIndex:{1} Value:{2}", this.OldIndex, this.NewIndex, this.Value);
		}

		public override int GetHashCode()
		{
			return this.OldIndex.GetHashCode() ^ this.NewIndex.GetHashCode() << 2 ^ EqualityComparer<T>.Default.GetHashCode(this.Value) >> 2;
		}

		public bool Equals(CollectionMoveEvent<T> other)
		{
			return this.OldIndex.Equals(other.OldIndex) && this.NewIndex.Equals(other.NewIndex) && EqualityComparer<T>.Default.Equals(this.Value, other.Value);
		}
	}
}
