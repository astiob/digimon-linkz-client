using System;

public sealed class RevivalCancelData : TCPData<RevivalCancelData>
{
	public string playerUserId;

	public string hashValue;

	public string[] cancelRevivalUserIds;
}
