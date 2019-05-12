using Master;
using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIListPartsS_DungeonR : GUIListPartBS
{
	[SerializeField]
	[Header("ドロップの画像")]
	private List<PresentBoxItem> itemDROP_ITEM_LIST;

	[SerializeField]
	[Header("ステージ数用のラベル")]
	private UILabel ngTXT_STAGE;

	[SerializeField]
	[Header("ステージ名用のラベル")]
	private UILabel ngTXT_STAGE_NAME;

	[SerializeField]
	[Header("消費スタミナ用のラベル")]
	private UILabel ngTXT_STAMINA;

	[SerializeField]
	[Header("残回数 ラベル")]
	public UILabel ngTXT_TICKET_LEFT;

	[SerializeField]
	[Header("ノーコン ラベル")]
	public UILabel ngTXT_NO_CONTINUE;

	[SerializeField]
	[Header("ソロとマルチができるステージ色")]
	private Color colorNormalStage;

	[SerializeField]
	[Header("マルチ専用のステージ色")]
	private Color colorMultiStage;

	[SerializeField]
	[Header("ソロ専用のステージ色")]
	private Color colorSoloStage;

	[SerializeField]
	[Header("イベント用のステージ背景色")]
	private Color colorEventStageBackground;

	[SerializeField]
	[Header("ソロとマルチステージ名装飾色")]
	private Color colorSoloMultiStageNameOutline;

	[SerializeField]
	[Header("マルチ専用名装飾色")]
	private Color colorMultiStageNameOutline;

	[SerializeField]
	[Header("ソロ専用ステージ名装飾色")]
	private Color colorSoloStageNameOutline;

	[SerializeField]
	[Header("イベント用のステージ名装飾色")]
	private Color colorEventStageNameOutline;

	[SerializeField]
	[Header("背景色のパーツ（板）")]
	private UISprite backgroundBord;

	[SerializeField]
	[Header("背景色のパーツ（ライン）")]
	private UITexture backgroundLine;

	[SerializeField]
	[Header("残回数無しラベル")]
	private UILabel ngTXT_PLAY_LIMIT;

	[SerializeField]
	[Header("回数限定用グレーアウトSPR素材")]
	private UISprite spGRAYOUT_PLAY_LIMIT;

	[Header("NEWとCLEARのアイコン")]
	[SerializeField]
	private UISprite ngSPR_NEW;

	[Header("指定クエストクリア管理フラグ 閉じている時のカギ")]
	[SerializeField]
	private UISprite ngSPR_LOCK;

	[Header("クリアのマークの画像")]
	[SerializeField]
	private string clearMark = "Common02_text_Clear";

	[Header("ステージギミック表記Obj")]
	[SerializeField]
	private GameObject stageGimmickObj;

	[SerializeField]
	private float gimmickFadeTime = 1f;

	[SerializeField]
	private float gimmickViewTime;

	[SerializeField]
	private UILabel stageStateLabel;

	[SerializeField]
	private UiLabelToggle labelToggle;

	private List<Color> stateLabelColor = new List<Color>();

	private List<Color> stateEffectColor = new List<Color>();

	[SerializeField]
	private bool useLongDescription;

	private List<string> campaignTextList = new List<string>();

	[SerializeField]
	private GameWebAPI.RespDataCP_Campaign.CampaignType[] refCampaignType;

	[SerializeField]
	private CampaignLabelQuest campagin;

	private bool isTouchEndFromChild;

	private GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo;

	private bool isShowedDropItems;

	private List<QuestData.DSCondition_AND_Data> dngStartCondition_OR_DataList;

	public string StageNum { private get; set; }

	public QuestData.WorldDungeonData WorldDungeonData { get; set; }

	public bool IsEventStage { private get; set; }

	protected override void Awake()
	{
		base.Awake();
		this.campagin.Initialize(new Action<string, string>(this.SetExtraEffectDescription));
	}

	protected override void OnDestroy()
	{
		this.campagin.Destroy();
		base.OnDestroy();
	}

	public void ChangeSprite(string sprName)
	{
		UISprite component = base.gameObject.GetComponent<UISprite>();
		if (component != null)
		{
			component.spriteName = sprName;
			component.MakePixelPerfect();
		}
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		switch (this.WorldDungeonData.status)
		{
		case 1:
			this.ngSPR_NEW.gameObject.SetActive(false);
			break;
		case 2:
			this.ngSPR_NEW.MakePixelPerfect();
			break;
		case 3:
			this.ngSPR_NEW.gameObject.SetActive(false);
			break;
		case 4:
			this.SetClearIcon();
			break;
		}
		if (this.ngTXT_STAGE != null)
		{
			if (ClassSingleton<QuestData>.Instance.ExistSortieLimit(this.WorldDungeonData.dungeon.worldDungeonId))
			{
				this.ngTXT_STAGE.text = StringMaster.GetString("QuestLimited");
			}
			else if (CMD_QuestTOP.instance.IsSpecialDungeon())
			{
				this.ngTXT_STAGE.text = StringMaster.GetString("QuestSpecial");
			}
			else
			{
				this.ngTXT_STAGE.text = string.Format(StringMaster.GetString("QuestStage"), this.StageNum.ToInt32());
			}
		}
		if (this.ngTXT_STAGE_NAME != null)
		{
			this.ngTXT_STAGE_NAME.text = this.WorldDungeonData.worldDungeonM.name;
		}
		if (!this.IsEventStage)
		{
			string worldStageId = this.WorldDungeonData.worldDungeonM.worldStageId;
			GameWebAPI.RespDataCP_Campaign respDataCP_Campaign = DataMng.Instance().RespDataCP_Campaign;
			this.campaignInfo = respDataCP_Campaign.GetCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDown, worldStageId);
			if (this.ngTXT_STAMINA != null)
			{
				int num = int.Parse(this.WorldDungeonData.worldDungeonM.needStamina);
				if (this.campaignInfo != null)
				{
					float num2 = (float)num;
					num = Mathf.CeilToInt(num2 * float.Parse(this.campaignInfo.rate));
				}
				this.ngTXT_STAMINA.text = string.Format(StringMaster.GetString("QuestStaminaCost"), num);
			}
			if (null != this.backgroundBord && null != this.backgroundLine)
			{
				if (this.WorldDungeonData.worldDungeonM.IsMultiOnly())
				{
					this.colorMultiStage.a = this.backgroundBord.color.a;
					this.backgroundBord.color = this.colorMultiStage;
					this.colorMultiStage.a = this.backgroundLine.color.a;
					this.backgroundLine.color = this.colorMultiStage;
					this.colorMultiStageNameOutline.a = this.ngTXT_STAGE_NAME.effectColor.a;
					this.ngTXT_STAGE_NAME.effectColor = this.colorMultiStageNameOutline;
					this.ngTXT_STAMINA.effectColor = this.colorMultiStageNameOutline;
				}
				else if (this.WorldDungeonData.worldDungeonM.IsSoloOnly())
				{
					this.colorSoloStage.a = this.backgroundBord.color.a;
					this.backgroundBord.color = this.colorSoloStage;
					this.colorSoloStage.a = this.backgroundLine.color.a;
					this.backgroundLine.color = this.colorSoloStage;
					this.colorSoloStageNameOutline.a = this.ngTXT_STAGE_NAME.effectColor.a;
					this.ngTXT_STAGE_NAME.effectColor = this.colorSoloStageNameOutline;
					this.ngTXT_STAMINA.effectColor = this.colorSoloStageNameOutline;
				}
				else
				{
					this.colorNormalStage.a = this.backgroundBord.color.a;
					this.backgroundBord.color = this.colorNormalStage;
					this.colorNormalStage.a = this.backgroundLine.color.a;
					this.backgroundLine.color = this.colorNormalStage;
					this.colorSoloMultiStageNameOutline.a = this.ngTXT_STAGE_NAME.effectColor.a;
					this.ngTXT_STAGE_NAME.effectColor = this.colorSoloMultiStageNameOutline;
					this.ngTXT_STAMINA.effectColor = this.colorSoloMultiStageNameOutline;
				}
			}
			this.campagin.AreaId = worldStageId;
			string text = this.campagin.GetText(DataMng.Instance().RespDataCP_Campaign, DataMng.Instance().CampaignForceHide);
			this.SetExtraEffectDescription(worldStageId, text);
		}
		else
		{
			this.SetEventStageParts();
		}
		if (null != CMD_QuestTOP.instance)
		{
			QuestData.WorldStageData worldStageData = CMD_QuestTOP.instance.GetWorldStageData();
			if ("8" == worldStageData.worldStageM.worldAreaId)
			{
				this.ngTXT_TICKET_LEFT.gameObject.SetActive(true);
				if (!string.IsNullOrEmpty(this.WorldDungeonData.dungeon.dungeonTicketNum))
				{
					this.ngTXT_TICKET_LEFT.text = string.Format(StringMaster.GetString("TicketQuestLeftNum"), int.Parse(this.WorldDungeonData.dungeon.dungeonTicketNum));
				}
				else
				{
					this.ngTXT_TICKET_LEFT.text = string.Format(StringMaster.GetString("TicketQuestLeftNum"), 0);
				}
			}
			else
			{
				this.RefreshShowPlayLimit();
			}
		}
		this.ShowNoContinue();
		this.ShowLockStatus();
	}

	private void SetExtraEffectDescription(string worldStageId, string campaignDescript)
	{
		this.campaignTextList.Clear();
		if (DataMng.Instance().StageGimmick.DataDic.ContainsKey(worldStageId))
		{
			Dictionary<string, List<string>> dictionary = DataMng.Instance().StageGimmick.DataDic[worldStageId];
			bool flag = dictionary.ContainsKey(this.WorldDungeonData.worldDungeonM.worldDungeonId);
			if (flag)
			{
				this.campaignTextList.Add(StringMaster.GetString("QuestGimmick"));
				this.stateLabelColor.Add(new Color(1f, 0.94f, 0f));
				this.stateEffectColor.Add(new Color(0f, 0.51f, 0f));
			}
		}
		if (!string.IsNullOrEmpty(campaignDescript))
		{
			this.campaignTextList.Add(campaignDescript);
			this.stateLabelColor.Add(new Color(0.78f, 0f, 0f));
			this.stateEffectColor.Add(new Color(1f, 0.94f, 0f));
		}
		this.stageGimmickObj.SetActive(this.campaignTextList.Count > 0);
		if (this.stageGimmickObj.activeSelf && this.labelToggle != null)
		{
			if (this.campaignTextList.Count > 1)
			{
				this.labelToggle.InitToggleData(this.stageStateLabel, this.campaignTextList, this.stateLabelColor, this.stateEffectColor, this.gimmickFadeTime, this.gimmickViewTime, true);
			}
			else if (this.campaignTextList.Count == 1)
			{
				this.stageStateLabel.text = this.campaignTextList[0];
				this.stageStateLabel.color = this.stateLabelColor[0];
				this.stageStateLabel.effectColor = this.stateEffectColor[0];
			}
		}
	}

	public void RefreshShowPlayLimit()
	{
		if (this.WorldDungeonData.dungeon.playLimit != null)
		{
			this.ngTXT_PLAY_LIMIT.gameObject.SetActive(true);
			GameWebAPI.RespDataWD_GetDungeonInfo.PlayLimit playLimit = this.WorldDungeonData.dungeon.playLimit;
			int num = int.Parse(playLimit.restCount);
			this.ngTXT_PLAY_LIMIT.gameObject.SetActive(false);
			this.ngTXT_TICKET_LEFT.gameObject.SetActive(false);
			this.spGRAYOUT_PLAY_LIMIT.gameObject.SetActive(false);
			string text = string.Empty;
			if (playLimit.limitType == "1")
			{
				text = StringMaster.GetString("QuestPlayLimitBattle");
			}
			else if (playLimit.limitType == "2")
			{
				text = StringMaster.GetString("QuestPlayLimitClear");
			}
			if (playLimit.recoveryFlg == "1")
			{
				if (playLimit.dailyResetFlg == "1")
				{
					text += string.Format(StringMaster.GetString("PlayLimitQuestRestCT_4"), num);
				}
				else
				{
					text += string.Format(StringMaster.GetString("PlayLimitQuestRestCT_3"), num);
				}
			}
			else
			{
				if (0 >= num)
				{
					text = string.Empty;
					this.ngTXT_PLAY_LIMIT.gameObject.SetActive(true);
					this.ngTXT_PLAY_LIMIT.text = StringMaster.GetString("PlayLimitQuestRestNone");
					this.spGRAYOUT_PLAY_LIMIT.gameObject.SetActive(true);
					GUICollider component = base.gameObject.GetComponent<GUICollider>();
					if (null != component)
					{
						component.activeCollider = false;
					}
					this.ngTXT_TICKET_LEFT.text = text;
					return;
				}
				text += string.Format(StringMaster.GetString("PlayLimitQuestRestCT_3"), num);
			}
			this.ngTXT_TICKET_LEFT.gameObject.SetActive(true);
			this.ngTXT_TICKET_LEFT.text = text;
		}
		else
		{
			this.ngTXT_PLAY_LIMIT.gameObject.SetActive(false);
			this.ngTXT_TICKET_LEFT.text = string.Empty;
			this.ngTXT_TICKET_LEFT.gameObject.SetActive(false);
			this.spGRAYOUT_PLAY_LIMIT.gameObject.SetActive(false);
		}
	}

	private void ShowNoContinue()
	{
		if (null != this.ngTXT_NO_CONTINUE)
		{
			if (this.WorldDungeonData.worldDungeonM.canContinue == "1")
			{
				this.ngTXT_NO_CONTINUE.text = string.Empty;
			}
			else
			{
				this.ngTXT_NO_CONTINUE.text = StringMaster.GetString("QuestNoContinue");
			}
		}
	}

	private void ShowLockStatus()
	{
		if (this.WorldDungeonData.status == 1)
		{
			this.dngStartCondition_OR_DataList = ClassSingleton<QuestData>.Instance.GetDngStartCondition_OR_DataList(this.WorldDungeonData.wdscMList);
			if (this.dngStartCondition_OR_DataList.Count <= 0)
			{
				this.ngSPR_LOCK.gameObject.SetActive(false);
			}
			else
			{
				this.ngSPR_LOCK.gameObject.SetActive(true);
			}
		}
		else
		{
			this.ngSPR_LOCK.gameObject.SetActive(false);
		}
	}

	private string MakeStringLockStatus()
	{
		string text = string.Empty;
		if (this.dngStartCondition_OR_DataList.Count <= 0)
		{
			return null;
		}
		for (int i = 0; i < this.dngStartCondition_OR_DataList.Count; i++)
		{
			List<QuestData.DSConditionData> dscondition_AND_List = this.dngStartCondition_OR_DataList[i].DSCondition_AND_List;
			for (int j = 0; j < dscondition_AND_List.Count; j++)
			{
				string str;
				if (dscondition_AND_List[j].clearCT <= 1)
				{
					if (dscondition_AND_List[j].dngM != null)
					{
						str = string.Format(StringMaster.GetString("QuestClearConditionOne"), dscondition_AND_List[j].dngM.name);
					}
					else
					{
						str = "ERROR クリアダンジョンID: " + dscondition_AND_List[j].preDngID;
					}
				}
				else if (dscondition_AND_List[j].dngM != null)
				{
					str = string.Format(StringMaster.GetString("QuestClearConditionTwo"), dscondition_AND_List[j].dngM.name, dscondition_AND_List[j].clearCT);
				}
				else
				{
					str = "ERROR クリアダンジョンID: " + dscondition_AND_List[j].preDngID;
				}
				text += str;
				if (dscondition_AND_List.Count > 1 && 0 <= j && j < dscondition_AND_List.Count - 1)
				{
					text += StringMaster.GetString("QuestClearConditionAND_2");
				}
			}
			if (this.dngStartCondition_OR_DataList.Count > 1 && 0 <= i && i < this.dngStartCondition_OR_DataList.Count - 1)
			{
				text += StringMaster.GetString("QuestClearConditionOR");
			}
		}
		return text;
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchBegan(touch, pos);
		this.isTouchEndFromChild = false;
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchEnded(touch, pos, flag);
		if (flag)
		{
			float magnitude = (this.beganPostion - pos).magnitude;
			if (magnitude < 40f && !this.isTouchEndFromChild)
			{
				if (this.WorldDungeonData.status == 1)
				{
					string text = this.MakeStringLockStatus();
					if (!string.IsNullOrEmpty(text))
					{
						CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage", null) as CMD_ModalMessage;
						cmd_ModalMessage.Title = StringMaster.GetString("QuestClearConditionTitle");
						cmd_ModalMessage.InfoWithNoReturn = text;
					}
				}
				else if (!this.IsEventStage)
				{
					if (CMD_QuestTOP.instance != null)
					{
						CMD_QuestTOP.instance.OnClickedDungeon(this.WorldDungeonData, this.ngTXT_STAGE.text, this.campaignInfo);
					}
				}
				else
				{
					this.OnSelectEventStage();
				}
			}
		}
	}

	private void OnClickedBtnSelect()
	{
	}

	protected override void Update()
	{
		base.Update();
		if (this.WorldDungeonData == null)
		{
			return;
		}
		if (!this.isShowedDropItems)
		{
			this.ShowDropItems();
			this.isShowedDropItems = true;
		}
	}

	private void SetEventStageParts()
	{
		this.campaignInfo = null;
		this.stageGimmickObj.SetActive(false);
		this.ngTXT_STAMINA.gameObject.SetActive(false);
		this.colorEventStageBackground.a = this.backgroundBord.color.a;
		this.backgroundBord.color = this.colorEventStageBackground;
		this.colorEventStageBackground.a = this.backgroundLine.color.a;
		this.backgroundLine.color = this.colorEventStageBackground;
		this.colorEventStageNameOutline.a = this.ngTXT_STAGE_NAME.effectColor.a;
		this.ngTXT_STAGE_NAME.effectColor = this.colorEventStageNameOutline;
		this.ngTXT_STAMINA.effectColor = this.colorEventStageNameOutline;
	}

	private void OnSelectEventStage()
	{
		if (this.WorldDungeonData.status == 4)
		{
			QuestStart.OpenConfirmReplayEvent(new Action<int>(this.OnClosedEventConfirm));
		}
		else
		{
			this.StartEventStage();
		}
	}

	private void OnClosedEventConfirm(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			this.StartEventStage();
		}
	}

	private void StartEventStage()
	{
		GameWebAPI.WD_Req_DngStart startInfo = new GameWebAPI.WD_Req_DngStart
		{
			dungeonId = this.WorldDungeonData.worldDungeonM.worldDungeonId,
			deckNum = DataMng.Instance().RespDataMN_DeckList.selectDeckNum,
			userDungeonTicketId = this.WorldDungeonData.dungeon.userDungeonTicketId
		};
		QuestStart.StartEventStage(startInfo);
	}

	private void ShowDropItems()
	{
		GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dungeon = this.WorldDungeonData.dungeon;
		List<GameWebAPI.RespDataWD_GetDungeonInfo.DropAsset> list = new List<GameWebAPI.RespDataWD_GetDungeonInfo.DropAsset>();
		QuestData.CreateDropAssetList(dungeon, list);
		for (int i = 0; i < this.itemDROP_ITEM_LIST.Count; i++)
		{
			if (i < list.Count)
			{
				string assetCategoryId = list[i].assetCategoryId.ToString();
				string objectId = list[i].assetValue.ToString();
				this.itemDROP_ITEM_LIST[i].SetItem(assetCategoryId, objectId, "1", true, new Action(this.ScaleEnd));
			}
			else
			{
				this.itemDROP_ITEM_LIST[i].enabled = false;
			}
		}
	}

	private void ScaleEnd()
	{
	}

	private bool ExistCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType type)
	{
		for (int i = 0; i < this.refCampaignType.Length; i++)
		{
			if (this.refCampaignType[i] == type)
			{
				return true;
			}
		}
		return false;
	}

	private void SetClearIcon()
	{
		this.ngSPR_NEW.spriteName = this.clearMark;
		this.ngSPR_NEW.MakePixelPerfect();
		UITweener component = this.ngSPR_NEW.GetComponent<UITweener>();
		if (null != component)
		{
			component.enabled = false;
		}
		this.ngSPR_NEW.alpha = 1f;
	}
}
