using AdventureScene;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAdventureSceneManager
{
	private GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene[] worldDungeonAdventureScenes;

	private Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary;

	private bool currentSpeed2x;

	public BattleAdventureSceneManager(GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene[] worldDungeonAdventureScenes)
	{
		this.worldDungeonAdventureScenes = worldDungeonAdventureScenes;
		this.dictionary = new Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>>();
		this.dictionary.Add(BattleAdventureSceneManager.TriggerType.DigimonEntryStart, new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteDigimonEntry));
		this.dictionary.Add(BattleAdventureSceneManager.TriggerType.DigimonEntryEnd, new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteDigimonEntry));
		this.dictionary.Add(BattleAdventureSceneManager.TriggerType.WaveStart, new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteWave));
		this.dictionary.Add(BattleAdventureSceneManager.TriggerType.WaveEnd, new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteWave));
		this.dictionary.Add(BattleAdventureSceneManager.TriggerType.RoundStart, new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteRound));
		this.dictionary.Add(BattleAdventureSceneManager.TriggerType.RoundEnd, new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteRound));
		this.dictionary.Add(BattleAdventureSceneManager.TriggerType.SkillStart, new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteSkill));
		this.dictionary.Add(BattleAdventureSceneManager.TriggerType.SkillEnd, new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteSkill));
		this.dictionary.Add(BattleAdventureSceneManager.TriggerType.SkillHitStart, new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteSkillHit));
		this.dictionary.Add(BattleAdventureSceneManager.TriggerType.SkillHitEnd, new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteSkillHit));
		this.dictionary.Add(BattleAdventureSceneManager.TriggerType.WinStart, new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteWin));
		this.dictionary.Add(BattleAdventureSceneManager.TriggerType.WinEnd, new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteWin));
		this.dictionary.Add(BattleAdventureSceneManager.TriggerType.LoseStart, new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteLose));
		this.dictionary.Add(BattleAdventureSceneManager.TriggerType.LoseEnd, new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteLose));
	}

	public bool isUpdate { get; private set; }

	public IEnumerator Update()
	{
		if (!this.isUpdate)
		{
			yield break;
		}
		bool isEnd = false;
		IEnumerator syncFunction = this.SyncFunction();
		while (!isEnd)
		{
			bool isSyncUpdate = syncFunction.MoveNext();
			isEnd = (!this.isUpdate && !isSyncUpdate);
			yield return null;
		}
		yield break;
	}

	private IEnumerator SyncFunction()
	{
		BattleStateManager battleStateManager = BattleStateManager.current;
		while (battleStateManager.battleMode == BattleMode.Multi || battleStateManager.battleMode == BattleMode.PvP)
		{
			float time = 10f;
			do
			{
				time -= Time.deltaTime;
				yield return null;
			}
			while (time > 0f);
			if (battleStateManager.multiBasicFunction.IsOwner)
			{
				IEnumerator sendAdventureSceneData = battleStateManager.multiBasicFunction.SendAdventureSceneData();
				while (sendAdventureSceneData.MoveNext())
				{
					yield return null;
				}
				if (battleStateManager.multiBasicFunction.isAdventureSceneAllEnd)
				{
					break;
				}
			}
			else
			{
				IEnumerator waitAllPlayers = battleStateManager.multiBasicFunction.WaitAllPlayers(TCPMessageType.AdventureScene);
				while (waitAllPlayers.MoveNext())
				{
					yield return null;
				}
				if (battleStateManager.multiBasicFunction.isAdventureSceneAllEnd)
				{
					break;
				}
			}
		}
		yield break;
	}

	public void OnTrigger(BattleAdventureSceneManager.TriggerType triggerType)
	{
		if (this.worldDungeonAdventureScenes == null)
		{
			return;
		}
		this.isUpdate = false;
		foreach (GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene worldDungeonAdventureScene in this.worldDungeonAdventureScenes)
		{
			if (triggerType == (BattleAdventureSceneManager.TriggerType)worldDungeonAdventureScene.adventureTrigger.ToInt32())
			{
				bool flag = this.dictionary[triggerType](worldDungeonAdventureScene);
				if (flag)
				{
					this.isUpdate = true;
					ClassSingleton<AdventureSceneController>.Instance.Ready(worldDungeonAdventureScene.adventureValue, new Action(this.BeginAction), new Action(this.EndAction));
					ClassSingleton<AdventureSceneController>.Instance.Start();
					break;
				}
			}
		}
	}

	private void SetActive(bool value)
	{
		BattleStateManager current = BattleStateManager.current;
		current.hierarchyData.cameraObject.gameObject.SetActive(value);
	}

	private void BeginAction()
	{
		this.SetActive(false);
		this.currentSpeed2x = BattleStateManager.current.hierarchyData.on2xSpeedPlay;
		BattleStateManager.current.time.SetPlaySpeed(false, false);
	}

	private void EndAction()
	{
		UnityEngine.Debug.LogError("EndAction");
		this.isUpdate = false;
		this.SetActive(true);
		BattleStateManager.current.time.SetPlaySpeed(this.currentSpeed2x, false);
	}

	private static bool CheckExecuteDigimonEntry(GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene data)
	{
		if (string.IsNullOrEmpty(data.adventureTriggerValue) || data.adventureTriggerValue == "0")
		{
			return true;
		}
		BattleStateManager current = BattleStateManager.current;
		int num = Math.Max(current.battleStateData.currentWaveNumber, 1);
		return num == data.adventureTriggerValue.ToInt32();
	}

	private static bool CheckExecuteWave(GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene data)
	{
		if (string.IsNullOrEmpty(data.adventureTriggerValue) || data.adventureTriggerValue == "0")
		{
			return true;
		}
		BattleStateManager current = BattleStateManager.current;
		string[] array = data.adventureTriggerValue.Split(new char[]
		{
			','
		});
		int num = Math.Max(current.battleStateData.currentWaveNumber, 1);
		if (array.Length == 1)
		{
			int num2 = array[0].ToInt32();
			if (num == num2)
			{
				return true;
			}
		}
		else
		{
			int num3 = array[0].ToInt32();
			int num4 = array[1].ToInt32();
			if (num == num3 && current.battleStateData.currentRoundNumber == num4)
			{
				return true;
			}
		}
		return false;
	}

	private static bool CheckExecuteRound(GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene data)
	{
		if (string.IsNullOrEmpty(data.adventureTriggerValue) || data.adventureTriggerValue == "0")
		{
			return true;
		}
		BattleStateManager current = BattleStateManager.current;
		int num = data.adventureTriggerValue.ToInt32();
		return current.battleStateData.currentRoundNumber == num;
	}

	private static bool CheckExecuteSkill(GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene data)
	{
		return true;
	}

	private static bool CheckExecuteSkillHit(GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene data)
	{
		if (string.IsNullOrEmpty(data.adventureTriggerValue) || data.adventureTriggerValue == "0")
		{
			return true;
		}
		if (data.adventureTriggerValue == "1")
		{
			BattleStateManager current = BattleStateManager.current;
			CharacterStateControl[] totalCharacters = current.battleStateData.GetTotalCharacters();
			foreach (CharacterStateControl characterStateControl in totalCharacters)
			{
				if (!characterStateControl.isDied && characterStateControl.isDiedJustBefore)
				{
					return true;
				}
			}
		}
		return false;
	}

	private static bool CheckExecuteWin(GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene data)
	{
		return true;
	}

	private static bool CheckExecuteLose(GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene data)
	{
		if (string.IsNullOrEmpty(data.adventureTriggerValue) || data.adventureTriggerValue == "0")
		{
			return true;
		}
		if (data.adventureTriggerValue == "1")
		{
			BattleStateManager current = BattleStateManager.current;
			if (current.battleStateData.totalRoundNumber > current.hierarchyData.limitRound)
			{
				return true;
			}
		}
		return false;
	}

	public enum TriggerType
	{
		DigimonEntryStart = 1,
		DigimonEntryEnd,
		WaveStart,
		WaveEnd,
		RoundStart,
		RoundEnd,
		SkillStart,
		SkillEnd,
		SkillHitStart,
		SkillHitEnd,
		WinStart,
		WinEnd,
		LoseStart,
		LoseEnd
	}
}
