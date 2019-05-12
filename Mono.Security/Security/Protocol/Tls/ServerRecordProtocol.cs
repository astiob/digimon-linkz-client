using Mono.Security.Protocol.Tls.Handshake;
using Mono.Security.Protocol.Tls.Handshake.Server;
using System;
using System.Globalization;
using System.IO;

namespace Mono.Security.Protocol.Tls
{
	internal class ServerRecordProtocol : RecordProtocol
	{
		public ServerRecordProtocol(Stream innerStream, ServerContext context) : base(innerStream, context)
		{
		}

		public override HandshakeMessage GetMessage(HandshakeType type)
		{
			return this.createServerHandshakeMessage(type);
		}

		protected override void ProcessHandshakeMessage(TlsStream handMsg)
		{
			HandshakeType handshakeType = (HandshakeType)handMsg.ReadByte();
			int num = handMsg.ReadInt24();
			byte[] array = new byte[num];
			handMsg.Read(array, 0, num);
			HandshakeMessage handshakeMessage = this.createClientHandshakeMessage(handshakeType, array);
			handshakeMessage.Process();
			base.Context.LastHandshakeMsg = handshakeType;
			if (handshakeMessage != null)
			{
				handshakeMessage.Update();
				base.Context.HandshakeMessages.WriteByte((byte)handshakeType);
				base.Context.HandshakeMessages.WriteInt24(num);
				base.Context.HandshakeMessages.Write(array, 0, array.Length);
			}
		}

		private HandshakeMessage createClientHandshakeMessage(HandshakeType type, byte[] buffer)
		{
			switch (type)
			{
			case HandshakeType.CertificateVerify:
				return new TlsClientCertificateVerify(this.context, buffer);
			case HandshakeType.ClientKeyExchange:
				return new TlsClientKeyExchange(this.context, buffer);
			default:
				if (type == HandshakeType.ClientHello)
				{
					return new TlsClientHello(this.context, buffer);
				}
				if (type != HandshakeType.Certificate)
				{
					throw new TlsException(AlertDescription.UnexpectedMessage, string.Format(CultureInfo.CurrentUICulture, "Unknown server handshake message received ({0})", new object[]
					{
						type.ToString()
					}));
				}
				return new TlsClientCertificate(this.context, buffer);
			case HandshakeType.Finished:
				return new TlsClientFinished(this.context, buffer);
			}
		}

		private HandshakeMessage createServerHandshakeMessage(HandshakeType type)
		{
			switch (type)
			{
			case HandshakeType.Certificate:
				return new TlsServerCertificate(this.context);
			case HandshakeType.ServerKeyExchange:
				return new TlsServerKeyExchange(this.context);
			case HandshakeType.CertificateRequest:
				return new TlsServerCertificateRequest(this.context);
			case HandshakeType.ServerHelloDone:
				return new TlsServerHelloDone(this.context);
			default:
				switch (type)
				{
				case HandshakeType.HelloRequest:
					this.SendRecord(HandshakeType.ClientHello);
					return null;
				case HandshakeType.ServerHello:
					return new TlsServerHello(this.context);
				}
				throw new InvalidOperationException("Unknown server handshake message type: " + type.ToString());
			case HandshakeType.Finished:
				return new TlsServerFinished(this.context);
			}
		}
	}
}
