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
			Partytrack.start(5789, "07e569a17368b4a04e0eed94ee2937f3");
			AlertMaster.Initialize();
			StringMaster.Initialize();
		}
		GUIMain.StartupScreen("UIStartupCaution");
	}
}
