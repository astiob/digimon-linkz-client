using System;

namespace Mono.Security.Protocol.Tls.Handshake.Client
{
	internal class TlsServerHello : HandshakeMessage
	{
		private SecurityCompressionType compressionMethod;

		private byte[] random;

		private byte[] sessionId;

		private CipherSuite cipherSuite;

		public TlsServerHello(Context context, byte[] buffer) : base(context, HandshakeType.ServerHello, buffer)
		{
		}

		public override void Update()
		{
			base.Update();
			base.Context.SessionId = this.sessionId;
			base.Context.ServerRandom = this.random;
			base.Context.Negotiating.Cipher = this.cipherSuite;
			base.Context.CompressionMethod = this.compressionMethod;
			base.Context.ProtocolNegotiated = true;
			int num = base.Context.ClientRandom.Length;
			int num2 = base.Context.ServerRandom.Length;
			int num3 = num + num2;
			byte[] array = new byte[num3];
			Buffer.BlockCopy(base.Context.ClientRandom, 0, array, 0, num);
			Buffer.BlockCopy(base.Context.ServerRandom, 0, array, num, num2);
			base.Context.RandomCS = array;
			byte[] array2 = new byte[num3];
			Buffer.BlockCopy(base.Context.ServerRandom, 0, array2, 0, num2);
			Buffer.BlockCopy(base.Context.ClientRandom, 0, array2, num2, num);
			base.Context.RandomSC = array2;
		}

		protected override void ProcessAsSsl3()
		{
			this.ProcessAsTls1();
		}

		protected override void ProcessAsTls1()
		{
			this.processProtocol(base.ReadInt16());
			this.random = base.ReadBytes(32);
			int num = (int)base.ReadByte();
			if (num > 0)
			{
				this.sessionId = base.ReadBytes(num);
				ClientSessionCache.Add(base.Context.ClientSettings.TargetHost, this.sessionId);
				base.Context.AbbreviatedHandshake = HandshakeMessage.Compare(this.sessionId, base.Context.SessionId);
			}
			else
			{
				base.Context.AbbreviatedHandshake = false;
			}
			short code = base.ReadInt16();
			if (base.Context.SupportedCiphers.IndexOf(code) == -1)
			{
				throw new TlsException(AlertDescription.InsuficientSecurity, "Invalid cipher suite received from server");
			}
			this.cipherSuite = base.Context.SupportedCiphers[code];
			this.compressionMethod = (SecurityCompressionType)base.ReadByte();
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
	}
}
