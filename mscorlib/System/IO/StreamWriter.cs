using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO
{
	/// <summary>Implements a <see cref="T:System.IO.TextWriter" /> for writing characters to a stream in a particular encoding.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class StreamWriter : TextWriter
	{
		private const int DefaultBufferSize = 1024;

		private const int DefaultFileBufferSize = 4096;

		private const int MinimumBufferSize = 256;

		private Encoding internalEncoding;

		private Stream internalStream;

		private bool iflush;

		private byte[] byte_buf;

		private int byte_pos;

		private char[] decode_buf;

		private int decode_pos;

		private bool DisposedAlready;

		private bool preamble_done;

		/// <summary>Provides a StreamWriter with no backing store that can be written to, but not read from.</summary>
		/// <filterpriority>1</filterpriority>
		public new static readonly StreamWriter Null = new StreamWriter(Stream.Null, Encoding.UTF8Unmarked, 1);

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified stream, using UTF-8 encoding and the default buffer size.</summary>
		/// <param name="stream">The stream to write to. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="stream" /> is not writable. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> is null. </exception>
		public StreamWriter(Stream stream) : this(stream, Encoding.UTF8Unmarked, 1024)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified stream, using the specified encoding and the default buffer size.</summary>
		/// <param name="stream">The stream to write to. </param>
		/// <param name="encoding">The character encoding to use. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> or <paramref name="encoding" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="stream" /> is not writable. </exception>
		public StreamWriter(Stream stream, Encoding encoding) : this(stream, encoding, 1024)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified stream, using the specified encoding and buffer size.</summary>
		/// <param name="stream">The stream to write to. </param>
		/// <param name="encoding">The character encoding to use. </param>
		/// <param name="bufferSize">Sets the buffer size. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> or <paramref name="encoding" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bufferSize" /> is negative. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="stream" /> is not writable. </exception>
		public StreamWriter(Stream stream, Encoding encoding, int bufferSize)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize");
			}
			if (!stream.CanWrite)
			{
				throw new ArgumentException("Can not write to stream");
			}
			this.internalStream = stream;
			this.Initialize(encoding, bufferSize);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file on the specified path, using the default encoding and buffer size.</summary>
		/// <param name="path">The complete file path to write to. <paramref name="path" /> can be a file name. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">Access is denied. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is an empty string(""). -or-<paramref name="path" /> contains the name of a system device (com1, com2, and so on).</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public StreamWriter(string path) : this(path, false, Encoding.UTF8Unmarked, 4096)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file on the specified path, using the default encoding and buffer size. If the file exists, it can be either overwritten or appended to. If the file does not exist, this constructor creates a new file.</summary>
		/// <param name="path">The complete file path to write to. </param>
		/// <param name="append">Determines whether data is to be appended to the file. If the file exists and <paramref name="append" /> is false, the file is overwritten. If the file exists and <paramref name="append" /> is true, the data is appended to the file. Otherwise, a new file is created. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">Access is denied. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is empty. -or-<paramref name="path" /> contains the name of a system device (com1, com2, and so on).</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public StreamWriter(string path, bool append) : this(path, append, Encoding.UTF8Unmarked, 4096)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file on the specified path, using the specified encoding and default buffer size. If the file exists, it can be either overwritten or appended to. If the file does not exist, this constructor creates a new file.</summary>
		/// <param name="path">The complete file path to write to. </param>
		/// <param name="append">Determines whether data is to be appended to the file. If the file exists and <paramref name="append" /> is false, the file is overwritten. If the file exists and <paramref name="append" /> is true, the data is appended to the file. Otherwise, a new file is created. </param>
		/// <param name="encoding">The character encoding to use. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">Access is denied. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is empty. -or-<paramref name="path" /> contains the name of a system device (com1, com2, etc).</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is null. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public StreamWriter(string path, bool append, Encoding encoding) : this(path, append, encoding, 4096)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file on the specified path, using the specified encoding and buffer size. If the file exists, it can be either overwritten or appended to. If the file does not exist, this constructor creates a new file.</summary>
		/// <param name="path">The complete file path to write to. </param>
		/// <param name="append">Determines whether data is to be appended to the file. If the file exists and <paramref name="append" /> is false, the file is overwritten. If the file exists and <paramref name="append" /> is true, the data is appended to the file. Otherwise, a new file is created. </param>
		/// <param name="encoding">The character encoding to use. </param>
		/// <param name="bufferSize">Sets the buffer size. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is an empty string (""). -or-<paramref name="path" /> contains the name of a system device (com1, com2, etc).</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> or <paramref name="encoding" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bufferSize" /> is negative. </exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">Access is denied. </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
		public StreamWriter(string path, bool append, Encoding encoding, int bufferSize)
		{
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize");
			}
			FileMode mode;
			if (append)
			{
				mode = FileMode.Append;
			}
			else
			{
				mode = FileMode.Create;
			}
			this.internalStream = new FileStream(path, mode, FileAccess.Write, FileShare.Read);
			if (append)
			{
				this.internalStream.Position = this.internalStream.Length;
			}
			else
			{
				this.internalStream.SetLength(0L);
			}
			this.Initialize(encoding, bufferSize);
		}

		internal void Initialize(Encoding encoding, int bufferSize)
		{
			this.internalEncoding = encoding;
			this.decode_pos = (this.byte_pos = 0);
			int num = Math.Max(bufferSize, 256);
			this.decode_buf = new char[num];
			this.byte_buf = new byte[encoding.GetMaxByteCount(num)];
			if (this.internalStream.CanSeek && this.internalStream.Position > 0L)
			{
				this.preamble_done = true;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.IO.StreamWriter" /> will flush its buffer to the underlying stream after every call to <see cref="M:System.IO.StreamWriter.Write(System.Char)" />.</summary>
		/// <returns>true to force <see cref="T:System.IO.StreamWriter" /> to flush its buffer; otherwise, false.</returns>
		/// <filterpriority>1</filterpriority>
		public virtual bool AutoFlush
		{
			get
			{
				return this.iflush;
			}
			set
			{
				this.iflush = value;
				if (this.iflush)
				{
					this.Flush();
				}
			}
		}

		/// <summary>Gets the underlying stream that interfaces with a backing store.</summary>
		/// <returns>The stream this StreamWriter is writing to.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual Stream BaseStream
		{
			get
			{
				return this.internalStream;
			}
		}

		/// <summary>Gets the <see cref="T:System.Text.Encoding" /> in which the output is written.</summary>
		/// <returns>The <see cref="T:System.Text.Encoding" /> specified in the constructor for the current instance, or <see cref="T:System.Text.UTF8Encoding" /> if an encoding was not specified.</returns>
		/// <filterpriority>2</filterpriority>
		public override Encoding Encoding
		{
			get
			{
				return this.internalEncoding;
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.StreamWriter" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		/// <exception cref="T:System.Text.EncoderFallbackException">The current encoding does not support displaying half of a Unicode surrogate pair.</exception>
		protected override void Dispose(bool disposing)
		{
			Exception ex = null;
			if (!this.DisposedAlready && disposing && this.internalStream != null)
			{
				try
				{
					this.Flush();
				}
				catch (Exception ex2)
				{
					ex = ex2;
				}
				this.DisposedAlready = true;
				try
				{
					this.internalStream.Close();
				}
				catch (Exception ex3)
				{
					if (ex == null)
					{
						ex = ex3;
					}
				}
			}
			this.internalStream = null;
			this.byte_buf = null;
			this.internalEncoding = null;
			this.decode_buf = null;
			if (ex != null)
			{
				throw ex;
			}
		}

		/// <summary>Clears all buffers for the current writer and causes any buffered data to be written to the underlying stream.</summary>
		/// <exception cref="T:System.ObjectDisposedException">The current writer is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error has occurred. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">The current encoding does not support displaying half of a Unicode surrogate pair. </exception>
		/// <filterpriority>1</filterpriority>
		public override void Flush()
		{
			if (this.DisposedAlready)
			{
				throw new ObjectDisposedException("StreamWriter");
			}
			this.Decode();
			if (this.byte_pos > 0)
			{
				this.FlushBytes();
				this.internalStream.Flush();
			}
		}

		private void FlushBytes()
		{
			if (!this.preamble_done && this.byte_pos > 0)
			{
				byte[] preamble = this.internalEncoding.GetPreamble();
				if (preamble.Length > 0)
				{
					this.internalStream.Write(preamble, 0, preamble.Length);
				}
				this.preamble_done = true;
			}
			this.internalStream.Write(this.byte_buf, 0, this.byte_pos);
			this.byte_pos = 0;
		}

		private void Decode()
		{
			if (this.byte_pos > 0)
			{
				this.FlushBytes();
			}
			if (this.decode_pos > 0)
			{
				int bytes = this.internalEncoding.GetBytes(this.decode_buf, 0, this.decode_pos, this.byte_buf, this.byte_pos);
				this.byte_pos += bytes;
				this.decode_pos = 0;
			}
		}

		/// <summary>Writes a subarray of characters to the stream.</summary>
		/// <param name="buffer">A character array containing the data to write. </param>
		/// <param name="index">The index into <paramref name="buffer" /> at which to begin writing. </param>
		/// <param name="count">The number of characters to read from <paramref name="buffer" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream. </exception>
		/// <filterpriority>1</filterpriority>
		public override void Write(char[] buffer, int index, int count)
		{
			if (this.DisposedAlready)
			{
				throw new ObjectDisposedException("StreamWriter");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "< 0");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "< 0");
			}
			if (index > buffer.Length - count)
			{
				throw new ArgumentException("index + count > buffer.Length");
			}
			this.LowLevelWrite(buffer, index, count);
			if (this.iflush)
			{
				this.Flush();
			}
		}

		private void LowLevelWrite(char[] buffer, int index, int count)
		{
			while (count > 0)
			{
				int num = this.decode_buf.Length - this.decode_pos;
				if (num == 0)
				{
					this.Decode();
					num = this.decode_buf.Length;
				}
				if (num > count)
				{
					num = count;
				}
				Buffer.BlockCopy(buffer, index * 2, this.decode_buf, this.decode_pos * 2, num * 2);
				count -= num;
				index += num;
				this.decode_pos += num;
			}
		}

		private void LowLevelWrite(string s)
		{
			int i = s.Length;
			int num = 0;
			while (i > 0)
			{
				int num2 = this.decode_buf.Length - this.decode_pos;
				if (num2 == 0)
				{
					this.Decode();
					num2 = this.decode_buf.Length;
				}
				if (num2 > i)
				{
					num2 = i;
				}
				for (int j = 0; j < num2; j++)
				{
					this.decode_buf[j + this.decode_pos] = s[j + num];
				}
				i -= num2;
				num += num2;
				this.decode_pos += num2;
			}
		}

		/// <summary>Writes a character to the stream.</summary>
		/// <param name="value">The character to write to the text stream. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream. </exception>
		/// <filterpriority>1</filterpriority>
		public override void Write(char value)
		{
			if (this.DisposedAlready)
			{
				throw new ObjectDisposedException("StreamWriter");
			}
			if (this.decode_pos >= this.decode_buf.Length)
			{
				this.Decode();
			}
			this.decode_buf[this.decode_pos++] = value;
			if (this.iflush)
			{
				this.Flush();
			}
		}

		/// <summary>Writes a character array to the stream.</summary>
		/// <param name="buffer">A character array containing the data to write. If <paramref name="buffer" /> is null, nothing is written. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream. </exception>
		/// <filterpriority>1</filterpriority>
		public override void Write(char[] buffer)
		{
			if (this.DisposedAlready)
			{
				throw new ObjectDisposedException("StreamWriter");
			}
			if (buffer != null)
			{
				this.LowLevelWrite(buffer, 0, buffer.Length);
			}
			if (this.iflush)
			{
				this.Flush();
			}
		}

		/// <summary>Writes a string to the stream.</summary>
		/// <param name="value">The string to write to the stream. If <paramref name="value" /> is null, nothing is written. </param>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>1</filterpriority>
		public override void Write(string value)
		{
			if (this.DisposedAlready)
			{
				throw new ObjectDisposedException("StreamWriter");
			}
			if (value != null)
			{
				this.LowLevelWrite(value);
			}
			if (this.iflush)
			{
				this.Flush();
			}
		}

		/// <summary>Closes the current StreamWriter object and the underlying stream.</summary>
		/// <exception cref="T:System.Text.EncoderFallbackException">The current encoding does not support displaying half of a Unicode surrogate pair.</exception>
		/// <filterpriority>1</filterpriority>
		public override void Close()
		{
			this.Dispose(true);
		}

		/// <summary>Frees the resources of the current <see cref="T:System.IO.StreamWriter" /> before it is reclaimed by the garbage collector.</summary>
		~StreamWriter()
		{
			this.Dispose(false);
		}
	}
}
