using System;

namespace Firebase.Messaging
{
	public sealed class TokenReceivedEventArgs : EventArgs
	{
		public TokenReceivedEventArgs(string token)
		{
			this.Token = token;
		}

		public string Token { get; set; }
	}
}
