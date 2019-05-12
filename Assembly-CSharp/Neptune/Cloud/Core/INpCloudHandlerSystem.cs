using System;
using System.Collections.Generic;

namespace Neptune.Cloud.Core
{
	public interface INpCloudHandlerSystem
	{
		void Update();

		void OnApplicationPause(bool pauseStatus);

		void OnApplicationQuit();

		void OnGetRoomList(List<NpRoomParameter> roomData);

		void OnHttpRequestException(uint errorCode, string command, string errorMsg, string detail);
	}
}
