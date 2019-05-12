using System;

namespace System.Xml
{
	/// <summary>Table of atomized string objects.</summary>
	public abstract class XmlNameTable
	{
		/// <summary>When overridden in a derived class, atomizes the specified string and adds it to the XmlNameTable.</summary>
		/// <returns>The new atomized string or the existing one if it already exists.</returns>
		/// <param name="array">The name to add. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		public abstract string Add(string name);

		/// <summary>When overridden in a derived class, atomizes the specified string and adds it to the XmlNameTable.</summary>
		/// <returns>The new atomized string or the existing one if it already exists. If length is zero, String.Empty is returned.</returns>
		/// <param name="array">The character array containing the name to add. </param>
		/// <param name="offset">Zero-based index into the array specifying the first character of the name. </param>
		/// <param name="length">The number of characters in the name. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">0 &gt; <paramref name="offset" />-or- <paramref name="offset" /> &gt;= <paramref name="array" />.Length -or- <paramref name="length" /> &gt; <paramref name="array" />.Length The above conditions do not cause an exception to be thrown if <paramref name="length" /> =0. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="length" /> &lt; 0. </exception>
		public abstract string Add(char[] buffer, int offset, int length);

		/// <summary>When overridden in a derived class, gets the atomized string containing the same value as the specified string.</summary>
		/// <returns>The atomized string or null if the string has not already been atomized.</returns>
		/// <param name="array">The name to look up. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		public abstract string Get(string name);

		/// <summary>When overridden in a derived class, gets the atomized string containing the same characters as the specified range of characters in the given array.</summary>
		/// <returns>The atomized string or null if the string has not already been atomized. If <paramref name="length" /> is zero, String.Empty is returned.</returns>
		/// <param name="array">The character array containing the name to look up. </param>
		/// <param name="offset">The zero-based index into the array specifying the first character of the name. </param>
		/// <param name="length">The number of characters in the name. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">0 &gt; <paramref name="offset" />-or- <paramref name="offset" /> &gt;= <paramref name="array" />.Length -or- <paramref name="length" /> &gt; <paramref name="array" />.Length The above conditions do not cause an exception to be thrown if <paramref name="length" /> =0. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="length" /> &lt; 0. </exception>
		public abstract string Get(char[] buffer, int offset, int length);
	}
}
