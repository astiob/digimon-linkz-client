using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

namespace System
{
	/// <summary>Represents a 32-bit signed integer.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public struct Int32 : IFormattable, IConvertible, IComparable, IComparable<int>, IEquatable<int>
	{
		/// <summary>Represents the largest possible value of an <see cref="T:System.Int32" />. This field is constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const int MaxValue = 2147483647;

		/// <summary>Represents the smallest possible value of <see cref="T:System.Int32" />. This field is constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const int MinValue = -2147483648;

		internal int m_value;

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToBoolean(System.IFormatProvider)" />. </summary>
		/// <returns>true if the value of the current instance is not zero; otherwise, false.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToByte(System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Byte" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToChar(System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Char" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return Convert.ToDateTime(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDecimal(System.IFormatProvider)" />.</summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Decimal" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDouble(System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Double" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt16(System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, converted to an <see cref="T:System.Int16" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt32(System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, unchanged.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return this;
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt64(System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, converted to an <see cref="T:System.Int64" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSByte(System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, converted to an <see cref="T:System.SByte" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSingle(System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Single" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, converted to <paramref name="type" />.</returns>
		/// <param name="type">The type to which to convert this <see cref="T:System.Int32" /> value.</param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> implementation that provides information about the format of the returned value.</param>
		object IConvertible.ToType(Type targetType, IFormatProvider provider)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			return Convert.ToType(this, targetType, provider, false);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt16(System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.UInt16" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt32(System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.UInt32" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt64(System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.UInt64" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		/// <summary>Compares this instance to a specified object and returns an indication of their relative values.</summary>
		/// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.Return Value Description Less than zero This instance is less than <paramref name="value" />. Zero This instance is equal to <paramref name="value" />. Greater than zero This instance is greater than <paramref name="value" />.-or- <paramref name="value" /> is null. </returns>
		/// <param name="value">An object to compare, or null. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is not an <see cref="T:System.Int32" />. </exception>
		/// <filterpriority>2</filterpriority>
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is int))
			{
				throw new ArgumentException(Locale.GetText("Value is not a System.Int32"));
			}
			int num = (int)value;
			if (this == num)
			{
				return 0;
			}
			if (this > num)
			{
				return 1;
			}
			return -1;
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
		/// <returns>true if <paramref name="obj" /> is an instance of <see cref="T:System.Int32" /> and equals the value of this instance; otherwise, false.</returns>
		/// <param name="obj">An object to compare with this instance. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			return obj is int && (int)obj == this;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return this;
		}

		/// <summary>Compares this instance to a specified 32-bit signed integer and returns an indication of their relative values.</summary>
		/// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.Return Value Description Less than zero This instance is less than <paramref name="value" />. Zero This instance is equal to <paramref name="value" />. Greater than zero This instance is greater than <paramref name="value" />. </returns>
		/// <param name="value">An integer to compare. </param>
		/// <filterpriority>2</filterpriority>
		public int CompareTo(int value)
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

		/// <summary>Returns a value indicating whether this instance is equal to a specified <see cref="T:System.Int32" /> value. </summary>
		/// <returns>true if <paramref name="obj" /> has the same value as this instance; otherwise, false.</returns>
		/// <param name="obj">An <see cref="T:System.Int32" /> value to compare to this instance.</param>
		/// <filterpriority>2</filterpriority>
		public bool Equals(int obj)
		{
			return obj == this;
		}

		internal static bool ProcessTrailingWhitespace(bool tryParse, string s, int position, ref Exception exc)
		{
			int length = s.Length;
			for (int i = position; i < length; i++)
			{
				char c = s[i];
				if (c != '\0' && !char.IsWhiteSpace(c))
				{
					if (!tryParse)
					{
						exc = int.GetFormatException();
					}
					return false;
				}
			}
			return true;
		}

		internal static bool Parse(string s, bool tryParse, out int result, out Exception exc)
		{
			int num = 0;
			int num2 = 1;
			bool flag = false;
			result = 0;
			exc = null;
			if (s == null)
			{
				if (!tryParse)
				{
					exc = new ArgumentNullException("s");
				}
				return false;
			}
			int length = s.Length;
			int i;
			char c;
			for (i = 0; i < length; i++)
			{
				c = s[i];
				if (!char.IsWhiteSpace(c))
				{
					break;
				}
			}
			if (i == length)
			{
				if (!tryParse)
				{
					exc = int.GetFormatException();
				}
				return false;
			}
			c = s[i];
			if (c == '+')
			{
				i++;
			}
			else if (c == '-')
			{
				num2 = -1;
				i++;
			}
			while (i < length)
			{
				c = s[i];
				if (c == '\0')
				{
					i = length;
				}
				else
				{
					if (c >= '0' && c <= '9')
					{
						byte b = (byte)(c - '0');
						if (num <= 214748364)
						{
							if (num != 214748364)
							{
								num = num * 10 + (int)b;
								flag = true;
								goto IL_15F;
							}
							if (b <= 7 || (num2 != 1 && b <= 8))
							{
								if (num2 == -1)
								{
									num = num * num2 * 10 - (int)b;
								}
								else
								{
									num = num * 10 + (int)b;
								}
								if (int.ProcessTrailingWhitespace(tryParse, s, i + 1, ref exc))
								{
									result = num;
									return true;
								}
							}
						}
						if (!tryParse)
						{
							exc = new OverflowException("Value is too large");
						}
						return false;
					}
					if (!int.ProcessTrailingWhitespace(tryParse, s, i, ref exc))
					{
						return false;
					}
				}
				IL_15F:
				i++;
			}
			if (!flag)
			{
				if (!tryParse)
				{
					exc = int.GetFormatException();
				}
				return false;
			}
			if (num2 == -1)
			{
				result = num * num2;
			}
			else
			{
				result = num;
			}
			return true;
		}

		/// <summary>Converts the string representation of a number in a specified culture-specific format to its 32-bit signed integer equivalent.</summary>
		/// <returns>A 32-bit signed integer equivalent to the number specified in <paramref name="s" />.</returns>
		/// <param name="s">A string containing a number to convert. </param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information about <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not of the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static int Parse(string s, IFormatProvider provider)
		{
			return int.Parse(s, NumberStyles.Integer, provider);
		}

		/// <summary>Converts the string representation of a number in a specified style to its 32-bit signed integer equivalent.</summary>
		/// <returns>A 32-bit signed integer equivalent to the number specified in <paramref name="s" />.</returns>
		/// <param name="s">A string containing a number to convert. </param>
		/// <param name="style">A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value. -or-<paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in a format compliant with <paramref name="style" />. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. -or-<paramref name="s" /> includes non-zero, fractional digits.</exception>
		/// <filterpriority>1</filterpriority>
		public static int Parse(string s, NumberStyles style)
		{
			return int.Parse(s, style, null);
		}

		internal static bool CheckStyle(NumberStyles style, bool tryParse, ref Exception exc)
		{
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				NumberStyles numberStyles = style ^ NumberStyles.AllowHexSpecifier;
				if ((numberStyles & NumberStyles.AllowLeadingWhite) != NumberStyles.None)
				{
					numberStyles ^= NumberStyles.AllowLeadingWhite;
				}
				if ((numberStyles & NumberStyles.AllowTrailingWhite) != NumberStyles.None)
				{
					numberStyles ^= NumberStyles.AllowTrailingWhite;
				}
				if (numberStyles != NumberStyles.None)
				{
					if (!tryParse)
					{
						exc = new ArgumentException("With AllowHexSpecifier only AllowLeadingWhite and AllowTrailingWhite are permitted.");
					}
					return false;
				}
			}
			else if (style > NumberStyles.Any)
			{
				if (!tryParse)
				{
					exc = new ArgumentException("Not a valid number style");
				}
				return false;
			}
			return true;
		}

		internal static bool JumpOverWhite(ref int pos, string s, bool reportError, bool tryParse, ref Exception exc)
		{
			while (pos < s.Length && char.IsWhiteSpace(s[pos]))
			{
				pos++;
			}
			if (reportError && pos >= s.Length)
			{
				if (!tryParse)
				{
					exc = int.GetFormatException();
				}
				return false;
			}
			return true;
		}

		internal static void FindSign(ref int pos, string s, NumberFormatInfo nfi, ref bool foundSign, ref bool negative)
		{
			if (pos + nfi.NegativeSign.Length <= s.Length && s.IndexOf(nfi.NegativeSign, pos, nfi.NegativeSign.Length) == pos)
			{
				negative = true;
				foundSign = true;
				pos += nfi.NegativeSign.Length;
			}
			else if (pos + nfi.PositiveSign.Length < s.Length && s.IndexOf(nfi.PositiveSign, pos, nfi.PositiveSign.Length) == pos)
			{
				negative = false;
				pos += nfi.PositiveSign.Length;
				foundSign = true;
			}
		}

		internal static void FindCurrency(ref int pos, string s, NumberFormatInfo nfi, ref bool foundCurrency)
		{
			if (pos + nfi.CurrencySymbol.Length <= s.Length && s.Substring(pos, nfi.CurrencySymbol.Length) == nfi.CurrencySymbol)
			{
				foundCurrency = true;
				pos += nfi.CurrencySymbol.Length;
			}
		}

		internal static bool FindExponent(ref int pos, string s, ref int exponent, bool tryParse, ref Exception exc)
		{
			exponent = 0;
			long num = 0L;
			int i = s.IndexOfAny(new char[]
			{
				'e',
				'E'
			}, pos);
			if (i < 0)
			{
				exc = null;
				return false;
			}
			if (++i == s.Length)
			{
				exc = ((!tryParse) ? int.GetFormatException() : null);
				return true;
			}
			if (s[i] == '-')
			{
				exc = ((!tryParse) ? new OverflowException("Value too large or too small.") : null);
				return true;
			}
			if (s[i] == '+' && ++i == s.Length)
			{
				exc = ((!tryParse) ? int.GetFormatException() : null);
				return true;
			}
			while (i < s.Length)
			{
				if (!char.IsDigit(s[i]))
				{
					exc = ((!tryParse) ? int.GetFormatException() : null);
					return true;
				}
				num = checked(num * 10L - unchecked((long)(checked(s[i] - '0'))));
				if (num < -2147483648L || num > 2147483647L)
				{
					exc = ((!tryParse) ? new OverflowException("Value too large or too small.") : null);
					return true;
				}
				i++;
			}
			num = -num;
			exc = null;
			exponent = (int)num;
			pos = i;
			return true;
		}

		internal static bool FindOther(ref int pos, string s, string other)
		{
			if (pos + other.Length <= s.Length && s.Substring(pos, other.Length) == other)
			{
				pos += other.Length;
				return true;
			}
			return false;
		}

		internal static bool ValidDigit(char e, bool allowHex)
		{
			if (allowHex)
			{
				return char.IsDigit(e) || (e >= 'A' && e <= 'F') || (e >= 'a' && e <= 'f');
			}
			return char.IsDigit(e);
		}

		internal static Exception GetFormatException()
		{
			return new FormatException("Input string was not in the correct format");
		}

		internal static bool Parse(string s, NumberStyles style, IFormatProvider fp, bool tryParse, out int result, out Exception exc)
		{
			result = 0;
			exc = null;
			if (s == null)
			{
				if (!tryParse)
				{
					exc = new ArgumentNullException();
				}
				return false;
			}
			if (s.Length == 0)
			{
				if (!tryParse)
				{
					exc = int.GetFormatException();
				}
				return false;
			}
			NumberFormatInfo numberFormatInfo = null;
			if (fp != null)
			{
				Type typeFromHandle = typeof(NumberFormatInfo);
				numberFormatInfo = (NumberFormatInfo)fp.GetFormat(typeFromHandle);
			}
			if (numberFormatInfo == null)
			{
				numberFormatInfo = Thread.CurrentThread.CurrentCulture.NumberFormat;
			}
			if (!int.CheckStyle(style, tryParse, ref exc))
			{
				return false;
			}
			bool flag = (style & NumberStyles.AllowCurrencySymbol) != NumberStyles.None;
			bool flag2 = (style & NumberStyles.AllowHexSpecifier) != NumberStyles.None;
			bool flag3 = (style & NumberStyles.AllowThousands) != NumberStyles.None;
			bool flag4 = (style & NumberStyles.AllowDecimalPoint) != NumberStyles.None;
			bool flag5 = (style & NumberStyles.AllowParentheses) != NumberStyles.None;
			bool flag6 = (style & NumberStyles.AllowTrailingSign) != NumberStyles.None;
			bool flag7 = (style & NumberStyles.AllowLeadingSign) != NumberStyles.None;
			bool flag8 = (style & NumberStyles.AllowTrailingWhite) != NumberStyles.None;
			bool flag9 = (style & NumberStyles.AllowLeadingWhite) != NumberStyles.None;
			bool flag10 = (style & NumberStyles.AllowExponent) != NumberStyles.None;
			int num = 0;
			if (flag9 && !int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
			{
				return false;
			}
			bool flag11 = false;
			bool flag12 = false;
			bool flag13 = false;
			bool flag14 = false;
			if (flag5 && s[num] == '(')
			{
				flag11 = true;
				flag13 = true;
				flag12 = true;
				num++;
				if (flag9 && int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
				{
					return false;
				}
				if (s.Substring(num, numberFormatInfo.NegativeSign.Length) == numberFormatInfo.NegativeSign)
				{
					if (!tryParse)
					{
						exc = int.GetFormatException();
					}
					return false;
				}
				if (s.Substring(num, numberFormatInfo.PositiveSign.Length) == numberFormatInfo.PositiveSign)
				{
					if (!tryParse)
					{
						exc = int.GetFormatException();
					}
					return false;
				}
			}
			if (flag7 && !flag13)
			{
				int.FindSign(ref num, s, numberFormatInfo, ref flag13, ref flag12);
				if (flag13)
				{
					if (flag9 && !int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
					{
						return false;
					}
					if (flag)
					{
						int.FindCurrency(ref num, s, numberFormatInfo, ref flag14);
						if (flag14 && flag9 && !int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
						{
							return false;
						}
					}
				}
			}
			if (flag && !flag14)
			{
				int.FindCurrency(ref num, s, numberFormatInfo, ref flag14);
				if (flag14)
				{
					if (flag9 && !int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
					{
						return false;
					}
					if (flag14 && !flag13 && flag7)
					{
						int.FindSign(ref num, s, numberFormatInfo, ref flag13, ref flag12);
						if (flag13 && flag9 && !int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
						{
							return false;
						}
					}
				}
			}
			int num2 = 0;
			int num3 = 0;
			bool flag15 = false;
			int num4 = 0;
			do
			{
				if (!int.ValidDigit(s[num], flag2))
				{
					if (!flag3 || !int.FindOther(ref num, s, numberFormatInfo.NumberGroupSeparator))
					{
						if (flag15 || !flag4 || !int.FindOther(ref num, s, numberFormatInfo.NumberDecimalSeparator))
						{
							break;
						}
						flag15 = true;
					}
				}
				else if (flag2)
				{
					num3++;
					char c = s[num++];
					int num5;
					if (char.IsDigit(c))
					{
						num5 = (int)(c - '0');
					}
					else if (char.IsLower(c))
					{
						num5 = (int)(c - 'a' + '\n');
					}
					else
					{
						num5 = (int)(c - 'A' + '\n');
					}
					uint num6 = (uint)num2;
					if (tryParse)
					{
						if ((num6 & 4026531840u) != 0u)
						{
							return false;
						}
						num2 = (int)(num6 * 16u + (uint)num5);
					}
					else
					{
						num2 = (int)(checked(num6 * 16u + (uint)num5));
					}
				}
				else if (flag15)
				{
					num3++;
					if (s[num++] != '0')
					{
						goto Block_50;
					}
				}
				else
				{
					num3++;
					try
					{
						num2 = checked(num2 * 10 - (int)(s[num++] - '0'));
					}
					catch (OverflowException)
					{
						if (!tryParse)
						{
							exc = new OverflowException("Value too large or too small.");
						}
						return false;
					}
				}
			}
			while (num < s.Length);
			goto IL_43A;
			Block_50:
			if (!tryParse)
			{
				exc = new OverflowException("Value too large or too small.");
			}
			return false;
			IL_43A:
			if (num3 == 0)
			{
				if (!tryParse)
				{
					exc = int.GetFormatException();
				}
				return false;
			}
			if (flag10 && int.FindExponent(ref num, s, ref num4, tryParse, ref exc) && exc != null)
			{
				return false;
			}
			if (flag6 && !flag13)
			{
				int.FindSign(ref num, s, numberFormatInfo, ref flag13, ref flag12);
				if (flag13)
				{
					if (flag8 && !int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
					{
						return false;
					}
					if (flag)
					{
						int.FindCurrency(ref num, s, numberFormatInfo, ref flag14);
					}
				}
			}
			if (flag && !flag14)
			{
				int.FindCurrency(ref num, s, numberFormatInfo, ref flag14);
				if (flag14)
				{
					if (flag8 && !int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
					{
						return false;
					}
					if (!flag13 && flag6)
					{
						int.FindSign(ref num, s, numberFormatInfo, ref flag13, ref flag12);
					}
				}
			}
			if (flag8 && num < s.Length && !int.JumpOverWhite(ref num, s, false, tryParse, ref exc))
			{
				return false;
			}
			if (flag11)
			{
				if (num >= s.Length || s[num++] != ')')
				{
					if (!tryParse)
					{
						exc = int.GetFormatException();
					}
					return false;
				}
				if (flag8 && num < s.Length && !int.JumpOverWhite(ref num, s, false, tryParse, ref exc))
				{
					return false;
				}
			}
			if (num < s.Length && s[num] != '\0')
			{
				if (!tryParse)
				{
					exc = int.GetFormatException();
				}
				return false;
			}
			if (!flag12 && !flag2)
			{
				if (tryParse)
				{
					long num7 = -(long)num2;
					if (num7 < -2147483648L || num7 > 2147483647L)
					{
						return false;
					}
					num2 = (int)num7;
				}
				else
				{
					num2 = checked(0 - num2);
				}
			}
			if (num4 > 0)
			{
				double num8 = Math.Pow(10.0, (double)num4) * (double)num2;
				if (num8 < -2147483648.0 || num8 > 2147483647.0)
				{
					if (!tryParse)
					{
						exc = new OverflowException("Value too large or too small.");
					}
					return false;
				}
				num2 = (int)num8;
			}
			result = num2;
			return true;
		}

		/// <summary>Converts the string representation of a number to its 32-bit signed integer equivalent.</summary>
		/// <returns>A 32-bit signed integer equivalent to the number contained in <paramref name="s" />.</returns>
		/// <param name="s">A string containing a number to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static int Parse(string s)
		{
			int result;
			Exception ex;
			if (!int.Parse(s, false, out result, out ex))
			{
				throw ex;
			}
			return result;
		}

		/// <summary>Converts the string representation of a number in a specified style and culture-specific format to its 32-bit signed integer equivalent.</summary>
		/// <returns>A 32-bit signed integer equivalent to the number specified in <paramref name="s" />.</returns>
		/// <param name="s">A string containing a number to convert. </param>
		/// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific information about the format of <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value. -or-<paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in a format compliant with <paramref name="style" />. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. -or-<paramref name="s" /> includes non-zero, fractional digits.</exception>
		/// <filterpriority>1</filterpriority>
		public static int Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			int result;
			Exception ex;
			if (!int.Parse(s, style, provider, false, out result, out ex))
			{
				throw ex;
			}
			return result;
		}

		/// <summary>Converts the string representation of a number to its 32-bit signed integer equivalent. A return value indicates whether the conversion succeeded.</summary>
		/// <returns>true if <paramref name="s" /> was converted successfully; otherwise, false.</returns>
		/// <param name="s">A string containing a number to convert. </param>
		/// <param name="result">When this method returns, contains the 32-bit signed integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is null, is not of the correct format, or represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. This parameter is passed uninitialized. </param>
		/// <filterpriority>1</filterpriority>
		public static bool TryParse(string s, out int result)
		{
			Exception ex;
			if (!int.Parse(s, true, out result, out ex))
			{
				result = 0;
				return false;
			}
			return true;
		}

		/// <summary>Converts the string representation of a number in a specified style and culture-specific format to its 32-bit signed integer equivalent. A return value indicates whether the conversion succeeded.</summary>
		/// <returns>true if <paramref name="s" /> was converted successfully; otherwise, false.</returns>
		/// <param name="s">A string containing a number to convert. The string is interpreted using the style specified by <paramref name="style" />.</param>
		/// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />. </param>
		/// <param name="result">When this method returns, contains the 32-bit signed integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is null, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. This parameter is passed uninitialized. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value. -or-<paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
		/// <filterpriority>1</filterpriority>
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out int result)
		{
			Exception ex;
			if (!int.Parse(s, style, provider, true, out result, out ex))
			{
				result = 0;
				return false;
			}
			return true;
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
		/// <returns>The string representation of the value of this instance, consisting of a negative sign if the value is negative, and a sequence of digits ranging from 0 to 9 with no leading zeroes.</returns>
		/// <filterpriority>1</filterpriority>
		public override string ToString()
		{
			return NumberFormatter.NumberToString(this, null);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		public string ToString(IFormatProvider provider)
		{
			return NumberFormatter.NumberToString(this, provider);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation, using the specified format.</summary>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
		/// <param name="format">A numeric format string (see Remarks).</param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is invalid or not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
		/// <param name="format">A numeric format string (see Remarks).</param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is invalid or not supported.</exception>
		/// <filterpriority>1</filterpriority>
		public string ToString(string format, IFormatProvider provider)
		{
			return NumberFormatter.NumberToString(format, this, provider);
		}

		/// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.Int32" />.</summary>
		/// <returns>The enumerated constant, <see cref="F:System.TypeCode.Int32" />.</returns>
		/// <filterpriority>2</filterpriority>
		public TypeCode GetTypeCode()
		{
			return TypeCode.Int32;
		}
	}
}
