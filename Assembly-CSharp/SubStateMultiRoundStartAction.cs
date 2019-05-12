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
		bool isSetCallback = false;
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
				if (isRevivalAp)
				{
					base.stateManager.uiControlMulti.isPlayingSharedAp = true;
					if (!isSetCallback)
					{
						isSetCallback = true;
						HitEffectParams hitEffectParams = base.battleStateData.roundChangeApRevivalEffect[i];
						base.stateManager.uiControlMulti.APEffectCallBackMulti(hitEffectParams);
					}
				}
			}
		}
		if (base.battleStateData.currentWaveNumber > 0 || base.battleStateData.currentRoundNumber > 1)
		{
			base.stateManager.uiControlMulti.ResetHUD(totalCharacters);
			base.stateManager.uiControl.ShowCharacterHUDFunction(totalCharacters);
			base.stateManager.uiControlMulti.RefreshSharedAP(false);
			for (int j = 0; j < totalCharacters.Length; j++)
			{
				totalCharacters[j].skillOrder = -1;
			}
			yield return null;
			base.stateManager.uiControl.RepositionCharacterHUDPosition(totalCharacters);
			while (base.stateManager.uiControlMulti.isPlayingSharedAp)
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
		}
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.RoundStartActionWaitSecond, null, null);
		while (wait.MoveNext())
		{
			yield return null;
		}
		yield break;
	}
}
