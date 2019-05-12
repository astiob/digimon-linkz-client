using BattleStateMachineInternal;
using Enemy.AI;
using Enemy.DropItem;
using Master;
using Monster;
using MultiBattle.Tools;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TutorialRequestHeader;
using UnityEngine;
using UnityExtension;

public class BattleServerControl : BattleFunctionBase
{
	public override void BattleAwakeInitialize()
	{
	}

	public override void BattleTriggerInitialize()
	{
		this.InitBattleAdventureSceneManager();
		if (base.onServerConnect)
		{
			if (base.stateManager.battleMode == BattleMode.PvP)
			{
				this.GetPvPBattleData();
			}
			else if (base.stateManager.battleMode == BattleMode.Multi)
			{
				this.GetMultiBattleData();
			}
			else
			{
				this.GetBattleData();
			}
		}
	}

	private void InitBattleAdventureSceneManager()
	{
		if (base.onServerConnect)
		{
			GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene[] worldDungeonAdventureScenes = null;
			GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene[] worldDungeonAdventureSceneM = MasterDataMng.Instance().ResponseWorldDungeonAdventureSceneMaster.worldDungeonAdventureSceneM;
			if (worldDungeonAdventureSceneM != null)
			{
				string dungeonId = string.Empty;
				if (base.stateManager.battleMode == BattleMode.Multi)
				{
					dungeonId = DataMng.Instance().RespData_WorldMultiStartInfo.worldDungeonId;
				}
				else if (base.stateManager.battleMode == BattleMode.PvP)
				{
					dungeonId = ClassSingleton<MultiBattleData>.Instance.PvPField.worldDungeonId;
				}
				else
				{
					dungeonId = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart.worldDungeonId;
				}
				worldDungeonAdventureScenes = worldDungeonAdventureSceneM.Where((GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene item) => item.worldDungeonId == dungeonId).ToArray<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene>();
			}
			base.stateManager.battleAdventureSceneManager = new BattleAdventureSceneManager(worldDungeonAdventureScenes);
		}
		else
		{
			base.stateManager.battleAdventureSceneManager = new BattleAdventureSceneManager(null);
		}
	}

	private void GetBattleData()
	{
		GameWebAPI.RespDataWD_DungeonStart respDataWD_DungeonStart = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart;
		DataMng.Instance().WD_ReqDngResult.startId = respDataWD_DungeonStart.startId;
		DataMng.Instance().WD_ReqDngResult.dungeonId = respDataWD_DungeonStart.worldDungeonId;
		base.hierarchyData.startId = respDataWD_DungeonStart.startId;
		base.hierarchyData.usePlayerCharacters = new PlayerStatus[respDataWD_DungeonStart.deck.userMonsterIds.Length];
		for (int i = 0; i < base.hierarchyData.usePlayerCharacters.Length; i++)
		{
			base.hierarchyData.usePlayerCharacters[i] = this.DeckToPlayerStatus(respDataWD_DungeonStart.deck.userMonsterIds[i]);
		}
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonM = this.GetWorldDungeonM(respDataWD_DungeonStart.worldDungeonId);
		base.hierarchyData.useStageId = worldDungeonM.background;
		base.hierarchyData.areaName = worldDungeonM.name;
		base.hierarchyData.limitRound = int.Parse(worldDungeonM.limitRound);
		base.hierarchyData.speedClearRound = int.Parse(worldDungeonM.optionRewardFlg);
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
		foreach (GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 in worldStageM)
		{
			if (worldDungeonM.worldStageId == worldStageM2.worldStageId)
			{
				base.hierarchyData.areaId = worldStageM2.worldAreaId.ToInt32();
				break;
			}
		}
		this.InitExtraEffectStatus(respDataWD_DungeonStart.worldDungeonId);
		base.hierarchyData.battleNum = worldDungeonM.battleNum;
		base.hierarchyData.batteWaves = this.DungeonFloorToBattleWave(respDataWD_DungeonStart.dungeonFloor, worldDungeonM);
		base.hierarchyData.digiStoneNumber = DataMng.Instance().GetStone();
		base.battleStateData.beforeConfirmDigiStoneNumber = base.hierarchyData.digiStoneNumber;
		base.hierarchyData.isPossibleContinue = (int.Parse(worldDungeonM.canContinue) == 1);
		this.InitialIntroduction();
	}

	private void InitialIntroduction()
	{
		base.hierarchyData.useInitialIntroduction = DataMng.Instance().IsBattleFailShowDungeon;
		base.hierarchyData.initialIntroductionIndex = 0;
		if (PlayerPrefs.HasKey("BATTLE_LOSE_COUNT"))
		{
			base.hierarchyData.initialIntroductionIndex = PlayerPrefs.GetInt("BATTLE_LOSE_COUNT");
		}
		else
		{
			PlayerPrefs.SetInt("BATTLE_LOSE_COUNT", base.hierarchyData.initialIntroductionIndex);
		}
		List<TutorialNaviMaster> list = new List<TutorialNaviMaster>();
		foreach (TutorialNaviMaster tutorialNaviMaster2 in MasterDataMng.Instance().Tutorial.GetTutorialNaviMaster())
		{
			if (tutorialNaviMaster2.tutorialNaviId.ToInt32() >= 3000 && tutorialNaviMaster2.tutorialNaviId.ToInt32() < 4000)
			{
				list.Add(tutorialNaviMaster2);
			}
		}
		List<InitialIntroductionBox> list2 = new List<InitialIntroductionBox>();
		foreach (TutorialNaviMaster tutorialNaviMaster3 in list)
		{
			list2.Add(new InitialIntroductionBox(tutorialNaviMaster3.face.ToInt32(), tutorialNaviMaster3.message));
		}
		base.hierarchyData.initialIntroductionMessage = list2.ToArray();
	}

