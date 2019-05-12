using AdventureScene;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleAdventureSceneManager
{
	private GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene[] worldDungeonAdventureScenes;

	private Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary;

	private bool currentSpeed2x;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cache0;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cache1;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cache2;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cache3;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cache4;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cache5;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cache6;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cache7;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cache8;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cache9;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cacheA;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cacheB;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cacheC;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cacheD;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cacheE;

	[CompilerGenerated]
	private static Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool> <>f__mg$cacheF;

	public BattleAdventureSceneManager(GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene[] worldDungeonAdventureScenes)
	{
		this.worldDungeonAdventureScenes = worldDungeonAdventureScenes;
		this.dictionary = new Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>>();
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary = this.dictionary;
		BattleAdventureSceneManager.TriggerType key = BattleAdventureSceneManager.TriggerType.DigimonEntryStart;
		if (BattleAdventureSceneManager.<>f__mg$cache0 == null)
		{
			BattleAdventureSceneManager.<>f__mg$cache0 = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteDigimonEntry);
		}
		dictionary.Add(key, BattleAdventureSceneManager.<>f__mg$cache0);
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary2 = this.dictionary;
		BattleAdventureSceneManager.TriggerType key2 = BattleAdventureSceneManager.TriggerType.DigimonEntryEnd;
		if (BattleAdventureSceneManager.<>f__mg$cache1 == null)
		{
			BattleAdventureSceneManager.<>f__mg$cache1 = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteDigimonEntry);
		}
		dictionary2.Add(key2, BattleAdventureSceneManager.<>f__mg$cache1);
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary3 = this.dictionary;
		BattleAdventureSceneManager.TriggerType key3 = BattleAdventureSceneManager.TriggerType.WaveStart;
		if (BattleAdventureSceneManager.<>f__mg$cache2 == null)
		{
			BattleAdventureSceneManager.<>f__mg$cache2 = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteWave);
		}
		dictionary3.Add(key3, BattleAdventureSceneManager.<>f__mg$cache2);
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary4 = this.dictionary;
		BattleAdventureSceneManager.TriggerType key4 = BattleAdventureSceneManager.TriggerType.WaveEnd;
		if (BattleAdventureSceneManager.<>f__mg$cache3 == null)
		{
			BattleAdventureSceneManager.<>f__mg$cache3 = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteWave);
		}
		dictionary4.Add(key4, BattleAdventureSceneManager.<>f__mg$cache3);
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary5 = this.dictionary;
		BattleAdventureSceneManager.TriggerType key5 = BattleAdventureSceneManager.TriggerType.RoundStart;
		if (BattleAdventureSceneManager.<>f__mg$cache4 == null)
		{
			BattleAdventureSceneManager.<>f__mg$cache4 = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteRound);
		}
		dictionary5.Add(key5, BattleAdventureSceneManager.<>f__mg$cache4);
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary6 = this.dictionary;
		BattleAdventureSceneManager.TriggerType key6 = BattleAdventureSceneManager.TriggerType.RoundEnd;
		if (BattleAdventureSceneManager.<>f__mg$cache5 == null)
		{
			BattleAdventureSceneManager.<>f__mg$cache5 = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteRound);
		}
		dictionary6.Add(key6, BattleAdventureSceneManager.<>f__mg$cache5);
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary7 = this.dictionary;
		BattleAdventureSceneManager.TriggerType key7 = BattleAdventureSceneManager.TriggerType.SkillStart;
		if (BattleAdventureSceneManager.<>f__mg$cache6 == null)
		{
			BattleAdventureSceneManager.<>f__mg$cache6 = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteSkill);
		}
		dictionary7.Add(key7, BattleAdventureSceneManager.<>f__mg$cache6);
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary8 = this.dictionary;
		BattleAdventureSceneManager.TriggerType key8 = BattleAdventureSceneManager.TriggerType.SkillEnd;
		if (BattleAdventureSceneManager.<>f__mg$cache7 == null)
		{
			BattleAdventureSceneManager.<>f__mg$cache7 = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteSkill);
		}
		dictionary8.Add(key8, BattleAdventureSceneManager.<>f__mg$cache7);
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary9 = this.dictionary;
		BattleAdventureSceneManager.TriggerType key9 = BattleAdventureSceneManager.TriggerType.SkillHitStart;
		if (BattleAdventureSceneManager.<>f__mg$cache8 == null)
		{
			BattleAdventureSceneManager.<>f__mg$cache8 = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteSkillHit);
		}
		dictionary9.Add(key9, BattleAdventureSceneManager.<>f__mg$cache8);
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary10 = this.dictionary;
		BattleAdventureSceneManager.TriggerType key10 = BattleAdventureSceneManager.TriggerType.SkillHitEnd;
		if (BattleAdventureSceneManager.<>f__mg$cache9 == null)
		{
			BattleAdventureSceneManager.<>f__mg$cache9 = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteSkillHit);
		}
		dictionary10.Add(key10, BattleAdventureSceneManager.<>f__mg$cache9);
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary11 = this.dictionary;
		BattleAdventureSceneManager.TriggerType key11 = BattleAdventureSceneManager.TriggerType.WinStart;
		if (BattleAdventureSceneManager.<>f__mg$cacheA == null)
		{
			BattleAdventureSceneManager.<>f__mg$cacheA = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteWin);
		}
		dictionary11.Add(key11, BattleAdventureSceneManager.<>f__mg$cacheA);
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary12 = this.dictionary;
		BattleAdventureSceneManager.TriggerType key12 = BattleAdventureSceneManager.TriggerType.WinEnd;
		if (BattleAdventureSceneManager.<>f__mg$cacheB == null)
		{
			BattleAdventureSceneManager.<>f__mg$cacheB = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteWin);
		}
		dictionary12.Add(key12, BattleAdventureSceneManager.<>f__mg$cacheB);
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary13 = this.dictionary;
		BattleAdventureSceneManager.TriggerType key13 = BattleAdventureSceneManager.TriggerType.LoseStart;
		if (BattleAdventureSceneManager.<>f__mg$cacheC == null)
		{
			BattleAdventureSceneManager.<>f__mg$cacheC = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteLose);
		}
		dictionary13.Add(key13, BattleAdventureSceneManager.<>f__mg$cacheC);
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary14 = this.dictionary;
		BattleAdventureSceneManager.TriggerType key14 = BattleAdventureSceneManager.TriggerType.LoseEnd;
		if (BattleAdventureSceneManager.<>f__mg$cacheD == null)
		{
			BattleAdventureSceneManager.<>f__mg$cacheD = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteLose);
		}
		dictionary14.Add(key14, BattleAdventureSceneManager.<>f__mg$cacheD);
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary15 = this.dictionary;
		BattleAdventureSceneManager.TriggerType key15 = BattleAdventureSceneManager.TriggerType.TotalRoundStart;
		if (BattleAdventureSceneManager.<>f__mg$cacheE == null)
		{
			BattleAdventureSceneManager.<>f__mg$cacheE = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteTotalRound);
		}
		dictionary15.Add(key15, BattleAdventureSceneManager.<>f__mg$cacheE);
		Dictionary<BattleAdventureSceneManager.TriggerType, Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>> dictionary16 = this.dictionary;
		BattleAdventureSceneManager.TriggerType key16 = BattleAdventureSceneManager.TriggerType.TotalRoundEnd;
		if (BattleAdventureSceneManager.<>f__mg$cacheF == null)
		{
			BattleAdventureSceneManager.<>f__mg$cacheF = new Func<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene, bool>(BattleAdventureSceneManager.CheckExecuteTotalRound);
		}
		dictionary16.Add(key16, BattleAdventureSceneManager.<>f__mg$cacheF);
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
		if (battleStateManager.battleMode != BattleMode.Multi && battleStateManager.battleMode != BattleMode.PvP)
		{
			yield break;
		}
		do
		{
			float time = 3f;
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
				IEnumerator wait = Util.WaitForRealTime(0.5f);
				while (wait.MoveNext())
				{
					object obj = wait.Current;
					yield return obj;
				}
			}
			else
			{
				IEnumerator waitAllPlayers = battleStateManager.multiBasicFunction.WaitAllPlayers(TCPMessageType.AdventureScene);
				while (waitAllPlayers.MoveNext())
				{
					yield return null;
				}
			}
		}
		while (!battleStateManager.multiBasicFunction.isAdventureSceneAllEnd);
		battleStateManager.uiControlMulti.HideLoading();
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
			if (string.IsNullOrEmpty(worldDungeonAdventureScene.adventureValue))
			{
				global::Debug.LogWarning("アドベンチャーテキストがない");
			}
			else if (triggerType == (BattleAdventureSceneManager.TriggerType)worldDungeonAdventureScene.adventureTrigger.ToInt32())
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
		this.isUpdate = false;
		this.SetActive(true);
		BattleStateManager.current.time.SetPlaySpeed(this.currentSpeed2x, false);
		if (BattleStateManager.current.battleMode == BattleMode.Multi || BattleStateManager.current.battleMode == BattleMode.PvP)
		{
			BattleStateManager.current.uiControlMulti.ShowLoading(false);
		}
	}

	private static bool CheckExecuteDigimonEntry(GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene data)
	{
		if (string.IsNullOrEmpty(data.adventureTriggerValue) || data.adventureTriggerValue == "0")
		{
			return true;
		}
		BattleStateManager current = BattleStateManager.current;
		int num = current.battleStateData.currentWaveNumber + 1;
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
		int num = current.battleStateData.currentWaveNumber + 1;
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

	private static bool CheckExecuteTotalRound(GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene data)
	{
		if (string.IsNullOrEmpty(data.adventureTriggerValue) || data.adventureTriggerValue == "0")
		{
			return true;
		}
		BattleStateManager current = BattleStateManager.current;
		int num = data.adventureTriggerValue.ToInt32();
		return current.battleStateData.totalRoundNumber == num;
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
		LoseEnd,
		TotalRoundStart,
		TotalRoundEnd
	}
}
