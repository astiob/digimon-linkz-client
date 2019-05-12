using Master;
using MultiBattle.Tools;
using Quest;
using System;
using System.Collections;
using UnityEngine;

public class DataMng : MonoBehaviour
{
	private static DataMng instance;

	private GameWebAPI.WD_Req_DngResult result_from_battle;

	private GameWebAPI.RespDataCM_Login resp_data_cm_login;

	private GameWebAPI.RespDataCM_ABVersion resp_data_cm_abversion;

	private GameWebAPI.RespDataUS_GetPlayerInfo resp_data_us_playerinfo;

	private GameWebAPI.RespDataUS_GetSoulInfo resp_data_us_soulinfo;

	private GameWebAPI.RespDataFR_FriendList resp_data_fr_friendlist;

	private GameWebAPI.RespDataFR_FriendRequestList resp_data_fr_friendrequestlist;

	private GameWebAPI.RespDataFR_FriendUnapprovedList resp_data_fr_friendunapprovedlist;

	private GameWebAPI.RespDataFR_FriendInfo resp_data_fr_friendinfo;

	private GameWebAPI.RespDataBL_BlockSet resp_data_bl_blockset;

	private GameWebAPI.RespDataBL_BlockList resp_data_bl_blocklist;

	private GameWebAPI.RespDataPRF_Profile resp_data_prf_profile;

	private GameWebAPI.RespDataSH_Info resp_data_sh_info;

	private GameWebAPI.RespDataSH_ReqVerify resp_data_sh_reqverify;

	private GameWebAPI.RespDataSH_AgeCheck resp_data_sh_agecheck;

	private GameWebAPI.RespDataPR_PrizeReceiveHistory resp_data_pr_prizeReceiveHistory;

	private GameWebAPI.RespDataIN_InfoList resp_data_in_infoList;

	private GameWebAPI.RespDataMN_LaboExec resp_data_mn_laboexec;

	private GameWebAPI.RespDataMN_GetDeckList resp_data_mn_decklist;

	public static int MAX_SHOW_POPUP_INFO_IDS = 10;

	private DataMng.ResultUtilData resultUtilData = new DataMng.ResultUtilData();

	private Action<GameWebAPI.RespDataCP_Campaign, bool> _onCampaignUpdate = delegate(GameWebAPI.RespDataCP_Campaign x, bool b)
	{
	};

	private bool _campaignForceHide;

	private GameWebAPI.RespDataMA_BannerM resp_data_bannermaster;

	private StageGimmick stageGimmick = new StageGimmick();

	public static DataMng Instance()
	{
		return DataMng.instance;
	}

	protected virtual void Awake()
	{
		DataMng.instance = this;
		this.InitWD_ReqDngResult();
	}

	public string UserId
	{
		get
		{
			return this.RespDataCM_Login.playerInfo.userId;
		}
	}

	public string UserName
	{
		get
		{
			return this.RespDataUS_PlayerInfo.playerInfo.nickname;
		}
	}

	public GameWebAPI.WD_Req_DngResult WD_ReqDngResult
	{
		get
		{
			return this.result_from_battle;
		}
	}

	private void InitWD_ReqDngResult()
	{
		this.result_from_battle = new GameWebAPI.WD_Req_DngResult();
		this.result_from_battle.clear = 0;
		this.result_from_battle.aliveInfo = new int[]
		{
			1,
			1,
			1
		};
	}

	public void SetClearFlag(DataMng.ClearFlag clear)
	{
		this.result_from_battle.clear = (int)clear;
	}

	public void SetAliveFlag(bool alive1, bool alive2, bool alive3)
	{
		this.result_from_battle.aliveInfo = new int[]
		{
			(!alive1) ? 0 : 1,
			(!alive2) ? 0 : 1,
			(!alive3) ? 0 : 1
		};
	}

	public void SetAliveFlag(params bool[] alive123)
	{
		this.SetAliveFlag(alive123[0], alive123[1], alive123[2]);
	}

