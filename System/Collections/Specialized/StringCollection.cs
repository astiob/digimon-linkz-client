using System;

namespace System.Collections.Specialized
{
	/// <summary>Represents a collection of strings.</summary>
	[Serializable]
	public class StringCollection : ICollection, IEnumerable, IList
	{
		private ArrayList data = new ArrayList();

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.StringCollection" /> object is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Specialized.StringCollection" /> object is read-only; otherwise, false. The default is false.</returns>
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.StringCollection" /> object has a fixed size.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Specialized.StringCollection" /> object has a fixed size; otherwise, false. The default is false.</returns>
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <returns>The element at the specified index.</returns>
		/// <param name="index">The zero-based index of the element to get or set. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.Specialized.StringCollection.Count" />. </exception>
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (string)value;
			}
		}

		/// <summary>Adds an object to the end of the <see cref="T:System.Collections.Specialized.StringCollection" />.</summary>
		/// <returns>The <see cref="T:System.Collections.Specialized.StringCollection" /> index at which the <paramref name="value" /> has been added.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to be added to the end of the <see cref="T:System.Collections.Specialized.StringCollection" />. The value can be null. </param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Specialized.StringCollection" /> is read-only.-or- The <see cref="T:System.Collections.Specialized.StringCollection" /> has a fixed size. </exception>
		int IList.Add(object value)
		{
			return this.Add((string)value);
		}

		/// <summary>Determines whether an element is in the <see cref="T:System.Collections.Specialized.StringCollection" />.</summary>
		/// <returns>true if <paramref name="value" /> is found in the <see cref="T:System.Collections.Specialized.StringCollection" />; otherwise, false.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.Specialized.StringCollection" />. The value can be null. </param>
		bool IList.Contains(object value)
		{
			return this.Contains((string)value);
		}

		/// <summary>Searches for the specified <see cref="T:System.Object" /> and returns the zero-based index of the first occurrence within the entire <see cref="T:System.Collections.Specialized.StringCollection" />.</summary>
		/// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the entire <see cref="T:System.Collections.Specialized.StringCollection" />, if found; otherwise, -1.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.Specialized.StringCollection" />. The value can be null. </param>
		int IList.IndexOf(object value)
		{
			return this.IndexOf((string)value);
		}

		/// <summary>Inserts an element into the <see cref="T:System.Collections.Specialized.StringCollection" /> at the specified index.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to insert. The value can be null. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is greater than <see cref="P:System.Collections.Specialized.StringCollection.Count" />. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Specialized.StringCollection" /> is read-only.-or- The <see cref="T:System.Collections.Specialized.StringCollection" /> has a fixed size. </exception>
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (string)value);
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Specialized.StringCollection" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Collections.Specialized.StringCollection" />. The value can be null. </param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Specialized.StringCollection" /> is read-only.-or- The <see cref="T:System.Collections.Specialized.StringCollection" /> has a fixed size. </exception>
		void IList.Remove(object value)
		{
			this.Remove((string)value);
		}

		/// <summary>Copies the entire <see cref="T:System.Collections.Specialized.StringCollection" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Specialized.StringCollection" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.Specialized.StringCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.Specialized.StringCollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		void ICollection.CopyTo(Array array, int index)
		{
			this.data.CopyTo(array, index);
		}

		/// <summary>Returns a <see cref="T:System.Collections.IEnumerator" /> that iterates through the <see cref="T:System.Collections.Specialized.StringCollection" />.</summary>
		/// <returns>A <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.Specialized.StringCollection" />.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.data.GetEnumerator();
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <returns>The element at the specified index.</returns>
		/// <param name="index">The zero-based index of the entry to get or set. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.Specialized.StringCollection.Count" />. </exception>
		public string this[int index]
		{
			get
			{
				return (string)this.data[index];
			}
			set
			{
				this.data[index] = value;
			}
		}

		/// <summary>Gets the number of strings contained in the <see cref="T:System.Collections.Specialized.StringCollection" />.</summary>
		/// <returns>The number of strings contained in the <see cref="T:System.Collections.Specialized.StringCollection" />.</returns>
		public int Count
		{
			get
			{
				return this.data.Count;
			}
		}

		/// <summary>Adds a string to the end of the <see cref="T:System.Collections.Specialized.StringCollection" />.</summary>
		/// <returns>The zero-based index at which the new element is inserted.</returns>
		/// <param name="value">The string to add to the end of the <see cref="T:System.Collections.Specialized.StringCollection" />. The value can be null. </param>
		public int Add(string value)
		{
			return this.data.Add(value);
		}

		/// <summary>Copies the elements of a string array to the end of the <see cref="T:System.Collections.Specialized.StringCollection" />.</summary>
		/// <param name="value">An array of strings to add to the end of the <see cref="T:System.Collections.Specialized.StringCollection" />. The array itself can not be null but it can contain elements that are null. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		public void AddRange(string[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.data.AddRange(value);
		}

		/// <summary>Removes all the strings from the <see cref="T:System.Collections.Specialized.StringCollection" />.</summary>
		public void Clear()
		{
			this.data.Clear();
		}

		/// <summary>Determines whether the specified string is in the <see cref="T:System.Collections.Specialized.StringCollection" />.</summary>
		/// <returns>true if <paramref name="value" /> is found in the <see cref="T:System.Collections.Specialized.StringCollection" />; otherwise, false.</returns>
		/// <param name="value">The string to locate in the <see cref="T:System.Collections.Specialized.StringCollection" />. The value can be null. </param>
		public bool Contains(string value)
		{
			return this.data.Contains(value);
		}

		/// <summary>Copies the entire <see cref="T:System.Collections.Specialized.StringCollection" /> values to a one-dimensional array of strings, starting at the specified index of the target array.</summary>
		/// <param name="array">The one-dimensional array of strings that is the destination of the elements copied from <see cref="T:System.Collections.Specialized.StringCollection" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.Specialized.StringCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.Specialized.StringCollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		public void CopyTo(string[] array, int index)
		{
			this.data.CopyTo(array, index);
		}

		/// <summary>Returns a <see cref="T:System.Collections.Specialized.StringEnumerator" /> that iterates through the <see cref="T:System.Collections.Specialized.StringCollection" />.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.StringEnumerator" /> for the <see cref="T:System.Collections.Specialized.StringCollection" />.</returns>
		public StringEnumerator GetEnumerator()
		{
			return new StringEnumerator(this);
		}

		/// <summary>Searches for the specified string and returns the zero-based index of the first occurrence within the <see cref="T:System.Collections.Specialized.StringCollection" />.</summary>
		/// <returns>The zero-based index of the first occurrence of <paramref name="value" /> in the <see cref="T:System.Collections.Specialized.StringCollection" />, if found; otherwise, -1.</returns>
		/// <param name="value">The string to locate. The value can be null. </param>
		public int IndexOf(string value)
		{
			return this.data.IndexOf(value);
		}

		/// <summary>Inserts a string into the <see cref="T:System.Collections.Specialized.StringCollection" /> at the specified index.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> is inserted. </param>
		/// <param name="value">The string to insert. The value can be null. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> greater than <see cref="P:System.Collections.Specialized.StringCollection.Count" />. </exception>
		public void Insert(int index, string value)
		{
			this.data.Insert(index, value);
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.StringCollection" /> is read-only.</summary>
		/// <returns>This property always returns false.</returns>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.Specialized.StringCollection" /> is synchronized (thread safe).</summary>
		/// <returns>This property always returns false.</returns>
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Removes the first occurrence of a specific string from the <see cref="T:System.Collections.Specialized.StringCollection" />.</summary>
		/// <param name="value">The string to remove from the <see cref="T:System.Collections.Specialized.StringCollection" />. The value can be null. </param>
		public void Remove(string value)
		{
			this.data.Remove(value);
		}

		/// <summary>Removes the string at the specified index of the <see cref="T:System.Collections.Specialized.StringCollection" />.</summary>
		/// <param name="index">The zero-based index of the string to remove. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.Specialized.StringCollection.Count" />. </exception>
		public void RemoveAt(int index)
		{
			this.data.RemoveAt(index);
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.StringCollection" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.Specialized.StringCollection" />.</returns>
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}
	}
}
