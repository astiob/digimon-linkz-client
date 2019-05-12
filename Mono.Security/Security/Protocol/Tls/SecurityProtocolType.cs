using System;

namespace Mono.Security.Protocol.Tls
{
	[Flags]
	[Serializable]
	public enum SecurityProtocolType
	{
		Default = -1073741824,
		Ssl2 = 12,
		Ssl3 = 48,
		Tls = 192
	}
}
