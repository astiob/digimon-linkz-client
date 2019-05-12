using System;
using UnityEngine;

namespace CharacterMiniStatusUI
{
	[Serializable]
	public sealed class StatusPanelViewControl
	{
		[SerializeField]
		private bool defaultHidden;

		[SerializeField]
		private GameObject[] pageList;

		private int viewPage;

		private void HidePage(int hidePage)
		{
			if (0 <= hidePage && hidePage < this.pageList.Length && this.pageList[hidePage].activeSelf)
			{
				this.pageList[hidePage].SetActive(false);
			}
		}

		private void HideAllPage()
		{
			for (int i = 0; i < this.pageList.Length; i++)
			{
				if (this.pageList[i].activeSelf)
				{
					this.pageList[i].SetActive(false);
				}
			}
		}

		private int UpdateViewPage(int targetPage)
		{
			if (targetPage < 0 || this.pageList.Length <= targetPage)
			{
				if (this.defaultHidden)
				{
					targetPage = -1;
				}
				else
				{
					targetPage = 0;
					this.pageList[targetPage].SetActive(true);
				}
			}
			else
			{
				this.pageList[targetPage].SetActive(true);
			}
			return targetPage;
		}

		public void Initialize()
		{
			this.HideAllPage();
			int targetPage = (!this.defaultHidden) ? 0 : -1;
			this.viewPage = this.UpdateViewPage(targetPage);
		}

		public void SetNextPage()
		{
			this.HidePage(this.viewPage);
			this.viewPage = this.UpdateViewPage(this.viewPage + 1);
		}
	}
}
