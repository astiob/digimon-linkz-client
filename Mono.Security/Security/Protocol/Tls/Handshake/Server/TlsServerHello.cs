using System;

namespace Mono.Security.Protocol.Tls.Handshake.Server
{
	internal class TlsServerHello : HandshakeMessage
	{
		private int unixTime;

		private byte[] random;

		public TlsServerHello(Context context) : base(context, HandshakeType.ServerHello)
		{
		}

		public override void Update()
		{
			base.Update();
			TlsStream tlsStream = new TlsStream();
			tlsStream.Write(this.unixTime);
			tlsStream.Write(this.random);
			base.Context.ServerRandom = tlsStream.ToArray();
			tlsStream.Reset();
			tlsStream.Write(base.Context.ClientRandom);
			tlsStream.Write(base.Context.ServerRandom);
			base.Context.RandomCS = tlsStream.ToArray();
			tlsStream.Reset();
			tlsStream.Write(base.Context.ServerRandom);
			tlsStream.Write(base.Context.ClientRandom);
			base.Context.RandomSC = tlsStream.ToArray();
			tlsStream.Reset();
		}

		protected override void ProcessAsSsl3()
		{
			this.ProcessAsTls1();
		}

		protected override void ProcessAsTls1()
		{
			base.Write(base.Context.Protocol);
			this.unixTime = base.Context.GetUnixTime();
			base.Write(this.unixTime);
			this.random = base.Context.GetSecureRandomBytes(28);
			base.Write(this.random);
			if (base.Context.SessionId == null)
			{
				this.WriteByte(0);
			}
			else
			{
				this.WriteByte((byte)base.Context.SessionId.Length);
				base.Write(base.Context.SessionId);
			}
			base.Write(base.Context.Negotiating.Cipher.Code);
			this.WriteByte((byte)base.Context.CompressionMethod);
		}
	}
}
