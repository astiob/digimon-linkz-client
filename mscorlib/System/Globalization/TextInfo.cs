using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace System.Globalization
{
	/// <summary>Defines text properties and behaviors, such as casing, that are specific to a writing system. </summary>
	[ComVisible(true)]
	[MonoTODO("IDeserializationCallback isn't implemented.")]
	[Serializable]
	public class TextInfo : ICloneable, IDeserializationCallback
	{
		private string m_listSeparator;

		private bool m_isReadOnly;

		private string customCultureName;

		[NonSerialized]
		private int m_nDataItem;

		private bool m_useUserOverride;

		private int m_win32LangID;

		[NonSerialized]
		private readonly CultureInfo ci;

		[NonSerialized]
		private readonly bool handleDotI;

		[NonSerialized]
		private readonly TextInfo.Data data;

		internal unsafe TextInfo(CultureInfo ci, int lcid, void* data, bool read_only)
		{
			this.m_isReadOnly = read_only;
			this.m_win32LangID = lcid;
			this.ci = ci;
			if (data != null)
			{
				this.data = *(TextInfo.Data*)data;
			}
			else
			{
				this.data = default(TextInfo.Data);
				this.data.list_sep = 44;
			}
			CultureInfo cultureInfo = ci;
			while (cultureInfo.Parent != null && cultureInfo.Parent.LCID != 127 && cultureInfo.Parent != cultureInfo)
			{
				cultureInfo = cultureInfo.Parent;
			}
			if (cultureInfo != null)
			{
				int lcid2 = cultureInfo.LCID;
				if (lcid2 == 31 || lcid2 == 44)
				{
					this.handleDotI = true;
				}
			}
		}

		private TextInfo(TextInfo textInfo)
		{
			this.m_win32LangID = textInfo.m_win32LangID;
			this.m_nDataItem = textInfo.m_nDataItem;
			this.m_useUserOverride = textInfo.m_useUserOverride;
			this.m_listSeparator = textInfo.ListSeparator;
			this.customCultureName = textInfo.CultureName;
			this.ci = textInfo.ci;
			this.handleDotI = textInfo.handleDotI;
			this.data = textInfo.data;
		}

		/// <summary>Raises the deserialization event when deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event. </param>
		[MonoTODO]
		void IDeserializationCallback.OnDeserialization(object sender)
		{
		}

		/// <summary>Gets the American National Standards Institute (ANSI) code page used by the writing system represented by the current <see cref="T:System.Globalization.TextInfo" />.</summary>
		/// <returns>The ANSI code page used by the writing system represented by the current <see cref="T:System.Globalization.TextInfo" />.</returns>
		public virtual int ANSICodePage
		{
			get
			{
				return this.data.ansi;
			}
		}

		/// <summary>Gets the Extended Binary Coded Decimal Interchange Code (EBCDIC) code page used by the writing system represented by the current <see cref="T:System.Globalization.TextInfo" />.</summary>
		/// <returns>The EBCDIC code page used by the writing system represented by the current <see cref="T:System.Globalization.TextInfo" />.</returns>
		public virtual int EBCDICCodePage
		{
			get
			{
				return this.data.ebcdic;
			}
		}

		/// <summary>Gets the culture identifier for the culture associated with the current <see cref="T:System.Globalization.TextInfo" /> object.</summary>
		/// <returns>A number that identifies the culture from which the current <see cref="T:System.Globalization.TextInfo" /> object was created.</returns>
		[ComVisible(false)]
		public int LCID
		{
			get
			{
				return this.m_win32LangID;
			}
		}

		/// <summary>Gets or sets the string that separates items in a list.</summary>
		/// <returns>The string that separates items in a list.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value in a set operation is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">In a set operation, the current <see cref="T:System.Globalization.TextInfo" /> object is read-only.</exception>
		public virtual string ListSeparator
		{
			get
			{
				if (this.m_listSeparator == null)
				{
					this.m_listSeparator = ((char)this.data.list_sep).ToString();
				}
				return this.m_listSeparator;
			}
			[ComVisible(false)]
			set
			{
				this.m_listSeparator = value;
			}
		}

		/// <summary>Gets the Macintosh code page used by the writing system represented by the current <see cref="T:System.Globalization.TextInfo" />.</summary>
		/// <returns>The Macintosh code page used by the writing system represented by the current <see cref="T:System.Globalization.TextInfo" />.</returns>
		public virtual int MacCodePage
		{
			get
			{
				return this.data.mac;
			}
		}

		/// <summary>Gets the original equipment manufacturer (OEM) code page used by the writing system represented by the current <see cref="T:System.Globalization.TextInfo" />.</summary>
		/// <returns>The OEM code page used by the writing system represented by the current <see cref="T:System.Globalization.TextInfo" />.</returns>
		public virtual int OEMCodePage
		{
			get
			{
				return this.data.oem;
			}
		}

		/// <summary>Gets the name of the culture associated with the current <see cref="T:System.Globalization.TextInfo" /> object.</summary>
		/// <returns>The name of a culture. </returns>
		[ComVisible(false)]
		public string CultureName
		{
			get
			{
				if (this.customCultureName == null)
				{
					this.customCultureName = this.ci.Name;
				}
				return this.customCultureName;
			}
		}

		/// <summary>Gets a value indicating whether the current <see cref="T:System.Globalization.TextInfo" /> object is read-only.</summary>
		/// <returns>true if the current <see cref="T:System.Globalization.TextInfo" /> object is read-only; otherwise, false.</returns>
		[ComVisible(false)]
		public bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		/// <summary>Gets a value indicating whether the current <see cref="T:System.Globalization.TextInfo" /> object represents a writing system where text flows from right to left.</summary>
		/// <returns>true if text flows from right to left; otherwise, false.</returns>
		[ComVisible(false)]
		public bool IsRightToLeft
		{
			get
			{
				int win32LangID = this.m_win32LangID;
				return win32LangID == 1 || win32LangID == 13 || win32LangID == 32 || win32LangID == 41 || win32LangID == 90 || win32LangID == 101 || win32LangID == 1025 || win32LangID == 1037 || win32LangID == 1056 || win32LangID == 1065 || win32LangID == 1114 || win32LangID == 1125 || win32LangID == 2049 || win32LangID == 3073 || win32LangID == 4097 || win32LangID == 5121 || win32LangID == 6145 || win32LangID == 7169 || win32LangID == 8193 || win32LangID == 9217 || win32LangID == 10241 || win32LangID == 11265 || win32LangID == 12289 || win32LangID == 13313 || win32LangID == 14337 || win32LangID == 15361 || win32LangID == 16385;
			}
		}

		/// <summary>Determines whether the specified object represents the same writing system as the current <see cref="T:System.Globalization.TextInfo" /> object.</summary>
		/// <returns>true if <paramref name="obj" /> represents the same writing system as the current <see cref="T:System.Globalization.TextInfo" />; otherwise, false.</returns>
		/// <param name="obj">The object to compare with the current <see cref="T:System.Globalization.TextInfo" />. </param>
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			TextInfo textInfo = obj as TextInfo;
			return textInfo != null && textInfo.m_win32LangID == this.m_win32LangID && textInfo.ci == this.ci;
		}

		/// <summary>Serves as a hash function for the current <see cref="T:System.Globalization.TextInfo" />, suitable for hashing algorithms and data structures, such as a hash table.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Globalization.TextInfo" />.</returns>
		public override int GetHashCode()
		{
			return this.m_win32LangID;
		}

		/// <summary>Returns a string that represents the current <see cref="T:System.Globalization.TextInfo" />.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Globalization.TextInfo" />.</returns>
		public override string ToString()
		{
			return "TextInfo - " + this.m_win32LangID;
		}

		/// <summary>Converts the specified string to titlecase.</summary>
		/// <returns>The specified string converted to titlecase.</returns>
		/// <param name="str">The string to convert to titlecase. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="str" /> is null. </exception>
		public string ToTitleCase(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			StringBuilder stringBuilder = null;
			int i = 0;
			int num = 0;
			while (i < str.Length)
			{
				if (char.IsLetter(str[i++]))
				{
					i--;
					char c = this.ToTitleCase(str[i]);
					bool flag = true;
					if (c == str[i])
					{
						flag = false;
						bool flag2 = true;
						int num2 = i;
						while (++i < str.Length)
						{
							if (char.IsWhiteSpace(str[i]))
							{
								break;
							}
							c = this.ToTitleCase(str[i]);
							if (c != str[i])
							{
								flag2 = false;
								break;
							}
						}
						if (flag2)
						{
							continue;
						}
						i = num2;
						while (++i < str.Length)
						{
							if (char.IsWhiteSpace(str[i]))
							{
								break;
							}
							if (this.ToLower(str[i]) != str[i])
							{
								flag = true;
								i = num2;
								break;
							}
						}
					}
					if (flag)
					{
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(str.Length);
						}
						stringBuilder.Append(str, num, i - num);
						stringBuilder.Append(this.ToTitleCase(str[i]));
						num = i + 1;
						while (++i < str.Length)
						{
							if (char.IsWhiteSpace(str[i]))
							{
								break;
							}
							stringBuilder.Append(this.ToLower(str[i]));
						}
						num = i;
					}
				}
			}
			if (stringBuilder != null)
			{
				stringBuilder.Append(str, num, str.Length - num);
			}
			return (stringBuilder == null) ? str : stringBuilder.ToString();
		}

		/// <summary>Converts the specified character to lowercase.</summary>
		/// <returns>The specified character converted to lowercase.</returns>
		/// <param name="c">The character to convert to lowercase. </param>
		public virtual char ToLower(char c)
		{
			if (c < '@' || ('`' < c && c < '\u0080'))
			{
				return c;
			}
			if ('A' <= c && c <= 'Z' && (!this.handleDotI || c != 'I'))
			{
				return c + ' ';
			}
			if (this.ci == null || this.ci.LCID == 127)
			{
				return char.ToLowerInvariant(c);
			}
			switch (c)
			{
			case 'ǅ':
				return 'ǆ';
			default:
				switch (c)
				{
				case 'ϒ':
					return 'υ';
				case 'ϓ':
					return 'ύ';
				case 'ϔ':
					return 'ϋ';
				default:
					if (c != 'I')
					{
						if (c == 'İ')
						{
							return 'i';
						}
						if (c == 'ǋ')
						{
							return 'ǌ';
						}
						if (c == 'ǲ')
						{
							return 'ǳ';
						}
					}
					else if (this.handleDotI)
					{
						return 'ı';
					}
					return char.ToLowerInvariant(c);
				}
				break;
			case 'ǈ':
				return 'ǉ';
			}
		}

		/// <summary>Converts the specified character to uppercase.</summary>
		/// <returns>The specified character converted to uppercase.</returns>
		/// <param name="c">The character to convert to uppercase. </param>
		public virtual char ToUpper(char c)
		{
			if (c < '`')
			{
				return c;
			}
			if ('a' <= c && c <= 'z' && (!this.handleDotI || c != 'i'))
			{
				return c - ' ';
			}
			if (this.ci == null || this.ci.LCID == 127)
			{
				return char.ToUpperInvariant(c);
			}
			switch (c)
			{
			case 'ϐ':
				return 'Β';
			case 'ϑ':
				return 'Θ';
			default:
				switch (c)
				{
				case 'ǅ':
					return 'Ǆ';
				default:
					if (c == 'ϰ')
					{
						return 'Κ';
					}
					if (c != 'ϱ')
					{
						if (c != 'i')
						{
							if (c == 'ı')
							{
								return 'I';
							}
							if (c == 'ǋ')
							{
								return 'Ǌ';
							}
							if (c == 'ǲ')
							{
								return 'Ǳ';
							}
							if (c == 'ΐ')
							{
								return 'Ϊ';
							}
							if (c == 'ΰ')
							{
								return 'Ϋ';
							}
						}
						else if (this.handleDotI)
						{
							return 'İ';
						}
						return char.ToUpperInvariant(c);
					}
					return 'Ρ';
				case 'ǈ':
					return 'Ǉ';
				}
				break;
			case 'ϕ':
				return 'Φ';
			case 'ϖ':
				return 'Π';
			}
		}

		private char ToTitleCase(char c)
		{
			switch (c)
			{
			case 'Ǆ':
			case 'ǅ':
			case 'ǆ':
				return 'ǅ';
			case 'Ǉ':
			case 'ǈ':
			case 'ǉ':
				return 'ǈ';
			case 'Ǌ':
			case 'ǋ':
			case 'ǌ':
				return 'ǋ';
			default:
				switch (c)
				{
				case 'Ǳ':
				case 'ǲ':
				case 'ǳ':
					return 'ǲ';
				default:
					if (('ⅰ' <= c && c <= 'ⅿ') || ('ⓐ' <= c && c <= 'ⓩ'))
					{
						return c;
					}
					return this.ToUpper(c);
				}
				break;
			}
		}

		/// <summary>Converts the specified string to lowercase.</summary>
		/// <returns>The specified string converted to lowercase.</returns>
		/// <param name="str">The string to convert to lowercase. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="str" /> is null. </exception>
		public unsafe virtual string ToLower(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (str.Length == 0)
			{
				return string.Empty;
			}
			string text = string.InternalAllocateStr(str.Length);
			fixed (string text2 = str)
			{
				fixed (char* ptr = text2 + RuntimeHelpers.OffsetToStringData / 2)
				{
					fixed (string text3 = text)
					{
						fixed (char* ptr2 = text3 + RuntimeHelpers.OffsetToStringData / 2)
						{
							char* ptr3 = ptr2;
							char* ptr4 = ptr;
							for (int i = 0; i < str.Length; i++)
							{
								*ptr3 = this.ToLower(*ptr4);
								ptr4++;
								ptr3++;
							}
							text2 = null;
							text3 = null;
							return text;
						}
					}
				}
			}
		}

		/// <summary>Converts the specified string to uppercase.</summary>
		/// <returns>The specified string converted to uppercase.</returns>
		/// <param name="str">The string to convert to uppercase. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="str" /> is null. </exception>
		public unsafe virtual string ToUpper(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (str.Length == 0)
			{
				return string.Empty;
			}
			string text = string.InternalAllocateStr(str.Length);
			fixed (string text2 = str)
			{
				fixed (char* ptr = text2 + RuntimeHelpers.OffsetToStringData / 2)
				{
					fixed (string text3 = text)
					{
						fixed (char* ptr2 = text3 + RuntimeHelpers.OffsetToStringData / 2)
						{
							char* ptr3 = ptr2;
							char* ptr4 = ptr;
							for (int i = 0; i < str.Length; i++)
							{
								*ptr3 = this.ToUpper(*ptr4);
								ptr4++;
								ptr3++;
							}
							text2 = null;
							text3 = null;
							return text;
						}
					}
				}
			}
		}

		/// <summary>Returns a read-only version of the specified <see cref="T:System.Globalization.TextInfo" /> object.</summary>
		/// <returns>The <see cref="T:System.Globalization.TextInfo" /> object specified by the <paramref name="textInfo" /> parameter, if <paramref name="textInfo" /> is read-only.-or-A read-only memberwise clone of the <see cref="T:System.Globalization.TextInfo" /> object specified by <paramref name="textInfo" />, if <paramref name="textInfo" /> is not read-only.</returns>
		/// <param name="textInfo">A <see cref="T:System.Globalization.TextInfo" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="textInfo" /> is null.</exception>
		[ComVisible(false)]
		public static TextInfo ReadOnly(TextInfo textInfo)
		{
			if (textInfo == null)
			{
				throw new ArgumentNullException("textInfo");
			}
			return new TextInfo(textInfo)
			{
				m_isReadOnly = true
			};
		}

		/// <summary>Creates a new object that is a copy of the current <see cref="T:System.Globalization.TextInfo" /> object.</summary>
		/// <returns>A new instance of <see cref="T:System.Object" /> that is the memberwise clone of the current <see cref="T:System.Globalization.TextInfo" /> object.</returns>
		[ComVisible(false)]
		public virtual object Clone()
		{
			return new TextInfo(this);
		}

		private struct Data
		{
			public int ansi;

			public int ebcdic;

			public int mac;

			public int oem;

			public byte list_sep;
		}
	}
}
