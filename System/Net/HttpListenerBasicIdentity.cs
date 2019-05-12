using System;
using System.Security.Principal;

namespace System.Net
{
	/// <summary>Holds the user name and password from a basic authentication request.</summary>
	public class HttpListenerBasicIdentity : GenericIdentity
	{
		private string password;

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.HttpListenerBasicIdentity" /> class using the specified user name and password.</summary>
		/// <param name="username">The user name.</param>
		/// <param name="password">The password.</param>
		public HttpListenerBasicIdentity(string username, string password) : base(username, "Basic")
		{
			this.password = password;
		}

		/// <summary>Indicates the password from a basic authentication attempt.</summary>
		/// <returns>A <see cref="T:System.String" /> that holds the password.</returns>
		public virtual string Password
		{
			get
			{
				return this.password;
			}
		}
	}
}
