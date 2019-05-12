using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Represents the abstract base class from which all implementations of asymmetric algorithms must inherit.</summary>
	[ComVisible(true)]
	public abstract class AsymmetricAlgorithm : IDisposable
	{
		/// <summary>Represents the size, in bits, of the key modulus used by the asymmetric algorithm.</summary>
		protected int KeySizeValue;

		/// <summary>Specifies the key sizes that are supported by the asymmetric algorithm.</summary>
		protected KeySizes[] LegalKeySizesValue;

		/// <summary>For a description of this member, see <see cref="M:System.IDisposable.Dispose" />.</summary>
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>When overridden in a derived class, gets the name of the key exchange algorithm.</summary>
		/// <returns>The name of the key exchange algorithm.</returns>
		public abstract string KeyExchangeAlgorithm { get; }

		/// <summary>Gets or sets the size, in bits, of the key modulus used by the asymmetric algorithm.</summary>
		/// <returns>The size, in bits, of the key modulus used by the asymmetric algorithm.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The key modulus size is invalid. </exception>
		public virtual int KeySize
		{
			get
			{
				return this.KeySizeValue;
			}
			set
			{
				if (!KeySizes.IsLegalKeySize(this.LegalKeySizesValue, value))
				{
					throw new CryptographicException(Locale.GetText("Key size not supported by algorithm."));
				}
				this.KeySizeValue = value;
			}
		}

		/// <summary>Gets the key sizes that are supported by the asymmetric algorithm.</summary>
		/// <returns>An array that contains the key sizes supported by the asymmetric algorithm.</returns>
		public virtual KeySizes[] LegalKeySizes
		{
			get
			{
				return this.LegalKeySizesValue;
			}
		}

		/// <summary>Gets the name of the signature algorithm.</summary>
		/// <returns>The name of the signature algorithm.</returns>
		public abstract string SignatureAlgorithm { get; }

		/// <summary>Releases all resources used by the <see cref="T:System.Security.Cryptography.AsymmetricAlgorithm" /> class.</summary>
		public void Clear()
		{
			this.Dispose(false);
		}

		/// <summary>When overridden in a derived class, releases the unmanaged resources used by the <see cref="T:System.Security.Cryptography.AsymmetricAlgorithm" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected abstract void Dispose(bool disposing);

		/// <summary>When overridden in a derived class, reconstructs an <see cref="T:System.Security.Cryptography.AsymmetricAlgorithm" /> object from an XML string.</summary>
		/// <param name="xmlString">The XML string to use to reconstruct the <see cref="T:System.Security.Cryptography.AsymmetricAlgorithm" /> object. </param>
		public abstract void FromXmlString(string xmlString);

		/// <summary>When overridden in a derived class, creates and returns an XML string representation of the current <see cref="T:System.Security.Cryptography.AsymmetricAlgorithm" /> object.</summary>
		/// <returns>An XML string encoding of the current <see cref="T:System.Security.Cryptography.AsymmetricAlgorithm" /> object.</returns>
		/// <param name="includePrivateParameters">true to include private parameters; otherwise, false. </param>
		public abstract string ToXmlString(bool includePrivateParameters);

		/// <summary>Creates an instance of the default implementation of an asymmetric algorithm.</summary>
		/// <returns>A new <see cref="T:System.Security.Cryptography.RSACryptoServiceProvider" /> instance, unless the default settings have been changed with the &lt;cryptoClass&gt; element.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static AsymmetricAlgorithm Create()
		{
			return AsymmetricAlgorithm.Create("System.Security.Cryptography.AsymmetricAlgorithm");
		}

		/// <summary>Creates an instance of the specified implementation of an asymmetric algorithm.</summary>
		/// <returns>A new instance of the specified asymmetric algorithm implementation.</returns>
		/// <param name="algName">The asymmetric algorithm implementation to use. The following table shows the valid values for the <paramref name="algName" /> parameter and the algorithms they map to.Parameter valueImplements System.Security.Cryptography.AsymmetricAlgorithm<see cref="T:System.Security.Cryptography.AsymmetricAlgorithm" />RSA<see cref="T:System.Security.Cryptography.RSA" />System.Security.Cryptography.RSA<see cref="T:System.Security.Cryptography.RSA" />DSA<see cref="T:System.Security.Cryptography.DSA" />System.Security.Cryptography.DSA<see cref="T:System.Security.Cryptography.DSA" />ECDsa<see cref="T:System.Security.Cryptography.ECDsa" />ECDsaCng<see cref="T:System.Security.Cryptography.ECDsaCng" />System.Security.Cryptography.ECDsaCng<see cref="T:System.Security.Cryptography.ECDsaCng" />ECDH<see cref="T:System.Security.Cryptography.ECDiffieHellman" />ECDiffieHellman<see cref="T:System.Security.Cryptography.ECDiffieHellman" />ECDiffieHellmanCng<see cref="T:System.Security.Cryptography.ECDiffieHellmanCng" />System.Security.Cryptography.ECDiffieHellmanCng<see cref="T:System.Security.Cryptography.ECDiffieHellmanCng" /></param>
		public static AsymmetricAlgorithm Create(string algName)
		{
			return (AsymmetricAlgorithm)CryptoConfig.CreateFromName(algName);
		}

		internal static byte[] GetNamedParam(string xml, string param)
		{
			string text = "<" + param + ">";
			int num = xml.IndexOf(text);
			if (num == -1)
			{
				return null;
			}
			string value = "</" + param + ">";
			int num2 = xml.IndexOf(value);
			if (num2 == -1 || num2 <= num)
			{
				return null;
			}
			num += text.Length;
			string s = xml.Substring(num, num2 - num);
			return Convert.FromBase64String(s);
		}
	}
}
