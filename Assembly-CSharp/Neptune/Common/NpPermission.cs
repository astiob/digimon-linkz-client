using JsonFx.Json;
using System;
using System.Collections.Generic;

namespace Neptune.Common
{
	public class NpPermission : NpSingleton<NpPermission>
	{
		private INpPermission mCallbackListener;

		public void EnableDebugLog(bool isDebug)
		{
			NpPermissionAndroid.EnableDebugLog(isDebug);
		}

		public void RequestPermissions(ManifestPermission manifestPermission, INpPermission listener)
		{
			this.mCallbackListener = listener;
			NpPermissionAndroid.RequestPermissions(manifestPermission);
		}

		private void OnRequestPermissionsResult(string resultJson)
		{
			Dictionary<string, object> dictionary = JsonReader.Deserialize<Dictionary<string, object>>(resultJson);
			int num = (int)dictionary["permission"];
			int num2 = (int)dictionary["permissionState"];
			if (this.mCallbackListener != null)
			{
				this.mCallbackListener.OnRequestPermissionsResult((ManifestPermission)num, (PermisionState)num2);
			}
			else
			{
				Debug.LogError("OnRequestPermissionsResult : Callback Listener is null.");
			}
		}
	}
}
