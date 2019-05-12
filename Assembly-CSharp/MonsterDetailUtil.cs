using System;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterDetailUtil
{
	public static void SetTolerances(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceM, List<GameObject> toleranceGOs)
	{
		string[] array = new string[]
		{
			resistanceM.none,
			resistanceM.fire,
			resistanceM.water,
			resistanceM.thunder,
			resistanceM.nature,
			resistanceM.light,
			resistanceM.dark,
			resistanceM.stun,
			resistanceM.skillLock,
			resistanceM.sleep,
			resistanceM.paralysis,
			resistanceM.confusion,
			resistanceM.poison,
			resistanceM.death
		};
		int i = 0;
		while (i < array.Length)
		{
			toleranceGOs[i].SetActive(true);
			UISprite component = toleranceGOs[i].GetComponent<UISprite>();
			string spriteName = "Battle_Resistance_" + (i + 1).ToString();
			string text = array[i];
			switch (text)
			{
			case "1":
				component.color = new Color(0.784313738f, 0f, 0f);
				break;
			case "0":
				component.color = Color.gray;
				break;
			case "-1":
				component.color = new Color(0f, 0.5882353f, 1f);
				break;
			case "2":
				component.color = Color.green;
				break;
			}
			IL_1AE:
			component.spriteName = spriteName;
			i++;
			continue;
			goto IL_1AE;
		}
	}

	public static void SetInvalidTolerances(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceM, List<GameObject> toleranceGOs)
	{
		int[] array = new int[]
		{
			int.Parse(resistanceM.none),
			int.Parse(resistanceM.fire),
			int.Parse(resistanceM.water),
			int.Parse(resistanceM.thunder),
			int.Parse(resistanceM.nature),
			int.Parse(resistanceM.light),
			int.Parse(resistanceM.dark),
			int.Parse(resistanceM.stun),
			int.Parse(resistanceM.skillLock),
			int.Parse(resistanceM.sleep),
			int.Parse(resistanceM.paralysis),
			int.Parse(resistanceM.confusion),
			int.Parse(resistanceM.poison),
			int.Parse(resistanceM.death)
		};
		for (int i = 0; i < array.Length; i++)
		{
			toleranceGOs[i].SetActive(array[i] == 99);
		}
	}

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
			if (5 < rarity)
			{
				rarity = 5;
			}
			string str = (rarity - 1).ToString();
			result = "Common02_Arousal_" + str;
		}
		return result;
	}
}
