using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Provides methods for creating, manipulating, searching, and sorting arrays, thereby serving as the base class for all arrays in the common language runtime.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public abstract class Array : IEnumerable, ICloneable, ICollection, IList
	{
		private Array()
		{
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <returns>The element at the specified index.</returns>
		/// <param name="index">The index of the element to get or set.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or-<paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.ICollection.Count" />.</exception>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly one dimension.</exception>
		object IList.this[int index]
		{
			get
			{
				if (index >= this.Length)
				{
					throw new IndexOutOfRangeException("index");
				}
				if (this.Rank > 1)
				{
					throw new ArgumentException(Locale.GetText("Only single dimension arrays are supported."));
				}
				return this.GetValueImpl(index);
			}
			set
			{
				if (index >= this.Length)
				{
					throw new IndexOutOfRangeException("index");
				}
				if (this.Rank > 1)
				{
					throw new ArgumentException(Locale.GetText("Only single dimension arrays are supported."));
				}
				this.SetValueImpl(value, index);
			}
		}

		/// <summary>Implements <see cref="M:System.Collections.IList.Add(System.Object)" />. Throws a <see cref="T:System.NotSupportedException" /> in all cases.</summary>
		/// <returns>An exception is always thrown.</returns>
		/// <param name="value">The object to be added to the <see cref="T:System.Array" />.</param>
		/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
		int IList.Add(object value)
		{
			throw new NotSupportedException();
		}

		/// <summary>Sets all elements in the <see cref="T:System.Array" /> to zero, to false, or to null, depending on the element type.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Array" /> is read-only.</exception>
		void IList.Clear()
		{
			Array.Clear(this, this.GetLowerBound(0), this.Length);
		}

		/// <summary>Determines whether an element is in the <see cref="T:System.Array" />.</summary>
		/// <returns>true if <paramref name="value" /> is found in the <see cref="T:System.Array" />; otherwise, false.</returns>
		/// <param name="value">The object to locate in the <see cref="T:System.Array" />. The element to locate can be null for reference types.</param>
		/// <exception cref="T:System.RankException">The current <see cref="T:System.Array" /> is multidimensional.</exception>
		bool IList.Contains(object value)
		{
			if (this.Rank > 1)
			{
				throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
			}
			int length = this.Length;
			for (int i = 0; i < length; i++)
			{
				if (object.Equals(this.GetValueImpl(i), value))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Searches for the specified object and returns the index of the first occurrence within the current one-dimensional instance.</summary>
		/// <returns>The index of the first occurrence of <paramref name="value" /> within the entire <see cref="T:System.Array" />, if found; otherwise, the lower bound of the <see cref="T:System.Array" /> minus 1.</returns>
		/// <param name="value">The object to locate in the current <see cref="T:System.Array" />.</param>
		/// <exception cref="T:System.RankException">The current <see cref="T:System.Array" /> is multidimensional.</exception>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		int IList.IndexOf(object value)
		{
			if (this.Rank > 1)
			{
				throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
			}
			int length = this.Length;
			for (int i = 0; i < length; i++)
			{
				if (object.Equals(this.GetValueImpl(i), value))
				{
					return i + this.GetLowerBound(0);
				}
			}
			return this.GetLowerBound(0) - 1;
		}

		/// <summary>Implements <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />. Throws a <see cref="T:System.NotSupportedException" /> in all cases.</summary>
		/// <param name="index">The index at which <paramref name="value" /> should be inserted.</param>
		/// <param name="value">The object to insert.</param>
		/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		/// <summary>Implements <see cref="M:System.Collections.IList.Remove(System.Object)" />. Throws a <see cref="T:System.NotSupportedException" /> in all cases.</summary>
		/// <param name="value">The object to remove from the <see cref="T:System.Array" />.</param>
		/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
		void IList.Remove(object value)
		{
			throw new NotSupportedException();
		}

		/// <summary>Implements <see cref="M:System.Collections.IList.RemoveAt(System.Int32)" />. Throws a <see cref="T:System.NotSupportedException" /> in all cases.</summary>
		/// <param name="index">The index of the element to remove.</param>
		/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Array" />.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Array" />.</returns>
		int ICollection.Count
		{
			get
			{
				return this.Length;
			}
		}

		internal int InternalArray__ICollection_get_Count()
		{
			return this.Length;
		}

		internal bool InternalArray__ICollection_get_IsReadOnly()
		{
			return true;
		}

		internal IEnumerator<T> InternalArray__IEnumerable_GetEnumerator<T>()
		{
			return new Array.InternalEnumerator<T>(this);
		}

		internal void InternalArray__ICollection_Clear()
		{
			throw new NotSupportedException("Collection is read-only");
		}

		internal void InternalArray__ICollection_Add<T>(T item)
		{
			throw new NotSupportedException("Collection is read-only");
		}

		internal bool InternalArray__ICollection_Remove<T>(T item)
		{
			throw new NotSupportedException("Collection is read-only");
		}

		internal bool InternalArray__ICollection_Contains<T>(T item)
		{
			if (this.Rank > 1)
			{
				throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
			}
			int length = this.Length;
			for (int i = 0; i < length; i++)
			{
				T t;
				this.GetGenericValueImpl<T>(i, out t);
				if (item == null)
				{
					return t == null;
				}
				if (item.Equals(t))
				{
					return true;
				}
			}
			return false;
		}

		internal void InternalArray__ICollection_CopyTo<T>(T[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (this.Rank > 1)
			{
				throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
			}
			if (index + this.GetLength(0) > array.GetLowerBound(0) + array.GetLength(0))
			{
				throw new ArgumentException("Destination array was not long enough. Check destIndex and length, and the array's lower bounds.");
			}
			if (array.Rank > 1)
			{
				throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Locale.GetText("Value has to be >= 0."));
			}
			Array.Copy(this, this.GetLowerBound(0), array, index, this.GetLength(0));
		}

		internal void InternalArray__Insert<T>(int index, T item)
		{
			throw new NotSupportedException("Collection is read-only");
		}

		internal void InternalArray__RemoveAt(int index)
		{
			throw new NotSupportedException("Collection is read-only");
		}

		internal int InternalArray__IndexOf<T>(T item)
		{
			if (this.Rank > 1)
			{
				throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
			}
			int length = this.Length;
			int i = 0;
			while (i < length)
			{
				T t;
				this.GetGenericValueImpl<T>(i, out t);
				if (item == null)
				{
					if (t == null)
					{
						return i + this.GetLowerBound(0);
					}
					return this.GetLowerBound(0) - 1;
				}
				else
				{
					if (t.Equals(item))
					{
						return i + this.GetLowerBound(0);
					}
					i++;
				}
			}
			return this.GetLowerBound(0) - 1;
		}

		internal T InternalArray__get_Item<T>(int index)
		{
			if (index >= this.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			T result;
			this.GetGenericValueImpl<T>(index, out result);
			return result;
		}

		internal void InternalArray__set_Item<T>(int index, T item)
		{
			if (index >= this.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			object[] array = this as object[];
			if (array != null)
			{
				array[index] = item;
				return;
			}
			this.SetGenericValueImpl<T>(index, ref item);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void GetGenericValueImpl<T>(int pos, out T value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetGenericValueImpl<T>(int pos, ref T value);

		/// <summary>Gets a 32-bit integer that represents the total number of elements in all the dimensions of the <see cref="T:System.Array" />.</summary>
		/// <returns>A 32-bit integer that represents the total number of elements in all the dimensions of the <see cref="T:System.Array" />; zero if there are no elements in the array.</returns>
		/// <filterpriority>1</filterpriority>
		public int Length
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				int num = this.GetLength(0);
				for (int i = 1; i < this.Rank; i++)
				{
					num *= this.GetLength(i);
				}
				return num;
			}
		}

		/// <summary>Gets a 64-bit integer that represents the total number of elements in all the dimensions of the <see cref="T:System.Array" />.</summary>
		/// <returns>A 64-bit integer that represents the total number of elements in all the dimensions of the <see cref="T:System.Array" />.</returns>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public long LongLength
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return (long)this.Length;
			}
		}

		/// <summary>Gets the zero-based rank (number of dimensions) of the <see cref="T:System.Array" />.</summary>
		/// <returns>The zero-based rank (number of dimensions) of the <see cref="T:System.Array" />.</returns>
		/// <filterpriority>1</filterpriority>
		public int Rank
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.GetRank();
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetRank();

		/// <summary>Gets a 32-bit integer that represents the number of elements in the specified dimension of the <see cref="T:System.Array" />.</summary>
		/// <returns>A 32-bit integer that represents the number of elements in the specified dimension.</returns>
		/// <param name="dimension">A zero-based dimension of the <see cref="T:System.Array" /> whose length needs to be determined.</param>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="dimension" /> is less than zero.-or-<paramref name="dimension" /> is equal to or greater than <see cref="P:System.Array.Rank" />.</exception>
		/// <filterpriority>1</filterpriority>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetLength(int dimension);

		/// <summary>Gets a 64-bit integer that represents the number of elements in the specified dimension of the <see cref="T:System.Array" />.</summary>
		/// <returns>A 64-bit integer that represents the number of elements in the specified dimension.</returns>
		/// <param name="dimension">A zero-based dimension of the <see cref="T:System.Array" /> whose length needs to be determined.</param>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="dimension" /> is less than zero.-or-<paramref name="dimension" /> is equal to or greater than <see cref="P:System.Array.Rank" />.</exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public long GetLongLength(int dimension)
		{
			return (long)this.GetLength(dimension);
		}

		/// <summary>Gets the lower bound of the specified dimension in the <see cref="T:System.Array" />.</summary>
		/// <returns>The lower bound of the specified dimension in the <see cref="T:System.Array" />.</returns>
		/// <param name="dimension">A zero-based dimension of the <see cref="T:System.Array" /> whose lower bound needs to be determined.</param>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="dimension" /> is less than zero.-or-<paramref name="dimension" /> is equal to or greater than <see cref="P:System.Array.Rank" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetLowerBound(int dimension);

		/// <summary>Gets the value at the specified position in the multidimensional <see cref="T:System.Array" />. The indexes are specified as an array of 32-bit integers.</summary>
		/// <returns>The value at the specified position in the multidimensional <see cref="T:System.Array" />.</returns>
		/// <param name="indices">A one-dimensional array of 32-bit integers that represent the indexes specifying the position of the <see cref="T:System.Array" /> element to get.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="indices" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The number of dimensions in the current <see cref="T:System.Array" /> is not equal to the number of elements in <paramref name="indices" />.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">Any element in <paramref name="indices" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>2</filterpriority>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object GetValue(params int[] indices);

		/// <summary>Sets a value to the element at the specified position in the multidimensional <see cref="T:System.Array" />. The indexes are specified as an array of 32-bit integers.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="indices">A one-dimensional array of 32-bit integers that represent the indexes specifying the position of the element to set.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="indices" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The number of dimensions in the current <see cref="T:System.Array" /> is not equal to the number of elements in <paramref name="indices" />.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">Any element in <paramref name="indices" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>1</filterpriority>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetValue(object value, params int[] indices);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object GetValueImpl(int pos);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetValueImpl(object value, int pos);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool FastCopy(Array source, int source_idx, Array dest, int dest_idx, int length);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Array CreateInstanceImpl(Type elementType, int[] lengths, int[] bounds);

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Array" /> is synchronized (thread safe).</summary>
		/// <returns>This property is always false for all arrays.</returns>
		/// <filterpriority>2</filterpriority>
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Array" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Array" />.</returns>
		/// <filterpriority>2</filterpriority>
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Array" /> has a fixed size.</summary>
		/// <returns>This property is always true for all arrays.</returns>
		/// <filterpriority>2</filterpriority>
		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Array" /> is read-only.</summary>
		/// <returns>This property is always false for all arrays.</returns>
		/// <filterpriority>2</filterpriority>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Array" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Array" />.</returns>
		/// <filterpriority>2</filterpriority>
		public IEnumerator GetEnumerator()
		{
			return new Array.SimpleEnumerator(this);
		}

		/// <summary>Gets the upper bound of the specified dimension in the <see cref="T:System.Array" />.</summary>
		/// <returns>The upper bound of the specified dimension in the <see cref="T:System.Array" />.</returns>
		/// <param name="dimension">A zero-based dimension of the <see cref="T:System.Array" /> whose upper bound needs to be determined.</param>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="dimension" /> is less than zero.-or-<paramref name="dimension" /> is equal to or greater than <see cref="P:System.Array.Rank" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public int GetUpperBound(int dimension)
		{
			return this.GetLowerBound(dimension) + this.GetLength(dimension) - 1;
		}

		/// <summary>Gets the value at the specified position in the one-dimensional <see cref="T:System.Array" />. The index is specified as a 32-bit integer.</summary>
		/// <returns>The value at the specified position in the one-dimensional <see cref="T:System.Array" />.</returns>
		/// <param name="index">A 32-bit integer that represents the position of the <see cref="T:System.Array" /> element to get.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly one dimension.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>2</filterpriority>
		public object GetValue(int index)
		{
			if (this.Rank != 1)
			{
				throw new ArgumentException(Locale.GetText("Array was not a one-dimensional array."));
			}
			if (index < this.GetLowerBound(0) || index > this.GetUpperBound(0))
			{
				throw new IndexOutOfRangeException(Locale.GetText("Index has to be between upper and lower bound of the array."));
			}
			return this.GetValueImpl(index - this.GetLowerBound(0));
		}

		/// <summary>Gets the value at the specified position in the two-dimensional <see cref="T:System.Array" />. The indexes are specified as 32-bit integers.</summary>
		/// <returns>The value at the specified position in the two-dimensional <see cref="T:System.Array" />.</returns>
		/// <param name="index1">A 32-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <param name="index2">A 32-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly two dimensions.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">Either <paramref name="index1" /> or <paramref name="index2" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>2</filterpriority>
		public object GetValue(int index1, int index2)
		{
			int[] indices = new int[]
			{
				index1,
				index2
			};
			return this.GetValue(indices);
		}

		/// <summary>Gets the value at the specified position in the three-dimensional <see cref="T:System.Array" />. The indexes are specified as 32-bit integers.</summary>
		/// <returns>The value at the specified position in the three-dimensional <see cref="T:System.Array" />.</returns>
		/// <param name="index1">A 32-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <param name="index2">A 32-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <param name="index3">A 32-bit integer that represents the third-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly three dimensions.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="index1" /> or <paramref name="index2" /> or <paramref name="index3" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>2</filterpriority>
		public object GetValue(int index1, int index2, int index3)
		{
			int[] indices = new int[]
			{
				index1,
				index2,
				index3
			};
			return this.GetValue(indices);
		}

		/// <summary>Gets the value at the specified position in the one-dimensional <see cref="T:System.Array" />. The index is specified as a 64-bit integer.</summary>
		/// <returns>The value at the specified position in the one-dimensional <see cref="T:System.Array" />.</returns>
		/// <param name="index">A 64-bit integer that represents the position of the <see cref="T:System.Array" /> element to get.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly one dimension.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public object GetValue(long index)
		{
			if (index < 0L || index > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("index", Locale.GetText("Value must be >= 0 and <= Int32.MaxValue."));
			}
			return this.GetValue((int)index);
		}

		/// <summary>Gets the value at the specified position in the two-dimensional <see cref="T:System.Array" />. The indexes are specified as 64-bit integers.</summary>
		/// <returns>The value at the specified position in the two-dimensional <see cref="T:System.Array" />.</returns>
		/// <param name="index1">A 64-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <param name="index2">A 64-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly two dimensions.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Either <paramref name="index1" /> or <paramref name="index2" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public object GetValue(long index1, long index2)
		{
			if (index1 < 0L || index1 > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("index1", Locale.GetText("Value must be >= 0 and <= Int32.MaxValue."));
			}
			if (index2 < 0L || index2 > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("index2", Locale.GetText("Value must be >= 0 and <= Int32.MaxValue."));
			}
			return this.GetValue((int)index1, (int)index2);
		}

		/// <summary>Gets the value at the specified position in the three-dimensional <see cref="T:System.Array" />. The indexes are specified as 64-bit integers.</summary>
		/// <returns>The value at the specified position in the three-dimensional <see cref="T:System.Array" />.</returns>
		/// <param name="index1">A 64-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <param name="index2">A 64-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <param name="index3">A 64-bit integer that represents the third-dimension index of the <see cref="T:System.Array" /> element to get.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly three dimensions.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index1" /> or <paramref name="index2" /> or <paramref name="index3" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public object GetValue(long index1, long index2, long index3)
		{
			if (index1 < 0L || index1 > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("index1", Locale.GetText("Value must be >= 0 and <= Int32.MaxValue."));
			}
			if (index2 < 0L || index2 > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("index2", Locale.GetText("Value must be >= 0 and <= Int32.MaxValue."));
			}
			if (index3 < 0L || index3 > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("index3", Locale.GetText("Value must be >= 0 and <= Int32.MaxValue."));
			}
			return this.GetValue((int)index1, (int)index2, (int)index3);
		}

		/// <summary>Sets a value to the element at the specified position in the one-dimensional <see cref="T:System.Array" />. The index is specified as a 64-bit integer.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="index">A 64-bit integer that represents the position of the <see cref="T:System.Array" /> element to set.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly one dimension.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		public void SetValue(object value, long index)
		{
			if (index < 0L || index > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("index", Locale.GetText("Value must be >= 0 and <= Int32.MaxValue."));
			}
			this.SetValue(value, (int)index);
		}

		/// <summary>Sets a value to the element at the specified position in the two-dimensional <see cref="T:System.Array" />. The indexes are specified as 64-bit integers.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="index1">A 64-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <param name="index2">A 64-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly two dimensions.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Either <paramref name="index1" /> or <paramref name="index2" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		public void SetValue(object value, long index1, long index2)
		{
			if (index1 < 0L || index1 > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("index1", Locale.GetText("Value must be >= 0 and <= Int32.MaxValue."));
			}
			if (index2 < 0L || index2 > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("index2", Locale.GetText("Value must be >= 0 and <= Int32.MaxValue."));
			}
			int[] indices = new int[]
			{
				(int)index1,
				(int)index2
			};
			this.SetValue(value, indices);
		}

		/// <summary>Sets a value to the element at the specified position in the three-dimensional <see cref="T:System.Array" />. The indexes are specified as 64-bit integers.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="index1">A 64-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <param name="index2">A 64-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <param name="index3">A 64-bit integer that represents the third-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly three dimensions.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index1" /> or <paramref name="index2" /> or <paramref name="index3" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		public void SetValue(object value, long index1, long index2, long index3)
		{
			if (index1 < 0L || index1 > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("index1", Locale.GetText("Value must be >= 0 and <= Int32.MaxValue."));
			}
			if (index2 < 0L || index2 > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("index2", Locale.GetText("Value must be >= 0 and <= Int32.MaxValue."));
			}
			if (index3 < 0L || index3 > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("index3", Locale.GetText("Value must be >= 0 and <= Int32.MaxValue."));
			}
			int[] indices = new int[]
			{
				(int)index1,
				(int)index2,
				(int)index3
			};
			this.SetValue(value, indices);
		}

		/// <summary>Sets a value to the element at the specified position in the one-dimensional <see cref="T:System.Array" />. The index is specified as a 32-bit integer.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="index">A 32-bit integer that represents the position of the <see cref="T:System.Array" /> element to set.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly one dimension.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>1</filterpriority>
		public void SetValue(object value, int index)
		{
			if (this.Rank != 1)
			{
				throw new ArgumentException(Locale.GetText("Array was not a one-dimensional array."));
			}
			if (index < this.GetLowerBound(0) || index > this.GetUpperBound(0))
			{
				throw new IndexOutOfRangeException(Locale.GetText("Index has to be >= lower bound and <= upper bound of the array."));
			}
			this.SetValueImpl(value, index - this.GetLowerBound(0));
		}

		/// <summary>Sets a value to the element at the specified position in the two-dimensional <see cref="T:System.Array" />. The indexes are specified as 32-bit integers.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="index1">A 32-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <param name="index2">A 32-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly two dimensions.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">Either <paramref name="index1" /> or <paramref name="index2" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>1</filterpriority>
		public void SetValue(object value, int index1, int index2)
		{
			int[] indices = new int[]
			{
				index1,
				index2
			};
			this.SetValue(value, indices);
		}

		/// <summary>Sets a value to the element at the specified position in the three-dimensional <see cref="T:System.Array" />. The indexes are specified as 32-bit integers.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="index1">A 32-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <param name="index2">A 32-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <param name="index3">A 32-bit integer that represents the third-dimension index of the <see cref="T:System.Array" /> element to set.</param>
		/// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly three dimensions.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="index1" /> or <paramref name="index2" /> or <paramref name="index3" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>1</filterpriority>
		public void SetValue(object value, int index1, int index2, int index3)
		{
			int[] indices = new int[]
			{
				index1,
				index2,
				index3
			};
			this.SetValue(value, indices);
		}

		/// <summary>Creates a one-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and length, with zero-based indexing.</summary>
		/// <returns>A new one-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length, using zero-based indexing.</returns>
		/// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="length">The size of the <see cref="T:System.Array" /> to create.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="elementType" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported.-or-<paramref name="elementType" /> is an open generic type.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="length" /> is less than zero.</exception>
		/// <filterpriority>1</filterpriority>
		public static Array CreateInstance(Type elementType, int length)
		{
			int[] lengths = new int[]
			{
				length
			};
			return Array.CreateInstance(elementType, lengths);
		}

		/// <summary>Creates a two-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and dimension lengths, with zero-based indexing.</summary>
		/// <returns>A new two-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length for each dimension, using zero-based indexing.</returns>
		/// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="length1">The size of the first dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="length2">The size of the second dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="elementType" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported. -or-<paramref name="elementType" /> is an open generic type.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="length1" /> is less than zero.-or-<paramref name="length2" /> is less than zero.</exception>
		/// <filterpriority>1</filterpriority>
		public static Array CreateInstance(Type elementType, int length1, int length2)
		{
			int[] lengths = new int[]
			{
				length1,
				length2
			};
			return Array.CreateInstance(elementType, lengths);
		}

		/// <summary>Creates a three-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and dimension lengths, with zero-based indexing.</summary>
		/// <returns>A new three-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length for each dimension, using zero-based indexing.</returns>
		/// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="length1">The size of the first dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="length2">The size of the second dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="length3">The size of the third dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="elementType" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported. -or-<paramref name="elementType" /> is an open generic type.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="length1" /> is less than zero.-or-<paramref name="length2" /> is less than zero.-or-<paramref name="length3" /> is less than zero.</exception>
		/// <filterpriority>1</filterpriority>
		public static Array CreateInstance(Type elementType, int length1, int length2, int length3)
		{
			int[] lengths = new int[]
			{
				length1,
				length2,
				length3
			};
			return Array.CreateInstance(elementType, lengths);
		}

		/// <summary>Creates a multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and dimension lengths, with zero-based indexing. The dimension lengths are specified in an array of 32-bit integers.</summary>
		/// <returns>A new multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length for each dimension, using zero-based indexing.</returns>
		/// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="lengths">An array of 32-bit integers that represent the size of each dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="elementType" /> is null.-or-<paramref name="lengths" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.-or-The <paramref name="lengths" /> array contains less than one element.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported. -or-<paramref name="elementType" /> is an open generic type.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Any value in <paramref name="lengths" /> is less than zero.</exception>
		/// <filterpriority>1</filterpriority>
		public static Array CreateInstance(Type elementType, params int[] lengths)
		{
			if (elementType == null)
			{
				throw new ArgumentNullException("elementType");
			}
			if (lengths == null)
			{
				throw new ArgumentNullException("lengths");
			}
			if (lengths.Length > 255)
			{
				throw new TypeLoadException();
			}
			int[] bounds = null;
			elementType = elementType.UnderlyingSystemType;
			if (!elementType.IsSystemType)
			{
				throw new ArgumentException("Type must be a type provided by the runtime.", "elementType");
			}
			if (elementType.Equals(typeof(void)))
			{
				throw new NotSupportedException("Array type can not be void");
			}
			if (elementType.ContainsGenericParameters)
			{
				throw new NotSupportedException("Array type can not be an open generic type");
			}
			return Array.CreateInstanceImpl(elementType, lengths, bounds);
		}

		/// <summary>Creates a multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and dimension lengths, with the specified lower bounds.</summary>
		/// <returns>A new multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length and lower bound for each dimension.</returns>
		/// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="lengths">A one-dimensional array that contains the size of each dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="lowerBounds">A one-dimensional array that contains the lower bound (starting index) of each dimension of the <see cref="T:System.Array" /> to create.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="elementType" /> is null.-or-<paramref name="lengths" /> is null.-or-<paramref name="lowerBounds" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.-or-The <paramref name="lengths" /> array contains less than one element.-or-The <paramref name="lengths" /> and <paramref name="lowerBounds" /> arrays do not contain the same number of elements.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported. -or-<paramref name="elementType" /> is an open generic type.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Any value in <paramref name="lengths" /> is less than zero.-or-Any value in <paramref name="lowerBounds" /> is very large, such that the sum of a dimension's lower bound and length is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static Array CreateInstance(Type elementType, int[] lengths, int[] lowerBounds)
		{
			if (elementType == null)
			{
				throw new ArgumentNullException("elementType");
			}
			if (lengths == null)
			{
				throw new ArgumentNullException("lengths");
			}
			if (lowerBounds == null)
			{
				throw new ArgumentNullException("lowerBounds");
			}
			elementType = elementType.UnderlyingSystemType;
			if (!elementType.IsSystemType)
			{
				throw new ArgumentException("Type must be a type provided by the runtime.", "elementType");
			}
			if (elementType.Equals(typeof(void)))
			{
				throw new NotSupportedException("Array type can not be void");
			}
			if (elementType.ContainsGenericParameters)
			{
				throw new NotSupportedException("Array type can not be an open generic type");
			}
			if (lengths.Length < 1)
			{
				throw new ArgumentException(Locale.GetText("Arrays must contain >= 1 elements."));
			}
			if (lengths.Length != lowerBounds.Length)
			{
				throw new ArgumentException(Locale.GetText("Arrays must be of same size."));
			}
			for (int i = 0; i < lowerBounds.Length; i++)
			{
				if (lengths[i] < 0)
				{
					throw new ArgumentOutOfRangeException("lengths", Locale.GetText("Each value has to be >= 0."));
				}
				if ((long)lowerBounds[i] + (long)lengths[i] > 2147483647L)
				{
					throw new ArgumentOutOfRangeException("lengths", Locale.GetText("Length + bound must not exceed Int32.MaxValue."));
				}
			}
			if (lengths.Length > 255)
			{
				throw new TypeLoadException();
			}
			return Array.CreateInstanceImpl(elementType, lengths, lowerBounds);
		}

		private static int[] GetIntArray(long[] values)
		{
			int num = values.Length;
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				long num2 = values[i];
				if (num2 < 0L || num2 > 2147483647L)
				{
					throw new ArgumentOutOfRangeException("values", Locale.GetText("Each value has to be >= 0 and <= Int32.MaxValue."));
				}
				array[i] = (int)num2;
			}
			return array;
		}

		/// <summary>Creates a multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and dimension lengths, with zero-based indexing. The dimension lengths are specified in an array of 64-bit integers.</summary>
		/// <returns>A new multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length for each dimension, using zero-based indexing.</returns>
		/// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
		/// <param name="lengths">An array of 64-bit integers that represent the size of each dimension of the <see cref="T:System.Array" /> to create. Each integer in the array must be between zero and <see cref="F:System.Int32.MaxValue" />, inclusive.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="elementType" /> is null.-or-<paramref name="lengths" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.-or-The <paramref name="lengths" /> array contains less than one element.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported. -or-<paramref name="elementType" /> is an open generic type.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Any value in <paramref name="lengths" /> is less than zero or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static Array CreateInstance(Type elementType, params long[] lengths)
		{
			if (lengths == null)
			{
				throw new ArgumentNullException("lengths");
			}
			return Array.CreateInstance(elementType, Array.GetIntArray(lengths));
		}

		/// <summary>Gets the value at the specified position in the multidimensional <see cref="T:System.Array" />. The indexes are specified as an array of 64-bit integers.</summary>
		/// <returns>The value at the specified position in the multidimensional <see cref="T:System.Array" />.</returns>
		/// <param name="indices">A one-dimensional array of 64-bit integers that represent the indexes specifying the position of the <see cref="T:System.Array" /> element to get.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="indices" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The number of dimensions in the current <see cref="T:System.Array" /> is not equal to the number of elements in <paramref name="indices" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Any element in <paramref name="indices" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public object GetValue(params long[] indices)
		{
			if (indices == null)
			{
				throw new ArgumentNullException("indices");
			}
			return this.GetValue(Array.GetIntArray(indices));
		}

		/// <summary>Sets a value to the element at the specified position in the multidimensional <see cref="T:System.Array" />. The indexes are specified as an array of 64-bit integers.</summary>
		/// <param name="value">The new value for the specified element.</param>
		/// <param name="indices">A one-dimensional array of 64-bit integers that represent the indexes specifying the position of the element to set.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="indices" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The number of dimensions in the current <see cref="T:System.Array" /> is not equal to the number of elements in <paramref name="indices" />.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Any element in <paramref name="indices" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		public void SetValue(object value, params long[] indices)
		{
			if (indices == null)
			{
				throw new ArgumentNullException("indices");
			}
			this.SetValue(value, Array.GetIntArray(indices));
		}

		/// <summary>Searches an entire one-dimensional sorted <see cref="T:System.Array" /> for a specific element, using the <see cref="T:System.IComparable" /> interface implemented by each element of the <see cref="T:System.Array" /> and by the specified object.</summary>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, a negative number which is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than any of the elements in <paramref name="array" />, a negative number which is the bitwise complement of (the index of the last element plus 1).</returns>
		/// <param name="array">The sorted one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to search for.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IComparable" /> interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable" /> interface.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int BinarySearch(Array array, object value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (value == null)
			{
				return -1;
			}
			if (array.Rank > 1)
			{
				throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
			}
			if (array.Length == 0)
			{
				return -1;
			}
			if (!(value is IComparable))
			{
				throw new ArgumentException(Locale.GetText("value does not support IComparable."));
			}
			return Array.DoBinarySearch(array, array.GetLowerBound(0), array.GetLength(0), value, null);
		}

		/// <summary>Searches an entire one-dimensional sorted <see cref="T:System.Array" /> for a value using the specified <see cref="T:System.Collections.IComparer" /> interface.</summary>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, a negative number which is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than any of the elements in <paramref name="array" />, a negative number which is the bitwise complement of (the index of the last element plus 1).</returns>
		/// <param name="array">The sorted one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to search for.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.-or- null to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparer" /> is null, and <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is null, <paramref name="value" /> does not implement the <see cref="T:System.IComparable" /> interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable" /> interface.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int BinarySearch(Array array, object value, IComparer comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank > 1)
			{
				throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
			}
			if (array.Length == 0)
			{
				return -1;
			}
			if (comparer == null && value != null && !(value is IComparable))
			{
				throw new ArgumentException(Locale.GetText("comparer is null and value does not support IComparable."));
			}
			return Array.DoBinarySearch(array, array.GetLowerBound(0), array.GetLength(0), value, comparer);
		}

		/// <summary>Searches a range of elements in a one-dimensional sorted <see cref="T:System.Array" /> for a value, using the <see cref="T:System.IComparable" /> interface implemented by each element of the <see cref="T:System.Array" /> and by the specified value.</summary>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, a negative number which is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than any of the elements in <paramref name="array" />, a negative number which is the bitwise complement of (the index of the last element plus 1).</returns>
		/// <param name="array">The sorted one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="index">The starting index of the range to search.</param>
		/// <param name="length">The length of the range to search.</param>
		/// <param name="value">The object to search for.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.-or-<paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.-or-<paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IComparable" /> interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable" /> interface.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int BinarySearch(Array array, int index, int length, object value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank > 1)
			{
				throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
			}
			if (index < array.GetLowerBound(0))
			{
				throw new ArgumentOutOfRangeException("index", Locale.GetText("index is less than the lower bound of array."));
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Locale.GetText("Value has to be >= 0."));
			}
			if (index > array.GetLowerBound(0) + array.GetLength(0) - length)
			{
				throw new ArgumentException(Locale.GetText("index and length do not specify a valid range in array."));
			}
			if (array.Length == 0)
			{
				return -1;
			}
			if (value != null && !(value is IComparable))
			{
				throw new ArgumentException(Locale.GetText("value does not support IComparable"));
			}
			return Array.DoBinarySearch(array, index, length, value, null);
		}

		/// <summary>Searches a range of elements in a one-dimensional sorted <see cref="T:System.Array" /> for a value, using the specified <see cref="T:System.Collections.IComparer" /> interface.</summary>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, a negative number which is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than any of the elements in <paramref name="array" />, a negative number which is the bitwise complement of (the index of the last element plus 1).</returns>
		/// <param name="array">The sorted one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="index">The starting index of the range to search.</param>
		/// <param name="length">The length of the range to search.</param>
		/// <param name="value">The object to search for.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.-or- null to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.-or-<paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.-or-<paramref name="comparer" /> is null, and <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is null, <paramref name="value" /> does not implement the <see cref="T:System.IComparable" /> interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable" /> interface.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int BinarySearch(Array array, int index, int length, object value, IComparer comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank > 1)
			{
				throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
			}
			if (index < array.GetLowerBound(0))
			{
				throw new ArgumentOutOfRangeException("index", Locale.GetText("index is less than the lower bound of array."));
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Locale.GetText("Value has to be >= 0."));
			}
			if (index > array.GetLowerBound(0) + array.GetLength(0) - length)
			{
				throw new ArgumentException(Locale.GetText("index and length do not specify a valid range in array."));
			}
			if (array.Length == 0)
			{
				return -1;
			}
			if (comparer == null && value != null && !(value is IComparable))
			{
				throw new ArgumentException(Locale.GetText("comparer is null and value does not support IComparable."));
			}
			return Array.DoBinarySearch(array, index, length, value, comparer);
		}

		private static int DoBinarySearch(Array array, int index, int length, object value, IComparer comparer)
		{
			if (comparer == null)
			{
				comparer = Comparer.Default;
			}
			int i = index;
			int num = index + length - 1;
			try
			{
				while (i <= num)
				{
					int num2 = i + (num - i) / 2;
					object valueImpl = array.GetValueImpl(num2);
					int num3 = comparer.Compare(valueImpl, value);
					if (num3 == 0)
					{
						return num2;
					}
					if (num3 > 0)
					{
						num = num2 - 1;
					}
					else
					{
						i = num2 + 1;
					}
				}
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException(Locale.GetText("Comparer threw an exception."), innerException);
			}
			return ~i;
		}

		/// <summary>Sets a range of elements in the <see cref="T:System.Array" /> to zero, to false, or to null, depending on the element type.</summary>
		/// <param name="array">The <see cref="T:System.Array" /> whose elements need to be cleared.</param>
		/// <param name="index">The starting index of the range of elements to clear.</param>
		/// <param name="length">The number of elements to clear.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.-or-<paramref name="length" /> is less than zero.-or-The sum of <paramref name="index" /> and <paramref name="length" /> is greater than the size of the <see cref="T:System.Array" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void Clear(Array array, int index, int length)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (length < 0)
			{
				throw new IndexOutOfRangeException("length < 0");
			}
			int lowerBound = array.GetLowerBound(0);
			if (index < lowerBound)
			{
				throw new IndexOutOfRangeException("index < lower bound");
			}
			index -= lowerBound;
			if (index > array.Length - length)
			{
				throw new IndexOutOfRangeException("index + length > size");
			}
			Array.ClearInternal(array, index, length);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearInternal(Array a, int index, int count);

		/// <summary>Creates a shallow copy of the <see cref="T:System.Array" />.</summary>
		/// <returns>A shallow copy of the <see cref="T:System.Array" />.</returns>
		/// <filterpriority>1</filterpriority>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern object Clone();

		/// <summary>Copies a range of elements from an <see cref="T:System.Array" /> starting at the first element and pastes them into another <see cref="T:System.Array" /> starting at the first element. The length is specified as a 32-bit integer.</summary>
		/// <param name="sourceArray">The <see cref="T:System.Array" /> that contains the data to copy.</param>
		/// <param name="destinationArray">The <see cref="T:System.Array" /> that receives the data.</param>
		/// <param name="length">A 32-bit integer that represents the number of elements to copy.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceArray" /> is null.-or-<paramref name="destinationArray" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> have different ranks.</exception>
		/// <exception cref="T:System.ArrayTypeMismatchException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> are of incompatible types.</exception>
		/// <exception cref="T:System.InvalidCastException">At least one element in <paramref name="sourceArray" /> cannot be cast to the type of <paramref name="destinationArray" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="length" /> is greater than the number of elements in <paramref name="sourceArray" />.-or-<paramref name="length" /> is greater than the number of elements in <paramref name="destinationArray" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Copy(Array sourceArray, Array destinationArray, int length)
		{
			if (sourceArray == null)
			{
				throw new ArgumentNullException("sourceArray");
			}
			if (destinationArray == null)
			{
				throw new ArgumentNullException("destinationArray");
			}
			Array.Copy(sourceArray, sourceArray.GetLowerBound(0), destinationArray, destinationArray.GetLowerBound(0), length);
		}

		/// <summary>Copies a range of elements from an <see cref="T:System.Array" /> starting at the specified source index and pastes them to another <see cref="T:System.Array" /> starting at the specified destination index. The length and the indexes are specified as 32-bit integers.</summary>
		/// <param name="sourceArray">The <see cref="T:System.Array" /> that contains the data to copy.</param>
		/// <param name="sourceIndex">A 32-bit integer that represents the index in the <paramref name="sourceArray" /> at which copying begins.</param>
		/// <param name="destinationArray">The <see cref="T:System.Array" /> that receives the data.</param>
		/// <param name="destinationIndex">A 32-bit integer that represents the index in the <paramref name="destinationArray" /> at which storing begins.</param>
		/// <param name="length">A 32-bit integer that represents the number of elements to copy.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceArray" /> is null.-or-<paramref name="destinationArray" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> have different ranks.</exception>
		/// <exception cref="T:System.ArrayTypeMismatchException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> are of incompatible types.</exception>
		/// <exception cref="T:System.InvalidCastException">At least one element in <paramref name="sourceArray" /> cannot be cast to the type of <paramref name="destinationArray" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="sourceIndex" /> is less than the lower bound of the first dimension of <paramref name="sourceArray" />.-or-<paramref name="destinationIndex" /> is less than the lower bound of the first dimension of <paramref name="destinationArray" />.-or-<paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="length" /> is greater than the number of elements from <paramref name="sourceIndex" /> to the end of <paramref name="sourceArray" />.-or-<paramref name="length" /> is greater than the number of elements from <paramref name="destinationIndex" /> to the end of <paramref name="destinationArray" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
		{
			if (sourceArray == null)
			{
				throw new ArgumentNullException("sourceArray");
			}
			if (destinationArray == null)
			{
				throw new ArgumentNullException("destinationArray");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Locale.GetText("Value has to be >= 0."));
			}
			if (sourceIndex < 0)
			{
				throw new ArgumentOutOfRangeException("sourceIndex", Locale.GetText("Value has to be >= 0."));
			}
			if (destinationIndex < 0)
			{
				throw new ArgumentOutOfRangeException("destinationIndex", Locale.GetText("Value has to be >= 0."));
			}
			if (Array.FastCopy(sourceArray, sourceIndex, destinationArray, destinationIndex, length))
			{
				return;
			}
			int num = sourceIndex - sourceArray.GetLowerBound(0);
			int num2 = destinationIndex - destinationArray.GetLowerBound(0);
			if (num > sourceArray.Length - length)
			{
				throw new ArgumentException("length");
			}
			if (num2 > destinationArray.Length - length)
			{
				string message = "Destination array was not long enough. Check destIndex and length, and the array's lower bounds";
				throw new ArgumentException(message, string.Empty);
			}
			if (sourceArray.Rank != destinationArray.Rank)
			{
				throw new RankException(Locale.GetText("Arrays must be of same size."));
			}
			Type elementType = sourceArray.GetType().GetElementType();
			Type elementType2 = destinationArray.GetType().GetElementType();
			if (!object.ReferenceEquals(sourceArray, destinationArray) || num > num2)
			{
				for (int i = 0; i < length; i++)
				{
					object valueImpl = sourceArray.GetValueImpl(num + i);
					try
					{
						destinationArray.SetValueImpl(valueImpl, num2 + i);
					}
					catch
					{
						if (elementType.Equals(typeof(object)))
						{
							throw new InvalidCastException();
						}
						throw new ArrayTypeMismatchException(string.Format(Locale.GetText("(Types: source={0};  target={1})"), elementType.FullName, elementType2.FullName));
					}
				}
			}
			else
			{
				for (int j = length - 1; j >= 0; j--)
				{
					object valueImpl2 = sourceArray.GetValueImpl(num + j);
					try
					{
						destinationArray.SetValueImpl(valueImpl2, num2 + j);
					}
					catch
					{
						if (elementType.Equals(typeof(object)))
						{
							throw new InvalidCastException();
						}
						throw new ArrayTypeMismatchException(string.Format(Locale.GetText("(Types: source={0};  target={1})"), elementType.FullName, elementType2.FullName));
					}
				}
			}
		}

		/// <summary>Copies a range of elements from an <see cref="T:System.Array" /> starting at the specified source index and pastes them to another <see cref="T:System.Array" /> starting at the specified destination index. The length and the indexes are specified as 64-bit integers.</summary>
		/// <param name="sourceArray">The <see cref="T:System.Array" /> that contains the data to copy.</param>
		/// <param name="sourceIndex">A 64-bit integer that represents the index in the <paramref name="sourceArray" /> at which copying begins.</param>
		/// <param name="destinationArray">The <see cref="T:System.Array" /> that receives the data.</param>
		/// <param name="destinationIndex">A 64-bit integer that represents the index in the <paramref name="destinationArray" /> at which storing begins.</param>
		/// <param name="length">A 64-bit integer that represents the number of elements to copy. The integer must be between zero and <see cref="F:System.Int32.MaxValue" />, inclusive.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceArray" /> is null.-or-<paramref name="destinationArray" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> have different ranks.</exception>
		/// <exception cref="T:System.ArrayTypeMismatchException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> are of incompatible types.</exception>
		/// <exception cref="T:System.InvalidCastException">At least one element in <paramref name="sourceArray" /> cannot be cast to the type of <paramref name="destinationArray" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="sourceIndex" /> is outside the range of valid indexes for the <paramref name="sourceArray" />.-or-<paramref name="destinationIndex" /> is outside the range of valid indexes for the <paramref name="destinationArray" />.-or-<paramref name="length" /> is less than 0 or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="length" /> is greater than the number of elements from <paramref name="sourceIndex" /> to the end of <paramref name="sourceArray" />.-or-<paramref name="length" /> is greater than the number of elements from <paramref name="destinationIndex" /> to the end of <paramref name="destinationArray" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Copy(Array sourceArray, long sourceIndex, Array destinationArray, long destinationIndex, long length)
		{
			if (sourceArray == null)
			{
				throw new ArgumentNullException("sourceArray");
			}
			if (destinationArray == null)
			{
				throw new ArgumentNullException("destinationArray");
			}
			if (sourceIndex < -2147483648L || sourceIndex > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("sourceIndex", Locale.GetText("Must be in the Int32 range."));
			}
			if (destinationIndex < -2147483648L || destinationIndex > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("destinationIndex", Locale.GetText("Must be in the Int32 range."));
			}
			if (length < 0L || length > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("length", Locale.GetText("Value must be >= 0 and <= Int32.MaxValue."));
			}
			Array.Copy(sourceArray, (int)sourceIndex, destinationArray, (int)destinationIndex, (int)length);
		}

		/// <summary>Copies a range of elements from an <see cref="T:System.Array" /> starting at the first element and pastes them into another <see cref="T:System.Array" /> starting at the first element. The length is specified as a 64-bit integer.</summary>
		/// <param name="sourceArray">The <see cref="T:System.Array" /> that contains the data to copy.</param>
		/// <param name="destinationArray">The <see cref="T:System.Array" /> that receives the data.</param>
		/// <param name="length">A 64-bit integer that represents the number of elements to copy. The integer must be between zero and <see cref="F:System.Int32.MaxValue" />, inclusive.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceArray" /> is null.-or-<paramref name="destinationArray" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> have different ranks.</exception>
		/// <exception cref="T:System.ArrayTypeMismatchException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> are of incompatible types.</exception>
		/// <exception cref="T:System.InvalidCastException">At least one element in <paramref name="sourceArray" /> cannot be cast to the type of <paramref name="destinationArray" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="length" /> is less than 0 or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="length" /> is greater than the number of elements in <paramref name="sourceArray" />.-or-<paramref name="length" /> is greater than the number of elements in <paramref name="destinationArray" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Copy(Array sourceArray, Array destinationArray, long length)
		{
			if (length < 0L || length > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("length", Locale.GetText("Value must be >= 0 and <= Int32.MaxValue."));
			}
			Array.Copy(sourceArray, destinationArray, (int)length);
		}

		/// <summary>Searches for the specified object and returns the index of the first occurrence within the entire one-dimensional <see cref="T:System.Array" />.</summary>
		/// <returns>The index of the first occurrence of <paramref name="value" /> within the entire <paramref name="array" />, if found; otherwise, the lower bound of the array minus 1.</returns>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int IndexOf(Array array, object value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.IndexOf(array, value, 0, array.Length);
		}

		/// <summary>Searches for the specified object and returns the index of the first occurrence within the range of elements in the one-dimensional <see cref="T:System.Array" /> that extends from the specified index to the last element.</summary>
		/// <returns>The index of the first occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that extends from <paramref name="startIndex" /> to the last element, if found; otherwise, the lower bound of the array minus 1.</returns>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The starting index of the search. 0 (zero) is valid in an empty array.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int IndexOf(Array array, object value, int startIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.IndexOf(array, value, startIndex, array.Length - startIndex);
		}

		/// <summary>Searches for the specified object and returns the index of the first occurrence within the range of elements in the one-dimensional <see cref="T:System.Array" /> that starts at the specified index and contains the specified number of elements.</summary>
		/// <returns>The index of the first occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that starts at <paramref name="startIndex" /> and contains the number of elements specified in <paramref name="count" />, if found; otherwise, the lower bound of the array minus 1.</returns>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The starting index of the search. 0 (zero) is valid in an empty array.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.-or-<paramref name="count" /> is less than zero.-or-<paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int IndexOf(Array array, object value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank > 1)
			{
				throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
			}
			if (count < 0 || startIndex < array.GetLowerBound(0) || startIndex - 1 > array.GetUpperBound(0) - count)
			{
				throw new ArgumentOutOfRangeException();
			}
			int num = startIndex + count;
			for (int i = startIndex; i < num; i++)
			{
				if (object.Equals(array.GetValueImpl(i), value))
				{
					return i;
				}
			}
			return array.GetLowerBound(0) - 1;
		}

		/// <summary>Initializes every element of the value-type <see cref="T:System.Array" /> by calling the default constructor of the value type.</summary>
		/// <filterpriority>2</filterpriority>
		public void Initialize()
		{
		}

		/// <summary>Searches for the specified object and returns the index of the last occurrence within the entire one-dimensional <see cref="T:System.Array" />.</summary>
		/// <returns>The index of the last occurrence of <paramref name="value" /> within the entire <paramref name="array" />, if found; otherwise, the lower bound of the array minus 1.</returns>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int LastIndexOf(Array array, object value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Length == 0)
			{
				return array.GetLowerBound(0) - 1;
			}
			return Array.LastIndexOf(array, value, array.Length - 1);
		}

		/// <summary>Searches for the specified object and returns the index of the last occurrence within the range of elements in the one-dimensional <see cref="T:System.Array" /> that extends from the first element to the specified index.</summary>
		/// <returns>The index of the last occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that extends from the first element to <paramref name="startIndex" />, if found; otherwise, the lower bound of the array minus 1.</returns>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The starting index of the backward search.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int LastIndexOf(Array array, object value, int startIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.LastIndexOf(array, value, startIndex, startIndex - array.GetLowerBound(0) + 1);
		}

		/// <summary>Searches for the specified object and returns the index of the last occurrence within the range of elements in the one-dimensional <see cref="T:System.Array" /> that contains the specified number of elements and ends at the specified index.</summary>
		/// <returns>The index of the last occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that contains the number of elements specified in <paramref name="count" /> and ends at <paramref name="startIndex" />, if found; otherwise, the lower bound of the array minus 1.</returns>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The starting index of the backward search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.-or-<paramref name="count" /> is less than zero.-or-<paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int LastIndexOf(Array array, object value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank > 1)
			{
				throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
			}
			int lowerBound = array.GetLowerBound(0);
			if (array.Length == 0)
			{
				return lowerBound - 1;
			}
			if (count < 0 || startIndex < lowerBound || startIndex > array.GetUpperBound(0) || startIndex - count + 1 < lowerBound)
			{
				throw new ArgumentOutOfRangeException();
			}
			for (int i = startIndex; i >= startIndex - count + 1; i--)
			{
				if (object.Equals(array.GetValueImpl(i), value))
				{
					return i;
				}
			}
			return lowerBound - 1;
		}

		private static Array.Swapper get_swapper(Array array)
		{
			if (array is int[])
			{
				return new Array.Swapper(array.int_swapper);
			}
			if (array is double[])
			{
				return new Array.Swapper(array.double_swapper);
			}
			if (array is object[])
			{
				return new Array.Swapper(array.obj_swapper);
			}
			return new Array.Swapper(array.slow_swapper);
		}

		private static Array.Swapper get_swapper<T>(T[] array)
		{
			if (array is int[])
			{
				return new Array.Swapper(array.int_swapper);
			}
			if (array is double[])
			{
				return new Array.Swapper(array.double_swapper);
			}
			return new Array.Swapper(array.slow_swapper);
		}

		/// <summary>Reverses the sequence of the elements in the entire one-dimensional <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to reverse.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional. </exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Reverse(Array array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Reverse(array, array.GetLowerBound(0), array.GetLength(0));
		}

		/// <summary>Reverses the sequence of the elements in a range of elements in the one-dimensional <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to reverse.</param>
		/// <param name="index">The starting index of the section to reverse.</param>
		/// <param name="length">The number of elements in the section to reverse.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.-or-<paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Reverse(Array array, int index, int length)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank > 1)
			{
				throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
			}
			if (index < array.GetLowerBound(0) || length < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (index > array.GetUpperBound(0) + 1 - length)
			{
				throw new ArgumentException();
			}
			int num = index + length - 1;
			object[] array2 = array as object[];
			if (array2 != null)
			{
				while (index < num)
				{
					object obj = array2[index];
					array2[index] = array2[num];
					array2[num] = obj;
					index++;
					num--;
				}
				return;
			}
			int[] array3 = array as int[];
			if (array3 != null)
			{
				while (index < num)
				{
					int num2 = array3[index];
					array3[index] = array3[num];
					array3[num] = num2;
					index++;
					num--;
				}
				return;
			}
			double[] array4 = array as double[];
			if (array4 != null)
			{
				while (index < num)
				{
					double num3 = array4[index];
					array4[index] = array4[num];
					array4[num] = num3;
					index++;
					num--;
				}
				return;
			}
			Array.Swapper swapper = Array.get_swapper(array);
			while (index < num)
			{
				swapper(index, num);
				index++;
				num--;
			}
		}

		/// <summary>Sorts the elements in an entire one-dimensional <see cref="T:System.Array" /> using the <see cref="T:System.IComparable" /> implementation of each element of the <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to sort.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort(Array array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Sort(array, null, array.GetLowerBound(0), array.GetLength(0), null);
		}

		/// <summary>Sorts a pair of one-dimensional <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the <see cref="T:System.IComparable" /> implementation of each key.</summary>
		/// <param name="keys">The one-dimensional <see cref="T:System.Array" /> that contains the keys to sort.</param>
		/// <param name="items">The one-dimensional <see cref="T:System.Array" /> that contains the items that correspond to each of the keys in the <paramref name="keys" /><see cref="T:System.Array" />.-or-null to sort only the <paramref name="keys" /><see cref="T:System.Array" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is null.</exception>
		/// <exception cref="T:System.RankException">The <paramref name="keys" /><see cref="T:System.Array" /> is multidimensional.-or-The <paramref name="items" /><see cref="T:System.Array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not null, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.-or-<paramref name="items" /> is not null, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort(Array keys, Array items)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			Array.Sort(keys, items, keys.GetLowerBound(0), keys.GetLength(0), null);
		}

		/// <summary>Sorts the elements in a one-dimensional <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.IComparer" />.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to sort.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.-or-null to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is null, and one or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		/// <exception cref="T:System.ArgumentException">The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort(Array array, IComparer comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Sort(array, null, array.GetLowerBound(0), array.GetLength(0), comparer);
		}

		/// <summary>Sorts the elements in a range of elements in a one-dimensional <see cref="T:System.Array" /> using the <see cref="T:System.IComparable" /> implementation of each element of the <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to sort.</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.-or-<paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort(Array array, int index, int length)
		{
			Array.Sort(array, null, index, length, null);
		}

		/// <summary>Sorts a pair of one-dimensional <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.IComparer" />.</summary>
		/// <param name="keys">The one-dimensional <see cref="T:System.Array" /> that contains the keys to sort.</param>
		/// <param name="items">The one-dimensional <see cref="T:System.Array" /> that contains the items that correspond to each of the keys in the <paramref name="keys" /><see cref="T:System.Array" />.-or-null to sort only the <paramref name="keys" /><see cref="T:System.Array" />.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.-or-null to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is null.</exception>
		/// <exception cref="T:System.RankException">The <paramref name="keys" /><see cref="T:System.Array" /> is multidimensional.-or-The <paramref name="items" /><see cref="T:System.Array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not null, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.-or-<paramref name="items" /> is not null, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />. -or-The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is null, and one or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort(Array keys, Array items, IComparer comparer)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			Array.Sort(keys, items, keys.GetLowerBound(0), keys.GetLength(0), comparer);
		}

		/// <summary>Sorts a range of elements in a pair of one-dimensional <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the <see cref="T:System.IComparable" /> implementation of each key.</summary>
		/// <param name="keys">The one-dimensional <see cref="T:System.Array" /> that contains the keys to sort.</param>
		/// <param name="items">The one-dimensional <see cref="T:System.Array" /> that contains the items that correspond to each of the keys in the <paramref name="keys" /><see cref="T:System.Array" />.-or-null to sort only the <paramref name="keys" /><see cref="T:System.Array" />.</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is null.</exception>
		/// <exception cref="T:System.RankException">The <paramref name="keys" /><see cref="T:System.Array" /> is multidimensional.-or-The <paramref name="items" /><see cref="T:System.Array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="keys" />.-or-<paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not null, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.-or-<paramref name="items" /> is not null, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.-or-<paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="keys" /><see cref="T:System.Array" />.-or-<paramref name="items" /> is not null, and <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="items" /><see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort(Array keys, Array items, int index, int length)
		{
			Array.Sort(keys, items, index, length, null);
		}

		/// <summary>Sorts the elements in a range of elements in a one-dimensional <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.IComparer" />.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> to sort.</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.-or-null to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.-or-<paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />. -or-The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is null, and one or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort(Array array, int index, int length, IComparer comparer)
		{
			Array.Sort(array, null, index, length, comparer);
		}

		/// <summary>Sorts a range of elements in a pair of one-dimensional <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.IComparer" />.</summary>
		/// <param name="keys">The one-dimensional <see cref="T:System.Array" /> that contains the keys to sort.</param>
		/// <param name="items">The one-dimensional <see cref="T:System.Array" /> that contains the items that correspond to each of the keys in the <paramref name="keys" /><see cref="T:System.Array" />.-or-null to sort only the <paramref name="keys" /><see cref="T:System.Array" />.</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.-or-null to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is null.</exception>
		/// <exception cref="T:System.RankException">The <paramref name="keys" /><see cref="T:System.Array" /> is multidimensional.-or-The <paramref name="items" /><see cref="T:System.Array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="keys" />.-or-<paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not null, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.-or-<paramref name="items" /> is not null, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.-or-<paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="keys" /><see cref="T:System.Array" />.-or-<paramref name="items" /> is not null, and <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="items" /><see cref="T:System.Array" />. -or-The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is null, and one or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort(Array keys, Array items, int index, int length, IComparer comparer)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			if (keys.Rank > 1 || (items != null && items.Rank > 1))
			{
				throw new RankException();
			}
			if (items != null && keys.GetLowerBound(0) != items.GetLowerBound(0))
			{
				throw new ArgumentException();
			}
			if (index < keys.GetLowerBound(0))
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Locale.GetText("Value has to be >= 0."));
			}
			if (keys.Length - (index + keys.GetLowerBound(0)) < length || (items != null && index > items.Length - length))
			{
				throw new ArgumentException();
			}
			if (length <= 1)
			{
				return;
			}
			if (comparer == null)
			{
				Array.Swapper swap_items;
				if (items == null)
				{
					swap_items = null;
				}
				else
				{
					swap_items = Array.get_swapper(items);
				}
				if (keys is double[])
				{
					Array.combsort(keys as double[], index, length, swap_items);
					return;
				}
				if (keys is int[])
				{
					Array.combsort(keys as int[], index, length, swap_items);
					return;
				}
				if (keys is char[])
				{
					Array.combsort(keys as char[], index, length, swap_items);
					return;
				}
			}
			try
			{
				int high = index + length - 1;
				Array.qsort(keys, items, index, high, comparer);
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException(Locale.GetText("The comparer threw an exception."), innerException);
			}
		}

		private void int_swapper(int i, int j)
		{
			int[] array = this as int[];
			int num = array[i];
			array[i] = array[j];
			array[j] = num;
		}

		private void obj_swapper(int i, int j)
		{
			object[] array = this as object[];
			object obj = array[i];
			array[i] = array[j];
			array[j] = obj;
		}

		private void slow_swapper(int i, int j)
		{
			object valueImpl = this.GetValueImpl(i);
			this.SetValueImpl(this.GetValue(j), i);
			this.SetValueImpl(valueImpl, j);
		}

		private void double_swapper(int i, int j)
		{
			double[] array = this as double[];
			double num = array[i];
			array[i] = array[j];
			array[j] = num;
		}

		private static int new_gap(int gap)
		{
			gap = gap * 10 / 13;
			if (gap == 9 || gap == 10)
			{
				return 11;
			}
			if (gap < 1)
			{
				return 1;
			}
			return gap;
		}

		private static void combsort(double[] array, int start, int size, Array.Swapper swap_items)
		{
			int num = size;
			bool flag;
			do
			{
				num = Array.new_gap(num);
				flag = false;
				int num2 = start + size - num;
				for (int i = start; i < num2; i++)
				{
					int num3 = i + num;
					if (array[i] > array[num3])
					{
						double num4 = array[i];
						array[i] = array[num3];
						array[num3] = num4;
						flag = true;
						if (swap_items != null)
						{
							swap_items(i, num3);
						}
					}
				}
			}
			while (num != 1 || flag);
		}

		private static void combsort(int[] array, int start, int size, Array.Swapper swap_items)
		{
			int num = size;
			bool flag;
			do
			{
				num = Array.new_gap(num);
				flag = false;
				int num2 = start + size - num;
				for (int i = start; i < num2; i++)
				{
					int num3 = i + num;
					if (array[i] > array[num3])
					{
						int num4 = array[i];
						array[i] = array[num3];
						array[num3] = num4;
						flag = true;
						if (swap_items != null)
						{
							swap_items(i, num3);
						}
					}
				}
			}
			while (num != 1 || flag);
		}

		private static void combsort(char[] array, int start, int size, Array.Swapper swap_items)
		{
			int num = size;
			bool flag;
			do
			{
				num = Array.new_gap(num);
				flag = false;
				int num2 = start + size - num;
				for (int i = start; i < num2; i++)
				{
					int num3 = i + num;
					if (array[i] > array[num3])
					{
						char c = array[i];
						array[i] = array[num3];
						array[num3] = c;
						flag = true;
						if (swap_items != null)
						{
							swap_items(i, num3);
						}
					}
				}
			}
			while (num != 1 || flag);
		}

		private static void qsort(Array keys, Array items, int low0, int high0, IComparer comparer)
		{
			if (low0 >= high0)
			{
				return;
			}
			int num = low0;
			int num2 = high0;
			int pos = num + (num2 - num) / 2;
			object valueImpl = keys.GetValueImpl(pos);
			for (;;)
			{
				while (num < high0 && Array.compare(keys.GetValueImpl(num), valueImpl, comparer) < 0)
				{
					num++;
				}
				while (num2 > low0 && Array.compare(valueImpl, keys.GetValueImpl(num2), comparer) < 0)
				{
					num2--;
				}
				if (num > num2)
				{
					break;
				}
				Array.swap(keys, items, num, num2);
				num++;
				num2--;
			}
			if (low0 < num2)
			{
				Array.qsort(keys, items, low0, num2, comparer);
			}
			if (num < high0)
			{
				Array.qsort(keys, items, num, high0, comparer);
			}
		}

		private static void swap(Array keys, Array items, int i, int j)
		{
			object valueImpl = keys.GetValueImpl(i);
			keys.SetValueImpl(keys.GetValue(j), i);
			keys.SetValueImpl(valueImpl, j);
			if (items != null)
			{
				valueImpl = items.GetValueImpl(i);
				items.SetValueImpl(items.GetValueImpl(j), i);
				items.SetValueImpl(valueImpl, j);
			}
		}

		private static int compare(object value1, object value2, IComparer comparer)
		{
			if (value1 == null)
			{
				return (value2 != null) ? -1 : 0;
			}
			if (value2 == null)
			{
				return 1;
			}
			if (comparer == null)
			{
				return ((IComparable)value1).CompareTo(value2);
			}
			return comparer.Compare(value1, value2);
		}

		/// <summary>Sorts the elements in an entire <see cref="T:System.Array" /> using the <see cref="T:System.IComparable`1" /> generic interface implementation of each element of the <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to sort.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort<T>(T[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Sort<T, T>(array, null, 0, array.Length, null);
		}

		/// <summary>Sorts a pair of <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the <see cref="T:System.IComparable`1" /> generic interface implementation of each key.</summary>
		/// <param name="keys">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the keys to sort. </param>
		/// <param name="items">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the items that correspond to the keys in <paramref name="keys" />, or null to sort only <paramref name="keys" />.</param>
		/// <typeparam name="TKey">The type of the elements of the key array.</typeparam>
		/// <typeparam name="TValue">The type of the elements of the items array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not null, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.-or-<paramref name="items" /> is not null, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			Array.Sort<TKey, TValue>(keys, items, 0, keys.Length, null);
		}

		/// <summary>Sorts the elements in an <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
		/// <param name="array">The one-dimensional, zero-base <see cref="T:System.Array" /> to sort</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface implementation to use when comparing elements, or null to use the <see cref="T:System.IComparable`1" /> generic interface implementation of each element.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is null, and one or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		/// <exception cref="T:System.ArgumentException">The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort<T>(T[] array, IComparer<T> comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Sort<T, T>(array, null, 0, array.Length, comparer);
		}

		/// <summary>Sorts a pair of <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
		/// <param name="keys">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the keys to sort. </param>
		/// <param name="items">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the items that correspond to the keys in <paramref name="keys" />, or null to sort only <paramref name="keys" />.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface implementation to use when comparing elements, or null to use the <see cref="T:System.IComparable`1" /> generic interface implementation of each element.</param>
		/// <typeparam name="TKey">The type of the elements of the key array.</typeparam>
		/// <typeparam name="TValue">The type of the elements of the items array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not null, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.-or-<paramref name="items" /> is not null, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.-or-The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is null, and one or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, IComparer<TKey> comparer)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			Array.Sort<TKey, TValue>(keys, items, 0, keys.Length, comparer);
		}

		/// <summary>Sorts the elements in a range of elements in an <see cref="T:System.Array" /> using the <see cref="T:System.IComparable`1" /> generic interface implementation of each element of the <see cref="T:System.Array" />.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to sort</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.-or-<paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort<T>(T[] array, int index, int length)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Sort<T, T>(array, null, index, length, null);
		}

		/// <summary>Sorts a range of elements in a pair of <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the <see cref="T:System.IComparable`1" /> generic interface implementation of each key.</summary>
		/// <param name="keys">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the keys to sort. </param>
		/// <param name="items">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the items that correspond to the keys in <paramref name="keys" />, or null to sort only <paramref name="keys" />.</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <typeparam name="TKey">The type of the elements of the key array.</typeparam>
		/// <typeparam name="TValue">The type of the elements of the items array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="keys" />.-or-<paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not null, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.-or-<paramref name="items" /> is not null, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.-or-<paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="keys" /><see cref="T:System.Array" />.-or-<paramref name="items" /> is not null, and <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="items" /><see cref="T:System.Array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">One or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, int index, int length)
		{
			Array.Sort<TKey, TValue>(keys, items, index, length, null);
		}

		/// <summary>Sorts the elements in a range of elements in an <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to sort.</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface implementation to use when comparing elements, or null to use the <see cref="T:System.IComparable`1" /> generic interface implementation of each element.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.-or-<paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />. -or-The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is null, and one or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort<T>(T[] array, int index, int length, IComparer<T> comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Sort<T, T>(array, null, index, length, comparer);
		}

		/// <summary>Sorts a range of elements in a pair of <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
		/// <param name="keys">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the keys to sort. </param>
		/// <param name="items">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the items that correspond to the keys in <paramref name="keys" />, or null to sort only <paramref name="keys" />.</param>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="length">The number of elements in the range to sort.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface implementation to use when comparing elements, or null to use the <see cref="T:System.IComparable`1" /> generic interface implementation of each element.</param>
		/// <typeparam name="TKey">The type of the elements of the key array.</typeparam>
		/// <typeparam name="TValue">The type of the elements of the items array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="keys" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="keys" />.-or-<paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="items" /> is not null, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.-or-<paramref name="items" /> is not null, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.-or-<paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="keys" /><see cref="T:System.Array" />.-or-<paramref name="items" /> is not null, and <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="items" /><see cref="T:System.Array" />. -or-The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is null, and one or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, int index, int length, IComparer<TKey> comparer)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			if (keys.Length - index < length || (items != null && index > items.Length - length))
			{
				throw new ArgumentException();
			}
			if (length <= 1)
			{
				return;
			}
			if (comparer == null)
			{
				Array.Swapper swap_items;
				if (items == null)
				{
					swap_items = null;
				}
				else
				{
					swap_items = Array.get_swapper<TValue>(items);
				}
				if (keys is double[])
				{
					Array.combsort(keys as double[], index, length, swap_items);
					return;
				}
				if (keys is int[])
				{
					Array.combsort(keys as int[], index, length, swap_items);
					return;
				}
				if (keys is char[])
				{
					Array.combsort(keys as char[], index, length, swap_items);
					return;
				}
			}
			try
			{
				int high = index + length - 1;
				Array.qsort<TKey, TValue>(keys, items, index, high, comparer);
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException(Locale.GetText("The comparer threw an exception."), innerException);
			}
		}

		/// <summary>Sorts the elements in an <see cref="T:System.Array" /> using the specified <see cref="T:System.Comparison`1" />.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to sort</param>
		/// <param name="comparison">The <see cref="T:System.Comparison`1" /> to use when comparing elements.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.-or-<paramref name="comparison" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The implementation of <paramref name="comparison" /> caused an error during the sort. For example, <paramref name="comparison" /> might not return 0 when comparing an item with itself.</exception>
		public static void Sort<T>(T[] array, Comparison<T> comparison)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Array.Sort<T>(array, array.Length, comparison);
		}

		internal static void Sort<T>(T[] array, int length, Comparison<T> comparison)
		{
			if (comparison == null)
			{
				throw new ArgumentNullException("comparison");
			}
			if (length <= 1 || array.Length <= 1)
			{
				return;
			}
			try
			{
				int low = 0;
				int high = length - 1;
				Array.qsort<T>(array, low, high, comparison);
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException(Locale.GetText("Comparison threw an exception."), innerException);
			}
		}

		private static void qsort<K, V>(K[] keys, V[] items, int low0, int high0, IComparer<K> comparer)
		{
			if (low0 >= high0)
			{
				return;
			}
			int num = low0;
			int num2 = high0;
			int num3 = num + (num2 - num) / 2;
			K k = keys[num3];
			for (;;)
			{
				while (num < high0 && Array.compare<K>(keys[num], k, comparer) < 0)
				{
					num++;
				}
				while (num2 > low0 && Array.compare<K>(k, keys[num2], comparer) < 0)
				{
					num2--;
				}
				if (num > num2)
				{
					break;
				}
				Array.swap<K, V>(keys, items, num, num2);
				num++;
				num2--;
			}
			if (low0 < num2)
			{
				Array.qsort<K, V>(keys, items, low0, num2, comparer);
			}
			if (num < high0)
			{
				Array.qsort<K, V>(keys, items, num, high0, comparer);
			}
		}

		private static int compare<T>(T value1, T value2, IComparer<T> comparer)
		{
			if (comparer != null)
			{
				return comparer.Compare(value1, value2);
			}
			if (value1 == null)
			{
				return (value2 != null) ? -1 : 0;
			}
			if (value2 == null)
			{
				return 1;
			}
			if (value1 is IComparable<T>)
			{
				return ((IComparable<T>)((object)value1)).CompareTo(value2);
			}
			if (value1 is IComparable)
			{
				return ((IComparable)((object)value1)).CompareTo(value2);
			}
			string text = Locale.GetText("No IComparable or IComparable<{0}> interface found.");
			throw new InvalidOperationException(string.Format(text, typeof(T)));
		}

		private static void qsort<T>(T[] array, int low0, int high0, Comparison<T> comparison)
		{
			if (low0 >= high0)
			{
				return;
			}
			int num = low0;
			int num2 = high0;
			int num3 = num + (num2 - num) / 2;
			T t = array[num3];
			for (;;)
			{
				while (num < high0 && comparison(array[num], t) < 0)
				{
					num++;
				}
				while (num2 > low0 && comparison(t, array[num2]) < 0)
				{
					num2--;
				}
				if (num > num2)
				{
					break;
				}
				Array.swap<T>(array, num, num2);
				num++;
				num2--;
			}
			if (low0 < num2)
			{
				Array.qsort<T>(array, low0, num2, comparison);
			}
			if (num < high0)
			{
				Array.qsort<T>(array, num, high0, comparison);
			}
		}

		private static void swap<K, V>(K[] keys, V[] items, int i, int j)
		{
			K k = keys[i];
			keys[i] = keys[j];
			keys[j] = k;
			if (items != null)
			{
				V v = items[i];
				items[i] = items[j];
				items[j] = v;
			}
		}

		private static void swap<T>(T[] array, int i, int j)
		{
			T t = array[i];
			array[i] = array[j];
			array[j] = t;
		}

		/// <summary>Copies all the elements of the current one-dimensional <see cref="T:System.Array" /> to the specified one-dimensional <see cref="T:System.Array" /> starting at the specified destination <see cref="T:System.Array" /> index. The index is specified as a 32-bit integer.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the current <see cref="T:System.Array" />.</param>
		/// <param name="index">A 32-bit integer that represents the index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or-The number of elements in the source <see cref="T:System.Array" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		/// <exception cref="T:System.ArrayTypeMismatchException">The type of the source <see cref="T:System.Array" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
		/// <exception cref="T:System.RankException">The source <see cref="T:System.Array" /> is multidimensional.</exception>
		/// <exception cref="T:System.InvalidCastException">At least one element in the source <see cref="T:System.Array" /> cannot be cast to the type of destination <paramref name="array" />.</exception>
		/// <filterpriority>2</filterpriority>
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (this.Rank > 1)
			{
				throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
			}
			if (index + this.GetLength(0) > array.GetLowerBound(0) + array.GetLength(0))
			{
				throw new ArgumentException("Destination array was not long enough. Check destIndex and length, and the array's lower bounds.");
			}
			if (array.Rank > 1)
			{
				throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Locale.GetText("Value has to be >= 0."));
			}
			Array.Copy(this, this.GetLowerBound(0), array, index, this.GetLength(0));
		}

		/// <summary>Copies all the elements of the current one-dimensional <see cref="T:System.Array" /> to the specified one-dimensional <see cref="T:System.Array" /> starting at the specified destination <see cref="T:System.Array" /> index. The index is specified as a 64-bit integer.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the current <see cref="T:System.Array" />.</param>
		/// <param name="index">A 64-bit integer that represents the index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or-The number of elements in the source <see cref="T:System.Array" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		/// <exception cref="T:System.ArrayTypeMismatchException">The type of the source <see cref="T:System.Array" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
		/// <exception cref="T:System.RankException">The source <see cref="T:System.Array" /> is multidimensional.</exception>
		/// <exception cref="T:System.InvalidCastException">At least one element in the source <see cref="T:System.Array" /> cannot be cast to the type of destination <paramref name="array" />.</exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public void CopyTo(Array array, long index)
		{
			if (index < 0L || index > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("index", Locale.GetText("Value must be >= 0 and <= Int32.MaxValue."));
			}
			this.CopyTo(array, (int)index);
		}

		/// <summary>Changes the number of elements of an array to the specified new size.</summary>
		/// <param name="array">The one-dimensional, zero-based array to resize, or null to create a new array with the specified size.</param>
		/// <param name="newSize">The size of the new array.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="newSize" /> is less than zero.</exception>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void Resize<T>(ref T[] array, int newSize)
		{
			Array.Resize<T>(ref array, (array != null) ? array.Length : 0, newSize);
		}

		internal static void Resize<T>(ref T[] array, int length, int newSize)
		{
			if (newSize < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (array == null)
			{
				array = new T[newSize];
				return;
			}
			if (array.Length == newSize)
			{
				return;
			}
			T[] array2 = new T[newSize];
			Array.Copy(array, array2, Math.Min(newSize, length));
			array = array2;
		}

		/// <summary>Determines whether every element in the array matches the conditions defined by the specified predicate.</summary>
		/// <returns>true if every element in <paramref name="array" /> matches the conditions defined by the specified predicate; otherwise, false. If there are no elements in the array, the return value is true.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to check against the conditions</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions to check against the elements.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.-or-<paramref name="match" /> is null.</exception>
		public static bool TrueForAll<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			foreach (T obj in array)
			{
				if (!match(obj))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Performs the specified action on each element of the specified array.</summary>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> on whose elements the action is to be performed.</param>
		/// <param name="action">The <see cref="T:System.Action`1" /> to perform on each element of <paramref name="array" />.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.-or-<paramref name="action" /> is null.</exception>
		public static void ForEach<T>(T[] array, Action<T> action)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			foreach (T obj in array)
			{
				action(obj);
			}
		}

		/// <summary>Converts an array of one type to an array of another type.</summary>
		/// <returns>An array of the target type containing the converted elements from the source array.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to convert to a target type.</param>
		/// <param name="converter">A <see cref="T:System.Converter`2" /> that converts each element from one type to another type.</param>
		/// <typeparam name="TInput">The type of the elements of the source array.</typeparam>
		/// <typeparam name="TOutput">The type of the elements of the target array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.-or-<paramref name="converter" /> is null.</exception>
		public static TOutput[] ConvertAll<TInput, TOutput>(TInput[] array, Converter<TInput, TOutput> converter)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (converter == null)
			{
				throw new ArgumentNullException("converter");
			}
			TOutput[] array2 = new TOutput[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = converter(array[i]);
			}
			return array2;
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the entire <see cref="T:System.Array" />.</summary>
		/// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, –1.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.-or-<paramref name="match" /> is null.</exception>
		public static int FindLastIndex<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.FindLastIndex<T>(array, 0, array.Length, match);
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Array" /> that extends from the first element to the specified index.</summary>
		/// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, –1.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="startIndex">The zero-based starting index of the backward search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.-or-<paramref name="match" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
		public static int FindLastIndex<T>(T[] array, int startIndex, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException();
			}
			return Array.FindLastIndex<T>(array, startIndex, array.Length - startIndex, match);
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Array" /> that contains the specified number of elements and ends at the specified index.</summary>
		/// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, –1.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="startIndex">The zero-based starting index of the backward search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.-or-<paramref name="match" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.-or-<paramref name="count" /> is less than zero.-or-<paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
		public static int FindLastIndex<T>(T[] array, int startIndex, int count, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			if (startIndex > array.Length || startIndex + count > array.Length)
			{
				throw new ArgumentOutOfRangeException();
			}
			for (int i = startIndex + count - 1; i >= startIndex; i--)
			{
				if (match(array[i]))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire <see cref="T:System.Array" />.</summary>
		/// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, –1.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.-or-<paramref name="match" /> is null.</exception>
		public static int FindIndex<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.FindIndex<T>(array, 0, array.Length, match);
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Array" /> that extends from the specified index to the last element.</summary>
		/// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, –1.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="startIndex">The zero-based starting index of the search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.-or-<paramref name="match" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
		public static int FindIndex<T>(T[] array, int startIndex, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.FindIndex<T>(array, startIndex, array.Length - startIndex, match);
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Array" /> that starts at the specified index and contains the specified number of elements.</summary>
		/// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, –1.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="startIndex">The zero-based starting index of the search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.-or-<paramref name="match" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.-or-<paramref name="count" /> is less than zero.-or-<paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
		public static int FindIndex<T>(T[] array, int startIndex, int count, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			if (startIndex > array.Length || startIndex + count > array.Length)
			{
				throw new ArgumentOutOfRangeException();
			}
			for (int i = startIndex; i < startIndex + count; i++)
			{
				if (match(array[i]))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>Searches an entire one-dimensional sorted <see cref="T:System.Array" /> for a specific element, using the <see cref="T:System.IComparable`1" /> generic interface implemented by each element of the <see cref="T:System.Array" /> and by the specified object.</summary>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, a negative number which is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than any of the elements in <paramref name="array" />, a negative number which is the bitwise complement of (the index of the last element plus 1).</returns>
		/// <param name="array">The sorted one-dimensional, zero-based <see cref="T:System.Array" /> to search. </param>
		/// <param name="value">The object to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IComparable`1" /> generic interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int BinarySearch<T>(T[] array, T value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.BinarySearch<T>(array, 0, array.Length, value, null);
		}

		/// <summary>Searches an entire one-dimensional sorted <see cref="T:System.Array" /> for a value using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, a negative number which is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than any of the elements in <paramref name="array" />, a negative number which is the bitwise complement of (the index of the last element plus 1).</returns>
		/// <param name="array">The sorted one-dimensional, zero-based <see cref="T:System.Array" /> to search.  </param>
		/// <param name="value">The object to search for.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing elements.-or- null to use the <see cref="T:System.IComparable`1" /> implementation of each element.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="comparer" /> is null, and <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is null, <paramref name="value" /> does not implement the <see cref="T:System.IComparable`1" /> generic interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int BinarySearch<T>(T[] array, T value, IComparer<T> comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.BinarySearch<T>(array, 0, array.Length, value, comparer);
		}

		/// <summary>Searches a range of elements in a one-dimensional sorted <see cref="T:System.Array" /> for a value, using the <see cref="T:System.IComparable`1" /> generic interface implemented by each element of the <see cref="T:System.Array" /> and by the specified value.</summary>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, a negative number which is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than any of the elements in <paramref name="array" />, a negative number which is the bitwise complement of (the index of the last element plus 1).</returns>
		/// <param name="array">The sorted one-dimensional, zero-based <see cref="T:System.Array" /> to search. </param>
		/// <param name="index">The starting index of the range to search.</param>
		/// <param name="length">The length of the range to search.</param>
		/// <param name="value">The object to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.-or-<paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.-or-<paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IComparable`1" /> generic interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int BinarySearch<T>(T[] array, int index, int length, T value)
		{
			return Array.BinarySearch<T>(array, index, length, value, null);
		}

		/// <summary>Searches a range of elements in a one-dimensional sorted <see cref="T:System.Array" /> for a value, using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
		/// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, a negative number which is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than any of the elements in <paramref name="array" />, a negative number which is the bitwise complement of (the index of the last element plus 1).</returns>
		/// <param name="array">The sorted one-dimensional, zero-based <see cref="T:System.Array" /> to search. </param>
		/// <param name="index">The starting index of the range to search.</param>
		/// <param name="length">The length of the range to search.</param>
		/// <param name="value">The object to search for.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing elements.-or- null to use the <see cref="T:System.IComparable`1" /> implementation of each element.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than the lower bound of <paramref name="array" />.-or-<paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.-or-<paramref name="comparer" /> is null, and <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="comparer" /> is null, <paramref name="value" /> does not implement the <see cref="T:System.IComparable`1" /> generic interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int BinarySearch<T>(T[] array, int index, int length, T value, IComparer<T> comparer)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Locale.GetText("index is less than the lower bound of array."));
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Locale.GetText("Value has to be >= 0."));
			}
			if (index > array.Length - length)
			{
				throw new ArgumentException(Locale.GetText("index and length do not specify a valid range in array."));
			}
			if (comparer == null)
			{
				comparer = Comparer<T>.Default;
			}
			int i = index;
			int num = index + length - 1;
			try
			{
				while (i <= num)
				{
					int num2 = i + (num - i) / 2;
					int num3 = comparer.Compare(value, array[num2]);
					if (num3 == 0)
					{
						return num2;
					}
					if (num3 < 0)
					{
						num = num2 - 1;
					}
					else
					{
						i = num2 + 1;
					}
				}
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException(Locale.GetText("Comparer threw an exception."), innerException);
			}
			return ~i;
		}

		/// <summary>Searches for the specified object and returns the index of the first occurrence within the entire <see cref="T:System.Array" />.</summary>
		/// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the entire <paramref name="array" />, if found; otherwise, –1.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		public static int IndexOf<T>(T[] array, T value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.IndexOf<T>(array, value, 0, array.Length);
		}

		/// <summary>Searches for the specified object and returns the index of the first occurrence within the range of elements in the <see cref="T:System.Array" /> that extends from the specified index to the last element.</summary>
		/// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that extends from <paramref name="startIndex" /> to the last element, if found; otherwise, –1.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The zero-based starting index of the search. 0 (zero) is valid in an empty array.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
		public static int IndexOf<T>(T[] array, T value, int startIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.IndexOf<T>(array, value, startIndex, array.Length - startIndex);
		}

		/// <summary>Searches for the specified object and returns the index of the first occurrence within the range of elements in the <see cref="T:System.Array" /> that starts at the specified index and contains the specified number of elements.</summary>
		/// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that starts at <paramref name="startIndex" /> and contains the number of elements specified in <paramref name="count" />, if found; otherwise, –1.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The zero-based starting index of the search. 0 (zero) is valid in an empty array.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.-or-<paramref name="count" /> is less than zero.-or-<paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
		public static int IndexOf<T>(T[] array, T value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (count < 0 || startIndex < array.GetLowerBound(0) || startIndex - 1 > array.GetUpperBound(0) - count)
			{
				throw new ArgumentOutOfRangeException();
			}
			int num = startIndex + count;
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = startIndex; i < num; i++)
			{
				if (@default.Equals(array[i], value))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>Searches for the specified object and returns the index of the last occurrence within the entire <see cref="T:System.Array" />.</summary>
		/// <returns>The zero-based index of the last occurrence of <paramref name="value" /> within the entire <paramref name="array" />, if found; otherwise, –1.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		public static int LastIndexOf<T>(T[] array, T value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Length == 0)
			{
				return -1;
			}
			return Array.LastIndexOf<T>(array, value, array.Length - 1);
		}

		/// <summary>Searches for the specified object and returns the index of the last occurrence within the range of elements in the <see cref="T:System.Array" /> that extends from the first element to the specified index.</summary>
		/// <returns>The zero-based index of the last occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that extends from the first element to <paramref name="startIndex" />, if found; otherwise, –1.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The zero-based starting index of the backward search.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
		public static int LastIndexOf<T>(T[] array, T value, int startIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return Array.LastIndexOf<T>(array, value, startIndex, startIndex + 1);
		}

		/// <summary>Searches for the specified object and returns the index of the last occurrence within the range of elements in the <see cref="T:System.Array" /> that contains the specified number of elements and ends at the specified index.</summary>
		/// <returns>The zero-based index of the last occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that contains the number of elements specified in <paramref name="count" /> and ends at <paramref name="startIndex" />, if found; otherwise, –1.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="value">The object to locate in <paramref name="array" />.</param>
		/// <param name="startIndex">The zero-based starting index of the backward search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.-or-<paramref name="count" /> is less than zero.-or-<paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
		public static int LastIndexOf<T>(T[] array, T value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (count < 0 || startIndex < array.GetLowerBound(0) || startIndex > array.GetUpperBound(0) || startIndex - count + 1 < array.GetLowerBound(0))
			{
				throw new ArgumentOutOfRangeException();
			}
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = startIndex; i >= startIndex - count + 1; i--)
			{
				if (@default.Equals(array[i], value))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>Retrieves all the elements that match the conditions defined by the specified predicate.</summary>
		/// <returns>An <see cref="T:System.Array" /> containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="T:System.Array" />.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the elements to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.-or-<paramref name="match" /> is null.</exception>
		public static T[] FindAll<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			int newSize = 0;
			T[] array2 = new T[array.Length];
			foreach (T t in array)
			{
				if (match(t))
				{
					array2[newSize++] = t;
				}
			}
			Array.Resize<T>(ref array2, newSize);
			return array2;
		}

		/// <summary>Determines whether the specified array contains elements that match the conditions defined by the specified predicate.</summary>
		/// <returns>true if <paramref name="array" /> contains one or more elements that match the conditions defined by the specified predicate; otherwise, false.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the elements to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.-or-<paramref name="match" /> is null.</exception>
		public static bool Exists<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			foreach (T obj in array)
			{
				if (match(obj))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Returns a read-only wrapper for the specified array.</summary>
		/// <returns>A read-only <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> wrapper for the specified array.</returns>
		/// <param name="array">The one-dimensional, zero-based array to wrap in a read-only <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" />  wrapper. </param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		public static ReadOnlyCollection<T> AsReadOnly<T>(T[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			return new ReadOnlyCollection<T>(new Array.ArrayReadOnlyList<T>(array));
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire <see cref="T:System.Array" />.</summary>
		/// <returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type <paramref name="T" />.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.-or-<paramref name="match" /> is null.</exception>
		public static T Find<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			foreach (T t in array)
			{
				if (match(t))
				{
					return t;
				}
			}
			return default(T);
		}

		/// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the last occurrence within the entire <see cref="T:System.Array" />.</summary>
		/// <returns>The last element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type <paramref name="T" />.</returns>
		/// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
		/// <typeparam name="T">The type of the elements of the array.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.-or-<paramref name="match" /> is null.</exception>
		public static T FindLast<T>(T[] array, Predicate<T> match)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			for (int i = array.Length - 1; i >= 0; i--)
			{
				if (match(array[i]))
				{
					return array[i];
				}
			}
			return default(T);
		}

		/// <summary>Copies a range of elements from an <see cref="T:System.Array" /> starting at the specified source index and pastes them to another <see cref="T:System.Array" /> starting at the specified destination index.  Guarantees that all changes are undone if the copy does not succeed completely.</summary>
		/// <param name="sourceArray">The <see cref="T:System.Array" /> that contains the data to copy.</param>
		/// <param name="sourceIndex">A 32-bit integer that represents the index in the <paramref name="sourceArray" /> at which copying begins.</param>
		/// <param name="destinationArray">The <see cref="T:System.Array" /> that receives the data.</param>
		/// <param name="destinationIndex">A 32-bit integer that represents the index in the <paramref name="destinationArray" /> at which storing begins.</param>
		/// <param name="length">A 32-bit integer that represents the number of elements to copy.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceArray" /> is null.-or-<paramref name="destinationArray" /> is null.</exception>
		/// <exception cref="T:System.RankException">
		///   <paramref name="sourceArray" /> and <paramref name="destinationArray" /> have different ranks.</exception>
		/// <exception cref="T:System.ArrayTypeMismatchException">The <paramref name="sourceArray" /> type is neither the same as nor derived from the <paramref name="destinationArray" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">At least one element in <paramref name="sourceArray" /> cannot be cast to the type of <paramref name="destinationArray" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="sourceIndex" /> is less than the lower bound of the first dimension of <paramref name="sourceArray" />.-or-<paramref name="destinationIndex" /> is less than the lower bound of the first dimension of <paramref name="destinationArray" />.-or-<paramref name="length" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="length" /> is greater than the number of elements from <paramref name="sourceIndex" /> to the end of <paramref name="sourceArray" />.-or-<paramref name="length" /> is greater than the number of elements from <paramref name="destinationIndex" /> to the end of <paramref name="destinationArray" />.</exception>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static void ConstrainedCopy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
		{
			Array.Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);
		}

		internal struct InternalEnumerator<T> : IEnumerator, IDisposable, IEnumerator<T>
		{
			private const int NOT_STARTED = -2;

			private const int FINISHED = -1;

			private Array array;

			private int idx;

			internal InternalEnumerator(Array array)
			{
				this.array = array;
				this.idx = -2;
			}

			void IEnumerator.Reset()
			{
				this.idx = -2;
			}

			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			public void Dispose()
			{
				this.idx = -2;
			}

			public bool MoveNext()
			{
				if (this.idx == -2)
				{
					this.idx = this.array.Length;
				}
				return this.idx != -1 && --this.idx != -1;
			}

			public T Current
			{
				get
				{
					if (this.idx == -2)
					{
						throw new InvalidOperationException("Enumeration has not started. Call MoveNext");
					}
					if (this.idx == -1)
					{
						throw new InvalidOperationException("Enumeration already finished");
					}
					return this.array.InternalArray__get_Item<T>(this.array.Length - 1 - this.idx);
				}
			}
		}

		internal class SimpleEnumerator : IEnumerator, ICloneable
		{
			private Array enumeratee;

			private int currentpos;

			private int length;

			public SimpleEnumerator(Array arrayToEnumerate)
			{
				this.enumeratee = arrayToEnumerate;
				this.currentpos = -1;
				this.length = arrayToEnumerate.Length;
			}

			public object Current
			{
				get
				{
					if (this.currentpos < 0)
					{
						throw new InvalidOperationException(Locale.GetText("Enumeration has not started."));
					}
					if (this.currentpos >= this.length)
					{
						throw new InvalidOperationException(Locale.GetText("Enumeration has already ended"));
					}
					return this.enumeratee.GetValueImpl(this.currentpos);
				}
			}

			public bool MoveNext()
			{
				if (this.currentpos < this.length)
				{
					this.currentpos++;
				}
				return this.currentpos < this.length;
			}

			public void Reset()
			{
				this.currentpos = -1;
			}

			public object Clone()
			{
				return base.MemberwiseClone();
			}
		}

		private class ArrayReadOnlyList<T> : IEnumerable, IList<T>, ICollection<T>, IEnumerable<T>
		{
			private T[] array;

			public ArrayReadOnlyList(T[] array)
			{
				this.array = array;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			public T this[int index]
			{
				get
				{
					if (index >= this.array.Length)
					{
						throw new ArgumentOutOfRangeException("index");
					}
					return this.array[index];
				}
				set
				{
					throw Array.ArrayReadOnlyList<T>.ReadOnlyError();
				}
			}

			public int Count
			{
				get
				{
					return this.array.Length;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			public void Add(T item)
			{
				throw Array.ArrayReadOnlyList<T>.ReadOnlyError();
			}

			public void Clear()
			{
				throw Array.ArrayReadOnlyList<T>.ReadOnlyError();
			}

			public bool Contains(T item)
			{
				return Array.IndexOf<T>(this.array, item) >= 0;
			}

			public void CopyTo(T[] array, int index)
			{
				this.array.CopyTo(array, index);
			}

			public IEnumerator<T> GetEnumerator()
			{
				for (int i = 0; i < this.array.Length; i++)
				{
					yield return this.array[i];
				}
				yield break;
			}

			public int IndexOf(T item)
			{
				return Array.IndexOf<T>(this.array, item);
			}

			public void Insert(int index, T item)
			{
				throw Array.ArrayReadOnlyList<T>.ReadOnlyError();
			}

			public bool Remove(T item)
			{
				throw Array.ArrayReadOnlyList<T>.ReadOnlyError();
			}

			public void RemoveAt(int index)
			{
				throw Array.ArrayReadOnlyList<T>.ReadOnlyError();
			}

			private static Exception ReadOnlyError()
			{
				return new NotSupportedException("This collection is read-only.");
			}
		}

		private delegate void Swapper(int i, int j);
	}
}
