using System;
using System.Collections.Generic;

namespace UnityEngine
{
	internal class IntervalNode<T> where T : class, IInterval
	{
		private long m_Center;

		private List<T> m_Children;

		private IntervalNode<T> m_LeftNode;

		private IntervalNode<T> m_RightNode;

		public IntervalNode(ICollection<T> items)
		{
			if (items.Count != 0)
			{
				long num = long.MaxValue;
				long num2 = long.MinValue;
				foreach (T t in items)
				{
					num = Math.Min(num, t.intervalStart);
					num2 = Math.Max(num2, t.intervalEnd);
				}
				this.m_Center = (num2 + num) / 2L;
				this.m_Children = new List<T>();
				List<T> list = new List<T>();
				List<T> list2 = new List<T>();
				foreach (T item in items)
				{
					if (item.intervalEnd < this.m_Center)
					{
						list.Add(item);
					}
					else if (item.intervalStart > this.m_Center)
					{
						list2.Add(item);
					}
					else
					{
						this.m_Children.Add(item);
					}
				}
				if (this.m_Children.Count == 0)
				{
					this.m_Children = null;
				}
				if (list.Count > 0)
				{
					this.m_LeftNode = new IntervalNode<T>(list);
				}
				if (list2.Count > 0)
				{
					this.m_RightNode = new IntervalNode<T>(list2);
				}
			}
		}

		public void Query(long time, int bitflag, ref List<T> results)
		{
			if (this.m_Children != null)
			{
				foreach (T item in this.m_Children)
				{
					if (time >= item.intervalStart && time < item.intervalEnd)
					{
						item.intervalBit = bitflag;
						results.Add(item);
					}
				}
			}
			if (time < this.m_Center && this.m_LeftNode != null)
			{
				this.m_LeftNode.Query(time, bitflag, ref results);
			}
			else if (time > this.m_Center && this.m_RightNode != null)
			{
				this.m_RightNode.Query(time, bitflag, ref results);
			}
		}
	}
}
