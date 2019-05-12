using Mono.Security.Cryptography;
using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Computes masks according to PKCS #1 for use by key exchange algorithms.</summary>
	[ComVisible(true)]
	public class PKCS1MaskGenerationMethod : MaskGenerationMethod
	{
		private string hashName;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.PKCS1MaskGenerationMethod" /> class.</summary>
		public PKCS1MaskGenerationMethod()
		{
			this.hashName = "SHA1";
		}

		/// <summary>Gets or sets the name of the hash algorithm type to use for generating the mask.</summary>
		/// <returns>The name of the type that implements the hash algorithm to use for computing the mask.</returns>
		public string HashName
		{
			get
			{
				return this.hashName;
			}
			set
			{
				this.hashName = ((value != null) ? value : "SHA1");
			}
		}

		/// <summary>Generates and returns a mask from the specified random seed of the specified length.</summary>
		/// <returns>A randomly generated mask whose length is equal to the <paramref name="cbReturn" /> parameter.</returns>
		/// <param name="rgbSeed">The random seed to use for computing the mask. </param>
		/// <param name="cbReturn">The length of the generated mask in bytes. </param>
		public override byte[] GenerateMask(byte[] rgbSeed, int cbReturn)
		{
			HashAlgorithm hash = HashAlgorithm.Create(this.hashName);
			return PKCS1.MGF1(hash, rgbSeed, cbReturn);
		}
	}
}
