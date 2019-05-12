using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CMD_ChipAdministration : CMD
{
	private const string CMD_NAME = "CMD_ChipList";

	private const int MAX_SALE_COUNT = 10;

	[SerializeField]
	private GUICollider sortButton;

	[SerializeField]
	private UILabel sortButtonLabel;

	[SerializeField]
	private UILabel sortNameLabel;

	[SerializeField]
	private UILabel messageLabel;

	[SerializeField]
	private GUICollider listButton;

	[SerializeField]
	private UILabel listButtonLabel;

	[SerializeField]
	private GUICollider saleButton;

	[SerializeField]
	private UILabel saleButtonLabel;

	[SerializeField]
	private UILabel clusterLabel;

	[SerializeField]
	private GameObject saleCluster;

	[SerializeField]
	private UILabel saleClusterTitleLabel;

	[SerializeField]
	private UILabel saleClusterMessageLabel;

	[SerializeField]
	private GUICollider saleDecisionButton;

	[SerializeField]
	private UILabel saleDecisionButtonLabel;

	[SerializeField]
	private UILabel listCountLabel;

	private CMD_ChipAdministration.ViewModeType viewModeType;

	private List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> saleUserChipList = new List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>();

	private ChipList chipList;

	private int totalPrice;

	private GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] userChipList;

	public static CMD_ChipAdministration Create(Action<int> callback = null)
	{
		return GUIMain.ShowCommonDialog(callback, "CMD_ChipList") as CMD_ChipAdministration;
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		ChipTutoial.Start("second_tutorial_chip_list", delegate
		{
			GUICollider.EnableAllCollider("CMD_ChipList");
		});
	}

	public override void ClosePanel(bool animation = true)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		AppCoroutine.Start(this.Init(f, sizeX, sizeY, aT), false);
	}

	private IEnumerator Init(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_ChipList");
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		this.userChipList = this.ConvertChipList(ChipDataMng.userChipData);
		this.chipList = new ChipList(this.goEFC_FOOTER, 7, new Vector2(960f, 450f), this.userChipList, false);
		this.chipList.SetPosition(new Vector3(-110f, -70f, 0f));
		this.chipList.SetScrollBarPosX(450f);
		this.chipList.SetShortTouchCallback(new Action<GUIListChipParts.Data>(this.OnShortTouchChip));
		this.chipList.SetLongTouchCallback(new Action<GUIListChipParts.Data>(this.OnLongTouchChip));
		this.chipList.AddWidgetDepth(base.gameObject.GetComponent<UIWidget>().depth);
		this.messageLabel.gameObject.SetActive(this.userChipList.Length == 0);
		this.messageLabel.text = StringMaster.GetString("ChipAdministration-01");
		int currentListNum = 0;
		if (ChipDataMng.userChipData != null && ChipDataMng.userChipData.userChipList != null)
		{
			currentListNum = ChipDataMng.userChipData.userChipList.Length;
		}
		this.listCountLabel.text = string.Format(StringMaster.GetString("SystemFraction"), currentListNum, DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.chipLimitMax);
		this.listButtonLabel.text = StringMaster.GetString("SystemList");
		this.saleButtonLabel.text = StringMaster.GetString("SystemButtonSell");
		this.saleClusterTitleLabel.text = StringMaster.GetString("SaleConfirmGain");
		this.saleClusterMessageLabel.text = "0";
		this.saleDecisionButtonLabel.text = this.saleButtonLabel.text;
		this.sortButtonLabel.text = StringMaster.GetString("SystemSortButton");
		this.sortNameLabel.text = CMD_ChipSortModal.GetSortName();
		this.UpdateCluster();
		this.listButton.CallBackClass = base.gameObject;
		this.listButton.MethodToInvoke = "OnListButton";
		this.saleButton.CallBackClass = base.gameObject;
		this.saleButton.MethodToInvoke = "OnSaleButton";
		this.saleDecisionButton.CallBackClass = base.gameObject;
		this.saleDecisionButton.MethodToInvoke = "OnSaleDecisionButton";
		this.sortButton.CallBackClass = base.gameObject;
		this.sortButton.MethodToInvoke = "OnSortButton";
		this.ChangeViewMode(CMD_ChipAdministration.ViewModeType.List);
		base.ShowDLG();
		base.Show(f, sizeX, sizeY, aT);
		RestrictionInput.EndLoad();
		yield return null;
		yield break;
	}

	private void OnShortTouchChip(GUIListChipParts.Data data)
	{
		global::Debug.LogWarning("ShortTouch " + data.userChip.userChipId);
		if (this.viewModeType == CMD_ChipAdministration.ViewModeType.List)
		{
			CMD_QuestItemPOP.Create(data.masterChip);
		}
		else
		{
			if (data.userChip.userMonsterId > 0)
			{
				return;
			}
			if (this.saleUserChipList.Contains(data.userChip))
			{
				this.totalPrice -= data.masterChip.GetSellPrice();
				this.saleClusterMessageLabel.text = StringFormat.Cluster(this.totalPrice);
				if (this.saleUserChipList.Count >= 10)
				{
					foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList in this.userChipList)
					{
						this.chipList.SetSelectColor(userChipList.userChipId, userChipList.userMonsterId > 0);
					}
					foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList2 in this.saleUserChipList)
					{
						this.chipList.SetSelectColor(userChipList2.userChipId, true);
					}
				}
				this.saleUserChipList.Remove(data.userChip);
				this.chipList.SetSelectColor(data.userChip.userChipId, false);
				this.chipList.SetAllSelectMessage(string.Empty);
				for (int j = 0; j < this.saleUserChipList.Count<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>(); j++)
				{
					this.chipList.SetSelectMessage(this.saleUserChipList[j].userChipId, (j + 1).ToString());
				}
			}
			else if (this.saleUserChipList.Count < 10)
			{
				this.totalPrice += data.masterChip.GetSellPrice();
				this.saleClusterMessageLabel.text = StringFormat.Cluster(this.totalPrice);
				this.saleUserChipList.Add(data.userChip);
				this.chipList.SetSelectColor(data.userChip.userChipId, true);
				this.chipList.SetAllSelectMessage(string.Empty);
				for (int k = 0; k < this.saleUserChipList.Count<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>(); k++)
				{
					this.chipList.SetSelectMessage(this.saleUserChipList[k].userChipId, (k + 1).ToString());
				}
				if (this.saleUserChipList.Count >= 10)
				{
					this.chipList.SetAllSelectColor(true);
				}
			}
			this.EnableSaleDecisionButton(this.saleUserChipList.Count > 0);
		}
	}

	private void OnLongTouchChip(GUIListChipParts.Data data)
	{
		global::Debug.LogWarning("LongTouch " + data.userChip.userChipId);
		if (this.viewModeType == CMD_ChipAdministration.ViewModeType.List)
		{
			CMD_QuestItemPOP.Create(data.masterChip);
		}
		else
		{
			CMD_QuestItemPOP.Create(data.masterChip);
		}
	}

	private void OnListButton()
	{
		this.ChangeViewMode(CMD_ChipAdministration.ViewModeType.List);
	}

	private void OnSaleButton()
	{
		this.ChangeViewMode(CMD_ChipAdministration.ViewModeType.Sale);
	}

	private void OnSaleDecisionButton()
	{
		Action<int> action = delegate(int result)
		{
			global::Debug.Log("result " + result);
			if (result == 0)
			{
				AppCoroutine.Start(this.Send(), false);
			}
		};
		CMD_SaleCheck cmd_SaleCheck = GUIMain.ShowCommonDialog(action, "CMD_SaleCheck") as CMD_SaleCheck;
		cmd_SaleCheck.SetParams(this.saleUserChipList, StringFormat.Cluster(this.totalPrice));
	}

	private IEnumerator Send()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		List<int> ids = new List<int>();
		foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList saleUserChip in this.saleUserChipList)
		{
			ids.Add(saleUserChip.userChipId);
		}
		Action<int> callback = delegate(int resultCode)
		{
			RestrictionInput.EndLoad();
			if (resultCode == 1)
			{
				int num = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
				int num2 = num + this.totalPrice;
				DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney = num2.ToString();
				this.UpdateCluster();
				this.saleUserChipList.Clear();
				this.chipList.SetAllSelectColor(false);
				this.saleClusterMessageLabel.text = "0";
				this.totalPrice = 0;
				this.userChipList = this.ConvertChipList(ChipDataMng.userChipData);
				this.chipList.ReAllBuild(this.userChipList, false);
				this.chipList.SetShortTouchCallback(new Action<GUIListChipParts.Data>(this.OnShortTouchChip));
				this.chipList.SetLongTouchCallback(new Action<GUIListChipParts.Data>(this.OnLongTouchChip));
				foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList in this.userChipList)
				{
					if (userChipList.userMonsterId > 0)
					{
						this.chipList.SetSelectColor(userChipList.userChipId, true);
					}
				}
				this.messageLabel.gameObject.SetActive(this.userChipList.Length == 0);
				this.listCountLabel.text = string.Format(StringMaster.GetString("SystemFraction"), ChipDataMng.userChipData.userChipList.Length, DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.chipLimitMax);
				this.EnableSaleDecisionButton(false);
			}
			else
			{
				string @string = StringMaster.GetString("SystemDataMismatchTitle");
				string message = string.Format(StringMaster.GetString("ChipDataMismatchMesage"), resultCode);
				AlertManager.ShowModalMessage(delegate(int modal)
				{
				}, @string, message, AlertManager.ButtonActionType.Close, false);
			}
		};
		GameWebAPI.ChipSellLogic logic = ChipDataMng.RequestAPIChipSell(ids.ToArray(), callback);
		IEnumerator run = logic.Run(null, null, null);
		while (run.MoveNext())
		{
			object obj = run.Current;
			yield return obj;
		}
		yield break;
	}

	private void OnSortButton()
	{
		Action<int> callback = delegate(int result)
		{
			global::Debug.Log("result " + result);
			if (result > 0)
			{
				this.sortNameLabel.text = CMD_ChipSortModal.GetSortName();
				this.userChipList = this.ConvertChipList(ChipDataMng.userChipData);
				this.chipList.ReAllBuild(this.userChipList, false);
				this.chipList.SetShortTouchCallback(new Action<GUIListChipParts.Data>(this.OnShortTouchChip));
				this.chipList.SetLongTouchCallback(new Action<GUIListChipParts.Data>(this.OnLongTouchChip));
				this.messageLabel.gameObject.SetActive(this.userChipList.Length == 0);
				if (this.viewModeType == CMD_ChipAdministration.ViewModeType.Sale)
				{
					foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList in this.saleUserChipList)
					{
						this.chipList.SetSelectColor(userChipList.userChipId, true);
					}
					foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList2 in this.userChipList)
					{
						if (userChipList2.userMonsterId > 0)
						{
							this.chipList.SetSelectColor(userChipList2.userChipId, true);
						}
					}
					for (int j = 0; j < this.saleUserChipList.Count<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>(); j++)
					{
						this.chipList.SetSelectMessage(this.saleUserChipList[j].userChipId, (j + 1).ToString());
					}
				}
			}
		};
		CMD_ChipSortModal.Create(callback);
	}

	private void ChangeViewMode(CMD_ChipAdministration.ViewModeType type)
	{
		this.viewModeType = type;
		bool flag = this.viewModeType == CMD_ChipAdministration.ViewModeType.List;
		base.PartsTitle.SetTitle((!flag) ? StringMaster.GetString("ChipAdministration-03") : StringMaster.GetString("ChipAdministration-02"));
		this.listButton.GetComponent<BoxCollider>().enabled = !flag;
		this.saleButton.GetComponent<BoxCollider>().enabled = flag;
		this.listButton.GetComponent<UISprite>().spriteName = ((!flag) ? "Common02_Btn_BaseON" : "Common02_Btn_BaseG");
		this.saleButton.GetComponent<UISprite>().spriteName = ((!flag) ? "Common02_Btn_BaseG" : "Common02_Btn_BaseON");
		this.saleCluster.SetActive(!flag);
		this.saleDecisionButton.gameObject.SetActive(!flag);
		this.saleButtonLabel.color = ((!flag) ? ConstValue.DEACTIVE_BUTTON_LABEL : Color.white);
		this.listButtonLabel.color = ((!flag) ? Color.white : ConstValue.DEACTIVE_BUTTON_LABEL);
		this.EnableSaleDecisionButton(false);
		if (this.saleUserChipList.Count > 0)
		{
			this.saleUserChipList.Clear();
			this.chipList.SetAllSelectColor(false);
			this.chipList.SetAllSelectMessage(string.Empty);
			this.totalPrice = 0;
			this.saleClusterMessageLabel.text = "0";
		}
		if (this.viewModeType == CMD_ChipAdministration.ViewModeType.Sale)
		{
			foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList in this.userChipList)
			{
				if (userChipList.userMonsterId > 0)
				{
					this.chipList.SetSelectColor(userChipList.userChipId, true);
				}
			}
		}
	}

	private void UpdateCluster()
	{
		GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
		this.clusterLabel.text = StringFormat.Cluster(playerInfo.gamemoney);
	}

	private void EnableSaleDecisionButton(bool value)
	{
		UISprite component = this.saleDecisionButton.GetComponent<UISprite>();
		component.GetComponent<BoxCollider>().enabled = value;
		component.spriteName = ((!value) ? "Common02_Btn_Gray" : "Common02_Btn_Red");
		this.saleDecisionButtonLabel.color = ((!value) ? ConstValue.DEACTIVE_BUTTON_LABEL : Color.white);
	}

	private GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] ConvertChipList(GameWebAPI.RespDataCS_ChipListLogic chipListLogic)
	{
		List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> list = new List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>();
		if (chipListLogic != null && chipListLogic.userChipList != null)
		{
			list.AddRange(chipListLogic.userChipList);
			CMD_ChipSortModal.UpdateSortedUserChipList(list.ToArray());
			return CMD_ChipSortModal.sortedUserChipList;
		}
		return list.ToArray();
	}

	private enum ViewModeType
	{
		List,
		Sale
	}
}
