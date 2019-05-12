using System;

namespace System.Collections.Generic
{
	/// <summary>Represents a collection of key/value pairs that are sorted on the key.</summary>
	/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
	/// <filterpriority>1</filterpriority>
	[Serializable]
	public class SortedDictionary<TKey, TValue> : ICollection, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, IEnumerable, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>
	{
		private RBTree tree;

		private SortedDictionary<TKey, TValue>.NodeHelper hlp;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.SortedDictionary`2" /> class that is empty and uses the default <see cref="T:System.Collections.Generic.IComparer`1" /> implementation for the key type.</summary>
		public SortedDictionary() : this(null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.SortedDictionary`2" /> class that is empty and uses the specified <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to compare keys.</summary>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.Comparer`1" /> for the type of the key.</param>
		public SortedDictionary(IComparer<TKey> comparer)
		{
			this.hlp = SortedDictionary<TKey, TValue>.NodeHelper.GetHelper(comparer);
			this.tree = new RBTree(this.hlp);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.SortedDictionary`2" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2" /> and uses the default <see cref="T:System.Collections.Generic.IComparer`1" /> implementation for the key type.</summary>
		/// <param name="dictionary">The <see cref="T:System.Collections.Generic.IDictionary`2" /> whose elements are copied to the new <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="dictionary" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="dictionary" /> contains one or more duplicate keys.</exception>
		public SortedDictionary(IDictionary<TKey, TValue> dic) : this(dic, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.SortedDictionary`2" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2" /> and uses the specified <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to compare keys.</summary>
		/// <param name="dictionary">The <see cref="T:System.Collections.Generic.IDictionary`2" /> whose elements are copied to the new <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.Comparer`1" /> for the type of the key.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="dictionary" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="dictionary" /> contains one or more duplicate keys.</exception>
		public SortedDictionary(IDictionary<TKey, TValue> dic, IComparer<TKey> comparer) : this(comparer)
		{
			if (dic == null)
			{
				throw new ArgumentNullException();
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair in dic)
			{
				this.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				return new SortedDictionary<TKey, TValue>.KeyCollection(this);
			}
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				return new SortedDictionary<TKey, TValue>.ValueCollection(this);
			}
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			TValue y;
			return this.TryGetValue(item.Key, out y) && EqualityComparer<TValue>.Default.Equals(item.Value, y);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			TValue y;
			return this.TryGetValue(item.Key, out y) && EqualityComparer<TValue>.Default.Equals(item.Value, y) && this.Remove(item.Key);
		}

		/// <summary>Adds an element with the provided key and value to the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <param name="key">The object to use as the key of the element to add.</param>
		/// <param name="value">The object to use as the value of the element to add.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="key" /> is of a type that is not assignable to the key type <paramref name="TKey" /> of the <see cref="T:System.Collections.IDictionary" />.-or-<paramref name="value" /> is of a type that is not assignable to the value type <paramref name="TValue" /> of the <see cref="T:System.Collections.IDictionary" />.-or-An element with the same key already exists in the <see cref="T:System.Collections.IDictionary" />.</exception>
		void IDictionary.Add(object key, object value)
		{
			this.Add(this.ToKey(key), this.ToValue(value));
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.IDictionary" /> contains an element with the specified key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.IDictionary" /> contains an element with the key; otherwise, false.</returns>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.IDictionary" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		bool IDictionary.Contains(object key)
		{
			return this.ContainsKey(this.ToKey(key));
		}

		/// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.IDictionary" />.</returns>
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new SortedDictionary<TKey, TValue>.Enumerator(this);
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IDictionary" /> has a fixed size.</summary>
		/// <returns>true if the <see cref="T:System.Collections.IDictionary" /> has a fixed size; otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.SortedDictionary`2" />, this property always returns false.</returns>
		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IDictionary" /> is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.IDictionary" /> is read-only; otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.SortedDictionary`2" />, this property always returns false.</returns>
		bool IDictionary.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the keys of the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the keys of the <see cref="T:System.Collections.IDictionary" />.</returns>
		ICollection IDictionary.Keys
		{
			get
			{
				return new SortedDictionary<TKey, TValue>.KeyCollection(this);
			}
		}

		/// <summary>Removes the element with the specified key from the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		void IDictionary.Remove(object key)
		{
			this.Remove(this.ToKey(key));
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.IDictionary" />.</returns>
		ICollection IDictionary.Values
		{
			get
			{
				return new SortedDictionary<TKey, TValue>.ValueCollection(this);
			}
		}

		/// <summary>Gets or sets the element with the specified key.</summary>
		/// <returns>The element with the specified key, or null if <paramref name="key" /> is not in the dictionary or <paramref name="key" /> is of a type that is not assignable to the key type <paramref name="TKey" /> of the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</returns>
		/// <param name="key">The key of the element to get.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">A value is being assigned, and <paramref name="key" /> is of a type that is not assignable to the key type <paramref name="TKey" /> of the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.-or-A value is being assigned, and <paramref name="value" /> is of a type that is not assignable to the value type <paramref name="TValue" /> of the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</exception>
		object IDictionary.this[object key]
		{
			get
			{
				return this[this.ToKey(key)];
			}
			set
			{
				this[this.ToKey(key)] = this.ToValue(value);
			}
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an array, starting at the specified array index.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from the <see cref="T:System.Collections.Generic.ICollection`1" />. The array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or-<paramref name="array" /> does not have zero-based indexing.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.-or-The type of the source <see cref="T:System.Collections.Generic.ICollection`1" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
		void ICollection.CopyTo(Array array, int index)
		{
			if (this.Count == 0)
			{
				return;
			}
			if (array == null)
			{
				throw new ArgumentNullException();
			}
			if (index < 0 || array.Length <= index)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (array.Length - index < this.Count)
			{
				throw new ArgumentException();
			}
			foreach (RBTree.Node node in this.tree)
			{
				SortedDictionary<TKey, TValue>.Node node2 = (SortedDictionary<TKey, TValue>.Node)node;
				array.SetValue(node2.AsDE(), index++);
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.SortedDictionary`2" />, this property always returns false.</returns>
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />. </returns>
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SortedDictionary<TKey, TValue>.Enumerator(this);
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return new SortedDictionary<TKey, TValue>.Enumerator(this);
		}

		/// <summary>Gets the <see cref="T:System.Collections.Generic.IComparer`1" /> used to order the elements of the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</summary>
		/// <returns>The <see cref="T:System.Collections.Generic.IComparer`1" /> used to order the elements of the <see cref="T:System.Collections.Generic.SortedDictionary`2" /></returns>
		public IComparer<TKey> Comparer
		{
			get
			{
				return this.hlp.cmp;
			}
		}

		/// <summary>Gets the number of key/value pairs contained in the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</summary>
		/// <returns>The number of key/value pairs contained in the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</returns>
		public int Count
		{
			get
			{
				return this.tree.Count;
			}
		}

		/// <summary>Gets or sets the value associated with the specified key.</summary>
		/// <returns>The value associated with the specified key. If the specified key is not found, a get operation throws a <see cref="T:System.Collections.Generic.KeyNotFoundException" />, and a set operation creates a new element with the specified key.</returns>
		/// <param name="key">The key of the value to get or set.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		/// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key" /> does not exist in the collection.</exception>
		public TValue this[TKey key]
		{
			get
			{
				SortedDictionary<TKey, TValue>.Node node = (SortedDictionary<TKey, TValue>.Node)this.tree.Lookup<TKey>(key);
				if (node == null)
				{
					throw new KeyNotFoundException();
				}
				return node.value;
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				SortedDictionary<TKey, TValue>.Node node = (SortedDictionary<TKey, TValue>.Node)this.tree.Intern<TKey>(key, null);
				node.value = value;
			}
		}

		/// <summary>Gets a collection containing the keys in the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection" /> containing the keys in the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</returns>
		public SortedDictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				return new SortedDictionary<TKey, TValue>.KeyCollection(this);
			}
		}

