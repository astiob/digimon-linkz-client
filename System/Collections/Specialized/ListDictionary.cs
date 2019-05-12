using System;

namespace System.Collections.Specialized
{
	/// <summary>Implements IDictionary using a singly linked list. Recommended for collections that typically contain 10 items or less.</summary>
	[Serializable]
	public class ListDictionary : ICollection, IDictionary, IEnumerable
	{
		private int count;

		private int version;

		private ListDictionary.DictionaryNode head;

		private IComparer comparer;

		/// <summary>Creates an empty <see cref="T:System.Collections.Specialized.ListDictionary" /> using the default comparer.</summary>
		public ListDictionary()
		{
			this.count = 0;
			this.version = 0;
			this.comparer = null;
			this.head = null;
		}

		/// <summary>Creates an empty <see cref="T:System.Collections.Specialized.ListDictionary" /> using the specified comparer.</summary>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> to use to determine whether two keys are equal.-or- null to use the default comparer, which is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />. </param>
		public ListDictionary(IComparer comparer) : this()
		{
			this.comparer = comparer;
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> that iterates through the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.Specialized.ListDictionary" />.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new ListDictionary.DictionaryNodeEnumerator(this);
		}

		private ListDictionary.DictionaryNode FindEntry(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", "Attempted lookup for a null key.");
			}
			ListDictionary.DictionaryNode next = this.head;
			if (this.comparer == null)
			{
				while (next != null)
				{
					if (key.Equals(next.key))
					{
						break;
					}
					next = next.next;
				}
			}
			else
			{
				while (next != null)
				{
					if (this.comparer.Compare(key, next.key) == 0)
					{
						break;
					}
					next = next.next;
				}
			}
			return next;
		}

