using Mono.Math.Prime;
using Mono.Math.Prime.Generator;
using System;
using System.Security.Cryptography;

namespace Mono.Math
{
	internal class BigInteger
	{
		private const uint DEFAULT_LEN = 20u;

		private const string WouldReturnNegVal = "Operation would return a negative value";

		private uint length = 1u;

		private uint[] data;

		internal static readonly uint[] smallPrimes = new uint[]
		{
			2u,
			3u,
			5u,
			7u,
			11u,
			13u,
			17u,
			19u,
			23u,
			29u,
			31u,
			37u,
			41u,
			43u,
			47u,
			53u,
			59u,
			61u,
			67u,
			71u,
			73u,
			79u,
			83u,
			89u,
			97u,
			101u,
			103u,
			107u,
			109u,
			113u,
			127u,
			131u,
			137u,
			139u,
			149u,
			151u,
			157u,
			163u,
			167u,
			173u,
			179u,
			181u,
			191u,
			193u,
			197u,
			199u,
			211u,
			223u,
			227u,
			229u,
			233u,
			239u,
			241u,
			251u,
			257u,
			263u,
			269u,
			271u,
			277u,
			281u,
			283u,
			293u,
			307u,
			311u,
			313u,
			317u,
			331u,
			337u,
			347u,
			349u,
			353u,
			359u,
			367u,
			373u,
			379u,
			383u,
			389u,
			397u,
			401u,
			409u,
			419u,
			421u,
			431u,
			433u,
			439u,
			443u,
			449u,
			457u,
			461u,
			463u,
			467u,
			479u,
			487u,
			491u,
			499u,
			503u,
			509u,
			521u,
			523u,
			541u,
			547u,
			557u,
			563u,
			569u,
			571u,
			577u,
			587u,
			593u,
			599u,
			601u,
			607u,
			613u,
			617u,
			619u,
			631u,
			641u,
			643u,
			647u,
			653u,
			659u,
			661u,
			673u,
			677u,
			683u,
			691u,
			701u,
			709u,
			719u,
			727u,
			733u,
			739u,
			743u,
			751u,
			757u,
			761u,
			769u,
			773u,
			787u,
			797u,
			809u,
			811u,
			821u,
			823u,
			827u,
			829u,
			839u,
			853u,
			857u,
			859u,
			863u,
			877u,
			881u,
			883u,
			887u,
			907u,
			911u,
			919u,
			929u,
			937u,
			941u,
			947u,
			953u,
			967u,
			971u,
			977u,
			983u,
			991u,
			997u,
			1009u,
			1013u,
			1019u,
			1021u,
			1031u,
			1033u,
			1039u,
			1049u,
			1051u,
			1061u,
			1063u,
			1069u,
			1087u,
			1091u,
			1093u,
			1097u,
			1103u,
			1109u,
			1117u,
			1123u,
			1129u,
			1151u,
			1153u,
			1163u,
			1171u,
			1181u,
			1187u,
			1193u,
			1201u,
			1213u,
			1217u,
			1223u,
			1229u,
			1231u,
			1237u,
			1249u,
			1259u,
			1277u,
			1279u,
			1283u,
			1289u,
			1291u,
			1297u,
			1301u,
			1303u,
			1307u,
			1319u,
			1321u,
			1327u,
			1361u,
			1367u,
			1373u,
			1381u,
			1399u,
			1409u,
			1423u,
			1427u,
			1429u,
			1433u,
			1439u,
			1447u,
			1451u,
			1453u,
			1459u,
			1471u,
			1481u,
			1483u,
			1487u,
			1489u,
			1493u,
			1499u,
			1511u,
			1523u,
			1531u,
			1543u,
			1549u,
			1553u,
			1559u,
			1567u,
			1571u,
			1579u,
			1583u,
			1597u,
			1601u,
			1607u,
			1609u,
			1613u,
			1619u,
			1621u,
			1627u,
			1637u,
			1657u,
			1663u,
			1667u,
			1669u,
			1693u,
			1697u,
			1699u,
			1709u,
			1721u,
			1723u,
			1733u,
			1741u,
			1747u,
			1753u,
			1759u,
			1777u,
			1783u,
			1787u,
			1789u,
			1801u,
			1811u,
			1823u,
			1831u,
			1847u,
			1861u,
			1867u,
			1871u,
			1873u,
			1877u,
			1879u,
			1889u,
			1901u,
			1907u,
			1913u,
			1931u,
			1933u,
			1949u,
			1951u,
			1973u,
			1979u,
			1987u,
			1993u,
			1997u,
			1999u,
			2003u,
			2011u,
			2017u,
			2027u,
			2029u,
			2039u,
			2053u,
			2063u,
			2069u,
			2081u,
			2083u,
			2087u,
			2089u,
			2099u,
			2111u,
			2113u,
			2129u,
			2131u,
			2137u,
			2141u,
			2143u,
			2153u,
			2161u,
			2179u,
			2203u,
			2207u,
			2213u,
			2221u,
			2237u,
			2239u,
			2243u,
			2251u,
			2267u,
			2269u,
			2273u,
			2281u,
			2287u,
			2293u,
			2297u,
			2309u,
			2311u,
			2333u,
			2339u,
			2341u,
			2347u,
			2351u,
			2357u,
			2371u,
			2377u,
			2381u,
			2383u,
			2389u,
			2393u,
			2399u,
			2411u,
			2417u,
			2423u,
			2437u,
			2441u,
			2447u,
			2459u,
			2467u,
			2473u,
			2477u,
			2503u,
			2521u,
			2531u,
			2539u,
			2543u,
			2549u,
			2551u,
			2557u,
			2579u,
			2591u,
			2593u,
			2609u,
			2617u,
			2621u,
			2633u,
			2647u,
			2657u,
			2659u,
			2663u,
			2671u,
			2677u,
			2683u,
			2687u,
			2689u,
			2693u,
			2699u,
			2707u,
			2711u,
			2713u,
			2719u,
			2729u,
			2731u,
			2741u,
			2749u,
			2753u,
			2767u,
			2777u,
			2789u,
			2791u,
			2797u,
			2801u,
			2803u,
			2819u,
			2833u,
			2837u,
			2843u,
			2851u,
			2857u,
			2861u,
			2879u,
			2887u,
			2897u,
			2903u,
			2909u,
			2917u,
			2927u,
			2939u,
			2953u,
			2957u,
			2963u,
			2969u,
			2971u,
			2999u,
			3001u,
			3011u,
			3019u,
			3023u,
			3037u,
			3041u,
			3049u,
			3061u,
			3067u,
			3079u,
			3083u,
			3089u,
			3109u,
			3119u,
			3121u,
			3137u,
			3163u,
			3167u,
			3169u,
			3181u,
			3187u,
			3191u,
			3203u,
			3209u,
			3217u,
			3221u,
			3229u,
			3251u,
			3253u,
			3257u,
			3259u,
			3271u,
			3299u,
			3301u,
			3307u,
			3313u,
			3319u,
			3323u,
			3329u,
			3331u,
			3343u,
			3347u,
			3359u,
			3361u,
			3371u,
			3373u,
			3389u,
			3391u,
			3407u,
			3413u,
			3433u,
			3449u,
			3457u,
			3461u,
			3463u,
			3467u,
			3469u,
			3491u,
			3499u,
			3511u,
			3517u,
			3527u,
			3529u,
			3533u,
			3539u,
			3541u,
			3547u,
			3557u,
			3559u,
			3571u,
			3581u,
			3583u,
			3593u,
			3607u,
			3613u,
			3617u,
			3623u,
			3631u,
			3637u,
			3643u,
			3659u,
			3671u,
			3673u,
			3677u,
			3691u,
			3697u,
			3701u,
			3709u,
			3719u,
			3727u,
			3733u,
			3739u,
			3761u,
			3767u,
			3769u,
			3779u,
			3793u,
			3797u,
			3803u,
			3821u,
			3823u,
			3833u,
			3847u,
			3851u,
			3853u,
			3863u,
			3877u,
			3881u,
			3889u,
			3907u,
			3911u,
			3917u,
			3919u,
			3923u,
			3929u,
			3931u,
			3943u,
			3947u,
			3967u,
			3989u,
			4001u,
			4003u,
			4007u,
			4013u,
			4019u,
			4021u,
			4027u,
			4049u,
			4051u,
			4057u,
			4073u,
			4079u,
			4091u,
			4093u,
			4099u,
			4111u,
			4127u,
			4129u,
			4133u,
			4139u,
			4153u,
			4157u,
			4159u,
			4177u,
			4201u,
			4211u,
			4217u,
			4219u,
			4229u,
			4231u,
			4241u,
			4243u,
			4253u,
			4259u,
			4261u,
			4271u,
			4273u,
			4283u,
			4289u,
			4297u,
			4327u,
			4337u,
			4339u,
			4349u,
			4357u,
			4363u,
			4373u,
			4391u,
			4397u,
			4409u,
			4421u,
			4423u,
			4441u,
			4447u,
			4451u,
			4457u,
			4463u,
			4481u,
			4483u,
			4493u,
			4507u,
			4513u,
			4517u,
			4519u,
			4523u,
			4547u,
			4549u,
			4561u,
			4567u,
			4583u,
			4591u,
			4597u,
			4603u,
			4621u,
			4637u,
			4639u,
			4643u,
			4649u,
			4651u,
			4657u,
			4663u,
			4673u,
			4679u,
			4691u,
			4703u,
			4721u,
			4723u,
			4729u,
			4733u,
			4751u,
			4759u,
			4783u,
			4787u,
			4789u,
			4793u,
			4799u,
			4801u,
			4813u,
			4817u,
			4831u,
			4861u,
			4871u,
			4877u,
			4889u,
			4903u,
			4909u,
			4919u,
			4931u,
			4933u,
			4937u,
			4943u,
			4951u,
			4957u,
			4967u,
			4969u,
			4973u,
			4987u,
			4993u,
			4999u,
			5003u,
			5009u,
			5011u,
			5021u,
			5023u,
			5039u,
			5051u,
			5059u,
			5077u,
			5081u,
			5087u,
			5099u,
			5101u,
			5107u,
			5113u,
			5119u,
			5147u,
			5153u,
			5167u,
			5171u,
			5179u,
			5189u,
			5197u,
			5209u,
			5227u,
			5231u,
			5233u,
			5237u,
			5261u,
			5273u,
			5279u,
			5281u,
			5297u,
			5303u,
			5309u,
			5323u,
			5333u,
			5347u,
			5351u,
			5381u,
			5387u,
			5393u,
			5399u,
			5407u,
			5413u,
			5417u,
			5419u,
			5431u,
			5437u,
			5441u,
			5443u,
			5449u,
			5471u,
			5477u,
			5479u,
			5483u,
			5501u,
			5503u,
			5507u,
			5519u,
			5521u,
			5527u,
			5531u,
			5557u,
			5563u,
			5569u,
			5573u,
			5581u,
			5591u,
			5623u,
			5639u,
			5641u,
			5647u,
			5651u,
			5653u,
			5657u,
			5659u,
			5669u,
			5683u,
			5689u,
			5693u,
			5701u,
			5711u,
			5717u,
			5737u,
			5741u,
			5743u,
			5749u,
			5779u,
			5783u,
			5791u,
			5801u,
			5807u,
			5813u,
			5821u,
			5827u,
			5839u,
			5843u,
			5849u,
			5851u,
			5857u,
			5861u,
			5867u,
			5869u,
			5879u,
			5881u,
			5897u,
			5903u,
			5923u,
			5927u,
			5939u,
			5953u,
			5981u,
			5987u
		};

