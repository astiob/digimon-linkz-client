using Master;
using System;
using System.Collections;
using UnityEngine;
using UnityExtension;

public class SubStateMultiAreaRandomDamageHitFunction : BattleStateController
{
	public SubStateMultiAreaRandomDamageHitFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void AwakeThisState()
	{
		base.AddState(new SubStateMultiSkillDetailsFunction(null, new Action<EventState>(base.SendEventState)));
		base.AddState(new SubStatePlayChipEffect(null, new Action<EventState>(base.SendEventState), null));
	}

	protected override IEnumerator MainRoutine()
	{
		CharacterStateControl attacker = base.battleStateData.currentSelectCharacterState;
		if ((!attacker.isEnemy && !RandomExtension.Switch(base.hierarchyData.playerPursuitPercentage)) || (attacker.isEnemy && !RandomExtension.Switch(base.hierarchyData.enemyPursuitPercentage)))
		{
			yield break;
		}
		if (!attacker.currentSkillStatus.ThisSkillIsAttack)
		{
			yield break;
		}
		bool isConfusion = false;
		if (attacker.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Confusion) && attacker.currentSufferState.onConfusion.GetOccurrenceFreeze())
		{
			isConfusion = true;
		}
		CharacterStateControl[] targets;
		if (!isConfusion)
		{
			targets = ((!attacker.isEnemy) ? base.battleStateData.enemies : base.battleStateData.playerCharacters);
		}
		else
		{
			targets = (attacker.isEnemy ? base.battleStateData.enemies : base.battleStateData.playerCharacters);
		}
		int count = 0;
		for (int i = 0; i < targets.Length; i++)
		{
			if (targets[i].isDied)
			{
				count++;
			}
		}
		if (count == targets.Length)
		{
			yield break;
		}
		int index = 0;
		do
		{
			index = UnityEngine.Random.Range(0, targets.Length);
			attacker.targetCharacter = targets[index];
		}
		while (attacker.targetCharacter.isDied);
		base.stateManager.uiControl.ApplyTurnActionBarSwipeout(true);
		base.stateManager.uiControl.ApplySkillName(true, StringMaster.GetString("BattleUI-46"), attacker);
		IEnumerator wait2 = base.stateManager.time.WaitForCertainPeriodTimeAction(0.5f, null, null);
		while (wait2.MoveNext())
		{
			object obj = wait2.Current;
			yield return obj;
		}
		base.stateManager.uiControl.ApplyTurnActionBarSwipeout(false);
		attacker.isSelectSkill = attacker.SkillIdToIndexOf(base.stateManager.publicAttackSkillId);
		attacker.isMultiAreaRandomDamageSkill = true;
		base.SetState(typeof(SubStateMultiSkillDetailsFunction));
		while (base.isWaitState)
		{
			yield return null;
		}
		base.SetState(typeof(SubStatePlayChipEffect));
		while (base.isWaitState)
		{
			yield return null;
		}
		attacker.isMultiAreaRandomDamageSkill = false;
		yield break;
	}
}
