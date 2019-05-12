using System;
using System.Collections.Generic;
using UnityEngine.Cloud.Service;

namespace UnityEngine.Analytics
{
	internal class AnalyticsCloudService : ICloudService
	{
		private CloudService m_CloudService = new CloudService(CloudServiceType.Analytics);

		public bool Initialize(string projectId)
		{
			return this.m_CloudService.Initialize(projectId);
		}

		public bool StartEventHandler(string sessionInfo, int maxNumberOfEventInQueue, int maxEventTimeoutInSec)
		{
			return this.m_CloudService.StartEventHandler(sessionInfo, maxNumberOfEventInQueue, maxEventTimeoutInSec);
		}

		public bool PauseEventHandler(bool flushEvents)
		{
			return this.m_CloudService.PauseEventHandler(flushEvents);
		}

		public bool StopEventHandler()
		{
			return this.m_CloudService.StopEventHandler();
		}

		public bool StartEventDispatcher(CloudServiceConfig serviceConfig, Dictionary<string, string> headers)
		{
			return this.m_CloudService.StartEventDispatcher(serviceConfig, headers);
		}

		public bool PauseEventDispatcher()
		{
			return this.m_CloudService.PauseEventDispatcher();
		}

		public bool StopEventDispatcher()
		{
			return this.m_CloudService.StopEventDispatcher();
		}

		public void ResetNetworkRetryIndex()
		{
			this.m_CloudService.ResetNetworkRetryIndex();
		}

		public bool QueueEvent(string eventData, CloudEventFlags flags)
		{
			return this.m_CloudService.QueueEvent(eventData, flags);
		}

		public bool SaveFileFromServer(string fileName, string url, Dictionary<string, string> headers, ICloudServiceListener listener, Action<string, string, bool, int> doneMethod)
		{
			return this.m_CloudService.SaveFileFromServer(fileName, url, headers, listener, "OnDoneSaveFileFromServer");
		}

		public bool SaveFile(string fileName, string data)
		{
			return this.m_CloudService.SaveFile(fileName, data);
		}

		public string RestoreFile(string fileName)
		{
			return this.m_CloudService.RestoreFile(fileName);
		}

		public string serviceFolderName
		{
			get
			{
				return this.m_CloudService.serviceFolderName;
			}
		}
	}
}
