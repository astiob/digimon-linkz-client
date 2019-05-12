using System;
using UnityEngine;

namespace BattleStateMachineInternal
{
	[Serializable]
	public class BattleStateHierarchyData
	{
		public BattleCameraObject cameraObject;

		public string[] usePlayerCharactersId = new string[]
		{
			string.Empty,
			string.Empty,
			string.Empty
		};

		public BattleWave[] batteWaves = new BattleWave[]
		{
			new BattleWave()
		};

		public int digiStoneNumber = 99;

		[Range(0f, 1f)]
		public float playerPursuitPercentage;

		[Range(0f, 1f)]
		public float enemyPursuitPercentage;

		public string battleNum = "1";

		public string areaName = string.Empty;

		public int areaId;

		public string[] extraEffectsId = new string[0];

		public string leaderSkillDescription;

		public InitialIntroductionBox[] initialIntroductionMessage = new InitialIntroductionBox[]
		{
			new InitialIntroductionBox()
		};

		public string useStageId;

		[Range(0f, 2f)]
		public int leaderCharacter;

		public string useHitEffectId;

		public bool onInstanceStage = true;

		public bool onInverseCamera;

		public int limitRound = -1;

		public int speedClearRound = -1;

		public bool onAutoPlay;

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
	}
}
