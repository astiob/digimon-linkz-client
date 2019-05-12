using Mono.Security.Protocol.Tls;
using System;
using System.Security.Cryptography;

namespace Mono.Security.Cryptography
{
	internal class MD5SHA1 : HashAlgorithm
	{
		private HashAlgorithm md5;

		private HashAlgorithm sha;

		private bool hashing;

		public MD5SHA1()
		{
			this.md5 = MD5.Create();
			this.sha = SHA1.Create();
			this.HashSizeValue = this.md5.HashSize + this.sha.HashSize;
		}

		public override void Initialize()
		{
			this.md5.Initialize();
			this.sha.Initialize();
			this.hashing = false;
		}

		protected override byte[] HashFinal()
		{
			if (!this.hashing)
			{
				this.hashing = true;
			}
			this.md5.TransformFinalBlock(new byte[0], 0, 0);
			this.sha.TransformFinalBlock(new byte[0], 0, 0);
			byte[] array = new byte[36];
			Buffer.BlockCopy(this.md5.Hash, 0, array, 0, 16);
			Buffer.BlockCopy(this.sha.Hash, 0, array, 16, 20);
			return array;
		}

		protected override void HashCore(byte[] array, int ibStart, int cbSize)
		{
			if (!this.hashing)
			{
				this.hashing = true;
			}
			this.md5.TransformBlock(array, ibStart, cbSize, array, ibStart);
			this.sha.TransformBlock(array, ibStart, cbSize, array, ibStart);
		}

		public byte[] CreateSignature(RSA rsa)
		{
			if (rsa == null)
			{
				throw new CryptographicUnexpectedOperationException("missing key");
			}
			RSASslSignatureFormatter rsasslSignatureFormatter = new RSASslSignatureFormatter(rsa);
			rsasslSignatureFormatter.SetHashAlgorithm("MD5SHA1");
			return rsasslSignatureFormatter.CreateSignature(this.Hash);
		}

		public bool VerifySignature(RSA rsa, byte[] rgbSignature)
		{
			if (rsa == null)
			{
				throw new CryptographicUnexpectedOperationException("missing key");
			}
			if (rgbSignature == null)
			{
				throw new ArgumentNullException("rgbSignature");
			}
			RSASslSignatureDeformatter rsasslSignatureDeformatter = new RSASslSignatureDeformatter(rsa);
			rsasslSignatureDeformatter.SetHashAlgorithm("MD5SHA1");
			return rsasslSignatureDeformatter.VerifySignature(this.Hash, rgbSignature);
		}
	}
}
