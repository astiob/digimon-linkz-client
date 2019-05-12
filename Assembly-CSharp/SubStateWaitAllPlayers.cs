using System;
using System.Collections;

public class SubStateWaitAllPlayers : BattleStateController
{
	public SubStateWaitAllPlayers(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void EnabledThisState()
	{
		base.stateManager.multiBasicFunction.OnSkillTrigger();
		base.stateManager.uiControlMultiBasic.HideLoading();
		base.stateManager.uiControlMultiBasic.ShowLoading(false);
	}

	protected override IEnumerator MainRoutine()
	{
		CharacterStateControl[] allCharacters = base.battleStateData.GetTotalCharacters();
		IEnumerator action = base.stateManager.multiBasicFunction.SendAttack();
		while (action.MoveNext())
		{
			base.stateManager.uiControl.RepositionCharacterHUDPosition(allCharacters);
			yield return action.Current;
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		base.stateManager.uiControlMultiBasic.HideLoading();
	}

	protected override void GetEventThisState(EventState eventState)
	{
	}
}