		private ListDictionary.DictionaryNode FindEntry(object key, out ListDictionary.DictionaryNode prev)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", "Attempted lookup for a null key.");
			}
			ListDictionary.DictionaryNode next = this.head;
			prev = null;
			if (this.comparer == null)
			{
				while (next != null)
				{
					if (key.Equals(next.key))
					{
						break;
					}
					prev = next;
					next = next.next;
				}
			}
			else
			{
				while (next != null)
				{
					if (this.comparer.Compare(key, next.key) == 0)
					{
						break;
					}
					prev = next;
					next = next.next;
				}
			}
			return next;
		}

		private void AddImpl(object key, object value, ListDictionary.DictionaryNode prev)
		{
			if (prev == null)
			{
				this.head = new ListDictionary.DictionaryNode(key, value, this.head);
			}
			else
			{
				prev.next = new ListDictionary.DictionaryNode(key, value, prev.next);
			}
			this.count++;
			this.version++;
		}

		/// <summary>Gets the number of key/value pairs contained in the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <returns>The number of key/value pairs contained in the <see cref="T:System.Collections.Specialized.ListDictionary" />.</returns>
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.ListDictionary" /> is synchronized (thread safe).</summary>
		/// <returns>This property always returns false.</returns>
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.ListDictionary" />.</returns>
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Copies the <see cref="T:System.Collections.Specialized.ListDictionary" /> entries to a one-dimensional <see cref="T:System.Array" /> instance at the specified index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the <see cref="T:System.Collections.DictionaryEntry" /> objects copied from <see cref="T:System.Collections.Specialized.ListDictionary" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.Specialized.ListDictionary" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.Specialized.ListDictionary" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", "Array cannot be null.");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "index is less than 0");
			}
			if (index > array.Length)
			{
				throw new IndexOutOfRangeException("index is too large");
			}
			if (this.Count > array.Length - index)
			{
				throw new ArgumentException("Not enough room in the array");
			}
			foreach (object obj in this)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				array.SetValue(dictionaryEntry, index++);
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.ListDictionary" /> has a fixed size.</summary>
		/// <returns>This property always returns false.</returns>
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.ListDictionary" /> is read-only.</summary>
		/// <returns>This property always returns false.</returns>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the value associated with the specified key.</summary>
		/// <returns>The value associated with the specified key. If the specified key is not found, attempting to get it returns null, and attempting to set it creates a new entry using the specified key.</returns>
		/// <param name="key">The key whose value to get or set. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		public object this[object key]
		{
			get
			{
				ListDictionary.DictionaryNode dictionaryNode = this.FindEntry(key);
				return (dictionaryNode != null) ? dictionaryNode.value : null;
			}
			set
			{
				ListDictionary.DictionaryNode prev;
				ListDictionary.DictionaryNode dictionaryNode = this.FindEntry(key, out prev);
				if (dictionaryNode != null)
				{
					dictionaryNode.value = value;
				}
				else
				{
					this.AddImpl(key, value, prev);
				}
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the keys in the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the keys in the <see cref="T:System.Collections.Specialized.ListDictionary" />.</returns>
		public ICollection Keys
		{
			get
			{
				return new ListDictionary.DictionaryNodeCollection(this, true);
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.Specialized.ListDictionary" />.</returns>
		public ICollection Values
		{
			get
			{
				return new ListDictionary.DictionaryNodeCollection(this, false);
			}
		}

		/// <summary>Adds an entry with the specified key and value into the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <param name="key">The key of the entry to add. </param>
		/// <param name="value">The value of the entry to add. The value can be null. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">An entry with the same key already exists in the <see cref="T:System.Collections.Specialized.ListDictionary" />. </exception>
		public void Add(object key, object value)
		{
			ListDictionary.DictionaryNode prev;
			ListDictionary.DictionaryNode dictionaryNode = this.FindEntry(key, out prev);
			if (dictionaryNode != null)
			{
				throw new ArgumentException("key", "Duplicate key in add.");
			}
			this.AddImpl(key, value, prev);
		}

		/// <summary>Removes all entries from the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		public void Clear()
		{
			this.head = null;
			this.count = 0;
			this.version++;
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Specialized.ListDictionary" /> contains a specific key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Specialized.ListDictionary" /> contains an entry with the specified key; otherwise, false.</returns>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Specialized.ListDictionary" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		public bool Contains(object key)
		{
			return this.FindEntry(key) != null;
		}

		/// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> that iterates through the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.Specialized.ListDictionary" />.</returns>
		public IDictionaryEnumerator GetEnumerator()
		{
			return new ListDictionary.DictionaryNodeEnumerator(this);
		}

		/// <summary>Removes the entry with the specified key from the <see cref="T:System.Collections.Specialized.ListDictionary" />.</summary>
		/// <param name="key">The key of the entry to remove. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		public void Remove(object key)
		{
			ListDictionary.DictionaryNode dictionaryNode2;
			ListDictionary.DictionaryNode dictionaryNode = this.FindEntry(key, out dictionaryNode2);
			if (dictionaryNode == null)
			{
				return;
			}
			if (dictionaryNode2 == null)
			{
				this.head = dictionaryNode.next;
			}
			else
			{
				dictionaryNode2.next = dictionaryNode.next;
			}
			dictionaryNode.value = null;
			this.count--;
			this.version++;
		}

		[Serializable]
		private class DictionaryNode
		{
			public object key;

			public object value;

			public ListDictionary.DictionaryNode next;

			public DictionaryNode(object key, object value, ListDictionary.DictionaryNode next)
			{
				this.key = key;
				this.value = value;
				this.next = next;
			}
		}

		private class DictionaryNodeEnumerator : IEnumerator, IDictionaryEnumerator
		{
			private ListDictionary dict;

			private bool isAtStart;

			private ListDictionary.DictionaryNode current;

			private int version;

			public DictionaryNodeEnumerator(ListDictionary dict)
			{
				this.dict = dict;
				this.version = dict.version;
				this.Reset();
			}

			private void FailFast()
			{
				if (this.version != this.dict.version)
				{
					throw new InvalidOperationException("The ListDictionary's contents changed after this enumerator was instantiated.");
				}
			}

			public bool MoveNext()
			{
				this.FailFast();
				if (this.current == null && !this.isAtStart)
				{
					return false;
				}
				this.current = ((!this.isAtStart) ? this.current.next : this.dict.head);
				this.isAtStart = false;
				return this.current != null;
			}

			public void Reset()
			{
				this.FailFast();
				this.isAtStart = true;
				this.current = null;
			}

			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			private ListDictionary.DictionaryNode DictionaryNode
			{
				get
				{
					this.FailFast();
					if (this.current == null)
					{
						throw new InvalidOperationException("Enumerator is positioned before the collection's first element or after the last element.");
					}
					return this.current;
				}
			}

			public DictionaryEntry Entry
			{
				get
				{
					object key = this.DictionaryNode.key;
					return new DictionaryEntry(key, this.current.value);
				}
			}

			public object Key
			{
				get
				{
					return this.DictionaryNode.key;
				}
			}

			public object Value
			{
				get
				{
					return this.DictionaryNode.value;
				}
			}
		}

		private class DictionaryNodeCollection : ICollection, IEnumerable
		{
			private ListDictionary dict;

			private bool isKeyList;

			public DictionaryNodeCollection(ListDictionary dict, bool isKeyList)
			{
				this.dict = dict;
				this.isKeyList = isKeyList;
			}

			public int Count
			{
				get
				{
					return this.dict.Count;
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
					return this.dict.SyncRoot;
				}
			}

			public void CopyTo(Array array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array", "Array cannot be null.");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index", "index is less than 0");
				}
				if (index > array.Length)
				{
					throw new IndexOutOfRangeException("index is too large");
				}
				if (this.Count > array.Length - index)
				{
					throw new ArgumentException("Not enough room in the array");
				}
				foreach (object value in this)
				{
					array.SetValue(value, index++);
				}
			}

			public IEnumerator GetEnumerator()
			{
				return new ListDictionary.DictionaryNodeCollection.DictionaryNodeCollectionEnumerator(this.dict.GetEnumerator(), this.isKeyList);
			}

			private class DictionaryNodeCollectionEnumerator : IEnumerator
			{
				private IDictionaryEnumerator inner;

				private bool isKeyList;

				public DictionaryNodeCollectionEnumerator(IDictionaryEnumerator inner, bool isKeyList)
				{
					this.inner = inner;
					this.isKeyList = isKeyList;
				}

				public object Current
				{
					get
					{
						return (!this.isKeyList) ? this.inner.Value : this.inner.Key;
					}
				}

				public bool MoveNext()
				{
					return this.inner.MoveNext();
				}

				public void Reset()
				{
					this.inner.Reset();
				}
			}
		}
	}
}
