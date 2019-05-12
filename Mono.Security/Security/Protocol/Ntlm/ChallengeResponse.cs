using Mono.Security.Cryptography;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Mono.Security.Protocol.Ntlm
{
	public class ChallengeResponse : IDisposable
	{
		private static byte[] magic = new byte[]
		{
			75,
			71,
			83,
			33,
			64,
			35,
			36,
			37
		};

		private static byte[] nullEncMagic = new byte[]
		{
			170,
			211,
			180,
			53,
			181,
			20,
			4,
			238
		};

		private bool _disposed;

		private byte[] _challenge;

		private byte[] _lmpwd;

		private byte[] _ntpwd;

		public ChallengeResponse()
		{
			this._disposed = false;
			this._lmpwd = new byte[21];
			this._ntpwd = new byte[21];
		}

		public ChallengeResponse(string password, byte[] challenge) : this()
		{
			this.Password = password;
			this.Challenge = challenge;
		}

		~ChallengeResponse()
		{
			if (!this._disposed)
			{
				this.Dispose();
			}
		}

		public string Password
		{
			get
			{
				return null;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("too late");
				}
				DES des = DES.Create();
				des.Mode = CipherMode.ECB;
				if (value == null || value.Length < 1)
				{
					Buffer.BlockCopy(ChallengeResponse.nullEncMagic, 0, this._lmpwd, 0, 8);
				}
				else
				{
					des.Key = this.PasswordToKey(value, 0);
					ICryptoTransform cryptoTransform = des.CreateEncryptor();
					cryptoTransform.TransformBlock(ChallengeResponse.magic, 0, 8, this._lmpwd, 0);
				}
				if (value == null || value.Length < 8)
				{
					Buffer.BlockCopy(ChallengeResponse.nullEncMagic, 0, this._lmpwd, 8, 8);
				}
				else
				{
					des.Key = this.PasswordToKey(value, 7);
					ICryptoTransform cryptoTransform = des.CreateEncryptor();
					cryptoTransform.TransformBlock(ChallengeResponse.magic, 0, 8, this._lmpwd, 8);
				}
				MD4 md = MD4.Create();
				byte[] array = (value != null) ? Encoding.Unicode.GetBytes(value) : new byte[0];
				byte[] array2 = md.ComputeHash(array);
				Buffer.BlockCopy(array2, 0, this._ntpwd, 0, 16);
				Array.Clear(array, 0, array.Length);
				Array.Clear(array2, 0, array2.Length);
				des.Clear();
			}
		}

		public byte[] Challenge
		{
			get
			{
				return null;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Challenge");
				}
				if (this._disposed)
				{
					throw new ObjectDisposedException("too late");
				}
				this._challenge = (byte[])value.Clone();
			}
		}

		public byte[] LM
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("too late");
				}
				return this.GetResponse(this._lmpwd);
			}
		}

		public byte[] NT
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("too late");
				}
				return this.GetResponse(this._ntpwd);
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				Array.Clear(this._lmpwd, 0, this._lmpwd.Length);
				Array.Clear(this._ntpwd, 0, this._ntpwd.Length);
				if (this._challenge != null)
				{
					Array.Clear(this._challenge, 0, this._challenge.Length);
				}
				this._disposed = true;
			}
		}

		private byte[] GetResponse(byte[] pwd)
		{
			byte[] array = new byte[24];
			DES des = DES.Create();
			des.Mode = CipherMode.ECB;
			des.Key = this.PrepareDESKey(pwd, 0);
			ICryptoTransform cryptoTransform = des.CreateEncryptor();
			cryptoTransform.TransformBlock(this._challenge, 0, 8, array, 0);
			des.Key = this.PrepareDESKey(pwd, 7);
			cryptoTransform = des.CreateEncryptor();
			cryptoTransform.TransformBlock(this._challenge, 0, 8, array, 8);
			des.Key = this.PrepareDESKey(pwd, 14);
			cryptoTransform = des.CreateEncryptor();
			cryptoTransform.TransformBlock(this._challenge, 0, 8, array, 16);
			return array;
		}

		private byte[] PrepareDESKey(byte[] key56bits, int position)
		{
			return new byte[]
			{
				key56bits[position],
				(byte)((int)key56bits[position] << 7 | key56bits[position + 1] >> 1),
				(byte)((int)key56bits[position + 1] << 6 | key56bits[position + 2] >> 2),
				(byte)((int)key56bits[position + 2] << 5 | key56bits[position + 3] >> 3),
				(byte)((int)key56bits[position + 3] << 4 | key56bits[position + 4] >> 4),
				(byte)((int)key56bits[position + 4] << 3 | key56bits[position + 5] >> 5),
				(byte)((int)key56bits[position + 5] << 2 | key56bits[position + 6] >> 6),
				(byte)(key56bits[position + 6] << 1)
			};
		}

		private byte[] PasswordToKey(string password, int position)
		{
			byte[] array = new byte[7];
			int charCount = Math.Min(password.Length - position, 7);
			Encoding.ASCII.GetBytes(password.ToUpper(CultureInfo.CurrentCulture), position, charCount, array, 0);
			byte[] result = this.PrepareDESKey(array, 0);
			Array.Clear(array, 0, array.Length);
			return result;
		}
	}
}
