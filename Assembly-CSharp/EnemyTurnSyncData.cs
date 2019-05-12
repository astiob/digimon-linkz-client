using System;

public sealed class EnemyTurnSyncData : TCPData<EnemyTurnSyncData>
{
	public string playerUserId;

	public string hashValue;
}
