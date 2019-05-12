using System;

namespace Mono.Security.Protocol.Tls
{
	[Serializable]
	internal enum HandshakeState
	{
		None,
		Started,
		Finished
	}
}
