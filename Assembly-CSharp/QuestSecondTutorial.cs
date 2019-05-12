using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class QuestSecondTutorial
{
	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	[CompilerGenerated]
	private static Action <>f__mg$cache1;

	[CompilerGenerated]
	private static Action <>f__mg$cache2;

	[CompilerGenerated]
	private static Action <>f__mg$cache3;

	private static string GetQuestTopTutorialFileName(string worldAreaId)
	{
		string result = string.Empty;
		if (worldAreaId != null)
		{
			if (!(worldAreaId == "1"))
			{
				if (!(worldAreaId == "2"))
				{
					if (!(worldAreaId == "3"))
					{
						if (!(worldAreaId == "8"))
						{
							if (worldAreaId == "9")
							{
								result = "second_tutorial_quest_beginner";
							}
						}
						else
						{
							result = string.Empty;
						}
					}
					else
					{
						result = "second_tutorial_quest_advent";
					}
				}
				else
				{
					result = "second_tutorial_quest_week";
				}
			}
			else
			{
				result = "second_tutorial_quest";
			}
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
			TutorialObserver tutorialObserver2 = tutorialObserver;
			string tutorialName = "second_tutorial_quest";
			if (QuestSecondTutorial.<>f__mg$cache0 == null)
			{
				QuestSecondTutorial.<>f__mg$cache0 = new Action(GUIMain.BarrierOFF);
			}
			Action completed = QuestSecondTutorial.<>f__mg$cache0;
			if (QuestSecondTutorial.<>f__mg$cache1 == null)
			{
				QuestSecondTutorial.<>f__mg$cache1 = new Action(QuestSecondTutorial.InitializedQuestSelectTutorial);
			}
			tutorialObserver2.StartSecondTutorial(tutorialName, completed, QuestSecondTutorial.<>f__mg$cache1);
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
				TutorialObserver tutorialObserver2 = tutorialObserver;
				string tutorialName = questTopTutorialFileName;
				if (QuestSecondTutorial.<>f__mg$cache2 == null)
				{
					QuestSecondTutorial.<>f__mg$cache2 = new Action(GUIMain.BarrierOFF);
				}
				Action completed = QuestSecondTutorial.<>f__mg$cache2;
				if (QuestSecondTutorial.<>f__mg$cache3 == null)
				{
					QuestSecondTutorial.<>f__mg$cache3 = new Action(QuestSecondTutorial.InitializedQuestTopTutorial);
				}
				tutorialObserver2.StartSecondTutorial(tutorialName, completed, QuestSecondTutorial.<>f__mg$cache3);
			}
		}
	}
}
