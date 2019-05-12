using Monster;
using System;
using UnityEngine;

public static class MonsterDetailUtil
{
	public static string GetResistanceSpriteName(ConstValue.ResistanceType resistanceType)
	{
		int num = (int)resistanceType;
		string str = num.ToString();
		return "Battle_Resistance_" + str;
	}

	public static Color GetResistanceSpriteColor(string resistance)
	{
		Color gray;
		if (resistance != null)
		{
			if (resistance == "-1")
			{
				gray = new Color(0f, 0.5882f, 1f);
				return gray;
			}
			if (resistance == "1")
			{
				gray = new Color(0.7843f, 0f, 0f);
				return gray;
			}
			if (resistance == "2")
			{
				return Color.green;
			}
			if (!(resistance == "0"))
			{
			}
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
