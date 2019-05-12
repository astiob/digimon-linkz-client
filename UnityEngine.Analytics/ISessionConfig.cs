using System;

namespace UnityEngine.Analytics
{
	internal interface ISessionConfig
	{
		bool analyticsEnabled { get; }

		string eventsEndPoint { get; }

		string configEndPoint { get; }

		int requestNumberOfEvents { get; }

		int maxNumberOfEventInGroup { get; }

		int maxTimeoutForGrouping { get; }

		int maxNumberOfEventInQueue { get; }

		int resumeTimeoutInMillSeconds { get; }

		int[] dispatcherWaitTimeInSeconds { get; }
	}
}
