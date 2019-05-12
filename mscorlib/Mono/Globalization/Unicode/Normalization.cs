using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Mono.Globalization.Unicode
{
	internal class Normalization
	{
		public const int NoNfd = 1;

		public const int NoNfkd = 2;

		public const int NoNfc = 4;

		public const int MaybeNfc = 8;

		public const int NoNfkc = 16;

		public const int MaybeNfkc = 32;

		public const int FullCompositionExclusion = 64;

		public const int IsUnsafe = 128;

		private const int HangulSBase = 44032;

		private const int HangulLBase = 4352;

		private const int HangulVBase = 4449;

		private const int HangulTBase = 4519;

		private const int HangulLCount = 19;

		private const int HangulVCount = 21;

		private const int HangulTCount = 28;

		private const int HangulNCount = 588;

		private const int HangulSCount = 11172;

		private unsafe static byte* props;

		private unsafe static int* mappedChars;

		private unsafe static short* charMapIndex;

		private unsafe static short* helperIndex;

		private unsafe static ushort* mapIdxToComposite;

		private unsafe static byte* combiningClass;

		private static object forLock = new object();

		public static readonly bool isReady;

		unsafe static Normalization()
		{
			object obj = Normalization.forLock;
			lock (obj)
			{
				IntPtr value;
				IntPtr value2;
				IntPtr value3;
				IntPtr value4;
				IntPtr value5;
				IntPtr value6;
				Normalization.load_normalization_resource(out value, out value2, out value3, out value4, out value5, out value6);
				Normalization.props = (byte*)((void*)value);
				Normalization.mappedChars = (int*)((void*)value2);
				Normalization.charMapIndex = (short*)((void*)value3);
				Normalization.helperIndex = (short*)((void*)value4);
				Normalization.mapIdxToComposite = (ushort*)((void*)value5);
				Normalization.combiningClass = (byte*)((void*)value6);
			}
			Normalization.isReady = true;
		}

		private unsafe static uint PropValue(int cp)
		{
			return (uint)Normalization.props[NormalizationTableUtil.PropIdx(cp)];
		}

		private unsafe static int CharMapIdx(int cp)
		{
			return (int)Normalization.charMapIndex[NormalizationTableUtil.MapIdx(cp)];
		}

		private unsafe static int GetNormalizedStringLength(int ch)
		{
			int num = (int)Normalization.charMapIndex[NormalizationTableUtil.MapIdx(ch)];
			int num2 = num;
			while (Normalization.mappedChars[num2] != 0)
			{
				num2++;
			}
			return num2 - num;
		}

		private unsafe static byte GetCombiningClass(int c)
		{
			return Normalization.combiningClass[NormalizationTableUtil.Combining.ToIndex(c)];
		}

		private unsafe static int GetPrimaryCompositeFromMapIndex(int src)
		{
			return (int)Normalization.mapIdxToComposite[NormalizationTableUtil.Composite.ToIndex(src)];
		}

		private unsafe static int GetPrimaryCompositeHelperIndex(int cp)
		{
			return (int)Normalization.helperIndex[NormalizationTableUtil.Helper.ToIndex(cp)];
		}

		private unsafe static int GetPrimaryCompositeCharIndex(object chars, int start)
		{
			string text = chars as string;
			StringBuilder stringBuilder = chars as StringBuilder;
			char c = (text == null) ? stringBuilder[start] : text[start];
			int num = (stringBuilder == null) ? text.Length : stringBuilder.Length;
			int num2 = Normalization.GetPrimaryCompositeHelperIndex((int)c);
			if (num2 == 0)
			{
				return 0;
			}
			while (Normalization.mappedChars[num2] == (int)c)
			{
				int num3 = 0;
				int num4 = 1;
				int num5 = 1;
				for (;;)
				{
					int num6 = num3;
					if (Normalization.mappedChars[num2 + num4] == 0)
					{
						return num2;
					}
					if (start + num4 >= num)
					{
						return 0;
					}
					bool flag = false;
					char c2;
					do
					{
						c2 = ((text == null) ? stringBuilder[start + num5] : text[start + num5]);
						num3 = (int)Normalization.GetCombiningClass((int)c2);
						if (Normalization.mappedChars[num2 + num4] == (int)c2)
						{
							goto Block_7;
						}
						if (num3 < num6)
						{
							break;
						}
					}
					while (++num5 + start < num && num3 != 0);
					IL_105:
					if (!flag)
					{
						if (num6 >= num3)
						{
							break;
						}
						num5--;
						if (Normalization.mappedChars[num2 + num4] != (int)c2)
						{
							break;
						}
					}
					num4++;
					num5++;
					continue;
					Block_7:
					flag = true;
					goto IL_105;
				}
				while (Normalization.mappedChars[num4] != 0)
				{
					num4++;
				}
				num2 += num4 + 1;
			}
			return 0;
		}

		private static string Compose(string source, int checkType)
		{
			StringBuilder stringBuilder = null;
			Normalization.Decompose(source, ref stringBuilder, checkType);
			if (stringBuilder == null)
			{
				stringBuilder = Normalization.Combine(source, 0, checkType);
			}
			else
			{
				Normalization.Combine(stringBuilder, 0, checkType);
			}
			return (stringBuilder == null) ? source : stringBuilder.ToString();
		}

		private static StringBuilder Combine(string source, int start, int checkType)
		{
			for (int i = 0; i < source.Length; i++)
			{
				if (Normalization.QuickCheck(source[i], checkType) != NormalizationCheck.Yes)
				{
					StringBuilder stringBuilder = new StringBuilder(source.Length + source.Length / 10);
					stringBuilder.Append(source);
					Normalization.Combine(stringBuilder, i, checkType);
					return stringBuilder;
				}
			}
			return null;
		}

		private static bool CanBePrimaryComposite(int i)
		{
			if (i >= 13312 && i <= 40891)
			{
				return Normalization.GetPrimaryCompositeHelperIndex(i) != 0;
			}
			return (Normalization.PropValue(i) & 128u) != 0u;
		}

		private unsafe static void Combine(StringBuilder sb, int start, int checkType)
		{
			for (int i = start; i < sb.Length; i++)
			{
				if (Normalization.QuickCheck(sb[i], checkType) != NormalizationCheck.Yes)
				{
					int num = i;
					while (i > 0)
					{
						if (Normalization.GetCombiningClass((int)sb[i]) == 0)
						{
							break;
						}
						i--;
					}
					int num2 = 0;
					while (i < num)
					{
						num2 = Normalization.GetPrimaryCompositeMapIndex(sb, (int)sb[i], i);
						if (num2 > 0)
						{
							break;
						}
						i++;
					}
					if (num2 == 0)
					{
						i = num;
					}
					else
					{
						int primaryCompositeFromMapIndex = Normalization.GetPrimaryCompositeFromMapIndex(num2);
						int normalizedStringLength = Normalization.GetNormalizedStringLength(primaryCompositeFromMapIndex);
						if (primaryCompositeFromMapIndex == 0 || normalizedStringLength == 0)
						{
							throw new SystemException("Internal error: should not happen. Input: " + sb);
						}
						int j = 0;
						sb.Insert(i++, (char)primaryCompositeFromMapIndex);
						while (j < normalizedStringLength)
						{
							if ((int)sb[i] == Normalization.mappedChars[num2 + j])
							{
								sb.Remove(i, 1);
								j++;
							}
							else
							{
								i++;
							}
						}
						i = num - 1;
					}
				}
			}
		}

		private static int GetPrimaryCompositeMapIndex(object o, int cur, int bufferPos)
		{
			if ((Normalization.PropValue(cur) & 64u) != 0u)
			{
				return 0;
			}
			if (Normalization.GetCombiningClass(cur) != 0)
			{
				return 0;
			}
			return Normalization.GetPrimaryCompositeCharIndex(o, bufferPos);
		}

		private static string Decompose(string source, int checkType)
		{
			StringBuilder stringBuilder = null;
			Normalization.Decompose(source, ref stringBuilder, checkType);
			return (stringBuilder == null) ? source : stringBuilder.ToString();
		}

		private static void Decompose(string source, ref StringBuilder sb, int checkType)
		{
			int[] array = null;
			int num = 0;
			for (int i = 0; i < source.Length; i++)
			{
				if (Normalization.QuickCheck(source[i], checkType) == NormalizationCheck.No)
				{
					Normalization.DecomposeChar(ref sb, ref array, source, i, ref num);
				}
			}
			if (sb != null)
			{
				sb.Append(source, num, source.Length - num);
			}
			Normalization.ReorderCanonical(source, ref sb, 1);
		}

		private static void ReorderCanonical(string src, ref StringBuilder sb, int start)
		{
			if (sb == null)
			{
				for (int i = 1; i < src.Length; i++)
				{
					int num = (int)Normalization.GetCombiningClass((int)src[i]);
					if (num != 0)
					{
						if ((int)Normalization.GetCombiningClass((int)src[i - 1]) > num)
						{
							sb = new StringBuilder(src.Length);
							sb.Append(src, 0, src.Length);
							Normalization.ReorderCanonical(src, ref sb, i);
							return;
						}
					}
				}
				return;
			}
			for (int j = start; j < sb.Length; j++)
			{
				int num2 = (int)Normalization.GetCombiningClass((int)sb[j]);
				if (num2 != 0)
				{
					if ((int)Normalization.GetCombiningClass((int)sb[j - 1]) > num2)
					{
						char value = sb[j - 1];
						sb[j - 1] = sb[j];
						sb[j] = value;
						j--;
					}
				}
			}
		}

		private static void DecomposeChar(ref StringBuilder sb, ref int[] buf, string s, int i, ref int start)
		{
			if (sb == null)
			{
				sb = new StringBuilder(s.Length + 100);
			}
			sb.Append(s, start, i - start);
			if (buf == null)
			{
				buf = new int[19];
			}
			Normalization.GetCanonical((int)s[i], buf, 0);
			int num = 0;
			while (buf[num] != 0)
			{
				if (buf[num] < 65535)
				{
					sb.Append((char)buf[num]);
				}
				else
				{
					sb.Append((char)(buf[num] >> 10));
					sb.Append((char)((buf[num] & 4095) + 56320));
				}
				num++;
			}
			start = i + 1;
		}

		public static NormalizationCheck QuickCheck(char c, int type)
		{
			switch (type)
			{
			case 1:
				if ('가' <= c && c <= '힣')
				{
					return NormalizationCheck.No;
				}
				return ((Normalization.PropValue((int)c) & 1u) == 0u) ? NormalizationCheck.Yes : NormalizationCheck.No;
			case 2:
			{
				uint num = Normalization.PropValue((int)c);
				return ((num & 16u) == 0u) ? (((num & 32u) == 0u) ? NormalizationCheck.Yes : NormalizationCheck.Maybe) : NormalizationCheck.No;
			}
			case 3:
				if ('가' <= c && c <= '힣')
				{
					return NormalizationCheck.No;
				}
				return ((Normalization.PropValue((int)c) & 2u) == 0u) ? NormalizationCheck.Yes : NormalizationCheck.No;
			default:
			{
				uint num = Normalization.PropValue((int)c);
				return ((num & 4u) != 0u) ? NormalizationCheck.No : (((num & 8u) != 0u) ? NormalizationCheck.Maybe : NormalizationCheck.Yes);
			}
			}
		}

		private static bool GetCanonicalHangul(int s, int[] buf, int bufIdx)
		{
			int num = s - 44032;
			if (num < 0 || num >= 11172)
			{
				return false;
			}
			int num2 = 4352 + num / 588;
			int num3 = 4449 + num % 588 / 28;
			int num4 = 4519 + num % 28;
			buf[bufIdx++] = num2;
			buf[bufIdx++] = num3;
			if (num4 != 4519)
			{
				buf[bufIdx++] = num4;
			}
			buf[bufIdx] = 0;
			return true;
		}

		public unsafe static void GetCanonical(int c, int[] buf, int bufIdx)
		{
			if (!Normalization.GetCanonicalHangul(c, buf, bufIdx))
			{
				int num = Normalization.CharMapIdx(c);
				while (Normalization.mappedChars[num] != 0)
				{
					buf[bufIdx++] = Normalization.mappedChars[num];
					num++;
				}
				buf[bufIdx] = 0;
			}
		}

		public static bool IsNormalized(string source, int type)
		{
			int num = -1;
			for (int i = 0; i < source.Length; i++)
			{
				int num2 = (int)Normalization.GetCombiningClass((int)source[i]);
				if (num2 != 0 && num2 < num)
				{
					return false;
				}
				num = num2;
				switch (Normalization.QuickCheck(source[i], type))
				{
				case NormalizationCheck.No:
					return false;
				case NormalizationCheck.Maybe:
				{
					switch (type)
					{
					case 0:
					case 2:
						return source == Normalization.Normalize(source, type);
					}
					int num3 = i;
					while (i > 0)
					{
						if (Normalization.GetCombiningClass((int)source[i]) == 0)
						{
							break;
						}
						i--;
					}
					while (i < num3)
					{
						if (Normalization.GetPrimaryCompositeCharIndex(source, i) != 0)
						{
							return false;
						}
						i++;
					}
					break;
				}
				}
			}
			return true;
		}

		public static string Normalize(string source, int type)
		{
			switch (type)
			{
			case 1:
			case 3:
				return Normalization.Decompose(source, type);
			default:
				return Normalization.Compose(source, type);
			}
		}

		public static bool IsReady
		{
			get
			{
				return Normalization.isReady;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void load_normalization_resource(out IntPtr props, out IntPtr mappedChars, out IntPtr charMapIndex, out IntPtr helperIndex, out IntPtr mapIdxToComposite, out IntPtr combiningClass);
	}
}
