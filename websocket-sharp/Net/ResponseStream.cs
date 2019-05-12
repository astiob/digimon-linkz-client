using System;
using System.IO;
using System.Text;

namespace WebSocketSharp.Net
{
	internal class ResponseStream : Stream
	{
		private static byte[] _crlf = new byte[]
		{
			13,
			10
		};

		private bool _disposed;

		private bool _ignoreErrors;

		private HttpListenerResponse _response;

		private Stream _stream;

		private bool _trailerSent;

		internal ResponseStream(Stream stream, HttpListenerResponse response, bool ignoreErrors)
		{
			this._stream = stream;
			this._response = response;
			this._ignoreErrors = ignoreErrors;
		}

		public override bool CanRead
		{
			get
			{
				return false;
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
				return true;
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

		private static byte[] getChunkSizeBytes(int size, bool final)
		{
			return Encoding.ASCII.GetBytes(string.Format("{0:x}\r\n{1}", size, (!final) ? "" : "\r\n"));
		}

		private MemoryStream getHeaders(bool closing)
		{
			if (this._response.HeadersSent)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			this._response.SendHeaders(closing, memoryStream);
			return memoryStream;
		}

		internal void InternalWrite(byte[] buffer, int offset, int count)
		{
			if (this._ignoreErrors)
			{
				try
				{
					this._stream.Write(buffer, offset, count);
				}
				catch
				{
				}
			}
			else
			{
				this._stream.Write(buffer, offset, count);
			}
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException();
		}

		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			MemoryStream headers = this.getHeaders(false);
			bool sendChunked = this._response.SendChunked;
			if (headers != null)
			{
				long position = headers.Position;
				headers.Position = headers.Length;
				if (sendChunked)
				{
					byte[] chunkSizeBytes = ResponseStream.getChunkSizeBytes(count, false);
					headers.Write(chunkSizeBytes, 0, chunkSizeBytes.Length);
				}
				headers.Write(buffer, offset, count);
				buffer = headers.GetBuffer();
				offset = (int)position;
				count = (int)(headers.Position - position);
			}
			else if (sendChunked)
			{
				byte[] chunkSizeBytes = ResponseStream.getChunkSizeBytes(count, false);
				this.InternalWrite(chunkSizeBytes, 0, chunkSizeBytes.Length);
			}
			return this._stream.BeginWrite(buffer, offset, count, callback, state);
		}

		public override void Close()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			MemoryStream headers = this.getHeaders(true);
			bool sendChunked = this._response.SendChunked;
			if (headers != null)
			{
				long position = headers.Position;
				if (sendChunked && !this._trailerSent)
				{
					byte[] chunkSizeBytes = ResponseStream.getChunkSizeBytes(0, true);
					headers.Position = headers.Length;
					headers.Write(chunkSizeBytes, 0, chunkSizeBytes.Length);
				}
				this.InternalWrite(headers.GetBuffer(), (int)position, (int)(headers.Length - position));
				this._trailerSent = true;
			}
			else if (sendChunked && !this._trailerSent)
			{
				byte[] chunkSizeBytes = ResponseStream.getChunkSizeBytes(0, true);
				this.InternalWrite(chunkSizeBytes, 0, chunkSizeBytes.Length);
				this._trailerSent = true;
			}
			this._response.Close();
		}

		public override int EndRead(IAsyncResult asyncResult)
		{
			throw new NotSupportedException();
		}

		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			Action<IAsyncResult> action = delegate(IAsyncResult ares)
			{
				this._stream.EndWrite(ares);
				if (this._response.SendChunked)
				{
					this._stream.Write(ResponseStream._crlf, 0, 2);
				}
			};
			if (this._ignoreErrors)
			{
				try
				{
					action(asyncResult);
				}
				catch
				{
				}
			}
			else
			{
				action(asyncResult);
			}
		}

		public override void Flush()
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
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
			if (this._disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			MemoryStream headers = this.getHeaders(false);
			bool sendChunked = this._response.SendChunked;
			if (headers != null)
			{
				long position = headers.Position;
				headers.Position = headers.Length;
				if (sendChunked)
				{
					byte[] chunkSizeBytes = ResponseStream.getChunkSizeBytes(count, false);
					headers.Write(chunkSizeBytes, 0, chunkSizeBytes.Length);
				}
				int num = Math.Min(count, 16384 - (int)headers.Position + (int)position);
				headers.Write(buffer, offset, num);
				count -= num;
				offset += num;
				this.InternalWrite(headers.GetBuffer(), (int)position, (int)(headers.Length - position));
				headers.SetLength(0L);
				headers.Capacity = 0;
			}
			else if (sendChunked)
			{
				byte[] chunkSizeBytes = ResponseStream.getChunkSizeBytes(count, false);
				this.InternalWrite(chunkSizeBytes, 0, chunkSizeBytes.Length);
			}
			if (count > 0)
			{
				this.InternalWrite(buffer, offset, count);
			}
			if (sendChunked)
			{
				this.InternalWrite(ResponseStream._crlf, 0, 2);
			}
		}
	}
}
