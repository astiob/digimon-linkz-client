using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Collections
{
	/// <summary>Represents a simple last-in-first-out (LIFO) non-generic collection of objects.</summary>
	/// <filterpriority>1</filterpriority>
	[DebuggerDisplay("Count={Count}")]
	[DebuggerTypeProxy(typeof(CollectionDebuggerView))]
	[ComVisible(true)]
	[Serializable]
	public class Stack : IEnumerable, ICloneable, ICollection
	{
		private const int default_capacity = 16;

		private object[] contents;

		private int current = -1;

		private int count;

		private int capacity;

		private int modCount;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Stack" /> class that is empty and has the default initial capacity.</summary>
		public Stack()
		{
			this.contents = new object[16];
			this.capacity = 16;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Stack" /> class that contains elements copied from the specified collection and has the same initial capacity as the number of elements copied.</summary>
		/// <param name="col">The <see cref="T:System.Collections.ICollection" /> to copy elements from. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="col" /> is null. </exception>
		public Stack(ICollection col) : this((col != null) ? col.Count : 16)
		{
			if (col == null)
			{
				throw new ArgumentNullException("col");
			}
			foreach (object obj in col)
			{
				this.Push(obj);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Stack" /> class that is empty and has the specified initial capacity or the default initial capacity, whichever is greater.</summary>
		/// <param name="initialCapacity">The initial number of elements that the <see cref="T:System.Collections.Stack" /> can contain. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="initialCapacity" /> is less than zero. </exception>
		public Stack(int initialCapacity)
		{
			if (initialCapacity < 0)
			{
				throw new ArgumentOutOfRangeException("initialCapacity");
			}
			this.capacity = initialCapacity;
			this.contents = new object[this.capacity];
		}

		private void Resize(int ncapacity)
		{
			ncapacity = Math.Max(ncapacity, 16);
			object[] destinationArray = new object[ncapacity];
			Array.Copy(this.contents, destinationArray, this.count);
			this.capacity = ncapacity;
			this.contents = destinationArray;
		}

		/// <summary>Returns a synchronized (thread safe) wrapper for the <see cref="T:System.Collections.Stack" />.</summary>
		/// <returns>A synchronized wrapper around the <see cref="T:System.Collections.Stack" />.</returns>
		/// <param name="stack">The <see cref="T:System.Collections.Stack" /> to synchronize. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stack" /> is null. </exception>
		/// <filterpriority>2</filterpriority>
		public static Stack Synchronized(Stack stack)
		{
			if (stack == null)
			{
				throw new ArgumentNullException("stack");
			}
			return new Stack.SyncStack(stack);
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Stack" />.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Stack" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual int Count
		{
			get
			{
				return this.count;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.Stack" /> is synchronized (thread safe).</summary>
		/// <returns>true, if access to the <see cref="T:System.Collections.Stack" /> is synchronized (thread safe); otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.Stack" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> that can be used to synchronize access to the <see cref="T:System.Collections.Stack" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Removes all objects from the <see cref="T:System.Collections.Stack" />.</summary>
		/// <filterpriority>2</filterpriority>
		public virtual void Clear()
		{
			this.modCount++;
			for (int i = 0; i < this.count; i++)
			{
				this.contents[i] = null;
			}
			this.count = 0;
			this.current = -1;
		}

		/// <summary>Creates a shallow copy of the <see cref="T:System.Collections.Stack" />.</summary>
		/// <returns>A shallow copy of the <see cref="T:System.Collections.Stack" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual object Clone()
		{
			return new Stack(this.contents)
			{
				current = this.current,
				count = this.count
			};
		}

		/// <summary>Determines whether an element is in the <see cref="T:System.Collections.Stack" />.</summary>
		/// <returns>true, if <paramref name="obj" /> is found in the <see cref="T:System.Collections.Stack" />; otherwise, false.</returns>
		/// <param name="obj">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.Stack" />. The value can be null. </param>
		/// <filterpriority>2</filterpriority>
		public virtual bool Contains(object obj)
		{
			if (this.count == 0)
			{
				return false;
			}
			if (obj == null)
			{
				for (int i = 0; i < this.count; i++)
				{
					if (this.contents[i] == null)
					{
						return true;
					}
				}
			}
			else
			{
				for (int j = 0; j < this.count; j++)
				{
					if (obj.Equals(this.contents[j]))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>Copies the <see cref="T:System.Collections.Stack" /> to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Stack" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.Stack" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.Stack" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
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
			if (array.Rank > 1 || (array.Length > 0 && index >= array.Length) || this.count > array.Length - index)
			{
				throw new ArgumentException();
			}
			for (int num = this.current; num != -1; num--)
			{
				array.SetValue(this.contents[num], this.count - (num + 1) + index);
			}
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.Stack" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.Stack" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual IEnumerator GetEnumerator()
		{
			return new Stack.Enumerator(this);
		}

		/// <summary>Returns the object at the top of the <see cref="T:System.Collections.Stack" /> without removing it.</summary>
		/// <returns>The <see cref="T:System.Object" /> at the top of the <see cref="T:System.Collections.Stack" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Stack" /> is empty. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual object Peek()
		{
			if (this.current == -1)
			{
				throw new InvalidOperationException();
			}
			return this.contents[this.current];
		}

		/// <summary>Removes and returns the object at the top of the <see cref="T:System.Collections.Stack" />.</summary>
		/// <returns>The <see cref="T:System.Object" /> removed from the top of the <see cref="T:System.Collections.Stack" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Stack" /> is empty. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual object Pop()
		{
			if (this.current == -1)
			{
				throw new InvalidOperationException();
			}
			this.modCount++;
			object result = this.contents[this.current];
			this.contents[this.current] = null;
			this.count--;
			this.current--;
			if (this.count <= this.capacity / 4 && this.count > 16)
			{
				this.Resize(this.capacity / 2);
			}
			return result;
		}

		/// <summary>Inserts an object at the top of the <see cref="T:System.Collections.Stack" />.</summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to push onto the <see cref="T:System.Collections.Stack" />. The value can be null. </param>
		/// <filterpriority>2</filterpriority>
		public virtual void Push(object obj)
		{
			this.modCount++;
			if (this.capacity == this.count)
			{
				this.Resize(this.capacity * 2);
			}
			this.count++;
			this.current++;
			this.contents[this.current] = obj;
		}

		/// <summary>Copies the <see cref="T:System.Collections.Stack" /> to a new array.</summary>
		/// <returns>A new array containing copies of the elements of the <see cref="T:System.Collections.Stack" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual object[] ToArray()
		{
			object[] array = new object[this.count];
			Array.Copy(this.contents, array, this.count);
			Array.Reverse(array);
			return array;
		}

		[Serializable]
		private class SyncStack : Stack
		{
			private Stack stack;

			internal SyncStack(Stack s)
			{
				this.stack = s;
			}

			public override int Count
			{
				get
				{
					Stack obj = this.stack;
					int count;
					lock (obj)
					{
						count = this.stack.Count;
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
					return this.stack.SyncRoot;
				}
			}

			public override void Clear()
			{
				Stack obj = this.stack;
				lock (obj)
				{
					this.stack.Clear();
				}
			}

			public override object Clone()
			{
				Stack obj = this.stack;
				object result;
				lock (obj)
				{
					result = Stack.Synchronized((Stack)this.stack.Clone());
				}
				return result;
			}

			public override bool Contains(object obj)
			{
				Stack obj2 = this.stack;
				bool result;
				lock (obj2)
				{
					result = this.stack.Contains(obj);
				}
				return result;
			}

			public override void CopyTo(Array array, int index)
			{
				Stack obj = this.stack;
				lock (obj)
				{
					this.stack.CopyTo(array, index);
				}
			}

			public override IEnumerator GetEnumerator()
			{
				Stack obj = this.stack;
				IEnumerator result;
				lock (obj)
				{
					result = new Stack.Enumerator(this.stack);
				}
				return result;
			}

			public override object Peek()
			{
				Stack obj = this.stack;
				object result;
				lock (obj)
				{
					result = this.stack.Peek();
				}
				return result;
			}

			public override object Pop()
			{
				Stack obj = this.stack;
				object result;
				lock (obj)
				{
					result = this.stack.Pop();
				}
				return result;
			}

			public override void Push(object obj)
			{
				Stack obj2 = this.stack;
				lock (obj2)
				{
					this.stack.Push(obj);
				}
			}

			public override object[] ToArray()
			{
				Stack obj = this.stack;
				object[] result;
				lock (obj)
				{
					result = this.stack.ToArray();
				}
				return result;
			}
		}

		private class Enumerator : IEnumerator, ICloneable
		{
			private const int EOF = -1;

			private const int BOF = -2;

			private Stack stack;

			private int modCount;

			private int current;

			internal Enumerator(Stack s)
			{
				this.stack = s;
				this.modCount = s.modCount;
				this.current = -2;
			}

			public object Clone()
			{
				return base.MemberwiseClone();
			}

			public virtual object Current
			{
				get
				{
					if (this.modCount != this.stack.modCount || this.current == -2 || this.current == -1 || this.current > this.stack.count)
					{
						throw new InvalidOperationException();
					}
					return this.stack.contents[this.current];
				}
			}

			public virtual bool MoveNext()
			{
				if (this.modCount != this.stack.modCount)
				{
					throw new InvalidOperationException();
				}
				int num = this.current;
				if (num == -2)
				{
					this.current = this.stack.current;
					return this.current != -1;
				}
				if (num != -1)
				{
					this.current--;
					return this.current != -1;
				}
				return false;
			}

			public virtual void Reset()
			{
				if (this.modCount != this.stack.modCount)
				{
					throw new InvalidOperationException();
				}
				this.current = -2;
			}
		}
	}
}
