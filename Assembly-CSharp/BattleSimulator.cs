using BattleStateMachineInternal;
using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BattleSimulator : MonoBehaviour
{
	[SerializeField]
	private string _areaName = string.Empty;

	[SerializeField]
	protected string[] _usePlayerCharactersId = new string[]
	{
		string.Empty,
		string.Empty,
		string.Empty
	};

	[SerializeField]
	protected BattleWave[] _battleWaves = new BattleWave[]
	{
		new BattleWave()
	};

	[SerializeField]
	private string _useStageId;

	[SerializeField]
	private int _digiStoneNumber = 5;

	[SerializeField]
	private int _limitRound = -1;

	private BattleStateManager battleStateManager;

	private void Awake()
	{
		this.battleStateManager = UnityEngine.Object.FindObjectOfType<BattleStateManager>();
		if (BattleStateManager.onAutoServerConnect)
		{
			this.battleStateManager.BattleTrigger();
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		StringMaster.Initialize();
		GameObject gameObject = new GameObject("MasterDataMng");
		gameObject.AddComponent<MasterDataMng>();
	}

	private void Start()
	{
		this.ApplyBattleHierarchy(this.battleStateManager.hierarchyData, false, 10);
		this.battleStateManager.BattleTrigger();
	}

	protected virtual void ApplyBattleHierarchy(BattleStateHierarchyData hierarchyData, bool onAutoPlay = false, int continueWaitSecond = 10)
	{
		hierarchyData.batteWaves = this._battleWaves;
		hierarchyData.digiStoneNumber = this._digiStoneNumber;
		hierarchyData.useStageId = this._useStageId;
		hierarchyData.areaName = this._areaName;
		hierarchyData.leaderCharacter = 0;
		hierarchyData.limitRound = this._limitRound;
		int onAutoPlay2 = 0;
		if (onAutoPlay)
		{
			onAutoPlay2 = 2;
		}
		hierarchyData.onAutoPlay = onAutoPlay2;
		hierarchyData.usePlayerCharacters = new PlayerStatus[this._usePlayerCharactersId.Length];
		for (int i = 0; i < hierarchyData.usePlayerCharacters.Length; i++)
		{
			PlayerStatus playerStatus = ResourcesPath.GetPlayerStatus(this._usePlayerCharactersId[i]);
			List<string> list = new List<string>();
			list.Add("public_attack");
			foreach (string text in playerStatus.skillIds)
			{
				if (text != "public_attack")
				{
					list.Add(text);
				}
			}
			playerStatus.skillIds = list.ToArray();
			hierarchyData.usePlayerCharacters[i] = playerStatus;
			hierarchyData.AddCharacterDatas(playerStatus.groupId, ResourcesPath.GetCharacterData(playerStatus.groupId));
			hierarchyData.AddLeaderSkillStatus(playerStatus.leaderSkillId, ResourcesPath.GetLeaderSkillStatus(playerStatus.leaderSkillId));
			hierarchyData.AddSkillStatus("public_attack", ResourcesPath.GetSkillStatus("public_attack"));
			hierarchyData.AddSkillStatus(playerStatus.skillIds[1], ResourcesPath.GetSkillStatus(playerStatus.skillIds[1]));
			hierarchyData.AddSkillStatus(playerStatus.skillIds[2], ResourcesPath.GetSkillStatus(playerStatus.skillIds[2]));
		}
		foreach (BattleWave battleWave in hierarchyData.batteWaves)
		{
			battleWave.enemyStatuses = new CharacterStatus[battleWave.useEnemiesId.Length];
			for (int l = 0; l < battleWave.useEnemiesId.Length; l++)
			{
				string id = battleWave.useEnemiesId[l];
				EnemyStatus enemyStatus = ResourcesPath.GetEnemyStatus(id);
				battleWave.enemyStatuses[l] = enemyStatus;
				List<string> list2 = new List<string>();
				list2.Add("public_attack");
				list2.AddRange(enemyStatus.enemyAiPattern.GetAllSkillID());
				battleWave.enemyStatuses[l].skillIds = list2.ToArray();
				hierarchyData.AddCharacterDatas(enemyStatus.groupId, ResourcesPath.GetCharacterData(enemyStatus.groupId));
				hierarchyData.AddLeaderSkillStatus(enemyStatus.leaderSkillId, ResourcesPath.GetLeaderSkillStatus(enemyStatus.leaderSkillId));
				foreach (string text2 in battleWave.enemyStatuses[l].skillIds)
				{
					hierarchyData.AddSkillStatus(text2, ResourcesPath.GetSkillStatus(text2));
				}
			}
		}
	}
}
