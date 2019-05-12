using Master;
using Monster;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_PairSelectBase : CMD
{
	public static CMD_PairSelectBase instance;

	[SerializeField]
	private LaboratoryPartsStatusDetail baseDetail;

	[SerializeField]
	private LaboratoryPartsStatusDetail partnerDetail;

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

	[SerializeField]
	[Header("ベースデジモンラベル")]
	private UILabel baseDigimonLabel;

	[Header("パートナーデジモンラベル")]
	[SerializeField]
	private UILabel partnerDigimonLabel;

	protected int useClusterBK;

	public MonsterData baseDigimon;

	public MonsterData partnerDigimon;

	private GameObject goBaseDigimon;

	private GameObject goPartnerDigimon;

	private List<BoxCollider> buttonEnableChangeList = new List<BoxCollider>();

	protected CMD_CharacterDetailed characterDetailed;

	protected override void Awake()
	{
		base.Awake();
		CMD_PairSelectBase.instance = this;
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
		CMD_PairSelectBase.instance = null;
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		this.ShowSecondTutorial();
	}

	protected virtual void ShowSecondTutorial()
	{
	}

	private void OnTouchDecide()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (this.baseDigimon != null)
		{
			flag3 = MonsterStatusData.IsVersionUp(this.baseDigimon.GetMonsterMaster().Simple.rare);
		}
		if (this.partnerDigimon != null)
		{
			flag = MonsterStatusData.IsArousal(this.partnerDigimon.monsterM.rare);
			flag2 = MonsterStatusData.IsVersionUp(this.partnerDigimon.GetMonsterMaster().Simple.rare);
		}
		Action<int> action = new Action<int>(this.OpenConfirmTargetParameter);
		if (flag2)
		{
			action = delegate(int i)
			{
				if (i == 1)
				{
					this.OpenConfirmPartnerVersionUp(new Action<int>(this.OpenConfirmTargetParameter));
				}
			};
		}
		else if (flag)
		{
			action = delegate(int i)
			{
				if (i == 1)
				{
					this.OpenConfirmPartnerArousal(new Action<int>(this.OpenConfirmTargetParameter));
				}
			};
		}
		if (base.GetType() == typeof(CMD_Laboratory) && flag3)
		{
			Action<int> _callback = action;
			action = delegate(int i)
			{
				if (i == 1)
				{
					this.OpenConfirmBaseVersionUp(_callback);
				}
			};
		}
		action(1);
	}

	protected virtual void SetTextConfirmPartnerArousal(CMD_ResearchModalAlert cd)
	{
	}

	private void OpenConfirmPartnerArousal(Action<int> onClosedPopupAction)
	{
		CMD_ResearchModalAlert cmd_ResearchModalAlert = GUIMain.ShowCommonDialog(onClosedPopupAction, "CMD_ResearchModalAlert") as CMD_ResearchModalAlert;
		cmd_ResearchModalAlert.SetDigimonIcon(this.partnerDigimon);
		this.SetTextConfirmPartnerArousal(cmd_ResearchModalAlert);
		cmd_ResearchModalAlert.AdjustSize();
	}

	protected virtual void SetTextConfirmBaseVersionUp(CMD_ResearchModalAlert cd)
	{
	}

	private void OpenConfirmBaseVersionUp(Action<int> onClosedPopupAction)
	{
		CMD_ResearchModalAlert cmd_ResearchModalAlert = GUIMain.ShowCommonDialog(onClosedPopupAction, "CMD_ResearchModalAlert") as CMD_ResearchModalAlert;
		cmd_ResearchModalAlert.SetDigimonIcon(this.baseDigimon);
		this.SetTextConfirmBaseVersionUp(cmd_ResearchModalAlert);
		cmd_ResearchModalAlert.AdjustSize();
	}

	protected virtual void SetTextConfirmPartnerVersionUp(CMD_ResearchModalAlert cd)
	{
	}

	private void OpenConfirmPartnerVersionUp(Action<int> onClosedPopupAction)
	{
		CMD_ResearchModalAlert cmd_ResearchModalAlert = GUIMain.ShowCommonDialog(onClosedPopupAction, "CMD_ResearchModalAlert") as CMD_ResearchModalAlert;
		cmd_ResearchModalAlert.SetDigimonIcon(this.partnerDigimon);
		this.SetTextConfirmPartnerVersionUp(cmd_ResearchModalAlert);
		cmd_ResearchModalAlert.AdjustSize();
	}

	protected virtual void OpenConfirmTargetParameter(int selectButtonIndex)
	{
	}

	protected void OnCloseConfirm(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			DataMng.Instance().CheckCampaign(new Action<int>(this.DoExec), new GameWebAPI.RespDataCP_Campaign.CampaignType[]
			{
				GameWebAPI.RespDataCP_Campaign.CampaignType.MedalTakeOverUp
			});
		}
	}

	protected virtual void DoExec(int result)
	{
	}

	protected IEnumerator GetChipSlotInfo()
	{
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] monsterList = new GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[]
		{
			this.GetUserMonsterData()
		};
		GameWebAPI.MonsterSlotInfoListLogic request = ChipDataMng.RequestAPIMonsterSlotInfo(monsterList);
		yield return AppCoroutine.Start(request.Run(new Action(this.EndSuccess), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(true);
		}, null), false);
		yield break;
	}

	protected virtual void EndSuccess()
	{
	}

	protected void DisableCutinButton(Transform t)
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

	protected void EnableCutinButton()
	{
		for (int i = 0; i < this.buttonEnableChangeList.Count; i++)
		{
			this.buttonEnableChangeList[i].enabled = true;
		}
		this.buttonEnableChangeList.Clear();
	}

	protected virtual string GetTitle()
	{
		return string.Empty;
	}

	protected virtual string GetStoreChipInfo()
	{
		return string.Empty;
	}

	protected void ShowStoreChipDialog(bool hasChip)
	{
		if (hasChip)
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int i)
			{
				RestrictionInput.EndLoad();
			}, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = this.GetTitle();
			cmd_ModalMessage.Info = this.GetStoreChipInfo();
			this.EnableCutinButton();
			PartsMenu partsMenu = UnityEngine.Object.FindObjectOfType<PartsMenu>();
			if (null != partsMenu)
			{
				partsMenu.SetEnableMenuButton(true);
			}
		}
	}

	protected void StartCutSceneCallBack()
	{
		this.RemoveBaseDigimon();
		this.RemovePartnerDigimon();
		this.ClearTargetStatus();
		DataMng.Instance().US_PlayerInfoSubChipNum(this.useClusterBK);
		this.UpdateClusterNum();
		GUIPlayerStatus.RefreshParams_S(false);
		string userMonsterId = this.GetUserMonsterData().userMonsterId;
		MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(userMonsterId, false);
		this.AddButton();
		CMD_CharacterDetailed.DataChg = monsterDataByUserMonsterID;
		this.characterDetailed = (GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed") as CMD_CharacterDetailed);
	}

	protected virtual void ClearTargetStatus()
	{
	}

	protected virtual GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList GetUserMonsterData()
	{
		return null;
	}

	protected virtual void AddButton()
	{
	}

	protected virtual int CalcCluster()
	{
		return 0;
	}

	private void ShowMATInfo_0()
	{
		if (this.baseDetail != null && this.baseDetail.gameObject.activeSelf)
		{
			this.baseDetail.ShowMATInfo(this.baseDigimon);
			this.costTip.text = StringFormat.Cluster(this.CalcCluster());
		}
	}

	protected virtual void ShowMATInfo_1()
	{
		if (this.partnerDetail != null && this.partnerDetail.gameObject.activeSelf)
		{
			this.partnerDetail.ShowMATInfo(this.partnerDigimon);
			this.costTip.text = StringFormat.Cluster(this.CalcCluster());
		}
	}

	protected virtual void SetTargetStatus()
	{
	}

	protected virtual bool TargetStatusReady()
	{
		return this.baseDigimon != null && this.partnerDigimon != null;
	}

	private void ShowCHGInfo()
	{
		if (this.TargetStatusReady())
		{
			this.SetTargetStatus();
		}
		else
		{
			this.ClearTargetStatus();
		}
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_PairSelectBase");
		base.HideDLG();
		base.PartsTitle.SetTitle(this.GetTitle());
		this.ShowMATInfo_0();
		this.ShowMATInfo_1();
		this.ClearTargetStatus();
		this.UpdateClusterNum();
		if (!this.CanEnter())
		{
			base.StartCoroutine(this.OpenMaxMessage());
		}
		else
		{
			base.ShowDLG();
			base.Show(f, sizeX, sizeY, aT);
		}
	}

	protected virtual bool CanEnter()
	{
		return true;
	}

	protected virtual string GetInfoCannotEnter()
	{
		return string.Empty;
	}

	private IEnumerator OpenMaxMessage()
	{
		yield return null;
		GUICollider.EnableAllCollider("CMD_PairSelectBase");
		CMD_ModalMessage cd = GUIMain.ShowCommonDialog(delegate(int i)
		{
			this.ClosePanel(false);
		}, "CMD_ModalMessage") as CMD_ModalMessage;
		cd.Title = this.GetTitle();
		cd.Info = this.GetInfoCannotEnter();
		yield break;
	}

	protected virtual bool CanSelectMonster(int idx)
	{
		return true;
	}

	protected virtual void OpenCanNotSelectMonsterPop()
	{
	}

	private void UpdateClusterNum()
	{
		this.possessionTip.text = StringFormat.Cluster(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
	}

	protected void ActMIconLong(MonsterData monsterData)
	{
		CMD_CharacterDetailed.DataChg = monsterData;
		CMD_CharacterDetailed cmd_CharacterDetailed = GUIMain.ShowCommonDialog(delegate(int i)
		{
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterData);
			icon.Lock = monsterData.userMonster.IsLocked;
		}, "CMD_CharacterDetailed") as CMD_CharacterDetailed;
		cmd_CharacterDetailed.Mode = CMD_CharacterDetailed.LockMode.Laboratory;
	}

	protected void ActMIconLongFree(MonsterData monsterData)
	{
		CMD_CharacterDetailed.DataChg = monsterData;
		GUIMain.ShowCommonDialog(delegate(int i)
		{
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterData);
			icon.Lock = monsterData.userMonster.IsLocked;
		}, "CMD_CharacterDetailed");
	}

	protected virtual void SetBaseTouchAct_L(GUIMonsterIcon cs)
	{
	}

	protected virtual void SetPartnerTouchAct_L(GUIMonsterIcon cs)
	{
	}

	protected virtual void SetBaseSelectType()
	{
	}

	protected virtual void OnBaseSelected()
	{
	}

	private void OnTappedMAT_0()
	{
		if (!this.CanEnter())
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = this.GetTitle();
			cmd_ModalMessage.Info = this.GetInfoCannotEnter();
			return;
		}
		bool flag = this.CanSelectMonster(0);
		if (flag)
		{
			this.SetBaseSelectType();
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
					this.OnBaseSelected();
				}
			}, "CMD_BaseSelect");
			commonDialog.SetForceReturnValue(0);
		}
		else
		{
			this.OpenCanNotSelectMonsterPop();
		}
	}

	protected virtual void OpenBaseDigimonNonePop()
	{
	}

	private void OnTappedMAT_1()
	{
		if (this.baseDigimon == null)
		{
			this.OpenBaseDigimonNonePop();
			return;
		}
		bool flag = this.CanSelectMonster(1);
		if (flag)
		{
			this.SetBaseSelectType();
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
			this.OpenCanNotSelectMonsterPop();
		}
	}

	private GameObject SetSelectedCharChg(MonsterData md, GameObject goEmpty, GameObject goIcon, int inum)
	{
		if (md != null)
		{
			if (goIcon != null)
			{
				UnityEngine.Object.DestroyImmediate(goIcon);
			}
			GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(md, goEmpty.transform.localScale, goEmpty.transform.localPosition, goEmpty.transform.parent, true, false);
			goIcon = guimonsterIcon.gameObject;
			goIcon.SetActive(true);
			guimonsterIcon.Data = md;
			if (inum == 0)
			{
				guimonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconShort_0));
				this.SetBaseTouchAct_L(guimonsterIcon);
			}
			else if (inum == 1)
			{
				guimonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconShort_1));
				this.SetPartnerTouchAct_L(guimonsterIcon);
			}
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

	protected virtual bool CheckMaterial()
	{
		return true;
	}

	protected void BtnCont()
	{
		bool flag = false;
		if (this.TargetStatusReady())
		{
			int num = this.CalcCluster();
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
		bool flag2 = this.CheckMaterial();
		if (flag2 && flag)
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
		if (this.partnerDetail != null)
		{
			this.partnerDetail.SetActiveIcon(true);
		}
		this.ShowMATInfo_1();
		this.ShowCHGInfo();
		this.BtnCont();
	}
}
