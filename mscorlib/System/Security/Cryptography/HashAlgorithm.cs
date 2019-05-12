using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Represents the base class from which all implementations of cryptographic hash algorithms must derive.</summary>
	[ComVisible(true)]
	public abstract class HashAlgorithm : IDisposable, ICryptoTransform
	{
		/// <summary>Represents the value of the computed hash code.</summary>
		protected internal byte[] HashValue;

		/// <summary>Represents the size, in bits, of the computed hash code.</summary>
		protected int HashSizeValue;

		/// <summary>Represents the state of the hash computation.</summary>
		protected int State;

		private bool disposed;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.HashAlgorithm" /> class.</summary>
		protected HashAlgorithm()
		{
			this.disposed = false;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Security.Cryptography.HashAlgorithm" /> and optionally releases the managed resources.</summary>
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether multiple blocks can be transformed.</summary>
		/// <returns>true if multiple blocks can be transformed; otherwise, false.</returns>
		public virtual bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value indicating whether the current transform can be reused.</summary>
		/// <returns>Always true.</returns>
		public virtual bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Security.Cryptography.HashAlgorithm" /> class.</summary>
		public void Clear()
		{
			this.Dispose(true);
		}

		/// <summary>Computes the hash value for the specified byte array.</summary>
		/// <returns>The computed hash code.</returns>
		/// <param name="buffer">The input to compute the hash code for. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
		public byte[] ComputeHash(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			return this.ComputeHash(buffer, 0, buffer.Length);
		}

		/// <summary>Computes the hash value for the specified region of the specified byte array.</summary>
		/// <returns>The computed hash code.</returns>
		/// <param name="buffer">The input to compute the hash code for. </param>
		/// <param name="offset">The offset into the byte array from which to begin using data. </param>
		/// <param name="count">The number of bytes in the array to use as data. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="count" /> is an invalid value.-or-<paramref name="buffer" /> length is invalid.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is out of range. This parameter requires a non-negative number.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
		public byte[] ComputeHash(byte[] buffer, int offset, int count)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("HashAlgorithm");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "< 0");
			}
			if (count < 0)
			{
				throw new ArgumentException("count", "< 0");
			}
			if (offset > buffer.Length - count)
			{
				throw new ArgumentException("offset + count", Locale.GetText("Overflow"));
			}
			this.HashCore(buffer, offset, count);
			this.HashValue = this.HashFinal();
			this.Initialize();
			return this.HashValue;
		}

		/// <summary>Computes the hash value for the specified <see cref="T:System.IO.Stream" /> object.</summary>
		/// <returns>The computed hash code.</returns>
		/// <param name="inputStream">The input to compute the hash code for. </param>
		/// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
		public byte[] ComputeHash(Stream inputStream)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("HashAlgorithm");
			}
			byte[] array = new byte[4096];
			for (int i = inputStream.Read(array, 0, 4096); i > 0; i = inputStream.Read(array, 0, 4096))
			{
				this.HashCore(array, 0, i);
			}
			this.HashValue = this.HashFinal();
			this.Initialize();
			return this.HashValue;
		}

		/// <summary>Creates an instance of the default implementation of a hash algorithm.</summary>
		/// <returns>A new <see cref="T:System.Security.Cryptography.SHA1CryptoServiceProvider" /> instance, unless the default settings have been changed using the &lt;cryptoClass&gt; element.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static HashAlgorithm Create()
		{
			return HashAlgorithm.Create("System.Security.Cryptography.HashAlgorithm");
		}

		/// <summary>Creates an instance of the specified implementation of a hash algorithm.</summary>
		/// <returns>A new instance of the specified hash algorithm, or null if <paramref name="hashName" /> is not a valid hash algorithm.</returns>
		/// <param name="hashName">The hash algorithm implementation to use. The following table shows the valid values for the <paramref name="hashName" /> parameter and the algorithms they map to. Parameter value Implements SHA <see cref="T:System.Security.Cryptography.SHA1CryptoServiceProvider" />SHA1 <see cref="T:System.Security.Cryptography.SHA1CryptoServiceProvider" />System.Security.Cryptography.SHA1 <see cref="T:System.Security.Cryptography.SHA1CryptoServiceProvider" />System.Security.Cryptography.HashAlgorithm <see cref="T:System.Security.Cryptography.SHA1CryptoServiceProvider" />MD5 <see cref="T:System.Security.Cryptography.MD5CryptoServiceProvider" />System.Security.Cryptography.MD5 <see cref="T:System.Security.Cryptography.MD5CryptoServiceProvider" />SHA256 <see cref="T:System.Security.Cryptography.SHA256Managed" />SHA-256 <see cref="T:System.Security.Cryptography.SHA256Managed" />System.Security.Cryptography.SHA256 <see cref="T:System.Security.Cryptography.SHA256Managed" />SHA384 <see cref="T:System.Security.Cryptography.SHA384Managed" />SHA-384 <see cref="T:System.Security.Cryptography.SHA384Managed" />System.Security.Cryptography.SHA384 <see cref="T:System.Security.Cryptography.SHA384Managed" />SHA512 <see cref="T:System.Security.Cryptography.SHA512Managed" />SHA-512 <see cref="T:System.Security.Cryptography.SHA512Managed" />System.Security.Cryptography.SHA512 <see cref="T:System.Security.Cryptography.SHA512Managed" /></param>
		public static HashAlgorithm Create(string hashName)
		{
			return (HashAlgorithm)CryptoConfig.CreateFromName(hashName);
		}

		/// <summary>Gets the value of the computed hash code.</summary>
		/// <returns>The current value of the computed hash code.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicUnexpectedOperationException">
		///   <see cref="F:System.Security.Cryptography.HashAlgorithm.HashValue" /> is null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
		public virtual byte[] Hash
		{
			get
			{
				if (this.HashValue == null)
				{
					throw new CryptographicUnexpectedOperationException(Locale.GetText("No hash value computed."));
				}
				return this.HashValue;
			}
		}

		/// <summary>When overridden in a derived class, routes data written to the object into the hash algorithm for computing the hash.</summary>
		/// <param name="array">The input to compute the hash code for. </param>
		/// <param name="ibStart">The offset into the byte array from which to begin using data. </param>
		/// <param name="cbSize">The number of bytes in the byte array to use as data. </param>
		protected abstract void HashCore(byte[] array, int ibStart, int cbSize);

		/// <summary>When overridden in a derived class, finalizes the hash computation after the last data is processed by the cryptographic stream object.</summary>
		/// <returns>The computed hash code.</returns>
		protected abstract byte[] HashFinal();

		/// <summary>Gets the size, in bits, of the computed hash code.</summary>
		/// <returns>The size, in bits, of the computed hash code.</returns>
		public virtual int HashSize
		{
			get
			{
				return this.HashSizeValue;
			}
		}

		/// <summary>Initializes an implementation of the <see cref="T:System.Security.Cryptography.HashAlgorithm" /> class.</summary>
		public abstract void Initialize();

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Security.Cryptography.HashAlgorithm" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected virtual void Dispose(bool disposing)
		{
			this.disposed = true;
		}

		/// <summary>When overridden in a derived class, gets the input block size.</summary>
		/// <returns>The input block size.</returns>
		public virtual int InputBlockSize
		{
			get
			{
				return 1;
			}
		}

		/// <summary>When overridden in a derived class, gets the output block size.</summary>
		/// <returns>The output block size.</returns>
		public virtual int OutputBlockSize
		{
			get
			{
				return 1;
			}
		}

		/// <summary>Computes the hash value for the specified region of the input byte array and copies the specified region of the input byte array to the specified region of the output byte array.</summary>
		/// <returns>The number of bytes written.</returns>
		/// <param name="inputBuffer">The input to compute the hash code for. </param>
		/// <param name="inputOffset">The offset into the input byte array from which to begin using data. </param>
		/// <param name="inputCount">The number of bytes in the input byte array to use as data. </param>
		/// <param name="outputBuffer">A copy of the part of the input array used to compute the hash code. </param>
		/// <param name="outputOffset">The offset into the output byte array from which to begin writing data. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="inputCount" /> uses an invalid value.-or-<paramref name="inputBuffer" /> has an invalid length.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="inputBuffer" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="inputOffset" /> is out of range. This parameter requires a non-negative number.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset", "< 0");
			}
			if (inputCount < 0)
			{
				throw new ArgumentException("inputCount");
			}
			if (inputOffset < 0 || inputOffset > inputBuffer.Length - inputCount)
			{
				throw new ArgumentException("inputBuffer");
			}
			if (outputBuffer != null)
			{
				if (outputOffset < 0)
				{
					throw new ArgumentOutOfRangeException("outputOffset", "< 0");
				}
				if (outputOffset > outputBuffer.Length - inputCount)
				{
					throw new ArgumentException("outputOffset + inputCount", Locale.GetText("Overflow"));
				}
			}
			this.HashCore(inputBuffer, inputOffset, inputCount);
			if (outputBuffer != null)
			{
				Buffer.BlockCopy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
			}
			return inputCount;
		}

		/// <summary>Computes the hash value for the specified region of the specified byte array.</summary>
		/// <returns>An array that is a copy of the part of the input that is hashed.</returns>
		/// <param name="inputBuffer">The input to compute the hash code for. </param>
		/// <param name="inputOffset">The offset into the byte array from which to begin using data. </param>
		/// <param name="inputCount">The number of bytes in the byte array to use as data. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="inputCount" /> uses an invalid value.-or-<paramref name="inputBuffer" /> has an invalid offset length.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="inputBuffer" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="inputOffset" /> is out of range. This parameter requires a non-negative number.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (inputCount < 0)
			{
				throw new ArgumentException("inputCount");
			}
			if (inputOffset > inputBuffer.Length - inputCount)
			{
				throw new ArgumentException("inputOffset + inputCount", Locale.GetText("Overflow"));
			}
			byte[] array = new byte[inputCount];
			Buffer.BlockCopy(inputBuffer, inputOffset, array, 0, inputCount);
			this.HashCore(inputBuffer, inputOffset, inputCount);
			this.HashValue = this.HashFinal();
			this.Initialize();
			return array;
		}
	}
}
