using Master;
using MultiBattle.Tools;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using WebAPIRequest;

public class CMD_MultiRecruitPartyWait : CMD
{
	[Header("エリア名 ステージ名")]
	[SerializeField]
	private UILabel lbAreaName;

	[SerializeField]
	private UILabel lbStageName;

	[Header("リーダースキル名")]
	[SerializeField]
	private UILabel lbTXT_LEADERSKILL;

	[SerializeField]
	private UILabel lbTXT_LEADERSKILL_EXP;

	[Header("ルームパスワード")]
	[SerializeField]
	private GameObject goPASSWORD_BOX;

	[SerializeField]
	private UILabel lbTXT_PASSWORD;

	[SerializeField]
	private UILabel lbTXT_PASSWORD_EXP;

	[Header("フレンドボタン")]
	[SerializeField]
	private GameObject goBTN_FRIEND;

	[Header("チャットボタン")]
	[SerializeField]
	private GameObject goBTN_CHAT;

	[Header("ステータス変更ボタン")]
	[SerializeField]
	private UILabel lbTXT_ST_EXCHANGE;

	[Header("準備完了ボタン")]
	[SerializeField]
	private UILabel lbTXT_BTN_READY;

	[SerializeField]
	private UISprite spBTN_READY;

	[SerializeField]
	private BoxCollider coBTN_READY;

	[Header("決定ボタン")]
	[SerializeField]
	private GameObject goBTN_DECIDE;

	[SerializeField]
	private UISprite spBTN_DECIDE;

	[SerializeField]
	private UILabel lbBTN_DECIDE;

	[SerializeField]
	private BoxCollider coBTN_DECIDE;

	[SerializeField]
	private MonsterInfoList partyUI;

	private PartsMultiRecruitMonsInfo[] monsterInfoList;

	[Header("エモーション処理")]
	[SerializeField]
	private EmotionSenderMulti emotionSenderMulti;

	[Header("エモーションコンポーネント")]
	[SerializeField]
	private EmotionButtonFront emotionButtonCP;

	[SerializeField]
	private GameObject bossIconRootObject;

	[SerializeField]
	private BossThumbnail[] bossIconList;

	[SerializeField]
	private SortieLimitList sortieLimitList;

	private static CMD_MultiRecruitPartyWait instance;

	private string myUserId;

	private string myNickname;

	private string myTitleId;

	private List<MonsterData> monsterDataList;

	private List<GameWebAPI.Common_MultiMemberList> memberList;

	private GameWebAPI.Common_MultiRoomInfo roomInfo;

	private MonsterData leaderMonsterData;

	public MonsterData ownerSubMonsterData;

	private MonsterData myMonsterData;

	private int myPositionNumber;

	private List<int> TCPSendUserIdList;

	private List<PartsMultiRecruitMonsInfo> partsMonsInfoList;

	private bool isEndFirstSetMonsterUI;

	private bool isReady;

	private bool isGoingBattle;

	private bool isBattleClicked;

	private bool isCancelClicked;

	private bool isDispTCPSendingLoad;

	private bool isDispErrorModal;

	private bool isOwnerReturnRoom;

	private Vector3 v3DPos = new Vector3(0f, 4000f, 0f);

	private float xPos = -1000f;

	private CMD_Confirm cmdConfirm;

	private CMD_ModalMessage cmdModalMessage;

	private Queue hashValueQueue = new Queue();

	[NonSerialized]
	public List<string> recruitedFriendIdList;

	private bool haveOwner;

	private bool ownerGoingBattle;

	private CMD_MultiRecruitTop parentDialog;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	[CompilerGenerated]
	private static Action <>f__mg$cache1;

	public static GameWebAPI.ResponseData_Common_MultiRoomList.room StageDataBk { get; set; }

	public static CMD_MultiRecruitPartyWait Instance
	{
		get
		{
			return CMD_MultiRecruitPartyWait.instance;
		}
	}

	public static CMD_MultiRecruitPartyWait.USER_TYPE UserType { get; set; }

	public static GameWebAPI.RespData_MultiRoomCreate roomCreateData { get; set; }

	public static GameWebAPI.RespData_MultiRoomJoin roomJoinData { get; set; }

	protected override void Awake()
	{
		base.Awake();
		CMD_MultiRecruitPartyWait.instance = this;
		this.myUserId = DataMng.Instance().RespDataCM_Login.playerInfo.userId;
		this.myNickname = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.nickname;
		this.myTitleId = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.titleId;
		CMD_PartyEdit.ModeType = CMD_PartyEdit.MODE_TYPE.MULTI;
	}

	protected override void Update()
	{
		base.Update();
		if (TCPUtil.IsTCPSending && !this.isDispErrorModal)
		{
			MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
			this.isDispTCPSendingLoad = true;
		}
		else if (this.isDispTCPSendingLoad)
		{
			MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			this.isDispTCPSendingLoad = false;
		}
	}

	private void OnDisable()
	{
		MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		Screen.sleepTimeout = -1;
		if (CMD_MultiRecruitPartyWait.UserType != CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
		{
			SoundMng.Instance().PlayGameBGM("bgm_301");
		}
		base.SetOpendAction(new Action<int>(this.OpendAction));
		for (int i = 0; i < this.bossIconList.Length; i++)
		{
			this.bossIconList[i].Initialize();
		}
		this.SetBossMonsterIcon(ClassSingleton<QuestData>.Instance.GetBossMonsterList(ClassSingleton<PartyBossIconsAccessor>.Instance.StageEnemies));
		this.sortieLimitList.Initialize();
		this.SetSortieLimit();
		if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
		{
			this.sortieLimitList.gameObject.SetActive(false);
		}
		this.InitData();
		this.SetMonsterUI();
		this.isEndFirstSetMonsterUI = true;
		this.SetLabelText();
		Singleton<TCPUtil>.Instance.PrepareTCPServer(new Action<string>(this.AfterPrepareTCPServer), "multi");
		base.Show(f, sizeX, sizeY, aT);
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.returnVal == -1)
		{
			this.returnVal = 0;
			this.OnClickCancel();
		}
		else
		{
			MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			if (CMD_MultiRecruitPartyWait.UserType != CMD_MultiRecruitPartyWait.USER_TYPE.OWNER && !this.isGoingBattle)
			{
				SoundMng.Instance().PlayGameBGM("bgm_201");
			}
			if (PartsMenu.instance != null)
			{
				PartsMenu.instance.Active(true);
			}
			base.ClosePanel(animation);
		}
	}

	protected override void OnDestroy()
	{
		if (!GUIManager.IsCloseAllMode() && CMD_PartyEdit.instance != null)
		{
			CMD_PartyEdit.instance.ReloadAllCharacters(true);
		}
		if (this.parentDialog != null)
		{
			this.parentDialog.ClickUpdateBtn();
		}
		Screen.sleepTimeout = -2;
		CMD_MultiRecruitPartyWait.StageDataBk = null;
		base.OnDestroy();
	}

	private void OpendAction(int i)
	{
		if (CMD_PartyEdit.instance != null)
		{
			CMD_PartyEdit.instance.ReloadAllCharacters(false);
		}
	}

	private void InitData()
	{
		this.monsterDataList = new List<MonsterData>();
		this.memberList = new List<GameWebAPI.Common_MultiMemberList>();
		if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
		{
			this.roomInfo = CMD_MultiRecruitPartyWait.roomCreateData.multiRoomInfo;
			this.memberList.AddRange(CMD_MultiRecruitPartyWait.roomCreateData.multiRoomMemberList);
		}
		else
		{
			this.roomInfo = CMD_MultiRecruitPartyWait.roomJoinData.multiRoomInfo;
			this.memberList.AddRange(CMD_MultiRecruitPartyWait.roomJoinData.multiRoomMemberList);
		}
		for (int j = this.memberList.Count; j < 3; j++)
		{
			this.memberList.Add(null);
		}
		foreach (var <>__AnonType in this.memberList.Select((GameWebAPI.Common_MultiMemberList v, int i) => new
		{
			v,
			i
		}))
		{
			if (<>__AnonType.v != null)
			{
				MonsterData item = <>__AnonType.v.userMonsterList[0].ToMonsterData();
				this.monsterDataList.Add(item);
				if (<>__AnonType.i == 0)
				{
					this.leaderMonsterData = item;
					if (<>__AnonType.v.userMonsterList.Length > 1)
					{
						this.ownerSubMonsterData = <>__AnonType.v.userMonsterList[1].ToMonsterData();
					}
				}
				else if (<>__AnonType.v.userId == this.myUserId)
				{
					this.myMonsterData = item;
					this.myPositionNumber = <>__AnonType.i;
				}
			}
			else
			{
				this.monsterDataList.Add(null);
			}
		}
	}

