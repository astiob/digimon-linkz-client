using Mono.Security.Cryptography;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Mono.Security.Protocol.Tls
{
	internal abstract class CipherSuite
	{
		public static byte[] EmptyArray = new byte[0];

		private short code;

		private string name;

		private CipherAlgorithmType cipherAlgorithmType;

		private HashAlgorithmType hashAlgorithmType;

		private ExchangeAlgorithmType exchangeAlgorithmType;

		private bool isExportable;

		private CipherMode cipherMode;

		private byte keyMaterialSize;

		private int keyBlockSize;

		private byte expandedKeyMaterialSize;

		private short effectiveKeyBits;

		private byte ivSize;

		private byte blockSize;

		private Context context;

		private SymmetricAlgorithm encryptionAlgorithm;

		private ICryptoTransform encryptionCipher;

		private SymmetricAlgorithm decryptionAlgorithm;

		private ICryptoTransform decryptionCipher;

		private KeyedHashAlgorithm clientHMAC;

		private KeyedHashAlgorithm serverHMAC;

		public CipherSuite(short code, string name, CipherAlgorithmType cipherAlgorithmType, HashAlgorithmType hashAlgorithmType, ExchangeAlgorithmType exchangeAlgorithmType, bool exportable, bool blockMode, byte keyMaterialSize, byte expandedKeyMaterialSize, short effectiveKeyBits, byte ivSize, byte blockSize)
		{
			this.code = code;
			this.name = name;
			this.cipherAlgorithmType = cipherAlgorithmType;
			this.hashAlgorithmType = hashAlgorithmType;
			this.exchangeAlgorithmType = exchangeAlgorithmType;
			this.isExportable = exportable;
			if (blockMode)
			{
				this.cipherMode = CipherMode.CBC;
			}
			this.keyMaterialSize = keyMaterialSize;
			this.expandedKeyMaterialSize = expandedKeyMaterialSize;
			this.effectiveKeyBits = effectiveKeyBits;
			this.ivSize = ivSize;
			this.blockSize = blockSize;
			this.keyBlockSize = (int)this.keyMaterialSize + this.HashSize + (int)this.ivSize << 1;
		}

		protected ICryptoTransform EncryptionCipher
		{
			get
			{
				return this.encryptionCipher;
			}
		}

		protected ICryptoTransform DecryptionCipher
		{
			get
			{
				return this.decryptionCipher;
			}
		}

		protected KeyedHashAlgorithm ClientHMAC
		{
			get
			{
				return this.clientHMAC;
			}
		}

		protected KeyedHashAlgorithm ServerHMAC
		{
			get
			{
				return this.serverHMAC;
			}
		}

		public CipherAlgorithmType CipherAlgorithmType
		{
			get
			{
				return this.cipherAlgorithmType;
			}
		}

		public string HashAlgorithmName
		{
			get
			{
				switch (this.hashAlgorithmType)
				{
				case HashAlgorithmType.Md5:
					return "MD5";
				case HashAlgorithmType.Sha1:
					return "SHA1";
				}
				return "None";
			}
		}

		public HashAlgorithmType HashAlgorithmType
		{
			get
			{
				return this.hashAlgorithmType;
			}
		}

		public int HashSize
		{
			get
			{
				switch (this.hashAlgorithmType)
				{
				case HashAlgorithmType.Md5:
					return 16;
				case HashAlgorithmType.Sha1:
					return 20;
				}
				return 0;
			}
		}

		public ExchangeAlgorithmType ExchangeAlgorithmType
		{
			get
			{
				return this.exchangeAlgorithmType;
			}
		}

		public CipherMode CipherMode
		{
			get
			{
				return this.cipherMode;
			}
		}

		public short Code
		{
			get
			{
				return this.code;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public bool IsExportable
		{
			get
			{
				return this.isExportable;
			}
		}

		public byte KeyMaterialSize
		{
			get
			{
				return this.keyMaterialSize;
			}
		}

		public int KeyBlockSize
		{
			get
			{
				return this.keyBlockSize;
			}
		}

		public byte ExpandedKeyMaterialSize
		{
			get
			{
				return this.expandedKeyMaterialSize;
			}
		}

		public short EffectiveKeyBits
		{
			get
			{
				return this.effectiveKeyBits;
			}
		}

		public byte IvSize
		{
			get
			{
				return this.ivSize;
			}
		}

		public Context Context
		{
			get
			{
				return this.context;
			}
			set
			{
				this.context = value;
			}
		}

		internal void Write(byte[] array, int offset, short value)
		{
			if (offset > array.Length - 2)
			{
				throw new ArgumentException("offset");
			}
			array[offset] = (byte)(value >> 8);
			array[offset + 1] = (byte)value;
		}

		internal void Write(byte[] array, int offset, ulong value)
		{
			if (offset > array.Length - 8)
			{
				throw new ArgumentException("offset");
			}
			array[offset] = (byte)(value >> 56);
			array[offset + 1] = (byte)(value >> 48);
			array[offset + 2] = (byte)(value >> 40);
			array[offset + 3] = (byte)(value >> 32);
			array[offset + 4] = (byte)(value >> 24);
			array[offset + 5] = (byte)(value >> 16);
			array[offset + 6] = (byte)(value >> 8);
			array[offset + 7] = (byte)value;
		}

		public void InitializeCipher()
		{
			this.createEncryptionCipher();
			this.createDecryptionCipher();
		}

		public byte[] EncryptRecord(byte[] fragment, byte[] mac)
		{
			int num = fragment.Length + mac.Length;
			int num2 = 0;
			if (this.CipherMode == CipherMode.CBC)
			{
				num++;
				num2 = (int)this.blockSize - num % (int)this.blockSize;
				if (num2 == (int)this.blockSize)
				{
					num2 = 0;
				}
				num += num2;
			}
			byte[] array = new byte[num];
			Buffer.BlockCopy(fragment, 0, array, 0, fragment.Length);
			Buffer.BlockCopy(mac, 0, array, fragment.Length, mac.Length);
			if (num2 > 0)
			{
				int num3 = fragment.Length + mac.Length;
				for (int i = num3; i < num3 + num2 + 1; i++)
				{
					array[i] = (byte)num2;
				}
			}
			this.EncryptionCipher.TransformBlock(array, 0, array.Length, array, 0);
			return array;
		}

		public void DecryptRecord(byte[] fragment, out byte[] dcrFragment, out byte[] dcrMAC)
		{
			this.DecryptionCipher.TransformBlock(fragment, 0, fragment.Length, fragment, 0);
			int num2;
			if (this.CipherMode == CipherMode.CBC)
			{
				int num = (int)fragment[fragment.Length - 1];
				num2 = fragment.Length - (num + 1) - this.HashSize;
			}
			else
			{
				num2 = fragment.Length - this.HashSize;
			}
			dcrFragment = new byte[num2];
			dcrMAC = new byte[this.HashSize];
			Buffer.BlockCopy(fragment, 0, dcrFragment, 0, dcrFragment.Length);
			Buffer.BlockCopy(fragment, dcrFragment.Length, dcrMAC, 0, dcrMAC.Length);
		}

		public abstract byte[] ComputeClientRecordMAC(ContentType contentType, byte[] fragment);

		public abstract byte[] ComputeServerRecordMAC(ContentType contentType, byte[] fragment);

		public abstract void ComputeMasterSecret(byte[] preMasterSecret);

		public abstract void ComputeKeys();

		public byte[] CreatePremasterSecret()
		{
			ClientContext clientContext = (ClientContext)this.context;
			byte[] secureRandomBytes = this.context.GetSecureRandomBytes(48);
			secureRandomBytes[0] = (byte)(clientContext.ClientHelloProtocol >> 8);
			secureRandomBytes[1] = (byte)clientContext.ClientHelloProtocol;
			return secureRandomBytes;
		}

		public byte[] PRF(byte[] secret, string label, byte[] data, int length)
		{
			int num = secret.Length >> 1;
			if ((secret.Length & 1) == 1)
			{
				num++;
			}
			TlsStream tlsStream = new TlsStream();
			tlsStream.Write(Encoding.ASCII.GetBytes(label));
			tlsStream.Write(data);
			byte[] seed = tlsStream.ToArray();
			tlsStream.Reset();
			byte[] array = new byte[num];
			Buffer.BlockCopy(secret, 0, array, 0, num);
			byte[] array2 = new byte[num];
			Buffer.BlockCopy(secret, secret.Length - num, array2, 0, num);
			byte[] array3 = this.Expand("MD5", array, seed, length);
			byte[] array4 = this.Expand("SHA1", array2, seed, length);
			byte[] array5 = new byte[length];
			for (int i = 0; i < array5.Length; i++)
			{
				array5[i] = (array3[i] ^ array4[i]);
			}
			return array5;
		}

		public byte[] Expand(string hashName, byte[] secret, byte[] seed, int length)
		{
			int num = (!(hashName == "MD5")) ? 20 : 16;
			int num2 = length / num;
			if (length % num > 0)
			{
				num2++;
			}
			Mono.Security.Cryptography.HMAC hmac = new Mono.Security.Cryptography.HMAC(hashName, secret);
			TlsStream tlsStream = new TlsStream();
			byte[][] array = new byte[num2 + 1][];
			array[0] = seed;
			for (int i = 1; i <= num2; i++)
			{
				TlsStream tlsStream2 = new TlsStream();
				hmac.TransformFinalBlock(array[i - 1], 0, array[i - 1].Length);
				array[i] = hmac.Hash;
				tlsStream2.Write(array[i]);
				tlsStream2.Write(seed);
				hmac.TransformFinalBlock(tlsStream2.ToArray(), 0, (int)tlsStream2.Length);
				tlsStream.Write(hmac.Hash);
				tlsStream2.Reset();
			}
			byte[] array2 = new byte[length];
			Buffer.BlockCopy(tlsStream.ToArray(), 0, array2, 0, array2.Length);
			tlsStream.Reset();
			return array2;
		}

		private void createEncryptionCipher()
		{
			switch (this.cipherAlgorithmType)
			{
			case CipherAlgorithmType.Des:
				this.encryptionAlgorithm = DES.Create();
				break;
			case CipherAlgorithmType.Rc2:
				this.encryptionAlgorithm = RC2.Create();
				break;
			case CipherAlgorithmType.Rc4:
				this.encryptionAlgorithm = new ARC4Managed();
				break;
			case CipherAlgorithmType.Rijndael:
				this.encryptionAlgorithm = Rijndael.Create();
				break;
			case CipherAlgorithmType.TripleDes:
				this.encryptionAlgorithm = TripleDES.Create();
				break;
			}
			if (this.cipherMode == CipherMode.CBC)
			{
				this.encryptionAlgorithm.Mode = this.cipherMode;
				this.encryptionAlgorithm.Padding = PaddingMode.None;
				this.encryptionAlgorithm.KeySize = (int)(this.expandedKeyMaterialSize * 8);
				this.encryptionAlgorithm.BlockSize = (int)(this.blockSize * 8);
			}
			if (this.context is ClientContext)
			{
				this.encryptionAlgorithm.Key = this.context.ClientWriteKey;
				this.encryptionAlgorithm.IV = this.context.ClientWriteIV;
			}
			else
			{
				this.encryptionAlgorithm.Key = this.context.ServerWriteKey;
				this.encryptionAlgorithm.IV = this.context.ServerWriteIV;
			}
			this.encryptionCipher = this.encryptionAlgorithm.CreateEncryptor();
			if (this.context is ClientContext)
			{
				this.clientHMAC = new Mono.Security.Cryptography.HMAC(this.HashAlgorithmName, this.context.Negotiating.ClientWriteMAC);
			}
			else
			{
				this.serverHMAC = new Mono.Security.Cryptography.HMAC(this.HashAlgorithmName, this.context.Negotiating.ServerWriteMAC);
			}
		}

		private void createDecryptionCipher()
		{
			switch (this.cipherAlgorithmType)
			{
			case CipherAlgorithmType.Des:
				this.decryptionAlgorithm = DES.Create();
				break;
			case CipherAlgorithmType.Rc2:
				this.decryptionAlgorithm = RC2.Create();
				break;
			case CipherAlgorithmType.Rc4:
				this.decryptionAlgorithm = new ARC4Managed();
				break;
			case CipherAlgorithmType.Rijndael:
				this.decryptionAlgorithm = Rijndael.Create();
				break;
			case CipherAlgorithmType.TripleDes:
				this.decryptionAlgorithm = TripleDES.Create();
				break;
			}
			if (this.cipherMode == CipherMode.CBC)
			{
				this.decryptionAlgorithm.Mode = this.cipherMode;
				this.decryptionAlgorithm.Padding = PaddingMode.None;
				this.decryptionAlgorithm.KeySize = (int)(this.expandedKeyMaterialSize * 8);
				this.decryptionAlgorithm.BlockSize = (int)(this.blockSize * 8);
			}
			if (this.context is ClientContext)
			{
				this.decryptionAlgorithm.Key = this.context.ServerWriteKey;
				this.decryptionAlgorithm.IV = this.context.ServerWriteIV;
			}
			else
			{
				this.decryptionAlgorithm.Key = this.context.ClientWriteKey;
				this.decryptionAlgorithm.IV = this.context.ClientWriteIV;
			}
			this.decryptionCipher = this.decryptionAlgorithm.CreateDecryptor();
			if (this.context is ClientContext)
			{
				this.serverHMAC = new Mono.Security.Cryptography.HMAC(this.HashAlgorithmName, this.context.Negotiating.ServerWriteMAC);
			}
			else
			{
				this.clientHMAC = new Mono.Security.Cryptography.HMAC(this.HashAlgorithmName, this.context.Negotiating.ClientWriteMAC);
			}
		}
	}
}
