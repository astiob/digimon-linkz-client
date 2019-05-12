using Mono.Security.Cryptography;
using System;
using System.Configuration.Assemblies;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;

namespace Mono.Security
{
	public sealed class StrongName
	{
		private RSA rsa;

		private byte[] publicKey;

		private byte[] keyToken;

		private string tokenAlgorithm;

		public StrongName()
		{
		}

		public StrongName(int keySize)
		{
			this.rsa = new RSAManaged(keySize);
		}

		public StrongName(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (data.Length == 16)
			{
				int i = 0;
				int num = 0;
				while (i < data.Length)
				{
					num += (int)data[i++];
				}
				if (num == 4)
				{
					this.publicKey = (byte[])data.Clone();
				}
			}
			else
			{
				this.RSA = CryptoConvert.FromCapiKeyBlob(data);
				if (this.rsa == null)
				{
					throw new ArgumentException("data isn't a correctly encoded RSA public key");
				}
			}
		}

		public StrongName(RSA rsa)
		{
			if (rsa == null)
			{
				throw new ArgumentNullException("rsa");
			}
			this.RSA = rsa;
		}

		private void InvalidateCache()
		{
			this.publicKey = null;
			this.keyToken = null;
		}

		public bool CanSign
		{
			get
			{
				if (this.rsa == null)
				{
					return false;
				}
				if (this.RSA is RSAManaged)
				{
					return !(this.rsa as RSAManaged).PublicOnly;
				}
				bool result;
				try
				{
					RSAParameters rsaparameters = this.rsa.ExportParameters(true);
					result = (rsaparameters.D != null && rsaparameters.P != null && rsaparameters.Q != null);
				}
				catch (CryptographicException)
				{
					result = false;
				}
				return result;
			}
		}

		public RSA RSA
		{
			get
			{
				if (this.rsa == null)
				{
					this.rsa = RSA.Create();
				}
				return this.rsa;
			}
			set
			{
				this.rsa = value;
				this.InvalidateCache();
			}
		}

		public byte[] PublicKey
		{
			get
			{
				if (this.publicKey == null)
				{
					byte[] array = CryptoConvert.ToCapiKeyBlob(this.rsa, false);
					this.publicKey = new byte[32 + (this.rsa.KeySize >> 3)];
					this.publicKey[0] = array[4];
					this.publicKey[1] = array[5];
					this.publicKey[2] = array[6];
					this.publicKey[3] = array[7];
					this.publicKey[4] = 4;
					this.publicKey[5] = 128;
					this.publicKey[6] = 0;
					this.publicKey[7] = 0;
					byte[] bytes = BitConverterLE.GetBytes(this.publicKey.Length - 12);
					this.publicKey[8] = bytes[0];
					this.publicKey[9] = bytes[1];
					this.publicKey[10] = bytes[2];
					this.publicKey[11] = bytes[3];
					this.publicKey[12] = 6;
					Buffer.BlockCopy(array, 1, this.publicKey, 13, this.publicKey.Length - 13);
					this.publicKey[23] = 49;
				}
				return (byte[])this.publicKey.Clone();
			}
		}

		public byte[] PublicKeyToken
		{
			get
			{
				if (this.keyToken == null)
				{
					byte[] array = this.PublicKey;
					if (array == null)
					{
						return null;
					}
					HashAlgorithm hashAlgorithm = HashAlgorithm.Create(this.TokenAlgorithm);
					byte[] array2 = hashAlgorithm.ComputeHash(array);
					this.keyToken = new byte[8];
					Buffer.BlockCopy(array2, array2.Length - 8, this.keyToken, 0, 8);
					Array.Reverse(this.keyToken, 0, 8);
				}
				return (byte[])this.keyToken.Clone();
			}
		}

		public string TokenAlgorithm
		{
			get
			{
				if (this.tokenAlgorithm == null)
				{
					this.tokenAlgorithm = "SHA1";
				}
				return this.tokenAlgorithm;
			}
			set
			{
				string a = value.ToUpper(CultureInfo.InvariantCulture);
				if (a == "SHA1" || a == "MD5")
				{
					this.tokenAlgorithm = value;
					this.InvalidateCache();
					return;
				}
				throw new ArgumentException("Unsupported hash algorithm for token");
			}
		}

		public byte[] GetBytes()
		{
			return CryptoConvert.ToCapiPrivateKeyBlob(this.RSA);
		}

		private uint RVAtoPosition(uint r, int sections, byte[] headers)
		{
			for (int i = 0; i < sections; i++)
			{
				uint num = BitConverterLE.ToUInt32(headers, i * 40 + 20);
				uint num2 = BitConverterLE.ToUInt32(headers, i * 40 + 12);
				int num3 = (int)BitConverterLE.ToUInt32(headers, i * 40 + 8);
				if (num2 <= r && (ulong)r < (ulong)num2 + (ulong)((long)num3))
				{
					return num + r - num2;
				}
			}
			return 0u;
		}

