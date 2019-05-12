using FarmData;
using Monster;
using SmartBeat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using WebAPIRequest;

public class GameWebAPI : WebAPI
{
	private static GameWebAPI instance;

	public static GameWebAPI Instance()
	{
		return GameWebAPI.instance;
	}

	protected void Awake()
	{
		GameWebAPI.instance = this;
		this.InitDisableVCList();
	}

	protected void OnDestroy()
	{
		GameWebAPI.instance = null;
	}

	private void InitDisableVCList()
	{
		this.disableVC_StaticList = new string[]
		{
			"000005",
			"000004",
			"000103",
			"000401",
			"000501",
			"000601",
			"000602",
			"020010",
			"020015",
			"020016",
			"060001",
			"060101",
			"060102",
			"060103",
			"060110",
			"080109",
			"090003",
			"090008",
			"090013",
			"020301",
			"120110",
			"120202",
			"150024",
			"120203"
		};
		this.disableVC_DynamicList = new List<string>();
	}

	public void AddDisableVersionCheckApiId(string apiId)
	{
		global::Debug.Assert(!this.disableVC_DynamicList.Contains(apiId), "disableVC リストに登録済みのAPIです (" + apiId + ")");
		this.disableVC_DynamicList.Add(apiId);
	}

	public void RemoveDisableVersionCheckApiId(string apiId)
	{
		global::Debug.Assert(this.disableVC_DynamicList.Contains(apiId), "disableVC リストに未登録のAPIです (" + apiId + ")");
		this.disableVC_DynamicList.Remove(apiId);
	}

	public IEnumerator SendActivityCheatLog(string errorType, string errorMessage, string errorApiId = null)
	{
		GameWebAPI.ActivityCheatLog request = new GameWebAPI.ActivityCheatLog
		{
			SetSendData = delegate(GameWebAPI.RequestCM_ActivityCheatLog param)
			{
				string activityId = errorApiId;
				if (string.IsNullOrEmpty(errorApiId))
				{
					activityId = "0";
				}
				param.activityId = activityId;
				param.errorType = errorType;
				param.message = errorMessage;
			}
		};
		APIRequestTask apirequestTask = new APIRequestTask(request, false);
		apirequestTask.SetAfterBehavior(TaskBase.AfterAlertClosed.RETURN);
		return apirequestTask.Run(null, null, (Exception noop) => null);
	}

	public sealed class OAuthLogin : WebAPI.ResponseData
	{
		public GameWebAPI.OAuthLogin.MaintenanceInfo maintenance;

		public sealed class MaintenanceInfo
		{
			public string type;

			public string userCode;
		}
	}

	public sealed class RequestCM_Login : RequestTypeBase<GameWebAPI.RequestCM_LoginRequest, GameWebAPI.RespDataCM_Login>
	{
		public RequestCM_Login()
		{
			this.apiId = "000001";
		}

		protected override void SetHeader(string json)
		{
			if (DataMng.Instance().RespDataCM_Login != null)
			{
				DataMng.Instance().RespDataCM_Login.SetParam(json);
				SmartBeat.setUserId(DataMng.Instance().RespDataCM_Login.playerInfo.userId);
			}
		}
	}

	public sealed class RequestCM_LoginRequest : WebAPI.SendBaseData
	{
		public string osType;

		public string osVersion;

		public string appVersion;

		public string modelName;
	}

	public sealed class RespDataCM_Login : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataCM_Login.PlayerInfo playerInfo;

		public GameWebAPI.RespDataCM_Login.TutorialStatus tutorialStatus;

		public Dictionary<string, string> optionList;

		public GameWebAPI.RespDataCM_Login.AdminUser adminUser;

		public GameWebAPI.PenaltyUserInfo penaltyUserInfo;

		public int isWarning;

		public string[] warningData;

		public string nowDateTime;

		public int isPolicyConfirm;

