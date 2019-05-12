using System;
using System.IO;

internal sealed class \uE020
{
	public static void \uE000(Stream \uE0A5, Stream \uE0A6)
	{
		byte[] array = new byte[4096];
		\uE020.\uE000 uE = new \uE020.\uE000(\uE0A5);
		for (;;)
		{
			int num = uE.\uE000(array, 0, array.Length);
			for (;;)
			{
				int num2 = \uE01A.\uE00D(49);
				for (;;)
				{
					switch (num2)
					{
					case 0:
						if (num > 0)
						{
							num2 = 1;
							continue;
						}
						return;
					case 1:
						\uE0A6.Write(array, 0, num);
						num2 = 2;
						continue;
					case 2:
						goto IL_5F;
					}
					break;
				}
			}
			IL_5F:;
		}
	}

	internal static int \uE001(int \uE0A7)
	{
		switch (\uE0A7 - ~(-(-(~-2147483605)) ^ -2147483648))
		{
		case 0:
			return ~(-(-(-(-(-6 ^ int.MinValue))) ^ int.MinValue));
		case 1:
			break;
		case 2:
			return -(-(-2147483641 ^ int.MinValue ^ int.MinValue ^ int.MinValue));
		default:
			if (\uE0A7 == ~(-(~(~(-(~(-2147483595 ^ -2147483648)))))))
			{
				return -(-(-(~10)));
			}
			if (\uE0A7 == ~(-(-(-(-2147483585 ^ -2147483648)))))
			{
				return ~(~(~(~8)));
			}
			break;
		}
		return ~(~(~(~(~int.MinValue) ^ int.MinValue)));
	}

	public sealed class \uE000
	{
		private \uE020.\uE002 \uE00C = new \uE020.\uE002(32769);

		private \uE020.\uE001 \uE00D;

		private \uE01F \uE00E;

		private int \uE00F = -1;

		private int \uE010 = -1;

		private bool \uE011;

		private int \uE012;

		private long \uE013;

		private long \uE014;

		private bool \uE015;

		private int \uE016;

		private bool \uE017;

		public \uE000(Stream \uE0A8)
		{
			this.\uE00D = new \uE020.\uE001(\uE0A8);
		}

		public int \uE000(byte[] \uE0A9, int \uE0AA, int \uE0AB)
		{
			if (\uE0AB == 0 || this.\uE015)
			{
				return 0;
			}
			int num = 0;
			if (num == 0)
			{
				goto IL_C3;
			}
			IL_2F:
			this.\uE015 = !this.\uE001();
			IL_3E:
			if (this.\uE00F < 0 && !this.\uE015)
			{
				goto IL_2F;
			}
			if (this.\uE015)
			{
				return num;
			}
			int num2 = this.\uE002(\uE0A9, \uE0AA + num, \uE0AB - num);
			for (;;)
			{
				int num3 = \uE01A.\uE00D(49);
				for (;;)
				{
					switch (num3)
					{
					case 0:
						if (num2 > 0)
						{
							num3 = 1;
							continue;
						}
						goto IL_B0;
					case 1:
						num += num2;
						num3 = 2;
						continue;
					case 2:
						goto IL_AE;
					}
					break;
				}
			}
			IL_AE:
			goto IL_C3;
			IL_B0:
			this.\uE00F = -1;
			IL_C3:
			if (num < \uE0AB)
			{
				goto IL_3E;
			}
			return num;
		}

