using Master;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsMenu : MonoBehaviour
{
	public static PartsMenu instance;

	public Vector3 closPos;

	public Vector3 openPos;

	private bool trainingAlertState;

	private bool clearAlertState;

	private bool evolutionAlertState;

	[SerializeField]
	private List<PartsMenuButtonData> buttonDataList;

	[SerializeField]
	private GUIBannerPanel bannerPanel;

	[SerializeField]
	private BoxCollider boxPlate;

	[SerializeField]
	private BoxCollider menuButtonCollider;

	private float orgPosZ = -300f;

	private bool isShow;

	private bool _isShowed;

	public GameObject goMENU_BAR;

	public GameObject goNEW_ICON;

	public GameObject goTX_DATE;

	private UILabel ngTX_DATE;

	public GameObject goROOT;

	private EfcCont ecROOT;

	private int quest_ui_type_bk;

	protected virtual void Awake()
	{
		this.ngTX_DATE = this.goTX_DATE.GetComponent<UILabel>();
		this.ecROOT = this.goROOT.GetComponent<EfcCont>();
		this.ForceHide(false);
		GUIManager.SetActCallShowDialog(new Action<CommonDialog>(this.SetZPos));
		this.SetBtnXYPos();
		PartsMenu.instance = this;
	}

	protected virtual void Update()
	{
		this.UpdateBackKeyAndroid();
		if (this.isShow)
		{
			this.ngTX_DATE.text = DateTime.Now.ToString(StringMaster.GetString("PartsMenu_txt"));
		}
	}

	protected virtual void OnDestroy()
	{
		GUIManager.SetActCallShowDialog(null);
		GUIManager.ExtBackKeyReady = true;
		PartsMenu.instance = null;
	}

	private void SetBtnXYPos()
	{
		if (!false)
		{
			List<Vector3> list = new List<Vector3>();
			if (this.buttonDataList != null)
			{
				for (int i = 0; i < this.buttonDataList.Count; i++)
				{
					Vector3 localPosition = this.buttonDataList[i].go.transform.localPosition;
					list.Add(localPosition);
					int num = i;
					if (num == 7)
					{
						this.buttonDataList[i].state = (ConstValue.IS_CHAT_OPEN == 1);
					}
				}
				int num2 = 0;
				for (int i = 0; i < this.buttonDataList.Count; i++)
				{
					if (this.buttonDataList[i].state)
					{
						this.buttonDataList[i].go.transform.localPosition = list[num2++];
					}
					else
					{
						this.buttonDataList[i].go.SetActive(false);
					}
				}
			}
		}
	}

	private void SetZPos(CommonDialog cd)
	{
		DepthController component = base.gameObject.GetComponent<DepthController>();
		if (cd != null)
		{
			CMD component2 = cd.gameObject.GetComponent<CMD>();
			if (component2 != null && (component2.PartsTitle != null || component2.requestMenu))
			{
				float dlgpitch = GUIManager.GetDLGPitch();
				float z = cd.gameObject.transform.localPosition.z;
				float z2 = base.gameObject.transform.localPosition.z;
				float num = z + dlgpitch / 2f;
				int add = (int)(-(int)num) - (int)(-(int)z2);
				Vector3 localPosition = base.gameObject.transform.localPosition;
				localPosition.z = num;
				base.gameObject.transform.localPosition = localPosition;
				component.AddWidgetDepth(base.transform, add);
			}
		}
		else
		{
			float z3 = base.gameObject.transform.localPosition.z;
			int add2 = (int)(-(int)this.orgPosZ) - (int)(-(int)z3);
			Vector3 localPosition2 = base.gameObject.transform.localPosition;
			localPosition2.z = this.orgPosZ;
			base.gameObject.transform.localPosition = localPosition2;
			component.AddWidgetDepth(base.transform, add2);
		}
	}

	public void ForceHide(bool sound = true)
	{
		this.isShow = true;
		this.ShowHide(sound);
	}

	public bool IsOpenSeq()
	{
		return this.isShow;
	}

	public void Active(bool flg)
	{
		if (flg)
		{
			base.gameObject.SetActive(true);
			CommonDialog topDialogANM = GUIManager.GetTopDialogANM(null, false);
			this.SetZPos(topDialogANM);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	private void ShowHide(bool sound = true)
	{
		float time = 0.2f;
		if (!this.isShow)
		{
			GUIManager.ExtBackKeyReady = false;
			if (CMD_CharacterDetailed.Instance != null)
			{
				CMD_CharacterDetailed.Instance.TranceEffectActiveSet(false);
			}
			this.isShow = true;
			EfcCont efcCont = this.ecROOT;
			Vector2 vP;
			vP.x = this.openPos.x;
			vP.y = this.openPos.y;
			efcCont.MoveTo(vP, time, new Action<int>(this.EndMoved), iTween.EaseType.linear, 0f);
			GUICollider.DisableAllCollider("PartsMenu");
			this.boxPlate.size = new Vector3(1240f, this.boxPlate.size.y, this.boxPlate.size.z);
			FarmRoot farmRoot = FarmRoot.Instance;
			if (null != farmRoot)
			{
				farmRoot.ClearSettingFarmObject();
			}
			ClassSingleton<FaceMissionAccessor>.Instance.faceMission.EnableCollider(false);
			ClassSingleton<FacePresentAccessor>.Instance.facePresent.EnableCollider(false);
			GUIFace.instance.EnableCollider(false);
			GUIFaceIndicator.instance.EnableCollider(false);
			if (sound)
			{
				SoundMng.Instance().TryPlaySE("SEInternal/Common/se_103", 0f, false, true, null, -1);
			}
		}
		else
		{
			this.isShow = false;
			ClassSingleton<FaceMissionAccessor>.Instance.faceMission.EnableCollider(true);
			ClassSingleton<FacePresentAccessor>.Instance.facePresent.EnableCollider(true);
			GUIFace.instance.EnableCollider(true);
			GUIFaceIndicator.instance.EnableCollider(true);
			EfcCont efcCont = this.ecROOT;
			Vector2 vP;
			vP.x = this.closPos.x;
			vP.y = this.closPos.y;
			efcCont.MoveTo(vP, time, new Action<int>(this.EndMoved), iTween.EaseType.linear, 0f);
			GUICollider.DisableAllCollider("PartsMenu");
			this.boxPlate.size = new Vector3(1136f, this.boxPlate.size.y, this.boxPlate.size.z);
			if (sound)
			{
				SoundMng.Instance().TryPlaySE("SEInternal/Common/se_104", 0f, false, true, null, -1);
			}
		}
	}

	private void EndMoved(int i)
	{
		if (this.isShow)
		{
			this.goMENU_BAR.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
			this.goNEW_ICON.SetActive(false);
			this._isShowed = true;
		}
		else
		{
			this.goMENU_BAR.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
			PartsMenu.SetMenuButtonAlertBadge();
			this._isShowed = false;
			GUIManager.ExtBackKeyReady = true;
			if (CMD_CharacterDetailed.Instance != null)
			{
				CMD_CharacterDetailed.Instance.TranceEffectActiveSet(true);
			}
		}
		GUICollider.EnableAllCollider("PartsMenu");
	}

	private void OnTappedTab()
	{
		global::Debug.Log("============================================= PARTS_MENU TAB");
		this.ShowHide(true);
	}

	public void OnClickedMeal()
	{
		this.ForceHide(true);
		CommonDialog x = GUIManager.CheckTopDialog("CMD_BaseSelect", null);
		if (x == null || CMD_BaseSelect.BaseType != CMD_BaseSelect.BASE_TYPE.MEAL)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedMeal));
		}
	}

	private void OnClickedMeal(int i)
	{
		CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.MEAL;
		this.PartsMenuShowDialog(null, "CMD_BaseSelect");
		global::Debug.Log("============================================= PARTS_MENU Meal");
	}

	public void OnClickedTraining()
	{
		this.ForceHide(true);
		if (GUIManager.CheckTopDialog("CMD_ReinforcementTOP", null) == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedTraining));
		}
	}

	private void OnClickedTraining(int i)
	{
		this.PartsMenuShowDialog(null, "CMD_ReinforcementTOP");
		global::Debug.Log("============================================= PARTS_MENU Training");
	}

	public void OnClickedTrainingMenu()
	{
		this.ForceHide(true);
		CommonDialog x = GUIManager.CheckTopDialog("CMD_Training_Menu", null);
		if (x == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedTrainingMenu));
		}
	}

	private void OnClickedTrainingMenu(int i)
	{
		this.PartsMenuShowDialog(null, "CMD_Training_Menu");
		global::Debug.Log("============================================= PARTS_MENU Training_Menu");
	}

	public void OnClickedArousal()
	{
		this.ForceHide(true);
		CommonDialog x = GUIManager.CheckTopDialog("CMD_ArousalTOP", null);
		if (x == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedArousal));
		}
	}

	private void OnClickedArousal(int i)
	{
		this.PartsMenuShowDialog(null, "CMD_ArousalTOP");
		global::Debug.Log("============================================= PARTS_MENU ArousalTOP");
	}

	public void OnClickedClearing()
	{
		this.ForceHide(true);
		if (FarmRoot.Instance.Scenery.GetFacilityCount(22) > 0)
		{
			if (GUIManager.CheckTopDialog("CMD_ClearingHouseTOP", null) == null)
			{
				GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedClearing));
			}
		}
		else
		{
			CMD_ModalMessage cmd_ModalMessage = this.PartsMenuShowDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("ExchangeMissingAlertTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("ExchangeMissingAlertInfo");
		}
	}

	private void OnClickedClearing(int i)
	{
		this.PartsMenuShowDialog(null, "CMD_ClearingHouseTOP");
		global::Debug.Log("============================================= PARTS_MENU Training");
	}

	public void OnClickedEvo()
	{
		this.ForceHide(true);
		CommonDialog x = GUIManager.CheckTopDialog("CMD_BaseSelect", null);
		if (x == null || CMD_BaseSelect.BaseType != CMD_BaseSelect.BASE_TYPE.EVOLVE)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedEvo));
		}
	}

	private void OnClickedEvo(int i)
	{
		CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.EVOLVE;
		this.PartsMenuShowDialog(null, "CMD_BaseSelect");
		global::Debug.Log("============================================= PARTS_MENU Evo");
	}

	private void OnClickedParty()
	{
		this.ForceHide(true);
		if (GUIManager.CheckTopDialog("CMD_PartyEdit", null) == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedParty));
		}
	}

	private void OnClickedParty(int i)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		CMD_PartyEdit.ModeType = CMD_PartyEdit.MODE_TYPE.EDIT;
		this.PartsMenuShowDialog(null, "CMD_PartyEdit");
	}

	public void OnClickedQuest()
	{
		this.OnClickedQuestType(0);
	}

	public void OnClickedQuestType(int quest_ui_type = 0)
	{
		this.ForceHide(true);
		this.quest_ui_type_bk = quest_ui_type;
		if (GUIManager.CheckTopDialog("CMD_QuestSelect", null) == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.__OnClickedQuest));
		}
	}

	private void __OnClickedQuest(int i)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		List<string> list = new List<string>();
		list.Add("1");
		list.Add("3");
		list.Add("8");
		ClassSingleton<QuestData>.Instance.GetWorldDungeonInfo(list, new Action<bool>(this.actCBQuest));
	}

	private void actCBQuest(bool isSuccess)
	{
		if (isSuccess)
		{
			this.GetCampaignDataFromServer(delegate
			{
				if (this.quest_ui_type_bk == 0)
				{
					this.PartsMenuShowDialog(null, "CMD_QuestSelect");
				}
				else if (this.quest_ui_type_bk == 1)
				{
					RestrictionInput.EndLoad();
					CMD_QuestTOP.AreaData = ClassSingleton<QuestData>.Instance.GetWorldAreaM_NormalByAreaId("8");
					CMD cmd = this.PartsMenuShowDialog(null, "CMD_QuestTOP") as CMD;
					PartsTitleBase partsTitle = cmd.PartsTitle;
					if (partsTitle != null)
					{
						partsTitle.SetReturnAct(delegate(int idx)
						{
							cmd.SetCloseAction(delegate(int i)
							{
								this.PartsMenuShowDialog(null, "CMD_QuestSelect");
							});
							cmd.ClosePanel(true);
						});
					}
				}
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
			APIRequestTask task = DataMng.Instance().RequestCampaignAll(true);
			IEnumerator routine = task.Run(complete, null, null);
			base.StartCoroutine(routine);
		}
		else
		{
			complete();
		}
	}

	private void OnClicked_VS()
	{
		this.ForceHide(true);
		if (GUIManager.CheckTopDialog("CMD_PvPTop", null) == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClicked_VS));
		}
	}

	private void OnClicked_VS(int i)
	{
		FarmColosseum.ShowPvPTop();
	}

	public void OnClickedGacha()
	{
		this.ForceHide(true);
		CommonDialog topDialog = GUIManager.GetTopDialog(null, false);
		if (topDialog != null)
		{
			if (topDialog.name == "CMD_10gashaResult" || topDialog.name == "CMD_ChipGashaResult" || topDialog.name == "CMD_TicketGashaResult")
			{
				topDialog.ClosePanel(true);
				return;
			}
			if (topDialog.name == "CMD_GashaTOP")
			{
				return;
			}
		}
		GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedGacha));
	}

	private void OnClickedGacha(int i)
	{
		global::Debug.Log("================================================== OnClickedGacha;");
		this.PartsMenuShowDialog(null, "CMD_GashaTOP");
	}

	private void OnClickedChat()
	{
		this.ForceHide(true);
		if (GUIManager.CheckTopDialog("CMD_ChatTop", null) == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedChat));
		}
	}

	private void OnClickedChat(int i)
	{
		this.PartsMenuShowDialog(null, "CMD_ChatTop");
	}

	private void OnClickedShop()
	{
		this.ForceHide(true);
		if (GUIManager.CheckTopDialog("CMD_Shop", null) == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedShop));
		}
	}

	private void OnClickedShop(int i)
	{
		this.PartsMenuShowDialog(null, "CMD_Shop");
		FarmRoot farmRoot = FarmRoot.Instance;
		if (null != farmRoot)
		{
			farmRoot.ClearSettingFarmObject();
		}
		global::Debug.Log("============================================= PARTS_MENU Shop");
	}

	public void OnClickedFriend()
	{
		this.ForceHide(true);
		if (GUIManager.CheckTopDialog("CMD_FriendTop", null) == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedFriend));
		}
	}

	private void OnClickedFriend(int i)
	{
		global::Debug.Log("============================================= PARTS_MENU Friend");
		this.PartsMenuShowDialog(null, "CMD_FriendTop");
	}

	private void OnClickedBook()
	{
		this.ForceHide(true);
		if (GUIManager.CheckTopDialog("CMD_PictureBookTOP", null) == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedBook));
		}
	}

	private void OnClickedBook(int i)
	{
		this.PartsMenuShowDialog(null, "CMD_PictureBookTOP");
	}

	private void OnClickedHelp()
	{
		this.ForceHide(true);
		if (GUIManager.CheckTopDialog("CMD_HelpCategory", null) == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OpenHelpUI));
		}
	}

	private void OpenHelpUI(int i)
	{
		CMD_HelpCategory.Data = MasterDataMng.Instance().RespDataMA_HelpCategoryM;
		this.PartsMenuShowDialog(null, "CMD_HelpCategory");
	}

	private void OnClickedSettings()
	{
		this.ForceHide(true);
		if (GUIManager.CheckTopDialog("CMD_Setting1", null) == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedSettings));
		}
	}

	private void OnClickedSettings(int i)
	{
		this.PartsMenuShowDialog(null, "CMD_Setting1");
	}

	private void OnClickedInfo()
	{
		this.ForceHide(true);
		if (GUIManager.CheckTopDialog("CMD_NewsALL", null) == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedInfo));
		}
	}

	private void OnClickedInfo(int i)
	{
		this.PartsMenuShowDialog(null, "CMD_NewsALL");
	}

	private void OnClickedProfile()
	{
		this.ForceHide(true);
		if (GUIManager.CheckTopDialog("CMD_Profile", null) == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedProfile));
		}
	}

	private void OnClickedProfile(int i)
	{
		this.PartsMenuShowDialog(null, "CMD_Profile");
	}

	private void OnClickedOthers()
	{
		this.ForceHide(true);
		if (GUIManager.CheckTopDialog("CMD_OtherTOP", null) == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedOthers));
		}
	}

	private void OnClickedOthers(int i)
	{
		this.PartsMenuShowDialog(null, "CMD_OtherTOP");
	}

	private void OnClickedClearingHouse()
	{
		this.ForceHide(true);
		if (GUIManager.CheckTopDialog("CMD_ClearingHouse", null) == null)
		{
			GUIManager.CloseAllCommonDialog(new Action<int>(this.OnClickedClearingHouse));
		}
	}

	private void OnClickedClearingHouse(int i)
	{
		this.PartsMenuShowDialog(null, "CMD_ClearingHouse");
	}

	private void SetCommonUI()
	{
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -445f;
		listWindowViewRect.xMax = 445f;
		listWindowViewRect.yMin = -310f;
		listWindowViewRect.yMax = 310f;
		this.bannerPanel.ListWindowViewRect = listWindowViewRect;
	}

	public void SetMenuBanner()
	{
		this.SetCommonUI();
		base.StartCoroutine(this.bannerPanel.AllBuild(GUIBannerPanel.Data));
	}

	public void RefreshMenuBannerNewAlert()
	{
		this.bannerPanel.RefreshNewAlert();
	}

	public void SetEnableMenuButton(bool active)
	{
		this.menuButtonCollider.enabled = active;
	}

	public static void SetMenuButtonAlertBadge()
	{
		if (PartsMenu.instance != null)
		{
			bool active = ClassSingleton<PartsMenuFriendIconAccessor>.Instance.partsMenuFriendIcon.IsBadgeActive() | PartsMenu.instance.trainingAlertState | PartsMenu.instance.clearAlertState | PartsMenu.instance.evolutionAlertState;
			PartsMenu.instance.goNEW_ICON.SetActive(active);
		}
	}

	public static void SetMenuAlertState(bool state)
	{
		if (PartsMenu.instance != null)
		{
			PartsMenu.instance.trainingAlertState = state;
		}
	}

	public static void SetTrainingAlertState(bool state)
	{
		if (PartsMenu.instance != null)
		{
			PartsMenu.instance.trainingAlertState = state;
		}
	}

	public static void SetClearAlertState(bool state)
	{
		if (PartsMenu.instance != null)
		{
			PartsMenu.instance.clearAlertState = state;
		}
	}

	public static void SetEvolutionAlertState(bool state)
	{
		if (PartsMenu.instance != null)
		{
			PartsMenu.instance.evolutionAlertState = state;
		}
	}

	private CommonDialog PartsMenuShowDialog(Action<int> action, string dialogName)
	{
		CommonDialog result = GUIManager.ShowCommonDialog(action, true, dialogName, null, null, 0.2f, 0f, 0f, -1f, -1f);
		GUIMain.AdjustBarrierZ();
		return result;
	}

	private void UpdateBackKeyAndroid()
	{
		if (!GUICollider.IsAllColliderDisable() && Input.GetKeyDown(KeyCode.Escape) && this._isShowed && this.isShow)
		{
			this.ForceHide(true);
			SoundMng.Instance().PlaySE("SEInternal/Common/se_106", 0f, false, true, null, -1, 1f);
		}
	}
}
