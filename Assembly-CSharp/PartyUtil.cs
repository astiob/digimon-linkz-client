using System;
using System.Collections.Generic;

public class PartyUtil
{
	public static Action<MonsterData> ActMIconShort { get; set; }

	public static void SetLock(MonsterData deckMonsterData, bool isDimCheck = false)
	{
		GUIMonsterIcon monsterCS_ByMonsterData = MonsterDataMng.Instance().GetMonsterCS_ByMonsterData(deckMonsterData);
		if (monsterCS_ByMonsterData == null)
		{
			Debug.LogError("PartyUtil.SetLock Error");
			return;
		}
		bool isLocked = deckMonsterData.userMonster.IsLocked;
		if (isDimCheck)
		{
			PartyUtil.SetDimIcon(deckMonsterData, monsterCS_ByMonsterData, isLocked);
		}
		else
		{
			monsterCS_ByMonsterData.Lock = isLocked;
		}
	}

	public static void CheckLockIcons()
	{
		List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList(false);
		foreach (MonsterData deckMonsterData in monsterDataList)
		{
			PartyUtil.SetLock(deckMonsterData, false);
		}
	}

	public static GUIMonsterIcon SetDimIcon(bool isDim, MonsterData deckMonsterData, string warningMessage = "", bool isLock = false)
	{
		if (deckMonsterData == null)
		{
			return null;
		}
		GUIMonsterIcon monsterCS_ByMonsterData = MonsterDataMng.Instance().GetMonsterCS_ByMonsterData(deckMonsterData);
		if (monsterCS_ByMonsterData == null)
		{
			Debug.LogError("PartyUtil.SetDimIcon Error");
			return null;
		}
		if (isLock)
		{
			monsterCS_ByMonsterData.Lock = isLock;
		}
		else if (!isLock)
		{
			deckMonsterData.dimmMess = warningMessage;
		}
		return PartyUtil.SetDimIcon(deckMonsterData, monsterCS_ByMonsterData, isDim);
	}

	private static GUIMonsterIcon SetDimIcon(MonsterData deckMonsterData, GUIMonsterIcon monsterIcon, bool isDim)
	{
		deckMonsterData.dimmLevel = ((!isDim) ? GUIMonsterIcon.DIMM_LEVEL.ACTIVE : GUIMonsterIcon.DIMM_LEVEL.DISABLE);
		monsterIcon.DimmLevel = deckMonsterData.dimmLevel;
		monsterIcon.DimmMess = deckMonsterData.dimmMess;
		monsterIcon.Lock = deckMonsterData.userMonster.IsLocked;
		if (isDim)
		{
			monsterIcon.SetTouchAct_S(null);
		}
		else
		{
			if (PartyUtil.ActMIconShort == null)
			{
				Debug.LogError("[PartyUtil.ActMIconShort = アイコン押下時のメソッド]してから使用してください");
				return null;
			}
			monsterIcon.SetTouchAct_S(PartyUtil.ActMIconShort);
		}
		return monsterIcon;
	}
}
