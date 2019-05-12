using System;
using System.Collections;

namespace System.Security.Cryptography
{
	/// <summary>Represents a collection of <see cref="T:System.Security.Cryptography.Oid" /> objects. This class cannot be inherited.</summary>
	public sealed class OidCollection : ICollection, IEnumerable
	{
		private ArrayList _list;

		private bool _readOnly;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.OidCollection" /> class.</summary>
		public OidCollection()
		{
			this._list = new ArrayList();
		}

		/// <summary>Copies the <see cref="T:System.Security.Cryptography.OidCollection" /> object into an array.</summary>
		/// <param name="array">The array to copy the <see cref="T:System.Security.Cryptography.OidCollection" /> object to.</param>
		/// <param name="index">The location where the copy operation starts.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> cannot be a multidimensional array.-or-The length of <paramref name="array" /> is an invalid offset length.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="index" /> is out range.</exception>
		void ICollection.CopyTo(Array array, int index)
		{
			this._list.CopyTo(array, index);
		}

		/// <summary>Returns an <see cref="T:System.Security.Cryptography.OidEnumerator" /> object that can be used to navigate the <see cref="T:System.Security.Cryptography.OidCollection" /> object.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.OidEnumerator" /> object that can be used to navigate the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new OidEnumerator(this);
		}

		/// <summary>Gets the number of <see cref="T:System.Security.Cryptography.Oid" /> objects in a collection. </summary>
		/// <returns>The number of <see cref="T:System.Security.Cryptography.Oid" /> objects in a collection.</returns>
		public int Count
		{
			get
			{
				return this._list.Count;
			}
		}

		/// <summary>Gets a value that indicates whether access to the <see cref="T:System.Security.Cryptography.OidCollection" /> object is thread safe.</summary>
		/// <returns>false in all cases.</returns>
		public bool IsSynchronized
		{
			get
			{
				return this._list.IsSynchronized;
			}
		}

		/// <summary>Gets an <see cref="T:System.Security.Cryptography.Oid" /> object from the <see cref="T:System.Security.Cryptography.OidCollection" /> object.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.Oid" /> object.</returns>
		/// <param name="index">The location of the <see cref="T:System.Security.Cryptography.Oid" /> object in the collection.</param>
		public Oid this[int index]
		{
			get
			{
				return (Oid)this._list[index];
			}
		}

		/// <summary>Gets the first <see cref="T:System.Security.Cryptography.Oid" /> object that contains a value of the <see cref="P:System.Security.Cryptography.Oid.Value" /> property or a value of the <see cref="P:System.Security.Cryptography.Oid.FriendlyName" /> property that matches the specified string value from the <see cref="T:System.Security.Cryptography.OidCollection" /> object.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.Oid" /> object.</returns>
		/// <param name="oid">A string that represents a <see cref="P:System.Security.Cryptography.Oid.Value" /> property or a <see cref="P:System.Security.Cryptography.Oid.FriendlyName" /> property.</param>
		public Oid this[string oid]
		{
			get
			{
				foreach (object obj in this._list)
				{
					Oid oid2 = (Oid)obj;
					if (oid2.Value == oid)
					{
						return oid2;
					}
				}
				return null;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Security.Cryptography.OidCollection" /> object.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Security.Cryptography.OidCollection" /> object.</returns>
		public object SyncRoot
		{
			get
			{
				return this._list.SyncRoot;
			}
		}

		/// <summary>Adds an <see cref="T:System.Security.Cryptography.Oid" /> object to the <see cref="T:System.Security.Cryptography.OidCollection" /> object.</summary>
		/// <returns>The index of the added <see cref="T:System.Security.Cryptography.Oid" /> object.</returns>
		/// <param name="oid">The <see cref="T:System.Security.Cryptography.Oid" /> object to add to the collection.</param>
		public int Add(Oid oid)
		{
			return (!this._readOnly) ? this._list.Add(oid) : 0;
		}

		/// <summary>Copies the <see cref="T:System.Security.Cryptography.OidCollection" /> object into an array.</summary>
		/// <param name="array">The array to copy the <see cref="T:System.Security.Cryptography.OidCollection" /> object into.</param>
		/// <param name="index">The location where the copy operation starts.</param>
		public void CopyTo(Oid[] array, int index)
		{
			this._list.CopyTo(array, index);
		}

		/// <summary>Returns an <see cref="T:System.Security.Cryptography.OidEnumerator" /> object that can be used to navigate the <see cref="T:System.Security.Cryptography.OidCollection" /> object.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.OidEnumerator" /> object.</returns>
		public OidEnumerator GetEnumerator()
		{
			return new OidEnumerator(this);
		}

		internal bool ReadOnly
		{
			get
			{
				return this._readOnly;
			}
			set
			{
				this._readOnly = value;
			}
		}

		internal OidCollection ReadOnlyCopy()
		{
			OidCollection oidCollection = new OidCollection();
			foreach (object obj in this._list)
			{
				Oid oid = (Oid)obj;
				oidCollection.Add(oid);
			}
			oidCollection._readOnly = true;
			return oidCollection;
		}
	}
}
