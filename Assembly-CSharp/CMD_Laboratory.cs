using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_Laboratory : CMD
{
	public static CMD_Laboratory instance;

	[SerializeField]
	private LaboratoryPartsStatusDetail baseDetail;

	[SerializeField]
	private LaboratoryPartsStatusDetail partnerDetail;

	[SerializeField]
	private LaboratoryPartsStatusDetail digitamaDetail;

	[SerializeField]
	private UILabel possessionTip;

	[SerializeField]
	private UILabel costTipTitle;

	[SerializeField]
	private UILabel costTip;

	[SerializeField]
	private UISprite ngBTN_DECIDE;

	[SerializeField]
	private GUICollider clBTN_DECIDE;

	[SerializeField]
	private UILabel ngTX_DECIDE;

	[Header("ベースデジモンラベル")]
	[SerializeField]
	private UILabel baseDigimonLabel;

	[Header("パートナーデジモンラベル")]
	[SerializeField]
	private UILabel partnerDigimonLabel;

	[NonSerialized]
	public string monsterRare;

	private List<BoxCollider> buttonEnableChangeList = new List<BoxCollider>();

	private CMD_CharacterDetailed characterDetailed;

	private int useClusterBK;

	private bool isShowRare;

	public MonsterData baseDigimon;

	public MonsterData partnerDigimon;

	private GameObject goBaseDigimon;

	private GameObject goPartnerDigimon;

	protected override void Awake()
	{
		base.Awake();
		CMD_Laboratory.instance = this;
		ClassSingleton<LaboratoryAccessor>.Instance.laboratory = this;
	}

	private void Start()
	{
		this.baseDigimonLabel.text = StringMaster.GetString("Succession-01");
		this.partnerDigimonLabel.text = StringMaster.GetString("ArousalPartner");
		this.costTipTitle.text = StringMaster.GetString("SystemCost");
		this.ngTX_DECIDE.text = StringMaster.GetString("SystemButtonDecision");
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_Laboratory.instance = null;
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (tutorialObserver != null)
		{
			GUIMain.BarrierON(null);
			tutorialObserver.StartSecondTutorial("second_tutorial_laboratory", new Action(GUIMain.BarrierOFF), delegate
			{
				GUICollider.EnableAllCollider("CMD_Laboratory");
			});
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_Laboratory");
		}
	}

	private void OnTouchDecide()
	{
		if (this.partnerDigimon.IsArousal())
		{
			this.OpenConfirmPartnerArousal(new Action<int>(this.OpenConfirmDigitamaParameter));
		}
		else
		{
			this.OpenConfirmDigitamaParameter(1);
		}
	}

	private void OpenConfirmPartnerArousal(Action<int> onClosedPopupAction)
	{
		CMD_ResearchModalAlert cmd_ResearchModalAlert = GUIMain.ShowCommonDialog(onClosedPopupAction, "CMD_ResearchModalAlert") as CMD_ResearchModalAlert;
		cmd_ResearchModalAlert.SetDigimonIcon(this.partnerDigimon);
	}

	private void OpenConfirmDigitamaParameter(int selectButtonIndex)
	{
		if (selectButtonIndex == 1)
		{
			MonsterEggStatusInfo digitamaStatus = this.CreateDigitamaStatus(this.baseDigimon, this.partnerDigimon);
			CMD_ResearchModal cmd_ResearchModal = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseLabo), "CMD_ResearchModal") as CMD_ResearchModal;
			cmd_ResearchModal.SetChipParams(this.baseDigimon, this.partnerDigimon);
			cmd_ResearchModal.SetDigitamaStatus(digitamaStatus);
		}
	}

	private bool CheckHaveMedal(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList UserMonsterData)
	{
		return UserMonsterData.hpAbilityFlg == "1" || UserMonsterData.hpAbilityFlg == "2" || (UserMonsterData.attackAbilityFlg == "1" || UserMonsterData.attackAbilityFlg == "2") || (UserMonsterData.defenseAbilityFlg == "1" || UserMonsterData.defenseAbilityFlg == "2") || (UserMonsterData.spAttackAbilityFlg == "1" || UserMonsterData.spAttackAbilityFlg == "2") || (UserMonsterData.spDefenseAbilityFlg == "1" || UserMonsterData.spDefenseAbilityFlg == "2") || (UserMonsterData.speedAbilityFlg == "1" || UserMonsterData.speedAbilityFlg == "2");
	}

	private void OnCloseLabo(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			DataMng.Instance().CheckCampaign(new Action<int>(this.LaboExec), new GameWebAPI.RespDataCP_Campaign.CampaignType[]
			{
				GameWebAPI.RespDataCP_Campaign.CampaignType.MedalTakeOverUp
			});
		}
	}

	private void LaboExec(int result)
	{
		if (result == -1)
		{
			return;
		}
		if (result > 0)
		{
			RestrictionInput.EndLoad();
			DataMng.Instance().CampaignErrorCloseAllCommonDialog(result == 1, delegate
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
				DataMng.Instance().ReloadCampaign(delegate
				{
					RestrictionInput.EndLoad();
				});
			});
			RestrictionInput.EndLoad();
			return;
		}
		this.useClusterBK = CalculatorUtil.CalcClusterForLaboratory(this.baseDigimon, this.partnerDigimon);
		this.isShowRare = (int.Parse(this.baseDigimon.userMonster.friendship) >= int.Parse(this.baseDigimon.growStepM.maxFriendship));
		GameWebAPI.RequestMN_MonsterCombination requestMN_MonsterCombination = new GameWebAPI.RequestMN_MonsterCombination();
		requestMN_MonsterCombination.SetSendData = delegate(GameWebAPI.MN_Req_Labo param)
		{
			param.baseUserMonsterId = int.Parse(this.baseDigimon.userMonster.userMonsterId);
			param.materialUserMonsterId = int.Parse(this.partnerDigimon.userMonster.userMonsterId);
		};
		requestMN_MonsterCombination.OnReceived = delegate(GameWebAPI.RespDataMN_LaboExec response)
		{
			DataMng.Instance().RespDataMN_LaboExec = response;
			if (response.userMonster != null)
			{
				DataMng.Instance().AddUserMonster(response.userMonster);
			}
		};
		GameWebAPI.RequestMN_MonsterCombination request = requestMN_MonsterCombination;
		base.StartCoroutine(request.Run(delegate()
		{
			AppCoroutine.Start(this.GetChipSlotInfo(), false);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private IEnumerator GetChipSlotInfo()
	{
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] monsterList = new GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[]
		{
			DataMng.Instance().RespDataMN_LaboExec.userMonster
		};
		GameWebAPI.MonsterSlotInfoListLogic request = ChipDataMng.RequestAPIMonsterSlotInfo(monsterList, null);
		yield return AppCoroutine.Start(request.Run(new Action(this.EndLaboSuccess), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(true);
		}, null), false);
		yield break;
	}

	private void EndLaboSuccess()
	{
		bool hasChip = this.baseDigimon.IsAttachedChip() || this.partnerDigimon.IsAttachedChip();
		GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] monsterChipList = ChipDataMng.GetMonsterChipList(this.baseDigimon.userMonster.userMonsterId);
		if (monsterChipList != null)
		{
			foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList in monsterChipList)
			{
				if (userChipList.userMonsterId == int.Parse(this.baseDigimon.userMonster.userMonsterId))
				{
					userChipList.userMonsterId = 0;
				}
			}
		}
		GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] monsterChipList2 = ChipDataMng.GetMonsterChipList(this.partnerDigimon.userMonster.userMonsterId);
		if (monsterChipList2 != null)
		{
			foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList2 in monsterChipList2)
			{
				if (userChipList2.userMonsterId == int.Parse(this.partnerDigimon.userMonster.userMonsterId))
				{
					userChipList2.userMonsterId = 0;
				}
			}
		}
		int[] umidL = new int[]
		{
			int.Parse(this.baseDigimon.userMonster.userMonsterId),
			int.Parse(this.partnerDigimon.userMonster.userMonsterId)
		};
		DataMng.Instance().DeleteUserMonsterList(umidL);
		GooglePlayGamesTool.Instance.Laboratory();
		bool isAwakening = int.Parse(this.baseDigimon.userMonster.friendship) == CommonSentenceData.MaxFriendshipValue(this.baseDigimon.monsterMG.growStep);
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		monsterDataMng.RefreshMonsterDataList();
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonsterList;
		if (DataMng.Instance().RespDataMN_LaboExec.userMonsterList != null)
		{
			userMonsterList = DataMng.Instance().RespDataMN_LaboExec.userMonsterList[0];
		}
		else
		{
			userMonsterList = DataMng.Instance().RespDataMN_LaboExec.userMonster;
		}
		string userMonsterId = userMonsterList.userMonsterId;
		MonsterData monsterDataByUserMonsterID = monsterDataMng.GetMonsterDataByUserMonsterID(userMonsterId, false);
		string eggTypeFromMD = MonsterDataMng.Instance().GetEggTypeFromMD(monsterDataByUserMonsterID);
		int item = int.Parse(this.baseDigimon.monsterM.monsterGroupId);
		int item2 = int.Parse(this.partnerDigimon.monsterM.monsterGroupId);
		List<int> umidList = new List<int>
		{
			item
		};
		List<int> list = new List<int>
		{
			item2
		};
		list.Add(int.Parse(eggTypeFromMD));
		int rareNum = 0;
		if (this.isShowRare)
		{
			rareNum = int.Parse(monsterDataByUserMonsterID.monsterM.rare);
		}
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene("Cutscenes/Fusion", new Action<int>(this.StartCutSceneCallBack), delegate(int index)
		{
			CutSceneMain.FadeReqCutSceneEnd();
			if (null != this.characterDetailed)
			{
				this.DisableCutinButton(this.characterDetailed.transform);
			}
			PartsMenu partsMenu = UnityEngine.Object.FindObjectOfType<PartsMenu>();
			if (null != partsMenu)
			{
				partsMenu.SetEnableMenuButton(false);
			}
		}, delegate(int index)
		{
			if (PartsUpperCutinController.Instance != null)
			{
				if (isAwakening)
				{
					PartsUpperCutinController.Instance.PlayAnimator(PartsUpperCutinController.AnimeType.ResearchComplete, delegate
					{
						PartsUpperCutinController.Instance.PlayAnimator(PartsUpperCutinController.AnimeType.AwakeningComplete, delegate
						{
							this.ShowStoreChipDialog(hasChip);
						});
					});
				}
				else
				{
					PartsUpperCutinController.Instance.PlayAnimator(PartsUpperCutinController.AnimeType.ResearchComplete, delegate
					{
						this.ShowStoreChipDialog(hasChip);
					});
				}
			}
			if (!hasChip)
			{
				RestrictionInput.EndLoad();
				this.EnableCutinButton();
				PartsMenu partsMenu = UnityEngine.Object.FindObjectOfType<PartsMenu>();
				if (null != partsMenu)
				{
					partsMenu.SetEnableMenuButton(true);
				}
			}
		}, umidList, list, 2, rareNum, 0.5f, 0.5f);
		this.monsterRare = this.baseDigimon.monsterM.rare;
	}

	private void DisableCutinButton(Transform t)
	{
		if (null != t)
		{
			for (int i = 0; i < t.childCount; i++)
			{
				Transform child = t.GetChild(i);
				BoxCollider component = child.GetComponent<BoxCollider>();
				if (null != component && component.enabled)
				{
					this.buttonEnableChangeList.Add(component);
					component.enabled = false;
				}
				this.DisableCutinButton(child);
			}
		}
	}

	private void EnableCutinButton()
	{
		for (int i = 0; i < this.buttonEnableChangeList.Count; i++)
		{
			this.buttonEnableChangeList[i].enabled = true;
		}
		this.buttonEnableChangeList.Clear();
	}

	private void ShowStoreChipDialog(bool hasChip)
	{
		if (hasChip)
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int i)
			{
				RestrictionInput.EndLoad();
			}, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("LaboratoryTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("LaboratoryCautionChip");
			this.EnableCutinButton();
			PartsMenu partsMenu = UnityEngine.Object.FindObjectOfType<PartsMenu>();
			if (null != partsMenu)
			{
				partsMenu.SetEnableMenuButton(true);
			}
		}
	}

	private void StartCutSceneCallBack(int i)
	{
		this.RemoveBaseDigimon();
		this.RemovePartnerDigimon();
		this.digitamaDetail.ClearDigitamaStatus();
		DataMng.Instance().US_PlayerInfoSubChipNum(this.useClusterBK);
		this.UpdateClusterNum();
		GUIPlayerStatus.RefreshParams_S(false);
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonsterList;
		if (DataMng.Instance().RespDataMN_LaboExec.userMonsterList != null)
		{
			userMonsterList = DataMng.Instance().RespDataMN_LaboExec.userMonsterList[0];
		}
		else
		{
			userMonsterList = DataMng.Instance().RespDataMN_LaboExec.userMonster;
		}
		string userMonsterId = userMonsterList.userMonsterId;
		MonsterData monsterDataByUserMonsterID = monsterDataMng.GetMonsterDataByUserMonsterID(userMonsterId, false);
		CMD_CharacterDetailed.AddButton = CMD_CharacterDetailed.ButtonType.Garden;
		CMD_CharacterDetailed.DataChg = monsterDataByUserMonsterID;
		this.characterDetailed = (GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed") as CMD_CharacterDetailed);
	}

	private void ShowMATInfo_0()
	{
		this.baseDetail.ShowMATInfo(this.baseDigimon);
		this.costTip.text = StringFormat.Cluster(CalculatorUtil.CalcClusterForLaboratory(this.baseDigimon, this.partnerDigimon));
	}

	private void ShowMATInfo_1()
	{
		this.partnerDetail.ShowMATInfo(this.partnerDigimon);
		this.costTip.text = StringFormat.Cluster(CalculatorUtil.CalcClusterForLaboratory(this.baseDigimon, this.partnerDigimon));
	}

	private void ShowCHGInfo()
	{
		if (this.baseDigimon != null && this.partnerDigimon != null)
		{
			MonsterEggStatusInfo digitamaStatus = this.CreateDigitamaStatus(this.baseDigimon, this.partnerDigimon);
			this.digitamaDetail.SetDigitamaStatus(digitamaStatus);
		}
		else
		{
			this.digitamaDetail.ClearDigitamaStatus();
		}
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_Laboratory");
		base.HideDLG();
		base.PartsTitle.SetTitle(StringMaster.GetString("LaboratoryTitle"));
		this.ShowMATInfo_0();
		this.ShowMATInfo_1();
		this.digitamaDetail.ClearDigitamaStatus();
		this.UpdateClusterNum();
		List<MonsterData> list = MonsterDataMng.Instance().GetMonsterDataList(false);
		list = MonsterDataMng.Instance().SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.GROWING_IN_GARDEN);
		if (list.Count >= ConstValue.MAX_CHILD_MONSTER)
		{
			base.StartCoroutine(this.OpenLaboMaxMessage());
		}
		else
		{
			base.ShowDLG();
			base.Show(f, sizeX, sizeY, aT);
		}
	}

	private IEnumerator OpenLaboMaxMessage()
	{
		yield return null;
		GUICollider.EnableAllCollider("CMD_Laboratory");
		CMD_ModalMessage cd = GUIMain.ShowCommonDialog(delegate(int i)
		{
			this.ClosePanel(false);
		}, "CMD_ModalMessage") as CMD_ModalMessage;
		cd.Title = StringMaster.GetString("LaboratoryTitle");
		cd.Info = StringMaster.GetString("LaboratoryMaxGarden");
		yield break;
	}

	private bool InitUltimateMonsterList()
	{
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		List<MonsterData> list = monsterDataMng.GetMonsterDataList(false);
		list = monsterDataMng.SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.RESEARCH_TARGET);
		list = monsterDataMng.SortMDList(list, false);
		return list.Count > 1;
	}

	private void UpdateClusterNum()
	{
		this.possessionTip.text = StringFormat.Cluster(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
	}

	private void ActMIconLong(MonsterData monsterData)
	{
		CMD_CharacterDetailed.DataChg = monsterData;
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(delegate(int i)
		{
			PartyUtil.SetLock(monsterData, false);
		}, "CMD_CharacterDetailed") as CMD_CharacterDetailed;
		cmd_CharacterDetailed.Mode = CMD_CharacterDetailed.LockMode.Laboratory;
	}

	private void OnTappedMAT_0()
	{
		List<MonsterData> list = MonsterDataMng.Instance().GetMonsterDataList(false);
		list = MonsterDataMng.Instance().SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.GROWING_IN_GARDEN);
		if (list.Count >= 3)
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("LaboratoryTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("LaboratoryMaxGarden");
			return;
		}
		bool flag = this.InitUltimateMonsterList();
		if (flag)
		{
			CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.LABO;
			CMD_BaseSelect.ElementType = CMD_BaseSelect.ELEMENT_TYPE.BASE;
			CommonDialog commonDialog = GUIMain.ShowCommonDialog(delegate(int index)
			{
				if (index == 1 && CMD_BaseSelect.DataChg != null)
				{
					this.baseDigimon = CMD_BaseSelect.DataChg;
					this.goBaseDigimon = this.SetSelectedCharChg(this.baseDigimon, this.baseDetail.GetCharaIconObject(), this.goBaseDigimon, 0);
					this.ShowMATInfo_0();
					this.ShowCHGInfo();
					this.BtnCont();
				}
			}, "CMD_BaseSelect");
			commonDialog.SetForceReturnValue(0);
		}
		else
		{
			this.ShowNonUltimateDialog();
		}
	}

	private void OnTappedMAT_1()
	{
		if (this.baseDigimon == null)
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("LaboratoryNotSelectedTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("LaboratoryNotSelectedInfo");
			return;
		}
		bool flag = this.InitUltimateMonsterList();
		if (flag)
		{
			CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.LABO;
			CMD_BaseSelect.ElementType = CMD_BaseSelect.ELEMENT_TYPE.PARTNER;
			CommonDialog commonDialog = GUIMain.ShowCommonDialog(delegate(int index)
			{
				if (index == 1 && CMD_BaseSelect.DataChg != null)
				{
					this.partnerDigimon = CMD_BaseSelect.DataChg;
					this.goPartnerDigimon = this.SetSelectedCharChg(this.partnerDigimon, this.partnerDetail.GetCharaIconObject(), this.goPartnerDigimon, 1);
					this.ShowMATInfo_1();
					this.ShowCHGInfo();
					this.BtnCont();
				}
			}, "CMD_BaseSelect");
			commonDialog.SetForceReturnValue(0);
		}
		else
		{
			this.ShowNonUltimateDialog();
		}
	}

	private void ShowNonUltimateDialog()
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("LaboratoryNoUltimateTitle");
		cmd_ModalMessage.Info = StringMaster.GetString("LaboratoryNoUltimateInfo");
	}

	private GameObject SetSelectedCharChg(MonsterData md, GameObject goEmpty, GameObject goIcon, int inum)
	{
		if (md != null)
		{
			if (goIcon != null)
			{
				UnityEngine.Object.DestroyImmediate(goIcon);
			}
			GUIMonsterIcon guimonsterIcon = MonsterDataMng.Instance().MakePrefabByMonsterData(md, goEmpty.transform.localScale, goEmpty.transform.localPosition, goEmpty.transform.parent, true, false);
			goIcon = guimonsterIcon.gameObject;
			goIcon.SetActive(true);
			guimonsterIcon.Data = md;
			if (inum == 0)
			{
				guimonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconShort_0));
			}
			else if (inum == 1)
			{
				guimonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconShort_1));
			}
			guimonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.ActMIconLong));
			UIWidget component = goEmpty.GetComponent<UIWidget>();
			UIWidget component2 = guimonsterIcon.gameObject.GetComponent<UIWidget>();
			if (component != null && component2 != null)
			{
				int add = component.depth - component2.depth;
				DepthController component3 = guimonsterIcon.gameObject.GetComponent<DepthController>();
				component3.AddWidgetDepth(guimonsterIcon.transform, add);
			}
			goEmpty.SetActive(false);
		}
		return goIcon;
	}

	private void ActMIconShort_0(MonsterData md)
	{
		this.OnTappedMAT_0();
	}

	private void ActMIconShort_1(MonsterData md)
	{
		this.OnTappedMAT_1();
	}

	private void BtnCont()
	{
		bool flag = false;
		if (this.baseDigimon != null && this.partnerDigimon != null)
		{
			int num = CalculatorUtil.CalcClusterForLaboratory(this.baseDigimon, this.partnerDigimon);
			int num2 = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
			if (num <= num2)
			{
				flag = true;
				this.costTip.color = Color.white;
			}
			else
			{
				this.costTip.color = Color.red;
			}
		}
		else
		{
			this.costTip.text = "0";
			this.costTip.color = Color.white;
		}
		if (flag)
		{
			this.ngBTN_DECIDE.spriteName = "Common02_Btn_Blue";
			this.ngTX_DECIDE.color = Color.white;
			this.clBTN_DECIDE.activeCollider = true;
		}
		else
		{
			this.ngBTN_DECIDE.spriteName = "Common02_Btn_Gray";
			this.ngTX_DECIDE.color = Color.gray;
			this.clBTN_DECIDE.activeCollider = false;
		}
	}

	public void RemoveBaseDigimon()
	{
		if (this.goBaseDigimon != null)
		{
			UnityEngine.Object.DestroyImmediate(this.goBaseDigimon);
		}
		this.baseDigimon = null;
		this.baseDetail.SetActiveIcon(true);
		this.ShowMATInfo_0();
		this.ShowCHGInfo();
		this.BtnCont();
	}

	public void RemovePartnerDigimon()
	{
		if (this.goPartnerDigimon != null)
		{
			UnityEngine.Object.DestroyImmediate(this.goPartnerDigimon);
		}
		this.partnerDigimon = null;
		this.partnerDetail.SetActiveIcon(true);
		this.ShowMATInfo_1();
		this.ShowCHGInfo();
		this.BtnCont();
	}

	private MonsterEggStatusInfo CreateDigitamaStatus(MonsterData baseData, MonsterData partnerData)
	{
		MonsterEggStatusInfo monsterEggStatusInfo = new MonsterEggStatusInfo();
		monsterEggStatusInfo.rare = baseData.monsterM.rare;
		int num = int.Parse(this.baseDigimon.userMonster.friendship);
		int num2 = CommonSentenceData.MaxFriendshipValue(this.baseDigimon.monsterMG.growStep);
		int num3 = this.baseDigimon.monsterM.rare.ToInt32();
		monsterEggStatusInfo.isArousal = false;
		if (num == num2 && num3 < 5)
		{
			monsterEggStatusInfo.isArousal = true;
		}
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster = baseData.userMonster;
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster2 = partnerData.userMonster;
		int goldMedalCount = this.GetGoldMedalCount(userMonster, userMonster2);
		monsterEggStatusInfo.hpAbilityFlg = this.GetCandidateMedal(userMonster.hpAbilityFlg, userMonster2.hpAbilityFlg, goldMedalCount);
		monsterEggStatusInfo.attackAbilityFlg = this.GetCandidateMedal(userMonster.attackAbilityFlg, userMonster2.attackAbilityFlg, goldMedalCount);
		monsterEggStatusInfo.defenseAbilityFlg = this.GetCandidateMedal(userMonster.defenseAbilityFlg, userMonster2.defenseAbilityFlg, goldMedalCount);
		monsterEggStatusInfo.spAttackAbilityFlg = this.GetCandidateMedal(userMonster.spAttackAbilityFlg, userMonster2.spAttackAbilityFlg, goldMedalCount);
		monsterEggStatusInfo.spDefenseAbilityFlg = this.GetCandidateMedal(userMonster.spDefenseAbilityFlg, userMonster2.spDefenseAbilityFlg, goldMedalCount);
		monsterEggStatusInfo.speedAbilityFlg = this.GetCandidateMedal(userMonster.speedAbilityFlg, userMonster2.speedAbilityFlg, goldMedalCount);
		monsterEggStatusInfo.luck = baseData.userMonster.luck;
		return monsterEggStatusInfo;
	}

	private int GetGoldMedalCount(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList baseUserMonsterData, GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList partnerUserMonsterData)
	{
		int num = 0;
		if (this.IsGoldMedal(baseUserMonsterData.hpAbilityFlg, partnerUserMonsterData.hpAbilityFlg))
		{
			num++;
		}
		if (this.IsGoldMedal(baseUserMonsterData.attackAbilityFlg, partnerUserMonsterData.attackAbilityFlg))
		{
			num++;
		}
		if (this.IsGoldMedal(baseUserMonsterData.defenseAbilityFlg, partnerUserMonsterData.defenseAbilityFlg))
		{
			num++;
		}
		if (this.IsGoldMedal(baseUserMonsterData.spAttackAbilityFlg, partnerUserMonsterData.spAttackAbilityFlg))
		{
			num++;
		}
		if (this.IsGoldMedal(baseUserMonsterData.spDefenseAbilityFlg, partnerUserMonsterData.spDefenseAbilityFlg))
		{
			num++;
		}
		if (this.IsGoldMedal(baseUserMonsterData.speedAbilityFlg, partnerUserMonsterData.speedAbilityFlg))
		{
			num++;
		}
		return num;
	}

	private bool IsGoldMedal(string baseMedalType, string partnerMedalType)
	{
		int num = baseMedalType.ToInt32();
		int num2 = partnerMedalType.ToInt32();
		return num == 1 || num2 == 1;
	}

	private ConstValue.CandidateMedal GetCandidateMedal(string baseMedalType, string partnerMedalType, int goldMedalCount)
	{
		int num = baseMedalType.ToInt32();
		int num2 = partnerMedalType.ToInt32();
		ConstValue.CandidateMedal result = ConstValue.CandidateMedal.NONE;
		if (num == 1)
		{
			if (ConstValue.MAX_GOLD_MEDAL_COUNT < goldMedalCount)
			{
				result = ConstValue.CandidateMedal.GOLD_OR_SILVER;
			}
			else
			{
				result = ConstValue.CandidateMedal.GOLD;
			}
		}
		else if (num2 == 1)
		{
			result = ConstValue.CandidateMedal.GOLD_OR_SILVER;
		}
		else if (num == 2)
		{
			result = ConstValue.CandidateMedal.SILVER;
		}
		else if (num2 == 2)
		{
			result = ConstValue.CandidateMedal.SILVER_OR_NONE;
		}
		return result;
	}
}
