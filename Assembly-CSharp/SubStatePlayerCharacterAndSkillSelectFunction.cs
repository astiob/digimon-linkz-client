using System;
using System.Collections;
using UnityEngine;

public class SubStatePlayerCharacterAndSkillSelectFunction : BattleStateController
{
	private bool isOutCharacterView;

	protected CharacterStateControl currentCharacter;

	protected CharacterStateControl lastCharacter;

	public SubStatePlayerCharacterAndSkillSelectFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void EnabledThisState()
	{
		this.isOutCharacterView = false;
		this.lastCharacter = null;
		this.currentCharacter = base.battleStateData.currentSelectCharacterState;
		CharacterStateControl[] totalCharacters = base.stateManager.battleStateData.GetTotalCharacters();
		base.stateManager.threeDAction.ShowAliveCharactersAction(totalCharacters);
		base.stateManager.threeDAction.PlayIdleAnimationUndeadCharactersAction(totalCharacters);
		this.SetSelectCharacter(this.currentCharacter.myIndex);
		base.battleStateData.currentSelectCharacterIndex = this.currentCharacter.myIndex;
		if (base.hierarchyData.onAutoPlay == 2)
		{
			base.stateManager.targetSelect.AutoPlayCharacterAndSkillSelectFunction(this.currentCharacter);
		}
		else if (base.hierarchyData.onAutoPlay == 1)
		{
			base.stateManager.targetSelect.AutoPlayCharacterAndAttackSelectFunction(this.currentCharacter);
		}
		else if (base.hierarchyData.onAutoPlay == 0)
		{
			base.stateManager.callAction.HideMonsterDescription();
			base.stateManager.callAction.ForceHideSkillDescription();
			base.stateManager.targetSelect.TargetManualSelectAndApplyUIFunction(null);
			base.stateManager.uiControl.HideCharacterHUDFunction();
			base.stateManager.SetBattleScreen(BattleScreen.SkillSelects);
			base.stateManager.uiControl.ShowCharacterHUDFunction(base.battleStateData.GetTotalCharacters());
			if (base.battleStateData.enableRotateCam)
			{
				if (base.battleMode != BattleMode.PvP)
				{
					if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
					{
						base.stateManager.cameraControl.PlayCameraMotionAction("BigBoss/0007_commandCharaView", base.battleStateData.stageSpawnPoint, true);
					}
					else
					{
						base.stateManager.cameraControl.PlayCameraMotionAction("0007_commandCharaView", base.battleStateData.stageSpawnPoint, true);
					}
				}
				else
				{
					base.stateManager.cameraControl.PlayTweenCameraMotion(base.battleStateData.commandSelectTweenTargetCamera, null);
					base.stateManager.cameraControl.SetCameraLengthAction(base.battleStateData.commandSelectTweenTargetCamera);
				}
			}
			else
			{
				this.isOutCharacterView = true;
				base.stateManager.cameraControl.PlayTweenCameraMotion(base.battleStateData.commandSelectTweenTargetCamera, null);
				base.stateManager.cameraControl.SetCameraLengthAction(base.battleStateData.commandSelectTweenTargetCamera);
			}
		}
	}

