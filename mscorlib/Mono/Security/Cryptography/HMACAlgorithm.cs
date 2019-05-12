using System;
using System.Security.Cryptography;

namespace Mono.Security.Cryptography
{
	internal class HMACAlgorithm
	{
		private byte[] key;

		private byte[] hash;

		private HashAlgorithm algo;

		private string hashName;

		private BlockProcessor block;

		public HMACAlgorithm(string algoName)
		{
			this.CreateHash(algoName);
		}

		~HMACAlgorithm()
		{
			this.Dispose();
		}

		private void CreateHash(string algoName)
		{
			this.algo = HashAlgorithm.Create(algoName);
			this.hashName = algoName;
			this.block = new BlockProcessor(this.algo, 8);
		}

		public void Dispose()
		{
			if (this.key != null)
			{
				Array.Clear(this.key, 0, this.key.Length);
			}
		}

		public HashAlgorithm Algo
		{
			get
			{
				return this.algo;
			}
		}

		public string HashName
		{
			get
			{
				return this.hashName;
			}
			set
			{
				this.CreateHash(value);
			}
		}

		public byte[] Key
		{
			get
			{
				return this.key;
			}
			set
			{
				if (value != null && value.Length > 64)
				{
					this.key = this.algo.ComputeHash(value);
				}
				else
				{
					this.key = (byte[])value.Clone();
				}
			}
		}

		public void Initialize()
		{
			this.hash = null;
			this.block.Initialize();
			byte[] array = this.KeySetup(this.key, 54);
			this.algo.Initialize();
			this.block.Core(array);
			Array.Clear(array, 0, array.Length);
		}

		private byte[] KeySetup(byte[] key, byte padding)
		{
			byte[] array = new byte[64];
			for (int i = 0; i < key.Length; i++)
			{
				array[i] = (key[i] ^ padding);
			}
			for (int j = key.Length; j < 64; j++)
			{
				array[j] = padding;
			}
			return array;
		}

		public void Core(byte[] rgb, int ib, int cb)
		{
			this.block.Core(rgb, ib, cb);
		}

		public byte[] Final()
		{
			this.block.Final();
			byte[] array = this.algo.Hash;
			byte[] array2 = this.KeySetup(this.key, 92);
			this.algo.Initialize();
			this.algo.TransformBlock(array2, 0, array2.Length, array2, 0);
			this.algo.TransformFinalBlock(array, 0, array.Length);
			this.hash = this.algo.Hash;
			this.algo.Clear();
			Array.Clear(array2, 0, array2.Length);
			Array.Clear(array, 0, array.Length);
			return this.hash;
		}
	}
}
