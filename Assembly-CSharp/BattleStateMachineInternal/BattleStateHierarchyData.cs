using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleStateMachineInternal
{
	[Serializable]
	public class BattleStateHierarchyData
	{
		public BattleCameraObject cameraObject;

		public PlayerStatus[] usePlayerCharacters;

		public BattleWave[] batteWaves = new BattleWave[]
		{
			new BattleWave()
		};

		public int digiStoneNumber = 99;

		[Range(0f, 1f)]
		public float playerPursuitPercentage;

		[Range(0f, 1f)]
		public float enemyPursuitPercentage;

		public string startId = string.Empty;

		public string battleNum = "1";

		public string areaName = string.Empty;

		public int areaId;

		public InitialIntroductionBox[] initialIntroductionMessage = new InitialIntroductionBox[]
		{
			new InitialIntroductionBox()
		};

		public string useStageId = string.Empty;

		[Range(0f, 2f)]
		public int leaderCharacter;

		public bool onInstanceStage = true;

		public bool onInverseCamera;

		public int limitRound = -1;

		public int speedClearRound = -1;

		public int onAutoPlay;

		public bool on2xSpeedPlay;

		public bool onHitRate100Percent;

		public bool onEnableRandomValue = true;

		public bool onApRevivalMax;

		public bool isPossibleContinue = true;

		public bool isPossibleRetire = true;

		public bool useInitialIntroduction;

		public int initialIntroductionIndex;

		public bool onEnableBackKey = true;

		public StageParams stageParams;

		private Dictionary<string, CharacterDatas> characterDatasDictionary = new Dictionary<string, CharacterDatas>();

		private Dictionary<string, LeaderSkillStatus> leaderSkillStatusDictionary = new Dictionary<string, LeaderSkillStatus>();

		private Dictionary<string, SkillStatus> skillStatusDictionary = new Dictionary<string, SkillStatus>();

		public void Initialize()
		{
		}

		public void RemoveAllCachedObjects()
		{
			if (this.cameraObject != null)
			{
				this.cameraObject.RemoveAllCachedObjects();
			}
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}

		public int maxInitialIntroductionIndex
		{
			get
			{
				return this.initialIntroductionMessage.Length;
			}
		}

		public void AddCharacterDatas(string key, CharacterDatas value)
		{
			if (!string.IsNullOrEmpty(key) && value != null && !this.characterDatasDictionary.ContainsKey(key))
			{
				this.characterDatasDictionary.Add(key, value);
			}
		}

		public CharacterDatas GetCharacterDatas(string key)
		{
			CharacterDatas result = null;
			this.characterDatasDictionary.TryGetValue(key, out result);
			return result;
		}

		public void AddLeaderSkillStatus(string key, LeaderSkillStatus value)
		{
			if (!string.IsNullOrEmpty(key) && value != null && !this.leaderSkillStatusDictionary.ContainsKey(key))
			{
				this.leaderSkillStatusDictionary.Add(key, value);
			}
		}

		public LeaderSkillStatus GetLeaderSkillStatus(string key)
		{
			LeaderSkillStatus result = null;
			this.leaderSkillStatusDictionary.TryGetValue(key, out result);
			return result;
		}

		public void AddSkillStatus(string key, SkillStatus value)
		{
			if (!string.IsNullOrEmpty(key) && value != null && !this.skillStatusDictionary.ContainsKey(key))
			{
				this.skillStatusDictionary.Add(key, value);
			}
		}

		public SkillStatus GetSkillStatus(string key)
		{
			SkillStatus result = null;
			this.skillStatusDictionary.TryGetValue(key, out result);
			return result;
		}

		public SkillStatus[] GetAllSkillStatus()
		{
			List<SkillStatus> list = new List<SkillStatus>();
			foreach (SkillStatus item in this.skillStatusDictionary.Values)
			{
				list.Add(item);
			}
			return list.ToArray();
		}
	}
}
