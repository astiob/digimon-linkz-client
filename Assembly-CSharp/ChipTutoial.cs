using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChipTutoial
{
	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	public static void Start(string tutorialName, Action callback = null)
	{
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (tutorialObserver != null)
		{
			GUIMain.BarrierON(null);
			TutorialObserver tutorialObserver2 = tutorialObserver;
			if (ChipTutoial.<>f__mg$cache0 == null)
			{
				ChipTutoial.<>f__mg$cache0 = new Action(GUIMain.BarrierOFF);
			}
			tutorialObserver2.StartSecondTutorial(tutorialName, ChipTutoial.<>f__mg$cache0, callback);
		}
		else if (callback != null)
		{
			callback();
		}
	}
}
