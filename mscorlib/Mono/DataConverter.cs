using System;
using System.Collections;
using System.Text;

namespace Mono
{
	internal abstract class DataConverter
	{
		private static DataConverter SwapConv = new DataConverter.SwapConverter();

		private static DataConverter CopyConv = new DataConverter.CopyConverter();

		public static readonly bool IsLittleEndian = BitConverter.IsLittleEndian;

		public abstract double GetDouble(byte[] data, int index);

		public abstract float GetFloat(byte[] data, int index);

		public abstract long GetInt64(byte[] data, int index);

		public abstract int GetInt32(byte[] data, int index);

		public abstract short GetInt16(byte[] data, int index);

		[CLSCompliant(false)]
		public abstract uint GetUInt32(byte[] data, int index);

		[CLSCompliant(false)]
		public abstract ushort GetUInt16(byte[] data, int index);

		[CLSCompliant(false)]
		public abstract ulong GetUInt64(byte[] data, int index);

		public abstract void PutBytes(byte[] dest, int destIdx, double value);

		public abstract void PutBytes(byte[] dest, int destIdx, float value);

		public abstract void PutBytes(byte[] dest, int destIdx, int value);

		public abstract void PutBytes(byte[] dest, int destIdx, long value);

		public abstract void PutBytes(byte[] dest, int destIdx, short value);

		[CLSCompliant(false)]
		public abstract void PutBytes(byte[] dest, int destIdx, ushort value);

		[CLSCompliant(false)]
		public abstract void PutBytes(byte[] dest, int destIdx, uint value);

		[CLSCompliant(false)]
		public abstract void PutBytes(byte[] dest, int destIdx, ulong value);

		public byte[] GetBytes(double value)
		{
			byte[] array = new byte[8];
			this.PutBytes(array, 0, value);
			return array;
		}

		public byte[] GetBytes(float value)
		{
			byte[] array = new byte[4];
			this.PutBytes(array, 0, value);
			return array;
		}

		public byte[] GetBytes(int value)
		{
			byte[] array = new byte[4];
			this.PutBytes(array, 0, value);
			return array;
		}

		public byte[] GetBytes(long value)
		{
			byte[] array = new byte[8];
			this.PutBytes(array, 0, value);
			return array;
		}

		public byte[] GetBytes(short value)
		{
			byte[] array = new byte[2];
			this.PutBytes(array, 0, value);
			return array;
		}

		[CLSCompliant(false)]
		public byte[] GetBytes(ushort value)
		{
			byte[] array = new byte[2];
			this.PutBytes(array, 0, value);
			return array;
		}

		[CLSCompliant(false)]
		public byte[] GetBytes(uint value)
		{
			byte[] array = new byte[4];
			this.PutBytes(array, 0, value);
			return array;
		}

		[CLSCompliant(false)]
		public byte[] GetBytes(ulong value)
		{
			byte[] array = new byte[8];
			this.PutBytes(array, 0, value);
			return array;
		}

		public static DataConverter LittleEndian
		{
			get
			{
				return (!BitConverter.IsLittleEndian) ? DataConverter.SwapConv : DataConverter.CopyConv;
			}
		}

		public static DataConverter BigEndian
		{
			get
			{
				return (!BitConverter.IsLittleEndian) ? DataConverter.CopyConv : DataConverter.SwapConv;
			}
		}

		public static DataConverter Native
		{
			get
			{
				return DataConverter.CopyConv;
			}
		}

		private static int Align(int current, int align)
		{
			return (current + align - 1) / align * align;
		}

		public static byte[] Pack(string description, params object[] args)
		{
			int num = 0;
			DataConverter.PackContext packContext = new DataConverter.PackContext();
			packContext.conv = DataConverter.CopyConv;
			packContext.description = description;
			packContext.i = 0;
			while (packContext.i < description.Length)
			{
				object oarg;
				if (num < args.Length)
				{
					oarg = args[num];
				}
				else
				{
					if (packContext.repeat != 0)
					{
						break;
					}
					oarg = null;
				}
				int i = packContext.i;
				if (DataConverter.PackOne(packContext, oarg))
				{
					num++;
					if (packContext.repeat > 0)
					{
						if (--packContext.repeat > 0)
						{
							packContext.i = i;
						}
						else
						{
							packContext.i++;
						}
					}
					else
					{
						packContext.i++;
					}
				}
				else
				{
					packContext.i++;
				}
			}
			return packContext.Get();
		}

