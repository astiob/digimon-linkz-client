using System;

namespace System.Security.Cryptography
{
	internal class DSASignatureDescription : SignatureDescription
	{
		public DSASignatureDescription()
		{
			base.DeformatterAlgorithm = "System.Security.Cryptography.DSASignatureDeformatter";
			base.DigestAlgorithm = "System.Security.Cryptography.SHA1CryptoServiceProvider";
			base.FormatterAlgorithm = "System.Security.Cryptography.DSASignatureFormatter";
			base.KeyAlgorithm = "System.Security.Cryptography.DSACryptoServiceProvider";
		}
	}
}
