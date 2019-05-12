using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Collections
{
	/// <summary>Implements the <see cref="T:System.Collections.IList" /> interface using an array whose size is dynamically increased as required.</summary>
	/// <filterpriority>1</filterpriority>
	[DebuggerDisplay("Count={Count}")]
	[DebuggerTypeProxy(typeof(CollectionDebuggerView))]
	[ComVisible(true)]
	[Serializable]
	public class ArrayList : IEnumerable, ICloneable, ICollection, IList
	{
		private const int DefaultInitialCapacity = 4;

		private int _size;

		private object[] _items;

		private int _version;

		private static readonly object[] EmptyArray = new object[0];

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.ArrayList" /> class that is empty and has the default initial capacity.</summary>
		public ArrayList()
		{
			this._items = ArrayList.EmptyArray;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.ArrayList" /> class that contains elements copied from the specified collection and that has the same initial capacity as the number of elements copied.</summary>
		/// <param name="c">The <see cref="T:System.Collections.ICollection" /> whose elements are copied to the new list. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="c" /> is null. </exception>
		public ArrayList(ICollection c)
		{
			if (c == null)
			{
				throw new ArgumentNullException("c");
			}
			Array array = c as Array;
			if (array != null && array.Rank != 1)
			{
				throw new RankException();
			}
			this._items = new object[c.Count];
			this.AddRange(c);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.ArrayList" /> class that is empty and has the specified initial capacity.</summary>
		/// <param name="capacity">The number of elements that the new list can initially store. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero. </exception>
		public ArrayList(int capacity)
		{
			if (capacity < 0)
			{
				ArrayList.ThrowNewArgumentOutOfRangeException("capacity", capacity, "The initial capacity can't be smaller than zero.");
			}
			if (capacity == 0)
			{
				capacity = 4;
			}
			this._items = new object[capacity];
		}

		private ArrayList(int initialCapacity, bool forceZeroSize)
		{
			if (forceZeroSize)
			{
				this._items = null;
				return;
			}
			throw new InvalidOperationException("Use ArrayList(int)");
		}

		private ArrayList(object[] array, int index, int count)
		{
			if (count == 0)
			{
				this._items = new object[4];
			}
			else
			{
				this._items = new object[count];
			}
			Array.Copy(array, index, this._items, 0, count);
			this._size = count;
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <returns>The element at the specified index.</returns>
		/// <param name="index">The zero-based index of the element to get or set. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.ArrayList.Count" />. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual object this[int index]
		{
			get
			{
				if (index < 0 || index >= this._size)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("index", index, "Index is less than 0 or more than or equal to the list count.");
				}
				return this._items[index];
			}
			set
			{
				if (index < 0 || index >= this._size)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("index", index, "Index is less than 0 or more than or equal to the list count.");
				}
				this._items[index] = value;
				this._version++;
			}
		}

		/// <summary>Gets the number of elements actually contained in the <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <returns>The number of elements actually contained in the <see cref="T:System.Collections.ArrayList" />.</returns>
		/// <filterpriority>1</filterpriority>
		public virtual int Count
		{
			get
			{
				return this._size;
			}
		}

		/// <summary>Gets or sets the number of elements that the <see cref="T:System.Collections.ArrayList" /> can contain.</summary>
		/// <returns>The number of elements that the <see cref="T:System.Collections.ArrayList" /> can contain.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <see cref="P:System.Collections.ArrayList.Capacity" /> is set to a value that is less than <see cref="P:System.Collections.ArrayList.Count" />.</exception>
		/// <exception cref="T:System.OutOfMemoryException">There is not enough memory available on the system.</exception>
		/// <filterpriority>1</filterpriority>
		public virtual int Capacity
		{
			get
			{
				return this._items.Length;
			}
			set
			{
				if (value < this._size)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("Capacity", value, "Must be more than count.");
				}
				object[] array = new object[value];
				Array.Copy(this._items, 0, array, 0, this._size);
				this._items = array;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.ArrayList" /> has a fixed size.</summary>
		/// <returns>true if the <see cref="T:System.Collections.ArrayList" /> has a fixed size; otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.ArrayList" /> is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.ArrayList" /> is read-only; otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ArrayList" /> is synchronized (thread safe).</summary>
		/// <returns>true if access to the <see cref="T:System.Collections.ArrayList" /> is synchronized (thread safe); otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ArrayList" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		private void EnsureCapacity(int count)
		{
			if (count <= this._items.Length)
			{
				return;
			}
			int i = this._items.Length << 1;
			if (i == 0)
			{
				i = 4;
			}
			while (i < count)
			{
				i <<= 1;
			}
			object[] array = new object[i];
			Array.Copy(this._items, 0, array, 0, this._items.Length);
			this._items = array;
		}

		private void Shift(int index, int count)
		{
			if (count > 0)
			{
				if (this._size + count > this._items.Length)
				{
					int i;
					for (i = ((this._items.Length <= 0) ? 1 : (this._items.Length << 1)); i < this._size + count; i <<= 1)
					{
					}
					object[] array = new object[i];
					Array.Copy(this._items, 0, array, 0, index);
					Array.Copy(this._items, index, array, index + count, this._size - index);
					this._items = array;
				}
				else
				{
					Array.Copy(this._items, index, this._items, index + count, this._size - index);
				}
			}
			else if (count < 0)
			{
				int num = index - count;
				Array.Copy(this._items, num, this._items, index, this._size - num);
				Array.Clear(this._items, this._size + count, -count);
			}
		}

		/// <summary>Adds an object to the end of the <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <returns>The <see cref="T:System.Collections.ArrayList" /> index at which the <paramref name="value" /> has been added.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to be added to the end of the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual int Add(object value)
		{
			if (this._items.Length <= this._size)
			{
				this.EnsureCapacity(this._size + 1);
			}
			this._items[this._size] = value;
			this._version++;
			return this._size++;
		}

		/// <summary>Removes all elements from the <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Clear()
		{
			Array.Clear(this._items, 0, this._size);
			this._size = 0;
			this._version++;
		}

		/// <summary>Determines whether an element is in the <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <returns>true if <paramref name="item" /> is found in the <see cref="T:System.Collections.ArrayList" />; otherwise, false.</returns>
		/// <param name="item">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
		/// <filterpriority>1</filterpriority>
		public virtual bool Contains(object item)
		{
			return this.IndexOf(item, 0, this._size) > -1;
		}

		internal virtual bool Contains(object value, int startIndex, int count)
		{
			return this.IndexOf(value, startIndex, count) > -1;
		}

		/// <summary>Searches for the specified <see cref="T:System.Object" /> and returns the zero-based index of the first occurrence within the entire <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the entire <see cref="T:System.Collections.ArrayList" />, if found; otherwise, -1.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
		/// <filterpriority>1</filterpriority>
		public virtual int IndexOf(object value)
		{
			return this.IndexOf(value, 0);
		}

		/// <summary>Searches for the specified <see cref="T:System.Object" /> and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that extends from the specified index to the last element.</summary>
		/// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that extends from <paramref name="startIndex" /> to the last element, if found; otherwise, -1.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
		/// <param name="startIndex">The zero-based starting index of the search. 0 (zero) is valid in an empty array.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.ArrayList" />. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual int IndexOf(object value, int startIndex)
		{
			return this.IndexOf(value, startIndex, this._size - startIndex);
		}

		/// <summary>Searches for the specified <see cref="T:System.Object" /> and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that starts at the specified index and contains the specified number of elements.</summary>
		/// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that starts at <paramref name="startIndex" /> and contains <paramref name="count" /> number of elements, if found; otherwise, -1.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
		/// <param name="startIndex">The zero-based starting index of the search. 0 (zero) is valid in an empty array.</param>
		/// <param name="count">The number of elements in the section to search. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.ArrayList" />.-or- <paramref name="count" /> is less than zero.-or- <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in the <see cref="T:System.Collections.ArrayList" />. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual int IndexOf(object value, int startIndex, int count)
		{
			if (startIndex < 0 || startIndex > this._size)
			{
				ArrayList.ThrowNewArgumentOutOfRangeException("startIndex", startIndex, "Does not specify valid index.");
			}
			if (count < 0)
			{
				ArrayList.ThrowNewArgumentOutOfRangeException("count", count, "Can't be less than 0.");
			}
			if (startIndex > this._size - count)
			{
				throw new ArgumentOutOfRangeException("count", "Start index and count do not specify a valid range.");
			}
			return Array.IndexOf<object>(this._items, value, startIndex, count);
		}

		/// <summary>Searches for the specified <see cref="T:System.Object" /> and returns the zero-based index of the last occurrence within the entire <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <returns>The zero-based index of the last occurrence of <paramref name="value" /> within the entire the <see cref="T:System.Collections.ArrayList" />, if found; otherwise, -1.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
		/// <filterpriority>2</filterpriority>
		public virtual int LastIndexOf(object value)
		{
			return this.LastIndexOf(value, this._size - 1);
		}

		/// <summary>Searches for the specified <see cref="T:System.Object" /> and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that extends from the first element to the specified index.</summary>
		/// <returns>The zero-based index of the last occurrence of <paramref name="value" /> within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that extends from the first element to <paramref name="startIndex" />, if found; otherwise, -1.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
		/// <param name="startIndex">The zero-based starting index of the backward search. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.ArrayList" />. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual int LastIndexOf(object value, int startIndex)
		{
			return this.LastIndexOf(value, startIndex, startIndex + 1);
		}

		/// <summary>Searches for the specified <see cref="T:System.Object" /> and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that contains the specified number of elements and ends at the specified index.</summary>
		/// <returns>The zero-based index of the last occurrence of <paramref name="value" /> within the range of elements in the <see cref="T:System.Collections.ArrayList" /> that contains <paramref name="count" /> number of elements and ends at <paramref name="startIndex" />, if found; otherwise, -1.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
		/// <param name="startIndex">The zero-based starting index of the backward search. </param>
		/// <param name="count">The number of elements in the section to search. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.ArrayList" />.-or- <paramref name="count" /> is less than zero.-or- <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in the <see cref="T:System.Collections.ArrayList" />. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual int LastIndexOf(object value, int startIndex, int count)
		{
			return Array.LastIndexOf<object>(this._items, value, startIndex, count);
		}

		/// <summary>Inserts an element into the <see cref="T:System.Collections.ArrayList" /> at the specified index.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to insert. The value can be null. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is greater than <see cref="P:System.Collections.ArrayList.Count" />. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Insert(int index, object value)
		{
			if (index < 0 || index > this._size)
			{
				ArrayList.ThrowNewArgumentOutOfRangeException("index", index, "Index must be >= 0 and <= Count.");
			}
			this.Shift(index, 1);
			this._items[index] = value;
			this._size++;
			this._version++;
		}

		/// <summary>Inserts the elements of a collection into the <see cref="T:System.Collections.ArrayList" /> at the specified index.</summary>
		/// <param name="index">The zero-based index at which the new elements should be inserted. </param>
		/// <param name="c">The <see cref="T:System.Collections.ICollection" /> whose elements should be inserted into the <see cref="T:System.Collections.ArrayList" />. The collection itself cannot be null, but it can contain elements that are null. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="c" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is greater than <see cref="P:System.Collections.ArrayList.Count" />. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void InsertRange(int index, ICollection c)
		{
			if (c == null)
			{
				throw new ArgumentNullException("c");
			}
			if (index < 0 || index > this._size)
			{
				ArrayList.ThrowNewArgumentOutOfRangeException("index", index, "Index must be >= 0 and <= Count.");
			}
			int count = c.Count;
			if (this._items.Length < this._size + count)
			{
				this.EnsureCapacity(this._size + count);
			}
			if (index < this._size)
			{
				Array.Copy(this._items, index, this._items, index + count, this._size - index);
			}
			if (this == c.SyncRoot)
			{
				Array.Copy(this._items, 0, this._items, index, index);
				Array.Copy(this._items, index + count, this._items, index << 1, this._size - index);
			}
			else
			{
				c.CopyTo(this._items, index);
			}
			this._size += c.Count;
			this._version++;
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Remove(object obj)
		{
			int num = this.IndexOf(obj);
			if (num > -1)
			{
				this.RemoveAt(num);
			}
			this._version++;
		}

		/// <summary>Removes the element at the specified index of the <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <param name="index">The zero-based index of the element to remove. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.ArrayList.Count" />. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void RemoveAt(int index)
		{
			if (index < 0 || index >= this._size)
			{
				ArrayList.ThrowNewArgumentOutOfRangeException("index", index, "Less than 0 or more than list count.");
			}
			this.Shift(index, -1);
			this._size--;
			this._version++;
		}

		/// <summary>Removes a range of elements from the <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <param name="index">The zero-based starting index of the range of elements to remove. </param>
		/// <param name="count">The number of elements to remove. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="count" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="count" /> do not denote a valid range of elements in the <see cref="T:System.Collections.ArrayList" />. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void RemoveRange(int index, int count)
		{
			ArrayList.CheckRange(index, count, this._size);
			this.Shift(index, -count);
			this._size -= count;
			this._version++;
		}

		/// <summary>Reverses the order of the elements in the entire <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void Reverse()
		{
			Array.Reverse(this._items, 0, this._size);
			this._version++;
		}

		/// <summary>Reverses the order of the elements in the specified range.</summary>
		/// <param name="index">The zero-based starting index of the range to reverse. </param>
		/// <param name="count">The number of elements in the range to reverse. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="count" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="count" /> do not denote a valid range of elements in the <see cref="T:System.Collections.ArrayList" />. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void Reverse(int index, int count)
		{
			ArrayList.CheckRange(index, count, this._size);
			Array.Reverse(this._items, index, count);
			this._version++;
		}

		/// <summary>Copies the entire <see cref="T:System.Collections.ArrayList" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the beginning of the target array.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ArrayList" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ArrayList" /> is greater than the number of elements that the destination <paramref name="array" /> can contain. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ArrayList" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void CopyTo(Array array)
		{
			Array.Copy(this._items, array, this._size);
		}

		/// <summary>Copies the entire <see cref="T:System.Collections.ArrayList" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ArrayList" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="arrayIndex" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ArrayList" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ArrayList" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void CopyTo(Array array, int arrayIndex)
		{
			this.CopyTo(0, array, arrayIndex, this._size);
		}

		/// <summary>Copies a range of elements from the <see cref="T:System.Collections.ArrayList" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
		/// <param name="index">The zero-based index in the source <see cref="T:System.Collections.ArrayList" /> at which copying begins. </param>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ArrayList" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		/// <param name="count">The number of elements to copy. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="arrayIndex" /> is less than zero.-or- <paramref name="count" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- <paramref name="index" /> is equal to or greater than the <see cref="P:System.Collections.ArrayList.Count" /> of the source <see cref="T:System.Collections.ArrayList" />.-or- The number of elements from <paramref name="index" /> to the end of the source <see cref="T:System.Collections.ArrayList" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ArrayList" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void CopyTo(int index, Array array, int arrayIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException("Must have only 1 dimensions.", "array");
			}
			Array.Copy(this._items, index, array, arrayIndex, count);
		}

		/// <summary>Returns an enumerator for the entire <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the entire <see cref="T:System.Collections.ArrayList" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual IEnumerator GetEnumerator()
		{
			return new ArrayList.SimpleEnumerator(this);
		}

		/// <summary>Returns an enumerator for a range of elements in the <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the specified range of elements in the <see cref="T:System.Collections.ArrayList" />.</returns>
		/// <param name="index">The zero-based starting index of the <see cref="T:System.Collections.ArrayList" /> section that the enumerator should refer to. </param>
		/// <param name="count">The number of elements in the <see cref="T:System.Collections.ArrayList" /> section that the enumerator should refer to. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="count" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="count" /> do not specify a valid range in the <see cref="T:System.Collections.ArrayList" />. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual IEnumerator GetEnumerator(int index, int count)
		{
			ArrayList.CheckRange(index, count, this._size);
			return new ArrayList.ArrayListEnumerator(this, index, count);
		}

		/// <summary>Adds the elements of an <see cref="T:System.Collections.ICollection" /> to the end of the <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <param name="c">The <see cref="T:System.Collections.ICollection" /> whose elements should be added to the end of the <see cref="T:System.Collections.ArrayList" />. The collection itself cannot be null, but it can contain elements that are null. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="c" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void AddRange(ICollection c)
		{
			this.InsertRange(this._size, c);
		}

		/// <summary>Searches the entire sorted <see cref="T:System.Collections.ArrayList" /> for an element using the default comparer and returns the zero-based index of the element.</summary>
		/// <returns>The zero-based index of <paramref name="value" /> in the sorted <see cref="T:System.Collections.ArrayList" />, if <paramref name="value" /> is found; otherwise, a negative number, which is the bitwise complement of the index of the next element that is larger than <paramref name="value" /> or, if there is no larger element, the bitwise complement of <see cref="P:System.Collections.ArrayList.Count" />.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to locate. The value can be null. </param>
		/// <exception cref="T:System.ArgumentException">Neither <paramref name="value" /> nor the elements of <see cref="T:System.Collections.ArrayList" /> implement the <see cref="T:System.IComparable" /> interface. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="value" /> is not of the same type as the elements of the <see cref="T:System.Collections.ArrayList" />. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual int BinarySearch(object value)
		{
			int result;
			try
			{
				result = Array.BinarySearch<object>(this._items, 0, this._size, value);
			}
			catch (InvalidOperationException ex)
			{
				throw new ArgumentException(ex.Message);
			}
			return result;
		}

		/// <summary>Searches the entire sorted <see cref="T:System.Collections.ArrayList" /> for an element using the specified comparer and returns the zero-based index of the element.</summary>
		/// <returns>The zero-based index of <paramref name="value" /> in the sorted <see cref="T:System.Collections.ArrayList" />, if <paramref name="value" /> is found; otherwise, a negative number, which is the bitwise complement of the index of the next element that is larger than <paramref name="value" /> or, if there is no larger element, the bitwise complement of <see cref="P:System.Collections.ArrayList.Count" />.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to locate. The value can be null. </param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.-or- null to use the default comparer that is the <see cref="T:System.IComparable" /> implementation of each element. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparer" /> is null and neither <paramref name="value" /> nor the elements of <see cref="T:System.Collections.ArrayList" /> implement the <see cref="T:System.IComparable" /> interface. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is null and <paramref name="value" /> is not of the same type as the elements of the <see cref="T:System.Collections.ArrayList" />. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual int BinarySearch(object value, IComparer comparer)
		{
			int result;
			try
			{
				result = Array.BinarySearch(this._items, 0, this._size, value, comparer);
			}
			catch (InvalidOperationException ex)
			{
				throw new ArgumentException(ex.Message);
			}
			return result;
		}

		/// <summary>Searches a range of elements in the sorted <see cref="T:System.Collections.ArrayList" /> for an element using the specified comparer and returns the zero-based index of the element.</summary>
		/// <returns>The zero-based index of <paramref name="value" /> in the sorted <see cref="T:System.Collections.ArrayList" />, if <paramref name="value" /> is found; otherwise, a negative number, which is the bitwise complement of the index of the next element that is larger than <paramref name="value" /> or, if there is no larger element, the bitwise complement of <see cref="P:System.Collections.ArrayList.Count" />.</returns>
		/// <param name="index">The zero-based starting index of the range to search. </param>
		/// <param name="count">The length of the range to search. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to locate. The value can be null. </param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.-or- null to use the default comparer that is the <see cref="T:System.IComparable" /> implementation of each element. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in the <see cref="T:System.Collections.ArrayList" />.-or- <paramref name="comparer" /> is null and neither <paramref name="value" /> nor the elements of <see cref="T:System.Collections.ArrayList" /> implement the <see cref="T:System.IComparable" /> interface. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is null and <paramref name="value" /> is not of the same type as the elements of the <see cref="T:System.Collections.ArrayList" />. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="count" /> is less than zero. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual int BinarySearch(int index, int count, object value, IComparer comparer)
		{
			int result;
			try
			{
				result = Array.BinarySearch(this._items, index, count, value, comparer);
			}
			catch (InvalidOperationException ex)
			{
				throw new ArgumentException(ex.Message);
			}
			return result;
		}

		/// <summary>Returns an <see cref="T:System.Collections.ArrayList" /> which represents a subset of the elements in the source <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ArrayList" /> which represents a subset of the elements in the source <see cref="T:System.Collections.ArrayList" />.</returns>
		/// <param name="index">The zero-based <see cref="T:System.Collections.ArrayList" /> index at which the range starts. </param>
		/// <param name="count">The number of elements in the range. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="count" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="count" /> do not denote a valid range of elements in the <see cref="T:System.Collections.ArrayList" />. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual ArrayList GetRange(int index, int count)
		{
			ArrayList.CheckRange(index, count, this._size);
			if (this.IsSynchronized)
			{
				return ArrayList.Synchronized(new ArrayList.RangedArrayList(this, index, count));
			}
			return new ArrayList.RangedArrayList(this, index, count);
		}

		/// <summary>Copies the elements of a collection over a range of elements in the <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <param name="index">The zero-based <see cref="T:System.Collections.ArrayList" /> index at which to start copying the elements of <paramref name="c" />. </param>
		/// <param name="c">The <see cref="T:System.Collections.ICollection" /> whose elements to copy to the <see cref="T:System.Collections.ArrayList" />. The collection itself cannot be null, but it can contain elements that are null. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> plus the number of elements in <paramref name="c" /> is greater than <see cref="P:System.Collections.ArrayList.Count" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="c" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void SetRange(int index, ICollection c)
		{
			if (c == null)
			{
				throw new ArgumentNullException("c");
			}
			if (index < 0 || index + c.Count > this._size)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			c.CopyTo(this._items, index);
			this._version++;
		}

		/// <summary>Sets the capacity to the actual number of elements in the <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only.-or- The <see cref="T:System.Collections.ArrayList" /> has a fixed size. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void TrimToSize()
		{
			if (this._items.Length > this._size)
			{
				object[] array;
				if (this._size == 0)
				{
					array = new object[4];
				}
				else
				{
					array = new object[this._size];
				}
				Array.Copy(this._items, 0, array, 0, this._size);
				this._items = array;
			}
		}

		/// <summary>Sorts the elements in the entire <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Sort()
		{
			Array.Sort<object>(this._items, 0, this._size);
			this._version++;
		}

		/// <summary>Sorts the elements in the entire <see cref="T:System.Collections.ArrayList" /> using the specified comparer.</summary>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.-or- null to use the <see cref="T:System.IComparable" /> implementation of each element. </param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Sort(IComparer comparer)
		{
			Array.Sort(this._items, 0, this._size, comparer);
		}

		/// <summary>Sorts the elements in a range of elements in <see cref="T:System.Collections.ArrayList" /> using the specified comparer.</summary>
		/// <param name="index">The zero-based starting index of the range to sort. </param>
		/// <param name="count">The length of the range to sort. </param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.-or- null to use the <see cref="T:System.IComparable" /> implementation of each element. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="count" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="count" /> do not specify a valid range in the <see cref="T:System.Collections.ArrayList" />. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.ArrayList" /> is read-only. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Sort(int index, int count, IComparer comparer)
		{
			ArrayList.CheckRange(index, count, this._size);
			Array.Sort(this._items, index, count, comparer);
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.ArrayList" /> to a new <see cref="T:System.Object" /> array.</summary>
		/// <returns>An <see cref="T:System.Object" /> array containing copies of the elements of the <see cref="T:System.Collections.ArrayList" />.</returns>
		/// <filterpriority>1</filterpriority>
		public virtual object[] ToArray()
		{
			object[] array = new object[this._size];
			this.CopyTo(array);
			return array;
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.ArrayList" /> to a new array of the specified element type.</summary>
		/// <returns>An array of the specified element type containing copies of the elements of the <see cref="T:System.Collections.ArrayList" />.</returns>
		/// <param name="type">The element <see cref="T:System.Type" /> of the destination array to create and copy elements to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ArrayList" /> cannot be cast automatically to the specified type. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual Array ToArray(Type type)
		{
			Array array = Array.CreateInstance(type, this._size);
			this.CopyTo(array);
			return array;
		}

		/// <summary>Creates a shallow copy of the <see cref="T:System.Collections.ArrayList" />.</summary>
		/// <returns>A shallow copy of the <see cref="T:System.Collections.ArrayList" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual object Clone()
		{
			return new ArrayList(this._items, 0, this._size);
		}

		internal static void CheckRange(int index, int count, int listCount)
		{
			if (index < 0)
			{
				ArrayList.ThrowNewArgumentOutOfRangeException("index", index, "Can't be less than 0.");
			}
			if (count < 0)
			{
				ArrayList.ThrowNewArgumentOutOfRangeException("count", count, "Can't be less than 0.");
			}
			if (index > listCount - count)
			{
				throw new ArgumentException("Index and count do not denote a valid range of elements.", "index");
			}
		}

		internal static void ThrowNewArgumentOutOfRangeException(string name, object actual, string message)
		{
			throw new ArgumentOutOfRangeException(name, actual, message);
		}

		/// <summary>Creates an <see cref="T:System.Collections.ArrayList" /> wrapper for a specific <see cref="T:System.Collections.IList" />.</summary>
		/// <returns>The <see cref="T:System.Collections.ArrayList" /> wrapper around the <see cref="T:System.Collections.IList" />.</returns>
		/// <param name="list">The <see cref="T:System.Collections.IList" /> to wrap.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="list" /> is null.</exception>
		/// <filterpriority>2</filterpriority>
		public static ArrayList Adapter(IList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			ArrayList arrayList = list as ArrayList;
			if (arrayList != null)
			{
				return arrayList;
			}
			arrayList = new ArrayList.ArrayListAdapter(list);
			if (list.IsSynchronized)
			{
				return ArrayList.Synchronized(arrayList);
			}
			return arrayList;
		}

		/// <summary>Returns an <see cref="T:System.Collections.ArrayList" /> wrapper that is synchronized (thread safe).</summary>
		/// <returns>An <see cref="T:System.Collections.ArrayList" /> wrapper that is synchronized (thread safe).</returns>
		/// <param name="list">The <see cref="T:System.Collections.ArrayList" /> to synchronize. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="list" /> is null. </exception>
		/// <filterpriority>2</filterpriority>
		public static ArrayList Synchronized(ArrayList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (list.IsSynchronized)
			{
				return list;
			}
			return new ArrayList.SynchronizedArrayListWrapper(list);
		}

		/// <summary>Returns an <see cref="T:System.Collections.IList" /> wrapper that is synchronized (thread safe).</summary>
		/// <returns>An <see cref="T:System.Collections.IList" /> wrapper that is synchronized (thread safe).</returns>
		/// <param name="list">The <see cref="T:System.Collections.IList" /> to synchronize. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="list" /> is null. </exception>
		/// <filterpriority>2</filterpriority>
		public static IList Synchronized(IList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (list.IsSynchronized)
			{
				return list;
			}
			return new ArrayList.SynchronizedListWrapper(list);
		}

		/// <summary>Returns a read-only <see cref="T:System.Collections.ArrayList" /> wrapper.</summary>
		/// <returns>A read-only <see cref="T:System.Collections.ArrayList" /> wrapper around <paramref name="list" />.</returns>
		/// <param name="list">The <see cref="T:System.Collections.ArrayList" /> to wrap. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="list" /> is null. </exception>
		/// <filterpriority>2</filterpriority>
		public static ArrayList ReadOnly(ArrayList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (list.IsReadOnly)
			{
				return list;
			}
			return new ArrayList.ReadOnlyArrayListWrapper(list);
		}

		/// <summary>Returns a read-only <see cref="T:System.Collections.IList" /> wrapper.</summary>
		/// <returns>A read-only <see cref="T:System.Collections.IList" /> wrapper around <paramref name="list" />.</returns>
		/// <param name="list">The <see cref="T:System.Collections.IList" /> to wrap. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="list" /> is null. </exception>
		/// <filterpriority>2</filterpriority>
		public static IList ReadOnly(IList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (list.IsReadOnly)
			{
				return list;
			}
			return new ArrayList.ReadOnlyListWrapper(list);
		}

		/// <summary>Returns an <see cref="T:System.Collections.ArrayList" /> wrapper with a fixed size.</summary>
		/// <returns>An <see cref="T:System.Collections.ArrayList" /> wrapper with a fixed size.</returns>
		/// <param name="list">The <see cref="T:System.Collections.ArrayList" /> to wrap. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="list" /> is null. </exception>
		/// <filterpriority>2</filterpriority>
		public static ArrayList FixedSize(ArrayList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (list.IsFixedSize)
			{
				return list;
			}
			return new ArrayList.FixedSizeArrayListWrapper(list);
		}

		/// <summary>Returns an <see cref="T:System.Collections.IList" /> wrapper with a fixed size.</summary>
		/// <returns>An <see cref="T:System.Collections.IList" /> wrapper with a fixed size.</returns>
		/// <param name="list">The <see cref="T:System.Collections.IList" /> to wrap. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="list" /> is null. </exception>
		/// <filterpriority>2</filterpriority>
		public static IList FixedSize(IList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (list.IsFixedSize)
			{
				return list;
			}
			return new ArrayList.FixedSizeListWrapper(list);
		}

		/// <summary>Returns an <see cref="T:System.Collections.ArrayList" /> whose elements are copies of the specified value.</summary>
		/// <returns>An <see cref="T:System.Collections.ArrayList" /> with <paramref name="count" /> number of elements, all of which are copies of <paramref name="value" />.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to copy multiple times in the new <see cref="T:System.Collections.ArrayList" />. The value can be null. </param>
		/// <param name="count">The number of times <paramref name="value" /> should be copied. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is less than zero. </exception>
		/// <filterpriority>2</filterpriority>
		public static ArrayList Repeat(object value, int count)
		{
			ArrayList arrayList = new ArrayList(count);
			for (int i = 0; i < count; i++)
			{
				arrayList.Add(value);
			}
			return arrayList;
		}

		private sealed class ArrayListEnumerator : IEnumerator, ICloneable
		{
			private int m_Pos;

			private int m_Index;

			private int m_Count;

			private object m_Current;

			private ArrayList m_List;

			private int m_ExpectedStateChanges;

			public ArrayListEnumerator(ArrayList list) : this(list, 0, list.Count)
			{
			}

			public ArrayListEnumerator(ArrayList list, int index, int count)
			{
				this.m_List = list;
				this.m_Index = index;
				this.m_Count = count;
				this.m_Pos = this.m_Index - 1;
				this.m_Current = null;
				this.m_ExpectedStateChanges = list._version;
			}

			public object Clone()
			{
				return base.MemberwiseClone();
			}

			public object Current
			{
				get
				{
					if (this.m_Pos == this.m_Index - 1)
					{
						throw new InvalidOperationException("Enumerator unusable (Reset pending, or past end of array.");
					}
					return this.m_Current;
				}
			}

			public bool MoveNext()
			{
				if (this.m_List._version != this.m_ExpectedStateChanges)
				{
					throw new InvalidOperationException("List has changed.");
				}
				this.m_Pos++;
				if (this.m_Pos - this.m_Index < this.m_Count)
				{
					this.m_Current = this.m_List[this.m_Pos];
					return true;
				}
				return false;
			}

			public void Reset()
			{
				this.m_Current = null;
				this.m_Pos = this.m_Index - 1;
			}
		}

		private sealed class SimpleEnumerator : IEnumerator, ICloneable
		{
			private ArrayList list;

			private int index;

			private int version;

			private object currentElement;

			private static object endFlag = new object();

			public SimpleEnumerator(ArrayList list)
			{
				this.list = list;
				this.index = -1;
				this.version = list._version;
				this.currentElement = ArrayList.SimpleEnumerator.endFlag;
			}

			public object Clone()
			{
				return base.MemberwiseClone();
			}

			public bool MoveNext()
			{
				if (this.version != this.list._version)
				{
					throw new InvalidOperationException("List has changed.");
				}
				if (++this.index < this.list.Count)
				{
					this.currentElement = this.list[this.index];
					return true;
				}
				this.currentElement = ArrayList.SimpleEnumerator.endFlag;
				return false;
			}

			public object Current
			{
				get
				{
					if (this.currentElement != ArrayList.SimpleEnumerator.endFlag)
					{
						return this.currentElement;
					}
					if (this.index == -1)
					{
						throw new InvalidOperationException("Enumerator not started");
					}
					throw new InvalidOperationException("Enumerator ended");
				}
			}

			public void Reset()
			{
				if (this.version != this.list._version)
				{
					throw new InvalidOperationException("List has changed.");
				}
				this.currentElement = ArrayList.SimpleEnumerator.endFlag;
				this.index = -1;
			}
		}

		[Serializable]
		private sealed class ArrayListAdapter : ArrayList
		{
			private IList m_Adaptee;

			public ArrayListAdapter(IList adaptee) : base(0, true)
			{
				this.m_Adaptee = adaptee;
			}

			public override object this[int index]
			{
				get
				{
					return this.m_Adaptee[index];
				}
				set
				{
					this.m_Adaptee[index] = value;
				}
			}

			public override int Count
			{
				get
				{
					return this.m_Adaptee.Count;
				}
			}

			public override int Capacity
			{
				get
				{
					return this.m_Adaptee.Count;
				}
				set
				{
					if (value < this.m_Adaptee.Count)
					{
						throw new ArgumentException("capacity");
					}
				}
			}

			public override bool IsFixedSize
			{
				get
				{
					return this.m_Adaptee.IsFixedSize;
				}
			}

			public override bool IsReadOnly
			{
				get
				{
					return this.m_Adaptee.IsReadOnly;
				}
			}

			public override object SyncRoot
			{
				get
				{
					return this.m_Adaptee.SyncRoot;
				}
			}

			public override int Add(object value)
			{
				return this.m_Adaptee.Add(value);
			}

			public override void Clear()
			{
				this.m_Adaptee.Clear();
			}

			public override bool Contains(object value)
			{
				return this.m_Adaptee.Contains(value);
			}

			public override int IndexOf(object value)
			{
				return this.m_Adaptee.IndexOf(value);
			}

			public override int IndexOf(object value, int startIndex)
			{
				return this.IndexOf(value, startIndex, this.m_Adaptee.Count - startIndex);
			}

			public override int IndexOf(object value, int startIndex, int count)
			{
				if (startIndex < 0 || startIndex > this.m_Adaptee.Count)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("startIndex", startIndex, "Does not specify valid index.");
				}
				if (count < 0)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("count", count, "Can't be less than 0.");
				}
				if (startIndex > this.m_Adaptee.Count - count)
				{
					throw new ArgumentOutOfRangeException("count", "Start index and count do not specify a valid range.");
				}
				if (value == null)
				{
					for (int i = startIndex; i < startIndex + count; i++)
					{
						if (this.m_Adaptee[i] == null)
						{
							return i;
						}
					}
				}
				else
				{
					for (int j = startIndex; j < startIndex + count; j++)
					{
						if (value.Equals(this.m_Adaptee[j]))
						{
							return j;
						}
					}
				}
				return -1;
			}

			public override int LastIndexOf(object value)
			{
				return this.LastIndexOf(value, this.m_Adaptee.Count - 1);
			}

			public override int LastIndexOf(object value, int startIndex)
			{
				return this.LastIndexOf(value, startIndex, startIndex + 1);
			}

			public override int LastIndexOf(object value, int startIndex, int count)
			{
				if (startIndex < 0)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("startIndex", startIndex, "< 0");
				}
				if (count < 0)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("count", count, "count is negative.");
				}
				if (startIndex - count + 1 < 0)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("count", count, "count is too large.");
				}
				if (value == null)
				{
					for (int i = startIndex; i > startIndex - count; i--)
					{
						if (this.m_Adaptee[i] == null)
						{
							return i;
						}
					}
				}
				else
				{
					for (int j = startIndex; j > startIndex - count; j--)
					{
						if (value.Equals(this.m_Adaptee[j]))
						{
							return j;
						}
					}
				}
				return -1;
			}

			public override void Insert(int index, object value)
			{
				this.m_Adaptee.Insert(index, value);
			}

			public override void InsertRange(int index, ICollection c)
			{
				if (c == null)
				{
					throw new ArgumentNullException("c");
				}
				if (index > this.m_Adaptee.Count)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("index", index, "Index must be >= 0 and <= Count.");
				}
				foreach (object value in c)
				{
					this.m_Adaptee.Insert(index++, value);
				}
			}

			public override void Remove(object value)
			{
				this.m_Adaptee.Remove(value);
			}

			public override void RemoveAt(int index)
			{
				this.m_Adaptee.RemoveAt(index);
			}

			public override void RemoveRange(int index, int count)
			{
				ArrayList.CheckRange(index, count, this.m_Adaptee.Count);
				for (int i = 0; i < count; i++)
				{
					this.m_Adaptee.RemoveAt(index);
				}
			}

			public override void Reverse()
			{
				this.Reverse(0, this.m_Adaptee.Count);
			}

			public override void Reverse(int index, int count)
			{
				ArrayList.CheckRange(index, count, this.m_Adaptee.Count);
				for (int i = 0; i < count / 2; i++)
				{
					object value = this.m_Adaptee[i + index];
					this.m_Adaptee[i + index] = this.m_Adaptee[index + count - i + index - 1];
					this.m_Adaptee[index + count - i + index - 1] = value;
				}
			}

			public override void SetRange(int index, ICollection c)
			{
				if (c == null)
				{
					throw new ArgumentNullException("c");
				}
				if (index < 0 || index + c.Count > this.m_Adaptee.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				int num = index;
				foreach (object value in c)
				{
					this.m_Adaptee[num++] = value;
				}
			}

			public override void CopyTo(Array array)
			{
				this.m_Adaptee.CopyTo(array, 0);
			}

			public override void CopyTo(Array array, int index)
			{
				this.m_Adaptee.CopyTo(array, index);
			}

			public override void CopyTo(int index, Array array, int arrayIndex, int count)
			{
				if (index < 0)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("index", index, "Can't be less than zero.");
				}
				if (arrayIndex < 0)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("arrayIndex", arrayIndex, "Can't be less than zero.");
				}
				if (count < 0)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("index", index, "Can't be less than zero.");
				}
				if (index >= this.m_Adaptee.Count)
				{
					throw new ArgumentException("Can't be more or equal to list count.", "index");
				}
				if (array.Rank > 1)
				{
					throw new ArgumentException("Can't copy into multi-dimensional array.");
				}
				if (arrayIndex >= array.Length)
				{
					throw new ArgumentException("arrayIndex can't be greater than array.Length - 1.");
				}
				if (array.Length - arrayIndex + 1 < count)
				{
					throw new ArgumentException("Destination array is too small.");
				}
				if (index > this.m_Adaptee.Count - count)
				{
					throw new ArgumentException("Index and count do not denote a valid range of elements.", "index");
				}
				for (int i = 0; i < count; i++)
				{
					array.SetValue(this.m_Adaptee[index + i], arrayIndex + i);
				}
			}

			public override bool IsSynchronized
			{
				get
				{
					return this.m_Adaptee.IsSynchronized;
				}
			}

			public override IEnumerator GetEnumerator()
			{
				return this.m_Adaptee.GetEnumerator();
			}

			public override IEnumerator GetEnumerator(int index, int count)
			{
				ArrayList.CheckRange(index, count, this.m_Adaptee.Count);
				return new ArrayList.ArrayListAdapter.EnumeratorWithRange(this.m_Adaptee.GetEnumerator(), index, count);
			}

			public override void AddRange(ICollection c)
			{
				foreach (object value in c)
				{
					this.m_Adaptee.Add(value);
				}
			}

			public override int BinarySearch(object value)
			{
				return this.BinarySearch(value, null);
			}

			public override int BinarySearch(object value, IComparer comparer)
			{
				return this.BinarySearch(0, this.m_Adaptee.Count, value, comparer);
			}

			public override int BinarySearch(int index, int count, object value, IComparer comparer)
			{
				ArrayList.CheckRange(index, count, this.m_Adaptee.Count);
				if (comparer == null)
				{
					comparer = Comparer.Default;
				}
				int i = index;
				int num = index + count - 1;
				while (i <= num)
				{
					int num2 = i + (num - i) / 2;
					int num3 = comparer.Compare(value, this.m_Adaptee[num2]);
					if (num3 < 0)
					{
						num = num2 - 1;
					}
					else
					{
						if (num3 <= 0)
						{
							return num2;
						}
						i = num2 + 1;
					}
				}
				return ~i;
			}

			public override object Clone()
			{
				return new ArrayList.ArrayListAdapter(this.m_Adaptee);
			}

			public override ArrayList GetRange(int index, int count)
			{
				ArrayList.CheckRange(index, count, this.m_Adaptee.Count);
				return new ArrayList.RangedArrayList(this, index, count);
			}

			public override void TrimToSize()
			{
			}

			public override void Sort()
			{
				this.Sort(Comparer.Default);
			}

			public override void Sort(IComparer comparer)
			{
				this.Sort(0, this.m_Adaptee.Count, comparer);
			}

			public override void Sort(int index, int count, IComparer comparer)
			{
				ArrayList.CheckRange(index, count, this.m_Adaptee.Count);
				if (comparer == null)
				{
					comparer = Comparer.Default;
				}
				ArrayList.ArrayListAdapter.QuickSort(this.m_Adaptee, index, index + count - 1, comparer);
			}

			private static void Swap(IList list, int x, int y)
			{
				object value = list[x];
				list[x] = list[y];
				list[y] = value;
			}

			internal static void QuickSort(IList list, int left, int right, IComparer comparer)
			{
				if (left >= right)
				{
					return;
				}
				int num = left + (right - left) / 2;
				if (comparer.Compare(list[num], list[left]) < 0)
				{
					ArrayList.ArrayListAdapter.Swap(list, num, left);
				}
				if (comparer.Compare(list[right], list[left]) < 0)
				{
					ArrayList.ArrayListAdapter.Swap(list, right, left);
				}
				if (comparer.Compare(list[right], list[num]) < 0)
				{
					ArrayList.ArrayListAdapter.Swap(list, right, num);
				}
				if (right - left + 1 <= 3)
				{
					return;
				}
				ArrayList.ArrayListAdapter.Swap(list, right - 1, num);
				object y = list[right - 1];
				int num2 = left;
				int num3 = right - 1;
				for (;;)
				{
					while (comparer.Compare(list[++num2], y) < 0)
					{
					}
					while (comparer.Compare(list[--num3], y) > 0)
					{
					}
					if (num2 >= num3)
					{
						break;
					}
					ArrayList.ArrayListAdapter.Swap(list, num2, num3);
				}
				ArrayList.ArrayListAdapter.Swap(list, right - 1, num2);
				ArrayList.ArrayListAdapter.QuickSort(list, left, num2 - 1, comparer);
				ArrayList.ArrayListAdapter.QuickSort(list, num2 + 1, right, comparer);
			}

			public override object[] ToArray()
			{
				object[] array = new object[this.m_Adaptee.Count];
				this.m_Adaptee.CopyTo(array, 0);
				return array;
			}

			public override Array ToArray(Type elementType)
			{
				Array array = Array.CreateInstance(elementType, this.m_Adaptee.Count);
				this.m_Adaptee.CopyTo(array, 0);
				return array;
			}

			private sealed class EnumeratorWithRange : IEnumerator, ICloneable
			{
				private int m_StartIndex;

				private int m_Count;

				private int m_MaxCount;

				private IEnumerator m_Enumerator;

				public EnumeratorWithRange(IEnumerator enumerator, int index, int count)
				{
					this.m_Count = 0;
					this.m_StartIndex = index;
					this.m_MaxCount = count;
					this.m_Enumerator = enumerator;
					this.Reset();
				}

				public object Clone()
				{
					return base.MemberwiseClone();
				}

				public object Current
				{
					get
					{
						return this.m_Enumerator.Current;
					}
				}

				public bool MoveNext()
				{
					if (this.m_Count >= this.m_MaxCount)
					{
						return false;
					}
					this.m_Count++;
					return this.m_Enumerator.MoveNext();
				}

				public void Reset()
				{
					this.m_Count = 0;
					this.m_Enumerator.Reset();
					for (int i = 0; i < this.m_StartIndex; i++)
					{
						this.m_Enumerator.MoveNext();
					}
				}
			}
		}

		[Serializable]
		private class ArrayListWrapper : ArrayList
		{
			protected ArrayList m_InnerArrayList;

			public ArrayListWrapper(ArrayList innerArrayList)
			{
				this.m_InnerArrayList = innerArrayList;
			}

			public override object this[int index]
			{
				get
				{
					return this.m_InnerArrayList[index];
				}
				set
				{
					this.m_InnerArrayList[index] = value;
				}
			}

			public override int Count
			{
				get
				{
					return this.m_InnerArrayList.Count;
				}
			}

			public override int Capacity
			{
				get
				{
					return this.m_InnerArrayList.Capacity;
				}
				set
				{
					this.m_InnerArrayList.Capacity = value;
				}
			}

			public override bool IsFixedSize
			{
				get
				{
					return this.m_InnerArrayList.IsFixedSize;
				}
			}

			public override bool IsReadOnly
			{
				get
				{
					return this.m_InnerArrayList.IsReadOnly;
				}
			}

			public override bool IsSynchronized
			{
				get
				{
					return this.m_InnerArrayList.IsSynchronized;
				}
			}

			public override object SyncRoot
			{
				get
				{
					return this.m_InnerArrayList.SyncRoot;
				}
			}

			public override int Add(object value)
			{
				return this.m_InnerArrayList.Add(value);
			}

			public override void Clear()
			{
				this.m_InnerArrayList.Clear();
			}

			public override bool Contains(object value)
			{
				return this.m_InnerArrayList.Contains(value);
			}

			public override int IndexOf(object value)
			{
				return this.m_InnerArrayList.IndexOf(value);
			}

			public override int IndexOf(object value, int startIndex)
			{
				return this.m_InnerArrayList.IndexOf(value, startIndex);
			}

			public override int IndexOf(object value, int startIndex, int count)
			{
				return this.m_InnerArrayList.IndexOf(value, startIndex, count);
			}

			public override int LastIndexOf(object value)
			{
				return this.m_InnerArrayList.LastIndexOf(value);
			}

			public override int LastIndexOf(object value, int startIndex)
			{
				return this.m_InnerArrayList.LastIndexOf(value, startIndex);
			}

			public override int LastIndexOf(object value, int startIndex, int count)
			{
				return this.m_InnerArrayList.LastIndexOf(value, startIndex, count);
			}

			public override void Insert(int index, object value)
			{
				this.m_InnerArrayList.Insert(index, value);
			}

			public override void InsertRange(int index, ICollection c)
			{
				this.m_InnerArrayList.InsertRange(index, c);
			}

			public override void Remove(object value)
			{
				this.m_InnerArrayList.Remove(value);
			}

			public override void RemoveAt(int index)
			{
				this.m_InnerArrayList.RemoveAt(index);
			}

			public override void RemoveRange(int index, int count)
			{
				this.m_InnerArrayList.RemoveRange(index, count);
			}

			public override void Reverse()
			{
				this.m_InnerArrayList.Reverse();
			}

			public override void Reverse(int index, int count)
			{
				this.m_InnerArrayList.Reverse(index, count);
			}

			public override void SetRange(int index, ICollection c)
			{
				this.m_InnerArrayList.SetRange(index, c);
			}

			public override void CopyTo(Array array)
			{
				this.m_InnerArrayList.CopyTo(array);
			}

			public override void CopyTo(Array array, int index)
			{
				this.m_InnerArrayList.CopyTo(array, index);
			}

			public override void CopyTo(int index, Array array, int arrayIndex, int count)
			{
				this.m_InnerArrayList.CopyTo(index, array, arrayIndex, count);
			}

			public override IEnumerator GetEnumerator()
			{
				return this.m_InnerArrayList.GetEnumerator();
			}

			public override IEnumerator GetEnumerator(int index, int count)
			{
				return this.m_InnerArrayList.GetEnumerator(index, count);
			}

			public override void AddRange(ICollection c)
			{
				this.m_InnerArrayList.AddRange(c);
			}

			public override int BinarySearch(object value)
			{
				return this.m_InnerArrayList.BinarySearch(value);
			}

			public override int BinarySearch(object value, IComparer comparer)
			{
				return this.m_InnerArrayList.BinarySearch(value, comparer);
			}

			public override int BinarySearch(int index, int count, object value, IComparer comparer)
			{
				return this.m_InnerArrayList.BinarySearch(index, count, value, comparer);
			}

			public override object Clone()
			{
				return this.m_InnerArrayList.Clone();
			}

			public override ArrayList GetRange(int index, int count)
			{
				return this.m_InnerArrayList.GetRange(index, count);
			}

			public override void TrimToSize()
			{
				this.m_InnerArrayList.TrimToSize();
			}

			public override void Sort()
			{
				this.m_InnerArrayList.Sort();
			}

			public override void Sort(IComparer comparer)
			{
				this.m_InnerArrayList.Sort(comparer);
			}

			public override void Sort(int index, int count, IComparer comparer)
			{
				this.m_InnerArrayList.Sort(index, count, comparer);
			}

			public override object[] ToArray()
			{
				return this.m_InnerArrayList.ToArray();
			}

			public override Array ToArray(Type elementType)
			{
				return this.m_InnerArrayList.ToArray(elementType);
			}
		}

		[Serializable]
		private sealed class SynchronizedArrayListWrapper : ArrayList.ArrayListWrapper
		{
			private object m_SyncRoot;

			internal SynchronizedArrayListWrapper(ArrayList innerArrayList) : base(innerArrayList)
			{
				this.m_SyncRoot = innerArrayList.SyncRoot;
			}

			public override object this[int index]
			{
				get
				{
					object syncRoot = this.m_SyncRoot;
					object result;
					lock (syncRoot)
					{
						result = this.m_InnerArrayList[index];
					}
					return result;
				}
				set
				{
					object syncRoot = this.m_SyncRoot;
					lock (syncRoot)
					{
						this.m_InnerArrayList[index] = value;
					}
				}
			}

			public override int Count
			{
				get
				{
					object syncRoot = this.m_SyncRoot;
					int count;
					lock (syncRoot)
					{
						count = this.m_InnerArrayList.Count;
					}
					return count;
				}
			}

			public override int Capacity
			{
				get
				{
					object syncRoot = this.m_SyncRoot;
					int capacity;
					lock (syncRoot)
					{
						capacity = this.m_InnerArrayList.Capacity;
					}
					return capacity;
				}
				set
				{
					object syncRoot = this.m_SyncRoot;
					lock (syncRoot)
					{
						this.m_InnerArrayList.Capacity = value;
					}
				}
			}

			public override bool IsFixedSize
			{
				get
				{
					object syncRoot = this.m_SyncRoot;
					bool isFixedSize;
					lock (syncRoot)
					{
						isFixedSize = this.m_InnerArrayList.IsFixedSize;
					}
					return isFixedSize;
				}
			}

			public override bool IsReadOnly
			{
				get
				{
					object syncRoot = this.m_SyncRoot;
					bool isReadOnly;
					lock (syncRoot)
					{
						isReadOnly = this.m_InnerArrayList.IsReadOnly;
					}
					return isReadOnly;
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
					return this.m_SyncRoot;
				}
			}

			public override int Add(object value)
			{
				object syncRoot = this.m_SyncRoot;
				int result;
				lock (syncRoot)
				{
					result = this.m_InnerArrayList.Add(value);
				}
				return result;
			}

			public override void Clear()
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.Clear();
				}
			}

			public override bool Contains(object value)
			{
				object syncRoot = this.m_SyncRoot;
				bool result;
				lock (syncRoot)
				{
					result = this.m_InnerArrayList.Contains(value);
				}
				return result;
			}

			public override int IndexOf(object value)
			{
				object syncRoot = this.m_SyncRoot;
				int result;
				lock (syncRoot)
				{
					result = this.m_InnerArrayList.IndexOf(value);
				}
				return result;
			}

			public override int IndexOf(object value, int startIndex)
			{
				object syncRoot = this.m_SyncRoot;
				int result;
				lock (syncRoot)
				{
					result = this.m_InnerArrayList.IndexOf(value, startIndex);
				}
				return result;
			}

			public override int IndexOf(object value, int startIndex, int count)
			{
				object syncRoot = this.m_SyncRoot;
				int result;
				lock (syncRoot)
				{
					result = this.m_InnerArrayList.IndexOf(value, startIndex, count);
				}
				return result;
			}

			public override int LastIndexOf(object value)
			{
				object syncRoot = this.m_SyncRoot;
				int result;
				lock (syncRoot)
				{
					result = this.m_InnerArrayList.LastIndexOf(value);
				}
				return result;
			}

			public override int LastIndexOf(object value, int startIndex)
			{
				object syncRoot = this.m_SyncRoot;
				int result;
				lock (syncRoot)
				{
					result = this.m_InnerArrayList.LastIndexOf(value, startIndex);
				}
				return result;
			}

			public override int LastIndexOf(object value, int startIndex, int count)
			{
				object syncRoot = this.m_SyncRoot;
				int result;
				lock (syncRoot)
				{
					result = this.m_InnerArrayList.LastIndexOf(value, startIndex, count);
				}
				return result;
			}

			public override void Insert(int index, object value)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.Insert(index, value);
				}
			}

			public override void InsertRange(int index, ICollection c)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.InsertRange(index, c);
				}
			}

			public override void Remove(object value)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.Remove(value);
				}
			}

			public override void RemoveAt(int index)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.RemoveAt(index);
				}
			}

			public override void RemoveRange(int index, int count)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.RemoveRange(index, count);
				}
			}

			public override void Reverse()
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.Reverse();
				}
			}

			public override void Reverse(int index, int count)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.Reverse(index, count);
				}
			}

			public override void CopyTo(Array array)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.CopyTo(array);
				}
			}

			public override void CopyTo(Array array, int index)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.CopyTo(array, index);
				}
			}

			public override void CopyTo(int index, Array array, int arrayIndex, int count)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.CopyTo(index, array, arrayIndex, count);
				}
			}

			public override IEnumerator GetEnumerator()
			{
				object syncRoot = this.m_SyncRoot;
				IEnumerator enumerator;
				lock (syncRoot)
				{
					enumerator = this.m_InnerArrayList.GetEnumerator();
				}
				return enumerator;
			}

			public override IEnumerator GetEnumerator(int index, int count)
			{
				object syncRoot = this.m_SyncRoot;
				IEnumerator enumerator;
				lock (syncRoot)
				{
					enumerator = this.m_InnerArrayList.GetEnumerator(index, count);
				}
				return enumerator;
			}

			public override void AddRange(ICollection c)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.AddRange(c);
				}
			}

			public override int BinarySearch(object value)
			{
				object syncRoot = this.m_SyncRoot;
				int result;
				lock (syncRoot)
				{
					result = this.m_InnerArrayList.BinarySearch(value);
				}
				return result;
			}

			public override int BinarySearch(object value, IComparer comparer)
			{
				object syncRoot = this.m_SyncRoot;
				int result;
				lock (syncRoot)
				{
					result = this.m_InnerArrayList.BinarySearch(value, comparer);
				}
				return result;
			}

			public override int BinarySearch(int index, int count, object value, IComparer comparer)
			{
				object syncRoot = this.m_SyncRoot;
				int result;
				lock (syncRoot)
				{
					result = this.m_InnerArrayList.BinarySearch(index, count, value, comparer);
				}
				return result;
			}

			public override object Clone()
			{
				object syncRoot = this.m_SyncRoot;
				object result;
				lock (syncRoot)
				{
					result = this.m_InnerArrayList.Clone();
				}
				return result;
			}

			public override ArrayList GetRange(int index, int count)
			{
				object syncRoot = this.m_SyncRoot;
				ArrayList range;
				lock (syncRoot)
				{
					range = this.m_InnerArrayList.GetRange(index, count);
				}
				return range;
			}

			public override void TrimToSize()
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.TrimToSize();
				}
			}

			public override void Sort()
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.Sort();
				}
			}

			public override void Sort(IComparer comparer)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.Sort(comparer);
				}
			}

			public override void Sort(int index, int count, IComparer comparer)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerArrayList.Sort(index, count, comparer);
				}
			}

			public override object[] ToArray()
			{
				object syncRoot = this.m_SyncRoot;
				object[] result;
				lock (syncRoot)
				{
					result = this.m_InnerArrayList.ToArray();
				}
				return result;
			}

			public override Array ToArray(Type elementType)
			{
				object syncRoot = this.m_SyncRoot;
				Array result;
				lock (syncRoot)
				{
					result = this.m_InnerArrayList.ToArray(elementType);
				}
				return result;
			}
		}

		[Serializable]
		private class FixedSizeArrayListWrapper : ArrayList.ArrayListWrapper
		{
			public FixedSizeArrayListWrapper(ArrayList innerList) : base(innerList)
			{
			}

			protected virtual string ErrorMessage
			{
				get
				{
					return "Can't add or remove from a fixed-size list.";
				}
			}

			public override int Capacity
			{
				get
				{
					return base.Capacity;
				}
				set
				{
					throw new NotSupportedException(this.ErrorMessage);
				}
			}

			public override bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			public override int Add(object value)
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void AddRange(ICollection c)
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void Clear()
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void Insert(int index, object value)
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void InsertRange(int index, ICollection c)
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void Remove(object value)
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void RemoveAt(int index)
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void RemoveRange(int index, int count)
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void TrimToSize()
			{
				throw new NotSupportedException(this.ErrorMessage);
			}
		}

		[Serializable]
		private sealed class ReadOnlyArrayListWrapper : ArrayList.FixedSizeArrayListWrapper
		{
			public ReadOnlyArrayListWrapper(ArrayList innerArrayList) : base(innerArrayList)
			{
			}

			protected override string ErrorMessage
			{
				get
				{
					return "Can't modify a readonly list.";
				}
			}

			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			public override object this[int index]
			{
				get
				{
					return this.m_InnerArrayList[index];
				}
				set
				{
					throw new NotSupportedException(this.ErrorMessage);
				}
			}

			public override void Reverse()
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void Reverse(int index, int count)
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void SetRange(int index, ICollection c)
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void Sort()
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void Sort(IComparer comparer)
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void Sort(int index, int count, IComparer comparer)
			{
				throw new NotSupportedException(this.ErrorMessage);
			}
		}

		[Serializable]
		private sealed class RangedArrayList : ArrayList.ArrayListWrapper
		{
			private int m_InnerIndex;

			private int m_InnerCount;

			private int m_InnerStateChanges;

			public RangedArrayList(ArrayList innerList, int index, int count) : base(innerList)
			{
				this.m_InnerIndex = index;
				this.m_InnerCount = count;
				this.m_InnerStateChanges = innerList._version;
			}

			public override bool IsSynchronized
			{
				get
				{
					return false;
				}
			}

			public override object this[int index]
			{
				get
				{
					if (index < 0 || index > this.m_InnerCount)
					{
						throw new ArgumentOutOfRangeException("index");
					}
					return this.m_InnerArrayList[this.m_InnerIndex + index];
				}
				set
				{
					if (index < 0 || index > this.m_InnerCount)
					{
						throw new ArgumentOutOfRangeException("index");
					}
					this.m_InnerArrayList[this.m_InnerIndex + index] = value;
				}
			}

			public override int Count
			{
				get
				{
					this.VerifyStateChanges();
					return this.m_InnerCount;
				}
			}

			public override int Capacity
			{
				get
				{
					return this.m_InnerArrayList.Capacity;
				}
				set
				{
					if (value < this.m_InnerCount)
					{
						throw new ArgumentOutOfRangeException();
					}
				}
			}

			private void VerifyStateChanges()
			{
				if (this.m_InnerStateChanges != this.m_InnerArrayList._version)
				{
					throw new InvalidOperationException("ArrayList view is invalid because the underlying ArrayList was modified.");
				}
			}

			public override int Add(object value)
			{
				this.VerifyStateChanges();
				this.m_InnerArrayList.Insert(this.m_InnerIndex + this.m_InnerCount, value);
				this.m_InnerStateChanges = this.m_InnerArrayList._version;
				return ++this.m_InnerCount;
			}

			public override void Clear()
			{
				this.VerifyStateChanges();
				this.m_InnerArrayList.RemoveRange(this.m_InnerIndex, this.m_InnerCount);
				this.m_InnerCount = 0;
				this.m_InnerStateChanges = this.m_InnerArrayList._version;
			}

			public override bool Contains(object value)
			{
				return this.m_InnerArrayList.Contains(value, this.m_InnerIndex, this.m_InnerCount);
			}

			public override int IndexOf(object value)
			{
				return this.IndexOf(value, 0);
			}

			public override int IndexOf(object value, int startIndex)
			{
				return this.IndexOf(value, startIndex, this.m_InnerCount - startIndex);
			}

			public override int IndexOf(object value, int startIndex, int count)
			{
				if (startIndex < 0 || startIndex > this.m_InnerCount)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("startIndex", startIndex, "Does not specify valid index.");
				}
				if (count < 0)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("count", count, "Can't be less than 0.");
				}
				if (startIndex > this.m_InnerCount - count)
				{
					throw new ArgumentOutOfRangeException("count", "Start index and count do not specify a valid range.");
				}
				int num = this.m_InnerArrayList.IndexOf(value, this.m_InnerIndex + startIndex, count);
				if (num == -1)
				{
					return -1;
				}
				return num - this.m_InnerIndex;
			}

			public override int LastIndexOf(object value)
			{
				return this.LastIndexOf(value, this.m_InnerCount - 1);
			}

			public override int LastIndexOf(object value, int startIndex)
			{
				return this.LastIndexOf(value, startIndex, startIndex + 1);
			}

			public override int LastIndexOf(object value, int startIndex, int count)
			{
				if (startIndex < 0)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("startIndex", startIndex, "< 0");
				}
				if (count < 0)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("count", count, "count is negative.");
				}
				int num = this.m_InnerArrayList.LastIndexOf(value, this.m_InnerIndex + startIndex, count);
				if (num == -1)
				{
					return -1;
				}
				return num - this.m_InnerIndex;
			}

			public override void Insert(int index, object value)
			{
				this.VerifyStateChanges();
				if (index < 0 || index > this.m_InnerCount)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("index", index, "Index must be >= 0 and <= Count.");
				}
				this.m_InnerArrayList.Insert(this.m_InnerIndex + index, value);
				this.m_InnerCount++;
				this.m_InnerStateChanges = this.m_InnerArrayList._version;
			}

			public override void InsertRange(int index, ICollection c)
			{
				this.VerifyStateChanges();
				if (index < 0 || index > this.m_InnerCount)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("index", index, "Index must be >= 0 and <= Count.");
				}
				this.m_InnerArrayList.InsertRange(this.m_InnerIndex + index, c);
				this.m_InnerCount += c.Count;
				this.m_InnerStateChanges = this.m_InnerArrayList._version;
			}

			public override void Remove(object value)
			{
				this.VerifyStateChanges();
				int num = this.IndexOf(value);
				if (num > -1)
				{
					this.RemoveAt(num);
				}
				this.m_InnerStateChanges = this.m_InnerArrayList._version;
			}

			public override void RemoveAt(int index)
			{
				this.VerifyStateChanges();
				if (index < 0 || index > this.m_InnerCount)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("index", index, "Index must be >= 0 and <= Count.");
				}
				this.m_InnerArrayList.RemoveAt(this.m_InnerIndex + index);
				this.m_InnerCount--;
				this.m_InnerStateChanges = this.m_InnerArrayList._version;
			}

			public override void RemoveRange(int index, int count)
			{
				this.VerifyStateChanges();
				ArrayList.CheckRange(index, count, this.m_InnerCount);
				this.m_InnerArrayList.RemoveRange(this.m_InnerIndex + index, count);
				this.m_InnerCount -= count;
				this.m_InnerStateChanges = this.m_InnerArrayList._version;
			}

			public override void Reverse()
			{
				this.Reverse(0, this.m_InnerCount);
			}

			public override void Reverse(int index, int count)
			{
				this.VerifyStateChanges();
				ArrayList.CheckRange(index, count, this.m_InnerCount);
				this.m_InnerArrayList.Reverse(this.m_InnerIndex + index, count);
				this.m_InnerStateChanges = this.m_InnerArrayList._version;
			}

			public override void SetRange(int index, ICollection c)
			{
				this.VerifyStateChanges();
				if (index < 0 || index > this.m_InnerCount)
				{
					ArrayList.ThrowNewArgumentOutOfRangeException("index", index, "Index must be >= 0 and <= Count.");
				}
				this.m_InnerArrayList.SetRange(this.m_InnerIndex + index, c);
				this.m_InnerStateChanges = this.m_InnerArrayList._version;
			}

			public override void CopyTo(Array array)
			{
				this.CopyTo(array, 0);
			}

			public override void CopyTo(Array array, int index)
			{
				this.CopyTo(0, array, index, this.m_InnerCount);
			}

			public override void CopyTo(int index, Array array, int arrayIndex, int count)
			{
				ArrayList.CheckRange(index, count, this.m_InnerCount);
				this.m_InnerArrayList.CopyTo(this.m_InnerIndex + index, array, arrayIndex, count);
			}

			public override IEnumerator GetEnumerator()
			{
				return this.GetEnumerator(0, this.m_InnerCount);
			}

			public override IEnumerator GetEnumerator(int index, int count)
			{
				ArrayList.CheckRange(index, count, this.m_InnerCount);
				return this.m_InnerArrayList.GetEnumerator(this.m_InnerIndex + index, count);
			}

			public override void AddRange(ICollection c)
			{
				this.VerifyStateChanges();
				this.m_InnerArrayList.InsertRange(this.m_InnerCount, c);
				this.m_InnerCount += c.Count;
				this.m_InnerStateChanges = this.m_InnerArrayList._version;
			}

			public override int BinarySearch(object value)
			{
				return this.BinarySearch(0, this.m_InnerCount, value, Comparer.Default);
			}

			public override int BinarySearch(object value, IComparer comparer)
			{
				return this.BinarySearch(0, this.m_InnerCount, value, comparer);
			}

			public override int BinarySearch(int index, int count, object value, IComparer comparer)
			{
				ArrayList.CheckRange(index, count, this.m_InnerCount);
				return this.m_InnerArrayList.BinarySearch(this.m_InnerIndex + index, count, value, comparer);
			}

			public override object Clone()
			{
				return new ArrayList.RangedArrayList((ArrayList)this.m_InnerArrayList.Clone(), this.m_InnerIndex, this.m_InnerCount);
			}

			public override ArrayList GetRange(int index, int count)
			{
				ArrayList.CheckRange(index, count, this.m_InnerCount);
				return new ArrayList.RangedArrayList(this, index, count);
			}

			public override void TrimToSize()
			{
				throw new NotSupportedException();
			}

			public override void Sort()
			{
				this.Sort(Comparer.Default);
			}

			public override void Sort(IComparer comparer)
			{
				this.Sort(0, this.m_InnerCount, comparer);
			}

			public override void Sort(int index, int count, IComparer comparer)
			{
				this.VerifyStateChanges();
				ArrayList.CheckRange(index, count, this.m_InnerCount);
				this.m_InnerArrayList.Sort(this.m_InnerIndex + index, count, comparer);
				this.m_InnerStateChanges = this.m_InnerArrayList._version;
			}

			public override object[] ToArray()
			{
				object[] array = new object[this.m_InnerCount];
				this.m_InnerArrayList.CopyTo(this.m_InnerIndex, array, 0, this.m_InnerCount);
				return array;
			}

			public override Array ToArray(Type elementType)
			{
				Array array = Array.CreateInstance(elementType, this.m_InnerCount);
				this.m_InnerArrayList.CopyTo(this.m_InnerIndex, array, 0, this.m_InnerCount);
				return array;
			}
		}

		[Serializable]
		private sealed class SynchronizedListWrapper : ArrayList.ListWrapper
		{
			private object m_SyncRoot;

			public SynchronizedListWrapper(IList innerList) : base(innerList)
			{
				this.m_SyncRoot = innerList.SyncRoot;
			}

			public override int Count
			{
				get
				{
					object syncRoot = this.m_SyncRoot;
					int count;
					lock (syncRoot)
					{
						count = this.m_InnerList.Count;
					}
					return count;
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
					object syncRoot = this.m_SyncRoot;
					object syncRoot2;
					lock (syncRoot)
					{
						syncRoot2 = this.m_InnerList.SyncRoot;
					}
					return syncRoot2;
				}
			}

			public override bool IsFixedSize
			{
				get
				{
					object syncRoot = this.m_SyncRoot;
					bool isFixedSize;
					lock (syncRoot)
					{
						isFixedSize = this.m_InnerList.IsFixedSize;
					}
					return isFixedSize;
				}
			}

			public override bool IsReadOnly
			{
				get
				{
					object syncRoot = this.m_SyncRoot;
					bool isReadOnly;
					lock (syncRoot)
					{
						isReadOnly = this.m_InnerList.IsReadOnly;
					}
					return isReadOnly;
				}
			}

			public override object this[int index]
			{
				get
				{
					object syncRoot = this.m_SyncRoot;
					object result;
					lock (syncRoot)
					{
						result = this.m_InnerList[index];
					}
					return result;
				}
				set
				{
					object syncRoot = this.m_SyncRoot;
					lock (syncRoot)
					{
						this.m_InnerList[index] = value;
					}
				}
			}

			public override int Add(object value)
			{
				object syncRoot = this.m_SyncRoot;
				int result;
				lock (syncRoot)
				{
					result = this.m_InnerList.Add(value);
				}
				return result;
			}

			public override void Clear()
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerList.Clear();
				}
			}

			public override bool Contains(object value)
			{
				object syncRoot = this.m_SyncRoot;
				bool result;
				lock (syncRoot)
				{
					result = this.m_InnerList.Contains(value);
				}
				return result;
			}

			public override int IndexOf(object value)
			{
				object syncRoot = this.m_SyncRoot;
				int result;
				lock (syncRoot)
				{
					result = this.m_InnerList.IndexOf(value);
				}
				return result;
			}

			public override void Insert(int index, object value)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerList.Insert(index, value);
				}
			}

			public override void Remove(object value)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerList.Remove(value);
				}
			}

			public override void RemoveAt(int index)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerList.RemoveAt(index);
				}
			}

			public override void CopyTo(Array array, int index)
			{
				object syncRoot = this.m_SyncRoot;
				lock (syncRoot)
				{
					this.m_InnerList.CopyTo(array, index);
				}
			}

			public override IEnumerator GetEnumerator()
			{
				object syncRoot = this.m_SyncRoot;
				IEnumerator enumerator;
				lock (syncRoot)
				{
					enumerator = this.m_InnerList.GetEnumerator();
				}
				return enumerator;
			}
		}

		[Serializable]
		private class FixedSizeListWrapper : ArrayList.ListWrapper
		{
			public FixedSizeListWrapper(IList innerList) : base(innerList)
			{
			}

			protected virtual string ErrorMessage
			{
				get
				{
					return "List is fixed-size.";
				}
			}

			public override bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			public override int Add(object value)
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void Clear()
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void Insert(int index, object value)
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void Remove(object value)
			{
				throw new NotSupportedException(this.ErrorMessage);
			}

			public override void RemoveAt(int index)
			{
				throw new NotSupportedException(this.ErrorMessage);
			}
		}

		[Serializable]
		private sealed class ReadOnlyListWrapper : ArrayList.FixedSizeListWrapper
		{
			public ReadOnlyListWrapper(IList innerList) : base(innerList)
			{
			}

			protected override string ErrorMessage
			{
				get
				{
					return "List is read-only.";
				}
			}

			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			public override object this[int index]
			{
				get
				{
					return this.m_InnerList[index];
				}
				set
				{
					throw new NotSupportedException(this.ErrorMessage);
				}
			}
		}

		[Serializable]
		private class ListWrapper : IEnumerable, ICollection, IList
		{
			protected IList m_InnerList;

			public ListWrapper(IList innerList)
			{
				this.m_InnerList = innerList;
			}

			public virtual object this[int index]
			{
				get
				{
					return this.m_InnerList[index];
				}
				set
				{
					this.m_InnerList[index] = value;
				}
			}

			public virtual int Count
			{
				get
				{
					return this.m_InnerList.Count;
				}
			}

			public virtual bool IsSynchronized
			{
				get
				{
					return this.m_InnerList.IsSynchronized;
				}
			}

			public virtual object SyncRoot
			{
				get
				{
					return this.m_InnerList.SyncRoot;
				}
			}

			public virtual bool IsFixedSize
			{
				get
				{
					return this.m_InnerList.IsFixedSize;
				}
			}

			public virtual bool IsReadOnly
			{
				get
				{
					return this.m_InnerList.IsReadOnly;
				}
			}

			public virtual int Add(object value)
			{
				return this.m_InnerList.Add(value);
			}

			public virtual void Clear()
			{
				this.m_InnerList.Clear();
			}

			public virtual bool Contains(object value)
			{
				return this.m_InnerList.Contains(value);
			}

			public virtual int IndexOf(object value)
			{
				return this.m_InnerList.IndexOf(value);
			}

			public virtual void Insert(int index, object value)
			{
				this.m_InnerList.Insert(index, value);
			}

			public virtual void Remove(object value)
			{
				this.m_InnerList.Remove(value);
			}

			public virtual void RemoveAt(int index)
			{
				this.m_InnerList.RemoveAt(index);
			}

			public virtual void CopyTo(Array array, int index)
			{
				this.m_InnerList.CopyTo(array, index);
			}

			public virtual IEnumerator GetEnumerator()
			{
				return this.m_InnerList.GetEnumerator();
			}
		}
	}
}