		private bool \uE001()
		{
			if (((!this.\uE011) ? (-2147483647 ^ -2147483648 ^ -2147483648 ^ -2147483648) : (-(-(-(-0))))) == 0)
			{
				return (-(-(-(int.MinValue != 0) != 0) != 0) ^ int.MinValue) != 0;
			}
			this.\uE013 = this.\uE00D.\uE009;
			for (;;)
			{
				int num = \uE01A.\uE00D(-(~(-2147483597 ^ int.MinValue)));
				for (;;)
				{
					switch (num)
					{
					case 0:
						this.\uE00F = this.\uE00D.\uE000(~(~3 ^ int.MinValue) ^ int.MinValue);
						num = -(-(~(-7)));
						continue;
					case 1:
						this.\uE017 = (-(~(-(~(-1 != 0) != 0) != 0) != 0) != 0);
						num = ~(~(-2147483640 ^ int.MinValue));
						continue;
					case 2:
						this.\uE00E = null;
						num = -(~(~(int.MaxValue ^ int.MinValue)));
						continue;
					case 3:
					{
						int uE = this.\uE00D.\uE000(~(-(-2147483631)) ^ int.MinValue) ^ ~(~(~2147422376) ^ int.MinValue);
						num = \uE020.\uE001(~(~43) ^ int.MinValue ^ int.MinValue);
						continue;
					}
					case 4:
						this.\uE011 = (this.\uE00D.\uE000(~(-(2 ^ int.MinValue)) ^ int.MinValue) > ~(-(-(int.MaxValue ^ int.MinValue))));
						num = -(~(~int.MinValue) ^ int.MinValue);
						continue;
					case 5:
					{
						int uE;
						this.\uE012 = uE;
						num = (~(~(-2147483646)) ^ int.MinValue);
						continue;
					}
					case 6:
						if (this.\uE00F == ~(-(5 ^ -2147483648 ^ -2147483648)))
						{
							num = (-(~6) ^ int.MinValue ^ int.MinValue);
							continue;
						}
						goto IL_23B;
					case 7:
						this.\uE00D.\uE001();
						num = -(~(-(~1)));
						continue;
					case 8:
						goto IL_236;
					}
					break;
				}
			}
			IL_236:
			goto IL_491;
			IL_23B:
			if (this.\uE00F == ~(-(-2147483646) ^ -2147483648))
			{
				\uE01D[] uE2 = \uE01B.\uE000;
				for (;;)
				{
					int num2 = \uE01A.\uE00D(-(~(~(-48))));
					for (;;)
					{
						switch (num2)
						{
						case 0:
							this.\uE00E = \uE01B.\uE002;
							num2 = (~(~3) ^ int.MinValue ^ int.MinValue);
							continue;
						case 1:
							this.\uE012 = ~(int.MaxValue ^ int.MinValue ^ int.MinValue ^ int.MinValue);
							num2 = -(~(-1 ^ int.MinValue ^ int.MinValue));
							continue;
						case 2:
						{
							\uE01D[] uE3 = \uE01B.\uE001;
							num2 = ~(~(1 ^ int.MinValue) ^ int.MinValue);
							continue;
						}
						case 3:
							this.\uE017 = (-(~(~(~(-1 != 0) != 0) != 0) != 0) != 0);
							num2 = -(~(-2147483645) ^ int.MinValue);
							continue;
						case 4:
							goto IL_34E;
						}
						break;
					}
				}
				IL_34E:;
			}
			else if (this.\uE00F == ~(-(-2147483645 ^ -2147483648)))
			{
				\uE01D[] uE2;
				\uE01D[] uE3;
				this.\uE009(this.\uE00D, out uE2, out uE3);
				for (;;)
				{
					int num3 = \uE01A.\uE00D(-(~(45 ^ int.MinValue ^ int.MinValue)));
					for (;;)
					{
						switch (num3)
						{
						case 0:
							this.\uE00E = \uE01B.\uE004(uE2, uE3);
							num3 = (~(~2) ^ int.MinValue ^ int.MinValue);
							continue;
						case 1:
							this.\uE012 = ~(~(0 ^ int.MinValue) ^ int.MinValue);
							num3 = ~(int.MaxValue ^ int.MinValue ^ int.MinValue ^ int.MinValue);
							continue;
						case 2:
							goto IL_442;
						}
						break;
					}
				}
				IL_442:
				this.\uE017 = (-((-((0 ^ int.MinValue) != 0) ^ int.MinValue) != 0) != 0);
			}
			IL_491:
			this.\uE014 = this.\uE00D.\uE009;
			return ~((~(~(2147483646 != 0) != 0) ^ int.MinValue) != 0) != 0;
		}

