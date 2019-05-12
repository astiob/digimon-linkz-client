using System;
using System.Collections;

public class SubStatePlayInvocationEffectAction : BattleStateController
{
	private string gettedId = string.Empty;

	private string cameraKey = "skillF";

	private AlwaysEffectParams[] currentActiveRevivalEffect;

	private CharacterStateControl currentCharacter;

	private CharacterStateControl[] sortedCharacters;

	private SkillStatus skillStatus;

	public SubStatePlayInvocationEffectAction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void EnabledThisState()
	{
		this.currentCharacter = base.battleStateData.currentSelectCharacterState;
		this.sortedCharacters = base.battleStateData.GetTotalCharacters();
	}

	protected override IEnumerator MainRoutine()
	{
		if (base.battleStateData.IsChipSkill())
		{
			this.currentCharacter = base.battleStateData.GetAutoCounterCharacter();
		}
		string applyName = string.Empty;
		if (base.battleStateData.IsChipSkill())
		{
			this.skillStatus = base.hierarchyData.GetSkillStatus(this.currentCharacter.chipSkillId);
			GameWebAPI.RespDataMA_ChipM.Chip chipData = ChipDataMng.GetChipMainData(this.currentCharacter.currentChipId);
			applyName = chipData.name;
		}
		else
		{
			this.skillStatus = this.currentCharacter.currentSkillStatus;
			applyName = this.skillStatus.name;
		}
		base.stateManager.uiControl.ApplySkillName(true, applyName, this.currentCharacter);
		base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(this.sortedCharacters);
		this.cameraKey = "skillF";
		if (!this.skillStatus.invocationEffectParams.cameraMotionId.Equals(string.Empty))
		{
			this.cameraKey = this.skillStatus.invocationEffectParams.cameraMotionId;
		}
		else
		{
			bool isBigBoss = false;
			if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
			{
				isBigBoss = true;
			}
			if (this.currentCharacter.isEnemy && isBigBoss)
			{
				this.cameraKey = "BigBoss/skillF";
			}
			else
			{
				this.cameraKey = "skillF";
			}
		}
		base.stateManager.threeDAction.HideAllCharactersAction(this.sortedCharacters);
		this.currentCharacter.CharacterParams.gameObject.SetActive(true);
		this.currentActiveRevivalEffect = base.battleStateData.GetIsActiveRevivalReservedEffect();
		foreach (AlwaysEffectParams a in this.currentActiveRevivalEffect)
		{
			base.stateManager.threeDAction.StopAlwaysEffectAction(new AlwaysEffectParams[]
			{
				a
			});
			base.stateManager.soundPlayer.TryStopSE(a);
		}
		if (this.skillStatus.TryGetInvocationSEID(out this.gettedId))
		{
			base.stateManager.soundPlayer.TryPlaySE(this.gettedId, 0f, false);
		}
		base.stateManager.cameraControl.PlayCameraMotionActionCharacter(this.cameraKey, this.currentCharacter);
		IEnumerator playSkillAnimation = this.skillStatus.invocationEffectParams.PlaySkillAnimation(this.currentCharacter.CharacterParams);
		while (playSkillAnimation.MoveNext())
		{
			object obj = playSkillAnimation.Current;
			yield return obj;
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
