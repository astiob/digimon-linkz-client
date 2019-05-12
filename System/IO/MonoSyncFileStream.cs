using System;
using System.Runtime.Remoting.Messaging;

namespace System.IO
{
	internal class MonoSyncFileStream : FileStream
	{
		public MonoSyncFileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize) : base(handle, access, ownsHandle, bufferSize, false)
		{
		}

		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback cback, object state)
		{
			if (!this.CanWrite)
			{
				throw new NotSupportedException("This stream does not support writing");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Must be >= 0");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Must be >= 0");
			}
			MonoSyncFileStream.WriteDelegate writeDelegate = new MonoSyncFileStream.WriteDelegate(this.Write);
			return writeDelegate.BeginInvoke(buffer, offset, count, cback, state);
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
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			MonoSyncFileStream.WriteDelegate writeDelegate = asyncResult2.AsyncDelegate as MonoSyncFileStream.WriteDelegate;
			if (writeDelegate == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			writeDelegate.EndInvoke(asyncResult);
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback cback, object state)
		{
			if (!this.CanRead)
			{
				throw new NotSupportedException("This stream does not support reading");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Must be >= 0");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Must be >= 0");
			}
			MonoSyncFileStream.ReadDelegate readDelegate = new MonoSyncFileStream.ReadDelegate(this.Read);
			return readDelegate.BeginInvoke(buffer, offset, count, cback, state);
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
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			MonoSyncFileStream.ReadDelegate readDelegate = asyncResult2.AsyncDelegate as MonoSyncFileStream.ReadDelegate;
			if (readDelegate == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			return readDelegate.EndInvoke(asyncResult);
		}

		private delegate void WriteDelegate(byte[] buffer, int offset, int count);

		private delegate int ReadDelegate(byte[] buffer, int offset, int count);
	}
}
