using System;
using System.Collections;
using TS;
using UnityEngine;

public sealed class TutorialFirstPart : TutorialBasePart
{
	public void StartTutorial(GameObject observer, string tutorialStatusId, Action completed)
	{
		APIRequestTask task = APIUtil.Instance().RequestFirstTutorialData();
		AppCoroutine.Start(task.Run(delegate
		{
			AppCoroutine.Start(this.Initialize(observer, tutorialStatusId, completed), false);
		}, null, null), false);
	}

	private IEnumerator Initialize(GameObject observer, string tutorialStatusId, Action completed)
	{
		GashaTutorialMode.TutoExec = true;
		DataMng.Instance().CampaignForceHide = true;
		this.gameEngineController = observer.AddComponent<TutorialControlToGame>();
		MonsterDataMng.Instance().GetMonsterDataList(false);
		yield return null;
		yield return AppCoroutine.Start(base.LoadTutorialUI("Tutorial/TutorialUI"), false);
		yield return AppCoroutine.Start(this.tutorialUI.LoadNonFrameText(), false);
		yield return AppCoroutine.Start(this.tutorialUI.LoadSelectItem(), false);
		UIRoot root = GUIMain.GetUIRoot();
		if (null != root)
		{
			UIPanel uiPanel = root.GetComponent<UIPanel>();
			if (null != uiPanel)
			{
				uiPanel.depth = 100;
			}
		}
		yield return AppCoroutine.Start(this.LoadScriptEngine(tutorialStatusId), false);
		if (completed != null)
		{
			completed();
		}
		yield break;
	}

	private IEnumerator LoadScriptEngine(string tutorialStatusId)
	{
		this.scriptEngine = new ScriptEngine();
		yield return AppCoroutine.Start(base.LoadScriptFile("FirstTutorial", delegate(string text)
		{
			this.scriptEngine.Deserialize(text);
		}), false);
		int statusId = 0;
		if (!int.TryParse(tutorialStatusId, out statusId))
		{
		}
		if (100 <= statusId)
		{
			TutorialRestart commandRestart = new TutorialRestart(this.scriptEngine, this.tutorialUI, this.gameEngineController, tutorialStatusId);
			yield return AppCoroutine.Start(commandRestart.SkipCommand(), false);
			yield return AppCoroutine.Start(commandRestart.RetryDownload(), false);
			yield return AppCoroutine.Start(commandRestart.ReproduceTutorialStatus(), false);
		}
		this.tutorialCommandAction = new TutorialCommandAction(this.scriptEngine, this.tutorialUI, this.gameEngineController);
		yield break;
	}

	public override void FinishTutorial(GameObject observer)
	{
		global::Debug.Log("PartyTrack送信：チュートリアル終了");
		Partytrack.sendEvent(40871);
		this.tutorialCommandAction = null;
		this.scriptEngine = null;
		UIRoot uiroot = GUIMain.GetUIRoot();
		if (null != uiroot)
		{
			UIPanel component = uiroot.GetComponent<UIPanel>();
			if (null != component)
			{
				component.depth = 0;
			}
		}
		UnityEngine.Object.Destroy(this.tutorialUI.gameObject);
		UnityEngine.Object.Destroy(observer.GetComponent<TutorialControlToGame>());
		DataMng.Instance().CampaignForceHide = false;
		BattleStateManager.onAutoChangeTutorialMode = false;
		GashaTutorialMode.TutoExec = false;
		GUIManager.ExtBackKeyReady = true;
		RestrictionInput.isDisableBackKeySetting = false;
		DataMng.Instance().RespDataUS_MonsterList = null;
	}
}
