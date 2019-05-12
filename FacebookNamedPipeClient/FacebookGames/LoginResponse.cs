using System;
using System.Collections.Generic;

namespace FacebookGames
{
	public class LoginResponse : PipePacketResponse
	{
		public string UserId { get; set; }

		public string AccessToken { get; set; }

		public string ExpirationTimestamp { get; set; }

		public string Permissions { get; set; }

		public LoginResponse()
		{
		}

		public LoginResponse(string userId, string accessToken, string expirationTimestamp, string permissions, string error = null, bool cancelled = false) : base(error, cancelled)
		{
			this.UserId = userId;
			this.AccessToken = accessToken;
			this.ExpirationTimestamp = expirationTimestamp;
			this.Permissions = permissions;
		}

		public override IDictionary<string, object> ToDictionary()
		{
			IDictionary<string, object> dictionary = base.ToDictionary();
			if (base.Cancelled || !string.IsNullOrEmpty(base.Error))
			{
				return dictionary;
			}
			dictionary.Add("user_id", this.UserId);
			dictionary.Add("access_token", this.AccessToken);
			dictionary.Add("expiration_timestamp", this.ExpirationTimestamp);
			dictionary.Add("permissions", this.Permissions);
			return dictionary;
		}
	}
}
