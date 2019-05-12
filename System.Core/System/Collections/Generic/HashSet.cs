using System;
using System.Linq;
using System.Runtime.Serialization;

namespace System.Collections.Generic
{
	/// <summary>Represents a set of values.</summary>
	/// <typeparam name="T">The type of elements in the hash set.</typeparam>
	[Serializable]
	public class HashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, ISerializable, IDeserializationCallback
	{
		private const int INITIAL_SIZE = 10;

		private const float DEFAULT_LOAD_FACTOR = 0.9f;

		private const int NO_SLOT = -1;

		private const int HASH_FLAG = -2147483648;

		private int[] table;

		private HashSet<T>.Link[] links;

		private T[] slots;

		private int touched;

		private int empty_slot;

		private int count;

		private int threshold;

		private IEqualityComparer<T> comparer;

		private SerializationInfo si;

		private int generation;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.HashSet`1" /> class that is empty and uses the default equality comparer for the set type.</summary>
		public HashSet()
		{
			this.Init(10, null);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.HashSet`1" /> class that is empty and uses the specified equality comparer for the set type.</summary>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing values in the set, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> implementation for the set type.</param>
		public HashSet(IEqualityComparer<T> comparer)
		{
			this.Init(10, comparer);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.HashSet`1" /> class that uses the default equality comparer for the set type, contains elements copied from the specified collection, and has sufficient capacity to accommodate the number of elements copied.</summary>
		/// <param name="collection">The collection whose elements are copied to the new set.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="collection" /> is null.</exception>
		public HashSet(IEnumerable<T> collection) : this(collection, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.HashSet`1" /> class that uses the specified equality comparer for the set type, contains elements copied from the specified collection, and has sufficient capacity to accommodate the number of elements copied.</summary>
		/// <param name="collection">The collection whose elements are copied to the new set.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing values in the set, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> implementation for the set type.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="collection" /> is null.</exception>
		public HashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			int capacity = 0;
			ICollection<T> collection2 = collection as ICollection<T>;
			if (collection2 != null)
			{
				capacity = collection2.Count;
			}
			this.Init(capacity, comparer);
			foreach (T item in collection)
			{
				this.Add(item);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.HashSet`1" /> class with serialized data.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
		protected HashSet(SerializationInfo info, StreamingContext context)
		{
			this.si = info;
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new HashSet<T>.Enumerator(this);
		}

		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		void ICollection<T>.CopyTo(T[] array, int index)
		{
			this.CopyTo(array, index);
		}

		void ICollection<T>.Add(T item)
		{
			this.Add(item);
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new HashSet<T>.Enumerator(this);
		}

		/// <summary>Gets the number of elements that are contained in a set.</summary>
		/// <returns>The number of elements that are contained in the set.</returns>
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		private void Init(int capacity, IEqualityComparer<T> comparer)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			this.comparer = (comparer ?? EqualityComparer<T>.Default);
			if (capacity == 0)
			{
				capacity = 10;
			}
			capacity = (int)((float)capacity / 0.9f) + 1;
			this.InitArrays(capacity);
			this.generation = 0;
		}

		private void InitArrays(int size)
		{
			this.table = new int[size];
			this.links = new HashSet<T>.Link[size];
			this.empty_slot = -1;
			this.slots = new T[size];
			this.touched = 0;
			this.threshold = (int)((float)this.table.Length * 0.9f);
			if (this.threshold == 0 && this.table.Length > 0)
			{
				this.threshold = 1;
			}
		}

		private bool SlotsContainsAt(int index, int hash, T item)
		{
			HashSet<T>.Link link;
			for (int num = this.table[index] - 1; num != -1; num = link.Next)
			{
				link = this.links[num];
				if (link.HashCode == hash && ((hash != -2147483648 || (item != null && this.slots[num] != null)) ? this.comparer.Equals(item, this.slots[num]) : (item == null && null == this.slots[num])))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Copies the elements of a <see cref="T:System.Collections.Generic.HashSet`1" /> object to an array.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from the <see cref="T:System.Collections.Generic.HashSet`1" /> object. The array must have zero-based indexing.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		public void CopyTo(T[] array)
		{
			this.CopyTo(array, 0, this.count);
		}

		/// <summary>Copies the elements of a <see cref="T:System.Collections.Generic.HashSet`1" /> object to an array, starting at the specified array index.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from the <see cref="T:System.Collections.Generic.HashSet`1" /> object. The array must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="arrayIndex" /> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="arrayIndex" /> is greater than the length of the destination <paramref name="array" />.-or-<paramref name="count" /> is larger than the size of the destination <paramref name="array" />.</exception>
		public void CopyTo(T[] array, int index)
		{
			this.CopyTo(array, index, this.count);
		}

		/// <summary>Copies the specified number of elements of a <see cref="T:System.Collections.Generic.HashSet`1" /> object to an array, starting at the specified array index.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from the <see cref="T:System.Collections.Generic.HashSet`1" /> object. The array must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <param name="count">The number of elements to copy to <paramref name="array" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="arrayIndex" /> is less than 0.-or-<paramref name="count" /> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="arrayIndex" /> is greater than the length of the destination <paramref name="array" />.-or-<paramref name="count" /> is greater than the available space from the <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		public void CopyTo(T[] array, int index, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (index > array.Length)
			{
				throw new ArgumentException("index larger than largest valid index of array");
			}
			if (array.Length - index < count)
			{
				throw new ArgumentException("Destination array cannot hold the requested elements!");
			}
			int num = 0;
			int num2 = 0;
			while (num < this.touched && num2 < count)
			{
				if (this.GetLinkHashCode(num) != 0)
				{
					array[index++] = this.slots[num];
				}
				num++;
			}
		}

		private void Resize()
		{
			int num = HashSet<T>.PrimeHelper.ToPrime(this.table.Length << 1 | 1);
			int[] array = new int[num];
			HashSet<T>.Link[] array2 = new HashSet<T>.Link[num];
			for (int i = 0; i < this.table.Length; i++)
			{
				for (int num2 = this.table[i] - 1; num2 != -1; num2 = this.links[num2].Next)
				{
					int num3 = array2[num2].HashCode = this.GetItemHashCode(this.slots[num2]);
					int num4 = (num3 & int.MaxValue) % num;
					array2[num2].Next = array[num4] - 1;
					array[num4] = num2 + 1;
				}
			}
			this.table = array;
			this.links = array2;
			T[] destinationArray = new T[num];
			Array.Copy(this.slots, 0, destinationArray, 0, this.touched);
			this.slots = destinationArray;
			this.threshold = (int)((float)num * 0.9f);
		}

		private int GetLinkHashCode(int index)
		{
			return this.links[index].HashCode & int.MinValue;
		}

		private int GetItemHashCode(T item)
		{
			if (item == null)
			{
				return int.MinValue;
			}
			return this.comparer.GetHashCode(item) | int.MinValue;
		}

		/// <summary>Adds the specified element to a set.</summary>
		/// <returns>true if the element is added to the <see cref="T:System.Collections.Generic.HashSet`1" /> object; false if the element is already present.</returns>
		/// <param name="item">The element to add to the set.</param>
		public bool Add(T item)
		{
			int itemHashCode = this.GetItemHashCode(item);
			int num = (itemHashCode & int.MaxValue) % this.table.Length;
			if (this.SlotsContainsAt(num, itemHashCode, item))
			{
				return false;
			}
			if (++this.count > this.threshold)
			{
				this.Resize();
				num = (itemHashCode & int.MaxValue) % this.table.Length;
			}
			int num2 = this.empty_slot;
			if (num2 == -1)
			{
				num2 = this.touched++;
			}
			else
			{
				this.empty_slot = this.links[num2].Next;
			}
			this.links[num2].HashCode = itemHashCode;
			this.links[num2].Next = this.table[num] - 1;
			this.table[num] = num2 + 1;
			this.slots[num2] = item;
			this.generation++;
			return true;
		}

		/// <summary>Gets the <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> object that is used to determine equality for the values in the set.</summary>
		/// <returns>The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> object that is used to determine equality for the values in the set.</returns>
		public IEqualityComparer<T> Comparer
		{
			get
			{
				return this.comparer;
			}
		}

		/// <summary>Removes all elements from a <see cref="T:System.Collections.Generic.HashSet`1" /> object.</summary>
		public void Clear()
		{
			this.count = 0;
			Array.Clear(this.table, 0, this.table.Length);
			Array.Clear(this.slots, 0, this.slots.Length);
			Array.Clear(this.links, 0, this.links.Length);
			this.empty_slot = -1;
			this.touched = 0;
			this.generation++;
		}

		/// <summary>Determines whether a <see cref="T:System.Collections.Generic.HashSet`1" /> object contains the specified element.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.HashSet`1" /> object contains the specified element; otherwise, false.</returns>
		/// <param name="item">The element to locate in the <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
		public bool Contains(T item)
		{
			int itemHashCode = this.GetItemHashCode(item);
			int index = (itemHashCode & int.MaxValue) % this.table.Length;
			return this.SlotsContainsAt(index, itemHashCode, item);
		}

		/// <summary>Removes the specified element from a <see cref="T:System.Collections.Generic.HashSet`1" /> object.</summary>
		/// <returns>true if the element is successfully found and removed; otherwise, false.  This method returns false if <paramref name="item" /> is not found in the <see cref="T:System.Collections.Generic.HashSet`1" /> object.</returns>
		/// <param name="item">The element to remove.</param>
		public bool Remove(T item)
		{
			int itemHashCode = this.GetItemHashCode(item);
			int num = (itemHashCode & int.MaxValue) % this.table.Length;
			int num2 = this.table[num] - 1;
			if (num2 == -1)
			{
				return false;
			}
			int num3 = -1;
			do
			{
				HashSet<T>.Link link = this.links[num2];
				if (link.HashCode == itemHashCode && ((itemHashCode != -2147483648 || (item != null && this.slots[num2] != null)) ? this.comparer.Equals(this.slots[num2], item) : (item == null && null == this.slots[num2])))
				{
					break;
				}
				num3 = num2;
				num2 = link.Next;
			}
			while (num2 != -1);
			if (num2 == -1)
			{
				return false;
			}
			this.count--;
			if (num3 == -1)
			{
				this.table[num] = this.links[num2].Next + 1;
			}
			else
			{
				this.links[num3].Next = this.links[num2].Next;
			}
			this.links[num2].Next = this.empty_slot;
			this.empty_slot = num2;
			this.links[num2].HashCode = 0;
			this.slots[num2] = default(T);
			this.generation++;
			return true;
		}

		/// <summary>Removes all elements that match the conditions defined by the specified predicate from a <see cref="T:System.Collections.Generic.HashSet`1" /> collection.</summary>
		/// <returns>The number of elements that were removed from the <see cref="T:System.Collections.Generic.HashSet`1" /> collection.</returns>
		/// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the elements to remove.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="match" /> is null.</exception>
		public int RemoveWhere(Predicate<T> predicate)
		{
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			int num = 0;
			T[] array = new T[this.count];
			this.CopyTo(array, 0);
			foreach (T t in array)
			{
				if (predicate(t))
				{
					this.Remove(t);
					num++;
				}
			}
			return num;
		}

		/// <summary>Sets the capacity of a <see cref="T:System.Collections.Generic.HashSet`1" /> object to the actual number of elements it contains, rounded up to a nearby, implementation-specific value.</summary>
		public void TrimExcess()
		{
			this.Resize();
		}

		/// <summary>Modifies the current <see cref="T:System.Collections.Generic.HashSet`1" /> object to contain only elements that are present in that object and in the specified collection.</summary>
		/// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="other" /> is null.</exception>
		public void IntersectWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			T[] array = new T[this.count];
			this.CopyTo(array, 0);
			foreach (T t in array)
			{
				if (!other.Contains(t))
				{
					this.Remove(t);
				}
			}
			foreach (T item in other)
			{
				if (!this.Contains(item))
				{
					this.Remove(item);
				}
			}
		}

		/// <summary>Removes all elements in the specified collection from the current <see cref="T:System.Collections.Generic.HashSet`1" /> object.</summary>
		/// <param name="other">The collection of items to remove from the <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="other" /> is null.</exception>
		public void ExceptWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			foreach (T item in other)
			{
				this.Remove(item);
			}
		}