		internal StrongName.StrongNameSignature StrongHash(Stream stream, StrongName.StrongNameOptions options)
		{
			StrongName.StrongNameSignature strongNameSignature = new StrongName.StrongNameSignature();
			HashAlgorithm hashAlgorithm = HashAlgorithm.Create(this.TokenAlgorithm);
			CryptoStream cryptoStream = new CryptoStream(Stream.Null, hashAlgorithm, CryptoStreamMode.Write);
			byte[] array = new byte[128];
			stream.Read(array, 0, 128);
			if (BitConverterLE.ToUInt16(array, 0) != 23117)
			{
				return null;
			}
			uint num = BitConverterLE.ToUInt32(array, 60);
			cryptoStream.Write(array, 0, 128);
			if (num != 128u)
			{
				byte[] array2 = new byte[num - 128u];
				stream.Read(array2, 0, array2.Length);
				cryptoStream.Write(array2, 0, array2.Length);
			}
			byte[] array3 = new byte[248];
			stream.Read(array3, 0, 248);
			if (BitConverterLE.ToUInt32(array3, 0) != 17744u)
			{
				return null;
			}
			if (BitConverterLE.ToUInt16(array3, 4) != 332)
			{
				return null;
			}
			byte[] src = new byte[8];
			Buffer.BlockCopy(src, 0, array3, 88, 4);
			Buffer.BlockCopy(src, 0, array3, 152, 8);
			cryptoStream.Write(array3, 0, 248);
			ushort num2 = BitConverterLE.ToUInt16(array3, 6);
			int num3 = (int)(num2 * 40);
			byte[] array4 = new byte[num3];
			stream.Read(array4, 0, num3);
			cryptoStream.Write(array4, 0, num3);
			uint r = BitConverterLE.ToUInt32(array3, 232);
			uint num4 = this.RVAtoPosition(r, (int)num2, array4);
			int num5 = (int)BitConverterLE.ToUInt32(array3, 236);
			byte[] array5 = new byte[num5];
			stream.Position = (long)((ulong)num4);
			stream.Read(array5, 0, num5);
			uint r2 = BitConverterLE.ToUInt32(array5, 32);
			strongNameSignature.SignaturePosition = this.RVAtoPosition(r2, (int)num2, array4);
			strongNameSignature.SignatureLength = BitConverterLE.ToUInt32(array5, 36);
			uint r3 = BitConverterLE.ToUInt32(array5, 8);
			strongNameSignature.MetadataPosition = this.RVAtoPosition(r3, (int)num2, array4);
			strongNameSignature.MetadataLength = BitConverterLE.ToUInt32(array5, 12);
			if (options == StrongName.StrongNameOptions.Metadata)
			{
				cryptoStream.Close();
				hashAlgorithm.Initialize();
				byte[] array6 = new byte[strongNameSignature.MetadataLength];
				stream.Position = (long)((ulong)strongNameSignature.MetadataPosition);
				stream.Read(array6, 0, array6.Length);
				strongNameSignature.Hash = hashAlgorithm.ComputeHash(array6);
				return strongNameSignature;
			}
			for (int i = 0; i < (int)num2; i++)
			{
				uint num6 = BitConverterLE.ToUInt32(array4, i * 40 + 20);
				int num7 = (int)BitConverterLE.ToUInt32(array4, i * 40 + 16);
				byte[] array7 = new byte[num7];
				stream.Position = (long)((ulong)num6);
				stream.Read(array7, 0, num7);
				if (num6 <= strongNameSignature.SignaturePosition && (ulong)strongNameSignature.SignaturePosition < (ulong)num6 + (ulong)((long)num7))
				{
					int num8 = (int)(strongNameSignature.SignaturePosition - num6);
					if (num8 > 0)
					{
						cryptoStream.Write(array7, 0, num8);
					}
					strongNameSignature.Signature = new byte[strongNameSignature.SignatureLength];
					Buffer.BlockCopy(array7, num8, strongNameSignature.Signature, 0, (int)strongNameSignature.SignatureLength);
					Array.Reverse(strongNameSignature.Signature);
					int num9 = (int)((long)num8 + (long)((ulong)strongNameSignature.SignatureLength));
					int num10 = num7 - num9;
					if (num10 > 0)
					{
						cryptoStream.Write(array7, num9, num10);
					}
				}
				else
				{
					cryptoStream.Write(array7, 0, num7);
				}
			}
			cryptoStream.Close();
			strongNameSignature.Hash = hashAlgorithm.Hash;
			return strongNameSignature;
		}

