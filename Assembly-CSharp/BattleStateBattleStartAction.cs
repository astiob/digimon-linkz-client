using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExtension;

public class BattleStateBattleStartAction : BattleStateBase
{
	private bool isBigBoss;

	private bool isFindBoss;

	private bool currentSpeed2x;

	private bool is1stStart;

	private Action final;

	private string cameraKey = "skillA";

	private List<IEnumerator> smallToBigTransition = new List<IEnumerator>();

	public BattleStateBattleStartAction(Action OnExit, Action<EventState> OnExitGotEvent = null) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void EnabledThisState()
	{
		this.final = null;
		this.isFindBoss = !BoolExtension.AllMachValue(false, base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].enemiesBossFlag);
		this.currentSpeed2x = base.hierarchyData.on2xSpeedPlay;
		this.is1stStart = (base.battleStateData.currentWaveNumber == 0);
		this.isBigBoss = (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1);
		if (base.battleMode == BattleMode.PvP)
		{
			base.stateManager.uiControlPvP.ApplySetAlwaysUIColliders(false);
		}
		else
		{
			base.stateManager.uiControl.SetMenuAuto2xButtonEnabled(false);
		}
		base.stateManager.time.SetPlaySpeed(false, false);
	}

	protected override IEnumerator MainRoutine()
	{
		base.battleStateData.calledBattleStartAction = true;
		base.stateManager.threeDAction.HideAllCharactersAction(base.battleStateData.GetTotalCharacters());
		base.hierarchyData.stageParams.TransformStage(base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType);
		this.smallToBigTransition.Clear();
		IEnumerator transitionCount;
		if (this.is1stStart)
		{
			base.stateManager.soundPlayer.TryPlayBGM(base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].bgmId, 0f);
			if (!this.isBigBoss)
			{
				transitionCount = this.InsertPlayer();
				while (transitionCount.MoveNext())
				{
					foreach (IEnumerator ie in this.smallToBigTransition)
					{
						ie.MoveNext();
					}
					yield return null;
				}
			}
		}
		else
		{
			base.stateManager.soundPlayer.TryStopBGM(base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].bgmId);
			base.stateManager.threeDAction.ShowAliveCharactersAction(base.battleStateData.playerCharacters);
			base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(base.battleStateData.playerCharacters);
		}
		if (this.isBigBoss)
		{
			base.battleStateData.commandSelectTweenTargetCamera = base.battleStateData.bigBossCommandSelectTweenTargetCamera;
			transitionCount = this.InsertBigBoss();
		}
		else
		{
			base.battleStateData.commandSelectTweenTargetCamera = base.battleStateData.normalCommandSelectTweenTargetCamera;
			transitionCount = this.InsertEnemies();
		}
		while (transitionCount.MoveNext())
		{
			foreach (IEnumerator ie2 in this.smallToBigTransition)
			{
				ie2.MoveNext();
			}
			yield return null;
		}
		base.stateManager.cameraControl.StopCameraMotionAction(this.cameraKey);
		base.stateManager.threeDAction.StopAlwaysEffectAction(base.battleStateData.insertCharacterEffect);
		if (base.battleMode == BattleMode.PvP)
		{
			base.stateManager.threeDAction.StopAlwaysEffectAction(base.battleStateData.insertEnemyEffect);
		}
		if (this.isFindBoss)
		{
			base.stateManager.threeDAction.StopAlwaysEffectAction(base.battleStateData.insertBossCharacterEffect);
		}
		if (!this.isBigBoss)
		{
			base.stateManager.threeDAction.SmallToBigTransitionAfter(base.battleStateData.enemies, this.smallToBigTransition.ToArray());
		}
		if (this.is1stStart && !this.isBigBoss)
		{
			if (base.battleMode == BattleMode.PvP)
			{
				GameObject go = (base.stateManager.battleUiComponents as BattleUIComponentsPvP).pvpVSUi.gameObject;
				NGUITools.SetActiveSelf(go, true);
				SoundPlayer.PlayBattleVSSE();
			}
			base.stateManager.threeDAction.ShowAliveCharactersAction(base.battleStateData.playerCharacters);
			base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(base.battleStateData.playerCharacters);
			this.cameraKey = "0002_roundStart";
			base.stateManager.cameraControl.PlayCameraMotionAction(this.cameraKey, base.battleStateData.stageSpawnPoint, true);
			base.stateManager.SetBattleScreen(BattleScreen.StartAction);
			IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.battleStartActionWaitSecond, null, null);
			while (wait.MoveNext())
			{
				yield return null;
			}
			base.stateManager.cameraControl.StopCameraMotionAction(this.cameraKey);
		}
		if (this.final != null)
		{
			this.final();
		}
		yield break;
	}

	private void StopBattleWarning()
	{
		SoundPlayer.StopBattleWarning();
	}

	protected override void DisabledThisState()
	{
		base.stateManager.soundPlayer.TryStopSE(base.battleStateData.insertCharacterEffect);
		if (this.isFindBoss)
		{
			base.stateManager.soundPlayer.TryStopSE(base.battleStateData.insertBossCharacterEffect);
		}
		base.stateManager.time.SetPlaySpeed(this.currentSpeed2x, false);
		if (base.battleMode == BattleMode.PvP)
		{
			base.stateManager.uiControlPvP.ApplySetAlwaysUIColliders(true);
		}
		else
		{
			base.stateManager.uiControl.SetMenuAuto2xButtonEnabled(true);
		}
		if (!this.is1stStart)
		{
			base.stateManager.soundPlayer.TryPlayBGM(base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].bgmId, 0f);
		}
	}

	private IEnumerator InsertPlayer()
	{
		float transitionSpeed = base.stateManager.stateProperty.insertCharacterWaitSecond;
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			this.smallToBigTransition.Add(base.stateManager.threeDAction.SmallToBigTransition((float)base.stateManager.stateProperty.playersTargetSelectOrder[i] * transitionSpeed, base.battleStateData.playerCharacters[i], base.battleStateData.insertCharacterEffect[i]));
		}
		this.cameraKey = "skillA";
		if (base.battleMode == BattleMode.PvP)
		{
			this.cameraKey = "pvpBattleStart";
			GameObject go = (base.stateManager.battleUiComponents as BattleUIComponentsPvP).pvpBattleYourPartyUi.gameObject;
			NGUITools.SetActiveSelf(go, true);
		}
		base.stateManager.cameraControl.PlayCameraMotionActionCharacter(this.cameraKey, base.battleStateData.playerCharacters[0]);
		IEnumerator transitionCount = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.battleStartPlayerInsertWaitSecond, null, null);
		base.stateManager.SetBattleScreen(BattleScreen.InsertPlayer);
		while (transitionCount.MoveNext())
		{
			foreach (IEnumerator ie in this.smallToBigTransition)
			{
				ie.MoveNext();
			}
			yield return null;
		}
		base.stateManager.threeDAction.StopAlwaysEffectAction(base.battleStateData.insertCharacterEffect);
		base.stateManager.cameraControl.StopCameraMotionAction(this.cameraKey);
		base.stateManager.threeDAction.SmallToBigTransitionAfter(base.battleStateData.playerCharacters, this.smallToBigTransition.ToArray());
		this.smallToBigTransition.Clear();
		base.stateManager.soundPlayer.TryStopSE(base.battleStateData.insertCharacterEffect);
		yield break;
	}

	private IEnumerator InsertEnemies()
	{
		float transitionSpeed = base.stateManager.stateProperty.insertCharacterWaitSecond;
		int countEffect2 = 0;
		float findBossTransition = (!this.isFindBoss) ? 0f : base.stateManager.stateProperty.bossIntroInsertActionWaitSecond;
		for (int i = 0; i < base.battleStateData.enemies.Length; i++)
		{
			bool isFindBossNow = base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].enemiesBossFlag[i];
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
			this.smallToBigTransition.Add(base.stateManager.threeDAction.SmallToBigTransition((float)base.battleStateData.enemiesTargetSelectOrder[base.battleStateData.enemiesTargetSelectOrder.Length - 1 - i] * transitionSpeed + findBossTransition, base.battleStateData.enemies[i], effect));
			if (isFindBossNow)
			{
				countEffect2++;
			}
		}
		IEnumerator transitionCount = null;
		this.cameraKey = "skillA";
		if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].floorType == 3)
		{
			this.cameraKey = "0001_bossStart";
			base.stateManager.cameraControl.PlayCameraMotionActionCharacter(this.cameraKey, base.battleStateData.enemies[0]);
			this.final = new Action(this.StopBattleWarning);
			SoundPlayer.PlayBattleWarning();
			base.stateManager.SetBattleScreen(BattleScreen.ExtraStartAction);
			this.cameraKey = "0001_bossStart";
			transitionCount = base.stateManager.uiControl.IsStageStartAnimation();
		}
		else if (this.isFindBoss)
		{
			this.cameraKey = "0001_bossStart";
			base.stateManager.cameraControl.PlayCameraMotionActionCharacter(this.cameraKey, base.battleStateData.enemies[0]);
			this.final = new Action(this.StopBattleWarning);
			SoundPlayer.PlayBattleWarning();
			base.stateManager.SetBattleScreen(BattleScreen.BossStartAction);
			this.cameraKey = "0001_bossStart";
			transitionCount = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.bossIntroActionWaitSecond, null, null);
		}
		else
		{
			base.stateManager.SetBattleScreen((!this.is1stStart) ? BattleScreen.NextBattle : BattleScreen.InsertEnemy);
			this.cameraKey = "skillA";
			if (base.battleMode == BattleMode.PvP)
			{
				this.cameraKey = "pvpBattleStart";
				GameObject go = (base.stateManager.battleUiComponents as BattleUIComponentsPvP).pvpBattleEnemyPartyUi.gameObject;
				NGUITools.SetActiveSelf(go, true);
			}
			base.stateManager.cameraControl.PlayCameraMotionActionCharacter(this.cameraKey, base.battleStateData.enemies[0]);
			transitionCount = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.battleStartEnemyInsertWaitSecond + (base.stateManager.stateProperty.insertCharacterWaitSecond * (float)base.battleStateData.enemies.Length - 1f), null, null);
		}
		while (transitionCount.MoveNext())
		{
			foreach (IEnumerator ie in this.smallToBigTransition)
			{
				ie.MoveNext();
			}
			yield return null;
		}
		yield break;
	}

	private IEnumerator InsertBigBoss()
	{
		this.cameraKey = "BigBoss/0001_Start";
		base.stateManager.cameraControl.PlayCameraMotionAction(this.cameraKey, base.battleStateData.stageSpawnPoint, true);
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
		base.stateManager.cameraControl.StopCameraMotionAction(this.cameraKey);
		while (base.battleStateData.enemies[0].CharacterParams.isPlaying(CharacterAnimationType.eat))
		{
			yield return null;
		}
		yield return null;
		yield break;
	}
}
