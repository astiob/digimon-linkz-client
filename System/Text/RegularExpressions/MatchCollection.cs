using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	/// <summary>Represents the set of successful matches found by iteratively applying a regular expression pattern to the input string.</summary>
	[Serializable]
	public class MatchCollection : ICollection, IEnumerable
	{
		private Match current;

		private ArrayList list;

		internal MatchCollection(Match start)
		{
			this.current = start;
			this.list = new ArrayList();
		}

		/// <summary>Gets the number of matches.</summary>
		/// <returns>The number of matches.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public int Count
		{
			get
			{
				return this.FullList.Count;
			}
		}

		/// <summary>Gets a value that indicates whether the collection is read only.</summary>
		/// <returns>This value of this property is always true.</returns>
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value indicating whether access to the collection is synchronized (thread-safe).</summary>
		/// <returns>The value of this property is always false.</returns>
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an individual member of the collection.</summary>
		/// <returns>The captured substring at position <paramref name="i" /> in the collection.</returns>
		/// <param name="i">Index into the Match collection. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="i" /> is less than 0 or greater than or equal to <see cref="P:System.Text.RegularExpressions.MatchCollection.Count" />. </exception>
		public virtual Match this[int i]
		{
			get
			{
				if (i < 0 || !this.TryToGet(i))
				{
					throw new ArgumentOutOfRangeException("i");
				}
				return (i >= this.list.Count) ? this.current : ((Match)this.list[i]);
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the collection. This property always returns the object itself.</returns>
		public object SyncRoot
		{
			get
			{
				return this.list;
			}
		}

		/// <summary>Copies all the elements of the collection to the given array starting at the given index.</summary>
		/// <param name="array">The array the collection is to be copied into. </param>
		/// <param name="arrayIndex">The position in the array where copying is to begin. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is a multi-dimensional array.</exception>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="arrayIndex" /> is outside the bounds of <paramref name="array" />.-or-<paramref name="arrayIndex" /> plus <see cref="P:System.Text.RegularExpressions.GroupCollection.Count" /> is outside the bounds of <paramref name="array" />.</exception>
		public void CopyTo(Array array, int index)
		{
			this.FullList.CopyTo(array, index);
		}

		/// <summary>Provides an enumerator in the same order as <see cref="P:System.Text.RegularExpressions.MatchCollection.Item(System.Int32)" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that contains all Match objects within the MatchCollection.</returns>
		public IEnumerator GetEnumerator()
		{
			IEnumerator result;
			if (this.current.Success)
			{
				IEnumerator enumerator = new MatchCollection.Enumerator(this);
				result = enumerator;
			}
			else
			{
				result = this.list.GetEnumerator();
			}
			return result;
		}

		private bool TryToGet(int i)
		{
			while (i > this.list.Count && this.current.Success)
			{
				this.list.Add(this.current);
				this.current = this.current.NextMatch();
			}
			return i < this.list.Count || this.current.Success;
		}

		private ICollection FullList
		{
			get
			{
				if (this.TryToGet(2147483647))
				{
					throw new SystemException("too many matches");
				}
				return this.list;
			}
		}

		private class Enumerator : IEnumerator
		{
			private int index;

			private MatchCollection coll;

			internal Enumerator(MatchCollection coll)
			{
				this.coll = coll;
				this.index = -1;
			}

			void IEnumerator.Reset()
			{
				this.index = -1;
			}

			object IEnumerator.Current
			{
				get
				{
					if (this.index < 0)
					{
						throw new InvalidOperationException("'Current' called before 'MoveNext()'");
					}
					if (this.index > this.coll.list.Count)
					{
						throw new SystemException("MatchCollection in invalid state");
					}
					if (this.index == this.coll.list.Count && !this.coll.current.Success)
					{
						throw new InvalidOperationException("'Current' called after 'MoveNext()' returned false");
					}
					return (this.index >= this.coll.list.Count) ? this.coll.current : this.coll.list[this.index];
				}
			}

			bool IEnumerator.MoveNext()
			{
				if (this.index > this.coll.list.Count)
				{
					throw new SystemException("MatchCollection in invalid state");
				}
				return (this.index != this.coll.list.Count || this.coll.current.Success) && this.coll.TryToGet(++this.index);
			}
		}
	}
}
