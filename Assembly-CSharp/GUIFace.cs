using Master;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GUIFace : MonoBehaviour
{
	public static GUIFace instance;

	[SerializeField]
	private BoxCollider digiviceCollider;

	[SerializeField]
	private FacilityShopButton facilityShopButton;

	[SerializeField]
	private UISprite facilityStockIcon;

	[SerializeField]
	private UISprite allAlertIcon;

	[SerializeField]
	private EfcCont digiviceSlotBackground;

	[SerializeField]
	private List<GameObject> digiviceChildButtonList;

	[SerializeField]
	private List<GameObject> facilityChildButtonList;

	private bool isShowBtnFacility;

	private static Action eventShowBtnDigivice;

	private Action<CommonDialog> actionPushedGashaButton;

	public static void SetFacilityStockIcon(bool enable)
	{
		if (null != GUIFace.instance)
		{
			GUIFace.instance.facilityStockIcon.gameObject.SetActive(enable);
		}
		GUIFace.SetFacilityAlertIcon();
	}

	public static void SetFacilityAlertIcon()
	{
		if (null != GUIFace.instance)
		{
			GUIFace.instance.SetAllAlertIcon();
		}
	}

	private void SetAllAlertIcon()
	{
		bool flag = false;
		GameWebAPI.RespDataMP_MyPage respDataMP_MyPage = DataMng.Instance().RespDataMP_MyPage;
		if (respDataMP_MyPage != null && respDataMP_MyPage.userNewsCountList != null && (0 < respDataMP_MyPage.userNewsCountList.facilityNewCount || 0 < respDataMP_MyPage.userNewsCountList.decorationNewCount))
		{
			flag = true;
		}
		if (!this.isShowBtnFacility && (flag || this.facilityStockIcon.gameObject.activeSelf))
		{
			this.allAlertIcon.gameObject.SetActive(true);
		}
		else
		{
			this.allAlertIcon.gameObject.SetActive(false);
		}
	}

	private void Awake()
	{
		GUIFace.instance = this;
	}

	private void Start()
	{
		this.InitFacilityBtn();
	}

	public void EnableCollider(bool enable)
	{
		this.digiviceCollider.enabled = enable;
	}

	private void ResetFarmWork()
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		if (null != farmRoot)
		{
			farmRoot.ClearSettingFarmObject();
		}
	}

	private void UIMission()
	{
		this.ResetFarmWork();
		GUIMain.ShowCommonDialog(null, "CMD_Mission", null);
	}

	private void UIPresent()
	{
		this.ShowCommonDialog(null, "CMD_ModalPresentBox");
	}

	private void UIInfo()
	{
		this.ResetFarmWork();
		GUIMain.ShowCommonDialog(null, "CMD_NewsALL", null);
	}

	private void OnPushedDigiviceButton()
	{
		if (!this.digiviceSlotBackground.gameObject.activeSelf)
		{
			this.OpenDigiviceButton();
		}
		else
		{
			this.CloseDigiviceButton(true);
		}
	}

	private void UIFacility()
	{
		this.ShowHideFacilityBtn(true);
	}

	private void UIFarmCombine()
	{
		FarmUI componentInChildren = Singleton<GUIManager>.Instance.GetComponentInChildren<FarmUI>();
		GameObject gameObject = GUIManager.LoadCommonGUI("Farm/EditFooter", componentInChildren.gameObject);
		if (null != gameObject)
		{
			gameObject.name = "FarmEditFooter";
			this.HideGUI();
			if (null != PartsMenu.instance)
			{
				PartsMenu.instance.gameObject.SetActive(false);
			}
		}
		GUIFace.CloseDigiviceChildButton();
		GUIFace.CloseFacilityChildButton();
	}

	private void UIFactory()
	{
		GUIFace.CloseDigiviceChildButton();
		GUIFace.CloseFacilityChildButton();
		this.ShowCommonDialog(null, "CMD_FacilityShop");
	}

	private void UIFacilityStock()
	{
		GUIFace.CloseDigiviceChildButton();
		GUIFace.CloseFacilityChildButton();
		this.ShowCommonDialog(null, "CMD_FacilityStock");
	}

	private void UIQuest()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		List<string> worldIdList = new List<string>
		{
			"1",
			"3",
			"8"
		};
		ClassSingleton<QuestData>.Instance.GetWorldDungeonInfo(worldIdList, new Action<bool>(this.QuestCallback));
	}

	private void QuestCallback(bool isSuccess)
	{
		if (isSuccess)
		{
			this.GetCampaignDataFromServer(delegate
			{
				this.ShowCommonDialog(null, "CMD_QuestSelect");
			});
		}
		else
		{
			RestrictionInput.EndLoad();
		}
	}

	private void GetCampaignDataFromServer(Action complete)
	{
		GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestExpUp);
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestCipUp);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestMatUp);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestRrDrpUp);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestFriUp);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDown);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestExpUpMul);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestCipUpMul);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestMatUpMul);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestRrDrpUpMul);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestFriUpMul);
		}
		if (campaignInfo == null)
		{
			campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDownMul);
		}
		if (campaignInfo == null)
		{
			if (null == base.gameObject || !base.gameObject.activeSelf)
			{
				complete();
				return;
			}
			APIRequestTask task = DataMng.Instance().RequestCampaignAll(true);
			IEnumerator routine = task.Run(complete, null, null);
			base.StartCoroutine(routine);
		}
		else
		{
			complete();
		}
	}

	private void UICapture()
	{
		this.ResetFarmWork();
		GUIMain.ShowCommonDialog(null, "CMD_GashaTOP", this.actionPushedGashaButton);
	}

	public void AddActionPushedGashaButton(Action<CommonDialog> action)
	{
		this.actionPushedGashaButton = (Action<CommonDialog>)Delegate.Combine(this.actionPushedGashaButton, action);
	}

	public void RemoveActionPushedGashaButton(Action<CommonDialog> action)
	{
		this.actionPushedGashaButton = (Action<CommonDialog>)Delegate.Remove(this.actionPushedGashaButton, action);
	}

	private void UIDigiviceTrainingMenu()
	{
		this.ShowCommonDialog(null, "CMD_Training_Menu");
	}

	private void UIDigiviceClearingHouse()
	{
		if (0 < FarmRoot.Instance.Scenery.GetFacilityCount(22))
		{
			this.ShowCommonDialog(null, "CMD_ClearingHouseTOP");
		}
		else
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("ExchangeMissingAlertTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("ExchangeMissingAlertInfo");
		}
	}

	private void UIDigivice_shinka()
	{
		CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.EVOLVE;
		this.ShowCommonDialog(null, "CMD_BaseSelect");
	}

	private void UIDigivice_hensei()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		CMD_PartyEdit.ModeType = CMD_PartyEdit.MODE_TYPE.EDIT;
		GUIMain.ShowCommonDialog(null, "CMD_PartyEdit", null);
	}

	private CommonDialog ShowCommonDialog(Action<int> action, string dialogName)
	{
		this.ResetFarmWork();
		return GUIMain.ShowCommonDialog(action, dialogName, null);
	}

	public static void CloseDigiviceChildButton()
	{
		if (null != GUIFace.instance && GUIFace.instance.digiviceSlotBackground.gameObject.activeSelf)
		{
			GUIFace.instance.CloseDigiviceButton(true);
		}
	}

	public static void CloseDigiviceChildButtonNotPlaySE()
	{
		if (null != GUIFace.instance && GUIFace.instance.digiviceSlotBackground.gameObject.activeSelf)
		{
			GUIFace.instance.CloseDigiviceButton(false);
		}
	}

	public static void OpenDigiviceChildButton()
	{
		if (null != GUIFace.instance && !GUIFace.instance.digiviceSlotBackground.gameObject.activeSelf)
		{
			GUIFace.instance.OpenDigiviceButton();
		}
	}

	public static Action EventShowBtnDigivice
	{
		set
		{
			GUIFace.eventShowBtnDigivice = value;
		}
	}

	private void OpenDigiviceButton()
	{
		Vector2 zero = Vector2.zero;
		Color c = new Color(1f, 1f, 1f, 1f);
		EfcCont component;
		for (int i = 0; i < this.digiviceChildButtonList.Count; i++)
		{
			GameObject gameObject = this.digiviceChildButtonList[i];
			component = gameObject.GetComponent<EfcCont>();
			GUICollider component2 = gameObject.GetComponent<GUICollider>();
			zero.x = component2.GetOriginalPos().x;
			zero.y = component2.GetOriginalPos().y;
			component.MoveTo(zero, 0.2f, null, iTween.EaseType.spring, 0f);
			zero.x = 1f;
			zero.y = 1f;
			component.ScaleTo(zero, 0.2f, null, iTween.EaseType.spring, 0f);
			component.ColorTo(c, 0.2f, null, iTween.EaseType.spring, 0f);
			Transform transform = gameObject.transform.FindChild("Campaign");
			if (transform != null)
			{
				component = transform.GetComponent<EfcCont>();
				component.ScaleTo(zero, 0.2f, null, iTween.EaseType.linear, 0f);
			}
		}
		this.digiviceSlotBackground.gameObject.SetActive(true);
		component = this.digiviceSlotBackground;
		component.ScaleTo(zero, 0.2f, new Action<int>(this.FinishOpenDigiviceButtonAnimation), iTween.EaseType.spring, 0f);
		component.ColorTo(c, 0.2f, null, iTween.EaseType.spring, 0f);
		SoundMng.Instance().TryPlaySE("SEInternal/Farm/se_205", 0f, false, true, null, -1);
		this.ResetFarmWork();
	}

	private void CloseDigiviceButton(bool isPlaySE)
	{
		Vector2 zero = Vector2.zero;
		Color c = new Color(1f, 1f, 1f, 0f);
		EfcCont component;
		for (int i = 0; i < this.digiviceChildButtonList.Count; i++)
		{
			GameObject gameObject = this.digiviceChildButtonList[i];
			component = gameObject.GetComponent<EfcCont>();
			component.MoveTo(zero, 0.2f, null, iTween.EaseType.linear, 0f);
			component.ScaleTo(zero, 0.2f, null, iTween.EaseType.linear, 0f);
			component.ColorTo(c, 0.2f, null, iTween.EaseType.linear, 0f);
			Transform transform = gameObject.transform.FindChild("Campaign");
			if (transform != null)
			{
				component = transform.GetComponent<EfcCont>();
				component.ScaleTo(zero, 0.2f, null, iTween.EaseType.linear, 0f);
			}
		}
		component = this.digiviceSlotBackground;
		component.ScaleTo(zero, 0.2f, new Action<int>(this.FinishCloseDigiviceButtonAnimation), iTween.EaseType.spring, 0f);
		component.ColorTo(c, 0.2f, null, iTween.EaseType.spring, 0f);
		if (isPlaySE)
		{
			SoundMng soundMng = SoundMng.Instance();
			if (soundMng != null)
			{
				soundMng.TryPlaySE("SEInternal/Farm/se_206", 0f, false, true, null, -1);
			}
		}
	}

	private void FinishOpenDigiviceButtonAnimation(int noop)
	{
		if (GUIFace.eventShowBtnDigivice != null)
		{
			GUIFace.eventShowBtnDigivice();
			GUIFace.eventShowBtnDigivice = null;
		}
	}

	private void FinishCloseDigiviceButtonAnimation(int noop)
	{
		this.digiviceSlotBackground.gameObject.SetActive(false);
	}

	private void InitFacilityBtn()
	{
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < this.facilityChildButtonList.Count; i++)
		{
			GameObject gameObject = this.facilityChildButtonList[i];
			zero.z = gameObject.transform.localPosition.z;
			gameObject.transform.localPosition = zero;
			zero.z = 1f;
			gameObject.transform.localScale = zero;
			EfcCont component = gameObject.GetComponent<EfcCont>();
			component.SetColor(new Color(1f, 1f, 1f, 0f));
			gameObject.GetComponent<GUICollider>().activeCollider = false;
		}
		this.isShowBtnFacility = false;
	}

	public static void CloseFacilityChildButton()
	{
		if (null != GUIFace.instance && GUIFace.instance.isShowBtnFacility)
		{
			GUIFace.instance.ShowHideFacilityBtn(true);
		}
	}

	public static void CloseFacilityChildButtonNotPlaySE()
	{
		if (null != GUIFace.instance && GUIFace.instance.isShowBtnFacility)
		{
			GUIFace.instance.ShowHideFacilityBtn(false);
		}
	}

	public static void OpenFacilityChildButton()
	{
		if (null != GUIFace.instance && !GUIFace.instance.isShowBtnFacility)
		{
			GUIFace.instance.ShowHideFacilityBtn(true);
		}
	}

	private void ShowHideFacilityBtn(bool isPlaySE)
	{
		Vector2 zero = Vector2.zero;
		Color c = new Color(1f, 1f, 1f, 0f);
		if (!this.isShowBtnFacility)
		{
			this.isShowBtnFacility = true;
			float time = 0.2f;
			c.a = 1f;
			for (int i = 0; i < this.facilityChildButtonList.Count; i++)
			{
				GameObject gameObject = this.facilityChildButtonList[i];
				EfcCont component = gameObject.GetComponent<EfcCont>();
				GUICollider component2 = gameObject.GetComponent<GUICollider>();
				component2.activeCollider = true;
				zero.x = component2.GetOriginalPos().x;
				zero.y = component2.GetOriginalPos().y;
				component.MoveTo(zero, time, null, iTween.EaseType.spring, 0f);
				zero.x = (zero.y = 1f);
				component.ScaleTo(zero, time, null, iTween.EaseType.spring, 0f);
				component.ColorTo(c, time, null, iTween.EaseType.spring, 0f);
				Transform transform = gameObject.transform.FindChild("Campaign");
				if (null != transform)
				{
					component = transform.GetComponent<EfcCont>();
					component.ScaleTo(zero, time, null, iTween.EaseType.linear, 0f);
				}
			}
			if (isPlaySE)
			{
				SoundMng.Instance().TryPlaySE("SEInternal/Farm/se_205", 0f, false, true, null, -1);
			}
			this.ResetFarmWork();
		}
		else
		{
			this.isShowBtnFacility = false;
			float time2 = 0.2f;
			c.a = 0f;
			for (int j = 0; j < this.facilityChildButtonList.Count; j++)
			{
				GameObject gameObject2 = this.facilityChildButtonList[j];
				EfcCont component = gameObject2.GetComponent<EfcCont>();
				component.MoveTo(zero, time2, null, iTween.EaseType.linear, 0f);
				GUICollider component2 = gameObject2.GetComponent<GUICollider>();
				component2.activeCollider = false;
				component.ScaleTo(Vector2.one, time2, null, iTween.EaseType.linear, 0f);
				component.ColorTo(c, time2, null, iTween.EaseType.linear, 0f);
				Transform transform2 = gameObject2.transform.FindChild("Campaign");
				if (null != transform2)
				{
					component = transform2.GetComponent<EfcCont>();
					component.ScaleTo(zero, time2, null, iTween.EaseType.linear, 0f);
				}
			}
			if (isPlaySE)
			{
				SoundMng soundMng = SoundMng.Instance();
				if (null != soundMng)
				{
					soundMng.TryPlaySE("SEInternal/Farm/se_206", 0f, false, true, null, -1);
				}
			}
		}
		this.SetAllAlertIcon();
	}

	public void ShowGUI()
	{
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			foreach (object obj2 in transform)
			{
				Transform transform2 = (Transform)obj2;
				GUICollider component = transform2.gameObject.GetComponent<GUICollider>();
				if (null != component)
				{
					component.ShowGUI();
				}
			}
		}
		if (base.gameObject.activeSelf)
		{
			Transform transform3 = base.transform;
			Vector3 localPosition = transform3.localPosition;
			localPosition.x = 0f;
			transform3.localPosition = localPosition;
		}
	}

	public void HideGUI()
	{
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			foreach (object obj2 in transform)
			{
				Transform transform2 = (Transform)obj2;
				GUICollider component = transform2.gameObject.GetComponent<GUICollider>();
				if (null != component)
				{
					component.HideGUI();
				}
			}
		}
		if (base.gameObject.activeSelf)
		{
			Transform transform3 = base.transform;
			Vector3 localPosition = transform3.localPosition;
			localPosition.x = 10000f;
			transform3.localPosition = localPosition;
		}
	}

	public static void SetFacilityShopButtonBadge()
	{
		if (null != GUIFace.instance && null != GUIFace.instance.facilityShopButton)
		{
			GameWebAPI.RespDataMP_MyPage respDataMP_MyPage = DataMng.Instance().RespDataMP_MyPage;
			if (respDataMP_MyPage != null && respDataMP_MyPage.userNewsCountList != null)
			{
				if (0 < respDataMP_MyPage.userNewsCountList.facilityNewCount || 0 < respDataMP_MyPage.userNewsCountList.decorationNewCount)
				{
					GUIFace.instance.facilityShopButton.SetBadge(true);
				}
				else
				{
					GUIFace.instance.facilityShopButton.SetBadge(false);
				}
			}
		}
		GUIFace.SetFacilityAlertIcon();
	}
}