		public Dictionary<string, int> GetOptionList_ProvisionalFunction()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			foreach (KeyValuePair<string, string> keyValuePair in this.optionList)
			{
				dictionary.Add(keyValuePair.Key, int.Parse(keyValuePair.Value));
			}
			return dictionary;
		}

		public override void SetParam(string json)
		{
			this.playerInfo.osType = WebAPIJsonParse.GetString(json, "osType");
			this.playerInfo.compassCarrierId = WebAPIJsonParse.GetInt(json, "compassCarrierId");
		}

		public bool ConfirmedPolicy()
		{
			return 1 == this.isPolicyConfirm;
		}

		public sealed class TutorialStatus
		{
			public string statusId;

			public string endFlg;
		}

		public sealed class AdminUser
		{
			public string adminUserId;

			public string ldapId;

			public string adminUserLv;

			public string appBranchName;

			public string gameIds;

			public string createdTime;

			public string createUserId;

			public string createActivityId;

			public string updateTime;

			public string updateUserId;

			public string updateActivityId;

			public string deleteFlg;
		}

		public sealed class PlayerInfo
		{
			public string userId;

			public string userCode;

			public int compassCarrierId;

			public string osType;

			public int UserId
			{
				get
				{
					return int.Parse(this.userId);
				}
			}
		}
	}

	public sealed class PenaltyUserInfo
	{
		public string userId;

		public string penaltyLevel;

		public GameWebAPI.PenaltyUserInfo.Penalty penalty;

		public sealed class Penalty
		{
			public string penaltyId;

			public string penaltyLevel;

			public string loginFlg;

			public string title;

			public string message;
		}
	}

	public sealed class RequestMP_MyPage : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMP_MyPage>
	{
		public RequestMP_MyPage()
		{
			this.apiId = "000002";
		}
	}

	public sealed class RespDataMP_MyPage : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMP_MyPage.UserNewsCountList userNewsCountList;

		public GameWebAPI.PenaltyUserInfo penaltyUserInfo;

		public sealed class UserNewsCountList
		{
			public string prize;

			public int friendApplication;

			public int profileComment;

			public int collectionReceiveCount;

			public int info;

			public int isPopUpInformaiton;

			public int newFriend;

			public int addCost;

			public int endlessDungeon;

			public int missionRewardCount;

			public int missionNewCount;

			public int beginnerMissionRewardCount;

			public int beginnerMissionNewCount;

			public int facilityNewCount;

			public int decorationNewCount;
		}
	}

	public sealed class RequestCM_MasterDataVersion : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataCM_MDVersion>
	{
		public RequestCM_MasterDataVersion()
		{
			this.apiId = "000004";
		}
	}

	[Serializable]
	public sealed class RespDataCM_MDVersion : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataCM_MDVersion.DataVersionList[] dataVersionList;

		public GameWebAPI.RespDataCM_MDVersion.VersionManagerMaster versionManagerMaster;

		[Serializable]
		public sealed class DataVersionList
		{
			public string tableName;

			public string version;

			public string cacheVersion;
		}

		[Serializable]
		public sealed class VersionManagerMaster
		{
			public string versionManagerId;

			public string versionManagerType;

			public string osType;

			public string version;
		}
	}

	public sealed class RequestCM_ABVersion : RequestTypeBase<GameWebAPI.CM_Req_ABInfo, GameWebAPI.RespDataCM_ABVersion>
	{
		public RequestCM_ABVersion()
		{
			this.apiId = "000005";
		}
	}

	public sealed class CM_Req_ABInfo : WebAPI.SendBaseData
	{
		public int downloadType;
	}

	public sealed class RespDataCM_ABVersion : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataCM_ABVersion.AssetBundleVersionList[] assetBundleVersionList;

		public GameWebAPI.RespDataCM_ABVersion.VersionManagerAsset versionManagerAsset;

		public sealed class AssetBundleVersionList
		{
			public string assetbundlePath;

			public string assetbundleGroup;

			public string osType;

			public string version;

			public string accessPath;
		}

		public sealed class VersionManagerAsset
		{
			public string versionManagerId;

			public string versionManagerType;

			public string osType;

			public string version;
		}
	}

	public sealed class RequestCM_LoginBonus : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataCM_LoginBonus>
	{
		public RequestCM_LoginBonus()
		{
			this.apiId = "000007";
		}
	}

	public sealed class RespDataCM_LoginBonus : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataCM_LoginBonus.LoginBonusSet loginBonus;

		public sealed class LoginReward
		{
			public string assetCategoryId;

			public string assetValue;

			public string assetNum;
		}

		public sealed class LoginBonus
		{
			public string loginBonusId;

			public int loginCount;

			public string backgroundImg;

			public GameWebAPI.RespDataCM_LoginBonus.LoginReward[] rewardList;

			public GameWebAPI.RespDataCM_LoginBonus.LoginReward[] nextRewardList;
		}

		public sealed class LoginBonusSet
		{
			public GameWebAPI.RespDataCM_LoginBonus.LoginBonus[] normal;

			public GameWebAPI.RespDataCM_LoginBonus.LoginBonus[] campaign;

			public GameWebAPI.RespDataCM_LoginBonus.LoginBonus[] special;

			public GameWebAPI.RespDataCM_LoginBonus.LoginBonus[] week;
		}
	}

	public sealed class Request_CM_TakeoverIssue : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataCM_TakeoverIssue>
	{
		public Request_CM_TakeoverIssue()
		{
			this.apiId = "000102";
		}
	}

	public sealed class RespDataCM_TakeoverIssue : WebAPI.ResponseData
	{
		public int appliId;

		public string userCode;

		public string transferCode;

		public string expireTime;
	}

	public sealed class Request_CM_TakeoverInput : RequestTypeBase<GameWebAPI.CM_Req_TakeoverInput, GameWebAPI.RespDataCM_TakeoverInput>
	{
		public Request_CM_TakeoverInput()
		{
			this.apiId = "000103";
		}
	}

	public sealed class CM_Req_TakeoverInput : WebAPI.SendBaseData
	{
		public string transferUserCode;

		public string transferCode;
	}

	public sealed class RespDataCM_TakeoverInput : WebAPI.ResponseData
	{
		public int transferStatus;
	}

	public sealed class Request_CM_UpdateDeviceToken : RequestTypeBase<GameWebAPI.CM_Req_UpdateDeviceToken, GameWebAPI.RespDataCM_UpdateDeviceToken>
	{
		public Request_CM_UpdateDeviceToken()
		{
			this.apiId = "000201";
		}
	}

	public sealed class CM_Req_UpdateDeviceToken : WebAPI.SendBaseData
	{
		public int osType;

		public string deviceToken;
	}

	public sealed class RespDataCM_UpdateDeviceToken : WebAPI.ResponseData
	{
		public int tokenStatus;
	}

	public sealed class ActivityCheatLog : RequestTypeBase<GameWebAPI.RequestCM_ActivityCheatLog, GameWebAPI.ResponseCM_ActivityCheatLog>
	{
		public ActivityCheatLog()
		{
			this.apiId = "000401";
		}
	}

	public sealed class RequestCM_ActivityCheatLog : WebAPI.SendBaseData
	{
		public string activityId;

		public string errorType;

		public string message;
	}

	public sealed class ResponseCM_ActivityCheatLog : WebAPI.ResponseData
	{
		public int result;
	}

	public sealed class Request_CM_GetSystemDateTime : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataCM_GetSystemDateTime>
	{
		public Request_CM_GetSystemDateTime()
		{
			this.apiId = "000501";
		}
	}

	public sealed class RespDataCM_GetSystemDateTime : WebAPI.ResponseData
	{
		public string nowDateTime;
	}

	public sealed class RequestCM_InquiryCodeRequest : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.InquiryCodeRequest>
	{
		public RequestCM_InquiryCodeRequest()
		{
			this.apiId = "000601";
		}
	}

	public sealed class InquiryCodeRequest : WebAPI.ResponseData
	{
		public string result;

		public string inquiryCode;
	}

	public sealed class RequestCM_TakeOverUseInquiryCode : RequestTypeBase<GameWebAPI.ReqDataCM_TakeOverUseInquiryCode, GameWebAPI.RespDataCM_TakeOverUseInquiryCode>
	{
		public RequestCM_TakeOverUseInquiryCode()
		{
			this.apiId = "000602";
		}
	}

	public sealed class ReqDataCM_TakeOverUseInquiryCode : WebAPI.SendBaseData
	{
		public string userCode;

		public string inquiryCode;
	}

	public sealed class RespDataCM_TakeOverUseInquiryCode : WebAPI.ResponseData
	{
		public int transferStatus;
	}

	public sealed class UserProtectMonsterLogic : RequestTypeBase<GameWebAPI.ReqDataUS_UserProtectMonsterLogic, GameWebAPI.RespDataMS_UserProtectMonsterLogic>
	{
		public UserProtectMonsterLogic()
		{
			this.apiId = "010001";
		}
	}

	public sealed class ReqDataUS_UserProtectMonsterLogic : WebAPI.SendBaseData
	{
		public string userMonsterId;

		public string setFlg;
	}

	public sealed class RespDataMS_UserProtectMonsterLogic : WebAPI.ResponseData
	{
		public int result;
	}

	public sealed class RequestMN_MonsterSale : RequestTypeBase<GameWebAPI.MN_Req_Sale, GameWebAPI.RespDataMN_SaleExec>
	{
		public RequestMN_MonsterSale()
		{
			this.apiId = "010002";
		}
	}

	public sealed class MN_Req_Sale : WebAPI.SendBaseData
	{
		public GameWebAPI.MN_Req_Sale.SaleMonsterDataList[] saleMonsterDataList;

		public sealed class SaleMonsterDataList
		{
			public string userMonsterId;
		}
	}

	public sealed class RespDataMN_SaleExec : WebAPI.ResponseData
	{
		public int result;

		public int itemRecovered;
	}

	public sealed class RequestFA_MN_PicturebookExec : RequestTypeBase<GameWebAPI.MN_Req_Picturebook, GameWebAPI.RespDataMN_Picturebook>
	{
		public RequestFA_MN_PicturebookExec()
		{
			this.apiId = "010101";
		}
	}

	public sealed class MN_Req_Picturebook : WebAPI.SendBaseData
	{
		public string targetUserId;
	}

	public sealed class RespDataMN_Picturebook : WebAPI.ResponseData
	{
		public int possessionNum;

		public string totalNum;

		public GameWebAPI.RespDataMN_Picturebook.UserCollectionData[] userCollectionList;

		public sealed class UserCollectionData
		{
			public string monsterCollectionId;

			public string collectionStatus;

			public const string HAVE_STATUS = "1";

			private const string NEW_GET_STATUS = "2";

			public bool IsHave()
			{
				return this.collectionStatus == "1" || this.collectionStatus == "2";
			}

			public void SetHaveStatus()
			{
				this.collectionStatus = "2";
			}
		}
	}

	public sealed class RequestMN_MonsterFusion : RequestTypeBase<GameWebAPI.MN_Req_Fusion, GameWebAPI.RespDataMN_FusionExec>
	{
		public RequestMN_MonsterFusion()
		{
			this.apiId = "010201";
		}
	}

	public sealed class MN_Req_Fusion : WebAPI.SendBaseData
	{
		public string baseMonster;

		public string[] materialMonster;
	}

	public sealed class RespDataMN_FusionExec : WebAPI.ResponseData
	{
		public int fusionType;

		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;
	}

	public sealed class RequestMN_MonsterMeal : RequestTypeBase<GameWebAPI.MN_Req_Meal, GameWebAPI.RespDataMN_MealExec>
	{
		public RequestMN_MonsterMeal()
		{
			this.apiId = "010202";
		}
	}

	public sealed class MN_Req_Meal : WebAPI.SendBaseData
	{
		public string mealMonster;

		public int meatNum;
	}

	public sealed class RespDataMN_MealExec : WebAPI.ResponseData
	{
		public int mealType;

		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;
	}

	public sealed class RequestMN_MonsterHQMeal : RequestTypeBase<GameWebAPI.MN_Req_HQMeal, GameWebAPI.RespDataMN_HQMealExec>
	{
		public RequestMN_MonsterHQMeal()
		{
			this.apiId = "010203";
		}
	}

	public sealed class MN_Req_HQMeal : WebAPI.SendBaseData
	{
		public string baseMonster;

		public int fusionType;

		public enum FusionType
		{
			FREE = 1,
			STONE
		}
	}

	public sealed class RespDataMN_HQMealExec : WebAPI.ResponseData
	{
		public int fusionType;

		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;
	}

	public sealed class RequestMN_MonsterEvolution : RequestTypeBase<GameWebAPI.MN_Req_Evolution, GameWebAPI.RespDataMN_EvolutionExec>
	{
		public RequestMN_MonsterEvolution()
		{
			this.apiId = "010301";
		}
	}

	public sealed class MN_Req_Evolution : WebAPI.SendBaseData
	{
		public string userMonsterId;

		public int monsterId;
	}

	public sealed class RespDataMN_EvolutionExec : WebAPI.ResponseData
	{
		public int result;

		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;

		public int reviewStatus;

		public bool IsFirstEvolution()
		{
			return this.reviewStatus == 1;
		}

		public bool IsFirstUltimaEvolution()
		{
			return this.reviewStatus == 7;
		}
	}

	public sealed class RequestMN_MonsterEvolutionInGarden : RequestTypeBase<GameWebAPI.MN_Req_Grow, GameWebAPI.RespDataMN_GrowExec>
	{
		public RequestMN_MonsterEvolutionInGarden()
		{
			this.apiId = "010302";
		}
	}

	public sealed class MN_Req_Grow : WebAPI.SendBaseData
	{
		public string userMonsterId;

		public int shorteningFlg;

		public int stone;
	}

	public sealed class RespDataMN_GrowExec : WebAPI.ResponseData
	{
		public int result;

		public int useStone;

		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;
	}

	public sealed class RequestMN_MonsterHatching : RequestTypeBase<GameWebAPI.MN_Req_Born, GameWebAPI.RespDataMN_BornExec>
	{
		public RequestMN_MonsterHatching()
		{
			this.apiId = "010304";
		}
	}

	public sealed class MN_Req_Born : WebAPI.SendBaseData
	{
		public string userMonsterId;
	}

	public sealed class RespDataMN_BornExec : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;
	}

	public sealed class RequestMN_DeckList : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMN_GetDeckList>
	{
		public RequestMN_DeckList()
		{
			this.apiId = "010401";
		}
	}

	public sealed class RespDataMN_GetDeckList : WebAPI.ResponseData
	{
		public int deckCnt;

		public GameWebAPI.RespDataMN_GetDeckList.DeckList[] deckList;

		public string selectDeckNum;

		public string favoriteDeckNum;

		public sealed class MonsterList
		{
			public string userDeckMonsterId;

			public string userId;

			public string deckNum;

			public string position;

			public string userMonsterId;
		}

		public sealed class DeckList
		{
			public string deckNum;

			public GameWebAPI.RespDataMN_GetDeckList.MonsterList[] monsterList;
		}
	}

	public sealed class RequestMN_DeckEdit : RequestTypeBase<GameWebAPI.MN_Req_EditDeckList, GameWebAPI.RespDataMN_EditDeckList>
	{
		public RequestMN_DeckEdit()
		{
			this.apiId = "010402";
		}
	}

	public sealed class MN_Req_EditDeckList : WebAPI.SendBaseData
	{
		public int[][] deckData;

		public int selectDeckNum;

		public int favoriteDeckNum;
	}

	public sealed class RespDataMN_EditDeckList : WebAPI.ResponseData
	{
		public int result;
	}

	public sealed class RequestMN_MonsterCombination : RequestTypeBase<GameWebAPI.MN_Req_Labo, GameWebAPI.RespDataMN_LaboExec>
	{
		public RequestMN_MonsterCombination()
		{
			this.apiId = "010501";
		}
	}

	public sealed class MN_Req_Labo : WebAPI.SendBaseData
	{
		public string baseUserMonsterId;

		public string materialUserMonsterId;
	}

	public sealed class RespDataMN_LaboExec : WebAPI.ResponseData
	{
		public string newUserMonsterId;

		public string eggMonsterId;

		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;
	}

	public sealed class RequestMN_MedalInherit : RequestTypeBase<GameWebAPI.MN_Req_MedalInherit, GameWebAPI.RespDataMN_MedalInherit>
	{
		public RequestMN_MedalInherit()
		{
			this.apiId = "010502";
		}
	}

	public sealed class MN_Req_MedalInherit : WebAPI.SendBaseData
	{
		public string baseUserMonsterId;

		public string materialUserMonsterId;
	}

	public sealed class RespDataMN_MedalInherit : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;
	}

	public sealed class RequestMN_MonsterInheritance : RequestTypeBase<GameWebAPI.MN_Req_Success, GameWebAPI.RespDataMN_SuccessExec>
	{
		public RequestMN_MonsterInheritance()
		{
			this.apiId = "010601";
		}
	}

	public sealed class MN_Req_Success : WebAPI.SendBaseData
	{
		public string baseUserMonsterId;

		public string materialUserMonsterId;

		public int baseCommonSkillNumber;

		public int materialCommonSkillNumber;
	}

	public sealed class RespDataMN_SuccessExec : WebAPI.ResponseData
	{
		public int result;

		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;
	}

	public sealed class RequestMN_UserMonsterFriendship : RequestTypeBase<GameWebAPI.MN_Req_FriendshipStatus, GameWebAPI.RespDataMN_Friendship>
	{
		public RequestMN_UserMonsterFriendship()
		{
			this.apiId = "010701";
		}
	}

	public sealed class MN_Req_FriendshipStatus : WebAPI.SendBaseData
	{
		public string userMonsterId;
	}

	public sealed class RespDataMN_Friendship : WebAPI.ResponseData
	{
		public int upFriendship;

		public int nextTimeSec;

		public GameWebAPI.RespDataMN_Friendship.UpStatus upStatus;

		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;

		public sealed class UpStatus
		{
			public int hp;

			public int attack;

			public int defense;

			public int spAttack;

			public int spDefense;

			public int speed;
		}
	}

	public sealed class RequestMN_FriendTimeCheck : RequestTypeBase<GameWebAPI.MN_Req_FriendTimeCheck, GameWebAPI.RespDataMN_FriendTimeCheck>
	{
		public RequestMN_FriendTimeCheck()
		{
			this.apiId = "010702";
		}
	}

	public sealed class MN_Req_FriendTimeCheck : WebAPI.SendBaseData
	{
		public string[] userMonsterIds;
	}

	public sealed class RespDataMN_FriendTimeCheck : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMN_FriendTimeCheck.FriendshipTime[] friendshipTime;

		public sealed class FriendshipTime
		{
			public string userMonsterId;

			public int nextTimeSec;
		}
	}

	public sealed class RequestMN_MonsterTrance : RequestTypeBase<GameWebAPI.MN_Req_Trunce, GameWebAPI.RespDataMN_TrunceExec>
	{
		public RequestMN_MonsterTrance()
		{
			this.apiId = "010801";
		}
	}

	public sealed class MN_Req_Trunce : WebAPI.SendBaseData
	{
		public int baseUserMonsterId;

		public int materialUserMonsterId;

		public int type;
	}

	public sealed class RespDataMN_TrunceExec : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;
	}

	public sealed class RequestMN_VersionUP : RequestTypeBase<GameWebAPI.MN_Req_VersionUP, GameWebAPI.RespDataMN_VersionUP>
	{
		public RequestMN_VersionUP()
		{
			this.apiId = "010802";
		}
	}

	public sealed class MN_Req_VersionUP : WebAPI.SendBaseData
	{
		public int baseUserMonsterId;

		public int target;

		public int[] material;
	}

	public sealed class RespDataMN_VersionUP : WebAPI.ResponseData
	{
		public int result;

		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;
	}

	public sealed class RequestMN_MoveDigiGarden : RequestTypeBase<GameWebAPI.MN_Req_MoveDigiGarden, GameWebAPI.RespDataMN_MoveDigiGarden>
	{
		public RequestMN_MoveDigiGarden()
		{
			this.apiId = "010901";
		}
	}

	public sealed class MN_Req_MoveDigiGarden : WebAPI.SendBaseData
	{
		public string userMonsterId;
	}

	public sealed class RespDataMN_MoveDigiGarden : WebAPI.ResponseData
	{
		public int result;

		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;
	}

	public sealed class RequestUS_UserProfile : RequestTypeBase<GameWebAPI.PRF_Req_ProfileData, GameWebAPI.RespDataPRF_Profile>
	{
		public RequestUS_UserProfile()
		{
			this.apiId = "020002";
		}
	}

	public sealed class PRF_Req_ProfileData : WebAPI.SendBaseData
	{
		public int targetUserId;
	}

	public sealed class RespDataPRF_Profile : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataPRF_Profile.UserDataProf userData;

		public GameWebAPI.RespDataPRF_Profile.MonsterDataProf monsterData;

		public GameWebAPI.RespDataPRF_Profile.CollectionProf collection;

		public int friendStatus;

		public GameWebAPI.RespDataPRF_Profile.PlayHistory playHistory;

		public sealed class UserDataProf
		{
			public string userId;

			public string userCode;

			public string nickname;

			public string description;

			public string lastLoginTime;

			public string loginTime;

			public int loginTimeSort;

			public string birthday;

			public string titleId;
		}

		public sealed class MonsterDataProf
		{
			public string userMonsterId;

			public string userId;

			public string monsterId;

			public string level;

			public string ex;

			public string levelEx;

			public string nextLevelEx;

			public string leaderSkillId;

			public string uniqueSkillId;

			public string defaultSkillGroupSubId;

			public string commonSkillId;

			public string extraCommonSkillId;

			public string eggFlg;

			public string growEndDate;

			public string monsterEvolutionRouteId;

			public string hp;

			public string attack;

			public string defense;

			public string spAttack;

			public string spDefense;

			public string speed;

			public string luck;

			public string hpAbilityFlg;

			public string hpAbility;

			public string attackAbilityFlg;

			public string attackAbility;

			public string defenseAbilityFlg;

			public string defenseAbility;

			public string spAttackAbilityFlg;

			public string spAttackAbility;

			public string spDefenseAbilityFlg;

			public string spDefenseAbility;

			public string speedAbilityFlg;

			public string speedAbility;

			public string friendship;

			public string statusFlgs;

			public string tranceResistance;

			public string tranceStatusAilment;
		}

		public sealed class CollectionProf
		{
			public string possessionNum;

			public string totalNum;
		}

		public sealed class PlayHistory
		{
			public string dungeonClearCount;

			public string useMeatNum;

			public string fusionCount;

			public string evolutionCount;

			public string inheritanceCount;

			public string combinationCount;

			public string continueLoginCount;

			public string totalLoginCount;
		}
	}

	public sealed class RequestUS_UserStatus : RequestTypeBase<GameWebAPI.PlayerInfoSendData, GameWebAPI.RespDataUS_GetPlayerInfo>
	{
		public RequestUS_UserStatus()
		{
			this.apiId = "020001";
		}
	}

	public sealed class PlayerInfoSendData : WebAPI.SendBaseData
	{
		public string keys;
	}

	public sealed class RespDataUS_GetPlayerInfo : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo;

		public GameWebAPI.RespDataUS_GetPlayerInfo.LeaderMonster leaderMonster;

		public sealed class PlayerInfo
		{
			public string nickname;

			public string description;

			public string lastLoginTime;

			public string loginTime;

			public int loginTimeSort;

			public int point;

			public int stamina;

			public string staminaMax;

			public int recovery;

			public string gamemoney;

			public string friendPoint;

			public string meatNum;

			public string unitLimitMax;

			public string meatLimitMax;

			public int friendLimitMax;

			public int chipLimitMax;

			public string gemePlayTime;

			public int loginCount;

			public string birthday;

			public string titleId;

			public string countryCode;
		}

		public sealed class LeaderMonster
		{
			public string userMonsterId;

			public string userId;

			public string monsterId;

			public string level;

			public string ex;

			public string levelEx;

			public string nextLevelEx;

			public string leaderSkillId;

			public string uniqueSkillId;

			public string defaultSkillGroupSubId;

			public string commonSkillId;

			public string extraCommonSkillId;

			public string eggFlg;

			public string growEndDate;

			public string monsterEvolutionRouteId;

			public string hp;

			public string attack;

			public string defense;

			public string spAttack;

			public string spDefense;

			public string speed;

			public string luck;

			public string hpAbilityFlg;

			public string hpAbility;

			public string attackAbilityFlg;

			public string attackAbility;

			public string defenseAbilityFlg;

			public string defenseAbility;

			public string spAttackAbilityFlg;

			public string spAttackAbility;

			public string spDefenseAbilityFlg;

			public string spDefenseAbility;

			public string speedAbilityFlg;

			public string speedAbility;

			public string friendship;

			public string statusFlgs;

			public string tranceResistance;

			public string tranceStatusAilment;
		}
	}

	public sealed class RequestUS_UserUpdateNicknameLogic : RequestTypeBase<GameWebAPI.PRF_Req_UpdateNickname, WebAPI.ResponseData>
	{
		public RequestUS_UserUpdateNicknameLogic()
		{
			this.apiId = "020003";
		}
	}

	public sealed class PRF_Req_UpdateNickname : WebAPI.SendBaseData
	{
		public string nickname;
	}

	public sealed class PRF_Req_UpdateDescription : WebAPI.SendBaseData
	{
		public string description;
	}

	public sealed class RequestUS_UpdateDescription : RequestTypeBase<GameWebAPI.PRF_Req_UpdateDescription, WebAPI.ResponseData>
	{
		public RequestUS_UpdateDescription()
		{
			this.apiId = "020004";
		}
	}

	public sealed class RequestUS_UserUpdatePolicy : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataUS_UserUpdatePolicy>
	{
		public RequestUS_UserUpdatePolicy()
		{
			this.apiId = "020010";
		}
	}

	public sealed class RespDataUS_UserUpdatePolicy : WebAPI.ResponseData
	{
		public int resultStatus;
	}

	public sealed class RequestUS_RecoverLife : RequestTypeBase<WebAPI.SendBaseData, WebAPI.ResponseData>
	{
		public RequestUS_RecoverLife()
		{
			this.apiId = "020012";
		}
	}

	public sealed class Request_UpdateBirthday : RequestTypeBase<GameWebAPI.PRF_Req_UpdateBirthday, WebAPI.ResponseData>
	{
		public Request_UpdateBirthday()
		{
			this.apiId = "020014";
		}
	}

	public sealed class PRF_Req_UpdateBirthday : WebAPI.SendBaseData
	{
		public string birthday;
	}

	public sealed class Request_GdprInfo : RequestTypeBase<GameWebAPI.SendGdprInfo, GameWebAPI.ResponseGdprInfo>
	{
		public Request_GdprInfo()
		{
			this.apiId = "020015";
		}
	}

	public sealed class SendGdprInfo : WebAPI.SendBaseData
	{
		public int functionId = 1;
	}

	public sealed class ResponseGdprInfo : WebAPI.ResponseData
	{
		public GameWebAPI.ResponseGdprInfo.Details[] gdprList;

		public sealed class Details
		{
			public int type;

			public string url;
		}
	}

	public sealed class Request_GdprConfirmed : RequestTypeBase<GameWebAPI.SendGdprConfirmed, WebAPI.ResponseData>
	{
		public Request_GdprConfirmed()
		{
			this.apiId = "020016";
		}
	}

	public sealed class SendGdprConfirmed : WebAPI.SendBaseData
	{
		public int functionId = 2;
	}

	public sealed class RequestMonsterList : RequestTypeBase<GameWebAPI.ReqDataUS_GetMonsterList, GameWebAPI.RespDataUS_GetMonsterList>
	{
		public RequestMonsterList()
		{
			this.apiId = "020101";
		}
	}

	public sealed class ReqDataUS_GetMonsterList : WebAPI.SendBaseData
	{
		public int[] userMonsterIds;
	}

	public sealed class RespDataUS_GetMonsterList : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] userMonsterList;

		public class UserMonsterList
		{
			public string userMonsterId;

			public string userId;

			public string monsterId;

			public string level;

			public string ex;

			public string levelEx;

			public string nextLevelEx;

			public string leaderSkillId;

			public string uniqueSkillId;

			public string defaultSkillGroupSubId;

			public string commonSkillId;

			public string extraCommonSkillId;

			public string eggFlg;

			public string growEndDate;

			public string monsterEvolutionRouteId;

			public string hp;

			public string attack;

			public string defense;

			public string spAttack;

			public string spDefense;

			public string speed;

			public string luck;

			public string hpAbilityFlg;

			public string hpAbility;

			public string attackAbilityFlg;

			public string attackAbility;

			public string defenseAbilityFlg;

			public string defenseAbility;

			public string spAttackAbilityFlg;

			public string spAttackAbility;

			public string spDefenseAbilityFlg;

			public string spDefenseAbility;

			public string speedAbilityFlg;

			public string speedAbility;

			public string friendship;

			public string statusFlgs;

			public string tranceResistance;

			public string tranceStatusAilment;

			public string createTimeSec;

			public UserMonsterList()
			{
			}

			public UserMonsterList(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster)
			{
				this.userMonsterId = userMonster.userMonsterId;
				this.userId = userMonster.userId;
				this.monsterId = userMonster.monsterId;
				this.level = userMonster.level;
				this.ex = userMonster.ex;
				this.levelEx = userMonster.levelEx;
				this.nextLevelEx = userMonster.nextLevelEx;
				this.leaderSkillId = userMonster.leaderSkillId;
				this.uniqueSkillId = userMonster.uniqueSkillId;
				this.defaultSkillGroupSubId = userMonster.defaultSkillGroupSubId;
				this.commonSkillId = userMonster.commonSkillId;
				this.extraCommonSkillId = userMonster.extraCommonSkillId;
				this.eggFlg = userMonster.eggFlg;
				this.growEndDate = userMonster.growEndDate;
				this.monsterEvolutionRouteId = userMonster.monsterEvolutionRouteId;
				this.hp = userMonster.hp;
				this.attack = userMonster.attack;
				this.defense = userMonster.defense;
				this.spAttack = userMonster.spAttack;
				this.spDefense = userMonster.spDefense;
				this.speed = userMonster.speed;
				this.luck = userMonster.luck;
				this.friendship = userMonster.friendship;
				this.hpAbilityFlg = userMonster.hpAbilityFlg;
				this.hpAbility = userMonster.hpAbility;
				this.attackAbilityFlg = userMonster.attackAbilityFlg;
				this.attackAbility = userMonster.attackAbility;
				this.defenseAbilityFlg = userMonster.defenseAbilityFlg;
				this.defenseAbility = userMonster.defenseAbility;
				this.spAttackAbilityFlg = userMonster.spAttackAbilityFlg;
				this.spAttackAbility = userMonster.spAttackAbility;
				this.spDefenseAbilityFlg = userMonster.spDefenseAbilityFlg;
				this.spDefenseAbility = userMonster.spDefenseAbility;
				this.speedAbilityFlg = userMonster.speedAbilityFlg;
				this.speedAbility = userMonster.speedAbility;
				this.statusFlgs = userMonster.statusFlgs;
				this.tranceResistance = userMonster.tranceResistance;
				this.tranceStatusAilment = userMonster.tranceStatusAilment;
				this.createTimeSec = userMonster.createTimeSec;
			}

			public bool IsEgg()
			{
				return "1" == this.eggFlg;
			}

			public bool IsGrowing()
			{
				return !string.IsNullOrEmpty(this.growEndDate);
			}

			public bool IsLocked
			{
				get
				{
					return this.statusFlgs == "2" || this.statusFlgs == "3";
				}
			}

			public void SetLock(bool isLock)
			{
				if (isLock)
				{
					this.statusFlgs = Mathf.Clamp(this.statusFlgs.ToInt32() + 2, 0, 3).ToString();
				}
				else
				{
					this.statusFlgs = Mathf.Clamp(this.statusFlgs.ToInt32() - 2, 0, 3).ToString();
				}
			}
		}
	}

	public sealed class RequestUS_GetGardenInfo : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataUS_GetGardenInfo>
	{
		public RequestUS_GetGardenInfo()
		{
			this.apiId = "020102";
		}
	}

	public sealed class RespDataUS_GetGardenInfo : WebAPI.ResponseData
	{
		public GameWebAPI.GardenInfo gardenInfo;
	}

	public sealed class GardenInfo
	{
		public int time1;

		public int time2;

		public GameWebAPI.GardenInfo.MonsterInfo[] monster;

		public sealed class MonsterInfo
		{
			public int userMonsterId;

			public int growStep;

			public int remainingTime;

			public int stoneNum;
		}
	}

	public sealed class RequestUS_RegisterOptionInfo : RequestTypeBase<GameWebAPI.US_Req_RegisterOptionInfo, WebAPI.ResponseData>
	{
		public RequestUS_RegisterOptionInfo()
		{
			this.apiId = "020301";
		}
	}

	public sealed class US_Req_RegisterOptionInfo : WebAPI.SendBaseData
	{
		public Dictionary<string, int> optionList;
	}

	public sealed class UserSoulInfoList : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataUS_GetSoulInfo>
	{
		public UserSoulInfoList()
		{
			this.apiId = "020303";
		}
	}

	public sealed class RespDataUS_GetSoulInfo : WebAPI.ResponseData
	{
		public GameWebAPI.UserSoulData[] userSoulData;
	}

	public sealed class UserSoulData
	{
		public string soulId;

		public string num;
	}

	public sealed class ItemListLogic : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataUS_ItemListLogic>
	{
		public ItemListLogic()
		{
			this.apiId = "020304";
		}
	}

	public sealed class RespDataUS_ItemListLogic : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataUS_ItemListLogic.UserItemData[] userItemData;

		public sealed class UserItemData
		{
			public int itemId;

			public int userItemNum;
		}
	}

	public sealed class RequestUS_RegisterLanguageInfo : RequestTypeBase<GameWebAPI.US_Req_RegisterLanguageInfo, WebAPI.ResponseData>
	{
		public RequestUS_RegisterLanguageInfo()
		{
			this.apiId = "020305";
		}
	}

	public sealed class US_Req_RegisterLanguageInfo : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	public sealed class RequestFR_FriendList : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataFR_FriendList>
	{
		public RequestFR_FriendList()
		{
			this.apiId = "030001";
		}
	}

	public sealed class RespDataFR_FriendList : WebAPI.ResponseData
	{
		public int friendMaxCount;

		public string friendCount;

		public string friendApplicationCount;

		public string friendUnapprovedCount;

		public int friendFreeCount;

		public int friendApplicationFreeCount;

		public int friendUnapprovedFreeCount;

		public GameWebAPI.FriendList[] friendList;
	}

	public sealed class FriendList
	{
		public GameWebAPI.FriendList.UserData userData;

		public GameWebAPI.FriendList.MonsterData monsterData;

		public sealed class UserData
		{
			public string userId;

			public string nickname;

			public string description;

			public string loginTime;

			public int loginTimeSort;

			public string titleId;
		}

		public sealed class MonsterData
		{
			public string monsterId;
		}
	}

	public sealed class RequestFR_FriendApplicationList : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataFR_FriendRequestList>
	{
		public RequestFR_FriendApplicationList()
		{
			this.apiId = "030002";
		}
	}

	public sealed class RespDataFR_FriendRequestList : WebAPI.ResponseData
	{
		public int friendMaxCount;

		public string friendCount;

		public string friendApplicationCount;

		public string friendUnapprovedCount;

		public int friendFreeCount;

		public int friendApplicationFreeCount;

		public int friendUnapprovedFreeCount;

		public GameWebAPI.FriendList[] friendList;
	}

	public sealed class RequestFR_FriendUnapprovedList : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataFR_FriendUnapprovedList>
	{
		public RequestFR_FriendUnapprovedList()
		{
			this.apiId = "030003";
		}
	}

	public sealed class RespDataFR_FriendUnapprovedList : WebAPI.ResponseData
	{
		public int friendMaxCount;

		public string friendCount;

		public string friendApplicationCount;

		public string friendUnapprovedCount;

		public int friendFreeCount;

		public int friendApplicationFreeCount;

		public int friendUnapprovedFreeCount;

		public GameWebAPI.FriendList[] friendList;
	}

	public sealed class RequestFR_FriendInfo : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataFR_FriendInfo>
	{
		public RequestFR_FriendInfo()
		{
			this.apiId = "030004";
		}
	}

	public sealed class RespDataFR_FriendInfo : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataFR_FriendInfo.FriendInfo friendInfo;

		public sealed class FriendInfo
		{
			public int friendMaxCount;

			public string friendCount;

			public string friendApplicationCount;

			public string friendUnapprovedCount;

			public int friendFreeCount;

			public int friendApplicationFreeCount;

			public int friendUnapprovedFreeCount;
		}
	}

	public sealed class RequestFR_FriendApplication : RequestTypeBase<GameWebAPI.FR_Req_FriendRequest, WebAPI.ResponseData>
	{
		public RequestFR_FriendApplication()
		{
			this.apiId = "030101";
		}
	}

	public sealed class FR_Req_FriendRequest : WebAPI.SendBaseData
	{
		public int targetUserId;
	}

	public sealed class RequestFR_FriendApplicationDecision : RequestTypeBase<GameWebAPI.FR_Req_FriendDecision, WebAPI.ResponseData>
	{
		public RequestFR_FriendApplicationDecision()
		{
			this.apiId = "030102";
		}
	}

	public sealed class FR_Req_FriendDecision : WebAPI.SendBaseData
	{
		public int[] targetUserIds;

		public int decide;

		public enum DecisionType
		{
			APPROVE = 1,
			REFUSAL
		}
	}

	public sealed class RequestFR_FriendApplicationCancel : RequestTypeBase<GameWebAPI.FR_Req_FriendRequestCancel, WebAPI.ResponseData>
	{
		public RequestFR_FriendApplicationCancel()
		{
			this.apiId = "030103";
		}
	}

	public sealed class FR_Req_FriendRequestCancel : WebAPI.SendBaseData
	{
		public int[] targetUserIds;
	}

	public sealed class RequestFR_FriendBreak : RequestTypeBase<GameWebAPI.FR_Req_FriendBreak, WebAPI.ResponseData>
	{
		public RequestFR_FriendBreak()
		{
			this.apiId = "030104";
		}
	}

	public sealed class FR_Req_FriendBreak : WebAPI.SendBaseData
	{
		public int[] targetUserIds;
	}

	public sealed class RequestFR_FriendSearchUserCode : RequestTypeBase<GameWebAPI.FR_Req_FriendSearchUserCode, GameWebAPI.RespDataFR_FriendSearchUserCode>
	{
		public RequestFR_FriendSearchUserCode()
		{
			this.apiId = "030201";
		}
	}

	public sealed class RespDataFR_FriendSearchUserCode : WebAPI.ResponseData
	{
		public GameWebAPI.FriendList[] friendList;
	}

	public sealed class RequestGA_GashaInfo : RequestTypeBase<GameWebAPI.GA_Req_GashaInfo, GameWebAPI.RespDataGA_GetGachaInfo>
	{
		public RequestGA_GashaInfo()
		{
			this.apiId = "050001";
		}
	}

	public sealed class GA_Req_GashaInfo : WebAPI.SendBaseData
	{
		public int isTutorial;

		public int countryCode;

		public enum Type
		{
			NORMAL,
			TUTORIAL
		}
	}

	public sealed class RespDataGA_GetGachaInfo : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataGA_GetGachaInfo.Result[] result;

		public sealed class PriceType
		{
			public string category;

			public string value;
		}

		public sealed class Detail
		{
			public string count;

			public string price;

			public string priceFirst;

			public string dailyResetFirst;

			public string appealText;

			public string appealTextDisplayType;

			public int isFirst;

			public int isTodayFirst;
		}

		public sealed class Result
		{
			public string gachaId;

			public string gachaName;

			public string mainImagePath;

			public string[] subImagePath;

			public string startTime;

			public string endTime;

			public string prize;

			public GameWebAPI.RespDataGA_GetGachaInfo.PriceType priceType;

			public string totalPlayLimitCount;

			public string totalPlayCount;

			public GameWebAPI.RespDataGA_GetGachaInfo.Detail[] details;

			public string dispNum;
		}
	}

	public sealed class RequestGA_GashaExec : RequestTypeBase<GameWebAPI.GA_Req_ExecGacha, GameWebAPI.RespDataGA_ExecGacha>
	{
		public RequestGA_GashaExec()
		{
			this.apiId = "050002";
		}
	}

	public sealed class GA_Req_ExecGacha : WebAPI.SendBaseData
	{
		public int gachaId;

		public int playCount;

		public int itemCount;
	}

	public sealed class RespDataGA_ExecGacha : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataGA_ExecGacha.GachaResultMonster[] userMonsterList;

		public GameWebAPI.RespDataGA_ExecGacha.GachaRewardsData[] rewards;

		public sealed class GachaResultMonster : GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList
		{
			public int isNew;

			public GachaResultMonster()
			{
			}

			public GachaResultMonster(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster) : base(userMonster)
			{
			}
		}

		public sealed class GachaRewardsData
		{
			public string assetCategoryId;

			public string assetValue;

			public int count;
		}
	}

	public sealed class RequestGA_ChipExec : RequestTypeBase<GameWebAPI.GA_Req_ExecChip, GameWebAPI.RespDataGA_ExecChip>
	{
		public RequestGA_ChipExec()
		{
			this.apiId = "050012";
		}
	}

	public sealed class GA_Req_ExecChip : WebAPI.SendBaseData
	{
		public int gachaId;

		public int playCount;
	}

	public sealed class RespDataGA_ExecChip : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataGA_ExecChip.UserAssetList[] userAssetList;

		public GameWebAPI.RespDataGA_ExecGacha.GachaRewardsData[] rewards;

		public sealed class UserAssetList
		{
			public string assetCategoryId;

			public string assetValue;

			public string assetNum;

			public string exValue;

			public string userAssetId;

			public string effectType;

			public int isNew;
		}
	}

	public sealed class RequestGA_TicketExec : RequestTypeBase<GameWebAPI.GA_Req_ExecTicket, GameWebAPI.RespDataGA_ExecTicket>
	{
		public RequestGA_TicketExec()
		{
			this.apiId = "050022";
		}
	}

	public sealed class GA_Req_ExecTicket : WebAPI.SendBaseData
	{
		public int gachaId;

		public int playCount;
	}

	public sealed class RespDataGA_ExecTicket : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataGA_ExecTicket.UserDungeonTicketList[] userDungeonTicketList;

		public GameWebAPI.RespDataGA_ExecGacha.GachaRewardsData[] rewards;

		public sealed class UserDungeonTicketList
		{
			public string dungeonTicketId;

			public string num;

			public string effectType;

			public int isNew;
		}
	}

	public sealed class RequestSH_ShopList : RequestTypeBase<GameWebAPI.SendDataSH_ShopList, GameWebAPI.RespDataSH_Info>
	{
		public RequestSH_ShopList()
		{
			this.apiId = "060001";
		}
	}

	public sealed class SendDataSH_ShopList : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	public sealed class RespDataSH_Info : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataSH_Info.ShopList[] shopList;

		public int isShopMaintenance;

		public int isOverDigiStone;

		public sealed class AcquireList
		{
			public string shopPurchaseDetailId;

			public string shopPurchaseId;

			public string assetCategoryId;

			public string assetValue;

			public string assetNum;

			public string omakeFlg;
		}

		public sealed class ProductList
		{
			public string shopPurchaseId;

			public string shopId;

			public string productType;

			public string productTitle;

			public string productId;

			public string tierVer;

			public string tier;

			public string priority;

			public string displayType;

			public string limitCount;

			public string purchasedCount;

			public string osType;

			public string img;

			public string price;

			public string countDownDispFlg;

			public string packFlg;

			public string closeTime;

			public GameWebAPI.RespDataSH_Info.AcquireList[] acquireList;
		}

		public sealed class ShopList
		{
			public string shopId;

			public string shopTitle;

			public string shopType;

			public object shopOpenTime;

			public object shopCloseTime;

			public GameWebAPI.RespDataSH_Info.ProductList[] productList;
		}
	}

	public sealed class RequestSH_PurchaseIos : RequestTypeBase<GameWebAPI.SH_Req_Verify_IOS, GameWebAPI.RespDataSH_ReqVerify>
	{
		public RequestSH_PurchaseIos()
		{
			this.apiId = "060102";
		}
	}

	public sealed class SH_Req_Verify_IOS : WebAPI.SendBaseData
	{
		public string productId;

		public string osVersion;

		public string currencyCode;

		public string countryCode;

		public string priceNumber;

		public string receiptData;
	}

	public sealed class RequestSH_PurchaseAndroid : RequestTypeBase<GameWebAPI.SH_Req_Verify_AND, GameWebAPI.RespDataSH_ReqVerify>
	{
		public RequestSH_PurchaseAndroid()
		{
			this.apiId = "060103";
		}
	}

	public sealed class SH_Req_Verify_AND : WebAPI.SendBaseData
	{
		public string productId;

		public string osVersion;

		public string currencyCode;

		public string countryCode;

		public string priceNumber;

		public string priceAmount;

		public string signedData;

		public string signature;
	}

	public sealed class RespDataSH_ReqVerify : WebAPI.ResponseData
	{
		public string productId;

		public string transactionId;
	}

	public sealed class RequestSH_AgeCheck : RequestTypeBase<GameWebAPI.SH_Req_AgeCheck, GameWebAPI.RespDataSH_AgeCheck>
	{
		public RequestSH_AgeCheck()
		{
			this.apiId = "060110";
		}
	}

	public sealed class SH_Req_AgeCheck : WebAPI.SendBaseData
	{
		public string productId;

		public string ageType;
	}

	public sealed class RespDataSH_AgeCheck : WebAPI.ResponseData
	{
		public int isOverDigiStone;

		public int purchaseEnabled;

		public int isShopMaintenance;
	}

	public sealed class RequestPR_PrizeList : RequestTypeBase<GameWebAPI.PR_Req_PrizeData, GameWebAPI.RespDataPR_PrizeData>
	{
		public RequestPR_PrizeList()
		{
			this.apiId = "070001";
		}
	}

	public sealed class PR_Req_PrizeData : WebAPI.SendBaseData
	{
		public int page;
	}

	public sealed class RespDataPR_PrizeData : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataPR_PrizeData.PrizeData[] prizeData;

		public string prizeTotalCount;

		public sealed class PrizeData
		{
			public string receiveId;

			public string fromType;

			public string assetCategoryId;

			public string assetValue;

			public string assetNum;

			public string message;

			public string receiveLimitTime;

			public string receiveStatus;

			public string updateTime;

			public int isAssetNumLimitOver;

			public const int NUM_LIMIT_OVER = 1;
		}
	}

	public sealed class RequestPR_PrizeReceive : RequestTypeBase<GameWebAPI.PR_Req_PrizeReceiveIds, GameWebAPI.RespDataPR_PrizeReceiveIds>
	{
		public RequestPR_PrizeReceive()
		{
			this.apiId = "070002";
		}
	}

	public sealed class PR_Req_PrizeReceiveIds : WebAPI.SendBaseData
	{
		public int receiveType;

		public string[] receiveIds;

		public const int RECEIVE_TYPE_ID_LIST = 2;
	}

	public sealed class RespDataPR_PrizeReceiveIds : WebAPI.ResponseData
	{
		public string[] prizeReceiveIds;

		public int isWarning;

		public string[] warningData;

		public int resultCode;

		public const int RESULT_ERROR_PRESENT_IS_EXPIRED = 90;

		public const int RESULT_ERROR_PRESENT_NOT_FOUND = 91;

		public const int RESULT_ERROR_INVALID_RECEIVE_TYPE = 92;
	}

	public sealed class RequestPR_PrizeReceiveHistory : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataPR_PrizeReceiveHistory>
	{
		public RequestPR_PrizeReceiveHistory()
		{
			this.apiId = "070003";
		}
	}

	public sealed class RespDataPR_PrizeReceiveHistory : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataPR_PrizeReceiveHistory.PrizeReceiveHistory[] prizeReceiveHistory;

		public sealed class PrizeReceiveHistory
		{
			public string userPrizeReceiveId;

			public string userId;

			public string fromType;

			public string fromId;

			public string assetCategoryId;

			public string assetValue;

			public string assetNum;

			public string message;

			public string receiveLimitTime;

			public string receiveStatus;

			public string exValue;

			public string updateTime;
		}
	}

	public sealed class RequestCL_ColosseumReleaseCriteria : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataCL_ColosseumReleaseCriteria>
	{
		public RequestCL_ColosseumReleaseCriteria()
		{
			this.apiId = "080002";
		}
	}

	public sealed class RespDataCL_ColosseumReleaseCriteria : WebAPI.ResponseData
	{
		public int resultCode;
	}

	public sealed class RequestCL_ColosseumEntry : RequestTypeBase<GameWebAPI.SendDataCL_ColosseumEntry, GameWebAPI.RespDataCL_ColosseumEntry>
	{
		public RequestCL_ColosseumEntry()
		{
			this.apiId = "080101";
		}
	}

	public sealed class SendDataCL_ColosseumEntry : WebAPI.SendBaseData
	{
		public int colosseumId;

		public int isMockBattle;
	}

	public sealed class RespDataCL_ColosseumEntry : WebAPI.ResponseData
	{
		public int resultCode;

		public GameWebAPI.ColosseumUserStatus userStatus;

		public int freeCostBattleCount;
	}

	[Serializable]
	public sealed class ColosseumUserStatus
	{
		public int colosseumRankId;

		public int loseTotal;

		public int loseWeek;

		public string nickname;

		public int score;

		public string userId;

		public int winTotal;

		public int winWeek;

		public string titleId;
	}

	public sealed class RequestCL_GetColosseumReward : RequestTypeBase<GameWebAPI.SendDataCL_GetColosseumReward, GameWebAPI.RespDataCL_GetColosseumReward>
	{
		public RequestCL_GetColosseumReward()
		{
			this.apiId = "080116";
		}
	}

	public sealed class SendDataCL_GetColosseumReward : WebAPI.SendBaseData
	{
		public string act;
	}

	public sealed class ColosseumReward
	{
		public string assetCategoryId;

		public string assetValue;

		public string assetNum;

		public string title;
	}

	public sealed class RespDataCL_GetColosseumReward : WebAPI.ResponseData
	{
		public int resultCode;

		public Dictionary<string, GameWebAPI.ColosseumReward[]> rewardList;

		public Dictionary<string, GameWebAPI.ColosseumReward[]> interimRewardList;

		[CompilerGenerated]
		private static Func<string, int> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<string, int> <>f__mg$cache1;

		public bool ExistReward()
		{
			return 1 == this.resultCode;
		}

		public int maxRewardListKey()
		{
			if (this.rewardList != null)
			{
				IEnumerable<string> keys = this.rewardList.Keys;
				if (GameWebAPI.RespDataCL_GetColosseumReward.<>f__mg$cache0 == null)
				{
					GameWebAPI.RespDataCL_GetColosseumReward.<>f__mg$cache0 = new Func<string, int>(int.Parse);
				}
				return keys.Max(GameWebAPI.RespDataCL_GetColosseumReward.<>f__mg$cache0);
			}
			return 0;
		}

		public int maxInterimRewardListKey()
		{
			if (this.interimRewardList != null)
			{
				IEnumerable<string> keys = this.interimRewardList.Keys;
				if (GameWebAPI.RespDataCL_GetColosseumReward.<>f__mg$cache1 == null)
				{
					GameWebAPI.RespDataCL_GetColosseumReward.<>f__mg$cache1 = new Func<string, int>(int.Parse);
				}
				return keys.Max(GameWebAPI.RespDataCL_GetColosseumReward.<>f__mg$cache1);
			}
			return 0;
		}
	}

	public sealed class RequestCL_Ranking : RequestTypeBase<GameWebAPI.CL_Req_Ranking, GameWebAPI.RespDataCL_Ranking>
	{
		public RequestCL_Ranking()
		{
			this.apiId = "080118";
		}
	}

	public sealed class CL_Req_Ranking : WebAPI.SendBaseData
	{
		public int colosseumId;

		public int begin;

		public int end;
	}

	public sealed class RespDataCL_Ranking : WebAPI.ResponseData
	{
		public int myPoint;

		public int myRankingNo;

		public Dictionary<string, int> pointRankingList;

		public int pointToNextRank;

		private GameWebAPI.RespDataCL_Ranking.RankingData[] _rankingMember;

		public int resultCode;

		public GameWebAPI.RespDataCL_Ranking.RankingData[] rankingMember
		{
			get
			{
				return this._rankingMember;
			}
			set
			{
				this._rankingMember = value;
				if (this._rankingMember == null)
				{
					this._rankingMember = new GameWebAPI.RespDataCL_Ranking.RankingData[0];
				}
			}
		}

		public class RankingData
		{
			public string leaderMonsterId;

			public string nickname;

			public int point;

			public int rank;

			public int userId;

			public string titleId;
		}
	}

	public sealed class RequestWD_BuyDungeon : RequestTypeBase<GameWebAPI.WD_Req_BuyDungeon, GameWebAPI.RespDataWD_BuyDungeon>
	{
		public RequestWD_BuyDungeon()
		{
			this.apiId = "090801";
		}
	}

	public sealed class WD_Req_BuyDungeon : WebAPI.SendBaseData
	{
		public int worldStageId;
	}

	public sealed class RespDataWD_BuyDungeon : WebAPI.ResponseData
	{
		public string expireTime;

		public int timeLeft;
	}

	public sealed class RequestWD_WorldStart : RequestTypeBase<GameWebAPI.WD_Req_DngStart, GameWebAPI.RespDataWD_DungeonStart>
	{
		public RequestWD_WorldStart()
		{
			this.apiId = "090002";
		}
	}

	public sealed class WD_Req_DngStart : WebAPI.SendBaseData
	{
		public string dungeonId;

		public string deckNum;

		public string userDungeonTicketId;
	}

	public class RespDataWD_DungeonStart : WebAPI.ResponseData
	{
		public string startId;

		public string worldDungeonId;

		public GameWebAPI.RespDataWD_DungeonStart.LuckDrop luckDrop;

		public GameWebAPI.RespDataWD_DungeonStart.Deck deck;

		public GameWebAPI.RespDataWD_DungeonStart.DungeonFloor[] dungeonFloor;

		public sealed class Deck
		{
			public string[] userMonsterIds;
		}

		public sealed class LuckDrop
		{
			public int dropBoxType;

			public string assetCategoryId;

			public int assetValue;

			public int assetNum;
		}

		public sealed class Drop
		{
			public int dropBoxType;
		}

		public sealed class Ai
		{
			public int type;

			public int minHpRange;

			public int maxHpRange;

			public int priority;

			public int rate;

			public int lookTarget;

			public int lookStatus;

			public int lookType;

			public int invokeMinRange;

			public int invokeMaxRange;

			public string skillId;
		}

		public sealed class Enemy
		{
			public string worldEnemyId;

			public string monsterId;

			public int type;

			public int level;

			public int hp;

			public int attack;

			public int defense;

			public int spAttack;

			public int spDefense;

			public int speed;

			public string resistanceId;

			public int fixedExp;

			public int fixedMoney;

			public GameWebAPI.RespDataWD_DungeonStart.Drop[] drop;

			public GameWebAPI.RespDataWD_DungeonStart.Ai[] ai;

			public int[] chipIdList;
		}

		public sealed class DungeonFloor
		{
			public string floorId;

			public int floorNum;

			public int floorType;

			public int healingRate;

			public int cameraType;

			public GameWebAPI.RespDataWD_DungeonStart.Enemy[] enemy;
		}
	}

	public sealed class WorldResultLogic : RequestTypeBase<GameWebAPI.WD_Req_DngResult, GameWebAPI.RespDataWD_DungeonResult>
	{
		public WorldResultLogic()
		{
			this.apiId = "090003";
		}
	}

	public sealed class WD_Req_DngResult : WebAPI.SendBaseData
	{
		public string startId;

		public string dungeonId;

		public int clear;

		public int clearRound;

		public int[] aliveInfo;

		public int[][] enemyAliveInfo;
	}

	public sealed class RespDataWD_DungeonResult : WebAPI.ResponseData
	{
		public string worldDungeonId;

		public int clearType;

		public int totalExp;

		public int totalMoney;

		public GameWebAPI.RespDataWD_DungeonResult.DungeonReward[] dungeonReward;

		public GameWebAPI.RespDataWD_DungeonResult.Drop[] dropReward;

		public GameWebAPI.RespDataWD_DungeonResult.OptionDrop[] optionDrop;

		public GameWebAPI.RespDataWD_DungeonResult.EventChipReward[] eventChipReward;

		public sealed class DungeonReward
		{
			public string worldDungeonRewardId;

			public string assetCategoryId;

			public int assetValue;

			public int assetNum;

			public int everyTimeFlg;

			public string exValue;
		}

		public class Drop
		{
			public string assetCategoryId;

			public int assetValue;

			public int assetNum;

			public int dropBoxType;
		}

		public class OptionDrop
		{
			public string type;

			public string subType;

			public string assetCategoryId;

			public string assetValue;

			public string assetNum;

			public string exValue;
		}

		public class EventChipReward
		{
			public string assetCategoryId;

			public string assetValue;

			public string assetNum;

			public string dropBoxType;
		}
	}

	public sealed class RequestWD_Continue : RequestTypeBase<GameWebAPI.WD_Req_Continue, GameWebAPI.RespDataWD_Continue>
	{
		public RequestWD_Continue()
		{
			this.apiId = "090008";
		}
	}

	public sealed class WD_Req_Continue : WebAPI.SendBaseData
	{
		public int startId;

		public int floorNum;

		public int roundNum;

		public int[] userMonsterId;
	}

	public sealed class RespDataWD_Continue : WebAPI.ResponseData
	{
		public int result;

		public bool IsSuccess()
		{
			return 1 == this.result;
		}
	}

	public sealed class RequestWD_WorldDungeonInfo : RequestTypeBase<GameWebAPI.WD_Req_DngInfo, GameWebAPI.RespDataWD_GetDungeonInfo>
	{
		public RequestWD_WorldDungeonInfo()
		{
			this.apiId = "090009";
		}
	}

	public sealed class WD_Req_DngInfo : WebAPI.SendBaseData
	{
		public string worldId;
	}

	public sealed class RespDataWD_GetDungeonInfo : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataWD_GetDungeonInfo.WorldDungeonInfo[] worldDungeonInfo;

		public sealed class Dungeons
		{
			public int worldDungeonId;

			public int status;

			public int isExtraWave;

			public int exp;

			public int money;

			public GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy[] encountEnemies;

			public GameWebAPI.RespDataWD_GetDungeonInfo.DropAsset[] dropAssets;

			public GameWebAPI.RespDataWD_GetDungeonInfo.PlayLimit playLimit;

			public string dungeonTicketId;

			public string userDungeonTicketId;

			public string dungeonTicketNum;
		}

		public sealed class EncountEnemy
		{
			public int worldEnemyId;

			public int monsterId;

			public int type;

			public int resistanceId;
		}

		public sealed class DropAsset
		{
			public int assetCategoryId;

			public int assetValue;

			public int assetNum;

			public int dropBoxType;
		}

		public sealed class PlayLimit
		{
			public string limitType;

			public string initCount;

			public string maxCount;

			public string restCount;

			public string dailyResetFlg;

			public string recoveryFlg;

			public string recoveryCount;

			public int recoveryAssetCategoryId;

			public int recoveryAssetValue;

			public int recoveryAssetNum;
		}

		public sealed class WorldDungeonInfo
		{
			public int worldStageId;

			public string updateTime;

			public string expireTime;

			private int _timeLeft;

			public int isOpen = 1;

			public int isCounting;

			public GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons[] dungeons;

			public DateTime closeTime;

			public int totalTicketNum;

			public int sortIdx;

			public bool isEvent;

			public int timeLeft
			{
				get
				{
					return this._timeLeft;
				}
				set
				{
					this._timeLeft = value;
					this.closeTime = ServerDateTime.Now;
					this.closeTime = this.closeTime.AddSeconds((double)this._timeLeft);
				}
			}
		}
	}

	public sealed class WorldStartDataLogic : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.ReceiveQuestResume>
	{
		public WorldStartDataLogic()
		{
			this.apiId = "090012";
		}
	}

	public sealed class ReceiveQuestResume : GameWebAPI.RespDataWD_DungeonStart
	{
		public string userDungeonTicketId;
	}

	public sealed class WorldDungeonTutorialContinueLogic : RequestTypeBase<GameWebAPI.WD_Req_Continue, GameWebAPI.RespDataWD_Continue>
	{
		public WorldDungeonTutorialContinueLogic()
		{
			this.apiId = "090013";
		}
	}

	public sealed class RequestWD_WorldEventDungeonInfo : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataWD_GetDungeonInfo>
	{
		public RequestWD_WorldEventDungeonInfo()
		{
			this.apiId = "090101";
		}
	}

	public sealed class RequestWD_WorldTicketDungeonInfo : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataWD_GetDungeonInfo>
	{
		public RequestWD_WorldTicketDungeonInfo()
		{
			this.apiId = "090201";
		}
	}

	public sealed class WD_RecoverPlayLimit : WebAPI.SendBaseData
	{
		public int worldDungeonId;
	}

	public sealed class RequestWD_RecoverPlayLimit : RequestTypeBase<GameWebAPI.WD_RecoverPlayLimit, WebAPI.ResponseData>
	{
		public RequestWD_RecoverPlayLimit()
		{
			this.apiId = "090301";
		}
	}

	public sealed class AreaEventResultLogic : RequestTypeBase<GameWebAPI.ReqData_AreaEventResult, GameWebAPI.RespData_AreaEventResult>
	{
		public AreaEventResultLogic()
		{
			this.apiId = "090103";
		}
	}

	public sealed class ReqData_AreaEventResult : WebAPI.SendBaseData
	{
		public string startId;
	}

	public sealed class RespData_AreaEventResult : WebAPI.ResponseData
	{
		public GameWebAPI.RespData_AreaEventResult.Point point;

		public GameWebAPI.RespData_AreaEventResult.Reward[] reward;

		public GameWebAPI.RespData_AreaEventResult.Reward[] nextReward;

		public sealed class Point
		{
			public int totalPoint;

			public int eventPoint;

			public GameWebAPI.RespData_AreaEventResult.Point.BonusPoint[] bonusPoint;

			public sealed class BonusPoint
			{
				public string eventPointBonusMessage;

				public int point;
			}
		}

		public sealed class Reward
		{
			public string eventPointAchieveRewardId;

			public string worldEventId;

			public string point;

			public string assetCategoryId;

			public string assetValue;

			public string assetNum;

			public string exValue;
		}
	}

	public sealed class MissionInfoLogic : RequestTypeBase<GameWebAPI.SendDataMS_MissionInfoLogic, GameWebAPI.RespDataMS_MissionInfoLogic>
	{
		public MissionInfoLogic()
		{
			this.apiId = "110101";
		}
	}

	public sealed class SendDataMS_MissionInfoLogic : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	public sealed class RespDataMS_MissionInfoLogic : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMS_MissionInfoLogic.Result result;

		public struct Result
		{
			public GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[] daily;

			public GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[] total;

			public GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[] beginner;

			public GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[] midrange;

			public sealed class Mission : IComparable
			{
				public string missionId;

				public string missionType;

				public string missionCategoryId;

				public string lastStepFlg;

				public string missionName;

				public int status;

				public int nowValue;

				public GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission.Detail detail;

				public GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission.Reward[] reward;

				public string informationTime;

				public string displayGroup;

				public int CompareTo(object oppositeMissoin)
				{
					if (oppositeMissoin == null)
					{
						return 1;
					}
					int[] array = new int[]
					{
						0,
						-1,
						1
					};
					int num = array[this.status] - array[(oppositeMissoin as GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission).status];
					if (num != 0)
					{
						return num;
					}
					return int.Parse(this.missionId) - int.Parse((oppositeMissoin as GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission).missionId);
				}

				public struct Detail
				{
					public string missionDetailId;

					public string missionId;

					public string missionDetail;

					public string missionRank;

					public string missionValue;

					public string missionRewardListId;
				}

				public struct Reward
				{
					public string missionRewardId;

					public string missionRewardListId;

					public string viewFlg;

					public string assetCategoryId;

					public string assetValue;

					public string assetNum;
				}
			}
		}
	}

	public sealed class PointQuestRankingList : RequestTypeBase<GameWebAPI.ReqDataMS_PointQuestRankingList, GameWebAPI.RespDataMS_PointQuestRankingList>
	{
		public PointQuestRankingList()
		{
			this.apiId = "090901";
		}
	}

	public sealed class ReqDataMS_PointQuestRankingList : WebAPI.SendBaseData
	{
		public int worldEventId;
	}

	public sealed class RespDataMS_PointQuestRankingList : WebAPI.ResponseData
	{
		public int resultCode;

		public int myRankingNo;

		public GameWebAPI.RespDataMS_PointQuestRankingList.RewardData[] myRankRewardList;

		public GameWebAPI.RespDataMS_PointQuestRankingList.RewardData[] nextRankRewardList;

		public int pointToNextRank;

		public Dictionary<string, int> pointRankingList;

		public struct RewardData
		{
			public string eventRankingRewardId;

			public string worldEventId;

			public string name;

			public string rankingUpper;

			public string rankingLower;

			public string assetCategoryId;

			public string assetValue;

			public string assetNum;

			public string debugFlg;
		}
	}

	public sealed class PointQuestInfo : RequestTypeBase<GameWebAPI.ReqDataWD_PointQuestInfo, GameWebAPI.RespDataWD_PointQuestInfo>
	{
		public PointQuestInfo()
		{
			this.apiId = "090902";
		}
	}

	public sealed class ReqDataWD_PointQuestInfo : WebAPI.SendBaseData
	{
		public int worldAreaId;
	}

	public sealed class RespDataWD_PointQuestInfo : WebAPI.ResponseData
	{
		public int worldEventId;

		public int currentPoint;

		public int currentRank;
	}

	public sealed class MissionRewardLogic : RequestTypeBase<GameWebAPI.ReqDataUS_MissionRewardLogic, GameWebAPI.RespDataMS_MissionRewardLogic>
	{
		public MissionRewardLogic()
		{
			this.apiId = "110102";
		}
	}

	public sealed class ReqDataUS_MissionRewardLogic : WebAPI.SendBaseData
	{
		public int missionId;
	}

	public sealed class RespDataMS_MissionRewardLogic : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMS_MissionRewardLogic.Result[] result;

		public struct Result
		{
			public string missionRewardId;

			public string missionRewardListId;

			public string viewFlg;

			public string assetCategoryId;

			public string assetValue;

			public string assetNum;
		}
	}

	public sealed class MissionClear : RequestTypeBase<GameWebAPI.ReqDataUS_MissionClear, GameWebAPI.RespDataMS_MissionClear>
	{
		public MissionClear()
		{
			this.apiId = "110103";
		}
	}

	public sealed class ReqDataUS_MissionClear : WebAPI.SendBaseData
	{
		public int missionCategoryId;
	}

	public sealed class RespDataMS_MissionClear : WebAPI.ResponseData
	{
		public int result;
	}

	public sealed class RequestOP_ChargeItemList : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataOP_ChargeItemList>
	{
		public RequestOP_ChargeItemList()
		{
			this.apiId = "130002";
		}
	}

	public sealed class RespDataOP_ChargeItemList : WebAPI.ResponseData
	{
		public int paymentStarPieceNum;

		public int freeStarPieceNum;
	}

	public sealed class RequestFA_UserFacilityList : RequestTypeBase<GameWebAPI.FA_Req_RequestFA_UserFacilityList, GameWebAPI.RespDataFA_GetFacilityList>
	{
		public RequestFA_UserFacilityList()
		{
			this.apiId = "140006";
		}
	}

	public sealed class FA_Req_RequestFA_UserFacilityList : WebAPI.SendBaseData
	{
		public int userId;
	}

	public sealed class RespDataFA_GetFacilityList : WebAPI.ResponseData
	{
		public UserFacility[] userFacilityList;

		public string currentTime;

		public LastHarvestTime[] lastHarvestTime;

		public int[] monsterIdsInFarm;
	}

	public sealed class RequestFA_UserStockFacilityList : RequestTypeBase<GameWebAPI.FA_Req_RequestFA_UserStockFacilityList, GameWebAPI.RespDataFA_GetStockFacilityList>
	{
		public RequestFA_UserStockFacilityList()
		{
			this.apiId = "140014";
		}
	}

	public sealed class FA_Req_RequestFA_UserStockFacilityList : WebAPI.SendBaseData
	{
		public int userId;
	}

	public sealed class RespDataFA_GetStockFacilityList : WebAPI.ResponseData
	{
		public UserFacility[] userFacilityList;

		public string currentTime;

		public LastHarvestTime[] lastHarvestTime;

		public int[] monsterIdsInFarm;
	}

	public sealed class RequestFA_FacilitySell : RequestTypeBase<GameWebAPI.FA_Req_FacilitySell, GameWebAPI.RespDataFA_FacilitySell>
	{
		public RequestFA_FacilitySell()
		{
			this.apiId = "140007";
		}
	}

	public sealed class FA_Req_FacilitySell : WebAPI.SendBaseData
	{
		public int userFacilityId;
	}

	public sealed class RespDataFA_FacilitySell : WebAPI.ResponseData
	{
		public int sellPrice;
	}

	public sealed class RequestFA_UserFacilityCondition : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataFA_UserFacilityCondition>
	{
		public RequestFA_UserFacilityCondition()
		{
			this.apiId = "140011";
		}
	}

	public sealed class RespDataFA_UserFacilityCondition : WebAPI.ResponseData
	{
		public UserFacilityCondition[] facilityCondition;
	}

	public sealed class RequestFA_FriendFacilityList : RequestTypeBase<GameWebAPI.FA_Req_FriendFacilityList, GameWebAPI.RespDataFA_FriendFacilityList>
	{
		public RequestFA_FriendFacilityList()
		{
			this.apiId = "140012";
		}
	}

	public sealed class FA_Req_FriendFacilityList : WebAPI.SendBaseData
	{
		public int targetUserId;
	}

	public sealed class RespDataFA_FriendFacilityList : WebAPI.ResponseData
	{
		public UserFacility[] userFacilityList;
	}

	public sealed class RequestCP_Campaign : RequestTypeBase<GameWebAPI.CP_Req_Campaign, GameWebAPI.RespDataCP_Campaign>
	{
		public RequestCP_Campaign()
		{
			this.apiId = "160001";
		}
	}

	public sealed class CP_Req_Campaign : WebAPI.SendBaseData
	{
		public int campaignId;
	}

	public sealed class RespDataCP_Campaign : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataCP_Campaign.CampaignInfo[] campaignInfo;

		public GameWebAPI.RespDataCP_Campaign.CampaignInfo GetCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType cpType, bool noTime = false)
		{
			DateTime now = ServerDateTime.Now;
			for (int i = 0; i < this.campaignInfo.Length; i++)
			{
				if (this.campaignInfo[i].GetCmpIdByEnum() == cpType && (this.campaignInfo[i].IsUnderway(now) || noTime))
				{
					return this.campaignInfo[i];
				}
			}
			return null;
		}

		public GameWebAPI.RespDataCP_Campaign.CampaignInfo GetCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType cpType, string key)
		{
			DateTime now = ServerDateTime.Now;
			for (int i = 0; i < this.campaignInfo.Length; i++)
			{
				if (this.campaignInfo[i].GetCmpIdByEnum() == cpType && this.campaignInfo[i].targetValue == key && this.campaignInfo[i].IsUnderway(now))
				{
					return this.campaignInfo[i];
				}
			}
			return null;
		}

		public enum CampaignType
		{
			Invalid,
			QuestExpUp,
			QuestCipUp,
			QuestMatUp,
			QuestRrDrpUp,
			QuestFriUp,
			QuestStmDown,
			QuestExpUpMul = 1001,
			QuestCipUpMul,
			QuestMatUpMul,
			QuestRrDrpUpMul,
			QuestFriUpMul,
			QuestStmDownMul,
			TrainExpUp = 10001,
			TrainCostDown,
			MeatExpUp,
			MeatHrvUp = 11001,
			TrainLuckUp = 10004,
			MedalTakeOverUp = 11002
		}

		public sealed class CampaignInfo
		{
			public string campaignManageId;

			public string campaignId;

			public string targetValue;

			public string rate;

			public string openTime;

			public string closeTime;

			public int GetCmpIdByInt()
			{
				return int.Parse(this.campaignId);
			}

			public GameWebAPI.RespDataCP_Campaign.CampaignType GetCmpIdByEnum()
			{
				int cmpIdByInt = this.GetCmpIdByInt();
				if (Enum.IsDefined(typeof(GameWebAPI.RespDataCP_Campaign.CampaignType), cmpIdByInt))
				{
					return (GameWebAPI.RespDataCP_Campaign.CampaignType)cmpIdByInt;
				}
				global::Debug.LogError("Enum parse error ::: campaignId is invalid");
				return GameWebAPI.RespDataCP_Campaign.CampaignType.Invalid;
			}

			public bool IsUnderway(DateTime now)
			{
				DateTime t = DateTime.Parse(this.openTime);
				DateTime t2 = DateTime.Parse(this.closeTime);
				return DateTime.Compare(t, now) < 0 && DateTime.Compare(t2, now) > 0;
			}

			public bool IsEqualInfo(GameWebAPI.RespDataCP_Campaign.CampaignInfo info)
			{
				return this.campaignManageId == info.campaignManageId && this.campaignId == info.campaignId && this.targetValue == info.targetValue && this.rate == info.rate && this.openTime == info.openTime && this.closeTime == info.closeTime;
			}
		}
	}

	public sealed class EventExchangeInfoLogic : RequestTypeBase<GameWebAPI.ReqDataUS_EventExchangeInfoLogic, GameWebAPI.RespDataMS_EventExchangeInfoLogic>
	{
		public EventExchangeInfoLogic()
		{
			this.apiId = "170001";
		}
	}

	public sealed class ReqDataUS_EventExchangeInfoLogic : WebAPI.SendBaseData
	{
		public int type;

		public int countryCode;
	}

	public sealed class RespDataMS_EventExchangeInfoLogic : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result[] result;

		public sealed class Result
		{
			public string eventExchangeId;

			public string type;

			public string name;

			public string img;

			public string startTime;

			public string endTime;

			public string updateTime;

			public int canExchange;

			public GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result.Detail[] detail;

			public string jumpGachaId;

			public bool IsAlways()
			{
				return this.type == "1";
			}

			public sealed class Detail
			{
				public string eventExchangeDetailId;

				public string eventExchangeId;

				public string eventExchangeItemId;

				public string needNum;

				public string assetCategoryId;

				public string assetValue;

				public string assetNum;

				public string limitNum;

				public string monsterFixedValueId;

				public string maxExtraSlotNum;

				public int canExchange;

				public int isExchangeLimit;

				public int remainCount;

				public GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result.Detail.Item item;

				public sealed class Item
				{
					public string eventExchangeItemId;

					public string eventExchangeId;

					public string assetCategoryId;

					public string assetValue;

					public int count;
				}
			}
		}
	}

	public sealed class EventExchangeLogic : RequestTypeBase<GameWebAPI.ReqDataUS_EventExchangeLogic, GameWebAPI.RespEventExchangeLogic>
	{
		public EventExchangeLogic()
		{
			this.apiId = "170002";
		}
	}

	public sealed class RespEventExchangeLogic : WebAPI.ResponseData
	{
		public string errorCode;

		public int resultStatus;
	}

	public sealed class ReqDataUS_EventExchangeLogic : WebAPI.SendBaseData
	{
		public int eventExchangeDetailId;

		public int exchangeNum;
	}

	public sealed class ReqDataCS_ChipListLogic : RequestTypeBase<GameWebAPI.SendDataCS_ChipListLogic, GameWebAPI.RespDataCS_ChipListLogic>
	{
		public ReqDataCS_ChipListLogic()
		{
			this.apiId = "180001";
		}
	}

	public sealed class SendDataCS_ChipListLogic : WebAPI.SendBaseData
	{
		public int[] userChipId;
	}

	public sealed class RespDataCS_ChipListLogic : WebAPI.ResponseData
	{
		public int count;

		public int resultCode;

		public GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] userChipList;

		public sealed class UserChipList
		{
			public int chipId;

			public int userChipId;

			public int userMonsterId;

			public bool IsUse()
			{
				return 0 != this.userMonsterId;
			}

			public void resetUserMonsterID()
			{
				this.userMonsterId = 0;
			}
		}
	}

	public sealed class ChipEquipLogic : RequestTypeBase<GameWebAPI.ReqDataCS_ChipEquipLogic, GameWebAPI.RespDataCS_ChipEquipLogic>
	{
		public ChipEquipLogic()
		{
			this.apiId = "180002";
		}
	}

	public sealed class ReqDataCS_ChipEquipLogic : WebAPI.SendBaseData
	{
		public int act;

		public int dispNum;

		public int type;

		public int userChipId;

		public int userMonsterId;

		public GameWebAPI.ReqDataCS_ChipEquipLogic.ACT GetActEnum()
		{
			return (GameWebAPI.ReqDataCS_ChipEquipLogic.ACT)this.act;
		}

		public enum ACT
		{
			REMOVE,
			ATTACH
		}
	}

	public sealed class RespDataCS_ChipEquipLogic : WebAPI.ResponseData
	{
		public int resultCode;

		public GameWebAPI.RespDataCS_ChipEquipLogic.Equip[] equip;

		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster;

		public class Equip
		{
			public int dispNum;

			public int type;

			public int userChipId;
		}
	}

	public sealed class ChipFusionLogic : RequestTypeBase<GameWebAPI.ReqDataCS_ChipFusionLogic, GameWebAPI.RespDataCS_ChipFusionLogic>
	{
		public ChipFusionLogic()
		{
			this.apiId = "180003";
		}
	}

	public sealed class ReqDataCS_ChipFusionLogic : WebAPI.SendBaseData
	{
		public int baseChip;

		public int[] materialChip;
	}

	public sealed class RespDataCS_ChipFusionLogic : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataCS_ChipFusionLogic.UserChip userChip;

		public int resultCode;

		public sealed class UserChip
		{
			public int chipId;
		}
	}

	public sealed class ChipUnlockExtraSlotLogic : RequestTypeBase<GameWebAPI.ReqDataCS_ChipUnlockExtraSlotLogic, GameWebAPI.RespDataCS_ChipUnlockExtraSlotLogic>
	{
		public ChipUnlockExtraSlotLogic()
		{
			this.apiId = "180004";
		}
	}

	public sealed class ReqDataCS_ChipUnlockExtraSlotLogic : WebAPI.SendBaseData
	{
		public int dispNum;

		public int userMonsterId;
	}

	public sealed class RespDataCS_ChipUnlockExtraSlotLogic : WebAPI.ResponseData
	{
		public int resultCode;
	}

	public sealed class ChipSellLogic : RequestTypeBase<GameWebAPI.ReqDataCS_ChipSellLogic, GameWebAPI.RespDataCS_ChipSellLogic>
	{
		public ChipSellLogic()
		{
			this.apiId = "180005";
		}
	}

	public sealed class ReqDataCS_ChipSellLogic : WebAPI.SendBaseData
	{
		public int[] materialChip;
	}

	public sealed class RespDataCS_ChipSellLogic : WebAPI.ResponseData
	{
		public int resultCode;

		public int itemRecovered;

		public bool IsBonus()
		{
			return 1 == this.itemRecovered;
		}
	}

	public sealed class MonsterSlotInfoListLogic : RequestTypeBase<GameWebAPI.ReqDataCS_MonsterSlotInfoListLogic, GameWebAPI.RespDataCS_MonsterSlotInfoListLogic>
	{
		public MonsterSlotInfoListLogic()
		{
			this.apiId = "180006";
		}
	}

	public sealed class ReqDataCS_MonsterSlotInfoListLogic : WebAPI.SendBaseData
	{
		public int[] userMonsterId;
	}

	public sealed class RespDataCS_MonsterSlotInfoListLogic : WebAPI.ResponseData
	{
		public int resultCode;

		public GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo[] slotInfo;

		public class Equip
		{
			public int dispNum;

			public int type;

			public int userChipId;
		}

		public sealed class Manage
		{
			public int slotNum;

			public int extraSlotNum;

			public int maxSlotNum;

			public int maxExtraSlotNum;
		}

		public sealed class SlotInfo
		{
			public int userMonsterId;

			public GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip[] equip;

			public GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Manage manage;
		}

		public enum TYPE
		{
			FREE,
			EXTRA
		}
	}

	public sealed class RequestNV_NavigationMessage : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataNV_NavigationMessage>
	{
		public RequestNV_NavigationMessage()
		{
			this.apiId = "190001";
		}
	}

	public sealed class RespDataNV_NavigationMessage : WebAPI.ResponseData
	{
		public int[] navigationMessageIdList;
	}

	public sealed class RequestNV_NavigationMessageReadStatusUpdate : RequestTypeBase<GameWebAPI.SendDataNV_NavigationMessageReadStatusUpdate, WebAPI.ResponseData>
	{
		public RequestNV_NavigationMessageReadStatusUpdate()
		{
			this.apiId = "190002";
		}
	}

	public sealed class SendDataNV_NavigationMessageReadStatusUpdate : WebAPI.SendBaseData
	{
		public int[] navigationMessageId;
	}

	public sealed class RequestTL_GetUserTitleList : RequestTypeBase<GameWebAPI.SendDataTL_GetUserTitleList, GameWebAPI.RespDataTL_GetUserTitleList>
	{
		public RequestTL_GetUserTitleList()
		{
			this.apiId = "200001";
		}
	}

	public sealed class SendDataTL_GetUserTitleList : WebAPI.SendBaseData
	{
	}

	public sealed class RespDataTL_GetUserTitleList : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataTL_GetUserTitleList.UserTitleList[] userTitleList;

		public class UserTitleList
		{
			public string userTitleId;

			public string userId;

			public string titleId;
		}
	}

	public sealed class RequestTL_EditUserTitle : RequestTypeBase<GameWebAPI.SendDataTL_EditUserTitle, GameWebAPI.RespDataTL_EditUserTitle>
	{
		public RequestTL_EditUserTitle()
		{
			this.apiId = "200002";
		}
	}

	public sealed class SendDataTL_EditUserTitle : WebAPI.SendBaseData
	{
		public int titleId;
	}

	public sealed class RespDataTL_EditUserTitle : WebAPI.ResponseData
	{
		public int resultStatus;
	}

	public sealed class RequestTL_ModelChangeRequest : RequestTypeBase<GameWebAPI.SendDataTL_ModelChangeRequest, GameWebAPI.RespDataTL_ModelChangeRequest>
	{
		public RequestTL_ModelChangeRequest()
		{
			this.apiId = "210001";
		}
	}

	public sealed class SendDataTL_ModelChangeRequest : WebAPI.SendBaseData
	{
		public string accessToken;
	}

	public sealed class RespDataTL_ModelChangeRequest : WebAPI.ResponseData
	{
		public int appliId;

		public string userCode;

		public string transferCode;

		public string expireTime;
	}

	public sealed class RequestTL_UserSocialStatusInfo : RequestTypeBase<GameWebAPI.SendDataTL_UserSocialStatusInfo, GameWebAPI.RespDataTL_UserSocialStatusInfo>
	{
		public RequestTL_UserSocialStatusInfo()
		{
			this.apiId = "210002";
		}
	}

	public sealed class SendDataTL_UserSocialStatusInfo : WebAPI.SendBaseData
	{
		public string accessToken;
	}

	public sealed class RespDataTL_UserSocialStatusInfo : WebAPI.ResponseData
	{
		public string userCode;

		public string nickname;

		public string transferCode;
	}

	public sealed class RequestTL_DeleteUserSocialStatus : RequestTypeBase<GameWebAPI.SendDataTL_DeleteUserSocialStatus, GameWebAPI.RespDataTL_DeleteUserSocialStatus>
	{
		public RequestTL_DeleteUserSocialStatus()
		{
			this.apiId = "210003";
		}
	}

	public sealed class SendDataTL_DeleteUserSocialStatus : WebAPI.SendBaseData
	{
		public string accessToken;
	}

	public sealed class RespDataTL_DeleteUserSocialStatus : WebAPI.ResponseData
	{
		public int resultCode;

		public bool IsSuccess()
		{
			return 1 == this.resultCode;
		}
	}

	public sealed class RequestTL_ModelChangeAuthLogic : RequestTypeBase<GameWebAPI.SendDataTL_ModelChangeAuthLogic, GameWebAPI.RespDataTL_ModelChangeAuthLogic>
	{
		public RequestTL_ModelChangeAuthLogic()
		{
			this.apiId = "210004";
		}
	}

	public sealed class SendDataTL_ModelChangeAuthLogic : WebAPI.SendBaseData
	{
		public string accessToken;

		public int transferUserCode;

		public string transferCode;
	}

	public sealed class RespDataTL_ModelChangeAuthLogic : WebAPI.ResponseData
	{
		public int transferStatus;
	}

	public sealed class RequestTL_UserSocialStatusCheckLogic : RequestTypeBase<GameWebAPI.SendDataTL_UserSocialStatusCheckLogic, GameWebAPI.RespDataTL_UserSocialStatusCheckLogic>
	{
		public RequestTL_UserSocialStatusCheckLogic()
		{
			this.apiId = "210005";
		}
	}

	public sealed class SendDataTL_UserSocialStatusCheckLogic : WebAPI.SendBaseData
	{
		public string accessToken;
	}

	public sealed class RespDataTL_UserSocialStatusCheckLogic : WebAPI.ResponseData
	{
		public int resultCode;

		public bool Authenticated()
		{
			return 1 == this.resultCode;
		}
	}

	public sealed class RequestIN_InfoList : RequestTypeBase<GameWebAPI.SendDataIN_InfoList, GameWebAPI.RespDataIN_InfoList>
	{
		public RequestIN_InfoList()
		{
			this.apiId = "900001";
		}
	}

	public sealed class SendDataIN_InfoList : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	public sealed class RespDataIN_InfoList : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataIN_InfoList.InfoList[] infoList;

		public sealed class InfoList
		{
			public string userInfoId;

			public string title;

			public string groupType;

			public string categoryType;

			public string linkCategoryType;

			public int confirmationFlg;

			public string startDateTime;

			public int prizeStatus;

			public int popupFlg;

			public int viewPriority;
		}
	}

	public sealed class RequestIN_disablePopup : RequestTypeBase<GameWebAPI.SendDataIN_disablePopup, WebAPI.ResponseData>
	{
		public RequestIN_disablePopup()
		{
			this.apiId = "900002";
		}
	}

	public sealed class SendDataIN_disablePopup : WebAPI.SendBaseData
	{
		public int userInfoId;
	}

	public sealed class RequestBL_BlockSet : RequestTypeBase<GameWebAPI.BL_Req_BlockSet, GameWebAPI.RespDataBL_BlockSet>
	{
		public RequestBL_BlockSet()
		{
			this.apiId = "920001";
		}
	}

	public sealed class BL_Req_BlockSet : WebAPI.SendBaseData
	{
		public int targetUserId;
	}

	public sealed class RespDataBL_BlockSet : WebAPI.ResponseData
	{
		public string userBlockId;
	}

	public sealed class RequestBL_BlockList : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataBL_BlockList>
	{
		public RequestBL_BlockList()
		{
			this.apiId = "920002";
		}
	}

	public sealed class FR_Req_FriendSearchUserCode : WebAPI.SendBaseData
	{
		public int userCode;
	}

	public sealed class RespDataBL_BlockList : WebAPI.ResponseData
	{
		public GameWebAPI.FriendList[] blockList;
	}

	public sealed class RequestBL_BlockReset : RequestTypeBase<GameWebAPI.BL_Req_BlockReset, WebAPI.ResponseData>
	{
		public RequestBL_BlockReset()
		{
			this.apiId = "920003";
		}
	}

	public sealed class BL_Req_BlockReset : WebAPI.SendBaseData
	{
		public int targetUserId;
	}

	public sealed class RequestDBG_AddParam : RequestTypeBase<GameWebAPI.GA_Req_DbgAddParamExec, GameWebAPI.RespDataGA_DbgAddParamExec>
	{
		public RequestDBG_AddParam()
		{
			this.apiId = "050098";
		}
	}

	public sealed class GA_Req_DbgAddParamExec : WebAPI.SendBaseData
	{
		public int type;

		public int num;
	}

	public sealed class RespDataGA_DbgAddParamExec : WebAPI.ResponseData
	{
		public int stone;

		public string friendPoint;

		public string money;

		public string meat;
	}

	public sealed class RequestDBG_AddSoul : RequestTypeBase<GameWebAPI.GA_Req_DbgSoul, GameWebAPI.RespDataGA_ReqDbgSoul>
	{
		public RequestDBG_AddSoul()
		{
			this.apiId = "050102";
		}
	}

	public sealed class GA_Req_DbgSoul : WebAPI.SendBaseData
	{
		public int num;
	}

	public sealed class RespDataGA_ReqDbgSoul : WebAPI.ResponseData
	{
		public int result;
	}

	public sealed class DebugMissionDailyReset : RequestTypeBase<GameWebAPI.ReqDataUS_DebugMissionDailyReset, GameWebAPI.RespDataMS_DebugMissionDailyReset>
	{
		public DebugMissionDailyReset()
		{
			this.apiId = "981101";
		}
	}

	public sealed class ReqDataUS_DebugMissionDailyReset : WebAPI.SendBaseData
	{
		public int missionCategoryId;
	}

	public sealed class RespDataMS_DebugMissionDailyReset : WebAPI.ResponseData
	{
		public int result;
	}

	public sealed class DebugMissionAddCount : RequestTypeBase<GameWebAPI.ReqDataUS_DebugMissionAddCount, GameWebAPI.RespDataMS_DebugMissionAddCount>
	{
		public DebugMissionAddCount()
		{
			this.apiId = "981102";
		}
	}

	public sealed class ReqDataUS_DebugMissionAddCount : WebAPI.SendBaseData
	{
		public int missionCategoryId;

		public int addCount;
	}

	public sealed class RespDataMS_DebugMissionAddCount : WebAPI.ResponseData
	{
		public int result;
	}

	public sealed class RequestMA_AssetCategoryMaster : RequestTypeBase<GameWebAPI.RequestMA_AssetCategoryM, GameWebAPI.RespDataMA_GetAssetCategoryM>
	{
		public RequestMA_AssetCategoryMaster()
		{
			this.apiId = "990003";
		}
	}

	public sealed class RequestMA_AssetCategoryM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetAssetCategoryM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM[] assetCategoryM;

		public GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM GetAssetCategory(string categoryID)
		{
			for (int i = 0; i < this.assetCategoryM.Length; i++)
			{
				if (this.assetCategoryM[i].assetCategoryId == categoryID)
				{
					return this.assetCategoryM[i];
				}
			}
			return null;
		}

		[Serializable]
		public sealed class AssetCategoryM
		{
			public string assetCategoryId;

			public string assetTitle;
		}
	}

	public sealed class RequestMA_AssetSalesBonusMaster : RequestTypeBase<GameWebAPI.SendAssetSalesBonusMaster, GameWebAPI.ResponseAssetSalesBonusMaster>
	{
		public RequestMA_AssetSalesBonusMaster()
		{
			this.apiId = "990004";
		}
	}

	public sealed class SendAssetSalesBonusMaster : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class ResponseAssetSalesBonusMaster : WebAPI.ResponseData
	{
		public GameWebAPI.ResponseAssetSalesBonusMaster.SalesBonus[] assetSalesBonusM;

		[Serializable]
		public sealed class SalesBonus
		{
			public string assetSalesBonusId;

			public string baseAssetCategoryId;

			public string baseAssetValue;

			public string bonusAssetCategoryId;

			public string bonusAssetValue;

			public string bonusAssetNum;

			public string exValue;
		}
	}

	public sealed class RequestMA_MonsterMasterWithoutGroupData : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetMonsterMS>
	{
		public RequestMA_MonsterMasterWithoutGroupData()
		{
			this.apiId = "990102";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetMonsterMS : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetMonsterMS.MonsterM[] monsterM;

		[Serializable]
		public sealed class MonsterM
		{
			public string monsterId;

			public string monsterGroupId;

			public string skillGroupId;

			public string rare;

			public string iconId;

			public string maxLevel;

			public string combinationFlg;

			public string defaultHp;

			public string maxHp;

			public string defaultAttack;

			public string maxAttack;

			public string defaultDefense;

			public string maxDefense;

			public string defaultSpAttack;

			public string maxSpAttack;

			public string defaultSpDefense;

			public string maxSpDefense;

			public string speed;

			public string maxLuck;

			public string resistanceId;

			public string price;

			public string priceRise;

			public string fusionExp;

			public string fusionExpRise;

			public int GetArousal()
			{
				return int.Parse(this.rare) - 1;
			}
		}
	}

	public sealed class RequestMA_MonsterMasterOnlyGroupData : RequestTypeBase<GameWebAPI.RequestMA_MonsterM, GameWebAPI.RespDataMA_GetMonsterMG>
	{
		public RequestMA_MonsterMasterOnlyGroupData()
		{
			this.apiId = "990103";
		}
	}

	public sealed class RequestMA_MonsterM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetMonsterMG : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetMonsterMG.MonsterM[] monsterM;

		[Serializable]
		public sealed class MonsterM
		{
			public string monsterGroupId;

			public string modelId;

			public string monsterCollectionId;

			public string monsterName;

			public string simpleDescription;

			public string description;

			public string growStep;

			public string tribe;

			public string monsterStatusId;

			public string monsterType;

			public string leaderSkillId;

			public string partyCharaPosX;

			public string partyCharaPosY;

			public string partyCharaPosZ;

			public string partyCharaRotY;
		}
	}

	public sealed class RequestMA_MonsterEvolutionMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetMonsterEvolutionM>
	{
		public RequestMA_MonsterEvolutionMaster()
		{
			this.apiId = "990104";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetMonsterEvolutionM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution[] monsterEvolutionM;

		public void Initialize()
		{
			Array.Sort<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>(this.monsterEvolutionM, new Comparison<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>(this.SortFunc));
		}

		private int SortFunc(GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution a, GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution b)
		{
			int num = int.Parse(a.baseMonsterId);
			int num2 = int.Parse(b.baseMonsterId);
			if (num > num2)
			{
				return 1;
			}
			if (num < num2)
			{
				return -1;
			}
			int num3 = int.Parse(a.nextMonsterId);
			int num4 = int.Parse(b.nextMonsterId);
			if (num3 > num4)
			{
				return 1;
			}
			if (num3 < num4)
			{
				return -1;
			}
			return 0;
		}

		[Serializable]
		public sealed class Evolution
		{
			public string monsterEvolutionId;

			public string baseMonsterId;

			public string nextMonsterId;

			public string type;

			public string effectType;

			public string effectMonsterId;

			public int monsterEvolutionMaterialId;

			public bool IsEvolution()
			{
				return "1" == this.type || "2" == this.type;
			}

			public bool IsVersionUp()
			{
				return "3" == this.type;
			}
		}
	}

	public sealed class RequestMA_MonsterExperienceMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetMonsterExperienceM>
	{
		public RequestMA_MonsterExperienceMaster()
		{
			this.apiId = "990106";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetMonsterExperienceM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetMonsterExperienceM.MonsterExperienceM[] monsterExperienceM;

		[Serializable]
		public sealed class MonsterExperienceM
		{
			public string monsterExperienceId;

			public string level;

			public string experienceNum;
		}
	}

	public sealed class RequestMA_MonsterResistanceMaster : RequestTypeBase<GameWebAPI.RequestMA_MonsterResistanceM, GameWebAPI.RespDataMA_GetMonsterResistanceM>
	{
		public RequestMA_MonsterResistanceMaster()
		{
			this.apiId = "990107";
		}
	}

	public sealed class RequestMA_MonsterResistanceM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetMonsterResistanceM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM[] monsterResistanceM;

		[Serializable]
		public sealed class MonsterResistanceM
		{
			public string monsterResistanceId;

			public string description;

			public string fire;

			public string water;

			public string thunder;

			public string nature;

			public string none;

			public string light;

			public string dark;

			public string poison;

			public string confusion;

			public string paralysis;

			public string sleep;

			public string stun;

			public string skillLock;

			public string death;
		}
	}

	public sealed class RequestMA_SkillMaster : RequestTypeBase<GameWebAPI.RequestMA_SkillM, GameWebAPI.RespDataMA_GetSkillM>
	{
		public RequestMA_SkillMaster()
		{
			this.apiId = "990108";
		}
	}

	public sealed class RequestMA_SkillM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetSkillM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetSkillM.SkillM[] skillM;

		[Serializable]
		public sealed class SkillM
		{
			public string skillId;

			public string name;

			public string simpleDescription;

			public string description;

			public string type;

			public string skillGroupId;

			public string skillGroupSubId;

			public string needPoint;

			public string inheritancePrice;

			public string attackEffect;

			public string soundEffect;

			public string rank;

			public string useCountValue;

			public static string ActionSkill;

			public string GetActionSkill
			{
				get
				{
					if (this.type == null)
					{
						return null;
					}
					if (this.skillId == null)
					{
						return null;
					}
					if (!(this.type == "4"))
					{
						return null;
					}
					int num;
					if (!int.TryParse(this.skillId, out num))
					{
						return null;
					}
					return this.skillId;
				}
			}
		}
	}

	public sealed class RequestMA_SkillDetailMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetSkillDetailM>
	{
		public RequestMA_SkillDetailMaster()
		{
			this.apiId = "990109";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetSkillDetailM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM[] convertSkillDetailM;

		public GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetailM
		{
			set
			{
				this.convertSkillDetailM = SkillConverter.Convert(value);
			}
		}

		[Serializable]
		public sealed class SkillDetailM
		{
			public string skillId;

			public string subId;

			public int effectType;

			public int hitRate;

			public int target;

			public int targetType;

			public int attribute;

			public int isMissTrough;

			public int effect1;

			public int effect2;

			public int effect3;

			public int effect4;

			public int effect5;

			public int effect6;

			public int effect7;

			public int effect8;

			public int effect9;

			public int effect10;

			public int effect11;

			public int effect12;

			public int effect13;

			public int effect14;

			public int effect15;

			public int effect16;
		}

		[Serializable]
		public sealed class ReceiveSkillDetailM
		{
			public string skillDetailId = string.Empty;

			public string skillId = string.Empty;

			public string subId = string.Empty;

			public string effectType = string.Empty;

			public string effect = string.Empty;

			public string effect2 = string.Empty;

			public string effect3 = string.Empty;

			public string effect4 = string.Empty;

			public string effect5 = string.Empty;

			public string target = string.Empty;

			public string targetType = string.Empty;

			public string attribute = string.Empty;

			public string motionCount = string.Empty;

			public string hitRate = string.Empty;

			public string isMissTrough = string.Empty;

			public string continuousRound = string.Empty;

			public string subRate = string.Empty;
		}
	}

	public sealed class RequestMA_MonsterEvolutionRouteMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM>
	{
		public RequestMA_MonsterEvolutionRouteMaster()
		{
			this.apiId = "990110";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetMonsterEvolutionRouteM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM.MonsterEvolutionRouteM[] monsterEvolutionRouteM;

		[Serializable]
		public sealed class MonsterEvolutionRouteM
		{
			public string monsterEvolutionRouteId;

			public string eggMonsterId;

			public string childhood1MonsterId;

			public string childhood2MonsterId;

			public string growthMonsterId;
		}
	}

	public sealed class RequestMA_MonsterTribeMaster : RequestTypeBase<GameWebAPI.RequestMA_MonsterTribeM, GameWebAPI.RespDataMA_GetMonsterTribeM>
	{
		public RequestMA_MonsterTribeMaster()
		{
			this.apiId = "990111";
		}
	}

	public sealed class RequestMA_MonsterTribeM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetMonsterTribeM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetMonsterTribeM.MonsterTribeM[] monsterTribeM;

		[Serializable]
		public sealed class MonsterTribeM
		{
			public string monsterTribeId;

			public string monsterTribeName;
		}
	}

	public sealed class RequestMA_MonsterGrowStepMaster : RequestTypeBase<GameWebAPI.RequestMA_MonsterGrowStepM, GameWebAPI.RespDataMA_GetMonsterGrowStepM>
	{
		public RequestMA_MonsterGrowStepMaster()
		{
			this.apiId = "990112";
		}
	}

	public sealed class RequestMA_MonsterGrowStepM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetMonsterGrowStepM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetMonsterGrowStepM.MonsterGrowStepM[] monsterGrowStepM;

		[Serializable]
		public sealed class MonsterGrowStepM
		{
			public string monsterGrowStepId;

			public string monsterGrowStepName;

			public string maxFriendship;
		}
	}

	public sealed class RequestMA_MonsterTranceMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetMonsterTranceM>
	{
		public RequestMA_MonsterTranceMaster()
		{
			this.apiId = "990113";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetMonsterTranceM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetMonsterTranceM.MonsterTranceM[] monsterTranceM;

		[Serializable]
		public sealed class MonsterTranceM
		{
			public string monsterTranceId;

			public string monsterId;

			public string soulId;

			public string num;
		}
	}

	public sealed class RequestMA_MonsterTribeTranceMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetMonsterTribeTranceM>
	{
		public RequestMA_MonsterTribeTranceMaster()
		{
			this.apiId = "990114";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetMonsterTribeTranceM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetMonsterTribeTranceM.MonsterTribeTranceM[] monsterTribeTranceM;

		[Serializable]
		public sealed class MonsterTribeTranceM
		{
			public string monsterTribeTranceId;

			public string tribe;

			public string growStep;

			public string soulId;

			public string num;
		}
	}

	public sealed class RequestMonsterEvolutionMaterialMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.MonsterEvolutionMaterialMaster>
	{
		public RequestMonsterEvolutionMaterialMaster()
		{
			this.apiId = "990115";
		}
	}

	[Serializable]
	public sealed class MonsterEvolutionMaterialMaster : WebAPI.ResponseData
	{
		public GameWebAPI.MonsterEvolutionMaterialMaster.Material[] monsterEvolutionMaterialM;

		public GameWebAPI.MonsterEvolutionMaterialMaster.Material[] Materials
		{
			get
			{
				return this.monsterEvolutionMaterialM;
			}
		}

		[Serializable]
		public sealed class Material
		{
			public int monsterEvolutionMaterialId;

			public string materialAssetCategoryId1;

			public string materialAssetValue1;

			public string materialAssetNum1;

			public string materialAssetCategoryId2;

			public string materialAssetValue2;

			public string materialAssetNum2;

			public string materialAssetCategoryId3;

			public string materialAssetValue3;

			public string materialAssetNum3;

			public string materialAssetCategoryId4;

			public string materialAssetValue4;

			public string materialAssetNum4;

			public string materialAssetCategoryId5;

			public string materialAssetValue5;

			public string materialAssetNum5;

			public string materialAssetCategoryId6;

			public string materialAssetValue6;

			public string materialAssetNum6;

			public string materialAssetCategoryId7;

			public string materialAssetValue7;

			public string materialAssetNum7;

			[NonSerialized]
			public const int MATERIAL_ASSET_MAX = 7;

			public string GetAssetCategoryId(int num)
			{
				string empty = string.Empty;
				switch (num)
				{
				case 1:
					empty = this.materialAssetCategoryId1;
					break;
				case 2:
					empty = this.materialAssetCategoryId2;
					break;
				case 3:
					empty = this.materialAssetCategoryId3;
					break;
				case 4:
					empty = this.materialAssetCategoryId4;
					break;
				case 5:
					empty = this.materialAssetCategoryId5;
					break;
				case 6:
					empty = this.materialAssetCategoryId6;
					break;
				case 7:
					empty = this.materialAssetCategoryId7;
					break;
				}
				return empty;
			}

			public string GetAssetValue(int num)
			{
				string empty = string.Empty;
				switch (num)
				{
				case 1:
					empty = this.materialAssetValue1;
					break;
				case 2:
					empty = this.materialAssetValue2;
					break;
				case 3:
					empty = this.materialAssetValue3;
					break;
				case 4:
					empty = this.materialAssetValue4;
					break;
				case 5:
					empty = this.materialAssetValue5;
					break;
				case 6:
					empty = this.materialAssetValue6;
					break;
				case 7:
					empty = this.materialAssetValue7;
					break;
				}
				return empty;
			}

			public string GetAssetNum(int num)
			{
				string empty = string.Empty;
				switch (num)
				{
				case 1:
					empty = this.materialAssetNum1;
					break;
				case 2:
					empty = this.materialAssetNum2;
					break;
				case 3:
					empty = this.materialAssetNum3;
					break;
				case 4:
					empty = this.materialAssetNum4;
					break;
				case 5:
					empty = this.materialAssetNum5;
					break;
				case 6:
					empty = this.materialAssetNum6;
					break;
				case 7:
					empty = this.materialAssetNum7;
					break;
				}
				return empty;
			}
		}
	}

	public sealed class RequestMonsterFixedMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_MonsterFixedM>
	{
		public RequestMonsterFixedMaster()
		{
			this.apiId = "990116";
		}
	}

	[Serializable]
	public sealed class RespDataMA_MonsterFixedM : WebAPI.ResponseData
	{
		public MonsterFixedM[] monsterFixedValueM;
	}

	public sealed class RequestMA_AbilityUpgradeMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_AbilityUpgradeM>
	{
		public RequestMA_AbilityUpgradeMaster()
		{
			this.apiId = "990117";
		}
	}

	[Serializable]
	public sealed class RespDataMA_AbilityUpgradeM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_AbilityUpgradeM.AbilityUpgradeRateM[] abilityUpgradeRateM;

		[Serializable]
		public sealed class AbilityUpgradeRateM
		{
			public string abilityUpgradeRateId;

			public string baseAbility;

			public string materialAbility;

			public string rate;
		}
	}

	public sealed class RequestMonsterArousalMaster : RequestTypeBase<GameWebAPI.RequestMonsterArousalM, GameWebAPI.RespDataMA_MonsterArousalMaster>
	{
		public RequestMonsterArousalMaster()
		{
			this.apiId = "990118";
		}
	}

	public sealed class RequestMonsterArousalM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_MonsterArousalMaster : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_MonsterArousalMaster.ArousalData[] monsterRareM;

		[Serializable]
		public sealed class ArousalData
		{
			public string monsterRareId;

			public string title;

			public string name;

			public string goldMedalMaxNum;
		}
	}

	public sealed class RequestMonsterSpecificTypeMaster : RequestTypeBase<GameWebAPI.RequestMonsterSpecificTypeM, GameWebAPI.RespDataMA_MonsterSpecificTypeMaster>
	{
		public RequestMonsterSpecificTypeMaster()
		{
			this.apiId = "990119";
		}
	}

	public sealed class RequestMonsterSpecificTypeM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_MonsterSpecificTypeMaster : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_MonsterSpecificTypeMaster.SpecificType[] monsterStatusM;

		[Serializable]
		public sealed class SpecificType
		{
			public string monsterStatusId;

			public string name;
		}
	}

	public sealed class RequestMonsterStatusAilmentMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_MonsterStatusAilmentMaster>
	{
		public RequestMonsterStatusAilmentMaster()
		{
			this.apiId = "990120";
		}
	}

	[Serializable]
	public sealed class RespDataMA_MonsterStatusAilmentMaster : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_MonsterStatusAilmentMaster.StatusAilment[] monsterStatusAilmentM;

		[Serializable]
		public sealed class StatusAilment
		{
			public string assetCategoryId;

			public string assetNum;

			public string assetValue;

			public string monsterId;

			public string monsterStatusAilmentMaterialId;
		}
	}

	public sealed class RequestMonsterStatusAilmentGroupMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_MonsterStatusAilmentGroupMaster>
	{
		public RequestMonsterStatusAilmentGroupMaster()
		{
			this.apiId = "990121";
		}
	}

	[Serializable]
	public sealed class RespDataMA_MonsterStatusAilmentGroupMaster : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_MonsterStatusAilmentGroupMaster.StatusAilmentGroup[] monsterStatusAilmentMaterialM;

		[Serializable]
		public sealed class StatusAilmentGroup
		{
			public string monsterStatusAilmentMaterialId;

			public string materialAssetCategoryId1;

			public string materialAssetValue1;

			public string materialAssetNum1;

			public string materialAssetCategoryId2;

			public string materialAssetValue2;

			public string materialAssetNum2;

			public string materialAssetCategoryId3;

			public string materialAssetValue3;

			public string materialAssetNum3;

			public string materialAssetCategoryId4;

			public string materialAssetValue4;

			public string materialAssetNum4;

			[NonSerialized]
			public const int MATERIAL_ASSET_MAX = 4;

			public string GetAssetCategoryId(int num)
			{
				string empty = string.Empty;
				switch (num)
				{
				case 1:
					empty = this.materialAssetCategoryId1;
					break;
				case 2:
					empty = this.materialAssetCategoryId2;
					break;
				case 3:
					empty = this.materialAssetCategoryId3;
					break;
				case 4:
					empty = this.materialAssetCategoryId4;
					break;
				}
				return empty;
			}

			public string GetAssetValue(int num)
			{
				string empty = string.Empty;
				switch (num)
				{
				case 1:
					empty = this.materialAssetValue1;
					break;
				case 2:
					empty = this.materialAssetValue2;
					break;
				case 3:
					empty = this.materialAssetValue3;
					break;
				case 4:
					empty = this.materialAssetValue4;
					break;
				}
				return empty;
			}

			public string GetAssetNum(int num)
			{
				string empty = string.Empty;
				switch (num)
				{
				case 1:
					empty = this.materialAssetNum1;
					break;
				case 2:
					empty = this.materialAssetNum2;
					break;
				case 3:
					empty = this.materialAssetNum3;
					break;
				case 4:
					empty = this.materialAssetNum4;
					break;
				}
				return empty;
			}
		}
	}

	public sealed class RequestMA_MonsterAutoLoadChipMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.ResponseMonsterAutoLoadChipMaster>
	{
		public RequestMA_MonsterAutoLoadChipMaster()
		{
			this.apiId = "990124";
		}
	}

	[Serializable]
	public sealed class ResponseMonsterAutoLoadChipMaster : WebAPI.ResponseData
	{
		public GameWebAPI.ResponseMonsterAutoLoadChipMaster.AutoLoadChip[] monsterAutoLoadChipM;

		public GameWebAPI.ResponseMonsterAutoLoadChipMaster.AutoLoadChip[] GetMaster()
		{
			return this.monsterAutoLoadChipM;
		}

		[Serializable]
		public sealed class AutoLoadChip
		{
			public int monsterId;

			public int attachChipId;
		}
	}

	public sealed class RequestMA_SoulMaster : RequestTypeBase<GameWebAPI.RequestMA_SoulM, GameWebAPI.RespDataMA_GetSoulM>
	{
		public RequestMA_SoulMaster()
		{
			this.apiId = "990201";
		}
	}

	public sealed class RequestMA_SoulM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetSoulM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetSoulM.SoulM[] soulM;

		public bool IsVersionUpGroup(GameWebAPI.RespDataMA_GetSoulM.SoulM soulM)
		{
			return soulM.soulGroup == "3";
		}

		public bool IsVersionUpAlMighty(GameWebAPI.RespDataMA_GetSoulM.SoulM soulM)
		{
			return soulM.priority == "2";
		}

		public GameWebAPI.RespDataMA_GetSoulM.SoulM GetSoul(string soulId)
		{
			for (int i = 0; i < this.soulM.Length; i++)
			{
				if (this.soulM[i].soulId == soulId)
				{
					return this.soulM[i];
				}
			}
			return null;
		}

		[Serializable]
		public sealed class SoulM
		{
			public string soulId;

			public string soulName;

			public string description;

			public string soulGroup;

			public string rare;

			public string priority;
		}
	}

	public sealed class RequestMA_FacilityMaster : RequestTypeBase<GameWebAPI.RequestMA_FacilityM, GameWebAPI.RespDataMA_GetFacilityM>
	{
		public RequestMA_FacilityMaster()
		{
			this.apiId = "990301";
		}
	}

	public sealed class RequestMA_FacilityM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetFacilityM : WebAPI.ResponseData
	{
		public FacilityM[] facilityM;
	}

	public sealed class RequestMA_FacilityUpgradeMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetFacilityUpgradeM>
	{
		public RequestMA_FacilityUpgradeMaster()
		{
			this.apiId = "990302";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetFacilityUpgradeM : WebAPI.ResponseData
	{
		public FacilityUpgradeM[] facilityUpgradeM;
	}

	public sealed class RequestMA_FacilityMeatFieldMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetFacilityMeatFieldM>
	{
		public RequestMA_FacilityMeatFieldMaster()
		{
			this.apiId = "990303";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetFacilityMeatFieldM : WebAPI.ResponseData
	{
		public FacilityMeatFieldM[] facilityMeatFieldM;
	}

	public sealed class RequestMA_FacilityWarehouseMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetFacilityWarehouseM>
	{
		public RequestMA_FacilityWarehouseMaster()
		{
			this.apiId = "990304";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetFacilityWarehouseM : WebAPI.ResponseData
	{
		public FacilityWarehouseM[] facilityWarehouseM;
	}

	public sealed class RequestMA_FacilityExpUpMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetFacilityExpUpM>
	{
		public RequestMA_FacilityExpUpMaster()
		{
			this.apiId = "990305";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetFacilityExpUpM : WebAPI.ResponseData
	{
		public FacilityExpUpM[] facilityExpUpM;
	}

	public sealed class RequestMA_FacilityRestaurantMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetFacilityRestaurantM>
	{
		public RequestMA_FacilityRestaurantMaster()
		{
			this.apiId = "990306";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetFacilityRestaurantM : WebAPI.ResponseData
	{
		public FacilityRestaurantM[] facilityRestaurantM;
	}

	public sealed class RequestMA_FacilityHouseMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetFacilityHouseM>
	{
		public RequestMA_FacilityHouseMaster()
		{
			this.apiId = "990307";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetFacilityHouseM : WebAPI.ResponseData
	{
		public FacilityHouseM[] facilityHouseM;
	}

	public sealed class RequestMA_FacilityConditionMaster : RequestTypeBase<GameWebAPI.RequestMA_FacilityConditionM, GameWebAPI.RespDataMA_FacilityConditionM>
	{
		public RequestMA_FacilityConditionMaster()
		{
			this.apiId = "990308";
		}
	}

	public sealed class RequestMA_FacilityConditionM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_FacilityConditionM : WebAPI.ResponseData
	{
		public FacilityConditionM[] facilityConditionM;
	}

	public sealed class RequestMA_FacilityExtraEffectMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_FacilityExtraEffectM>
	{
		public RequestMA_FacilityExtraEffectMaster()
		{
			this.apiId = "990309";
		}
	}

	[Serializable]
	public sealed class RespDataMA_FacilityExtraEffectM : WebAPI.ResponseData
	{
		public FacilityExtraEffectM[] facilityExtraEffectM;
	}

	public sealed class RequestMA_FacilityExtraEffectLevelM : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_FacilityExtraEffectLevelM>
	{
		public RequestMA_FacilityExtraEffectLevelM()
		{
			this.apiId = "990310";
		}
	}

	[Serializable]
	public sealed class RespDataMA_FacilityExtraEffectLevelM : WebAPI.ResponseData
	{
		public FacilityExtraEffectLevelM[] facilityExtraEffectLevelM;
	}

	public sealed class RequestMA_FacilityChipMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetFacilityChipM>
	{
		public RequestMA_FacilityChipMaster()
		{
			this.apiId = "990311";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetFacilityChipM : WebAPI.ResponseData
	{
		public FacilityChipM[] facilityChipM;
	}

	public sealed class RequestMA_FacilityKeyMaster : RequestTypeBase<GameWebAPI.RequestMA_FacilityKeyM, GameWebAPI.RespDataMA_GetFacilityKeyM>
	{
		public RequestMA_FacilityKeyMaster()
		{
			this.apiId = "990312";
		}
	}

	public sealed class RequestMA_FacilityKeyM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetFacilityKeyM : WebAPI.ResponseData
	{
		public FacilityKeyM[] facilityKeyM;
	}

	public sealed class RequestMA_TipsMaster : RequestTypeBase<GameWebAPI.RequestMA_TipsM, GameWebAPI.RespDataMA_GetTipsM>
	{
		public RequestMA_TipsMaster()
		{
			this.apiId = "990501";
		}
	}

	public sealed class RequestMA_TipsM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetTipsM : WebAPI.ResponseData
	{
		public CMD_Tips.TipsM tipsM;

		public void Initialize()
		{
			Array.Sort<CMD_Tips.TipsM.TipsManage>(this.tipsM.tipsManage, new Comparison<CMD_Tips.TipsM.TipsManage>(this.SortFuncDispType));
			Array.Sort<CMD_Tips.TipsM.Tips>(this.tipsM.tips, new Comparison<CMD_Tips.TipsM.Tips>(this.SortFuncTipsId));
		}

		private int SortFuncDispType(CMD_Tips.TipsM.TipsManage a, CMD_Tips.TipsM.TipsManage b)
		{
			int num = int.Parse(a.dispType);
			int num2 = int.Parse(b.dispType);
			if (num > num2)
			{
				return 1;
			}
			if (num < num2)
			{
				return -1;
			}
			return 0;
		}

		private int SortFuncTipsId(CMD_Tips.TipsM.Tips a, CMD_Tips.TipsM.Tips b)
		{
			int num = int.Parse(a.tipsId);
			int num2 = int.Parse(b.tipsId);
			if (num > num2)
			{
				return 1;
			}
			if (num < num2)
			{
				return -1;
			}
			return 0;
		}
	}

	public sealed class RequestMA_BannerMaster : RequestTypeBase<GameWebAPI.RequestMA_BannerM, GameWebAPI.RespDataMA_BannerM>
	{
		public RequestMA_BannerMaster()
		{
			this.apiId = "990601";
		}
	}

	public sealed class RequestMA_BannerM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	public sealed class RespDataMA_BannerM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_BannerM.BannerM[] bannerM;

		public sealed class BannerM
		{
			public string bannerId;

			public string actionType;

			public string name;

			public string img;

			public string linkCategoryType;

			public string dispNum;

			public string startTime;

			public string endTime;

			public string url;
		}
	}

	public sealed class RequestMA_CodeMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_CodeM>
	{
		public RequestMA_CodeMaster()
		{
			this.apiId = "990701";
		}
	}

	[Serializable]
	public sealed class RespDataMA_CodeM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_CodeM.CodeM codeM;

		[Serializable]
		public sealed class CodeM
		{
			private int[] _evolveCoefficientRare = new int[6];

			private int[] _modeChangeCoefficientRare = new int[6];

			private int[] _chipExtendSlotNeeds = new int[]
			{
				1,
				2,
				4,
				8,
				10
			};

			private string gdprOptOutSiteUrl = string.Empty;

			public string maxChildMonster
			{
				set
				{
					this.MAX_CHILD_MONSTER = int.Parse(value);
				}
			}

			public string useStoneNumToRecover
			{
				set
				{
				}
			}

			public string recoverStaminaDigistoneNum
			{
				set
				{
					this.RECOVER_STAMINA_DIGISTONE_NUM = int.Parse(value);
				}
			}

			public string maxGoldMedalCount
			{
				set
				{
					this.MAX_GOLD_MEDAL_COUNT = int.Parse(value);
				}
			}

			public string maxClusterCount
			{
				set
				{
					this.MAX_CLUSTER_COUNT = int.Parse(value);
				}
			}

			public string maxDigistoneCount
			{
				set
				{
					this.MAX_DIGISTONE_COUNT = int.Parse(value);
				}
			}

			public string maxLinkPointCount
			{
				set
				{
					this.MAX_LINK_POINT_COUNT = int.Parse(value);
				}
			}

			public string rareStar
			{
				set
				{
					this.RARE_STAR = int.Parse(value);
				}
			}

			public string maxRare
			{
				set
				{
					this.MAX_RARE = int.Parse(value);
				}
			}

			public string maxBlockCount
			{
				set
				{
					this.MAX_BLOCK_COUNT = int.Parse(value);
				}
			}

			public string enableMonsterSpaceToexecGasha1
			{
				set
				{
					this.ENABLE_MONSTER_SPACE_TOEXEC_GASHA_1 = int.Parse(value);
				}
			}

			public string enableMonsterSpaceToexecGasha10
			{
				set
				{
					this.ENABLE_MONSTER_SPACE_TOEXEC_GASHA_10 = int.Parse(value);
				}
			}

			public string enableSpaceToexecDungeon
			{
				set
				{
					this.ENABLE_SPACE_TOEXEC_DUNGEON = int.Parse(value);
				}
			}

			public string rareGashaType
			{
				set
				{
					this.RARE_GASHA_TYPE = int.Parse(value);
				}
			}

			public string linkGashaType
			{
				set
				{
					this.LINK_GASHA_TYPE = int.Parse(value);
				}
			}

			public string evolveItemMons
			{
				set
				{
					this.EVOLVE_ITEM_MONS = int.Parse(value);
				}
			}

			public string evolveItemSoul
			{
				set
				{
					this.EVOLVE_ITEM_SOUL = int.Parse(value);
				}
			}

			public string expOfOneMeat
			{
				set
				{
					this.EXP_OF_ONE_MEAT = int.Parse(value);
				}
			}

			public string successionCoefficient
			{
				set
				{
					this.SUCCESSION_COEFFICIENT = int.Parse(value);
				}
			}

			public string successionBaseCoefficient
			{
				set
				{
					this.SUCCESSION_BASE_COEFFICIENT = int.Parse(value);
				}
			}

			public string successionPartnerCoefficient
			{
				set
				{
					this.SUCCESSION_PARTNER_COEFFICIENT = int.Parse(value);
				}
			}

			public string laboratoryBasePlusCoefficient
			{
				set
				{
					this.LABORATORY_BASE_PLUS_COEFFICIENT = int.Parse(value);
				}
			}

			public string laboratoryPartnerPlusCoefficient
			{
				set
				{
					this.LABORATORY_PARTNER_PLUS_COEFFICIENT = int.Parse(value);
				}
			}

			public string laboratoryBaseCoefficient
			{
				set
				{
					this.LABORATORY_BASE_COEFFICIENT = int.Parse(value);
				}
			}

			public string laboratoryPartnerCoefficient
			{
				set
				{
					this.LABORATORY_PARTNER_COEFFICIENT = int.Parse(value);
				}
			}

			public string reinforcementCoefficient
			{
				set
				{
					this.REINFORCEMENT_COEFFICIENT = double.Parse(value);
				}
			}

			public string evolveCoefficientFor5
			{
				set
				{
					this.EVOLVE_COEFFICIENT_FOR_5 = int.Parse(value);
				}
			}

			public string evolveCoefficientFor6
			{
				set
				{
					this.EVOLVE_COEFFICIENT_FOR_6 = int.Parse(value);
				}
			}

			public string evolveCoefficientFor7
			{
				set
				{
					this.EVOLVE_COEFFICIENT_FOR_7 = int.Parse(value);
				}
			}

			public string evolveCoefficientRare1
			{
				set
				{
					this.EVOLVE_COEFFICIENT_RARE[0] = int.Parse(value);
				}
			}

			public string evolveCoefficientRare2
			{
				set
				{
					this.EVOLVE_COEFFICIENT_RARE[1] = int.Parse(value);
				}
			}

			public string evolveCoefficientRare3
			{
				set
				{
					this.EVOLVE_COEFFICIENT_RARE[2] = int.Parse(value);
				}
			}

			public string evolveCoefficientRare4
			{
				set
				{
					this.EVOLVE_COEFFICIENT_RARE[3] = int.Parse(value);
				}
			}

			public string evolveCoefficientRare5
			{
				set
				{
					this.EVOLVE_COEFFICIENT_RARE[4] = int.Parse(value);
				}
			}

			public string evolveCoefficientRare6
			{
				set
				{
					this.EVOLVE_COEFFICIENT_RARE[5] = int.Parse(value);
				}
			}

			public string modeChangeCoefficientFor5
			{
				set
				{
					this.MODE_CHANGE_COEFFICIENT_FOR_5 = int.Parse(value);
				}
			}

			public string modeChangeCoefficientFor6
			{
				set
				{
					this.MODE_CHANGE_COEFFICIENT_FOR_6 = int.Parse(value);
				}
			}

			public string modeChangeCoefficientFor7
			{
				set
				{
					this.MODE_CHANGE_COEFFICIENT_FOR_7 = int.Parse(value);
				}
			}

			public string modeChangeCoefficientRare1
			{
				set
				{
					this.MODE_CHANGE_COEFFICIENT_RARE[0] = int.Parse(value);
				}
			}

			public string modeChangeCoefficientRare2
			{
				set
				{
					this.MODE_CHANGE_COEFFICIENT_RARE[1] = int.Parse(value);
				}
			}

			public string modeChangeCoefficientRare3
			{
				set
				{
					this.MODE_CHANGE_COEFFICIENT_RARE[2] = int.Parse(value);
				}
			}

			public string modeChangeCoefficientRare4
			{
				set
				{
					this.MODE_CHANGE_COEFFICIENT_RARE[3] = int.Parse(value);
				}
			}

			public string modeChangeCoefficientRare5
			{
				set
				{
					this.MODE_CHANGE_COEFFICIENT_RARE[4] = int.Parse(value);
				}
			}

			public string modeChangeCoefficientRare6
			{
				set
				{
					this.MODE_CHANGE_COEFFICIENT_RARE[5] = int.Parse(value);
				}
			}

			public string questAreaIdDefault
			{
				set
				{
					this.QUEST_AREA_ID_DEFAULT = value;
				}
			}

			public string questAreaIdEvent
			{
				set
				{
					this.QUEST_AREA_ID_EVENT = value;
				}
			}

			public string questAreaIdAdevent
			{
				set
				{
					this.QUEST_AREA_ID_ADVENT = value;
				}
			}

			public string firstClearNum
			{
				set
				{
					this.FIRST_CLEAR_NUM = int.Parse(value);
				}
			}

			public string tcpPongEndCount
			{
				set
				{
					this.TCP_PONG_END_COUNT = int.Parse(value);
				}
			}

			public string tcpPongIntervalCount
			{
				set
				{
					this.TCP_PONG_INTERVAL_TIME = int.Parse(value);
				}
			}

			public string buildingTypeFacility
			{
				set
				{
					this.BUILDING_TYPE_FACILITY = value;
				}
			}

			public string buildingTypeDecoration
			{
				set
				{
					this.BUILDING_TYPE_DECORATION = value;
				}
			}

			public string reviewDeleteDateTime
			{
				set
				{
					this.REVIEW_DELETE_DATETIME = value;
					LeadReview leadReview = new LeadReview();
					leadReview.DeletePrefs(value);
				}
			}

			public string iosContactSiteURL
			{
				set
				{
					this.IOS_CONTACT_SITE_URL = value;
					PlayerPrefs.SetString("iosContactSiteURL", this.IOS_CONTACT_SITE_URL);
				}
			}

			public string androidContactSiteURL
			{
				set
				{
					this.ANDROID_CONTACT_SITE_URL = value;
					PlayerPrefs.SetString("androidContactSiteURL", this.ANDROID_CONTACT_SITE_URL);
				}
			}

			public string officialSiteURL
			{
				set
				{
					this.OFFICIAL_SITE_URL = value;
				}
			}

			public string riseConditionFriendShipStatus
			{
				set
				{
					this.RIZE_CONDITION_FRENDSHIPSTATUS = int.Parse(value);
				}
			}

			public string gashaPriceTypeDigiStone
			{
				set
				{
					this.GASHA_PRICE_TYPE_DIGISTONE = value;
				}
			}

			public string gashaPriceTypeLinkPoint
			{
				set
				{
					this.GASHA_PRICE_TYPE_LINKPOINT = value;
				}
			}

			public string gashaPriceTypeDigiStoneChip
			{
				set
				{
					this.GASHA_PRICE_TYPE_DIGISTONE_CHIP = value;
				}
			}

			public string gashaPriceTypeLinkPointChip
			{
				set
				{
					this.GASHA_PRICE_TYPE_LINKPOINT_CHIP = value;
				}
			}

			public string gashaPriceTypeDigiStoneTicket
			{
				set
				{
					this.GASHA_PRICE_TYPE_DIGISTONE_TICKET = value;
				}
			}

			public string gashaPriceTypeLinkPointTicket
			{
				set
				{
					this.GASHA_PRICE_TYPE_LINKPOINT_TICKET = value;
				}
			}

			public string gashaInharitancePrizeLevel
			{
				set
				{
					this.GASHA_INHARITANCE_PRIZE_LEVEL = int.Parse(value);
				}
			}

			public string gashaLeaderskillPrizeLevel
			{
				set
				{
					this.GASHA_LEADERSKILL_PRIZE_LEVEL = int.Parse(value);
				}
			}

			public string tutorialGashaMainBannerPath
			{
				set
				{
					this.TUTORIAL_GASHA_MAIN_BANNER_PATH = value;
				}
			}

			public string tutorialGashaSubBanner1Path
			{
				set
				{
					this.TUTORIAL_GASHA_SUB_BANNER1_PATH = value;
				}
			}

			public string tutorialGashaSubBanner2Path
			{
				set
				{
					this.TUTORIAL_GASHA_SUB_BANNER2_PATH = value;
				}
			}

			public string tutorialGashaSubBanner3Path
			{
				set
				{
					this.TUTORIAL_GASHA_SUB_BANNER3_PATH = value;
				}
			}

			public string sendAndroidPush
			{
				set
				{
					this.SEND_ANDROID_PUSH = int.Parse(value);
				}
			}

			public string iosStoreSiteURL
			{
				set
				{
					this.IOS_STORE_SITE_URL = value;
				}
			}

			public string androidStoreSiteURL
			{
				set
				{
					this.ANDROID_STORE_SITE_URL = value;
				}
			}

			public string cacheTextureCount
			{
				set
				{
					this.CACHE_TEXTURE_COUNT = int.Parse(value);
				}
			}

			public string isChatOpen
			{
				set
				{
					this.IS_CHAT_OPEN = int.Parse(value);
				}
			}

			public string chatGroupJoinMaxNum
			{
				set
				{
					this.CHAT_GROUP_JOIN_MAX_NUM = int.Parse(value);
				}
			}

			public string chatGroupMemberMaxNum
			{
				set
				{
					this.CHAT_GROUP_MEMBER_MAX_NUM = int.Parse(value);
				}
			}

			public string chatlogViewListInitNum
			{
				set
				{
					this.CHATLOG_VIEW_LIST_INIT_NUM = int.Parse(value);
				}
			}

			public string chatlogViewListMaxNum
			{
				set
				{
					this.CHATLOG_VIEW_LIST_MAX_NUM = int.Parse(value);
				}
			}

			public string chatNotificationIntervalTime
			{
				set
				{
					this.CHAT_NOTIFICATION_INTERVAL_TIME = value;
				}
			}

			public string meatShortCutDigistoneNum
			{
				set
				{
					this.MEAT_SHORTCUT_DEGISTONE_NUM = int.Parse(value);
				}
			}

			public string pvpMaxRank
			{
				set
				{
					this.PVP_MAX_RANK = int.Parse(value);
				}
			}

			public string pvpMaxAttackTime
			{
				set
				{
					this.PVP_MAX_ATTACK_TIME = int.Parse(value);
				}
			}

			public string pvpHurryUpAttackTime
			{
				set
				{
					this.PVP_HURRYUP_ATTACK_TIME = int.Parse(value);
				}
			}

			public string pvpMaxRoundNum
			{
				set
				{
					this.PVP_MAX_ROUND_NUM = int.Parse(value);
				}
			}

			public string pvpBattleTimeoutTime
			{
				set
				{
					this.PVP_BATTLE_TIMEOUT_TIME = int.Parse(value);
				}
			}

			public string pvpBattleEnemyRecoverTime
			{
				set
				{
					this.PVP_BATTLE_ENEMY_RECOVER_TIME = int.Parse(value);
				}
			}

			public string pvpBattleActionLog
			{
				set
				{
					this.PVP_BATTLE_ACTION_LOG = int.Parse(value);
				}
			}

			public string pvpActionLogRetryTime
			{
				set
				{
					this.PVP_ACTION_LOG_RETRY_TIME = int.Parse(value);
				}
			}

			public string pvpActionLogRetryCount
			{
				set
				{
					this.PVP_ACTION_LOG_RETRY_COUNT = int.Parse(value);
				}
			}

			public string pvpPartySelectTime
			{
				set
				{
					this.PVP_PARTY_SELECT_TIME = int.Parse(value);
				}
			}

			public string pvpSelectDataRetryCount
			{
				set
				{
					this.PVP_SELECT_DATA_RETRY_COUNT = int.Parse(value);
				}
			}

			public string pvpSelectDataRetryTime
			{
				set
				{
					this.PVP_SELECT_DATA_RETRY_TIME = int.Parse(value);
				}
			}

			public string multiMaxAttackTime
			{
				set
				{
					this.MULTI_MAX_ATTACK_TIME = int.Parse(value);
				}
			}

			public string multiHurryUpAttackTime
			{
				set
				{
					this.MULTI_HURRYUP_ATTACK_TIME = int.Parse(value);
				}
			}

			public string multiBattleTimeoutTime
			{
				set
				{
					this.MULTI_BATTLE_TIMEOUT_TIME = int.Parse(value);
				}
			}

			public string chipExtendSlotNeeds1
			{
				set
				{
					this.CHIP_EXTEND_SLOT_NEEDS[0] = int.Parse(value);
				}
			}

			public string chipExtendSlotNeeds2
			{
				set
				{
					this.CHIP_EXTEND_SLOT_NEEDS[1] = int.Parse(value);
				}
			}

			public string chipExtendSlotNeeds3
			{
				set
				{
					this.CHIP_EXTEND_SLOT_NEEDS[2] = int.Parse(value);
				}
			}

			public string chipExtendSlotNeeds4
			{
				set
				{
					this.CHIP_EXTEND_SLOT_NEEDS[3] = int.Parse(value);
				}
			}

			public string chipExtendSlotNeeds5
			{
				set
				{
					this.CHIP_EXTEND_SLOT_NEEDS[4] = int.Parse(value);
				}
			}

			public string eventQuestOpenTime
			{
				set
				{
					this.EVENT_QUEST_OPEN_TIME = int.Parse(value);
				}
			}

			public string reviewStopFlag
			{
				set
				{
					this.REVIEW_STOP_FLAG = value;
				}
			}

			public string baseChipSellPrice
			{
				set
				{
					this.BASE_CHIP_SELL_PRICE = int.Parse(value);
				}
			}

			public string maxChipCount
			{
				set
				{
					this.MAX_CHIP_COUNT = int.Parse(value);
				}
			}

			public string buyHqMeatDigistoneNum
			{
				set
				{
					this.BUY_HQMEAT_DIGISTONE_NUM = int.Parse(value);
				}
			}

			public string abilityUpgradeMulRateGrowing
			{
				set
				{
					this.ABILITY_UPGRADE_MULRATE_GROWING = float.Parse(value);
				}
			}

			public string abilityUpgradeMulRateRipe
			{
				set
				{
					this.ABILITY_UPGRADE_MULRATE_RIPE = float.Parse(value);
				}
			}

			public string abilityUpgradeMulRatePerfect
			{
				set
				{
					this.ABILITY_UPGRADE_MULRATE_PERFECT = float.Parse(value);
				}
			}

			public string abilityUpgradeMulRateUltimate
			{
				set
				{
					this.ABILITY_UPGRADE_MULRATE_ULTIMATE = float.Parse(value);
				}
			}

			public string abilityInheritRateGrowing
			{
				set
				{
					this.ABILITY_INHERITRATE_GROWING = int.Parse(value);
				}
			}

			public string abilityInheritRateRipe
			{
				set
				{
					this.ABILITY_INHERITRATE_RIPE = int.Parse(value);
				}
			}

			public string abilityInheritRatePerfect
			{
				set
				{
					this.ABILITY_INHERITRATE_PERFECT = int.Parse(value);
				}
			}

			public string abilityInheritRateUltimate
			{
				set
				{
					this.ABILITY_INHERITRATE_ULTIMATE = int.Parse(value);
				}
			}

			public string usdigimonAgreeAdress
			{
				set
				{
					this.EXT_ADR_AGREE = value;
				}
			}

			public string usdigimonPrivacyPolicyAdress
			{
				set
				{
					this.EXT_ADR_PRIVACY_POLICY = value;
				}
			}

			public string gdprOptOutSiteURL
			{
				set
				{
					this.GDPR_OPT_OUT_SITE_URL = value;
				}
			}

			public int MAX_CHILD_MONSTER { get; private set; }

			public int RECOVER_STAMINA_DIGISTONE_NUM { get; private set; }

			public int MAX_GOLD_MEDAL_COUNT { get; private set; }

			public int MAX_CLUSTER_COUNT { get; private set; }

			public int MAX_DIGISTONE_COUNT { get; private set; }

			public int MAX_LINK_POINT_COUNT { get; private set; }

			public int RARE_STAR { get; private set; }

			public int MAX_RARE { get; private set; }

			public int MAX_BLOCK_COUNT { get; private set; }

			public int ENABLE_MONSTER_SPACE_TOEXEC_GASHA_1 { get; private set; }

			public int ENABLE_MONSTER_SPACE_TOEXEC_GASHA_10 { get; private set; }

			public int ENABLE_SPACE_TOEXEC_DUNGEON { get; private set; }

			public int RARE_GASHA_TYPE { get; private set; }

			public int LINK_GASHA_TYPE { get; private set; }

			public int EVOLVE_ITEM_MONS { get; private set; }

			public int EVOLVE_ITEM_SOUL { get; private set; }

			public int EXP_OF_ONE_MEAT { get; private set; }

			public int SUCCESSION_COEFFICIENT { get; private set; }

			public int SUCCESSION_BASE_COEFFICIENT { get; private set; }

			public int SUCCESSION_PARTNER_COEFFICIENT { get; private set; }

			public int LABORATORY_BASE_PLUS_COEFFICIENT { get; private set; }

			public int LABORATORY_PARTNER_PLUS_COEFFICIENT { get; private set; }

			public int LABORATORY_BASE_COEFFICIENT { get; private set; }

			public int LABORATORY_PARTNER_COEFFICIENT { get; private set; }

			public double REINFORCEMENT_COEFFICIENT { get; private set; }

			public int MEAT_SHORTCUT_DEGISTONE_NUM { get; private set; }

			public int EVOLVE_COEFFICIENT_FOR_5 { get; private set; }

			public int EVOLVE_COEFFICIENT_FOR_6 { get; private set; }

			public int EVOLVE_COEFFICIENT_FOR_7 { get; private set; }

			public int[] EVOLVE_COEFFICIENT_RARE
			{
				get
				{
					return this._evolveCoefficientRare;
				}
				set
				{
					this._evolveCoefficientRare = value;
				}
			}

			public int MODE_CHANGE_COEFFICIENT_FOR_5 { get; private set; }

			public int MODE_CHANGE_COEFFICIENT_FOR_6 { get; private set; }

			public int MODE_CHANGE_COEFFICIENT_FOR_7 { get; private set; }

			public int[] MODE_CHANGE_COEFFICIENT_RARE
			{
				get
				{
					return this._modeChangeCoefficientRare;
				}
				set
				{
					this._modeChangeCoefficientRare = value;
				}
			}

			public string QUEST_AREA_ID_DEFAULT { get; private set; }

			public string QUEST_AREA_ID_EVENT { get; private set; }

			public string QUEST_AREA_ID_ADVENT { get; private set; }

			public int FIRST_CLEAR_NUM { get; private set; }

			public int TCP_PONG_END_COUNT { get; private set; }

			public int TCP_PONG_INTERVAL_TIME { get; private set; }

			public string BUILDING_TYPE_FACILITY { get; private set; }

			public string BUILDING_TYPE_DECORATION { get; private set; }

			public string REVIEW_DELETE_DATETIME { get; private set; }

			public string IOS_CONTACT_SITE_URL { get; private set; }

			public string ANDROID_CONTACT_SITE_URL { get; private set; }

			public string OFFICIAL_SITE_URL { get; private set; }

			public int RIZE_CONDITION_FRENDSHIPSTATUS { get; private set; }

			public string GASHA_PRICE_TYPE_DIGISTONE { get; private set; }

			public string GASHA_PRICE_TYPE_LINKPOINT { get; private set; }

			public string GASHA_PRICE_TYPE_DIGISTONE_CHIP { get; private set; }

			public string GASHA_PRICE_TYPE_LINKPOINT_CHIP { get; private set; }

			public string GASHA_PRICE_TYPE_DIGISTONE_TICKET { get; private set; }

			public string GASHA_PRICE_TYPE_LINKPOINT_TICKET { get; private set; }

			public int GASHA_INHARITANCE_PRIZE_LEVEL { get; private set; }

			public int GASHA_LEADERSKILL_PRIZE_LEVEL { get; private set; }

			public string TUTORIAL_GASHA_MAIN_BANNER_PATH { get; private set; }

			public string TUTORIAL_GASHA_SUB_BANNER1_PATH { get; private set; }

			public string TUTORIAL_GASHA_SUB_BANNER2_PATH { get; private set; }

			public string TUTORIAL_GASHA_SUB_BANNER3_PATH { get; private set; }

			public int SEND_ANDROID_PUSH { get; private set; }

			public string IOS_STORE_SITE_URL { get; private set; }

			public string ANDROID_STORE_SITE_URL { get; private set; }

			public int CACHE_TEXTURE_COUNT { get; private set; }

			public int IS_CHAT_OPEN { get; private set; }

			public int CHAT_GROUP_JOIN_MAX_NUM { get; private set; }

			public int CHAT_GROUP_MEMBER_MAX_NUM { get; private set; }

			public int CHATLOG_VIEW_LIST_INIT_NUM { get; private set; }

			public int CHATLOG_VIEW_LIST_MAX_NUM { get; private set; }

			public string CHAT_NOTIFICATION_INTERVAL_TIME { get; private set; }

			public int PVP_MAX_RANK { get; private set; }

			public int PVP_MAX_ATTACK_TIME { get; private set; }

			public int PVP_HURRYUP_ATTACK_TIME { get; private set; }

			public int PVP_MAX_ROUND_NUM { get; private set; }

			public int PVP_BATTLE_TIMEOUT_TIME { get; private set; }

			public int PVP_BATTLE_ENEMY_RECOVER_TIME { get; private set; }

			public int PVP_BATTLE_ACTION_LOG { get; private set; }

			public int PVP_ACTION_LOG_RETRY_TIME { get; private set; }

			public int PVP_ACTION_LOG_RETRY_COUNT { get; private set; }

			public int PVP_SELECT_DATA_RETRY_TIME { get; private set; }

			public int PVP_SELECT_DATA_RETRY_COUNT { get; private set; }

			public int PVP_PARTY_SELECT_TIME { get; private set; }

			public int MULTI_MAX_ATTACK_TIME { get; private set; }

			public int MULTI_HURRYUP_ATTACK_TIME { get; private set; }

			public int MULTI_BATTLE_TIMEOUT_TIME { get; private set; }

			public int[] CHIP_EXTEND_SLOT_NEEDS
			{
				get
				{
					return this._chipExtendSlotNeeds;
				}
				set
				{
					this._chipExtendSlotNeeds = value;
				}
			}

			public int EVENT_QUEST_OPEN_TIME { get; private set; }

			public string REVIEW_STOP_FLAG { get; private set; }

			public int BASE_CHIP_SELL_PRICE { get; private set; }

			public int MAX_CHIP_COUNT { get; private set; }

			public int BUY_HQMEAT_DIGISTONE_NUM { get; private set; }

			public float ABILITY_UPGRADE_MULRATE_GROWING { get; private set; }

			public float ABILITY_UPGRADE_MULRATE_RIPE { get; private set; }

			public float ABILITY_UPGRADE_MULRATE_PERFECT { get; private set; }

			public float ABILITY_UPGRADE_MULRATE_ULTIMATE { get; private set; }

			public int ABILITY_INHERITRATE_GROWING { get; private set; }

			public int ABILITY_INHERITRATE_RIPE { get; private set; }

			public int ABILITY_INHERITRATE_PERFECT { get; private set; }

			public int ABILITY_INHERITRATE_ULTIMATE { get; private set; }

			public string EXT_ADR_AGREE { get; private set; }

			public string EXT_ADR_PRIVACY_POLICY { get; private set; }

			public string GDPR_OPT_OUT_SITE_URL
			{
				get
				{
					return (!string.IsNullOrEmpty(this.gdprOptOutSiteUrl)) ? this.gdprOptOutSiteUrl : "https://optout.channel.or.jp";
				}
				private set
				{
					this.gdprOptOutSiteUrl = value;
				}
			}
		}
	}

	public sealed class RequestMA_MessageMaster : RequestTypeBase<GameWebAPI.RequestMA_MessageM, GameWebAPI.RespDataMA_MessageM>
	{
		public RequestMA_MessageMaster()
		{
			this.apiId = "990801";
		}
	}

	public sealed class RequestMA_MessageM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_MessageM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_MessageM.MessageM[] messageM;

		[Serializable]
		public sealed class MessageM
		{
			public string messageCode;

			public string messageTitle;

			public string messageText;

			public string actionType;

			public string actionValue;

			public string isResources;
		}
	}

	public sealed class RequestMA_WorldAreaMaster : RequestTypeBase<GameWebAPI.RequestMA_WorldAreaM, GameWebAPI.RespDataMA_GetWorldAreaM>
	{
		public RequestMA_WorldAreaMaster()
		{
			this.apiId = "990903";
		}
	}

	public sealed class RequestMA_WorldAreaM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetWorldAreaM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetWorldAreaM.WorldAreaM[] worldAreaM;

		[Serializable]
		public sealed class WorldAreaM
		{
			public string worldAreaId;

			public string worldId;

			public string priority;

			public string name;

			public string description;

			public string type;

			public string prerequisiteWorldAreaId;

			public string openTime;

			public string closeTime;

			public string img;

			public string backgroundImg;
		}
	}

	public sealed class RequestMA_WorldStageMaster : RequestTypeBase<GameWebAPI.RequestMA_WorldStageM, GameWebAPI.RespDataMA_GetWorldStageM>
	{
		public RequestMA_WorldStageMaster()
		{
			this.apiId = "990904";
		}
	}

	public sealed class RequestMA_WorldStageM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetWorldStageM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM;

		[Serializable]
		public sealed class WorldStageM
		{
			public string worldStageId;

			public string worldAreaId;

			public string priority;

			public string name;

			public string description;

			public string type;

			public string prerequisiteWorldStageId;

			public string stageImage;

			public string openTime;

			public string closeTime;

			public string adminOpenFlg;
		}
	}

	public sealed class RequestMA_WorldDungeonMaster : RequestTypeBase<GameWebAPI.RequestMA_WorldDungeonM, GameWebAPI.RespDataMA_GetWorldDungeonM>
	{
		public RequestMA_WorldDungeonMaster()
		{
			this.apiId = "990905";
		}
	}

	public sealed class RequestMA_WorldDungeonM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetWorldDungeonM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM[] worldDungeonM;

		[Serializable]
		public sealed class WorldDungeonM
		{
			public string worldDungeonId;

			public string worldStageId;

			public string priority;

			public string name;

			public string description;

			public string level;

			public string type;

			public string prerequisiteWorldDungeonId;

			public string needStamina;

			public string battleNum;

			public string attribute;

			public string linkPoint;

			public string entryNumLimit;

			public string entryPhaseLimit;

			public string clearRoundLimit;

			public string dungeonEffect1;

			public string dungeonEffect2;

			public string dungeonEffect3;

			public string dungeonEffect4;

			public string dungeonEffect5;

			public string background;

			public string sky;

			public string timeZone;

			public string weather;

			public string cloud;

			public string flare;

			public string environmentSound;

			public string bgm;

			public string bgmIntroFlg;

			public string bossBgm;

			public string bossBgmIntroFlg;

			public string exBossBgm;

			public string exBossBgmIntroFlg;

			public string exBossBgmCondition;

			public string canContinue;

			public string roundFlg;

			public string multiFlg;

			public string limitRound;

			public string optionRewardFlg;

			public bool IsSoloMulti()
			{
				return "0" == this.multiFlg;
			}

			public bool IsMultiOnly()
			{
				return "1" == this.multiFlg;
			}

			public bool IsSoloOnly()
			{
				return "2" == this.multiFlg;
			}
		}
	}

	public sealed class RequestMA_WorldDungeonStartCondition : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_WorldDungeonStartCondition>
	{
		public RequestMA_WorldDungeonStartCondition()
		{
			this.apiId = "991405";
		}
	}

	[Serializable]
	public sealed class RespDataMA_WorldDungeonStartCondition : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_WorldDungeonStartCondition.WorldDungeonStartConditionM[] worldDungeonStartConditionM;

		[Serializable]
		public class WorldDungeonStartConditionM
		{
			public string worldDungeonId;

			public string expireTime;

			public string preDungeonId1;

			public string preDungeonCount1;

			public string preDungeonId2;

			public string preDungeonCount2;

			public string preDungeonId3;

			public string preDungeonCount3;
		}
	}

	public sealed class RequestMA_WorldDungeonExtraEffectMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM>
	{
		public RequestMA_WorldDungeonExtraEffectMaster()
		{
			this.apiId = "990906";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetWorldDungeonExtraEffectM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM[] worldDungeonExtraEffectM;

		[Serializable]
		public sealed class WorldDungeonExtraEffectM
		{
			public string worldDungeonExtraEffectId;

			public string targetType;

			public string targetSubType;

			public string targetValue;

			public string targetValue2;

			public string effectType;

			public string effectSubType;

			public string effectValue;

			public string effectTrigger;

			public string effectTriggerValue;

			public string effectTurnType;

			public string effectTurn;

			public string effectCount;

			public string detail;
		}
	}

	public sealed class RequestMA_WorldDungeonExtraEffectManageMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectManageM>
	{
		public RequestMA_WorldDungeonExtraEffectManageMaster()
		{
			this.apiId = "990907";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetWorldDungeonExtraEffectManageM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectManageM.WorldDungeonExtraEffectManageM[] worldDungeonExtraEffectManageM;

		[Serializable]
		public sealed class WorldDungeonExtraEffectManageM
		{
			public string worldDungeonExtraEffectManageId;

			public string worldDungeonId;

			public string worldDungeonExtraEffectId;
		}
	}

	public sealed class RequestMA_WorldStageForceOpenMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.ResponseWorldStageForceOpenMaster>
	{
		public RequestMA_WorldStageForceOpenMaster()
		{
			this.apiId = "990908";
		}
	}

	[Serializable]
	public sealed class ResponseWorldStageForceOpenMaster : WebAPI.ResponseData
	{
		public GameWebAPI.ResponseWorldStageForceOpenMaster.ForceOpen[] worldStageForceOpenM;

		[Serializable]
		public sealed class ForceOpen
		{
			public int worldStageId;

			public int forceOpenMinute;

			public int assetCategoryId;

			public int assetValue;

			public int assetNum;
		}
	}

	public sealed class RequestMA_WorldDungeonRewardMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetWorldDungeonRewardM>
	{
		public RequestMA_WorldDungeonRewardMaster()
		{
			this.apiId = "990909";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetWorldDungeonRewardM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetWorldDungeonRewardM.WorldDungeonReward[] worldDungeonRewardM;

		[Serializable]
		public sealed class WorldDungeonReward
		{
			public string worldDungeonRewardId;

			public string worldDungeonId;

			public string assetCategoryId;

			public string assetValue;

			public string assetNum;

			public string exValue;

			public string everyTimeFlg;
		}
	}

	public sealed class RequestMA_WorldDungeonOptionRewardMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_GetWorldDungeonOptionRewardM>
	{
		public RequestMA_WorldDungeonOptionRewardMaster()
		{
			this.apiId = "990910";
		}
	}

	[Serializable]
	public sealed class RespDataMA_GetWorldDungeonOptionRewardM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetWorldDungeonOptionRewardM.WorldDungeonOptionReward[] worldDungeonOptionRewardM;

		[Serializable]
		public sealed class WorldDungeonOptionReward
		{
			public string worldDungeonOptionRewardId;

			public string worldDungeonId;

			public string type;

			public string subType;

			public string clearType;

			public string clearValue;

			public string assetCategoryId;

			public string assetValue;

			public string assetNum;

			public string exValue;

			public string joinType;
		}
	}

	public sealed class RequestMA_WorldDungeonSortieLimit : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_WorldDungeonSortieLimit>
	{
		public RequestMA_WorldDungeonSortieLimit()
		{
			this.apiId = "990911";
		}
	}

	[Serializable]
	public sealed class RespDataMA_WorldDungeonSortieLimit : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit[] worldDungeonSortieLimitM;

		[Serializable]
		public sealed class WorldDungeonSortieLimit
		{
			public string worldDungeonId;

			public string tribe;

			public string growStep;

			public string startDate;

			public string endDate;
		}
	}

	public sealed class RequestMA_HelpCategoryMaster : RequestTypeBase<GameWebAPI.RequestMA_HelpCategoryM, GameWebAPI.RespDataMA_GetHelpCategoryM>
	{
		public RequestMA_HelpCategoryMaster()
		{
			this.apiId = "991001";
		}
	}

	public sealed class RequestMA_HelpCategoryM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetHelpCategoryM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetHelpCategoryM.HelpCategoryM[] helpCategoryM;

		[Serializable]
		public sealed class HelpCategoryM
		{
			public string helpCategoryId;

			public string helpCategoryName;

			public string helpCategoryPriority;
		}
	}

	public sealed class RequestMA_HelpMaster : RequestTypeBase<GameWebAPI.RequestMA_HelpM, GameWebAPI.RespDataMA_GetHelpM>
	{
		public RequestMA_HelpMaster()
		{
			this.apiId = "991002";
		}
	}

	public sealed class RequestMA_HelpM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetHelpM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetHelpM.HelpM[] helpM;

		[Serializable]
		public sealed class HelpM
		{
			public string helpId;

			public string helpCategoryId;

			public string helpTitle;

			public string detail;

			public string tplPath;

			public string helpPriority;

			public string afterPath;
		}
	}

	public sealed class RequestMA_ItemMaster : RequestTypeBase<GameWebAPI.RequestMA_ItemM, GameWebAPI.RespDataMA_GetItemM>
	{
		public RequestMA_ItemMaster()
		{
			this.apiId = "991101";
		}
	}

	public sealed class RequestMA_ItemM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_GetItemM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_GetItemM.ItemM[] itemM;

		public GameWebAPI.RespDataMA_GetItemM.ItemM GetItemM(string itemId)
		{
			for (int i = 0; i < this.itemM.Length; i++)
			{
				if (this.itemM[i].itemId == itemId)
				{
					return this.itemM[i];
				}
			}
			return null;
		}

		[Serializable]
		public sealed class ItemM
		{
			public string itemId;

			public string name;

			public string description;

			public string category;

			public string[] img;

			public string unitName;

			public string upValue;

			public string dispFlg;

			public string GetSmallImagePath()
			{
				if (this.img.Length == 1 || string.IsNullOrEmpty(this.img[1]))
				{
					return this.img[0];
				}
				return this.img[1];
			}

			public string GetLargeImagePath()
			{
				return this.img[0];
			}

			public enum ImagePathType
			{
				LARGE_IMAGE,
				SMALL_IMAGE
			}
		}

		public enum ItemIdList
		{
			COLOSSEUM_BATTLE_POINT = 2,
			CHIP_SLOT_EXTENDER = 6,
			CHIP_REMOVER,
			HQ_MEAT = 50001
		}
	}

	public sealed class RequestMA_ColosseumMaster : RequestTypeBase<GameWebAPI.RequestMA_ColosseumM, GameWebAPI.RespDataMA_ColosseumM>
	{
		public RequestMA_ColosseumMaster()
		{
			this.apiId = "991201";
		}
	}

	public sealed class RequestMA_ColosseumM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_ColosseumM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_ColosseumM.Colosseum[] colosseumM;

		[Serializable]
		public class Colosseum
		{
			public string colosseumId;

			public string title;

			public string openStatus;

			public string prevColosseumId;

			public string worldDungeonId;

			public string openTime;

			public string startDate;

			public string endDate;

			public string closeTime;

			public string extraRewardRate;

			public string rewardGroupId;

			public string colosseumEventId;
		}
	}

	public sealed class RequestMA_ColosseumTimeScheduleMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_ColosseumTimeScheduleM>
	{
		public RequestMA_ColosseumTimeScheduleMaster()
		{
			this.apiId = "991202";
		}
	}

	[Serializable]
	public sealed class RespDataMA_ColosseumTimeScheduleM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule[] colosseumTimeScheduleM;

		[Serializable]
		public class ColosseumTimeSchedule
		{
			public string colosseumTimeScheduleId;

			public string colosseumId;

			public string startHour;

			public string endHour;
		}
	}

	public sealed class RequestMA_ColosseumRankMaster : RequestTypeBase<GameWebAPI.RequestMA_ColosseumRankM, GameWebAPI.RespDataMA_ColosseumRankM>
	{
		public RequestMA_ColosseumRankMaster()
		{
			this.apiId = "991205";
		}
	}

	public sealed class RequestMA_ColosseumRankM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_ColosseumRankM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_ColosseumRankM.ColosseumRank[] colosseumRankM;

		[Serializable]
		public class ColosseumRank
		{
			public string colosseumRankId;

			public string rankOrder;

			public string title;

			public string minScore;

			public string maxScore;

			public string nextRankId;

			public string prevRankId;
		}
	}

	public sealed class RequestMA_ChipMaster : RequestTypeBase<GameWebAPI.RequestMA_ChipM, GameWebAPI.RespDataMA_ChipM>
	{
		public RequestMA_ChipMaster()
		{
			this.apiId = "991301";
		}
	}

	public sealed class RequestMA_ChipM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_ChipM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_ChipM.Chip[] chipM;

		[Serializable]
		public class Chip
		{
			public string chipId;

			public string chipGroupId;

			public string name;

			public string rank;

			public string multiNum;

			public string needChip;

			public string icon;

			public string detail;

			public string effect;

			private string iconPath;

			private int sellPrice;

			public void SetIconPath(string path)
			{
				this.iconPath = path;
			}

			public string GetIconPath()
			{
				return this.iconPath;
			}

			public void SetSellPrice(int price)
			{
				this.sellPrice = price;
			}

			public int GetSellPrice()
			{
				return this.sellPrice;
			}
		}
	}

	public sealed class RequestMA_ChipEffectMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_ChipEffectM>
	{
		public RequestMA_ChipEffectMaster()
		{
			this.apiId = "991302";
		}
	}

	[Serializable]
	public sealed class RespDataMA_ChipEffectM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectM;

		[Serializable]
		public class ChipEffect
		{
			public string chipEffectId;

			public string chipId;

			public string targetType;

			public string targetSubType;

			public string targetValue;

			public string targetValue2;

			public string effectType;

			public string effectSubType;

			public string effectTrigger;

			public string effectTriggerValue;

			public string effectTrigger2;

			public string effectTriggerValue2;

			public string effectTurnType;

			public string effectTurn;

			public string effectValue;

			public string lot;
		}
	}

	public sealed class RequestMA_MessageStringMaster : RequestTypeBase<GameWebAPI.RequestMA_MessageStringM, GameWebAPI.RespDataMA_MessageStringM>
	{
		public RequestMA_MessageStringMaster()
		{
			this.apiId = "990802";
		}
	}

	public sealed class RequestMA_MessageStringM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_MessageStringM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_MessageStringM.Message[] messageStringM;

		[Serializable]
		public class Message
		{
			public string messageCode;

			public string messageText;
		}
	}

	public sealed class RequestDataMA_EventPointAchieveRewardM : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_EventPointAchieveRewardM>
	{
		public RequestDataMA_EventPointAchieveRewardM()
		{
			this.apiId = "991401";
		}
	}

	[Serializable]
	public sealed class RespDataMA_EventPointAchieveRewardM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_EventPointAchieveRewardM.EventPointAchieveReward[] eventPointAchieveRewardM;

		[Serializable]
		public class EventPointAchieveReward
		{
			public string eventPointAchieveRewardId;

			public string worldEventId;

			public string point;

			public string loopFlg;

			public string assetCategoryId;

			public string assetValue;

			public string assetNum;

			public string exValue;
		}
	}

	public sealed class RequestDataMA_EventPointBonusMaster : RequestTypeBase<GameWebAPI.RequestDataMA_EventPointBonusM, GameWebAPI.RespDataMA_EventPointBonusM>
	{
		public RequestDataMA_EventPointBonusMaster()
		{
			this.apiId = "991402";
		}
	}

	public sealed class RequestDataMA_EventPointBonusM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_EventPointBonusM : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_EventPointBonusM.EventPointBonus[] eventPointBonusM;

		[Serializable]
		public class EventPointBonus
		{
			public string eventPointBonusId;

			public string worldEventId;

			public string worldDungeonId;

			public string targetSubType;

			public string targetValue;

			public string targetValue2;

			public string effectType;

			public string effectSubType;

			public string effectValue;

			public string detail;

			public string createTime;

			public string createUserId;

			public string updateTime;

			public string updateUserId;
		}
	}

	public sealed class RequestDataMA_WorldEventAreaMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_WorldEventAreaMaster>
	{
		public RequestDataMA_WorldEventAreaMaster()
		{
			this.apiId = "991403";
		}
	}

	[Serializable]
	public sealed class RespDataMA_WorldEventAreaMaster : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_WorldEventAreaMaster.WorldEventAreaM[] worldEventAreaM;

		[Serializable]
		public class WorldEventAreaM
		{
			public string worldEventId;

			public string worldAreaId;
		}
	}

	public sealed class RequestDataMA_WorldEventMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_WorldEventMaster>
	{
		public RequestDataMA_WorldEventMaster()
		{
			this.apiId = "991404";
		}
	}

	[Serializable]
	public sealed class RespDataMA_WorldEventMaster : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_WorldEventMaster.WorldEventM[] worldEventM;

		[Serializable]
		public class WorldEventM
		{
			public string worldEventId;

			public string eventType;

			public string startTime;

			public string endTime;

			public string receiveStartTime;

			public string receiveEndTime;

			public string backgroundImg;
		}
	}

	public sealed class RequestDataMA_DungeonTicketMaster : RequestTypeBase<GameWebAPI.RequestDataMA_DungeonTicketM, GameWebAPI.RespDataMA_DungeonTicketMaster>
	{
		public RequestDataMA_DungeonTicketMaster()
		{
			this.apiId = "991501";
		}
	}

	public sealed class RequestDataMA_DungeonTicketM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_DungeonTicketMaster : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM[] dungeonTicketM;

		public GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM GetTicketMaster(string ticketId)
		{
			for (int i = 0; i < this.dungeonTicketM.Length; i++)
			{
				if (ticketId == this.dungeonTicketM[i].dungeonTicketId)
				{
					return this.dungeonTicketM[i];
				}
			}
			return null;
		}

		[Serializable]
		public class DungeonTicketM
		{
			public string dungeonTicketId;

			public string name;

			public string description;

			public string img;

			public string worldDungeonId;

			public string freeFlg;

			public string expireTime;
		}
	}

	public sealed class RequestDataMA_NavigationMessageMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_NavigationMessageMaster>
	{
		public RequestDataMA_NavigationMessageMaster()
		{
			this.apiId = "999102";
		}
	}

	[Serializable]
	public sealed class RespDataMA_NavigationMessageMaster : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo[] navigationMessageM;

		[Serializable]
		public class NavigationMessageInfo
		{
			public int navigationMessageId;

			public string scriptPath;
		}
	}

	public sealed class RequestDataMA_TitleMaster : RequestTypeBase<GameWebAPI.RequestDataMA_TitleM, GameWebAPI.RespDataMA_TitleMaster>
	{
		public RequestDataMA_TitleMaster()
		{
			this.apiId = "999103";
		}
	}

	public sealed class RequestDataMA_TitleM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_TitleMaster : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_TitleMaster.TitleM[] titleM;

		[Serializable]
		public class TitleM
		{
			public string titleId;

			public string name;

			public string detail;

			public string img;

			public string dispFlg;
		}
	}

	public sealed class RequestDataMA_WorldDungeonAdventureSceneMaster : RequestTypeBase<GameWebAPI.RequestDataMA_WorldDungeonAdventureSceneM, GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster>
	{
		public RequestDataMA_WorldDungeonAdventureSceneMaster()
		{
			this.apiId = "990912";
		}
	}

	public sealed class RequestDataMA_WorldDungeonAdventureSceneM : WebAPI.SendBaseData
	{
		public int countryCode;
	}

	[Serializable]
	public sealed class RespDataMA_WorldDungeonAdventureSceneMaster : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster.WorldDungeonAdventureScene[] worldDungeonAdventureSceneM;

		[Serializable]
		public class WorldDungeonAdventureScene
		{
			public string worldDungeonId = string.Empty;

			public string adventureId = string.Empty;

			public string adventureTrigger = string.Empty;

			public string adventureTriggerValue = string.Empty;

			public string adventureValue = string.Empty;
		}
	}

	public sealed class RequestDataMA_MonsterIntegrationGroupMaster : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster>
	{
		public RequestDataMA_MonsterIntegrationGroupMaster()
		{
			this.apiId = "990122";
		}
	}

	[Serializable]
	public sealed class RespDataMA_MonsterIntegrationGroupMaster : WebAPI.ResponseData
	{
		public GameWebAPI.RespDataMA_MonsterIntegrationGroupMaster.MonsterIntegrationGroup[] monsterIntegrationGroupM;

		[Serializable]
		public class MonsterIntegrationGroup
		{
			public string monsterIntegrationId = string.Empty;

			public string monsterId = string.Empty;
		}
	}

	public class ResponseData_ChatGroupList : WebAPI.ResponseData
	{
		public GameWebAPI.ResponseData_ChatGroupList.lists[] groupList;

		public GameWebAPI.ResponseData_ChatGroupList.lists[] inviteList;

		public GameWebAPI.ResponseData_ChatGroupList.lists[] requestList;

		public int viewNum;

		public int groupNum;

		public class lists
		{
			public string chatGroupId;

			public string categoryId;

			public string ownerUserId;

			public string groupName;

			public string comment;

			public string memberNum;

			public string approvalType;

			public string createTime;

			public string chatMessageHistoryId;

			public GameWebAPI.ResponseData_ChatGroupList.lists.respOwnerInfo ownerInfo;

			public string chatMemberInviteId;

			public string chatMemberRequestId;

			public class respOwnerInfo
			{
				public string nickname;

				public string monsterId;

				public string loginTime;

				public string description;

				public string titleId;
			}
		}
	}

	public class ResponseData_ChatUserList : WebAPI.ResponseData
	{
		public GameWebAPI.ResponseData_ChatUserList.respUserList[] memberList;

		public GameWebAPI.ResponseData_ChatUserList.respUserList[] inviteList;

		public GameWebAPI.ResponseData_ChatUserList.respUserList[] requestList;

		public int resultCode;

		public class respUserList
		{
			public string chatMemberInviteId;

			public string chatMemberRequestId;

			public string userId;

			public GameWebAPI.ResponseData_ChatUserList.respUserList.respUserInfo userInfo;

			public class respUserInfo
			{
				public string nickname;

				public string monsterId;

				public string loginTime;

				public string description;

				public string titleId;
			}
		}
	}

	public class Common_MessageData
	{
		public string chatMessageHistoryId;

		public int type;

		public string chatGroupId;

		public int ngwordFlg;

		public string userId;

		public string message;

		public string nickname;

		public string createTime;

		public GameWebAPI.Common_MessageData.respUserInfo userInfo;

		public string target;

		public string resultCode;

		public class respUserInfo
		{
			public string nickname;

			public string monsterId;

			public string titleId;
		}
	}

	public class Common_MonsterData
	{
		public string userMonsterId;

		public string userId;

		public string monsterId;

		public string level;

		public string ex;

		public string levelEx;

		public string nextLevelEx;

		public string leaderSkillId;

		public string uniqueSkillId;

		public string defaultSkillGroupSubId;

		public string commonSkillId;

		public string extraCommonSkillId;

		public string eggFlg;

		public string growEndDate;

		public string monsterEvolutionRouteId;

		public string hp;

		public string attack;

		public string defense;

		public string spAttack;

		public string spDefense;

		public string luck;

		public string hpAbilityFlg;

		public string hpAbility;

		public string attackAbilityFlg;

		public string attackAbility;

		public string defenseAbilityFlg;

		public string defenseAbility;

		public string spAttackAbilityFlg;

		public string spAttackAbility;

		public string spDefenseAbilityFlg;

		public string spDefenseAbility;

		public string speedAbilityFlg;

		public string speedAbility;

		public string friendship;

		public string statusFlgs;

		public string tranceResistance;

		public string tranceStatusAilment;

		public string speed;

		public int[] chipIdList;

		public Common_MonsterData()
		{
		}

		public Common_MonsterData(global::MonsterData monsterData) : this(monsterData.userMonster, monsterData.GetChipEquip())
		{
		}

		public Common_MonsterData(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster, MonsterChipEquipData equip)
		{
			this.userMonsterId = userMonster.userMonsterId;
			this.userId = userMonster.userId;
			this.monsterId = userMonster.monsterId;
			this.level = userMonster.level;
			this.ex = userMonster.ex;
			this.levelEx = userMonster.levelEx;
			this.nextLevelEx = userMonster.nextLevelEx;
			this.leaderSkillId = userMonster.leaderSkillId;
			this.uniqueSkillId = userMonster.uniqueSkillId;
			this.defaultSkillGroupSubId = userMonster.defaultSkillGroupSubId;
			this.commonSkillId = userMonster.commonSkillId;
			this.extraCommonSkillId = userMonster.extraCommonSkillId;
			this.eggFlg = userMonster.eggFlg;
			this.growEndDate = userMonster.growEndDate;
			this.monsterEvolutionRouteId = userMonster.monsterEvolutionRouteId;
			this.hp = userMonster.hp;
			this.attack = userMonster.attack;
			this.defense = userMonster.defense;
			this.spAttack = userMonster.spAttack;
			this.spDefense = userMonster.spDefense;
			this.luck = userMonster.luck;
			this.hpAbilityFlg = userMonster.hpAbilityFlg;
			this.hpAbility = userMonster.hpAbility;
			this.attackAbilityFlg = userMonster.attackAbilityFlg;
			this.attackAbility = userMonster.attackAbility;
			this.defenseAbilityFlg = userMonster.defenseAbilityFlg;
			this.defenseAbility = userMonster.defenseAbility;
			this.spAttackAbilityFlg = userMonster.spAttackAbilityFlg;
			this.spAttackAbility = userMonster.spAttackAbility;
			this.spDefenseAbilityFlg = userMonster.spDefenseAbilityFlg;
			this.spDefenseAbility = userMonster.spDefenseAbility;
			this.speedAbilityFlg = userMonster.speedAbilityFlg;
			this.speedAbility = userMonster.speedAbility;
			this.friendship = userMonster.friendship;
			this.statusFlgs = userMonster.statusFlgs;
			this.tranceResistance = userMonster.tranceResistance;
			this.tranceStatusAilment = userMonster.tranceStatusAilment;
			this.speed = userMonster.speed;
			if (equip != null)
			{
				this.chipIdList = equip.GetChipIdList();
			}
		}

		public Common_MonsterData(GameWebAPI.Common_MonsterData source)
		{
			this.userMonsterId = source.userMonsterId;
			this.userId = source.userId;
			this.monsterId = source.monsterId;
			this.level = source.level;
			this.ex = source.ex;
			this.levelEx = source.levelEx;
			this.nextLevelEx = source.nextLevelEx;
			this.leaderSkillId = source.leaderSkillId;
			this.uniqueSkillId = source.uniqueSkillId;
			this.defaultSkillGroupSubId = source.defaultSkillGroupSubId;
			this.commonSkillId = source.commonSkillId;
			this.extraCommonSkillId = source.extraCommonSkillId;
			this.eggFlg = source.eggFlg;
			this.growEndDate = source.growEndDate;
			this.monsterEvolutionRouteId = source.monsterEvolutionRouteId;
			this.hp = source.hp;
			this.attack = source.attack;
			this.defense = source.defense;
			this.spAttack = source.spAttack;
			this.spDefense = source.spDefense;
			this.luck = source.luck;
			this.hpAbilityFlg = source.hpAbilityFlg;
			this.hpAbility = source.hpAbility;
			this.attackAbilityFlg = source.attackAbilityFlg;
			this.attackAbility = source.attackAbility;
			this.defenseAbilityFlg = source.defenseAbilityFlg;
			this.defenseAbility = source.defenseAbility;
			this.spAttackAbilityFlg = source.spAttackAbilityFlg;
			this.spAttackAbility = source.spAttackAbility;
			this.spDefenseAbilityFlg = source.spDefenseAbilityFlg;
			this.spDefenseAbility = source.spDefenseAbility;
			this.speedAbilityFlg = source.speedAbilityFlg;
			this.speedAbility = source.speedAbility;
			this.friendship = source.friendship;
			this.statusFlgs = source.statusFlgs;
			this.tranceResistance = source.tranceResistance;
			this.tranceStatusAilment = source.tranceStatusAilment;
			this.speed = source.speed;
			if (source.chipIdList != null)
			{
				this.chipIdList = new int[source.chipIdList.Length];
				for (int i = 0; i < source.chipIdList.Length; i++)
				{
					this.chipIdList[i] = source.chipIdList[i];
				}
			}
		}

		public void SetChipIdList()
		{
			GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] monsterChipList = ChipDataMng.GetMonsterChipList(this.userMonsterId);
			if (monsterChipList != null && monsterChipList.Count<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>() > 0)
			{
				this.chipIdList = new int[monsterChipList.Count<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>()];
				for (int i = 0; i < monsterChipList.Count<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>(); i++)
				{
					this.chipIdList[i] = monsterChipList[i].chipId;
				}
			}
		}

		public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList ToUserMonsterList()
		{
			return new GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList
			{
				userMonsterId = this.userMonsterId,
				userId = this.userId,
				monsterId = this.monsterId,
				level = this.level,
				ex = this.ex,
				levelEx = this.levelEx,
				nextLevelEx = this.nextLevelEx,
				leaderSkillId = this.leaderSkillId,
				uniqueSkillId = this.uniqueSkillId,
				defaultSkillGroupSubId = this.defaultSkillGroupSubId,
				commonSkillId = this.commonSkillId,
				extraCommonSkillId = this.extraCommonSkillId,
				eggFlg = this.eggFlg,
				growEndDate = this.growEndDate,
				monsterEvolutionRouteId = this.monsterEvolutionRouteId,
				hp = this.hp,
				attack = this.attack,
				defense = this.defense,
				spAttack = this.spAttack,
				spDefense = this.spDefense,
				luck = this.luck,
				hpAbilityFlg = this.hpAbilityFlg,
				hpAbility = this.hpAbility,
				attackAbilityFlg = this.attackAbilityFlg,
				attackAbility = this.attackAbility,
				defenseAbilityFlg = this.defenseAbilityFlg,
				defenseAbility = this.defenseAbility,
				spAttackAbilityFlg = this.spAttackAbilityFlg,
				spAttackAbility = this.spAttackAbility,
				spDefenseAbilityFlg = this.spDefenseAbilityFlg,
				spDefenseAbility = this.spDefenseAbility,
				speedAbilityFlg = this.speedAbilityFlg,
				speedAbility = this.speedAbility,
				friendship = this.friendship,
				statusFlgs = this.statusFlgs,
				tranceResistance = this.tranceResistance,
				tranceStatusAilment = this.tranceStatusAilment,
				speed = this.speed
			};
		}

		public global::MonsterData ToMonsterData()
		{
			global::MonsterData monsterData = new global::MonsterData(this.ToUserMonsterList());
			monsterData.CreateEmptyChipEquip();
			if (!(DataMng.Instance().RespDataCM_Login.playerInfo.userId != monsterData.GetMonster().userId))
			{
				this.SetChipIdList();
			}
			if (this.chipIdList != null && 0 < this.chipIdList.Length)
			{
				monsterData.GetChipEquip().SetChipIdList(this.chipIdList);
			}
			return monsterData;
		}
	}

	public class ResponseData_Common_MultiRoomList : WebAPI.ResponseData
	{
		public GameWebAPI.ResponseData_Common_MultiRoomList.room[] multiRoomList;

		public int friendRequestFlag;

		public class room
		{
			public string multiRoomId;

			public string userId;

			public string ownerMonsterId;

			public string worldAreaId;

			public string worldDungeonId;

			public string worldStageId;

			public string moodType;

			public string status;

			public string introduction;

			public string password;

			public string announceType;

			public string closeTime;

			public string ownerName;

			public string dungeonName;

			public string titleId;

			public string memberUserId;

			public string secondMemberUserId;

			public int isAllow;

			public int memberCount;

			public string multiRoomRequestId;
		}
	}

	public class Common_MultiMemberList
	{
		public string userId;

		public string nickname;

		public string titleId;

		public string onlineStatus;

		public GameWebAPI.Common_MonsterData[] userMonsterList;
	}

	public class Common_MultiRoomInfo
	{
		public string announceType;

		public string closeTime;

		public string dungeonName;

		public string introduction;

		public string moodType;

		public string multiRoomId;

		public string ownerMonsterId;

		public string password;

		public string stageName;

		public string status;

		public string userId;

		public string priority;

		public string worldAreaId;

		public string worldStageId;

		public string worldDungeonId;
	}

	public class ReqData_MultiRoomList : WebAPI.SendBaseData
	{
		public int isFriend;

		public int dungeonId;

		public enum From
		{
			OTHERS,
			FRIEND
		}
	}

	public class RespData_MultiRoomList : GameWebAPI.ResponseData_Common_MultiRoomList
	{
	}

	public class MultiRoomList : RequestTypeBase<GameWebAPI.ReqData_MultiRoomList, GameWebAPI.RespData_MultiRoomList>
	{
		public MultiRoomList()
		{
			this.apiId = "120101";
		}
	}

	public class ReqData_MultiRoomCreate : WebAPI.SendBaseData
	{
		public int worldDungeonId;

		public int moodType;

		public int announceType;

		public string introduction;

		public int deckNum;
	}

	public class RespData_MultiRoomCreate : WebAPI.ResponseData
	{
		public GameWebAPI.Common_MultiRoomInfo multiRoomInfo;

		public GameWebAPI.Common_MultiMemberList[] multiRoomMemberList;
	}

	public class MultiRoomCreate : RequestTypeBase<GameWebAPI.ReqData_MultiRoomCreate, GameWebAPI.RespData_MultiRoomCreate>
	{
		public MultiRoomCreate()
		{
			this.apiId = "120102";
		}
	}

	public class ReqData_MultiRoomJoin : WebAPI.SendBaseData
	{
		public int roomId;

		public string password;
	}

	public class RespData_MultiRoomJoin : WebAPI.ResponseData
	{
		public GameWebAPI.Common_MultiRoomInfo multiRoomInfo;

		public GameWebAPI.Common_MultiMemberList[] multiRoomMemberList;
	}

	public class MultiRoomJoin : RequestTypeBase<GameWebAPI.ReqData_MultiRoomJoin, GameWebAPI.RespData_MultiRoomJoin>
	{
		public MultiRoomJoin()
		{
			this.apiId = "120103";
		}
	}

	public class ReqData_MultiRoomLeave : WebAPI.SendBaseData
	{
		public int roomId;
	}

	public class MultiRoomLeave : RequestTypeBase<GameWebAPI.ReqData_MultiRoomLeave, WebAPI.ResponseData>
	{
		public MultiRoomLeave()
		{
			this.apiId = "120105";
		}
	}

	public class MultiRoomBreakup : RequestTypeBase<WebAPI.SendBaseData, WebAPI.ResponseData>
	{
		public MultiRoomBreakup()
		{
			this.apiId = "120107";
		}
	}

	public class ReqData_MultiRoomRequestRegist : WebAPI.SendBaseData
	{
		public int roomId;

		public List<int> userList;
	}

	public class RespData_MultiRoomRequestRegist : WebAPI.ResponseData
	{
		public int result;
	}

	public class MultiRoomRequestRegist : RequestTypeBase<GameWebAPI.ReqData_MultiRoomRequestRegist, GameWebAPI.RespData_MultiRoomRequestRegist>
	{
		public MultiRoomRequestRegist()
		{
			this.apiId = "120108";
		}
	}

	public class ReqData_MultiRoomRequestList : WebAPI.SendBaseData
	{
	}

	public class RespData_MultiRoomRequestList : GameWebAPI.ResponseData_Common_MultiRoomList
	{
	}

	public class MultiRoomRequestList : RequestTypeBase<GameWebAPI.ReqData_MultiRoomRequestList, GameWebAPI.RespData_MultiRoomRequestList>
	{
		public MultiRoomRequestList()
		{
			this.apiId = "120109";
		}
	}

	public class ReqData_MultiRoomStatusInfoLogic : WebAPI.SendBaseData
	{
		public int roomId;
	}

	public class RespData_MultiRoomStatusInfoLogic : WebAPI.ResponseData
	{
		public GameWebAPI.RespData_MultiRoomStatusInfoLogic.StatusInfo statusInfo;

		public enum Status
		{
			RECRUIT = 1,
			INTERRRUPTION = 10
		}

		public class StatusInfo
		{
			public int roomId;

			public string status;

			public GameWebAPI.RespData_MultiRoomStatusInfoLogic.StatusInfo.Member[] member;

			public class Member
			{
				public string userId;

				public int onlineStatus;
			}
		}
	}

	public class MultiRoomStatusInfoLogic : RequestTypeBase<GameWebAPI.ReqData_MultiRoomStatusInfoLogic, GameWebAPI.RespData_MultiRoomStatusInfoLogic>
	{
		public MultiRoomStatusInfoLogic()
		{
			this.apiId = "120110";
		}
	}

	public class ReqData_MultiRoomUpdate : WebAPI.SendBaseData
	{
		public int roomId;

		public int user_monster_id;
	}

	public class RespData_MultiRoomUpdate : WebAPI.ResponseData
	{
		public int result;
	}

	public class MultiRoomUpdate : RequestTypeBase<GameWebAPI.ReqData_MultiRoomUpdate, GameWebAPI.RespData_MultiRoomUpdate>
	{
		public MultiRoomUpdate()
		{
			this.apiId = "120115";
		}
	}

	public class ReqData_WorldMultiStartInfo : WebAPI.SendBaseData
	{
		public int startId;
	}

	public class RespData_WorldMultiStartInfo : WebAPI.ResponseData
	{
		public string multiRoomId;

		public string startId;

		public string worldDungeonId;

		public int totalExp;

		public int totalMoney;

		public int memberPatternId;

		public GameWebAPI.RespData_WorldMultiStartInfo.CriticalRate criticalRate;

		public GameWebAPI.RespData_WorldMultiStartInfo.LuckDrop[] luckDrop;

		public GameWebAPI.RespData_WorldMultiStartInfo.Party[] party;

		public GameWebAPI.RespDataWD_DungeonStart.DungeonFloor[] dungeonFloor;

		public class CriticalRate
		{
			public int partyCriticalRate;

			public int enemyCriticalRate;
		}

		public class LuckDrop
		{
			public int dropBoxType;

			public string assetCategoryId;

			public int assetValue;

			public int assetNum;

			public string userId;
		}

		public class Party
		{
			public string userId;

			public string nickname;

			public string titleId;

			public GameWebAPI.Common_MonsterData[] userMonsters;
		}
	}

	public class WorldMultiStartInfo : RequestTypeBase<GameWebAPI.ReqData_WorldMultiStartInfo, GameWebAPI.RespData_WorldMultiStartInfo>
	{
		public WorldMultiStartInfo()
		{
			this.apiId = "120201";
		}
	}

	public class ReqData_WorldMultiResultInfoLogic : WebAPI.SendBaseData
	{
		public int startId;

		public int clearRound;
	}

	public class RespData_WorldMultiResultInfoLogic : WebAPI.ResponseData
	{
		public string worldDungeonId;

		public int clearType;

		public int[] onlineGuestIds;

		public int totalExp;

		public int totalMoney;

		public GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward dungeonReward;

		public GameWebAPI.RespDataWD_DungeonResult.Drop[] dropReward;

		public GameWebAPI.RespDataWD_DungeonResult.OptionDrop[] optionDrop;

		public GameWebAPI.RespDataWD_DungeonResult.EventChipReward[] eventChipReward;

		public class DungeonReward
		{
			public GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.ClearReward[] clearReward;

			public int friendPoint;

			public GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.DropReward[] multiReward;

			public GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.LuckDrop[] luckDrop;

			public GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.LinkBonus[] linkBonus;

			public GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.DropReward[] ownerDropReward;

			public class Reward
			{
				public string assetCategoryId;

				public int assetValue;

				public int assetNum;
			}

			public class DropReward : GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.Reward
			{
				public int dropBoxType;
			}

			public class ClearReward : GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.Reward
			{
				public string worldDungeonRewardId;

				public int everyTimeFlg;

				public string exValue;
			}

			public class LuckDrop : GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.DropReward
			{
				public string userId;
			}

			public class LinkBonus
			{
				public int type;

				public GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.LinkBonus.LinkBonusReward[] reward;

				public class LinkBonusReward : GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.Reward
				{
					public int linkNum;
				}
			}
		}
	}

	public class WorldMultiResultInfoLogic : RequestTypeBase<GameWebAPI.ReqData_WorldMultiResultInfoLogic, GameWebAPI.RespData_WorldMultiResultInfoLogic>
	{
		public WorldMultiResultInfoLogic()
		{
			this.apiId = "120202";
		}
	}

	public class ReqData_WorldMultiDungeonContinueLogic : WebAPI.SendBaseData
	{
		public int startId;

		public int floorNum;

		public int roundNum;

		public int[] userMonsterId;
	}

	public class RespData_WorldMultiDungeonContinueLogic : WebAPI.ResponseData
	{
		public int result;

		public bool IsSuccess()
		{
			return 1 == this.result;
		}
	}

	public class WorldMultiDungeonContinueLogic : RequestTypeBase<GameWebAPI.ReqData_WorldMultiDungeonContinueLogic, GameWebAPI.RespData_WorldMultiDungeonContinueLogic>
	{
		public WorldMultiDungeonContinueLogic()
		{
			this.apiId = "120203";
		}
	}

	public class RespData_UserChatGroupList : GameWebAPI.ResponseData_ChatGroupList
	{
	}

	public class UserChatGroupList : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespData_UserChatGroupList>
	{
		public UserChatGroupList()
		{
			this.apiId = "150001";
		}
	}

	public class ReqData_ChatRecruitGroupListLogic : WebAPI.SendBaseData
	{
		public List<int> categoryId;

		public List<int> approvalType;

		public int sortType;

		public int page;
	}

	public class RespData_ChatRecruitGroupListLogic : GameWebAPI.ResponseData_ChatGroupList
	{
	}

	public class ChatRecruitGroupListLogic : RequestTypeBase<GameWebAPI.ReqData_ChatRecruitGroupListLogic, GameWebAPI.RespData_ChatRecruitGroupListLogic>
	{
		public ChatRecruitGroupListLogic()
		{
			this.apiId = "150002";
		}
	}

	public class ReqData_CreateChatGroupLogic : WebAPI.SendBaseData
	{
		public int categoryId;

		public string groupName;

		public string comment;

		public int approvalType;
	}

	public class RespData_CreateChatGroupLogic : WebAPI.ResponseData
	{
		public int chatGroupId;
	}

	public class CreateChatGroupLogic : RequestTypeBase<GameWebAPI.ReqData_CreateChatGroupLogic, GameWebAPI.RespData_CreateChatGroupLogic>
	{
		public CreateChatGroupLogic()
		{
			this.apiId = "150003";
		}
	}

	public class ReqData_ChatInviteMemberLogic : WebAPI.SendBaseData
	{
		public int chatGroupId;

		public int[] inviteUserIds;
	}

	public class RespData_ChatInviteMemberLogic : WebAPI.ResponseData
	{
		public int[] inviteUserIds;

		public int[] failureUserIds;
	}

	public class ChatInviteMemberLogic : RequestTypeBase<GameWebAPI.ReqData_ChatInviteMemberLogic, GameWebAPI.RespData_ChatInviteMemberLogic>
	{
		public ChatInviteMemberLogic()
		{
			this.apiId = "150004";
		}
	}

	public class RespData_UserChatInviteListLogic : GameWebAPI.ResponseData_ChatGroupList
	{
	}

	public class UserChatInviteListLogic : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespData_UserChatInviteListLogic>
	{
		public UserChatInviteListLogic()
		{
			this.apiId = "150005";
		}
	}

	public class ReqData_ChatReplyToInviteLogic : WebAPI.SendBaseData
	{
		public int chatMemberInviteId;

		public int reply;
	}

	public class RespData_ChatReplyToInviteLogic : WebAPI.ResponseData
	{
		public int result;
	}

	public class ChatReplyToInviteLogic : RequestTypeBase<GameWebAPI.ReqData_ChatReplyToInviteLogic, GameWebAPI.RespData_ChatReplyToInviteLogic>
	{
		public ChatReplyToInviteLogic()
		{
			this.apiId = "150006";
		}
	}

	public class ReqData_ChatRequestMember : WebAPI.SendBaseData
	{
		public int chatGroupId;
	}

	public class RespData_ChatRequestMember : WebAPI.ResponseData
	{
		public int result;

		public int approvalType;
	}

	public class ChatRequestMember : RequestTypeBase<GameWebAPI.ReqData_ChatRequestMember, GameWebAPI.RespData_ChatRequestMember>
	{
		public ChatRequestMember()
		{
			this.apiId = "150007";
		}
	}

	public class RespData_UserChatRequestList : GameWebAPI.ResponseData_ChatGroupList
	{
	}

	public class UserChatRequestListLogic : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespData_UserChatRequestList>
	{
		public UserChatRequestListLogic()
		{
			this.apiId = "150008";
		}
	}

	public class ReqData_ChatMemberRequestList : WebAPI.SendBaseData
	{
		public int chatGroupId;
	}

	public class RespData_ChatMemberRequestList : GameWebAPI.ResponseData_ChatUserList
	{
	}

	public class ChatMemberRequestListLogic : RequestTypeBase<GameWebAPI.ReqData_ChatMemberRequestList, GameWebAPI.RespData_ChatMemberRequestList>
	{
		public ChatMemberRequestListLogic()
		{
			this.apiId = "150009";
		}
	}

	public class ReqData_ChatReplyToRequestLogic : WebAPI.SendBaseData
	{
		public int chatMemberRequestId;

		public int reply;
	}

	public class RespData_ChatReplyToRequestLogic : WebAPI.ResponseData
	{
		public int result;
	}

	public class ChatReplyToRequestLogic : RequestTypeBase<GameWebAPI.ReqData_ChatReplyToRequestLogic, GameWebAPI.RespData_ChatReplyToRequestLogic>
	{
		public ChatReplyToRequestLogic()
		{
			this.apiId = "150010";
		}
	}

	public class ReqData_ChatCancelMemberRequestLogic : WebAPI.SendBaseData
	{
		public int chatMemberRequestId;
	}

	public class RespData_ChatCancelMemberRequestLogic : WebAPI.ResponseData
	{
		public int result;
	}

	public class ChatCancelMemberRequestLogic : RequestTypeBase<GameWebAPI.ReqData_ChatCancelMemberRequestLogic, GameWebAPI.RespData_ChatCancelMemberRequestLogic>
	{
		public ChatCancelMemberRequestLogic()
		{
			this.apiId = "150011";
		}
	}

	public class ReqData_ChatGroupMemberList : WebAPI.SendBaseData
	{
		public int chatGroupId;
	}

	public class RespData_ChatGroupMemberList : GameWebAPI.ResponseData_ChatUserList
	{
	}

	public class ChatGroupMemberList : RequestTypeBase<GameWebAPI.ReqData_ChatGroupMemberList, GameWebAPI.RespData_ChatGroupMemberList>
	{
		public ChatGroupMemberList()
		{
			this.apiId = "150012";
		}
	}

	public class ReqData_ChatHandoverGroupOwnerLogic : WebAPI.SendBaseData
	{
		public int chatGroupId;

		public int toUserId;
	}

	public class RespData_ChatHandoverGroupOwnerLogic : WebAPI.ResponseData
	{
		public int result;
	}

	public class ChatHandoverGroupOwnerLogic : RequestTypeBase<GameWebAPI.ReqData_ChatHandoverGroupOwnerLogic, GameWebAPI.RespData_ChatHandoverGroupOwnerLogic>
	{
		public ChatHandoverGroupOwnerLogic()
		{
			this.apiId = "150013";
		}
	}

	public class ReqData_EditChatGroupLogic : WebAPI.SendBaseData
	{
		public int chatGroupId;

		public int categoryId;

		public string groupName;

		public string comment;

		public int approvalType;
	}

	public class RespData_EditChatGroupLogic : WebAPI.ResponseData
	{
		public int result;
	}

	public class EditChatGroupLogic : RequestTypeBase<GameWebAPI.ReqData_EditChatGroupLogic, GameWebAPI.RespData_EditChatGroupLogic>
	{
		public EditChatGroupLogic()
		{
			this.apiId = "150014";
		}
	}

	public class ReqData_DeleteChatGroupLogic : WebAPI.SendBaseData
	{
		public int chatGroupId;
	}

	public class RespData_DeleteChatGroupLogic : WebAPI.ResponseData
	{
		public int result;
	}

	public class DeleteChatGroupLogic : RequestTypeBase<GameWebAPI.ReqData_DeleteChatGroupLogic, GameWebAPI.RespData_DeleteChatGroupLogic>
	{
		public DeleteChatGroupLogic()
		{
			this.apiId = "150015";
		}
	}

	public class ReqData_ChatResignGroupLogic : WebAPI.SendBaseData
	{
		public int chatGroupId;
	}

	public class RespData_ChatResignGroupLogic : WebAPI.ResponseData
	{
		public int result;

		public int resultCode;
	}

	public class ChatResignGroupLogic : RequestTypeBase<GameWebAPI.ReqData_ChatResignGroupLogic, GameWebAPI.RespData_ChatResignGroupLogic>
	{
		public ChatResignGroupLogic()
		{
			this.apiId = "150016";
		}
	}

	public class ReqData_ChatNewMessageHistoryLogic : WebAPI.SendBaseData
	{
		public int chatGroupId;

		public int limit;

		public int borderMessageId;
	}

	public class RespData_ChatNewMessageHistoryLogic : WebAPI.ResponseData
	{
		public GameWebAPI.RespData_ChatNewMessageHistoryLogic.Result[] result;

		public int resultCode;

		public class Result : GameWebAPI.Common_MessageData
		{
		}
	}

	public class ChatNewMessageHistoryLogic : RequestTypeBase<GameWebAPI.ReqData_ChatNewMessageHistoryLogic, GameWebAPI.RespData_ChatNewMessageHistoryLogic>
	{
		public ChatNewMessageHistoryLogic()
		{
			this.apiId = "150017";
		}
	}

	public class ReqData_ChatReceiveMessage : WebAPI.SendBaseData
	{
		public int chatGroupId;

		public string message;

		public int type;
	}

	public class RespData_ChatReceiveMessage : WebAPI.ResponseData
	{
		public int chatMessageHistoryId;

		public int ngwordFlg;
	}

	public class ChatReceiveMessage : RequestTypeBase<GameWebAPI.ReqData_ChatReceiveMessage, GameWebAPI.RespData_ChatReceiveMessage>
	{
		public ChatReceiveMessage()
		{
			this.apiId = "150019";
		}
	}

	public class ReqData_ChatMemberInviteList : WebAPI.SendBaseData
	{
		public int chatGroupId;
	}

	public class RespData_ChatMemberInviteList : GameWebAPI.ResponseData_ChatUserList
	{
	}

	public class ChatMemberInviteList : RequestTypeBase<GameWebAPI.ReqData_ChatMemberInviteList, GameWebAPI.RespData_ChatMemberInviteList>
	{
		public ChatMemberInviteList()
		{
			this.apiId = "150020";
		}
	}

	public class ReqData_ChatWholeGroupMemberList : WebAPI.SendBaseData
	{
		public int chatGroupId;
	}

	public class RespData_ChatWholeGroupMemberList : WebAPI.ResponseData
	{
		public GameWebAPI.RespData_ChatWholeGroupMemberList.respMember member;

		public GameWebAPI.RespData_ChatWholeGroupMemberList.respRequest request;

		public GameWebAPI.RespData_ChatWholeGroupMemberList.respInvite invite;

		public class respMember
		{
			public GameWebAPI.RespData_ChatWholeGroupMemberList.respMember.respMemberList[] memberList;

			public int resultCode;

			public class respMemberList
			{
				public string userId;

				public GameWebAPI.RespData_ChatWholeGroupMemberList.respMember.respMemberList.respUserInfo userInfo;

				public class respUserInfo
				{
					public string nickname;

					public string description;

					public string loginTime;

					public string monsterId;

					public string titleId;
				}
			}
		}

		public class respRequest
		{
			public GameWebAPI.RespData_ChatWholeGroupMemberList.respRequest.respRequestList[] requestList;

			public class respRequestList
			{
				public string chatMemberRequestId;

				public string userId;

				public GameWebAPI.RespData_ChatWholeGroupMemberList.respRequest.respRequestList.respUserInfo userInfo;

				public class respUserInfo
				{
					public string nickname;

					public string description;

					public string loginTime;

					public string monsterId;

					public string titleId;
				}
			}
		}

		public class respInvite
		{
			public GameWebAPI.RespData_ChatWholeGroupMemberList.respInvite.respInviteList[] inviteList;

			public class respInviteList
			{
				public string chatMemberInviteId;

				public string userId;

				public GameWebAPI.RespData_ChatWholeGroupMemberList.respInvite.respInviteList.respUserInfo userInfo;

				public class respUserInfo
				{
					public string nickname;

					public string description;

					public string loginTime;

					public string monsterId;

					public string titleId;
				}
			}
		}
	}

	public class ChatWholeGroupMemberList : RequestTypeBase<GameWebAPI.ReqData_ChatWholeGroupMemberList, GameWebAPI.RespData_ChatWholeGroupMemberList>
	{
		public ChatWholeGroupMemberList()
		{
			this.apiId = "150021";
		}
	}

	public class ReqData_ChatGroupInfo : WebAPI.SendBaseData
	{
		public long[] chatGroupId;
	}

	public class RespData_ChatGroupInfo : GameWebAPI.ResponseData_ChatGroupList
	{
	}

	public class ChatGroupInfo : RequestTypeBase<GameWebAPI.ReqData_ChatGroupInfo, GameWebAPI.RespData_ChatGroupInfo>
	{
		public ChatGroupInfo()
		{
			this.apiId = "150022";
		}
	}

	public class ReqData_ChatCancelMemberInvite : WebAPI.SendBaseData
	{
		public int chatMemberInviteId;
	}

	public class RespData_ChatCancelMemberInvite : WebAPI.ResponseData
	{
		public int result;
	}

	public class ChatCancelMemberInvite : RequestTypeBase<GameWebAPI.ReqData_ChatCancelMemberInvite, GameWebAPI.RespData_ChatCancelMemberInvite>
	{
		public ChatCancelMemberInvite()
		{
			this.apiId = "150023";
		}
	}

	public class RespData_ChatLastHistoryIdList : WebAPI.ResponseData
	{
		public GameWebAPI.RespData_ChatLastHistoryIdList.LastHistoryIds[] lastHistoryIds;

		public string lastMockBattleRequestTime;

		public string multiRoomRequestId;

		public class LastHistoryIds
		{
			public string chatGroupId;

			public string chatMessageHistoryId;
		}
	}

	public class ChatLastHistoryIdList : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespData_ChatLastHistoryIdList>
	{
		public ChatLastHistoryIdList()
		{
			this.apiId = "150024";
		}
	}

	public class ReqData_GetTcpShardingString : WebAPI.SendBaseData
	{
		public string type;
	}

	public class RespData_GetTcpShardingString : WebAPI.ResponseData
	{
		public string server;
	}

	public class GetTcpShardingString : RequestTypeBase<GameWebAPI.ReqData_GetTcpShardingString, GameWebAPI.RespData_GetTcpShardingString>
	{
		public GetTcpShardingString()
		{
			this.apiId = "800001";
		}
	}

	public class ReqData_ColosseumInfoLogic : WebAPI.SendBaseData
	{
	}

	public class RespData_ColosseumInfoLogic : WebAPI.ResponseData
	{
		public int resultCode;

		public int colosseumId;

		public string helpPage;

		public int extraCost;

		public int openAllDay;

		public GameWebAPI.RespData_ColosseumInfoLogic.EventInfo eventInfo;

		public class EventInfo
		{
			public string backgroundImg;

			public string endTime;

			public string eventType;

			public string receiveEndTime;

			public string receiveStartTime;

			public string startTime;

			public string worldEventId;
		}
	}

	public class ColosseumInfoLogic : RequestTypeBase<WebAPI.SendBaseData, GameWebAPI.RespData_ColosseumInfoLogic>
	{
		public ColosseumInfoLogic()
		{
			this.apiId = "080001";
		}
	}

	public class ReqData_ColosseumDeckInfoLogic : WebAPI.SendBaseData
	{
		public string target;

		public int isMockBattle;
	}

	public class RespData_ColosseumDeckInfoLogic : WebAPI.ResponseData
	{
		public GameWebAPI.Common_MonsterData[] partyMonsters;

		public int resultCode;

		public GameWebAPI.RespData_ColosseumDeckInfoLogic.ResultCode GetResultCodeEnum
		{
			get
			{
				return (GameWebAPI.RespData_ColosseumDeckInfoLogic.ResultCode)this.resultCode;
			}
		}

		public enum ResultCode
		{
			SUCCESS = 1
		}
	}

	public class ColosseumDeckInfoLogic : RequestTypeBase<GameWebAPI.ReqData_ColosseumDeckInfoLogic, GameWebAPI.RespData_ColosseumDeckInfoLogic>
	{
		public ColosseumDeckInfoLogic()
		{
			this.apiId = "080104";
		}
	}

	public class ReqData_ColosseumDeckEditLogic : WebAPI.SendBaseData
	{
		public string[] userMonsterIdList;

		public int isMockBattle;

		public int deckNum;
	}

	public class RespData_ColosseumDeckEditLogic : WebAPI.ResponseData
	{
		public int resultCode;
	}

	public class ColosseumDeckEditLogic : RequestTypeBase<GameWebAPI.ReqData_ColosseumDeckEditLogic, GameWebAPI.RespData_ColosseumDeckEditLogic>
	{
		public ColosseumDeckEditLogic()
		{
			this.apiId = "080105";
		}
	}

	public class ReqData_ColosseumUserStatusLogic : WebAPI.SendBaseData
	{
		public string target;

		public int isMockBattle;

		public static string GetTargetMyFlag()
		{
			return "me";
		}

		public static string GetTargetEnemyFlag()
		{
			return "battle";
		}
	}

	public class RespData_ColosseumUserStatusLogic : WebAPI.ResponseData
	{
		public int resultCode;

		public GameWebAPI.ColosseumUserStatus userStatus;

		public GameWebAPI.RespData_ColosseumUserStatusLogic.ColosseumBattleRecord battleRecord;

		public string penalty;

		public string noticeCode;

		public int mockBattleStatus;

		public int freeCostBattleCount;

		public int ranking;

		public bool IsNotEntry
		{
			get
			{
				return this.resultCode == 2;
			}
		}

		public bool IsBattleInterruption
		{
			get
			{
				return this.resultCode == 4;
			}
		}

		public GameWebAPI.RespData_ColosseumUserStatusLogic.ResultCode GetResultCodeEnum
		{
			get
			{
				return (GameWebAPI.RespData_ColosseumUserStatusLogic.ResultCode)this.resultCode;
			}
		}

		public GameWebAPI.RespData_ColosseumUserStatusLogic.MockBattleStatus GetMockBattleStatusEnum
		{
			get
			{
				return (GameWebAPI.RespData_ColosseumUserStatusLogic.MockBattleStatus)this.mockBattleStatus;
			}
		}

		public enum ResultCode
		{
			SUCCESS = 1,
			NOT_ENTRY,
			EMPTY_OPPONENT,
			BATTLE_INTERRUPTION,
			NOT_MATCHING,
			AGGREGATE,
			BATTLE_INTERRUPTION_WIN
		}

		public enum MockBattleStatus
		{
			NOT_OPEN,
			NOT_ENTRY,
			SUCCESS,
			BATTLE_INTERRUPTION
		}

		public class ColosseumBattleRecord
		{
			public int count;

			public int winPercent;
		}
	}

	public class ColosseumUserStatusLogic : RequestTypeBase<GameWebAPI.ReqData_ColosseumUserStatusLogic, GameWebAPI.RespData_ColosseumUserStatusLogic>
	{
		public ColosseumUserStatusLogic()
		{
			this.apiId = "080107";
		}
	}

	public class ReqData_ColosseumBattleEndLogic : WebAPI.SendBaseData
	{
		public int battleResult;

		public int roundCount;

		public string skillUseDeckPosition;

		public int isMockBattle;

		public enum BattleResult
		{
			VICTORY = 1,
			DEFEAT,
			CONNECTION_ERROR,
			RETIRE
		}

		public enum BattleMode
		{
			NORMAL_BATTLE,
			MOCK_BATTLE
		}
	}

	public class RespData_ColosseumBattleEndLogic : WebAPI.ResponseData
	{
		public int resultCode;

		public GameWebAPI.RespData_ColosseumBattleEndLogic.Reward[] reward;

		public GameWebAPI.RespData_ColosseumBattleEndLogic.Reward[] firstRankUpReward;

		public int score;

		public int colosseumRankId;

		public int isFirstRankUp;

		public GameWebAPI.RespData_ColosseumBattleEndLogic.ColosseumBattleRecord battleRecord;

		public class Reward
		{
			public int assetCategoryId;

			public int assetNum;

			public int assetValue;
		}

		public class ColosseumBattleRecord
		{
			public int count;

			public int winPercent;
		}
	}

	public class ColosseumBattleEndLogic : RequestTypeBase<GameWebAPI.ReqData_ColosseumBattleEndLogic, GameWebAPI.RespData_ColosseumBattleEndLogic>
	{
		public ColosseumBattleEndLogic()
		{
			this.apiId = "080109";
		}
	}

	public class ReqData_ColosseumPrepareStatusLogic : WebAPI.SendBaseData
	{
		public int isMockBattle;
	}

	public class RespData_ColosseumPrepareStatusLogic : WebAPI.ResponseData
	{
		public int resultCode;

		public GameWebAPI.RespData_ColosseumPrepareStatusLogic.PrepareStatusCode GetResultCodeEnum
		{
			get
			{
				return (GameWebAPI.RespData_ColosseumPrepareStatusLogic.PrepareStatusCode)this.resultCode;
			}
		}

		public enum PrepareStatusCode
		{
			READY = 1,
			NOT_READY,
			CANT_START
		}
	}

	public class ColosseumPrepareStatusLogic : RequestTypeBase<GameWebAPI.ReqData_ColosseumPrepareStatusLogic, GameWebAPI.RespData_ColosseumPrepareStatusLogic>
	{
		public ColosseumPrepareStatusLogic()
		{
			this.apiId = "080113";
		}
	}

	public class ReqData_ColosseumBattleActionLog : WebAPI.SendBaseData
	{
		public string action;

		public int isMockBattle;
	}

	public class RespData_ColosseumBattleActionLog : WebAPI.ResponseData
	{
		public int resultCode;
	}

	public class ColosseumBattleActionLog : RequestTypeBase<GameWebAPI.ReqData_ColosseumBattleActionLog, GameWebAPI.RespData_ColosseumBattleActionLog>
	{
		public ColosseumBattleActionLog()
		{
			this.apiId = "080114";
		}
	}

	public class ReqData_ColosseumMockBattleRequestListLogic : WebAPI.SendBaseData
	{
	}

	public class RespData_ColosseumMockBattleRequestListLogic : WebAPI.ResponseData
	{
		public GameWebAPI.RespData_ColosseumMockBattleRequestListLogic.MemberList[] memberList;

		public int mockBattleStatus;

		public GameWebAPI.RespData_ColosseumMockBattleRequestListLogic.MockBattleStatus GetMockBattleStatus
		{
			get
			{
				return (GameWebAPI.RespData_ColosseumMockBattleRequestListLogic.MockBattleStatus)this.mockBattleStatus;
			}
		}

		public class MemberList
		{
			public string userId;

			public GameWebAPI.RespData_ColosseumMockBattleRequestListLogic.MemberList.UserInfo userInfo;

			public class UserInfo
			{
				public string userCode;

				public string nickname;

				public string monsterId;

				public string requestTime;

				public string titleId;
			}
		}

		public enum MockBattleStatus
		{
			NOT_OPEN,
			NOT_ENTRY,
			SUCCESS,
			BATTLE_INTERRUPTION
		}
	}

	public class ColosseumMockBattleRequestListLogic : RequestTypeBase<GameWebAPI.ReqData_ColosseumMockBattleRequestListLogic, GameWebAPI.RespData_ColosseumMockBattleRequestListLogic>
	{
		public ColosseumMockBattleRequestListLogic()
		{
			this.apiId = "080115";
		}
	}

	public class ReqData_ColosseumMatchingValidateLogic : WebAPI.SendBaseData
	{
		public int act;

		public string targetUserCode;

		public int isMockBattle;
	}

	public class RespData_ColosseumMatchingValidateLogic : WebAPI.ResponseData
	{
		public int resultCode;

		public GameWebAPI.RespData_ColosseumMatchingValidateLogic.ResultCode GetResultCode
		{
			get
			{
				return (GameWebAPI.RespData_ColosseumMatchingValidateLogic.ResultCode)this.resultCode;
			}
		}

		public enum ResultCode
		{
			SUCCESS = 1,
			TARGET_NOT_OPEN_COLOSSEUM = 92,
			TARGET_DISCONNECT,
			TARGET_BLACKLIST
		}
	}

	public class ColosseumMatchingValidateLogic : RequestTypeBase<GameWebAPI.ReqData_ColosseumMatchingValidateLogic, GameWebAPI.RespData_ColosseumMatchingValidateLogic>
	{
		public ColosseumMatchingValidateLogic()
		{
			this.apiId = "080117";
		}
	}
}
