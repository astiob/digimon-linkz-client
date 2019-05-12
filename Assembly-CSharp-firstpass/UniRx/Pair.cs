using System;
using System.Collections.Generic;

namespace UniRx
{
	[Serializable]
	public struct Pair<T> : IEquatable<Pair<T>>
	{
		private readonly T previous;

		private readonly T current;

		public Pair(T previous, T current)
		{
			this.previous = previous;
			this.current = current;
		}

		public T Previous
		{
			get
			{
				return this.previous;
			}
		}

		public T Current
		{
			get
			{
				return this.current;
			}
		}

		public override int GetHashCode()
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			int hashCode = @default.GetHashCode(this.previous);
			return (hashCode << 5) + hashCode ^ @default.GetHashCode(this.current);
		}

		public override bool Equals(object obj)
		{
			return obj is Pair<T> && this.Equals((Pair<T>)obj);
		}

		public bool Equals(Pair<T> other)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			return @default.Equals(this.previous, other.Previous) && @default.Equals(this.current, other.Current);
		}

		public override string ToString()
		{
			return string.Format("({0}, {1})", this.previous, this.current);
		}
	}
}
