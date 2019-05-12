using System;
using System.Collections.Generic;
using UI.Gasha;
using UnityEngine;

public sealed class CMD_MonsterGashaResult : CMD
{
	[SerializeField]
	private GashaUserAssetsInventory assetsInventory;

	[SerializeField]
	private GashaStartButtonEvent startButton;

	[SerializeField]
	private List<GameObject> goMN_ICON_LIST;

	[SerializeField]
	private PrizeEfcCont prizeEfcCont;

	[SerializeField]
	private PrizeEfcDirector prizeEfcDirector;

	private List<GUIMonsterIcon> monsterIconList = new List<GUIMonsterIcon>();

	private List<UISpriteAnimation> shiningList = new List<UISpriteAnimation>();

	public static CMD_MonsterGashaResult instance;

	public static GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo;

	public static bool isTutorial;

	public static List<MonsterData> DataList { get; set; }

	public static List<bool> IconNewFlagList { get; set; }

	public static GameWebAPI.RespDataGA_ExecGacha.GachaRewardsData[] RewardsData { get; set; }

	protected override void Awake()
	{
		CMD_MonsterGashaResult.instance = this;
		base.Awake();
	}

	private void OnEnable()
	{
		if (base.GetActionStatus() == CommonDialog.ACT_STATUS.OPEN)
		{
			this.assetsInventory.SetGashaPriceType(CMD_MonsterGashaResult.gashaInfo.priceType);
			this.startButton.SetGashaInfo(CMD_MonsterGashaResult.gashaInfo, CMD_MonsterGashaResult.isTutorial);
			this.startButton.SetPlayButton();
		}
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.ShowDetails();
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
		if (null != this.prizeEfcDirector)
		{
			this.prizeEfcDirector.SetTypeParam(CMD_MonsterGashaResult.DataList);
		}
		this.ShowMonsterIcons();
		this.assetsInventory.SetGashaPriceType(CMD_MonsterGashaResult.gashaInfo.priceType);
		this.startButton.SetGashaInfo(CMD_MonsterGashaResult.gashaInfo, CMD_MonsterGashaResult.isTutorial);
		this.startButton.SetPlayButton();
		if (null != this.prizeEfcCont)
		{
			this.prizeEfcCont.gameObject.SetActive(false);
		}
		if (CMD_MonsterGashaResult.gashaInfo.priceType.GetCostAssetsCategory() == MasterDataMng.AssetCategory.DIGI_STONE)
		{
			LeadReview leadReview = new LeadReview();
			leadReview.DisplayDialog(CMD_MonsterGashaResult.DataList);
		}
		CMD_MonsterGashaResult.DataList = null;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_MonsterGashaResult.DataList = null;
		CMD_MonsterGashaResult.instance = null;
	}

	private void ShowMonsterIcons()
	{
		for (int i = 0; i < this.goMN_ICON_LIST.Count; i++)
		{
			if (CMD_MonsterGashaResult.DataList.Count > i)
			{
				Transform transform = this.goMN_ICON_LIST[i].transform;
				GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(CMD_MonsterGashaResult.DataList[i], transform.localScale, transform.localPosition, transform.parent, true, false);
				guimonsterIcon.New = CMD_MonsterGashaResult.IconNewFlagList[i];
				this.monsterIconList.Add(guimonsterIcon);
				guimonsterIcon.SetTouchAct_S(new Action<MonsterData>(this.ActMIconLong));
				guimonsterIcon.SetTouchAct_L(new Action<MonsterData>(this.ActMIconLong));
				UIWidget component = guimonsterIcon.gameObject.GetComponent<UIWidget>();
				if (null != this.prizeEfcDirector && null != this.prizeEfcCont)
				{
					this.prizeEfcCont.gameObject.SetActive(true);
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prizeEfcCont.gameObject);
					PrizeEfcCont component2 = gameObject.GetComponent<PrizeEfcCont>();
					component2.prizeEfcDir = this.prizeEfcDirector;
					component2.transform.parent = guimonsterIcon.transform;
					component2.transform.localPosition = Vector3.zero;
					component2.transform.localScale = Vector3.one;
					component2.Data = CMD_MonsterGashaResult.DataList[i];
					component2.SetParam();
					UIWidget component3 = component2.gameObject.GetComponent<UIWidget>();
					if (null != component3 && null != component)
					{
						int add = component.depth - component3.depth;
						DepthController component4 = component2.gameObject.GetComponent<DepthController>();
						component4.AddWidgetDepth(component4.transform, add);
					}
				}
				UIWidget component5 = this.goMN_ICON_LIST[i].GetComponent<UIWidget>();
				if (null != component5 && null != component)
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
	}

	private void ActMIconLong(MonsterData md)
	{
		CMD_CharacterDetailed.DataChg = md;
		GUIMain.ShowCommonDialog(null, "CMD_CharacterDetailed", null);
	}

	public void FadeInEndAction()
	{
		this.prizeEfcDirector.KickEfc();
		if (CMD_MonsterGashaResult.RewardsData != null)
		{
			CMD_CaptureBonus cmd_CaptureBonus = GUIMain.ShowCommonDialog(null, "CMD_CaptureBonus", null) as CMD_CaptureBonus;
			cmd_CaptureBonus.DialogDataSet(CMD_MonsterGashaResult.RewardsData);
			cmd_CaptureBonus.AdjustSize();
		}
	}

	public static void CreateDialog()
	{
		if (null != CMD_MonsterGashaResult.instance)
		{
			CMD_MonsterGashaResult.instance.ShowDetails();
		}
		else
		{
			GUIMain.ShowCommonDialog(null, "CMD_MonsterGashaResult", null);
		}
	}
}
