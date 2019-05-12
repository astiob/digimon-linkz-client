using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Performs a cryptographic transformation of data using the Rijndael algorithm. This class cannot be inherited.</summary>
	[ComVisible(true)]
	public sealed class RijndaelManagedTransform : IDisposable, ICryptoTransform
	{
		private RijndaelTransform _st;

		private int _bs;

		internal RijndaelManagedTransform(Rijndael algo, bool encryption, byte[] key, byte[] iv)
		{
			this._st = new RijndaelTransform(algo, encryption, key, iv);
			this._bs = algo.BlockSize;
		}

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		void IDisposable.Dispose()
		{
			this._st.Clear();
		}

		/// <summary>Gets the block size.</summary>
		/// <returns>The size of the data blocks in bytes.</returns>
		public int BlockSizeValue
		{
			get
			{
				return this._bs;
			}
		}

		/// <summary>Gets a value indicating whether multiple blocks can be transformed.</summary>
		/// <returns>true if multiple blocks can be transformed; otherwise, false.</returns>
		public bool CanTransformMultipleBlocks
		{
			get
			{
				return this._st.CanTransformMultipleBlocks;
			}
		}

		/// <summary>Gets a value indicating whether the current transform can be reused.</summary>
		/// <returns>Always true.</returns>
		public bool CanReuseTransform
		{
			get
			{
				return this._st.CanReuseTransform;
			}
		}

		/// <summary>Gets the input block size.</summary>
		/// <returns>The size of the input data blocks in bytes.</returns>
		public int InputBlockSize
		{
			get
			{
				return this._st.InputBlockSize;
			}
		}

		/// <summary>Gets the output block size.</summary>
		/// <returns>The size of the output data blocks in bytes.</returns>
		public int OutputBlockSize
		{
			get
			{
				return this._st.OutputBlockSize;
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Security.Cryptography.RijndaelManagedTransform" /> class.</summary>
		public void Clear()
		{
			this._st.Clear();
		}

		/// <summary>Resets the internal state of <see cref="T:System.Security.Cryptography.RijndaelManagedTransform" /> so it can be used again to do a different encryption or decryption.</summary>
		[MonoTODO("Reset does nothing since CanReuseTransform return false.")]
		public void Reset()
		{
		}

		/// <summary>Computes the transformation for the specified region of the input byte array and copies the resulting transformation to the specified region of the output byte array.</summary>
		/// <returns>The number of bytes written.</returns>
		/// <param name="inputBuffer">The input to perform the operation on. </param>
		/// <param name="inputOffset">The offset into the input byte array to begin using data from. </param>
		/// <param name="inputCount">The number of bytes in the input byte array to use as data. </param>
		/// <param name="outputBuffer">The output to write the data to. </param>
		/// <param name="outputOffset">The offset into the output byte array to begin writing data from. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="inputBuffer" /> parameter is null.-or- The <paramref name="outputBuffer" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The length of the input buffer is less than the sum of the input offset and the input count. -or-The value of the <paramref name="inputCount" /> parameter is less than or equal to 0.-or-The value of the <paramref name="inputCount" /> parameter is greater than the length of the <paramref name="inputBuffer" /> parameter.-or-The length of the <paramref name="inputCount" /> parameter is not evenly devisable by input block size. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <paramref name="inputOffset" /> parameter is negative.</exception>
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			return this._st.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
		}

		/// <summary>Computes the transformation for the specified region of the specified byte array.</summary>
		/// <returns>The computed transformation.</returns>
		/// <param name="inputBuffer">The input to perform the operation on.</param>
		/// <param name="inputOffset">The offset into the byte array to begin using data from.</param>
		/// <param name="inputCount">The number of bytes in the byte array to use as data.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="inputBuffer" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The value of the <paramref name="inputCount" /> parameter is less than 0.-or-The value of the <paramref name="inputCount" /> parameter is grater than the length of <paramref name="inputBuffer" /> parameter.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <paramref name="inputOffset" /> parameter is negative.</exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The length of the <paramref name="inputCount" /> parameter is not evenly devisable by input block size.</exception>
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			return this._st.TransformFinalBlock(inputBuffer, inputOffset, inputCount);
		}
	}
}
