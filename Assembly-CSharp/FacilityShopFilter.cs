using FarmData;
using Quest;
using System;
using System.Collections.Generic;
using System.Linq;

public static class FacilityShopFilter
{
	public static FacilityM[] CheckFilter(FacilityM[] facilities)
	{
		if (facilities == null)
		{
			return null;
		}
		DateTime now = ServerDateTime.Now;
		List<FacilityM> list = new List<FacilityM>();
		for (int i = 0; i < facilities.Length; i++)
		{
			DateTime value = DateTime.Parse(facilities[i].openTime);
			DateTime value2 = DateTime.Parse(facilities[i].closeTime);
			if (now.CompareTo(value) >= 0 && now.CompareTo(value2) <= 0)
			{
				bool flag;
				if ("0" != facilities[i].releaseId)
				{
					FacilityConditionM[] facilityCondition = FarmDataManager.GetFacilityCondition(facilities[i].releaseId);
					flag = FacilityShopFilter.CheckDisplayCondition(facilityCondition);
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					list.Add(facilities[i]);
				}
			}
		}
		return list.ToArray();
	}

	private static bool CheckDisplayCondition(FacilityConditionM[] facilityConditions)
	{
		for (int i = 0; i < facilityConditions.Length; i++)
		{
			if (!("0" == facilityConditions[i].viewFlg))
			{
				return true;
			}
			if (FacilityShopFilter.CheckFacilityCondition(facilityConditions[i]))
			{
				return true;
			}
		}
		return false;
	}

	public static bool CheckFacilityCondition(FacilityConditionM condition)
	{
		switch (int.Parse(condition.conditionType))
		{
		case 1:
			return FacilityShopFilter.CheckFacilityConditionKey(condition.facilityConditionId);
		case 2:
			return FacilityShopFilter.CheckFacilityConditionWorldArea(condition.conditionParam);
		case 3:
			return FacilityShopFilter.CheckFacilityConditionFacilityNum(condition.conditionParam, condition.conditionValue, condition.operatorType);
		case 4:
			return FacilityShopFilter.CheckFacilityConditionFacilityLevel(condition.conditionParam, condition.conditionValue, condition.operatorType);
		case 5:
			return FacilityShopFilter.CheckFacilityConditionKey(condition.facilityConditionId);
		default:
			return false;
		}
	}

	private static bool CheckFacilityConditionKey(string key)
	{
		UserFacilityCondition userFacilityCondition = Singleton<UserDataMng>.Instance.GetUserFacilityCondition(key);
		return userFacilityCondition != null && 1 == userFacilityCondition.flg;
	}

	private static bool CheckFacilityConditionWorldArea(string areaId)
	{
		GameWebAPI.RespDataWD_GetDungeonInfo dngeonInfoByWorldId = ClassSingleton<QuestData>.Instance.GetDngeonInfoByWorldId(areaId);
		if (dngeonInfoByWorldId.worldDungeonInfo != null)
		{
			for (int i = 0; i < dngeonInfoByWorldId.worldDungeonInfo.Length; i++)
			{
				GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons[] dungeons = dngeonInfoByWorldId.worldDungeonInfo[i].dungeons;
				for (int j = 0; j < dungeons.Length; j++)
				{
					if (dungeons[j].status < 4)
					{
						return false;
					}
				}
			}
			return true;
		}
		return false;
	}

	private static bool CheckFacilityConditionFacilityNum(string facilityId, string num, string operatorType)
	{
		List<UserFacility> userFacilityList = Singleton<UserDataMng>.Instance.GetUserFacilityList();
		if (userFacilityList != null)
		{
			int id = int.Parse(facilityId);
			int arg = userFacilityList.Count((UserFacility x) => x.facilityId == id && x.level != 0);
			return FacilityShopFilter.CheckFacilityConditionOperatorType(int.Parse(num), operatorType, arg);
		}
		return false;
	}

	private static bool CheckFacilityConditionFacilityLevel(string facilityId, string level, string operatorType)
	{
		List<UserFacility> userFacilityList = Singleton<UserDataMng>.Instance.GetUserFacilityList();
		if (userFacilityList != null)
		{
			int id = int.Parse(facilityId);
			int lv = int.Parse(level);
			return userFacilityList.Where((UserFacility x) => x.facilityId == id).Any((UserFacility x) => FacilityShopFilter.CheckFacilityConditionOperatorType(lv, operatorType, x.level));
		}
		return false;
	}

	private static bool CheckFacilityConditionOperatorType(int arg1, string operatorType, int arg2)
	{
		switch (int.Parse(operatorType))
		{
		case 1:
			return arg1 == arg2;
		case 2:
			return arg1 <= arg2;
		case 3:
			return arg1 < arg2;
		case 4:
			return arg1 >= arg2;
		case 5:
			return arg1 > arg2;
		default:
			return false;
		}
	}
}
