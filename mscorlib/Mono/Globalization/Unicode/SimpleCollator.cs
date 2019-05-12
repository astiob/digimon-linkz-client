using System;
using System.Globalization;

namespace Mono.Globalization.Unicode
{
	internal class SimpleCollator
	{
		private const int UnsafeFlagLength = 96;

		private static bool QuickCheckDisabled = Environment.internalGetEnvironmentVariable("MONO_COLLATION_QUICK_CHECK_DISABLED") == "yes";

		private static SimpleCollator invariant = new SimpleCollator(CultureInfo.InvariantCulture);

		private readonly TextInfo textInfo;

		private readonly bool frenchSort;

		private unsafe readonly byte* cjkCatTable;

		private unsafe readonly byte* cjkLv1Table;

		private readonly CodePointIndexer cjkIndexer;

		private unsafe readonly byte* cjkLv2Table;

		private readonly CodePointIndexer cjkLv2Indexer;

		private readonly int lcid;

		private readonly Contraction[] contractions;

		private readonly Level2Map[] level2Maps;

		private readonly byte[] unsafeFlags;

		public SimpleCollator(CultureInfo culture)
		{
			this.lcid = culture.LCID;
			this.textInfo = culture.TextInfo;
			this.SetCJKTable(culture, ref this.cjkIndexer, ref this.cjkCatTable, ref this.cjkLv1Table, ref this.cjkLv2Indexer, ref this.cjkLv2Table);
			TailoringInfo tailoringInfo = null;
			CultureInfo cultureInfo = culture;
			while (cultureInfo.LCID != 127)
			{
				tailoringInfo = MSCompatUnicodeTable.GetTailoringInfo(cultureInfo.LCID);
				if (tailoringInfo != null)
				{
					break;
				}
				cultureInfo = cultureInfo.Parent;
			}
			if (tailoringInfo == null)
			{
				tailoringInfo = MSCompatUnicodeTable.GetTailoringInfo(127);
			}
			this.frenchSort = tailoringInfo.FrenchSort;
			MSCompatUnicodeTable.BuildTailoringTables(culture, tailoringInfo, ref this.contractions, ref this.level2Maps);
			this.unsafeFlags = new byte[96];
			foreach (Contraction contraction in this.contractions)
			{
				if (contraction.Source.Length > 1)
				{
					foreach (char c in contraction.Source)
					{
						byte[] array2 = this.unsafeFlags;
						char c2 = c / '\b';
						array2[(int)c2] = (array2[(int)c2] | (byte)(1 << (int)(c & '\a')));
					}
				}
			}
			if (this.lcid != 127)
			{
				foreach (Contraction contraction2 in SimpleCollator.invariant.contractions)
				{
					if (contraction2.Source.Length > 1)
					{
						foreach (char c3 in contraction2.Source)
						{
							byte[] array4 = this.unsafeFlags;
							char c4 = c3 / '\b';
							array4[(int)c4] = (array4[(int)c4] | (byte)(1 << (int)(c3 & '\a')));
						}
					}
				}
			}
		}

		private unsafe void SetCJKTable(CultureInfo culture, ref CodePointIndexer cjkIndexer, ref byte* catTable, ref byte* lv1Table, ref CodePointIndexer lv2Indexer, ref byte* lv2Table)
		{
			string name = SimpleCollator.GetNeutralCulture(culture).Name;
			MSCompatUnicodeTable.FillCJK(name, ref cjkIndexer, ref catTable, ref lv1Table, ref lv2Indexer, ref lv2Table);
		}

		private static CultureInfo GetNeutralCulture(CultureInfo info)
		{
			CultureInfo cultureInfo = info;
			while (cultureInfo.Parent != null && cultureInfo.Parent.LCID != 127)
			{
				cultureInfo = cultureInfo.Parent;
			}
			return cultureInfo;
		}

		private unsafe byte Category(int cp)
		{
			if (cp < 12288 || this.cjkCatTable == null)
			{
				return MSCompatUnicodeTable.Category(cp);
			}
			int num = this.cjkIndexer.ToIndex(cp);
			return (num >= 0) ? this.cjkCatTable[num] : MSCompatUnicodeTable.Category(cp);
		}

		private unsafe byte Level1(int cp)
		{
			if (cp < 12288 || this.cjkLv1Table == null)
			{
				return MSCompatUnicodeTable.Level1(cp);
			}
			int num = this.cjkIndexer.ToIndex(cp);
			return (num >= 0) ? this.cjkLv1Table[num] : MSCompatUnicodeTable.Level1(cp);
		}

		private unsafe byte Level2(int cp, SimpleCollator.ExtenderType ext)
		{
			if (ext == SimpleCollator.ExtenderType.Buggy)
			{
				return 5;
			}
			if (ext == SimpleCollator.ExtenderType.Conditional)
			{
				return 0;
			}
			if (cp < 12288 || this.cjkLv2Table == null)
			{
				return MSCompatUnicodeTable.Level2(cp);
			}
			int num = this.cjkLv2Indexer.ToIndex(cp);
			byte b = (num >= 0) ? this.cjkLv2Table[num] : 0;
			if (b != 0)
			{
				return b;
			}
			b = MSCompatUnicodeTable.Level2(cp);
			if (this.level2Maps.Length == 0)
			{
				return b;
			}
			for (int i = 0; i < this.level2Maps.Length; i++)
			{
				if (this.level2Maps[i].Source == b)
				{
					return this.level2Maps[i].Replace;
				}
				if (this.level2Maps[i].Source > b)
				{
					break;
				}
			}
			return b;
		}

		private static bool IsHalfKana(int cp, CompareOptions opt)
		{
			return (opt & CompareOptions.IgnoreWidth) != CompareOptions.None || MSCompatUnicodeTable.IsHalfWidthKana((char)cp);
		}

		private Contraction GetContraction(string s, int start, int end)
		{
			Contraction contraction = this.GetContraction(s, start, end, this.contractions);
			if (contraction != null || this.lcid == 127)
			{
				return contraction;
			}
			return this.GetContraction(s, start, end, SimpleCollator.invariant.contractions);
		}

		private Contraction GetContraction(string s, int start, int end, Contraction[] clist)
		{
			foreach (Contraction contraction in clist)
			{
				int num = (int)(contraction.Source[0] - s[start]);
				if (num > 0)
				{
					return null;
				}
				if (num >= 0)
				{
					char[] source = contraction.Source;
					if (end - start >= source.Length)
					{
						bool flag = true;
						for (int j = 0; j < source.Length; j++)
						{
							if (s[start + j] != source[j])
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							return contraction;
						}
					}
				}
			}
			return null;
		}

		private Contraction GetTailContraction(string s, int start, int end)
		{
			Contraction tailContraction = this.GetTailContraction(s, start, end, this.contractions);
			if (tailContraction != null || this.lcid == 127)
			{
				return tailContraction;
			}
			return this.GetTailContraction(s, start, end, SimpleCollator.invariant.contractions);
		}

		private Contraction GetTailContraction(string s, int start, int end, Contraction[] clist)
		{
			if (start == end || end < -1 || start >= s.Length || s.Length <= end + 1)
			{
				throw new SystemException(string.Format("MONO internal error. Failed to get TailContraction. start = {0} end = {1} string = '{2}'", start, end, s));
			}
			foreach (Contraction contraction in clist)
			{
				char[] source = contraction.Source;
				if (source.Length <= start - end)
				{
					if (source[source.Length - 1] == s[start])
					{
						bool flag = true;
						int j = 0;
						int num = start - source.Length + 1;
						while (j < source.Length)
						{
							if (s[num] != source[j])
							{
								flag = false;
								break;
							}
							j++;
							num++;
						}
						if (flag)
						{
							return contraction;
						}
					}
				}
			}
			return null;
		}

		private Contraction GetContraction(char c)
		{
			Contraction contraction = this.GetContraction(c, this.contractions);
			if (contraction != null || this.lcid == 127)
			{
				return contraction;
			}
			return this.GetContraction(c, SimpleCollator.invariant.contractions);
		}

		private Contraction GetContraction(char c, Contraction[] clist)
		{
			foreach (Contraction contraction in clist)
			{
				if (contraction.Source[0] > c)
				{
					return null;
				}
				if (contraction.Source[0] == c && contraction.Source.Length == 1)
				{
					return contraction;
				}
			}
			return null;
		}

