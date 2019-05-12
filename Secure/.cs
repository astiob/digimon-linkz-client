using System;
using System.IO;

internal sealed class \uE00E
{
	public static void \uE000(Stream \uE03B, Stream \uE03C)
	{
		byte[] array = new byte[4096];
		\uE00E.\uE000 uE = new \uE00E.\uE000(\uE03B);
		for (;;)
		{
			int num = uE.\uE000(array, 0, array.Length);
			for (;;)
			{
				int num2 = \uE008.\uE00D(49);
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
						\uE03C.Write(array, 0, num);
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

	internal static int \uE001(int \uE03D)
	{
		switch (\uE03D - ~(-(-(~-2147483605)) ^ -2147483648))
		{
		case 0:
			return ~(-(-(-(-(-6 ^ int.MinValue))) ^ int.MinValue));
		case 1:
			break;
		case 2:
			return -(-(-2147483641 ^ int.MinValue ^ int.MinValue ^ int.MinValue));
		default:
			if (\uE03D == ~(-(~(~(-(~(-2147483595 ^ -2147483648)))))))
			{
				return -(-(-(~10)));
			}
			if (\uE03D == ~(-(-(-(-2147483585 ^ -2147483648)))))
			{
				return ~(~(~(~8)));
			}
			break;
		}
		return ~(~(~(~(~int.MinValue) ^ int.MinValue)));
	}

	public sealed class \uE000
	{
		private \uE00E.\uE002 \uE00C = new \uE00E.\uE002(32769);

		private \uE00E.\uE001 \uE00D;

		private \uE00D \uE00E;

		private int \uE00F = -1;

		private int \uE010 = -1;

		private bool \uE011;

		private int \uE012;

		private long \uE013;

		private long \uE014;

		private bool \uE015;

		private int \uE016;

		private bool \uE017;

		public \uE000(Stream \uE03E)
		{
			this.\uE00D = new \uE00E.\uE001(\uE03E);
		}

		public int \uE000(byte[] \uE03F, int \uE040, int \uE041)
		{
			if (\uE041 == 0 || this.\uE015)
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
			int num2 = this.\uE002(\uE03F, \uE040 + num, \uE041 - num);
			for (;;)
			{
				int num3 = global::\uE008.\uE00D(49);
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
			if (num < \uE041)
			{
				goto IL_3E;
			}
			return num;
		}

		private bool \uE001()
		{
			if (((!this.\uE011) ? (~((2147483646 ^ -2147483648 ^ -2147483648) != 0) ^ -2147483648) : (~(-((-(2147483647 != 0) ^ -2147483648) != 0) != 0))) == 0)
			{
				return (~((int.MaxValue ^ int.MinValue) != 0) ^ int.MinValue ^ int.MinValue) != 0;
			}
			this.\uE013 = this.\uE00D.\uE009;
			for (;;)
			{
				int num = global::\uE008.\uE00D(-(~(~(2147483596 ^ int.MinValue))));
				for (;;)
				{
					switch (num)
					{
					case 0:
						this.\uE00F = this.\uE00D.\uE000(-(-(-2147483644) ^ int.MinValue));
						num = (-(~5) ^ int.MinValue ^ int.MinValue);
						continue;
					case 1:
						this.\uE017 = ((~(~(~(2147483646 != 0) != 0) != 0) ^ int.MinValue) != 0);
						num = -(~(~2147483640) ^ int.MinValue);
						continue;
					case 2:
						this.\uE00E = null;
						num = -(-(~(-2)));
						continue;
					case 3:
					{
						int uE = this.\uE00D.\uE000(~(~(-(-16)))) ^ (-(~(49015 ^ int.MinValue)) ^ int.MinValue);
						num = global::\uE00E.\uE001(-(~(~(~42))));
						continue;
					}
					case 4:
						this.\uE011 = (this.\uE00D.\uE000(~(-(2 ^ int.MinValue) ^ int.MinValue)) > -(-(0 ^ int.MinValue ^ int.MinValue)));
						num = -(-(~(int.MaxValue ^ int.MinValue)));
						continue;
					case 5:
					{
						int uE;
						this.\uE012 = uE;
						num = ~(~(2 ^ int.MinValue) ^ int.MinValue);
						continue;
					}
					case 6:
						if (this.\uE00F == ~(~(-(-12))))
						{
							num = (~(-8) ^ int.MinValue ^ int.MinValue);
							continue;
						}
						goto IL_24A;
					case 7:
						this.\uE00D.\uE001();
						num = -(-(~2147483644) ^ int.MinValue);
						continue;
					case 8:
						goto IL_245;
					}
					break;
				}
			}
			IL_245:
			goto IL_4A5;
			IL_24A:
			if (this.\uE00F == (~(2147483644 ^ -2147483648 ^ -2147483648) ^ -2147483648))
			{
				\uE00B[] uE2 = global::\uE009.\uE000;
				for (;;)
				{
					int num2 = global::\uE008.\uE00D(~(~(48 ^ int.MinValue) ^ int.MinValue));
					for (;;)
					{
						switch (num2)
						{
						case 0:
							this.\uE00E = global::\uE009.\uE002;
							num2 = ~(~(~2147483644 ^ int.MinValue));
							continue;
						case 1:
							this.\uE012 = -(-(~(~0)));
							num2 = (-(-0 ^ int.MinValue) ^ int.MinValue);
							continue;
						case 2:
						{
							\uE00B[] uE3 = global::\uE009.\uE001;
							num2 = (~(~(-int.MaxValue)) ^ int.MinValue);
							continue;
						}
						case 3:
							this.\uE017 = ((~(~((0 ^ int.MinValue) != 0) != 0) ^ int.MinValue) != 0);
							num2 = -(2147483644 ^ int.MinValue ^ int.MinValue ^ int.MinValue);
							continue;
						case 4:
							goto IL_36C;
						}
						break;
					}
				}
				IL_36C:;
			}
			else if (this.\uE00F == (-(2147483634 ^ -2147483648) ^ -2147483648 ^ -2147483648))
			{
				\uE00B[] uE2;
				\uE00B[] uE3;
				this.\uE009(this.\uE00D, out uE2, out uE3);
				for (;;)
				{
					int num3 = global::\uE008.\uE00D(-(2147483602 ^ int.MinValue) ^ int.MinValue ^ int.MinValue);
					for (;;)
					{
						switch (num3)
						{
						case 0:
							this.\uE00E = global::\uE009.\uE004(uE2, uE3);
							num3 = -(~(~(~1)));
							continue;
						case 1:
							this.\uE012 = ~(-1 ^ int.MinValue ^ int.MinValue);
							num3 = -(-(0 ^ int.MinValue ^ int.MinValue));
							continue;
						case 2:
							goto IL_460;
						}
						break;
					}
				}
				IL_460:
				this.\uE017 = -(-(-(-false)));
			}
			IL_4A5:
			this.\uE014 = this.\uE00D.\uE009;
			return (~(~((1 ^ int.MinValue) != 0) != 0) ^ int.MinValue) != 0;
		}

		private int \uE002(byte[] \uE042, int \uE043, int \uE044)
		{
			int num = \uE043;
			if (this.\uE00F == ~(~(~(-13))))
			{
				if (this.\uE012 > ~(~(~2147483647 ^ -2147483648)))
				{
					int num2 = Math.Min(\uE044, this.\uE012);
					for (;;)
					{
						int num3 = global::\uE008.\uE00D(~(-49 ^ int.MinValue) ^ int.MinValue);
						for (;;)
						{
							switch (num3)
							{
							case 0:
								\uE043 += num2;
								num3 = (~(-(6 ^ int.MinValue)) ^ int.MinValue);
								continue;
							case 1:
								this.\uE00C.\uE001(\uE042, \uE043, num2);
								num3 = global::\uE008.\uE00D(~2147483595 ^ int.MinValue ^ int.MinValue ^ int.MinValue);
								continue;
							case 2:
								this.\uE00D.\uE002(\uE042, \uE043, num2);
								num3 = (~(-(~2147483645)) ^ int.MinValue);
								continue;
							case 3:
								\uE044 -= num2;
								num3 = -(~(-(--1)));
								continue;
							case 4:
								this.\uE012 -= num2;
								num3 = ~(~(3 ^ int.MinValue ^ int.MinValue));
								continue;
							case 5:
								goto IL_185;
							}
							break;
						}
					}
					IL_185:;
				}
			}
			else if (((!this.\uE017) ? (-(-(--2147483648)) ^ -2147483648) : (-(-(-(-1))))) == 0)
			{
				if (this.\uE016 > (-(-2147483648 ^ -2147483648) ^ -2147483648 ^ -2147483648))
				{
					this.\uE003(\uE042, ref \uE043, ref \uE044);
				}
				if (\uE044 > ~(-(~2147483646) ^ -2147483648))
				{
					do
					{
						int num4 = global::\uE00E.\uE000.\uE006(this.\uE00D, this.\uE00E.\uE002);
						this.\uE017 = (num4 == -(-(-(~255))));
						if (((!this.\uE017) ? (-((~((-1 ^ -2147483648) != 0) ^ -2147483648) != 0)) : (-(-((1 ^ -2147483648) != 0) != 0) ^ -2147483648)) != 0)
						{
							break;
						}
						if (num4 < ~(-(~2147483390) ^ -2147483648))
						{
							int num5 = \uE043;
							\uE043 = num5 + -(int.MaxValue ^ int.MinValue ^ int.MinValue ^ int.MinValue);
							\uE042[num5] = (byte)num4;
							for (;;)
							{
								int num6 = global::\uE008.\uE00D(-(-(-2147483602 ^ int.MinValue)));
								for (;;)
								{
									switch (num6)
									{
									case 0:
										\uE044 -= -(-(1 ^ int.MinValue) ^ int.MinValue);
										num6 = ~(-(-(2147483645 ^ int.MinValue)));
										continue;
									case 1:
										this.\uE00C.\uE000((byte)num4);
										num6 = -(~(~int.MinValue) ^ int.MinValue);
										continue;
									case 2:
										goto IL_361;
									}
									break;
								}
							}
							IL_361:;
						}
						else if (num4 <= (-(-(285 ^ -2147483648)) ^ -2147483648))
						{
							int uE = global::\uE00E.\uE000.\uE007(this.\uE00D, num4);
							for (;;)
							{
								int num7 = global::\uE008.\uE00D(-(-(-2147483600 ^ int.MinValue)));
								for (;;)
								{
									switch (num7)
									{
									case 0:
										this.\uE016 = uE;
										num7 = (~(~(-2147483645)) ^ int.MinValue);
										continue;
									case 1:
									{
										int uE2;
										this.\uE010 = uE2;
										num7 = -(-(-int.MinValue) ^ int.MinValue);
										continue;
									}
									case 2:
									{
										int uE2 = global::\uE00E.\uE000.\uE008(this.\uE00D, this.\uE00E.\uE003);
										num7 = global::\uE008.\uE00D(-(-(-(2147483602 ^ int.MinValue))));
										continue;
									}
									case 3:
										goto IL_456;
									}
									break;
								}
							}
							IL_456:
							this.\uE003(\uE042, ref \uE043, ref \uE044);
						}
					}
					while (\uE044 > -(-(-(-0))));
				}
			}
			this.\uE014 = this.\uE00D.\uE009;
			return \uE043 - num;
		}

		private void \uE003(byte[] \uE045, ref int \uE046, ref int \uE047)
		{
			int i = Math.Min(this.\uE016, \uE047);
			byte[] array = this.\uE00C.\uE002(this.\uE010, Math.Min(i, this.\uE010));
			for (;;)
			{
				int num = global::\uE008.\uE00D(46);
				for (;;)
				{
					switch (num)
					{
					case 0:
						this.\uE016 -= i;
						num = 2;
						continue;
					case 1:
						\uE047 -= i;
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
				Array.Copy(array, 0, \uE045, \uE046, array.Length);
				for (;;)
				{
					int num2 = global::\uE008.\uE00D(46);
					for (;;)
					{
						switch (num2)
						{
						case 0:
							i -= array.Length;
							num2 = 2;
							continue;
						case 1:
							\uE046 += array.Length;
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
			Array.Copy(array, 0, \uE045, \uE046, i);
			\uE046 += i;
			this.\uE00C.\uE001(array, 0, i);
		}

		public bool \uE004(int \uE048)
		{
			byte[] uE03F = new byte[1024];
			while (\uE048 > 0)
			{
				int num;
				if ((num = this.\uE000(uE03F, 0, Math.Min(1024, \uE048))) <= 0)
				{
					break;
				}
				\uE048 -= num;
			}
			return \uE048 <= 0;
		}

		public void \uE005()
		{
			byte[] uE03F = new byte[1024];
			while (this.\uE000(uE03F, 0, 1024) > 0)
			{
			}
		}

		private static int \uE006(\uE00E.\uE001 \uE049, \uE00C \uE04A)
		{
			while (\uE04A != null && !\uE04A.\uE004)
			{
				\uE04A = ((\uE049.\uE000(1) > 0) ? \uE04A.\uE007 : \uE04A.\uE006);
			}
			return (int)\uE04A.\uE005;
		}

		private static int \uE007(\uE00E.\uE001 \uE04B, int \uE04C)
		{
			int num;
			int num2;
			global::\uE009.\uE007(\uE04C, out num, out num2);
			if (num2 > 0)
			{
				return num + \uE04B.\uE000(num2);
			}
			return num;
		}

		private static int \uE008(\uE00E.\uE001 \uE04D, \uE00C \uE04E)
		{
			int num = global::\uE00E.\uE000.\uE006(\uE04D, \uE04E);
			int num3;
			int num4;
			for (;;)
			{
				int num2 = global::\uE008.\uE00D(49);
				for (;;)
				{
					switch (num2)
					{
					case 0:
						num3 = global::\uE009.\uE006[num];
						num2 = 3;
						continue;
					case 1:
					{
						int num5;
						num4 = \uE04D.\uE000(num5);
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
						int num5 = global::\uE009.\uE007[num];
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

		private void \uE009(\uE00E.\uE001 \uE04F, out \uE00B[] \uE050, out \uE00B[] \uE051)
		{
			int num = \uE04F.\uE000(5) + 257;
			int[] array;
			int num3;
			for (;;)
			{
				int num2 = global::\uE008.\uE00D(46);
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
						num3 = \uE04F.\uE000(5) + 1;
						num2 = 2;
						continue;
					case 2:
						num4 = \uE04F.\uE000(4) + 4;
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
						array[uE[num5]] = \uE04F.\uE000(3);
						num2 = global::\uE00E.\uE001(45);
						continue;
					}
					case 6:
					{
						int[] uE = global::\uE009.\uE003;
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
			\uE00C uE2 = global::\uE009.\uE005(global::\uE009.\uE002(array));
			int[] array2;
			for (;;)
			{
				int num6 = global::\uE008.\uE00D(46);
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
						array2 = global::\uE00E.\uE000.\uE00A(\uE04F, uE2, num + num3);
						num6 = global::\uE008.\uE00D(48);
						continue;
					case 2:
						\uE050 = new \uE00B[num];
						num6 = 4;
						continue;
					case 3:
						IL_FE:
						\uE050[num7].\uE003 = array2[num7];
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
			global::\uE009.\uE003(\uE050);
			for (;;)
			{
				int num8 = global::\uE008.\uE00D(48);
				for (;;)
				{
					int num9;
					switch (num8)
					{
					case 0:
						if (num9 != 0)
						{
							num8 = global::\uE008.\uE00D(42);
							continue;
						}
						goto IL_1B6;
					case 1:
						num9 = 0;
						num8 = 0;
						continue;
					case 2:
						\uE051 = new \uE00B[num3];
						num8 = 1;
						continue;
					case 3:
						IL_180:
						\uE051[num9].\uE003 = array2[num9 + num];
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
			global::\uE009.\uE003(\uE051);
		}

		private static int[] \uE00A(\uE00E.\uE001 \uE052, \uE00C \uE053, int \uE054)
		{
			int[] array = new int[\uE054];
			int num = 0;
			if (num == 0)
			{
				goto IL_12E;
			}
			IL_17:
			int num2 = global::\uE00E.\uE000.\uE006(\uE052, \uE053);
			for (;;)
			{
				int num3 = global::\uE008.\uE00D(49);
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
				int num4 = \uE052.\uE000(2) + 3;
				for (;;)
				{
					int num5 = global::\uE008.\uE00D(48);
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
					int num7 = global::\uE008.\uE00D(46);
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
							int num8 = \uE052.\uE000(3) + 3;
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
				int num9 = \uE052.\uE000(7) + 11;
				num += num9 - 1;
			}
			IL_122:
			num++;
			IL_12E:
			if (num >= \uE054)
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

		internal \uE001(Stream \uE055)
		{
			this.\uE008 = \uE055;
		}

		internal int \uE000(int \uE056)
		{
			this.\uE009 += (long)\uE056;
			for (int i = \uE056 - (this.\uE007 - this.\uE006); i > 0; i -= 8)
			{
				this.\uE005 |= checked((uint)this.\uE008.ReadByte()) << this.\uE007;
				this.\uE007 += 8;
			}
			uint result = this.\uE005 >> this.\uE006 & (1u << \uE056) - 1u;
			this.\uE006 += \uE056;
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

		internal void \uE002(byte[] \uE057, int \uE058, int \uE059)
		{
			int num = this.\uE008.Read(\uE057, \uE058, \uE059);
			this.\uE009 += (long)((long)num << 3);
		}
	}

	private sealed class \uE002
	{
		private byte[] \uE004;

		private int \uE005;

		internal int \uE006;

		internal long \uE007;

		internal \uE002(int \uE05A)
		{
			this.\uE006 = \uE05A;
			this.\uE004 = new byte[\uE05A];
		}

		internal void \uE000(byte \uE05B)
		{
			byte[] uE = this.\uE004;
			int uE2 = this.\uE005;
			this.\uE005 = uE2 + 1;
			uE[uE2] = \uE05B;
			if (this.\uE005 >= this.\uE006)
			{
				this.\uE005 = 0;
			}
			this.\uE007 += 1L;
		}

		internal void \uE001(byte[] \uE05C, int \uE05D, int \uE05E)
		{
			this.\uE007 += (long)\uE05E;
			if (\uE05E >= this.\uE006)
			{
				Array.Copy(\uE05C, \uE05D, this.\uE004, 0, this.\uE006);
				this.\uE005 = 0;
				return;
			}
			if (this.\uE005 + \uE05E > this.\uE006)
			{
				int num = this.\uE006 - this.\uE005;
				for (;;)
				{
					int num2 = \uE008.\uE00D(49);
					for (;;)
					{
						switch (num2)
						{
						case 0:
						{
							int num3 = this.\uE005 + \uE05E - this.\uE006;
							num2 = 3;
							continue;
						}
						case 1:
						{
							int num3;
							Array.Copy(\uE05C, \uE05D + num, this.\uE004, 0, num3);
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
							Array.Copy(\uE05C, \uE05D, this.\uE004, this.\uE005, num);
							num2 = \uE008.\uE00D(46);
							continue;
						case 4:
							return;
						}
						break;
					}
				}
				return;
			}
			Array.Copy(\uE05C, \uE05D, this.\uE004, this.\uE005, \uE05E);
			for (;;)
			{
				int num4 = \uE008.\uE00D(49);
				for (;;)
				{
					switch (num4)
					{
					case 0:
						this.\uE005 += \uE05E;
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

		internal byte[] \uE002(int \uE05F, int \uE060)
		{
			byte[] array = new byte[\uE060];
			if (this.\uE005 >= \uE05F)
			{
				Array.Copy(this.\uE004, this.\uE005 - \uE05F, array, 0, \uE060);
			}
			else
			{
				int num = \uE05F - this.\uE005;
				for (;;)
				{
					int num2 = \uE008.\uE00D(46);
					for (;;)
					{
						switch (num2)
						{
						case 0:
							Array.Copy(this.\uE004, this.\uE006 - num, array, 0, num);
							num2 = 2;
							continue;
						case 1:
							if (num < \uE060)
							{
								num2 = 0;
								continue;
							}
							goto IL_6A;
						case 2:
							Array.Copy(this.\uE004, 0, array, num, \uE060 - num);
							num2 = \uE008.\uE00D(42);
							continue;
						case 3:
							goto IL_AB;
						}
						break;
					}
				}
				IL_6A:
				Array.Copy(this.\uE004, this.\uE006 - num, array, 0, \uE060);
				IL_AB:;
			}
			return array;
		}
	}
}