		private static RandomNumberGenerator rng;

		public BigInteger()
		{
			this.data = new uint[20];
			this.length = 20u;
		}

		public BigInteger(BigInteger.Sign sign, uint len)
		{
			this.data = new uint[len];
			this.length = len;
		}

		public BigInteger(BigInteger bi)
		{
			this.data = (uint[])bi.data.Clone();
			this.length = bi.length;
		}

		public BigInteger(BigInteger bi, uint len)
		{
			this.data = new uint[len];
			for (uint num = 0u; num < bi.length; num += 1u)
			{
				this.data[(int)((UIntPtr)num)] = bi.data[(int)((UIntPtr)num)];
			}
			this.length = bi.length;
		}

		public BigInteger(byte[] inData)
		{
			this.length = (uint)inData.Length >> 2;
			int num = inData.Length & 3;
			if (num != 0)
			{
				this.length += 1u;
			}
			this.data = new uint[this.length];
			int i = inData.Length - 1;
			int num2 = 0;
			while (i >= 3)
			{
				this.data[num2] = (uint)((int)inData[i - 3] << 24 | (int)inData[i - 2] << 16 | (int)inData[i - 1] << 8 | (int)inData[i]);
				i -= 4;
				num2++;
			}
			switch (num)
			{
			case 1:
				this.data[(int)((UIntPtr)(this.length - 1u))] = (uint)inData[0];
				break;
			case 2:
				this.data[(int)((UIntPtr)(this.length - 1u))] = (uint)((int)inData[0] << 8 | (int)inData[1]);
				break;
			case 3:
				this.data[(int)((UIntPtr)(this.length - 1u))] = (uint)((int)inData[0] << 16 | (int)inData[1] << 8 | (int)inData[2]);
				break;
			}
			this.Normalize();
		}

		public BigInteger(uint[] inData)
		{
			this.length = (uint)inData.Length;
			this.data = new uint[this.length];
			int i = (int)(this.length - 1u);
			int num = 0;
			while (i >= 0)
			{
				this.data[num] = inData[i];
				i--;
				num++;
			}
			this.Normalize();
		}

		public BigInteger(uint ui)
		{
			this.data = new uint[]
			{
				ui
			};
		}

		public BigInteger(ulong ul)
		{
			this.data = new uint[]
			{
				(uint)ul,
				(uint)(ul >> 32)
			};
			this.length = 2u;
			this.Normalize();
		}

		public static BigInteger Parse(string number)
		{
			if (number == null)
			{
				throw new ArgumentNullException("number");
			}
			int i = 0;
			int num = number.Length;
			bool flag = false;
			BigInteger bigInteger = new BigInteger(0u);
			if (number[i] == '+')
			{
				i++;
			}
			else if (number[i] == '-')
			{
				throw new FormatException("Operation would return a negative value");
			}
			while (i < num)
			{
				char c = number[i];
				if (c == '\0')
				{
					i = num;
				}
				else if (c >= '0' && c <= '9')
				{
					bigInteger = bigInteger * 10 + (int)(c - '0');
					flag = true;
				}
				else
				{
					if (char.IsWhiteSpace(c))
					{
						for (i++; i < num; i++)
						{
							if (!char.IsWhiteSpace(number[i]))
							{
								throw new FormatException();
							}
						}
						break;
					}
					throw new FormatException();
				}
				i++;
			}
			if (!flag)
			{
				throw new FormatException();
			}
			return bigInteger;
		}

