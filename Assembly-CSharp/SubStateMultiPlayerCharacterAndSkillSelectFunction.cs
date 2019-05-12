using System;

public class SubStateMultiPlayerCharacterAndSkillSelectFunction : SubStatePlayerCharacterAndSkillSelectFunction
{
	private CharacterStateControl lastTargetCharacter;

	public SubStateMultiPlayerCharacterAndSkillSelectFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(OnExit, OnExitGotEvent)
	{
	}

	protected override void EnabledThisState()
	{
		base.EnabledThisState();
		if (base.hierarchyData.onAutoPlay == 0)
		{
			base.stateManager.uiControlMulti.ApplySkillSelectUI(base.stateManager.multiFunction.isMyTurn);
		}
		this.lastTargetCharacter = null;
	}

	protected override void UpdateTarget()
	{
		base.UpdateTarget();
		if (base.stateManager.multiFunction.isMyTurn && this.currentCharacter.targetCharacter != null && this.lastTargetCharacter != this.currentCharacter.targetCharacter)
		{
			this.lastTargetCharacter = this.currentCharacter.targetCharacter;
			base.stateManager.multiFunction.SendTarget();
		}
	}

	protected override void DisabledThisState()
	{
		base.DisabledThisState();
		base.stateManager.uiControlMulti.StartSharedAPAnimation();
		base.stateManager.targetSelect.TargetManualSelectAndApplyUIFunction(null);
		base.stateManager.uiControlMulti.ApplySkillSelectUI(false);
	}
}
