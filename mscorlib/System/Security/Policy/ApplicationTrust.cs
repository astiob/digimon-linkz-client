using Mono.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace System.Security.Policy
{
	/// <summary>Encapsulates security decisions about an application. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class ApplicationTrust : ISecurityEncodable
	{
		private ApplicationIdentity _appid;

		private PolicyStatement _defaultPolicy;

		private object _xtranfo;

		private bool _trustrun;

		private bool _persist;

		private IList<StrongName> fullTrustAssemblies;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.ApplicationTrust" /> class.</summary>
		public ApplicationTrust()
		{
			this.fullTrustAssemblies = new List<StrongName>(0);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.ApplicationTrust" /> class with an <see cref="T:System.ApplicationIdentity" />. </summary>
		/// <param name="applicationIdentity">An <see cref="T:System.ApplicationIdentity" /> that uniquely identifies an application.</param>
		public ApplicationTrust(ApplicationIdentity applicationIdentity) : this()
		{
			if (applicationIdentity == null)
			{
				throw new ArgumentNullException("applicationIdentity");
			}
			this._appid = applicationIdentity;
		}

		internal ApplicationTrust(PermissionSet defaultGrantSet, IEnumerable<StrongName> fullTrustAssemblies)
		{
			if (defaultGrantSet == null)
			{
				throw new ArgumentNullException("defaultGrantSet");
			}
			this._defaultPolicy = new PolicyStatement(defaultGrantSet);
			if (fullTrustAssemblies == null)
			{
				throw new ArgumentNullException("fullTrustAssemblies");
			}
			this.fullTrustAssemblies = new List<StrongName>();
			foreach (StrongName strongName in fullTrustAssemblies)
			{
				if (strongName == null)
				{
					throw new ArgumentException("fullTrustAssemblies contains an assembly that does not have a StrongName");
				}
				this.fullTrustAssemblies.Add((StrongName)strongName.Copy());
			}
		}

		/// <summary>Gets or sets the application identity for the application trust object.</summary>
		/// <returns>An <see cref="T:System.ApplicationIdentity" /> for the application trust object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <see cref="T:System.ApplicationIdentity" /> cannot be set because it has a value of null.</exception>
		public ApplicationIdentity ApplicationIdentity
		{
			get
			{
				return this._appid;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("ApplicationIdentity");
				}
				this._appid = value;
			}
		}

		/// <summary>Gets or sets the policy statement defining the default grant set.</summary>
		/// <returns>A <see cref="T:System.Security.Policy.PolicyStatement" /> describing the default grants.</returns>
		public PolicyStatement DefaultGrantSet
		{
			get
			{
				if (this._defaultPolicy == null)
				{
					this._defaultPolicy = this.GetDefaultGrantSet();
				}
				return this._defaultPolicy;
			}
			set
			{
				this._defaultPolicy = value;
			}
		}

		/// <summary>Gets or sets extra security information about the application.</summary>
		/// <returns>An object containing additional security information about the application.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public object ExtraInfo
		{
			get
			{
				return this._xtranfo;
			}
			set
			{
				this._xtranfo = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the application has the required permission grants and is trusted to run.</summary>
		/// <returns>true if the application is trusted to run; otherwise, false. The default is false.</returns>
		public bool IsApplicationTrustedToRun
		{
			get
			{
				return this._trustrun;
			}
			set
			{
				this._trustrun = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether application trust information is persisted.</summary>
		/// <returns>true if application trust information is persisted; otherwise, false. The default is false.</returns>
		public bool Persist
		{
			get
			{
				return this._persist;
			}
			set
			{
				this._persist = value;
			}
		}

		/// <summary>Reconstructs an <see cref="T:System.Security.Policy.ApplicationTrust" /> object with a given state from an XML encoding.</summary>
		/// <param name="element">The XML encoding to use to reconstruct the <see cref="T:System.Security.Policy.ApplicationTrust" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="element" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">The XML encoding used for <paramref name="element" /> is invalid.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void FromXml(SecurityElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (element.Tag != "ApplicationTrust")
			{
				throw new ArgumentException("element");
			}
			string text = element.Attribute("FullName");
			if (text != null)
			{
				this._appid = new ApplicationIdentity(text);
			}
			else
			{
				this._appid = null;
			}
			this._defaultPolicy = null;
			SecurityElement securityElement = element.SearchForChildByTag("DefaultGrant");
			if (securityElement != null)
			{
				for (int i = 0; i < securityElement.Children.Count; i++)
				{
					SecurityElement securityElement2 = securityElement.Children[i] as SecurityElement;
					if (securityElement2.Tag == "PolicyStatement")
					{
						this.DefaultGrantSet.FromXml(securityElement2, null);
						break;
					}
				}
			}
			if (!bool.TryParse(element.Attribute("TrustedToRun"), out this._trustrun))
			{
				this._trustrun = false;
			}
			if (!bool.TryParse(element.Attribute("Persist"), out this._persist))
			{
				this._persist = false;
			}
			this._xtranfo = null;
			SecurityElement securityElement3 = element.SearchForChildByTag("ExtraInfo");
			if (securityElement3 != null)
			{
				text = securityElement3.Attribute("Data");
				if (text != null)
				{
					byte[] buffer = CryptoConvert.FromHex(text);
					using (MemoryStream memoryStream = new MemoryStream(buffer))
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						this._xtranfo = binaryFormatter.Deserialize(memoryStream);
					}
				}
			}
		}

		/// <summary>Creates an XML encoding of the <see cref="T:System.Security.Policy.ApplicationTrust" /> object and its current state.</summary>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("ApplicationTrust");
			securityElement.AddAttribute("version", "1");
			if (this._appid != null)
			{
				securityElement.AddAttribute("FullName", this._appid.FullName);
			}
			if (this._trustrun)
			{
				securityElement.AddAttribute("TrustedToRun", "true");
			}
			if (this._persist)
			{
				securityElement.AddAttribute("Persist", "true");
			}
			SecurityElement securityElement2 = new SecurityElement("DefaultGrant");
			securityElement2.AddChild(this.DefaultGrantSet.ToXml());
			securityElement.AddChild(securityElement2);
			if (this._xtranfo != null)
			{
				byte[] input = null;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					binaryFormatter.Serialize(memoryStream, this._xtranfo);
					input = memoryStream.ToArray();
				}
				SecurityElement securityElement3 = new SecurityElement("ExtraInfo");
				securityElement3.AddAttribute("Data", CryptoConvert.ToHex(input));
				securityElement.AddChild(securityElement3);
			}
			return securityElement;
		}

		private PolicyStatement GetDefaultGrantSet()
		{
			PermissionSet permSet = new PermissionSet(PermissionState.None);
			return new PolicyStatement(permSet);
		}
	}
}
