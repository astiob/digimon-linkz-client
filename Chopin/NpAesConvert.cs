using System;

namespace CROOZ.Chopin.Core
{
	public class NpAesConvert
	{
		private \uE002 \uE000;

		public NpAesConvert()
		{
			this.\uE000 = new \uE002();
		}

		internal \uE002 \uE000
		{
			get
			{
				return this.\uE000;
			}
		}

		public byte[] Encrypt(string text)
		{
			return this.\uE000.\uE006(text);
		}

		public string EncryptString(string text)
		{
			return Convert.ToBase64String(this.\uE000.\uE006(text));
		}

		public string Decrypt(byte[] data)
		{
			return this.\uE000.\uE008(data);
		}

		public string DecryptString(string data)
		{
			return this.\uE000.\uE008(Convert.FromBase64String(data));
		}
	}
}
