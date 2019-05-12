using System;
using System.Collections;
using System.IO;

namespace Mono.Security.X509
{
	public sealed class X509StoreManager
	{
		private static X509Stores _userStore;

		private static X509Stores _machineStore;

		private X509StoreManager()
		{
		}

		public static X509Stores CurrentUser
		{
			get
			{
				if (X509StoreManager._userStore == null)
				{
					string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mono");
					text = Path.Combine(text, "certs");
					X509StoreManager._userStore = new X509Stores(text);
				}
				return X509StoreManager._userStore;
			}
		}

		public static X509Stores LocalMachine
		{
			get
			{
				if (X509StoreManager._machineStore == null)
				{
					string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ".mono");
					text = Path.Combine(text, "certs");
					X509StoreManager._machineStore = new X509Stores(text);
				}
				return X509StoreManager._machineStore;
			}
		}

		public static X509CertificateCollection IntermediateCACertificates
		{
			get
			{
				X509CertificateCollection x509CertificateCollection = new X509CertificateCollection();
				x509CertificateCollection.AddRange(X509StoreManager.CurrentUser.IntermediateCA.Certificates);
				x509CertificateCollection.AddRange(X509StoreManager.LocalMachine.IntermediateCA.Certificates);
				return x509CertificateCollection;
			}
		}

		public static ArrayList IntermediateCACrls
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				arrayList.AddRange(X509StoreManager.CurrentUser.IntermediateCA.Crls);
				arrayList.AddRange(X509StoreManager.LocalMachine.IntermediateCA.Crls);
				return arrayList;
			}
		}

		public static X509CertificateCollection TrustedRootCertificates
		{
			get
			{
				X509CertificateCollection x509CertificateCollection = new X509CertificateCollection();
				x509CertificateCollection.AddRange(X509StoreManager.CurrentUser.TrustedRoot.Certificates);
				x509CertificateCollection.AddRange(X509StoreManager.LocalMachine.TrustedRoot.Certificates);
				return x509CertificateCollection;
			}
		}

		public static ArrayList TrustedRootCACrls
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				arrayList.AddRange(X509StoreManager.CurrentUser.TrustedRoot.Crls);
				arrayList.AddRange(X509StoreManager.LocalMachine.TrustedRoot.Crls);
				return arrayList;
			}
		}

		public static X509CertificateCollection UntrustedCertificates
		{
			get
			{
				X509CertificateCollection x509CertificateCollection = new X509CertificateCollection();
				x509CertificateCollection.AddRange(X509StoreManager.CurrentUser.Untrusted.Certificates);
				x509CertificateCollection.AddRange(X509StoreManager.LocalMachine.Untrusted.Certificates);
				return x509CertificateCollection;
			}
		}
	}
}
