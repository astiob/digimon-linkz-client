using System;
using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Protocol.Tls
{
	internal class ClientContext : Context
	{
		private SslClientStream sslStream;

		private short clientHelloProtocol;

		public ClientContext(SslClientStream stream, SecurityProtocolType securityProtocolType, string targetHost, X509CertificateCollection clientCertificates) : base(securityProtocolType)
		{
			this.sslStream = stream;
			base.ClientSettings.Certificates = clientCertificates;
			base.ClientSettings.TargetHost = targetHost;
		}

		public SslClientStream SslStream
		{
			get
			{
				return this.sslStream;
			}
		}

		public short ClientHelloProtocol
		{
			get
			{
				return this.clientHelloProtocol;
			}
			set
			{
				this.clientHelloProtocol = value;
			}
		}

		public override void Clear()
		{
			this.clientHelloProtocol = 0;
			base.Clear();
		}
	}
}
