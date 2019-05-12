using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Protocol.Tls
{
	public delegate AsymmetricAlgorithm PrivateKeySelectionCallback(X509Certificate certificate, string targetHost);
}
