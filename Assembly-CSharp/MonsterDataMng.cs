using Evolution;
using Master;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterDataMng : MonoBehaviour
{
	private static MonsterDataMng instance;

	private List<MonsterData> monster_data_list;

	private List<GUIMonsterIcon> monster_icon_list;

	private GameObject goMONSTER_ICON_BAK_ROOT;

	private GameObject goMONSTER_ICON_M;

	private List<MonsterData> mdl_add_last;

	private List<MonsterData> mdl_chg_last;

	private List<MonsterData> monsterDataListBK;

	private MonsterDataMng.SORT_TYPE nowSortType = MonsterDataMng.SORT_TYPE.LEVEL;

	private MonsterDataMng.SORT_DIR nowSortDir;

	private MonsterDataMng.SELECTION_TYPE nowSelectionType;

	public static GameWebAPI.RespDataMN_Picturebook userPicturebookData;

	public GameWebAPI.RespDataCS_MonsterSlotInfoListLogic userMonsterSlotInfoListLogic { get; set; }

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

	public List<MonsterData> GetMonsterDataList(bool refresh = false)
	{
		if (!refresh && this.monster_data_list != null)
		{
			return this.monster_data_list;
		}
		this.monster_data_list = new List<MonsterData>();
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] userMonsterList = DataMng.Instance().RespDataUS_MonsterList.userMonsterList;
		for (int i = 0; i < userMonsterList.Length; i++)
		{
			MonsterData monsterData = new MonsterData();
			monsterData.DuplicateUserMonster(userMonsterList[i]);
			monsterData.monsterM = this.GetMonsterMasterByMonsterId(userMonsterList[i].monsterId);
			monsterData.monsterMG = this.GetMonsterGroupMasterByMonsterGroupId(monsterData.monsterM.monsterGroupId);
			monsterData.InitSkillInfo();
			monsterData.InitResistanceInfo();
			monsterData.InitGrowStepInfo();
			monsterData.InitTribeInfo();
			monsterData.InitChipInfo();
			monsterData.idx = -1;
			this.monster_data_list.Add(monsterData);
		}
		return this.monster_data_list;
	}

	public void InitMonsterGO()
	{
		this.monster_icon_list = new List<GUIMonsterIcon>();
		if (this.goMONSTER_ICON_BAK_ROOT != null)
		{
			UnityEngine.Object.DestroyImmediate(this.goMONSTER_ICON_BAK_ROOT);
		}
		this.goMONSTER_ICON_BAK_ROOT = new GameObject();
		this.goMONSTER_ICON_BAK_ROOT.name = "MONSTER_ICON_BAK_ROOT";
		this.goMONSTER_ICON_BAK_ROOT.transform.parent = base.transform;
		this.goMONSTER_ICON_BAK_ROOT.transform.localScale = new Vector3(1f, 1f, 1f);
		this.goMONSTER_ICON_BAK_ROOT.transform.localPosition = new Vector3(2000f, 2000f, 0f);
		this.goMONSTER_ICON_M = GUIManager.LoadCommonGUI("ListParts/ListPartsThumbnail", this.goMONSTER_ICON_BAK_ROOT);
		this.goMONSTER_ICON_M.transform.localScale = new Vector3(1f, 1f, 1f);
		float num = 200f;
		float num2 = -200f;
		float num3 = 200f;
		float num4 = 200f;
		for (int i = 0; i < this.monster_data_list.Count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.goMONSTER_ICON_M);
			gameObject.name += this.monster_data_list[i].monsterM.monsterId;
			gameObject.transform.parent = this.goMONSTER_ICON_BAK_ROOT.transform;
			gameObject.transform.localScale = Vector3.one;
			float num5 = num3 * (float)(i % 5);
			float num6 = num4 * (float)(i / 5);
			gameObject.transform.localPosition = new Vector3(num + num5, num2 - num6, -10f);
			GUIMonsterIcon component = gameObject.GetComponent<GUIMonsterIcon>();
			component.Data = this.monster_data_list[i];
			this.monster_data_list[i].idx = i;
			gameObject.SetActive(false);
			this.monster_icon_list.Add(component);
		}
		this.goMONSTER_ICON_M.SetActive(false);
	}

	public List<MonsterData> GetLastAddedMonsterDataList()
	{
		return this.mdl_add_last;
	}

	public List<MonsterData> GetLastChangedMonsterDataList()
	{
		return this.mdl_chg_last;
	}

	public List<MonsterData> RefreshMonsterDataList()
	{
		this.PushBackAllMonsterPrefab();
		List<MonsterData> list = new List<MonsterData>();
		List<GUIMonsterIcon> list2 = new List<GUIMonsterIcon>();
		this.mdl_add_last = new List<MonsterData>();
		this.mdl_chg_last = new List<MonsterData>();
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] userMonsterList = DataMng.Instance().RespDataUS_MonsterList.userMonsterList;
		int m;
		for (m = 0; m < this.monster_data_list.Count; m++)
		{
			this.monster_data_list[m].isTrash = true;
			this.monster_data_list[m].isEvolve = false;
		}
		for (m = 0; m < userMonsterList.Length; m++)
		{
			int i;
			for (i = 0; i < this.monster_data_list.Count; i++)
			{
				if (this.monster_data_list[i].userMonster.userMonsterId == userMonsterList[m].userMonsterId)
				{
					if (this.monster_data_list[i].userMonster.monsterId != userMonsterList[m].monsterId || this.monster_data_list[i].userMonster.eggFlg != userMonsterList[m].eggFlg || this.monster_data_list[i].userMonster.growEndDate != userMonsterList[m].growEndDate)
					{
						this.monster_data_list[i].isEvolve = true;
						this.monster_data_list[i].monsterM = this.GetMonsterMasterByMonsterId(userMonsterList[m].monsterId);
						this.monster_data_list[i].monsterMG = this.GetMonsterGroupMasterByMonsterGroupId(this.monster_data_list[i].monsterM.monsterGroupId);
						this.mdl_chg_last.Add(this.monster_data_list[i]);
					}
					this.monster_data_list[i].isTrash = false;
					this.monster_data_list[i].DuplicateUserMonster(userMonsterList[m]);
					this.monster_data_list[i].InitSkillInfo();
					this.monster_data_list[i].InitResistanceInfo();
					this.monster_data_list[i].InitGrowStepInfo();
					this.monster_data_list[i].InitTribeInfo();
					this.monster_data_list[i].InitChipInfo();
					list.Add(this.monster_data_list[i]);
					break;
				}
			}
			if (i == this.monster_data_list.Count)
			{
				MonsterData monsterData = new MonsterData();
				monsterData.DuplicateUserMonster(userMonsterList[m]);
				monsterData.monsterM = this.GetMonsterMasterByMonsterId(userMonsterList[m].monsterId);
				monsterData.monsterMG = this.GetMonsterGroupMasterByMonsterGroupId(monsterData.monsterM.monsterGroupId);
				monsterData.idx = -1;
				monsterData.InitSkillInfo();
				monsterData.InitResistanceInfo();
				monsterData.InitGrowStepInfo();
				monsterData.InitTribeInfo();
				monsterData.InitChipInfo();
				list.Add(monsterData);
				this.mdl_add_last.Add(monsterData);
			}
		}
		for (m = 0; m < this.monster_data_list.Count; m++)
		{
			if (this.monster_data_list[m].isTrash)
			{
				int idx = this.monster_data_list[m].idx;
				this.userMonsterSlotInfoListLogic.slotInfo = this.userMonsterSlotInfoListLogic.slotInfo.Where((GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo slot) => slot.userMonsterId != int.Parse(this.monster_data_list[m].userMonster.userMonsterId)).ToArray<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo>();
				UnityEngine.Object.DestroyImmediate(this.monster_icon_list[idx].gameObject);
			}
		}
		float num = 200f;
		float num2 = -200f;
		float num3 = 200f;
		float num4 = 200f;
		for (m = 0; m < list.Count; m++)
		{
			float num5 = num3 * (float)(m % 5);
			float num6 = num4 * (float)(m / 5);
			Vector3 vector = new Vector3(num + num5, num2 - num6, -10f);
			GameObject gameObject;
			if (list[m].idx > -1)
			{
				GUIMonsterIcon guimonsterIcon = this.monster_icon_list[list[m].idx];
				if (list[m].isEvolve)
				{
					guimonsterIcon.ShowGUI();
				}
				list2.Add(guimonsterIcon);
				gameObject = guimonsterIcon.gameObject;
				gameObject.transform.localPosition = vector;
				gameObject.transform.localScale = Vector3.one;
			}
			else
			{
				GUIMonsterIcon guimonsterIcon2 = this.MakePrefabByMonsterData(list[m], Vector3.one, vector, null, false, false);
				list2.Add(guimonsterIcon2);
				gameObject = guimonsterIcon2.gameObject;
			}
			gameObject.SetActive(false);
			list[m].idx = m;
		}
		this.monster_data_list = list;
		this.monster_icon_list = list2;
		return this.monster_data_list;
	}

	public void ChangeLevelMD_and_DM(MonsterData md, DataMng.ExperienceInfo ei)
	{
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] userMonsterList = DataMng.Instance().RespDataUS_MonsterList.userMonsterList;
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonsterList2 = Algorithm.BinarySearch<GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList>(userMonsterList, md.userMonster.userMonsterId, 0, userMonsterList.Length - 1, "userMonsterId", 8);
		userMonsterList2.ex = ei.exp.ToString();
		userMonsterList2.level = ei.lev.ToString();
		md.DuplicateUserMonster(userMonsterList2);
	}

	public bool IsHaveMonster(string monsterId)
	{
		for (int i = 0; i < this.monster_data_list.Count; i++)
		{
			if (this.monster_data_list[i].userMonster.monsterId == monsterId)
			{
				return true;
			}
		}
		return false;
	}

	public MonsterData GetMonsterDataByUserMonsterID(string userMonsterId, bool isEvolvePage = false)
	{
		this.monster_data_list.Sort(delegate(MonsterData a, MonsterData b)
		{
			int num = int.Parse(a.userMonster.userMonsterId);
			int num2 = int.Parse(b.userMonster.userMonsterId);
			if (num > num2)
			{
				return 1;
			}
			if (num < num2)
			{
				return -1;
			}
			return 0;
		});
		MonsterData result = Algorithm.BinarySearch<MonsterData>(this.monster_data_list, userMonsterId, 0, this.monster_data_list.Count - 1, "userMonsterId", "userMonster", 8);
		this.SortMDList(this.monster_data_list, isEvolvePage);
		return result;
	}

	public List<MonsterData> GetMonsterDataListByUserMonsterIDList(List<string> ids)
	{
		List<MonsterData> list = new List<MonsterData>();
		this.monster_data_list.Sort(delegate(MonsterData a, MonsterData b)
		{
			int num = int.Parse(a.userMonster.userMonsterId);
			int num2 = int.Parse(b.userMonster.userMonsterId);
			if (num > num2)
			{
				return 1;
			}
			if (num < num2)
			{
				return -1;
			}
			return 0;
		});
		foreach (string keyID in ids)
		{
			MonsterData monsterData = Algorithm.BinarySearch<MonsterData>(this.monster_data_list, keyID, 0, this.monster_data_list.Count - 1, "userMonsterId", "userMonster", 8);
			if (monsterData != null)
			{
				list.Add(monsterData);
			}
		}
		this.SortMDList(this.monster_data_list, false);
		return list;
	}

	public string GetMonsterIconPathByMonsterData(MonsterData md)
	{
		return this.GetMonsterIconPathByIconId(md.monsterM.iconId);
	}

	public string GetMonsterIconPathByIconId(string iconId)
	{
		return "CharacterThumbnail/" + iconId + "/thumb";
	}

	public string InternalGetMonsterIconPathByMonsterData(MonsterData md)
	{
		return this.InternalGetMonsterIconPathByIconId(md.monsterM.iconId);
	}

	public string InternalGetMonsterIconPathByIconId(string iconId)
	{
		return "CharacterThumbnailInternal/" + iconId + "/thumb";
	}

	public string GetMonsterCharaPathByMonsterData(MonsterData md)
	{
		return this.GetMonsterCharaPathByMonsterGroupId(md.monsterM.monsterGroupId);
	}

	public string GetMonsterCharaPathByMonsterGroupId(string monsterGroupId)
	{
		return "Characters/" + monsterGroupId + "/prefab";
	}

	public string GetEvolveItemIconPathByID(string id)
	{
		return "EvolveItemThumbnail/" + id + "/thumb";
	}

	public GameObject GetMonsterPrefabByMonsterData(MonsterData md)
	{
		if (md == null || md.idx < 0 || md.idx >= this.monster_icon_list.Count || this.monster_icon_list[md.idx] == null)
		{
			global::Debug.LogError("MonsterDataMng.GetMonsterPrefabByMonsterData Error");
			return null;
		}
		return this.monster_icon_list[md.idx].gameObject;
	}

	public GUIMonsterIcon GetMonsterCS_ByMonsterData(MonsterData md)
	{
		if (md == null || md.idx < 0 || md.idx >= this.monster_icon_list.Count)
		{
			global::Debug.LogError("MonsterDataMng.GetMonsterCS_ByMonsterData Error");
			return null;
		}
		return this.monster_icon_list[md.idx];
	}

	public List<MonsterData> GetDeckMonsterDataList(bool favour = false)
	{
		GameWebAPI.RespDataMN_GetDeckList.DeckList[] deckList = DataMng.Instance().RespDataMN_DeckList.deckList;
		string favoriteDeckNum = DataMng.Instance().RespDataMN_DeckList.favoriteDeckNum;
		List<string> list = new List<string>();
		List<MonsterData> list2 = new List<MonsterData>();
		for (int i = 0; i < deckList.Length; i++)
		{
			if (!favour || !(favoriteDeckNum != deckList[i].deckNum))
			{
				GameWebAPI.RespDataMN_GetDeckList.MonsterList[] monsterList = deckList[i].monsterList;
				for (int j = 0; j < monsterList.Length; j++)
				{
					list.Add(monsterList[j].userMonsterId);
				}
			}
		}
		for (int i = 0; i < this.monster_data_list.Count; i++)
		{
			string userMonsterId = this.monster_data_list[i].userMonster.userMonsterId;
			for (int j = 0; j < list.Count; j++)
			{
				if (userMonsterId == list[j])
				{
					list2.Add(this.monster_data_list[i]);
					break;
				}
			}
		}
		return list2;
	}

	public List<string> GetDeckMonsterPathList(bool favour = false)
	{
		List<MonsterData> deckMonsterDataList = this.GetDeckMonsterDataList(favour);
		List<string> list = new List<string>();
		for (int i = 0; i < deckMonsterDataList.Count; i++)
		{
			string monsterCharaPathByMonsterGroupId = this.GetMonsterCharaPathByMonsterGroupId(deckMonsterDataList[i].monsterM.monsterGroupId);
			list.Add(monsterCharaPathByMonsterGroupId);
		}
		return list;
	}

	public MonsterData CreateMonsterDataByMID(string monsterId)
	{
		MonsterData monsterData = new MonsterData();
		monsterData.userMonster = new GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList();
		monsterData.userMonster.userMonsterId = "-1";
		monsterData.userMonster.monsterId = monsterId;
		monsterData.userMonster.level = "1";
		monsterData.monsterM = this.GetMonsterMasterByMonsterId(monsterId);
		monsterData.monsterMG = this.GetMonsterGroupMasterByMonsterGroupId(monsterData.monsterM.monsterGroupId);
		monsterData.InitSkillInfo();
		monsterData.InitResistanceInfo();
		monsterData.InitGrowStepInfo();
		monsterData.InitTribeInfo();
		monsterData.idx = -1;
		monsterData.dimmLevel = GUIMonsterIcon.DIMM_LEVEL.ACTIVE;
		monsterData.New = false;
		monsterData.Lock = monsterData.userMonster.IsLocked;
		monsterData.selectNum = -1;
		return monsterData;
	}

	public List<MonsterData> GetMonsterDataListByMID(string monsterId)
	{
		List<MonsterData> list = new List<MonsterData>();
		for (int i = 0; i < this.monster_data_list.Count; i++)
		{
			if (this.monster_data_list[i].monsterM.monsterId == monsterId)
			{
				list.Add(this.monster_data_list[i]);
			}
		}
		return list;
	}

	public GameWebAPI.UserSoulData GetUserSoulDataBySID(string soulId)
	{
		GameWebAPI.UserSoulData[] userSoulData = DataMng.Instance().RespDataUS_SoulInfo.userSoulData;
		GameWebAPI.UserSoulData userSoulData2 = Algorithm.BinarySearch<GameWebAPI.UserSoulData>(userSoulData, soulId, 0, userSoulData.Length - 1, "soulId", 8);
		if (userSoulData2 == null)
		{
			userSoulData2 = new GameWebAPI.UserSoulData();
			userSoulData2.soulId = soulId;
			userSoulData2.num = "0";
		}
		return userSoulData2;
	}

	public MonsterData GetMonsterDataByUserMonsterLargeID()
	{
		int num = -1;
		int index = 0;
		for (int i = 0; i < this.monster_data_list.Count; i++)
		{
			int num2 = int.Parse(this.monster_data_list[i].userMonster.userMonsterId);
			if (num2 > num)
			{
				num = num2;
				index = i;
			}
		}
		if (num > -1)
		{
			return this.monster_data_list[index];
		}
		return null;
	}

	public GameWebAPI.RespDataMA_GetMonsterMS.MonsterM GetMonsterMasterByMonsterId(string monsterId)
	{
		GameWebAPI.RespDataMA_GetMonsterMS.MonsterM[] monsterM = MasterDataMng.Instance().RespDataMA_MonsterMS.monsterM;
		int num = 0;
		if (int.TryParse(monsterId, out num))
		{
			if (num < monsterM.Length - 1)
			{
				if (num <= 0)
				{
					global::Debug.LogError("monsterId == 0になっています。マスターデータは大丈夫ですか？");
					num = 1;
				}
				if (monsterM[num - 1].monsterId == monsterId)
				{
					return monsterM[num - 1];
				}
			}
			return Algorithm.BinarySearch<GameWebAPI.RespDataMA_GetMonsterMS.MonsterM>(monsterM, monsterId, 0, monsterM.Length - 1, "monsterId", 8);
		}
		global::Debug.LogError("not found [ monster id : " + monsterId + " ] from MonsterMaster");
		return null;
	}

	public GameWebAPI.RespDataMA_GetMonsterMG.MonsterM GetMonsterGroupMasterByMonsterGroupId(string monsterGroupId)
	{
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM[] monsterM = MasterDataMng.Instance().RespDataMA_MonsterMG.monsterM;
		int num = 0;
		if (int.TryParse(monsterGroupId, out num))
		{
			if (num < monsterM.Length - 1)
			{
				if (num <= 0)
				{
					global::Debug.LogError("monsterGroupId == 0になっています。マスターデータは大丈夫ですか？");
					num = 1;
				}
				if (monsterM[num - 1].monsterGroupId == monsterGroupId)
				{
					return monsterM[num - 1];
				}
			}
			return Algorithm.BinarySearch<GameWebAPI.RespDataMA_GetMonsterMG.MonsterM>(monsterM, monsterGroupId, 0, monsterM.Length - 1, "monsterGroupId", 8);
		}
		global::Debug.LogError("not found [ monster group id : " + monsterGroupId + " ] from MonsterMaster");
		return null;
	}

	public GameWebAPI.RespDataMA_GetMonsterMG.MonsterM GetMonsterGroupMasterByMonsterId(string monsterId)
	{
		GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterMasterByMonsterId = this.GetMonsterMasterByMonsterId(monsterId);
		return this.GetMonsterGroupMasterByMonsterGroupId(monsterMasterByMonsterId.monsterGroupId);
	}

	public GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM GetMonsterResistanceMasterByMonsterId(string monsterId)
	{
		GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterMasterByMonsterId = this.GetMonsterMasterByMonsterId(monsterId);
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM[] monsterResistanceM = MasterDataMng.Instance().RespDataMA_MonsterResistanceM.monsterResistanceM;
		int num = 0;
		if (int.TryParse(monsterMasterByMonsterId.resistanceId, out num))
		{
			if (num < monsterResistanceM.Length - 1)
			{
				if (num <= 0)
				{
					global::Debug.LogError("monsM.resistanceId == 0になっています。マスターデータは大丈夫ですか？");
					num = 1;
				}
				if (monsterResistanceM[num - 1].monsterResistanceId == monsterMasterByMonsterId.resistanceId)
				{
					return monsterResistanceM[num - 1];
				}
			}
			return Algorithm.BinarySearch<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM>(monsterResistanceM, monsterId, 0, monsterResistanceM.Length - 1, "monsterResistanceId", 8);
		}
		global::Debug.LogError("not found [ resistance id : " + monsterMasterByMonsterId.resistanceId + " ] from ResistanceMaster");
		return null;
	}

	public string GetEggTypeFromMD(MonsterData md)
	{
		string result = "1";
		foreach (GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM.MonsterEvolutionRouteM monsterEvolutionRouteM2 in MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM)
		{
			if (monsterEvolutionRouteM2.monsterEvolutionRouteId == md.userMonster.monsterEvolutionRouteId)
			{
				result = monsterEvolutionRouteM2.eggMonsterId;
				break;
			}
		}
		return result;
	}

	public void PushBackAllMonsterPrefab()
	{
		if (this.monster_icon_list != null)
		{
			float num = 200f;
			float num2 = -200f;
			float num3 = 200f;
			float num4 = 200f;
			for (int i = 0; i < this.monster_icon_list.Count; i++)
			{
				this.monster_icon_list[i].transform.parent = this.goMONSTER_ICON_BAK_ROOT.transform;
				this.monster_icon_list[i].transform.localScale = Vector3.one;
				float num5 = num3 * (float)(i % 5);
				float num6 = num4 * (float)(i / 5);
				this.monster_icon_list[i].transform.localPosition = new Vector3(num + num5, num2 - num6, -10f);
				this.monster_icon_list[i].ResetDepthToOriginal();
				this.monster_icon_list[i].gameObject.SetActive(false);
			}
		}
	}

	public void DestroyAllMonsterDataAndPrefab()
	{
		for (int i = 0; i < this.monster_data_list.Count; i++)
		{
			int idx = this.monster_data_list[i].idx;
			UnityEngine.Object.DestroyImmediate(this.monster_icon_list[idx].gameObject);
		}
		this.monster_icon_list = null;
	}

	public void DestroyAllMonsterData()
	{
		this.monster_data_list = null;
		if (this.monster_icon_list != null)
		{
			for (int i = 0; i < this.monster_icon_list.Count; i++)
			{
				UnityEngine.Object.Destroy(this.monster_icon_list[i].gameObject);
			}
		}
		this.monster_icon_list = null;
		DataMng.Instance().RespDataUS_MonsterList = new GameWebAPI.RespDataUS_GetMonsterList();
		DataMng.Instance().RespDataUS_MonsterList.userMonsterList = new GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[0];
	}

	public GameWebAPI.RespDataMA_GetSoulM.SoulM GetSoulMasterBySoulId(string sid)
	{
		GameWebAPI.RespDataMA_GetSoulM.SoulM[] soulM = MasterDataMng.Instance().RespDataMA_SoulM.soulM;
		return Algorithm.BinarySearch<GameWebAPI.RespDataMA_GetSoulM.SoulM>(soulM, sid, 0, soulM.Length - 1, "soulId", 8);
	}

	public int GetAddExpFromMonsterDataList(List<MonsterData> mdL)
	{
		int num = 0;
		for (int i = 0; i < mdL.Count; i++)
		{
			int num2 = int.Parse(mdL[i].userMonster.level) - 1;
			int num3 = int.Parse(mdL[i].monsterM.fusionExp);
			int num4 = int.Parse(mdL[i].monsterM.fusionExpRise);
			num += num3 + num4 * num2;
		}
		return num;
	}

	public float GetReinforcementCost(List<MonsterData> mdL)
	{
		float num = 0f;
		for (int i = 0; i < mdL.Count; i++)
		{
			int num2 = int.Parse(mdL[i].userMonster.level) - 1;
			int num3 = int.Parse(mdL[i].monsterM.fusionExp);
			int num4 = int.Parse(mdL[i].monsterM.fusionExpRise);
			num += (float)(num3 + num4 * num2 + 70) * ConstValue.REINFORCEMENT_COEFFICIENT;
		}
		return num;
	}

	public List<MonsterData> SelectMonsterDataList(List<MonsterData> monsterDataList, MonsterDataMng.SELECT_TYPE type = MonsterDataMng.SELECT_TYPE.ALL)
	{
		if (type == MonsterDataMng.SELECT_TYPE.ALL)
		{
			return monsterDataList;
		}
		this.monsterDataListBK = new List<MonsterData>();
		switch (type)
		{
		case MonsterDataMng.SELECT_TYPE.GROWING_IN_GARDEN:
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				int num = int.Parse(monsterDataList[i].monsterMG.growStep);
				string growEndDate = monsterDataList[i].userMonster.growEndDate;
				bool flag = monsterDataList[i].userMonster.eggFlg == "1";
				if (num < 4 && (flag || !string.IsNullOrEmpty(growEndDate)))
				{
					this.monsterDataListBK.Add(monsterDataList[i]);
				}
			}
			break;
		case MonsterDataMng.SELECT_TYPE.ALL_OUT_GARDEN:
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				int num2 = int.Parse(monsterDataList[i].monsterMG.growStep);
				if (num2 >= 4)
				{
					this.monsterDataListBK.Add(monsterDataList[i]);
				}
			}
			break;
		case MonsterDataMng.SELECT_TYPE.RESEARCH_TARGET:
		{
			string b = 7.ToString();
			string b2 = 9.ToString();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				if (monsterDataList[i].monsterMG.growStep == b || monsterDataList[i].monsterMG.growStep == b2)
				{
					this.monsterDataListBK.Add(monsterDataList[i]);
				}
			}
			break;
		}
		case MonsterDataMng.SELECT_TYPE.CAN_EVOLVE:
			foreach (MonsterData monsterData in monsterDataList)
			{
				List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> evoList = ClassSingleton<EvolutionData>.Instance.GetEvoList(monsterData.monsterM.monsterId);
				int num3 = int.Parse(monsterData.monsterMG.growStep);
				if (num3 >= 4 && evoList.Count > 0)
				{
					this.monsterDataListBK.Add(monsterData);
				}
			}
			break;
		case MonsterDataMng.SELECT_TYPE.ALL_IN_GARDEN:
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				int num4 = int.Parse(monsterDataList[i].monsterMG.growStep);
				if (num4 < 4)
				{
					this.monsterDataListBK.Add(monsterDataList[i]);
				}
			}
			break;
		}
		return this.monsterDataListBK;
	}

	public int SelectMonsterDataListCount(List<MonsterData> monsterDataList, MonsterDataMng.SELECT_TYPE type = MonsterDataMng.SELECT_TYPE.ALL)
	{
		if (type == MonsterDataMng.SELECT_TYPE.ALL)
		{
			return monsterDataList.Count;
		}
		int num = 0;
		switch (type)
		{
		case MonsterDataMng.SELECT_TYPE.GROWING_IN_GARDEN:
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				int num2 = int.Parse(monsterDataList[i].monsterMG.growStep);
				string growEndDate = monsterDataList[i].userMonster.growEndDate;
				bool flag = monsterDataList[i].userMonster.eggFlg == "1";
				if (num2 < 4 && (flag || !string.IsNullOrEmpty(growEndDate)))
				{
					num++;
				}
			}
			break;
		case MonsterDataMng.SELECT_TYPE.ALL_OUT_GARDEN:
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				int num3 = int.Parse(monsterDataList[i].monsterMG.growStep);
				if (num3 >= 4)
				{
					num++;
				}
			}
			break;
		case MonsterDataMng.SELECT_TYPE.RESEARCH_TARGET:
		{
			string b = 7.ToString();
			string b2 = 9.ToString();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				if (monsterDataList[i].monsterMG.growStep == b || monsterDataList[i].monsterMG.growStep == b2)
				{
					num++;
				}
			}
			break;
		}
		case MonsterDataMng.SELECT_TYPE.CAN_EVOLVE:
			foreach (MonsterData monsterData in monsterDataList)
			{
				List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> evoList = ClassSingleton<EvolutionData>.Instance.GetEvoList(monsterData.monsterM.monsterId);
				int num4 = int.Parse(monsterData.monsterMG.growStep);
				if (num4 >= 4 && evoList.Count > 0)
				{
					num++;
				}
			}
			break;
		case MonsterDataMng.SELECT_TYPE.ALL_IN_GARDEN:
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				int num5 = int.Parse(monsterDataList[i].monsterMG.growStep);
				if (num5 < 4)
				{
					num++;
				}
			}
			break;
		}
		return num;
	}

	public List<MonsterData> GetSelectMonsterDataList()
	{
		return this.monsterDataListBK;
	}

	public MonsterDataMng.SORT_TYPE NowSortType
	{
		get
		{
			return this.nowSortType;
		}
		set
		{
			this.nowSortType = value;
		}
	}

	public MonsterDataMng.SORT_DIR NowSortDir
	{
		get
		{
			return this.nowSortDir;
		}
		set
		{
			this.nowSortDir = value;
		}
	}

	public List<MonsterData> SortMDList(List<MonsterData> mdList, bool isEvolvePage = false)
	{
		switch (this.nowSortType)
		{
		case MonsterDataMng.SORT_TYPE.DATE:
			mdList.Sort(new Comparison<MonsterData>(this.CompareDATE));
			break;
		case MonsterDataMng.SORT_TYPE.RARE:
			mdList.Sort(new Comparison<MonsterData>(this.CompareRARE));
			break;
		case MonsterDataMng.SORT_TYPE.LEVEL:
			mdList.Sort(new Comparison<MonsterData>(this.CompareLEVEL));
			break;
		case MonsterDataMng.SORT_TYPE.HP:
			mdList.Sort(new Comparison<MonsterData>(this.CompareHP));
			break;
		case MonsterDataMng.SORT_TYPE.ATK:
			mdList.Sort(new Comparison<MonsterData>(this.CompareATK));
			break;
		case MonsterDataMng.SORT_TYPE.DEF:
			mdList.Sort(new Comparison<MonsterData>(this.CompareDEF));
			break;
		case MonsterDataMng.SORT_TYPE.S_ATK:
			mdList.Sort(new Comparison<MonsterData>(this.CompareS_ATK));
			break;
		case MonsterDataMng.SORT_TYPE.S_DEF:
			mdList.Sort(new Comparison<MonsterData>(this.CompareS_DEF));
			break;
		case MonsterDataMng.SORT_TYPE.SPD:
			mdList.Sort(new Comparison<MonsterData>(this.CompareSPD));
			break;
		case MonsterDataMng.SORT_TYPE.LUCK:
			mdList.Sort(new Comparison<MonsterData>(this.CompareLUCK));
			break;
		case MonsterDataMng.SORT_TYPE.GRADE:
			mdList.Sort(new Comparison<MonsterData>(this.CompareGRADE));
			break;
		case MonsterDataMng.SORT_TYPE.TRIBE:
			mdList.Sort(new Comparison<MonsterData>(this.CompareTRIBE));
			break;
		case MonsterDataMng.SORT_TYPE.FRIENDSHIP:
			mdList.Sort(new Comparison<MonsterData>(this.CompareFRIENDSHIP));
			break;
		default:
			mdList.Sort(new Comparison<MonsterData>(this.CompareLEVEL));
			break;
		}
		mdList = this.SetSortLSMessage(mdList, isEvolvePage);
		return mdList;
	}

	private int CompareDATE(MonsterData x, MonsterData y)
	{
		int num = int.Parse(x.userMonster.userMonsterId);
		int num2 = int.Parse(y.userMonster.userMonsterId);
		if (this.nowSortDir == MonsterDataMng.SORT_DIR.UPPER_2_LOWER)
		{
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return 0;
		}
		else
		{
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return 0;
		}
	}

	private int CompareRARE(MonsterData x, MonsterData y)
	{
		int num = int.Parse(x.monsterM.rare);
		int num2 = int.Parse(y.monsterM.rare);
		if (this.nowSortDir == MonsterDataMng.SORT_DIR.UPPER_2_LOWER)
		{
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
		else
		{
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
	}

	private int CompareLEVEL(MonsterData x, MonsterData y)
	{
		int num = (!(x.userMonster.eggFlg == "1")) ? int.Parse(x.userMonster.level) : -1;
		int num2 = (!(y.userMonster.eggFlg == "1")) ? int.Parse(y.userMonster.level) : -1;
		if (this.nowSortDir == MonsterDataMng.SORT_DIR.UPPER_2_LOWER)
		{
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return this.Compare_MID_RARE(x, y);
		}
		else
		{
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return this.Compare_MID_RARE(x, y);
		}
	}

	private int CompareHP(MonsterData x, MonsterData y)
	{
		int num = (!(x.userMonster.eggFlg == "1")) ? int.Parse(x.userMonster.hp) : -1;
		int num2 = (!(y.userMonster.eggFlg == "1")) ? int.Parse(y.userMonster.hp) : -1;
		if (this.nowSortDir == MonsterDataMng.SORT_DIR.UPPER_2_LOWER)
		{
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
		else
		{
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
	}

	private int CompareATK(MonsterData x, MonsterData y)
	{
		int num = (!(x.userMonster.eggFlg == "1")) ? int.Parse(x.userMonster.attack) : -1;
		int num2 = (!(y.userMonster.eggFlg == "1")) ? int.Parse(y.userMonster.attack) : -1;
		if (this.nowSortDir == MonsterDataMng.SORT_DIR.UPPER_2_LOWER)
		{
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
		else
		{
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
	}

	private int CompareDEF(MonsterData x, MonsterData y)
	{
		int num = (!(x.userMonster.eggFlg == "1")) ? int.Parse(x.userMonster.defense) : -1;
		int num2 = (!(y.userMonster.eggFlg == "1")) ? int.Parse(y.userMonster.defense) : -1;
		if (this.nowSortDir == MonsterDataMng.SORT_DIR.UPPER_2_LOWER)
		{
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
		else
		{
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
	}

	private int CompareS_ATK(MonsterData x, MonsterData y)
	{
		int num = (!(x.userMonster.eggFlg == "1")) ? int.Parse(x.userMonster.spAttack) : -1;
		int num2 = (!(y.userMonster.eggFlg == "1")) ? int.Parse(y.userMonster.spAttack) : -1;
		if (this.nowSortDir == MonsterDataMng.SORT_DIR.UPPER_2_LOWER)
		{
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
		else
		{
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
	}

	private int CompareS_DEF(MonsterData x, MonsterData y)
	{
		int num = (!(x.userMonster.eggFlg == "1")) ? int.Parse(x.userMonster.spDefense) : -1;
		int num2 = (!(y.userMonster.eggFlg == "1")) ? int.Parse(y.userMonster.spDefense) : -1;
		if (this.nowSortDir == MonsterDataMng.SORT_DIR.UPPER_2_LOWER)
		{
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
		else
		{
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
	}

	private int CompareSPD(MonsterData x, MonsterData y)
	{
		int num = (!(x.userMonster.eggFlg == "1")) ? x.Now_SPD(-1) : -1;
		int num2 = (!(y.userMonster.eggFlg == "1")) ? y.Now_SPD(-1) : -1;
		if (this.nowSortDir == MonsterDataMng.SORT_DIR.UPPER_2_LOWER)
		{
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
		else
		{
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
	}

	private int CompareLUCK(MonsterData x, MonsterData y)
	{
		int num = (!(x.userMonster.eggFlg == "1")) ? int.Parse(x.userMonster.luck) : -1;
		int num2 = (!(y.userMonster.eggFlg == "1")) ? int.Parse(y.userMonster.luck) : -1;
		if (this.nowSortDir == MonsterDataMng.SORT_DIR.UPPER_2_LOWER)
		{
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
		else
		{
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
	}

	private int CompareFRIENDSHIP(MonsterData x, MonsterData y)
	{
		int num = (!(x.userMonster.eggFlg == "1")) ? int.Parse(x.userMonster.friendship) : -1;
		int num2 = (!(y.userMonster.eggFlg == "1")) ? int.Parse(y.userMonster.friendship) : -1;
		if (this.nowSortDir == MonsterDataMng.SORT_DIR.UPPER_2_LOWER)
		{
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
		else
		{
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
	}

	private int CompareGRADE(MonsterData x, MonsterData y)
	{
		int num = (!(x.userMonster.eggFlg == "1")) ? this.GetGrowStepSortValue(x.monsterMG.growStep) : -1;
		int num2 = (!(y.userMonster.eggFlg == "1")) ? this.GetGrowStepSortValue(y.monsterMG.growStep) : -1;
		if (this.nowSortDir == MonsterDataMng.SORT_DIR.UPPER_2_LOWER)
		{
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
		else
		{
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
	}

	private int CompareTRIBE(MonsterData x, MonsterData y)
	{
		int num = (!(x.userMonster.eggFlg == "1")) ? int.Parse(x.monsterMG.tribe) : -1;
		int num2 = (!(y.userMonster.eggFlg == "1")) ? int.Parse(y.monsterMG.tribe) : -1;
		if (this.nowSortDir == MonsterDataMng.SORT_DIR.UPPER_2_LOWER)
		{
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
		else
		{
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return this.Compare_MID_LEV(x, y);
		}
	}

	private int Compare_MID_LEV(MonsterData x, MonsterData y)
	{
		int num = int.Parse(x.monsterM.monsterId);
		int num2 = int.Parse(y.monsterM.monsterId);
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		num = int.Parse(x.userMonster.level);
		num2 = int.Parse(y.userMonster.level);
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		num = int.Parse(x.userMonster.userMonsterId);
		num2 = int.Parse(y.userMonster.userMonsterId);
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	private int Compare_MID_RARE(MonsterData x, MonsterData y)
	{
		int num = int.Parse(x.monsterM.monsterId);
		int num2 = int.Parse(y.monsterM.monsterId);
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		num = int.Parse(x.monsterM.rare);
		num2 = int.Parse(y.monsterM.rare);
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		num = int.Parse(x.userMonster.userMonsterId);
		num2 = int.Parse(y.userMonster.userMonsterId);
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	private int GetGrowStepSortValue(string growStep)
	{
		switch (growStep.ToInt32())
		{
		case 1:
			return 0;
		case 2:
			return 1;
		case 3:
			return 2;
		case 4:
			return 3;
		case 5:
			return 4;
		case 6:
			return 6;
		case 7:
			return 8;
		case 8:
			return 5;
		case 9:
			return 7;
		default:
			return 0;
		}
	}

	public MonsterDataMng.SELECTION_TYPE NowSelectionType
	{
		get
		{
			return this.nowSelectionType;
		}
		set
		{
			this.nowSelectionType = value;
		}
	}

	public List<MonsterData> SelectionMDList(List<MonsterData> mdList)
	{
		if (this.nowSelectionType == MonsterDataMng.SELECTION_TYPE.NONE)
		{
			return mdList;
		}
		if ((this.nowSelectionType & MonsterDataMng.SELECTION_TYPE.LEADER_SKILL) > MonsterDataMng.SELECTION_TYPE.NONE)
		{
			mdList = this.SelectionLeaderSkill(mdList, true);
		}
		if ((this.nowSelectionType & MonsterDataMng.SELECTION_TYPE.ACTIVE_SUCCESS) > MonsterDataMng.SELECTION_TYPE.NONE)
		{
			mdList = this.SelectionActiveSuccess(mdList);
		}
		if ((this.nowSelectionType & MonsterDataMng.SELECTION_TYPE.PASSIV_SUCCESS) > MonsterDataMng.SELECTION_TYPE.NONE)
		{
			mdList = this.SelectionPassivSuccess(mdList);
		}
		if ((this.nowSelectionType & MonsterDataMng.SELECTION_TYPE.MEDAL) > MonsterDataMng.SELECTION_TYPE.NONE)
		{
			mdList = this.SelectionMedal(mdList, true);
		}
		if ((this.nowSelectionType & MonsterDataMng.SELECTION_TYPE.NO_LEADER_SKILL) > MonsterDataMng.SELECTION_TYPE.NONE)
		{
			mdList = this.SelectionLeaderSkill(mdList, false);
		}
		if ((this.nowSelectionType & MonsterDataMng.SELECTION_TYPE.NO_MEDAL) > MonsterDataMng.SELECTION_TYPE.NONE)
		{
			mdList = this.SelectionMedal(mdList, false);
		}
		return mdList;
	}

	private List<MonsterData> SelectionLeaderSkill(List<MonsterData> mdList, bool isPossession = true)
	{
		List<MonsterData> list = new List<MonsterData>();
		for (int i = 0; i < mdList.Count; i++)
		{
			if (isPossession)
			{
				if (mdList[i].leaderSkillM != null && mdList[i].userMonster.eggFlg == "0")
				{
					list.Add(mdList[i]);
				}
			}
			else if (mdList[i].leaderSkillM == null && mdList[i].userMonster.eggFlg == "0")
			{
				list.Add(mdList[i]);
			}
		}
		return list;
	}

	private List<MonsterData> SelectionActiveSuccess(List<MonsterData> mdList)
	{
		List<MonsterData> list = new List<MonsterData>();
		for (int i = 0; i < mdList.Count; i++)
		{
			if (mdList[i].commonSkillM != null && mdList[i].userMonster.eggFlg == "0")
			{
				for (int j = 0; j < ConstValue.SkillDetailM_effectType.Length; j++)
				{
					if (mdList[i].commonSkillDetailM.effectType == ConstValue.SkillDetailM_effectType[j])
					{
						list.Add(mdList[i]);
						break;
					}
				}
			}
		}
		return list;
	}

	private List<MonsterData> SelectionPassivSuccess(List<MonsterData> mdList)
	{
		List<MonsterData> list = new List<MonsterData>();
		for (int i = 0; i < mdList.Count; i++)
		{
			if (mdList[i].commonSkillM != null && mdList[i].userMonster.eggFlg == "0")
			{
				int j;
				for (j = 0; j < ConstValue.SkillDetailM_effectType.Length; j++)
				{
					if (mdList[i].commonSkillDetailM.effectType == ConstValue.SkillDetailM_effectType[j])
					{
						break;
					}
				}
				if (j == ConstValue.SkillDetailM_effectType.Length)
				{
					list.Add(mdList[i]);
				}
			}
		}
		return list;
	}

	private List<MonsterData> SelectionMedal(List<MonsterData> mdList, bool isPossession = true)
	{
		List<MonsterData> list = new List<MonsterData>();
		for (int i = 0; i < mdList.Count; i++)
		{
			if (mdList[i].CheckHaveMedal() == isPossession)
			{
				list.Add(mdList[i]);
			}
		}
		return list;
	}

	public int MyCluster { private get; set; }

	private List<MonsterData> SetSortLSMessage(List<MonsterData> monsterDataList, bool isEvolvePage = false)
	{
		if (this.monster_icon_list == null || this.monster_icon_list.Count <= 0)
		{
			return monsterDataList;
		}
		foreach (MonsterData monsterData in monsterDataList)
		{
			GUIMonsterIcon guimonsterIcon = this.monster_icon_list[monsterData.idx];
			if (!(guimonsterIcon == null))
			{
				if (monsterData.userMonster.IsEgg())
				{
					guimonsterIcon.LevelMess = string.Format(StringMaster.GetString("CharaIconLv"), StringMaster.GetString("CharaStatus-01"));
					guimonsterIcon.SetLevelMessageColor(new Color(1f, 1f, 1f, 1f));
				}
				else
				{
					int num = int.Parse(monsterData.userMonster.level);
					int num2 = int.Parse(monsterData.monsterM.maxLevel);
					if (num >= num2)
					{
						guimonsterIcon.LevelMess = string.Format(StringMaster.GetString("CharaIconLv"), StringMaster.GetString("CharaStatus-18"));
						guimonsterIcon.SetLevelMessageColor(new Color(1f, 0.9411765f, 0f, 1f));
					}
					else
					{
						guimonsterIcon.LevelMess = string.Format(StringMaster.GetString("CharaIconLv"), monsterData.userMonster.level);
						guimonsterIcon.SetLevelMessageColor(new Color(1f, 1f, 1f, 1f));
					}
				}
				guimonsterIcon.SetSortMessageColor(ConstValue.DIGIMON_GREEN);
				switch (this.nowSortType)
				{
				case MonsterDataMng.SORT_TYPE.DATE:
					guimonsterIcon.SortMess = string.Empty;
					if (isEvolvePage)
					{
						ClassSingleton<EvolutionData>.Instance.CheckEvolveable(guimonsterIcon, monsterData, false);
					}
					break;
				case MonsterDataMng.SORT_TYPE.RARE:
					guimonsterIcon.SortMess = string.Empty;
					if (isEvolvePage)
					{
						ClassSingleton<EvolutionData>.Instance.CheckEvolveable(guimonsterIcon, monsterData, true);
					}
					break;
				case MonsterDataMng.SORT_TYPE.LEVEL:
					guimonsterIcon.SortMess = string.Empty;
					if (isEvolvePage)
					{
						ClassSingleton<EvolutionData>.Instance.CheckEvolveable(guimonsterIcon, monsterData, false);
					}
					break;
				case MonsterDataMng.SORT_TYPE.HP:
					if (monsterData.userMonster.eggFlg == "1")
					{
						guimonsterIcon.SortMess = StringMaster.GetString("CharaStatus-01");
					}
					else
					{
						guimonsterIcon.SortMess = monsterData.userMonster.hp;
					}
					if (isEvolvePage)
					{
						ClassSingleton<EvolutionData>.Instance.CheckEvolveable(guimonsterIcon, monsterData, true);
					}
					break;
				case MonsterDataMng.SORT_TYPE.ATK:
					if (monsterData.userMonster.eggFlg == "1")
					{
						guimonsterIcon.SortMess = StringMaster.GetString("CharaStatus-01");
					}
					else
					{
						guimonsterIcon.SortMess = monsterData.userMonster.attack;
					}
					if (isEvolvePage)
					{
						ClassSingleton<EvolutionData>.Instance.CheckEvolveable(guimonsterIcon, monsterData, true);
					}
					break;
				case MonsterDataMng.SORT_TYPE.DEF:
					if (monsterData.userMonster.eggFlg == "1")
					{
						guimonsterIcon.SortMess = StringMaster.GetString("CharaStatus-01");
					}
					else
					{
						guimonsterIcon.SortMess = monsterData.userMonster.defense;
					}
					if (isEvolvePage)
					{
						ClassSingleton<EvolutionData>.Instance.CheckEvolveable(guimonsterIcon, monsterData, true);
					}
					break;
				case MonsterDataMng.SORT_TYPE.S_ATK:
					if (monsterData.userMonster.eggFlg == "1")
					{
						guimonsterIcon.SortMess = StringMaster.GetString("CharaStatus-01");
					}
					else
					{
						guimonsterIcon.SortMess = monsterData.userMonster.spAttack;
					}
					if (isEvolvePage)
					{
						ClassSingleton<EvolutionData>.Instance.CheckEvolveable(guimonsterIcon, monsterData, true);
					}
					break;
				case MonsterDataMng.SORT_TYPE.S_DEF:
					if (monsterData.userMonster.eggFlg == "1")
					{
						guimonsterIcon.SortMess = StringMaster.GetString("CharaStatus-01");
					}
					else
					{
						guimonsterIcon.SortMess = monsterData.userMonster.spDefense;
					}
					if (isEvolvePage)
					{
						ClassSingleton<EvolutionData>.Instance.CheckEvolveable(guimonsterIcon, monsterData, true);
					}
					break;
				case MonsterDataMng.SORT_TYPE.SPD:
					if (monsterData.userMonster.eggFlg == "1")
					{
						guimonsterIcon.SortMess = StringMaster.GetString("CharaStatus-01");
					}
					else
					{
						guimonsterIcon.SortMess = monsterData.userMonster.speed;
					}
					if (isEvolvePage)
					{
						ClassSingleton<EvolutionData>.Instance.CheckEvolveable(guimonsterIcon, monsterData, true);
					}
					break;
				case MonsterDataMng.SORT_TYPE.LUCK:
					if (monsterData.userMonster.eggFlg == "1")
					{
						guimonsterIcon.SortMess = StringMaster.GetString("CharaStatus-01");
					}
					else
					{
						guimonsterIcon.SortMess = monsterData.userMonster.luck;
					}
					if (isEvolvePage)
					{
						ClassSingleton<EvolutionData>.Instance.CheckEvolveable(guimonsterIcon, monsterData, true);
					}
					break;
				case MonsterDataMng.SORT_TYPE.GRADE:
					guimonsterIcon.SortMess = monsterData.growStepM.monsterGrowStepName;
					if (monsterData.userMonster.eggFlg == "1")
					{
						guimonsterIcon.SortMess = StringMaster.GetString("CharaStatus-04");
					}
					if (isEvolvePage)
					{
						ClassSingleton<EvolutionData>.Instance.CheckEvolveable(guimonsterIcon, monsterData, true);
					}
					break;
				case MonsterDataMng.SORT_TYPE.TRIBE:
					if (monsterData.userMonster.eggFlg == "1")
					{
						guimonsterIcon.SortMess = StringMaster.GetString("CharaStatus-01");
					}
					else
					{
						guimonsterIcon.SortMess = monsterData.tribeM.monsterTribeName;
					}
					if (isEvolvePage)
					{
						ClassSingleton<EvolutionData>.Instance.CheckEvolveable(guimonsterIcon, monsterData, true);
					}
					break;
				}
			}
		}
		return monsterDataList;
	}

	public string GetSortName()
	{
		string result = string.Empty;
		switch (this.nowSortType)
		{
		case MonsterDataMng.SORT_TYPE.DATE:
			result = StringMaster.GetString("Sort-13");
			break;
		case MonsterDataMng.SORT_TYPE.RARE:
			result = StringMaster.GetString("Sort-10");
			break;
		case MonsterDataMng.SORT_TYPE.LEVEL:
			result = StringMaster.GetString("Sort-05");
			break;
		case MonsterDataMng.SORT_TYPE.HP:
			result = StringMaster.GetString("Sort-04");
			break;
		case MonsterDataMng.SORT_TYPE.ATK:
			result = StringMaster.GetString("Sort-02");
			break;
		case MonsterDataMng.SORT_TYPE.DEF:
			result = StringMaster.GetString("Sort-03");
			break;
		case MonsterDataMng.SORT_TYPE.S_ATK:
			result = StringMaster.GetString("Sort-06");
			break;
		case MonsterDataMng.SORT_TYPE.S_DEF:
			result = StringMaster.GetString("Sort-07");
			break;
		case MonsterDataMng.SORT_TYPE.SPD:
			result = StringMaster.GetString("Sort-08");
			break;
		case MonsterDataMng.SORT_TYPE.LUCK:
			result = StringMaster.GetString("Sort-09");
			break;
		case MonsterDataMng.SORT_TYPE.GRADE:
			result = StringMaster.GetString("Sort-12");
			break;
		case MonsterDataMng.SORT_TYPE.TRIBE:
			result = StringMaster.GetString("Sort-11");
			break;
		case MonsterDataMng.SORT_TYPE.FRIENDSHIP:
			result = StringMaster.GetString("Sort-14");
			break;
		}
		return result;
	}

	public GUIMonsterIcon MakePrefabByMonsterData(MonsterData monsterData, Vector3 vScl, Vector3 vPos, Transform parent = null, bool initIconState = true, bool readTexByASync = false)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.goMONSTER_ICON_M);
		gameObject.SetActive(true);
		Transform transform = gameObject.transform;
		if (parent == null)
		{
			transform.parent = this.goMONSTER_ICON_BAK_ROOT.transform;
		}
		else
		{
			transform.parent = parent;
		}
		transform.localScale = vScl;
		transform.localPosition = vPos;
		GUIMonsterIcon component = gameObject.GetComponent<GUIMonsterIcon>();
		component.ReadTexByASync = readTexByASync;
		component.Data = monsterData;
		if (initIconState)
		{
			component.DimmLevel = GUIMonsterIcon.DIMM_LEVEL.ACTIVE;
			component.SelectNum = -1;
			component.DimmMess = string.Empty;
			component.SortMess = string.Empty;
			component.New = monsterData.New;
		}
		return component;
	}

	public void RefreshPrefabByMonsterData(MonsterData monsterData, GUIMonsterIcon monsterIcon, bool initIconState = true, bool readTexByASync = false)
	{
		monsterIcon.ReadTexByASync = readTexByASync;
		monsterIcon.Data = monsterData;
		if (initIconState)
		{
			monsterIcon.DimmLevel = GUIMonsterIcon.DIMM_LEVEL.ACTIVE;
			monsterIcon.SelectNum = -1;
			monsterIcon.DimmMess = string.Empty;
			monsterIcon.SortMess = string.Empty;
		}
	}

	public GUIMonsterIcon MakeQuestionPrefab(Vector3 vScl, Vector3 vPos, int depth, Transform parent)
	{
		GameObject gameObject = GUIManager.LoadCommonGUI("ListParts/ListPartsThumbnail", null);
		gameObject.SetActive(true);
		GUIMonsterIcon component = gameObject.GetComponent<GUIMonsterIcon>();
		component.transform.parent = parent;
		component.transform.localScale = vScl;
		component.transform.localPosition = vPos;
		DepthController.SetWidgetDepth_2(component.transform, depth);
		component.Data = new MonsterData
		{
			monsterM = new GameWebAPI.RespDataMA_GetMonsterMS.MonsterM(),
			monsterMG = new GameWebAPI.RespDataMA_GetMonsterMG.MonsterM(),
			monsterMG = 
			{
				growStep = "-2"
			},
			monsterM = 
			{
				iconId = string.Empty,
				rare = "-2"
			}
		};
		return component;
	}

	public void SetDimmAll(GUIMonsterIcon.DIMM_LEVEL level = GUIMonsterIcon.DIMM_LEVEL.ACTIVE)
	{
		for (int i = 0; i < this.monster_data_list.Count; i++)
		{
			this.monster_data_list[i].dimmLevel = level;
		}
	}

	public void SetDimmByMonsterDataList(List<MonsterData> mdlist, GUIMonsterIcon.DIMM_LEVEL level, MonsterData orgMD = null)
	{
		for (int i = 0; i < mdlist.Count; i++)
		{
			if (mdlist[i] != null)
			{
				mdlist[i].dimmLevel = level;
				if (orgMD == null)
				{
					mdlist[i].dimmMess = StringMaster.GetString("CharaIcon-04");
				}
				else if (orgMD.userMonster.userMonsterId != mdlist[i].userMonster.userMonsterId)
				{
					mdlist[i].dimmLevel = GUIMonsterIcon.DIMM_LEVEL.ACTIVE;
					mdlist[i].dimmMess = string.Empty;
				}
				else
				{
					mdlist[i].dimmMess = StringMaster.GetString("CharaIcon-04");
				}
			}
		}
	}

	public void SetSelectOffAll()
	{
		for (int i = 0; i < this.monster_data_list.Count; i++)
		{
			this.monster_data_list[i].selectNum = -1;
		}
	}

	public void ClearDimmMessAll()
	{
		for (int i = 0; i < this.monster_data_list.Count; i++)
		{
			this.monster_data_list[i].dimmMess = string.Empty;
		}
	}

	public void ClearSortMessAll()
	{
		for (int i = 0; i < this.monster_data_list.Count; i++)
		{
			this.monster_data_list[i].sortMess = string.Empty;
		}
	}

	public void ClearLevelMessAll()
	{
		for (int i = 0; i < this.monster_data_list.Count; i++)
		{
			this.monster_data_list[i].levelMess = string.Empty;
		}
	}

	public void SetSelectByMonsterDataList(List<MonsterData> mdlist, int snum, bool refresh = false)
	{
		for (int i = 0; i < mdlist.Count; i++)
		{
			if (mdlist[i] != null)
			{
				mdlist[i].selectNum = snum;
				snum++;
				if (refresh)
				{
					this.monster_icon_list[mdlist[i].idx].SelectNum = mdlist[i].selectNum;
				}
			}
		}
	}

	public void UnnewMonserDataList()
	{
		foreach (MonsterData monsterData in this.monster_data_list)
		{
			monsterData.New = false;
		}
	}

	public APIRequestTask GetPicturebookData(string userId, bool requestRetry = true)
	{
		int uid = 0;
		if (!int.TryParse(userId, out uid))
		{
			return null;
		}
		GameWebAPI.RequestFA_MN_PicturebookExec requestFA_MN_PicturebookExec = new GameWebAPI.RequestFA_MN_PicturebookExec();
		requestFA_MN_PicturebookExec.SetSendData = delegate(GameWebAPI.MN_Req_Picturebook param)
		{
			param.targetUserId = uid;
		};
		requestFA_MN_PicturebookExec.OnReceived = delegate(GameWebAPI.RespDataMN_Picturebook response)
		{
			MonsterDataMng.userPicturebookData = response;
		};
		GameWebAPI.RequestFA_MN_PicturebookExec request = requestFA_MN_PicturebookExec;
		return new APIRequestTask(request, requestRetry);
	}

	public void DispArousal(int rare_i, GameObject goArousal, UISprite spArousal)
	{
		if (2 <= rare_i)
		{
			goArousal.SetActive(true);
			spArousal.spriteName = MonsterDetailUtil.GetArousalSpriteName(rare_i);
		}
		else
		{
			goArousal.SetActive(false);
		}
	}

	public void DispArousal2(int rare_i, GameObject goArousal, GameObject goArousalNone, UISprite spArousal)
	{
		if (2 <= rare_i)
		{
			goArousal.SetActive(true);
			goArousalNone.SetActive(false);
			spArousal.spriteName = MonsterDetailUtil.GetArousalSpriteName(rare_i);
		}
		else
		{
			goArousal.SetActive(false);
			goArousalNone.SetActive(true);
		}
	}

	public bool HasGrowStepHigh(List<MonsterData> mdL)
	{
		for (int i = 0; i < mdL.Count; i++)
		{
			if (mdL[i].IsGrowStepHigh())
			{
				return true;
			}
		}
		return false;
	}

	public bool HasArousal(List<MonsterData> mdL)
	{
		for (int i = 0; i < mdL.Count; i++)
		{
			if (mdL[i].IsArousal())
			{
				return true;
			}
		}
		return false;
	}

	public bool HasChip(List<MonsterData> mdL)
	{
		for (int i = 0; i < mdL.Count; i++)
		{
			if (mdL[i].IsAttachedChip())
			{
				return true;
			}
		}
		return false;
	}

	public void RefreshUserMonsterByUserMonsterList(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] umL)
	{
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] userMonsterList = DataMng.Instance().RespDataUS_MonsterList.userMonsterList;
		for (int i = 0; i < userMonsterList.Length; i++)
		{
			for (int j = 0; j < umL.Length; j++)
			{
				if (userMonsterList[i].userMonsterId == umL[j].userMonsterId)
				{
					userMonsterList[i] = umL[j];
				}
			}
		}
	}

	public enum SELECT_TYPE
	{
		ALL,
		GROWING_IN_GARDEN,
		ALL_OUT_GARDEN,
		RESEARCH_TARGET,
		CAN_EVOLVE,
		ALL_IN_GARDEN
	}

	public enum SORT_TYPE
	{
		DATE,
		RARE,
		LEVEL,
		HP,
		ATK,
		DEF,
		S_ATK,
		S_DEF,
		SPD,
		LUCK,
		GRADE,
		TRIBE,
		FRIENDSHIP
	}

	public enum SORT_DIR
	{
		NONE = -1,
		UPPER_2_LOWER,
		LOWER_2_UPPER
	}

	public enum SELECTION_TYPE
	{
		NONE,
		LEADER_SKILL,
		ACTIVE_SUCCESS,
		PASSIV_SUCCESS = 4,
		MEDAL = 8,
		NO_LEADER_SKILL = 16,
		NO_MEDAL = 32
	}
}
