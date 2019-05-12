using System;

namespace System.Xml
{
	internal class XmlConstructs
	{
		internal const int VALID = 1;

		internal const int SPACE = 2;

		internal const int NAME_START = 4;

		internal const int NAME = 8;

		internal const int PUBID = 16;

		internal const int CONTENT = 32;

		internal const int NCNAME_START = 64;

		internal const int NCNAME = 128;

		internal static readonly char[] WhitespaceChars = new char[]
		{
			' ',
			'\n',
			'\t',
			'\r'
		};

		internal static readonly byte[] CHARS = new byte[65536];

		static XmlConstructs()
		{
			int[] array = new int[]
			{
				9,
				10,
				13,
				13,
				32,
				55295,
				57344,
				65533
			};
			int[] array2 = new int[]
			{
				32,
				9,
				13,
				10
			};
			int[] array3 = new int[]
			{
				45,
				46
			};
			int[] array4 = new int[]
			{
				58,
				95
			};
			int[] array5 = new int[]
			{
				10,
				13,
				32,
				33,
				35,
				36,
				37,
				61,
				95
			};
			int[] array6 = new int[]
			{
				39,
				59,
				63,
				90,
				97,
				122
			};
			int[] array7 = new int[]
			{
				65,
				90,
				97,
				122,
				192,
				214,
				216,
				246,
				248,
				305,
				308,
				318,
				321,
				328,
				330,
				382,
				384,
				451,
				461,
				496,
				500,
				501,
				506,
				535,
				592,
				680,
				699,
				705,
				904,
				906,
				910,
				929,
				931,
				974,
				976,
				982,
				994,
				1011,
				1025,
				1036,
				1038,
				1103,
				1105,
				1116,
				1118,
				1153,
				1168,
				1220,
				1223,
				1224,
				1227,
				1228,
				1232,
				1259,
				1262,
				1269,
				1272,
				1273,
				1329,
				1366,
				1377,
				1414,
				1488,
				1514,
				1520,
				1522,
				1569,
				1594,
				1601,
				1610,
				1649,
				1719,
				1722,
				1726,
				1728,
				1742,
				1744,
				1747,
				1765,
				1766,
				2309,
				2361,
				2392,
				2401,
				2437,
				2444,
				2447,
				2448,
				2451,
				2472,
				2474,
				2480,
				2486,
				2489,
				2524,
				2525,
				2527,
				2529,
				2544,
				2545,
				2565,
				2570,
				2575,
				2576,
				2579,
				2600,
				2602,
				2608,
				2610,
				2611,
				2613,
				2614,
				2616,
				2617,
				2649,
				2652,
				2674,
				2676,
				2693,
				2699,
				2703,
				2705,
				2707,
				2728,
				2730,
				2736,
				2738,
				2739,
				2741,
				2745,
				2821,
				2828,
				2831,
				2832,
				2835,
				2856,
				2858,
				2864,
				2866,
				2867,
				2870,
				2873,
				2908,
				2909,
				2911,
				2913,
				2949,
				2954,
				2958,
				2960,
				2962,
				2965,
				2969,
				2970,
				2974,
				2975,
				2979,
				2980,
				2984,
				2986,
				2990,
				2997,
				2999,
				3001,
				3077,
				3084,
				3086,
				3088,
				3090,
				3112,
				3114,
				3123,
				3125,
				3129,
				3168,
				3169,
				3205,
				3212,
				3214,
				3216,
				3218,
				3240,
				3242,
				3251,
				3253,
				3257,
				3296,
				3297,
				3333,
				3340,
				3342,
				3344,
				3346,
				3368,
				3370,
				3385,
				3424,
				3425,
				3585,
				3630,
				3634,
				3635,
				3648,
				3653,
				3713,
				3714,
				3719,
				3720,
				3732,
				3735,
				3737,
				3743,
				3745,
				3747,
				3754,
				3755,
				3757,
				3758,
				3762,
				3763,
				3776,
				3780,
				3904,
				3911,
				3913,
				3945,
				4256,
				4293,
				4304,
				4342,
				4354,
				4355,
				4357,
				4359,
				4363,
				4364,
				4366,
				4370,
				4436,
				4437,
				4447,
				4449,
				4461,
				4462,
				4466,
				4467,
				4526,
				4527,
				4535,
				4536,
				4540,
				4546,
				7680,
				7835,
				7840,
				7929,
				7936,
				7957,
				7960,
				7965,
				7968,
				8005,
				8008,
				8013,
				8016,
				8023,
				8031,
				8061,
				8064,
				8116,
				8118,
				8124,
				8130,
				8132,
				8134,
				8140,
				8144,
				8147,
				8150,
				8155,
				8160,
				8172,
				8178,
				8180,
				8182,
				8188,
				8490,
				8491,
				8576,
				8578,
				12353,
				12436,
				12449,
				12538,
				12549,
				12588,
				44032,
				55203,
				12321,
				12329,
				19968,
				40869
			};
			int[] array8 = new int[]
			{
				902,
				908,
				986,
				988,
				990,
				992,
				1369,
				1749,
				2365,
				2482,
				2654,
				2701,
				2749,
				2784,
				2877,
				2972,
				3294,
				3632,
				3716,
				3722,
				3725,
				3749,
				3751,
				3760,
				3773,
				4352,
				4361,
				4412,
				4414,
				4416,
				4428,
				4430,
				4432,
				4441,
				4451,
				4453,
				4455,
				4457,
				4469,
				4510,
				4520,
				4523,
				4538,
				4587,
				4592,
				4601,
				8025,
				8027,
				8029,
				8126,
				8486,
				8494,
				12295
			};
			int[] array9 = new int[]
			{
				768,
				837,
				864,
				865,
				1155,
				1158,
				1425,
				1441,
				1443,
				1465,
				1467,
				1469,
				1473,
				1474,
				1611,
				1618,
				1750,
				1756,
				1757,
				1759,
				1760,
				1764,
				1767,
				1768,
				1770,
				1773,
				2305,
				2307,
				2366,
				2380,
				2385,
				2388,
				2402,
				2403,
				2433,
				2435,
				2496,
				2500,
				2503,
				2504,
				2507,
				2509,
				2530,
				2531,
				2624,
				2626,
				2631,
				2632,
				2635,
				2637,
				2672,
				2673,
				2689,
				2691,
				2750,
				2757,
				2759,
				2761,
				2763,
				2765,
				2817,
				2819,
				2878,
				2883,
				2887,
				2888,
				2891,
				2893,
				2902,
				2903,
				2946,
				2947,
				3006,
				3010,
				3014,
				3016,
				3018,
				3021,
				3073,
				3075,
				3134,
				3140,
				3142,
				3144,
				3146,
				3149,
				3157,
				3158,
				3202,
				3203,
				3262,
				3268,
				3270,
				3272,
				3274,
				3277,
				3285,
				3286,
				3330,
				3331,
				3390,
				3395,
				3398,
				3400,
				3402,
				3405,
				3636,
				3642,
				3655,
				3662,
				3764,
				3769,
				3771,
				3772,
				3784,
				3789,
				3864,
				3865,
				3953,
				3972,
				3974,
				3979,
				3984,
				3989,
				3993,
				4013,
				4017,
				4023,
				8400,
				8412,
				12330,
				12335
			};
			int[] array10 = new int[]
			{
				1471,
				1476,
				1648,
				2364,
				2381,
				2492,
				2494,
				2495,
				2519,
				2562,
				2620,
				2622,
				2623,
				2748,
				2876,
				3031,
				3415,
				3633,
				3761,
				3893,
				3895,
				3897,
				3902,
				3903,
				3991,
				4025,
				8417,
				12441,
				12442
			};
			int[] array11 = new int[]
			{
				48,
				57,
				1632,
				1641,
				1776,
				1785,
				2406,
				2415,
				2534,
				2543,
				2662,
				2671,
				2790,
				2799,
				2918,
				2927,
				3047,
				3055,
				3174,
				3183,
				3302,
				3311,
				3430,
				3439,
				3664,
				3673,
				3792,
				3801,
				3872,
				3881
			};
			int[] array12 = new int[]
			{
				12337,
				12341,
				12445,
				12446,
				12540,
				12542
			};
			int[] array13 = new int[]
			{
				183,
				720,
				721,
				903,
				1600,
				3654,
				3782,
				12293
			};
			int[] array14 = new int[]
			{
				60,
				38,
				10,
				13,
				93
			};
			for (int i = 0; i < array.Length; i += 2)
			{
				for (int j = array[i]; j <= array[i + 1]; j++)
				{
					XmlConstructs.CHARS[j] = (XmlConstructs.CHARS[j] | 1 | 32);
				}
			}
			for (int k = 0; k < array14.Length; k++)
			{
				XmlConstructs.CHARS[array14[k]] = (byte)((int)XmlConstructs.CHARS[array14[k]] & -33);
			}
			for (int l = 0; l < array2.Length; l++)
			{
				XmlConstructs.CHARS[array2[l]] = (XmlConstructs.CHARS[array2[l]] | 2);
			}
			for (int m = 0; m < array4.Length; m++)
			{
				XmlConstructs.CHARS[array4[m]] = (XmlConstructs.CHARS[array4[m]] | 4 | 8 | 64 | 128);
			}
			for (int n = 0; n < array7.Length; n += 2)
			{
				for (int num = array7[n]; num <= array7[n + 1]; num++)
				{
					XmlConstructs.CHARS[num] = (XmlConstructs.CHARS[num] | 4 | 8 | 64 | 128);
				}
			}
			for (int num2 = 0; num2 < array8.Length; num2++)
			{
				XmlConstructs.CHARS[array8[num2]] = (XmlConstructs.CHARS[array8[num2]] | 4 | 8 | 64 | 128);
			}
			for (int num3 = 0; num3 < array3.Length; num3++)
			{
				XmlConstructs.CHARS[array3[num3]] = (XmlConstructs.CHARS[array3[num3]] | 8 | 128);
			}
			for (int num4 = 0; num4 < array11.Length; num4 += 2)
			{
				for (int num5 = array11[num4]; num5 <= array11[num4 + 1]; num5++)
				{
					XmlConstructs.CHARS[num5] = (XmlConstructs.CHARS[num5] | 8 | 128);
				}
			}
			for (int num6 = 0; num6 < array9.Length; num6 += 2)
			{
				for (int num7 = array9[num6]; num7 <= array9[num6 + 1]; num7++)
				{
					XmlConstructs.CHARS[num7] = (XmlConstructs.CHARS[num7] | 8 | 128);
				}
			}
			for (int num8 = 0; num8 < array10.Length; num8++)
			{
				XmlConstructs.CHARS[array10[num8]] = (XmlConstructs.CHARS[array10[num8]] | 8 | 128);
			}
			for (int num9 = 0; num9 < array12.Length; num9 += 2)
			{
				for (int num10 = array12[num9]; num10 <= array12[num9 + 1]; num10++)
				{
					XmlConstructs.CHARS[num10] = (XmlConstructs.CHARS[num10] | 8 | 128);
				}
			}
			for (int num11 = 0; num11 < array13.Length; num11++)
			{
				XmlConstructs.CHARS[array13[num11]] = (XmlConstructs.CHARS[array13[num11]] | 8 | 128);
			}
			XmlConstructs.CHARS[58] = (byte)((int)XmlConstructs.CHARS[58] & -193);
			for (int num12 = 0; num12 < array5.Length; num12++)
			{
				XmlConstructs.CHARS[array5[num12]] = (XmlConstructs.CHARS[array5[num12]] | 16);
			}
			for (int num13 = 0; num13 < array6.Length; num13 += 2)
			{
				for (int num14 = array6[num13]; num14 <= array6[num13 + 1]; num14++)
				{
					XmlConstructs.CHARS[num14] = (XmlConstructs.CHARS[num14] | 16);
				}
			}
		}

