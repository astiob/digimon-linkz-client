using Mono.Security.Protocol.Tls.Handshake;
using Mono.Security.X509;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Protocol.Tls
{
	public class SslServerStream : SslStreamBase
	{
		public SslServerStream(Stream stream, System.Security.Cryptography.X509Certificates.X509Certificate serverCertificate) : this(stream, serverCertificate, false, false, SecurityProtocolType.Default)
		{
		}

		public SslServerStream(Stream stream, System.Security.Cryptography.X509Certificates.X509Certificate serverCertificate, bool clientCertificateRequired, bool ownsStream) : this(stream, serverCertificate, clientCertificateRequired, ownsStream, SecurityProtocolType.Default)
		{
		}

		public SslServerStream(Stream stream, System.Security.Cryptography.X509Certificates.X509Certificate serverCertificate, bool clientCertificateRequired, bool requestClientCertificate, bool ownsStream) : this(stream, serverCertificate, clientCertificateRequired, requestClientCertificate, ownsStream, SecurityProtocolType.Default)
		{
		}

		public SslServerStream(Stream stream, System.Security.Cryptography.X509Certificates.X509Certificate serverCertificate, bool clientCertificateRequired, bool ownsStream, SecurityProtocolType securityProtocolType) : this(stream, serverCertificate, clientCertificateRequired, false, ownsStream, securityProtocolType)
		{
		}

		public SslServerStream(Stream stream, System.Security.Cryptography.X509Certificates.X509Certificate serverCertificate, bool clientCertificateRequired, bool requestClientCertificate, bool ownsStream, SecurityProtocolType securityProtocolType) : base(stream, ownsStream)
		{
			this.context = new ServerContext(this, securityProtocolType, serverCertificate, clientCertificateRequired, requestClientCertificate);
			this.protocol = new ServerRecordProtocol(this.innerStream, (ServerContext)this.context);
		}

		internal event CertificateValidationCallback ClientCertValidation;

		internal event PrivateKeySelectionCallback PrivateKeySelection;

		public event CertificateValidationCallback2 ClientCertValidation2;

		public System.Security.Cryptography.X509Certificates.X509Certificate ClientCertificate
		{
			get
			{
				if (this.context.HandshakeState == HandshakeState.Finished)
				{
					return this.context.ClientSettings.ClientCertificate;
				}
				return null;
			}
		}

		public CertificateValidationCallback ClientCertValidationDelegate
		{
			get
			{
				return this.ClientCertValidation;
			}
			set
			{
				this.ClientCertValidation = value;
			}
		}

		public PrivateKeySelectionCallback PrivateKeyCertSelectionDelegate
		{
			get
			{
				return this.PrivateKeySelection;
			}
			set
			{
				this.PrivateKeySelection = value;
			}
		}

		~SslServerStream()
		{
			this.Dispose(false);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				this.ClientCertValidation = null;
				this.PrivateKeySelection = null;
			}
		}

		internal override IAsyncResult OnBeginNegotiateHandshake(AsyncCallback callback, object state)
		{
			if (this.context.HandshakeState != HandshakeState.None)
			{
				this.context.Clear();
			}
			this.context.SupportedCiphers = CipherSuiteFactory.GetSupportedCiphers(this.context.SecurityProtocol);
			this.context.HandshakeState = HandshakeState.Started;
			return this.protocol.BeginReceiveRecord(this.innerStream, callback, state);
		}

		internal override void OnNegotiateHandshakeCallback(IAsyncResult asyncResult)
		{
			this.protocol.EndReceiveRecord(asyncResult);
			if (this.context.LastHandshakeMsg != HandshakeType.ClientHello)
			{
				this.protocol.SendAlert(AlertDescription.UnexpectedMessage);
			}
			this.protocol.SendRecord(HandshakeType.ServerHello);
			this.protocol.SendRecord(HandshakeType.Certificate);
			if (this.context.Negotiating.Cipher.IsExportable)
			{
				this.protocol.SendRecord(HandshakeType.ServerKeyExchange);
			}
			bool flag = false;
			if (this.context.Negotiating.Cipher.IsExportable || ((ServerContext)this.context).ClientCertificateRequired || ((ServerContext)this.context).RequestClientCertificate)
			{
				this.protocol.SendRecord(HandshakeType.CertificateRequest);
				flag = true;
			}
			this.protocol.SendRecord(HandshakeType.ServerHelloDone);
			while (this.context.LastHandshakeMsg != HandshakeType.Finished)
			{
				byte[] array = this.protocol.ReceiveRecord(this.innerStream);
				if (array == null || array.Length == 0)
				{
					throw new TlsException(AlertDescription.HandshakeFailiure, "The client stopped the handshake.");
				}
			}
			if (flag)
			{
				System.Security.Cryptography.X509Certificates.X509Certificate clientCertificate = this.context.ClientSettings.ClientCertificate;
				if (clientCertificate == null && ((ServerContext)this.context).ClientCertificateRequired)
				{
					throw new TlsException(AlertDescription.BadCertificate, "No certificate received from client.");
				}
				if (!this.RaiseClientCertificateValidation(clientCertificate, new int[0]))
				{
					throw new TlsException(AlertDescription.BadCertificate, "Client certificate not accepted.");
				}
			}
			this.protocol.SendChangeCipherSpec();
			this.protocol.SendRecord(HandshakeType.Finished);
			this.context.HandshakeState = HandshakeState.Finished;
			this.context.HandshakeMessages.Reset();
			this.context.ClearKeyInfo();
		}

		internal override System.Security.Cryptography.X509Certificates.X509Certificate OnLocalCertificateSelection(System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates, System.Security.Cryptography.X509Certificates.X509Certificate serverCertificate, string targetHost, System.Security.Cryptography.X509Certificates.X509CertificateCollection serverRequestedCertificates)
		{
			throw new NotSupportedException();
		}

		internal override bool OnRemoteCertificateValidation(System.Security.Cryptography.X509Certificates.X509Certificate certificate, int[] errors)
		{
			if (this.ClientCertValidation != null)
			{
				return this.ClientCertValidation(certificate, errors);
			}
			return errors != null && errors.Length == 0;
		}

		internal override bool HaveRemoteValidation2Callback
		{
			get
			{
				return this.ClientCertValidation2 != null;
			}
		}

		internal override ValidationResult OnRemoteCertificateValidation2(Mono.Security.X509.X509CertificateCollection collection)
		{
			CertificateValidationCallback2 clientCertValidation = this.ClientCertValidation2;
			if (clientCertValidation != null)
			{
				return clientCertValidation(collection);
			}
			return null;
		}

		internal bool RaiseClientCertificateValidation(System.Security.Cryptography.X509Certificates.X509Certificate certificate, int[] certificateErrors)
		{
			return base.RaiseRemoteCertificateValidation(certificate, certificateErrors);
		}

		internal override AsymmetricAlgorithm OnLocalPrivateKeySelection(System.Security.Cryptography.X509Certificates.X509Certificate certificate, string targetHost)
		{
			if (this.PrivateKeySelection != null)
			{
				return this.PrivateKeySelection(certificate, targetHost);
			}
			return null;
		}

		internal AsymmetricAlgorithm RaisePrivateKeySelection(System.Security.Cryptography.X509Certificates.X509Certificate certificate, string targetHost)
		{
			return base.RaiseLocalPrivateKeySelection(certificate, targetHost);
		}
	}
}