		public static BigInteger Add(BigInteger bi1, BigInteger bi2)
		{
			return bi1 + bi2;
		}

		public static BigInteger Subtract(BigInteger bi1, BigInteger bi2)
		{
			return bi1 - bi2;
		}

		public static int Modulus(BigInteger bi, int i)
		{
			return bi % i;
		}

		public static uint Modulus(BigInteger bi, uint ui)
		{
			return bi % ui;
		}

		public static BigInteger Modulus(BigInteger bi1, BigInteger bi2)
		{
			return bi1 % bi2;
		}

		public static BigInteger Divid(BigInteger bi, int i)
		{
			return bi / i;
		}

		public static BigInteger Divid(BigInteger bi1, BigInteger bi2)
		{
			return bi1 / bi2;
		}

		public static BigInteger Multiply(BigInteger bi1, BigInteger bi2)
		{
			return bi1 * bi2;
		}

		public static BigInteger Multiply(BigInteger bi, int i)
		{
			return bi * i;
		}

		private static RandomNumberGenerator Rng
		{
			get
			{
				if (BigInteger.rng == null)
				{
					BigInteger.rng = RandomNumberGenerator.Create();
				}
				return BigInteger.rng;
			}
		}

		public static BigInteger GenerateRandom(int bits, RandomNumberGenerator rng)
		{
			int num = bits >> 5;
			int num2 = bits & 31;
			if (num2 != 0)
			{
				num++;
			}
			BigInteger bigInteger = new BigInteger(BigInteger.Sign.Positive, (uint)(num + 1));
			byte[] src = new byte[num << 2];
			rng.GetBytes(src);
			Buffer.BlockCopy(src, 0, bigInteger.data, 0, num << 2);
			if (num2 != 0)
			{
				uint num3 = 1u << num2 - 1;
				bigInteger.data[num - 1] |= num3;
				num3 = uint.MaxValue >> 32 - num2;
				bigInteger.data[num - 1] &= num3;
			}
			else
			{
				bigInteger.data[num - 1] |= 2147483648u;
			}
			bigInteger.Normalize();
			return bigInteger;
		}

		public static BigInteger GenerateRandom(int bits)
		{
			return BigInteger.GenerateRandom(bits, BigInteger.Rng);
		}

		public void Randomize(RandomNumberGenerator rng)
		{
			if (this == 0u)
			{
				return;
			}
			int num = this.BitCount();
			int num2 = num >> 5;
			int num3 = num & 31;
			if (num3 != 0)
			{
				num2++;
			}
			byte[] src = new byte[num2 << 2];
			rng.GetBytes(src);
			Buffer.BlockCopy(src, 0, this.data, 0, num2 << 2);
			if (num3 != 0)
			{
				uint num4 = 1u << num3 - 1;
				this.data[num2 - 1] |= num4;
				num4 = uint.MaxValue >> 32 - num3;
				this.data[num2 - 1] &= num4;
			}
			else
			{
				this.data[num2 - 1] |= 2147483648u;
			}
			this.Normalize();
		}

		public void Randomize()
		{
			this.Randomize(BigInteger.Rng);
		}

		public int BitCount()
		{
			this.Normalize();
			uint num = this.data[(int)((UIntPtr)(this.length - 1u))];
			uint num2 = 2147483648u;
			uint num3 = 32u;
			while (num3 > 0u && (num & num2) == 0u)
			{
				num3 -= 1u;
				num2 >>= 1;
			}
			return (int)(num3 + (this.length - 1u << 5));
		}

		public bool TestBit(uint bitNum)
		{
			uint num = bitNum >> 5;
			byte b = (byte)(bitNum & 31u);
			uint num2 = 1u << (int)b;
			return (this.data[(int)((UIntPtr)num)] & num2) != 0u;
		}

		public bool TestBit(int bitNum)
		{
			if (bitNum < 0)
			{
				throw new IndexOutOfRangeException("bitNum out of range");
			}
			uint num = (uint)bitNum >> 5;
			byte b = (byte)(bitNum & 31);
			uint num2 = 1u << (int)b;
			return (this.data[(int)((UIntPtr)num)] | num2) == this.data[(int)((UIntPtr)num)];
		}

		public void SetBit(uint bitNum)
		{
			this.SetBit(bitNum, true);
		}

		public void ClearBit(uint bitNum)
		{
			this.SetBit(bitNum, false);
		}

		public void SetBit(uint bitNum, bool value)
		{
			uint num = bitNum >> 5;
			if (num < this.length)
			{
				uint num2 = 1u << (int)bitNum;
				if (value)
				{
					this.data[(int)((UIntPtr)num)] |= num2;
				}
				else
				{
					this.data[(int)((UIntPtr)num)] &= ~num2;
				}
			}
		}

		public int LowestSetBit()
		{
			if (this == 0u)
			{
				return -1;
			}
			int num = 0;
			while (!this.TestBit(num))
			{
				num++;
			}
			return num;
		}

		public byte[] GetBytes()
		{
			if (this == 0u)
			{
				return new byte[1];
			}
			int num = this.BitCount();
			int num2 = num >> 3;
			if ((num & 7) != 0)
			{
				num2++;
			}
			byte[] array = new byte[num2];
			int num3 = num2 & 3;
			if (num3 == 0)
			{
				num3 = 4;
			}
			int num4 = 0;
			for (int i = (int)(this.length - 1u); i >= 0; i--)
			{
				uint num5 = this.data[i];
				for (int j = num3 - 1; j >= 0; j--)
				{
					array[num4 + j] = (byte)(num5 & 255u);
					num5 >>= 8;
				}
				num4 += num3;
				num3 = 4;
			}
			return array;
		}

		public BigInteger.Sign Compare(BigInteger bi)
		{
			return BigInteger.Kernel.Compare(this, bi);
		}

		public string ToString(uint radix)
		{
			return this.ToString(radix, "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
		}

		public string ToString(uint radix, string characterSet)
		{
			if ((long)characterSet.Length < (long)((ulong)radix))
			{
				throw new ArgumentException("charSet length less than radix", "characterSet");
			}
			if (radix == 1u)
			{
				throw new ArgumentException("There is no such thing as radix one notation", "radix");
			}
			if (this == 0u)
			{
				return "0";
			}
			if (this == 1u)
			{
				return "1";
			}
			string text = string.Empty;
			BigInteger bigInteger = new BigInteger(this);
			while (bigInteger != 0u)
			{
				uint index = BigInteger.Kernel.SingleByteDivideInPlace(bigInteger, radix);
				text = characterSet[(int)index] + text;
			}
			return text;
		}

		private void Normalize()
		{
			while (this.length > 0u && this.data[(int)((UIntPtr)(this.length - 1u))] == 0u)
			{
				this.length -= 1u;
			}
			if (this.length == 0u)
			{
				this.length += 1u;
			}
		}

		public void Clear()
		{
			int num = 0;
			while ((long)num < (long)((ulong)this.length))
			{
				this.data[num] = 0u;
				num++;
			}
		}

		public override int GetHashCode()
		{
			uint num = 0u;
			for (uint num2 = 0u; num2 < this.length; num2 += 1u)
			{
				num ^= this.data[(int)((UIntPtr)num2)];
			}
			return (int)num;
		}

		public override string ToString()
		{
			return this.ToString(10u);
		}

		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			if (o is int)
			{
				return (int)o >= 0 && this == (uint)o;
			}
			BigInteger bigInteger = o as BigInteger;
			return !(bigInteger == null) && BigInteger.Kernel.Compare(this, bigInteger) == BigInteger.Sign.Zero;
		}

