using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Net.NetworkInformation
{
	/// <summary>Stores a set of <see cref="T:System.Net.IPAddress" /> types.</summary>
	public class IPAddressCollection : IEnumerable, ICollection<IPAddress>, IEnumerable<IPAddress>
	{
		private IList<IPAddress> list = new List<IPAddress>();

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.NetworkInformation.IPAddressCollection" /> class.</summary>
		protected internal IPAddressCollection()
		{
		}

		/// <summary>Returns an object that can be used to iterate through this collection.</summary>
		/// <returns>An object that implements the <see cref="T:System.Collections.IEnumerator" /> interface and provides access to the <see cref="T:System.Net.NetworkInformation.IPAddressCollection" /> types in this collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		internal void SetReadOnly()
		{
			if (!this.IsReadOnly)
			{
				this.list = ((List<IPAddress>)this.list).AsReadOnly();
			}
		}

		/// <summary>Throws a <see cref="T:System.NotSupportedException" /> because this operation is not supported for this collection.</summary>
		/// <param name="address">The object to be added to the collection.</param>
		public virtual void Add(IPAddress address)
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException("The collection is read-only.");
			}
			this.list.Add(address);
		}

		/// <summary>Throws a <see cref="T:System.NotSupportedException" /> because this operation is not supported for this collection.</summary>
		public virtual void Clear()
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException("The collection is read-only.");
			}
			this.list.Clear();
		}

		/// <summary>Checks whether the collection contains the specified <see cref="T:System.Net.IPAddress" /> object.</summary>
		/// <returns>true if the <see cref="T:System.Net.IPAddress" /> object exists in the collection; otherwise, false.</returns>
		/// <param name="address">The <see cref="T:System.Net.IPAddress" /> object to be searched in the collection.</param>
		public virtual bool Contains(IPAddress address)
		{
			return this.list.Contains(address);
		}

		/// <summary>Copies the elements in this collection to a one-dimensional array of type <see cref="T:System.Net.IPAddress" />.</summary>
		/// <param name="array">A one-dimensional array that receives a copy of the collection.</param>
		/// <param name="offset">The zero-based index in <paramref name="array" /> at which the copy begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or-The number of elements in this <see cref="T:System.Net.NetworkInformation.IPAddressCollection" /> is greater than the available space from <paramref name="offset" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The elements in this <see cref="T:System.Net.NetworkInformation.IPAddressCollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		public virtual void CopyTo(IPAddress[] array, int offset)
		{
			this.list.CopyTo(array, offset);
		}

		/// <summary>Returns an object that can be used to iterate through this collection.</summary>
		/// <returns>An object that implements the <see cref="T:System.Collections.IEnumerator" /> interface and provides access to the <see cref="T:System.Net.NetworkInformation.IPAddressCollection" /> types in this collection.</returns>
		public virtual IEnumerator<IPAddress> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		/// <summary>Throws a <see cref="T:System.NotSupportedException" /> because this operation is not supported for this collection.</summary>
		/// <returns>Always throws a <see cref="T:System.NotSupportedException" />.</returns>
		/// <param name="address">The object to be removed.</param>
		public virtual bool Remove(IPAddress address)
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException("The collection is read-only.");
			}
			return this.list.Remove(address);
		}

		/// <summary>Gets the number of <see cref="T:System.Net.IPAddress" /> types in this collection.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that contains the number of <see cref="T:System.Net.IPAddress" /> types in this collection.</returns>
		public virtual int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		/// <summary>Gets a value that indicates whether access to this collection is read-only.</summary>
		/// <returns>true in all cases.</returns>
		public virtual bool IsReadOnly
		{
			get
			{
				return this.list.IsReadOnly;
			}
		}

		/// <summary>Gets the <see cref="T:System.Net.IPAddress" /> at the specific index of the collection.</summary>
		/// <returns>The <see cref="T:System.Net.IPAddress" /> at the specific index in the collection.</returns>
		/// <param name="index">The index of interest.</param>
		public virtual IPAddress this[int index]
		{
			get
			{
				return this.list[index];
			}
		}
	}
}
