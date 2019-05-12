using System;

namespace Mono.Security.Protocol.Tls
{
	[Serializable]
	public enum ExchangeAlgorithmType
	{
		DiffieHellman,
		Fortezza,
		None,
		RsaKeyX,
		RsaSign
	}
}
