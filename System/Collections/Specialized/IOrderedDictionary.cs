using System;

namespace System.Collections.Specialized
{
	/// <summary>Represents an indexed collection of key/value pairs.</summary>
	public interface IOrderedDictionary : ICollection, IDictionary, IEnumerable
	{
		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Specialized.IOrderedDictionary" /> collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> for the entire <see cref="T:System.Collections.Specialized.IOrderedDictionary" /> collection.</returns>
		IDictionaryEnumerator GetEnumerator();

		/// <summary>Inserts a key/value pair into the collection at the specified index.</summary>
		/// <param name="index">The zero-based index at which the key/value pair should be inserted.</param>
		/// <param name="key">The object to use as the key of the element to add.</param>
		/// <param name="value">The object to use as the value of the element to add.  The value can be null.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than 0.-or-<paramref name="index" /> is greater than <see cref="P:System.Collections.ICollection.Count" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Specialized.IOrderedDictionary" /> collection.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Specialized.IOrderedDictionary" /> collection is read-only.-or-The <see cref="T:System.Collections.Specialized.IOrderedDictionary" /> collection has a fixed size.</exception>
		void Insert(int idx, object key, object value);

		/// <summary>Removes the element at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than 0.-or- <paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.ICollection.Count" />. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Specialized.IOrderedDictionary" /> collection is read-only.-or- The <see cref="T:System.Collections.Specialized.IOrderedDictionary" /> collection has a fixed size. </exception>
		void RemoveAt(int idx);

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <returns>The element at the specified index.</returns>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than 0.-or- <paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.ICollection.Count" />. </exception>
		object this[int idx]
		{
			get;
			set;
		}
	}
}