		public static byte[] PackEnumerable(string description, IEnumerable args)
		{
			DataConverter.PackContext packContext = new DataConverter.PackContext();
			packContext.conv = DataConverter.CopyConv;
			packContext.description = description;
			IEnumerator enumerator = args.GetEnumerator();
			bool flag = enumerator.MoveNext();
			packContext.i = 0;
			while (packContext.i < description.Length)
			{
				object oarg;
				if (flag)
				{
					oarg = enumerator.Current;
				}
				else
				{
					if (packContext.repeat != 0)
					{
						break;
					}
					oarg = null;
				}
				int i = packContext.i;
				if (DataConverter.PackOne(packContext, oarg))
				{
					flag = enumerator.MoveNext();
					if (packContext.repeat > 0)
					{
						if (--packContext.repeat > 0)
						{
							packContext.i = i;
						}
						else
						{
							packContext.i++;
						}
					}
					else
					{
						packContext.i++;
					}
				}
				else
				{
					packContext.i++;
				}
			}
			return packContext.Get();
		}

		private static bool PackOne(DataConverter.PackContext b, object oarg)
		{
			char c = b.description[b.i];
			switch (c)
			{
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				b.repeat = (int)((short)b.description[b.i] - 48);
				return false;
			default:
				switch (c)
				{
				case '[':
				{
					int num = -1;
					int i;
					for (i = b.i + 1; i < b.description.Length; i++)
					{
						if (b.description[i] == ']')
						{
							break;
						}
						int num2 = (int)((short)b.description[i] - 48);
						if (num2 >= 0 && num2 <= 9)
						{
							if (num == -1)
							{
								num = num2;
							}
							else
							{
								num = num * 10 + num2;
							}
						}
					}
					if (num == -1)
					{
						throw new ArgumentException("invalid size specification");
					}
					b.i = i;
					b.repeat = num;
					return false;
				}
				default:
				{
					switch (c)
					{
					case '!':
						b.align = -1;
						return false;
					default:
						switch (c)
						{
						case 'I':
							b.Add(b.conv.GetBytes(Convert.ToUInt32(oarg)));
							return true;
						default:
							switch (c)
							{
							case 'x':
								b.Add(new byte[1]);
								return false;
							default:
								if (c == '*')
								{
									b.repeat = int.MaxValue;
									return false;
								}
								if (c == 'S')
								{
									b.Add(b.conv.GetBytes(Convert.ToUInt16(oarg)));
									return true;
								}
								if (c != 's')
								{
									throw new ArgumentException(string.Format("invalid format specified `{0}'", b.description[b.i]));
								}
								b.Add(b.conv.GetBytes(Convert.ToInt16(oarg)));
								return true;
							case 'z':
								break;
							}
							break;
						case 'L':
							b.Add(b.conv.GetBytes(Convert.ToUInt64(oarg)));
							return true;
						}
						break;
					case '$':
						break;
					case '%':
						b.conv = DataConverter.Native;
						return false;
					}
					bool flag = b.description[b.i] == 'z';
					b.i++;
					if (b.i >= b.description.Length)
					{
						throw new ArgumentException("$ description needs a type specified", "description");
					}
					char c2 = b.description[b.i];
					char c3 = c2;
					int num2;
					Encoding encoding;
					switch (c3)
					{
					case '3':
						encoding = Encoding.GetEncoding(12000);
						num2 = 4;
						break;
					case '4':
						encoding = Encoding.GetEncoding(12001);
						num2 = 4;
						break;
					default:
						if (c3 != 'b')
						{
							throw new ArgumentException("Invalid format for $ specifier", "description");
						}
						encoding = Encoding.BigEndianUnicode;
						num2 = 2;
						break;
					case '6':
						encoding = Encoding.Unicode;
						num2 = 2;
						break;
					case '7':
						encoding = Encoding.UTF7;
						num2 = 1;
						break;
					case '8':
						encoding = Encoding.UTF8;
						num2 = 1;
						break;
					}
					if (b.align == -1)
					{
						b.align = 4;
					}
					b.Add(encoding.GetBytes(Convert.ToString(oarg)));
					if (flag)
					{
						b.Add(new byte[num2]);
					}
					break;
				}
				case '^':
					b.conv = DataConverter.BigEndian;
					return false;
				case '_':
					b.conv = DataConverter.LittleEndian;
					return false;
				case 'b':
					b.Add(new byte[]
					{
						Convert.ToByte(oarg)
					});
					break;
				case 'c':
					b.Add(new byte[]
					{
						(byte)Convert.ToSByte(oarg)
					});
					break;
				case 'd':
					b.Add(b.conv.GetBytes(Convert.ToDouble(oarg)));
					break;
				case 'f':
					b.Add(b.conv.GetBytes(Convert.ToSingle(oarg)));
					break;
				case 'i':
					b.Add(b.conv.GetBytes(Convert.ToInt32(oarg)));
					break;
				case 'l':
					b.Add(b.conv.GetBytes(Convert.ToInt64(oarg)));
					break;
				}
				break;
			case 'C':
				b.Add(new byte[]
				{
					Convert.ToByte(oarg)
				});
				break;
			}
			return true;
		}

