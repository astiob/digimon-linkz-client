using Evolution;
using ExchangeData;
using Master;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class CMD_ClearingHouse : CMD
{
	[SerializeField]
	private GUISelectPanelExchange exchangeList;

	[SerializeField]
	private GameObject goListPartsExchange;

	[SerializeField]
	private List<UILabel> exchangeConsumeItemNumLabel;

	[SerializeField]
	private UILabel exchangeLimitLabel;

	[SerializeField]
	private List<UISprite> exchangeConsumeSprite;

	[SerializeField]
	private List<UITexture> exchangeConsumeTexture;

	[SerializeField]
	private List<GameObject> exchangeBaseObject;

	[SerializeField]
	private UILabel exchangeEmptyLabel;

	[SerializeField]
	private UILabel canNumText;

	[SerializeField]
	private UILabel needNumText;

	[SerializeField]
	private UILabel buttonText;

	[SerializeField]
	private UILabel limitText;

	private Dictionary<string, string> itemDictionary = new Dictionary<string, string>();

	private static GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result exchangeResultInfo;

	private GameWebAPI.RespDataMA_GetItemM.ItemM exchangeConsumeItemInfo;

	private string exchangeConsumeItemTexturePath;

	private List<string> exchangeConsumeItemName = new List<string>();

	private List<string> exchangeItemPathList = new List<string>();

	private List<ExchangeItem> exchangeItemList;

	private List<GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result.Detail.Item> exchangeItemDataList = new List<GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result.Detail.Item>();

	private string consumeNum;

	public static GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result ExchangeResultInfo
	{
		set
		{
			CMD_ClearingHouse.exchangeResultInfo = value;
		}
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.canNumText.text = StringMaster.GetString("ExchangeCanNum");
		this.needNumText.text = StringMaster.GetString("ExchangeNeedNum");
		this.buttonText.text = StringMaster.GetString("ExchangeButtonText");
		this.limitText.text = StringMaster.GetString("ExchangeLimit");
		base.SetTutorialAnyTime("anytime_second_tutorial_exchange");
		this.InitExchange(f, sizeX, sizeY, aT);
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	public override void ClosePanel(bool animation = true)
	{
		this.CloseAndFarmCamOn(animation);
		if (this.exchangeList != null)
		{
			this.exchangeList.FadeOutAllListParts(null, false);
			this.exchangeList.SetHideScrollBarAllWays(true);
		}
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	private void InitExchange(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.InfoSetting();
		base.Show(f, sizeX, sizeY, aT);
	}

	private void InfoSetting()
	{
		this.goListPartsExchange.SetActive(false);
		if (CMD_ClearingHouse.exchangeResultInfo == null || CMD_ClearingHouse.exchangeResultInfo.detail == null || CMD_ClearingHouse.exchangeResultInfo.detail.Length == 0)
		{
			base.PartsTitle.SetTitle(StringMaster.GetString("ExchangeTitle"));
			this.goEFC_LEFT.SetActive(false);
			this.exchangeEmptyLabel.gameObject.SetActive(true);
			return;
		}
		if (!string.IsNullOrEmpty(CMD_ClearingHouse.exchangeResultInfo.name))
		{
			base.PartsTitle.SetTitle(CMD_ClearingHouse.exchangeResultInfo.name);
		}
		else
		{
			base.PartsTitle.SetTitle(StringMaster.GetString("ExchangeTitle"));
		}
		for (int i = 0; i < this.exchangeBaseObject.Count; i++)
		{
			this.exchangeBaseObject[i].SetActive(false);
		}
		for (int j = 0; j < CMD_ClearingHouse.exchangeResultInfo.detail.Length; j++)
		{
			string assetCategoryId = CMD_ClearingHouse.exchangeResultInfo.detail[j].item.assetCategoryId;
			string assetValue = CMD_ClearingHouse.exchangeResultInfo.detail[j].item.assetValue;
			string key = assetCategoryId + assetValue;
			if (!this.itemDictionary.ContainsKey(key) || (this.itemDictionary.ContainsKey(key) && this.itemDictionary[key] != assetValue))
			{
				this.itemDictionary.Add(key, assetValue);
				this.exchangeItemDataList.Add(CMD_ClearingHouse.exchangeResultInfo.detail[j].item);
			}
		}
		int num = 0;
		foreach (string key2 in this.itemDictionary.Keys)
		{
			this.exchangeConsumeItemInfo = Utility.GetUseExchangeItem(this.exchangeItemDataList[num], MasterDataMng.Instance().RespDataMA_ItemM.itemM);
			if (this.exchangeBaseObject.Count > num)
			{
				this.exchangeConsumeItemNumLabel[num].text = this.exchangeItemDataList[num].count.ToString();
			}
			string assetCategoryId2 = this.exchangeItemDataList[num].assetCategoryId;
			string consumeAssetValue = this.itemDictionary[key2];
			MasterDataMng.AssetCategory assetCategory = (MasterDataMng.AssetCategory)assetCategoryId2.ToInt32();
			if (assetCategory == MasterDataMng.AssetCategory.ITEM)
			{
				if (this.exchangeBaseObject.Count > num)
				{
					this.exchangeConsumeSprite[num].enabled = false;
					this.exchangeConsumeTexture[num].enabled = true;
				}
				if (this.exchangeConsumeItemInfo != null)
				{
					this.exchangeConsumeItemTexturePath = this.exchangeConsumeItemInfo.GetSmallImagePath();
					this.exchangeItemPathList.Add(this.exchangeConsumeItemTexturePath);
					this.exchangeConsumeItemName.Add(this.exchangeConsumeItemInfo.name);
					if (this.exchangeBaseObject.Count > num)
					{
						NGUIUtil.ChangeUITextureFromFile(this.exchangeConsumeTexture[num], this.exchangeConsumeItemTexturePath, false);
					}
				}
			}
			else
			{
				if (this.exchangeBaseObject.Count > num)
				{
					this.exchangeConsumeSprite[num].enabled = true;
					this.exchangeConsumeTexture[num].enabled = false;
				}
				GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory2 = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(assetCategoryId2);
				if (assetCategory2 != null)
				{
					this.exchangeConsumeItemName.Add(assetCategory2.assetTitle);
				}
				string text = string.Empty;
				MasterDataMng.AssetCategory assetCategory3 = assetCategory;
				switch (assetCategory3)
				{
				case MasterDataMng.AssetCategory.MEAT:
					text = "Common02_Icon_Meat";
					this.exchangeItemPathList.Add(text);
					break;
				case MasterDataMng.AssetCategory.SOUL:
					this.exchangeConsumeItemTexturePath = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(consumeAssetValue);
					if (this.exchangeBaseObject.Count > num)
					{
						this.exchangeConsumeTexture[num].enabled = true;
						NGUIUtil.ChangeUITextureFromFile(this.exchangeConsumeTexture[num], this.exchangeConsumeItemTexturePath, false);
					}
					this.exchangeItemPathList.Add(this.exchangeConsumeItemTexturePath);
					break;
				default:
					switch (assetCategory3)
					{
					case MasterDataMng.AssetCategory.DIGI_STONE:
						text = "Common02_ShopList_1";
						this.exchangeItemPathList.Add(text);
						break;
					case MasterDataMng.AssetCategory.LINK_POINT:
						text = "Common02_Icon_Link";
						this.exchangeItemPathList.Add(text);
						break;
					case MasterDataMng.AssetCategory.TIP:
						text = "Common02_Icon_Chip";
						this.exchangeItemPathList.Add(text);
						break;
					}
					break;
				case MasterDataMng.AssetCategory.DUNGEON_TICKET:
				{
					GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM dungeonTicketM = MasterDataMng.Instance().RespDataMA_DungeonTicketMaster.dungeonTicketM.FirstOrDefault((GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM x) => consumeAssetValue == x.dungeonTicketId);
					if (dungeonTicketM != null)
					{
						this.exchangeConsumeItemTexturePath = dungeonTicketM.img;
						this.exchangeItemPathList.Add(this.exchangeConsumeItemTexturePath);
						if (this.exchangeBaseObject.Count > num)
						{
							this.exchangeConsumeTexture[num].enabled = true;
							if (dungeonTicketM != null)
							{
								NGUIUtil.ChangeUITextureFromFile(this.exchangeConsumeTexture[num], this.exchangeConsumeItemTexturePath, false);
							}
						}
					}
					break;
				}
				}
				if (this.exchangeBaseObject.Count > num)
				{
					if (!string.IsNullOrEmpty(text))
					{
						this.exchangeConsumeSprite[num].spriteName = text;
					}
					else
					{
						this.exchangeConsumeSprite[num].enabled = false;
					}
				}
			}
			if (this.exchangeBaseObject.Count > num)
			{
				this.exchangeBaseObject[num].SetActive(true);
			}
			num++;
		}
		if (ClassSingleton<ExchangeWebAPI>.Instance.IsExistAlwaysExchangeInfo(CMD_ClearingHouse.exchangeResultInfo))
		{
			this.exchangeLimitLabel.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			base.InvokeRepeating("TimeSetting", 1f, 1f);
			this.TimeSetting();
		}
		this.SetCommonUI_Exchange();
		this.InitExchangeList();
		this.SetExchangeDetail();
	}

	private void TimeSetting()
	{
		DateTime dateTime = DateTime.Parse(CMD_ClearingHouse.exchangeResultInfo.endTime);
		int restTimeSeconds = GUIBannerParts.GetRestTimeSeconds(dateTime);
		GUIBannerParts.SetTimeText(this.exchangeLimitLabel, restTimeSeconds, dateTime);
		this.exchangeLimitLabel.text = string.Format(StringMaster.GetString("ExchangeTimeLimit"), this.exchangeLimitLabel.text);
		if (restTimeSeconds <= 0)
		{
			base.CancelInvoke("TimeSetting");
		}
	}

	private void SetCommonUI_Exchange()
	{
		Vector3 localPosition = this.exchangeList.transform.localPosition;
		GUICollider component = this.exchangeList.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition);
		this.exchangeList.selectParts = this.goListPartsExchange;
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -560f;
		listWindowViewRect.xMax = 560f;
		listWindowViewRect.yMin = -256f - GUIMain.VerticalSpaceSize;
		listWindowViewRect.yMax = 256f + GUIMain.VerticalSpaceSize;
		this.exchangeList.ListWindowViewRect = listWindowViewRect;
	}

	private void InitExchangeList()
	{
		int num = CMD_ClearingHouse.exchangeResultInfo.detail.Length;
		this.goListPartsExchange.SetActive(true);
		this.exchangeList.initLocation = true;
		Vector3 vector = this.exchangeList.AllBuild(num, out this.exchangeItemList);
		this.goListPartsExchange.SetActive(false);
		if (0 < num)
		{
			float num2 = this.goListPartsExchange.transform.position.x - vector.x;
			Vector3 position = this.exchangeList.transform.position;
			position.x += num2;
			this.exchangeList.transform.position = position;
		}
	}

	private void SetExchangeDetail()
	{
		for (int i = 0; i < this.exchangeItemList.Count; i++)
		{
			this.exchangeItemList[i].SetDetail(CMD_ClearingHouse.exchangeResultInfo.detail[i], i, new Action<ExchangeItem>(this.OnPushedButton));
		}
	}

	private void OnPushedButton(ExchangeItem exchangeItem)
	{
		int num = 0;
		foreach (string a in this.itemDictionary.Keys)
		{
			string assetCategoryId = exchangeItem.exchangeItemData.assetCategoryId;
			string assetValue = exchangeItem.exchangeItemData.assetValue;
			string text = assetCategoryId + assetValue;
			if (this.itemDictionary.ContainsKey(text) && a == text && this.itemDictionary[text] == assetValue)
			{
				break;
			}
			num++;
		}
		string text2 = this.exchangeConsumeItemName[num];
		this.consumeNum = (int.Parse(exchangeItem.exchangeConsumeNum) * exchangeItem.numCounter).ToString();
		string text3 = (this.exchangeConsumeItemInfo == null) ? string.Empty : this.exchangeConsumeItemInfo.unitName;
		MasterDataMng.AssetCategory assetCategory = (MasterDataMng.AssetCategory)exchangeItem.exchangeItemData.assetCategoryId.ToInt32();
		CMD_ClearingHouse.IconType iconType = CMD_ClearingHouse.IconType.NON;
		switch (assetCategory)
		{
		case MasterDataMng.AssetCategory.MONSTER:
			text3 = StringMaster.GetString("ConsumeUnitMonster");
			iconType = CMD_ClearingHouse.IconType.NON;
			break;
		case MasterDataMng.AssetCategory.DIGI_STONE:
			iconType = CMD_ClearingHouse.IconType.SPRITE;
			break;
		case MasterDataMng.AssetCategory.LINK_POINT:
			text3 = string.Empty;
			iconType = CMD_ClearingHouse.IconType.SPRITE;
			break;
		case MasterDataMng.AssetCategory.TIP:
			text3 = string.Empty;
			iconType = CMD_ClearingHouse.IconType.SPRITE;
			break;
		case MasterDataMng.AssetCategory.ITEM:
			iconType = CMD_ClearingHouse.IconType.TEXTURE;
			break;
		case MasterDataMng.AssetCategory.GATHA_TICKET:
			text3 = StringMaster.GetString("ConsumeUnitTicket");
			iconType = CMD_ClearingHouse.IconType.NON;
			break;
		case MasterDataMng.AssetCategory.MEAT:
			iconType = CMD_ClearingHouse.IconType.SPRITE;
			break;
		case MasterDataMng.AssetCategory.SOUL:
			text3 = StringMaster.GetString("ConsumeUnitPlugin");
			iconType = CMD_ClearingHouse.IconType.TEXTURE;
			break;
		case MasterDataMng.AssetCategory.DUNGEON_TICKET:
			text3 = StringMaster.GetString("ConsumeUnitTicket");
			iconType = CMD_ClearingHouse.IconType.TEXTURE;
			break;
		}
		string text4 = exchangeItem.exchangeDetailName;
		MasterDataMng.AssetCategory assetCategory2 = (MasterDataMng.AssetCategory)exchangeItem.exchangeDetailCategoryID.ToInt32();
		if (assetCategory2 == MasterDataMng.AssetCategory.MONSTER)
		{
			text4 = string.Format(StringMaster.GetString("SystemItemCount"), text4, exchangeItem.exchangeDetailNum);
		}
		string info = string.Format(StringMaster.GetString("ExchangeConfirmInfo"), new object[]
		{
			text2,
			this.consumeNum,
			text3,
			text4
		});
		CMD_ChangePOP cd = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP") as CMD_ChangePOP;
		cd.Title = StringMaster.GetString("ExchangeConfirmTitle");
		cd.Info = info;
		cd.SetPoint(exchangeItem.exchangeItemData.count, int.Parse(this.consumeNum));
		if (iconType == CMD_ClearingHouse.IconType.SPRITE)
		{
			cd.SetSpriteIcon(this.exchangeItemPathList[num]);
		}
		else if (iconType == CMD_ClearingHouse.IconType.TEXTURE)
		{
			cd.SetTextureIcon(this.exchangeItemPathList[num]);
		}
		cd.OnPushedYesAction = delegate()
		{
			cd.ClosePanel(true);
			this.AccessExchangeLogic(exchangeItem);
		};
	}

	private void AccessExchangeLogic(ExchangeItem exchangeItem)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		APIRequestTask task = ClassSingleton<ExchangeWebAPI>.Instance.AccessEventExchangeLogicAPI(exchangeItem.exchangeDetailId, exchangeItem.numCounter);
		base.StartCoroutine(task.Run(delegate
		{
			RestrictionInput.EndLoad();
			if (ClassSingleton<ExchangeWebAPI>.Instance.exchangeErrorCode == string.Empty)
			{
				this.OpenExchangedItemModalMessage(exchangeItem);
			}
			else
			{
				this.ExchangeErrorPopup();
			}
		}, null, null));
	}

	private void OpenExchangedItemModalMessage(ExchangeItem exchangeItem)
	{
		MasterDataMng.AssetCategory assetCategory = (MasterDataMng.AssetCategory)exchangeItem.exchangeItemData.assetCategoryId.ToInt32();
		MasterDataMng.AssetCategory assetCategory2 = assetCategory;
		switch (assetCategory2)
		{
		case MasterDataMng.AssetCategory.DIGI_STONE:
			DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point -= int.Parse(this.consumeNum);
			break;
		default:
			if (assetCategory2 == MasterDataMng.AssetCategory.MEAT)
			{
				int num = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.meatNum) - int.Parse(this.consumeNum);
				DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.meatNum = num.ToString();
			}
			break;
		case MasterDataMng.AssetCategory.TIP:
		{
			int num2 = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney) - int.Parse(this.consumeNum);
			DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney = num2.ToString();
			break;
		}
		case MasterDataMng.AssetCategory.ITEM:
		{
			GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(exchangeItem.exchangeItemData.assetValue);
			if (itemM != null && int.Parse(itemM.itemId) == 2)
			{
				Singleton<UserDataMng>.Instance.UpdateUserItemNum(2, -int.Parse(this.consumeNum));
			}
			break;
		}
		}
		int num3 = int.Parse(exchangeItem.exchangeDetailNum, NumberStyles.AllowThousands);
		string exchangeDetailName = exchangeItem.exchangeDetailName;
		string arg = StringFormat.Cluster(num3 * exchangeItem.numCounter);
		string info = string.Format(StringMaster.GetString("ExchangeSuccessInfo"), exchangeDetailName, arg);
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(new Action<int>(this.RunReExchangeInfoLogicAPI), "CMD_ModalMessage") as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("ExchangeSuccessTitle");
		cmd_ModalMessage.Info = info;
		GUIPlayerStatus.RefreshParams_S(true);
	}

	private void ExchangeErrorPopup()
	{
		string exchangeErrorCode = ClassSingleton<ExchangeWebAPI>.Instance.exchangeErrorCode;
		switch (exchangeErrorCode)
		{
		case "E-EX01":
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(new Action<int>(this.ErrorPopUIClose), "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("ExchangeTermTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("ExchangeTermInfo");
			return;
		}
		case "E-EX02":
		{
			CMD_ModalMessage cmd_ModalMessage2 = GUIMain.ShowCommonDialog(new Action<int>(this.ErrorPopUIClose), "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage2.Title = StringMaster.GetString("ExchangeShortageTitle");
			cmd_ModalMessage2.Info = StringMaster.GetString("ExchangeShortageInfo");
			return;
		}
		case "E-EX03":
		{
			CMD_ModalMessage cmd_ModalMessage3 = GUIMain.ShowCommonDialog(new Action<int>(this.ErrorPopUIClose), "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage3.Title = StringMaster.GetString("ExchangeLimitTitle");
			cmd_ModalMessage3.Info = StringMaster.GetString("ExchangeLimitInfo");
			return;
		}
		}
		CMD_ModalMessage cmd_ModalMessage4 = GUIMain.ShowCommonDialog(new Action<int>(this.ErrorPopUIClose), "CMD_ModalMessage") as CMD_ModalMessage;
		cmd_ModalMessage4.Title = StringMaster.GetString("ExchangeFailedTitle");
		cmd_ModalMessage4.Info = StringMaster.GetString("ExchangeFailedInfo");
	}

	private void ErrorPopUIClose(int dialogValue)
	{
		base.closeAll();
	}

	private void RunReExchangeInfoLogicAPI(int dialogValue)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		APIRequestTask apirequestTask = new APIRequestTask();
		apirequestTask.Add(ClassSingleton<ExchangeWebAPI>.Instance.AccessEventExchangeInfoLogicAPI()).Add(Singleton<UserDataMng>.Instance.RequestPlayerInfo(true)).Add(DataMng.Instance().RequestMyPageData(true));
		base.StartCoroutine(apirequestTask.Run(delegate
		{
			RestrictionInput.EndLoad();
			ClassSingleton<FacePresentAccessor>.Instance.facePresent.SetBadgeOnly();
			this.RefreshInfo();
		}, delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(true);
		}, null));
	}

	private void RefreshInfo()
	{
		bool flag = false;
		foreach (GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result result in ClassSingleton<ExchangeWebAPI>.Instance.EventExchangeInfoLogicData)
		{
			if (result.eventExchangeId == CMD_ClearingHouse.exchangeResultInfo.eventExchangeId)
			{
				flag = true;
				CMD_ClearingHouse.exchangeResultInfo = result;
				break;
			}
		}
		if (!flag)
		{
			ClassSingleton<ExchangeWebAPI>.Instance.DeleteExchangeInfoLogicResult(CMD_ClearingHouse.exchangeResultInfo.eventExchangeId);
			base.SetCloseAction(delegate(int i)
			{
				CMD_ClearingHouseTop.instance.Rebuild();
			});
			this.ClosePanel(true);
			return;
		}
		this.exchangeItemDataList.Clear();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		for (int j = 0; j < CMD_ClearingHouse.exchangeResultInfo.detail.Length; j++)
		{
			string assetCategoryId = CMD_ClearingHouse.exchangeResultInfo.detail[j].item.assetCategoryId;
			string assetValue = CMD_ClearingHouse.exchangeResultInfo.detail[j].item.assetValue;
			string key = assetCategoryId + assetValue;
			if (!dictionary.ContainsKey(key) || (dictionary.ContainsKey(key) && dictionary[key] != assetValue))
			{
				dictionary.Add(key, assetValue);
				this.exchangeItemDataList.Add(CMD_ClearingHouse.exchangeResultInfo.detail[j].item);
			}
		}
		for (int k = 0; k < this.exchangeItemDataList.Count; k++)
		{
			if (this.exchangeBaseObject.Count <= k)
			{
				break;
			}
			this.exchangeConsumeItemNumLabel[k].text = this.exchangeItemDataList[k].count.ToString();
		}
		for (int l = 0; l < this.exchangeItemList.Count; l++)
		{
			this.exchangeItemList[l].ResetNum(CMD_ClearingHouse.exchangeResultInfo.detail[l]);
			this.exchangeItemList[l].SetButton(CMD_ClearingHouse.exchangeResultInfo.detail[l]);
		}
		GUIExchangeMenu.instance.ReloadResultInfo();
	}

	private enum IconType
	{
		SPRITE,
		TEXTURE,
		NON
	}
}
