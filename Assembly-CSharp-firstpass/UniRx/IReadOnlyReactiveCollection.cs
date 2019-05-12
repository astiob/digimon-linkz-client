using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
	public interface IReadOnlyReactiveCollection<T> : IEnumerable<T>, IEnumerable
	{
		int Count { get; }

		T this[int index]
		{
			get;
		}

		IObservable<CollectionAddEvent<T>> ObserveAdd();

		IObservable<int> ObserveCountChanged(bool notifyCurrentCount = false);

		IObservable<CollectionMoveEvent<T>> ObserveMove();

		IObservable<CollectionRemoveEvent<T>> ObserveRemove();

		IObservable<CollectionReplaceEvent<T>> ObserveReplace();

		IObservable<Unit> ObserveReset();
	}
}
