using System;
using System.Runtime.Serialization;

namespace System.Collections.Specialized
{
	/// <summary>Provides the abstract base class for a collection of associated <see cref="T:System.String" /> keys and <see cref="T:System.Object" /> values that can be accessed either with the key or with the index.</summary>
	[Serializable]
	public abstract class NameObjectCollectionBase : ICollection, IDeserializationCallback, IEnumerable, ISerializable
	{
		private Hashtable m_ItemsContainer;

		private NameObjectCollectionBase._Item m_NullKeyItem;

		private ArrayList m_ItemsArray;

		private IHashCodeProvider m_hashprovider;

		private IComparer m_comparer;

		private int m_defCapacity;

		private bool m_readonly;

		private SerializationInfo infoCopy;

		private NameObjectCollectionBase.KeysCollection keyscoll;

		private IEqualityComparer equality_comparer;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> class that is empty.</summary>
		protected NameObjectCollectionBase()
		{
			this.m_readonly = false;
			this.m_hashprovider = CaseInsensitiveHashCodeProvider.DefaultInvariant;
			this.m_comparer = CaseInsensitiveComparer.DefaultInvariant;
			this.m_defCapacity = 0;
			this.Init();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> class that is empty, has the specified initial capacity, and uses the default hash code provider and the default comparer.</summary>
		/// <param name="capacity">The approximate number of entries that the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance can initially contain.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero. </exception>
		protected NameObjectCollectionBase(int capacity)
		{
			this.m_readonly = false;
			this.m_hashprovider = CaseInsensitiveHashCodeProvider.DefaultInvariant;
			this.m_comparer = CaseInsensitiveComparer.DefaultInvariant;
			this.m_defCapacity = capacity;
			this.Init();
		}

		internal NameObjectCollectionBase(IEqualityComparer equalityComparer, IComparer comparer, IHashCodeProvider hcp)
		{
			this.equality_comparer = equalityComparer;
			this.m_comparer = comparer;
			this.m_hashprovider = hcp;
			this.m_readonly = false;
			this.m_defCapacity = 0;
			this.Init();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> class that is empty, has the default initial capacity, and uses the specified <see cref="T:System.Collections.IEqualityComparer" /> object.</summary>
		/// <param name="equalityComparer">The <see cref="T:System.Collections.IEqualityComparer" /> object to use to determine whether two keys are equal and to generate hash codes for the keys in the collection.</param>
		protected NameObjectCollectionBase(IEqualityComparer equalityComparer)
		{
			IEqualityComparer equalityComparer2;
			if (equalityComparer == null)
			{
				IEqualityComparer invariantCultureIgnoreCase = StringComparer.InvariantCultureIgnoreCase;
				equalityComparer2 = invariantCultureIgnoreCase;
			}
			else
			{
				equalityComparer2 = equalityComparer;
			}
			this..ctor(equalityComparer2, null, null);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> class that is empty, has the default initial capacity, and uses the specified hash code provider and the specified comparer.</summary>
		/// <param name="hashProvider">The <see cref="T:System.Collections.IHashCodeProvider" /> that will supply the hash codes for all keys in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> to use to determine whether two keys are equal.</param>
		[Obsolete("Use NameObjectCollectionBase(IEqualityComparer)")]
		protected NameObjectCollectionBase(IHashCodeProvider hashProvider, IComparer comparer)
		{
			this.m_comparer = comparer;
			this.m_hashprovider = hashProvider;
			this.m_readonly = false;
			this.m_defCapacity = 0;
			this.Init();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> class that is serializable and uses the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" />.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the new <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains the source and destination of the serialized stream associated with the new <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</param>
		protected NameObjectCollectionBase(SerializationInfo info, StreamingContext context)
		{
			this.infoCopy = info;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> class that is empty, has the specified initial capacity, and uses the specified <see cref="T:System.Collections.IEqualityComparer" /> object.</summary>
		/// <param name="capacity">The approximate number of entries that the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> object can initially contain.</param>
		/// <param name="equalityComparer">The <see cref="T:System.Collections.IEqualityComparer" /> object to use to determine whether two keys are equal and to generate hash codes for the keys in the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero.</exception>
		protected NameObjectCollectionBase(int capacity, IEqualityComparer equalityComparer)
		{
			this.m_readonly = false;
			IEqualityComparer equalityComparer2;
			if (equalityComparer == null)
			{
				IEqualityComparer invariantCultureIgnoreCase = StringComparer.InvariantCultureIgnoreCase;
				equalityComparer2 = invariantCultureIgnoreCase;
			}
			else
			{
				equalityComparer2 = equalityComparer;
			}
			this.equality_comparer = equalityComparer2;
			this.m_defCapacity = capacity;
			this.Init();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> class that is empty, has the specified initial capacity and uses the specified hash code provider and the specified comparer.</summary>
		/// <param name="capacity">The approximate number of entries that the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance can initially contain.</param>
		/// <param name="hashProvider">The <see cref="T:System.Collections.IHashCodeProvider" /> that will supply the hash codes for all keys in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> to use to determine whether two keys are equal.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero.</exception>
		[Obsolete("Use NameObjectCollectionBase(int,IEqualityComparer)")]
		protected NameObjectCollectionBase(int capacity, IHashCodeProvider hashProvider, IComparer comparer)
		{
			this.m_readonly = false;
			this.m_hashprovider = hashProvider;
			this.m_comparer = comparer;
			this.m_defCapacity = capacity;
			this.Init();
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> object is synchronized (thread safe).</summary>
		/// <returns>true if access to the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> object is synchronized (thread safe); otherwise, false. The default is false.</returns>
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> object.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> object.</returns>
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Copies the entire <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this.Keys).CopyTo(array, index);
		}

		internal IEqualityComparer EqualityComparer
		{
			get
			{
				return this.equality_comparer;
			}
		}

		internal IComparer Comparer
		{
			get
			{
				return this.m_comparer;
			}
		}

		internal IHashCodeProvider HashCodeProvider
		{
			get
			{
				return this.m_hashprovider;
			}
		}

		private void Init()
		{
			if (this.equality_comparer != null)
			{
				this.m_ItemsContainer = new Hashtable(this.m_defCapacity, this.equality_comparer);
			}
			else
			{
				this.m_ItemsContainer = new Hashtable(this.m_defCapacity, this.m_hashprovider, this.m_comparer);
			}
			this.m_ItemsArray = new ArrayList();
			this.m_NullKeyItem = null;
		}

		/// <summary>Gets a <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" /> instance that contains all the keys in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" /> instance that contains all the keys in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</returns>
		public virtual NameObjectCollectionBase.KeysCollection Keys
		{
			get
			{
				if (this.keyscoll == null)
				{
					this.keyscoll = new NameObjectCollectionBase.KeysCollection(this);
				}
				return this.keyscoll;
			}
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</returns>
		public virtual IEnumerator GetEnumerator()
		{
			return new NameObjectCollectionBase._KeysEnumerator(this);
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null.</exception>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			int count = this.Count;
			string[] array = new string[count];
			object[] array2 = new object[count];
			int num = 0;
			foreach (object obj in this.m_ItemsArray)
			{
				NameObjectCollectionBase._Item item = (NameObjectCollectionBase._Item)obj;
				array[num] = item.key;
				array2[num] = item.value;
				num++;
			}
			if (this.equality_comparer != null)
			{
				info.AddValue("KeyComparer", this.equality_comparer, typeof(IEqualityComparer));
				info.AddValue("Version", 4, typeof(int));
			}
			else
			{
				info.AddValue("HashProvider", this.m_hashprovider, typeof(IHashCodeProvider));
				info.AddValue("Comparer", this.m_comparer, typeof(IComparer));
				info.AddValue("Version", 2, typeof(int));
			}
			info.AddValue("ReadOnly", this.m_readonly);
			info.AddValue("Count", count);
			info.AddValue("Keys", array, typeof(string[]));
			info.AddValue("Values", array2, typeof(object[]));
		}

		/// <summary>Gets the number of key/value pairs contained in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <returns>The number of key/value pairs contained in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</returns>
		public virtual int Count
		{
			get
			{
				return this.m_ItemsArray.Count;
			}
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and raises the deserialization event when the deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object associated with the current <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance is invalid.</exception>
		public virtual void OnDeserialization(object sender)
		{
			SerializationInfo serializationInfo = this.infoCopy;
			if (serializationInfo == null)
			{
				return;
			}
			this.infoCopy = null;
			this.m_hashprovider = (IHashCodeProvider)serializationInfo.GetValue("HashProvider", typeof(IHashCodeProvider));
			if (this.m_hashprovider == null)
			{
				this.equality_comparer = (IEqualityComparer)serializationInfo.GetValue("KeyComparer", typeof(IEqualityComparer));
			}
			else
			{
				this.m_comparer = (IComparer)serializationInfo.GetValue("Comparer", typeof(IComparer));
				if (this.m_comparer == null)
				{
					throw new SerializationException("The comparer is null");
				}
			}
			this.m_readonly = serializationInfo.GetBoolean("ReadOnly");
			string[] array = (string[])serializationInfo.GetValue("Keys", typeof(string[]));
			if (array == null)
			{
				throw new SerializationException("keys is null");
			}
			object[] array2 = (object[])serializationInfo.GetValue("Values", typeof(object[]));
			if (array2 == null)
			{
				throw new SerializationException("values is null");
			}
			this.Init();
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				this.BaseAdd(array[i], array2[i]);
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance is read-only; otherwise, false.</returns>
		protected bool IsReadOnly
		{
			get
			{
				return this.m_readonly;
			}
			set
			{
				this.m_readonly = value;
			}
		}

		/// <summary>Adds an entry with the specified key and value into the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <param name="name">The <see cref="T:System.String" /> key of the entry to add. The key can be null.</param>
		/// <param name="value">The <see cref="T:System.Object" /> value of the entry to add. The value can be null.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only. </exception>
		protected void BaseAdd(string name, object value)
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException("Collection is read-only");
			}
			NameObjectCollectionBase._Item item = new NameObjectCollectionBase._Item(name, value);
			if (name == null)
			{
				if (this.m_NullKeyItem == null)
				{
					this.m_NullKeyItem = item;
				}
			}
			else if (this.m_ItemsContainer[name] == null)
			{
				this.m_ItemsContainer.Add(name, item);
			}
			this.m_ItemsArray.Add(item);
		}

		/// <summary>Removes all entries from the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		protected void BaseClear()
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException("Collection is read-only");
			}
			this.Init();
		}

		/// <summary>Gets the value of the entry at the specified index of the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the value of the entry at the specified index.</returns>
		/// <param name="index">The zero-based index of the value to get.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the valid range of indexes for the collection. </exception>
		protected object BaseGet(int index)
		{
			return ((NameObjectCollectionBase._Item)this.m_ItemsArray[index]).value;
		}

		/// <summary>Gets the value of the first entry with the specified key from the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the value of the first entry with the specified key, if found; otherwise, null.</returns>
		/// <param name="name">The <see cref="T:System.String" /> key of the entry to get. The key can be null.</param>
		protected object BaseGet(string name)
		{
			NameObjectCollectionBase._Item item = this.FindFirstMatchedItem(name);
			if (item == null)
			{
				return null;
			}
			return item.value;
		}

		/// <summary>Returns a <see cref="T:System.String" /> array that contains all the keys in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <returns>A <see cref="T:System.String" /> array that contains all the keys in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</returns>
		protected string[] BaseGetAllKeys()
		{
			int count = this.m_ItemsArray.Count;
			string[] array = new string[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.BaseGetKey(i);
			}
			return array;
		}

		/// <summary>Returns an <see cref="T:System.Object" /> array that contains all the values in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <returns>An <see cref="T:System.Object" /> array that contains all the values in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</returns>
		protected object[] BaseGetAllValues()
		{
			int count = this.m_ItemsArray.Count;
			object[] array = new object[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.BaseGet(i);
			}
			return array;
		}

		/// <summary>Returns an array of the specified type that contains all the values in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <returns>An array of the specified type that contains all the values in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</returns>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the type of array to return.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> is not a valid <see cref="T:System.Type" />. </exception>
		protected object[] BaseGetAllValues(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("'type' argument can't be null");
			}
			int count = this.m_ItemsArray.Count;
			object[] array = (object[])Array.CreateInstance(type, count);
			for (int i = 0; i < count; i++)
			{
				array[i] = this.BaseGet(i);
			}
			return array;
		}

		/// <summary>Gets the key of the entry at the specified index of the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <returns>A <see cref="T:System.String" /> that represents the key of the entry at the specified index.</returns>
		/// <param name="index">The zero-based index of the key to get.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the valid range of indexes for the collection. </exception>
		protected string BaseGetKey(int index)
		{
			return ((NameObjectCollectionBase._Item)this.m_ItemsArray[index]).key;
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance contains entries whose keys are not null.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance contains entries whose keys are not null; otherwise, false.</returns>
		protected bool BaseHasKeys()
		{
			return this.m_ItemsContainer.Count > 0;
		}

		/// <summary>Removes the entries with the specified key from the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <param name="name">The <see cref="T:System.String" /> key of the entries to remove. The key can be null.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only. </exception>
		protected void BaseRemove(string name)
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException("Collection is read-only");
			}
			if (name != null)
			{
				this.m_ItemsContainer.Remove(name);
			}
			else
			{
				this.m_NullKeyItem = null;
			}
			int num = this.m_ItemsArray.Count;
			int i = 0;
			while (i < num)
			{
				string s = this.BaseGetKey(i);
				if (this.Equals(s, name))
				{
					this.m_ItemsArray.RemoveAt(i);
					num--;
				}
				else
				{
					i++;
				}
			}
		}

		/// <summary>Removes the entry at the specified index of the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <param name="index">The zero-based index of the entry to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the valid range of indexes for the collection.</exception>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		protected void BaseRemoveAt(int index)
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException("Collection is read-only");
			}
			string text = this.BaseGetKey(index);
			if (text != null)
			{
				this.m_ItemsContainer.Remove(text);
			}
			else
			{
				this.m_NullKeyItem = null;
			}
			this.m_ItemsArray.RemoveAt(index);
		}

