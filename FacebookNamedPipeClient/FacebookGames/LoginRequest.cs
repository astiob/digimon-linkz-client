using System;

namespace FacebookGames
{
	public class LoginRequest : PipePacketRequest
	{
		public string Permissions { get; set; }

		public LoginRequest()
		{
		}

		public LoginRequest(string appId, string permissions) : base(appId)
		{
			this.Permissions = permissions;
		}
	}
}
