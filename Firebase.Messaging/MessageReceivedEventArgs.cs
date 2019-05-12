using System;

namespace Firebase.Messaging
{
	public sealed class MessageReceivedEventArgs : EventArgs
	{
		public MessageReceivedEventArgs(FirebaseMessage msg)
		{
			this.Message = msg;
		}

		public FirebaseMessage Message { get; set; }
	}
}
