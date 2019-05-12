using Mono.Security.Protocol.Tls.Handshake;
using Mono.Security.X509;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Protocol.Tls
{
	internal class ServerContext : Context
	{
		private SslServerStream sslStream;

		private bool request_client_certificate;

		private bool clientCertificateRequired;

		public ServerContext(SslServerStream stream, SecurityProtocolType securityProtocolType, System.Security.Cryptography.X509Certificates.X509Certificate serverCertificate, bool clientCertificateRequired, bool requestClientCertificate) : base(securityProtocolType)
		{
			this.sslStream = stream;
			this.clientCertificateRequired = clientCertificateRequired;
			this.request_client_certificate = requestClientCertificate;
			Mono.Security.X509.X509Certificate value = new Mono.Security.X509.X509Certificate(serverCertificate.GetRawCertData());
			base.ServerSettings.Certificates = new Mono.Security.X509.X509CertificateCollection();
			base.ServerSettings.Certificates.Add(value);
			base.ServerSettings.UpdateCertificateRSA();
			base.ServerSettings.CertificateTypes = new ClientCertificateType[1];
			base.ServerSettings.CertificateTypes[0] = ClientCertificateType.RSA;
			Mono.Security.X509.X509CertificateCollection trustedRootCertificates = X509StoreManager.TrustedRootCertificates;
			string[] array = new string[trustedRootCertificates.Count];
			int num = 0;
			foreach (Mono.Security.X509.X509Certificate x509Certificate in trustedRootCertificates)
			{
				array[num++] = x509Certificate.IssuerName;
			}
			base.ServerSettings.DistinguisedNames = array;
		}

		public SslServerStream SslStream
		{
			get
			{
				return this.sslStream;
			}
		}

		public bool ClientCertificateRequired
		{
			get
			{
				return this.clientCertificateRequired;
			}
		}

		public bool RequestClientCertificate
		{
			get
			{
				return this.request_client_certificate;
			}
		}
	}
}
