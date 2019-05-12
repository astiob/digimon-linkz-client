using System;

namespace Mono.Security.Protocol.Tls
{
	[Serializable]
	internal enum ContentType : byte
	{
		ChangeCipherSpec = 20,
		Alert,
		Handshake,
		ApplicationData
	}
}
