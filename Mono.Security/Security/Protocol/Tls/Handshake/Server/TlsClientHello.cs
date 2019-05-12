using System;

namespace Mono.Security.Protocol.Tls.Handshake.Server
{
	internal class TlsClientHello : HandshakeMessage
	{
		private byte[] random;

		private byte[] sessionId;

		private short[] cipherSuites;

		private byte[] compressionMethods;

		public TlsClientHello(Context context, byte[] buffer) : base(context, HandshakeType.ClientHello, buffer)
		{
		}

		public override void Update()
		{
			base.Update();
			this.selectCipherSuite();
			this.selectCompressionMethod();
			base.Context.SessionId = this.sessionId;
			base.Context.ClientRandom = this.random;
			base.Context.ProtocolNegotiated = true;
		}

		protected override void ProcessAsSsl3()
		{
			this.ProcessAsTls1();
		}

		protected override void ProcessAsTls1()
		{
			this.processProtocol(base.ReadInt16());
			this.random = base.ReadBytes(32);
			this.sessionId = base.ReadBytes((int)base.ReadByte());
			this.cipherSuites = new short[(int)(base.ReadInt16() / 2)];
			for (int i = 0; i < this.cipherSuites.Length; i++)
			{
				this.cipherSuites[i] = base.ReadInt16();
			}
			this.compressionMethods = new byte[(int)base.ReadByte()];
			for (int j = 0; j < this.compressionMethods.Length; j++)
			{
				this.compressionMethods[j] = base.ReadByte();
			}
		}

		private void processProtocol(short protocol)
		{
			SecurityProtocolType securityProtocolType = base.Context.DecodeProtocolCode(protocol);
			if ((securityProtocolType & base.Context.SecurityProtocolFlags) == securityProtocolType || (base.Context.SecurityProtocolFlags & SecurityProtocolType.Default) == SecurityProtocolType.Default)
			{
				base.Context.SecurityProtocol = securityProtocolType;
				base.Context.SupportedCiphers.Clear();
				base.Context.SupportedCiphers = null;
				base.Context.SupportedCiphers = CipherSuiteFactory.GetSupportedCiphers(securityProtocolType);
				return;
			}
			throw new TlsException(AlertDescription.ProtocolVersion, "Incorrect protocol version received from server");
		}

		private void selectCipherSuite()
		{
			for (int i = 0; i < this.cipherSuites.Length; i++)
			{
				int index;
				if ((index = base.Context.SupportedCiphers.IndexOf(this.cipherSuites[i])) != -1)
				{
					base.Context.Negotiating.Cipher = base.Context.SupportedCiphers[index];
					break;
				}
			}
			if (base.Context.Negotiating.Cipher == null)
			{
				throw new TlsException(AlertDescription.InsuficientSecurity, "Insuficient Security");
			}
		}

		private void selectCompressionMethod()
		{
			base.Context.CompressionMethod = SecurityCompressionType.None;
		}
	}
}
