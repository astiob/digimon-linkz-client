using System;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace System.Net
{
	internal class FtpDataStream : Stream, IDisposable
	{
		private FtpWebRequest request;

		private Stream networkStream;

		private bool disposed;

		private bool isRead;

		private int totalRead;

		internal FtpDataStream(FtpWebRequest request, Stream stream, bool isRead)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			this.request = request;
			this.networkStream = stream;
			this.isRead = isRead;
		}

		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public override bool CanRead
		{
			get
			{
				return this.isRead;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return !this.isRead;
			}
		}

		public override bool CanSeek
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

		internal Stream NetworkStream
		{
			get
			{
				this.CheckDisposed();
				return this.networkStream;
			}
		}

		public override void Close()
		{
			this.Dispose(true);
		}

		public override void Flush()
		{
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		private int ReadInternal(byte[] buffer, int offset, int size)
		{
			int num = 0;
			this.request.CheckIfAborted();
			try
			{
				num = this.networkStream.Read(buffer, offset, size);
			}
			catch (IOException)
			{
				throw new ProtocolViolationException("Server commited a protocol violation");
			}
			this.totalRead += num;
			if (num == 0)
			{
				this.networkStream = null;
				this.request.CloseDataConnection();
				this.request.SetTransferCompleted();
			}
			return num;
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback cb, object state)
		{
			this.CheckDisposed();
			if (!this.isRead)
			{
				throw new NotSupportedException();
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("offset+size");
			}
			FtpDataStream.ReadDelegate readDelegate = new FtpDataStream.ReadDelegate(this.ReadInternal);
			return readDelegate.BeginInvoke(buffer, offset, size, cb, state);
		}

		public override int EndRead(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			AsyncResult asyncResult2 = asyncResult as AsyncResult;
			if (asyncResult2 == null)
			{
				throw new ArgumentException("Invalid asyncResult", "asyncResult");
			}
			FtpDataStream.ReadDelegate readDelegate = asyncResult2.AsyncDelegate as FtpDataStream.ReadDelegate;
			if (readDelegate == null)
			{
				throw new ArgumentException("Invalid asyncResult", "asyncResult");
			}
			return readDelegate.EndInvoke(asyncResult);
		}

		public override int Read(byte[] buffer, int offset, int size)
		{
			this.request.CheckIfAborted();
			IAsyncResult asyncResult = this.BeginRead(buffer, offset, size, null, null);
			if (!asyncResult.IsCompleted && !asyncResult.AsyncWaitHandle.WaitOne(this.request.ReadWriteTimeout, false))
			{
				throw new WebException("Read timed out.", WebExceptionStatus.Timeout);
			}
			return this.EndRead(asyncResult);
		}

		private void WriteInternal(byte[] buffer, int offset, int size)
		{
			this.request.CheckIfAborted();
			try
			{
				this.networkStream.Write(buffer, offset, size);
			}
			catch (IOException)
			{
				throw new ProtocolViolationException();
			}
		}

		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback cb, object state)
		{
			this.CheckDisposed();
			if (this.isRead)
			{
				throw new NotSupportedException();
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("offset+size");
			}
			FtpDataStream.WriteDelegate writeDelegate = new FtpDataStream.WriteDelegate(this.WriteInternal);
			return writeDelegate.BeginInvoke(buffer, offset, size, cb, state);
		}

		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			AsyncResult asyncResult2 = asyncResult as AsyncResult;
			if (asyncResult2 == null)
			{
				throw new ArgumentException("Invalid asyncResult.", "asyncResult");
			}
			FtpDataStream.WriteDelegate writeDelegate = asyncResult2.AsyncDelegate as FtpDataStream.WriteDelegate;
			if (writeDelegate == null)
			{
				throw new ArgumentException("Invalid asyncResult.", "asyncResult");
			}
			writeDelegate.EndInvoke(asyncResult);
		}

		public override void Write(byte[] buffer, int offset, int size)
		{
			this.request.CheckIfAborted();
			IAsyncResult asyncResult = this.BeginWrite(buffer, offset, size, null, null);
			if (!asyncResult.IsCompleted && !asyncResult.AsyncWaitHandle.WaitOne(this.request.ReadWriteTimeout, false))
			{
				throw new WebException("Read timed out.", WebExceptionStatus.Timeout);
			}
			this.EndWrite(asyncResult);
		}

		~FtpDataStream()
		{
			this.Dispose(false);
		}

		protected override void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			if (this.networkStream != null)
			{
				this.request.CloseDataConnection();
				this.request.SetTransferCompleted();
				this.request = null;
				this.networkStream = null;
			}
		}

		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		private delegate void WriteDelegate(byte[] buffer, int offset, int size);

		private delegate int ReadDelegate(byte[] buffer, int offset, int size);
	}
}
