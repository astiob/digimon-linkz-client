using System;

namespace System.Security.Cryptography
{
	internal class SHA1Internal
	{
		private const int BLOCK_SIZE_BYTES = 64;

		private const int HASH_SIZE_BYTES = 20;

		private uint[] _H;

		private ulong count;

		private byte[] _ProcessingBuffer;

		private int _ProcessingBufferCount;

		private uint[] buff;

		public SHA1Internal()
		{
			this._H = new uint[5];
			this._ProcessingBuffer = new byte[64];
			this.buff = new uint[80];
			this.Initialize();
		}

		public void HashCore(byte[] rgb, int ibStart, int cbSize)
		{
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
				this.ProcessBlock(this._ProcessingBuffer, 0u);
				this._ProcessingBufferCount = 0;
				ibStart += i;
				cbSize -= i;
			}
			for (int i = 0; i < cbSize - cbSize % 64; i += 64)
			{
				this.ProcessBlock(rgb, (uint)(ibStart + i));
			}
			if (cbSize % 64 != 0)
			{
				Buffer.BlockCopy(rgb, cbSize - cbSize % 64 + ibStart, this._ProcessingBuffer, 0, cbSize % 64);
				this._ProcessingBufferCount = cbSize % 64;
			}
		}

		public byte[] HashFinal()
		{
			byte[] array = new byte[20];
			this.ProcessFinalBlock(this._ProcessingBuffer, 0, this._ProcessingBufferCount);
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					array[i * 4 + j] = (byte)(this._H[i] >> 8 * (3 - j));
				}
			}
			return array;
		}

		public void Initialize()
		{
			this.count = 0UL;
			this._ProcessingBufferCount = 0;
			this._H[0] = 1732584193u;
			this._H[1] = 4023233417u;
			this._H[2] = 2562383102u;
			this._H[3] = 271733878u;
			this._H[4] = 3285377520u;
		}

		private void ProcessBlock(byte[] inputBuffer, uint inputOffset)
		{
			this.count += 64UL;
			uint[] h = this._H;
			uint[] array = this.buff;
			SHA1Internal.InitialiseBuff(array, inputBuffer, inputOffset);
			SHA1Internal.FillBuff(array);
			uint num = h[0];
			uint num2 = h[1];
			uint num3 = h[2];
			uint num4 = h[3];
			uint num5 = h[4];
			int i;
			for (i = 0; i < 20; i += 5)
			{
				num5 += (num << 5 | num >> 27) + (((num3 ^ num4) & num2) ^ num4) + 1518500249u + array[i];
				num2 = (num2 << 30 | num2 >> 2);
				num4 += (num5 << 5 | num5 >> 27) + (((num2 ^ num3) & num) ^ num3) + 1518500249u + array[i + 1];
				num = (num << 30 | num >> 2);
				num3 += (num4 << 5 | num4 >> 27) + (((num ^ num2) & num5) ^ num2) + 1518500249u + array[i + 2];
				num5 = (num5 << 30 | num5 >> 2);
				num2 += (num3 << 5 | num3 >> 27) + (((num5 ^ num) & num4) ^ num) + 1518500249u + array[i + 3];
				num4 = (num4 << 30 | num4 >> 2);
				num += (num2 << 5 | num2 >> 27) + (((num4 ^ num5) & num3) ^ num5) + 1518500249u + array[i + 4];
				num3 = (num3 << 30 | num3 >> 2);
			}
			while (i < 40)
			{
				num5 += (num << 5 | num >> 27) + (num2 ^ num3 ^ num4) + 1859775393u + array[i];
				num2 = (num2 << 30 | num2 >> 2);
				num4 += (num5 << 5 | num5 >> 27) + (num ^ num2 ^ num3) + 1859775393u + array[i + 1];
				num = (num << 30 | num >> 2);
				num3 += (num4 << 5 | num4 >> 27) + (num5 ^ num ^ num2) + 1859775393u + array[i + 2];
				num5 = (num5 << 30 | num5 >> 2);
				num2 += (num3 << 5 | num3 >> 27) + (num4 ^ num5 ^ num) + 1859775393u + array[i + 3];
				num4 = (num4 << 30 | num4 >> 2);
				num += (num2 << 5 | num2 >> 27) + (num3 ^ num4 ^ num5) + 1859775393u + array[i + 4];
				num3 = (num3 << 30 | num3 >> 2);
				i += 5;
			}
			while (i < 60)
			{
				num5 += (num << 5 | num >> 27) + ((num2 & num3) | (num2 & num4) | (num3 & num4)) + 2400959708u + array[i];
				num2 = (num2 << 30 | num2 >> 2);
				num4 += (num5 << 5 | num5 >> 27) + ((num & num2) | (num & num3) | (num2 & num3)) + 2400959708u + array[i + 1];
				num = (num << 30 | num >> 2);
				num3 += (num4 << 5 | num4 >> 27) + ((num5 & num) | (num5 & num2) | (num & num2)) + 2400959708u + array[i + 2];
				num5 = (num5 << 30 | num5 >> 2);
				num2 += (num3 << 5 | num3 >> 27) + ((num4 & num5) | (num4 & num) | (num5 & num)) + 2400959708u + array[i + 3];
				num4 = (num4 << 30 | num4 >> 2);
				num += (num2 << 5 | num2 >> 27) + ((num3 & num4) | (num3 & num5) | (num4 & num5)) + 2400959708u + array[i + 4];
				num3 = (num3 << 30 | num3 >> 2);
				i += 5;
			}
			while (i < 80)
			{
				num5 += (num << 5 | num >> 27) + (num2 ^ num3 ^ num4) + 3395469782u + array[i];
				num2 = (num2 << 30 | num2 >> 2);
				num4 += (num5 << 5 | num5 >> 27) + (num ^ num2 ^ num3) + 3395469782u + array[i + 1];
				num = (num << 30 | num >> 2);
				num3 += (num4 << 5 | num4 >> 27) + (num5 ^ num ^ num2) + 3395469782u + array[i + 2];
				num5 = (num5 << 30 | num5 >> 2);
				num2 += (num3 << 5 | num3 >> 27) + (num4 ^ num5 ^ num) + 3395469782u + array[i + 3];
				num4 = (num4 << 30 | num4 >> 2);
				num += (num2 << 5 | num2 >> 27) + (num3 ^ num4 ^ num5) + 3395469782u + array[i + 4];
				num3 = (num3 << 30 | num3 >> 2);
				i += 5;
			}
			h[0] += num;
			h[1] += num2;
			h[2] += num3;
			h[3] += num4;
			h[4] += num5;
		}

		private static void InitialiseBuff(uint[] buff, byte[] input, uint inputOffset)
		{
			buff[0] = (uint)((int)input[(int)((UIntPtr)inputOffset)] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 1u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 2u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 3u))]);
			buff[1] = (uint)((int)input[(int)((UIntPtr)(inputOffset + 4u))] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 5u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 6u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 7u))]);
			buff[2] = (uint)((int)input[(int)((UIntPtr)(inputOffset + 8u))] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 9u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 10u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 11u))]);
			buff[3] = (uint)((int)input[(int)((UIntPtr)(inputOffset + 12u))] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 13u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 14u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 15u))]);
			buff[4] = (uint)((int)input[(int)((UIntPtr)(inputOffset + 16u))] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 17u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 18u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 19u))]);
			buff[5] = (uint)((int)input[(int)((UIntPtr)(inputOffset + 20u))] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 21u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 22u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 23u))]);
			buff[6] = (uint)((int)input[(int)((UIntPtr)(inputOffset + 24u))] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 25u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 26u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 27u))]);
			buff[7] = (uint)((int)input[(int)((UIntPtr)(inputOffset + 28u))] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 29u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 30u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 31u))]);
			buff[8] = (uint)((int)input[(int)((UIntPtr)(inputOffset + 32u))] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 33u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 34u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 35u))]);
			buff[9] = (uint)((int)input[(int)((UIntPtr)(inputOffset + 36u))] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 37u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 38u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 39u))]);
			buff[10] = (uint)((int)input[(int)((UIntPtr)(inputOffset + 40u))] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 41u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 42u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 43u))]);
			buff[11] = (uint)((int)input[(int)((UIntPtr)(inputOffset + 44u))] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 45u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 46u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 47u))]);
			buff[12] = (uint)((int)input[(int)((UIntPtr)(inputOffset + 48u))] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 49u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 50u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 51u))]);
			buff[13] = (uint)((int)input[(int)((UIntPtr)(inputOffset + 52u))] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 53u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 54u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 55u))]);
			buff[14] = (uint)((int)input[(int)((UIntPtr)(inputOffset + 56u))] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 57u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 58u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 59u))]);
			buff[15] = (uint)((int)input[(int)((UIntPtr)(inputOffset + 60u))] << 24 | (int)input[(int)((UIntPtr)(inputOffset + 61u))] << 16 | (int)input[(int)((UIntPtr)(inputOffset + 62u))] << 8 | (int)input[(int)((UIntPtr)(inputOffset + 63u))]);
		}

		private static void FillBuff(uint[] buff)
		{
			for (int i = 16; i < 80; i += 8)
			{
				uint num = buff[i - 3] ^ buff[i - 8] ^ buff[i - 14] ^ buff[i - 16];
				buff[i] = (num << 1 | num >> 31);
				num = (buff[i - 2] ^ buff[i - 7] ^ buff[i - 13] ^ buff[i - 15]);
				buff[i + 1] = (num << 1 | num >> 31);
				num = (buff[i - 1] ^ buff[i - 6] ^ buff[i - 12] ^ buff[i - 14]);
				buff[i + 2] = (num << 1 | num >> 31);
				num = (buff[i] ^ buff[i - 5] ^ buff[i - 11] ^ buff[i - 13]);
				buff[i + 3] = (num << 1 | num >> 31);
				num = (buff[i + 1] ^ buff[i - 4] ^ buff[i - 10] ^ buff[i - 12]);
				buff[i + 4] = (num << 1 | num >> 31);
				num = (buff[i + 2] ^ buff[i - 3] ^ buff[i - 9] ^ buff[i - 11]);
				buff[i + 5] = (num << 1 | num >> 31);
				num = (buff[i + 3] ^ buff[i - 2] ^ buff[i - 8] ^ buff[i - 10]);
				buff[i + 6] = (num << 1 | num >> 31);
				num = (buff[i + 4] ^ buff[i - 1] ^ buff[i - 7] ^ buff[i - 9]);
				buff[i + 7] = (num << 1 | num >> 31);
			}
		}

		private void ProcessFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			ulong num = this.count + (ulong)((long)inputCount);
			int num2 = 56 - (int)(num % 64UL);
			if (num2 < 1)
			{
				num2 += 64;
			}
			int num3 = inputCount + num2 + 8;
			byte[] array = (num3 != 64) ? new byte[num3] : this._ProcessingBuffer;
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
			this.ProcessBlock(array, 0u);
			if (num3 == 128)
			{
				this.ProcessBlock(array, 64u);
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
