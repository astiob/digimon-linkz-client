using System;

namespace WebSocketSharp.Net
{
	public class NetworkCredential
	{
		private string _domain;

		private string _password;

		private string[] _roles;

		private string _username;

		public NetworkCredential(string username, string password) : this(username, password, null, new string[0])
		{
		}

		public NetworkCredential(string username, string password, string domain, params string[] roles)
		{
			if (username == null || username.Length == 0)
			{
				throw new ArgumentException("Must not be null or empty.", "username");
			}
			this._username = username;
			this._password = password;
			this._domain = domain;
			this._roles = roles;
		}

		public string Domain
		{
			get
			{
				return this._domain ?? string.Empty;
			}
			internal set
			{
				this._domain = value;
			}
		}

		public string Password
		{
			get
			{
				return this._password ?? string.Empty;
			}
			internal set
			{
				this._password = value;
			}
		}

		public string[] Roles
		{
			get
			{
				return this._roles;
			}
			internal set
			{
				this._roles = value;
			}
		}

		public string UserName
		{
			get
			{
				return this._username;
			}
			internal set
			{
				this._username = value;
			}
		}
	}
}
