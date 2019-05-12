using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Defines a stream that links data streams to cryptographic transformations.</summary>
	[ComVisible(true)]
	public class CryptoStream : Stream
	{
		private Stream _stream;

		private ICryptoTransform _transform;

		private CryptoStreamMode _mode;

		private byte[] _currentBlock;

		private bool _disposed;

		private bool _flushedFinalBlock;

		private int _partialCount;

		private bool _endOfStream;

		private byte[] _waitingBlock;

		private int _waitingCount;

		private byte[] _transformedBlock;

		private int _transformedPos;

		private int _transformedCount;

		private byte[] _workingBlock;

		private int _workingCount;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.CryptoStream" /> class with a target data stream, the transformation to use, and the mode of the stream.</summary>
		/// <param name="stream">The stream on which to perform the cryptographic transformation. </param>
		/// <param name="transform">The cryptographic transformation that is to be performed on the stream. </param>
		/// <param name="mode">One of the <see cref="T:System.Security.Cryptography.CryptoStreamMode" /> values. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="stream" /> is not readable.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="stream" /> is not writable.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="stream" /> is invalid.</exception>
		public CryptoStream(Stream stream, ICryptoTransform transform, CryptoStreamMode mode)
		{
			if (mode == CryptoStreamMode.Read && !stream.CanRead)
			{
				throw new ArgumentException(Locale.GetText("Can't read on stream"));
			}
			if (mode == CryptoStreamMode.Write && !stream.CanWrite)
			{
				throw new ArgumentException(Locale.GetText("Can't write on stream"));
			}
			this._stream = stream;
			this._transform = transform;
			this._mode = mode;
			this._disposed = false;
			if (transform != null)
			{
				if (mode == CryptoStreamMode.Read)
				{
					this._currentBlock = new byte[transform.InputBlockSize];
					this._workingBlock = new byte[transform.InputBlockSize];
				}
				else if (mode == CryptoStreamMode.Write)
				{
					this._currentBlock = new byte[transform.OutputBlockSize];
					this._workingBlock = new byte[transform.OutputBlockSize];
				}
			}
		}

		~CryptoStream()
		{
			this.Dispose(false);
		}

		/// <summary>Gets a value indicating whether the current <see cref="T:System.Security.Cryptography.CryptoStream" /> is readable.</summary>
		/// <returns>true if the current stream is readable; otherwise, false.</returns>
		public override bool CanRead
		{
			get
			{
				return this._mode == CryptoStreamMode.Read;
			}
		}

		/// <summary>Gets a value indicating whether you can seek within the current <see cref="T:System.Security.Cryptography.CryptoStream" />.</summary>
		/// <returns>Always false.</returns>
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the current <see cref="T:System.Security.Cryptography.CryptoStream" /> is writable.</summary>
		/// <returns>true if the current stream is writable; otherwise, false.</returns>
		public override bool CanWrite
		{
			get
			{
				return this._mode == CryptoStreamMode.Write;
			}
		}

		/// <summary>Gets the length in bytes of the stream.</summary>
		/// <returns>This property is not supported.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is not supported. </exception>
		public override long Length
		{
			get
			{
				throw new NotSupportedException("Length");
			}
		}

		/// <summary>Gets or sets the position within the current stream.</summary>
		/// <returns>This property is not supported.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is not supported. </exception>
		public override long Position
		{
			get
			{
				throw new NotSupportedException("Position");
			}
			set
			{
				throw new NotSupportedException("Position");
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Security.Cryptography.CryptoStream" />.</summary>
		public void Clear()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Closes the current stream and releases any resources (such as sockets and file handles) associated with the current stream.</summary>
		/// <exception cref="T:System.NotSupportedException">The current stream is not writable. </exception>
		public override void Close()
		{
			if (!this._flushedFinalBlock && this._mode == CryptoStreamMode.Write)
			{
				this.FlushFinalBlock();
			}
			if (this._stream != null)
			{
				this._stream.Close();
			}
		}

		/// <summary>Reads a sequence of bytes from the current <see cref="T:System.Security.Cryptography.CryptoStream" /> and advances the position within the stream by the number of bytes read.</summary>
		/// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero if the end of the stream has been reached.</returns>
		/// <param name="buffer">An array of bytes. A maximum of <paramref name="count" /> bytes are read from the current stream and stored in <paramref name="buffer" />. </param>
		/// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream. </param>
		/// <param name="count">The maximum number of bytes to be read from the current stream. </param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Security.Cryptography.CryptoStreamMode" /> associated with current <see cref="T:System.Security.Cryptography.CryptoStream" /> object does not match the underlying stream.  For example, this exception is thrown when using <see cref="F:System.Security.Cryptography.CryptoStreamMode.Read" /> with an underlying stream that is write only.  </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="offset" /> parameter is less than zero.-or- The <paramref name="count" /> parameter is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">Thesum of the <paramref name="count" /> and <paramref name="offset" /> parameters is longer than the length of the buffer. </exception>
		public override int Read([In] [Out] byte[] buffer, int offset, int count)
		{
			if (this._mode != CryptoStreamMode.Read)
			{
				throw new NotSupportedException(Locale.GetText("not in Read mode"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Locale.GetText("negative"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Locale.GetText("negative"));
			}
			if (offset > buffer.Length - count)
			{
				throw new ArgumentException("(offset+count)", Locale.GetText("buffer overflow"));
			}
			if (this._workingBlock == null)
			{
				return 0;
			}
			int num = 0;
			if (count == 0 || (this._transformedPos == this._transformedCount && this._endOfStream))
			{
				return num;
			}
			if (this._waitingBlock == null)
			{
				this._transformedBlock = new byte[this._transform.OutputBlockSize << 2];
				this._transformedPos = 0;
				this._transformedCount = 0;
				this._waitingBlock = new byte[this._transform.InputBlockSize];
				this._waitingCount = this._stream.Read(this._waitingBlock, 0, this._waitingBlock.Length);
			}
			while (count > 0)
			{
				int num2 = this._transformedCount - this._transformedPos;
				if (num2 < this._transform.InputBlockSize)
				{
					int num3 = 0;
					this._workingCount = this._stream.Read(this._workingBlock, 0, this._transform.InputBlockSize);
					this._endOfStream = (this._workingCount < this._transform.InputBlockSize);
					if (!this._endOfStream)
					{
						num3 = this._transform.TransformBlock(this._waitingBlock, 0, this._waitingBlock.Length, this._transformedBlock, this._transformedCount);
						Buffer.BlockCopy(this._workingBlock, 0, this._waitingBlock, 0, this._workingCount);
						this._waitingCount = this._workingCount;
					}
					else
					{
						if (this._workingCount > 0)
						{
							num3 = this._transform.TransformBlock(this._waitingBlock, 0, this._waitingBlock.Length, this._transformedBlock, this._transformedCount);
							Buffer.BlockCopy(this._workingBlock, 0, this._waitingBlock, 0, this._workingCount);
							this._waitingCount = this._workingCount;
							num2 += num3;
							this._transformedCount += num3;
						}
						if (!this._flushedFinalBlock)
						{
							byte[] array = this._transform.TransformFinalBlock(this._waitingBlock, 0, this._waitingCount);
							num3 = array.Length;
							Buffer.BlockCopy(array, 0, this._transformedBlock, this._transformedCount, array.Length);
							Array.Clear(array, 0, array.Length);
							this._flushedFinalBlock = true;
						}
					}
					num2 += num3;
					this._transformedCount += num3;
				}
				if (this._transformedPos > this._transform.OutputBlockSize)
				{
					Buffer.BlockCopy(this._transformedBlock, this._transformedPos, this._transformedBlock, 0, num2);
					this._transformedCount -= this._transformedPos;
					this._transformedPos = 0;
				}
				num2 = ((count >= num2) ? num2 : count);
				if (num2 > 0)
				{
					Buffer.BlockCopy(this._transformedBlock, this._transformedPos, buffer, offset, num2);
					this._transformedPos += num2;
					num += num2;
					offset += num2;
					count -= num2;
				}
				if ((num2 != this._transform.InputBlockSize && this._waitingCount != this._transform.InputBlockSize) || this._endOfStream)
				{
					count = 0;
				}
			}
			return num;
		}

		/// <summary>Writes a sequence of bytes to the current <see cref="T:System.Security.Cryptography.CryptoStream" /> and advances the current position within this stream by the number of bytes written.</summary>
		/// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream. </param>
		/// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream. </param>
		/// <param name="count">The number of bytes to be written to the current stream. </param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Security.Cryptography.CryptoStreamMode" /> associated with current <see cref="T:System.Security.Cryptography.CryptoStream" /> object does not match the underlying stream.  For example, this exception is thrown when using <see cref="F:System.Security.Cryptography.CryptoStreamMode.Write" />  with an underlying stream that is read only.  </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="offset" /> parameter is less than zero.-or- The <paramref name="count" /> parameter is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">The sum of the <paramref name="count" /> and <paramref name="offset" /> parameters is longer than the length of the buffer. </exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._mode != CryptoStreamMode.Write)
			{
				throw new NotSupportedException(Locale.GetText("not in Write mode"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Locale.GetText("negative"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Locale.GetText("negative"));
			}
			if (offset > buffer.Length - count)
			{
				throw new ArgumentException("(offset+count)", Locale.GetText("buffer overflow"));
			}
			if (this._stream == null)
			{
				throw new ArgumentNullException("inner stream was diposed");
			}
			int num = count;
			if (this._partialCount > 0 && this._partialCount != this._transform.InputBlockSize)
			{
				int num2 = this._transform.InputBlockSize - this._partialCount;
				num2 = ((count >= num2) ? num2 : count);
				Buffer.BlockCopy(buffer, offset, this._workingBlock, this._partialCount, num2);
				this._partialCount += num2;
				offset += num2;
				count -= num2;
			}
			int num3 = offset;
			while (count > 0)
			{
				if (this._partialCount == this._transform.InputBlockSize)
				{
					int count2 = this._transform.TransformBlock(this._workingBlock, 0, this._partialCount, this._currentBlock, 0);
					this._stream.Write(this._currentBlock, 0, count2);
					this._partialCount = 0;
				}
				if (this._transform.CanTransformMultipleBlocks)
				{
					int num4 = count & ~(this._transform.InputBlockSize - 1);
					int num5 = count & this._transform.InputBlockSize - 1;
					int num6 = (1 + num4 / this._transform.InputBlockSize) * this._transform.OutputBlockSize;
					if (this._workingBlock.Length < num6)
					{
						Array.Clear(this._workingBlock, 0, this._workingBlock.Length);
						this._workingBlock = new byte[num6];
					}
					if (num4 > 0)
					{
						int count3 = this._transform.TransformBlock(buffer, offset, num4, this._workingBlock, 0);
						this._stream.Write(this._workingBlock, 0, count3);
					}
					if (num5 > 0)
					{
						Buffer.BlockCopy(buffer, num - num5, this._workingBlock, 0, num5);
					}
					this._partialCount = num5;
					count = 0;
				}
				else
				{
					int num7 = Math.Min(this._transform.InputBlockSize - this._partialCount, count);
					Buffer.BlockCopy(buffer, num3, this._workingBlock, this._partialCount, num7);
					num3 += num7;
					this._partialCount += num7;
					count -= num7;
				}
			}
		}

		/// <summary>Clears all buffers for this stream and causes any buffered data to be written to the underlying device.</summary>
		public override void Flush()
		{
			if (this._stream != null)
			{
				this._stream.Flush();
			}
		}

		/// <summary>Updates the underlying data source or repository with the current state of the buffer, then clears the buffer.</summary>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The key is corrupt which can cause invalid padding to the stream. </exception>
		/// <exception cref="T:System.NotSupportedException">The current stream is not writable.-or- The final block has already been transformed. </exception>
		public void FlushFinalBlock()
		{
			if (this._flushedFinalBlock)
			{
				throw new NotSupportedException(Locale.GetText("This method cannot be called twice."));
			}
			if (this._disposed)
			{
				throw new NotSupportedException(Locale.GetText("CryptoStream was disposed."));
			}
			if (this._mode != CryptoStreamMode.Write)
			{
				return;
			}
			this._flushedFinalBlock = true;
			byte[] array = this._transform.TransformFinalBlock(this._workingBlock, 0, this._partialCount);
			if (this._stream != null)
			{
				this._stream.Write(array, 0, array.Length);
				if (this._stream is CryptoStream)
				{
					(this._stream as CryptoStream).FlushFinalBlock();
				}
				this._stream.Flush();
			}
			Array.Clear(array, 0, array.Length);
		}

		/// <summary>Sets the position within the current stream.</summary>
		/// <returns>This method is not supported.</returns>
		/// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter. </param>
		/// <param name="origin">A <see cref="T:System.IO.SeekOrigin" /> object indicating the reference point used to obtain the new position. </param>
		/// <exception cref="T:System.NotSupportedException">This method is not supported. </exception>
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("Seek");
		}

		/// <summary>Sets the length of the current stream.</summary>
		/// <param name="value">The desired length of the current stream in bytes. </param>
		/// <exception cref="T:System.NotSupportedException">This property exists only to support inheritance from <see cref="T:System.IO.Stream" />, and cannot be used.</exception>
		public override void SetLength(long value)
		{
			throw new NotSupportedException("SetLength");
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Security.Cryptography.CryptoStream" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected override void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				this._disposed = true;
				if (this._workingBlock != null)
				{
					Array.Clear(this._workingBlock, 0, this._workingBlock.Length);
				}
				if (this._currentBlock != null)
				{
					Array.Clear(this._currentBlock, 0, this._currentBlock.Length);
				}
				if (disposing)
				{
					this._stream = null;
					this._workingBlock = null;
					this._currentBlock = null;
				}
			}
		}
	}
}
