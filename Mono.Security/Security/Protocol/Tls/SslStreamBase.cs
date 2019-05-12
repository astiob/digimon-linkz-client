using Mono.Security.X509;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Mono.Security.Protocol.Tls
{
	public abstract class SslStreamBase : Stream, IDisposable
	{
		private const int WaitTimeOut = 300000;

		private static ManualResetEvent record_processing = new ManualResetEvent(true);

		internal Stream innerStream;

		internal MemoryStream inputBuffer;

		internal Context context;

		internal RecordProtocol protocol;

		internal bool ownsStream;

		private volatile bool disposed;

		private bool checkCertRevocationStatus;

		private object negotiate;

		private object read;

		private object write;

		private ManualResetEvent negotiationComplete;

		private byte[] recbuf = new byte[16384];

		private MemoryStream recordStream = new MemoryStream();

		protected SslStreamBase(Stream stream, bool ownsStream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream is null.");
			}
			if (!stream.CanRead || !stream.CanWrite)
			{
				throw new ArgumentNullException("stream is not both readable and writable.");
			}
			this.inputBuffer = new MemoryStream();
			this.innerStream = stream;
			this.ownsStream = ownsStream;
			this.negotiate = new object();
			this.read = new object();
			this.write = new object();
			this.negotiationComplete = new ManualResetEvent(false);
		}

		private void AsyncHandshakeCallback(IAsyncResult asyncResult)
		{
			SslStreamBase.InternalAsyncResult internalAsyncResult = asyncResult.AsyncState as SslStreamBase.InternalAsyncResult;
			try
			{
				try
				{
					this.OnNegotiateHandshakeCallback(asyncResult);
				}
				catch (TlsException ex)
				{
					this.protocol.SendAlert(ex.Alert);
					throw new IOException("The authentication or decryption has failed.", ex);
				}
				catch (Exception innerException)
				{
					this.protocol.SendAlert(AlertDescription.InternalError);
					throw new IOException("The authentication or decryption has failed.", innerException);
				}
				if (internalAsyncResult.ProceedAfterHandshake)
				{
					if (internalAsyncResult.FromWrite)
					{
						this.InternalBeginWrite(internalAsyncResult);
					}
					else
					{
						this.InternalBeginRead(internalAsyncResult);
					}
					this.negotiationComplete.Set();
				}
				else
				{
					this.negotiationComplete.Set();
					internalAsyncResult.SetComplete();
				}
			}
			catch (Exception complete)
			{
				this.negotiationComplete.Set();
				internalAsyncResult.SetComplete(complete);
			}
		}

		internal bool MightNeedHandshake
		{
			get
			{
				if (this.context.HandshakeState == HandshakeState.Finished)
				{
					return false;
				}
				object obj = this.negotiate;
				bool result;
				lock (obj)
				{
					result = (this.context.HandshakeState != HandshakeState.Finished);
				}
				return result;
			}
		}

		internal void NegotiateHandshake()
		{
			if (this.MightNeedHandshake)
			{
				SslStreamBase.InternalAsyncResult asyncResult = new SslStreamBase.InternalAsyncResult(null, null, null, 0, 0, false, false);
				if (!this.BeginNegotiateHandshake(asyncResult))
				{
					this.negotiationComplete.WaitOne();
				}
				else
				{
					this.EndNegotiateHandshake(asyncResult);
				}
			}
		}

		internal abstract IAsyncResult OnBeginNegotiateHandshake(AsyncCallback callback, object state);

		internal abstract void OnNegotiateHandshakeCallback(IAsyncResult asyncResult);

		internal abstract System.Security.Cryptography.X509Certificates.X509Certificate OnLocalCertificateSelection(System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates, System.Security.Cryptography.X509Certificates.X509Certificate serverCertificate, string targetHost, System.Security.Cryptography.X509Certificates.X509CertificateCollection serverRequestedCertificates);

		internal abstract bool OnRemoteCertificateValidation(System.Security.Cryptography.X509Certificates.X509Certificate certificate, int[] errors);

		internal abstract ValidationResult OnRemoteCertificateValidation2(Mono.Security.X509.X509CertificateCollection collection);

		internal abstract bool HaveRemoteValidation2Callback { get; }

		internal abstract AsymmetricAlgorithm OnLocalPrivateKeySelection(System.Security.Cryptography.X509Certificates.X509Certificate certificate, string targetHost);

		internal System.Security.Cryptography.X509Certificates.X509Certificate RaiseLocalCertificateSelection(System.Security.Cryptography.X509Certificates.X509CertificateCollection certificates, System.Security.Cryptography.X509Certificates.X509Certificate remoteCertificate, string targetHost, System.Security.Cryptography.X509Certificates.X509CertificateCollection requestedCertificates)
		{
			return this.OnLocalCertificateSelection(certificates, remoteCertificate, targetHost, requestedCertificates);
		}

		internal bool RaiseRemoteCertificateValidation(System.Security.Cryptography.X509Certificates.X509Certificate certificate, int[] errors)
		{
			return this.OnRemoteCertificateValidation(certificate, errors);
		}

		internal ValidationResult RaiseRemoteCertificateValidation2(Mono.Security.X509.X509CertificateCollection collection)
		{
			return this.OnRemoteCertificateValidation2(collection);
		}

		internal AsymmetricAlgorithm RaiseLocalPrivateKeySelection(System.Security.Cryptography.X509Certificates.X509Certificate certificate, string targetHost)
		{
			return this.OnLocalPrivateKeySelection(certificate, targetHost);
		}

		public bool CheckCertRevocationStatus
		{
			get
			{
				return this.checkCertRevocationStatus;
			}
			set
			{
				this.checkCertRevocationStatus = value;
			}
		}

		public CipherAlgorithmType CipherAlgorithm
		{
			get
			{
				if (this.context.HandshakeState == HandshakeState.Finished)
				{
					return this.context.Current.Cipher.CipherAlgorithmType;
				}
				return CipherAlgorithmType.None;
			}
		}

		public int CipherStrength
		{
			get
			{
				if (this.context.HandshakeState == HandshakeState.Finished)
				{
					return (int)this.context.Current.Cipher.EffectiveKeyBits;
				}
				return 0;
			}
		}

		public HashAlgorithmType HashAlgorithm
		{
			get
			{
				if (this.context.HandshakeState == HandshakeState.Finished)
				{
					return this.context.Current.Cipher.HashAlgorithmType;
				}
				return HashAlgorithmType.None;
			}
		}

		public int HashStrength
		{
			get
			{
				if (this.context.HandshakeState == HandshakeState.Finished)
				{
					return this.context.Current.Cipher.HashSize * 8;
				}
				return 0;
			}
		}

		public int KeyExchangeStrength
		{
			get
			{
				if (this.context.HandshakeState == HandshakeState.Finished)
				{
					return this.context.ServerSettings.Certificates[0].RSA.KeySize;
				}
				return 0;
			}
		}

		public ExchangeAlgorithmType KeyExchangeAlgorithm
		{
			get
			{
				if (this.context.HandshakeState == HandshakeState.Finished)
				{
					return this.context.Current.Cipher.ExchangeAlgorithmType;
				}
				return ExchangeAlgorithmType.None;
			}
		}

		public SecurityProtocolType SecurityProtocol
		{
			get
			{
				if (this.context.HandshakeState == HandshakeState.Finished)
				{
					return this.context.SecurityProtocol;
				}
				return (SecurityProtocolType)0;
			}
		}

		public System.Security.Cryptography.X509Certificates.X509Certificate ServerCertificate
		{
			get
			{
				if (this.context.HandshakeState == HandshakeState.Finished && this.context.ServerSettings.Certificates != null && this.context.ServerSettings.Certificates.Count > 0)
				{
					return new System.Security.Cryptography.X509Certificates.X509Certificate(this.context.ServerSettings.Certificates[0].RawData);
				}
				return null;
			}
		}

		internal Mono.Security.X509.X509CertificateCollection ServerCertificates
		{
			get
			{
				return this.context.ServerSettings.Certificates;
			}
		}

		private bool BeginNegotiateHandshake(SslStreamBase.InternalAsyncResult asyncResult)
		{
			bool result;
			try
			{
				object obj = this.negotiate;
				lock (obj)
				{
					if (this.context.HandshakeState == HandshakeState.None)
					{
						this.OnBeginNegotiateHandshake(new AsyncCallback(this.AsyncHandshakeCallback), asyncResult);
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			catch (TlsException ex)
			{
				this.negotiationComplete.Set();
				this.protocol.SendAlert(ex.Alert);
				throw new IOException("The authentication or decryption has failed.", ex);
			}
			catch (Exception innerException)
			{
				this.negotiationComplete.Set();
				this.protocol.SendAlert(AlertDescription.InternalError);
				throw new IOException("The authentication or decryption has failed.", innerException);
			}
			return result;
		}

		private void EndNegotiateHandshake(SslStreamBase.InternalAsyncResult asyncResult)
		{
			if (!asyncResult.IsCompleted)
			{
				asyncResult.AsyncWaitHandle.WaitOne();
			}
			if (asyncResult.CompletedWithError)
			{
				throw asyncResult.AsyncException;
			}
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this.checkDisposed();
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer is a null reference.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset is less than 0.");
			}
			if (offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset is greater than the length of buffer.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count is less than 0.");
			}
			if (count > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("count is less than the length of buffer minus the value of the offset parameter.");
			}
			SslStreamBase.InternalAsyncResult internalAsyncResult = new SslStreamBase.InternalAsyncResult(callback, state, buffer, offset, count, false, true);
			if (this.MightNeedHandshake)
			{
				if (!this.BeginNegotiateHandshake(internalAsyncResult))
				{
					this.negotiationComplete.WaitOne();
					this.InternalBeginRead(internalAsyncResult);
				}
			}
			else
			{
				this.InternalBeginRead(internalAsyncResult);
			}
			return internalAsyncResult;
		}

		private void InternalBeginRead(SslStreamBase.InternalAsyncResult asyncResult)
		{
			try
			{
				int num = 0;
				object obj = this.read;
				lock (obj)
				{
					bool flag = this.inputBuffer.Position == this.inputBuffer.Length && this.inputBuffer.Length > 0L;
					bool flag2 = this.inputBuffer.Length > 0L && asyncResult.Count > 0;
					if (flag)
					{
						this.resetBuffer();
					}
					else if (flag2)
					{
						num = this.inputBuffer.Read(asyncResult.Buffer, asyncResult.Offset, asyncResult.Count);
					}
				}
				if (0 < num)
				{
					asyncResult.SetComplete(num);
				}
				else if (!this.context.ReceivedConnectionEnd)
				{
					this.innerStream.BeginRead(this.recbuf, 0, this.recbuf.Length, new AsyncCallback(this.InternalReadCallback), new object[]
					{
						this.recbuf,
						asyncResult
					});
				}
				else
				{
					asyncResult.SetComplete(0);
				}
			}
			catch (TlsException ex)
			{
				this.protocol.SendAlert(ex.Alert);
				throw new IOException("The authentication or decryption has failed.", ex);
			}
			catch (Exception innerException)
			{
				throw new IOException("IO exception during read.", innerException);
			}
		}

		private void InternalReadCallback(IAsyncResult result)
		{
			if (this.disposed)
			{
				return;
			}
			object[] array = (object[])result.AsyncState;
			byte[] array2 = (byte[])array[0];
			SslStreamBase.InternalAsyncResult internalAsyncResult = (SslStreamBase.InternalAsyncResult)array[1];
			try
			{
				int num = this.innerStream.EndRead(result);
				if (num > 0)
				{
					this.recordStream.Write(array2, 0, num);
					bool flag = false;
					long position = this.recordStream.Position;
					this.recordStream.Position = 0L;
					byte[] array3 = null;
					if (this.recordStream.Length >= 5L)
					{
						array3 = this.protocol.ReceiveRecord(this.recordStream);
					}
					while (array3 != null)
					{
						long num2 = this.recordStream.Length - this.recordStream.Position;
						byte[] array4 = null;
						if (num2 > 0L)
						{
							array4 = new byte[num2];
							this.recordStream.Read(array4, 0, array4.Length);
						}
						object obj = this.read;
						lock (obj)
						{
							long position2 = this.inputBuffer.Position;
							if (array3.Length > 0)
							{
								this.inputBuffer.Seek(0L, SeekOrigin.End);
								this.inputBuffer.Write(array3, 0, array3.Length);
								this.inputBuffer.Seek(position2, SeekOrigin.Begin);
								flag = true;
							}
						}
						this.recordStream.SetLength(0L);
						array3 = null;
						if (num2 > 0L)
						{
							this.recordStream.Write(array4, 0, array4.Length);
							if (this.recordStream.Length >= 5L)
							{
								this.recordStream.Position = 0L;
								array3 = this.protocol.ReceiveRecord(this.recordStream);
								if (array3 == null)
								{
									position = this.recordStream.Length;
								}
							}
							else
							{
								position = num2;
							}
						}
						else
						{
							position = 0L;
						}
					}
					if (!flag && num > 0)
					{
						if (this.context.ReceivedConnectionEnd)
						{
							internalAsyncResult.SetComplete(0);
						}
						else
						{
							this.recordStream.Position = this.recordStream.Length;
							this.innerStream.BeginRead(array2, 0, array2.Length, new AsyncCallback(this.InternalReadCallback), array);
						}
					}
					else
					{
						this.recordStream.Position = position;
						int complete = 0;
						object obj2 = this.read;
						lock (obj2)
						{
							complete = this.inputBuffer.Read(internalAsyncResult.Buffer, internalAsyncResult.Offset, internalAsyncResult.Count);
						}
						internalAsyncResult.SetComplete(complete);
					}
				}
				else
				{
					internalAsyncResult.SetComplete(0);
				}
			}
			catch (Exception complete2)
			{
				internalAsyncResult.SetComplete(complete2);
			}
		}

		private void InternalBeginWrite(SslStreamBase.InternalAsyncResult asyncResult)
		{
			try
			{
				object obj = this.write;
				lock (obj)
				{
					byte[] array = this.protocol.EncodeRecord(ContentType.ApplicationData, asyncResult.Buffer, asyncResult.Offset, asyncResult.Count);
					this.innerStream.BeginWrite(array, 0, array.Length, new AsyncCallback(this.InternalWriteCallback), asyncResult);
				}
			}
			catch (TlsException ex)
			{
				this.protocol.SendAlert(ex.Alert);
				this.Close();
				throw new IOException("The authentication or decryption has failed.", ex);
			}
			catch (Exception innerException)
			{
				throw new IOException("IO exception during Write.", innerException);
			}
		}

		private void InternalWriteCallback(IAsyncResult ar)
		{
			if (this.disposed)
			{
				return;
			}
			SslStreamBase.InternalAsyncResult internalAsyncResult = (SslStreamBase.InternalAsyncResult)ar.AsyncState;
			try
			{
				this.innerStream.EndWrite(ar);
				internalAsyncResult.SetComplete();
			}
			catch (Exception complete)
			{
				internalAsyncResult.SetComplete(complete);
			}
		}

		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this.checkDisposed();
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer is a null reference.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset is less than 0.");
			}
			if (offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset is greater than the length of buffer.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count is less than 0.");
			}
			if (count > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("count is less than the length of buffer minus the value of the offset parameter.");
			}
			SslStreamBase.InternalAsyncResult internalAsyncResult = new SslStreamBase.InternalAsyncResult(callback, state, buffer, offset, count, true, true);
			if (this.MightNeedHandshake)
			{
				if (!this.BeginNegotiateHandshake(internalAsyncResult))
				{
					this.negotiationComplete.WaitOne();
					this.InternalBeginWrite(internalAsyncResult);
				}
			}
			else
			{
				this.InternalBeginWrite(internalAsyncResult);
			}
			return internalAsyncResult;
		}

		public override int EndRead(IAsyncResult asyncResult)
		{
			this.checkDisposed();
			SslStreamBase.InternalAsyncResult internalAsyncResult = asyncResult as SslStreamBase.InternalAsyncResult;
			if (internalAsyncResult == null)
			{
				throw new ArgumentNullException("asyncResult is null or was not obtained by calling BeginRead.");
			}
			if (!asyncResult.IsCompleted && !asyncResult.AsyncWaitHandle.WaitOne(300000, false))
			{
				throw new TlsException(AlertDescription.InternalError, "Couldn't complete EndRead");
			}
			if (internalAsyncResult.CompletedWithError)
			{
				throw internalAsyncResult.AsyncException;
			}
			return internalAsyncResult.BytesRead;
		}

		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.checkDisposed();
			SslStreamBase.InternalAsyncResult internalAsyncResult = asyncResult as SslStreamBase.InternalAsyncResult;
			if (internalAsyncResult == null)
			{
				throw new ArgumentNullException("asyncResult is null or was not obtained by calling BeginWrite.");
			}
			if (!asyncResult.IsCompleted && !internalAsyncResult.AsyncWaitHandle.WaitOne(300000, false))
			{
				throw new TlsException(AlertDescription.InternalError, "Couldn't complete EndWrite");
			}
			if (internalAsyncResult.CompletedWithError)
			{
				throw internalAsyncResult.AsyncException;
			}
		}

		public override void Close()
		{
			base.Close();
		}

		public override void Flush()
		{
			this.checkDisposed();
			this.innerStream.Flush();
		}

		public int Read(byte[] buffer)
		{
			return this.Read(buffer, 0, buffer.Length);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			this.checkDisposed();
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset is less than 0.");
			}
			if (offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset is greater than the length of buffer.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count is less than 0.");
			}
			if (count > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("count is less than the length of buffer minus the value of the offset parameter.");
			}
			if (this.context.HandshakeState != HandshakeState.Finished)
			{
				this.NegotiateHandshake();
			}
			object obj = this.read;
			int result;
			lock (obj)
			{
				try
				{
					SslStreamBase.record_processing.Reset();
					if (this.inputBuffer.Position > 0L)
					{
						if (this.inputBuffer.Position == this.inputBuffer.Length)
						{
							this.inputBuffer.SetLength(0L);
						}
						else
						{
							int num = this.inputBuffer.Read(buffer, offset, count);
							if (num > 0)
							{
								SslStreamBase.record_processing.Set();
								return num;
							}
						}
					}
					bool flag = false;
					for (;;)
					{
						if (this.recordStream.Position == 0L || flag)
						{
							flag = false;
							byte[] array = new byte[16384];
							int num2 = 0;
							if (count == 1)
							{
								int num3 = this.innerStream.ReadByte();
								if (num3 >= 0)
								{
									array[0] = (byte)num3;
									num2 = 1;
								}
							}
							else
							{
								num2 = this.innerStream.Read(array, 0, array.Length);
							}
							if (num2 <= 0)
							{
								break;
							}
							if (this.recordStream.Length > 0L && this.recordStream.Position != this.recordStream.Length)
							{
								this.recordStream.Seek(0L, SeekOrigin.End);
							}
							this.recordStream.Write(array, 0, num2);
						}
						bool flag2 = false;
						this.recordStream.Position = 0L;
						byte[] array2 = null;
						if (this.recordStream.Length >= 5L)
						{
							array2 = this.protocol.ReceiveRecord(this.recordStream);
							flag = (array2 == null);
						}
						while (array2 != null)
						{
							long num4 = this.recordStream.Length - this.recordStream.Position;
							byte[] array3 = null;
							if (num4 > 0L)
							{
								array3 = new byte[num4];
								this.recordStream.Read(array3, 0, array3.Length);
							}
							long position = this.inputBuffer.Position;
							if (array2.Length > 0)
							{
								this.inputBuffer.Seek(0L, SeekOrigin.End);
								this.inputBuffer.Write(array2, 0, array2.Length);
								this.inputBuffer.Seek(position, SeekOrigin.Begin);
								flag2 = true;
							}
							this.recordStream.SetLength(0L);
							array2 = null;
							if (num4 > 0L)
							{
								this.recordStream.Write(array3, 0, array3.Length);
							}
							if (flag2)
							{
								goto Block_23;
							}
						}
					}
					SslStreamBase.record_processing.Set();
					return 0;
					Block_23:
					int num5 = this.inputBuffer.Read(buffer, offset, count);
					SslStreamBase.record_processing.Set();
					result = num5;
				}
				catch (TlsException innerException)
				{
					throw new IOException("The authentication or decryption has failed.", innerException);
				}
				catch (Exception innerException2)
				{
					throw new IOException("IO exception during read.", innerException2);
				}
			}
			return result;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public void Write(byte[] buffer)
		{
			this.Write(buffer, 0, buffer.Length);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			this.checkDisposed();
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset is less than 0.");
			}
			if (offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset is greater than the length of buffer.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count is less than 0.");
			}
			if (count > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("count is less than the length of buffer minus the value of the offset parameter.");
			}
			if (this.context.HandshakeState != HandshakeState.Finished)
			{
				this.NegotiateHandshake();
			}
			object obj = this.write;
			lock (obj)
			{
				try
				{
					byte[] array = this.protocol.EncodeRecord(ContentType.ApplicationData, buffer, offset, count);
					this.innerStream.Write(array, 0, array.Length);
				}
				catch (TlsException ex)
				{
					this.protocol.SendAlert(ex.Alert);
					this.Close();
					throw new IOException("The authentication or decryption has failed.", ex);
				}
				catch (Exception innerException)
				{
					throw new IOException("IO exception during Write.", innerException);
				}
			}
		}

		public override bool CanRead
		{
			get
			{
				return this.innerStream.CanRead;
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
				return this.innerStream.CanWrite;
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

		~SslStreamBase()
		{
			this.Dispose(false);
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					if (this.innerStream != null)
					{
						if (this.context.HandshakeState == HandshakeState.Finished && !this.context.SentConnectionEnd)
						{
							try
							{
								this.protocol.SendAlert(AlertDescription.CloseNotify);
							}
							catch
							{
							}
						}
						if (this.ownsStream)
						{
							this.innerStream.Close();
						}
					}
					this.ownsStream = false;
					this.innerStream = null;
				}
				this.disposed = true;
				base.Dispose(disposing);
			}
		}

		private void resetBuffer()
		{
			this.inputBuffer.SetLength(0L);
			this.inputBuffer.Position = 0L;
		}

		internal void checkDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("The Stream is closed.");
			}
		}

		private class InternalAsyncResult : IAsyncResult
		{
			private object locker = new object();

			private AsyncCallback _userCallback;

			private object _userState;

			private Exception _asyncException;

			private ManualResetEvent handle;

			private bool completed;

			private int _bytesRead;

			private bool _fromWrite;

			private bool _proceedAfterHandshake;

			private byte[] _buffer;

			private int _offset;

			private int _count;

			public InternalAsyncResult(AsyncCallback userCallback, object userState, byte[] buffer, int offset, int count, bool fromWrite, bool proceedAfterHandshake)
			{
				this._userCallback = userCallback;
				this._userState = userState;
				this._buffer = buffer;
				this._offset = offset;
				this._count = count;
				this._fromWrite = fromWrite;
				this._proceedAfterHandshake = proceedAfterHandshake;
			}

			public bool ProceedAfterHandshake
			{
				get
				{
					return this._proceedAfterHandshake;
				}
			}

			public bool FromWrite
			{
				get
				{
					return this._fromWrite;
				}
			}

			public byte[] Buffer
			{
				get
				{
					return this._buffer;
				}
			}

			public int Offset
			{
				get
				{
					return this._offset;
				}
			}

			public int Count
			{
				get
				{
					return this._count;
				}
			}

			public int BytesRead
			{
				get
				{
					return this._bytesRead;
				}
			}

			public object AsyncState
			{
				get
				{
					return this._userState;
				}
			}

			public Exception AsyncException
			{
				get
				{
					return this._asyncException;
				}
			}

			public bool CompletedWithError
			{
				get
				{
					return this.IsCompleted && null != this._asyncException;
				}
			}

			public WaitHandle AsyncWaitHandle
			{
				get
				{
					object obj = this.locker;
					lock (obj)
					{
						if (this.handle == null)
						{
							this.handle = new ManualResetEvent(this.completed);
						}
					}
					return this.handle;
				}
			}

			public bool CompletedSynchronously
			{
				get
				{
					return false;
				}
			}

			public bool IsCompleted
			{
				get
				{
					object obj = this.locker;
					bool result;
					lock (obj)
					{
						result = this.completed;
					}
					return result;
				}
			}

			private void SetComplete(Exception ex, int bytesRead)
			{
				object obj = this.locker;
				lock (obj)
				{
					if (this.completed)
					{
						return;
					}
					this.completed = true;
					this._asyncException = ex;
					this._bytesRead = bytesRead;
					if (this.handle != null)
					{
						this.handle.Set();
					}
				}
				if (this._userCallback != null)
				{
					this._userCallback.BeginInvoke(this, null, null);
				}
			}

			public void SetComplete(Exception ex)
			{
				this.SetComplete(ex, 0);
			}

			public void SetComplete(int bytesRead)
			{
				this.SetComplete(null, bytesRead);
			}

			public void SetComplete()
			{
				this.SetComplete(null, 0);
			}
		}

		private delegate void AsyncHandshakeDelegate(SslStreamBase.InternalAsyncResult asyncResult, bool fromWrite);
	}
}
