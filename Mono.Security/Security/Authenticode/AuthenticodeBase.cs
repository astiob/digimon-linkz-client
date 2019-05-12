using System;
using System.IO;
using System.Security.Cryptography;

namespace Mono.Security.Authenticode
{
	public class AuthenticodeBase
	{
		public const string spcIndirectDataContext = "1.3.6.1.4.1.311.2.1.4";

		private byte[] fileblock;

		private FileStream fs;

		private int blockNo;

		private int blockLength;

		private int peOffset;

		private int dirSecurityOffset;

		private int dirSecuritySize;

		private int coffSymbolTableOffset;

		public AuthenticodeBase()
		{
			this.fileblock = new byte[4096];
		}

		internal int PEOffset
		{
			get
			{
				if (this.blockNo < 1)
				{
					this.ReadFirstBlock();
				}
				return this.peOffset;
			}
		}

		internal int CoffSymbolTableOffset
		{
			get
			{
				if (this.blockNo < 1)
				{
					this.ReadFirstBlock();
				}
				return this.coffSymbolTableOffset;
			}
		}

		internal int SecurityOffset
		{
			get
			{
				if (this.blockNo < 1)
				{
					this.ReadFirstBlock();
				}
				return this.dirSecurityOffset;
			}
		}

		internal void Open(string filename)
		{
			if (this.fs != null)
			{
				this.Close();
			}
			this.fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
			this.blockNo = 0;
		}

		internal void Close()
		{
			if (this.fs != null)
			{
				this.fs.Close();
				this.fs = null;
			}
		}

		internal void ReadFirstBlock()
		{
			int num = this.ProcessFirstBlock();
			if (num != 0)
			{
				string text = Locale.GetText("Cannot sign non PE files, e.g. .CAB or .MSI files (error {0}).", new object[]
				{
					num
				});
				throw new NotSupportedException(text);
			}
		}

		internal int ProcessFirstBlock()
		{
			if (this.fs == null)
			{
				return 1;
			}
			this.fs.Position = 0L;
			this.blockLength = this.fs.Read(this.fileblock, 0, this.fileblock.Length);
			this.blockNo = 1;
			if (this.blockLength < 64)
			{
				return 2;
			}
			if (BitConverterLE.ToUInt16(this.fileblock, 0) != 23117)
			{
				return 3;
			}
			this.peOffset = BitConverterLE.ToInt32(this.fileblock, 60);
			if (this.peOffset > this.fileblock.Length)
			{
				string message = string.Format(Locale.GetText("Header size too big (> {0} bytes)."), this.fileblock.Length);
				throw new NotSupportedException(message);
			}
			if ((long)this.peOffset > this.fs.Length)
			{
				return 4;
			}
			if (BitConverterLE.ToUInt32(this.fileblock, this.peOffset) != 17744u)
			{
				return 5;
			}
			this.dirSecurityOffset = BitConverterLE.ToInt32(this.fileblock, this.peOffset + 152);
			this.dirSecuritySize = BitConverterLE.ToInt32(this.fileblock, this.peOffset + 156);
			this.coffSymbolTableOffset = BitConverterLE.ToInt32(this.fileblock, this.peOffset + 12);
			return 0;
		}

		internal byte[] GetSecurityEntry()
		{
			if (this.blockNo < 1)
			{
				this.ReadFirstBlock();
			}
			if (this.dirSecuritySize > 8)
			{
				byte[] array = new byte[this.dirSecuritySize - 8];
				this.fs.Position = (long)(this.dirSecurityOffset + 8);
				this.fs.Read(array, 0, array.Length);
				return array;
			}
			return null;
		}

		internal byte[] GetHash(HashAlgorithm hash)
		{
			if (this.blockNo < 1)
			{
				this.ReadFirstBlock();
			}
			this.fs.Position = (long)this.blockLength;
			int num = 0;
			long num2;
			if (this.dirSecurityOffset > 0)
			{
				if (this.dirSecurityOffset < this.blockLength)
				{
					this.blockLength = this.dirSecurityOffset;
					num2 = 0L;
				}
				else
				{
					num2 = (long)(this.dirSecurityOffset - this.blockLength);
				}
			}
			else if (this.coffSymbolTableOffset > 0)
			{
				this.fileblock[this.PEOffset + 12] = 0;
				this.fileblock[this.PEOffset + 13] = 0;
				this.fileblock[this.PEOffset + 14] = 0;
				this.fileblock[this.PEOffset + 15] = 0;
				this.fileblock[this.PEOffset + 16] = 0;
				this.fileblock[this.PEOffset + 17] = 0;
				this.fileblock[this.PEOffset + 18] = 0;
				this.fileblock[this.PEOffset + 19] = 0;
				if (this.coffSymbolTableOffset < this.blockLength)
				{
					this.blockLength = this.coffSymbolTableOffset;
					num2 = 0L;
				}
				else
				{
					num2 = (long)(this.coffSymbolTableOffset - this.blockLength);
				}
			}
			else
			{
				num = (int)(this.fs.Length & 7L);
				if (num > 0)
				{
					num = 8 - num;
				}
				num2 = this.fs.Length - (long)this.blockLength;
			}
			int num3 = this.peOffset + 88;
			hash.TransformBlock(this.fileblock, 0, num3, this.fileblock, 0);
			num3 += 4;
			hash.TransformBlock(this.fileblock, num3, 60, this.fileblock, num3);
			num3 += 68;
			if (num2 == 0L)
			{
				hash.TransformFinalBlock(this.fileblock, num3, this.blockLength - num3);
			}
			else
			{
				hash.TransformBlock(this.fileblock, num3, this.blockLength - num3, this.fileblock, num3);
				long num4 = num2 >> 12;
				int num5 = (int)(num2 - (num4 << 12));
				if (num5 == 0)
				{
					num4 -= 1L;
					num5 = 4096;
				}
				for (;;)
				{
					long num6 = num4;
					num4 = num6 - 1L;
					if (num6 <= 0L)
					{
						break;
					}
					this.fs.Read(this.fileblock, 0, this.fileblock.Length);
					hash.TransformBlock(this.fileblock, 0, this.fileblock.Length, this.fileblock, 0);
				}
				if (this.fs.Read(this.fileblock, 0, num5) != num5)
				{
					return null;
				}
				if (num > 0)
				{
					hash.TransformBlock(this.fileblock, 0, num5, this.fileblock, 0);
					hash.TransformFinalBlock(new byte[num], 0, num);
				}
				else
				{
					hash.TransformFinalBlock(this.fileblock, 0, num5);
				}
			}
			return hash.Hash;
		}

		protected byte[] HashFile(string fileName, string hashName)
		{
			byte[] result;
			try
			{
				this.Open(fileName);
				HashAlgorithm hash = HashAlgorithm.Create(hashName);
				byte[] hash2 = this.GetHash(hash);
				this.Close();
				result = hash2;
			}
			catch
			{
				result = null;
			}
			return result;
		}
	}
}
