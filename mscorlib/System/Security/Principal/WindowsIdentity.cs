using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Security.Principal
{
	/// <summary>Represents a Windows user.</summary>
	[ComVisible(true)]
	[Serializable]
	public class WindowsIdentity : IDisposable, ISerializable, IDeserializationCallback, IIdentity
	{
		private IntPtr _token;

		private string _type;

		private WindowsAccountType _account;

		private bool _authenticated;

		private string _name;

		private SerializationInfo _info;

		private static IntPtr invalidWindows = IntPtr.Zero;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Principal.WindowsIdentity" /> class for the user represented by the specified Windows account token.</summary>
		/// <param name="userToken">The account token for the user on whose behalf the code is running. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="userToken" /> is 0.-or-<paramref name="userToken" /> is duplicated and invalid for impersonation.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the correct permissions. -or-A Win32 error occurred.</exception>
		public WindowsIdentity(IntPtr userToken) : this(userToken, null, WindowsAccountType.Normal, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Principal.WindowsIdentity" /> class for the user represented by the specified Windows account token and the specified authentication type.</summary>
		/// <param name="userToken">The account token for the user on whose behalf the code is running. </param>
		/// <param name="type">(Informational) The type of authentication used to identify the user. For more information, see Remarks.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="userToken" /> is 0.-or-<paramref name="userToken" /> is duplicated and invalid for impersonation.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the correct permissions. -or-A Win32 error occurred.</exception>
		public WindowsIdentity(IntPtr userToken, string type) : this(userToken, type, WindowsAccountType.Normal, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Principal.WindowsIdentity" /> class for the user represented by the specified Windows account token, the specified authentication type, and the specified Windows account type.</summary>
		/// <param name="userToken">The account token for the user on whose behalf the code is running. </param>
		/// <param name="type">(Informational) The type of authentication used to identify the user. For more information, see Remarks.</param>
		/// <param name="acctType">One of the <see cref="T:System.Security.Principal.WindowsAccountType" /> values. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="userToken" /> is 0.-or-<paramref name="userToken" /> is duplicated and invalid for impersonation.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the correct permissions. -or-A Win32 error occurred.</exception>
		public WindowsIdentity(IntPtr userToken, string type, WindowsAccountType acctType) : this(userToken, type, acctType, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Principal.WindowsIdentity" /> class for the user represented by the specified Windows account token, the specified authentication type, the specified Windows account type, and the specified authentication status.</summary>
		/// <param name="userToken">The account token for the user on whose behalf the code is running. </param>
		/// <param name="type">(Informational) The type of authentication used to identify the user. For more information, see Remarks.</param>
		/// <param name="acctType">One of the <see cref="T:System.Security.Principal.WindowsAccountType" /> values. </param>
		/// <param name="isAuthenticated">true to indicate that the user is authenticated; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="userToken" /> is 0.-or-<paramref name="userToken" /> is duplicated and invalid for impersonation.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the correct permissions. -or-A Win32 error occurred.</exception>
		public WindowsIdentity(IntPtr userToken, string type, WindowsAccountType acctType, bool isAuthenticated)
		{
			this._type = type;
			this._account = acctType;
			this._authenticated = isAuthenticated;
			this._name = null;
			this.SetToken(userToken);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Principal.WindowsIdentity" /> class for the user represented by the specified User Principal Name (UPN).</summary>
		/// <param name="sUserPrincipalName">The UPN for the user on whose behalf the code is running. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">Windows returned the Windows NT status code STATUS_ACCESS_DENIED.</exception>
		/// <exception cref="T:System.OutOfMemoryException">There is insufficient memory available.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the correct permissions. </exception>
		public WindowsIdentity(string sUserPrincipalName) : this(sUserPrincipalName, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Principal.WindowsIdentity" /> class for the user represented by the specified User Principal Name (UPN) and the specified authentication type.</summary>
		/// <param name="sUserPrincipalName">The UPN for the user on whose behalf the code is running. </param>
		/// <param name="type">(Informational) The type of authentication used to identify the user. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">Windows returned the Windows NT status code STATUS_ACCESS_DENIED.</exception>
		/// <exception cref="T:System.OutOfMemoryException">There is insufficient memory available.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the correct permissions. </exception>
		public WindowsIdentity(string sUserPrincipalName, string type)
		{
			if (sUserPrincipalName == null)
			{
				throw new NullReferenceException("sUserPrincipalName");
			}
			IntPtr userToken = WindowsIdentity.GetUserToken(sUserPrincipalName);
			if (!WindowsIdentity.IsPosix && userToken == IntPtr.Zero)
			{
				throw new ArgumentException("only for Windows Server 2003 +");
			}
			this._authenticated = true;
			this._account = WindowsAccountType.Normal;
			this._type = type;
			this.SetToken(userToken);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Principal.WindowsIdentity" /> class for the user represented by information in a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> stream.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> containing the account information for the user. </param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that indicates the stream characteristics. </param>
		/// <exception cref="T:System.NotSupportedException">A <see cref="T:System.Security.Principal.WindowsIdentity" /> cannot be serialized across processes. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the correct permissions. -or-A Win32 error occurred.</exception>
		public WindowsIdentity(SerializationInfo info, StreamingContext context)
		{
			this._info = info;
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and is called back by the deserialization event when deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event. </param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this._token = (IntPtr)this._info.GetValue("m_userToken", typeof(IntPtr));
			this._name = this._info.GetString("m_name");
			if (this._name != null)
			{
				string tokenName = WindowsIdentity.GetTokenName(this._token);
				if (tokenName != this._name)
				{
					throw new SerializationException("Token-Name mismatch.");
				}
			}
			else
			{
				this._name = WindowsIdentity.GetTokenName(this._token);
				if (this._name == string.Empty || this._name == null)
				{
					throw new SerializationException("Token doesn't match a user.");
				}
			}
			this._type = this._info.GetString("m_type");
			this._account = (WindowsAccountType)((int)this._info.GetValue("m_acctType", typeof(WindowsAccountType)));
			this._authenticated = this._info.GetBoolean("m_isAuthenticated");
		}

		/// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the logical context information needed to recreate an instance of this execution context.</summary>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Hashtable" />. </param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Hashtable" />. </param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("m_userToken", this._token);
			info.AddValue("m_name", this._name);
			info.AddValue("m_type", this._type);
			info.AddValue("m_acctType", this._account);
			info.AddValue("m_isAuthenticated", this._authenticated);
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Security.Principal.WindowsIdentity" />. </summary>
		[ComVisible(false)]
		public void Dispose()
		{
			this._token = IntPtr.Zero;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Security.Principal.WindowsIdentity" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		[ComVisible(false)]
		protected virtual void Dispose(bool disposing)
		{
			this._token = IntPtr.Zero;
		}

		/// <summary>Returns a <see cref="T:System.Security.Principal.WindowsIdentity" /> object that represents an anonymous user.</summary>
		/// <returns>A <see cref="T:System.Security.Principal.WindowsIdentity" /> object that represents an anonymous user.</returns>
		public static WindowsIdentity GetAnonymous()
		{
			WindowsIdentity windowsIdentity;
			if (WindowsIdentity.IsPosix)
			{
				windowsIdentity = new WindowsIdentity("nobody");
				windowsIdentity._account = WindowsAccountType.Anonymous;
				windowsIdentity._authenticated = false;
				windowsIdentity._type = string.Empty;
			}
			else
			{
				windowsIdentity = new WindowsIdentity(IntPtr.Zero, string.Empty, WindowsAccountType.Anonymous, false);
				windowsIdentity._name = string.Empty;
			}
			return windowsIdentity;
		}

		/// <summary>Returns a <see cref="T:System.Security.Principal.WindowsIdentity" /> object that represents the current Windows user.</summary>
		/// <returns>A <see cref="T:System.Security.Principal.WindowsIdentity" /> object that represents the current user.</returns>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the correct permissions. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlPrincipal" />
		/// </PermissionSet>
		public static WindowsIdentity GetCurrent()
		{
			return new WindowsIdentity(WindowsIdentity.GetCurrentToken(), null, WindowsAccountType.Normal, true);
		}

		/// <summary>Returns a <see cref="T:System.Security.Principal.WindowsIdentity" /> object that represents the Windows identity for either the thread or the process, depending on the value of the <paramref name="ifImpersonating" /> parameter.</summary>
		/// <returns>A <see cref="T:System.Security.Principal.WindowsIdentity" /> object that represents a Windows user.</returns>
		/// <param name="ifImpersonating">true to return the <see cref="T:System.Security.Principal.WindowsIdentity" /> only if the thread is currently impersonating; false to return the <see cref="T:System.Security.Principal.WindowsIdentity" />   of the thread if it is impersonating or the <see cref="T:System.Security.Principal.WindowsIdentity" /> of the process if the thread is not currently impersonating.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlPrincipal" />
		/// </PermissionSet>
		[MonoTODO("need icall changes")]
		public static WindowsIdentity GetCurrent(bool ifImpersonating)
		{
			throw new NotImplementedException();
		}

		/// <summary>Returns a <see cref="T:System.Security.Principal.WindowsIdentity" /> object that represents the current Windows user, using the specified desired token access level.</summary>
		/// <returns>A <see cref="T:System.Security.Principal.WindowsIdentity" /> object that represents the current user.</returns>
		/// <param name="desiredAccess">A bitwise combination of the <see cref="T:System.Security.Principal.TokenAccessLevels" /> values. </param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlPrincipal" />
		/// </PermissionSet>
		[MonoTODO("need icall changes")]
		public static WindowsIdentity GetCurrent(TokenAccessLevels desiredAccess)
		{
			throw new NotImplementedException();
		}

		/// <summary>Impersonates the user represented by the <see cref="T:System.Security.Principal.WindowsIdentity" /> object.</summary>
		/// <returns>A <see cref="T:System.Security.Principal.WindowsImpersonationContext" /> object that represents the Windows user prior to impersonation; this can be used to revert to the original user's context.</returns>
		/// <exception cref="T:System.InvalidOperationException">An anonymous identity attempted to perform an impersonation.</exception>
		/// <exception cref="T:System.Security.SecurityException">A Win32 error occurred.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlPrincipal" />
		/// </PermissionSet>
		public virtual WindowsImpersonationContext Impersonate()
		{
			return new WindowsImpersonationContext(this._token);
		}

		/// <summary>Impersonates the user represented by the specified user token.</summary>
		/// <returns>A <see cref="T:System.Security.Principal.WindowsImpersonationContext" /> object that represents the Windows user prior to impersonation; this object can be used to revert to the original user's context.</returns>
		/// <param name="userToken">The handle of a Windows account token. This token is usually retrieved through a call to unmanaged code, such as a call to the Win32 API LogonUser function. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">Windows returned the Windows NT status code STATUS_ACCESS_DENIED.</exception>
		/// <exception cref="T:System.OutOfMemoryException">There is insufficient memory available.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the correct permissions. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlPrincipal" />
		/// </PermissionSet>
		public static WindowsImpersonationContext Impersonate(IntPtr userToken)
		{
			return new WindowsImpersonationContext(userToken);
		}

		/// <summary>Gets the type of authentication used to identify the user.</summary>
		/// <returns>The type of authentication used to identify the user.</returns>
		public string AuthenticationType
		{
			get
			{
				return this._type;
			}
		}

		/// <summary>Gets a value indicating whether the user account is identified as an anonymous account by the system.</summary>
		/// <returns>true if the user account is an anonymous account; otherwise, false.</returns>
		public virtual bool IsAnonymous
		{
			get
			{
				return this._account == WindowsAccountType.Anonymous;
			}
		}

		/// <summary>Gets a value indicating whether the user has been authenticated by Windows.</summary>
		/// <returns>true if the user was authenticated; otherwise, false.</returns>
		public virtual bool IsAuthenticated
		{
			get
			{
				return this._authenticated;
			}
		}

		/// <summary>Gets a value indicating whether the user account is identified as a <see cref="F:System.Security.Principal.WindowsAccountType.Guest" /> account by the system.</summary>
		/// <returns>true if the user account is a <see cref="F:System.Security.Principal.WindowsAccountType.Guest" /> account; otherwise, false.</returns>
		public virtual bool IsGuest
		{
			get
			{
				return this._account == WindowsAccountType.Guest;
			}
		}

		/// <summary>Gets a value indicating whether the user account is identified as a <see cref="F:System.Security.Principal.WindowsAccountType.System" /> account by the system.</summary>
		/// <returns>true if the user account is a <see cref="F:System.Security.Principal.WindowsAccountType.System" /> account; otherwise, false.</returns>
		public virtual bool IsSystem
		{
			get
			{
				return this._account == WindowsAccountType.System;
			}
		}

		/// <summary>Gets the user's Windows logon name.</summary>
		/// <returns>The Windows logon name of the user on whose behalf the code is being run.</returns>
		public virtual string Name
		{
			get
			{
				if (this._name == null)
				{
					this._name = WindowsIdentity.GetTokenName(this._token);
				}
				return this._name;
			}
		}

		/// <summary>Gets the Windows account token for the user.</summary>
		/// <returns>The handle of the access token associated with the current execution thread.</returns>
		public virtual IntPtr Token
		{
			get
			{
				return this._token;
			}
		}

		/// <summary>Gets the groups the current Windows user belongs to.</summary>
		/// <returns>An <see cref="T:System.Security.Principal.IdentityReferenceCollection" /> object representing the groups the current Windows user belongs to.</returns>
		[MonoTODO("not implemented")]
		public IdentityReferenceCollection Groups
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets the impersonation level for the user.</summary>
		/// <returns>One of the <see cref="T:System.Management.ImpersonationLevel" /> values. </returns>
		[ComVisible(false)]
		[MonoTODO("not implemented")]
		public TokenImpersonationLevel ImpersonationLevel
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets the security identifier (SID) for the token owner.</summary>
		/// <returns>A <see cref="T:System.Security.Principal.SecurityIdentifier" /> object for the token owner.</returns>
		[MonoTODO("not implemented")]
		[ComVisible(false)]
		public SecurityIdentifier Owner
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets the security identifier (SID) for the user.</summary>
		/// <returns>A <see cref="T:System.Security.Principal.SecurityIdentifier" /> object for the user.</returns>
		[MonoTODO("not implemented")]
		[ComVisible(false)]
		public SecurityIdentifier User
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		private static bool IsPosix
		{
			get
			{
				int platform = (int)Environment.Platform;
				return platform == 128 || platform == 4 || platform == 6;
			}
		}

		private void SetToken(IntPtr token)
		{
			if (WindowsIdentity.IsPosix)
			{
				this._token = token;
				if (this._type == null)
				{
					this._type = "POSIX";
				}
				if (this._token == IntPtr.Zero)
				{
					this._account = WindowsAccountType.System;
				}
			}
			else
			{
				if (token == WindowsIdentity.invalidWindows && this._account != WindowsAccountType.Anonymous)
				{
					throw new ArgumentException("Invalid token");
				}
				this._token = token;
				if (this._type == null)
				{
					this._type = "NTLM";
				}
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string[] _GetRoles(IntPtr token);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr GetCurrentToken();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetTokenName(IntPtr token);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetUserToken(string username);
	}
}