		public static bool IsValid(char c)
		{
			return c > '\0' && (XmlConstructs.CHARS[(int)c] & 1) != 0;
		}

		public static bool IsValid(int c)
		{
			if (c > 65535)
			{
				return c < 1114112;
			}
			return c > 0 && (XmlConstructs.CHARS[c] & 1) != 0;
		}

		public static bool IsInvalid(char c)
		{
			return !XmlConstructs.IsValid(c);
		}

		public static bool IsInvalid(int c)
		{
			return !XmlConstructs.IsValid(c);
		}

		public static bool IsContent(char c)
		{
			return (XmlConstructs.CHARS[(int)c] & 32) != 0;
		}

		public static bool IsContent(int c)
		{
			return c > 0 && c < XmlConstructs.CHARS.Length && (XmlConstructs.CHARS[c] & 32) != 0;
		}

		public static bool IsMarkup(char c)
		{
			return c == '<' || c == '&' || c == '%';
		}

		public static bool IsMarkup(int c)
		{
			return c > 0 && c < XmlConstructs.CHARS.Length && (c == 60 || c == 38 || c == 37);
		}

		public static bool IsWhitespace(char c)
		{
			return (XmlConstructs.CHARS[(int)c] & 2) != 0;
		}

		public static bool IsWhitespace(int c)
		{
			return c > 0 && c < XmlConstructs.CHARS.Length && (XmlConstructs.CHARS[c] & 2) != 0;
		}

