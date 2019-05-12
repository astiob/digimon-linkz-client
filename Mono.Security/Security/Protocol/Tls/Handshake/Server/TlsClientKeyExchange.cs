using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Protocol.Tls.Handshake.Server
{
	internal class TlsClientKeyExchange : HandshakeMessage
	{
		public TlsClientKeyExchange(Context context, byte[] buffer) : base(context, HandshakeType.ClientKeyExchange, buffer)
		{
		}

		protected override void ProcessAsSsl3()
		{
			ServerContext serverContext = (ServerContext)base.Context;
			AsymmetricAlgorithm asymmetricAlgorithm = serverContext.SslStream.RaisePrivateKeySelection(new X509Certificate(serverContext.ServerSettings.Certificates[0].RawData), null);
			if (asymmetricAlgorithm == null)
			{
				throw new TlsException(AlertDescription.UserCancelled, "Server certificate Private Key unavailable.");
			}
			byte[] rgbIn = base.ReadBytes((int)this.Length);
			RSAPKCS1KeyExchangeDeformatter rsapkcs1KeyExchangeDeformatter = new RSAPKCS1KeyExchangeDeformatter(asymmetricAlgorithm);
			byte[] preMasterSecret = rsapkcs1KeyExchangeDeformatter.DecryptKeyExchange(rgbIn);
			base.Context.Negotiating.Cipher.ComputeMasterSecret(preMasterSecret);
			base.Context.Negotiating.Cipher.ComputeKeys();
			base.Context.Negotiating.Cipher.InitializeCipher();
		}

		protected override void ProcessAsTls1()
		{
			ServerContext serverContext = (ServerContext)base.Context;
			AsymmetricAlgorithm asymmetricAlgorithm = serverContext.SslStream.RaisePrivateKeySelection(new X509Certificate(serverContext.ServerSettings.Certificates[0].RawData), null);
			if (asymmetricAlgorithm == null)
			{
				throw new TlsException(AlertDescription.UserCancelled, "Server certificate Private Key unavailable.");
			}
			byte[] rgbIn = base.ReadBytes((int)base.ReadInt16());
			RSAPKCS1KeyExchangeDeformatter rsapkcs1KeyExchangeDeformatter = new RSAPKCS1KeyExchangeDeformatter(asymmetricAlgorithm);
			byte[] preMasterSecret = rsapkcs1KeyExchangeDeformatter.DecryptKeyExchange(rgbIn);
			base.Context.Negotiating.Cipher.ComputeMasterSecret(preMasterSecret);
			base.Context.Negotiating.Cipher.ComputeKeys();
			base.Context.Negotiating.Cipher.InitializeCipher();
		}
	}
}
