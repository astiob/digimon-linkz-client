using System;
using System.Collections;
using UnityEngine;
using UnityExtension;

public class SubStatePvPAreaRandomDamageHitFunction : BattleStateController
{
	public SubStatePvPAreaRandomDamageHitFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void AwakeThisState()
	{
		base.AddState(new SubStatePvPSkillDetailsFunction(null, new Action<EventState>(base.SendEventState)));
	}

	protected override IEnumerator MainRoutine()
	{
		CharacterStateControl attacker = base.battleStateData.currentSelectCharacterState;
		if ((!attacker.isEnemy && !RandomExtension.Switch(base.hierarchyData.playerPursuitPercentage, null)) || (attacker.isEnemy && !RandomExtension.Switch(base.hierarchyData.enemyPursuitPercentage, null)))
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
		do
		{
			attacker.targetCharacter = targets[UnityEngine.Random.Range(0, targets.Length)];
		}
		while (attacker.targetCharacter.isDied);
		attacker.isSelectSkill = attacker.SkillIdToIndexOf(base.stateManager.publicAttackSkillId);
		base.SetState(typeof(SubStatePvPSkillDetailsFunction));
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}
}