	public void SetEnemyAliveFlag(int[][] enemyAliveList)
	{
		this.result_from_battle.enemyAliveInfo = enemyAliveList;
	}

	public void SetClearRound(int clearRound)
	{
		this.result_from_battle.clearRound = clearRound;
	}

	public GameWebAPI.RespDataCM_Login RespDataCM_Login
	{
		get
		{
			return this.resp_data_cm_login;
		}
		set
		{
			this.resp_data_cm_login = value;
		}
	}

	public GameWebAPI.RespDataCM_ABVersion RespDataCM_ABVersion
	{
		get
		{
			return this.resp_data_cm_abversion;
		}
		set
		{
			this.resp_data_cm_abversion = value;
		}
	}

	public APIRequestTask RequestABVersion(bool requestRetry = true)
	{
		GameWebAPI.RequestCM_ABVersion requestCM_ABVersion = new GameWebAPI.RequestCM_ABVersion();
		requestCM_ABVersion.SetSendData = delegate(GameWebAPI.CM_Req_ABInfo param)
		{
			param.downloadType = 1;
		};
		requestCM_ABVersion.OnReceived = delegate(GameWebAPI.RespDataCM_ABVersion response)
		{
			this.RespDataCM_ABVersion = response;
		};
		GameWebAPI.RequestCM_ABVersion request = requestCM_ABVersion;
		return new APIRequestTask(request, requestRetry);
	}

	public GameWebAPI.RespDataUS_GetPlayerInfo RespDataUS_PlayerInfo
	{
		get
		{
			return this.resp_data_us_playerinfo;
		}
		set
		{
			this.resp_data_us_playerinfo = value;
		}
	}

	public void AddStone(int num)
	{
		this.RespDataUS_PlayerInfo.playerInfo.point += num;
	}

	public int GetStone()
	{
		return this.RespDataUS_PlayerInfo.playerInfo.point;
	}

	public void US_PlayerInfoSubMeatNum(int subMeat)
	{
		int num = int.Parse(this.RespDataUS_PlayerInfo.playerInfo.meatNum);
		num -= subMeat;
		this.RespDataUS_PlayerInfo.playerInfo.meatNum = num.ToString();
	}

	public void US_PlayerInfoSubChipNum(int subChip)
	{
		int num = int.Parse(this.RespDataUS_PlayerInfo.playerInfo.gamemoney);
		num -= subChip;
		this.RespDataUS_PlayerInfo.playerInfo.gamemoney = num.ToString();
	}

	public GameWebAPI.RespDataUS_GetSoulInfo RespDataUS_SoulInfo
	{
		get
		{
			return this.resp_data_us_soulinfo;
		}
		set
		{
			this.resp_data_us_soulinfo = value;
		}
	}

	public GameWebAPI.RespDataUS_GetGardenInfo RespDataUS_GardenInfo { get; set; }

	public GameWebAPI.RespDataFR_FriendList RespDataFR_FriendList
	{
		get
		{
			return this.resp_data_fr_friendlist;
		}
		set
		{
			this.resp_data_fr_friendlist = value;
		}
	}

	public GameWebAPI.RespDataFR_FriendRequestList RespDataFR_FriendRequestList
	{
		get
		{
			return this.resp_data_fr_friendrequestlist;
		}
		set
		{
			this.resp_data_fr_friendrequestlist = value;
		}
	}

	public GameWebAPI.RespDataFR_FriendUnapprovedList RespDataFR_FriendUnapprovedList
	{
		get
		{
			return this.resp_data_fr_friendunapprovedlist;
		}
		set
		{
			this.resp_data_fr_friendunapprovedlist = value;
		}
	}

	public GameWebAPI.RespDataFR_FriendInfo RespDataFR_FriendInfo
	{
		get
		{
			return this.resp_data_fr_friendinfo;
		}
		set
		{
			this.resp_data_fr_friendinfo = value;
		}
	}

	public GameWebAPI.RespDataBL_BlockSet RespDataBL_BlockSet
	{
		get
		{
			return this.resp_data_bl_blockset;
		}
		set
		{
			this.resp_data_bl_blockset = value;
		}
	}

