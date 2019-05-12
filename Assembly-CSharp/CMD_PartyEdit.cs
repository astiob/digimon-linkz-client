using Master;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_PartyEdit : CMD
{
	[SerializeField]
	private PartyBossIcons partyBossIcons;

	[SerializeField]
	private GameObject goTXT_LEADERSKILL;

	[SerializeField]
	private GameObject goTXT_LEADERSKILL_EXP;

	[SerializeField]
	private GameObject goTXT_PARTY_NUM;

	[SerializeField]
	private GameObject goListParts;

	[SerializeField]
	private GameObject goTX_DECIDE;

	[SerializeField]
	private GameObject goBTN_FAVO;

	[SerializeField]
	private GameObject goTXT_FAVO;

	[SerializeField]
	private GameObject goSaveButton;

	[SerializeField]
	private GameObject goChangeStatusButton;

	[SerializeField]
	private UISprite spLeaderSkillTitleBase;

	[SerializeField]
	private UISprite spLeaderSkillDescriptionBase;

	[Header("クリッピングしているオブジェクト達")]
	[SerializeField]
	private GameObject[] clipObjects;

	private UILabel gsfTXT_LEADERSKILL;

	private UILabel gsfTXT_LEADERSKILL_EXP;

	private GameStringsFont gsfTXT_PARTY_NUM;

	private UISprite spBTN_FAVO;

	private UILabel lbTXT_FAVO;

	private GUICollider colBTN_FAVO;

	private string favoriteDeckNum_Now;

	private string favoriteDeckNum_Org;

	private GameObject goSelectPanel;

	private GUISelectPanelPartyEdit csSelectPanel;

	private GameWebAPI.MN_Req_EditDeckList req_editdecklist_bk;

	private Vector3 v3DPos = new Vector3(0f, 4000f, 0f);

	private float xPos = 1000f;

	private List<GameWebAPI.RespDataMN_GetDeckList.DeckList> dataList;

	public static CMD_PartyEdit instance;

	private bool isBattleClicked;

	private int backUpIdx = -1;

	public int idxNumber = -1;

	private Color colFavoOff = new Color(0.227450982f, 0.227450982f, 0.227450982f, 1f);

	public GameObject goEFC_CTR_2;

	public static CMD_PartyEdit.MODE_TYPE ModeType { private get; set; }

	public static int SelectedParty { get; set; }

	protected override void Awake()
	{
		CMD_PartyEdit.instance = this;
		base.Awake();
		this.gsfTXT_LEADERSKILL = this.goTXT_LEADERSKILL.GetComponent<UILabel>();
		this.gsfTXT_LEADERSKILL_EXP = this.goTXT_LEADERSKILL_EXP.GetComponent<UILabel>();
		this.gsfTXT_PARTY_NUM = this.goTXT_PARTY_NUM.GetComponent<GameStringsFont>();
		this.InitFavoriteBtn();
		this.InitDesideBtn();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_PartyEdit");
		Vector3 localPosition = base.gameObject.transform.localPosition;
		localPosition.x = 10000f;
		base.gameObject.transform.localPosition = localPosition;
		if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.SELECT || CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.MULTI)
		{
			this.partyBossIcons.SetBossIconAndTolerances();
		}
		else
		{
			UnityEngine.Object.Destroy(this.partyBossIcons.gameObject);
		}
		base.StartCoroutine(this.ShowAll(f, sizeX, sizeY, aT));
	}

	private IEnumerator ShowAll(Action<int> f, float sizeX, float sizeY, float aT)
	{
		while (!AssetDataCacheMng.Instance().IsCacheAllReadyType(AssetDataCacheMng.CACHE_TYPE.CHARA_PARTY))
		{
			yield return null;
		}
		base.PartsTitle.SetTitle(StringMaster.GetString("PartyTitleEdit"));
		this.SetCommonUI();
		this.RefreshPartyNumText();
		Vector3 v3 = base.gameObject.transform.localPosition;
		v3.x = 0f;
		base.gameObject.transform.localPosition = v3;
		base.Show(f, sizeX, sizeY, aT);
		this.RefreshLeaderSkillText();
		if (Loading.IsShow())
		{
			RestrictionInput.EndLoad();
		}
		if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.EDIT)
		{
			this.SaveButtonActive(false);
		}
		yield break;
	}

	protected override void Update()
	{
		base.Update();
		this.RefreshPartyNumText();
		this.BtnFavoriteControl();
	}

	protected override void OnDestroy()
	{
		if ((CMD_PartyEdit.ModeType != CMD_PartyEdit.MODE_TYPE.SELECT && CMD_PartyEdit.ModeType != CMD_PartyEdit.MODE_TYPE.PVP && CMD_PartyEdit.ModeType != CMD_PartyEdit.MODE_TYPE.MULTI) || CMD_PartyEdit.SelectedParty <= 0)
		{
			FarmRoot farmRoot = FarmRoot.Instance;
			if (farmRoot != null && farmRoot.DigimonManager != null)
			{
				farmRoot.DigimonManager.RefreshDigimonGameObject(false, null);
			}
		}
		base.OnDestroy();
		CMD_PartyEdit.instance = null;
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.EDIT)
		{
			TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
			if (tutorialObserver != null)
			{
				GUIMain.BarrierON(null);
				tutorialObserver.StartSecondTutorial("second_tutorial_partyedit", new Action(GUIMain.BarrierOFF), delegate
				{
					GUICollider.EnableAllCollider("CMD_PartyEdit");
				});
			}
			else
			{
				GUICollider.EnableAllCollider("CMD_PartyEdit");
			}
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_PartyEdit");
		}
	}

	private void OnClickedSave()
	{
		if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.EDIT)
		{
			this.OnClickedSaveOperation();
		}
		else if (CMD_QuestTOP.instance != null)
		{
			QuestData.WorldStageData worldStageData = CMD_QuestTOP.instance.GetWorldStageData();
			QuestData.WorldDungeonData stageDataBk = CMD_QuestTOP.instance.StageDataBk;
			bool flag = false;
			if (flag)
			{
				AlertManager.ShowAlertDialog(null, "警告", "出撃条件エラー", AlertManager.ButtonActionType.Close, false);
			}
			else if (worldStageData.worldStageM.worldAreaId == "8")
			{
				int num = int.Parse(stageDataBk.dungeon.dungeonTicketNum);
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(delegate(int idx)
				{
					if (idx == 0)
					{
						this.OnClickedSaveOperation();
					}
				}, "CMD_Confirm") as CMD_Confirm;
				cmd_Confirm.Title = StringMaster.GetString("TicketQuestTitle");
				string info = string.Format(StringMaster.GetString("TicketQuestConfirmInfo"), worldStageData.worldStageM.name, num, num - 1);
				cmd_Confirm.Info = info;
				cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonYes");
				cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
			}
			else
			{
				this.OnClickedSaveOperation();
			}
		}
		else
		{
			this.OnClickedSaveOperation();
		}
	}

	private void OnClickedSaveOperation()
	{
		if (this.isBattleClicked)
		{
			return;
		}
		this.isBattleClicked = true;
		if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.SELECT || CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.PVP || CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.MULTI)
		{
			CMD_PartyEdit.SelectedParty = this.idxNumber;
		}
		this.CloseOperation(true);
	}

	private void OpenMultiRecruitSettingModal()
	{
		CMD_MultiRecruitSettingModal cmd_MultiRecruitSettingModal = GUIMain.ShowCommonDialog(null, "CMD_MultiRecruitSettingModal") as CMD_MultiRecruitSettingModal;
		cmd_MultiRecruitSettingModal.SetCallbackAction(new Action<GameWebAPI.RespData_MultiRoomCreate>(this.MultiRoomCreateCallback));
	}

	private void MultiRoomCreateCallback(GameWebAPI.RespData_MultiRoomCreate data)
	{
		this.isBattleClicked = false;
		CMD_MultiRecruitPartyWait.UserType = CMD_MultiRecruitPartyWait.USER_TYPE.OWNER;
		CMD_MultiRecruitPartyWait.roomCreateData = data;
		GUIMain.ShowCommonDialog(null, "CMD_MultiRecruitPartyWait");
	}

	public override void ClosePanel(bool animation = true)
	{
		CMD_PartyEdit.SelectedParty = -1;
		this.CloseOperation(animation);
	}

	private void CloseOperation(bool animation = true)
	{
		if (this.IsChanged() || this.IsChangedFavorite())
		{
			GameWebAPI.MN_Req_EditDeckList req = this.MakeRequest();
			if (this.CheckReqEmpty(req))
			{
				return;
			}
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			this.req_editdecklist_bk = req;
			GameWebAPI.RequestMN_DeckEdit request = new GameWebAPI.RequestMN_DeckEdit
			{
				SetSendData = delegate(GameWebAPI.MN_Req_EditDeckList param)
				{
					param.deckData = req.deckData;
					param.selectDeckNum = req.selectDeckNum;
					param.favoriteDeckNum = req.favoriteDeckNum;
				},
				OnReceived = new Action<GameWebAPI.RespDataMN_EditDeckList>(this.EndPartyEdit)
			};
			AppCoroutine.Start(request.Run(null, null, null), false);
		}
		else
		{
			this.CloseAndFarmCamOn(animation);
			if (CMD_PartyEdit.SelectedParty > 0)
			{
				if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.MULTI)
				{
					this.isBattleClicked = false;
					this.OpenMultiRecruitSettingModal();
				}
				else if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.PVP)
				{
					CMD_PvPMatchingWait cmd_PvPMatchingWait = GUIMain.ShowCommonDialog(null, "CMD_PvPMatchingWait") as CMD_PvPMatchingWait;
					cmd_PvPMatchingWait.deckNum = CMD_PartyEdit.SelectedParty;
					this.isBattleClicked = false;
				}
				else
				{
					this.HideClips();
					RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
					DataMng.Instance().RespData_WorldMultiStartInfo = null;
					ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.StopGetHistoryIdList();
					TipsLoading.Instance.StartTipsLoad(CMD_Tips.DISPLAY_PLACE.QuestToSoloBattle, false);
				}
			}
		}
	}

	private void EndPartyEdit(GameWebAPI.RespDataMN_EditDeckList noop)
	{
		DataMng.Instance().DeckListUpdate(this.req_editdecklist_bk);
		this.FavoriteUpdate();
		APIRequestTask task = Singleton<UserDataMng>.Instance.RequestUserMonsterFriendshipTime(true);
		base.StartCoroutine(task.Run(delegate
		{
			RestrictionInput.EndLoad();
			base.StartCoroutine(this.CloseWait());
		}, null, null));
	}

	private void EndPartyEditCantSave(int i)
	{
		CMD_PartyEdit.SelectedParty = -1;
		base.StartCoroutine(this.CloseWait());
	}

	private IEnumerator CloseWait()
	{
		while (base.GetActCount() > 0)
		{
			yield return null;
		}
		this.CloseAndFarmCamOn(true);
		if (CMD_PartyEdit.SelectedParty > 0)
		{
			if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.MULTI)
			{
				this.isBattleClicked = false;
				this.OpenMultiRecruitSettingModal();
			}
			else if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.PVP)
			{
				CMD_PvPMatchingWait cmd = GUIMain.ShowCommonDialog(null, "CMD_PvPMatchingWait") as CMD_PvPMatchingWait;
				cmd.deckNum = CMD_PartyEdit.SelectedParty;
				this.isBattleClicked = false;
			}
			else if (!Loading.IsShow())
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
				DataMng.Instance().RespData_WorldMultiStartInfo = null;
				ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.StopGetHistoryIdList();
				TipsLoading.Instance.StartTipsLoad(CMD_Tips.DISPLAY_PLACE.TitleToFarm, true);
			}
		}
		yield break;
	}

	private IEnumerator CloseWaitFail()
	{
		while (base.GetActCount() > 0)
		{
			yield return null;
		}
		while (!AssetDataCacheMng.Instance().IsCacheAllReadyType(AssetDataCacheMng.CACHE_TYPE.CHARA_PARTY))
		{
			yield return null;
		}
		this.CloseAndFarmCamOn(true);
		yield break;
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		if ((CMD_PartyEdit.ModeType != CMD_PartyEdit.MODE_TYPE.PVP && CMD_PartyEdit.ModeType != CMD_PartyEdit.MODE_TYPE.MULTI) || CMD_PartyEdit.SelectedParty <= 0)
		{
			FarmCameraControlForCMD.On();
			base.ClosePanel(animation);
		}
	}

	public List<MonsterData> GetSelectedMD()
	{
		int num = this.csSelectPanel.fastSetPartObjs.Count - 1 - this.idxNumber - 1;
		if (this.idxNumber != -1)
		{
			num = this.idxNumber - 1;
		}
		int count = this.csSelectPanel.fastSetPartObjs.Count;
		GUIListPartsPartyEdit guilistPartsPartyEdit = this.csSelectPanel.fastSetPartObjs[count - 1 - num];
		return guilistPartsPartyEdit.GetNowMD();
	}

	private bool IsChanged()
	{
		string selectDeckNum = DataMng.Instance().RespDataMN_DeckList.selectDeckNum;
		string b = this.idxNumber.ToString();
		if (selectDeckNum != b)
		{
			return true;
		}
		for (int i = 0; i < this.csSelectPanel.fastSetPartObjs.Count; i++)
		{
			GUIListPartsPartyEdit guilistPartsPartyEdit = this.csSelectPanel.fastSetPartObjs[i];
			if (guilistPartsPartyEdit.IsChanged())
			{
				return true;
			}
		}
		return false;
	}

	private GameWebAPI.MN_Req_EditDeckList MakeRequest()
	{
		int selectDeckNum = this.idxNumber;
		int count = this.csSelectPanel.fastSetPartObjs.Count;
		GameWebAPI.MN_Req_EditDeckList mn_Req_EditDeckList = new GameWebAPI.MN_Req_EditDeckList();
		mn_Req_EditDeckList.deckData = new int[count][];
		for (int i = 0; i < count; i++)
		{
			GUIListPartsPartyEdit guilistPartsPartyEdit = this.csSelectPanel.fastSetPartObjs[i];
			int[] changed = guilistPartsPartyEdit.GetChanged();
			mn_Req_EditDeckList.deckData[count - 1 - i] = changed;
		}
		mn_Req_EditDeckList.selectDeckNum = selectDeckNum;
		mn_Req_EditDeckList.favoriteDeckNum = int.Parse(this.favoriteDeckNum_Now);
		return mn_Req_EditDeckList;
	}

	public List<string> MakeAllCharaPath()
	{
		List<string> list = new List<string>();
		int count = this.csSelectPanel.fastSetPartObjs.Count;
		for (int i = 0; i < count; i++)
		{
			GUIListPartsPartyEdit guilistPartsPartyEdit = this.csSelectPanel.fastSetPartObjs[i];
			List<MonsterData> nowMD = guilistPartsPartyEdit.GetNowMD();
			for (int j = 0; j < nowMD.Count; j++)
			{
				string monsterCharaPathByMonsterGroupId = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterGroupId(nowMD[j].monsterM.monsterGroupId);
				list.Add(monsterCharaPathByMonsterGroupId);
			}
		}
		return list;
	}

	private bool CheckReqEmpty(GameWebAPI.MN_Req_EditDeckList req)
	{
		int[][] deckData = req.deckData;
		for (int i = 0; i < deckData.Length; i++)
		{
			for (int j = 0; j < deckData[i].Length; j++)
			{
				if (deckData[i][j] == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void RefreshLeaderSkillText()
	{
		if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.EDIT)
		{
			if ((this.IsChanged() || this.IsChangedFavorite()) && this.backUpIdx != -1)
			{
				this.SaveButtonActive(true);
			}
			else
			{
				this.SaveButtonActive(false);
			}
		}
		List<MonsterData> selectedMD = this.GetSelectedMD();
		this.gsfTXT_LEADERSKILL.text = string.Empty;
		this.gsfTXT_LEADERSKILL_EXP.text = string.Empty;
		if (selectedMD.Count < 1)
		{
			return;
		}
		MonsterData monsterData = selectedMD[0];
		GameWebAPI.RespDataMA_GetSkillM.SkillM skillM = null;
		if (monsterData != null)
		{
			skillM = monsterData.leaderSkillM;
		}
		if (skillM == null)
		{
			this.gsfTXT_LEADERSKILL.text = StringMaster.GetString("SystemNone");
			this.gsfTXT_LEADERSKILL_EXP.text = StringMaster.GetString("CharaStatus-02");
		}
		else
		{
			this.gsfTXT_LEADERSKILL.text = skillM.name;
			this.gsfTXT_LEADERSKILL_EXP.text = skillM.description.Replace("\n", string.Empty).Replace("\r", string.Empty);
		}
	}

	private void RefreshPartyNumText()
	{
		if (this.csSelectPanel != null)
		{
			if (this.backUpIdx != this.idxNumber)
			{
				this.RefreshLeaderSkillText();
			}
			this.backUpIdx = this.idxNumber;
			this.gsfTXT_PARTY_NUM.text = string.Format(StringMaster.GetString("PartyNumber"), string.Empty) + this.idxNumber.ToString();
		}
	}

	private void InitMonsList()
	{
		this.dataList = new List<GameWebAPI.RespDataMN_GetDeckList.DeckList>();
		DataMng dataMng = DataMng.Instance();
		GameWebAPI.RespDataMN_GetDeckList.DeckList[] deckList = dataMng.RespDataMN_DeckList.deckList;
		for (int i = 0; i < deckList.Length; i++)
		{
			this.dataList.Add(deckList[i]);
		}
		this.csSelectPanel.initLocation = true;
		this.csSelectPanel.AllBuild(this.dataList);
		string selectDeckNum = dataMng.RespDataMN_DeckList.selectDeckNum;
		int selectedIndexRev = int.Parse(selectDeckNum) - 1;
		this.csSelectPanel.SetSelectedIndexRev(selectedIndexRev);
		this.goListParts.SetActive(false);
	}

	private void InitFavoriteBtn()
	{
		this.favoriteDeckNum_Org = DataMng.Instance().RespDataMN_DeckList.favoriteDeckNum;
		this.favoriteDeckNum_Now = this.favoriteDeckNum_Org;
		this.spBTN_FAVO = this.goBTN_FAVO.GetComponent<UISprite>();
		this.lbTXT_FAVO = this.goTXT_FAVO.GetComponent<UILabel>();
		this.colBTN_FAVO = this.goBTN_FAVO.GetComponent<GUICollider>();
		if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.SELECT || CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.PVP || CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.MULTI)
		{
			this.goBTN_FAVO.SetActive(false);
		}
	}

	private void InitDesideBtn()
	{
		if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.SELECT || CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.PVP || CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.MULTI)
		{
			UILabel component = this.goTX_DECIDE.GetComponent<UILabel>();
			if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.MULTI)
			{
				component.text = StringMaster.GetString("PartyRecruit");
			}
			else
			{
				component.text = StringMaster.GetString("PartyBattleStart");
			}
		}
		else
		{
			this.goSaveButton.SetActive(false);
		}
	}

	private void BtnFavoriteControl()
	{
		int num = int.Parse(this.favoriteDeckNum_Now) - 1;
		int num2 = this.idxNumber - 1;
		if (num == num2)
		{
			this.spBTN_FAVO.spriteName = "Common02_Btn_SupportRed";
			this.colBTN_FAVO.activeCollider = false;
			this.spBTN_FAVO.color = Color.white;
			this.lbTXT_FAVO.color = Color.white;
		}
		else
		{
			this.spBTN_FAVO.spriteName = "Common02_Btn_SupportWhite";
			this.colBTN_FAVO.activeCollider = true;
			this.spBTN_FAVO.color = Color.white;
			this.lbTXT_FAVO.color = this.colFavoOff;
		}
	}

	private void OnClickedFavorite()
	{
		int num = this.idxNumber - 1;
		this.favoriteDeckNum_Now = (num + 1).ToString();
		this.BtnFavoriteControl();
		this.ShowDaialogFavo();
	}

	private void OnClickedChangeStatus()
	{
		List<GUIListPartsPartyEdit> fastSetPartObjs = this.csSelectPanel.fastSetPartObjs;
		int num = fastSetPartObjs.Count - 1;
		int num2 = num - this.idxNumber - 1;
		if (this.idxNumber != -1)
		{
			num2 = this.idxNumber - 1;
		}
		int index = num - num2;
		int num3 = fastSetPartObjs[index].ppmiList[0].GetStatusPage();
		num3++;
		for (int i = 0; i < fastSetPartObjs[index].ppmiList.Count; i++)
		{
			fastSetPartObjs[index].ppmiList[i].SetStatusPage(num3);
		}
	}

	protected void ShowDaialogFavo()
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
		if (cmd_ModalMessage == null)
		{
			return;
		}
		cmd_ModalMessage.Title = StringMaster.GetString("PartyFavoriteTitle");
		cmd_ModalMessage.Info = StringMaster.GetString("PartyFavoriteInfo");
	}

	private bool IsChangedFavorite()
	{
		return this.favoriteDeckNum_Now != this.favoriteDeckNum_Org;
	}

	private void FavoriteUpdate()
	{
		this.favoriteDeckNum_Org = this.favoriteDeckNum_Now;
	}

	private void SetCommonUI()
	{
		this.goSelectPanel = GUIManager.LoadCommonGUI("SelectListPanel/SelectListPanelPartyEdit", base.gameObject);
		this.csSelectPanel = this.goSelectPanel.GetComponent<GUISelectPanelPartyEdit>();
		this.csSelectPanel.popupCallback = new Action(this.RefreshLeaderSkillText);
		this.csSelectPanel.partyEdit = this;
		if (this.goEFC_CTR_2 != null)
		{
			this.goSelectPanel.transform.parent = this.goEFC_CTR_2.transform;
		}
		Vector3 localPosition = this.goListParts.transform.localPosition;
		this.goSelectPanel.transform.localPosition.y = localPosition.y;
		GUICollider component = this.goSelectPanel.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition);
		this.csSelectPanel.selectParts = this.goListParts;
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -525f + localPosition.x;
		listWindowViewRect.xMax = 525f + localPosition.x;
		listWindowViewRect.yMin = -240f;
		listWindowViewRect.yMax = 240f;
		this.csSelectPanel.ListWindowViewRect = listWindowViewRect;
		this.InitMonsList();
		this.BtnFavoriteControl();
		if (CMD_PartyEdit.ModeType == CMD_PartyEdit.MODE_TYPE.EDIT)
		{
			Vector3 localPosition2 = new Vector3(486f, -278.6f, this.goChangeStatusButton.transform.localPosition.z);
			this.goChangeStatusButton.transform.localPosition = localPosition2;
			this.spLeaderSkillTitleBase.width = 950;
			this.spLeaderSkillDescriptionBase.width = 804;
		}
	}

	public Vector3 Get3DPos()
	{
		this.v3DPos.x = this.xPos;
		this.xPos += 1000f;
		return this.v3DPos;
	}

	public void Reset3DPos()
	{
		this.xPos = 1000f;
	}

	public void DispClips()
	{
		foreach (GameObject go in this.clipObjects)
		{
			NGUITools.SetActiveSelf(go, true);
		}
	}

	public void HideClips()
	{
		foreach (GameObject go in this.clipObjects)
		{
			NGUITools.SetActiveSelf(go, false);
		}
		for (int j = 0; j < this.csSelectPanel.fastSetPartObjs.Count; j++)
		{
			GUIListPartsPartyEdit guilistPartsPartyEdit = this.csSelectPanel.fastSetPartObjs[j];
			for (int k = 0; k < guilistPartsPartyEdit.ppmiList.Count; k++)
			{
				guilistPartsPartyEdit.ppmiList[k].HideClips();
			}
		}
	}

	private void SaveButtonActive(bool act)
	{
		this.goSaveButton.gameObject.GetComponent<GUICollider>().activeCollider = act;
		if (act)
		{
			this.goSaveButton.gameObject.GetComponent<UISprite>().spriteName = "Common02_Btn_Red";
		}
		else
		{
			this.goSaveButton.gameObject.GetComponent<UISprite>().spriteName = "Common02_Btn_Gray";
		}
	}

	public void ReloadAllCharacters(bool flg)
	{
		this.Reset3DPos();
		this.csSelectPanel.ReloadAllCharacters(flg);
	}

	public enum MODE_TYPE
	{
		EDIT,
		SELECT,
		PVP,
		MULTI
	}
}