		public static bool IsFirstNameChar(char c)
		{
			return (XmlConstructs.CHARS[(int)c] & 4) != 0;
		}

		public static bool IsFirstNameChar(int c)
		{
			return c > 0 && c < XmlConstructs.CHARS.Length && (XmlConstructs.CHARS[c] & 4) != 0;
		}

		public static bool IsNameChar(char c)
		{
			return (XmlConstructs.CHARS[(int)c] & 8) != 0;
		}

		public static bool IsNameChar(int c)
		{
			return c > 0 && c < XmlConstructs.CHARS.Length && (XmlConstructs.CHARS[c] & 8) != 0;
		}

		public static bool IsNCNameStart(char c)
		{
			return (XmlConstructs.CHARS[(int)c] & 64) != 0;
		}

		public static bool IsNCNameStart(int c)
		{
			return c > 0 && c < XmlConstructs.CHARS.Length && (XmlConstructs.CHARS[c] & 64) != 0;
		}

		public static bool IsNCNameChar(char c)
		{
			return (XmlConstructs.CHARS[(int)c] & 128) != 0;
		}

		public static bool IsNCNameChar(int c)
		{
			return c > 0 && c < XmlConstructs.CHARS.Length && (XmlConstructs.CHARS[c] & 128) != 0;
		}