		private int \uE002(byte[] \uE0AC, int \uE0AD, int \uE0AE)
		{
			int num = \uE0AD;
			if (this.\uE00F == (4 ^ -2147483648 ^ -2147483648 ^ -2147483648 ^ -2147483648))
			{
				if (this.\uE012 > (~(-(~2147483646)) ^ -2147483648))
				{
					int num2 = Math.Min(\uE0AE, this.\uE012);
					for (;;)
					{
						int num3 = \uE01A.\uE00D(-(-(48 ^ int.MinValue ^ int.MinValue)));
						for (;;)
						{
							switch (num3)
							{
							case 0:
								\uE0AD += num2;
								num3 = ~(-6 ^ int.MinValue ^ int.MinValue);
								continue;
							case 1:
								this.\uE00C.\uE001(\uE0AC, \uE0AD, num2);
								num3 = \uE01A.\uE00D(~(-(53 ^ int.MinValue ^ int.MinValue)));
								continue;
							case 2:
								this.\uE00D.\uE002(\uE0AC, \uE0AD, num2);
								num3 = (~(2147483646 ^ int.MinValue ^ int.MinValue) ^ int.MinValue);
								continue;
							case 3:
								\uE0AE -= num2;
								num3 = (~(-1) ^ int.MinValue ^ int.MinValue);
								continue;
							case 4:
								this.\uE012 -= num2;
								num3 = -(~(2 ^ int.MinValue ^ int.MinValue));
								continue;
							case 5:
								goto IL_1A8;
							}
							break;
						}
					}
					IL_1A8:;
				}
			}
			else if (((!this.\uE017) ? (~((~((0 ^ -2147483648) != 0) ^ -2147483648) != 0)) : (~(-(~(2147483645 != 0) != 0) != 0) ^ -2147483648)) == 0)
			{
				if (this.\uE016 > (-(-2147483648 ^ -2147483648) ^ -2147483648 ^ -2147483648))
				{
					this.\uE003(\uE0AC, ref \uE0AD, ref \uE0AE);
				}
				if (\uE0AE > (0 ^ -2147483648 ^ -2147483648 ^ -2147483648 ^ -2147483648))
				{
					do
					{
						int num4 = \uE020.\uE000.\uE006(this.\uE00D, this.\uE00E.\uE002);
						this.\uE017 = (num4 == (~2147483391 ^ int.MinValue ^ int.MinValue ^ int.MinValue));
						if (((!this.\uE017) ? (~(-((1 ^ -2147483648) != 0) != 0) ^ -2147483648) : (~(-((2 ^ -2147483648) != 0) != 0) ^ -2147483648)) != 0)
						{
							break;
						}
						if (num4 < -(2147483392 ^ -2147483648 ^ -2147483648 ^ -2147483648))
						{
							int num5 = \uE0AD;
							\uE0AD = num5 + -(~(-int.MinValue) ^ int.MinValue);
							\uE0AC[num5] = (byte)num4;
							for (;;)
							{
								int num6 = \uE01A.\uE00D(-(~(~2147483602) ^ int.MinValue));
								for (;;)
								{
									switch (num6)
									{
									case 0:
										\uE0AE -= (-(-(1 ^ int.MinValue)) ^ int.MinValue);
										num6 = -(-(~2147483645 ^ int.MinValue));
										continue;
									case 1:
										this.\uE00C.\uE000((byte)num4);
										num6 = ~(~(-int.MinValue) ^ int.MinValue);
										continue;
									case 2:
										goto IL_3AC;
									}
									break;
								}
							}
							IL_3AC:;
						}
						else if (num4 <= ~(~(-2147483363) ^ -2147483648))
						{
							int uE = \uE020.\uE000.\uE007(this.\uE00D, num4);
							for (;;)
							{
								int num7 = \uE01A.\uE00D(~(-49 ^ int.MinValue) ^ int.MinValue);
								for (;;)
								{
									switch (num7)
									{
									case 0:
										this.\uE016 = uE;
										num7 = -(~(2 ^ int.MinValue) ^ int.MinValue);
										continue;
									case 1:
									{
										int uE2;
										this.\uE010 = uE2;
										num7 = -(~-1 ^ int.MinValue ^ int.MinValue);
										continue;
									}
									case 2:
									{
										int uE2 = \uE020.\uE000.\uE008(this.\uE00D, this.\uE00E.\uE003);
										num7 = \uE01A.\uE00D(~(-(-(2147483601 ^ int.MinValue))));
										continue;
									}
									case 3:
										goto IL_4AB;
									}
									break;
								}
							}
							IL_4AB:
							this.\uE003(\uE0AC, ref \uE0AD, ref \uE0AE);
						}
					}
					while (\uE0AE > ~(-(~(-2))));
				}
			}
			this.\uE014 = this.\uE00D.\uE009;
			return \uE0AD - num;
		}