		/// <summary>Determines whether the current <see cref="T:System.Collections.Generic.HashSet`1" /> object and a specified collection share common elements.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.HashSet`1" /> object and <paramref name="other" /> share at least one common element; otherwise, false.</returns>
		/// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="other" /> is null.</exception>
		public bool Overlaps(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			foreach (T item in other)
			{
				if (this.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Determines whether a <see cref="T:System.Collections.Generic.HashSet`1" /> object and the specified collection contain the same elements.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.HashSet`1" /> object is equal to <paramref name="other" />; otherwise, false.</returns>
		/// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="other" /> is null.</exception>
		public bool SetEquals(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (this.count != other.Count<T>())
			{
				return false;
			}
			foreach (T value in this)
			{
				if (!other.Contains(value))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Modifies the current <see cref="T:System.Collections.Generic.HashSet`1" /> object to contain only elements that are present either in that object or in the specified collection, but not both.</summary>
		/// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="other" /> is null.</exception>
		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			foreach (T item in other)
			{
				if (this.Contains(item))
				{
					this.Remove(item);
				}
				else
				{
					this.Add(item);
				}
			}
		}

		/// <summary>Modifies the current <see cref="T:System.Collections.Generic.HashSet`1" /> object to contain all elements that are present in both itself and in the specified collection.</summary>
		/// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="other" /> is null.</exception>
		public void UnionWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			foreach (T item in other)
			{
				this.Add(item);
			}
		}

		private bool CheckIsSubsetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			foreach (T value in this)
			{
				if (!other.Contains(value))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Determines whether a <see cref="T:System.Collections.Generic.HashSet`1" /> object is a subset of the specified collection.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.HashSet`1" /> object is a subset of <paramref name="other" />; otherwise, false.</returns>
		/// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="other" /> is null.</exception>
		public bool IsSubsetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			return this.count == 0 || (this.count <= other.Count<T>() && this.CheckIsSubsetOf(other));
		}

		/// <summary>Determines whether a <see cref="T:System.Collections.Generic.HashSet`1" /> object is a proper subset of the specified collection.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.HashSet`1" /> object is a proper subset of <paramref name="other" />; otherwise, false.</returns>
		/// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="other" /> is null.</exception>
		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			return this.count == 0 || (this.count < other.Count<T>() && this.CheckIsSubsetOf(other));
		}

		private bool CheckIsSupersetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			foreach (T item in other)
			{
				if (!this.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Determines whether a <see cref="T:System.Collections.Generic.HashSet`1" /> object is a superset of the specified collection.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.HashSet`1" /> object is a superset of <paramref name="other" />; otherwise, false.</returns>
		/// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="other" /> is null.</exception>
		public bool IsSupersetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			return this.count >= other.Count<T>() && this.CheckIsSupersetOf(other);
		}

		/// <summary>Determines whether a <see cref="T:System.Collections.Generic.HashSet`1" /> object is a proper superset of the specified collection.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.HashSet`1" /> object is a proper superset of <paramref name="other" />; otherwise, false.</returns>
		/// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="other" /> is null.</exception>
		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			return this.count > other.Count<T>() && this.CheckIsSupersetOf(other);
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEqualityComparer" /> object that can be used for equality testing of a <see cref="T:System.Collections.Generic.HashSet`1" /> object.</summary>
		/// <returns>An <see cref="T:System.Collections.IEqualityComparer" /> object that can be used for deep equality testing of the <see cref="T:System.Collections.Generic.HashSet`1" /> object.</returns>
		[MonoTODO]
		public static IEqualityComparer<HashSet<T>> CreateSetComparer()
		{
			throw new NotImplementedException();
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize a <see cref="T:System.Collections.Generic.HashSet`1" /> object.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null.</exception>
		/// <filterpriority>2</filterpriority>
		[MonoTODO]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and raises the deserialization event when the deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object associated with the current <see cref="T:System.Collections.Generic.HashSet`1" /> object is invalid.</exception>
		/// <filterpriority>2</filterpriority>
		[MonoTODO]
		public virtual void OnDeserialization(object sender)
		{
			if (this.si == null)
			{
				return;
			}
			throw new NotImplementedException();
		}

		/// <summary>Returns an enumerator that iterates through a <see cref="T:System.Collections.Generic.HashSet`1" /> object.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.HashSet`1.Enumerator" /> object for the <see cref="T:System.Collections.Generic.HashSet`1" /> object.</returns>
		public HashSet<T>.Enumerator GetEnumerator()
		{
			return new HashSet<T>.Enumerator(this);
		}

		private struct Link
		{
			public int HashCode;

			public int Next;
		}

		/// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.HashSet`1" /> object.</summary>
		/// <filterpriority>2</filterpriority>
		[Serializable]
		public struct Enumerator : IEnumerator, IDisposable, IEnumerator<T>
		{
			private HashSet<T> hashset;

			private int next;

			private int stamp;

			private T current;

			internal Enumerator(HashSet<T> hashset)
			{
				this.hashset = hashset;
				this.stamp = hashset.generation;
			}

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the collection at the current position of the enumerator, as an <see cref="T:System.Object" />.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
			object IEnumerator.Current
			{
				get
				{
					this.CheckState();
					if (this.next <= 0)
					{
						throw new InvalidOperationException("Current is not valid");
					}
					return this.current;
				}
			}

			/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			void IEnumerator.Reset()
			{
				this.CheckState();
				this.next = 0;
			}

			/// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.HashSet`1" /> collection.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			public bool MoveNext()
			{
				this.CheckState();
				if (this.next < 0)
				{
					return false;
				}
				while (this.next < this.hashset.touched)
				{
					int num = this.next++;
					if (this.hashset.GetLinkHashCode(num) != 0)
					{
						this.current = this.hashset.slots[num];
						return true;
					}
				}
				this.next = -1;
				return false;
			}

			/// <summary>Gets the element at the current position of the enumerator.</summary>
			/// <returns>The element in the <see cref="T:System.Collections.Generic.HashSet`1" /> collection at the current position of the enumerator.</returns>
			public T Current
			{
				get
				{
					return this.current;
				}
			}

			/// <summary>Releases all resources used by a <see cref="T:System.Collections.Generic.HashSet`1.Enumerator" /> object.</summary>
			public void Dispose()
			{
				this.hashset = null;
			}

			private void CheckState()
			{
				if (this.hashset == null)
				{
					throw new ObjectDisposedException(null);
				}
				if (this.hashset.generation != this.stamp)
				{
					throw new InvalidOperationException("HashSet have been modified while it was iterated over");
				}
			}
		}

		private static class PrimeHelper
		{
			private static readonly int[] primes_table = new int[]
			{
				11,
				19,
				37,
				73,
				109,
				163,
				251,
				367,
				557,
				823,
				1237,
				1861,
				2777,
				4177,
				6247,
				9371,
				14057,
				21089,
				31627,
				47431,
				71143,
				106721,
				160073,
				240101,
				360163,
				540217,
				810343,
				1215497,
				1823231,
				2734867,
				4102283,
				6153409,
				9230113,
				13845163
			};

			private static bool TestPrime(int x)
			{
				if ((x & 1) != 0)
				{
					int num = (int)Math.Sqrt((double)x);
					for (int i = 3; i < num; i += 2)
					{
						if (x % i == 0)
						{
							return false;
						}
					}
					return true;
				}
				return x == 2;
			}

			private static int CalcPrime(int x)
			{
				for (int i = (x & -2) - 1; i < 2147483647; i += 2)
				{
					if (HashSet<T>.PrimeHelper.TestPrime(i))
					{
						return i;
					}
				}
				return x;
			}

			public static int ToPrime(int x)
			{
				for (int i = 0; i < HashSet<T>.PrimeHelper.primes_table.Length; i++)
				{
					if (x <= HashSet<T>.PrimeHelper.primes_table[i])
					{
						return HashSet<T>.PrimeHelper.primes_table[i];
					}
				}
				return HashSet<T>.PrimeHelper.CalcPrime(x);
			}
		}
	}
}
