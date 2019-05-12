using Quest;
using System;
using System.Collections.Generic;

public sealed class RecruitmentBoardButton : FacilityButtonSet
{
	private void OnPushedTransitionButton()
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

	private void AfterGetDungeonInfo(bool complate)
	{
		RestrictionInput.EndLoad();
		if (GUIManager.CheckTopDialog("CMD_MultiRecruitTop", null) == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OpenCMD));
		}
	}

	private void OpenCMD(int i)
	{
		GUIMain.ShowCommonDialog(null, "CMD_MultiRecruitTop");
	}
}