		private int FilterOptions(int i, CompareOptions opt)
		{
			if ((opt & CompareOptions.IgnoreWidth) != CompareOptions.None)
			{
				int num = MSCompatUnicodeTable.ToWidthCompat(i);
				if (num != 0)
				{
					i = num;
				}
			}
			if ((opt & CompareOptions.OrdinalIgnoreCase) != CompareOptions.None)
			{
				i = (int)this.textInfo.ToLower((char)i);
			}
			if ((opt & CompareOptions.IgnoreCase) != CompareOptions.None)
			{
				i = (int)this.textInfo.ToLower((char)i);
			}
			if ((opt & CompareOptions.IgnoreKanaType) != CompareOptions.None)
			{
				i = MSCompatUnicodeTable.ToKanaTypeInsensitive(i);
			}
			return i;
		}

		private SimpleCollator.ExtenderType GetExtenderType(int i)
		{
			if (i == 8213)
			{
				return (this.lcid != 16) ? SimpleCollator.ExtenderType.None : SimpleCollator.ExtenderType.Conditional;
			}
			if (i < 12293 || i > 65392)
			{
				return SimpleCollator.ExtenderType.None;
			}
			if (i >= 65148)
			{
				if (i == 65148 || i == 65149)
				{
					return SimpleCollator.ExtenderType.Simple;
				}
				if (i == 65438 || i == 65439)
				{
					return SimpleCollator.ExtenderType.Voiced;
				}
				if (i == 65392)
				{
					return SimpleCollator.ExtenderType.Conditional;
				}
			}
			if (i > 12542)
			{
				return SimpleCollator.ExtenderType.None;
			}
			switch (i)
			{
			case 12540:
				return SimpleCollator.ExtenderType.Conditional;
			case 12541:
				break;
			case 12542:
				return SimpleCollator.ExtenderType.Voiced;
			default:
				if (i != 12337 && i != 12338 && i != 12445)
				{
					if (i == 12446)
					{
						return SimpleCollator.ExtenderType.Voiced;
					}
					if (i != 12293)
					{
						return SimpleCollator.ExtenderType.None;
					}
					return SimpleCollator.ExtenderType.Buggy;
				}
				break;
			}
			return SimpleCollator.ExtenderType.Simple;
		}

		private static byte ToDashTypeValue(SimpleCollator.ExtenderType ext, CompareOptions opt)
		{
			if ((opt & CompareOptions.IgnoreNonSpace) != CompareOptions.None)
			{
				return 3;
			}
			switch (ext)
			{
			case SimpleCollator.ExtenderType.None:
				return 3;
			case SimpleCollator.ExtenderType.Conditional:
				return 5;
			}
			return 4;
		}

		private int FilterExtender(int i, SimpleCollator.ExtenderType ext, CompareOptions opt)
		{
			if (ext == SimpleCollator.ExtenderType.Conditional && MSCompatUnicodeTable.HasSpecialWeight((char)i))
			{
				bool flag = SimpleCollator.IsHalfKana((int)((ushort)i), opt);
				bool flag2 = !MSCompatUnicodeTable.IsHiragana((char)i);
				switch (this.Level1(i) & 7)
				{
				case 2:
					return (!flag) ? ((!flag2) ? 12354 : 12450) : 65393;
				case 3:
					return (!flag) ? ((!flag2) ? 12356 : 12452) : 65394;
				case 4:
					return (!flag) ? ((!flag2) ? 12358 : 12454) : 65395;
				case 5:
					return (!flag) ? ((!flag2) ? 12360 : 12456) : 65396;
				case 6:
					return (!flag) ? ((!flag2) ? 12362 : 12458) : 65397;
				}
			}
			return i;
		}

		private static bool IsIgnorable(int i, CompareOptions opt)
		{
			return MSCompatUnicodeTable.IsIgnorable(i, (byte)(1 + (((opt & CompareOptions.IgnoreSymbols) == CompareOptions.None) ? 0 : 2) + (((opt & CompareOptions.IgnoreNonSpace) == CompareOptions.None) ? 0 : 4)));
		}

		private bool IsSafe(int i)
		{
			return i / 8 >= this.unsafeFlags.Length || ((int)this.unsafeFlags[i / 8] & 1 << i % 8) == 0;
		}

		public SortKey GetSortKey(string s)
		{
			return this.GetSortKey(s, CompareOptions.None);
		}

		public SortKey GetSortKey(string s, CompareOptions options)
		{
			return this.GetSortKey(s, 0, s.Length, options);
		}

		public SortKey GetSortKey(string s, int start, int length, CompareOptions options)
		{
			SortKeyBuffer sortKeyBuffer = new SortKeyBuffer(this.lcid);
			sortKeyBuffer.Initialize(options, this.lcid, s, this.frenchSort);
			int end = start + length;
			this.GetSortKey(s, start, end, sortKeyBuffer, options);
			return sortKeyBuffer.GetResultAndReset();
		}

		private unsafe void GetSortKey(string s, int start, int end, SortKeyBuffer buf, CompareOptions opt)
		{
			byte* ptr = stackalloc byte[checked(4 * 1)];
			this.ClearBuffer(ptr, 4);
			SimpleCollator.Context context = new SimpleCollator.Context(opt, null, null, null, null, ptr, false);
			for (int i = start; i < end; i++)
			{
				int num = (int)s[i];
				SimpleCollator.ExtenderType extenderType = this.GetExtenderType(num);
				if (extenderType != SimpleCollator.ExtenderType.None)
				{
					num = this.FilterExtender(context.PrevCode, extenderType, opt);
					if (num >= 0)
					{
						this.FillSortKeyRaw(num, extenderType, buf, opt);
					}
					else if (context.PrevSortKey != null)
					{
						byte* prevSortKey = context.PrevSortKey;
						buf.AppendNormal(*prevSortKey, prevSortKey[1], (prevSortKey[2] == 1) ? this.Level2(num, extenderType) : prevSortKey[2], (prevSortKey[3] == 1) ? MSCompatUnicodeTable.Level3(num) : prevSortKey[3]);
					}
				}
				else if (!SimpleCollator.IsIgnorable(num, opt))
				{
					num = this.FilterOptions(num, opt);
					Contraction contraction = this.GetContraction(s, i, end);
					if (contraction != null)
					{
						if (contraction.Replacement != null)
						{
							this.GetSortKey(contraction.Replacement, 0, contraction.Replacement.Length, buf, opt);
						}
						else
						{
							byte* prevSortKey2 = context.PrevSortKey;
							for (int j = 0; j < contraction.SortKey.Length; j++)
							{
								prevSortKey2[j] = contraction.SortKey[j];
							}
							buf.AppendNormal(*prevSortKey2, prevSortKey2[1], (prevSortKey2[2] == 1) ? this.Level2(num, extenderType) : prevSortKey2[2], (prevSortKey2[3] == 1) ? MSCompatUnicodeTable.Level3(num) : prevSortKey2[3]);
							context.PrevCode = -1;
						}
						i += contraction.Source.Length - 1;
					}
					else
					{
						if (!MSCompatUnicodeTable.IsIgnorableNonSpacing(num))
						{
							context.PrevCode = num;
						}
						this.FillSortKeyRaw(num, SimpleCollator.ExtenderType.None, buf, opt);
					}
				}
			}
		}

		private void FillSortKeyRaw(int i, SimpleCollator.ExtenderType ext, SortKeyBuffer buf, CompareOptions opt)
		{
			if (13312 <= i && i <= 19893)
			{
				int num = i - 13312;
				buf.AppendCJKExtension((byte)(16 + num / 254), (byte)(num % 254 + 2));
				return;
			}
			UnicodeCategory unicodeCategory = char.GetUnicodeCategory((char)i);
			UnicodeCategory unicodeCategory2 = unicodeCategory;
			if (unicodeCategory2 == UnicodeCategory.Surrogate)
			{
				this.FillSurrogateSortKeyRaw(i, buf);
				return;
			}
			if (unicodeCategory2 != UnicodeCategory.PrivateUse)
			{
				byte lv = this.Level2(i, ext);
				if (MSCompatUnicodeTable.HasSpecialWeight((char)i))
				{
					byte lv2 = this.Level1(i);
					buf.AppendKana(this.Category(i), lv2, lv, MSCompatUnicodeTable.Level3(i), MSCompatUnicodeTable.IsJapaneseSmallLetter((char)i), SimpleCollator.ToDashTypeValue(ext, opt), !MSCompatUnicodeTable.IsHiragana((char)i), SimpleCollator.IsHalfKana((int)((ushort)i), opt));
					if ((opt & CompareOptions.IgnoreNonSpace) == CompareOptions.None && ext == SimpleCollator.ExtenderType.Voiced)
					{
						buf.AppendNormal(1, 1, 1, 0);
					}
				}
				else
				{
					buf.AppendNormal(this.Category(i), this.Level1(i), lv, MSCompatUnicodeTable.Level3(i));
				}
				return;
			}
			int num2 = i - 57344;
			buf.AppendNormal((byte)(229 + num2 / 254), (byte)(num2 % 254 + 2), 0, 0);
		}

