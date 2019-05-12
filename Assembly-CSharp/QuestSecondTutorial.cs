using System;
using UnityEngine;

public static class QuestSecondTutorial
{
	private static string GetQuestTopTutorialFileName(string worldAreaId)
	{
		string result = string.Empty;
		switch (worldAreaId)
		{
		case "1":
			result = "second_tutorial_quest";
			break;
		case "2":
			result = "second_tutorial_quest_week";
			break;
		case "3":
			result = "second_tutorial_quest_advent";
			break;
		case "8":
			result = string.Empty;
			break;
		case "9":
			result = "second_tutorial_quest_beginner";
			break;
		}
		return result;
	}

	private static void InitializedQuestSelectTutorial()
	{
		GUICollider.EnableAllCollider("CMD_QuestSelect");
	}

	private static void InitializedQuestTopTutorial()
	{
		GUICollider.EnableAllCollider("CMD_QuestTOP");
	}

	public static void StartQuestSelectTutorial()
	{
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (null == tutorialObserver)
		{
			GUICollider.EnableAllCollider("CMD_QuestSelect");
		}
		else
		{
			GUIMain.BarrierON(null);
			tutorialObserver.StartSecondTutorial("second_tutorial_quest", new Action(GUIMain.BarrierOFF), new Action(QuestSecondTutorial.InitializedQuestSelectTutorial));
		}
	}

	public static void StartQuestTopTutorial(string worldAreaId)
	{
		string questTopTutorialFileName = QuestSecondTutorial.GetQuestTopTutorialFileName(worldAreaId);
		if (string.IsNullOrEmpty(questTopTutorialFileName))
		{
			GUICollider.EnableAllCollider("CMD_QuestTOP");
		}
		else
		{
			TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
			if (null == tutorialObserver)
			{
				GUICollider.EnableAllCollider("CMD_QuestTOP");
			}
			else
			{
				GUIMain.BarrierON(null);
				tutorialObserver.StartSecondTutorial(questTopTutorialFileName, new Action(GUIMain.BarrierOFF), new Action(QuestSecondTutorial.InitializedQuestTopTutorial));
			}
		}
	}
}
