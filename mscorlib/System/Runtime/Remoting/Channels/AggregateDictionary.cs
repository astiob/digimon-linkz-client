using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	[ComVisible(true)]
	internal class AggregateDictionary : IEnumerable, ICollection, IDictionary
	{
		private IDictionary[] dictionaries;

		private ArrayList _values;

		private ArrayList _keys;

		public AggregateDictionary(IDictionary[] dics)
		{
			this.dictionaries = dics;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new AggregateEnumerator(this.dictionaries);
		}

		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public object this[object key]
		{
			get
			{
				foreach (IDictionary dictionary in this.dictionaries)
				{
					if (dictionary.Contains(key))
					{
						return dictionary[key];
					}
				}
				return null;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public ICollection Keys
		{
			get
			{
				if (this._keys != null)
				{
					return this._keys;
				}
				this._keys = new ArrayList();
				foreach (IDictionary dictionary in this.dictionaries)
				{
					this._keys.AddRange(dictionary.Keys);
				}
				return this._keys;
			}
		}

		public ICollection Values
		{
			get
			{
				if (this._values != null)
				{
					return this._values;
				}
				this._values = new ArrayList();
				foreach (IDictionary dictionary in this.dictionaries)
				{
					this._values.AddRange(dictionary.Values);
				}
				return this._values;
			}
		}

		public void Add(object key, object value)
		{
			throw new NotSupportedException();
		}

		public void Clear()
		{
			throw new NotSupportedException();
		}

		public bool Contains(object ob)
		{
			foreach (IDictionary dictionary in this.dictionaries)
			{
				if (dictionary.Contains(ob))
				{
					return true;
				}
			}
			return false;
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return new AggregateEnumerator(this.dictionaries);
		}

		public void Remove(object ob)
		{
			throw new NotSupportedException();
		}

		public void CopyTo(Array array, int index)
		{
			foreach (object value in this)
			{
				array.SetValue(value, index++);
			}
		}

		public int Count
		{
			get
			{
				int num = 0;
				foreach (IDictionary dictionary in this.dictionaries)
				{
					num += dictionary.Count;
				}
				return num;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public object SyncRoot
		{
			get
			{
				return this;
			}
		}
	}
}
