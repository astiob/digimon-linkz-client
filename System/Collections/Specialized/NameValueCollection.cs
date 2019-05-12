using System;
using System.Runtime.Serialization;
using System.Text;

namespace System.Collections.Specialized
{
	/// <summary>Represents a collection of associated <see cref="T:System.String" /> keys and <see cref="T:System.String" /> values that can be accessed either with the key or with the index. </summary>
	[Serializable]
	public class NameValueCollection : NameObjectCollectionBase
	{
		private string[] cachedAllKeys;

		private string[] cachedAll;

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.NameValueCollection" /> class that is empty, has the default initial capacity and uses the default case-insensitive hash code provider and the default case-insensitive comparer.</summary>
		public NameValueCollection()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.NameValueCollection" /> class that is empty, has the specified initial capacity and uses the default case-insensitive hash code provider and the default case-insensitive comparer.</summary>
		/// <param name="capacity">The initial number of entries that the <see cref="T:System.Collections.Specialized.NameValueCollection" /> can contain.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero.</exception>
		public NameValueCollection(int capacity) : base(capacity)
		{
		}

		/// <summary>Copies the entries from the specified <see cref="T:System.Collections.Specialized.NameValueCollection" /> to a new <see cref="T:System.Collections.Specialized.NameValueCollection" /> with the same initial capacity as the number of entries copied and using the same hash code provider and the same comparer as the source collection.</summary>
		/// <param name="col">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to copy to the new <see cref="T:System.Collections.Specialized.NameValueCollection" /> instance.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="col" /> is null.</exception>
		public NameValueCollection(NameValueCollection col)
		{
			IEqualityComparer equalityComparer2;
			if (col == null)
			{
				IEqualityComparer equalityComparer = null;
				equalityComparer2 = equalityComparer;
			}
			else
			{
				equalityComparer2 = col.EqualityComparer;
			}
			IComparer comparer2;
			if (col == null)
			{
				IComparer comparer = null;
				comparer2 = comparer;
			}
			else
			{
				comparer2 = col.Comparer;
			}
			IHashCodeProvider hcp;
			if (col == null)
			{
				IHashCodeProvider hashCodeProvider = null;
				hcp = hashCodeProvider;
			}
			else
			{
				hcp = col.HashCodeProvider;
			}
			base..ctor(equalityComparer2, comparer2, hcp);
			if (col == null)
			{
				throw new ArgumentNullException("col");
			}
			this.Add(col);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.NameValueCollection" /> class that is empty, has the default initial capacity and uses the specified hash code provider and the specified comparer.</summary>
		/// <param name="hashProvider">The <see cref="T:System.Collections.IHashCodeProvider" /> that will supply the hash codes for all keys in the <see cref="T:System.Collections.Specialized.NameValueCollection" />.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> to use to determine whether two keys are equal.</param>
		[Obsolete("Use NameValueCollection (IEqualityComparer)")]
		public NameValueCollection(IHashCodeProvider hashProvider, IComparer comparer) : base(hashProvider, comparer)
		{
		}

		/// <summary>Copies the entries from the specified <see cref="T:System.Collections.Specialized.NameValueCollection" /> to a new <see cref="T:System.Collections.Specialized.NameValueCollection" /> with the specified initial capacity or the same initial capacity as the number of entries copied, whichever is greater, and using the default case-insensitive hash code provider and the default case-insensitive comparer.</summary>
		/// <param name="capacity">The initial number of entries that the <see cref="T:System.Collections.Specialized.NameValueCollection" /> can contain.</param>
		/// <param name="col">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to copy to the new <see cref="T:System.Collections.Specialized.NameValueCollection" /> instance.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="col" /> is null.</exception>
		public NameValueCollection(int capacity, NameValueCollection col)
		{
			IHashCodeProvider hashProvider;
			if (col == null)
			{
				IHashCodeProvider hashCodeProvider = null;
				hashProvider = hashCodeProvider;
			}
			else
			{
				hashProvider = col.HashCodeProvider;
			}
			IComparer comparer2;
			if (col == null)
			{
				IComparer comparer = null;
				comparer2 = comparer;
			}
			else
			{
				comparer2 = col.Comparer;
			}
			base..ctor(capacity, hashProvider, comparer2);
			this.Add(col);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.NameValueCollection" /> class that is serializable and uses the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" />.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the new <see cref="T:System.Collections.Specialized.NameValueCollection" /> instance.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains the source and destination of the serialized stream associated with the new <see cref="T:System.Collections.Specialized.NameValueCollection" /> instance.</param>
		protected NameValueCollection(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.NameValueCollection" /> class that is empty, has the specified initial capacity and uses the specified hash code provider and the specified comparer.</summary>
		/// <param name="capacity">The initial number of entries that the <see cref="T:System.Collections.Specialized.NameValueCollection" /> can contain.</param>
		/// <param name="hashProvider">The <see cref="T:System.Collections.IHashCodeProvider" /> that will supply the hash codes for all keys in the <see cref="T:System.Collections.Specialized.NameValueCollection" />.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> to use to determine whether two keys are equal.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero.</exception>
		[Obsolete("Use NameValueCollection (IEqualityComparer)")]
		public NameValueCollection(int capacity, IHashCodeProvider hashProvider, IComparer comparer) : base(capacity, hashProvider, comparer)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.NameValueCollection" /> class that is empty, has the default initial capacity, and uses the specified <see cref="T:System.Collections.IEqualityComparer" /> object.</summary>
		/// <param name="equalityComparer">The <see cref="T:System.Collections.IEqualityComparer" /> object to use to determine whether two keys are equal and to generate hash codes for the keys in the collection.</param>
		public NameValueCollection(IEqualityComparer equalityComparer) : base(equalityComparer)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Specialized.NameValueCollection" /> class that is empty, has the specified initial capacity, and uses the specified <see cref="T:System.Collections.IEqualityComparer" /> object.</summary>
		/// <param name="capacity">The initial number of entries that the <see cref="T:System.Collections.Specialized.NameValueCollection" /> object can contain.</param>
		/// <param name="equalityComparer">The <see cref="T:System.Collections.IEqualityComparer" /> object to use to determine whether two keys are equal and to generate hash codes for the keys in the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is less than zero.</exception>
		public NameValueCollection(int capacity, IEqualityComparer equalityComparer) : base(capacity, equalityComparer)
		{
		}

		/// <summary>Gets all the keys in the <see cref="T:System.Collections.Specialized.NameValueCollection" />.</summary>
		/// <returns>A <see cref="T:System.String" /> array that contains all the keys of the <see cref="T:System.Collections.Specialized.NameValueCollection" />.</returns>
		public virtual string[] AllKeys
		{
			get
			{
				if (this.cachedAllKeys == null)
				{
					this.cachedAllKeys = base.BaseGetAllKeys();
				}
				return this.cachedAllKeys;
			}
		}

		/// <summary>Gets the entry at the specified index of the <see cref="T:System.Collections.Specialized.NameValueCollection" />.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the comma-separated list of values at the specified index of the collection.</returns>
		/// <param name="index">The zero-based index of the entry to locate in the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the valid range of indexes for the collection.</exception>
		public string this[int index]
		{
			get
			{
				return this.Get(index);
			}
		}

		/// <summary>Gets or sets the entry with the specified key in the <see cref="T:System.Collections.Specialized.NameValueCollection" />.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the comma-separated list of values associated with the specified key, if found; otherwise, null.</returns>
		/// <param name="name">The <see cref="T:System.String" /> key of the entry to locate. The key can be null.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only and the operation attempts to modify the collection. </exception>
		public string this[string name]
		{
			get
			{
				return this.Get(name);
			}
			set
			{
				this.Set(name, value);
			}
		}

		/// <summary>Copies the entries in the specified <see cref="T:System.Collections.Specialized.NameValueCollection" /> to the current <see cref="T:System.Collections.Specialized.NameValueCollection" />.</summary>
		/// <param name="c">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to copy to the current <see cref="T:System.Collections.Specialized.NameValueCollection" />.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="c" /> is null.</exception>
		public void Add(NameValueCollection c)
		{
			if (base.IsReadOnly)
			{
				throw new NotSupportedException("Collection is read-only");
			}
			if (c == null)
			{
				throw new ArgumentNullException("c");
			}
			this.InvalidateCachedArrays();
			int count = c.Count;
			for (int i = 0; i < count; i++)
			{
				string key = c.GetKey(i);
				ArrayList arrayList = (ArrayList)c.BaseGet(i);
				ArrayList arrayList2 = (ArrayList)base.BaseGet(key);
				if (arrayList2 != null && arrayList != null)
				{
					arrayList2.AddRange(arrayList);
				}
				else if (arrayList != null)
				{
					arrayList2 = new ArrayList(arrayList);
				}
				base.BaseSet(key, arrayList2);
			}
		}

		/// <summary>Adds an entry with the specified name and value to the <see cref="T:System.Collections.Specialized.NameValueCollection" />.</summary>
		/// <param name="name">The <see cref="T:System.String" /> key of the entry to add. The key can be null.</param>
		/// <param name="value">The <see cref="T:System.String" /> value of the entry to add. The value can be null.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only. </exception>
		public virtual void Add(string name, string val)
		{
			if (base.IsReadOnly)
			{
				throw new NotSupportedException("Collection is read-only");
			}
			this.InvalidateCachedArrays();
			ArrayList arrayList = (ArrayList)base.BaseGet(name);
			if (arrayList == null)
			{
				arrayList = new ArrayList();
				if (val != null)
				{
					arrayList.Add(val);
				}
				base.BaseAdd(name, arrayList);
			}
			else if (val != null)
			{
				arrayList.Add(val);
			}
		}

		/// <summary>Invalidates the cached arrays and removes all entries from the <see cref="T:System.Collections.Specialized.NameValueCollection" />.</summary>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public virtual void Clear()
		{
			if (base.IsReadOnly)
			{
				throw new NotSupportedException("Collection is read-only");
			}
			this.InvalidateCachedArrays();
			base.BaseClear();
		}

		/// <summary>Copies the entire <see cref="T:System.Collections.Specialized.NameValueCollection" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
		/// <param name="dest">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Specialized.NameValueCollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="dest" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="dest" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="dest" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.Specialized.NameValueCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="dest" />.</exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.Specialized.NameValueCollection" /> cannot be cast automatically to the type of the destination <paramref name="dest" />.</exception>
		public void CopyTo(Array dest, int index)
		{
			if (dest == null)
			{
				throw new ArgumentNullException("dest", "Null argument - dest");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "index is less than 0");
			}
			if (dest.Rank > 1)
			{
				throw new ArgumentException("dest", "multidim");
			}
			if (this.cachedAll == null)
			{
				this.RefreshCachedAll();
			}
			try
			{
				this.cachedAll.CopyTo(dest, index);
			}
			catch (ArrayTypeMismatchException)
			{
				throw new InvalidCastException();
			}
		}

		private void RefreshCachedAll()
		{
			this.cachedAll = null;
			int count = this.Count;
			this.cachedAll = new string[count];
			for (int i = 0; i < count; i++)
			{
				this.cachedAll[i] = this.Get(i);
			}
		}

		/// <summary>Gets the values at the specified index of the <see cref="T:System.Collections.Specialized.NameValueCollection" /> combined into one comma-separated list.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains a comma-separated list of the values at the specified index of the <see cref="T:System.Collections.Specialized.NameValueCollection" />, if found; otherwise, null.</returns>
		/// <param name="index">The zero-based index of the entry that contains the values to get from the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the valid range of indexes for the collection.</exception>
		public virtual string Get(int index)
		{
			ArrayList values = (ArrayList)base.BaseGet(index);
			return NameValueCollection.AsSingleString(values);
		}

		/// <summary>Gets the values associated with the specified key from the <see cref="T:System.Collections.Specialized.NameValueCollection" /> combined into one comma-separated list.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains a comma-separated list of the values associated with the specified key from the <see cref="T:System.Collections.Specialized.NameValueCollection" />, if found; otherwise, null.</returns>
		/// <param name="name">The <see cref="T:System.String" /> key of the entry that contains the values to get. The key can be null.</param>
		public virtual string Get(string name)
		{
			ArrayList values = (ArrayList)base.BaseGet(name);
			return NameValueCollection.AsSingleString(values);
		}

		private static string AsSingleString(ArrayList values)
		{
			if (values == null)
			{
				return null;
			}
			int count = values.Count;
			switch (count)
			{
			case 0:
				return null;
			case 1:
				return (string)values[0];
			case 2:
				return (string)values[0] + ',' + (string)values[1];
			default:
			{
				int num = count;
				for (int i = 0; i < count; i++)
				{
					num += ((string)values[i]).Length;
				}
				StringBuilder stringBuilder = new StringBuilder((string)values[0], num);
				for (int j = 1; j < count; j++)
				{
					stringBuilder.Append(',');
					stringBuilder.Append(values[j]);
				}
				return stringBuilder.ToString();
			}
			}
		}

		/// <summary>Gets the key at the specified index of the <see cref="T:System.Collections.Specialized.NameValueCollection" />.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the key at the specified index of the <see cref="T:System.Collections.Specialized.NameValueCollection" />, if found; otherwise, null.</returns>
		/// <param name="index">The zero-based index of the key to get from the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the valid range of indexes for the collection. </exception>
		public virtual string GetKey(int index)
		{
			return base.BaseGetKey(index);
		}

		/// <summary>Gets the values at the specified index of the <see cref="T:System.Collections.Specialized.NameValueCollection" />.</summary>
		/// <returns>A <see cref="T:System.String" /> array that contains the values at the specified index of the <see cref="T:System.Collections.Specialized.NameValueCollection" />, if found; otherwise, null.</returns>
		/// <param name="index">The zero-based index of the entry that contains the values to get from the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the valid range of indexes for the collection. </exception>
		public virtual string[] GetValues(int index)
		{
			ArrayList values = (ArrayList)base.BaseGet(index);
			return NameValueCollection.AsStringArray(values);
		}

		/// <summary>Gets the values associated with the specified key from the <see cref="T:System.Collections.Specialized.NameValueCollection" />.</summary>
		/// <returns>A <see cref="T:System.String" /> array that contains the values associated with the specified key from the <see cref="T:System.Collections.Specialized.NameValueCollection" />, if found; otherwise, null.</returns>
		/// <param name="name">The <see cref="T:System.String" /> key of the entry that contains the values to get. The key can be null.</param>
		public virtual string[] GetValues(string name)
		{
			ArrayList values = (ArrayList)base.BaseGet(name);
			return NameValueCollection.AsStringArray(values);
		}

		private static string[] AsStringArray(ArrayList values)
		{
			if (values == null)
			{
				return null;
			}
			int count = values.Count;
			if (count == 0)
			{
				return null;
			}
			string[] array = new string[count];
			values.CopyTo(array);
			return array;
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Specialized.NameValueCollection" /> contains keys that are not null.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Specialized.NameValueCollection" /> contains keys that are not null; otherwise, false.</returns>
		public bool HasKeys()
		{
			return base.BaseHasKeys();
		}

		/// <summary>Removes the entries with the specified key from the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</summary>
		/// <param name="name">The <see cref="T:System.String" /> key of the entry to remove. The key can be null.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		public virtual void Remove(string name)
		{
			if (base.IsReadOnly)
			{
				throw new NotSupportedException("Collection is read-only");
			}
			this.InvalidateCachedArrays();
			base.BaseRemove(name);
		}

		/// <summary>Sets the value of an entry in the <see cref="T:System.Collections.Specialized.NameValueCollection" />.</summary>
		/// <param name="name">The <see cref="T:System.String" /> key of the entry to add the new value to. The key can be null.</param>
		/// <param name="value">The <see cref="T:System.Object" /> that represents the new value to add to the specified entry. The value can be null.</param>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
		public virtual void Set(string name, string value)
		{
			if (base.IsReadOnly)
			{
				throw new NotSupportedException("Collection is read-only");
			}
			this.InvalidateCachedArrays();
			ArrayList arrayList = new ArrayList();
			if (value != null)
			{
				arrayList.Add(value);
				base.BaseSet(name, arrayList);
			}
			else
			{
				base.BaseSet(name, null);
			}
		}

		/// <summary>Resets the cached arrays of the collection to null.</summary>
		protected void InvalidateCachedArrays()
		{
			this.cachedAllKeys = null;
			this.cachedAll = null;
		}
	}
}
