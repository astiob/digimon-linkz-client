using Master;
using System;
using UnityEngine;

public class DMMain : MonoBehaviour
{
	private static bool isJustOnce;

	private void Awake()
	{
	}

	private void Start()
	{
		if (!DMMain.isJustOnce)
		{
			DMMain.isJustOnce = true;
			AdjustWrapper.Instance.StartAdjust();
			AlertMaster.Initialize();
			StringMaster.Initialize();
		}
		GUIMain.StartupScreen("UIStartupCaution");
	}
}
