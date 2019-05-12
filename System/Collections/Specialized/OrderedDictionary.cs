using System;
using System.Runtime.Serialization;

namespace System.Collections.Specialized
{
	/// <summary>Represents a collection of key/value pairs that are accessible by the key or index.</summary>
	[Serializable]
	public class OrderedDictionary : ICollection, IOrderedDictionary, IDictionary, IDeserializationCallback, IEnumerable, ISerializable
	{
		private ArrayList list;

		private Hashtable hash;

		private bool readOnly;

		private int initialCapacity;

		private SerializationInfo serializationInfo;

		private IEqualityComparer comparer;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> class.</summary>
		public OrderedDictionary()
		{
			this.list = new ArrayList();
			this.hash = new Hashtable();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> class using the specified initial capacity.</summary>
		/// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection can contain.</param>
		public OrderedDictionary(int capacity)
		{
			this.initialCapacity = ((capacity >= 0) ? capacity : 0);
			this.list = new ArrayList(this.initialCapacity);
			this.hash = new Hashtable(this.initialCapacity);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> class using the specified comparer.</summary>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> to use to determine whether two keys are equal.-or- null to use the default comparer, which is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />.</param>
		public OrderedDictionary(IEqualityComparer equalityComparer)
		{
			this.list = new ArrayList();
			this.hash = new Hashtable(equalityComparer);
			this.comparer = equalityComparer;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> class using the specified initial capacity and comparer.</summary>
		/// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection can contain.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> to use to determine whether two keys are equal.-or- null to use the default comparer, which is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />.</param>
		public OrderedDictionary(int capacity, IEqualityComparer equalityComparer)
		{
			this.initialCapacity = ((capacity >= 0) ? capacity : 0);
			this.list = new ArrayList(this.initialCapacity);
			this.hash = new Hashtable(this.initialCapacity, equalityComparer);
			this.comparer = equalityComparer;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> class that is serializable using the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> objects.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Specialized.OrderedDictionary" />.</param>
		protected OrderedDictionary(SerializationInfo info, StreamingContext context)
		{
			this.serializationInfo = info;
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and is called back by the deserialization event when deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event.</param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			if (this.serializationInfo == null)
			{
				return;
			}
			this.comparer = (IEqualityComparer)this.serializationInfo.GetValue("KeyComparer", typeof(IEqualityComparer));
			this.readOnly = this.serializationInfo.GetBoolean("ReadOnly");
			this.initialCapacity = this.serializationInfo.GetInt32("InitialCapacity");
			if (this.list == null)
			{
				this.list = new ArrayList();
			}
			else
			{
				this.list.Clear();
			}
			this.hash = new Hashtable(this.comparer);
			object[] array = (object[])this.serializationInfo.GetValue("ArrayList", typeof(object[]));
			foreach (DictionaryEntry dictionaryEntry in array)
			{
				this.hash.Add(dictionaryEntry.Key, dictionaryEntry.Value);
				this.list.Add(dictionaryEntry);
			}
		}

		/// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> object that iterates through the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> object for the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> object is synchronized (thread-safe).</summary>
		/// <returns>This method always returns false.</returns>
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.list.IsSynchronized;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> object.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> object.</returns>
		object ICollection.SyncRoot
		{
			get
			{
				return this.list.SyncRoot;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> has a fixed size.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> has a fixed size; otherwise, false. The default is false.</returns>
		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and is called back by the deserialization event when deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object associated with the current <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is invalid.</exception>
		protected virtual void OnDeserialization(object sender)
		{
			((IDeserializationCallback)this).OnDeserialization(sender);
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Specialized.OrderedDictionary" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null.</exception>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("KeyComparer", this.comparer, typeof(IEqualityComparer));
			info.AddValue("ReadOnly", this.readOnly);
			info.AddValue("InitialCapacity", this.initialCapacity);
			object[] array = new object[this.hash.Count];
			this.hash.CopyTo(array, 0);
			info.AddValue("ArrayList", array);
		}

		/// <summary>Gets the number of key/values pairs contained in the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <returns>The number of key/value pairs contained in the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</returns>
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		/// <summary>Copies the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> elements to a one-dimensional <see cref="T:System.Array" /> object at the specified index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> object that is the destination of the <see cref="T:System.Collections.DictionaryEntry" /> objects copied from <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		public void CopyTo(Array array, int index)
		{
			this.list.CopyTo(array, index);
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only; otherwise, false. The default is false.</returns>
		public bool IsReadOnly
		{
			get
			{
				return this.readOnly;
			}
		}

		/// <summary>Gets or sets the value with the specified key.</summary>
		/// <returns>The value associated with the specified key. If the specified key is not found, attempting to get it returns null, and attempting to set it creates a new element using the specified key.</returns>
		/// <param name="key">The key of the value to get or set.</param>
		/// <exception cref="T:System.NotSupportedException">The property is being set and the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only.</exception>
		public object this[object key]
		{
			get
			{
				return this.hash[key];
			}
			set
			{
				this.WriteCheck();
				if (this.hash.Contains(key))
				{
					int index = this.FindListEntry(key);
					this.list[index] = new DictionaryEntry(key, value);
				}
				else
				{
					this.list.Add(new DictionaryEntry(key, value));
				}
				this.hash[key] = value;
			}
		}

		/// <summary>Gets or sets the value at the specified index.</summary>
		/// <returns>The value of the item at the specified index. </returns>
		/// <param name="index">The zero-based index of the value to get or set.</param>
		/// <exception cref="T:System.NotSupportedException">The property is being set and the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or-<paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.Specialized.OrderedDictionary.Count" />.</exception>
		public object this[int index]
		{
			get
			{
				return ((DictionaryEntry)this.list[index]).Value;
			}
			set
			{
				this.WriteCheck();
				DictionaryEntry dictionaryEntry = (DictionaryEntry)this.list[index];
				dictionaryEntry.Value = value;
				this.list[index] = dictionaryEntry;
				this.hash[dictionaryEntry.Key] = value;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> object containing the keys in the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> object containing the keys in the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</returns>
		public ICollection Keys
		{
			get
			{
				return new OrderedDictionary.OrderedCollection(this.list, true);
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> object containing the values in the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> object containing the values in the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</returns>
		public ICollection Values
		{
			get
			{
				return new OrderedDictionary.OrderedCollection(this.list, false);
			}
		}

		/// <summary>Adds an entry with the specified key and value into the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection with the lowest available index.</summary>
		/// <param name="key">The key of the entry to add.</param>
		/// <param name="value">The value of the entry to add. This value can be null.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only.</exception>
		public void Add(object key, object value)
		{
			this.WriteCheck();
			this.hash.Add(key, value);
			this.list.Add(new DictionaryEntry(key, value));
		}

		/// <summary>Removes all elements from the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only.</exception>
		public void Clear()
		{
			this.WriteCheck();
			this.hash.Clear();
			this.list.Clear();
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection contains a specific key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection contains an element with the specified key; otherwise, false.</returns>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</param>
		public bool Contains(object key)
		{
			return this.hash.Contains(key);
		}

		/// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> object that iterates through the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> object for the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</returns>
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new OrderedDictionary.OrderedEntryCollectionEnumerator(this.list.GetEnumerator());
		}

		/// <summary>Removes the entry with the specified key from the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <param name="key">The key of the entry to remove.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		public void Remove(object key)
		{
			this.WriteCheck();
			if (this.hash.Contains(key))
			{
				this.hash.Remove(key);
				int index = this.FindListEntry(key);
				this.list.RemoveAt(index);
			}
		}

		private int FindListEntry(object key)
		{
			for (int i = 0; i < this.list.Count; i++)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)this.list[i];
				if ((this.comparer == null) ? dictionaryEntry.Key.Equals(key) : this.comparer.Equals(dictionaryEntry.Key, key))
				{
					return i;
				}
			}
			return -1;
		}

		private void WriteCheck()
		{
			if (this.readOnly)
			{
				throw new NotSupportedException("Collection is read only");
			}
		}

		/// <summary>Returns a read-only copy of the current <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <returns>A read-only copy of the current <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</returns>
		public OrderedDictionary AsReadOnly()
		{
			return new OrderedDictionary
			{
				list = this.list,
				hash = this.hash,
				comparer = this.comparer,
				readOnly = true
			};
		}

		/// <summary>Inserts a new entry into the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection with the specified key and value at the specified index.</summary>
		/// <param name="index">The zero-based index at which the element should be inserted.</param>
		/// <param name="key">The key of the entry to add.</param>
		/// <param name="value">The value of the entry to add. The value can be null.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is out of range.</exception>
		/// <exception cref="T:System.NotSupportedException">This collection is read-only.</exception>
		public void Insert(int index, object key, object value)
		{
			this.WriteCheck();
			this.hash.Add(key, value);
			this.list.Insert(index, new DictionaryEntry(key, value));
		}

		/// <summary>Removes the entry at the specified index from the <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection.</summary>
		/// <param name="index">The zero-based index of the entry to remove.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Specialized.OrderedDictionary" /> collection is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.- or -<paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.Specialized.OrderedDictionary.Count" />.</exception>
		public void RemoveAt(int index)
		{
			this.WriteCheck();
			DictionaryEntry dictionaryEntry = (DictionaryEntry)this.list[index];
			this.list.RemoveAt(index);
			this.hash.Remove(dictionaryEntry.Key);
		}

		private class OrderedEntryCollectionEnumerator : IEnumerator, IDictionaryEnumerator
		{
			private IEnumerator listEnumerator;

			public OrderedEntryCollectionEnumerator(IEnumerator listEnumerator)
			{
				this.listEnumerator = listEnumerator;
			}

			public bool MoveNext()
			{
				return this.listEnumerator.MoveNext();
			}

			public void Reset()
			{
				this.listEnumerator.Reset();
			}

			public object Current
			{
				get
				{
					return this.listEnumerator.Current;
				}
			}

			public DictionaryEntry Entry
			{
				get
				{
					return (DictionaryEntry)this.listEnumerator.Current;
				}
			}

			public object Key
			{
				get
				{
					return this.Entry.Key;
				}
			}

			public object Value
			{
				get
				{
					return this.Entry.Value;
				}
			}
		}

		private class OrderedCollection : ICollection, IEnumerable
		{
			private ArrayList list;

			private bool isKeyList;

			public OrderedCollection(ArrayList list, bool isKeyList)
			{
				this.list = list;
				this.isKeyList = isKeyList;
			}

			public int Count
			{
				get
				{
					return this.list.Count;
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
					return this.list.SyncRoot;
				}
			}

			public void CopyTo(Array array, int index)
			{
				for (int i = 0; i < this.list.Count; i++)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)this.list[i];
					if (this.isKeyList)
					{
						array.SetValue(dictionaryEntry.Key, index + i);
					}
					else
					{
						array.SetValue(dictionaryEntry.Value, index + i);
					}
				}
			}

			public IEnumerator GetEnumerator()
			{
				return new OrderedDictionary.OrderedCollection.OrderedCollectionEnumerator(this.list.GetEnumerator(), this.isKeyList);
			}

			private class OrderedCollectionEnumerator : IEnumerator
			{
				private bool isKeyList;

				private IEnumerator listEnumerator;

				public OrderedCollectionEnumerator(IEnumerator listEnumerator, bool isKeyList)
				{
					this.listEnumerator = listEnumerator;
					this.isKeyList = isKeyList;
				}

				public object Current
				{
					get
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)this.listEnumerator.Current;
						return (!this.isKeyList) ? dictionaryEntry.Value : dictionaryEntry.Key;
					}
				}

				public bool MoveNext()
				{
					return this.listEnumerator.MoveNext();
				}

				public void Reset()
				{
					this.listEnumerator.Reset();
				}
			}
		}
	}
}
