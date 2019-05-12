using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Net
{
	internal class ResponseStream : Stream
	{
		private HttpListenerResponse response;

		private bool ignore_errors;

		private bool disposed;

		private bool trailer_sent;

		private Stream stream;

		private static byte[] crlf = new byte[]
		{
			13,
			10
		};

		internal ResponseStream(Stream stream, HttpListenerResponse response, bool ignore_errors)
		{
			this.response = response;
			this.ignore_errors = ignore_errors;
			this.stream = stream;
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

		public override void Close()
		{
			if (!this.disposed)
			{
				this.disposed = true;
				MemoryStream headers = this.GetHeaders(true);
				bool sendChunked = this.response.SendChunked;
				if (headers != null)
				{
					long position = headers.Position;
					if (sendChunked && !this.trailer_sent)
					{
						byte[] chunkSizeBytes = ResponseStream.GetChunkSizeBytes(0, true);
						headers.Position = headers.Length;
						headers.Write(chunkSizeBytes, 0, chunkSizeBytes.Length);
					}
					this.InternalWrite(headers.GetBuffer(), (int)position, (int)(headers.Length - position));
					this.trailer_sent = true;
				}
				else if (sendChunked && !this.trailer_sent)
				{
					byte[] chunkSizeBytes = ResponseStream.GetChunkSizeBytes(0, true);
					this.InternalWrite(chunkSizeBytes, 0, chunkSizeBytes.Length);
					this.trailer_sent = true;
				}
				this.response.Close();
			}
		}

		private MemoryStream GetHeaders(bool closing)
		{
			if (this.response.HeadersSent)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			this.response.SendHeaders(closing, memoryStream);
			return memoryStream;
		}

		public override void Flush()
		{
		}

		private static byte[] GetChunkSizeBytes(int size, bool final)
		{
			string s = string.Format("{0:x}\r\n{1}", size, (!final) ? string.Empty : "\r\n");
			return Encoding.ASCII.GetBytes(s);
		}

		internal void InternalWrite(byte[] buffer, int offset, int count)
		{
			if (this.ignore_errors)
			{
				try
				{
					this.stream.Write(buffer, offset, count);
				}
				catch
				{
				}
			}
			else
			{
				this.stream.Write(buffer, offset, count);
			}
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			MemoryStream headers = this.GetHeaders(false);
			bool sendChunked = this.response.SendChunked;
			if (headers != null)
			{
				long position = headers.Position;
				headers.Position = headers.Length;
				if (sendChunked)
				{
					byte[] chunkSizeBytes = ResponseStream.GetChunkSizeBytes(count, false);
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
				byte[] chunkSizeBytes = ResponseStream.GetChunkSizeBytes(count, false);
				this.InternalWrite(chunkSizeBytes, 0, chunkSizeBytes.Length);
			}
			if (count > 0)
			{
				this.InternalWrite(buffer, offset, count);
			}
			if (sendChunked)
			{
				this.InternalWrite(ResponseStream.crlf, 0, 2);
			}
		}

		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback cback, object state)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			MemoryStream headers = this.GetHeaders(false);
			bool sendChunked = this.response.SendChunked;
			if (headers != null)
			{
				long position = headers.Position;
				headers.Position = headers.Length;
				if (sendChunked)
				{
					byte[] chunkSizeBytes = ResponseStream.GetChunkSizeBytes(count, false);
					headers.Write(chunkSizeBytes, 0, chunkSizeBytes.Length);
				}
				headers.Write(buffer, offset, count);
				buffer = headers.GetBuffer();
				offset = (int)position;
				count = (int)(headers.Position - position);
			}
			else if (sendChunked)
			{
				byte[] chunkSizeBytes = ResponseStream.GetChunkSizeBytes(count, false);
				this.InternalWrite(chunkSizeBytes, 0, chunkSizeBytes.Length);
			}
			return this.stream.BeginWrite(buffer, offset, count, cback, state);
		}

		public override void EndWrite(IAsyncResult ares)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			if (this.ignore_errors)
			{
				try
				{
					this.stream.EndWrite(ares);
					if (this.response.SendChunked)
					{
						this.stream.Write(ResponseStream.crlf, 0, 2);
					}
				}
				catch
				{
				}
			}
			else
			{
				this.stream.EndWrite(ares);
				if (this.response.SendChunked)
				{
					this.stream.Write(ResponseStream.crlf, 0, 2);
				}
			}
		}

		public override int Read([In] [Out] byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback cback, object state)
		{
			throw new NotSupportedException();
		}

		public override int EndRead(IAsyncResult ares)
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
	}
}