		private void \uE003(byte[] \uE0AF, ref int \uE0B0, ref int \uE0B1)
		{
			int i = Math.Min(this.\uE016, \uE0B1);
			byte[] array = this.\uE00C.\uE002(this.\uE010, Math.Min(i, this.\uE010));
			for (;;)
			{
				int num = \uE01A.\uE00D(46);
				for (;;)
				{
					switch (num)
					{
					case 0:
						this.\uE016 -= i;
						num = 2;
						continue;
					case 1:
						\uE0B1 -= i;
						num = 0;
						continue;
					case 2:
						goto IL_5E;
					}
					break;
				}
			}
			IL_5E:
			while (i > array.Length)
			{
				Array.Copy(array, 0, \uE0AF, \uE0B0, array.Length);
				for (;;)
				{
					int num2 = \uE01A.\uE00D(46);
					for (;;)
					{
						switch (num2)
						{
						case 0:
							i -= array.Length;
							num2 = 2;
							continue;
						case 1:
							\uE0B0 += array.Length;
							num2 = 0;
							continue;
						case 2:
							goto IL_A1;
						}
						break;
					}
				}
				IL_A1:
				this.\uE00C.\uE001(array, 0, array.Length);
			}
			Array.Copy(array, 0, \uE0AF, \uE0B0, i);
			\uE0B0 += i;
			this.\uE00C.\uE001(array, 0, i);
		}

		public bool \uE004(int \uE0B2)
		{
			byte[] uE0A = new byte[1024];
			while (\uE0B2 > 0)
			{
				int num;
				if ((num = this.\uE000(uE0A, 0, Math.Min(1024, \uE0B2))) <= 0)
				{
					break;
				}
				\uE0B2 -= num;
			}
			return \uE0B2 <= 0;
		}

		public void \uE005()
		{
			byte[] uE0A = new byte[1024];
			while (this.\uE000(uE0A, 0, 1024) > 0)
			{
			}
		}

		private static int \uE006(\uE020.\uE001 \uE0B3, \uE01E \uE0B4)
		{
			while (\uE0B4 != null && !\uE0B4.\uE004)
			{
				\uE0B4 = ((\uE0B3.\uE000(1) > 0) ? \uE0B4.\uE007 : \uE0B4.\uE006);
			}
			return (int)\uE0B4.\uE005;
		}

		private static int \uE007(\uE020.\uE001 \uE0B5, int \uE0B6)
		{
			int num;
			int num2;
			\uE01B.\uE007(\uE0B6, out num, out num2);
			if (num2 > 0)
			{
				return num + \uE0B5.\uE000(num2);
			}
			return num;
		}

