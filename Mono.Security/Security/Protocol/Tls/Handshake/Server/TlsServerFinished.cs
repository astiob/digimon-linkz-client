using Mono.Security.Cryptography;
using System;
using System.Security.Cryptography;

namespace Mono.Security.Protocol.Tls.Handshake.Server
{
	internal class TlsServerFinished : HandshakeMessage
	{
		private static byte[] Ssl3Marker = new byte[]
		{
			83,
			82,
			86,
			82
		};

		public TlsServerFinished(Context context) : base(context, HandshakeType.Finished)
		{
		}

		protected override void ProcessAsSsl3()
		{
			HashAlgorithm hashAlgorithm = new SslHandshakeHash(base.Context.MasterSecret);
			byte[] array = base.Context.HandshakeMessages.ToArray();
			hashAlgorithm.TransformBlock(array, 0, array.Length, array, 0);
			hashAlgorithm.TransformBlock(TlsServerFinished.Ssl3Marker, 0, TlsServerFinished.Ssl3Marker.Length, TlsServerFinished.Ssl3Marker, 0);
			hashAlgorithm.TransformFinalBlock(CipherSuite.EmptyArray, 0, 0);
			base.Write(hashAlgorithm.Hash);
		}

		protected override void ProcessAsTls1()
		{
			HashAlgorithm hashAlgorithm = new MD5SHA1();
			byte[] array = base.Context.HandshakeMessages.ToArray();
			byte[] data = hashAlgorithm.ComputeHash(array, 0, array.Length);
			base.Write(base.Context.Current.Cipher.PRF(base.Context.MasterSecret, "server finished", data, 12));
		}
	}
}
