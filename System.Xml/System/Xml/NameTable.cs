using System;

namespace System.Xml
{
	/// <summary>Implements a single-threaded <see cref="T:System.Xml.XmlNameTable" />.</summary>
	public class NameTable : XmlNameTable
	{
		private const int INITIAL_BUCKETS = 128;

		private int count = 128;

		private NameTable.Entry[] buckets = new NameTable.Entry[128];

		private int size;

		/// <summary>Atomizes the specified string and adds it to the NameTable.</summary>
		/// <returns>The atomized string or the existing string if one already exists in the NameTable. If <paramref name="len" /> is zero, String.Empty is returned.</returns>
		/// <param name="key">The character array containing the string to add. </param>
		/// <param name="start">The zero-based index into the array specifying the first character of the string. </param>
		/// <param name="len">The number of characters in the string. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">0 &gt; <paramref name="start" />-or- <paramref name="start" /> &gt;= <paramref name="key" />.Length -or- <paramref name="len" /> &gt;= <paramref name="key" />.Length The above conditions do not cause an exception to be thrown if <paramref name="len" /> =0. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="len" /> &lt; 0. </exception>
		public override string Add(char[] key, int start, int len)
		{
			if ((0 > start && start >= key.Length) || (0 > len && len >= key.Length - len))
			{
				throw new IndexOutOfRangeException("The Index is out of range.");
			}
			if (len == 0)
			{
				return string.Empty;
			}
			int num = 0;
			int num2 = start + len;
			for (int i = start; i < num2; i++)
			{
				num = (num << 5) - num + (int)key[i];
			}
			num &= int.MaxValue;
			for (NameTable.Entry entry = this.buckets[num % this.count]; entry != null; entry = entry.next)
			{
				if (entry.hash == num && entry.len == len && NameTable.StrEqArray(entry.str, key, start))
				{
					return entry.str;
				}
			}
			return this.AddEntry(new string(key, start, len), num);
		}

		/// <summary>Atomizes the specified string and adds it to the NameTable.</summary>
		/// <returns>The atomized string or the existing string if it already exists in the NameTable.</returns>
		/// <param name="key">The string to add. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null. </exception>
		public override string Add(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int length = key.Length;
			if (length == 0)
			{
				return string.Empty;
			}
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				num = (num << 5) - num + (int)key[i];
			}
			num &= int.MaxValue;
			for (NameTable.Entry entry = this.buckets[num % this.count]; entry != null; entry = entry.next)
			{
				if (entry.hash == num && entry.len == key.Length && entry.str == key)
				{
					return entry.str;
				}
			}
			return this.AddEntry(key, num);
		}

		/// <summary>Gets the atomized string containing the same characters as the specified range of characters in the given array.</summary>
		/// <returns>The atomized string or null if the string has not already been atomized. If <paramref name="len" /> is zero, String.Empty is returned.</returns>
		/// <param name="key">The character array containing the name to find. </param>
		/// <param name="start">The zero-based index into the array specifying the first character of the name. </param>
		/// <param name="len">The number of characters in the name. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">0 &gt; <paramref name="start" />-or- <paramref name="start" /> &gt;= <paramref name="key" />.Length -or- <paramref name="len" /> &gt;= <paramref name="key" />.Length The above conditions do not cause an exception to be thrown if <paramref name="len" /> =0. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="len" /> &lt; 0. </exception>
		public override string Get(char[] key, int start, int len)
		{
			if ((0 > start && start >= key.Length) || (0 > len && len >= key.Length - len))
			{
				throw new IndexOutOfRangeException("The Index is out of range.");
			}
			if (len == 0)
			{
				return string.Empty;
			}
			int num = 0;
			int num2 = start + len;
			for (int i = start; i < num2; i++)
			{
				num = (num << 5) - num + (int)key[i];
			}
			num &= int.MaxValue;
			for (NameTable.Entry entry = this.buckets[num % this.count]; entry != null; entry = entry.next)
			{
				if (entry.hash == num && entry.len == len && NameTable.StrEqArray(entry.str, key, start))
				{
					return entry.str;
				}
			}
			return null;
		}

		/// <summary>Gets the atomized string with the specified value.</summary>
		/// <returns>The atomized string object or null if the string has not already been atomized.</returns>
		/// <param name="value">The name to find. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		public override string Get(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int length = value.Length;
			if (length == 0)
			{
				return string.Empty;
			}
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				num = (num << 5) - num + (int)value[i];
			}
			num &= int.MaxValue;
			for (NameTable.Entry entry = this.buckets[num % this.count]; entry != null; entry = entry.next)
			{
				if (entry.hash == num && entry.len == value.Length && entry.str == value)
				{
					return entry.str;
				}
			}
			return null;
		}

		private string AddEntry(string str, int hash)
		{
			int num = hash % this.count;
			this.buckets[num] = new NameTable.Entry(str, hash, this.buckets[num]);
			if (this.size++ == this.count)
			{
				this.count <<= 1;
				int num2 = this.count - 1;
				NameTable.Entry[] array = new NameTable.Entry[this.count];
				for (int i = 0; i < this.buckets.Length; i++)
				{
					NameTable.Entry entry = this.buckets[i];
					NameTable.Entry next;
					for (NameTable.Entry entry2 = entry; entry2 != null; entry2 = next)
					{
						int num3 = entry2.hash & num2;
						next = entry2.next;
						entry2.next = array[num3];
						array[num3] = entry2;
					}
				}
				this.buckets = array;
			}
			return str;
		}

		private static bool StrEqArray(string str, char[] str2, int start)
		{
			int num = str.Length;
			num--;
			start += num;
			while (str[num] == str2[start])
			{
				num--;
				start--;
				if (num < 0)
				{
					return true;
				}
			}
			return false;
		}

		private class Entry
		{
			public string str;

			public int hash;

			public int len;

			public NameTable.Entry next;

			public Entry(string str, int hash, NameTable.Entry next)
			{
				this.str = str;
				this.len = str.Length;
				this.hash = hash;
				this.next = next;
			}
		}
	}
}
