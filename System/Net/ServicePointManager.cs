using Mono.Security.Protocol.Tls;
using Mono.Security.X509;
using Mono.Security.X509.Extensions;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace System.Net
{
	/// <summary>Manages the collection of <see cref="T:System.Net.ServicePoint" /> objects.</summary>
	public class ServicePointManager
	{
		/// <summary>The default number of non-persistent connections (4) allowed on a <see cref="T:System.Net.ServicePoint" /> object connected to an HTTP/1.0 or later server. This field is constant but is no longer used in the .NET Framework 2.0.</summary>
		public const int DefaultNonPersistentConnectionLimit = 4;

		/// <summary>The default number of persistent connections (2) allowed on a <see cref="T:System.Net.ServicePoint" /> object connected to an HTTP/1.1 or later server. This field is constant and is used to initialize the <see cref="P:System.Net.ServicePointManager.DefaultConnectionLimit" /> property if the value of the <see cref="P:System.Net.ServicePointManager.DefaultConnectionLimit" /> property has not been set either directly or through configuration.</summary>
		public const int DefaultPersistentConnectionLimit = 2;

		private static System.Collections.Specialized.HybridDictionary servicePoints = new System.Collections.Specialized.HybridDictionary();

		private static ICertificatePolicy policy = new DefaultCertificatePolicy();

		private static int defaultConnectionLimit = 2;

		private static int maxServicePointIdleTime = 900000;

		private static int maxServicePoints = 0;

		private static bool _checkCRL = false;

		private static SecurityProtocolType _securityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

		private static bool expectContinue = true;

		private static bool useNagle;

		private static System.Net.Security.RemoteCertificateValidationCallback server_cert_cb;

		private ServicePointManager()
		{
		}

		/// <summary>Gets or sets policy for server certificates.</summary>
		/// <returns>An object that implements the <see cref="T:System.Net.ICertificatePolicy" /> interface.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[Obsolete("Use ServerCertificateValidationCallback instead", false)]
		public static ICertificatePolicy CertificatePolicy
		{
			get
			{
				return ServicePointManager.policy;
			}
			set
			{
				ServicePointManager.policy = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that indicates whether the certificate is checked against the certificate authority revocation list.</summary>
		/// <returns>true if the certificate revocation list is checked; otherwise, false.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[MonoTODO("CRL checks not implemented")]
		public static bool CheckCertificateRevocationList
		{
			get
			{
				return ServicePointManager._checkCRL;
			}
			set
			{
				ServicePointManager._checkCRL = false;
			}
		}

		/// <summary>Gets or sets the maximum number of concurrent connections allowed by a <see cref="T:System.Net.ServicePoint" /> object.</summary>
		/// <returns>The maximum number of concurrent connections allowed by a <see cref="T:System.Net.ServicePoint" /> object. The default value is 2.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <see cref="P:System.Net.ServicePointManager.DefaultConnectionLimit" /> is less than or equal to 0. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static int DefaultConnectionLimit
		{
			get
			{
				return ServicePointManager.defaultConnectionLimit;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				ServicePointManager.defaultConnectionLimit = value;
			}
		}

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		/// <summary>Gets or sets a value that indicates how long a Domain Name Service (DNS) resolution is considered valid.</summary>
		/// <returns>The time-out value, in milliseconds. A value of -1 indicates an infinite time-out period. The default value is 120,000 milliseconds (two minutes).</returns>
		[MonoTODO]
		public static int DnsRefreshTimeout
		{
			get
			{
				throw ServicePointManager.GetMustImplement();
			}
			set
			{
				throw ServicePointManager.GetMustImplement();
			}
		}

		/// <summary>Gets or sets a value that indicates whether a Domain Name Service (DNS) resolution rotates among the applicable Internet Protocol (IP) addresses.</summary>
		/// <returns>false if a DNS resolution always returns the first IP address for a particular host; otherwise true. The default is false.</returns>
		[MonoTODO]
		public static bool EnableDnsRoundRobin
		{
			get
			{
				throw ServicePointManager.GetMustImplement();
			}
			set
			{
				throw ServicePointManager.GetMustImplement();
			}
		}

		/// <summary>Gets or sets the maximum idle time of a <see cref="T:System.Net.ServicePoint" /> object.</summary>
		/// <returns>The maximum idle time, in milliseconds, of a <see cref="T:System.Net.ServicePoint" /> object. The default value is 100,000 milliseconds (100 seconds).</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <see cref="P:System.Net.ServicePointManager.MaxServicePointIdleTime" /> is less than <see cref="F:System.Threading.Timeout.Infinite" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static int MaxServicePointIdleTime
		{
			get
			{
				return ServicePointManager.maxServicePointIdleTime;
			}
			set
			{
				if (value < -2 || value > 2147483647)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				ServicePointManager.maxServicePointIdleTime = value;
			}
		}

		/// <summary>Gets or sets the maximum number of <see cref="T:System.Net.ServicePoint" /> objects to maintain at any time.</summary>
		/// <returns>The maximum number of <see cref="T:System.Net.ServicePoint" /> objects to maintain. The default value is 0, which means there is no limit to the number of <see cref="T:System.Net.ServicePoint" /> objects.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <see cref="P:System.Net.ServicePointManager.MaxServicePoints" /> is less than 0 or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static int MaxServicePoints
		{
			get
			{
				return ServicePointManager.maxServicePoints;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("value");
				}
				ServicePointManager.maxServicePoints = value;
				ServicePointManager.RecycleServicePoints();
			}
		}

		/// <summary>Gets or sets the security protocol used by the <see cref="T:System.Net.ServicePoint" /> objects managed by the <see cref="T:System.Net.ServicePointManager" /> object.</summary>
		/// <returns>One of the values defined in the <see cref="T:System.Net.SecurityProtocolType" /> enumeration.</returns>
		/// <exception cref="T:System.NotSupportedException">The value specified to set the property is not a valid <see cref="T:System.Net.SecurityProtocolType" /> enumeration value. </exception>
		public static SecurityProtocolType SecurityProtocol
		{
			get
			{
				return ServicePointManager._securityProtocol;
			}
			set
			{
				ServicePointManager._securityProtocol = value;
			}
		}

		/// <summary>Gets or sets the callback to validate a server certificate.</summary>
		/// <returns>A <see cref="T:System.Net.Security.RemoteCertificateValidationCallback" /> The default value is null.</returns>
		public static System.Net.Security.RemoteCertificateValidationCallback ServerCertificateValidationCallback
		{
			get
			{
				return ServicePointManager.server_cert_cb;
			}
			set
			{
				ServicePointManager.server_cert_cb = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that determines whether 100-Continue behavior is used.</summary>
		/// <returns>true to enable 100-Continue behavior. The default value is true.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static bool Expect100Continue
		{
			get
			{
				return ServicePointManager.expectContinue;
			}
			set
			{
				ServicePointManager.expectContinue = value;
			}
		}

		/// <summary>Determines whether the Nagle algorithm is used by the service points managed by this <see cref="T:System.Net.ServicePointManager" /> object.</summary>
		/// <returns>true to use the Nagle algorithm; otherwise, false. The default value is true.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static bool UseNagleAlgorithm
		{
			get
			{
				return ServicePointManager.useNagle;
			}
			set
			{
				ServicePointManager.useNagle = value;
			}
		}

		/// <summary>Finds an existing <see cref="T:System.Net.ServicePoint" /> object or creates a new <see cref="T:System.Net.ServicePoint" /> object to manage communications with the specified <see cref="T:System.Uri" /> object.</summary>
		/// <returns>The <see cref="T:System.Net.ServicePoint" /> object that manages communications for the request.</returns>
		/// <param name="address">The <see cref="T:System.Uri" /> object of the Internet resource to contact. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The maximum number of <see cref="T:System.Net.ServicePoint" /> objects defined in <see cref="P:System.Net.ServicePointManager.MaxServicePoints" /> has been reached. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static ServicePoint FindServicePoint(System.Uri address)
		{
			return ServicePointManager.FindServicePoint(address, GlobalProxySelection.Select);
		}

		/// <summary>Finds an existing <see cref="T:System.Net.ServicePoint" /> object or creates a new <see cref="T:System.Net.ServicePoint" /> object to manage communications with the specified Uniform Resource Identifier (URI).</summary>
		/// <returns>The <see cref="T:System.Net.ServicePoint" /> object that manages communications for the request.</returns>
		/// <param name="uriString">The URI of the Internet resource to be contacted. </param>
		/// <param name="proxy">The proxy data for this request. </param>
		/// <exception cref="T:System.UriFormatException">The URI specified in <paramref name="uriString" /> is invalid. </exception>
		/// <exception cref="T:System.InvalidOperationException">The maximum number of <see cref="T:System.Net.ServicePoint" /> objects defined in <see cref="P:System.Net.ServicePointManager.MaxServicePoints" /> has been reached. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static ServicePoint FindServicePoint(string uriString, IWebProxy proxy)
		{
			return ServicePointManager.FindServicePoint(new System.Uri(uriString), proxy);
		}

		/// <summary>Finds an existing <see cref="T:System.Net.ServicePoint" /> object or creates a new <see cref="T:System.Net.ServicePoint" /> object to manage communications with the specified <see cref="T:System.Uri" /> object.</summary>
		/// <returns>The <see cref="T:System.Net.ServicePoint" /> object that manages communications for the request.</returns>
		/// <param name="address">A <see cref="T:System.Uri" /> object that contains the address of the Internet resource to contact. </param>
		/// <param name="proxy">The proxy data for this request. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The maximum number of <see cref="T:System.Net.ServicePoint" /> objects defined in <see cref="P:System.Net.ServicePointManager.MaxServicePoints" /> has been reached. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static ServicePoint FindServicePoint(System.Uri address, IWebProxy proxy)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			ServicePointManager.RecycleServicePoints();
			bool usesProxy = false;
			bool flag = false;
			if (proxy != null && !proxy.IsBypassed(address))
			{
				usesProxy = true;
				bool flag2 = address.Scheme == "https";
				address = proxy.GetProxy(address);
				if (address.Scheme != "http" && !flag2)
				{
					throw new NotSupportedException("Proxy scheme not supported.");
				}
				if (flag2 && address.Scheme == "http")
				{
					flag = true;
				}
			}
			address = new System.Uri(address.Scheme + "://" + address.Authority);
			ServicePoint servicePoint = null;
			System.Collections.Specialized.HybridDictionary obj = ServicePointManager.servicePoints;
			lock (obj)
			{
				ServicePointManager.SPKey key = new ServicePointManager.SPKey(address, flag);
				servicePoint = (ServicePointManager.servicePoints[key] as ServicePoint);
				if (servicePoint != null)
				{
					return servicePoint;
				}
				if (ServicePointManager.maxServicePoints > 0 && ServicePointManager.servicePoints.Count >= ServicePointManager.maxServicePoints)
				{
					throw new InvalidOperationException("maximum number of service points reached");
				}
				string text = address.ToString();
				int connectionLimit = ServicePointManager.defaultConnectionLimit;
				servicePoint = new ServicePoint(address, connectionLimit, ServicePointManager.maxServicePointIdleTime);
				servicePoint.Expect100Continue = ServicePointManager.expectContinue;
				servicePoint.UseNagleAlgorithm = ServicePointManager.useNagle;
				servicePoint.UsesProxy = usesProxy;
				servicePoint.UseConnect = flag;
				ServicePointManager.servicePoints.Add(key, servicePoint);
			}
			return servicePoint;
		}

		internal static void RecycleServicePoints()
		{
			ArrayList arrayList = new ArrayList();
			System.Collections.Specialized.HybridDictionary obj = ServicePointManager.servicePoints;
			lock (obj)
			{
				IDictionaryEnumerator enumerator = ServicePointManager.servicePoints.GetEnumerator();
				while (enumerator.MoveNext())
				{
					ServicePoint servicePoint = (ServicePoint)enumerator.Value;
					if (servicePoint.AvailableForRecycling)
					{
						arrayList.Add(enumerator.Key);
					}
				}
				for (int i = 0; i < arrayList.Count; i++)
				{
					ServicePointManager.servicePoints.Remove(arrayList[i]);
				}
				if (ServicePointManager.maxServicePoints != 0 && ServicePointManager.servicePoints.Count > ServicePointManager.maxServicePoints)
				{
					SortedList sortedList = new SortedList(ServicePointManager.servicePoints.Count);
					enumerator = ServicePointManager.servicePoints.GetEnumerator();
					while (enumerator.MoveNext())
					{
						ServicePoint servicePoint2 = (ServicePoint)enumerator.Value;
						if (servicePoint2.CurrentConnections == 0)
						{
							while (sortedList.ContainsKey(servicePoint2.IdleSince))
							{
								servicePoint2.IdleSince = servicePoint2.IdleSince.AddMilliseconds(1.0);
							}
							sortedList.Add(servicePoint2.IdleSince, servicePoint2.Address);
						}
					}
					int num = 0;
					while (num < sortedList.Count && ServicePointManager.servicePoints.Count > ServicePointManager.maxServicePoints)
					{
						ServicePointManager.servicePoints.Remove(sortedList.GetByIndex(num));
						num++;
					}
				}
			}
		}

		private class SPKey
		{
			private System.Uri uri;

			private bool use_connect;

			public SPKey(System.Uri uri, bool use_connect)
			{
				this.uri = uri;
				this.use_connect = use_connect;
			}

			public System.Uri Uri
			{
				get
				{
					return this.uri;
				}
			}

			public bool UseConnect
			{
				get
				{
					return this.use_connect;
				}
			}

			public override int GetHashCode()
			{
				return this.uri.GetHashCode() + ((!this.use_connect) ? 0 : 1);
			}

			public override bool Equals(object obj)
			{
				ServicePointManager.SPKey spkey = obj as ServicePointManager.SPKey;
				return obj != null && this.uri.Equals(spkey.uri) && spkey.use_connect == this.use_connect;
			}
		}

		internal class ChainValidationHelper
		{
			private object sender;

			private string host;

			private static bool is_macosx = File.Exists("/System/Library/Frameworks/Security.framework/Security");

			private static System.Security.Cryptography.X509Certificates.X509KeyUsageFlags s_flags = System.Security.Cryptography.X509Certificates.X509KeyUsageFlags.KeyAgreement | System.Security.Cryptography.X509Certificates.X509KeyUsageFlags.KeyEncipherment | System.Security.Cryptography.X509Certificates.X509KeyUsageFlags.DigitalSignature;

			public ChainValidationHelper(object sender)
			{
				this.sender = sender;
			}

			public string Host
			{
				get
				{
					if (this.host == null && this.sender is HttpWebRequest)
					{
						this.host = ((HttpWebRequest)this.sender).Address.Host;
					}
					return this.host;
				}
				set
				{
					this.host = value;
				}
			}

			internal ValidationResult ValidateChain(Mono.Security.X509.X509CertificateCollection certs)
			{
				bool user_denied = false;
				if (certs == null || certs.Count == 0)
				{
					return null;
				}
				ICertificatePolicy certificatePolicy = ServicePointManager.CertificatePolicy;
				System.Net.Security.RemoteCertificateValidationCallback serverCertificateValidationCallback = ServicePointManager.ServerCertificateValidationCallback;
				System.Security.Cryptography.X509Certificates.X509Chain x509Chain = new System.Security.Cryptography.X509Certificates.X509Chain();
				x509Chain.ChainPolicy = new System.Security.Cryptography.X509Certificates.X509ChainPolicy();
				for (int i = 1; i < certs.Count; i++)
				{
					System.Security.Cryptography.X509Certificates.X509Certificate2 certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certs[i].RawData);
					x509Chain.ChainPolicy.ExtraStore.Add(certificate);
				}
				System.Security.Cryptography.X509Certificates.X509Certificate2 x509Certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certs[0].RawData);
				int num = 0;
				System.Net.Security.SslPolicyErrors sslPolicyErrors = System.Net.Security.SslPolicyErrors.None;
				try
				{
					if (!x509Chain.Build(x509Certificate))
					{
						sslPolicyErrors |= ServicePointManager.ChainValidationHelper.GetErrorsFromChain(x509Chain);
					}
				}
				catch (Exception arg)
				{
					Console.Error.WriteLine("ERROR building certificate chain: {0}", arg);
					Console.Error.WriteLine("Please, report this problem to the Mono team");
					sslPolicyErrors |= System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors;
				}
				if (!ServicePointManager.ChainValidationHelper.CheckCertificateUsage(x509Certificate))
				{
					sslPolicyErrors |= System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors;
					num = -2146762490;
				}
				if (!ServicePointManager.ChainValidationHelper.CheckServerIdentity(certs[0], this.Host))
				{
					sslPolicyErrors |= System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch;
					num = -2146762481;
				}
				bool flag = false;
				try
				{
					OSX509Certificates.SecTrustResult secTrustResult = OSX509Certificates.TrustEvaluateSsl(certs);
					flag = (secTrustResult == OSX509Certificates.SecTrustResult.Proceed || secTrustResult == OSX509Certificates.SecTrustResult.Unspecified);
				}
				catch
				{
				}
				if (flag)
				{
					num = 0;
					sslPolicyErrors = System.Net.Security.SslPolicyErrors.None;
				}
				if (certificatePolicy != null && (!(certificatePolicy is DefaultCertificatePolicy) || serverCertificateValidationCallback == null))
				{
					ServicePoint srvPoint = null;
					HttpWebRequest httpWebRequest = this.sender as HttpWebRequest;
					if (httpWebRequest != null)
					{
						srvPoint = httpWebRequest.ServicePoint;
					}
					if (num == 0 && sslPolicyErrors != System.Net.Security.SslPolicyErrors.None)
					{
						num = ServicePointManager.ChainValidationHelper.GetStatusFromChain(x509Chain);
					}
					flag = certificatePolicy.CheckValidationResult(srvPoint, x509Certificate, httpWebRequest, num);
					user_denied = (!flag && !(certificatePolicy is DefaultCertificatePolicy));
				}
				if (serverCertificateValidationCallback != null)
				{
					flag = serverCertificateValidationCallback(this.sender, x509Certificate, x509Chain, sslPolicyErrors);
					user_denied = !flag;
				}
				return new ValidationResult(flag, user_denied, num);
			}

			private static int GetStatusFromChain(System.Security.Cryptography.X509Certificates.X509Chain chain)
			{
				long num = 0L;
				foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus x509ChainStatus in chain.ChainStatus)
				{
					System.Security.Cryptography.X509Certificates.X509ChainStatusFlags status = x509ChainStatus.Status;
					if (status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
					{
						if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NotTimeValid) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762495);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NotTimeNested) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762494);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.Revoked) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762484);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NotSignatureValid) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146869244);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NotValidForUsage) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762480);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762487);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.RevocationStatusUnknown) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146885614);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.Cyclic) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762486);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.InvalidExtension) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762485);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.InvalidPolicyConstraints) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762483);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.InvalidBasicConstraints) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146869223);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.InvalidNameConstraints) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762476);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.HasNotSupportedNameConstraint) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762476);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.HasNotDefinedNameConstraint) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762476);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.HasNotPermittedNameConstraint) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762476);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.HasExcludedNameConstraint) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762476);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.PartialChain) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762486);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.CtlNotTimeValid) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762495);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.CtlNotSignatureValid) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146869244);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.CtlNotValidForUsage) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762480);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.OfflineRevocation) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146885614);
						}
						else if ((status & System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoIssuanceChainPolicy) != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							num = (long)((ulong)-2146762489);
						}
						else
						{
							num = (long)((ulong)-2146762485);
						}
						break;
					}
				}
				return (int)num;
			}

			private static System.Net.Security.SslPolicyErrors GetErrorsFromChain(System.Security.Cryptography.X509Certificates.X509Chain chain)
			{
				System.Net.Security.SslPolicyErrors sslPolicyErrors = System.Net.Security.SslPolicyErrors.None;
				foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus x509ChainStatus in chain.ChainStatus)
				{
					if (x509ChainStatus.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
					{
						sslPolicyErrors |= System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors;
						break;
					}
				}
				return sslPolicyErrors;
			}

			private static bool CheckCertificateUsage(System.Security.Cryptography.X509Certificates.X509Certificate2 cert)
			{
				bool result;
				try
				{
					if (cert.Version < 3)
					{
						result = true;
					}
					else
					{
						System.Security.Cryptography.X509Certificates.X509KeyUsageExtension x509KeyUsageExtension = (System.Security.Cryptography.X509Certificates.X509KeyUsageExtension)cert.Extensions["2.5.29.15"];
						System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension x509EnhancedKeyUsageExtension = (System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension)cert.Extensions["2.5.29.37"];
						if (x509KeyUsageExtension != null && x509EnhancedKeyUsageExtension != null)
						{
							if ((x509KeyUsageExtension.KeyUsages & ServicePointManager.ChainValidationHelper.s_flags) == System.Security.Cryptography.X509Certificates.X509KeyUsageFlags.None)
							{
								result = false;
							}
							else
							{
								result = (x509EnhancedKeyUsageExtension.EnhancedKeyUsages["1.3.6.1.5.5.7.3.1"] != null || x509EnhancedKeyUsageExtension.EnhancedKeyUsages["2.16.840.1.113730.4.1"] != null);
							}
						}
						else if (x509KeyUsageExtension != null)
						{
							result = ((x509KeyUsageExtension.KeyUsages & ServicePointManager.ChainValidationHelper.s_flags) != System.Security.Cryptography.X509Certificates.X509KeyUsageFlags.None);
						}
						else if (x509EnhancedKeyUsageExtension != null)
						{
							result = (x509EnhancedKeyUsageExtension.EnhancedKeyUsages["1.3.6.1.5.5.7.3.1"] != null || x509EnhancedKeyUsageExtension.EnhancedKeyUsages["2.16.840.1.113730.4.1"] != null);
						}
						else
						{
							System.Security.Cryptography.X509Certificates.X509Extension x509Extension = cert.Extensions["2.16.840.1.113730.1.1"];
							if (x509Extension != null)
							{
								string text = x509Extension.NetscapeCertType(false);
								result = (text.IndexOf("SSL Server Authentication") != -1);
							}
							else
							{
								result = true;
							}
						}
					}
				}
				catch (Exception arg)
				{
					Console.Error.WriteLine("ERROR processing certificate: {0}", arg);
					Console.Error.WriteLine("Please, report this problem to the Mono team");
					result = false;
				}
				return result;
			}

			private static bool CheckServerIdentity(Mono.Security.X509.X509Certificate cert, string targetHost)
			{
				bool result;
				try
				{
					Mono.Security.X509.X509Extension x509Extension = cert.Extensions["2.5.29.17"];
					if (x509Extension != null)
					{
						SubjectAltNameExtension subjectAltNameExtension = new SubjectAltNameExtension(x509Extension);
						foreach (string pattern in subjectAltNameExtension.DNSNames)
						{
							if (ServicePointManager.ChainValidationHelper.Match(targetHost, pattern))
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
					result = ServicePointManager.ChainValidationHelper.CheckDomainName(cert.SubjectName, targetHost);
				}
				catch (Exception arg)
				{
					Console.Error.WriteLine("ERROR processing certificate: {0}", arg);
					Console.Error.WriteLine("Please, report this problem to the Mono team");
					result = false;
				}
				return result;
			}

			private static bool CheckDomainName(string subjectName, string targetHost)
			{
				string pattern = string.Empty;
				System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("CN\\s*=\\s*([^,]*)");
				System.Text.RegularExpressions.MatchCollection matchCollection = regex.Matches(subjectName);
				if (matchCollection.Count == 1 && matchCollection[0].Success)
				{
					pattern = matchCollection[0].Groups[1].Value.ToString();
				}
				return ServicePointManager.ChainValidationHelper.Match(targetHost, pattern);
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
}