	public GameWebAPI.RespDataBL_BlockList RespDataBL_BlockList
	{
		get
		{
			return this.resp_data_bl_blocklist;
		}
		set
		{
			this.resp_data_bl_blocklist = value;
		}
	}

	public GameWebAPI.RespDataPRF_Profile RespDataPRF_Profile
	{
		get
		{
			return this.resp_data_prf_profile;
		}
		set
		{
			this.resp_data_prf_profile = value;
		}
	}

	public void SetUserProfile(GameWebAPI.RespDataPRF_Profile profile)
	{
		this.resp_data_prf_profile = profile;
	}

	public GameWebAPI.RespDataSH_Info RespDataSH_Info
	{
		get
		{
			return this.resp_data_sh_info;
		}
		set
		{
			this.resp_data_sh_info = value;
		}
	}

	public GameWebAPI.RespDataSH_ReqVerify RespDataSH_ReqVerify
	{
		get
		{
			return this.resp_data_sh_reqverify;
		}
		set
		{
			this.resp_data_sh_reqverify = value;
		}
	}

	public GameWebAPI.RespDataSH_AgeCheck RespDataSH_AgeCheck
	{
		get
		{
			return this.resp_data_sh_agecheck;
		}
		set
		{
			this.resp_data_sh_agecheck = value;
		}
	}

	public GameWebAPI.RespDataPR_PrizeReceiveHistory RespDataPR_PrizeReceiveHistory
	{
		get
		{
			return this.resp_data_pr_prizeReceiveHistory;
		}
		set
		{
			this.resp_data_pr_prizeReceiveHistory = value;
		}
	}

	public GameWebAPI.RespDataIN_InfoList RespDataIN_InfoList
	{
		get
		{
			return this.resp_data_in_infoList;
		}
		set
		{
			this.resp_data_in_infoList = value;
		}
	}

	public GameWebAPI.RespDataMN_LaboExec RespDataMN_LaboExec
	{
		get
		{
			return this.resp_data_mn_laboexec;
		}
		set
		{
			this.resp_data_mn_laboexec = value;
		}
	}

	public GameWebAPI.RespDataMN_MedalInherit RespDataMN_MedalInherit { get; set; }

	public GameWebAPI.RespDataMN_GetDeckList RespDataMN_DeckList
	{
		get
		{
			return this.resp_data_mn_decklist;
		}
		set
		{
			this.resp_data_mn_decklist = value;
		}
	}

	public DataMng.ExperienceInfo GetExperienceInfo(int exp)
	{
		DataMng.ExperienceInfo experienceInfo = new DataMng.ExperienceInfo();
		GameWebAPI.RespDataMA_GetMonsterExperienceM.MonsterExperienceM[] monsterExperienceM = MasterDataMng.Instance().RespDataMA_MonsterExperienceM.monsterExperienceM;
		int num = 0;
		int num2 = 0;
		int i;
		for (i = 0; i < monsterExperienceM.Length; i++)
		{
			num = int.Parse(monsterExperienceM[i].experienceNum);
			if (i + 1 < monsterExperienceM.Length)
			{
				num2 = int.Parse(monsterExperienceM[i + 1].experienceNum);
				if (num <= exp && exp < num2)
				{
					break;
				}
			}
			else
			{
				num2 = num;
				exp = num;
			}
		}
		experienceInfo.exp = exp;
		experienceInfo.lev = int.Parse(monsterExperienceM[i].level);
		experienceInfo.expLev = exp - num;
		experienceInfo.expLevNext = num2 - exp;
		experienceInfo.expLevAll = num2 - num;
		return experienceInfo;
	}

	public int GetLvMAXExperienceInfo(int nowLv)
	{
		GameWebAPI.RespDataMA_GetMonsterExperienceM.MonsterExperienceM[] monsterExperienceM = MasterDataMng.Instance().RespDataMA_MonsterExperienceM.monsterExperienceM;
		return int.Parse(monsterExperienceM[nowLv - 1].experienceNum);
	}

