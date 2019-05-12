using Mono.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Mono.Security.Protocol.Tls
{
	internal class RSASslSignatureFormatter : AsymmetricSignatureFormatter
	{
		private RSA key;

		private HashAlgorithm hash;

		public RSASslSignatureFormatter()
		{
		}

		public RSASslSignatureFormatter(AsymmetricAlgorithm key)
		{
			this.SetKey(key);
		}

		public override byte[] CreateSignature(byte[] rgbHash)
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
			return PKCS1.Sign_v15(this.key, this.hash, rgbHash);
		}

		public override void SetHashAlgorithm(string strName)
		{
			if (strName != null)
			{
				if (RSASslSignatureFormatter.<>f__switch$map16 == null)
				{
					RSASslSignatureFormatter.<>f__switch$map16 = new Dictionary<string, int>(1)
					{
						{
							"MD5SHA1",
							0
						}
					};
				}
				int num;
				if (RSASslSignatureFormatter.<>f__switch$map16.TryGetValue(strName, out num))
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
