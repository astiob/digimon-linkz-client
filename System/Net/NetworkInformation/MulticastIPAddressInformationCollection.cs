using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Net.NetworkInformation
{
	/// <summary>Stores a set of <see cref="T:System.Net.NetworkInformation.MulticastIPAddressInformation" /> types.</summary>
	public class MulticastIPAddressInformationCollection : IEnumerable, IEnumerable<MulticastIPAddressInformation>, ICollection<MulticastIPAddressInformation>
	{
		private List<MulticastIPAddressInformation> list = new List<MulticastIPAddressInformation>();

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.NetworkInformation.MulticastIPAddressInformationCollection" /> class.</summary>
		protected internal MulticastIPAddressInformationCollection()
		{
		}

		/// <summary>Returns an object that can be used to iterate through this collection.</summary>
		/// <returns>An object that implements the <see cref="T:System.Collections.IEnumerator" /> interface and provides access to the <see cref="T:System.Net.NetworkInformation.UnicastIPAddressInformation" /> types in this collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		/// <summary>Throws a <see cref="T:System.NotSupportedException" /> because the collection is read-only and elements cannot be added to the collection.</summary>
		/// <param name="address">The object to be added to the collection.</param>
		public virtual void Add(MulticastIPAddressInformation address)
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException("The collection is read-only.");
			}
			this.list.Add(address);
		}

		/// <summary>Throws a <see cref="T:System.NotSupportedException" /> because the collection is read-only and elements cannot be removed.</summary>
		public virtual void Clear()
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException("The collection is read-only.");
			}
			this.list.Clear();
		}

		/// <summary>Checks whether the collection contains the specified <see cref="T:System.Net.NetworkInformation.MulticastIPAddressInformation" /> object.</summary>
		/// <returns>true if the <see cref="T:System.Net.NetworkInformation.MulticastIPAddressInformation" /> object exists in the collection; otherwise, false.</returns>
		/// <param name="address">The <see cref="T:System.Net.NetworkInformation.MulticastIPAddressInformation" /> object to be searched in the collection.</param>
		public virtual bool Contains(MulticastIPAddressInformation address)
		{
			return this.list.Contains(address);
		}

		/// <summary>Copies the elements in this collection to a one-dimensional array of type <see cref="T:System.Net.NetworkInformation.MulticastIPAddressInformation" />.</summary>
		/// <param name="array">A one-dimensional array that receives a copy of the collection.</param>
		/// <param name="offset">The zero-based index in <paramref name="array" /> at which the copy begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.-or- The number of elements in this <see cref="T:System.Net.NetworkInformation.MulticastIPAddressInformation" /> is greater than the available space from <paramref name="count" /> to the end of the destination <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The elements in this <see cref="T:System.Net.NetworkInformation.MulticastIPAddressInformation" /> cannot be cast automatically to the type of the destination <paramref name="array" />. </exception>
		public virtual void CopyTo(MulticastIPAddressInformation[] array, int offset)
		{
			this.list.CopyTo(array, offset);
		}

		/// <summary>Returns an object that can be used to iterate through this collection.</summary>
		/// <returns>An object that implements the <see cref="T:System.Collections.IEnumerator" /> interface and provides access to the <see cref="T:System.Net.NetworkInformation.UnicastIPAddressInformation" /> types in this collection.</returns>
		public virtual IEnumerator<MulticastIPAddressInformation> GetEnumerator()
		{
			return ((IEnumerable<MulticastIPAddressInformation>)this.list).GetEnumerator();
		}

		/// <summary>Throws a <see cref="T:System.NotSupportedException" /> because the collection is read-only and elements cannot be removed.</summary>
		/// <returns>Always throws a <see cref="T:System.NotSupportedException" />.</returns>
		/// <param name="address">The object to be removed.</param>
		public virtual bool Remove(MulticastIPAddressInformation address)
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException("The collection is read-only.");
			}
			return this.list.Remove(address);
		}

		/// <summary>Gets the number of <see cref="T:System.Net.NetworkInformation.MulticastIPAddressInformation" /> types in this collection.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that contains the number of <see cref="T:System.Net.NetworkInformation.MulticastIPAddressInformation" /> types in this collection.</returns>
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
				return true;
			}
		}

		/// <summary>Gets the <see cref="T:System.Net.NetworkInformation.MulticastIPAddressInformation" /> at the specific index of the collection.</summary>
		/// <returns>The <see cref="T:System.Net.NetworkInformation.MulticastIPAddressInformation" /> at the specific index in the collection.</returns>
		/// <param name="index">The index of interest.</param>
		public virtual MulticastIPAddressInformation this[int index]
		{
			get
			{
				return this.list[index];
			}
		}
	}
}
