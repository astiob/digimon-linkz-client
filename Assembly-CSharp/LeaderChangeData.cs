using System;

public sealed class LeaderChangeData : TCPData<LeaderChangeData>
{
	public string playerUserId;

	public string hashValue;

	public int leaderIndex;
}
