using System;

namespace System.Text.RegularExpressions
{
	internal struct Interval : IComparable
	{
		public int low;

		public int high;

		public bool contiguous;

		public Interval(int low, int high)
		{
			if (low > high)
			{
				int num = low;
				low = high;
				high = num;
			}
			this.low = low;
			this.high = high;
			this.contiguous = true;
		}

		public static Interval Empty
		{
			get
			{
				Interval result;
				result.low = 0;
				result.high = result.low - 1;
				result.contiguous = true;
				return result;
			}
		}

		public static Interval Entire
		{
			get
			{
				return new Interval(int.MinValue, int.MaxValue);
			}
		}

		public bool IsDiscontiguous
		{
			get
			{
				return !this.contiguous;
			}
		}

		public bool IsSingleton
		{
			get
			{
				return this.contiguous && this.low == this.high;
			}
		}

		public bool IsRange
		{
			get
			{
				return !this.IsSingleton && !this.IsEmpty;
			}
		}

		public bool IsEmpty
		{
			get
			{
				return this.low > this.high;
			}
		}

		public int Size
		{
			get
			{
				if (this.IsEmpty)
				{
					return 0;
				}
				return this.high - this.low + 1;
			}
		}

		public bool IsDisjoint(Interval i)
		{
			return this.IsEmpty || i.IsEmpty || (this.low > i.high || i.low > this.high);
		}

		public bool IsAdjacent(Interval i)
		{
			return !this.IsEmpty && !i.IsEmpty && (this.low == i.high + 1 || this.high == i.low - 1);
		}

		public bool Contains(Interval i)
		{
			return (!this.IsEmpty && i.IsEmpty) || (!this.IsEmpty && this.low <= i.low && i.high <= this.high);
		}

		public bool Contains(int i)
		{
			return this.low <= i && i <= this.high;
		}

		public bool Intersects(Interval i)
		{
			return !this.IsEmpty && !i.IsEmpty && ((this.Contains(i.low) && !this.Contains(i.high)) || (this.Contains(i.high) && !this.Contains(i.low)));
		}

		public void Merge(Interval i)
		{
			if (i.IsEmpty)
			{
				return;
			}
			if (this.IsEmpty)
			{
				this.low = i.low;
				this.high = i.high;
			}
			if (i.low < this.low)
			{
				this.low = i.low;
			}
			if (i.high > this.high)
			{
				this.high = i.high;
			}
		}

		public void Intersect(Interval i)
		{
			if (this.IsDisjoint(i))
			{
				this.low = 0;
				this.high = this.low - 1;
				return;
			}
			if (i.low > this.low)
			{
				this.low = i.low;
			}
			if (i.high > this.high)
			{
				this.high = i.high;
			}
		}

		public int CompareTo(object o)
		{
			return this.low - ((Interval)o).low;
		}

		public new string ToString()
		{
			if (this.IsEmpty)
			{
				return "(EMPTY)";
			}
			if (!this.contiguous)
			{
				return string.Concat(new object[]
				{
					"{",
					this.low,
					", ",
					this.high,
					"}"
				});
			}
			if (this.IsSingleton)
			{
				return "(" + this.low + ")";
			}
			return string.Concat(new object[]
			{
				"(",
				this.low,
				", ",
				this.high,
				")"
			});
		}
	}
}
