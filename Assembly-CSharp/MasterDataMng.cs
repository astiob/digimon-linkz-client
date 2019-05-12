using Evolution;
using Master;
using Picturebook;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebAPIRequest;

public sealed class MasterDataMng : MonoBehaviour
{
	private static MasterDataMng instance;

	private Dictionary<MasterId, MasterBase> masterList = new Dictionary<MasterId, MasterBase>();

	private MasterDataFileIO fileIO;

	private int downloadMasterDataNum;

	private int downloadedMasterDataCount;

	private TutorialMaster tutorialMaster = new TutorialMaster();

	public GameWebAPI.RespDataCM_MDVersion MasterDataVersion { get; set; }

	public static MasterDataMng Instance()
	{
		return MasterDataMng.instance;
	}

	private void Awake()
	{
		MasterDataMng.instance = this;
		this.CreateMasterData<MA_AssetCategoryMaster>();
		this.CreateMasterData<MA_MonsterMasterWithoutGroupData>();
		this.CreateMasterData<MA_MonsterMasterOnlyGroupData>();
		this.CreateMasterData<MA_MonsterEvolutionMaster>();
		this.CreateMasterData<MA_MonsterExperienceMaster>();
		this.CreateMasterData<MA_MonsterResistanceMaster>();
		this.CreateMasterData<MA_SkillMaster>();
		this.CreateMasterData<MA_SkillDetailMaster>();
		this.CreateMasterData<MA_MonsterEvolutionRouteMaster>();
		this.CreateMasterData<MA_MonsterTribeMaster>();
		this.CreateMasterData<MA_MonsterGrowStepMaster>();
		this.CreateMasterData<MA_MonsterTranceMaster>();
		this.CreateMasterData<MA_MonsterTribeTranceMaster>();
		this.CreateMasterData<MA_MonsterEvolutionMaterialMaster>();
		this.CreateMasterData<MA_SoulMaster>();
		this.CreateMasterData<MA_FacilityMaster>();
		this.CreateMasterData<MA_FacilityKeyMaster>();
		this.CreateMasterData<MA_FacilityUpgradeMaster>();
		this.CreateMasterData<MA_MonsterFixedMaster>();
		this.CreateMasterData<MA_FacilityMeatFieldMaster>();
		this.CreateMasterData<MA_FacilityChipMaster>();
		this.CreateMasterData<MA_FacilityWarehouseMaster>();
		this.CreateMasterData<MA_FacilityExpUpMaster>();
		this.CreateMasterData<MA_FacilityRestaurantMaster>();
		this.CreateMasterData<MA_FacilityHouseMaster>();
		this.CreateMasterData<MA_FacilityConditionMaster>();
		this.CreateMasterData<MA_FacilityExtraEffectMaster>();
		this.CreateMasterData<MA_FacilityExtraEffectLevelMaster>();
		this.CreateMasterData<MA_TipsMaster>();
		this.CreateMasterData<MA_CodeMaster>();
		this.CreateMasterData<MA_MessageMaster>();
		this.CreateMasterData<MA_WorldMaster>();
		this.CreateMasterData<MA_WorldAreaMaster>();
		this.CreateMasterData<MA_WorldStageMaster>();
		this.CreateMasterData<MA_WorldStageRewardMaster>();
		this.CreateMasterData<MA_WorldStageOptionRewardMaster>();
		this.CreateMasterData<MA_WorldDungeonSortieLimitMaster>();
		this.CreateMasterData<MA_WorldDungeonExtraEffectMaster>();
		this.CreateMasterData<MA_WorldDungeonExtraEffectManageMaster>();
		this.CreateMasterData<MA_HelpCategoryMaster>();
		this.CreateMasterData<MA_HelpMaster>();
		this.CreateMasterData<MA_ItemMaster>();
		this.CreateMasterData<MA_TutorialNaviMaster>();
		this.CreateMasterData<MA_ColosseumMaster>();
		this.CreateMasterData<MA_ColosseumTimeScheduleMaster>();
		this.CreateMasterData<MA_ColosseumRankMaster>();
		this.CreateMasterData<MA_ChipMaster>();
		this.CreateMasterData<MA_ChipEffectMaster>();
		this.CreateMasterData<MA_MessageStringMaster>();
		this.CreateMasterData<MA_EventPointAchieveRewardMaster>();
		this.CreateMasterData<MA_EventPointBonusMaster>();
		this.CreateMasterData<MA_WorldEventAreaMaster>();
		this.CreateMasterData<MA_WorldEventMaster>();
		this.CreateMasterData<MA_DungeonTicketMaster>();
		this.CreateMasterData<MA_WorldDungeonStartCondition>();
		this.CreateMasterData<MA_NavigationMessageMaster>();
		this.CreateMasterData<MA_TitleMaster>();
		this.CreateMasterData<MA_AbilityUpgradeM>();
		this.CreateMasterData<MA_MonsterArousalMaster>();
		this.CreateMasterData<MA_MonsterSpecificTypeMaster>();
		this.CreateMasterData<MA_MonsterStatusAilmentMaster>();
		this.CreateMasterData<MA_MonsterStatusAilmentGroupMaster>();
		this.CreateMasterData<MA_WorldDungeonAdventureSceneMaster>();
		this.CreateMasterData<MA_MonsterIntegrationGroupMaster>();
	}

