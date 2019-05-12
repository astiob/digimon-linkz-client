using System;

namespace Mono.Security.Protocol.Tls.Handshake
{
	[Serializable]
	internal enum ClientCertificateType
	{
		RSA = 1,
		DSS,
		RSAFixed,
		DSSFixed,
		Unknown = 255
	}
}
