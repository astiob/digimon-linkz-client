using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	/// <summary>Indicates the status of the server message processing.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum ServerProcessing
	{
		/// <summary>The server synchronously processed the message.</summary>
		Complete,
		/// <summary>The message was dispatched and no response can be sent.</summary>
		OneWay,
		/// <summary>The call was dispatched asynchronously, which indicates that the sink must store response data on the stack for later processing.</summary>
		Async
	}
}
