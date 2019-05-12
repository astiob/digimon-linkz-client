using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.Gasha;
using UnityEngine;

public sealed class CMD_TicketGashaResult : CMD
{
	[SerializeField]
	private GashaUserAssetsInventory assetsInventory;

	[SerializeField]
	private GashaStartButtonEvent startButton;

	[Header("アイコン中心位置")]
	[SerializeField]
	private GameObject goICON_CENTER_POS;

	[Header("アイコンオフセット XY")]
	[SerializeField]
	private Vector2 iconOffset;

	[Header("アイコンX方向の数")]
	[SerializeField]
	private int iconNumX;

	[Header("アイコン登場時間(フレーム数)")]
	[SerializeField]
	private int showChipInterval = 16;

	[Header("アイコンルート)")]
	[SerializeField]
	private GameObject goICON_ROOT;

	[Header("シングルキャプチャボタンSprite")]
	[SerializeField]
	private UISprite buttonSpriteSingle;

	[Header("10連キャプチャボタンSprite")]
	[SerializeField]
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

	[Header("WHITE エフェクト")]
	[SerializeField]
	private GameObject goEFC_WHITE;

	[Header("GOLD エフェクト")]
	[SerializeField]
	private GameObject goEFC_GOLD;

	[Header("RAINBOW エフェクト")]
	[SerializeField]
	private GameObject goEFC_RAINBOW;

	[Header("BG TEX")]
	[SerializeField]
	public UITexture txBG;

	private List<TicketEfc> ticketEffectList;

	private int curTicketInitNUM;

	private int curTicketFrameCT;

	private bool isOnTapped;

	public static CMD_TicketGashaResult instance;

	public static GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo;

	public static GameWebAPI.RespDataGA_ExecTicket.UserDungeonTicketList[] UserDungeonTicketList { get; set; }

	public static GameWebAPI.RespDataGA_ExecGacha.GachaRewardsData[] RewardsData { get; set; }

	public bool StartEffect { get; set; }

	protected override void Awake()
	{
		CMD_TicketGashaResult.instance = this;
		this.ticketEffectList = new List<TicketEfc>();
		base.Awake();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_TicketGashaResult.instance = null;
	}

	private void OnEnable()
	{
		if (base.GetActionStatus() == CommonDialog.ACT_STATUS.OPEN)
		{
			this.assetsInventory.SetGashaPriceType(CMD_TicketGashaResult.gashaInfo.priceType);
			this.startButton.SetGashaInfo(CMD_TicketGashaResult.gashaInfo, false);
			this.startButton.SetPlayButton();
		}
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateAnimationTicketIcon();
	}

	private void SetGashaResultDetails()
	{
		GUICollider.DisableAllCollider("=================================== CMD_TicketGashaResult::ShoeDetail");
		this.CreateTicketIconEffect();
		this.assetsInventory.SetGashaPriceType(CMD_TicketGashaResult.gashaInfo.priceType);
		this.startButton.SetGashaInfo(CMD_TicketGashaResult.gashaInfo, false);
		this.startButton.SetPlayButton();
	}

	private void CreateTicketIconEffect()
	{
		Vector3 zero = Vector3.zero;
		zero.x = -(this.iconOffset.x * (float)(this.iconNumX - 1) / 2f);
		zero.y = this.iconOffset.y / 2f;
		Transform transform = this.goICON_ROOT.transform;
		transform.localPosition = this.goICON_CENTER_POS.transform.localPosition;
		int i;
		for (i = 0; i < CMD_TicketGashaResult.UserDungeonTicketList.Length; i++)
		{
			float num = (float)(i % this.iconNumX);
			float num2 = (float)(i / this.iconNumX);
			Vector3 effectPosition = new Vector3(zero.x + this.iconOffset.x * num, zero.y - this.iconOffset.y * num2, -5f);
			TicketEfc ticketEfc = this.CreateTicketEffect(CMD_TicketGashaResult.UserDungeonTicketList[i].effectType, transform, effectPosition);
			GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM dungeonTicketM = MasterDataMng.Instance().RespDataMA_DungeonTicketMaster.dungeonTicketM.FirstOrDefault((GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM x) => CMD_TicketGashaResult.UserDungeonTicketList[i].dungeonTicketId == x.dungeonTicketId);
			if (dungeonTicketM != null)
			{
				this.CreateTicketIcon(dungeonTicketM, ticketEfc.ngTICKET_THUMB);
			}
			ticketEfc.ngTXT_TICKET_NUM.text = string.Format(StringMaster.GetString("SystemItemCount2"), CMD_TicketGashaResult.UserDungeonTicketList[i].num);
			ticketEfc.spNew.enabled = (1 == CMD_TicketGashaResult.UserDungeonTicketList[i].isNew);
			this.ticketEffectList.Add(ticketEfc);
		}
	}

