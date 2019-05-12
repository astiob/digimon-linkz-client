using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TitleDataMng
{
	public static Dictionary<int, GameWebAPI.RespDataMA_TitleMaster.TitleM> TitleM;

	public static List<GameWebAPI.RespDataTL_GetUserTitleList.UserTitleList> userTitleList { get; set; }

	public static GameWebAPI.RespDataTL_GetUserTitleList.UserTitleList GetEquipedUserTitle()
	{
		GameWebAPI.RespDataTL_GetUserTitleList.UserTitleList result = null;
		if (TitleDataMng.userTitleList != null)
		{
			GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
			result = TitleDataMng.userTitleList.FirstOrDefault((GameWebAPI.RespDataTL_GetUserTitleList.UserTitleList item) => item.titleId == playerInfo.titleId);
		}
		return result;
	}

	public static GameWebAPI.RespDataTL_GetUserTitleList.UserTitleList GetUserTitleByMasterId(string titleId)
	{
		GameWebAPI.RespDataTL_GetUserTitleList.UserTitleList result = null;
		if (TitleDataMng.userTitleList != null)
		{
			result = TitleDataMng.userTitleList.FirstOrDefault((GameWebAPI.RespDataTL_GetUserTitleList.UserTitleList item) => item.titleId == titleId);
		}
		return result;
	}

	public static Dictionary<int, GameWebAPI.RespDataMA_TitleMaster.TitleM> GetDictionaryTitleM()
	{
		if (TitleDataMng.TitleM == null)
		{
			TitleDataMng.TitleM = MasterDataMng.Instance().RespDataMA_TitleMaster.titleM.ToDictionary((GameWebAPI.RespDataMA_TitleMaster.TitleM x) => int.Parse(x.titleId));
		}
		return TitleDataMng.TitleM;
	}

	public static GameWebAPI.RespDataMA_TitleMaster.TitleM[] GetAvailableTitleM()
	{
		IEnumerable<GameWebAPI.RespDataMA_TitleMaster.TitleM> source = MasterDataMng.Instance().RespDataMA_TitleMaster.titleM.Where((GameWebAPI.RespDataMA_TitleMaster.TitleM _title) => _title.dispFlg == "1" || TitleDataMng.GetUserTitleByMasterId(_title.titleId) != null);
		return source.ToArray<GameWebAPI.RespDataMA_TitleMaster.TitleM>();
	}

	public static APIRequestTask RequestAPIUsetTitleList(bool requestRetry = true)
	{
		GameWebAPI.RequestTL_GetUserTitleList requestTL_GetUserTitleList = new GameWebAPI.RequestTL_GetUserTitleList();
		requestTL_GetUserTitleList.OnReceived = delegate(GameWebAPI.RespDataTL_GetUserTitleList response)
		{
			if (response.userTitleList != null)
			{
				TitleDataMng.userTitleList = new List<GameWebAPI.RespDataTL_GetUserTitleList.UserTitleList>(response.userTitleList);
			}
			else
			{
				TitleDataMng.userTitleList = new List<GameWebAPI.RespDataTL_GetUserTitleList.UserTitleList>();
			}
		};
		GameWebAPI.RequestTL_GetUserTitleList request = requestTL_GetUserTitleList;
		return new APIRequestTask(request, requestRetry);
	}

	public static void ClearCache()
	{
		if (TitleDataMng.TitleM != null)
		{
			TitleDataMng.TitleM.Clear();
			TitleDataMng.TitleM = null;
		}
	}

	public static APIRequestTask RequestUpdateEquipedTitle(int titleId, bool requestRetry = true)
	{
		GameWebAPI.RequestTL_EditUserTitle request = new GameWebAPI.RequestTL_EditUserTitle
		{
			SetSendData = delegate(GameWebAPI.SendDataTL_EditUserTitle param)
			{
				param.titleId = titleId;
			},
			OnReceived = delegate(GameWebAPI.RespDataTL_EditUserTitle noop)
			{
				GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
				GameWebAPI.RespDataPRF_Profile.UserDataProf userData = DataMng.Instance().RespDataPRF_Profile.userData;
				if (playerInfo != null)
				{
					playerInfo.titleId = titleId.ToString();
				}
				if (userData != null)
				{
					userData.titleId = titleId.ToString();
				}
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public static string GetImagePath(GameWebAPI.RespDataMA_TitleMaster.TitleM titleM)
	{
		return "HonorificThumbnail/" + titleM.img;
	}

	public static void SetTitleIcon(string titleId, UITexture targetTex)
	{
		if (titleId != null)
		{
			GameWebAPI.RespDataMA_TitleMaster.TitleM titleM = TitleDataMng.GetDictionaryTitleM()[int.Parse(titleId)];
			if (titleM != null)
			{
				string imagePath = TitleDataMng.GetImagePath(titleM);
				AssetDataMng.Instance().LoadObjectASync(imagePath, delegate(UnityEngine.Object obj)
				{
					if (obj != null)
					{
						Texture2D mainTexture = obj as Texture2D;
						targetTex.mainTexture = mainTexture;
					}
				});
			}
		}
	}
}
