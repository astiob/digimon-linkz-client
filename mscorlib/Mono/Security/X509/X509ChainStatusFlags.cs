using System;

namespace Mono.Security.X509
{
	[Flags]
	[Serializable]
	internal enum X509ChainStatusFlags
	{
		InvalidBasicConstraints = 1024,
		NoError = 0,
		NotSignatureValid = 8,
		NotTimeNested = 2,
		NotTimeValid = 1,
		PartialChain = 65536,
		UntrustedRoot = 32
	}
}
