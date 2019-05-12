using System;
using System.Collections;

namespace System.Security.Cryptography
{
	/// <summary>Provides the ability to navigate through an <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object. This class cannot be inherited.</summary>
	public sealed class AsnEncodedDataEnumerator : IEnumerator
	{
		private AsnEncodedDataCollection _collection;

		private int _position;

		internal AsnEncodedDataEnumerator(AsnEncodedDataCollection collection)
		{
			this._collection = collection;
			this._position = -1;
		}

		/// <summary>Gets the current <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object in an <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object.</summary>
		/// <returns>The current <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object.</returns>
		object IEnumerator.Current
		{
			get
			{
				if (this._position < 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				return this._collection[this._position];
			}
		}

		/// <summary>Gets the current <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object in an <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object.</summary>
		/// <returns>The current <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object in the collection.</returns>
		public AsnEncodedData Current
		{
			get
			{
				if (this._position < 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				return this._collection[this._position];
			}
		}

		/// <summary>Advances to the next <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object in an <see cref="T:System.Security.Cryptography.AsnEncodedDataCollection" /> object.</summary>
		/// <returns>true, if the enumerator was successfully advanced to the next element; false, if the enumerator has passed the end of the collection.</returns>
		/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
		public bool MoveNext()
		{
			if (++this._position < this._collection.Count)
			{
				return true;
			}
			this._position = this._collection.Count - 1;
			return false;
		}

		/// <summary>Sets an enumerator to its initial position.</summary>
		/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
		public void Reset()
		{
			this._position = -1;
		}
	}
}
