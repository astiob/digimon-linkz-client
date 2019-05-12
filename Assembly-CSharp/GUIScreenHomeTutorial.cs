using Monster;
using System;
using System.Collections;
using UnityEngine;

public class GUIScreenHomeTutorial : GUIScreenHome
{
	public Action actionFinishedLoad;

	public bool isSkipTutorial;

	public override void ShowGUI()
	{
		base.ShowGUI();
		ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.StopGetHistoryIdList();
	}

	protected override void ServerRequest()
	{
		ClassSingleton<MonsterUserDataMng>.Instance.Initialize();
		APIRequestTask task = APIUtil.Instance().RequestTutorialHomeData();
		base.StartCoroutine(task.Run(delegate
		{
			base.StartCoroutine(this.StartEvent());
		}, null, null));
	}

	protected override IEnumerator StartEvent()
	{
		GUIFace.CloseDigiviceChildButtonNotPlaySE();
		GUIFace.CloseFacilityChildButtonNotPlaySE();
		yield return base.StartCoroutine(this.CreateHomeData());
		if (!this.isSkipTutorial)
		{
			TipsLoading.Instance.StopTipsLoad(true);
			Loading.Invisible();
			GUIMain.BarrierOFF();
		}
		GUIManager.ExtBackKeyReady = false;
		RestrictionInput.isDisableBackKeySetting = true;
		base.EnableFarmInput();
		if (this.actionFinishedLoad != null)
		{
			this.actionFinishedLoad();
			this.actionFinishedLoad = null;
		}
		yield break;
	}

	protected override IEnumerator CreateHomeData()
	{
		GUIPlayerStatus.RefreshParams_S(false);
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		yield return base.StartCoroutine(base.CreateFarm());
		FarmRoot farm = FarmRoot.Instance;
		if (null != farm)
		{
			InputControll ic = farm.Input;
			ic.EnableControl = false;
		}
		base.StartCacheBattle();
		base.StartCacheParty();
		while (!AssetDataCacheMng.Instance().IsCacheAllReady())
		{
			yield return null;
		}
		yield break;
	}

	public override void HideGUI()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		UnityEngine.Object.Destroy(this.goFARM_ROOT);
		if (null != this.faceUI)
		{
			UnityEngine.Object.Destroy(this.faceUI);
			this.faceUI = null;
		}
	}
}