	private void SetLabelText()
	{
		if (this.roomInfo.worldAreaId == "1")
		{
			this.lbAreaName.text = string.Format(StringMaster.GetString("GUIListPartsA_txt"), int.Parse(this.roomInfo.worldStageId)) + " " + this.roomInfo.stageName;
			this.lbStageName.text = string.Format(StringMaster.GetString("MultiRecruitQuestStage"), int.Parse(this.roomInfo.priority), this.roomInfo.dungeonName);
		}
		else
		{
			this.lbAreaName.text = StringMaster.GetString("QuestSpecial") + " " + this.roomInfo.stageName;
			this.lbStageName.text = this.roomInfo.dungeonName;
		}
		this.lbTXT_ST_EXCHANGE.text = StringMaster.GetString("PartyStatusChange");
		if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
		{
			this.lbTXT_PASSWORD_EXP.text = StringMaster.GetString("MultiRecruit-01");
			this.lbTXT_PASSWORD.text = this.roomInfo.password;
			this.isReady = true;
		}
		else
		{
			this.goPASSWORD_BOX.SetActive(false);
			this.goBTN_FRIEND.SetActive(false);
			this.goBTN_CHAT.SetActive(false);
			this.goBTN_DECIDE.SetActive(false);
			this.isReady = false;
		}
		this.SetReadyBtnLabel(this.CheckSortieLimit());
		this.UpdateReadyState(this.myUserId, this.isReady);
		this.SetBtnDecide();
		this.RefreshLeaderSkillText();
	}

	private void AfterPrepareTCPServer(string server)
	{
		Singleton<TCPUtil>.Instance.MakeTCPClient();
		Singleton<TCPUtil>.Instance.SetAfterConnectTCPMethod(new Action<bool>(this.AfterConnectTCP));
		Singleton<TCPUtil>.Instance.SetTCPCallBackMethod(new Action<Dictionary<string, object>>(this.GetTCPReceponseData));
		Singleton<TCPUtil>.Instance.SetOnExitCallBackMethod(new Action(this.OnExitTCP));
		Singleton<TCPUtil>.Instance.SetExceptionMethod(new Action<short, string>(this.OnExceptionTCP));
		Singleton<TCPUtil>.Instance.InitConfirmationChecks();
		Singleton<TCPUtil>.Instance.ConnectTCPServerAsync(this.myUserId);
		this.emotionButtonCP.Initialize(this.emotionSenderMulti, new Action<int>(this.SendTCPEmotion));
		MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
	}

	private void AfterConnectTCP(bool result)
	{
		if (result)
		{
			if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
			{
				this.SendTCPRoomResume();
			}
			AppCoroutine.Start(this.SendTCPShareUserInfo(this.myMonsterData, true, false), true);
		}
		else
		{
			this.OnClickCancelExec(this.DispErrorModal(StringMaster.GetString("AlertNetworkErrorTitle"), StringMaster.GetString("AlertNetworkErrorInfo")));
		}
	}

	public void ChangeMonster(MonsterData md, int positionNumber, bool isOwnerSubMonster = false)
	{
		if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
		{
			if (positionNumber == 0)
			{
				this.leaderMonsterData = md;
				this.RefreshLeaderSkillText();
			}
			if (isOwnerSubMonster)
			{
				for (int i = 1; i < 3; i++)
				{
					if (this.monsterDataList[i] != null && this.monsterDataList[i].userMonster.userId == this.ownerSubMonsterData.userMonster.userId)
					{
						this.monsterDataList[i] = this.ownerSubMonsterData;
						this.UpdateMonsterUISingle(false, i);
					}
				}
			}
		}
		else
		{
			this.myMonsterData = md;
		}
		if (this.monsterDataList[positionNumber] != null && this.monsterDataList[positionNumber].userMonster.userId == md.userMonster.userId)
		{
			this.monsterDataList[positionNumber] = md;
		}
		this.SetReadyBtnLabel(this.CheckSortieLimit());
		AppCoroutine.Start(this.SendTCPShareUserInfo(md, false, isOwnerSubMonster), true);
	}

	private IEnumerator ReturnRoom(float connectWait = 0f)
	{
		if (connectWait > 0f)
		{
			MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
			yield return AppCoroutine.Start(Util.WaitForRealTime(connectWait), true);
		}
		if (!Singleton<TCPUtil>.Instance.CheckTCPConnection())
		{
			Singleton<TCPUtil>.Instance.TCPReConnection(0);
			bool isConnected;
			do
			{
				yield return null;
				isConnected = Singleton<TCPUtil>.Instance.CheckTCPConnection();
			}
			while (!isConnected);
			this.SendTCPRoomResume();
			yield return null;
			this.SendTCPGetMemberList();
			this.isBattleClicked = false;
		}
		MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		yield break;
	}

