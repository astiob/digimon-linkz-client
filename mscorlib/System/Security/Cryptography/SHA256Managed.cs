using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Computes the <see cref="T:System.Security.Cryptography.SHA256" /> hash for the input data using the managed library. </summary>
	[ComVisible(true)]
	public class SHA256Managed : SHA256
	{
		private const int BLOCK_SIZE_BYTES = 64;

		private const int HASH_SIZE_BYTES = 32;

		private uint[] _H;

		private ulong count;

		private byte[] _ProcessingBuffer;

		private int _ProcessingBufferCount;

		private uint[] buff;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.SHA256Managed" /> class using the managed library.</summary>
		/// <exception cref="T:System.InvalidOperationException">The Federal Information Processing Standards (FIPS) security setting is enabled. This implementation is not part of the Windows Platform FIPS-validated cryptographic algorithms.</exception>
		public SHA256Managed()
		{
			this._H = new uint[8];
			this._ProcessingBuffer = new byte[64];
			this.buff = new uint[64];
			this.Initialize();
		}

		/// <summary>When overridden in a derived class, routes data written to the object into the <see cref="T:System.Security.Cryptography.SHA256" /> hash algorithm for computing the hash.</summary>
		/// <param name="rgb">The input data. </param>
		/// <param name="ibStart">The offset into the byte array from which to begin using data. </param>
		/// <param name="cbSize">The number of bytes in the array to use as data. </param>
		protected override void HashCore(byte[] rgb, int ibStart, int cbSize)
		{
			this.State = 1;
			if (this._ProcessingBufferCount != 0)
			{
				if (cbSize < 64 - this._ProcessingBufferCount)
				{
					Buffer.BlockCopy(rgb, ibStart, this._ProcessingBuffer, this._ProcessingBufferCount, cbSize);
					this._ProcessingBufferCount += cbSize;
					return;
				}
				int i = 64 - this._ProcessingBufferCount;
				Buffer.BlockCopy(rgb, ibStart, this._ProcessingBuffer, this._ProcessingBufferCount, i);
				this.ProcessBlock(this._ProcessingBuffer, 0);
				this._ProcessingBufferCount = 0;
				ibStart += i;
				cbSize -= i;
			}
			for (int i = 0; i < cbSize - cbSize % 64; i += 64)
			{
				this.ProcessBlock(rgb, ibStart + i);
			}
			if (cbSize % 64 != 0)
			{
				Buffer.BlockCopy(rgb, cbSize - cbSize % 64 + ibStart, this._ProcessingBuffer, 0, cbSize % 64);
				this._ProcessingBufferCount = cbSize % 64;
			}
		}

		/// <summary>When overridden in a derived class, finalizes the hash computation after the last data is processed by the cryptographic stream object.</summary>
		/// <returns>The computed hash code.</returns>
		protected override byte[] HashFinal()
		{
			byte[] array = new byte[32];
			this.ProcessFinalBlock(this._ProcessingBuffer, 0, this._ProcessingBufferCount);
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					array[i * 4 + j] = (byte)(this._H[i] >> 24 - j * 8);
				}
			}
			this.State = 0;
			return array;
		}

		/// <summary>Initializes an instance of <see cref="T:System.Security.Cryptography.SHA256Managed" />.</summary>
		public override void Initialize()
		{
			this.count = 0UL;
			this._ProcessingBufferCount = 0;
			this._H[0] = 1779033703u;
			this._H[1] = 3144134277u;
			this._H[2] = 1013904242u;
			this._H[3] = 2773480762u;
			this._H[4] = 1359893119u;
			this._H[5] = 2600822924u;
			this._H[6] = 528734635u;
			this._H[7] = 1541459225u;
		}

		private void ProcessBlock(byte[] inputBuffer, int inputOffset)
		{
			uint[] k = SHAConstants.K1;
			uint[] array = this.buff;
			this.count += 64UL;
			for (int i = 0; i < 16; i++)
			{
				array[i] = (uint)((int)inputBuffer[inputOffset + 4 * i] << 24 | (int)inputBuffer[inputOffset + 4 * i + 1] << 16 | (int)inputBuffer[inputOffset + 4 * i + 2] << 8 | (int)inputBuffer[inputOffset + 4 * i + 3]);
			}
			for (int i = 16; i < 64; i++)
			{
				uint num = array[i - 15];
				num = ((num >> 7 | num << 25) ^ (num >> 18 | num << 14) ^ num >> 3);
				uint num2 = array[i - 2];
				num2 = ((num2 >> 17 | num2 << 15) ^ (num2 >> 19 | num2 << 13) ^ num2 >> 10);
				array[i] = num2 + array[i - 7] + num + array[i - 16];
			}
			uint num3 = this._H[0];
			uint num4 = this._H[1];
			uint num5 = this._H[2];
			uint num6 = this._H[3];
			uint num7 = this._H[4];
			uint num8 = this._H[5];
			uint num9 = this._H[6];
			uint num10 = this._H[7];
			for (int i = 0; i < 64; i++)
			{
				uint num = num10 + ((num7 >> 6 | num7 << 26) ^ (num7 >> 11 | num7 << 21) ^ (num7 >> 25 | num7 << 7)) + ((num7 & num8) ^ (~num7 & num9)) + k[i] + array[i];
				uint num2 = (num3 >> 2 | num3 << 30) ^ (num3 >> 13 | num3 << 19) ^ (num3 >> 22 | num3 << 10);
				num2 += ((num3 & num4) ^ (num3 & num5) ^ (num4 & num5));
				num10 = num9;
				num9 = num8;
				num8 = num7;
				num7 = num6 + num;
				num6 = num5;
				num5 = num4;
				num4 = num3;
				num3 = num + num2;
			}
			this._H[0] += num3;
			this._H[1] += num4;
			this._H[2] += num5;
			this._H[3] += num6;
			this._H[4] += num7;
			this._H[5] += num8;
			this._H[6] += num9;
			this._H[7] += num10;
		}

		private void ProcessFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			ulong num = this.count + (ulong)((long)inputCount);
			int num2 = 56 - (int)(num % 64UL);
			if (num2 < 1)
			{
				num2 += 64;
			}
			byte[] array = new byte[inputCount + num2 + 8];
			for (int i = 0; i < inputCount; i++)
			{
				array[i] = inputBuffer[i + inputOffset];
			}
			array[inputCount] = 128;
			for (int j = inputCount + 1; j < inputCount + num2; j++)
			{
				array[j] = 0;
			}
			ulong length = num << 3;
			this.AddLength(length, array, inputCount + num2);
			this.ProcessBlock(array, 0);
			if (inputCount + num2 + 8 == 128)
			{
				this.ProcessBlock(array, 64);
			}
		}

		internal void AddLength(ulong length, byte[] buffer, int position)
		{
			buffer[position++] = (byte)(length >> 56);
			buffer[position++] = (byte)(length >> 48);
			buffer[position++] = (byte)(length >> 40);
			buffer[position++] = (byte)(length >> 32);
			buffer[position++] = (byte)(length >> 24);
			buffer[position++] = (byte)(length >> 16);
			buffer[position++] = (byte)(length >> 8);
			buffer[position] = (byte)length;
		}
	}
}
