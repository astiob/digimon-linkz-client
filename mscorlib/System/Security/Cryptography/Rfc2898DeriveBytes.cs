using Mono.Security.Cryptography;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Cryptography
{
	/// <summary>Implements password-based key derivation functionality, PBKDF2, by using a pseudo-random number generator based on <see cref="T:System.Security.Cryptography.HMACSHA1" />.</summary>
	[ComVisible(true)]
	public class Rfc2898DeriveBytes : DeriveBytes
	{
		private const int defaultIterations = 1000;

		private int _iteration;

		private byte[] _salt;

		private HMACSHA1 _hmac;

		private byte[] _buffer;

		private int _pos;

		private int _f;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes" /> class using a password and salt to derive the key.</summary>
		/// <param name="password">The password used to derive the key. </param>
		/// <param name="salt">The key salt used to derive the key. </param>
		/// <exception cref="T:System.ArgumentException">The specified salt size is smaller than 8 bytes or the iteration count is less than 1. </exception>
		/// <exception cref="T:System.ArgumentNullException">The password or salt is null. </exception>
		public Rfc2898DeriveBytes(string password, byte[] salt) : this(password, salt, 1000)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes" /> class using a password, a salt, and number of iterations to derive the key.</summary>
		/// <param name="password">The password used to derive the key. </param>
		/// <param name="salt">The key salt used to derive the key. </param>
		/// <param name="iterations">The number of iterations for the operation. </param>
		/// <exception cref="T:System.ArgumentException">The specified salt size is smaller than 8 bytes or the iteration count is less than 1. </exception>
		/// <exception cref="T:System.ArgumentNullException">The password or salt is null. </exception>
		public Rfc2898DeriveBytes(string password, byte[] salt, int iterations)
		{
			if (password == null)
			{
				throw new ArgumentNullException("password");
			}
			this.Salt = salt;
			this.IterationCount = iterations;
			this._hmac = new HMACSHA1(Encoding.UTF8.GetBytes(password));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes" /> class using a password, a salt, and number of iterations to derive the key.</summary>
		/// <param name="password">The password used to derive the key. </param>
		/// <param name="salt">The key salt used to derive the key.</param>
		/// <param name="iterations">The number of iterations for the operation. </param>
		/// <exception cref="T:System.ArgumentException">The specified salt size is smaller than 8 bytes or the iteration count is less than 1. </exception>
		/// <exception cref="T:System.ArgumentNullException">The password or salt is null. </exception>
		public Rfc2898DeriveBytes(byte[] password, byte[] salt, int iterations)
		{
			if (password == null)
			{
				throw new ArgumentNullException("password");
			}
			this.Salt = salt;
			this.IterationCount = iterations;
			this._hmac = new HMACSHA1(password);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes" /> class using the password and salt size to derive the key.</summary>
		/// <param name="password">The password used to derive the key. </param>
		/// <param name="saltSize">The size of the random salt that you want the class to generate. </param>
		/// <exception cref="T:System.ArgumentException">The specified salt size is smaller than 8 bytes. </exception>
		/// <exception cref="T:System.ArgumentNullException">The password or salt is null. </exception>
		public Rfc2898DeriveBytes(string password, int saltSize) : this(password, saltSize, 1000)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Rfc2898DeriveBytes" /> class using a password, a salt size, and number of iterations to derive the key.</summary>
		/// <param name="password">The password used to derive the key. </param>
		/// <param name="saltSize">The size of the random salt that you want the class to generate. </param>
		/// <param name="iterations">The number of iterations for the operation. </param>
		/// <exception cref="T:System.ArgumentException">The specified salt size is smaller than 8 bytes or the iteration count is less than 1. </exception>
		/// <exception cref="T:System.ArgumentNullException">The password or salt is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="iterations " />is out of range. This parameter requires a non-negative number.</exception>
		public Rfc2898DeriveBytes(string password, int saltSize, int iterations)
		{
			if (password == null)
			{
				throw new ArgumentNullException("password");
			}
			if (saltSize < 0)
			{
				throw new ArgumentOutOfRangeException("invalid salt length");
			}
			this.Salt = KeyBuilder.Key(saltSize);
			this.IterationCount = iterations;
			this._hmac = new HMACSHA1(Encoding.UTF8.GetBytes(password));
		}

		/// <summary>Gets or sets the number of iterations for the operation.</summary>
		/// <returns>The number of iterations for the operation.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The number of iterations is less than 1. </exception>
		public int IterationCount
		{
			get
			{
				return this._iteration;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("IterationCount < 1");
				}
				this._iteration = value;
			}
		}

		/// <summary>Gets or sets the key salt value for the operation.</summary>
		/// <returns>The key salt value for the operation.</returns>
		/// <exception cref="T:System.ArgumentException">The specified salt size is smaller than 8 bytes. </exception>
		/// <exception cref="T:System.ArgumentNullException">The salt is null. </exception>
		public byte[] Salt
		{
			get
			{
				return (byte[])this._salt.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Salt");
				}
				if (value.Length < 8)
				{
					throw new ArgumentException("Salt < 8 bytes");
				}
				this._salt = (byte[])value.Clone();
			}
		}

		private byte[] F(byte[] s, int c, int i)
		{
			s[s.Length - 4] = (byte)(i >> 24);
			s[s.Length - 3] = (byte)(i >> 16);
			s[s.Length - 2] = (byte)(i >> 8);
			s[s.Length - 1] = (byte)i;
			byte[] array = this._hmac.ComputeHash(s);
			byte[] buffer = array;
			for (int j = 1; j < c; j++)
			{
				byte[] array2 = this._hmac.ComputeHash(buffer);
				for (int k = 0; k < 20; k++)
				{
					array[k] ^= array2[k];
				}
				buffer = array2;
			}
			return array;
		}

		/// <summary>Returns the pseudo-random key for this object.</summary>
		/// <returns>A byte array filled with pseudo-random key bytes.</returns>
		/// <param name="cb">The number of pseudo-random key bytes to generate. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="cb " />is out of range. This parameter requires a non-negative number.</exception>
		public override byte[] GetBytes(int cb)
		{
			if (cb < 1)
			{
				throw new ArgumentOutOfRangeException("cb");
			}
			int num = cb / 20;
			int num2 = cb % 20;
			if (num2 != 0)
			{
				num++;
			}
			byte[] array = new byte[cb];
			int num3 = 0;
			if (this._pos > 0)
			{
				int num4 = Math.Min(20 - this._pos, cb);
				Buffer.BlockCopy(this._buffer, this._pos, array, 0, num4);
				if (num4 >= cb)
				{
					return array;
				}
				this._pos = 0;
				num3 = num4;
			}
			byte[] array2 = new byte[this._salt.Length + 4];
			Buffer.BlockCopy(this._salt, 0, array2, 0, this._salt.Length);
			for (int i = 1; i <= num; i++)
			{
				this._buffer = this.F(array2, this._iteration, ++this._f);
				int num5 = (i != num) ? 20 : (array.Length - num3);
				Buffer.BlockCopy(this._buffer, this._pos, array, num3, num5);
				num3 += this._pos + num5;
				this._pos = ((num5 != 20) ? num5 : 0);
			}
			return array;
		}

		/// <summary>Resets the state of the operation.</summary>
		public override void Reset()
		{
			this._buffer = null;
			this._pos = 0;
			this._f = 0;
		}
	}
}