	private void ForceRoomOut(CMD_MultiRecruitPartyWait.ROOM_OUT_TYPE roomOutType)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		string title = null;
		string info = null;
		if (roomOutType != CMD_MultiRecruitPartyWait.ROOM_OUT_TYPE.BREAK)
		{
			if (roomOutType != CMD_MultiRecruitPartyWait.ROOM_OUT_TYPE.TIMEOUT)
			{
				if (roomOutType == CMD_MultiRecruitPartyWait.ROOM_OUT_TYPE.MEMBER_EXPIRE)
				{
					title = StringMaster.GetString("MultiRecruit-13");
					info = StringMaster.GetString("MultiRecruit-14");
				}
			}
			else
			{
				title = StringMaster.GetString("MultiRecruit-10");
				info = StringMaster.GetString("MultiRecruit-12");
			}
		}
		else
		{
			title = StringMaster.GetString("MultiRecruit-10");
			info = StringMaster.GetString("MultiRecruit-11");
		}
		AppCoroutine.Start(this.DispErrorModal(title, info), true);
	}

	private void PrepareGoToBattleScene(GameWebAPI.RespData_WorldMultiStartInfo data)
	{
		this.isGoingBattle = true;
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
		DataMng.Instance().WD_ReqDngResult.dungeonId = data.worldDungeonId;
		DataMng.Instance().WD_ReqDngResult.clear = 0;
		DataMng.Instance().RespData_WorldMultiStartInfo = data;
		ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId = this.myUserId;
		ClassSingleton<MultiBattleData>.Instance.MaxAttackTime = ConstValue.MULTI_MAX_ATTACK_TIME;
		ClassSingleton<MultiBattleData>.Instance.HurryUpAttackTime = ConstValue.MULTI_HURRYUP_ATTACK_TIME;
		ClassSingleton<MultiBattleData>.Instance.RandomSeed = this.roomInfo.multiRoomId.ToInt32();
		MultiUser[] array = new MultiUser[3];
		ClassSingleton<MultiBattleData>.Instance.PlayerUserMonsterIds = new string[3];
		if (data.memberPatternId > 0)
		{
			array[data.memberPatternId] = new MultiUser
			{
				isOwner = false,
				userId = data.party[0].userId,
				userName = data.party[0].nickname
			};
			ClassSingleton<MultiBattleData>.Instance.PlayerUserMonsterIds[data.memberPatternId] = data.party[0].userMonsters[1].userMonsterId;
		}
		for (int i = 0; i < data.party.Length; i++)
		{
			MultiUser multiUser = new MultiUser
			{
				isOwner = (data.party[i].userId == this.leaderMonsterData.userMonster.userId),
				userId = data.party[i].userId,
				userName = data.party[i].nickname
			};
			if (array[i] == null)
			{
				array[i] = multiUser;
				ClassSingleton<MultiBattleData>.Instance.PlayerUserMonsterIds[i] = data.party[i].userMonsters[0].userMonsterId;
			}
			else
			{
				array[i + 1] = multiUser;
				ClassSingleton<MultiBattleData>.Instance.PlayerUserMonsterIds[i + 1] = data.party[i].userMonsters[0].userMonsterId;
			}
		}
		ClassSingleton<MultiBattleData>.Instance.MultiUsers = array;
		List<MultiUser> list = array.ToList<MultiUser>();
		MultiUser item2 = array.FirstOrDefault((MultiUser item) => item.isOwner);
		int num = 3 - list.Count;
		for (int j = 0; j < num; j++)
		{
			list.Add(item2);
		}
		ClassSingleton<MultiBattleData>.Instance.PvPUserDatas = new MultiBattleData.PvPUserData[list.Count<MultiUser>()];
		for (int k = 0; k < list.Count<MultiUser>(); k++)
		{
			MultiBattleData.PvPUserData pvPUserData = new MultiBattleData.PvPUserData();
			pvPUserData.isOwner = list[k].isOwner;
			pvPUserData.userStatus = new GameWebAPI.ColosseumUserStatus();
			pvPUserData.userStatus.userId = list[k].userId;
			pvPUserData.userStatus.nickname = list[k].userName;
			pvPUserData.monsterData = null;
			ClassSingleton<MultiBattleData>.Instance.PvPUserDatas[k] = pvPUserData;
		}
		TipsLoading.Instance.StartTipsLoad(CMD_Tips.DISPLAY_PLACE.QuestToMultiBattle, false);
		base.SetLastCallBack(new Action(this.GoToBattleScene));
		this.CloseAllModal();
		base.ClosePanel(true);
	}

	private void GoToBattleScene()
	{
		if (CMD_PartyEdit.instance != null)
		{
			CMD_PartyEdit.instance.ClosePanel(false);
		}
		if (CMD_QuestTOP.instance != null)
		{
			CMD_QuestTOP.instance.isGoingBattle = true;
			CMD_QuestTOP.instance.ClosePanel(false);
		}
		if (this.parentDialog != null)
		{
			this.parentDialog.SetForceReturnValue(0);
			this.parentDialog.ClosePanel(false);
		}
		if (CMD_ChatTop.instance != null)
		{
			CMD_ChatTop.instance.ClosePanel(false);
		}
		GameObject dialog = GUIManager.GetDialog("CMD_BattleNextChoice");
		if (dialog != null)
		{
			CMD_BattleNextChoice component = dialog.GetComponent<CMD_BattleNextChoice>();
			component.ClosePanel(false);
		}
		FarmCameraControlForCMD.On();
		global::Debug.Log("バトルシーンのロードを開始します");
		BattleStateManager.StartMulti(0.5f, 0.5f, true, null);
	}

	private void SetReadyBtnLabel(bool isClearSortieLimit)
	{
		if (this.isReady)
		{
			this.lbTXT_BTN_READY.text = StringMaster.GetString("MultiRecruit-03");
			this.lbTXT_BTN_READY.color = Color.white;
			this.spBTN_READY.spriteName = "Common02_Btn_Green";
			this.coBTN_READY.enabled = true;
		}
		else if (isClearSortieLimit)
		{
			this.lbTXT_BTN_READY.text = StringMaster.GetString("MultiRecruit-05");
			this.lbTXT_BTN_READY.color = Color.white;
			this.spBTN_READY.spriteName = "Common02_Btn_Red";
			this.coBTN_READY.enabled = true;
		}
		else
		{
			this.lbTXT_BTN_READY.text = StringMaster.GetString("MultiRecruit-05");
			this.lbTXT_BTN_READY.color = Color.gray;
			this.spBTN_READY.spriteName = "Common02_Btn_Gray";
			this.coBTN_READY.enabled = false;
		}
	}

	private bool CheckSortieLimit()
	{
		string tribe = string.Empty;
		string growStep = string.Empty;
		if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
		{
			if (this.leaderMonsterData != null)
			{
				tribe = this.leaderMonsterData.monsterMG.tribe;
				growStep = this.leaderMonsterData.monsterMG.growStep;
			}
		}
		else if (this.myMonsterData != null)
		{
			tribe = this.myMonsterData.monsterMG.tribe;
			growStep = this.myMonsterData.monsterMG.growStep;
		}
		return ClassSingleton<QuestData>.Instance.CheckSortieLimit(this.sortieLimitList.GetSortieLimitList(), tribe, growStep);
	}

	private void UpdateReadyState(string userId, bool isReady)
	{
		foreach (PartsMultiRecruitMonsInfo partsMultiRecruitMonsInfo in this.partsMonsInfoList)
		{
			if (partsMultiRecruitMonsInfo.Data != null && partsMultiRecruitMonsInfo.Data.userMonster.userId == userId)
			{
				partsMultiRecruitMonsInfo.SetTagReady(isReady);
				if (partsMultiRecruitMonsInfo.Data.userMonster.userId == this.myUserId && (CMD_MultiRecruitPartyWait.UserType != CMD_MultiRecruitPartyWait.USER_TYPE.OWNER || (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER && partsMultiRecruitMonsInfo.Data != this.ownerSubMonsterData)))
				{
					partsMultiRecruitMonsInfo.GetComponent<BoxCollider>().enabled = !isReady;
					partsMultiRecruitMonsInfo.SetStatusPanelActiveCollider(!isReady);
				}
			}
			else if (partsMultiRecruitMonsInfo.Data != null && CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER && partsMultiRecruitMonsInfo.Data == this.ownerSubMonsterData)
			{
				partsMultiRecruitMonsInfo.SetTagReady(this.isReady);
			}
		}
	}

	private void RefreshLeaderSkillText()
	{
		this.lbTXT_LEADERSKILL.text = string.Empty;
		this.lbTXT_LEADERSKILL_EXP.text = string.Empty;
		GameWebAPI.RespDataMA_GetSkillM.SkillM skillM = null;
		if (this.leaderMonsterData != null)
		{
			skillM = this.leaderMonsterData.GetLeaderSkill();
		}
		if (skillM == null)
		{
			this.lbTXT_LEADERSKILL.text = StringMaster.GetString("SystemNone");
			this.lbTXT_LEADERSKILL_EXP.text = StringMaster.GetString("CharaStatus-02");
		}
		else
		{
			this.lbTXT_LEADERSKILL.text = skillM.name;
			this.lbTXT_LEADERSKILL_EXP.text = skillM.description.Replace("\n", string.Empty).Replace("\r", string.Empty);
		}
	}

	private void SetBtnDecide()
	{
		if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
		{
			bool flag = true;
			if (this.partsMonsInfoList.Count < 3)
			{
				flag = false;
			}
			else if (!this.CheckWorldDungeonSortieLimit())
			{
				flag = false;
			}
			else
			{
				foreach (PartsMultiRecruitMonsInfo partsMultiRecruitMonsInfo in this.partsMonsInfoList)
				{
					if (!partsMultiRecruitMonsInfo.GetTagReady())
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				this.spBTN_DECIDE.spriteName = "Common02_Btn_Red";
				this.lbBTN_DECIDE.color = ConstValue.DIGIMON_YELLOW;
				this.lbBTN_DECIDE.effectColor = new Color(this.lbBTN_DECIDE.effectColor.r, this.lbBTN_DECIDE.effectColor.g, this.lbBTN_DECIDE.effectColor.b, 255f);
			}
			else
			{
				this.spBTN_DECIDE.spriteName = "Common02_Btn_Gray";
				this.lbBTN_DECIDE.color = ConstValue.DEACTIVE_BUTTON_LABEL;
				this.lbBTN_DECIDE.effectColor = new Color(this.lbBTN_DECIDE.effectColor.r, this.lbBTN_DECIDE.effectColor.g, this.lbBTN_DECIDE.effectColor.b, 0f);
			}
			this.coBTN_DECIDE.enabled = flag;
		}
	}

	private bool CheckWorldDungeonSortieLimit()
	{
		bool result = true;
		List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> list = this.sortieLimitList.GetSortieLimitList();
		if (list != null && 0 < list.Count)
		{
			for (int i = 0; i < this.partsMonsInfoList.Count; i++)
			{
				MonsterData data = this.partsMonsInfoList[i].Data;
				if (data != null)
				{
					string tribe = data.monsterMG.tribe;
					string growStep = data.monsterMG.growStep;
					if (!ClassSingleton<QuestData>.Instance.CheckSortieLimit(list, tribe, growStep))
					{
						result = false;
						break;
					}
				}
			}
		}
		return result;
	}

	private void SetMonsterUI()
	{
		this.TCPSendUserIdList = new List<int>();
		this.partsMonsInfoList = new List<PartsMultiRecruitMonsInfo>();
		if (this.monsterInfoList == null)
		{
			List<GameWebAPI.RespDataMA_WorldDungeonSortieLimit.WorldDungeonSortieLimit> limitList = this.sortieLimitList.GetSortieLimitList();
			this.monsterInfoList = this.partyUI.CreateMonsterInfo(limitList);
		}
		else
		{
			for (int i = 0; i < this.monsterInfoList.Length; i++)
			{
				this.monsterInfoList[i].HideStatusPanel();
			}
		}
		QuestBonusPack questBonus = this.CreateQuestBonus();
		QuestBonusTargetCheck checker = new QuestBonusTargetCheck();
		for (int j = 0; j < this.monsterInfoList.Length; j++)
		{
			this.monsterInfoList[j].SetQuestBonus(questBonus, checker);
			this.emotionSenderMulti.iconSpriteParents.Add(this.monsterInfoList[j].GetEmotionParentObject());
		}
		for (int k = 0; k < 3; k++)
		{
			this.UpdateMonsterUISingle(true, k);
		}
	}

	private QuestBonusPack CreateQuestBonus()
	{
		string areaId = string.Empty;
		string stageId = string.Empty;
		string dungeonId = string.Empty;
		if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
		{
			areaId = CMD_MultiRecruitPartyWait.roomCreateData.multiRoomInfo.worldAreaId;
			stageId = CMD_MultiRecruitPartyWait.roomCreateData.multiRoomInfo.worldStageId;
			dungeonId = CMD_MultiRecruitPartyWait.roomCreateData.multiRoomInfo.worldDungeonId;
		}
		else
		{
			areaId = CMD_MultiRecruitPartyWait.roomJoinData.multiRoomInfo.worldAreaId;
			stageId = CMD_MultiRecruitPartyWait.roomJoinData.multiRoomInfo.worldStageId;
			dungeonId = CMD_MultiRecruitPartyWait.roomJoinData.multiRoomInfo.worldDungeonId;
		}
		QuestBonusPack questBonusPack = new QuestBonusPack();
		questBonusPack.CreateQuestBonus(areaId, stageId, dungeonId);
		return questBonusPack;
	}

	private void UpdateMonsterUISingle(bool isReset, int updatePosition)
	{
		PartsMultiRecruitMonsInfo partsMultiRecruitMonsInfo = this.monsterInfoList[updatePosition];
		partsMultiRecruitMonsInfo.positionNumber = updatePosition;
		if (this.monsterDataList[updatePosition] != null)
		{
			if (this.isEndFirstSetMonsterUI || this.monsterDataList[updatePosition].userMonster.userId == this.myUserId)
			{
				partsMultiRecruitMonsInfo.ChangeRecruitMode(false);
				if (this.monsterDataList[updatePosition].userMonster.userId == this.leaderMonsterData.userMonster.userId)
				{
					partsMultiRecruitMonsInfo.SetMonsterUserType(CMD_MultiRecruitPartyWait.USER_TYPE.OWNER);
				}
				else
				{
					partsMultiRecruitMonsInfo.SetMonsterUserType(CMD_MultiRecruitPartyWait.USER_TYPE.MEMBER);
				}
				partsMultiRecruitMonsInfo.Data = this.monsterDataList[updatePosition];
				partsMultiRecruitMonsInfo.ShowGUI();
			}
			else
			{
				partsMultiRecruitMonsInfo.ChangeWaitMode();
			}
			string nickname;
			string titleId;
			if (this.memberList[updatePosition] != null)
			{
				nickname = this.memberList[updatePosition].nickname;
				titleId = this.memberList[updatePosition].titleId;
				if (this.memberList[updatePosition].userId != this.myUserId && isReset)
				{
					this.TCPSendUserIdList.Add(int.Parse(this.memberList[updatePosition].userId));
				}
			}
			else
			{
				nickname = this.memberList[0].nickname;
				titleId = this.memberList[0].titleId;
			}
			CMD_MultiRecruitPartyWait.USER_TYPE playerNum;
			if (this.monsterDataList[updatePosition].userMonster.userId == this.leaderMonsterData.userMonster.userId)
			{
				playerNum = CMD_MultiRecruitPartyWait.USER_TYPE.OWNER;
			}
			else if (updatePosition == 1)
			{
				playerNum = CMD_MultiRecruitPartyWait.USER_TYPE.MEMBER;
			}
			else
			{
				playerNum = CMD_MultiRecruitPartyWait.USER_TYPE.MEMBER2;
			}
			partsMultiRecruitMonsInfo.AddInitLabel(nickname, titleId, playerNum, updatePosition == 0);
			if (this.monsterDataList[updatePosition].userMonster.userId != this.myUserId || (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER && updatePosition == 2))
			{
				partsMultiRecruitMonsInfo.GetComponent<BoxCollider>().enabled = false;
				partsMultiRecruitMonsInfo.SetStatusPanelActiveCollider(false);
			}
			this.partsMonsInfoList.Add(partsMultiRecruitMonsInfo);
		}
		else
		{
			partsMultiRecruitMonsInfo.Data = null;
			partsMultiRecruitMonsInfo.DoDestroy();
			partsMultiRecruitMonsInfo.SetTagReady(false);
			partsMultiRecruitMonsInfo.ClearArousal();
			if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
			{
				partsMultiRecruitMonsInfo.ChangeRecruitMode(true);
			}
			else
			{
				partsMultiRecruitMonsInfo.ChangeWaitMode();
			}
			partsMultiRecruitMonsInfo.GetComponent<BoxCollider>().enabled = false;
		}
		partsMultiRecruitMonsInfo.HideStatusPanel();
	}

	public void ClearStExchange()
	{
		for (int i = 0; i < 3; i++)
		{
			PartsMultiRecruitMonsInfo partsMultiRecruitMonsInfo = this.monsterInfoList[i];
			partsMultiRecruitMonsInfo.HideStatusPanel();
		}
	}

	private void CheckAndSetOwnerSubMonster()
	{
		int count = this.TCPSendUserIdList.Count;
		if (count != 0)
		{
			if (count == 1)
			{
				for (int i = 1; i < 3; i++)
				{
					if (this.monsterDataList[i] == null || this.monsterDataList[i].userMonster.userId == this.ownerSubMonsterData.userMonster.userId)
					{
						this.monsterDataList[i] = this.ownerSubMonsterData;
						this.UpdateMonsterUISingle(false, i);
						if (this.partsMonsInfoList[0].GetTagReady())
						{
							this.UpdateReadyState(this.ownerSubMonsterData.userMonster.userId, true);
						}
						break;
					}
				}
			}
		}
		else
		{
			for (int j = 1; j < 3; j++)
			{
				if (this.monsterDataList[j] != null)
				{
					this.monsterDataList[j] = null;
					this.UpdateMonsterUISingle(false, j);
					break;
				}
			}
		}
	}

	private void AnotherMemberRoomOut(string userId)
	{
		if (this.isBattleClicked || this.isGoingBattle)
		{
			return;
		}
		foreach (var <>__AnonType in this.memberList.Select((GameWebAPI.Common_MultiMemberList v, int i) => new
		{
			v,
			i
		}))
		{
			if (<>__AnonType.v != null && <>__AnonType.v.userId == userId)
			{
				this.memberList[<>__AnonType.i] = null;
			}
		}
		foreach (var <>__AnonType2 in this.monsterDataList.Select((MonsterData v, int i) => new
		{
			v,
			i
		}))
		{
			if ((<>__AnonType2.v != null && <>__AnonType2.v.userMonster != null && <>__AnonType2.v.userMonster.userId == userId) || (<>__AnonType2.v != null && <>__AnonType2.v.userMonster == this.ownerSubMonsterData.userMonster))
			{
				this.monsterDataList[<>__AnonType2.i] = null;
			}
		}
		this.CloseAllModal();
		this.SetMonsterUI();
		this.CheckAndSetOwnerSubMonster();
		this.SetBtnDecide();
	}

	private void OnClickReady()
	{
		if (this.isReady)
		{
			this.cmdConfirm = (GUIMain.ShowCommonDialog(new Action<int>(this.OnClickReadyExec), "CMD_Confirm", null) as CMD_Confirm);
			this.cmdConfirm.Title = StringMaster.GetString("MultiRecruit-03");
			this.cmdConfirm.Info = StringMaster.GetString("MultiRecruit-04");
		}
		else
		{
			this.OnClickReadyExec(0);
			this.cmdModalMessage = (GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage);
			this.cmdModalMessage.Title = StringMaster.GetString("MultiRecruit-05");
			this.cmdModalMessage.Info = StringMaster.GetString("MultiRecruit-06");
		}
	}

	private void OnClickReadyExec(int idx)
	{
		if (idx == 0)
		{
			this.isReady = !this.isReady;
			this.SetReadyBtnLabel(this.CheckSortieLimit());
			this.UpdateReadyState(this.myUserId, this.isReady);
			this.SetBtnDecide();
			AppCoroutine.Start(this.SendTCPReady(), true);
		}
	}

	public void OnClickStExchange()
	{
		for (int i = 0; i < 3; i++)
		{
			PartsMultiRecruitMonsInfo partsMultiRecruitMonsInfo = this.monsterInfoList[i];
			partsMultiRecruitMonsInfo.StatusPageChange();
		}
	}

	public void OnPassCodeCopy()
	{
		Clipboard.Text = this.lbTXT_PASSWORD.text;
		this.cmdModalMessage = (GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage);
		this.cmdModalMessage.Title = StringMaster.GetString("SystemCopy");
		this.cmdModalMessage.Info = StringMaster.GetString("MultiRecruit-02");
	}

	private void OnClickFriend()
	{
		if (this.recruitedFriendIdList == null)
		{
			this.recruitedFriendIdList = new List<string>();
		}
		CMD_MultiRecruitFriend cmd_MultiRecruitFriend = GUIMain.ShowCommonDialog(null, "CMD_MultiRecruitFriend", null) as CMD_MultiRecruitFriend;
		cmd_MultiRecruitFriend.roomId = int.Parse(this.roomInfo.multiRoomId);
	}

	private void OnClickChat()
	{
		GUIMain.ShowCommonDialog(null, "CMD_MultiRecruitChatList", null);
	}

	private void OnClickCancel()
	{
		if (this.isDispErrorModal)
		{
			return;
		}
		this.isCancelClicked = true;
		this.OnClickCancelExec(this.SendTCPRoomOut(CMD_MultiRecruitPartyWait.ROOM_OUT_TYPE.DEFAULT));
	}

	private void OnClickCancelExec(IEnumerator ie)
	{
		MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		RequestBase requestBase;
		if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
		{
			requestBase = new GameWebAPI.MultiRoomBreakup
			{
				OnReceived = delegate(WebAPI.ResponseData resData)
				{
					MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
					AppCoroutine.Start(ie, true);
				}
			};
		}
		else
		{
			GameWebAPI.MultiRoomLeave multiRoomLeave = new GameWebAPI.MultiRoomLeave();
			multiRoomLeave.SetSendData = delegate(GameWebAPI.ReqData_MultiRoomLeave param)
			{
				param.roomId = int.Parse(CMD_MultiRecruitPartyWait.roomJoinData.multiRoomInfo.multiRoomId);
			};
			multiRoomLeave.OnReceived = delegate(WebAPI.ResponseData resData)
			{
				MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
				AppCoroutine.Start(ie, true);
			};
			requestBase = multiRoomLeave;
		}
		RequestBase request = requestBase;
		if (CMD_MultiRecruitPartyWait.<>f__mg$cache0 == null)
		{
			CMD_MultiRecruitPartyWait.<>f__mg$cache0 = new Action(RestrictionInput.EndLoad);
		}
		AppCoroutine.Start(request.Run(CMD_MultiRecruitPartyWait.<>f__mg$cache0, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null), true);
	}

	private void OnClickStart()
	{
		this.cmdConfirm = (GUIMain.ShowCommonDialog(new Action<int>(this.OnClickStartExec), "CMD_Confirm", null) as CMD_Confirm);
		this.cmdConfirm.Title = StringMaster.GetString("MultiRecruit-07");
		if (this.TCPSendUserIdList.Count < 2)
		{
			this.cmdConfirm.Info = StringMaster.GetString("MultiRecruit-09");
		}
		else
		{
			this.cmdConfirm.Info = StringMaster.GetString("MultiRecruit-08");
		}
	}

	private void OnClickStartExec(int idx)
	{
		if (this.TCPSendUserIdList.Count > 0 && idx == 0 && CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
		{
			if (this.isBattleClicked)
			{
				return;
			}
			this.isBattleClicked = true;
			this.SendTCPBattleStart();
		}
	}

	private void SendTCPEmotion(int selectedEmotionType)
	{
		EmotionData message = new EmotionData
		{
			playerUserId = this.myUserId,
			emotionType = selectedEmotionType,
			iconSpritesIndex = this.myPositionNumber
		};
		this.emotionSenderMulti.SetEmotion(this.myPositionNumber, selectedEmotionType, false);
		SoundPlayer.PlayButtonSelect();
		Singleton<TCPUtil>.Instance.SendMessageForTarget(TCPMessageType.Emotion, message, this.TCPSendUserIdList, "multiRecruit");
	}

	private IEnumerator SendTCPShareUserInfo(MonsterData md, bool isRequestMemberData = false, bool isOwnerSubMonster = false)
	{
		if (this.TCPSendUserIdList.Count > 0)
		{
			RecruitShareUserInfo recruitShareUserInfo = new RecruitShareUserInfo();
			recruitShareUserInfo.monsInfo = new GameWebAPI.Common_MonsterData(md);
			recruitShareUserInfo.roomId = this.roomInfo.multiRoomId;
			recruitShareUserInfo.userId = this.myUserId;
			recruitShareUserInfo.isReady = this.isReady;
			recruitShareUserInfo.nickname = this.myNickname;
			recruitShareUserInfo.titleId = this.myTitleId;
			recruitShareUserInfo.isRequestMemberData = isRequestMemberData;
			recruitShareUserInfo.hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.RecruitShareUserInfo, this.myUserId, TCPMessageType.None);
			recruitShareUserInfo.positionNumber = this.myPositionNumber;
			if (isOwnerSubMonster && CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
			{
				recruitShareUserInfo.subMonsInfo = new GameWebAPI.Common_MonsterData(this.ownerSubMonsterData);
			}
			Action<List<int>> sendFaildAction = null;
			if (isRequestMemberData)
			{
				sendFaildAction = new Action<List<int>>(this.SendFaildRequestMember);
			}
			AppCoroutine.Start(Singleton<TCPUtil>.Instance.SendMessageInsistently<RecruitShareUserInfo>(TCPMessageType.RecruitShareUserInfo, recruitShareUserInfo, this.TCPSendUserIdList, "multiRecruit", sendFaildAction), false);
			yield break;
		}
		yield break;
	}

	private void SendFaildRequestMember(List<int> faildMemberList)
	{
		using (List<int>.Enumerator enumerator = faildMemberList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int member = enumerator.Current;
				CMD_MultiRecruitPartyWait $this = this;
				if (member == int.Parse(this.leaderMonsterData.userMonster.userId))
				{
					this.GetRoomStatus(delegate(int x)
					{
						$this.UpdateMonsterUISingle(false, 0);
						$this.CheckAndSetOwnerSubMonster();
						$this.TCPSendUserIdList.RemoveAll((int user) => user == member);
					});
				}
				else
				{
					this.SendTCPGetMemberList();
				}
			}
		}
	}

	private IEnumerator SendTCPRoomOut(CMD_MultiRecruitPartyWait.ROOM_OUT_TYPE roomoutType)
	{
		if (this.TCPSendUserIdList.Count > 0)
		{
			RecruitRoomOut message = new RecruitRoomOut
			{
				roomId = this.roomInfo.multiRoomId,
				userId = this.myUserId,
				roomOutType = (int)roomoutType,
				positionNumber = this.myPositionNumber,
				hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.RecruitRoomOut, this.myUserId, TCPMessageType.None)
			};
			IEnumerator wait = Singleton<TCPUtil>.Instance.SendMessageInsistently<RecruitRoomOut>(TCPMessageType.RecruitRoomOut, message, this.TCPSendUserIdList, "multiRecruit", null);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
		}
		if (roomoutType != CMD_MultiRecruitPartyWait.ROOM_OUT_TYPE.QUEST_ENDED)
		{
			Singleton<TCPUtil>.Instance.TCPDisConnect(false);
			this.CloseAllModal();
			this.ClosePanel(true);
		}
		else
		{
			AppCoroutine.Start(this.DispErrorModal(StringMaster.GetString("QuestEventTitle"), StringMaster.GetString("QuestEventInfo2")), true);
		}
		yield break;
	}

	private IEnumerator SendTCPReady()
	{
		if (this.TCPSendUserIdList.Count > 0)
		{
			RecruitReady message = new RecruitReady
			{
				roomId = this.roomInfo.multiRoomId,
				userId = this.myUserId,
				isReady = this.isReady,
				hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.RecruitReady, this.myUserId, TCPMessageType.None)
			};
			IEnumerator wait = Singleton<TCPUtil>.Instance.SendMessageInsistently<RecruitReady>(TCPMessageType.RecruitReady, message, this.TCPSendUserIdList, "multiRecruit", null);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			yield break;
		}
		yield break;
	}

	private void SendTCPBattleStart()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<BattleStartConfirm.userData> umis = new List<BattleStartConfirm.userData>();
		GameWebAPI.WD_Req_DngStart lastDngReq = DataMng.Instance().GetResultUtilData().GetLastDngReq();
		int userDungeonTicketId = -1;
		if (CMD_QuestTOP.instance != null)
		{
			userDungeonTicketId = ((CMD_QuestTOP.instance.StageDataBk.dungeon.userDungeonTicketId != null) ? int.Parse(CMD_QuestTOP.instance.StageDataBk.dungeon.userDungeonTicketId) : 0);
		}
		else if (lastDngReq != null)
		{
			userDungeonTicketId = ((lastDngReq.userDungeonTicketId != null) ? int.Parse(lastDngReq.userDungeonTicketId) : 0);
		}
		else
		{
			global::Debug.LogError("====================================== CMD_MultiRecruitPartyWait userDungeonTicketId");
		}
		BattleStartConfirm battleStartConfirm = new BattleStartConfirm
		{
			ri = int.Parse(this.roomInfo.multiRoomId),
			umis = umis,
			mpid = 0,
			userDungeonTicketId = userDungeonTicketId
		};
		for (int i = 1; i < this.monsterDataList.Count; i++)
		{
			if (this.monsterDataList[i].userMonster.userId == this.ownerSubMonsterData.userMonster.userId)
			{
				battleStartConfirm.mpid = i;
			}
		}
		foreach (GameWebAPI.Common_MultiMemberList common_MultiMemberList in this.memberList)
		{
			if (common_MultiMemberList != null)
			{
				BattleStartConfirm.userData userData = new BattleStartConfirm.userData();
				userData.ui = int.Parse(common_MultiMemberList.userId);
				userData.umi = new List<int>();
				foreach (MonsterData monsterData in this.monsterDataList)
				{
					if (monsterData != null && monsterData.userMonster.userId == common_MultiMemberList.userId)
					{
						userData.umi.Add(int.Parse(monsterData.userMonster.userMonsterId));
					}
				}
				battleStartConfirm.umis.Add(userData);
			}
		}
		dictionary.Add("820101", battleStartConfirm);
		Singleton<TCPUtil>.Instance.SendTCPRequest(dictionary, "activityList");
	}

	private void SendTCPRoomResume()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		RecruitRoomResume value = new RecruitRoomResume
		{
			ri = int.Parse(this.roomInfo.multiRoomId)
		};
		dictionary.Add("820106", value);
		Singleton<TCPUtil>.Instance.SendTCPRequest(dictionary, "activityList");
	}

	private void SendTCPGetMemberList()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		RecruitMemberList value = new RecruitMemberList
		{
			ri = int.Parse(this.roomInfo.multiRoomId)
		};
		dictionary.Add("820103", value);
		Singleton<TCPUtil>.Instance.SendTCPRequest(dictionary, "activityList");
	}

	private void GetTCPReceponseData(Dictionary<string, object> arg)
	{
		if (arg.ContainsKey("multiRecruit"))
		{
			Dictionary<object, object> dictionary = arg["multiRecruit"] as Dictionary<object, object>;
			string text = dictionary.Keys.First<object>().ToString();
			TCPMessageType tcpMessageType = MultiTools.StringToEnum<TCPMessageType>(text);
			this.RunRecieverPlayerActions(tcpMessageType, dictionary[text]);
		}
		else if (arg.ContainsKey("820101"))
		{
			if (this.isCancelClicked)
			{
				return;
			}
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			bool flag = false;
			int startId = 0;
			foreach (KeyValuePair<string, object> keyValuePair in arg)
			{
				Dictionary<object, object> dictionary2 = (Dictionary<object, object>)keyValuePair.Value;
				foreach (KeyValuePair<object, object> keyValuePair2 in dictionary2)
				{
					if (keyValuePair2.Key.ToString() == "sid")
					{
						flag = true;
						startId = Convert.ToInt32(keyValuePair2.Value);
						break;
					}
					if (keyValuePair2.Key.ToString() == "errorCode")
					{
						global::Debug.Log("例外: " + keyValuePair2.Key.ToString() + " " + keyValuePair2.Value.ToString());
						MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
						if (keyValuePair2.Value.ToString() == "E-WD10")
						{
							AppCoroutine.Start(this.SendTCPRoomOut(CMD_MultiRecruitPartyWait.ROOM_OUT_TYPE.QUEST_ENDED), true);
						}
						else
						{
							AppCoroutine.Start(this.DispErrorModal(StringMaster.GetString("AlertNetworkErrorTitle"), keyValuePair2.Key.ToString() + " " + keyValuePair2.Value.ToString()), true);
						}
						break;
					}
				}
			}
			if (flag)
			{
				GameWebAPI.RespData_WorldMultiStartInfo startInfo = null;
				GameWebAPI.WorldMultiStartInfo request = new GameWebAPI.WorldMultiStartInfo
				{
					SetSendData = delegate(GameWebAPI.ReqData_WorldMultiStartInfo param)
					{
						param.startId = startId;
					},
					OnReceived = delegate(GameWebAPI.RespData_WorldMultiStartInfo response)
					{
						startInfo = response;
					}
				};
				AppCoroutine.Start(request.Run(delegate()
				{
					RestrictionInput.EndLoad();
					this.PrepareGoToBattleScene(startInfo);
				}, delegate(Exception noop)
				{
					RestrictionInput.EndLoad();
				}, null), true);
			}
		}
		else if (arg.ContainsKey("820106"))
		{
			foreach (KeyValuePair<string, object> keyValuePair3 in arg)
			{
				Dictionary<object, object> dictionary3 = (Dictionary<object, object>)keyValuePair3.Value;
				foreach (KeyValuePair<object, object> keyValuePair4 in dictionary3)
				{
					if (keyValuePair4.Key.ToString() == "errorCode")
					{
						this.ForceRoomOut(CMD_MultiRecruitPartyWait.ROOM_OUT_TYPE.TIMEOUT);
						break;
					}
				}
			}
		}
		else if (arg.ContainsKey("820103"))
		{
			foreach (KeyValuePair<string, object> keyValuePair5 in arg)
			{
				List<object> source = (List<object>)keyValuePair5.Value;
				int i;
				for (i = 0; i < this.TCPSendUserIdList.Count; i++)
				{
					if (!source.Any((object user) => int.Parse(user.ToString()) == this.TCPSendUserIdList[i]))
					{
						global::Debug.Log(this.TCPSendUserIdList[i].ToString() + "は既にいなくなっています");
						this.AnotherMemberRoomOut(this.TCPSendUserIdList[i].ToString());
					}
				}
				this.TCPSendUserIdList = source.Where((object user) => int.Parse(user.ToString()) != 0 && user.ToString() != this.myUserId).Select((object user) => int.Parse(user.ToString())).ToList<int>();
				if (this.TCPSendUserIdList.Count > 0)
				{
					if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
					{
						AppCoroutine.Start(this.SendTCPShareUserInfo(this.leaderMonsterData, true, true), true);
						this.isOwnerReturnRoom = true;
					}
					else
					{
						AppCoroutine.Start(this.SendTCPShareUserInfo(this.myMonsterData, true, false), true);
					}
				}
			}
		}
		else if (arg.ContainsKey("800012"))
		{
			bool flag2 = false;
			foreach (KeyValuePair<string, object> keyValuePair6 in arg)
			{
				Dictionary<object, object> dictionary4 = (Dictionary<object, object>)keyValuePair6.Value;
				using (Dictionary<object, object>.Enumerator enumerator7 = dictionary4.GetEnumerator())
				{
					while (enumerator7.MoveNext())
					{
						KeyValuePair<object, object> c = enumerator7.Current;
						global::Debug.Log("切断データ受信");
						global::Debug.Log(c.Key + " == " + c.Value);
						if (c.Key.ToString() == "ri" && c.Value.ToString() == this.roomInfo.multiRoomId)
						{
							flag2 = true;
						}
						else if (c.Key.ToString() == "ui" && flag2)
						{
							global::Debug.Log(c.Value.ToString() + " が切断しました");
							if (c.Value.ToString() != this.leaderMonsterData.userMonster.userId)
							{
								this.AnotherMemberRoomOut(c.Value.ToString());
							}
							else if (this.partsMonsInfoList[0].Data == null)
							{
								List<int> faildMemberList = new List<int>
								{
									int.Parse(c.Value.ToString())
								};
								this.SendFaildRequestMember(faildMemberList);
							}
							else
							{
								this.TCPSendUserIdList.RemoveAll((int user) => user.ToString() == c.Value.ToString());
							}
						}
					}
				}
			}
		}
	}

	private bool RunRecieverPlayerActions(TCPMessageType tcpMessageType, object messageObj)
	{
		switch (tcpMessageType)
		{
		case TCPMessageType.RecruitShareUserInfo:
		{
			RecruitShareUserInfo recruitShareUserInfo = TCPData<RecruitShareUserInfo>.Convert(messageObj);
			bool flag = Singleton<TCPUtil>.Instance.CheckHash(recruitShareUserInfo.hashValue, this.hashValueQueue);
			if (recruitShareUserInfo.roomId == this.roomInfo.multiRoomId && !flag)
			{
				if (this.isBattleClicked || this.isGoingBattle || this.isCancelClicked)
				{
					return false;
				}
				GameWebAPI.Common_MultiMemberList common_MultiMemberList = new GameWebAPI.Common_MultiMemberList();
				common_MultiMemberList.nickname = recruitShareUserInfo.nickname;
				common_MultiMemberList.userId = recruitShareUserInfo.userId;
				common_MultiMemberList.titleId = recruitShareUserInfo.titleId;
				MonsterData value = recruitShareUserInfo.monsInfo.ToMonsterData();
				this.memberList[recruitShareUserInfo.positionNumber] = common_MultiMemberList;
				this.monsterDataList[recruitShareUserInfo.positionNumber] = value;
				if (recruitShareUserInfo.positionNumber == 0)
				{
					this.leaderMonsterData = value;
					this.haveOwner = true;
				}
				this.UpdateMonsterUISingle(recruitShareUserInfo.isRequestMemberData, recruitShareUserInfo.positionNumber);
				this.RefreshLeaderSkillText();
				this.ClearStExchange();
				if (recruitShareUserInfo.isRequestMemberData)
				{
					this.CloseAllModal();
					if (!base.gameObject.activeSelf)
					{
						base.gameObject.SetActive(true);
					}
					if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
					{
						AppCoroutine.Start(this.SendTCPShareUserInfo(this.leaderMonsterData, false, true), true);
						this.CheckAndSetOwnerSubMonster();
					}
					else
					{
						AppCoroutine.Start(this.SendTCPShareUserInfo(this.myMonsterData, false, false), true);
					}
				}
				if (recruitShareUserInfo.subMonsInfo != null)
				{
					this.ownerSubMonsterData = recruitShareUserInfo.subMonsInfo.ToMonsterData();
					this.CheckAndSetOwnerSubMonster();
				}
				this.UpdateReadyState(recruitShareUserInfo.userId, recruitShareUserInfo.isReady);
				if (this.isOwnerReturnRoom)
				{
					this.CheckAndSetOwnerSubMonster();
				}
				if (recruitShareUserInfo.isRequestMemberData || this.isOwnerReturnRoom)
				{
					this.SetBtnDecide();
				}
			}
			Singleton<TCPUtil>.Instance.SendConfirmationJoin(TCPMessageType.RecruitShareUserInfo, recruitShareUserInfo.userId, this.myUserId, "multiRecruit", this.isBattleClicked || this.isGoingBattle);
			break;
		}
		case TCPMessageType.RecruitRoomOut:
		{
			RecruitRoomOut recruitRoomOut = TCPData<RecruitRoomOut>.Convert(messageObj);
			bool flag = Singleton<TCPUtil>.Instance.CheckHash(recruitRoomOut.hashValue, this.hashValueQueue);
			if (!this.CheckStillAlive(int.Parse(recruitRoomOut.userId)))
			{
				return false;
			}
			if (recruitRoomOut.roomId == this.roomInfo.multiRoomId && !flag)
			{
				if (recruitRoomOut.userId != this.leaderMonsterData.userMonster.userId)
				{
					this.AnotherMemberRoomOut(recruitRoomOut.userId);
				}
				else if (recruitRoomOut.roomOutType == 4)
				{
					AppCoroutine.Start(this.DispErrorModal(StringMaster.GetString("QuestEventTitle"), StringMaster.GetString("QuestEventInfo2")), true);
				}
				else
				{
					this.ForceRoomOut(CMD_MultiRecruitPartyWait.ROOM_OUT_TYPE.BREAK);
				}
			}
			Singleton<TCPUtil>.Instance.SendConfirmation(TCPMessageType.RecruitRoomOut, recruitRoomOut.userId, this.myUserId, "multiRecruit");
			break;
		}
		case TCPMessageType.RecruitReady:
		{
			RecruitReady recruitReady = TCPData<RecruitReady>.Convert(messageObj);
			bool flag = Singleton<TCPUtil>.Instance.CheckHash(recruitReady.hashValue, this.hashValueQueue);
			if (!this.CheckStillAlive(int.Parse(recruitReady.userId)))
			{
				return false;
			}
			if (recruitReady.roomId == this.roomInfo.multiRoomId && !flag)
			{
				this.UpdateReadyState(recruitReady.userId, recruitReady.isReady);
				this.SetBtnDecide();
				if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER && !recruitReady.isReady)
				{
					this.CloseAllModal();
				}
			}
			Singleton<TCPUtil>.Instance.SendConfirmation(TCPMessageType.RecruitReady, recruitReady.userId, this.myUserId, "multiRecruit");
			break;
		}
		default:
			switch (tcpMessageType)
			{
			case TCPMessageType.None:
				return false;
			default:
				if (tcpMessageType == TCPMessageType.Confirmation)
				{
					ConfirmationData confirmationData = TCPData<ConfirmationData>.Convert(messageObj);
					bool flag = Singleton<TCPUtil>.Instance.CheckHash(confirmationData.hashValue, this.hashValueQueue);
					bool flag2 = this.CheckStillAlive(int.Parse(confirmationData.playerUserId));
					if (flag || !flag2)
					{
						return false;
					}
					bool flag3 = false;
					for (int i = 0; i < Singleton<TCPUtil>.Instance.confirmationChecks[(TCPMessageType)confirmationData.tcpMessageType].Count; i++)
					{
						if (confirmationData.playerUserId == Singleton<TCPUtil>.Instance.confirmationChecks[(TCPMessageType)confirmationData.tcpMessageType][i])
						{
							flag3 = true;
						}
					}
					if (!flag3)
					{
						Singleton<TCPUtil>.Instance.confirmationChecks[(TCPMessageType)confirmationData.tcpMessageType].Add(confirmationData.playerUserId);
					}
					this.ownerGoingBattle = (confirmationData.value1 == "True" || confirmationData.value1 == "true");
					if (!this.haveOwner && this.ownerGoingBattle)
					{
						this.OnClickCancelExec(this.DispErrorModal(StringMaster.GetString("AlertNetworkErrorTitle"), StringMaster.GetString("AlertNetworkErrorInfo")));
					}
					global::Debug.LogFormat("{0}から確認用{1}を受信しました", new object[]
					{
						confirmationData.playerUserId,
						(TCPMessageType)confirmationData.tcpMessageType
					});
				}
				break;
			case TCPMessageType.Emotion:
			{
				EmotionData emotionData = TCPData<EmotionData>.Convert(messageObj);
				if (!this.CheckStillAlive(int.Parse(emotionData.playerUserId)))
				{
					return false;
				}
				if (base.gameObject.activeSelf)
				{
					this.emotionSenderMulti.SetEmotion(emotionData.iconSpritesIndex, emotionData.emotionType, true);
				}
				break;
			}
			}
			break;
		}
		return true;
	}

	private void OnExitTCP()
	{
		global::Debug.Log("OnExit");
	}

	private void OnExceptionTCP(short errorCode, string errorMessage)
	{
		global::Debug.Log("erroeCode >>> " + errorCode);
		global::Debug.Log("errorMsg >>> " + errorMessage);
		if (this.isGoingBattle || this.isDispErrorModal || this.isCancelClicked)
		{
			return;
		}
		float connectWait = 0f;
		if (errorMessage.Contains("Network subsystem is down") || errorMessage.Contains("Connection timed out"))
		{
			connectWait = 7f;
		}
		if (errorCode != 741)
		{
			MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
			{
				if (this.cmdConfirm != null)
				{
					return;
				}
				this.CloseAllModal();
				this.cmdConfirm = (GUIMain.ShowCommonDialog(delegate(int x)
				{
					if (x == 0)
					{
						AppCoroutine.Start(this.ReturnRoom(connectWait), true);
					}
					else
					{
						this.ForceRoomOut(CMD_MultiRecruitPartyWait.ROOM_OUT_TYPE.MEMBER_EXPIRE);
					}
				}, "CMD_Confirm", null) as CMD_Confirm);
				this.cmdConfirm.Title = StringMaster.GetString("AlertNetworkErrorTitle");
				this.cmdConfirm.Info = StringMaster.GetString("MultiRecruit-15");
			}
			else
			{
				AppCoroutine.Start(this.DispErrorModal(StringMaster.GetString("AlertNetworkErrorTitle"), StringMaster.GetString("AlertNetworkErrorInfo")), true);
			}
		}
		else
		{
			global::Debug.Log("バックグラウンド化されたため、通信切断発生");
		}
	}

	public void OnApplicationPause(bool pauseStatus)
	{
		if (this.isGoingBattle || this.isDispErrorModal || this.isCancelClicked)
		{
			return;
		}
		if (!pauseStatus)
		{
			if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
			{
				if (!base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(true);
				}
				AppCoroutine.Start(this.ReturnRoom(0f), true);
			}
			else
			{
				this.ForceRoomOut(CMD_MultiRecruitPartyWait.ROOM_OUT_TYPE.MEMBER_EXPIRE);
			}
		}
	}

	private void GetRoomStatus(Action<int> successAction = null)
	{
		MultiTools.DispLoading(true, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		GameWebAPI.MultiRoomStatusInfoLogic multiRoomStatusInfoLogic = new GameWebAPI.MultiRoomStatusInfoLogic();
		multiRoomStatusInfoLogic.SetSendData = delegate(GameWebAPI.ReqData_MultiRoomStatusInfoLogic param)
		{
			param.roomId = int.Parse(CMD_MultiRecruitPartyWait.roomJoinData.multiRoomInfo.multiRoomId);
		};
		multiRoomStatusInfoLogic.OnReceived = delegate(GameWebAPI.RespData_MultiRoomStatusInfoLogic resData)
		{
			MultiTools.DispLoading(false, RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			int num = int.Parse(resData.statusInfo.status);
			if (num != 1 && num != 10)
			{
				global::Debug.Log("この部屋は既に解散されている");
				this.ForceRoomOut(CMD_MultiRecruitPartyWait.ROOM_OUT_TYPE.BREAK);
			}
			else
			{
				global::Debug.Log("募集中or中断中のため待機します");
				if (successAction != null)
				{
					successAction(0);
				}
			}
		};
		RequestBase requestBase = multiRoomStatusInfoLogic;
		RequestBase request = requestBase;
		if (CMD_MultiRecruitPartyWait.<>f__mg$cache1 == null)
		{
			CMD_MultiRecruitPartyWait.<>f__mg$cache1 = new Action(RestrictionInput.EndLoad);
		}
		AppCoroutine.Start(request.Run(CMD_MultiRecruitPartyWait.<>f__mg$cache1, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null), true);
	}

	public void ReloadAllCharacters(bool isShow)
	{
		for (int i = 0; i < this.partsMonsInfoList.Count; i++)
		{
			if (isShow)
			{
				this.partsMonsInfoList[i].ShowCharacter();
			}
			else
			{
				this.partsMonsInfoList[i].ReleaseCharacter();
			}
		}
	}

	private bool CheckStillAlive(int userId)
	{
		return this.TCPSendUserIdList.Any((int user) => user == userId);
	}

	public Vector3 Get3DPos()
	{
		this.v3DPos.x = this.xPos;
		this.xPos -= 1000f;
		return this.v3DPos;
	}

	private IEnumerator DispErrorModal(string title, string info)
	{
		if (this.isGoingBattle || this.isDispErrorModal || this.isCancelClicked)
		{
			yield break;
		}
		this.isDispErrorModal = true;
		this.CloseAllModal();
		yield return null;
		this.cmdModalMessage = (GUIMain.ShowCommonDialog(delegate(int modal)
		{
			if (CMD_MultiRecruitPartyWait.Instance == null || this.isGoingBattle)
			{
				return;
			}
			Singleton<TCPUtil>.Instance.TCPDisConnect(false);
			if (CMD_MultiRecruitFriend.Instance != null)
			{
				CMD_MultiRecruitFriend.Instance.SetCloseAction(delegate(int close)
				{
					this.ClosePanel(true);
				});
				CMD_MultiRecruitFriend.Instance.ClosePanel(true);
			}
			else if (CMD_CharacterDetailed.Instance != null)
			{
				CMD_CharacterDetailed.Instance.SetCloseAction(delegate(int charaClose)
				{
					CMD_DeckList.Instance.SetCloseAction(delegate(int deckClose)
					{
						this.ClosePanel(true);
					});
					CMD_DeckList.Instance.ClosePanel(true);
				});
				CMD_CharacterDetailed.Instance.ClosePanel(true);
			}
			else if (CMD_DeckList.Instance != null)
			{
				CMD_DeckList.Instance.SetCloseAction(delegate(int close)
				{
					this.ClosePanel(true);
				});
				CMD_DeckList.Instance.ClosePanel(true);
			}
			else
			{
				this.ClosePanel(true);
			}
		}, "CMD_ModalMessage", null) as CMD_ModalMessage);
		this.cmdModalMessage.Title = title;
		this.cmdModalMessage.Info = info;
		yield break;
	}

	private void CloseAllModal()
	{
		if (this.cmdConfirm != null && this.cmdConfirm.enabled)
		{
			this.cmdConfirm.SetForceReturnValue(1);
			this.cmdConfirm.ClosePanel(false);
		}
		if (this.cmdModalMessage != null && this.cmdModalMessage.enabled)
		{
			this.cmdModalMessage.ClosePanel(false);
		}
	}

	private void SetBossMonsterIcon(List<GameWebAPI.RespDataWD_GetDungeonInfo.EncountEnemy> enemyList)
	{
		int num = Mathf.Min(enemyList.Count, this.bossIconList.Length);
		if (0 < num)
		{
			this.bossIconRootObject.SetActive(true);
		}
		for (int i = 0; i < num; i++)
		{
			this.bossIconList[i].SetBossInfo(enemyList[i]);
		}
	}

	public void SetSortieLimit()
	{
		if (ClassSingleton<QuestData>.Instance.SelectDungeon != null)
		{
			string worldDungeonId = ClassSingleton<QuestData>.Instance.SelectDungeon.worldDungeonId;
			if (!string.IsNullOrEmpty(worldDungeonId))
			{
				this.sortieLimitList.SetSortieLimit(worldDungeonId.ToInt32());
			}
		}
	}

	public void SetParentDialog(CMD_MultiRecruitTop dialog)
	{
		this.parentDialog = dialog;
	}

	public enum USER_TYPE
	{
		OWNER,
		MEMBER,
		MEMBER2
	}

	private enum ROOM_OUT_TYPE
	{
		DEFAULT,
		BREAK,
		TIMEOUT,
		MEMBER_EXPIRE,
		QUEST_ENDED
	}
}
