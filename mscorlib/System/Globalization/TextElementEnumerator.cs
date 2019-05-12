using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	/// <summary>Enumerates the text elements of a string. </summary>
	[ComVisible(true)]
	[Serializable]
	public class TextElementEnumerator : IEnumerator
	{
		private int index;

		private int elementindex;

		private int startpos;

		private string str;

		private string element;

		internal TextElementEnumerator(string str, int startpos)
		{
			this.index = -1;
			this.startpos = startpos;
			this.str = str.Substring(startpos);
			this.element = null;
		}

		/// <summary>Gets the current text element in the string.</summary>
		/// <returns>An object containing the current text element in the string.</returns>
		/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first text element of the string or after the last text element. </exception>
		public object Current
		{
			get
			{
				if (this.element == null)
				{
					throw new InvalidOperationException();
				}
				return this.element;
			}
		}

		/// <summary>Gets the index of the text element that the enumerator is currently positioned over.</summary>
		/// <returns>The index of the text element that the enumerator is currently positioned over.</returns>
		/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first text element of the string or after the last text element. </exception>
		public int ElementIndex
		{
			get
			{
				if (this.element == null)
				{
					throw new InvalidOperationException();
				}
				return this.elementindex + this.startpos;
			}
		}

		/// <summary>Gets the current text element in the string.</summary>
		/// <returns>A new string containing the current text element in the string being read.</returns>
		/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first text element of the string or after the last text element. </exception>
		public string GetTextElement()
		{
			if (this.element == null)
			{
				throw new InvalidOperationException();
			}
			return this.element;
		}

		/// <summary>Advances the enumerator to the next text element of the string.</summary>
		/// <returns>true if the enumerator was successfully advanced to the next text element; false if the enumerator has passed the end of the string.</returns>
		public bool MoveNext()
		{
			this.elementindex = this.index + 1;
			if (this.elementindex < this.str.Length)
			{
				this.element = StringInfo.GetNextTextElement(this.str, this.elementindex);
				this.index += this.element.Length;
				return true;
			}
			this.element = null;
			return false;
		}

		/// <summary>Sets the enumerator to its initial position, which is before the first text element in the string.</summary>
		public void Reset()
		{
			this.element = null;
			this.index = -1;
		}
	}
}
