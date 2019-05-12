using System;

namespace WebSocketSharp
{
	public enum CloseStatusCode : ushort
	{
		Normal = 1000,
		Away,
		ProtocolError,
		IncorrectData,
		Undefined,
		NoStatusCode,
		Abnormal,
		InconsistentData,
		PolicyViolation,
		TooBig,
		IgnoreExtension,
		ServerError,
		TlsHandshakeFailure = 1015
	}
}
