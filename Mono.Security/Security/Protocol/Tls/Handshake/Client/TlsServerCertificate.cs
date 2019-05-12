using Mono.Security.X509;
using Mono.Security.X509.Extensions;
using System;
using System.Collections;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Mono.Security.Protocol.Tls.Handshake.Client
{
	internal class TlsServerCertificate : HandshakeMessage
	{
		private Mono.Security.X509.X509CertificateCollection certificates;

		public TlsServerCertificate(Context context, byte[] buffer) : base(context, HandshakeType.Certificate, buffer)
		{
		}

		public override void Update()
		{
			base.Update();
			base.Context.ServerSettings.Certificates = this.certificates;
			base.Context.ServerSettings.UpdateCertificateRSA();
		}

		protected override void ProcessAsSsl3()
		{
			this.ProcessAsTls1();
		}

		protected override void ProcessAsTls1()
		{
			this.certificates = new Mono.Security.X509.X509CertificateCollection();
			int i = 0;
			int num = base.ReadInt24();
			while (i < num)
			{
				int num2 = base.ReadInt24();
				i += 3;
				if (num2 > 0)
				{
					byte[] data = base.ReadBytes(num2);
					Mono.Security.X509.X509Certificate value = new Mono.Security.X509.X509Certificate(data);
					this.certificates.Add(value);
					i += num2;
				}
			}
			this.validateCertificates(this.certificates);
		}

		private bool checkCertificateUsage(Mono.Security.X509.X509Certificate cert)
		{
			ClientContext clientContext = (ClientContext)base.Context;
			if (cert.Version < 3)
			{
				return true;
			}
			KeyUsages usage = KeyUsages.none;
			switch (clientContext.Negotiating.Cipher.ExchangeAlgorithmType)
			{
			case ExchangeAlgorithmType.DiffieHellman:
				usage = KeyUsages.keyAgreement;
				break;
			case ExchangeAlgorithmType.Fortezza:
				return false;
			case ExchangeAlgorithmType.RsaKeyX:
				usage = KeyUsages.keyEncipherment;
				break;
			case ExchangeAlgorithmType.RsaSign:
				usage = KeyUsages.digitalSignature;
				break;
			}
			KeyUsageExtension keyUsageExtension = null;
			ExtendedKeyUsageExtension extendedKeyUsageExtension = null;
			Mono.Security.X509.X509Extension x509Extension = cert.Extensions["2.5.29.15"];
			if (x509Extension != null)
			{
				keyUsageExtension = new KeyUsageExtension(x509Extension);
			}
			x509Extension = cert.Extensions["2.5.29.37"];
			if (x509Extension != null)
			{
				extendedKeyUsageExtension = new ExtendedKeyUsageExtension(x509Extension);
			}
			if (keyUsageExtension != null && extendedKeyUsageExtension != null)
			{
				return keyUsageExtension.Support(usage) && (extendedKeyUsageExtension.KeyPurpose.Contains("1.3.6.1.5.5.7.3.1") || extendedKeyUsageExtension.KeyPurpose.Contains("2.16.840.1.113730.4.1"));
			}
			if (keyUsageExtension != null)
			{
				return keyUsageExtension.Support(usage);
			}
			if (extendedKeyUsageExtension != null)
			{
				return extendedKeyUsageExtension.KeyPurpose.Contains("1.3.6.1.5.5.7.3.1") || extendedKeyUsageExtension.KeyPurpose.Contains("2.16.840.1.113730.4.1");
			}
			x509Extension = cert.Extensions["2.16.840.1.113730.1.1"];
			if (x509Extension != null)
			{
				NetscapeCertTypeExtension netscapeCertTypeExtension = new NetscapeCertTypeExtension(x509Extension);
				return netscapeCertTypeExtension.Support(NetscapeCertTypeExtension.CertTypes.SslServer);
			}
			return true;
		}

		private static void VerifyOSX(Mono.Security.X509.X509CertificateCollection certificates)
		{
		}

		private void validateCertificates(Mono.Security.X509.X509CertificateCollection certificates)
		{
			ClientContext clientContext = (ClientContext)base.Context;
			AlertDescription description = AlertDescription.BadCertificate;
			if (clientContext.SslStream.HaveRemoteValidation2Callback)
			{
				ValidationResult validationResult = clientContext.SslStream.RaiseServerCertificateValidation2(certificates);
				if (validationResult.Trusted)
				{
					return;
				}
				long num = (long)validationResult.ErrorCode;
				long num2 = num;
				if (num2 != (long)((ulong)-2146762487))
				{
					if (num2 != (long)((ulong)-2146762486))
					{
						if (num2 != (long)((ulong)-2146762495))
						{
							description = AlertDescription.CertificateUnknown;
						}
						else
						{
							description = AlertDescription.CertificateExpired;
						}
					}
					else
					{
						description = AlertDescription.UnknownCA;
					}
				}
				else
				{
					description = AlertDescription.UnknownCA;
				}
				string str = string.Format("0x{0:x}", num);
				throw new TlsException(description, "Invalid certificate received from server. Error code: " + str);
			}
			else
			{
				Mono.Security.X509.X509Certificate x509Certificate = certificates[0];
				System.Security.Cryptography.X509Certificates.X509Certificate certificate = new System.Security.Cryptography.X509Certificates.X509Certificate(x509Certificate.RawData);
				ArrayList arrayList = new ArrayList();
				if (!this.checkCertificateUsage(x509Certificate))
				{
					arrayList.Add(-2146762490);
				}
				if (!this.checkServerIdentity(x509Certificate))
				{
					arrayList.Add(-2146762481);
				}
				Mono.Security.X509.X509CertificateCollection x509CertificateCollection = new Mono.Security.X509.X509CertificateCollection(certificates);
				x509CertificateCollection.Remove(x509Certificate);
				Mono.Security.X509.X509Chain x509Chain = new Mono.Security.X509.X509Chain(x509CertificateCollection);
				bool flag = false;
				try
				{
					flag = x509Chain.Build(x509Certificate);
				}
				catch (Exception)
				{
					flag = false;
				}
				if (!flag)
				{
					Mono.Security.X509.X509ChainStatusFlags status = x509Chain.Status;
					if (status != Mono.Security.X509.X509ChainStatusFlags.NotTimeValid)
					{
						if (status != Mono.Security.X509.X509ChainStatusFlags.NotTimeNested)
						{
							if (status != Mono.Security.X509.X509ChainStatusFlags.NotSignatureValid)
							{
								if (status != Mono.Security.X509.X509ChainStatusFlags.UntrustedRoot)
								{
									if (status != Mono.Security.X509.X509ChainStatusFlags.InvalidBasicConstraints)
									{
										if (status != Mono.Security.X509.X509ChainStatusFlags.PartialChain)
										{
											description = AlertDescription.CertificateUnknown;
											arrayList.Add((int)x509Chain.Status);
										}
										else
										{
											description = AlertDescription.UnknownCA;
											arrayList.Add(-2146762486);
										}
									}
									else
									{
										arrayList.Add(-2146869223);
									}
								}
								else
								{
									description = AlertDescription.UnknownCA;
									arrayList.Add(-2146762487);
								}
							}
							else
							{
								arrayList.Add(-2146869232);
							}
						}
						else
						{
							arrayList.Add(-2146762494);
						}
					}
					else
					{
						description = AlertDescription.CertificateExpired;
						arrayList.Add(-2146762495);
					}
				}
				int[] certificateErrors = (int[])arrayList.ToArray(typeof(int));
				if (!clientContext.SslStream.RaiseServerCertificateValidation(certificate, certificateErrors))
				{
					throw new TlsException(description, "Invalid certificate received from server.");
				}
				return;
			}
		}

		private bool checkServerIdentity(Mono.Security.X509.X509Certificate cert)
		{
			ClientContext clientContext = (ClientContext)base.Context;
			string targetHost = clientContext.ClientSettings.TargetHost;
			Mono.Security.X509.X509Extension x509Extension = cert.Extensions["2.5.29.17"];
			if (x509Extension != null)
			{
				SubjectAltNameExtension subjectAltNameExtension = new SubjectAltNameExtension(x509Extension);
				foreach (string pattern in subjectAltNameExtension.DNSNames)
				{
					if (TlsServerCertificate.Match(targetHost, pattern))
					{
						return true;
					}
				}
				foreach (string a in subjectAltNameExtension.IPAddresses)
				{
					if (a == targetHost)
					{
						return true;
					}
				}
			}
			return this.checkDomainName(cert.SubjectName);
		}

		private bool checkDomainName(string subjectName)
		{
			ClientContext clientContext = (ClientContext)base.Context;
			string pattern = string.Empty;
			Regex regex = new Regex("CN\\s*=\\s*([^,]*)");
			MatchCollection matchCollection = regex.Matches(subjectName);
			if (matchCollection.Count == 1 && matchCollection[0].Success)
			{
				pattern = matchCollection[0].Groups[1].Value.ToString();
			}
			return TlsServerCertificate.Match(clientContext.ClientSettings.TargetHost, pattern);
		}

		private static bool Match(string hostname, string pattern)
		{
			int num = pattern.IndexOf('*');
			if (num == -1)
			{
				return string.Compare(hostname, pattern, true, CultureInfo.InvariantCulture) == 0;
			}
			if (num != pattern.Length - 1 && pattern[num + 1] != '.')
			{
				return false;
			}
			int num2 = pattern.IndexOf('*', num + 1);
			if (num2 != -1)
			{
				return false;
			}
			string text = pattern.Substring(num + 1);
			int num3 = hostname.Length - text.Length;
			if (num3 <= 0)
			{
				return false;
			}
			if (string.Compare(hostname, num3, text, 0, text.Length, true, CultureInfo.InvariantCulture) != 0)
			{
				return false;
			}
			if (num == 0)
			{
				int num4 = hostname.IndexOf('.');
				return num4 == -1 || num4 >= hostname.Length - text.Length;
			}
			string text2 = pattern.Substring(0, num);
			return string.Compare(hostname, 0, text2, 0, text2.Length, true, CultureInfo.InvariantCulture) == 0;
		}
	}
}
