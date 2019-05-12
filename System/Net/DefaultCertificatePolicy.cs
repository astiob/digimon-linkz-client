using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	internal class DefaultCertificatePolicy : ICertificatePolicy
	{
		public bool CheckValidationResult(ServicePoint point, X509Certificate certificate, WebRequest request, int certificateProblem)
		{
			return ServicePointManager.ServerCertificateValidationCallback != null || certificateProblem == -2146762495 || certificateProblem == 0;
		}
	}
}
