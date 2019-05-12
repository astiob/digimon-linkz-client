using System;
using System.Runtime.Serialization;

namespace Mono.Security.Protocol.Tls
{
	[Serializable]
	internal sealed class TlsException : Exception
	{
		private Alert alert;

		internal TlsException(string message) : base(message)
		{
		}

		internal TlsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		internal TlsException(string message, Exception ex) : base(message, ex)
		{
		}

		internal TlsException(AlertLevel level, AlertDescription description) : this(level, description, Alert.GetAlertMessage(description))
		{
		}

		internal TlsException(AlertLevel level, AlertDescription description, string message) : base(message)
		{
			this.alert = new Alert(level, description);
		}

		internal TlsException(AlertDescription description) : this(description, Alert.GetAlertMessage(description))
		{
		}

		internal TlsException(AlertDescription description, string message) : base(message)
		{
			this.alert = new Alert(description);
		}

		public Alert Alert
		{
			get
			{
				return this.alert;
			}
		}
	}
}
