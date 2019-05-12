using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Firebase.Platform
{
	internal class NoopCertificateService : ICertificateService
	{
		private static NoopCertificateService _instance = new NoopCertificateService();

		public static NoopCertificateService Instance
		{
			get
			{
				return NoopCertificateService._instance;
			}
		}

		public X509CertificateCollection Install(IFirebaseAppPlatform app)
		{
			Services.Logging.LogMessage(PlatformLogLevel.Warning, "No certs are being installed because the platform doesn't support it.");
			return null;
		}

		public void UpdateRootCertificates(IFirebaseAppPlatform app)
		{
			Services.Logging.LogMessage(PlatformLogLevel.Warning, "No certs are being updated because the platform doesn't support it.");
		}

		public RemoteCertificateValidationCallback GetRemoteCertificateValidationCallback()
		{
			return null;
		}
	}
}
