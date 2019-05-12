using System;

namespace System.Net
{
	/// <summary>Contains an authentication message for an Internet server.</summary>
	public class Authorization
	{
		private string token;

		private bool complete;

		private string connectionGroupId;

		private string[] protectionRealm;

		private IAuthenticationModule module;

		/// <summary>Creates a new instance of the <see cref="T:System.Net.Authorization" /> class with the specified authorization message.</summary>
		/// <param name="token">The encrypted authorization message expected by the server. </param>
		public Authorization(string token) : this(token, true)
		{
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Net.Authorization" /> class with the specified authorization message and completion status.</summary>
		/// <param name="token">The encrypted authorization message expected by the server. </param>
		/// <param name="finished">The completion status of the authorization attempt. true if the authorization attempt is complete; otherwise, false. </param>
		public Authorization(string token, bool complete) : this(token, complete, null)
		{
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Net.Authorization" /> class with the specified authorization message, completion status, and connection group identifier.</summary>
		/// <param name="token">The encrypted authorization message expected by the server. </param>
		/// <param name="finished">The completion status of the authorization attempt. true if the authorization attempt is complete; otherwise, false. </param>
		/// <param name="connectionGroupId">A unique identifier that can be used to create private client-server connections that are bound only to this authentication scheme. </param>
		public Authorization(string token, bool complete, string connectionGroupId)
		{
			this.token = token;
			this.complete = complete;
			this.connectionGroupId = connectionGroupId;
		}

		/// <summary>Gets the message returned to the server in response to an authentication challenge.</summary>
		/// <returns>The message that will be returned to the server in response to an authentication challenge.</returns>
		public string Message
		{
			get
			{
				return this.token;
			}
		}

		/// <summary>Gets the completion status of the authorization.</summary>
		/// <returns>true if the authentication process is complete; otherwise, false.</returns>
		public bool Complete
		{
			get
			{
				return this.complete;
			}
		}

		/// <summary>Gets a unique identifier for user-specific connections.</summary>
		/// <returns>A unique string that associates a connection with an authenticating entity.</returns>
		public string ConnectionGroupId
		{
			get
			{
				return this.connectionGroupId;
			}
		}

		/// <summary>Gets or sets the prefix for Uniform Resource Identifiers (URIs) that can be authenticated with the <see cref="P:System.Net.Authorization.Message" /> property.</summary>
		/// <returns>An array of strings that contains URI prefixes.</returns>
		public string[] ProtectionRealm
		{
			get
			{
				return this.protectionRealm;
			}
			set
			{
				this.protectionRealm = value;
			}
		}

		internal IAuthenticationModule Module
		{
			get
			{
				return this.module;
			}
			set
			{
				this.module = value;
			}
		}

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that indicates whether mutual authentication occurred.</summary>
		/// <returns>true if both client and server were authenticated; otherwise, false.</returns>
		[MonoTODO]
		public bool MutuallyAuthenticated
		{
			get
			{
				throw Authorization.GetMustImplement();
			}
			set
			{
				throw Authorization.GetMustImplement();
			}
		}
	}
}
