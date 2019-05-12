using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CMD_10gashaResult : CMD
{
	public static CMD_10gashaResult instance;

	[SerializeField]
	private List<GameObject> goMN_ICON_LIST;

	[SerializeField]
	private GameObject rareGashaBG;

	[SerializeField]
	private GameObject linkGashaBG;

	[SerializeField]
	private UILabel ngTX_LINK_POINT;

	[SerializeField]
	private UILabel ngTX_STONE_NUM;

	[SerializeField]
	private UILabel ngTX_EXP_SINGLE;

	[SerializeField]
	private UILabel ngTX_EXP_TEN;

	[Header("シングルキャプチャボタンSprite")]
	[SerializeField]
	private UISprite buttonSpriteSingle;

	[SerializeField]
	[Header("10連キャプチャボタンSprite")]
	private UISprite buttonSpriteTen;

	[Header("シングルキャプチャボタンGUICollider")]
	[SerializeField]
	private GUICollider buttonColliderSingle;

	[SerializeField]
	[Header("10連キャプチャボタンGUICollider")]
	private GUICollider buttonColliderTen;

	[SerializeField]
	private GameObject goCAMPAIGN_1;

	[SerializeField]
	private GameObject goCAMPAIGN_10;

	[SerializeField]
	private UILabel lbCAMPAIGN_1;

	[SerializeField]
	private UILabel lbCAMPAIGN_10;

	[SerializeField]
	private PrizeEfcCont prizeEfcCont;

	[SerializeField]
	private PrizeEfcDirector prizeEfcDirector;

	private List<GUIMonsterIcon> monsterIconList = new List<GUIMonsterIcon>();

	private List<UISpriteAnimation> shiningList = new List<UISpriteAnimation>();

	public static int GashaType { get; set; }

	public static List<MonsterData> DataList { get; set; }

	public static GameWebAPI.RespDataGA_ExecGacha.GachaRewardsData RewardsData { get; set; }

	protected override void Awake()
	{
		CMD_10gashaResult.instance = this;
		base.Awake();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.ShowDetails();
		CMD_GashaTOP.removeExceededGasha();
		base.Show(f, sizeX, sizeY, aT);
	}

	private void ShowDetails()
	{
		foreach (GUIMonsterIcon guimonsterIcon in this.monsterIconList)
		{
			UnityEngine.Object.Destroy(guimonsterIcon.gameObject);
		}
		this.monsterIconList.Clear();
		foreach (UISpriteAnimation uispriteAnimation in this.shiningList)
		{
			UnityEngine.Object.Destroy(uispriteAnimation.gameObject);
		}
		this.rareGashaBG.SetActive(true);
		this.linkGashaBG.SetActive(false);
		if (this.prizeEfcDirector != null)
		{
			this.prizeEfcDirector.SetTypeParam(CMD_10gashaResult.DataList);
		}
		this.ShowMonsterIcons();
		this.ShowPointData();
		this.SettingButton();
		if (this.prizeEfcCont != null)
		{
			this.prizeEfcCont.gameObject.SetActive(false);
		}
		CMD_10gashaResult.DataList = null;
		this.ShowCampaign();
		this.ShowPlayCount();
	}

	private void ShowCampaign()
	{
		if (DataMng.Instance().RespDataCP_Campaign == null || GashaTutorialMode.TutoExec)
		{
			this.goCAMPAIGN_1.SetActive(false);
			this.goCAMPAIGN_10.SetActive(false);
			return;
		}
		if (CMD_GashaTOP.instance != null)
		{
			GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo = CMD_GashaTOP.instance.GetGashaInfo();
			if (gashaInfo != null)
			{
				if (string.IsNullOrEmpty(gashaInfo.appealText1) || gashaInfo.appealText1 == "null")
				{
					this.goCAMPAIGN_1.SetActive(false);
					this.lbCAMPAIGN_1.text = string.Empty;
				}
				else
				{
					this.goCAMPAIGN_1.SetActive(true);
					this.lbCAMPAIGN_1.text = gashaInfo.appealText1;
				}
				if (string.IsNullOrEmpty(gashaInfo.appealText10) || gashaInfo.appealText10 == "null")
				{
					this.goCAMPAIGN_10.SetActive(false);
					this.lbCAMPAIGN_10.text = string.Empty;
				}
				else
				{
					this.goCAMPAIGN_10.SetActive(true);
					this.lbCAMPAIGN_10.text = gashaInfo.appealText10;
				}
			}
		}
	}

	private void ShowPlayCount()
	{
		if (CMD_GashaTOP.instance != null)
		{
			GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo = CMD_GashaTOP.instance.GetGashaInfo();
			int num = int.Parse(gashaInfo.totalPlayLimitCount);
			int num2 = num - int.Parse(gashaInfo.totalPlayCount);
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num > 0)
			{
				this.goCAMPAIGN_1.SetActive(true);
				this.goCAMPAIGN_10.SetActive(true);
				string @string = StringMaster.GetString("RemainingPlayCount");
				string text = string.Format(@string, num2 / 1);
				if (this.lbCAMPAIGN_1.text.Length > 0)
				{
					text = text + "\n" + this.lbCAMPAIGN_1.text;
				}
				this.lbCAMPAIGN_1.text = text;
				text = string.Format(@string, num2 / 10);
				if (this.lbCAMPAIGN_10.text.Length > 0)
				{
					text = text + "\n" + this.lbCAMPAIGN_10.text;
				}
				this.lbCAMPAIGN_10.text = text;
			}
		}
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_10gashaResult.DataList = null;
		CMD_10gashaResult.instance = null;
	}

	public void ShowPointData()
	{
		if (CMD_GashaTOP.instance != null)
		{
			this.ngTX_LINK_POINT.text = CMD_GashaTOP.instance.LinkPointString;
			this.ngTX_STONE_NUM.text = CMD_GashaTOP.instance.StoneNumString;
			this.ngTX_EXP_SINGLE.text = CMD_GashaTOP.instance.NeedSingleNumString;
			this.ngTX_EXP_TEN.text = CMD_GashaTOP.instance.NeedTenNumString;
		}
	}

	private void SettingButton()
	{
		if (CMD_10gashaResult.GashaType == ConstValue.RARE_GASHA_TYPE)
		{
			this.buttonSpriteSingle.spriteName = "Common02_Btn_Blue";
			this.buttonColliderSingle.activeCollider = true;
			this.buttonSpriteTen.spriteName = "Common02_Btn_Red";
			this.buttonColliderTen.activeCollider = true;
		}
		else if (CMD_10gashaResult.GashaType == ConstValue.LINK_GASHA_TYPE)
		{
			int num = 0;
			int num2 = 0;
			if (CMD_GashaTOP.instance != null)
			{
				num = CMD_GashaTOP.instance.SingleNeedCount.ToInt32();
				num2 = CMD_GashaTOP.instance.TenNeedCount.ToInt32();
			}
			int num3 = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.friendPoint.ToInt32();
			this.buttonSpriteSingle.spriteName = ((num3 < num) ? "Common02_Btn_Gray" : "Common02_Btn_Blue");
			this.buttonColliderSingle.activeCollider = (num3 >= num);
			this.buttonSpriteTen.spriteName = ((num3 < num2) ? "Common02_Btn_Gray" : "Common02_Btn_Red");
			this.buttonColliderTen.activeCollider = (num3 >= num2);
		}
		if (CMD_GashaTOP.instance != null)
		{
			GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo = CMD_GashaTOP.instance.GetGashaInfo();
			if (gashaInfo != null && int.Parse(gashaInfo.totalPlayLimitCount) > 0)
			{
				int num4 = int.Parse(gashaInfo.totalPlayLimitCount) - int.Parse(gashaInfo.totalPlayCount);
				if (num4 < 1)
				{
					this.buttonSpriteSingle.spriteName = "Common02_Btn_Gray";
					this.buttonColliderSingle.activeCollider = false;
				}
				if (num4 < 10)
				{
					this.buttonSpriteTen.spriteName = "Common02_Btn_Gray";
					this.buttonColliderTen.activeCollider = false;
				}
			}
		}
	}

	private void ShowMonsterIcons()
	{
		for (int i = 0; i < this.goMN_ICON_LIST.Count; i++)
		{
			if (i < CMD_10gashaResult.DataList.Count)
			{
				Transform transform = this.goMN_ICON_LIST[i].transform;
				GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(CMD_10gashaResult.DataList[i], transform.localScale, transform.localPosition, transform.parent, true, false);
				this.monsterIconList.Add(guimonsterIcon);
				guimonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconLong));
				guimonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.ActMIconLong));
				UIWidget component = guimonsterIcon.gameObject.GetComponent<UIWidget>();
				if (this.prizeEfcDirector != null && this.prizeEfcCont != null)
				{
					this.prizeEfcCont.gameObject.SetActive(true);
					PrizeEfcCont component2 = UnityEngine.Object.Instantiate<GameObject>(this.prizeEfcCont.gameObject).GetComponent<PrizeEfcCont>();
					component2.prizeEfcDir = this.prizeEfcDirector;
					component2.transform.parent = guimonsterIcon.transform;
					component2.transform.localPosition = Vector3.zero;
					component2.transform.localScale = Vector3.one;
					component2.Data = CMD_10gashaResult.DataList[i];
					component2.SetParam();
					if (CMD_GashaTOP.instance != null)
					{
						CMD_GashaTOP.instance.SetFinishedActionCutScene_2(new Action(this.FadeInEndAction));
					}
					UIWidget component3 = component2.gameObject.GetComponent<UIWidget>();
					if (component3 != null && component != null)
					{
						int add = component.depth - component3.depth;
						DepthController component4 = component2.gameObject.GetComponent<DepthController>();
						component4.AddWidgetDepth(component4.transform, add);
					}
				}
				UIWidget component5 = this.goMN_ICON_LIST[i].GetComponent<UIWidget>();
				if (component5 != null && component != null)
				{
					int add2 = component5.depth - component.depth;
					DepthController component6 = guimonsterIcon.gameObject.GetComponent<DepthController>();
					component6.AddWidgetDepth(guimonsterIcon.transform, add2);
				}
				this.goMN_ICON_LIST[i].SetActive(false);
			}
			else
			{
				this.goMN_ICON_LIST[i].SetActive(false);
			}
		}
		MonsterDataMng.Instance().UnnewMonserDataList();
	}

	private void ActMIconLong(MonsterData md)
	{
		CMD_CharacterDetailed.DataChg = md;
		GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed");
	}

	private void FadeInEndAction()
	{
		this.prizeEfcDirector.KickEfc();
		if (CMD_10gashaResult.RewardsData != null)
		{
			this.GashaRewardSet();
		}
	}

	private void GashaRewardSet()
	{
		GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(CMD_10gashaResult.RewardsData.assetCategoryId);
		MasterDataMng.AssetCategory assetCategoryId = (MasterDataMng.AssetCategory)int.Parse(CMD_10gashaResult.RewardsData.assetCategoryId);
		CMD_CaptureBonus cmd_CaptureBonus = GUIMain.ShowCommonDialog(null, "CMD_CaptureBonus") as CMD_CaptureBonus;
		cmd_CaptureBonus.DialogDataSet(assetCategory, assetCategoryId, CMD_10gashaResult.RewardsData);
	}

	private void OnClickSingle()
	{
		if (CMD_GashaTOP.instance != null)
		{
			CMD_GashaTOP.instance.OnClickedSingle();
		}
	}

	private void OnClick10()
	{
		if (CMD_GashaTOP.instance != null)
		{
			CMD_GashaTOP.instance.OnClicked10();
		}
	}

	public void ReShow()
	{
		this.ShowDetails();
		CMD_GashaTOP.removeExceededGasha();
	}
}
