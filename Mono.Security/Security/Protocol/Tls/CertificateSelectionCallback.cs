using System;
using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Protocol.Tls
{
	public delegate X509Certificate CertificateSelectionCallback(X509CertificateCollection clientCertificates, X509Certificate serverCertificate, string targetHost, X509CertificateCollection serverRequestedCertificates);
}
