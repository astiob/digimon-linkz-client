using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
	[Serializable]
	public struct Tuple<T1, T2, T3> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple, IEquatable<Tuple<T1, T2, T3>>
	{
		private T1 item1;

		private T2 item2;

		private T3 item3;

		public Tuple(T1 item1, T2 item2, T3 item3)
		{
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
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

		public T3 Item3
		{
			get
			{
				return this.item3;
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
			if (!(other is Tuple<T1, T2, T3>))
			{
				throw new ArgumentException("other");
			}
			Tuple<T1, T2, T3> tuple = (Tuple<T1, T2, T3>)other;
			int num = comparer.Compare(this.item1, tuple.item1);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.item2, tuple.item2);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.item3, tuple.item3);
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (!(other is Tuple<T1, T2, T3>))
			{
				return false;
			}
			Tuple<T1, T2, T3> tuple = (Tuple<T1, T2, T3>)other;
			return comparer.Equals(this.item1, tuple.item1) && comparer.Equals(this.item2, tuple.item2) && comparer.Equals(this.item3, tuple.item3);
		}

		public override int GetHashCode()
		{
			EqualityComparer<T1> @default = EqualityComparer<T1>.Default;
			EqualityComparer<T2> default2 = EqualityComparer<T2>.Default;
			EqualityComparer<T3> default3 = EqualityComparer<T3>.Default;
			int num = @default.GetHashCode(this.item1);
			num = ((num << 5) + num ^ default2.GetHashCode(this.item2));
			return (num << 5) + num ^ default3.GetHashCode(this.item3);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			int num = comparer.GetHashCode(this.item1);
			num = ((num << 5) + num ^ comparer.GetHashCode(this.item2));
			return (num << 5) + num ^ comparer.GetHashCode(this.item3);
		}

		string ITuple.ToString()
		{
			return string.Format("{0}, {1}, {2}", this.item1, this.item2, this.item3);
		}

		public override string ToString()
		{
			return "(" + ((ITuple)this).ToString() + ")";
		}

		public bool Equals(Tuple<T1, T2, T3> other)
		{
			EqualityComparer<T1> @default = EqualityComparer<T1>.Default;
			EqualityComparer<T2> default2 = EqualityComparer<T2>.Default;
			EqualityComparer<T3> default3 = EqualityComparer<T3>.Default;
			return @default.Equals(this.item1, other.item1) && default2.Equals(this.item2, other.item2) && default3.Equals(this.item3, other.item3);
		}
	}
}