		public BigInteger GCD(BigInteger bi)
		{
			return BigInteger.Kernel.gcd(this, bi);
		}

		public BigInteger ModInverse(BigInteger modulus)
		{
			return BigInteger.Kernel.modInverse(this, modulus);
		}

		public BigInteger ModPow(BigInteger exp, BigInteger n)
		{
			BigInteger.ModulusRing modulusRing = new BigInteger.ModulusRing(n);
			return modulusRing.Pow(this, exp);
		}

		public bool IsProbablePrime()
		{
			if (this <= BigInteger.smallPrimes[BigInteger.smallPrimes.Length - 1])
			{
				for (int i = 0; i < BigInteger.smallPrimes.Length; i++)
				{
					if (this == BigInteger.smallPrimes[i])
					{
						return true;
					}
				}
				return false;
			}
			for (int j = 0; j < BigInteger.smallPrimes.Length; j++)
			{
				if (this % BigInteger.smallPrimes[j] == 0u)
				{
					return false;
				}
			}
			return PrimalityTests.Test(this, ConfidenceFactor.Medium);
		}

		public static BigInteger NextHighestPrime(BigInteger bi)
		{
			NextPrimeFinder nextPrimeFinder = new NextPrimeFinder();
			return nextPrimeFinder.GenerateNewPrime(0, bi);
		}

		public static BigInteger GeneratePseudoPrime(int bits)
		{
			SequentialSearchPrimeGeneratorBase sequentialSearchPrimeGeneratorBase = new SequentialSearchPrimeGeneratorBase();
			return sequentialSearchPrimeGeneratorBase.GenerateNewPrime(bits);
		}

		public void Incr2()
		{
			int num = 0;
			this.data[0] += 2u;
			if (this.data[0] < 2u)
			{
				this.data[++num] += 1u;
				while (this.data[num++] == 0u)
				{
					this.data[num] += 1u;
				}
				if (this.length == (uint)num)
				{
					this.length += 1u;
				}
			}
		}

		public static implicit operator BigInteger(uint value)
		{
			return new BigInteger(value);
		}

		public static implicit operator BigInteger(int value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			return new BigInteger((uint)value);
		}

		public static implicit operator BigInteger(ulong value)
		{
			return new BigInteger(value);
		}

		public static BigInteger operator +(BigInteger bi1, BigInteger bi2)
		{
			if (bi1 == 0u)
			{
				return new BigInteger(bi2);
			}
			if (bi2 == 0u)
			{
				return new BigInteger(bi1);
			}
			return BigInteger.Kernel.AddSameSign(bi1, bi2);
		}

		public static BigInteger operator -(BigInteger bi1, BigInteger bi2)
		{
			if (bi2 == 0u)
			{
				return new BigInteger(bi1);
			}
			if (bi1 == 0u)
			{
				throw new ArithmeticException("Operation would return a negative value");
			}
			BigInteger.Sign sign = BigInteger.Kernel.Compare(bi1, bi2);
			switch (sign + 1)
			{
			case BigInteger.Sign.Zero:
				throw new ArithmeticException("Operation would return a negative value");
			case BigInteger.Sign.Positive:
				return 0;
			case (BigInteger.Sign)2:
				return BigInteger.Kernel.Subtract(bi1, bi2);
			default:
				throw new Exception();
			}
		}

		public static int operator %(BigInteger bi, int i)
		{
			if (i > 0)
			{
				return (int)BigInteger.Kernel.DwordMod(bi, (uint)i);
			}
			return (int)(-(int)BigInteger.Kernel.DwordMod(bi, (uint)(-(uint)i)));
		}

		public static uint operator %(BigInteger bi, uint ui)
		{
			return BigInteger.Kernel.DwordMod(bi, ui);
		}

		public static BigInteger operator %(BigInteger bi1, BigInteger bi2)
		{
			return BigInteger.Kernel.multiByteDivide(bi1, bi2)[1];
		}

		public static BigInteger operator /(BigInteger bi, int i)
		{
			if (i > 0)
			{
				return BigInteger.Kernel.DwordDiv(bi, (uint)i);
			}
			throw new ArithmeticException("Operation would return a negative value");
		}

		public static BigInteger operator /(BigInteger bi1, BigInteger bi2)
		{
			return BigInteger.Kernel.multiByteDivide(bi1, bi2)[0];
		}

		public static BigInteger operator *(BigInteger bi1, BigInteger bi2)
		{
			if (bi1 == 0u || bi2 == 0u)
			{
				return 0;
			}
			if ((long)bi1.data.Length < (long)((ulong)bi1.length))
			{
				throw new IndexOutOfRangeException("bi1 out of range");
			}
			if ((long)bi2.data.Length < (long)((ulong)bi2.length))
			{
				throw new IndexOutOfRangeException("bi2 out of range");
			}
			BigInteger bigInteger = new BigInteger(BigInteger.Sign.Positive, bi1.length + bi2.length);
			BigInteger.Kernel.Multiply(bi1.data, 0u, bi1.length, bi2.data, 0u, bi2.length, bigInteger.data, 0u);
			bigInteger.Normalize();
			return bigInteger;
		}

		public static BigInteger operator *(BigInteger bi, int i)
		{
			if (i < 0)
			{
				throw new ArithmeticException("Operation would return a negative value");
			}
			if (i == 0)
			{
				return 0;
			}
			if (i == 1)
			{
				return new BigInteger(bi);
			}
			return BigInteger.Kernel.MultiplyByDword(bi, (uint)i);
		}

		public static BigInteger operator <<(BigInteger bi1, int shiftVal)
		{
			return BigInteger.Kernel.LeftShift(bi1, shiftVal);
		}

		public static BigInteger operator >>(BigInteger bi1, int shiftVal)
		{
			return BigInteger.Kernel.RightShift(bi1, shiftVal);
		}

		public static bool operator ==(BigInteger bi1, uint ui)
		{
			if (bi1.length != 1u)
			{
				bi1.Normalize();
			}
			return bi1.length == 1u && bi1.data[0] == ui;
		}

		public static bool operator !=(BigInteger bi1, uint ui)
		{
			if (bi1.length != 1u)
			{
				bi1.Normalize();
			}
			return bi1.length != 1u || bi1.data[0] != ui;
		}

		public static bool operator ==(BigInteger bi1, BigInteger bi2)
		{
			return bi1 == bi2 || (!(null == bi1) && !(null == bi2) && BigInteger.Kernel.Compare(bi1, bi2) == BigInteger.Sign.Zero);
		}

		public static bool operator !=(BigInteger bi1, BigInteger bi2)
		{
			return bi1 != bi2 && (null == bi1 || null == bi2 || BigInteger.Kernel.Compare(bi1, bi2) != BigInteger.Sign.Zero);
		}

		public static bool operator >(BigInteger bi1, BigInteger bi2)
		{
			return BigInteger.Kernel.Compare(bi1, bi2) > BigInteger.Sign.Zero;
		}

		public static bool operator <(BigInteger bi1, BigInteger bi2)
		{
			return BigInteger.Kernel.Compare(bi1, bi2) < BigInteger.Sign.Zero;
		}

		public static bool operator >=(BigInteger bi1, BigInteger bi2)
		{
			return BigInteger.Kernel.Compare(bi1, bi2) >= BigInteger.Sign.Zero;
		}

		public static bool operator <=(BigInteger bi1, BigInteger bi2)
		{
			return BigInteger.Kernel.Compare(bi1, bi2) <= BigInteger.Sign.Zero;
		}

