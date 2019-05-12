using Mono.Security.Cryptography;
using Mono.Security.X509;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Protocol.Tls
{
	internal sealed class TlsClientSettings
	{
		private string targetHost;

		private System.Security.Cryptography.X509Certificates.X509CertificateCollection certificates;

		private System.Security.Cryptography.X509Certificates.X509Certificate clientCertificate;

		private RSAManaged certificateRSA;

		public TlsClientSettings()
		{
			this.certificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
			this.targetHost = string.Empty;
		}

		public string TargetHost
		{
			get
			{
				return this.targetHost;
			}
			set
			{
				this.targetHost = value;
			}
		}

		public System.Security.Cryptography.X509Certificates.X509CertificateCollection Certificates
		{
			get
			{
				return this.certificates;
			}
			set
			{
				this.certificates = value;
			}
		}

		public System.Security.Cryptography.X509Certificates.X509Certificate ClientCertificate
		{
			get
			{
				return this.clientCertificate;
			}
			set
			{
				this.clientCertificate = value;
				this.UpdateCertificateRSA();
			}
		}

		public RSAManaged CertificateRSA
		{
			get
			{
				return this.certificateRSA;
			}
		}

		public void UpdateCertificateRSA()
		{
			if (this.clientCertificate == null)
			{
				this.certificateRSA = null;
			}
			else
			{
				Mono.Security.X509.X509Certificate x509Certificate = new Mono.Security.X509.X509Certificate(this.clientCertificate.GetRawCertData());
				this.certificateRSA = new RSAManaged(x509Certificate.RSA.KeySize);
				this.certificateRSA.ImportParameters(x509Certificate.RSA.ExportParameters(false));
			}
		}
	}
}
