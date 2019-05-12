using System;

namespace System.Text.RegularExpressions
{
	/// <summary>Represents the results from a single subexpression capture. </summary>
	[Serializable]
	public class Capture
	{
		internal int index;

		internal int length;

		internal string text;

		internal Capture(string text) : this(text, 0, 0)
		{
		}

		internal Capture(string text, int index, int length)
		{
			this.text = text;
			this.index = index;
			this.length = length;
		}

		/// <summary>The position in the original string where the first character of the captured substring was found.</summary>
		/// <returns>The zero-based starting position in the original string where the captured substring was found.</returns>
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		/// <summary>The length of the captured substring.</summary>
		/// <returns>The length of the captured substring.</returns>
		public int Length
		{
			get
			{
				return this.length;
			}
		}

		/// <summary>Gets the captured substring from the input string.</summary>
		/// <returns>The actual substring that was captured by the match.</returns>
		public string Value
		{
			get
			{
				return (this.text != null) ? this.text.Substring(this.index, this.length) : string.Empty;
			}
		}

		/// <summary>Gets the captured substring from the input string.</summary>
		/// <returns>The actual substring that was captured by the match.</returns>
		public override string ToString()
		{
			return this.Value;
		}

		internal string Text
		{
			get
			{
				return this.text;
			}
		}
	}
}
