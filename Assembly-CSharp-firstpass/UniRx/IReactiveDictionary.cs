using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx
{
	public interface IReactiveDictionary<TKey, TValue> : IReadOnlyReactiveDictionary<TKey, TValue>, IDictionary<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, ICollection<KeyValuePair<TKey, TValue>>
	{
	}
}
