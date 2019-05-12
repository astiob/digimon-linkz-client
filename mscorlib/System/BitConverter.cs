using System;
using System.Text;

namespace System
{
	/// <summary>Converts base data types to an array of bytes, and an array of bytes to base data types.</summary>
	/// <filterpriority>2</filterpriority>
	public static class BitConverter
	{
		private static readonly bool SwappedWordsInDouble = BitConverter.DoubleWordsAreSwapped();

		/// <summary>Indicates the byte order ("endianess") in which data is stored in this computer architecture.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly bool IsLittleEndian = BitConverter.AmILittleEndian();

		private static bool AmILittleEndian()
		{
			double num = 1.0;
			return num == (double)0;
		}

		private unsafe static bool DoubleWordsAreSwapped()
		{
			double num = 1.0;
			return *(ref num + 2) == 240;
		}

		/// <summary>Converts the specified double-precision floating point number to a 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer whose value is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static long DoubleToInt64Bits(double value)
		{
			return BitConverter.ToInt64(BitConverter.GetBytes(value), 0);
		}

		/// <summary>Converts the specified 64-bit signed integer to a double-precision floating point number.</summary>
		/// <returns>A double-precision floating point number whose value is equivalent to <paramref name="value" />.</returns>
		/// <param name="value">The number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public static double Int64BitsToDouble(long value)
		{
			return BitConverter.ToDouble(BitConverter.GetBytes(value), 0);
		}

		internal static double InternalInt64BitsToDouble(long value)
		{
			return BitConverter.SwappableToDouble(BitConverter.GetBytes(value), 0);
		}

		private unsafe static byte[] GetBytes(byte* ptr, int count)
		{
			byte[] array = new byte[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = ptr[i];
			}
			return array;
		}

		/// <summary>Returns the specified Boolean value as an array of bytes.</summary>
		/// <returns>An array of bytes with length 1.</returns>
		/// <param name="value">A Boolean value. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static byte[] GetBytes(bool value)
		{
			return BitConverter.GetBytes((byte*)(&value), 1);
		}

		/// <summary>Returns the specified Unicode character value as an array of bytes.</summary>
		/// <returns>An array of bytes with length 2.</returns>
		/// <param name="value">A character to convert. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static byte[] GetBytes(char value)
		{
			return BitConverter.GetBytes((byte*)(&value), 2);
		}

		/// <summary>Returns the specified 16-bit signed integer value as an array of bytes.</summary>
		/// <returns>An array of bytes with length 2.</returns>
		/// <param name="value">The number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static byte[] GetBytes(short value)
		{
			return BitConverter.GetBytes((byte*)(&value), 2);
		}

		/// <summary>Returns the specified 32-bit signed integer value as an array of bytes.</summary>
		/// <returns>An array of bytes with length 4.</returns>
		/// <param name="value">The number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static byte[] GetBytes(int value)
		{
			return BitConverter.GetBytes((byte*)(&value), 4);
		}

		/// <summary>Returns the specified 64-bit signed integer value as an array of bytes.</summary>
		/// <returns>An array of bytes with length 8.</returns>
		/// <param name="value">The number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static byte[] GetBytes(long value)
		{
			return BitConverter.GetBytes((byte*)(&value), 8);
		}

		/// <summary>Returns the specified 16-bit unsigned integer value as an array of bytes.</summary>
		/// <returns>An array of bytes with length 2.</returns>
		/// <param name="value">The number to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public unsafe static byte[] GetBytes(ushort value)
		{
			return BitConverter.GetBytes((byte*)(&value), 2);
		}

		/// <summary>Returns the specified 32-bit unsigned integer value as an array of bytes.</summary>
		/// <returns>An array of bytes with length 4.</returns>
		/// <param name="value">The number to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public unsafe static byte[] GetBytes(uint value)
		{
			return BitConverter.GetBytes((byte*)(&value), 4);
		}

		/// <summary>Returns the specified 64-bit unsigned integer value as an array of bytes.</summary>
		/// <returns>An array of bytes with length 8.</returns>
		/// <param name="value">The number to convert. </param>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public unsafe static byte[] GetBytes(ulong value)
		{
			return BitConverter.GetBytes((byte*)(&value), 8);
		}

