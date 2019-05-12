using Mono.Security.Cryptography;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Mono.Security.Authenticode
{
	public class PrivateKey
	{
		private const uint magic = 2964713758u;

		private bool encrypted;

		private RSA rsa;

		private bool weak;

		private int keyType;

		public PrivateKey()
		{
			this.keyType = 2;
		}

		public PrivateKey(byte[] data, string password)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (!this.Decode(data, password))
			{
				throw new CryptographicException(Locale.GetText("Invalid data and/or password"));
			}
		}

		public bool Encrypted
		{
			get
			{
				return this.encrypted;
			}
		}

		public int KeyType
		{
			get
			{
				return this.keyType;
			}
			set
			{
				this.keyType = value;
			}
		}

		public RSA RSA
		{
			get
			{
				return this.rsa;
			}
			set
			{
				this.rsa = value;
			}
		}

		public bool Weak
		{
			get
			{
				return !this.encrypted || this.weak;
			}
			set
			{
				this.weak = value;
			}
		}

		private byte[] DeriveKey(byte[] salt, string password)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(password);
			SHA1 sha = SHA1.Create();
			sha.TransformBlock(salt, 0, salt.Length, salt, 0);
			sha.TransformFinalBlock(bytes, 0, bytes.Length);
			byte[] array = new byte[16];
			Buffer.BlockCopy(sha.Hash, 0, array, 0, 16);
			sha.Clear();
			Array.Clear(bytes, 0, bytes.Length);
			return array;
		}

		private bool Decode(byte[] pvk, string password)
		{
			if (BitConverterLE.ToUInt32(pvk, 0) != 2964713758u)
			{
				return false;
			}
			if (BitConverterLE.ToUInt32(pvk, 4) != 0u)
			{
				return false;
			}
			this.keyType = BitConverterLE.ToInt32(pvk, 8);
			this.encrypted = (BitConverterLE.ToUInt32(pvk, 12) == 1u);
			int num = BitConverterLE.ToInt32(pvk, 16);
			int num2 = BitConverterLE.ToInt32(pvk, 20);
			byte[] array = new byte[num2];
			Buffer.BlockCopy(pvk, 24 + num, array, 0, num2);
			if (num > 0)
			{
				if (password == null)
				{
					return false;
				}
				byte[] array2 = new byte[num];
				Buffer.BlockCopy(pvk, 24, array2, 0, num);
				byte[] array3 = this.DeriveKey(array2, password);
				RC4 rc = RC4.Create();
				ICryptoTransform cryptoTransform = rc.CreateDecryptor(array3, null);
				cryptoTransform.TransformBlock(array, 8, array.Length - 8, array, 8);
				try
				{
					this.rsa = CryptoConvert.FromCapiPrivateKeyBlob(array);
					this.weak = false;
				}
				catch (CryptographicException)
				{
					this.weak = true;
					Buffer.BlockCopy(pvk, 24 + num, array, 0, num2);
					Array.Clear(array3, 5, 11);
					RC4 rc2 = RC4.Create();
					cryptoTransform = rc2.CreateDecryptor(array3, null);
					cryptoTransform.TransformBlock(array, 8, array.Length - 8, array, 8);
					this.rsa = CryptoConvert.FromCapiPrivateKeyBlob(array);
				}
				Array.Clear(array3, 0, array3.Length);
			}
			else
			{
				this.weak = true;
				this.rsa = CryptoConvert.FromCapiPrivateKeyBlob(array);
				Array.Clear(array, 0, array.Length);
			}
			Array.Clear(pvk, 0, pvk.Length);
			return this.rsa != null;
		}

		public void Save(string filename)
		{
			this.Save(filename, null);
		}

		public void Save(string filename, string password)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			byte[] array = null;
			FileStream fileStream = File.Open(filename, FileMode.Create, FileAccess.Write);
			try
			{
				byte[] array2 = new byte[4];
				byte[] bytes = BitConverterLE.GetBytes(2964713758u);
				fileStream.Write(bytes, 0, 4);
				fileStream.Write(array2, 0, 4);
				bytes = BitConverterLE.GetBytes(this.keyType);
				fileStream.Write(bytes, 0, 4);
				this.encrypted = (password != null);
				array = CryptoConvert.ToCapiPrivateKeyBlob(this.rsa);
				if (this.encrypted)
				{
					bytes = BitConverterLE.GetBytes(1);
					fileStream.Write(bytes, 0, 4);
					bytes = BitConverterLE.GetBytes(16);
					fileStream.Write(bytes, 0, 4);
					bytes = BitConverterLE.GetBytes(array.Length);
					fileStream.Write(bytes, 0, 4);
					byte[] array3 = new byte[16];
					RC4 rc = RC4.Create();
					byte[] array4 = null;
					try
					{
						RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
						randomNumberGenerator.GetBytes(array3);
						fileStream.Write(array3, 0, array3.Length);
						array4 = this.DeriveKey(array3, password);
						if (this.Weak)
						{
							Array.Clear(array4, 5, 11);
						}
						ICryptoTransform cryptoTransform = rc.CreateEncryptor(array4, null);
						cryptoTransform.TransformBlock(array, 8, array.Length - 8, array, 8);
					}
					finally
					{
						Array.Clear(array3, 0, array3.Length);
						Array.Clear(array4, 0, array4.Length);
						rc.Clear();
					}
				}
				else
				{
					fileStream.Write(array2, 0, 4);
					fileStream.Write(array2, 0, 4);
					bytes = BitConverterLE.GetBytes(array.Length);
					fileStream.Write(bytes, 0, 4);
				}
				fileStream.Write(array, 0, array.Length);
			}
			finally
			{
				Array.Clear(array, 0, array.Length);
				fileStream.Close();
			}
		}

		public static PrivateKey CreateFromFile(string filename)
		{
			return PrivateKey.CreateFromFile(filename, null);
		}

		public static PrivateKey CreateFromFile(string filename, string password)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			byte[] array = null;
			using (FileStream fileStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
				fileStream.Close();
			}
			return new PrivateKey(array, password);
		}
	}
}
