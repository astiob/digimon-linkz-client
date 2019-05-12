using System;
using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Protocol.Tls
{
	public delegate bool CertificateValidationCallback(X509Certificate certificate, int[] certificateErrors);
}