		/// <summary>Returns the specified single-precision floating point value as an array of bytes.</summary>
		/// <returns>An array of bytes with length 4.</returns>
		/// <param name="value">The number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static byte[] GetBytes(float value)
		{
			return BitConverter.GetBytes((byte*)(&value), 4);
		}

		/// <summary>Returns the specified double-precision floating point value as an array of bytes.</summary>
		/// <returns>An array of bytes with length 8.</returns>
		/// <param name="value">The number to convert. </param>
		/// <filterpriority>1</filterpriority>
		public unsafe static byte[] GetBytes(double value)
		{
			if (BitConverter.SwappedWordsInDouble)
			{
				return new byte[]
				{
					*(ref value + 4),
					*(ref value + 5),
					*(ref value + 6),
					*(ref value + 7),
					(byte)value,
					*(ref value + 1),
					*(ref value + 2),
					*(ref value + 3)
				};
			}
			return BitConverter.GetBytes((byte*)(&value), 8);
		}

		private unsafe static void PutBytes(byte* dst, byte[] src, int start_index, int count)
		{
			if (src == null)
			{
				throw new ArgumentNullException("value");
			}
			if (start_index < 0 || start_index > src.Length - 1)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (src.Length - count < start_index)
			{
				throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
			}
			for (int i = 0; i < count; i++)
			{
				dst[i] = src[i + start_index];
			}
		}

		/// <summary>Returns a Boolean value converted from one byte at a specified position in a byte array.</summary>
		/// <returns>true if the byte at <paramref name="startIndex" /> in <paramref name="value" /> is nonzero; otherwise, false.</returns>
		/// <param name="value">An array of bytes. </param>
		/// <param name="startIndex">The starting position within <paramref name="value" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool ToBoolean(byte[] value, int startIndex)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0 || startIndex > value.Length - 1)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			return value[startIndex] != 0;
		}

		/// <summary>Returns a Unicode character converted from two bytes at a specified position in a byte array.</summary>
		/// <returns>A character formed by two bytes beginning at <paramref name="startIndex" />.</returns>
		/// <param name="value">An array. </param>
		/// <param name="startIndex">The starting position within <paramref name="value" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> equals the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1. </exception>
		/// <filterpriority>1</filterpriority>
		public unsafe static char ToChar(byte[] value, int startIndex)
		{
			char result;
			BitConverter.PutBytes((byte*)(&result), value, startIndex, 2);
			return result;
		}

		/// <summary>Returns a 16-bit signed integer converted from two bytes at a specified position in a byte array.</summary>
		/// <returns>A 16-bit signed integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
		/// <param name="value">An array of bytes. </param>
		/// <param name="startIndex">The starting position within <paramref name="value" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> equals the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1. </exception>
		/// <filterpriority>1</filterpriority>
		public unsafe static short ToInt16(byte[] value, int startIndex)
		{
			short result;
			BitConverter.PutBytes((byte*)(&result), value, startIndex, 2);
			return result;
		}

		/// <summary>Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array.</summary>
		/// <returns>A 32-bit signed integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
		/// <param name="value">An array of bytes. </param>
		/// <param name="startIndex">The starting position within <paramref name="value" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> is greater than or equal to the length of <paramref name="value" /> minus 3, and is less than or equal to the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1. </exception>
		/// <filterpriority>1</filterpriority>
		public unsafe static int ToInt32(byte[] value, int startIndex)
		{
			int result;
			BitConverter.PutBytes((byte*)(&result), value, startIndex, 4);
			return result;
		}

		/// <summary>Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array.</summary>
		/// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
		/// <param name="value">An array of bytes. </param>
		/// <param name="startIndex">The starting position within <paramref name="value" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> is greater than or equal to the length of <paramref name="value" /> minus 7, and is less than or equal to the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1. </exception>
		/// <filterpriority>1</filterpriority>
		public unsafe static long ToInt64(byte[] value, int startIndex)
		{
			long result;
			BitConverter.PutBytes((byte*)(&result), value, startIndex, 8);
			return result;
		}

		/// <summary>Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array.</summary>
		/// <returns>A 16-bit unsigned integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
		/// <param name="value">The array of bytes. </param>
		/// <param name="startIndex">The starting position within <paramref name="value" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> equals the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public unsafe static ushort ToUInt16(byte[] value, int startIndex)
		{
			ushort result;
			BitConverter.PutBytes((byte*)(&result), value, startIndex, 2);
			return result;
		}

