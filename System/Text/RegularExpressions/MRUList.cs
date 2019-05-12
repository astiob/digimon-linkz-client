using System;

namespace System.Text.RegularExpressions
{
	internal class MRUList
	{
		private MRUList.Node head;

		private MRUList.Node tail;

		public MRUList()
		{
			this.head = (this.tail = null);
		}

		public void Use(object o)
		{
			MRUList.Node node;
			if (this.head == null)
			{
				node = new MRUList.Node(o);
				this.head = (this.tail = node);
				return;
			}
			node = this.head;
			while (node != null && !o.Equals(node.value))
			{
				node = node.previous;
			}
			if (node == null)
			{
				node = new MRUList.Node(o);
			}
			else
			{
				if (node == this.head)
				{
					return;
				}
				if (node == this.tail)
				{
					this.tail = node.next;
				}
				else
				{
					node.previous.next = node.next;
				}
				node.next.previous = node.previous;
			}
			this.head.next = node;
			node.previous = this.head;
			node.next = null;
			this.head = node;
		}

		public object Evict()
		{
			if (this.tail == null)
			{
				return null;
			}
			object value = this.tail.value;
			this.tail = this.tail.next;
			if (this.tail == null)
			{
				this.head = null;
			}
			else
			{
				this.tail.previous = null;
			}
			return value;
		}

		private class Node
		{
			public object value;

			public MRUList.Node previous;

			public MRUList.Node next;

			public Node(object value)
			{
				this.value = value;
			}
		}
	}
}
