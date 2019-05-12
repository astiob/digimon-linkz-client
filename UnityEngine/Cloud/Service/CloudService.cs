using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Cloud.Service
{
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class CloudService : IDisposable
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		public CloudService(CloudServiceType serviceType)
		{
			this.InternalCreate(serviceType);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void InternalCreate(CloudServiceType serviceType);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void InternalDestroy();

		~CloudService()
		{
			this.InternalDestroy();
		}

		public void Dispose()
		{
			this.InternalDestroy();
			GC.SuppressFinalize(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool Initialize(string projectId);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool StartEventHandler(string sessionInfo, int maxNumberOfEventInQueue, int maxEventTimeoutInSec);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool PauseEventHandler(bool flushEvents);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool StopEventHandler();

		public bool StartEventDispatcher(CloudServiceConfig serviceConfig, Dictionary<string, string> headers)
		{
			return this.InternalStartEventDispatcher(serviceConfig, CloudService.FlattenedHeadersFrom(headers));
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool InternalStartEventDispatcher(CloudServiceConfig serviceConfig, string[] headers);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool PauseEventDispatcher();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool StopEventDispatcher();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetNetworkRetryIndex();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool QueueEvent(string eventData, CloudEventFlags flags);

		public bool SaveFileFromServer(string fileName, string url, Dictionary<string, string> headers, object d, string methodName)
		{
			if (methodName == null)
			{
				methodName = string.Empty;
			}
			return this.InternalSaveFileFromServer(fileName, url, CloudService.FlattenedHeadersFrom(headers), d, methodName);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool InternalSaveFileFromServer(string fileName, string url, string[] headers, object d, string methodName);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SaveFile(string fileName, string data);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string RestoreFile(string fileName);

		public extern string serviceFolderName { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		private static string[] FlattenedHeadersFrom(Dictionary<string, string> headers)
		{
			if (headers == null)
			{
				return null;
			}
			string[] array = new string[headers.Count * 2];
			int num = 0;
			foreach (KeyValuePair<string, string> keyValuePair in headers)
			{
				array[num++] = keyValuePair.Key.ToString();
				array[num++] = keyValuePair.Value.ToString();
			}
			return array;
		}
	}
}