	private void CreateMasterData<MasterT>() where MasterT : MasterBase, new()
	{
		MasterT masterT = Activator.CreateInstance<MasterT>();
		this.masterList.Add(masterT.ID, masterT);
	}

	public MasterBase GetMaster(MasterId id)
	{
		global::Debug.Assert(Enum.IsDefined(typeof(MasterId), id), "存在しないマスターデータを取得しようとしました");
		MasterBase masterBase = null;
		this.masterList.TryGetValue(id, out masterBase);
		global::Debug.Assert(null != masterBase, "作成されていないマスターデータを取得しようとしました");
		return masterBase;
	}

	public GameWebAPI.RespDataMA_GetAssetCategoryM RespDataMA_AssetCategoryM
	{
		get
		{
			MA_AssetCategoryMaster ma_AssetCategoryMaster = this.GetMaster(MasterId.ASSET_CATEGORY) as MA_AssetCategoryMaster;
			return ma_AssetCategoryMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetMonsterMS RespDataMA_MonsterMS
	{
		get
		{
			MA_MonsterMasterWithoutGroupData ma_MonsterMasterWithoutGroupData = this.GetMaster(MasterId.MONSTERS) as MA_MonsterMasterWithoutGroupData;
			return ma_MonsterMasterWithoutGroupData.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetMonsterMG RespDataMA_MonsterMG
	{
		get
		{
			MA_MonsterMasterOnlyGroupData ma_MonsterMasterOnlyGroupData = this.GetMaster(MasterId.MONSTERG) as MA_MonsterMasterOnlyGroupData;
			return ma_MonsterMasterOnlyGroupData.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetMonsterEvolutionM RespDataMA_MonsterEvolutionM
	{
		get
		{
			MA_MonsterEvolutionMaster ma_MonsterEvolutionMaster = this.GetMaster(MasterId.MONSTER_EVOLUTION) as MA_MonsterEvolutionMaster;
			return ma_MonsterEvolutionMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetMonsterExperienceM RespDataMA_MonsterExperienceM
	{
		get
		{
			MA_MonsterExperienceMaster ma_MonsterExperienceMaster = this.GetMaster(MasterId.MONSTER_EXPERIENCE) as MA_MonsterExperienceMaster;
			return ma_MonsterExperienceMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetMonsterResistanceM RespDataMA_MonsterResistanceM
	{
		get
		{
			MA_MonsterResistanceMaster ma_MonsterResistanceMaster = this.GetMaster(MasterId.RESISTANCE) as MA_MonsterResistanceMaster;
			return ma_MonsterResistanceMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetSkillM RespDataMA_SkillM
	{
		get
		{
			MA_SkillMaster ma_SkillMaster = this.GetMaster(MasterId.SKILL) as MA_SkillMaster;
			return ma_SkillMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetSkillDetailM RespDataMA_SkillDetailM
	{
		get
		{
			MA_SkillDetailMaster ma_SkillDetailMaster = this.GetMaster(MasterId.SKILL_DETAIL) as MA_SkillDetailMaster;
			return ma_SkillDetailMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM RespDataMA_MonsterEvolutionRouteM
	{
		get
		{
			MA_MonsterEvolutionRouteMaster ma_MonsterEvolutionRouteMaster = this.GetMaster(MasterId.MONSTER_EVOLUTION_ROUTE) as MA_MonsterEvolutionRouteMaster;
			return ma_MonsterEvolutionRouteMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetMonsterTribeM RespDataMA_MonsterTribeM
	{
		get
		{
			MA_MonsterTribeMaster ma_MonsterTribeMaster = this.GetMaster(MasterId.TRIBE) as MA_MonsterTribeMaster;
			return ma_MonsterTribeMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetMonsterGrowStepM RespDataMA_MonsterGrowStepM
	{
		get
		{
			MA_MonsterGrowStepMaster ma_MonsterGrowStepMaster = this.GetMaster(MasterId.GROWSTEP) as MA_MonsterGrowStepMaster;
			return ma_MonsterGrowStepMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetMonsterTranceM RespDataMA_MonsterTranceM
	{
		get
		{
			MA_MonsterTranceMaster ma_MonsterTranceMaster = this.GetMaster(MasterId.TRANCE) as MA_MonsterTranceMaster;
			return ma_MonsterTranceMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetMonsterTribeTranceM RespDataMA_MonsterTribeTranceM
	{
		get
		{
			MA_MonsterTribeTranceMaster ma_MonsterTribeTranceMaster = this.GetMaster(MasterId.TRIBE_TRANCE) as MA_MonsterTribeTranceMaster;
			return ma_MonsterTribeTranceMaster.GetMasterData();
		}
	}

	public GameWebAPI.MonsterEvolutionMaterialMaster MonsterEvolutionMaterialMaster
	{
		get
		{
			MA_MonsterEvolutionMaterialMaster ma_MonsterEvolutionMaterialMaster = this.GetMaster(MasterId.MONSTER_EVOLUTION_MATERIAL) as MA_MonsterEvolutionMaterialMaster;
			return ma_MonsterEvolutionMaterialMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetSoulM RespDataMA_SoulM
	{
		get
		{
			MA_SoulMaster ma_SoulMaster = this.GetMaster(MasterId.SOUL) as MA_SoulMaster;
			return ma_SoulMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetTipsM RespDataMA_TipsM
	{
		get
		{
			MA_TipsMaster ma_TipsMaster = this.GetMaster(MasterId.TIPS) as MA_TipsMaster;
			return ma_TipsMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_CodeM RespDataMA_CodeM
	{
		get
		{
			MA_CodeMaster ma_CodeMaster = this.GetMaster(MasterId.CODE_MASTER) as MA_CodeMaster;
			return ma_CodeMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_MessageM RespDataMA_MessageM
	{
		get
		{
			MA_MessageMaster ma_MessageMaster = this.GetMaster(MasterId.MESSAGE_MASTER) as MA_MessageMaster;
			return ma_MessageMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetWorldAreaM RespDataMA_WorldAreaM
	{
		get
		{
			MA_WorldMaster ma_WorldMaster = this.GetMaster(MasterId.WORLD_AREA) as MA_WorldMaster;
			return ma_WorldMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetWorldStageM RespDataMA_WorldStageM
	{
		get
		{
			MA_WorldAreaMaster ma_WorldAreaMaster = this.GetMaster(MasterId.WORLD_STAGE) as MA_WorldAreaMaster;
			return ma_WorldAreaMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetWorldDungeonM RespDataMA_WorldDungeonM
	{
		get
		{
			MA_WorldStageMaster ma_WorldStageMaster = this.GetMaster(MasterId.WORLD_DNG) as MA_WorldStageMaster;
			return ma_WorldStageMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_WorldDungeonStartCondition RespDataMA_WorldDungeonStartCondition
	{
		get
		{
			MA_WorldDungeonStartCondition ma_WorldDungeonStartCondition = this.GetMaster(MasterId.WORLD_DUNGEON_START_CONDITION) as MA_WorldDungeonStartCondition;
			return ma_WorldDungeonStartCondition.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM RespDataMA_WorldDungeonExtraEffectM
	{
		get
		{
			MA_WorldDungeonExtraEffectMaster ma_WorldDungeonExtraEffectMaster = this.GetMaster(MasterId.WORLD_DUNGEON_EXTRA_EFFECT) as MA_WorldDungeonExtraEffectMaster;
			return ma_WorldDungeonExtraEffectMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectManageM RespDataMA_WorldDungeonExtraEffectManageM
	{
		get
		{
			MA_WorldDungeonExtraEffectManageMaster ma_WorldDungeonExtraEffectManageMaster = this.GetMaster(MasterId.WORLD_DUNGEON_EXTRA_EFFECT_MANAGE) as MA_WorldDungeonExtraEffectManageMaster;
			return ma_WorldDungeonExtraEffectManageMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetWorldDungeonRewardM RespDataMA_WorldDungeonRewardM
	{
		get
		{
			MA_WorldStageRewardMaster ma_WorldStageRewardMaster = this.GetMaster(MasterId.WORLD_DUNGEON_REWARD) as MA_WorldStageRewardMaster;
			return ma_WorldStageRewardMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetWorldDungeonOptionRewardM RespDataMA_WorldDungeonOptionRewardM
	{
		get
		{
			MA_WorldStageOptionRewardMaster ma_WorldStageOptionRewardMaster = this.GetMaster(MasterId.WORLD_DUNGEON_OPTION_REWARD) as MA_WorldStageOptionRewardMaster;
			return ma_WorldStageOptionRewardMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_WorldDungeonSortieLimit WorldDungeonSortieLimitMaster
	{
		get
		{
			MA_WorldDungeonSortieLimitMaster ma_WorldDungeonSortieLimitMaster = this.GetMaster(MasterId.WORLD_DUNGEON_SORTIE_LIMIT) as MA_WorldDungeonSortieLimitMaster;
			return ma_WorldDungeonSortieLimitMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetHelpCategoryM RespDataMA_HelpCategoryM
	{
		get
		{
			MA_HelpCategoryMaster ma_HelpCategoryMaster = this.GetMaster(MasterId.HELP_CATEGORY) as MA_HelpCategoryMaster;
			return ma_HelpCategoryMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetHelpM RespDataMA_HelpM
	{
		get
		{
			MA_HelpMaster ma_HelpMaster = this.GetMaster(MasterId.HELP) as MA_HelpMaster;
			return ma_HelpMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_GetItemM RespDataMA_ItemM
	{
		get
		{
			MA_ItemMaster ma_ItemMaster = this.GetMaster(MasterId.ITEM) as MA_ItemMaster;
			return ma_ItemMaster.GetMasterData();
		}
	}

	public TutorialMaster Tutorial
	{
		get
		{
			return this.tutorialMaster;
		}
	}

	public GameWebAPI.RespDataMA_ColosseumM RespDataMA_ColosseumMaster
	{
		get
		{
			MA_ColosseumMaster ma_ColosseumMaster = this.GetMaster(MasterId.COLOSSEUM) as MA_ColosseumMaster;
			return ma_ColosseumMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_ColosseumTimeScheduleM RespDataMA_ColosseumTimeScheduleMaster
	{
		get
		{
			MA_ColosseumTimeScheduleMaster ma_ColosseumTimeScheduleMaster = this.GetMaster(MasterId.COLOSSEUM_TIME_SCHEDULE) as MA_ColosseumTimeScheduleMaster;
			return ma_ColosseumTimeScheduleMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_ColosseumRankM RespDataMA_ColosseumRankMaster
	{
		get
		{
			MA_ColosseumRankMaster ma_ColosseumRankMaster = this.GetMaster(MasterId.COLOSSEUM_RANK) as MA_ColosseumRankMaster;
			return ma_ColosseumRankMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_ChipM RespDataMA_ChipMaster
	{
		get
		{
			MA_ChipMaster ma_ChipMaster = this.GetMaster(MasterId.CHIP) as MA_ChipMaster;
			return ma_ChipMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_ChipEffectM RespDataMA_ChipEffectMaster
	{
		get
		{
			MA_ChipEffectMaster ma_ChipEffectMaster = this.GetMaster(MasterId.CHIP_EFFECT) as MA_ChipEffectMaster;
			return ma_ChipEffectMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_MessageStringM RespDataMA_MessageStringMaster
	{
		get
		{
			MA_MessageStringMaster ma_MessageStringMaster = this.GetMaster(MasterId.MESSAGE_STRING_MASTER) as MA_MessageStringMaster;
			return ma_MessageStringMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_EventPointAchieveRewardM RespDataMA_EventPointAchieveRewardMaster
	{
		get
		{
			MA_EventPointAchieveRewardMaster ma_EventPointAchieveRewardMaster = this.GetMaster(MasterId.EVENT_POINT_REWARD) as MA_EventPointAchieveRewardMaster;
			return ma_EventPointAchieveRewardMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_EventPointBonusM RespDataMA_EventPointBonusMaster
	{
		get
		{
			MA_EventPointBonusMaster ma_EventPointBonusMaster = this.GetMaster(MasterId.EVENT_POINT_BONUS) as MA_EventPointBonusMaster;
			return ma_EventPointBonusMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_WorldEventAreaMaster RespDataMA_WorldEventAreaMaster
	{
		get
		{
			MA_WorldEventAreaMaster ma_WorldEventAreaMaster = this.GetMaster(MasterId.WORLD_EVENT_AREA) as MA_WorldEventAreaMaster;
			return ma_WorldEventAreaMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_WorldEventMaster RespDataMA_WorldEventMaster
	{
		get
		{
			MA_WorldEventMaster ma_WorldEventMaster = this.GetMaster(MasterId.WORLD_EVENT) as MA_WorldEventMaster;
			return ma_WorldEventMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_DungeonTicketMaster RespDataMA_DungeonTicketMaster
	{
		get
		{
			MA_DungeonTicketMaster ma_DungeonTicketMaster = this.GetMaster(MasterId.DUNGEON_TICKET) as MA_DungeonTicketMaster;
			return ma_DungeonTicketMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_NavigationMessageMaster NavigationMessageMaster
	{
		get
		{
			MA_NavigationMessageMaster ma_NavigationMessageMaster = this.GetMaster(MasterId.NAVIGATION_MESSAGE) as MA_NavigationMessageMaster;
			return ma_NavigationMessageMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_TitleMaster RespDataMA_TitleMaster
	{
		get
		{
			MA_TitleMaster ma_TitleMaster = this.GetMaster(MasterId.TITLE) as MA_TitleMaster;
			return ma_TitleMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_AbilityUpgradeM RespDataMA_AbilityUpgradeM
	{
		get
		{
			MA_AbilityUpgradeM ma_AbilityUpgradeM = this.GetMaster(MasterId.ABILITY_MEDAL_UPGRADE) as MA_AbilityUpgradeM;
			return ma_AbilityUpgradeM.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_MonsterArousalMaster ResponseMonsterArousalMaster
	{
		get
		{
			MA_MonsterArousalMaster ma_MonsterArousalMaster = this.GetMaster(MasterId.MONSTER_AROUSAL) as MA_MonsterArousalMaster;
			return ma_MonsterArousalMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_MonsterSpecificTypeMaster ResponseMonsterSpecificTypeMaster
	{
		get
		{
			MA_MonsterSpecificTypeMaster ma_MonsterSpecificTypeMaster = this.GetMaster(MasterId.MONSTER_SPECIFIC_TYPE) as MA_MonsterSpecificTypeMaster;
			return ma_MonsterSpecificTypeMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_MonsterStatusAilmentMaster ResponseMonsterStatusAilmentMaster
	{
		get
		{
			MA_MonsterStatusAilmentMaster ma_MonsterStatusAilmentMaster = this.GetMaster(MasterId.MONSTER_STATUS_AILMENT) as MA_MonsterStatusAilmentMaster;
			return ma_MonsterStatusAilmentMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_MonsterStatusAilmentGroupMaster ResponseMonsterStatusAilmentGroupMaster
	{
		get
		{
			MA_MonsterStatusAilmentGroupMaster ma_MonsterStatusAilmentGroupMaster = this.GetMaster(MasterId.MONSTER_STATUS_AILMENT_MATERIAL) as MA_MonsterStatusAilmentGroupMaster;
			return ma_MonsterStatusAilmentGroupMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster ResponseWorldDungeonAdventureSceneMaster
	{
		get
		{
			MA_WorldDungeonAdventureSceneMaster ma_WorldDungeonAdventureSceneMaster = this.GetMaster(MasterId.WORLD_DUNGEON_ADVENTURE_SCENE) as MA_WorldDungeonAdventureSceneMaster;
			return ma_WorldDungeonAdventureSceneMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster ResponseMonsterIntegrationGroupMaster
	{
		get
		{
			MA_MonsterIntegrationGroupMaster ma_MonsterIntegrationGroupMaster = this.GetMaster(MasterId.MONSTER_INTEGRATION_GROUP) as MA_MonsterIntegrationGroupMaster;
			return ma_MonsterIntegrationGroupMaster.GetMasterData();
		}
	}

	public GameWebAPI.RespDataMA_MonsterFixedM ResponseMonsterFixedMaster
	{
		get
		{
			MA_MonsterFixedMaster ma_MonsterFixedMaster = this.GetMaster(MasterId.MONSTER_FIXED) as MA_MonsterFixedMaster;
			return ma_MonsterFixedMaster.GetMasterData();
		}
	}

	public void ClearCache()
	{
		AlertMaster.ClearCache();
		StringMaster.ClearCache();
		EvolutionMaterialData.ClearCache();
		ChipDataMng.ClearCache();
		TitleDataMng.ClearCache();
		DataMng.Instance().StageGimmick.ZeroClear();
		MasterBase[] array = this.masterList.Values.ToArray<MasterBase>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ClearData();
		}
		MonsterPicturebookData.Initialize();
	}

	public void InitialFileIO()
	{
		this.fileIO = new MasterDataFileIO();
	}

	public void ReleaseFileIO()
	{
		this.fileIO = null;
	}

	public IEnumerator ReadMasterData(List<MasterId> updateMasterDataList, Action<int, int> loadProgress)
	{
		MasterBase[] list = this.masterList.Values.ToArray<MasterBase>();
		int masterDataNum = list.Length;
		loadProgress(0, masterDataNum);
		for (int i = 0; i < masterDataNum; i++)
		{
			yield return base.StartCoroutine(this.fileIO.ReadMasterDataFile(list[i]));
			if (list[i].GetData() == null)
			{
				updateMasterDataList.Add(list[i].ID);
			}
			loadProgress(i, masterDataNum);
		}
		loadProgress(1, 1);
		yield break;
	}

	public IEnumerator GetMasterDataUpdateInfo(List<MasterId> destUpdateInfoList)
	{
		MasterDataVersionInfo versionInfo = new MasterDataVersionInfo();
		yield return base.StartCoroutine(MasterDataVersionUpCheck.GetMasterDataVersion(this, this.fileIO, versionInfo));
		List<string> tableNameList = MasterDataVersionUpCheck.GetVersionUpMasterDataTableNameList(versionInfo);
		List<MasterId> masterIdList = MasterDataVersionUpCheck.GetVersionUpMasterIdList(tableNameList, this.masterList);
		MasterDataVersionUpCheck.AddVersionUpMasterId(masterIdList, destUpdateInfoList);
		this.MasterDataVersion = versionInfo.serverVersion;
		yield break;
	}

	public IEnumerator UpdateLocalMasterData(List<MasterId> updateInfoList)
	{
		this.downloadedMasterDataCount = 0;
		this.downloadMasterDataNum = updateInfoList.Count * 2 + 1;
		yield return base.StartCoroutine(this.RequestMasterData(updateInfoList));
		for (int i = 0; i < updateInfoList.Count; i++)
		{
			MasterBase masterData = this.GetMaster(updateInfoList[i]);
			yield return base.StartCoroutine(this.fileIO.WriteMasterDataFile(masterData));
			this.downloadedMasterDataCount++;
		}
		yield return base.StartCoroutine(this.fileIO.WriteMasterDataVersionFile(this.MasterDataVersion));
		this.downloadedMasterDataCount++;
		yield break;
	}

	private IEnumerator RequestMasterData(List<MasterId> updateInfoList)
	{
		for (int i = 0; i < updateInfoList.Count; i++)
		{
			MasterBase master = this.GetMaster(updateInfoList[i]);
			RequestBase request = master.CreateRequest();
			GameWebAPI.Instance().AddDisableVersionCheckApiId(request.apiId);
			yield return base.StartCoroutine(request.Run(null, null, null));
			GameWebAPI.Instance().RemoveDisableVersionCheckApiId(request.apiId);
			this.downloadedMasterDataCount++;
		}
		yield break;
	}

	public bool IsFinishedGetMasterData()
	{
		return this.downloadMasterDataNum == this.downloadedMasterDataCount;
	}

	public int GetMasterDataDownloadNum()
	{
		return this.downloadMasterDataNum;
	}

	public int GetMasterDataProgress()
	{
		return this.downloadedMasterDataCount;
	}

	public enum AssetCategory
	{
		MONSTER = 1,
		DIGI_STONE,
		LINK_POINT,
		TIP,
		EXP,
		ITEM,
		MONSTER_MAX,
		DECK_COST,
		FRIEND_MAX,
		STAMINA_MAX,
		GATHA_TICKET,
		MULTI_ITEM,
		MEAT,
		SOUL,
		NO_DATA_ID,
		FACILITY_KEY,
		CHIP,
		DUNGEON_TICKET,
		TITLE
	}
}
