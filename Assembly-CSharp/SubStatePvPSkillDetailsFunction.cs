using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;

public class SubStatePvPSkillDetailsFunction : SubStateSkillDetailsFunction
{
	public SubStatePvPSkillDetailsFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(OnExit, OnExitGotEvent)
	{
	}

	protected override void AfterEnemyDeadFunction(params CharacterStateControl[] currentDeathCharacters)
	{
	}

	protected override IEnumerator SendBattleLogs(CharacterStateControl currentCharacter, List<BattleLogData.AttackLog> attackLog, List<BattleLogData.BuffLog> buffLog)
	{
		int count = MasterDataMng.Instance().RespDataMA_CodeM.codeM.PVP_ACTION_LOG_RETRY_COUNT;
		float time = 0f;
		AttackData attackData = new AttackData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			selectSkillIdx = currentCharacter.isSelectSkill,
			targetIdx = currentCharacter.targetCharacter.myIndex,
			isTargetCharacterEnemy = currentCharacter.targetCharacter.isEnemy
		};
		bool isMyAction = !currentCharacter.isEnemy;
		int attackerIndex = currentCharacter.myIndex;
		if (MasterDataMng.Instance().RespDataMA_CodeM.codeM.PVP_BATTLE_ACTION_LOG == 1)
		{
			while (count > 0)
			{
				if (BattlePvPFunction.battleActionLogResult == 1)
				{
					BattlePvPFunction.battleActionLogResult = -1;
					break;
				}
				time -= 0.2f;
				if (time < 0f)
				{
					time = (float)MasterDataMng.Instance().RespDataMA_CodeM.codeM.PVP_ACTION_LOG_RETRY_TIME;
					Singleton<TCPMessageSender>.Instance.SendPvPBattleActionLog(attackData, attackerIndex, isMyAction, base.battleStateData, attackLog, buffLog);
					count--;
				}
				yield return null;
			}
			yield break;
		}
		Singleton<TCPMessageSender>.Instance.SendPvPBattleActionLog(attackData, attackerIndex, isMyAction, base.battleStateData, attackLog, buffLog);
		yield break;
	}

	protected override IEnumerator DropItem(CharacterStateControl[] currentDeadCharacters)
	{
		yield break;
	}
}
