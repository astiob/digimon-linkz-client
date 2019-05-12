using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Represents the base class for the Data Encryption Standard (DES) algorithm from which all <see cref="T:System.Security.Cryptography.DES" /> implementations must derive.</summary>
	[ComVisible(true)]
	public abstract class DES : SymmetricAlgorithm
	{
		private const int keySizeByte = 8;

		internal static readonly byte[,] weakKeys = new byte[,]
		{
			{
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1
			},
			{
				31,
				31,
				31,
				31,
				15,
				15,
				15,
				15
			},
			{
				225,
				225,
				225,
				225,
				241,
				241,
				241,
				241
			},
			{
				byte.MaxValue,
				byte.MaxValue,
				byte.MaxValue,
				byte.MaxValue,
				byte.MaxValue,
				byte.MaxValue,
				byte.MaxValue,
				byte.MaxValue
			}
		};

		internal static readonly byte[,] semiWeakKeys = new byte[,]
		{
			{
				0,
				30,
				0,
				30,
				0,
				14,
				0,
				14
			},
			{
				0,
				224,
				0,
				224,
				0,
				240,
				0,
				240
			},
			{
				0,
				254,
				0,
				254,
				0,
				254,
				0,
				254
			},
			{
				30,
				0,
				30,
				0,
				14,
				0,
				14,
				0
			},
			{
				30,
				224,
				30,
				224,
				14,
				240,
				14,
				240
			},
			{
				30,
				254,
				30,
				254,
				14,
				254,
				14,
				254
			},
			{
				224,
				0,
				224,
				0,
				240,
				0,
				240,
				0
			},
			{
				224,
				30,
				224,
				30,
				240,
				14,
				240,
				14
			},
			{
				224,
				254,
				224,
				254,
				240,
				254,
				240,
				254
			},
			{
				254,
				0,
				254,
				0,
				254,
				0,
				254,
				0
			},
			{
				254,
				30,
				254,
				30,
				254,
				14,
				254,
				14
			},
			{
				254,
				224,
				254,
				224,
				254,
				240,
				254,
				240
			}
		};

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.DES" /> class.</summary>
		protected DES()
		{
			this.KeySizeValue = 64;
			this.BlockSizeValue = 64;
			this.FeedbackSizeValue = 8;
			this.LegalKeySizesValue = new KeySizes[1];
			this.LegalKeySizesValue[0] = new KeySizes(64, 64, 0);
			this.LegalBlockSizesValue = new KeySizes[1];
			this.LegalBlockSizesValue[0] = new KeySizes(64, 64, 0);
		}

		/// <summary>Creates an instance of a cryptographic object to perform the Data Encryption Standard (<see cref="T:System.Security.Cryptography.DES" />) algorithm.</summary>
		/// <returns>A cryptographic object.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public new static DES Create()
		{
			return DES.Create("System.Security.Cryptography.DES");
		}

		/// <summary>Creates an instance of a cryptographic object to perform the specified implementation of the Data Encryption Standard (<see cref="T:System.Security.Cryptography.DES" />) algorithm.</summary>
		/// <returns>A cryptographic object.</returns>
		/// <param name="algName">The name of the specific implementation of <see cref="T:System.Security.Cryptography.DES" /> to use. </param>
		public new static DES Create(string algName)
		{
			return (DES)CryptoConfig.CreateFromName(algName);
		}

		/// <summary>Determines whether the specified key is weak.</summary>
		/// <returns>true if the key is weak; otherwise, false.</returns>
		/// <param name="rgbKey">The secret key to test for weakness. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The size of the <paramref name="rgbKey" /> parameter is not valid. </exception>
		public static bool IsWeakKey(byte[] rgbKey)
		{
			if (rgbKey == null)
			{
				throw new CryptographicException(Locale.GetText("Null Key"));
			}
			if (rgbKey.Length != 8)
			{
				throw new CryptographicException(Locale.GetText("Wrong Key Length"));
			}
			for (int i = 0; i < rgbKey.Length; i++)
			{
				int num = (int)(rgbKey[i] | 17);
				if (num != 17 && num != 31 && num != 241 && num != 255)
				{
					return false;
				}
			}
			for (int j = 0; j < DES.weakKeys.Length >> 3; j++)
			{
				int k;
				for (k = 0; k < rgbKey.Length; k++)
				{
					if ((rgbKey[k] ^ DES.weakKeys[j, k]) > 1)
					{
						break;
					}
				}
				if (k == 8)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Determines whether the specified key is semi-weak.</summary>
		/// <returns>true if the key is semi-weak; otherwise, false.</returns>
		/// <param name="rgbKey">The secret key to test for semi-weakness. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The size of the <paramref name="rgbKey" /> parameter is not valid. </exception>
		public static bool IsSemiWeakKey(byte[] rgbKey)
		{
			if (rgbKey == null)
			{
				throw new CryptographicException(Locale.GetText("Null Key"));
			}
			if (rgbKey.Length != 8)
			{
				throw new CryptographicException(Locale.GetText("Wrong Key Length"));
			}
			for (int i = 0; i < rgbKey.Length; i++)
			{
				int num = (int)(rgbKey[i] | 17);
				if (num != 17 && num != 31 && num != 241 && num != 255)
				{
					return false;
				}
			}
			for (int j = 0; j < DES.semiWeakKeys.Length >> 3; j++)
			{
				int k;
				for (k = 0; k < rgbKey.Length; k++)
				{
					if ((rgbKey[k] ^ DES.semiWeakKeys[j, k]) > 1)
					{
						break;
					}
				}
				if (k == 8)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Gets or sets the secret key for the Data Encryption Standard (<see cref="T:System.Security.Cryptography.DES" />) algorithm.</summary>
		/// <returns>The secret key for the <see cref="T:System.Security.Cryptography.DES" /> algorithm.</returns>
		/// <exception cref="T:System.ArgumentNullException">An attempt was made to set the key to null. </exception>
		/// <exception cref="T:System.ArgumentException">An attempt was made to set a key whose length is not equal to <see cref="F:System.Security.Cryptography.SymmetricAlgorithm.BlockSizeValue" />. </exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An attempt was made to set a weak key (see <see cref="M:System.Security.Cryptography.DES.IsWeakKey(System.Byte[])" />) or a semi-weak key (see <see cref="M:System.Security.Cryptography.DES.IsSemiWeakKey(System.Byte[])" />). </exception>
		public override byte[] Key
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
				if (value.Length != 8)
				{
					throw new ArgumentException(Locale.GetText("Wrong Key Length"));
				}
				if (DES.IsWeakKey(value))
				{
					throw new CryptographicException(Locale.GetText("Weak Key"));
				}
				if (DES.IsSemiWeakKey(value))
				{
					throw new CryptographicException(Locale.GetText("Semi Weak Key"));
				}
				this.KeyValue = (byte[])value.Clone();
			}
		}
	}
}