		/// <summary>Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array.</summary>
		/// <returns>A 32-bit unsigned integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
		/// <param name="value">An array of bytes. </param>
		/// <param name="startIndex">The starting position within <paramref name="value" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> is greater than or equal to the length of <paramref name="value" /> minus 3, and is less than or equal to the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public unsafe static uint ToUInt32(byte[] value, int startIndex)
		{
			uint result;
			BitConverter.PutBytes((byte*)(&result), value, startIndex, 4);
			return result;
		}

		/// <summary>Returns a 64-bit unsigned integer converted from eight bytes at a specified position in a byte array.</summary>
		/// <returns>A 64-bit unsigned integer formed by the eight bytes beginning at <paramref name="startIndex" />.</returns>
		/// <param name="value">An array of bytes. </param>
		/// <param name="startIndex">The starting position within <paramref name="value" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> is greater than or equal to the length of <paramref name="value" /> minus 7, and is less than or equal to the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1. </exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public unsafe static ulong ToUInt64(byte[] value, int startIndex)
		{
			ulong result;
			BitConverter.PutBytes((byte*)(&result), value, startIndex, 8);
			return result;
		}

		/// <summary>Returns a single-precision floating point number converted from four bytes at a specified position in a byte array.</summary>
		/// <returns>A single-precision floating point number formed by four bytes beginning at <paramref name="startIndex" />.</returns>
		/// <param name="value">An array of bytes. </param>
		/// <param name="startIndex">The starting position within <paramref name="value" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> is greater than or equal to the length of <paramref name="value" /> minus 3, and is less than or equal to the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1. </exception>
		/// <filterpriority>1</filterpriority>
		public unsafe static float ToSingle(byte[] value, int startIndex)
		{
			float result;
			BitConverter.PutBytes((byte*)(&result), value, startIndex, 4);
			return result;
		}

