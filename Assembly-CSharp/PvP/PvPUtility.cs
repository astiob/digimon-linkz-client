﻿using System;
using UnityEngine;

namespace PvP
{
	public static class PvPUtility
	{
		public static APIRequestTask RequestColosseumUserStatus(PvPUtility.RequestUserStatusType targetUserType, Action<GameWebAPI.RespData_ColosseumUserStatusLogic> onReceived)
		{
			string targetType = GameWebAPI.ReqData_ColosseumUserStatusLogic.GetTargetMyFlag();
			if (targetUserType == PvPUtility.RequestUserStatusType.ENEMY)
			{
				targetType = GameWebAPI.ReqData_ColosseumUserStatusLogic.GetTargetEnemyFlag();
			}
			GameWebAPI.ColosseumUserStatusLogic request = new GameWebAPI.ColosseumUserStatusLogic
			{
				SetSendData = delegate(GameWebAPI.ReqData_ColosseumUserStatusLogic param)
				{
					param.target = targetType;
					param.isMockBattle = 0;
				},
				OnReceived = onReceived
			};
			return new APIRequestTask(request, true);
		}

		public static APIRequestTask RequestMockBattleEntry(bool requestRetry = false)
		{
			return PvPUtility.RequestColosseumEntry(null, null, true, false);
		}

		public static APIRequestTask RequestColosseumEntry(GameWebAPI.RespData_ColosseumInfoLogic colosseumInfo, Action<GameWebAPI.RespDataCL_ColosseumEntry> onReceived, bool isMockBattle = false, bool requestRetry = false)
		{
			int colosseumId = 0;
			if (colosseumInfo != null && colosseumInfo.colosseumId != 0)
			{
				colosseumId = colosseumInfo.colosseumId;
			}
			GameWebAPI.RequestCL_ColosseumEntry request = new GameWebAPI.RequestCL_ColosseumEntry
			{
				SetSendData = delegate(GameWebAPI.SendDataCL_ColosseumEntry param)
				{
					param.colosseumId = colosseumId;
					param.isMockBattle = ((!isMockBattle) ? 0 : 1);
				},
				OnReceived = onReceived
			};
			return new APIRequestTask(request, requestRetry);
		}

		public static bool CopyUserEntryStatus(GameWebAPI.RespData_ColosseumUserStatusLogic dst, GameWebAPI.RespDataCL_ColosseumEntry src)
		{
			bool result = true;
			if (src != null && src.userStatus != null)
			{
				dst.userStatus = src.userStatus;
				dst.userStatus.nickname = DataMng.Instance().UserName;
				dst.freeCostBattleCount = src.freeCostBattleCount;
			}
			else
			{
				dst.userStatus = null;
				result = false;
			}
			return result;
		}

		public static GameWebAPI.ReqData_ColosseumBattleEndLogic.BattleResult GetBattleResult(GameWebAPI.RespData_ColosseumUserStatusLogic.ResultCode battleState)
		{
			GameWebAPI.ReqData_ColosseumBattleEndLogic.BattleResult result;
			if (battleState == GameWebAPI.RespData_ColosseumUserStatusLogic.ResultCode.BATTLE_INTERRUPTION_WIN)
			{
				result = GameWebAPI.ReqData_ColosseumBattleEndLogic.BattleResult.VICTORY;
			}
			else
			{
				result = GameWebAPI.ReqData_ColosseumBattleEndLogic.BattleResult.DEFEAT;
			}
			return result;
		}

		public static APIRequestTask RequestColosseumBattleEnd(GameWebAPI.ReqData_ColosseumBattleEndLogic.BattleResult result, GameWebAPI.ReqData_ColosseumBattleEndLogic.BattleMode mode)
		{
			GameWebAPI.ColosseumBattleEndLogic request = new GameWebAPI.ColosseumBattleEndLogic
			{
				SetSendData = delegate(GameWebAPI.ReqData_ColosseumBattleEndLogic param)
				{
					param.battleResult = (int)result;
					param.roundCount = 0;
					param.skillUseDeckPosition = "0";
					param.isMockBattle = (int)mode;
				}
			};
			return new APIRequestTask(request, true);
		}

		public static void SetPvPTopNoticeCode(GameWebAPI.RespData_ColosseumUserStatusLogic res)
		{
			if (!string.IsNullOrEmpty(res.noticeCode))
			{
				PlayerPrefs.SetString("ColosseumTopNoticeCode", res.noticeCode);
				PlayerPrefs.Save();
			}
		}

		public static string PopPvPTopNoticeCode()
		{
			string result = null;
			if (PlayerPrefs.HasKey("ColosseumTopNoticeCode"))
			{
				result = PlayerPrefs.GetString("ColosseumTopNoticeCode");
				PlayerPrefs.DeleteKey("ColosseumTopNoticeCode");
			}
			return result;
		}

		public enum RequestUserStatusType
		{
			MY,
			ENEMY
		}
	}
}
