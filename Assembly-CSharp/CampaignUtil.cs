using Master;
using System;

public class CampaignUtil
{
	public static string GetDescription(GameWebAPI.RespDataCP_Campaign.CampaignType cpmType, float rate, bool useLongDescription = false)
	{
		string format = (rate != (float)((int)rate)) ? "f1" : "f0";
		string result = string.Empty;
		if (!useLongDescription)
		{
			switch (cpmType)
			{
			case GameWebAPI.RespDataCP_Campaign.CampaignType.Invalid:
				break;
			case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestExpUp:
				result = string.Format(StringMaster.GetString("CampaignExp"), rate.ToString(format));
				goto IL_2CB;
			case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestCipUp:
				result = string.Format(StringMaster.GetString("CampaignTip"), rate.ToString(format));
				goto IL_2CB;
			case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestMatUp:
				result = string.Format(StringMaster.GetString("CampaignDrop"), rate.ToString(format));
				goto IL_2CB;
			case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestRrDrpUp:
				result = string.Format(StringMaster.GetString("CampaignDropRare"), rate.ToString(format));
				goto IL_2CB;
			case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestFriUp:
				result = string.Format(StringMaster.GetString("CampaignFriend"), rate.ToString(format));
				goto IL_2CB;
			case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDown:
				result = string.Format(StringMaster.GetString("CampaignStamina"), rate.ToString(format));
				goto IL_2CB;
			default:
				switch (cpmType)
				{
				case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestExpUpMul:
					result = string.Format(StringMaster.GetString("CampaignExp"), rate.ToString(format));
					goto IL_2CB;
				case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestCipUpMul:
					result = string.Format(StringMaster.GetString("CampaignTip"), rate.ToString(format));
					goto IL_2CB;
				case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestMatUpMul:
					result = string.Format(StringMaster.GetString("CampaignDrop"), rate.ToString(format));
					goto IL_2CB;
				case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestRrDrpUpMul:
					result = string.Format(StringMaster.GetString("CampaignDropRare"), rate.ToString(format));
					goto IL_2CB;
				case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestFriUpMul:
					result = string.Format(StringMaster.GetString("CampaignFriend"), rate.ToString(format));
					goto IL_2CB;
				case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDownMul:
					result = string.Format(StringMaster.GetString("CampaignStamina"), rate.ToString(format));
					goto IL_2CB;
				default:
					switch (cpmType)
					{
					case GameWebAPI.RespDataCP_Campaign.CampaignType.TrainExpUp:
						result = string.Format(StringMaster.GetString("CampaignExp"), rate.ToString(format));
						goto IL_2CB;
					case GameWebAPI.RespDataCP_Campaign.CampaignType.TrainCostDown:
						result = string.Format(StringMaster.GetString("CampaignTipOff"), rate.ToString(format));
						goto IL_2CB;
					case GameWebAPI.RespDataCP_Campaign.CampaignType.MeatExpUp:
						result = string.Format(StringMaster.GetString("CampaignExp"), rate.ToString(format));
						goto IL_2CB;
					case GameWebAPI.RespDataCP_Campaign.CampaignType.TrainLuckUp:
						result = string.Format(StringMaster.GetString("CampaignLuckUp"), rate.ToString(format));
						goto IL_2CB;
					default:
						if (cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.MeatHrvUp)
						{
							result = string.Format(StringMaster.GetString("CampaignMeat"), rate.ToString(format));
							goto IL_2CB;
						}
						if (cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.MedalTakeOverUp)
						{
							result = string.Format(StringMaster.GetString("CampaignMedalTakeOverUp"), rate.ToString(format));
							goto IL_2CB;
						}
						break;
					}
					break;
				}
				break;
			}
			result = string.Format(StringMaster.GetString("Campaign"), rate.ToString(format));
			IL_2CB:;
		}
		else
		{
			switch (cpmType)
			{
			case GameWebAPI.RespDataCP_Campaign.CampaignType.Invalid:
				break;
			case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestExpUp:
				return string.Format(StringMaster.GetString("CampaignNowExp"), rate.ToString(format));
			case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestCipUp:
				return string.Format(StringMaster.GetString("CampaignNowTip"), rate.ToString(format));
			case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestMatUp:
				return string.Format(StringMaster.GetString("CampaignNowDrop"), rate.ToString(format));
			case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestRrDrpUp:
				return string.Format(StringMaster.GetString("CampaignNowDropRare"), rate.ToString(format));
			case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestFriUp:
				return string.Format(StringMaster.GetString("CampaignNowFriend"), rate.ToString(format));
			case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDown:
				return string.Format(StringMaster.GetString("CampaignNowStamina"), rate.ToString(format));
			default:
				switch (cpmType)
				{
				case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestExpUpMul:
					return string.Format(StringMaster.GetString("CampaignNowExp"), rate.ToString(format));
				case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestCipUpMul:
					return string.Format(StringMaster.GetString("CampaignNowTip"), rate.ToString(format));
				case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestMatUpMul:
					return string.Format(StringMaster.GetString("CampaignNowDrop"), rate.ToString(format));
				case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestRrDrpUpMul:
					return string.Format(StringMaster.GetString("CampaignNowDropRare"), rate.ToString(format));
				case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestFriUpMul:
					return string.Format(StringMaster.GetString("CampaignNowFriend"), rate.ToString(format));
				case GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDownMul:
					return string.Format(StringMaster.GetString("CampaignNowStamina"), rate.ToString(format));
				default:
					switch (cpmType)
					{
					case GameWebAPI.RespDataCP_Campaign.CampaignType.TrainExpUp:
						return string.Format(StringMaster.GetString("CampaignNowExp"), rate.ToString(format));
					case GameWebAPI.RespDataCP_Campaign.CampaignType.TrainCostDown:
						rate = (1f - rate) * 100f;
						return string.Format(StringMaster.GetString("CampaignNowTipOff"), rate.ToString(format));
					case GameWebAPI.RespDataCP_Campaign.CampaignType.MeatExpUp:
						return string.Format(StringMaster.GetString("CampaignNowExp"), rate.ToString(format));
					case GameWebAPI.RespDataCP_Campaign.CampaignType.TrainLuckUp:
						return string.Format(StringMaster.GetString("CampaignNowLuckUp"), rate.ToString(format));
					default:
						if (cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.MeatHrvUp)
						{
							return string.Empty;
						}
						if (cpmType == GameWebAPI.RespDataCP_Campaign.CampaignType.MedalTakeOverUp)
						{
							return string.Format(StringMaster.GetString("CampaignNowMedalTakeOverUp"), rate.ToString(format));
						}
						break;
					}
					break;
				}
				break;
			}
			result = string.Empty;
		}
		return result;
	}
}
