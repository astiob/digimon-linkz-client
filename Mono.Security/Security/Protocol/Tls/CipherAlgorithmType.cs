using System;

namespace Mono.Security.Protocol.Tls
{
	[Serializable]
	public enum CipherAlgorithmType
	{
		Des,
		None,
		Rc2,
		Rc4,
		Rijndael,
		SkipJack,
		TripleDes
	}
}
