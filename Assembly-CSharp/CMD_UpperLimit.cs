using FarmData;
using Master;
using System;
using UI.Common;
using UnityEngine;

public sealed class CMD_UpperLimit : CMD
{
	[SerializeField]
	private UILabel message;

	[SerializeField]
	private UILabel reinforcementButtonLabel;

	[SerializeField]
	private UILabel houseButtonLabel;

	[SerializeField]
	private UILabel gardenButtonLabel;

	protected override void Awake()
	{
		base.Awake();
		this.reinforcementButtonLabel.text = StringMaster.GetString("ReinforcementTitle");
		this.houseButtonLabel.text = StringMaster.GetString("HouseTitle");
		this.gardenButtonLabel.text = StringMaster.GetString("GardenTitle");
	}

	public void SetType(CMD_UpperLimit.MessageType type)
	{
		int facilityID = 7;
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(facilityID);
		bool flag = false;
		for (int i = 0; i < Singleton<UserDataMng>.Instance.userFacilityList.Count; i++)
		{
			UserFacility userFacility = Singleton<UserDataMng>.Instance.userFacilityList[i];
			if (userFacility.facilityId == int.Parse(facilityMaster.facilityId))
			{
				flag = (userFacility.level >= int.Parse(facilityMaster.maxLevel));
				break;
			}
		}
		if (flag)
		{
			if (type != CMD_UpperLimit.MessageType.GASHA)
			{
				if (type != CMD_UpperLimit.MessageType.PRESENTS)
				{
					if (type == CMD_UpperLimit.MessageType.QUEST)
					{
						this.message.text = StringMaster.GetString("PossessionOverQuest");
					}
				}
				else
				{
					this.message.text = StringMaster.GetString("PossessionOverPresent");
				}
			}
			else
			{
				this.message.text = StringMaster.GetString("PossessionOverGasha");
			}
		}
		else if (type != CMD_UpperLimit.MessageType.GASHA)
		{
			if (type != CMD_UpperLimit.MessageType.PRESENTS)
			{
				if (type == CMD_UpperLimit.MessageType.QUEST)
				{
					this.message.text = StringMaster.GetString("PossessionOverQuestUpgrade");
				}
			}
			else
			{
				this.message.text = StringMaster.GetString("PossessionOverPresentUpgrade");
			}
		}
		else
		{
			this.message.text = StringMaster.GetString("PossessionOverGashaUpgrade");
		}
	}

	public void SetNoticeMessage(LimitOverNoticeType type)
	{
		int num = 7;
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(num);
		bool flag = false;
		for (int i = 0; i < Singleton<UserDataMng>.Instance.userFacilityList.Count; i++)
		{
			UserFacility userFacility = Singleton<UserDataMng>.Instance.userFacilityList[i];
			if (userFacility.facilityId == num)
			{
				int num2 = 0;
				if (int.TryParse(facilityMaster.maxLevel, out num2))
				{
					flag = (num2 <= userFacility.level);
				}
				break;
			}
		}
		if (flag)
		{
			if (type != LimitOverNoticeType.GASHA)
			{
				if (type != LimitOverNoticeType.PRESENTS)
				{
					if (type == LimitOverNoticeType.QUEST)
					{
						this.message.text = StringMaster.GetString("PossessionOverQuest");
					}
				}
				else
				{
					this.message.text = StringMaster.GetString("PossessionOverPresent");
				}
			}
			else
			{
				this.message.text = StringMaster.GetString("PossessionOverGasha");
			}
		}
		else if (type != LimitOverNoticeType.GASHA)
		{
			if (type != LimitOverNoticeType.PRESENTS)
			{
				if (type == LimitOverNoticeType.QUEST)
				{
					this.message.text = StringMaster.GetString("PossessionOverQuestUpgrade");
				}
			}
			else
			{
				this.message.text = StringMaster.GetString("PossessionOverPresentUpgrade");
			}
		}
		else
		{
			this.message.text = StringMaster.GetString("PossessionOverGashaUpgrade");
		}
	}

	private void OnPushedReinforcementButton()
	{
		base.ClosePanel(true);
		base.SetCloseAction(delegate(int x)
		{
			GUIMain.ShowCommonDialog(null, "CMD_ReinforcementTOP", null);
		});
	}

	private void OnPushedHouseButton()
	{
		CMD_FarewellListRun.Mode = CMD_FarewellListRun.MODE.SELL;
		base.ClosePanel(true);
		base.SetCloseAction(delegate(int x)
		{
			GUIMain.ShowCommonDialog(null, "CMD_FarewellListRun", null);
		});
	}

	private void OnPushedGardenButton()
	{
		CMD_FarewellListRun.Mode = CMD_FarewellListRun.MODE.GARDEN_SELL;
		base.ClosePanel(true);
		base.SetCloseAction(delegate(int x)
		{
			GUIMain.ShowCommonDialog(null, "CMD_GardenList", null);
		});
	}

	public enum MessageType
	{
		GASHA,
		PRESENTS,
		QUEST
	}
}
