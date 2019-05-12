using System;

namespace WebSocketSharp
{
	public class WebSocketException : Exception
	{
		internal WebSocketException() : this(CloseStatusCode.Abnormal, null, null)
		{
		}

		internal WebSocketException(string reason) : this(CloseStatusCode.Abnormal, reason, null)
		{
		}

		internal WebSocketException(CloseStatusCode code) : this(code, null, null)
		{
		}

		internal WebSocketException(string reason, Exception innerException) : this(CloseStatusCode.Abnormal, reason, innerException)
		{
		}

		internal WebSocketException(CloseStatusCode code, string reason) : this(code, reason, null)
		{
		}

		internal WebSocketException(CloseStatusCode code, string reason, Exception innerException) : base(reason ?? code.GetMessage(), innerException)
		{
			this.Code = code;
		}

		public CloseStatusCode Code { get; private set; }
	}
}
