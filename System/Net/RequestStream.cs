using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Net
{
	internal class RequestStream : Stream
	{
		private byte[] buffer;

		private int offset;

		private int length;

		private long remaining_body;

		private bool disposed;

		private Stream stream;

		internal RequestStream(Stream stream, byte[] buffer, int offset, int length) : this(stream, buffer, offset, length, -1L)
		{
		}

		internal RequestStream(Stream stream, byte[] buffer, int offset, int length, long contentlength)
		{
			this.stream = stream;
			this.buffer = buffer;
			this.offset = offset;
			this.length = length;
			this.remaining_body = contentlength;
		}

		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public override void Close()
		{
			this.disposed = true;
		}

		public override void Flush()
		{
		}

		private int FillFromBuffer(byte[] buffer, int off, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (off < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "< 0");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "< 0");
			}
			int num = buffer.Length;
			if (off > num)
			{
				throw new ArgumentException("destination offset is beyond array size");
			}
			if (off > num - count)
			{
				throw new ArgumentException("Reading would overrun buffer");
			}
			if (this.remaining_body == 0L)
			{
				return -1;
			}
			if (this.length == 0)
			{
				return 0;
			}
			int num2 = Math.Min(this.length, count);
			if (this.remaining_body > 0L)
			{
				num2 = (int)Math.Min((long)num2, this.remaining_body);
			}
			if (this.offset > this.buffer.Length - num2)
			{
				num2 = Math.Min(num2, this.buffer.Length - this.offset);
			}
			if (num2 == 0)
			{
				return 0;
			}
			Buffer.BlockCopy(this.buffer, this.offset, buffer, off, num2);
			this.offset += num2;
			this.length -= num2;
			if (this.remaining_body > 0L)
			{
				this.remaining_body -= (long)num2;
			}
			return num2;
		}

		public override int Read([In] [Out] byte[] buffer, int offset, int count)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(RequestStream).ToString());
			}
			int num = this.FillFromBuffer(buffer, offset, count);
			if (num == -1)
			{
				return 0;
			}
			if (num > 0)
			{
				return num;
			}
			num = this.stream.Read(buffer, offset, count);
			if (num > 0 && this.remaining_body > 0L)
			{
				this.remaining_body -= (long)num;
			}
			return num;
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback cback, object state)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(RequestStream).ToString());
			}
			int num = this.FillFromBuffer(buffer, offset, count);
			if (num > 0 || num == -1)
			{
				HttpStreamAsyncResult httpStreamAsyncResult = new HttpStreamAsyncResult();
				httpStreamAsyncResult.Buffer = buffer;
				httpStreamAsyncResult.Offset = offset;
				httpStreamAsyncResult.Count = count;
				httpStreamAsyncResult.Callback = cback;
				httpStreamAsyncResult.State = state;
				httpStreamAsyncResult.SynchRead = num;
				httpStreamAsyncResult.Complete();
				return httpStreamAsyncResult;
			}
			if (this.remaining_body >= 0L && (long)count > this.remaining_body)
			{
				count = (int)Math.Min(2147483647L, this.remaining_body);
			}
			return this.stream.BeginRead(buffer, offset, count, cback, state);
		}

		public override int EndRead(IAsyncResult ares)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(typeof(RequestStream).ToString());
			}
			if (ares == null)
			{
				throw new ArgumentNullException("async_result");
			}
			if (ares is HttpStreamAsyncResult)
			{
				HttpStreamAsyncResult httpStreamAsyncResult = (HttpStreamAsyncResult)ares;
				if (!ares.IsCompleted)
				{
					ares.AsyncWaitHandle.WaitOne();
				}
				return httpStreamAsyncResult.SynchRead;
			}
			int num = this.stream.EndRead(ares);
			if (this.remaining_body > 0L && num > 0)
			{
				this.remaining_body -= (long)num;
			}
			return num;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback cback, object state)
		{
			throw new NotSupportedException();
		}

		public override void EndWrite(IAsyncResult async_result)
		{
			throw new NotSupportedException();
		}
	}
}
