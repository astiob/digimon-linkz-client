using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Represents the abstract base class from which all implementations of symmetric algorithms must inherit.</summary>
	[ComVisible(true)]
	public abstract class SymmetricAlgorithm : IDisposable
	{
		/// <summary>Represents the block size, in bits, of the cryptographic operation.</summary>
		protected int BlockSizeValue;

		/// <summary>Represents the initialization vector (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV" />) for the symmetric algorithm.</summary>
		protected byte[] IVValue;

		/// <summary>Represents the size, in bits, of the secret key used by the symmetric algorithm.</summary>
		protected int KeySizeValue;

		/// <summary>Represents the secret key for the symmetric algorithm.</summary>
		protected byte[] KeyValue;

		/// <summary>Specifies the block sizes, in bits, that are supported by the symmetric algorithm.</summary>
		protected KeySizes[] LegalBlockSizesValue;

		/// <summary>Specifies the key sizes, in bits, that are supported by the symmetric algorithm.</summary>
		protected KeySizes[] LegalKeySizesValue;

		/// <summary>Represents the feedback size, in bits, of the cryptographic operation.</summary>
		protected int FeedbackSizeValue;

		/// <summary>Represents the cipher mode used in the symmetric algorithm.</summary>
		protected CipherMode ModeValue;

		/// <summary>Represents the padding mode used in the symmetric algorithm.</summary>
		protected PaddingMode PaddingValue;

		private bool m_disposed;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.SymmetricAlgorithm" /> class.</summary>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The implementation of the class derived from the symmetric algorithm is not valid.</exception>
		protected SymmetricAlgorithm()
		{
			this.ModeValue = CipherMode.CBC;
			this.PaddingValue = PaddingMode.PKCS7;
			this.m_disposed = false;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Security.Cryptography.SymmetricAlgorithm" /> and optionally releases the managed resources. </summary>
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		~SymmetricAlgorithm()
		{
			this.Dispose(false);
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Security.Cryptography.SymmetricAlgorithm" /> class.</summary>
		public void Clear()
		{
			this.Dispose(true);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Security.Cryptography.SymmetricAlgorithm" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected virtual void Dispose(bool disposing)
		{
			if (!this.m_disposed)
			{
				if (this.KeyValue != null)
				{
					Array.Clear(this.KeyValue, 0, this.KeyValue.Length);
					this.KeyValue = null;
				}
				if (disposing)
				{
				}
				this.m_disposed = true;
			}
		}

		/// <summary>Gets or sets the block size, in bits, of the cryptographic operation.</summary>
		/// <returns>The block size, in bits.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The block size is invalid. </exception>
		public virtual int BlockSize
		{
			get
			{
				return this.BlockSizeValue;
			}
			set
			{
				if (!KeySizes.IsLegalKeySize(this.LegalBlockSizesValue, value))
				{
					throw new CryptographicException(Locale.GetText("block size not supported by algorithm"));
				}
				if (this.BlockSizeValue != value)
				{
					this.BlockSizeValue = value;
					this.IVValue = null;
				}
			}
		}

		/// <summary>Gets or sets the feedback size, in bits, of the cryptographic operation.</summary>
		/// <returns>The feedback size in bits.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The feedback size is larger than the block size. </exception>
		public virtual int FeedbackSize
		{
			get
			{
				return this.FeedbackSizeValue;
			}
			set
			{
				if (value <= 0 || value > this.BlockSizeValue)
				{
					throw new CryptographicException(Locale.GetText("feedback size larger than block size"));
				}
				this.FeedbackSizeValue = value;
			}
		}

		/// <summary>Gets or sets the initialization vector (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV" />) for the symmetric algorithm.</summary>
		/// <returns>The initialization vector.</returns>
		/// <exception cref="T:System.ArgumentNullException">An attempt was made to set the initialization vector to null. </exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An attempt was made to set the initialization vector to an invalid size. </exception>
		public virtual byte[] IV
		{
			get
			{
				if (this.IVValue == null)
				{
					this.GenerateIV();
				}
				return (byte[])this.IVValue.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("IV");
				}
				if (value.Length << 3 != this.BlockSizeValue)
				{
					throw new CryptographicException(Locale.GetText("IV length is different than block size"));
				}
				this.IVValue = (byte[])value.Clone();
			}
		}

		/// <summary>Gets or sets the secret key for the symmetric algorithm.</summary>
		/// <returns>The secret key to use for the symmetric algorithm.</returns>
		/// <exception cref="T:System.ArgumentNullException">An attempt was made to set the key to null. </exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The key size is invalid.</exception>
		public virtual byte[] Key
		{
			get
			{
				if (this.KeyValue == null)
				{
					this.GenerateKey();
				}
				return (byte[])this.KeyValue.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Key");
				}
				int num = value.Length << 3;
				if (!KeySizes.IsLegalKeySize(this.LegalKeySizesValue, num))
				{
					throw new CryptographicException(Locale.GetText("Key size not supported by algorithm"));
				}
				this.KeySizeValue = num;
				this.KeyValue = (byte[])value.Clone();
			}
		}

		/// <summary>Gets or sets the size, in bits, of the secret key used by the symmetric algorithm.</summary>
		/// <returns>The size, in bits, of the secret key used by the symmetric algorithm.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The key size is not valid. </exception>
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
					throw new CryptographicException(Locale.GetText("Key size not supported by algorithm"));
				}
				this.KeySizeValue = value;
				this.KeyValue = null;
			}
		}

		/// <summary>Gets the block sizes, in bits, that are supported by the symmetric algorithm.</summary>
		/// <returns>An array that contains the block sizes supported by the algorithm.</returns>
		public virtual KeySizes[] LegalBlockSizes
		{
			get
			{
				return this.LegalBlockSizesValue;
			}
		}

		/// <summary>Gets the key sizes, in bits, that are supported by the symmetric algorithm.</summary>
		/// <returns>An array that contains the key sizes supported by the algorithm.</returns>
		public virtual KeySizes[] LegalKeySizes
		{
			get
			{
				return this.LegalKeySizesValue;
			}
		}

		/// <summary>Gets or sets the mode for operation of the symmetric algorithm.</summary>
		/// <returns>The mode for operation of the symmetric algorithm. The default is <see cref="F:System.Security.Cryptography.CipherMode.CBC" />.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The cipher mode is not one of the <see cref="T:System.Security.Cryptography.CipherMode" /> values. </exception>
		public virtual CipherMode Mode
		{
			get
			{
				return this.ModeValue;
			}
			set
			{
				if (!Enum.IsDefined(this.ModeValue.GetType(), value))
				{
					throw new CryptographicException(Locale.GetText("Cipher mode not available"));
				}
				this.ModeValue = value;
			}
		}

		/// <summary>Gets or sets the padding mode used in the symmetric algorithm.</summary>
		/// <returns>The padding mode used in the symmetric algorithm. The default is <see cref="F:System.Security.Cryptography.PaddingMode.PKCS7" />.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The padding mode is not one of the <see cref="T:System.Security.Cryptography.PaddingMode" /> values. </exception>
		public virtual PaddingMode Padding
		{
			get
			{
				return this.PaddingValue;
			}
			set
			{
				if (!Enum.IsDefined(this.PaddingValue.GetType(), value))
				{
					throw new CryptographicException(Locale.GetText("Padding mode not available"));
				}
				this.PaddingValue = value;
			}
		}

		/// <summary>Creates a symmetric decryptor object with the current <see cref="P:System.Security.Cryptography.SymmetricAlgorithm.Key" /> property and initialization vector (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV" />).</summary>
		/// <returns>A symmetric decryptor object.</returns>
		public virtual ICryptoTransform CreateDecryptor()
		{
			return this.CreateDecryptor(this.Key, this.IV);
		}

		/// <summary>When overridden in a derived class, creates a symmetric decryptor object with the specified <see cref="P:System.Security.Cryptography.SymmetricAlgorithm.Key" /> property and initialization vector (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV" />).</summary>
		/// <returns>A symmetric decryptor object.</returns>
		/// <param name="rgbKey">The secret key to use for the symmetric algorithm. </param>
		/// <param name="rgbIV">The initialization vector to use for the symmetric algorithm. </param>
		public abstract ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV);

		/// <summary>Creates a symmetric encryptor object with the current <see cref="P:System.Security.Cryptography.SymmetricAlgorithm.Key" /> property and initialization vector (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV" />).</summary>
		/// <returns>A symmetric encryptor object.</returns>
		public virtual ICryptoTransform CreateEncryptor()
		{
			return this.CreateEncryptor(this.Key, this.IV);
		}

		/// <summary>When overridden in a derived class, creates a symmetric encryptor object with the specified <see cref="P:System.Security.Cryptography.SymmetricAlgorithm.Key" /> property and initialization vector (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV" />).</summary>
		/// <returns>A symmetric encryptor object.</returns>
		/// <param name="rgbKey">The secret key to use for the symmetric algorithm. </param>
		/// <param name="rgbIV">The initialization vector to use for the symmetric algorithm. </param>
		public abstract ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV);

		/// <summary>When overridden in a derived class, generates a random initialization vector (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV" />) to use for the algorithm.</summary>
		public abstract void GenerateIV();

		/// <summary>When overridden in a derived class, generates a random key (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.Key" />) to use for the algorithm.</summary>
		public abstract void GenerateKey();

		/// <summary>Determines whether the specified key size is valid for the current algorithm.</summary>
		/// <returns>true if the specified key size is valid for the current algorithm; otherwise, false.</returns>
		/// <param name="bitLength">The length, in bits, to check for a valid key size. </param>
		public bool ValidKeySize(int bitLength)
		{
			return KeySizes.IsLegalKeySize(this.LegalKeySizesValue, bitLength);
		}

		/// <summary>Creates a default cryptographic object used to perform the symmetric algorithm.</summary>
		/// <returns>A default cryptographic object used to perform the symmetric algorithm.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static SymmetricAlgorithm Create()
		{
			return SymmetricAlgorithm.Create("System.Security.Cryptography.SymmetricAlgorithm");
		}

		/// <summary>Creates the specified cryptographic object used to perform the symmetric algorithm.</summary>
		/// <returns>A cryptographic object used to perform the symmetric algorithm.</returns>
		/// <param name="algName">The name of the specific implementation of the <see cref="T:System.Security.Cryptography.SymmetricAlgorithm" /> class to use. </param>
		public static SymmetricAlgorithm Create(string algName)
		{
			return (SymmetricAlgorithm)CryptoConfig.CreateFromName(algName);
		}
	}
}
