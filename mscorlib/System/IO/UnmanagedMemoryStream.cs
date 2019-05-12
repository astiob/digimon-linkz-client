using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	/// <summary>Provides access to unmanaged blocks of memory from managed code.</summary>
	/// <filterpriority>2</filterpriority>
	[CLSCompliant(false)]
	public class UnmanagedMemoryStream : Stream
	{
		private long length;

		private bool closed;

		private long capacity;

		private FileAccess fileaccess;

		private IntPtr initial_pointer;

		private long initial_position;

		private long current_position;

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.IO.UnmanagedMemoryStream" /> class.</summary>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the required permission.</exception>
		protected UnmanagedMemoryStream()
		{
			this.closed = true;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryStream" /> class using the specified location and memory length.</summary>
		/// <param name="pointer">A pointer to an unmanaged memory location.</param>
		/// <param name="length">The length of the memory to use.</param>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the required permission.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="pointer" /> value is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="length" /> value is less than zero.- or -The <paramref name="length" /> is large enough to cause an overflow.</exception>
		public unsafe UnmanagedMemoryStream(byte* pointer, long length)
		{
			this.Initialize(pointer, length, length, FileAccess.Read);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryStream" /> class using the specified location, memory length, total amount of memory, and file access values.</summary>
		/// <param name="pointer">A pointer to an unmanaged memory location.</param>
		/// <param name="length">The length of the memory to use.</param>
		/// <param name="capacity">The total amount of memory assigned to the stream.</param>
		/// <param name="access">One of the <see cref="T:System.IO.FileAccess" /> values.</param>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the required permission.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="pointer" /> value is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="length" /> value is less than zero.- or - The <paramref name="capacity" /> value is less than zero.- or -The <paramref name="length" /> value is greater than the <paramref name="capacity" /> value.</exception>
		public unsafe UnmanagedMemoryStream(byte* pointer, long length, long capacity, FileAccess access)
		{
			this.Initialize(pointer, length, capacity, access);
		}

		internal event EventHandler Closed;

		/// <summary>Gets a value indicating whether a stream supports reading.</summary>
		/// <returns>false if the object was created by a constructor with an <paramref name="access" /> parameter that did not include reading the stream and if the stream is closed; otherwise, true.</returns>
		/// <filterpriority>2</filterpriority>
		public override bool CanRead
		{
			get
			{
				return !this.closed && this.fileaccess != FileAccess.Write;
			}
		}

		/// <summary>Gets a value indicating whether a stream supports seeking.</summary>
		/// <returns>false if the stream is closed; otherwise, true.</returns>
		/// <filterpriority>2</filterpriority>
		public override bool CanSeek
		{
			get
			{
				return !this.closed;
			}
		}

		/// <summary>Gets a value indicating whether a stream supports writing.</summary>
		/// <returns>false if the object was created by a constructor with an <paramref name="access" /> parameter value that supports writing or was created by a constructor that had no parameters, or if the stream is closed; otherwise, true.</returns>
		/// <filterpriority>2</filterpriority>
		public override bool CanWrite
		{
			get
			{
				return !this.closed && this.fileaccess != FileAccess.Read;
			}
		}

		/// <summary>Gets the stream length (size) or the total amount of memory assigned to a stream (capacity).</summary>
		/// <returns>The size or capacity of the stream.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
		/// <filterpriority>2</filterpriority>
		public long Capacity
		{
			get
			{
				if (this.closed)
				{
					throw new ObjectDisposedException("The stream is closed");
				}
				return this.capacity;
			}
		}

		/// <summary>Gets the length of the data in a stream.</summary>
		/// <returns>The length of the data in the stream.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
		/// <filterpriority>2</filterpriority>
		public override long Length
		{
			get
			{
				if (this.closed)
				{
					throw new ObjectDisposedException("The stream is closed");
				}
				return this.length;
			}
		}

		/// <summary>Gets or sets the current position in a stream.</summary>
		/// <returns>The current position in the stream.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The position is set to a value that is less than zero, or the position is larger than <see cref="F:System.Int32.MaxValue" /> or results in overflow when added to the current pointer.</exception>
		/// <filterpriority>2</filterpriority>
		public override long Position
		{
			get
			{
				if (this.closed)
				{
					throw new ObjectDisposedException("The stream is closed");
				}
				return this.current_position;
			}
			set
			{
				if (this.closed)
				{
					throw new ObjectDisposedException("The stream is closed");
				}
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", "Non-negative number required.");
				}
				if (value > 2147483647L)
				{
					throw new ArgumentOutOfRangeException("value", "The position is larger than Int32.MaxValue.");
				}
				this.current_position = value;
			}
		}

		/// <summary>Gets or sets a byte pointer to a stream based on the current position in the stream.</summary>
		/// <returns>A byte pointer.</returns>
		/// <exception cref="T:System.IndexOutOfRangeException">The current position is larger than the capacity of the stream.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The position is being set is not a valid position in the current stream.</exception>
		/// <exception cref="T:System.IO.IOException">The pointer is being set to a lower value than the starting position of the stream.</exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		public unsafe byte* PositionPointer
		{
			get
			{
				if (this.closed)
				{
					throw new ObjectDisposedException("The stream is closed");
				}
				if (this.current_position >= this.length)
				{
					throw new IndexOutOfRangeException("value");
				}
				return (byte*)((void*)this.initial_pointer) + this.current_position;
			}
			set
			{
				if (this.closed)
				{
					throw new ObjectDisposedException("The stream is closed");
				}
				if (value < (byte*)((void*)this.initial_pointer))
				{
					throw new IOException("Address is below the inital address");
				}
				this.Position = (long)(value - (void*)this.initial_pointer);
			}
		}

		/// <summary>Reads the specified number of bytes into the specified array.</summary>
		/// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
		/// <param name="buffer">When this method returns, contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source. This parameter is passed uninitialized.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
		/// <param name="count">The maximum number of bytes to read from the current stream.</param>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The underlying memory does not support reading.- or - The <see cref="P:System.IO.UnmanagedMemoryStream.CanRead" /> property is set to false. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> parameter is set to null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="offset" /> parameter is less than zero. - or - The <paramref name="count" /> parameter is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">The length of the buffer array minus the <paramref name="offset" /> parameter is less than the <paramref name="count" /> parameter.</exception>
		/// <filterpriority>2</filterpriority>
		public override int Read([In] [Out] byte[] buffer, int offset, int count)
		{
			if (this.closed)
			{
				throw new ObjectDisposedException("The stream is closed");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("The length of the buffer array minus the offset parameter is less than the count parameter");
			}
			if (this.fileaccess == FileAccess.Write)
			{
				throw new NotSupportedException("Stream does not support reading");
			}
			if (this.current_position >= this.length)
			{
				return 0;
			}
			int num = (this.current_position + (long)count >= this.length) ? ((int)(this.length - this.current_position)) : count;
			Marshal.Copy(new IntPtr(this.initial_pointer.ToInt64() + this.current_position), buffer, offset, num);
			this.current_position += (long)num;
			return num;
		}

		/// <summary>Reads a byte from a stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.</summary>
		/// <returns>The unsigned byte cast to an <see cref="T:System.Int32" /> object, or -1 if at the end of the stream.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The underlying memory does not support reading.- or -The current position is at the end of the stream.</exception>
		/// <filterpriority>2</filterpriority>
		public override int ReadByte()
		{
			if (this.closed)
			{
				throw new ObjectDisposedException("The stream is closed");
			}
			if (this.fileaccess == FileAccess.Write)
			{
				throw new NotSupportedException("Stream does not support reading");
			}
			if (this.current_position >= this.length)
			{
				return -1;
			}
			IntPtr ptr = this.initial_pointer;
			long num;
			this.current_position = (num = this.current_position) + 1L;
			return (int)Marshal.ReadByte(ptr, (int)num);
		}

		/// <summary>Sets the current position of the current stream to the given value.</summary>
		/// <returns>The new position in the stream.</returns>
		/// <param name="offset">The point relative to <paramref name="origin" /> to begin seeking from. </param>
		/// <param name="loc">Specifies the beginning, the end, or the current position as a reference point for <paramref name="origin" />, using a value of type <see cref="T:System.IO.SeekOrigin" />. </param>
		/// <exception cref="T:System.IO.IOException">An attempt was made to seek before the beginning of the stream.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="offset" /> value is larger than the maximum size of the stream.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="loc" /> is invalid.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
		/// <filterpriority>2</filterpriority>
		public override long Seek(long offset, SeekOrigin loc)
		{
			if (this.closed)
			{
				throw new ObjectDisposedException("The stream is closed");
			}
			long num;
			switch (loc)
			{
			case SeekOrigin.Begin:
				if (offset < 0L)
				{
					throw new IOException("An attempt was made to seek before the beginning of the stream");
				}
				num = this.initial_position;
				break;
			case SeekOrigin.Current:
				num = this.current_position;
				break;
			case SeekOrigin.End:
				num = this.length;
				break;
			default:
				throw new ArgumentException("Invalid SeekOrigin option");
			}
			num += offset;
			if (num < this.initial_position)
			{
				throw new IOException("An attempt was made to seek before the beginning of the stream");
			}
			this.current_position = num;
			return this.current_position;
		}

		/// <summary>Sets the length of a stream to a specified value.</summary>
		/// <param name="value">The length of the stream.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error has occurred. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The underlying memory does not support writing.- or -An attempt is made to write to the stream and the <see cref="P:System.IO.UnmanagedMemoryStream.CanWrite" /> property is false.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified <paramref name="value" /> exceeds the capacity of the stream.- or -The specified <paramref name="value" /> is negative.</exception>
		/// <filterpriority>2</filterpriority>
		public override void SetLength(long value)
		{
			if (this.closed)
			{
				throw new ObjectDisposedException("The stream is closed");
			}
			if (value < 0L)
			{
				throw new ArgumentOutOfRangeException("length", "Non-negative number required.");
			}
			if (value > this.capacity)
			{
				throw new IOException("Unable to expand length of this stream beyond its capacity.");
			}
			if (this.fileaccess == FileAccess.Read)
			{
				throw new NotSupportedException("Stream does not support writing.");
			}
			this.length = value;
			if (this.length < this.current_position)
			{
				this.current_position = this.length;
			}
		}

		/// <summary>Overrides the <see cref="M:System.IO.Stream.Flush" /> method so that no action is performed.</summary>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
		/// <filterpriority>2</filterpriority>
		public override void Flush()
		{
			if (this.closed)
			{
				throw new ObjectDisposedException("The stream is closed");
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.UnmanagedMemoryStream" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (this.closed)
			{
				return;
			}
			this.closed = true;
			if (this.Closed != null)
			{
				this.Closed(this, null);
			}
		}

		/// <summary>Writes a block of bytes to the current stream using data from a buffer.</summary>
		/// <param name="buffer">The byte array from which to copy bytes to the current stream.</param>
		/// <param name="offset">The offset in the buffer at which to begin copying bytes to the current stream.</param>
		/// <param name="count">The number of bytes to write to the current stream.</param>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The underlying memory does not support writing. - or -An attempt is made to write to the stream and the <see cref="P:System.IO.UnmanagedMemoryStream.CanWrite" /> property is false.- or -The <paramref name="count" /> value is greater than the capacity of the stream.- or -The position is at the end of the stream capacity.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">One of the specified parameters is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="offset" /> parameter minus the length of the <paramref name="buffer" /> parameter is less than the <paramref name="count" /> parameter.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> parameter is null.</exception>
		/// <filterpriority>2</filterpriority>
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.closed)
			{
				throw new ObjectDisposedException("The stream is closed");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("The buffer parameter is a null reference");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("The length of the buffer array minus the offset parameter is less than the count parameter");
			}
			if (this.current_position > this.capacity - (long)count)
			{
				throw new NotSupportedException("Unable to expand length of this stream beyond its capacity.");
			}
			if (this.fileaccess == FileAccess.Read)
			{
				throw new NotSupportedException("Stream does not support writing.");
			}
			for (int i = 0; i < count; i++)
			{
				IntPtr ptr = this.initial_pointer;
				long num;
				this.current_position = (num = this.current_position) + 1L;
				Marshal.WriteByte(ptr, (int)num, buffer[offset + i]);
			}
			if (this.current_position > this.length)
			{
				this.length = this.current_position;
			}
		}

		/// <summary>Writes a byte to the current position in the file stream.</summary>
		/// <param name="value">A byte value written to the stream.</param>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The underlying memory does not support writing.- or -An attempt is made to write to the stream and the <see cref="P:System.IO.UnmanagedMemoryStream.CanWrite" /> property is false.- or - The current position is at the end of the capacity of the stream.</exception>
		/// <exception cref="T:System.IO.IOException">The supplied <paramref name="value" /> causes the stream exceed its maximum capacity.</exception>
		/// <filterpriority>2</filterpriority>
		public override void WriteByte(byte value)
		{
			if (this.closed)
			{
				throw new ObjectDisposedException("The stream is closed");
			}
			if (this.current_position == this.capacity)
			{
				throw new NotSupportedException("The current position is at the end of the capacity of the stream");
			}
			if (this.fileaccess == FileAccess.Read)
			{
				throw new NotSupportedException("Stream does not support writing.");
			}
			Marshal.WriteByte(this.initial_pointer, (int)this.current_position, value);
			this.current_position += 1L;
			if (this.current_position > this.length)
			{
				this.length = this.current_position;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryStream" /> class.</summary>
		/// <param name="pointer">A pointer to an unmanaged memory location.</param>
		/// <param name="length">The length of the memory to use.</param>
		/// <param name="capacity">The total amount of memory assigned to the stream.</param>
		/// <param name="access">One of the <see cref="T:System.IO.FileAccess" /> values.</param>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the required permission.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="pointer" /> value is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="length" /> value is less than zero.- or - The <paramref name="capacity" /> value is less than zero.- or -The <paramref name="length" /> value is large enough to cause an overflow.</exception>
		protected unsafe void Initialize(byte* pointer, long length, long capacity, FileAccess access)
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer");
			}
			if (length < 0L)
			{
				throw new ArgumentOutOfRangeException("length", "Non-negative number required.");
			}
			if (capacity < 0L)
			{
				throw new ArgumentOutOfRangeException("capacity", "Non-negative number required.");
			}
			if (length > capacity)
			{
				throw new ArgumentOutOfRangeException("length", "The length cannot be greater than the capacity.");
			}
			if (access < FileAccess.Read || access > FileAccess.ReadWrite)
			{
				throw new ArgumentOutOfRangeException("access", "Enum value was out of legal range.");
			}
			this.fileaccess = access;
			this.length = length;
			this.capacity = capacity;
			this.initial_position = 0L;
			this.current_position = this.initial_position;
			this.initial_pointer = new IntPtr((void*)pointer);
			this.closed = false;
		}
	}
}
