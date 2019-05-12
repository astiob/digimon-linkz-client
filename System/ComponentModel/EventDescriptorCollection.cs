using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	/// <summary>Represents a collection of <see cref="T:System.ComponentModel.EventDescriptor" /> objects.</summary>
	[ComVisible(true)]
	public class EventDescriptorCollection : ICollection, IEnumerable, IList
	{
		private ArrayList eventList = new ArrayList();

		private bool isReadOnly;

		/// <summary>Specifies an empty collection to use, rather than creating a new one with no items. This static field is read-only.</summary>
		public static readonly EventDescriptorCollection Empty = new EventDescriptorCollection(null, true);

		private EventDescriptorCollection()
		{
		}

		internal EventDescriptorCollection(ArrayList list)
		{
			this.eventList = list;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.EventDescriptorCollection" /> class with the given array of <see cref="T:System.ComponentModel.EventDescriptor" /> objects.</summary>
		/// <param name="events">An array of type <see cref="T:System.ComponentModel.EventDescriptor" /> that provides the events for this collection. </param>
		public EventDescriptorCollection(EventDescriptor[] events) : this(events, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.EventDescriptorCollection" /> class with the given array of <see cref="T:System.ComponentModel.EventDescriptor" /> objects. The collection is optionally read-only.</summary>
		/// <param name="events">An array of type <see cref="T:System.ComponentModel.EventDescriptor" /> that provides the events for this collection. </param>
		/// <param name="readOnly">true to specify a read-only collection; otherwise, false.</param>
		public EventDescriptorCollection(EventDescriptor[] events, bool readOnly)
		{
			this.isReadOnly = readOnly;
			if (events == null)
			{
				return;
			}
			for (int i = 0; i < events.Length; i++)
			{
				this.Add(events[i]);
			}
		}

		/// <summary>Removes all the items from the collection.</summary>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		void IList.Clear()
		{
			this.Clear();
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>Removes the item at the specified index.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		/// <summary>Gets the number of elements contained in the collection.</summary>
		/// <returns>The number of elements contained in the collection.</returns>
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		/// <summary>Adds an item to the collection.</summary>
		/// <returns>The position into which the new element was inserted.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to add to the collection.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		int IList.Add(object value)
		{
			return this.Add((EventDescriptor)value);
		}

		/// <summary>Determines whether the collection contains a specific value.</summary>
		/// <returns>true if the <see cref="T:System.Object" /> is found in the collection; otherwise, false.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the collection.</param>
		bool IList.Contains(object value)
		{
			return this.Contains((EventDescriptor)value);
		}

		/// <summary>Determines the index of a specific item in the collection.</summary>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the collection.</param>
		int IList.IndexOf(object value)
		{
			return this.IndexOf((EventDescriptor)value);
		}

		/// <summary>Inserts an item to the collection at the specified index.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to insert into the collection.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (EventDescriptor)value);
		}

		/// <summary>Removes the first occurrence of a specific object from the collection.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to remove from the collection.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		void IList.Remove(object value)
		{
			this.Remove((EventDescriptor)value);
		}

		/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
		/// <returns>true if the collection has a fixed size; otherwise, false.</returns>
		bool IList.IsFixedSize
		{
			get
			{
				return this.isReadOnly;
			}
		}

		/// <summary>Gets a value indicating whether the collection is read-only.</summary>
		/// <returns>true if the collection is read-only; otherwise, false.</returns>
		bool IList.IsReadOnly
		{
			get
			{
				return this.isReadOnly;
			}
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <returns>The element at the specified index.</returns>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="index" /> is less than 0. -or-<paramref name="index" /> is equal to or greater than <see cref="P:System.ComponentModel.EventDescriptorCollection.Count" />.</exception>
		object IList.this[int index]
		{
			get
			{
				return this.eventList[index];
			}
			set
			{
				if (this.isReadOnly)
				{
					throw new NotSupportedException("The collection is read-only");
				}
				this.eventList[index] = value;
			}
		}

		/// <summary>Copies the elements of the collection to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from collection. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		void ICollection.CopyTo(Array array, int index)
		{
			this.eventList.CopyTo(array, index);
		}

		/// <summary>Gets a value indicating whether access to the collection is synchronized.</summary>
		/// <returns>true if access to the collection is synchronized; otherwise, false.</returns>
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the collection.</returns>
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		/// <summary>Adds an <see cref="T:System.ComponentModel.EventDescriptor" /> to the end of the collection.</summary>
		/// <returns>The position of the <see cref="T:System.ComponentModel.EventDescriptor" /> within the collection.</returns>
		/// <param name="value">An <see cref="T:System.ComponentModel.EventDescriptor" /> to add to the collection. </param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		public int Add(EventDescriptor value)
		{
			if (this.isReadOnly)
			{
				throw new NotSupportedException("The collection is read-only");
			}
			return this.eventList.Add(value);
		}

		/// <summary>Removes all objects from the collection.</summary>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		public void Clear()
		{
			if (this.isReadOnly)
			{
				throw new NotSupportedException("The collection is read-only");
			}
			this.eventList.Clear();
		}

		/// <summary>Returns whether the collection contains the given <see cref="T:System.ComponentModel.EventDescriptor" />.</summary>
		/// <returns>true if the collection contains the <paramref name="value" /> parameter given; otherwise, false.</returns>
		/// <param name="value">The <see cref="T:System.ComponentModel.EventDescriptor" /> to find within the collection. </param>
		public bool Contains(EventDescriptor value)
		{
			return this.eventList.Contains(value);
		}

		/// <summary>Gets the description of the event with the specified name in the collection.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.EventDescriptor" /> with the specified name, or null if the event does not exist.</returns>
		/// <param name="name">The name of the event to get from the collection. </param>
		/// <param name="ignoreCase">true if you want to ignore the case of the event; otherwise, false. </param>
		public virtual EventDescriptor Find(string name, bool ignoreCase)
		{
			foreach (object obj in this.eventList)
			{
				EventDescriptor eventDescriptor = (EventDescriptor)obj;
				if (ignoreCase)
				{
					if (string.Compare(name, eventDescriptor.Name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return eventDescriptor;
					}
				}
				else if (string.Compare(name, eventDescriptor.Name, StringComparison.Ordinal) == 0)
				{
					return eventDescriptor;
				}
			}
			return null;
		}

		/// <summary>Gets an enumerator for this <see cref="T:System.ComponentModel.EventDescriptorCollection" />.</summary>
		/// <returns>An enumerator that implements <see cref="T:System.Collections.IEnumerator" />.</returns>
		public IEnumerator GetEnumerator()
		{
			return this.eventList.GetEnumerator();
		}

		/// <summary>Returns the index of the given <see cref="T:System.ComponentModel.EventDescriptor" />.</summary>
		/// <returns>The index of the given <see cref="T:System.ComponentModel.EventDescriptor" /> within the collection.</returns>
		/// <param name="value">The <see cref="T:System.ComponentModel.EventDescriptor" /> to find within the collection. </param>
		public int IndexOf(EventDescriptor value)
		{
			return this.eventList.IndexOf(value);
		}

		/// <summary>Inserts an <see cref="T:System.ComponentModel.EventDescriptor" /> to the collection at a specified index.</summary>
		/// <param name="index">The index within the collection in which to insert the <paramref name="value" /> parameter. </param>
		/// <param name="value">An <see cref="T:System.ComponentModel.EventDescriptor" /> to insert into the collection. </param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		public void Insert(int index, EventDescriptor value)
		{
			if (this.isReadOnly)
			{
				throw new NotSupportedException("The collection is read-only");
			}
			this.eventList.Insert(index, value);
		}

		/// <summary>Removes the specified <see cref="T:System.ComponentModel.EventDescriptor" /> from the collection.</summary>
		/// <param name="value">The <see cref="T:System.ComponentModel.EventDescriptor" /> to remove from the collection. </param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		public void Remove(EventDescriptor value)
		{
			if (this.isReadOnly)
			{
				throw new NotSupportedException("The collection is read-only");
			}
			this.eventList.Remove(value);
		}

		/// <summary>Removes the <see cref="T:System.ComponentModel.EventDescriptor" /> at the specified index from the collection.</summary>
		/// <param name="index">The index of the <see cref="T:System.ComponentModel.EventDescriptor" /> to remove. </param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		public void RemoveAt(int index)
		{
			if (this.isReadOnly)
			{
				throw new NotSupportedException("The collection is read-only");
			}
			this.eventList.RemoveAt(index);
		}

		/// <summary>Sorts the members of this <see cref="T:System.ComponentModel.EventDescriptorCollection" />, using the default sort for this collection, which is usually alphabetical.</summary>
		/// <returns>The new <see cref="T:System.ComponentModel.EventDescriptorCollection" />.</returns>
		public virtual EventDescriptorCollection Sort()
		{
			EventDescriptorCollection eventDescriptorCollection = this.CloneCollection();
			eventDescriptorCollection.InternalSort(null);
			return eventDescriptorCollection;
		}

		/// <summary>Sorts the members of this <see cref="T:System.ComponentModel.EventDescriptorCollection" />, using the specified <see cref="T:System.Collections.IComparer" />.</summary>
		/// <returns>The new <see cref="T:System.ComponentModel.EventDescriptorCollection" />.</returns>
		/// <param name="comparer">An <see cref="T:System.Collections.IComparer" /> to use to sort the <see cref="T:System.ComponentModel.EventDescriptor" /> objects in this collection. </param>
		public virtual EventDescriptorCollection Sort(IComparer comparer)
		{
			EventDescriptorCollection eventDescriptorCollection = this.CloneCollection();
			eventDescriptorCollection.InternalSort(comparer);
			return eventDescriptorCollection;
		}

		/// <summary>Sorts the members of this <see cref="T:System.ComponentModel.EventDescriptorCollection" />, given a specified sort order.</summary>
		/// <returns>The new <see cref="T:System.ComponentModel.EventDescriptorCollection" />.</returns>
		/// <param name="names">An array of strings describing the order in which to sort the <see cref="T:System.ComponentModel.EventDescriptor" /> objects in the collection. </param>
		public virtual EventDescriptorCollection Sort(string[] order)
		{
			EventDescriptorCollection eventDescriptorCollection = this.CloneCollection();
			eventDescriptorCollection.InternalSort(order);
			return eventDescriptorCollection;
		}

		/// <summary>Sorts the members of this <see cref="T:System.ComponentModel.EventDescriptorCollection" />, given a specified sort order and an <see cref="T:System.Collections.IComparer" />.</summary>
		/// <returns>The new <see cref="T:System.ComponentModel.EventDescriptorCollection" />.</returns>
		/// <param name="names">An array of strings describing the order in which to sort the <see cref="T:System.ComponentModel.EventDescriptor" /> objects in the collection. </param>
		/// <param name="comparer">An <see cref="T:System.Collections.IComparer" /> to use to sort the <see cref="T:System.ComponentModel.EventDescriptor" /> objects in this collection. </param>
		public virtual EventDescriptorCollection Sort(string[] order, IComparer comparer)
		{
			EventDescriptorCollection eventDescriptorCollection = this.CloneCollection();
			if (order != null)
			{
				ArrayList arrayList = eventDescriptorCollection.ExtractItems(order);
				eventDescriptorCollection.InternalSort(comparer);
				arrayList.AddRange(eventDescriptorCollection.eventList);
				eventDescriptorCollection.eventList = arrayList;
			}
			else
			{
				eventDescriptorCollection.InternalSort(comparer);
			}
			return eventDescriptorCollection;
		}

		/// <summary>Sorts the members of this <see cref="T:System.ComponentModel.EventDescriptorCollection" />, using the specified <see cref="T:System.Collections.IComparer" />.</summary>
		/// <param name="sorter">A comparer to use to sort the <see cref="T:System.ComponentModel.EventDescriptor" /> objects in this collection. </param>
		protected void InternalSort(IComparer comparer)
		{
			if (comparer == null)
			{
				comparer = MemberDescriptor.DefaultComparer;
			}
			this.eventList.Sort(comparer);
		}

		/// <summary>Sorts the members of this <see cref="T:System.ComponentModel.EventDescriptorCollection" />. The specified order is applied first, followed by the default sort for this collection, which is usually alphabetical.</summary>
		/// <param name="names">An array of strings describing the order in which to sort the <see cref="T:System.ComponentModel.EventDescriptor" /> objects in this collection. </param>
		protected void InternalSort(string[] order)
		{
			if (order != null)
			{
				ArrayList arrayList = this.ExtractItems(order);
				this.InternalSort(null);
				arrayList.AddRange(this.eventList);
				this.eventList = arrayList;
			}
			else
			{
				this.InternalSort(null);
			}
		}

		private ArrayList ExtractItems(string[] names)
		{
			ArrayList arrayList = new ArrayList(this.eventList.Count);
			object[] array = new object[names.Length];
			for (int i = 0; i < this.eventList.Count; i++)
			{
				EventDescriptor eventDescriptor = (EventDescriptor)this.eventList[i];
				int num = Array.IndexOf<string>(names, eventDescriptor.Name);
				if (num != -1)
				{
					array[num] = eventDescriptor;
					this.eventList.RemoveAt(i);
					i--;
				}
			}
			foreach (object obj in array)
			{
				if (obj != null)
				{
					arrayList.Add(obj);
				}
			}
			return arrayList;
		}

		private EventDescriptorCollection CloneCollection()
		{
			return new EventDescriptorCollection
			{
				eventList = (ArrayList)this.eventList.Clone()
			};
		}

		internal EventDescriptorCollection Filter(Attribute[] attributes)
		{
			EventDescriptorCollection eventDescriptorCollection = new EventDescriptorCollection();
			foreach (object obj in this.eventList)
			{
				EventDescriptor eventDescriptor = (EventDescriptor)obj;
				if (eventDescriptor.Attributes.Contains(attributes))
				{
					eventDescriptorCollection.eventList.Add(eventDescriptor);
				}
			}
			return eventDescriptorCollection;
		}

		/// <summary>Gets the number of event descriptors in the collection.</summary>
		/// <returns>The number of event descriptors in the collection.</returns>
		public int Count
		{
			get
			{
				return this.eventList.Count;
			}
		}

		/// <summary>Gets or sets the event with the specified name.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.EventDescriptor" /> with the specified name, or null if the event does not exist.</returns>
		/// <param name="name">The name of the <see cref="T:System.ComponentModel.EventDescriptor" /> to get or set. </param>
		public virtual EventDescriptor this[string name]
		{
			get
			{
				return this.Find(name, false);
			}
		}

		/// <summary>Gets or sets the event with the specified index number.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.EventDescriptor" /> with the specified index number.</returns>
		/// <param name="index">The zero-based index number of the <see cref="T:System.ComponentModel.EventDescriptor" /> to get or set. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="index" /> is not a valid index for <see cref="P:System.ComponentModel.EventDescriptorCollection.Item(System.Int32)" />. </exception>
		public virtual EventDescriptor this[int index]
		{
			get
			{
				return (EventDescriptor)this.eventList[index];
			}
		}
	}
}
