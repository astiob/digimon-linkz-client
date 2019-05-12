using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TutorialObserver : Singleton<TutorialObserver>
{
	[SerializeField]
	private List<string> assetBundleLevelList;

	private int downloadAssetBundleLevelIndex;

	private TutorialBasePart tutorial;

	public string[] assetNameList
	{
		get
		{
			return this.assetBundleLevelList.ToArray();
		}
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void StartFirstTutorial(GameWebAPI.RespDataCM_Login.TutorialStatus tutorialStatus)
	{
		GUIManager.ExtBackKeyReady = false;
		RestrictionInput.isDisableBackKeySetting = true;
		TutorialFirstPart firstTutorial = new TutorialFirstPart();
		firstTutorial.StartTutorial(base.gameObject, tutorialStatus.statusId, delegate
		{
			this.tutorial = firstTutorial;
			if (TipsLoading.Instance.IsShow)
			{
				TipsLoading.Instance.StopTipsLoad(true);
			}
			RestrictionInput.EndLoad();
		});
	}

	public void StartSecondTutorial(string tutorialName, Action completed, Action initialized)
	{
		if (!this.IsFinishTutorial(tutorialName))
		{
			GUIManager.ExtBackKeyReady = false;
			RestrictionInput.isDisableBackKeySetting = true;
			TutorialSecondPart secondTutorial = new TutorialSecondPart();
			Action initialized2 = delegate()
			{
				this.tutorial = secondTutorial;
				if (initialized != null)
				{
					initialized();
				}
			};
			base.StartCoroutine(secondTutorial.StartTutorial(base.gameObject, tutorialName, completed, initialized2));
		}
		else
		{
			if (initialized != null)
			{
				initialized();
			}
			if (completed != null)
			{
				completed();
			}
		}
	}

	public IEnumerator StartGuidance(Action<bool> onStarted)
	{
		GuidancePart guidance = new GuidancePart();
		yield return base.StartCoroutine(GuidanceUserData.RequestNavigation(new Action<int[]>(guidance.SetNavigationMessage)));
		GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo info = GuidanceSelector.Select(guidance.GetNavigationMessageInfoList());
		if (info != null)
		{
			GUIManager.ExtBackKeyReady = false;
			RestrictionInput.isDisableBackKeySetting = true;
			yield return base.StartCoroutine(guidance.InitializeUI());
			yield return base.StartCoroutine(guidance.InitializeScript(info.scriptPath));
			guidance.StartGuidance(base.gameObject, info, new Func<GuidancePart, IEnumerator>(this.EndGuidance));
			this.tutorial = guidance;
		}
		else
		{
			List<GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo> infoList = GuidanceSelector.GetDoneList(guidance.GetNavigationMessageInfoList());
			IEnumerator ie = GuidanceUserData.RequestFinishSave(infoList);
			if (ie != null)
			{
				yield return base.StartCoroutine(ie);
			}
		}
		if (onStarted != null)
		{
			onStarted(null != info);
		}
		yield break;
	}

	private IEnumerator EndGuidance(GuidancePart guidance)
	{
		List<GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo> infoList = GuidanceSelector.GetDoneList(guidance.GetNavigationMessageInfoList());
		infoList.Add(guidance.GetNavigationMessageInfo());
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		yield return base.StartCoroutine(GuidanceUserData.RequestFinishSave(infoList));
		RestrictionInput.EndLoad();
		yield break;
	}

	private void Update()
	{
		if (this.tutorial != null && !this.tutorial.RunScript())
		{
			this.tutorial.FinishTutorial(base.gameObject);
			this.tutorial = null;
		}
	}

	public int DownloaddAssetBundleLevelIndex
	{
		get
		{
			return this.downloadAssetBundleLevelIndex;
		}
		set
		{
			this.downloadAssetBundleLevelIndex = value;
		}
	}

	public string GetAssetBundleLevel()
	{
		string result = string.Empty;
		if (this.assetBundleLevelList != null && this.downloadAssetBundleLevelIndex < this.assetBundleLevelList.Count)
		{
			result = this.assetBundleLevelList[this.downloadAssetBundleLevelIndex];
		}
		return result;
	}

	public string[] GetDownloadedAssetBundleLevels()
	{
		List<string> list = new List<string>();
		if (this.assetBundleLevelList != null && this.downloadAssetBundleLevelIndex < this.assetBundleLevelList.Count)
		{
			for (int i = 0; i < this.downloadAssetBundleLevelIndex; i++)
			{
				list.Add(this.assetBundleLevelList[i]);
			}
		}
		return list.ToArray();
	}

	public bool IsFinishTutorial(string tutorialName)
	{
		return PlayerPrefs.HasKey(tutorialName);
	}
}
