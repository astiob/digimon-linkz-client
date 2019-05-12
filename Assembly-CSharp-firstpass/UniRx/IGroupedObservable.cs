using System;

namespace UniRx
{
	public interface IGroupedObservable<TKey, TElement> : IObservable<TElement>
	{
		TKey Key { get; }
	}
}
