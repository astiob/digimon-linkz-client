using System;
using System.Security.Cryptography;

namespace Mono.Security.Cryptography
{
	internal class HMAC : KeyedHashAlgorithm
	{
		private HashAlgorithm hash;

		private bool hashing;

		private byte[] innerPad;

		private byte[] outerPad;

		public HMAC()
		{
			this.hash = MD5.Create();
			this.HashSizeValue = this.hash.HashSize;
			byte[] array = new byte[64];
			RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider();
			rngcryptoServiceProvider.GetNonZeroBytes(array);
			this.KeyValue = (byte[])array.Clone();
			this.Initialize();
		}

		public HMAC(string hashName, byte[] rgbKey)
		{
			if (hashName == null || hashName.Length == 0)
			{
				hashName = "MD5";
			}
			this.hash = HashAlgorithm.Create(hashName);
			this.HashSizeValue = this.hash.HashSize;
			if (rgbKey.Length > 64)
			{
				this.KeyValue = this.hash.ComputeHash(rgbKey);
			}
			else
			{
				this.KeyValue = (byte[])rgbKey.Clone();
			}
			this.Initialize();
		}

		public override byte[] Key
		{
			get
			{
				return (byte[])this.KeyValue.Clone();
			}
			set
			{
				if (this.hashing)
				{
					throw new Exception("Cannot change key during hash operation.");
				}
				if (value.Length > 64)
				{
					this.KeyValue = this.hash.ComputeHash(value);
				}
				else
				{
					this.KeyValue = (byte[])value.Clone();
				}
				this.initializePad();
			}
		}

		public override void Initialize()
		{
			this.hash.Initialize();
			this.initializePad();
			this.hashing = false;
		}

		protected override byte[] HashFinal()
		{
			if (!this.hashing)
			{
				this.hash.TransformBlock(this.innerPad, 0, this.innerPad.Length, this.innerPad, 0);
				this.hashing = true;
			}
			this.hash.TransformFinalBlock(new byte[0], 0, 0);
			byte[] array = this.hash.Hash;
			this.hash.Initialize();
			this.hash.TransformBlock(this.outerPad, 0, this.outerPad.Length, this.outerPad, 0);
			this.hash.TransformFinalBlock(array, 0, array.Length);
			this.Initialize();
			return this.hash.Hash;
		}

		protected override void HashCore(byte[] array, int ibStart, int cbSize)
		{
			if (!this.hashing)
			{
				this.hash.TransformBlock(this.innerPad, 0, this.innerPad.Length, this.innerPad, 0);
				this.hashing = true;
			}
			this.hash.TransformBlock(array, ibStart, cbSize, array, ibStart);
		}

		private void initializePad()
		{
			this.innerPad = new byte[64];
			this.outerPad = new byte[64];
			for (int i = 0; i < this.KeyValue.Length; i++)
			{
				this.innerPad[i] = (this.KeyValue[i] ^ 54);
				this.outerPad[i] = (this.KeyValue[i] ^ 92);
			}
			for (int j = this.KeyValue.Length; j < 64; j++)
			{
				this.innerPad[j] = 54;
				this.outerPad[j] = 92;
			}
		}
	}
}
