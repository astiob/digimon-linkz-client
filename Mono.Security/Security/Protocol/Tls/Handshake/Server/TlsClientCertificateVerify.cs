using Mono.Security.Cryptography;
using System;

namespace Mono.Security.Protocol.Tls.Handshake.Server
{
	internal class TlsClientCertificateVerify : HandshakeMessage
	{
		public TlsClientCertificateVerify(Context context, byte[] buffer) : base(context, HandshakeType.CertificateVerify, buffer)
		{
		}

		protected override void ProcessAsSsl3()
		{
			ServerContext serverContext = (ServerContext)base.Context;
			int count = (int)base.ReadInt16();
			byte[] rgbSignature = base.ReadBytes(count);
			SslHandshakeHash sslHandshakeHash = new SslHandshakeHash(serverContext.MasterSecret);
			sslHandshakeHash.TransformFinalBlock(serverContext.HandshakeMessages.ToArray(), 0, (int)serverContext.HandshakeMessages.Length);
			if (!sslHandshakeHash.VerifySignature(serverContext.ClientSettings.CertificateRSA, rgbSignature))
			{
				throw new TlsException(AlertDescription.HandshakeFailiure, "Handshake Failure.");
			}
		}

		protected override void ProcessAsTls1()
		{
			ServerContext serverContext = (ServerContext)base.Context;
			int count = (int)base.ReadInt16();
			byte[] rgbSignature = base.ReadBytes(count);
			MD5SHA1 md5SHA = new MD5SHA1();
			md5SHA.ComputeHash(serverContext.HandshakeMessages.ToArray(), 0, (int)serverContext.HandshakeMessages.Length);
			if (!md5SHA.VerifySignature(serverContext.ClientSettings.CertificateRSA, rgbSignature))
			{
				throw new TlsException(AlertDescription.HandshakeFailiure, "Handshake Failure.");
			}
		}
	}
}
