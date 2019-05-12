using Mono.Security.X509;
using System;

namespace Mono.Security.Protocol.Tls.Handshake.Server
{
	internal class TlsServerCertificateRequest : HandshakeMessage
	{
		public TlsServerCertificateRequest(Context context) : base(context, HandshakeType.CertificateRequest)
		{
		}

		protected override void ProcessAsSsl3()
		{
			this.ProcessAsTls1();
		}

		protected override void ProcessAsTls1()
		{
			ServerContext serverContext = (ServerContext)base.Context;
			int num = serverContext.ServerSettings.CertificateTypes.Length;
			this.WriteByte(Convert.ToByte(num));
			for (int i = 0; i < num; i++)
			{
				this.WriteByte((byte)serverContext.ServerSettings.CertificateTypes[i]);
			}
			if (serverContext.ServerSettings.DistinguisedNames.Length > 0)
			{
				TlsStream tlsStream = new TlsStream();
				foreach (string rdn in serverContext.ServerSettings.DistinguisedNames)
				{
					byte[] bytes = X501.FromString(rdn).GetBytes();
					tlsStream.Write((short)bytes.Length);
					tlsStream.Write(bytes);
				}
				base.Write((short)tlsStream.Length);
				base.Write(tlsStream.ToArray());
			}
			else
			{
				base.Write(0);
			}
		}
	}
}
