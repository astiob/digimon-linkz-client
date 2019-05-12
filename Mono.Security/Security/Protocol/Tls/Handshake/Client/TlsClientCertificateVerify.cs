using Mono.Security.Cryptography;
using System;
using System.Security.Cryptography;

namespace Mono.Security.Protocol.Tls.Handshake.Client
{
	internal class TlsClientCertificateVerify : HandshakeMessage
	{
		public TlsClientCertificateVerify(Context context) : base(context, HandshakeType.CertificateVerify)
		{
		}

		public override void Update()
		{
			base.Update();
			base.Reset();
		}

		protected override void ProcessAsSsl3()
		{
			AsymmetricAlgorithm asymmetricAlgorithm = null;
			ClientContext clientContext = (ClientContext)base.Context;
			asymmetricAlgorithm = clientContext.SslStream.RaisePrivateKeySelection(clientContext.ClientSettings.ClientCertificate, clientContext.ClientSettings.TargetHost);
			if (asymmetricAlgorithm == null)
			{
				throw new TlsException(AlertDescription.UserCancelled, "Client certificate Private Key unavailable.");
			}
			SslHandshakeHash sslHandshakeHash = new SslHandshakeHash(clientContext.MasterSecret);
			sslHandshakeHash.TransformFinalBlock(clientContext.HandshakeMessages.ToArray(), 0, (int)clientContext.HandshakeMessages.Length);
			byte[] array = null;
			if (!(asymmetricAlgorithm is RSACryptoServiceProvider))
			{
				try
				{
					array = sslHandshakeHash.CreateSignature((RSA)asymmetricAlgorithm);
				}
				catch (NotImplementedException)
				{
				}
			}
			if (array == null)
			{
				RSA clientCertRSA = this.getClientCertRSA((RSA)asymmetricAlgorithm);
				array = sslHandshakeHash.CreateSignature(clientCertRSA);
			}
			base.Write((short)array.Length);
			this.Write(array, 0, array.Length);
		}

		protected override void ProcessAsTls1()
		{
			AsymmetricAlgorithm asymmetricAlgorithm = null;
			ClientContext clientContext = (ClientContext)base.Context;
			asymmetricAlgorithm = clientContext.SslStream.RaisePrivateKeySelection(clientContext.ClientSettings.ClientCertificate, clientContext.ClientSettings.TargetHost);
			if (asymmetricAlgorithm == null)
			{
				throw new TlsException(AlertDescription.UserCancelled, "Client certificate Private Key unavailable.");
			}
			MD5SHA1 md5SHA = new MD5SHA1();
			md5SHA.ComputeHash(clientContext.HandshakeMessages.ToArray(), 0, (int)clientContext.HandshakeMessages.Length);
			byte[] array = null;
			if (!(asymmetricAlgorithm is RSACryptoServiceProvider))
			{
				try
				{
					array = md5SHA.CreateSignature((RSA)asymmetricAlgorithm);
				}
				catch (NotImplementedException)
				{
				}
			}
			if (array == null)
			{
				RSA clientCertRSA = this.getClientCertRSA((RSA)asymmetricAlgorithm);
				array = md5SHA.CreateSignature(clientCertRSA);
			}
			base.Write((short)array.Length);
			this.Write(array, 0, array.Length);
		}

		private RSA getClientCertRSA(RSA privKey)
		{
			RSAParameters parameters = default(RSAParameters);
			RSAParameters rsaparameters = privKey.ExportParameters(true);
			ASN1 asn = new ASN1(base.Context.ClientSettings.Certificates[0].GetPublicKey());
			ASN1 asn2 = asn[0];
			if (asn2 == null || asn2.Tag != 2)
			{
				return null;
			}
			ASN1 asn3 = asn[1];
			if (asn3.Tag != 2)
			{
				return null;
			}
			parameters.Modulus = this.getUnsignedBigInteger(asn2.Value);
			parameters.Exponent = asn3.Value;
			parameters.D = rsaparameters.D;
			parameters.DP = rsaparameters.DP;
			parameters.DQ = rsaparameters.DQ;
			parameters.InverseQ = rsaparameters.InverseQ;
			parameters.P = rsaparameters.P;
			parameters.Q = rsaparameters.Q;
			int keySize = parameters.Modulus.Length << 3;
			RSAManaged rsamanaged = new RSAManaged(keySize);
			rsamanaged.ImportParameters(parameters);
			return rsamanaged;
		}

		private byte[] getUnsignedBigInteger(byte[] integer)
		{
			if (integer[0] == 0)
			{
				int num = integer.Length - 1;
				byte[] array = new byte[num];
				Buffer.BlockCopy(integer, 1, array, 0, num);
				return array;
			}
			return integer;
		}
	}
}