		/// <summary>Sets the value of the entry at the specified index of the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <param name="index">The zero-based index of the entry to set.</param>
		/// <param name="value">The <see cref="T:System.Object" /> that represents the new value of the entry to set. The value can be null.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the valid range of indexes for the collection.</exception>
		protected void BaseSet(int index, object value)
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException("Collection is read-only");
			}
			NameObjectCollectionBase._Item item = (NameObjectCollectionBase._Item)this.m_ItemsArray[index];
			item.value = value;
		}

		/// <summary>Sets the value of the first entry with the specified key in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance, if found; otherwise, adds an entry with the specified key and value into the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <param name="name">The <see cref="T:System.String" /> key of the entry to set. The key can be null.</param>
		/// <param name="value">The <see cref="T:System.Object" /> that represents the new value of the entry to set. The value can be null.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only. </exception>
		protected void BaseSet(string name, object value)
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException("Collection is read-only");
			}
			NameObjectCollectionBase._Item item = this.FindFirstMatchedItem(name);
			if (item != null)
			{
				item.value = value;
			}
			else
			{
				this.BaseAdd(name, value);
			}
		}

		[MonoTODO]
		private NameObjectCollectionBase._Item FindFirstMatchedItem(string name)
		{
			if (name != null)
			{
				return (NameObjectCollectionBase._Item)this.m_ItemsContainer[name];
			}
			return this.m_NullKeyItem;
		}

		internal bool Equals(string s1, string s2)
		{
			if (this.m_comparer != null)
			{
				return this.m_comparer.Compare(s1, s2) == 0;
			}
			return this.equality_comparer.Equals(s1, s2);
		}

		internal class _Item
		{
			public string key;

			public object value;

			public _Item(string key, object value)
			{
				this.key = key;
				this.value = value;
			}
		}

		[Serializable]
		internal class _KeysEnumerator : IEnumerator
		{
			private NameObjectCollectionBase m_collection;

			private int m_position;

			internal _KeysEnumerator(NameObjectCollectionBase collection)
			{
				this.m_collection = collection;
				this.Reset();
			}

			public object Current
			{
				get
				{
					if (this.m_position < this.m_collection.Count || this.m_position < 0)
					{
						return this.m_collection.BaseGetKey(this.m_position);
					}
					throw new InvalidOperationException();
				}
			}

			public bool MoveNext()
			{
				return ++this.m_position < this.m_collection.Count;
			}

			public void Reset()
			{
				this.m_position = -1;
			}
		}

		/// <summary>Represents a collection of the <see cref="T:System.String" /> keys of a collection. </summary>
		[Serializable]
		public class KeysCollection : ICollection, IEnumerable
		{
			private NameObjectCollectionBase m_collection;

			internal KeysCollection(NameObjectCollectionBase collection)
			{
				this.m_collection = collection;
			}

			/// <summary>Copies the entire <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
			/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
			/// <exception cref="T:System.ArgumentNullException">
			///   <paramref name="array" /> is null. </exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is less than zero. </exception>
			/// <exception cref="T:System.ArgumentException">
			///   <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />. </exception>
			/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
			void ICollection.CopyTo(Array array, int arrayIndex)
			{
				ArrayList itemsArray = this.m_collection.m_ItemsArray;
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException("arrayIndex");
				}
				if (array.Length > 0 && arrayIndex >= array.Length)
				{
					throw new ArgumentException("arrayIndex is equal to or greater than array.Length");
				}
				if (arrayIndex + itemsArray.Count > array.Length)
				{
					throw new ArgumentException("Not enough room from arrayIndex to end of array for this KeysCollection");
				}
				if (array != null && array.Rank > 1)
				{
					throw new ArgumentException("array is multidimensional");
				}
				object[] array2 = (object[])array;
				int i = 0;
				while (i < itemsArray.Count)
				{
					array2[arrayIndex] = ((NameObjectCollectionBase._Item)itemsArray[i]).key;
					i++;
					arrayIndex++;
				}
			}

			/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" /> is synchronized (thread safe).</summary>
			/// <returns>true if access to the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" /> is synchronized (thread safe); otherwise, false. The default is false.</returns>
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" />.</summary>
			/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" />.</returns>
			object ICollection.SyncRoot
			{
				get
				{
					return this.m_collection;
				}
			}

			/// <summary>Gets the key at the specified index of the collection.</summary>
			/// <returns>A <see cref="T:System.String" /> that contains the key at the specified index of the collection.</returns>
			/// <param name="index">The zero-based index of the key to get from the collection. </param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is outside the valid range of indexes for the collection. </exception>
			public virtual string Get(int index)
			{
				return this.m_collection.BaseGetKey(index);
			}

			/// <summary>Gets the number of keys in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" />.</summary>
			/// <returns>The number of keys in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" />.</returns>
			public int Count
			{
				get
				{
					return this.m_collection.Count;
				}
			}

			/// <summary>Gets the entry at the specified index of the collection.</summary>
			/// <returns>The <see cref="T:System.String" /> key of the entry at the specified index of the collection.</returns>
			/// <param name="index">The zero-based index of the entry to locate in the collection. </param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="index" /> is outside the valid range of indexes for the collection. </exception>
			public string this[int index]
			{
				get
				{
					return this.Get(index);
				}
			}

			/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" />.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" />.</returns>
			public IEnumerator GetEnumerator()
			{
				return new NameObjectCollectionBase._KeysEnumerator(this.m_collection);
			}
		}
	}
}
