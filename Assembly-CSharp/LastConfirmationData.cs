using System;

public sealed class LastConfirmationData : TCPData<LastConfirmationData>
{
	public string playerUserId;

	public string hashValue;

	public int tcpMessageType;

	public string[] failedPlayerUserIds;
}
