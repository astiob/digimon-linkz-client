using System;
using System.IO;
using System.Security.Cryptography;

namespace PersistentData
{
	public sealed class CryptoHelper
	{
		public const int BLOCK_SIZE = 8;

		private byte[] \uE000 = new byte[]
		{
			38,
			15,
			34,
			201,
			85,
			108,
			209,
			42
		};

		private byte[] \uE001 = new byte[]
		{
			246,
			34,
			216,
			121,
			8,
			35,
			78,
			88
		};

		public CryptoHelper(byte[] key, byte[] iv)
		{
			if (key != null)
			{
				this.\uE000 = key;
			}
			if (iv != null)
			{
				this.\uE001 = iv;
			}
		}

		public byte[] EncryptWithDES(byte[] data)
		{
			byte[] result;
			using (DESCryptoServiceProvider descryptoServiceProvider = new DESCryptoServiceProvider())
			{
				result = this.\uE000(descryptoServiceProvider, data, this.\uE000, this.\uE001);
			}
			return result;
		}

		public byte[] DecryptWithDES(byte[] data)
		{
			byte[] result;
			using (DESCryptoServiceProvider descryptoServiceProvider = new DESCryptoServiceProvider())
			{
				result = this.\uE001(descryptoServiceProvider, data, this.\uE000, this.\uE001);
			}
			return result;
		}

		private byte[] \uE000(SymmetricAlgorithm \uE000, byte[] \uE001, byte[] \uE002, byte[] \uE003)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				ICryptoTransform transform = \uE000.CreateEncryptor(\uE002, \uE003);
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
				{
					cryptoStream.Write(\uE001, 0, \uE001.Length);
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		private byte[] \uE001(SymmetricAlgorithm \uE000, byte[] \uE001, byte[] \uE002, byte[] \uE003)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				ICryptoTransform transform = \uE000.CreateDecryptor(\uE002, \uE003);
				using (MemoryStream memoryStream2 = new MemoryStream(\uE001))
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream2, transform, CryptoStreamMode.Read))
					{
						byte[] array = new byte[1024];
						for (int i = cryptoStream.Read(array, 0, array.Length); i > 0; i = cryptoStream.Read(array, 0, array.Length))
						{
							memoryStream.Write(array, 0, i);
						}
					}
				}
				result = memoryStream.ToArray();
			}
			return result;
		}
	}
}
