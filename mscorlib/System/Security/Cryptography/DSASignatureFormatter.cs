using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Creates a Digital Signature Algorithm (<see cref="T:System.Security.Cryptography.DSA" />) signature.</summary>
	[ComVisible(true)]
	public class DSASignatureFormatter : AsymmetricSignatureFormatter
	{
		private DSA dsa;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.DSASignatureFormatter" /> class.</summary>
		public DSASignatureFormatter()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.DSASignatureFormatter" /> class with the specified key.</summary>
		/// <param name="key">The instance of the Digital Signature Algorithm (<see cref="T:System.Security.Cryptography.DSA" />) that holds the key. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		public DSASignatureFormatter(AsymmetricAlgorithm key)
		{
			this.SetKey(key);
		}

		/// <summary>Creates the Digital Signature Algorithm (<see cref="T:System.Security.Cryptography.DSA" />) PKCS #1 signature for the specified data.</summary>
		/// <returns>The digital signature for the specified data.</returns>
		/// <param name="rgbHash">The data to be signed. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="rgbHash" /> is null.</exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicUnexpectedOperationException">The OID is null.-or-The DSA key is null.</exception>
		public override byte[] CreateSignature(byte[] rgbHash)
		{
			if (this.dsa == null)
			{
				throw new CryptographicUnexpectedOperationException(Locale.GetText("missing key"));
			}
			return this.dsa.CreateSignature(rgbHash);
		}

		/// <summary>Specifies the hash algorithm for the Digital Signature Algorithm (<see cref="T:System.Security.Cryptography.DSA" />) signature formatter.</summary>
		/// <param name="strName">The name of the hash algorithm to use for the signature formatter. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicUnexpectedOperationException">The <paramref name="strName" /> parameter does not map to the <see cref="T:System.Security.Cryptography.SHA1" /> hash algorithm. </exception>
		public override void SetHashAlgorithm(string strName)
		{
			if (strName == null)
			{
				throw new ArgumentNullException("strName");
			}
			try
			{
				SHA1.Create(strName);
			}
			catch (InvalidCastException)
			{
				throw new CryptographicUnexpectedOperationException(Locale.GetText("DSA requires SHA1"));
			}
		}

		/// <summary>Specifies the key to be used for the Digital Signature Algorithm (<see cref="T:System.Security.Cryptography.DSA" />) signature formatter.</summary>
		/// <param name="key">The instance of <see cref="T:System.Security.Cryptography.DSA" /> that holds the key. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is null.</exception>
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key != null)
			{
				this.dsa = (DSA)key;
				return;
			}
			throw new ArgumentNullException("key");
		}
	}
}