		private void FillSurrogateSortKeyRaw(int i, SortKeyBuffer buf)
		{
			int num;
			int num2;
			byte b;
			if (i < 55360)
			{
				num = 55296;
				num2 = 65;
				b = ((i != 55296) ? 63 : 62);
			}
			else if (55360 <= i && i < 55424)
			{
				num = 55360;
				num2 = 242;
				b = 62;
			}
			else if (56192 <= i && i < 56320)
			{
				num = 56128;
				num2 = 254;
				b = 62;
			}
			else
			{
				num = 56074;
				num2 = 65;
				b = 63;
			}
			int num3 = i - num;
			buf.AppendNormal((byte)(num2 + num3 / 254), (byte)(num3 % 254 + 2), b, b);
		}

		public int Compare(string s1, string s2)
		{
			return this.Compare(s1, s2, CompareOptions.None);
		}

		public int Compare(string s1, string s2, CompareOptions options)
		{
			return this.Compare(s1, 0, s1.Length, s2, 0, s2.Length, options);
		}

		private int CompareOrdinal(string s1, int idx1, int len1, string s2, int idx2, int len2)
		{
			int num = (len1 >= len2) ? len2 : len1;
			int num2 = idx1 + num;
			int num3 = idx2 + num;
			if (idx1 < 0 || idx2 < 0 || num2 > s1.Length || num3 > s2.Length)
			{
				throw new SystemException(string.Format("CompareInfo Internal Error: Should not happen. {0} {1} {2} {3} {4} {5}", new object[]
				{
					idx1,
					idx2,
					len1,
					len2,
					s1.Length,
					s2.Length
				}));
			}
			int num4 = idx1;
			int num5 = idx2;
			while (num4 < num2 && num5 < num3)
			{
				if (s1[num4] != s2[num5])
				{
					return (int)(s1[num4] - s2[num5]);
				}
				num4++;
				num5++;
			}
			return (len1 != len2) ? ((len1 != num) ? 1 : -1) : 0;
		}

		private int CompareQuick(string s1, int idx1, int len1, string s2, int idx2, int len2, out bool sourceConsumed, out bool targetConsumed, bool immediateBreakup)
		{
			sourceConsumed = false;
			targetConsumed = false;
			int num = (len1 >= len2) ? len2 : len1;
			int num2 = idx1 + num;
			int num3 = idx2 + num;
			if (idx1 < 0 || idx2 < 0 || num2 > s1.Length || num3 > s2.Length)
			{
				throw new SystemException(string.Format("CompareInfo Internal Error: Should not happen. {0} {1} {2} {3} {4} {5}", new object[]
				{
					idx1,
					idx2,
					len1,
					len2,
					s1.Length,
					s2.Length
				}));
			}
			int num4 = idx1;
			int num5 = idx2;
			while (num4 < num2 && num5 < num3)
			{
				if (s1[num4] != s2[num5])
				{
					if (immediateBreakup)
					{
						return -1;
					}
					int num6 = (int)(this.Category((int)s1[num4]) - this.Category((int)s2[num5]));
					if (num6 == 0)
					{
						num6 = (int)(this.Level1((int)s1[num4]) - this.Level1((int)s2[num5]));
					}
					if (num6 == 0)
					{
						num6 = (int)(MSCompatUnicodeTable.Level3((int)s1[num4]) - MSCompatUnicodeTable.Level3((int)s2[num5]));
					}
					if (num6 == 0)
					{
						throw new SystemException(string.Format("CompareInfo Internal Error: Should not happen. '{0}' {2} {3} '{1}' {4} {5}", new object[]
						{
							s1,
							s2,
							idx1,
							num2,
							idx2,
							num3
						}));
					}
					return num6;
				}
				else
				{
					num4++;
					num5++;
				}
			}
			sourceConsumed = (len1 <= len2);
			targetConsumed = (len1 >= len2);
			return (len1 != len2) ? ((len1 != num) ? 1 : -1) : 0;
		}

		private int CompareOrdinalIgnoreCase(string s1, int idx1, int len1, string s2, int idx2, int len2)
		{
			int num = (len1 >= len2) ? len2 : len1;
			int num2 = idx1 + num;
			int num3 = idx2 + num;
			if (idx1 < 0 || idx2 < 0 || num2 > s1.Length || num3 > s2.Length)
			{
				throw new SystemException(string.Format("CompareInfo Internal Error: Should not happen. {0} {1} {2} {3} {4} {5}", new object[]
				{
					idx1,
					idx2,
					len1,
					len2,
					s1.Length,
					s2.Length
				}));
			}
			TextInfo textInfo = SimpleCollator.invariant.textInfo;
			int num4 = idx1;
			int num5 = idx2;
			while (num4 < num2 && num5 < num3)
			{
				if (textInfo.ToLower(s1[num4]) != textInfo.ToLower(s2[num5]))
				{
					return (int)(textInfo.ToLower(s1[num4]) - textInfo.ToLower(s2[num5]));
				}
				num4++;
				num5++;
			}
			return (len1 != len2) ? ((len1 != num) ? 1 : -1) : 0;
		}

		public unsafe int Compare(string s1, int idx1, int len1, string s2, int idx2, int len2, CompareOptions options)
		{
			if (idx1 == idx2 && len1 == len2 && object.ReferenceEquals(s1, s2))
			{
				return 0;
			}
			if (options == CompareOptions.Ordinal)
			{
				return this.CompareOrdinal(s1, idx1, len1, s2, idx2, len2);
			}
			if (options == CompareOptions.OrdinalIgnoreCase)
			{
				return this.CompareOrdinalIgnoreCase(s1, idx1, len1, s2, idx2, len2);
			}
			byte* ptr;
			byte* ptr2;
			checked
			{
				ptr = stackalloc byte[4 * 1];
				ptr2 = stackalloc byte[4 * 1];
				this.ClearBuffer(ptr, 4);
				this.ClearBuffer(ptr2, 4);
			}
			SimpleCollator.Context context = new SimpleCollator.Context(options, null, null, ptr, ptr2, null, this.QuickCheckPossible(s1, idx1, idx1 + len1, s2, idx2, idx2 + len2));
			bool flag;
			bool flag2;
			int num = this.CompareInternal(s1, idx1, len1, s2, idx2, len2, out flag, out flag2, true, false, ref context);
			return (num != 0) ? ((num >= 0) ? 1 : -1) : 0;
		}

		private unsafe void ClearBuffer(byte* buffer, int size)
		{
			for (int i = 0; i < size; i++)
			{
				buffer[i] = 0;
			}
		}

		private bool QuickCheckPossible(string s1, int idx1, int end1, string s2, int idx2, int end2)
		{
			return false;
		}

