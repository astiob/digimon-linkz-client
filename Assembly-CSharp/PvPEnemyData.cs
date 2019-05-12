using System;

public sealed class PvPEnemyData : TCPData<PvPEnemyData>
{
	public string playerUserId;

	public string hashValue;

	public string[] indexId;
}
