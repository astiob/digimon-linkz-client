using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Represents a Unicode character.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public struct Char : IConvertible, IComparable, IComparable<char>, IEquatable<char>
	{
		/// <summary>Represents the largest possible value of a <see cref="T:System.Char" />. This field is constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const char MaxValue = '￿';

		/// <summary>Represents the smallest possible value of a <see cref="T:System.Char" />. This field is constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const char MinValue = '\0';

		internal char m_value;

		private unsafe static readonly byte* category_data;

		private unsafe static readonly byte* numeric_data;

		private unsafe static readonly double* numeric_data_values;

		private unsafe static readonly ushort* to_lower_data_low;

		private unsafe static readonly ushort* to_lower_data_high;

		private unsafe static readonly ushort* to_upper_data_low;

		private unsafe static readonly ushort* to_upper_data_high;

		static Char()
		{
			char.GetDataTablePointers(out char.category_data, out char.numeric_data, out char.numeric_data_values, out char.to_lower_data_low, out char.to_lower_data_high, out char.to_upper_data_low, out char.to_upper_data_high);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
		/// <returns>An object of the specified type.</returns>
		/// <param name="type">A <see cref="T:System.Type" /> object. </param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null.</exception>
		/// <exception cref="T:System.InvalidCastException">The value of the current <see cref="T:System.Char" /> object cannot be converted to the type specified by the <paramref name="type" /> parameter. </exception>
		object IConvertible.ToType(Type targetType, IFormatProvider provider)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			return Convert.ToType(this, targetType, provider, false);
		}

		/// <summary>Note   This conversion is not supported. Attempting to do so throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToByte(System.IFormatProvider)" />.</summary>
		/// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToChar(System.IFormatProvider)" />.</summary>
		/// <returns>The value of the current <see cref="T:System.Char" /> object unchanged.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return this;
		}

		/// <summary>Note   This conversion is not supported. Attempting to do so throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>No value is returned.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>Note   This conversion is not supported. Attempting to do so throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>No value is returned.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>Note   This conversion is not supported. Attempting to do so throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>No value is returned.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary> For a description of this member, see <see cref="M:System.IConvertible.ToInt16(System.IFormatProvider)" />.</summary>
		/// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt32(System.IFormatProvider)" />.</summary>
		/// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		/// <summary> For a description of this member, see <see cref="M:System.IConvertible.ToInt64(System.IFormatProvider)" />.</summary>
		/// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		/// <summary> For a description of this member, see <see cref="M:System.IConvertible.ToSByte(System.IFormatProvider)" />.</summary>
		/// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		/// <summary>Note   This conversion is not supported. Attempting to do so throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>No value is returned.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt16(System.IFormatProvider)" />.</summary>
		/// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> object. (Specify null because the <paramref name="provider" /> parameter is ignored.)</param>
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt32(System.IFormatProvider)" />.</summary>
		/// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> object. (Specify null because the <paramref name="provider" /> parameter is ignored.)</param>
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt64(System.IFormatProvider)" />.</summary>
		/// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> object. (Specify null because the <paramref name="provider" /> parameter is ignored.)</param>
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void GetDataTablePointers(out byte* category_data, out byte* numeric_data, out double* numeric_data_values, out ushort* to_lower_data_low, out ushort* to_lower_data_high, out ushort* to_upper_data_low, out ushort* to_upper_data_high);

		/// <summary>Compares this instance to a specified object and indicates whether this instance precedes, follows, or appears in the same position in the sort order as the specified <see cref="T:System.Object" />.</summary>
		/// <returns>A signed number indicating the position of this instance in the sort order in relation to the <paramref name="value" /> parameter.Return Value Description Less than zero This instance precedes <paramref name="value" />. Zero This instance has the same position in the sort order as <paramref name="value" />. Greater than zero This instance follows <paramref name="value" />.-or- <paramref name="value" /> is null. </returns>
		/// <param name="value">An object to compare this instance to, or null. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is not a <see cref="T:System.Char" /> object. </exception>
		/// <filterpriority>2</filterpriority>
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is char))
			{
				throw new ArgumentException(Locale.GetText("Value is not a System.Char"));
			}
			char c = (char)value;
			if (this == c)
			{
				return 0;
			}
			if (this > c)
			{
				return 1;
			}
			return -1;
		}

		/// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
		/// <returns>true if <paramref name="obj" /> is an instance of <see cref="T:System.Char" /> and equals the value of this instance; otherwise, false.</returns>
		/// <param name="obj">An object to compare with this instance, or null. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			return obj is char && (char)obj == this;
		}

		/// <summary>Compares this instance to a specified <see cref="T:System.Char" /> object and indicates whether this instance precedes, follows, or appears in the same position in the sort order as the specified <see cref="T:System.Char" /> object.</summary>
		/// <returns>A signed number indicating the position of this instance in the sort order in relation to the <paramref name="value" /> parameter.Return Value Description Less than zero This instance precedes <paramref name="value" />. Zero This instance has the same position in the sort order as <paramref name="value" />. Greater than zero This instance follows <paramref name="value" />. </returns>
		/// <param name="value">A <see cref="T:System.Char" /> object to compare. </param>
		/// <filterpriority>2</filterpriority>
		public int CompareTo(char value)
		{
			if (this == value)
			{
				return 0;
			}
			if (this > value)
			{
				return 1;
			}
			return -1;
		}

		/// <summary>Converts the specified Unicode code point into a UTF-16 encoded string.</summary>
		/// <returns>A string consisting of one <see cref="T:System.Char" /> object or a surrogate pair of <see cref="T:System.Char" /> objects equivalent to the code point specified by the <paramref name="utf32" /> parameter.</returns>
		/// <param name="utf32">A 21-bit Unicode code point. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="utf32" /> is not a valid 21-bit Unicode code point ranging from U+0 through U+10FFFF, excluding the surrogate pair range from U+D800 through U+DFFF. </exception>
		/// <filterpriority>1</filterpriority>
		public static string ConvertFromUtf32(int utf32)
		{
			if (utf32 < 0 || utf32 > 1114111)
			{
				throw new ArgumentOutOfRangeException("utf32", "The argument must be from 0 to 0x10FFFF.");
			}
			if (55296 <= utf32 && utf32 <= 57343)
			{
				throw new ArgumentOutOfRangeException("utf32", "The argument must not be in surrogate pair range.");
			}
			if (utf32 < 65536)
			{
				return new string((char)utf32, 1);
			}
			utf32 -= 65536;
			return new string(new char[]
			{
				(char)((utf32 >> 10) + 55296),
				(char)(utf32 % 1024 + 56320)
			});
		}

		/// <summary>Converts the value of a UTF-16 encoded surrogate pair into a Unicode code point.</summary>
		/// <returns>The 21-bit Unicode code point represented by the <paramref name="highSurrogate" /> and <paramref name="lowSurrogate" /> parameters.</returns>
		/// <param name="highSurrogate">A high surrogate code point (that is, a code point ranging from U+D800 through U+DBFF). </param>
		/// <param name="lowSurrogate">A low surrogate code point (that is, a code point ranging from U+DC00 through U+DFFF). </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="highSurrogate" /> is not in the range U+D800 through U+DBFF, or <paramref name="lowSurrogate" /> is not in the range U+DC00 through U+DFFF. </exception>
		/// <filterpriority>1</filterpriority>
		public static int ConvertToUtf32(char highSurrogate, char lowSurrogate)
		{
			if (highSurrogate < '\ud800' || '\udbff' < highSurrogate)
			{
				throw new ArgumentOutOfRangeException("highSurrogate");
			}
			if (lowSurrogate < '\udc00' || '\udfff' < lowSurrogate)
			{
				throw new ArgumentOutOfRangeException("lowSurrogate");
			}
			return 65536 + (int)((int)(highSurrogate - '\ud800') << 10) + (int)(lowSurrogate - '\udc00');
		}

		/// <summary>Converts the value of a UTF-16 encoded character or surrogate pair at a specified position in a string into a Unicode code point.</summary>
		/// <returns>The 21-bit Unicode code point represented by the character or surrogate pair at the position in the <paramref name="s" /> parameter specified by the <paramref name="index" /> parameter.</returns>
		/// <param name="s">A string that contains a character or surrogate pair. </param>
		/// <param name="index">The index position of the character or surrogate pair in <paramref name="s" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is not a position within <paramref name="s" />. </exception>
		/// <exception cref="T:System.ArgumentException">The specified index position contains a surrogate pair, and either the first character in the pair is not a valid high surrogate or the second character in the pair is not a valid low surrogate. </exception>
		/// <filterpriority>1</filterpriority>
		public static int ConvertToUtf32(string s, int index)
		{
			char.CheckParameter(s, index);
			if (!char.IsSurrogate(s[index]))
			{
				return (int)s[index];
			}
			if (!char.IsHighSurrogate(s[index]) || index == s.Length - 1 || !char.IsLowSurrogate(s[index + 1]))
			{
				throw new ArgumentException(string.Format("The string contains invalid surrogate pair character at {0}", index));
			}
			return char.ConvertToUtf32(s[index], s[index + 1]);
		}

		/// <summary>Returns a value that indicates whether this instance is equal to the specified <see cref="T:System.Char" /> object.</summary>
		/// <returns>true if the <paramref name="obj" /> parameter equals the value of this instance; otherwise, false.</returns>
		/// <param name="obj">An object to compare to this instance. </param>
		/// <filterpriority>2</filterpriority>
		public bool Equals(char obj)
		{
			return this == obj;
		}

		/// <summary>Indicates whether the two specified <see cref="T:System.Char" /> objects form a surrogate pair.</summary>
		/// <returns>true if the numeric value of the <paramref name="highSurrogate" /> parameter ranges from U+D800 through U+DBFF, and the numeric value of the <paramref name="lowSurrogate" /> parameter ranges from U+DC00 through U+DFFF; otherwise, false.</returns>
		/// <param name="highSurrogate">A character. </param>
		/// <param name="lowSurrogate">A character. </param>
		/// <filterpriority>1</filterpriority>
		public static bool IsSurrogatePair(char highSurrogate, char lowSurrogate)
		{
			return '\ud800' <= highSurrogate && highSurrogate <= '\udbff' && '\udc00' <= lowSurrogate && lowSurrogate <= '\udfff';
		}

		/// <summary>Indicates whether two adjacent <see cref="T:System.Char" /> objects at a specified position in a string form a surrogate pair.</summary>
		/// <returns>true if the <paramref name="s" /> parameter and the <paramref name="index" /> parameter specify a pair of adjacent characters, and the numeric value of the character at position <paramref name="index" /> ranges from U+D800 through U+DBFF, and the numeric value of the character at position <paramref name="index" />+1 ranges from U+DC00 through U+DFFF; otherwise, false.</returns>
		/// <param name="s">A string. </param>
		/// <param name="index">A position within <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is not a position within <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsSurrogatePair(string s, int index)
		{
			char.CheckParameter(s, index);
			return index + 1 < s.Length && char.IsSurrogatePair(s[index], s[index + 1]);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return (int)this;
		}

		/// <summary>Converts the specified numeric Unicode character to a double-precision floating point number.</summary>
		/// <returns>The numeric value of <paramref name="c" /> if that character represents a number; otherwise, -1.0.</returns>
		/// <param name="c">The Unicode character to convert. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static double GetNumericValue(char c)
		{
			if (c <= '㊉')
			{
				return char.numeric_data_values[char.numeric_data[c]];
			}
			if (c >= '０' && c <= '９')
			{
				return (double)(c - '０');
			}
			return -1.0;
		}

		/// <summary>Converts the numeric Unicode character at the specified position in a specified string to a double-precision floating point number.</summary>
		/// <returns>The numeric value of the character at position <paramref name="index" /> in <paramref name="s" /> if that character represents a number; otherwise, -1.</returns>
		/// <param name="s">A <see cref="T:System.String" />. </param>
		/// <param name="index">The character position in <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static double GetNumericValue(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.GetNumericValue(s[index]);
		}

		/// <summary>Categorizes a specified Unicode character into a group identified by one of the <see cref="T:System.Globalization.UnicodeCategory" /> values.</summary>
		/// <returns>A <see cref="T:System.Globalization.UnicodeCategory" /> value that identifies the group that contains <paramref name="c" />.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static UnicodeCategory GetUnicodeCategory(char c)
		{
			return (UnicodeCategory)char.category_data[c];
		}

		/// <summary>Categorizes the character at the specified position in a specified string into a group identified by one of the <see cref="T:System.Globalization.UnicodeCategory" /> values.</summary>
		/// <returns>A <see cref="T:System.Globalization.UnicodeCategory" /> enumerated constant that identifies the group that contains the character at position <paramref name="index" /> in <paramref name="s" />.</returns>
		/// <param name="s">A <see cref="T:System.String" />. </param>
		/// <param name="index">The character position in <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static UnicodeCategory GetUnicodeCategory(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.GetUnicodeCategory(s[index]);
		}

		/// <summary>Indicates whether the specified Unicode character is categorized as a control character.</summary>
		/// <returns>true if <paramref name="c" /> is a control character; otherwise, false.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static bool IsControl(char c)
		{
			return char.category_data[c] == 14;
		}

		/// <summary>Indicates whether the character at the specified position in a specified string is categorized as a control character.</summary>
		/// <returns>true if the character at position <paramref name="index" /> in <paramref name="s" /> is a control character; otherwise, false.</returns>
		/// <param name="s">A string. </param>
		/// <param name="index">The character position in <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsControl(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.IsControl(s[index]);
		}

		/// <summary>Indicates whether the specified Unicode character is categorized as a decimal digit.</summary>
		/// <returns>true if <paramref name="c" /> is a decimal digit; otherwise, false.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static bool IsDigit(char c)
		{
			return char.category_data[c] == 8;
		}

		/// <summary>Indicates whether the character at the specified position in a specified string is categorized as a decimal digit.</summary>
		/// <returns>true if the character at position <paramref name="index" /> in <paramref name="s" /> is a decimal digit; otherwise, false.</returns>
		/// <param name="s">A <see cref="T:System.String" />. </param>
		/// <param name="index">The character position in <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsDigit(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.IsDigit(s[index]);
		}

		/// <summary>Indicates whether the specified <see cref="T:System.Char" /> object is a high surrogate.</summary>
		/// <returns>true if the numeric value of the <paramref name="c" /> parameter ranges from U+D800 through U+DBFF; otherwise, false.</returns>
		/// <param name="c">A character. </param>
		/// <filterpriority>1</filterpriority>
		public static bool IsHighSurrogate(char c)
		{
			return c >= '\ud800' && c <= '\udbff';
		}

		/// <summary>Indicates whether the <see cref="T:System.Char" /> object at the specified position in a string is a high surrogate.</summary>
		/// <returns>true if the numeric value of the specified character in the <paramref name="s" /> parameter ranges from U+D800 through U+DBFF; otherwise, false.</returns>
		/// <param name="s">A string. </param>
		/// <param name="index">A position within <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is not a position within <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsHighSurrogate(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.IsHighSurrogate(s[index]);
		}

		/// <summary>Indicates whether the specified Unicode character is categorized as a Unicode letter.</summary>
		/// <returns>true if <paramref name="c" /> is a letter; otherwise, false.</returns>
		/// <param name="c">The Unicode character to evaluate. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static bool IsLetter(char c)
		{
			return char.category_data[c] <= 4;
		}

		/// <summary>Indicates whether the character at the specified position in a specified string is categorized as a Unicode letter.</summary>
		/// <returns>true if the character at position <paramref name="index" /> in <paramref name="s" /> is a letter; otherwise, false.</returns>
		/// <param name="s">A string. </param>
		/// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsLetter(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.IsLetter(s[index]);
		}

		/// <summary>Indicates whether the specified Unicode character is categorized as a letter or a decimal digit.</summary>
		/// <returns>true if <paramref name="c" /> is a letter or a decimal digit; otherwise, false.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static bool IsLetterOrDigit(char c)
		{
			int num = (int)char.category_data[c];
			return num <= 4 || num == 8;
		}

		/// <summary>Indicates whether the character at the specified position in a specified string is categorized as a letter or a decimal digit.</summary>
		/// <returns>true if the character at position <paramref name="index" /> in <paramref name="s" /> is a letter or a decimal digit; otherwise, false.</returns>
		/// <param name="s">A string. </param>
		/// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsLetterOrDigit(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.IsLetterOrDigit(s[index]);
		}

		/// <summary>Indicates whether the specified Unicode character is categorized as a lowercase letter.</summary>
		/// <returns>true if <paramref name="c" /> is a lowercase letter; otherwise, false.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static bool IsLower(char c)
		{
			return char.category_data[c] == 1;
		}

		/// <summary>Indicates whether the character at the specified position in a specified string is categorized as a lowercase letter.</summary>
		/// <returns>true if the character at position <paramref name="index" /> in <paramref name="s" /> is a lowercase letter; otherwise, false.</returns>
		/// <param name="s">A string. </param>
		/// <param name="index">The character position in <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsLower(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.IsLower(s[index]);
		}

		/// <summary>Indicates whether the specified <see cref="T:System.Char" /> object is a low surrogate.</summary>
		/// <returns>true if the numeric value of the <paramref name="c" /> parameter ranges from U+DC00 through U+DFFF; otherwise, false.</returns>
		/// <param name="c">A character. </param>
		/// <filterpriority>1</filterpriority>
		public static bool IsLowSurrogate(char c)
		{
			return c >= '\udc00' && c <= '\udfff';
		}

		/// <summary>Indicates whether the <see cref="T:System.Char" /> object at the specified position in a string is a low surrogate.</summary>
		/// <returns>true if the numeric value of the specified character in the <paramref name="s" /> parameter ranges from U+DC00 through U+DFFF; otherwise, false.</returns>
		/// <param name="s">A string. </param>
		/// <param name="index">A position within <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is not a position within <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsLowSurrogate(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.IsLowSurrogate(s[index]);
		}

		/// <summary>Indicates whether the specified Unicode character is categorized as a number.</summary>
		/// <returns>true if <paramref name="c" /> is a number; otherwise, false.</returns>
		/// <param name="c">The Unicode character to evaluate.</param>
		/// <filterpriority>1</filterpriority>
		public unsafe static bool IsNumber(char c)
		{
			int num = (int)char.category_data[c];
			return num >= 8 && num <= 10;
		}

		/// <summary>Indicates whether the character at the specified position in a specified string is categorized as a number.</summary>
		/// <returns>true if the character at position <paramref name="index" /> in <paramref name="s" /> is a number; otherwise, false.</returns>
		/// <param name="s">A string. </param>
		/// <param name="index">The zero-based position of the character to evaluate in <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsNumber(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.IsNumber(s[index]);
		}

		/// <summary>Indicates whether the specified Unicode character is categorized as a punctuation mark.</summary>
		/// <returns>true if <paramref name="c" /> is a punctuation mark; otherwise, false.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static bool IsPunctuation(char c)
		{
			int num = (int)char.category_data[c];
			return num >= 18 && num <= 24;
		}

		/// <summary>Indicates whether the character at the specified position in a specified string is categorized as a punctuation mark.</summary>
		/// <returns>true if the character at position <paramref name="index" /> in <paramref name="s" /> is a punctuation mark; otherwise, false.</returns>
		/// <param name="s">A string. </param>
		/// <param name="index">The character position in <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsPunctuation(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.IsPunctuation(s[index]);
		}

		/// <summary>Indicates whether the specified Unicode character is categorized as a separator character.</summary>
		/// <returns>true if <paramref name="c" /> is a separator character; otherwise, false.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static bool IsSeparator(char c)
		{
			int num = (int)char.category_data[c];
			return num >= 11 && num <= 13;
		}

		/// <summary>Indicates whether the character at the specified position in a specified string is categorized as a separator character.</summary>
		/// <returns>true if the character at position <paramref name="index" /> in <paramref name="s" /> is a separator character; otherwise, false.</returns>
		/// <param name="s">A string. </param>
		/// <param name="index">The character position in <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsSeparator(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.IsSeparator(s[index]);
		}

		/// <summary>Indicates whether the specified character has a surrogate code point.</summary>
		/// <returns>true if <paramref name="c" /> is either a high surrogate or a low surrogate; otherwise, false.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static bool IsSurrogate(char c)
		{
			return char.category_data[c] == 16;
		}

		/// <summary>Indicates whether the character at the specified position in a specified string has a surrogate code point.</summary>
		/// <returns>true if the character at position <paramref name="index" /> in <paramref name="s" /> is a either a high surrogate or a low surrogate; otherwise, false.</returns>
		/// <param name="s">A string. </param>
		/// <param name="index">The character position in <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsSurrogate(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.IsSurrogate(s[index]);
		}

		/// <summary>Indicates whether the specified Unicode character is categorized as a symbol character.</summary>
		/// <returns>true if <paramref name="c" /> is a symbol character; otherwise, false.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static bool IsSymbol(char c)
		{
			int num = (int)char.category_data[c];
			return num >= 25 && num <= 28;
		}

		/// <summary>Indicates whether the character at the specified position in a specified string is categorized as a symbol character.</summary>
		/// <returns>true if the character at position <paramref name="index" /> in <paramref name="s" /> is a symbol character; otherwise, false.</returns>
		/// <param name="s">A string. </param>
		/// <param name="index">The character position in <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsSymbol(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.IsSymbol(s[index]);
		}

		/// <summary>Indicates whether the specified Unicode character is categorized as an uppercase letter.</summary>
		/// <returns>true if <paramref name="c" /> is an uppercase letter; otherwise, false.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static bool IsUpper(char c)
		{
			return char.category_data[c] == 0;
		}

		/// <summary>Indicates whether the character at the specified position in a specified string is categorized as an uppercase letter.</summary>
		/// <returns>true if the character at position <paramref name="index" /> in <paramref name="s" /> is an uppercase letter; otherwise, false.</returns>
		/// <param name="s">A string. </param>
		/// <param name="index">The character position in <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsUpper(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.IsUpper(s[index]);
		}

		/// <summary>Indicates whether the specified Unicode character is categorized as white space.</summary>
		/// <returns>true if <paramref name="c" /> is white space; otherwise, false.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static bool IsWhiteSpace(char c)
		{
			int num = (int)char.category_data[c];
			return num > 10 && (num <= 13 || (c >= '\t' && c <= '\r') || c == '\u0085' || c == '\u205f');
		}

		/// <summary>Indicates whether the character at the specified position in a specified string is categorized as white space.</summary>
		/// <returns>true if the character at position <paramref name="index" /> in <paramref name="s" /> is white space; otherwise, false.</returns>
		/// <param name="s">A string. </param>
		/// <param name="index">The character position in <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsWhiteSpace(string s, int index)
		{
			char.CheckParameter(s, index);
			return char.IsWhiteSpace(s[index]);
		}

		private static void CheckParameter(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index < 0 || index >= s.Length)
			{
				throw new ArgumentOutOfRangeException(Locale.GetText("The value of index is less than zero, or greater than or equal to the length of s."));
			}
		}

		/// <summary>Converts the value of the specified string to its equivalent Unicode character. A return code indicates whether the conversion succeeded or failed.</summary>
		/// <returns>true if the <paramref name="s" /> parameter was converted successfully; otherwise, false.</returns>
		/// <param name="s">A string containing a single character or null. </param>
		/// <param name="result">When this method returns, contains a Unicode character equivalent to the sole character in <paramref name="s" />, if the conversion succeeded, or an undefined value if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is null or the length of <paramref name="s" /> is not 1. This parameter is passed uninitialized. </param>
		/// <filterpriority>1</filterpriority>
		public static bool TryParse(string s, out char result)
		{
			if (s == null || s.Length != 1)
			{
				result = '\0';
				return false;
			}
			result = s[0];
			return true;
		}

		/// <summary>Converts the value of the specified string to its equivalent Unicode character.</summary>
		/// <returns>A Unicode character equivalent to the sole character in <paramref name="s" />.</returns>
		/// <param name="s">A string containing a single character or null. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">The length of <paramref name="s" /> is not 1. </exception>
		/// <filterpriority>1</filterpriority>
		public static char Parse(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length != 1)
			{
				throw new FormatException(Locale.GetText("s contains more than one character."));
			}
			return s[0];
		}

		/// <summary>Converts the value of a Unicode character to its lowercase equivalent.</summary>
		/// <returns>The lowercase equivalent of <paramref name="c" />, or the unchanged value of <paramref name="c" />, if <paramref name="c" /> is already lowercase or not alphabetic.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public static char ToLower(char c)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToLower(c);
		}

		/// <summary>Converts the value of a Unicode character to its lowercase equivalent using the casing rules of the invariant culture.</summary>
		/// <returns>The lowercase equivalent of the <paramref name="c" /> parameter, or the unchanged value of <paramref name="c" />, if <paramref name="c" /> is already lowercase or not alphabetic.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static char ToLowerInvariant(char c)
		{
			if (c <= 'Ⓩ')
			{
				return (char)char.to_lower_data_low[c];
			}
			if (c >= 'Ａ')
			{
				return (char)char.to_lower_data_high[c - 'Ａ'];
			}
			return c;
		}

		/// <summary>Converts the value of a specified Unicode character to its lowercase equivalent using specified culture-specific formatting information.</summary>
		/// <returns>The lowercase equivalent of <paramref name="c" />, modified according to <paramref name="culture" />, or the unchanged value of <paramref name="c" />, if <paramref name="c" /> is already lowercase or not alphabetic.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <param name="culture">An object that supplies culture-specific casing rules. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="culture" /> is null. </exception>
		/// <filterpriority>1</filterpriority>
		public static char ToLower(char c, CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			if (culture.LCID == 127)
			{
				return char.ToLowerInvariant(c);
			}
			return culture.TextInfo.ToLower(c);
		}

		/// <summary>Converts the value of a Unicode character to its uppercase equivalent.</summary>
		/// <returns>The uppercase equivalent of <paramref name="c" />, or the unchanged value of <paramref name="c" /> if <paramref name="c" /> is already uppercase, has no uppercase equivalent, or is not alphabetic.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public static char ToUpper(char c)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToUpper(c);
		}

		/// <summary>Converts the value of a Unicode character to its uppercase equivalent using the casing rules of the invariant culture.</summary>
		/// <returns>The uppercase equivalent of the <paramref name="c" /> parameter, or the unchanged value of <paramref name="c" />, if <paramref name="c" /> is already uppercase or not alphabetic.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static char ToUpperInvariant(char c)
		{
			if (c <= 'ⓩ')
			{
				return (char)char.to_upper_data_low[c];
			}
			if (c >= 'Ａ')
			{
				return (char)char.to_upper_data_high[c - 'Ａ'];
			}
			return c;
		}

		/// <summary>Converts the value of a specified Unicode character to its uppercase equivalent using specified culture-specific formatting information.</summary>
		/// <returns>The uppercase equivalent of <paramref name="c" />, modified according to <paramref name="culture" />, or the unchanged value of <paramref name="c" /> if <paramref name="c" /> is already uppercase, has no uppercase equivalent, or is not alphabetic.</returns>
		/// <param name="c">The Unicode character to convert. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> object that supplies culture-specific casing rules.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="culture" /> is null. </exception>
		/// <filterpriority>1</filterpriority>
		public static char ToUpper(char c, CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			if (culture.LCID == 127)
			{
				return char.ToUpperInvariant(c);
			}
			return culture.TextInfo.ToUpper(c);
		}

		/// <summary>Converts the value of this instance to its equivalent string representation.</summary>
		/// <returns>The string representation of the value of this instance.</returns>
		/// <filterpriority>1</filterpriority>
		public override string ToString()
		{
			return new string(this, 1);
		}

		/// <summary>Converts the specified Unicode character to its equivalent string representation.</summary>
		/// <returns>The string representation of the value of <paramref name="c" />.</returns>
		/// <param name="c">A Unicode character. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(char c)
		{
			return new string(c, 1);
		}

		/// <summary>Converts the value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
		/// <param name="provider">(Reserved) An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		public string ToString(IFormatProvider provider)
		{
			return new string(this, 1);
		}

		/// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.Char" />.</summary>
		/// <returns>The enumerated constant, <see cref="F:System.TypeCode.Char" />.</returns>
		/// <filterpriority>2</filterpriority>
		public TypeCode GetTypeCode()
		{
			return TypeCode.Char;
		}
	}
}