	private TicketEfc CreateTicketEffect(string effectType, Transform parentTransform, Vector3 effectPosition)
	{
		GameObject original;
		if (effectType != null && !(effectType == "1"))
		{
			if (effectType == "2")
			{
				original = this.goEFC_GOLD;
				goto IL_61;
			}
			if (effectType == "3")
			{
				original = this.goEFC_RAINBOW;
				goto IL_61;
			}
		}
		original = this.goEFC_WHITE;
		IL_61:
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		Vector3 localScale = gameObject.transform.localScale;
		gameObject.SetActive(true);
		gameObject.transform.parent = parentTransform;
		gameObject.transform.localPosition = effectPosition;
		gameObject.transform.localScale = localScale;
		TicketEfc component = gameObject.GetComponent<TicketEfc>();
		component.enabled = true;
		return component;
	}

	private void CreateTicketIcon(GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM ticketMaster, UITexture ticketTexture)
	{
		Texture2D tex = NGUIUtil.LoadTexture(ticketMaster.img);
		if (null != tex)
		{
			NGUIUtil.ChangeUITexture(ticketTexture, tex, false);
		}
		GUICollider component = ticketTexture.gameObject.GetComponent<GUICollider>();
		if (null != component)
		{
			component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
			{
				if (flag)
				{
					CMD_ticketPOP cmd_ticketPOP = GUIMain.ShowCommonDialog(delegate(int selectButton)
					{
						if (selectButton == 0 && null != PartsMenu.instance)
						{
							PartsMenu.instance.OnClickedQuestType(1);
						}
					}, "CMD_ticketPOP", null) as CMD_ticketPOP;
					cmd_ticketPOP.Title = ticketMaster.name;
					cmd_ticketPOP.Info = ticketMaster.description;
					cmd_ticketPOP.BtnTextYes = StringMaster.GetString("QuestNormal");
					cmd_ticketPOP.BtnTextNo = StringMaster.GetString("SystemButtonClose");
					if (null != tex)
					{
						NGUIUtil.ChangeUITexture(cmd_ticketPOP.txIcon, tex, false);
					}
				}
			};
		}
	}

	private IEnumerator GashaRewardSet(float delay)
	{
		yield return new WaitForSeconds(delay);
		CMD_CaptureBonus dialog = GUIMain.ShowCommonDialog(null, "CMD_CaptureBonus", null) as CMD_CaptureBonus;
		dialog.DialogDataSet(CMD_TicketGashaResult.RewardsData);
		dialog.AdjustSize();
		GUICollider.EnableAllCollider("=================================== CMD_TicketGashaResult::ICON");
		yield break;
	}

	private void OnTapped()
	{
		this.isOnTapped = true;
	}

	private void ClearTicketIcons()
	{
		for (int i = 0; i < this.ticketEffectList.Count; i++)
		{
			UnityEngine.Object.Destroy(this.ticketEffectList[i].gameObject);
		}
		this.ticketEffectList.Clear();
		this.curTicketInitNUM = 0;
		this.curTicketFrameCT = 0;
		this.StartEffect = false;
		this.isOnTapped = false;
		this.startButton.gameObject.SetActive(false);
	}

	private void UpdateAnimationTicketIcon()
	{
		if (!this.StartEffect)
		{
			return;
		}
		int num = this.curTicketFrameCT % this.showChipInterval;
		if ((num == 0 || this.isOnTapped) && this.curTicketInitNUM < CMD_TicketGashaResult.UserDungeonTicketList.Length)
		{
			if (this.curTicketInitNUM == 0)
			{
				GUICollider.EnableAllCollider("=================================== CMD_TicketGashaResult::UpdateShowTicketIcons");
			}
			this.ticketEffectList[this.curTicketInitNUM].Play();
			this.curTicketInitNUM++;
			SoundMng.Instance().PlaySE("SEInternal/Farm/se_221", 0f, false, true, null, -1, 1f);
			if (CMD_TicketGashaResult.UserDungeonTicketList.Length == this.curTicketInitNUM)
			{
				this.StartEffect = false;
				this.startButton.gameObject.SetActive(true);
				if (CMD_TicketGashaResult.RewardsData != null)
				{
					GUICollider.DisableAllCollider("=================================== CMD_TicketGashaResult::ICON");
					AppCoroutine.Start(this.GashaRewardSet(0.7f), true);
				}
			}
		}
		this.curTicketFrameCT++;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.SetGashaResultDetails();
		base.Show(f, sizeX, sizeY, aT);
	}

	public static void CreateDialog()
	{
		if (null != CMD_TicketGashaResult.instance)
		{
			CMD_TicketGashaResult.instance.ClearTicketIcons();
			CMD_TicketGashaResult.instance.SetGashaResultDetails();
		}
		else
		{
			GUIMain.ShowCommonDialog(null, "CMD_TicketGashaResult", null);
		}
	}
}