		private static bool Prepare(byte[] buffer, ref int idx, int size, ref bool align)
		{
			if (align)
			{
				idx = DataConverter.Align(idx, size);
				align = false;
			}
			if (idx + size > buffer.Length)
			{
				idx = buffer.Length;
				return false;
			}
			return true;
		}

		public static IList Unpack(string description, byte[] buffer, int startIndex)
		{
			DataConverter dataConverter = DataConverter.CopyConv;
			ArrayList arrayList = new ArrayList();
			int num = startIndex;
			bool flag = false;
			int num2 = 0;
			int num3 = 0;
			while (num3 < description.Length && num < buffer.Length)
			{
				int num4 = num3;
				char c = description[num3];
				switch (c)
				{
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					num2 = (int)((short)description[num3] - 48);
					num4 = num3 + 1;
					break;
				default:
					switch (c)
					{
					case '[':
					{
						int num5 = -1;
						int i;
						for (i = num3 + 1; i < description.Length; i++)
						{
							if (description[i] == ']')
							{
								break;
							}
							int num6 = (int)((short)description[i] - 48);
							if (num6 >= 0 && num6 <= 9)
							{
								if (num5 == -1)
								{
									num5 = num6;
								}
								else
								{
									num5 = num5 * 10 + num6;
								}
							}
						}
						if (num5 == -1)
						{
							throw new ArgumentException("invalid size specification");
						}
						num3 = i;
						num2 = num5;
						break;
					}
					default:
					{
						switch (c)
						{
						case '!':
							flag = true;
							goto IL_683;
						default:
							switch (c)
							{
							case 'I':
								if (DataConverter.Prepare(buffer, ref num, 4, ref flag))
								{
									arrayList.Add(dataConverter.GetUInt32(buffer, num));
									num += 4;
								}
								goto IL_683;
							default:
								switch (c)
								{
								case 'x':
									num++;
									goto IL_683;
								default:
									if (c == '*')
									{
										num2 = int.MaxValue;
										goto IL_683;
									}
									if (c == 'S')
									{
										if (DataConverter.Prepare(buffer, ref num, 2, ref flag))
										{
											arrayList.Add(dataConverter.GetUInt16(buffer, num));
											num += 2;
										}
										goto IL_683;
									}
									if (c != 's')
									{
										throw new ArgumentException(string.Format("invalid format specified `{0}'", description[num3]));
									}
									if (DataConverter.Prepare(buffer, ref num, 2, ref flag))
									{
										arrayList.Add(dataConverter.GetInt16(buffer, num));
										num += 2;
									}
									goto IL_683;
								case 'z':
									break;
								}
								break;
							case 'L':
								if (DataConverter.Prepare(buffer, ref num, 8, ref flag))
								{
									arrayList.Add(dataConverter.GetUInt64(buffer, num));
									num += 8;
								}
								goto IL_683;
							}
							break;
						case '$':
							break;
						case '%':
							dataConverter = DataConverter.Native;
							goto IL_683;
						}
						num3++;
						if (num3 >= description.Length)
						{
							throw new ArgumentException("$ description needs a type specified", "description");
						}
						char c2 = description[num3];
						if (flag)
						{
							num = DataConverter.Align(num, 4);
							flag = false;
						}
						if (num < buffer.Length)
						{
							char c3 = c2;
							int num6;
							Encoding encoding;
							switch (c3)
							{
							case '3':
								encoding = Encoding.GetEncoding(12000);
								num6 = 4;
								break;
							case '4':
								encoding = Encoding.GetEncoding(12001);
								num6 = 4;
								break;
							default:
								if (c3 != 'b')
								{
									throw new ArgumentException("Invalid format for $ specifier", "description");
								}
								encoding = Encoding.BigEndianUnicode;
								num6 = 2;
								break;
							case '6':
								encoding = Encoding.Unicode;
								num6 = 2;
								break;
							case '7':
								encoding = Encoding.UTF7;
								num6 = 1;
								break;
							case '8':
								encoding = Encoding.UTF8;
								num6 = 1;
								break;
							}
							int j = num;
							switch (num6)
							{
							case 1:
								while (j < buffer.Length && buffer[j] != 0)
								{
									j++;
								}
								arrayList.Add(encoding.GetChars(buffer, num, j - num));
								if (j == buffer.Length)
								{
									num = j;
								}
								else
								{
									num = j + 1;
								}
								break;
							case 2:
								while (j < buffer.Length)
								{
									if (j + 1 == buffer.Length)
									{
										j++;
										break;
									}
									if (buffer[j] == 0 && buffer[j + 1] == 0)
									{
										break;
									}
									j++;
								}
								arrayList.Add(encoding.GetChars(buffer, num, j - num));
								if (j == buffer.Length)
								{
									num = j;
								}
								else
								{
									num = j + 2;
								}
								break;
							case 4:
								while (j < buffer.Length)
								{
									if (j + 3 >= buffer.Length)
									{
										j = buffer.Length;
										break;
									}
									if (buffer[j] == 0 && buffer[j + 1] == 0 && buffer[j + 2] == 0 && buffer[j + 3] == 0)
									{
										break;
									}
									j++;
								}
								arrayList.Add(encoding.GetChars(buffer, num, j - num));
								if (j == buffer.Length)
								{
									num = j;
								}
								else
								{
									num = j + 4;
								}
								break;
							}
						}
						break;
					}
					case '^':
						dataConverter = DataConverter.BigEndian;
						break;
					case '_':
						dataConverter = DataConverter.LittleEndian;
						break;
					case 'b':
						if (DataConverter.Prepare(buffer, ref num, 1, ref flag))
						{
							arrayList.Add(buffer[num]);
							num++;
						}
						break;
					case 'c':
						goto IL_300;
					case 'd':
						if (DataConverter.Prepare(buffer, ref num, 8, ref flag))
						{
							arrayList.Add(dataConverter.GetDouble(buffer, num));
							num += 8;
						}
						break;
					case 'f':
						if (DataConverter.Prepare(buffer, ref num, 4, ref flag))
						{
							arrayList.Add(dataConverter.GetDouble(buffer, num));
							num += 4;
						}
						break;
					case 'i':
						if (DataConverter.Prepare(buffer, ref num, 4, ref flag))
						{
							arrayList.Add(dataConverter.GetInt32(buffer, num));
							num += 4;
						}
						break;
					case 'l':
						if (DataConverter.Prepare(buffer, ref num, 8, ref flag))
						{
							arrayList.Add(dataConverter.GetInt64(buffer, num));
							num += 8;
						}
						break;
					}
					break;
				case 'C':
					goto IL_300;
				}
				IL_683:
				if (num2 > 0)
				{
					if (--num2 > 0)
					{
						num3 = num4;
					}
					continue;
				}
				num3++;
				continue;
				IL_300:
				if (DataConverter.Prepare(buffer, ref num, 1, ref flag))
				{
					char c4;
					if (description[num3] == 'c')
					{
						c4 = (char)((sbyte)buffer[num]);
					}
					else
					{
						c4 = (char)buffer[num];
					}
					arrayList.Add(c4);
					num++;
				}
				goto IL_683;
			}
			return arrayList;
		}

