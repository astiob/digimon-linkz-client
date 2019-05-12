using System;

namespace Mono.Security.Protocol.Tls
{
	[Serializable]
	internal enum AlertLevel : byte
	{
		Warning = 1,
		Fatal
	}
}
