using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
	internal class Grouping<K, T> : IEnumerable, IEnumerable<T>, IGrouping<K, T>
	{
		private K key;

		private IEnumerable<T> group;

		public Grouping(K key, IEnumerable<T> group)
		{
			this.group = group;
			this.key = key;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.group.GetEnumerator();
		}

		public K Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.group.GetEnumerator();
		}
	}
}