	private void GetMultiBattleData()
	{
		UnityEngine.Random.InitState(ClassSingleton<MultiBattleData>.Instance.RandomSeed);
		GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
		DataMng.Instance().WD_ReqDngResult.startId = respData_WorldMultiStartInfo.startId;
		DataMng.Instance().WD_ReqDngResult.dungeonId = respData_WorldMultiStartInfo.worldDungeonId;
		base.hierarchyData.startId = respData_WorldMultiStartInfo.startId;
		int num = 3;
		string[] playerUserMonsterIds = ClassSingleton<MultiBattleData>.Instance.PlayerUserMonsterIds;
		base.hierarchyData.usePlayerCharacters = new PlayerStatus[num];
		for (int i = 0; i < respData_WorldMultiStartInfo.party.Length; i++)
		{
			GameWebAPI.RespData_WorldMultiStartInfo.Party party = respData_WorldMultiStartInfo.party[i];
			for (int j = 0; j < party.userMonsters.Length; j++)
			{
				for (int k = 0; k < num; k++)
				{
					GameWebAPI.Common_MonsterData common_MonsterData = party.userMonsters[j];
					if (playerUserMonsterIds[k] == common_MonsterData.userMonsterId)
					{
						base.hierarchyData.usePlayerCharacters[k] = this.ConvertAPIParamsToPlayerStatus(common_MonsterData);
					}
				}
			}
		}
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonM = this.GetWorldDungeonM(respData_WorldMultiStartInfo.worldDungeonId);
		base.hierarchyData.useStageId = worldDungeonM.background;
		base.hierarchyData.areaName = worldDungeonM.name;
		base.hierarchyData.limitRound = int.Parse(worldDungeonM.limitRound);
		base.hierarchyData.speedClearRound = int.Parse(worldDungeonM.optionRewardFlg);
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
		foreach (GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 in worldStageM)
		{
			if (worldDungeonM.worldStageId == worldStageM2.worldStageId)
			{
				base.hierarchyData.areaId = worldStageM2.worldAreaId.ToInt32();
				break;
			}
		}
		this.InitExtraEffectStatus(worldDungeonM.worldDungeonId);
		base.hierarchyData.isPossibleContinue = (int.Parse(worldDungeonM.canContinue) == 1);
		base.hierarchyData.battleNum = worldDungeonM.battleNum;
		base.hierarchyData.batteWaves = this.DungeonFloorToBattleWave(respData_WorldMultiStartInfo.dungeonFloor, worldDungeonM);
		base.hierarchyData.digiStoneNumber = DataMng.Instance().GetStone();
		base.battleStateData.beforeConfirmDigiStoneNumber = base.hierarchyData.digiStoneNumber;
		base.hierarchyData.playerPursuitPercentage = ServerToBattleUtility.PermillionToPercentage(respData_WorldMultiStartInfo.criticalRate.partyCriticalRate);
		base.hierarchyData.enemyPursuitPercentage = ServerToBattleUtility.PermillionToPercentage(respData_WorldMultiStartInfo.criticalRate.enemyCriticalRate);
	}

	private void GetPvPBattleData()
	{
		UnityEngine.Random.InitState(ClassSingleton<MultiBattleData>.Instance.RandomSeed);
		MultiBattleData.PvPUserData pvPUserData = null;
		foreach (MultiBattleData.PvPUserData pvPUserData2 in ClassSingleton<MultiBattleData>.Instance.PvPUserDatas)
		{
			bool flag = pvPUserData2.userStatus.userId == ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId;
			if (flag)
			{
				pvPUserData = pvPUserData2;
				break;
			}
		}
		base.hierarchyData.usePlayerCharacters = new PlayerStatus[3];
		for (int j = 0; j < base.hierarchyData.usePlayerCharacters.Length; j++)
		{
			base.hierarchyData.usePlayerCharacters[j] = this.ConvertAPIParamsToPlayerStatus(pvPUserData.monsterData[j]);
		}
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonM = this.GetWorldDungeonM(ClassSingleton<MultiBattleData>.Instance.PvPField.worldDungeonId);
		base.hierarchyData.useStageId = worldDungeonM.background;
		base.hierarchyData.areaName = worldDungeonM.name;
		base.hierarchyData.limitRound = int.Parse(worldDungeonM.limitRound);
		base.hierarchyData.speedClearRound = int.Parse(worldDungeonM.optionRewardFlg);
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
		foreach (GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 in worldStageM)
		{
			if (worldDungeonM.worldStageId == worldStageM2.worldStageId)
			{
				base.hierarchyData.areaId = worldStageM2.worldAreaId.ToInt32();
				break;
			}
		}
		base.hierarchyData.isPossibleContinue = (int.Parse(worldDungeonM.canContinue) == 1);
		this.InitExtraEffectStatus(worldDungeonM.worldDungeonId);
		base.hierarchyData.battleNum = worldDungeonM.battleNum;
		base.hierarchyData.batteWaves = this.DungeonFloorToBattleWave(worldDungeonM);
		base.hierarchyData.digiStoneNumber = DataMng.Instance().GetStone();
		base.battleStateData.beforeConfirmDigiStoneNumber = base.stateManager.hierarchyData.digiStoneNumber;
	}

	private BattleWave[] DungeonFloorToBattleWave(GameWebAPI.RespDataWD_DungeonStart.DungeonFloor[] floor, GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonM)
	{
		BattleWave[] array = new BattleWave[floor.Length];
		for (int i = 0; i < array.Length; i++)
		{
			BattleWave battleWave = new BattleWave();
			battleWave.floorNum = floor[i].floorNum;
			battleWave.floorType = floor[i].floorType;
			battleWave.cameraType = floor[i].cameraType;
			battleWave.useEnemiesId = new string[floor[i].enemy.Length];
			battleWave.enemiesBossFlag = new bool[battleWave.useEnemiesId.Length];
			battleWave.enemiesInfinityApFlag = new bool[battleWave.useEnemiesId.Length];
			battleWave.enemyStatuses = new EnemyStatus[battleWave.useEnemiesId.Length];
			for (int j = 0; j < battleWave.useEnemiesId.Length; j++)
			{
				int num = i * 10 + j;
				battleWave.useEnemiesId[j] = num.ToString();
				EnemyType type = (EnemyType)floor[i].enemy[j].type;
				battleWave.enemiesBossFlag[j] = this.CheckBossFlag(type);
				battleWave.enemiesInfinityApFlag[j] = this.CheckInfinityAp(type);
				battleWave.enemyStatuses[j] = this.EnemyToEnemyStatus(floor[i].enemy[j]);
			}
			battleWave.hpRevivalPercentage = ServerToBattleUtility.PermillionToPercentage(floor[i].healingRate);
			if (BoolExtension.AllMachValue(false, battleWave.enemiesBossFlag))
			{
				battleWave.bgmId = worldDungeonM.bgm;
			}
			else
			{
				battleWave.bgmId = worldDungeonM.bossBgm;
				battleWave.bgmChangeHpPercentage = ServerToBattleUtility.PermillionToPercentage(worldDungeonM.exBossBgmCondition);
				battleWave.changedBgmId = worldDungeonM.exBossBgm;
			}
			array[i] = battleWave;
		}
		return array;
	}