		private static int \uE008(\uE020.\uE001 \uE0B7, \uE01E \uE0B8)
		{
			int num = \uE020.\uE000.\uE006(\uE0B7, \uE0B8);
			int num3;
			int num4;
			for (;;)
			{
				int num2 = \uE01A.\uE00D(49);
				for (;;)
				{
					switch (num2)
					{
					case 0:
						num3 = \uE01B.\uE006[num];
						num2 = 3;
						continue;
					case 1:
					{
						int num5;
						num4 = \uE0B7.\uE000(num5);
						num2 = 4;
						continue;
					}
					case 2:
					{
						int num5;
						if (num5 > 0)
						{
							num2 = 1;
							continue;
						}
						return num3;
					}
					case 3:
					{
						int num5 = \uE01B.\uE007[num];
						num2 = 2;
						continue;
					}
					case 4:
						goto IL_6F;
					}
					break;
				}
			}
			return num3;
			IL_6F:
			return num3 + num4;
		}

		private void \uE009(\uE020.\uE001 \uE0B9, out \uE01D[] \uE0BA, out \uE01D[] \uE0BB)
		{
			int num = \uE0B9.\uE000(5) + 257;
			int[] array;
			int num3;
			for (;;)
			{
				int num2 = \uE01A.\uE00D(46);
				for (;;)
				{
					int num4;
					int num5;
					switch (num2)
					{
					case 0:
						array = new int[19];
						num2 = 3;
						continue;
					case 1:
						num3 = \uE0B9.\uE000(5) + 1;
						num2 = 2;
						continue;
					case 2:
						num4 = \uE0B9.\uE000(4) + 4;
						num2 = 6;
						continue;
					case 3:
						num5 = 0;
						num2 = 4;
						continue;
					case 4:
						if (num5 != 0)
						{
							num2 = 5;
							continue;
						}
						goto IL_91;
					case 5:
					{
						IL_67:
						int[] uE;
						array[uE[num5]] = \uE0B9.\uE000(3);
						num2 = \uE020.\uE001(45);
						continue;
					}
					case 6:
					{
						int[] uE = \uE01B.\uE003;
						num2 = 0;
						continue;
					}
					case 7:
						num5++;
						goto IL_91;
					}
					break;
					IL_91:
					if (num5 >= num4)
					{
						goto Block_2;
					}
					goto IL_67;
				}
			}
			Block_2:
			\uE01E uE0BD = \uE01B.\uE005(\uE01B.\uE002(array));
			int[] array2;
			for (;;)
			{
				int num6 = \uE01A.\uE00D(46);
				for (;;)
				{
					int num7;
					switch (num6)
					{
					case 0:
						if (num7 != 0)
						{
							num6 = 3;
							continue;
						}
						goto IL_124;
					case 1:
						array2 = \uE020.\uE000.\uE00A(\uE0B9, uE0BD, num + num3);
						num6 = \uE01A.\uE00D(48);
						continue;
					case 2:
						\uE0BA = new \uE01D[num];
						num6 = 4;
						continue;
					case 3:
						IL_FE:
						\uE0BA[num7].\uE003 = array2[num7];
						num6 = 5;
						continue;
					case 4:
						num7 = 0;
						num6 = 0;
						continue;
					case 5:
						num7++;
						goto IL_124;
					}
					break;
					IL_124:
					if (num7 >= num)
					{
						goto Block_4;
					}
					goto IL_FE;
				}
			}
			Block_4:
			\uE01B.\uE003(\uE0BA);
			for (;;)
			{
				int num8 = \uE01A.\uE00D(48);
				for (;;)
				{
					int num9;
					switch (num8)
					{
					case 0:
						if (num9 != 0)
						{
							num8 = \uE01A.\uE00D(42);
							continue;
						}
						goto IL_1B6;
					case 1:
						num9 = 0;
						num8 = 0;
						continue;
					case 2:
						\uE0BB = new \uE01D[num3];
						num8 = 1;
						continue;
					case 3:
						IL_180:
						\uE0BB[num9].\uE003 = array2[num9 + num];
						num8 = 4;
						continue;
					case 4:
						num9++;
						num8 = 5;
						continue;
					case 5:
						goto IL_1B6;
					}
					break;
					IL_1B6:
					if (num9 >= num3)
					{
						goto Block_6;
					}
					goto IL_180;
				}
			}
			Block_6:
			\uE01B.\uE003(\uE0BB);
		}

