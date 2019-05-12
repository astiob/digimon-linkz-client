using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_ChipGashaResult : CMD
{
	public static CMD_ChipGashaResult instance;

	[SerializeField]
	[Header("アイコン開始位置")]
	private GameObject goICON_START_POS;

	[SerializeField]
	[Header("アイコンオフセット XY")]
	private Vector2 iconOffset;

	[SerializeField]
	[Header("アイコンX方向の数")]
	private int iconNumX;

	[Header("アイコン登場時間(フレーム数)")]
	[SerializeField]
	private int showChipInterval = 16;

	[SerializeField]
	private UILabel ngTX_LINK_POINT;

	[SerializeField]
	private UILabel ngTX_STONE_NUM;

	[SerializeField]
	private UILabel ngTX_EXP_SINGLE;

	[SerializeField]
	private UILabel ngTX_EXP_TEN;

	[SerializeField]
	[Header("シングルキャプチャボタンSprite")]
	private UISprite buttonSpriteSingle;

	[SerializeField]
	[Header("10連キャプチャボタンSprite")]
	private UISprite buttonSpriteTen;

	[SerializeField]
	[Header("TOPへボタンSprite")]
	private UISprite buttonSpriteTOP;

	[Header("シングルキャプチャボタンGUICollider")]
	[SerializeField]
	private GUICollider buttonColliderSingle;

	[Header("10連キャプチャボタンGUICollider")]
	[SerializeField]
	private GUICollider buttonColliderTen;

	[SerializeField]
	private GameObject goCAMPAIGN_ROOT;

	[SerializeField]
	private GameObject goCAMPAIGN_1;

	[SerializeField]
	private GameObject goCAMPAIGN_10;

	[SerializeField]
	private UILabel lbCAMPAIGN_1;

	[SerializeField]
	private UILabel lbCAMPAIGN_10;

	[Header("BLUE エフェクト")]
	[SerializeField]
	private GameObject goEFC_BLUE;

	[Header("GOLD エフェクト")]
	[SerializeField]
	private GameObject goEFC_GOLD;

	[SerializeField]
	[Header("RAINBOW エフェクト")]
	private GameObject goEFC_RAINBOW;

	[Header("BG TEX")]
	[SerializeField]
	public UITexture txBG;

	[SerializeField]
	private float showBonusDelayTime;

	private bool isOnTapped;

	private List<ChipEfc> CHIP_EFC_LIST;

	private int curChipInitNUM;

	private int curChipFrameCT;

	public static GameWebAPI.RespDataGA_ExecChip.UserAssetList[] UserAssetList { get; set; }

	public static List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> DataList { get; set; }

	public static int GashaType { get; set; }

	public bool StartEffect { get; set; }

	public static GameWebAPI.RespDataGA_ExecGacha.GachaRewardsData[] RewardsData { get; set; }

	protected override void Awake()
	{
		CMD_ChipGashaResult.instance = this;
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
		GUICollider.DisableAllCollider("=================================== CMD_ChipGashaResult::ShoeDetail");
		this.ClearChipIcons();
		this.ShowChipIcons();
		this.ShowPointData();
		this.SettingButton();
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

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_ChipGashaResult.instance = null;
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
		if (CMD_ChipGashaResult.GashaType == ConstValue.RARE_GASHA_TYPE)
		{
			this.buttonSpriteSingle.spriteName = "Common02_Btn_Blue";
			this.buttonColliderSingle.activeCollider = true;
			this.buttonSpriteTen.spriteName = "Common02_Btn_Red";
			this.buttonColliderTen.activeCollider = true;
		}
		else if (CMD_ChipGashaResult.GashaType == ConstValue.LINK_GASHA_TYPE)
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

	private void ClearChipIcons()
	{
		if (this.CHIP_EFC_LIST != null)
		{
			for (int i = 0; i < this.CHIP_EFC_LIST.Count; i++)
			{
				UnityEngine.Object.Destroy(this.CHIP_EFC_LIST[i].gameObject);
			}
		}
		this.CHIP_EFC_LIST = new List<ChipEfc>();
		this.curChipInitNUM = 0;
		this.curChipFrameCT = 0;
		this.StartEffect = false;
		this.isOnTapped = false;
		CMD_GashaTOP.instance.SetFinishedActionCutScene_2(delegate
		{
			this.StartEffect = true;
		});
		this.buttonSpriteSingle.gameObject.SetActive(false);
		this.buttonSpriteTen.gameObject.SetActive(false);
		this.buttonSpriteTOP.gameObject.SetActive(false);
		this.goCAMPAIGN_ROOT.SetActive(false);
	}

	private void ShowChipIcons()
	{
		CMD_ChipGashaResult.<ShowChipIcons>c__AnonStorey39F <ShowChipIcons>c__AnonStorey39F = new CMD_ChipGashaResult.<ShowChipIcons>c__AnonStorey39F();
		<ShowChipIcons>c__AnonStorey39F.<>f__this = this;
		<ShowChipIcons>c__AnonStorey39F.m = 0;
		while (<ShowChipIcons>c__AnonStorey39F.m < CMD_ChipGashaResult.DataList.Count)
		{
			CMD_ChipGashaResult.<ShowChipIcons>c__AnonStorey39E <ShowChipIcons>c__AnonStorey39E = new CMD_ChipGashaResult.<ShowChipIcons>c__AnonStorey39E();
			<ShowChipIcons>c__AnonStorey39E.<>f__ref$927 = <ShowChipIcons>c__AnonStorey39F;
			<ShowChipIcons>c__AnonStorey39E.<>f__this = this;
			<ShowChipIcons>c__AnonStorey39E.chipM = ChipDataMng.GetChipMainData(CMD_ChipGashaResult.DataList[<ShowChipIcons>c__AnonStorey39F.m].chipId.ToString());
			<ShowChipIcons>c__AnonStorey39E.vPos = this.goICON_START_POS.transform.localPosition;
			float num = (float)(<ShowChipIcons>c__AnonStorey39F.m % this.iconNumX);
			float num2 = (float)(<ShowChipIcons>c__AnonStorey39F.m / this.iconNumX);
			CMD_ChipGashaResult.<ShowChipIcons>c__AnonStorey39E <ShowChipIcons>c__AnonStorey39E2 = <ShowChipIcons>c__AnonStorey39E;
			<ShowChipIcons>c__AnonStorey39E2.vPos.x = <ShowChipIcons>c__AnonStorey39E2.vPos.x + this.iconOffset.x * num;
			CMD_ChipGashaResult.<ShowChipIcons>c__AnonStorey39E <ShowChipIcons>c__AnonStorey39E3 = <ShowChipIcons>c__AnonStorey39E;
			<ShowChipIcons>c__AnonStorey39E3.vPos.y = <ShowChipIcons>c__AnonStorey39E3.vPos.y + this.iconOffset.y * num2;
			<ShowChipIcons>c__AnonStorey39E.vPos.z = -5f;
			ChipDataMng.MakePrefabByChipData(<ShowChipIcons>c__AnonStorey39E.chipM, this.goICON_START_POS, <ShowChipIcons>c__AnonStorey39E.vPos, Vector3.one, delegate(ChipIcon icon)
			{
				GUIListChipParts component = icon.gameObject.GetComponent<GUIListChipParts>();
				if (component != null)
				{
					component.SetData(new GUIListChipParts.Data());
					component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
					{
						if (<ShowChipIcons>c__AnonStorey39E.<>f__this.curChipInitNUM == CMD_ChipGashaResult.DataList.Count)
						{
							CMD_QuestItemPOP.Create(<ShowChipIcons>c__AnonStorey39E.chipM);
						}
						<ShowChipIcons>c__AnonStorey39E.<>f__this.isOnTapped = true;
					};
				}
				Vector3 localScale = Vector3.one;
				GameObject gameObject = null;
				string effectType = CMD_ChipGashaResult.UserAssetList[<ShowChipIcons>c__AnonStorey39E.<>f__ref$927.m].effectType;
				switch (effectType)
				{
				case "1":
					gameObject = UnityEngine.Object.Instantiate<GameObject>(<ShowChipIcons>c__AnonStorey39E.<>f__this.goEFC_BLUE);
					break;
				case "2":
					gameObject = UnityEngine.Object.Instantiate<GameObject>(<ShowChipIcons>c__AnonStorey39E.<>f__this.goEFC_GOLD);
					break;
				case "3":
					gameObject = UnityEngine.Object.Instantiate<GameObject>(<ShowChipIcons>c__AnonStorey39E.<>f__this.goEFC_RAINBOW);
					break;
				}
				ChipEfc component2 = gameObject.GetComponent<ChipEfc>();
				component2.enabled = true;
				localScale = gameObject.transform.localScale;
				gameObject.transform.parent = <ShowChipIcons>c__AnonStorey39E.<>f__this.transform;
				gameObject.transform.localPosition = <ShowChipIcons>c__AnonStorey39E.vPos;
				gameObject.transform.localScale = localScale;
				localScale = icon.transform.localScale;
				icon.transform.parent = component2.goCHIP_THUMB.transform;
				icon.transform.localPosition = component2.goCHIP_THUMB.transform.localPosition;
				icon.transform.localScale = localScale;
				component2.goCHIP_THUMB.GetComponent<UITexture>().enabled = false;
				if (CMD_ChipGashaResult.UserAssetList[<ShowChipIcons>c__AnonStorey39E.<>f__ref$927.m].isNew != 1)
				{
					component2.spNew.enabled = false;
				}
				gameObject.SetActive(true);
				<ShowChipIcons>c__AnonStorey39E.<>f__this.CHIP_EFC_LIST.Add(component2);
			}, -1, -1, true);
			<ShowChipIcons>c__AnonStorey39F.m++;
		}
	}

	private void UpdateShowChipIcons()
	{
		if (!this.StartEffect)
		{
			return;
		}
		if ((this.curChipFrameCT % this.showChipInterval == 0 || this.isOnTapped) && this.curChipInitNUM < CMD_ChipGashaResult.DataList.Count)
		{
			if (this.curChipInitNUM == 0)
			{
				GUICollider.EnableAllCollider("=================================== CMD_ChipGashaResult::UpdateShowChipIcons");
			}
			this.CHIP_EFC_LIST[this.curChipInitNUM].Play();
			this.curChipInitNUM++;
			SoundMng.Instance().PlaySE("SEInternal/Farm/se_221", 0f, false, true, null, -1, 1f);
			if (this.curChipInitNUM == CMD_ChipGashaResult.DataList.Count)
			{
				this.buttonSpriteSingle.gameObject.SetActive(true);
				this.buttonSpriteTen.gameObject.SetActive(true);
				this.buttonSpriteTOP.gameObject.SetActive(true);
				this.goCAMPAIGN_ROOT.SetActive(true);
				if (CMD_ChipGashaResult.RewardsData != null)
				{
					GUICollider.DisableAllCollider("=================================== CMD_ChipGashaResult::ICON");
					AppCoroutine.Start(this.GashaRewardSet(this.showBonusDelayTime), true);
				}
			}
		}
		this.curChipFrameCT++;
	}

	private IEnumerator GashaRewardSet(float delay)
	{
		yield return new WaitForSeconds(delay);
		CMD_CaptureBonus cd = null;
		cd = (GUIMain.ShowCommonDialog(null, "CMD_CaptureBonus") as CMD_CaptureBonus);
		cd.DialogDataSet(CMD_ChipGashaResult.RewardsData);
		cd.AdjustSize();
		GUICollider.EnableAllCollider("=================================== CMD_ChipGashaResult::ICON");
		yield break;
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateShowChipIcons();
	}

	private void OnTapped()
	{
		if (!this.isOnTapped)
		{
			this.isOnTapped = true;
		}
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
