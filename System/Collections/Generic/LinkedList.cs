using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Collections.Generic
{
	/// <summary>Represents a doubly linked list.</summary>
	/// <typeparam name="T">Specifies the element type of the linked list.</typeparam>
	/// <filterpriority>1</filterpriority>
	[ComVisible(false)]
	[Serializable]
	public class LinkedList<T> : IEnumerable<T>, ICollection, IDeserializationCallback, IEnumerable, ICollection<T>, ISerializable
	{
		private const string DataArrayKey = "DataArray";

		private const string VersionKey = "version";

		private uint count;

		private uint version;

		private object syncRoot;

		internal LinkedListNode<T> first;

		internal SerializationInfo si;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.LinkedList`1" /> class that is empty.</summary>
		public LinkedList()
		{
			this.syncRoot = new object();
			this.first = null;
			this.count = (this.version = 0u);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.LinkedList`1" /> class that contains elements copied from the specified <see cref="T:System.Collections.IEnumerable" /> and has sufficient capacity to accommodate the number of elements copied. </summary>
		/// <param name="collection">The <see cref="T:System.Collections.IEnumerable" /> whose elements are copied to the new <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="collection" /> is null.</exception>
		public LinkedList(IEnumerable<T> collection) : this()
		{
			foreach (T value in collection)
			{
				this.AddLast(value);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.LinkedList`1" /> class that is serializable with the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" />.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		protected LinkedList(SerializationInfo info, StreamingContext context) : this()
		{
			this.si = info;
			this.syncRoot = new object();
		}

		void ICollection<T>.Add(T value)
		{
			this.AddLast(value);
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
			T[] array2 = array as T[];
			if (array2 == null)
			{
				throw new ArgumentException("array");
			}
			this.CopyTo(array2, index);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through the linked list as a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the linked list as a collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.LinkedList`1" />, this property always returns false.</returns>
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.LinkedList`1" />, this property always returns the current instance.</returns>
		object ICollection.SyncRoot
		{
			get
			{
				return this.syncRoot;
			}
		}

		private void VerifyReferencedNode(LinkedListNode<T> node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (node.List != this)
			{
				throw new InvalidOperationException();
			}
		}

		private static void VerifyBlankNode(LinkedListNode<T> newNode)
		{
			if (newNode == null)
			{
				throw new ArgumentNullException("newNode");
			}
			if (newNode.List != null)
			{
				throw new InvalidOperationException();
			}
		}

		/// <summary>Adds a new node containing the specified value after the specified existing node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</returns>
		/// <param name="node">The <see cref="T:System.Collections.Generic.LinkedListNode`1" /> after which to insert a new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</param>
		/// <param name="value">The value to add to the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="node" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="node" /> is not in the current <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
		public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
		{
			this.VerifyReferencedNode(node);
			LinkedListNode<T> result = new LinkedListNode<T>(this, value, node, node.forward);
			this.count += 1u;
			this.version += 1u;
			return result;
		}

		/// <summary>Adds the specified new node after the specified existing node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <param name="node">The <see cref="T:System.Collections.Generic.LinkedListNode`1" /> after which to insert <paramref name="newNode" />.</param>
		/// <param name="newNode">The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> to add to the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="node" /> is null.-or-<paramref name="newNode" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="node" /> is not in the current <see cref="T:System.Collections.Generic.LinkedList`1" />.-or-<paramref name="newNode" /> belongs to another <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
		public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
		{
			this.VerifyReferencedNode(node);
			LinkedList<T>.VerifyBlankNode(newNode);
			newNode.InsertBetween(node, node.forward, this);
			this.count += 1u;
			this.version += 1u;
		}

		/// <summary>Adds a new node containing the specified value before the specified existing node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</returns>
		/// <param name="node">The <see cref="T:System.Collections.Generic.LinkedListNode`1" /> before which to insert a new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</param>
		/// <param name="value">The value to add to the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="node" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="node" /> is not in the current <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
		public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
		{
			this.VerifyReferencedNode(node);
			LinkedListNode<T> result = new LinkedListNode<T>(this, value, node.back, node);
			this.count += 1u;
			this.version += 1u;
			if (node == this.first)
			{
				this.first = result;
			}
			return result;
		}

		/// <summary>Adds the specified new node before the specified existing node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <param name="node">The <see cref="T:System.Collections.Generic.LinkedListNode`1" /> before which to insert <paramref name="newNode" />.</param>
		/// <param name="newNode">The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> to add to the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="node" /> is null.-or-<paramref name="newNode" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="node" /> is not in the current <see cref="T:System.Collections.Generic.LinkedList`1" />.-or-<paramref name="newNode" /> belongs to another <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
		public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
		{
			this.VerifyReferencedNode(node);
			LinkedList<T>.VerifyBlankNode(newNode);
			newNode.InsertBetween(node.back, node, this);
			this.count += 1u;
			this.version += 1u;
			if (node == this.first)
			{
				this.first = newNode;
			}
		}

		/// <summary>Adds the specified new node at the start of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <param name="node">The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> to add at the start of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="node" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="node" /> belongs to another <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
		public void AddFirst(LinkedListNode<T> node)
		{
			LinkedList<T>.VerifyBlankNode(node);
			if (this.first == null)
			{
				node.SelfReference(this);
			}
			else
			{
				node.InsertBetween(this.first.back, this.first, this);
			}
			this.count += 1u;
			this.version += 1u;
			this.first = node;
		}

		/// <summary>Adds a new node containing the specified value at the start of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</returns>
		/// <param name="value">The value to add at the start of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		public LinkedListNode<T> AddFirst(T value)
		{
			LinkedListNode<T> result;
			if (this.first == null)
			{
				result = new LinkedListNode<T>(this, value);
			}
			else
			{
				result = new LinkedListNode<T>(this, value, this.first.back, this.first);
			}
			this.count += 1u;
			this.version += 1u;
			this.first = result;
			return result;
		}

		/// <summary>Adds a new node containing the specified value at the end of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> containing <paramref name="value" />.</returns>
		/// <param name="value">The value to add at the end of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		public LinkedListNode<T> AddLast(T value)
		{
			LinkedListNode<T> result;
			if (this.first == null)
			{
				result = new LinkedListNode<T>(this, value);
				this.first = result;
			}
			else
			{
				result = new LinkedListNode<T>(this, value, this.first.back, this.first);
			}
			this.count += 1u;
			this.version += 1u;
			return result;
		}

		/// <summary>Adds the specified new node at the end of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <param name="node">The new <see cref="T:System.Collections.Generic.LinkedListNode`1" /> to add at the end of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="node" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="node" /> belongs to another <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
		public void AddLast(LinkedListNode<T> node)
		{
			LinkedList<T>.VerifyBlankNode(node);
			if (this.first == null)
			{
				node.SelfReference(this);
				this.first = node;
			}
			else
			{
				node.InsertBetween(this.first.back, this.first, this);
			}
			this.count += 1u;
			this.version += 1u;
		}

		/// <summary>Removes all nodes from the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		public void Clear()
		{
			while (this.first != null)
			{
				this.RemoveLast();
			}
		}

		/// <summary>Determines whether a value is in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>true if <paramref name="value" /> is found in the <see cref="T:System.Collections.Generic.LinkedList`1" />; otherwise, false.</returns>
		/// <param name="value">The value to locate in the <see cref="T:System.Collections.Generic.LinkedList`1" />. The value can be null for reference types.</param>
		public bool Contains(T value)
		{
			LinkedListNode<T> forward = this.first;
			if (forward == null)
			{
				return false;
			}
			while (!value.Equals(forward.Value))
			{
				forward = forward.forward;
				if (forward == this.first)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Copies the entire <see cref="T:System.Collections.Generic.LinkedList`1" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.LinkedList`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.LinkedList`1" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		public void CopyTo(T[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < array.GetLowerBound(0))
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException("array", "Array is multidimensional");
			}
			if ((long)(array.Length - index + array.GetLowerBound(0)) < (long)((ulong)this.count))
			{
				throw new ArgumentException("number of items exceeds capacity");
			}
			LinkedListNode<T> forward = this.first;
			if (this.first == null)
			{
				return;
			}
			do
			{
				array[index] = forward.Value;
				index++;
				forward = forward.forward;
			}
			while (forward != this.first);
		}

		/// <summary>Finds the first node that contains the specified value.</summary>
		/// <returns>The first <see cref="T:System.Collections.Generic.LinkedListNode`1" /> that contains the specified value, if found; otherwise, null.</returns>
		/// <param name="value">The value to locate in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		public LinkedListNode<T> Find(T value)
		{
			LinkedListNode<T> forward = this.first;
			if (forward == null)
			{
				return null;
			}
			while ((value != null || forward.Value != null) && (value == null || !value.Equals(forward.Value)))
			{
				forward = forward.forward;
				if (forward == this.first)
				{
					return null;
				}
			}
			return forward;
		}

		/// <summary>Finds the last node that contains the specified value.</summary>
		/// <returns>The last <see cref="T:System.Collections.Generic.LinkedListNode`1" /> that contains the specified value, if found; otherwise, null.</returns>
		/// <param name="value">The value to locate in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		public LinkedListNode<T> FindLast(T value)
		{
			LinkedListNode<T> back = this.first;
			if (back == null)
			{
				return null;
			}
			for (;;)
			{
				back = back.back;
				if (value.Equals(back.Value))
				{
					break;
				}
				if (back == this.first)
				{
					goto Block_3;
				}
			}
			return back;
			Block_3:
			return null;
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.LinkedList`1.Enumerator" /> for the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
		public LinkedList<T>.Enumerator GetEnumerator()
		{
			return new LinkedList<T>.Enumerator(this);
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.Collections.Generic.LinkedList`1" /> instance.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the <see cref="T:System.Collections.Generic.LinkedList`1" /> instance.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.LinkedList`1" /> instance.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null.</exception>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			T[] array = new T[this.count];
			this.CopyTo(array, 0);
			info.AddValue("DataArray", array, typeof(T[]));
			info.AddValue("version", this.version);
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and raises the deserialization event when the deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object associated with the current <see cref="T:System.Collections.Generic.LinkedList`1" /> instance is invalid.</exception>
		public virtual void OnDeserialization(object sender)
		{
			if (this.si != null)
			{
				T[] array = (T[])this.si.GetValue("DataArray", typeof(T[]));
				if (array != null)
				{
					foreach (T value in array)
					{
						this.AddLast(value);
					}
				}
				this.version = this.si.GetUInt32("version");
				this.si = null;
			}
		}

		/// <summary>Removes the first occurrence of the specified value from the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>true if the element containing <paramref name="value" /> is successfully removed; otherwise, false.  This method also returns false if <paramref name="value" /> was not found in the original <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
		/// <param name="value">The value to remove from the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		public bool Remove(T value)
		{
			LinkedListNode<T> linkedListNode = this.Find(value);
			if (linkedListNode == null)
			{
				return false;
			}
			this.Remove(linkedListNode);
			return true;
		}

		/// <summary>Removes the specified node from the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <param name="node">The <see cref="T:System.Collections.Generic.LinkedListNode`1" /> to remove from the <see cref="T:System.Collections.Generic.LinkedList`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="node" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="node" /> is not in the current <see cref="T:System.Collections.Generic.LinkedList`1" />.</exception>
		public void Remove(LinkedListNode<T> node)
		{
			this.VerifyReferencedNode(node);
			this.count -= 1u;
			if (this.count == 0u)
			{
				this.first = null;
			}
			if (node == this.first)
			{
				this.first = this.first.forward;
			}
			this.version += 1u;
			node.Detach();
		}

		/// <summary>Removes the node at the start of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.LinkedList`1" /> is empty.</exception>
		public void RemoveFirst()
		{
			if (this.first != null)
			{
				this.Remove(this.first);
			}
		}

		/// <summary>Removes the node at the end of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.LinkedList`1" /> is empty.</exception>
		public void RemoveLast()
		{
			if (this.first != null)
			{
				this.Remove(this.first.back);
			}
		}

		/// <summary>Gets the number of nodes actually contained in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>The number of nodes actually contained in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
		public int Count
		{
			get
			{
				return (int)this.count;
			}
		}

		/// <summary>Gets the first node of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>The first <see cref="T:System.Collections.Generic.LinkedListNode`1" /> of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
		public LinkedListNode<T> First
		{
			get
			{
				return this.first;
			}
		}

		/// <summary>Gets the last node of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>The last <see cref="T:System.Collections.Generic.LinkedListNode`1" /> of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
		public LinkedListNode<T> Last
		{
			get
			{
				return (this.first == null) ? null : this.first.back;
			}
		}

		/// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		[Serializable]
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private const string VersionKey = "version";

			private const string IndexKey = "index";

			private const string ListKey = "list";

			private LinkedList<T> list;

			private LinkedListNode<T> current;

			private int index;

			private uint version;

			internal Enumerator(LinkedList<T> parent)
			{
				this.list = parent;
				this.current = null;
				this.index = -1;
				this.version = parent.version;
			}

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the collection at the current position of the enumerator.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection. This class cannot be inherited.</summary>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			void IEnumerator.Reset()
			{
				if (this.list == null)
				{
					throw new ObjectDisposedException(null);
				}
				if (this.version != this.list.version)
				{
					throw new InvalidOperationException("list modified");
				}
				this.current = null;
				this.index = -1;
			}

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the <see cref="T:System.Collections.Generic.LinkedList`1" /> at the current position of the enumerator.</returns>
			public T Current
			{
				get
				{
					if (this.list == null)
					{
						throw new ObjectDisposedException(null);
					}
					if (this.current == null)
					{
						throw new InvalidOperationException();
					}
					return this.current.Value;
				}
			}

			/// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			public bool MoveNext()
			{
				if (this.list == null)
				{
					throw new ObjectDisposedException(null);
				}
				if (this.version != this.list.version)
				{
					throw new InvalidOperationException("list modified");
				}
				if (this.current == null)
				{
					this.current = this.list.first;
				}
				else
				{
					this.current = this.current.forward;
					if (this.current == this.list.first)
					{
						this.current = null;
					}
				}
				if (this.current == null)
				{
					this.index = -1;
					return false;
				}
				this.index++;
				return true;
			}

			/// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.LinkedList`1.Enumerator" />.</summary>
			public void Dispose()
			{
				if (this.list == null)
				{
					throw new ObjectDisposedException(null);
				}
				this.current = null;
				this.list = null;
			}
		}
	}
}
