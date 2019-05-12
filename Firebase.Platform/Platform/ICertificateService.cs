using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Firebase.Platform
{
	internal interface ICertificateService
	{
		X509CertificateCollection Install(IFirebaseAppPlatform app);

		void UpdateRootCertificates(IFirebaseAppPlatform app);

		RemoteCertificateValidationCallback GetRemoteCertificateValidationCallback();
	}
}
