using System;
using System.Collections;

public class SubStatePlayPassiveEffectFunction : BattleStateBase
{
	private string gettedId = string.Empty;

	private SubStatePlayPassiveEffectFunction.Data data;

	public SubStatePlayPassiveEffectFunction(Action OnExit, Action<EventState> OnExitGotEven) : base(null, OnExit, OnExitGotEven)
	{
	}

	public void Init(SubStatePlayPassiveEffectFunction.Data data)
	{
		this.data = data;
	}

	protected override IEnumerator MainRoutine()
	{
		this.gettedId = string.Empty;
		if (this.data.skillStatus.TryGetPassiveSEID(out this.gettedId))
		{
			base.stateManager.soundPlayer.TryPlaySE(this.gettedId, 0f, false);
		}
		for (int i = 1; i < this.data.targets.Length; i++)
		{
			if (!this.data.targets[i].isDied)
			{
				this.data.targets[i].CharacterParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
				this.data.skillStatus.passiveEffectParams[i].PlayAnimation(this.data.targets[i].CharacterParams);
			}
		}
		IEnumerator passiveEffectparams = this.data.skillStatus.passiveEffectParams[0].PlayAnimationCorutine(this.data.targets[0].CharacterParams);
		while (passiveEffectparams.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		foreach (PassiveEffectParams passiveEffectParams2 in this.data.skillStatus.passiveEffectParams)
		{
			passiveEffectParams2.StopAnimation();
		}
		base.stateManager.soundPlayer.TryStopSE(this.gettedId, 0f);
	}

	public class Data
	{
		public CharacterStateControl[] targets;

		public SkillStatus skillStatus;
	}
}
