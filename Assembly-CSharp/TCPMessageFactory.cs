using System;
using System.Collections.Generic;

public class TCPMessageFactory : ClassSingleton<TCPMessageFactory>, ITCPMessageFactory
{
	public Dictionary<string, object> CreateMessage(TCPMessageType type, object tcpMessage)
	{
		return new Dictionary<string, object>
		{
			{
				type.ToString(),
				tcpMessage
			}
		};
	}
}
