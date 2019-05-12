using Master;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Home;
using UnityEngine;

public sealed class PartsMenu : MonoBehaviour
{
	public static PartsMenu instance;

	private bool trainingAlertState;

	private bool clearAlertState;

	private bool evolutionAlertState;

	[SerializeField]
	private HomeMenuMainParts mainParts;

	[SerializeField]
	private List<PartsMenuButtonData> buttonDataList;

	[SerializeField]
	private GUIBannerPanel bannerPanel;

	[SerializeField]
	private BoxCollider menuButtonCollider;

	[SerializeField]
	private GameObject goMENU_BAR;

	[SerializeField]
	private GameObject goNEW_ICON;

	[SerializeField]
	private UILabel ngTX_DATE;

	private float orgPosZ = -300f;

	private int questUITypeBackup;

	private bool _isShowed;

	private void Awake()
	{
		GUIManager.SetActCallShowDialog(new Action<CommonDialog>(this.SetZPos));
		this.SetBtnXYPos();
		PartsMenu.instance = this;
	}

	private void Start()
	{
		this.ForceHide(false);
	}

	private void Update()
	{
		this.UpdateBackKeyAndroid();
		if (this.mainParts.IsShow())
		{
			this.ngTX_DATE.text = DateTime.Now.ToString(StringMaster.GetString("PartsMenu_txt"));
		}
	}

	private void OnDestroy()
	{
		GUIManager.SetActCallShowDialog(null);
		GUIManager.ExtBackKeyReady = true;
		PartsMenu.instance = null;
	}