		private unsafe int CompareInternal(string s1, int idx1, int len1, string s2, int idx2, int len2, out bool targetConsumed, out bool sourceConsumed, bool skipHeadingExtenders, bool immediateBreakup, ref SimpleCollator.Context ctx)
		{
			CompareOptions option = ctx.Option;
			int num = idx1;
			int num2 = idx2;
			int num3 = idx1 + len1;
			int num4 = idx2 + len2;
			targetConsumed = false;
			sourceConsumed = false;
			SimpleCollator.PreviousInfo previousInfo = new SimpleCollator.PreviousInfo(false);
			if (option == CompareOptions.None && ctx.QuickCheckPossible)
			{
				return this.CompareQuick(s1, idx1, len1, s2, idx2, len2, out sourceConsumed, out targetConsumed, immediateBreakup);
			}
			int num5 = 0;
			int num6 = 5;
			int num7 = -1;
			int num8 = -1;
			int num9 = 0;
			int num10 = 0;
			if (skipHeadingExtenders)
			{
				while (idx1 < num3)
				{
					if (this.GetExtenderType((int)s1[idx1]) == SimpleCollator.ExtenderType.None)
					{
						break;
					}
					idx1++;
				}
				while (idx2 < num4)
				{
					if (this.GetExtenderType((int)s2[idx2]) == SimpleCollator.ExtenderType.None)
					{
						break;
					}
					idx2++;
				}
			}
			SimpleCollator.ExtenderType extenderType = SimpleCollator.ExtenderType.None;
			SimpleCollator.ExtenderType extenderType2 = SimpleCollator.ExtenderType.None;
			int num11 = idx1;
			int num12 = idx2;
			bool flag = (option & CompareOptions.StringSort) != CompareOptions.None;
			bool flag2 = (option & CompareOptions.IgnoreNonSpace) != CompareOptions.None;
			SimpleCollator.Escape escape = default(SimpleCollator.Escape);
			SimpleCollator.Escape escape2 = default(SimpleCollator.Escape);
			for (;;)
			{
				while (idx1 < num3)
				{
					if (!SimpleCollator.IsIgnorable((int)s1[idx1], option))
					{
						break;
					}
					idx1++;
				}
				while (idx2 < num4)
				{
					if (!SimpleCollator.IsIgnorable((int)s2[idx2], option))
					{
						break;
					}
					idx2++;
				}
				if (idx1 >= num3)
				{
					if (escape.Source == null)
					{
						break;
					}
					s1 = escape.Source;
					num = escape.Start;
					idx1 = escape.Index;
					num3 = escape.End;
					num11 = escape.Optional;
					escape.Source = null;
				}
				else if (idx2 >= num4)
				{
					if (escape2.Source == null)
					{
						break;
					}
					s2 = escape2.Source;
					num2 = escape2.Start;
					idx2 = escape2.Index;
					num4 = escape2.End;
					num12 = escape2.Optional;
					escape2.Source = null;
				}
				else
				{
					if (num11 < idx1 && num12 < idx2)
					{
						while (idx1 < num3 && idx2 < num4 && s1[idx1] == s2[idx2])
						{
							idx1++;
							idx2++;
						}
						if (idx1 == num3 || idx2 == num4)
						{
							continue;
						}
						int num13 = num11;
						int num14 = num12;
						num11 = idx1;
						num12 = idx2;
						idx1--;
						idx2--;
						while (idx1 > num13)
						{
							if (this.Category((int)s1[idx1]) != 1)
							{
								break;
							}
							idx1--;
						}
						while (idx2 > num14)
						{
							if (this.Category((int)s2[idx2]) != 1)
							{
								break;
							}
							idx2--;
						}
						while (idx1 > num13)
						{
							if (this.IsSafe((int)s1[idx1]))
							{
								break;
							}
							idx1--;
						}
						while (idx2 > num14)
						{
							if (this.IsSafe((int)s2[idx2]))
							{
								break;
							}
							idx2--;
						}
					}
					int num15 = idx1;
					int num16 = idx2;
					byte* ptr = null;
					byte* ptr2 = null;
					int num17 = this.FilterOptions((int)s1[idx1], option);
					int num18 = this.FilterOptions((int)s2[idx2], option);
					bool flag3 = false;
					bool flag4 = false;
					extenderType = this.GetExtenderType(num17);
					if (extenderType != SimpleCollator.ExtenderType.None)
					{
						if (ctx.PrevCode < 0)
						{
							if (ctx.PrevSortKey == null)
							{
								idx1++;
								continue;
							}
							ptr = ctx.PrevSortKey;
						}
						else
						{
							num17 = this.FilterExtender(ctx.PrevCode, extenderType, option);
						}
					}
					extenderType2 = this.GetExtenderType(num18);
					if (extenderType2 != SimpleCollator.ExtenderType.None)
					{
						if (previousInfo.Code < 0)
						{
							if (previousInfo.SortKey == null)
							{
								idx2++;
								continue;
							}
							ptr2 = previousInfo.SortKey;
						}
						else
						{
							num18 = this.FilterExtender(previousInfo.Code, extenderType2, option);
						}
					}
					byte b = this.Category(num17);
					byte b2 = this.Category(num18);
					if (b == 6)
					{
						if (!flag && num6 == 5)
						{
							num7 = ((escape.Source == null) ? (num15 - num) : (escape.Index - escape.Start));
							num9 = (int)this.Level1(num17) << (int)(8 + MSCompatUnicodeTable.Level3(num17));
						}
						ctx.PrevCode = num17;
						idx1++;
					}
					if (b2 == 6)
					{
						if (!flag && num6 == 5)
						{
							num8 = ((escape2.Source == null) ? (num16 - num2) : (escape2.Index - escape2.Start));
							num10 = (int)this.Level1(num18) << (int)(8 + MSCompatUnicodeTable.Level3(num18));
						}
						previousInfo.Code = num18;
						idx2++;
					}
					if (b == 6 || b2 == 6)
					{
						if (num6 == 5)
						{
							if (num9 == num10)
							{
								num8 = (num7 = -1);
								num10 = (num9 = 0);
							}
							else
							{
								num6 = 4;
							}
						}
					}
					else
					{
						Contraction contraction = null;
						if (extenderType == SimpleCollator.ExtenderType.None)
						{
							contraction = this.GetContraction(s1, idx1, num3);
						}
						int num19 = 1;
						if (ptr != null)
						{
							num19 = 1;
						}
						else if (contraction != null)
						{
							num19 = contraction.Source.Length;
							if (contraction.SortKey != null)
							{
								ptr = ctx.Buffer1;
								for (int i = 0; i < contraction.SortKey.Length; i++)
								{
									ptr[i] = contraction.SortKey[i];
								}
								ctx.PrevCode = -1;
								ctx.PrevSortKey = ptr;
							}
							else if (escape.Source == null)
							{
								escape.Source = s1;
								escape.Start = num;
								escape.Index = num15 + contraction.Source.Length;
								escape.End = num3;
								escape.Optional = num11;
								s1 = contraction.Replacement;
								idx1 = 0;
								num = 0;
								num3 = s1.Length;
								num11 = 0;
								continue;
							}
						}
						else
						{
							ptr = ctx.Buffer1;
							*ptr = b;
							ptr[1] = this.Level1(num17);
							if (!flag2 && num6 > 1)
							{
								ptr[2] = this.Level2(num17, extenderType);
							}
							if (num6 > 2)
							{
								ptr[3] = MSCompatUnicodeTable.Level3(num17);
							}
							if (num6 > 3)
							{
								flag3 = MSCompatUnicodeTable.HasSpecialWeight((char)num17);
							}
							if (b > 1)
							{
								ctx.PrevCode = num17;
							}
						}
						Contraction contraction2 = null;
						if (extenderType2 == SimpleCollator.ExtenderType.None)
						{
							contraction2 = this.GetContraction(s2, idx2, num4);
						}
						if (ptr2 != null)
						{
							idx2++;
						}
						else if (contraction2 != null)
						{
							idx2 += contraction2.Source.Length;
							if (contraction2.SortKey != null)
							{
								ptr2 = ctx.Buffer2;
								for (int j = 0; j < contraction2.SortKey.Length; j++)
								{
									ptr2[j] = contraction2.SortKey[j];
								}
								previousInfo.Code = -1;
								previousInfo.SortKey = ptr2;
							}
							else if (escape2.Source == null)
							{
								escape2.Source = s2;
								escape2.Start = num2;
								escape2.Index = num16 + contraction2.Source.Length;
								escape2.End = num4;
								escape2.Optional = num12;
								s2 = contraction2.Replacement;
								idx2 = 0;
								num2 = 0;
								num4 = s2.Length;
								num12 = 0;
								continue;
							}
						}
						else
						{
							ptr2 = ctx.Buffer2;
							*ptr2 = b2;
							ptr2[1] = this.Level1(num18);
							if (!flag2 && num6 > 1)
							{
								ptr2[2] = this.Level2(num18, extenderType2);
							}
							if (num6 > 2)
							{
								ptr2[3] = MSCompatUnicodeTable.Level3(num18);
							}
							if (num6 > 3)
							{
								flag4 = MSCompatUnicodeTable.HasSpecialWeight((char)num18);
							}
							if (b2 > 1)
							{
								previousInfo.Code = num18;
							}
							idx2++;
						}
						idx1 += num19;
						if (!flag2)
						{
							while (idx1 < num3)
							{
								if (this.Category((int)s1[idx1]) != 1)
								{
									break;
								}
								if (ptr[2] == 0)
								{
									ptr[2] = 2;
								}
								ptr[2] = ptr[2] + this.Level2((int)s1[idx1], SimpleCollator.ExtenderType.None);
								idx1++;
							}
							while (idx2 < num4)
							{
								if (this.Category((int)s2[idx2]) != 1)
								{
									break;
								}
								if (ptr2[2] == 0)
								{
									ptr2[2] = 2;
								}
								ptr2[2] = ptr2[2] + this.Level2((int)s2[idx2], SimpleCollator.ExtenderType.None);
								idx2++;
							}
						}
						int num20 = (int)(*ptr - *ptr2);
						num20 = ((num20 == 0) ? ((int)(ptr[1] - ptr2[1])) : num20);
						if (num20 != 0)
						{
							return num20;
						}
						if (num6 != 1)
						{
							if (!flag2)
							{
								num20 = (int)(ptr[2] - ptr2[2]);
								if (num20 != 0)
								{
									num5 = num20;
									if (immediateBreakup)
									{
										return -1;
									}
									num6 = ((!this.frenchSort) ? 1 : 2);
									continue;
								}
							}
							if (num6 != 2)
							{
								num20 = (int)(ptr[3] - ptr2[3]);
								if (num20 != 0)
								{
									num5 = num20;
									if (immediateBreakup)
									{
										return -1;
									}
									num6 = 2;
								}
								else if (num6 != 3)
								{
									if (flag3 != flag4)
									{
										if (immediateBreakup)
										{
											return -1;
										}
										num5 = ((!flag3) ? -1 : 1);
										num6 = 3;
									}
									else if (flag3)
									{
										num20 = this.CompareFlagPair(!MSCompatUnicodeTable.IsJapaneseSmallLetter((char)num17), !MSCompatUnicodeTable.IsJapaneseSmallLetter((char)num18));
										num20 = ((num20 == 0) ? ((int)(SimpleCollator.ToDashTypeValue(extenderType, option) - SimpleCollator.ToDashTypeValue(extenderType2, option))) : num20);
										num20 = ((num20 == 0) ? this.CompareFlagPair(MSCompatUnicodeTable.IsHiragana((char)num17), MSCompatUnicodeTable.IsHiragana((char)num18)) : num20);
										num20 = ((num20 == 0) ? this.CompareFlagPair(!SimpleCollator.IsHalfKana((int)((ushort)num17), option), !SimpleCollator.IsHalfKana((int)((ushort)num18), option)) : num20);
										if (num20 != 0)
										{
											if (immediateBreakup)
											{
												return -1;
											}
											num5 = num20;
											num6 = 3;
										}
									}
								}
							}
						}
					}
				}
			}
			if (!flag2 && num5 != 0 && num6 > 2)
			{
				while (idx1 < num3 && idx2 < num4)
				{
					if (!MSCompatUnicodeTable.IsIgnorableNonSpacing((int)s1[idx1]))
					{
						break;
					}
					if (!MSCompatUnicodeTable.IsIgnorableNonSpacing((int)s2[idx2]))
					{
						break;
					}
					num5 = (int)(this.Level2(this.FilterOptions((int)s1[idx1], option), extenderType) - this.Level2(this.FilterOptions((int)s2[idx2], option), extenderType2));
					if (num5 != 0)
					{
						break;
					}
					idx1++;
					idx2++;
					extenderType = SimpleCollator.ExtenderType.None;
					extenderType2 = SimpleCollator.ExtenderType.None;
				}
			}
			if (num6 == 1 && num5 != 0)
			{
				while (idx1 < num3)
				{
					if (!MSCompatUnicodeTable.IsIgnorableNonSpacing((int)s1[idx1]))
					{
						break;
					}
					idx1++;
				}
				while (idx2 < num4)
				{
					if (!MSCompatUnicodeTable.IsIgnorableNonSpacing((int)s2[idx2]))
					{
						break;
					}
					idx2++;
				}
			}
			if (num5 == 0)
			{
				if (num7 < 0 && num8 >= 0)
				{
					num5 = -1;
				}
				else if (num8 < 0 && num7 >= 0)
				{
					num5 = 1;
				}
				else
				{
					num5 = num7 - num8;
					if (num5 == 0)
					{
						num5 = num9 - num10;
					}
				}
			}
			if (num5 == 0)
			{
				if (idx2 == num4)
				{
					targetConsumed = true;
				}
				if (idx1 == num3)
				{
					sourceConsumed = true;
				}
			}
			return (idx1 == num3) ? ((idx2 != num4) ? -1 : num5) : 1;
		}

