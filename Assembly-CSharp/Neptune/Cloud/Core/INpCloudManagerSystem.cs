using System;

namespace Neptune.Cloud.Core
{
	public interface INpCloudManagerSystem
	{
		bool Receive(string command, object receiveData, long resTime);

		void CloudExit(int exitCode, string message);

		bool ReceiveException(NpCloudErrorData errorData);
	}
}
