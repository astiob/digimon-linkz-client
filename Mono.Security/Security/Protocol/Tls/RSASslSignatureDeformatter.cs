using Mono.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Mono.Security.Protocol.Tls
{
	internal class RSASslSignatureDeformatter : AsymmetricSignatureDeformatter
	{
		private RSA key;

		private HashAlgorithm hash;

		public RSASslSignatureDeformatter()
		{
		}

		public RSASslSignatureDeformatter(AsymmetricAlgorithm key)
		{
			this.SetKey(key);
		}

		public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
		{
			if (this.key == null)
			{
				throw new CryptographicUnexpectedOperationException("The key is a null reference");
			}
			if (this.hash == null)
			{
				throw new CryptographicUnexpectedOperationException("The hash algorithm is a null reference.");
			}
			if (rgbHash == null)
			{
				throw new ArgumentNullException("The rgbHash parameter is a null reference.");
			}
			return PKCS1.Verify_v15(this.key, this.hash, rgbHash, rgbSignature);
		}

		public override void SetHashAlgorithm(string strName)
		{
			if (strName != null)
			{
				if (RSASslSignatureDeformatter.<>f__switch$map15 == null)
				{
					RSASslSignatureDeformatter.<>f__switch$map15 = new Dictionary<string, int>(1)
					{
						{
							"MD5SHA1",
							0
						}
					};
				}
				int num;
				if (RSASslSignatureDeformatter.<>f__switch$map15.TryGetValue(strName, out num))
				{
					if (num == 0)
					{
						this.hash = new MD5SHA1();
						return;
					}
				}
			}
			this.hash = HashAlgorithm.Create(strName);
		}

		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (!(key is RSA))
			{
				throw new ArgumentException("Specfied key is not an RSA key");
			}
			this.key = (key as RSA);
		}
	}
}
