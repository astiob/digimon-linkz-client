using System;

namespace UI.Common
{
	public static class MonsterMedalIcon
	{
		private static string GetMedalSpriteNamePrefix(string medalType)
		{
			string result = string.Empty;
			if (!string.IsNullOrEmpty(medalType))
			{
				int num = int.Parse(medalType);
				if (num != 1)
				{
					if (num == 2)
					{
						result = "Common02_Talent_Silver";
					}
				}
				else
				{
					result = "Common02_Talent_Gold";
				}
			}
			return result;
		}

		private static string GetMedalSpriteNameSuffix(string upPercentage)
		{
			string result = string.Empty;
			switch (upPercentage)
			{
			case "5":
				result = "1";
				break;
			case "6":
				result = "2";
				break;
			case "7":
				result = "3";
				break;
			case "8":
				result = "4";
				break;
			case "9":
				result = "5";
				break;
			case "10":
				result = "6";
				break;
			case "15":
				result = "1";
				break;
			case "16":
				result = "2";
				break;
			case "17":
				result = "3";
				break;
			case "18":
				result = "4";
				break;
			case "19":
				result = "5";
				break;
			case "20":
				result = "6";
				break;
			}
			return result;
		}

		public static string GetMedalSpriteName(string medalType, string upPercentage)
		{
			return MonsterMedalIcon.GetMedalSpriteNamePrefix(medalType) + MonsterMedalIcon.GetMedalSpriteNameSuffix(upPercentage);
		}

		public static string GetMedalType(string upPercentage)
		{
			string result = "0";
			if (!string.IsNullOrEmpty(upPercentage))
			{
				int num = int.Parse(upPercentage);
				if (15 <= num && num <= 20)
				{
					result = "1";
				}
				else if (5 <= num && num <= 10)
				{
					result = "2";
				}
			}
			return result;
		}
	}
}