		/// <summary>Returns a double-precision floating point number converted from eight bytes at a specified position in a byte array.</summary>
		/// <returns>A double precision floating point number formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
		/// <param name="value">An array of bytes. </param>
		/// <param name="startIndex">The starting position within <paramref name="value" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> is greater than or equal to the length of <paramref name="value" /> minus 7, and is less than or equal to the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1. </exception>
		/// <filterpriority>1</filterpriority>
		public unsafe static double ToDouble(byte[] value, int startIndex)
		{
			double result;
			if (!BitConverter.SwappedWordsInDouble)
			{
				BitConverter.PutBytes((byte*)(&result), value, startIndex, 8);
				return result;
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0 || startIndex > value.Length - 1)
			{
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (value.Length - 8 < startIndex)
			{
				throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
			}
			result = (double)value[startIndex + 4];
			*(ref result + 1) = value[startIndex + 5];
			*(ref result + 2) = value[startIndex + 6];
			*(ref result + 3) = value[startIndex + 7];
			*(ref result + 4) = value[startIndex];
			*(ref result + 5) = value[startIndex + 1];
			*(ref result + 6) = value[startIndex + 2];
			*(ref result + 7) = value[startIndex + 3];
			return result;
		}

		internal unsafe static double SwappableToDouble(byte[] value, int startIndex)
		{
			if (BitConverter.SwappedWordsInDouble)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (startIndex < 0 || startIndex > value.Length - 1)
				{
					throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				if (value.Length - 8 < startIndex)
				{
					throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
				}
				double result = (double)value[startIndex + 4];
				*(ref result + 1) = value[startIndex + 5];
				*(ref result + 2) = value[startIndex + 6];
				*(ref result + 3) = value[startIndex + 7];
				*(ref result + 4) = value[startIndex];
				*(ref result + 5) = value[startIndex + 1];
				*(ref result + 6) = value[startIndex + 2];
				*(ref result + 7) = value[startIndex + 3];
				return result;
			}
			else
			{
				double result;
				if (BitConverter.IsLittleEndian)
				{
					BitConverter.PutBytes((byte*)(&result), value, startIndex, 8);
					return result;
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (startIndex < 0 || startIndex > value.Length - 1)
				{
					throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
				}
				if (value.Length - 8 < startIndex)
				{
					throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
				}
				result = (double)value[startIndex + 7];
				*(ref result + 1) = value[startIndex + 6];
				*(ref result + 2) = value[startIndex + 5];
				*(ref result + 3) = value[startIndex + 4];
				*(ref result + 4) = value[startIndex + 3];
				*(ref result + 5) = value[startIndex + 2];
				*(ref result + 6) = value[startIndex + 1];
				*(ref result + 7) = value[startIndex];
				return result;
			}
		}

		/// <summary>Converts the numeric value of each element of a specified array of bytes to its equivalent hexadecimal string representation.</summary>
		/// <returns>A <see cref="T:System.String" /> of hexadecimal pairs separated by hyphens, where each pair represents the corresponding element in <paramref name="value" />; for example, "7F-2C-4A".</returns>
		/// <param name="value">An array of bytes. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <filterpriority>1</filterpriority>
		public static string ToString(byte[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return BitConverter.ToString(value, 0, value.Length);
		}

		/// <summary>Converts the numeric value of each element of a specified subarray of bytes to its equivalent hexadecimal string representation.</summary>
		/// <returns>A <see cref="T:System.String" /> of hexadecimal pairs separated by hyphens, where each pair represents the corresponding element in a subarray of <paramref name="value" />; for example, "7F-2C-4A".</returns>
		/// <param name="value">An array of bytes. </param>
		/// <param name="startIndex">The starting position within <paramref name="value" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1. </exception>
		/// <filterpriority>1</filterpriority>
		public static string ToString(byte[] value, int startIndex)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return BitConverter.ToString(value, startIndex, value.Length - startIndex);
		}

		/// <summary>Converts the numeric value of each element of a specified subarray of bytes to its equivalent hexadecimal string representation.</summary>
		/// <returns>A <see cref="T:System.String" /> of hexadecimal pairs separated by hyphens, where each pair represents the corresponding element in a subarray of <paramref name="value" />; for example, "7F-2C-4A".</returns>
		/// <param name="value">An array of bytes. </param>
		/// <param name="startIndex">The starting position within <paramref name="value" />. </param>
		/// <param name="length">The number of array elements in <paramref name="value" /> to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> or <paramref name="length" /> is less than zero.-or-<paramref name="startIndex" /> is greater than zero and is greater than or equal to the length of <paramref name="value" />.</exception>
		/// <exception cref="T:System.ArgumentException">The combination of <paramref name="startIndex" /> and <paramref name="length" /> does not specify a position within <paramref name="value" />; that is, the <paramref name="startIndex" /> parameter is greater than the length of <paramref name="value" /> minus the <paramref name="length" /> parameter.</exception>
		/// <filterpriority>1</filterpriority>
		public static string ToString(byte[] value, int startIndex, int length)
		{
			if (value == null)
			{
				throw new ArgumentNullException("byteArray");
			}
			if (startIndex < 0 || startIndex >= value.Length)
			{
				if (startIndex == 0 && value.Length == 0)
				{
					return string.Empty;
				}
				throw new ArgumentOutOfRangeException("startIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			else
			{
				if (length < 0)
				{
					throw new ArgumentOutOfRangeException("length", "Value must be positive.");
				}
				if (startIndex > value.Length - length)
				{
					throw new ArgumentException("startIndex + length > value.Length");
				}
				if (length == 0)
				{
					return string.Empty;
				}
				StringBuilder stringBuilder = new StringBuilder(length * 3 - 1);
				int num = startIndex + length;
				for (int i = startIndex; i < num; i++)
				{
					if (i > startIndex)
					{
						stringBuilder.Append('-');
					}
					char c = (char)(value[i] >> 4 & 15);
					char c2 = (char)(value[i] & 15);
					if (c < '\n')
					{
						c += '0';
					}
					else
					{
						c -= '\n';
						c += 'A';
					}
					if (c2 < '\n')
					{
						c2 += '0';
					}
					else
					{
						c2 -= '\n';
						c2 += 'A';
					}
					stringBuilder.Append(c);
					stringBuilder.Append(c2);
				}
				return stringBuilder.ToString();
			}
		}
	}
}