	private BattleWave[] DungeonFloorToBattleWave(GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonM)
	{
		MultiBattleData.PvPUserData pvPUserData = null;
		foreach (MultiBattleData.PvPUserData pvPUserData2 in ClassSingleton<MultiBattleData>.Instance.PvPUserDatas)
		{
			if (!(pvPUserData2.userStatus.userId == ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId))
			{
				pvPUserData = pvPUserData2;
				break;
			}
		}
		BattleWave[] array = new BattleWave[1];
		for (int j = 0; j < array.Length; j++)
		{
			BattleWave battleWave = new BattleWave();
			battleWave.useEnemiesId = new string[pvPUserData.monsterData.Length];
			battleWave.enemiesBossFlag = new bool[battleWave.useEnemiesId.Length];
			battleWave.enemyStatuses = new CharacterStatus[battleWave.useEnemiesId.Length];
			for (int k = 0; k < pvPUserData.monsterData.Length; k++)
			{
				int num = (j + 1) * 10 + k;
				battleWave.useEnemiesId[k] = num.ToString();
				battleWave.enemiesBossFlag[k] = false;
				battleWave.enemyStatuses[k] = this.ConvertAPIParamsToPlayerStatus(pvPUserData.monsterData[k]);
			}
			if (BoolExtension.AllMachValue(false, battleWave.enemiesBossFlag))
			{
				battleWave.bgmId = worldDungeonM.bgm;
			}
			else
			{
				battleWave.bgmId = worldDungeonM.bossBgm;
				battleWave.bgmChangeHpPercentage = ServerToBattleUtility.PermillionToPercentage(worldDungeonM.exBossBgmCondition);
				battleWave.changedBgmId = worldDungeonM.exBossBgm;
			}
			array[j] = battleWave;
		}
		return array;
	}

	private bool CheckBossFlag(EnemyType enemyType)
	{
		return enemyType == EnemyType.Boss || enemyType == EnemyType.BossInfinityAp;
	}

	private bool CheckInfinityAp(EnemyType enemyType)
	{
		return enemyType == EnemyType.StandardInfinityAp || enemyType == EnemyType.BossInfinityAp;
	}

	private PlayerStatus DeckToPlayerStatus(string userMonsterId)
	{
		MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(userMonsterId, false);
		List<int> list = new List<int>();
		GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip[] slotEquip = monsterDataByUserMonsterID.GetSlotEquip();
		foreach (GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip equip in slotEquip)
		{
			GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChip = ChipDataMng.GetUserChip(equip.userChipId);
			list.Add(userChip.chipId);
		}
		return this.ConvertMonsterDataToPlayerStatus(userMonsterId, monsterDataByUserMonsterID, list);
	}

	private PlayerStatus ConvertAPIParamsToPlayerStatus(GameWebAPI.Common_MonsterData commonMonsterData)
	{
		MonsterData monsterData = MultiTools.MakeAndSetMonster(commonMonsterData);
		List<int> list = new List<int>();
		if (commonMonsterData.chipIdList != null && commonMonsterData.chipIdList.Length > 0)
		{
			foreach (int item in commonMonsterData.chipIdList)
			{
				list.Add(item);
			}
		}
		return this.ConvertMonsterDataToPlayerStatus(commonMonsterData.userMonsterId, monsterData, list);
	}

