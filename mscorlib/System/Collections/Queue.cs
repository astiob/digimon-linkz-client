using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Collections
{
	/// <summary>Represents a first-in, first-out collection of objects.</summary>
	/// <filterpriority>1</filterpriority>
	[DebuggerDisplay("Count={Count}")]
	[ComVisible(true)]
	[DebuggerTypeProxy(typeof(CollectionDebuggerView))]
	[Serializable]
	public class Queue : IEnumerable, ICloneable, ICollection
	{
		private object[] _array;

		private int _head;

		private int _size;

		private int _tail;

		private int _growFactor;

		private int _version;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Queue" /> class that is empty, has the default initial capacity, and uses the default growth factor.</summary>
		public Queue() : this(32, 2f)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Queue" /> class that is empty, has the specified initial capacity, and uses the default growth factor.</summary>
		/// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Queue" /> can contain. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero. </exception>
		public Queue(int capacity) : this(capacity, 2f)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Queue" /> class that contains elements copied from the specified collection, has the same initial capacity as the number of elements copied, and uses the default growth factor.</summary>
		/// <param name="col">The <see cref="T:System.Collections.ICollection" /> to copy elements from. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="col" /> is null. </exception>
		public Queue(ICollection col) : this((col != null) ? col.Count : 32)
		{
			if (col == null)
			{
				throw new ArgumentNullException("col");
			}
			foreach (object obj in col)
			{
				this.Enqueue(obj);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Queue" /> class that is empty, has the specified initial capacity, and uses the specified growth factor.</summary>
		/// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Queue" /> can contain. </param>
		/// <param name="growFactor">The factor by which the capacity of the <see cref="T:System.Collections.Queue" /> is expanded. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero.-or- <paramref name="growFactor" /> is less than 1.0 or greater than 10.0. </exception>
		public Queue(int capacity, float growFactor)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", "Needs a non-negative number");
			}
			if (growFactor < 1f || growFactor > 10f)
			{
				throw new ArgumentOutOfRangeException("growFactor", "Queue growth factor must be between 1.0 and 10.0, inclusive");
			}
			this._array = new object[capacity];
			this._growFactor = (int)(growFactor * 100f);
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Queue" />.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Queue" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual int Count
		{
			get
			{
				return this._size;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.Queue" /> is synchronized (thread safe).</summary>
		/// <returns>true if access to the <see cref="T:System.Collections.Queue" /> is synchronized (thread safe); otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.Queue" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.Queue" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Copies the <see cref="T:System.Collections.Queue" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Queue" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.Queue" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.ArrayTypeMismatchException">The type of the source <see cref="T:System.Collections.Queue" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (array.Rank > 1 || (index != 0 && index >= array.Length) || this._size > array.Length - index)
			{
				throw new ArgumentException();
			}
			int num = this._array.Length;
			int num2 = num - this._head;
			Array.Copy(this._array, this._head, array, index, Math.Min(this._size, num2));
			if (this._size > num2)
			{
				Array.Copy(this._array, 0, array, index + num2, this._size - num2);
			}
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Queue" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.Queue" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual IEnumerator GetEnumerator()
		{
			return new Queue.QueueEnumerator(this);
		}

		/// <summary>Creates a shallow copy of the <see cref="T:System.Collections.Queue" />.</summary>
		/// <returns>A shallow copy of the <see cref="T:System.Collections.Queue" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual object Clone()
		{
			Queue queue = new Queue(this._array.Length);
			queue._growFactor = this._growFactor;
			Array.Copy(this._array, 0, queue._array, 0, this._array.Length);
			queue._head = this._head;
			queue._size = this._size;
			queue._tail = this._tail;
			return queue;
		}

		/// <summary>Removes all objects from the <see cref="T:System.Collections.Queue" />.</summary>
		/// <filterpriority>2</filterpriority>
		public virtual void Clear()
		{
			this._version++;
			this._head = 0;
			this._size = 0;
			this._tail = 0;
			for (int i = this._array.Length - 1; i >= 0; i--)
			{
				this._array[i] = null;
			}
		}

		/// <summary>Determines whether an element is in the <see cref="T:System.Collections.Queue" />.</summary>
		/// <returns>true if <paramref name="obj" /> is found in the <see cref="T:System.Collections.Queue" />; otherwise, false.</returns>
		/// <param name="obj">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.Queue" />. The value can be null. </param>
		/// <filterpriority>2</filterpriority>
		public virtual bool Contains(object obj)
		{
			int num = this._head + this._size;
			if (obj == null)
			{
				for (int i = this._head; i < num; i++)
				{
					if (this._array[i % this._array.Length] == null)
					{
						return true;
					}
				}
			}
			else
			{
				for (int j = this._head; j < num; j++)
				{
					if (obj.Equals(this._array[j % this._array.Length]))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>Removes and returns the object at the beginning of the <see cref="T:System.Collections.Queue" />.</summary>
		/// <returns>The object that is removed from the beginning of the <see cref="T:System.Collections.Queue" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Queue" /> is empty. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual object Dequeue()
		{
			this._version++;
			if (this._size < 1)
			{
				throw new InvalidOperationException();
			}
			object result = this._array[this._head];
			this._array[this._head] = null;
			this._head = (this._head + 1) % this._array.Length;
			this._size--;
			return result;
		}

		/// <summary>Adds an object to the end of the <see cref="T:System.Collections.Queue" />.</summary>
		/// <param name="obj">The object to add to the <see cref="T:System.Collections.Queue" />. The value can be null. </param>
		/// <filterpriority>2</filterpriority>
		public virtual void Enqueue(object obj)
		{
			this._version++;
			if (this._size == this._array.Length)
			{
				this.grow();
			}
			this._array[this._tail] = obj;
			this._tail = (this._tail + 1) % this._array.Length;
			this._size++;
		}

		/// <summary>Returns the object at the beginning of the <see cref="T:System.Collections.Queue" /> without removing it.</summary>
		/// <returns>The object at the beginning of the <see cref="T:System.Collections.Queue" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Queue" /> is empty. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual object Peek()
		{
			if (this._size < 1)
			{
				throw new InvalidOperationException();
			}
			return this._array[this._head];
		}

		/// <summary>Returns a <see cref="T:System.Collections.Queue" /> wrapper that is synchronized (thread safe).</summary>
		/// <returns>A <see cref="T:System.Collections.Queue" /> wrapper that is synchronized (thread safe).</returns>
		/// <param name="queue">The <see cref="T:System.Collections.Queue" /> to synchronize. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="queue" /> is null. </exception>
		/// <filterpriority>2</filterpriority>
		public static Queue Synchronized(Queue queue)
		{
			if (queue == null)
			{
				throw new ArgumentNullException("queue");
			}
			return new Queue.SyncQueue(queue);
		}

		/// <summary>Copies the <see cref="T:System.Collections.Queue" /> elements to a new array.</summary>
		/// <returns>A new array containing elements copied from the <see cref="T:System.Collections.Queue" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual object[] ToArray()
		{
			object[] array = new object[this._size];
			this.CopyTo(array, 0);
			return array;
		}

		/// <summary>Sets the capacity to the actual number of elements in the <see cref="T:System.Collections.Queue" />.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Queue" /> is read-only.</exception>
		/// <filterpriority>2</filterpriority>
		public virtual void TrimToSize()
		{
			this._version++;
			object[] array = new object[this._size];
			this.CopyTo(array, 0);
			this._array = array;
			this._head = 0;
			this._tail = 0;
		}

		private void grow()
		{
			int num = this._array.Length * this._growFactor / 100;
			if (num < this._array.Length + 1)
			{
				num = this._array.Length + 1;
			}
			object[] array = new object[num];
			this.CopyTo(array, 0);
			this._array = array;
			this._head = 0;
			this._tail = this._head + this._size;
		}

		private class SyncQueue : Queue
		{
			private Queue queue;

			internal SyncQueue(Queue queue)
			{
				this.queue = queue;
			}

			public override int Count
			{
				get
				{
					Queue obj = this.queue;
					int count;
					lock (obj)
					{
						count = this.queue.Count;
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
					return this.queue.SyncRoot;
				}
			}

			public override void CopyTo(Array array, int index)
			{
				Queue obj = this.queue;
				lock (obj)
				{
					this.queue.CopyTo(array, index);
				}
			}

			public override IEnumerator GetEnumerator()
			{
				Queue obj = this.queue;
				IEnumerator enumerator;
				lock (obj)
				{
					enumerator = this.queue.GetEnumerator();
				}
				return enumerator;
			}

			public override object Clone()
			{
				Queue obj = this.queue;
				object result;
				lock (obj)
				{
					result = new Queue.SyncQueue((Queue)this.queue.Clone());
				}
				return result;
			}

			public override void Clear()
			{
				Queue obj = this.queue;
				lock (obj)
				{
					this.queue.Clear();
				}
			}

			public override void TrimToSize()
			{
				Queue obj = this.queue;
				lock (obj)
				{
					this.queue.TrimToSize();
				}
			}

			public override bool Contains(object obj)
			{
				Queue obj2 = this.queue;
				bool result;
				lock (obj2)
				{
					result = this.queue.Contains(obj);
				}
				return result;
			}

			public override object Dequeue()
			{
				Queue obj = this.queue;
				object result;
				lock (obj)
				{
					result = this.queue.Dequeue();
				}
				return result;
			}

			public override void Enqueue(object obj)
			{
				Queue obj2 = this.queue;
				lock (obj2)
				{
					this.queue.Enqueue(obj);
				}
			}

			public override object Peek()
			{
				Queue obj = this.queue;
				object result;
				lock (obj)
				{
					result = this.queue.Peek();
				}
				return result;
			}

			public override object[] ToArray()
			{
				Queue obj = this.queue;
				object[] result;
				lock (obj)
				{
					result = this.queue.ToArray();
				}
				return result;
			}
		}

		[Serializable]
		private class QueueEnumerator : IEnumerator, ICloneable
		{
			private Queue queue;

			private int _version;

			private int current;

			internal QueueEnumerator(Queue q)
			{
				this.queue = q;
				this._version = q._version;
				this.current = -1;
			}

			public object Clone()
			{
				return new Queue.QueueEnumerator(this.queue)
				{
					_version = this._version,
					current = this.current
				};
			}

			public virtual object Current
			{
				get
				{
					if (this._version != this.queue._version || this.current < 0 || this.current >= this.queue._size)
					{
						throw new InvalidOperationException();
					}
					return this.queue._array[(this.queue._head + this.current) % this.queue._array.Length];
				}
			}

			public virtual bool MoveNext()
			{
				if (this._version != this.queue._version)
				{
					throw new InvalidOperationException();
				}
				if (this.current >= this.queue._size - 1)
				{
					this.current = int.MaxValue;
					return false;
				}
				this.current++;
				return true;
			}

			public virtual void Reset()
			{
				if (this._version != this.queue._version)
				{
					throw new InvalidOperationException();
				}
				this.current = -1;
			}
		}
	}
}