		internal void Check(byte[] dest, int destIdx, int size)
		{
			if (dest == null)
			{
				throw new ArgumentNullException("dest");
			}
			if (destIdx < 0 || destIdx > dest.Length - size)
			{
				throw new ArgumentException("destIdx");
			}
		}

		private class PackContext
		{
			public byte[] buffer;

			private int next;

			public string description;

			public int i;

			public DataConverter conv;

			public int repeat;

			public int align;

			public void Add(byte[] group)
			{
				if (this.buffer == null)
				{
					this.buffer = group;
					this.next = group.Length;
					return;
				}
				if (this.align != 0)
				{
					if (this.align == -1)
					{
						this.next = DataConverter.Align(this.next, group.Length);
					}
					else
					{
						this.next = DataConverter.Align(this.next, this.align);
					}
					this.align = 0;
				}
				if (this.next + group.Length > this.buffer.Length)
				{
					byte[] destinationArray = new byte[Math.Max(this.next, 16) * 2 + group.Length];
					Array.Copy(this.buffer, destinationArray, this.buffer.Length);
					Array.Copy(group, 0, destinationArray, this.next, group.Length);
					this.next += group.Length;
					this.buffer = destinationArray;
				}
				else
				{
					Array.Copy(group, 0, this.buffer, this.next, group.Length);
					this.next += group.Length;
				}
			}

