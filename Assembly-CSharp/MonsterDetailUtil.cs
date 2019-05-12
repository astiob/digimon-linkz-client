using Monster;
using System;
using UnityEngine;

public static class MonsterDetailUtil
{
	public static string GetMedalSpriteName(string medalType)
	{
		ConstValue.Medal medal = (ConstValue.Medal)medalType.ToInt32();
		string result = string.Empty;
		ConstValue.Medal medal2 = medal;
		if (medal2 != ConstValue.Medal.Gold)
		{
			if (medal2 != ConstValue.Medal.Silver)
			{
				result = string.Empty;
			}
			else
			{
				result = "Common02_Talent_Silver";
			}
		}
		else
		{
			result = "Common02_Talent_Gold";
		}
		return result;
	}

	public static string GetResistanceSpriteName(ConstValue.ResistanceType resistanceType)
	{
		int num = (int)resistanceType;
		string str = num.ToString();
		return "Battle_Resistance_" + str;
	}

	public static Color GetResistanceSpriteColor(string resistance)
	{
		Color gray;
		switch (resistance)
		{
		case "-1":
			gray = new Color(0f, 0.5882f, 1f);
			return gray;
		case "1":
			gray = new Color(0.7843f, 0f, 0f);
			return gray;
		case "2":
			return Color.green;
		}
		gray = Color.gray;
		return gray;
	}

	public static string GetArousalSpriteName(int rarity)
	{
		string result = string.Empty;
		if (2 <= rarity)
		{
			if (6 < rarity)
			{
				rarity = 6;
			}
			result = MonsterArousalData.GetSpriteName(rarity.ToString());
		}
		return result;
	}
}