		private static int[] \uE00A(\uE020.\uE001 \uE0BC, \uE01E \uE0BD, int \uE0BE)
		{
			int[] array = new int[\uE0BE];
			int num = 0;
			if (num == 0)
			{
				goto IL_12E;
			}
			IL_17:
			int num2 = \uE020.\uE000.\uE006(\uE0BC, \uE0BD);
			for (;;)
			{
				int num3 = \uE01A.\uE00D(49);
				for (;;)
				{
					switch (num3)
					{
					case 0:
						if (num2 < 16)
						{
							num3 = 1;
							continue;
						}
						goto IL_57;
					case 1:
						array[num] = num2;
						num3 = 2;
						continue;
					case 2:
						goto IL_52;
					}
					break;
				}
			}
			IL_52:
			goto IL_122;
			IL_57:
			if (num2 == 16)
			{
				int num4 = \uE0BC.\uE000(2) + 3;
				for (;;)
				{
					int num5 = \uE01A.\uE00D(48);
					for (;;)
					{
						int num6;
						switch (num5)
						{
						case 0:
							IL_8A:
							array[num + num6] = array[num - 1];
							num5 = 3;
							continue;
						case 1:
							if (num6 != 0)
							{
								num5 = 0;
								continue;
							}
							goto IL_B1;
						case 2:
							num6 = 0;
							num5 = 1;
							continue;
						case 3:
							num6++;
							goto IL_B1;
						}
						break;
						IL_B1:
						if (num6 >= num4)
						{
							goto Block_4;
						}
						goto IL_8A;
					}
				}
				Block_4:
				num += num4 - 1;
			}
			else if (num2 == 17)
			{
				for (;;)
				{
					int num7 = \uE01A.\uE00D(46);
					for (;;)
					{
						switch (num7)
						{
						case 0:
						{
							int num8;
							num += num8 - 1;
							num7 = 2;
							continue;
						}
						case 1:
						{
							int num8 = \uE0BC.\uE000(3) + 3;
							num7 = 0;
							continue;
						}
						case 2:
							goto IL_FF;
						}
						break;
					}
				}
				IL_FF:;
			}
			else if (num2 == 18)
			{
				int num9 = \uE0BC.\uE000(7) + 11;
				num += num9 - 1;
			}
			IL_122:
			num++;
			IL_12E:
			if (num >= \uE0BE)
			{
				return array;
			}
			goto IL_17;
		}
	}

	private sealed class \uE001
	{
		private uint \uE005;

		private int \uE006;

		private int \uE007;

		private Stream \uE008;

		internal long \uE009;

		internal \uE001(Stream \uE0BF)
		{
			this.\uE008 = \uE0BF;
		}

		internal int \uE000(int \uE0C0)
		{
			this.\uE009 += (long)\uE0C0;
			for (int i = \uE0C0 - (this.\uE007 - this.\uE006); i > 0; i -= 8)
			{
				this.\uE005 |= checked((uint)this.\uE008.ReadByte()) << this.\uE007;
				this.\uE007 += 8;
			}
			uint result = this.\uE005 >> this.\uE006 & (1u << \uE0C0) - 1u;
			this.\uE006 += \uE0C0;
			if (this.\uE007 == this.\uE006)
			{
				this.\uE007 = (this.\uE006 = 0);
				this.\uE005 = 0u;
				return (int)result;
			}
			if (this.\uE006 >= 8)
			{
				this.\uE005 >>= this.\uE006;
				this.\uE007 -= this.\uE006;
				this.\uE006 = 0;
			}
			return (int)result;
		}

		internal void \uE001()
		{
			if (this.\uE007 != this.\uE006)
			{
				this.\uE009 += (long)(this.\uE007 - this.\uE006);
			}
			this.\uE007 = (this.\uE006 = 0);
			this.\uE005 = 0u;
		}

