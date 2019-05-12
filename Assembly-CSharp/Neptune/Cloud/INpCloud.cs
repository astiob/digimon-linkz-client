using System;
using System.Collections.Generic;

namespace Neptune.Cloud
{
	public interface INpCloud
	{
		void OnExit();

		void OnResponse(int sender, Dictionary<string, object> parameter);

		void OnCtrlResponse(string command, Dictionary<string, object> parameter);

		void OnCloudException(short exitCode, string message);

		void OnRequestException(NpCloudErrorData error);
	}
}
