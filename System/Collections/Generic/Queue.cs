using System;
using System.Runtime.InteropServices;

namespace System.Collections.Generic
{
	/// <summary>Represents a first-in, first-out collection of objects.</summary>
	/// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
	/// <filterpriority>1</filterpriority>
	[ComVisible(false)]
	[Serializable]
	public class Queue<T> : IEnumerable<T>, ICollection, IEnumerable
	{
		private T[] _array;

		private int _head;

		private int _tail;

		private int _size;

		private int _version;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Queue`1" /> class that is empty and has the default initial capacity.</summary>
		public Queue()
		{
			this._array = new T[0];
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Queue`1" /> class that is empty and has the specified initial capacity.</summary>
		/// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Queue`1" /> can contain.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero.</exception>
		public Queue(int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this._array = new T[count];
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Queue`1" /> class that contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.</summary>
		/// <param name="collection">The collection whose elements are copied to the new <see cref="T:System.Collections.Generic.Queue`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="collection" /> is null.</exception>
		public Queue(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			ICollection<T> collection2 = collection as ICollection<T>;
			int num = (collection2 == null) ? 0 : collection2.Count;
			this._array = new T[num];
			foreach (T item in collection)
			{
				this.Enqueue(item);
			}
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
		void ICollection.CopyTo(Array array, int idx)
		{
			if (array == null)
			{
				throw new ArgumentNullException();
			}
			if (idx > array.Length)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (array.Length - idx < this._size)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (this._size == 0)
			{
				return;
			}
			try
			{
				int num = this._array.Length;
				int num2 = num - this._head;
				Array.Copy(this._array, this._head, array, idx, Math.Min(this._size, num2));
				if (this._size > num2)
				{
					Array.Copy(this._array, 0, array, idx + num2, this._size - num2);
				}
			}
			catch (ArrayTypeMismatchException)
			{
				throw new ArgumentException();
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.  In the default implementation of <see cref="T:System.Collections.Generic.Queue`1" />, this property always returns false.</returns>
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.Queue`1" />, this property always returns the current instance.</returns>
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>Removes all objects from the <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
		/// <filterpriority>1</filterpriority>
		public void Clear()
		{
			Array.Clear(this._array, 0, this._array.Length);
			this._head = (this._tail = (this._size = 0));
			this._version++;
		}

		/// <summary>Determines whether an element is in the <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
		/// <returns>true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.Queue`1" />; otherwise, false.</returns>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.Queue`1" />. The value can be null for reference types.</param>
		public bool Contains(T item)
		{
			if (item == null)
			{
				foreach (T t in this)
				{
					if (t == null)
					{
						return true;
					}
				}
			}
			else
			{
				foreach (T t2 in this)
				{
					if (item.Equals(t2))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>Copies the <see cref="T:System.Collections.Generic.Queue`1" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.Queue`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="arrayIndex" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.Queue`1" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
		public void CopyTo(T[] array, int idx)
		{
			if (array == null)
			{
				throw new ArgumentNullException();
			}
			((ICollection)this).CopyTo(array, idx);
		}

		/// <summary>Removes and returns the object at the beginning of the <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
		/// <returns>The object that is removed from the beginning of the <see cref="T:System.Collections.Generic.Queue`1" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.Queue`1" /> is empty.</exception>
		public T Dequeue()
		{
			T result = this.Peek();
			this._array[this._head] = default(T);
			if (++this._head == this._array.Length)
			{
				this._head = 0;
			}
			this._size--;
			this._version++;
			return result;
		}

		/// <summary>Returns the object at the beginning of the <see cref="T:System.Collections.Generic.Queue`1" /> without removing it.</summary>
		/// <returns>The object at the beginning of the <see cref="T:System.Collections.Generic.Queue`1" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.Queue`1" /> is empty.</exception>
		public T Peek()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException();
			}
			return this._array[this._head];
		}

		/// <summary>Adds an object to the end of the <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.Queue`1" />. The value can be null for reference types.</param>
		public void Enqueue(T item)
		{
			if (this._size == this._array.Length || this._tail == this._array.Length)
			{
				this.SetCapacity(Math.Max(Math.Max(this._size, this._tail) * 2, 4));
			}
			this._array[this._tail] = item;
			if (++this._tail == this._array.Length)
			{
				this._tail = 0;
			}
			this._size++;
			this._version++;
		}

		/// <summary>Copies the <see cref="T:System.Collections.Generic.Queue`1" /> elements to a new array.</summary>
		/// <returns>A new array containing elements copied from the <see cref="T:System.Collections.Generic.Queue`1" />.</returns>
		public T[] ToArray()
		{
			T[] array = new T[this._size];
			this.CopyTo(array, 0);
			return array;
		}

		/// <summary>Sets the capacity to the actual number of elements in the <see cref="T:System.Collections.Generic.Queue`1" />, if that number is less than 90 percent of current capacity.</summary>
		public void TrimExcess()
		{
			if ((double)this._size < (double)this._array.Length * 0.9)
			{
				this.SetCapacity(this._size);
			}
		}

		private void SetCapacity(int new_size)
		{
			if (new_size == this._array.Length)
			{
				return;
			}
			if (new_size < this._size)
			{
				throw new InvalidOperationException("shouldnt happen");
			}
			T[] array = new T[new_size];
			if (this._size > 0)
			{
				this.CopyTo(array, 0);
			}
			this._array = array;
			this._tail = this._size;
			this._head = 0;
			this._version++;
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.Queue`1" />.</returns>
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.Queue`1.Enumerator" /> for the <see cref="T:System.Collections.Generic.Queue`1" />.</returns>
		public Queue<T>.Enumerator GetEnumerator()
		{
			return new Queue<T>.Enumerator(this);
		}

		/// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
		[Serializable]
		public struct Enumerator : IEnumerator, IDisposable, IEnumerator<T>
		{
			private const int NOT_STARTED = -2;

			private const int FINISHED = -1;

			private Queue<T> q;

			private int idx;

			private int ver;

			internal Enumerator(Queue<T> q)
			{
				this.q = q;
				this.idx = -2;
				this.ver = q._version;
			}

			/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			void IEnumerator.Reset()
			{
				if (this.ver != this.q._version)
				{
					throw new InvalidOperationException();
				}
				this.idx = -2;
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

			/// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Queue`1.Enumerator" />.</summary>
			public void Dispose()
			{
				this.idx = -2;
			}

			/// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Queue`1" />.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			public bool MoveNext()
			{
				if (this.ver != this.q._version)
				{
					throw new InvalidOperationException();
				}
				if (this.idx == -2)
				{
					this.idx = this.q._size;
				}
				return this.idx != -1 && --this.idx != -1;
			}

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the <see cref="T:System.Collections.Generic.Queue`1" /> at the current position of the enumerator.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
			public T Current
			{
				get
				{
					if (this.idx < 0)
					{
						throw new InvalidOperationException();
					}
					return this.q._array[(this.q._size - 1 - this.idx + this.q._head) % this.q._array.Length];
				}
			}
		}
	}
}
