using System;
using UnityEngine;

public class ChipTutoial
{
	public static void Start(string tutorialName, Action callback = null)
	{
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (tutorialObserver != null)
		{
			GUIMain.BarrierON(null);
			tutorialObserver.StartSecondTutorial(tutorialName, new Action(GUIMain.BarrierOFF), callback);
		}
		else if (callback != null)
		{
			callback();
		}
	}
}
