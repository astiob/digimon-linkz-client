using System;
using System.IO;

namespace WebSocketSharp.Net
{
	internal class RequestStream : Stream
	{
		private byte[] _buffer;

		private bool _disposed;

		private int _length;

		private int _offset;

		private long _remainingBody;

		private Stream _stream;

		internal RequestStream(Stream stream, byte[] buffer, int offset, int length) : this(stream, buffer, offset, length, -1L)
		{
		}

		internal RequestStream(Stream stream, byte[] buffer, int offset, int length, long contentlength)
		{
			this._stream = stream;
			this._buffer = buffer;
			this._offset = offset;
			this._length = length;
			this._remainingBody = contentlength;
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

		private int fillFromBuffer(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Less than zero.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Less than zero.");
			}
			int num = buffer.Length;
			if (offset > num)
			{
				throw new ArgumentException("'offset' is greater than 'buffer' size.");
			}
			if (offset > num - count)
			{
				throw new ArgumentException("Reading would overrun 'buffer'.");
			}
			if (this._remainingBody == 0L)
			{
				return -1;
			}
			if (this._length == 0)
			{
				return 0;
			}
			int num2 = (this._length >= count) ? count : this._length;
			if (this._remainingBody > 0L && this._remainingBody < (long)num2)
			{
				num2 = (int)this._remainingBody;
			}
			int num3 = this._buffer.Length - this._offset;
			if (num3 < num2)
			{
				num2 = num3;
			}
			if (num2 == 0)
			{
				return 0;
			}
			Buffer.BlockCopy(this._buffer, this._offset, buffer, offset, num2);
			this._offset += num2;
			this._length -= num2;
			if (this._remainingBody > 0L)
			{
				this._remainingBody -= (long)num2;
			}
			return num2;
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			int num = this.fillFromBuffer(buffer, offset, count);
			if (num > 0 || num == -1)
			{
				HttpStreamAsyncResult httpStreamAsyncResult = new HttpStreamAsyncResult();
				httpStreamAsyncResult.Buffer = buffer;
				httpStreamAsyncResult.Offset = offset;
				httpStreamAsyncResult.Count = count;
				httpStreamAsyncResult.Callback = callback;
				httpStreamAsyncResult.State = state;
				httpStreamAsyncResult.SyncRead = num;
				httpStreamAsyncResult.Complete();
				return httpStreamAsyncResult;
			}
			if (this._remainingBody >= 0L && this._remainingBody < (long)count)
			{
				count = (int)this._remainingBody;
			}
			return this._stream.BeginRead(buffer, offset, count, callback, state);
		}

		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException();
		}

		public override void Close()
		{
			this._disposed = true;
		}

		public override int EndRead(IAsyncResult asyncResult)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (asyncResult is HttpStreamAsyncResult)
			{
				HttpStreamAsyncResult httpStreamAsyncResult = (HttpStreamAsyncResult)asyncResult;
				if (!httpStreamAsyncResult.IsCompleted)
				{
					httpStreamAsyncResult.AsyncWaitHandle.WaitOne();
				}
				return httpStreamAsyncResult.SyncRead;
			}
			int num = this._stream.EndRead(asyncResult);
			if (num > 0 && this._remainingBody > 0L)
			{
				this._remainingBody -= (long)num;
			}
			return num;
		}

		public override void EndWrite(IAsyncResult asyncResult)
		{
			throw new NotSupportedException();
		}

		public override void Flush()
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			int num = this.fillFromBuffer(buffer, offset, count);
			if (num == -1)
			{
				return 0;
			}
			if (num > 0)
			{
				return num;
			}
			num = this._stream.Read(buffer, offset, count);
			if (num > 0 && this._remainingBody > 0L)
			{
				this._remainingBody -= (long)num;
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
	}
}
