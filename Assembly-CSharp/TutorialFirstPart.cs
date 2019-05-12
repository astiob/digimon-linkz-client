using Master;
using Monster;
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
		bool idOk = false;
		int totalSize = 0;
		int size = 0;
		AssetDataMng.Instance().GetDownloadAssetBundleCount("TUTO1", out size);
		totalSize += size;
		AssetDataMng.Instance().GetDownloadAssetBundleCount("TUTO2", out size);
		totalSize += size;
		AssetDataMng.Instance().GetDownloadAssetBundleCount("TUTO3", out size);
		totalSize += size;
		AssetDataMng.Instance().GetDownloadAssetBundleCount("TUTO4", out size);
		totalSize += size;
		this.DownloadConfirmation(totalSize, delegate
		{
			idOk = true;
		}, false);
		RestrictionInput.EndLoad();
		while (!idOk)
		{
			yield return null;
		}
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		DataMng.Instance().CampaignForceHide = true;
		this.gameEngineController = observer.AddComponent<TutorialControlToGame>();
		yield return null;
		yield return AppCoroutine.Start(base.LoadTutorialUI("Tutorial/TutorialUI"), false);
		yield return AppCoroutine.Start(this.tutorialUI.LoadNonFrameText(), false);
		yield return AppCoroutine.Start(this.tutorialUI.LoadSelectItem(), false);
		UIRoot root = GUIMain.GetUIRoot();
		if (null != root)
		{
			UIPanel component = root.GetComponent<UIPanel>();
			if (null != component)
			{
				component.depth = 100;
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

	protected override string GetFilePath(string fileName)
	{
		return "Tutorial/Text/" + fileName;
	}

	public override void FinishTutorial(GameObject observer)
	{
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
		GUIManager.ExtBackKeyReady = true;
		RestrictionInput.isDisableBackKeySetting = false;
		ClassSingleton<MonsterUserDataMng>.Instance.Initialize();
	}

	private void DownloadConfirmation(int size, Action callback, bool returnConfirmation = false)
	{
		if (size <= 0)
		{
			callback();
			return;
		}
		string[] array = new string[]
		{
			StringMaster.GetString("DownloadSizeKB"),
			StringMaster.GetString("DownloadSizeMB"),
			StringMaster.GetString("DownloadSizeGB")
		};
		ScriptUtil.SIZE_TYPE size_TYPE = ScriptUtil.SIZE_TYPE.KILOBYTE;
		ScriptUtil.ShowCommonDialogForMessage(delegate(int index)
		{
			if (index == 0)
			{
				callback();
			}
			else if (returnConfirmation)
			{
				ScriptUtil.ShowCommonDialog(delegate(int id)
				{
					if (id == 0)
					{
						GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
					}
					else
					{
						this.DownloadConfirmation(size, callback, false);
					}
				}, "BackKeyConfirmTitle", "DownloadSizeExit", "SEInternal/Common/se_106");
			}
			else
			{
				GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
			}
		}, StringMaster.GetString("DownloadSizeTitle"), string.Format(StringMaster.GetString("DownloadSizeInfo"), ScriptUtil.CheckSize(size, ref size_TYPE), array[(int)size_TYPE]), "SEInternal/Common/se_106");
	}
}
