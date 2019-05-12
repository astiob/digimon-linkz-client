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

	private MonsterData GetMonsterData(string userMonsterId)
	{
		return ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(userMonsterId);
	}

	public void RefreshUserMonsterByUserMonsterList(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] umL)
	{
		ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(umL);
	}

	public void DestroyAllMonsterData()
	{
		ClassSingleton<MonsterUserDataMng>.Instance.Initialize();
	}

	public List<MonsterData> GetMonsterDataList()
	{
		return ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonsterList();
	}

	public MonsterData GetMonsterDataByUserMonsterID(string userMonsterId, bool noop = false)
	{
		return ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(userMonsterId);
	}

	public List<MonsterData> GetMonsterDataListByMID(string monsterId)
	{
		return ClassSingleton<MonsterUserDataMng>.Instance.GetMonsterList(monsterId);
	}

	public MonsterData GetMonsterDataByUserMonsterLargeID()
	{
		return ClassSingleton<MonsterUserDataMng>.Instance.GetOldestMonster();
	}

	public List<MonsterData> GetDeckMonsterDataList()
	{
		return ClassSingleton<MonsterUserDataMng>.Instance.GetDeckUserMonsterList();
	}

	public bool HasGrowStepHigh(List<MonsterData> mdL)
	{
		return MonsterUserDataMng.AnyHighGrowStepMonster(mdL);
	}

	public bool HasChip(List<MonsterData> mdL)
	{
		return MonsterUserDataMng.AnyChipEquipMonster(mdL);
	}

	public MonsterData CreateMonsterDataByMID(string monsterId)
	{
		return MonsterData.CreateMonsterData(monsterId);
	}

	public void InitMonsterGO()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.InitMonsterGO(Singleton<GUIMain>.Instance.transform, this.GetMonsterDataList());
	}

	public GameObject GetMonsterPrefabByMonsterData(MonsterData md)
	{
		GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(md);
		return icon.gameObject;
	}

	public GUIMonsterIcon GetMonsterCS_ByMonsterData(MonsterData md)
	{
		return ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(md);
	}

	public void PushBackAllMonsterPrefab()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.PushBackAllMonsterPrefab();
	}

	public void DestroyAllMonsterDataAndPrefab()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.AllDisable();
		ClassSingleton<GUIMonsterIconList>.Instance.AllDelete();
	}

	public void UnnewMonserDataList()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.UnnewMonserDataList();
	}

	public void SetSortLSMessage()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.SetSortLSMessage(CMD_BaseSelect.IconSortType);
	}

	public void SetDimmAll(GUIMonsterIcon.DIMM_LEVEL level)
	{
		ClassSingleton<GUIMonsterIconList>.Instance.SetDimmAll(level);
	}

	public void SetSelectOffAll()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.SetSelectOffAll();
	}

	public void ClearDimmMessAll()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.ClearDimmMessAll();
	}

	public void ClearSortMessAll()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.ClearAction();
		ClassSingleton<GUIMonsterIconList>.Instance.ClearSortMessAll();
	}

	public void ClearLevelMessAll()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.ClearLevelMessAll();
	}

	public void SetLockIcon()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.SetLockIcon();
	}

	public string GetMonsterCharaPathByMonsterGroupId(string monsterGroupId)
	{
		return MonsterObject.GetFilePath(monsterGroupId);
	}

	public List<string> GetDeckMonsterPathList(bool favour)
	{
		return ClassSingleton<MonsterUserDataMng>.Instance.GetDeckMonsterPathList(favour);
	}

	public GameWebAPI.RespDataMA_GetMonsterMS.MonsterM GetMonsterMasterByMonsterId(string monsterId)
	{
		MonsterClientMaster monsterMasterByMonsterId = MonsterMaster.GetMonsterMasterByMonsterId(monsterId);
		return monsterMasterByMonsterId.Simple;
	}

	public GameWebAPI.RespDataMA_GetMonsterMG.MonsterM GetMonsterGroupMasterByMonsterGroupId(string monsterGroupId)
	{
		MonsterClientMaster monsterMasterByMonsterGroupId = MonsterMaster.GetMonsterMasterByMonsterGroupId(monsterGroupId);
		return monsterMasterByMonsterGroupId.Group;
	}

	public GameWebAPI.RespDataMA_GetMonsterMG.MonsterM GetMonsterGroupMasterByMonsterId(string monsterId)
	{
		MonsterClientMaster monsterMasterByMonsterId = MonsterMaster.GetMonsterMasterByMonsterId(monsterId);
		return monsterMasterByMonsterId.Group;
	}

	public GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM GetMonsterResistanceMasterByMonsterId(string monsterId)
	{
		MonsterClientMaster monsterMasterByMonsterId = MonsterMaster.GetMonsterMasterByMonsterId(monsterId);
		return MonsterResistanceData.GetResistanceMaster(monsterMasterByMonsterId.Simple.resistanceId);
	}

	public void SortMDList(List<MonsterData> destMonsterDataList)
	{
		this.SortMonsterUserDataList(destMonsterDataList, CMD_BaseSelect.IconSortType, CMD_BaseSelect.IconSortOrder);
	}

	public void SortMonsterUserDataList(List<MonsterData> destMonsterDataList, MonsterSortType sortType, MonsterSortOrder sortOrder)
	{
		MonsterSort.SortMonsterUserDataList(destMonsterDataList, sortType, sortOrder);
	}

	public List<MonsterData> SelectMonsterDataList(List<MonsterData> monsterDataList, MonsterFilterType type)
	{
		return MonsterFilter.Filter(monsterDataList, type);
	}

	public List<MonsterData> SelectionMDList(List<MonsterData> mdList)
	{
		return this.DetailFilterMonsterDataList(mdList, CMD_BaseSelect.IconFilterType);
	}

	public List<MonsterData> DetailFilterMonsterDataList(List<MonsterData> mdList, MonsterDetailedFilterType type)
	{
		return MonsterFilter.DetailedFilter(mdList, type);
	}
}
