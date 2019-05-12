using System;

public static class StringExtensions
{
	public static bool IsChar2Byte(char c)
	{
		return (c < '\0' || c >= '\u0081') && c != '' && (c < '｡' || c >= 'ﾠ') && (c < '' || c >= '');
	}

	public static int GetByteCount(this string self)
	{
		int num = 0;
		for (int i = 0; i < self.Length; i++)
		{
			if (StringExtensions.IsChar2Byte(self[i]))
			{
				num++;
			}
			num++;
		}
		return num;
	}

	public static string SubstringInByte(this string self, int byteCount)
	{
		string text = string.Empty;
		int num = 0;
		foreach (char c in self)
		{
			if (StringExtensions.IsChar2Byte(c))
			{
				num++;
			}
			num++;
			if (num > byteCount)
			{
				break;
			}
			text += c;
		}
		return text;
	}

	public static string SafeSubstringInByte(this string self, int byteCount)
	{
		return (byteCount >= self.GetByteCount()) ? self : self.SubstringInByte(byteCount);
	}
}
