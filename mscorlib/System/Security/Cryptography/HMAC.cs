using Mono.Security.Cryptography;
using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Represents the abstract class from which all implementations of Hash-based Message Authentication Code (HMAC) must derive.</summary>
	[ComVisible(true)]
	public abstract class HMAC : KeyedHashAlgorithm
	{
		private bool _disposed;

		private string _hashName;

		private HashAlgorithm _algo;

		private BlockProcessor _block;

		private int _blockSizeValue;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.HMAC" /> class. </summary>
		protected HMAC()
		{
			this._disposed = false;
			this._blockSizeValue = 64;
		}

		/// <summary>Gets or sets the block size to use in the hash value.</summary>
		/// <returns>The block size to use in the hash value.</returns>
		protected int BlockSizeValue
		{
			get
			{
				return this._blockSizeValue;
			}
			set
			{
				this._blockSizeValue = value;
			}
		}

		/// <summary>Gets or sets the name of the hash algorithm to use for hashing.</summary>
		/// <returns>The name of the hash algorithm.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The current hash algorithm cannot be changed.</exception>
		public string HashName
		{
			get
			{
				return this._hashName;
			}
			set
			{
				this._hashName = value;
				this._algo = HashAlgorithm.Create(this._hashName);
			}
		}

		/// <summary>Gets or sets the key to use in the hash algorithm.</summary>
		/// <returns>The key to use in the hash algorithm.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">An attempt is made to change the <see cref="P:System.Security.Cryptography.HMAC.Key" /> property after hashing has begun. </exception>
		public override byte[] Key
		{
			get
			{
				return (byte[])base.Key.Clone();
			}
			set
			{
				if (value != null && value.Length > 64)
				{
					base.Key = this._algo.ComputeHash(value);
				}
				else
				{
					base.Key = (byte[])value.Clone();
				}
			}
		}

		internal BlockProcessor Block
		{
			get
			{
				if (this._block == null)
				{
					this._block = new BlockProcessor(this._algo, this.BlockSizeValue >> 3);
				}
				return this._block;
			}
		}

		private byte[] KeySetup(byte[] key, byte padding)
		{
			byte[] array = new byte[this.BlockSizeValue];
			for (int i = 0; i < key.Length; i++)
			{
				array[i] = (key[i] ^ padding);
			}
			for (int j = key.Length; j < this.BlockSizeValue; j++)
			{
				array[j] = padding;
			}
			return array;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Security.Cryptography.HMAC" /> class when a key change is legitimate and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected override void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				base.Dispose(disposing);
			}
		}

		/// <summary>When overridden in a derived class, routes data written to the object into the default <see cref="T:System.Security.Cryptography.HMAC" /> hash algorithm for computing the hash value.</summary>
		/// <param name="rgb">The input data. </param>
		/// <param name="ib">The offset into the byte array from which to begin using data. </param>
		/// <param name="cb">The number of bytes in the array to use as data. </param>
		protected override void HashCore(byte[] rgb, int ib, int cb)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("HMACSHA1");
			}
			if (this.State == 0)
			{
				this.Initialize();
				this.State = 1;
			}
			this.Block.Core(rgb, ib, cb);
		}

		/// <summary>When overridden in a derived class, finalizes the hash computation after the last data is processed by the cryptographic stream object.</summary>
		/// <returns>The computed hash code in a byte array.</returns>
		protected override byte[] HashFinal()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("HMAC");
			}
			this.State = 0;
			this.Block.Final();
			byte[] hash = this._algo.Hash;
			byte[] array = this.KeySetup(this.Key, 92);
			this._algo.Initialize();
			this._algo.TransformBlock(array, 0, array.Length, array, 0);
			this._algo.TransformFinalBlock(hash, 0, hash.Length);
			byte[] hash2 = this._algo.Hash;
			this._algo.Initialize();
			Array.Clear(array, 0, array.Length);
			Array.Clear(hash, 0, hash.Length);
			return hash2;
		}

		/// <summary>Initializes an instance of the default implementation of <see cref="T:System.Security.Cryptography.HMAC" />.</summary>
		public override void Initialize()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("HMAC");
			}
			this.State = 0;
			this.Block.Initialize();
			byte[] array = this.KeySetup(this.Key, 54);
			this._algo.Initialize();
			this.Block.Core(array);
			Array.Clear(array, 0, array.Length);
		}

		/// <summary>Creates an instance of the default implementation of a Hash-based Message Authentication Code (HMAC).</summary>
		/// <returns>A new SHA-1 instance, unless the default settings have been changed by using the &lt;cryptoClass&gt; element.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public new static HMAC Create()
		{
			return HMAC.Create("System.Security.Cryptography.HMAC");
		}

		/// <summary>Creates an instance of the specified implementation of a Hash-based Message Authentication Code (HMAC).</summary>
		/// <returns>A new instance of the specified HMAC implementation.</returns>
		/// <param name="algorithmName">The HMAC implementation to use. The following table shows the valid values for the <paramref name="algorithmName" /> parameter and the algorithms they map to.Parameter valueImplements System.Security.Cryptography.HMAC<see cref="T:System.Security.Cryptography.HMACSHA1" />System.Security.Cryptography.KeyedHashAlgorithm<see cref="T:System.Security.Cryptography.HMACSHA1" />HMACMD5<see cref="T:System.Security.Cryptography.HMACMD5" />System.Security.Cryptography.HMACMD5<see cref="T:System.Security.Cryptography.HMACMD5" />HMACRIPEMD160<see cref="T:System.Security.Cryptography.HMACRIPEMD160" />System.Security.Cryptography.HMACRIPEMD160<see cref="T:System.Security.Cryptography. HMACRIPEMD160" />HMACSHA1<see cref="T:System.Security.Cryptography.HMACSHA1" />System.Security.Cryptography.HMACSHA1<see cref="T:System.Security.Cryptography. HMACSHA1" />HMACSHA256<see cref="T:System.Security.Cryptography.HMACSHA256" />System.Security.Cryptography.HMACSHA256<see cref="T:System.Security.Cryptography.HMACSHA256" />HMACSHA384<see cref="T:System.Security.Cryptography.HMACSHA384" />System.Security.Cryptography.HMACSHA384<see cref="T:System.Security.Cryptography.HMACSHA384" />HMACSHA512<see cref="T:System.Security.Cryptography.HMACSHA512" />System.Security.Cryptography.HMACSHA512<see cref="T:System.Security.Cryptography.HMACSHA512" />MACTripleDES<see cref="T:System.Security.Cryptography. MACTripleDES" />System.Security.Cryptography.MACTripleDES<see cref="T:System.Security.Cryptography.MACTripleDES" /></param>
		public new static HMAC Create(string algorithmName)
		{
			return (HMAC)CryptoConfig.CreateFromName(algorithmName);
		}
	}
}