	private PlayerStatus ConvertMonsterDataToPlayerStatus(string userMonsterId, MonsterData monsterData, List<int> chipIdList)
	{
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = MonsterMaster.GetMonsterMasterByMonsterGroupId(monsterData.monsterM.monsterGroupId).Group;
		string monsterId = monsterData.monsterM.monsterId;
		string monsterGroupId = group.monsterGroupId;
		string modelId = group.modelId;
		int arousal = monsterData.monsterM.GetArousal();
		int friendshipLevel = monsterData.userMonster.friendship.ToInt32();
		int hp = monsterData.userMonster.hp.ToInt32();
		int attackPower = monsterData.userMonster.attack.ToInt32();
		int defencePower = monsterData.userMonster.defense.ToInt32();
		int specialAttackPower = monsterData.userMonster.spAttack.ToInt32();
		int specialDefencePower = monsterData.userMonster.spDefense.ToInt32();
		int speed = monsterData.userMonster.speed.ToInt32();
		int maxAttackPower = monsterData.monsterM.maxAttack.ToInt32();
		int maxDefencePower = monsterData.monsterM.maxDefense.ToInt32();
		int maxSpecialAttackPower = monsterData.monsterM.maxSpAttack.ToInt32();
		int maxSpecialDefencePower = monsterData.monsterM.maxSpDefense.ToInt32();
		int maxSpeed = monsterData.monsterM.speed.ToInt32();
		int level = monsterData.userMonster.level.ToInt32();
		int luck = monsterData.userMonster.luck.ToInt32();
		string text = monsterData.userMonster.leaderSkillId.Equals("0") ? string.Empty : monsterData.userMonster.leaderSkillId;
		string iconId = monsterData.monsterM.iconId;
		Talent talent = new Talent(monsterData.userMonster);
		FriendshipStatus friendshipStatus = new FriendshipStatus(friendshipLevel, maxAttackPower, maxDefencePower, maxSpecialAttackPower, maxSpecialDefencePower, maxSpeed);
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM resistanceMaster = MonsterResistanceData.GetResistanceMaster(monsterData.monsterM.resistanceId);
		List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> uniqueResistanceList = MonsterResistanceData.GetUniqueResistanceList(monsterData.GetResistanceIdList());
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM data = MonsterResistanceData.AddResistanceFromMultipleTranceData(resistanceMaster, uniqueResistanceList);
		Tolerance tolerance = ServerToBattleUtility.ResistanceToTolerance(data);
		List<string> list = new List<string>();
		if (!string.IsNullOrEmpty(base.stateManager.publicAttackSkillId))
		{
			SkillStatus skillStatus = this.SkillMToSkillStatus(base.stateManager.publicAttackSkillId);
			if (skillStatus != null)
			{
				list.Add(base.stateManager.publicAttackSkillId);
				base.hierarchyData.AddSkillStatus(base.stateManager.publicAttackSkillId, skillStatus);
			}
		}
		if (!string.IsNullOrEmpty(monsterData.userMonster.uniqueSkillId))
		{
			SkillStatus skillStatus2 = this.SkillMToSkillStatus(monsterData.userMonster.uniqueSkillId);
			if (skillStatus2 != null)
			{
				list.Add(monsterData.userMonster.uniqueSkillId);
				base.hierarchyData.AddSkillStatus(monsterData.userMonster.uniqueSkillId, skillStatus2);
			}
		}
		if (!string.IsNullOrEmpty(monsterData.userMonster.commonSkillId))
		{
			SkillStatus skillStatus3 = this.SkillMToSkillStatus(monsterData.userMonster.commonSkillId);
			if (skillStatus3 != null)
			{
				list.Add(monsterData.userMonster.commonSkillId);
				base.hierarchyData.AddSkillStatus(monsterData.userMonster.commonSkillId, skillStatus3);
			}
		}
		if (!string.IsNullOrEmpty(monsterData.userMonster.extraCommonSkillId))
		{
			SkillStatus skillStatus4 = this.SkillMToSkillStatus(monsterData.userMonster.extraCommonSkillId);
			if (skillStatus4 != null)
			{
				list.Add(monsterData.userMonster.extraCommonSkillId);
				base.hierarchyData.AddSkillStatus(monsterData.userMonster.extraCommonSkillId, skillStatus4);
			}
		}
		foreach (int num in chipIdList)
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectData = ChipDataMng.GetChipEffectData(num.ToString());
			if (chipEffectData != null)
			{
				foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffectData)
				{
					int num2 = chipEffect.effectType.ToInt32();
					string effectValue = chipEffect.effectValue;
					if (num2 == 60 || num2 == 61 || (num2 == 56 && num2 > 0))
					{
						SkillStatus skillStatus5 = base.stateManager.serverControl.SkillMToSkillStatus(effectValue);
						base.hierarchyData.AddSkillStatus(skillStatus5.skillId, skillStatus5);
					}
				}
			}
		}
		GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster responseMonsterIntegrationGroupMaster = MasterDataMng.Instance().ResponseMonsterIntegrationGroupMaster;
		GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup[] source = responseMonsterIntegrationGroupMaster.monsterIntegrationGroupM.Where((GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup item) => item.monsterId == monsterId).ToArray<GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup>();
		string[] monsterIntegrationIds = source.Select((GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup item) => item.monsterIntegrationId).ToArray<string>();
		PlayerStatus result = new PlayerStatus(userMonsterId, modelId, monsterGroupId, hp, attackPower, defencePower, specialAttackPower, specialDefencePower, speed, level, tolerance, luck, text, iconId, talent, arousal, friendshipStatus, list.ToArray(), chipIdList.ToArray(), monsterIntegrationIds);
		base.hierarchyData.AddLeaderSkillStatus(text, this.SkillMToLeaderSkillStatus(text));
		base.hierarchyData.AddCharacterDatas(monsterGroupId, this.MonsterMToCharacterData(monsterGroupId));
		return result;
	}

	private EnemyStatus EnemyToEnemyStatus(GameWebAPI.RespDataWD_DungeonStart.Enemy enemy)
	{
		GameWebAPI.RespDataMA_GetMonsterMS.MonsterM simple = MonsterMaster.GetMonsterMasterByMonsterId(enemy.monsterId).Simple;
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = MonsterMaster.GetMonsterMasterByMonsterGroupId(simple.monsterGroupId).Group;
		string monsterId = simple.monsterId;
		string monsterGroupId = group.monsterGroupId;
		string modelId = group.modelId;
		int hp = enemy.hp;
		int attack = enemy.attack;
		int defense = enemy.defense;
		int spAttack = enemy.spAttack;
		int spDefense = enemy.spDefense;
		int speed = enemy.speed;
		int level = enemy.level;
		Tolerance tolerance = this.ResistanceToTolerance(enemy.resistanceId);
		string empty = string.Empty;
		if (enemy.ai == null || enemy.ai.Length <= 0)
		{
			global::Debug.LogError("enemy.aiのマスタがおかしいです.");
			return null;
		}
		AICycle aicycle = ServerToBattleUtility.IntToAICycle(enemy.ai[0].type);
		List<AIActionClip> list = new List<AIActionClip>();
		List<AIActionPattern> list2 = new List<AIActionPattern>();
		List<SkillStatus> list3 = new List<SkillStatus>();
		for (int i = 0; i < enemy.ai.Length; i++)
		{
			int num = (aicycle != AICycle.fixableRotation) ? 0 : (enemy.ai[i].priority - 1);
			float minRange = (aicycle != AICycle.targetHpAltenation) ? 0f : ServerToBattleUtility.HundredToPercentage(enemy.ai[i].minHpRange);
			float maxRange = (aicycle != AICycle.targetHpAltenation) ? 0f : ServerToBattleUtility.HundredToPercentage(enemy.ai[i].maxHpRange);
			TargetSelectReference targetSelectReference = ServerToBattleUtility.IntToTargetSelectReference(enemy.ai[i].lookStatus);
			SelectingOrder selectingOrder = ServerToBattleUtility.IntToSelectingOrder(enemy.ai[i].lookType);
			float minValue = (targetSelectReference != TargetSelectReference.Hp) ? ((float)enemy.ai[i].invokeMinRange) : ServerToBattleUtility.HundredToPercentage(enemy.ai[i].invokeMinRange);
			float maxValue = (targetSelectReference != TargetSelectReference.Hp) ? ((float)enemy.ai[i].invokeMaxRange) : ServerToBattleUtility.HundredToPercentage(enemy.ai[i].invokeMaxRange);
			float minRange2 = 0f;
			float maxRange2 = ServerToBattleUtility.PermillionToPercentage(enemy.ai[i].rate);
			string skillId = enemy.ai[i].skillId;
			list3.Add(this.SkillMToSkillStatus(skillId));
			AIActionClip item5 = new AIActionClip(targetSelectReference, selectingOrder, minValue, maxValue, minRange2, maxRange2, skillId);
			bool flag = false;
			if (i < enemy.ai.Length - 1)
			{
				if (aicycle == AICycle.fixableRotation)
				{
					if (enemy.ai[i + 1].priority - 1 != num)
					{
						flag = true;
					}
				}
				else if (enemy.ai[i].minHpRange != enemy.ai[i + 1].minHpRange && enemy.ai[i].maxHpRange != enemy.ai[i + 1].maxHpRange)
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			list.Add(item5);
			if (flag)
			{
				list2.Add(new AIActionPattern(minRange, maxRange, list.ToArray()));
				list.Clear();
			}
		}
		EnemyAIPattern enemyAIPattern = new EnemyAIPattern(aicycle, list2.ToArray());
		int fixedMoney = enemy.fixedMoney;
		int fixedExp = enemy.fixedExp;
		List<ItemDropResult> list4 = new List<ItemDropResult>();
		if (enemy.drop != null)
		{
			foreach (GameWebAPI.RespDataWD_DungeonStart.Drop drop2 in enemy.drop)
			{
				DropBoxType dropBoxType = ServerToBattleUtility.IntToDropBoxType(drop2.dropBoxType);
				ItemDropResult item2 = new ItemDropResult(dropBoxType);
				list4.Add(item2);
			}
		}
		else
		{
			ItemDropResult item3 = new ItemDropResult(false);
			list4.Add(item3);
		}
		List<string> list5 = new List<string>();
		list5.Add(base.stateManager.publicAttackSkillId);
		foreach (string item4 in enemyAIPattern.GetAllSkillID())
		{
			if (!list5.Contains(item4))
			{
				list5.Add(item4);
			}
		}
		int[] array = enemy.chipIdList;
		if (enemy.chipIdList == null)
		{
			array = new int[0];
		}
		foreach (int num2 in array)
		{
			GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectData = ChipDataMng.GetChipEffectData(num2.ToString());
			if (chipEffectData != null)
			{
				foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffectData)
				{
					int num3 = chipEffect.effectType.ToInt32();
					string effectValue = chipEffect.effectValue;
					if (num3 == 60 || num3 == 61 || (num3 == 56 && num3 > 0))
					{
						SkillStatus skillStatus = base.stateManager.serverControl.SkillMToSkillStatus(effectValue);
						base.hierarchyData.AddSkillStatus(skillStatus.skillId, skillStatus);
					}
				}
			}
		}
		GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster responseMonsterIntegrationGroupMaster = MasterDataMng.Instance().ResponseMonsterIntegrationGroupMaster;
		GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup[] source = responseMonsterIntegrationGroupMaster.monsterIntegrationGroupM.Where((GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup item) => item.monsterId == monsterId).ToArray<GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup>();
		string[] monsterIntegrationIds = source.Select((GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup item) => item.monsterIntegrationId).ToArray<string>();
		EnemyStatus result = new EnemyStatus(modelId, monsterGroupId, hp, attack, defense, spAttack, spDefense, speed, level, tolerance, enemyAIPattern, fixedMoney, fixedExp, list4, list5.ToArray(), array, monsterIntegrationIds);
		base.hierarchyData.AddLeaderSkillStatus(empty, this.SkillMToLeaderSkillStatus(empty));
		foreach (SkillStatus skillStatus2 in list3)
		{
			base.hierarchyData.AddSkillStatus(skillStatus2.skillId, skillStatus2);
		}
		base.hierarchyData.AddCharacterDatas(monsterGroupId, this.MonsterMToCharacterData(monsterGroupId));
		return result;
	}

	private SkillStatus SkillMToSkillStatus(string skillId)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetSkillStatus(skillId);
		}
		if (string.IsNullOrEmpty(skillId))
		{
			return null;
		}
		GameWebAPI.RespDataMA_GetSkillM.SkillM[] skillM = MasterDataMng.Instance().RespDataMA_SkillM.skillM;
		for (int i = 0; i < skillM.Length; i++)
		{
			if (!ServerToBattleUtility.GetIsLeaderSkill(skillM[i].type))
			{
				if (skillId.Equals(skillM[i].skillId))
				{
					List<AffectEffectProperty> affectEffectPropertyList = this.GetAffectEffectPropertyList(skillId);
					string attackEffect = skillM[i].attackEffect;
					string soundEffect = skillM[i].soundEffect;
					SkillType skillType = ServerToBattleUtility.IntToSkillType(skillM[i].type);
					string name = skillM[i].name;
					string description = skillM[i].description;
					int needAp = int.Parse(skillM[i].needPoint);
					string useCountType = "0";
					int useCountValue = (!string.IsNullOrEmpty(skillM[i].useCountValue)) ? int.Parse(skillM[i].useCountValue) : 0;
					EffectTarget target = affectEffectPropertyList[0].target;
					EffectNumbers effectNumbers = affectEffectPropertyList[0].effectNumbers;
					foreach (AffectEffectProperty affectEffectProperty in affectEffectPropertyList)
					{
						if (affectEffectProperty.target == EffectTarget.Enemy || affectEffectProperty.target == EffectTarget.EnemyWithoutAttacker)
						{
							target = affectEffectProperty.target;
							effectNumbers = affectEffectProperty.effectNumbers;
							break;
						}
					}
					return new SkillStatus(skillId, attackEffect, soundEffect, skillType, name, description, target, effectNumbers, needAp, useCountType, useCountValue, affectEffectPropertyList.ToArray());
				}
			}
		}
		global::Debug.LogWarning("SkillStatusの生成に失敗しました. (" + skillId + ")");
		return null;
	}

	private LeaderSkillStatus SkillMToLeaderSkillStatus(string leaderSkillId)
	{
		if (string.IsNullOrEmpty(leaderSkillId))
		{
			return null;
		}
		GameWebAPI.RespDataMA_GetSkillM.SkillM[] skillM = MasterDataMng.Instance().RespDataMA_SkillM.skillM;
		LeaderSkillStatus leaderSkillStatus = null;
		for (int i = 0; i < skillM.Length; i++)
		{
			if (ServerToBattleUtility.GetIsLeaderSkill(skillM[i].type))
			{
				if (leaderSkillId.Equals(skillM[i].skillId))
				{
					string name = skillM[i].name;
					string description = skillM[i].description;
					GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM[] convertSkillDetailM = MasterDataMng.Instance().RespDataMA_SkillDetailM.convertSkillDetailM;
					for (int j = 0; j < convertSkillDetailM.Length; j++)
					{
						if (leaderSkillId.Equals(convertSkillDetailM[j].skillId))
						{
							LeaderSkillType leaderSkillType = ServerToBattleUtility.IntToLeaderSkillType(convertSkillDetailM[j].effectType);
							float hpFollowingPercent = (!ServerToBattleUtility.GetOnHpFollowingLeaderSkill(leaderSkillType)) ? 0f : ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[j].effect2);
							float upPercent = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[j].effect1);
							Tolerance tolerance;
							if (leaderSkillType == LeaderSkillType.ToleranceUp)
							{
								tolerance = new Tolerance((Strength)convertSkillDetailM[j].effect3, (Strength)convertSkillDetailM[j].effect4, (Strength)convertSkillDetailM[j].effect5, (Strength)convertSkillDetailM[j].effect6, (Strength)convertSkillDetailM[j].effect7, (Strength)convertSkillDetailM[j].effect8, (Strength)convertSkillDetailM[j].effect9, (Strength)convertSkillDetailM[j].effect10, (Strength)convertSkillDetailM[j].effect11, (Strength)convertSkillDetailM[j].effect12, (Strength)convertSkillDetailM[j].effect13, (Strength)convertSkillDetailM[j].effect14, (Strength)convertSkillDetailM[j].effect15, (Strength)convertSkillDetailM[j].effect16);
							}
							else
							{
								tolerance = Tolerance.GetNutralTolerance();
							}
							leaderSkillStatus = new LeaderSkillStatus(leaderSkillId, name, description, leaderSkillType, hpFollowingPercent, upPercent, tolerance);
							break;
						}
					}
					if (leaderSkillStatus != null)
					{
						break;
					}
					global::Debug.LogError("スキル詳細マスタから一致するリーダースキルIDが一つも見つかりませんでした. (" + leaderSkillId + ")");
				}
			}
		}
		if (leaderSkillStatus == null)
		{
			global::Debug.LogError("LeaderSkillStatusの生成に失敗しました. (" + leaderSkillId + ")");
			return null;
		}
		return leaderSkillStatus;
	}

	private CharacterDatas MonsterMToCharacterData(string monsterGroupId)
	{
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = MonsterMaster.GetMonsterMasterByMonsterGroupId(monsterGroupId).Group;
		string monsterName = group.monsterName;
		string tribe = group.tribe;
		GrowStep growStep = MonsterGrowStepData.ToGrowStep(group.growStep);
		string monsterStatusId = group.monsterStatusId;
		return new CharacterDatas(monsterName, tribe, growStep, monsterStatusId);
	}

	private Tolerance ResistanceToTolerance(string resistanceId)
	{
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM[] monsterResistanceM = MasterDataMng.Instance().RespDataMA_MonsterResistanceM.monsterResistanceM;
		Tolerance result = null;
		for (int i = 0; i < monsterResistanceM.Length; i++)
		{
			if (monsterResistanceM[i].monsterResistanceId.Equals(resistanceId))
			{
				result = ServerToBattleUtility.ResistanceToTolerance(monsterResistanceM[i]);
			}
		}
		return result;
	}

	private void InitExtraEffectStatus(string dungeonId)
	{
		IEnumerable<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> enumerable = MasterDataMng.Instance().RespDataMA_WorldDungeonExtraEffectM.worldDungeonExtraEffectM.SelectMany((GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM x) => MasterDataMng.Instance().RespDataMA_WorldDungeonExtraEffectManageM.worldDungeonExtraEffectManageM, (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM x, GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectManageM.WorldDungeonExtraEffectManageM y) => new
		{
			x,
			y
		}).Where(<>__TranspIdent0 => <>__TranspIdent0.x.worldDungeonExtraEffectId == <>__TranspIdent0.y.worldDungeonExtraEffectId && <>__TranspIdent0.y.worldDungeonId == dungeonId).Select(<>__TranspIdent0 => <>__TranspIdent0.x);
		List<ExtraEffectStatus> list = new List<ExtraEffectStatus>();
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM worldDungeonExtraEffectM in enumerable)
		{
			list.Add(new ExtraEffectStatus(worldDungeonExtraEffectM));
		}
		base.battleStateData.extraEffectStatus.AddRange(list);
		foreach (ExtraEffectStatus extraEffectStatus in list)
		{
			EffectStatusBase.ExtraEffectType effectType = (EffectStatusBase.ExtraEffectType)extraEffectStatus.EffectType;
			if (effectType == EffectStatusBase.ExtraEffectType.Skill)
			{
				string skillId = extraEffectStatus.EffectValue.ToString();
				SkillStatus skillStatus = base.stateManager.serverControl.SkillMToSkillStatus(skillId);
				base.hierarchyData.AddSkillStatus(skillStatus.skillId, skillStatus);
			}
		}
	}

	public List<AffectEffectProperty> GetAffectEffectPropertyList(string skillId)
	{
		return BattleServerControl.GetAffectEffectPropertyListForUtil(skillId);
	}

	public static List<AffectEffectProperty> GetAffectEffectPropertyListForUtil(string skillId)
	{
		GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM[] convertSkillDetailM = MasterDataMng.Instance().RespDataMA_SkillDetailM.convertSkillDetailM;
		List<AffectEffectProperty> list = new List<AffectEffectProperty>();
		for (int i = 0; i < convertSkillDetailM.Length; i++)
		{
			if (skillId.Equals(convertSkillDetailM[i].skillId))
			{
				EffectTarget target = ServerToBattleUtility.IntToEffectTarget(convertSkillDetailM[i].target);
				AffectEffect type = ServerToBattleUtility.IntToAffectEffect(convertSkillDetailM[i].effectType);
				float hitRate = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[i].hitRate);
				EffectNumbers effectNumbers = ServerToBattleUtility.IntToEffectNumbers(convertSkillDetailM[i].targetType);
				int effect = convertSkillDetailM[i].effect1;
				int effect2 = convertSkillDetailM[i].effect2;
				float num = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[i].effect3, false);
				float num2 = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[i].effect4, false);
				float num3 = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[i].effect5, false);
				float num4 = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[i].effect6, false);
				float num5 = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[i].effect7, false);
				float num6 = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[i].effect8, false);
				float num7 = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[i].effect9, false);
				float num8 = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[i].effect10, false);
				float num9 = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[i].effect11, false);
				float num10 = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[i].effect12, false);
				float num11 = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[i].effect13, false);
				float num12 = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[i].effect14, false);
				float num13 = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[i].effect15, false);
				float num14 = ServerToBattleUtility.PermillionToPercentage(convertSkillDetailM[i].effect16, false);
				bool useDrainAffectEffect = ServerToBattleUtility.GetUseDrainAffectEffect(convertSkillDetailM[i].effectType);
				PowerType powerType = ServerToBattleUtility.GetPowerType(convertSkillDetailM[i].effectType);
				TechniqueType techniqueType = ServerToBattleUtility.GetTechniqueType(convertSkillDetailM[i].effectType);
				global::Attribute attribute = ServerToBattleUtility.IntToAttribute(convertSkillDetailM[i].attribute);
				bool isMissThrough = convertSkillDetailM[i].isMissTrough > 0;
				AffectEffectProperty item = new AffectEffectProperty(type, skillId.ToInt32(), convertSkillDetailM[i].subId.ToInt32(), hitRate, target, effectNumbers, new int[]
				{
					effect,
					effect2
				}, new float[]
				{
					num,
					num2,
					num3,
					num4,
					num5,
					num6,
					num7,
					num8,
					num9,
					num10,
					num11,
					num12,
					num13,
					num14
				}, useDrainAffectEffect, powerType, techniqueType, attribute, isMissThrough);
				list.Add(item);
			}
		}
		return list;
	}

	private GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM GetWorldDungeonM(string worldDungeonId)
	{
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM;
		for (int i = 0; i < worldDungeonM.Length; i++)
		{
			if (worldDungeonM[i].worldDungeonId.Equals(worldDungeonId))
			{
				return worldDungeonM[i];
			}
		}
		return null;
	}

	public IEnumerator ContinueBuyDigistoneFunction(bool isContinue)
	{
		if (!base.onServerConnect)
		{
			yield break;
		}
		if (base.battleStateData.isShowShop)
		{
			yield break;
		}
		base.battleStateData.isShowShop = true;
		base.battleStateData.isEnableBackKeySystem = false;
		bool shopCloseWait = false;
		SoundPlayer.PlayButtonEnter();
		IEnumerator wait = StoreInit.Instance().GetProductsOperation();
		while (wait.MoveNext())
		{
			yield return null;
		}
		if (DataMng.Instance().RespDataSH_Info.isShopMaintenance != 1)
		{
			Transform obstacleBGTrans = base.stateManager.battleUiComponents.characterRevivalDialog.transform;
			if (isContinue)
			{
				obstacleBGTrans = base.stateManager.battleUiComponents.dialogContinue.transform;
			}
			Action<int> action = delegate(int a)
			{
				global::Debug.Log(this.battleStateData.beforeConfirmDigiStoneNumber + " / " + DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point);
				this.battleStateData.beforeConfirmDigiStoneNumber = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
				shopCloseWait = true;
			};
			CMD_Shop cmd_Shop = GUIMain.ShowCommonDialog(action, "CMD_Shop", null) as CMD_Shop;
			cmd_Shop.CloseWhenConsumed = true;
			float obstacleBGLocalY = obstacleBGTrans.localPosition.y;
			cmd_Shop.SetOnOpened(delegate(int a)
			{
				obstacleBGTrans.SetLocalY(-5000f);
			});
			cmd_Shop.SetHideGUIAction(delegate
			{
				obstacleBGTrans.SetLocalY(obstacleBGLocalY);
			});
			cmd_Shop.VirtualUsedStoneNum = base.battleStateData.turnUseDigiStoneCount;
		}
		else
		{
			Action<int> action2 = delegate(int a)
			{
				shopCloseWait = true;
			};
			CMD_Alert cmd_Alert = GUIMain.ShowCommonDialog(action2, "CMD_Alert", null) as CMD_Alert;
			cmd_Alert.SetDisplayButton(CMD_Alert.DisplayButton.CLOSE);
			cmd_Alert.Title = StringMaster.GetString("Maintenance-03");
			cmd_Alert.Info = StringMaster.GetString("Maintenance-04");
		}
		while (!shopCloseWait)
		{
			yield return null;
		}
		base.battleStateData.isEnableBackKeySystem = true;
		base.battleStateData.isShowShop = false;
		yield break;
	}

	public GameObject GetCharacterPrefab(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetCharacterPrefab(id);
		}
		BattleDebug.Log("----- モンスターAB単体取得 id[" + id + "]: 開始");
		string filePath = MonsterObject.GetFilePath(id);
		GameObject gameObject = AssetDataMng.Instance().LoadObject(filePath, null, true) as GameObject;
		if (gameObject != null)
		{
			BattleDebug.Log("----- モンスターAB単体取得 id[" + id + "]: 完了");
			return gameObject;
		}
		global::Debug.LogError("キャラクターデータ(PlayerStatus)が見つかりません. (" + id + ")");
		return null;
	}

	public Sprite GetCharacterThumbnail(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetCharacterThumbnail(id);
		}
		BattleDebug.Log("----- モンスターアイコンAB単体取得 id[" + id + "]: 開始");
		string monsterIconPathByIconId = GUIMonsterIcon.GetMonsterIconPathByIconId(id);
		Texture2D texture2D = AssetDataMng.Instance().LoadObject(monsterIconPathByIconId, null, true) as Texture2D;
		Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
		if (sprite != null)
		{
			BattleDebug.Log("----- モンスターアイコンAB単体取得 id[" + id + "]: 完了");
			return sprite;
		}
		global::Debug.LogError("キャラクターサムネイルデータが見つかりません. (" + id + ")");
		return null;
	}

	public GameObject GetInvocationEffectPrefab(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetInvocationEffectPrefab(id);
		}
		BattleDebug.Log("----- 発動スキルエフェクトAB単体取得 id[" + id + "]: 開始");
		string uniqueSkillPrefabPathByAttackEffectId = CommonResourcesDataMng.Instance().GetUniqueSkillPrefabPathByAttackEffectId(id);
		GameObject gameObject = AssetDataMng.Instance().LoadObject(uniqueSkillPrefabPathByAttackEffectId, null, true) as GameObject;
		if (gameObject != null)
		{
			BattleDebug.Log("----- 発動スキルエフェクトAB単体取得 id[" + id + "]: 完了");
			return gameObject;
		}
		global::Debug.LogError("InvocationEffectが見つかりません. (" + id + ")");
		return null;
	}

	public GameObject GetPassiveEffectPrefab(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetPassiveEffectPrefab(id);
		}
		BattleDebug.Log("----- 受動スキルエフェクトAB単体取得 id[" + id + "]: 開始");
		string commonSkillPrefabPathByAttackEffectId = CommonResourcesDataMng.Instance().GetCommonSkillPrefabPathByAttackEffectId(id);
		GameObject gameObject = AssetDataMng.Instance().LoadObject(commonSkillPrefabPathByAttackEffectId, null, true) as GameObject;
		if (gameObject != null)
		{
			BattleDebug.Log("----- 受動スキルエフェクトAB単体取得 id[" + id + "]: 完了");
			return gameObject;
		}
		global::Debug.LogError("PassiveEffectが見つかりません. (" + id + ")");
		return null;
	}

	public GameObject GetHitEffectPrefab(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetHitEffectPrefab(id);
		}
		BattleDebug.Log("----- ヒットエフェクトAB単体取得 id[" + id + "]: 開始");
		string hitEffectPrefabPathByHitEffectId = CommonResourcesDataMng.Instance().GetHitEffectPrefabPathByHitEffectId(id);
		GameObject gameObject = AssetDataMng.Instance().LoadObject(hitEffectPrefabPathByHitEffectId, null, true) as GameObject;
		if (gameObject != null)
		{
			BattleDebug.Log("----- ヒットエフェクトAB単体取得 id[" + id + "]: 完了");
			return gameObject;
		}
		global::Debug.LogError("HitEffectが見つかりません. (" + id + ")");
		return null;
	}

	public GameObject GetAlwaysEffectPrefab(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetAlwaysEffectPrefab(id);
		}
		BattleDebug.Log("----- 常設エフェクトAB単体取得 id[" + id + "]: 開始");
		string alwaysEffectPrefabPathByAlwaysEffectId = CommonResourcesDataMng.Instance().GetAlwaysEffectPrefabPathByAlwaysEffectId(id);
		GameObject gameObject = AssetDataMng.Instance().LoadObject(alwaysEffectPrefabPathByAlwaysEffectId, null, true) as GameObject;
		if (gameObject != null)
		{
			BattleDebug.Log("----- 常設エフェクトAB単体取得 id[" + id + "]: 完了");
			return gameObject;
		}
		global::Debug.LogError("AlwaysEffectが見つかりません. (" + id + ")");
		return null;
	}

	public GameObject GetStagePrefab(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetStagePrefab(id);
		}
		BattleDebug.Log("----- ステージAB単体取得 id[" + id + "]: 開始");
		string stagePrefabPathByAttackEffectId = CommonResourcesDataMng.Instance().GetStagePrefabPathByAttackEffectId(id);
		GameObject gameObject = AssetDataMng.Instance().LoadObject(stagePrefabPathByAttackEffectId, null, true) as GameObject;
		if (gameObject != null)
		{
			BattleDebug.Log("----- ステージAB単体取得 id[" + id + "]: 完了");
			return gameObject;
		}
		global::Debug.LogError("Stageが見つかりません. (" + id + ")");
		return null;
	}

	public GameObject GetSpawnPointPrefab(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetSpawnPointPrefab(id);
		}
		BattleDebug.Log("----- 出現ポイントAB単体取得 id[" + id + "]: 開始");
		string spawnPointPrefabPathBySpawnPointId = CommonResourcesDataMng.Instance().GetSpawnPointPrefabPathBySpawnPointId(id);
		GameObject gameObject = AssetDataMng.Instance().LoadObject(spawnPointPrefabPathBySpawnPointId, null, true) as GameObject;
		if (gameObject != null)
		{
			BattleDebug.Log("----- 出現ポイントAB単体取得 id[" + id + "]: 完了");
			return gameObject;
		}
		global::Debug.LogError("SpawnPointが見つかりません. (" + id + ")");
		return null;
	}

	public GameObject GetCameraMotionPrefab(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetCameraMotionPrefab(id);
		}
		BattleDebug.Log("----- カメラモーションAB単体取得 id[" + id + "]: 開始");
		string cameraMotionPrefabPathByCameraId = CommonResourcesDataMng.Instance().GetCameraMotionPrefabPathByCameraId(id);
		GameObject gameObject = AssetDataMng.Instance().LoadObject(cameraMotionPrefabPathByCameraId, null, true) as GameObject;
		if (gameObject != null)
		{
			BattleDebug.Log("----- カメラモーションAB単体取得 id[" + id + "]: 完了");
			return gameObject;
		}
		global::Debug.LogError("CameraMotionが見つかりません. (" + id + ")");
		return null;
	}
}
