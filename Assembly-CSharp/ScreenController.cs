using System;
using UnityEngine;

public static class ScreenController
{
	private static void CreateHomeUI()
	{
		if (null == GUIFaceIndicator.instance && null == GUIFace.instance)
		{
			GameObject gameObject = AssetDataMng.Instance().LoadObject("UIPrefab/GUI/Face", null, true) as GameObject;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			if (null != gameObject2)
			{
				Transform transform = gameObject2.transform;
				transform.parent = Singleton<GUIMain>.Instance.transform;
				transform.localScale = Vector3.one;
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				transform.name = gameObject.name;
			}
		}
	}

	private static void OnStartHomeScreen(CMD_Tips.DISPLAY_PLACE tipsType)
	{
		ScreenController.CreateHomeUI();
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
		ScreenController.CreateHomeUI();
		GUIMain.ReqScreen("UIHome", "UIHomeTutorial");
		return GUIManager.GetGUI("UIHome");
	}
}
