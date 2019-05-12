using Mono.Security.Cryptography;
using System;
using System.Security.Cryptography;

namespace Mono.Security.Protocol.Tls.Handshake.Client
{
	internal class TlsServerKeyExchange : HandshakeMessage
	{
		private RSAParameters rsaParams;

		private byte[] signedParams;

		public TlsServerKeyExchange(Context context, byte[] buffer) : base(context, HandshakeType.ServerKeyExchange, buffer)
		{
			this.verifySignature();
		}

		public override void Update()
		{
			base.Update();
			base.Context.ServerSettings.ServerKeyExchange = true;
			base.Context.ServerSettings.RsaParameters = this.rsaParams;
			base.Context.ServerSettings.SignedParams = this.signedParams;
		}

		protected override void ProcessAsSsl3()
		{
			this.ProcessAsTls1();
		}

		protected override void ProcessAsTls1()
		{
			this.rsaParams = default(RSAParameters);
			this.rsaParams.Modulus = base.ReadBytes((int)base.ReadInt16());
			this.rsaParams.Exponent = base.ReadBytes((int)base.ReadInt16());
			this.signedParams = base.ReadBytes((int)base.ReadInt16());
		}

		private void verifySignature()
		{
			MD5SHA1 md5SHA = new MD5SHA1();
			int count = this.rsaParams.Modulus.Length + this.rsaParams.Exponent.Length + 4;
			TlsStream tlsStream = new TlsStream();
			tlsStream.Write(base.Context.RandomCS);
			tlsStream.Write(base.ToArray(), 0, count);
			md5SHA.ComputeHash(tlsStream.ToArray());
			tlsStream.Reset();
			if (!md5SHA.VerifySignature(base.Context.ServerSettings.CertificateRSA, this.signedParams))
			{
				throw new TlsException(AlertDescription.DecodeError, "Data was not signed with the server certificate.");
			}
		}
	}
}
