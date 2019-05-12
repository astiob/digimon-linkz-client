using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Represents a double-precision floating-point number.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public struct Double : IFormattable, IConvertible, IComparable, IComparable<double>, IEquatable<double>
	{
		/// <summary>Represents the smallest positive <see cref="T:System.Double" /> value greater than zero. This field is constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const double Epsilon = 4.94065645841247E-324;

		/// <summary>Represents the largest possible value of a <see cref="T:System.Double" />. This field is constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const double MaxValue = 1.7976931348623157E+308;

		/// <summary>Represents the smallest possible value of a <see cref="T:System.Double" />. This field is constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const double MinValue = -1.7976931348623157E+308;

		/// <summary>Represents a value that is not a number (NaN). This field is constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const double NaN = double.NaN;

		/// <summary>Represents negative infinity. This field is constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const double NegativeInfinity = double.NegativeInfinity;

		/// <summary>Represents positive infinity. This field is constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const double PositiveInfinity = double.PositiveInfinity;

		private const int State_AllowSign = 1;

		private const int State_Digits = 2;

		private const int State_Decimal = 3;

		private const int State_ExponentSign = 4;

		private const int State_Exponent = 5;

		private const int State_ConsumeWhiteSpace = 6;

		private const int State_Exit = 7;

		internal double m_value;

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, converted to <paramref name="type" />.</returns>
		/// <param name="type">The type to which to convert this <see cref="T:System.Double" /> value.</param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> implementation that supplies culture-specific information about the format of the returned value.</param>
		object IConvertible.ToType(Type targetType, IFormatProvider provider)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			return Convert.ToType(this, targetType, provider, false);
		}

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

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />. </summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" /></summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDecimal(System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Decimal" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDouble(System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, unchanged.</returns>
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

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt16(System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.UInt16" />.</returns>
		/// <param name="provider">This parameter is ignored. </param>
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt32(System.IFormatProvider)" />. </summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.UInt32" />.</returns>
		/// <param name="provider">This parameter is ignored.   </param>
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

		/// <summary>Compares this instance to a specified object and returns an integer that indicates whether the value of this instance is less than, equal to, or greater than the value of the specified object.</summary>
		/// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.Value Description A negative integer This instance is less than <paramref name="value" />.-or- This instance is not a number (<see cref="F:System.Double.NaN" />) and <paramref name="value" /> is a number. Zero This instance is equal to <paramref name="value" />.-or- This instance and <paramref name="value" /> are both Double.NaN, <see cref="F:System.Double.PositiveInfinity" />, or <see cref="F:System.Double.NegativeInfinity" />A positive integer This instance is greater than <paramref name="value" />.-or- This instance is a number and <paramref name="value" /> is not a number (<see cref="F:System.Double.NaN" />).-or- <paramref name="value" /> is null. </returns>
		/// <param name="value">An object to compare, or null. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is not a <see cref="T:System.Double" />. </exception>
		/// <filterpriority>2</filterpriority>
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is double))
			{
				throw new ArgumentException(Locale.GetText("Value is not a System.Double"));
			}
			double num = (double)value;
			if (double.IsPositiveInfinity(this) && double.IsPositiveInfinity(num))
			{
				return 0;
			}
			if (double.IsNegativeInfinity(this) && double.IsNegativeInfinity(num))
			{
				return 0;
			}
			if (double.IsNaN(num))
			{
				if (double.IsNaN(this))
				{
					return 0;
				}
				return 1;
			}
			else if (double.IsNaN(this))
			{
				if (double.IsNaN(num))
				{
					return 0;
				}
				return -1;
			}
			else
			{
				if (this > num)
				{
					return 1;
				}
				if (this < num)
				{
					return -1;
				}
				return 0;
			}
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
		/// <returns>true if <paramref name="obj" /> is an instance of <see cref="T:System.Double" /> and equals the value of this instance; otherwise, false.</returns>
		/// <param name="obj">An object to compare with this instance. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (!(obj is double))
			{
				return false;
			}
			double num = (double)obj;
			if (double.IsNaN(num))
			{
				return double.IsNaN(this);
			}
			return num == this;
		}

		/// <summary>Compares this instance to a specified double-precision floating-point number and returns an integer that indicates whether the value of this instance is less than, equal to, or greater than the value of the specified double-precision floating-point number.</summary>
		/// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.Return Value Description Less than zero This instance is less than <paramref name="value" />.-or- This instance is not a number (<see cref="F:System.Double.NaN" />) and <paramref name="value" /> is a number. Zero This instance is equal to <paramref name="value" />.-or- Both this instance and <paramref name="value" /> are not a number (<see cref="F:System.Double.NaN" />), <see cref="F:System.Double.PositiveInfinity" />, or <see cref="F:System.Double.NegativeInfinity" />. Greater than zero This instance is greater than <paramref name="value" />.-or- This instance is a number and <paramref name="value" /> is not a number (<see cref="F:System.Double.NaN" />). </returns>
		/// <param name="value">A double-precision floating-point number to compare. </param>
		/// <filterpriority>2</filterpriority>
		public int CompareTo(double value)
		{
			if (double.IsPositiveInfinity(this) && double.IsPositiveInfinity(value))
			{
				return 0;
			}
			if (double.IsNegativeInfinity(this) && double.IsNegativeInfinity(value))
			{
				return 0;
			}
			if (double.IsNaN(value))
			{
				if (double.IsNaN(this))
				{
					return 0;
				}
				return 1;
			}
			else if (double.IsNaN(this))
			{
				if (double.IsNaN(value))
				{
					return 0;
				}
				return -1;
			}
			else
			{
				if (this > value)
				{
					return 1;
				}
				if (this < value)
				{
					return -1;
				}
				return 0;
			}
		}

		/// <summary>Returns a value indicating whether this instance and a specified <see cref="T:System.Double" /> object represent the same value.</summary>
		/// <returns>true if <paramref name="obj" /> is equal to this instance; otherwise, false.</returns>
		/// <param name="obj">A <see cref="T:System.Double" /> object to compare to this instance.</param>
		/// <filterpriority>2</filterpriority>
		public bool Equals(double obj)
		{
			if (double.IsNaN(obj))
			{
				return double.IsNaN(this);
			}
			return obj == this;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			double num = this;
			return num.GetHashCode();
		}

		/// <summary>Returns a value indicating whether the specified number evaluates to negative or positive infinity </summary>
		/// <returns>true if <paramref name="d" /> evaluates to <see cref="F:System.Double.PositiveInfinity" /> or <see cref="F:System.Double.NegativeInfinity" />; otherwise, false.</returns>
		/// <param name="d">A double-precision floating-point number. </param>
		/// <filterpriority>1</filterpriority>
		public static bool IsInfinity(double d)
		{
			return d == double.PositiveInfinity || d == double.NegativeInfinity;
		}

		/// <summary>Returns a value indicating whether the specified number evaluates to a value that is not a number (<see cref="F:System.Double.NaN" />).</summary>
		/// <returns>true if <paramref name="d" /> evaluates to <see cref="F:System.Double.NaN" />; otherwise, false.</returns>
		/// <param name="d">A double-precision floating-point number. </param>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static bool IsNaN(double d)
		{
			return d != d;
		}

		/// <summary>Returns a value indicating whether the specified number evaluates to negative infinity.</summary>
		/// <returns>true if <paramref name="d" /> evaluates to <see cref="F:System.Double.NegativeInfinity" />; otherwise, false.</returns>
		/// <param name="d">A double-precision floating-point number. </param>
		/// <filterpriority>1</filterpriority>
		public static bool IsNegativeInfinity(double d)
		{
			return d < 0.0 && (d == double.NegativeInfinity || d == double.PositiveInfinity);
		}

		/// <summary>Returns a value indicating whether the specified number evaluates to positive infinity.</summary>
		/// <returns>true if <paramref name="d" /> evaluates to <see cref="F:System.Double.PositiveInfinity" />; otherwise, false.</returns>
		/// <param name="d">A double-precision floating-point number. </param>
		/// <filterpriority>1</filterpriority>
		public static bool IsPositiveInfinity(double d)
		{
			return d > 0.0 && (d == double.NegativeInfinity || d == double.PositiveInfinity);
		}

		/// <summary>Converts the string representation of a number to its double-precision floating-point number equivalent.</summary>
		/// <returns>A double-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="s" />. Because of differences in precision, the return value may not be exactly equal to <paramref name="s" />, and for values of <paramref name="s" /> that are less than <see cref="F:System.Double.Epsilon" />, the return value may also differ depending on processor architecture. For more information, see the Remarks section of <see cref="T:System.Double" />.</returns>
		/// <param name="s">A string containing a number to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not a number in a valid format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static double Parse(string s)
		{
			return double.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, null);
		}

		/// <summary>Converts the string representation of a number in a specified culture-specific format to its double-precision floating-point number equivalent.</summary>
		/// <returns>A double-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="s" />. Because of differences in precision, the return value may not be exactly equal to <paramref name="s" />, and for values of <paramref name="s" /> that are less than <see cref="F:System.Double.Epsilon" />, the return value may also differ depending on processor architecture. For more information, see the Remarks section of <see cref="T:System.Double" />.</returns>
		/// <param name="s">A string containing a number to convert. </param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information about <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not a number in a valid format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static double Parse(string s, IFormatProvider provider)
		{
			return double.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, provider);
		}

		/// <summary>Converts the string representation of a number in a specified style to its double-precision floating-point number equivalent.</summary>
		/// <returns>A double-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="s" />. Because of differences in precision, the return value may not be exactly equal to <paramref name="s" />, and for values of <paramref name="s" /> that are less than <see cref="F:System.Double.Epsilon" />, the return value may also differ depending on processor architecture. For more information, see the Remarks section of <see cref="T:System.Double" />.</returns>
		/// <param name="s">A string containing a number to convert. </param>
		/// <param name="style">A bitwise combination of <see cref="T:System.Globalization.NumberStyles" /> values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is a combination of <see cref="F:System.Globalization.NumberStyles.Float" /> and <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not a number in a valid format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value. -or-<paramref name="style" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
		/// <filterpriority>1</filterpriority>
		public static double Parse(string s, NumberStyles style)
		{
			return double.Parse(s, style, null);
		}

		/// <summary>Converts the string representation of a number in a specified style and culture-specific format to its double-precision floating-point number equivalent.</summary>
		/// <returns>A double-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="s" />. Because of differences in precision, the return value may not be exactly equal to <paramref name="s" />, and for values of <paramref name="s" /> that are less than <see cref="F:System.Double.Epsilon" />, the return value may also differ depending on processor architecture. For more information, see the Remarks section of <see cref="T:System.Double" />.</returns>
		/// <param name="s">A string containing a number to convert. </param>
		/// <param name="style">A bitwise combination of <see cref="T:System.Globalization.NumberStyles" /> values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Float" /> combined with <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> object that supplies culture-specific formatting information about <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not a numeric value. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value. -or-<paramref name="style" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static double Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			double result;
			Exception ex;
			if (!double.Parse(s, style, provider, false, out result, out ex))
			{
				throw ex;
			}
			return result;
		}

		internal unsafe static bool Parse(string s, NumberStyles style, IFormatProvider provider, bool tryParse, out double result, out Exception exc)
		{
			result = 0.0;
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
					exc = new FormatException();
				}
				return false;
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				string text = Locale.GetText("Double doesn't support parsing with '{0}'.", new object[]
				{
					"AllowHexSpecifier"
				});
				throw new ArgumentException(text);
			}
			if (style > NumberStyles.Any)
			{
				if (!tryParse)
				{
					exc = new ArgumentException();
				}
				return false;
			}
			NumberFormatInfo instance = NumberFormatInfo.GetInstance(provider);
			if (instance == null)
			{
				throw new Exception("How did this happen?");
			}
			int length = s.Length;
			int num = 0;
			int i = 0;
			bool flag = (style & NumberStyles.AllowLeadingWhite) != NumberStyles.None;
			bool flag2 = (style & NumberStyles.AllowTrailingWhite) != NumberStyles.None;
			if (flag)
			{
				while (i < length && char.IsWhiteSpace(s[i]))
				{
					i++;
				}
				if (i == length)
				{
					if (!tryParse)
					{
						exc = int.GetFormatException();
					}
					return false;
				}
			}
			int num2 = s.Length - 1;
			if (flag2)
			{
				while (char.IsWhiteSpace(s[num2]))
				{
					num2--;
				}
			}
			if (double.TryParseStringConstant(instance.NaNSymbol, s, i, num2))
			{
				result = double.NaN;
				return true;
			}
			if (double.TryParseStringConstant(instance.PositiveInfinitySymbol, s, i, num2))
			{
				result = double.PositiveInfinity;
				return true;
			}
			if (double.TryParseStringConstant(instance.NegativeInfinitySymbol, s, i, num2))
			{
				result = double.NegativeInfinity;
				return true;
			}
			byte[] array = new byte[length + 1];
			int num3 = 1;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			if ((style & NumberStyles.AllowDecimalPoint) != NumberStyles.None)
			{
				text2 = instance.NumberDecimalSeparator;
				num4 = text2.Length;
			}
			if ((style & NumberStyles.AllowThousands) != NumberStyles.None)
			{
				text3 = instance.NumberGroupSeparator;
				num5 = text3.Length;
			}
			if ((style & NumberStyles.AllowCurrencySymbol) != NumberStyles.None)
			{
				text4 = instance.CurrencySymbol;
				num6 = text4.Length;
			}
			string positiveSign = instance.PositiveSign;
			string negativeSign = instance.NegativeSign;
			while (i < length)
			{
				char c = s[i];
				if (c == '\0')
				{
					i = length;
				}
				else
				{
					switch (num3)
					{
					case 1:
						if ((style & NumberStyles.AllowLeadingSign) != NumberStyles.None)
						{
							if (c == positiveSign[0] && s.Substring(i, positiveSign.Length) == positiveSign)
							{
								num3 = 2;
								i += positiveSign.Length - 1;
								goto IL_624;
							}
							if (c == negativeSign[0] && s.Substring(i, negativeSign.Length) == negativeSign)
							{
								num3 = 2;
								array[num++] = 45;
								i += negativeSign.Length - 1;
								goto IL_624;
							}
						}
						num3 = 2;
						goto IL_304;
					case 2:
						goto IL_304;
					case 3:
						goto IL_429;
					case 4:
						if (char.IsDigit(c))
						{
							num3 = 5;
							goto IL_599;
						}
						if (c == positiveSign[0] && s.Substring(i, positiveSign.Length) == positiveSign)
						{
							num3 = 2;
							i += positiveSign.Length - 1;
							goto IL_624;
						}
						if (c == negativeSign[0] && s.Substring(i, negativeSign.Length) == negativeSign)
						{
							num3 = 2;
							array[num++] = 45;
							i += negativeSign.Length - 1;
							goto IL_624;
						}
						if (char.IsWhiteSpace(c))
						{
							goto IL_5E7;
						}
						if (!tryParse)
						{
							exc = new FormatException("Unknown char: " + c);
						}
						return false;
					case 5:
						goto IL_599;
					case 6:
						goto IL_5E7;
					}
					IL_617:
					if (num3 == 7)
					{
						break;
					}
					goto IL_624;
					IL_429:
					if (char.IsDigit(c))
					{
						array[num++] = (byte)c;
						goto IL_617;
					}
					if (c == 'e' || c == 'E')
					{
						if ((style & NumberStyles.AllowExponent) == NumberStyles.None)
						{
							if (!tryParse)
							{
								exc = new FormatException("Unknown char: " + c);
							}
							return false;
						}
						array[num++] = (byte)c;
						num3 = 4;
						goto IL_617;
					}
					else
					{
						if (char.IsWhiteSpace(c))
						{
							goto IL_5E7;
						}
						if (!tryParse)
						{
							exc = new FormatException("Unknown char: " + c);
						}
						return false;
					}
					IL_304:
					if (char.IsDigit(c))
					{
						array[num++] = (byte)c;
						goto IL_617;
					}
					if (c == 'e' || c == 'E')
					{
						goto IL_429;
					}
					if (num4 > 0 && text2[0] == c && string.CompareOrdinal(s, i, text2, 0, num4) == 0)
					{
						array[num++] = 46;
						i += num4 - 1;
						num3 = 3;
						goto IL_617;
					}
					if (num5 > 0 && text3[0] == c && s.Substring(i, num5) == text3)
					{
						i += num5 - 1;
						num3 = 2;
						goto IL_617;
					}
					if (num6 > 0 && text4[0] == c && s.Substring(i, num6) == text4)
					{
						i += num6 - 1;
						num3 = 2;
						goto IL_617;
					}
					if (char.IsWhiteSpace(c))
					{
						goto IL_5E7;
					}
					if (!tryParse)
					{
						exc = new FormatException("Unknown char: " + c);
					}
					return false;
					IL_599:
					if (char.IsDigit(c))
					{
						array[num++] = (byte)c;
						goto IL_617;
					}
					if (!char.IsWhiteSpace(c))
					{
						if (!tryParse)
						{
							exc = new FormatException("Unknown char: " + c);
						}
						return false;
					}
					IL_5E7:
					if (flag2 && char.IsWhiteSpace(c))
					{
						num3 = 6;
						goto IL_617;
					}
					if (!tryParse)
					{
						exc = new FormatException("Unknown char");
					}
					return false;
				}
				IL_624:
				i++;
			}
			array[num] = 0;
			double num7;
			if (!double.ParseImpl(&array[0], out num7))
			{
				if (!tryParse)
				{
					exc = int.GetFormatException();
				}
				return false;
			}
			if (double.IsPositiveInfinity(num7) || double.IsNegativeInfinity(num7))
			{
				if (!tryParse)
				{
					exc = new OverflowException();
				}
				return false;
			}
			result = num7;
			return true;
		}

		private static bool TryParseStringConstant(string format, string s, int start, int end)
		{
			return end - start + 1 == format.Length && string.CompareOrdinal(format, 0, s, start, format.Length) == 0;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool ParseImpl(byte* byte_ptr, out double value);

		/// <summary>Converts the string representation of a number in a specified style and culture-specific format to its double-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
		/// <returns>true if <paramref name="s" /> was converted successfully; otherwise, false.</returns>
		/// <param name="s">A string containing a number to convert. </param>
		/// <param name="style">A bitwise combination of <see cref="T:System.Globalization.NumberStyles" /> values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Float" /> combined with <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information about <paramref name="s" />. </param>
		/// <param name="result">When this method returns, contains a double-precision floating-point number equivalent to the numeric value or symbol contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is null, is not in a format compliant with <paramref name="style" />, represents a number less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />, or if <paramref name="style" /> is not a valid combination of <see cref="T:System.Globalization.NumberStyles" /> enumerated constants. This parameter is passed uninitialized. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value. -or-<paramref name="style" /> includes the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
		/// <filterpriority>1</filterpriority>
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out double result)
		{
			Exception ex;
			if (!double.Parse(s, style, provider, true, out result, out ex))
			{
				result = 0.0;
				return false;
			}
			return true;
		}

		/// <summary>Converts the string representation of a number to its double-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
		/// <returns>true if <paramref name="s" /> was converted successfully; otherwise, false.</returns>
		/// <param name="s">A string containing a number to convert. </param>
		/// <param name="result">When this method returns, contains the double-precision floating-point number equivalent to the <paramref name="s" /> parameter, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is null, is not a number in a valid format, or represents a number less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />. This parameter is passed uninitialized.  </param>
		/// <filterpriority>1</filterpriority>
		public static bool TryParse(string s, out double result)
		{
			return double.TryParse(s, NumberStyles.Any, null, out result);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
		/// <returns>The string representation of the value of this instance.</returns>
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
		///   <paramref name="format" /> is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
		/// <param name="format">A numeric format string (see Remarks).</param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		public string ToString(string format, IFormatProvider provider)
		{
			return NumberFormatter.NumberToString(format, this, provider);
		}

		/// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.Double" />.</summary>
		/// <returns>The enumerated constant, <see cref="F:System.TypeCode.Double" />.</returns>
		/// <filterpriority>2</filterpriority>
		public TypeCode GetTypeCode()
		{
			return TypeCode.Double;
		}
	}
}
