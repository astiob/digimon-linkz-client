using System;

namespace WebSocketSharp
{
	public enum Opcode : byte
	{
		Cont,
		Text,
		Binary,
		Close = 8,
		Ping,
		Pong
	}
}
