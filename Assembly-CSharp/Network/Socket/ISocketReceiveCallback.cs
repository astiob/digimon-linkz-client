using System;
using System.Collections.Generic;

namespace Network.Socket
{
	public interface ISocketReceiveCallback
	{
		void APIResponse(int activityId, List<object> response);

		void APIResponse(int activityId, Dictionary<string, object> response);

		void ClientMessage(string dataType, Dictionary<object, object> response);
	}
}
