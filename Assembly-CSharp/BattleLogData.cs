using System;
using System.Collections.Generic;

public sealed class BattleLogData
{
	public string playerUserId;

	public int attackerIndex;

	public int selectSkillIdx;

	public int targetIdx;

	public bool isTargetCharacterEnemy;

	public bool isMyAction;

	public int round;

	public int turn;

	public List<BattleLogData.AttackLog> attackLog;

	public List<BattleLogData.BuffLog> buffLog;

	public class AttackLog
	{
		public int index;

		public List<int> damage;

		public List<bool> miss;

		public List<bool> critical;

		public bool isDead;

		public List<int> startUpChip;
	}

	public class BuffLog
	{
		public int index;

		public bool miss;

		public AffectEffect effectType;

		public int chipUserIndex;

		public int value;

		public List<int> startUpChip;
	}
}
