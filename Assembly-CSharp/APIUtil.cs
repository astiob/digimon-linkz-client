using Colosseum.DeckUI;
using FarmData;
using Master;
using Monster;
using Neptune.Common;
using Neptune.OAuth;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebAPIRequest;

public sealed class APIUtil : MonoBehaviour
{
	private static APIUtil instance;

	private int requestId;

	public bool alertOnlyCloseButton;

	public static APIUtil Instance()
	{
		return APIUtil.instance;
	}

	private void Awake()
	{
		APIUtil.instance = this;
		this.InitRequestID();
	}

	private void OnDestroy()
	{
		APIUtil.instance = null;
	}

	private void InitRequestID()
	{
		int num = DateTime.Now.Hour * 10000;
		int num2 = DateTime.Now.Minute * 100;
		int second = DateTime.Now.Second;
		this.requestId = (num + num2 + second) * 100;
	}

	public int GetRequestID()
	{
		return this.requestId;
	}

	public void UpdateRequestID()
	{
		this.requestId++;
	}

	public APIRequestTask RequestFriendData(bool requestRetry = true)
	{
		RequestList requestList = new RequestList();
		GameWebAPI.RequestFR_FriendList requestFR_FriendList = new GameWebAPI.RequestFR_FriendList();
		requestFR_FriendList.OnReceived = delegate(GameWebAPI.RespDataFR_FriendList response)
		{
			DataMng.Instance().RespDataFR_FriendList = response;
		};
		RequestBase addRequest = requestFR_FriendList;
		requestList.AddRequest(addRequest);
		GameWebAPI.RequestFR_FriendApplicationList requestFR_FriendApplicationList = new GameWebAPI.RequestFR_FriendApplicationList();
		requestFR_FriendApplicationList.OnReceived = delegate(GameWebAPI.RespDataFR_FriendRequestList response)
		{
			DataMng.Instance().RespDataFR_FriendRequestList = response;
		};
		addRequest = requestFR_FriendApplicationList;
		requestList.AddRequest(addRequest);
		GameWebAPI.RequestFR_FriendUnapprovedList requestFR_FriendUnapprovedList = new GameWebAPI.RequestFR_FriendUnapprovedList();
		requestFR_FriendUnapprovedList.OnReceived = delegate(GameWebAPI.RespDataFR_FriendUnapprovedList response)
		{
			DataMng.Instance().RespDataFR_FriendUnapprovedList = response;
		};
		addRequest = requestFR_FriendUnapprovedList;
		requestList.AddRequest(addRequest);
		GameWebAPI.RequestFR_FriendInfo requestFR_FriendInfo = new GameWebAPI.RequestFR_FriendInfo();
		requestFR_FriendInfo.OnReceived = delegate(GameWebAPI.RespDataFR_FriendInfo response)
		{
			DataMng.Instance().RespDataFR_FriendInfo = response;
		};
		addRequest = requestFR_FriendInfo;
		requestList.AddRequest(addRequest);
		return new APIRequestTask(requestList, requestRetry);
	}

