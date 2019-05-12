using System;
using System.Security.Cryptography;

namespace Mono.Security.Cryptography
{
	internal class BlockProcessor
	{
		private ICryptoTransform transform;

		private byte[] block;

		private int blockSize;

		private int blockCount;

		public BlockProcessor(ICryptoTransform transform) : this(transform, transform.InputBlockSize)
		{
		}

		public BlockProcessor(ICryptoTransform transform, int blockSize)
		{
			this.transform = transform;
			this.blockSize = blockSize;
			this.block = new byte[blockSize];
		}

		~BlockProcessor()
		{
			Array.Clear(this.block, 0, this.blockSize);
		}

		public void Initialize()
		{
			Array.Clear(this.block, 0, this.blockSize);
			this.blockCount = 0;
		}

		public void Core(byte[] rgb)
		{
			this.Core(rgb, 0, rgb.Length);
		}

		public void Core(byte[] rgb, int ib, int cb)
		{
			int num = Math.Min(this.blockSize - this.blockCount, cb);
			Buffer.BlockCopy(rgb, ib, this.block, this.blockCount, num);
			this.blockCount += num;
			if (this.blockCount == this.blockSize)
			{
				this.transform.TransformBlock(this.block, 0, this.blockSize, this.block, 0);
				int num2 = (cb - num) / this.blockSize;
				for (int i = 0; i < num2; i++)
				{
					this.transform.TransformBlock(rgb, num + ib, this.blockSize, this.block, 0);
					num += this.blockSize;
				}
				this.blockCount = cb - num;
				if (this.blockCount > 0)
				{
					Buffer.BlockCopy(rgb, num + ib, this.block, 0, this.blockCount);
				}
			}
		}

		public byte[] Final()
		{
			return this.transform.TransformFinalBlock(this.block, 0, this.blockCount);
		}
	}
}
