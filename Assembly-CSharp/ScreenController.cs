using System;

public static class ScreenController
{
	private static void OnStartHomeScreen(CMD_Tips.DISPLAY_PLACE tipsType)
	{
		Loading.DisableMask();
		TipsLoading.Instance.StartTipsLoad(tipsType, true);
	}

	public static void ChangeHomeScreen(CMD_Tips.DISPLAY_PLACE tipsType)
	{
		GUIMain.FadeBlackReqScreen("UIHome", delegate(int noop)
		{
			ScreenController.OnStartHomeScreen(tipsType);
		}, 1f, 0f, true, null);
	}

	public static GUIBase ChangeTutorialHomeScreen()
	{
		if (!Loading.IsShow())
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
		}
		if (!TipsLoading.Instance.IsShow)
		{
			TipsLoading.Instance.StartTipsLoad(CMD_Tips.DISPLAY_PLACE.TutorialToFarm, true);
		}
		GUIMain.ReqScreen("UIHome", "UIHomeTutorial");
		return GUIManager.GetGUI("UIHome");
	}
}
