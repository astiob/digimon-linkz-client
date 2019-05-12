using System;

namespace Mono.Security.Protocol.Tls.Handshake.Client
{
	internal class TlsServerHelloDone : HandshakeMessage
	{
		public TlsServerHelloDone(Context context, byte[] buffer) : base(context, HandshakeType.ServerHelloDone, buffer)
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
