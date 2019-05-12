using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Provides the base class for enumerations.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public abstract class Enum : ValueType, IFormattable, IConvertible, IComparable
	{
		private static char[] split_char = new char[]
		{
			','
		};

		/// <summary>Converts the current value to a Boolean value based on the underlying type.</summary>
		/// <returns>This member always throws an exception.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <exception cref="T:System.InvalidCastException">In all cases. </exception>
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this.Value, provider);
		}

		/// <summary>Converts the current value to an 8-bit unsigned integer based on the underlying type.</summary>
		/// <returns>The converted value.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this.Value, provider);
		}

		/// <summary>Converts the current value to a Unicode character based on the underlying type.</summary>
		/// <returns>This member always throws an exception.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <exception cref="T:System.InvalidCastException">In all cases. </exception>
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this.Value, provider);
		}

		/// <summary>Converts the current value to a <see cref="T:System.DateTime" /> based on the underlying type.</summary>
		/// <returns>This member always throws an exception.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <exception cref="T:System.InvalidCastException">In all cases. </exception>
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return Convert.ToDateTime(this.Value, provider);
		}

		/// <summary>Converts the current value to a <see cref="T:System.Decimal" /> based on the underlying type.</summary>
		/// <returns>This member always throws an exception.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <exception cref="T:System.InvalidCastException">In all cases. </exception>
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this.Value, provider);
		}

		/// <summary>Converts the current value to a double-precision floating point number based on the underlying type.</summary>
		/// <returns>This member always throws an exception.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <exception cref="T:System.InvalidCastException">In all cases. </exception>
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this.Value, provider);
		}

		/// <summary>Converts the current value to a 16-bit signed integer based on the underlying type.</summary>
		/// <returns>The converted value.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this.Value, provider);
		}

		/// <summary>Converts the current value to a 32-bit signed integer based on the underlying type.</summary>
		/// <returns>The converted value.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this.Value, provider);
		}

		/// <summary>Converts the current value to a 64-bit signed integer based on the underlying type.</summary>
		/// <returns>The converted value.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this.Value, provider);
		}

		/// <summary>Converts the current value to an 8-bit signed integer based on the underlying type.</summary>
		/// <returns>The converted value.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this.Value, provider);
		}

		/// <summary>Converts the current value to a single-precision floating point number based on the underlying type.</summary>
		/// <returns>This member always throws an exception.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <exception cref="T:System.InvalidCastException">In all cases. </exception>
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this.Value, provider);
		}

		/// <summary>Converts the current value to a specified type based on the underlying type.</summary>
		/// <returns>The converted value.</returns>
		/// <param name="type">The type to convert to. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		object IConvertible.ToType(Type targetType, IFormatProvider provider)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			if (targetType == typeof(string))
			{
				return this.ToString(provider);
			}
			return Convert.ToType(this.Value, targetType, provider, false);
		}

		/// <summary>Converts the current value to a 16-bit unsigned integer based on the underlying type.</summary>
		/// <returns>The converted value.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this.Value, provider);
		}

		/// <summary>Converts the current value to a 32-bit unsigned integer based on the underlying type.</summary>
		/// <returns>The converted value.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this.Value, provider);
		}

		/// <summary>Converts the current value to a 64-bit unsigned integer based on the underlying type.</summary>
		/// <returns>The converted value.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this.Value, provider);
		}

		/// <summary>Returns the underlying <see cref="T:System.TypeCode" /> for this instance.</summary>
		/// <returns>The <see cref="T:System.TypeCode" /> for this instance.</returns>
		/// <exception cref="T:System.InvalidOperationException">The enumeration type is unknown.</exception>
		/// <filterpriority>2</filterpriority>
		public TypeCode GetTypeCode()
		{
			return Type.GetTypeCode(Enum.GetUnderlyingType(base.GetType()));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object get_value();

		private object Value
		{
			get
			{
				return this.get_value();
			}
		}

		/// <summary>Retrieves an array of the values of the constants in a specified enumeration.</summary>
		/// <returns>An <see cref="T:System.Array" /> of the values of the constants in <paramref name="enumType" />. The elements of the array are sorted by the binary values of the enumeration constants.</returns>
		/// <param name="enumType">An enumeration type. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> is not an <see cref="T:System.Enum" />. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		public static Array GetValues(Type enumType)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("enumType is not an Enum type.", "enumType");
			}
			MonoEnumInfo monoEnumInfo;
			MonoEnumInfo.GetInfo(enumType, out monoEnumInfo);
			return (Array)monoEnumInfo.values.Clone();
		}

		/// <summary>Retrieves an array of the names of the constants in a specified enumeration.</summary>
		/// <returns>A string array of the names of the constants in <paramref name="enumType" />. </returns>
		/// <param name="enumType">An enumeration type. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> parameter is not an <see cref="T:System.Enum" />. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		public static string[] GetNames(Type enumType)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("enumType is not an Enum type.");
			}
			MonoEnumInfo monoEnumInfo;
			MonoEnumInfo.GetInfo(enumType, out monoEnumInfo);
			return (string[])monoEnumInfo.names.Clone();
		}

		private static int FindPosition(object value, Array values)
		{
			if (!(values is byte[]) && !(values is ushort[]) && !(values is uint[]) && !(values is ulong[]))
			{
				if (values is int[])
				{
					return Array.BinarySearch(values, value, MonoEnumInfo.int_comparer);
				}
				if (values is short[])
				{
					return Array.BinarySearch(values, value, MonoEnumInfo.short_comparer);
				}
				if (values is sbyte[])
				{
					return Array.BinarySearch(values, value, MonoEnumInfo.sbyte_comparer);
				}
				if (values is long[])
				{
					return Array.BinarySearch(values, value, MonoEnumInfo.long_comparer);
				}
			}
			return Array.BinarySearch(values, value);
		}

		/// <summary>Retrieves the name of the constant in the specified enumeration that has the specified value.</summary>
		/// <returns>A string containing the name of the enumerated constant in <paramref name="enumType" /> whose value is <paramref name="value" />, or null if no such constant is found.</returns>
		/// <param name="enumType">An enumeration type. </param>
		/// <param name="value">The value of a particular enumerated constant in terms of its underlying type. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> or <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.-or- <paramref name="value" /> is neither of type <paramref name="enumType" /> nor does it have the same underlying type as <paramref name="enumType" />. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		public static string GetName(Type enumType, object value)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("enumType is not an Enum type.", "enumType");
			}
			value = Enum.ToObject(enumType, value);
			MonoEnumInfo monoEnumInfo;
			MonoEnumInfo.GetInfo(enumType, out monoEnumInfo);
			int num = Enum.FindPosition(value, monoEnumInfo.values);
			return (num < 0) ? null : monoEnumInfo.names[num];
		}

		/// <summary>Returns an indication whether a constant with a specified value exists in a specified enumeration.</summary>
		/// <returns>true if a constant in <paramref name="enumType" /> has a value equal to <paramref name="value" />; otherwise, false.</returns>
		/// <param name="enumType">An enumeration type. </param>
		/// <param name="value">The value or name of a constant in <paramref name="enumType" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> or <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> is not an Enum.-or- The type of <paramref name="value" /> is an enumeration, but it is not an enumeration of type <paramref name="enumType" />.-or- The type of <paramref name="value" /> is not an underlying type of <paramref name="enumType" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="value" /> is not type <see cref="T:System.SByte" />, <see cref="T:System.Int16" />, <see cref="T:System.Int32" />, <see cref="T:System.Int64" />, <see cref="T:System.Byte" />, <see cref="T:System.UInt16" />, <see cref="T:System.UInt32" />, or <see cref="T:System.UInt64" />, or <see cref="T:System.String" />. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		public static bool IsDefined(Type enumType, object value)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("enumType is not an Enum type.", "enumType");
			}
			MonoEnumInfo monoEnumInfo;
			MonoEnumInfo.GetInfo(enumType, out monoEnumInfo);
			Type type = value.GetType();
			if (type == typeof(string))
			{
				return ((IList)monoEnumInfo.names).Contains(value);
			}
			if (type == monoEnumInfo.utype || type == enumType)
			{
				value = Enum.ToObject(enumType, value);
				MonoEnumInfo.GetInfo(enumType, out monoEnumInfo);
				return Enum.FindPosition(value, monoEnumInfo.values) >= 0;
			}
			throw new ArgumentException("The value parameter is not the correct type.It must be type String or the same type as the underlying typeof the Enum.");
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type get_underlying_type(Type enumType);

		/// <summary>Returns the underlying type of the specified enumeration.</summary>
		/// <returns>The underlying <see cref="T:System.Type" /> of <paramref name="enumType" />.</returns>
		/// <param name="enumType">An enumeration type. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> is not an <see cref="T:System.Enum" />. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		public static Type GetUnderlyingType(Type enumType)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("enumType is not an Enum type.", "enumType");
			}
			return Enum.get_underlying_type(enumType);
		}

		/// <summary>Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.</summary>
		/// <returns>An object of type <paramref name="enumType" /> whose value is represented by <paramref name="value" />.</returns>
		/// <param name="enumType">The <see cref="T:System.Type" /> of the enumeration. </param>
		/// <param name="value">A string containing the name or value to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> or <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.-or- <paramref name="value" /> is either an empty string or only contains white space.-or- <paramref name="value" /> is a name, but not one of the named constants defined for the enumeration. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is outside the range of the underlying type of <paramref name="enumType" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		public static object Parse(Type enumType, string value)
		{
			return Enum.Parse(enumType, value, false);
		}

		private static int FindName(Hashtable name_hash, string[] names, string name, bool ignoreCase)
		{
			if (!ignoreCase)
			{
				if (name_hash != null)
				{
					object obj = name_hash[name];
					if (obj != null)
					{
						return (int)obj;
					}
				}
				else
				{
					for (int i = 0; i < names.Length; i++)
					{
						if (name == names[i])
						{
							return i;
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < names.Length; j++)
				{
					if (string.Compare(name, names[j], ignoreCase, CultureInfo.InvariantCulture) == 0)
					{
						return j;
					}
				}
			}
			return -1;
		}

		private static ulong GetValue(object value, TypeCode typeCode)
		{
			switch (typeCode)
			{
			case TypeCode.SByte:
				return (ulong)((byte)((sbyte)value));
			case TypeCode.Byte:
				return (ulong)((byte)value);
			case TypeCode.Int16:
				return (ulong)((ushort)((short)value));
			case TypeCode.UInt16:
				return (ulong)((ushort)value);
			case TypeCode.Int32:
				return (ulong)((int)value);
			case TypeCode.UInt32:
				return (ulong)((uint)value);
			case TypeCode.Int64:
				return (ulong)((long)value);
			case TypeCode.UInt64:
				return (ulong)value;
			default:
				throw new ArgumentException("typeCode is not a valid type code for an Enum");
			}
		}

		/// <summary>Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object. A parameter specifies whether the operation is case-sensitive.</summary>
		/// <returns>An object of type <paramref name="enumType" /> whose value is represented by <paramref name="value" />.</returns>
		/// <param name="enumType">The <see cref="T:System.Type" /> of the enumeration. </param>
		/// <param name="value">A string containing the name or value to convert. </param>
		/// <param name="ignoreCase">If true, ignore case; otherwise, regard case. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> or <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.-or- <paramref name="value" /> is either an empty string ("") or only contains white space.-or- <paramref name="value" /> is a name, but not one of the named constants defined for the enumeration. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is outside the range of the underlying type of <paramref name="enumType" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		public static object Parse(Type enumType, string value, bool ignoreCase)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("enumType is not an Enum type.", "enumType");
			}
			value = value.Trim();
			if (value.Length == 0)
			{
				throw new ArgumentException("An empty string is not considered a valid value.");
			}
			MonoEnumInfo monoEnumInfo;
			MonoEnumInfo.GetInfo(enumType, out monoEnumInfo);
			int num = Enum.FindName(monoEnumInfo.name_hash, monoEnumInfo.names, value, ignoreCase);
			if (num >= 0)
			{
				return monoEnumInfo.values.GetValue(num);
			}
			TypeCode typeCode = ((Enum)monoEnumInfo.values.GetValue(0)).GetTypeCode();
			if (value.IndexOf(',') != -1)
			{
				string[] array = value.Split(Enum.split_char);
				ulong num2 = 0UL;
				for (int i = 0; i < array.Length; i++)
				{
					num = Enum.FindName(monoEnumInfo.name_hash, monoEnumInfo.names, array[i].Trim(), ignoreCase);
					if (num < 0)
					{
						throw new ArgumentException("The requested value was not found.");
					}
					num2 |= Enum.GetValue(monoEnumInfo.values.GetValue(num), typeCode);
				}
				return Enum.ToObject(enumType, num2);
			}
			switch (typeCode)
			{
			case TypeCode.SByte:
			{
				sbyte value2;
				if (sbyte.TryParse(value, out value2))
				{
					return Enum.ToObject(enumType, value2);
				}
				break;
			}
			case TypeCode.Byte:
			{
				byte value3;
				if (byte.TryParse(value, out value3))
				{
					return Enum.ToObject(enumType, value3);
				}
				break;
			}
			case TypeCode.Int16:
			{
				short value4;
				if (short.TryParse(value, out value4))
				{
					return Enum.ToObject(enumType, value4);
				}
				break;
			}
			case TypeCode.UInt16:
			{
				ushort value5;
				if (ushort.TryParse(value, out value5))
				{
					return Enum.ToObject(enumType, value5);
				}
				break;
			}
			case TypeCode.Int32:
			{
				int value6;
				if (int.TryParse(value, out value6))
				{
					return Enum.ToObject(enumType, value6);
				}
				break;
			}
			case TypeCode.UInt32:
			{
				uint value7;
				if (uint.TryParse(value, out value7))
				{
					return Enum.ToObject(enumType, value7);
				}
				break;
			}
			case TypeCode.Int64:
			{
				long value8;
				if (long.TryParse(value, out value8))
				{
					return Enum.ToObject(enumType, value8);
				}
				break;
			}
			case TypeCode.UInt64:
			{
				ulong value9;
				if (ulong.TryParse(value, out value9))
				{
					return Enum.ToObject(enumType, value9);
				}
				break;
			}
			}
			throw new ArgumentException(string.Format("The requested value '{0}' was not found.", value));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int compare_value_to(object other);

		/// <summary>Compares this instance to a specified object and returns an indication of their relative values.</summary>
		/// <returns>A signed number indicating the relative values of this instance and <paramref name="target" />.Return Value Description Less than zero The value of this instance is less than the value of <paramref name="target" />. Zero The value of this instance is equal to the value of <paramref name="target" />. Greater than zero The value of this instance is greater than the value of <paramref name="target" />.-or- <paramref name="target" /> is null. </returns>
		/// <param name="target">An object to compare, or null. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="target" /> and this instance are not the same type. </exception>
		/// <exception cref="T:System.InvalidOperationException">This instance is not type <see cref="T:System.SByte" />, <see cref="T:System.Int16" />, <see cref="T:System.Int32" />, <see cref="T:System.Int64" />, <see cref="T:System.Byte" />, <see cref="T:System.UInt16" />, <see cref="T:System.UInt32" />, or <see cref="T:System.UInt64" />. </exception>
		/// <exception cref="T:System.NullReferenceException">This instance is null.</exception>
		/// <filterpriority>2</filterpriority>
		public int CompareTo(object target)
		{
			if (target == null)
			{
				return 1;
			}
			Type type = base.GetType();
			if (target.GetType() != type)
			{
				throw new ArgumentException(string.Format("Object must be the same type as the enum. The type passed in was {0}; the enum type was {1}.", target.GetType(), type));
			}
			return this.compare_value_to(target);
		}

		/// <summary>Converts the value of this instance to its equivalent string representation.</summary>
		/// <returns>The string representation of the value of this instance.</returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return this.ToString("G");
		}

		/// <summary>This method overload is obsolete; use <see cref="M:System.Enum.ToString" />.</summary>
		/// <returns>The string representation of the value of this instance.</returns>
		/// <param name="provider">(obsolete) </param>
		/// <filterpriority>2</filterpriority>
		[Obsolete("Provider is ignored, just use ToString")]
		public string ToString(IFormatProvider provider)
		{
			return this.ToString("G", provider);
		}

		/// <summary>Converts the value of this instance to its equivalent string representation using the specified format.</summary>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
		/// <param name="format">A format string. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> contains an invalid specification. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="format" /> equals "X", but the enumeration type is unknown.</exception>
		/// <filterpriority>2</filterpriority>
		public string ToString(string format)
		{
			if (format == string.Empty || format == null)
			{
				format = "G";
			}
			return Enum.Format(base.GetType(), this.Value, format);
		}

		/// <summary>This method overload is obsolete; use <see cref="M:System.Enum.ToString(System.String)" />.</summary>
		/// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
		/// <param name="format">A format specification. </param>
		/// <param name="provider">(obsolete)</param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> does not contain a valid format specification. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="format" /> equals "X", but the enumeration type is unknown.</exception>
		/// <filterpriority>2</filterpriority>
		[Obsolete("Provider is ignored, just use ToString")]
		public string ToString(string format, IFormatProvider provider)
		{
			if (format == string.Empty || format == null)
			{
				format = "G";
			}
			return Enum.Format(base.GetType(), this.Value, format);
		}

		/// <summary>Returns an instance of the specified enumeration type set to the specified 8-bit unsigned integer value.</summary>
		/// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
		/// <param name="enumType">The enumeration for which to create a value. </param>
		/// <param name="value">The value to set. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> is not an <see cref="T:System.Enum" />. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		public static object ToObject(Type enumType, byte value)
		{
			return Enum.ToObject(enumType, value);
		}

		/// <summary>Returns an instance of the specified enumeration type set to the specified 16-bit signed integer value.</summary>
		/// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
		/// <param name="enumType">The enumeration for which to create a value. </param>
		/// <param name="value">The value to set. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> is not an <see cref="T:System.Enum" />. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		public static object ToObject(Type enumType, short value)
		{
			return Enum.ToObject(enumType, value);
		}

		/// <summary>Returns an instance of the specified enumeration type set to the specified 32-bit signed integer value.</summary>
		/// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
		/// <param name="enumType">The enumeration for which to create a value. </param>
		/// <param name="value">The value to set. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> is not an <see cref="T:System.Enum" />. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		public static object ToObject(Type enumType, int value)
		{
			return Enum.ToObject(enumType, value);
		}

		/// <summary>Returns an instance of the specified enumeration type set to the specified 64-bit signed integer value.</summary>
		/// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
		/// <param name="enumType">The enumeration for which to create a value. </param>
		/// <param name="value">The value to set. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> is not an <see cref="T:System.Enum" />. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		public static object ToObject(Type enumType, long value)
		{
			return Enum.ToObject(enumType, value);
		}

		/// <summary>Returns an instance of the specified enumeration set to the specified value.</summary>
		/// <returns>An enumeration object whose value is <paramref name="value" />.</returns>
		/// <param name="enumType">An enumeration. </param>
		/// <param name="value">The value. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.-or- <paramref name="value" /> is not type <see cref="T:System.SByte" />, <see cref="T:System.Int16" />, <see cref="T:System.Int32" />, <see cref="T:System.Int64" />, <see cref="T:System.Byte" />, <see cref="T:System.UInt16" />, <see cref="T:System.UInt32" />, or <see cref="T:System.UInt64" />. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object ToObject(Type enumType, object value);

		/// <summary>Returns an instance of the specified enumeration type set to the specified 8-bit signed integer value.</summary>
		/// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
		/// <param name="enumType">The enumeration for which to create a value. </param>
		/// <param name="value">The value to set. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> is not an <see cref="T:System.Enum" />. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		[CLSCompliant(false)]
		public static object ToObject(Type enumType, sbyte value)
		{
			return Enum.ToObject(enumType, value);
		}

		/// <summary>Returns an instance of the specified enumeration type set to the specified 16-bit unsigned integer value.</summary>
		/// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
		/// <param name="enumType">The enumeration for which to create a value. </param>
		/// <param name="value">The value to set. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> is not an <see cref="T:System.Enum" />. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		[CLSCompliant(false)]
		public static object ToObject(Type enumType, ushort value)
		{
			return Enum.ToObject(enumType, value);
		}

		/// <summary>Returns an instance of the specified enumeration type set to the specified 32-bit unsigned integer value.</summary>
		/// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
		/// <param name="enumType">The enumeration for which to create a value. </param>
		/// <param name="value">The value to set. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> is not an <see cref="T:System.Enum" />. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		[CLSCompliant(false)]
		public static object ToObject(Type enumType, uint value)
		{
			return Enum.ToObject(enumType, value);
		}

		/// <summary>Returns an instance of the specified enumeration type set to the specified 64-bit unsigned integer value.</summary>
		/// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
		/// <param name="enumType">The enumeration for which to create a value. </param>
		/// <param name="value">The value to set. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="enumType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enumType" /> is not an <see cref="T:System.Enum" />. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		[CLSCompliant(false)]
		public static object ToObject(Type enumType, ulong value)
		{
			return Enum.ToObject(enumType, value);
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
		/// <returns>true if <paramref name="obj" /> is an <see cref="T:System.Enum" /> with the same underlying type and value as this instance; otherwise, false.</returns>
		/// <param name="obj">An object to compare with this instance, or null. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			return ValueType.DefaultEquals(this, obj);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int get_hashcode();

		/// <summary>Returns the hash code for the value of this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return this.get_hashcode();
		}

		private static string FormatSpecifier_X(Type enumType, object value, bool upper)
		{
			switch (Type.GetTypeCode(enumType))
			{
			case TypeCode.SByte:
				return ((sbyte)value).ToString((!upper) ? "x2" : "X2");
			case TypeCode.Byte:
				return ((byte)value).ToString((!upper) ? "x2" : "X2");
			case TypeCode.Int16:
				return ((short)value).ToString((!upper) ? "x4" : "X4");
			case TypeCode.UInt16:
				return ((ushort)value).ToString((!upper) ? "x4" : "X4");
			case TypeCode.Int32:
				return ((int)value).ToString((!upper) ? "x8" : "X8");
			case TypeCode.UInt32:
				return ((uint)value).ToString((!upper) ? "x8" : "X8");
			case TypeCode.Int64:
				return ((long)value).ToString((!upper) ? "x16" : "X16");
			case TypeCode.UInt64:
				return ((ulong)value).ToString((!upper) ? "x16" : "X16");
			default:
				throw new Exception("Invalid type code for enumeration.");
			}
		}

		private static string FormatFlags(Type enumType, object value)
		{
			string text = string.Empty;
			MonoEnumInfo monoEnumInfo;
			MonoEnumInfo.GetInfo(enumType, out monoEnumInfo);
			string text2 = value.ToString();
			if (text2 == "0")
			{
				text = Enum.GetName(enumType, value);
				if (text == null)
				{
					text = text2;
				}
				return text;
			}
			switch (((Enum)monoEnumInfo.values.GetValue(0)).GetTypeCode())
			{
			case TypeCode.SByte:
			{
				sbyte b = (sbyte)value;
				for (int i = monoEnumInfo.values.Length - 1; i >= 0; i--)
				{
					sbyte b2 = (sbyte)monoEnumInfo.values.GetValue(i);
					if ((int)b2 != 0)
					{
						if (((int)b & (int)b2) == (int)b2)
						{
							text = monoEnumInfo.names[i] + ((!(text == string.Empty)) ? ", " : string.Empty) + text;
							b = (sbyte)((int)b - (int)b2);
						}
					}
				}
				if ((int)b != 0)
				{
					return text2;
				}
				break;
			}
			case TypeCode.Byte:
			{
				byte b3 = (byte)value;
				for (int j = monoEnumInfo.values.Length - 1; j >= 0; j--)
				{
					byte b4 = (byte)monoEnumInfo.values.GetValue(j);
					if (b4 != 0)
					{
						if ((b3 & b4) == b4)
						{
							text = monoEnumInfo.names[j] + ((!(text == string.Empty)) ? ", " : string.Empty) + text;
							b3 -= b4;
						}
					}
				}
				if (b3 != 0)
				{
					return text2;
				}
				break;
			}
			case TypeCode.Int16:
			{
				short num = (short)value;
				for (int k = monoEnumInfo.values.Length - 1; k >= 0; k--)
				{
					short num2 = (short)monoEnumInfo.values.GetValue(k);
					if (num2 != 0)
					{
						if ((num & num2) == num2)
						{
							text = monoEnumInfo.names[k] + ((!(text == string.Empty)) ? ", " : string.Empty) + text;
							num -= num2;
						}
					}
				}
				if (num != 0)
				{
					return text2;
				}
				break;
			}
			case TypeCode.UInt16:
			{
				ushort num3 = (ushort)value;
				for (int l = monoEnumInfo.values.Length - 1; l >= 0; l--)
				{
					ushort num4 = (ushort)monoEnumInfo.values.GetValue(l);
					if (num4 != 0)
					{
						if ((num3 & num4) == num4)
						{
							text = monoEnumInfo.names[l] + ((!(text == string.Empty)) ? ", " : string.Empty) + text;
							num3 -= num4;
						}
					}
				}
				if (num3 != 0)
				{
					return text2;
				}
				break;
			}
			case TypeCode.Int32:
			{
				int num5 = (int)value;
				for (int m = monoEnumInfo.values.Length - 1; m >= 0; m--)
				{
					int num6 = (int)monoEnumInfo.values.GetValue(m);
					if (num6 != 0)
					{
						if ((num5 & num6) == num6)
						{
							text = monoEnumInfo.names[m] + ((!(text == string.Empty)) ? ", " : string.Empty) + text;
							num5 -= num6;
						}
					}
				}
				if (num5 != 0)
				{
					return text2;
				}
				break;
			}
			case TypeCode.UInt32:
			{
				uint num7 = (uint)value;
				for (int n = monoEnumInfo.values.Length - 1; n >= 0; n--)
				{
					uint num8 = (uint)monoEnumInfo.values.GetValue(n);
					if (num8 != 0u)
					{
						if ((num7 & num8) == num8)
						{
							text = monoEnumInfo.names[n] + ((!(text == string.Empty)) ? ", " : string.Empty) + text;
							num7 -= num8;
						}
					}
				}
				if (num7 != 0u)
				{
					return text2;
				}
				break;
			}
			case TypeCode.Int64:
			{
				long num9 = (long)value;
				for (int num10 = monoEnumInfo.values.Length - 1; num10 >= 0; num10--)
				{
					long num11 = (long)monoEnumInfo.values.GetValue(num10);
					if (num11 != 0L)
					{
						if ((num9 & num11) == num11)
						{
							text = monoEnumInfo.names[num10] + ((!(text == string.Empty)) ? ", " : string.Empty) + text;
							num9 -= num11;
						}
					}
				}
				if (num9 != 0L)
				{
					return text2;
				}
				break;
			}
			case TypeCode.UInt64:
			{
				ulong num12 = (ulong)value;
				for (int num13 = monoEnumInfo.values.Length - 1; num13 >= 0; num13--)
				{
					ulong num14 = (ulong)monoEnumInfo.values.GetValue(num13);
					if (num14 != 0UL)
					{
						if ((num12 & num14) == num14)
						{
							text = monoEnumInfo.names[num13] + ((!(text == string.Empty)) ? ", " : string.Empty) + text;
							num12 -= num14;
						}
					}
				}
				if (num12 != 0UL)
				{
					return text2;
				}
				break;
			}
			}
			if (text == string.Empty)
			{
				return text2;
			}
			return text;
		}

		/// <summary>Converts the specified value of a specified enumerated type to its equivalent string representation according to the specified format.</summary>
		/// <returns>A string representation of <paramref name="value" />.</returns>
		/// <param name="enumType">The enumeration type of the value to convert. </param>
		/// <param name="value">The value to convert. </param>
		/// <param name="format">The output format to use. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="enumType" />, <paramref name="value" />, or <paramref name="format" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="enumType" /> parameter is not an <see cref="T:System.Enum" /> type.-or- The <paramref name="value" /> is from an enumeration that differs in type from <paramref name="enumType" />.-or- The type of <paramref name="value" /> is not an underlying type of <paramref name="enumType" />. </exception>
		/// <exception cref="T:System.FormatException">The <paramref name="format" /> parameter contains an invalid value. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="format" /> equals "X", but the enumeration type is unknown.</exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(true)]
		public static string Format(Type enumType, object value, string format)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("enumType is not an Enum type.", "enumType");
			}
			Type type = value.GetType();
			Type underlyingType = Enum.GetUnderlyingType(enumType);
			if (type.IsEnum)
			{
				if (type != enumType)
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Object must be the same type as the enum. The type passed in was {0}; the enum type was {1}.", new object[]
					{
						type.FullName,
						enumType.FullName
					}));
				}
			}
			else if (type != underlyingType)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Enum underlying type and the object must be the same type or object. Type passed in was {0}; the enum underlying type was {1}.", new object[]
				{
					type.FullName,
					underlyingType.FullName
				}));
			}
			if (format.Length != 1)
			{
				throw new FormatException("Format String can be only \"G\",\"g\",\"X\",\"x\",\"F\",\"f\",\"D\" or \"d\".");
			}
			char c = format[0];
			string text;
			if (c == 'G' || c == 'g')
			{
				if (!enumType.IsDefined(typeof(FlagsAttribute), false))
				{
					text = Enum.GetName(enumType, value);
					if (text == null)
					{
						text = value.ToString();
					}
					return text;
				}
				c = 'f';
			}
			if (c == 'f' || c == 'F')
			{
				return Enum.FormatFlags(enumType, value);
			}
			text = string.Empty;
			char c2 = c;
			if (c2 != 'D')
			{
				if (c2 == 'X')
				{
					return Enum.FormatSpecifier_X(enumType, value, true);
				}
				if (c2 != 'd')
				{
					if (c2 != 'x')
					{
						throw new FormatException("Format String can be only \"G\",\"g\",\"X\",\"x\",\"F\",\"f\",\"D\" or \"d\".");
					}
					return Enum.FormatSpecifier_X(enumType, value, false);
				}
			}
			if (underlyingType == typeof(ulong))
			{
				text = Convert.ToUInt64(value).ToString();
			}
			else
			{
				text = Convert.ToInt64(value).ToString();
			}
			return text;
		}
	}
}
