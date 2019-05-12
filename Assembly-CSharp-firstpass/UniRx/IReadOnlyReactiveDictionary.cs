using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
	public interface IReadOnlyReactiveDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		int Count { get; }

		TValue this[TKey index]
		{
			get;
		}

		bool ContainsKey(TKey key);

		bool TryGetValue(TKey key, out TValue value);

		IObservable<DictionaryAddEvent<TKey, TValue>> ObserveAdd();

		IObservable<int> ObserveCountChanged();

		IObservable<DictionaryRemoveEvent<TKey, TValue>> ObserveRemove();

		IObservable<DictionaryReplaceEvent<TKey, TValue>> ObserveReplace();

		IObservable<Unit> ObserveReset();
	}
}
