using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NextChoice
{
	[AddComponentMenu("NextChoice/RecruitmentBoardButton")]
	public sealed class RecruitmentBoardButton : GUICollider
	{
		[SerializeField]
		private CMD parentWindow;

		public void OpenRecruitmentBoard()
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			List<string> worldIdList = new List<string>
			{
				"1",
				"3",
				"8"
			};
			ClassSingleton<QuestData>.Instance.GetWorldDungeonInfo(worldIdList, new Action<bool>(this.AfterGetDungeonInfo));
		}

		private void AfterGetDungeonInfo(bool success)
		{
			RestrictionInput.EndLoad();
			if (success)
			{
				CMD_MultiRecruitTop window = CMD_MultiRecruitTop.Create();
				window.PartsTitle.SetReturnAct(delegate(int i)
				{
					window.ClosePanel(true);
				});
				window.PartsTitle.DisableReturnBtn(false);
				window.PartsTitle.SetCloseAct(delegate(int i)
				{
					this.parentWindow.ClosePanel(false);
					window.SetCloseAction(delegate(int x)
					{
						CMD_BattleNextChoice.GoToFarm();
					});
					window.ClosePanel(true);
				});
			}
		}
	}
}
