using Neptune.OAuth;
using System;
using System.Collections;
using UnityEngine;

public class CMD_maintenance : CMD
{
	[SerializeField]
	private GameObject goTXT_EXP;

	private UILabel ngTXT_EXP;

	private string info = string.Empty;

	public string Info
	{
		get
		{
			return this.info;
		}
		set
		{
			this.ngTXT_EXP.overflowMethod = UILabel.Overflow.ClampContent;
			this.info = value;
			this.ngTXT_EXP.text = this.info;
		}
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		base.StopAllCoroutines();
		if (null != ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification)
		{
			ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.StopGetHistoryIdList();
		}
		SoundMng.Instance().StopBGM(0.5f, null);
		GUIMain.ResetBGM();
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (tutorialObserver != null)
		{
			UnityEngine.Object.Destroy(tutorialObserver.gameObject);
		}
		TutorialUI tutorialUI = UnityEngine.Object.FindObjectOfType<TutorialUI>();
		if (tutorialUI != null)
		{
			UnityEngine.Object.Destroy(tutorialUI.gameObject);
		}
		AppCoroutine appCoroutine = UnityEngine.Object.FindObjectOfType<AppCoroutine>();
		if (appCoroutine != null)
		{
			UnityEngine.Object.Destroy(appCoroutine.gameObject);
		}
		FarmCameraControlForCMD.ClearRefCT();
		Resources.UnloadUnusedAssets();
		FarmCameraControlForCMD.Off();
		GUIMain.DestroyAllDialog(this);
		GUICollider.InitAllCollider();
		GUIMain.BarrierReset();
		Singleton<GUIManager>.Instance.UseOutsideTouchControl = false;
		FarmRoot farmRoot = UnityEngine.Object.FindObjectOfType<FarmRoot>();
		if (farmRoot != null)
		{
			UnityEngine.Object.Destroy(farmRoot.gameObject);
		}
		GUIFadeControll.StartFadeIn(1f);
		CMDWebWindow.DeleteWebView();
		GUIMain.ReqScreen("UIMaintenance", string.Empty);
	}

	protected override void Awake()
	{
		Time.timeScale = 1f;
		base.Awake();
		this.ngTXT_EXP = this.goTXT_EXP.GetComponent<UILabel>();
		this.enableAndroidBackKey = false;
	}

	public override void ClosePanel(bool animation = true)
	{
		this.CloseAndFarmCamOn(animation);
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	private void OnClickedConnect()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		if (!NpOAuth.Instance.IsInitialized)
		{
			base.StartCoroutine(this.OAuthLogin());
		}
		else
		{
			this.CheckMaintenance();
		}
	}

	private void CheckMaintenance()
	{
		APIRequestTask task = DataMng.Instance().RequestMyPageData(false);
		base.StartCoroutine(task.Run(delegate
		{
			RestrictionInput.EndLoad();
			base.SetCloseAction(delegate(int idx)
			{
				GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
			});
			this.ClosePanel(true);
		}, delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private IEnumerator OAuthLogin()
	{
		string authResult = null;
		Action<string> onCompleted = delegate(string result)
		{
			authResult = result;
		};
		base.StartCoroutine(APIUtil.Instance().OAuthLogin(onCompleted));
		while (string.IsNullOrEmpty(authResult))
		{
			yield return null;
		}
		if ("Success" == authResult)
		{
			this.CheckMaintenance();
		}
		else
		{
			RestrictionInput.EndLoad();
		}
		yield break;
	}

	private void OnInquiryButton()
	{
		GUIMain.ShowCommonDialog(null, "CMD_inquiry");
	}
}
