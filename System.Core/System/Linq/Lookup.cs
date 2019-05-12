using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
	/// <summary>Represents a collection of keys each mapped to one or more values.</summary>
	/// <typeparam name="TKey">The type of the keys in the <see cref="T:System.Linq.Lookup`2" />.</typeparam>
	/// <typeparam name="TElement">The type of the elements of each <see cref="T:System.Collections.Generic.IEnumerable`1" /> value in the <see cref="T:System.Linq.Lookup`2" />.</typeparam>
	/// <filterpriority>2</filterpriority>
	public class Lookup<TKey, TElement> : IEnumerable, IEnumerable<IGrouping<TKey, TElement>>, ILookup<TKey, TElement>
	{
		private IGrouping<TKey, TElement> nullGrouping;

		private Dictionary<TKey, IGrouping<TKey, TElement>> groups;

		internal Lookup(Dictionary<TKey, List<TElement>> lookup, IEnumerable<TElement> nullKeyElements)
		{
			this.groups = new Dictionary<TKey, IGrouping<TKey, TElement>>(lookup.Comparer);
			foreach (KeyValuePair<TKey, List<TElement>> keyValuePair in lookup)
			{
				this.groups.Add(keyValuePair.Key, new Grouping<TKey, TElement>(keyValuePair.Key, keyValuePair.Value));
			}
			if (nullKeyElements != null)
			{
				this.nullGrouping = new Grouping<TKey, TElement>(default(TKey), nullKeyElements);
			}
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Linq.Lookup`2" />. This class cannot be inherited.</summary>
		/// <returns>An enumerator for the <see cref="T:System.Linq.Lookup`2" />.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>Gets the number of key/value collection pairs in the <see cref="T:System.Linq.Lookup`2" />.</summary>
		/// <returns>The number of key/value collection pairs in the <see cref="T:System.Linq.Lookup`2" />.</returns>
		public int Count
		{
			get
			{
				return (this.nullGrouping != null) ? (this.groups.Count + 1) : this.groups.Count;
			}
		}

		/// <summary>Gets the collection of values indexed by the specified key.</summary>
		/// <returns>The collection of values indexed by the specified key.</returns>
		/// <param name="key">The key of the desired collection of values.</param>
		public IEnumerable<TElement> this[TKey key]
		{
			get
			{
				if (key == null && this.nullGrouping != null)
				{
					return this.nullGrouping;
				}
				IGrouping<TKey, TElement> result;
				if (key != null && this.groups.TryGetValue(key, out result))
				{
					return result;
				}
				return new TElement[0];
			}
		}

		/// <summary>Applies a transform function to each key and its associated values and returns the results.</summary>
		/// <returns>A collection that contains one value for each key/value collection pair in the <see cref="T:System.Linq.Lookup`2" />.</returns>
		/// <param name="resultSelector">A function to project a result value from each key and its associated values.</param>
		/// <typeparam name="TResult">The type of the result values produced by <paramref name="resultSelector" />.</typeparam>
		/// <filterpriority>2</filterpriority>
		public IEnumerable<TResult> ApplyResultSelector<TResult>(Func<TKey, IEnumerable<TElement>, TResult> selector)
		{
			if (this.nullGrouping != null)
			{
				yield return selector(this.nullGrouping.Key, this.nullGrouping);
			}
			foreach (IGrouping<TKey, TElement> group in this.groups.Values)
			{
				yield return selector(group.Key, group);
			}
			yield break;
		}

		/// <summary>Determines whether a specified key is in the <see cref="T:System.Linq.Lookup`2" />.</summary>
		/// <returns>true if <paramref name="key" /> is in the <see cref="T:System.Linq.Lookup`2" />; otherwise, false.</returns>
		/// <param name="key">The key to find in the <see cref="T:System.Linq.Lookup`2" />.</param>
		public bool Contains(TKey key)
		{
			return (key == null) ? (this.nullGrouping != null) : this.groups.ContainsKey(key);
		}

		/// <summary>Returns a generic enumerator that iterates through the <see cref="T:System.Linq.Lookup`2" />.</summary>
		/// <returns>An enumerator for the <see cref="T:System.Linq.Lookup`2" />.</returns>
		public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
		{
			if (this.nullGrouping != null)
			{
				yield return this.nullGrouping;
			}
			foreach (IGrouping<TKey, TElement> g in this.groups.Values)
			{
				yield return g;
			}
			yield break;
		}
	}
}
