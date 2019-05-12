using CROOZ.Chopin.Core;
using System;

namespace Neptune.Common
{
	public class NpAes
	{
		private NpAesConvert aes;

		public NpAes()
		{
			this.aes = new NpAesConvert();
		}

		public NpAesConvert instance
		{
			get
			{
				return this.aes;
			}
		}

		public byte[] Encrypt(string text)
		{
			return this.aes.Encrypt(text);
		}

		public string EncryptString(string text)
		{
			return this.aes.EncryptString(text);
		}

		public string Decrypt(byte[] data)
		{
			return this.aes.Decrypt(data);
		}

		public string DecryptString(string data)
		{
			return this.aes.DecryptString(data);
		}
	}
}
