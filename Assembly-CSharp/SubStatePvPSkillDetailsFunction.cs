using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStatePvPSkillDetailsFunction : SubStateSkillDetailsFunction
{
	public SubStatePvPSkillDetailsFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(OnExit, OnExitGotEvent)
	{
	}

	protected override void SetRandomSeed(AffectEffectProperty currentSuffer)
	{
		currentSuffer.SetRandomSeed(UnityEngine.Random.seed);
	}

	protected override void AfterEnemyDeadFunction(params CharacterStateControl[] currentDeathCharacters)
	{
	}

	protected override void SendBattleLogs(CharacterStateControl currentCharacter, List<BattleLogData.AttackLog> attackLog, List<BattleLogData.BuffLog> buffLog)
	{
		AttackData attackData = new AttackData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			selectSkillIdx = currentCharacter.isSelectSkill,
			targetIdx = currentCharacter.targetCharacter.myIndex,
			isTargetCharacterEnemy = currentCharacter.targetCharacter.isEnemy
		};
		bool isMyAction = !currentCharacter.isEnemy;
		int myIndex = currentCharacter.myIndex;
		Singleton<TCPMessageSender>.Instance.SendPvPBattleActionLog(attackData, myIndex, isMyAction, base.battleStateData, attackLog, buffLog);
	}

	protected override IEnumerator DropItem(CharacterStateControl[] currentDeadCharacters)
	{
		yield break;
	}
}
