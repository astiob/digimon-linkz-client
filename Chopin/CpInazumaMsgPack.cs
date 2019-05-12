using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace CROOZ.Chopin.Core
{
	public static class CpInazumaMsgPack
	{
		private static readonly Encoding \uE000 = new UTF8Encoding(false);

		private static readonly int \uE001 = 8;

		private static readonly int \uE002 = 64;

		private static readonly int \uE003 = 8192;

		private static readonly int \uE004 = 8;

		private static CpCustomDataPackMode \uE005 = CpCustomDataPackMode.Map;

		private static Stack<CpInazumaMsgPack.\uE019> \uE006 = null;

		private static \uE018 \uE007 = null;

		private static byte[] \uE008 = null;

		private static int \uE009 = 15;

		private static bool \uE00A = true;

		[CompilerGenerated]
		private static Comparison<\uE016> \uE00B;

		[CompilerGenerated]
		private static Comparison<\uE016> \uE00C;

		static CpInazumaMsgPack()
		{
			CpInazumaMsgPack.\uE008 = new byte[CpInazumaMsgPack.\uE001];
			Array.Clear(CpInazumaMsgPack.\uE008, 0, CpInazumaMsgPack.\uE001);
			CpInazumaMsgPack.\uE007 = new \uE018(CpInazumaMsgPack.\uE002);
			CpInazumaMsgPack.\uE006 = new Stack<CpInazumaMsgPack.\uE019>(CpInazumaMsgPack.\uE004);
		}

		public static CpCustomDataPackMode PackMode
		{
			get
			{
				return CpInazumaMsgPack.\uE005;
			}
			set
			{
				CpInazumaMsgPack.\uE005 = value;
			}
		}

		public static int DumpStringLength
		{
			get
			{
				return CpInazumaMsgPack.\uE009;
			}
			set
			{
				CpInazumaMsgPack.\uE009 = value;
			}
		}

		public static bool DumpStringEraseNewLine
		{
			get
			{
				return CpInazumaMsgPack.\uE00A;
			}
			set
			{
				CpInazumaMsgPack.\uE00A = value;
			}
		}

		public static void PushSetting()
		{
			CpInazumaMsgPack.\uE019 uE = new CpInazumaMsgPack.\uE019();
			uE.PackMode = CpInazumaMsgPack.\uE005;
			uE.DumpStringLength = CpInazumaMsgPack.\uE009;
			uE.DumpStringEraseNewLine = CpInazumaMsgPack.\uE00A;
			CpInazumaMsgPack.\uE006.Push(uE);
		}

		public static void PopSetting()
		{
			if (CpInazumaMsgPack.\uE006.Count == 0)
			{
				return;
			}
			CpInazumaMsgPack.\uE019 uE = CpInazumaMsgPack.\uE006.Pop();
			CpInazumaMsgPack.\uE005 = uE.PackMode;
			CpInazumaMsgPack.\uE009 = uE.DumpStringLength;
			CpInazumaMsgPack.\uE00A = uE.DumpStringEraseNewLine;
		}

		private static byte[] \uE000(byte[] \uE000)
		{
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(\uE000);
			}
			return \uE000;
		}

		private static long \uE001(Stream \uE000, int \uE001)
		{
			\uE001 = ((\uE001 <= CpInazumaMsgPack.\uE001) ? \uE001 : CpInazumaMsgPack.\uE001);
			byte[] array = new byte[\uE001];
			\uE000.Read(array, 0, \uE001);
			Array.Clear(CpInazumaMsgPack.\uE008, 0, CpInazumaMsgPack.\uE008.Length);
			CpInazumaMsgPack.\uE000(array).CopyTo(CpInazumaMsgPack.\uE008, 0);
			return BitConverter.ToInt64(CpInazumaMsgPack.\uE008, 0);
		}

		private static float \uE002(Stream \uE000)
		{
			byte[] array = new byte[4];
			\uE000.Read(array, 0, 4);
			return BitConverter.ToSingle(CpInazumaMsgPack.\uE000(array), 0);
		}

		private static double \uE003(Stream \uE000)
		{
			byte[] array = new byte[8];
			\uE000.Read(array, 0, 8);
			return BitConverter.ToDouble(CpInazumaMsgPack.\uE000(array), 0);
		}

		private static int \uE004(\uE015 \uE000, sbyte \uE001)
		{
			switch (\uE000)
			{
			case global::\uE015.\uE00E:
				return 1;
			case global::\uE015.\uE00F:
				return 2;
			case global::\uE015.\uE010:
				return 4;
			case global::\uE015.\uE011:
				return 1;
			case global::\uE015.\uE012:
				return 2;
			case global::\uE015.\uE013:
				return 4;
			case global::\uE015.\uE014:
				return 4;
			case global::\uE015.\uE015:
				return 8;
			case global::\uE015.\uE016:
				return 1;
			case global::\uE015.\uE017:
				return 2;
			case global::\uE015.\uE018:
				return 4;
			case global::\uE015.\uE019:
				return 8;
			case global::\uE015.\uE01A:
				return 1;
			case global::\uE015.\uE01B:
				return 2;
			case global::\uE015.\uE01C:
				return 4;
			case global::\uE015.\uE01D:
				return 8;
			case global::\uE015.\uE01E:
				return 1;
			case global::\uE015.\uE01F:
				return 2;
			case global::\uE015.\uE020:
				return 4;
			case global::\uE015.\uE021:
				return 2;
			case global::\uE015.\uE022:
				return 4;
			case global::\uE015.\uE023:
				return 2;
			case global::\uE015.\uE024:
				return 4;
			}
			return 0;
		}

		private static int \uE005(Stream \uE000, \uE015 \uE001, sbyte \uE002)
		{
			if (\uE001 == global::\uE015.\uE004)
			{
				return (int)\uE002;
			}
			if (\uE001 == global::\uE015.\uE002)
			{
				return (int)\uE002;
			}
			if (\uE001 == global::\uE015.\uE003)
			{
				return (int)\uE002;
			}
			if (!CpInazumaMsgPack.\uE00F(\uE001))
			{
				if (!CpInazumaMsgPack.\uE010(\uE001))
				{
					if (!CpInazumaMsgPack.\uE011(\uE001))
					{
						if (!CpInazumaMsgPack.\uE012(\uE001))
						{
							return 0;
						}
					}
				}
			}
			return (int)CpInazumaMsgPack.\uE001(\uE000, CpInazumaMsgPack.\uE004(\uE001, \uE002));
		}

		private static \uE015 \uE006(int \uE000)
		{
			if (\uE000 <= 15)
			{
				return global::\uE015.\uE002;
			}
			if (\uE000 <= 65535)
			{
				return global::\uE015.\uE023;
			}
			return global::\uE015.\uE024;
		}

		private static \uE015 \uE007(int \uE000)
		{
			if (\uE000 <= 15)
			{
				return global::\uE015.\uE003;
			}
			if (\uE000 <= 65535)
			{
				return global::\uE015.\uE021;
			}
			return global::\uE015.\uE022;
		}

		private static \uE015 \uE008(int \uE000)
		{
			if (\uE000 <= 31)
			{
				return global::\uE015.\uE004;
			}
			if (\uE000 <= 255)
			{
				return global::\uE015.\uE01E;
			}
			if (\uE000 <= 65535)
			{
				return global::\uE015.\uE01F;
			}
			return global::\uE015.\uE020;
		}

		private static \uE015 \uE009(int \uE000)
		{
			if (\uE000 <= 255)
			{
				return global::\uE015.\uE00E;
			}
			if (\uE000 <= 65535)
			{
				return global::\uE015.\uE00F;
			}
			return global::\uE015.\uE010;
		}

		private static \uE015 \uE00A(Stream \uE000, out sbyte \uE001)
		{
			\uE001 = 0;
			byte b = (byte)\uE000.ReadByte();
			if (b >> 5 == 6)
			{
				return (\uE015)b;
			}
			if (b >> 7 == 0)
			{
				\uE001 = (sbyte)b;
				return global::\uE015.\uE000;
			}
			if (b >> 5 == 7)
			{
				\uE001 = (sbyte)b;
				return global::\uE015.\uE001;
			}
			if (b >> 4 == 8)
			{
				\uE001 = (sbyte)(b & 15);
				return global::\uE015.\uE002;
			}
			if (b >> 4 == 9)
			{
				\uE001 = (sbyte)(b & 15);
				return global::\uE015.\uE003;
			}
			if (b >> 5 == 5)
			{
				\uE001 = (sbyte)(b & 31);
				return global::\uE015.\uE004;
			}
			return global::\uE015.\uE00B;
		}

		private static void \uE00B(object \uE000, \uE015 \uE001, Stream \uE002)
		{
			switch (\uE001)
			{
			case global::\uE015.\uE00A:
			case global::\uE015.\uE00C:
			case global::\uE015.\uE00D:
				\uE002.WriteByte((byte)\uE001);
				return;
			case global::\uE015.\uE00E:
			case global::\uE015.\uE011:
			case global::\uE015.\uE016:
			case global::\uE015.\uE005:
			case global::\uE015.\uE006:
			case global::\uE015.\uE007:
			case global::\uE015.\uE008:
			case global::\uE015.\uE009:
			case global::\uE015.\uE01E:
				\uE002.WriteByte((byte)\uE001);
				\uE002.WriteByte(Convert.ToByte(\uE000));
				return;
			case global::\uE015.\uE00F:
			case global::\uE015.\uE012:
			case global::\uE015.\uE017:
			case global::\uE015.\uE01F:
			case global::\uE015.\uE021:
			case global::\uE015.\uE023:
				\uE002.WriteByte((byte)\uE001);
				\uE002.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToUInt16(\uE000))), 0, 2);
				return;
			case global::\uE015.\uE010:
			case global::\uE015.\uE013:
			case global::\uE015.\uE018:
			case global::\uE015.\uE020:
			case global::\uE015.\uE022:
			case global::\uE015.\uE024:
				\uE002.WriteByte((byte)\uE001);
				\uE002.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToUInt32(\uE000))), 0, 4);
				return;
			case global::\uE015.\uE014:
				\uE002.WriteByte((byte)\uE001);
				\uE002.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToSingle(\uE000))), 0, 4);
				return;
			case global::\uE015.\uE015:
				\uE002.WriteByte((byte)\uE001);
				\uE002.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToDouble(\uE000))), 0, 8);
				return;
			case global::\uE015.\uE019:
				\uE002.WriteByte((byte)\uE001);
				\uE002.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToUInt64(\uE000))), 0, 8);
				return;
			case global::\uE015.\uE01A:
				\uE002.WriteByte((byte)\uE001);
				\uE002.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes((short)Convert.ToSByte(\uE000))), 0, 1);
				return;
			case global::\uE015.\uE01B:
				\uE002.WriteByte((byte)\uE001);
				\uE002.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToInt16(\uE000))), 0, 2);
				return;
			case global::\uE015.\uE01C:
				\uE002.WriteByte((byte)\uE001);
				\uE002.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToInt32(\uE000))), 0, 4);
				return;
			case global::\uE015.\uE01D:
				\uE002.WriteByte((byte)\uE001);
				\uE002.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToInt64(\uE000))), 0, 8);
				return;
			case global::\uE015.\uE001:
			{
				long num = Convert.ToInt64(\uE000);
				\uE002.WriteByte(Convert.ToByte(num & 255L));
				return;
			}
			}
			if (\uE001 == global::\uE015.\uE000)
			{
				ulong num2 = Convert.ToUInt64(\uE000);
				\uE002.WriteByte(Convert.ToByte(num2 & 255UL));
				return;
			}
			if (\uE001 == global::\uE015.\uE002 || \uE001 == global::\uE015.\uE003)
			{
				\uE002.WriteByte((byte)(\uE001 | (\uE015)(Convert.ToByte(\uE000) & 15)));
				return;
			}
			if (\uE001 != global::\uE015.\uE004)
			{
				return;
			}
			\uE002.WriteByte((byte)(\uE001 | (\uE015)(Convert.ToByte(\uE000) & 31)));
		}

		private static bool \uE00C(TypeCode \uE000)
		{
			if (\uE000 != TypeCode.SByte && \uE000 != TypeCode.Int16 && \uE000 != TypeCode.Int32 && \uE000 != TypeCode.Int64)
			{
				if (\uE000 != TypeCode.Byte)
				{
					if (\uE000 != TypeCode.UInt16)
					{
						if (\uE000 != TypeCode.UInt32)
						{
							return \uE000 == TypeCode.UInt64;
						}
					}
				}
			}
			return true;
		}

		private static bool \uE00D(TypeCode \uE000)
		{
			if (\uE000 != TypeCode.SByte && \uE000 != TypeCode.Int16)
			{
				if (\uE000 != TypeCode.Int32)
				{
					return \uE000 == TypeCode.Int64;
				}
			}
			return true;
		}

		private static bool \uE00E(\uE015 \uE000)
		{
			if (\uE000 != global::\uE015.\uE00D && \uE000 != global::\uE015.\uE00C && \uE000 != global::\uE015.\uE000 && \uE000 != global::\uE015.\uE001 && \uE000 != global::\uE015.\uE016 && \uE000 != global::\uE015.\uE017 && \uE000 != global::\uE015.\uE018 && \uE000 != global::\uE015.\uE019 && \uE000 != global::\uE015.\uE01A && \uE000 != global::\uE015.\uE01B)
			{
				if (\uE000 != global::\uE015.\uE01C)
				{
					if (\uE000 != global::\uE015.\uE01D)
					{
						if (\uE000 != global::\uE015.\uE014)
						{
							return \uE000 == global::\uE015.\uE015;
						}
					}
				}
			}
			return true;
		}

		private static bool \uE00F(\uE015 \uE000)
		{
			if (\uE000 != global::\uE015.\uE003)
			{
				if (\uE000 != global::\uE015.\uE021)
				{
					return \uE000 == global::\uE015.\uE022;
				}
			}
			return true;
		}

		private static bool \uE010(\uE015 \uE000)
		{
			if (\uE000 != global::\uE015.\uE002)
			{
				if (\uE000 != global::\uE015.\uE023)
				{
					return \uE000 == global::\uE015.\uE024;
				}
			}
			return true;
		}

		private static bool \uE011(\uE015 \uE000)
		{
			if (\uE000 != global::\uE015.\uE004 && \uE000 != global::\uE015.\uE01E)
			{
				if (\uE000 != global::\uE015.\uE01F)
				{
					return \uE000 == global::\uE015.\uE020;
				}
			}
			return true;
		}

		private static bool \uE012(\uE015 \uE000)
		{
			if (\uE000 != global::\uE015.\uE00E)
			{
				if (\uE000 != global::\uE015.\uE00F)
				{
					return \uE000 == global::\uE015.\uE010;
				}
			}
			return true;
		}

		private static IList \uE013(Type \uE000, int \uE001, out Type \uE002)
		{
			\uE002 = \uE000.GetElementType();
			if (\uE002 == null)
			{
				if (\uE000.IsGenericType)
				{
					\uE002 = \uE000.GetGenericArguments()[0];
				}
				if (\uE002 == null)
				{
					\uE002 = typeof(object);
				}
				Type type = typeof(List<>).MakeGenericType(new Type[]
				{
					\uE002
				});
				return Activator.CreateInstance(type, new object[]
				{
					Array.CreateInstance(\uE002, \uE001)
				}) as IList;
			}
			return Activator.CreateInstance(\uE000, new object[]
			{
				\uE001
			}) as IList;
		}

		private static object \uE014(object \uE000, TypeCode \uE001)
		{
			switch (\uE001)
			{
			case TypeCode.Boolean:
				return Convert.ToBoolean(\uE000);
			case TypeCode.Char:
				return Convert.ToChar(\uE000);
			case TypeCode.SByte:
				return Convert.ToSByte(\uE000);
			case TypeCode.Byte:
				return Convert.ToByte(\uE000);
			case TypeCode.Int16:
				return Convert.ToInt16(\uE000);
			case TypeCode.UInt16:
				return Convert.ToUInt16(\uE000);
			case TypeCode.Int32:
				return Convert.ToInt32(\uE000);
			case TypeCode.UInt32:
				return Convert.ToUInt32(\uE000);
			case TypeCode.Int64:
				return Convert.ToInt64(\uE000);
			case TypeCode.UInt64:
				return Convert.ToUInt64(\uE000);
			case TypeCode.Single:
				return Convert.ToSingle(\uE000);
			case TypeCode.Double:
				return Convert.ToDouble(\uE000);
			default:
				return \uE000;
			}
		}

		public static void Pack(object data, string file_path)
		{
			FileStream fileStream = new FileStream(file_path, FileMode.Create);
			CpInazumaMsgPack.Pack(data, fileStream);
			fileStream.Close();
		}

		public static void Pack(object data, Stream stream)
		{
			if (stream != null)
			{
				if (stream.CanWrite)
				{
					if (data == null)
					{
						CpInazumaMsgPack.\uE00B(null, global::\uE015.\uE00A, stream);
						return;
					}
					if (data.GetType().IsPrimitive)
					{
						CpInazumaMsgPack.\uE015(data, stream);
						return;
					}
					if (data is string)
					{
						CpInazumaMsgPack.\uE018((string)data, stream);
						return;
					}
					if (data is byte[])
					{
						CpInazumaMsgPack.\uE019((byte[])data, stream);
						return;
					}
					if (data is IList)
					{
						CpInazumaMsgPack.\uE016((IList)data, stream);
						return;
					}
					if (data is IDictionary)
					{
						CpInazumaMsgPack.\uE017((IDictionary)data, stream);
						return;
					}
					\uE017 uE = default(\uE017);
					if (CpInazumaMsgPack.\uE022(data.GetType(), ref uE))
					{
						CpInazumaMsgPack.\uE01A(data, ref uE, stream);
						return;
					}
					return;
				}
			}
			throw new IOException(global::\uE01B.\uE000(11664));
		}

		private static void \uE015(object \uE000, Stream \uE001)
		{
			TypeCode typeCode = Type.GetTypeCode(\uE000.GetType());
			if (CpInazumaMsgPack.\uE00D(typeCode) && Convert.ToInt64(\uE000) < 0L && Convert.ToInt64(\uE000) >= -32L)
			{
				CpInazumaMsgPack.\uE00B(\uE000, global::\uE015.\uE001, \uE001);
				return;
			}
			if (CpInazumaMsgPack.\uE00C(typeCode))
			{
				if (!CpInazumaMsgPack.\uE00D(typeCode))
				{
					if (Convert.ToUInt64(\uE000) <= 127UL)
					{
						CpInazumaMsgPack.\uE00B(\uE000, global::\uE015.\uE000, \uE001);
						return;
					}
				}
			}
			switch (typeCode)
			{
			case TypeCode.Boolean:
				CpInazumaMsgPack.\uE00B(null, (!(bool)\uE000) ? global::\uE015.\uE00C : global::\uE015.\uE00D, \uE001);
				return;
			case TypeCode.Char:
				CpInazumaMsgPack.\uE00B(\uE000, global::\uE015.\uE017, \uE001);
				return;
			case TypeCode.SByte:
				CpInazumaMsgPack.\uE00B(\uE000, global::\uE015.\uE01A, \uE001);
				return;
			case TypeCode.Byte:
				CpInazumaMsgPack.\uE00B(\uE000, global::\uE015.\uE016, \uE001);
				return;
			case TypeCode.Int16:
				CpInazumaMsgPack.\uE00B(\uE000, global::\uE015.\uE01B, \uE001);
				return;
			case TypeCode.UInt16:
				CpInazumaMsgPack.\uE00B(\uE000, global::\uE015.\uE017, \uE001);
				return;
			case TypeCode.Int32:
				CpInazumaMsgPack.\uE00B(\uE000, global::\uE015.\uE01C, \uE001);
				return;
			case TypeCode.UInt32:
				CpInazumaMsgPack.\uE00B(\uE000, global::\uE015.\uE018, \uE001);
				return;
			case TypeCode.Int64:
				CpInazumaMsgPack.\uE00B(\uE000, global::\uE015.\uE01D, \uE001);
				return;
			case TypeCode.UInt64:
				CpInazumaMsgPack.\uE00B(\uE000, global::\uE015.\uE019, \uE001);
				return;
			case TypeCode.Single:
				CpInazumaMsgPack.\uE00B(\uE000, global::\uE015.\uE014, \uE001);
				return;
			case TypeCode.Double:
				CpInazumaMsgPack.\uE00B(\uE000, global::\uE015.\uE015, \uE001);
				return;
			default:
				return;
			}
		}

		private static void \uE016(IList \uE000, Stream \uE001)
		{
			int count = \uE000.Count;
			if (CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Older)
			{
				\uE001.WriteByte(221);
				\uE001.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(count)), 0, 4);
			}
			else
			{
				CpInazumaMsgPack.\uE00B(\uE000.Count, CpInazumaMsgPack.\uE007(count), \uE001);
			}
			for (int i = 0; i < count; i++)
			{
				CpInazumaMsgPack.Pack(\uE000[i], \uE001);
			}
		}

		private static void \uE017(IDictionary \uE000, Stream \uE001)
		{
			int count = \uE000.Count;
			if (CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Older)
			{
				\uE001.WriteByte(223);
				\uE001.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(count)), 0, 4);
			}
			else
			{
				CpInazumaMsgPack.\uE00B(count, CpInazumaMsgPack.\uE006(count), \uE001);
			}
			IDictionaryEnumerator enumerator = \uE000.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					CpInazumaMsgPack.Pack(dictionaryEntry.Key, \uE001);
					CpInazumaMsgPack.Pack(dictionaryEntry.Value, \uE001);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		private static void \uE018(string \uE000, Stream \uE001)
		{
			byte[] bytes = CpInazumaMsgPack.\uE000.GetBytes(\uE000);
			int num = bytes.Length;
			if (CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Older)
			{
				\uE001.WriteByte(219);
				\uE001.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(num)), 0, 4);
			}
			else
			{
				CpInazumaMsgPack.\uE00B(num, CpInazumaMsgPack.\uE008(num), \uE001);
			}
			\uE001.Write(bytes, 0, bytes.Length);
		}

		private static void \uE019(byte[] \uE000, Stream \uE001)
		{
			int num = \uE000.Length;
			if (CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Older)
			{
				\uE001.WriteByte(219);
				\uE001.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(num)), 0, 4);
			}
			else
			{
				CpInazumaMsgPack.\uE00B(num, CpInazumaMsgPack.\uE009(num), \uE001);
			}
			\uE001.Write(\uE000, 0, \uE000.Length);
		}

		private static void \uE01A(object \uE000, ref \uE017 \uE001, Stream \uE002)
		{
			if (CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Array)
			{
				CpInazumaMsgPack.\uE00B(\uE001.FieldList.Count, CpInazumaMsgPack.\uE007(\uE001.FieldList.Count), \uE002);
			}
			if (CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Raw || CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Array)
			{
				for (int i = 0; i < \uE001.FieldList.Count; i++)
				{
					\uE016 uE = \uE001.FieldList[i];
					CpInazumaMsgPack.Pack(uE.FieldInfo.GetValue(\uE000), \uE002);
				}
				return;
			}
			if (CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Map || CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Older)
			{
				CpInazumaMsgPack.\uE00B(\uE001.FieldList.Count, CpInazumaMsgPack.\uE006(\uE001.FieldList.Count), \uE002);
				for (int j = 0; j < \uE001.FieldList.Count; j++)
				{
					\uE016 uE2 = \uE001.FieldList[j];
					CpInazumaMsgPack.\uE018(uE2.Name, \uE002);
					CpInazumaMsgPack.Pack(uE2.FieldInfo.GetValue(\uE000), \uE002);
				}
				return;
			}
		}

		public static T Unpack<T>(byte[] buffer)
		{
			return (T)((object)Convert.ChangeType(CpInazumaMsgPack.Unpack(buffer, typeof(T)), typeof(T)));
		}

		public static object Unpack(byte[] buffer, Type type)
		{
			if (buffer == null)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream(buffer);
			object result = CpInazumaMsgPack.Unpack(memoryStream, type);
			memoryStream.Close();
			return result;
		}

		public static T Unpack<T>(string file_path)
		{
			if (!File.Exists(file_path))
			{
				return default(T);
			}
			FileStream fileStream = new FileStream(file_path, FileMode.Open);
			T result = CpInazumaMsgPack.Unpack<T>(fileStream);
			fileStream.Close();
			return result;
		}

		public static T Unpack<T>(Stream stream)
		{
			Type typeFromHandle = typeof(T);
			return (T)((object)Convert.ChangeType(CpInazumaMsgPack.Unpack(stream, typeFromHandle), typeFromHandle));
		}

		public static object Unpack(Stream stream, Type type)
		{
			if (stream != null && stream.CanRead)
			{
				if (stream.CanSeek)
				{
					\uE017 uE = default(\uE017);
					if (CpInazumaMsgPack.\uE022(type, ref uE))
					{
						return CpInazumaMsgPack.\uE020(stream, ref uE);
					}
					sbyte uE2 = 0;
					\uE015 uE3 = CpInazumaMsgPack.\uE00A(stream, out uE2);
					if (uE3 == global::\uE015.\uE00B)
					{
						stream.Seek(-1L, SeekOrigin.Current);
						return null;
					}
					if (uE3 == global::\uE015.\uE00A)
					{
						return null;
					}
					if (CpInazumaMsgPack.\uE00E(uE3))
					{
						return CpInazumaMsgPack.\uE01B(stream, uE3, uE2);
					}
					if (CpInazumaMsgPack.\uE00F(uE3))
					{
						return CpInazumaMsgPack.\uE01C(stream, uE3, uE2, type);
					}
					if (CpInazumaMsgPack.\uE010(uE3))
					{
						return CpInazumaMsgPack.\uE01D(stream, uE3, uE2, type);
					}
					if (CpInazumaMsgPack.\uE011(uE3))
					{
						return CpInazumaMsgPack.\uE01E(stream, uE3, uE2);
					}
					if (CpInazumaMsgPack.\uE012(uE3))
					{
						return CpInazumaMsgPack.\uE01F(stream, uE3, uE2);
					}
					stream.Seek(-1L, SeekOrigin.Current);
					return null;
				}
			}
			throw new IOException(global::\uE01B.\uE000(11664));
		}

		private static object \uE01B(Stream \uE000, \uE015 \uE001, sbyte \uE002)
		{
			switch (\uE001)
			{
			case global::\uE015.\uE014:
				return CpInazumaMsgPack.\uE002(\uE000);
			case global::\uE015.\uE015:
				return CpInazumaMsgPack.\uE003(\uE000);
			case global::\uE015.\uE016:
				return (byte)\uE000.ReadByte();
			case global::\uE015.\uE017:
				return (ushort)CpInazumaMsgPack.\uE001(\uE000, 2);
			case global::\uE015.\uE018:
				return (uint)CpInazumaMsgPack.\uE001(\uE000, 4);
			case global::\uE015.\uE019:
				return (ulong)CpInazumaMsgPack.\uE001(\uE000, 8);
			case global::\uE015.\uE01A:
				return (sbyte)\uE000.ReadByte();
			case global::\uE015.\uE01B:
				return (short)CpInazumaMsgPack.\uE001(\uE000, 2);
			case global::\uE015.\uE01C:
				return (int)CpInazumaMsgPack.\uE001(\uE000, 4);
			case global::\uE015.\uE01D:
				return CpInazumaMsgPack.\uE001(\uE000, 8);
			default:
				if (\uE001 == global::\uE015.\uE00C)
				{
					return false;
				}
				if (\uE001 == global::\uE015.\uE00D)
				{
					return true;
				}
				if (\uE001 == global::\uE015.\uE000)
				{
					return (byte)\uE002;
				}
				if (\uE001 != global::\uE015.\uE001)
				{
					return null;
				}
				return \uE002;
			}
		}

		private static object \uE01C(Stream \uE000, \uE015 \uE001, sbyte \uE002, Type \uE003)
		{
			int num = CpInazumaMsgPack.\uE005(\uE000, \uE001, \uE002);
			Type type = null;
			IList list = CpInazumaMsgPack.\uE013(\uE003, num, out type);
			for (int i = 0; i < num; i++)
			{
				object value = CpInazumaMsgPack.Unpack(\uE000, type);
				try
				{
					list[i] = Convert.ChangeType(value, type);
				}
				catch (InvalidCastException)
				{
					list[i] = value;
				}
			}
			return list;
		}

		private static object \uE01D(Stream \uE000, \uE015 \uE001, sbyte \uE002, Type \uE003)
		{
			Type type = typeof(object);
			Type type2 = typeof(object);
			if (\uE003.IsGenericType)
			{
				Type[] genericArguments = \uE003.GetGenericArguments();
				if (genericArguments.Length >= 1)
				{
					type = genericArguments[0];
				}
				if (genericArguments.Length >= 2)
				{
					type2 = genericArguments[1];
				}
			}
			int num = CpInazumaMsgPack.\uE005(\uE000, \uE001, \uE002);
			IDictionary dictionary = Activator.CreateInstance(typeof(Dictionary<, >).MakeGenericType(new Type[]
			{
				type,
				type2
			})) as IDictionary;
			for (int i = 0; i < num; i++)
			{
				object key = CpInazumaMsgPack.Unpack(\uE000, type);
				object value = CpInazumaMsgPack.Unpack(\uE000, type2);
				dictionary.Add(key, value);
			}
			return dictionary;
		}

		private static string \uE01E(Stream \uE000, \uE015 \uE001, sbyte \uE002)
		{
			int num = CpInazumaMsgPack.\uE005(\uE000, \uE001, \uE002);
			byte[] array = new byte[num];
			\uE000.Read(array, 0, num);
			return CpInazumaMsgPack.\uE000.GetString(array);
		}

		private static byte[] \uE01F(Stream \uE000, \uE015 \uE001, sbyte \uE002)
		{
			int num = CpInazumaMsgPack.\uE005(\uE000, \uE001, \uE002);
			byte[] array = new byte[num];
			\uE000.Read(array, 0, num);
			return array;
		}

		private static object \uE020(Stream \uE000, ref \uE017 \uE001)
		{
			sbyte b = 0;
			\uE015 uE = CpInazumaMsgPack.\uE00A(\uE000, out b);
			object obj = Activator.CreateInstance(\uE001.DataType);
			if (CpInazumaMsgPack.\uE010(uE))
			{
				int num = CpInazumaMsgPack.\uE005(\uE000, uE, b);
				for (int i = 0; i < num; i++)
				{
					CpInazumaMsgPack.\uE01A uE01A = new CpInazumaMsgPack.\uE01A();
					uE = CpInazumaMsgPack.\uE00A(\uE000, out b);
					uE01A.\uE000 = CpInazumaMsgPack.\uE01E(\uE000, uE, b);
					\uE016 uE2 = \uE001.FieldList.Find(new Predicate<\uE016>(uE01A.\uE000));
					if (uE2 == null)
					{
						CpInazumaMsgPack.Unpack(\uE000, typeof(object));
					}
					else
					{
						TypeCode typeCode = Type.GetTypeCode(uE2.FieldInfo.FieldType);
						object value = CpInazumaMsgPack.\uE014(CpInazumaMsgPack.Unpack(\uE000, uE2.FieldInfo.FieldType), typeCode);
						uE2.FieldInfo.SetValue(obj, value);
					}
				}
				return obj;
			}
			if (CpInazumaMsgPack.\uE00F(uE))
			{
				\uE000.Seek((long)CpInazumaMsgPack.\uE004(uE, b), SeekOrigin.Current);
				return CpInazumaMsgPack.\uE020(\uE000, ref \uE001);
			}
			\uE000.Seek(-1L, SeekOrigin.Current);
			if (uE == global::\uE015.\uE00A)
			{
				return null;
			}
			for (int j = 0; j < \uE001.FieldList.Count; j++)
			{
				FieldInfo fieldInfo = \uE001.FieldList[j].FieldInfo;
				object obj2 = CpInazumaMsgPack.Unpack(\uE000, fieldInfo.FieldType);
				if (obj2 != null)
				{
					fieldInfo.SetValue(obj, Convert.ChangeType(obj2, fieldInfo.FieldType));
				}
			}
			return obj;
		}

		public static bool Analyze<T>()
		{
			\uE017 uE = default(\uE017);
			return CpInazumaMsgPack.\uE021<T>(ref uE);
		}

		private static bool \uE021<\uE000>(ref \uE017 \uE000)
		{
			return CpInazumaMsgPack.\uE022(typeof(\uE000), ref \uE000);
		}

		private static bool \uE022(Type \uE000, ref \uE017 \uE001)
		{
			if (CpInazumaMsgPack.\uE007.TryGetValue(\uE000, out \uE001))
			{
				return true;
			}
			if (\uE000.IsPrimitive || typeof(IList).IsAssignableFrom(\uE000) || typeof(IDictionary).IsAssignableFrom(\uE000) || \uE000 == typeof(object) || typeof(string) == \uE000)
			{
				return false;
			}
			BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			FieldInfo[] fields = \uE000.GetFields(bindingAttr);
			List<\uE016> list = new List<\uE016>(fields.Length);
			bool flag = false;
			foreach (FieldInfo fieldInfo in fields)
			{
				if (!fieldInfo.IsDefined(typeof(NonSerializedAttribute), false))
				{
					\uE016 uE = new \uE016();
					uE.Name = fieldInfo.Name;
					uE.Order = int.MaxValue;
					uE.FieldInfo = fieldInfo;
					object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(CpSerializeFieldAttribute), false);
					if (customAttributes.Length > 0 && customAttributes[0] is CpSerializeFieldAttribute)
					{
						flag = true;
						CpSerializeFieldAttribute cpSerializeFieldAttribute = customAttributes[0] as CpSerializeFieldAttribute;
						uE.Name = (cpSerializeFieldAttribute.Name ?? fieldInfo.Name);
						uE.Order = cpSerializeFieldAttribute.Order;
					}
					list.Add(uE);
				}
			}
			if (flag)
			{
				List<\uE016> list2 = list;
				if (CpInazumaMsgPack.\uE00B == null)
				{
					CpInazumaMsgPack.\uE00B = new Comparison<\uE016>(CpInazumaMsgPack.\uE024);
				}
				list2.Sort(CpInazumaMsgPack.\uE00B);
			}
			else
			{
				List<\uE016> list3 = list;
				if (CpInazumaMsgPack.\uE00C == null)
				{
					CpInazumaMsgPack.\uE00C = new Comparison<\uE016>(CpInazumaMsgPack.\uE025);
				}
				list3.Sort(CpInazumaMsgPack.\uE00C);
			}
			\uE017 uE2 = default(\uE017);
			uE2.DataType = \uE000;
			uE2.FieldList = list;
			CpInazumaMsgPack.\uE007[\uE000] = uE2;
			\uE017 uE3 = default(\uE017);
			for (int j = 0; j < list.Count; j++)
			{
				CpInazumaMsgPack.\uE022(list[j].FieldInfo.FieldType, ref uE3);
			}
			\uE001 = uE2;
			return true;
		}

		public static string Dump(string file_path)
		{
			if (!string.IsNullOrEmpty(file_path))
			{
				if (File.Exists(file_path))
				{
					FileStream fileStream = new FileStream(file_path, FileMode.Open);
					string result = CpInazumaMsgPack.Dump(fileStream);
					fileStream.Close();
					return result;
				}
			}
			return string.Empty;
		}

		public static string Dump(byte[] buffer)
		{
			if (buffer == null)
			{
				return string.Empty;
			}
			MemoryStream memoryStream = new MemoryStream(buffer);
			string result = CpInazumaMsgPack.Dump(memoryStream);
			memoryStream.Close();
			return result;
		}

		public static string Dump(Stream stream)
		{
			if (stream != null && stream.CanRead)
			{
				if (stream.CanSeek)
				{
					StringBuilder stringBuilder = new StringBuilder(CpInazumaMsgPack.\uE003);
					int num = 0;
					stringBuilder.Append(global::\uE01B.\uE000(11756));
					long position = stream.Position;
					while (stream.Position < stream.Length)
					{
						long position2 = stream.Position;
						sbyte uE = 0;
						\uE015 uE2 = CpInazumaMsgPack.\uE00A(stream, out uE);
						string arg = CpInazumaMsgPack.\uE023(stream, uE2, uE);
						num++;
						stringBuilder.AppendFormat(global::\uE01B.\uE000(11733), global::\uE01B.\uE000(11739), uE2);
						stringBuilder.AppendFormat(global::\uE01B.\uE000(11733), global::\uE01B.\uE000(11554), arg);
						stringBuilder.AppendFormat(global::\uE01B.\uE000(11562), global::\uE01B.\uE000(11520), position2);
						stringBuilder.AppendFormat(global::\uE01B.\uE000(11562), global::\uE01B.\uE000(11535), stream.Position - position2);
						stringBuilder.Append(global::\uE01B.\uE000(11542));
					}
					stringBuilder.AppendFormat(global::\uE01B.\uE000(11647), global::\uE01B.\uE000(11605), stream.Length);
					stringBuilder.AppendFormat(global::\uE01B.\uE000(11615), global::\uE01B.\uE000(11944), num);
					stringBuilder.AppendFormat(global::\uE01B.\uE000(11955), global::\uE01B.\uE000(11913), (double)stream.Length / (double)num);
					stringBuilder.Append(global::\uE01B.\uE000(12005));
					stream.Seek(position, SeekOrigin.Begin);
					return stringBuilder.ToString();
				}
			}
			return string.Empty;
		}

		private static string \uE023(Stream \uE000, \uE015 \uE001, sbyte \uE002)
		{
			if (\uE001 == global::\uE015.\uE00B)
			{
				return global::\uE01B.\uE000(11982);
			}
			if (\uE001 == global::\uE015.\uE00A)
			{
				return global::\uE01B.\uE000(11984);
			}
			if (CpInazumaMsgPack.\uE00E(\uE001))
			{
				return CpInazumaMsgPack.\uE01B(\uE000, \uE001, \uE002).ToString();
			}
			if (CpInazumaMsgPack.\uE00F(\uE001))
			{
				int num = CpInazumaMsgPack.\uE005(\uE000, \uE001, \uE002);
				return string.Format(global::\uE01B.\uE000(11997), num);
			}
			if (CpInazumaMsgPack.\uE010(\uE001))
			{
				int num2 = CpInazumaMsgPack.\uE005(\uE000, \uE001, \uE002);
				return string.Format(global::\uE01B.\uE000(11823), num2);
			}
			if (CpInazumaMsgPack.\uE011(\uE001))
			{
				string text = CpInazumaMsgPack.\uE01E(\uE000, \uE001, \uE002);
				if (CpInazumaMsgPack.\uE00A)
				{
					text = text.Replace(global::\uE01B.\uE000(11838), string.Empty).Replace(global::\uE01B.\uE000(11832), string.Empty);
				}
				return string.Format(global::\uE01B.\uE000(11834), text.Substring(0, Math.Min(text.Length, CpInazumaMsgPack.\uE009)));
			}
			if (CpInazumaMsgPack.\uE012(\uE001))
			{
				return string.Format(global::\uE01B.\uE000(11776), CpInazumaMsgPack.\uE01F(\uE000, \uE001, \uE002).Length);
			}
			return string.Empty;
		}

		[CompilerGenerated]
		private static int \uE024(\uE016 \uE000, \uE016 \uE001)
		{
			return \uE000.Order.CompareTo(\uE001.Order);
		}

		[CompilerGenerated]
		private static int \uE025(\uE016 \uE000, \uE016 \uE001)
		{
			return StringComparer.Ordinal.Compare(\uE000.Name, \uE001.Name);
		}

		private class \uE019
		{
			public CpCustomDataPackMode PackMode = CpCustomDataPackMode.Map;

			public int DumpStringLength = 15;

			public bool DumpStringEraseNewLine = true;
		}

		[CompilerGenerated]
		private sealed class \uE01A
		{
			internal string \uE000;

			internal bool \uE000(\uE016 \uE000)
			{
				return \uE000.Name == this.\uE000;
			}
		}
	}
}
