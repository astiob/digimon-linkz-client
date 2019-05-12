using System;

namespace Mono.Security
{
	internal sealed class BitConverterLE
	{
		private BitConverterLE()
		{
		}

		private unsafe static byte[] GetUShortBytes(byte* bytes)
		{
			if (BitConverter.IsLittleEndian)
			{
				return new byte[]
				{
					*bytes,
					bytes[1]
				};
			}
			return new byte[]
			{
				bytes[1],
				*bytes
			};
		}

		private unsafe static byte[] GetUIntBytes(byte* bytes)
		{
			if (BitConverter.IsLittleEndian)
			{
				return new byte[]
				{
					*bytes,
					bytes[1],
					bytes[2],
					bytes[3]
				};
			}
			return new byte[]
			{
				bytes[3],
				bytes[2],
				bytes[1],
				*bytes
			};
		}

		private unsafe static byte[] GetULongBytes(byte* bytes)
		{
			if (BitConverter.IsLittleEndian)
			{
				return new byte[]
				{
					*bytes,
					bytes[1],
					bytes[2],
					bytes[3],
					bytes[4],
					bytes[5],
					bytes[6],
					bytes[7]
				};
			}
			return new byte[]
			{
				bytes[7],
				bytes[6],
				bytes[5],
				bytes[4],
				bytes[3],
				bytes[2],
				bytes[1],
				*bytes
			};
		}

		internal static byte[] GetBytes(bool value)
		{
			return new byte[]
			{
				(!value) ? 0 : 1
			};
		}

		internal unsafe static byte[] GetBytes(char value)
		{
			return BitConverterLE.GetUShortBytes((byte*)(&value));
		}

		internal unsafe static byte[] GetBytes(short value)
		{
			return BitConverterLE.GetUShortBytes((byte*)(&value));
		}

		internal unsafe static byte[] GetBytes(int value)
		{
			return BitConverterLE.GetUIntBytes((byte*)(&value));
		}

		internal unsafe static byte[] GetBytes(long value)
		{
			return BitConverterLE.GetULongBytes((byte*)(&value));
		}

		internal unsafe static byte[] GetBytes(ushort value)
		{
			return BitConverterLE.GetUShortBytes((byte*)(&value));
		}

		internal unsafe static byte[] GetBytes(uint value)
		{
			return BitConverterLE.GetUIntBytes((byte*)(&value));
		}

		internal unsafe static byte[] GetBytes(ulong value)
		{
			return BitConverterLE.GetULongBytes((byte*)(&value));
		}

		internal unsafe static byte[] GetBytes(float value)
		{
			return BitConverterLE.GetUIntBytes((byte*)(&value));
		}

		internal unsafe static byte[] GetBytes(double value)
		{
			return BitConverterLE.GetULongBytes((byte*)(&value));
		}

		private unsafe static void UShortFromBytes(byte* dst, byte[] src, int startIndex)
		{
			if (BitConverter.IsLittleEndian)
			{
				*dst = src[startIndex];
				dst[1] = src[startIndex + 1];
			}
			else
			{
				*dst = src[startIndex + 1];
				dst[1] = src[startIndex];
			}
		}

		private unsafe static void UIntFromBytes(byte* dst, byte[] src, int startIndex)
		{
			if (BitConverter.IsLittleEndian)
			{
				*dst = src[startIndex];
				dst[1] = src[startIndex + 1];
				dst[2] = src[startIndex + 2];
				dst[3] = src[startIndex + 3];
			}
			else
			{
				*dst = src[startIndex + 3];
				dst[1] = src[startIndex + 2];
				dst[2] = src[startIndex + 1];
				dst[3] = src[startIndex];
			}
		}

		private unsafe static void ULongFromBytes(byte* dst, byte[] src, int startIndex)
		{
			if (BitConverter.IsLittleEndian)
			{
				for (int i = 0; i < 8; i++)
				{
					dst[i] = src[startIndex + i];
				}
			}
			else
			{
				for (int j = 0; j < 8; j++)
				{
					dst[j] = src[startIndex + (7 - j)];
				}
			}
		}

		internal static bool ToBoolean(byte[] value, int startIndex)
		{
			return value[startIndex] != 0;
		}

		internal unsafe static char ToChar(byte[] value, int startIndex)
		{
			char result;
			BitConverterLE.UShortFromBytes((byte*)(&result), value, startIndex);
			return result;
		}

		internal unsafe static short ToInt16(byte[] value, int startIndex)
		{
			short result;
			BitConverterLE.UShortFromBytes((byte*)(&result), value, startIndex);
			return result;
		}

		internal unsafe static int ToInt32(byte[] value, int startIndex)
		{
			int result;
			BitConverterLE.UIntFromBytes((byte*)(&result), value, startIndex);
			return result;
		}

		internal unsafe static long ToInt64(byte[] value, int startIndex)
		{
			long result;
			BitConverterLE.ULongFromBytes((byte*)(&result), value, startIndex);
			return result;
		}

		internal unsafe static ushort ToUInt16(byte[] value, int startIndex)
		{
			ushort result;
			BitConverterLE.UShortFromBytes((byte*)(&result), value, startIndex);
			return result;
		}

		internal unsafe static uint ToUInt32(byte[] value, int startIndex)
		{
			uint result;
			BitConverterLE.UIntFromBytes((byte*)(&result), value, startIndex);
			return result;
		}

		internal unsafe static ulong ToUInt64(byte[] value, int startIndex)
		{
			ulong result;
			BitConverterLE.ULongFromBytes((byte*)(&result), value, startIndex);
			return result;
		}

		internal unsafe static float ToSingle(byte[] value, int startIndex)
		{
			float result;
			BitConverterLE.UIntFromBytes((byte*)(&result), value, startIndex);
			return result;
		}

		internal unsafe static double ToDouble(byte[] value, int startIndex)
		{
			double result;
			BitConverterLE.ULongFromBytes((byte*)(&result), value, startIndex);
			return result;
		}
	}
}
