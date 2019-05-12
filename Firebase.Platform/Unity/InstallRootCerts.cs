using Firebase.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Firebase.Unity
{
	internal class InstallRootCerts : ICertificateService
	{
		private static readonly object Sync = new object();

		private static Dictionary<IFirebaseAppPlatform, X509CertificateCollection> _installedRoots = new Dictionary<IFirebaseAppPlatform, X509CertificateCollection>();

		private static bool _attemptedWebDownload;

		private static InstallRootCerts _instance = null;

		private bool _needsCertificateWorkaround;

		private const string CoreFoundationLibrary = "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";

		private static readonly string TrustedRoot = "Trust";

		private static readonly string IntermediateCA = "CA";

		private InstallRootCerts()
		{
			var <>__AnonType = FirebaseHandler.RunOnMainThread(() => new
			{
				Platform = Application.platform,
				IsEditor = Application.isEditor,
				InstallationRequired = InstallRootCerts.InstallationRequired
			});
			if (<>__AnonType.InstallationRequired && InstallRootCerts.IsCertBugPresent(<>__AnonType.Platform))
			{
				if (<>__AnonType.IsEditor)
				{
					Services.Logging.LogMessage(PlatformLogLevel.Info, "Using workaround for .NET 4.6 certificate bug.");
					this._needsCertificateWorkaround = true;
				}
				else
				{
					Services.Logging.LogMessage(PlatformLogLevel.Warning, "Detected .NET 4.6 certificate bug, Firebase might not work.");
				}
			}
		}

		private static bool InstallationRequired
		{
			get
			{
				if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
				{
					return false;
				}
				bool flag = false;
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					Type type = assembly.GetType("Firebase.Database.Internal.TubeSock.WebSocket");
					if (type != null)
					{
						flag = true;
					}
				}
				Services.Logging.LogMessage(PlatformLogLevel.Debug, string.Format("Root cert install required: {0}", flag));
				return flag;
			}
		}

		public static InstallRootCerts Instance
		{
			get
			{
				object sync = InstallRootCerts.Sync;
				lock (sync)
				{
					if (InstallRootCerts._instance == null)
					{
						InstallRootCerts._instance = new InstallRootCerts();
					}
				}
				return InstallRootCerts._instance;
			}
		}

		[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
		private static extern void AFunctionThatDoesNotExistInternal();

		private static void AFunctionThatDoesNotExist()
		{
			InstallRootCerts.AFunctionThatDoesNotExistInternal();
		}

		private static bool IsCertBugPresent(RuntimePlatform platform)
		{
			if (platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer)
			{
				Type type = typeof(X509Certificate2).Assembly.GetType("System.Security.Cryptography.X509Certificates.OSX509Certificates");
				if (type != null)
				{
					try
					{
						InstallRootCerts.AFunctionThatDoesNotExist();
					}
					catch (DllNotFoundException)
					{
						return true;
					}
					catch (EntryPointNotFoundException)
					{
						return false;
					}
					catch
					{
					}
					return true;
				}
			}
			return false;
		}

		private static List<byte[]> DecodeBase64Blobs(string base64BlobList, string startLine, string endLine)
		{
			List<byte[]> list = new List<byte[]>();
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			string[] array = Regex.Split(base64BlobList, "\n|\r|\r\n");
			foreach (string text in array)
			{
				if (flag)
				{
					if (text.StartsWith(endLine))
					{
						flag = false;
						list.Add(Convert.FromBase64String(stringBuilder.ToString()));
						stringBuilder = new StringBuilder();
					}
					else
					{
						stringBuilder.Append(text);
					}
				}
				else
				{
					flag = text.StartsWith(startLine);
				}
			}
			return list;
		}

		private static X509CertificateCollection DecodeCertificateCollectionFromString(string certString)
		{
			X509CertificateCollection x509CertificateCollection = new X509CertificateCollection();
			foreach (byte[] rawData in InstallRootCerts.DecodeBase64Blobs(certString, "-----BEGIN CERTIFICATE-----", "-----END CERTIFICATE-----"))
			{
				x509CertificateCollection.Add(new X509Certificate2(rawData));
			}
			return x509CertificateCollection;
		}

		private static X509CertificateCollection DecodeDefaultCollection()
		{
			try
			{
				using (StreamReader streamReader = new StreamReader(typeof(InstallRootCerts).Assembly.GetManifestResourceStream("Firebase.Platform.cacert_pem.txt")))
				{
					return InstallRootCerts.DecodeCertificateCollectionFromString(streamReader.ReadToEnd());
				}
			}
			catch (Exception ex)
			{
				Services.Logging.LogMessage(PlatformLogLevel.Error, ex.ToString());
			}
			return new X509CertificateCollection();
		}

		private static X509CertificateCollection DecodeCollection(IFirebaseAppPlatform app)
		{
			string certPemFile = Services.AppConfig.GetCertPemFile(app);
			if (!string.IsNullOrEmpty(certPemFile))
			{
				TextAsset textAsset = Resources.Load(certPemFile) as TextAsset;
				if (textAsset != null)
				{
					return InstallRootCerts.DecodeCertificateCollectionFromString(textAsset.text);
				}
			}
			return new X509CertificateCollection();
		}

		private static X509CertificateCollection DecodeWebRootCollection(IFirebaseAppPlatform app)
		{
			Uri certUpdateUrl = Services.AppConfig.GetCertUpdateUrl(app);
			if (certUpdateUrl != null)
			{
				FirebaseHttpRequest firebaseHttpRequest = Services.HttpFactory.OpenConnection(certUpdateUrl);
				firebaseHttpRequest.SetRequestMethod("GET");
				if (firebaseHttpRequest.ResponseCode >= 200 && firebaseHttpRequest.ResponseCode < 300)
				{
					Services.Logging.LogMessage(PlatformLogLevel.Debug, string.Format("updated certs from {0} completed with code {1}", certUpdateUrl, firebaseHttpRequest.ResponseCode.ToString()));
					StreamReader streamReader = new StreamReader(firebaseHttpRequest.InputStream);
					return InstallRootCerts.DecodeCertificateCollectionFromString(streamReader.ReadToEnd());
				}
				Services.Logging.LogMessage(PlatformLogLevel.Error, string.Format("error loading updated certs from {0} with code {1}", certUpdateUrl, firebaseHttpRequest.ResponseCode.ToString()));
			}
			else
			{
				Services.Logging.LogMessage(PlatformLogLevel.Warning, "No root cert url to download.");
			}
			return new X509CertificateCollection();
		}

		private static void InstallDefaultCRLs(string resource_name, string directory)
		{
			Directory.CreateDirectory(directory);
			int num = 0;
			Services.Logging.LogMessage(PlatformLogLevel.Debug, "Installing CRLs in " + directory);
			try
			{
				Stream manifestResourceStream = typeof(InstallRootCerts).Assembly.GetManifestResourceStream(resource_name);
				using (StreamReader streamReader = new StreamReader(manifestResourceStream))
				{
					foreach (byte[] buffer in InstallRootCerts.DecodeBase64Blobs(streamReader.ReadToEnd(), "-----BEGIN X509 CRL-----", "-----END X509 CRL-----"))
					{
						string str;
						using (MD5 md = MD5.Create())
						{
							str = BitConverter.ToString(md.ComputeHash(buffer)).Replace("-", string.Empty);
						}
						string path = Path.Combine(directory, str + ".crl");
						if (!File.Exists(path))
						{
							using (BinaryWriter binaryWriter = new BinaryWriter(new FileStream(path, FileMode.CreateNew)))
							{
								binaryWriter.Write(buffer);
							}
							num++;
						}
					}
				}
				Services.Logging.LogMessage(PlatformLogLevel.Debug, string.Format("Installed {0} CRLs", num));
			}
			catch (Exception ex)
			{
				Services.Logging.LogMessage(PlatformLogLevel.Error, string.Format("Error installing CRLs: {0}", ex.ToString()));
			}
		}

		private static void PrintCert(PlatformLogLevel logLevel, X509Certificate2 cert, X509ChainStatus[] chainElementStatus, string chainElementInformation)
		{
			if (cert == null)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("Issuer name: {0}\n", cert.Issuer ?? string.Empty));
			stringBuilder.Append(string.Format("Subject: {0}\n", cert.Subject ?? string.Empty));
			stringBuilder.Append(string.Format("Serial: {0}\n", cert.SerialNumber ?? string.Empty));
			stringBuilder.Append(string.Format("Certificate valid until: {0}\n", cert.NotAfter));
			stringBuilder.Append(string.Format("Error status length: {0}\n", (chainElementStatus == null) ? 0 : chainElementStatus.Length));
			stringBuilder.Append(string.Format("Chain Information: {0}\n", chainElementInformation ?? string.Empty));
			X509KeyUsageExtension x509KeyUsageExtension = (cert.Extensions == null) ? null : (cert.Extensions["2.5.29.15"] as X509KeyUsageExtension);
			if (x509KeyUsageExtension != null)
			{
				stringBuilder.Append(string.Format("Key Usage Extension flags={0:X}\n", x509KeyUsageExtension.KeyUsages));
			}
			if (chainElementStatus != null)
			{
				foreach (X509ChainStatus x509ChainStatus in chainElementStatus)
				{
					stringBuilder.Append(string.Format("Error status={0}, info={1}", x509ChainStatus.Status, x509ChainStatus.StatusInformation ?? string.Empty));
				}
			}
			Services.Logging.LogMessage(logLevel, stringBuilder.ToString());
		}

		private void HackRefreshMonoRootStore()
		{
			try
			{
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					if (assembly.GetName().Name.Equals("Mono.Security"))
					{
						Type type = assembly.GetType("Mono.Security.X509.X509StoreManager");
						if (type != null)
						{
							FieldInfo field = type.GetField("_userPath", BindingFlags.Static | BindingFlags.NonPublic);
							if (field != null)
							{
								field.SetValue(null, null);
							}
							FieldInfo field2 = type.GetField("_userStore", BindingFlags.Static | BindingFlags.NonPublic);
							if (field2 != null)
							{
								field2.SetValue(null, null);
								return;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Services.Logging.LogMessage(PlatformLogLevel.Error, "Error resetting cert store: " + ex.ToString());
			}
			Services.Logging.LogMessage(PlatformLogLevel.Warning, "Could not refresh the cert store. You may have to restart the app to connect to Firebase.");
		}

		public void UpdateRootCertificates(IFirebaseAppPlatform app)
		{
			if (!InstallRootCerts.InstallationRequired)
			{
				return;
			}
			object sync = InstallRootCerts.Sync;
			lock (sync)
			{
				if (!InstallRootCerts._attemptedWebDownload)
				{
					InstallRootCerts._attemptedWebDownload = true;
					X509CertificateCollection x509CertificateCollection = null;
					try
					{
						x509CertificateCollection = InstallRootCerts.DecodeWebRootCollection(app);
					}
					catch (Exception ex)
					{
						Services.Logging.LogMessage(PlatformLogLevel.Error, ex.ToString());
					}
					if (x509CertificateCollection != null && x509CertificateCollection.Count != 0)
					{
						X509Store x509Store = new X509Store(InstallRootCerts.TrustedRoot);
						x509Store.Open(OpenFlags.ReadWrite);
						X509CertificateCollection certificates = x509Store.Certificates;
						X509CertificateCollection.X509CertificateEnumerator enumerator = x509CertificateCollection.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								X509Certificate x509Certificate = enumerator.Current;
								if (!certificates.Contains(x509Certificate))
								{
									try
									{
										x509Store.Add((X509Certificate2)x509Certificate);
									}
									catch (Exception ex2)
									{
										Services.Logging.LogMessage(PlatformLogLevel.Error, ex2.ToString());
									}
								}
							}
						}
						finally
						{
							IDisposable disposable;
							if ((disposable = (enumerator as IDisposable)) != null)
							{
								disposable.Dispose();
							}
						}
						x509Store.Close();
						this.HackRefreshMonoRootStore();
					}
				}
			}
		}

		public X509CertificateCollection Install(IFirebaseAppPlatform app)
		{
			if (!InstallRootCerts.InstallationRequired)
			{
				return null;
			}
			object sync = InstallRootCerts.Sync;
			X509CertificateCollection result;
			lock (sync)
			{
				X509CertificateCollection x509CertificateCollection;
				if (InstallRootCerts._installedRoots.TryGetValue(app, out x509CertificateCollection))
				{
					result = x509CertificateCollection;
				}
				else
				{
					x509CertificateCollection = new X509CertificateCollection();
					string text = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mono"), "certs");
					bool flag = false;
					try
					{
						flag = (Directory.Exists(text) || Directory.CreateDirectory(text) != null);
					}
					catch (Exception)
					{
					}
					if (!flag)
					{
						string writeablePath = Services.AppConfig.GetWriteablePath(app);
						if (!string.IsNullOrEmpty(writeablePath))
						{
							Services.Logging.LogMessage(PlatformLogLevel.Debug, string.Format("Saving root certs in {0} ({1} is not writable)", writeablePath, text));
							Environment.SetEnvironmentVariable("XDG_CONFIG_HOME", writeablePath);
							text = writeablePath;
							this.HackRefreshMonoRootStore();
						}
					}
					X509CertificateCollection value = InstallRootCerts.DecodeDefaultCollection();
					X509CertificateCollection x509CertificateCollection2 = InstallRootCerts.DecodeCollection(app);
					if (string.Equals(app.Name, FirebaseHandler.AppUtils.GetDefaultInstanceName()))
					{
						x509CertificateCollection2.AddRange(value);
						x509CertificateCollection = x509CertificateCollection2;
					}
					else
					{
						x509CertificateCollection.AddRange(value);
					}
					InstallRootCerts._installedRoots[app] = x509CertificateCollection2;
					if (x509CertificateCollection.Count == 0)
					{
						result = x509CertificateCollection;
					}
					else
					{
						InstallRootCerts.InstallDefaultCRLs("Firebase.Platform.cacrl_pem.txt", Path.Combine(text, InstallRootCerts.TrustedRoot));
						InstallRootCerts.InstallDefaultCRLs("Firebase.Platform.caintermediatecrl_pem.txt", Path.Combine(text, InstallRootCerts.IntermediateCA));
						Services.Logging.LogMessage(PlatformLogLevel.Debug, string.Format("Installing {0} certs", x509CertificateCollection2.Count));
						X509Store x509Store = new X509Store(InstallRootCerts.TrustedRoot);
						x509Store.Open(OpenFlags.ReadWrite);
						X509CertificateCollection certificates = x509Store.Certificates;
						X509CertificateCollection.X509CertificateEnumerator enumerator = x509CertificateCollection.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								X509Certificate x509Certificate = enumerator.Current;
								if (!certificates.Contains(x509Certificate))
								{
									try
									{
										x509Store.Add((X509Certificate2)x509Certificate);
									}
									catch (Exception ex)
									{
										Services.Logging.LogMessage(PlatformLogLevel.Error, ex.ToString());
									}
								}
							}
						}
						finally
						{
							IDisposable disposable;
							if ((disposable = (enumerator as IDisposable)) != null)
							{
								disposable.Dispose();
							}
						}
						x509Store.Close();
						result = x509CertificateCollection;
					}
				}
			}
			return result;
		}

		private bool MacOSXWorkaroundRemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors)
			{
				return chain.Build(new X509Certificate2(certificate));
			}
			return sslPolicyErrors == SslPolicyErrors.None;
		}

		public RemoteCertificateValidationCallback GetRemoteCertificateValidationCallback()
		{
			if (InstallRootCerts.InstallationRequired && this._needsCertificateWorkaround)
			{
				return new RemoteCertificateValidationCallback(this.MacOSXWorkaroundRemoteCertificateValidationCallback);
			}
			return null;
		}
	}
}