		public enum Sign
		{
			Negative = -1,
			Zero,
			Positive
		}

		internal sealed class ModulusRing
		{
			private BigInteger mod;

			private BigInteger constant;

			public ModulusRing(BigInteger modulus)
			{
				this.mod = modulus;
				uint num = this.mod.length << 1;
				this.constant = new BigInteger(BigInteger.Sign.Positive, num + 1u);
				this.constant.data[(int)((UIntPtr)num)] = 1u;
				this.constant /= this.mod;
			}

			public void BarrettReduction(BigInteger x)
			{
				BigInteger bigInteger = this.mod;
				uint length = bigInteger.length;
				uint num = length + 1u;
				uint num2 = length - 1u;
				if (x.length < length)
				{
					return;
				}
				if ((long)x.data.Length < (long)((ulong)x.length))
				{
					throw new IndexOutOfRangeException("x out of range");
				}
				BigInteger bigInteger2 = new BigInteger(BigInteger.Sign.Positive, x.length - num2 + this.constant.length);
				BigInteger.Kernel.Multiply(x.data, num2, x.length - num2, this.constant.data, 0u, this.constant.length, bigInteger2.data, 0u);
				uint length2 = (x.length <= num) ? x.length : num;
				x.length = length2;
				x.Normalize();
				BigInteger bigInteger3 = new BigInteger(BigInteger.Sign.Positive, num);
				BigInteger.Kernel.MultiplyMod2p32pmod(bigInteger2.data, (int)num, (int)(bigInteger2.length - num), bigInteger.data, 0, (int)bigInteger.length, bigInteger3.data, 0, (int)num);
				bigInteger3.Normalize();
				if (bigInteger3 <= x)
				{
					BigInteger.Kernel.MinusEq(x, bigInteger3);
				}
				else
				{
					BigInteger bigInteger4 = new BigInteger(BigInteger.Sign.Positive, num + 1u);
					bigInteger4.data[(int)((UIntPtr)num)] = 1u;
					BigInteger.Kernel.MinusEq(bigInteger4, bigInteger3);
					BigInteger.Kernel.PlusEq(x, bigInteger4);
				}
				while (x >= bigInteger)
				{
					BigInteger.Kernel.MinusEq(x, bigInteger);
				}
			}

			public BigInteger Multiply(BigInteger a, BigInteger b)
			{
				if (a == 0u || b == 0u)
				{
					return 0;
				}
				if (a > this.mod)
				{
					a %= this.mod;
				}
				if (b > this.mod)
				{
					b %= this.mod;
				}
				BigInteger bigInteger = a * b;
				this.BarrettReduction(bigInteger);
				return bigInteger;
			}

			public BigInteger Difference(BigInteger a, BigInteger b)
			{
				BigInteger.Sign sign = BigInteger.Kernel.Compare(a, b);
				BigInteger.Sign sign2 = sign;
				BigInteger bigInteger;
				switch (sign2 + 1)
				{
				case BigInteger.Sign.Zero:
					bigInteger = b - a;
					break;
				case BigInteger.Sign.Positive:
					return 0;
				case (BigInteger.Sign)2:
					bigInteger = a - b;
					break;
				default:
					throw new Exception();
				}
				if (bigInteger >= this.mod)
				{
					if (bigInteger.length >= this.mod.length << 1)
					{
						bigInteger %= this.mod;
					}
					else
					{
						this.BarrettReduction(bigInteger);
					}
				}
				if (sign == BigInteger.Sign.Negative)
				{
					bigInteger = this.mod - bigInteger;
				}
				return bigInteger;
			}

			public BigInteger Pow(BigInteger a, BigInteger k)
			{
				BigInteger bigInteger = new BigInteger(1u);
				if (k == 0u)
				{
					return bigInteger;
				}
				BigInteger bigInteger2 = a;
				if (k.TestBit(0))
				{
					bigInteger = a;
				}
				for (int i = 1; i < k.BitCount(); i++)
				{
					bigInteger2 = this.Multiply(bigInteger2, bigInteger2);
					if (k.TestBit(i))
					{
						bigInteger = this.Multiply(bigInteger2, bigInteger);
					}
				}
				return bigInteger;
			}

			public BigInteger Pow(uint b, BigInteger exp)
			{
				return this.Pow(new BigInteger(b), exp);
			}
		}

		internal sealed class Montgomery
		{
			private Montgomery()
			{
			}

			public static uint Inverse(uint n)
			{
				uint num = n;
				uint num2;
				while ((num2 = n * num) != 1u)
				{
					num *= 2u - num2;
				}
				return (uint)(-(uint)((ulong)num));
			}

			public static BigInteger ToMont(BigInteger n, BigInteger m)
			{
				n.Normalize();
				m.Normalize();
				n <<= (int)(m.length * 32u);
				n %= m;
				return n;
			}

			public unsafe static BigInteger Reduce(BigInteger n, BigInteger m, uint mPrime)
			{
				fixed (uint* ptr = ref (n.data != null && n.data.Length != 0) ? ref n.data[0] : ref *null, ptr2 = ref (m.data != null && m.data.Length != 0) ? ref m.data[0] : ref *null)
				{
					for (uint num = 0u; num < m.length; num += 1u)
					{
						uint num2 = *ptr * mPrime;
						uint* ptr3 = ptr2;
						uint* ptr4 = ptr;
						uint* ptr5 = ptr;
						ulong num3 = (ulong)num2 * (ulong)(*(ptr3++)) + (ulong)(*(ptr4++));
						num3 >>= 32;
						uint num4;
						for (num4 = 1u; num4 < m.length; num4 += 1u)
						{
							num3 += (ulong)num2 * (ulong)(*(ptr3++)) + (ulong)(*(ptr4++));
							*(ptr5++) = (uint)num3;
							num3 >>= 32;
						}
						while (num4 < n.length)
						{
							num3 += (ulong)(*(ptr4++));
							*(ptr5++) = (uint)num3;
							num3 >>= 32;
							if (num3 == 0UL)
							{
								num4 += 1u;
								break;
							}
							num4 += 1u;
						}
						while (num4 < n.length)
						{
							*(ptr5++) = *(ptr4++);
							num4 += 1u;
						}
						*(ptr5++) = (uint)num3;
					}
					while (n.length > 1u && ptr[n.length - 1u] == 0u)
					{
						n.length -= 1u;
					}
				}
				if (n >= m)
				{
					BigInteger.Kernel.MinusEq(n, m);
				}
				return n;
			}
		}

		private sealed class Kernel
		{
			public static BigInteger AddSameSign(BigInteger bi1, BigInteger bi2)
			{
				uint num = 0u;
				uint[] data;
				uint length;
				uint[] data2;
				uint length2;
				if (bi1.length < bi2.length)
				{
					data = bi2.data;
					length = bi2.length;
					data2 = bi1.data;
					length2 = bi1.length;
				}
				else
				{
					data = bi1.data;
					length = bi1.length;
					data2 = bi2.data;
					length2 = bi2.length;
				}
				BigInteger bigInteger = new BigInteger(BigInteger.Sign.Positive, length + 1u);
				uint[] data3 = bigInteger.data;
				ulong num2 = 0UL;
				do
				{
					num2 = (ulong)data[(int)((UIntPtr)num)] + (ulong)data2[(int)((UIntPtr)num)] + num2;
					data3[(int)((UIntPtr)num)] = (uint)num2;
					num2 >>= 32;
				}
				while ((num += 1u) < length2);
				bool flag = num2 != 0UL;
				if (flag)
				{
					if (num < length)
					{
						do
						{
							flag = ((data3[(int)((UIntPtr)num)] = data[(int)((UIntPtr)num)] + 1u) == 0u);
						}
						while ((num += 1u) < length && flag);
					}
					if (flag)
					{
						data3[(int)((UIntPtr)num)] = 1u;
						bigInteger.length = num + 1u;
						return bigInteger;
					}
				}
				if (num < length)
				{
					do
					{
						data3[(int)((UIntPtr)num)] = data[(int)((UIntPtr)num)];
					}
					while ((num += 1u) < length);
				}
				bigInteger.Normalize();
				return bigInteger;
			}

