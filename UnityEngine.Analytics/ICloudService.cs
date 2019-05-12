using System;
using System.Collections.Generic;
using UnityEngine.Cloud.Service;

namespace UnityEngine.Analytics
{
	internal interface ICloudService
	{
		bool Initialize(string projectId);

		bool StartEventHandler(string sessionInfo, int maxNumberOfEventInQueue, int maxEventTimeoutInSec);

		bool PauseEventHandler(bool flushEvents);

		bool StopEventHandler();

		bool StartEventDispatcher(CloudServiceConfig serviceConfig, Dictionary<string, string> headers);

		bool PauseEventDispatcher();

		bool StopEventDispatcher();

		void ResetNetworkRetryIndex();

		bool QueueEvent(string eventData, CloudEventFlags flags);

		bool SaveFileFromServer(string fileName, string url, Dictionary<string, string> headers, ICloudServiceListener listener, Action<string, string, bool, int> doneMethod);

		bool SaveFile(string fileName, string data);

		string RestoreFile(string fileName);

		string serviceFolderName { get; }
	}
}
