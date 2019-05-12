using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Represents the base class from which all implementations of the <see cref="T:System.Security.Cryptography.RC2" /> algorithm must derive.</summary>
	[ComVisible(true)]
	public abstract class RC2 : SymmetricAlgorithm
	{
		/// <summary>Represents the effective size of the secret key used by the <see cref="T:System.Security.Cryptography.RC2" /> algorithm in bits.</summary>
		protected int EffectiveKeySizeValue;

		/// <summary>Initializes a new instance of <see cref="T:System.Security.Cryptography.RC2" />.</summary>
		protected RC2()
		{
			this.KeySizeValue = 128;
			this.BlockSizeValue = 64;
			this.FeedbackSizeValue = 8;
			this.LegalKeySizesValue = new KeySizes[1];
			this.LegalKeySizesValue[0] = new KeySizes(40, 128, 8);
			this.LegalBlockSizesValue = new KeySizes[1];
			this.LegalBlockSizesValue[0] = new KeySizes(64, 64, 0);
		}

		/// <summary>Creates an instance of a cryptographic object to perform the <see cref="T:System.Security.Cryptography.RC2" /> algorithm.</summary>
		/// <returns>An instance of a cryptographic object.</returns>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The algorithm was used with Federal Information Processing Standards (FIPS) mode enabled, but is not FIPS compatible.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public new static RC2 Create()
		{
			return RC2.Create("System.Security.Cryptography.RC2");
		}

		/// <summary>Creates an instance of a cryptographic object to perform the specified implementation of the <see cref="T:System.Security.Cryptography.RC2" /> algorithm.</summary>
		/// <returns>An instance of a cryptographic object.</returns>
		/// <param name="AlgName">The name of the specific implementation of <see cref="T:System.Security.Cryptography.RC2" /> to use. </param>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The algorithm described by the <paramref name="algName" /> parameter was used with Federal Information Processing Standards (FIPS) mode enabled, but is not FIPS compatible.</exception>
		public new static RC2 Create(string AlgName)
		{
			return (RC2)CryptoConfig.CreateFromName(AlgName);
		}

		/// <summary>Gets or sets the effective size of the secret key used by the <see cref="T:System.Security.Cryptography.RC2" /> algorithm in bits.</summary>
		/// <returns>The effective key size used by the <see cref="T:System.Security.Cryptography.RC2" /> algorithm.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The effective key size is invalid. </exception>
		public virtual int EffectiveKeySize
		{
			get
			{
				if (this.EffectiveKeySizeValue == 0)
				{
					return this.KeySizeValue;
				}
				return this.EffectiveKeySizeValue;
			}
			set
			{
				this.EffectiveKeySizeValue = value;
			}
		}

		/// <summary>Gets or sets the size of the secret key used by the <see cref="T:System.Security.Cryptography.RC2" /> algorithm in bits.</summary>
		/// <returns>The size of the secret key used by the <see cref="T:System.Security.Cryptography.RC2" /> algorithm.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The value for the RC2 key size is less than the effective key size value.</exception>
		public override int KeySize
		{
			get
			{
				return base.KeySize;
			}
			set
			{
				base.KeySize = value;
				this.EffectiveKeySizeValue = value;
			}
		}
	}
}
