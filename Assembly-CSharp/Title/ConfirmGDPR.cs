using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Title
{
	public sealed class ConfirmGDPR
	{
		private ConfirmGDPR_Network network;

		[CompilerGenerated]
		private static Action <>f__mg$cache0;

		private IEnumerator WaitClose(CMD dialog)
		{
			bool isClose = false;
			dialog.SetLastCallBack(delegate
			{
				isClose = true;
			});
			while (!isClose)
			{
				yield return null;
			}
			yield break;
		}

		public IEnumerator Ready()
		{
			this.network = new ConfirmGDPR_Network();
			APIRequestTask requestInfo = this.network.GetRequestInfo();
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			ConfirmGDPR_Network confirmGDPR_Network = this.network;
			APIRequestTask request = requestInfo;
			if (ConfirmGDPR.<>f__mg$cache0 == null)
			{
				ConfirmGDPR.<>f__mg$cache0 = new Action(RestrictionInput.EndLoad);
			}
			return confirmGDPR_Network.Send(request, ConfirmGDPR.<>f__mg$cache0);
		}

		public bool IsUpdateRule()
		{
			return this.network.Exists();
		}

		public IEnumerator ShowConfirm(Action<bool> completed)
		{
			bool isCancel = false;
			CMD_ConfirmGDPR popup = CMD_ConfirmGDPR.Create(this.network);
			if (null != popup)
			{
				yield return AppCoroutine.Start(this.WaitClose(popup), false);
				isCancel = popup.IsCancel();
			}
			if (completed != null)
			{
				completed(isCancel);
			}
			yield break;
		}
	}
}