	public int GetExpFromMeat(int meat_num)
	{
		return ConstValue.EXP_OF_ONE_MEAT * meat_num;
	}

	public GameWebAPI.RespDataMP_MyPage RespDataMP_MyPage { get; set; }

	public int ShowLoginBonusNumC { get; set; }

	public int ShowLoginBonusNumN { get; set; }

	public int ShowPopupInfoNum { get; set; }

	public Queue ShowPopupInfoIds { get; set; }

	public bool IsPopUpInformaiton { get; set; }

	public GameWebAPI.RespDataCM_LoginBonus RespDataCM_LoginBonus { get; set; }

	public GameWebAPI.RespDataCP_Campaign RespDataCP_Campaign { get; set; }

	public void DeckListUpdate(GameWebAPI.MN_Req_EditDeckList req)
	{
		GameWebAPI.RespDataMN_GetDeckList respDataMN_GetDeckList = this.resp_data_mn_decklist;
		for (int i = 0; i < req.deckData.Length; i++)
		{
			int[] array = req.deckData[i];
			for (int j = 0; j < array.Length; j++)
			{
				int num = j + 1;
				GameWebAPI.RespDataMN_GetDeckList.MonsterList[] monsterList = respDataMN_GetDeckList.deckList[i].monsterList;
				for (int k = 0; k < monsterList.Length; k++)
				{
					if (num.ToString() == monsterList[k].position)
					{
						monsterList[k].userMonsterId = array[j].ToString();
					}
				}
			}
		}
		respDataMN_GetDeckList.selectDeckNum = req.selectDeckNum.ToString();
		respDataMN_GetDeckList.favoriteDeckNum = req.favoriteDeckNum.ToString();
	}

	public DataMng.ResultUtilData GetResultUtilData()
	{
		return this.resultUtilData;
	}

