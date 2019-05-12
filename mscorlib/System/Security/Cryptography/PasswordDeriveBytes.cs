using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Cryptography
{
	/// <summary>Derives a key from a password using an extension of the PBKDF1 algorithm.</summary>
	[ComVisible(true)]
	public class PasswordDeriveBytes : DeriveBytes
	{
		private string HashNameValue;

		private byte[] SaltValue;

		private int IterationsValue;

		private HashAlgorithm hash;

		private int state;

		private byte[] password;

		private byte[] initial;

		private byte[] output;

		private int position;

		private int hashnumber;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.PasswordDeriveBytes" /> class with the password and key salt to use to derive the key.</summary>
		/// <param name="strPassword">The password for which to derive the key. </param>
		/// <param name="rgbSalt">The key salt to use to derive the key. </param>
		public PasswordDeriveBytes(string strPassword, byte[] rgbSalt)
		{
			this.Prepare(strPassword, rgbSalt, "SHA1", 100);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.PasswordDeriveBytes" /> class with the password, key salt, and cryptographic service provider (CSP) parameters to use to derive the key.</summary>
		/// <param name="strPassword">The password for which to derive the key. </param>
		/// <param name="rgbSalt">The key salt to use to derive the key. </param>
		/// <param name="cspParams">The CSP parameters for the operation. </param>
		public PasswordDeriveBytes(string strPassword, byte[] rgbSalt, CspParameters cspParams)
		{
			this.Prepare(strPassword, rgbSalt, "SHA1", 100);
			if (cspParams != null)
			{
				throw new NotSupportedException(Locale.GetText("CspParameters not supported by Mono for PasswordDeriveBytes."));
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.PasswordDeriveBytes" /> class with the password, key salt, hash name, and number of iterations to use to derive the key.</summary>
		/// <param name="strPassword">The password for which to derive the key. </param>
		/// <param name="rgbSalt">The key salt to use to derive the key. </param>
		/// <param name="strHashName">The name of the hash algorithm for the operation. </param>
		/// <param name="iterations">The number of iterations for the operation. </param>
		public PasswordDeriveBytes(string strPassword, byte[] rgbSalt, string strHashName, int iterations)
		{
			this.Prepare(strPassword, rgbSalt, strHashName, iterations);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.PasswordDeriveBytes" /> class with the password, key salt, hash name, number of iterations, and cryptographic service provider (CSP) parameters to use to derive the key.</summary>
		/// <param name="strPassword">The password for which to derive the key. </param>
		/// <param name="rgbSalt">The key salt to use to derive the key. </param>
		/// <param name="strHashName">The name of the hash algorithm for the operation. </param>
		/// <param name="iterations">The number of iterations for the operation. </param>
		/// <param name="cspParams">The CSP parameters for the operation. </param>
		public PasswordDeriveBytes(string strPassword, byte[] rgbSalt, string strHashName, int iterations, CspParameters cspParams)
		{
			this.Prepare(strPassword, rgbSalt, strHashName, iterations);
			if (cspParams != null)
			{
				throw new NotSupportedException(Locale.GetText("CspParameters not supported by Mono for PasswordDeriveBytes."));
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.PasswordDeriveBytes" /> class specifying the password and key salt to use to derive the key.</summary>
		/// <param name="password">The password to derive the key for.</param>
		/// <param name="salt">The key salt to use to derive the key.</param>
		public PasswordDeriveBytes(byte[] password, byte[] salt)
		{
			this.Prepare(password, salt, "SHA1", 100);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.PasswordDeriveBytes" /> class specifying the password, key salt, and cryptographic service provider (CSP) to use to derive the key.</summary>
		/// <param name="password">The password to derive the key for.</param>
		/// <param name="salt">The key salt to use to derive the key.</param>
		/// <param name="cspParams">The cryptographic service provider (CSP) parameters for the operation.</param>
		public PasswordDeriveBytes(byte[] password, byte[] salt, CspParameters cspParams)
		{
			this.Prepare(password, salt, "SHA1", 100);
			if (cspParams != null)
			{
				throw new NotSupportedException(Locale.GetText("CspParameters not supported by Mono for PasswordDeriveBytes."));
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.PasswordDeriveBytes" /> class specifying the password, key salt, hash name, and iterations to use to derive the key.</summary>
		/// <param name="password">The password to derive the key for.</param>
		/// <param name="salt">The key salt to use to derive the key.</param>
		/// <param name="hashName">The hash algorithm to use to derive the key.</param>
		/// <param name="iterations">The iteration count to use to derive the key.</param>
		public PasswordDeriveBytes(byte[] password, byte[] salt, string hashName, int iterations)
		{
			this.Prepare(password, salt, hashName, iterations);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.PasswordDeriveBytes" /> class specifying the password, key salt, hash name, iterations, and cryptographic service provider (CSP) to use to derive the key.</summary>
		/// <param name="password">The password to derive the key for.</param>
		/// <param name="salt">The key salt to use to derive the key.</param>
		/// <param name="hashName">The hash algorithm to use to derive the key.</param>
		/// <param name="iterations">The iteration count to use to derive the key.</param>
		/// <param name="cspParams">The cryptographic service provider (CSP) parameters for the operation.</param>
		public PasswordDeriveBytes(byte[] password, byte[] salt, string hashName, int iterations, CspParameters cspParams)
		{
			this.Prepare(password, salt, hashName, iterations);
			if (cspParams != null)
			{
				throw new NotSupportedException(Locale.GetText("CspParameters not supported by Mono for PasswordDeriveBytes."));
			}
		}

		~PasswordDeriveBytes()
		{
			if (this.initial != null)
			{
				Array.Clear(this.initial, 0, this.initial.Length);
				this.initial = null;
			}
			Array.Clear(this.password, 0, this.password.Length);
		}

		private void Prepare(string strPassword, byte[] rgbSalt, string strHashName, int iterations)
		{
			if (strPassword == null)
			{
				throw new ArgumentNullException("strPassword");
			}
			byte[] bytes = Encoding.UTF8.GetBytes(strPassword);
			this.Prepare(bytes, rgbSalt, strHashName, iterations);
			Array.Clear(bytes, 0, bytes.Length);
		}

		private void Prepare(byte[] password, byte[] rgbSalt, string strHashName, int iterations)
		{
			if (password == null)
			{
				throw new ArgumentNullException("password");
			}
			this.password = (byte[])password.Clone();
			this.Salt = rgbSalt;
			this.HashName = strHashName;
			this.IterationCount = iterations;
			this.state = 0;
		}

		/// <summary>Gets or sets the name of the hash algorithm for the operation.</summary>
		/// <returns>The name of the hash algorithm for the operation.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The name of the hash value is fixed and an attempt is made to change this value. </exception>
		public string HashName
		{
			get
			{
				return this.HashNameValue;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("HashName");
				}
				if (this.state != 0)
				{
					throw new CryptographicException(Locale.GetText("Can't change this property at this stage"));
				}
				this.HashNameValue = value;
			}
		}

		/// <summary>Gets or sets the number of iterations for the operation.</summary>
		/// <returns>The number of iterations for the operation.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The number of iterations is fixed and an attempt is made to change this value. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The property cannot be set because its value is out of range. This property requires a non-negative number.</exception>
		public int IterationCount
		{
			get
			{
				return this.IterationsValue;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("> 0", "IterationCount");
				}
				if (this.state != 0)
				{
					throw new CryptographicException(Locale.GetText("Can't change this property at this stage"));
				}
				this.IterationsValue = value;
			}
		}

		/// <summary>Gets or sets the key salt value for the operation.</summary>
		/// <returns>The key salt value for the operation.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The key salt value is fixed and an attempt is made to change this value. </exception>
		public byte[] Salt
		{
			get
			{
				if (this.SaltValue == null)
				{
					return null;
				}
				return (byte[])this.SaltValue.Clone();
			}
			set
			{
				if (this.state != 0)
				{
					throw new CryptographicException(Locale.GetText("Can't change this property at this stage"));
				}
				if (value != null)
				{
					this.SaltValue = (byte[])value.Clone();
				}
				else
				{
					this.SaltValue = null;
				}
			}
		}

		/// <summary>Derives a cryptographic key from the <see cref="T:System.Security.Cryptography.PasswordDeriveBytes" /> object.</summary>
		/// <returns>The derived key.</returns>
		/// <param name="algname">The algorithm name for which to derive the key. </param>
		/// <param name="alghashname">The hash algorithm name to use to derive the key. </param>
		/// <param name="keySize">The size of the key, in bits, to derive. </param>
		/// <param name="rgbIV">The initialization vector (IV) to use to derive the key. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The <paramref name="keySize" /> parameter is incorrect.-or- The cryptographic service provider (CSP) cannot be acquired.-or- The <paramref name="algname" /> parameter is not a valid algorithm name.-or- The <paramref name="alghashname" /> parameter is not a valid hash algorithm name. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public byte[] CryptDeriveKey(string algname, string alghashname, int keySize, byte[] rgbIV)
		{
			if (keySize > 128)
			{
				throw new CryptographicException(Locale.GetText("Key Size can't be greater than 128 bits"));
			}
			throw new NotSupportedException(Locale.GetText("CspParameters not supported by Mono"));
		}

		/// <summary>Returns pseudo-random key bytes.</summary>
		/// <returns>A byte array filled with pseudo-random key bytes.</returns>
		/// <param name="cb">The number of pseudo-random key bytes to generate. </param>
		[Obsolete("see Rfc2898DeriveBytes for PKCS#5 v2 support")]
		public override byte[] GetBytes(int cb)
		{
			if (cb < 1)
			{
				throw new IndexOutOfRangeException("cb");
			}
			if (this.state == 0)
			{
				this.Reset();
				this.state = 1;
			}
			byte[] array = new byte[cb];
			int i = 0;
			int num = Math.Max(1, this.IterationsValue - 1);
			if (this.output == null)
			{
				this.output = this.initial;
				for (int j = 0; j < num - 1; j++)
				{
					this.output = this.hash.ComputeHash(this.output);
				}
			}
			while (i < cb)
			{
				byte[] array2;
				if (this.hashnumber == 0)
				{
					array2 = this.hash.ComputeHash(this.output);
				}
				else
				{
					if (this.hashnumber >= 1000)
					{
						throw new CryptographicException(Locale.GetText("too long"));
					}
					string text = Convert.ToString(this.hashnumber);
					array2 = new byte[this.output.Length + text.Length];
					for (int k = 0; k < text.Length; k++)
					{
						array2[k] = (byte)text[k];
					}
					Buffer.BlockCopy(this.output, 0, array2, text.Length, this.output.Length);
					array2 = this.hash.ComputeHash(array2);
				}
				int val = array2.Length - this.position;
				int num2 = Math.Min(cb - i, val);
				Buffer.BlockCopy(array2, this.position, array, i, num2);
				i += num2;
				this.position += num2;
				while (this.position >= array2.Length)
				{
					this.position -= array2.Length;
					this.hashnumber++;
				}
			}
			return array;
		}

		/// <summary>Resets the state of the operation.</summary>
		public override void Reset()
		{
			this.state = 0;
			this.position = 0;
			this.hashnumber = 0;
			this.hash = HashAlgorithm.Create(this.HashNameValue);
			if (this.SaltValue != null)
			{
				this.hash.TransformBlock(this.password, 0, this.password.Length, this.password, 0);
				this.hash.TransformFinalBlock(this.SaltValue, 0, this.SaltValue.Length);
				this.initial = this.hash.Hash;
			}
			else
			{
				this.initial = this.hash.ComputeHash(this.password);
			}
		}
	}
}
