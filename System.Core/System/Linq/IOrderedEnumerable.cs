using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
	/// <summary>Represents a sorted sequence.</summary>
	/// <typeparam name="TElement">The type of the elements of the sequence.</typeparam>
	/// <filterpriority>2</filterpriority>
	public interface IOrderedEnumerable<TElement> : IEnumerable, IEnumerable<TElement>
	{
		/// <summary>Performs a subsequent ordering on the elements of an <see cref="T:System.Linq.IOrderedEnumerable`1" /> according to a key.</summary>
		/// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1" /> whose elements are sorted according to a key.</returns>
		/// <param name="keySelector">The <see cref="T:System.Func`2" /> used to extract the key for each element.</param>
		/// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> used to compare keys for placement in the returned sequence.</param>
		/// <param name="descending">true to sort the elements in descending order; false to sort the elements in ascending order.</param>
		/// <typeparam name="TKey">The type of the key produced by <paramref name="keySelector" />.</typeparam>
		/// <filterpriority>2</filterpriority>
		IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> selector, IComparer<TKey> comparer, bool descending);
	}
}
