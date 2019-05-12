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

		private static Stack<CpInazumaMsgPack.\uE000> \uE006 = null;

		private static \uE018 \uE007 = null;

		private static byte[] \uE008 = null;

		private static int \uE009 = 15;

		private static bool \uE00A = true;

		[CompilerGenerated]
		private static Comparison<\uE016> \uE00B;

		[CompilerGenerated]
		private static Comparison<\uE016> \uE00C;

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

		static CpInazumaMsgPack()
		{
			CpInazumaMsgPack.\uE008 = new byte[CpInazumaMsgPack.\uE001];
			Array.Clear(CpInazumaMsgPack.\uE008, 0, CpInazumaMsgPack.\uE001);
			CpInazumaMsgPack.\uE007 = new \uE018(CpInazumaMsgPack.\uE002);
			CpInazumaMsgPack.\uE006 = new Stack<CpInazumaMsgPack.\uE000>(CpInazumaMsgPack.\uE004);
		}

		public static void PushSetting()
		{
			CpInazumaMsgPack.\uE000 uE = new CpInazumaMsgPack.\uE000();
			uE.\uE000 = CpInazumaMsgPack.\uE005;
			uE.\uE001 = CpInazumaMsgPack.\uE009;
			uE.\uE002 = CpInazumaMsgPack.\uE00A;
			CpInazumaMsgPack.\uE006.Push(uE);
		}

		public static void PopSetting()
		{
			if (CpInazumaMsgPack.\uE006.Count == 0)
			{
				return;
			}
			CpInazumaMsgPack.\uE000 uE = CpInazumaMsgPack.\uE006.Pop();
			CpInazumaMsgPack.\uE005 = uE.\uE000;
			CpInazumaMsgPack.\uE009 = uE.\uE001;
			CpInazumaMsgPack.\uE00A = uE.\uE002;
		}

		private static byte[] \uE000(byte[] \uE020)
		{
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(\uE020);
			}
			return \uE020;
		}

		private static long \uE001(Stream \uE021, int \uE022)
		{
			\uE022 = ((\uE022 > CpInazumaMsgPack.\uE001) ? CpInazumaMsgPack.\uE001 : \uE022);
			byte[] array = new byte[\uE022];
			\uE021.Read(array, 0, \uE022);
			Array.Clear(CpInazumaMsgPack.\uE008, 0, CpInazumaMsgPack.\uE008.Length);
			CpInazumaMsgPack.\uE000(array).CopyTo(CpInazumaMsgPack.\uE008, 0);
			return BitConverter.ToInt64(CpInazumaMsgPack.\uE008, 0);
		}

		private static float \uE002(Stream \uE023)
		{
			byte[] array = new byte[4];
			\uE023.Read(array, 0, 4);
			return BitConverter.ToSingle(CpInazumaMsgPack.\uE000(array), 0);
		}

		private static double \uE003(Stream \uE024)
		{
			byte[] array = new byte[8];
			\uE024.Read(array, 0, 8);
			return BitConverter.ToDouble(CpInazumaMsgPack.\uE000(array), 0);
		}

		private static int \uE004(\uE015 \uE025, sbyte \uE026)
		{
			switch (\uE025)
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

		private static int \uE005(Stream \uE027, \uE015 \uE028, sbyte \uE029)
		{
			if (\uE028 == global::\uE015.\uE004)
			{
				return (int)\uE029;
			}
			if (\uE028 == global::\uE015.\uE002)
			{
				return (int)\uE029;
			}
			if (\uE028 == global::\uE015.\uE003)
			{
				return (int)\uE029;
			}
			if (!CpInazumaMsgPack.\uE00F(\uE028))
			{
				if (!CpInazumaMsgPack.\uE010(\uE028))
				{
					if (!CpInazumaMsgPack.\uE011(\uE028))
					{
						if (!CpInazumaMsgPack.\uE012(\uE028))
						{
							return 0;
						}
					}
				}
			}
			return (int)CpInazumaMsgPack.\uE001(\uE027, CpInazumaMsgPack.\uE004(\uE028, \uE029));
		}

		private static \uE015 \uE006(int \uE02A)
		{
			if (\uE02A <= 15)
			{
				return global::\uE015.\uE002;
			}
			if (\uE02A <= 65535)
			{
				return global::\uE015.\uE023;
			}
			return global::\uE015.\uE024;
		}

		private static \uE015 \uE007(int \uE02B)
		{
			if (\uE02B <= 15)
			{
				return global::\uE015.\uE003;
			}
			if (\uE02B <= 65535)
			{
				return global::\uE015.\uE021;
			}
			return global::\uE015.\uE022;
		}

		private static \uE015 \uE008(int \uE02C)
		{
			if (\uE02C <= 31)
			{
				return global::\uE015.\uE004;
			}
			if (\uE02C <= 255)
			{
				return global::\uE015.\uE01E;
			}
			if (\uE02C <= 65535)
			{
				return global::\uE015.\uE01F;
			}
			return global::\uE015.\uE020;
		}

		private static \uE015 \uE009(int \uE02D)
		{
			if (\uE02D <= 255)
			{
				return global::\uE015.\uE00E;
			}
			if (\uE02D <= 65535)
			{
				return global::\uE015.\uE00F;
			}
			return global::\uE015.\uE010;
		}

		private static \uE015 \uE00A(Stream \uE02E, out sbyte \uE02F)
		{
			\uE02F = 0;
			byte b = (byte)\uE02E.ReadByte();
			if (b >> 5 == 6)
			{
				return (\uE015)b;
			}
			if (b >> 7 == 0)
			{
				\uE02F = (sbyte)b;
				return global::\uE015.\uE000;
			}
			if (b >> 5 == 7)
			{
				\uE02F = (sbyte)b;
				return global::\uE015.\uE001;
			}
			if (b >> 4 == 8)
			{
				\uE02F = (sbyte)(b & 15);
				return global::\uE015.\uE002;
			}
			if (b >> 4 == 9)
			{
				\uE02F = (sbyte)(b & 15);
				return global::\uE015.\uE003;
			}
			if (b >> 5 == 5)
			{
				\uE02F = (sbyte)(b & 31);
				return global::\uE015.\uE004;
			}
			return global::\uE015.\uE00B;
		}

		private static void \uE00B(object \uE030, \uE015 \uE031, Stream \uE032)
		{
			if (\uE031 <= global::\uE015.\uE002)
			{
				if (\uE031 == global::\uE015.\uE000)
				{
					ulong num = Convert.ToUInt64(\uE030);
					\uE032.WriteByte(Convert.ToByte(num & 255UL));
					return;
				}
				if (\uE031 != global::\uE015.\uE002)
				{
					return;
				}
			}
			else if (\uE031 != global::\uE015.\uE003)
			{
				switch (\uE031)
				{
				case global::\uE015.\uE004:
					\uE032.WriteByte((byte)(\uE031 | (\uE015)(Convert.ToByte(\uE030) & 31)));
					return;
				case (\uE015)161:
				case (\uE015)162:
				case (\uE015)163:
				case (\uE015)164:
				case (\uE015)165:
				case (\uE015)166:
				case (\uE015)167:
				case (\uE015)168:
				case (\uE015)169:
				case (\uE015)170:
				case (\uE015)171:
				case (\uE015)172:
				case (\uE015)173:
				case (\uE015)174:
				case (\uE015)175:
				case (\uE015)176:
				case (\uE015)177:
				case (\uE015)178:
				case (\uE015)179:
				case (\uE015)180:
				case (\uE015)181:
				case (\uE015)182:
				case (\uE015)183:
				case (\uE015)184:
				case (\uE015)185:
				case (\uE015)186:
				case (\uE015)187:
				case (\uE015)188:
				case (\uE015)189:
				case (\uE015)190:
				case (\uE015)191:
				case global::\uE015.\uE00B:
					break;
				case global::\uE015.\uE00A:
				case global::\uE015.\uE00C:
				case global::\uE015.\uE00D:
					\uE032.WriteByte((byte)\uE031);
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
					\uE032.WriteByte((byte)\uE031);
					\uE032.WriteByte(Convert.ToByte(\uE030));
					return;
				case global::\uE015.\uE00F:
				case global::\uE015.\uE012:
				case global::\uE015.\uE017:
				case global::\uE015.\uE01F:
				case global::\uE015.\uE021:
				case global::\uE015.\uE023:
					\uE032.WriteByte((byte)\uE031);
					\uE032.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToUInt16(\uE030))), 0, 2);
					return;
				case global::\uE015.\uE010:
				case global::\uE015.\uE013:
				case global::\uE015.\uE018:
				case global::\uE015.\uE020:
				case global::\uE015.\uE022:
				case global::\uE015.\uE024:
					\uE032.WriteByte((byte)\uE031);
					\uE032.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToUInt32(\uE030))), 0, 4);
					return;
				case global::\uE015.\uE014:
					\uE032.WriteByte((byte)\uE031);
					\uE032.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToSingle(\uE030))), 0, 4);
					return;
				case global::\uE015.\uE015:
					\uE032.WriteByte((byte)\uE031);
					\uE032.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToDouble(\uE030))), 0, 8);
					return;
				case global::\uE015.\uE019:
					\uE032.WriteByte((byte)\uE031);
					\uE032.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToUInt64(\uE030))), 0, 8);
					return;
				case global::\uE015.\uE01A:
					\uE032.WriteByte((byte)\uE031);
					\uE032.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes((short)Convert.ToSByte(\uE030))), 0, 1);
					return;
				case global::\uE015.\uE01B:
					\uE032.WriteByte((byte)\uE031);
					\uE032.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToInt16(\uE030))), 0, 2);
					return;
				case global::\uE015.\uE01C:
					\uE032.WriteByte((byte)\uE031);
					\uE032.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToInt32(\uE030))), 0, 4);
					return;
				case global::\uE015.\uE01D:
					\uE032.WriteByte((byte)\uE031);
					\uE032.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(Convert.ToInt64(\uE030))), 0, 8);
					break;
				case global::\uE015.\uE001:
				{
					long num2 = Convert.ToInt64(\uE030);
					\uE032.WriteByte(Convert.ToByte(num2 & 255L));
					return;
				}
				default:
					return;
				}
				return;
			}
			\uE032.WriteByte((byte)(\uE031 | (\uE015)(Convert.ToByte(\uE030) & 15)));
		}

		private static bool \uE00C(TypeCode \uE033)
		{
			if (\uE033 != TypeCode.SByte && \uE033 != TypeCode.Int16 && \uE033 != TypeCode.Int32 && \uE033 != TypeCode.Int64)
			{
				if (\uE033 != TypeCode.Byte)
				{
					if (\uE033 != TypeCode.UInt16)
					{
						if (\uE033 != TypeCode.UInt32)
						{
							return \uE033 == TypeCode.UInt64;
						}
					}
				}
			}
			return true;
		}

		private static bool \uE00D(TypeCode \uE034)
		{
			if (\uE034 != TypeCode.SByte && \uE034 != TypeCode.Int16)
			{
				if (\uE034 != TypeCode.Int32)
				{
					return \uE034 == TypeCode.Int64;
				}
			}
			return true;
		}

		private static bool \uE00E(\uE015 \uE035)
		{
			if (\uE035 != global::\uE015.\uE00D && \uE035 != global::\uE015.\uE00C && \uE035 != global::\uE015.\uE000 && \uE035 != global::\uE015.\uE001 && \uE035 != global::\uE015.\uE016 && \uE035 != global::\uE015.\uE017 && \uE035 != global::\uE015.\uE018 && \uE035 != global::\uE015.\uE019 && \uE035 != global::\uE015.\uE01A && \uE035 != global::\uE015.\uE01B)
			{
				if (\uE035 != global::\uE015.\uE01C)
				{
					if (\uE035 != global::\uE015.\uE01D)
					{
						if (\uE035 != global::\uE015.\uE014)
						{
							return \uE035 == global::\uE015.\uE015;
						}
					}
				}
			}
			return true;
		}

		private static bool \uE00F(\uE015 \uE036)
		{
			if (\uE036 != global::\uE015.\uE003)
			{
				if (\uE036 != global::\uE015.\uE021)
				{
					return \uE036 == global::\uE015.\uE022;
				}
			}
			return true;
		}

		private static bool \uE010(\uE015 \uE037)
		{
			if (\uE037 != global::\uE015.\uE002)
			{
				if (\uE037 != global::\uE015.\uE023)
				{
					return \uE037 == global::\uE015.\uE024;
				}
			}
			return true;
		}

		private static bool \uE011(\uE015 \uE038)
		{
			if (\uE038 != global::\uE015.\uE004 && \uE038 != global::\uE015.\uE01E)
			{
				if (\uE038 != global::\uE015.\uE01F)
				{
					return \uE038 == global::\uE015.\uE020;
				}
			}
			return true;
		}

		private static bool \uE012(\uE015 \uE039)
		{
			if (\uE039 != global::\uE015.\uE00E)
			{
				if (\uE039 != global::\uE015.\uE00F)
				{
					return \uE039 == global::\uE015.\uE010;
				}
			}
			return true;
		}

		private static IList \uE013(Type \uE03A, int \uE03B, out Type \uE03C)
		{
			\uE03C = \uE03A.GetElementType();
			if (\uE03C == null)
			{
				if (\uE03A.IsGenericType)
				{
					\uE03C = \uE03A.GetGenericArguments()[0];
				}
				if (\uE03C == null)
				{
					\uE03C = typeof(object);
				}
				Type type = typeof(List<>).MakeGenericType(new Type[]
				{
					\uE03C
				});
				return Activator.CreateInstance(type, new object[]
				{
					Array.CreateInstance(\uE03C, \uE03B)
				}) as IList;
			}
			return Activator.CreateInstance(\uE03A, new object[]
			{
				\uE03B
			}) as IList;
		}

		private static object \uE014(object \uE03D, TypeCode \uE03E)
		{
			switch (\uE03E)
			{
			case TypeCode.Boolean:
				return Convert.ToBoolean(\uE03D);
			case TypeCode.Char:
				return Convert.ToChar(\uE03D);
			case TypeCode.SByte:
				return Convert.ToSByte(\uE03D);
			case TypeCode.Byte:
				return Convert.ToByte(\uE03D);
			case TypeCode.Int16:
				return Convert.ToInt16(\uE03D);
			case TypeCode.UInt16:
				return Convert.ToUInt16(\uE03D);
			case TypeCode.Int32:
				return Convert.ToInt32(\uE03D);
			case TypeCode.UInt32:
				return Convert.ToUInt32(\uE03D);
			case TypeCode.Int64:
				return Convert.ToInt64(\uE03D);
			case TypeCode.UInt64:
				return Convert.ToUInt64(\uE03D);
			case TypeCode.Single:
				return Convert.ToSingle(\uE03D);
			case TypeCode.Double:
				return Convert.ToDouble(\uE03D);
			default:
				return \uE03D;
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
					}
					return;
				}
			}
			throw new IOException(global::\uE019.\uE000(11502));
		}

		private static void \uE015(object \uE03F, Stream \uE040)
		{
			TypeCode typeCode = Type.GetTypeCode(\uE03F.GetType());
			if (CpInazumaMsgPack.\uE00D(typeCode) && Convert.ToInt64(\uE03F) < 0L && Convert.ToInt64(\uE03F) >= -32L)
			{
				CpInazumaMsgPack.\uE00B(\uE03F, global::\uE015.\uE001, \uE040);
				return;
			}
			if (CpInazumaMsgPack.\uE00C(typeCode))
			{
				if (!CpInazumaMsgPack.\uE00D(typeCode))
				{
					if (Convert.ToUInt64(\uE03F) <= 127UL)
					{
						CpInazumaMsgPack.\uE00B(\uE03F, global::\uE015.\uE000, \uE040);
						return;
					}
				}
			}
			switch (typeCode)
			{
			case TypeCode.Boolean:
				CpInazumaMsgPack.\uE00B(null, ((bool)\uE03F) ? global::\uE015.\uE00D : global::\uE015.\uE00C, \uE040);
				return;
			case TypeCode.Char:
				CpInazumaMsgPack.\uE00B(\uE03F, global::\uE015.\uE017, \uE040);
				return;
			case TypeCode.SByte:
				CpInazumaMsgPack.\uE00B(\uE03F, global::\uE015.\uE01A, \uE040);
				return;
			case TypeCode.Byte:
				CpInazumaMsgPack.\uE00B(\uE03F, global::\uE015.\uE016, \uE040);
				return;
			case TypeCode.Int16:
				CpInazumaMsgPack.\uE00B(\uE03F, global::\uE015.\uE01B, \uE040);
				return;
			case TypeCode.UInt16:
				CpInazumaMsgPack.\uE00B(\uE03F, global::\uE015.\uE017, \uE040);
				return;
			case TypeCode.Int32:
				CpInazumaMsgPack.\uE00B(\uE03F, global::\uE015.\uE01C, \uE040);
				return;
			case TypeCode.UInt32:
				CpInazumaMsgPack.\uE00B(\uE03F, global::\uE015.\uE018, \uE040);
				return;
			case TypeCode.Int64:
				CpInazumaMsgPack.\uE00B(\uE03F, global::\uE015.\uE01D, \uE040);
				return;
			case TypeCode.UInt64:
				CpInazumaMsgPack.\uE00B(\uE03F, global::\uE015.\uE019, \uE040);
				return;
			case TypeCode.Single:
				CpInazumaMsgPack.\uE00B(\uE03F, global::\uE015.\uE014, \uE040);
				return;
			case TypeCode.Double:
				CpInazumaMsgPack.\uE00B(\uE03F, global::\uE015.\uE015, \uE040);
				return;
			default:
				return;
			}
		}

		private static void \uE016(IList \uE041, Stream \uE042)
		{
			int count = \uE041.Count;
			if (CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Older)
			{
				\uE042.WriteByte(221);
				\uE042.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(count)), 0, 4);
			}
			else
			{
				CpInazumaMsgPack.\uE00B(\uE041.Count, CpInazumaMsgPack.\uE007(count), \uE042);
			}
			for (int i = 0; i < count; i++)
			{
				CpInazumaMsgPack.Pack(\uE041[i], \uE042);
			}
		}

		private static void \uE017(IDictionary \uE043, Stream \uE044)
		{
			int count = \uE043.Count;
			if (CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Older)
			{
				\uE044.WriteByte(223);
				\uE044.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(count)), 0, 4);
			}
			else
			{
				CpInazumaMsgPack.\uE00B(count, CpInazumaMsgPack.\uE006(count), \uE044);
			}
			foreach (object obj in \uE043)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				CpInazumaMsgPack.Pack(dictionaryEntry.Key, \uE044);
				CpInazumaMsgPack.Pack(dictionaryEntry.Value, \uE044);
			}
		}

		private static void \uE018(string \uE045, Stream \uE046)
		{
			byte[] bytes = CpInazumaMsgPack.\uE000.GetBytes(\uE045);
			int num = bytes.Length;
			if (CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Older)
			{
				\uE046.WriteByte(219);
				\uE046.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(num)), 0, 4);
			}
			else
			{
				CpInazumaMsgPack.\uE00B(num, CpInazumaMsgPack.\uE008(num), \uE046);
			}
			\uE046.Write(bytes, 0, bytes.Length);
		}

		private static void \uE019(byte[] \uE047, Stream \uE048)
		{
			int num = \uE047.Length;
			if (CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Older)
			{
				\uE048.WriteByte(219);
				\uE048.Write(CpInazumaMsgPack.\uE000(BitConverter.GetBytes(num)), 0, 4);
			}
			else
			{
				CpInazumaMsgPack.\uE00B(num, CpInazumaMsgPack.\uE009(num), \uE048);
			}
			\uE048.Write(\uE047, 0, \uE047.Length);
		}

		private static void \uE01A(object \uE049, ref \uE017 \uE04A, Stream \uE04B)
		{
			if (CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Array)
			{
				CpInazumaMsgPack.\uE00B(\uE04A.\uE001.Count, CpInazumaMsgPack.\uE007(\uE04A.\uE001.Count), \uE04B);
			}
			if (CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Raw || CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Array)
			{
				for (int i = 0; i < \uE04A.\uE001.Count; i++)
				{
					\uE016 uE = \uE04A.\uE001[i];
					CpInazumaMsgPack.Pack(uE.\uE002.GetValue(\uE049), \uE04B);
				}
				return;
			}
			if (CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Map || CpInazumaMsgPack.\uE005 == CpCustomDataPackMode.Older)
			{
				CpInazumaMsgPack.\uE00B(\uE04A.\uE001.Count, CpInazumaMsgPack.\uE006(\uE04A.\uE001.Count), \uE04B);
				for (int j = 0; j < \uE04A.\uE001.Count; j++)
				{
					\uE016 uE2 = \uE04A.\uE001[j];
					CpInazumaMsgPack.\uE018(uE2.\uE001, \uE04B);
					CpInazumaMsgPack.Pack(uE2.\uE002.GetValue(\uE049), \uE04B);
				}
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
					sbyte b = 0;
					\uE015 uE2 = CpInazumaMsgPack.\uE00A(stream, out b);
					if (uE2 == global::\uE015.\uE00B)
					{
						stream.Seek(-1L, SeekOrigin.Current);
						return null;
					}
					if (uE2 == global::\uE015.\uE00A)
					{
						return null;
					}
					if (CpInazumaMsgPack.\uE00E(uE2))
					{
						return CpInazumaMsgPack.\uE01B(stream, uE2, b);
					}
					if (CpInazumaMsgPack.\uE00F(uE2))
					{
						return CpInazumaMsgPack.\uE01C(stream, uE2, b, type);
					}
					if (CpInazumaMsgPack.\uE010(uE2))
					{
						return CpInazumaMsgPack.\uE01D(stream, uE2, b, type);
					}
					if (CpInazumaMsgPack.\uE011(uE2))
					{
						return CpInazumaMsgPack.\uE01E(stream, uE2, b);
					}
					if (CpInazumaMsgPack.\uE012(uE2))
					{
						return CpInazumaMsgPack.\uE01F(stream, uE2, b);
					}
					stream.Seek(-1L, SeekOrigin.Current);
					return null;
				}
			}
			throw new IOException(global::\uE019.\uE000(11502));
		}

		private static object \uE01B(Stream \uE04C, \uE015 \uE04D, sbyte \uE04E)
		{
			if (\uE04D != global::\uE015.\uE000)
			{
				switch (\uE04D)
				{
				case global::\uE015.\uE00C:
					return false;
				case global::\uE015.\uE00D:
					return true;
				case global::\uE015.\uE00E:
				case global::\uE015.\uE00F:
				case global::\uE015.\uE010:
				case global::\uE015.\uE011:
				case global::\uE015.\uE012:
				case global::\uE015.\uE013:
					break;
				case global::\uE015.\uE014:
					return CpInazumaMsgPack.\uE002(\uE04C);
				case global::\uE015.\uE015:
					return CpInazumaMsgPack.\uE003(\uE04C);
				case global::\uE015.\uE016:
					return (byte)\uE04C.ReadByte();
				case global::\uE015.\uE017:
					return (ushort)CpInazumaMsgPack.\uE001(\uE04C, 2);
				case global::\uE015.\uE018:
					return (uint)CpInazumaMsgPack.\uE001(\uE04C, 4);
				case global::\uE015.\uE019:
					return (ulong)CpInazumaMsgPack.\uE001(\uE04C, 8);
				case global::\uE015.\uE01A:
					return (sbyte)\uE04C.ReadByte();
				case global::\uE015.\uE01B:
					return (short)CpInazumaMsgPack.\uE001(\uE04C, 2);
				case global::\uE015.\uE01C:
					return (int)CpInazumaMsgPack.\uE001(\uE04C, 4);
				case global::\uE015.\uE01D:
					return CpInazumaMsgPack.\uE001(\uE04C, 8);
				default:
					if (\uE04D == global::\uE015.\uE001)
					{
						return \uE04E;
					}
					break;
				}
				return null;
			}
			return (byte)\uE04E;
		}

		private static object \uE01C(Stream \uE04F, \uE015 \uE050, sbyte \uE051, Type \uE052)
		{
			int num = CpInazumaMsgPack.\uE005(\uE04F, \uE050, \uE051);
			Type type = null;
			IList list = CpInazumaMsgPack.\uE013(\uE052, num, out type);
			for (int i = 0; i < num; i++)
			{
				object value = CpInazumaMsgPack.Unpack(\uE04F, type);
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

		private static object \uE01D(Stream \uE053, \uE015 \uE054, sbyte \uE055, Type \uE056)
		{
			Type type = typeof(object);
			Type type2 = typeof(object);
			if (\uE056.IsGenericType)
			{
				Type[] genericArguments = \uE056.GetGenericArguments();
				if (genericArguments.Length >= 1)
				{
					type = genericArguments[0];
				}
				if (genericArguments.Length >= 2)
				{
					type2 = genericArguments[1];
				}
			}
			int num = CpInazumaMsgPack.\uE005(\uE053, \uE054, \uE055);
			IDictionary dictionary = Activator.CreateInstance(typeof(Dictionary<, >).MakeGenericType(new Type[]
			{
				type,
				type2
			})) as IDictionary;
			for (int i = 0; i < num; i++)
			{
				object key = CpInazumaMsgPack.Unpack(\uE053, type);
				object value = CpInazumaMsgPack.Unpack(\uE053, type2);
				dictionary.Add(key, value);
			}
			return dictionary;
		}

		private static string \uE01E(Stream \uE057, \uE015 \uE058, sbyte \uE059)
		{
			int num = CpInazumaMsgPack.\uE005(\uE057, \uE058, \uE059);
			byte[] array = new byte[num];
			\uE057.Read(array, 0, num);
			return CpInazumaMsgPack.\uE000.GetString(array);
		}

		private static byte[] \uE01F(Stream \uE05A, \uE015 \uE05B, sbyte \uE05C)
		{
			int num = CpInazumaMsgPack.\uE005(\uE05A, \uE05B, \uE05C);
			byte[] array = new byte[num];
			\uE05A.Read(array, 0, num);
			return array;
		}

		private static object \uE020(Stream \uE05D, ref \uE017 \uE05E)
		{
			sbyte b = 0;
			\uE015 uE = CpInazumaMsgPack.\uE00A(\uE05D, out b);
			object obj = Activator.CreateInstance(\uE05E.\uE000);
			if (CpInazumaMsgPack.\uE010(uE))
			{
				int num = CpInazumaMsgPack.\uE005(\uE05D, uE, b);
				for (int i = 0; i < num; i++)
				{
					CpInazumaMsgPack.\uE001 uE2 = new CpInazumaMsgPack.\uE001();
					uE = CpInazumaMsgPack.\uE00A(\uE05D, out b);
					uE2.\uE000 = CpInazumaMsgPack.\uE01E(\uE05D, uE, b);
					\uE016 uE3 = \uE05E.\uE001.Find(new Predicate<\uE016>(uE2.\uE000));
					if (uE3 == null)
					{
						CpInazumaMsgPack.Unpack(\uE05D, typeof(object));
					}
					else
					{
						TypeCode typeCode = Type.GetTypeCode(uE3.\uE002.FieldType);
						object value = CpInazumaMsgPack.\uE014(CpInazumaMsgPack.Unpack(\uE05D, uE3.\uE002.FieldType), typeCode);
						uE3.\uE002.SetValue(obj, value);
					}
				}
				return obj;
			}
			if (CpInazumaMsgPack.\uE00F(uE))
			{
				\uE05D.Seek((long)CpInazumaMsgPack.\uE004(uE, b), SeekOrigin.Current);
				return CpInazumaMsgPack.\uE020(\uE05D, ref \uE05E);
			}
			\uE05D.Seek(-1L, SeekOrigin.Current);
			if (uE == global::\uE015.\uE00A)
			{
				return null;
			}
			for (int j = 0; j < \uE05E.\uE001.Count; j++)
			{
				FieldInfo uE4 = \uE05E.\uE001[j].\uE002;
				object obj2 = CpInazumaMsgPack.Unpack(\uE05D, uE4.FieldType);
				if (obj2 != null)
				{
					uE4.SetValue(obj, Convert.ChangeType(obj2, uE4.FieldType));
				}
			}
			return obj;
		}

		public static bool Analyze<T>()
		{
			\uE017 uE = default(\uE017);
			return CpInazumaMsgPack.\uE021<T>(ref uE);
		}

		private static bool \uE021<\uE000>(ref \uE017 \uE05F)
		{
			return CpInazumaMsgPack.\uE022(typeof(\uE000), ref \uE05F);
		}

		private static bool \uE022(Type \uE060, ref \uE017 \uE061)
		{
			if (CpInazumaMsgPack.\uE007.TryGetValue(\uE060, out \uE061))
			{
				return true;
			}
			if (\uE060.IsPrimitive || typeof(IList).IsAssignableFrom(\uE060) || typeof(IDictionary).IsAssignableFrom(\uE060) || \uE060 == typeof(object) || typeof(string) == \uE060)
			{
				return false;
			}
			BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			FieldInfo[] fields = \uE060.GetFields(bindingAttr);
			List<\uE016> list = new List<\uE016>(fields.Length);
			bool flag = false;
			foreach (FieldInfo fieldInfo in fields)
			{
				if (!fieldInfo.IsDefined(typeof(NonSerializedAttribute), false))
				{
					\uE016 uE = new \uE016();
					uE.\uE001 = fieldInfo.Name;
					uE.\uE000 = int.MaxValue;
					uE.\uE002 = fieldInfo;
					object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(CpSerializeFieldAttribute), false);
					if (customAttributes.Length > 0 && customAttributes[0] is CpSerializeFieldAttribute)
					{
						flag = true;
						CpSerializeFieldAttribute cpSerializeFieldAttribute = customAttributes[0] as CpSerializeFieldAttribute;
						uE.\uE001 = (cpSerializeFieldAttribute.Name ?? fieldInfo.Name);
						uE.\uE000 = cpSerializeFieldAttribute.Order;
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
			uE2.\uE000 = \uE060;
			uE2.\uE001 = list;
			CpInazumaMsgPack.\uE007[\uE060] = uE2;
			\uE017 uE3 = default(\uE017);
			for (int j = 0; j < list.Count; j++)
			{
				CpInazumaMsgPack.\uE022(list[j].\uE002.FieldType, ref uE3);
			}
			\uE061 = uE2;
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
					stringBuilder.Append(global::\uE019.\uE000(11514));
					long position = stream.Position;
					while (stream.Position < stream.Length)
					{
						long position2 = stream.Position;
						sbyte uE = 0;
						\uE015 uE2 = CpInazumaMsgPack.\uE00A(stream, out uE);
						string arg = CpInazumaMsgPack.\uE023(stream, uE2, uE);
						num++;
						stringBuilder.AppendFormat(global::\uE019.\uE000(11475), global::\uE019.\uE000(11297), uE2);
						stringBuilder.AppendFormat(global::\uE019.\uE000(11475), global::\uE019.\uE000(11320), arg);
						stringBuilder.AppendFormat(global::\uE019.\uE000(11312), global::\uE019.\uE000(11294), position2);
						stringBuilder.AppendFormat(global::\uE019.\uE000(11312), global::\uE019.\uE000(11285), stream.Position - position2);
						stringBuilder.Append(global::\uE019.\uE000(11372));
					}
					stringBuilder.AppendFormat(global::\uE019.\uE000(11333), global::\uE019.\uE000(11347), stream.Length);
					stringBuilder.AppendFormat(global::\uE019.\uE000(12197), global::\uE019.\uE000(12214), num);
					stringBuilder.AppendFormat(global::\uE019.\uE000(12169), global::\uE019.\uE000(12183), (double)stream.Length / (double)num);
					stringBuilder.Append(global::\uE019.\uE000(12259));
					stream.Seek(position, SeekOrigin.Begin);
					return stringBuilder.ToString();
				}
			}
			return string.Empty;
		}

		private static string \uE023(Stream \uE062, \uE015 \uE063, sbyte \uE064)
		{
			if (\uE063 == global::\uE015.\uE00B)
			{
				return global::\uE019.\uE000(12244);
			}
			if (\uE063 == global::\uE015.\uE00A)
			{
				return global::\uE019.\uE000(12078);
			}
			if (CpInazumaMsgPack.\uE00E(\uE063))
			{
				return CpInazumaMsgPack.\uE01B(\uE062, \uE063, \uE064).ToString();
			}
			if (CpInazumaMsgPack.\uE00F(\uE063))
			{
				int num = CpInazumaMsgPack.\uE005(\uE062, \uE063, \uE064);
				return string.Format(global::\uE019.\uE000(12075), num);
			}
			if (CpInazumaMsgPack.\uE010(\uE063))
			{
				int num2 = CpInazumaMsgPack.\uE005(\uE062, \uE063, \uE064);
				return string.Format(global::\uE019.\uE000(12085), num2);
			}
			if (CpInazumaMsgPack.\uE011(\uE063))
			{
				string text = CpInazumaMsgPack.\uE01E(\uE062, \uE063, \uE064);
				if (CpInazumaMsgPack.\uE00A)
				{
					text = text.Replace(global::\uE019.\uE000(12036), "").Replace(global::\uE019.\uE000(12038), "");
				}
				return string.Format(global::\uE019.\uE000(12032), text.Substring(0, Math.Min(text.Length, CpInazumaMsgPack.\uE009)));
			}
			if (CpInazumaMsgPack.\uE012(\uE063))
			{
				return string.Format(global::\uE019.\uE000(12062), CpInazumaMsgPack.\uE01F(\uE062, \uE063, \uE064).Length);
			}
			return string.Empty;
		}

		[CompilerGenerated]
		private static int \uE024(\uE016 \uE065, \uE016 \uE066)
		{
			return \uE065.\uE000.CompareTo(\uE066.\uE000);
		}

		[CompilerGenerated]
		private static int \uE025(\uE016 \uE067, \uE016 \uE068)
		{
			return StringComparer.Ordinal.Compare(\uE067.\uE001, \uE068.\uE001);
		}

		private class \uE000
		{
			public CpCustomDataPackMode \uE000 = CpCustomDataPackMode.Map;

			public int \uE001 = 15;

			public bool \uE002 = true;
		}

		[CompilerGenerated]
		private sealed class \uE001
		{
			public string \uE000;

			public bool \uE000(\uE016 \uE069)
			{
				return \uE069.\uE001 == this.\uE000;
			}
		}
	}
}
