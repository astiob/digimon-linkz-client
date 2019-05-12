using BattleStateMachineInternal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExtension;

public class BattleStateBattleStartAction : BattleStateBase
{
	private BattleWave battleWave;

	private bool isBigBoss;

	private bool isExtra;

	private bool isFindBoss;

	private bool currentSpeed2x;

	private bool is1stStart;

	public BattleStateBattleStartAction(Action OnExit, Action<EventState> OnExitGotEvent = null) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void EnabledThisState()
	{
		this.battleWave = base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber];
		this.isFindBoss = !BoolExtension.AllMachValue(false, this.battleWave.enemiesBossFlag);
		this.is1stStart = (base.battleStateData.currentWaveNumber == 0);
		this.isBigBoss = (this.battleWave.cameraType == 1);
		this.isExtra = (this.battleWave.floorType == 3);
		if (base.battleMode == BattleMode.PvP)
		{
			base.stateManager.uiControlPvP.ApplySetAlwaysUIColliders(false);
		}
		else
		{
			base.stateManager.uiControl.SetMenuAuto2xButtonEnabled(false);
		}
		this.currentSpeed2x = base.hierarchyData.on2xSpeedPlay;
		base.stateManager.time.SetPlaySpeed(false, false);
		if (this.is1stStart)
		{
			base.stateManager.soundPlayer.TryPlayBGM(this.battleWave.bgmId, 0f);
		}
		else
		{
			base.stateManager.soundPlayer.TryStopBGM(this.battleWave.bgmId);
		}
	}

	protected override IEnumerator MainRoutine()
	{
		base.stateManager.threeDAction.HideAllCharactersAction(base.battleStateData.GetTotalCharacters());
		base.hierarchyData.stageParams.TransformStage(this.battleWave.cameraType);
		if (this.isBigBoss)
		{
			base.battleStateData.commandSelectTweenTargetCamera = base.battleStateData.bigBossCommandSelectTweenTargetCamera;
		}
		else
		{
			base.battleStateData.commandSelectTweenTargetCamera = base.battleStateData.normalCommandSelectTweenTargetCamera;
		}
		base.stateManager.battleAdventureSceneManager.OnTrigger(BattleAdventureSceneManager.TriggerType.DigimonEntryStart);
		if (base.stateManager.battleAdventureSceneManager.isUpdate)
		{
			IEnumerator digimonEntryStart = base.stateManager.battleAdventureSceneManager.Update();
			while (digimonEntryStart.MoveNext())
			{
				yield return null;
			}
		}
		else
		{
			if (this.is1stStart && !this.isBigBoss)
			{
				IEnumerator insertPlayer = this.InsertPlayer();
				while (insertPlayer.MoveNext())
				{
					yield return null;
				}
			}
			IEnumerator insertEnemies = null;
			if (this.isBigBoss)
			{
				insertEnemies = this.InsertBigBoss();
			}
			else
			{
				insertEnemies = this.InsertEnemies();
			}
			while (insertEnemies.MoveNext())
			{
				yield return null;
			}
			if (this.is1stStart && !this.isBigBoss)
			{
				IEnumerator insertFinish = this.InsertFinish();
				while (insertFinish.MoveNext())
				{
					yield return null;
				}
			}
		}
		base.stateManager.battleAdventureSceneManager.OnTrigger(BattleAdventureSceneManager.TriggerType.DigimonEntryEnd);
		IEnumerator digimonEntryEnd = base.stateManager.battleAdventureSceneManager.Update();
		while (digimonEntryEnd.MoveNext())
		{
			yield return null;
		}
		base.stateManager.threeDAction.ShowAliveCharactersAction(base.battleStateData.GetTotalCharacters());
		base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(base.battleStateData.GetTotalCharacters());
		yield break;
	}

	protected override void DisabledThisState()
	{
		base.stateManager.soundPlayer.TryStopSE(base.battleStateData.insertCharacterEffect);
		if (this.isFindBoss)
		{
			base.stateManager.soundPlayer.TryStopSE(base.battleStateData.insertBossCharacterEffect);
		}
		base.stateManager.uiControl.ApplyBattleStartAction(false);
		if (base.battleMode == BattleMode.PvP)
		{
			base.stateManager.uiControl.ApplyVSUI(false);
			base.stateManager.uiControlPvP.ApplySetAlwaysUIColliders(true);
		}
		else
		{
			base.stateManager.uiControl.SetMenuAuto2xButtonEnabled(true);
		}
		if (!this.is1stStart)
		{
			base.stateManager.soundPlayer.TryPlayBGM(this.battleWave.bgmId, 0f);
		}
		base.stateManager.time.SetPlaySpeed(this.currentSpeed2x, false);
	}

	private IEnumerator InsertPlayer()
	{
		float transitionSpeed = base.stateManager.stateProperty.insertCharacterWaitSecond;
		List<IEnumerator> smallToBigTransition = new List<IEnumerator>();
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			smallToBigTransition.Add(base.stateManager.threeDAction.SmallToBigTransition((float)base.stateManager.stateProperty.playersTargetSelectOrder[i] * transitionSpeed, base.battleStateData.playerCharacters[i], base.battleStateData.insertCharacterEffect[i]));
		}
		string cameraKey = "skillA";
		if (base.battleMode == BattleMode.PvP)
		{
			cameraKey = "pvpBattleStart";
			GameObject go = (base.stateManager.battleUiComponents as BattleUIComponentsPvP).pvpBattleYourPartyUi.gameObject;
			NGUITools.SetActiveSelf(go, true);
		}
		base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, base.battleStateData.playerCharacters[0]);
		IEnumerator transitionCount = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.battleStartPlayerInsertWaitSecond, null, null);
		base.stateManager.SetBattleScreen(BattleScreen.InsertPlayer);
		while (transitionCount.MoveNext())
		{
			foreach (IEnumerator ie in smallToBigTransition)
			{
				ie.MoveNext();
			}
			yield return null;
		}
		base.stateManager.threeDAction.StopAlwaysEffectAction(base.battleStateData.insertCharacterEffect);
		base.stateManager.cameraControl.StopCameraMotionAction(cameraKey);
		base.stateManager.threeDAction.SmallToBigTransitionAfter(base.battleStateData.playerCharacters, smallToBigTransition.ToArray());
		base.stateManager.soundPlayer.TryStopSE(base.battleStateData.insertCharacterEffect);
		yield break;
	}

	private IEnumerator InsertEnemies()
	{
		float transitionSpeed = base.stateManager.stateProperty.insertCharacterWaitSecond;
		List<IEnumerator> smallToBigTransition = new List<IEnumerator>();
		int countEffect2 = 0;
		float findBossTransition = (!this.isFindBoss) ? 0f : base.stateManager.stateProperty.bossIntroInsertActionWaitSecond;
		for (int i = 0; i < base.battleStateData.enemies.Length; i++)
		{
			bool isFindBossNow = this.battleWave.enemiesBossFlag[i];
			AlwaysEffectParams effect = null;
			if (isFindBossNow)
			{
				effect = base.battleStateData.insertBossCharacterEffect[countEffect2];
			}
			else if (base.battleMode == BattleMode.PvP)
			{
				effect = base.battleStateData.insertEnemyEffect[i];
			}
			else
			{
				effect = base.battleStateData.insertCharacterEffect[i];
			}
			smallToBigTransition.Add(base.stateManager.threeDAction.SmallToBigTransition((float)base.battleStateData.enemiesTargetSelectOrder[base.battleStateData.enemiesTargetSelectOrder.Length - 1 - i] * transitionSpeed + findBossTransition, base.battleStateData.enemies[i], effect));
			if (isFindBossNow)
			{
				countEffect2++;
			}
		}
		IEnumerator transitionCount = null;
		string cameraKey = "skillA";
		if (this.isExtra)
		{
			cameraKey = "0001_bossStart";
			base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, base.battleStateData.enemies[0]);
			SoundPlayer.PlayBattleWarning();
			base.stateManager.SetBattleScreen(BattleScreen.ExtraStartAction);
			cameraKey = "0001_bossStart";
			transitionCount = base.stateManager.uiControl.IsStageStartAnimation();
		}
		else if (this.isFindBoss)
		{
			cameraKey = "0001_bossStart";
			base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, base.battleStateData.enemies[0]);
			SoundPlayer.PlayBattleWarning();
			base.stateManager.SetBattleScreen(BattleScreen.BossStartAction);
			cameraKey = "0001_bossStart";
			transitionCount = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.bossIntroActionWaitSecond, null, null);
		}
		else
		{
			base.stateManager.SetBattleScreen((!this.is1stStart) ? BattleScreen.NextBattle : BattleScreen.InsertEnemy);
			cameraKey = "skillA";
			if (base.battleMode == BattleMode.PvP)
			{
				cameraKey = "pvpBattleStart";
				GameObject go = (base.stateManager.battleUiComponents as BattleUIComponentsPvP).pvpBattleEnemyPartyUi.gameObject;
				NGUITools.SetActiveSelf(go, true);
			}
			base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, base.battleStateData.enemies[0]);
			transitionCount = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.battleStartEnemyInsertWaitSecond + (base.stateManager.stateProperty.insertCharacterWaitSecond * (float)base.battleStateData.enemies.Length - 1f), null, null);
		}
		while (transitionCount.MoveNext())
		{
			foreach (IEnumerator ie in smallToBigTransition)
			{
				ie.MoveNext();
			}
			yield return null;
		}
		base.stateManager.cameraControl.StopCameraMotionAction(cameraKey);
		base.stateManager.threeDAction.SmallToBigTransitionAfter(base.battleStateData.enemies, smallToBigTransition.ToArray());
		if (base.battleMode == BattleMode.PvP)
		{
			base.stateManager.threeDAction.StopAlwaysEffectAction(base.battleStateData.insertEnemyEffect);
		}
		else
		{
			base.stateManager.threeDAction.StopAlwaysEffectAction(base.battleStateData.insertCharacterEffect);
		}
		if (this.isFindBoss)
		{
			base.stateManager.threeDAction.StopAlwaysEffectAction(base.battleStateData.insertBossCharacterEffect);
		}
		if (this.isFindBoss || this.isExtra)
		{
			SoundPlayer.StopBattleWarning();
		}
		yield break;
	}

	private IEnumerator InsertBigBoss()
	{
		string cameraKey = "BigBoss/0001_Start";
		base.stateManager.cameraControl.PlayCameraMotionAction(cameraKey, base.battleStateData.stageSpawnPoint, true);
		base.stateManager.threeDAction.BigBossInsertAction(base.battleStateData.enemies[0], base.battleStateData.playerCharacters);
		IEnumerator transitionCount = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.battleStartBigBossActionWaitSecond - base.stateManager.stateProperty.battleStartActionWaitSecond, null, null);
		base.stateManager.SetBattleScreen(BattleScreen.InsertPlayer);
		while (transitionCount.MoveNext())
		{
			yield return null;
		}
		base.stateManager.SetBattleScreen(BattleScreen.StartAction);
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.battleStartActionWaitSecond, null, null);
		while (wait.MoveNext())
		{
			yield return null;
		}
		base.stateManager.cameraControl.StopCameraMotionAction(cameraKey);
		while (base.battleStateData.enemies[0].CharacterParams.isPlaying(CharacterAnimationType.eat))
		{
			yield return null;
		}
		yield return null;
		yield break;
	}

	private IEnumerator InsertFinish()
	{
		string cameraKey = "0002_roundStart";
		base.stateManager.cameraControl.PlayCameraMotionAction(cameraKey, base.battleStateData.stageSpawnPoint, true);
		base.stateManager.SetBattleScreen(BattleScreen.StartAction);
		base.stateManager.uiControl.ApplyBattleStartAction(true);
		if (base.battleMode == BattleMode.PvP)
		{
			base.stateManager.uiControl.ApplyVSUI(true);
			base.stateManager.uiControl.ApplyPlayerLeaderSkill(base.battleStateData.leaderCharacter.isHavingLeaderSkill, base.battleStateData.leaderCharacter.leaderSkillStatus.name, false);
			base.stateManager.uiControl.ApplyEnemyLeaderSkill(base.battleStateData.leaderEnemyCharacter.isHavingLeaderSkill, base.battleStateData.leaderEnemyCharacter.leaderSkillStatus.name, false);
		}
		else
		{
			base.stateManager.uiControl.ApplyPlayerLeaderSkill(base.battleStateData.leaderCharacter.isHavingLeaderSkill, base.battleStateData.leaderCharacter.leaderSkillStatus.name, false);
		}
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.battleStartActionWaitSecond, null, null);
		while (wait.MoveNext())
		{
			yield return null;
		}
		base.stateManager.uiControl.ApplyBattleStartAction(false);
		base.stateManager.cameraControl.StopCameraMotionAction(cameraKey);
		yield break;
	}
}