	private void SetBtnXYPos()
	{
		if (this.buttonDataList != null)
		{
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < this.buttonDataList.Count; i++)
			{
				list.Add(this.buttonDataList[i].go.transform.localPosition);
				if (i == 7)
				{
					this.buttonDataList[i].state = (1 == ConstValue.IS_CHAT_OPEN);
				}
			}
			int num = 0;
			for (int j = 0; j < this.buttonDataList.Count; j++)
			{
				if (this.buttonDataList[j].state)
				{
					this.buttonDataList[j].go.transform.localPosition = list[num];
					num++;
				}
				else
				{
					this.buttonDataList[j].go.SetActive(false);
				}
			}
		}
	}

	private void SetZPos(CommonDialog cd)
	{
		DepthController component = base.gameObject.GetComponent<DepthController>();
		if (null != cd)
		{
			CMD component2 = cd.gameObject.GetComponent<CMD>();
			if (null != component2 && (null != component2.PartsTitle || component2.requestMenu))
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
		this.HideMainParts(sound);
	}

	public void Active(bool isActive)
	{
		if (isActive)
		{
			base.gameObject.SetActive(true);
			this.SetZPos(GUIManager.GetTopDialogANM(null, false));
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	private void ShowMainParts()
	{
		GUIManager.ExtBackKeyReady = false;
		if (CMD_CharacterDetailed.Instance != null)
		{
			CMD_CharacterDetailed.Instance.TranceEffectActiveSet(false);
		}
		this.mainParts.Open(new Action<int>(this.EndMoved));
		GUICollider.DisableAllCollider("PartsMenu");
		FarmRoot farmRoot = FarmRoot.Instance;
		if (null != farmRoot)
		{
			farmRoot.ClearSettingFarmObject();
		}
		ClassSingleton<FaceMissionAccessor>.Instance.faceMission.EnableCollider(false);
		ClassSingleton<FacePresentAccessor>.Instance.facePresent.EnableCollider(false);
		GUIFace.instance.EnableCollider(false);
		GUIFaceIndicator.instance.EnableCollider(false);
	}

	private void HideMainParts(bool sound)
	{
		ClassSingleton<FaceMissionAccessor>.Instance.faceMission.EnableCollider(true);
		ClassSingleton<FacePresentAccessor>.Instance.facePresent.EnableCollider(true);
		GUIFace.instance.EnableCollider(true);
		GUIFaceIndicator.instance.EnableCollider(true);
		this.mainParts.Close(sound, new Action<int>(this.EndMoved));
		GUICollider.DisableAllCollider("PartsMenu");
	}

	private void EndMoved(int i)
	{
		if (this.mainParts.IsShow())
		{
			this.goMENU_BAR.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
			this.goNEW_ICON.SetActive(false);
			this._isShowed = true;
		}
		else
		{
			this.goMENU_BAR.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
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
		if (this.mainParts.IsShow())
		{
			this.HideMainParts(true);
		}
		else
		{
			this.ShowMainParts();
		}
	}

	public void OnClickedMeal()
	{
		this.ForceHide(true);
		CommonDialog y = GUIManager.CheckTopDialog("CMD_BaseSelect", null);
		if (null == y || CMD_BaseSelect.BaseType != CMD_BaseSelect.BASE_TYPE.MEAL)
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.MEAL;
				this.PartsMenuShowDialog(null, "CMD_BaseSelect");
			});
		}
	}

	public void OnClickedTraining()
	{
		this.ForceHide(true);
		if (null == GUIManager.CheckTopDialog("CMD_ReinforcementTOP", null))
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				this.PartsMenuShowDialog(null, "CMD_ReinforcementTOP");
			});
		}
	}

	public void OnClickedTrainingMenu()
	{
		this.ForceHide(true);
		CommonDialog y = GUIManager.CheckTopDialog("CMD_Training_Menu", null);
		if (null == y)
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				this.PartsMenuShowDialog(null, "CMD_Training_Menu");
			});
		}
	}

	public void OnClickedArousal()
	{
		this.ForceHide(true);
		CommonDialog y = GUIManager.CheckTopDialog("CMD_ArousalTOP", null);
		if (null == y)
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				this.PartsMenuShowDialog(null, "CMD_ArousalTOP");
			});
		}
	}

	public void OnClickedClearing()
	{
		this.ForceHide(true);
		if (FarmRoot.Instance.Scenery.GetFacilityCount(22) > 0)
		{
			if (null == GUIManager.CheckTopDialog("CMD_ClearingHouseTOP", null))
			{
				GUIManager.CloseAllCommonDialog(delegate
				{
					this.PartsMenuShowDialog(null, "CMD_ClearingHouseTOP");
				});
			}
		}
		else
		{
			CMD_ModalMessage cmd_ModalMessage = this.PartsMenuShowDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("ExchangeMissingAlertTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("ExchangeMissingAlertInfo");
		}
	}

	public void OnClickedEvo()
	{
		this.ForceHide(true);
		CommonDialog y = GUIManager.CheckTopDialog("CMD_BaseSelect", null);
		if (null == y || CMD_BaseSelect.BaseType != CMD_BaseSelect.BASE_TYPE.EVOLVE)
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.EVOLVE;
				this.PartsMenuShowDialog(null, "CMD_BaseSelect");
			});
		}
	}

	private void OnClickedParty()
	{
		this.ForceHide(true);
		if (null == GUIManager.CheckTopDialog("CMD_PartyEdit", null))
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
				CMD_PartyEdit.ModeType = CMD_PartyEdit.MODE_TYPE.EDIT;
				this.PartsMenuShowDialog(null, "CMD_PartyEdit");
			});
		}
	}

	public void OnClickedQuest()
	{
		this.OnClickedQuestType(0);
	}

	public void OnClickedQuestType(int uiType)
	{
		this.ForceHide(true);
		this.questUITypeBackup = uiType;
		if (null == GUIManager.CheckTopDialog("CMD_QuestSelect", null))
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
				List<string> worldIdList = new List<string>
				{
					"1",
					"3",
					"8"
				};
				ClassSingleton<QuestData>.Instance.GetWorldDungeonInfo(worldIdList, new Action<bool>(this.OnResponseQuestInfo));
			});
		}
	}

	private void OnResponseQuestInfo(bool isSuccess)
	{
		if (isSuccess)
		{
			this.GetCampaignDataFromServer(delegate
			{
				if (this.questUITypeBackup == 0)
				{
					this.PartsMenuShowDialog(null, "CMD_QuestSelect");
				}
				else if (this.questUITypeBackup == 1)
				{
					RestrictionInput.EndLoad();
					CMD_QuestTOP.AreaData = ClassSingleton<QuestData>.Instance.GetWorldAreaM_NormalByAreaId("8");
					CMD cmd = this.PartsMenuShowDialog(null, "CMD_QuestTOP") as CMD;
					if (null != cmd.PartsTitle)
					{
						cmd.PartsTitle.SetReturnAct(delegate(int idx)
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
		if (null == GUIManager.CheckTopDialog("CMD_PvPTop", null))
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				FarmColosseum.ShowPvPTop();
			});
		}
	}

	public void OnClickedGacha()
	{
		this.ForceHide(true);
		CommonDialog topDialog = GUIManager.GetTopDialog(null, false);
		if (null != topDialog)
		{
			if (topDialog.name == "CMD_MonsterGashaResult" || topDialog.name == "CMD_ChipGashaResult" || topDialog.name == "CMD_TicketGashaResult")
			{
				topDialog.ClosePanel(true);
				return;
			}
			if (topDialog.name == "CMD_GashaTOP")
			{
				return;
			}
		}
		GUIManager.CloseAllCommonDialog(delegate
		{
			this.PartsMenuShowDialog(null, "CMD_GashaTOP");
		});
	}

	private void OnClickedChat()
	{
		this.ForceHide(true);
		if (null == GUIManager.CheckTopDialog("CMD_ChatTop", null))
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				this.PartsMenuShowDialog(null, "CMD_ChatTop");
			});
		}
	}

	private void OnClickedShop()
	{
		this.ForceHide(true);
		if (null == GUIManager.CheckTopDialog("CMD_Shop", null))
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				this.PartsMenuShowDialog(null, "CMD_Shop");
				FarmRoot farmRoot = FarmRoot.Instance;
				if (null != farmRoot)
				{
					farmRoot.ClearSettingFarmObject();
				}
			});
		}
	}

	public void OnClickedFriend()
	{
		this.ForceHide(true);
		if (null == GUIManager.CheckTopDialog("CMD_FriendTop", null))
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				this.PartsMenuShowDialog(null, "CMD_FriendTop");
			});
		}
	}

	private void OnClickedBook()
	{
		this.ForceHide(true);
		if (null == GUIManager.CheckTopDialog("CMD_PictureBookTOP", null))
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				this.PartsMenuShowDialog(null, "CMD_PictureBookTOP");
			});
		}
	}

	private void OnClickedHelp()
	{
		this.ForceHide(true);
		if (null == GUIManager.CheckTopDialog("CMD_HelpCategory", null))
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				CMD_HelpCategory.Data = MasterDataMng.Instance().RespDataMA_HelpCategoryM;
				this.PartsMenuShowDialog(null, "CMD_HelpCategory");
			});
		}
	}

	private void OnClickedSettings()
	{
		this.ForceHide(true);
		if (null == GUIManager.CheckTopDialog("CMD_Setting1", null))
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				this.PartsMenuShowDialog(null, "CMD_Setting1");
			});
		}
	}

	private void OnClickedInfo()
	{
		this.ForceHide(true);
		if (null == GUIManager.CheckTopDialog("CMD_NewsALL", null))
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				this.PartsMenuShowDialog(null, "CMD_NewsALL");
			});
		}
	}

	private void OnClickedProfile()
	{
		this.ForceHide(true);
		if (null == GUIManager.CheckTopDialog("CMD_Profile", null))
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				this.PartsMenuShowDialog(null, "CMD_Profile");
			});
		}
	}

	private void OnClickedOthers()
	{
		this.ForceHide(true);
		if (null == GUIManager.CheckTopDialog("CMD_OtherTOP", null))
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				this.PartsMenuShowDialog(null, "CMD_OtherTOP");
			});
		}
	}

	private void OnClickedClearingHouse()
	{
		this.ForceHide(true);
		if (null == GUIManager.CheckTopDialog("CMD_ClearingHouse", null))
		{
			GUIManager.CloseAllCommonDialog(delegate
			{
				this.PartsMenuShowDialog(null, "CMD_ClearingHouse");
			});
		}
	}

	public void SetMenuBanner()
	{
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -445f;
		listWindowViewRect.xMax = 445f;
		listWindowViewRect.yMin = -310f;
		listWindowViewRect.yMax = 310f;
		this.bannerPanel.ListWindowViewRect = listWindowViewRect;
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
		if (null != PartsMenu.instance)
		{
			PartsMenu.instance.goNEW_ICON.SetActive(ClassSingleton<PartsMenuFriendIconAccessor>.Instance.partsMenuFriendIcon.IsBadgeActive() | PartsMenu.instance.trainingAlertState | PartsMenu.instance.clearAlertState | PartsMenu.instance.evolutionAlertState);
		}
	}

	public static void SetMenuAlertState(bool state)
	{
		if (null != PartsMenu.instance)
		{
			PartsMenu.instance.trainingAlertState = state;
		}
	}

	public static void SetTrainingAlertState(bool state)
	{
		if (null != PartsMenu.instance)
		{
			PartsMenu.instance.trainingAlertState = state;
		}
	}

	public static void SetClearAlertState(bool state)
	{
		if (null != PartsMenu.instance)
		{
			PartsMenu.instance.clearAlertState = state;
		}
	}

	public static void SetEvolutionAlertState(bool state)
	{
		if (null != PartsMenu.instance)
		{
			PartsMenu.instance.evolutionAlertState = state;
		}
	}

	private CommonDialog PartsMenuShowDialog(Action<int> action, string dialogName)
	{
		CommonDialog result = GUIMain.ShowCommonDialog(action, dialogName, null);
		GUIMain.AdjustBarrierZ();
		return result;
	}

	private void UpdateBackKeyAndroid()
	{
		if (!GUICollider.IsAllColliderDisable() && Input.GetKeyDown(KeyCode.Escape) && this._isShowed && this.mainParts.IsShow())
		{
			this.ForceHide(true);
			SoundMng.Instance().PlaySE("SEInternal/Common/se_106", 0f, false, true, null, -1, 1f);
		}
	}
}
