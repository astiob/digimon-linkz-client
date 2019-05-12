using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDataMng : MonoBehaviour
{
	private static MonsterDataMng instance;

	public static MonsterDataMng Instance()
	{
		return MonsterDataMng.instance;
	}

	protected virtual void Awake()
	{
		MonsterDataMng.instance = this;
	}

	protected virtual void OnDestroy()
	{
		MonsterDataMng.instance = null;
	}

	public List<MonsterData> GetMonsterDataList()
	{
		return ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonsterList();
	}

	public MonsterData GetMonsterDataByUserMonsterID(string userMonsterId, bool noop = false)
	{
		return ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(userMonsterId);
	}

	public MonsterData CreateMonsterDataByMID(string monsterId)
	{
		return MonsterData.CreateMonsterData(monsterId);
	}

	public void SetSortLSMessage()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.SetSortLSMessage(CMD_BaseSelect.IconSortType);
	}

	public void SortMDList(List<MonsterData> destMonsterDataList)
	{
		MonsterSort.SortMonsterUserDataList(destMonsterDataList, CMD_BaseSelect.IconSortType, CMD_BaseSelect.IconSortOrder);
	}

	public List<MonsterData> SelectionMDList(List<MonsterData> mdList)
	{
		return MonsterFilter.DetailedFilter(mdList, CMD_BaseSelect.IconFilterType);
	}
}
