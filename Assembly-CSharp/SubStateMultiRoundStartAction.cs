using System;
using System.Collections;

public class SubStateMultiRoundStartAction : SubStateRoundStartAction
{
	public SubStateMultiRoundStartAction(Action OnExit, Action<EventState> OnExitGotEvent) : base(OnExit, OnExitGotEvent)
	{
	}

	protected override IEnumerator RoundStartCameraMotionFunction()
	{
		CharacterStateControl[] totalCharacters = base.battleStateData.GetTotalCharacters();
		bool isHPWait = false;
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			if (!base.battleStateData.playerCharacters[i].isDied)
			{
				bool isRevivalAp = base.battleStateData.isRoundStartApRevival[i];
				bool isRevivalHp = base.battleStateData.isRoundStartHpRevival[i];
				base.stateManager.uiControlMulti.ApplyHUDRecoverMulti(i, isRevivalAp, isRevivalHp, totalCharacters[i].upAp);
				if (isRevivalHp)
				{
					isHPWait = true;
				}
			}
		}
		for (int j = 0; j < totalCharacters.Length; j++)
		{
			totalCharacters[j].skillOrder = -1;
		}
		bool isEndPlayApUpAnimations = false;
		base.stateManager.uiControlMulti.PlayApUpAnimations(delegate
		{
			isEndPlayApUpAnimations = true;
		});
		while (!isEndPlayApUpAnimations)
		{
			yield return null;
		}
		if (isHPWait)
		{
			IEnumerator asdWait = base.stateManager.time.WaitForCertainPeriodTimeAction(1f, null, null);
			while (asdWait.MoveNext())
			{
				yield return null;
			}
		}
		for (int k = 0; k < base.battleStateData.playerCharacters.Length; k++)
		{
			base.stateManager.uiControlMulti.HideHUDRecoverMulti(k);
		}
		yield break;
	}
}
