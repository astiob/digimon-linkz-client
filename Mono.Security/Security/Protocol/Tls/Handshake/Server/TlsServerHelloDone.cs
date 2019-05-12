using System;

namespace Mono.Security.Protocol.Tls.Handshake.Server
{
	internal class TlsServerHelloDone : HandshakeMessage
	{
		public TlsServerHelloDone(Context context) : base(context, HandshakeType.ServerHelloDone)
		{
		}

		protected override void ProcessAsSsl3()
		{
		}

		protected override void ProcessAsTls1()
		{
		}
	}
}
