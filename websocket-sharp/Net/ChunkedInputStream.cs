using System;
using System.IO;
using System.Runtime.InteropServices;

namespace WebSocketSharp.Net
{
	internal class ChunkedInputStream : RequestStream
	{
		private HttpListenerContext context;

		private ChunkStream decoder;

		private bool disposed;

		private bool no_more_data;

		public ChunkedInputStream(HttpListenerContext context, Stream stream, byte[] buffer, int offset, int length) : base(stream, buffer, offset, length)
		{
			this.context = context;
			WebHeaderCollection headers = (WebHeaderCollection)context.Request.Headers;
			this.decoder = new ChunkStream(headers);
		}

		public ChunkStream Decoder
		{
			get
			{
				return this.decoder;
			}
			set
			{
				this.decoder = value;
			}
		}

		private void OnRead(IAsyncResult base_ares)
		{
			ChunkedInputStream.ReadBufferState readBufferState = (ChunkedInputStream.ReadBufferState)base_ares.AsyncState;
			HttpStreamAsyncResult ares = readBufferState.Ares;
			try
			{
				int num = base.EndRead(base_ares);
				this.decoder.Write(ares.Buffer, ares.Offset, num);
				num = this.decoder.Read(readBufferState.Buffer, readBufferState.Offset, readBufferState.Count);
				readBufferState.Offset += num;
				readBufferState.Count -= num;
				if (readBufferState.Count == 0 || !this.decoder.WantMore || num == 0)
				{
					this.no_more_data = (!this.decoder.WantMore && num == 0);
					ares.Count = readBufferState.InitialCount - readBufferState.Count;
					ares.Complete();
				}
				else
				{
					ares.Offset = 0;
					ares.Count = Math.Min(8192, this.decoder.ChunkLeft + 6);
					base.BeginRead(ares.Buffer, ares.Offset, ares.Count, new AsyncCallback(this.OnRead), readBufferState);
				}
			}
			catch (Exception ex)
			{
				this.context.Connection.SendError(ex.Message, 400);
				ares.Complete(ex);
			}
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback cback, object state)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = buffer.Length;
			if (offset < 0 || offset > num)
			{
				throw new ArgumentOutOfRangeException("'offset' exceeds the size of buffer.");
			}
			if (count < 0 || offset > num - count)
			{
				throw new ArgumentOutOfRangeException("'offset' + 'count' exceeds the size of buffer.");
			}
			HttpStreamAsyncResult httpStreamAsyncResult = new HttpStreamAsyncResult();
			httpStreamAsyncResult.Callback = cback;
			httpStreamAsyncResult.State = state;
			if (this.no_more_data)
			{
				httpStreamAsyncResult.Complete();
				return httpStreamAsyncResult;
			}
			int num2 = this.decoder.Read(buffer, offset, count);
			offset += num2;
			count -= num2;
			if (count == 0)
			{
				httpStreamAsyncResult.Count = num2;
				httpStreamAsyncResult.Complete();
				return httpStreamAsyncResult;
			}
			if (!this.decoder.WantMore)
			{
				this.no_more_data = (num2 == 0);
				httpStreamAsyncResult.Count = num2;
				httpStreamAsyncResult.Complete();
				return httpStreamAsyncResult;
			}
			httpStreamAsyncResult.Buffer = new byte[8192];
			httpStreamAsyncResult.Offset = 0;
			httpStreamAsyncResult.Count = 8192;
			ChunkedInputStream.ReadBufferState readBufferState = new ChunkedInputStream.ReadBufferState(buffer, offset, count, httpStreamAsyncResult);
			readBufferState.InitialCount += num2;
			base.BeginRead(httpStreamAsyncResult.Buffer, httpStreamAsyncResult.Offset, httpStreamAsyncResult.Count, new AsyncCallback(this.OnRead), readBufferState);
			return httpStreamAsyncResult;
		}

		public override void Close()
		{
			if (!this.disposed)
			{
				this.disposed = true;
				base.Close();
			}
		}

		public override int EndRead(IAsyncResult ares)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			if (ares == null)
			{
				throw new ArgumentException("Invalid IAsyncResult.", "ares");
			}
			if (!ares.IsCompleted)
			{
				ares.AsyncWaitHandle.WaitOne();
			}
			HttpStreamAsyncResult httpStreamAsyncResult = ares as HttpStreamAsyncResult;
			if (httpStreamAsyncResult.Error != null)
			{
				throw new HttpListenerException(400, "I/O operation aborted.");
			}
			return httpStreamAsyncResult.Count;
		}

		public override int Read([In] [Out] byte[] buffer, int offset, int count)
		{
			IAsyncResult asyncResult = this.BeginRead(buffer, offset, count, null, null);
			return this.EndRead(asyncResult);
		}

		private class ReadBufferState
		{
			public HttpStreamAsyncResult Ares;

			public byte[] Buffer;

			public int Count;

			public int InitialCount;

			public int Offset;

			public ReadBufferState(byte[] buffer, int offset, int count, HttpStreamAsyncResult ares)
			{
				this.Buffer = buffer;
				this.Offset = offset;
				this.Count = count;
				this.InitialCount = count;
				this.Ares = ares;
			}
		}
	}
}