			public static BigInteger Subtract(BigInteger big, BigInteger small)
			{
				BigInteger bigInteger = new BigInteger(BigInteger.Sign.Positive, big.length);
				uint[] data = bigInteger.data;
				uint[] data2 = big.data;
				uint[] data3 = small.data;
				uint num = 0u;
				uint num2 = 0u;
				do
				{
					uint num3 = data3[(int)((UIntPtr)num)];
					if ((num3 += num2) < num2 | (data[(int)((UIntPtr)num)] = data2[(int)((UIntPtr)num)] - num3) > ~num3)
					{
						num2 = 1u;
					}
					else
					{
						num2 = 0u;
					}
				}
				while ((num += 1u) < small.length);
				if (num != big.length)
				{
					if (num2 == 1u)
					{
						do
						{
							data[(int)((UIntPtr)num)] = data2[(int)((UIntPtr)num)] - 1u;
						}
						while (data2[(int)((UIntPtr)(num++))] == 0u && num < big.length);
						if (num == big.length)
						{
							goto IL_E5;
						}
					}
					do
					{
						data[(int)((UIntPtr)num)] = data2[(int)((UIntPtr)num)];
					}
					while ((num += 1u) < big.length);
				}
				IL_E5:
				bigInteger.Normalize();
				return bigInteger;
			}

			public static void MinusEq(BigInteger big, BigInteger small)
			{
				uint[] data = big.data;
				uint[] data2 = small.data;
				uint num = 0u;
				uint num2 = 0u;
				do
				{
					uint num3 = data2[(int)((UIntPtr)num)];
					if ((num3 += num2) < num2 | (data[(int)((UIntPtr)num)] -= num3) > ~num3)
					{
						num2 = 1u;
					}
					else
					{
						num2 = 0u;
					}
				}
				while ((num += 1u) < small.length);
				if (num != big.length)
				{
					if (num2 == 1u)
					{
						do
						{
							data[(int)((UIntPtr)num)] -= 1u;
						}
						while (data[(int)((UIntPtr)(num++))] == 0u && num < big.length);
					}
				}
				while (big.length > 0u && big.data[(int)((UIntPtr)(big.length - 1u))] == 0u)
				{
					big.length -= 1u;
				}
				if (big.length == 0u)
				{
					big.length += 1u;
				}
			}

			public static void PlusEq(BigInteger bi1, BigInteger bi2)
			{
				uint num = 0u;
				bool flag = false;
				uint[] data;
				uint length;
				uint[] data2;
				uint length2;
				if (bi1.length < bi2.length)
				{
					flag = true;
					data = bi2.data;
					length = bi2.length;
					data2 = bi1.data;
					length2 = bi1.length;
				}
				else
				{
					data = bi1.data;
					length = bi1.length;
					data2 = bi2.data;
					length2 = bi2.length;
				}
				uint[] data3 = bi1.data;
				ulong num2 = 0UL;
				do
				{
					num2 += (ulong)data[(int)((UIntPtr)num)] + (ulong)data2[(int)((UIntPtr)num)];
					data3[(int)((UIntPtr)num)] = (uint)num2;
					num2 >>= 32;
				}
				while ((num += 1u) < length2);
				bool flag2 = num2 != 0UL;
				if (flag2)
				{
					if (num < length)
					{
						do
						{
							flag2 = ((data3[(int)((UIntPtr)num)] = data[(int)((UIntPtr)num)] + 1u) == 0u);
						}
						while ((num += 1u) < length && flag2);
					}
					if (flag2)
					{
						data3[(int)((UIntPtr)num)] = 1u;
						bi1.length = num + 1u;
						return;
					}
				}
				if (flag && num < length - 1u)
				{
					do
					{
						data3[(int)((UIntPtr)num)] = data[(int)((UIntPtr)num)];
					}
					while ((num += 1u) < length);
				}
				bi1.length = length + 1u;
				bi1.Normalize();
			}

			public static BigInteger.Sign Compare(BigInteger bi1, BigInteger bi2)
			{
				uint num = bi1.length;
				uint num2 = bi2.length;
				while (num > 0u && bi1.data[(int)((UIntPtr)(num - 1u))] == 0u)
				{
					num -= 1u;
				}
				while (num2 > 0u && bi2.data[(int)((UIntPtr)(num2 - 1u))] == 0u)
				{
					num2 -= 1u;
				}
				if (num == 0u && num2 == 0u)
				{
					return BigInteger.Sign.Zero;
				}
				if (num < num2)
				{
					return BigInteger.Sign.Negative;
				}
				if (num > num2)
				{
					return BigInteger.Sign.Positive;
				}
				uint num3 = num - 1u;
				while (num3 != 0u && bi1.data[(int)((UIntPtr)num3)] == bi2.data[(int)((UIntPtr)num3)])
				{
					num3 -= 1u;
				}
				if (bi1.data[(int)((UIntPtr)num3)] < bi2.data[(int)((UIntPtr)num3)])
				{
					return BigInteger.Sign.Negative;
				}
				if (bi1.data[(int)((UIntPtr)num3)] > bi2.data[(int)((UIntPtr)num3)])
				{
					return BigInteger.Sign.Positive;
				}
				return BigInteger.Sign.Zero;
			}

			public static uint SingleByteDivideInPlace(BigInteger n, uint d)
			{
				ulong num = 0UL;
				uint length = n.length;
				while (length-- > 0u)
				{
					num <<= 32;
					num |= (ulong)n.data[(int)((UIntPtr)length)];
					n.data[(int)((UIntPtr)length)] = (uint)(num / (ulong)d);
					num %= (ulong)d;
				}
				n.Normalize();
				return (uint)num;
			}

			public static uint DwordMod(BigInteger n, uint d)
			{
				ulong num = 0UL;
				uint length = n.length;
				while (length-- > 0u)
				{
					num <<= 32;
					num |= (ulong)n.data[(int)((UIntPtr)length)];
					num %= (ulong)d;
				}
				return (uint)num;
			}

			public static BigInteger DwordDiv(BigInteger n, uint d)
			{
				BigInteger bigInteger = new BigInteger(BigInteger.Sign.Positive, n.length);
				ulong num = 0UL;
				uint length = n.length;
				while (length-- > 0u)
				{
					num <<= 32;
					num |= (ulong)n.data[(int)((UIntPtr)length)];
					bigInteger.data[(int)((UIntPtr)length)] = (uint)(num / (ulong)d);
					num %= (ulong)d;
				}
				bigInteger.Normalize();
				return bigInteger;
			}

			public static BigInteger[] DwordDivMod(BigInteger n, uint d)
			{
				BigInteger bigInteger = new BigInteger(BigInteger.Sign.Positive, n.length);
				ulong num = 0UL;
				uint length = n.length;
				while (length-- > 0u)
				{
					num <<= 32;
					num |= (ulong)n.data[(int)((UIntPtr)length)];
					bigInteger.data[(int)((UIntPtr)length)] = (uint)(num / (ulong)d);
					num %= (ulong)d;
				}
				bigInteger.Normalize();
				BigInteger bigInteger2 = (uint)num;
				return new BigInteger[]
				{
					bigInteger,
					bigInteger2
				};
			}

