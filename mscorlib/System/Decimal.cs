using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;

namespace System
{
	/// <summary>Represents a decimal number.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public struct Decimal : IFormattable, IConvertible, IComparable, IComparable<decimal>, IEquatable<decimal>
	{
		/// <summary>Represents the smallest possible value of <see cref="T:System.Decimal" />. This field is constant and read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public const decimal MinValue = -79228162514264337593543950335m;

		/// <summary>Represents the largest possible value of <see cref="T:System.Decimal" />. This field is constant and read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public const decimal MaxValue = 79228162514264337593543950335m;

		/// <summary>Represents the number negative one (-1).</summary>
		/// <filterpriority>1</filterpriority>
		public const decimal MinusOne = -1m;

		/// <summary>Represents the number one (1).</summary>
		/// <filterpriority>1</filterpriority>
		public const decimal One = 1m;

		/// <summary>Represents the number zero (0).</summary>
		/// <filterpriority>1</filterpriority>
		public const decimal Zero = 0m;

		private const int DECIMAL_DIVIDE_BY_ZERO = 5;

		private const uint MAX_SCALE = 28u;

		private const int iMAX_SCALE = 28;

		private const uint SIGN_FLAG = 2147483648u;

		private const uint SCALE_MASK = 16711680u;

		private const int SCALE_SHIFT = 16;

		private const uint RESERVED_SS32_BITS = 2130771967u;

		private static readonly decimal MaxValueDiv10 = 7922816251426433759354395033.5m;

		private uint flags;

		private uint hi;

		private uint lo;

		private uint mid;

		/// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> from parameters specifying the instance's constituent parts.</summary>
		/// <param name="lo">The low 32 bits of a 96-bit integer. </param>
		/// <param name="mid">The middle 32 bits of a 96-bit integer. </param>
		/// <param name="hi">The high 32 bits of a 96-bit integer. </param>
		/// <param name="isNegative">The sign of the number; 1 is negative, 0 is positive. </param>
		/// <param name="scale">A power of 10 ranging from 0 to 28. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="scale" /> is greater than 28. </exception>
		public Decimal(int lo, int mid, int hi, bool isNegative, byte scale)
		{
			this.lo = (uint)lo;
			this.mid = (uint)mid;
			this.hi = (uint)hi;
			if (scale > 28)
			{
				throw new ArgumentOutOfRangeException(Locale.GetText("scale must be between 0 and 28"));
			}
			this.flags = (uint)scale;
			this.flags <<= 16;
			if (isNegative)
			{
				this.flags |= 2147483648u;
			}
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> to the value of the specified 32-bit signed integer.</summary>
		/// <param name="value">The value to represent as a <see cref="T:System.Decimal" />. </param>
		public Decimal(int value)
		{
			this.hi = (this.mid = 0u);
			if (value < 0)
			{
				this.flags = 2147483648u;
				this.lo = (uint)(~value + 1);
			}
			else
			{
				this.flags = 0u;
				this.lo = (uint)value;
			}
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> to the value of the specified 32-bit unsigned integer.</summary>
		/// <param name="value">The value to represent as a <see cref="T:System.Decimal" />. </param>
		[CLSCompliant(false)]
		public Decimal(uint value)
		{
			this.lo = value;
			this.flags = (this.hi = (this.mid = 0u));
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> to the value of the specified 64-bit signed integer.</summary>
		/// <param name="value">The value to represent as a <see cref="T:System.Decimal" />. </param>
		public Decimal(long value)
		{
			this.hi = 0u;
			if (value < 0L)
			{
				this.flags = 2147483648u;
				ulong num = (ulong)(~value + 1L);
				this.lo = (uint)num;
				this.mid = (uint)(num >> 32);
			}
			else
			{
				this.flags = 0u;
				this.lo = (uint)value;
				this.mid = (uint)((ulong)value >> 32);
			}
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> to the value of the specified 64-bit unsigned integer.</summary>
		/// <param name="value">The value to represent as a <see cref="T:System.Decimal" />. </param>
		[CLSCompliant(false)]
		public Decimal(ulong value)
		{
			this.flags = (this.hi = 0u);
			this.lo = (uint)value;
			this.mid = (uint)(value >> 32);
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> to the value of the specified single-precision floating-point number.</summary>
		/// <param name="value">The value to represent as a <see cref="T:System.Decimal" />. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Decimal.MaxValue" /> or less than <see cref="F:System.Decimal.MinValue" />.-or- <paramref name="value" /> is <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.PositiveInfinity" />, or <see cref="F:System.Single.NegativeInfinity" />. </exception>
		public Decimal(float value)
		{
			if (value > 7.92281625E+28f || value < -7.92281625E+28f || float.IsNaN(value) || float.IsNegativeInfinity(value) || float.IsPositiveInfinity(value))
			{
				throw new OverflowException(Locale.GetText("Value {0} is greater than Decimal.MaxValue or less than Decimal.MinValue", new object[]
				{
					value
				}));
			}
			decimal num = decimal.Parse(value.ToString(CultureInfo.InvariantCulture), NumberStyles.Float, CultureInfo.InvariantCulture);
			this.flags = num.flags;
			this.hi = num.hi;
			this.lo = num.lo;
			this.mid = num.mid;
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> to the value of the specified double-precision floating-point number.</summary>
		/// <param name="value">The value to represent as a <see cref="T:System.Decimal" />. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Decimal.MaxValue" /> or less than <see cref="F:System.Decimal.MinValue" />.-or- <paramref name="value" /> is <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.PositiveInfinity" />, or <see cref="F:System.Double.NegativeInfinity" />. </exception>
		public Decimal(double value)
		{
			if (value > 7.9228162514264338E+28 || value < -7.9228162514264338E+28 || double.IsNaN(value) || double.IsNegativeInfinity(value) || double.IsPositiveInfinity(value))
			{
				throw new OverflowException(Locale.GetText("Value {0} is greater than Decimal.MaxValue or less than Decimal.MinValue", new object[]
				{
					value
				}));
			}
			decimal num = decimal.Parse(value.ToString(CultureInfo.InvariantCulture), NumberStyles.Float, CultureInfo.InvariantCulture);
			this.flags = num.flags;
			this.hi = num.hi;
			this.lo = num.lo;
			this.mid = num.mid;
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> to a decimal value represented in binary and contained in a specified array.</summary>
		/// <param name="bits">An array of 32-bit signed integers containing a representation of a decimal value. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bits" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">The length of the <paramref name="bits" /> is not 4.-or- The representation of the decimal value in <paramref name="bits" /> is not valid. </exception>
		public Decimal(int[] bits)
		{
			if (bits == null)
			{
				throw new ArgumentNullException(Locale.GetText("Bits is a null reference"));
			}
			if (bits.GetLength(0) != 4)
			{
				throw new ArgumentException(Locale.GetText("bits does not contain four values"));
			}
			this.lo = (uint)bits[0];
			this.mid = (uint)bits[1];
			this.hi = (uint)bits[2];
			this.flags = (uint)bits[3];
			byte b = (byte)(this.flags >> 16);
			if (b > 28 || (this.flags & 2130771967u) != 0u)
			{
				throw new ArgumentException(Locale.GetText("Invalid bits[3]"));
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
		/// <returns>The value of the current instance, converted to a <paramref name="type" />.</returns>
		/// <param name="type">The type to which to convert the value of this <see cref="T:System.Decimal" /> instance. </param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> implementation that supplies culture-specific information about the format of the returned value.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The requested type conversion is not supported. </exception>
		object IConvertible.ToType(Type targetType, IFormatProvider provider)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			return Convert.ToType(this, targetType, provider, false);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToBoolean(System.IFormatProvider)" />.</summary>
		/// <returns>true if the value of the current instance is not zero; otherwise, false.</returns>
		/// <param name="provider">This parameter is ignored. </param>
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToByte(System.IFormatProvider)" />.</summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Byte" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />. </exception>
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.InvalidCastException">In all cases. </exception>
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDecimal(System.IFormatProvider)" />.</summary>
		/// <returns>The value of the current instance, unchanged.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return this;
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDouble(System.IFormatProvider)" />.</summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Double" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt16(System.IFormatProvider)" />.</summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Int16" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.</exception>
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt32(System.IFormatProvider)" />.</summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Int32" />.</returns>
		/// <param name="provider">The parameter is ignored.</param>
		/// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt64(System.IFormatProvider)" />.</summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Int64" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />. </exception>
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSByte(System.IFormatProvider)" />.</summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.SByte" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />. </exception>
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSingle(System.IFormatProvider)" />.</summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.Single" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt16(System.IFormatProvider)" />.</summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.UInt16" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt32(System.IFormatProvider)" />.</summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.UInt32" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		/// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt64(System.IFormatProvider)" />.</summary>
		/// <returns>The value of the current instance, converted to a <see cref="T:System.UInt64" />.</returns>
		/// <param name="provider">This parameter is ignored.</param>
		/// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		/// <summary>Converts the specified 64-bit signed integer, which contains an OLE Automation Currency value, to the equivalent <see cref="T:System.Decimal" /> value.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> that contains the equivalent of <paramref name="cy" />.</returns>
		/// <param name="cy">An OLE Automation Currency value. </param>
		/// <filterpriority>1</filterpriority>
		public static decimal FromOACurrency(long cy)
		{
			return cy / 10000m;
		}

		/// <summary>Converts the value of a specified instance of <see cref="T:System.Decimal" /> to its equivalent binary representation.</summary>
		/// <returns>A 32-bit signed integer array with four elements that contain the binary representation of <paramref name="d" />.</returns>
		/// <param name="d">A <see cref="T:System.Decimal" /> value. </param>
		/// <filterpriority>1</filterpriority>
		public static int[] GetBits(decimal d)
		{
			return new int[]
			{
				(int)d.lo,
				(int)d.mid,
				(int)d.hi,
				(int)d.flags
			};
		}

		/// <summary>Returns the result of multiplying the specified <see cref="T:System.Decimal" /> value by negative one.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> with the value of <paramref name="d" />, but the opposite sign.-or- Zero, if <paramref name="d" /> is zero.</returns>
		/// <param name="d">A <see cref="T:System.Decimal" />. </param>
		/// <filterpriority>1</filterpriority>
		public static decimal Negate(decimal d)
		{
			d.flags ^= 2147483648u;
			return d;
		}

		/// <summary>Adds two specified <see cref="T:System.Decimal" /> values.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> value that is the sum of <paramref name="d1" /> and <paramref name="d2" />.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" />. </param>
		/// <param name="d2">A <see cref="T:System.Decimal" />. </param>
		/// <exception cref="T:System.OverflowException">The sum of <paramref name="d1" /> and <paramref name="d2" /> is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static decimal Add(decimal d1, decimal d2)
		{
			if (decimal.decimalIncr(ref d1, ref d2) == 0)
			{
				return d1;
			}
			throw new OverflowException(Locale.GetText("Overflow on adding decimal number"));
		}

		/// <summary>Subtracts one specified <see cref="T:System.Decimal" /> value from another.</summary>
		/// <returns>The <see cref="T:System.Decimal" /> result of subtracting <paramref name="d2" /> from <paramref name="d1" />.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" /> (the minuend). </param>
		/// <param name="d2">A <see cref="T:System.Decimal" /> (the subtrahend). </param>
		/// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static decimal Subtract(decimal d1, decimal d2)
		{
			d2.flags ^= 2147483648u;
			int num = decimal.decimalIncr(ref d1, ref d2);
			if (num == 0)
			{
				return d1;
			}
			throw new OverflowException(Locale.GetText("Overflow on subtracting decimal numbers (" + num + ")"));
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return (int)(this.flags ^ this.hi ^ this.lo ^ this.mid);
		}

		private static ulong u64(decimal value)
		{
			decimal.decimalFloorAndTrunc(ref value, 0);
			ulong result;
			if (decimal.decimal2UInt64(ref value, out result) != 0)
			{
				throw new OverflowException();
			}
			return result;
		}

		private static long s64(decimal value)
		{
			decimal.decimalFloorAndTrunc(ref value, 0);
			long result;
			if (decimal.decimal2Int64(ref value, out result) != 0)
			{
				throw new OverflowException();
			}
			return result;
		}

		/// <summary>Returns a value indicating whether two specified instances of <see cref="T:System.Decimal" /> represent the same value.</summary>
		/// <returns>true if <paramref name="d1" /> and <paramref name="d2" /> are equal; otherwise, false.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" />. </param>
		/// <param name="d2">A <see cref="T:System.Decimal" />. </param>
		/// <filterpriority>1</filterpriority>
		public static bool Equals(decimal d1, decimal d2)
		{
			return decimal.Compare(d1, d2) == 0;
		}

		/// <summary>Returns a value indicating whether this instance and a specified <see cref="T:System.Object" /> represent the same type and value.</summary>
		/// <returns>true if <paramref name="value" /> is a <see cref="T:System.Decimal" /> and equal to this instance; otherwise, false.</returns>
		/// <param name="value">An <see cref="T:System.Object" />. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object value)
		{
			return value is decimal && decimal.Equals((decimal)value, this);
		}

		private bool IsZero()
		{
			return this.hi == 0u && this.lo == 0u && this.mid == 0u;
		}

		private bool IsNegative()
		{
			return (this.flags & 2147483648u) == 2147483648u;
		}

		/// <summary>Rounds a specified <see cref="T:System.Decimal" /> number to the closest integer toward negative infinity.</summary>
		/// <returns>If <paramref name="d" /> has a fractional part, the next whole <see cref="T:System.Decimal" /> number toward negative infinity that is less than <paramref name="d" />.-or- If <paramref name="d" /> doesn't have a fractional part, <paramref name="d" /> is returned unchanged.</returns>
		/// <param name="d">A <see cref="T:System.Decimal" />. </param>
		/// <filterpriority>1</filterpriority>
		public static decimal Floor(decimal d)
		{
			decimal.decimalFloorAndTrunc(ref d, 1);
			return d;
		}

		/// <summary>Returns the integral digits of the specified <see cref="T:System.Decimal" />; any fractional digits are discarded.</summary>
		/// <returns>The <see cref="T:System.Decimal" /> result of <paramref name="d" /> rounded toward zero, to the nearest whole number.</returns>
		/// <param name="d">A <see cref="T:System.Decimal" /> to truncate. </param>
		/// <filterpriority>1</filterpriority>
		public static decimal Truncate(decimal d)
		{
			decimal.decimalFloorAndTrunc(ref d, 0);
			return d;
		}

		/// <summary>Rounds a <see cref="T:System.Decimal" /> value to a specified number of decimal places.</summary>
		/// <returns>The <see cref="T:System.Decimal" /> number equivalent to <paramref name="d" /> rounded to <paramref name="decimals" /> number of decimal places.</returns>
		/// <param name="d">A <see cref="T:System.Decimal" /> value to round. </param>
		/// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="decimals" /> is not a value from 0 to 28. </exception>
		/// <filterpriority>1</filterpriority>
		public static decimal Round(decimal d, int decimals)
		{
			return decimal.Round(d, decimals, MidpointRounding.ToEven);
		}

		/// <summary>Rounds a decimal value to a specified precision. A parameter specifies how to round the value if it is midway between two other numbers.</summary>
		/// <returns>The number that is nearest to the <paramref name="d" /> parameter with a precision equal to the <paramref name="decimals" /> parameter. If <paramref name="d" /> is halfway between two numbers, one of which is even and the other odd, the <paramref name="mode" /> parameter determines which of the two numbers is returned. If the precision of <paramref name="d" /> is less than <paramref name="decimals" />, <paramref name="d" /> is returned unchanged.</returns>
		/// <param name="d">A decimal number to round. </param>
		/// <param name="decimals">The number of significant decimal places (precision) in the return value. </param>
		/// <param name="mode">A value that specifies how to round <paramref name="d" /> if it is midway between two other numbers.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="decimals" /> is less than 0 or greater than 28. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="mode" /> is not a <see cref="T:System.MidpointRounding" /> value.</exception>
		/// <exception cref="T:System.OverflowException">The result is outside the range of a <see cref="T:System.Decimal" /> object.</exception>
		/// <filterpriority>1</filterpriority>
		public static decimal Round(decimal d, int decimals, MidpointRounding mode)
		{
			if (mode != MidpointRounding.ToEven && mode != MidpointRounding.AwayFromZero)
			{
				throw new ArgumentException("The value '" + mode + "' is not valid for this usage of the type MidpointRounding.", "mode");
			}
			if (decimals < 0 || decimals > 28)
			{
				throw new ArgumentOutOfRangeException("decimals", "[0,28]");
			}
			bool flag = d.IsNegative();
			if (flag)
			{
				d.flags ^= 2147483648u;
			}
			decimal d2 = (decimal)Math.Pow(10.0, (double)decimals);
			decimal num = decimal.Floor(d);
			decimal num2 = d - num;
			num2 *= 10000000000000000000000000000m;
			num2 = decimal.Floor(num2);
			num2 /= 10000000000000000000000000000m / d2;
			num2 = Math.Round(num2, mode);
			num2 /= d2;
			decimal num3 = num + num2;
			long num4 = (long)decimals - (long)((ulong)((num3.flags & 2147418112u) >> 16));
			if (num4 > 0L)
			{
				while (num4 > 0L)
				{
					if (num3 > decimal.MaxValueDiv10)
					{
						break;
					}
					num3 *= 10m;
					num4 -= 1L;
				}
			}
			else if (num4 < 0L)
			{
				while (num4 < 0L)
				{
					num3 /= 10m;
					num4 += 1L;
				}
			}
			num3.flags = (uint)((uint)((long)decimals - num4) << 16);
			if (flag)
			{
				num3.flags ^= 2147483648u;
			}
			return num3;
		}

		/// <summary>Rounds a decimal value to the nearest integer.</summary>
		/// <returns>The integer that is nearest to the <paramref name="d" /> parameter. If <paramref name="d" /> is halfway between two integers, one of which is even and the other odd, the even number is returned.</returns>
		/// <param name="d">A decimal number to round. </param>
		/// <exception cref="T:System.OverflowException">The result is outside the range of a <see cref="T:System.Decimal" /> object.</exception>
		/// <filterpriority>1</filterpriority>
		public static decimal Round(decimal d)
		{
			return Math.Round(d);
		}

		/// <summary>Rounds a decimal value to the nearest integer. A parameter specifies how to round the value if it is midway between two other numbers.</summary>
		/// <returns>The integer that is nearest to the <paramref name="d" /> parameter. If <paramref name="d" /> is halfway between two numbers, one of which is even and the other odd, the <paramref name="mode" /> parameter determines which of the two numbers is returned. </returns>
		/// <param name="d">A decimal number to round. </param>
		/// <param name="mode">A value that specifies how to round <paramref name="d" /> if it is midway between two other numbers.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="mode" /> is not a <see cref="T:System.MidpointRounding" /> value.</exception>
		/// <exception cref="T:System.OverflowException">The result is outside the range of a <see cref="T:System.Decimal" /> object.</exception>
		/// <filterpriority>1</filterpriority>
		public static decimal Round(decimal d, MidpointRounding mode)
		{
			return Math.Round(d, mode);
		}

		/// <summary>Multiplies two specified <see cref="T:System.Decimal" /> values.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> that is the result of multiplying <paramref name="d1" /> and <paramref name="d2" />.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" /> (the multiplicand). </param>
		/// <param name="d2">A <see cref="T:System.Decimal" /> (the multiplier). </param>
		/// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static decimal Multiply(decimal d1, decimal d2)
		{
			if (d1.IsZero() || d2.IsZero())
			{
				return 0m;
			}
			if (decimal.decimalMult(ref d1, ref d2) != 0)
			{
				throw new OverflowException();
			}
			return d1;
		}

		/// <summary>Divides two specified <see cref="T:System.Decimal" /> values.</summary>
		/// <returns>The <see cref="T:System.Decimal" /> that is the result of dividing <paramref name="d1" /> by <paramref name="d2" />.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" /> (the dividend). </param>
		/// <param name="d2">A <see cref="T:System.Decimal" /> (the divisor). </param>
		/// <exception cref="T:System.DivideByZeroException">
		///   <paramref name="d2" /> is zero. </exception>
		/// <exception cref="T:System.OverflowException">The return value (that is, the quotient) is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static decimal Divide(decimal d1, decimal d2)
		{
			if (d2.IsZero())
			{
				throw new DivideByZeroException();
			}
			if (d1.IsZero())
			{
				return 0m;
			}
			d1.flags ^= 2147483648u;
			d1.flags ^= 2147483648u;
			decimal result;
			if (decimal.decimalDiv(out result, ref d1, ref d2) != 0)
			{
				throw new OverflowException();
			}
			return result;
		}

		/// <summary>Computes the remainder after dividing two <see cref="T:System.Decimal" /> values.</summary>
		/// <returns>The <see cref="T:System.Decimal" /> that is the remainder after dividing <paramref name="d1" /> by <paramref name="d2" />.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" /> (the dividend). </param>
		/// <param name="d2">A <see cref="T:System.Decimal" /> (the divisor). </param>
		/// <exception cref="T:System.DivideByZeroException">
		///   <paramref name="d2" /> is zero. </exception>
		/// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static decimal Remainder(decimal d1, decimal d2)
		{
			if (d2.IsZero())
			{
				throw new DivideByZeroException();
			}
			if (d1.IsZero())
			{
				return 0m;
			}
			bool flag = d1.IsNegative();
			if (flag)
			{
				d1.flags ^= 2147483648u;
			}
			if (d2.IsNegative())
			{
				d2.flags ^= 2147483648u;
			}
			if (d1 == d2)
			{
				return 0m;
			}
			decimal num;
			if (d2 > d1)
			{
				num = d1;
			}
			else
			{
				if (decimal.decimalDiv(out num, ref d1, ref d2) != 0)
				{
					throw new OverflowException();
				}
				num = decimal.Truncate(num);
				num = d1 - num * d2;
			}
			if (flag)
			{
				num.flags ^= 2147483648u;
			}
			return num;
		}

		/// <summary>Compares two specified <see cref="T:System.Decimal" /> values.</summary>
		/// <returns>A signed number indicating the relative values of <paramref name="d1" /> and <paramref name="d2" />.Return Value Meaning Less than zero <paramref name="d1" /> is less than <paramref name="d2" />. Zero <paramref name="d1" /> and <paramref name="d2" /> are equal. Greater than zero <paramref name="d1" /> is greater than <paramref name="d2" />. </returns>
		/// <param name="d1">A <see cref="T:System.Decimal" />. </param>
		/// <param name="d2">A <see cref="T:System.Decimal" />. </param>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static int Compare(decimal d1, decimal d2)
		{
			return decimal.decimalCompare(ref d1, ref d2);
		}

		/// <summary>Compares this instance to a specified <see cref="T:System.Object" />.</summary>
		/// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.Return Value Meaning Less than zero This instance is less than <paramref name="value" />. Zero This instance is equal to <paramref name="value" />. Greater than zero This instance is greater than <paramref name="value" />.-or- <paramref name="value" /> is null. </returns>
		/// <param name="value">An <see cref="T:System.Object" /> or null. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is not a <see cref="T:System.Decimal" />. </exception>
		/// <filterpriority>2</filterpriority>
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is decimal))
			{
				throw new ArgumentException(Locale.GetText("Value is not a System.Decimal"));
			}
			return decimal.Compare(this, (decimal)value);
		}

		/// <summary>Compares this instance to a specified <see cref="T:System.Decimal" /> object.</summary>
		/// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.Return Value Meaning Less than zero This instance is less than <paramref name="value" />. Zero This instance is equal to <paramref name="value" />. Greater than zero This instance is greater than <paramref name="value" />. </returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> object.</param>
		/// <filterpriority>2</filterpriority>
		public int CompareTo(decimal value)
		{
			return decimal.Compare(this, value);
		}

		/// <summary>Returns a value indicating whether this instance and a specified <see cref="T:System.Decimal" /> object represent the same value.</summary>
		/// <returns>true if <paramref name="value" /> is equal to this instance; otherwise, false.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> object to compare to this instance.</param>
		/// <filterpriority>2</filterpriority>
		public bool Equals(decimal value)
		{
			return decimal.Equals(value, this);
		}

		/// <summary>Returns the smallest integral value greater than or equal to the specified decimal number.</summary>
		/// <returns>The smallest integral value greater than or equal to the <paramref name="d" /> parameter. Note that this method returns a <see cref="T:System.Decimal" /> rather than an integral type.</returns>
		/// <param name="d">A decimal number. </param>
		/// <filterpriority>1</filterpriority>
		public static decimal Ceiling(decimal d)
		{
			return Math.Ceiling(d);
		}

		/// <summary>Converts the string representation of a number to its <see cref="T:System.Decimal" /> equivalent.</summary>
		/// <returns>The <see cref="T:System.Decimal" /> number equivalent to the number contained in <paramref name="s" />.</returns>
		/// <param name="s">The string representation of the number to convert.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static decimal Parse(string s)
		{
			return decimal.Parse(s, NumberStyles.Number, null);
		}

		/// <summary>Converts the string representation of a number in a specified style to its <see cref="T:System.Decimal" /> equivalent.</summary>
		/// <returns>The <see cref="T:System.Decimal" /> number equivalent to the number contained in <paramref name="s" /> as specified by <paramref name="style" />.</returns>
		/// <param name="s">The string representation of the number to convert. </param>
		/// <param name="style">A bitwise combination of <see cref="T:System.Globalization.NumberStyles" /> values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Number" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value. -or-<paramref name="style" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" /></exception>
		/// <filterpriority>1</filterpriority>
		public static decimal Parse(string s, NumberStyles style)
		{
			return decimal.Parse(s, style, null);
		}

		/// <summary>Converts the string representation of a number to its <see cref="T:System.Decimal" /> equivalent using the specified culture-specific format information.</summary>
		/// <returns>The <see cref="T:System.Decimal" /> number equivalent to the number contained in <paramref name="s" /> as specified by <paramref name="provider" />.</returns>
		/// <param name="s">The string representation of the number to convert. </param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific parsing information about <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not of the correct format </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" /></exception>
		/// <filterpriority>1</filterpriority>
		public static decimal Parse(string s, IFormatProvider provider)
		{
			return decimal.Parse(s, NumberStyles.Number, provider);
		}

		private static void ThrowAtPos(int pos)
		{
			throw new FormatException(string.Format(Locale.GetText("Invalid character at position {0}"), pos));
		}

		private static void ThrowInvalidExp()
		{
			throw new FormatException(Locale.GetText("Invalid exponent"));
		}

		private static string stripStyles(string s, NumberStyles style, NumberFormatInfo nfi, out int decPos, out bool isNegative, out bool expFlag, out int exp, bool throwex)
		{
			isNegative = false;
			expFlag = false;
			exp = 0;
			decPos = -1;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = (style & NumberStyles.AllowLeadingWhite) != NumberStyles.None;
			bool flag5 = (style & NumberStyles.AllowTrailingWhite) != NumberStyles.None;
			bool flag6 = (style & NumberStyles.AllowLeadingSign) != NumberStyles.None;
			bool flag7 = (style & NumberStyles.AllowTrailingSign) != NumberStyles.None;
			bool flag8 = (style & NumberStyles.AllowParentheses) != NumberStyles.None;
			bool flag9 = (style & NumberStyles.AllowThousands) != NumberStyles.None;
			bool flag10 = (style & NumberStyles.AllowDecimalPoint) != NumberStyles.None;
			bool flag11 = (style & NumberStyles.AllowExponent) != NumberStyles.None;
			bool flag12 = false;
			if ((style & NumberStyles.AllowCurrencySymbol) != NumberStyles.None)
			{
				int num = s.IndexOf(nfi.CurrencySymbol);
				if (num >= 0)
				{
					s = s.Remove(num, nfi.CurrencySymbol.Length);
					flag12 = true;
				}
			}
			string text = (!flag12) ? nfi.NumberDecimalSeparator : nfi.CurrencyDecimalSeparator;
			string text2 = (!flag12) ? nfi.NumberGroupSeparator : nfi.CurrencyGroupSeparator;
			int i = 0;
			int length = s.Length;
			StringBuilder stringBuilder = new StringBuilder(length);
			while (i < length)
			{
				char c = s[i];
				if (char.IsDigit(c))
				{
					break;
				}
				if (flag4 && char.IsWhiteSpace(c))
				{
					i++;
				}
				else if (flag8 && c == '(' && !flag && !flag2)
				{
					flag2 = true;
					flag = true;
					isNegative = true;
					i++;
				}
				else if (flag6 && c == nfi.NegativeSign[0] && !flag)
				{
					int length2 = nfi.NegativeSign.Length;
					if (length2 == 1 || s.IndexOf(nfi.NegativeSign, i, length2) == i)
					{
						flag = true;
						isNegative = true;
						i += length2;
					}
				}
				else if (flag6 && c == nfi.PositiveSign[0] && !flag)
				{
					int length3 = nfi.PositiveSign.Length;
					if (length3 == 1 || s.IndexOf(nfi.PositiveSign, i, length3) == i)
					{
						flag = true;
						i += length3;
					}
				}
				else
				{
					if (flag10 && c == text[0])
					{
						int length4 = text.Length;
						if (length4 != 1 && s.IndexOf(text, i, length4) != i)
						{
							if (!throwex)
							{
								return null;
							}
							decimal.ThrowAtPos(i);
						}
						break;
					}
					if (!throwex)
					{
						return null;
					}
					decimal.ThrowAtPos(i);
				}
			}
			if (i == length)
			{
				if (throwex)
				{
					throw new FormatException(Locale.GetText("No digits found"));
				}
				return null;
			}
			else
			{
				while (i < length)
				{
					char c2 = s[i];
					if (char.IsDigit(c2))
					{
						stringBuilder.Append(c2);
						i++;
					}
					else if (flag9 && c2 == text2[0])
					{
						int length5 = text2.Length;
						if (length5 != 1 && s.IndexOf(text2, i, length5) != i)
						{
							if (!throwex)
							{
								return null;
							}
							decimal.ThrowAtPos(i);
						}
						i += length5;
					}
					else
					{
						if (!flag10 || c2 != text[0] || flag3)
						{
							break;
						}
						int length6 = text.Length;
						if (length6 == 1 || s.IndexOf(text, i, length6) == i)
						{
							decPos = stringBuilder.Length;
							flag3 = true;
							i += length6;
						}
					}
				}
				if (i < length)
				{
					char c3 = s[i];
					if (flag11 && char.ToUpperInvariant(c3) == 'E')
					{
						expFlag = true;
						i++;
						if (i >= length)
						{
							if (!throwex)
							{
								return null;
							}
							decimal.ThrowInvalidExp();
						}
						c3 = s[i];
						bool flag13 = false;
						if (c3 == nfi.PositiveSign[0])
						{
							int length7 = nfi.PositiveSign.Length;
							if (length7 == 1 || s.IndexOf(nfi.PositiveSign, i, length7) == i)
							{
								i += length7;
								if (i >= length)
								{
									if (!throwex)
									{
										return null;
									}
									decimal.ThrowInvalidExp();
								}
							}
						}
						else if (c3 == nfi.NegativeSign[0])
						{
							int length8 = nfi.NegativeSign.Length;
							if (length8 == 1 || s.IndexOf(nfi.NegativeSign, i, length8) == i)
							{
								i += length8;
								if (i >= length)
								{
									if (!throwex)
									{
										return null;
									}
									decimal.ThrowInvalidExp();
								}
								flag13 = true;
							}
						}
						c3 = s[i];
						if (!char.IsDigit(c3))
						{
							if (!throwex)
							{
								return null;
							}
							decimal.ThrowInvalidExp();
						}
						exp = (int)(c3 - '0');
						i++;
						while (i < length && char.IsDigit(s[i]))
						{
							exp *= 10;
							exp += (int)(s[i] - '0');
							i++;
						}
						if (flag13)
						{
							exp *= -1;
						}
					}
				}
				while (i < length)
				{
					char c4 = s[i];
					if (flag5 && char.IsWhiteSpace(c4))
					{
						i++;
					}
					else if (flag8 && c4 == ')' && flag2)
					{
						flag2 = false;
						i++;
					}
					else if (flag7 && c4 == nfi.NegativeSign[0] && !flag)
					{
						int length9 = nfi.NegativeSign.Length;
						if (length9 == 1 || s.IndexOf(nfi.NegativeSign, i, length9) == i)
						{
							flag = true;
							isNegative = true;
							i += length9;
						}
					}
					else if (flag7 && c4 == nfi.PositiveSign[0] && !flag)
					{
						int length10 = nfi.PositiveSign.Length;
						if (length10 == 1 || s.IndexOf(nfi.PositiveSign, i, length10) == i)
						{
							flag = true;
							i += length10;
						}
					}
					else
					{
						if (!throwex)
						{
							return null;
						}
						decimal.ThrowAtPos(i);
					}
				}
				if (!flag2)
				{
					if (!flag3)
					{
						decPos = stringBuilder.Length;
					}
					return stringBuilder.ToString();
				}
				if (throwex)
				{
					throw new FormatException(Locale.GetText("Closing Parentheses not found"));
				}
				return null;
			}
		}

		/// <summary>Converts the string representation of a number to its <see cref="T:System.Decimal" /> equivalent using the specified style and culture-specific format.</summary>
		/// <returns>The <see cref="T:System.Decimal" /> number equivalent to the number contained in <paramref name="s" /> as specified by <paramref name="style" /> and <paramref name="provider" />.</returns>
		/// <param name="s">The string representation of the number to convert. </param>
		/// <param name="style">A bitwise combination of <see cref="T:System.Globalization.NumberStyles" /> values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Number" />.</param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> object that supplies culture-specific information about the format of <paramref name="s" />. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is not in the correct format. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="s" /> represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value. -or-<paramref name="style" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
		/// <filterpriority>1</filterpriority>
		public static decimal Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				throw new ArgumentException("Decimal.TryParse does not accept AllowHexSpecifier", "style");
			}
			decimal result;
			decimal.PerformParse(s, style, provider, out result, true);
			return result;
		}

		/// <summary>Converts the string representation of a number to its <see cref="T:System.Decimal" /> equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
		/// <returns>true if <paramref name="s" /> was converted successfully; otherwise, false.</returns>
		/// <param name="s">The string representation of the number to convert.</param>
		/// <param name="result">When this method returns, contains the <see cref="T:System.Decimal" /> number that is equivalent to the numeric value contained in <paramref name="s" />, if the conversion succeeded, or is zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is null, is not a number in a valid format, or represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. This parameter is passed uninitialized. </param>
		/// <filterpriority>1</filterpriority>
		public static bool TryParse(string s, out decimal result)
		{
			if (s == null)
			{
				result = 0m;
				return false;
			}
			return decimal.PerformParse(s, NumberStyles.Number, null, out result, false);
		}

		/// <summary>Converts the string representation of a number to its <see cref="T:System.Decimal" /> equivalent using the specified style and culture-specific format. A return value indicates whether the conversion succeeded or failed.</summary>
		/// <returns>true if <paramref name="s" /> was converted successfully; otherwise, false.</returns>
		/// <param name="s">The string representation of the number to convert.</param>
		/// <param name="style">A bitwise combination of <see cref="T:System.Globalization.NumberStyles" /> values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Number" />.</param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> object that supplies culture-specific parsing information about <paramref name="s" />. </param>
		/// <param name="result">When this method returns, contains the <see cref="T:System.Decimal" /> number that is equivalent to the numeric value contained in <paramref name="s" />, if the conversion succeeded, or is zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is null, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. This parameter is passed uninitialized. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value. -or-<paramref name="style" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
		/// <filterpriority>1</filterpriority>
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out decimal result)
		{
			if (s == null || (style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				result = 0m;
				return false;
			}
			return decimal.PerformParse(s, style, provider, out result, false);
		}

		private static bool PerformParse(string s, NumberStyles style, IFormatProvider provider, out decimal res, bool throwex)
		{
			NumberFormatInfo instance = NumberFormatInfo.GetInstance(provider);
			int num;
			bool flag;
			bool flag2;
			int exp;
			s = decimal.stripStyles(s, style, instance, out num, out flag, out flag2, out exp, throwex);
			if (s == null)
			{
				res = 0m;
				return false;
			}
			if (num < 0)
			{
				if (throwex)
				{
					throw new Exception(Locale.GetText("Error in System.Decimal.Parse"));
				}
				res = 0m;
				return false;
			}
			else
			{
				int length = s.Length;
				int num2 = 0;
				while (num2 < num && s[num2] == '0')
				{
					num2++;
				}
				if (num2 > 1 && length > 1)
				{
					s = s.Substring(num2, length - num2);
					num -= num2;
				}
				int num3 = (num != 0) ? 28 : 27;
				length = s.Length;
				if (length >= num3 + 1 && string.Compare(s, 0, "79228162514264337593543950335", 0, num3 + 1, false, CultureInfo.InvariantCulture) <= 0)
				{
					num3++;
				}
				if (length > num3 && num < length)
				{
					int num4 = (int)(s[num3] - '0');
					s = s.Substring(0, num3);
					bool flag3 = false;
					if (num4 > 5)
					{
						flag3 = true;
					}
					else if (num4 == 5)
					{
						if (flag)
						{
							flag3 = true;
						}
						else
						{
							int num5 = (int)(s[num3 - 1] - '0');
							flag3 = ((num5 & 1) == 1);
						}
					}
					if (flag3)
					{
						char[] array = s.ToCharArray();
						int i = num3 - 1;
						while (i >= 0)
						{
							int num6 = (int)(array[i] - '0');
							if (array[i] != '9')
							{
								array[i] = (char)(num6 + 49);
								break;
							}
							array[i--] = '0';
						}
						if (i == -1 && array[0] == '0')
						{
							num++;
							s = "1".PadRight(num, '0');
						}
						else
						{
							s = new string(array);
						}
					}
				}
				decimal num7;
				if (decimal.string2decimal(out num7, s, (uint)num, 0) != 0)
				{
					if (throwex)
					{
						throw new OverflowException();
					}
					res = 0m;
					return false;
				}
				else
				{
					if (!flag2 || decimal.decimalSetExponent(ref num7, exp) == 0)
					{
						if (flag)
						{
							num7.flags ^= 2147483648u;
						}
						res = num7;
						return true;
					}
					if (throwex)
					{
						throw new OverflowException();
					}
					res = 0m;
					return false;
				}
			}
		}

		/// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.Decimal" />.</summary>
		/// <returns>The enumerated constant <see cref="F:System.TypeCode.Decimal" />.</returns>
		/// <filterpriority>2</filterpriority>
		public TypeCode GetTypeCode()
		{
			return TypeCode.Decimal;
		}

		/// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 8-bit unsigned integer.</summary>
		/// <returns>An 8-bit unsigned integer equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The <see cref="T:System.Decimal" /> value. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(decimal value)
		{
			if (value > 255m || value < 0m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Byte.MaxValue or less than Byte.MinValue"));
			}
			return (byte)decimal.Truncate(value);
		}

		/// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent double-precision floating-point number.</summary>
		/// <returns>A double-precision floating-point number equivalent to <paramref name="d" />.</returns>
		/// <param name="d">The <see cref="T:System.Decimal" /> value to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static double ToDouble(decimal d)
		{
			return Convert.ToDouble(d);
		}

		/// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 16-bit signed integer.</summary>
		/// <returns>A 16-bit signed integer equivalent to <paramref name="value" />.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> value. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(decimal value)
		{
			if (value > 32767m || value < -32768m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int16.MaxValue or less than Int16.MinValue"));
			}
			return (short)decimal.Truncate(value);
		}

		/// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 32-bit signed integer.</summary>
		/// <returns>A 32-bit signed integer equivalent to the value of <paramref name="d" />.</returns>
		/// <param name="d">The <see cref="T:System.Decimal" /> value to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="d" /> is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(decimal d)
		{
			if (d > 2147483647m || d < -2147483648m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int32.MaxValue or less than Int32.MinValue"));
			}
			return (int)decimal.Truncate(d);
		}

		/// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer equivalent to the value of <paramref name="d" />.</returns>
		/// <param name="d">The <see cref="T:System.Decimal" /> value to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="d" /> is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(decimal d)
		{
			if (d > 9223372036854775807m || d < -9223372036854775808m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int64.MaxValue or less than Int64.MinValue"));
			}
			return (long)decimal.Truncate(d);
		}

		/// <summary>Converts the specified <see cref="T:System.Decimal" /> value to the equivalent OLE Automation Currency value, which is contained in a 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer that contains the OLE Automation equivalent of <paramref name="value" />.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> value. </param>
		/// <filterpriority>2</filterpriority>
		public static long ToOACurrency(decimal value)
		{
			return (long)(value * 10000m);
		}

		/// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 8-bit signed integer.</summary>
		/// <returns>An 8-bit signed integer equivalent to <paramref name="value" />.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> value. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(decimal value)
		{
			if (value > 127m || value < -128m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than SByte.MaxValue or less than SByte.MinValue"));
			}
			return (sbyte)decimal.Truncate(value);
		}

		/// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent single-precision floating-point number.</summary>
		/// <returns>A single-precision floating-point number equivalent to the value of <paramref name="d" />.</returns>
		/// <param name="d">A <see cref="T:System.Decimal" /> value to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static float ToSingle(decimal d)
		{
			return Convert.ToSingle(d);
		}

		/// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 16-bit unsigned integer.</summary>
		/// <returns>A 16-bit unsigned integer equivalent to the value of <paramref name="value" />.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> value to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.UInt16.MaxValue" /> or less than <see cref="F:System.UInt16.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(decimal value)
		{
			if (value > 65535m || value < 0m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt16.MaxValue or less than UInt16.MinValue"));
			}
			return (ushort)decimal.Truncate(value);
		}

		/// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 32-bit unsigned integer.</summary>
		/// <returns>A 32-bit unsigned integer equivalent to the value of <paramref name="d" />.</returns>
		/// <param name="d">A <see cref="T:System.Decimal" /> value to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="d" /> is negative or greater than <see cref="F:System.UInt32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(decimal d)
		{
			if (d > 4294967295m || d < 0m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt32.MaxValue or less than UInt32.MinValue"));
			}
			return (uint)decimal.Truncate(d);
		}

		/// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 64-bit unsigned integer.</summary>
		/// <returns>A 64-bit unsigned integer equivalent to the value of <paramref name="d" />.</returns>
		/// <param name="d">A <see cref="T:System.Decimal" /> value to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="d" /> is negative or greater than <see cref="F:System.UInt64.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(decimal d)
		{
			if (d > 18446744073709551615m || d < 0m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt64.MaxValue or less than UInt64.MinValue"));
			}
			return (ulong)decimal.Truncate(d);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
		/// <param name="format">A numeric format string (see Remarks).</param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public string ToString(string format, IFormatProvider provider)
		{
			return NumberFormatter.NumberToString(format, this, provider);
		}

		/// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
		/// <returns>A string that represents the value of this instance.</returns>
		/// <filterpriority>1</filterpriority>
		public override string ToString()
		{
			return this.ToString("G", null);
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

		/// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		public string ToString(IFormatProvider provider)
		{
			return this.ToString("G", provider);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int decimal2UInt64(ref decimal val, out ulong result);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int decimal2Int64(ref decimal val, out long result);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int decimalIncr(ref decimal d1, ref decimal d2);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int decimal2string(ref decimal val, int digits, int decimals, char[] bufDigits, int bufSize, out int decPos, out int sign);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int string2decimal(out decimal val, string sDigits, uint decPos, int sign);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int decimalSetExponent(ref decimal val, int exp);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern double decimal2double(ref decimal val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void decimalFloorAndTrunc(ref decimal val, int floorFlag);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int decimalMult(ref decimal pd1, ref decimal pd2);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int decimalDiv(out decimal pc, ref decimal pa, ref decimal pb);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int decimalIntDiv(out decimal pc, ref decimal pa, ref decimal pb);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int decimalCompare(ref decimal d1, ref decimal d2);

		/// <summary>Adds two specified <see cref="T:System.Decimal" /> values.</summary>
		/// <returns>The <see cref="T:System.Decimal" /> result of adding <paramref name="d1" /> and <paramref name="d2" />.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" />. </param>
		/// <param name="d2">A <see cref="T:System.Decimal" />. </param>
		/// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static decimal operator +(decimal d1, decimal d2)
		{
			return decimal.Add(d1, d2);
		}

		/// <summary>Decrements the <see cref="T:System.Decimal" /> operand by one.</summary>
		/// <returns>The value of <paramref name="d" /> decremented by 1.</returns>
		/// <param name="d">The <see cref="T:System.Decimal" /> operand. </param>
		/// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static decimal operator --(decimal d)
		{
			return decimal.Add(d, -1m);
		}

		/// <summary>Increments the <see cref="T:System.Decimal" /> operand by 1.</summary>
		/// <returns>The value of <paramref name="d" /> incremented by 1.</returns>
		/// <param name="d">The <see cref="T:System.Decimal" /> operand. </param>
		/// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static decimal operator ++(decimal d)
		{
			return decimal.Add(d, 1m);
		}

		/// <summary>Subtracts two specified <see cref="T:System.Decimal" /> values.</summary>
		/// <returns>The <see cref="T:System.Decimal" /> result of subtracting <paramref name="d2" /> from <paramref name="d1" />.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" />. </param>
		/// <param name="d2">A <see cref="T:System.Decimal" />. </param>
		/// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static decimal operator -(decimal d1, decimal d2)
		{
			return decimal.Subtract(d1, d2);
		}

		/// <summary>Negates the value of the specified <see cref="T:System.Decimal" /> operand.</summary>
		/// <returns>The result of <paramref name="d" /> multiplied by negative one (-1).</returns>
		/// <param name="d">The <see cref="T:System.Decimal" /> operand. </param>
		/// <filterpriority>3</filterpriority>
		public static decimal operator -(decimal d)
		{
			return decimal.Negate(d);
		}

		/// <summary>Returns the value of the <see cref="T:System.Decimal" /> operand (the sign of the operand is unchanged).</summary>
		/// <returns>The value of the operand, <paramref name="d" />.</returns>
		/// <param name="d">The <see cref="T:System.Decimal" /> operand. </param>
		/// <filterpriority>3</filterpriority>
		public static decimal operator +(decimal d)
		{
			return d;
		}

		/// <summary>Multiplies two specified <see cref="T:System.Decimal" /> values.</summary>
		/// <returns>The <see cref="T:System.Decimal" /> result of multiplying <paramref name="d1" /> by <paramref name="d2" />.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" />. </param>
		/// <param name="d2">A <see cref="T:System.Decimal" />. </param>
		/// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static decimal operator *(decimal d1, decimal d2)
		{
			return decimal.Multiply(d1, d2);
		}

		/// <summary>Divides two specified <see cref="T:System.Decimal" /> values.</summary>
		/// <returns>The <see cref="T:System.Decimal" /> result of <paramref name="d1" /> by <paramref name="d2" />.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" /> (the dividend). </param>
		/// <param name="d2">A <see cref="T:System.Decimal" /> (the divisor). </param>
		/// <exception cref="T:System.DivideByZeroException">
		///   <paramref name="d2" /> is zero. </exception>
		/// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static decimal operator /(decimal d1, decimal d2)
		{
			return decimal.Divide(d1, d2);
		}

		/// <summary>Returns the remainder resulting from dividing two specified <see cref="T:System.Decimal" /> values.</summary>
		/// <returns>The <see cref="T:System.Decimal" /> remainder resulting from dividing <paramref name="d1" /> by <paramref name="d2" />.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" /> (the dividend). </param>
		/// <param name="d2">A <see cref="T:System.Decimal" /> (the divisor). </param>
		/// <exception cref="T:System.DivideByZeroException">
		///   <paramref name="d2" /> is zero. </exception>
		/// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static decimal operator %(decimal d1, decimal d2)
		{
			return decimal.Remainder(d1, d2);
		}

		/// <summary>Converts a <see cref="T:System.Decimal" /> to an 8-bit unsigned integer.</summary>
		/// <returns>An 8-bit unsigned integer that represents the converted <see cref="T:System.Decimal" />.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static explicit operator byte(decimal value)
		{
			ulong num = decimal.u64(value);
			return checked((byte)num);
		}

		/// <summary>Converts a <see cref="T:System.Decimal" /> to an 8-bit signed integer.</summary>
		/// <returns>An 8-bit signed integer that represents the converted <see cref="T:System.Decimal" />.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		[CLSCompliant(false)]
		public static explicit operator sbyte(decimal value)
		{
			long num = decimal.s64(value);
			return checked((sbyte)num);
		}

		/// <summary>Converts a <see cref="T:System.Decimal" /> to a Unicode character.</summary>
		/// <returns>A Unicode character that represents the converted <see cref="T:System.Decimal" />.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static explicit operator char(decimal value)
		{
			ulong num = decimal.u64(value);
			return checked((char)num);
		}

		/// <summary>Converts a <see cref="T:System.Decimal" /> to a 16-bit signed integer.</summary>
		/// <returns>A 16-bit signed integer that represents the converted <see cref="T:System.Decimal" />.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static explicit operator short(decimal value)
		{
			long num = decimal.s64(value);
			return checked((short)num);
		}

		/// <summary>Converts a <see cref="T:System.Decimal" /> to a 16-bit unsigned integer.</summary>
		/// <returns>A 16-bit unsigned integer that represents the converted <see cref="T:System.Decimal" />.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.UInt16.MaxValue" /> or less than <see cref="F:System.UInt16.MinValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		[CLSCompliant(false)]
		public static explicit operator ushort(decimal value)
		{
			ulong num = decimal.u64(value);
			return checked((ushort)num);
		}

		/// <summary>Converts a <see cref="T:System.Decimal" /> to a 32-bit signed integer.</summary>
		/// <returns>A 32-bit signed integer that represents the converted <see cref="T:System.Decimal" />.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static explicit operator int(decimal value)
		{
			long num = decimal.s64(value);
			return checked((int)num);
		}

		/// <summary>Converts a <see cref="T:System.Decimal" /> to a 32-bit unsigned integer.</summary>
		/// <returns>A 32-bit unsigned integer that represents the converted <see cref="T:System.Decimal" />.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is negative or greater than <see cref="F:System.UInt32.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		[CLSCompliant(false)]
		public static explicit operator uint(decimal value)
		{
			ulong num = decimal.u64(value);
			return checked((uint)num);
		}

		/// <summary>Converts a <see cref="T:System.Decimal" /> to a 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer that represents the converted <see cref="T:System.Decimal" />.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static explicit operator long(decimal value)
		{
			return decimal.s64(value);
		}

		/// <summary>Converts a <see cref="T:System.Decimal" /> to a 64-bit unsigned integer.</summary>
		/// <returns>A 64-bit unsigned integer that represents the converted <see cref="T:System.Decimal" />.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is negative or greater than <see cref="F:System.UInt64.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		[CLSCompliant(false)]
		public static explicit operator ulong(decimal value)
		{
			return decimal.u64(value);
		}

		/// <summary>Converts an 8-bit unsigned integer to a <see cref="T:System.Decimal" />.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> that represents the converted 8-bit unsigned integer.</returns>
		/// <param name="value">An 8-bit unsigned integer. </param>
		/// <filterpriority>3</filterpriority>
		public static implicit operator decimal(byte value)
		{
			return new decimal((int)value);
		}

		/// <summary>Converts an 8-bit signed integer to a <see cref="T:System.Decimal" />.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> that represents the converted 8-bit signed integer.</returns>
		/// <param name="value">An 8-bit signed integer. </param>
		/// <filterpriority>3</filterpriority>
		[CLSCompliant(false)]
		public static implicit operator decimal(sbyte value)
		{
			return new decimal((int)value);
		}

		/// <summary>Converts a 16-bit signed integer to a <see cref="T:System.Decimal" />.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> that represents the converted 16-bit signed integer.</returns>
		/// <param name="value">A 16-bit signed integer. </param>
		/// <filterpriority>3</filterpriority>
		public static implicit operator decimal(short value)
		{
			return new decimal((int)value);
		}

		/// <summary>Converts a 16-bit unsigned integer to a <see cref="T:System.Decimal" />.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> that represents the converted 16-bit unsigned integer.</returns>
		/// <param name="value">A 16-bit unsigned integer. </param>
		/// <filterpriority>3</filterpriority>
		[CLSCompliant(false)]
		public static implicit operator decimal(ushort value)
		{
			return new decimal((int)value);
		}

		/// <summary>Converts a Unicode character to a <see cref="T:System.Decimal" />.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> that represents the converted Unicode character.</returns>
		/// <param name="value">A Unicode character. </param>
		/// <filterpriority>3</filterpriority>
		public static implicit operator decimal(char value)
		{
			return new decimal((int)value);
		}

		/// <summary>Converts a 32-bit signed integer to a <see cref="T:System.Decimal" />.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> that represents the converted 32-bit signed integer.</returns>
		/// <param name="value">A 32-bit signed integer. </param>
		/// <filterpriority>3</filterpriority>
		public static implicit operator decimal(int value)
		{
			return new decimal(value);
		}

		/// <summary>Converts a 32-bit unsigned integer to a <see cref="T:System.Decimal" />.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> that represents the converted 32-bit unsigned integer.</returns>
		/// <param name="value">A 32-bit unsigned integer. </param>
		/// <filterpriority>3</filterpriority>
		[CLSCompliant(false)]
		public static implicit operator decimal(uint value)
		{
			return new decimal(value);
		}

		/// <summary>Converts a 64-bit signed integer to a <see cref="T:System.Decimal" />.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> that represents the converted 64-bit signed integer.</returns>
		/// <param name="value">A 64-bit signed integer. </param>
		/// <filterpriority>3</filterpriority>
		public static implicit operator decimal(long value)
		{
			return new decimal(value);
		}

		/// <summary>Converts a 64-bit unsigned integer to a <see cref="T:System.Decimal" />.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> that represents the converted 64-bit unsigned integer.</returns>
		/// <param name="value">A 64-bit unsigned integer. </param>
		/// <filterpriority>3</filterpriority>
		[CLSCompliant(false)]
		public static implicit operator decimal(ulong value)
		{
			return new decimal(value);
		}

		/// <summary>Converts a single-precision floating-point number to a <see cref="T:System.Decimal" />.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> that represents the converted single-precision floating point number.</returns>
		/// <param name="value">A single-precision floating-point number. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.-or- <paramref name="value" /> is <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.PositiveInfinity" />, or <see cref="F:System.Single.NegativeInfinity" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static explicit operator decimal(float value)
		{
			return new decimal(value);
		}

		/// <summary>Converts a double-precision floating-point number to a <see cref="T:System.Decimal" />.</summary>
		/// <returns>A <see cref="T:System.Decimal" /> that represents the converted double-precision floating point number.</returns>
		/// <param name="value">A double-precision floating-point number. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.-or- <paramref name="value" /> is <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.PositiveInfinity" />, or <see cref="F:System.Double.NegativeInfinity" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static explicit operator decimal(double value)
		{
			return new decimal(value);
		}

		/// <summary>Converts a <see cref="T:System.Decimal" /> to a single-precision floating-point number.</summary>
		/// <returns>A single-precision floating-point number that represents the converted <see cref="T:System.Decimal" />.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> to convert. </param>
		/// <filterpriority>3</filterpriority>
		public static explicit operator float(decimal value)
		{
			return (float)((double)value);
		}

		/// <summary>Converts a <see cref="T:System.Decimal" /> to a double-precision floating-point number.</summary>
		/// <returns>A double-precision floating-point number that represents the converted <see cref="T:System.Decimal" />.</returns>
		/// <param name="value">A <see cref="T:System.Decimal" /> to convert. </param>
		/// <filterpriority>3</filterpriority>
		public static explicit operator double(decimal value)
		{
			return decimal.decimal2double(ref value);
		}

		/// <summary>Returns a value indicating whether two instances of <see cref="T:System.Decimal" /> are not equal.</summary>
		/// <returns>true if <paramref name="d1" /> and <paramref name="d2" /> are not equal; otherwise, false.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" />. </param>
		/// <param name="d2">A <see cref="T:System.Decimal" />. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator !=(decimal d1, decimal d2)
		{
			return !decimal.Equals(d1, d2);
		}

		/// <summary>Returns a value indicating whether two instances of <see cref="T:System.Decimal" /> are equal.</summary>
		/// <returns>true if <paramref name="d1" /> and <paramref name="d2" /> are equal; otherwise, false.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" />. </param>
		/// <param name="d2">A <see cref="T:System.Decimal" />. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator ==(decimal d1, decimal d2)
		{
			return decimal.Equals(d1, d2);
		}

		/// <summary>Returns a value indicating whether a specified <see cref="T:System.Decimal" /> is greater than another specified <see cref="T:System.Decimal" />.</summary>
		/// <returns>true if <paramref name="d1" /> is greater than <paramref name="d2" />; otherwise, false.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" />. </param>
		/// <param name="d2">A <see cref="T:System.Decimal" />. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator >(decimal d1, decimal d2)
		{
			return decimal.Compare(d1, d2) > 0;
		}

		/// <summary>Returns a value indicating whether a specified <see cref="T:System.Decimal" /> is greater than or equal to another specified <see cref="T:System.Decimal" />.</summary>
		/// <returns>true if <paramref name="d1" /> is greater than or equal to <paramref name="d2" />; otherwise, false.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" />. </param>
		/// <param name="d2">A <see cref="T:System.Decimal" />. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator >=(decimal d1, decimal d2)
		{
			return decimal.Compare(d1, d2) >= 0;
		}

		/// <summary>Returns a value indicating whether a specified <see cref="T:System.Decimal" /> is less than another specified <see cref="T:System.Decimal" />.</summary>
		/// <returns>true if <paramref name="d1" /> is less than <paramref name="d2" />; otherwise, false.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" />. </param>
		/// <param name="d2">A <see cref="T:System.Decimal" />. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator <(decimal d1, decimal d2)
		{
			return decimal.Compare(d1, d2) < 0;
		}

		/// <summary>Returns a value indicating whether a specified <see cref="T:System.Decimal" /> is less than or equal to another specified <see cref="T:System.Decimal" />.</summary>
		/// <returns>true if <paramref name="d1" /> is less than or equal to <paramref name="d2" />; otherwise, false.</returns>
		/// <param name="d1">A <see cref="T:System.Decimal" />. </param>
		/// <param name="d2">A <see cref="T:System.Decimal" />. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator <=(decimal d1, decimal d2)
		{
			return decimal.Compare(d1, d2) <= 0;
		}
	}
}