	public APIRequestTask RequestMyPageData(bool requestRetry = true)
	{
		GameWebAPI.RequestMP_MyPage request = new GameWebAPI.RequestMP_MyPage
		{
			OnReceived = delegate(GameWebAPI.RespDataMP_MyPage response)
			{
				this.RespDataMP_MyPage = response;
				if (0 < this.RespDataMP_MyPage.userNewsCountList.isPopUpInformaiton)
				{
					this.IsPopUpInformaiton = true;
				}
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public APIRequestTask RequestLoginBonus(bool requestRetry = true)
	{
		GameWebAPI.RequestCM_LoginBonus request = new GameWebAPI.RequestCM_LoginBonus
		{
			OnReceived = delegate(GameWebAPI.RespDataCM_LoginBonus response)
			{
				this.RespDataCM_LoginBonus = response;
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public Action<GameWebAPI.RespDataCP_Campaign, bool> OnCampaignUpdate
	{
		get
		{
			return this._onCampaignUpdate;
		}
		set
		{
			this._onCampaignUpdate = value;
		}
	}

	public bool CampaignForceHide
	{
		get
		{
			return this._campaignForceHide;
		}
		set
		{
			this.OnCampaignUpdate(this.RespDataCP_Campaign, value);
			this._campaignForceHide = value;
		}
	}

	public APIRequestTask RequestCampaign(int campaignID, bool requestRetry)
	{
		GameWebAPI.RequestCP_Campaign request = new GameWebAPI.RequestCP_Campaign
		{
			SetSendData = delegate(GameWebAPI.CP_Req_Campaign param)
			{
				param.campaignId = campaignID;
			},
			OnReceived = delegate(GameWebAPI.RespDataCP_Campaign response)
			{
				this.RespDataCP_Campaign = response;
				this.OnCampaignUpdate(this.RespDataCP_Campaign, this.CampaignForceHide);
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public APIRequestTask RequestCampaignAll(bool requestRetry = true)
	{
		return this.RequestCampaign(0, requestRetry);
	}

	public void CheckCampaign(Action<int> finish, GameWebAPI.RespDataCP_Campaign.CampaignType[] campaign_type)
	{
		GameWebAPI.RespDataCP_Campaign respDataCP_Campaign = DataMng.Instance().RespDataCP_Campaign;
		bool isExistedData = false;
		GameWebAPI.RespDataCP_Campaign.CampaignInfo currentInfo = null;
		GameWebAPI.RespDataCP_Campaign.CampaignType currentType = GameWebAPI.RespDataCP_Campaign.CampaignType.Invalid;
		for (int i = 0; i < campaign_type.Length; i++)
		{
			GameWebAPI.RespDataCP_Campaign.CampaignInfo campaign = respDataCP_Campaign.GetCampaign(campaign_type[i], true);
			if (campaign != null)
			{
				isExistedData = true;
				currentInfo = campaign;
				currentType = campaign_type[i];
				break;
			}
		}
		bool isExistNewData = false;
		GameWebAPI.RespDataCP_Campaign.CampaignInfo newInfo = null;
		GameWebAPI.RequestCP_Campaign request = new GameWebAPI.RequestCP_Campaign
		{
			SetSendData = delegate(GameWebAPI.CP_Req_Campaign param)
			{
				param.campaignId = (int)currentType;
			},
			OnReceived = delegate(GameWebAPI.RespDataCP_Campaign response)
			{
				if (currentType != GameWebAPI.RespDataCP_Campaign.CampaignType.Invalid)
				{
					GameWebAPI.RespDataCP_Campaign.CampaignInfo campaign2 = response.GetCampaign(currentType, false);
					if (campaign2 != null)
					{
						isExistNewData = true;
						newInfo = campaign2;
					}
				}
				else
				{
					for (int j = 0; j < campaign_type.Length; j++)
					{
						GameWebAPI.RespDataCP_Campaign.CampaignInfo campaign3 = response.GetCampaign(campaign_type[j], false);
						if (campaign3 != null)
						{
							isExistNewData = true;
							newInfo = campaign3;
							break;
						}
					}
				}
			}
		};
		base.StartCoroutine(request.RunOneTime(delegate()
		{
			if (isExistedData && isExistNewData && currentInfo != null && newInfo != null && !currentInfo.IsEqualInfo(newInfo))
			{
				isExistedData = false;
			}
			if (isExistedData && !isExistNewData)
			{
				if (finish != null)
				{
					finish(1);
				}
			}
			else if (!isExistedData && isExistNewData)
			{
				if (finish != null)
				{
					finish(2);
				}
			}
			else if (finish != null)
			{
				finish(0);
			}
		}, delegate(Exception nop)
		{
			if (finish != null)
			{
				finish(-1);
			}
		}, null));
	}

	public void CampaignErrorCloseAllCommonDialog(bool is_finished, Action finish)
	{
		if (is_finished)
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(delegate(int r)
			{
				if (finish != null)
				{
					finish();
				}
			}, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("CampaignEndTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("CampaignEndInfo");
		}
		else
		{
			CMD_ModalMessage cmd_ModalMessage2 = GUIMain.ShowCommonDialog(delegate(int r)
			{
				if (finish != null)
				{
					finish();
				}
			}, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage2.Title = StringMaster.GetString("CampaignStartTitle");
			cmd_ModalMessage2.Info = StringMaster.GetString("CampaignStartInfo");
		}
	}

	public void ReloadCampaign(Action finish = null)
	{
		APIRequestTask task = DataMng.Instance().RequestCampaignAll(true);
		base.StartCoroutine(task.Run(delegate
		{
			GUIManager.CloseAllCommonDialog(null);
			if (finish != null)
			{
				finish();
			}
		}, null, null));
	}

	public GameWebAPI.RespDataCP_Campaign.CampaignInfo GetCampaignInfo(GameWebAPI.RespDataCP_Campaign.CampaignType campaign_type)
	{
		if (this.RespDataCP_Campaign != null)
		{
			return this.RespDataCP_Campaign.GetCampaign(campaign_type, false);
		}
		return null;
	}

	public APIRequestTask RequestNews(bool requestRetry = true)
	{
		GameWebAPI.RequestIN_InfoList requestIN_InfoList = new GameWebAPI.RequestIN_InfoList();
		requestIN_InfoList.SetSendData = delegate(GameWebAPI.SendDataIN_InfoList requestParam)
		{
			int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
			requestParam.countryCode = countryCode;
		};
		requestIN_InfoList.OnReceived = delegate(GameWebAPI.RespDataIN_InfoList response)
		{
			this.RespDataIN_InfoList = response;
		};
		GameWebAPI.RequestIN_InfoList request = requestIN_InfoList;
		return new APIRequestTask(request, requestRetry);
	}

	public GameWebAPI.RespDataMA_BannerM RespData_BannerMaster
	{
		get
		{
			return this.resp_data_bannermaster;
		}
		set
		{
			this.resp_data_bannermaster = value;
		}
	}

	public APIRequestTask RequestBannerMaster(bool requestRetry = true)
	{
		GameWebAPI.RequestMA_BannerMaster requestMA_BannerMaster = new GameWebAPI.RequestMA_BannerMaster();
		requestMA_BannerMaster.SetSendData = delegate(GameWebAPI.RequestMA_BannerM requestParam)
		{
			int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
			requestParam.countryCode = countryCode;
		};
		requestMA_BannerMaster.OnReceived = delegate(GameWebAPI.RespDataMA_BannerM response)
		{
			this.RespData_BannerMaster = response;
		};
		GameWebAPI.RequestMA_BannerMaster request = requestMA_BannerMaster;
		return new APIRequestTask(request, requestRetry);
	}

	public APIRequestTask RequestAgeCheck(string productId, string ageType, bool requestRetry = true)
	{
		GameWebAPI.RequestSH_AgeCheck request = new GameWebAPI.RequestSH_AgeCheck
		{
			SetSendData = delegate(GameWebAPI.SH_Req_AgeCheck param)
			{
				param.productId = productId;
				param.ageType = ageType;
			},
			OnReceived = delegate(GameWebAPI.RespDataSH_AgeCheck response)
			{
				this.RespDataSH_AgeCheck = response;
			}
		};
		return new APIRequestTask(request, requestRetry);
	}

	public bool IsBattleFailShowDungeon
	{
		get
		{
			if (ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart != null)
			{
				int num = int.Parse(ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart.worldDungeonId);
				return num >= ConstValue.BATTLEFAIL_TIPS_SHOW_DNG_NUM;
			}
			return false;
		}
	}

	public GameWebAPI.RespData_WorldMultiStartInfo RespData_WorldMultiStartInfo { get; set; }

	public static GameWebAPI.RespData_WorldMultiStartInfo.LuckDrop ExchangeMultiLuckDrop(GameWebAPI.RespDataWD_DungeonStart.LuckDrop soloPlayLuckDrop)
	{
		if (soloPlayLuckDrop == null)
		{
			return null;
		}
		return new GameWebAPI.RespData_WorldMultiStartInfo.LuckDrop
		{
			dropBoxType = soloPlayLuckDrop.dropBoxType,
			assetCategoryId = soloPlayLuckDrop.assetCategoryId,
			assetValue = soloPlayLuckDrop.assetValue,
			assetNum = soloPlayLuckDrop.assetNum,
			userId = string.Empty
		};
	}

	public static GameWebAPI.RespData_WorldMultiStartInfo.LuckDrop[] ExchangeMultiLuckDrop(GameWebAPI.RespDataWD_DungeonStart.LuckDrop[] soloPlayLuckDrop)
	{
		GameWebAPI.RespData_WorldMultiStartInfo.LuckDrop[] array = new GameWebAPI.RespData_WorldMultiStartInfo.LuckDrop[soloPlayLuckDrop.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = DataMng.ExchangeMultiLuckDrop(soloPlayLuckDrop[i]);
		}
		return array;
	}

	public int GetPartyIndex(int index)
	{
		MultiBattleData multiBattleData = ClassSingleton<MultiBattleData>.Instance;
		string[] playerUserMonsterIds = multiBattleData.PlayerUserMonsterIds;
		GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = this.RespData_WorldMultiStartInfo;
		GameWebAPI.RespData_WorldMultiStartInfo.Party[] party = respData_WorldMultiStartInfo.party;
		for (int i = 0; i < party.Length; i++)
		{
			if (this.IsExistUserMonster(party[i].userMonsters, playerUserMonsterIds[index]))
			{
				return i;
			}
		}
		return -1;
	}

	public int GetMonsterIndex(int index)
	{
		MultiBattleData multiBattleData = ClassSingleton<MultiBattleData>.Instance;
		string[] playerUserMonsterIds = multiBattleData.PlayerUserMonsterIds;
		GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = this.RespData_WorldMultiStartInfo;
		GameWebAPI.RespData_WorldMultiStartInfo.Party[] party = respData_WorldMultiStartInfo.party;
		int partyIndex = this.GetPartyIndex(index);
		GameWebAPI.Common_MonsterData[] userMonsters = party[partyIndex].userMonsters;
		for (int i = 0; i < userMonsters.Length; i++)
		{
			if (userMonsters[i] != null)
			{
				if (userMonsters[i].userMonsterId == playerUserMonsterIds[index])
				{
					return i;
				}
			}
		}
		return -1;
	}

	private bool IsExistUserMonster(GameWebAPI.Common_MonsterData[] monstersData, string userMonsterId)
	{
		foreach (GameWebAPI.Common_MonsterData common_MonsterData in monstersData)
		{
			if (common_MonsterData != null)
			{
				if (common_MonsterData.userMonsterId == userMonsterId)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsReleaseColosseum { get; set; }

	public GameWebAPI.RespData_ColosseumInfoLogic RespData_ColosseumInfo { get; set; }

	public GameWebAPI.RespDataCL_GetColosseumReward RespData_ColosseumReward { get; set; }

	public StageGimmick StageGimmick
	{
		get
		{
			return this.stageGimmick;
		}
	}

	public string GetAssetTitle(string categoryId, string soulId)
	{
		string result = string.Empty;
		if (int.Parse(categoryId) == 14)
		{
			GameWebAPI.RespDataMA_GetSoulM respDataMA_SoulM = MasterDataMng.Instance().RespDataMA_SoulM;
			if (respDataMA_SoulM != null)
			{
				GameWebAPI.RespDataMA_GetSoulM.SoulM soul = respDataMA_SoulM.GetSoul(soulId);
				if (soul != null)
				{
					result = soul.soulName;
				}
			}
		}
		else
		{
			GameWebAPI.RespDataMA_GetAssetCategoryM respDataMA_AssetCategoryM = MasterDataMng.Instance().RespDataMA_AssetCategoryM;
			if (respDataMA_AssetCategoryM != null)
			{
				GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = respDataMA_AssetCategoryM.GetAssetCategory(categoryId);
				if (assetCategory != null)
				{
					result = assetCategory.assetTitle;
				}
			}
		}
		return result;
	}

	public enum ClearFlag
	{
		Defeat,
		Win,
		Excess
	}

	public class ExperienceInfo
	{
		public int exp;

		public int lev;

		public int expLev;

		public int expLevNext;

		public int expLevAll;
	}

	public class ResultUtilData
	{
		public GameWebAPI.WD_Req_DngStart last_dng_req;

		public void SetLastDngReq(string dungeonId, string deckNum = "-1", string userDungeonTicketId = "-1")
		{
			this.last_dng_req = new GameWebAPI.WD_Req_DngStart();
			this.last_dng_req.dungeonId = dungeonId;
			this.last_dng_req.deckNum = deckNum;
			this.last_dng_req.userDungeonTicketId = userDungeonTicketId;
		}

		public void ClearLastDngReq()
		{
			this.last_dng_req = null;
		}

		public GameWebAPI.WD_Req_DngStart GetLastDngReq()
		{
			return this.last_dng_req;
		}
	}
}