			public static BigInteger[] multiByteDivide(BigInteger bi1, BigInteger bi2)
			{
				if (BigInteger.Kernel.Compare(bi1, bi2) == BigInteger.Sign.Negative)
				{
					return new BigInteger[]
					{
						0,
						new BigInteger(bi1)
					};
				}
				bi1.Normalize();
				bi2.Normalize();
				if (bi2.length == 1u)
				{
					return BigInteger.Kernel.DwordDivMod(bi1, bi2.data[0]);
				}
				uint num = bi1.length + 1u;
				int num2 = (int)(bi2.length + 1u);
				uint num3 = 2147483648u;
				uint num4 = bi2.data[(int)((UIntPtr)(bi2.length - 1u))];
				int num5 = 0;
				int num6 = (int)(bi1.length - bi2.length);
				while (num3 != 0u && (num4 & num3) == 0u)
				{
					num5++;
					num3 >>= 1;
				}
				BigInteger bigInteger = new BigInteger(BigInteger.Sign.Positive, bi1.length - bi2.length + 1u);
				BigInteger bigInteger2 = bi1 << num5;
				uint[] data = bigInteger2.data;
				bi2 <<= num5;
				int i = (int)(num - bi2.length);
				int num7 = (int)(num - 1u);
				uint num8 = bi2.data[(int)((UIntPtr)(bi2.length - 1u))];
				ulong num9 = (ulong)bi2.data[(int)((UIntPtr)(bi2.length - 2u))];
				while (i > 0)
				{
					ulong num10 = ((ulong)data[num7] << 32) + (ulong)data[num7 - 1];
					ulong num11 = num10 / (ulong)num8;
					ulong num12 = num10 % (ulong)num8;
					while (num11 == 4294967296UL || num11 * num9 > (num12 << 32) + (ulong)data[num7 - 2])
					{
						num11 -= 1UL;
						num12 += (ulong)num8;
						if (num12 >= 4294967296UL)
						{
							break;
						}
					}
					uint num13 = 0u;
					int num14 = num7 - num2 + 1;
					ulong num15 = 0UL;
					uint num16 = (uint)num11;
					do
					{
						num15 += (ulong)bi2.data[(int)((UIntPtr)num13)] * (ulong)num16;
						uint num17 = data[num14];
						data[num14] -= (uint)num15;
						num15 >>= 32;
						if (data[num14] > num17)
						{
							num15 += 1UL;
						}
						num13 += 1u;
						num14++;
					}
					while ((ulong)num13 < (ulong)((long)num2));
					num14 = num7 - num2 + 1;
					num13 = 0u;
					if (num15 != 0UL)
					{
						num16 -= 1u;
						ulong num18 = 0UL;
						do
						{
							num18 = (ulong)data[num14] + (ulong)bi2.data[(int)((UIntPtr)num13)] + num18;
							data[num14] = (uint)num18;
							num18 >>= 32;
							num13 += 1u;
							num14++;
						}
						while ((ulong)num13 < (ulong)((long)num2));
					}
					bigInteger.data[num6--] = num16;
					num7--;
					i--;
				}
				bigInteger.Normalize();
				bigInteger2.Normalize();
				BigInteger[] array = new BigInteger[]
				{
					bigInteger,
					bigInteger2
				};
				if (num5 != 0)
				{
					BigInteger[] array2 = array;
					int num19 = 1;
					array2[num19] >>= num5;
				}
				return array;
			}

			public static BigInteger LeftShift(BigInteger bi, int n)
			{
				if (n == 0)
				{
					return new BigInteger(bi, bi.length + 1u);
				}
				int num = n >> 5;
				n &= 31;
				BigInteger bigInteger = new BigInteger(BigInteger.Sign.Positive, bi.length + 1u + (uint)num);
				uint num2 = 0u;
				uint length = bi.length;
				if (n != 0)
				{
					uint num3 = 0u;
					while (num2 < length)
					{
						uint num4 = bi.data[(int)((UIntPtr)num2)];
						bigInteger.data[(int)(checked((IntPtr)(unchecked((ulong)num2 + (ulong)((long)num)))))] = (num4 << n | num3);
						num3 = num4 >> 32 - n;
						num2 += 1u;
					}
					bigInteger.data[(int)(checked((IntPtr)(unchecked((ulong)num2 + (ulong)((long)num)))))] = num3;
				}
				else
				{
					while (num2 < length)
					{
						bigInteger.data[(int)(checked((IntPtr)(unchecked((ulong)num2 + (ulong)((long)num)))))] = bi.data[(int)((UIntPtr)num2)];
						num2 += 1u;
					}
				}
				bigInteger.Normalize();
				return bigInteger;
			}

			public static BigInteger RightShift(BigInteger bi, int n)
			{
				if (n == 0)
				{
					return new BigInteger(bi);
				}
				int num = n >> 5;
				int num2 = n & 31;
				BigInteger bigInteger = new BigInteger(BigInteger.Sign.Positive, bi.length - (uint)num + 1u);
				uint num3 = (uint)(bigInteger.data.Length - 1);
				if (num2 != 0)
				{
					uint num4 = 0u;
					while (num3-- > 0u)
					{
						uint num5 = bi.data[(int)(checked((IntPtr)(unchecked((ulong)num3 + (ulong)((long)num)))))];
						bigInteger.data[(int)((UIntPtr)num3)] = (num5 >> n | num4);
						num4 = num5 << 32 - n;
					}
				}
				else
				{
					while (num3-- > 0u)
					{
						bigInteger.data[(int)((UIntPtr)num3)] = bi.data[(int)(checked((IntPtr)(unchecked((ulong)num3 + (ulong)((long)num)))))];
					}
				}
				bigInteger.Normalize();
				return bigInteger;
			}

			public static BigInteger MultiplyByDword(BigInteger n, uint f)
			{
				BigInteger bigInteger = new BigInteger(BigInteger.Sign.Positive, n.length + 1u);
				uint num = 0u;
				ulong num2 = 0UL;
				do
				{
					num2 += (ulong)n.data[(int)((UIntPtr)num)] * (ulong)f;
					bigInteger.data[(int)((UIntPtr)num)] = (uint)num2;
					num2 >>= 32;
				}
				while ((num += 1u) < n.length);
				bigInteger.data[(int)((UIntPtr)num)] = (uint)num2;
				bigInteger.Normalize();
				return bigInteger;
			}

			public unsafe static void Multiply(uint[] x, uint xOffset, uint xLen, uint[] y, uint yOffset, uint yLen, uint[] d, uint dOffset)
			{
				fixed (uint* ptr = ref (x != null && x.Length != 0) ? ref x[0] : ref *null, ptr2 = ref (y != null && y.Length != 0) ? ref y[0] : ref *null, ptr3 = ref (d != null && d.Length != 0) ? ref d[0] : ref *null)
				{
					uint* ptr4 = ptr + xOffset;
					uint* ptr5 = ptr4 + xLen;
					uint* ptr6 = ptr2 + yOffset;
					uint* ptr7 = ptr6 + yLen;
					uint* ptr8 = ptr3 + dOffset;
					while (ptr4 < ptr5)
					{
						if (*ptr4 != 0u)
						{
							ulong num = 0UL;
							uint* ptr9 = ptr8;
							uint* ptr10 = ptr6;
							while (ptr10 < ptr7)
							{
								num += (ulong)(*ptr4) * (ulong)(*ptr10) + (ulong)(*ptr9);
								*ptr9 = (uint)num;
								num >>= 32;
								ptr10++;
								ptr9++;
							}
							if (num != 0UL)
							{
								*ptr9 = (uint)num;
							}
						}
						ptr4++;
						ptr8++;
					}
				}
			}

