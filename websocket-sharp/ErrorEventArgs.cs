using System;

namespace WebSocketSharp
{
	public class ErrorEventArgs : EventArgs
	{
		internal ErrorEventArgs(string message)
		{
			this.Message = message;
		}

		public string Message { get; private set; }
	}
}
