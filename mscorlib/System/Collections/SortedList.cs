using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Collections
{
	/// <summary>Represents a collection of key/value pairs that are sorted by the keys and are accessible by key and by index.</summary>
	/// <filterpriority>1</filterpriority>
	[DebuggerDisplay("Count={Count}")]
	[ComVisible(true)]
	[Serializable]
	public class SortedList : IEnumerable, ICloneable, ICollection, IDictionary
	{
		private static readonly int INITIAL_SIZE = 16;

		private int inUse;

		private int modificationCount;

		private SortedList.Slot[] table;

		private IComparer comparer;

		private int defaultCapacity;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.SortedList" /> class that is empty, has the default initial capacity, and is sorted according to the <see cref="T:System.IComparable" /> interface implemented by each key added to the <see cref="T:System.Collections.SortedList" /> object.</summary>
		public SortedList() : this(null, SortedList.INITIAL_SIZE)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.SortedList" /> class that is empty, has the specified initial capacity, and is sorted according to the <see cref="T:System.IComparable" /> interface implemented by each key added to the <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <param name="initialCapacity">The initial number of elements that the <see cref="T:System.Collections.SortedList" /> object can contain. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="initialCapacity" /> is less than zero. </exception>
		/// <exception cref="T:System.OutOfMemoryException">There is not enough available memory to create a <see cref="T:System.Collections.SortedList" /> object with the specified <paramref name="initialCapacity" />.</exception>
		public SortedList(int initialCapacity) : this(null, initialCapacity)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.SortedList" /> class that is empty, has the specified initial capacity, and is sorted according to the specified <see cref="T:System.Collections.IComparer" /> interface.</summary>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing keys.-or- null to use the <see cref="T:System.IComparable" /> implementation of each key. </param>
		/// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.SortedList" /> object can contain. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero. </exception>
		/// <exception cref="T:System.OutOfMemoryException">There is not enough available memory to create a <see cref="T:System.Collections.SortedList" /> object with the specified <paramref name="capacity" />.</exception>
		public SortedList(IComparer comparer, int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			if (capacity == 0)
			{
				this.defaultCapacity = 0;
			}
			else
			{
				this.defaultCapacity = SortedList.INITIAL_SIZE;
			}
			this.comparer = comparer;
			this.InitTable(capacity, true);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.SortedList" /> class that is empty, has the default initial capacity, and is sorted according to the specified <see cref="T:System.Collections.IComparer" /> interface.</summary>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing keys.-or- null to use the <see cref="T:System.IComparable" /> implementation of each key. </param>
		public SortedList(IComparer comparer)
		{
			this.comparer = comparer;
			this.InitTable(SortedList.INITIAL_SIZE, true);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.SortedList" /> class that contains elements copied from the specified dictionary, has the same initial capacity as the number of elements copied, and is sorted according to the <see cref="T:System.IComparable" /> interface implemented by each key.</summary>
		/// <param name="d">The <see cref="T:System.Collections.IDictionary" /> implementation to copy to a new <see cref="T:System.Collections.SortedList" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="d" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">One or more elements in <paramref name="d" /> do not implement the <see cref="T:System.IComparable" /> interface. </exception>
		public SortedList(IDictionary d) : this(d, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.SortedList" /> class that contains elements copied from the specified dictionary, has the same initial capacity as the number of elements copied, and is sorted according to the specified <see cref="T:System.Collections.IComparer" /> interface.</summary>
		/// <param name="d">The <see cref="T:System.Collections.IDictionary" /> implementation to copy to a new <see cref="T:System.Collections.SortedList" /> object.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing keys.-or- null to use the <see cref="T:System.IComparable" /> implementation of each key. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="d" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="comparer" /> is null, and one or more elements in <paramref name="d" /> do not implement the <see cref="T:System.IComparable" /> interface. </exception>
		public SortedList(IDictionary d, IComparer comparer)
		{
			if (d == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			this.InitTable(d.Count, true);
			this.comparer = comparer;
			IDictionaryEnumerator enumerator = d.GetEnumerator();
			while (enumerator.MoveNext())
			{
				this.Add(enumerator.Key, enumerator.Value);
			}
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> that iterates through the <see cref="T:System.Collections.SortedList" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.SortedList" />.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SortedList.Enumerator(this, SortedList.EnumeratorMode.ENTRY_MODE);
		}

		/// <summary>Gets the number of elements contained in a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.SortedList" /> object.</returns>
		/// <filterpriority>1</filterpriority>
		public virtual int Count
		{
			get
			{
				return this.inUse;
			}
		}

		/// <summary>Gets a value indicating whether access to a <see cref="T:System.Collections.SortedList" /> object is synchronized (thread safe).</summary>
		/// <returns>true if access to the <see cref="T:System.Collections.SortedList" /> object is synchronized (thread safe); otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.SortedList" /> object.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Gets a value indicating whether a <see cref="T:System.Collections.SortedList" /> object has a fixed size.</summary>
		/// <returns>true if the <see cref="T:System.Collections.SortedList" /> object has a fixed size; otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether a <see cref="T:System.Collections.SortedList" /> object is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.SortedList" /> object is read-only; otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets the keys in a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> object containing the keys in the <see cref="T:System.Collections.SortedList" /> object.</returns>
		/// <filterpriority>1</filterpriority>
		public virtual ICollection Keys
		{
			get
			{
				return new SortedList.ListKeys(this);
			}
		}

		/// <summary>Gets the values in a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> object containing the values in the <see cref="T:System.Collections.SortedList" /> object.</returns>
		/// <filterpriority>1</filterpriority>
		public virtual ICollection Values
		{
			get
			{
				return new SortedList.ListValues(this);
			}
		}

		/// <summary>Gets and sets the value associated with a specific key in a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <returns>The value associated with the <paramref name="key" /> parameter in the <see cref="T:System.Collections.SortedList" /> object, if <paramref name="key" /> is found; otherwise, null.</returns>
		/// <param name="key">The key associated with the value to get or set. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.SortedList" /> object is read-only.-or- The property is set, <paramref name="key" /> does not exist in the collection, and the <see cref="T:System.Collections.SortedList" /> has a fixed size. </exception>
		/// <exception cref="T:System.OutOfMemoryException">There is not enough available memory to add the element to the <see cref="T:System.Collections.SortedList" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The comparer throws an exception. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual object this[object key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException();
				}
				return this.GetImpl(key);
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException();
				}
				if (this.IsReadOnly)
				{
					throw new NotSupportedException("SortedList is Read Only.");
				}
				if (this.Find(key) < 0 && this.IsFixedSize)
				{
					throw new NotSupportedException("Key not found and SortedList is fixed size.");
				}
				this.PutImpl(key, value, true);
			}
		}

		/// <summary>Gets or sets the capacity of a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <returns>The number of elements that the <see cref="T:System.Collections.SortedList" /> object can contain.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value assigned is less than the current number of elements in the <see cref="T:System.Collections.SortedList" /> object.</exception>
		/// <exception cref="T:System.OutOfMemoryException">There is not enough memory available on the system.</exception>
		/// <filterpriority>2</filterpriority>
		public virtual int Capacity
		{
			get
			{
				return this.table.Length;
			}
			set
			{
				int num = this.table.Length;
				if (this.inUse > value)
				{
					throw new ArgumentOutOfRangeException("capacity too small");
				}
				if (value == 0)
				{
					SortedList.Slot[] destinationArray = new SortedList.Slot[this.defaultCapacity];
					Array.Copy(this.table, destinationArray, this.inUse);
					this.table = destinationArray;
				}
				else if (value > this.inUse)
				{
					SortedList.Slot[] destinationArray2 = new SortedList.Slot[value];
					Array.Copy(this.table, destinationArray2, this.inUse);
					this.table = destinationArray2;
				}
				else if (value > num)
				{
					SortedList.Slot[] destinationArray3 = new SortedList.Slot[value];
					Array.Copy(this.table, destinationArray3, num);
					this.table = destinationArray3;
				}
			}
		}

		/// <summary>Adds an element with the specified key and value to a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <param name="key">The key of the element to add. </param>
		/// <param name="value">The value of the element to add. The value can be null. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">An element with the specified <paramref name="key" /> already exists in the <see cref="T:System.Collections.SortedList" /> object.-or- The <see cref="T:System.Collections.SortedList" /> is set to use the <see cref="T:System.IComparable" /> interface, and <paramref name="key" /> does not implement the <see cref="T:System.IComparable" /> interface. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.SortedList" /> is read-only.-or- The <see cref="T:System.Collections.SortedList" /> has a fixed size. </exception>
		/// <exception cref="T:System.OutOfMemoryException">There is not enough available memory to add the element to the <see cref="T:System.Collections.SortedList" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The comparer throws an exception. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void Add(object key, object value)
		{
			this.PutImpl(key, value, false);
		}

		/// <summary>Removes all elements from a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.SortedList" /> object is read-only.-or- The <see cref="T:System.Collections.SortedList" /> has a fixed size. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Clear()
		{
			this.defaultCapacity = SortedList.INITIAL_SIZE;
			this.table = new SortedList.Slot[this.defaultCapacity];
			this.inUse = 0;
			this.modificationCount++;
		}

		/// <summary>Determines whether a <see cref="T:System.Collections.SortedList" /> object contains a specific key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.SortedList" /> object contains an element with the specified <paramref name="key" />; otherwise, false.</returns>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.SortedList" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The comparer throws an exception. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual bool Contains(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException();
			}
			bool result;
			try
			{
				result = (this.Find(key) >= 0);
			}
			catch (Exception)
			{
				throw new InvalidOperationException();
			}
			return result;
		}

		/// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> object that iterates through a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> object for the <see cref="T:System.Collections.SortedList" /> object.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new SortedList.Enumerator(this, SortedList.EnumeratorMode.ENTRY_MODE);
		}

		/// <summary>Removes the element with the specified key from a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <param name="key">The key of the element to remove. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.SortedList" /> object is read-only.-or- The <see cref="T:System.Collections.SortedList" /> has a fixed size. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Remove(object key)
		{
			int num = this.IndexOfKey(key);
			if (num >= 0)
			{
				this.RemoveAt(num);
			}
		}

		/// <summary>Copies <see cref="T:System.Collections.SortedList" /> elements to a one-dimensional <see cref="T:System.Array" /> object, starting at the specified index in the array.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> object that is the destination of the <see cref="T:System.Collections.DictionaryEntry" /> objects copied from <see cref="T:System.Collections.SortedList" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="arrayIndex" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.SortedList" /> object is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.SortedList" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void CopyTo(Array array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException();
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (array.Rank > 1)
			{
				throw new ArgumentException("array is multi-dimensional");
			}
			if (arrayIndex >= array.Length)
			{
				throw new ArgumentNullException("arrayIndex is greater than or equal to array.Length");
			}
			if (this.Count > array.Length - arrayIndex)
			{
				throw new ArgumentNullException("Not enough space in array from arrayIndex to end of array");
			}
			IDictionaryEnumerator enumerator = this.GetEnumerator();
			int num = arrayIndex;
			while (enumerator.MoveNext())
			{
				array.SetValue(enumerator.Entry, num++);
			}
		}

		/// <summary>Creates a shallow copy of a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <returns>A shallow copy of the <see cref="T:System.Collections.SortedList" /> object.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public virtual object Clone()
		{
			return new SortedList(this, this.comparer)
			{
				modificationCount = this.modificationCount
			};
		}

		/// <summary>Gets the keys in a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <returns>An <see cref="T:System.Collections.IList" /> object containing the keys in the <see cref="T:System.Collections.SortedList" /> object.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual IList GetKeyList()
		{
			return new SortedList.ListKeys(this);
		}

		/// <summary>Gets the values in a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <returns>An <see cref="T:System.Collections.IList" /> object containing the values in the <see cref="T:System.Collections.SortedList" /> object.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual IList GetValueList()
		{
			return new SortedList.ListValues(this);
		}

		/// <summary>Removes the element at the specified index of a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <param name="index">The zero-based index of the element to remove. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for the <see cref="T:System.Collections.SortedList" /> object. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.SortedList" /> is read-only.-or- The <see cref="T:System.Collections.SortedList" /> has a fixed size. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void RemoveAt(int index)
		{
			SortedList.Slot[] array = this.table;
			int count = this.Count;
			if (index >= 0 && index < count)
			{
				if (index != count - 1)
				{
					Array.Copy(array, index + 1, array, index, count - 1 - index);
				}
				else
				{
					array[index].key = null;
					array[index].value = null;
				}
				this.inUse--;
				this.modificationCount++;
				return;
			}
			throw new ArgumentOutOfRangeException("index out of range");
		}

		/// <summary>Returns the zero-based index of the specified key in a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <returns>The zero-based index of the <paramref name="key" /> parameter, if <paramref name="key" /> is found in the <see cref="T:System.Collections.SortedList" /> object; otherwise, -1.</returns>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.SortedList" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The comparer throws an exception. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual int IndexOfKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException();
			}
			int num = 0;
			try
			{
				num = this.Find(key);
			}
			catch (Exception)
			{
				throw new InvalidOperationException();
			}
			return num | num >> 31;
		}

		/// <summary>Returns the zero-based index of the first occurrence of the specified value in a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <returns>The zero-based index of the first occurrence of the <paramref name="value" /> parameter, if <paramref name="value" /> is found in the <see cref="T:System.Collections.SortedList" /> object; otherwise, -1.</returns>
		/// <param name="value">The value to locate in the <see cref="T:System.Collections.SortedList" /> object. The value can be null. </param>
		/// <filterpriority>1</filterpriority>
		public virtual int IndexOfValue(object value)
		{
			if (this.inUse == 0)
			{
				return -1;
			}
			for (int i = 0; i < this.inUse; i++)
			{
				SortedList.Slot slot = this.table[i];
				if (object.Equals(value, slot.value))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>Determines whether a <see cref="T:System.Collections.SortedList" /> object contains a specific key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.SortedList" /> object contains an element with the specified <paramref name="key" />; otherwise, false.</returns>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.SortedList" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The comparer throws an exception. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual bool ContainsKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException();
			}
			bool result;
			try
			{
				result = this.Contains(key);
			}
			catch (Exception)
			{
				throw new InvalidOperationException();
			}
			return result;
		}

		/// <summary>Determines whether a <see cref="T:System.Collections.SortedList" /> object contains a specific value.</summary>
		/// <returns>true if the <see cref="T:System.Collections.SortedList" /> object contains an element with the specified <paramref name="value" />; otherwise, false.</returns>
		/// <param name="value">The value to locate in the <see cref="T:System.Collections.SortedList" /> object. The value can be null. </param>
		/// <filterpriority>2</filterpriority>
		public virtual bool ContainsValue(object value)
		{
			return this.IndexOfValue(value) >= 0;
		}

		/// <summary>Gets the value at the specified index of a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <returns>The value at the specified index of the <see cref="T:System.Collections.SortedList" /> object.</returns>
		/// <param name="index">The zero-based index of the value to get. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for the <see cref="T:System.Collections.SortedList" /> object. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual object GetByIndex(int index)
		{
			if (index >= 0 && index < this.Count)
			{
				return this.table[index].value;
			}
			throw new ArgumentOutOfRangeException("index out of range");
		}

		/// <summary>Replaces the value at a specific index in a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <param name="index">The zero-based index at which to save <paramref name="value" />. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to save into the <see cref="T:System.Collections.SortedList" /> object. The value can be null. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for the <see cref="T:System.Collections.SortedList" /> object. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void SetByIndex(int index, object value)
		{
			if (index >= 0 && index < this.Count)
			{
				this.table[index].value = value;
				return;
			}
			throw new ArgumentOutOfRangeException("index out of range");
		}

		/// <summary>Gets the key at the specified index of a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <returns>The key at the specified index of the <see cref="T:System.Collections.SortedList" /> object.</returns>
		/// <param name="index">The zero-based index of the key to get. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for the <see cref="T:System.Collections.SortedList" /> object.</exception>
		/// <filterpriority>2</filterpriority>
		public virtual object GetKey(int index)
		{
			if (index >= 0 && index < this.Count)
			{
				return this.table[index].key;
			}
			throw new ArgumentOutOfRangeException("index out of range");
		}

		/// <summary>Returns a synchronized (thread-safe) wrapper for a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <returns>A synchronized (thread-safe) wrapper for the <see cref="T:System.Collections.SortedList" /> object.</returns>
		/// <param name="list">The <see cref="T:System.Collections.SortedList" /> object to synchronize. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="list" /> is null. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static SortedList Synchronized(SortedList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException(Locale.GetText("Base list is null."));
			}
			return new SortedList.SynchedSortedList(list);
		}

		/// <summary>Sets the capacity to the actual number of elements in a <see cref="T:System.Collections.SortedList" /> object.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.SortedList" /> object is read-only.-or- The <see cref="T:System.Collections.SortedList" /> has a fixed size. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void TrimToSize()
		{
			if (this.Count == 0)
			{
				this.Resize(this.defaultCapacity, false);
			}
			else
			{
				this.Resize(this.Count, true);
			}
		}

		private void Resize(int n, bool copy)
		{
			SortedList.Slot[] sourceArray = this.table;
			SortedList.Slot[] destinationArray = new SortedList.Slot[n];
			if (copy)
			{
				Array.Copy(sourceArray, 0, destinationArray, 0, n);
			}
			this.table = destinationArray;
		}

		private void EnsureCapacity(int n, int free)
		{
			SortedList.Slot[] array = this.table;
			SortedList.Slot[] array2 = null;
			int capacity = this.Capacity;
			bool flag = free >= 0 && free < this.Count;
			if (n > capacity)
			{
				array2 = new SortedList.Slot[n << 1];
			}
			if (array2 != null)
			{
				if (flag)
				{
					if (free > 0)
					{
						Array.Copy(array, 0, array2, 0, free);
					}
					int num = this.Count - free;
					if (num > 0)
					{
						Array.Copy(array, free, array2, free + 1, num);
					}
				}
				else
				{
					Array.Copy(array, array2, this.Count);
				}
				this.table = array2;
			}
			else if (flag)
			{
				Array.Copy(array, free, array, free + 1, this.Count - free);
			}
		}

		private void PutImpl(object key, object value, bool overwrite)
		{
			if (key == null)
			{
				throw new ArgumentNullException("null key");
			}
			SortedList.Slot[] array = this.table;
			int num = -1;
			try
			{
				num = this.Find(key);
			}
			catch (Exception)
			{
				throw new InvalidOperationException();
			}
			if (num >= 0)
			{
				if (!overwrite)
				{
					string text = Locale.GetText("Key '{0}' already exists in list.", new object[]
					{
						key
					});
					throw new ArgumentException(text);
				}
				array[num].value = value;
				this.modificationCount++;
				return;
			}
			else
			{
				num = ~num;
				if (num > this.Capacity + 1)
				{
					throw new Exception(string.Concat(new object[]
					{
						"SortedList::internal error (",
						key,
						", ",
						value,
						") at [",
						num,
						"]"
					}));
				}
				this.EnsureCapacity(this.Count + 1, num);
				array = this.table;
				array[num].key = key;
				array[num].value = value;
				this.inUse++;
				this.modificationCount++;
				return;
			}
		}

		private object GetImpl(object key)
		{
			int num = this.Find(key);
			if (num >= 0)
			{
				return this.table[num].value;
			}
			return null;
		}

		private void InitTable(int capacity, bool forceSize)
		{
			if (!forceSize && capacity < this.defaultCapacity)
			{
				capacity = this.defaultCapacity;
			}
			this.table = new SortedList.Slot[capacity];
			this.inUse = 0;
			this.modificationCount = 0;
		}

		private void CopyToArray(Array arr, int i, SortedList.EnumeratorMode mode)
		{
			if (arr == null)
			{
				throw new ArgumentNullException("arr");
			}
			if (i < 0 || i + this.Count > arr.Length)
			{
				throw new ArgumentOutOfRangeException("i");
			}
			IEnumerator enumerator = new SortedList.Enumerator(this, mode);
			while (enumerator.MoveNext())
			{
				object value = enumerator.Current;
				arr.SetValue(value, i++);
			}
		}

		private int Find(object key)
		{
			SortedList.Slot[] array = this.table;
			int count = this.Count;
			if (count == 0)
			{
				return -1;
			}
			IComparer comparer;
			if (this.comparer == null)
			{
				IComparer @default = Comparer.Default;
				comparer = @default;
			}
			else
			{
				comparer = this.comparer;
			}
			IComparer comparer2 = comparer;
			int i = 0;
			int num = count - 1;
			while (i <= num)
			{
				int num2 = i + num >> 1;
				int num3 = comparer2.Compare(array[num2].key, key);
				if (num3 == 0)
				{
					return num2;
				}
				if (num3 < 0)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2 - 1;
				}
			}
			return ~i;
		}

		[Serializable]
		internal struct Slot
		{
			internal object key;

			internal object value;
		}

		private enum EnumeratorMode
		{
			KEY_MODE,
			VALUE_MODE,
			ENTRY_MODE
		}

		private sealed class Enumerator : IEnumerator, ICloneable, IDictionaryEnumerator
		{
			private SortedList host;

			private int stamp;

			private int pos;

			private int size;

			private SortedList.EnumeratorMode mode;

			private object currentKey;

			private object currentValue;

			private bool invalid;

			private static readonly string xstr = "SortedList.Enumerator: snapshot out of sync.";

			public Enumerator(SortedList host, SortedList.EnumeratorMode mode)
			{
				this.host = host;
				this.stamp = host.modificationCount;
				this.size = host.Count;
				this.mode = mode;
				this.Reset();
			}

			public Enumerator(SortedList host) : this(host, SortedList.EnumeratorMode.ENTRY_MODE)
			{
			}

			public void Reset()
			{
				if (this.host.modificationCount != this.stamp || this.invalid)
				{
					throw new InvalidOperationException(SortedList.Enumerator.xstr);
				}
				this.pos = -1;
				this.currentKey = null;
				this.currentValue = null;
			}

			public bool MoveNext()
			{
				if (this.host.modificationCount != this.stamp || this.invalid)
				{
					throw new InvalidOperationException(SortedList.Enumerator.xstr);
				}
				SortedList.Slot[] table = this.host.table;
				if (++this.pos < this.size)
				{
					SortedList.Slot slot = table[this.pos];
					this.currentKey = slot.key;
					this.currentValue = slot.value;
					return true;
				}
				this.currentKey = null;
				this.currentValue = null;
				return false;
			}

			public DictionaryEntry Entry
			{
				get
				{
					if (this.invalid || this.pos >= this.size || this.pos == -1)
					{
						throw new InvalidOperationException(SortedList.Enumerator.xstr);
					}
					return new DictionaryEntry(this.currentKey, this.currentValue);
				}
			}

			public object Key
			{
				get
				{
					if (this.invalid || this.pos >= this.size || this.pos == -1)
					{
						throw new InvalidOperationException(SortedList.Enumerator.xstr);
					}
					return this.currentKey;
				}
			}

			public object Value
			{
				get
				{
					if (this.invalid || this.pos >= this.size || this.pos == -1)
					{
						throw new InvalidOperationException(SortedList.Enumerator.xstr);
					}
					return this.currentValue;
				}
			}

			public object Current
			{
				get
				{
					if (this.invalid || this.pos >= this.size || this.pos == -1)
					{
						throw new InvalidOperationException(SortedList.Enumerator.xstr);
					}
					switch (this.mode)
					{
					case SortedList.EnumeratorMode.KEY_MODE:
						return this.currentKey;
					case SortedList.EnumeratorMode.VALUE_MODE:
						return this.currentValue;
					case SortedList.EnumeratorMode.ENTRY_MODE:
						return this.Entry;
					default:
						throw new NotSupportedException(this.mode + " is not a supported mode.");
					}
				}
			}

			public object Clone()
			{
				return new SortedList.Enumerator(this.host, this.mode)
				{
					stamp = this.stamp,
					pos = this.pos,
					size = this.size,
					currentKey = this.currentKey,
					currentValue = this.currentValue,
					invalid = this.invalid
				};
			}
		}

		[Serializable]
		private class ListKeys : IEnumerable, ICollection, IList
		{
			private SortedList host;

			public ListKeys(SortedList host)
			{
				if (host == null)
				{
					throw new ArgumentNullException();
				}
				this.host = host;
			}

			public virtual int Count
			{
				get
				{
					return this.host.Count;
				}
			}

			public virtual bool IsSynchronized
			{
				get
				{
					return this.host.IsSynchronized;
				}
			}

			public virtual object SyncRoot
			{
				get
				{
					return this.host.SyncRoot;
				}
			}

			public virtual void CopyTo(Array array, int arrayIndex)
			{
				this.host.CopyToArray(array, arrayIndex, SortedList.EnumeratorMode.KEY_MODE);
			}

			public virtual bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			public virtual bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			public virtual object this[int index]
			{
				get
				{
					return this.host.GetKey(index);
				}
				set
				{
					throw new NotSupportedException("attempt to modify a key");
				}
			}

			public virtual int Add(object value)
			{
				throw new NotSupportedException("IList::Add not supported");
			}

			public virtual void Clear()
			{
				throw new NotSupportedException("IList::Clear not supported");
			}

			public virtual bool Contains(object key)
			{
				return this.host.Contains(key);
			}

			public virtual int IndexOf(object key)
			{
				return this.host.IndexOfKey(key);
			}

			public virtual void Insert(int index, object value)
			{
				throw new NotSupportedException("IList::Insert not supported");
			}

			public virtual void Remove(object value)
			{
				throw new NotSupportedException("IList::Remove not supported");
			}

			public virtual void RemoveAt(int index)
			{
				throw new NotSupportedException("IList::RemoveAt not supported");
			}

			public virtual IEnumerator GetEnumerator()
			{
				return new SortedList.Enumerator(this.host, SortedList.EnumeratorMode.KEY_MODE);
			}
		}

		[Serializable]
		private class ListValues : IEnumerable, ICollection, IList
		{
			private SortedList host;

			public ListValues(SortedList host)
			{
				if (host == null)
				{
					throw new ArgumentNullException();
				}
				this.host = host;
			}

			public virtual int Count
			{
				get
				{
					return this.host.Count;
				}
			}

			public virtual bool IsSynchronized
			{
				get
				{
					return this.host.IsSynchronized;
				}
			}

			public virtual object SyncRoot
			{
				get
				{
					return this.host.SyncRoot;
				}
			}

			public virtual void CopyTo(Array array, int arrayIndex)
			{
				this.host.CopyToArray(array, arrayIndex, SortedList.EnumeratorMode.VALUE_MODE);
			}

			public virtual bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			public virtual bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			public virtual object this[int index]
			{
				get
				{
					return this.host.GetByIndex(index);
				}
				set
				{
					throw new NotSupportedException("This operation is not supported on GetValueList return");
				}
			}

			public virtual int Add(object value)
			{
				throw new NotSupportedException("IList::Add not supported");
			}

			public virtual void Clear()
			{
				throw new NotSupportedException("IList::Clear not supported");
			}

			public virtual bool Contains(object value)
			{
				return this.host.ContainsValue(value);
			}

			public virtual int IndexOf(object value)
			{
				return this.host.IndexOfValue(value);
			}

			public virtual void Insert(int index, object value)
			{
				throw new NotSupportedException("IList::Insert not supported");
			}

			public virtual void Remove(object value)
			{
				throw new NotSupportedException("IList::Remove not supported");
			}

			public virtual void RemoveAt(int index)
			{
				throw new NotSupportedException("IList::RemoveAt not supported");
			}

			public virtual IEnumerator GetEnumerator()
			{
				return new SortedList.Enumerator(this.host, SortedList.EnumeratorMode.VALUE_MODE);
			}
		}

		private class SynchedSortedList : SortedList
		{
			private SortedList host;

			public SynchedSortedList(SortedList host)
			{
				if (host == null)
				{
					throw new ArgumentNullException();
				}
				this.host = host;
			}

			public override int Capacity
			{
				get
				{
					object syncRoot = this.host.SyncRoot;
					int capacity;
					lock (syncRoot)
					{
						capacity = this.host.Capacity;
					}
					return capacity;
				}
				set
				{
					object syncRoot = this.host.SyncRoot;
					lock (syncRoot)
					{
						this.host.Capacity = value;
					}
				}
			}

			public override int Count
			{
				get
				{
					return this.host.Count;
				}
			}

			public override bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			public override object SyncRoot
			{
				get
				{
					return this.host.SyncRoot;
				}
			}

			public override bool IsFixedSize
			{
				get
				{
					return this.host.IsFixedSize;
				}
			}

			public override bool IsReadOnly
			{
				get
				{
					return this.host.IsReadOnly;
				}
			}

			public override ICollection Keys
			{
				get
				{
					ICollection result = null;
					object syncRoot = this.host.SyncRoot;
					lock (syncRoot)
					{
						result = this.host.Keys;
					}
					return result;
				}
			}

			public override ICollection Values
			{
				get
				{
					ICollection result = null;
					object syncRoot = this.host.SyncRoot;
					lock (syncRoot)
					{
						result = this.host.Values;
					}
					return result;
				}
			}

			public override object this[object key]
			{
				get
				{
					object syncRoot = this.host.SyncRoot;
					object impl;
					lock (syncRoot)
					{
						impl = this.host.GetImpl(key);
					}
					return impl;
				}
				set
				{
					object syncRoot = this.host.SyncRoot;
					lock (syncRoot)
					{
						this.host.PutImpl(key, value, true);
					}
				}
			}

			public override void CopyTo(Array array, int arrayIndex)
			{
				object syncRoot = this.host.SyncRoot;
				lock (syncRoot)
				{
					this.host.CopyTo(array, arrayIndex);
				}
			}

			public override void Add(object key, object value)
			{
				object syncRoot = this.host.SyncRoot;
				lock (syncRoot)
				{
					this.host.PutImpl(key, value, false);
				}
			}

			public override void Clear()
			{
				object syncRoot = this.host.SyncRoot;
				lock (syncRoot)
				{
					this.host.Clear();
				}
			}

			public override bool Contains(object key)
			{
				object syncRoot = this.host.SyncRoot;
				bool result;
				lock (syncRoot)
				{
					result = (this.host.Find(key) >= 0);
				}
				return result;
			}

			public override IDictionaryEnumerator GetEnumerator()
			{
				object syncRoot = this.host.SyncRoot;
				IDictionaryEnumerator enumerator;
				lock (syncRoot)
				{
					enumerator = this.host.GetEnumerator();
				}
				return enumerator;
			}

			public override void Remove(object key)
			{
				object syncRoot = this.host.SyncRoot;
				lock (syncRoot)
				{
					this.host.Remove(key);
				}
			}

			public override bool ContainsKey(object key)
			{
				object syncRoot = this.host.SyncRoot;
				bool result;
				lock (syncRoot)
				{
					result = this.host.Contains(key);
				}
				return result;
			}

			public override bool ContainsValue(object value)
			{
				object syncRoot = this.host.SyncRoot;
				bool result;
				lock (syncRoot)
				{
					result = this.host.ContainsValue(value);
				}
				return result;
			}

			public override object Clone()
			{
				object syncRoot = this.host.SyncRoot;
				object result;
				lock (syncRoot)
				{
					result = (this.host.Clone() as SortedList);
				}
				return result;
			}

			public override object GetByIndex(int index)
			{
				object syncRoot = this.host.SyncRoot;
				object byIndex;
				lock (syncRoot)
				{
					byIndex = this.host.GetByIndex(index);
				}
				return byIndex;
			}

			public override object GetKey(int index)
			{
				object syncRoot = this.host.SyncRoot;
				object key;
				lock (syncRoot)
				{
					key = this.host.GetKey(index);
				}
				return key;
			}

			public override IList GetKeyList()
			{
				object syncRoot = this.host.SyncRoot;
				IList result;
				lock (syncRoot)
				{
					result = new SortedList.ListKeys(this.host);
				}
				return result;
			}

			public override IList GetValueList()
			{
				object syncRoot = this.host.SyncRoot;
				IList result;
				lock (syncRoot)
				{
					result = new SortedList.ListValues(this.host);
				}
				return result;
			}

			public override void RemoveAt(int index)
			{
				object syncRoot = this.host.SyncRoot;
				lock (syncRoot)
				{
					this.host.RemoveAt(index);
				}
			}

			public override int IndexOfKey(object key)
			{
				object syncRoot = this.host.SyncRoot;
				int result;
				lock (syncRoot)
				{
					result = this.host.IndexOfKey(key);
				}
				return result;
			}

			public override int IndexOfValue(object val)
			{
				object syncRoot = this.host.SyncRoot;
				int result;
				lock (syncRoot)
				{
					result = this.host.IndexOfValue(val);
				}
				return result;
			}

			public override void SetByIndex(int index, object value)
			{
				object syncRoot = this.host.SyncRoot;
				lock (syncRoot)
				{
					this.host.SetByIndex(index, value);
				}
			}

			public override void TrimToSize()
			{
				object syncRoot = this.host.SyncRoot;
				lock (syncRoot)
				{
					this.host.TrimToSize();
				}
			}
		}
	}
}
