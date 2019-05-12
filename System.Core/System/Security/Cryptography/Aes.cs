using System;

namespace System.Security.Cryptography
{
	/// <summary>Represents the abstract base class from which all implementations of the Advanced Encryption Standard (AES) must inherit. </summary>
	public abstract class Aes : SymmetricAlgorithm
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Aes" /> class. </summary>
		protected Aes()
		{
			this.KeySizeValue = 256;
			this.BlockSizeValue = 128;
			this.LegalKeySizesValue = new KeySizes[1];
			this.LegalKeySizesValue[0] = new KeySizes(128, 256, 64);
			this.LegalBlockSizesValue = new KeySizes[1];
			this.LegalBlockSizesValue[0] = new KeySizes(128, 128, 0);
		}

		/// <summary>Creates a cryptographic object that is used to perform the symmetric algorithm.</summary>
		/// <returns>A cryptographic object that is used to perform the symmetric algorithm.</returns>
		public new static Aes Create()
		{
			return Aes.Create("System.Security.Cryptography.AesManaged, System.Core, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
		}

		/// <summary>Creates a cryptographic object that specifies the implementation of AES to use to perform the symmetric algorithm.</summary>
		/// <returns>A cryptographic object that is used to perform the symmetric algorithm.</returns>
		/// <param name="algorithmName">The name of the specific implementation of AES to use.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="algorithmName" /> parameter is null.</exception>
		public new static Aes Create(string algName)
		{
			return (Aes)CryptoConfig.CreateFromName(algName);
		}
	}
}
