using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Supports iterating over a <see cref="T:System.String" /> object and reading its individual characters. This class cannot be inherited.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public sealed class CharEnumerator : IEnumerator, IDisposable, ICloneable, IEnumerator<char>
	{
		private string str;

		private int index;

		private int length;

		internal CharEnumerator(string s)
		{
			this.str = s;
			this.index = -1;
			this.length = s.Length;
		}

		/// <summary>Gets the currently referenced character in the string enumerated by this <see cref="T:System.CharEnumerator" /> object. For a description of this member, see <see cref="P:System.Collections.IEnumerator.Current" />. </summary>
		/// <returns>The boxed Unicode character currently referenced by this <see cref="T:System.CharEnumerator" /> object.</returns>
		/// <exception cref="T:System.InvalidOperationException">Enumeration has not started.-or-Enumeration has ended.</exception>
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.CharEnumerator" /> class.</summary>
		void IDisposable.Dispose()
		{
		}

		/// <summary>Gets the currently referenced character in the string enumerated by this <see cref="T:System.CharEnumerator" /> object.</summary>
		/// <returns>The Unicode character currently referenced by this <see cref="T:System.CharEnumerator" /> object.</returns>
		/// <exception cref="T:System.InvalidOperationException">The index is invalid; that is, it is before the first or after the last character of the enumerated string. </exception>
		/// <filterpriority>2</filterpriority>
		public char Current
		{
			get
			{
				if (this.index == -1 || this.index >= this.length)
				{
					throw new InvalidOperationException(Locale.GetText("The position is not valid."));
				}
				return this.str[this.index];
			}
		}

		/// <summary>Creates a copy of the current <see cref="T:System.CharEnumerator" /> object.</summary>
		/// <returns>An <see cref="T:System.Object" /> that is a copy of the current <see cref="T:System.CharEnumerator" /> object.</returns>
		/// <filterpriority>2</filterpriority>
		public object Clone()
		{
			return new CharEnumerator(this.str)
			{
				index = this.index
			};
		}

		/// <summary>Increments the internal index of the current <see cref="T:System.CharEnumerator" /> object to the next character of the enumerated string.</summary>
		/// <returns>true if the index is successfully incremented and within the enumerated string; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		public bool MoveNext()
		{
			this.index++;
			if (this.index >= this.length)
			{
				this.index = this.length;
				return false;
			}
			return true;
		}

		/// <summary>Initializes the index to a position logically before the first character of the enumerated string.</summary>
		/// <filterpriority>2</filterpriority>
		public void Reset()
		{
			this.index = -1;
		}
	}
}
