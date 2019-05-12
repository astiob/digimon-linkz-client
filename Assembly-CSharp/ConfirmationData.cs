using System;

public class ConfirmationData : TCPData<ConfirmationData>
{
	public string playerUserId;

	public string hashValue;

	public int tcpMessageType;

	public string value1;
}
