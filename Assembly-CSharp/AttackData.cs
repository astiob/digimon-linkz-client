using System;

public sealed class AttackData : TCPData<AttackData>
{
	public string playerUserId;

	public string hashValue;

	public int selectSkillIdx;

	public int targetIdx;

	public bool isTargetCharacterEnemy;
}
