using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	internal class BinaryCommon
	{
		public static byte[] BinaryHeader = new byte[]
		{
			0,
			1,
			0,
			0,
			0,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};

		private static Type[] _typeCodesToType;

		private static byte[] _typeCodeMap;

		public static bool UseReflectionSerialization = false;

		static BinaryCommon()
		{
			BinaryCommon._typeCodesToType = new Type[19];
			BinaryCommon._typeCodesToType[1] = typeof(bool);
			BinaryCommon._typeCodesToType[2] = typeof(byte);
			BinaryCommon._typeCodesToType[3] = typeof(char);
			BinaryCommon._typeCodesToType[12] = typeof(TimeSpan);
			BinaryCommon._typeCodesToType[13] = typeof(DateTime);
			BinaryCommon._typeCodesToType[5] = typeof(decimal);
			BinaryCommon._typeCodesToType[6] = typeof(double);
			BinaryCommon._typeCodesToType[7] = typeof(short);
			BinaryCommon._typeCodesToType[8] = typeof(int);
			BinaryCommon._typeCodesToType[9] = typeof(long);
			BinaryCommon._typeCodesToType[10] = typeof(sbyte);
			BinaryCommon._typeCodesToType[11] = typeof(float);
			BinaryCommon._typeCodesToType[14] = typeof(ushort);
			BinaryCommon._typeCodesToType[15] = typeof(uint);
			BinaryCommon._typeCodesToType[16] = typeof(ulong);
			BinaryCommon._typeCodesToType[17] = null;
			BinaryCommon._typeCodesToType[18] = typeof(string);
			BinaryCommon._typeCodeMap = new byte[30];
			BinaryCommon._typeCodeMap[3] = 1;
			BinaryCommon._typeCodeMap[6] = 2;
			BinaryCommon._typeCodeMap[4] = 3;
			BinaryCommon._typeCodeMap[16] = 13;
			BinaryCommon._typeCodeMap[15] = 5;
			BinaryCommon._typeCodeMap[14] = 6;
			BinaryCommon._typeCodeMap[7] = 7;
			BinaryCommon._typeCodeMap[9] = 8;
			BinaryCommon._typeCodeMap[11] = 9;
			BinaryCommon._typeCodeMap[5] = 10;
			BinaryCommon._typeCodeMap[13] = 11;
			BinaryCommon._typeCodeMap[8] = 14;
			BinaryCommon._typeCodeMap[10] = 15;
			BinaryCommon._typeCodeMap[12] = 16;
			BinaryCommon._typeCodeMap[18] = 18;
			string text = Environment.GetEnvironmentVariable("MONO_REFLECTION_SERIALIZER");
			if (text == null)
			{
				text = "no";
			}
			BinaryCommon.UseReflectionSerialization = (text != "no");
		}

		public static bool IsPrimitive(Type type)
		{
			return (type.IsPrimitive && type != typeof(IntPtr)) || type == typeof(DateTime) || type == typeof(TimeSpan) || type == typeof(decimal);
		}

		public static byte GetTypeCode(Type type)
		{
			if (type == typeof(TimeSpan))
			{
				return 12;
			}
			return BinaryCommon._typeCodeMap[(int)Type.GetTypeCode(type)];
		}

		public static Type GetTypeFromCode(int code)
		{
			return BinaryCommon._typeCodesToType[code];
		}

		public static void CheckSerializable(Type type, ISurrogateSelector selector, StreamingContext context)
		{
			if (type.IsSerializable || type.IsInterface)
			{
				return;
			}
			if (selector != null && selector.GetSurrogate(type, context, out selector) != null)
			{
				return;
			}
			throw new SerializationException("Type " + type + " is not marked as Serializable.");
		}

		public static void SwapBytes(byte[] byteArray, int size, int dataSize)
		{
			if (dataSize == 8)
			{
				for (int i = 0; i < size; i += 8)
				{
					byte b = byteArray[i];
					byteArray[i] = byteArray[i + 7];
					byteArray[i + 7] = b;
					b = byteArray[i + 1];
					byteArray[i + 1] = byteArray[i + 6];
					byteArray[i + 6] = b;
					b = byteArray[i + 2];
					byteArray[i + 2] = byteArray[i + 5];
					byteArray[i + 5] = b;
					b = byteArray[i + 3];
					byteArray[i + 3] = byteArray[i + 4];
					byteArray[i + 4] = b;
				}
			}
			else if (dataSize == 4)
			{
				for (int j = 0; j < size; j += 4)
				{
					byte b = byteArray[j];
					byteArray[j] = byteArray[j + 3];
					byteArray[j + 3] = b;
					b = byteArray[j + 1];
					byteArray[j + 1] = byteArray[j + 2];
					byteArray[j + 2] = b;
				}
			}
			else if (dataSize == 2)
			{
				for (int k = 0; k < size; k += 2)
				{
					byte b = byteArray[k];
					byteArray[k] = byteArray[k + 1];
					byteArray[k + 1] = b;
				}
			}
		}
	}
}
