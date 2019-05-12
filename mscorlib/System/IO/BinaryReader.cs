using Mono.Security;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO
{
	/// <summary>Reads primitive data types as binary values in a specific encoding.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public class BinaryReader : IDisposable
	{
		private const int MaxBufferSize = 128;

		private Stream m_stream;

		private Encoding m_encoding;

		private byte[] m_buffer;

		private Decoder decoder;

		private char[] charBuffer;

		private bool m_disposed;

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.BinaryReader" /> class based on the supplied stream and using <see cref="T:System.Text.UTF8Encoding" />.</summary>
		/// <param name="input">A stream. </param>
		/// <exception cref="T:System.ArgumentException">The stream does not support reading, the stream is null, or the stream is already closed. </exception>
		public BinaryReader(Stream input) : this(input, Encoding.UTF8UnmarkedUnsafe)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.BinaryReader" /> class based on the supplied stream and a specific character encoding.</summary>
		/// <param name="input">The supplied stream. </param>
		/// <param name="encoding">The character encoding. </param>
		/// <exception cref="T:System.ArgumentException">The stream does not support reading, the stream is null, or the stream is already closed. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="encoding" /> is null. </exception>
		public BinaryReader(Stream input, Encoding encoding)
		{
			if (input == null || encoding == null)
			{
				throw new ArgumentNullException(Locale.GetText("Input or Encoding is a null reference."));
			}
			if (!input.CanRead)
			{
				throw new ArgumentException(Locale.GetText("The stream doesn't support reading."));
			}
			this.m_stream = input;
			this.m_encoding = encoding;
			this.decoder = encoding.GetDecoder();
			this.m_buffer = new byte[32];
		}

		/// <summary>Releases all resources used by the <see cref="T:System.IO.BinaryWriter" />.</summary>
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>Exposes access to the underlying stream of the <see cref="T:System.IO.BinaryReader" />.</summary>
		/// <returns>The underlying stream associated with the BinaryReader.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual Stream BaseStream
		{
			get
			{
				return this.m_stream;
			}
		}

		/// <summary>Closes the current reader and the underlying stream.</summary>
		/// <filterpriority>2</filterpriority>
		public virtual void Close()
		{
			this.Dispose(true);
			this.m_disposed = true;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.BinaryReader" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.m_stream != null)
			{
				this.m_stream.Close();
			}
			this.m_disposed = true;
			this.m_buffer = null;
			this.m_encoding = null;
			this.m_stream = null;
			this.charBuffer = null;
		}

		/// <summary>Fills the internal buffer with the specified number of bytes read from the stream.</summary>
		/// <param name="numBytes">The number of bytes to be read. </param>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached before <paramref name="numBytes" /> could be read. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Requested <paramref name="numBytes" /> is larger than the internal buffer size.</exception>
		protected virtual void FillBuffer(int numBytes)
		{
			if (this.m_disposed)
			{
				throw new ObjectDisposedException("BinaryReader", "Cannot read from a closed BinaryReader.");
			}
			if (this.m_stream == null)
			{
				throw new IOException("Stream is invalid");
			}
			this.CheckBuffer(numBytes);
			int num;
			for (int i = 0; i < numBytes; i += num)
			{
				num = this.m_stream.Read(this.m_buffer, i, numBytes - i);
				if (num == 0)
				{
					throw new EndOfStreamException();
				}
			}
		}

		/// <summary>Returns the next available character and does not advance the byte or character position.</summary>
		/// <returns>The next available character, or -1 if no more characters are available or the stream does not support seeking.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ArgumentException">The current character cannot be decoded into the internal character buffer by using the <see cref="T:System.Text.Encoding" /> selected for the stream.</exception>
		/// <filterpriority>2</filterpriority>
		public virtual int PeekChar()
		{
			if (this.m_stream == null)
			{
				if (this.m_disposed)
				{
					throw new ObjectDisposedException("BinaryReader", "Cannot read from a closed BinaryReader.");
				}
				throw new IOException("Stream is invalid");
			}
			else
			{
				if (!this.m_stream.CanSeek)
				{
					return -1;
				}
				char[] array = new char[1];
				int num2;
				int num = this.ReadCharBytes(array, 0, 1, out num2);
				this.m_stream.Position -= (long)num2;
				if (num == 0)
				{
					return -1;
				}
				return (int)array[0];
			}
		}

		/// <summary>Reads characters from the underlying stream and advances the current position of the stream in accordance with the Encoding used and the specific character being read from the stream.</summary>
		/// <returns>The next character from the input stream, or -1 if no characters are currently available.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual int Read()
		{
			if (this.charBuffer == null)
			{
				this.charBuffer = new char[128];
			}
			if (this.Read(this.charBuffer, 0, 1) == 0)
			{
				return -1;
			}
			return (int)this.charBuffer[0];
		}

		/// <summary>Reads <paramref name="count" /> bytes from the stream with <paramref name="index" /> as the starting point in the byte array.</summary>
		/// <returns>The number of characters read into <paramref name="buffer" />. This might be less than the number of bytes requested if that many bytes are not available, or it might be zero if the end of the stream is reached.</returns>
		/// <param name="buffer">The buffer to read data into. </param>
		/// <param name="index">The starting point in the buffer at which to begin reading into the buffer. </param>
		/// <param name="count">The number of characters to read. </param>
		/// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />. -or-The number of decoded characters to read is greater than <paramref name="count" />. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual int Read(byte[] buffer, int index, int count)
		{
			if (this.m_stream == null)
			{
				if (this.m_disposed)
				{
					throw new ObjectDisposedException("BinaryReader", "Cannot read from a closed BinaryReader.");
				}
				throw new IOException("Stream is invalid");
			}
			else
			{
				if (buffer == null)
				{
					throw new ArgumentNullException("buffer is null");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index is less than 0");
				}
				if (count < 0)
				{
					throw new ArgumentOutOfRangeException("count is less than 0");
				}
				if (buffer.Length - index < count)
				{
					throw new ArgumentException("buffer is too small");
				}
				return this.m_stream.Read(buffer, index, count);
			}
		}

		/// <summary>Reads <paramref name="count" /> characters from the stream with <paramref name="index" /> as the starting point in the character array.</summary>
		/// <returns>The total number of characters read into the buffer. This might be less than the number of characters requested if that many characters are not currently available, or it might be zero if the end of the stream is reached.</returns>
		/// <param name="buffer">The buffer to read data into. </param>
		/// <param name="index">The starting point in the buffer at which to begin reading into the buffer. </param>
		/// <param name="count">The number of characters to read. </param>
		/// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />. -or-The number of decoded characters to read is greater than <paramref name="count" />. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual int Read(char[] buffer, int index, int count)
		{
			if (this.m_stream == null)
			{
				if (this.m_disposed)
				{
					throw new ObjectDisposedException("BinaryReader", "Cannot read from a closed BinaryReader.");
				}
				throw new IOException("Stream is invalid");
			}
			else
			{
				if (buffer == null)
				{
					throw new ArgumentNullException("buffer is null");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index is less than 0");
				}
				if (count < 0)
				{
					throw new ArgumentOutOfRangeException("count is less than 0");
				}
				if (buffer.Length - index < count)
				{
					throw new ArgumentException("buffer is too small");
				}
				int num;
				return this.ReadCharBytes(buffer, index, count, out num);
			}
		}

		private int ReadCharBytes(char[] buffer, int index, int count, out int bytes_read)
		{
			int i = 0;
			bytes_read = 0;
			while (i < count)
			{
				int num = 0;
				int chars;
				do
				{
					this.CheckBuffer(num + 1);
					int num2 = this.m_stream.ReadByte();
					if (num2 == -1)
					{
						return i;
					}
					this.m_buffer[num++] = (byte)num2;
					bytes_read++;
					chars = this.m_encoding.GetChars(this.m_buffer, 0, num, buffer, index + i);
				}
				while (chars <= 0);
				i++;
			}
			return i;
		}

		/// <summary>Reads in a 32-bit integer in compressed format.</summary>
		/// <returns>A 32-bit integer in compressed format.</returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.FormatException">The stream is corrupted.</exception>
		protected int Read7BitEncodedInt()
		{
			int num = 0;
			int num2 = 0;
			int i;
			for (i = 0; i < 5; i++)
			{
				byte b = this.ReadByte();
				num |= (int)(b & 127) << num2;
				num2 += 7;
				if ((b & 128) == 0)
				{
					break;
				}
			}
			if (i < 5)
			{
				return num;
			}
			throw new FormatException("Too many bytes in what should have been a 7 bit encoded Int32.");
		}

		/// <summary>Reads a Boolean value from the current stream and advances the current position of the stream by one byte.</summary>
		/// <returns>true if the byte is nonzero; otherwise, false.</returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual bool ReadBoolean()
		{
			return this.ReadByte() != 0;
		}

		/// <summary>Reads the next byte from the current stream and advances the current position of the stream by one byte.</summary>
		/// <returns>The next byte read from the current stream.</returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual byte ReadByte()
		{
			if (this.m_stream == null)
			{
				if (this.m_disposed)
				{
					throw new ObjectDisposedException("BinaryReader", "Cannot read from a closed BinaryReader.");
				}
				throw new IOException("Stream is invalid");
			}
			else
			{
				int num = this.m_stream.ReadByte();
				if (num != -1)
				{
					return (byte)num;
				}
				throw new EndOfStreamException();
			}
		}

		/// <summary>Reads <paramref name="count" /> bytes from the current stream into a byte array and advances the current position by <paramref name="count" /> bytes.</summary>
		/// <returns>A byte array containing data read from the underlying stream. This might be less than the number of bytes requested if the end of the stream is reached.</returns>
		/// <param name="count">The number of bytes to read. </param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is negative. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual byte[] ReadBytes(int count)
		{
			if (this.m_stream == null)
			{
				if (this.m_disposed)
				{
					throw new ObjectDisposedException("BinaryReader", "Cannot read from a closed BinaryReader.");
				}
				throw new IOException("Stream is invalid");
			}
			else
			{
				if (count < 0)
				{
					throw new ArgumentOutOfRangeException("count is less than 0");
				}
				byte[] array = new byte[count];
				int i;
				int num;
				for (i = 0; i < count; i += num)
				{
					num = this.m_stream.Read(array, i, count - i);
					if (num == 0)
					{
						break;
					}
				}
				if (i != count)
				{
					byte[] array2 = new byte[i];
					Buffer.BlockCopyInternal(array, 0, array2, 0, i);
					return array2;
				}
				return array;
			}
		}

		/// <summary>Reads the next character from the current stream and advances the current position of the stream in accordance with the Encoding used and the specific character being read from the stream.</summary>
		/// <returns>A character read from the current stream.</returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ArgumentException">A surrogate character was read. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual char ReadChar()
		{
			int num = this.Read();
			if (num == -1)
			{
				throw new EndOfStreamException();
			}
			return (char)num;
		}

		/// <summary>Reads <paramref name="count" /> characters from the current stream, returns the data in a character array, and advances the current position in accordance with the Encoding used and the specific character being read from the stream.</summary>
		/// <returns>A character array containing data read from the underlying stream. This might be less than the number of characters requested if the end of the stream is reached.</returns>
		/// <param name="count">The number of characters to read. </param>
		/// <exception cref="T:System.ArgumentException">The number of decoded characters to read is greater than <paramref name="count" />. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is negative. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual char[] ReadChars(int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count is less than 0");
			}
			if (count == 0)
			{
				return new char[0];
			}
			char[] array = new char[count];
			int num = this.Read(array, 0, count);
			if (num == 0)
			{
				throw new EndOfStreamException();
			}
			if (num != array.Length)
			{
				char[] array2 = new char[num];
				Array.Copy(array, 0, array2, 0, num);
				return array2;
			}
			return array;
		}

		/// <summary>Reads a decimal value from the current stream and advances the current position of the stream by sixteen bytes.</summary>
		/// <returns>A decimal value read from the current stream.</returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public unsafe virtual decimal ReadDecimal()
		{
			this.FillBuffer(16);
			decimal result;
			if (BitConverter.IsLittleEndian)
			{
				for (int i = 0; i < 16; i++)
				{
					if (i < 4)
					{
						*(ref result + (i + 8)) = this.m_buffer[i];
					}
					else if (i < 8)
					{
						*(ref result + (i + 8)) = this.m_buffer[i];
					}
					else if (i < 12)
					{
						*(ref result + (i - 4)) = this.m_buffer[i];
					}
					else if (i < 16)
					{
						*(ref result + (i - 12)) = this.m_buffer[i];
					}
				}
			}
			else
			{
				for (int j = 0; j < 16; j++)
				{
					if (j < 4)
					{
						*(ref result + (11 - j)) = this.m_buffer[j];
					}
					else if (j < 8)
					{
						*(ref result + (19 - j)) = this.m_buffer[j];
					}
					else if (j < 12)
					{
						*(ref result + (15 - j)) = this.m_buffer[j];
					}
					else if (j < 16)
					{
						*(ref result + (15 - j)) = this.m_buffer[j];
					}
				}
			}
			return result;
		}

		/// <summary>Reads an 8-byte floating point value from the current stream and advances the current position of the stream by eight bytes.</summary>
		/// <returns>An 8-byte floating point value read from the current stream.</returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual double ReadDouble()
		{
			this.FillBuffer(8);
			return BitConverterLE.ToDouble(this.m_buffer, 0);
		}

		/// <summary>Reads a 2-byte signed integer from the current stream and advances the current position of the stream by two bytes.</summary>
		/// <returns>A 2-byte signed integer read from the current stream.</returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual short ReadInt16()
		{
			this.FillBuffer(2);
			return (short)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8);
		}

		/// <summary>Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.</summary>
		/// <returns>A 4-byte signed integer read from the current stream.</returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual int ReadInt32()
		{
			this.FillBuffer(4);
			return (int)this.m_buffer[0] | (int)this.m_buffer[1] << 8 | (int)this.m_buffer[2] << 16 | (int)this.m_buffer[3] << 24;
		}

		/// <summary>Reads an 8-byte signed integer from the current stream and advances the current position of the stream by eight bytes.</summary>
		/// <returns>An 8-byte signed integer read from the current stream.</returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual long ReadInt64()
		{
			this.FillBuffer(8);
			uint num = (uint)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8 | (int)this.m_buffer[2] << 16 | (int)this.m_buffer[3] << 24);
			uint num2 = (uint)((int)this.m_buffer[4] | (int)this.m_buffer[5] << 8 | (int)this.m_buffer[6] << 16 | (int)this.m_buffer[7] << 24);
			return (long)((ulong)num2 << 32 | (ulong)num);
		}

		/// <summary>Reads a signed byte from this stream and advances the current position of the stream by one byte.</summary>
		/// <returns>A signed byte read from the current stream.</returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		[CLSCompliant(false)]
		public virtual sbyte ReadSByte()
		{
			return (sbyte)this.ReadByte();
		}

		/// <summary>Reads a string from the current stream. The string is prefixed with the length, encoded as an integer seven bits at a time.</summary>
		/// <returns>The string being read.</returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual string ReadString()
		{
			int num = this.Read7BitEncodedInt();
			if (num < 0)
			{
				throw new IOException("Invalid binary file (string len < 0)");
			}
			if (num == 0)
			{
				return string.Empty;
			}
			if (this.charBuffer == null)
			{
				this.charBuffer = new char[128];
			}
			StringBuilder stringBuilder = null;
			int chars;
			for (;;)
			{
				int num2 = (num <= 128) ? num : 128;
				this.FillBuffer(num2);
				chars = this.decoder.GetChars(this.m_buffer, 0, num2, this.charBuffer, 0);
				if (stringBuilder == null && num2 == num)
				{
					break;
				}
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(num);
				}
				stringBuilder.Append(this.charBuffer, 0, chars);
				num -= num2;
				if (num <= 0)
				{
					goto Block_8;
				}
			}
			return new string(this.charBuffer, 0, chars);
			Block_8:
			return stringBuilder.ToString();
		}

		/// <summary>Reads a 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.</summary>
		/// <returns>A 4-byte floating point value read from the current stream.</returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual float ReadSingle()
		{
			this.FillBuffer(4);
			return BitConverterLE.ToSingle(this.m_buffer, 0);
		}

		/// <summary>Reads a 2-byte unsigned integer from the current stream using little-endian encoding and advances the position of the stream by two bytes.</summary>
		/// <returns>A 2-byte unsigned integer read from this stream.</returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		[CLSCompliant(false)]
		public virtual ushort ReadUInt16()
		{
			this.FillBuffer(2);
			return (ushort)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8);
		}

		/// <summary>Reads a 4-byte unsigned integer from the current stream and advances the position of the stream by four bytes.</summary>
		/// <returns>A 4-byte unsigned integer read from this stream.</returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <filterpriority>2</filterpriority>
		[CLSCompliant(false)]
		public virtual uint ReadUInt32()
		{
			this.FillBuffer(4);
			return (uint)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8 | (int)this.m_buffer[2] << 16 | (int)this.m_buffer[3] << 24);
		}

		/// <summary>Reads an 8-byte unsigned integer from the current stream and advances the position of the stream by eight bytes.</summary>
		/// <returns>An 8-byte unsigned integer read from this stream.</returns>
		/// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
		/// <filterpriority>2</filterpriority>
		[CLSCompliant(false)]
		public virtual ulong ReadUInt64()
		{
			this.FillBuffer(8);
			uint num = (uint)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8 | (int)this.m_buffer[2] << 16 | (int)this.m_buffer[3] << 24);
			uint num2 = (uint)((int)this.m_buffer[4] | (int)this.m_buffer[5] << 8 | (int)this.m_buffer[6] << 16 | (int)this.m_buffer[7] << 24);
			return (ulong)num2 << 32 | (ulong)num;
		}

		private void CheckBuffer(int length)
		{
			if (this.m_buffer.Length <= length)
			{
				byte[] array = new byte[length];
				Buffer.BlockCopyInternal(this.m_buffer, 0, array, 0, this.m_buffer.Length);
				this.m_buffer = array;
			}
		}
	}
}