		public static bool IsPubidChar(char c)
		{
			return (XmlConstructs.CHARS[(int)c] & 16) != 0;
		}

		public static bool IsPubidChar(int c)
		{
			return c > 0 && c < XmlConstructs.CHARS.Length && (XmlConstructs.CHARS[c] & 16) != 0;
		}

		public static bool IsValidName(string name, out Exception err)
		{
			err = null;
			if (name.Length == 0)
			{
				err = new XmlException("Name can not be an empty string", null);
				return false;
			}
			char c = name[0];
			if (!XmlConstructs.IsFirstNameChar(c))
			{
				err = new XmlException("The character '" + c + "' cannot start a Name", null);
				return false;
			}
			for (int i = 1; i < name.Length; i++)
			{
				c = name[i];
				if (!XmlConstructs.IsNameChar(c))
				{
					err = new XmlException("The character '" + c + "' is not allowed in a Name", null);
					return false;
				}
			}
			return true;
		}

		public static int IsValidName(string name)
		{
			if (name.Length == 0)
			{
				return 0;
			}
			if (!XmlConstructs.IsFirstNameChar(name[0]))
			{
				return 0;
			}
			for (int i = 1; i < name.Length; i++)
			{
				if (!XmlConstructs.IsNameChar(name[i]))
				{
					return i;
				}
			}
			return -1;
		}

		public static bool IsValidNCName(string ncName, out Exception err)
		{
			err = null;
			if (ncName.Length == 0)
			{
				err = new XmlException("NCName can not be an empty string", null);
				return false;
			}
			char c = ncName[0];
			if (!XmlConstructs.IsNCNameStart(c))
			{
				err = new XmlException("The character '" + c + "' cannot start a NCName", null);
				return false;
			}
			for (int i = 1; i < ncName.Length; i++)
			{
				c = ncName[i];
				if (!XmlConstructs.IsNCNameChar(c))
				{
					err = new XmlException("The character '" + c + "' is not allowed in a NCName", null);
					return false;
				}
			}
			return true;
		}

		public static bool IsValidNmtoken(string nmtoken, out Exception err)
		{
			err = null;
			if (nmtoken.Length == 0)
			{
				err = new XmlException("NMTOKEN can not be an empty string", null);
				return false;
			}
			foreach (char c in nmtoken)
			{
				if (!XmlConstructs.IsNameChar(c))
				{
					err = new XmlException("The character '" + c + "' is not allowed in a NMTOKEN", null);
					return false;
				}
			}
			return true;
		}

		public static bool IsValidIANAEncoding(string ianaEncoding)
		{
			if (ianaEncoding != null)
			{
				int length = ianaEncoding.Length;
				if (length > 0)
				{
					char c = ianaEncoding[0];
					if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
					{
						for (int i = 1; i < length; i++)
						{
							c = ianaEncoding[i];
							if ((c < 'A' || c > 'Z') && (c < 'a' || c > 'z') && (c < '0' || c > '9') && c != '.' && c != '_' && c != '-')
							{
								return false;
							}
						}
						return true;
					}
				}
			}
			return false;
		}

		public static bool IsName(string str)
		{
			if (str.Length == 0)
			{
				return false;
			}
			if (!XmlConstructs.IsFirstNameChar(str[0]))
			{
				return false;
			}
			for (int i = 1; i < str.Length; i++)
			{
				if (!XmlConstructs.IsNameChar(str[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsNCName(string str)
		{
			if (str.Length == 0)
			{
				return false;
			}
			if (!XmlConstructs.IsFirstNameChar(str[0]))
			{
				return false;
			}
			for (int i = 0; i < str.Length; i++)
			{
				if (!XmlConstructs.IsNCNameChar(str[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsNmToken(string str)
		{
			if (str.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < str.Length; i++)
			{
				if (!XmlConstructs.IsNameChar(str[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsWhitespace(string str)
		{
			for (int i = 0; i < str.Length; i++)
			{
				if (!XmlConstructs.IsWhitespace(str[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static int GetPredefinedEntity(string name)
		{
			switch (name)
			{
			case "amp":
				return 38;
			case "lt":
				return 60;
			case "gt":
				return 62;
			case "quot":
				return 34;
			case "apos":
				return 39;
			}
			return -1;
		}
	}
}
