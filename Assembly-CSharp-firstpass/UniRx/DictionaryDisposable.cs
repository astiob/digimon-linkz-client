using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UniRx
{
	public sealed class DictionaryDisposable<TKey, TValue> : IDisposable, IDictionary<TKey, TValue>, IEnumerable, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>> where TValue : IDisposable
	{
		private bool isDisposed;

		private readonly Dictionary<TKey, TValue> inner;

		public DictionaryDisposable()
		{
			this.inner = new Dictionary<TKey, TValue>();
		}

		public DictionaryDisposable(IEqualityComparer<TKey> comparer)
		{
			this.inner = new Dictionary<TKey, TValue>(comparer);
		}

		public TValue this[TKey key]
		{
			get
			{
				object obj = this.inner;
				TValue result;
				lock (obj)
				{
					result = this.inner[key];
				}
				return result;
			}
			set
			{
				object obj = this.inner;
				lock (obj)
				{
					if (this.isDisposed)
					{
						value.Dispose();
					}
					TValue tvalue;
					if (this.TryGetValue(key, out tvalue))
					{
						tvalue.Dispose();
						this.inner[key] = value;
					}
					else
					{
						this.inner[key] = value;
					}
				}
			}
		}

		public int Count
		{
			get
			{
				object obj = this.inner;
				int count;
				lock (obj)
				{
					count = this.inner.Count;
				}
				return count;
			}
		}

		public Dictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				throw new NotSupportedException("please use .Select(x => x.Key).ToArray()");
			}
		}

		public Dictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				throw new NotSupportedException("please use .Select(x => x.Value).ToArray()");
			}
		}

		public void Add(TKey key, TValue value)
		{
			object obj = this.inner;
			lock (obj)
			{
				if (this.isDisposed)
				{
					value.Dispose();
				}
				else
				{
					this.inner.Add(key, value);
				}
			}
		}

		public void Clear()
		{
			object obj = this.inner;
			lock (obj)
			{
				foreach (KeyValuePair<TKey, TValue> keyValuePair in this.inner)
				{
					TValue value = keyValuePair.Value;
					value.Dispose();
				}
				this.inner.Clear();
			}
		}

		public bool Remove(TKey key)
		{
			object obj = this.inner;
			bool result;
			lock (obj)
			{
				TValue tvalue;
				if (this.inner.TryGetValue(key, out tvalue))
				{
					bool flag = this.inner.Remove(key);
					if (flag)
					{
						tvalue.Dispose();
					}
					result = flag;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		public bool ContainsKey(TKey key)
		{
			object obj = this.inner;
			bool result;
			lock (obj)
			{
				result = this.inner.ContainsKey(key);
			}
			return result;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			object obj = this.inner;
			bool result;
			lock (obj)
			{
				result = this.inner.TryGetValue(key, out value);
			}
			return result;
		}

		public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			object obj = this.inner;
			Dictionary<TKey, TValue>.Enumerator enumerator;
			lock (obj)
			{
				enumerator = new Dictionary<TKey, TValue>(this.inner).GetEnumerator();
			}
			return enumerator;
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
				object obj = this.inner;
				ICollection<TKey> result;
				lock (obj)
				{
					result = new List<TKey>(this.inner.Keys);
				}
				return result;
			}
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				object obj = this.inner;
				ICollection<TValue> result;
				lock (obj)
				{
					result = new List<TValue>(this.inner.Values);
				}
				return result;
			}
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			object obj = this.inner;
			lock (obj)
			{
				((ISerializable)this.inner).GetObjectData(info, context);
			}
		}

		public void OnDeserialization(object sender)
		{
			object obj = this.inner;
			lock (obj)
			{
				((IDeserializationCallback)this.inner).OnDeserialization(sender);
			}
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			object obj = this.inner;
			bool result;
			lock (obj)
			{
				result = ((ICollection<KeyValuePair<TKey, TValue>>)this.inner).Contains(item);
			}
			return result;
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			object obj = this.inner;
			lock (obj)
			{
				((ICollection<KeyValuePair<TKey, TValue>>)this.inner).CopyTo(array, arrayIndex);
			}
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			object obj = this.inner;
			IEnumerator<KeyValuePair<TKey, TValue>> result;
			lock (obj)
			{
				result = new List<KeyValuePair<TKey, TValue>>(this.inner).GetEnumerator();
			}
			return result;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			throw new NotSupportedException();
		}

		public void Dispose()
		{
			object obj = this.inner;
			lock (obj)
			{
				if (!this.isDisposed)
				{
					this.isDisposed = true;
					foreach (KeyValuePair<TKey, TValue> keyValuePair in this.inner)
					{
						TValue value = keyValuePair.Value;
						value.Dispose();
					}
					this.inner.Clear();
				}
			}
		}
	}
}
