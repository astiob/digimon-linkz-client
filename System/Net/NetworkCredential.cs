using System;

namespace System.Net
{
	/// <summary>Provides credentials for password-based authentication schemes such as basic, digest, NTLM, and Kerberos authentication.</summary>
	public class NetworkCredential : ICredentials
	{
		private string userName;

		private string password;

		private string domain;

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.NetworkCredential" /> class.</summary>
		public NetworkCredential()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.NetworkCredential" /> class with the specified user name and password.</summary>
		/// <param name="userName">The user name associated with the credentials. </param>
		/// <param name="password">The password for the user name associated with the credentials. </param>
		public NetworkCredential(string userName, string password)
		{
			this.userName = userName;
			this.password = password;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.NetworkCredential" /> class with the specified user name, password, and domain.</summary>
		/// <param name="userName">The user name associated with the credentials. </param>
		/// <param name="password">The password for the user name associated with the credentials. </param>
		/// <param name="domain">The domain associated with these credentials. </param>
		public NetworkCredential(string userName, string password, string domain)
		{
			this.userName = userName;
			this.password = password;
			this.domain = domain;
		}

		/// <summary>Gets or sets the domain or computer name that verifies the credentials.</summary>
		/// <returns>The name of the domain associated with the credentials.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public string Domain
		{
			get
			{
				return (this.domain != null) ? this.domain : string.Empty;
			}
			set
			{
				this.domain = value;
			}
		}

		/// <summary>Gets or sets the user name associated with the credentials.</summary>
		/// <returns>The user name associated with the credentials.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public string UserName
		{
			get
			{
				return (this.userName != null) ? this.userName : string.Empty;
			}
			set
			{
				this.userName = value;
			}
		}

		/// <summary>Gets or sets the password for the user name associated with the credentials.</summary>
		/// <returns>The password associated with the credentials. If this <see cref="T:System.Net.NetworkCredential" /> instance was constructed with a null password, then the <see cref="P:System.Net.NetworkCredential.Password" /> property will return an empty string.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public string Password
		{
			get
			{
				return (this.password != null) ? this.password : string.Empty;
			}
			set
			{
				this.password = value;
			}
		}

		/// <summary>Returns an instance of the <see cref="T:System.Net.NetworkCredential" /> class for the specified Uniform Resource Identifier (URI) and authentication type.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkCredential" /> object.</returns>
		/// <param name="uri">The URI that the client provides authentication for. </param>
		/// <param name="authType">The type of authentication requested, as defined in the <see cref="P:System.Net.IAuthenticationModule.AuthenticationType" /> property. </param>
		public NetworkCredential GetCredential(System.Uri uri, string authType)
		{
			return this;
		}
	}
}
