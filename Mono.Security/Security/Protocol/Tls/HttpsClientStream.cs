using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Protocol.Tls
{
	internal class HttpsClientStream : SslClientStream
	{
		private HttpWebRequest _request;

		private int _status;

		public HttpsClientStream(Stream stream, X509CertificateCollection clientCertificates, HttpWebRequest request, byte[] buffer) : base(stream, request.Address.Host, false, (SecurityProtocolType)ServicePointManager.SecurityProtocol, clientCertificates)
		{
			this._request = request;
			this._status = 0;
			if (buffer != null)
			{
				base.InputBuffer.Write(buffer, 0, buffer.Length);
			}
			base.CheckCertRevocationStatus = ServicePointManager.CheckCertificateRevocationList;
			base.ClientCertSelection += ((X509CertificateCollection clientCerts, X509Certificate serverCertificate, string targetHost, X509CertificateCollection serverRequestedCertificates) => (clientCerts != null && clientCerts.Count != 0) ? clientCerts[0] : null);
			base.PrivateKeySelection += delegate(X509Certificate certificate, string targetHost)
			{
				X509Certificate2 x509Certificate = certificate as X509Certificate2;
				return (x509Certificate != null) ? x509Certificate.PrivateKey : null;
			};
		}

		public bool TrustFailure
		{
			get
			{
				int status = this._status;
				return status == -2146762487 || status == -2146762486;
			}
		}

		internal override bool RaiseServerCertificateValidation(X509Certificate certificate, int[] certificateErrors)
		{
			bool flag = certificateErrors.Length > 0;
			this._status = ((!flag) ? 0 : certificateErrors[0]);
			if (ServicePointManager.CertificatePolicy != null)
			{
				ServicePoint servicePoint = this._request.ServicePoint;
				if (!ServicePointManager.CertificatePolicy.CheckValidationResult(servicePoint, certificate, this._request, this._status))
				{
					return false;
				}
				flag = true;
			}
			if (this.HaveRemoteValidation2Callback)
			{
				return flag;
			}
			RemoteCertificateValidationCallback serverCertificateValidationCallback = ServicePointManager.ServerCertificateValidationCallback;
			if (serverCertificateValidationCallback != null)
			{
				SslPolicyErrors sslPolicyErrors = SslPolicyErrors.None;
				foreach (int num in certificateErrors)
				{
					if (num == -2146762490)
					{
						sslPolicyErrors |= SslPolicyErrors.RemoteCertificateNotAvailable;
					}
					else if (num == -2146762481)
					{
						sslPolicyErrors |= SslPolicyErrors.RemoteCertificateNameMismatch;
					}
					else
					{
						sslPolicyErrors |= SslPolicyErrors.RemoteCertificateChainErrors;
					}
				}
				X509Certificate2 certificate2 = new X509Certificate2(certificate.GetRawCertData());
				X509Chain x509Chain = new X509Chain();
				if (!x509Chain.Build(certificate2))
				{
					sslPolicyErrors |= SslPolicyErrors.RemoteCertificateChainErrors;
				}
				return serverCertificateValidationCallback(this._request, certificate2, x509Chain, sslPolicyErrors);
			}
			return flag;
		}
	}
}
