using Evolution;
using FarmData;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class UserDataMng : Singleton<UserDataMng>
{
	public List<UserFacility> userFacilityList;

	public List<GameWebAPI.RespDataUS_ItemListLogic.UserItemData> userItemList;

	public List<UserFacility> userStockFacilityList = new List<UserFacility>();

	private bool isLoadedUserStockFacility;

	private List<LastHarvestTime> lastHarvestTimeList;

	private List<UserFacilityCondition> userFacilityConditionList;

	public DateTime playerStaminaBaseTime;

	public int[] monsterIdsInFarm;

	public bool IsLoadedUserStockFacility
	{
		get
		{
			return this.isLoadedUserStockFacility;
		}
		set
		{
			this.isLoadedUserStockFacility = value;
		}
	}

	public APIRequestTask RequestPlayerInfo(bool requestRetry = true)
	{
		GameWebAPI.RequestUS_UserStatus request = new GameWebAPI.RequestUS_UserStatus
		{
			OnReceived = delegate(GameWebAPI.RespDataUS_GetPlayerInfo response)
			{
				DataMng.Instance().RespDataUS_PlayerInfo = response;
				this.playerStaminaBaseTime = ServerDateTime.Now;
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public APIRequestTask RequestUserSoulData(bool requestRetry = true)
	{
		GameWebAPI.UserSoulInfoList userSoulInfoList = new GameWebAPI.UserSoulInfoList();
		userSoulInfoList.OnReceived = delegate(GameWebAPI.RespDataUS_GetSoulInfo response)
		{
			DataMng.Instance().RespDataUS_SoulInfo = response;
		};
		GameWebAPI.UserSoulInfoList request = userSoulInfoList;
		return new APIRequestTask(request, requestRetry);
	}

	public APIRequestTask RequestUserFacilityData(int requestUserid = 0, bool requestRetry = true)
	{
		GameWebAPI.RequestFA_UserFacilityList request = new GameWebAPI.RequestFA_UserFacilityList
		{
			SetSendData = delegate(GameWebAPI.FA_Req_RequestFA_UserFacilityList param)
			{
				param.userId = requestUserid;
			},
			OnReceived = delegate(GameWebAPI.RespDataFA_GetFacilityList response)
			{
				this.userFacilityList = response.userFacilityList.Where((UserFacility x) => x.facilityId != 6).ToList<UserFacility>();
				this.monsterIdsInFarm = response.monsterIdsInFarm;
				this.lastHarvestTimeList = response.lastHarvestTime.ToList<LastHarvestTime>();
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public APIRequestTask RequestUserStockFacilityData(int requestUserid = 0, bool requestRetry = true)
	{
		GameWebAPI.RequestFA_UserStockFacilityList request = new GameWebAPI.RequestFA_UserStockFacilityList
		{
			SetSendData = delegate(GameWebAPI.FA_Req_RequestFA_UserStockFacilityList param)
			{
				param.userId = requestUserid;
			},
			OnReceived = delegate(GameWebAPI.RespDataFA_GetStockFacilityList response)
			{
				this.userStockFacilityList = response.userFacilityList.Where((UserFacility x) => x.facilityId != 6).ToList<UserFacility>();
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public APIRequestTask RequestUserFacilityConditionData(bool requestRetry = true)
	{
		GameWebAPI.RequestFA_UserFacilityCondition request = new GameWebAPI.RequestFA_UserFacilityCondition
		{
			OnReceived = delegate(GameWebAPI.RespDataFA_UserFacilityCondition response)
			{
				this.userFacilityConditionList = response.facilityCondition.ToList<UserFacilityCondition>();
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public void DestroyUserFacilityData()
	{
		this.userFacilityList = null;
		this.userStockFacilityList = null;
		this.isLoadedUserStockFacility = false;
		this.lastHarvestTimeList = null;
		this.userFacilityConditionList = null;
	}

	public void AddUserFacility(UserFacility facility)
	{
		this.userFacilityList.Add(facility);
	}

	public List<UserFacility> GetUserFacilityList()
	{
		return this.userFacilityList;
	}

	public UserFacility GetUserFacility(int userFacilityID)
	{
		for (int i = 0; i < this.userFacilityList.Count; i++)
		{
			if (userFacilityID == this.userFacilityList[i].userFacilityId)
			{
				return this.userFacilityList[i];
			}
		}
		return null;
	}

	public UserFacility GetUserStorehouse()
	{
		for (int i = 0; i < this.userFacilityList.Count; i++)
		{
			if (this.userFacilityList[i].facilityId == 2)
			{
				return this.userFacilityList[i];
			}
		}
		return null;
	}

	public void DeleteUserFacility(int userFacilityID)
	{
		for (int i = 0; i < this.userFacilityList.Count; i++)
		{
			if (userFacilityID == this.userFacilityList[i].userFacilityId)
			{
				this.userFacilityList.Remove(this.userFacilityList[i]);
				break;
			}
		}
	}

	public void SetLastHarvestTime(LastHarvestTime[] newLastHarvestTimeList)
	{
		if (newLastHarvestTimeList == null)
		{
			this.lastHarvestTimeList = new List<LastHarvestTime>();
		}
		else
		{
			this.lastHarvestTimeList = newLastHarvestTimeList.ToList<LastHarvestTime>();
		}
	}

	public void AddLastHarvestTime(LastHarvestTime lastHarvestTime)
	{
		this.lastHarvestTimeList.Add(lastHarvestTime);
	}

	public LastHarvestTime GetLastHarvestTime(int userFacilityID)
	{
		return this.lastHarvestTimeList.SingleOrDefault((LastHarvestTime x) => x.userFacilityId == userFacilityID);
	}

	public bool IsOverUnitLimit(int count)
	{
		int num = 0;
		if (DataMng.Instance().RespDataUS_PlayerInfo.playerInfo != null)
		{
			num = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.unitLimitMax);
		}
		return num < count;
	}

	public bool IsOverChipLimit(int addCount = 0)
	{
		int chipLimitMax = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.chipLimitMax;
		if (ChipDataMng.userChipData != null && ChipDataMng.userChipData.userChipList != null)
		{
			GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] userChipList = ChipDataMng.userChipData.userChipList;
			return chipLimitMax < userChipList.Length + addCount;
		}
		return false;
	}

	public bool HasUserItem(int itemId)
	{
		if (this.userItemList != null)
		{
			for (int i = 0; i < this.userItemList.Count; i++)
			{
				if (this.userItemList[i].itemId == itemId)
				{
					return true;
				}
			}
		}
		return false;
	}

	public int GetUserItemNumByItemId(int itemId)
	{
		if (this.userItemList != null)
		{
			for (int i = 0; i < this.userItemList.Count; i++)
			{
				if (this.userItemList[i].itemId == itemId)
				{
					return this.userItemList[i].userItemNum;
				}
			}
		}
		return 0;
	}

	public void UpdateUserItemNum(int itemId, int diffNum)
	{
		if (this.HasUserItem(itemId))
		{
			if (this.userItemList != null)
			{
				for (int i = 0; i < this.userItemList.Count; i++)
				{
					if (this.userItemList[i].itemId == itemId)
					{
						this.userItemList[i].userItemNum += diffNum;
						break;
					}
				}
			}
		}
		else
		{
			this.AddUserItem(itemId, diffNum);
		}
	}

	private void AddUserItem(int id, int num)
	{
		GameWebAPI.RespDataUS_ItemListLogic.UserItemData item = new GameWebAPI.RespDataUS_ItemListLogic.UserItemData
		{
			itemId = id,
			userItemNum = num
		};
		this.userItemList.Add(item);
	}

	public bool ExistUserFacilityCondition()
	{
		return null != this.userFacilityConditionList;
	}

	public void ClearUserFacilityCondition()
	{
		this.userFacilityConditionList = null;
	}

	public UserFacilityCondition GetUserFacilityCondition(string key)
	{
		return this.userFacilityConditionList.SingleOrDefault((UserFacilityCondition x) => x.key == key);
	}

	public void RecoveryUserStamina(int staminaValue)
	{
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.stamina = staminaValue;
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.recovery = 0;
		this.playerStaminaBaseTime = ServerDateTime.Now;
	}

	public void ConsumeUserStamina(int staminaValue)
	{
		DateTime now = ServerDateTime.Now;
		TimeSpan timeSpan = now - this.playerStaminaBaseTime;
		if (DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.recovery - (int)timeSpan.TotalSeconds <= 0)
		{
			this.playerStaminaBaseTime = now;
			DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.recovery = 0;
		}
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.stamina -= staminaValue;
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.recovery += staminaValue * 180;
	}

	public void AddDigiStone(int addCount)
	{
		int point = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
		int point2 = Mathf.Clamp(point + addCount, 0, ConstValue.MAX_DIGISTONE_COUNT);
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point = point2;
	}

	public APIRequestTask RequestUserMonsterFriendshipTime(bool requestRetry = true)
	{
		GameWebAPI.RequestMN_FriendTimeCheck requestMN_FriendTimeCheck = new GameWebAPI.RequestMN_FriendTimeCheck();
		requestMN_FriendTimeCheck.SetSendData = delegate(GameWebAPI.MN_Req_FriendTimeCheck param)
		{
			GameWebAPI.RespDataMN_GetDeckList.DeckList favoriteDeckNumDeck = this.GetFavoriteDeckNumDeck();
			param.userMonsterIds = favoriteDeckNumDeck.monsterList.Select((GameWebAPI.RespDataMN_GetDeckList.MonsterList x) => x.userMonsterId).ToArray<string>();
		};
		requestMN_FriendTimeCheck.OnReceived = delegate(GameWebAPI.RespDataMN_FriendTimeCheck response)
		{
			FarmDigimonManager.FriendTimeList = response.friendshipTime;
		};
		GameWebAPI.RequestMN_FriendTimeCheck request = requestMN_FriendTimeCheck;
		return new APIRequestTask(request, requestRetry);
	}

	public int GetFavoriteDeckNum()
	{
		int num = 0;
		if (int.TryParse(DataMng.Instance().RespDataMN_DeckList.favoriteDeckNum, out num))
		{
			num = Mathf.Max(0, num - 1);
		}
		return num;
	}

	public GameWebAPI.RespDataMN_GetDeckList.DeckList GetFavoriteDeckNumDeck()
	{
		int favoriteDeckNum = this.GetFavoriteDeckNum();
		return DataMng.Instance().RespDataMN_DeckList.deckList[favoriteDeckNum];
	}

	public APIRequestTask RequestRecoverStamina(bool requestRetry = true)
	{
		GameWebAPI.RequestUS_RecoverLife request = new GameWebAPI.RequestUS_RecoverLife
		{
			OnReceived = delegate(WebAPI.ResponseData response)
			{
				GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
				int stamina = playerInfo.stamina;
				int num = int.Parse(playerInfo.staminaMax);
				this.RecoveryUserStamina(stamina + num);
				this.AddDigiStone(-ConstValue.RECOVER_STAMINA_DIGISTONE_NUM);
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public APIRequestTask RequestUpdateNickName(string newNickName, bool requestRetry = true)
	{
		GameWebAPI.RequestUS_UserUpdateNicknameLogic request = new GameWebAPI.RequestUS_UserUpdateNicknameLogic
		{
			SetSendData = delegate(GameWebAPI.PRF_Req_UpdateNickname param)
			{
				param.nickname = newNickName;
			},
			OnReceived = delegate(WebAPI.ResponseData noop)
			{
				DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.nickname = newNickName;
				DataMng.Instance().RespDataPRF_Profile.userData.nickname = newNickName;
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public APIRequestTask RequestUpdateComment(string newComment, bool requestRetry = true)
	{
		GameWebAPI.RequestUS_UpdateDescription request = new GameWebAPI.RequestUS_UpdateDescription
		{
			SetSendData = delegate(GameWebAPI.PRF_Req_UpdateDescription param)
			{
				param.description = newComment;
			},
			OnReceived = delegate(WebAPI.ResponseData noop)
			{
				DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.description = newComment;
				DataMng.Instance().RespDataPRF_Profile.userData.description = newComment;
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public APIRequestTask RequestUpdateBirthDay(string birth, bool requestRetry = true)
	{
		GameWebAPI.Request_UpdateBirthday request = new GameWebAPI.Request_UpdateBirthday
		{
			SetSendData = delegate(GameWebAPI.PRF_Req_UpdateBirthday param)
			{
				param.birthday = birth;
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public APIRequestTask RequestFriendProfile(string friendUserId, Action<GameWebAPI.RespDataPRF_Profile> onResponse, bool requestRetry = true)
	{
		GameWebAPI.RequestUS_UserProfile request = new GameWebAPI.RequestUS_UserProfile
		{
			SetSendData = delegate(GameWebAPI.PRF_Req_ProfileData param)
			{
				param.targetUserId = int.Parse(friendUserId);
			},
			OnReceived = onResponse
		};
		return new APIRequestTask(request, requestRetry);
	}

	public APIRequestTask RequestGardenInfo(bool requestRetry = true)
	{
		GameWebAPI.RequestUS_GetGardenInfo requestUS_GetGardenInfo = new GameWebAPI.RequestUS_GetGardenInfo();
		requestUS_GetGardenInfo.OnReceived = delegate(GameWebAPI.RespDataUS_GetGardenInfo response)
		{
			DataMng.Instance().RespDataUS_GardenInfo = response;
		};
		GameWebAPI.RequestUS_GetGardenInfo request = requestUS_GetGardenInfo;
		return new APIRequestTask(request, requestRetry);
	}

	public void AddUserStockFacility(UserFacility facility)
	{
		this.userStockFacilityList.Add(facility);
	}

	public void DeleteUserStockFacility(int userFacilityID)
	{
		for (int i = 0; i < this.userStockFacilityList.Count; i++)
		{
			if (userFacilityID == this.userStockFacilityList[i].userFacilityId)
			{
				this.userStockFacilityList.Remove(this.userStockFacilityList[i]);
				break;
			}
		}
	}

	public List<UserFacility> GetUserStockFacilityList()
	{
		return this.userStockFacilityList;
	}

	public UserFacility GetUserStockFacility(int userFacilityID)
	{
		for (int i = 0; i < this.userStockFacilityList.Count; i++)
		{
			if (userFacilityID == this.userStockFacilityList[i].userFacilityId)
			{
				return this.userStockFacilityList[i];
			}
		}
		return null;
	}

	public bool IsFacilityExistInUserStockFacility(int facilityId)
	{
		for (int i = 0; i < this.userStockFacilityList.Count; i++)
		{
			if (facilityId == this.userStockFacilityList[i].facilityId)
			{
				return true;
			}
		}
		return false;
	}

	public UserFacility GetStockFacilityByfacilityId(int facilityId)
	{
		for (int i = 0; i < this.userStockFacilityList.Count; i++)
		{
			if (facilityId == this.userStockFacilityList[i].facilityId)
			{
				return this.userStockFacilityList[i];
			}
		}
		return null;
	}

	public List<UserFacility> GetStockFacilityListByfacilityIdAndLevel(int facilityId, int level = -1)
	{
		List<UserFacility> list = new List<UserFacility>();
		for (int i = 0; i < this.userStockFacilityList.Count; i++)
		{
			if (facilityId == this.userStockFacilityList[i].facilityId && (level == this.userStockFacilityList[i].level || level == -1))
			{
				list.Add(this.userStockFacilityList[i]);
			}
		}
		return list;
	}

	public void RequestUserStockFacilityDataAPI(Action<bool> callback = null)
	{
		if (this.isLoadedUserStockFacility)
		{
			if (callback != null)
			{
				callback(true);
			}
			return;
		}
		APIRequestTask task = this.RequestUserStockFacilityData(0, false);
		base.StartCoroutine(task.Run(delegate
		{
			this.isLoadedUserStockFacility = true;
			if (callback != null)
			{
				callback(this.isLoadedUserStockFacility);
			}
		}, delegate(Exception exception)
		{
			this.isLoadedUserStockFacility = false;
			if (callback != null)
			{
				callback(this.isLoadedUserStockFacility);
			}
		}, null));
	}

	private GameWebAPI.UserSoulData GetUserSoulDataBySID(string soulId)
	{
		return EvolutionMaterialData.GetUserEvolutionMaterial(soulId);
	}

	public bool CheckMaterialCount(GameWebAPI.MonsterEvolutionMaterialMaster.Material materialMaster)
	{
		bool result = true;
		for (int i = 1; i <= 7; i++)
		{
			string assetValue = materialMaster.GetAssetValue(i);
			int num = int.Parse(materialMaster.GetAssetNum(i));
			GameWebAPI.UserSoulData userSoulDataBySID = this.GetUserSoulDataBySID(assetValue);
			if (userSoulDataBySID != null)
			{
				int num2 = int.Parse(userSoulDataBySID.num);
				if (num > num2)
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}
}
