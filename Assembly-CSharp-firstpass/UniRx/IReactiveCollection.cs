using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
	public interface IReactiveCollection<T> : IList<T>, IReadOnlyReactiveCollection<T>, IEnumerable, ICollection<T>, IEnumerable<T>
	{
		int Count { get; }

		T this[int index]
		{
			get;
			set;
		}

		void Move(int oldIndex, int newIndex);
	}
}
