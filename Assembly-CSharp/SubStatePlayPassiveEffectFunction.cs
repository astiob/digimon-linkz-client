using System;
using System.Collections;

public class SubStatePlayPassiveEffectFunction : BattleStateBase
{
	private CharacterStateControl[] isTargetsStatus;

	private SkillStatus status;

	private AffectEffectProperty currentSuffer;

	private string gettedId = string.Empty;

	public SubStatePlayPassiveEffectFunction(Action OnExit, Action<EventState> OnExitGotEven) : base(null, OnExit, OnExitGotEven)
	{
	}

	protected override void EnabledThisState()
	{
		this.isTargetsStatus = (base.battleStateData.sendValues["isTargetsStatus"] as CharacterStateControl[]);
		this.status = (base.battleStateData.sendValues["status"] as SkillStatus);
		this.currentSuffer = (base.battleStateData.sendValues["currentSuffer"] as AffectEffectProperty);
	}

	protected override IEnumerator MainRoutine()
	{
		this.gettedId = string.Empty;
		if (this.status.TryGetPassiveSEID(out this.gettedId))
		{
			base.stateManager.soundPlayer.TryPlaySE(this.gettedId, 0f, false);
		}
		for (int i = 1; i < this.isTargetsStatus.Length; i++)
		{
			if (!this.isTargetsStatus[i].isDied)
			{
				base.stateManager.threeDAction.PlayIdleAnimationCharactersAction(new CharacterStateControl[]
				{
					this.isTargetsStatus[i]
				});
				this.status.passiveEffectParams[i].PlayAnimation(this.isTargetsStatus[i].CharacterParams);
			}
		}
		IEnumerator passiveEffectparams = this.status.passiveEffectParams[0].PlayAnimationCorutine(this.isTargetsStatus[0].CharacterParams);
		while (passiveEffectparams.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		foreach (PassiveEffectParams passiveEffectParams2 in this.status.passiveEffectParams)
		{
			passiveEffectParams2.StopAnimation();
		}
		base.stateManager.soundPlayer.TryStopSE(this.gettedId, 0f);
	}
}
