using FarmData;
using Master;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UI.TrainingMenu
{
	public static class TrainingMenuButtonResistance
	{
		[CompilerGenerated]
		private static Action <>f__mg$cache0;

		[CompilerGenerated]
		private static Action <>f__mg$cache1;

		private static void EnableButtonPushAction()
		{
			GUIMain.ShowCommonDialog(null, "CMD_ArousalTOP", null);
		}

		private static void DisableButtonPushAction()
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("TrainingMissingAlertTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("TrainingMissingAlertInfo-02");
		}

		private static Action GetActionButtonPush(bool enableButton)
		{
			if (enableButton)
			{
				if (TrainingMenuButtonResistance.<>f__mg$cache0 == null)
				{
					TrainingMenuButtonResistance.<>f__mg$cache0 = new Action(TrainingMenuButtonResistance.EnableButtonPushAction);
				}
				return TrainingMenuButtonResistance.<>f__mg$cache0;
			}
			if (TrainingMenuButtonResistance.<>f__mg$cache1 == null)
			{
				TrainingMenuButtonResistance.<>f__mg$cache1 = new Action(TrainingMenuButtonResistance.DisableButtonPushAction);
			}
			return TrainingMenuButtonResistance.<>f__mg$cache1;
		}

		private static bool BuildedFacilityOnFarm(int facilityId)
		{
			bool result = false;
			List<UserFacility> userFacilityList = Singleton<UserDataMng>.Instance.GetUserFacilityList();
			for (int i = 0; i < userFacilityList.Count; i++)
			{
				if (userFacilityList[i].facilityId == facilityId && (string.IsNullOrEmpty(userFacilityList[i].completeTime) || 0 < userFacilityList[i].level))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		private static bool BuildedFacilityOnStock(int facilityId)
		{
			bool result = false;
			List<UserFacility> userStockFacilityList = Singleton<UserDataMng>.Instance.GetUserStockFacilityList();
			for (int i = 0; i < userStockFacilityList.Count; i++)
			{
				if (userStockFacilityList[i].facilityId == facilityId && (string.IsNullOrEmpty(userStockFacilityList[i].completeTime) || 0 < userStockFacilityList[i].level))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public static void SetButtonInfo(GUIListPartsTrainingMenu.PartsData menuButtonInfo, Color disableColor)
		{
			int facilityId = 4;
			if (TrainingMenuButtonResistance.BuildedFacilityOnFarm(facilityId) || TrainingMenuButtonResistance.BuildedFacilityOnStock(facilityId))
			{
				menuButtonInfo.actCallBack = TrainingMenuButtonResistance.GetActionButtonPush(true);
			}
			else
			{
				menuButtonInfo.actCallBack = TrainingMenuButtonResistance.GetActionButtonPush(false);
				menuButtonInfo.col = disableColor;
				menuButtonInfo.labelCol = disableColor;
				menuButtonInfo.LRCol = disableColor;
			}
		}
	}
}
