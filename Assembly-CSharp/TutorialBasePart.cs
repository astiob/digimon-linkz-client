using PersistentData;
using System;
using System.Collections;
using System.Collections.Generic;
using TS;
using TypeSerialize;
using UnityEngine;

public abstract class TutorialBasePart
{
	protected TutorialUI tutorialUI;

	protected TutorialControlToGame gameEngineController;

	protected TutorialCommandAction tutorialCommandAction;

	protected ScriptEngine scriptEngine;

	private void Destroy()
	{
		UnityEngine.Object.Destroy(this.tutorialUI);
		this.tutorialUI = null;
	}

	protected IEnumerator LoadTutorialUI(string resourceName)
	{
		GameObject go = AssetDataMng.Instance().LoadObject(resourceName, null, true) as GameObject;
		yield return null;
		GameObject ui = UnityEngine.Object.Instantiate<GameObject>(go);
		yield return null;
		ui.transform.parent = Singleton<GUIMain>.Instance.transform;
		ui.transform.localScale = Vector3.one;
		ui.transform.localPosition = new Vector3(0f, 0f, -2000f);
		ui.transform.localRotation = Quaternion.identity;
		this.tutorialUI = ui.GetComponent<TutorialUI>();
		go = null;
		Resources.UnloadUnusedAssets();
		List<Coroutine> routine = new List<Coroutine>();
		routine.Add(AppCoroutine.Start(this.tutorialUI.LoadMessageWindow(), false));
		routine.Add(AppCoroutine.Start(this.tutorialUI.LoadImageWindow(), false));
		routine.Add(AppCoroutine.Start(this.tutorialUI.LoadThumbnail(), false));
		for (int i = 0; i < routine.Count; i++)
		{
			yield return routine[i];
		}
		yield break;
	}

	protected IEnumerator LoadScriptFile(string fileName, Action<string> onCompleted)
	{
		string path = "Tutorial/Text/" + fileName;
		TextAsset textAsset = Resources.Load(path) as TextAsset;
		Action<byte[]> onCompleted2 = delegate(byte[] loadData)
		{
			string empty = string.Empty;
			try
			{
				TypeSerializeHelper.BytesToData<string>(loadData, out empty);
			}
			catch
			{
				global::Debug.Log("チュートリアルファイルの読み込みに失敗しました。");
			}
			if (onCompleted != null)
			{
				onCompleted(empty);
			}
		};
		FileControlHelper fileControlHelper = new FileControlHelper();
		return fileControlHelper.Decrypt(textAsset.bytes, onCompleted2);
	}

	public bool RunScript()
	{
		this.scriptEngine.StepCommandScript(new Action<ScriptEngine.Status, int>(this.tutorialCommandAction.ActionScriptCommand));
		return false == this.scriptEngine.IsFinished();
	}

	public abstract void FinishTutorial(GameObject observer);
}
