using System;
using System.Collections.Generic;

namespace UnityEngine
{
	internal class IntervalTree<T> where T : class, IInterval
	{
		private List<T> m_Nodes = new List<T>();

		private bool m_Dirty = true;

		private IntervalNode<T> m_Root;

		public void Add(T item)
		{
			this.m_Nodes.Add(item);
			this.m_Dirty = true;
		}

		public void IntersectsWith(long value, int bitFlag, ref List<T> results)
		{
			if (this.m_Dirty)
			{
				this.m_Root = new IntervalNode<T>(this.m_Nodes);
				this.m_Dirty = false;
			}
			this.m_Root.Query(value, bitFlag, ref results);
		}
	}
}
