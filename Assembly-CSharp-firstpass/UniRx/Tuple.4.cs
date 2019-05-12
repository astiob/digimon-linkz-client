using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
	[Serializable]
	public struct Tuple<T1, T2> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple, IEquatable<Tuple<T1, T2>>
	{
		private T1 item1;

		private T2 item2;

		public Tuple(T1 item1, T2 item2)
		{
			this.item1 = item1;
			this.item2 = item2;
		}

		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}

		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is Tuple<T1, T2>))
			{
				throw new ArgumentException("other");
			}
			Tuple<T1, T2> tuple = (Tuple<T1, T2>)other;
			int num = comparer.Compare(this.item1, tuple.item1);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.item2, tuple.item2);
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (!(other is Tuple<T1, T2>))
			{
				return false;
			}
			Tuple<T1, T2> tuple = (Tuple<T1, T2>)other;
			return comparer.Equals(this.item1, tuple.item1) && comparer.Equals(this.item2, tuple.item2);
		}

		public override int GetHashCode()
		{
			EqualityComparer<T1> @default = EqualityComparer<T1>.Default;
			EqualityComparer<T2> default2 = EqualityComparer<T2>.Default;
			int hashCode = @default.GetHashCode(this.item1);
			return (hashCode << 5) + hashCode ^ default2.GetHashCode(this.item2);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			int hashCode = comparer.GetHashCode(this.item1);
			return (hashCode << 5) + hashCode ^ comparer.GetHashCode(this.item2);
		}

		string ITuple.ToString()
		{
			return string.Format("{0}, {1}", this.item1, this.item2);
		}

		public override string ToString()
		{
			return "(" + ((ITuple)this).ToString() + ")";
		}

		public bool Equals(Tuple<T1, T2> other)
		{
			EqualityComparer<T1> @default = EqualityComparer<T1>.Default;
			EqualityComparer<T2> default2 = EqualityComparer<T2>.Default;
			return @default.Equals(this.item1, other.item1) && default2.Equals(this.item2, other.item2);
		}
	}
}
