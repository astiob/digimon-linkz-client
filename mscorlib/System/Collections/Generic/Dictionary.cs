using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Collections.Generic
{
	/// <summary>Represents a collection of keys and values.</summary>
	/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
	/// <filterpriority>1</filterpriority>
	[DebuggerTypeProxy(typeof(CollectionDebuggerView))]
	[DebuggerDisplay("Count={Count}")]
	[ComVisible(false)]
	[Serializable]
	public class Dictionary<TKey, TValue> : IEnumerable, ISerializable, ICollection, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>, IDictionary, IDeserializationCallback
	{
		private const int INITIAL_SIZE = 10;

		private const float DEFAULT_LOAD_FACTOR = 0.9f;

		private const int NO_SLOT = -1;

		private const int HASH_FLAG = -2147483648;

		private int[] table;

		private Link[] linkSlots;

		private TKey[] keySlots;

		private TValue[] valueSlots;

		private int touchedSlots;

		private int emptySlot;

		private int count;

		private int threshold;

		private IEqualityComparer<TKey> hcp;

		private SerializationInfo serialization_info;

		private int generation;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.</summary>
		public Dictionary()
		{
			this.Init(10, null);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the default initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
		public Dictionary(IEqualityComparer<TKey> comparer)
		{
			this.Init(10, comparer);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2" /> and uses the default equality comparer for the key type.</summary>
		/// <param name="dictionary">The <see cref="T:System.Collections.Generic.IDictionary`2" /> whose elements are copied to the new <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="dictionary" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="dictionary" /> contains one or more duplicate keys.</exception>
		public Dictionary(IDictionary<TKey, TValue> dictionary) : this(dictionary, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.</summary>
		/// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than 0.</exception>
		public Dictionary(int capacity)
		{
			this.Init(capacity, null);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2" /> and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
		/// <param name="dictionary">The <see cref="T:System.Collections.Generic.IDictionary`2" /> whose elements are copied to the new <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="dictionary" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="dictionary" /> contains one or more duplicate keys.</exception>
		public Dictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			int capacity = dictionary.Count;
			this.Init(capacity, comparer);
			foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
			{
				this.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the specified initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
		/// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than 0.</exception>
		public Dictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			this.Init(capacity, comparer);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class with serialized data.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
		protected Dictionary(SerializationInfo info, StreamingContext context)
		{
			this.serialization_info = info;
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				return this.Values;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the keys of the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the keys of the <see cref="T:System.Collections.IDictionary" />.</returns>
		ICollection IDictionary.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.IDictionary" />.</returns>
		ICollection IDictionary.Values
		{
			get
			{
				return this.Values;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IDictionary" /> has a fixed size.</summary>
		/// <returns>true if the <see cref="T:System.Collections.IDictionary" /> has a fixed size; otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2" />, this property always returns false.</returns>
		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IDictionary" /> is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.IDictionary" /> is read-only; otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2" />, this property always returns false.</returns>
		bool IDictionary.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the value with the specified key.</summary>
		/// <returns>The value associated with the specified key, or null if <paramref name="key" /> is not in the dictionary or <paramref name="key" /> is of a type that is not assignable to the key type <paramref name="TKey" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
		/// <param name="key">The key of the value to get.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">A value is being assigned, and <paramref name="key" /> is of a type that is not assignable to the key type <paramref name="TKey" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.-or-A value is being assigned, and <paramref name="value" /> is of a type that is not assignable to the value type <paramref name="TValue" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.</exception>
		object IDictionary.this[object key]
		{
			get
			{
				if (key is TKey && this.ContainsKey((TKey)((object)key)))
				{
					return this[this.ToTKey(key)];
				}
				return null;
			}
			set
			{
				this[this.ToTKey(key)] = this.ToTValue(value);
			}
		}

		/// <summary>Adds the specified key and value to the dictionary.</summary>
		/// <param name="key">The object to use as the key.</param>
		/// <param name="value">The object to use as the value.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="key" /> is of a type that is not assignable to the key type <paramref name="TKey" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.-or-<paramref name="value" /> is of a type that is not assignable to <paramref name="TValue" />, the type of values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.-or-A value with the same key already exists in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</exception>
		void IDictionary.Add(object key, object value)
		{
			this.Add(this.ToTKey(key), this.ToTValue(value));
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.IDictionary" /> contains an element with the specified key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.IDictionary" /> contains an element with the specified key; otherwise, false.</returns>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.IDictionary" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		bool IDictionary.Contains(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return key is TKey && this.ContainsKey((TKey)((object)key));
		}

		/// <summary>Removes the element with the specified key from the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		void IDictionary.Remove(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (key is TKey)
			{
				this.Remove((TKey)((object)key));
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2" />, this property always returns false.</returns>
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

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
		{
			this.Add(keyValuePair.Key, keyValuePair.Value);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
		{
			return this.ContainsKeyValuePair(keyValuePair);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			this.CopyTo(array, index);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
		{
			return this.ContainsKeyValuePair(keyValuePair) && this.Remove(keyValuePair.Key);
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an array, starting at the specified array index.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or-<paramref name="array" /> does not have zero-based indexing.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.-or-The type of the source <see cref="T:System.Collections.Generic.ICollection`1" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
		void ICollection.CopyTo(Array array, int index)
		{
			KeyValuePair<TKey, TValue>[] array2 = array as KeyValuePair<TKey, TValue>[];
			if (array2 != null)
			{
				this.CopyTo(array2, index);
				return;
			}
			this.CopyToCheck(array, index);
			DictionaryEntry[] array3 = array as DictionaryEntry[];
			if (array3 != null)
			{
				this.Do_CopyTo<DictionaryEntry, DictionaryEntry>(array3, index, (TKey key, TValue value) => new DictionaryEntry(key, value));
				return;
			}
			this.Do_ICollectionCopyTo<KeyValuePair<TKey, TValue>>(array, index, new Dictionary<TKey, TValue>.Transform<KeyValuePair<TKey, TValue>>(Dictionary<TKey, TValue>.make_pair));
		}

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.Enumerator(this);
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.Enumerator(this);
		}

		/// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.IDictionary" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.IDictionary" />.</returns>
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.ShimEnumerator(this);
		}

		/// <summary>Gets the number of key/value pairs contained in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		/// <returns>The number of key/value pairs contained in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
		public int Count
		{
			get
			{
				return this.count;
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
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				int num = this.hcp.GetHashCode(key) | int.MinValue;
				for (int num2 = this.table[(num & int.MaxValue) % this.table.Length] - 1; num2 != -1; num2 = this.linkSlots[num2].Next)
				{
					if (this.linkSlots[num2].HashCode == num && this.hcp.Equals(this.keySlots[num2], key))
					{
						return this.valueSlots[num2];
					}
				}
				throw new KeyNotFoundException();
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				int num = this.hcp.GetHashCode(key) | int.MinValue;
				int num2 = (num & int.MaxValue) % this.table.Length;
				int num3 = this.table[num2] - 1;
				int num4 = -1;
				if (num3 != -1)
				{
					while (this.linkSlots[num3].HashCode != num || !this.hcp.Equals(this.keySlots[num3], key))
					{
						num4 = num3;
						num3 = this.linkSlots[num3].Next;
						if (num3 == -1)
						{
							break;
						}
					}
				}
				if (num3 == -1)
				{
					if (++this.count > this.threshold)
					{
						this.Resize();
						num2 = (num & int.MaxValue) % this.table.Length;
					}
					num3 = this.emptySlot;
					if (num3 == -1)
					{
						num3 = this.touchedSlots++;
					}
					else
					{
						this.emptySlot = this.linkSlots[num3].Next;
					}
					this.linkSlots[num3].Next = this.table[num2] - 1;
					this.table[num2] = num3 + 1;
					this.linkSlots[num3].HashCode = num;
					this.keySlots[num3] = key;
				}
				else if (num4 != -1)
				{
					this.linkSlots[num4].Next = this.linkSlots[num3].Next;
					this.linkSlots[num3].Next = this.table[num2] - 1;
					this.table[num2] = num3 + 1;
				}
				this.valueSlots[num3] = value;
				this.generation++;
			}
		}

		private void Init(int capacity, IEqualityComparer<TKey> hcp)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			this.hcp = ((hcp == null) ? EqualityComparer<TKey>.Default : hcp);
			if (capacity == 0)
			{
				capacity = 10;
			}
			capacity = (int)((float)capacity / 0.9f) + 1;
			this.InitArrays(capacity);
			this.generation = 0;
		}

		private void InitArrays(int size)
		{
			this.table = new int[size];
			this.linkSlots = new Link[size];
			this.emptySlot = -1;
			this.keySlots = new TKey[size];
			this.valueSlots = new TValue[size];
			this.touchedSlots = 0;
			this.threshold = (int)((float)this.table.Length * 0.9f);
			if (this.threshold == 0 && this.table.Length > 0)
			{
				this.threshold = 1;
			}
		}

		private void CopyToCheck(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (index > array.Length)
			{
				throw new ArgumentException("index larger than largest valid index of array");
			}
			if (array.Length - index < this.Count)
			{
				throw new ArgumentException("Destination array cannot hold the requested elements!");
			}
		}

		private void Do_CopyTo<TRet, TElem>(TElem[] array, int index, Dictionary<TKey, TValue>.Transform<TRet> transform) where TRet : TElem
		{
			for (int i = 0; i < this.touchedSlots; i++)
			{
				if ((this.linkSlots[i].HashCode & -2147483648) != 0)
				{
					array[index++] = (TElem)((object)transform(this.keySlots[i], this.valueSlots[i]));
				}
			}
		}

		private static KeyValuePair<TKey, TValue> make_pair(TKey key, TValue value)
		{
			return new KeyValuePair<TKey, TValue>(key, value);
		}

		private static TKey pick_key(TKey key, TValue value)
		{
			return key;
		}

		private static TValue pick_value(TKey key, TValue value)
		{
			return value;
		}

		private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			this.CopyToCheck(array, index);
			this.Do_CopyTo<KeyValuePair<TKey, TValue>, KeyValuePair<TKey, TValue>>(array, index, new Dictionary<TKey, TValue>.Transform<KeyValuePair<TKey, TValue>>(Dictionary<TKey, TValue>.make_pair));
		}

		private void Do_ICollectionCopyTo<TRet>(Array array, int index, Dictionary<TKey, TValue>.Transform<TRet> transform)
		{
			Type typeFromHandle = typeof(TRet);
			Type elementType = array.GetType().GetElementType();
			try
			{
				if ((typeFromHandle.IsPrimitive || elementType.IsPrimitive) && !elementType.IsAssignableFrom(typeFromHandle))
				{
					throw new Exception();
				}
				this.Do_CopyTo<TRet, object>((object[])array, index, transform);
			}
			catch (Exception innerException)
			{
				throw new ArgumentException("Cannot copy source collection elements to destination array", "array", innerException);
			}
		}

		private void Resize()
		{
			int num = Hashtable.ToPrime(this.table.Length << 1 | 1);
			int[] array = new int[num];
			Link[] array2 = new Link[num];
			for (int i = 0; i < this.table.Length; i++)
			{
				for (int num2 = this.table[i] - 1; num2 != -1; num2 = this.linkSlots[num2].Next)
				{
					int num3 = array2[num2].HashCode = (this.hcp.GetHashCode(this.keySlots[num2]) | int.MinValue);
					int num4 = (num3 & int.MaxValue) % num;
					array2[num2].Next = array[num4] - 1;
					array[num4] = num2 + 1;
				}
			}
			this.table = array;
			this.linkSlots = array2;
			TKey[] destinationArray = new TKey[num];
			TValue[] destinationArray2 = new TValue[num];
			Array.Copy(this.keySlots, 0, destinationArray, 0, this.touchedSlots);
			Array.Copy(this.valueSlots, 0, destinationArray2, 0, this.touchedSlots);
			this.keySlots = destinationArray;
			this.valueSlots = destinationArray2;
			this.threshold = (int)((float)num * 0.9f);
		}

		/// <summary>Adds the specified key and value to the dictionary.</summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add. The value can be null for reference types.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</exception>
		public void Add(TKey key, TValue value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int num = this.hcp.GetHashCode(key) | int.MinValue;
			int num2 = (num & int.MaxValue) % this.table.Length;
			int num3;
			for (num3 = this.table[num2] - 1; num3 != -1; num3 = this.linkSlots[num3].Next)
			{
				if (this.linkSlots[num3].HashCode == num && this.hcp.Equals(this.keySlots[num3], key))
				{
					throw new ArgumentException("An element with the same key already exists in the dictionary.");
				}
			}
			if (++this.count > this.threshold)
			{
				this.Resize();
				num2 = (num & int.MaxValue) % this.table.Length;
			}
			num3 = this.emptySlot;
			if (num3 == -1)
			{
				num3 = this.touchedSlots++;
			}
			else
			{
				this.emptySlot = this.linkSlots[num3].Next;
			}
			this.linkSlots[num3].HashCode = num;
			this.linkSlots[num3].Next = this.table[num2] - 1;
			this.table[num2] = num3 + 1;
			this.keySlots[num3] = key;
			this.valueSlots[num3] = value;
			this.generation++;
		}

		/// <summary>Gets the <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> that is used to determine equality of keys for the dictionary. </summary>
		/// <returns>The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> generic interface implementation that is used to determine equality of keys for the current <see cref="T:System.Collections.Generic.Dictionary`2" /> and to provide hash values for the keys.</returns>
		public IEqualityComparer<TKey> Comparer
		{
			get
			{
				return this.hcp;
			}
		}

		/// <summary>Removes all keys and values from the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		public void Clear()
		{
			this.count = 0;
			Array.Clear(this.table, 0, this.table.Length);
			Array.Clear(this.keySlots, 0, this.keySlots.Length);
			Array.Clear(this.valueSlots, 0, this.valueSlots.Length);
			Array.Clear(this.linkSlots, 0, this.linkSlots.Length);
			this.emptySlot = -1;
			this.touchedSlots = 0;
			this.generation++;
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains the specified key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains an element with the specified key; otherwise, false.</returns>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		public bool ContainsKey(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int num = this.hcp.GetHashCode(key) | int.MinValue;
			for (int num2 = this.table[(num & int.MaxValue) % this.table.Length] - 1; num2 != -1; num2 = this.linkSlots[num2].Next)
			{
				if (this.linkSlots[num2].HashCode == num && this.hcp.Equals(this.keySlots[num2], key))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains a specific value.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains an element with the specified value; otherwise, false.</returns>
		/// <param name="value">The value to locate in the <see cref="T:System.Collections.Generic.Dictionary`2" />. The value can be null for reference types.</param>
		public bool ContainsValue(TValue value)
		{
			IEqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			for (int i = 0; i < this.table.Length; i++)
			{
				for (int num = this.table[i] - 1; num != -1; num = this.linkSlots[num].Next)
				{
					if (@default.Equals(this.valueSlots[num], value))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" /> instance.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" /> instance.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2" /> instance.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null.</exception>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("Version", this.generation);
			info.AddValue("Comparer", this.hcp);
			KeyValuePair<TKey, TValue>[] array = null;
			if (this.count > 0)
			{
				array = new KeyValuePair<TKey, TValue>[this.count];
				this.CopyTo(array, 0);
			}
			info.AddValue("HashSize", this.table.Length);
			info.AddValue("KeyValuePairs", array);
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and raises the deserialization event when the deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object associated with the current <see cref="T:System.Collections.Generic.Dictionary`2" /> instance is invalid.</exception>
		public virtual void OnDeserialization(object sender)
		{
			if (this.serialization_info == null)
			{
				return;
			}
			this.generation = this.serialization_info.GetInt32("Version");
			this.hcp = (IEqualityComparer<TKey>)this.serialization_info.GetValue("Comparer", typeof(IEqualityComparer<TKey>));
			int num = this.serialization_info.GetInt32("HashSize");
			KeyValuePair<TKey, TValue>[] array = (KeyValuePair<TKey, TValue>[])this.serialization_info.GetValue("KeyValuePairs", typeof(KeyValuePair<TKey, TValue>[]));
			if (num < 10)
			{
				num = 10;
			}
			this.InitArrays(num);
			this.count = 0;
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					this.Add(array[i].Key, array[i].Value);
				}
			}
			this.generation++;
			this.serialization_info = null;
		}

		/// <summary>Removes the value with the specified key from the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		/// <returns>true if the element is successfully found and removed; otherwise, false.  This method returns false if <paramref name="key" /> is not found in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
		/// <param name="key">The key of the element to remove.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		public bool Remove(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int num = this.hcp.GetHashCode(key) | int.MinValue;
			int num2 = (num & int.MaxValue) % this.table.Length;
			int num3 = this.table[num2] - 1;
			if (num3 == -1)
			{
				return false;
			}
			int num4 = -1;
			while (this.linkSlots[num3].HashCode != num || !this.hcp.Equals(this.keySlots[num3], key))
			{
				num4 = num3;
				num3 = this.linkSlots[num3].Next;
				if (num3 == -1)
				{
					IL_A4:
					if (num3 == -1)
					{
						return false;
					}
					this.count--;
					if (num4 == -1)
					{
						this.table[num2] = this.linkSlots[num3].Next + 1;
					}
					else
					{
						this.linkSlots[num4].Next = this.linkSlots[num3].Next;
					}
					this.linkSlots[num3].Next = this.emptySlot;
					this.emptySlot = num3;
					this.linkSlots[num3].HashCode = 0;
					this.keySlots[num3] = default(TKey);
					this.valueSlots[num3] = default(TValue);
					this.generation++;
					return true;
				}
			}
			goto IL_A4;
		}

		/// <summary>Gets the value associated with the specified key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains an element with the specified key; otherwise, false.</returns>
		/// <param name="key">The key of the value to get.</param>
		/// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int num = this.hcp.GetHashCode(key) | int.MinValue;
			for (int num2 = this.table[(num & int.MaxValue) % this.table.Length] - 1; num2 != -1; num2 = this.linkSlots[num2].Next)
			{
				if (this.linkSlots[num2].HashCode == num && this.hcp.Equals(this.keySlots[num2], key))
				{
					value = this.valueSlots[num2];
					return true;
				}
			}
			value = default(TValue);
			return false;
		}

		/// <summary>Gets a collection containing the keys in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> containing the keys in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
		public Dictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				return new Dictionary<TKey, TValue>.KeyCollection(this);
			}
		}

		/// <summary>Gets a collection containing the values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> containing the values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
		public Dictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				return new Dictionary<TKey, TValue>.ValueCollection(this);
			}
		}

		private TKey ToTKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!(key is TKey))
			{
				throw new ArgumentException("not of type: " + typeof(TKey).ToString(), "key");
			}
			return (TKey)((object)key);
		}

		private TValue ToTValue(object value)
		{
			if (value == null && !typeof(TValue).IsValueType)
			{
				return default(TValue);
			}
			if (!(value is TValue))
			{
				throw new ArgumentException("not of type: " + typeof(TValue).ToString(), "value");
			}
			return (TValue)((object)value);
		}

		private bool ContainsKeyValuePair(KeyValuePair<TKey, TValue> pair)
		{
			TValue y;
			return this.TryGetValue(pair.Key, out y) && EqualityComparer<TValue>.Default.Equals(pair.Value, y);
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.Enumerator" /> structure for the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
		public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.Enumerator(this);
		}

		[Serializable]
		private class ShimEnumerator : IEnumerator, IDictionaryEnumerator
		{
			private Dictionary<TKey, TValue>.Enumerator host_enumerator;

			public ShimEnumerator(Dictionary<TKey, TValue> host)
			{
				this.host_enumerator = host.GetEnumerator();
			}

			public void Dispose()
			{
				this.host_enumerator.Dispose();
			}

			public bool MoveNext()
			{
				return this.host_enumerator.MoveNext();
			}

			public DictionaryEntry Entry
			{
				get
				{
					return ((IDictionaryEnumerator)this.host_enumerator).Entry;
				}
			}

			public object Key
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this.host_enumerator.Current;
					return keyValuePair.Key;
				}
			}

			public object Value
			{
				get
				{
					KeyValuePair<TKey, TValue> keyValuePair = this.host_enumerator.Current;
					return keyValuePair.Value;
				}
			}

			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			public void Reset()
			{
				this.host_enumerator.Reset();
			}
		}

		/// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
		[Serializable]
		public struct Enumerator : IEnumerator, IDisposable, IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
		{
			private Dictionary<TKey, TValue> dictionary;

			private int next;

			private int stamp;

			internal KeyValuePair<TKey, TValue> current;

			internal Enumerator(Dictionary<TKey, TValue> dictionary)
			{
				this.dictionary = dictionary;
				this.stamp = dictionary.generation;
			}

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the collection at the current position of the enumerator, as an <see cref="T:System.Object" />.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
			object IEnumerator.Current
			{
				get
				{
					this.VerifyCurrent();
					return this.current;
				}
			}

			/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			void IEnumerator.Reset()
			{
				this.Reset();
			}

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the dictionary at the current position of the enumerator, as a <see cref="T:System.Collections.DictionaryEntry" />.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					this.VerifyCurrent();
					return new DictionaryEntry(this.current.Key, this.current.Value);
				}
			}

			/// <summary>Gets the key of the element at the current position of the enumerator.</summary>
			/// <returns>The key of the element in the dictionary at the current position of the enumerator.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
			object IDictionaryEnumerator.Key
			{
				get
				{
					return this.CurrentKey;
				}
			}

			/// <summary>Gets the value of the element at the current position of the enumerator.</summary>
			/// <returns>The value of the element in the dictionary at the current position of the enumerator.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
			object IDictionaryEnumerator.Value
			{
				get
				{
					return this.CurrentValue;
				}
			}

			/// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			public bool MoveNext()
			{
				this.VerifyState();
				if (this.next < 0)
				{
					return false;
				}
				while (this.next < this.dictionary.touchedSlots)
				{
					int num = this.next++;
					if ((this.dictionary.linkSlots[num].HashCode & -2147483648) != 0)
					{
						this.current = new KeyValuePair<TKey, TValue>(this.dictionary.keySlots[num], this.dictionary.valueSlots[num]);
						return true;
					}
				}
				this.next = -1;
				return false;
			}

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the <see cref="T:System.Collections.Generic.Dictionary`2" /> at the current position of the enumerator.</returns>
			public KeyValuePair<TKey, TValue> Current
			{
				get
				{
					return this.current;
				}
			}

			internal TKey CurrentKey
			{
				get
				{
					this.VerifyCurrent();
					return this.current.Key;
				}
			}

			internal TValue CurrentValue
			{
				get
				{
					this.VerifyCurrent();
					return this.current.Value;
				}
			}

			internal void Reset()
			{
				this.VerifyState();
				this.next = 0;
			}

			private void VerifyState()
			{
				if (this.dictionary == null)
				{
					throw new ObjectDisposedException(null);
				}
				if (this.dictionary.generation != this.stamp)
				{
					throw new InvalidOperationException("out of sync");
				}
			}

			private void VerifyCurrent()
			{
				this.VerifyState();
				if (this.next <= 0)
				{
					throw new InvalidOperationException("Current is not valid");
				}
			}

			/// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Dictionary`2.Enumerator" />.</summary>
			public void Dispose()
			{
				this.dictionary = null;
			}
		}

		/// <summary>Represents the collection of keys in a <see cref="T:System.Collections.Generic.Dictionary`2" />. This class cannot be inherited. </summary>
		[DebuggerDisplay("Count={Count}")]
		[DebuggerTypeProxy(typeof(CollectionDebuggerView))]
		[Serializable]
		public sealed class KeyCollection : IEnumerable, ICollection, ICollection<TKey>, IEnumerable<TKey>
		{
			private Dictionary<TKey, TValue> dictionary;

			/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> class that reflects the keys in the specified <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
			/// <param name="dictionary">The <see cref="T:System.Collections.Generic.Dictionary`2" /> whose keys are reflected in the new <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="dictionary" /> is null.</exception>
			public KeyCollection(Dictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					throw new ArgumentNullException("dictionary");
				}
				this.dictionary = dictionary;
			}

			void ICollection<TKey>.Add(TKey item)
			{
				throw new NotSupportedException("this is a read-only collection");
			}

			void ICollection<TKey>.Clear()
			{
				throw new NotSupportedException("this is a read-only collection");
			}

			bool ICollection<TKey>.Contains(TKey item)
			{
				return this.dictionary.ContainsKey(item);
			}

			bool ICollection<TKey>.Remove(TKey item)
			{
				throw new NotSupportedException("this is a read-only collection");
			}

			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
			/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is null.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is less than zero.</exception>
			/// <exception cref="T:System.ArgumentException">
			///   <paramref name="array" /> is multidimensional.-or-<paramref name="array" /> does not have zero-based indexing.-or-The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.-or-The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
			void ICollection.CopyTo(Array array, int index)
			{
				TKey[] array2 = array as TKey[];
				if (array2 != null)
				{
					this.CopyTo(array2, index);
					return;
				}
				this.dictionary.CopyToCheck(array, index);
				this.dictionary.Do_ICollectionCopyTo<TKey>(array, index, new Dictionary<TKey, TValue>.Transform<TKey>(Dictionary<TKey, TValue>.pick_key));
			}

			/// <summary>Returns an enumerator that iterates through a collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
			IEnumerator IEnumerable.GetEnumerator()
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

			/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
			/// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />, this property always returns false.</returns>
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
			/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />, this property always returns the current instance.</returns>
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this.dictionary).SyncRoot;
				}
			}

			/// <summary>Copies the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
			/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is null. </exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is less than zero.</exception>
			/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
			public void CopyTo(TKey[] array, int index)
			{
				this.dictionary.CopyToCheck(array, index);
				this.dictionary.Do_CopyTo<TKey, TKey>(array, index, new Dictionary<TKey, TValue>.Transform<TKey>(Dictionary<TKey, TValue>.pick_key));
			}

			/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
			/// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection.Enumerator" /> for the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</returns>
			public Dictionary<TKey, TValue>.KeyCollection.Enumerator GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}

			/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
			/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.Retrieving the value of this property is an O(1) operation.</returns>
			public int Count
			{
				get
				{
					return this.dictionary.Count;
				}
			}

			/// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
			[Serializable]
			public struct Enumerator : IEnumerator, IDisposable, IEnumerator<TKey>
			{
				private Dictionary<TKey, TValue>.Enumerator host_enumerator;

				internal Enumerator(Dictionary<TKey, TValue> host)
				{
					this.host_enumerator = host.GetEnumerator();
				}

				/// <summary>Gets the element at the current position of the enumerator.</summary>
				/// <returns>The element in the collection at the current position of the enumerator.</returns>
				/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
				object IEnumerator.Current
				{
					get
					{
						return this.host_enumerator.CurrentKey;
					}
				}

				/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
				/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
				void IEnumerator.Reset()
				{
					this.host_enumerator.Reset();
				}

				/// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection.Enumerator" />.</summary>
				public void Dispose()
				{
					this.host_enumerator.Dispose();
				}

				/// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
				/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
				/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
				public bool MoveNext()
				{
					return this.host_enumerator.MoveNext();
				}

				/// <summary>Gets the element at the current position of the enumerator.</summary>
				/// <returns>The element in the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> at the current position of the enumerator.</returns>
				public TKey Current
				{
					get
					{
						return this.host_enumerator.current.Key;
					}
				}
			}
		}

		/// <summary>Represents the collection of values in a <see cref="T:System.Collections.Generic.Dictionary`2" />. This class cannot be inherited. </summary>
		[DebuggerTypeProxy(typeof(CollectionDebuggerView))]
		[DebuggerDisplay("Count={Count}")]
		[Serializable]
		public sealed class ValueCollection : IEnumerable, ICollection, ICollection<TValue>, IEnumerable<TValue>
		{
			private Dictionary<TKey, TValue> dictionary;

			/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> class that reflects the values in the specified <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
			/// <param name="dictionary">The <see cref="T:System.Collections.Generic.Dictionary`2" /> whose values are reflected in the new <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="dictionary" /> is null.</exception>
			public ValueCollection(Dictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					throw new ArgumentNullException("dictionary");
				}
				this.dictionary = dictionary;
			}

			void ICollection<TValue>.Add(TValue item)
			{
				throw new NotSupportedException("this is a read-only collection");
			}

			void ICollection<TValue>.Clear()
			{
				throw new NotSupportedException("this is a read-only collection");
			}

			bool ICollection<TValue>.Contains(TValue item)
			{
				return this.dictionary.ContainsValue(item);
			}

			bool ICollection<TValue>.Remove(TValue item)
			{
				throw new NotSupportedException("this is a read-only collection");
			}

			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
			/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is null.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is less than zero.</exception>
			/// <exception cref="T:System.ArgumentException">
			///   <paramref name="array" /> is multidimensional.-or-<paramref name="array" /> does not have zero-based indexing.-or-The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.-or-The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
			void ICollection.CopyTo(Array array, int index)
			{
				TValue[] array2 = array as TValue[];
				if (array2 != null)
				{
					this.CopyTo(array2, index);
					return;
				}
				this.dictionary.CopyToCheck(array, index);
				this.dictionary.Do_ICollectionCopyTo<TValue>(array, index, new Dictionary<TKey, TValue>.Transform<TValue>(Dictionary<TKey, TValue>.pick_value));
			}

			/// <summary>Returns an enumerator that iterates through a collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			bool ICollection<TValue>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
			/// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />, this property always returns false.</returns>
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
			/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />, this property always returns the current instance.</returns>
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this.dictionary).SyncRoot;
				}
			}

			/// <summary>Copies the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
			/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is null.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is less than zero.</exception>
			/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
			public void CopyTo(TValue[] array, int index)
			{
				this.dictionary.CopyToCheck(array, index);
				this.dictionary.Do_CopyTo<TValue, TValue>(array, index, new Dictionary<TKey, TValue>.Transform<TValue>(Dictionary<TKey, TValue>.pick_value));
			}

			/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
			/// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection.Enumerator" /> for the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</returns>
			public Dictionary<TKey, TValue>.ValueCollection.Enumerator GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}

			/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
			/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</returns>
			public int Count
			{
				get
				{
					return this.dictionary.Count;
				}
			}

			/// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
			[Serializable]
			public struct Enumerator : IEnumerator, IDisposable, IEnumerator<TValue>
			{
				private Dictionary<TKey, TValue>.Enumerator host_enumerator;

				internal Enumerator(Dictionary<TKey, TValue> host)
				{
					this.host_enumerator = host.GetEnumerator();
				}

				/// <summary>Gets the element at the current position of the enumerator.</summary>
				/// <returns>The element in the collection at the current position of the enumerator.</returns>
				/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
				object IEnumerator.Current
				{
					get
					{
						return this.host_enumerator.CurrentValue;
					}
				}

				/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
				/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
				void IEnumerator.Reset()
				{
					this.host_enumerator.Reset();
				}

				/// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection.Enumerator" />.</summary>
				public void Dispose()
				{
					this.host_enumerator.Dispose();
				}

				/// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
				/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
				/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
				public bool MoveNext()
				{
					return this.host_enumerator.MoveNext();
				}

				/// <summary>Gets the element at the current position of the enumerator.</summary>
				/// <returns>The element in the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> at the current position of the enumerator.</returns>
				public TValue Current
				{
					get
					{
						return this.host_enumerator.current.Value;
					}
				}
			}
		}

		private delegate TRet Transform<TRet>(TKey key, TValue value);
	}
}
