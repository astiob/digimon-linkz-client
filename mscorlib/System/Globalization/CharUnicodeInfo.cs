using System;

namespace System.Globalization
{
	/// <summary>Retrieves information about a Unicode character. This class cannot be inherited.</summary>
	public sealed class CharUnicodeInfo
	{
		private CharUnicodeInfo()
		{
		}

		/// <summary>Gets the decimal digit value of the specified numeric character.</summary>
		/// <returns>The decimal digit value of the specified numeric character.-or- -1, if the specified character is not a decimal digit.</returns>
		/// <param name="ch">The Unicode character for which to get the decimal digit value. </param>
		public static int GetDecimalDigitValue(char ch)
		{
			if (ch == '²')
			{
				return 2;
			}
			if (ch == '³')
			{
				return 3;
			}
			if (ch == '¹')
			{
				return 1;
			}
			if (ch == '⁰')
			{
				return 0;
			}
			if ('⁴' <= ch && ch < '⁺')
			{
				return (int)(ch - '⁰');
			}
			if ('₀' <= ch && ch < '₊')
			{
				return (int)(ch - '₀');
			}
			if (!char.IsDigit(ch))
			{
				return -1;
			}
			if (ch < ':')
			{
				return (int)(ch - '0');
			}
			if (ch < '٪')
			{
				return (int)(ch - '٠');
			}
			if (ch < 'ۺ')
			{
				return (int)(ch - '۰');
			}
			if (ch < '॰')
			{
				return (int)(ch - '०');
			}
			if (ch < 'ৰ')
			{
				return (int)(ch - '০');
			}
			if (ch < 'ੰ')
			{
				return (int)(ch - '੦');
			}
			if (ch < '૰')
			{
				return (int)(ch - '૦');
			}
			if (ch < '୰')
			{
				return (int)(ch - '୦');
			}
			if (ch < '௰')
			{
				return (int)(ch - '௦');
			}
			if (ch < '౰')
			{
				return (int)(ch - '౦');
			}
			if (ch < '೰')
			{
				return (int)(ch - '೦');
			}
			if (ch < '൰')
			{
				return (int)(ch - '൦');
			}
			if (ch < '๚')
			{
				return (int)(ch - '๐');
			}
			if (ch < '໚')
			{
				return (int)(ch - '໐');
			}
			if (ch < '༪')
			{
				return (int)(ch - '༠');
			}
			if (ch < '၊')
			{
				return (int)(ch - '၀');
			}
			if (ch < '፲')
			{
				return (int)(ch - '፨');
			}
			if (ch < '៪')
			{
				return (int)(ch - '០');
			}
			if (ch < '᠚')
			{
				return (int)(ch - '᠐');
			}
			if (ch < '⁺')
			{
				return (int)(ch - '⁰');
			}
			if (ch < '₊')
			{
				return (int)(ch - '₀');
			}
			if (ch < '０')
			{
				return -1;
			}
			if (ch < '：')
			{
				return (int)(ch - '０');
			}
			return -1;
		}

		/// <summary>Gets the decimal digit value of the numeric character at the specified index of the specified string.</summary>
		/// <returns>The decimal digit value of the numeric character at the specified index of the specified string.-or- -1, if the character at the specified index of the specified string is not a decimal digit.</returns>
		/// <param name="s">The <see cref="T:System.String" /> containing the Unicode character for which to get the decimal digit value. </param>
		/// <param name="index">The index of the Unicode character for which to get the decimal digit value. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes in <paramref name="s" />. </exception>
		public static int GetDecimalDigitValue(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return CharUnicodeInfo.GetDecimalDigitValue(s[index]);
		}

		/// <summary>Gets the digit value of the specified numeric character.</summary>
		/// <returns>The digit value of the specified numeric character.-or- -1, if the specified character is not a digit.</returns>
		/// <param name="ch">The Unicode character for which to get the digit value. </param>
		public static int GetDigitValue(char ch)
		{
			int decimalDigitValue = CharUnicodeInfo.GetDecimalDigitValue(ch);
			if (decimalDigitValue >= 0)
			{
				return decimalDigitValue;
			}
			if (ch == '⓪')
			{
				return 0;
			}
			if (ch >= '①' && ch < '⑩')
			{
				return (int)(ch - '⑟');
			}
			if (ch >= '⑴' && ch < '⑽')
			{
				return (int)(ch - '⑳');
			}
			if (ch >= '⒈' && ch < '⒑')
			{
				return (int)(ch - '⒇');
			}
			if (ch >= '⓵' && ch < '⓾')
			{
				return (int)(ch - '⓴');
			}
			if (ch >= '❶' && ch < '❿')
			{
				return (int)(ch - '❵');
			}
			if (ch >= '➀' && ch < '➉')
			{
				return (int)(ch - '❿');
			}
			if (ch >= '➊' && ch < '➓')
			{
				return (int)(ch - '➉');
			}
			return -1;
		}

		/// <summary>Gets the digit value of the numeric character at the specified index of the specified string.</summary>
		/// <returns>The digit value of the numeric character at the specified index of the specified string.-or- -1, if the character at the specified index of the specified string is not a digit.</returns>
		/// <param name="s">The <see cref="T:System.String" /> containing the Unicode character for which to get the digit value. </param>
		/// <param name="index">The index of the Unicode character for which to get the digit value. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes in <paramref name="s" />. </exception>
		public static int GetDigitValue(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return CharUnicodeInfo.GetDigitValue(s[index]);
		}

