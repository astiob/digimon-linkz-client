using System;

public sealed class TargetData : TCPData<TargetData>
{
	public string playerUserId;

	public string hashValue;

	public int attckerIndex;

	public int selectSkillIdx;

	public int targetIdx;

	public bool isTargetCharacterEnemy;
}
