using BattleStateMachineInternal;
using Enemy.AI;
using Enemy.DropItem;
using Master;
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
	public Dictionary<int, PlayerStatus> _cachedPlayerStatus = new Dictionary<int, PlayerStatus>();

	public Dictionary<int, EnemyStatus> _cachedEnemyStatus = new Dictionary<int, EnemyStatus>();

	public Dictionary<string, CharacterDatas> _cachedCharacterDatas = new Dictionary<string, CharacterDatas>();

	public Dictionary<string, SkillStatus> _cachedSkillStatus = new Dictionary<string, SkillStatus>();

	public Dictionary<string, LeaderSkillStatus> _cachedLeaderSkillStatus = new Dictionary<string, LeaderSkillStatus>();

	public Dictionary<string, GameWebAPI.RespDataMA_GetSkillM.SkillM> _cachedLeaderSkillM = new Dictionary<string, GameWebAPI.RespDataMA_GetSkillM.SkillM>();

	public Dictionary<string, Tolerance> _cachedTolerance = new Dictionary<string, Tolerance>();

	public Dictionary<string, ExtraEffectStatus> _cachedExtraEffectStatus = new Dictionary<string, ExtraEffectStatus>();

	public GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM _cachedWorldDungeonM;

	public string _startId;

	public string[] _userMonsterId;

	public override void BattleAwakeInitialize()
	{
	}

	public override void BattleTriggerInitialize()
	{
		if (base.onServerConnect)
		{
			if (base.stateManager.battleMode == BattleMode.PvP)
			{
				base.stateManager.pvpFunction.GetBattleData();
			}
			else if (base.stateManager.battleMode == BattleMode.Multi)
			{
				base.stateManager.multiFunction.GetBattleData();
			}
			else
			{
				this.GetBattleData();
			}
		}
	}

	public void GetBattleData()
	{
		GameWebAPI.RespDataWD_DungeonStart respDataWD_DungeonStart = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart;
		GameWebAPI.WD_Req_DngResult wd_ReqDngResult = DataMng.Instance().WD_ReqDngResult;
		this._startId = respDataWD_DungeonStart.startId;
		wd_ReqDngResult.startId = this._startId;
		wd_ReqDngResult.dungeonId = respDataWD_DungeonStart.worldDungeonId;
		this._cachedSkillStatus.Add(base.stateManager.publicAttackSkillId.Trim(), this.SkillMToSkillStatus(base.stateManager.publicAttackSkillId));
		this._userMonsterId = respDataWD_DungeonStart.deck.userMonsterIds;
		base.hierarchyData.usePlayerCharactersId = new string[this._userMonsterId.Length];
		for (int i = 0; i < base.hierarchyData.usePlayerCharactersId.Length; i++)
		{
			base.hierarchyData.usePlayerCharactersId[i] = i.ToString();
			this._cachedPlayerStatus.Add(i, this.DeckToPlayerStatus(respDataWD_DungeonStart.deck.userMonsterIds[i]));
		}
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonM = this.GetWorldDungeonM(respDataWD_DungeonStart.worldDungeonId);
		base.hierarchyData.useStageId = worldDungeonM.background;
		base.hierarchyData.areaName = worldDungeonM.name;
		base.hierarchyData.limitRound = int.Parse(worldDungeonM.limitRound);
		base.hierarchyData.speedClearRound = int.Parse(worldDungeonM.speedClearRound);
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
		foreach (GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 in worldStageM)
		{
			if (worldDungeonM.worldStageId == worldStageM2.worldStageId)
			{
				base.hierarchyData.areaId = worldStageM2.worldAreaId.ToInt32();
				break;
			}
		}
		base.hierarchyData.extraEffectsId = this.SetWorldDungeonExtraEffect(respDataWD_DungeonStart.worldDungeonId);
		base.hierarchyData.battleNum = worldDungeonM.battleNum;
		base.hierarchyData.batteWaves = this.DungeonFloorToBattleWave(respDataWD_DungeonStart.dungeonFloor, respDataWD_DungeonStart.worldDungeonId);
		base.hierarchyData.digiStoneNumber = DataMng.Instance().GetStone();
		base.battleStateData.beforeConfirmDigiStoneNumber = base.hierarchyData.digiStoneNumber;
		base.hierarchyData.isPossibleContinue = (int.Parse(worldDungeonM.canContinue) == 1);
		string[] array2 = new string[this._cachedLeaderSkillM.Keys.Count];
		this._cachedLeaderSkillM.Keys.CopyTo(array2, 0);
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

	public void SetBattleResult(DataMng.ClearFlag onClearBattle, bool[] characterAliveFlags, bool isRetire, int[][] enemyAliveList)
	{
		DataMng.Instance().SetClearFlag(onClearBattle);
		DataMng.Instance().SetAliveFlag(characterAliveFlags);
		DataMng.Instance().SetEnemyAliveFlag(enemyAliveList);
		this.ApplyResultDigiStone();
		if (!isRetire && onClearBattle != DataMng.ClearFlag.Win)
		{
			if (!base.hierarchyData.useInitialIntroduction)
			{
				return;
			}
			PlayerPrefs.SetInt("BATTLE_LOSE_COUNT", Mathf.Clamp(base.hierarchyData.initialIntroductionIndex + 1, 0, base.hierarchyData.maxInitialIntroductionIndex + 1));
			PlayerPrefs.Save();
		}
	}

	private void SetSkillSeeds(int[,] seeds)
	{
		int num = 0;
		foreach (KeyValuePair<string, SkillStatus> keyValuePair in this._cachedSkillStatus)
		{
			int num2 = 0;
			foreach (AffectEffectProperty affectEffectProperty in keyValuePair.Value.affectEffect)
			{
				affectEffectProperty.SetRandomSeed(seeds[num, num2]);
				num2++;
			}
			num++;
		}
	}

	private void ApplyResultDigiStone()
	{
		DataMng.Instance().AddStone(base.battleStateData.beforeConfirmDigiStoneNumber - DataMng.Instance().GetStone());
	}

	public BattleWave[] DungeonFloorToBattleWave(GameWebAPI.RespDataWD_DungeonStart.DungeonFloor[] floor, string worldDungeonId)
	{
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonM = this.GetWorldDungeonM(worldDungeonId);
		GameWebAPI.RespDataWD_DungeonStart.DungeonFloor dungeonFloor = null;
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
			for (int j = 0; j < battleWave.useEnemiesId.Length; j++)
			{
				battleWave.useEnemiesId[j] = (i * 10 + j).ToString();
				EnemyType type = (EnemyType)floor[i].enemy[j].type;
				battleWave.enemiesBossFlag[j] = this.CheckBossFlag(type);
				battleWave.enemiesInfinityApFlag[j] = this.CheckInfinityAp(type);
				this._cachedEnemyStatus.Add(i * 10 + j, this.EnemyToEnemyStatus(floor[i].enemy[j]));
			}
			if (dungeonFloor != null)
			{
				battleWave.hpRevivalPercentage = ServerToBattleUtility.PermillionToPercentage(dungeonFloor.healingRate);
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
			array[i] = battleWave;
			dungeonFloor = floor[i];
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
		string monsterGroupId = monsterDataByUserMonsterID.monsterM.monsterGroupId;
		int arousal = ServerToBattleUtility.GetArousal(monsterDataByUserMonsterID.monsterM.rare.ToInt32() - 1);
		int friendshipLevel = ServerToBattleUtility.ServerValueToInt(monsterDataByUserMonsterID.userMonster.friendship);
		int hp = ServerToBattleUtility.ServerValueToInt(monsterDataByUserMonsterID.userMonster.hp);
		int attackPower = ServerToBattleUtility.ServerValueToInt(monsterDataByUserMonsterID.userMonster.attack);
		int defencePower = ServerToBattleUtility.ServerValueToInt(monsterDataByUserMonsterID.userMonster.defense);
		int specialAttackPower = ServerToBattleUtility.ServerValueToInt(monsterDataByUserMonsterID.userMonster.spAttack);
		int specialDefencePower = ServerToBattleUtility.ServerValueToInt(monsterDataByUserMonsterID.userMonster.spDefense);
		int speed = ServerToBattleUtility.ServerValueToInt(monsterDataByUserMonsterID.userMonster.speed);
		int maxAttackPower = ServerToBattleUtility.ServerValueToInt(monsterDataByUserMonsterID.monsterM.maxAttack);
		int maxDefencePower = ServerToBattleUtility.ServerValueToInt(monsterDataByUserMonsterID.monsterM.maxDefense);
		int maxSpecialAttackPower = ServerToBattleUtility.ServerValueToInt(monsterDataByUserMonsterID.monsterM.maxSpAttack);
		int maxSpecialDefencePower = ServerToBattleUtility.ServerValueToInt(monsterDataByUserMonsterID.monsterM.maxSpDefense);
		int maxSpeed = ServerToBattleUtility.ServerValueToInt(monsterDataByUserMonsterID.monsterM.speed);
		int level = ServerToBattleUtility.ServerValueToInt(monsterDataByUserMonsterID.userMonster.level);
		string resistanceId = monsterDataByUserMonsterID.monsterM.resistanceId;
		int luck = ServerToBattleUtility.ServerValueToInt(monsterDataByUserMonsterID.userMonster.luck);
		string uniqueSkillId = monsterDataByUserMonsterID.userMonster.uniqueSkillId;
		string commonSkillId = monsterDataByUserMonsterID.userMonster.commonSkillId;
		string text = monsterDataByUserMonsterID.userMonster.leaderSkillId.Equals("0") ? string.Empty : monsterDataByUserMonsterID.userMonster.leaderSkillId;
		string iconId = monsterDataByUserMonsterID.monsterM.iconId;
		Talent talent = new Talent(monsterDataByUserMonsterID.userMonster);
		FriendshipStatus friendshipStatus = new FriendshipStatus(friendshipLevel, maxAttackPower, maxDefencePower, maxSpecialAttackPower, maxSpecialDefencePower, maxSpeed);
		if (!this._cachedTolerance.ContainsKey(resistanceId.Trim()))
		{
			this._cachedTolerance.Add(resistanceId.Trim(), this.ResistanceToTolerance(resistanceId));
		}
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM data = monsterDataByUserMonsterID.AddResistanceFromMultipleTranceData();
		Tolerance tolerance = this.ResistanceToTolerance(data);
		List<int> list = new List<int>();
		if (monsterDataByUserMonsterID.userMonsterSlotInfo != null && monsterDataByUserMonsterID.userMonsterSlotInfo.equip != null)
		{
			foreach (GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip equip2 in monsterDataByUserMonsterID.userMonsterSlotInfo.equip)
			{
				GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipDataByUserChipId = ChipDataMng.GetUserChipDataByUserChipId(equip2.userChipId);
				list.Add(userChipDataByUserChipId.chipId);
			}
		}
		PlayerStatus result = new PlayerStatus(monsterGroupId, hp, attackPower, defencePower, specialAttackPower, specialDefencePower, speed, level, resistanceId, tolerance, luck, uniqueSkillId, commonSkillId, text, iconId, talent, arousal, friendshipStatus, list.ToArray());
		if (!this._cachedSkillStatus.ContainsKey(uniqueSkillId.Trim()))
		{
			this._cachedSkillStatus.Add(uniqueSkillId.Trim(), this.SkillMToSkillStatus(uniqueSkillId));
		}
		if (!this._cachedSkillStatus.ContainsKey(commonSkillId.Trim()))
		{
			this._cachedSkillStatus.Add(commonSkillId.Trim(), this.SkillMToSkillStatus(commonSkillId));
		}
		if (!this._cachedLeaderSkillStatus.ContainsKey(text.Trim()) && !text.Equals(string.Empty))
		{
			GameWebAPI.RespDataMA_GetSkillM.SkillM value;
			this._cachedLeaderSkillStatus.Add(text.Trim(), this.SkillMToLeaderSkillStatus(text, out value));
			this._cachedLeaderSkillM.Add(text.Trim(), value);
		}
		if (!this._cachedCharacterDatas.ContainsKey(monsterGroupId.Trim()))
		{
			this._cachedCharacterDatas.Add(monsterGroupId.Trim(), this.MonsterMToCharacterData(monsterGroupId));
		}
		return result;
	}

	private EnemyStatus EnemyToEnemyStatus(GameWebAPI.RespDataWD_DungeonStart.Enemy enemy)
	{
		GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterMasterByMonsterId = MonsterDataMng.Instance().GetMonsterMasterByMonsterId(enemy.monsterId);
		string monsterGroupId = monsterMasterByMonsterId.monsterGroupId;
		int hp = enemy.hp;
		int attack = enemy.attack;
		int defense = enemy.defense;
		int spAttack = enemy.spAttack;
		int spDefense = enemy.spDefense;
		int speed = enemy.speed;
		int level = enemy.level;
		string resistanceId = enemy.resistanceId;
		if (enemy.ai == null || enemy.ai.Length <= 0)
		{
			global::Debug.LogError("enemy.aiのマスタがおかしいです.");
			return null;
		}
		AICycle aicycle = ServerToBattleUtility.IntToAICycle(enemy.ai[0].type);
		List<AIActionClip> list = new List<AIActionClip>();
		List<AIActionPattern> list2 = new List<AIActionPattern>();
		for (int i = 0; i < enemy.ai.Length; i++)
		{
			int num = (aicycle != AICycle.fixableRotation) ? 0 : ServerToBattleUtility.ServerSeqIdToCpuSeqId(enemy.ai[i].priority);
			float minRange = (aicycle != AICycle.targetHpAltenation) ? 0f : ServerToBattleUtility.HundredToPercentage(enemy.ai[i].minHpRange);
			float maxRange = (aicycle != AICycle.targetHpAltenation) ? 0f : ServerToBattleUtility.HundredToPercentage(enemy.ai[i].maxHpRange);
			TargetSelectReference targetSelectReference = ServerToBattleUtility.IntToTargetSelectReference(enemy.ai[i].lookStatus);
			SelectingOrder selectingOrder = ServerToBattleUtility.IntToSelectingOrder(enemy.ai[i].lookType);
			float minValue = (targetSelectReference != TargetSelectReference.Hp) ? ((float)enemy.ai[i].invokeMinRange) : ServerToBattleUtility.HundredToPercentage(enemy.ai[i].invokeMinRange);
			float maxValue = (targetSelectReference != TargetSelectReference.Hp) ? ((float)enemy.ai[i].invokeMaxRange) : ServerToBattleUtility.HundredToPercentage(enemy.ai[i].invokeMaxRange);
			float num2 = ServerToBattleUtility.PermillionToPercentage(enemy.ai[i].minRate);
			float maxRange2 = ServerToBattleUtility.PermillionToPercentage(enemy.ai[i].maxRate);
			string skillId = enemy.ai[i].skillId;
			if (num2 <= 1f)
			{
				num2 = 0f;
			}
			if (!this._cachedSkillStatus.ContainsKey(skillId.Trim()))
			{
				this._cachedSkillStatus.Add(skillId.Trim(), this.SkillMToSkillStatus(skillId));
			}
			AIActionClip item = new AIActionClip(targetSelectReference, selectingOrder, minValue, maxValue, num2, maxRange2, skillId);
			bool flag = false;
			if (i < enemy.ai.Length - 1)
			{
				if (aicycle == AICycle.fixableRotation)
				{
					if (ServerToBattleUtility.ServerSeqIdToCpuSeqId(enemy.ai[i + 1].priority) != num)
					{
						flag = true;
					}
				}
				else if (enemy.ai[i + 1].minHpRange != enemy.ai[i + 1].minHpRange)
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			list.Add(item);
			if (flag)
			{
				list2.Add(new AIActionPattern(minRange, maxRange, list.ToArray()));
				list.Clear();
			}
		}
		EnemyAIPattern enemyAiPattern = new EnemyAIPattern(aicycle, list2.ToArray());
		int fixedMoney = enemy.fixedMoney;
		int fixedExp = enemy.fixedExp;
		List<ItemDropResult> list3 = new List<ItemDropResult>();
		if (enemy.drop != null)
		{
			foreach (GameWebAPI.RespDataWD_DungeonStart.Drop drop2 in enemy.drop)
			{
				DropBoxType dropBoxType = ServerToBattleUtility.IntToDropBoxType(drop2.dropBoxType);
				DropAssetType dropAssetType = ServerToBattleUtility.IntToDropAssetType(drop2.assetCategoryId);
				int assetNum = drop2.assetNum;
				ItemDropResult item2 = new ItemDropResult(dropBoxType, dropAssetType, assetNum);
				list3.Add(item2);
			}
		}
		else
		{
			ItemDropResult item3 = new ItemDropResult(false);
			list3.Add(item3);
		}
		int[] chipIdList = enemy.chipIdList;
		if (enemy.chipIdList == null)
		{
			chipIdList = new int[0];
		}
		EnemyStatus result = new EnemyStatus(monsterGroupId, hp, attack, defense, spAttack, spDefense, speed, level, resistanceId, enemyAiPattern, fixedMoney, fixedExp, list3, chipIdList);
		if (!this._cachedTolerance.ContainsKey(resistanceId.Trim()))
		{
			this._cachedTolerance.Add(resistanceId.Trim(), this.ResistanceToTolerance(resistanceId));
		}
		if (!this._cachedCharacterDatas.ContainsKey(monsterGroupId.Trim()))
		{
			this._cachedCharacterDatas.Add(monsterGroupId.Trim(), this.MonsterMToCharacterData(monsterGroupId));
		}
		return result;
	}

	public SkillStatus SkillMToSkillStatus(string skillId)
	{
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
					return new SkillStatus(attackEffect, soundEffect, skillType, name, description, target, effectNumbers, needAp, affectEffectPropertyList.ToArray());
				}
			}
		}
		global::Debug.LogError("SkillStatusの生成に失敗しました. (" + skillId + ")");
		return null;
	}

	public LeaderSkillStatus SkillMToLeaderSkillStatus(string leaderSkillId, out GameWebAPI.RespDataMA_GetSkillM.SkillM leaderSkillM)
	{
		GameWebAPI.RespDataMA_GetSkillM.SkillM[] skillM = MasterDataMng.Instance().RespDataMA_SkillM.skillM;
		LeaderSkillStatus leaderSkillStatus = null;
		leaderSkillM = null;
		for (int i = 0; i < skillM.Length; i++)
		{
			if (ServerToBattleUtility.GetIsLeaderSkill(skillM[i].type))
			{
				if (leaderSkillId.Equals(skillM[i].skillId))
				{
					string name = skillM[i].name;
					string description = skillM[i].description;
					GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM[] skillDetailM = MasterDataMng.Instance().RespDataMA_SkillDetailM.skillDetailM;
					leaderSkillM = skillM[i];
					for (int j = 0; j < skillDetailM.Length; j++)
					{
						if (leaderSkillId.Equals(skillDetailM[j].skillId))
						{
							LeaderSkillType leaderSkillType = ServerToBattleUtility.IntToLeaderSkillType(skillDetailM[j].effectType);
							float hpFollowingPercent = (!ServerToBattleUtility.GetOnHpFollowingLeaderSkill(leaderSkillType)) ? 0f : ServerToBattleUtility.PermillionToPercentage(skillDetailM[j].effect2);
							float upPercent = ServerToBattleUtility.PermillionToPercentage(skillDetailM[j].effect1);
							Tolerance tolerance;
							if (leaderSkillType == LeaderSkillType.ToleranceUp)
							{
								tolerance = new Tolerance((Strength)skillDetailM[j].effect3, (Strength)skillDetailM[j].effect4, (Strength)skillDetailM[j].effect5, (Strength)skillDetailM[j].effect6, (Strength)skillDetailM[j].effect7, (Strength)skillDetailM[j].effect8, (Strength)skillDetailM[j].effect9, (Strength)skillDetailM[j].effect10, (Strength)skillDetailM[j].effect11, (Strength)skillDetailM[j].effect12, (Strength)skillDetailM[j].effect13, (Strength)skillDetailM[j].effect14, (Strength)skillDetailM[j].effect15, (Strength)skillDetailM[j].effect16);
							}
							else
							{
								tolerance = Tolerance.GetNutralTolerance();
							}
							leaderSkillStatus = new LeaderSkillStatus(name, description, leaderSkillType, hpFollowingPercent, upPercent, tolerance);
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

	public CharacterDatas MonsterMToCharacterData(string monsterGroupId)
	{
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMasterByMonsterGroupId = MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(monsterGroupId);
		string monsterName = monsterGroupMasterByMonsterGroupId.monsterName;
		Species species = ServerToBattleUtility.IntToSpecies(monsterGroupMasterByMonsterGroupId.tribe);
		EvolutionStep evolutionStep = ServerToBattleUtility.IntToEvolutionStep(monsterGroupMasterByMonsterGroupId.growStep);
		return new CharacterDatas(monsterName, species, evolutionStep);
	}

	public Tolerance ResistanceToTolerance(string resistanceId)
	{
		GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM[] monsterResistanceM = MasterDataMng.Instance().RespDataMA_MonsterResistanceM.monsterResistanceM;
		Tolerance tolerance = null;
		for (int i = 0; i < monsterResistanceM.Length; i++)
		{
			if (monsterResistanceM[i].monsterResistanceId.Equals(resistanceId))
			{
				Strength noneValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].none);
				Strength redValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].fire);
				Strength blueValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].water);
				Strength yellowValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].thunder);
				Strength greenValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].nature);
				Strength whiteValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].light);
				Strength blackValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].dark);
				Strength poisonValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].poison);
				Strength confusionValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].confusion);
				Strength paralysisValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].paralysis);
				Strength sleepValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].sleep);
				Strength stunValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].stun);
				Strength skillLockValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].skillLock);
				Strength instantDeathValue = ServerToBattleUtility.IntToStrength(monsterResistanceM[i].death);
				tolerance = new Tolerance(noneValue, redValue, blueValue, yellowValue, greenValue, whiteValue, blackValue, poisonValue, confusionValue, paralysisValue, sleepValue, stunValue, skillLockValue, instantDeathValue);
				break;
			}
		}
		if (tolerance == null)
		{
			global::Debug.LogError("Toleranceの生成に失敗しました. (" + resistanceId + ")");
			return null;
		}
		return tolerance;
	}

	public Tolerance ResistanceToTolerance(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM data)
	{
		Strength noneValue = ServerToBattleUtility.IntToStrength(data.none);
		Strength redValue = ServerToBattleUtility.IntToStrength(data.fire);
		Strength blueValue = ServerToBattleUtility.IntToStrength(data.water);
		Strength yellowValue = ServerToBattleUtility.IntToStrength(data.thunder);
		Strength greenValue = ServerToBattleUtility.IntToStrength(data.nature);
		Strength whiteValue = ServerToBattleUtility.IntToStrength(data.light);
		Strength blackValue = ServerToBattleUtility.IntToStrength(data.dark);
		Strength poisonValue = ServerToBattleUtility.IntToStrength(data.poison);
		Strength confusionValue = ServerToBattleUtility.IntToStrength(data.confusion);
		Strength paralysisValue = ServerToBattleUtility.IntToStrength(data.paralysis);
		Strength sleepValue = ServerToBattleUtility.IntToStrength(data.sleep);
		Strength stunValue = ServerToBattleUtility.IntToStrength(data.stun);
		Strength skillLockValue = ServerToBattleUtility.IntToStrength(data.skillLock);
		Strength instantDeathValue = ServerToBattleUtility.IntToStrength(data.death);
		return new Tolerance(noneValue, redValue, blueValue, yellowValue, greenValue, whiteValue, blackValue, poisonValue, confusionValue, paralysisValue, sleepValue, stunValue, skillLockValue, instantDeathValue);
	}

	public string[] SetWorldDungeonExtraEffect(string dungeonId)
	{
		IEnumerable<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> enumerable = MasterDataMng.Instance().RespDataMA_WorldDungeonExtraEffectM.worldDungeonExtraEffectM.SelectMany((GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM x) => MasterDataMng.Instance().RespDataMA_WorldDungeonExtraEffectManageM.worldDungeonExtraEffectManageM, (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM x, GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectManageM.WorldDungeonExtraEffectManageM y) => new
		{
			x,
			y
		}).Where(<>__TranspIdent0 => <>__TranspIdent0.x.worldDungeonExtraEffectId == <>__TranspIdent0.y.worldDungeonExtraEffectId && <>__TranspIdent0.y.worldDungeonId == dungeonId).Select(<>__TranspIdent0 => <>__TranspIdent0.x);
		this._cachedExtraEffectStatus.Clear();
		int num = 0;
		string[] array = new string[enumerable.Count<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>()];
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM worldDungeonExtraEffectM in enumerable)
		{
			this._cachedExtraEffectStatus.Add(num.ToString(), new ExtraEffectStatus(worldDungeonExtraEffectM));
			array[num] = num.ToString();
			num++;
		}
		return array;
	}

	public List<AffectEffectProperty> GetAffectEffectPropertyList(string skillId)
	{
		GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM[] skillDetailM = MasterDataMng.Instance().RespDataMA_SkillDetailM.skillDetailM;
		List<AffectEffectProperty> list = new List<AffectEffectProperty>();
		for (int i = 0; i < skillDetailM.Length; i++)
		{
			if (skillId.Equals(skillDetailM[i].skillId))
			{
				EffectTarget target = ServerToBattleUtility.IntToEffectTarget(skillDetailM[i].target);
				AffectEffect type = ServerToBattleUtility.IntToAffectEffect(skillDetailM[i].effectType);
				float hitRate = ServerToBattleUtility.PermillionToPercentage(skillDetailM[i].hitRate);
				EffectNumbers effectNumbers = ServerToBattleUtility.IntToEffectNumbers(skillDetailM[i].targetType);
				int effect = skillDetailM[i].effect1;
				int effect2 = skillDetailM[i].effect2;
				float num = ServerToBattleUtility.PermillionToPercentage(skillDetailM[i].effect3, false);
				float num2 = ServerToBattleUtility.PermillionToPercentage(skillDetailM[i].effect4, false);
				float num3 = ServerToBattleUtility.PermillionToPercentage(skillDetailM[i].effect5, false);
				float num4 = ServerToBattleUtility.PermillionToPercentage(skillDetailM[i].effect6, false);
				float num5 = ServerToBattleUtility.PermillionToPercentage(skillDetailM[i].effect7, false);
				float num6 = ServerToBattleUtility.PermillionToPercentage(skillDetailM[i].effect8, false);
				float num7 = ServerToBattleUtility.PermillionToPercentage(skillDetailM[i].effect9, false);
				bool useDrainAffectEffect = ServerToBattleUtility.GetUseDrainAffectEffect(skillDetailM[i].effectType);
				PowerType powerType = ServerToBattleUtility.GetPowerType(skillDetailM[i].effectType);
				TechniqueType techniqueType = ServerToBattleUtility.GetTechniqueType(skillDetailM[i].effectType);
				global::Attribute attribute = ServerToBattleUtility.IntToAttribute(skillDetailM[i].attribute);
				bool isMissThrough = skillDetailM[i].isMissTrough > 0;
				AffectEffectProperty item = new AffectEffectProperty(type, skillId.ToInt32(), hitRate, target, effectNumbers, new int[]
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
					num7
				}, useDrainAffectEffect, powerType, techniqueType, attribute, isMissThrough);
				list.Add(item);
			}
		}
		return list;
	}

	public GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM GetWorldDungeonM(string worldDungeonId)
	{
		if (this._cachedWorldDungeonM == null || !this._cachedWorldDungeonM.worldDungeonId.Equals(worldDungeonId))
		{
			GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM;
			GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM cachedWorldDungeonM = null;
			for (int i = 0; i < worldDungeonM.Length; i++)
			{
				if (worldDungeonM[i].worldDungeonId.Equals(worldDungeonId))
				{
					cachedWorldDungeonM = worldDungeonM[i];
					break;
				}
			}
			this._cachedWorldDungeonM = cachedWorldDungeonM;
		}
		return this._cachedWorldDungeonM;
	}

	public IEnumerator CharacterRevivalBeforeFunction(params int[] playerIndex)
	{
		if (base.stateManager.onEnableTutorial)
		{
			yield return true;
			yield break;
		}
		if (base.onServerConnect)
		{
			int[] continueCharacters = new int[playerIndex.Length];
			for (int i = 0; i < continueCharacters.Length; i++)
			{
				continueCharacters[i] = int.Parse(this._userMonsterId[playerIndex[i]]);
			}
			bool getResult = false;
			bool outResult = false;
			if (base.stateManager.battleMode == BattleMode.Multi)
			{
				yield return ClassSingleton<QuestData>.Instance.DungeonContinueMulti(int.Parse(this._startId), base.battleStateData.currentWaveNumberGUI, base.battleStateData.currentRoundNumber, continueCharacters, delegate(bool result)
				{
					getResult = true;
					outResult = result;
				});
			}
			else
			{
				yield return ClassSingleton<QuestData>.Instance.DungeonContinue(int.Parse(this._startId), base.battleStateData.currentWaveNumberGUI, base.battleStateData.currentRoundNumber, continueCharacters, delegate(bool result)
				{
					getResult = true;
					outResult = result;
				});
			}
			while (!getResult)
			{
				yield return null;
			}
			if (outResult)
			{
				int minusCalcedStone = playerIndex.Length + ((playerIndex.Length != base.battleStateData.playerCharacters.Length) ? 0 : 2);
				DataMng.Instance().AddStone(-minusCalcedStone);
				base.battleStateData.beforeConfirmDigiStoneNumber = DataMng.Instance().GetStone();
				yield return true;
			}
			else
			{
				global::Debug.LogError("復活に失敗しました.");
				yield return false;
			}
		}
		else
		{
			int minus = playerIndex.Length + ((playerIndex.Length != base.battleStateData.playerCharacters.Length) ? 0 : 2);
			base.battleStateData.beforeConfirmDigiStoneNumber -= minus;
			yield return true;
		}
		yield break;
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
			Action<int> Result = delegate(int a)
			{
				global::Debug.Log(base.battleStateData.beforeConfirmDigiStoneNumber + " / " + DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point);
				base.battleStateData.beforeConfirmDigiStoneNumber = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
				shopCloseWait = true;
			};
			CMD_Shop cs = GUIMain.ShowCommonDialog(Result, "CMD_Shop") as CMD_Shop;
			cs.CloseWhenConsumed = true;
			float obstacleBGLocalY = obstacleBGTrans.localPosition.y;
			cs.SetOnOpened(delegate(int a)
			{
				obstacleBGTrans.SetLocalY(-5000f);
			});
			cs.SetHideGUIAction(delegate
			{
				obstacleBGTrans.SetLocalY(obstacleBGLocalY);
			});
			cs.virtualAddStoneNum = -base.battleStateData.turnUseDigiStoneCount;
		}
		else
		{
			Action<int> Result2 = delegate(int a)
			{
				shopCloseWait = true;
			};
			CMD_Alert ca = GUIMain.ShowCommonDialog(Result2, "CMD_Alert") as CMD_Alert;
			ca.SetDisplayButton(CMD_Alert.DisplayButton.CLOSE);
			ca.Title = StringMaster.GetString("Maintenance-03");
			ca.Info = StringMaster.GetString("Maintenance-04");
		}
		while (!shopCloseWait)
		{
			yield return null;
		}
		base.battleStateData.isEnableBackKeySystem = true;
		base.battleStateData.isShowShop = false;
		yield break;
	}

	public string publicAttackSkillId
	{
		get
		{
			return base.stateManager.publicAttackSkillId;
		}
	}

	public void SetLoadingImage(bool isShow)
	{
		if (base.onServerConnect)
		{
			if (isShow)
			{
				if (!Loading.IsShow())
				{
					RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
					TipsLoading.Instance.StartTipsLoad(CMD_Tips.DISPLAY_PLACE.QuestToSoloBattle, false);
				}
			}
			else
			{
				TipsLoading.Instance.StopTipsLoad(false);
				RestrictionInput.EndLoad();
			}
		}
	}

	public void ApplyShowSpecificTrade()
	{
		if (base.onServerConnect)
		{
			base.stateManager.uiControl.ApplySpecificTrade(true);
			Action<int> action = delegate(int x)
			{
				base.stateManager.callAction.OnHideSpecificTrade();
			};
			CommonDialog commonDialog = GUIMain.ShowCommonDialog(action, "CMDWebWindow");
			((CMDWebWindow)commonDialog).TitleText = StringMaster.GetString("ShopRule-02");
			((CMDWebWindow)commonDialog).Url = WebAddress.EXT_ADR_TRADE;
			base.battleStateData.isShowSpecificTrade = true;
		}
	}

	public GameObject GetCharacterPrefab(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetCharacterPrefab(id);
		}
		BattleDebug.Log("----- モンスターAB単体取得 id[" + id + "]: 開始");
		string monsterCharaPathByMonsterGroupId = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterGroupId(id);
		GameObject gameObject = AssetDataMng.Instance().LoadObject(monsterCharaPathByMonsterGroupId, null, true) as GameObject;
		if (gameObject != null)
		{
			BattleDebug.Log("----- モンスターAB単体取得 id[" + id + "]: 完了");
			return gameObject;
		}
		global::Debug.LogError("キャラクターデータ(PlayerStatus)が見つかりません. (" + id + ")");
		return null;
	}

	public PlayerStatus GetPlayerStatus(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetPlayerStatus(id);
		}
		int key;
		if (!int.TryParse(id, out key))
		{
			global::Debug.LogErrorFormat("{0}というキャラクターデータのidは正しくありません.正しいマスターを入れてください.", new object[]
			{
				id
			});
			return null;
		}
		PlayerStatus result;
		if (this._cachedPlayerStatus.TryGetValue(key, out result))
		{
			return result;
		}
		global::Debug.LogError("キャラクターデータ(PlayerStatus)が見つかりません. (" + id + ")");
		return null;
	}

	public EnemyStatus GetEnemyStatus(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetEnemyStatus(id);
		}
		EnemyStatus result;
		if (this._cachedEnemyStatus.TryGetValue(int.Parse(id), out result))
		{
			return result;
		}
		global::Debug.LogError("キャラクターデータ(EnemyStatus)が見つかりません. (" + id + ")");
		return null;
	}

	public Tolerance GetToleranceStatus(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetToleranceStatus(id);
		}
		Tolerance result;
		if (this._cachedTolerance.TryGetValue(id.Trim(), out result))
		{
			return result;
		}
		global::Debug.LogError("耐性データが見つかりません. (" + id + ")");
		return null;
	}

	public ExtraEffectStatus GetExtraEffectStatus(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetExtraEffectStatus(id);
		}
		ExtraEffectStatus result;
		if (this._cachedExtraEffectStatus.TryGetValue(id.Trim(), out result))
		{
			return result;
		}
		global::Debug.LogError("エリア効果データが見つかりません. (" + id + ")");
		return null;
	}

	public Sprite GetCharacterThumbnail(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetCharacterThumbnail(id);
		}
		BattleDebug.Log("----- モンスターアイコンAB単体取得 id[" + id + "]: 開始");
		string monsterIconPathByIconId = MonsterDataMng.Instance().GetMonsterIconPathByIconId(id);
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

	public CharacterDatas GetCharacterData(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetCharacterData(id);
		}
		CharacterDatas result;
		if (this._cachedCharacterDatas.TryGetValue(id.Trim(), out result))
		{
			return result;
		}
		global::Debug.LogError("キャラクターデータ(CharacterDatas)が見つかりません. (" + id + ")");
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

	public SkillStatus GetSkillStatus(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetSkillStatus(id);
		}
		SkillStatus result;
		if (this._cachedSkillStatus.TryGetValue(id.Trim(), out result))
		{
			return result;
		}
		global::Debug.LogError("スキルステータスデータが見つかりません. (" + id + ")");
		return null;
	}

	public LeaderSkillStatus GetLeaderSkillStatus(string id)
	{
		if (!base.onServerConnect)
		{
			return ResourcesPath.GetLeaderSkillStatus(id);
		}
		LeaderSkillStatus result;
		if (this._cachedLeaderSkillStatus.TryGetValue(id.Trim(), out result))
		{
			return result;
		}
		global::Debug.LogError("リーダースキルステータスデータが見つかりません. (" + id + ")");
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
