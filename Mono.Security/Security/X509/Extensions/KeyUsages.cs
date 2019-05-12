using System;

namespace Mono.Security.X509.Extensions
{
	[Flags]
	public enum KeyUsages
	{
		digitalSignature = 128,
		nonRepudiation = 64,
		keyEncipherment = 32,
		dataEncipherment = 16,
		keyAgreement = 8,
		keyCertSign = 4,
		cRLSign = 2,
		encipherOnly = 1,
		decipherOnly = 2048,
		none = 0
	}
}
