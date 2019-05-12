using Mono.Xml;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Mono.Security.Cryptography
{
	internal class KeyPairPersistence
	{
		private static bool _userPathExists = false;

		private static string _userPath;

		private static bool _machinePathExists = false;

		private static string _machinePath;

		private CspParameters _params;

		private string _keyvalue;

		private string _filename;

		private string _container;

		private static object lockobj = new object();

		public KeyPairPersistence(CspParameters parameters) : this(parameters, null)
		{
		}

		public KeyPairPersistence(CspParameters parameters, string keyPair)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			this._params = this.Copy(parameters);
			this._keyvalue = keyPair;
		}

		public string Filename
		{
			get
			{
				if (this._filename == null)
				{
					this._filename = string.Format(CultureInfo.InvariantCulture, "[{0}][{1}][{2}].xml", new object[]
					{
						this._params.ProviderType,
						this.ContainerName,
						this._params.KeyNumber
					});
					if (this.UseMachineKeyStore)
					{
						this._filename = Path.Combine(KeyPairPersistence.MachinePath, this._filename);
					}
					else
					{
						this._filename = Path.Combine(KeyPairPersistence.UserPath, this._filename);
					}
				}
				return this._filename;
			}
		}

		public string KeyValue
		{
			get
			{
				return this._keyvalue;
			}
			set
			{
				if (this.CanChange)
				{
					this._keyvalue = value;
				}
			}
		}

		public CspParameters Parameters
		{
			get
			{
				return this.Copy(this._params);
			}
		}

		public bool Load()
		{
			if (Environment.SocketSecurityEnabled)
			{
				return false;
			}
			bool flag = File.Exists(this.Filename);
			if (flag)
			{
				using (StreamReader streamReader = File.OpenText(this.Filename))
				{
					this.FromXml(streamReader.ReadToEnd());
				}
			}
			return flag;
		}

		public void Save()
		{
			if (Environment.SocketSecurityEnabled)
			{
				return;
			}
			using (FileStream fileStream = File.Open(this.Filename, FileMode.Create))
			{
				StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
				streamWriter.Write(this.ToXml());
				streamWriter.Close();
			}
			if (this.UseMachineKeyStore)
			{
				KeyPairPersistence.ProtectMachine(this.Filename);
			}
			else
			{
				KeyPairPersistence.ProtectUser(this.Filename);
			}
		}

		public void Remove()
		{
			if (Environment.SocketSecurityEnabled)
			{
				return;
			}
			File.Delete(this.Filename);
		}

		private static string UserPath
		{
			get
			{
				object obj = KeyPairPersistence.lockobj;
				lock (obj)
				{
					if (KeyPairPersistence._userPath == null || !KeyPairPersistence._userPathExists)
					{
						KeyPairPersistence._userPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mono");
						KeyPairPersistence._userPath = Path.Combine(KeyPairPersistence._userPath, "keypairs");
						KeyPairPersistence._userPathExists = Directory.Exists(KeyPairPersistence._userPath);
						if (!KeyPairPersistence._userPathExists)
						{
							try
							{
								Directory.CreateDirectory(KeyPairPersistence._userPath);
								KeyPairPersistence.ProtectUser(KeyPairPersistence._userPath);
								KeyPairPersistence._userPathExists = true;
							}
							catch (Exception inner)
							{
								string text = Locale.GetText("Could not create user key store '{0}'.");
								throw new CryptographicException(string.Format(text, KeyPairPersistence._userPath), inner);
							}
						}
					}
				}
				if (!KeyPairPersistence.IsUserProtected(KeyPairPersistence._userPath))
				{
					string text2 = Locale.GetText("Improperly protected user's key pairs in '{0}'.");
					throw new CryptographicException(string.Format(text2, KeyPairPersistence._userPath));
				}
				return KeyPairPersistence._userPath;
			}
		}

		private static string MachinePath
		{
			get
			{
				object obj = KeyPairPersistence.lockobj;
				lock (obj)
				{
					if (KeyPairPersistence._machinePath == null || !KeyPairPersistence._machinePathExists)
					{
						KeyPairPersistence._machinePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ".mono");
						KeyPairPersistence._machinePath = Path.Combine(KeyPairPersistence._machinePath, "keypairs");
						KeyPairPersistence._machinePathExists = Directory.Exists(KeyPairPersistence._machinePath);
						if (!KeyPairPersistence._machinePathExists)
						{
							try
							{
								Directory.CreateDirectory(KeyPairPersistence._machinePath);
								KeyPairPersistence.ProtectMachine(KeyPairPersistence._machinePath);
								KeyPairPersistence._machinePathExists = true;
							}
							catch (Exception inner)
							{
								string text = Locale.GetText("Could not create machine key store '{0}'.");
								throw new CryptographicException(string.Format(text, KeyPairPersistence._machinePath), inner);
							}
						}
					}
				}
				if (!KeyPairPersistence.IsMachineProtected(KeyPairPersistence._machinePath))
				{
					string text2 = Locale.GetText("Improperly protected machine's key pairs in '{0}'.");
					throw new CryptographicException(string.Format(text2, KeyPairPersistence._machinePath));
				}
				return KeyPairPersistence._machinePath;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _CanSecure(string root);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _ProtectUser(string path);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _ProtectMachine(string path);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _IsUserProtected(string path);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _IsMachineProtected(string path);

		private static bool CanSecure(string path)
		{
			int platform = (int)Environment.OSVersion.Platform;
			return platform == 4 || platform == 128 || platform == 6 || KeyPairPersistence._CanSecure(Path.GetPathRoot(path));
		}

		private static bool ProtectUser(string path)
		{
			return !KeyPairPersistence.CanSecure(path) || KeyPairPersistence._ProtectUser(path);
		}

		private static bool ProtectMachine(string path)
		{
			return !KeyPairPersistence.CanSecure(path) || KeyPairPersistence._ProtectMachine(path);
		}

		private static bool IsUserProtected(string path)
		{
			return !KeyPairPersistence.CanSecure(path) || KeyPairPersistence._IsUserProtected(path);
		}

		private static bool IsMachineProtected(string path)
		{
			return !KeyPairPersistence.CanSecure(path) || KeyPairPersistence._IsMachineProtected(path);
		}

		private bool CanChange
		{
			get
			{
				return this._keyvalue == null;
			}
		}

		private bool UseDefaultKeyContainer
		{
			get
			{
				return (this._params.Flags & CspProviderFlags.UseDefaultKeyContainer) == CspProviderFlags.UseDefaultKeyContainer;
			}
		}

		private bool UseMachineKeyStore
		{
			get
			{
				return (this._params.Flags & CspProviderFlags.UseMachineKeyStore) == CspProviderFlags.UseMachineKeyStore;
			}
		}

		private string ContainerName
		{
			get
			{
				if (this._container == null)
				{
					if (this.UseDefaultKeyContainer)
					{
						this._container = "default";
					}
					else if (this._params.KeyContainerName == null || this._params.KeyContainerName.Length == 0)
					{
						this._container = Guid.NewGuid().ToString();
					}
					else
					{
						byte[] bytes = Encoding.UTF8.GetBytes(this._params.KeyContainerName);
						MD5 md = MD5.Create();
						byte[] b = md.ComputeHash(bytes);
						Guid guid = new Guid(b);
						this._container = guid.ToString();
					}
				}
				return this._container;
			}
		}

		private CspParameters Copy(CspParameters p)
		{
			return new CspParameters(p.ProviderType, p.ProviderName, p.KeyContainerName)
			{
				KeyNumber = p.KeyNumber,
				Flags = p.Flags
			};
		}

		private void FromXml(string xml)
		{
			SecurityParser securityParser = new SecurityParser();
			securityParser.LoadXml(xml);
			SecurityElement securityElement = securityParser.ToXml();
			if (securityElement.Tag == "KeyPair")
			{
				SecurityElement securityElement2 = securityElement.SearchForChildByTag("KeyValue");
				if (securityElement2.Children.Count > 0)
				{
					this._keyvalue = securityElement2.Children[0].ToString();
				}
			}
		}

		private string ToXml()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("<KeyPair>{0}\t<Properties>{0}\t\t<Provider ", Environment.NewLine);
			if (this._params.ProviderName != null && this._params.ProviderName.Length != 0)
			{
				stringBuilder.AppendFormat("Name=\"{0}\" ", this._params.ProviderName);
			}
			stringBuilder.AppendFormat("Type=\"{0}\" />{1}\t\t<Container ", this._params.ProviderType, Environment.NewLine);
			stringBuilder.AppendFormat("Name=\"{0}\" />{1}\t</Properties>{1}\t<KeyValue", this.ContainerName, Environment.NewLine);
			if (this._params.KeyNumber != -1)
			{
				stringBuilder.AppendFormat(" Id=\"{0}\" ", this._params.KeyNumber);
			}
			stringBuilder.AppendFormat(">{1}\t\t{0}{1}\t</KeyValue>{1}</KeyPair>{1}", this.KeyValue, Environment.NewLine);
			return stringBuilder.ToString();
		}
	}
}
