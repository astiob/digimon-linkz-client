using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Performs a cryptographic transformation of data. This class cannot be inherited.</summary>
	[ComVisible(true)]
	public sealed class CryptoAPITransform : IDisposable, ICryptoTransform
	{
		private bool m_disposed;

		internal CryptoAPITransform()
		{
			this.m_disposed = false;
		}

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Gets a value indicating whether the current transform can be reused.</summary>
		/// <returns>Always true.</returns>
		public bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value indicating whether multiple blocks can be transformed.</summary>
		/// <returns>true if multiple blocks can be transformed; otherwise, false.</returns>
		public bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets the input block size.</summary>
		/// <returns>The input block size in bytes.</returns>
		public int InputBlockSize
		{
			get
			{
				return 0;
			}
		}

		/// <summary>Gets the key handle.</summary>
		/// <returns>The key handle.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public IntPtr KeyHandle
		{
			get
			{
				return IntPtr.Zero;
			}
		}

		/// <summary>Gets the output block size.</summary>
		/// <returns>The output block size in bytes.</returns>
		public int OutputBlockSize
		{
			get
			{
				return 0;
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Security.Cryptography.CryptoAPITransform" /> method.</summary>
		public void Clear()
		{
			this.Dispose(false);
		}

		private void Dispose(bool disposing)
		{
			if (!this.m_disposed)
			{
				if (disposing)
				{
				}
				this.m_disposed = true;
			}
		}

		/// <summary>Computes the transformation for the specified region of the input byte array and copies the resulting transformation to the specified region of the output byte array.</summary>
		/// <returns>The number of bytes written.</returns>
		/// <param name="inputBuffer">The input on which to perform the operation on. </param>
		/// <param name="inputOffset">The offset into the input byte array from which to begin using data from. </param>
		/// <param name="inputCount">The number of bytes in the input byte array to use as data. </param>
		/// <param name="outputBuffer">The output to which to write the data to. </param>
		/// <param name="outputOffset">The offset into the output byte array from which to begin writing data from. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="inputBuffer" /> parameter is null.-or- The <paramref name="outputBuffer" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The length of the input buffer is less than the sum of the input offset and the input count. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="inputOffset" /> is out of range. This parameter requires a non-negative number.</exception>
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			return 0;
		}

		/// <summary>Computes the transformation for the specified region of the specified byte array.</summary>
		/// <returns>The computed transformation.</returns>
		/// <param name="inputBuffer">The input on which to perform the operation on. </param>
		/// <param name="inputOffset">The offset into the byte array from which to begin using data from. </param>
		/// <param name="inputCount">The number of bytes in the byte array to use as data. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="inputBuffer" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="inputOffset" /> parameter is less than zero.-or- The <paramref name="inputCount" /> parameter is less than zero.-or- The length of the input buffer is less than the sum of the input offset and the input count. </exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The <see cref="F:System.Security.Cryptography.PaddingMode.PKCS7" /> padding is invalid. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="inputOffset" /> parameter is out of range. This parameter requires a non-negative number.</exception>
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			return null;
		}

		/// <summary>Resets the internal state of <see cref="T:System.Security.Cryptography.CryptoAPITransform" /> so that it can be used again to do a different encryption or decryption.</summary>
		[ComVisible(false)]
		public void Reset()
		{
		}
	}
}
