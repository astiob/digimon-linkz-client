using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	internal class IntervalCollection : ICollection, IEnumerable
	{
		private ArrayList intervals;

		public IntervalCollection()
		{
			this.intervals = new ArrayList();
		}

		public Interval this[int i]
		{
			get
			{
				return (Interval)this.intervals[i];
			}
			set
			{
				this.intervals[i] = value;
			}
		}

		public void Add(Interval i)
		{
			this.intervals.Add(i);
		}

		public void Clear()
		{
			this.intervals.Clear();
		}

		public void Sort()
		{
			this.intervals.Sort();
		}

		public void Normalize()
		{
			this.intervals.Sort();
			int i = 0;
			while (i < this.intervals.Count - 1)
			{
				Interval interval = (Interval)this.intervals[i];
				Interval i2 = (Interval)this.intervals[i + 1];
				if (!interval.IsDisjoint(i2) || interval.IsAdjacent(i2))
				{
					interval.Merge(i2);
					this.intervals[i] = interval;
					this.intervals.RemoveAt(i + 1);
				}
				else
				{
					i++;
				}
			}
		}

		public IntervalCollection GetMetaCollection(IntervalCollection.CostDelegate cost_del)
		{
			IntervalCollection intervalCollection = new IntervalCollection();
			this.Normalize();
			this.Optimize(0, this.Count - 1, intervalCollection, cost_del);
			intervalCollection.intervals.Sort();
			return intervalCollection;
		}

		private void Optimize(int begin, int end, IntervalCollection meta, IntervalCollection.CostDelegate cost_del)
		{
			Interval i;
			i.contiguous = false;
			int num = -1;
			int num2 = -1;
			double num3 = 0.0;
			for (int j = begin; j <= end; j++)
			{
				i.low = this[j].low;
				double num4 = 0.0;
				for (int k = j; k <= end; k++)
				{
					i.high = this[k].high;
					num4 += cost_del(this[k]);
					double num5 = cost_del(i);
					if (num5 < num4 && num4 > num3)
					{
						num = j;
						num2 = k;
						num3 = num4;
					}
				}
			}
			if (num < 0)
			{
				for (int l = begin; l <= end; l++)
				{
					meta.Add(this[l]);
				}
			}
			else
			{
				i.low = this[num].low;
				i.high = this[num2].high;
				meta.Add(i);
				if (num > begin)
				{
					this.Optimize(begin, num - 1, meta, cost_del);
				}
				if (num2 < end)
				{
					this.Optimize(num2 + 1, end, meta, cost_del);
				}
			}
		}

		public int Count
		{
			get
			{
				return this.intervals.Count;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public object SyncRoot
		{
			get
			{
				return this.intervals;
			}
		}

		public void CopyTo(Array array, int index)
		{
			foreach (object obj in this.intervals)
			{
				Interval interval = (Interval)obj;
				if (index > array.Length)
				{
					break;
				}
				array.SetValue(interval, index++);
			}
		}

		public IEnumerator GetEnumerator()
		{
			return new IntervalCollection.Enumerator(this.intervals);
		}

		private class Enumerator : IEnumerator
		{
			private IList list;

			private int ptr;

			public Enumerator(IList list)
			{
				this.list = list;
				this.Reset();
			}

			public object Current
			{
				get
				{
					if (this.ptr >= this.list.Count)
					{
						throw new InvalidOperationException();
					}
					return this.list[this.ptr];
				}
			}

			public bool MoveNext()
			{
				if (this.ptr > this.list.Count)
				{
					throw new InvalidOperationException();
				}
				return ++this.ptr < this.list.Count;
			}

			public void Reset()
			{
				this.ptr = -1;
			}
		}

		public delegate double CostDelegate(Interval i);
	}
}