		/// <summary>Gets a collection containing the values in the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection" /> containing the values in the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</returns>
		public SortedDictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				return new SortedDictionary<TKey, TValue>.ValueCollection(this);
			}
		}

		/// <summary>Adds an element with the specified key and value into the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add. The value can be null for reference types.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</exception>
		public void Add(TKey key, TValue value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			RBTree.Node node = new SortedDictionary<TKey, TValue>.Node(key, value);
			if (this.tree.Intern<TKey>(key, node) != node)
			{
				throw new ArgumentException("key already present in dictionary", "key");
			}
		}

		/// <summary>Removes all elements from the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</summary>
		public void Clear()
		{
			this.tree.Clear();
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.SortedDictionary`2" /> contains an element with the specified key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.SortedDictionary`2" /> contains an element with the specified key; otherwise, false.</returns>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		public bool ContainsKey(TKey key)
		{
			return this.tree.Lookup<TKey>(key) != null;
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.SortedDictionary`2" /> contains an element with the specified value.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.SortedDictionary`2" /> contains an element with the specified value; otherwise, false.</returns>
		/// <param name="value">The value to locate in the <see cref="T:System.Collections.Generic.SortedDictionary`2" />. The value can be null for reference types.</param>
		public bool ContainsValue(TValue value)
		{
			IEqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			foreach (RBTree.Node node in this.tree)
			{
				SortedDictionary<TKey, TValue>.Node node2 = (SortedDictionary<TKey, TValue>.Node)node;
				if (@default.Equals(value, node2.value))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.SortedDictionary`2" /> to the specified array of <see cref="T:System.Collections.Generic.KeyValuePair`2" /> structures, starting at the specified index.</summary>
		/// <param name="array">The one-dimensional array of <see cref="T:System.Collections.Generic.KeyValuePair`2" /> structures that is the destination of the elements copied from the current <see cref="T:System.Collections.Generic.SortedDictionary`2" /> The array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.SortedDictionary`2" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (this.Count == 0)
			{
				return;
			}
			if (array == null)
			{
				throw new ArgumentNullException();
			}
			if (arrayIndex < 0 || array.Length <= arrayIndex)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (array.Length - arrayIndex < this.Count)
			{
				throw new ArgumentException();
			}
			foreach (RBTree.Node node in this.tree)
			{
				SortedDictionary<TKey, TValue>.Node node2 = (SortedDictionary<TKey, TValue>.Node)node;
				array[arrayIndex++] = node2.AsKV();
			}
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.SortedDictionary`2.Enumerator" /> for the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</returns>
		public SortedDictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return new SortedDictionary<TKey, TValue>.Enumerator(this);
		}

		/// <summary>Removes the element with the specified key from the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</summary>
		/// <returns>true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key" /> is not found in the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</returns>
		/// <param name="key">The key of the element to remove.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		public bool Remove(TKey key)
		{
			return this.tree.Remove<TKey>(key) != null;
		}

		/// <summary>Gets the value associated with the specified key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.SortedDictionary`2" /> contains an element with the specified key; otherwise, false.</returns>
		/// <param name="key">The key of the value to get.</param>
		/// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		public bool TryGetValue(TKey key, out TValue value)
		{
			SortedDictionary<TKey, TValue>.Node node = (SortedDictionary<TKey, TValue>.Node)this.tree.Lookup<TKey>(key);
			value = ((node != null) ? node.value : default(TValue));
			return node != null;
		}

		private TKey ToKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!(key is TKey))
			{
				throw new ArgumentException(string.Format("Key \"{0}\" cannot be converted to the key type {1}.", key, typeof(TKey)));
			}
			return (TKey)((object)key);
		}

		private TValue ToValue(object value)
		{
			if (!(value is TValue) && (value != null || typeof(TValue).IsValueType))
			{
				throw new ArgumentException(string.Format("Value \"{0}\" cannot be converted to the value type {1}.", value, typeof(TValue)));
			}
			return (TValue)((object)value);
		}

		private class Node : RBTree.Node
		{
			public TKey key;

			public TValue value;

			public Node(TKey key)
			{
				this.key = key;
			}

			public Node(TKey key, TValue value)
			{
				this.key = key;
				this.value = value;
			}

			public override void SwapValue(RBTree.Node other)
			{
				SortedDictionary<TKey, TValue>.Node node = (SortedDictionary<TKey, TValue>.Node)other;
				TKey tkey = this.key;
				this.key = node.key;
				node.key = tkey;
				TValue tvalue = this.value;
				this.value = node.value;
				node.value = tvalue;
			}

			public KeyValuePair<TKey, TValue> AsKV()
			{
				return new KeyValuePair<TKey, TValue>(this.key, this.value);
			}

			public DictionaryEntry AsDE()
			{
				return new DictionaryEntry(this.key, this.value);
			}
		}

		private class NodeHelper : RBTree.INodeHelper<TKey>
		{
			public IComparer<TKey> cmp;

			private static SortedDictionary<TKey, TValue>.NodeHelper Default = new SortedDictionary<TKey, TValue>.NodeHelper(Comparer<TKey>.Default);

			private NodeHelper(IComparer<TKey> cmp)
			{
				this.cmp = cmp;
			}

			public int Compare(TKey key, RBTree.Node node)
			{
				return this.cmp.Compare(key, ((SortedDictionary<TKey, TValue>.Node)node).key);
			}

			public RBTree.Node CreateNode(TKey key)
			{
				return new SortedDictionary<TKey, TValue>.Node(key);
			}

			public static SortedDictionary<TKey, TValue>.NodeHelper GetHelper(IComparer<TKey> cmp)
			{
				if (cmp == null || cmp == Comparer<TKey>.Default)
				{
					return SortedDictionary<TKey, TValue>.NodeHelper.Default;
				}
				return new SortedDictionary<TKey, TValue>.NodeHelper(cmp);
			}
		}

		/// <summary>Represents the collection of values in a <see cref="T:System.Collections.Generic.SortedDictionary`2" />. This class cannot be inherited.</summary>
		[Serializable]
		public sealed class ValueCollection : ICollection, IEnumerable, ICollection<TValue>, IEnumerable<TValue>
		{
			private SortedDictionary<TKey, TValue> _dic;

			/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection" /> class that reflects the values in the specified <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</summary>
			/// <param name="dictionary">The <see cref="T:System.Collections.Generic.SortedDictionary`2" /> whose values are reflected in the new <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection" />.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="dictionary" /> is null.</exception>
			public ValueCollection(SortedDictionary<TKey, TValue> dic)
			{
				this._dic = dic;
			}

			void ICollection<TValue>.Add(TValue item)
			{
				throw new NotSupportedException();
			}

			void ICollection<TValue>.Clear()
			{
				throw new NotSupportedException();
			}

			bool ICollection<TValue>.Contains(TValue item)
			{
				return this._dic.ContainsValue(item);
			}

			bool ICollection<TValue>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			bool ICollection<TValue>.Remove(TValue item)
			{
				throw new NotSupportedException();
			}

			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an array, starting at a particular array index.</summary>
			/// <param name="array">The one-dimensional array that is the destination of the elements copied from the <see cref="T:System.Collections.ICollection" />. The array must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is null.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is less than 0.</exception>
			/// <exception cref="T:System.ArgumentException">
			///   <paramref name="array" /> is multidimensional.-or-<paramref name="array" /> does not have zero-based indexing.-or-The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.-or-The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
			void ICollection.CopyTo(Array array, int index)
			{
				if (this.Count == 0)
				{
					return;
				}
				if (array == null)
				{
					throw new ArgumentNullException();
				}
				if (index < 0 || array.Length <= index)
				{
					throw new ArgumentOutOfRangeException();
				}
				if (array.Length - index < this.Count)
				{
					throw new ArgumentException();
				}
				foreach (RBTree.Node node in this._dic.tree)
				{
					SortedDictionary<TKey, TValue>.Node node2 = (SortedDictionary<TKey, TValue>.Node)node;
					array.SetValue(node2.value, index++);
				}
			}

			/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
			/// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection" />, this property always returns false.</returns>
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
			/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection" />, this property always returns the current instance.</returns>
			object ICollection.SyncRoot
			{
				get
				{
					return this._dic;
				}
			}

			/// <summary>Returns an enumerator that iterates through the collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.ValueCollection.Enumerator(this._dic);
			}

			/// <summary>Copies the <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection" /> elements to an existing one-dimensional array, starting at the specified array index.</summary>
			/// <param name="array">The one-dimensional array that is the destination of the elements copied from the <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection" />. The array must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is null.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is less than 0.</exception>
			/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
			public void CopyTo(TValue[] array, int arrayIndex)
			{
				if (this.Count == 0)
				{
					return;
				}
				if (array == null)
				{
					throw new ArgumentNullException();
				}
				if (arrayIndex < 0 || array.Length <= arrayIndex)
				{
					throw new ArgumentOutOfRangeException();
				}
				if (array.Length - arrayIndex < this.Count)
				{
					throw new ArgumentException();
				}
				foreach (RBTree.Node node in this._dic.tree)
				{
					SortedDictionary<TKey, TValue>.Node node2 = (SortedDictionary<TKey, TValue>.Node)node;
					array[arrayIndex++] = node2.value;
				}
			}

			/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection" />.</summary>
			/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection" />.</returns>
			public int Count
			{
				get
				{
					return this._dic.Count;
				}
			}

			/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection" />.</summary>
			/// <returns>A <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection.Enumerator" /> structure for the <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection" />.</returns>
			public SortedDictionary<TKey, TValue>.ValueCollection.Enumerator GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.ValueCollection.Enumerator(this._dic);
			}

			/// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection" />.</summary>
			public struct Enumerator : IEnumerator, IDisposable, IEnumerator<TValue>
			{
				private RBTree.NodeEnumerator host;

				private TValue current;

				internal Enumerator(SortedDictionary<TKey, TValue> dic)
				{
					this.host = dic.tree.GetEnumerator();
				}

				/// <summary>Gets the element at the current position of the enumerator.</summary>
				/// <returns>The element in the collection at the current position of the enumerator.</returns>
				/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
				object IEnumerator.Current
				{
					get
					{
						this.host.check_current();
						return this.current;
					}
				}

				/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
				/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
				void IEnumerator.Reset()
				{
					this.host.Reset();
				}

				/// <summary>Gets the element at the current position of the enumerator.</summary>
				/// <returns>The element in the <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection" /> at the current position of the enumerator.</returns>
				public TValue Current
				{
					get
					{
						return this.current;
					}
				}

				/// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection" />.</summary>
				/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
				/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
				public bool MoveNext()
				{
					if (!this.host.MoveNext())
					{
						return false;
					}
					this.current = ((SortedDictionary<TKey, TValue>.Node)this.host.Current).value;
					return true;
				}

				/// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.SortedDictionary`2.ValueCollection.Enumerator" />.</summary>
				public void Dispose()
				{
					this.host.Dispose();
				}
			}
		}

		/// <summary>Represents the collection of keys in a <see cref="T:System.Collections.Generic.SortedDictionary`2" />. This class cannot be inherited. </summary>
		[Serializable]
		public sealed class KeyCollection : ICollection, IEnumerable, ICollection<TKey>, IEnumerable<TKey>
		{
			private SortedDictionary<TKey, TValue> _dic;

			/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection" /> class that reflects the keys in the specified <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</summary>
			/// <param name="dictionary">The <see cref="T:System.Collections.Generic.SortedDictionary`2" /> whose keys are reflected in the new <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection" />.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="dictionary" /> is null.</exception>
			public KeyCollection(SortedDictionary<TKey, TValue> dic)
			{
				this._dic = dic;
			}

			void ICollection<TKey>.Add(TKey item)
			{
				throw new NotSupportedException();
			}

			void ICollection<TKey>.Clear()
			{
				throw new NotSupportedException();
			}

			bool ICollection<TKey>.Contains(TKey item)
			{
				return this._dic.ContainsKey(item);
			}

			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			bool ICollection<TKey>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			bool ICollection<TKey>.Remove(TKey item)
			{
				throw new NotSupportedException();
			}

			/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an array, starting at a particular array index.</summary>
			/// <param name="array">The one-dimensional array that is the destination of the elements copied from the <see cref="T:System.Collections.ICollection" />. The array must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is null.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is less than 0.</exception>
			/// <exception cref="T:System.ArgumentException">
			///   <paramref name="array" /> is multidimensional.-or-<paramref name="array" /> does not have zero-based indexing.-or-The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.-or-The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
			void ICollection.CopyTo(Array array, int index)
			{
				if (this.Count == 0)
				{
					return;
				}
				if (array == null)
				{
					throw new ArgumentNullException();
				}
				if (index < 0 || array.Length <= index)
				{
					throw new ArgumentOutOfRangeException();
				}
				if (array.Length - index < this.Count)
				{
					throw new ArgumentException();
				}
				foreach (RBTree.Node node in this._dic.tree)
				{
					SortedDictionary<TKey, TValue>.Node node2 = (SortedDictionary<TKey, TValue>.Node)node;
					array.SetValue(node2.key, index++);
				}
			}

			/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
			/// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection" />, this property always returns false.</returns>
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
			/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection" />, this property always returns the current instance.</returns>
			object ICollection.SyncRoot
			{
				get
				{
					return this._dic;
				}
			}

			/// <summary>Returns an enumerator that iterates through the collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.KeyCollection.Enumerator(this._dic);
			}

			/// <summary>Copies the <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection" /> elements to an existing one-dimensional array, starting at the specified array index.</summary>
			/// <param name="array">The one-dimensional array that is the destination of the elements copied from the <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection" />. The array must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is null. </exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is less than 0.</exception>
			/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
			public void CopyTo(TKey[] array, int arrayIndex)
			{
				if (this.Count == 0)
				{
					return;
				}
				if (array == null)
				{
					throw new ArgumentNullException();
				}
				if (arrayIndex < 0 || array.Length <= arrayIndex)
				{
					throw new ArgumentOutOfRangeException();
				}
				if (array.Length - arrayIndex < this.Count)
				{
					throw new ArgumentException();
				}
				foreach (RBTree.Node node in this._dic.tree)
				{
					SortedDictionary<TKey, TValue>.Node node2 = (SortedDictionary<TKey, TValue>.Node)node;
					array[arrayIndex++] = node2.key;
				}
			}

			/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection" />.</summary>
			/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection" />.</returns>
			public int Count
			{
				get
				{
					return this._dic.Count;
				}
			}

			/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection" />.</summary>
			/// <returns>A <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection.Enumerator" /> structure for the <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection" />.</returns>
			public SortedDictionary<TKey, TValue>.KeyCollection.Enumerator GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.KeyCollection.Enumerator(this._dic);
			}

			/// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection" />.</summary>
			public struct Enumerator : IEnumerator, IDisposable, IEnumerator<TKey>
			{
				private RBTree.NodeEnumerator host;

				private TKey current;

				internal Enumerator(SortedDictionary<TKey, TValue> dic)
				{
					this.host = dic.tree.GetEnumerator();
				}

				/// <summary>Gets the element at the current position of the enumerator.</summary>
				/// <returns>The element in the collection at the current position of the enumerator.</returns>
				/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
				object IEnumerator.Current
				{
					get
					{
						this.host.check_current();
						return this.current;
					}
				}

				/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
				/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
				void IEnumerator.Reset()
				{
					this.host.Reset();
				}

				/// <summary>Gets the element at the current position of the enumerator.</summary>
				/// <returns>The element in the <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection" /> at the current position of the enumerator.</returns>
				public TKey Current
				{
					get
					{
						return this.current;
					}
				}

				/// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection" />.</summary>
				/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
				/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
				public bool MoveNext()
				{
					if (!this.host.MoveNext())
					{
						return false;
					}
					this.current = ((SortedDictionary<TKey, TValue>.Node)this.host.Current).key;
					return true;
				}

				/// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.SortedDictionary`2.KeyCollection.Enumerator" />.</summary>
				public void Dispose()
				{
					this.host.Dispose();
				}
			}
		}

		/// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</summary>
		public struct Enumerator : IEnumerator, IDisposable, IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
		{
			private RBTree.NodeEnumerator host;

			private KeyValuePair<TKey, TValue> current;

			internal Enumerator(SortedDictionary<TKey, TValue> dic)
			{
				this.host = dic.tree.GetEnumerator();
			}

			/// <summary>Gets the element at the current position of the enumerator as a <see cref="T:System.Collections.DictionaryEntry" /> structure.</summary>
			/// <returns>The element in the collection at the current position of the dictionary, as a <see cref="T:System.Collections.DictionaryEntry" /> structure.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					return this.CurrentNode.AsDE();
				}
			}

			/// <summary>Gets the key of the element at the current position of the enumerator.</summary>
			/// <returns>The key of the element in the collection at the current position of the enumerator.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
			object IDictionaryEnumerator.Key
			{
				get
				{
					return this.CurrentNode.key;
				}
			}

			/// <summary>Gets the value of the element at the current position of the enumerator.</summary>
			/// <returns>The value of the element in the collection at the current position of the enumerator.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
			object IDictionaryEnumerator.Value
			{
				get
				{
					return this.CurrentNode.value;
				}
			}

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the collection at the current position of the enumerator.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
			object IEnumerator.Current
			{
				get
				{
					return this.CurrentNode.AsDE();
				}
			}

			/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			void IEnumerator.Reset()
			{
				this.host.Reset();
			}

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the <see cref="T:System.Collections.Generic.SortedDictionary`2" /> at the current position of the enumerator.</returns>
			public KeyValuePair<TKey, TValue> Current
			{
				get
				{
					return this.current;
				}
			}

			/// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.SortedDictionary`2" />.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			public bool MoveNext()
			{
				if (!this.host.MoveNext())
				{
					return false;
				}
				this.current = ((SortedDictionary<TKey, TValue>.Node)this.host.Current).AsKV();
				return true;
			}

			/// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.SortedDictionary`2.Enumerator" />.</summary>
			public void Dispose()
			{
				this.host.Dispose();
			}

			private SortedDictionary<TKey, TValue>.Node CurrentNode
			{
				get
				{
					this.host.check_current();
					return (SortedDictionary<TKey, TValue>.Node)this.host.Current;
				}
			}
		}
	}
}
