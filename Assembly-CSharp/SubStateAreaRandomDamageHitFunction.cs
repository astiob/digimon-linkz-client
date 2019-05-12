using System;
using System.Collections;
using UnityEngine;
using UnityExtension;

public class SubStateAreaRandomDamageHitFunction : BattleStateController
{
	public SubStateAreaRandomDamageHitFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void AwakeThisState()
	{
		base.AddState(new SubStateSkillDetailsFunction(null, new Action<EventState>(base.SendEventState)));
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
		SufferStateProperty confusionSuffer = attacker.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.Confusion);
		if (confusionSuffer.isActive && confusionSuffer.GetOccurrenceFreeze())
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
		base.SetState(typeof(SubStateSkillDetailsFunction));
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}
}