		/// <summary>Gets the numeric value associated with the specified character.</summary>
		/// <returns>The numeric value associated with the specified character.-or- -1, if the specified character is not a numeric character.</returns>
		/// <param name="ch">The Unicode character for which to get the numeric value. </param>
		public static double GetNumericValue(char ch)
		{
			int digitValue = CharUnicodeInfo.GetDigitValue(ch);
			if (digitValue >= 0)
			{
				return (double)digitValue;
			}
			switch (ch)
			{
			case '⅓':
				return 0.33333333333333331;
			case '⅔':
				return 0.66666666666666663;
			default:
				switch (ch)
				{
				case 'ⅼ':
					return 50.0;
				case 'ⅽ':
					return 100.0;
				case 'ⅾ':
					return 500.0;
				case 'ⅿ':
					return 1000.0;
				case 'ↀ':
					return 1000.0;
				case 'ↁ':
					return 5000.0;
				case 'ↂ':
					return 10000.0;
				default:
					switch (ch)
					{
					case '৴':
						return 1.0;
					case '৵':
						return 2.0;
					case '৶':
						return 3.0;
					case '৷':
						return 4.0;
					default:
						switch (ch)
						{
						case 'Ⅼ':
							return 50.0;
						case 'Ⅽ':
							return 100.0;
						case 'Ⅾ':
							return 500.0;
						case 'Ⅿ':
							return 1000.0;
						default:
							switch (ch)
							{
							case '¼':
								return 0.25;
							case '½':
								return 0.5;
							case '¾':
								return 0.75;
							default:
								switch (ch)
								{
								case '௰':
									return 10.0;
								case '௱':
									return 100.0;
								case '௲':
									return 1000.0;
								default:
									switch (ch)
									{
									case 'ᛮ':
										return 17.0;
									case 'ᛯ':
										return 18.0;
									case 'ᛰ':
										return 19.0;
									default:
										switch (ch)
										{
										case '〸':
											return 10.0;
										case '〹':
											return 20.0;
										case '〺':
											return 30.0;
										default:
											if (ch == '፼')
											{
												return 10000.0;
											}
											if (ch == '⓾' || ch == '❿' || ch == '➉' || ch == '➓')
											{
												return 10.0;
											}
											if (ch == '〇')
											{
												return 0.0;
											}
											if ('⓫' <= ch && ch < '⓵')
											{
												return (double)(ch - 'ⓠ');
											}
											if ('〡' <= ch && ch < '〪')
											{
												return (double)(ch - '〠');
											}
											if ('㉑' <= ch && ch < '㉠')
											{
												return (double)(ch - '㈼');
											}
											if ('㊱' <= ch && ch < '㋀')
											{
												return (double)(ch - '㊍');
											}
											if (!char.IsNumber(ch))
											{
												return -1.0;
											}
											if (ch < '༳')
											{
												return 0.5 + (double)ch - 3882.0;
											}
											if (ch < '፼')
											{
												return (double)((ch - '፱') * '\n');
											}
											if (ch < '⅙')
											{
												return 0.2 * (double)(ch - '⅔');
											}
											if (ch < 'Ⅼ')
											{
												return (double)(ch - '⅟');
											}
											if (ch < 'ⅼ')
											{
												return (double)(ch - 'Ⅿ');
											}
											if (ch < '⑴')
											{
												return (double)(ch - '⑟');
											}
											if (ch < '⒈')
											{
												return (double)(ch - '⑳');
											}
											if (ch < '⒜')
											{
												return (double)(ch - '⒇');
											}
											if (ch < '㆖')
											{
												return (double)(ch - '㆑');
											}
											if (ch < '㈪')
											{
												return (double)(ch - '㈟');
											}
											if (ch < '㊊')
											{
												return (double)(ch - '㉿');
											}
											return -1.0;
										}
										break;
									}
									break;
								}
								break;
							}
							break;
						}
						break;
					case '৹':
						return 16.0;
					}
					break;
				}
				break;
			case '⅙':
				return 0.16666666666666666;
			case '⅚':
				return 0.83333333333333337;
			case '⅛':
				return 0.125;
			case '⅜':
				return 0.375;
			case '⅝':
				return 0.625;
			case '⅞':
				return 0.875;
			case '⅟':
				return 1.0;
			}
		}

		/// <summary>Gets the numeric value associated with the character at the specified index of the specified string.</summary>
		/// <returns>The numeric value associated with the character at the specified index of the specified string.-or- -1, if the character at the specified index of the specified string is not a numeric character.</returns>
		/// <param name="s">The <see cref="T:System.String" /> containing the Unicode character for which to get the numeric value. </param>
		/// <param name="index">The index of the Unicode character for which to get the numeric value. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes in <paramref name="s" />. </exception>
		public static double GetNumericValue(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return CharUnicodeInfo.GetNumericValue(s[index]);
		}

		/// <summary>Gets the Unicode category of the specified character.</summary>
		/// <returns>A <see cref="T:System.Globalization.UnicodeCategory" /> value indicating the category of the specified character.</returns>
		/// <param name="ch">The Unicode character for which to get the Unicode category. </param>
		public static UnicodeCategory GetUnicodeCategory(char ch)
		{
			return char.GetUnicodeCategory(ch);
		}

		/// <summary>Gets the Unicode category of the character at the specified index of the specified string.</summary>
		/// <returns>A <see cref="T:System.Globalization.UnicodeCategory" /> value indicating the category of the character at the specified index of the specified string.</returns>
		/// <param name="s">The <see cref="T:System.String" /> containing the Unicode character for which to get the Unicode category. </param>
		/// <param name="index">The index of the Unicode character for which to get the Unicode category. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is outside the range of valid indexes in <paramref name="s" />. </exception>
		public static UnicodeCategory GetUnicodeCategory(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return char.GetUnicodeCategory(s, index);
		}
	}
}
