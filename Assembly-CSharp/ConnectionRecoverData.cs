using System;

public sealed class ConnectionRecoverData : TCPData<ConnectionRecoverData>
{
	public string playerUserId;

	public string hashValue;

	public string[] failedUserIds;

	public int randomSeed;
}
