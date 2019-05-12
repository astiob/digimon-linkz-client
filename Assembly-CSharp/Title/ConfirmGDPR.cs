using System;
using System.Collections;

namespace Title
{
	public sealed class ConfirmGDPR
	{
		private ConfirmGDPR_Network network;

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
			return this.network.Send(requestInfo, new Action(RestrictionInput.EndLoad));
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