		private int CompareFlagPair(bool b1, bool b2)
		{
			return (b1 != b2) ? ((!b1) ? -1 : 1) : 0;
		}

		public bool IsPrefix(string src, string target, CompareOptions opt)
		{
			return this.IsPrefix(src, target, 0, src.Length, opt);
		}

		public unsafe bool IsPrefix(string s, string target, int start, int length, CompareOptions opt)
		{
			if (target.Length == 0)
			{
				return true;
			}
			byte* ptr;
			byte* ptr2;
			checked
			{
				ptr = stackalloc byte[4 * 1];
				ptr2 = stackalloc byte[4 * 1];
				this.ClearBuffer(ptr, 4);
				this.ClearBuffer(ptr2, 4);
			}
			SimpleCollator.Context context = new SimpleCollator.Context(opt, null, null, ptr, ptr2, null, this.QuickCheckPossible(s, start, start + length, target, 0, target.Length));
			return this.IsPrefix(s, target, start, length, true, ref context);
		}

		private bool IsPrefix(string s, string target, int start, int length, bool skipHeadingExtenders, ref SimpleCollator.Context ctx)
		{
			bool result;
			bool flag;
			this.CompareInternal(s, start, length, target, 0, target.Length, out result, out flag, skipHeadingExtenders, true, ref ctx);
			return result;
		}

		public bool IsSuffix(string src, string target, CompareOptions opt)
		{
			return this.IsSuffix(src, target, src.Length - 1, src.Length, opt);
		}

		public bool IsSuffix(string s, string target, int start, int length, CompareOptions opt)
		{
			if (target.Length == 0)
			{
				return true;
			}
			int num = this.LastIndexOf(s, target, start, length, opt);
			return num >= 0 && this.Compare(s, num, s.Length - num, target, 0, target.Length, opt) == 0;
		}

		public int IndexOf(string s, string target, CompareOptions opt)
		{
			return this.IndexOf(s, target, 0, s.Length, opt);
		}

