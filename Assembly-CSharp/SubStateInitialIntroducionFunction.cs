using System;
using System.Collections;

public class SubStateInitialIntroducionFunction : BattleStateBase
{
	public SubStateInitialIntroducionFunction(Action OnExit) : base(null, OnExit)
	{
	}

	public SubStateInitialIntroducionFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override IEnumerator MainRoutine()
	{
		if (base.isMulti)
		{
			yield break;
		}
		if (!base.hierarchyData.useInitialIntroduction)
		{
			yield break;
		}
		if (base.battleStateData.isBattleRetired)
		{
			yield break;
		}
		if (base.hierarchyData.initialIntroductionIndex >= base.hierarchyData.maxInitialIntroductionIndex)
		{
			yield break;
		}
		base.battleStateData.isShowInitialIntroduction = true;
		IEnumerator wait = base.stateManager.uiControl.ApplyShowInitialInduction(base.hierarchyData.initialIntroductionIndex);
		while (wait.MoveNext())
		{
			yield return null;
		}
		yield break;
	}
}
