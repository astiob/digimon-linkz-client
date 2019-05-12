using System;
using System.Text.RegularExpressions;

namespace UnityEngine.Timeline
{
	internal static class TimeUtility
	{
		public static readonly double kTimeEpsilon = 1E-14;

		public static readonly double kFrameRateEpsilon = 1E-06;

		public static readonly double k_MaxTimelineDurationInSeconds = 9000000.0;

		private static void ValidateFrameRate(double frameRate)
		{
			if (frameRate <= TimeUtility.kTimeEpsilon)
			{
				throw new ArgumentException("frame rate cannot be 0 or negative");
			}
		}

		public static int ToFrames(double time, double frameRate)
		{
			TimeUtility.ValidateFrameRate(frameRate);
			time = Math.Min(Math.Max(time, -TimeUtility.k_MaxTimelineDurationInSeconds), TimeUtility.k_MaxTimelineDurationInSeconds);
			double num = TimeUtility.kTimeEpsilon * time;
			int result;
			if (time < 0.0)
			{
				result = (int)Math.Ceiling(time * frameRate + num);
			}
			else
			{
				result = (int)Math.Floor(time * frameRate + num);
			}
			return result;
		}

		public static double ToExactFrames(double time, double frameRate)
		{
			TimeUtility.ValidateFrameRate(frameRate);
			return time * frameRate;
		}

		public static double FromFrames(int frames, double frameRate)
		{
			TimeUtility.ValidateFrameRate(frameRate);
			return (double)frames / frameRate;
		}

		public static double FromFrames(double frames, double frameRate)
		{
			TimeUtility.ValidateFrameRate(frameRate);
			return frames / frameRate;
		}

		public static bool OnFrameBoundary(double time, double frameRate)
		{
			return TimeUtility.OnFrameBoundary(time, frameRate, Math.Max(time, 1.0) * frameRate * TimeUtility.kTimeEpsilon);
		}

		public static bool OnFrameBoundary(double time, double frameRate, double epsilon)
		{
			TimeUtility.ValidateFrameRate(frameRate);
			double num = TimeUtility.ToExactFrames(time, frameRate);
			double num2 = Math.Round(num);
			return Math.Abs(num - num2) < epsilon;
		}

		public static double RoundToFrame(double time, double frameRate)
		{
			TimeUtility.ValidateFrameRate(frameRate);
			double num = Math.Max((double)((int)Math.Floor(time * frameRate)) / frameRate, 0.0);
			double num2 = Math.Max((double)((int)Math.Ceiling(time * frameRate)) / frameRate, 0.0);
			return (Math.Abs(time - num) >= Math.Abs(time - num2)) ? num2 : num;
		}

		public static string TimeAsFrames(double timeValue, double frameRate, string format = "F2")
		{
			string result;
			if (TimeUtility.OnFrameBoundary(timeValue, frameRate))
			{
				result = TimeUtility.ToFrames(timeValue, frameRate).ToString();
			}
			else
			{
				result = TimeUtility.ToExactFrames(timeValue, frameRate).ToString(format);
			}
			return result;
		}

		public static string TimeAsTimeCode(double timeValue, double frameRate, string format = "F2")
		{
			TimeUtility.ValidateFrameRate(frameRate);
			int num = (int)Math.Abs(timeValue);
			int num2 = num / 3600;
			int num3 = num % 3600 / 60;
			int num4 = num % 60;
			string str = (timeValue >= 0.0) ? string.Empty : "-";
			string str2;
			if (num2 > 0)
			{
				str2 = string.Concat(new object[]
				{
					num2,
					":",
					num3.ToString("D2"),
					":",
					num4.ToString("D2")
				});
			}
			else if (num3 > 0)
			{
				str2 = num3 + ":" + num4.ToString("D2");
			}
			else
			{
				str2 = num4.ToString();
			}
			int totalWidth = (int)Math.Floor(Math.Log10(frameRate) + 1.0);
			string text = (TimeUtility.ToFrames(timeValue, frameRate) - TimeUtility.ToFrames((double)num, frameRate)).ToString().PadLeft(totalWidth, '0');
			if (!TimeUtility.OnFrameBoundary(timeValue, frameRate))
			{
				string text2 = TimeUtility.ToExactFrames(timeValue, frameRate).ToString(format);
				int num5 = text2.IndexOf('.');
				if (num5 >= 0)
				{
					text = text + " [" + text2.Substring(num5) + "]";
				}
			}
			return str + str2 + ":" + text;
		}

		public static double ParseTimeCode(string timeCode, double frameRate, double defaultValue)
		{
			timeCode = TimeUtility.RemoveChar(timeCode, (char c) => char.IsWhiteSpace(c));
			string[] array = timeCode.Split(new char[]
			{
				':'
			});
			double result;
			if (array.Length == 0 || array.Length > 4)
			{
				result = defaultValue;
			}
			else
			{
				int num = 0;
				int num2 = 0;
				double num3 = 0.0;
				double num4 = 0.0;
				try
				{
					string text = array[array.Length - 1];
					if (Regex.Match(text, "^\\d+\\.\\d+$").Success)
					{
						num3 = double.Parse(text);
						if (array.Length > 3)
						{
							return defaultValue;
						}
						if (array.Length > 1)
						{
							num2 = int.Parse(array[array.Length - 2]);
						}
						if (array.Length > 2)
						{
							num = int.Parse(array[array.Length - 3]);
						}
					}
					else
					{
						if (Regex.Match(text, "^\\d+\\[\\.\\d+\\]$").Success)
						{
							string s = TimeUtility.RemoveChar(text, (char c) => c == '[' || c == ']');
							num4 = double.Parse(s);
						}
						else
						{
							if (!Regex.Match(text, "^\\d*$").Success)
							{
								return defaultValue;
							}
							num4 = (double)int.Parse(text);
						}
						if (array.Length > 1)
						{
							num3 = (double)int.Parse(array[array.Length - 2]);
						}
						if (array.Length > 2)
						{
							num2 = int.Parse(array[array.Length - 3]);
						}
						if (array.Length > 3)
						{
							num = int.Parse(array[array.Length - 4]);
						}
					}
				}
				catch (FormatException)
				{
					return defaultValue;
				}
				result = num4 / frameRate + num3 + (double)(num2 * 60) + (double)(num * 3600);
			}
			return result;
		}

		private static string RemoveChar(string str, Func<char, bool> charToRemoveFunc)
		{
			int length = str.Length;
			char[] array = str.ToCharArray();
			int length2 = 0;
			for (int i = 0; i < length; i++)
			{
				if (!charToRemoveFunc(array[i]))
				{
					array[length2++] = array[i];
				}
			}
			return new string(array, 0, length2);
		}
	}
}
