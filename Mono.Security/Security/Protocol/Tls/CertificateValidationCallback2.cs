using Mono.Security.X509;
using System;

namespace Mono.Security.Protocol.Tls
{
	public delegate ValidationResult CertificateValidationCallback2(X509CertificateCollection collection);
}
