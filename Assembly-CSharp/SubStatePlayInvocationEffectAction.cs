using System;
using System.Collections;

public class SubStatePlayInvocationEffectAction : BattleStateController
{
	private string gettedId = string.Empty;

	private string cameraKey = "skillF";

	private AlwaysEffectParams[] currentActiveRevivalEffect;

	private SkillStatus skillStatus;

	public SubStatePlayInvocationEffectAction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override IEnumerator MainRoutine()
	{
		CharacterStateControl currentCharacter = base.battleStateData.currentSelectCharacterState;
		if (base.battleStateData.IsChipSkill())
		{
			currentCharacter = base.battleStateData.GetAutoCounterCharacter();
		}
		string applyName = string.Empty;
		if (base.battleStateData.IsChipSkill())
		{
			this.skillStatus = base.hierarchyData.GetSkillStatus(currentCharacter.chipSkillId);
			GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(currentCharacter.currentChipId);
			applyName = chipMainData.name;
		}
		else
		{
			this.skillStatus = currentCharacter.currentSkillStatus;
			applyName = this.skillStatus.name;
		}
		base.stateManager.uiControl.ApplySkillName(true, applyName, currentCharacter);
		this.cameraKey = "skillF";
		if (!this.skillStatus.invocationEffectParams.cameraMotionId.Equals(string.Empty))
		{
			this.cameraKey = this.skillStatus.invocationEffectParams.cameraMotionId;
		}
		else
		{
			bool flag = false;
			if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
			{
				flag = true;
			}
			if (currentCharacter.isEnemy && flag)
			{
				this.cameraKey = "BigBoss/skillF";
			}
			else
			{
				this.cameraKey = "skillF";
			}
		}
		this.currentActiveRevivalEffect = base.battleStateData.GetIsActiveRevivalReservedEffect();
		foreach (AlwaysEffectParams alwaysEffectParams in this.currentActiveRevivalEffect)
		{
			base.stateManager.threeDAction.StopAlwaysEffectAction(new AlwaysEffectParams[]
			{
				alwaysEffectParams
			});
			base.stateManager.soundPlayer.TryStopSE(alwaysEffectParams);
		}
		if (this.skillStatus.TryGetInvocationSEID(out this.gettedId))
		{
			base.stateManager.soundPlayer.TryPlaySE(this.gettedId, 0f, false);
		}
		base.stateManager.threeDAction.HideAllCharactersAction(base.stateManager.battleStateData.GetTotalCharacters());
		currentCharacter.CharacterParams.gameObject.SetActive(true);
		currentCharacter.CharacterParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
		base.stateManager.cameraControl.PlayCameraMotionActionCharacter(this.cameraKey, currentCharacter);
		if (this.skillStatus.invocationEffectParams.isVoice)
		{
			bool lastOn2xSpeedPlay = base.stateManager.hierarchyData.on2xSpeedPlay;
			base.stateManager.soundPlayer.SetVolumeSE(lastOn2xSpeedPlay);
			IEnumerator playSkillAnimation = this.skillStatus.invocationEffectParams.PlaySkillAnimation(currentCharacter.CharacterParams);
			while (playSkillAnimation.MoveNext())
			{
				if (lastOn2xSpeedPlay != base.stateManager.hierarchyData.on2xSpeedPlay)
				{
					lastOn2xSpeedPlay = base.stateManager.hierarchyData.on2xSpeedPlay;
					base.stateManager.soundPlayer.SetVolumeSE(lastOn2xSpeedPlay);
				}
				yield return playSkillAnimation.Current;
			}
			base.stateManager.soundPlayer.SetVolumeSE(false);
		}
		else
		{
			IEnumerator playSkillAnimation2 = this.skillStatus.invocationEffectParams.PlaySkillAnimation(currentCharacter.CharacterParams);
			while (playSkillAnimation2.MoveNext())
			{
				object obj = playSkillAnimation2.Current;
				yield return obj;
			}
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		base.stateManager.soundPlayer.TryStopSE(this.gettedId, 0f);
		base.stateManager.cameraControl.StopCameraMotionAction(this.cameraKey);
		bool flag = false;
		foreach (AlwaysEffectParams alwaysEffect in this.currentActiveRevivalEffect)
		{
			base.stateManager.threeDAction.PlayAlwaysEffectAction(alwaysEffect, AlwaysEffectState.Always);
			if (!flag)
			{
				base.stateManager.soundPlayer.TryPlaySE(alwaysEffect, AlwaysEffectState.Always);
				flag = true;
			}
		}
	}

	protected override void GetEventThisState(EventState eventState)
	{
		this.skillStatus.invocationEffectParams.StopSkillAnimation();
	}
}
