using Cutscene;
using Master;
using Monster;
using MonsterList.InheritSkill;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CMD_Succession : CMD
{
	public List<GameObject> goMN_ICON_LIST;

	[SerializeField]
	private GameObject goMN_ICON_CHG;

	[SerializeField]
	private List<GameObject> goMN_ICON_MAT_LIST;

	[SerializeField]
	private UILabel baseTitleName;

	[SerializeField]
	private UILabel partnerTitleName;

	[SerializeField]
	private MonsterBasicInfo baseMonsterBasicInfo;

	[SerializeField]
	private MonsterLearnSkill baseMonsterSuccessionSkill;

	[SerializeField]
	private CMD_Succession.SkillTab baseMonsterSkillTab1;

	[SerializeField]
	private CMD_Succession.SkillTab baseMonsterSkillTab2;

	[SerializeField]
	private MonsterBasicInfo materialMonsterBasicInfo;

	[SerializeField]
	private MonsterLearnSkill materialMonsterSuccessionSkill;

	[SerializeField]
	private CMD_Succession.SkillTab materialMonsterSkillTab1;

	[SerializeField]
	private CMD_Succession.SkillTab materialMonsterSkillTab2;

	[Header("所持クラスタ数")]
	[SerializeField]
	private UILabel myClusterLabel;

	[Header("必要クラスタ数")]
	[SerializeField]
	private UILabel useClusterLabel;

	[SerializeField]
	private UISprite ngBTN_DECIDE;

	[SerializeField]
	private GUICollider clBTN_DECIDE;

	[SerializeField]
	private UILabel ngTX_DECIDE;

	[Header("表示デジモン数")]
	[SerializeField]
	private UILabel ngTX_MN_HAVE;

	[SerializeField]
	[Header("ソートのラベル")]
	private UILabel ngTX_SORT_DISP;

	[SerializeField]
	private UILabel sortButtonLabel;

	[SerializeField]
	private UILabel necessaryLabel;

	private BtnSort sortButton;

	private GUIMonsterIcon leftLargeMonsterIcon;

	private int useClusterBK;

	private int baseDigimonSkillNumber = 1;

	private int partnerDigimonSkillNumber = 1;

	private List<MonsterData> targetMonsterList;

	private List<GUIMonsterIcon> targetMonsterIconList;

	private List<MonsterData> deckMDList;

	private InheritSkillIconGrayOut iconGrayOut;

	private InheritSkillMonsterList monsterList;

	private GameObject goSelectPanelMonsterIcon;

	private GUISelectPanelMonsterIcon csSelectPanelMonsterIcon;

	private List<MonsterData> selecterPartnerDigimons = new List<MonsterData>();

	protected override void Awake()
	{
		base.Awake();
		this.deckMDList = ClassSingleton<MonsterUserDataMng>.Instance.GetDeckUserMonsterList();
		this.iconGrayOut = new InheritSkillIconGrayOut();
		this.iconGrayOut.SetNormalAction(new Action<MonsterData>(this.ActMIconShort), new Action<MonsterData>(this.ActMIconLong));
		this.iconGrayOut.SetSelectedAction(new Action<MonsterData>(this.ActMIconS_Remove), new Action<MonsterData>(this.ActMIconLong));
		this.iconGrayOut.SetBlockAction(null, new Action<MonsterData>(this.ActMIconLong));
		this.monsterList = new InheritSkillMonsterList();
		this.monsterList.Initialize(this.deckMDList, ClassSingleton<MonsterUserDataMng>.Instance.GetColosseumDeckUserMonsterList(), this.iconGrayOut);
		for (int i = 0; i < this.goMN_ICON_LIST.Count; i++)
		{
			this.goMN_ICON_LIST[i].SetActive(false);
		}
		this.targetMonsterIconList = new List<GUIMonsterIcon>();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_Succession");
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.PartsTitle.SetTitle(StringMaster.GetString("SuccessionTitle"));
		this.ngTX_DECIDE.text = StringMaster.GetString("SuccessionTitle");
		this.sortButtonLabel.text = StringMaster.GetString("SystemSortButton");
		this.necessaryLabel.text = StringMaster.GetString("Succession-02");
		this.baseMonsterSkillTab1.SetLabel(StringMaster.GetString("SkillInheritTitle1"));
		this.baseMonsterSkillTab2.SetLabel(StringMaster.GetString("SkillInheritTitle2"));
		this.materialMonsterSkillTab1.SetLabel(StringMaster.GetString("SkillInheritTitle1"));
		this.materialMonsterSkillTab2.SetLabel(StringMaster.GetString("SkillInheritTitle2"));
		this.baseMonsterSkillTab1.Off();
		this.baseMonsterSkillTab2.Off();
		this.baseMonsterSkillTab2.SetActive(false);
		this.materialMonsterSkillTab1.Off();
		this.materialMonsterSkillTab2.Off();
		this.materialMonsterSkillTab2.SetActive(false);
		this.SetCommonUI();
		this.InitMonsterList(true);
		this.ShowChgInfo();
		this.ShowMATInfo();
		this.UpdateClusterNum();
		base.Show(f, sizeX, sizeY, aT);
		base.SetTutorialAnyTime("anytime_second_tutorial_succession");
		RestrictionInput.EndLoad();
	}

	protected override void WindowClosed()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.PushBackAllMonsterPrefab();
		base.WindowClosed();
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (tutorialObserver != null)
		{
			GUIMain.BarrierON(null);
			tutorialObserver.StartSecondTutorial("second_tutorial_succession", new Action(GUIMain.BarrierOFF), delegate
			{
				GUICollider.EnableAllCollider("CMD_Succession");
			});
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_Succession");
		}
	}

	private void OnTouchSort()
	{
	}

	private void OnTouchDecide()
	{
		CMD_InheritCheck cmd_InheritCheck = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseSuccession), "CMD_InheritCheck") as CMD_InheritCheck;
		cmd_InheritCheck.SetParams(this.selecterPartnerDigimons, this.useClusterLabel.text, this.baseDigimonSkillNumber, this.partnerDigimonSkillNumber);
	}

	private void OnBaseDigimonSkill1()
	{
		if (this.baseDigimon != null)
		{
			this.FunctionBaseDigimonSkill1();
		}
	}

	private void FunctionBaseDigimonSkill1()
	{
		this.baseDigimonSkillNumber = 1;
		this.baseMonsterSuccessionSkill.SetCommonSkill(this.baseDigimon);
		this.baseMonsterSkillTab1.On();
		this.baseMonsterSkillTab2.Off();
	}

	private void OnBaseDigimonSkill2()
	{
		if (this.baseDigimon != null && MonsterStatusData.IsVersionUp(this.baseDigimon.GetMonsterMaster().Simple.rare))
		{
			this.FunctionBaseDigimonSkill2();
		}
	}

	private void FunctionBaseDigimonSkill2()
	{
		this.baseDigimonSkillNumber = 2;
		this.baseMonsterSuccessionSkill.SetCommonSkill2(this.baseDigimon);
		this.baseMonsterSkillTab1.Off();
		this.baseMonsterSkillTab2.On();
	}

	private void OnPartnerDigimonSkill1()
	{
		if (this.selecterPartnerDigimons.Count > 0)
		{
			this.FunctionPartnerDigimonSkill1();
		}
	}

	private void FunctionPartnerDigimonSkill1()
	{
		this.partnerDigimonSkillNumber = 1;
		this.materialMonsterSuccessionSkill.SetCommonSkill(this.selecterPartnerDigimons[0]);
		this.materialMonsterSkillTab1.On();
		this.materialMonsterSkillTab2.Off();
	}

	private void OnPartnerDigimonSkill2()
	{
		if (this.selecterPartnerDigimons.Count > 0 && this.selecterPartnerDigimons[0].GetExtraCommonSkill() != null)
		{
			this.FunctionPartnerDigimonSkill2();
		}
	}

	private void FunctionPartnerDigimonSkill2()
	{
		this.partnerDigimonSkillNumber = 2;
		this.materialMonsterSuccessionSkill.SetCommonSkill2(this.selecterPartnerDigimons[0]);
		this.materialMonsterSkillTab1.Off();
		this.materialMonsterSkillTab2.On();
	}

	private bool IsMATLevelMax()
	{
		for (int i = 0; i < this.selecterPartnerDigimons.Count; i++)
		{
			int num = int.Parse(this.selecterPartnerDigimons[i].userMonster.level);
			int num2 = int.Parse(this.selecterPartnerDigimons[i].monsterM.maxLevel);
			if (num < num2)
			{
				return false;
			}
		}
		return true;
	}

	private bool IsSameSkill()
	{
		string a = string.Empty;
		if (this.baseDigimonSkillNumber == 1)
		{
			a = this.baseDigimon.userMonster.commonSkillId;
		}
		else
		{
			a = this.baseDigimon.userMonster.extraCommonSkillId;
		}
		string b = string.Empty;
		if (this.partnerDigimonSkillNumber == 1)
		{
			b = this.selecterPartnerDigimons[0].userMonster.commonSkillId;
		}
		else
		{
			b = this.selecterPartnerDigimons[0].userMonster.extraCommonSkillId;
		}
		return a == b;
	}

	private void OnCloseSuccession(int idx)
	{
		if (idx == 0)
		{
			if (!this.IsMATLevelMax())
			{
				CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
				cmd_ModalMessage.Title = StringMaster.GetString("SuccessionTitle");
				cmd_ModalMessage.Info = StringMaster.GetString("SuccessionFailedLv");
			}
			else if (this.IsSameSkill())
			{
				CMD_ModalMessage cmd_ModalMessage2 = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
				cmd_ModalMessage2.Title = StringMaster.GetString("SuccessionTitle");
				cmd_ModalMessage2.Info = StringMaster.GetString("SuccessionFailedSame");
			}
			else
			{
				this.ExecSuccession();
			}
		}
	}

	private void ExecSuccession()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		string materialMonster = this.selecterPartnerDigimons[0].userMonster.userMonsterId;
		string materialModelId = this.selecterPartnerDigimons[0].GetMonsterMaster().Group.modelId;
		string materialGrowStep = this.selecterPartnerDigimons[0].GetMonsterMaster().Group.growStep;
		this.useClusterBK = this.CalcClusterForSuccession(this.baseDigimon, this.selecterPartnerDigimons);
		GameWebAPI.RequestMN_MonsterInheritance requestMN_MonsterInheritance = new GameWebAPI.RequestMN_MonsterInheritance();
		requestMN_MonsterInheritance.SetSendData = delegate(GameWebAPI.MN_Req_Success param)
		{
			param.baseUserMonsterId = this.baseDigimon.userMonster.userMonsterId;
			param.materialUserMonsterId = materialMonster;
			param.baseCommonSkillNumber = this.baseDigimonSkillNumber;
			param.materialCommonSkillNumber = this.partnerDigimonSkillNumber;
		};
		requestMN_MonsterInheritance.OnReceived = delegate(GameWebAPI.RespDataMN_SuccessExec response)
		{
			ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonster);
		};
		GameWebAPI.RequestMN_MonsterInheritance request = requestMN_MonsterInheritance;
		base.StartCoroutine(request.Run(delegate()
		{
			this.EndSuccession(materialModelId, materialGrowStep);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void EndSuccession(string materialMonsterModelId, string materialMonsterGrowStep)
	{
		string[] userMonsterIdList = this.selecterPartnerDigimons.Select((MonsterData x) => x.userMonster.userMonsterId).ToArray<string>();
		ClassSingleton<MonsterUserDataMng>.Instance.DeleteUserMonsterData(userMonsterIdList);
		ChipDataMng.DeleteEquipChip(userMonsterIdList);
		ChipDataMng.GetUserChipSlotData().DeleteMonsterSlotList(userMonsterIdList);
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		this.InitMonsterList(false);
		CutsceneDataInheritance cutsceneData = new CutsceneDataInheritance
		{
			path = "Cutscenes/Inheritance",
			baseModelId = this.baseDigimon.GetMonsterMaster().Group.modelId,
			materialModelId = materialMonsterModelId,
			endCallback = new Action(CutSceneMain.FadeReqCutSceneEnd)
		};
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene(cutsceneData, new Action(this.StartCutSceneCallBack), null, delegate(int index)
		{
			if (PartsUpperCutinController.Instance != null)
			{
				PartsUpperCutinController.Instance.PlayAnimator(PartsUpperCutinController.AnimeType.InheritanceComplete, null);
			}
			RestrictionInput.EndLoad();
		}, 0.5f, 0.5f);
	}

	private void StartCutSceneCallBack()
	{
		this.leftLargeMonsterIcon.Data = this.baseDigimon;
		GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.baseDigimon);
		this.iconGrayOut.SetSelect(icon);
		for (int i = 0; i < this.selecterPartnerDigimons.Count; i++)
		{
			UnityEngine.Object.Destroy(this.targetMonsterIconList[i].gameObject);
			this.goMN_ICON_MAT_LIST[i].SetActive(true);
		}
		this.selecterPartnerDigimons = new List<MonsterData>();
		this.targetMonsterIconList.Clear();
		DataMng.Instance().US_PlayerInfoSubChipNum(this.useClusterBK);
		this.UpdateClusterNum();
		this.baseMonsterSkillTab1.Off();
		this.baseMonsterSkillTab2.Off();
		this.baseMonsterSkillTab2.SetActive(false);
		this.materialMonsterSkillTab1.Off();
		this.materialMonsterSkillTab2.Off();
		this.materialMonsterSkillTab2.SetActive(false);
		this.ShowChgInfo();
		this.ShowMATInfo();
		this.monsterList.SetGrayOutDeckMonster(this.baseDigimon);
		this.monsterList.SetGrayOutUserMonsterList(this.baseDigimon);
		this.BtnCont();
		CMD_CharacterDetailed.DataChg = this.baseDigimon;
		GUIMain.ShowCommonDialog(delegate(int nop)
		{
			this.leftLargeMonsterIcon.Lock = this.baseDigimon.userMonster.IsLocked;
			icon.Lock = this.baseDigimon.userMonster.IsLocked;
		}, "CMD_CharacterDetailed");
	}

	private void ShowChgInfo()
	{
		if (this.baseDigimon != null)
		{
			this.baseMonsterBasicInfo.SetMonsterData(this.baseDigimon);
			if (MonsterStatusData.IsVersionUp(this.baseDigimon.GetMonsterMaster().Simple.rare))
			{
				this.baseMonsterSkillTab2.SetActive(true);
				if (this.baseDigimon.GetExtraCommonSkill() != null)
				{
					this.FunctionBaseDigimonSkill1();
				}
				else
				{
					this.FunctionBaseDigimonSkill2();
				}
			}
			else
			{
				this.baseMonsterSkillTab2.SetActive(false);
				this.FunctionBaseDigimonSkill1();
			}
		}
		else
		{
			this.baseMonsterBasicInfo.ClearMonsterData();
			this.baseMonsterSuccessionSkill.SetCommonSkill(null);
			this.baseMonsterSkillTab1.Off();
			this.baseMonsterSkillTab2.Off();
			this.baseMonsterSkillTab2.SetActive(false);
		}
	}

	private void ShowMATInfo()
	{
		if (this.selecterPartnerDigimons.Count > 0 && this.selecterPartnerDigimons[0] != null)
		{
			MonsterData monsterData = this.selecterPartnerDigimons[0];
			this.materialMonsterBasicInfo.SetMonsterData(monsterData);
			this.materialMonsterSuccessionSkill.SetSkill(monsterData);
			int num = this.CalcClusterForSuccession(this.baseDigimon, this.selecterPartnerDigimons);
			this.useClusterLabel.text = StringFormat.Cluster(num);
			if (num > int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney))
			{
				this.useClusterLabel.color = Color.red;
			}
			else
			{
				this.useClusterLabel.color = Color.white;
			}
			bool active = MonsterStatusData.IsVersionUp(monsterData.GetMonsterMaster().Simple.rare);
			this.materialMonsterSkillTab2.SetActive(active);
			this.FunctionPartnerDigimonSkill1();
		}
		else
		{
			this.materialMonsterBasicInfo.ClearMonsterData();
			this.materialMonsterSuccessionSkill.ClearSkill();
			this.useClusterLabel.text = "0";
			this.useClusterLabel.color = Color.white;
			this.materialMonsterSkillTab1.Off();
			this.materialMonsterSkillTab2.Off();
			this.materialMonsterSkillTab2.SetActive(false);
		}
	}

	private void SetCommonUI()
	{
		this.baseTitleName.text = StringMaster.GetString("Succession-01");
		this.partnerTitleName.text = StringMaster.GetString("ArousalPartner");
		this.goSelectPanelMonsterIcon = GUIManager.LoadCommonGUI("SelectListPanel/SelectListPanelMonsterIcon", base.gameObject);
		this.csSelectPanelMonsterIcon = this.goSelectPanelMonsterIcon.GetComponent<GUISelectPanelMonsterIcon>();
		Vector3 localPosition = this.goSelectPanelMonsterIcon.transform.localPosition;
		localPosition.x = 208f;
		GUICollider component = this.goSelectPanelMonsterIcon.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition);
		if (this.goEFC_RIGHT != null)
		{
			this.goSelectPanelMonsterIcon.transform.parent = this.goEFC_RIGHT.transform;
		}
		this.csSelectPanelMonsterIcon.ListWindowViewRect = ConstValue.GetRectWindow2();
	}

	private void InitMonsterList(bool initLoc = true)
	{
		ClassSingleton<GUIMonsterIconList>.Instance.ResetIconState();
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		List<MonsterData> list = monsterDataMng.GetMonsterDataList();
		list = MonsterFilter.Filter(list, MonsterFilterType.ALL_OUT_GARDEN);
		monsterDataMng.SortMDList(list);
		monsterDataMng.SetSortLSMessage();
		this.csSelectPanelMonsterIcon.initLocation = initLoc;
		Vector3 localScale = this.goMN_ICON_LIST[0].transform.localScale;
		ClassSingleton<GUIMonsterIconList>.Instance.SetLockIcon();
		this.csSelectPanelMonsterIcon.SetCheckEnablePushAction(null);
		this.csSelectPanelMonsterIcon.useLocationRecord = true;
		this.targetMonsterList = list;
		list = MonsterDataMng.Instance().SelectionMDList(list);
		this.csSelectPanelMonsterIcon.AllBuild(list, localScale, new Action<MonsterData>(this.ActMIconLong), new Action<MonsterData>(this.ActMIconShort), false);
		BtnSort[] componentsInChildren = base.GetComponentsInChildren<BtnSort>(true);
		this.sortButton = componentsInChildren[0];
		this.sortButton.OnChangeSortType = new Action(this.OnChangeSortSetting);
		this.sortButton.SortTargetMonsterList = this.targetMonsterList;
	}

	private void OnChangeSortSetting()
	{
		MonsterDataMng.Instance().SortMDList(this.targetMonsterList);
		MonsterDataMng.Instance().SetSortLSMessage();
		List<MonsterData> dts = MonsterDataMng.Instance().SelectionMDList(this.targetMonsterList);
		this.csSelectPanelMonsterIcon.ReAllBuild(dts);
		if (this.baseDigimon != null)
		{
			this.monsterList.SetGrayOutDeckMonster(this.baseDigimon);
			this.monsterList.SetGrayOutUserMonsterList(this.baseDigimon);
		}
	}

	private void ActMIconLong(MonsterData tappedMonsterData)
	{
		bool flag = this.CheckPartnerMonster(tappedMonsterData);
		CMD_CharacterDetailed.DataChg = tappedMonsterData;
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(delegate(int i)
		{
			if (tappedMonsterData == this.baseDigimon)
			{
				this.leftLargeMonsterIcon.Lock = tappedMonsterData.userMonster.IsLocked;
			}
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(tappedMonsterData);
			if (null != icon)
			{
				icon.Lock = tappedMonsterData.userMonster.IsLocked;
				if (!MonsterStatusData.IsSpecialTrainingType(tappedMonsterData.GetMonsterMaster().Group.monsterType) && this.IsPartnerCandidateMonster(tappedMonsterData))
				{
					this.iconGrayOut.SetLockReturnDetailed(icon, tappedMonsterData.userMonster.IsLocked);
				}
			}
		}, "CMD_CharacterDetailed") as CMD_CharacterDetailed;
		if (flag)
		{
			cmd_CharacterDetailed.Mode = CMD_CharacterDetailed.LockMode.Succession;
		}
	}

	private bool IsDeckMonster(string userMonsterId)
	{
		bool flag = false;
		for (int i = 0; i < this.deckMDList.Count; i++)
		{
			if (userMonsterId == this.deckMDList[i].userMonster.userMonsterId)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			flag = ClassSingleton<MonsterUserDataMng>.Instance.FindMonsterColosseumDeck(userMonsterId);
		}
		return flag;
	}

	private bool CheckPartnerMonster(MonsterData targetMonster)
	{
		bool result = false;
		for (int i = 0; i < this.selecterPartnerDigimons.Count; i++)
		{
			if (this.selecterPartnerDigimons[i] == targetMonster)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private bool IsPartnerCandidateMonster(MonsterData targetMonster)
	{
		bool result = false;
		if (this.baseDigimon != null && this.baseDigimon != targetMonster && !this.CheckPartnerMonster(targetMonster) && !this.IsDeckMonster(targetMonster.userMonster.userMonsterId))
		{
			result = true;
		}
		return result;
	}

	private MonsterData baseDigimon { get; set; }

	private void ActMIconShort(MonsterData md)
	{
		GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(md);
		if (this.baseDigimon == null)
		{
			this.iconGrayOut.SetSelect(icon);
			this.baseDigimon = md;
			this.leftLargeMonsterIcon = this.ShowCreateIcon(md, this.goMN_ICON_CHG);
			this.ShowChgInfo();
			this.monsterList.SetGrayOutDeckMonster(this.baseDigimon);
			this.monsterList.SetGrayOutUserMonsterList(this.baseDigimon);
		}
		else if (1 > this.selecterPartnerDigimons.Count)
		{
			this.iconGrayOut.SetSelectPartnerIcon(icon);
			this.selecterPartnerDigimons.Add(md);
			GUIMonsterIcon item = this.ShowCreateIcon(this.selecterPartnerDigimons[0], this.goMN_ICON_MAT_LIST[0]);
			this.targetMonsterIconList.Add(item);
			this.ShowMATInfo();
		}
		else
		{
			this.iconGrayOut.SetSelectPartnerIcon(icon);
			this.ActMIconS_Remove(this.selecterPartnerDigimons[0]);
			this.selecterPartnerDigimons.Add(md);
			GUIMonsterIcon item2 = this.ShowCreateIcon(this.selecterPartnerDigimons[0], this.goMN_ICON_MAT_LIST[0]);
			this.targetMonsterIconList.Add(item2);
			this.ShowMATInfo();
		}
		this.BtnCont();
	}

	private void ActMIconS_Remove(MonsterData md)
	{
		if (md == this.baseDigimon)
		{
			this.baseDigimon = null;
			UnityEngine.Object.Destroy(this.leftLargeMonsterIcon.gameObject);
			this.leftLargeMonsterIcon = null;
			this.goMN_ICON_CHG.SetActive(true);
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(md);
			this.iconGrayOut.CancelSelect(icon);
			this.ShowChgInfo();
			MonsterData ignoreMonster = null;
			if (0 < this.selecterPartnerDigimons.Count)
			{
				ignoreMonster = this.selecterPartnerDigimons[0];
			}
			this.monsterList.ClearIconGrayOutUserMonster(ignoreMonster);
		}
		else
		{
			for (int i = 0; i < this.selecterPartnerDigimons.Count; i++)
			{
				if (this.selecterPartnerDigimons[i] == md)
				{
					this.selecterPartnerDigimons.RemoveAt(i);
					UnityEngine.Object.Destroy(this.targetMonsterIconList[i].gameObject);
					this.targetMonsterIconList.RemoveAt(i);
					this.goMN_ICON_MAT_LIST[i].SetActive(true);
					GUIMonsterIcon icon2 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(md);
					this.iconGrayOut.CancelSelect(icon2);
					this.ShiftMatIcon(i);
					break;
				}
			}
			this.ShowMATInfo();
		}
		this.BtnCont();
	}

	private GUIMonsterIcon ShowCreateIcon(MonsterData md, GameObject goEmpty)
	{
		Transform transform = goEmpty.transform;
		GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(md, transform.localScale, transform.localPosition, transform.parent, true, false);
		guimonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
		guimonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.ActMIconLong));
		guimonsterIcon.transform.localScale = new Vector3(0.84f, 0.84f, 1f);
		UIWidget component = goEmpty.GetComponent<UIWidget>();
		UIWidget component2 = guimonsterIcon.gameObject.GetComponent<UIWidget>();
		if (component != null && component2 != null)
		{
			int add = component.depth - component2.depth;
			DepthController component3 = guimonsterIcon.gameObject.GetComponent<DepthController>();
			component3.AddWidgetDepth(guimonsterIcon.transform, add);
		}
		goEmpty.SetActive(false);
		return guimonsterIcon;
	}

	private void ShiftMatIcon(int idx)
	{
		int i;
		for (i = 0; i < this.selecterPartnerDigimons.Count; i++)
		{
			this.targetMonsterIconList[i].gameObject.transform.localPosition = this.goMN_ICON_MAT_LIST[i].transform.localPosition;
			this.goMN_ICON_MAT_LIST[i].SetActive(false);
		}
		while (i < this.goMN_ICON_MAT_LIST.Count)
		{
			this.goMN_ICON_MAT_LIST[i].SetActive(true);
			i++;
		}
	}

	private void UpdateClusterNum()
	{
		this.myClusterLabel.text = StringFormat.Cluster(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
		GUIPlayerStatus.RefreshParams_S(false);
	}

	private void BtnCont()
	{
		bool flag = false;
		if (this.baseDigimon != null && this.selecterPartnerDigimons.Count > 0)
		{
			int num = this.CalcClusterForSuccession(this.baseDigimon, this.selecterPartnerDigimons);
			int num2 = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
			if (num <= num2)
			{
				flag = true;
			}
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

	private int CalcClusterForSuccession(MonsterData baseDigimon, List<MonsterData> partnerDigimons)
	{
		int num = baseDigimon.monsterM.GetArousal() + ConstValue.SUCCESSION_BASE_COEFFICIENT;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < partnerDigimons.Count; i++)
		{
			MonsterData monsterData = partnerDigimons[i];
			if (monsterData.GetCommonSkill() != null)
			{
				num2 += int.Parse(monsterData.GetCommonSkill().inheritancePrice);
				num3 += monsterData.monsterM.GetArousal() + ConstValue.SUCCESSION_PARTNER_COEFFICIENT;
			}
		}
		return num * num2 * num3 + ConstValue.SUCCESSION_COEFFICIENT;
	}

	[Serializable]
	private class SkillTab
	{
		[SerializeField]
		[Header("スプライト")]
		private UISprite sprite;

		[Header("ラベル")]
		[SerializeField]
		private UILabelEx label;

		public void On()
		{
			this.sprite.spriteName = "Common02_Btn_tab_1";
			this.label.color = Color.white;
		}

		public void Off()
		{
			this.sprite.spriteName = "Common02_Btn_tab_2";
			this.label.color = Color.gray;
		}

		public void SetLabel(string text)
		{
			this.label.text = text;
		}

		public void SetActive(bool value)
		{
			this.sprite.gameObject.SetActive(value);
			this.label.gameObject.SetActive(value);
		}
	}
}
