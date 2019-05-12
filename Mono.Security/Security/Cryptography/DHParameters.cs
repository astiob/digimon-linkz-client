using System;

namespace Mono.Security.Cryptography
{
	[Serializable]
	public struct DHParameters
	{
		public byte[] P;

		public byte[] G;

		[NonSerialized]
		public byte[] X;
	}
}
