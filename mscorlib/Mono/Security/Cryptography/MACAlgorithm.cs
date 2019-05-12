using System;
using System.Security.Cryptography;

namespace Mono.Security.Cryptography
{
	internal class MACAlgorithm
	{
		private SymmetricAlgorithm algo;

		private ICryptoTransform enc;

		private byte[] block;

		private int blockSize;

		private int blockCount;

		public MACAlgorithm(SymmetricAlgorithm algorithm)
		{
			this.algo = algorithm;
			this.algo.Mode = CipherMode.CBC;
			this.blockSize = this.algo.BlockSize >> 3;
			this.algo.IV = new byte[this.blockSize];
			this.block = new byte[this.blockSize];
		}

		public void Initialize(byte[] key)
		{
			this.algo.Key = key;
			if (this.enc == null)
			{
				this.enc = this.algo.CreateEncryptor();
			}
			Array.Clear(this.block, 0, this.blockSize);
			this.blockCount = 0;
		}

		public void Core(byte[] rgb, int ib, int cb)
		{
			int num = Math.Min(this.blockSize - this.blockCount, cb);
			Array.Copy(rgb, ib, this.block, this.blockCount, num);
			this.blockCount += num;
			if (this.blockCount == this.blockSize)
			{
				this.enc.TransformBlock(this.block, 0, this.blockSize, this.block, 0);
				int num2 = (cb - num) / this.blockSize;
				for (int i = 0; i < num2; i++)
				{
					this.enc.TransformBlock(rgb, num, this.blockSize, this.block, 0);
					num += this.blockSize;
				}
				this.blockCount = cb - num;
				if (this.blockCount > 0)
				{
					Array.Copy(rgb, num, this.block, 0, this.blockCount);
				}
			}
		}

		public byte[] Final()
		{
			byte[] result;
			if (this.blockCount > 0 || (this.algo.Padding != PaddingMode.Zeros && this.algo.Padding != PaddingMode.None))
			{
				result = this.enc.TransformFinalBlock(this.block, 0, this.blockCount);
			}
			else
			{
				result = (byte[])this.block.Clone();
			}
			if (!this.enc.CanReuseTransform)
			{
				this.enc.Dispose();
				this.enc = null;
			}
			return result;
		}
	}
}
