using System;
using System.IO;

internal sealed class \uE019
{
	public static void \uE000(Stream \uE000, Stream \uE001)
	{
		byte[] array = new byte[4096];
		\uE019.\uE01A uE01A = new \uE019.\uE01A(\uE000);
		for (;;)
		{
			int num = uE01A.\uE000(array, 0, array.Length);
			for (;;)
			{
				int num2 = \uE011.\uE006(92);
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

	public sealed class \uE01A
	{
		private \uE019.\uE01C \uE000 = new \uE019.\uE01C(32769);

		private \uE019.\uE01B \uE001;

		private \uE018 \uE002;

		private int \uE003 = -1;

		private int \uE004 = -1;

		private bool \uE005;

		private int \uE006;

		private long \uE007;

		private long \uE008;

		private bool \uE009;

		private int \uE00A;

		private bool \uE00B;

		public \uE01A(Stream \uE000)
		{
			this.\uE001 = new \uE019.\uE01B(\uE000);
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
				int num3 = \uE011.\uE006(92);
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
			if (((!this.\uE005) ? (~(-((~(-1241069154 != 0) ^ 1241069155) != 0) != 0)) : (-((-(2122543480 != 0) ^ 1064030600 ^ -1105764608) != 0))) == 0)
			{
				return -((-2080284107 ^ -1054633095 ^ -2035548383 ^ -1014428051) != 0) != 0;
			}
			this.\uE007 = this.\uE001.\uE004;
			for (;;)
			{
				int num = \uE011.\uE006(-(-(~461791376) ^ -461791438));
				for (;;)
				{
					switch (num)
					{
					case 0:
						this.\uE001.\uE001();
						num = (~(-(~1665256153)) ^ -1665256156);
						continue;
					case 1:
					{
						int uE = this.\uE001.\uE000(~(~(~(~16)))) ^ -(~(-(-47414)));
						num = (-(-(664785188 ^ 1398537360)) ^ 1959013808);
						continue;
					}
					case 2:
						this.\uE005 = (this.\uE001.\uE000(-(~1264410101) ^ 892182662 ^ 2121334129) > ~(-(~(~1))));
						num = -(-(~(~6)));
						continue;
					case 3:
						if (this.\uE003 == -(~(-(1819329339 ^ 1819329339))))
						{
							num = -(-(--1716863189 ^ 1716863189));
							continue;
						}
						goto IL_1AD;
					case 4:
					{
						int uE;
						this.\uE006 = uE;
						num = -(~(--563611425 ^ 563611431));
						continue;
					}
					case 5:
						this.\uE00B = (-((1889652524 ^ -461300344 ^ -1215301002 ^ -598681811) != 0) != 0);
						num = -(-(-(~7)));
						continue;
					case 6:
						this.\uE003 = this.\uE001.\uE000(-(~(~(-3))));
						num = \uE011.\uE006(-(~(~(~93))));
						continue;
					case 7:
						this.\uE002 = null;
						num = -(~(~(-5)));
						continue;
					case 8:
						goto IL_1A8;
					}
					break;
				}
			}
			IL_1A8:
			goto IL_39B;
			IL_1AD:
			if (this.\uE003 == (-(86381617 ^ -1280340502 ^ 1724129702) ^ 800212869))
			{
				\uE016[] uE2 = \uE012.\uE000;
				for (;;)
				{
					int num2 = \uE011.\uE006(-1308302473 ^ -274357101 ^ -1504311851 ^ 1939644048 ^ -2006196996);
					for (;;)
					{
						switch (num2)
						{
						case 0:
							this.\uE00B = ((-((-(-766129473 != 0) ^ 1045996420) != 0) ^ -334665413) != 0);
							num2 = (-(1521868751 ^ 1252306486 ^ -1241674851) ^ 1511196056);
							continue;
						case 1:
							this.\uE006 = (-(~(--494292466)) ^ 494292467);
							num2 = \uE011.\uE006(-(--1422637043 ^ 555791133) ^ -1978336436);
							continue;
						case 2:
						{
							\uE016[] uE3 = \uE012.\uE001;
							num2 = (-(~(-139525247)) ^ -139525245);
							continue;
						}
						case 3:
							this.\uE002 = \uE012.\uE002;
							num2 = -(~(-(731132118 ^ 731132119)));
							continue;
						case 4:
							goto IL_2BC;
						}
						break;
					}
				}
				IL_2BC:;
			}
			else if (this.\uE003 == -(-(-(-4))))
			{
				\uE016[] uE2;
				\uE016[] uE3;
				this.\uE009(this.\uE001, out uE2, out uE3);
				for (;;)
				{
					int num3 = \uE011.\uE006(~(~(~(~92))));
					for (;;)
					{
						switch (num3)
						{
						case 0:
							this.\uE006 = (~(~(-1156175819 ^ -978990781)) ^ 2125717878);
							num3 = (~(--745685458 ^ 1405459979) ^ -2142756825);
							continue;
						case 1:
							this.\uE002 = \uE012.\uE004(uE2, uE3);
							num3 = (~(~(--487358927)) ^ 487358925);
							continue;
						case 2:
							goto IL_360;
						}
						break;
					}
				}
				IL_360:
				this.\uE00B = (-(~(-(-(-1 != 0) != 0) != 0) != 0) != 0);
			}
			IL_39B:
			this.\uE008 = this.\uE001.\uE004;
			return -(~(~(-true)));
		}

		private int \uE002(byte[] \uE000, int \uE001, int \uE002)
		{
			int num = \uE001;
			if (this.\uE003 == ~(~(-1896383546 ^ 1114107810) ^ -862940059))
			{
				if (this.\uE006 > (~(~(758884498 ^ -846854884)) ^ -524440178))
				{
					int num2 = Math.Min(\uE002, this.\uE006);
					for (;;)
					{
						int num3 = \uE011.\uE006(~(~(-(~92))));
						for (;;)
						{
							switch (num3)
							{
							case 0:
								\uE002 -= num2;
								num3 = -(~(~(-1)));
								continue;
							case 1:
								\uE001 += num2;
								num3 = -(-(~1184317155) ^ -1184317153);
								continue;
							case 2:
								this.\uE001.\uE002(\uE000, \uE001, num2);
								num3 = \uE019.\uE001(-(~(-46964119 ^ 1770815524)) ^ -1799364074);
								continue;
							case 3:
								this.\uE006 -= num2;
								num3 = -(~(-(977218290 ^ 977218291)));
								continue;
							case 4:
								this.\uE000.\uE001(\uE000, \uE001, num2);
								num3 = (-(--427743344 ^ -1728526089) ^ 2121920378);
								continue;
							case 5:
								goto IL_120;
							}
							break;
						}
					}
					IL_120:;
				}
			}
			else if (((!this.\uE00B) ? (-(-(~866663724)) ^ -866663725) : (~(~(~(~1))))) == 0)
			{
				if (this.\uE00A > ~(-(~1066803456 ^ -1066803458)))
				{
					this.\uE003(\uE000, ref \uE001, ref \uE002);
				}
				if (\uE002 > (~(~1263040919 ^ 1102839653) ^ 183731954))
				{
					do
					{
						int num4 = \uE019.\uE01A.\uE006(this.\uE001, this.\uE002.\uE000);
						this.\uE00B = (num4 == ~(-(-884626208 ^ -1861229190 ^ 1514817691)));
						if (((!this.\uE00B) ? (~(~(-((-239927751 ^ -239927751) != 0) != 0) != 0)) : (-(~(~(-700605364 != 0) != 0) != 0) ^ 700605365)) != 0)
						{
							break;
						}
						if (num4 < -(-401936340 ^ -223432166 ^ -446972618))
						{
							int num5 = \uE001;
							\uE001 = num5 + ~(-(-1806860542 ^ 1459935476) ^ -1018583052);
							\uE000[num5] = (byte)num4;
							for (;;)
							{
								int num6 = \uE011.\uE006(~(-(-(-93))));
								for (;;)
								{
									switch (num6)
									{
									case 0:
										this.\uE000.\uE000((byte)num4);
										num6 = (-(-(--1543697379)) ^ 1543697378);
										continue;
									case 1:
										\uE002 -= ~(-(-1442575675 ^ 1942512595 ^ -640915180));
										num6 = -(-1641486659 ^ -513219957 ^ -377335799 ^ 1765649345);
										continue;
									case 2:
										goto IL_292;
									}
									break;
								}
							}
							IL_292:;
						}
						else if (num4 <= ~(~-1371467045 ^ -26057009 ^ 1345549577))
						{
							int uE00A = \uE019.\uE01A.\uE007(this.\uE001, num4);
							for (;;)
							{
								int num7 = \uE011.\uE006(~-2026156145 ^ -1338124872 ^ 1254869088 ^ -2110607883);
								for (;;)
								{
									switch (num7)
									{
									case 0:
										this.\uE00A = uE00A;
										num7 = \uE011.\uE006(~(-(~1471228323)) ^ -1471228411);
										continue;
									case 1:
									{
										int uE;
										this.\uE004 = uE;
										num7 = -(~(~-726194963 ^ -726194963));
										continue;
									}
									case 2:
									{
										int uE = \uE019.\uE01A.\uE008(this.\uE001, this.\uE002.\uE001);
										num7 = -(~(-1411486888 ^ -1735009239 ^ 860600177));
										continue;
									}
									case 3:
										goto IL_361;
									}
									break;
								}
							}
							IL_361:
							this.\uE003(\uE000, ref \uE001, ref \uE002);
						}
					}
					while (\uE002 > (~(-937071660) ^ -949116143 ^ -256428230));
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
				int num = \uE011.\uE006(90);
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
					int num2 = \uE011.\uE006(90);
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

		private static int \uE006(\uE019.\uE01B \uE000, \uE017 \uE001)
		{
			while (\uE001 != null && !\uE001.\uE000)
			{
				\uE001 = ((\uE000.\uE000(1) > 0) ? \uE001.\uE003 : \uE001.\uE002);
			}
			return (int)\uE001.\uE001;
		}

		private static int \uE007(\uE019.\uE01B \uE000, int \uE001)
		{
			int num;
			int num2;
			\uE012.\uE007(\uE001, out num, out num2);
			if (num2 > 0)
			{
				return num + \uE000.\uE000(num2);
			}
			return num;
		}

		private static int \uE008(\uE019.\uE01B \uE000, \uE017 \uE001)
		{
			int num = \uE019.\uE01A.\uE006(\uE000, \uE001);
			int num3;
			int num4;
			for (;;)
			{
				int num2 = \uE011.\uE006(92);
				for (;;)
				{
					switch (num2)
					{
					case 0:
						num3 = \uE012.\uE006[num];
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
							num2 = \uE011.\uE006(90);
							continue;
						}
						return num3;
					}
					case 3:
					{
						int num5 = \uE012.\uE007[num];
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

		private void \uE009(\uE019.\uE01B \uE000, out \uE016[] \uE001, out \uE016[] \uE002)
		{
			int num = \uE000.\uE000(5) + 257;
			int[] array;
			int num5;
			for (;;)
			{
				int num2 = \uE019.\uE001(87);
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
						int[] uE = \uE012.\uE003;
						num2 = \uE011.\uE006(92);
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
			\uE017 uE2 = \uE012.\uE005(\uE012.\uE002(array));
			int[] array2;
			for (;;)
			{
				int num6 = \uE011.\uE006(90);
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
						array2 = \uE019.\uE01A.\uE00A(\uE000, uE2, num + num5);
						num6 = 4;
						continue;
					case 2:
						IL_EB:
						\uE001[num7].\uE001 = array2[num7];
						num6 = 5;
						continue;
					case 3:
						num7 = 0;
						num6 = \uE011.\uE006(92);
						continue;
					case 4:
						\uE001 = new \uE016[num];
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
			\uE012.\uE003(\uE001);
			for (;;)
			{
				int num8 = \uE011.\uE006(90);
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
						\uE002 = new \uE016[num5];
						num8 = 4;
						continue;
					case 2:
						num9++;
						num8 = \uE019.\uE001(99);
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
			\uE012.\uE003(\uE002);
		}

		private static int[] \uE00A(\uE019.\uE01B \uE000, \uE017 \uE001, int \uE002)
		{
			int[] array = new int[\uE002];
			int num = 0;
			if (num == 0)
			{
				goto IL_12E;
			}
			IL_17:
			int num2 = \uE019.\uE01A.\uE006(\uE000, \uE001);
			for (;;)
			{
				int num3 = \uE011.\uE006(92);
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
					int num5 = \uE011.\uE006(90);
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
					int num7 = \uE011.\uE006(92);
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

	private sealed class \uE01B
	{
		private uint \uE000;

		private int \uE001;

		private int \uE002;

		private Stream \uE003;

		internal long \uE004;

		internal \uE01B(Stream \uE000)
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

	private sealed class \uE01C
	{
		private byte[] \uE000;

		private int \uE001;

		internal int \uE002;

		internal long \uE003;

		internal \uE01C(int \uE000)
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
					int num2 = \uE011.\uE006(93);
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
							num2 = \uE011.\uE006(90);
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
				int num4 = \uE011.\uE006(92);
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
					int num2 = \uE011.\uE006(93);
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
							num2 = \uE011.\uE006(92);
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
