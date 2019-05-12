using Master;
using Monster;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CMD_QuestTOP : CMD
{
	public const string POINT_QUEST_TYPE = "3";

	public const string POINT_WITHOUT_RANKING_QUEST_TYPE = "4";

	public static CMD_QuestTOP instance;

	[SerializeField]
	private GameObject goLP_SATGE;

	[SerializeField]
	private GameObject goLP_SATGE_Ticket;

	[SerializeField]
	private GameObject goLP_SATGE_Banner;

	[SerializeField]
	private GameObject goLP_DNG;

	[SerializeField]
	private UserStamina userStamina;

	[SerializeField]
	[Header("クリッピングしているオブジェクト達")]
	private GameObject[] clipObjects;

	[Header("ポイントクエスト用 ROOT")]
	[SerializeField]
	private GameObject goPartsPointROOT;

	[SerializeField]
	[Header("ポイントクエスト用（ランキングなし） ROOT")]
	private GameObject goPartsPointWithoutRankingROOT;

	[Header("ポイントクエスト用 BG")]
	[SerializeField]
	private UITexture txEVENT_BG;

	[Header("降臨エリア用 スケジュールバナー領域")]
	[SerializeField]
	private GameObject goScheduleBannerROOT;

	[Header("降臨エリア用 スケジュールバナー")]
	[SerializeField]
	private GameObject goScheduleBannerParts;

	[Header("降臨エリア用 バナー画像のDLタイムアウト秒")]
	[SerializeField]
	private float timeOutSeconds;

	public bool isGoingBattle;

	private GameObject goSelectPanelA_StageL;

	private GUISelectPanelA_StageL csSelectPanelA_StageL;

	private GameObject goSelectPanelS_DungeonR;

	private GUISelectPanelS_DungeonR csSelectPanelS_DungeonR;

	private List<QuestData.WorldDungeonData> worldDungeonData;

	public int battlePartyDeckNo;

	private bool isScheduleBannerActive;

	private List<QuestData.WorldStageData> worldStageData;

	private GameWebAPI.RespDataWD_PointQuestInfo pointInfo;

	private int needLife;

	private PartsQuestPoint partsQuestPoint;

	private int currentSelected;

	public static QuestData.WorldAreaData AreaData { get; set; }

	protected override void Awake()
	{
		base.Awake();
		CMD_QuestTOP.instance = this;
		ClassSingleton<QuestTOPAccessor>.Instance.questTOP = this;
		DataMng.Instance().GetResultUtilData().ClearLastDngReq();
		ClassSingleton<PlayLimit>.Instance.ClearTicketNumCont();
		ClassSingleton<PlayLimit>.Instance.ClearPlayLimitNumCont();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.HideDLG();
		if (CMD_QuestTOP.AreaData == null)
		{
			CMD_QuestTOP.AreaData = ClassSingleton<QuestData>.Instance.GetWorldAreaM_NormalByAreaId(ClassSingleton<QuestTOPAccessor>.Instance.nextStage.worldAreaId);
		}
		if (CMD_QuestTOP.AreaData == null || !CMD_QuestTOP.AreaData.isActive)
		{
			AppCoroutine.Start(this.CloseImmidiate_OpenQuestSelect(1), false);
			return;
		}
		List<QuestData.WorldStageData> worldStageData_ByAreaID = ClassSingleton<QuestData>.Instance.GetWorldStageData_ByAreaID(CMD_QuestTOP.AreaData.data.worldAreaId, false);
		if (worldStageData_ByAreaID.Count <= 0)
		{
			AppCoroutine.Start(this.CloseImmidiate_OpenQuestSelect(1), false);
			return;
		}
		if (CMD_QuestTOP.AreaData.data.type == "3" || CMD_QuestTOP.AreaData.data.type == "4")
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			if (CMD_QuestTOP.AreaData.data.type == "3")
			{
				this.partsQuestPoint = this.goPartsPointROOT.GetComponent<PartsQuestPoint>();
			}
			else if (CMD_QuestTOP.AreaData.data.type == "4")
			{
				this.partsQuestPoint = this.goPartsPointWithoutRankingROOT.GetComponent<PartsQuestPoint>();
			}
			APIRequestTask apirequestTask = new APIRequestTask();
			apirequestTask.Add(ClassSingleton<QuestData>.Instance.RequestPointQuestInfo(CMD_QuestTOP.AreaData.data.worldAreaId, delegate(GameWebAPI.RespDataWD_PointQuestInfo resp)
			{
				this.pointInfo = resp;
				this.partsQuestPoint.PointInfo = this.pointInfo;
				this.partsQuestPoint.AreaData = CMD_QuestTOP.AreaData;
				this.partsQuestPoint.ShowData();
			}, false));
			base.StartCoroutine(apirequestTask.Run(delegate
			{
				this.IntDLG(f, sizeX, sizeY, aT);
				RestrictionInput.EndLoad();
				if (CMD_QuestTOP.AreaData.data.type == "3")
				{
					this.goPartsPointROOT.SetActive(true);
					this.goPartsPointWithoutRankingROOT.SetActive(false);
				}
				else if (CMD_QuestTOP.AreaData.data.type == "4")
				{
					this.goPartsPointROOT.SetActive(false);
					this.goPartsPointWithoutRankingROOT.SetActive(true);
				}
			}, delegate(Exception nop)
			{
				RestrictionInput.EndLoad();
				this.ClosePanel(false);
			}, null));
		}
		else
		{
			this.goPartsPointROOT.SetActive(false);
			this.goPartsPointWithoutRankingROOT.SetActive(false);
			this.goScheduleBannerROOT.SetActive(false);
			if (CMD_QuestTOP.AreaData.data.worldAreaId == "3")
			{
				IEnumerable<GameWebAPI.RespDataMA_BannerM.BannerM> source = DataMng.Instance().RespData_BannerMaster.bannerM.Where((GameWebAPI.RespDataMA_BannerM.BannerM _banner) => _banner.linkCategoryType == "9" && _banner.actionType == "menu" && ServerDateTime.Now >= DateTime.Parse(_banner.startTime) && GUIBannerParts.GetRestTimeSeconds(DateTime.Parse(_banner.endTime)) > 0);
				this.isScheduleBannerActive = (source.Count<GameWebAPI.RespDataMA_BannerM.BannerM>() >= 1);
				this.goScheduleBannerROOT.SetActive(this.isScheduleBannerActive);
				if (this.isScheduleBannerActive)
				{
					IEnumerator routine = this.BuildBanner(source.ElementAt(0));
					base.StartCoroutine(routine);
				}
			}
			this.IntDLG(f, sizeX, sizeY, aT);
		}
	}

	private IEnumerator BuildBanner(GameWebAPI.RespDataMA_BannerM.BannerM banner)
	{
		GUIBannerParts parts = this.goScheduleBannerParts.GetComponent<GUIBannerParts>();
		if (parts != null)
		{
			parts.name += banner.dispNum.ToString();
			parts.Data = banner;
			parts.SetBGColor();
			string path = ConstValue.APP_ASSET_DOMAIN + "/asset/img" + banner.img;
			yield return TextureManager.instance.Load(path, new Action<Texture2D>(parts.OnBannerReceived), this.timeOutSeconds, true);
		}
		yield break;
	}

	private IEnumerator CloseImmidiate_OpenQuestSelect(int ct)
	{
		while (ct > 0)
		{
			ct--;
			yield return null;
		}
		base.SetCloseAction(delegate(int idx)
		{
			CMD cmd = GUIMain.ShowCommonDialog(new Action<int>(CMD_BattleNextChoice.OnCloseQuestTOP), "CMD_QuestSelect") as CMD;
			cmd.SetForceReturnValue(1);
		});
		this.ClosePanel(false);
		yield break;
	}

	private void IntDLG(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.ShowDLG();
		GUICollider.DisableAllCollider("CMD_QuestTOP");
		SoundMng.Instance().PlayGameBGM("bgm_301");
		base.PartsTitle.SetTitle(StringMaster.GetString("QuestTitle"));
		this.SetCommonUI_A_Stage();
		this.SetCommonUI_S_Dungeon();
		this.InitQuest();
		this.userStamina.SetMode(UserStamina.Mode.QUEST);
		this.userStamina.RefreshParams();
		base.Show(f, sizeX, sizeY, aT);
		string bgpath = this.GetBGPath();
		if (!string.IsNullOrEmpty(bgpath))
		{
			AppCoroutine.Start(this.DownloadBannerTexture(bgpath), false);
		}
		string anyTimeQuestTopTutorialFileName = this.GetAnyTimeQuestTopTutorialFileName(this.worldStageData[0].worldStageM.worldAreaId);
		if (!string.IsNullOrEmpty(anyTimeQuestTopTutorialFileName))
		{
			base.SetTutorialAnyTime(anyTimeQuestTopTutorialFileName);
		}
	}

	private IEnumerator DownloadBannerTexture(string path)
	{
		yield return TextureManager.instance.Load(path, delegate(Texture2D tex)
		{
			if (tex != null)
			{
				this.txEVENT_BG.mainTexture = tex;
				this.txEVENT_BG.gameObject.SetActive(true);
			}
		}, 30f, true);
		yield break;
	}

	private string GetBGPath()
	{
		GameWebAPI.RespDataMA_WorldEventAreaMaster respDataMA_WorldEventAreaMaster = MasterDataMng.Instance().RespDataMA_WorldEventAreaMaster;
		int i;
		for (i = 0; i < respDataMA_WorldEventAreaMaster.worldEventAreaM.Length; i++)
		{
			if (respDataMA_WorldEventAreaMaster.worldEventAreaM[i].worldAreaId == CMD_QuestTOP.AreaData.data.worldAreaId)
			{
				break;
			}
		}
		if (i < respDataMA_WorldEventAreaMaster.worldEventAreaM.Length && !string.IsNullOrEmpty(respDataMA_WorldEventAreaMaster.worldEventAreaM[i].worldEventId))
		{
			string worldEventId = respDataMA_WorldEventAreaMaster.worldEventAreaM[i].worldEventId;
			GameWebAPI.RespDataMA_WorldEventMaster respDataMA_WorldEventMaster = MasterDataMng.Instance().RespDataMA_WorldEventMaster;
			for (int j = 0; j < respDataMA_WorldEventMaster.worldEventM.Length; j++)
			{
				if (respDataMA_WorldEventMaster.worldEventM[j].worldEventId == worldEventId)
				{
					string backgroundImg = respDataMA_WorldEventMaster.worldEventM[j].backgroundImg;
					if (!string.IsNullOrEmpty(backgroundImg))
					{
						return ConstValue.APP_ASSET_DOMAIN + "/asset/img/events/" + backgroundImg;
					}
				}
			}
			return null;
		}
		return null;
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.isGoingBattle)
		{
			base.SetForceReturnValue(0);
			if (CMD_QuestSelect.instance != null)
			{
				CMD_QuestSelect.instance.SetForceReturnValue(0);
			}
			this.CloseAndFarmCamOn(false);
			if (CMD_QuestSelect.instance != null)
			{
				CMD_QuestSelect.instance.ClosePanel(false);
			}
		}
		else
		{
			SoundMng.Instance().PlayGameBGM("bgm_201");
			base.SetForceReturnValue(6);
			CMD_QuestTOP.AreaData = null;
			this.CloseAndFarmCamOn(animation);
		}
		if (this.csSelectPanelA_StageL != null)
		{
			this.csSelectPanelA_StageL.FadeOutAllListParts(null, false);
			this.csSelectPanelA_StageL.SetHideScrollBarAllWays(true);
		}
		if (this.csSelectPanelS_DungeonR != null)
		{
			this.csSelectPanelS_DungeonR.FadeOutAllListParts(null, false);
			this.csSelectPanelS_DungeonR.SetHideScrollBarAllWays(true);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_QuestTOP.instance = null;
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		if (0 < this.worldStageData.Count)
		{
			QuestSecondTutorial.StartQuestTopTutorial(this.worldStageData[0].worldStageM.worldAreaId);
		}
		else
		{
			GUICollider.EnableAllCollider("CMD_QuestTOP");
		}
	}

	private string GetAnyTimeQuestTopTutorialFileName(string worldAreaId)
	{
		string result = string.Empty;
		switch (worldAreaId)
		{
		case "1":
			result = "anytime_second_tutorial_quest";
			break;
		case "2":
			result = "anytime_second_tutorial_quest_week";
			break;
		case "3":
			result = "anytime_second_tutorial_quest_advent";
			break;
		case "8":
			result = string.Empty;
			break;
		case "9":
			result = "anytime_second_tutorial_quest_beginner";
			break;
		}
		return result;
	}

	public void HideClips()
	{
		foreach (GameObject go in this.clipObjects)
		{
			NGUITools.SetActiveSelf(go, false);
		}
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	public static void ChangeSelectA_StageL_S(int idx, bool forceChange = false)
	{
		if (CMD_QuestTOP.instance != null)
		{
			CMD_QuestTOP.instance.ChangeSelectA_StageL(idx, forceChange);
			CMD_QuestTOP.instance.csSelectPanelA_StageL.SetCellAnim(CMD_QuestTOP.instance.currentSelected);
		}
	}

	private void ChangeSelectA_StageL(int idx, bool forceChange = false)
	{
		if (idx < 0)
		{
			idx = 0;
		}
		else if (idx >= this.csSelectPanelA_StageL.partObjs.Count)
		{
			idx = this.csSelectPanelA_StageL.partObjs.Count - 1;
		}
		if (this.currentSelected == idx && !forceChange)
		{
			return;
		}
		this.currentSelected = idx;
		this.csSelectPanelS_DungeonR.ClearActCBFadeAll();
		this.InitS_DungeonRist();
	}

	private void SetCommonUI_A_Stage()
	{
		this.goSelectPanelA_StageL = GUIManager.LoadCommonGUI("SelectListPanel/SelectListPanelA_StageL", base.gameObject);
		this.csSelectPanelA_StageL = this.goSelectPanelA_StageL.GetComponent<GUISelectPanelA_StageL>();
		int depth = this.txEVENT_BG.gameObject.GetComponent<UIWidget>().depth - 1;
		DepthController.SetWidgetDepth_2(this.csSelectPanelA_StageL.transform, depth);
		if (CMD_QuestTOP.AreaData.data.type == "3" || CMD_QuestTOP.AreaData.data.type == "4")
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UISelectPanelParam/SelectPanelParamUD_A_StageL_Point")) as GameObject;
			gameObject.transform.parent = base.transform;
			this.csSelectPanelA_StageL.SetSelectPanelParam(gameObject);
		}
		else if (CMD_QuestTOP.AreaData.data.worldAreaId == "3" && this.isScheduleBannerActive)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("UISelectPanelParam/SelectPanelParamUD_A_StageL_Point")) as GameObject;
			gameObject2.transform.parent = base.transform;
			this.csSelectPanelA_StageL.SetSelectPanelParam(gameObject2);
		}
		Vector3 localPosition = this.goLP_SATGE.transform.localPosition;
		GUICollider component = this.goSelectPanelA_StageL.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition);
		this.csSelectPanelA_StageL.selectParts = this.goLP_SATGE;
		this.csSelectPanelA_StageL.ListWindowViewRect = this.GetRectWindowArea(localPosition);
	}

	private void InitA_StageList()
	{
		if (CMD_QuestTOP.AreaData.data.worldAreaId != "1" && CMD_QuestTOP.AreaData.data.worldAreaId != "8")
		{
			this.worldStageData.Sort(delegate(QuestData.WorldStageData a, QuestData.WorldStageData b)
			{
				if (int.Parse(a.worldStageM.priority) < int.Parse(b.worldStageM.priority))
				{
					return 1;
				}
				if (int.Parse(a.worldStageM.priority) > int.Parse(b.worldStageM.priority))
				{
					return -1;
				}
				return 0;
			});
		}
		this.goLP_SATGE.SetActive(true);
		this.goLP_SATGE_Banner.SetActive(true);
		this.csSelectPanelA_StageL.initLocation = true;
		bool fromResult = ClassSingleton<QuestTOPAccessor>.Instance != null && ClassSingleton<QuestTOPAccessor>.Instance.nextAreaFlg;
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM nextDungeon = (ClassSingleton<QuestTOPAccessor>.Instance == null) ? null : ClassSingleton<QuestTOPAccessor>.Instance.nextDungeon;
		if (CMD_QuestTOP.AreaData.data.worldAreaId == "1")
		{
			this.csSelectPanelA_StageL.selectParts = this.goLP_SATGE;
			this.csSelectPanelA_StageL.AllBuild(this.worldStageData, fromResult, nextDungeon);
		}
		else if (CMD_QuestTOP.AreaData.data.worldAreaId == "8")
		{
			this.csSelectPanelA_StageL.selectParts = this.goLP_SATGE_Ticket;
			this.csSelectPanelA_StageL.AllBuild_Ticket(this.worldStageData, fromResult, nextDungeon);
		}
		else
		{
			this.csSelectPanelA_StageL.selectParts = this.goLP_SATGE_Banner;
			this.currentSelected = this.csSelectPanelA_StageL.AllBuildBanner(this.worldStageData, fromResult, nextDungeon);
		}
		this.goLP_SATGE.SetActive(false);
		this.goLP_SATGE_Banner.SetActive(false);
	}

	private void SetCommonUI_S_Dungeon()
	{
		this.goSelectPanelS_DungeonR = GUIManager.LoadCommonGUI("SelectListPanel/SelectListPanelS_DungeonR", base.gameObject);
		this.csSelectPanelS_DungeonR = this.goSelectPanelS_DungeonR.GetComponent<GUISelectPanelS_DungeonR>();
		int depth = this.txEVENT_BG.gameObject.GetComponent<UIWidget>().depth - 1;
		DepthController.SetWidgetDepth_2(this.csSelectPanelS_DungeonR.transform, depth);
		Vector3 localPosition = this.goLP_DNG.transform.localPosition;
		this.goSelectPanelS_DungeonR.transform.localPosition.y = localPosition.x;
		GUICollider component = this.goSelectPanelS_DungeonR.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition);
		this.csSelectPanelS_DungeonR.selectParts = this.goLP_DNG;
		this.csSelectPanelS_DungeonR.ListWindowViewRect = this.GetRectWindow(localPosition);
	}

	private Rect GetRectWindow(Vector3 v3)
	{
		return new Rect
		{
			xMin = -280f + v3.x,
			xMax = 280f + v3.x,
			yMin = -240f - GUIMain.VerticalSpaceSize,
			yMax = 160f + GUIMain.VerticalSpaceSize
		};
	}

	private Rect GetRectWindowArea(Vector3 v3)
	{
		return new Rect
		{
			xMin = -280f + v3.x,
			xMax = 280f + v3.x,
			yMin = -240f - GUIMain.VerticalSpaceSize,
			yMax = 250f + GUIMain.VerticalSpaceSize
		};
	}

	private void InitS_DungeonRist()
	{
		if (this.worldStageData.Count > this.currentSelected && this.currentSelected >= 0)
		{
			bool flag = ClassSingleton<QuestTOPAccessor>.Instance != null && ClassSingleton<QuestTOPAccessor>.Instance.nextAreaFlg;
			if (!flag)
			{
				this.worldDungeonData = this.worldStageData[this.currentSelected].wddL;
			}
			else if (flag)
			{
				string worldStageId = ClassSingleton<QuestTOPAccessor>.Instance.nextStage.worldStageId;
				for (int i = 0; i < this.worldStageData.Count; i++)
				{
					if (this.worldStageData[i].worldStageM.worldStageId == worldStageId)
					{
						this.worldDungeonData = this.worldStageData[i].wddL;
					}
				}
				ClassSingleton<QuestTOPAccessor>.Instance.nextAreaFlg = false;
			}
		}
		else
		{
			this.worldDungeonData = new List<QuestData.WorldDungeonData>();
		}
		QuestData.WorldStageData worldStageData = this.worldStageData[this.currentSelected];
		if (worldStageData != null && worldStageData.wdi.isOpen == 1)
		{
			this.goLP_DNG.SetActive(true);
			this.csSelectPanelS_DungeonR.initLocation = true;
			List<QuestData.WorldDungeonData> list = new List<QuestData.WorldDungeonData>();
			for (int j = 0; j < this.worldDungeonData.Count; j++)
			{
				list.Add(this.worldDungeonData[j]);
			}
			this.csSelectPanelS_DungeonR.AllBuild(list);
		}
		this.goLP_DNG.SetActive(false);
	}

	public QuestData.WorldStageData GetWorldStageData()
	{
		return this.worldStageData[this.currentSelected];
	}

	public QuestData.WorldDungeonData StageDataBk { get; private set; }

	public string StageNumBk { get; private set; }

	public GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfoBk { get; private set; }

	public void OnClickedDungeon(QuestData.WorldDungeonData data, string stageNum, GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo = null)
	{
		this.StageDataBk = data;
		this.StageNumBk = stageNum;
		this.campaignInfoBk = campaignInfo;
		if (data.worldDungeonM.IsSoloMulti())
		{
			CMD_QuestDetailedPOP.CampaignInfo = campaignInfo;
			GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseQuestDetailedPOP), "CMD_QuestDetailedPOP");
		}
		else if (data.worldDungeonM.IsMultiOnly())
		{
			CMD_QuestDetailedPOP.CampaignInfo = campaignInfo;
			GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseQuestMultiDetailedPOP), "CMD_QuestMultiDetailedPOP");
		}
		else if (data.worldDungeonM.IsSoloOnly())
		{
			CMD_QuestDetailedPOP.CampaignInfo = campaignInfo;
			GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseQuestDetailedPOP), "CMD_QuestSoloDetailedPOP");
		}
	}

	private bool CanPlayDungeonStamina()
	{
		bool result = false;
		int stamina = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.stamina;
		int num = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.staminaMax);
		this.needLife = int.Parse(this.StageDataBk.worldDungeonM.needStamina);
		if (this.campaignInfoBk != null)
		{
			float num2 = (float)this.needLife;
			this.needLife = Mathf.CeilToInt(num2 * float.Parse(this.campaignInfoBk.rate));
		}
		int point = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
		if (num < this.needLife)
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("QuestNormal");
			cmd_ModalMessage.Info = StringMaster.GetString("QuestStaminaOver");
		}
		else if (stamina < this.needLife)
		{
			if (point >= ConstValue.RECOVER_STAMINA_DIGISTONE_NUM)
			{
				CMD_ChangePOP_STONE cmd_ChangePOP_STONE = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP_STONE") as CMD_ChangePOP_STONE;
				cmd_ChangePOP_STONE.Title = StringMaster.GetString("StaminaShortageTitle");
				string info = string.Format(StringMaster.GetString("StaminaShortageInfo"), new object[]
				{
					ConstValue.RECOVER_STAMINA_DIGISTONE_NUM,
					stamina,
					stamina + num,
					point
				});
				cmd_ChangePOP_STONE.OnPushedYesAction = new Action(this.OnSelectedRecover);
				cmd_ChangePOP_STONE.Info = info;
				cmd_ChangePOP_STONE.SetDigistone(point, ConstValue.RECOVER_STAMINA_DIGISTONE_NUM);
				cmd_ChangePOP_STONE.BtnTextYes = StringMaster.GetString("StaminaRecoveryExecution");
				cmd_ChangePOP_STONE.BtnTextNo = StringMaster.GetString("SystemButtonClose");
			}
			else
			{
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseConfirmShop), "CMD_Confirm") as CMD_Confirm;
				cmd_Confirm.Title = StringMaster.GetString("StaminaShortageTitle");
				string info2 = string.Format(StringMaster.GetString("StaminaShortageGoShop"), ConstValue.RECOVER_STAMINA_DIGISTONE_NUM);
				cmd_Confirm.Info = info2;
				cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonGoShop");
				cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
			}
		}
		else
		{
			result = true;
		}
		return result;
	}

	private bool CanPlayDungeonOver()
	{
		if (Singleton<UserDataMng>.Instance.IsOverUnitLimit(ClassSingleton<MonsterUserDataMng>.Instance.GetMonsterNum() + ConstValue.ENABLE_MONSTER_SPACE_TOEXEC_DUNGEON))
		{
			CMD_UpperLimit cmd_UpperLimit = GUIMain.ShowCommonDialog(null, "CMD_Upperlimit") as CMD_UpperLimit;
			cmd_UpperLimit.SetType(CMD_UpperLimit.MessageType.QUEST);
			return false;
		}
		if (!Singleton<UserDataMng>.Instance.IsOverChipLimit(ConstValue.ENABLE_CHIP_SPACE_TOEXEC_DUNGEON))
		{
			return true;
		}
		CMD_UpperlimitChip cmd_UpperlimitChip = GUIMain.ShowCommonDialog(null, "CMD_UpperlimitChip") as CMD_UpperlimitChip;
		cmd_UpperlimitChip.SetType(CMD_UpperlimitChip.MessageType.QUEST);
		return false;
	}

	private void OnCloseConfirmShop(int idx)
	{
		int hadStone = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
		if (idx == 0)
		{
			GUIMain.ShowCommonDialog(delegate(int indexe)
			{
				if (hadStone < DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point && this.GetActionStatus() == CommonDialog.ACT_STATUS.OPEN)
				{
					this.OnClickedDungeon(this.StageDataBk, this.StageNumBk, null);
				}
			}, "CMD_Shop");
		}
	}

	private void OnSelectedRecover()
	{
		base.StartCoroutine(this.RequestRecoverLife(delegate
		{
			GUIPlayerStatus.RefreshParams_S(true);
			this.userStamina.RefreshParams();
			CMD_ChangePOP_STONE cmd_ChangePOP_STONE = UnityEngine.Object.FindObjectOfType<CMD_ChangePOP_STONE>();
			if (null != cmd_ChangePOP_STONE)
			{
				cmd_ChangePOP_STONE.SetCloseAction(delegate(int i)
				{
					CMD_ModalMessage.Create(StringMaster.GetString("StaminaRecoveryTitle"), StringMaster.GetString("StaminaRecoveryCompleted"), delegate(int index)
					{
						this.OnClickedDungeon(this.StageDataBk, this.StageNumBk, null);
					});
				});
				cmd_ChangePOP_STONE.ClosePanel(true);
			}
			else
			{
				CMD_ModalMessage.Create(StringMaster.GetString("StaminaRecoveryTitle"), StringMaster.GetString("StaminaRecoveryCompleted"), delegate(int index)
				{
					this.OnClickedDungeon(this.StageDataBk, this.StageNumBk, null);
				});
			}
		}));
	}

	private IEnumerator RequestRecoverLife(Action onSuccessed)
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

	private void OnCloseQuestDetailedPOP(int idx)
	{
		if (!this.CanPlayDungeonOver())
		{
			return;
		}
		if (idx == 0 || idx == 10 || idx == 20)
		{
			if (!ClassSingleton<PlayLimit>.Instance.PlayLimitCheck(this.StageDataBk.dungeon, delegate(int _idx)
			{
				if (this.StageDataBk.dungeon.playLimit.recoveryAssetCategoryId == 2)
				{
					this.OnCloseConfirmShop(_idx);
				}
				else if (this.StageDataBk.dungeon.playLimit.recoveryAssetCategoryId == 6)
				{
					GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(this.StageDataBk.dungeon.playLimit.recoveryAssetValue.ToString());
					CMD_ModalMessage cmd_ModalMessage2 = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
					cmd_ModalMessage2.Title = string.Format(StringMaster.GetString("SystemShortage"), itemM.name);
					cmd_ModalMessage2.Info = string.Format(StringMaster.GetString("QuestPlayLimitItemShortInfo"), itemM.name);
				}
			}, delegate(int _idx_)
			{
				ClassSingleton<PlayLimit>.Instance.RecoverPlayLimit(this.StageDataBk.dungeon, new Action<GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons>(this.OnSuccessedRecoverPlayLimit));
			}, ConstValue.PLAYLIMIT_USE_COUNT))
			{
				return;
			}
			if (CMD_QuestTOP.AreaData.data.worldAreaId == "8")
			{
				ClassSingleton<PlayLimit>.Instance.SetTicketNumCont(this.StageDataBk.dungeon, ConstValue.PLAYLIMIT_USE_COUNT);
			}
		}
		if (idx != 0)
		{
			if (idx != 10)
			{
				if (idx == 20)
				{
					if (!this.CanPlayDungeonStamina())
					{
						return;
					}
					this.OnTapMultiRecruitButton();
				}
			}
			else
			{
				this.OnTapMultiParticipateButton();
			}
		}
		else
		{
			if (!this.CanPlayDungeonStamina())
			{
				return;
			}
			if (this.needLife <= DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.stamina)
			{
				ClassSingleton<QuestData>.Instance.SelectDungeon = this.StageDataBk.worldDungeonM;
				CMD_PartyEdit.ModeType = CMD_PartyEdit.MODE_TYPE.SELECT;
				CMD_PartyEdit cmd_PartyEdit = GUIMain.ShowCommonDialog(new Action<int>(this.OnClosePartySelect), "CMD_PartyEdit") as CMD_PartyEdit;
				cmd_PartyEdit.parentCMD = this;
			}
			else
			{
				CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
				cmd_ModalMessage.Title = StringMaster.GetString("StaminaShortageTitle");
				cmd_ModalMessage.Info = StringMaster.GetString("QuestStaminaShortage");
			}
		}
	}

	private void OnCloseQuestMultiDetailedPOP(int selectButtonIndex)
	{
		if (!this.CanPlayDungeonOver())
		{
			return;
		}
		if (selectButtonIndex == 10 || selectButtonIndex == 20)
		{
			if (!ClassSingleton<PlayLimit>.Instance.PlayLimitCheck(this.StageDataBk.dungeon, delegate(int _idx)
			{
				if (this.StageDataBk.dungeon.playLimit.recoveryAssetCategoryId == 2)
				{
					this.OnCloseConfirmShop(_idx);
				}
				else if (this.StageDataBk.dungeon.playLimit.recoveryAssetCategoryId == 6)
				{
					GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(this.StageDataBk.dungeon.playLimit.recoveryAssetValue.ToString());
					CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
					cmd_ModalMessage.Title = string.Format(StringMaster.GetString("SystemShortage"), itemM.name);
					cmd_ModalMessage.Info = string.Format(StringMaster.GetString("QuestPlayLimitItemShortInfo"), itemM.name);
				}
			}, delegate(int _idx_)
			{
				ClassSingleton<PlayLimit>.Instance.RecoverPlayLimit(this.StageDataBk.dungeon, new Action<GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons>(this.OnSuccessedRecoverPlayLimit));
			}, ConstValue.PLAYLIMIT_USE_COUNT))
			{
				return;
			}
			if (CMD_QuestTOP.AreaData.data.worldAreaId == "8")
			{
				ClassSingleton<PlayLimit>.Instance.SetTicketNumCont(this.StageDataBk.dungeon, ConstValue.PLAYLIMIT_USE_COUNT);
			}
		}
		if (selectButtonIndex != 10)
		{
			if (selectButtonIndex == 20)
			{
				if (!this.CanPlayDungeonStamina())
				{
					return;
				}
				this.OnTapMultiRecruitButton();
			}
		}
		else
		{
			this.OnTapMultiParticipateButton();
		}
	}

	private void OnSuccessedRecoverPlayLimit(GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dng)
	{
		List<GUIListPartBS> partObjs = this.csSelectPanelS_DungeonR.partObjs;
		for (int i = 0; i < partObjs.Count; i++)
		{
			GUIListPartsS_DungeonR guilistPartsS_DungeonR = (GUIListPartsS_DungeonR)partObjs[i];
			if (guilistPartsS_DungeonR.Data.dungeon.worldDungeonId == dng.worldDungeonId)
			{
				guilistPartsS_DungeonR.RefreshShowPlayLimit();
				break;
			}
		}
		GUIPlayerStatus.RefreshParams_S(true);
	}

	private void OnTapMultiRecruitButton()
	{
		if (this.needLife <= DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.stamina)
		{
			ClassSingleton<QuestData>.Instance.SelectDungeon = this.StageDataBk.worldDungeonM;
			DataMng.Instance().GetResultUtilData().SetLastDngReq(this.StageDataBk.worldDungeonM.worldDungeonId, "-1", this.StageDataBk.dungeon.userDungeonTicketId);
			CMD_PartyEdit.ModeType = CMD_PartyEdit.MODE_TYPE.MULTI;
			GUIMain.ShowCommonDialog(new Action<int>(this.OnClosePartySelect), "CMD_PartyEdit");
		}
		else
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("StaminaShortageTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("QuestStaminaShortage");
		}
	}

	private void OnTapMultiParticipateButton()
	{
		CMD_MultiRecruitTop.CreateTargetStage(this.StageDataBk.worldDungeonM.worldDungeonId);
	}

	private void OnClosePartySelect(int i)
	{
		this.OnCloseQuestTOP(ConstValue.NO_MEAN_NUM);
	}

	private void OnCloseQuestTOP(int i)
	{
		ClassSingleton<QuestData>.Instance.RespDataWD_DungeonResult = null;
		ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic = null;
		if (0 >= this.battlePartyDeckNo)
		{
			if (GameObject.FindGameObjectWithTag("FarmRoot") == null)
			{
				this.DeleteLoading();
			}
		}
		else
		{
			GameWebAPI.WD_Req_DngStart sendData = new GameWebAPI.WD_Req_DngStart
			{
				dungeonId = this.StageDataBk.worldDungeonM.worldDungeonId,
				deckNum = this.battlePartyDeckNo.ToString(),
				userDungeonTicketId = this.StageDataBk.dungeon.userDungeonTicketId
			};
			GameWebAPI.RequestWD_WorldStart requestWD_WorldStart = new GameWebAPI.RequestWD_WorldStart();
			requestWD_WorldStart.SetSendData = delegate(GameWebAPI.WD_Req_DngStart param)
			{
				param.dungeonId = sendData.dungeonId;
				param.deckNum = sendData.deckNum;
				param.userDungeonTicketId = sendData.userDungeonTicketId;
			};
			requestWD_WorldStart.OnReceived = delegate(GameWebAPI.RespDataWD_DungeonStart response)
			{
				ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart = response;
			};
			GameWebAPI.RequestWD_WorldStart request = requestWD_WorldStart;
			AppCoroutine.Start(request.RunOneTime(delegate()
			{
				if (null != DataMng.Instance() && DataMng.Instance().WD_ReqDngResult != null)
				{
					DataMng.Instance().WD_ReqDngResult.dungeonId = sendData.dungeonId;
				}
				DataMng.Instance().GetResultUtilData().last_dng_req = sendData;
				this.StartCoroutine(this.EndDungeonStartWait());
			}, delegate(Exception noop)
			{
				this.DeleteLoading();
				FarmCameraControlForCMD.On();
				GUIManager.CloseAllCommonDialog(null);
			}, null), false);
		}
	}

	private void DeleteLoading()
	{
		if (TipsLoading.Instance.IsShow)
		{
			TipsLoading.Instance.StopTipsLoad(true);
		}
		RestrictionInput.EndLoad();
	}

	private IEnumerator EndDungeonStartWait()
	{
		while (!AssetDataCacheMng.Instance().IsCacheAllReadyType(AssetDataCacheMng.CACHE_TYPE.BATTLE_COMMON))
		{
			yield return null;
		}
		RestrictionInput.EndLoad();
		Singleton<UserDataMng>.Instance.ConsumeUserStamina(this.needLife);
		this.isGoingBattle = true;
		DataMng.Instance().WD_ReqDngResult.dungeonId = this.StageDataBk.worldDungeonM.worldDungeonId;
		DataMng.Instance().WD_ReqDngResult.clear = 0;
		base.SetLastCallBack(new Action<int>(this.GoToBattleScene));
		this.ClosePanel(true);
		yield break;
	}

	private void GoToBattleScene(int noop)
	{
		BattleStateManager.StartSingle(0.5f, 0.5f, true, null);
	}

	private void InitQuest()
	{
		this.worldStageData = ClassSingleton<QuestData>.Instance.GetWorldStageData_ByAreaID(CMD_QuestTOP.AreaData.data.worldAreaId, false);
		if (ClassSingleton<QuestTOPAccessor>.Instance == null || !ClassSingleton<QuestTOPAccessor>.Instance.nextAreaFlg)
		{
			this.currentSelected = 0;
		}
		else
		{
			this.currentSelected = this.worldStageData.Count - int.Parse(ClassSingleton<QuestTOPAccessor>.Instance.nextStage.worldStageId);
		}
		this.InitA_StageList();
		this.ChangeSelectA_StageL(this.currentSelected, true);
	}

	public bool IsSpecialDungeon()
	{
		return CMD_QuestTOP.AreaData.data.worldAreaId != "1";
	}
}
