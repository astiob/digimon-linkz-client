using FarmData;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class FarmUtility
{
	public static double RestSeconds(string limitTime)
	{
		if (!string.IsNullOrEmpty(limitTime))
		{
			return (DateTime.Parse(limitTime) - ServerDateTime.Now).TotalSeconds;
		}
		return 0.0;
	}

	public static int GetBuildFacilityCount()
	{
		List<UserFacility> userFacilityList = Singleton<UserDataMng>.Instance.GetUserFacilityList();
		int num = 0;
		for (int i = 0; i < userFacilityList.Count; i++)
		{
			UserFacility userFacility = userFacilityList[i];
			if (userFacility != null && !string.IsNullOrEmpty(userFacility.completeTime))
			{
				num++;
			}
		}
		return num;
	}

	public static Vector3 GetDistanceToGround()
	{
		Vector3 result = Vector3.zero;
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			GUICameraControll component = instance.Camera.GetComponent<GUICameraControll>();
			result = component.distanceToGround;
		}
		return result;
	}

	public static string GetConstructionModelName(FarmObject farmObject)
	{
		return string.Format("Farm/Builds/Construction/{0}x{1}/construction_{0}x{1}", farmObject.sizeX, farmObject.sizeY);
	}

	public static string GetDateString(DateTime time)
	{
		return time.ToString("yyyy-MM-dd HH:mm:ss");
	}

	public static bool IsShortage(string categoryId, string cost)
	{
		bool result = false;
		int cost2 = 0;
		if (int.TryParse(cost, out cost2))
		{
			result = FarmUtility.IsShortageToInt(categoryId, cost2);
		}
		return result;
	}

	public static bool IsShortageToInt(string categoryId, int cost)
	{
		bool result = false;
		int num = 0;
		if (int.TryParse(categoryId, out num))
		{
			int num2 = 0;
			if (num == 4)
			{
				int.TryParse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney, out num2);
			}
			else
			{
				num2 = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
			}
			result = (cost > num2);
		}
		return result;
	}

	public static void PayCost(string categoryID, string cost)
	{
		GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
		if (int.Parse(categoryID) == 4)
		{
			playerInfo.gamemoney = (int.Parse(playerInfo.gamemoney) - int.Parse(cost)).ToString();
		}
		else
		{
			playerInfo.point -= int.Parse(cost);
		}
		GUIPlayerStatus.RefreshParams_S(false);
	}

	public static int GetShortCutDigiStoneCost(string categoryId, string num, string completeTime)
	{
		int num2 = 0;
		int result = 0;
		if (int.TryParse(num, out result) && int.TryParse(categoryId, out num2) && num2 == 2 && !string.IsNullOrEmpty(completeTime))
		{
			int num3 = (int)(DateTime.Parse(completeTime) - ServerDateTime.Now).TotalSeconds;
			int num4 = num3 / 3600;
			int num5 = num4 * 2;
			result = num5 + ((num3 <= num4 * 3600) ? 0 : 2);
		}
		return result;
	}

	public static string GetCostString(string categoryId, int costValue)
	{
		string result = "0";
		int num = 0;
		if (int.TryParse(categoryId, out num))
		{
			if (num == 4)
			{
				result = StringFormat.Cluster(costValue);
			}
			else
			{
				result = costValue.ToString();
			}
		}
		return result;
	}

	public static bool IsExtendBuild(int facilityID)
	{
		bool result = false;
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(facilityID);
		if (facilityMaster != null && "0" != facilityMaster.isExtend)
		{
			result = true;
		}
		return result;
	}

	public static bool IsWalkBuild(int facilityID)
	{
		bool result = false;
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(facilityID);
		if (facilityMaster != null && "0" != facilityMaster.isWalk)
		{
			result = true;
		}
		return result;
	}

	public static bool IsPassableGrid(List<FarmGrid.Grid> grids, int gridIndex, bool isIgnorePutedFlag)
	{
		bool result;
		if (isIgnorePutedFlag)
		{
			result = !grids[gridIndex].invalid;
		}
		else
		{
			result = (!grids[gridIndex].invalid && (!grids[gridIndex].put || !grids[gridIndex].impassable));
		}
		return result;
	}
}