	public APIRequestTask RequestFriendApplication(string friendUserId, bool requestRetry = true)
	{
		GameWebAPI.RequestFR_FriendApplication request = new GameWebAPI.RequestFR_FriendApplication
		{
			SetSendData = delegate(GameWebAPI.FR_Req_FriendRequest param)
			{
				param.targetUserId = int.Parse(friendUserId);
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public APIRequestTask RequestFriendApplicationDecision(string[] friendUserIds, GameWebAPI.FR_Req_FriendDecision.DecisionType decisionType, bool requestRetry = true)
	{
		int[] targetUserIds = new int[friendUserIds.Length];
		for (int i = 0; i < friendUserIds.Length; i++)
		{
			targetUserIds[i] = int.Parse(friendUserIds[i]);
		}
		GameWebAPI.RequestFR_FriendApplicationDecision request = new GameWebAPI.RequestFR_FriendApplicationDecision
		{
			SetSendData = delegate(GameWebAPI.FR_Req_FriendDecision param)
			{
				param.targetUserIds = targetUserIds;
				param.decide = (int)decisionType;
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public APIRequestTask RequestFriendApplicationCancel(string[] friendUserIds, bool requestRetry = true)
	{
		int[] targetUserIds = new int[friendUserIds.Length];
		for (int i = 0; i < friendUserIds.Length; i++)
		{
			targetUserIds[i] = int.Parse(friendUserIds[i]);
		}
		GameWebAPI.RequestFR_FriendApplicationCancel request = new GameWebAPI.RequestFR_FriendApplicationCancel
		{
			SetSendData = delegate(GameWebAPI.FR_Req_FriendRequestCancel param)
			{
				param.targetUserIds = targetUserIds;
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public APIRequestTask RequestFriendBreak(string[] friendUserIds, bool requestRetry = true)
	{
		int[] targetUserIds = new int[friendUserIds.Length];
		for (int i = 0; i < friendUserIds.Length; i++)
		{
			targetUserIds[i] = int.Parse(friendUserIds[i]);
		}
		GameWebAPI.RequestFR_FriendBreak request = new GameWebAPI.RequestFR_FriendBreak
		{
			SetSendData = delegate(GameWebAPI.FR_Req_FriendBreak param)
			{
				param.targetUserIds = targetUserIds;
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public IEnumerator OAuthLogin(Action<string> onCompleted)
	{
		string result = null;
		Action onSuccess = delegate()
		{
			result = "Success";
		};
		WebAPIException exception = null;
		Action<WebAPIException> onException = delegate(WebAPIException ex)
		{
			exception = ex;
			result = "Failed";
		};
		this.AuthStart(onSuccess, onException);
		while (string.IsNullOrEmpty(result))
		{
			yield return null;
		}
		if ("Success" == result)
		{
			onCompleted(result);
		}
		else if (exception != null)
		{
			APIAlert apiAlert = new APIAlert();
			if (!exception.IsBackTopScreenError())
			{
				RestrictionInput.SuspensionLoad();
				apiAlert.NetworkAPIError(exception, true);
				apiAlert.ClosedAction = delegate()
				{
					RestrictionInput.ResumeLoad();
					onCompleted(result);
				};
			}
			else
			{
				apiAlert.NetworkAPIException(exception);
			}
		}
		else
		{
			string title = StringMaster.GetString("AuthFailedTitle");
			string message = StringMaster.GetString("AuthFailedInfo");
			Action<int> onClosed = delegate(int nop)
			{
				RestrictionInput.ResumeLoad();
				onCompleted(result);
			};
			RestrictionInput.SuspensionLoad();
			AlertManager.ShowAlertDialog(onClosed, title, message, AlertManager.ButtonActionType.Retry, false);
		}
		yield break;
	}

	private void AuthStart(Action onSuccess, Action<WebAPIException> onFailed)
	{
		float timeout = GameWebAPI.Instance().GetTimeout();
		NpOAuth.Instance.X_AppVer = WebAPIPlatformValue.GetAppVersion();
		NpOAuth.Instance.initURL = WebAPIPlatformValue.GetHttpAddressAuth();
		NpOAuth.Instance.counsumerKey = WebAPIPlatformValue.GetCounsumerKey();
		NpOAuth.Instance.counsumerSecret = WebAPIPlatformValue.GetCounsumerSecret();
		NpOAuth.Instance.TimeOut = timeout;
		NpOAuth.Instance.type = NpOAuthType.Json;
		Action<NpOAuthErrData> faildAction = delegate(NpOAuthErrData errData)
		{
			if (errData.FailedCode != NpOatuhFailedCodeE.ServerFailed)
			{
				onFailed(null);
			}
			else
			{
				WWWResponse request_result = new WWWResponse
				{
					responseJson = errData.VenusRespones_.ResJson,
					errorText = errData.VenusRespones_.Message,
					errorStatus = WWWResponse.LocalErrorStatus.NONE
				};
				try
				{
					GameWebAPI.Instance().GetResponseJson(request_result);
				}
				catch (WebAPIException obj)
				{
					onFailed(obj);
				}
			}
		};
		NpAes aes = new NpAes();
		NpOAuth.Instance.Init(this, onSuccess, faildAction, aes);
	}

	public IEnumerator StartGameLogin()
	{
		GameWebAPI.RequestCM_Login requestCM_Login = new GameWebAPI.RequestCM_Login();
		requestCM_Login.SetSendData = delegate(GameWebAPI.RequestCM_LoginRequest param)
		{
			param.osType = "2";
			param.modelName = SystemInfo.deviceModel;
			param.osVersion = NpDeviceInfo.GetOSVersion();
			param.appVersion = WebAPIPlatformValue.GetAppVersion();
		};
		requestCM_Login.OnReceived = new Action<GameWebAPI.RespDataCM_Login>(this.OnRecievedLogin);
		GameWebAPI.RequestCM_Login request = requestCM_Login;
		return request.Run(null, null, null);
	}

	private void OnRecievedLogin(GameWebAPI.RespDataCM_Login responseData)
	{
		ServerDateTime.Initialize(responseData.nowDateTime);
		DataMng.Instance().RespDataCM_Login = responseData;
		OptionSetting.Instance.Initialize(DataMng.Instance().RespDataCM_Login.GetOptionList_ProvisionalFunction());
	}

	public APIRequestTask RequestHomeData()
	{
		RequestList requestList = new RequestList();
		requestList.AddRequest(new GameWebAPI.RequestUS_UserStatus
		{
			OnReceived = new Action<GameWebAPI.RespDataUS_GetPlayerInfo>(this.OnRecievedUserStatus)
		});
		requestList.AddRequest(new GameWebAPI.RequestTL_GetUserTitleList
		{
			OnReceived = new Action<GameWebAPI.RespDataTL_GetUserTitleList>(this.OnRecievedUserTitleList)
		});
		requestList.AddRequest(ColosseumDeckWeb.Request());
		if (ClassSingleton<MonsterUserDataMng>.Instance.GetMonsterNum() == 0)
		{
			requestList.AddRequest(new GameWebAPI.RequestMonsterList
			{
				OnReceived = new Action<GameWebAPI.RespDataUS_GetMonsterList>(this.OnRecievedUserMonster)
			});
		}
		requestList.AddRequest(new GameWebAPI.RequestMN_DeckList
		{
			OnReceived = new Action<GameWebAPI.RespDataMN_GetDeckList>(this.OnRecievedDeckList)
		});
		requestList.AddRequest(new GameWebAPI.ReqDataCS_ChipListLogic
		{
			OnReceived = new Action<GameWebAPI.RespDataCS_ChipListLogic>(this.OnRecievedUserChipList)
		});
		requestList.AddRequest(new GameWebAPI.ItemListLogic
		{
			OnReceived = new Action<GameWebAPI.RespDataUS_ItemListLogic>(this.OnRecievedItemList)
		});
		requestList.AddRequest(new GameWebAPI.MonsterSlotInfoListLogic
		{
			OnReceived = new Action<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic>(this.OnRecievedMonsterSlotInfo)
		});
		requestList.AddRequest(new GameWebAPI.UserSoulInfoList
		{
			OnReceived = new Action<GameWebAPI.RespDataUS_GetSoulInfo>(this.OnRecievedUserSoulData)
		});
		GameWebAPI.RequestFA_UserFacilityList requestFA_UserFacilityList = new GameWebAPI.RequestFA_UserFacilityList();
		requestFA_UserFacilityList.SetSendData = delegate(GameWebAPI.FA_Req_RequestFA_UserFacilityList param)
		{
			param.userId = DataMng.Instance().RespDataCM_Login.playerInfo.UserId;
		};
		requestFA_UserFacilityList.OnReceived = new Action<GameWebAPI.RespDataFA_GetFacilityList>(this.OnRecievedUserFacility);
		GameWebAPI.RequestFA_UserFacilityList addRequest = requestFA_UserFacilityList;
		requestList.AddRequest(addRequest);
		requestList.AddRequest(new GameWebAPI.RequestCM_LoginBonus
		{
			OnReceived = new Action<GameWebAPI.RespDataCM_LoginBonus>(this.OnRecievedLoginBonus)
		});
		requestList.AddRequest(new GameWebAPI.RequestMP_MyPage
		{
			OnReceived = new Action<GameWebAPI.RespDataMP_MyPage>(this.OnRecievedMyPage)
		});
		GameWebAPI.RequestCP_Campaign requestCP_Campaign = new GameWebAPI.RequestCP_Campaign();
		requestCP_Campaign.SetSendData = delegate(GameWebAPI.CP_Req_Campaign param)
		{
			param.campaignId = 0;
		};
		requestCP_Campaign.OnReceived = new Action<GameWebAPI.RespDataCP_Campaign>(this.OnRecievedCampaign);
		GameWebAPI.RequestCP_Campaign addRequest2 = requestCP_Campaign;
		requestList.AddRequest(addRequest2);
		RequestList requestList2 = requestList;
		GameWebAPI.RequestIN_InfoList requestIN_InfoList = new GameWebAPI.RequestIN_InfoList();
		requestIN_InfoList.SetSendData = delegate(GameWebAPI.SendDataIN_InfoList requestParam)
		{
			int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
			requestParam.countryCode = countryCode;
		};
		requestIN_InfoList.OnReceived = new Action<GameWebAPI.RespDataIN_InfoList>(this.OnRecievedInformationList);
		requestList2.AddRequest(requestIN_InfoList);
		RequestList requestList3 = requestList;
		GameWebAPI.RequestMA_BannerMaster requestMA_BannerMaster = new GameWebAPI.RequestMA_BannerMaster();
		requestMA_BannerMaster.SetSendData = delegate(GameWebAPI.RequestMA_BannerM requestParam)
		{
			int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
			requestParam.countryCode = countryCode;
		};
		requestMA_BannerMaster.OnReceived = new Action<GameWebAPI.RespDataMA_BannerM>(this.OnRecievedBannerMaster);
		requestList3.AddRequest(requestMA_BannerMaster);
		GameWebAPI.RequestUS_UserProfile requestUS_UserProfile = new GameWebAPI.RequestUS_UserProfile();
		requestUS_UserProfile.SetSendData = delegate(GameWebAPI.PRF_Req_ProfileData param)
		{
			param.targetUserId = DataMng.Instance().RespDataCM_Login.playerInfo.UserId;
		};
		requestUS_UserProfile.OnReceived = new Action<GameWebAPI.RespDataPRF_Profile>(this.OnRecievedProfileData);
		GameWebAPI.RequestUS_UserProfile addRequest3 = requestUS_UserProfile;
		requestList.AddRequest(addRequest3);
		requestList.AddRequest(new GameWebAPI.RequestCL_ColosseumReleaseCriteria
		{
			OnReceived = new Action<GameWebAPI.RespDataCL_ColosseumReleaseCriteria>(this.OnRecievedColosseumReleaseCriteria)
		});
		requestList.AddRequest(new GameWebAPI.ColosseumInfoLogic
		{
			OnReceived = new Action<GameWebAPI.RespData_ColosseumInfoLogic>(this.OnRecievedColosseumInfo)
		});
		GameWebAPI.RequestCL_GetColosseumReward requestCL_GetColosseumReward = new GameWebAPI.RequestCL_GetColosseumReward();
		requestCL_GetColosseumReward.SetSendData = delegate(GameWebAPI.SendDataCL_GetColosseumReward param)
		{
			param.act = "2";
		};
		requestCL_GetColosseumReward.OnReceived = new Action<GameWebAPI.RespDataCL_GetColosseumReward>(this.OnRecievedColosseumReward);
		GameWebAPI.RequestCL_GetColosseumReward addRequest4 = requestCL_GetColosseumReward;
		requestList.AddRequest(addRequest4);
		APIRequestTask apirequestTask = new APIRequestTask(requestList, true);
		apirequestTask.Add(Singleton<UserDataMng>.Instance.RequestUserMonsterFriendshipTime(true)).Delegate(ClassSingleton<BattleDataStore>.Instance.RequestWorldStartDataLogic(true));
		return apirequestTask;
	}

	public APIRequestTask RequestFirstTutorialData()
	{
		RequestList requestList = new RequestList();
		requestList.AddRequest(new GameWebAPI.RequestUS_UserStatus
		{
			OnReceived = new Action<GameWebAPI.RespDataUS_GetPlayerInfo>(this.OnRecievedUserStatus)
		});
		requestList.AddRequest(new GameWebAPI.RequestMonsterList
		{
			OnReceived = new Action<GameWebAPI.RespDataUS_GetMonsterList>(this.OnRecievedUserMonster)
		});
		requestList.AddRequest(new GameWebAPI.RequestMN_DeckList
		{
			OnReceived = new Action<GameWebAPI.RespDataMN_GetDeckList>(this.OnRecievedDeckList)
		});
		requestList.AddRequest(new GameWebAPI.ReqDataCS_ChipListLogic
		{
			OnReceived = new Action<GameWebAPI.RespDataCS_ChipListLogic>(this.OnRecievedUserChipList)
		});
		requestList.AddRequest(new GameWebAPI.MonsterSlotInfoListLogic
		{
			OnReceived = new Action<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic>(this.OnRecievedMonsterSlotInfo)
		});
		return new APIRequestTask(requestList, true);
	}

	public APIRequestTask RequestTutorialHomeData()
	{
		RequestList requestList = new RequestList();
		requestList.AddRequest(new GameWebAPI.RequestUS_UserStatus
		{
			OnReceived = new Action<GameWebAPI.RespDataUS_GetPlayerInfo>(this.OnRecievedUserStatus)
		});
		requestList.AddRequest(new GameWebAPI.RequestMonsterList
		{
			OnReceived = new Action<GameWebAPI.RespDataUS_GetMonsterList>(this.OnRecievedUserMonster)
		});
		requestList.AddRequest(new GameWebAPI.RequestMN_DeckList
		{
			OnReceived = new Action<GameWebAPI.RespDataMN_GetDeckList>(this.OnRecievedDeckList)
		});
		requestList.AddRequest(new GameWebAPI.ReqDataCS_ChipListLogic
		{
			OnReceived = new Action<GameWebAPI.RespDataCS_ChipListLogic>(this.OnRecievedUserChipList)
		});
		requestList.AddRequest(new GameWebAPI.MonsterSlotInfoListLogic
		{
			OnReceived = new Action<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic>(this.OnRecievedMonsterSlotInfo)
		});
		requestList.AddRequest(new GameWebAPI.UserSoulInfoList
		{
			OnReceived = new Action<GameWebAPI.RespDataUS_GetSoulInfo>(this.OnRecievedUserSoulData)
		});
		GameWebAPI.RequestFA_UserFacilityList requestFA_UserFacilityList = new GameWebAPI.RequestFA_UserFacilityList();
		requestFA_UserFacilityList.SetSendData = delegate(GameWebAPI.FA_Req_RequestFA_UserFacilityList param)
		{
			param.userId = DataMng.Instance().RespDataCM_Login.playerInfo.UserId;
		};
		requestFA_UserFacilityList.OnReceived = new Action<GameWebAPI.RespDataFA_GetFacilityList>(this.OnRecievedUserFacility);
		GameWebAPI.RequestFA_UserFacilityList addRequest = requestFA_UserFacilityList;
		requestList.AddRequest(addRequest);
		return new APIRequestTask(requestList, true);
	}

	private void OnRecievedUserTitleList(GameWebAPI.RespDataTL_GetUserTitleList responseData)
	{
		if (responseData.userTitleList != null)
		{
			TitleDataMng.userTitleList = new List<GameWebAPI.RespDataTL_GetUserTitleList.UserTitleList>(responseData.userTitleList);
		}
		else
		{
			TitleDataMng.userTitleList = new List<GameWebAPI.RespDataTL_GetUserTitleList.UserTitleList>();
		}
	}

	private void OnRecievedDeckList(GameWebAPI.RespDataMN_GetDeckList responseData)
	{
		DataMng.Instance().RespDataMN_DeckList = responseData;
	}

	private void OnRecievedUserChipList(GameWebAPI.RespDataCS_ChipListLogic responseData)
	{
		ChipDataMng.userChipData = responseData;
	}

	private void OnRecievedItemList(GameWebAPI.RespDataUS_ItemListLogic responseData)
	{
		if (responseData.userItemData != null)
		{
			Singleton<UserDataMng>.Instance.userItemList = new List<GameWebAPI.RespDataUS_ItemListLogic.UserItemData>(responseData.userItemData);
		}
		else
		{
			Singleton<UserDataMng>.Instance.userItemList = new List<GameWebAPI.RespDataUS_ItemListLogic.UserItemData>();
		}
	}

	private void OnRecievedMonsterSlotInfo(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic responseData)
	{
		if (responseData != null)
		{
			ChipDataMng.GetUserChipSlotData().SetMonsterSlotList(responseData.slotInfo);
		}
		ClassSingleton<MonsterUserDataMng>.Instance.RefreshMonsterSlot();
	}

	private void OnRecievedMyPage(GameWebAPI.RespDataMP_MyPage responseData)
	{
		DataMng.Instance().RespDataMP_MyPage = responseData;
		if (0 < responseData.userNewsCountList.isPopUpInformaiton)
		{
			DataMng.Instance().IsPopUpInformaiton = true;
		}
	}

	private void OnRecievedLoginBonus(GameWebAPI.RespDataCM_LoginBonus responseData)
	{
		DataMng.Instance().RespDataCM_LoginBonus = responseData;
	}

	private void OnRecievedUserStatus(GameWebAPI.RespDataUS_GetPlayerInfo responseData)
	{
		DataMng.Instance().RespDataUS_PlayerInfo = responseData;
		Singleton<UserDataMng>.Instance.playerStaminaBaseTime = ServerDateTime.Now;
	}

	private void OnRecievedProfileData(GameWebAPI.RespDataPRF_Profile responseData)
	{
		DataMng.Instance().RespDataPRF_Profile = responseData;
	}

	private void OnRecievedUserMonster(GameWebAPI.RespDataUS_GetMonsterList responseData)
	{
		ClassSingleton<MonsterUserDataMng>.Instance.SetUserMonsterData(responseData.userMonsterList);
	}

	private void OnRecievedUserSoulData(GameWebAPI.RespDataUS_GetSoulInfo responseData)
	{
		DataMng.Instance().RespDataUS_SoulInfo = responseData;
	}

	private void OnRecievedUserFacility(GameWebAPI.RespDataFA_GetFacilityList responseData)
	{
		Singleton<UserDataMng>.Instance.userFacilityList = responseData.userFacilityList.Where((UserFacility x) => x.facilityId != 6).ToList<UserFacility>();
		Singleton<UserDataMng>.Instance.monsterIdsInFarm = responseData.monsterIdsInFarm;
		Singleton<UserDataMng>.Instance.SetLastHarvestTime(responseData.lastHarvestTime);
	}

	private void OnRecievedCampaign(GameWebAPI.RespDataCP_Campaign responseData)
	{
		DataMng.Instance().RespDataCP_Campaign = responseData;
		DataMng.Instance().OnCampaignUpdate(DataMng.Instance().RespDataCP_Campaign, DataMng.Instance().CampaignForceHide);
	}

	private void OnRecievedInformationList(GameWebAPI.RespDataIN_InfoList responseData)
	{
		DataMng.Instance().RespDataIN_InfoList = responseData;
	}

	private void OnRecievedBannerMaster(GameWebAPI.RespDataMA_BannerM responseData)
	{
		DataMng.Instance().RespData_BannerMaster = responseData;
	}

	private void OnRecievedColosseumReleaseCriteria(GameWebAPI.RespDataCL_ColosseumReleaseCriteria responseData)
	{
		DataMng.Instance().IsReleaseColosseum = (responseData.resultCode > 0);
	}

	private void OnRecievedColosseumInfo(GameWebAPI.RespData_ColosseumInfoLogic responseData)
	{
		DataMng.Instance().RespData_ColosseumInfo = responseData;
	}

	private void OnRecievedColosseumReward(GameWebAPI.RespDataCL_GetColosseumReward responseData)
	{
		DataMng.Instance().RespData_ColosseumReward = responseData;
	}

	public IEnumerator SendBattleResult(Action remainingSceneChangeAction)
	{
		if (!Loading.IsShow())
		{
			Loading.Display(Loading.LoadingType.LARGE, false);
		}
		GameWebAPI.WorldResultLogic worldResultLogic = new GameWebAPI.WorldResultLogic();
		worldResultLogic.SetSendData = delegate(GameWebAPI.WD_Req_DngResult param)
		{
			param.startId = DataMng.Instance().WD_ReqDngResult.startId;
			param.dungeonId = DataMng.Instance().WD_ReqDngResult.dungeonId;
			param.clear = DataMng.Instance().WD_ReqDngResult.clear;
			param.aliveInfo = DataMng.Instance().WD_ReqDngResult.aliveInfo;
			param.clearRound = DataMng.Instance().WD_ReqDngResult.clearRound;
			param.enemyAliveInfo = DataMng.Instance().WD_ReqDngResult.enemyAliveInfo;
		};
		worldResultLogic.OnReceived = delegate(GameWebAPI.RespDataWD_DungeonResult response)
		{
			ClassSingleton<QuestData>.Instance.RespDataWD_DungeonResult = response;
			ClassSingleton<QuestData>.Instance.ClearDNGDataCache();
		};
		GameWebAPI.WorldResultLogic request = worldResultLogic;
		return request.Run(remainingSceneChangeAction, null, null);
	}

	public IEnumerator SendBattleResultForMulti(Action remainingSceneChangeAction, int startId)
	{
		if (!Loading.IsShow())
		{
			Loading.Display(Loading.LoadingType.LARGE, false);
		}
		GameWebAPI.WorldMultiResultInfoLogic worldMultiResultInfoLogic = new GameWebAPI.WorldMultiResultInfoLogic();
		worldMultiResultInfoLogic.SetSendData = delegate(GameWebAPI.ReqData_WorldMultiResultInfoLogic param)
		{
			param.startId = startId;
			param.clearRound = DataMng.Instance().WD_ReqDngResult.clearRound;
		};
		worldMultiResultInfoLogic.OnReceived = delegate(GameWebAPI.RespData_WorldMultiResultInfoLogic response)
		{
			ClassSingleton<QuestData>.Instance.ClearDNGDataCache();
			ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic = response;
		};
		GameWebAPI.WorldMultiResultInfoLogic request = worldMultiResultInfoLogic;
		return request.Run(remainingSceneChangeAction, null, null);
	}
}
