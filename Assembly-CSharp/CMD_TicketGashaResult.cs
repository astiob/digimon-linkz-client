using Master;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CMD_TicketGashaResult : CMD
{
	public static CMD_TicketGashaResult instance;

	[SerializeField]
	[Header("アイコン中心位置")]
	private GameObject goICON_CENTER_POS;

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
	[Header("アイコンルート)")]
	private GameObject goICON_ROOT;

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

	[Header("TOPへボタンSprite")]
	[SerializeField]
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

	[Header("WHITE エフェクト")]
	[SerializeField]
	private GameObject goEFC_WHITE;

	[Header("GOLD エフェクト")]
	[SerializeField]
	private GameObject goEFC_GOLD;

	[SerializeField]
	[Header("RAINBOW エフェクト")]
	private GameObject goEFC_RAINBOW;

	[SerializeField]
	[Header("BG TEX")]
	public UITexture txBG;

	private bool isOnTapped;

	private List<TicketEfc> TICKET_EFC_LIST;

	private int curTicketInitNUM;

	private int curTicketFrameCT;

	public static GameWebAPI.RespDataGA_ExecTicket.UserDungeonTicketList[] UserDungeonTicketList { get; set; }

	public static int GashaType { get; set; }

	public bool StartEffect { get; set; }

	protected override void Awake()
	{
		CMD_TicketGashaResult.instance = this;
		base.Awake();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.ShowDetails();
		base.Show(f, sizeX, sizeY, aT);
	}

	private void ShowDetails()
	{
		GUICollider.DisableAllCollider("=================================== CMD_TicketGashaResult::ShoeDetail");
		this.ClearTicketIcons();
		this.ShowTicketIcons();
		this.ShowPointData();
		this.SettingButton();
		this.ShowCampaign();
	}

	private void ShowCampaign()
	{
		if (GashaTutorialMode.TutoExec)
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
				}
				else
				{
					this.goCAMPAIGN_1.SetActive(true);
					this.lbCAMPAIGN_1.text = gashaInfo.appealText1;
				}
				if (string.IsNullOrEmpty(gashaInfo.appealText10) || gashaInfo.appealText10 == "null")
				{
					this.goCAMPAIGN_10.SetActive(false);
				}
				else
				{
					this.goCAMPAIGN_10.SetActive(true);
					this.lbCAMPAIGN_10.text = gashaInfo.appealText10;
				}
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
		CMD_TicketGashaResult.instance = null;
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
		if (CMD_TicketGashaResult.GashaType == ConstValue.RARE_GASHA_TYPE)
		{
			this.buttonSpriteSingle.spriteName = "Common02_Btn_Blue";
			this.buttonColliderSingle.activeCollider = true;
			this.buttonSpriteTen.spriteName = "Common02_Btn_Red";
			this.buttonColliderTen.activeCollider = true;
		}
		else if (CMD_TicketGashaResult.GashaType == ConstValue.LINK_GASHA_TYPE)
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
	}

	private void ClearTicketIcons()
	{
		if (this.TICKET_EFC_LIST != null)
		{
			for (int i = 0; i < this.TICKET_EFC_LIST.Count; i++)
			{
				UnityEngine.Object.Destroy(this.TICKET_EFC_LIST[i].gameObject);
			}
		}
		this.TICKET_EFC_LIST = new List<TicketEfc>();
		this.curTicketInitNUM = 0;
		this.curTicketFrameCT = 0;
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

	private void ShowTicketIcons()
	{
		Vector3 zero = Vector3.zero;
		zero.x = -(this.iconOffset.x * (float)(this.iconNumX - 1) / 2f);
		zero.y = this.iconOffset.y / 2f;
		this.goICON_ROOT.transform.localPosition = this.goICON_CENTER_POS.transform.localPosition;
		int m;
		for (m = 0; m < CMD_TicketGashaResult.UserDungeonTicketList.Length; m++)
		{
			Vector3 zero2 = Vector3.zero;
			float num = (float)(m % this.iconNumX);
			float num2 = (float)(m / this.iconNumX);
			zero2.x = zero.x + this.iconOffset.x * num;
			zero2.y = zero.y - this.iconOffset.y * num2;
			zero2.z = -5f;
			Vector3 localScale = Vector3.one;
			GameObject gameObject = null;
			string effectType = CMD_TicketGashaResult.UserDungeonTicketList[m].effectType;
			switch (effectType)
			{
			case "1":
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this.goEFC_WHITE);
				break;
			case "2":
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this.goEFC_GOLD);
				break;
			case "3":
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this.goEFC_RAINBOW);
				break;
			}
			gameObject.SetActive(true);
			TicketEfc component = gameObject.GetComponent<TicketEfc>();
			component.enabled = true;
			localScale = gameObject.transform.localScale;
			gameObject.transform.parent = this.goICON_ROOT.transform;
			gameObject.transform.localPosition = zero2;
			gameObject.transform.localScale = localScale;
			GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM ticket = MasterDataMng.Instance().RespDataMA_DungeonTicketMaster.dungeonTicketM.FirstOrDefault((GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM x) => CMD_TicketGashaResult.UserDungeonTicketList[m].dungeonTicketId == x.dungeonTicketId);
			if (ticket != null)
			{
				Texture2D tex = NGUIUtil.LoadTexture(ticket.img);
				if (tex != null)
				{
					NGUIUtil.ChangeUITexture(component.ngTICKET_THUMB, tex, false);
				}
				GUICollider component2 = component.ngTICKET_THUMB.gameObject.GetComponent<GUICollider>();
				if (component2 != null)
				{
					component2.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
					{
						if (flag)
						{
							CMD_ticketPOP cmd_ticketPOP = GUIMain.ShowCommonDialog(delegate(int selectButton)
							{
								if (selectButton == 0 && PartsMenu.instance != null)
								{
									PartsMenu.instance.OnClickedQuestType(1);
								}
							}, "CMD_ticketPOP") as CMD_ticketPOP;
							cmd_ticketPOP.Title = ticket.name;
							cmd_ticketPOP.Info = ticket.description;
							cmd_ticketPOP.BtnTextYes = StringMaster.GetString("QuestNormal");
							cmd_ticketPOP.BtnTextNo = StringMaster.GetString("SystemButtonClose");
							if (tex != null)
							{
								NGUIUtil.ChangeUITexture(cmd_ticketPOP.txIcon, tex, false);
							}
						}
					};
				}
			}
			if (component.ngTXT_TICKET_NUM != null)
			{
				component.ngTXT_TICKET_NUM.text = string.Format(StringMaster.GetString("SystemItemCount2"), CMD_TicketGashaResult.UserDungeonTicketList[m].num.ToString());
			}
			if (CMD_TicketGashaResult.UserDungeonTicketList[m].isNew == 1)
			{
				component.spNew.enabled = true;
			}
			else
			{
				component.spNew.enabled = false;
			}
			this.TICKET_EFC_LIST.Add(component);
		}
	}

	private void UpdateShowTicketIcons()
	{
		if (!this.StartEffect)
		{
			return;
		}
		if ((this.curTicketFrameCT % this.showChipInterval == 0 || this.isOnTapped) && this.curTicketInitNUM < CMD_TicketGashaResult.UserDungeonTicketList.Length)
		{
			if (this.curTicketInitNUM == 0)
			{
				GUICollider.EnableAllCollider("=================================== CMD_TicketGashaResult::UpdateShowTicketIcons");
			}
			this.TICKET_EFC_LIST[this.curTicketInitNUM].Play();
			this.curTicketInitNUM++;
			SoundMng.Instance().PlaySE("SEInternal/Farm/se_221", 0f, false, true, null, -1, 1f);
			if (this.curTicketInitNUM == CMD_TicketGashaResult.UserDungeonTicketList.Length)
			{
				this.buttonSpriteSingle.gameObject.SetActive(true);
				this.buttonSpriteTen.gameObject.SetActive(true);
				this.buttonSpriteTOP.gameObject.SetActive(true);
				this.goCAMPAIGN_ROOT.SetActive(true);
			}
		}
		this.curTicketFrameCT++;
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateShowTicketIcons();
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
	}
}
