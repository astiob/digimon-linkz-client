using System;
using System.Collections.Generic;

internal interface ITCPMessageFactory
{
	Dictionary<string, object> CreateMessage(TCPMessageType type, object tcpMessage);
}
