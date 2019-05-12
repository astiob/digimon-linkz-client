using System;
using System.Runtime.InteropServices;

namespace System.Collections.Generic
{
	/// <summary>Represents a node in a <see cref="T:System.Collections.Generic.LinkedList`1" />. This class cannot be inherited.</summary>
	/// <typeparam name="T">Specifies the element type of the linked list.</typeparam>
	/// <filterpriority>1</filterpriority>
	[ComVisible(false)]
	public sealed class LinkedListNode<T>
	{
		private T item;

		private LinkedList<T> container;

		internal LinkedListNode<T> forward;

		internal LinkedListNode<T> back;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.LinkedListNode`1" /> class, containing the specified value.</summary>
		/// <param name="value">The value to contain in the <see cref="T:System.Collections.Generic.LinkedListNode`1" />.</param>
		public LinkedListNode(T value)
		{
			this.item = value;
		}

		internal LinkedListNode(LinkedList<T> list, T value)
		{
			this.container = list;
			this.item = value;
			this.forward = this;
			this.back = this;
		}

		internal LinkedListNode(LinkedList<T> list, T value, LinkedListNode<T> previousNode, LinkedListNode<T> nextNode)
		{
			this.container = list;
			this.item = value;
			this.back = previousNode;
			this.forward = nextNode;
			previousNode.forward = this;
			nextNode.back = this;
		}

		internal void Detach()
		{
			this.back.forward = this.forward;
			this.forward.back = this.back;
			this.forward = (this.back = null);
			this.container = null;
		}

		internal void SelfReference(LinkedList<T> list)
		{
			this.forward = this;
			this.back = this;
			this.container = list;
		}

		internal void InsertBetween(LinkedListNode<T> previousNode, LinkedListNode<T> nextNode, LinkedList<T> list)
		{
			previousNode.forward = this;
			nextNode.back = this;
			this.forward = nextNode;
			this.back = previousNode;
			this.container = list;
		}

		/// <summary>Gets the <see cref="T:System.Collections.Generic.LinkedList`1" /> that the <see cref="T:System.Collections.Generic.LinkedListNode`1" /> belongs to.</summary>
		/// <returns>A reference to the <see cref="T:System.Collections.Generic.LinkedList`1" /> that the <see cref="T:System.Collections.Generic.LinkedListNode`1" /> belongs to, or null if the <see cref="T:System.Collections.Generic.LinkedListNode`1" /> is not linked.</returns>
		public LinkedList<T> List
		{
			get
			{
				return this.container;
			}
		}

		/// <summary>Gets the next node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>A reference to the next node in the <see cref="T:System.Collections.Generic.LinkedList`1" />, or null if the current node is the last element (<see cref="P:System.Collections.Generic.LinkedList`1.Last" />) of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
		public LinkedListNode<T> Next
		{
			get
			{
				return (this.container == null || this.forward == this.container.first) ? null : this.forward;
			}
		}

		/// <summary>Gets the previous node in the <see cref="T:System.Collections.Generic.LinkedList`1" />.</summary>
		/// <returns>A reference to the previous node in the <see cref="T:System.Collections.Generic.LinkedList`1" />, or null if the current node is the first element (<see cref="P:System.Collections.Generic.LinkedList`1.First" />) of the <see cref="T:System.Collections.Generic.LinkedList`1" />.</returns>
		public LinkedListNode<T> Previous
		{
			get
			{
				return (this.container == null || this == this.container.first) ? null : this.back;
			}
		}

		/// <summary>Gets the value contained in the node.</summary>
		/// <returns>The value contained in the node.</returns>
		public T Value
		{
			get
			{
				return this.item;
			}
			set
			{
				this.item = value;
			}
		}
	}
}