			public byte[] Get()
			{
				if (this.buffer == null)
				{
					return new byte[0];
				}
				if (this.buffer.Length != this.next)
				{
					byte[] array = new byte[this.next];
					Array.Copy(this.buffer, array, this.next);
					return array;
				}
				return this.buffer;
			}
		}

		private class CopyConverter : DataConverter
		{
			public unsafe override double GetDouble(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 8)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				double result;
				for (int i = 0; i < 8; i++)
				{
					*(ref result + i) = data[index + i];
				}
				return result;
			}

			public unsafe override ulong GetUInt64(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 8)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				ulong result;
				for (int i = 0; i < 8; i++)
				{
					*(ref result + i) = data[index + i];
				}
				return result;
			}

			public unsafe override long GetInt64(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 8)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				long result;
				for (int i = 0; i < 8; i++)
				{
					*(ref result + i) = data[index + i];
				}
				return result;
			}

			public unsafe override float GetFloat(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 4)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				float result;
				for (int i = 0; i < 4; i++)
				{
					*(ref result + i) = data[index + i];
				}
				return result;
			}

			public unsafe override int GetInt32(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 4)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				int result;
				for (int i = 0; i < 4; i++)
				{
					*(ref result + i) = data[index + i];
				}
				return result;
			}

			public unsafe override uint GetUInt32(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 4)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				uint result;
				for (int i = 0; i < 4; i++)
				{
					*(ref result + i) = data[index + i];
				}
				return result;
			}

			public unsafe override short GetInt16(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 2)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				short result;
				for (int i = 0; i < 2; i++)
				{
					*(ref result + i) = data[index + i];
				}
				return result;
			}

			public unsafe override ushort GetUInt16(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 2)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				ushort result;
				for (int i = 0; i < 2; i++)
				{
					*(ref result + i) = data[index + i];
				}
				return result;
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, double value)
			{
				base.Check(dest, destIdx, 8);
				fixed (byte* ptr = &dest[destIdx])
				{
					*(long*)ptr = (long)value;
				}
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, float value)
			{
				base.Check(dest, destIdx, 4);
				fixed (byte* ptr = &dest[destIdx])
				{
					*(int*)ptr = (int)value;
				}
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, int value)
			{
				base.Check(dest, destIdx, 4);
				fixed (byte* ptr = &dest[destIdx])
				{
					*(int*)ptr = value;
				}
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, uint value)
			{
				base.Check(dest, destIdx, 4);
				fixed (byte* ptr = &dest[destIdx])
				{
					*(int*)ptr = (int)value;
				}
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, long value)
			{
				base.Check(dest, destIdx, 8);
				fixed (byte* ptr = &dest[destIdx])
				{
					*(long*)ptr = value;
				}
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, ulong value)
			{
				base.Check(dest, destIdx, 8);
				fixed (byte* ptr = &dest[destIdx])
				{
					*(long*)ptr = (long)value;
				}
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, short value)
			{
				base.Check(dest, destIdx, 2);
				fixed (byte* ptr = &dest[destIdx])
				{
					*(short*)ptr = value;
				}
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, ushort value)
			{
				base.Check(dest, destIdx, 2);
				fixed (byte* ptr = &dest[destIdx])
				{
					*(short*)ptr = (short)value;
				}
			}
		}

		private class SwapConverter : DataConverter
		{
			public unsafe override double GetDouble(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 8)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				double result;
				for (int i = 0; i < 8; i++)
				{
					*(ref result + (7 - i)) = data[index + i];
				}
				return result;
			}

			public unsafe override ulong GetUInt64(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 8)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				ulong result;
				for (int i = 0; i < 8; i++)
				{
					*(ref result + (7 - i)) = data[index + i];
				}
				return result;
			}

			public unsafe override long GetInt64(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 8)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				long result;
				for (int i = 0; i < 8; i++)
				{
					*(ref result + (7 - i)) = data[index + i];
				}
				return result;
			}

			public unsafe override float GetFloat(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 4)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				float result;
				for (int i = 0; i < 4; i++)
				{
					*(ref result + (3 - i)) = data[index + i];
				}
				return result;
			}

			public unsafe override int GetInt32(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 4)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				int result;
				for (int i = 0; i < 4; i++)
				{
					*(ref result + (3 - i)) = data[index + i];
				}
				return result;
			}

			public unsafe override uint GetUInt32(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 4)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				uint result;
				for (int i = 0; i < 4; i++)
				{
					*(ref result + (3 - i)) = data[index + i];
				}
				return result;
			}

			public unsafe override short GetInt16(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 2)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				short result;
				for (int i = 0; i < 2; i++)
				{
					*(ref result + (1 - i)) = data[index + i];
				}
				return result;
			}

			public unsafe override ushort GetUInt16(byte[] data, int index)
			{
				if (data == null)
				{
					throw new ArgumentNullException("data");
				}
				if (data.Length - index < 2)
				{
					throw new ArgumentException("index");
				}
				if (index < 0)
				{
					throw new ArgumentException("index");
				}
				ushort result;
				for (int i = 0; i < 2; i++)
				{
					*(ref result + (1 - i)) = data[index + i];
				}
				return result;
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, double value)
			{
				base.Check(dest, destIdx, 8);
				fixed (byte* ptr = &dest[destIdx])
				{
					for (int i = 0; i < 8; i++)
					{
						ptr[i] = *(ref value + (7 - i));
					}
				}
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, float value)
			{
				base.Check(dest, destIdx, 4);
				fixed (byte* ptr = &dest[destIdx])
				{
					for (int i = 0; i < 4; i++)
					{
						ptr[i] = *(ref value + (3 - i));
					}
				}
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, int value)
			{
				base.Check(dest, destIdx, 4);
				fixed (byte* ptr = &dest[destIdx])
				{
					for (int i = 0; i < 4; i++)
					{
						ptr[i] = *(ref value + (3 - i));
					}
				}
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, uint value)
			{
				base.Check(dest, destIdx, 4);
				fixed (byte* ptr = &dest[destIdx])
				{
					for (int i = 0; i < 4; i++)
					{
						ptr[i] = *(ref value + (3 - i));
					}
				}
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, long value)
			{
				base.Check(dest, destIdx, 8);
				fixed (byte* ptr = &dest[destIdx])
				{
					for (int i = 0; i < 8; i++)
					{
						ptr[i] = *(ref value + (7 - i));
					}
				}
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, ulong value)
			{
				base.Check(dest, destIdx, 8);
				fixed (byte* ptr = &dest[destIdx])
				{
					for (int i = 0; i < 4; i++)
					{
						ptr[i] = *(ref value + (7 - i));
					}
				}
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, short value)
			{
				base.Check(dest, destIdx, 2);
				fixed (byte* ptr = &dest[destIdx])
				{
					for (int i = 0; i < 2; i++)
					{
						ptr[i] = *(ref value + (1 - i));
					}
				}
			}

			public unsafe override void PutBytes(byte[] dest, int destIdx, ushort value)
			{
				base.Check(dest, destIdx, 2);
				fixed (byte* ptr = &dest[destIdx])
				{
					for (int i = 0; i < 2; i++)
					{
						ptr[i] = *(ref value + (1 - i));
					}
				}
			}
		}
	}
}
