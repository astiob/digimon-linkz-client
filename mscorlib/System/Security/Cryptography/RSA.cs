using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Cryptography
{
	/// <summary>Represents the base class from which all implementations of the <see cref="T:System.Security.Cryptography.RSA" /> algorithm inherit.</summary>
	[ComVisible(true)]
	public abstract class RSA : AsymmetricAlgorithm
	{
		/// <summary>Creates an instance of the default implementation of the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.</summary>
		/// <returns>A new instance of the default implementation of <see cref="T:System.Security.Cryptography.RSA" />.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public new static RSA Create()
		{
			return RSA.Create("System.Security.Cryptography.RSA");
		}

		/// <summary>Creates an instance of the specified implementation of <see cref="T:System.Security.Cryptography.RSA" />.</summary>
		/// <returns>A new instance of the specified implementation of <see cref="T:System.Security.Cryptography.RSA" />.</returns>
		/// <param name="algName">The name of the implementation of <see cref="T:System.Security.Cryptography.RSA" /> to use. </param>
		public new static RSA Create(string algName)
		{
			return (RSA)CryptoConfig.CreateFromName(algName);
		}

		/// <summary>When overridden in a derived class, encrypts the input data using the public key.</summary>
		/// <returns>The resulting encryption of the <paramref name="rgb" /> parameter as cipher text.</returns>
		/// <param name="rgb">The plain text to be encrypted. </param>
		public abstract byte[] EncryptValue(byte[] rgb);

		/// <summary>When overridden in a derived class, decrypts the input data using the private key.</summary>
		/// <returns>The resulting decryption of the <paramref name="rgb" /> parameter in plain text.</returns>
		/// <param name="rgb">The cipher text to be decrypted. </param>
		public abstract byte[] DecryptValue(byte[] rgb);

		/// <summary>When overridden in a derived class, exports the <see cref="T:System.Security.Cryptography.RSAParameters" />.</summary>
		/// <returns>The parameters for <see cref="T:System.Security.Cryptography.DSA" />.</returns>
		/// <param name="includePrivateParameters">true to include private parameters; otherwise, false. </param>
		public abstract RSAParameters ExportParameters(bool includePrivateParameters);

		/// <summary>When overridden in a derived class, imports the specified <see cref="T:System.Security.Cryptography.RSAParameters" />.</summary>
		/// <param name="parameters">The parameters for <see cref="T:System.Security.Cryptography.RSA" />. </param>
		public abstract void ImportParameters(RSAParameters parameters);

		internal void ZeroizePrivateKey(RSAParameters parameters)
		{
			if (parameters.P != null)
			{
				Array.Clear(parameters.P, 0, parameters.P.Length);
			}
			if (parameters.Q != null)
			{
				Array.Clear(parameters.Q, 0, parameters.Q.Length);
			}
			if (parameters.DP != null)
			{
				Array.Clear(parameters.DP, 0, parameters.DP.Length);
			}
			if (parameters.DQ != null)
			{
				Array.Clear(parameters.DQ, 0, parameters.DQ.Length);
			}
			if (parameters.InverseQ != null)
			{
				Array.Clear(parameters.InverseQ, 0, parameters.InverseQ.Length);
			}
			if (parameters.D != null)
			{
				Array.Clear(parameters.D, 0, parameters.D.Length);
			}
		}

		/// <summary>Initializes an <see cref="T:System.Security.Cryptography.RSA" /> object from the key information from an XML string.</summary>
		/// <param name="xmlString">The XML string containing <see cref="T:System.Security.Cryptography.RSA" /> key information. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="xmlString" /> parameter is null. </exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The format of the <paramref name="xmlString" /> parameter is not valid. </exception>
		public override void FromXmlString(string xmlString)
		{
			if (xmlString == null)
			{
				throw new ArgumentNullException("xmlString");
			}
			RSAParameters parameters = default(RSAParameters);
			try
			{
				parameters.P = AsymmetricAlgorithm.GetNamedParam(xmlString, "P");
				parameters.Q = AsymmetricAlgorithm.GetNamedParam(xmlString, "Q");
				parameters.D = AsymmetricAlgorithm.GetNamedParam(xmlString, "D");
				parameters.DP = AsymmetricAlgorithm.GetNamedParam(xmlString, "DP");
				parameters.DQ = AsymmetricAlgorithm.GetNamedParam(xmlString, "DQ");
				parameters.InverseQ = AsymmetricAlgorithm.GetNamedParam(xmlString, "InverseQ");
				parameters.Exponent = AsymmetricAlgorithm.GetNamedParam(xmlString, "Exponent");
				parameters.Modulus = AsymmetricAlgorithm.GetNamedParam(xmlString, "Modulus");
				this.ImportParameters(parameters);
			}
			catch (Exception inner)
			{
				this.ZeroizePrivateKey(parameters);
				throw new CryptographicException(Locale.GetText("Couldn't decode XML"), inner);
			}
			finally
			{
				this.ZeroizePrivateKey(parameters);
			}
		}

		/// <summary>Creates and returns an XML string containing the key of the current <see cref="T:System.Security.Cryptography.RSA" /> object.</summary>
		/// <returns>An XML string containing the key of the current <see cref="T:System.Security.Cryptography.RSA" /> object.</returns>
		/// <param name="includePrivateParameters">true to include a public and private RSA key; false to include only the public key. </param>
		public override string ToXmlString(bool includePrivateParameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			RSAParameters parameters = this.ExportParameters(includePrivateParameters);
			try
			{
				stringBuilder.Append("<RSAKeyValue>");
				stringBuilder.Append("<Modulus>");
				stringBuilder.Append(Convert.ToBase64String(parameters.Modulus));
				stringBuilder.Append("</Modulus>");
				stringBuilder.Append("<Exponent>");
				stringBuilder.Append(Convert.ToBase64String(parameters.Exponent));
				stringBuilder.Append("</Exponent>");
				if (includePrivateParameters)
				{
					if (parameters.D == null)
					{
						string text = Locale.GetText("Missing D parameter for the private key.");
						throw new ArgumentNullException(text);
					}
					if (parameters.P == null || parameters.Q == null || parameters.DP == null || parameters.DQ == null || parameters.InverseQ == null)
					{
						string text2 = Locale.GetText("Missing some CRT parameters for the private key.");
						throw new CryptographicException(text2);
					}
					stringBuilder.Append("<P>");
					stringBuilder.Append(Convert.ToBase64String(parameters.P));
					stringBuilder.Append("</P>");
					stringBuilder.Append("<Q>");
					stringBuilder.Append(Convert.ToBase64String(parameters.Q));
					stringBuilder.Append("</Q>");
					stringBuilder.Append("<DP>");
					stringBuilder.Append(Convert.ToBase64String(parameters.DP));
					stringBuilder.Append("</DP>");
					stringBuilder.Append("<DQ>");
					stringBuilder.Append(Convert.ToBase64String(parameters.DQ));
					stringBuilder.Append("</DQ>");
					stringBuilder.Append("<InverseQ>");
					stringBuilder.Append(Convert.ToBase64String(parameters.InverseQ));
					stringBuilder.Append("</InverseQ>");
					stringBuilder.Append("<D>");
					stringBuilder.Append(Convert.ToBase64String(parameters.D));
					stringBuilder.Append("</D>");
				}
				stringBuilder.Append("</RSAKeyValue>");
			}
			catch
			{
				this.ZeroizePrivateKey(parameters);
				throw;
			}
			return stringBuilder.ToString();
		}
	}
}
