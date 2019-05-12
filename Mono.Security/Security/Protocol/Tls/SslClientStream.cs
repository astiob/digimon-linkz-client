using Mono.Security.Protocol.Tls.Handshake;
using Mono.Security.X509;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Protocol.Tls
{
	public class SslClientStream : SslStreamBase
	{
		public SslClientStream(Stream stream, string targetHost, bool ownsStream) : this(stream, targetHost, ownsStream, SecurityProtocolType.Default, null)
		{
		}

		public SslClientStream(Stream stream, string targetHost, System.Security.Cryptography.X509Certificates.X509Certificate clientCertificate) : this(stream, targetHost, false, SecurityProtocolType.Default, new System.Security.Cryptography.X509Certificates.X509CertificateCollection(new System.Security.Cryptography.X509Certificates.X509Certificate[]
		{
			clientCertificate
		}))
		{
		}

		public SslClientStream(Stream stream, string targetHost, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates) : this(stream, targetHost, false, SecurityProtocolType.Default, clientCertificates)
		{
		}

		public SslClientStream(Stream stream, string targetHost, bool ownsStream, SecurityProtocolType securityProtocolType) : this(stream, targetHost, ownsStream, securityProtocolType, new System.Security.Cryptography.X509Certificates.X509CertificateCollection())
		{
		}

		public SslClientStream(Stream stream, string targetHost, bool ownsStream, SecurityProtocolType securityProtocolType, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates) : base(stream, ownsStream)
		{
			if (targetHost == null || targetHost.Length == 0)
			{
				throw new ArgumentNullException("targetHost is null or an empty string.");
			}
			this.context = new ClientContext(this, securityProtocolType, targetHost, clientCertificates);
			this.protocol = new ClientRecordProtocol(this.innerStream, (ClientContext)this.context);
		}

		internal event CertificateValidationCallback ServerCertValidation;

		internal event CertificateSelectionCallback ClientCertSelection;

		internal event PrivateKeySelectionCallback PrivateKeySelection;

		public event CertificateValidationCallback2 ServerCertValidation2;

		internal Stream InputBuffer
		{
			get
			{
				return this.inputBuffer;
			}
		}

		public System.Security.Cryptography.X509Certificates.X509CertificateCollection ClientCertificates
		{
			get
			{
				return this.context.ClientSettings.Certificates;
			}
		}

		public System.Security.Cryptography.X509Certificates.X509Certificate SelectedClientCertificate
		{
			get
			{
				return this.context.ClientSettings.ClientCertificate;
			}
		}

		public CertificateValidationCallback ServerCertValidationDelegate
		{
			get
			{
				return this.ServerCertValidation;
			}
			set
			{
				this.ServerCertValidation = value;
			}
		}

		public CertificateSelectionCallback ClientCertSelectionDelegate
		{
			get
			{
				return this.ClientCertSelection;
			}
			set
			{
				this.ClientCertSelection = value;
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

		~SslClientStream()
		{
			base.Dispose(false);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				this.ServerCertValidation = null;
				this.ClientCertSelection = null;
				this.PrivateKeySelection = null;
				this.ServerCertValidation2 = null;
			}
		}

		internal override IAsyncResult OnBeginNegotiateHandshake(AsyncCallback callback, object state)
		{
			IAsyncResult result;
			try
			{
				if (this.context.HandshakeState != HandshakeState.None)
				{
					this.context.Clear();
				}
				this.context.SupportedCiphers = CipherSuiteFactory.GetSupportedCiphers(this.context.SecurityProtocol);
				this.context.HandshakeState = HandshakeState.Started;
				result = this.protocol.BeginSendRecord(HandshakeType.ClientHello, callback, state);
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
			return result;
		}

		private void SafeReceiveRecord(Stream s)
		{
			byte[] array = this.protocol.ReceiveRecord(s);
			if (array == null || array.Length == 0)
			{
				throw new TlsException(AlertDescription.HandshakeFailiure, "The server stopped the handshake.");
			}
		}

		internal override void OnNegotiateHandshakeCallback(IAsyncResult asyncResult)
		{
			this.protocol.EndSendRecord(asyncResult);
			while (this.context.LastHandshakeMsg != HandshakeType.ServerHelloDone)
			{
				this.SafeReceiveRecord(this.innerStream);
				if (this.context.AbbreviatedHandshake && this.context.LastHandshakeMsg == HandshakeType.ServerHello)
				{
					break;
				}
			}
			if (this.context.AbbreviatedHandshake)
			{
				ClientSessionCache.SetContextFromCache(this.context);
				this.context.Negotiating.Cipher.ComputeKeys();
				this.context.Negotiating.Cipher.InitializeCipher();
				this.protocol.SendChangeCipherSpec();
				while (this.context.HandshakeState != HandshakeState.Finished)
				{
					this.SafeReceiveRecord(this.innerStream);
				}
				this.protocol.SendRecord(HandshakeType.Finished);
			}
			else
			{
				bool flag = this.context.ServerSettings.CertificateRequest;
				if (this.context.SecurityProtocol == SecurityProtocolType.Ssl3)
				{
					flag = (this.context.ClientSettings.Certificates != null && this.context.ClientSettings.Certificates.Count > 0);
				}
				if (flag)
				{
					this.protocol.SendRecord(HandshakeType.Certificate);
				}
				this.protocol.SendRecord(HandshakeType.ClientKeyExchange);
				this.context.Negotiating.Cipher.InitializeCipher();
				if (flag && this.context.ClientSettings.ClientCertificate != null)
				{
					this.protocol.SendRecord(HandshakeType.CertificateVerify);
				}
				this.protocol.SendChangeCipherSpec();
				this.protocol.SendRecord(HandshakeType.Finished);
				while (this.context.HandshakeState != HandshakeState.Finished)
				{
					this.SafeReceiveRecord(this.innerStream);
				}
			}
			this.context.HandshakeMessages.Reset();
			this.context.ClearKeyInfo();
		}

		internal override System.Security.Cryptography.X509Certificates.X509Certificate OnLocalCertificateSelection(System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates, System.Security.Cryptography.X509Certificates.X509Certificate serverCertificate, string targetHost, System.Security.Cryptography.X509Certificates.X509CertificateCollection serverRequestedCertificates)
		{
			if (this.ClientCertSelection != null)
			{
				return this.ClientCertSelection(clientCertificates, serverCertificate, targetHost, serverRequestedCertificates);
			}
			return null;
		}

		internal override bool HaveRemoteValidation2Callback
		{
			get
			{
				return this.ServerCertValidation2 != null;
			}
		}

		internal override ValidationResult OnRemoteCertificateValidation2(Mono.Security.X509.X509CertificateCollection collection)
		{
			CertificateValidationCallback2 serverCertValidation = this.ServerCertValidation2;
			if (serverCertValidation != null)
			{
				return serverCertValidation(collection);
			}
			return null;
		}

		internal override bool OnRemoteCertificateValidation(System.Security.Cryptography.X509Certificates.X509Certificate certificate, int[] errors)
		{
			if (this.ServerCertValidation != null)
			{
				return this.ServerCertValidation(certificate, errors);
			}
			return errors != null && errors.Length == 0;
		}

		internal virtual bool RaiseServerCertificateValidation(System.Security.Cryptography.X509Certificates.X509Certificate certificate, int[] certificateErrors)
		{
			return base.RaiseRemoteCertificateValidation(certificate, certificateErrors);
		}

		internal virtual ValidationResult RaiseServerCertificateValidation2(Mono.Security.X509.X509CertificateCollection collection)
		{
			return base.RaiseRemoteCertificateValidation2(collection);
		}

		internal System.Security.Cryptography.X509Certificates.X509Certificate RaiseClientCertificateSelection(System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates, System.Security.Cryptography.X509Certificates.X509Certificate serverCertificate, string targetHost, System.Security.Cryptography.X509Certificates.X509CertificateCollection serverRequestedCertificates)
		{
			return base.RaiseLocalCertificateSelection(clientCertificates, serverCertificate, targetHost, serverRequestedCertificates);
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
