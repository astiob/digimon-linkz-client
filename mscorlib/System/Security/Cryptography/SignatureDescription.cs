using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Contains information about the properties of a digital signature.</summary>
	[ComVisible(true)]
	public class SignatureDescription
	{
		private string _DeformatterAlgorithm;

		private string _DigestAlgorithm;

		private string _FormatterAlgorithm;

		private string _KeyAlgorithm;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.SignatureDescription" /> class.</summary>
		public SignatureDescription()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.SignatureDescription" /> class from the specified <see cref="T:System.Security.SecurityElement" />.</summary>
		/// <param name="el">The <see cref="T:System.Security.SecurityElement" /> from which to get the algorithms for the signature description. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="el" /> parameter is null.</exception>
		public SignatureDescription(SecurityElement el)
		{
			if (el == null)
			{
				throw new ArgumentNullException("el");
			}
			SecurityElement securityElement = el.SearchForChildByTag("Deformatter");
			this._DeformatterAlgorithm = ((securityElement != null) ? securityElement.Text : null);
			securityElement = el.SearchForChildByTag("Digest");
			this._DigestAlgorithm = ((securityElement != null) ? securityElement.Text : null);
			securityElement = el.SearchForChildByTag("Formatter");
			this._FormatterAlgorithm = ((securityElement != null) ? securityElement.Text : null);
			securityElement = el.SearchForChildByTag("Key");
			this._KeyAlgorithm = ((securityElement != null) ? securityElement.Text : null);
		}

		/// <summary>Gets or sets the deformatter algorithm for the signature description.</summary>
		/// <returns>The deformatter algorithm for the signature description.</returns>
		public string DeformatterAlgorithm
		{
			get
			{
				return this._DeformatterAlgorithm;
			}
			set
			{
				this._DeformatterAlgorithm = value;
			}
		}

		/// <summary>Gets or sets the digest algorithm for the signature description.</summary>
		/// <returns>The digest algorithm for the signature description.</returns>
		public string DigestAlgorithm
		{
			get
			{
				return this._DigestAlgorithm;
			}
			set
			{
				this._DigestAlgorithm = value;
			}
		}

		/// <summary>Gets or sets the formatter algorithm for the signature description.</summary>
		/// <returns>The formatter algorithm for the signature description.</returns>
		public string FormatterAlgorithm
		{
			get
			{
				return this._FormatterAlgorithm;
			}
			set
			{
				this._FormatterAlgorithm = value;
			}
		}

		/// <summary>Gets or sets the key algorithm for the signature description.</summary>
		/// <returns>The key algorithm for the signature description.</returns>
		public string KeyAlgorithm
		{
			get
			{
				return this._KeyAlgorithm;
			}
			set
			{
				this._KeyAlgorithm = value;
			}
		}

		/// <summary>Creates an <see cref="T:System.Security.Cryptography.AsymmetricSignatureDeformatter" /> instance with the specified key using the <see cref="P:System.Security.Cryptography.SignatureDescription.DeformatterAlgorithm" /> property.</summary>
		/// <returns>The newly created <see cref="T:System.Security.Cryptography.AsymmetricSignatureDeformatter" /> instance.</returns>
		/// <param name="key">The key to use in the <see cref="T:System.Security.Cryptography.AsymmetricSignatureDeformatter" />. </param>
		public virtual AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
		{
			if (this._DeformatterAlgorithm == null)
			{
				throw new ArgumentNullException("DeformatterAlgorithm");
			}
			AsymmetricSignatureDeformatter asymmetricSignatureDeformatter = (AsymmetricSignatureDeformatter)CryptoConfig.CreateFromName(this._DeformatterAlgorithm);
			if (this._KeyAlgorithm == null)
			{
				throw new NullReferenceException("KeyAlgorithm");
			}
			asymmetricSignatureDeformatter.SetKey(key);
			return asymmetricSignatureDeformatter;
		}

		/// <summary>Creates a <see cref="T:System.Security.Cryptography.HashAlgorithm" /> instance using the <see cref="P:System.Security.Cryptography.SignatureDescription.DigestAlgorithm" /> property.</summary>
		/// <returns>The newly created <see cref="T:System.Security.Cryptography.HashAlgorithm" /> instance.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public virtual HashAlgorithm CreateDigest()
		{
			if (this._DigestAlgorithm == null)
			{
				throw new ArgumentNullException("DigestAlgorithm");
			}
			return (HashAlgorithm)CryptoConfig.CreateFromName(this._DigestAlgorithm);
		}

		/// <summary>Creates an <see cref="T:System.Security.Cryptography.AsymmetricSignatureFormatter" /> instance with the specified key using the <see cref="P:System.Security.Cryptography.SignatureDescription.FormatterAlgorithm" /> property.</summary>
		/// <returns>The newly created <see cref="T:System.Security.Cryptography.AsymmetricSignatureFormatter" /> instance.</returns>
		/// <param name="key">The key to use in the <see cref="T:System.Security.Cryptography.AsymmetricSignatureFormatter" />. </param>
		public virtual AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
		{
			if (this._FormatterAlgorithm == null)
			{
				throw new ArgumentNullException("FormatterAlgorithm");
			}
			AsymmetricSignatureFormatter asymmetricSignatureFormatter = (AsymmetricSignatureFormatter)CryptoConfig.CreateFromName(this._FormatterAlgorithm);
			if (this._KeyAlgorithm == null)
			{
				throw new NullReferenceException("KeyAlgorithm");
			}
			asymmetricSignatureFormatter.SetKey(key);
			return asymmetricSignatureFormatter;
		}
	}
}
