using Master;
using System;
using System.Collections.Generic;
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
	private MonsterBasicInfo materialMonsterBasicInfo;

	[SerializeField]
	private MonsterLearnSkill materialMonsterSuccessionSkill;

	[SerializeField]
	[Header("所持クラスタ数")]
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

	[SerializeField]
	[Header("表示デジモン数")]
	private UILabel ngTX_MN_HAVE;

	[SerializeField]
	[Header("ソートのラベル")]
	private UILabel ngTX_SORT_DISP;

	[SerializeField]
	private UILabel sortButtonLabel;

	[SerializeField]
	private UILabel necessaryLabel;

	private GUIMonsterIcon leftLargeMonsterIcon;

	private int useClusterBK;

	private int materialMonsterGroupId;

	private GameObject goSelectPanelMonsterIcon;

	private GUISelectPanelMonsterIcon csSelectPanelMonsterIcon;

	private List<MonsterData> deckMDList;

	private MonsterData baseDigimon;

	private List<MonsterData> selecterPartnerDigimons = new List<MonsterData>();

	protected override void Awake()
	{
		base.Awake();
		for (int i = 0; i < this.goMN_ICON_LIST.Count; i++)
		{
			this.goMN_ICON_LIST[i].SetActive(false);
		}
		PartyUtil.ActMIconShort = new Action<MonsterData>(this.ActMIconShort);
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_Succession");
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.PartsTitle.SetTitle(StringMaster.GetString("SuccessionTitle"));
		this.ngTX_DECIDE.text = StringMaster.GetString("SuccessionTitle");
		this.sortButtonLabel.text = StringMaster.GetString("SystemSortButton");
		this.necessaryLabel.text = StringMaster.GetString("Succession-02");
		this.SetCommonUI();
		this.InitMonsterList(true);
		this.ShowChgInfo();
		this.ShowMATInfo();
		this.UpdateClusterNum();
		this.ShowHaveMonster();
		base.Show(f, sizeX, sizeY, aT);
		RestrictionInput.EndLoad();
	}

	protected override void WindowClosed()
	{
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		if (monsterDataMng != null)
		{
			monsterDataMng.PushBackAllMonsterPrefab();
		}
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
		cmd_InheritCheck.SetParams(this.selecterPartnerDigimons, this.useClusterLabel.text);
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
		return this.baseDigimon.userMonster.commonSkillId == this.selecterPartnerDigimons[0].userMonster.commonSkillId;
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
		int[] mat = new int[this.selecterPartnerDigimons.Count];
		for (int i = 0; i < this.selecterPartnerDigimons.Count; i++)
		{
			mat[i] = int.Parse(this.selecterPartnerDigimons[i].userMonster.userMonsterId);
		}
		this.materialMonsterGroupId = int.Parse(this.selecterPartnerDigimons[0].monsterM.monsterGroupId);
		this.useClusterBK = CalculatorUtil.CalcClusterForSuccession(this.baseDigimon, this.selecterPartnerDigimons);
		GameWebAPI.RequestMN_MonsterInheritance requestMN_MonsterInheritance = new GameWebAPI.RequestMN_MonsterInheritance();
		requestMN_MonsterInheritance.SetSendData = delegate(GameWebAPI.MN_Req_Success param)
		{
			param.baseUserMonsterId = int.Parse(this.baseDigimon.userMonster.userMonsterId);
			param.materialUserMonsterId = mat[0];
		};
		requestMN_MonsterInheritance.OnReceived = delegate(GameWebAPI.RespDataMN_SuccessExec response)
		{
			if (response.userMonsterList != null)
			{
				DataMng.Instance().SetUserMonsterList(response.userMonsterList);
			}
			if (response.userMonster != null)
			{
				DataMng.Instance().SetUserMonster(response.userMonster);
			}
		};
		GameWebAPI.RequestMN_MonsterInheritance request = requestMN_MonsterInheritance;
		base.StartCoroutine(request.Run(new Action(this.EndSuccession), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private void EndSuccession()
	{
		int[] array = new int[this.selecterPartnerDigimons.Count];
		for (int i = 0; i < this.selecterPartnerDigimons.Count; i++)
		{
			array[i] = int.Parse(this.selecterPartnerDigimons[i].userMonster.userMonsterId);
		}
		DataMng.Instance().DeleteUserMonsterList(array);
		MonsterDataMng.Instance().RefreshMonsterDataList();
		this.InitMonsterList(false);
		int item = int.Parse(this.baseDigimon.monsterM.monsterGroupId);
		List<int> umidList = new List<int>
		{
			item
		};
		List<int> umidList2 = new List<int>
		{
			this.materialMonsterGroupId
		};
		Loading.Invisible();
		CutSceneMain.FadeReqCutScene("Cutscenes/Inheritance", new Action<int>(this.StartCutSceneCallBack), delegate(int index)
		{
			CutSceneMain.FadeReqCutSceneEnd();
		}, delegate(int index)
		{
			if (PartsUpperCutinController.Instance != null)
			{
				PartsUpperCutinController.Instance.PlayAnimator(PartsUpperCutinController.AnimeType.InheritanceComplete, null);
			}
			RestrictionInput.EndLoad();
		}, umidList, umidList2, 3, 1, 0.5f, 0.5f);
	}

	private void StartCutSceneCallBack(int i)
	{
		this.baseDigimon.csMIcon.Data = this.baseDigimon;
		this.baseDigimon.csMIcon.DimmLevel = GUIMonsterIcon.DIMM_LEVEL.ACTIVE;
		this.baseDigimon.csMIcon.SelectNum = -1;
		GUIMonsterIcon monsterCS_ByMonsterData = MonsterDataMng.Instance().GetMonsterCS_ByMonsterData(this.baseDigimon);
		monsterCS_ByMonsterData.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
		this.baseDigimon.dimmLevel = GUIMonsterIcon.DIMM_LEVEL.DISABLE;
		monsterCS_ByMonsterData.DimmLevel = this.baseDigimon.dimmLevel;
		this.baseDigimon.selectNum = 0;
		monsterCS_ByMonsterData.SelectNum = this.baseDigimon.selectNum;
		for (int j = 0; j < this.selecterPartnerDigimons.Count; j++)
		{
			UnityEngine.Object.DestroyImmediate(this.selecterPartnerDigimons[j].csMIcon.gameObject);
			this.goMN_ICON_MAT_LIST[j].SetActive(true);
		}
		this.selecterPartnerDigimons = new List<MonsterData>();
		DataMng.Instance().US_PlayerInfoSubChipNum(this.useClusterBK);
		this.UpdateClusterNum();
		this.ShowChgInfo();
		this.ShowMATInfo();
		this.SetDimParty(true);
		this.CheckDimRightPartners(true);
		this.BtnCont();
		this.ShowHaveMonster();
		this.ActMIconLong(this.baseDigimon);
	}

	private void ShowChgInfo()
	{
		if (this.baseDigimon != null)
		{
			this.baseMonsterBasicInfo.SetMonsterData(this.baseDigimon);
			this.baseMonsterSuccessionSkill.SetSkill(this.baseDigimon);
		}
		else
		{
			this.baseMonsterBasicInfo.ClearMonsterData();
			this.baseMonsterSuccessionSkill.ClearSkill();
		}
	}

	private void ShowMATInfo()
	{
		if (this.selecterPartnerDigimons.Count > 0 && this.selecterPartnerDigimons[0] != null)
		{
			MonsterData monsterData = this.selecterPartnerDigimons[0];
			this.materialMonsterBasicInfo.SetMonsterData(monsterData);
			this.materialMonsterSuccessionSkill.SetSkill(monsterData);
			int num = CalculatorUtil.CalcClusterForSuccession(this.baseDigimon, this.selecterPartnerDigimons);
			this.useClusterLabel.text = StringFormat.Cluster(num);
			if (num > int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney))
			{
				this.useClusterLabel.color = Color.red;
			}
			else
			{
				this.useClusterLabel.color = Color.white;
			}
		}
		else
		{
			this.materialMonsterBasicInfo.ClearMonsterData();
			this.materialMonsterSuccessionSkill.ClearSkill();
			this.useClusterLabel.text = "0";
			this.useClusterLabel.color = Color.white;
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
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		monsterDataMng.ClearSortMessAll();
		monsterDataMng.ClearLevelMessAll();
		List<MonsterData> list = monsterDataMng.GetMonsterDataList(false);
		list = monsterDataMng.SelectMonsterDataList(list, MonsterDataMng.SELECT_TYPE.ALL_OUT_GARDEN);
		list = monsterDataMng.SortMDList(list, false);
		this.csSelectPanelMonsterIcon.initLocation = initLoc;
		Vector3 localScale = this.goMN_ICON_LIST[0].transform.localScale;
		monsterDataMng.SetDimmAll(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
		monsterDataMng.SetSelectOffAll();
		monsterDataMng.ClearDimmMessAll();
		this.csSelectPanelMonsterIcon.useLocationRecord = true;
		this.csSelectPanelMonsterIcon.AllBuild(list, localScale, new Action<MonsterData>(this.ActMIconLong), new Action<MonsterData>(this.ActMIconShort), false);
		this.deckMDList = MonsterDataMng.Instance().GetDeckMonsterDataList(false);
	}

	private void ActMIconLong(MonsterData tappedMonsterData)
	{
		CMD_CharacterDetailed.DataChg = tappedMonsterData;
		bool flag = false;
		bool isCheckDim = true;
		foreach (MonsterData monsterData in this.selecterPartnerDigimons)
		{
			if (monsterData == tappedMonsterData)
			{
				flag = true;
				isCheckDim = false;
			}
		}
		if (this.baseDigimon == null)
		{
			isCheckDim = false;
		}
		if (tappedMonsterData == this.baseDigimon)
		{
			isCheckDim = false;
		}
		foreach (MonsterData monsterData2 in this.deckMDList)
		{
			if (monsterData2 == tappedMonsterData)
			{
				isCheckDim = false;
			}
		}
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(delegate(int i)
		{
			if (tappedMonsterData == this.baseDigimon)
			{
				this.leftLargeMonsterIcon.Lock = tappedMonsterData.userMonster.IsLocked;
			}
			PartyUtil.SetLock(tappedMonsterData, isCheckDim);
		}, "CMD_CharacterDetailed") as CMD_CharacterDetailed;
		if (flag)
		{
			cmd_CharacterDetailed.Mode = CMD_CharacterDetailed.LockMode.Succession;
		}
	}

	private void ActMIconShort(MonsterData md)
	{
		if (this.baseDigimon == null)
		{
			this.baseDigimon = md;
			this.leftLargeMonsterIcon = this.ShowCreateIcon(this.baseDigimon, this.goMN_ICON_CHG);
			this.baseDigimon.csMIcon = this.leftLargeMonsterIcon;
			this.leftLargeMonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
			this.ShowChgInfo();
			this.SetDimParty(true);
			this.CheckDimRightPartners(true);
		}
		else if (this.selecterPartnerDigimons.Count < 1)
		{
			this.selecterPartnerDigimons.Add(md);
			int index = this.selecterPartnerDigimons.Count - 1;
			GUIMonsterIcon guimonsterIcon = this.ShowCreateIcon(this.selecterPartnerDigimons[index], this.goMN_ICON_MAT_LIST[index]);
			this.selecterPartnerDigimons[index].csMIcon = guimonsterIcon;
			this.ShowMATInfo();
			guimonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
		}
		else
		{
			this.ActMIconS_Remove(this.selecterPartnerDigimons[0]);
			this.selecterPartnerDigimons.Add(md);
			int index2 = this.selecterPartnerDigimons.Count - 1;
			GUIMonsterIcon guimonsterIcon2 = this.ShowCreateIcon(this.selecterPartnerDigimons[index2], this.goMN_ICON_MAT_LIST[index2]);
			this.selecterPartnerDigimons[index2].csMIcon = guimonsterIcon2;
			this.ShowMATInfo();
			guimonsterIcon2.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
		}
		this.RefreshSelectedInMonsterList();
		this.BtnCont();
	}

	private void ActMIconS_Remove(MonsterData md)
	{
		if (md == this.baseDigimon)
		{
			this.DeleteIcon(this.baseDigimon, this.goMN_ICON_CHG);
			this.baseDigimon = null;
			this.ShowChgInfo();
			this.SetDimParty(false);
			this.CheckDimRightPartners(false);
		}
		else
		{
			for (int i = 0; i < this.selecterPartnerDigimons.Count; i++)
			{
				if (this.selecterPartnerDigimons[i] == md)
				{
					this.DeleteIcon(this.selecterPartnerDigimons[i], this.goMN_ICON_MAT_LIST[i]);
					this.selecterPartnerDigimons.RemoveAt(i);
					this.ShiftMatIcon(i);
				}
			}
			this.ShowMATInfo();
		}
		this.RefreshSelectedInMonsterList();
		this.BtnCont();
	}

	private void SetDimParty(bool flg)
	{
		for (int i = 0; i < this.deckMDList.Count; i++)
		{
			MonsterData deckMonsterData = this.deckMDList[i];
			if (flg)
			{
				if (this.deckMDList[i] != this.baseDigimon)
				{
					PartyUtil.SetDimIcon(true, deckMonsterData, StringMaster.GetString("CharaIcon-04"), false);
				}
			}
			else
			{
				PartyUtil.SetDimIcon(false, deckMonsterData, string.Empty, false);
			}
		}
	}

	private void CheckDimRightPartners(bool isDim)
	{
		MonsterDataMng monsterDataMng = MonsterDataMng.Instance();
		if (isDim)
		{
			bool isDimCheck = false;
			if (this.baseDigimon != null)
			{
				isDimCheck = true;
			}
			List<MonsterData> monsterDataList = monsterDataMng.GetMonsterDataList(false);
			foreach (MonsterData monsterData in monsterDataList)
			{
				if (monsterData.userMonster.IsLocked)
				{
					if (monsterData == this.baseDigimon)
					{
						PartyUtil.SetLock(monsterData, false);
					}
					else
					{
						PartyUtil.SetLock(monsterData, isDimCheck);
					}
				}
			}
		}
		else
		{
			List<MonsterData> monsterDataList2 = monsterDataMng.GetMonsterDataList(false);
			foreach (MonsterData monsterData2 in monsterDataList2)
			{
				if (this.selecterPartnerDigimons.Count <= 0 || monsterData2 != this.selecterPartnerDigimons[0])
				{
					PartyUtil.SetDimIcon(false, monsterData2, string.Empty, false);
				}
			}
		}
	}

	private GUIMonsterIcon ShowCreateIcon(MonsterData md, GameObject goEmpty)
	{
		Transform transform = goEmpty.transform;
		GUIMonsterIcon guimonsterIcon = MonsterDataMng.Instance().MakePrefabByMonsterData(md, transform.localScale, transform.localPosition, transform.parent, true, false);
		guimonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.ActMIconLong));
		md.dimmLevel = GUIMonsterIcon.DIMM_LEVEL.DISABLE;
		md.selectNum = 0;
		GUIMonsterIcon monsterCS_ByMonsterData = MonsterDataMng.Instance().GetMonsterCS_ByMonsterData(md);
		monsterCS_ByMonsterData.DimmLevel = md.dimmLevel;
		monsterCS_ByMonsterData.SelectNum = md.selectNum;
		monsterCS_ByMonsterData.SetTouchAct_S(new Action<MonsterData>(this.ActMIconS_Remove));
		guimonsterIcon.Lock = md.userMonster.IsLocked;
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

	private void DeleteIcon(MonsterData md, GameObject goEmpty)
	{
		UnityEngine.Object.DestroyImmediate(md.csMIcon.gameObject);
		goEmpty.SetActive(true);
		md.dimmLevel = GUIMonsterIcon.DIMM_LEVEL.ACTIVE;
		md.selectNum = -1;
		GUIMonsterIcon monsterCS_ByMonsterData = MonsterDataMng.Instance().GetMonsterCS_ByMonsterData(md);
		monsterCS_ByMonsterData.DimmLevel = md.dimmLevel;
		monsterCS_ByMonsterData.SelectNum = md.selectNum;
		monsterCS_ByMonsterData.SetTouchAct_S(new Action<MonsterData>(this.ActMIconShort));
	}

	private void ShiftMatIcon(int idx)
	{
		int i;
		for (i = 0; i < this.selecterPartnerDigimons.Count; i++)
		{
			this.selecterPartnerDigimons[i].csMIcon.gameObject.transform.localPosition = this.goMN_ICON_MAT_LIST[i].transform.localPosition;
			this.goMN_ICON_MAT_LIST[i].SetActive(false);
		}
		while (i < this.goMN_ICON_MAT_LIST.Count)
		{
			this.goMN_ICON_MAT_LIST[i].SetActive(true);
			i++;
		}
	}

	private void RefreshSelectedInMonsterList()
	{
		List<MonsterData> list = new List<MonsterData>();
		int snum;
		if (this.baseDigimon != null)
		{
			list.Add(this.baseDigimon);
			snum = 0;
		}
		else
		{
			snum = 1;
		}
		for (int i = 0; i < this.selecterPartnerDigimons.Count; i++)
		{
			list.Add(this.selecterPartnerDigimons[i]);
		}
		MonsterDataMng.Instance().SetSelectByMonsterDataList(list, snum, true);
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
			int num = CalculatorUtil.CalcClusterForSuccession(this.baseDigimon, this.selecterPartnerDigimons);
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

	private void ShowHaveMonster()
	{
		int count = MonsterDataMng.Instance().GetSelectMonsterDataList().Count;
		int num = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.unitLimitMax);
		this.ngTX_MN_HAVE.text = string.Format(StringMaster.GetString("SystemFraction"), count.ToString(), num.ToString());
	}
}
