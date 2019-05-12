using System;
using System.Security.Cryptography;

namespace Mono.Security.Cryptography
{
	public abstract class RC4 : SymmetricAlgorithm
	{
		private static KeySizes[] s_legalBlockSizes = new KeySizes[]
		{
			new KeySizes(64, 64, 0)
		};

		private static KeySizes[] s_legalKeySizes = new KeySizes[]
		{
			new KeySizes(40, 2048, 8)
		};

		public RC4()
		{
			this.KeySizeValue = 128;
			this.BlockSizeValue = 64;
			this.FeedbackSizeValue = this.BlockSizeValue;
			this.LegalBlockSizesValue = RC4.s_legalBlockSizes;
			this.LegalKeySizesValue = RC4.s_legalKeySizes;
		}

		public override byte[] IV
		{
			get
			{
				return new byte[0];
			}
			set
			{
			}
		}

		public new static RC4 Create()
		{
			return RC4.Create("RC4");
		}

		public new static RC4 Create(string algName)
		{
			object obj = CryptoConfig.CreateFromName(algName);
			if (obj == null)
			{
				obj = new ARC4Managed();
			}
			return (RC4)obj;
		}
	}
}
