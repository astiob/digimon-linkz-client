using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	/// <summary>Provides functionality to split a string into text elements and to iterate through those text elements.</summary>
	[ComVisible(true)]
	[Serializable]
	public class StringInfo
	{
		private string s;

		private int length;

		/// <summary>Initializes a new instance of the <see cref="T:System.Globalization.StringInfo" /> class. </summary>
		public StringInfo()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Globalization.StringInfo" /> class to a specified string.</summary>
		/// <param name="value">A string to initialize this <see cref="T:System.Globalization.StringInfo" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null.</exception>
		public StringInfo(string value)
		{
			this.String = value;
		}

		/// <summary>Indicates whether the current <see cref="T:System.Globalization.StringInfo" /> object is equal to a specified object.</summary>
		/// <returns>true if the <paramref name="value" /> parameter is a <see cref="T:System.Globalization.StringInfo" /> object and its <see cref="P:System.Globalization.StringInfo.String" /> property equals the <see cref="P:System.Globalization.StringInfo.String" /> property of this <see cref="T:System.Globalization.StringInfo" /> object; otherwise, false.</returns>
		/// <param name="value">An object.</param>
		[ComVisible(false)]
		public override bool Equals(object value)
		{
			StringInfo stringInfo = value as StringInfo;
			return stringInfo != null && this.s == stringInfo.s;
		}

		/// <summary>Calculates a hash code for the value of the current <see cref="T:System.Globalization.StringInfo" /> object.</summary>
		/// <returns>A 32-bit signed integer hash code based on the string value of this <see cref="T:System.Globalization.StringInfo" /> object.</returns>
		[ComVisible(false)]
		public override int GetHashCode()
		{
			return this.s.GetHashCode();
		}

		/// <summary>Gets the number of text elements in the current <see cref="T:System.Globalization.StringInfo" /> object.</summary>
		/// <returns>The number of base characters, surrogate pairs, and combining character sequences in this <see cref="T:System.Globalization.StringInfo" /> object.</returns>
		public int LengthInTextElements
		{
			get
			{
				if (this.length < 0)
				{
					this.length = 0;
					int i = 0;
					while (i < this.s.Length)
					{
						i += StringInfo.GetNextTextElementLength(this.s, i);
						this.length++;
					}
				}
				return this.length;
			}
		}

		/// <summary>Gets or sets the value of the current <see cref="T:System.Globalization.StringInfo" /> object.</summary>
		/// <returns>The string that is the value of the current <see cref="T:System.Globalization.StringInfo" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value in a set operation is null.</exception>
		public string String
		{
			get
			{
				return this.s;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.length = -1;
				this.s = value;
			}
		}

		/// <summary>Retrieves a substring of text elements from the current <see cref="T:System.Globalization.StringInfo" /> object starting from a specified text element and continuing through the last text element.</summary>
		/// <returns>A substring of text elements in this <see cref="T:System.Globalization.StringInfo" /> object, starting from the text element index specified by the <paramref name="startingTextElement" /> parameter and continuing through the last text element in this object.</returns>
		/// <param name="startingTextElement">The zero-based index of a text element in this <see cref="T:System.Globalization.StringInfo" /> object.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startingTextElement" /> is less than zero.-or-The string that is the value of the current <see cref="T:System.Globalization.StringInfo" /> object is the empty string ("").</exception>
		public string SubstringByTextElements(int startingTextElement)
		{
			if (startingTextElement < 0 || this.s.Length == 0)
			{
				throw new ArgumentOutOfRangeException("startingTextElement");
			}
			int num = 0;
			for (int i = 0; i < startingTextElement; i++)
			{
				if (num >= this.s.Length)
				{
					throw new ArgumentOutOfRangeException("startingTextElement");
				}
				num += StringInfo.GetNextTextElementLength(this.s, num);
			}
			return this.s.Substring(num);
		}

		/// <summary>Retrieves a substring of text elements from the current <see cref="T:System.Globalization.StringInfo" /> object starting from a specified text element and continuing through the specified number of text elements.</summary>
		/// <returns>A substring of text elements in this <see cref="T:System.Globalization.StringInfo" /> object. The substring consists of the number of text elements specified by the <paramref name="lengthInTextElements" /> parameter and starts from the text element index specified by the <paramref name="startingTextElement" /> parameter.</returns>
		/// <param name="startingTextElement">The zero-based index of a text element in this <see cref="T:System.Globalization.StringInfo" /> object.</param>
		/// <param name="lengthInTextElements">The number of text elements to retrieve.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startingTextElement" /> is less than zero.-or-<paramref name="startingTextElement" /> is greater than or equal to the length of the string that is the value of the current <see cref="T:System.Globalization.StringInfo" /> object.-or-<paramref name="lengthInTextElements" /> is less than zero.-or-The string that is the value of the current <see cref="T:System.Globalization.StringInfo" /> object is the empty string ("").-or-<paramref name="startingTextElement" /> + <paramref name="lengthInTextElements" /> specify an index that is greater than the number of text elements in this <see cref="T:System.Globalization.StringInfo" /> object.</exception>
		public string SubstringByTextElements(int startingTextElement, int lengthInTextElements)
		{
			if (startingTextElement < 0 || this.s.Length == 0)
			{
				throw new ArgumentOutOfRangeException("startingTextElement");
			}
			if (lengthInTextElements < 0)
			{
				throw new ArgumentOutOfRangeException("lengthInTextElements");
			}
			int num = 0;
			for (int i = 0; i < startingTextElement; i++)
			{
				if (num >= this.s.Length)
				{
					throw new ArgumentOutOfRangeException("startingTextElement");
				}
				num += StringInfo.GetNextTextElementLength(this.s, num);
			}
			int num2 = num;
			for (int j = 0; j < lengthInTextElements; j++)
			{
				if (num >= this.s.Length)
				{
					throw new ArgumentOutOfRangeException("lengthInTextElements");
				}
				num += StringInfo.GetNextTextElementLength(this.s, num);
			}
			return this.s.Substring(num2, num - num2);
		}

		/// <summary>Gets the first text element in a specified string.</summary>
		/// <returns>A string containing the first text element in the specified string.</returns>
		/// <param name="str">The string from which to get the text element. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="str" /> is null. </exception>
		public static string GetNextTextElement(string str)
		{
			if (str == null || str.Length == 0)
			{
				throw new ArgumentNullException("string is null");
			}
			return StringInfo.GetNextTextElement(str, 0);
		}

		/// <summary>Gets the text element at the specified index of the specified string.</summary>
		/// <returns>A string containing the text element at the specified index of the specified string.</returns>
		/// <param name="str">The string from which to get the text element. </param>
		/// <param name="index">The zero-based index at which the text element starts. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="str" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for <paramref name="str" />. </exception>
		public static string GetNextTextElement(string str, int index)
		{
			int nextTextElementLength = StringInfo.GetNextTextElementLength(str, index);
			return (nextTextElementLength == 1) ? new string(str[index], 1) : str.Substring(index, nextTextElementLength);
		}

		private static int GetNextTextElementLength(string str, int index)
		{
			if (str == null)
			{
				throw new ArgumentNullException("string is null");
			}
			if (index >= str.Length)
			{
				return 0;
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("Index is not valid");
			}
			char c = str[index];
			UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
			if (unicodeCategory == UnicodeCategory.Surrogate)
			{
				if (c < '\ud800' || c > '\udbff')
				{
					return 1;
				}
				if (index + 1 < str.Length && str[index + 1] >= '\udc00' && str[index + 1] <= '\udfff')
				{
					return 2;
				}
				return 1;
			}
			else
			{
				if (unicodeCategory == UnicodeCategory.NonSpacingMark || unicodeCategory == UnicodeCategory.SpacingCombiningMark || unicodeCategory == UnicodeCategory.EnclosingMark)
				{
					return 1;
				}
				int num = 1;
				while (index + num < str.Length)
				{
					unicodeCategory = char.GetUnicodeCategory(str[index + num]);
					if (unicodeCategory != UnicodeCategory.NonSpacingMark && unicodeCategory != UnicodeCategory.SpacingCombiningMark && unicodeCategory != UnicodeCategory.EnclosingMark)
					{
						break;
					}
					num++;
				}
				return num;
			}
		}

		/// <summary>Returns an enumerator that iterates through the text elements of the entire string.</summary>
		/// <returns>A <see cref="T:System.Globalization.TextElementEnumerator" /> for the entire string.</returns>
		/// <param name="str">The string to iterate through. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="str" /> is null. </exception>
		public static TextElementEnumerator GetTextElementEnumerator(string str)
		{
			if (str == null || str.Length == 0)
			{
				throw new ArgumentNullException("string is null");
			}
			return new TextElementEnumerator(str, 0);
		}

		/// <summary>Returns an enumerator that iterates through the text elements of the string, starting at the specified index.</summary>
		/// <returns>A <see cref="T:System.Globalization.TextElementEnumerator" /> for the string starting at <paramref name="index" />.</returns>
		/// <param name="str">The string to iterate through. </param>
		/// <param name="index">The zero-based index at which to start iterating. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="str" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes for <paramref name="str" />. </exception>
		public static TextElementEnumerator GetTextElementEnumerator(string str, int index)
		{
			if (str == null)
			{
				throw new ArgumentNullException("string is null");
			}
			if (index < 0 || index >= str.Length)
			{
				throw new ArgumentOutOfRangeException("Index is not valid");
			}
			return new TextElementEnumerator(str, index);
		}

		/// <summary>Returns the indexes of each base character, high surrogate, or control character within the specified string.</summary>
		/// <returns>An array of integers that contains the zero-based indexes of each base character, high surrogate, or control character within the specified string.</returns>
		/// <param name="str">The string to search. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="str" /> is null. </exception>
		public static int[] ParseCombiningCharacters(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("string is null");
			}
			ArrayList arrayList = new ArrayList(str.Length);
			TextElementEnumerator textElementEnumerator = StringInfo.GetTextElementEnumerator(str);
			textElementEnumerator.Reset();
			while (textElementEnumerator.MoveNext())
			{
				arrayList.Add(textElementEnumerator.ElementIndex);
			}
			return (int[])arrayList.ToArray(typeof(int));
		}
	}
}
