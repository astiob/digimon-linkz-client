using System;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Collections
{
	/// <summary>Represents a collection of key/value pairs that are organized based on the hash code of the key.</summary>
	/// <filterpriority>1</filterpriority>
	[DebuggerDisplay("Count={Count}")]
	[ComVisible(true)]
	[DebuggerTypeProxy(typeof(CollectionDebuggerView))]
	[Serializable]
	public class Hashtable : IEnumerable, ICloneable, ISerializable, ICollection, IDictionary, IDeserializationCallback
	{
		private const int CHAIN_MARKER = -2147483648;

		private int inUse;

		private int modificationCount;

		private float loadFactor;

		private Hashtable.Slot[] table;

		private int[] hashes;

		private int threshold;

		private Hashtable.HashKeys hashKeys;

		private Hashtable.HashValues hashValues;

		private IHashCodeProvider hcpRef;

		private IComparer comparerRef;

		private SerializationInfo serializationInfo;

		private IEqualityComparer equalityComparer;

		private static readonly int[] primeTbl = new int[]
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

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Collections.Hashtable" /> class using the default initial capacity, load factor, hash code provider, and comparer.</summary>
		public Hashtable() : this(0, 1f)
		{
		}

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Collections.Hashtable" /> class using the specified initial capacity, load factor, hash code provider, and comparer.</summary>
		/// <param name="capacity">The approximate number of elements that the <see cref="T:System.Collections.Hashtable" /> object can initially contain. </param>
		/// <param name="loadFactor">A number in the range from 0.1 through 1.0 that is multiplied by the default value which provides the best performance. The result is the maximum ratio of elements to buckets.</param>
		/// <param name="hcp">The <see cref="T:System.Collections.IHashCodeProvider" /> object that supplies the hash codes for all keys in the <see cref="T:System.Collections.Hashtable" />.-or- null to use the default hash code provider, which is each key's implementation of <see cref="M:System.Object.GetHashCode" />. </param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> object to use to determine whether two keys are equal.-or- null to use the default comparer, which is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero.-or- <paramref name="loadFactor" /> is less than 0.1.-or- <paramref name="loadFactor" /> is greater than 1.0. </exception>
		[Obsolete("Please use Hashtable(int, float, IEqualityComparer) instead")]
		public Hashtable(int capacity, float loadFactor, IHashCodeProvider hcp, IComparer comparer)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", "negative capacity");
			}
			if (loadFactor < 0.1f || loadFactor > 1f || float.IsNaN(loadFactor))
			{
				throw new ArgumentOutOfRangeException("loadFactor", "load factor");
			}
			if (capacity == 0)
			{
				capacity++;
			}
			this.loadFactor = 0.75f * loadFactor;
			double num = (double)((float)capacity / this.loadFactor);
			if (num > 2147483647.0)
			{
				throw new ArgumentException("Size is too big");
			}
			int num2 = (int)num;
			num2 = Hashtable.ToPrime(num2);
			this.SetTable(new Hashtable.Slot[num2], new int[num2]);
			this.hcp = hcp;
			this.comparer = comparer;
			this.inUse = 0;
			this.modificationCount = 0;
		}

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Collections.Hashtable" /> class using the specified initial capacity and load factor, and the default hash code provider and comparer.</summary>
		/// <param name="capacity">The approximate number of elements that the <see cref="T:System.Collections.Hashtable" /> object can initially contain. </param>
		/// <param name="loadFactor">A number in the range from 0.1 through 1.0 that is multiplied by the default value which provides the best performance. The result is the maximum ratio of elements to buckets. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero.-or- <paramref name="loadFactor" /> is less than 0.1.-or- <paramref name="loadFactor" /> is greater than 1.0. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="capacity" /> is causing an overflow.</exception>
		public Hashtable(int capacity, float loadFactor) : this(capacity, loadFactor, null, null)
		{
		}

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Collections.Hashtable" /> class using the specified initial capacity, and the default load factor, hash code provider, and comparer.</summary>
		/// <param name="capacity">The approximate number of elements that the <see cref="T:System.Collections.Hashtable" /> object can initially contain. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero. </exception>
		public Hashtable(int capacity) : this(capacity, 1f)
		{
		}

		internal Hashtable(Hashtable source)
		{
			this.inUse = source.inUse;
			this.loadFactor = source.loadFactor;
			this.table = (Hashtable.Slot[])source.table.Clone();
			this.hashes = (int[])source.hashes.Clone();
			this.threshold = source.threshold;
			this.hcpRef = source.hcpRef;
			this.comparerRef = source.comparerRef;
			this.equalityComparer = source.equalityComparer;
		}

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Collections.Hashtable" /> class using the specified initial capacity, hash code provider, comparer, and the default load factor.</summary>
		/// <param name="capacity">The approximate number of elements that the <see cref="T:System.Collections.Hashtable" /> object can initially contain. </param>
		/// <param name="hcp">The <see cref="T:System.Collections.IHashCodeProvider" /> object that supplies the hash codes for all keys in the <see cref="T:System.Collections.Hashtable" />.-or- null to use the default hash code provider, which is each key's implementation of <see cref="M:System.Object.GetHashCode" />. </param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> object to use to determine whether two keys are equal.-or- null to use the default comparer, which is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero. </exception>
		[Obsolete("Please use Hashtable(int, IEqualityComparer) instead")]
		public Hashtable(int capacity, IHashCodeProvider hcp, IComparer comparer) : this(capacity, 1f, hcp, comparer)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Hashtable" /> class by copying the elements from the specified dictionary to the new <see cref="T:System.Collections.Hashtable" /> object. The new <see cref="T:System.Collections.Hashtable" /> object has an initial capacity equal to the number of elements copied, and uses the specified load factor, hash code provider, and comparer.</summary>
		/// <param name="d">The <see cref="T:System.Collections.IDictionary" /> object to copy to a new <see cref="T:System.Collections.Hashtable" /> object.</param>
		/// <param name="loadFactor">A number in the range from 0.1 through 1.0 that is multiplied by the default value which provides the best performance. The result is the maximum ratio of elements to buckets.</param>
		/// <param name="hcp">The <see cref="T:System.Collections.IHashCodeProvider" /> object that supplies the hash codes for all keys in the <see cref="T:System.Collections.Hashtable" />.-or- null to use the default hash code provider, which is each key's implementation of <see cref="M:System.Object.GetHashCode" />. </param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> object to use to determine whether two keys are equal.-or- null to use the default comparer, which is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="d" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="loadFactor" /> is less than 0.1.-or- <paramref name="loadFactor" /> is greater than 1.0. </exception>
		[Obsolete("Please use Hashtable(IDictionary, float, IEqualityComparer) instead")]
		public Hashtable(IDictionary d, float loadFactor, IHashCodeProvider hcp, IComparer comparer) : this((d == null) ? 0 : d.Count, loadFactor, hcp, comparer)
		{
			if (d == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			IDictionaryEnumerator enumerator = d.GetEnumerator();
			while (enumerator.MoveNext())
			{
				this.Add(enumerator.Key, enumerator.Value);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Hashtable" /> class by copying the elements from the specified dictionary to the new <see cref="T:System.Collections.Hashtable" /> object. The new <see cref="T:System.Collections.Hashtable" /> object has an initial capacity equal to the number of elements copied, and uses the specified load factor, and the default hash code provider and comparer.</summary>
		/// <param name="d">The <see cref="T:System.Collections.IDictionary" /> object to copy to a new <see cref="T:System.Collections.Hashtable" /> object.</param>
		/// <param name="loadFactor">A number in the range from 0.1 through 1.0 that is multiplied by the default value which provides the best performance. The result is the maximum ratio of elements to buckets.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="d" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="loadFactor" /> is less than 0.1.-or- <paramref name="loadFactor" /> is greater than 1.0. </exception>
		public Hashtable(IDictionary d, float loadFactor) : this(d, loadFactor, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Hashtable" /> class by copying the elements from the specified dictionary to the new <see cref="T:System.Collections.Hashtable" /> object. The new <see cref="T:System.Collections.Hashtable" /> object has an initial capacity equal to the number of elements copied, and uses the default load factor, hash code provider, and comparer.</summary>
		/// <param name="d">The <see cref="T:System.Collections.IDictionary" /> object to copy to a new <see cref="T:System.Collections.Hashtable" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="d" /> is null. </exception>
		public Hashtable(IDictionary d) : this(d, 1f)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Hashtable" /> class by copying the elements from the specified dictionary to the new <see cref="T:System.Collections.Hashtable" /> object. The new <see cref="T:System.Collections.Hashtable" /> object has an initial capacity equal to the number of elements copied, and uses the default load factor, and the specified hash code provider and comparer.</summary>
		/// <param name="d">The <see cref="T:System.Collections.IDictionary" /> object to copy to a new <see cref="T:System.Collections.Hashtable" /> object.</param>
		/// <param name="hcp">The <see cref="T:System.Collections.IHashCodeProvider" /> object that supplies the hash codes for all keys in the <see cref="T:System.Collections.Hashtable" />.-or- null to use the default hash code provider, which is each key's implementation of <see cref="M:System.Object.GetHashCode" />. </param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> object to use to determine whether two keys are equal.-or- null to use the default comparer, which is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="d" /> is null. </exception>
		[Obsolete("Please use Hashtable(IDictionary, IEqualityComparer) instead")]
		public Hashtable(IDictionary d, IHashCodeProvider hcp, IComparer comparer) : this(d, 1f, hcp, comparer)
		{
		}

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Collections.Hashtable" /> class using the default initial capacity and load factor, and the specified hash code provider and comparer.</summary>
		/// <param name="hcp">The <see cref="T:System.Collections.IHashCodeProvider" /> object that supplies the hash codes for all keys in the <see cref="T:System.Collections.Hashtable" /> object.-or- null to use the default hash code provider, which is each key's implementation of <see cref="M:System.Object.GetHashCode" />.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> object to use to determine whether two keys are equal.-or- null to use the default comparer, which is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />.</param>
		[Obsolete("Please use Hashtable(IEqualityComparer) instead")]
		public Hashtable(IHashCodeProvider hcp, IComparer comparer) : this(1, 1f, hcp, comparer)
		{
		}

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Collections.Hashtable" /> class that is serializable using the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> objects.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Hashtable" /> object.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Hashtable" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null. </exception>
		public Hashtable(SerializationInfo info, StreamingContext context)
		{
			this.serializationInfo = info;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Hashtable" /> class by copying the elements from the specified dictionary to a new <see cref="T:System.Collections.Hashtable" /> object. The new <see cref="T:System.Collections.Hashtable" /> object has an initial capacity equal to the number of elements copied, and uses the default load factor and the specified <see cref="T:System.Collections.IEqualityComparer" /> object.</summary>
		/// <param name="d">The <see cref="T:System.Collections.IDictionary" /> object to copy to a new <see cref="T:System.Collections.Hashtable" /> object.</param>
		/// <param name="equalityComparer">The <see cref="T:System.Collections.IEqualityComparer" /> object that defines the hash code provider and the comparer to use with the <see cref="T:System.Collections.Hashtable" />.-or- null to use the default hash code provider and the default comparer. The default hash code provider is each key's implementation of <see cref="M:System.Object.GetHashCode" /> and the default comparer is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="d" /> is null. </exception>
		public Hashtable(IDictionary d, IEqualityComparer equalityComparer) : this(d, 1f, equalityComparer)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Hashtable" /> class by copying the elements from the specified dictionary to the new <see cref="T:System.Collections.Hashtable" /> object. The new <see cref="T:System.Collections.Hashtable" /> object has an initial capacity equal to the number of elements copied, and uses the specified load factor and <see cref="T:System.Collections.IEqualityComparer" /> object.</summary>
		/// <param name="d">The <see cref="T:System.Collections.IDictionary" /> object to copy to a new <see cref="T:System.Collections.Hashtable" /> object.</param>
		/// <param name="loadFactor">A number in the range from 0.1 through 1.0 that is multiplied by the default value which provides the best performance. The result is the maximum ratio of elements to buckets.</param>
		/// <param name="equalityComparer">The <see cref="T:System.Collections.IEqualityComparer" /> object that defines the hash code provider and the comparer to use with the <see cref="T:System.Collections.Hashtable" />.-or- null to use the default hash code provider and the default comparer. The default hash code provider is each key's implementation of <see cref="M:System.Object.GetHashCode" /> and the default comparer is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="d" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="loadFactor" /> is less than 0.1.-or- <paramref name="loadFactor" /> is greater than 1.0. </exception>
		public Hashtable(IDictionary d, float loadFactor, IEqualityComparer equalityComparer) : this(d, loadFactor)
		{
			this.equalityComparer = equalityComparer;
		}

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Collections.Hashtable" /> class using the default initial capacity and load factor, and the specified <see cref="T:System.Collections.IEqualityComparer" /> object.</summary>
		/// <param name="equalityComparer">The <see cref="T:System.Collections.IEqualityComparer" /> object that defines the hash code provider and the comparer to use with the <see cref="T:System.Collections.Hashtable" /> object.-or- null to use the default hash code provider and the default comparer. The default hash code provider is each key's implementation of <see cref="M:System.Object.GetHashCode" /> and the default comparer is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />. </param>
		public Hashtable(IEqualityComparer equalityComparer) : this(1, 1f, equalityComparer)
		{
		}

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Collections.Hashtable" /> class using the specified initial capacity and <see cref="T:System.Collections.IEqualityComparer" />, and the default load factor.</summary>
		/// <param name="capacity">The approximate number of elements that the <see cref="T:System.Collections.Hashtable" /> object can initially contain. </param>
		/// <param name="equalityComparer">The <see cref="T:System.Collections.IEqualityComparer" /> object that defines the hash code provider and the comparer to use with the <see cref="T:System.Collections.Hashtable" />.-or- null to use the default hash code provider and the default comparer. The default hash code provider is each key's implementation of <see cref="M:System.Object.GetHashCode" /> and the default comparer is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero. </exception>
		public Hashtable(int capacity, IEqualityComparer equalityComparer) : this(capacity, 1f, equalityComparer)
		{
		}

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Collections.Hashtable" /> class using the specified initial capacity, load factor, and <see cref="T:System.Collections.IEqualityComparer" /> object.</summary>
		/// <param name="capacity">The approximate number of elements that the <see cref="T:System.Collections.Hashtable" /> object can initially contain. </param>
		/// <param name="loadFactor">A number in the range from 0.1 through 1.0 that is multiplied by the default value which provides the best performance. The result is the maximum ratio of elements to buckets.</param>
		/// <param name="equalityComparer">The <see cref="T:System.Collections.IEqualityComparer" /> object that defines the hash code provider and the comparer to use with the <see cref="T:System.Collections.Hashtable" />.-or- null to use the default hash code provider and the default comparer. The default hash code provider is each key's implementation of <see cref="M:System.Object.GetHashCode" /> and the default comparer is each key's implementation of <see cref="M:System.Object.Equals(System.Object)" />. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero.-or- <paramref name="loadFactor" /> is less than 0.1.-or- <paramref name="loadFactor" /> is greater than 1.0. </exception>
		public Hashtable(int capacity, float loadFactor, IEqualityComparer equalityComparer) : this(capacity, loadFactor)
		{
			this.equalityComparer = equalityComparer;
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Hashtable.Enumerator(this, Hashtable.EnumeratorMode.ENTRY_MODE);
		}

		/// <summary>Gets or sets the <see cref="T:System.Collections.IComparer" /> to use for the <see cref="T:System.Collections.Hashtable" />.</summary>
		/// <returns>The <see cref="T:System.Collections.IComparer" /> to use for the <see cref="T:System.Collections.Hashtable" />.</returns>
		/// <exception cref="T:System.ArgumentException">The property is set to a value, but the hash table was created using an <see cref="T:System.Collections.IEqualityComparer" />. </exception>
		[Obsolete("Please use EqualityComparer property.")]
		protected IComparer comparer
		{
			get
			{
				return this.comparerRef;
			}
			set
			{
				this.comparerRef = value;
			}
		}

		/// <summary>Gets or sets the object that can dispense hash codes.</summary>
		/// <returns>The object that can dispense hash codes.</returns>
		/// <exception cref="T:System.ArgumentException">The property is set to a value, but the hash table was created using an <see cref="T:System.Collections.IEqualityComparer" />. </exception>
		[Obsolete("Please use EqualityComparer property.")]
		protected IHashCodeProvider hcp
		{
			get
			{
				return this.hcpRef;
			}
			set
			{
				this.hcpRef = value;
			}
		}

		/// <summary>Gets the <see cref="T:System.Collections.IEqualityComparer" /> to use for the <see cref="T:System.Collections.Hashtable" />.</summary>
		/// <returns>The <see cref="T:System.Collections.IEqualityComparer" /> to use for the <see cref="T:System.Collections.Hashtable" />.</returns>
		/// <exception cref="T:System.ArgumentException">The property is set to a value, but the hash table was created using an <see cref="T:System.Collections.IHashCodeProvider" /> and an <see cref="T:System.Collections.IComparer" />. </exception>
		protected IEqualityComparer EqualityComparer
		{
			get
			{
				return this.equalityComparer;
			}
		}

		/// <summary>Gets the number of key/value pairs contained in the <see cref="T:System.Collections.Hashtable" />.</summary>
		/// <returns>The number of key/value pairs contained in the <see cref="T:System.Collections.Hashtable" />.</returns>
		/// <filterpriority>1</filterpriority>
		public virtual int Count
		{
			get
			{
				return this.inUse;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.Hashtable" /> is synchronized (thread safe).</summary>
		/// <returns>true if access to the <see cref="T:System.Collections.Hashtable" /> is synchronized (thread safe); otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.Hashtable" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.Hashtable" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Hashtable" /> has a fixed size.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Hashtable" /> has a fixed size; otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Hashtable" /> is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Hashtable" /> is read-only; otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the keys in the <see cref="T:System.Collections.Hashtable" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the keys in the <see cref="T:System.Collections.Hashtable" />.</returns>
		/// <filterpriority>1</filterpriority>
		public virtual ICollection Keys
		{
			get
			{
				if (this.hashKeys == null)
				{
					this.hashKeys = new Hashtable.HashKeys(this);
				}
				return this.hashKeys;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.Hashtable" />.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.Hashtable" />.</returns>
		/// <filterpriority>1</filterpriority>
		public virtual ICollection Values
		{
			get
			{
				if (this.hashValues == null)
				{
					this.hashValues = new Hashtable.HashValues(this);
				}
				return this.hashValues;
			}
		}

		/// <summary>Gets or sets the value associated with the specified key.</summary>
		/// <returns>The value associated with the specified key. If the specified key is not found, attempting to get it returns null, and attempting to set it creates a new element using the specified key.</returns>
		/// <param name="key">The key whose value to get or set. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Hashtable" /> is read-only.-or- The property is set, <paramref name="key" /> does not exist in the collection, and the <see cref="T:System.Collections.Hashtable" /> has a fixed size. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual object this[object key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", "null key");
				}
				Hashtable.Slot[] array = this.table;
				int[] array2 = this.hashes;
				uint num = (uint)array.Length;
				int num2 = this.GetHash(key) & int.MaxValue;
				uint num3 = (uint)num2;
				uint num4 = (uint)(((num2 >> 5) + 1) % (int)(num - 1u) + 1);
				for (uint num5 = num; num5 > 0u; num5 -= 1u)
				{
					num3 %= num;
					Hashtable.Slot slot = array[(int)((UIntPtr)num3)];
					int num6 = array2[(int)((UIntPtr)num3)];
					object key2 = slot.key;
					if (key2 == null)
					{
						break;
					}
					if (key2 == key || ((num6 & 2147483647) == num2 && this.KeyEquals(key, key2)))
					{
						return slot.value;
					}
					if ((num6 & -2147483648) == 0)
					{
						break;
					}
					num3 += num4;
				}
				return null;
			}
			set
			{
				this.PutImpl(key, value, true);
			}
		}

		/// <summary>Copies the <see cref="T:System.Collections.Hashtable" /> elements to a one-dimensional <see cref="T:System.Array" /> instance at the specified index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the <see cref="T:System.Collections.DictionaryEntry" /> objects copied from <see cref="T:System.Collections.Hashtable" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="arrayIndex" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.Hashtable" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.Hashtable" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void CopyTo(Array array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (array.Rank > 1)
			{
				throw new ArgumentException("array is multidimensional");
			}
			if (array.Length > 0 && arrayIndex >= array.Length)
			{
				throw new ArgumentException("arrayIndex is equal to or greater than array.Length");
			}
			if (arrayIndex + this.inUse > array.Length)
			{
				throw new ArgumentException("Not enough room from arrayIndex to end of array for this Hashtable");
			}
			IDictionaryEnumerator enumerator = this.GetEnumerator();
			int num = arrayIndex;
			while (enumerator.MoveNext())
			{
				array.SetValue(enumerator.Entry, num++);
			}
		}

		/// <summary>Adds an element with the specified key and value into the <see cref="T:System.Collections.Hashtable" />.</summary>
		/// <param name="key">The key of the element to add. </param>
		/// <param name="value">The value of the element to add. The value can be null. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Hashtable" />. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Hashtable" /> is read-only.-or- The <see cref="T:System.Collections.Hashtable" /> has a fixed size. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual void Add(object key, object value)
		{
			this.PutImpl(key, value, false);
		}

		/// <summary>Removes all elements from the <see cref="T:System.Collections.Hashtable" />.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Hashtable" /> is read-only. </exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public virtual void Clear()
		{
			for (int i = 0; i < this.table.Length; i++)
			{
				this.table[i].key = null;
				this.table[i].value = null;
				this.hashes[i] = 0;
			}
			this.inUse = 0;
			this.modificationCount++;
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Hashtable" /> contains a specific key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Hashtable" /> contains an element with the specified key; otherwise, false.</returns>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Hashtable" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual bool Contains(object key)
		{
			return this.Find(key) >= 0;
		}

		/// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> that iterates through the <see cref="T:System.Collections.Hashtable" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.Hashtable" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new Hashtable.Enumerator(this, Hashtable.EnumeratorMode.ENTRY_MODE);
		}

		/// <summary>Removes the element with the specified key from the <see cref="T:System.Collections.Hashtable" />.</summary>
		/// <param name="key">The key of the element to remove. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Hashtable" /> is read-only.-or- The <see cref="T:System.Collections.Hashtable" /> has a fixed size. </exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public virtual void Remove(object key)
		{
			int num = this.Find(key);
			if (num >= 0)
			{
				Hashtable.Slot[] array = this.table;
				int num2 = this.hashes[num];
				num2 &= int.MinValue;
				this.hashes[num] = num2;
				array[num].key = ((num2 == 0) ? null : Hashtable.KeyMarker.Removed);
				array[num].value = null;
				this.inUse--;
				this.modificationCount++;
			}
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Hashtable" /> contains a specific key.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Hashtable" /> contains an element with the specified key; otherwise, false.</returns>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Hashtable" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		/// <filterpriority>1</filterpriority>
		public virtual bool ContainsKey(object key)
		{
			return this.Contains(key);
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Hashtable" /> contains a specific value.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Hashtable" /> contains an element with the specified <paramref name="value" />; otherwise, false.</returns>
		/// <param name="value">The value to locate in the <see cref="T:System.Collections.Hashtable" />. The value can be null. </param>
		/// <filterpriority>1</filterpriority>
		public virtual bool ContainsValue(object value)
		{
			int num = this.table.Length;
			Hashtable.Slot[] array = this.table;
			if (value == null)
			{
				for (int i = 0; i < num; i++)
				{
					Hashtable.Slot slot = array[i];
					if (slot.key != null && slot.key != Hashtable.KeyMarker.Removed && slot.value == null)
					{
						return true;
					}
				}
			}
			else
			{
				for (int j = 0; j < num; j++)
				{
					Hashtable.Slot slot2 = array[j];
					if (slot2.key != null && slot2.key != Hashtable.KeyMarker.Removed && value.Equals(slot2.value))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>Creates a shallow copy of the <see cref="T:System.Collections.Hashtable" />.</summary>
		/// <returns>A shallow copy of the <see cref="T:System.Collections.Hashtable" />.</returns>
		/// <filterpriority>1</filterpriority>
		public virtual object Clone()
		{
			return new Hashtable(this);
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.Collections.Hashtable" />.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Hashtable" />. </param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Hashtable" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The collection was modified.</exception>
		/// <filterpriority>2</filterpriority>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("LoadFactor", this.loadFactor);
			info.AddValue("Version", this.modificationCount);
			if (this.equalityComparer != null)
			{
				info.AddValue("KeyComparer", this.equalityComparer);
			}
			else
			{
				info.AddValue("Comparer", this.comparerRef);
			}
			if (this.hcpRef != null)
			{
				info.AddValue("HashCodeProvider", this.hcpRef);
			}
			info.AddValue("HashSize", this.table.Length);
			object[] array = new object[this.inUse];
			this.CopyToArray(array, 0, Hashtable.EnumeratorMode.KEY_MODE);
			object[] array2 = new object[this.inUse];
			this.CopyToArray(array2, 0, Hashtable.EnumeratorMode.VALUE_MODE);
			info.AddValue("Keys", array);
			info.AddValue("Values", array2);
			info.AddValue("equalityComparer", this.equalityComparer);
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and raises the deserialization event when the deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event. </param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object associated with the current <see cref="T:System.Collections.Hashtable" /> is invalid. </exception>
		/// <filterpriority>2</filterpriority>
		[MonoTODO("Serialize equalityComparer")]
		public virtual void OnDeserialization(object sender)
		{
			if (this.serializationInfo == null)
			{
				return;
			}
			this.loadFactor = (float)this.serializationInfo.GetValue("LoadFactor", typeof(float));
			this.modificationCount = (int)this.serializationInfo.GetValue("Version", typeof(int));
			try
			{
				this.equalityComparer = (IEqualityComparer)this.serializationInfo.GetValue("KeyComparer", typeof(object));
			}
			catch
			{
			}
			if (this.equalityComparer == null)
			{
				this.comparerRef = (IComparer)this.serializationInfo.GetValue("Comparer", typeof(object));
			}
			try
			{
				this.hcpRef = (IHashCodeProvider)this.serializationInfo.GetValue("HashCodeProvider", typeof(object));
			}
			catch
			{
			}
			int num = (int)this.serializationInfo.GetValue("HashSize", typeof(int));
			object[] array = (object[])this.serializationInfo.GetValue("Keys", typeof(object[]));
			object[] array2 = (object[])this.serializationInfo.GetValue("Values", typeof(object[]));
			if (array.Length != array2.Length)
			{
				throw new SerializationException("Keys and values of uneven size");
			}
			num = Hashtable.ToPrime(num);
			this.SetTable(new Hashtable.Slot[num], new int[num]);
			for (int i = 0; i < array.Length; i++)
			{
				this.Add(array[i], array2[i]);
			}
			this.AdjustThreshold();
			this.serializationInfo = null;
		}

		/// <summary>Returns a synchronized (thread safe) wrapper for the <see cref="T:System.Collections.Hashtable" />.</summary>
		/// <returns>A synchronized (thread safe) wrapper for the <see cref="T:System.Collections.Hashtable" />.</returns>
		/// <param name="table">The <see cref="T:System.Collections.Hashtable" /> to synchronize. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="table" /> is null. </exception>
		/// <filterpriority>1</filterpriority>
		public static Hashtable Synchronized(Hashtable table)
		{
			if (table == null)
			{
				throw new ArgumentNullException("table");
			}
			return new Hashtable.SyncHashtable(table);
		}

		/// <summary>Returns the hash code for the specified key.</summary>
		/// <returns>The hash code for <paramref name="key" />.</returns>
		/// <param name="key">The <see cref="T:System.Object" /> for which a hash code is to be returned. </param>
		/// <exception cref="T:System.NullReferenceException">
		///   <paramref name="key" /> is null. </exception>
		protected virtual int GetHash(object key)
		{
			if (this.equalityComparer != null)
			{
				return this.equalityComparer.GetHashCode(key);
			}
			if (this.hcpRef == null)
			{
				return key.GetHashCode();
			}
			return this.hcpRef.GetHashCode(key);
		}

		/// <summary>Compares a specific <see cref="T:System.Object" /> with a specific key in the <see cref="T:System.Collections.Hashtable" />.</summary>
		/// <returns>true if <paramref name="item" /> and <paramref name="key" /> are equal; otherwise, false.</returns>
		/// <param name="item">The <see cref="T:System.Object" /> to compare with <paramref name="key" />. </param>
		/// <param name="key">The key in the <see cref="T:System.Collections.Hashtable" /> to compare with <paramref name="item" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="item" /> is null.-or- <paramref name="key" /> is null. </exception>
		protected virtual bool KeyEquals(object item, object key)
		{
			if (key == Hashtable.KeyMarker.Removed)
			{
				return false;
			}
			if (this.equalityComparer != null)
			{
				return this.equalityComparer.Equals(item, key);
			}
			if (this.comparerRef == null)
			{
				return item.Equals(key);
			}
			return this.comparerRef.Compare(item, key) == 0;
		}

		private void AdjustThreshold()
		{
			int num = this.table.Length;
			this.threshold = (int)((float)num * this.loadFactor);
			if (this.threshold >= num)
			{
				this.threshold = num - 1;
			}
		}

		private void SetTable(Hashtable.Slot[] table, int[] hashes)
		{
			if (table == null)
			{
				throw new ArgumentNullException("table");
			}
			this.table = table;
			this.hashes = hashes;
			this.AdjustThreshold();
		}

		private int Find(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", "null key");
			}
			Hashtable.Slot[] array = this.table;
			int[] array2 = this.hashes;
			uint num = (uint)array.Length;
			int num2 = this.GetHash(key) & int.MaxValue;
			uint num3 = (uint)num2;
			uint num4 = (uint)(((num2 >> 5) + 1) % (int)(num - 1u) + 1);
			for (uint num5 = num; num5 > 0u; num5 -= 1u)
			{
				num3 %= num;
				Hashtable.Slot slot = array[(int)((UIntPtr)num3)];
				int num6 = array2[(int)((UIntPtr)num3)];
				object key2 = slot.key;
				if (key2 == null)
				{
					break;
				}
				if (key2 == key || ((num6 & 2147483647) == num2 && this.KeyEquals(key, key2)))
				{
					return (int)num3;
				}
				if ((num6 & -2147483648) == 0)
				{
					break;
				}
				num3 += num4;
			}
			return -1;
		}

		private void Rehash()
		{
			int num = this.table.Length;
			uint num2 = (uint)Hashtable.ToPrime(num << 1 | 1);
			Hashtable.Slot[] array = new Hashtable.Slot[num2];
			Hashtable.Slot[] array2 = this.table;
			int[] array3 = new int[num2];
			int[] array4 = this.hashes;
			for (int i = 0; i < num; i++)
			{
				Hashtable.Slot slot = array2[i];
				if (slot.key != null)
				{
					int num3 = array4[i] & int.MaxValue;
					uint num4 = (uint)num3;
					uint num5 = (uint)(((num3 >> 5) + 1) % (int)(num2 - 1u) + 1);
					uint num6 = num4 % num2;
					while (array[(int)((UIntPtr)num6)].key != null)
					{
						array3[(int)((UIntPtr)num6)] |= int.MinValue;
						num4 += num5;
						num6 = num4 % num2;
					}
					array[(int)((UIntPtr)num6)].key = slot.key;
					array[(int)((UIntPtr)num6)].value = slot.value;
					array3[(int)((UIntPtr)num6)] |= num3;
				}
			}
			this.modificationCount++;
			this.SetTable(array, array3);
		}

		private void PutImpl(object key, object value, bool overwrite)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", "null key");
			}
			if (this.inUse >= this.threshold)
			{
				this.Rehash();
			}
			uint num = (uint)this.table.Length;
			int num2 = this.GetHash(key) & int.MaxValue;
			uint num3 = (uint)num2;
			uint num4 = ((num3 >> 5) + 1u) % (num - 1u) + 1u;
			Hashtable.Slot[] array = this.table;
			int[] array2 = this.hashes;
			int num5 = -1;
			int num6 = 0;
			while ((long)num6 < (long)((ulong)num))
			{
				int num7 = (int)(num3 % num);
				Hashtable.Slot slot = array[num7];
				int num8 = array2[num7];
				if (num5 == -1 && slot.key == Hashtable.KeyMarker.Removed && (num8 & -2147483648) != 0)
				{
					num5 = num7;
				}
				if (slot.key == null || (slot.key == Hashtable.KeyMarker.Removed && (num8 & -2147483648) == 0))
				{
					if (num5 == -1)
					{
						num5 = num7;
					}
					break;
				}
				if ((num8 & 2147483647) == num2 && this.KeyEquals(key, slot.key))
				{
					if (overwrite)
					{
						array[num7].value = value;
						this.modificationCount++;
						return;
					}
					throw new ArgumentException("Key duplication when adding: " + key);
				}
				else
				{
					if (num5 == -1)
					{
						array2[num7] |= int.MinValue;
					}
					num3 += num4;
					num6++;
				}
			}
			if (num5 != -1)
			{
				array[num5].key = key;
				array[num5].value = value;
				array2[num5] |= num2;
				this.inUse++;
				this.modificationCount++;
			}
		}

		private void CopyToArray(Array arr, int i, Hashtable.EnumeratorMode mode)
		{
			IEnumerator enumerator = new Hashtable.Enumerator(this, mode);
			while (enumerator.MoveNext())
			{
				object value = enumerator.Current;
				arr.SetValue(value, i++);
			}
		}

		internal static bool TestPrime(int x)
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

		internal static int CalcPrime(int x)
		{
			for (int i = (x & -2) - 1; i < 2147483647; i += 2)
			{
				if (Hashtable.TestPrime(i))
				{
					return i;
				}
			}
			return x;
		}

		internal static int ToPrime(int x)
		{
			for (int i = 0; i < Hashtable.primeTbl.Length; i++)
			{
				if (x <= Hashtable.primeTbl[i])
				{
					return Hashtable.primeTbl[i];
				}
			}
			return Hashtable.CalcPrime(x);
		}

		[Serializable]
		internal struct Slot
		{
			internal object key;

			internal object value;
		}

		[Serializable]
		internal class KeyMarker
		{
			public static readonly Hashtable.KeyMarker Removed = new Hashtable.KeyMarker();
		}

		private enum EnumeratorMode
		{
			KEY_MODE,
			VALUE_MODE,
			ENTRY_MODE
		}

		[Serializable]
		private sealed class Enumerator : IEnumerator, IDictionaryEnumerator
		{
			private Hashtable host;

			private int stamp;

			private int pos;

			private int size;

			private Hashtable.EnumeratorMode mode;

			private object currentKey;

			private object currentValue;

			private static readonly string xstr = "Hashtable.Enumerator: snapshot out of sync.";

			public Enumerator(Hashtable host, Hashtable.EnumeratorMode mode)
			{
				this.host = host;
				this.stamp = host.modificationCount;
				this.size = host.table.Length;
				this.mode = mode;
				this.Reset();
			}

			public Enumerator(Hashtable host) : this(host, Hashtable.EnumeratorMode.KEY_MODE)
			{
			}

			private void FailFast()
			{
				if (this.host.modificationCount != this.stamp)
				{
					throw new InvalidOperationException(Hashtable.Enumerator.xstr);
				}
			}

			public void Reset()
			{
				this.FailFast();
				this.pos = -1;
				this.currentKey = null;
				this.currentValue = null;
			}

			public bool MoveNext()
			{
				this.FailFast();
				if (this.pos < this.size)
				{
					while (++this.pos < this.size)
					{
						Hashtable.Slot slot = this.host.table[this.pos];
						if (slot.key != null && slot.key != Hashtable.KeyMarker.Removed)
						{
							this.currentKey = slot.key;
							this.currentValue = slot.value;
							return true;
						}
					}
				}
				this.currentKey = null;
				this.currentValue = null;
				return false;
			}

			public DictionaryEntry Entry
			{
				get
				{
					if (this.currentKey == null)
					{
						throw new InvalidOperationException();
					}
					this.FailFast();
					return new DictionaryEntry(this.currentKey, this.currentValue);
				}
			}

			public object Key
			{
				get
				{
					if (this.currentKey == null)
					{
						throw new InvalidOperationException();
					}
					this.FailFast();
					return this.currentKey;
				}
			}

			public object Value
			{
				get
				{
					if (this.currentKey == null)
					{
						throw new InvalidOperationException();
					}
					this.FailFast();
					return this.currentValue;
				}
			}

			public object Current
			{
				get
				{
					if (this.currentKey == null)
					{
						throw new InvalidOperationException();
					}
					switch (this.mode)
					{
					case Hashtable.EnumeratorMode.KEY_MODE:
						return this.currentKey;
					case Hashtable.EnumeratorMode.VALUE_MODE:
						return this.currentValue;
					case Hashtable.EnumeratorMode.ENTRY_MODE:
						return new DictionaryEntry(this.currentKey, this.currentValue);
					default:
						throw new Exception("should never happen");
					}
				}
			}
		}

		[DebuggerTypeProxy(typeof(CollectionDebuggerView))]
		[DebuggerDisplay("Count={Count}")]
		[Serializable]
		private class HashKeys : IEnumerable, ICollection
		{
			private Hashtable host;

			public HashKeys(Hashtable host)
			{
				if (host == null)
				{
					throw new ArgumentNullException();
				}
				this.host = host;
			}

			public virtual int Count
			{
				get
				{
					return this.host.Count;
				}
			}

			public virtual bool IsSynchronized
			{
				get
				{
					return this.host.IsSynchronized;
				}
			}

			public virtual object SyncRoot
			{
				get
				{
					return this.host.SyncRoot;
				}
			}

			public virtual void CopyTo(Array array, int arrayIndex)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new ArgumentException("array");
				}
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException("arrayIndex");
				}
				if (array.Length - arrayIndex < this.Count)
				{
					throw new ArgumentException("not enough space");
				}
				this.host.CopyToArray(array, arrayIndex, Hashtable.EnumeratorMode.KEY_MODE);
			}

			public virtual IEnumerator GetEnumerator()
			{
				return new Hashtable.Enumerator(this.host, Hashtable.EnumeratorMode.KEY_MODE);
			}
		}

		[DebuggerTypeProxy(typeof(CollectionDebuggerView))]
		[DebuggerDisplay("Count={Count}")]
		[Serializable]
		private class HashValues : IEnumerable, ICollection
		{
			private Hashtable host;

			public HashValues(Hashtable host)
			{
				if (host == null)
				{
					throw new ArgumentNullException();
				}
				this.host = host;
			}

			public virtual int Count
			{
				get
				{
					return this.host.Count;
				}
			}

			public virtual bool IsSynchronized
			{
				get
				{
					return this.host.IsSynchronized;
				}
			}

			public virtual object SyncRoot
			{
				get
				{
					return this.host.SyncRoot;
				}
			}

			public virtual void CopyTo(Array array, int arrayIndex)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new ArgumentException("array");
				}
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException("arrayIndex");
				}
				if (array.Length - arrayIndex < this.Count)
				{
					throw new ArgumentException("not enough space");
				}
				this.host.CopyToArray(array, arrayIndex, Hashtable.EnumeratorMode.VALUE_MODE);
			}

			public virtual IEnumerator GetEnumerator()
			{
				return new Hashtable.Enumerator(this.host, Hashtable.EnumeratorMode.VALUE_MODE);
			}
		}

		[Serializable]
		private class SyncHashtable : Hashtable, IEnumerable
		{
			private Hashtable host;

			public SyncHashtable(Hashtable host)
			{
				if (host == null)
				{
					throw new ArgumentNullException();
				}
				this.host = host;
			}

			internal SyncHashtable(SerializationInfo info, StreamingContext context)
			{
				this.host = (Hashtable)info.GetValue("ParentTable", typeof(Hashtable));
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Hashtable.Enumerator(this.host, Hashtable.EnumeratorMode.ENTRY_MODE);
			}

			public override void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				info.AddValue("ParentTable", this.host);
			}

			public override int Count
			{
				get
				{
					return this.host.Count;
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
					return this.host.SyncRoot;
				}
			}

			public override bool IsFixedSize
			{
				get
				{
					return this.host.IsFixedSize;
				}
			}

			public override bool IsReadOnly
			{
				get
				{
					return this.host.IsReadOnly;
				}
			}

			public override ICollection Keys
			{
				get
				{
					ICollection result = null;
					object syncRoot = this.host.SyncRoot;
					lock (syncRoot)
					{
						result = this.host.Keys;
					}
					return result;
				}
			}

			public override ICollection Values
			{
				get
				{
					ICollection result = null;
					object syncRoot = this.host.SyncRoot;
					lock (syncRoot)
					{
						result = this.host.Values;
					}
					return result;
				}
			}

			public override object this[object key]
			{
				get
				{
					return this.host[key];
				}
				set
				{
					object syncRoot = this.host.SyncRoot;
					lock (syncRoot)
					{
						this.host[key] = value;
					}
				}
			}

			public override void CopyTo(Array array, int arrayIndex)
			{
				this.host.CopyTo(array, arrayIndex);
			}

			public override void Add(object key, object value)
			{
				object syncRoot = this.host.SyncRoot;
				lock (syncRoot)
				{
					this.host.Add(key, value);
				}
			}

			public override void Clear()
			{
				object syncRoot = this.host.SyncRoot;
				lock (syncRoot)
				{
					this.host.Clear();
				}
			}

			public override bool Contains(object key)
			{
				return this.host.Find(key) >= 0;
			}

			public override IDictionaryEnumerator GetEnumerator()
			{
				return new Hashtable.Enumerator(this.host, Hashtable.EnumeratorMode.ENTRY_MODE);
			}

			public override void Remove(object key)
			{
				object syncRoot = this.host.SyncRoot;
				lock (syncRoot)
				{
					this.host.Remove(key);
				}
			}

			public override bool ContainsKey(object key)
			{
				return this.host.Contains(key);
			}

			public override bool ContainsValue(object value)
			{
				return this.host.ContainsValue(value);
			}

			public override object Clone()
			{
				object syncRoot = this.host.SyncRoot;
				object result;
				lock (syncRoot)
				{
					result = new Hashtable.SyncHashtable((Hashtable)this.host.Clone());
				}
				return result;
			}
		}
	}
}
