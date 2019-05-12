using Cutscene;
using System;
using UnityEngine;

public sealed class CutSceneMain : MonoBehaviour
{
	private static Action cs_startSceneCallBack;

	private static CutsceneBase cutscene;

	private static GameObject goGUI_CAM;

	public static void FadeReqCutScene(CutsceneDataBase cutsceneData, Action startSceneCallBack, Action endSceneCallBack, Action<int> endFadeInCallBack, float outSec = 0.5f, float inSec = 0.5f)
	{
		CutSceneMain.cs_startSceneCallBack = startSceneCallBack;
		GUIFadeControll.SetFadeInfo(outSec, 0f, inSec, 1f);
		GUIFadeControll.SetLoadInfo(new Action<int>(CutSceneMain.ExecCutScene), string.Empty, string.Empty, string.Empty, endFadeInCallBack, false);
		CutSceneMain.cutscene = CutsceneFactory.Create(cutsceneData);
		GUIManager.LoadCommonGUI("Effect/FADE_B", GUIMain.GetOrthoCamera().gameObject);
	}

	private static void ExecCutScene(int i)
	{
		GameObject gameObject = GUIMain.GetOrthoCamera().gameObject;
		if (null != gameObject)
		{
			CutSceneMain.goGUI_CAM = gameObject;
			Camera orthoCamera = GUIMain.GetOrthoCamera();
			orthoCamera.depth = 0f;
			orthoCamera.enabled = false;
		}
		CutSceneMain.cutscene.StartCutscene();
		if (CutSceneMain.cs_startSceneCallBack != null)
		{
			CutSceneMain.cs_startSceneCallBack();
			GUIMain.AdjustBarrierZ();
		}
	}

	public static void FadeReqCutSceneEnd()
	{
		if (null != CutSceneMain.goGUI_CAM)
		{
			Camera component = CutSceneMain.goGUI_CAM.GetComponent<Camera>();
			component.depth = 8f;
			component.enabled = true;
		}
		GUIFadeControll.ActionRestart();
	}
}
