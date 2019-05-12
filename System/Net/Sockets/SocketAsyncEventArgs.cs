using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Net.Sockets
{
	/// <summary>Represents an asynchronous socket operation.</summary>
	public class SocketAsyncEventArgs : EventArgs, IDisposable
	{
		private IList<ArraySegment<byte>> _bufferList;

		private Socket curSocket;

		internal SocketAsyncEventArgs(bool policy) : this()
		{
			this.PolicyRestricted = policy;
		}

		/// <summary>Creates an empty <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> instance.</summary>
		/// <exception cref="T:System.NotSupportedException">The platform is not supported. </exception>
		public SocketAsyncEventArgs()
		{
			this.AcceptSocket = null;
			this.Buffer = null;
			this.BufferList = null;
			this.BytesTransferred = 0;
			this.Count = 0;
			this.DisconnectReuseSocket = false;
			this.LastOperation = SocketAsyncOperation.None;
			this.Offset = 0;
			this.RemoteEndPoint = null;
			this.SendPacketsSendSize = -1;
			this.SocketError = SocketError.Success;
			this.SocketFlags = SocketFlags.None;
			this.UserToken = null;
		}

		/// <summary>The event used to complete an asynchronous operation.</summary>
		public event EventHandler<SocketAsyncEventArgs> Completed;

		/// <summary>Gets or sets the socket to use or the socket created for accepting a connection with an asynchronous socket method.</summary>
		/// <returns>The <see cref="T:System.Net.Sockets.Socket" /> to use or the socket created for accepting a connection with an asynchronous socket method.</returns>
		public Socket AcceptSocket { get; set; }

		/// <summary>Gets the data buffer to use with an asynchronous socket method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array that represents the data buffer to use with an asynchronous socket method.</returns>
		public byte[] Buffer { get; private set; }

		/// <summary>Gets or sets an array of data buffers to use with an asynchronous socket method.</summary>
		/// <returns>An <see cref="T:System.Collections.IList" /> that represents an array of data buffers to use with an asynchronous socket method.</returns>
		/// <exception cref="T:System.ArgumentException">There are ambiguous buffers specified on a set operation. This exception occurs if a value other than null is passed and the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property is also not null.</exception>
		[MonoTODO("not supported in all cases")]
		public IList<ArraySegment<byte>> BufferList
		{
			get
			{
				return this._bufferList;
			}
			set
			{
				if (this.Buffer != null && value != null)
				{
					throw new ArgumentException("Buffer and BufferList properties cannot both be non-null.");
				}
				this._bufferList = value;
			}
		}

		/// <summary>Gets the number of bytes transferred in the socket operation.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the number of bytes transferred in the socket operation.</returns>
		public int BytesTransferred { get; private set; }

		/// <summary>Gets the maximum amount of data, in bytes, to send or receive in an asynchronous operation.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the maximum amount of data, in bytes, to send or receive.</returns>
		public int Count { get; private set; }

		/// <summary>Gets or sets a value that specifies if socket can be reused after a disconnect operation.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> that specifies if socket can be reused after a disconnect operation.</returns>
		public bool DisconnectReuseSocket { get; set; }

		/// <summary>Gets the type of socket operation most recently performed with this context object.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.SocketAsyncOperation" /> instance that indicates the type of socket operation most recently performed with this context object.</returns>
		public SocketAsyncOperation LastOperation { get; private set; }

		/// <summary>Gets the offset, in bytes, into the data buffer referenced by the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the offset, in bytes, into the data buffer referenced by the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property.</returns>
		public int Offset { get; private set; }

		/// <summary>Gets or sets the remote IP endpoint for an asynchronous operation.</summary>
		/// <returns>An <see cref="T:System.Net.EndPoint" /> that represents the remote IP endpoint for an asynchronous operation.</returns>
		public EndPoint RemoteEndPoint { get; set; }

		/// <summary>Gets or sets the size, in bytes, of the data block used in the send operation.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the size, in bytes, of the data block used in the send operation.</returns>
		[MonoTODO("unused property")]
		public int SendPacketsSendSize { get; set; }

		/// <summary>Gets or sets the result of the asynchronous socket operation.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.SocketError" /> that represents the result of the asynchronous socket operation.</returns>
		public SocketError SocketError { get; set; }

		/// <summary>Gets the results of an asynchronous socket operation or sets the behavior of an asynchronous operation.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.SocketFlags" /> that represents the results of an asynchronous socket operation.</returns>
		public SocketFlags SocketFlags { get; set; }

		/// <summary>Gets or sets a user or application object associated with this asynchronous socket operation.</summary>
		/// <returns>An object that represents the user or application object associated with this asynchronous socket operation.</returns>
		public object UserToken { get; set; }

		public Socket ConnectSocket
		{
			get
			{
				SocketError socketError = this.SocketError;
				if (socketError != SocketError.AccessDenied)
				{
					return this.curSocket;
				}
				return null;
			}
		}

		internal bool PolicyRestricted { get; private set; }

		/// <summary>Frees resources used by the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> class.</summary>
		~SocketAsyncEventArgs()
		{
			this.Dispose(false);
		}

		private void Dispose(bool disposing)
		{
			Socket acceptSocket = this.AcceptSocket;
			if (acceptSocket != null)
			{
				acceptSocket.Close();
			}
			if (disposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> instance and optionally disposes of the managed resources.</summary>
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>Represents a method that is called when an asynchronous operation completes.</summary>
		/// <param name="e">The event that is signaled.</param>
		protected virtual void OnCompleted(SocketAsyncEventArgs e)
		{
			if (e == null)
			{
				return;
			}
			EventHandler<SocketAsyncEventArgs> completed = e.Completed;
			if (completed != null)
			{
				completed(e.curSocket, e);
			}
		}

		/// <summary>Sets the data buffer to use with an asynchronous socket method.</summary>
		/// <param name="offset">The offset, in bytes, in the data buffer where the operation starts.</param>
		/// <param name="count">The maximum amount of data, in bytes, to send or receive in the buffer.</param>
		/// <exception cref="T:System.ArgumentException">There are ambiguous buffers specified. This exception occurs if the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property is also not null and the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.BufferList" /> property is also not null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">An argument was out of range. This exception occurs if the <paramref name="offset" /> parameter is less than zero or greater than the length of the array in the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property. This exception also occurs if the <paramref name="count" /> parameter is less than zero or greater than the length of the array in the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property minus the <paramref name="offset" /> parameter.</exception>
		public void SetBuffer(int offset, int count)
		{
			this.SetBufferInternal(this.Buffer, offset, count);
		}

		/// <summary>Sets the data buffer to use with an asynchronous socket method.</summary>
		/// <param name="buffer">The data buffer to use with an asynchronous socket method.</param>
		/// <param name="offset">The offset, in bytes, in the data buffer where the operation starts.</param>
		/// <param name="count">The maximum amount of data, in bytes, to send or receive in the buffer.</param>
		/// <exception cref="T:System.ArgumentException">There are ambiguous buffers specified. This exception occurs if the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property is also not null and the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.BufferList" /> property is also not null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">An argument was out of range. This exception occurs if the <paramref name="offset" /> parameter is less than zero or greater than the length of the array in the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property. This exception also occurs if the <paramref name="count" /> parameter is less than zero or greater than the length of the array in the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property minus the <paramref name="offset" /> parameter.</exception>
		public void SetBuffer(byte[] buffer, int offset, int count)
		{
			this.SetBufferInternal(buffer, offset, count);
		}

		private void SetBufferInternal(byte[] buffer, int offset, int count)
		{
			if (buffer != null)
			{
				if (this.BufferList != null)
				{
					throw new ArgumentException("Buffer and BufferList properties cannot both be non-null.");
				}
				int num = buffer.Length;
				if (offset < 0 || (offset != 0 && offset >= num))
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				if (count < 0 || count > num - offset)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				this.Count = count;
				this.Offset = offset;
			}
			this.Buffer = buffer;
		}

		private void ReceiveCallback()
		{
			this.SocketError = SocketError.Success;
			this.LastOperation = SocketAsyncOperation.Receive;
			SocketError socketError = SocketError.Success;
			if (!this.curSocket.Connected)
			{
				this.SocketError = SocketError.NotConnected;
				return;
			}
			try
			{
				this.BytesTransferred = this.curSocket.Receive_nochecks(this.Buffer, this.Offset, this.Count, this.SocketFlags, out socketError);
			}
			finally
			{
				this.SocketError = socketError;
				this.OnCompleted(this);
			}
		}

		private void ConnectCallback()
		{
			this.LastOperation = SocketAsyncOperation.Connect;
			SocketError socketError = SocketError.AccessDenied;
			try
			{
				socketError = this.TryConnect(this.RemoteEndPoint);
			}
			finally
			{
				this.SocketError = socketError;
				this.OnCompleted(this);
			}
		}

		private SocketError TryConnect(EndPoint endpoint)
		{
			this.curSocket.Connected = false;
			SocketError result = SocketError.Success;
			try
			{
				this.curSocket.seed_endpoint = endpoint;
				this.curSocket.Connect(endpoint);
				this.curSocket.Connected = true;
			}
			catch (SocketException ex)
			{
				result = ex.SocketErrorCode;
			}
			return result;
		}

		private void SendCallback()
		{
			this.SocketError = SocketError.Success;
			this.LastOperation = SocketAsyncOperation.Send;
			SocketError socketError = SocketError.Success;
			if (!this.curSocket.Connected)
			{
				this.SocketError = SocketError.NotConnected;
				return;
			}
			try
			{
				if (this.Buffer != null)
				{
					this.BytesTransferred = this.curSocket.Send_nochecks(this.Buffer, this.Offset, this.Count, SocketFlags.None, out socketError);
				}
				else if (this.BufferList != null)
				{
					this.BytesTransferred = 0;
					foreach (ArraySegment<byte> arraySegment in this.BufferList)
					{
						this.BytesTransferred += this.curSocket.Send_nochecks(arraySegment.Array, arraySegment.Offset, arraySegment.Count, SocketFlags.None, out socketError);
						if (socketError != SocketError.Success)
						{
							break;
						}
					}
				}
			}
			finally
			{
				this.SocketError = socketError;
				this.OnCompleted(this);
			}
		}

		internal void DoOperation(SocketAsyncOperation operation, Socket socket)
		{
			this.curSocket = socket;
			ThreadStart start;
			switch (operation)
			{
			case SocketAsyncOperation.Connect:
				start = new ThreadStart(this.ConnectCallback);
				goto IL_6A;
			case SocketAsyncOperation.Receive:
				start = new ThreadStart(this.ReceiveCallback);
				goto IL_6A;
			case SocketAsyncOperation.Send:
				start = new ThreadStart(this.SendCallback);
				goto IL_6A;
			}
			throw new NotSupportedException();
			IL_6A:
			new Thread(start)
			{
				IsBackground = true
			}.Start();
		}
	}
}