	protected override IEnumerator MainRoutine()
	{
		if (base.hierarchyData.onAutoPlay != 0)
		{
			yield return null;
			yield break;
		}
		base.stateManager.uiControl.SetHudCollider(true);
		for (;;)
		{
			base.battleStateData.commandSelectTweenTargetCamera.transitionScale = Time.timeScale;
			if (!this.isOutCharacterView && !base.stateManager.time.isPause && Input.GetKeyUp(KeyCode.Mouse0))
			{
				this.isOutCharacterView = true;
				if (base.battleStateData.enableRotateCam)
				{
					base.stateManager.cameraControl.StopCameraMotionAction("0007_commandCharaView");
					base.stateManager.cameraControl.StopCameraMotionAction("BigBoss/0007_commandCharaView");
				}
				base.stateManager.cameraControl.PlayTweenCameraMotion(base.battleStateData.commandSelectTweenTargetCamera, null);
				base.stateManager.cameraControl.SetCameraLengthAction(base.battleStateData.commandSelectTweenTargetCamera);
			}
			this.UpdateTarget();
			if (base.battleStateData.isShowMenuWindow || base.battleStateData.isShowRevivalWindow)
			{
				base.stateManager.callAction.HideMonsterDescription();
				base.stateManager.callAction.ForceHideSkillDescription();
			}
			if (base.hierarchyData.onAutoPlay != 0)
			{
				break;
			}
			if (base.battleStateData.onSkillTrigger)
			{
				goto Block_10;
			}
			if (!this.isOutCharacterView || this.lastCharacter == null || this.lastCharacter != this.currentCharacter.targetCharacter || base.battleStateData.commandSelectTweenTargetCamera.isMoving)
			{
				base.stateManager.uiControl.ApplyCurrentSelectArrow(true, base.stateManager.uiControl.GetHUDCenterPosition2DFunction(this.currentCharacter), this.currentCharacter.myIndex);
				base.stateManager.uiControl.RepositionCharacterHUDPosition(base.battleStateData.GetTotalCharacters());
			}
			if (this.isOutCharacterView)
			{
				this.lastCharacter = this.currentCharacter.targetCharacter;
			}
			yield return null;
		}
		if (base.hierarchyData.onAutoPlay == 2)
		{
			base.stateManager.targetSelect.AutoPlayCharacterAndSkillSelectFunction(this.currentCharacter);
		}
		else if (base.hierarchyData.onAutoPlay == 1)
		{
			base.stateManager.targetSelect.AutoPlayCharacterAndAttackSelectFunction(this.currentCharacter);
		}
		Block_10:
		base.stateManager.uiControl.ApplyCurrentSelectArrow(false, default(Vector3), 0);
		base.stateManager.uiControl.SetHudCollider(false);
		yield break;
	}

	protected virtual void UpdateTarget()
	{
		base.stateManager.targetSelect.TargetManualSelectAndApplyUIFunction(this.currentCharacter);
	}

	protected override void GetEventThisState(EventState eventState)
	{
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.uiControl.ApplyCurrentSelectArrow(false, default(Vector3), 0);
		base.stateManager.cameraControl.StopTweenCameraMotionAction(base.battleStateData.commandSelectTweenTargetCamera);
	}

	private void SetSelectCharacter(int index)
	{
		base.stateManager.uiControl.ApplySkillButtonReflesh();
		base.battleStateData.currentSelectCharacterIndex = index;
		CharacterStateControl characterStateControl = base.battleStateData.playerCharacters[index];
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			bool isSelect = i == index;
			base.stateManager.uiControl.ApplyMonsterButtonEnable(i, isSelect, base.battleStateData.playerCharacters[i].isDied);
		}
		bool flag = characterStateControl.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SkillLock);
		int num;
		if (characterStateControl.isVersionUp)
		{
			base.stateManager.uiControl.ApplyThreeSkillButtonPosition();
			num = 4;
		}
		else
		{
			base.stateManager.uiControl.ApplyTwoSkillButtonPosition();
			num = 3;
		}
		for (int j = 1; j < num; j++)
		{
			bool flag2 = j < characterStateControl.skillStatus.Length;
			base.stateManager.uiControl.ApplySkillButtonColliderActive(j, flag2);
			if (flag2)
			{
				bool onEnable = characterStateControl.isUseSkill(j);
				if (flag)
				{
					onEnable = false;
				}
				base.stateManager.uiControl.ApplySkillButtonData(j, characterStateControl.skillStatus[j], onEnable, flag, characterStateControl, characterStateControl.skillUseCounts[j]);
			}
			else
			{
				base.stateManager.uiControl.ApplySkillButtonData(j, new SkillStatus(), false, false, characterStateControl, 0);
			}
		}
	}
}
