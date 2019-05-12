using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace System
{
	/// <summary>Converts a base data type to another base data type.</summary>
	/// <filterpriority>1</filterpriority>
	public static class Convert
	{
		private const int MaxBytesPerLine = 57;

		/// <summary>A constant that represents a database column that is absent of data; that is, database null.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly object DBNull = System.DBNull.Value;

		private static readonly Type[] conversionTable = new Type[]
		{
			null,
			typeof(object),
			typeof(DBNull),
			typeof(bool),
			typeof(char),
			typeof(sbyte),
			typeof(byte),
			typeof(short),
			typeof(ushort),
			typeof(int),
			typeof(uint),
			typeof(long),
			typeof(ulong),
			typeof(float),
			typeof(double),
			typeof(decimal),
			typeof(DateTime),
			null,
			typeof(string)
		};

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte[] InternalFromBase64String(string str, bool allowWhitespaceOnly);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte[] InternalFromBase64CharArray(char[] arr, int offset, int length);

		/// <summary>Converts a subset of a Unicode character array, which encodes binary data as base-64 digits, to an equivalent 8-bit unsigned integer array. Parameters specify the subset in the input array and the number of elements to convert.</summary>
		/// <returns>An array of 8-bit unsigned integers equivalent to <paramref name="length" /> elements at position <paramref name="offset" /> in <paramref name="inArray" />.</returns>
		/// <param name="inArray">A Unicode character array. </param>
		/// <param name="offset">A position within <paramref name="inArray" />. </param>
		/// <param name="length">The number of elements in <paramref name="inArray" /> to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="inArray" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="length" /> is less than 0.-or- <paramref name="offset" /> plus <paramref name="length" /> indicates a position not within <paramref name="inArray" />. </exception>
		/// <exception cref="T:System.FormatException">The length of <paramref name="inArray" />, ignoring white-space characters, is not zero or a multiple of 4. -or-The format of <paramref name="inArray" /> is invalid. <paramref name="inArray" /> contains a non-base-64 character, more than two padding characters, or a non-white-space character among the padding characters. </exception>
		/// <filterpriority>1</filterpriority>
		public static byte[] FromBase64CharArray(char[] inArray, int offset, int length)
		{
			if (inArray == null)
			{
				throw new ArgumentNullException("inArray");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset < 0");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length < 0");
			}
			if (offset > inArray.Length - length)
			{
				throw new ArgumentOutOfRangeException("offset + length > array.Length");
			}
			return Convert.InternalFromBase64CharArray(inArray, offset, length);
		}

		/// <summary>Converts the specified string, which encodes binary data as base-64 digits, to an equivalent 8-bit unsigned integer array.</summary>
		/// <returns>An array of 8-bit unsigned integers that is equivalent to <paramref name="s" />.</returns>
		/// <param name="s">The string to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">The length of <paramref name="s" />, ignoring white-space characters, is not zero or a multiple of 4. -or-The format of <paramref name="s" /> is invalid. <paramref name="s" /> contains a non-base-64 character, more than two padding characters, or a non-white space-character among the padding characters.</exception>
		/// <filterpriority>1</filterpriority>
		public static byte[] FromBase64String(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length == 0)
			{
				return new byte[0];
			}
			return Convert.InternalFromBase64String(s, true);
		}

		/// <summary>Returns the <see cref="T:System.TypeCode" /> for the specified object.</summary>
		/// <returns>The <see cref="T:System.TypeCode" /> for <paramref name="value" />, or <see cref="F:System.TypeCode.Empty" /> if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <filterpriority>1</filterpriority>
		public static TypeCode GetTypeCode(object value)
		{
			if (value == null)
			{
				return TypeCode.Empty;
			}
			return Type.GetTypeCode(value.GetType());
		}

		/// <summary>Returns an indication whether the specified object is of type <see cref="T:System.DBNull" />.</summary>
		/// <returns>true if <paramref name="value" /> is of type <see cref="T:System.DBNull" />; otherwise, false.</returns>
		/// <param name="value">An object. </param>
		/// <filterpriority>1</filterpriority>
		public static bool IsDBNull(object value)
		{
			return value is DBNull;
		}

		/// <summary>Converts a subset of an 8-bit unsigned integer array to an equivalent subset of a Unicode character array encoded with base-64 digits. Parameters specify the subsets as offsets in the input and output arrays, and the number of elements in the input array to convert.</summary>
		/// <returns>A 32-bit signed integer containing the number of bytes in <paramref name="outArray" />.</returns>
		/// <param name="inArray">An input array of 8-bit unsigned integers. </param>
		/// <param name="offsetIn">A position within <paramref name="inArray" />. </param>
		/// <param name="length">The number of elements of <paramref name="inArray" /> to convert. </param>
		/// <param name="outArray">An output array of Unicode characters. </param>
		/// <param name="offsetOut">A position within <paramref name="outArray" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="inArray" /> or <paramref name="outArray" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offsetIn" />, <paramref name="offsetOut" />, or <paramref name="length" /> is negative.-or- <paramref name="offsetIn" /> plus <paramref name="length" /> is greater than the length of <paramref name="inArray" />.-or- <paramref name="offsetOut" /> plus the number of elements to return is greater than the length of <paramref name="outArray" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static int ToBase64CharArray(byte[] inArray, int offsetIn, int length, char[] outArray, int offsetOut)
		{
			if (inArray == null)
			{
				throw new ArgumentNullException("inArray");
			}
			if (outArray == null)
			{
				throw new ArgumentNullException("outArray");
			}
			if (offsetIn < 0 || length < 0 || offsetOut < 0)
			{
				throw new ArgumentOutOfRangeException("offsetIn, length, offsetOut < 0");
			}
			if (offsetIn > inArray.Length - length)
			{
				throw new ArgumentOutOfRangeException("offsetIn + length > array.Length");
			}
			byte[] bytes = ToBase64Transform.InternalTransformFinalBlock(inArray, offsetIn, length);
			char[] chars = new ASCIIEncoding().GetChars(bytes);
			if (offsetOut > outArray.Length - chars.Length)
			{
				throw new ArgumentOutOfRangeException("offsetOut + cOutArr.Length > outArray.Length");
			}
			Array.Copy(chars, 0, outArray, offsetOut, chars.Length);
			return chars.Length;
		}

		/// <summary>Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with base-64 digits.</summary>
		/// <returns>The string representation, in base 64, of the contents of <paramref name="inArray" />.</returns>
		/// <param name="inArray">An array of 8-bit unsigned integers. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="inArray" /> is null. </exception>
		/// <filterpriority>1</filterpriority>
		public static string ToBase64String(byte[] inArray)
		{
			if (inArray == null)
			{
				throw new ArgumentNullException("inArray");
			}
			return Convert.ToBase64String(inArray, 0, inArray.Length);
		}

		/// <summary>Converts a subset of an array of 8-bit unsigned integers to its equivalent string representation that is encoded with base-64 digits. Parameters specify the subset as an offset in the input array, and the number of elements in the array to convert.</summary>
		/// <returns>The string representation in base 64 of <paramref name="length" /> elements of <paramref name="inArray" />, starting at position <paramref name="offset" />.</returns>
		/// <param name="inArray">An array of 8-bit unsigned integers. </param>
		/// <param name="offset">An offset in <paramref name="inArray" />. </param>
		/// <param name="length">The number of elements of <paramref name="inArray" /> to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="inArray" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="length" /> is negative.-or- <paramref name="offset" /> plus <paramref name="length" /> is greater than the length of <paramref name="inArray" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static string ToBase64String(byte[] inArray, int offset, int length)
		{
			if (inArray == null)
			{
				throw new ArgumentNullException("inArray");
			}
			if (offset < 0 || length < 0)
			{
				throw new ArgumentOutOfRangeException("offset < 0 || length < 0");
			}
			if (offset > inArray.Length - length)
			{
				throw new ArgumentOutOfRangeException("offset + length > array.Length");
			}
			byte[] bytes = ToBase64Transform.InternalTransformFinalBlock(inArray, offset, length);
			return new ASCIIEncoding().GetString(bytes);
		}

		/// <summary>Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with base-64 digits. A parameter specifies whether to insert line breaks in the return value.</summary>
		/// <returns>The string representation in base 64 of the elements in <paramref name="inArray" />.</returns>
		/// <param name="inArray">An array of 8-bit unsigned integers. </param>
		/// <param name="options">
		///   <see cref="F:System.Base64FormattingOptions.InsertLineBreaks" /> to insert a line break every 76 characters, or <see cref="F:System.Base64FormattingOptions.None" /> to not insert line breaks.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="inArray" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="options" /> is not a valid <see cref="T:System.Base64FormattingOptions" /> value. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		public static string ToBase64String(byte[] inArray, Base64FormattingOptions options)
		{
			if (inArray == null)
			{
				throw new ArgumentNullException("inArray");
			}
			return Convert.ToBase64String(inArray, 0, inArray.Length, options);
		}

		/// <summary>Converts a subset of an array of 8-bit unsigned integers to its equivalent string representation that is encoded with base-64 digits. Parameters specify the subset as an offset in the input array, the number of elements in the array to convert, and whether to insert line breaks in the return value.</summary>
		/// <returns>The string representation in base 64 of <paramref name="length" /> elements of <paramref name="inArray" />, starting at position <paramref name="offset" />.</returns>
		/// <param name="inArray">An array of 8-bit unsigned integers. </param>
		/// <param name="offset">An offset in <paramref name="inArray" />. </param>
		/// <param name="length">The number of elements of <paramref name="inArray" /> to convert. </param>
		/// <param name="options">
		///   <see cref="F:System.Base64FormattingOptions.InsertLineBreaks" /> to insert a line break every 76 characters, or <see cref="F:System.Base64FormattingOptions.None" /> to not insert line breaks.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="inArray" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="length" /> is negative.-or- <paramref name="offset" /> plus <paramref name="length" /> is greater than the length of <paramref name="inArray" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="options" /> is not a valid <see cref="T:System.Base64FormattingOptions" /> value. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		public static string ToBase64String(byte[] inArray, int offset, int length, Base64FormattingOptions options)
		{
			if (inArray == null)
			{
				throw new ArgumentNullException("inArray");
			}
			if (offset < 0 || length < 0)
			{
				throw new ArgumentOutOfRangeException("offset < 0 || length < 0");
			}
			if (offset > inArray.Length - length)
			{
				throw new ArgumentOutOfRangeException("offset + length > array.Length");
			}
			if (length == 0)
			{
				return string.Empty;
			}
			if (options == Base64FormattingOptions.InsertLineBreaks)
			{
				return Convert.ToBase64StringBuilderWithLine(inArray, offset, length).ToString();
			}
			return Encoding.ASCII.GetString(ToBase64Transform.InternalTransformFinalBlock(inArray, offset, length));
		}

		/// <summary>Converts a subset of an 8-bit unsigned integer array to an equivalent subset of a Unicode character array encoded with base-64 digits. Parameters specify the subsets as offsets in the input and output arrays, the number of elements in the input array to convert, and whether line breaks are inserted in the output array.</summary>
		/// <returns>A 32-bit signed integer containing the number of bytes in <paramref name="outArray" />.</returns>
		/// <param name="inArray">An input array of 8-bit unsigned integers. </param>
		/// <param name="offsetIn">A position within <paramref name="inArray" />. </param>
		/// <param name="length">The number of elements of <paramref name="inArray" /> to convert. </param>
		/// <param name="outArray">An output array of Unicode characters. </param>
		/// <param name="offsetOut">A position within <paramref name="outArray" />. </param>
		/// <param name="options">
		///   <see cref="F:System.Base64FormattingOptions.InsertLineBreaks" /> to insert a line break every 76 characters, or <see cref="F:System.Base64FormattingOptions.None" /> to not insert line breaks.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="inArray" /> or <paramref name="outArray" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offsetIn" />, <paramref name="offsetOut" />, or <paramref name="length" /> is negative.-or- <paramref name="offsetIn" /> plus <paramref name="length" /> is greater than the length of <paramref name="inArray" />.-or- <paramref name="offsetOut" /> plus the number of elements to return is greater than the length of <paramref name="outArray" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="options" /> is not a valid <see cref="T:System.Base64FormattingOptions" /> value. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		public static int ToBase64CharArray(byte[] inArray, int offsetIn, int length, char[] outArray, int offsetOut, Base64FormattingOptions options)
		{
			if (inArray == null)
			{
				throw new ArgumentNullException("inArray");
			}
			if (outArray == null)
			{
				throw new ArgumentNullException("outArray");
			}
			if (offsetIn < 0 || length < 0 || offsetOut < 0)
			{
				throw new ArgumentOutOfRangeException("offsetIn, length, offsetOut < 0");
			}
			if (offsetIn > inArray.Length - length)
			{
				throw new ArgumentOutOfRangeException("offsetIn + length > array.Length");
			}
			if (length == 0)
			{
				return 0;
			}
			if (options == Base64FormattingOptions.InsertLineBreaks)
			{
				StringBuilder stringBuilder = Convert.ToBase64StringBuilderWithLine(inArray, offsetIn, length);
				stringBuilder.CopyTo(0, outArray, offsetOut, stringBuilder.Length);
				return stringBuilder.Length;
			}
			byte[] bytes = ToBase64Transform.InternalTransformFinalBlock(inArray, offsetIn, length);
			char[] chars = Encoding.ASCII.GetChars(bytes);
			if (offsetOut > outArray.Length - chars.Length)
			{
				throw new ArgumentOutOfRangeException("offsetOut + cOutArr.Length > outArray.Length");
			}
			Array.Copy(chars, 0, outArray, offsetOut, chars.Length);
			return chars.Length;
		}

		private static StringBuilder ToBase64StringBuilderWithLine(byte[] inArray, int offset, int length)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num2;
			int num = Math.DivRem(length, 57, out num2);
			for (int i = 0; i < num; i++)
			{
				byte[] bytes = ToBase64Transform.InternalTransformFinalBlock(inArray, offset, 57);
				stringBuilder.AppendLine(Encoding.ASCII.GetString(bytes));
				offset += 57;
			}
			if (num2 == 0)
			{
				int length2 = Environment.NewLine.Length;
				stringBuilder.Remove(stringBuilder.Length - length2, length2);
			}
			else
			{
				byte[] bytes2 = ToBase64Transform.InternalTransformFinalBlock(inArray, offset, num2);
				stringBuilder.Append(Encoding.ASCII.GetString(bytes2));
			}
			return stringBuilder;
		}

		/// <summary>Returns the specified Boolean value; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">The Boolean value to return. </param>
		/// <filterpriority>1</filterpriority>
		public static bool ToBoolean(bool value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 8-bit unsigned integer to an equivalent Boolean value.</summary>
		/// <returns>true if <paramref name="value" /> is not zero; otherwise, false.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static bool ToBoolean(byte value)
		{
			return value != 0;
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The Unicode character to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool ToBoolean(char value)
		{
			throw new InvalidCastException(Locale.GetText("Can't convert char to bool"));
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The date and time value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool ToBoolean(DateTime value)
		{
			throw new InvalidCastException(Locale.GetText("Can't convert date to bool"));
		}

		/// <summary>Converts the value of the specified decimal number to an equivalent Boolean value.</summary>
		/// <returns>true if <paramref name="value" /> is not zero; otherwise, false.</returns>
		/// <param name="value">The number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static bool ToBoolean(decimal value)
		{
			return value != 0m;
		}

		/// <summary>Converts the value of the specified double-precision floating-point number to an equivalent Boolean value.</summary>
		/// <returns>true if <paramref name="value" /> is not zero; otherwise, false.</returns>
		/// <param name="value">The double-precision floating-point number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static bool ToBoolean(double value)
		{
			return value != 0.0;
		}

		/// <summary>Converts the value of the specified single-precision floating-point number to an equivalent Boolean value.</summary>
		/// <returns>true if <paramref name="value" /> is not zero; otherwise, false.</returns>
		/// <param name="value">The single-precision floating-point number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static bool ToBoolean(float value)
		{
			return value != 0f;
		}

		/// <summary>Converts the value of the specified 32-bit signed integer to an equivalent Boolean value.</summary>
		/// <returns>true if <paramref name="value" /> is not zero; otherwise, false.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static bool ToBoolean(int value)
		{
			return value != 0;
		}

		/// <summary>Converts the value of the specified 64-bit signed integer to an equivalent Boolean value.</summary>
		/// <returns>true if <paramref name="value" /> is not zero; otherwise, false.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static bool ToBoolean(long value)
		{
			return value != 0L;
		}

		/// <summary>Converts the value of the specified 8-bit signed integer to an equivalent Boolean value.</summary>
		/// <returns>true if <paramref name="value" /> is not zero; otherwise, false.</returns>
		/// <param name="value">The 8-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static bool ToBoolean(sbyte value)
		{
			return (int)value != 0;
		}

		/// <summary>Converts the value of the specified 16-bit signed integer to an equivalent Boolean value.</summary>
		/// <returns>true if <paramref name="value" /> is not zero; otherwise, false.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static bool ToBoolean(short value)
		{
			return value != 0;
		}

		/// <summary>Converts the specified string representation of a logical value to its Boolean equivalent.</summary>
		/// <returns>true if <paramref name="value" /> equals <see cref="F:System.Boolean.TrueString" />, or false if <paramref name="value" /> equals <see cref="F:System.Boolean.FalseString" /> or null.</returns>
		/// <param name="value">A string that contains the value of either <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" />. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not equal to <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool ToBoolean(string value)
		{
			return value != null && bool.Parse(value);
		}

		/// <summary>Converts the specified string representation of a logical value to its Boolean equivalent, using the specified culture-specific formatting information.</summary>
		/// <returns>true if <paramref name="value" /> equals <see cref="F:System.Boolean.TrueString" />, or false if <paramref name="value" /> equals <see cref="F:System.Boolean.FalseString" /> or null.</returns>
		/// <param name="value">A string that contains the value of either <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" />. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. This parameter is ignored.</param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not equal to <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool ToBoolean(string value, IFormatProvider provider)
		{
			return value != null && bool.Parse(value);
		}

		/// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent Boolean value.</summary>
		/// <returns>true if <paramref name="value" /> is not zero; otherwise, false.</returns>
		/// <param name="value">The 32-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static bool ToBoolean(uint value)
		{
			return value != 0u;
		}

		/// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent Boolean value.</summary>
		/// <returns>true if <paramref name="value" /> is not zero; otherwise, false.</returns>
		/// <param name="value">The 64-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static bool ToBoolean(ulong value)
		{
			return value != 0UL;
		}

		/// <summary>Converts the value of the specified 16-bit unsigned integer to an equivalent Boolean value.</summary>
		/// <returns>true if <paramref name="value" /> is not zero; otherwise, false.</returns>
		/// <param name="value">The 16-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static bool ToBoolean(ushort value)
		{
			return value != 0;
		}

		/// <summary>Converts the value of a specified object to an equivalent Boolean value.</summary>
		/// <returns>true or false, which reflects the value returned by invoking the <see cref="M:System.IConvertible.ToBoolean(System.IFormatProvider)" /> method for the underlying type of <paramref name="value" />. If <paramref name="value" /> is null, the method returns false. </returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or null. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is a string that does not equal <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" />.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.-or-The conversion of <paramref name="value" /> to a <see cref="T:System.Boolean" /> is not supported.</exception>
		/// <filterpriority>1</filterpriority>
		public static bool ToBoolean(object value)
		{
			return value != null && Convert.ToBoolean(value, null);
		}

		/// <summary>Converts the value of the specified object to an equivalent Boolean value, using the specified culture-specific formatting information.</summary>
		/// <returns>true or false, which reflects the value returned by invoking the <see cref="M:System.IConvertible.ToBoolean(System.IFormatProvider)" /> method for the underlying type of <paramref name="value" />. If <paramref name="value" /> is null, the method returns false.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or null. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is a string that does not equal <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" />.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.-or-The conversion of <paramref name="value" /> to a <see cref="T:System.Boolean" /> is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool ToBoolean(object value, IFormatProvider provider)
		{
			return value != null && ((IConvertible)value).ToBoolean(provider);
		}

		/// <summary>Converts the specified Boolean value to the equivalent 8-bit unsigned integer.</summary>
		/// <returns>The number 1 if <paramref name="value" /> is true; otherwise, 0.</returns>
		/// <param name="value">The Boolean value to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(bool value)
		{
			return (!value) ? 0 : 1;
		}

		/// <summary>Returns the specified 8-bit unsigned integer; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">The 8-bit unsigned integer to return. </param>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(byte value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified Unicode character to the equivalent 8-bit unsigned integer.</summary>
		/// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The Unicode character to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is greater than <see cref="F:System.Byte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(char value)
		{
			if (value > 'ÿ')
			{
				throw new OverflowException(Locale.GetText("Value is greater than Byte.MaxValue"));
			}
			return (byte)value;
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The date and time value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(DateTime value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Converts the value of the specified decimal number to an equivalent 8-bit unsigned integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 8-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Byte.MaxValue" /> or less than <see cref="F:System.Byte.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(decimal value)
		{
			if (value > 255m || value < 0m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Byte.MaxValue or less than Byte.MinValue"));
			}
			return (byte)Math.Round(value);
		}

		/// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 8-bit unsigned integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 8-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The double-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Byte.MaxValue" /> or less than <see cref="F:System.Byte.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(double value)
		{
			if (value > 255.0 || value < 0.0)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Byte.MaxValue or less than Byte.MinValue"));
			}
			if (double.IsNaN(value) || double.IsInfinity(value))
			{
				throw new OverflowException(Locale.GetText("Value is equal to Double.NaN, Double.PositiveInfinity, or Double.NegativeInfinity"));
			}
			return (byte)Math.Round(value);
		}

		/// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 8-bit unsigned integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 8-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">A single-precision floating-point number. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Byte.MaxValue" /> or less than <see cref="F:System.Byte.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(float value)
		{
			if (value > 255f || value < 0f)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Byte.MaxValue or less than Byte.Minalue"));
			}
			if (float.IsNaN(value) || float.IsInfinity(value))
			{
				throw new OverflowException(Locale.GetText("Value is equal to Single.NaN, Single.PositiveInfinity, or Single.NegativeInfinity"));
			}
			return (byte)Math.Round((double)value);
		}

		/// <summary>Converts the value of the specified 32-bit signed integer to an equivalent 8-bit unsigned integer.</summary>
		/// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(int value)
		{
			if (value > 255 || value < 0)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Byte.MaxValue or less than Byte.MinValue"));
			}
			return (byte)value;
		}

		/// <summary>Converts the value of the specified 64-bit signed integer to an equivalent 8-bit unsigned integer.</summary>
		/// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(long value)
		{
			if (value > 255L || value < 0L)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Byte.MaxValue or less than Byte.MinValue"));
			}
			return (byte)value;
		}

		/// <summary>Converts the value of the specified 8-bit signed integer to an equivalent 8-bit unsigned integer.</summary>
		/// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit signed integer to be converted. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Byte.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static byte ToByte(sbyte value)
		{
			if ((int)value < 0)
			{
				throw new OverflowException(Locale.GetText("Value is less than Byte.MinValue"));
			}
			return (byte)value;
		}

		/// <summary>Converts the value of the specified 16-bit signed integer to an equivalent 8-bit unsigned integer.</summary>
		/// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(short value)
		{
			if (value > 255 || value < 0)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Byte.MaxValue or less than Byte.MinValue"));
			}
			return (byte)value;
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 8-bit unsigned integer.</summary>
		/// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return byte.Parse(value);
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 8-bit unsigned integer, using specified culture-specific formatting information.</summary>
		/// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return byte.Parse(value, provider);
		}

		/// <summary>Converts the string representation of a number in a specified base to an equivalent 8-bit unsigned integer.</summary>
		/// <returns>An 8-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="fromBase" /> is not 2, 8, 10, or 16. -or-<paramref name="value" />, which represents a non-base 10 unsigned number, is prefixed with a negative sign. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" />, which represents a base 10 unsigned number, is prefixed with a negative sign.-or-<paramref name="value" /> represents a number that is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(string value, int fromBase)
		{
			int num = Convert.ConvertFromBase(value, fromBase, true);
			if (num < 0 || num > 255)
			{
				throw new OverflowException();
			}
			return (byte)num;
		}

		/// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent 8-bit unsigned integer.</summary>
		/// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Byte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static byte ToByte(uint value)
		{
			if (value > 255u)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Byte.MaxValue"));
			}
			return (byte)value;
		}

		/// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent 8-bit unsigned integer.</summary>
		/// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Byte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static byte ToByte(ulong value)
		{
			if (value > 255UL)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Byte.MaxValue"));
			}
			return (byte)value;
		}

		/// <summary>Converts the value of the specified 16-bit unsigned integer to an equivalent 8-bit unsigned integer.</summary>
		/// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Byte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static byte ToByte(ushort value)
		{
			if (value > 255)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Byte.MaxValue"));
			}
			return (byte)value;
		}

		/// <summary>Converts the value of the specified object to an 8-bit unsigned integer.</summary>
		/// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or null. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in the property format for a <see cref="T:System.Byte" /> value.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement <see cref="T:System.IConvertible" />. -or-Conversion from <paramref name="value" /> to the <see cref="T:System.Byte" /> type is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(object value)
		{
			if (value == null)
			{
				return 0;
			}
			return Convert.ToByte(value, null);
		}

		/// <summary>Converts the value of the specified object to an 8-bit unsigned integer, using the specified culture-specific formatting information.</summary>
		/// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in the property format for a <see cref="T:System.Byte" /> value.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement <see cref="T:System.IConvertible" />. -or-Conversion from <paramref name="value" /> to the <see cref="T:System.Byte" /> type is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static byte ToByte(object value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return ((IConvertible)value).ToByte(provider);
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The Boolean value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static char ToChar(bool value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Converts the value of the specified 8-bit unsigned integer to its equivalent Unicode character.</summary>
		/// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static char ToChar(byte value)
		{
			return (char)value;
		}

		/// <summary>Returns the specified Unicode character value; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">The Unicode character to return. </param>
		/// <filterpriority>1</filterpriority>
		public static char ToChar(char value)
		{
			return value;
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The date and time value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static char ToChar(DateTime value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The decimal number to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static char ToChar(decimal value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The double-precision floating-point number to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static char ToChar(double value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Converts the value of the specified 32-bit signed integer to its equivalent Unicode character.</summary>
		/// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Char.MinValue" /> or greater than <see cref="F:System.Char.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static char ToChar(int value)
		{
			if (value > 65535 || value < 0)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Char.MaxValue or less than Char.MinValue"));
			}
			return (char)value;
		}

		/// <summary>Converts the value of the specified 64-bit signed integer to its equivalent Unicode character.</summary>
		/// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Char.MinValue" /> or greater than <see cref="F:System.Char.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static char ToChar(long value)
		{
			if (value > 65535L || value < 0L)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Char.MaxValue or less than Char.MinValue"));
			}
			return (char)value;
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The single-precision floating-point number to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static char ToChar(float value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Converts the value of the specified 8-bit signed integer to its equivalent Unicode character.</summary>
		/// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Char.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static char ToChar(sbyte value)
		{
			if ((int)value < 0)
			{
				throw new OverflowException(Locale.GetText("Value is less than Char.MinValue"));
			}
			return (char)value;
		}

		/// <summary>Converts the value of the specified 16-bit signed integer to its equivalent Unicode character.</summary>
		/// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Char.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static char ToChar(short value)
		{
			if (value < 0)
			{
				throw new OverflowException(Locale.GetText("Value is less than Char.MinValue"));
			}
			return (char)value;
		}

		/// <summary>Converts the first character of a specified string to a Unicode character.</summary>
		/// <returns>A Unicode character that is equivalent to the first and only character in <paramref name="value" />.</returns>
		/// <param name="value">A string of length 1. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.FormatException">The length of <paramref name="value" /> is not 1. </exception>
		/// <filterpriority>1</filterpriority>
		public static char ToChar(string value)
		{
			return char.Parse(value);
		}

		/// <summary>Converts the first character of a specified string to a Unicode character, using specified culture-specific formatting information.</summary>
		/// <returns>A Unicode character that is equivalent to the first and only character in <paramref name="value" />.</returns>
		/// <param name="value">A string of length 1 or null. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. This parameter is ignored.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.FormatException">The length of <paramref name="value" /> is not 1. </exception>
		/// <filterpriority>1</filterpriority>
		public static char ToChar(string value, IFormatProvider provider)
		{
			return char.Parse(value);
		}

		/// <summary>Converts the value of the specified 32-bit unsigned integer to its equivalent Unicode character.</summary>
		/// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Char.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static char ToChar(uint value)
		{
			if (value > 65535u)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Char.MaxValue"));
			}
			return (char)value;
		}

		/// <summary>Converts the value of the specified 64-bit unsigned integer to its equivalent Unicode character.</summary>
		/// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Char.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static char ToChar(ulong value)
		{
			if (value > 65535UL)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Char.MaxValue"));
			}
			return (char)value;
		}

		/// <summary>Converts the value of the specified 16-bit unsigned integer to its equivalent Unicode character.</summary>
		/// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static char ToChar(ushort value)
		{
			return (char)value;
		}

		/// <summary>Converts the value of the specified object to a Unicode character.</summary>
		/// <returns>A Unicode character that is equivalent to value, or <see cref="F:System.Char.MinValue" /> if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is a null string.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.-or-The conversion of <paramref name="value" /> to a <see cref="T:System.Char" /> is not supported. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Char.MinValue" /> or greater than <see cref="F:System.Char.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static char ToChar(object value)
		{
			if (value == null)
			{
				return '\0';
			}
			return Convert.ToChar(value, null);
		}

		/// <summary>Converts the value of the specified object to its equivalent Unicode character, using the specified culture-specific formatting information.</summary>
		/// <returns>A Unicode character that is equivalent to <paramref name="value" />, or <see cref="F:System.Char.MinValue" /> if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is a null string.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface. -or-The conversion of <paramref name="value" /> to a <see cref="T:System.Char" /> is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than <see cref="F:System.Char.MinValue" /> or greater than <see cref="F:System.Char.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static char ToChar(object value, IFormatProvider provider)
		{
			if (value == null)
			{
				return '\0';
			}
			return ((IConvertible)value).ToChar(provider);
		}

		/// <summary>Converts the specified string representation of a date and time to an equivalent date and time value.</summary>
		/// <returns>The date and time equivalent of the value of <paramref name="value" />, or the date and time equivalent of <see cref="F:System.DateTime.MinValue" /> if <paramref name="value" /> is null.</returns>
		/// <param name="value">The string representation of a date and time.</param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not a properly formatted date and time string. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime ToDateTime(string value)
		{
			if (value == null)
			{
				return DateTime.MinValue;
			}
			return DateTime.Parse(value);
		}

		/// <summary>Converts the specified string representation of a number to an equivalent date and time, using the specified culture-specific formatting information.</summary>
		/// <returns>The date and time equivalent of the value of <paramref name="value" />, or the date and time equivalent of <see cref="F:System.DateTime.MinValue" /> if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains a date and time to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not a properly formatted date and time string. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime ToDateTime(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return DateTime.MinValue;
			}
			return DateTime.Parse(value, provider);
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The Boolean value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime ToDateTime(bool value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime ToDateTime(byte value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The Unicode character to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime ToDateTime(char value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Returns the specified <see cref="T:System.DateTime" /> object; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">A date and time value. </param>
		/// <filterpriority>1</filterpriority>
		public static DateTime ToDateTime(DateTime value)
		{
			return value;
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The number to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime ToDateTime(decimal value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The double-precision floating-point value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime ToDateTime(double value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime ToDateTime(short value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime ToDateTime(int value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime ToDateTime(long value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The single-precision floating-point value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime ToDateTime(float value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Converts the value of the specified object to a <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>The date and time equivalent of the value of <paramref name="value" />, or a date and time equivalent of <see cref="F:System.DateTime.MinValue" /> if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or null. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not a valid date and time value.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime ToDateTime(object value)
		{
			if (value == null)
			{
				return DateTime.MinValue;
			}
			return Convert.ToDateTime(value, null);
		}

		/// <summary>Converts the value of the specified object to a <see cref="T:System.DateTime" /> object, using the specified culture-specific formatting information.</summary>
		/// <returns>The date and time equivalent of the value of <paramref name="value" />, or the date and time equivalent of <see cref="F:System.DateTime.MinValue" /> if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not a valid date and time value.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime ToDateTime(object value, IFormatProvider provider)
		{
			if (value == null)
			{
				return DateTime.MinValue;
			}
			return ((IConvertible)value).ToDateTime(provider);
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The 8-bit signed integer to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static DateTime ToDateTime(sbyte value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The 16-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static DateTime ToDateTime(ushort value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The 32-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static DateTime ToDateTime(uint value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The 64-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static DateTime ToDateTime(ulong value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Converts the specified Boolean value to the equivalent decimal number.</summary>
		/// <returns>The number 1 if <paramref name="value" /> is true; otherwise, 0.</returns>
		/// <param name="value">The Boolean value to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static decimal ToDecimal(bool value)
		{
			return (!value) ? 0 : 1;
		}

		/// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent decimal number.</summary>
		/// <returns>The decimal number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static decimal ToDecimal(byte value)
		{
			return value;
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The Unicode character to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static decimal ToDecimal(char value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The date and time value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static decimal ToDecimal(DateTime value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Returns the specified decimal number; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">A decimal number. </param>
		/// <filterpriority>1</filterpriority>
		public static decimal ToDecimal(decimal value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified double-precision floating-point number to an equivalent decimal number.</summary>
		/// <returns>A decimal number that is equivalent to <paramref name="value" />. </returns>
		/// <param name="value">The double-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Decimal.MaxValue" /> or less than <see cref="F:System.Decimal.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static decimal ToDecimal(double value)
		{
			return (decimal)value;
		}

		/// <summary>Converts the value of the specified single-precision floating-point number to the equivalent decimal number.</summary>
		/// <returns>A decimal number that is equivalent to <paramref name="value" />. </returns>
		/// <param name="value">The single-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Decimal.MaxValue" /> or less than <see cref="F:System.Decimal.MinValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static decimal ToDecimal(float value)
		{
			return (decimal)value;
		}

		/// <summary>Converts the value of the specified 32-bit signed integer to an equivalent decimal number.</summary>
		/// <returns>A decimal number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static decimal ToDecimal(int value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 64-bit signed integer to an equivalent decimal number.</summary>
		/// <returns>A decimal number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static decimal ToDecimal(long value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 8-bit signed integer to the equivalent decimal number.</summary>
		/// <returns>A decimal number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static decimal ToDecimal(sbyte value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 16-bit signed integer to an equivalent decimal number.</summary>
		/// <returns>A decimal number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static decimal ToDecimal(short value)
		{
			return value;
		}

		/// <summary>Converts the specified string representation of a number to an equivalent decimal number.</summary>
		/// <returns>A decimal number that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains a number to convert. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not a number in a valid format.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static decimal ToDecimal(string value)
		{
			if (value == null)
			{
				return 0m;
			}
			return decimal.Parse(value);
		}

		/// <summary>Converts the specified string representation of a number to an equivalent decimal number, using the specified culture-specific formatting information.</summary>
		/// <returns>A decimal number that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains a number to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not a number in a valid format.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static decimal ToDecimal(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0m;
			}
			return decimal.Parse(value, provider);
		}

		/// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent decimal number.</summary>
		/// <returns>A decimal number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static decimal ToDecimal(uint value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent decimal number.</summary>
		/// <returns>A decimal number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static decimal ToDecimal(ulong value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 16-bit unsigned integer to an equivalent decimal number.</summary>
		/// <returns>The decimal number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static decimal ToDecimal(ushort value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified object to an equivalent decimal number.</summary>
		/// <returns>A decimal number that is equivalent to <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or null. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format for a <see cref="T:System.Decimal" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static decimal ToDecimal(object value)
		{
			if (value == null)
			{
				return 0m;
			}
			return Convert.ToDecimal(value, null);
		}

		/// <summary>Converts the value of the specified object to an equivalent decimal number, using the specified culture-specific formatting information.</summary>
		/// <returns>A decimal number that is equivalent to <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format for a <see cref="T:System.Decimal" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.-or-The conversion is not supported. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static decimal ToDecimal(object value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0m;
			}
			return ((IConvertible)value).ToDecimal(provider);
		}

		/// <summary>Converts the specified Boolean value to the equivalent double-precision floating-point number.</summary>
		/// <returns>The number 1 if <paramref name="value" /> is true; otherwise, 0.</returns>
		/// <param name="value">The Boolean value to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static double ToDouble(bool value)
		{
			return (double)((!value) ? 0 : 1);
		}

		/// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent double-precision floating-point number.</summary>
		/// <returns>The double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static double ToDouble(byte value)
		{
			return (double)value;
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The Unicode character to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static double ToDouble(char value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The date and time value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static double ToDouble(DateTime value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Converts the value of the specified decimal number to an equivalent double-precision floating-point number.</summary>
		/// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The decimal number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static double ToDouble(decimal value)
		{
			return (double)value;
		}

		/// <summary>Returns the specified double-precision floating-point number; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">The double-precision floating-point number to return. </param>
		/// <filterpriority>1</filterpriority>
		public static double ToDouble(double value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified single-precision floating-point number to an equivalent double-precision floating-point number.</summary>
		/// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The single-precision floating-point number. </param>
		/// <filterpriority>1</filterpriority>
		public static double ToDouble(float value)
		{
			return (double)value;
		}

		/// <summary>Converts the value of the specified 32-bit signed integer to an equivalent double-precision floating-point number.</summary>
		/// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static double ToDouble(int value)
		{
			return (double)value;
		}

		/// <summary>Converts the value of the specified 64-bit signed integer to an equivalent double-precision floating-point number.</summary>
		/// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static double ToDouble(long value)
		{
			return (double)value;
		}

		/// <summary>Converts the value of the specified 8-bit signed integer to the equivalent double-precision floating-point number.</summary>
		/// <returns>The 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static double ToDouble(sbyte value)
		{
			return (double)value;
		}

		/// <summary>Converts the value of the specified 16-bit signed integer to an equivalent double-precision floating-point number.</summary>
		/// <returns>A double-precision floating-point number equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static double ToDouble(short value)
		{
			return (double)value;
		}

		/// <summary>Converts the specified string representation of a number to an equivalent double-precision floating-point number.</summary>
		/// <returns>A double-precision floating-point number that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null. Because of differences in precision, the return value may not be exactly equal to <paramref name="value" />, and for values of <paramref name="value" /> that are less than <see cref="F:System.Double.Epsilon" />, the return value may also differ depending on processor architecture. For more information, see the Remarks section of <see cref="T:System.Double" />.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not a number in a valid format.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static double ToDouble(string value)
		{
			if (value == null)
			{
				return 0.0;
			}
			return double.Parse(value);
		}

		/// <summary>Converts the specified string representation of a number to an equivalent double-precision floating-point number, using the specified culture-specific formatting information.</summary>
		/// <returns>A double-precision floating-point number that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null. Because of differences in precision, the return value may not be exactly equal to <paramref name="value" />, and for values of <paramref name="value" /> that are less than <see cref="F:System.Double.Epsilon" />, the return value may also differ depending on processor architecture. For more information, see the Remarks section of <see cref="T:System.Double" />.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not a number in a valid format.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static double ToDouble(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0.0;
			}
			return double.Parse(value, provider);
		}

		/// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent double-precision floating-point number.</summary>
		/// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static double ToDouble(uint value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent double-precision floating-point number.</summary>
		/// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static double ToDouble(ulong value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent double-precision floating-point number.</summary>
		/// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static double ToDouble(ushort value)
		{
			return (double)value;
		}

		/// <summary>Converts the value of the specified object to a double-precision floating-point number.</summary>
		/// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or null. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format for a <see cref="T:System.Double" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static double ToDouble(object value)
		{
			if (value == null)
			{
				return 0.0;
			}
			return Convert.ToDouble(value, null);
		}

		/// <summary>Converts the value of the specified object to an double-precision floating-point number, using the specified culture-specific formatting information.</summary>
		/// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format for a <see cref="T:System.Double" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static double ToDouble(object value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0.0;
			}
			return ((IConvertible)value).ToDouble(provider);
		}

		/// <summary>Converts the specified Boolean value to the equivalent 16-bit signed integer.</summary>
		/// <returns>The number 1 if <paramref name="value" /> is true; otherwise, 0.</returns>
		/// <param name="value">The Boolean value to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(bool value)
		{
			return (!value) ? 0 : 1;
		}

		/// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent 16-bit signed integer.</summary>
		/// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(byte value)
		{
			return (short)value;
		}

		/// <summary>Converts the value of the specified Unicode character to the equivalent 16-bit signed integer.</summary>
		/// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />. </returns>
		/// <param name="value">The Unicode character to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(char value)
		{
			if (value > '翿')
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int16.MaxValue"));
			}
			return (short)value;
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The date and time value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(DateTime value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Converts the value of the specified decimal number to an equivalent 16-bit signed integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 16-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The decimal number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" /> or less than <see cref="F:System.Int16.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(decimal value)
		{
			if (value > 32767m || value < -32768m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int16.MaxValue or less than Int16.MinValue"));
			}
			return (short)Math.Round(value);
		}

		/// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 16-bit signed integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 16-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The double-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" /> or less than <see cref="F:System.Int16.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(double value)
		{
			if (value > 32767.0 || value < -32768.0)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int16.MaxValue or less than Int16.MinValue"));
			}
			return (short)Math.Round(value);
		}

		/// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 16-bit signed integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 16-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The single-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" /> or less than <see cref="F:System.Int16.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(float value)
		{
			if (value > 32767f || value < -32768f)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int16.MaxValue or less than Int16.MinValue"));
			}
			return (short)Math.Round((double)value);
		}

		/// <summary>Converts the value of the specified 32-bit signed integer to an equivalent 16-bit signed integer.</summary>
		/// <returns>The 16-bit signed integer equivalent of <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" /> or less than <see cref="F:System.Int16.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(int value)
		{
			if (value > 32767 || value < -32768)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int16.MaxValue or less than Int16.MinValue"));
			}
			return (short)value;
		}

		/// <summary>Converts the value of the specified 64-bit signed integer to an equivalent 16-bit signed integer.</summary>
		/// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" /> or less than <see cref="F:System.Int16.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(long value)
		{
			if (value > 32767L || value < -32768L)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int16.MaxValue or less than Int16.MinValue"));
			}
			return (short)value;
		}

		/// <summary>Converts the value of the specified 8-bit signed integer to the equivalent 16-bit signed integer.</summary>
		/// <returns>A 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static short ToInt16(sbyte value)
		{
			return (short)value;
		}

		/// <summary>Returns the specified 16-bit signed integer; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">The 16-bit signed integer to return. </param>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(short value)
		{
			return value;
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 16-bit signed integer.</summary>
		/// <returns>A 16-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return short.Parse(value);
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 16-bit signed integer, using the specified culture-specific formatting information.</summary>
		/// <returns>A 16-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return short.Parse(value, provider);
		}

		/// <summary>Converts the string representation of a number in a specified base to an equivalent 16-bit signed integer.</summary>
		/// <returns>A 16-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="fromBase" /> is not 2, 8, 10, or 16. -or-<paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign.-or-<paramref name="value" /> represents a number that is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(string value, int fromBase)
		{
			int num = Convert.ConvertFromBase(value, fromBase, false);
			if (fromBase != 10)
			{
				if (num > 65535)
				{
					throw new OverflowException("Value was either too large or too small for an Int16.");
				}
				if (num > 32767)
				{
					return Convert.ToInt16(-(65536 - num));
				}
			}
			return Convert.ToInt16(num);
		}

		/// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent 16-bit signed integer.</summary>
		/// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static short ToInt16(uint value)
		{
			if ((ulong)value > 32767UL)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int16.MaxValue"));
			}
			return (short)value;
		}

		/// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent 16-bit signed integer.</summary>
		/// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static short ToInt16(ulong value)
		{
			if (value > 32767UL)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int16.MaxValue"));
			}
			return (short)value;
		}

		/// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent 16-bit signed integer.</summary>
		/// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static short ToInt16(ushort value)
		{
			if (value > 32767)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int16.MaxValue"));
			}
			return (short)value;
		}

		/// <summary>Converts the value of the specified object to a 16-bit signed integer.</summary>
		/// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or null. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format for an <see cref="T:System.Int16" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(object value)
		{
			if (value == null)
			{
				return 0;
			}
			return Convert.ToInt16(value, null);
		}

		/// <summary>Converts the value of the specified object to a 16-bit signed integer, using the specified culture-specific formatting information.</summary>
		/// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format for an <see cref="T:System.Int16" /> type.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement <see cref="T:System.IConvertible" />. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static short ToInt16(object value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return ((IConvertible)value).ToInt16(provider);
		}

		/// <summary>Converts the specified Boolean value to the equivalent 32-bit signed integer.</summary>
		/// <returns>The number 1 if <paramref name="value" /> is true; otherwise, 0.</returns>
		/// <param name="value">The Boolean value to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(bool value)
		{
			return (!value) ? 0 : 1;
		}

		/// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent 32-bit signed integer.</summary>
		/// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(byte value)
		{
			return (int)value;
		}

		/// <summary>Converts the value of the specified Unicode character to the equivalent 32-bit signed integer.</summary>
		/// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The Unicode character to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(char value)
		{
			return (int)value;
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The date and time value to convert.</param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(DateTime value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Converts the value of the specified decimal number to an equivalent 32-bit signed integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 32-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The decimal number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int32.MaxValue" /> or less than <see cref="F:System.Int32.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(decimal value)
		{
			if (value > 2147483647m || value < -2147483648m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int32.MaxValue or less than Int32.MinValue"));
			}
			return (int)Math.Round(value);
		}

		/// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 32-bit signed integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 32-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The double-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int32.MaxValue" /> or less than <see cref="F:System.Int32.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(double value)
		{
			if (value > 2147483647.0 || value < -2147483648.0)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int32.MaxValue or less than Int32.MinValue"));
			}
			return checked((int)Math.Round(value));
		}

		/// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 32-bit signed integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 32-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The single-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int32.MaxValue" /> or less than <see cref="F:System.Int32.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(float value)
		{
			if (value > 2.14748365E+09f || value < -2.14748365E+09f)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int32.MaxValue or less than Int32.MinValue"));
			}
			return checked((int)Math.Round((double)value));
		}

		/// <summary>Returns the specified 32-bit signed integer; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">The 32-bit signed integer to return. </param>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(int value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 64-bit signed integer to an equivalent 32-bit signed integer.</summary>
		/// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int32.MaxValue" /> or less than <see cref="F:System.Int32.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(long value)
		{
			if (value > 2147483647L || value < -2147483648L)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int32.MaxValue or less than Int32.MinValue"));
			}
			return (int)value;
		}

		/// <summary>Converts the value of the specified 8-bit signed integer to the equivalent 32-bit signed integer.</summary>
		/// <returns>A 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static int ToInt32(sbyte value)
		{
			return (int)value;
		}

		/// <summary>Converts the value of the specified 16-bit signed integer to an equivalent 32-bit signed integer.</summary>
		/// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(short value)
		{
			return (int)value;
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 32-bit signed integer.</summary>
		/// <returns>A 32-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return int.Parse(value);
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 32-bit signed integer, using the specified culture-specific formatting information.</summary>
		/// <returns>A 32-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return int.Parse(value, provider);
		}

		/// <summary>Converts the string representation of a number in a specified base to an equivalent 32-bit signed integer.</summary>
		/// <returns>A 32-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="fromBase" /> is not 2, 8, 10, or 16. -or-<paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign.-or-<paramref name="value" /> represents a number that is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(string value, int fromBase)
		{
			return Convert.ConvertFromBase(value, fromBase, false);
		}

		/// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent 32-bit signed integer.</summary>
		/// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static int ToInt32(uint value)
		{
			if (value > 2147483647u)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int32.MaxValue"));
			}
			return (int)value;
		}

		/// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent 32-bit signed integer.</summary>
		/// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static int ToInt32(ulong value)
		{
			if (value > 2147483647UL)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int32.MaxValue"));
			}
			return (int)value;
		}

		/// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent 32-bit signed integer.</summary>
		/// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static int ToInt32(ushort value)
		{
			return (int)value;
		}

		/// <summary>Converts the value of the specified object to a 32-bit signed integer.</summary>
		/// <returns>A 32-bit signed integer equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or null. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the  <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(object value)
		{
			if (value == null)
			{
				return 0;
			}
			return Convert.ToInt32(value, null);
		}

		/// <summary>Converts the value of the specified object to a 32-bit signed integer, using the specified culture-specific formatting information.</summary>
		/// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement <see cref="T:System.IConvertible" />. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static int ToInt32(object value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return ((IConvertible)value).ToInt32(provider);
		}

		/// <summary>Converts the specified Boolean value to the equivalent 64-bit signed integer.</summary>
		/// <returns>The number 1 if <paramref name="value" /> is true; otherwise, 0.</returns>
		/// <param name="value">The Boolean value to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(bool value)
		{
			return (!value) ? 0L : 1L;
		}

		/// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(byte value)
		{
			return (long)((ulong)value);
		}

		/// <summary>Converts the value of the specified Unicode character to the equivalent 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The Unicode character to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(char value)
		{
			return (long)value;
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The date and time value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(DateTime value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Converts the value of the specified decimal number to an equivalent 64-bit signed integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 64-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The decimal number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int64.MaxValue" /> or less than <see cref="F:System.Int64.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(decimal value)
		{
			if (value > 9223372036854775807m || value < -9223372036854775808m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int64.MaxValue or less than Int64.MinValue"));
			}
			return (long)Math.Round(value);
		}

		/// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 64-bit signed integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 64-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The double-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int64.MaxValue" /> or less than <see cref="F:System.Int64.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(double value)
		{
			if (value > 9.2233720368547758E+18 || value < -9.2233720368547758E+18)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int64.MaxValue or less than Int64.MinValue"));
			}
			return (long)Math.Round(value);
		}

		/// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 64-bit signed integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 64-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The single-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int64.MaxValue" /> or less than <see cref="F:System.Int64.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(float value)
		{
			if (value > 9.223372E+18f || value < -9.223372E+18f)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int64.MaxValue or less than Int64.MinValue"));
			}
			return (long)Math.Round((double)value);
		}

		/// <summary>Converts the value of the specified 32-bit signed integer to an equivalent 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(int value)
		{
			return (long)value;
		}

		/// <summary>Returns the specified 64-bit signed integer; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">A 64-bit signed integer. </param>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(long value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 8-bit signed integer to the equivalent 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static long ToInt64(sbyte value)
		{
			return (long)value;
		}

		/// <summary>Converts the value of the specified 16-bit signed integer to an equivalent 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(short value)
		{
			return (long)value;
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains a number to convert. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(string value)
		{
			if (value == null)
			{
				return 0L;
			}
			return long.Parse(value);
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 64-bit signed integer, using the specified culture-specific formatting information.</summary>
		/// <returns>A 64-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0L;
			}
			return long.Parse(value, provider);
		}

		/// <summary>Converts the string representation of a number in a specified base to an equivalent 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="fromBase" /> is not 2, 8, 10, or 16. -or-<paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign.-or-<paramref name="value" /> represents a number that is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(string value, int fromBase)
		{
			return Convert.ConvertFromBase64(value, fromBase, false);
		}

		/// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static long ToInt64(uint value)
		{
			return (long)((ulong)value);
		}

		/// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.Int64.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static long ToInt64(ulong value)
		{
			if (value > 9223372036854775807UL)
			{
				throw new OverflowException(Locale.GetText("Value is greater than Int64.MaxValue"));
			}
			return (long)value;
		}

		/// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static long ToInt64(ushort value)
		{
			return (long)((ulong)value);
		}

		/// <summary>Converts the value of the specified object to a 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or null. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(object value)
		{
			if (value == null)
			{
				return 0L;
			}
			return Convert.ToInt64(value, null);
		}

		/// <summary>Converts the value of the specified object to a 64-bit signed integer, using the specified culture-specific formatting information.</summary>
		/// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.-or-The conversion is not supported. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static long ToInt64(object value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0L;
			}
			return ((IConvertible)value).ToInt64(provider);
		}

		/// <summary>Converts the specified Boolean value to the equivalent 8-bit signed integer.</summary>
		/// <returns>The number 1 if <paramref name="value" /> is true; otherwise, 0.</returns>
		/// <param name="value">The Boolean value to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(bool value)
		{
			return (!value) ? 0 : 1;
		}

		/// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent 8-bit signed integer.</summary>
		/// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(byte value)
		{
			if (value > 127)
			{
				throw new OverflowException(Locale.GetText("Value is greater than SByte.MaxValue"));
			}
			return (sbyte)value;
		}

		/// <summary>Converts the value of the specified Unicode character to the equivalent 8-bit signed integer.</summary>
		/// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The Unicode character to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(char value)
		{
			if (value > '\u007f')
			{
				throw new OverflowException(Locale.GetText("Value is greater than SByte.MaxValue"));
			}
			return (sbyte)value;
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The date and time value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(DateTime value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Converts the value of the specified decimal number to an equivalent 8-bit signed integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 8-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The decimal number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(decimal value)
		{
			if (value > 127m || value < -128m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than SByte.MaxValue or less than SByte.MinValue"));
			}
			return (sbyte)Math.Round(value);
		}

		/// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 8-bit signed integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 8-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The double-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(double value)
		{
			if (value > 127.0 || value < -128.0)
			{
				throw new OverflowException(Locale.GetText("Value is greater than SByte.MaxValue or less than SByte.MinValue"));
			}
			return (sbyte)Math.Round(value);
		}

		/// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 8-bit signed integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 8-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The single-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(float value)
		{
			if (value > 127f || value < -128f)
			{
				throw new OverflowException(Locale.GetText("Value is greater than SByte.MaxValue or less than SByte.Minalue"));
			}
			return (sbyte)Math.Round((double)value);
		}

		/// <summary>Converts the value of the specified 32-bit signed integer to an equivalent 8-bit signed integer.</summary>
		/// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(int value)
		{
			if (value > 127 || value < -128)
			{
				throw new OverflowException(Locale.GetText("Value is greater than SByte.MaxValue or less than SByte.MinValue"));
			}
			return (sbyte)value;
		}

		/// <summary>Converts the value of the specified 64-bit signed integer to an equivalent 8-bit signed integer.</summary>
		/// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(long value)
		{
			if (value > 127L || value < -128L)
			{
				throw new OverflowException(Locale.GetText("Value is greater than SByte.MaxValue or less than SByte.MinValue"));
			}
			return (sbyte)value;
		}

		/// <summary>Returns the specified 8-bit signed integer; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">The 8-bit signed integer to return. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(sbyte value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 16-bit signed integer to the equivalent 8-bit signed integer.</summary>
		/// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(short value)
		{
			if (value > 127 || value < -128)
			{
				throw new OverflowException(Locale.GetText("Value is greater than SByte.MaxValue or less than SByte.MinValue"));
			}
			return (sbyte)value;
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 8-bit signed integer.</summary>
		/// <returns>An 8-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if value is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return sbyte.Parse(value);
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 8-bit signed integer, using the specified culture-specific formatting information.</summary>
		/// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return sbyte.Parse(value, provider);
		}

		/// <summary>Converts the string representation of a number in a specified base to an equivalent 8-bit signed integer.</summary>
		/// <returns>An 8-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="fromBase" /> is not 2, 8, 10, or 16. -or-<paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign.-or-<paramref name="value" /> represents a number that is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(string value, int fromBase)
		{
			int num = Convert.ConvertFromBase(value, fromBase, false);
			if (fromBase != 10 && num > 127)
			{
				return Convert.ToSByte(-(256 - num));
			}
			return Convert.ToSByte(num);
		}

		/// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent 8-bit signed integer.</summary>
		/// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(uint value)
		{
			if ((ulong)value > 127UL)
			{
				throw new OverflowException(Locale.GetText("Value is greater than SByte.MaxValue"));
			}
			return (sbyte)value;
		}

		/// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent 8-bit signed integer.</summary>
		/// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(ulong value)
		{
			if (value > 127UL)
			{
				throw new OverflowException(Locale.GetText("Value is greater than SByte.MaxValue"));
			}
			return (sbyte)value;
		}

		/// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent 8-bit signed integer.</summary>
		/// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(ushort value)
		{
			if (value > 127)
			{
				throw new OverflowException(Locale.GetText("Value is greater than SByte.MaxValue"));
			}
			return (sbyte)value;
		}

		/// <summary>Converts the value of the specified object to an 8-bit signed integer.</summary>
		/// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or null. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format. </exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(object value)
		{
			if (value == null)
			{
				return 0;
			}
			return Convert.ToSByte(value, null);
		}

		/// <summary>Converts the value of the specified object to an 8-bit signed integer, using the specified culture-specific formatting information.</summary>
		/// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format. </exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static sbyte ToSByte(object value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return ((IConvertible)value).ToSByte(provider);
		}

		/// <summary>Converts the specified Boolean value to the equivalent single-precision floating-point number.</summary>
		/// <returns>The number 1 if <paramref name="value" /> is true; otherwise, 0.</returns>
		/// <param name="value">The Boolean value to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static float ToSingle(bool value)
		{
			return (float)((!value) ? 0 : 1);
		}

		/// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent single-precision floating-point number.</summary>
		/// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static float ToSingle(byte value)
		{
			return (float)value;
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The Unicode character to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static float ToSingle(char value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The date and time value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		public static float ToSingle(DateTime value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Converts the value of the specified decimal number to an equivalent single-precision floating-point number.</summary>
		/// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.<paramref name="value" /> is rounded using rounding to nearest. For example, when rounded to two decimals, the value 2.345 becomes 2.34 and the value 2.355 becomes 2.36.</returns>
		/// <param name="value">The decimal number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static float ToSingle(decimal value)
		{
			return (float)value;
		}

		/// <summary>Converts the value of the specified double-precision floating-point number to an equivalent single-precision floating-point number.</summary>
		/// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.<paramref name="value" /> is rounded using rounding to nearest. For example, when rounded to two decimals, the value 2.345 becomes 2.34 and the value 2.355 becomes 2.36.</returns>
		/// <param name="value">The double-precision floating-point number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static float ToSingle(double value)
		{
			return (float)value;
		}

		/// <summary>Returns the specified single-precision floating-point number; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">The single-precision floating-point number to return. </param>
		/// <filterpriority>1</filterpriority>
		public static float ToSingle(float value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 32-bit signed integer to an equivalent single-precision floating-point number.</summary>
		/// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static float ToSingle(int value)
		{
			return (float)value;
		}

		/// <summary>Converts the value of the specified 64-bit signed integer to an equivalent single-precision floating-point number.</summary>
		/// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static float ToSingle(long value)
		{
			return (float)value;
		}

		/// <summary>Converts the value of the specified 8-bit signed integer to the equivalent single-precision floating-point number.</summary>
		/// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static float ToSingle(sbyte value)
		{
			return (float)value;
		}

		/// <summary>Converts the value of the specified 16-bit signed integer to an equivalent single-precision floating-point number.</summary>
		/// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static float ToSingle(short value)
		{
			return (float)value;
		}

		/// <summary>Converts the specified string representation of a number to an equivalent single-precision floating-point number.</summary>
		/// <returns>A single-precision floating-point number that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not a number in a valid format.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static float ToSingle(string value)
		{
			if (value == null)
			{
				return 0f;
			}
			return float.Parse(value);
		}

		/// <summary>Converts the specified string representation of a number to an equivalent single-precision floating-point number, using the specified culture-specific formatting information.</summary>
		/// <returns>A single-precision floating-point number that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not a number in a valid format.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static float ToSingle(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0f;
			}
			return float.Parse(value, provider);
		}

		/// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent single-precision floating-point number.</summary>
		/// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static float ToSingle(uint value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent single-precision floating-point number.</summary>
		/// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static float ToSingle(ulong value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent single-precision floating-point number.</summary>
		/// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static float ToSingle(ushort value)
		{
			return (float)value;
		}

		/// <summary>Converts the value of the specified object to a single-precision floating-point number.</summary>
		/// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or null. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static float ToSingle(object value)
		{
			if (value == null)
			{
				return 0f;
			}
			return Convert.ToSingle(value, null);
		}

		/// <summary>Converts the value of the specified object to an single-precision floating-point number, using the specified culture-specific formatting information.</summary>
		/// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement <see cref="T:System.IConvertible" />. </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static float ToSingle(object value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0f;
			}
			return ((IConvertible)value).ToSingle(provider);
		}

		/// <summary>Converts the specified Boolean value to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The Boolean value to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(bool value)
		{
			return value.ToString();
		}

		/// <summary>Converts the specified Boolean value to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The Boolean value to convert. </param>
		/// <param name="provider">An instance of an object. This parameter is ignored.</param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(bool value, IFormatProvider provider)
		{
			return value.ToString();
		}

		/// <summary>Converts the value of the specified 8-bit unsigned integer to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(byte value)
		{
			return value.ToString();
		}

		/// <summary>Converts the value of the specified 8-bit unsigned integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(byte value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		/// <summary>Converts the value of an 8-bit unsigned integer to its equivalent string representation in a specified base.</summary>
		/// <returns>The string representation of <paramref name="value" /> in base <paramref name="toBase" />.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <param name="toBase">The base of the return value, which must be 2, 8, 10, or 16. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="toBase" /> is not 2, 8, 10, or 16. </exception>
		/// <filterpriority>1</filterpriority>
		public static string ToString(byte value, int toBase)
		{
			if (value == 0)
			{
				return "0";
			}
			if (toBase == 10)
			{
				return value.ToString();
			}
			byte[] bytes = BitConverter.GetBytes((short)value);
			if (toBase == 2)
			{
				return Convert.ConvertToBase2(bytes);
			}
			if (toBase == 8)
			{
				return Convert.ConvertToBase8(bytes);
			}
			if (toBase != 16)
			{
				throw new ArgumentException(Locale.GetText("toBase is not valid."));
			}
			return Convert.ConvertToBase16(bytes);
		}

		/// <summary>Converts the value of the specified Unicode character to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The Unicode character to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(char value)
		{
			return value.ToString();
		}

		/// <summary>Converts the value of the specified Unicode character to its equivalent string representation, using the specified culture-specific formatting information.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The Unicode character to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(char value, IFormatProvider provider)
		{
			return value.ToString();
		}

		/// <summary>Converts the value of the specified <see cref="T:System.DateTime" /> to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The date and time value to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(DateTime value)
		{
			return value.ToString();
		}

		/// <summary>Converts the value of the specified <see cref="T:System.DateTime" /> to its equivalent string representation, using the specified culture-specific formatting information.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The date and time value to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(DateTime value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		/// <summary>Converts the value of the specified decimal number to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The decimal number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(decimal value)
		{
			return value.ToString();
		}

		/// <summary>Converts the value of the specified decimal number to its equivalent string representation, using the specified culture-specific formatting information.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The decimal number to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(decimal value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		/// <summary>Converts the value of the specified double-precision floating-point number to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The double-precision floating-point number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(double value)
		{
			return value.ToString();
		}

		/// <summary>Converts the value of the specified double-precision floating-point number to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The double-precision floating-point number to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(double value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		/// <summary>Converts the value of the specified single-precision floating-point number to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The single-precision floating-point number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(float value)
		{
			return value.ToString();
		}

		/// <summary>Converts the value of the specified single-precision floating-point number to its equivalent string representation, using the specified culture-specific formatting information.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The single-precision floating-point number to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(float value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		/// <summary>Converts the value of the specified 32-bit signed integer to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(int value)
		{
			return value.ToString();
		}

		/// <summary>Converts the value of a 32-bit signed integer to its equivalent string representation in a specified base.</summary>
		/// <returns>The string representation of <paramref name="value" /> in base <paramref name="toBase" />.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <param name="toBase">The base of the return value, which must be 2, 8, 10, or 16. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="toBase" /> is not 2, 8, 10, or 16. </exception>
		/// <filterpriority>1</filterpriority>
		public static string ToString(int value, int toBase)
		{
			if (value == 0)
			{
				return "0";
			}
			if (toBase == 10)
			{
				return value.ToString();
			}
			byte[] bytes = BitConverter.GetBytes(value);
			if (toBase == 2)
			{
				return Convert.ConvertToBase2(bytes);
			}
			if (toBase == 8)
			{
				return Convert.ConvertToBase8(bytes);
			}
			if (toBase != 16)
			{
				throw new ArgumentException(Locale.GetText("toBase is not valid."));
			}
			return Convert.ConvertToBase16(bytes);
		}

		/// <summary>Converts the value of the specified 32-bit signed integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(int value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		/// <summary>Converts the value of the specified 64-bit signed integer to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(long value)
		{
			return value.ToString();
		}

		/// <summary>Converts the value of a 64-bit signed integer to its equivalent string representation in a specified base.</summary>
		/// <returns>The string representation of <paramref name="value" /> in base <paramref name="toBase" />.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <param name="toBase">The base of the return value, which must be 2, 8, 10, or 16. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="toBase" /> is not 2, 8, 10, or 16. </exception>
		/// <filterpriority>1</filterpriority>
		public static string ToString(long value, int toBase)
		{
			if (value == 0L)
			{
				return "0";
			}
			if (toBase == 10)
			{
				return value.ToString();
			}
			byte[] bytes = BitConverter.GetBytes(value);
			if (toBase == 2)
			{
				return Convert.ConvertToBase2(bytes);
			}
			if (toBase == 8)
			{
				return Convert.ConvertToBase8(bytes);
			}
			if (toBase != 16)
			{
				throw new ArgumentException(Locale.GetText("toBase is not valid."));
			}
			return Convert.ConvertToBase16(bytes);
		}

		/// <summary>Converts the value of the specified 64-bit signed integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(long value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		/// <summary>Converts the value of the specified object to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />, or <see cref="F:System.String.Empty" /> if value is null.</returns>
		/// <param name="value">An object that supplies the value to convert, or null. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(object value)
		{
			return Convert.ToString(value, null);
		}

		/// <summary>Converts the value of the specified object to its equivalent string representation using the specified culture-specific formatting information.</summary>
		/// <returns>The string representation of <paramref name="value" />, or <see cref="F:System.String.Empty" /> if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that supplies the value to convert, or null. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(object value, IFormatProvider provider)
		{
			if (value is IConvertible)
			{
				return ((IConvertible)value).ToString(provider);
			}
			if (value != null)
			{
				return value.ToString();
			}
			return string.Empty;
		}

		/// <summary>Converts the value of the specified 8-bit signed integer to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static string ToString(sbyte value)
		{
			return value.ToString();
		}

		/// <summary>Converts the value of the specified 8-bit signed integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit signed integer to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static string ToString(sbyte value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		/// <summary>Converts the value of the specified 16-bit signed integer to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(short value)
		{
			return value.ToString();
		}

		/// <summary>Converts the value of a 16-bit signed integer to its equivalent string representation in a specified base.</summary>
		/// <returns>The string representation of <paramref name="value" /> in base <paramref name="toBase" />.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <param name="toBase">The base of the return value, which must be 2, 8, 10, or 16. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="toBase" /> is not 2, 8, 10, or 16. </exception>
		/// <filterpriority>1</filterpriority>
		public static string ToString(short value, int toBase)
		{
			if (value == 0)
			{
				return "0";
			}
			if (toBase == 10)
			{
				return value.ToString();
			}
			byte[] bytes = BitConverter.GetBytes(value);
			if (toBase == 2)
			{
				return Convert.ConvertToBase2(bytes);
			}
			if (toBase == 8)
			{
				return Convert.ConvertToBase8(bytes);
			}
			if (toBase != 16)
			{
				throw new ArgumentException(Locale.GetText("toBase is not valid."));
			}
			return Convert.ConvertToBase16(bytes);
		}

		/// <summary>Converts the value of the specified 16-bit signed integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(short value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		/// <summary>Returns the specified string instance; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">The string to return. </param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(string value)
		{
			return value;
		}

		/// <summary>Returns the specified string instance; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">The string to return. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. This parameter is ignored.</param>
		/// <filterpriority>1</filterpriority>
		public static string ToString(string value, IFormatProvider provider)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 32-bit unsigned integer to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static string ToString(uint value)
		{
			return value.ToString();
		}

		/// <summary>Converts the value of the specified 32-bit unsigned integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit unsigned integer to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static string ToString(uint value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		/// <summary>Converts the value of the specified 64-bit unsigned integer to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static string ToString(ulong value)
		{
			return value.ToString();
		}

		/// <summary>Converts the value of the specified 64-bit unsigned integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit unsigned integer to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static string ToString(ulong value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		/// <summary>Converts the value of the specified 16-bit unsigned integer to its equivalent string representation.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static string ToString(ushort value)
		{
			return value.ToString();
		}

		/// <summary>Converts the value of the specified 16-bit unsigned integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
		/// <returns>The string representation of <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit unsigned integer to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static string ToString(ushort value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		/// <summary>Converts the specified Boolean value to the equivalent 16-bit unsigned integer.</summary>
		/// <returns>The number 1 if <paramref name="value" /> is true; otherwise, 0.</returns>
		/// <param name="value">The Boolean value to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(bool value)
		{
			return (!value) ? 0 : 1;
		}

		/// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent 16-bit unsigned integer.</summary>
		/// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(byte value)
		{
			return (ushort)value;
		}

		/// <summary>Converts the value of the specified Unicode character to the equivalent 16-bit unsigned integer.</summary>
		/// <returns>The 16-bit unsigned integer equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The Unicode character to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(char value)
		{
			return (ushort)value;
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The date and time value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(DateTime value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Converts the value of the specified decimal number to an equivalent 16-bit unsigned integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 16-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The decimal number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(decimal value)
		{
			if (value > 65535m || value < 0m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt16.MaxValue or less than UInt16.MinValue"));
			}
			return (ushort)Math.Round(value);
		}

		/// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 16-bit unsigned integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 16-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The double-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(double value)
		{
			if (value > 65535.0 || value < 0.0)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt16.MaxValue or less than UInt16.MinValue"));
			}
			return (ushort)Math.Round(value);
		}

		/// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 16-bit unsigned integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 16-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The single-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(float value)
		{
			if (value > 65535f || value < 0f)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt16.MaxValue or less than UInt16.MinValue"));
			}
			return (ushort)Math.Round((double)value);
		}

		/// <summary>Converts the value of the specified 32-bit signed integer to an equivalent 16-bit unsigned integer.</summary>
		/// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(int value)
		{
			if (value > 65535 || value < 0)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt16.MaxValue or less than UInt16.MinValue"));
			}
			return (ushort)value;
		}

		/// <summary>Converts the value of the specified 64-bit signed integer to an equivalent 16-bit unsigned integer.</summary>
		/// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(long value)
		{
			if (value > 65535L || value < 0L)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt16.MaxValue or less than UInt16.MinValue"));
			}
			return (ushort)value;
		}

		/// <summary>Converts the value of the specified 8-bit signed integer to the equivalent 16-bit unsigned integer.</summary>
		/// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(sbyte value)
		{
			if ((int)value < 0)
			{
				throw new OverflowException(Locale.GetText("Value is less than UInt16.MinValue"));
			}
			return (ushort)value;
		}

		/// <summary>Converts the value of the specified 16-bit signed integer to the equivalent 16-bit unsigned integer.</summary>
		/// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(short value)
		{
			if (value < 0)
			{
				throw new OverflowException(Locale.GetText("Value is less than UInt16.MinValue"));
			}
			return (ushort)value;
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 16-bit unsigned integer.</summary>
		/// <returns>A 16-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return ushort.Parse(value);
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 16-bit unsigned integer, using the specified culture-specific formatting information.</summary>
		/// <returns>A 16-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return ushort.Parse(value, provider);
		}

		/// <summary>Converts the string representation of a number in a specified base to an equivalent 16-bit unsigned integer.</summary>
		/// <returns>A 16-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="fromBase" /> is not 2, 8, 10, or 16. -or-<paramref name="value" />, which represents a non-base 10 unsigned number, is prefixed with a negative sign. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" />, which represents a non-base 10 unsigned number, is prefixed with a negative sign.-or-<paramref name="value" /> represents a number that is less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(string value, int fromBase)
		{
			return Convert.ToUInt16(Convert.ConvertFromBase(value, fromBase, true));
		}

		/// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent 16-bit unsigned integer.</summary>
		/// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.UInt16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(uint value)
		{
			if (value > 65535u)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt16.MaxValue"));
			}
			return (ushort)value;
		}

		/// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent 16-bit unsigned integer.</summary>
		/// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.UInt16.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(ulong value)
		{
			if (value > 65535UL)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt16.MaxValue"));
			}
			return (ushort)value;
		}

		/// <summary>Returns the specified 16-bit unsigned integer; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">The 16-bit unsigned integer to return. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(ushort value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified object to a 16-bit unsigned integer.</summary>
		/// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or null. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the  <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(object value)
		{
			if (value == null)
			{
				return 0;
			}
			return Convert.ToUInt16(value, null);
		}

		/// <summary>Converts the value of the specified object to a 16-bit unsigned integer, using the specified culture-specific formatting information.</summary>
		/// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the  <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ushort ToUInt16(object value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return ((IConvertible)value).ToUInt16(provider);
		}

		/// <summary>Converts the specified Boolean value to the equivalent 32-bit unsigned integer.</summary>
		/// <returns>The number 1 if <paramref name="value" /> is true; otherwise, 0.</returns>
		/// <param name="value">The Boolean value to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(bool value)
		{
			return (!value) ? 0u : 1u;
		}

		/// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent 32-bit unsigned integer.</summary>
		/// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(byte value)
		{
			return (uint)value;
		}

		/// <summary>Converts the value of the specified Unicode character to the equivalent 32-bit unsigned integer.</summary>
		/// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The Unicode character to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(char value)
		{
			return (uint)value;
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The date and time value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(DateTime value)
		{
			throw new InvalidCastException("This conversion is not supported.");
		}

		/// <summary>Converts the value of the specified decimal number to an equivalent 32-bit unsigned integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 32-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The decimal number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(decimal value)
		{
			if (value > 4294967295m || value < 0m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt32.MaxValue or less than UInt32.MinValue"));
			}
			return (uint)Math.Round(value);
		}

		/// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 32-bit unsigned integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 32-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The double-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(double value)
		{
			if (value > 4294967295.0 || value < 0.0)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt32.MaxValue or less than UInt32.MinValue"));
			}
			return (uint)Math.Round(value);
		}

		/// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 32-bit unsigned integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 32-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The single-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(float value)
		{
			if (value > 4.2949673E+09f || value < 0f)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt32.MaxValue or less than UInt32.MinValue"));
			}
			return (uint)Math.Round((double)value);
		}

		/// <summary>Converts the value of the specified 32-bit signed integer to an equivalent 32-bit unsigned integer.</summary>
		/// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(int value)
		{
			if ((long)value < 0L)
			{
				throw new OverflowException(Locale.GetText("Value is less than UInt32.MinValue"));
			}
			return (uint)value;
		}

		/// <summary>Converts the value of the specified 64-bit signed integer to an equivalent 32-bit unsigned integer.</summary>
		/// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(long value)
		{
			if (value > (long)((ulong)-1) || value < 0L)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt32.MaxValue or less than UInt32.MinValue"));
			}
			return (uint)value;
		}

		/// <summary>Converts the value of the specified 8-bit signed integer to the equivalent 32-bit unsigned integer.</summary>
		/// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(sbyte value)
		{
			if ((long)value < 0L)
			{
				throw new OverflowException(Locale.GetText("Value is less than UInt32.MinValue"));
			}
			return (uint)value;
		}

		/// <summary>Converts the value of the specified 16-bit signed integer to the equivalent 32-bit unsigned integer.</summary>
		/// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(short value)
		{
			if ((long)value < 0L)
			{
				throw new OverflowException(Locale.GetText("Value is less than UInt32.MinValue"));
			}
			return (uint)value;
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 32-bit unsigned integer.</summary>
		/// <returns>A 32-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(string value)
		{
			if (value == null)
			{
				return 0u;
			}
			return uint.Parse(value);
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 32-bit unsigned integer, using the specified culture-specific formatting information.</summary>
		/// <returns>A 32-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0u;
			}
			return uint.Parse(value, provider);
		}

		/// <summary>Converts the string representation of a number in a specified base to an equivalent 32-bit unsigned integer.</summary>
		/// <returns>A 32-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="fromBase" /> is not 2, 8, 10, or 16. -or-<paramref name="value" />, which represents a non-base 10 unsigned number, is prefixed with a negative sign. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" />, which represents a non-base 10 unsigned number, is prefixed with a negative sign.-or-<paramref name="value" /> represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(string value, int fromBase)
		{
			return (uint)Convert.ConvertFromBase(value, fromBase, true);
		}

		/// <summary>Returns the specified 32-bit unsigned integer; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">The 32-bit unsigned integer to return. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(uint value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent 32-bit unsigned integer.</summary>
		/// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit unsigned integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is greater than <see cref="F:System.UInt32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(ulong value)
		{
			if (value > (ulong)-1)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt32.MaxValue"));
			}
			return (uint)value;
		}

		/// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent 32-bit unsigned integer.</summary>
		/// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(ushort value)
		{
			return (uint)value;
		}

		/// <summary>Converts the value of the specified object to a 32-bit unsigned integer.</summary>
		/// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or null. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(object value)
		{
			if (value == null)
			{
				return 0u;
			}
			return Convert.ToUInt32(value, null);
		}

		/// <summary>Converts the value of the specified object to a 32-bit unsigned integer, using the specified culture-specific formatting information.</summary>
		/// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static uint ToUInt32(object value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0u;
			}
			return ((IConvertible)value).ToUInt32(provider);
		}

		/// <summary>Converts the specified Boolean value to the equivalent 64-bit unsigned integer.</summary>
		/// <returns>The number 1 if <paramref name="value" /> is true; otherwise, 0.</returns>
		/// <param name="value">The Boolean value to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(bool value)
		{
			return (ulong)((!value) ? 0L : 1L);
		}

		/// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent 64-bit unsigned integer.</summary>
		/// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(byte value)
		{
			return (ulong)value;
		}

		/// <summary>Converts the value of the specified Unicode character to the equivalent 64-bit unsigned integer.</summary>
		/// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The Unicode character to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(char value)
		{
			return (ulong)value;
		}

		/// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>This conversion is not supported. No value is returned.</returns>
		/// <param name="value">The date and time value to convert. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(DateTime value)
		{
			throw new InvalidCastException("The conversion is not supported.");
		}

		/// <summary>Converts the value of the specified decimal number to an equivalent 64-bit unsigned integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 64-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The decimal number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt64.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(decimal value)
		{
			if (value > 18446744073709551615m || value < 0m)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt64.MaxValue or less than UInt64.MinValue"));
			}
			return (ulong)Math.Round(value);
		}

		/// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 64-bit unsigned integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 64-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The double-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt64.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(double value)
		{
			if (value > 1.8446744073709552E+19 || value < 0.0)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt64.MaxValue or less than UInt64.MinValue"));
			}
			return (ulong)Math.Round(value);
		}

		/// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 64-bit unsigned integer.</summary>
		/// <returns>
		///   <paramref name="value" />, rounded to the nearest 64-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
		/// <param name="value">The single-precision floating-point number to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt64.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(float value)
		{
			if (value > 1.84467441E+19f || value < 0f)
			{
				throw new OverflowException(Locale.GetText("Value is greater than UInt64.MaxValue or less than UInt64.MinValue"));
			}
			return (ulong)Math.Round((double)value);
		}

		/// <summary>Converts the value of the specified 32-bit signed integer to an equivalent 64-bit unsigned integer.</summary>
		/// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(int value)
		{
			if (value < 0)
			{
				throw new OverflowException(Locale.GetText("Value is less than UInt64.MinValue"));
			}
			return (ulong)((long)value);
		}

		/// <summary>Converts the value of the specified 64-bit signed integer to an equivalent 64-bit unsigned integer.</summary>
		/// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 64-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(long value)
		{
			if (value < 0L)
			{
				throw new OverflowException(Locale.GetText("Value is less than UInt64.MinValue"));
			}
			return (ulong)value;
		}

		/// <summary>Converts the value of the specified 8-bit signed integer to the equivalent 64-bit unsigned integer.</summary>
		/// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 8-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(sbyte value)
		{
			if ((int)value < 0)
			{
				throw new OverflowException("Value is less than UInt64.MinValue");
			}
			return (ulong)((long)value);
		}

		/// <summary>Converts the value of the specified 16-bit signed integer to the equivalent 64-bit unsigned integer.</summary>
		/// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit signed integer to convert. </param>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> is less than zero. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(short value)
		{
			if (value < 0)
			{
				throw new OverflowException(Locale.GetText("Value is less than UInt64.MinValue"));
			}
			return (ulong)((long)value);
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 64-bit unsigned integer.</summary>
		/// <returns>A 64-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(string value)
		{
			if (value == null)
			{
				return 0UL;
			}
			return ulong.Parse(value);
		}

		/// <summary>Converts the specified string representation of a number to an equivalent 64-bit unsigned integer, using the specified culture-specific formatting information.</summary>
		/// <returns>A 64-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9). </exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0UL;
			}
			return ulong.Parse(value, provider);
		}

		/// <summary>Converts the string representation of a number in a specified base to an equivalent 64-bit unsigned integer.</summary>
		/// <returns>A 64-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is null.</returns>
		/// <param name="value">A string that contains the number to convert. </param>
		/// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="fromBase" /> is not 2, 8, 10, or 16. -or-<paramref name="value" />, which represents a non-base 10 unsigned number, is prefixed with a negative sign. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" />, which represents a non-base 10 unsigned number, is prefixed with a negative sign.-or-<paramref name="value" /> represents a number that is less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(string value, int fromBase)
		{
			return (ulong)Convert.ConvertFromBase64(value, fromBase, true);
		}

		/// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent 64-bit unsigned integer.</summary>
		/// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 32-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(uint value)
		{
			return (ulong)value;
		}

		/// <summary>Returns the specified 64-bit unsigned integer; no actual conversion is performed.</summary>
		/// <returns>
		///   <paramref name="value" /> is returned unchanged.</returns>
		/// <param name="value">The 64-bit unsigned integer to return. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(ulong value)
		{
			return value;
		}

		/// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent 64-bit unsigned integer.</summary>
		/// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The 16-bit unsigned integer to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(ushort value)
		{
			return (ulong)value;
		}

		/// <summary>Converts the value of the specified object to a 64-bit unsigned integer.</summary>
		/// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or null. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(object value)
		{
			if (value == null)
			{
				return 0UL;
			}
			return Convert.ToUInt64(value, null);
		}

		/// <summary>Converts the value of the specified object to a 64-bit unsigned integer, using the specified culture-specific formatting information.</summary>
		/// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is null.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in an appropriate format.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///   <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface. -or-The conversion is not supported.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public static ulong ToUInt64(object value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0UL;
			}
			return ((IConvertible)value).ToUInt64(provider);
		}

		/// <summary>Returns an object of the specified type and whose value is equivalent to the specified object.</summary>
		/// <returns>An object whose type is <paramref name="conversionType" /> and whose value is equivalent to <paramref name="value" />.-or-A null reference (Nothing in Visual Basic), if <paramref name="value" /> is null and <paramref name="conversionType" /> is not a value type. </returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="conversionType">The type of object to return. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported.  -or-<paramref name="value" /> is null and <paramref name="conversionType" /> is a value type.-or-<paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in a format recognized by <paramref name="conversionType" />.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is out of the range of <paramref name="conversionType" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="conversionType" /> is null.</exception>
		/// <filterpriority>1</filterpriority>
		public static object ChangeType(object value, Type conversionType)
		{
			if (value != null && conversionType == null)
			{
				throw new ArgumentNullException("conversionType");
			}
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			IFormatProvider provider;
			if (conversionType == typeof(DateTime))
			{
				provider = currentCulture.DateTimeFormat;
			}
			else
			{
				provider = currentCulture.NumberFormat;
			}
			return Convert.ToType(value, conversionType, provider, true);
		}

		/// <summary>Returns an object of the specified type whose value is equivalent to the specified object.</summary>
		/// <returns>An object whose underlying type is <paramref name="typeCode" /> and whose value is equivalent to <paramref name="value" />.-or-A null reference (Nothing in Visual Basic), if <paramref name="value" /> is null and <paramref name="typeCode" /> is <see cref="F:System.TypeCode.Empty" />, <see cref="F:System.TypeCode.String" />, or <see cref="F:System.TypeCode.Object" />.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="typeCode">The type of object to return. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported.  -or-<paramref name="value" /> is null and <paramref name="typeCode" /> specifies a value type.-or-<paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in a format recognized by the <paramref name="typeCode" /> type.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is out of the range of the <paramref name="typeCode" /> type.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="typeCode" /> is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public static object ChangeType(object value, TypeCode typeCode)
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			Type type = Convert.conversionTable[(int)typeCode];
			IFormatProvider provider;
			if (type == typeof(DateTime))
			{
				provider = currentCulture.DateTimeFormat;
			}
			else
			{
				provider = currentCulture.NumberFormat;
			}
			return Convert.ToType(value, type, provider, true);
		}

		/// <summary>Returns an object of the specified type whose value is equivalent to the specified object. A parameter supplies culture-specific formatting information.</summary>
		/// <returns>An object whose type is <paramref name="conversionType" /> and whose value is equivalent to <paramref name="value" />.-or- <paramref name="value" />, if the <see cref="T:System.Type" /> of <paramref name="value" /> and <paramref name="conversionType" /> are equal.-or- A null reference (Nothing in Visual Basic), if <paramref name="value" /> is null and <paramref name="conversionType" /> is not a value type.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="conversionType">The type of object to return. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported. -or-<paramref name="value" /> is null and <paramref name="conversionType" /> is a value type.-or-<paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in a format for <paramref name="conversionType" /> recognized by <paramref name="provider" />.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is out of the range of <paramref name="conversionType" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="conversionType" /> is null.</exception>
		/// <filterpriority>1</filterpriority>
		public static object ChangeType(object value, Type conversionType, IFormatProvider provider)
		{
			if (value != null && conversionType == null)
			{
				throw new ArgumentNullException("conversionType");
			}
			return Convert.ToType(value, conversionType, provider, true);
		}

		/// <summary>Returns an object of the specified type whose value is equivalent to the specified object. A parameter supplies culture-specific formatting information.</summary>
		/// <returns>An object whose underlying type is <paramref name="typeCode" /> and whose value is equivalent to <paramref name="value" />.-or- A null reference (Nothing in Visual Basic), if <paramref name="value" /> is null and <paramref name="typeCode" /> is <see cref="F:System.TypeCode.Empty" />, <see cref="F:System.TypeCode.String" />, or <see cref="F:System.TypeCode.Object" />.</returns>
		/// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface. </param>
		/// <param name="typeCode">The type of object to return. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported.  -or-<paramref name="value" /> is null and <paramref name="typeCode" /> specifies a value type.-or-<paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="value" /> is not in a format for the <paramref name="typeCode" /> type recognized by <paramref name="provider" />.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="value" /> represents a number that is out of the range of the <paramref name="typeCode" /> type.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="typeCode" /> is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public static object ChangeType(object value, TypeCode typeCode, IFormatProvider provider)
		{
			Type conversionType = Convert.conversionTable[(int)typeCode];
			return Convert.ToType(value, conversionType, provider, true);
		}

		private static bool NotValidBase(int value)
		{
			return value != 2 && value != 8 && value != 10 && value != 16;
		}

		private static int ConvertFromBase(string value, int fromBase, bool unsigned)
		{
			if (Convert.NotValidBase(fromBase))
			{
				throw new ArgumentException("fromBase is not valid.");
			}
			if (value == null)
			{
				return 0;
			}
			int num = 0;
			int num2 = 0;
			int i = 0;
			int length = value.Length;
			bool flag = false;
			if (fromBase != 10)
			{
				if (fromBase != 16)
				{
					if (value.Substring(i, 1) == "-")
					{
						throw new ArgumentException("String cannot contain a minus sign if the base is not 10.");
					}
				}
				else
				{
					if (value.Substring(i, 1) == "-")
					{
						throw new ArgumentException("String cannot contain a minus sign if the base is not 10.");
					}
					if (length >= i + 2 && value[i] == '0' && (value[i + 1] == 'x' || value[i + 1] == 'X'))
					{
						i += 2;
					}
				}
			}
			else if (value.Substring(i, 1) == "-")
			{
				if (unsigned)
				{
					throw new OverflowException(Locale.GetText("The string was being parsed as an unsigned number and could not have a negative sign."));
				}
				flag = true;
				i++;
			}
			if (length == i)
			{
				throw new FormatException("Could not find any parsable digits.");
			}
			if (value[i] == '+')
			{
				i++;
			}
			while (i < length)
			{
				char c = value[i++];
				int num3;
				if (char.IsNumber(c))
				{
					num3 = (int)(c - '0');
				}
				else if (char.IsLetter(c))
				{
					num3 = (int)(char.ToLowerInvariant(c) - 'a' + '\n');
				}
				else
				{
					if (num > 0)
					{
						throw new FormatException("Additional unparsable characters are at the end of the string.");
					}
					throw new FormatException("Could not find any parsable digits.");
				}
				if (num3 >= fromBase)
				{
					if (num > 0)
					{
						throw new FormatException("Additional unparsable characters are at the end of the string.");
					}
					throw new FormatException("Could not find any parsable digits.");
				}
				else
				{
					num2 = fromBase * num2 + num3;
					num++;
				}
			}
			if (num == 0)
			{
				throw new FormatException("Could not find any parsable digits.");
			}
			if (flag)
			{
				return -num2;
			}
			return num2;
		}

		private static long ConvertFromBase64(string value, int fromBase, bool unsigned)
		{
			if (Convert.NotValidBase(fromBase))
			{
				throw new ArgumentException("fromBase is not valid.");
			}
			if (value == null)
			{
				return 0L;
			}
			int num = 0;
			long num2 = 0L;
			bool flag = false;
			int i = 0;
			int length = value.Length;
			if (fromBase != 10)
			{
				if (fromBase != 16)
				{
					if (value.Substring(i, 1) == "-")
					{
						throw new ArgumentException("String cannot contain a minus sign if the base is not 10.");
					}
				}
				else
				{
					if (value.Substring(i, 1) == "-")
					{
						throw new ArgumentException("String cannot contain a minus sign if the base is not 10.");
					}
					if (length >= i + 2 && value[i] == '0' && (value[i + 1] == 'x' || value[i + 1] == 'X'))
					{
						i += 2;
					}
				}
			}
			else if (value.Substring(i, 1) == "-")
			{
				if (unsigned)
				{
					throw new OverflowException(Locale.GetText("The string was being parsed as an unsigned number and could not have a negative sign."));
				}
				flag = true;
				i++;
			}
			if (length == i)
			{
				throw new FormatException("Could not find any parsable digits.");
			}
			if (value[i] == '+')
			{
				i++;
			}
			while (i < length)
			{
				char c = value[i++];
				int num3;
				if (char.IsNumber(c))
				{
					num3 = (int)(c - '0');
				}
				else if (char.IsLetter(c))
				{
					num3 = (int)(char.ToLowerInvariant(c) - 'a' + '\n');
				}
				else
				{
					if (num > 0)
					{
						throw new FormatException("Additional unparsable characters are at the end of the string.");
					}
					throw new FormatException("Could not find any parsable digits.");
				}
				if (num3 >= fromBase)
				{
					if (num > 0)
					{
						throw new FormatException("Additional unparsable characters are at the end of the string.");
					}
					throw new FormatException("Could not find any parsable digits.");
				}
				else
				{
					num2 = (long)fromBase * num2 + (long)num3;
					num++;
				}
			}
			if (num == 0)
			{
				throw new FormatException("Could not find any parsable digits.");
			}
			if (flag)
			{
				return -1L * num2;
			}
			return num2;
		}

		private static void EndianSwap(ref byte[] value)
		{
			byte[] array = new byte[value.Length];
			for (int i = 0; i < value.Length; i++)
			{
				array[i] = value[value.Length - 1 - i];
			}
			value = array;
		}

		private static string ConvertToBase2(byte[] value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				Convert.EndianSwap(ref value);
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = value.Length - 1; i >= 0; i--)
			{
				byte b = value[i];
				for (int j = 0; j < 8; j++)
				{
					if ((b & 128) == 128)
					{
						stringBuilder.Append('1');
					}
					else if (stringBuilder.Length > 0)
					{
						stringBuilder.Append('0');
					}
					b = (byte)(b << 1);
				}
			}
			return stringBuilder.ToString();
		}

		private static string ConvertToBase8(byte[] value)
		{
			switch (value.Length)
			{
			case 1:
			{
				ulong num = (ulong)value[0];
				goto IL_74;
			}
			case 2:
			{
				ulong num = (ulong)BitConverter.ToUInt16(value, 0);
				goto IL_74;
			}
			case 4:
			{
				ulong num = (ulong)BitConverter.ToUInt32(value, 0);
				goto IL_74;
			}
			case 8:
			{
				ulong num = BitConverter.ToUInt64(value, 0);
				goto IL_74;
			}
			}
			throw new ArgumentException("value");
			IL_74:
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 21; i >= 0; i--)
			{
				ulong num;
				char c = (char)(num >> i * 3 & 7UL);
				if (c != '\0' || stringBuilder.Length > 0)
				{
					c += '0';
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		private static string ConvertToBase16(byte[] value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				Convert.EndianSwap(ref value);
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = value.Length - 1; i >= 0; i--)
			{
				char c = (char)(value[i] >> 4 & 15);
				if (c != '\0' || stringBuilder.Length > 0)
				{
					if (c < '\n')
					{
						c += '0';
					}
					else
					{
						c -= '\n';
						c += 'a';
					}
					stringBuilder.Append(c);
				}
				char c2 = (char)(value[i] & 15);
				if (c2 != '\0' || stringBuilder.Length > 0)
				{
					if (c2 < '\n')
					{
						c2 += '0';
					}
					else
					{
						c2 -= '\n';
						c2 += 'a';
					}
					stringBuilder.Append(c2);
				}
			}
			return stringBuilder.ToString();
		}

		internal static object ToType(object value, Type conversionType, IFormatProvider provider, bool try_target_to_type)
		{
			if (value == null)
			{
				if (conversionType != null && conversionType.IsValueType)
				{
					throw new InvalidCastException("Null object can not be converted to a value type.");
				}
				return null;
			}
			else
			{
				if (conversionType == null)
				{
					throw new InvalidCastException("Cannot cast to destination type.");
				}
				if (value.GetType() == conversionType)
				{
					return value;
				}
				if (value is IConvertible)
				{
					IConvertible convertible = (IConvertible)value;
					if (conversionType == Convert.conversionTable[0])
					{
						throw new ArgumentNullException();
					}
					if (conversionType == Convert.conversionTable[1])
					{
						return value;
					}
					if (conversionType == Convert.conversionTable[2])
					{
						throw new InvalidCastException("Cannot cast to DBNull, it's not IConvertible");
					}
					if (conversionType == Convert.conversionTable[3])
					{
						return convertible.ToBoolean(provider);
					}
					if (conversionType == Convert.conversionTable[4])
					{
						return convertible.ToChar(provider);
					}
					if (conversionType == Convert.conversionTable[5])
					{
						return convertible.ToSByte(provider);
					}
					if (conversionType == Convert.conversionTable[6])
					{
						return convertible.ToByte(provider);
					}
					if (conversionType == Convert.conversionTable[7])
					{
						return convertible.ToInt16(provider);
					}
					if (conversionType == Convert.conversionTable[8])
					{
						return convertible.ToUInt16(provider);
					}
					if (conversionType == Convert.conversionTable[9])
					{
						return convertible.ToInt32(provider);
					}
					if (conversionType == Convert.conversionTable[10])
					{
						return convertible.ToUInt32(provider);
					}
					if (conversionType == Convert.conversionTable[11])
					{
						return convertible.ToInt64(provider);
					}
					if (conversionType == Convert.conversionTable[12])
					{
						return convertible.ToUInt64(provider);
					}
					if (conversionType == Convert.conversionTable[13])
					{
						return convertible.ToSingle(provider);
					}
					if (conversionType == Convert.conversionTable[14])
					{
						return convertible.ToDouble(provider);
					}
					if (conversionType == Convert.conversionTable[15])
					{
						return convertible.ToDecimal(provider);
					}
					if (conversionType == Convert.conversionTable[16])
					{
						return convertible.ToDateTime(provider);
					}
					if (conversionType == Convert.conversionTable[18])
					{
						return convertible.ToString(provider);
					}
					if (try_target_to_type)
					{
						return convertible.ToType(conversionType, provider);
					}
				}
				throw new InvalidCastException(Locale.GetText("Value is not a convertible object: " + value.GetType().ToString() + " to " + conversionType.FullName));
			}
		}
	}
}
