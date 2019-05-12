using Mono.Security.X509;
using System;

namespace Mono.Security.Protocol.Tls.Handshake.Server
{
	internal class TlsServerCertificate : HandshakeMessage
	{
		public TlsServerCertificate(Context context) : base(context, HandshakeType.Certificate)
		{
		}

		protected override void ProcessAsSsl3()
		{
			this.ProcessAsTls1();
		}

		protected override void ProcessAsTls1()
		{
			TlsStream tlsStream = new TlsStream();
			foreach (X509Certificate x509Certificate in base.Context.ServerSettings.Certificates)
			{
				tlsStream.WriteInt24(x509Certificate.RawData.Length);
				tlsStream.Write(x509Certificate.RawData);
			}
			base.WriteInt24(Convert.ToInt32(tlsStream.Length));
			base.Write(tlsStream.ToArray());
			tlsStream.Close();
		}
	}
}
