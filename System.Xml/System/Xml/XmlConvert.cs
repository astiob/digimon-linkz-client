using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Xml
{
	/// <summary>Encodes and decodes XML names and provides methods for converting between common language runtime types and XML Schema definition language (XSD) types. When converting data types the values returned are locale independent.</summary>
	public class XmlConvert
	{
		private const string encodedColon = "_x003A_";

		private const NumberStyles floatStyle = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowCurrencySymbol;

		private const NumberStyles integerStyle = NumberStyles.Integer;

		private static readonly string[] datetimeFormats = new string[]
		{
			"yyyy-MM-ddTHH:mm:sszzz",
			"yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz",
			"yyyy-MM-ddTHH:mm:ssZ",
			"yyyy-MM-ddTHH:mm:ss.FFFFFFFZ",
			"yyyy-MM-ddTHH:mm:ss",
			"yyyy-MM-ddTHH:mm:ss.FFFFFFF",
			"HH:mm:ss",
			"HH:mm:ss.FFFFFFF",
			"HH:mm:sszzz",
			"HH:mm:ss.FFFFFFFzzz",
			"HH:mm:ssZ",
			"HH:mm:ss.FFFFFFFZ",
			"yyyy-MM-dd",
			"yyyy-MM-ddzzz",
			"yyyy-MM-ddZ",
			"yyyy-MM",
			"yyyy-MMzzz",
			"yyyy-MMZ",
			"yyyy",
			"yyyyzzz",
			"yyyyZ",
			"--MM-dd",
			"--MM-ddzzz",
			"--MM-ddZ",
			"---dd",
			"---ddzzz",
			"---ddZ"
		};

		private static readonly string[] defaultDateTimeFormats = new string[]
		{
			"yyyy-MM-ddTHH:mm:ss",
			"yyyy-MM-ddTHH:mm:ss.FFFFFFF",
			"yyyy-MM-dd",
			"HH:mm:ss",
			"yyyy-MM",
			"yyyy",
			"--MM-dd",
			"---dd"
		};

		private static readonly string[] roundtripDateTimeFormats;

		private static readonly string[] localDateTimeFormats;

		private static readonly string[] utcDateTimeFormats;

		private static readonly string[] unspecifiedDateTimeFormats;

		private static DateTimeStyles _defaultStyle = DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite;

		static XmlConvert()
		{
			int num = XmlConvert.defaultDateTimeFormats.Length;
			XmlConvert.roundtripDateTimeFormats = new string[num];
			XmlConvert.localDateTimeFormats = new string[num];
			XmlConvert.utcDateTimeFormats = new string[num * 3];
			XmlConvert.unspecifiedDateTimeFormats = new string[num * 4];
			for (int i = 0; i < num; i++)
			{
				string text = XmlConvert.defaultDateTimeFormats[i];
				XmlConvert.localDateTimeFormats[i] = text + "zzz";
				XmlConvert.roundtripDateTimeFormats[i] = text + 'K';
				XmlConvert.utcDateTimeFormats[i * 3] = text;
				XmlConvert.utcDateTimeFormats[i * 3 + 1] = text + 'Z';
				XmlConvert.utcDateTimeFormats[i * 3 + 2] = text + "zzz";
				XmlConvert.unspecifiedDateTimeFormats[i * 4] = text;
				XmlConvert.unspecifiedDateTimeFormats[i * 4 + 1] = XmlConvert.localDateTimeFormats[i];
				XmlConvert.unspecifiedDateTimeFormats[i * 4 + 2] = XmlConvert.roundtripDateTimeFormats[i];
				XmlConvert.unspecifiedDateTimeFormats[i * 4 + 3] = XmlConvert.utcDateTimeFormats[i];
			}
		}

		private static string TryDecoding(string s)
		{
			if (s == null || s.Length < 6)
			{
				return s;
			}
			char c = char.MaxValue;
			try
			{
				c = (char)int.Parse(s.Substring(1, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}
			catch
			{
				return s[0] + XmlConvert.DecodeName(s.Substring(1));
			}
			if (s.Length == 6)
			{
				return c.ToString();
			}
			return c + XmlConvert.DecodeName(s.Substring(6));
		}

		/// <summary>Decodes a name. This method does the reverse of the <see cref="M:System.Xml.XmlConvert.EncodeName(System.String)" /> and <see cref="M:System.Xml.XmlConvert.EncodeLocalName(System.String)" /> methods.</summary>
		/// <returns>The decoded name.</returns>
		/// <param name="name">The name to be transformed. </param>
		public static string DecodeName(string name)
		{
			if (name == null || name.Length == 0)
			{
				return name;
			}
			int num = name.IndexOf('_');
			if (num == -1 || num + 6 >= name.Length)
			{
				return name;
			}
			if ((name[num + 1] != 'X' && name[num + 1] != 'x') || name[num + 6] != '_')
			{
				return name[0] + XmlConvert.DecodeName(name.Substring(1));
			}
			return name.Substring(0, num) + XmlConvert.TryDecoding(name.Substring(num + 1));
		}

		/// <summary>Converts the name to a valid XML local name.</summary>
		/// <returns>The encoded name.</returns>
		/// <param name="name">The name to be encoded. </param>
		public static string EncodeLocalName(string name)
		{
			if (name == null)
			{
				return name;
			}
			string text = XmlConvert.EncodeName(name);
			int num = text.IndexOf(':');
			if (num == -1)
			{
				return text;
			}
			return text.Replace(":", "_x003A_");
		}

		internal static bool IsInvalid(char c, bool firstOnlyLetter)
		{
			if (c == ':')
			{
				return false;
			}
			if (firstOnlyLetter)
			{
				return !XmlChar.IsFirstNameChar((int)c);
			}
			return !XmlChar.IsNameChar((int)c);
		}

		private static string EncodeName(string name, bool nmtoken)
		{
			if (name == null || name.Length == 0)
			{
				return name;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int length = name.Length;
			for (int i = 0; i < length; i++)
			{
				char c = name[i];
				if (XmlConvert.IsInvalid(c, i == 0 && !nmtoken))
				{
					stringBuilder.AppendFormat("_x{0:X4}_", (int)c);
				}
				else if (c == '_' && i + 6 < length && name[i + 1] == 'x' && name[i + 6] == '_')
				{
					stringBuilder.Append("_x005F_");
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		/// <summary>Converts the name to a valid XML name.</summary>
		/// <returns>Returns the name with any invalid characters replaced by an escape string.</returns>
		/// <param name="name">A name to be translated. </param>
		public static string EncodeName(string name)
		{
			return XmlConvert.EncodeName(name, false);
		}

		/// <summary>Verifies the name is valid according to the XML specification.</summary>
		/// <returns>The encoded name.</returns>
		/// <param name="name">The name to be encoded. </param>
		public static string EncodeNmToken(string name)
		{
			if (name == string.Empty)
			{
				throw new XmlException("Invalid NmToken: ''");
			}
			return XmlConvert.EncodeName(name, true);
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.Boolean" /> equivalent.</summary>
		/// <returns>A Boolean value, that is, true or false.</returns>
		/// <param name="s">The string to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> does not represent a Boolean value. </exception>
		public static bool ToBoolean(string s)
		{
			s = s.Trim(XmlChar.WhitespaceChars);
			string text = s;
			switch (text)
			{
			case "1":
				return true;
			case "true":
				return true;
			case "0":
				return false;
			case "false":
				return false;
			}
			throw new FormatException(s + " is not a valid boolean value");
		}

		/// <param name="inArray"></param>
		internal static string ToBinHexString(byte[] buffer)
		{
			StringWriter stringWriter = new StringWriter();
			XmlConvert.WriteBinHex(buffer, 0, buffer.Length, stringWriter);
			return stringWriter.ToString();
		}

		internal static void WriteBinHex(byte[] buffer, int index, int count, TextWriter w)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index must be non negative integer.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count must be non negative integer.");
			}
			if (buffer.Length < index + count)
			{
				throw new ArgumentOutOfRangeException("index and count must be smaller than the length of the buffer.");
			}
			int num = index + count;
			for (int i = index; i < num; i++)
			{
				int num2 = (int)buffer[i];
				int num3 = num2 >> 4;
				int num4 = num2 & 15;
				if (num3 > 9)
				{
					w.Write((char)(num3 + 55));
				}
				else
				{
					w.Write((char)(num3 + 48));
				}
				if (num4 > 9)
				{
					w.Write((char)(num4 + 55));
				}
				else
				{
					w.Write((char)(num4 + 48));
				}
			}
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.Byte" /> equivalent.</summary>
		/// <returns>A Byte equivalent of the string.</returns>
		/// <param name="s">The string to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />. </exception>
		public static byte ToByte(string s)
		{
			return byte.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.Char" /> equivalent.</summary>
		/// <returns>A Char representing the single character.</returns>
		/// <param name="s">The string containing a single character to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">The value of the <paramref name="s" /> parameter is null. </exception>
		/// <exception cref="T:System.FormatException">The <paramref name="s" /> parameter contains more than one character. </exception>
		public static char ToChar(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length != 1)
			{
				throw new FormatException("String contain more than one char");
			}
			return s[0];
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.DateTime" /> equivalent.</summary>
		/// <returns>A DateTime equivalent of the string.</returns>
		/// <param name="s">The string to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is an empty string or is not in the correct format. </exception>
		[Obsolete]
		public static DateTime ToDateTime(string s)
		{
			return XmlConvert.ToDateTime(s, XmlConvert.datetimeFormats);
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.DateTime" /> using the <see cref="T:System.Xml.XmlDateTimeSerializationMode" /> specified</summary>
		/// <returns>A <see cref="T:System.DateTime" /> equivalent of the <see cref="T:System.String" />.</returns>
		/// <param name="s">The <see cref="T:System.String" /> value to convert.</param>
		/// <param name="dateTimeOption">One of the <see cref="T:System.Xml.XmlDateTimeSerializationMode" /> values that specify whether the date should be converted to local time or preserved as Coordinated Universal Time (UTC), if it is a UTC date.</param>
		/// <exception cref="T:System.NullReferenceException">
		///   <paramref name="s" /> is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="dateTimeOption" /> value is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is an empty string or is not in a valid format.</exception>
		public static DateTime ToDateTime(string value, XmlDateTimeSerializationMode mode)
		{
			switch (mode)
			{
			case XmlDateTimeSerializationMode.Local:
			{
				DateTime dateTime = XmlConvert.ToDateTime(value, XmlConvert.localDateTimeFormats);
				return (!(dateTime == DateTime.MinValue) && !(dateTime == DateTime.MaxValue)) ? dateTime.ToLocalTime() : dateTime;
			}
			case XmlDateTimeSerializationMode.Utc:
			{
				DateTime dateTime = XmlConvert.ToDateTime(value, XmlConvert.utcDateTimeFormats);
				return (!(dateTime == DateTime.MinValue) && !(dateTime == DateTime.MaxValue)) ? dateTime.ToUniversalTime() : dateTime;
			}
			case XmlDateTimeSerializationMode.Unspecified:
				return XmlConvert.ToDateTime(value, XmlConvert.unspecifiedDateTimeFormats);
			case XmlDateTimeSerializationMode.RoundtripKind:
				return XmlConvert.ToDateTime(value, XmlConvert.roundtripDateTimeFormats, XmlConvert._defaultStyle | DateTimeStyles.RoundtripKind);
			default:
				return XmlConvert.ToDateTime(value, XmlConvert.defaultDateTimeFormats);
			}
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.DateTime" /> equivalent.</summary>
		/// <returns>A DateTime equivalent of the string.</returns>
		/// <param name="s">The string to convert. </param>
		/// <param name="format">The format structure to apply to the converted DateTime. Valid formats include "yyyy-MM-ddTHH:mm:sszzzzzz" and its subsets. The string is validated against this format. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> or <paramref name="format" /> is String.Empty -or- <paramref name="s" /> does not contain a date and time that corresponds to <paramref name="format" />. </exception>
		public static DateTime ToDateTime(string s, string format)
		{
			DateTimeStyles style = DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite;
			return DateTime.ParseExact(s, format, DateTimeFormatInfo.InvariantInfo, style);
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.DateTime" /> equivalent.</summary>
		/// <returns>A DateTime equivalent of the string.</returns>
		/// <param name="s">The string to convert. </param>
		/// <param name="formats">An array containing the format structures to apply to the converted DateTime. Valid formats include "yyyy-MM-ddTHH:mm:sszzzzzz" and its subsets. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> or an element of <paramref name="formats" /> is String.Empty -or- <paramref name="s" /> does not contain a date and time that corresponds to any of the elements of <paramref name="formats" />. </exception>
		public static DateTime ToDateTime(string s, string[] formats)
		{
			return XmlConvert.ToDateTime(s, formats, XmlConvert._defaultStyle);
		}

		private static DateTime ToDateTime(string s, string[] formats, DateTimeStyles style)
		{
			return DateTime.ParseExact(s, formats, DateTimeFormatInfo.InvariantInfo, style);
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.Decimal" /> equivalent.</summary>
		/// <returns>A Decimal equivalent of the string.</returns>
		/// <param name="s">The string to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		public static decimal ToDecimal(string s)
		{
			return decimal.Parse(s, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.Double" /> equivalent.</summary>
		/// <returns>A Double equivalent of the string.</returns>
		/// <param name="s">The string to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />. </exception>
		public static double ToDouble(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException();
			}
			float num = XmlConvert.TryParseStringFloatConstants(s);
			if (num != 0f)
			{
				return (double)num;
			}
			return double.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowCurrencySymbol, CultureInfo.InvariantCulture);
		}

		private static float TryParseStringFloatConstants(string s)
		{
			int num = 0;
			while (num < s.Length && char.IsWhiteSpace(s[num]))
			{
				num++;
			}
			if (num == s.Length)
			{
				throw new FormatException();
			}
			int num2 = s.Length - 1;
			while (char.IsWhiteSpace(s[num2]))
			{
				num2--;
			}
			if (XmlConvert.TryParseStringConstant("NaN", s, num, num2))
			{
				return float.NaN;
			}
			if (XmlConvert.TryParseStringConstant("INF", s, num, num2))
			{
				return float.PositiveInfinity;
			}
			if (XmlConvert.TryParseStringConstant("-INF", s, num, num2))
			{
				return float.NegativeInfinity;
			}
			if (XmlConvert.TryParseStringConstant("Infinity", s, num, num2))
			{
				return float.PositiveInfinity;
			}
			if (XmlConvert.TryParseStringConstant("-Infinity", s, num, num2))
			{
				return float.NegativeInfinity;
			}
			return 0f;
		}

		private static bool TryParseStringConstant(string format, string s, int start, int end)
		{
			return end - start + 1 == format.Length && string.CompareOrdinal(format, 0, s, start, format.Length) == 0;
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.Guid" /> equivalent.</summary>
		/// <returns>A Guid equivalent of the string.</returns>
		/// <param name="s">The string to convert. </param>
		public static Guid ToGuid(string s)
		{
			Guid result;
			try
			{
				result = new Guid(s);
			}
			catch (FormatException ex)
			{
				throw new FormatException(string.Format("Invalid Guid input '{0}'", ex.InnerException));
			}
			return result;
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.Int16" /> equivalent.</summary>
		/// <returns>An Int16 equivalent of the string.</returns>
		/// <param name="s">The string to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />. </exception>
		public static short ToInt16(string s)
		{
			return short.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.Int32" /> equivalent.</summary>
		/// <returns>An Int32 equivalent of the string.</returns>
		/// <param name="s">The string to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		public static int ToInt32(string s)
		{
			return int.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.Int64" /> equivalent.</summary>
		/// <returns>An Int64 equivalent of the string.</returns>
		/// <param name="s">The string to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />. </exception>
		public static long ToInt64(string s)
		{
			return long.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.SByte" /> equivalent.</summary>
		/// <returns>An SByte equivalent of the string.</returns>
		/// <param name="s">The string to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />. </exception>
		[CLSCompliant(false)]
		public static sbyte ToSByte(string s)
		{
			return sbyte.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.Single" /> equivalent.</summary>
		/// <returns>A Single equivalent of the string.</returns>
		/// <param name="s">The string to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />. </exception>
		public static float ToSingle(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException();
			}
			float num = XmlConvert.TryParseStringFloatConstants(s);
			if (num != 0f)
			{
				return num;
			}
			return float.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowCurrencySymbol, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.Guid" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the Guid.</returns>
		/// <param name="value">The value to convert. </param>
		public static string ToString(Guid value)
		{
			return value.ToString("D", CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.Int32" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the Int32.</returns>
		/// <param name="value">The value to convert. </param>
		public static string ToString(int value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.Int16" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the Int16.</returns>
		/// <param name="value">The value to convert. </param>
		public static string ToString(short value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.Byte" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the Byte.</returns>
		/// <param name="value">The value to convert. </param>
		public static string ToString(byte value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.Int64" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the Int64.</returns>
		/// <param name="value">The value to convert. </param>
		public static string ToString(long value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.Char" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the Char.</returns>
		/// <param name="value">The value to convert. </param>
		public static string ToString(char value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.Boolean" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the Boolean, that is, "true" or "false".</returns>
		/// <param name="value">The value to convert. </param>
		public static string ToString(bool value)
		{
			if (value)
			{
				return "true";
			}
			return "false";
		}

		/// <summary>Converts the <see cref="T:System.SByte" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the SByte.</returns>
		/// <param name="value">The value to convert. </param>
		[CLSCompliant(false)]
		public static string ToString(sbyte value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.Decimal" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the Decimal.</returns>
		/// <param name="value">The value to convert. </param>
		public static string ToString(decimal value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.UInt64" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the UInt64.</returns>
		/// <param name="value">The value to convert. </param>
		[CLSCompliant(false)]
		public static string ToString(ulong value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.TimeSpan" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the TimeSpan.</returns>
		/// <param name="value">The value to convert. </param>
		public static string ToString(TimeSpan value)
		{
			if (value == TimeSpan.Zero)
			{
				return "PT0S";
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (value.Ticks < 0L)
			{
				if (value == TimeSpan.MinValue)
				{
					return "-P10675199DT2H48M5.4775808S";
				}
				stringBuilder.Append('-');
				value = value.Negate();
			}
			stringBuilder.Append('P');
			if (value.Days > 0)
			{
				stringBuilder.Append(value.Days).Append('D');
			}
			long num = value.Ticks % 10000L;
			if (value.Days > 0 || value.Hours > 0 || value.Minutes > 0 || value.Seconds > 0 || value.Milliseconds > 0 || num > 0L)
			{
				stringBuilder.Append('T');
				if (value.Hours > 0)
				{
					stringBuilder.Append(value.Hours).Append('H');
				}
				if (value.Minutes > 0)
				{
					stringBuilder.Append(value.Minutes).Append('M');
				}
				if (value.Seconds > 0 || value.Milliseconds > 0 || num > 0L)
				{
					stringBuilder.Append(value.Seconds);
					bool flag = true;
					if (num > 0L)
					{
						stringBuilder.Append('.').AppendFormat("{0:0000000}", value.Ticks % 10000000L);
					}
					else if (value.Milliseconds > 0)
					{
						stringBuilder.Append('.').AppendFormat("{0:000}", value.Milliseconds);
					}
					else
					{
						flag = false;
					}
					if (flag)
					{
						while (stringBuilder[stringBuilder.Length - 1] == '0')
						{
							stringBuilder.Remove(stringBuilder.Length - 1, 1);
						}
					}
					stringBuilder.Append('S');
				}
			}
			return stringBuilder.ToString();
		}

		/// <summary>Converts the <see cref="T:System.Double" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the Double.</returns>
		/// <param name="value">The value to convert. </param>
		public static string ToString(double value)
		{
			if (double.IsNegativeInfinity(value))
			{
				return "-INF";
			}
			if (double.IsPositiveInfinity(value))
			{
				return "INF";
			}
			if (double.IsNaN(value))
			{
				return "NaN";
			}
			return value.ToString("R", CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.Single" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the Single.</returns>
		/// <param name="value">The value to convert. </param>
		public static string ToString(float value)
		{
			if (float.IsNegativeInfinity(value))
			{
				return "-INF";
			}
			if (float.IsPositiveInfinity(value))
			{
				return "INF";
			}
			if (float.IsNaN(value))
			{
				return "NaN";
			}
			return value.ToString("R", CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.UInt32" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the UInt32.</returns>
		/// <param name="value">The value to convert. </param>
		[CLSCompliant(false)]
		public static string ToString(uint value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.UInt16" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the UInt16.</returns>
		/// <param name="value">The value to convert. </param>
		[CLSCompliant(false)]
		public static string ToString(ushort value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.DateTime" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the DateTime in the format yyyy-MM-ddTHH:mm:ss where 'T' is a constant literal.</returns>
		/// <param name="value">The value to convert. </param>
		[Obsolete]
		public static string ToString(DateTime value)
		{
			return value.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz", CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.DateTime" /> to a <see cref="T:System.String" /> using the <see cref="T:System.Xml.XmlDateTimeSerializationMode" /> specified.</summary>
		/// <returns>A <see cref="T:System.String" /> equivalent of the <see cref="T:System.DateTime" />.</returns>
		/// <param name="value">The <see cref="T:System.DateTime" /> value to convert.</param>
		/// <param name="dateTimeOption">One of the <see cref="T:System.Xml.XmlDateTimeSerializationMode" /> values that specify how to treat the <see cref="T:System.DateTime" /> value.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="dateTimeOption" /> value is not valid.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> or <paramref name="dateTimeOption" /> value is null.</exception>
		public static string ToString(DateTime value, XmlDateTimeSerializationMode mode)
		{
			switch (mode)
			{
			case XmlDateTimeSerializationMode.Local:
				return ((!(value == DateTime.MinValue)) ? ((!(value == DateTime.MaxValue)) ? value.ToLocalTime() : value) : DateTime.MinValue).ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz", CultureInfo.InvariantCulture);
			case XmlDateTimeSerializationMode.Utc:
				return ((!(value == DateTime.MinValue)) ? ((!(value == DateTime.MaxValue)) ? value.ToUniversalTime() : value) : DateTime.MinValue).ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFZ", CultureInfo.InvariantCulture);
			case XmlDateTimeSerializationMode.Unspecified:
				return value.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFF", CultureInfo.InvariantCulture);
			case XmlDateTimeSerializationMode.RoundtripKind:
				return value.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFK", CultureInfo.InvariantCulture);
			default:
				return value.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz", CultureInfo.InvariantCulture);
			}
		}

		/// <summary>Converts the <see cref="T:System.DateTime" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A string representation of the DateTime in the specified format.</returns>
		/// <param name="value">The value to convert. </param>
		/// <param name="format">The format structure that defines how to display the converted string. Valid formats include "yyyy-MM-ddTHH:mm:sszzzzzz" and its subsets. </param>
		public static string ToString(DateTime value, string format)
		{
			return value.ToString(format, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.TimeSpan" /> equivalent.</summary>
		/// <returns>A TimeSpan equivalent of the string.</returns>
		/// <param name="s">The string to convert. The string format must conform to the W3C XML Schema Part 2: Datatypes recommendation for duration.</param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in correct format to represent a TimeSpan value. </exception>
		public static TimeSpan ToTimeSpan(string s)
		{
			s = s.Trim(XmlChar.WhitespaceChars);
			if (s.Length == 0)
			{
				throw new FormatException("Invalid format string for duration schema datatype.");
			}
			int num = 0;
			if (s[0] == '-')
			{
				num = 1;
			}
			bool flag = num == 1;
			if (s[num] != 'P')
			{
				throw new FormatException("Invalid format string for duration schema datatype.");
			}
			num++;
			int num2 = 0;
			int num3 = 0;
			bool flag2 = false;
			int hours = 0;
			int minutes = 0;
			int seconds = 0;
			long num4 = 0L;
			int i = 0;
			bool flag3 = false;
			int j = num;
			while (j < s.Length)
			{
				if (s[j] == 'T')
				{
					flag2 = true;
					num2 = 4;
					j++;
					num = j;
				}
				else
				{
					while (j < s.Length)
					{
						if (s[j] < '0' || '9' < s[j])
						{
							break;
						}
						j++;
					}
					if (num2 == 7)
					{
						i = j - num;
					}
					int num5 = int.Parse(s.Substring(num, j - num), CultureInfo.InvariantCulture);
					if (num2 == 7)
					{
						while (i > 7)
						{
							num5 /= 10;
							i--;
						}
						while (i < 7)
						{
							num5 *= 10;
							i++;
						}
					}
					char c = s[j];
					if (c != '.')
					{
						if (c != 'D')
						{
							if (c != 'H')
							{
								if (c != 'M')
								{
									if (c != 'S')
									{
										if (c != 'Y')
										{
											flag3 = true;
										}
										else
										{
											num3 += num5 * 365;
											if (num2 > 0)
											{
												flag3 = true;
											}
											else
											{
												num2 = 1;
											}
										}
									}
									else
									{
										if (num2 == 7)
										{
											num4 = (long)num5;
										}
										else
										{
											seconds = num5;
										}
										if (!flag2 || num2 > 7)
										{
											flag3 = true;
										}
										else
										{
											num2 = 8;
										}
									}
								}
								else if (num2 < 2)
								{
									num3 += 365 * (num5 / 12) + 30 * (num5 % 12);
									num2 = 2;
								}
								else if (flag2 && num2 < 6)
								{
									minutes = num5;
									num2 = 6;
								}
								else
								{
									flag3 = true;
								}
							}
							else
							{
								hours = num5;
								if (!flag2 || num2 > 4)
								{
									flag3 = true;
								}
								else
								{
									num2 = 5;
								}
							}
						}
						else
						{
							num3 += num5;
							if (num2 > 2)
							{
								flag3 = true;
							}
							else
							{
								num2 = 3;
							}
						}
					}
					else
					{
						if (num2 > 7)
						{
							flag3 = true;
						}
						seconds = num5;
						num2 = 7;
					}
					if (flag3)
					{
						break;
					}
					j++;
					num = j;
				}
			}
			if (flag3)
			{
				throw new FormatException("Invalid format string for duration schema datatype.");
			}
			TimeSpan timeSpan = new TimeSpan(num3, hours, minutes, seconds);
			if (flag)
			{
				return TimeSpan.FromTicks(-(timeSpan.Ticks + num4));
			}
			return TimeSpan.FromTicks(timeSpan.Ticks + num4);
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.UInt16" /> equivalent.</summary>
		/// <returns>A UInt16 equivalent of the string.</returns>
		/// <param name="s">The string to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />. </exception>
		[CLSCompliant(false)]
		public static ushort ToUInt16(string s)
		{
			return ushort.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.UInt32" /> equivalent.</summary>
		/// <returns>A UInt32 equivalent of the string.</returns>
		/// <param name="s">The string to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />. </exception>
		[CLSCompliant(false)]
		public static uint ToUInt32(string s)
		{
			return uint.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the <see cref="T:System.String" /> to a <see cref="T:System.UInt64" /> equivalent.</summary>
		/// <returns>A UInt64 equivalent of the string.</returns>
		/// <param name="s">The string to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />. </exception>
		[CLSCompliant(false)]
		public static ulong ToUInt64(string s)
		{
			return ulong.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
		}

		/// <summary>Verifies that the name is a valid name according to the W3C Extended Markup Language recommendation.</summary>
		/// <returns>The name, if it is a valid XML name.</returns>
		/// <param name="name">The name to verify. </param>
		/// <exception cref="T:System.Xml.XmlException">
		///   <paramref name="name" /> is not a valid XML name. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null or String.Empty. </exception>
		public static string VerifyName(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentNullException("name");
			}
			if (!XmlChar.IsName(name))
			{
				throw new XmlException("'" + name + "' is not a valid XML Name");
			}
			return name;
		}

		/// <summary>Verifies that the name is a valid NCName according to the W3C Extended Markup Language recommendation.</summary>
		/// <returns>The name, if it is a valid NCName.</returns>
		/// <param name="name">The name to verify. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null or String.Empty. </exception>
		/// <exception cref="T:System.Xml.XmlException">
		///   <paramref name="name" /> is not a valid NCName. </exception>
		public static string VerifyNCName(string ncname)
		{
			if (ncname == null || ncname.Length == 0)
			{
				throw new ArgumentNullException("ncname");
			}
			if (!XmlChar.IsNCName(ncname))
			{
				throw new XmlException("'" + ncname + "' is not a valid XML NCName");
			}
			return ncname;
		}

		/// <summary>Verifies that the string is a valid token according to the W3C XML Schema Part2: Datatypes recommendation.</summary>
		/// <returns>The token, if it is a valid token.</returns>
		/// <param name="token">The string value you wish to verify.</param>
		/// <exception cref="T:System.Xml.XmlException">The string value is not a valid token.</exception>
		public static string VerifyTOKEN(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				return name;
			}
			if (XmlChar.IsWhitespace((int)name[0]) || XmlChar.IsWhitespace((int)name[name.Length - 1]))
			{
				throw new XmlException("Whitespace characters (#xA, #xD, #x9, #x20) are not allowed as leading or trailing whitespaces of xs:token.");
			}
			for (int i = 0; i < name.Length; i++)
			{
				if (XmlChar.IsWhitespace((int)name[i]) && name[i] != ' ')
				{
					throw new XmlException("Either #xA, #xD or #x9 are not allowed inside xs:token.");
				}
			}
			return name;
		}

		/// <summary>Verifies that the string is a valid NMTOKEN according to the W3C XML Schema Part2: Datatypes recommendation</summary>
		/// <returns>The name token, if it is a valid NMTOKEN.</returns>
		/// <param name="name">The string you wish to verify.</param>
		/// <exception cref="T:System.Xml.XmlException">The string is not a valid name token.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null.</exception>
		public static string VerifyNMTOKEN(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (!XmlChar.IsNmToken(name))
			{
				throw new XmlException("'" + name + "' is not a valid XML NMTOKEN");
			}
			return name;
		}

		/// <param name="s"></param>
		internal static byte[] FromBinHexString(string s)
		{
			char[] array = s.ToCharArray();
			byte[] array2 = new byte[array.Length / 2 + array.Length % 2];
			XmlConvert.FromBinHexString(array, 0, array.Length, array2);
			return array2;
		}

		internal static int FromBinHexString(char[] chars, int offset, int charLength, byte[] buffer)
		{
			int num = offset;
			for (int i = 0; i < charLength - 1; i += 2)
			{
				buffer[num] = ((chars[i] <= '9') ? ((byte)(chars[i] - '0')) : ((byte)(chars[i] - 'A' + '\n')));
				int num2 = num;
				buffer[num2] = (byte)(buffer[num2] << 4);
				int num3 = num;
				buffer[num3] += ((chars[i + 1] <= '9') ? ((byte)(chars[i + 1] - '0')) : ((byte)(chars[i + 1] - 'A' + '\n')));
				num++;
			}
			if (charLength % 2 != 0)
			{
				buffer[num++] = (byte)(((chars[charLength - 1] <= '9') ? ((byte)(chars[charLength - 1] - '0')) : ((byte)(chars[charLength - 1] - 'A' + '\n'))) << 4);
			}
			return num - offset;
		}

		/// <summary>Converts the supplied <see cref="T:System.String" /> to a <see cref="T:System.DateTimeOffset" /> equivalent.</summary>
		/// <returns>The <see cref="T:System.DateTimeOffset" /> equivalent of the supplied string.</returns>
		/// <param name="s">The string to convert.Note   The string must conform to a subset of the W3C Recommendation for the XML dateTime type. For more information see http://www.w3.org/TR/xmlschema-2/#dateTime.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The argument passed to this method is outside the range of allowable values. For information about allowable values, see <see cref="T:System.DateTimeOffset" />.</exception>
		/// <exception cref="T:System.FormatException">The argument passed to this method does not conform to a subset of the W3C Recommendations for the XML dateTime type. For more information see http://www.w3.org/TR/xmlschema-2/#dateTime.</exception>
		public static DateTimeOffset ToDateTimeOffset(string s)
		{
			return XmlConvert.ToDateTimeOffset(s, XmlConvert.datetimeFormats);
		}

		/// <summary>Converts the supplied <see cref="T:System.String" /> to a <see cref="T:System.DateTimeOffset" /> equivalent.</summary>
		/// <returns>The <see cref="T:System.DateTimeOffset" /> equivalent of the supplied string.</returns>
		/// <param name="s">The string to convert.</param>
		/// <param name="format">The format from which <paramref name="s" /> is converted. The format parameter can be any subset of the W3C Recommendation for the XML dateTime type. (For more information see http://www.w3.org/TR/xmlschema-2/#dateTime.) The string <paramref name="s" /> is validated against this format.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> or <paramref name="format" /> is an empty string or is not in the specified format.</exception>
		public static DateTimeOffset ToDateTimeOffset(string s, string format)
		{
			return DateTimeOffset.ParseExact(s, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
		}

		/// <summary>Converts the supplied <see cref="T:System.String" /> to a <see cref="T:System.DateTimeOffset" /> equivalent.</summary>
		/// <returns>The <see cref="T:System.DateTimeOffset" /> equivalent of the supplied string.</returns>
		/// <param name="s">The string to convert.</param>
		/// <param name="formats">An array of formats from which <paramref name="s" /> can be converted. Each format in <paramref name="formats" /> can be any subset of the W3C Recommendation for the XML dateTime type. (For more information see http://www.w3.org/TR/xmlschema-2/#dateTime.) The string <paramref name="s" /> is validated against one of these formats.</param>
		public static DateTimeOffset ToDateTimeOffset(string s, string[] formats)
		{
			DateTimeStyles styles = DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AssumeUniversal;
			return DateTimeOffset.ParseExact(s, formats, CultureInfo.InvariantCulture, styles);
		}

		/// <summary>Converts the supplied <see cref="T:System.DateTimeOffset" /> to a <see cref="T:System.String" />.</summary>
		/// <returns>A <see cref="T:System.String" /> representation of the supplied <see cref="T:System.DateTimeOffset" />.</returns>
		/// <param name="value">The <see cref="T:System.DateTimeOffset" /> to be converted.</param>
		public static string ToString(DateTimeOffset value)
		{
			return XmlConvert.ToString(value, "yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz");
		}

		/// <summary>Converts the supplied <see cref="T:System.DateTimeOffset" /> to a <see cref="T:System.String" /> in the specified format.</summary>
		/// <returns>A <see cref="T:System.String" /> representation in the specified format of the supplied <see cref="T:System.DateTimeOffset" />.</returns>
		/// <param name="value">The <see cref="T:System.DateTimeOffset" /> to be converted.</param>
		/// <param name="format">The format to which <paramref name="s" /> is converted. The format parameter can be any subset of the W3C Recommendation for the XML dateTime type. (For more information see http://www.w3.org/TR/xmlschema-2/#dateTime.)</param>
		public static string ToString(DateTimeOffset value, string format)
		{
			return value.ToString(format, CultureInfo.InvariantCulture);
		}

		internal static Uri ToUri(string s)
		{
			return new Uri(s, UriKind.RelativeOrAbsolute);
		}
	}
}
