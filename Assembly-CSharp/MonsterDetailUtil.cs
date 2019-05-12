using System;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterDetailUtil
{
	public static void SetTolerances(MonsterData data_chg, List<GameObject> toleranceGOs)
	{
		if (data_chg == null || toleranceGOs == null)
		{
			return;
		}
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM monsterResistanceM = data_chg.AddResistanceFromMultipleTranceData();
		if (monsterResistanceM == null)
		{
			global::Debug.LogError(string.Format("MonsterId{0}のresistanceのマスタが入ってません", data_chg.userMonster.userMonsterId));
			return;
		}
		MonsterDetailUtil.SetTolerances(monsterResistanceM, toleranceGOs);
	}

	public static void SetTolerances(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceM, List<GameObject> toleranceGOs)
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
			toleranceGOs[i].SetActive(true);
			UISprite component = toleranceGOs[i].GetComponent<UISprite>();
			string spriteName = "Battle_Resistance_" + (i + 1).ToString();
			int num = array[i];
			switch (num + 1)
			{
			case 0:
				component.color = new Color(0f, 0.5882353f, 1f);
				break;
			case 1:
				component.color = Color.gray;
				break;
			case 2:
				component.color = new Color(0.784313738f, 0f, 0f);
				break;
			}
			component.spriteName = spriteName;
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

	public static void SetTolerancesDefault(List<GameObject> toleranceGOs)
	{
		if (toleranceGOs == null)
		{
			return;
		}
		for (int i = 0; i < toleranceGOs.Count; i++)
		{
			toleranceGOs[i].SetActive(true);
			UISprite component = toleranceGOs[i].GetComponent<UISprite>();
			string spriteName = "Battle_Resistance_" + (i + 1).ToString();
			component.color = Color.gray;
			component.spriteName = spriteName;
		}
	}

	public static void SetMedals(MonsterData monsterData, List<GameObject> talentGOList)
	{
		if (monsterData == null && talentGOList != null)
		{
			foreach (GameObject gameObject in talentGOList)
			{
				gameObject.SetActive(false);
			}
			return;
		}
		if (monsterData == null || talentGOList == null)
		{
			return;
		}
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster = monsterData.userMonster;
		int[] array = new int[]
		{
			int.Parse(userMonster.hpAbilityFlg),
			int.Parse(userMonster.attackAbilityFlg),
			int.Parse(userMonster.defenseAbilityFlg),
			int.Parse(userMonster.spAttackAbilityFlg),
			int.Parse(userMonster.spDefenseAbilityFlg),
			int.Parse(userMonster.speedAbilityFlg)
		};
		for (int i = 0; i < array.Length; i++)
		{
			talentGOList[i].SetActive(true);
			UISprite component = talentGOList[i].GetComponent<UISprite>();
			int num = array[i];
			if (num != 1)
			{
				if (num != 2)
				{
					talentGOList[i].SetActive(false);
				}
				else
				{
					component.spriteName = "Common02_Talent_Silver";
				}
			}
			else
			{
				component.spriteName = "Common02_Talent_Gold";
			}
		}
	}

	public static void ResetMedals(List<GameObject> talentGOList)
	{
		foreach (GameObject gameObject in talentGOList)
		{
			gameObject.SetActive(false);
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
