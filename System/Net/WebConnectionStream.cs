using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace System.Net
{
	internal class WebConnectionStream : Stream
	{
		private static byte[] crlf = new byte[]
		{
			13,
			10
		};

		private bool isRead;

		private WebConnection cnc;

		private HttpWebRequest request;

		private byte[] readBuffer;

		private int readBufferOffset;

		private int readBufferSize;

		private int contentLength;

		private int totalRead;

		private long totalWritten;

		private bool nextReadCalled;

		private int pendingReads;

		private int pendingWrites;

		private ManualResetEvent pending;

		private bool allowBuffering;

		private bool sendChunked;

		private MemoryStream writeBuffer;

		private bool requestWritten;

		private byte[] headers;

		private bool disposed;

		private bool headersSent;

		private object locker = new object();

		private bool initRead;

		private bool read_eof;

		private bool complete_request_written;

		private int read_timeout;

		private int write_timeout;

		public WebConnectionStream(WebConnection cnc)
		{
			this.isRead = true;
			this.pending = new ManualResetEvent(true);
			this.request = cnc.Data.request;
			this.read_timeout = this.request.ReadWriteTimeout;
			this.write_timeout = this.read_timeout;
			this.cnc = cnc;
			string text = cnc.Data.Headers["Transfer-Encoding"];
			bool flag = text != null && text.ToLower().IndexOf("chunked") != -1;
			string text2 = cnc.Data.Headers["Content-Length"];
			if (!flag && text2 != null && text2 != string.Empty)
			{
				try
				{
					this.contentLength = int.Parse(text2);
					if (this.contentLength == 0 && !this.IsNtlmAuth())
					{
						this.ReadAll();
					}
				}
				catch
				{
					this.contentLength = int.MaxValue;
				}
			}
			else
			{
				this.contentLength = int.MaxValue;
			}
		}

		public WebConnectionStream(WebConnection cnc, HttpWebRequest request)
		{
			this.read_timeout = request.ReadWriteTimeout;
			this.write_timeout = this.read_timeout;
			this.isRead = false;
			this.cnc = cnc;
			this.request = request;
			this.allowBuffering = request.InternalAllowBuffering;
			this.sendChunked = request.SendChunked;
			if (this.sendChunked)
			{
				this.pending = new ManualResetEvent(true);
			}
			else if (this.allowBuffering)
			{
				this.writeBuffer = new MemoryStream();
			}
		}

		private bool IsNtlmAuth()
		{
			bool flag = this.request.Proxy != null && !this.request.Proxy.IsBypassed(this.request.Address);
			string name = (!flag) ? "WWW-Authenticate" : "Proxy-Authenticate";
			string text = this.cnc.Data.Headers[name];
			return text != null && text.IndexOf("NTLM") != -1;
		}

		internal void CheckResponseInBuffer()
		{
			if (this.contentLength > 0 && this.readBufferSize - this.readBufferOffset >= this.contentLength && !this.IsNtlmAuth())
			{
				this.ReadAll();
			}
		}

		internal HttpWebRequest Request
		{
			get
			{
				return this.request;
			}
		}

		internal WebConnection Connection
		{
			get
			{
				return this.cnc;
			}
		}

		public override bool CanTimeout
		{
			get
			{
				return true;
			}
		}

		public override int ReadTimeout
		{
			get
			{
				return this.read_timeout;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.read_timeout = value;
			}
		}

		public override int WriteTimeout
		{
			get
			{
				return this.write_timeout;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.write_timeout = value;
			}
		}

		internal bool CompleteRequestWritten
		{
			get
			{
				return this.complete_request_written;
			}
		}

		internal bool SendChunked
		{
			set
			{
				this.sendChunked = value;
			}
		}

		internal byte[] ReadBuffer
		{
			set
			{
				this.readBuffer = value;
			}
		}

		internal int ReadBufferOffset
		{
			set
			{
				this.readBufferOffset = value;
			}
		}

		internal int ReadBufferSize
		{
			set
			{
				this.readBufferSize = value;
			}
		}

		internal byte[] WriteBuffer
		{
			get
			{
				return this.writeBuffer.GetBuffer();
			}
		}

		internal int WriteBufferLength
		{
			get
			{
				return (this.writeBuffer == null) ? -1 : ((int)this.writeBuffer.Length);
			}
		}

		internal void ForceCompletion()
		{
			if (!this.nextReadCalled)
			{
				if (this.contentLength == 2147483647)
				{
					this.contentLength = 0;
				}
				this.nextReadCalled = true;
				this.cnc.NextRead();
			}
		}

		internal void CheckComplete()
		{
			if (!this.nextReadCalled && this.readBufferSize - this.readBufferOffset == this.contentLength)
			{
				this.nextReadCalled = true;
				this.cnc.NextRead();
			}
		}

		internal void ReadAll()
		{
			if (!this.isRead || this.read_eof || this.totalRead >= this.contentLength || this.nextReadCalled)
			{
				if (this.isRead && !this.nextReadCalled)
				{
					this.nextReadCalled = true;
					this.cnc.NextRead();
				}
				return;
			}
			this.pending.WaitOne();
			object obj = this.locker;
			lock (obj)
			{
				if (this.totalRead >= this.contentLength)
				{
					return;
				}
				int num = this.readBufferSize - this.readBufferOffset;
				byte[] array2;
				int num2;
				if (this.contentLength == 2147483647)
				{
					MemoryStream memoryStream = new MemoryStream();
					byte[] array = null;
					if (this.readBuffer != null && num > 0)
					{
						memoryStream.Write(this.readBuffer, this.readBufferOffset, num);
						if (this.readBufferSize >= 8192)
						{
							array = this.readBuffer;
						}
					}
					if (array == null)
					{
						array = new byte[8192];
					}
					int count;
					while ((count = this.cnc.Read(this.request, array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
					array2 = memoryStream.GetBuffer();
					num2 = (int)memoryStream.Length;
					this.contentLength = num2;
				}
				else
				{
					num2 = this.contentLength - this.totalRead;
					array2 = new byte[num2];
					if (this.readBuffer != null && num > 0)
					{
						if (num > num2)
						{
							num = num2;
						}
						Buffer.BlockCopy(this.readBuffer, this.readBufferOffset, array2, 0, num);
					}
					int num3 = num2 - num;
					int num4 = -1;
					while (num3 > 0 && num4 != 0)
					{
						num4 = this.cnc.Read(this.request, array2, num, num3);
						num3 -= num4;
						num += num4;
					}
				}
				this.readBuffer = array2;
				this.readBufferOffset = 0;
				this.readBufferSize = num2;
				this.totalRead = 0;
				this.nextReadCalled = true;
			}
			this.cnc.NextRead();
		}

		private void WriteCallbackWrapper(IAsyncResult r)
		{
			WebAsyncResult webAsyncResult = r as WebAsyncResult;
			if (webAsyncResult != null && webAsyncResult.AsyncWriteAll)
			{
				return;
			}
			if (r.AsyncState != null)
			{
				webAsyncResult = (WebAsyncResult)r.AsyncState;
				webAsyncResult.InnerAsyncResult = r;
				webAsyncResult.DoCallback();
			}
			else
			{
				this.EndWrite(r);
			}
		}

		private void ReadCallbackWrapper(IAsyncResult r)
		{
			if (r.AsyncState != null)
			{
				WebAsyncResult webAsyncResult = (WebAsyncResult)r.AsyncState;
				webAsyncResult.InnerAsyncResult = r;
				webAsyncResult.DoCallback();
			}
			else
			{
				this.EndRead(r);
			}
		}

		public override int Read(byte[] buffer, int offset, int size)
		{
			AsyncCallback cb = new AsyncCallback(this.ReadCallbackWrapper);
			WebAsyncResult webAsyncResult = (WebAsyncResult)this.BeginRead(buffer, offset, size, cb, null);
			if (!webAsyncResult.IsCompleted && !webAsyncResult.WaitUntilComplete(this.ReadTimeout, false))
			{
				this.nextReadCalled = true;
				this.cnc.Close(true);
				throw new WebException("The operation has timed out.", WebExceptionStatus.Timeout);
			}
			return this.EndRead(webAsyncResult);
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback cb, object state)
		{
			if (!this.isRead)
			{
				throw new NotSupportedException("this stream does not allow reading");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = buffer.Length;
			if (offset < 0 || num < offset)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || num - offset < size)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			object obj = this.locker;
			lock (obj)
			{
				this.pendingReads++;
				this.pending.Reset();
			}
			WebAsyncResult webAsyncResult = new WebAsyncResult(cb, state, buffer, offset, size);
			if (this.totalRead >= this.contentLength)
			{
				webAsyncResult.SetCompleted(true, -1);
				webAsyncResult.DoCallback();
				return webAsyncResult;
			}
			int num2 = this.readBufferSize - this.readBufferOffset;
			if (num2 > 0)
			{
				int num3 = (num2 <= size) ? num2 : size;
				Buffer.BlockCopy(this.readBuffer, this.readBufferOffset, buffer, offset, num3);
				this.readBufferOffset += num3;
				offset += num3;
				size -= num3;
				this.totalRead += num3;
				if (size == 0 || this.totalRead >= this.contentLength)
				{
					webAsyncResult.SetCompleted(true, num3);
					webAsyncResult.DoCallback();
					return webAsyncResult;
				}
				webAsyncResult.NBytes = num3;
			}
			if (cb != null)
			{
				cb = new AsyncCallback(this.ReadCallbackWrapper);
			}
			if (this.contentLength != 2147483647 && this.contentLength - this.totalRead < size)
			{
				size = this.contentLength - this.totalRead;
			}
			if (!this.read_eof)
			{
				webAsyncResult.InnerAsyncResult = this.cnc.BeginRead(this.request, buffer, offset, size, cb, webAsyncResult);
			}
			else
			{
				webAsyncResult.SetCompleted(true, webAsyncResult.NBytes);
				webAsyncResult.DoCallback();
			}
			return webAsyncResult;
		}

		public override int EndRead(IAsyncResult r)
		{
			WebAsyncResult webAsyncResult = (WebAsyncResult)r;
			if (webAsyncResult.EndCalled)
			{
				int nbytes = webAsyncResult.NBytes;
				return (nbytes < 0) ? 0 : nbytes;
			}
			webAsyncResult.EndCalled = true;
			if (!webAsyncResult.IsCompleted)
			{
				int num = -1;
				try
				{
					num = this.cnc.EndRead(this.request, webAsyncResult);
				}
				catch (Exception e)
				{
					object obj = this.locker;
					lock (obj)
					{
						this.pendingReads--;
						if (this.pendingReads == 0)
						{
							this.pending.Set();
						}
					}
					this.nextReadCalled = true;
					this.cnc.Close(true);
					webAsyncResult.SetCompleted(false, e);
					webAsyncResult.DoCallback();
					throw;
				}
				if (num < 0)
				{
					num = 0;
					this.read_eof = true;
				}
				this.totalRead += num;
				webAsyncResult.SetCompleted(false, num + webAsyncResult.NBytes);
				webAsyncResult.DoCallback();
				if (num == 0)
				{
					this.contentLength = this.totalRead;
				}
			}
			object obj2 = this.locker;
			lock (obj2)
			{
				this.pendingReads--;
				if (this.pendingReads == 0)
				{
					this.pending.Set();
				}
			}
			if (this.totalRead >= this.contentLength && !this.nextReadCalled)
			{
				this.ReadAll();
			}
			int nbytes2 = webAsyncResult.NBytes;
			return (nbytes2 < 0) ? 0 : nbytes2;
		}

		private void WriteRequestAsyncCB(IAsyncResult r)
		{
			WebAsyncResult webAsyncResult = (WebAsyncResult)r.AsyncState;
			try
			{
				this.cnc.EndWrite2(this.request, r);
				webAsyncResult.SetCompleted(false, 0);
				if (!this.initRead)
				{
					this.initRead = true;
					WebConnection.InitRead(this.cnc);
				}
			}
			catch (Exception ex)
			{
				this.KillBuffer();
				this.nextReadCalled = true;
				this.cnc.Close(true);
				if (ex is System.Net.Sockets.SocketException)
				{
					ex = new IOException("Error writing request", ex);
				}
				webAsyncResult.SetCompleted(false, ex);
			}
			this.complete_request_written = true;
			webAsyncResult.DoCallback();
		}

		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback cb, object state)
		{
			if (this.request.Aborted)
			{
				throw new WebException("The request was canceled.", null, WebExceptionStatus.RequestCanceled);
			}
			if (this.isRead)
			{
				throw new NotSupportedException("this stream does not allow writing");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = buffer.Length;
			if (offset < 0 || num < offset)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || num - offset < size)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (this.sendChunked)
			{
				object obj = this.locker;
				lock (obj)
				{
					this.pendingWrites++;
					this.pending.Reset();
				}
			}
			WebAsyncResult webAsyncResult = new WebAsyncResult(cb, state);
			if (!this.sendChunked)
			{
				this.CheckWriteOverflow(this.request.ContentLength, this.totalWritten, (long)size);
			}
			if (this.allowBuffering && !this.sendChunked)
			{
				if (this.writeBuffer == null)
				{
					this.writeBuffer = new MemoryStream();
				}
				this.writeBuffer.Write(buffer, offset, size);
				this.totalWritten += (long)size;
				if (this.request.ContentLength > 0L && this.totalWritten == this.request.ContentLength)
				{
					try
					{
						webAsyncResult.AsyncWriteAll = true;
						webAsyncResult.InnerAsyncResult = this.WriteRequestAsync(new AsyncCallback(this.WriteRequestAsyncCB), webAsyncResult);
						if (webAsyncResult.InnerAsyncResult == null)
						{
							if (!webAsyncResult.IsCompleted)
							{
								webAsyncResult.SetCompleted(true, 0);
							}
							webAsyncResult.DoCallback();
						}
					}
					catch (Exception e)
					{
						webAsyncResult.SetCompleted(true, e);
						webAsyncResult.DoCallback();
					}
				}
				else
				{
					webAsyncResult.SetCompleted(true, 0);
					webAsyncResult.DoCallback();
				}
				return webAsyncResult;
			}
			AsyncCallback cb2 = null;
			if (cb != null)
			{
				cb2 = new AsyncCallback(this.WriteCallbackWrapper);
			}
			if (this.sendChunked)
			{
				this.WriteRequest();
				string s = string.Format("{0:X}\r\n", size);
				byte[] bytes = Encoding.ASCII.GetBytes(s);
				int num2 = 2 + size + bytes.Length;
				byte[] array = new byte[num2];
				Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
				Buffer.BlockCopy(buffer, offset, array, bytes.Length, size);
				Buffer.BlockCopy(WebConnectionStream.crlf, 0, array, bytes.Length + size, WebConnectionStream.crlf.Length);
				buffer = array;
				offset = 0;
				size = num2;
			}
			webAsyncResult.InnerAsyncResult = this.cnc.BeginWrite(this.request, buffer, offset, size, cb2, webAsyncResult);
			this.totalWritten += (long)size;
			return webAsyncResult;
		}

		private void CheckWriteOverflow(long contentLength, long totalWritten, long size)
		{
			if (contentLength == -1L)
			{
				return;
			}
			long num = contentLength - totalWritten;
			if (size > num)
			{
				this.KillBuffer();
				this.nextReadCalled = true;
				this.cnc.Close(true);
				throw new ProtocolViolationException("The number of bytes to be written is greater than the specified ContentLength.");
			}
		}

		public override void EndWrite(IAsyncResult r)
		{
			if (r == null)
			{
				throw new ArgumentNullException("r");
			}
			WebAsyncResult webAsyncResult = r as WebAsyncResult;
			if (webAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult");
			}
			if (webAsyncResult.EndCalled)
			{
				return;
			}
			webAsyncResult.EndCalled = true;
			if (webAsyncResult.AsyncWriteAll)
			{
				webAsyncResult.WaitUntilComplete();
				if (webAsyncResult.GotException)
				{
					throw webAsyncResult.Exception;
				}
				return;
			}
			else
			{
				if (this.allowBuffering && !this.sendChunked)
				{
					return;
				}
				if (webAsyncResult.GotException)
				{
					throw webAsyncResult.Exception;
				}
				try
				{
					this.cnc.EndWrite2(this.request, webAsyncResult.InnerAsyncResult);
					webAsyncResult.SetCompleted(false, 0);
					webAsyncResult.DoCallback();
				}
				catch (Exception e)
				{
					webAsyncResult.SetCompleted(false, e);
					webAsyncResult.DoCallback();
					throw;
				}
				finally
				{
					if (this.sendChunked)
					{
						object obj = this.locker;
						lock (obj)
						{
							this.pendingWrites--;
							if (this.pendingWrites == 0)
							{
								this.pending.Set();
							}
						}
					}
				}
				return;
			}
		}

		public override void Write(byte[] buffer, int offset, int size)
		{
			AsyncCallback cb = new AsyncCallback(this.WriteCallbackWrapper);
			WebAsyncResult webAsyncResult = (WebAsyncResult)this.BeginWrite(buffer, offset, size, cb, null);
			if (!webAsyncResult.IsCompleted && !webAsyncResult.WaitUntilComplete(this.WriteTimeout, false))
			{
				this.KillBuffer();
				this.nextReadCalled = true;
				this.cnc.Close(true);
				throw new IOException("Write timed out.");
			}
			this.EndWrite(webAsyncResult);
		}

		public override void Flush()
		{
		}

		internal void SetHeaders(byte[] buffer)
		{
			if (this.headersSent)
			{
				return;
			}
			this.headers = buffer;
			long num = this.request.ContentLength;
			string method = this.request.Method;
			bool flag = method == "GET" || method == "CONNECT" || method == "HEAD" || method == "TRACE" || method == "DELETE";
			if (this.sendChunked || num > -1L || flag)
			{
				this.WriteHeaders();
				if (!this.initRead)
				{
					this.initRead = true;
					WebConnection.InitRead(this.cnc);
				}
				if (!this.sendChunked && num == 0L)
				{
					this.requestWritten = true;
				}
			}
		}

		internal bool RequestWritten
		{
			get
			{
				return this.requestWritten;
			}
		}

		private IAsyncResult WriteRequestAsync(AsyncCallback cb, object state)
		{
			this.requestWritten = true;
			byte[] buffer = this.writeBuffer.GetBuffer();
			int num = (int)this.writeBuffer.Length;
			IAsyncResult result;
			if (num > 0)
			{
				IAsyncResult asyncResult = this.cnc.BeginWrite(this.request, buffer, 0, num, cb, state);
				result = asyncResult;
			}
			else
			{
				result = null;
			}
			return result;
		}

		private void WriteHeaders()
		{
			if (this.headersSent)
			{
				return;
			}
			this.headersSent = true;
			string str = null;
			if (!this.cnc.Write(this.request, this.headers, 0, this.headers.Length, ref str))
			{
				throw new WebException("Error writing request: " + str, null, WebExceptionStatus.SendFailure, null);
			}
		}

		internal void WriteRequest()
		{
			if (this.requestWritten)
			{
				return;
			}
			this.requestWritten = true;
			if (this.sendChunked)
			{
				return;
			}
			if (!this.allowBuffering || this.writeBuffer == null)
			{
				return;
			}
			byte[] buffer = this.writeBuffer.GetBuffer();
			int num = (int)this.writeBuffer.Length;
			if (this.request.ContentLength != -1L && this.request.ContentLength < (long)num)
			{
				this.nextReadCalled = true;
				this.cnc.Close(true);
				throw new WebException("Specified Content-Length is less than the number of bytes to write", null, WebExceptionStatus.ServerProtocolViolation, null);
			}
			if (!this.headersSent)
			{
				string method = this.request.Method;
				if (!(method == "GET") && !(method == "CONNECT") && !(method == "HEAD") && !(method == "TRACE") && !(method == "DELETE"))
				{
					this.request.InternalContentLength = (long)num;
				}
				this.request.SendRequestHeaders(true);
			}
			this.WriteHeaders();
			if (this.cnc.Data.StatusCode != 0 && this.cnc.Data.StatusCode != 100)
			{
				return;
			}
			IAsyncResult result = null;
			if (num > 0)
			{
				result = this.cnc.BeginWrite(this.request, buffer, 0, num, null, null);
			}
			if (!this.initRead)
			{
				this.initRead = true;
				WebConnection.InitRead(this.cnc);
			}
			if (num > 0)
			{
				this.complete_request_written = this.cnc.EndWrite(this.request, result);
			}
			else
			{
				this.complete_request_written = true;
			}
		}

		internal void InternalClose()
		{
			this.disposed = true;
		}

		public override void Close()
		{
			if (this.sendChunked)
			{
				if (this.disposed)
				{
					return;
				}
				this.disposed = true;
				this.pending.WaitOne();
				byte[] bytes = Encoding.ASCII.GetBytes("0\r\n\r\n");
				string text = null;
				this.cnc.Write(this.request, bytes, 0, bytes.Length, ref text);
				return;
			}
			else
			{
				if (this.isRead)
				{
					if (!this.nextReadCalled)
					{
						this.CheckComplete();
						if (!this.nextReadCalled)
						{
							this.nextReadCalled = true;
							this.cnc.Close(true);
						}
					}
					return;
				}
				if (!this.allowBuffering)
				{
					this.complete_request_written = true;
					if (!this.initRead)
					{
						this.initRead = true;
						WebConnection.InitRead(this.cnc);
					}
					return;
				}
				if (this.disposed || this.requestWritten)
				{
					return;
				}
				long num = this.request.ContentLength;
				if (!this.sendChunked && num != -1L && this.totalWritten != num)
				{
					IOException innerException = new IOException("Cannot close the stream until all bytes are written");
					this.nextReadCalled = true;
					this.cnc.Close(true);
					throw new WebException("Request was cancelled.", innerException, WebExceptionStatus.RequestCanceled);
				}
				this.WriteRequest();
				this.disposed = true;
				return;
			}
		}

		internal void KillBuffer()
		{
			this.writeBuffer = null;
		}

		public override long Seek(long a, SeekOrigin b)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long a)
		{
			throw new NotSupportedException();
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override bool CanRead
		{
			get
			{
				return !this.disposed && this.isRead;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return !this.disposed && !this.isRead;
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
	}
}
