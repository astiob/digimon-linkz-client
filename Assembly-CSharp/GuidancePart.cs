using PersistentData;
using System;
using System.Collections;
using System.Collections.Generic;
using TS;
using TypeSerialize;
using UnityEngine;

public sealed class GuidancePart : TutorialBasePart
{
	private Func<GuidancePart, IEnumerator> actionFinish;

	private List<GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo> navigationMessageInfoList;

	private GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo navigationMessageInfo;

	private GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo GetNavigationMessage(GameWebAPI.RespDataMA_NavigationMessageMaster master, int id)
	{
		GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo result = null;
		for (int i = 0; i < master.navigationMessageM.Length; i++)
		{
			if (master.navigationMessageM[i].navigationMessageId == id)
			{
				result = master.navigationMessageM[i];
				break;
			}
		}
		return result;
	}

	private void OnLoadScriptFile(byte[] loadData)
	{
		string empty = string.Empty;
		try
		{
			TypeSerializeHelper.BytesToData<string>(loadData, out empty);
		}
		catch
		{
			global::Debug.Log("動画シーン用のスクリプトファイルの読み込みに失敗しました。");
		}
		this.scriptEngine.Deserialize(empty);
	}

	public void SetNavigationMessage(int[] idList)
	{
		this.navigationMessageInfoList = new List<GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo>();
		if (idList != null)
		{
			GameWebAPI.RespDataMA_NavigationMessageMaster navigationMessageMaster = MasterDataMng.Instance().NavigationMessageMaster;
			for (int i = 0; i < idList.Length; i++)
			{
				GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo navigationMessage = this.GetNavigationMessage(navigationMessageMaster, idList[i]);
				if (navigationMessage != null)
				{
					this.navigationMessageInfoList.Add(navigationMessage);
				}
			}
		}
	}

	public IEnumerator InitializeUI()
	{
		return base.LoadTutorialUI("Tutorial/TutorialUI");
	}

	public IEnumerator InitializeScript(string fileName)
	{
		ScriptEngine scriptEngine = new ScriptEngine();
		this.scriptEngine = scriptEngine;
		string path = "AdventureScene/Text/Guidance/" + fileName;
		TextAsset textAsset = AssetDataMng.Instance().LoadObject(path, null, true) as TextAsset;
		FileControlHelper fileControlHelper = new FileControlHelper();
		return fileControlHelper.Decrypt(textAsset.bytes, new Action<byte[]>(this.OnLoadScriptFile));
	}

	public GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo GetNavigationMessageInfo()
	{
		return this.navigationMessageInfo;
	}

	public List<GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo> GetNavigationMessageInfoList()
	{
		return this.navigationMessageInfoList;
	}

	public void StartGuidance(GameObject observer, GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo messageInfo, Func<GuidancePart, IEnumerator> finished)
	{
		TutorialControlToGame tutorialControlToGame = observer.GetComponent<TutorialControlToGame>();
		if (null == tutorialControlToGame)
		{
			tutorialControlToGame = observer.AddComponent<TutorialControlToGame>();
		}
		this.tutorialCommandAction = new TutorialCommandAction(this.scriptEngine, this.tutorialUI, tutorialControlToGame);
		this.gameEngineController = tutorialControlToGame;
		this.navigationMessageInfo = messageInfo;
		this.actionFinish = finished;
	}

	public override void FinishTutorial(GameObject observerGameObject)
	{
		this.tutorialCommandAction = null;
		this.scriptEngine = null;
		UnityEngine.Object.Destroy(this.tutorialUI.gameObject);
		this.tutorialUI = null;
		UnityEngine.Object.Destroy(observerGameObject.GetComponent<TutorialControlToGame>());
		GUIManager.ExtBackKeyReady = true;
		RestrictionInput.isDisableBackKeySetting = false;
		if (this.actionFinish != null)
		{
			TutorialObserver component = observerGameObject.GetComponent<TutorialObserver>();
			component.StartCoroutine(this.actionFinish(this));
		}
	}
}
