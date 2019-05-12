using System;
using System.Collections;
using TS;
using UnityEngine;

public sealed class TutorialSecondPart : TutorialBasePart
{
	private Action actionWindowClose;

	private string tutorialName;

	public IEnumerator StartTutorial(GameObject observer, string tutorialName, Action tutorialCompleted, Action initialized)
	{
		this.actionWindowClose = tutorialCompleted;
		yield return AppCoroutine.Start(this.Initialize(observer, tutorialName), false);
		if (initialized != null)
		{
			initialized();
		}
		yield break;
	}

	public IEnumerator Initialize(GameObject observer, string tutorialName)
	{
		this.gameEngineController = observer.AddComponent<TutorialControlToGame>();
		yield return AppCoroutine.Start(base.LoadTutorialUI("Tutorial/TutorialUI"), false);
		this.scriptEngine = new ScriptEngine();
		yield return AppCoroutine.Start(base.LoadScriptFile(tutorialName, delegate(string text)
		{
			this.tutorialName = tutorialName;
			this.scriptEngine.Deserialize(text);
		}), false);
		this.tutorialCommandAction = new TutorialCommandAction(this.scriptEngine, this.tutorialUI, this.gameEngineController);
		yield break;
	}

	public override void FinishTutorial(GameObject observer)
	{
		PlayerPrefs.SetInt(this.tutorialName, 1);
		this.tutorialCommandAction = null;
		this.scriptEngine = null;
		UnityEngine.Object.Destroy(this.tutorialUI.gameObject);
		this.tutorialUI = null;
		UnityEngine.Object.Destroy(observer.GetComponent<TutorialControlToGame>());
		GUIManager.ExtBackKeyReady = true;
		RestrictionInput.isDisableBackKeySetting = false;
		if (this.actionWindowClose != null)
		{
			this.actionWindowClose();
			this.actionWindowClose = null;
		}
	}
}
