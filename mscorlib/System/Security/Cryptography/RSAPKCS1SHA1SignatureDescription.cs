using System;

namespace System.Security.Cryptography
{
	internal class RSAPKCS1SHA1SignatureDescription : SignatureDescription
	{
		public RSAPKCS1SHA1SignatureDescription()
		{
			base.DeformatterAlgorithm = "System.Security.Cryptography.RSAPKCS1SignatureDeformatter";
			base.DigestAlgorithm = "System.Security.Cryptography.SHA1CryptoServiceProvider";
			base.FormatterAlgorithm = "System.Security.Cryptography.RSAPKCS1SignatureFormatter";
			base.KeyAlgorithm = "System.Security.Cryptography.RSACryptoServiceProvider";
		}

		public override AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
		{
			return base.CreateDeformatter(key);
		}
	}
}
