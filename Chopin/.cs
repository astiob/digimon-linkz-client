using System;
using System.IO;

internal sealed class \uE026
{
	public static void \uE000(Stream \uE000, Stream \uE001)
	{
		byte[] array = new byte[4096];
		\uE026.\uE027 uE = new \uE026.\uE027(\uE000);
		for (;;)
		{
			int num = uE.\uE000(array, 0, array.Length);
			for (;;)
			{
				int num2 = \uE01E.\uE006(92);
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
						\uE001.Write(array, 0, num);
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

	internal static int \uE001(int \uE000)
	{
		switch (\uE000 - (-(~(~1396229864) ^ 1038770928) ^ -1859298370))
		{
		case 0:
			return -(-(~-361765888 ^ 361765876));
		case 1:
			return ~(-(-(717580065 ^ 444224431)) ^ -817837193);
		case 2:
			return ~(~(~-838819203 ^ -1294205950) ^ 1801666544 ^ -1227927541) ^ 1586024063;
		case 3:
		case 4:
			break;
		case 5:
			return ~(~(--469492998 ^ -96464865) ^ 716439816) ^ -888146941;
		default:
			switch (\uE000 - ~(~(~(~212861649 ^ 212861618))))
			{
			case 0:
				return ~(-(~(~1639872911))) ^ -96937460 ^ -1685675129;
			case 2:
				return -(~(~728259878 ^ -1975430901) ^ -1047985186) ^ -1621102587;
			}
			break;
		}
		return -(-(-(391314195 ^ -882848157 ^ -600689295)));
	}

	public sealed class \uE027
	{
		private \uE026.\uE029 \uE000 = new \uE026.\uE029(32769);

		private \uE026.\uE028 \uE001;

		private \uE025 \uE002;

		private int \uE003 = -1;

		private int \uE004 = -1;

		private bool \uE005;

		private int \uE006;

		private long \uE007;

		private long \uE008;

		private bool \uE009;

		private int \uE00A;

		private bool \uE00B;

		public \uE027(Stream \uE000)
		{
			this.\uE001 = new \uE026.\uE028(\uE000);
		}

		public int \uE000(byte[] \uE000, int \uE001, int \uE002)
		{
			if (\uE002 == 0 || this.\uE009)
			{
				return 0;
			}
			int num = 0;
			if (num == 0)
			{
				goto IL_C3;
			}
			IL_2F:
			this.\uE009 = !this.\uE001();
			IL_3E:
			if (this.\uE003 < 0 && !this.\uE009)
			{
				goto IL_2F;
			}
			if (this.\uE009)
			{
				return num;
			}
			int num2 = this.\uE002(\uE000, \uE001 + num, \uE002 - num);
			for (;;)
			{
				int num3 = \uE01E.\uE006(92);
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
			this.\uE003 = -1;
			IL_C3:
			if (num < \uE002)
			{
				goto IL_3E;
			}
			return num;
		}

		private bool \uE001()
		{
			if (((!this.\uE005) ? (-(~((-1317731740 ^ -1235979566) != 0) != 0) ^ 119632054) : (~(1710535630 != 0) ^ -81916148 ^ -2040482495 ^ -411699588)) == 0)
			{
				return ~((~(227440978 != 0) ^ 1265065429 ^ 1189684359) != 0) != 0;
			}
			this.\uE007 = this.\uE001.\uE004;
			for (;;)
			{
				int num = \uE01E.\uE006(~(--1683559245 ^ -46307249) ^ 1721468064);
				for (;;)
				{
					switch (num)
					{
					case 0:
						this.\uE001.\uE001();
						num = (-(-(-1077335103)) ^ -1077335104);
						continue;
					case 1:
					{
						int uE = this.\uE001.\uE000(~(-(~(~17)))) ^ -(-(~(16808915 ^ -16799763)));
						num = ~(-(686746239 ^ -114886293) ^ -775417065);
						continue;
					}
					case 2:
						this.\uE005 = (this.\uE001.\uE000(~(-(~(~2)))) > (-(-(2016855988 ^ 935113586)) ^ 1334473926));
						num = -(-(-(-6)));
						continue;
					case 3:
						if (this.\uE003 == (~(-513407598) ^ -37175365 ^ -481212976))
						{
							num = -(-(-1266291926) ^ 1266291926);
							continue;
						}
						goto IL_1E9;
					case 4:
					{
						int uE;
						this.\uE006 = uE;
						num = -(-(~-61717564 ^ 61717564));
						continue;
					}
					case 5:
						this.\uE00B = ((-(~((-1659994485 ^ 202271632) != 0) != 0) ^ -1862265571) != 0);
						num = ~(~(-(1326981542 ^ -1326981538)));
						continue;
					case 6:
						this.\uE003 = this.\uE001.\uE000(-(~(--1183373318)) ^ 1183373315);
						num = \uE01E.\uE006(-(-(-961602089 ^ 798811956)) ^ -382469443);
						continue;
					case 7:
						this.\uE002 = null;
						num = ~(-(-1996277888 ^ 2131546380 ^ -166730102));
						continue;
					case 8:
						goto IL_1E4;
					}
					break;
				}
			}
			IL_1E4:
			goto IL_3D5;
			IL_1E9:
			if (this.\uE003 == (~(-(~-1139528615)) ^ 1139528623))
			{
				\uE023[] uE2 = \uE01F.\uE000;
				for (;;)
				{
					int num2 = \uE01E.\uE006(-(-(~2120494271) ^ -2120494237));
					for (;;)
					{
						switch (num2)
						{
						case 0:
							this.\uE00B = (~(-(~(-(2 != 0) != 0) != 0) != 0) != 0);
							num2 = (-48471334 ^ 1090423864 ^ 1206690677 ^ -99725933);
							continue;
						case 1:
							this.\uE006 = (~(~(189441729 ^ 1278648372)) ^ 1199310581);
							num2 = \uE01E.\uE006(~(~(~-355956236 ^ 355956309)));
							continue;
						case 2:
						{
							\uE023[] uE3 = \uE01F.\uE001;
							num2 = (-(1228106584 ^ -971574659) ^ 287036502 ^ 1640084108);
							continue;
						}
						case 3:
							this.\uE002 = \uE01F.\uE002;
							num2 = ~(-1270529435 ^ -2105196977 ^ -918584363);
							continue;
						case 4:
							goto IL_2E0;
						}
						break;
					}
				}
				IL_2E0:;
			}
			else if (this.\uE003 == -(-1113845221 ^ 707117483 ^ 1749429320))
			{
				\uE023[] uE2;
				\uE023[] uE3;
				this.\uE009(this.\uE001, out uE2, out uE3);
				for (;;)
				{
					int num3 = \uE01E.\uE006(-(-(~(2007315936 ^ -2007315901))));
					for (;;)
					{
						switch (num3)
						{
						case 0:
							this.\uE006 = -(-(~(-1)));
							num3 = (-(-(~-165510107)) ^ 165510107);
							continue;
						case 1:
							this.\uE002 = \uE01F.\uE004(uE2, uE3);
							num3 = (-(~(207247651 ^ -909445155)) ^ -980373763);
							continue;
						case 2:
							goto IL_38C;
						}
						break;
					}
				}
				IL_38C:
				this.\uE00B = ((~(~(-172348367 != 0) != 0) ^ 259409752 ^ -87266455) != 0);
			}
			IL_3D5:
			this.\uE008 = this.\uE001.\uE004;
			return ~((1392205356 ^ 1387029594 ^ -1419792917 ^ 1425500259) != 0) != 0;
		}

		private int \uE002(byte[] \uE000, int \uE001, int \uE002)
		{
			int num = \uE001;
			if (this.\uE003 == (--1373467841 ^ -113930813 ^ 911648341 ^ -1631690927))
			{
				if (this.\uE006 > ~(~(2019639942 ^ 1165923473 ^ 1025496599)))
				{
					int num2 = Math.Min(\uE002, this.\uE006);
					for (;;)
					{
						int num3 = \uE01E.\uE006(~(~1719412029) ^ 1433437986 ^ 856465986);
						for (;;)
						{
							switch (num3)
							{
							case 0:
								\uE002 -= num2;
								num3 = ~(~(-(-1)));
								continue;
							case 1:
								\uE001 += num2;
								num3 = -(-(~-1120153039) ^ 1120153033);
								continue;
							case 2:
								this.\uE001.\uE002(\uE000, \uE001, num2);
								num3 = \uE026.\uE001(~(~(-(-88))));
								continue;
							case 3:
								this.\uE006 -= num2;
								num3 = -(-(~(~0)));
								continue;
							case 4:
								this.\uE000.\uE001(\uE000, \uE001, num2);
								num3 = -(~(-1422633445 ^ 1596662138 ^ -199264413));
								continue;
							case 5:
								goto IL_11C;
							}
							break;
						}
					}
					IL_11C:;
				}
			}
			else if (((!this.\uE00B) ? (-((~(1773980648 != 0) ^ 559547179 ^ -1223084228) != 0)) : (~(~(-(383797689 != 0) != 0) != 0) ^ -383797690)) == 0)
			{
				if (this.\uE00A > (~(-2031201020 ^ 1068309286 ^ -454465955) ^ -1571432576))
				{
					this.\uE003(\uE000, ref \uE001, ref \uE002);
				}
				if (\uE002 > -(-(500647104 ^ -1675036559 ^ -2113946447)))
				{
					do
					{
						int num4 = \uE026.\uE027.\uE006(this.\uE001, this.\uE002.\uE000);
						this.\uE00B = (num4 == ~(-1198118015 ^ 1343670734 ^ 1534756951 ^ 1275434215));
						if (((!this.\uE00B) ? (~((-(-(1968240366 != 0) != 0) ^ -1968240367) != 0)) : (~(-(-(978254232 != 0) != 0) != 0) ^ -978254234)) != 0)
						{
							break;
						}
						if (num4 < ~(-(-2003075054 ^ -1031994272 ^ 1256621427)))
						{
							int num5 = \uE001;
							\uE001 = num5 + (~(~-989791990) ^ 987685088 ^ -2221589);
							\uE000[num5] = (byte)num4;
							for (;;)
							{
								int num6 = \uE01E.\uE006(~(~(~(~92))));
								for (;;)
								{
									switch (num6)
									{
									case 0:
										this.\uE000.\uE000((byte)num4);
										num6 = -(-(172399969 ^ 1556110832) ^ 1451671696);
										continue;
									case 1:
										\uE002 -= ~(-(~(~2)));
										num6 = -(~(-(~0)));
										continue;
									case 2:
										goto IL_28F;
									}
									break;
								}
							}
							IL_28F:;
						}
						else if (num4 <= ~(-(~(-287))))
						{
							int uE00A = \uE026.\uE027.\uE007(this.\uE001, num4);
							for (;;)
							{
								int num7 = \uE01E.\uE006(~(~(-(~92))));
								for (;;)
								{
									switch (num7)
									{
									case 0:
										this.\uE00A = uE00A;
										num7 = \uE01E.\uE006(-1063322125 ^ 650305627 ^ 1190867188 ^ -1599656702);
										continue;
									case 1:
									{
										int uE;
										this.\uE004 = uE;
										num7 = -(~(1714069951 ^ 1136747652) ^ -636208444);
										continue;
									}
									case 2:
									{
										int uE = \uE026.\uE027.\uE008(this.\uE001, this.\uE002.\uE001);
										num7 = (~(-(~-310612545)) ^ 310612542);
										continue;
									}
									case 3:
										goto IL_34C;
									}
									break;
								}
							}
							IL_34C:
							this.\uE003(\uE000, ref \uE001, ref \uE002);
						}
					}
					while (\uE002 > (~(393901320 ^ 1513527409) ^ 1632564031 ^ -738336327));
				}
			}
			this.\uE008 = this.\uE001.\uE004;
			return \uE001 - num;
		}

		private void \uE003(byte[] \uE000, ref int \uE001, ref int \uE002)
		{
			int i = Math.Min(this.\uE00A, \uE002);
			byte[] array = this.\uE000.\uE002(this.\uE004, Math.Min(i, this.\uE004));
			for (;;)
			{
				int num = \uE01E.\uE006(90);
				for (;;)
				{
					switch (num)
					{
					case 0:
						this.\uE00A -= i;
						num = 2;
						continue;
					case 1:
						\uE002 -= i;
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
				Array.Copy(array, 0, \uE000, \uE001, array.Length);
				for (;;)
				{
					int num2 = \uE01E.\uE006(90);
					for (;;)
					{
						switch (num2)
						{
						case 0:
							i -= array.Length;
							num2 = 2;
							continue;
						case 1:
							\uE001 += array.Length;
							num2 = 0;
							continue;
						case 2:
							goto IL_A1;
						}
						break;
					}
				}
				IL_A1:
				this.\uE000.\uE001(array, 0, array.Length);
			}
			Array.Copy(array, 0, \uE000, \uE001, i);
			\uE001 += i;
			this.\uE000.\uE001(array, 0, i);
		}

		public bool \uE004(int \uE000)
		{
			byte[] uE = new byte[1024];
			while (\uE000 > 0)
			{
				int num;
				if ((num = this.\uE000(uE, 0, Math.Min(1024, \uE000))) <= 0)
				{
					break;
				}
				\uE000 -= num;
			}
			return \uE000 <= 0;
		}

		public void \uE005()
		{
			byte[] uE = new byte[1024];
			while (this.\uE000(uE, 0, 1024) > 0)
			{
			}
		}

		private static int \uE006(\uE026.\uE028 \uE000, \uE024 \uE001)
		{
			while (\uE001 != null && !\uE001.\uE000)
			{
				\uE001 = ((\uE000.\uE000(1) > 0) ? \uE001.\uE003 : \uE001.\uE002);
			}
			return (int)\uE001.\uE001;
		}

		private static int \uE007(\uE026.\uE028 \uE000, int \uE001)
		{
			int num;
			int num2;
			\uE01F.\uE007(\uE001, out num, out num2);
			if (num2 > 0)
			{
				return num + \uE000.\uE000(num2);
			}
			return num;
		}

		private static int \uE008(\uE026.\uE028 \uE000, \uE024 \uE001)
		{
			int num = \uE026.\uE027.\uE006(\uE000, \uE001);
			int num3;
			int num4;
			for (;;)
			{
				int num2 = \uE01E.\uE006(92);
				for (;;)
				{
					switch (num2)
					{
					case 0:
						num3 = \uE01F.\uE006[num];
						num2 = 3;
						continue;
					case 1:
					{
						int num5;
						num4 = \uE000.\uE000(num5);
						num2 = 4;
						continue;
					}
					case 2:
					{
						int num5;
						if (num5 > 0)
						{
							num2 = \uE01E.\uE006(90);
							continue;
						}
						return num3;
					}
					case 3:
					{
						int num5 = \uE01F.\uE007[num];
						num2 = 2;
						continue;
					}
					case 4:
						goto IL_75;
					}
					break;
				}
			}
			return num3;
			IL_75:
			return num3 + num4;
		}

		private void \uE009(\uE026.\uE028 \uE000, out \uE023[] \uE001, out \uE023[] \uE002)
		{
			int num = \uE000.\uE000(5) + 257;
			int[] array;
			int num5;
			for (;;)
			{
				int num2 = \uE026.\uE001(87);
				for (;;)
				{
					int num3;
					int num4;
					switch (num2)
					{
					case 0:
						array = new int[19];
						num2 = 2;
						continue;
					case 1:
					{
						IL_34:
						int[] uE;
						array[uE[num3]] = \uE000.\uE000(3);
						num2 = 7;
						continue;
					}
					case 2:
						num3 = 0;
						num2 = 4;
						continue;
					case 3:
					{
						int[] uE = \uE01F.\uE003;
						num2 = \uE01E.\uE006(92);
						continue;
					}
					case 4:
						if (num3 != 0)
						{
							num2 = 1;
							continue;
						}
						goto IL_91;
					case 5:
						num4 = \uE000.\uE000(4) + 4;
						num2 = 3;
						continue;
					case 6:
						num5 = \uE000.\uE000(5) + 1;
						num2 = 5;
						continue;
					case 7:
						num3++;
						goto IL_91;
					}
					break;
					IL_91:
					if (num3 >= num4)
					{
						goto Block_2;
					}
					goto IL_34;
				}
			}
			Block_2:
			\uE024 uE2 = \uE01F.\uE005(\uE01F.\uE002(array));
			int[] array2;
			for (;;)
			{
				int num6 = \uE01E.\uE006(90);
				for (;;)
				{
					int num7;
					switch (num6)
					{
					case 0:
						if (num7 != 0)
						{
							num6 = 2;
							continue;
						}
						goto IL_124;
					case 1:
						array2 = \uE026.\uE027.\uE00A(\uE000, uE2, num + num5);
						num6 = 4;
						continue;
					case 2:
						IL_EB:
						\uE001[num7].\uE001 = array2[num7];
						num6 = 5;
						continue;
					case 3:
						num7 = 0;
						num6 = \uE01E.\uE006(92);
						continue;
					case 4:
						\uE001 = new \uE023[num];
						num6 = 3;
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
					goto IL_EB;
				}
			}
			Block_4:
			\uE01F.\uE003(\uE001);
			for (;;)
			{
				int num8 = \uE01E.\uE006(90);
				for (;;)
				{
					int num9;
					switch (num8)
					{
					case 0:
						IL_15C:
						\uE002[num9].\uE001 = array2[num9 + num];
						num8 = 2;
						continue;
					case 1:
						\uE002 = new \uE023[num5];
						num8 = 4;
						continue;
					case 2:
						num9++;
						num8 = \uE026.\uE001(99);
						continue;
					case 3:
						if (num9 != 0)
						{
							num8 = 0;
							continue;
						}
						goto IL_1B6;
					case 4:
						num9 = 0;
						num8 = 3;
						continue;
					case 5:
						goto IL_1B6;
					}
					break;
					IL_1B6:
					if (num9 >= num5)
					{
						goto Block_6;
					}
					goto IL_15C;
				}
			}
			Block_6:
			\uE01F.\uE003(\uE002);
		}

		private static int[] \uE00A(\uE026.\uE028 \uE000, \uE024 \uE001, int \uE002)
		{
			int[] array = new int[\uE002];
			int num = 0;
			if (num == 0)
			{
				goto IL_12E;
			}
			IL_17:
			int num2 = \uE026.\uE027.\uE006(\uE000, \uE001);
			for (;;)
			{
				int num3 = \uE01E.\uE006(92);
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
				int num4 = \uE000.\uE000(2) + 3;
				for (;;)
				{
					int num5 = \uE01E.\uE006(90);
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
							num6 = 0;
							num5 = 2;
							continue;
						case 2:
							if (num6 != 0)
							{
								num5 = 0;
								continue;
							}
							goto IL_B1;
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
					int num7 = \uE01E.\uE006(92);
					for (;;)
					{
						switch (num7)
						{
						case 0:
						{
							int num8 = \uE000.\uE000(3) + 3;
							num7 = 1;
							continue;
						}
						case 1:
						{
							int num8;
							num += num8 - 1;
							num7 = 2;
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
				int num9 = \uE000.\uE000(7) + 11;
				num += num9 - 1;
			}
			IL_122:
			num++;
			IL_12E:
			if (num >= \uE002)
			{
				return array;
			}
			goto IL_17;
		}
	}

	private sealed class \uE028
	{
		private uint \uE000;

		private int \uE001;

		private int \uE002;

		private Stream \uE003;

		internal long \uE004;

		internal \uE028(Stream \uE000)
		{
			this.\uE003 = \uE000;
		}

		internal int \uE000(int \uE000)
		{
			this.\uE004 += (long)\uE000;
			for (int i = \uE000 - (this.\uE002 - this.\uE001); i > 0; i -= 8)
			{
				this.\uE000 |= checked((uint)this.\uE003.ReadByte()) << this.\uE002;
				this.\uE002 += 8;
			}
			uint result = this.\uE000 >> this.\uE001 & (1u << \uE000) - 1u;
			this.\uE001 += \uE000;
			if (this.\uE002 == this.\uE001)
			{
				this.\uE002 = (this.\uE001 = 0);
				this.\uE000 = 0u;
				return (int)result;
			}
			if (this.\uE001 >= 8)
			{
				this.\uE000 >>= this.\uE001;
				this.\uE002 -= this.\uE001;
				this.\uE001 = 0;
			}
			return (int)result;
		}

		internal void \uE001()
		{
			if (this.\uE002 != this.\uE001)
			{
				this.\uE004 += (long)(this.\uE002 - this.\uE001);
			}
			this.\uE002 = (this.\uE001 = 0);
			this.\uE000 = 0u;
		}

		internal void \uE002(byte[] \uE000, int \uE001, int \uE002)
		{
			int num = this.\uE003.Read(\uE000, \uE001, \uE002);
			this.\uE004 += (long)((long)num << 3);
		}
	}

	private sealed class \uE029
	{
		private byte[] \uE000;

		private int \uE001;

		internal int \uE002;

		internal long \uE003;

		internal \uE029(int \uE000)
		{
			this.\uE002 = \uE000;
			this.\uE000 = new byte[\uE000];
		}

		internal void \uE000(byte \uE000)
		{
			byte[] uE = this.\uE000;
			int uE2 = this.\uE001;
			this.\uE001 = uE2 + 1;
			uE[uE2] = \uE000;
			if (this.\uE001 >= this.\uE002)
			{
				this.\uE001 = 0;
			}
			this.\uE003 += 1L;
		}

		internal void \uE001(byte[] \uE000, int \uE001, int \uE002)
		{
			this.\uE003 += (long)\uE002;
			if (\uE002 >= this.\uE002)
			{
				Array.Copy(\uE000, \uE001, this.\uE000, 0, this.\uE002);
				this.\uE001 = 0;
				return;
			}
			if (this.\uE001 + \uE002 > this.\uE002)
			{
				int num = this.\uE002 - this.\uE001;
				for (;;)
				{
					int num2 = \uE01E.\uE006(93);
					for (;;)
					{
						switch (num2)
						{
						case 0:
						{
							int num3;
							this.\uE001 = num3;
							num2 = 4;
							continue;
						}
						case 1:
							Array.Copy(\uE000, \uE001, this.\uE000, this.\uE001, num);
							num2 = 3;
							continue;
						case 2:
						{
							int num3 = this.\uE001 + \uE002 - this.\uE002;
							num2 = \uE01E.\uE006(90);
							continue;
						}
						case 3:
						{
							int num3;
							Array.Copy(\uE000, \uE001 + num, this.\uE000, 0, num3);
							num2 = 0;
							continue;
						}
						case 4:
							return;
						}
						break;
					}
				}
				return;
			}
			Array.Copy(\uE000, \uE001, this.\uE000, this.\uE001, \uE002);
			for (;;)
			{
				int num4 = \uE01E.\uE006(92);
				for (;;)
				{
					switch (num4)
					{
					case 0:
						this.\uE001 += \uE002;
						num4 = 1;
						continue;
					case 1:
						if (this.\uE001 == this.\uE002)
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
			this.\uE001 = 0;
		}

		internal byte[] \uE002(int \uE000, int \uE001)
		{
			byte[] array = new byte[\uE001];
			if (this.\uE001 >= \uE000)
			{
				Array.Copy(this.\uE000, this.\uE001 - \uE000, array, 0, \uE001);
			}
			else
			{
				int num = \uE000 - this.\uE001;
				for (;;)
				{
					int num2 = \uE01E.\uE006(93);
					for (;;)
					{
						switch (num2)
						{
						case 0:
							Array.Copy(this.\uE000, 0, array, num, \uE001 - num);
							num2 = 3;
							continue;
						case 1:
							Array.Copy(this.\uE000, this.\uE002 - num, array, 0, num);
							num2 = \uE01E.\uE006(92);
							continue;
						case 2:
							if (num < \uE001)
							{
								num2 = 1;
								continue;
							}
							goto IL_9E;
						case 3:
							goto IL_AB;
						}
						break;
					}
				}
				IL_9E:
				Array.Copy(this.\uE000, this.\uE002 - num, array, 0, \uE001);
				IL_AB:;
			}
			return array;
		}
	}
}