		private int QuickIndexOf(string s, string target, int start, int length, out bool testWasUnable)
		{
			int num = -1;
			int num2 = -1;
			testWasUnable = true;
			if (target.Length == 0)
			{
				return 0;
			}
			if (target.Length > length)
			{
				return -1;
			}
			testWasUnable = false;
			int num3 = start + length - target.Length + 1;
			for (int i = start; i < num3; i++)
			{
				bool flag = false;
				for (int j = 0; j < target.Length; j++)
				{
					if (num2 < j)
					{
						if (target[j] >= '\u0080')
						{
							testWasUnable = true;
							return -1;
						}
						num2 = j;
					}
					if (num < i + j)
					{
						if (s[i + j] >= '\u0080')
						{
							testWasUnable = true;
							return -1;
						}
						num = i + j;
					}
					if (s[i + j] != target[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return i;
				}
			}
			return -1;
		}

		public unsafe int IndexOf(string s, string target, int start, int length, CompareOptions opt)
		{
			if (opt == CompareOptions.Ordinal)
			{
				return this.IndexOfOrdinal(s, target, start, length);
			}
			if (opt == CompareOptions.OrdinalIgnoreCase)
			{
				return this.IndexOfOrdinalIgnoreCase(s, target, start, length);
			}
			if (opt == CompareOptions.None)
			{
				bool flag;
				int result = this.QuickIndexOf(s, target, start, length, out flag);
				if (!flag)
				{
					return result;
				}
			}
			checked
			{
				byte* ptr = stackalloc byte[16 * 1];
				byte* ptr2 = stackalloc byte[16 * 1];
				byte* ptr3 = stackalloc byte[4 * 1];
				byte* ptr4 = stackalloc byte[4 * 1];
				byte* ptr5 = stackalloc byte[4 * 1];
				this.ClearBuffer(ptr, 16);
				this.ClearBuffer(ptr2, 16);
				this.ClearBuffer(ptr3, 4);
				this.ClearBuffer(ptr4, 4);
				this.ClearBuffer(ptr5, 4);
				SimpleCollator.Context context = new SimpleCollator.Context(opt, ptr, ptr2, ptr4, ptr5, null, false);
				return this.IndexOf(s, target, start, length, ptr3, ref context);
			}
		}

		private int IndexOfOrdinal(string s, string target, int start, int length)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			if (target.Length > length)
			{
				return -1;
			}
			int num = start + length - target.Length + 1;
			for (int i = start; i < num; i++)
			{
				bool flag = false;
				for (int j = 0; j < target.Length; j++)
				{
					if (s[i + j] != target[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return i;
				}
			}
			return -1;
		}

		private int IndexOfOrdinalIgnoreCase(string s, string target, int start, int length)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			if (target.Length > length)
			{
				return -1;
			}
			int num = start + length - target.Length + 1;
			for (int i = start; i < num; i++)
			{
				bool flag = false;
				for (int j = 0; j < target.Length; j++)
				{
					if (this.textInfo.ToLower(s[i + j]) != this.textInfo.ToLower(target[j]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return i;
				}
			}
			return -1;
		}

		public int IndexOf(string s, char target, CompareOptions opt)
		{
			return this.IndexOf(s, target, 0, s.Length, opt);
		}

		public unsafe int IndexOf(string s, char target, int start, int length, CompareOptions opt)
		{
			if (opt == CompareOptions.Ordinal)
			{
				return this.IndexOfOrdinal(s, target, start, length);
			}
			if (opt == CompareOptions.OrdinalIgnoreCase)
			{
				return this.IndexOfOrdinalIgnoreCase(s, target, start, length);
			}
			byte* ptr3;
			byte* ptr5;
			SimpleCollator.Context context;
			Contraction contraction;
			checked
			{
				byte* ptr = stackalloc byte[16 * 1];
				byte* ptr2 = stackalloc byte[16 * 1];
				ptr3 = stackalloc byte[4 * 1];
				byte* ptr4 = stackalloc byte[4 * 1];
				ptr5 = stackalloc byte[4 * 1];
				this.ClearBuffer(ptr, 16);
				this.ClearBuffer(ptr2, 16);
				this.ClearBuffer(ptr3, 4);
				this.ClearBuffer(ptr4, 4);
				this.ClearBuffer(ptr5, 4);
				context = new SimpleCollator.Context(opt, ptr, ptr2, ptr4, ptr5, null, false);
				contraction = this.GetContraction(target);
			}
			if (contraction == null)
			{
				int num = this.FilterOptions((int)target, opt);
				*ptr3 = this.Category(num);
				ptr3[1] = this.Level1(num);
				if ((opt & CompareOptions.IgnoreNonSpace) == CompareOptions.None)
				{
					ptr3[2] = this.Level2(num, SimpleCollator.ExtenderType.None);
				}
				ptr3[3] = MSCompatUnicodeTable.Level3(num);
				return this.IndexOfSortKey(s, start, length, ptr3, target, num, !MSCompatUnicodeTable.HasSpecialWeight((char)num), ref context);
			}
			if (contraction.Replacement != null)
			{
				return this.IndexOf(s, contraction.Replacement, start, length, ptr3, ref context);
			}
			for (int i = 0; i < contraction.SortKey.Length; i++)
			{
				ptr5[i] = contraction.SortKey[i];
			}
			return this.IndexOfSortKey(s, start, length, ptr5, '\0', -1, true, ref context);
		}

		private int IndexOfOrdinal(string s, char target, int start, int length)
		{
			int num = start + length;
			for (int i = start; i < num; i++)
			{
				if (s[i] == target)
				{
					return i;
				}
			}
			return -1;
		}

		private int IndexOfOrdinalIgnoreCase(string s, char target, int start, int length)
		{
			int num = start + length;
			target = this.textInfo.ToLower(target);
			for (int i = start; i < num; i++)
			{
				if (this.textInfo.ToLower(s[i]) == target)
				{
					return i;
				}
			}
			return -1;
		}

		private unsafe int IndexOfSortKey(string s, int start, int length, byte* sortkey, char target, int ti, bool noLv4, ref SimpleCollator.Context ctx)
		{
			int num = start + length;
			int i = start;
			while (i < num)
			{
				int result = i;
				if (this.MatchesForward(s, ref i, num, ti, sortkey, noLv4, ref ctx))
				{
					return result;
				}
			}
			return -1;
		}

		private unsafe int IndexOf(string s, string target, int start, int length, byte* targetSortKey, ref SimpleCollator.Context ctx)
		{
			CompareOptions option = ctx.Option;
			int i;
			for (i = 0; i < target.Length; i++)
			{
				if (!SimpleCollator.IsIgnorable((int)target[i], option))
				{
					break;
				}
			}
			if (i == target.Length)
			{
				return start;
			}
			Contraction contraction = this.GetContraction(target, i, target.Length - i);
			string text = (contraction == null) ? null : contraction.Replacement;
			byte* ptr = (text != null) ? null : targetSortKey;
			bool noLv = true;
			char target2 = '\0';
			int num = -1;
			if (contraction != null && ptr != null)
			{
				for (int j = 0; j < contraction.SortKey.Length; j++)
				{
					ptr[j] = contraction.SortKey[j];
				}
			}
			else if (ptr != null)
			{
				target2 = target[i];
				num = this.FilterOptions((int)target[i], option);
				*ptr = this.Category(num);
				ptr[1] = this.Level1(num);
				if ((option & CompareOptions.IgnoreNonSpace) == CompareOptions.None)
				{
					ptr[2] = this.Level2(num, SimpleCollator.ExtenderType.None);
				}
				ptr[3] = MSCompatUnicodeTable.Level3(num);
				noLv = !MSCompatUnicodeTable.HasSpecialWeight((char)num);
			}
			if (ptr != null)
			{
				for (i++; i < target.Length; i++)
				{
					if (this.Category((int)target[i]) != 1)
					{
						break;
					}
					if (ptr[2] == 0)
					{
						ptr[2] = 2;
					}
					ptr[2] = ptr[2] + this.Level2((int)target[i], SimpleCollator.ExtenderType.None);
				}
			}
			for (;;)
			{
				int num2;
				if (text != null)
				{
					num2 = this.IndexOf(s, text, start, length, targetSortKey, ref ctx);
				}
				else
				{
					num2 = this.IndexOfSortKey(s, start, length, ptr, target2, num, noLv, ref ctx);
				}
				if (num2 < 0)
				{
					break;
				}
				length -= num2 - start;
				start = num2;
				if (this.IsPrefix(s, target, start, length, false, ref ctx))
				{
					return num2;
				}
				Contraction contraction2 = this.GetContraction(s, start, length);
				if (contraction2 != null)
				{
					start += contraction2.Source.Length;
					length -= contraction2.Source.Length;
				}
				else
				{
					start++;
					length--;
				}
				if (length <= 0)
				{
					return -1;
				}
			}
			return -1;
		}

		public int LastIndexOf(string s, string target, CompareOptions opt)
		{
			return this.LastIndexOf(s, target, s.Length - 1, s.Length, opt);
		}

		public unsafe int LastIndexOf(string s, string target, int start, int length, CompareOptions opt)
		{
			if (opt == CompareOptions.Ordinal)
			{
				return this.LastIndexOfOrdinal(s, target, start, length);
			}
			if (opt == CompareOptions.OrdinalIgnoreCase)
			{
				return this.LastIndexOfOrdinalIgnoreCase(s, target, start, length);
			}
			checked
			{
				byte* ptr = stackalloc byte[16 * 1];
				byte* ptr2 = stackalloc byte[16 * 1];
				byte* ptr3 = stackalloc byte[4 * 1];
				byte* ptr4 = stackalloc byte[4 * 1];
				byte* ptr5 = stackalloc byte[4 * 1];
				this.ClearBuffer(ptr, 16);
				this.ClearBuffer(ptr2, 16);
				this.ClearBuffer(ptr3, 4);
				this.ClearBuffer(ptr4, 4);
				this.ClearBuffer(ptr5, 4);
				SimpleCollator.Context context = new SimpleCollator.Context(opt, ptr, ptr2, ptr4, ptr5, null, false);
				return this.LastIndexOf(s, target, start, length, ptr3, ref context);
			}
		}

		private int LastIndexOfOrdinal(string s, string target, int start, int length)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			if (s.Length < target.Length || target.Length > length)
			{
				return -1;
			}
			int num = start - length + target.Length - 1;
			char c = target[target.Length - 1];
			int i = start;
			while (i > num)
			{
				if (s[i] != c)
				{
					i--;
				}
				else
				{
					int num2 = i - target.Length + 1;
					i--;
					bool flag = false;
					for (int j = target.Length - 2; j >= 0; j--)
					{
						if (s[num2 + j] != target[j])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return num2;
					}
				}
			}
			return -1;
		}

		private int LastIndexOfOrdinalIgnoreCase(string s, string target, int start, int length)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			if (s.Length < length || target.Length > length)
			{
				return -1;
			}
			int num = start - length + target.Length - 1;
			char c = this.textInfo.ToLower(target[target.Length - 1]);
			int i = start;
			while (i > num)
			{
				if (this.textInfo.ToLower(s[i]) != c)
				{
					i--;
				}
				else
				{
					int num2 = i - target.Length + 1;
					i--;
					bool flag = false;
					for (int j = target.Length - 2; j >= 0; j--)
					{
						if (this.textInfo.ToLower(s[num2 + j]) != this.textInfo.ToLower(target[j]))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return num2;
					}
				}
			}
			return -1;
		}

		public int LastIndexOf(string s, char target, CompareOptions opt)
		{
			return this.LastIndexOf(s, target, s.Length - 1, s.Length, opt);
		}

		public unsafe int LastIndexOf(string s, char target, int start, int length, CompareOptions opt)
		{
			if (opt == CompareOptions.Ordinal)
			{
				return this.LastIndexOfOrdinal(s, target, start, length);
			}
			if (opt == CompareOptions.OrdinalIgnoreCase)
			{
				return this.LastIndexOfOrdinalIgnoreCase(s, target, start, length);
			}
			byte* ptr3;
			byte* ptr5;
			SimpleCollator.Context context;
			Contraction contraction;
			checked
			{
				byte* ptr = stackalloc byte[16 * 1];
				byte* ptr2 = stackalloc byte[16 * 1];
				ptr3 = stackalloc byte[4 * 1];
				byte* ptr4 = stackalloc byte[4 * 1];
				ptr5 = stackalloc byte[4 * 1];
				this.ClearBuffer(ptr, 16);
				this.ClearBuffer(ptr2, 16);
				this.ClearBuffer(ptr3, 4);
				this.ClearBuffer(ptr4, 4);
				this.ClearBuffer(ptr5, 4);
				context = new SimpleCollator.Context(opt, ptr, ptr2, ptr4, ptr5, null, false);
				contraction = this.GetContraction(target);
			}
			if (contraction == null)
			{
				int num = this.FilterOptions((int)target, opt);
				*ptr3 = this.Category(num);
				ptr3[1] = this.Level1(num);
				if ((opt & CompareOptions.IgnoreNonSpace) == CompareOptions.None)
				{
					ptr3[2] = this.Level2(num, SimpleCollator.ExtenderType.None);
				}
				ptr3[3] = MSCompatUnicodeTable.Level3(num);
				return this.LastIndexOfSortKey(s, start, start, length, ptr3, num, !MSCompatUnicodeTable.HasSpecialWeight((char)num), ref context);
			}
			if (contraction.Replacement != null)
			{
				return this.LastIndexOf(s, contraction.Replacement, start, length, ptr3, ref context);
			}
			for (int i = 0; i < contraction.SortKey.Length; i++)
			{
				ptr5[i] = contraction.SortKey[i];
			}
			return this.LastIndexOfSortKey(s, start, start, length, ptr5, -1, true, ref context);
		}

		private int LastIndexOfOrdinal(string s, char target, int start, int length)
		{
			if (s.Length == 0)
			{
				return -1;
			}
			int num = start - length;
			for (int i = start; i > num; i--)
			{
				if (s[i] == target)
				{
					return i;
				}
			}
			return -1;
		}

		private int LastIndexOfOrdinalIgnoreCase(string s, char target, int start, int length)
		{
			if (s.Length == 0)
			{
				return -1;
			}
			int num = start - length;
			char c = this.textInfo.ToUpper(target);
			for (int i = start; i > num; i--)
			{
				if (this.textInfo.ToUpper(s[i]) == c)
				{
					return i;
				}
			}
			return -1;
		}

		private unsafe int LastIndexOfSortKey(string s, int start, int orgStart, int length, byte* sortkey, int ti, bool noLv4, ref SimpleCollator.Context ctx)
		{
			int num = start - length;
			int i = start;
			while (i > num)
			{
				int result = i;
				if (this.MatchesBackward(s, ref i, num, orgStart, ti, sortkey, noLv4, ref ctx))
				{
					return result;
				}
			}
			return -1;
		}

		private unsafe int LastIndexOf(string s, string target, int start, int length, byte* targetSortKey, ref SimpleCollator.Context ctx)
		{
			CompareOptions option = ctx.Option;
			int num = start;
			int i;
			for (i = 0; i < target.Length; i++)
			{
				if (!SimpleCollator.IsIgnorable((int)target[i], option))
				{
					break;
				}
			}
			if (i == target.Length)
			{
				return start;
			}
			Contraction contraction = this.GetContraction(target, i, target.Length - i);
			string text = (contraction == null) ? null : contraction.Replacement;
			byte* ptr = (text != null) ? null : targetSortKey;
			bool noLv = true;
			int num2 = -1;
			if (contraction != null && ptr != null)
			{
				for (int j = 0; j < contraction.SortKey.Length; j++)
				{
					ptr[j] = contraction.SortKey[j];
				}
			}
			else if (ptr != null)
			{
				num2 = this.FilterOptions((int)target[i], option);
				*ptr = this.Category(num2);
				ptr[1] = this.Level1(num2);
				if ((option & CompareOptions.IgnoreNonSpace) == CompareOptions.None)
				{
					ptr[2] = this.Level2(num2, SimpleCollator.ExtenderType.None);
				}
				ptr[3] = MSCompatUnicodeTable.Level3(num2);
				noLv = !MSCompatUnicodeTable.HasSpecialWeight((char)num2);
			}
			if (ptr != null)
			{
				for (i++; i < target.Length; i++)
				{
					if (this.Category((int)target[i]) != 1)
					{
						break;
					}
					if (ptr[2] == 0)
					{
						ptr[2] = 2;
					}
					ptr[2] = ptr[2] + this.Level2((int)target[i], SimpleCollator.ExtenderType.None);
				}
			}
			int k;
			for (;;)
			{
				if (text != null)
				{
					k = this.LastIndexOf(s, text, start, length, targetSortKey, ref ctx);
				}
				else
				{
					k = this.LastIndexOfSortKey(s, start, num, length, ptr, num2, noLv, ref ctx);
				}
				if (k < 0)
				{
					break;
				}
				length -= start - k;
				start = k;
				if (this.IsPrefix(s, target, k, num - k + 1, false, ref ctx))
				{
					goto Block_15;
				}
				Contraction contraction2 = this.GetContraction(s, k, num - k + 1);
				if (contraction2 != null)
				{
					start -= contraction2.Source.Length;
					length -= contraction2.Source.Length;
				}
				else
				{
					start--;
					length--;
				}
				if (length <= 0)
				{
					return -1;
				}
			}
			return -1;
			Block_15:
			while (k < num)
			{
				if (!SimpleCollator.IsIgnorable((int)s[k], option))
				{
					break;
				}
				k++;
			}
			return k;
		}

		private unsafe bool MatchesForward(string s, ref int idx, int end, int ti, byte* sortkey, bool noLv4, ref SimpleCollator.Context ctx)
		{
			int num = (int)s[idx];
			if (ctx.AlwaysMatchFlags != null && num < 128 && ((int)ctx.AlwaysMatchFlags[num / 8] & 1 << num % 8) != 0)
			{
				return true;
			}
			if (ctx.NeverMatchFlags != null && num < 128 && ((int)ctx.NeverMatchFlags[num / 8] & 1 << num % 8) != 0)
			{
				idx++;
				return false;
			}
			SimpleCollator.ExtenderType extenderType = this.GetExtenderType((int)s[idx]);
			Contraction contraction = null;
			if (this.MatchesForwardCore(s, ref idx, end, ti, sortkey, noLv4, extenderType, ref contraction, ref ctx))
			{
				if (ctx.AlwaysMatchFlags != null && contraction == null && extenderType == SimpleCollator.ExtenderType.None && num < 128)
				{
					byte* ptr = ctx.AlwaysMatchFlags + num / 8;
					*ptr |= (byte)(1 << num % 8);
				}
				return true;
			}
			if (ctx.NeverMatchFlags != null && contraction == null && extenderType == SimpleCollator.ExtenderType.None && num < 128)
			{
				byte* ptr2 = ctx.NeverMatchFlags + num / 8;
				*ptr2 |= (byte)(1 << num % 8);
			}
			return false;
		}

		private unsafe bool MatchesForwardCore(string s, ref int idx, int end, int ti, byte* sortkey, bool noLv4, SimpleCollator.ExtenderType ext, ref Contraction ct, ref SimpleCollator.Context ctx)
		{
			CompareOptions option = ctx.Option;
			byte* ptr = ctx.Buffer1;
			bool flag = (option & CompareOptions.IgnoreNonSpace) != CompareOptions.None;
			int num = -1;
			if (ext == SimpleCollator.ExtenderType.None)
			{
				ct = this.GetContraction(s, idx, end);
			}
			else if (ctx.PrevCode < 0)
			{
				if (ctx.PrevSortKey == null)
				{
					idx++;
					return false;
				}
				ptr = ctx.PrevSortKey;
			}
			else
			{
				num = this.FilterExtender(ctx.PrevCode, ext, option);
			}
			if (ct != null)
			{
				idx += ct.Source.Length;
				if (!noLv4)
				{
					return false;
				}
				if (ct.SortKey == null)
				{
					int num2 = 0;
					return this.MatchesForward(ct.Replacement, ref num2, ct.Replacement.Length, ti, sortkey, noLv4, ref ctx);
				}
				for (int i = 0; i < 4; i++)
				{
					ptr[i] = sortkey[i];
				}
				ctx.PrevCode = -1;
				ctx.PrevSortKey = ptr;
			}
			else
			{
				if (num < 0)
				{
					num = this.FilterOptions((int)s[idx], option);
				}
				idx++;
				*ptr = this.Category(num);
				bool flag2 = false;
				if (*sortkey == *ptr)
				{
					ptr[1] = this.Level1(num);
				}
				else
				{
					flag2 = true;
				}
				if (!flag && sortkey[1] == ptr[1])
				{
					ptr[2] = this.Level2(num, ext);
				}
				else if (!flag)
				{
					flag2 = true;
				}
				if (flag2)
				{
					while (idx < end)
					{
						if (this.Category((int)s[idx]) != 1)
						{
							break;
						}
						idx++;
					}
					return false;
				}
				ptr[3] = MSCompatUnicodeTable.Level3(num);
				if (*ptr != 1)
				{
					ctx.PrevCode = num;
				}
			}
			while (idx < end)
			{
				if (this.Category((int)s[idx]) != 1)
				{
					break;
				}
				if (!flag)
				{
					if (ptr[2] == 0)
					{
						ptr[2] = 2;
					}
					ptr[2] = ptr[2] + this.Level2((int)s[idx], SimpleCollator.ExtenderType.None);
				}
				idx++;
			}
			return this.MatchesPrimitive(option, ptr, num, ext, sortkey, ti, noLv4);
		}

		private unsafe bool MatchesPrimitive(CompareOptions opt, byte* source, int si, SimpleCollator.ExtenderType ext, byte* target, int ti, bool noLv4)
		{
			bool flag = (opt & CompareOptions.IgnoreNonSpace) != CompareOptions.None;
			return *source == *target && source[1] == target[1] && (flag || source[2] == target[2]) && source[3] == target[3] && ((noLv4 && (si < 0 || !MSCompatUnicodeTable.HasSpecialWeight((char)si))) || (!noLv4 && (flag || ext != SimpleCollator.ExtenderType.Conditional) && MSCompatUnicodeTable.IsJapaneseSmallLetter((char)si) == MSCompatUnicodeTable.IsJapaneseSmallLetter((char)ti) && SimpleCollator.ToDashTypeValue(ext, opt) == SimpleCollator.ToDashTypeValue(SimpleCollator.ExtenderType.None, opt) && !MSCompatUnicodeTable.IsHiragana((char)si) == !MSCompatUnicodeTable.IsHiragana((char)ti) && SimpleCollator.IsHalfKana((int)((ushort)si), opt) == SimpleCollator.IsHalfKana((int)((ushort)ti), opt)));
		}

		private unsafe bool MatchesBackward(string s, ref int idx, int end, int orgStart, int ti, byte* sortkey, bool noLv4, ref SimpleCollator.Context ctx)
		{
			int num = (int)s[idx];
			if (ctx.AlwaysMatchFlags != null && num < 128 && ((int)ctx.AlwaysMatchFlags[num / 8] & 1 << num % 8) != 0)
			{
				return true;
			}
			if (ctx.NeverMatchFlags != null && num < 128 && ((int)ctx.NeverMatchFlags[num / 8] & 1 << num % 8) != 0)
			{
				idx--;
				return false;
			}
			SimpleCollator.ExtenderType extenderType = this.GetExtenderType((int)s[idx]);
			Contraction contraction = null;
			if (this.MatchesBackwardCore(s, ref idx, end, orgStart, ti, sortkey, noLv4, extenderType, ref contraction, ref ctx))
			{
				if (ctx.AlwaysMatchFlags != null && contraction == null && extenderType == SimpleCollator.ExtenderType.None && num < 128)
				{
					byte* ptr = ctx.AlwaysMatchFlags + num / 8;
					*ptr |= (byte)(1 << num % 8);
				}
				return true;
			}
			if (ctx.NeverMatchFlags != null && contraction == null && extenderType == SimpleCollator.ExtenderType.None && num < 128)
			{
				byte* ptr2 = ctx.NeverMatchFlags + num / 8;
				*ptr2 |= (byte)(1 << num % 8);
			}
			return false;
		}

		private unsafe bool MatchesBackwardCore(string s, ref int idx, int end, int orgStart, int ti, byte* sortkey, bool noLv4, SimpleCollator.ExtenderType ext, ref Contraction ct, ref SimpleCollator.Context ctx)
		{
			CompareOptions option = ctx.Option;
			byte* buffer = ctx.Buffer1;
			bool flag = (option & CompareOptions.IgnoreNonSpace) != CompareOptions.None;
			int num = idx;
			int num2 = -1;
			if (ext != SimpleCollator.ExtenderType.None)
			{
				byte b = 0;
				for (int i = 0; i >= 0; i--)
				{
					if (!SimpleCollator.IsIgnorable((int)s[i], option))
					{
						int num3 = this.FilterOptions((int)s[i], option);
						byte b2 = this.Category(num3);
						if (b2 != 1)
						{
							num2 = this.FilterExtender(num3, ext, option);
							*buffer = b2;
							buffer[1] = this.Level1(num2);
							if (!flag)
							{
								buffer[2] = this.Level2(num2, ext);
							}
							buffer[3] = MSCompatUnicodeTable.Level3(num2);
							if (ext != SimpleCollator.ExtenderType.Conditional && b != 0)
							{
								buffer[2] = ((buffer[2] != 0) ? b : (b + 2));
							}
							idx--;
							goto IL_101;
						}
						b = this.Level2(num3, SimpleCollator.ExtenderType.None);
					}
				}
				return false;
			}
			IL_101:
			if (ext == SimpleCollator.ExtenderType.None)
			{
				ct = this.GetTailContraction(s, idx, end);
			}
			if (ct != null)
			{
				idx -= ct.Source.Length;
				if (!noLv4)
				{
					return false;
				}
				if (ct.SortKey == null)
				{
					int num4 = ct.Replacement.Length - 1;
					return 0 <= this.LastIndexOfSortKey(ct.Replacement, num4, num4, ct.Replacement.Length, sortkey, ti, noLv4, ref ctx);
				}
				for (int j = 0; j < 4; j++)
				{
					buffer[j] = sortkey[j];
				}
				ctx.PrevCode = -1;
				ctx.PrevSortKey = buffer;
			}
			else if (ext == SimpleCollator.ExtenderType.None)
			{
				if (num2 < 0)
				{
					num2 = this.FilterOptions((int)s[idx], option);
				}
				idx--;
				bool flag2 = false;
				*buffer = this.Category(num2);
				if (*buffer == *sortkey)
				{
					buffer[1] = this.Level1(num2);
				}
				else
				{
					flag2 = true;
				}
				if (!flag && buffer[1] == sortkey[1])
				{
					buffer[2] = this.Level2(num2, ext);
				}
				else if (!flag)
				{
					flag2 = true;
				}
				if (flag2)
				{
					return false;
				}
				buffer[3] = MSCompatUnicodeTable.Level3(num2);
				if (*buffer != 1)
				{
					ctx.PrevCode = num2;
				}
			}
			if (ext == SimpleCollator.ExtenderType.None)
			{
				for (int k = num + 1; k < orgStart; k++)
				{
					if (this.Category((int)s[k]) != 1)
					{
						break;
					}
					if (!flag)
					{
						if (buffer[2] == 0)
						{
							buffer[2] = 2;
						}
						buffer[2] = buffer[2] + this.Level2((int)s[k], SimpleCollator.ExtenderType.None);
					}
				}
			}
			return this.MatchesPrimitive(option, buffer, num2, ext, sortkey, ti, noLv4);
		}

		internal struct Context
		{
			public readonly CompareOptions Option;

			public unsafe readonly byte* NeverMatchFlags;

			public unsafe readonly byte* AlwaysMatchFlags;

			public unsafe byte* Buffer1;

			public unsafe byte* Buffer2;

			public int PrevCode;

			public unsafe byte* PrevSortKey;

			public readonly bool QuickCheckPossible;

			public unsafe Context(CompareOptions opt, byte* alwaysMatchFlags, byte* neverMatchFlags, byte* buffer1, byte* buffer2, byte* prev1, bool quickCheckPossible)
			{
				this.Option = opt;
				this.AlwaysMatchFlags = alwaysMatchFlags;
				this.NeverMatchFlags = neverMatchFlags;
				this.Buffer1 = buffer1;
				this.Buffer2 = buffer2;
				this.PrevSortKey = prev1;
				this.PrevCode = -1;
				this.QuickCheckPossible = quickCheckPossible;
			}

			public void ClearPrevInfo()
			{
				this.PrevCode = -1;
				this.PrevSortKey = null;
			}
		}

		private struct PreviousInfo
		{
			public int Code;

			public unsafe byte* SortKey;

			public PreviousInfo(bool dummy)
			{
				this.Code = -1;
				this.SortKey = null;
			}
		}

		private struct Escape
		{
			public string Source;

			public int Index;

			public int Start;

			public int End;

			public int Optional;
		}

		private enum ExtenderType
		{
			None,
			Simple,
			Voiced,
			Conditional,
			Buggy
		}
	}
}
