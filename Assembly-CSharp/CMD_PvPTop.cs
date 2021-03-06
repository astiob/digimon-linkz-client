﻿using Colosseum.DeckUI;
using Master;
using MultiBattle.Tools;
using PvP;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CMD_PvPTop : CMD
{
	private readonly float SWITCH_COMMENT_INTERVAL_TIME = 5f;

	private static CMD_PvPTop instance;

	[Header("以下,PvPTopに関する設定")]
	[SerializeField]
	private UILabel lastTimeLabel;

	[SerializeField]
	private GameObject lastTimeObj;

	[SerializeField]
	private UILabel commentLabel;

	[SerializeField]
	private UILabel userNameLabel;

	[SerializeField]
	private UITexture titleIcon;

	[SerializeField]
	private UILabel wartimeCareerLabel;

	[SerializeField]
	private UILabel nextRankLabel;

	[SerializeField]
	private UILabel nowScore;

	[SerializeField]
	private GameObject staminaDispGroup;

	[SerializeField]
	private GameObject rankingDispGroup;

	[SerializeField]
	private GameObject bpDispGroup;

	[SerializeField]
	private UILabel aggregateLabel;

	[Header("全国バトルボタン")]
	[SerializeField]
	private GUICollider nationwideCollider;

	[SerializeField]
	private UISprite nationwideSprite;

	[SerializeField]
	private UISprite nationwideFrameSprite;

	[SerializeField]
	private GameObject nationwideText;

	[SerializeField]
	private GameObject nationwideTextG;

	[Header("拡張全国バトルボタン")]
	[SerializeField]
	private GUICollider nationwideExtraCollider;

	[SerializeField]
	private UISprite nationwideExtraSprite;

	[SerializeField]
	private UISprite nationwideExtraFrameSprite;

	[SerializeField]
	private GameObject nationwideExtraText;

	[SerializeField]
	private GameObject nationwideExtraTextG;

	[SerializeField]
	private UISprite rankSprite;

	[SerializeField]
	private GUICollider mockBattleCollider;

	[SerializeField]
	private UISprite mockBattleSprite;

	[SerializeField]
	private UISprite mockBattleFrameSprite;

	[SerializeField]
	private GameObject mockBattleText;

	[SerializeField]
	private GameObject mockBattleTextG;

	[SerializeField]
	private GameObject aggregateMarkObj;

	[SerializeField]
	private UITexture thumbnail;

	[SerializeField]
	private UILabel periodLabel;

	[SerializeField]
	private UserStamina userStamina;

	[SerializeField]
	private UILabel lbColosseumBattlePoint;

	[SerializeField]
	private UILabel lbColosseumBattleRanking;

	[SerializeField]
	private UILabel staminaCostLabel;

	[SerializeField]
	private UILabel staminaCostLabelExtra;

	[SerializeField]
	private UILabel lbRewardPercentage;

	[Header("スケジュール")]
	[SerializeField]
	private GameObject goTimeSchedule;

	[Header("以下,○回目ラベル(昇順)")]
	[SerializeField]
	private UILabel[] spanTimeLabelArray = new UILabel[0];

	[Header("以下,タイムスケジュールの開始時間ラベル(昇順)")]
	[SerializeField]
	private UILabel[] startTimeLabelArray = new UILabel[0];

	[Header("以下,タイムスケジュールの'~'ラベル(昇順)")]
	[SerializeField]
	private UILabel[] fromMarkLabelArray = new UILabel[0];

	[Header("以下,タイムスケジュールの終了時間ラベル(昇順)")]
	[SerializeField]
	private UILabel[] endTimeLabelArray = new UILabel[0];

	[Header("以下,タイムスケジュールの開催中ラベル(昇順)")]
	[SerializeField]
	private GameObject[] inSessionObjArray = new GameObject[0];

	[Header("以下,ヘルプの画像パス(表示順)")]
	[SerializeField]
	private List<string> helpImagePathList = new List<string>();

	[Header("以下,UIテキスト")]
	[SerializeField]
	private UILabel toColiseumExitText;

	[SerializeField]
	private UILabel wartimeCareerText;

	[SerializeField]
	private UILabel rankListText;

	[SerializeField]
	private UILabel timeGuidanceText;

	[SerializeField]
	private UILabel[] currentlyBeingHeldTexts;

	[SerializeField]
	private UILabel coliseumDescriptionText;

	[SerializeField]
	private UILabel notesText;

	[SerializeField]
	private UITexture backgroundImg;

	[SerializeField]
	private UITexture bpIcon;

	private GameWebAPI.ColosseumUserStatus colosseumUserStatus;

	private GameWebAPI.RespData_ColosseumUserStatusLogic.ColosseumBattleRecord colosseumBattleRecord;

	private int freeCostBattleCount;

	private DateTime? endDateTime;

	private DateTime? nextDateTime;

	private List<GUIListPartsRank.RankData> rankDataList = new List<GUIListPartsRank.RankData>();

	private List<CMD_Tips.TipsM.Tips> displayCommentDataList = new List<CMD_Tips.TipsM.Tips>();

	private int displayCommentDataListIndex;

	private DateTime? cancelPenaltyDateTime;

	private GUIListPartsRank.RankData currentRankData = new GUIListPartsRank.RankData();

	private bool isToBattle;

	private int needStamina;

	private int normalBattleStamina;

	private bool isAggregate;

	private Dictionary<string, Texture2D> thumbnails;

	[HideInInspector]
	public bool isExtraBattle;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	public static CMD_PvPTop Instance
	{
		get
		{
			return CMD_PvPTop.instance;
		}
	}

	public GameWebAPI.ColosseumUserStatus ColosseumUserStatus
	{
		get
		{
			return this.colosseumUserStatus;
		}
	}

	public bool IsToBattle
	{
		set
		{
			this.isToBattle = value;
		}
	}

	public int NeedStamina
	{
		get
		{
			return this.needStamina;
		}
	}

	public bool IsAggregate
	{
		get
		{
			return this.isAggregate;
		}
	}

	protected override void Awake()
	{
		if (CMD_PvPTop.instance == null)
		{
			CMD_PvPTop.instance = this;
			base.Awake();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		base.StartCoroutine(this.Initialize(delegate(bool isSuccess)
		{
			if (isSuccess)
			{
				this.ShowDLG();
				this.<Show>__BaseCallProxy0(f, sizeX, sizeY, aT);
				if (GUIScreenHome.isManualScreenFadeIn)
				{
					this.StartCoroutine(this.FadeIn());
				}
				else
				{
					RestrictionInput.EndLoad();
				}
			}
			else
			{
				this.<ClosePanel>__BaseCallProxy1(false);
				RestrictionInput.EndLoad();
			}
		}));
	}

	public override void ClosePanel(bool animation = true)
	{
		if (!this.isToBattle)
		{
			SoundMng.Instance().PlayGameBGM("bgm_201");
			base.SetForceReturnValue(100);
		}
		else
		{
			base.SetForceReturnValue(0);
		}
		if (CMD_PvPTop.instance == this)
		{
			CMD_PvPTop.instance = null;
		}
		base.ClosePanel(animation);
	}

	private IEnumerator FadeIn()
	{
		GUIBase uiHome = GUIManager.GetGUI("UIHome");
		while (uiHome == null)
		{
			uiHome = GUIManager.GetGUI("UIHome");
			yield return null;
		}
		GUIScreenHome screenHome = uiHome.GetComponent<GUIScreenHome>();
		while (!screenHome.isFinishedStartLoading)
		{
			yield return null;
		}
		yield return new WaitForSeconds(0.5f);
		GUIScreenHome guiscreenHome = screenHome;
		if (CMD_PvPTop.<>f__mg$cache0 == null)
		{
			CMD_PvPTop.<>f__mg$cache0 = new Action(RestrictionInput.EndLoad);
		}
		base.StartCoroutine(guiscreenHome.StartScreenFadeIn(CMD_PvPTop.<>f__mg$cache0));
		yield break;
	}

	private IEnumerator Initialize(Action<bool> onInitialized)
	{
		int bpItemId = 2;
		NGUIUtil.ChangeUITextureFromFile(this.bpIcon, MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(bpItemId.ToString()).GetSmallImagePath(), false);
		SoundMng.Instance().PlayGameBGM("bgm_203");
		Singleton<TCPUtil>.Instance.InitializeUniqueRequestId();
		GameWebAPI.ColosseumInfoLogic colosseumInfoLogic = new GameWebAPI.ColosseumInfoLogic();
		colosseumInfoLogic.OnReceived = delegate(GameWebAPI.RespData_ColosseumInfoLogic resData)
		{
			DataMng.Instance().RespData_ColosseumInfo = resData;
		};
		GameWebAPI.ColosseumInfoLogic request = colosseumInfoLogic;
		APIRequestTask task = new APIRequestTask(request, true);
		GameWebAPI.RespData_ColosseumUserStatusLogic responseStatus = null;
		task.Add(PvPUtility.RequestColosseumUserStatus(PvPUtility.RequestUserStatusType.MY, delegate(GameWebAPI.RespData_ColosseumUserStatusLogic res)
		{
			PvPUtility.SetPvPTopNoticeCode(res);
			responseStatus = res;
		}));
		yield return base.StartCoroutine(task.Run(null, null, null));
		GameWebAPI.RequestUS_UserStatus requestUS_UserStatus = new GameWebAPI.RequestUS_UserStatus();
		requestUS_UserStatus.SetSendData = delegate(GameWebAPI.PlayerInfoSendData param)
		{
			param.keys = "recovery";
		};
		requestUS_UserStatus.OnReceived = delegate(GameWebAPI.RespDataUS_GetPlayerInfo response)
		{
			DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.recovery = response.playerInfo.recovery;
			DkLog.W("stamina : " + DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.stamina, false);
		};
		GameWebAPI.RequestUS_UserStatus statusrRequest = requestUS_UserStatus;
		yield return base.StartCoroutine(statusrRequest.Run(null, null, null));
		bool isSuccess = false;
		IEnumerator ie = this.SetUserEntryStatus(responseStatus, delegate(bool result, GameWebAPI.RespData_ColosseumUserStatusLogic status)
		{
			isSuccess = result;
			responseStatus = status;
		});
		yield return base.StartCoroutine(ie);
		if (!isSuccess)
		{
			onInitialized(false);
			yield break;
		}
		if (!this.SetUserStatus(responseStatus))
		{
			RestrictionInput.EndLoad();
			bool isOpen = false;
			isOpen = AlertManager.ShowAlertDialog(delegate(int noop)
			{
				isOpen = true;
			}, "E-PV99");
			while (isOpen)
			{
				yield return null;
			}
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			onInitialized(false);
			yield break;
		}
		yield return base.StartCoroutine(this.SetMockBattleStatus(responseStatus.GetMockBattleStatusEnum, delegate(bool result)
		{
			isSuccess = result;
		}));
		if (!isSuccess)
		{
			onInitialized(false);
			yield break;
		}
		this.isAggregate = (GameWebAPI.RespData_ColosseumUserStatusLogic.ResultCode.AGGREGATE == responseStatus.GetResultCodeEnum);
		this.CreateRankDataList();
		this.InitializeUI();
		isSuccess = false;
		GameWebAPI.RespDataCL_GetColosseumReward responseReward = null;
		GameWebAPI.RequestCL_GetColosseumReward requestCL_GetColosseumReward = new GameWebAPI.RequestCL_GetColosseumReward();
		requestCL_GetColosseumReward.SetSendData = delegate(GameWebAPI.SendDataCL_GetColosseumReward param)
		{
			param.act = "1";
		};
		requestCL_GetColosseumReward.OnReceived = delegate(GameWebAPI.RespDataCL_GetColosseumReward resData)
		{
			responseReward = resData;
		};
		GameWebAPI.RequestCL_GetColosseumReward requestReward = requestCL_GetColosseumReward;
		yield return base.StartCoroutine(requestReward.RunOneTime(delegate()
		{
			if (responseReward != null && responseReward.ExistReward())
			{
				DataMng.Instance().RespData_ColosseumReward = null;
				this.ShowColosseumReward(responseReward, delegate(int value)
				{
					this.ShowRankDownDialog();
				});
			}
			isSuccess = true;
		}, null, null));
		if (!isSuccess)
		{
			onInitialized(false);
			yield break;
		}
		if (responseReward == null || !responseReward.ExistReward())
		{
			this.ShowRankDownDialog();
		}
		onInitialized(true);
		yield break;
	}

	private IEnumerator SetUserEntryStatus(GameWebAPI.RespData_ColosseumUserStatusLogic colosseumStatus, Action<bool, GameWebAPI.RespData_ColosseumUserStatusLogic> onFinished)
	{
		CMD_PvPTop.<SetUserEntryStatus>c__Iterator2.<SetUserEntryStatus>c__AnonStorey9 <SetUserEntryStatus>c__AnonStorey = new CMD_PvPTop.<SetUserEntryStatus>c__Iterator2.<SetUserEntryStatus>c__AnonStorey9();
		bool isSuccess = true;
		APIRequestTask task = null;
		<SetUserEntryStatus>c__AnonStorey.status = colosseumStatus;
		GameWebAPI.RespData_ColosseumUserStatusLogic.ResultCode getResultCodeEnum = colosseumStatus.GetResultCodeEnum;
		if (getResultCodeEnum != GameWebAPI.RespData_ColosseumUserStatusLogic.ResultCode.NOT_ENTRY)
		{
			if (getResultCodeEnum == GameWebAPI.RespData_ColosseumUserStatusLogic.ResultCode.BATTLE_INTERRUPTION || getResultCodeEnum == GameWebAPI.RespData_ColosseumUserStatusLogic.ResultCode.BATTLE_INTERRUPTION_WIN)
			{
				GameWebAPI.ReqData_ColosseumBattleEndLogic.BattleResult result = PvPUtility.GetBattleResult(colosseumStatus.GetResultCodeEnum);
				task = PvPUtility.RequestColosseumBattleEnd(result, GameWebAPI.ReqData_ColosseumBattleEndLogic.BattleMode.NORMAL_BATTLE);
				task.Add(PvPUtility.RequestColosseumUserStatus(PvPUtility.RequestUserStatusType.MY, delegate(GameWebAPI.RespData_ColosseumUserStatusLogic res)
				{
					PvPUtility.SetPvPTopNoticeCode(res);
					<SetUserEntryStatus>c__AnonStorey.status = res;
				}));
				yield return base.StartCoroutine(task.Run(null, null, null));
			}
		}
		else
		{
			GameWebAPI.RespDataCL_ColosseumEntry entry = null;
			task = PvPUtility.RequestColosseumEntry(DataMng.Instance().RespData_ColosseumInfo, delegate(GameWebAPI.RespDataCL_ColosseumEntry res)
			{
				entry = res;
			}, false, false);
			yield return base.StartCoroutine(task.Run(null, null, null));
			isSuccess = PvPUtility.CopyUserEntryStatus(<SetUserEntryStatus>c__AnonStorey.status, entry);
		}
		onFinished(isSuccess, <SetUserEntryStatus>c__AnonStorey.status);
		yield break;
	}

	private IEnumerator SetMockBattleStatus(GameWebAPI.RespData_ColosseumUserStatusLogic.MockBattleStatus mockBattleStatus, Action<bool> onFinished)
	{
		bool isSuccess = false;
		APIRequestTask task = null;
		switch (mockBattleStatus)
		{
		case GameWebAPI.RespData_ColosseumUserStatusLogic.MockBattleStatus.NOT_OPEN:
			this.SetActiveMockBattleButton(false);
			isSuccess = true;
			break;
		case GameWebAPI.RespData_ColosseumUserStatusLogic.MockBattleStatus.NOT_ENTRY:
			task = PvPUtility.RequestMockBattleEntry(false);
			yield return base.StartCoroutine(task.Run(delegate
			{
				isSuccess = true;
			}, null, null));
			break;
		default:
			isSuccess = true;
			break;
		case GameWebAPI.RespData_ColosseumUserStatusLogic.MockBattleStatus.BATTLE_INTERRUPTION:
			task = PvPUtility.RequestColosseumBattleEnd(GameWebAPI.ReqData_ColosseumBattleEndLogic.BattleResult.DEFEAT, GameWebAPI.ReqData_ColosseumBattleEndLogic.BattleMode.MOCK_BATTLE);
			yield return base.StartCoroutine(task.Run(null, null, null));
			break;
		}
		onFinished(isSuccess);
		yield break;
	}

	private bool SetUserStatus(GameWebAPI.RespData_ColosseumUserStatusLogic colosseumStatus)
	{
		bool result = false;
		if (colosseumStatus != null && colosseumStatus.userStatus != null)
		{
			this.colosseumUserStatus = colosseumStatus.userStatus;
			this.colosseumBattleRecord = colosseumStatus.battleRecord;
			this.colosseumUserStatus.nickname = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.nickname;
			this.freeCostBattleCount = colosseumStatus.freeCostBattleCount;
			this.lbColosseumBattleRanking.text = ((colosseumStatus.ranking != 0) ? string.Format(StringMaster.GetString("ColosseumRankLabel"), colosseumStatus.ranking.ToString()) : StringMaster.GetString("ColosseumRankOutside"));
			if (!string.IsNullOrEmpty(colosseumStatus.penalty))
			{
				this.cancelPenaltyDateTime = new DateTime?(DateTime.Parse(colosseumStatus.penalty));
			}
			result = true;
		}
		return result;
	}

	private void InitializeUI()
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("ChatTabColosseum"));
		this.toColiseumExitText.text = StringMaster.GetString("PvPTopUI-1");
		this.wartimeCareerText.text = StringMaster.GetString("PvPTopUI-2");
		this.rankListText.text = StringMaster.GetString("PvPTopUI-3");
		this.timeGuidanceText.text = StringMaster.GetString("PvPTopUI-4");
		foreach (UILabel uilabel in this.currentlyBeingHeldTexts)
		{
			uilabel.text = StringMaster.GetString("PvPTopUI-5");
		}
		this.coliseumDescriptionText.text = StringMaster.GetString("PvPTopUI-6");
		this.notesText.text = StringMaster.GetString("SystemCaution");
		if (this.isAggregate)
		{
			this.colosseumUserStatus = new GameWebAPI.ColosseumUserStatus();
			this.colosseumUserStatus.nickname = DataMng.Instance().UserName;
			this.wartimeCareerLabel.text = StringMaster.GetString("PvpAggregate");
		}
		else
		{
			this.wartimeCareerLabel.text = string.Format(StringMaster.GetString("ColosseumScore"), this.colosseumUserStatus.winTotal, this.colosseumUserStatus.loseTotal);
		}
		this.userNameLabel.text = this.colosseumUserStatus.nickname;
		TitleDataMng.SetTitleIcon(this.colosseumUserStatus.titleId, this.titleIcon);
		this.lbColosseumBattlePoint.text = Singleton<UserDataMng>.Instance.GetUserItemNumByItemId(2).ToString();
		if (DataMng.Instance().RespData_ColosseumInfo.eventInfo != null && DataMng.Instance().RespData_ColosseumInfo.eventInfo.backgroundImg.Length > 0)
		{
			string path = AssetDataMng.GetWebAssetImagePath() + "/events/" + DataMng.Instance().RespData_ColosseumInfo.eventInfo.backgroundImg;
			AppCoroutine.Start(this.DownloadBGTexture(path), false);
			base.PartsTitle.SetTitle(StringMaster.GetString("ChatTabColosseum") + StringMaster.GetString("ColosseumTitleOnEvent"));
		}
		this.SetRankSprite();
		this.SetRankGuage();
		this.SetTimeSchedule();
		this.SetPeriod();
		this.SetStamina();
		this.LastTimer();
		this.GetCommentData();
		this.LoadNaviThumb();
		this.DisplayComment();
	}

	private IEnumerator DownloadBGTexture(string path)
	{
		yield return TextureManager.instance.Load(path, delegate(Texture2D tex)
		{
			if (null != tex)
			{
				this.backgroundImg.mainTexture = tex;
			}
		}, 30f, true);
		yield break;
	}

	private void SetActiveNationwideButton(bool IsActive)
	{
		this.nationwideCollider.activeCollider = IsActive;
		this.nationwideSprite.spriteName = ((!IsActive) ? "Common02_Btn_Gray" : "Common02_Btn_Blue");
		this.nationwideFrameSprite.color = ((!IsActive) ? Color.gray : Color.white);
		this.nationwideText.SetActive(IsActive);
		this.nationwideTextG.SetActive(!IsActive);
	}

	private void SetActiveNationwideExtraButton(bool IsActive)
	{
		this.nationwideExtraCollider.activeCollider = IsActive;
		this.nationwideExtraSprite.spriteName = ((!IsActive) ? "Common02_Btn_Gray" : "Common02_Btn_Red");
		this.nationwideExtraFrameSprite.color = ((!IsActive) ? Color.gray : Color.white);
		this.nationwideExtraText.SetActive(IsActive);
		this.nationwideExtraTextG.SetActive(!IsActive);
	}

	private void SetActiveMockBattleButton(bool IsActive)
	{
		this.mockBattleCollider.activeCollider = IsActive;
		this.mockBattleSprite.spriteName = ((!IsActive) ? "Common02_Btn_Gray" : "Common02_Btn_Green");
		this.mockBattleFrameSprite.color = ((!IsActive) ? Color.gray : Color.white);
		this.mockBattleText.SetActive(IsActive);
		this.mockBattleTextG.SetActive(!IsActive);
	}

	private void SetTimeSchedule()
	{
		GameWebAPI.RespData_ColosseumInfoLogic colosseumInfo = DataMng.Instance().RespData_ColosseumInfo;
		if (DataMng.Instance().RespData_ColosseumInfo.openAllDay == 1)
		{
			this.goTimeSchedule.SetActive(false);
		}
		GameWebAPI.RespDataMA_ColosseumM.Colosseum colosseum = new GameWebAPI.RespDataMA_ColosseumM.Colosseum();
		GameWebAPI.RespDataMA_ColosseumM respDataMA_ColosseumMaster = MasterDataMng.Instance().RespDataMA_ColosseumMaster;
		if (respDataMA_ColosseumMaster != null)
		{
			colosseum = respDataMA_ColosseumMaster.colosseumM.Where((GameWebAPI.RespDataMA_ColosseumM.Colosseum data) => data.colosseumId == colosseumInfo.colosseumId.ToString()).SingleOrDefault<GameWebAPI.RespDataMA_ColosseumM.Colosseum>();
		}
		GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule[] array;
		if (MasterDataMng.Instance().RespDataMA_ColosseumTimeScheduleMaster == null)
		{
			array = new GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule[0];
		}
		else if (DataMng.Instance().RespData_ColosseumInfo != null && DataMng.Instance().RespData_ColosseumInfo.colosseumId > 0)
		{
			array = MasterDataMng.Instance().RespDataMA_ColosseumTimeScheduleMaster.colosseumTimeScheduleM.Where((GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule schedule) => schedule.colosseumId == DataMng.Instance().RespData_ColosseumInfo.colosseumId.ToString()).ToArray<GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule>();
		}
		else
		{
			array = new GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule[0];
		}
		Array.Sort<GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule>(array, (GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule a, GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule b) => int.Parse(a.colosseumTimeScheduleId) - int.Parse(b.colosseumTimeScheduleId));
		this.endDateTime = (this.nextDateTime = null);
		foreach (GameObject gameObject in this.inSessionObjArray)
		{
			gameObject.SetActive(false);
		}
		string @string = StringMaster.GetString("ColosseumTimes");
		for (int j = 0; j < this.spanTimeLabelArray.Length; j++)
		{
			UILabel uilabel = this.spanTimeLabelArray[j];
			uilabel.text = string.Format(@string, j + 1);
		}
		foreach (UILabel uilabel2 in this.startTimeLabelArray)
		{
			uilabel2.gameObject.SetActive(false);
		}
		foreach (UILabel uilabel3 in this.endTimeLabelArray)
		{
			uilabel3.text = StringMaster.GetString("ColosseumCloseDay");
		}
		foreach (UILabel uilabel4 in this.fromMarkLabelArray)
		{
			uilabel4.gameObject.SetActive(false);
		}
		int num = 0;
		while (num < array.Length && num < this.startTimeLabelArray.Length && num < this.endTimeLabelArray.Length)
		{
			DateTime dateTime = DateTime.Parse(array[num].startHour);
			DateTime dateTime2 = DateTime.Parse(array[num].endHour);
			DateTime? dateTime3 = null;
			if (num + 1 < array.Length && num + 1 < this.startTimeLabelArray.Length)
			{
				dateTime3 = new DateTime?(DateTime.Parse(array[num + 1].startHour));
			}
			this.startTimeLabelArray[num].gameObject.SetActive(true);
			this.startTimeLabelArray[num].text = dateTime.ToString("H:mm");
			this.endTimeLabelArray[num].text = dateTime2.ToString("H:mm");
			this.endTimeLabelArray[num].gameObject.SetActive(true);
			this.fromMarkLabelArray[num].gameObject.SetActive(true);
			DateTime? dateTime4 = this.nextDateTime;
			if (dateTime4 == null)
			{
				if (DataMng.Instance().RespData_ColosseumInfo.openAllDay == 1)
				{
					this.SetActiveNationwideButton(true);
					this.SetActiveNationwideExtraButton(true);
					this.lastTimeObj.SetActive(true);
					this.endDateTime = new DateTime?(new DateTime(ServerDateTime.Now.Year, ServerDateTime.Now.Month, ServerDateTime.Now.Day, 23, 59, 59));
				}
				else if (DateTime.Parse(colosseum.openTime) < ServerDateTime.Now && ServerDateTime.Now < DateTime.Parse(colosseum.closeTime) && dateTime < ServerDateTime.Now && ServerDateTime.Now < dateTime2)
				{
					this.inSessionObjArray[num].SetActive(true);
					this.nextDateTime = new DateTime?(dateTime);
					this.endDateTime = new DateTime?(dateTime2);
					this.lastTimeObj.SetActive(true);
					this.SetActiveNationwideButton(!this.isAggregate);
					this.SetActiveNationwideExtraButton(!this.isAggregate);
				}
				else if (DateTime.Parse(colosseum.openTime) < ServerDateTime.Now && ServerDateTime.Now < DateTime.Parse(colosseum.closeTime) && dateTime2 < ServerDateTime.Now)
				{
					this.endDateTime = new DateTime?(dateTime2);
					if (dateTime3 != null && ServerDateTime.Now < dateTime3)
					{
						this.nextDateTime = dateTime3;
					}
				}
				else
				{
					this.nextDateTime = new DateTime?(dateTime);
				}
			}
			num++;
		}
	}

	private void LastTimer()
	{
		DateTime? dateTime = this.endDateTime;
		if (dateTime != null)
		{
			DateTime now = ServerDateTime.Now;
			DateTime? dateTime2 = this.endDateTime;
			if (now < dateTime2.Value)
			{
				DateTime? dateTime3 = this.endDateTime;
				TimeSpan timeSpan = dateTime3.Value - ServerDateTime.Now;
				if (0.0 >= timeSpan.TotalSeconds)
				{
					this.ResetLastTimer();
					return;
				}
				this.lastTimeLabel.text = string.Concat(new string[]
				{
					timeSpan.Hours.ToString("00"),
					":",
					timeSpan.Minutes.ToString("00"),
					":",
					timeSpan.Seconds.ToString("00")
				});
				goto IL_1DD;
			}
		}
		if (this.lastTimeObj.activeSelf)
		{
			this.lastTimeObj.SetActive(false);
			this.SetActiveNationwideButton(false);
			this.SetActiveNationwideExtraButton(false);
			foreach (GameObject gameObject in this.inSessionObjArray)
			{
				gameObject.SetActive(false);
			}
		}
		DateTime? dateTime4 = this.nextDateTime;
		if (dateTime4 != null)
		{
			DateTime? dateTime5 = this.nextDateTime;
			if ((dateTime5.Value - ServerDateTime.Now).TotalSeconds <= 0.0)
			{
				this.ResetLastTimer();
				return;
			}
		}
		else
		{
			DateTime dateTime6 = ServerDateTime.Now.AddDays(1.0);
			if ((new DateTime(dateTime6.Year, dateTime6.Month, dateTime6.Day) - ServerDateTime.Now).TotalSeconds <= 0.0)
			{
				this.ResetLastTimer();
				return;
			}
		}
		IL_1DD:
		if (this.isAggregate)
		{
			return;
		}
		base.Invoke("LastTimer", 1f);
	}

	private void ResetLastTimer()
	{
		this.SetTimeSchedule();
		this.LastTimer();
		this.GetCommentData();
		this.LoadNaviThumb();
		this.DisplayComment();
	}

	private void SetRankSprite()
	{
		if (this.isAggregate)
		{
			this.rankSprite.gameObject.SetActive(false);
			this.aggregateMarkObj.SetActive(true);
		}
		else
		{
			foreach (GUIListPartsRank.RankData rankData in this.rankDataList)
			{
				if (rankData.id == this.colosseumUserStatus.colosseumRankId)
				{
					this.rankSprite.spriteName = "Rank_" + rankData.id.ToString();
					this.currentRankData = rankData;
					break;
				}
			}
		}
	}

	private void SetRankGuage()
	{
		if (this.isAggregate)
		{
			this.nextRankLabel.gameObject.SetActive(false);
			this.nowScore.gameObject.SetActive(false);
			this.staminaDispGroup.SetActive(false);
			this.rankingDispGroup.SetActive(false);
			this.bpDispGroup.SetActive(false);
			this.aggregateLabel.gameObject.SetActive(true);
		}
		else
		{
			if (this.colosseumBattleRecord != null)
			{
				this.nextRankLabel.text = string.Format(StringMaster.GetString("ColosseumRankAGroup"), this.colosseumBattleRecord.count, this.colosseumBattleRecord.winPercent);
			}
			else
			{
				this.nextRankLabel.text = string.Format(StringMaster.GetString("ColosseumNextRank"), (this.currentRankData.upperPoint - this.colosseumUserStatus.winTotal + 1).ToString());
			}
			this.nowScore.text = string.Format(StringMaster.GetString("NowColosseumScore"), this.colosseumUserStatus.score.ToString());
		}
	}

	private void CreateRankDataList()
	{
		this.rankDataList = new List<GUIListPartsRank.RankData>();
		foreach (GameWebAPI.RespDataMA_ColosseumRankM.ColosseumRank colosseumRank in MasterDataMng.Instance().RespDataMA_ColosseumRankMaster.colosseumRankM)
		{
			this.rankDataList.Add(new GUIListPartsRank.RankData
			{
				upperPoint = int.Parse(colosseumRank.maxScore),
				lowerPoint = int.Parse(colosseumRank.minScore),
				id = int.Parse(colosseumRank.colosseumRankId)
			});
		}
		this.rankDataList.Sort((GUIListPartsRank.RankData a, GUIListPartsRank.RankData b) => b.lowerPoint - a.lowerPoint);
		int lowerPoint = this.rankDataList[0].lowerPoint;
		for (int j = 0; j < this.rankDataList.Count; j++)
		{
			this.rankDataList[j].isHideMaximum = (lowerPoint == this.rankDataList[j].lowerPoint);
		}
	}

	private void GetCommentData()
	{
		List<List<CMD_Tips.TipsM.TipsManage>> list = new List<List<CMD_Tips.TipsM.TipsManage>>();
		List<CMD_Tips.TipsM.Tips> list2 = new List<CMD_Tips.TipsM.Tips>();
		foreach (CMD_Tips.TipsM.TipsManage tipsManage2 in MasterDataMng.Instance().RespDataMA_TipsM.tipsM.tipsManage)
		{
			int num = int.Parse(tipsManage2.dispType);
			if (list.Count < num)
			{
				list.Add(new List<CMD_Tips.TipsM.TipsManage>());
			}
			if (tipsManage2 != null)
			{
				list[list.Count - 1].Add(tipsManage2);
			}
		}
		foreach (CMD_Tips.TipsM.Tips tips2 in MasterDataMng.Instance().RespDataMA_TipsM.tipsM.tips)
		{
			if (tips2 != null)
			{
				list2.Add(tips2);
			}
		}
		this.displayCommentDataList.Clear();
		CMD_PvPTop.COMMENT_TIMING commentTiming = this.GetCommentTiming();
		foreach (CMD_Tips.TipsM.TipsManage tipsManage3 in list[commentTiming - (CMD_PvPTop.COMMENT_TIMING)1])
		{
			this.displayCommentDataList.Add(Algorithm.BinarySearch<CMD_Tips.TipsM.Tips>(list2, tipsManage3.tipsId, 0, list2.Count - 1, "tipsId", 8));
		}
	}

	private CMD_PvPTop.COMMENT_TIMING GetCommentTiming()
	{
		CMD_PvPTop.COMMENT_TIMING result;
		if (this.isAggregate)
		{
			result = CMD_PvPTop.COMMENT_TIMING.Aggregating;
		}
		else if (DataMng.Instance().RespData_ColosseumInfo.openAllDay == 1)
		{
			result = CMD_PvPTop.COMMENT_TIMING.InSession;
		}
		else
		{
			DateTime? dateTime = this.endDateTime;
			if (dateTime == null)
			{
				result = CMD_PvPTop.COMMENT_TIMING.HeldBefore;
			}
			else
			{
				DateTime? dateTime2 = this.nextDateTime;
				if (dateTime2 == null)
				{
					result = CMD_PvPTop.COMMENT_TIMING.NonHeld;
				}
				else
				{
					DateTime? dateTime3 = this.endDateTime;
					if (ServerDateTime.Now < dateTime3)
					{
						DateTime now = ServerDateTime.Now;
						DateTime? dateTime4 = this.nextDateTime;
						TimeSpan timeSpan = now - dateTime4.Value;
						DateTime? dateTime5 = this.endDateTime;
						TimeSpan timeSpan2 = dateTime5.Value - ServerDateTime.Now;
						double num = timeSpan.TotalSeconds / timeSpan2.TotalSeconds;
						if (num > 2.0)
						{
							result = CMD_PvPTop.COMMENT_TIMING.EndBefore;
						}
						else
						{
							result = CMD_PvPTop.COMMENT_TIMING.InSession;
						}
					}
					else
					{
						DateTime? dateTime6 = this.nextDateTime;
						TimeSpan timeSpan3 = dateTime6.Value - ServerDateTime.Now;
						DateTime now2 = ServerDateTime.Now;
						DateTime? dateTime7 = this.endDateTime;
						TimeSpan timeSpan4 = now2 - dateTime7.Value;
						double num2 = timeSpan3.TotalSeconds / timeSpan4.TotalSeconds;
						if (num2 > 2.0)
						{
							result = CMD_PvPTop.COMMENT_TIMING.EndAfter;
						}
						else
						{
							result = CMD_PvPTop.COMMENT_TIMING.HeldBefore;
						}
					}
				}
			}
		}
		return result;
	}

	private void DisplayComment()
	{
		base.InvokeRepeating("DisplayComment_", 0f, this.SWITCH_COMMENT_INTERVAL_TIME);
	}

	private void DisplayComment_()
	{
		if (this.displayCommentDataListIndex == 0)
		{
			this.displayCommentDataList = Algorithm.ShuffuleList<CMD_Tips.TipsM.Tips>(this.displayCommentDataList);
		}
		if (this.displayCommentDataListIndex < this.displayCommentDataList.Count)
		{
			if ("45" == this.displayCommentDataList[this.displayCommentDataListIndex].tipsId)
			{
				if (ConstValue.PVP_MAX_RANK <= this.currentRankData.id)
				{
					this.commentLabel.text = StringMaster.GetString("ColosseumTips");
				}
				else if (this.colosseumBattleRecord != null)
				{
					this.displayCommentDataListIndex++;
					if (this.displayCommentDataListIndex >= this.displayCommentDataList.Count)
					{
						this.displayCommentDataListIndex = 0;
					}
					this.commentLabel.text = this.displayCommentDataList[this.displayCommentDataListIndex].message;
				}
				else
				{
					this.commentLabel.text = string.Format(this.displayCommentDataList[this.displayCommentDataListIndex].message, (this.currentRankData.upperPoint - this.colosseumUserStatus.winTotal + 1).ToString());
				}
			}
			else
			{
				this.commentLabel.text = this.displayCommentDataList[this.displayCommentDataListIndex].message;
			}
			Texture2D mainTexture;
			this.thumbnails.TryGetValue(this.displayCommentDataList[this.displayCommentDataListIndex].img + this.displayCommentDataList[this.displayCommentDataListIndex].icon, out mainTexture);
			this.thumbnail.mainTexture = mainTexture;
			this.displayCommentDataListIndex++;
		}
		if (this.displayCommentDataListIndex >= this.displayCommentDataList.Count)
		{
			this.displayCommentDataListIndex = 0;
		}
	}

	private void LoadNaviThumb()
	{
		this.thumbnails = new Dictionary<string, Texture2D>();
		foreach (CMD_Tips.TipsM.Tips tips in this.displayCommentDataList)
		{
			string text = tips.img + tips.icon;
			if (!this.thumbnails.ContainsKey(text))
			{
				Texture2D value = NGUIUtil.LoadTexture("Navi/" + text);
				this.thumbnails.Add(text, value);
			}
		}
	}

	private void SetPeriod()
	{
		if (DataMng.Instance().RespData_ColosseumInfo != null && DataMng.Instance().RespData_ColosseumInfo.colosseumId > 0)
		{
			GameWebAPI.RespDataMA_ColosseumM.Colosseum colosseum = MasterDataMng.Instance().RespDataMA_ColosseumMaster.colosseumM.SingleOrDefault((GameWebAPI.RespDataMA_ColosseumM.Colosseum x) => x.colosseumId == DataMng.Instance().RespData_ColosseumInfo.colosseumId.ToString());
			if (colosseum != null)
			{
				CultureInfo cultureInfo = new CultureInfo(string.Empty);
				DateTimeFormatInfo dateTimeFormat = cultureInfo.DateTimeFormat;
				dateTimeFormat.AbbreviatedDayNames = new string[]
				{
					StringMaster.GetString("SundayShortName"),
					StringMaster.GetString("MondayShortName"),
					StringMaster.GetString("TuesdayShortName"),
					StringMaster.GetString("WednesdayShortName"),
					StringMaster.GetString("ThursdayShortName"),
					StringMaster.GetString("FridayShortName"),
					StringMaster.GetString("SaturdayShortName")
				};
				DateTime dateTime = DateTime.Parse(colosseum.openTime);
				DateTime dateTime2 = DateTime.Parse(colosseum.closeTime);
				this.periodLabel.text = string.Format(StringMaster.GetString("ColosseumTerm"), new object[]
				{
					dateTime.ToString("M/d(ddd) H:mm", cultureInfo),
					dateTime2.ToString("M/d(ddd) H:mm", cultureInfo),
					string.Empty,
					string.Empty
				});
			}
			else
			{
				this.periodLabel.text = StringMaster.GetString("ColosseumCloseTime");
			}
		}
		else
		{
			this.periodLabel.text = StringMaster.GetString("ColosseumCloseTime");
		}
	}

	private void SetStamina()
	{
		GameWebAPI.RespDataMA_ColosseumM.Colosseum colosseumM = MasterDataMng.Instance().RespDataMA_ColosseumMaster.colosseumM.Single((GameWebAPI.RespDataMA_ColosseumM.Colosseum x) => x.colosseumId == DataMng.Instance().RespData_ColosseumInfo.colosseumId.ToString());
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM.worldDungeonM.Single((GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM x) => x.worldDungeonId == colosseumM.worldDungeonId);
		this.normalBattleStamina = int.Parse(worldDungeonM.needStamina);
		this.staminaCostLabel.text = string.Format(StringMaster.GetString("PvPTop_txt2"), StringMaster.GetString("QuestDetailsCost"), worldDungeonM.needStamina);
		this.staminaCostLabelExtra.text = string.Format(StringMaster.GetString("PvPTop_txt2"), StringMaster.GetString("QuestDetailsCost"), DataMng.Instance().RespData_ColosseumInfo.extraCost);
		if (0 < this.freeCostBattleCount)
		{
			this.staminaCostLabel.text = string.Format(StringMaster.GetString("ColosseumStamina"), this.freeCostBattleCount);
			this.staminaCostLabel.color = ConstValue.DIGIMON_YELLOW;
		}
		if (1 < int.Parse(colosseumM.extraRewardRate))
		{
			this.lbRewardPercentage.text = string.Format(StringMaster.GetString("ColosseumRewardPercentage"), colosseumM.extraRewardRate);
		}
		else
		{
			this.lbRewardPercentage.text = string.Empty;
		}
		this.userStamina.SetMode(UserStamina.Mode.QUEST);
		this.userStamina.RefreshParams();
	}

	private void StartNationwide()
	{
		int stamina = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.stamina;
		int num = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.staminaMax);
		int point = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
		if (num < this.needStamina)
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("QuestNormal");
			cmd_ModalMessage.Info = StringMaster.GetString("QuestStaminaOver");
		}
		else if (stamina < this.needStamina)
		{
			if (point >= ConstValue.RECOVER_STAMINA_DIGISTONE_NUM)
			{
				CMD_ChangePOP_STONE cmd_ChangePOP_STONE = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP_STONE", null) as CMD_ChangePOP_STONE;
				cmd_ChangePOP_STONE.Title = StringMaster.GetString("StaminaShortageTitle");
				cmd_ChangePOP_STONE.OnPushedYesAction = new Action(this.OnSelectedRecover);
				cmd_ChangePOP_STONE.Info = string.Format(StringMaster.GetString("StaminaShortageInfo"), new object[]
				{
					ConstValue.RECOVER_STAMINA_DIGISTONE_NUM,
					stamina,
					stamina + num,
					point
				});
				cmd_ChangePOP_STONE.SetDigistone(point, ConstValue.RECOVER_STAMINA_DIGISTONE_NUM);
				cmd_ChangePOP_STONE.BtnTextYes = StringMaster.GetString("StaminaRecoveryExecution");
				cmd_ChangePOP_STONE.BtnTextNo = StringMaster.GetString("SystemButtonClose");
			}
			else
			{
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseConfirmShop), "CMD_Confirm", null) as CMD_Confirm;
				cmd_Confirm.Title = StringMaster.GetString("StaminaShortageTitle");
				cmd_Confirm.Info = string.Format(StringMaster.GetString("StaminaShortageGoShop"), ConstValue.RECOVER_STAMINA_DIGISTONE_NUM);
				cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonGoShop");
				cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
			}
		}
		else
		{
			ClassSingleton<QuestData>.Instance.SelectDungeon = this.GetWorldDungeonMaster();
			ClassSingleton<MultiBattleData>.Instance.MockBattleUserCode = "0";
			CMD_ColosseumDeck.Create(CMD_ColosseumDeck.Mode.FROM_COLOSSEUM_TOP);
		}
	}

	private void OnSelectedRecover()
	{
		base.StartCoroutine(this.RequestRecoverStamina(delegate
		{
			GUIPlayerStatus.RefreshParams_S(true);
			this.userStamina.RefreshParams();
			CMD_ChangePOP_STONE cmd_ChangePOP_STONE = UnityEngine.Object.FindObjectOfType<CMD_ChangePOP_STONE>();
			if (null != cmd_ChangePOP_STONE)
			{
				cmd_ChangePOP_STONE.SetCloseAction(delegate(int i)
				{
					CMD_ModalMessage.Create(StringMaster.GetString("StaminaRecoveryTitle"), StringMaster.GetString("StaminaRecoveryCompleted"), null);
				});
				cmd_ChangePOP_STONE.ClosePanel(true);
			}
			else
			{
				CMD_ModalMessage.Create(StringMaster.GetString("StaminaRecoveryTitle"), StringMaster.GetString("StaminaRecoveryCompleted"), null);
			}
		}));
	}

	private IEnumerator RequestRecoverStamina(Action onSuccessed)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		APIRequestTask task = Singleton<UserDataMng>.Instance.RequestRecoverStamina(false);
		return task.Run(delegate
		{
			RestrictionInput.EndLoad();
			if (onSuccessed != null)
			{
				onSuccessed();
			}
		}, delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
		}, null);
	}

	private void OnCloseConfirmShop(int idx)
	{
		if (idx == 0)
		{
			Action<int> action = delegate(int selectButtonIndex)
			{
				if (selectButtonIndex == 9)
				{
					this.StartNationwide();
				}
			};
			GUIMain.ShowCommonDialog(action, "CMD_Shop", null);
		}
	}

	private void ShowColosseumReward(GameWebAPI.RespDataCL_GetColosseumReward colosseumReward, Action<int> action)
	{
		int num = colosseumReward.maxRewardListKey();
		int num2 = colosseumReward.maxInterimRewardListKey();
		CMD_ColosseumBonus cmd_ColosseumBonus = GUIMain.ShowCommonDialog(action, "CMD_ColosseumBonus", null) as CMD_ColosseumBonus;
		string title = string.Empty;
		GameWebAPI.ColosseumReward[] rewardList;
		if (num >= num2)
		{
			rewardList = colosseumReward.rewardList[num.ToString()];
			title = StringMaster.GetString("ColosseumRankRewardTitle");
		}
		else
		{
			rewardList = colosseumReward.interimRewardList[num2.ToString()];
			title = StringMaster.GetString("ColosseumInterimRankRewardName");
		}
		cmd_ColosseumBonus.SetReward(title, rewardList);
	}

	private void ShowRankDownDialog()
	{
		string text = PvPUtility.PopPvPTopNoticeCode();
		if (!string.IsNullOrEmpty(text))
		{
			string @string = StringMaster.GetString(string.Format("ColosseumNotice-{0}", text));
			CMD_ModalMessageNoBtn cmd_ModalMessageNoBtn = GUIMain.ShowCommonDialog(null, "CMD_ModalMessageNoBtn", null) as CMD_ModalMessageNoBtn;
			cmd_ModalMessageNoBtn.SetParam(@string);
			cmd_ModalMessageNoBtn.AdjustSize();
		}
	}

	public void OnTouchedNationwide()
	{
		DateTime? dateTime = this.cancelPenaltyDateTime;
		if (dateTime != null)
		{
			DateTime? dateTime2 = this.cancelPenaltyDateTime;
			if (ServerDateTime.Now < dateTime2)
			{
				CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
				cmd_ModalMessage.Title = StringMaster.GetString("ColosseumPenaltyTitle");
				cmd_ModalMessage.Info = StringMaster.GetString("ColosseumPenaltyInfo");
				return;
			}
		}
		this.needStamina = ((this.freeCostBattleCount <= 0) ? this.normalBattleStamina : 0);
		this.StartNationwide();
	}

	public void OnTouchedNationwideExtra()
	{
		DateTime? dateTime = this.cancelPenaltyDateTime;
		if (dateTime != null)
		{
			DateTime? dateTime2 = this.cancelPenaltyDateTime;
			if (ServerDateTime.Now < dateTime2)
			{
				CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
				cmd_ModalMessage.Title = StringMaster.GetString("ColosseumPenaltyTitle");
				cmd_ModalMessage.Info = StringMaster.GetString("ColosseumPenaltyInfo");
				return;
			}
		}
		this.needStamina = DataMng.Instance().RespData_ColosseumInfo.extraCost;
		this.isExtraBattle = true;
		this.StartNationwide();
	}

	public void OnTouchedMockBattle()
	{
		GUIMain.ShowCommonDialog(null, "CMD_MockBattleMenu", null);
	}

	public void OnTouchedRanking()
	{
		GUIMain.ShowCommonDialog(null, "CMD_ColosseumRanking", null);
	}

	public void OnTouchedDiscription()
	{
		GUIManager.ShowBarrierZset((float)(-(float)this.thumbnail.depth) + -500f);
		GameObject gameObject = UnityEngine.Object.Instantiate(AssetDataMng.Instance().LoadObject("Tutorial/TutorialImageWindow", null, true)) as GameObject;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = new Vector3(0f, 0f, (float)(-(float)this.thumbnail.depth) + -1000f);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		TutorialImageWindow component = gameObject.GetComponent<TutorialImageWindow>();
		AppCoroutine.Start(component.OpenWindow(this.helpImagePathList, false, delegate
		{
			GUIManager.HideBarrier();
			GUIManager.ShowBarrierZset((float)(-(float)this.thumbnail.depth) + 100f);
		}, null), false);
	}

	public void OnTouchedNotes()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow", null) as CMDWebWindow;
		cmdwebWindow.TitleText = StringMaster.GetString("SystemCaution");
		cmdwebWindow.Url = WebAddress.EXT_ADR_COLOSSEUM_NOTICE;
	}

	public void OnTouchedRankInfo()
	{
		CMD_RankModal cmd_RankModal = GUIMain.ShowCommonDialog(null, "CMD_RankModal", null) as CMD_RankModal;
		cmd_RankModal.Initialize(this.rankDataList, this.colosseumUserStatus.colosseumRankId, this.colosseumUserStatus.winTotal, this.isAggregate);
	}

	public void OnTouchedReward()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow", null) as CMDWebWindow;
		cmdwebWindow.TitleText = StringMaster.GetString("ColosseumReward");
		cmdwebWindow.Url = WebAddress.EXT_ADR_COLOSSEUM_REWARD + DataMng.Instance().RespData_ColosseumInfo.colosseumId.ToString();
	}

	private GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM GetWorldDungeonMaster()
	{
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM result = null;
		if (DataMng.Instance().RespData_ColosseumInfo != null && 0 < DataMng.Instance().RespData_ColosseumInfo.colosseumId)
		{
			string text = string.Empty;
			int colosseumId = DataMng.Instance().RespData_ColosseumInfo.colosseumId;
			GameWebAPI.RespDataMA_ColosseumM.Colosseum[] colosseumM = MasterDataMng.Instance().RespDataMA_ColosseumMaster.colosseumM;
			string b = colosseumId.ToString();
			for (int i = 0; i < colosseumM.Length; i++)
			{
				if (colosseumM[i].colosseumId == b)
				{
					text = colosseumM[i].worldDungeonId;
					break;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				result = ClassSingleton<QuestData>.Instance.GetWorldDungeonMaster(text);
			}
		}
		return result;
	}

	public enum COMMENT_TIMING
	{
		NonHeld = 7,
		HeldBefore,
		InSession,
		EndBefore,
		EndAfter,
		Aggregating = 15
	}
}
