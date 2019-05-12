using System;
using System.Collections;

namespace System.Security.Cryptography
{
	/// <summary>Represents a collection of <see cref="T:System.Security.Cryptography.AsnEncodedData" /> objects. This class cannot be inherited.</summary>
	public sealed class AsnEncodedDataCollection : ICollection, IEnumerable
	{
		private ArrayList _list;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> class.</summary>
		public AsnEncodedDataCollection()
		{
			this._list = new ArrayList();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> class and adds an <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object to the collection.</summary>
		/// <param name="asnEncodedData">The <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object to add to the collection.</param>
		public AsnEncodedDataCollection(AsnEncodedData asnEncodedData)
		{
			this._list = new ArrayList();
			this._list.Add(asnEncodedData);
		}

		/// <summary>Copies the <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object into an array.</summary>
		/// <param name="array">The array that the <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object is to be copied into.</param>
		/// <param name="index">The location where the copy operation starts.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is a multidimensional array, which is not supported by this method.</exception>
		/// <exception cref="T:System.ArgumentException">The length for <paramref name="index" /> is invalid.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length for <paramref name="index" /> is out of range.</exception>
		void ICollection.CopyTo(Array array, int index)
		{
			this._list.CopyTo(array, index);
		}

		/// <summary>Returns an <see cref="T:System.Security.Cryptography.AsnEncodedDataEnumerator" /> object that can be used to navigate the <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.AsnEncodedDataEnumerator" /> object that can be used to navigate the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new AsnEncodedDataEnumerator(this);
		}

		/// <summary>Gets the number of <see cref="T:System.Security.Cryptography.AsnEncodedData" /> objects in a collection.</summary>
		/// <returns>The number of <see cref="T:System.Security.Cryptography.AsnEncodedData" /> objects.</returns>
		public int Count
		{
			get
			{
				return this._list.Count;
			}
		}

		/// <summary>Gets a value that indicates whether access to the <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object is thread safe.</summary>
		/// <returns>false in all cases.</returns>
		public bool IsSynchronized
		{
			get
			{
				return this._list.IsSynchronized;
			}
		}

		/// <summary>Gets an <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object from the <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object.</returns>
		/// <param name="index">The location in the collection.</param>
		public AsnEncodedData this[int index]
		{
			get
			{
				return (AsnEncodedData)this._list[index];
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object.</summary>
		/// <returns>An object used to synchronize access to the <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object.</returns>
		public object SyncRoot
		{
			get
			{
				return this._list.SyncRoot;
			}
		}

		/// <summary>Adds an <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object to the <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object.</summary>
		/// <returns>The index of the added <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object.</returns>
		/// <param name="asnEncodedData">The <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object to add to the collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asnEncodedData" /> is null.</exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">Neither of the OIDs are null and the OIDs do not match.</exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">One of the OIDs is null and the OIDs do not match.</exception>
		public int Add(AsnEncodedData asnEncodedData)
		{
			return this._list.Add(asnEncodedData);
		}

		/// <summary>Copies the <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object into an array.</summary>
		/// <param name="array">The array that the <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object is to be copied into.</param>
		/// <param name="index">The location where the copy operation starts.</param>
		public void CopyTo(AsnEncodedData[] array, int index)
		{
			this._list.CopyTo(array, index);
		}

		/// <summary>Returns an <see cref="T:System.Security.Cryptography.AsnEncodedDataEnumerator" /> object that can be used to navigate the <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.AsnEncodedDataEnumerator" /> object.</returns>
		public AsnEncodedDataEnumerator GetEnumerator()
		{
			return new AsnEncodedDataEnumerator(this);
		}

		/// <summary>Removes an <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object from the <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object.</summary>
		/// <param name="asnEncodedData">The <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object to remove.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asnEncodedData" /> is null.</exception>
		public void Remove(AsnEncodedData asnEncodedData)
		{
			this._list.Remove(asnEncodedData);
		}
	}
}
