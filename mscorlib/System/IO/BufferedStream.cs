using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	/// <summary>Adds a buffering layer to read and write operations on another stream. This class cannot be inherited.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public sealed class BufferedStream : Stream
	{
		private Stream m_stream;

		private byte[] m_buffer;

		private int m_buffer_pos;

		private int m_buffer_read_ahead;

		private bool m_buffer_reading;

		private bool disposed;

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.BufferedStream" /> class with a default buffer size of 4096 bytes.</summary>
		/// <param name="stream">The current stream. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> is null. </exception>
		public BufferedStream(Stream stream) : this(stream, 4096)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.BufferedStream" /> class with the specified buffer size.</summary>
		/// <param name="stream">The current stream. </param>
		/// <param name="bufferSize">The buffer size in bytes. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bufferSize" /> is negative. </exception>
		public BufferedStream(Stream stream, int bufferSize)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", "<= 0");
			}
			if (!stream.CanRead && !stream.CanWrite)
			{
				throw new ObjectDisposedException(Locale.GetText("Cannot access a closed Stream."));
			}
			this.m_stream = stream;
			this.m_buffer = new byte[bufferSize];
		}

		/// <summary>Gets a value indicating whether the current stream supports reading.</summary>
		/// <returns>true if the stream supports reading; false if the stream is closed or was opened with write-only access.</returns>
		/// <filterpriority>2</filterpriority>
		public override bool CanRead
		{
			get
			{
				return this.m_stream.CanRead;
			}
		}

		/// <summary>Gets a value indicating whether the current stream supports writing.</summary>
		/// <returns>true if the stream supports writing; false if the stream is closed or was opened with read-only access.</returns>
		/// <filterpriority>2</filterpriority>
		public override bool CanWrite
		{
			get
			{
				return this.m_stream.CanWrite;
			}
		}

		/// <summary>Gets a value indicating whether the current stream supports seeking.</summary>
		/// <returns>true if the stream supports seeking; false if the stream is closed or if the stream was constructed from an operating system handle such as a pipe or output to the console.</returns>
		/// <filterpriority>2</filterpriority>
		public override bool CanSeek
		{
			get
			{
				return this.m_stream.CanSeek;
			}
		}

		/// <summary>Gets the stream length in bytes.</summary>
		/// <returns>The stream length in bytes.</returns>
		/// <exception cref="T:System.IO.IOException">The underlying stream is null or closed. </exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support seeking. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override long Length
		{
			get
			{
				this.Flush();
				return this.m_stream.Length;
			}
		}

		/// <summary>Gets the position within the current stream.</summary>
		/// <returns>The position within the current stream.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value passed to <see cref="M:System.IO.BufferedStream.Seek(System.Int64,System.IO.SeekOrigin)" /> is negative. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs, such as the stream being closed. </exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support seeking. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override long Position
		{
			get
			{
				this.CheckObjectDisposedException();
				return this.m_stream.Position - (long)this.m_buffer_read_ahead + (long)this.m_buffer_pos;
			}
			set
			{
				if (value < this.Position && this.Position - value <= (long)this.m_buffer_pos && this.m_buffer_reading)
				{
					this.m_buffer_pos -= (int)(this.Position - value);
				}
				else if (value > this.Position && value - this.Position < (long)(this.m_buffer_read_ahead - this.m_buffer_pos) && this.m_buffer_reading)
				{
					this.m_buffer_pos += (int)(value - this.Position);
				}
				else
				{
					this.Flush();
					this.m_stream.Position = value;
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (this.m_buffer != null)
			{
				this.Flush();
			}
			this.m_stream.Close();
			this.m_buffer = null;
			this.disposed = true;
		}

		/// <summary>Clears all buffers for this stream and causes any buffered data to be written to the underlying device.</summary>
		/// <exception cref="T:System.IO.IOException">The data source or repository is not open. </exception>
		/// <filterpriority>2</filterpriority>
		public override void Flush()
		{
			this.CheckObjectDisposedException();
			if (this.m_buffer_reading)
			{
				if (this.CanSeek)
				{
					this.m_stream.Position = this.Position;
				}
			}
			else if (this.m_buffer_pos > 0)
			{
				this.m_stream.Write(this.m_buffer, 0, this.m_buffer_pos);
			}
			this.m_buffer_read_ahead = 0;
			this.m_buffer_pos = 0;
		}

		/// <summary>Sets the position within the current buffered stream.</summary>
		/// <returns>The new position within the current buffered stream.</returns>
		/// <param name="offset">A byte offset relative to <paramref name="origin" />. </param>
		/// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point from which to obtain the new position. </param>
		/// <exception cref="T:System.IO.IOException">The stream is not open or is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support seeking. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.CheckObjectDisposedException();
			if (!this.CanSeek)
			{
				throw new NotSupportedException(Locale.GetText("Non seekable stream."));
			}
			this.Flush();
			return this.m_stream.Seek(offset, origin);
		}

		/// <summary>Sets the length of the buffered stream.</summary>
		/// <param name="value">An integer indicating the desired length of the current buffered stream in bytes. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="value" /> is negative. </exception>
		/// <exception cref="T:System.IO.IOException">The stream is not open or is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override void SetLength(long value)
		{
			this.CheckObjectDisposedException();
			if (value < 0L)
			{
				throw new ArgumentOutOfRangeException("value must be positive");
			}
			if (!this.m_stream.CanWrite && !this.m_stream.CanSeek)
			{
				throw new NotSupportedException("the stream cannot seek nor write.");
			}
			if (this.m_stream == null || (!this.m_stream.CanRead && !this.m_stream.CanWrite))
			{
				throw new IOException("the stream is not open");
			}
			this.m_stream.SetLength(value);
			if (this.Position > value)
			{
				this.Position = value;
			}
		}

		/// <summary>Reads a byte from the underlying stream and returns the byte cast to an int, or returns -1 if reading from the end of the stream.</summary>
		/// <returns>The byte cast to an int, or -1 if reading from the end of the stream.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs, such as the stream being closed. </exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override int ReadByte()
		{
			this.CheckObjectDisposedException();
			byte[] array = new byte[1];
			if (this.Read(array, 0, 1) == 1)
			{
				return (int)array[0];
			}
			return -1;
		}

		/// <summary>Writes a byte to the current position in the buffered stream.</summary>
		/// <param name="value">A byte to write to the stream. </param>
		/// <exception cref="T:System.NotSupportedException">The stream does not support writing. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override void WriteByte(byte value)
		{
			this.CheckObjectDisposedException();
			this.Write(new byte[]
			{
				value
			}, 0, 1);
		}

		/// <summary>Copies bytes from the current buffered stream to an array.</summary>
		/// <returns>The total number of bytes read into <paramref name="array" />. This can be less than the number of bytes requested if that many bytes are not currently available, or 0 if the end of the stream has been reached before any data can be read.</returns>
		/// <param name="array">The buffer to which bytes are to be copied. </param>
		/// <param name="offset">The byte offset in the buffer at which to begin reading bytes. </param>
		/// <param name="count">The number of bytes to be read. </param>
		/// <exception cref="T:System.ArgumentException">Length of <paramref name="array" /> minus <paramref name="offset" /> is less than <paramref name="count" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative. </exception>
		/// <exception cref="T:System.IO.IOException">The stream is not open or is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override int Read([In] [Out] byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			this.CheckObjectDisposedException();
			if (!this.m_stream.CanRead)
			{
				throw new NotSupportedException(Locale.GetText("Cannot read from stream"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "< 0");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "< 0");
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException("array.Length - offset < count");
			}
			if (!this.m_buffer_reading)
			{
				this.Flush();
				this.m_buffer_reading = true;
			}
			if (count <= this.m_buffer_read_ahead - this.m_buffer_pos)
			{
				Buffer.BlockCopyInternal(this.m_buffer, this.m_buffer_pos, array, offset, count);
				this.m_buffer_pos += count;
				if (this.m_buffer_pos == this.m_buffer_read_ahead)
				{
					this.m_buffer_pos = 0;
					this.m_buffer_read_ahead = 0;
				}
				return count;
			}
			int num = this.m_buffer_read_ahead - this.m_buffer_pos;
			Buffer.BlockCopyInternal(this.m_buffer, this.m_buffer_pos, array, offset, num);
			this.m_buffer_pos = 0;
			this.m_buffer_read_ahead = 0;
			offset += num;
			count -= num;
			if (count >= this.m_buffer.Length)
			{
				num += this.m_stream.Read(array, offset, count);
			}
			else
			{
				this.m_buffer_read_ahead = this.m_stream.Read(this.m_buffer, 0, this.m_buffer.Length);
				if (count < this.m_buffer_read_ahead)
				{
					Buffer.BlockCopyInternal(this.m_buffer, 0, array, offset, count);
					this.m_buffer_pos = count;
					num += count;
				}
				else
				{
					Buffer.BlockCopyInternal(this.m_buffer, 0, array, offset, this.m_buffer_read_ahead);
					num += this.m_buffer_read_ahead;
					this.m_buffer_read_ahead = 0;
				}
			}
			return num;
		}

		/// <summary>Copies bytes to the buffered stream and advances the current position within the buffered stream by the number of bytes written.</summary>
		/// <param name="array">The byte array from which to copy <paramref name="count" /> bytes to the current buffered stream. </param>
		/// <param name="offset">The offset in the buffer at which to begin copying bytes to the current buffered stream. </param>
		/// <param name="count">The number of bytes to be written to the current buffered stream. </param>
		/// <exception cref="T:System.ArgumentException">Length of <paramref name="array" /> minus <paramref name="offset" /> is less than <paramref name="count" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative. </exception>
		/// <exception cref="T:System.IO.IOException">The stream is closed or null. </exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support writing. </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override void Write(byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			this.CheckObjectDisposedException();
			if (!this.m_stream.CanWrite)
			{
				throw new NotSupportedException(Locale.GetText("Cannot write to stream"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "< 0");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "< 0");
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException("array.Length - offset < count");
			}
			if (this.m_buffer_reading)
			{
				this.Flush();
				this.m_buffer_reading = false;
			}
			if (this.m_buffer_pos >= this.m_buffer.Length - count)
			{
				this.Flush();
				this.m_stream.Write(array, offset, count);
			}
			else
			{
				Buffer.BlockCopyInternal(array, offset, this.m_buffer, this.m_buffer_pos, count);
				this.m_buffer_pos += count;
			}
		}

		private void CheckObjectDisposedException()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("BufferedStream", Locale.GetText("Stream is closed"));
			}
		}
	}
}
