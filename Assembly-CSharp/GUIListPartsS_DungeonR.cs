using Master;
using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIListPartsS_DungeonR : GUIListPartBS
{
	[SerializeField]
	[Header("NEWの画像")]
	private GameObject goNEW;

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

	[Header("ソロ専用のステージ色")]
	[SerializeField]
	private Color colorSoloStage;

	[Header("イベント用のステージ背景色")]
	[SerializeField]
	private Color colorEventStageBackground;

	[SerializeField]
	[Header("####ソロとマルチステージ名装飾色")]
	private Color colorSoloMultiStageNameOutline;

	[Header("####マルチ専用名装飾色")]
	[SerializeField]
	private Color colorMultiStageNameOutline;

	[SerializeField]
	[Header("####ソロ専用ステージ名装飾色")]
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

	private bool isTouchEndFromChild;

	private GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo;

	private bool isShowedDropItems;

	private List<QuestData.DSCondition_AND_Data> dngStartCondition_OR_DataList;

	public string StageNum { private get; set; }

	public QuestData.WorldDungeonData Data { get; set; }

	public bool IsEventStage { private get; set; }

	protected override void Awake()
	{
		base.Awake();
		this.ngSPR_NEW = this.goNEW.GetComponent<UISprite>();
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
		switch (this.Data.status)
		{
		case 1:
			this.goNEW.SetActive(false);
			break;
		case 2:
			this.ngSPR_NEW.MakePixelPerfect();
			break;
		case 3:
			this.goNEW.SetActive(false);
			break;
		case 4:
			this.SetClearIcon();
			break;
		}
		if (this.ngTXT_STAGE != null)
		{
			if (ClassSingleton<QuestData>.Instance.ExistSortieLimit(this.Data.dungeon.worldDungeonId))
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
			this.ngTXT_STAGE_NAME.text = this.Data.worldDungeonM.name;
		}
		if (!this.IsEventStage)
		{
			GameWebAPI.RespDataCP_Campaign respDataCP_Campaign = DataMng.Instance().RespDataCP_Campaign;
			this.campaignInfo = respDataCP_Campaign.GetCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDown, this.Data.worldDungeonM.worldStageId);
			if (this.ngTXT_STAMINA != null)
			{
				int num = int.Parse(this.Data.worldDungeonM.needStamina);
				if (this.campaignInfo != null)
				{
					float num2 = (float)num;
					num = Mathf.CeilToInt(num2 * float.Parse(this.campaignInfo.rate));
				}
				this.ngTXT_STAMINA.text = string.Format(StringMaster.GetString("QuestStaminaCost"), num);
			}
			if (null != this.backgroundBord && null != this.backgroundLine)
			{
				if (this.Data.worldDungeonM.IsMultiOnly())
				{
					this.colorMultiStage.a = this.backgroundBord.color.a;
					this.backgroundBord.color = this.colorMultiStage;
					this.colorMultiStage.a = this.backgroundLine.color.a;
					this.backgroundLine.color = this.colorMultiStage;
					this.colorMultiStageNameOutline.a = this.ngTXT_STAGE_NAME.effectColor.a;
					this.ngTXT_STAGE_NAME.effectColor = this.colorMultiStageNameOutline;
					this.ngTXT_STAMINA.effectColor = this.colorMultiStageNameOutline;
				}
				else if (this.Data.worldDungeonM.IsSoloOnly())
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
			if (DataMng.Instance().StageGimmick.DataDic.ContainsKey(this.Data.worldDungeonM.worldStageId))
			{
				bool flag = DataMng.Instance().StageGimmick.DataDic[this.Data.worldDungeonM.worldStageId].ContainsKey(this.Data.worldDungeonM.worldDungeonId);
				if (flag)
				{
					this.stateLabelColor.Add(new Color(1f, 0.94f, 0f));
					this.stateEffectColor.Add(new Color(0f, 0.51f, 0f));
					this.campaignTextList.Add(StringMaster.GetString("QuestGimmick"));
				}
			}
			List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> list = new List<GameWebAPI.RespDataCP_Campaign.CampaignInfo>();
			DateTime now = ServerDateTime.Now;
			for (int i = 0; i < respDataCP_Campaign.campaignInfo.Length; i++)
			{
				if (this.ExistCampaign(respDataCP_Campaign.campaignInfo[i].GetCmpIdByEnum()) && respDataCP_Campaign.campaignInfo[i].targetValue == this.Data.worldDungeonM.worldStageId && respDataCP_Campaign.campaignInfo[i].IsUnderway(now))
				{
					list.Add(respDataCP_Campaign.campaignInfo[i]);
				}
			}
			if (list.Count > 0 && list[0] != null)
			{
				GameWebAPI.RespDataCP_Campaign.CampaignType cmpIdByEnum = list[0].GetCmpIdByEnum();
				float num3 = float.Parse(list[0].rate);
				if (cmpIdByEnum == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDown || cmpIdByEnum == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDownMul)
				{
					num3 = Mathf.Ceil(1f / num3);
				}
				string description = CampaignUtil.GetDescription(cmpIdByEnum, num3, this.useLongDescription);
				this.campaignTextList.Add(description);
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
		else
		{
			this.SetEventStageParts();
		}
		if (CMD_QuestTOP.instance != null)
		{
			QuestData.WorldStageData worldStageData = CMD_QuestTOP.instance.GetWorldStageData();
			if (worldStageData.worldStageM.worldAreaId == "8")
			{
				this.ngTXT_TICKET_LEFT.gameObject.SetActive(true);
				if (!string.IsNullOrEmpty(this.Data.dungeon.dungeonTicketNum))
				{
					this.ngTXT_TICKET_LEFT.text = string.Format(StringMaster.GetString("TicketQuestLeftNum"), int.Parse(this.Data.dungeon.dungeonTicketNum));
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

	public void RefreshShowPlayLimit()
	{
		if (this.Data.dungeon.playLimit != null)
		{
			this.ngTXT_PLAY_LIMIT.gameObject.SetActive(true);
			GameWebAPI.RespDataWD_GetDungeonInfo.PlayLimit playLimit = this.Data.dungeon.playLimit;
			int num = int.Parse(playLimit.restCount);
			int num2 = int.Parse(playLimit.maxCount);
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
					text += string.Format(StringMaster.GetString("PlayLimitQuestRestCT_1"), num, num2);
				}
				else
				{
					text += string.Format(StringMaster.GetString("PlayLimitQuestRestCT_2"), num, num2);
				}
			}
			else
			{
				if (num <= 0)
				{
					text = string.Empty;
					this.ngTXT_PLAY_LIMIT.gameObject.SetActive(true);
					this.ngTXT_PLAY_LIMIT.text = StringMaster.GetString("PlayLimitQuestRestNone");
					this.spGRAYOUT_PLAY_LIMIT.gameObject.SetActive(true);
					GUICollider component = base.gameObject.GetComponent<GUICollider>();
					if (component != null)
					{
						component.activeCollider = false;
					}
					if (playLimit.dailyResetFlg == "0")
					{
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
		if (this.ngTXT_NO_CONTINUE != null)
		{
			if (this.Data.worldDungeonM.canContinue == "1")
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
		if (this.Data.status == 1)
		{
			this.dngStartCondition_OR_DataList = ClassSingleton<QuestData>.Instance.GetDngStartCondition_OR_DataList(this.Data.wdscMList);
			if (this.dngStartCondition_OR_DataList.Count <= 0)
			{
				this.ngSPR_LOCK.gameObject.SetActive(false);
			}
			else if (this.dngStartCondition_OR_DataList.Count <= 1 && this.dngStartCondition_OR_DataList[0].DSCondition_AND_List.Count <= 1)
			{
				this.ngSPR_LOCK.gameObject.SetActive(true);
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
				if (this.Data.status == 1)
				{
					string text = this.MakeStringLockStatus();
					if (!string.IsNullOrEmpty(text))
					{
						CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
						cmd_ModalMessage.Title = StringMaster.GetString("QuestClearConditionTitle");
						cmd_ModalMessage.InfoWithNoReturn = text;
					}
				}
				else if (!this.IsEventStage)
				{
					if (CMD_QuestTOP.instance != null)
					{
						CMD_QuestTOP.instance.OnClickedDungeon(this.Data, this.ngTXT_STAGE.text, this.campaignInfo);
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
		if (this.Data == null)
		{
			return;
		}
		if (!this.isShowedDropItems)
		{
			this.ShowDropItems();
			this.isShowedDropItems = true;
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
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
		if (this.Data.status == 4)
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
			dungeonId = this.Data.worldDungeonM.worldDungeonId,
			deckNum = DataMng.Instance().RespDataMN_DeckList.selectDeckNum,
			userDungeonTicketId = this.Data.dungeon.userDungeonTicketId
		};
		QuestStart.StartEventStage(startInfo);
	}

	private void ShowDropItems()
	{
		GameWebAPI.RespDataWD_GetDungeonInfo.Dungeons dungeon = this.Data.dungeon;
		List<GameWebAPI.RespDataWD_GetDungeonInfo.DropAsset> list = new List<GameWebAPI.RespDataWD_GetDungeonInfo.DropAsset>();
		for (int i = 0; i < dungeon.dropAssets.Length; i++)
		{
			if (dungeon.dropAssets[i].assetCategoryId != 4 && dungeon.dropAssets[i].assetCategoryId != 5)
			{
				list.Add(dungeon.dropAssets[i]);
			}
		}
		for (int j = 0; j < this.itemDROP_ITEM_LIST.Count; j++)
		{
			if (j < list.Count)
			{
				string assetCategoryId = list[j].assetCategoryId.ToString();
				string objectId = list[j].assetValue.ToString();
				this.itemDROP_ITEM_LIST[j].SetItem(assetCategoryId, objectId, "1", true, new Action(this.ScaleEnd));
			}
			else
			{
				this.itemDROP_ITEM_LIST[j].enabled = false;
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
		this.ngSPR_NEW.GetComponent<UITweener>().enabled = false;
		this.ngSPR_NEW.alpha = 1f;
	}
}