		internal void \uE002(byte[] \uE0C1, int \uE0C2, int \uE0C3)
		{
			int num = this.\uE008.Read(\uE0C1, \uE0C2, \uE0C3);
			this.\uE009 += (long)((long)num << 3);
		}
	}

	private sealed class \uE002
	{
		private byte[] \uE004;

		private int \uE005;

		internal int \uE006;

		internal long \uE007;

		internal \uE002(int \uE0C4)
		{
			this.\uE006 = \uE0C4;
			this.\uE004 = new byte[\uE0C4];
		}

		internal void \uE000(byte \uE0C5)
		{
			byte[] uE = this.\uE004;
			int uE2 = this.\uE005;
			this.\uE005 = uE2 + 1;
			uE[uE2] = \uE0C5;
			if (this.\uE005 >= this.\uE006)
			{
				this.\uE005 = 0;
			}
			this.\uE007 += 1L;
		}

		internal void \uE001(byte[] \uE0C6, int \uE0C7, int \uE0C8)
		{
			this.\uE007 += (long)\uE0C8;
			if (\uE0C8 >= this.\uE006)
			{
				Array.Copy(\uE0C6, \uE0C7, this.\uE004, 0, this.\uE006);
				this.\uE005 = 0;
				return;
			}
			if (this.\uE005 + \uE0C8 > this.\uE006)
			{
				int num = this.\uE006 - this.\uE005;
				for (;;)
				{
					int num2 = \uE01A.\uE00D(49);
					for (;;)
					{
						switch (num2)
						{
						case 0:
						{
							int num3 = this.\uE005 + \uE0C8 - this.\uE006;
							num2 = 3;
							continue;
						}
						case 1:
						{
							int num3;
							Array.Copy(\uE0C6, \uE0C7 + num, this.\uE004, 0, num3);
							num2 = 2;
							continue;
						}
						case 2:
						{
							int num3;
							this.\uE005 = num3;
							num2 = 4;
							continue;
						}
						case 3:
							Array.Copy(\uE0C6, \uE0C7, this.\uE004, this.\uE005, num);
							num2 = \uE01A.\uE00D(46);
							continue;
						case 4:
							return;
						}
						break;
					}
				}
				return;
			}
			Array.Copy(\uE0C6, \uE0C7, this.\uE004, this.\uE005, \uE0C8);
			for (;;)
			{
				int num4 = \uE01A.\uE00D(49);
				for (;;)
				{
					switch (num4)
					{
					case 0:
						this.\uE005 += \uE0C8;
						num4 = 1;
						continue;
					case 1:
						if (this.\uE005 == this.\uE006)
						{
							num4 = 2;
							continue;
						}
						goto IL_131;
					case 2:
						goto IL_141;
					}
					break;
				}
			}
			IL_131:
			return;
			IL_141:
			this.\uE005 = 0;
		}

		internal byte[] \uE002(int \uE0C9, int \uE0CA)
		{
			byte[] array = new byte[\uE0CA];
			if (this.\uE005 >= \uE0C9)
			{
				Array.Copy(this.\uE004, this.\uE005 - \uE0C9, array, 0, \uE0CA);
			}
			else
			{
				int num = \uE0C9 - this.\uE005;
				for (;;)
				{
					int num2 = \uE01A.\uE00D(46);
					for (;;)
					{
						switch (num2)
						{
						case 0:
							Array.Copy(this.\uE004, this.\uE006 - num, array, 0, num);
							num2 = 2;
							continue;
						case 1:
							if (num < \uE0CA)
							{
								num2 = 0;
								continue;
							}
							goto IL_6A;
						case 2:
							Array.Copy(this.\uE004, 0, array, num, \uE0CA - num);
							num2 = \uE01A.\uE00D(42);
							continue;
						case 3:
							goto IL_AB;
						}
						break;
					}
				}
				IL_6A:
				Array.Copy(this.\uE004, this.\uE006 - num, array, 0, \uE0CA);
				IL_AB:;
			}
			return array;
		}
	}
}
