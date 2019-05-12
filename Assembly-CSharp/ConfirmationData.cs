using System;

public sealed class ConfirmationData : TCPData<ConfirmationData>
{
	public string playerUserId;

	public string hashValue;

	public int tcpMessageType;
}
