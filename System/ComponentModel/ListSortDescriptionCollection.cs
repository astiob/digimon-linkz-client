using System;
using System.Collections;

namespace System.ComponentModel
{
	/// <summary>Represents a collection of <see cref="T:System.ComponentModel.ListSortDescription" /> objects.</summary>
	public class ListSortDescriptionCollection : ICollection, IEnumerable, IList
	{
		private ArrayList list;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ListSortDescriptionCollection" /> class. </summary>
		public ListSortDescriptionCollection()
		{
			this.list = new ArrayList();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.ListSortDescriptionCollection" /> class with the specified array of <see cref="T:System.ComponentModel.ListSortDescription" /> objects.</summary>
		/// <param name="sorts">The array of <see cref="T:System.ComponentModel.ListSortDescription" /> objects to be contained in the collection.</param>
		public ListSortDescriptionCollection(ListSortDescription[] sorts)
		{
			this.list = new ArrayList();
			foreach (ListSortDescription value in sorts)
			{
				this.list.Add(value);
			}
		}

		/// <summary>Gets the specified <see cref="T:System.ComponentModel.ListSortDescription" />.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ListSortDescription" /> with the specified index.</returns>
		/// <param name="index">The zero-based index of the <see cref="T:System.ComponentModel.ListSortDescription" />  to get in the collection </param>
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				throw new InvalidOperationException("ListSortDescriptorCollection is read only.");
			}
		}

		/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
		/// <returns>true in all cases.</returns>
		bool IList.IsFixedSize
		{
			get
			{
				return this.list.IsFixedSize;
			}
		}

		/// <summary>Gets a value indicating whether access to the collection is thread safe.</summary>
		/// <returns>true in all cases.</returns>
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.list.IsSynchronized;
			}
		}

		/// <summary>Gets the current instance that can be used to synchronize access to the collection.</summary>
		/// <returns>The current instance of the <see cref="T:System.ComponentModel.ListSortDescriptionCollection" />.</returns>
		object ICollection.SyncRoot
		{
			get
			{
				return this.list.SyncRoot;
			}
		}

		/// <summary>Gets a value indicating whether the collection is read-only.</summary>
		/// <returns>true in all cases.</returns>
		bool IList.IsReadOnly
		{
			get
			{
				return this.list.IsReadOnly;
			}
		}

		/// <summary>Gets a <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		/// <summary>Adds an item to the collection.</summary>
		/// <returns>The position into which the new element was inserted.</returns>
		/// <param name="value">The item to add to the collection.</param>
		/// <exception cref="T:System.InvalidOperationException">In all cases.</exception>
		int IList.Add(object value)
		{
			return this.list.Add(value);
		}

		/// <summary>Removes all items from the collection.</summary>
		/// <exception cref="T:System.InvalidOperationException">In all cases.</exception>
		void IList.Clear()
		{
			this.list.Clear();
		}

		/// <summary>Inserts an item into the collection at a specified index.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.ComponentModel.ListSortDescription" />  to get or set in the collection</param>
		/// <param name="value">The item to insert into the collection.</param>
		/// <exception cref="T:System.InvalidOperationException">In all cases.</exception>
		void IList.Insert(int index, object value)
		{
			this.list.Insert(index, value);
		}

		/// <summary>Removes the first occurrence of an item from the collection.</summary>
		/// <param name="value">The item to remove from the collection.</param>
		/// <exception cref="T:System.InvalidOperationException">In all cases.</exception>
		void IList.Remove(object value)
		{
			this.list.Remove(value);
		}

		/// <summary>Removes an item from the collection at a specified index.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.ComponentModel.ListSortDescription" />  to remove from the collection</param>
		/// <exception cref="T:System.InvalidOperationException">In all cases.</exception>
		void IList.RemoveAt(int index)
		{
			this.list.RemoveAt(index);
		}

		/// <summary>Gets the number of items in the collection.</summary>
		/// <returns>The number of items in the collection.</returns>
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		/// <summary>Gets or sets the specified <see cref="T:System.ComponentModel.ListSortDescription" />.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ListSortDescription" /> with the specified index.</returns>
		/// <param name="index">The zero-based index of the <see cref="T:System.ComponentModel.ListSortDescription" />  to get or set in the collection. </param>
		/// <exception cref="T:System.InvalidOperationException">An item is set in the <see cref="T:System.ComponentModel.ListSortDescriptionCollection" />, which is read-only.</exception>
		public ListSortDescription this[int index]
		{
			get
			{
				return this.list[index] as ListSortDescription;
			}
			set
			{
				throw new InvalidOperationException("ListSortDescriptorCollection is read only.");
			}
		}

		/// <summary>Determines if the <see cref="T:System.ComponentModel.ListSortDescriptionCollection" /> contains a specific value.</summary>
		/// <returns>true if the <see cref="T:System.Object" /> is found in the collection; otherwise, false. </returns>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the collection.</param>
		public bool Contains(object value)
		{
			return this.list.Contains(value);
		}

		/// <summary>Copies the contents of the collection to the specified array, starting at the specified destination array index.</summary>
		/// <param name="array">The destination array for the items copied from the collection.</param>
		/// <param name="index">The index of the destination array at which copying begins.</param>
		public void CopyTo(Array array, int index)
		{
			this.list.CopyTo(array, index);
		}

		/// <summary>Returns the index of the specified item in the collection.</summary>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the collection.</param>
		public int IndexOf(object value)
		{
			return this.list.IndexOf(value);
		}
	}
}