			public unsafe static void MultiplyMod2p32pmod(uint[] x, int xOffset, int xLen, uint[] y, int yOffest, int yLen, uint[] d, int dOffset, int mod)
			{
				fixed (uint* ptr = ref (x != null && x.Length != 0) ? ref x[0] : ref *null, ptr2 = ref (y != null && y.Length != 0) ? ref y[0] : ref *null, ptr3 = ref (d != null && d.Length != 0) ? ref d[0] : ref *null)
				{
					uint* ptr4 = ptr + xOffset;
					uint* ptr5 = ptr4 + xLen;
					uint* ptr6 = ptr2 + yOffest;
					uint* ptr7 = ptr6 + yLen;
					uint* ptr8 = ptr3 + dOffset;
					uint* ptr9 = ptr8 + mod;
					while (ptr4 < ptr5)
					{
						if (*ptr4 != 0u)
						{
							ulong num = 0UL;
							uint* ptr10 = ptr8;
							uint* ptr11 = ptr6;
							while (ptr11 < ptr7 && ptr10 < ptr9)
							{
								num += (ulong)(*ptr4) * (ulong)(*ptr11) + (ulong)(*ptr10);
								*ptr10 = (uint)num;
								num >>= 32;
								ptr11++;
								ptr10++;
							}
							if (num != 0UL && ptr10 < ptr9)
							{
								*ptr10 = (uint)num;
							}
						}
						ptr4++;
						ptr8++;
					}
				}
			}

			public unsafe static void SquarePositive(BigInteger bi, ref uint[] wkSpace)
			{
				uint[] array = wkSpace;
				wkSpace = bi.data;
				uint[] data = bi.data;
				uint length = bi.length;
				bi.data = array;
				fixed (uint* ptr = ref (data != null && data.Length != 0) ? ref data[0] : ref *null, ptr2 = ref (array != null && array.Length != 0) ? ref array[0] : ref *null)
				{
					uint* ptr3 = ptr2 + array.Length;
					for (uint* ptr4 = ptr2; ptr4 < ptr3; ptr4++)
					{
						*ptr4 = 0u;
					}
					uint* ptr5 = ptr;
					uint* ptr6 = ptr2;
					uint num = 0u;
					while (num < length)
					{
						if (*ptr5 != 0u)
						{
							ulong num2 = 0UL;
							uint num3 = *ptr5;
							uint* ptr7 = ptr5 + 1;
							uint* ptr8 = ptr6 + 2u * num + 1;
							uint num4 = num + 1u;
							while (num4 < length)
							{
								num2 += (ulong)num3 * (ulong)(*ptr7) + (ulong)(*ptr8);
								*ptr8 = (uint)num2;
								num2 >>= 32;
								num4 += 1u;
								ptr8++;
								ptr7++;
							}
							if (num2 != 0UL)
							{
								*ptr8 = (uint)num2;
							}
						}
						num += 1u;
						ptr5++;
					}
					ptr6 = ptr2;
					uint num5 = 0u;
					while (ptr6 < ptr3)
					{
						uint num6 = *ptr6;
						*ptr6 = (num6 << 1 | num5);
						num5 = num6 >> 31;
						ptr6++;
					}
					if (num5 != 0u)
					{
						*ptr6 = num5;
					}
					ptr5 = ptr;
					ptr6 = ptr2;
					uint* ptr9 = ptr5 + length;
					while (ptr5 < ptr9)
					{
						ulong num7 = (ulong)(*ptr5) * (ulong)(*ptr5) + (ulong)(*ptr6);
						*ptr6 = (uint)num7;
						num7 >>= 32;
						*(++ptr6) += (uint)num7;
						if (*ptr6 < (uint)num7)
						{
							uint* ptr10 = ptr6;
							*(++ptr10) += 1u;
							while (*(ptr10++) == 0u)
							{
								*ptr10 += 1u;
							}
						}
						ptr5++;
						ptr6++;
					}
					bi.length <<= 1;
					while (ptr2[bi.length - 1u] == 0u && bi.length > 1u)
					{
						bi.length -= 1u;
					}
				}
			}

			public static BigInteger gcd(BigInteger a, BigInteger b)
			{
				BigInteger bigInteger = a;
				BigInteger bigInteger2 = b;
				BigInteger bigInteger3 = bigInteger2;
				while (bigInteger.length > 1u)
				{
					bigInteger3 = bigInteger;
					bigInteger = bigInteger2 % bigInteger;
					bigInteger2 = bigInteger3;
				}
				if (bigInteger == 0u)
				{
					return bigInteger3;
				}
				uint num = bigInteger.data[0];
				uint num2 = bigInteger2 % num;
				int num3 = 0;
				while (((num2 | num) & 1u) == 0u)
				{
					num2 >>= 1;
					num >>= 1;
					num3++;
				}
				while (num2 != 0u)
				{
					while ((num2 & 1u) == 0u)
					{
						num2 >>= 1;
					}
					while ((num & 1u) == 0u)
					{
						num >>= 1;
					}
					if (num2 >= num)
					{
						num2 = num2 - num >> 1;
					}
					else
					{
						num = num - num2 >> 1;
					}
				}
				return num << num3;
			}

			public static uint modInverse(BigInteger bi, uint modulus)
			{
				uint num = modulus;
				uint num2 = bi % modulus;
				uint num3 = 0u;
				uint num4 = 1u;
				while (num2 != 0u)
				{
					if (num2 == 1u)
					{
						return num4;
					}
					num3 += num / num2 * num4;
					num %= num2;
					if (num == 0u)
					{
						break;
					}
					if (num == 1u)
					{
						return modulus - num3;
					}
					num4 += num2 / num * num3;
					num2 %= num;
				}
				return 0u;
			}

			public static BigInteger modInverse(BigInteger bi, BigInteger modulus)
			{
				if (modulus.length == 1u)
				{
					return BigInteger.Kernel.modInverse(bi, modulus.data[0]);
				}
				BigInteger[] array = new BigInteger[]
				{
					0,
					1
				};
				BigInteger[] array2 = new BigInteger[2];
				BigInteger[] array3 = new BigInteger[]
				{
					0,
					0
				};
				int num = 0;
				BigInteger bi2 = modulus;
				BigInteger bigInteger = bi;
				BigInteger.ModulusRing modulusRing = new BigInteger.ModulusRing(modulus);
				while (bigInteger != 0u)
				{
					if (num > 1)
					{
						BigInteger bigInteger2 = modulusRing.Difference(array[0], array[1] * array2[0]);
						array[0] = array[1];
						array[1] = bigInteger2;
					}
					BigInteger[] array4 = BigInteger.Kernel.multiByteDivide(bi2, bigInteger);
					array2[0] = array2[1];
					array2[1] = array4[0];
					array3[0] = array3[1];
					array3[1] = array4[1];
					bi2 = bigInteger;
					bigInteger = array4[1];
					num++;
				}
				if (array3[0] != 1u)
				{
					throw new ArithmeticException("No inverse!");
				}
				return modulusRing.Difference(array[0], array[1] * array2[0]);
			}
		}
	}
}
