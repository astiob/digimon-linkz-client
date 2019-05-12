using Mono.Security.Cryptography;
using System;
using System.Security.Cryptography;

namespace Mono.Security.Protocol.Tls.Handshake.Server
{
	internal class TlsClientFinished : HandshakeMessage
	{
		public TlsClientFinished(Context context, byte[] buffer) : base(context, HandshakeType.Finished, buffer)
		{
		}

		protected override void ProcessAsSsl3()
		{
			HashAlgorithm hashAlgorithm = new SslHandshakeHash(base.Context.MasterSecret);
			TlsStream tlsStream = new TlsStream();
			tlsStream.Write(base.Context.HandshakeMessages.ToArray());
			tlsStream.Write(1129074260);
			hashAlgorithm.TransformFinalBlock(tlsStream.ToArray(), 0, (int)tlsStream.Length);
			tlsStream.Reset();
			byte[] buffer = base.ReadBytes((int)this.Length);
			byte[] hash = hashAlgorithm.Hash;
			if (!HandshakeMessage.Compare(buffer, hash))
			{
				throw new TlsException(AlertDescription.DecryptError, "Decrypt error.");
			}
		}

		protected override void ProcessAsTls1()
		{
			byte[] buffer = base.ReadBytes((int)this.Length);
			HashAlgorithm hashAlgorithm = new MD5SHA1();
			byte[] array = base.Context.HandshakeMessages.ToArray();
			byte[] data = hashAlgorithm.ComputeHash(array, 0, array.Length);
			byte[] buffer2 = base.Context.Current.Cipher.PRF(base.Context.MasterSecret, "client finished", data, 12);
			if (!HandshakeMessage.Compare(buffer, buffer2))
			{
				throw new TlsException(AlertDescription.DecryptError, "Decrypt error.");
			}
		}
	}
}
