using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	/// <summary>Creates a stream whose backing store is memory.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[MonoTODO("Serialization format not compatible with .NET")]
	[Serializable]
	public class MemoryStream : Stream
	{
		private bool canWrite;

		private bool allowGetBuffer;

		private int capacity;

		private int length;

		private byte[] internalBuffer;

		private int initialIndex;

		private bool expandable;

		private bool streamClosed;

		private int position;

		private int dirty_bytes;

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.MemoryStream" /> class with an expandable capacity initialized to zero.</summary>
		public MemoryStream() : this(0)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.MemoryStream" /> class with an expandable capacity initialized as specified.</summary>
		/// <param name="capacity">The initial size of the internal array in bytes. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="capacity" /> is negative. </exception>
		public MemoryStream(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			this.canWrite = true;
			this.capacity = capacity;
			this.internalBuffer = new byte[capacity];
			this.expandable = true;
			this.allowGetBuffer = true;
		}

		/// <summary>Initializes a new non-resizable instance of the <see cref="T:System.IO.MemoryStream" /> class based on the specified byte array.</summary>
		/// <param name="buffer">The array of unsigned bytes from which to create the current stream. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		public MemoryStream(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.InternalConstructor(buffer, 0, buffer.Length, true, false);
		}

		/// <summary>Initializes a new non-resizable instance of the <see cref="T:System.IO.MemoryStream" /> class based on the specified byte array with the <see cref="P:System.IO.MemoryStream.CanWrite" /> property set as specified.</summary>
		/// <param name="buffer">The array of unsigned bytes from which to create this stream. </param>
		/// <param name="writable">The setting of the <see cref="P:System.IO.MemoryStream.CanWrite" /> property, which determines whether the stream supports writing. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		public MemoryStream(byte[] buffer, bool writable)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.InternalConstructor(buffer, 0, buffer.Length, writable, false);
		}

		/// <summary>Initializes a new non-resizable instance of the <see cref="T:System.IO.MemoryStream" /> class based on the specified region (index) of a byte array.</summary>
		/// <param name="buffer">The array of unsigned bytes from which to create this stream. </param>
		/// <param name="index">The index into <paramref name="buffer" /> at which the stream begins. </param>
		/// <param name="count">The length of the stream in bytes. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">The sum of <paramref name="index" /> and <paramref name="count" /> is greater than the length of <paramref name="buffer" />. </exception>
		public MemoryStream(byte[] buffer, int index, int count)
		{
			this.InternalConstructor(buffer, index, count, true, false);
		}

		/// <summary>Initializes a new non-resizable instance of the <see cref="T:System.IO.MemoryStream" /> class based on the specified region of a byte array, with the <see cref="P:System.IO.MemoryStream.CanWrite" /> property set as specified.</summary>
		/// <param name="buffer">The array of unsigned bytes from which to create this stream. </param>
		/// <param name="index">The index in <paramref name="buffer" /> at which the stream begins. </param>
		/// <param name="count">The length of the stream in bytes. </param>
		/// <param name="writable">The setting of the <see cref="P:System.IO.MemoryStream.CanWrite" /> property, which determines whether the stream supports writing. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> are negative. </exception>
		/// <exception cref="T:System.ArgumentException">The sum of <paramref name="index" /> and <paramref name="count" /> is greater than the length of <paramref name="buffer" />. </exception>
		public MemoryStream(byte[] buffer, int index, int count, bool writable)
		{
			this.InternalConstructor(buffer, index, count, writable, false);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.MemoryStream" /> class based on the specified region of a byte array, with the <see cref="P:System.IO.MemoryStream.CanWrite" /> property set as specified, and the ability to call <see cref="M:System.IO.MemoryStream.GetBuffer" /> set as specified.</summary>
		/// <param name="buffer">The array of unsigned bytes from which to create this stream. </param>
		/// <param name="index">The index into <paramref name="buffer" /> at which the stream begins. </param>
		/// <param name="count">The length of the stream in bytes. </param>
		/// <param name="writable">The setting of the <see cref="P:System.IO.MemoryStream.CanWrite" /> property, which determines whether the stream supports writing. </param>
		/// <param name="publiclyVisible">true to enable <see cref="M:System.IO.MemoryStream.GetBuffer" />, which returns the unsigned byte array from which the stream was created; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative. </exception>
		/// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />. </exception>
		public MemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible)
		{
			this.InternalConstructor(buffer, index, count, writable, publiclyVisible);
		}

		private void InternalConstructor(byte[] buffer, int index, int count, bool writable, bool publicallyVisible)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException("index or count is less than 0.");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException("index+count", "The size of the buffer is less than index + count.");
			}
			this.canWrite = writable;
			this.internalBuffer = buffer;
			this.capacity = count + index;
			this.length = this.capacity;
			this.position = index;
			this.initialIndex = index;
			this.allowGetBuffer = publicallyVisible;
			this.expandable = false;
		}

		private void CheckIfClosedThrowDisposed()
		{
			if (this.streamClosed)
			{
				throw new ObjectDisposedException("MemoryStream");
			}
		}

		/// <summary>Gets a value indicating whether the current stream supports reading.</summary>
		/// <returns>true if the stream is open.</returns>
		/// <filterpriority>2</filterpriority>
		public override bool CanRead
		{
			get
			{
				return !this.streamClosed;
			}
		}

		/// <summary>Gets a value indicating whether the current stream supports seeking.</summary>
		/// <returns>true if the stream is open.</returns>
		/// <filterpriority>2</filterpriority>
		public override bool CanSeek
		{
			get
			{
				return !this.streamClosed;
			}
		}

		/// <summary>Gets a value indicating whether the current stream supports writing.</summary>
		/// <returns>true if the stream supports writing; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		public override bool CanWrite
		{
			get
			{
				return !this.streamClosed && this.canWrite;
			}
		}

		/// <summary>Gets or sets the number of bytes allocated for this stream.</summary>
		/// <returns>The length of the usable portion of the buffer for the stream.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">A capacity is set that is negative or less than the current length of the stream. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current stream is closed. </exception>
		/// <exception cref="T:System.NotSupportedException">set is invoked on a stream whose capacity cannot be modified. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual int Capacity
		{
			get
			{
				this.CheckIfClosedThrowDisposed();
				return this.capacity - this.initialIndex;
			}
			set
			{
				this.CheckIfClosedThrowDisposed();
				if (value == this.capacity)
				{
					return;
				}
				if (!this.expandable)
				{
					throw new NotSupportedException("Cannot expand this MemoryStream");
				}
				if (value < 0 || value < this.length)
				{
					throw new ArgumentOutOfRangeException("value", string.Concat(new object[]
					{
						"New capacity cannot be negative or less than the current capacity ",
						value,
						" ",
						this.capacity
					}));
				}
				byte[] dst = null;
				if (value != 0)
				{
					dst = new byte[value];
					Buffer.BlockCopy(this.internalBuffer, 0, dst, 0, this.length);
				}
				this.dirty_bytes = 0;
				this.internalBuffer = dst;
				this.capacity = value;
			}
		}

		/// <summary>Gets the length of the stream in bytes.</summary>
		/// <returns>The length of the stream in bytes.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override long Length
		{
			get
			{
				this.CheckIfClosedThrowDisposed();
				return (long)(this.length - this.initialIndex);
			}
		}

		/// <summary>Gets or sets the current position within the stream.</summary>
		/// <returns>The current position within the stream.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The position is set to a negative value or a value greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override long Position
		{
			get
			{
				this.CheckIfClosedThrowDisposed();
				return (long)(this.position - this.initialIndex);
			}
			set
			{
				this.CheckIfClosedThrowDisposed();
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", "Position cannot be negative");
				}
				if (value > 2147483647L)
				{
					throw new ArgumentOutOfRangeException("value", "Position must be non-negative and less than 2^31 - 1 - origin");
				}
				this.position = this.initialIndex + (int)value;
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.MemoryStream" /> class and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			this.streamClosed = true;
			this.expandable = false;
		}

		/// <summary>Overrides <see cref="M:System.IO.Stream.Flush" /> so that no action is performed.</summary>
		/// <filterpriority>2</filterpriority>
		public override void Flush()
		{
		}

		/// <summary>Returns the array of unsigned bytes from which this stream was created.</summary>
		/// <returns>The byte array from which this stream was created, or the underlying array if a byte array was not provided to the <see cref="T:System.IO.MemoryStream" /> constructor during construction of the current instance.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The MemoryStream instance was not created with a publicly visible buffer. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual byte[] GetBuffer()
		{
			if (!this.allowGetBuffer)
			{
				throw new UnauthorizedAccessException();
			}
			return this.internalBuffer;
		}

		/// <summary>Reads a block of bytes from the current stream and writes the data to <paramref name="buffer" />.</summary>
		/// <returns>The total number of bytes written into the buffer. This can be less than the number of bytes requested if that number of bytes are not currently available, or zero if the end of the stream is reached before any bytes are read.</returns>
		/// <param name="buffer">When this method returns, contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the characters read from the current stream. </param>
		/// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin reading. </param>
		/// <param name="count">The maximum number of bytes to read. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" /> subtracted from the buffer length is less than <paramref name="count" />. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current stream instance is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override int Read([In] [Out] byte[] buffer, int offset, int count)
		{
			this.CheckIfClosedThrowDisposed();
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException("offset or count less than zero.");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("offset+count", "The size of the buffer is less than offset + count.");
			}
			if (this.position >= this.length || count == 0)
			{
				return 0;
			}
			if (this.position > this.length - count)
			{
				count = this.length - this.position;
			}
			Buffer.BlockCopy(this.internalBuffer, this.position, buffer, offset, count);
			this.position += count;
			return count;
		}

		/// <summary>Reads a byte from the current stream.</summary>
		/// <returns>The byte cast to a <see cref="T:System.Int32" />, or -1 if the end of the stream has been reached.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current stream instance is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override int ReadByte()
		{
			this.CheckIfClosedThrowDisposed();
			if (this.position >= this.length)
			{
				return -1;
			}
			return (int)this.internalBuffer[this.position++];
		}

		/// <summary>Sets the position within the current stream to the specified value.</summary>
		/// <returns>The new position within the stream, calculated by combining the initial reference point and the offset.</returns>
		/// <param name="offset">The new position within the stream. This is relative to the <paramref name="loc" /> parameter, and can be positive or negative. </param>
		/// <param name="loc">A value of type <see cref="T:System.IO.SeekOrigin" />, which acts as the seek reference point. </param>
		/// <exception cref="T:System.IO.IOException">Seeking is attempted before the beginning of the stream. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <exception cref="T:System.ArgumentException">There is an invalid <see cref="T:System.IO.SeekOrigin" />. -or-<paramref name="offset" /> caused an arithmetic overflow.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The current stream instance is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override long Seek(long offset, SeekOrigin loc)
		{
			this.CheckIfClosedThrowDisposed();
			if (offset > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("Offset out of range. " + offset);
			}
			int num;
			switch (loc)
			{
			case SeekOrigin.Begin:
				if (offset < 0L)
				{
					throw new IOException("Attempted to seek before start of MemoryStream.");
				}
				num = this.initialIndex;
				break;
			case SeekOrigin.Current:
				num = this.position;
				break;
			case SeekOrigin.End:
				num = this.length;
				break;
			default:
				throw new ArgumentException("loc", "Invalid SeekOrigin");
			}
			num += (int)offset;
			if (num < this.initialIndex)
			{
				throw new IOException("Attempted to seek before start of MemoryStream.");
			}
			this.position = num;
			return (long)this.position;
		}

		private int CalculateNewCapacity(int minimum)
		{
			if (minimum < 256)
			{
				minimum = 256;
			}
			if (minimum < this.capacity * 2)
			{
				minimum = this.capacity * 2;
			}
			return minimum;
		}

		private void Expand(int newSize)
		{
			if (newSize > this.capacity)
			{
				this.Capacity = this.CalculateNewCapacity(newSize);
			}
			else if (this.dirty_bytes > 0)
			{
				Array.Clear(this.internalBuffer, this.length, this.dirty_bytes);
				this.dirty_bytes = 0;
			}
		}

		/// <summary>Sets the length of the current stream to the specified value.</summary>
		/// <param name="value">The value at which to set the length. </param>
		/// <exception cref="T:System.NotSupportedException">The current stream is not resizable and <paramref name="value" /> is larger than the current capacity.-or- The current stream does not support writing. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="value" /> is negative or is greater than the maximum length of the <see cref="T:System.IO.MemoryStream" />, where the maximum length is(<see cref="F:System.Int32.MaxValue" /> - origin), and origin is the index into the underlying buffer at which the stream starts. </exception>
		/// <filterpriority>2</filterpriority>
		public override void SetLength(long value)
		{
			if (!this.expandable && value > (long)this.capacity)
			{
				throw new NotSupportedException("Expanding this MemoryStream is not supported");
			}
			this.CheckIfClosedThrowDisposed();
			if (!this.canWrite)
			{
				throw new NotSupportedException(Locale.GetText("Cannot write to this MemoryStream"));
			}
			if (value < 0L || value + (long)this.initialIndex > 2147483647L)
			{
				throw new ArgumentOutOfRangeException();
			}
			int num = (int)value + this.initialIndex;
			if (num > this.length)
			{
				this.Expand(num);
			}
			else if (num < this.length)
			{
				this.dirty_bytes += this.length - num;
			}
			this.length = num;
			if (this.position > this.length)
			{
				this.position = this.length;
			}
		}

		/// <summary>Writes the stream contents to a byte array, regardless of the <see cref="P:System.IO.MemoryStream.Position" /> property.</summary>
		/// <returns>A new byte array.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual byte[] ToArray()
		{
			int num = this.length - this.initialIndex;
			byte[] array = new byte[num];
			if (this.internalBuffer != null)
			{
				Buffer.BlockCopy(this.internalBuffer, this.initialIndex, array, 0, num);
			}
			return array;
		}

		/// <summary>Writes a block of bytes to the current stream using data read from buffer.</summary>
		/// <param name="buffer">The buffer to write data from. </param>
		/// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing from. </param>
		/// <param name="count">The maximum number of bytes to write. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support writing. For additional information see <see cref="P:System.IO.Stream.CanWrite" />.-or- The current position is closer than <paramref name="count" /> bytes to the end of the stream, and the capacity cannot be modified. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" /> subtracted from the buffer length is less than <paramref name="count" />. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> are negative. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current stream instance is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.CheckIfClosedThrowDisposed();
			if (!this.canWrite)
			{
				throw new NotSupportedException("Cannot write to this stream.");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("offset+count", "The size of the buffer is less than offset + count.");
			}
			if (this.position > this.length - count)
			{
				this.Expand(this.position + count);
			}
			Buffer.BlockCopy(buffer, offset, this.internalBuffer, this.position, count);
			this.position += count;
			if (this.position >= this.length)
			{
				this.length = this.position;
			}
		}

		/// <summary>Writes a byte to the current stream at the current position.</summary>
		/// <param name="value">The byte to write. </param>
		/// <exception cref="T:System.NotSupportedException">The stream does not support writing. For additional information see <see cref="P:System.IO.Stream.CanWrite" />.-or- The current position is at the end of the stream, and the capacity cannot be modified. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current stream is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public override void WriteByte(byte value)
		{
			this.CheckIfClosedThrowDisposed();
			if (!this.canWrite)
			{
				throw new NotSupportedException("Cannot write to this stream.");
			}
			if (this.position >= this.length)
			{
				this.Expand(this.position + 1);
				this.length = this.position + 1;
			}
			this.internalBuffer[this.position++] = value;
		}

		/// <summary>Writes the entire contents of this memory stream to another stream.</summary>
		/// <param name="stream">The stream to write this memory stream to. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> is null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current or target stream is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void WriteTo(Stream stream)
		{
			this.CheckIfClosedThrowDisposed();
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			stream.Write(this.internalBuffer, this.initialIndex, this.length - this.initialIndex);
		}
	}
}
