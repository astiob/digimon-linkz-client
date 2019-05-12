using FarmData;
using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_Training_Menu : CMD
{
	[SerializeField]
	private UILabel lbTX_Stone;

	[SerializeField]
	private UILabel lbTX_Chip;

	[SerializeField]
	private UILabel lbTX_Meat;

	[SerializeField]
	private UILabel lbTX_HQ_Meat;

	[SerializeField]
	private GUISelectPanelViewPartsUD csSelectPanel;

	[SerializeField]
	[Header("非アクティブ ベース 色")]
	private Color colBase;

	[Header("非アクティブ タイトル 色")]
	[SerializeField]
	private Color colTitle;

	[Header("非アクティブ 【】 色")]
	[SerializeField]
	private Color colLR;

	[SerializeField]
	private UISprite spChipList;

	[SerializeField]
	private UILabel lbChipList;

	public static CMD_Training_Menu instance;

	[Header("各パーツのデータ")]
	[SerializeField]
	private List<GUIListPartsTrainingMenu.PartsData> TrainingMenuPartsDataL;

	protected override void Awake()
	{
		base.Awake();
		CMD_Training_Menu.instance = this;
	}

	protected override void WindowClosed()
	{
		base.WindowClosed();
		CMD_Training_Menu.instance = null;
	}

	public GUIListPartsTrainingMenu.PartsData GetData(int idx)
	{
		return this.TrainingMenuPartsDataL[idx];
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		base.StartCoroutine(this.InitDLG(f, sizeX, sizeY, aT));
	}

	private IEnumerator InitDLG(Action<int> f, float sizeX, float sizeY, float aT)
	{
		Singleton<UserDataMng>.Instance.RequestUserStockFacilityDataAPI(delegate(bool flg)
		{
			if (flg)
			{
				base.ShowDLG();
				base.PartsTitle.SetTitle(StringMaster.GetString("TrainingMenuTitle"));
				this.InitUI();
				base.Show(f, sizeX, sizeY, aT);
			}
			else
			{
				base.ClosePanel(false);
			}
			RestrictionInput.EndLoad();
		});
		yield break;
	}

	private void InitUI()
	{
		this.MakeData();
		this.csSelectPanel.initLocation = true;
		this.csSelectPanel.AllBuild(this.TrainingMenuPartsDataL.Count, true, 1f, 1f, null, null);
		this.ShowDatas();
		this.OnClickedChipList(this.spChipList, this.lbChipList);
	}

	private void MakeData()
	{
		int trainHouseCT = this.GetFacilityCount(4);
		int chipFactoryCT = this.GetFacilityCount(25);
		if (this.TrainingMenuPartsDataL != null)
		{
			for (int i = 0; i < this.TrainingMenuPartsDataL.Count; i++)
			{
				GUIListPartsTrainingMenu.PartsData partsData = this.TrainingMenuPartsDataL[i];
				partsData.strCampaign = string.Empty;
				partsData.isInfo = false;
				partsData.isNew = false;
				int num = 0;
				string strTitle = partsData.strTitle;
				switch (strTitle)
				{
				case "MealTitle":
				{
					GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.MeatExpUp);
					if (campaignInfo != null)
					{
						num++;
					}
					if (num > 1)
					{
						partsData.strCampaign = StringMaster.GetString("Campaign");
					}
					else if (num == 1 && campaignInfo != null)
					{
						partsData.strCampaign = CampaignUtil.GetDescription(GameWebAPI.RespDataCP_Campaign.CampaignType.MeatExpUp, float.Parse(campaignInfo.rate), true);
					}
					partsData.actCallBack = delegate()
					{
						CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.MEAL;
						GUIMain.ShowCommonDialog(null, "CMD_BaseSelect");
					};
					break;
				}
				case "ReinforcementTitle":
				{
					GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.TrainExpUp);
					if (campaignInfo != null)
					{
						num++;
					}
					GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo2 = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.TrainCostDown);
					if (campaignInfo2 != null)
					{
						num++;
					}
					GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo3 = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.TrainLuckUp);
					if (campaignInfo3 != null)
					{
						num++;
					}
					if (num > 1)
					{
						partsData.strCampaign = StringMaster.GetString("Campaign");
					}
					else if (num == 1)
					{
						if (campaignInfo != null)
						{
							partsData.strCampaign = CampaignUtil.GetDescription(GameWebAPI.RespDataCP_Campaign.CampaignType.TrainExpUp, float.Parse(campaignInfo.rate), true);
						}
						if (campaignInfo2 != null)
						{
							partsData.strCampaign = CampaignUtil.GetDescription(GameWebAPI.RespDataCP_Campaign.CampaignType.TrainCostDown, float.Parse(campaignInfo2.rate), true);
						}
						if (campaignInfo3 != null)
						{
							partsData.strCampaign = CampaignUtil.GetDescription(GameWebAPI.RespDataCP_Campaign.CampaignType.TrainLuckUp, float.Parse(campaignInfo3.rate), true);
						}
					}
					partsData.actCallBack = delegate()
					{
						GUIMain.ShowCommonDialog(null, "CMD_ReinforcementTOP");
					};
					break;
				}
				case "SuccessionTitle":
					if (trainHouseCT <= 0)
					{
						partsData.col = this.colBase;
						partsData.labelCol = this.colTitle;
						partsData.LRCol = this.colLR;
					}
					partsData.actCallBack = delegate()
					{
						if (trainHouseCT > 0)
						{
							GUIMain.ShowCommonDialog(null, "CMD_Succession");
						}
						else
						{
							CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
							cmd_ModalMessage.Title = StringMaster.GetString("TrainingMissingAlertTitle");
							cmd_ModalMessage.Info = StringMaster.GetString("TrainingMissingAlertInfo-01");
						}
					};
					break;
				case "ArousalTitle":
					if (trainHouseCT <= 0)
					{
						partsData.col = this.colBase;
						partsData.labelCol = this.colTitle;
						partsData.LRCol = this.colLR;
					}
					partsData.actCallBack = delegate()
					{
						if (trainHouseCT > 0)
						{
							GUIMain.ShowCommonDialog(null, "CMD_ArousalTOP");
						}
						else
						{
							CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
							cmd_ModalMessage.Title = StringMaster.GetString("TrainingMissingAlertTitle");
							cmd_ModalMessage.Info = StringMaster.GetString("TrainingMissingAlertInfo-02");
						}
					};
					break;
				case "LaboratoryTitle":
					partsData.actCallBack = delegate()
					{
						GUIMain.ShowCommonDialog(null, "CMD_Laboratory");
					};
					break;
				case "MedalInheritTitle":
				{
					GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo = DataMng.Instance().GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType.MedalTakeOverUp);
					if (campaignInfo != null)
					{
						num++;
					}
					if (num > 1)
					{
						partsData.strCampaign = StringMaster.GetString("Campaign");
					}
					else if (num == 1 && campaignInfo != null)
					{
						partsData.strCampaign = CampaignUtil.GetDescription(GameWebAPI.RespDataCP_Campaign.CampaignType.MedalTakeOverUp, float.Parse(campaignInfo.rate), true);
					}
					partsData.actCallBack = delegate()
					{
						GUIMain.ShowCommonDialog(null, "CMD_MedalInherit");
					};
					break;
				}
				case "ChipSphereTitle":
					if (chipFactoryCT <= 0)
					{
						partsData.col = this.colBase;
						partsData.labelCol = this.colTitle;
						partsData.LRCol = this.colLR;
					}
					partsData.actCallBack = delegate()
					{
						if (chipFactoryCT > 0)
						{
							CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.CHIP;
							CMD_BaseSelect.ElementType = CMD_BaseSelect.ELEMENT_TYPE.BASE;
							GUIMain.ShowCommonDialog(null, "CMD_BaseSelect");
						}
						else
						{
							CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
							cmd_ModalMessage.Title = StringMaster.GetString("ChipFactoryMissingAlertTitle");
							cmd_ModalMessage.Info = StringMaster.GetString("ChipFactoryMissingAlertInfo-1");
						}
					};
					break;
				case "ChipReinforceTitle":
					if (chipFactoryCT <= 0)
					{
						partsData.col = this.colBase;
						partsData.labelCol = this.colTitle;
						partsData.LRCol = this.colLR;
					}
					partsData.actCallBack = delegate()
					{
						if (chipFactoryCT > 0)
						{
							CMD_ChipReinforcement.Create(null);
						}
						else
						{
							CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
							cmd_ModalMessage.Title = StringMaster.GetString("ChipFactoryMissingAlertTitle");
							cmd_ModalMessage.Info = StringMaster.GetString("ChipFactoryMissingAlertInfo-2");
						}
					};
					break;
				case "VersionUpTitle":
					partsData.actCallBack = delegate()
					{
						GUIMain.ShowCommonDialog(null, "CMD_VersionUP");
					};
					break;
				}
				if (partsData.strCampaign != string.Empty)
				{
					partsData.isInfo = true;
				}
			}
		}
	}

	public void ShowDatas()
	{
		GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
		this.lbTX_Stone.text = playerInfo.point.ToString();
		this.lbTX_Chip.text = StringFormat.Cluster(playerInfo.gamemoney);
		this.lbTX_Meat.text = string.Format(StringMaster.GetString("SystemFraction"), playerInfo.meatNum, playerInfo.meatLimitMax);
		this.lbTX_HQ_Meat.text = Singleton<UserDataMng>.Instance.GetUserItemNumByItemId(50001).ToString();
		base.SetReOpendAction(new Func<int, bool>(this.UpdatePlayerInfo));
	}

	private void OnClickedShop()
	{
		GUIMain.ShowCommonDialog(delegate(int idx)
		{
			this.ShowDatas();
		}, "CMD_Shop");
	}

	private void OnClickedFarewellList()
	{
		CMD_FarewellListRun.Mode = CMD_FarewellListRun.MODE.SHOW;
		GUIMain.ShowCommonDialog(null, "CMD_FarewellListRun");
	}

	private int GetFacilityCount(int facilityID)
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		if (null == farmRoot)
		{
			global::Debug.LogError("FarmRoot Not Found");
			return -1;
		}
		int facilityCount = farmRoot.Scenery.GetFacilityCount(facilityID);
		List<UserFacility> stockFacilityListByfacilityIdAndLevel = Singleton<UserDataMng>.Instance.GetStockFacilityListByfacilityIdAndLevel(facilityID, -1);
		int count = stockFacilityListByfacilityIdAndLevel.Count;
		return facilityCount + count;
	}

	private void OnClickedChipList()
	{
		int facilityCount = this.GetFacilityCount(25);
		if (facilityCount > 0)
		{
			CMD_ChipAdministration.Create(null);
		}
		else
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("ChipFactoryMissingAlertTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("ChipFactoryMissingAlertInfo-3");
		}
	}

	private void OnClickedChipList(UISprite sp, UILabel lb)
	{
		int facilityCount = this.GetFacilityCount(25);
		if (facilityCount > 0)
		{
			sp.color = new Color(1f, 1f, 1f, 1f);
			lb.color = new Color(1f, 1f, 1f, 1f);
		}
		else
		{
			sp.color = new Color(0.5f, 0.5f, 0.5f, 1f);
			lb.color = new Color(0.5f, 0.5f, 0.5f, 1f);
		}
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	public override void ClosePanel(bool animation = true)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	private bool UpdatePlayerInfo(int noop)
	{
		GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
		this.lbTX_Chip.text = StringFormat.Cluster(playerInfo.gamemoney);
		return false;
	}
}
