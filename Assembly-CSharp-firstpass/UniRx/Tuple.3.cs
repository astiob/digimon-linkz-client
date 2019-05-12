using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
	[Serializable]
	public struct Tuple<T1> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple, IEquatable<Tuple<T1>>
	{
		private T1 item1;

		public Tuple(T1 item1)
		{
			this.item1 = item1;
		}

		public T1 Item1
		{
			get
			{
				return this.item1;
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
			if (!(other is Tuple<T1>))
			{
				throw new ArgumentException("other");
			}
			Tuple<T1> tuple = (Tuple<T1>)other;
			return comparer.Compare(this.item1, tuple.item1);
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (!(other is Tuple<T1>))
			{
				return false;
			}
			Tuple<T1> tuple = (Tuple<T1>)other;
			return comparer.Equals(this.item1, tuple.item1);
		}

		public override int GetHashCode()
		{
			return EqualityComparer<T1>.Default.GetHashCode(this.item1);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return comparer.GetHashCode(this.item1);
		}

		string ITuple.ToString()
		{
			return string.Format("{0}", this.item1);
		}

		public override string ToString()
		{
			return "(" + ((ITuple)this).ToString() + ")";
		}

		public bool Equals(Tuple<T1> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.item1, other.item1);
		}
	}
}
