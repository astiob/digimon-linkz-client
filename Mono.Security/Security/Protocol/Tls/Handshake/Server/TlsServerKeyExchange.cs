using Mono.Security.Cryptography;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Protocol.Tls.Handshake.Server
{
	internal class TlsServerKeyExchange : HandshakeMessage
	{
		public TlsServerKeyExchange(Context context) : base(context, HandshakeType.ServerKeyExchange)
		{
		}

		public override void Update()
		{
			throw new NotSupportedException();
		}

		protected override void ProcessAsSsl3()
		{
			this.ProcessAsTls1();
		}

		protected override void ProcessAsTls1()
		{
			ServerContext serverContext = (ServerContext)base.Context;
			RSA rsa = (RSA)serverContext.SslStream.PrivateKeyCertSelectionDelegate(new X509Certificate(serverContext.ServerSettings.Certificates[0].RawData), null);
			RSAParameters rsaparameters = rsa.ExportParameters(false);
			base.WriteInt24(rsaparameters.Modulus.Length);
			this.Write(rsaparameters.Modulus, 0, rsaparameters.Modulus.Length);
			base.WriteInt24(rsaparameters.Exponent.Length);
			this.Write(rsaparameters.Exponent, 0, rsaparameters.Exponent.Length);
			byte[] array = this.createSignature(rsa, base.ToArray());
			base.WriteInt24(array.Length);
			base.Write(array);
		}

		private byte[] createSignature(RSA rsa, byte[] buffer)
		{
			MD5SHA1 md5SHA = new MD5SHA1();
			TlsStream tlsStream = new TlsStream();
			tlsStream.Write(base.Context.RandomCS);
			tlsStream.Write(buffer, 0, buffer.Length);
			md5SHA.ComputeHash(tlsStream.ToArray());
			tlsStream.Reset();
			return md5SHA.CreateSignature(rsa);
		}
	}
}
