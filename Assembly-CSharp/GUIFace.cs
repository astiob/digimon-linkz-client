using Master;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GUIFace : GUIBase
{
	public static GUIFace instance;

	[SerializeField]
	private BoxCollider digiviceCollider;

	[SerializeField]
	private FacilityShopButton facilityShopButton;

	[SerializeField]
	private UISprite facilityShopIcon;

	[SerializeField]
	private UISprite facilityStockIcon;

	[SerializeField]
	private UISprite allAlertIcon;

	private static Transform faceLocator;

	private static Vector3 faceLocatorOrigin;

	private static Vector3 faceLocatorHideAnimOfs = new Vector3(0f, -200f, 0f);

	private EffectBase slideEffectBase_ = new EffectBase();

	[FormerlySerializedAs("goRootBtnWise")]
	public GameObject goRootBtnDigivice;

	[FormerlySerializedAs("goBtnWiseList")]
	public List<GameObject> goBtnDigiviceList;

	[FormerlySerializedAs("goBtnWiseBase")]
	public GameObject goBtnDigiviceBase;

	[SerializeField]
	private List<UIHeader> goHeaderBtnList;

	private bool isShowBtnDigivice;

	[FormerlySerializedAs("goBtnFacilityList")]
	public List<GameObject> goBtnFacilityList;

	private bool isShowBtnFacility;

	private static Action eventShowBtnDigivice;

	private static Action eventShowBtnFacility;

	public static void SetFacilityStockIcon(bool flg)
	{
		if (GUIFace.instance != null)
		{
			GUIFace.instance.facilityStockIcon.gameObject.SetActive(flg);
		}
		GUIFace.SetFacilityAlertIcon();
	}

	public static void SetFacilityAlertIcon()
	{
		if (GUIFace.instance != null)
		{
			GUIFace.instance.SetAllAlertIcon();
		}
	}

	private void SetAllAlertIcon()
	{
		bool flag = false;
		GameWebAPI.RespDataMP_MyPage respDataMP_MyPage = DataMng.Instance().RespDataMP_MyPage;
		if (respDataMP_MyPage != null && respDataMP_MyPage.userNewsCountList != null && (respDataMP_MyPage.userNewsCountList.facilityNewCount > 0 || respDataMP_MyPage.userNewsCountList.decorationNewCount > 0))
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

	protected override void Start()
	{
		base.Start();
		this.InitDigiviceBtn();
		this.InitFacilityBtn();
	}

	public void EnableCollider(bool isEnable)
	{
		this.digiviceCollider.enabled = isEnable;
	}

	protected override void Awake()
	{
		GUIFace.instance = this;
		foreach (object obj in base.transform)
		{
			Transform tran = (Transform)obj;
			this.InitChild(tran);
		}
	}

	private void SetUpStartEfc(Vector3 start, Vector3 end)
	{
		EFFECT_BASE_KEY_FRAME[] tagSlideKeys = EffectKeyFrame.GetTagSlideKeys2(start, end);
		this.slideEffectBase_.efSetKeyFrameTbl(tagSlideKeys);
		this.slideEffectBase_.efSetLoopCt(1);
		this.slideEffectBase_.efInit();
		this.slideEffectBase_.efStop();
		this.slideEffectBase_.efStart();
	}

	public static void ShowLocatorAnim()
	{
		GUIFace.instance.SetUpStartEfc(GUIFace.faceLocatorOrigin + GUIFace.faceLocatorHideAnimOfs, GUIFace.faceLocatorOrigin);
	}

	public static void HideLocatorAnim()
	{
		GUIFace.instance.SetUpStartEfc(GUIFace.faceLocatorOrigin, GUIFace.faceLocatorOrigin + GUIFace.faceLocatorHideAnimOfs);
	}

	protected override void Update()
	{
		base.Update();
		if (!this.slideEffectBase_.efIsEnd())
		{
			this.slideEffectBase_.efTransform(base.gameObject.transform);
			this.slideEffectBase_.efUpdate();
		}
	}

	public virtual void InitChild(Transform tran)
	{
		foreach (object obj2 in tran)
		{
			Transform tran2 = (Transform)obj2;
			this.InitChild(tran2);
		}
		if (tran.name == "FaceLocator")
		{
			GUIFace.faceLocator = tran;
			GUIFace.faceLocatorOrigin = new Vector3(0f, 0f, 0f);
			GUIFace.faceLocatorOrigin = GUIFace.faceLocator.localPosition;
		}
		else
		{
			GUICollider obj = tran.gameObject.GetComponent<GUICollider>();
			if (obj != null)
			{
				obj.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
				{
					if (obj.pullDownVal >= 0 || flag)
					{
						obj.pullDownVal = -1;
					}
				};
			}
		}
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
		GUIMain.ShowCommonDialog(null, "CMD_Mission");
	}

	private void UIPresent()
	{
		global::Debug.Log("================================================== UIPresent;");
		this.ShowCommonDialog(null, "CMD_ModalPresentBox");
	}

	private void UIInfo()
	{
		this.ResetFarmWork();
		GUIMain.ShowCommonDialog(null, "CMD_NewsALL");
	}

	private void UIDigivice()
	{
		global::Debug.Log("================================================== UIDigivice;");
		this.ShowHideDigiviceBtn();
	}

	private void UIFacility()
	{
		global::Debug.Log("================================================== UIFacility;");
		this.ShowHideFacilityBtn();
	}

	private void UIFarmCombine()
	{
		global::Debug.Log("================================================== UIFarmCombine;");
		FarmUI componentInChildren = Singleton<GUIManager>.Instance.GetComponentInChildren<FarmUI>();
		GameObject gameObject = GUIManager.LoadCommonGUI("Farm/EditFooter", componentInChildren.gameObject);
		if (null != gameObject)
		{
			this.FoterHeaterEnable(false);
			gameObject.name = "FarmEditFooter";
			GUIFace.instance.HideGUI();
		}
		GUIFace.ForceHideDigiviceBtn_S();
	}

	private void UIFactory()
	{
		GUIFace.ForceHideDigiviceBtn_S();
		this.ShowCommonDialog(null, "CMD_FacilityShop");
	}

	private void UIFacilityStock()
	{
		GUIFace.ForceHideDigiviceBtn_S();
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
			if (base.gameObject == null || !base.gameObject.activeSelf)
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
		global::Debug.Log("================================================== UICapture;");
		this.ResetFarmWork();
		GUIMain.ShowCommonDialog(null, "CMD_GashaTOP");
	}

	private void UIDigiviceTrainingMenu()
	{
		this.ShowCommonDialog(null, "CMD_Training_Menu");
	}

	private void UIDigiviceClearingHouse()
	{
		if (FarmRoot.Instance.Scenery.GetFacilityCount(22) > 0)
		{
			this.ShowCommonDialog(null, "CMD_ClearingHouseTOP");
		}
		else
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
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
		global::Debug.Log("================================================== UIDigivice_hensei;");
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		CMD_PartyEdit.ModeType = CMD_PartyEdit.MODE_TYPE.EDIT;
		GUIMain.ShowCommonDialog(null, "CMD_PartyEdit");
	}

	private CommonDialog ShowCommonDialog(Action<int> action, string dialogName)
	{
		this.ResetFarmWork();
		return GUIMain.ShowCommonDialog(action, dialogName);
	}

	private void InitDigiviceBtn()
	{
		Vector3 zero = Vector3.zero;
		EfcCont component;
		for (int i = 0; i < this.goBtnDigiviceList.Count; i++)
		{
			zero.z = this.goBtnDigiviceList[i].transform.localPosition.z;
			this.goBtnDigiviceList[i].transform.localPosition = zero;
			zero.z = 1f;
			this.goBtnDigiviceList[i].transform.localScale = zero;
			component = this.goBtnDigiviceList[i].GetComponent<EfcCont>();
			component.SetColor(new Color(1f, 1f, 1f, 0f));
		}
		this.goBtnDigiviceBase.transform.localScale = Vector3.zero;
		component = this.goBtnDigiviceBase.GetComponent<EfcCont>();
		component.SetColor(new Color(1f, 1f, 1f, 0f));
		this.isShowBtnDigivice = false;
	}

	public static void ForceHideDigiviceBtn_S()
	{
		if (GUIFace.instance != null && GUIFace.instance.isShowBtnDigivice)
		{
			GUIFace.instance.ShowHideDigiviceBtn();
		}
		GUIFace.ForceHideFacilityBtn_S();
	}

	public static void ForceShowDigiviceBtn_S()
	{
		if (GUIFace.instance != null && !GUIFace.instance.isShowBtnDigivice)
		{
			GUIFace.instance.ShowHideDigiviceBtn();
		}
	}

	public static Action EventShowBtnDigivice
	{
		get
		{
			return GUIFace.eventShowBtnDigivice;
		}
		set
		{
			GUIFace.eventShowBtnDigivice = value;
		}
	}

	private void ShowHideDigiviceBtn()
	{
		Vector2 zero = Vector2.zero;
		Color c = new Color(1f, 1f, 1f, 0f);
		if (!this.isShowBtnDigivice)
		{
			this.isShowBtnDigivice = true;
			float time = 0.2f;
			c.a = 1f;
			EfcCont component;
			for (int i = 0; i < this.goBtnDigiviceList.Count; i++)
			{
				component = this.goBtnDigiviceList[i].GetComponent<EfcCont>();
				GUICollider component2 = this.goBtnDigiviceList[i].GetComponent<GUICollider>();
				zero.x = component2.GetOriginalPos().x;
				zero.y = component2.GetOriginalPos().y;
				component.MoveTo(zero, time, null, iTween.EaseType.spring, 0f);
				zero.x = (zero.y = 1f);
				component.ScaleTo(zero, time, null, iTween.EaseType.spring, 0f);
				component.ColorTo(c, time, null, iTween.EaseType.spring, 0f);
				Transform transform = this.goBtnDigiviceList[i].transform.FindChild("Campaign");
				if (transform != null)
				{
					component = transform.GetComponent<EfcCont>();
					component.ScaleTo(zero, time, null, iTween.EaseType.linear, 0f);
				}
			}
			component = this.goBtnDigiviceBase.GetComponent<EfcCont>();
			component.ScaleTo(zero, time, new Action<int>(this.actEndShowBtnDigivice), iTween.EaseType.spring, 0f);
			component.ColorTo(c, time, null, iTween.EaseType.spring, 0f);
			SoundMng.Instance().TryPlaySE("SEInternal/Farm/se_205", 0f, false, true, null, -1);
			this.ResetFarmWork();
		}
		else
		{
			this.isShowBtnDigivice = false;
			float time2 = 0.2f;
			c.a = 0f;
			EfcCont component;
			for (int i = 0; i < this.goBtnDigiviceList.Count; i++)
			{
				component = this.goBtnDigiviceList[i].GetComponent<EfcCont>();
				component.MoveTo(zero, time2, null, iTween.EaseType.linear, 0f);
				component.ScaleTo(zero, time2, null, iTween.EaseType.linear, 0f);
				component.ColorTo(c, time2, null, iTween.EaseType.linear, 0f);
				Transform transform2 = this.goBtnDigiviceList[i].transform.FindChild("Campaign");
				if (transform2 != null)
				{
					component = transform2.GetComponent<EfcCont>();
					component.ScaleTo(zero, time2, null, iTween.EaseType.linear, 0f);
				}
			}
			component = this.goBtnDigiviceBase.GetComponent<EfcCont>();
			component.ScaleTo(zero, time2, null, iTween.EaseType.spring, 0f);
			component.ColorTo(c, time2, null, iTween.EaseType.spring, 0f);
			SoundMng soundMng = SoundMng.Instance();
			if (soundMng != null)
			{
				soundMng.TryPlaySE("SEInternal/Farm/se_206", 0f, false, true, null, -1);
			}
		}
	}

	private void actEndShowBtnDigivice(int i)
	{
		if (GUIFace.eventShowBtnDigivice != null)
		{
			GUIFace.eventShowBtnDigivice();
			GUIFace.eventShowBtnDigivice = null;
		}
	}

	private void InitFacilityBtn()
	{
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < this.goBtnFacilityList.Count; i++)
		{
			zero.z = this.goBtnFacilityList[i].transform.localPosition.z;
			this.goBtnFacilityList[i].transform.localPosition = zero;
			zero.z = 1f;
			this.goBtnFacilityList[i].transform.localScale = zero;
			EfcCont component = this.goBtnFacilityList[i].GetComponent<EfcCont>();
			this.goBtnFacilityList[i].GetComponent<GUICollider>().activeCollider = false;
			component.SetColor(new Color(1f, 1f, 1f, 0f));
		}
		this.isShowBtnFacility = false;
	}

	public static void ForceHideFacilityBtn_S()
	{
		if (GUIFace.instance != null && GUIFace.instance.isShowBtnFacility)
		{
			GUIFace.instance.ShowHideFacilityBtn();
		}
	}

	public static void ForceShowFacilityBtn_S()
	{
		if (GUIFace.instance != null && !GUIFace.instance.isShowBtnFacility)
		{
			GUIFace.instance.ShowHideFacilityBtn();
		}
	}

	public static Action EventShowBtnFacility
	{
		get
		{
			return GUIFace.eventShowBtnFacility;
		}
		set
		{
			GUIFace.eventShowBtnFacility = value;
		}
	}

	private void ShowHideFacilityBtn()
	{
		Vector2 zero = Vector2.zero;
		Color c = new Color(1f, 1f, 1f, 0f);
		if (!this.isShowBtnFacility)
		{
			this.isShowBtnFacility = true;
			float time = 0.2f;
			c.a = 1f;
			for (int i = 0; i < this.goBtnFacilityList.Count; i++)
			{
				EfcCont component = this.goBtnFacilityList[i].GetComponent<EfcCont>();
				GUICollider component2 = this.goBtnFacilityList[i].GetComponent<GUICollider>();
				component2.activeCollider = true;
				zero.x = component2.GetOriginalPos().x;
				zero.y = component2.GetOriginalPos().y;
				component.MoveTo(zero, time, null, iTween.EaseType.spring, 0f);
				zero.x = (zero.y = 1f);
				component.ScaleTo(zero, time, null, iTween.EaseType.spring, 0f);
				component.ColorTo(c, time, null, iTween.EaseType.spring, 0f);
				Transform transform = this.goBtnFacilityList[i].transform.FindChild("Campaign");
				if (transform != null)
				{
					component = transform.GetComponent<EfcCont>();
					component.ScaleTo(zero, time, null, iTween.EaseType.linear, 0f);
				}
			}
			SoundMng.Instance().TryPlaySE("SEInternal/Farm/se_205", 0f, false, true, null, -1);
			this.ResetFarmWork();
		}
		else
		{
			this.isShowBtnFacility = false;
			float time2 = 0.2f;
			c.a = 0f;
			for (int i = 0; i < this.goBtnFacilityList.Count; i++)
			{
				EfcCont component = this.goBtnFacilityList[i].GetComponent<EfcCont>();
				component.MoveTo(zero, time2, null, iTween.EaseType.linear, 0f);
				GUICollider component2 = this.goBtnFacilityList[i].GetComponent<GUICollider>();
				component2.activeCollider = false;
				component.ScaleTo(Vector2.one, time2, null, iTween.EaseType.linear, 0f);
				component.ColorTo(c, time2, null, iTween.EaseType.linear, 0f);
				Transform transform2 = this.goBtnFacilityList[i].transform.FindChild("Campaign");
				if (transform2 != null)
				{
					component = transform2.GetComponent<EfcCont>();
					component.ScaleTo(zero, time2, null, iTween.EaseType.linear, 0f);
				}
			}
			SoundMng soundMng = SoundMng.Instance();
			if (soundMng != null)
			{
				soundMng.TryPlaySE("SEInternal/Farm/se_206", 0f, false, true, null, -1);
			}
		}
		this.SetAllAlertIcon();
	}

	private void actEndShowBtnFacility(int i)
	{
		if (GUIFace.eventShowBtnFacility != null)
		{
			GUIFace.eventShowBtnFacility();
			GUIFace.eventShowBtnFacility = null;
		}
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name == "FaceLocator")
			{
				foreach (object obj2 in transform)
				{
					Transform transform2 = (Transform)obj2;
					GUICollider component = transform2.gameObject.GetComponent<GUICollider>();
					if (component != null)
					{
						component.ShowGUI();
					}
				}
			}
		}
		GUIBase gui = GUIManager.GetGUI("UIHome");
		if (null != gui)
		{
			GUIScreenHome component2 = gui.GetComponent<GUIScreenHome>();
			if (null != component2)
			{
				component2.SetActiveOfPartsMenu(true);
			}
		}
		this.FoterHeaterEnable(true);
	}

	private void FoterHeaterEnable(bool ena)
	{
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name == "FaceLocator")
			{
				foreach (object obj2 in transform)
				{
					Transform transform2 = (Transform)obj2;
					GUICollider component = transform2.gameObject.GetComponent<GUICollider>();
					if (component != null)
					{
						if (component.GetComponent<UIHeader>())
						{
							component.GetComponent<UIHeader>().enabled = ena;
						}
						else if (component.GetComponent<UIFooter>())
						{
							component.GetComponent<UIFooter>().enabled = ena;
						}
					}
				}
			}
		}
	}

	public override void HideGUI()
	{
		base.HideGUI();
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name == "FaceLocator")
			{
				foreach (object obj2 in transform)
				{
					Transform transform2 = (Transform)obj2;
					GUICollider component = transform2.gameObject.GetComponent<GUICollider>();
					if (component != null)
					{
						component.HideGUI();
					}
				}
			}
		}
		GUIBase gui = GUIManager.GetGUI("UIHome");
		if (null != gui)
		{
			GUIScreenHome component2 = gui.GetComponent<GUIScreenHome>();
			if (null != component2)
			{
				component2.SetActiveOfPartsMenu(false);
			}
		}
	}

	public void HideHeaderBtns()
	{
		foreach (UIHeader uiheader in this.goHeaderBtnList)
		{
			uiheader.enabled = false;
			Vector3 localPosition = new Vector3(0f, -2000f, 0f);
			uiheader.gameObject.transform.localPosition = localPosition;
		}
	}

	public static void ShowLocator()
	{
		GUIFace.faceLocator.localPosition = GUIFace.faceLocatorOrigin;
	}

	public static void HideLocator()
	{
		Vector3 localPosition = GUIFace.faceLocator.localPosition;
		localPosition.z += 2000f;
		GUIFace.faceLocator.localPosition = localPosition;
	}

	public static void SetFacilityShopButtonBadge()
	{
		if (null != GUIFace.instance && null != GUIFace.instance.facilityShopButton)
		{
			GameWebAPI.RespDataMP_MyPage respDataMP_MyPage = DataMng.Instance().RespDataMP_MyPage;
			if (respDataMP_MyPage != null && respDataMP_MyPage.userNewsCountList != null)
			{
				if (respDataMP_MyPage.userNewsCountList.facilityNewCount > 0 || respDataMP_MyPage.userNewsCountList.decorationNewCount > 0)
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
