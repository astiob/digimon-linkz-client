using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace System.Security.Cryptography
{
	/// <summary>Contains parameters that are passed to the cryptographic service provider (CSP) that performs cryptographic computations. This class cannot be inherited.</summary>
	[ComVisible(true)]
	public sealed class CspParameters
	{
		private CspProviderFlags _Flags;

		/// <summary>Represents the key container name for <see cref="T:System.Security.Cryptography.CspParameters" />.</summary>
		public string KeyContainerName;

		/// <summary>Specifies whether an asymmetric key is created as a signature key or an exchange key.</summary>
		public int KeyNumber;

		/// <summary>Represents the provider name for <see cref="T:System.Security.Cryptography.CspParameters" />.</summary>
		public string ProviderName;

		/// <summary>Represents the provider type code for <see cref="T:System.Security.Cryptography.CspParameters" />.</summary>
		public int ProviderType;

		private SecureString _password;

		private IntPtr _windowHandle;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.CspParameters" /> class.</summary>
		public CspParameters() : this(1)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.CspParameters" /> class with the specified provider type code.</summary>
		/// <param name="dwTypeIn">A provider type code that specifies the kind of provider to create. </param>
		public CspParameters(int dwTypeIn) : this(dwTypeIn, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.CspParameters" /> class with the specified provider type code and name.</summary>
		/// <param name="dwTypeIn">A provider type code that specifies the kind of provider to create.</param>
		/// <param name="strProviderNameIn">A provider name. </param>
		public CspParameters(int dwTypeIn, string strProviderNameIn) : this(dwTypeIn, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.CspParameters" /> class with the specified provider type code and name, and the specified container name.</summary>
		/// <param name="dwTypeIn">The provider type code that specifies the kind of provider to create.</param>
		/// <param name="strProviderNameIn">A provider name. </param>
		/// <param name="strContainerNameIn">A container name. </param>
		public CspParameters(int dwTypeIn, string strProviderNameIn, string strContainerNameIn)
		{
			this.ProviderType = dwTypeIn;
			this.ProviderName = strProviderNameIn;
			this.KeyContainerName = strContainerNameIn;
			this.KeyNumber = -1;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.CspParameters" /> class using a provider type, a provider name, a container name, access information, and a handle to an unmanaged smart card password dialog. </summary>
		/// <param name="providerType">The provider type code that specifies the kind of provider to create.</param>
		/// <param name="providerName">A provider name. </param>
		/// <param name="keyContainerName">A container name. </param>
		/// <param name="cryptoKeySecurity">A <see cref="T:System.Security.AccessControl.CryptoKeySecurity" /> object that represents access rights and audit rules for the container.</param>
		/// <param name="parentWindowHandle">A handle to the parent window for a smart card password dialog.</param>
		public CspParameters(int providerType, string providerName, string keyContainerName, CryptoKeySecurity cryptoKeySecurity, IntPtr parentWindowHandle) : this(providerType, providerName, keyContainerName)
		{
			if (cryptoKeySecurity != null)
			{
				this.CryptoKeySecurity = cryptoKeySecurity;
			}
			this._windowHandle = parentWindowHandle;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.CspParameters" /> class using a provider type, a provider name, a container name, access information, and a password associated with a smart card key.</summary>
		/// <param name="providerType">The provider type code that specifies the kind of provider to create.</param>
		/// <param name="providerName">A provider name. </param>
		/// <param name="keyContainerName">A container name. </param>
		/// <param name="cryptoKeySecurity">A <see cref="T:System.Security.AccessControl.CryptoKeySecurity" /> object that represents access rights and audit rules for a container. </param>
		/// <param name="keyPassword">A password associated with a smart card key.</param>
		public CspParameters(int providerType, string providerName, string keyContainerName, CryptoKeySecurity cryptoKeySecurity, SecureString keyPassword) : this(providerType, providerName, keyContainerName)
		{
			if (cryptoKeySecurity != null)
			{
				this.CryptoKeySecurity = cryptoKeySecurity;
			}
			this._password = keyPassword;
		}

		/// <summary>Represents the flags for <see cref="T:System.Security.Cryptography.CspParameters" /> that modify the behavior of the cryptographic service provider (CSP).</summary>
		/// <returns>An enumeration value, or a bitwise combination of enumeration values.</returns>
		public CspProviderFlags Flags
		{
			get
			{
				return this._Flags;
			}
			set
			{
				this._Flags = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Security.AccessControl.CryptoKeySecurity" /> object that represents access rights and audit rules for a container. </summary>
		/// <returns>A <see cref="T:System.Security.AccessControl.CryptoKeySecurity" /> object that represents access rights and audit rules for a container.</returns>
		[MonoTODO("access control isn't implemented")]
		public CryptoKeySecurity CryptoKeySecurity
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets or sets a password associated with a smart card key. </summary>
		/// <returns>A password associated with a smart card key.</returns>
		public SecureString KeyPassword
		{
			get
			{
				return this._password;
			}
			set
			{
				this._password = value;
			}
		}

		/// <summary>Gets or sets a handle to the unmanaged parent window for a smart card password dialog.</summary>
		/// <returns>A handle to the parent window for a smart card password dialog.</returns>
		public IntPtr ParentWindowHandle
		{
			get
			{
				return this._windowHandle;
			}
			set
			{
				this._windowHandle = value;
			}
		}
	}
}
