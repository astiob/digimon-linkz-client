using System;
using System.Collections;
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
	private UILabel costTip;

	[SerializeField]
	private UISprite ngBTN_DECIDE;

	[SerializeField]
	private GUICollider clBTN_DECIDE;

	[SerializeField]
	private UILabel ngTX_DECIDE;

	protected int useClusterBK;

	public MonsterData baseDigimon;

	public MonsterData partnerDigimon;

	private GameObject goBaseDigimon;

	private GameObject goPartnerDigimon;

	protected CMD_CharacterDetailed characterDetailed;

	protected override void Awake()
	{
		base.Awake();
		CMD_PairSelectBase.instance = this;
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

	protected virtual string GetTitle()
	{
		return string.Empty;
	}

	protected virtual void ClearTargetStatus()
	{
	}

	protected virtual GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList GetUserMonsterData()
	{
		return null;
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
		}, "CMD_ModalMessage", null) as CMD_ModalMessage;
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

	protected void UpdateClusterNum()
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
		}, "CMD_CharacterDetailed", null) as CMD_CharacterDetailed;
		cmd_CharacterDetailed.Mode = CMD_CharacterDetailed.LockMode.Laboratory;
	}

	protected void ActMIconLongFree(MonsterData monsterData)
	{
		CMD_CharacterDetailed.DataChg = monsterData;
		GUIMain.ShowCommonDialog(delegate(int i)
		{
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterData);
			icon.Lock = monsterData.userMonster.IsLocked;
		}, "CMD_CharacterDetailed", null);
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
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
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
			}, "CMD_BaseSelect", null);
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
			}, "CMD_BaseSelect", null);
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

	protected void RemoveEquipChip(bool destroyChip, string userMonsterId)
	{
		GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] monsterChipList = ChipDataMng.GetMonsterChipList(userMonsterId);
		if (monsterChipList != null)
		{
			foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList in monsterChipList)
			{
				if (destroyChip)
				{
					ChipDataMng.DeleteUserChipData(userChipList.userChipId);
				}
				else
				{
					userChipList.resetUserMonsterID();
				}
			}
		}
	}
}
