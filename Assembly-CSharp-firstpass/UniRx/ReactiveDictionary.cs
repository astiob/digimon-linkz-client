using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class ReactiveDictionary<TKey, TValue> : IReactiveDictionary<TKey, TValue>, IDictionary<TKey, TValue>, IEnumerable, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, IDisposable, ISerializable, IDeserializationCallback, IReadOnlyReactiveDictionary<TKey, TValue>, ICollection
	{
		[NonSerialized]
		private bool isDisposed;

		[SerializeField]
		private readonly Dictionary<TKey, TValue> inner;

		private bool disposedValue;

		[NonSerialized]
		private Subject<int> countChanged;

		[NonSerialized]
		private Subject<Unit> collectionReset;

		[NonSerialized]
		private Subject<DictionaryAddEvent<TKey, TValue>> dictionaryAdd;

		[NonSerialized]
		private Subject<DictionaryRemoveEvent<TKey, TValue>> dictionaryRemove;

		[NonSerialized]
		private Subject<DictionaryReplaceEvent<TKey, TValue>> dictionaryReplace;

		public ReactiveDictionary()
		{
			this.inner = new Dictionary<TKey, TValue>();
		}

		public ReactiveDictionary(IEqualityComparer<TKey> comparer)
		{
			this.inner = new Dictionary<TKey, TValue>(comparer);
		}

		public ReactiveDictionary(Dictionary<TKey, TValue> innerDictionary)
		{
			this.inner = innerDictionary;
		}

		public TValue this[TKey key]
		{
			get
			{
				return this.inner[key];
			}
			set
			{
				TValue oldValue;
				if (this.TryGetValue(key, out oldValue))
				{
					this.inner[key] = value;
					if (this.dictionaryReplace != null)
					{
						this.dictionaryReplace.OnNext(new DictionaryReplaceEvent<TKey, TValue>(key, oldValue, value));
					}
				}
				else
				{
					this.inner[key] = value;
					if (this.dictionaryAdd != null)
					{
						this.dictionaryAdd.OnNext(new DictionaryAddEvent<TKey, TValue>(key, value));
					}
					if (this.countChanged != null)
					{
						this.countChanged.OnNext(this.Count);
					}
				}
			}
		}

		public int Count
		{
			get
			{
				return this.inner.Count;
			}
		}

		public Dictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				return this.inner.Keys;
			}
		}

		public Dictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				return this.inner.Values;
			}
		}

		public void Add(TKey key, TValue value)
		{
			this.inner.Add(key, value);
			if (this.dictionaryAdd != null)
			{
				this.dictionaryAdd.OnNext(new DictionaryAddEvent<TKey, TValue>(key, value));
			}
			if (this.countChanged != null)
			{
				this.countChanged.OnNext(this.Count);
			}
		}

		public void Clear()
		{
			int count = this.Count;
			this.inner.Clear();
			if (this.collectionReset != null)
			{
				this.collectionReset.OnNext(Unit.Default);
			}
			if (count > 0 && this.countChanged != null)
			{
				this.countChanged.OnNext(this.Count);
			}
		}

		public bool Remove(TKey key)
		{
			TValue value;
			if (this.inner.TryGetValue(key, out value))
			{
				bool flag = this.inner.Remove(key);
				if (flag)
				{
					if (this.dictionaryRemove != null)
					{
						this.dictionaryRemove.OnNext(new DictionaryRemoveEvent<TKey, TValue>(key, value));
					}
					if (this.countChanged != null)
					{
						this.countChanged.OnNext(this.Count);
					}
				}
				return flag;
			}
			return false;
		}

		public bool ContainsKey(TKey key)
		{
			return this.inner.ContainsKey(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return this.inner.TryGetValue(key, out value);
		}

		public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return this.inner.GetEnumerator();
		}

		private void DisposeSubject<TSubject>(ref Subject<TSubject> subject)
		{
			if (subject != null)
			{
				try
				{
					subject.OnCompleted();
				}
				finally
				{
					subject.Dispose();
					subject = null;
				}
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					this.DisposeSubject<int>(ref this.countChanged);
					this.DisposeSubject<Unit>(ref this.collectionReset);
					this.DisposeSubject<DictionaryAddEvent<TKey, TValue>>(ref this.dictionaryAdd);
					this.DisposeSubject<DictionaryRemoveEvent<TKey, TValue>>(ref this.dictionaryRemove);
					this.DisposeSubject<DictionaryReplaceEvent<TKey, TValue>>(ref this.dictionaryReplace);
				}
				this.disposedValue = true;
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
		}

		public IObservable<int> ObserveCountChanged()
		{
			if (this.isDisposed)
			{
				return Observable.Empty<int>();
			}
			Subject<int> result;
			if ((result = this.countChanged) == null)
			{
				result = (this.countChanged = new Subject<int>());
			}
			return result;
		}

		public IObservable<Unit> ObserveReset()
		{
			if (this.isDisposed)
			{
				return Observable.Empty<Unit>();
			}
			Subject<Unit> result;
			if ((result = this.collectionReset) == null)
			{
				result = (this.collectionReset = new Subject<Unit>());
			}
			return result;
		}

		public IObservable<DictionaryAddEvent<TKey, TValue>> ObserveAdd()
		{
			if (this.isDisposed)
			{
				return Observable.Empty<DictionaryAddEvent<TKey, TValue>>();
			}
			Subject<DictionaryAddEvent<TKey, TValue>> result;
			if ((result = this.dictionaryAdd) == null)
			{
				result = (this.dictionaryAdd = new Subject<DictionaryAddEvent<TKey, TValue>>());
			}
			return result;
		}

		public IObservable<DictionaryRemoveEvent<TKey, TValue>> ObserveRemove()
		{
			if (this.isDisposed)
			{
				return Observable.Empty<DictionaryRemoveEvent<TKey, TValue>>();
			}
			Subject<DictionaryRemoveEvent<TKey, TValue>> result;
			if ((result = this.dictionaryRemove) == null)
			{
				result = (this.dictionaryRemove = new Subject<DictionaryRemoveEvent<TKey, TValue>>());
			}
			return result;
		}

		public IObservable<DictionaryReplaceEvent<TKey, TValue>> ObserveReplace()
		{
			if (this.isDisposed)
			{
				return Observable.Empty<DictionaryReplaceEvent<TKey, TValue>>();
			}
			Subject<DictionaryReplaceEvent<TKey, TValue>> result;
			if ((result = this.dictionaryReplace) == null)
			{
				result = (this.dictionaryReplace = new Subject<DictionaryReplaceEvent<TKey, TValue>>());
			}
			return result;
		}

		object IDictionary.this[object key]
		{
			get
			{
				return this[(TKey)((object)key)];
			}
			set
			{
				this[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		bool IDictionary.IsFixedSize
		{
			get
			{
				return ((IDictionary)this.inner).IsFixedSize;
			}
		}

		bool IDictionary.IsReadOnly
		{
			get
			{
				return ((IDictionary)this.inner).IsReadOnly;
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				return ((ICollection)this.inner).IsSynchronized;
			}
		}

		ICollection IDictionary.Keys
		{
			get
			{
				return ((IDictionary)this.inner).Keys;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				return ((ICollection)this.inner).SyncRoot;
			}
		}

		ICollection IDictionary.Values
		{
			get
			{
				return ((IDictionary)this.inner).Values;
			}
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return ((ICollection<KeyValuePair<TKey, TValue>>)this.inner).IsReadOnly;
			}
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				return this.inner.Keys;
			}
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				return this.inner.Values;
			}
		}

		void IDictionary.Add(object key, object value)
		{
			this.Add((TKey)((object)key), (TValue)((object)value));
		}

		bool IDictionary.Contains(object key)
		{
			return ((IDictionary)this.inner).Contains(key);
		}

		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this.inner).CopyTo(array, index);
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			((ISerializable)this.inner).GetObjectData(info, context);
		}

		public void OnDeserialization(object sender)
		{
			((IDeserializationCallback)this.inner).OnDeserialization(sender);
		}

		void IDictionary.Remove(object key)
		{
			this.Remove((TKey)((object)key));
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)this.inner).Contains(item);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, TValue>>)this.inner).CopyTo(array, arrayIndex);
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<TKey, TValue>>)this.inner).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.inner.GetEnumerator();
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			TValue x;
			if (this.TryGetValue(item.Key, out x) && EqualityComparer<TValue>.Default.Equals(x, item.Value))
			{
				this.Remove(item.Key);
				return true;
			}
			return false;
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IDictionary)this.inner).GetEnumerator();
		}
	}
}