		public byte[] Hash(string fileName)
		{
			FileStream fileStream = File.OpenRead(fileName);
			StrongName.StrongNameSignature strongNameSignature = this.StrongHash(fileStream, StrongName.StrongNameOptions.Metadata);
			fileStream.Close();
			return strongNameSignature.Hash;
		}

		public bool Sign(string fileName)
		{
			bool result = false;
			StrongName.StrongNameSignature strongNameSignature;
			using (FileStream fileStream = File.OpenRead(fileName))
			{
				strongNameSignature = this.StrongHash(fileStream, StrongName.StrongNameOptions.Signature);
				fileStream.Close();
			}
			if (strongNameSignature.Hash == null)
			{
				return false;
			}
			byte[] array = null;
			try
			{
				RSAPKCS1SignatureFormatter rsapkcs1SignatureFormatter = new RSAPKCS1SignatureFormatter(this.rsa);
				rsapkcs1SignatureFormatter.SetHashAlgorithm(this.TokenAlgorithm);
				array = rsapkcs1SignatureFormatter.CreateSignature(strongNameSignature.Hash);
				Array.Reverse(array);
			}
			catch (CryptographicException)
			{
				return false;
			}
			using (FileStream fileStream2 = File.OpenWrite(fileName))
			{
				fileStream2.Position = (long)((ulong)strongNameSignature.SignaturePosition);
				fileStream2.Write(array, 0, array.Length);
				fileStream2.Close();
				result = true;
			}
			return result;
		}

		public bool Verify(string fileName)
		{
			bool result = false;
			using (FileStream fileStream = File.OpenRead(fileName))
			{
				result = this.Verify(fileStream);
				fileStream.Close();
			}
			return result;
		}

		public bool Verify(Stream stream)
		{
			StrongName.StrongNameSignature strongNameSignature = this.StrongHash(stream, StrongName.StrongNameOptions.Signature);
			if (strongNameSignature.Hash == null)
			{
				return false;
			}
			bool result;
			try
			{
				AssemblyHashAlgorithm algorithm = AssemblyHashAlgorithm.SHA1;
				if (this.tokenAlgorithm == "MD5")
				{
					algorithm = AssemblyHashAlgorithm.MD5;
				}
				result = StrongName.Verify(this.rsa, algorithm, strongNameSignature.Hash, strongNameSignature.Signature);
			}
			catch (CryptographicException)
			{
				result = false;
			}
			return result;
		}

		private static bool Verify(RSA rsa, AssemblyHashAlgorithm algorithm, byte[] hash, byte[] signature)
		{
			RSAPKCS1SignatureDeformatter rsapkcs1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
			if (algorithm != AssemblyHashAlgorithm.MD5)
			{
				if (algorithm != AssemblyHashAlgorithm.SHA1 && algorithm != AssemblyHashAlgorithm.None)
				{
				}
				rsapkcs1SignatureDeformatter.SetHashAlgorithm("SHA1");
			}
			else
			{
				rsapkcs1SignatureDeformatter.SetHashAlgorithm("MD5");
			}
			return rsapkcs1SignatureDeformatter.VerifySignature(hash, signature);
		}

		internal class StrongNameSignature
		{
			private byte[] hash;

			private byte[] signature;

			private uint signaturePosition;

			private uint signatureLength;

			private uint metadataPosition;

			private uint metadataLength;

			private byte cliFlag;

			private uint cliFlagPosition;

			public byte[] Hash
			{
				get
				{
					return this.hash;
				}
				set
				{
					this.hash = value;
				}
			}

			public byte[] Signature
			{
				get
				{
					return this.signature;
				}
				set
				{
					this.signature = value;
				}
			}

			public uint MetadataPosition
			{
				get
				{
					return this.metadataPosition;
				}
				set
				{
					this.metadataPosition = value;
				}
			}

			public uint MetadataLength
			{
				get
				{
					return this.metadataLength;
				}
				set
				{
					this.metadataLength = value;
				}
			}

			public uint SignaturePosition
			{
				get
				{
					return this.signaturePosition;
				}
				set
				{
					this.signaturePosition = value;
				}
			}

			public uint SignatureLength
			{
				get
				{
					return this.signatureLength;
				}
				set
				{
					this.signatureLength = value;
				}
			}

			public byte CliFlag
			{
				get
				{
					return this.cliFlag;
				}
				set
				{
					this.cliFlag = value;
				}
			}

			public uint CliFlagPosition
			{
				get
				{
					return this.cliFlagPosition;
				}
				set
				{
					this.cliFlagPosition = value;
				}
			}
		}

		internal enum StrongNameOptions
		{
			Metadata,
			Signature
		}
	}
}
