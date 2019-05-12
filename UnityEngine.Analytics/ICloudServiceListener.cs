using System;

namespace UnityEngine.Analytics
{
	internal interface ICloudServiceListener
	{
		void OnDoneSaveFileFromServer(string fileName, string etag, bool fileUpdated, int responseStatus);
	}
}
