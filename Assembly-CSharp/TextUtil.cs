using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TextUtil
{
	public static string GetWinTextSkipColorCode(string str, int iCMax)
	{
		if (str == null)
		{
			return string.Empty;
		}
		string text = str;
		int i = 0;
		int num = iCMax;
		while (i < text.Length)
		{
			i = TextUtil.SkipColorCode(text, i);
			if (i == -1)
			{
				return string.Empty;
			}
			if (i < text.Length)
			{
				int num2 = i;
				i = TextUtil.SkipCRLFCode(text, i);
				if (i != num2)
				{
					num = iCMax;
				}
				if (i < text.Length)
				{
					i++;
					num--;
					if (i < text.Length)
					{
						i = TextUtil.SkipColorCode(text, i);
						if (i == -1)
						{
							return string.Empty;
						}
						if (i < text.Length)
						{
							num2 = i;
							i = TextUtil.SkipCRLFCode(text, i);
							if (i != num2)
							{
								num = iCMax;
							}
							if (i < text.Length)
							{
								if (num <= 0 || (num == 1 && i + 1 < text.Length && TextUtil.IsBadTopChar(text, i + 1)))
								{
									text = text.Insert(i, "\n");
									i++;
									num = iCMax;
								}
								if (i < text.Length)
								{
									continue;
								}
							}
						}
					}
				}
			}
			return text;
		}
		return text;
	}

	private static bool IsBadTopChar(string tmp, int idx)
	{
		return tmp[idx] == '.' || tmp[idx] == '・' || tmp[idx] == '。' || tmp[idx] == '、';
	}

	private static int SkipColorCode(string tmp, int cidx)
	{
		if (tmp[cidx] != '<')
		{
			return cidx;
		}
		while (tmp[cidx] != '>')
		{
			cidx++;
			if (cidx >= tmp.Length)
			{
				return -1;
			}
		}
		return cidx + 1;
	}

	private static int SkipCRLFCode(string tmp, int cidx)
	{
		if (tmp[cidx] == '\r' && tmp[cidx + 1] == '\n')
		{
			return cidx + 2;
		}
		if (tmp[cidx] == '\n')
		{
			return cidx + 1;
		}
		return cidx;
	}

	public static string GetFormatDate(DateTime dt)
	{
		int year = dt.Year;
		int month = dt.Month;
		int day = dt.Day;
		int hour = dt.Hour;
		int minute = dt.Minute;
		string str = string.Empty;
		str = str + day.ToString() + " ";
		str = str + TextUtil.GetMonthStringFromNum(month) + " ";
		str = str + year.ToString() + " at ";
		return str + hour.ToString() + ":" + minute.ToString("D2");
	}

	private static string GetMonthStringFromNum(int m)
	{
		string result = string.Empty;
		switch (m)
		{
		case 1:
			result = "January";
			break;
		case 2:
			result = "February";
			break;
		case 3:
			result = "March";
			break;
		case 4:
			result = "April";
			break;
		case 5:
			result = "May";
			break;
		case 6:
			result = "June";
			break;
		case 7:
			result = "July";
			break;
		case 8:
			result = "August";
			break;
		case 9:
			result = "September";
			break;
		case 10:
			result = "October";
			break;
		case 11:
			result = "November";
			break;
		case 12:
			result = "December";
			break;
		default:
			result = string.Empty;
			break;
		}
		return result;
	}

	public static bool SurrogateCheck(string s)
	{
		foreach (char c in s)
		{
			if (char.IsSurrogate(c))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsHalfChar(char c)
	{
		return TextUtil.IsHalfChar(char.ToString(c));
	}

	public static bool IsHalfChar(string s)
	{
		string pattern = "^[^ -~｡-ﾟ]+$";
		return Regex.IsMatch(s, pattern);
	}

	public static int TextLengthFullAndHalf(string str)
	{
		int num = 0;
		int num2 = 0;
		foreach (char c in str)
		{
			if (TextUtil.IsHalfChar(c))
			{
				num++;
			}
			else
			{
				num2++;
			}
		}
		return num * 2 + num2;
	}

	public static bool IsOverLengthFullAndHalf(string str, int limit)
	{
		return TextUtil.TextLengthFullAndHalf(str) > limit;
	}

	public static void ShowNumber(List<UISprite> spL, string sprName, int num, bool dontSHowZero = false)
	{
		string text = num.ToString();
		int num2 = text.Length - 1;
		for (int i = 0; i < spL.Count; i++)
		{
			if (dontSHowZero && num == 0)
			{
				spL[i].gameObject.SetActive(false);
			}
			else
			{
				if (num2 >= 0)
				{
					spL[i].gameObject.SetActive(true);
					string str = text[num2].ToString();
					spL[i].spriteName = sprName + str;
				}
				else
				{
					spL[i].gameObject.SetActive(false);
				}
				num2--;
			}
		}
	}
}
