using Mono.Security.Protocol.Tls;
using System;
using System.IO;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security
{
	/// <summary>Provides a stream used for client-server communication that uses the Secure Socket Layer (SSL) security protocol to authenticate the server and optionally the client.</summary>
	[MonoTODO("Non-X509Certificate2 certificate is not supported")]
	public class SslStream : AuthenticatedStream
	{
		private SslStreamBase ssl_stream;

		private RemoteCertificateValidationCallback validation_callback;

		private LocalCertificateSelectionCallback selection_callback;

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="innerStream">A <see cref="T:System.IO.Stream" /> object used by the <see cref="T:System.Net.Security.SslStream" /> for sending and receiving data.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="innerStream" /> is not readable.-or-<paramref name="innerStream" /> is not writable.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="innerStream" /> is null.-or-<paramref name="innerStream" /> is equal to <see cref="F:System.IO.Stream.Null" />.</exception>
		public SslStream(Stream innerStream) : this(innerStream, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" /> and stream closure behavior.</summary>
		/// <param name="innerStream">Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" /> and stream closure behavior.</param>
		/// <param name="leaveInnerStreamOpen">Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" /> and stream closure behavior.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="innerStream" /> is not readable.-or-<paramref name="innerStream" /> is not writable.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="innerStream" /> is null.-or-<paramref name="innerStream" /> is equal to <see cref="F:System.IO.Stream.Null" />.</exception>
		public SslStream(Stream innerStream, bool leaveStreamOpen) : base(innerStream, leaveStreamOpen)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" />, stream closure behavior and certificate validation delegate.</summary>
		/// <param name="innerStream">Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" /> and stream closure behavior.</param>
		/// <param name="leaveInnerStreamOpen">Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" /> and stream closure behavior.</param>
		/// <param name="userCertificateValidationCallback">A <see cref="T:System.Net.Security.RemoteCertificateValidationCallback" /> delegate responsible for validating the certificate supplied by the remote party.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="innerStream" /> is not readable.-or-<paramref name="innerStream" /> is not writable.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="innerStream" /> is null.-or-<paramref name="innerStream" /> is equal to <see cref="F:System.IO.Stream.Null" />.</exception>
		[MonoTODO("certValidationCallback is not passed X509Chain and SslPolicyErrors correctly")]
		public SslStream(Stream innerStream, bool leaveStreamOpen, RemoteCertificateValidationCallback certValidationCallback) : this(innerStream, leaveStreamOpen, certValidationCallback, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" />, stream closure behavior, certificate validation delegate and certificate selection delegate.</summary>
		/// <param name="innerStream">Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" /> and stream closure behavior.</param>
		/// <param name="leaveInnerStreamOpen">Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" /> and stream closure behavior.</param>
		/// <param name="userCertificateValidationCallback">A <see cref="T:System.Net.Security.RemoteCertificateValidationCallback" /> delegate responsible for validating the certificate supplied by the remote party.</param>
		/// <param name="userCertificateSelectionCallback">A <see cref="T:System.Net.Security.LocalCertificateSelectionCallback" /> delegate responsible for selecting the certificate used for authentication.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="innerStream" /> is not readable.-or-<paramref name="innerStream" /> is not writable.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="innerStream" /> is null.-or-<paramref name="innerStream" /> is equal to <see cref="F:System.IO.Stream.Null" />.</exception>
		[MonoTODO("certValidationCallback is not passed X509Chain and SslPolicyErrors correctly")]
		public SslStream(Stream innerStream, bool leaveStreamOpen, RemoteCertificateValidationCallback certValidationCallback, LocalCertificateSelectionCallback certSelectionCallback) : base(innerStream, leaveStreamOpen)
		{
			this.validation_callback = certValidationCallback;
			this.selection_callback = certSelectionCallback;
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the underlying stream is readable.</summary>
		/// <returns>true if authentication has occurred and the underlying stream is readable; otherwise false.</returns>
		public override bool CanRead
		{
			get
			{
				return base.InnerStream.CanRead;
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the underlying stream is seekable.</summary>
		/// <returns>This property always returns false.</returns>
		public override bool CanSeek
		{
			get
			{
				return base.InnerStream.CanSeek;
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the underlying stream supports time-outs.</summary>
		/// <returns>true if the underlying stream supports time-outs; otherwise, false.</returns>
		public override bool CanTimeout
		{
			get
			{
				return base.InnerStream.CanTimeout;
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the underlying stream is writable.</summary>
		/// <returns>true if authentication has occurred and the underlying stream is writable; otherwise false.</returns>
		public override bool CanWrite
		{
			get
			{
				return base.InnerStream.CanWrite;
			}
		}

		/// <summary>Gets the length of the underlying stream.</summary>
		/// <returns>A <see cref="T:System.Int64" />.</returns>
		/// <exception cref="T:System.NotSupportedException">Getting the value of this property is not supported when the underlying stream is a <see cref="T:System.Net.Sockets.NetworkStream" />.</exception>
		public override long Length
		{
			get
			{
				return base.InnerStream.Length;
			}
		}

		/// <summary>Gets or sets the current position in the underlying stream.</summary>
		/// <returns>A <see cref="T:System.Int64" />.</returns>
		/// <exception cref="T:System.NotSupportedException">Setting this property is not supported.-or-Getting the value of this property is not supported when the underlying stream is a <see cref="T:System.Net.Sockets.NetworkStream" />.</exception>
		public override long Position
		{
			get
			{
				return base.InnerStream.Position;
			}
			set
			{
				throw new NotSupportedException("This stream does not support seek operations");
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether authentication was successful.</summary>
		/// <returns>true if successful authentication occurred; otherwise, false.</returns>
		public override bool IsAuthenticated
		{
			get
			{
				return this.ssl_stream != null;
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether this <see cref="T:System.Net.Security.SslStream" /> uses data encryption.</summary>
		/// <returns>true if data is encrypted before being transmitted over the network and decrypted when it reaches the remote endpoint; otherwise false.</returns>
		public override bool IsEncrypted
		{
			get
			{
				return this.IsAuthenticated;
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether both server and client have been authenticated.</summary>
		/// <returns>true if the server has been authenticated; otherwise false.</returns>
		public override bool IsMutuallyAuthenticated
		{
			get
			{
				return this.IsAuthenticated && ((!this.IsServer) ? (this.LocalCertificate != null) : (this.RemoteCertificate != null));
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the local side of the connection used by this <see cref="T:System.Net.Security.SslStream" /> was authenticated as the server.</summary>
		/// <returns>true if the local endpoint was successfully authenticated as the server side of the authenticated connection; otherwise false.</returns>
		public override bool IsServer
		{
			get
			{
				return this.ssl_stream is SslServerStream;
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the data sent using this stream is signed.</summary>
		/// <returns>true if the data is signed before being transmitted; otherwise false.</returns>
		public override bool IsSigned
		{
			get
			{
				return this.IsAuthenticated;
			}
		}

		/// <summary>Gets or sets the amount of time a read operation blocks waiting for data.</summary>
		/// <returns>A <see cref="T:System.Int32" /> that specifies the amount of time that elapses before a synchronous read operation fails.</returns>
		public override int ReadTimeout
		{
			get
			{
				return base.InnerStream.ReadTimeout;
			}
			set
			{
				base.InnerStream.ReadTimeout = value;
			}
		}

		/// <summary>Gets or sets the amount of time a write operation blocks waiting for data.</summary>
		/// <returns>A <see cref="T:System.Int32" /> that specifies the amount of time that elapses before a synchronous write operation fails. </returns>
		public override int WriteTimeout
		{
			get
			{
				return base.InnerStream.WriteTimeout;
			}
			set
			{
				base.InnerStream.WriteTimeout = value;
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the certificate revocation list is checked during the certificate validation process.</summary>
		/// <returns>true if the certificate revocation list is checked; otherwise, false.</returns>
		public virtual bool CheckCertRevocationStatus
		{
			get
			{
				return this.IsAuthenticated && this.ssl_stream.CheckCertRevocationStatus;
			}
		}

		/// <summary>Gets a value that identifies the bulk encryption algorithm used by this <see cref="T:System.Net.Security.SslStream" />.</summary>
		/// <returns>A <see cref="T:System.Security.Authentication.CipherAlgorithmType" /> value.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Net.Security.SslStream.CipherAlgorithm" /> property was accessed before the completion of the authentication process or the authentication process failed.</exception>
		public virtual System.Security.Authentication.CipherAlgorithmType CipherAlgorithm
		{
			get
			{
				this.CheckConnectionAuthenticated();
				switch (this.ssl_stream.CipherAlgorithm)
				{
				case Mono.Security.Protocol.Tls.CipherAlgorithmType.Des:
					return System.Security.Authentication.CipherAlgorithmType.Des;
				case Mono.Security.Protocol.Tls.CipherAlgorithmType.None:
					return System.Security.Authentication.CipherAlgorithmType.None;
				case Mono.Security.Protocol.Tls.CipherAlgorithmType.Rc2:
					return System.Security.Authentication.CipherAlgorithmType.Rc2;
				case Mono.Security.Protocol.Tls.CipherAlgorithmType.Rc4:
					return System.Security.Authentication.CipherAlgorithmType.Rc4;
				case Mono.Security.Protocol.Tls.CipherAlgorithmType.Rijndael:
				{
					int cipherStrength = this.ssl_stream.CipherStrength;
					if (cipherStrength == 128)
					{
						return System.Security.Authentication.CipherAlgorithmType.Aes128;
					}
					if (cipherStrength == 192)
					{
						return System.Security.Authentication.CipherAlgorithmType.Aes192;
					}
					if (cipherStrength == 256)
					{
						return System.Security.Authentication.CipherAlgorithmType.Aes256;
					}
					break;
				}
				case Mono.Security.Protocol.Tls.CipherAlgorithmType.TripleDes:
					return System.Security.Authentication.CipherAlgorithmType.TripleDes;
				}
				throw new InvalidOperationException("Not supported cipher algorithm is in use. It is likely a bug in SslStream.");
			}
		}

		/// <summary>Gets a value that identifies the strength of the cipher algorithm used by this <see cref="T:System.Net.Security.SslStream" />.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that specifies the strength of the algorithm, in bits.</returns>
		public virtual int CipherStrength
		{
			get
			{
				this.CheckConnectionAuthenticated();
				return this.ssl_stream.CipherStrength;
			}
		}

		/// <summary>Gets the algorithm used for generating message authentication codes (MACs).</summary>
		/// <returns>A <see cref="T:System.Security.Authentication.HashAlgorithmType" /> value.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Net.Security.SslStream.HashAlgorithm" /> property was accessed before the completion of the authentication process or the authentication process failed.</exception>
		public virtual System.Security.Authentication.HashAlgorithmType HashAlgorithm
		{
			get
			{
				this.CheckConnectionAuthenticated();
				switch (this.ssl_stream.HashAlgorithm)
				{
				case Mono.Security.Protocol.Tls.HashAlgorithmType.Md5:
					return System.Security.Authentication.HashAlgorithmType.Md5;
				case Mono.Security.Protocol.Tls.HashAlgorithmType.None:
					return System.Security.Authentication.HashAlgorithmType.None;
				case Mono.Security.Protocol.Tls.HashAlgorithmType.Sha1:
					return System.Security.Authentication.HashAlgorithmType.Sha1;
				default:
					throw new InvalidOperationException("Not supported hash algorithm is in use. It is likely a bug in SslStream.");
				}
			}
		}

		/// <summary>Gets a value that identifies the strength of the hash algorithm used by this instance.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that specifies the strength of the <see cref="T:System.Security.Authentication.HashAlgorithmType" /> algorithm, in bits. Valid values are 128 or 160.</returns>
		public virtual int HashStrength
		{
			get
			{
				this.CheckConnectionAuthenticated();
				return this.ssl_stream.HashStrength;
			}
		}

		/// <summary>Gets the key exchange algorithm used by this <see cref="T:System.Net.Security.SslStream" />.</summary>
		/// <returns>An <see cref="T:System.Security.Authentication.ExchangeAlgorithmType" /> value.</returns>
		public virtual System.Security.Authentication.ExchangeAlgorithmType KeyExchangeAlgorithm
		{
			get
			{
				this.CheckConnectionAuthenticated();
				switch (this.ssl_stream.KeyExchangeAlgorithm)
				{
				case Mono.Security.Protocol.Tls.ExchangeAlgorithmType.DiffieHellman:
					return System.Security.Authentication.ExchangeAlgorithmType.DiffieHellman;
				case Mono.Security.Protocol.Tls.ExchangeAlgorithmType.None:
					return System.Security.Authentication.ExchangeAlgorithmType.None;
				case Mono.Security.Protocol.Tls.ExchangeAlgorithmType.RsaKeyX:
					return System.Security.Authentication.ExchangeAlgorithmType.RsaKeyX;
				case Mono.Security.Protocol.Tls.ExchangeAlgorithmType.RsaSign:
					return System.Security.Authentication.ExchangeAlgorithmType.RsaSign;
				}
				throw new InvalidOperationException("Not supported exchange algorithm is in use. It is likely a bug in SslStream.");
			}
		}

		/// <summary>Gets a value that identifies the strength of the key exchange algorithm used by this instance.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that specifies the strength of the <see cref="T:System.Security.Authentication.ExchangeAlgorithmType" /> algorithm, in bits.</returns>
		public virtual int KeyExchangeStrength
		{
			get
			{
				this.CheckConnectionAuthenticated();
				return this.ssl_stream.KeyExchangeStrength;
			}
		}

		/// <summary>Gets the certificate used to authenticate the local endpoint.</summary>
		/// <returns>An X509Certificate object that represents the certificate supplied for authentication or null if no certificate was supplied.</returns>
		/// <exception cref="T:System.InvalidOperationException">Authentication failed or has not occurred.</exception>
		public virtual X509Certificate LocalCertificate
		{
			get
			{
				this.CheckConnectionAuthenticated();
				return (!this.IsServer) ? ((SslClientStream)this.ssl_stream).SelectedClientCertificate : this.ssl_stream.ServerCertificate;
			}
		}

		/// <summary>Gets the certificate used to authenticate the remote endpoint.</summary>
		/// <returns>An X509Certificate object that represents the certificate supplied for authentication or null if no certificate was supplied.</returns>
		/// <exception cref="T:System.InvalidOperationException">Authentication failed or has not occurred.</exception>
		public virtual X509Certificate RemoteCertificate
		{
			get
			{
				this.CheckConnectionAuthenticated();
				return this.IsServer ? ((SslServerStream)this.ssl_stream).ClientCertificate : this.ssl_stream.ServerCertificate;
			}
		}

		/// <summary>Gets a value that indicates the security protocol used to authenticate this connection.</summary>
		/// <returns>The <see cref="T:System.Security.Authentication.SslProtocols" /> value that represents the protocol used for authentication.</returns>
		public virtual System.Security.Authentication.SslProtocols SslProtocol
		{
			get
			{
				this.CheckConnectionAuthenticated();
				SecurityProtocolType securityProtocol = this.ssl_stream.SecurityProtocol;
				if (securityProtocol == SecurityProtocolType.Default)
				{
					return System.Security.Authentication.SslProtocols.Default;
				}
				if (securityProtocol == SecurityProtocolType.Ssl2)
				{
					return System.Security.Authentication.SslProtocols.Ssl2;
				}
				if (securityProtocol == SecurityProtocolType.Ssl3)
				{
					return System.Security.Authentication.SslProtocols.Ssl3;
				}
				if (securityProtocol != SecurityProtocolType.Tls)
				{
					throw new InvalidOperationException("Not supported SSL/TLS protocol is in use. It is likely a bug in SslStream.");
				}
				return System.Security.Authentication.SslProtocols.Tls;
			}
		}

		private X509Certificate OnCertificateSelection(System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCerts, X509Certificate serverCert, string targetHost, System.Security.Cryptography.X509Certificates.X509CertificateCollection serverRequestedCerts)
		{
			string[] array = new string[(serverRequestedCerts == null) ? 0 : serverRequestedCerts.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = serverRequestedCerts[i].GetIssuerName();
			}
			return this.selection_callback(this, targetHost, clientCerts, serverCert, array);
		}

		/// <summary>Called by clients to begin an asynchronous operation to authenticate the server and optionally the client.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that indicates the status of the asynchronous operation. </returns>
		/// <param name="targetHost">The name of the server that shares this <see cref="T:System.Net.Security.SslStream" />.</param>
		/// <param name="asyncCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the authentication is complete. </param>
		/// <param name="asyncState">A user-defined object that contains information about the operation. This object is passed to the <paramref name="asyncCallback" /> delegate when the operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="targetHost" /> is null.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has already occurred.-or-Server authentication using this <see cref="T:System.Net.Security.SslStream" /> was tried previously.-or- Authentication is already in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		public virtual IAsyncResult BeginAuthenticateAsClient(string targetHost, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginAuthenticateAsClient(targetHost, new System.Security.Cryptography.X509Certificates.X509CertificateCollection(), System.Security.Authentication.SslProtocols.Tls, false, asyncCallback, asyncState);
		}

		/// <summary>Called by clients to begin an asynchronous operation to authenticate the server and optionally the client using the specified certificates and security protocol.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that indicates the status of the asynchronous operation. </returns>
		/// <param name="targetHost">The name of the server that shares this <see cref="T:System.Net.Security.SslStream" />.</param>
		/// <param name="clientCertificates">The <see cref="T:System.Security.Cryptography.X509Certificates.X509CertificateCollection" /> containing client certificates.</param>
		/// <param name="enabledSslProtocols">The <see cref="T:System.Security.Authentication.SslProtocols" /> value that represents the protocol used for authentication.</param>
		/// <param name="checkCertificateRevocation">A <see cref="T:System.Boolean" /> value that specifies whether the certificate revocation list is checked during authentication.</param>
		/// <param name="asyncCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the authentication is complete. </param>
		/// <param name="asyncState">A user-defined object that contains information about the operation. This object is passed to the <paramref name="asyncCallback" /> delegate when the operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="targetHost" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enabledSslProtocols" /> is not a valid <see cref="T:System.Security.Authentication.SslProtocols" />  value.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has already occurred.-or-Server authentication using this <see cref="T:System.Net.Security.SslStream" /> was tried previously.-or- Authentication is already in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		public virtual IAsyncResult BeginAuthenticateAsClient(string targetHost, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates, System.Security.Authentication.SslProtocols sslProtocolType, bool checkCertificateRevocation, AsyncCallback asyncCallback, object asyncState)
		{
			if (this.IsAuthenticated)
			{
				throw new InvalidOperationException("This SslStream is already authenticated");
			}
			SslClientStream sslClientStream = new SslClientStream(base.InnerStream, targetHost, !base.LeaveInnerStreamOpen, this.GetMonoSslProtocol(sslProtocolType), clientCertificates);
			sslClientStream.CheckCertRevocationStatus = checkCertificateRevocation;
			sslClientStream.PrivateKeyCertSelectionDelegate = delegate(X509Certificate cert, string host)
			{
				string certHashString = cert.GetCertHashString();
				foreach (X509Certificate x509Certificate in clientCertificates)
				{
					if (!(x509Certificate.GetCertHashString() != certHashString))
					{
						System.Security.Cryptography.X509Certificates.X509Certificate2 x509Certificate2 = x509Certificate as System.Security.Cryptography.X509Certificates.X509Certificate2;
						x509Certificate2 = (x509Certificate2 ?? new System.Security.Cryptography.X509Certificates.X509Certificate2(x509Certificate));
						return x509Certificate2.PrivateKey;
					}
				}
				return null;
			};
			if (this.validation_callback != null)
			{
				sslClientStream.ServerCertValidationDelegate = delegate(X509Certificate cert, int[] certErrors)
				{
					System.Security.Cryptography.X509Certificates.X509Chain x509Chain = new System.Security.Cryptography.X509Certificates.X509Chain();
					System.Security.Cryptography.X509Certificates.X509Certificate2 x509Certificate = cert as System.Security.Cryptography.X509Certificates.X509Certificate2;
					if (x509Certificate == null)
					{
						x509Certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(cert);
					}
					if (!ServicePointManager.CheckCertificateRevocationList)
					{
						x509Chain.ChainPolicy.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck;
					}
					SslPolicyErrors sslPolicyErrors = SslPolicyErrors.None;
					foreach (int num in certErrors)
					{
						int num2 = num;
						if (num2 != -2146762490)
						{
							if (num2 != -2146762481)
							{
								sslPolicyErrors |= SslPolicyErrors.RemoteCertificateChainErrors;
							}
							else
							{
								sslPolicyErrors |= SslPolicyErrors.RemoteCertificateNameMismatch;
							}
						}
						else
						{
							sslPolicyErrors |= SslPolicyErrors.RemoteCertificateNotAvailable;
						}
					}
					x509Chain.Build(x509Certificate);
					foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus x509ChainStatus in x509Chain.ChainStatus)
					{
						if (x509ChainStatus.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							if ((x509ChainStatus.Status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.PartialChain) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
							{
								sslPolicyErrors |= SslPolicyErrors.RemoteCertificateNotAvailable;
							}
							else
							{
								sslPolicyErrors |= SslPolicyErrors.RemoteCertificateChainErrors;
							}
						}
					}
					return this.validation_callback(this, cert, x509Chain, sslPolicyErrors);
				};
			}
			if (this.selection_callback != null)
			{
				sslClientStream.ClientCertSelectionDelegate = new CertificateSelectionCallback(this.OnCertificateSelection);
			}
			this.ssl_stream = sslClientStream;
			return this.BeginWrite(new byte[0], 0, 0, asyncCallback, asyncState);
		}

		/// <summary>Begins an asynchronous read operation that reads data from the stream and stores it in the specified array.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that indicates the status of the asynchronous operation. </returns>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that receives the bytes read from the stream.</param>
		/// <param name="offset">The zero-based location in <paramref name="buffer" /> at which to begin storing the data read from this stream.</param>
		/// <param name="count">The maximum number of bytes to read from the stream.</param>
		/// <param name="asyncCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the read operation is complete. </param>
		/// <param name="asyncState">A user-defined object that contains information about the read operation. This object is passed to the <paramref name="asyncCallback" /> delegate when the operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" />
		///   <paramref name="&lt;" />
		///   <paramref name="0" />.<paramref name="-or-" /><paramref name="offset" /> &gt; the length of <paramref name="buffer" />.-or-<paramref name="offset" /> + count &gt; the length of <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.IO.IOException">The read operation failed.-or-Encryption is in use, but the data could not be decrypted.</exception>
		/// <exception cref="T:System.NotSupportedException">There is already a read operation in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has not occurred.</exception>
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			this.CheckConnectionAuthenticated();
			return this.ssl_stream.BeginRead(buffer, offset, count, asyncCallback, asyncState);
		}

		/// <summary>Called by servers to begin an asynchronous operation to authenticate the client and optionally the server in a client-server connection.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object indicating the status of the asynchronous operation. </returns>
		/// <param name="serverCertificate">The X509Certificate used to authenticate the server.</param>
		/// <param name="asyncCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the authentication is complete.</param>
		/// <param name="asyncState">A user-defined object that contains information about the operation. This object is passed to the <paramref name="asyncCallback" /> delegate when the operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="serverCertificate" /> is null.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has already occurred.-or-Client authentication using this <see cref="T:System.Net.Security.SslStream" /> was tried previously.-or- Authentication is already in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsServer" /> method is not supported on Windows 95, Windows 98, or Windows Millennium.</exception>
		public virtual IAsyncResult BeginAuthenticateAsServer(X509Certificate serverCertificate, AsyncCallback callback, object asyncState)
		{
			return this.BeginAuthenticateAsServer(serverCertificate, false, System.Security.Authentication.SslProtocols.Tls, false, callback, asyncState);
		}

		/// <summary>Called by servers to begin an asynchronous operation to authenticate the server and optionally the client using the specified certificates, requirements and security protocol.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that indicates the status of the asynchronous operation. </returns>
		/// <param name="serverCertificate">The X509Certificate used to authenticate the server.</param>
		/// <param name="clientCertificateRequired">A <see cref="T:System.Boolean" /> value that specifies whether the client must supply a certificate for authentication.</param>
		/// <param name="enabledSslProtocols">The <see cref="T:System.Security.Authentication.SslProtocols" />  value that represents the protocol used for authentication.</param>
		/// <param name="checkCertificateRevocation">A <see cref="T:System.Boolean" /> value that specifies whether the certificate revocation list is checked during authentication.</param>
		/// <param name="asyncCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the authentication is complete. </param>
		/// <param name="asyncState">A user-defined object that contains information about the operation. This object is passed to the <paramref name="asyncCallback" /> delegate when the operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="serverCertificate" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enabledSslProtocols" /> is not a valid <see cref="T:System.Security.Authentication.SslProtocols" /> value.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has already occurred.-or-Server authentication using this <see cref="T:System.Net.Security.SslStream" /> was tried previously.-or- Authentication is already in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsServer" /> method is not supported on Windows 95, Windows 98, or Windows Millennium.</exception>
		public virtual IAsyncResult BeginAuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, System.Security.Authentication.SslProtocols sslProtocolType, bool checkCertificateRevocation, AsyncCallback callback, object asyncState)
		{
			if (this.IsAuthenticated)
			{
				throw new InvalidOperationException("This SslStream is already authenticated");
			}
			SslServerStream sslServerStream = new SslServerStream(base.InnerStream, serverCertificate, clientCertificateRequired, !base.LeaveInnerStreamOpen, this.GetMonoSslProtocol(sslProtocolType));
			sslServerStream.CheckCertRevocationStatus = checkCertificateRevocation;
			sslServerStream.PrivateKeyCertSelectionDelegate = delegate(X509Certificate cert, string targetHost)
			{
				System.Security.Cryptography.X509Certificates.X509Certificate2 x509Certificate = (serverCertificate as System.Security.Cryptography.X509Certificates.X509Certificate2) ?? new System.Security.Cryptography.X509Certificates.X509Certificate2(serverCertificate);
				return (x509Certificate == null) ? null : x509Certificate.PrivateKey;
			};
			if (this.validation_callback != null)
			{
				sslServerStream.ClientCertValidationDelegate = delegate(X509Certificate cert, int[] certErrors)
				{
					System.Security.Cryptography.X509Certificates.X509Chain x509Chain = null;
					if (cert is System.Security.Cryptography.X509Certificates.X509Certificate2)
					{
						x509Chain = new System.Security.Cryptography.X509Certificates.X509Chain();
						x509Chain.Build((System.Security.Cryptography.X509Certificates.X509Certificate2)cert);
					}
					SslPolicyErrors sslPolicyErrors = (certErrors.Length <= 0) ? SslPolicyErrors.None : SslPolicyErrors.RemoteCertificateChainErrors;
					return this.validation_callback(this, cert, x509Chain, sslPolicyErrors);
				};
			}
			this.ssl_stream = sslServerStream;
			return this.BeginRead(new byte[0], 0, 0, callback, asyncState);
		}

		private SecurityProtocolType GetMonoSslProtocol(System.Security.Authentication.SslProtocols ms)
		{
			if (ms == System.Security.Authentication.SslProtocols.Ssl2)
			{
				return SecurityProtocolType.Ssl2;
			}
			if (ms == System.Security.Authentication.SslProtocols.Ssl3)
			{
				return SecurityProtocolType.Ssl3;
			}
			if (ms != System.Security.Authentication.SslProtocols.Tls)
			{
				return SecurityProtocolType.Default;
			}
			return SecurityProtocolType.Tls;
		}

		/// <summary>Begins an asynchronous write operation that writes <see cref="T:System.Byte" />s from the specified buffer to the stream.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object indicating the status of the asynchronous operation. </returns>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that supplies the bytes to be written to the stream.</param>
		/// <param name="offset">The zero-based location in <paramref name="buffer" /> at which to begin reading bytes to be written to the stream.</param>
		/// <param name="count">An <see cref="T:System.Int32" /> value that specifies the number of bytes to read from <paramref name="buffer" />.</param>
		/// <param name="asyncCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the write operation is complete. </param>
		/// <param name="asyncState">A user-defined object that contains information about the write operation. This object is passed to the <paramref name="asyncCallback" /> delegate when the operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" />
		///   <paramref name="&lt;" />
		///   <paramref name="0" />.<paramref name="-or-" /><paramref name="offset" /> &gt; the length of <paramref name="buffer" />.-or-<paramref name="offset" /> + count &gt; the length of <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.IO.IOException">The write operation failed.</exception>
		/// <exception cref="T:System.NotSupportedException">There is already a write operation in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has not occurred.</exception>
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			this.CheckConnectionAuthenticated();
			return this.ssl_stream.BeginWrite(buffer, offset, count, asyncCallback, asyncState);
		}

		/// <summary>Called by clients to authenticate the server and optionally the client in a client-server connection.</summary>
		/// <param name="targetHost">The name of the server that shares this <see cref="T:System.Net.Security.SslStream" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="targetHost" /> is null.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has already occurred.-or-Server authentication using this <see cref="T:System.Net.Security.SslStream" /> was tried previously.-or- Authentication is already in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		public virtual void AuthenticateAsClient(string targetHost)
		{
			this.AuthenticateAsClient(targetHost, new System.Security.Cryptography.X509Certificates.X509CertificateCollection(), System.Security.Authentication.SslProtocols.Tls, false);
		}

		/// <summary>Called by clients to authenticate the server and optionally the client in a client-server connection. The authentication process uses the specified certificate collection and SSL protocol.</summary>
		/// <param name="targetHost">The name of the server that will share this <see cref="T:System.Net.Security.SslStream" />.</param>
		/// <param name="clientCertificates">The <see cref="T:System.Security.Cryptography.X509Certificates.X509CertificateCollection" /> that contains client certificates.</param>
		/// <param name="enabledSslProtocols">The <see cref="T:System.Security.Authentication.SslProtocols" /> value that represents the protocol used for authentication.</param>
		/// <param name="checkCertificateRevocation">A <see cref="T:System.Boolean" /> value that specifies whether the certificate revocation list is checked during authentication.</param>
		public virtual void AuthenticateAsClient(string targetHost, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates, System.Security.Authentication.SslProtocols sslProtocolType, bool checkCertificateRevocation)
		{
			this.EndAuthenticateAsClient(this.BeginAuthenticateAsClient(targetHost, clientCertificates, sslProtocolType, checkCertificateRevocation, null, null));
		}

		/// <summary>Called by servers to authenticate the server and optionally the client in a client-server connection using the specified certificate.</summary>
		/// <param name="serverCertificate">The certificate used to authenticate the server.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="serverCertificate" /> is null.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has already occurred.-or-Client authentication using this <see cref="T:System.Net.Security.SslStream" /> was tried previously.-or- Authentication is already in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The <see cref="Overload:System.Net.Security.SslStream.AuthenticateAsServer" /> method is not supported on Windows 95, Windows 98, or Windows Millennium.</exception>
		public virtual void AuthenticateAsServer(X509Certificate serverCertificate)
		{
			this.AuthenticateAsServer(serverCertificate, false, System.Security.Authentication.SslProtocols.Tls, false);
		}

		/// <summary>Called by servers to begin an asynchronous operation to authenticate the server and optionally the client using the specified certificates, requirements and security protocol.</summary>
		/// <param name="serverCertificate">The X509Certificate used to authenticate the server.</param>
		/// <param name="clientCertificateRequired">A <see cref="T:System.Boolean" /> value that specifies whether the client must supply a certificate for authentication.</param>
		/// <param name="enabledSslProtocols">The <see cref="T:System.Security.Authentication.SslProtocols" />  value that represents the protocol used for authentication.</param>
		/// <param name="checkCertificateRevocation">A <see cref="T:System.Boolean" /> value that specifies whether the certificate revocation list is checked during authentication.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="serverCertificate" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enabledSslProtocols" /> is not a valid <see cref="T:System.Security.Authentication.SslProtocols" /> value.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has already occurred.-or-Client authentication using this <see cref="T:System.Net.Security.SslStream" /> was tried previously.-or- Authentication is already in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The <see cref="Overload:System.Net.Security.SslStream.AuthenticateAsServer" /> method is not supported on Windows 95, Windows 98, or Windows Millennium.</exception>
		public virtual void AuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, System.Security.Authentication.SslProtocols sslProtocolType, bool checkCertificateRevocation)
		{
			this.EndAuthenticateAsServer(this.BeginAuthenticateAsServer(serverCertificate, clientCertificateRequired, sslProtocolType, checkCertificateRevocation, null, null));
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Security.SslStream" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.ssl_stream != null)
				{
					this.ssl_stream.Dispose();
				}
				this.ssl_stream = null;
			}
			base.Dispose(disposing);
		}

		/// <summary>Ends a pending asynchronous server authentication operation started with a previous call to <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsServer" />.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> instance returned by a call to <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsServer" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not created by a call to <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsServer" />.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">There is no pending server authentication to complete.</exception>
		public virtual void EndAuthenticateAsClient(IAsyncResult asyncResult)
		{
			this.CheckConnectionAuthenticated();
			if (this.CanRead)
			{
				this.ssl_stream.EndRead(asyncResult);
			}
			else
			{
				this.ssl_stream.EndWrite(asyncResult);
			}
		}

		/// <summary>Ends a pending asynchronous client authentication operation started with a previous call to <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsClient" />.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> instance returned by a call to <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsClient" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not created by a call to <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsClient" />.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">There is no pending client authentication to complete.</exception>
		public virtual void EndAuthenticateAsServer(IAsyncResult asyncResult)
		{
			this.CheckConnectionAuthenticated();
			if (this.CanRead)
			{
				this.ssl_stream.EndRead(asyncResult);
			}
			else
			{
				this.ssl_stream.EndWrite(asyncResult);
			}
		}

		/// <summary>Ends an asynchronous read operation started with a previous call to <see cref="M:System.Net.Security.SslStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" />.</summary>
		/// <returns>A <see cref="T:System.Int32" /> value that specifies the number of bytes read from the underlying stream.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> instance returned by a call to <see cref="M:System.Net.Security.SslStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /></param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not created by a call to <see cref="M:System.Net.Security.SslStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">There is no pending read operation to complete.</exception>
		/// <exception cref="T:System.IO.IOException">The read operation failed.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has not occurred.</exception>
		public override int EndRead(IAsyncResult asyncResult)
		{
			this.CheckConnectionAuthenticated();
			return this.ssl_stream.EndRead(asyncResult);
		}

		/// <summary>Ends an asynchronous write operation started with a previous call to <see cref="M:System.Net.Security.SslStream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" />.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> instance returned by a call to <see cref="M:System.Net.Security.SslStream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /></param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not created by a call to <see cref="M:System.Net.Security.SslStream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">There is no pending write operation to complete.</exception>
		/// <exception cref="T:System.IO.IOException">The write operation failed.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has not occurred.</exception>
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.CheckConnectionAuthenticated();
			this.ssl_stream.EndWrite(asyncResult);
		}

		/// <summary>Causes any buffered data to be written to the underlying device.</summary>
		public override void Flush()
		{
			this.CheckConnectionAuthenticated();
			base.InnerStream.Flush();
		}

		/// <summary>Reads data from this stream and stores it in the specified array.</summary>
		/// <returns>A <see cref="T:System.Int32" /> value that specifies the number of bytes read. When there is no more data to be read, returns 0.</returns>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that receives the bytes read from this stream.</param>
		/// <param name="offset">A <see cref="T:System.Int32" /> that contains the zero-based location in <paramref name="buffer" /> at which to begin storing the data read from this stream.</param>
		/// <param name="count">A <see cref="T:System.Int32" /> that contains the maximum number of bytes to read from this stream.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" />
		///   <paramref name="&lt;" />
		///   <paramref name="0" />.<paramref name="-or-" /><paramref name="offset" /> &gt; the length of <paramref name="buffer" />.-or-<paramref name="offset" /> + count &gt; the length of <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.IO.IOException">The read operation failed. Check the inner exception, if present to determine the cause of the failure.</exception>
		/// <exception cref="T:System.NotSupportedException">There is already a read operation in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has not occurred.</exception>
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.EndRead(this.BeginRead(buffer, offset, count, null, null));
		}

		/// <summary>Throws a <see cref="T:System.NotSupportedException" />.</summary>
		/// <returns>Always throws a <see cref="T:System.NotSupportedException" />.</returns>
		/// <param name="offset">This value is ignored.</param>
		/// <param name="origin">This value is ignored.</param>
		/// <exception cref="T:System.NotSupportedException">Seeking is not supported by <see cref="T:System.Net.Security.SslStream" /> objects.</exception>
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("This stream does not support seek operations");
		}

		/// <summary>Sets the length of the underlying stream.</summary>
		/// <param name="value">An <see cref="T:System.Int64" /> value that specifies the length of the stream.</param>
		public override void SetLength(long value)
		{
			base.InnerStream.SetLength(value);
		}

		/// <summary>Write the specified number of <see cref="T:System.Byte" />s to the underlying stream using the specified buffer and offset.</summary>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that supplies the bytes written to the stream.</param>
		/// <param name="offset">A <see cref="T:System.Int32" /> that contains the zero-based location in <paramref name="buffer" /> at which to begin reading bytes to be written to the stream.</param>
		/// <param name="count">A <see cref="T:System.Int32" /> that contains the number of bytes to read from <paramref name="buffer" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" />
		///   <paramref name="&lt;" />
		///   <paramref name="0" />.<paramref name="-or-" /><paramref name="offset" /> &gt; the length of <paramref name="buffer" />.-or-<paramref name="offset" /> + count &gt; the length of <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.IO.IOException">The write operation failed.</exception>
		/// <exception cref="T:System.NotSupportedException">There is already a write operation in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has not occurred.</exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.EndWrite(this.BeginWrite(buffer, offset, count, null, null));
		}

		/// <summary>Writes the specified data to this stream.</summary>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that supplies the bytes written to the stream.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.IO.IOException">The write operation failed.</exception>
		/// <exception cref="T:System.NotSupportedException">There is already a write operation in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has not occurred.</exception>
		public void Write(byte[] buffer)
		{
			this.Write(buffer, 0, buffer.Length);
		}

		private void CheckConnectionAuthenticated()
		{
			if (!this.IsAuthenticated)
			{
				throw new InvalidOperationException("This operation is invalid until it is successfully authenticated");
			}
		}
	}
}
