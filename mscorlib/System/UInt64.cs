using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

namespace System
{
	/// <summary>Represents a 64-bit unsigned integer.</summary>
	/// <filterpriority>1</filterpriority>
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Serializable]
	public struct UInt64 : IFormattable, IConvertible, IComparable, IComparable<ulong>, IEquatable<ulong>
	{
		/// <summary>Represents the largest possible value of <see cref="T:System.UInt64" />. This field is constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const ulong MaxValue = 18446744073709551615UL;

		/// <summary>Represents the smallest possible value of <see cref="T:System.UInt64" />. This field is constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const ulong MinValue = 0UL;

		internal ulong m_value;

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

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDecimal(System.IFormatProvider)" />. </summary>
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
		/// <returns>The value of the current instance, converted to an <see cref="T:System.Int32" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
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
		/// <param name="type">The type to which to convert this <see cref="T:System.UInt64" /> value.</param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> implementation that supplies information about the format of the returned value.</param>
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
		/// <returns>The value of the current instance, unchanged.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return this;
		}

		/// <summary>Compares this instance to a specified object and returns an indication of their relative values.</summary>
		/// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.Return Value Description Less than zero This instance is less than <paramref name="value" />. Zero This instance is equal to <paramref name="value" />. Greater than zero This instance is greater than <paramref name="value" />.-or- <paramref name="value" /> is null. </returns>
		/// <param name="value">An object to compare, or null. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is not a <see cref="T:System.UInt64" />. </exception>
		/// <filterpriority>2</filterpriority>
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is ulong))
			{
				throw new ArgumentException(Locale.GetText("Value is not a System.UInt64."));
			}
			ulong num = (ulong)value;
			if (this == num)
			{
				return 0;
			}
			return (this >= num) ? 1 : -1;
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
		/// <returns>true if <paramref name="obj" /> is an instance of <see cref="T:System.UInt64" /> and equals the value of this instance; otherwise, false.</returns>
		/// <param name="obj">An object to compare to this instance. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			return obj is ulong && (ulong)obj == this;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return (int)(this & (ulong)-1) ^ (int)(this >> 32);
		}

		/// <summary>Compares this instance to a specified 64-bit unsigned integer and returns an indication of their relative values.</summary>
		/// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.Return Value Description Less than zero This instance is less than <paramref name="value" />. Zero This instance is equal to <paramref name="value" />. Greater than zero This instance is greater than <paramref name="value" />. </returns>
		/// <param name="value">An unsigned integer to compare. </param>
		/// <filterpriority>2</filterpriority>
		public int CompareTo(ulong value)
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

		/// <summary>Returns a value indicating whether this instance is equal to a specified <see cref="T:System.UInt64" /> value.</summary>
		/// <returns>true if <paramref name="obj" /> has the same value as this instance; otherwise, false.</returns>
		/// <param name="obj">A <see cref="T:System.UInt64" /> value to compare to this instance.</param>
		/// <filterpriority>2</filterpriority>
		public bool Equals(ulong obj)
		{
			return obj == this;
		}

		/// <summary>Converts the string representation of a number to its 64-bit unsigned integer equivalent.</summary>
		/// <returns>A 64-bit unsigned integer equivalent to the number contained in <paramref name="s" />.</returns>
		/// <param name="s">A string that represents the number to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="s" /> parameter is null. </exception>
		/// <exception cref="T:System.FormatException">The <paramref name="s" /> parameter is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">The <paramref name="s" /> parameter represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong Parse(string s)
		{
			return ulong.Parse(s, NumberStyles.Integer, null);
		}

		/// <summary>Converts the string representation of a number in a specified culture-specific format to its 64-bit unsigned integer equivalent.</summary>
		/// <returns>A 64-bit unsigned integer equivalent to the number specified in <paramref name="s" />.</returns>
		/// <param name="s">A string that represents the number to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="s" /> parameter is null. </exception>
		/// <exception cref="T:System.FormatException">The <paramref name="s" /> parameter is not in the correct style. </exception>
		/// <exception cref="T:System.OverflowException">The <paramref name="s" /> parameter represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong Parse(string s, IFormatProvider provider)
		{
			return ulong.Parse(s, NumberStyles.Integer, provider);
		}

		/// <summary>Converts the string representation of a number in a specified style to its 64-bit unsigned integer equivalent.</summary>
		/// <returns>A 64-bit unsigned integer equivalent to the number specified in <paramref name="s" />.</returns>
		/// <param name="s">A string that represents the number to convert. The string is interpreted by using the style specified by the <paramref name="style" /> parameter. </param>
		/// <param name="style">A bitwise combination of the enumeration values that specifies the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="s" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value. -or-<paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
		/// <exception cref="T:System.FormatException">The <paramref name="s" /> parameter is not in a format compliant with <paramref name="style" />. </exception>
		/// <exception cref="T:System.OverflowException">The <paramref name="s" /> parameter represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />. -or-<paramref name="s" /> includes non-zero, fractional digits.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong Parse(string s, NumberStyles style)
		{
			return ulong.Parse(s, style, null);
		}

		internal static bool Parse(string s, NumberStyles style, IFormatProvider provider, bool tryParse, out ulong result, out Exception exc)
		{
			result = 0UL;
			exc = null;
			if (s == null)
			{
				if (!tryParse)
				{
					exc = new ArgumentNullException("s");
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
			if (provider != null)
			{
				Type typeFromHandle = typeof(NumberFormatInfo);
				numberFormatInfo = (NumberFormatInfo)provider.GetFormat(typeFromHandle);
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
			int num = 0;
			if (flag9 && !int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
			{
				return false;
			}
			bool flag10 = false;
			bool flag11 = false;
			bool flag12 = false;
			bool flag13 = false;
			if (flag5 && s[num] == '(')
			{
				flag10 = true;
				flag12 = true;
				flag11 = true;
				num++;
				if (flag9 && !int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
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
			if (flag7 && !flag12)
			{
				int.FindSign(ref num, s, numberFormatInfo, ref flag12, ref flag11);
				if (flag12)
				{
					if (flag9 && !int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
					{
						return false;
					}
					if (flag)
					{
						int.FindCurrency(ref num, s, numberFormatInfo, ref flag13);
						if (flag13 && flag9 && !int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
						{
							return false;
						}
					}
				}
			}
			if (flag && !flag13)
			{
				int.FindCurrency(ref num, s, numberFormatInfo, ref flag13);
				if (flag13)
				{
					if (flag9 && !int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
					{
						return false;
					}
					if (flag13 && !flag12 && flag7)
					{
						int.FindSign(ref num, s, numberFormatInfo, ref flag12, ref flag11);
						if (flag12 && flag9 && !int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
						{
							return false;
						}
					}
				}
			}
			ulong num2 = 0UL;
			int num3 = 0;
			bool flag14 = false;
			do
			{
				if (!int.ValidDigit(s[num], flag2))
				{
					if (!flag3 || !int.FindOther(ref num, s, numberFormatInfo.NumberGroupSeparator))
					{
						if (flag14 || !flag4 || !int.FindOther(ref num, s, numberFormatInfo.NumberDecimalSeparator))
						{
							break;
						}
						flag14 = true;
					}
				}
				else if (flag2)
				{
					num3++;
					char c = s[num++];
					ulong num4;
					if (char.IsDigit(c))
					{
						num4 = (ulong)((long)(c - '0'));
					}
					else if (char.IsLower(c))
					{
						num4 = (ulong)((long)(c - 'a' + '\n'));
					}
					else
					{
						num4 = (ulong)((long)(c - 'A' + '\n'));
					}
					if (tryParse)
					{
						bool flag15 = num2 > 65535UL;
						num2 = num2 * 16UL + num4;
						if (flag15 && num2 < 16UL)
						{
							return false;
						}
					}
					else
					{
						num2 = checked(num2 * 16UL + num4);
					}
				}
				else if (flag14)
				{
					num3++;
					if (s[num++] != '0')
					{
						goto Block_51;
					}
				}
				else
				{
					num3++;
					try
					{
						num2 = checked(num2 * 10UL + (ulong)(s[num++] - '0'));
					}
					catch (OverflowException)
					{
						if (!tryParse)
						{
							exc = new OverflowException(Locale.GetText("Value too large or too small."));
						}
						return false;
					}
				}
			}
			while (num < s.Length);
			goto IL_44B;
			Block_51:
			if (!tryParse)
			{
				exc = new OverflowException(Locale.GetText("Value too large or too small."));
			}
			return false;
			IL_44B:
			if (num3 == 0)
			{
				if (!tryParse)
				{
					exc = int.GetFormatException();
				}
				return false;
			}
			if (flag6 && !flag12)
			{
				int.FindSign(ref num, s, numberFormatInfo, ref flag12, ref flag11);
				if (flag12)
				{
					if (flag8 && !int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
					{
						return false;
					}
					if (flag)
					{
						int.FindCurrency(ref num, s, numberFormatInfo, ref flag13);
					}
				}
			}
			if (flag && !flag13)
			{
				int.FindCurrency(ref num, s, numberFormatInfo, ref flag13);
				if (flag13)
				{
					if (flag8 && !int.JumpOverWhite(ref num, s, true, tryParse, ref exc))
					{
						return false;
					}
					if (!flag12 && flag6)
					{
						int.FindSign(ref num, s, numberFormatInfo, ref flag12, ref flag11);
					}
				}
			}
			if (flag8 && num < s.Length && !int.JumpOverWhite(ref num, s, false, tryParse, ref exc))
			{
				return false;
			}
			if (flag10)
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
			if (flag11 && num2 > 0UL)
			{
				if (!tryParse)
				{
					exc = new OverflowException(Locale.GetText("Negative number"));
				}
				return false;
			}
			result = num2;
			return true;
		}

		/// <summary>Converts the string representation of a number in a specified style and culture-specific format to its 64-bit unsigned integer equivalent.</summary>
		/// <returns>A 64-bit unsigned integer equivalent to the number specified in <paramref name="s" />.</returns>
		/// <param name="s">A string that represents the number to convert. The string is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
		/// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="s" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value. -or-<paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
		/// <exception cref="T:System.FormatException">The <paramref name="s" /> parameter is not in a format compliant with <paramref name="style" />. </exception>
		/// <exception cref="T:System.OverflowException">The <paramref name="s" /> parameter represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />. -or-<paramref name="s" /> includes non-zero, fractional digits.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			ulong result;
			Exception ex;
			if (!ulong.Parse(s, style, provider, false, out result, out ex))
			{
				throw ex;
			}
			return result;
		}

		/// <summary>Tries to convert the string representation of a number to its 64-bit unsigned integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
		/// <returns>true if <paramref name="s" /> was converted successfully; otherwise, false.</returns>
		/// <param name="s">A string that represents the number to convert. </param>
		/// <param name="result">When this method returns, contains the 64-bit unsigned integer value that is equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is null, is not of the correct format, or represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />. This parameter is passed uninitialized. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static bool TryParse(string s, out ulong result)
		{
			Exception ex;
			if (!ulong.Parse(s, NumberStyles.Integer, null, true, out result, out ex))
			{
				result = 0UL;
				return false;
			}
			return true;
		}

		/// <summary>Tries to convert the string representation of a number in a specified style and culture-specific format to its 64-bit unsigned integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
		/// <returns>true if <paramref name="s" /> was converted successfully; otherwise, false.</returns>
		/// <param name="s">A string that represents the number to convert. The string is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
		/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />. </param>
		/// <param name="result">When this method returns, contains the 64-bit unsigned integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is null, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />. This parameter is passed uninitialized. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value. -or-<paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ulong result)
		{
			Exception ex;
			if (!ulong.Parse(s, style, provider, true, out result, out ex))
			{
				result = 0UL;
				return false;
			}
			return true;
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
		/// <returns>The string representation of the value of this instance, consisting of a sequence of digits ranging from 0 to 9, without a sign or leading zeroes.</returns>
		/// <filterpriority>1</filterpriority>
		public override string ToString()
		{
			return NumberFormatter.NumberToString(this, null);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		public string ToString(IFormatProvider provider)
		{
			return NumberFormatter.NumberToString(this, provider);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format.</summary>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
		/// <param name="format">A numeric format string.</param>
		/// <exception cref="T:System.FormatException">The <paramref name="format" /> parameter is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
		/// <param name="format">A numeric format string.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information about this instance. </param>
		/// <exception cref="T:System.FormatException">The <paramref name="format" /> parameter is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public string ToString(string format, IFormatProvider provider)
		{
			return NumberFormatter.NumberToString(format, this, provider);
		}

		/// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.UInt64" />.</summary>
		/// <returns>The enumerated constant, <see cref="F:System.TypeCode.UInt64" />.</returns>
		/// <filterpriority>2</filterpriority>
		public TypeCode GetTypeCode()
		{
			return TypeCode.UInt64;
		}
	}
}
