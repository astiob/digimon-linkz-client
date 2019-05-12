using System;

namespace Mono.Security.Protocol.Tls.Handshake
{
	[Serializable]
	internal enum HandshakeType : byte
	{
		HelloRequest,
		ClientHello,
		ServerHello,
		Certificate = 11,
		ServerKeyExchange,
		CertificateRequest,
		ServerHelloDone,
		CertificateVerify,
		ClientKeyExchange,
		Finished = 20,
		None = 255
	}
}
