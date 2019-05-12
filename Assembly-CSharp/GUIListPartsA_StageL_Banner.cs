using Master;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GUIListPartsA_StageL_Banner : GUIListPartBS
{
	[Header("NEWのGameObject")]
	[SerializeField]
	private GameObject goNEW;

	[SerializeField]
	[Header("選択してないときの背景色")]
	private Color normalBGColor = new Color32(180, 0, 0, byte.MaxValue);

	[SerializeField]
	[Header("選択時の背景色")]
	private Color selectedBGColor = new Color32(180, 0, 0, byte.MaxValue);

	[Header("選択してないときの外枠色")]
	[SerializeField]
	private Color normalFrameColor = Color.white;

	[Header("選択時の外枠色")]
	[SerializeField]
	private Color selectedFrameColor = new Color32(150, 0, 0, byte.MaxValue);

	[SerializeField]
	[Header("残り時間のラベル")]
	private UILabel timeLabel;

	[Header("バナー読み込み失敗時のテキスト")]
	[SerializeField]
	private UILabel failedTextLabel;

	[SerializeField]
	[Header("背景のスプライト")]
	private UISprite bgSprite;

	[Header("外枠のスプライト")]
	[SerializeField]
	private UISprite frameSprite;

	[Header("バナーのテクスチャ")]
	[SerializeField]
	public UITexture bannerTex;

	[SerializeField]
	[Header("オープンの時の色")]
	private Color openBannerCol;

	[Header("クローズの時の色")]
	[SerializeField]
	private Color closeBannerCol;

	[SerializeField]
	[Header("オープンしてないを示す鍵")]
	private GameObject goCloseKey;

	[SerializeField]
	[Header("NEWとCLEARのアイコン")]
	private UISprite ngSPR_NEW;

	[SerializeField]
	[Header("クリアのマークの画像")]
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

	private QuestData.WorldStageData worldStageData;

	private bool isTouchEndFromChild;

	private DateTime restTimeDate;

	public GUISelectPanelA_StageL selectPanelA;

	private int totalSeconds;

	private Shader shader;

	public QuestData.WorldStageData WorldStageData
	{
		get
		{
			return this.worldStageData;
		}
		set
		{
			this.worldStageData = value;
			this.ShowGUI();
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.ngSPR_NEW = this.goNEW.GetComponent<UISprite>();
	}

	public void SetBGColor(bool isActive)
	{
		if (isActive)
		{
			this.bgSprite.color = this.selectedBGColor;
			this.frameSprite.color = this.selectedFrameColor;
		}
		else
		{
			this.bgSprite.color = this.normalBGColor;
			this.frameSprite.color = this.normalFrameColor;
		}
	}

	private static void SetActiveRestTime(bool active, UILabel text)
	{
		if (!active)
		{
			if (text.gameObject.activeSelf)
			{
				text.gameObject.SetActive(false);
			}
		}
		else if (!text.gameObject.activeSelf)
		{
			text.gameObject.SetActive(true);
		}
	}

	public void SetBannerErrorText(string stageName, bool active = true)
	{
		this.failedTextLabel.text = stageName;
		this.failedTextLabel.gameObject.SetActive(active);
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		this.SetNewClearStatus();
		this.bgSprite.color = this.normalBGColor;
		this.SetOpenStatus();
		if (DataMng.Instance().StageGimmick.DataDic.ContainsKey(this.worldStageData.worldStageM.worldStageId))
		{
			this.stateLabelColor.Add(new Color(1f, 0.94f, 0f));
			this.stateEffectColor.Add(new Color(0f, 0.51f, 0f));
			this.campaignTextList.Add(StringMaster.GetString("QuestGimmick"));
		}
		this.GetCampaignData();
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

	private void InitializeBannerTex()
	{
		if (this.shader == null)
		{
			this.shader = Shader.Find("Effect/TransparentColored_GrayScaleControl");
			this.bannerTex.material = new Material(this.shader);
		}
	}

	private void SetOpenStatus()
	{
		this.InitializeBannerTex();
		if (this.worldStageData.wdi.isOpen == 1)
		{
			this.timeLabel.gameObject.SetActive(true);
			this.SetTimeStatus();
			this.bannerTex.material.SetFloat("_Rate", 0f);
			this.bannerTex.color = this.openBannerCol;
			this.goCloseKey.SetActive(false);
		}
		else
		{
			this.timeLabel.gameObject.SetActive(false);
			this.bannerTex.material.SetFloat("_Rate", 1f);
			this.bannerTex.color = this.closeBannerCol;
			this.goCloseKey.SetActive(true);
		}
	}

	private void ReleaseBannerTexture()
	{
		if (this.bannerTex.material != null)
		{
			if (this.bannerTex.material.mainTexture != null)
			{
				this.bannerTex.material.mainTexture = null;
			}
			this.bannerTex.material = null;
		}
	}

	protected override void OnDestroy()
	{
		DataMng dataMng = DataMng.Instance();
		dataMng.OnCampaignUpdate = (Action<GameWebAPI.RespDataCP_Campaign, bool>)Delegate.Remove(dataMng.OnCampaignUpdate, new Action<GameWebAPI.RespDataCP_Campaign, bool>(this.OnCampaignUpdate));
		base.OnDestroy();
		this.ReleaseBannerTexture();
	}

	private void SetTimeStatus()
	{
		this.restTimeDate = this.worldStageData.wdi.closeTime;
		this.totalSeconds = GUIBannerParts.GetRestTimeSeconds(this.restTimeDate);
		if (this.WorldStageData.worldStageM.worldAreaId == ConstValue.QUEST_AREA_ID_EVENT)
		{
			if (this.totalSeconds >= 99999999)
			{
				if (this.IsMatchDayOfWeek(int.Parse(this.WorldStageData.worldStageM.worldStageId)))
				{
					this.totalSeconds = GUIBannerParts.GetRestTimeOneDaySeconds(this.restTimeDate);
				}
				else
				{
					this.totalSeconds = 0;
				}
			}
			GUIBannerParts.SetTimeTextForDayOfWeek(this.timeLabel, this.totalSeconds, this.restTimeDate, true);
		}
		else if (this.totalSeconds < 99999999)
		{
			GUIBannerParts.SetTimeText(this.timeLabel, this.totalSeconds, this.restTimeDate);
		}
		else
		{
			this.timeLabel.text = string.Empty;
		}
		if ((this.WorldStageData.worldStageM.worldAreaId == ConstValue.QUEST_AREA_ID_EVENT || this.WorldStageData.worldStageM.worldAreaId == ConstValue.QUEST_AREA_ID_ADVENT) && 0 < this.totalSeconds)
		{
			base.InvokeRepeating("CountDown", 1f, 1f);
		}
	}

	private void SetNewClearStatus()
	{
		switch (this.WorldStageData.status)
		{
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
		if (flag && !this.selectPanelA.animationMoving)
		{
			base.OnTouchEnded(touch, pos, flag);
			float magnitude = (this.beganPostion - pos).magnitude;
			if (magnitude < 40f && !this.isTouchEndFromChild)
			{
				if (this.worldStageData.wdi.isOpen != 1)
				{
					int useStoneNum = int.Parse(this.worldStageData.worldStageM.forceOpenNum);
					int hasStoneNum = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
					CMD_ChangePOP_STONE cd = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP_STONE") as CMD_ChangePOP_STONE;
					cd.Title = StringMaster.GetString("QuestUnlockTitle");
					cd.OnPushedYesAction = delegate()
					{
						if (hasStoneNum < useStoneNum)
						{
							cd.SetCloseAction(delegate(int idx)
							{
								GUIMain.ShowCommonDialog(delegate(int i)
								{
									GUIPlayerStatus.RefreshParams_S(false);
								}, "CMD_Shop");
							});
							cd.ClosePanel(true);
						}
						else
						{
							AppCoroutine.Start(this.BuyEventDungeon(), false);
							cd.ClosePanel(true);
						}
					};
					cd.Info = string.Format(StringMaster.GetString("QuestUnlockInfo"), ConstValue.EVENT_QUEST_OPEN_TIME);
					cd.SetDigistone(hasStoneNum, useStoneNum);
				}
				else
				{
					this.ExecuteTouch(false);
				}
			}
		}
	}

	private IEnumerator BuyEventDungeon()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		GameWebAPI.RequestWD_BuyDungeon request = new GameWebAPI.RequestWD_BuyDungeon
		{
			SetSendData = delegate(GameWebAPI.WD_Req_BuyDungeon param)
			{
				param.worldStageId = int.Parse(this.worldStageData.worldStageM.worldStageId);
			},
			OnReceived = delegate(GameWebAPI.RespDataWD_BuyDungeon response)
			{
				this.worldStageData.wdi.timeLeft = response.timeLeft;
			}
		};
		yield return AppCoroutine.Start(request.RunOneTime(new Action(this.BuyEventDng_OK), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
			if (CMD_QuestTOP.instance != null)
			{
				CMD_QuestTOP.instance.ClosePanel(true);
			}
		}, null), false);
		yield break;
	}

	private void BuyEventDng_OK()
	{
		RestrictionInput.EndLoad();
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point -= int.Parse(this.worldStageData.worldStageM.forceOpenNum);
		GUIPlayerStatus.RefreshParams_S(false);
		this.worldStageData.wdi.isOpen = 1;
		this.SetOpenStatus();
		this.ExecuteTouch(true);
	}

	private void ExecuteTouch(bool forceChange = false)
	{
		if (0 < this.totalSeconds)
		{
			CMD_QuestTOP.ChangeSelectA_StageL_S(base.IDX, forceChange);
		}
		else
		{
			CMD_Alert cmd_Alert = GUIMain.ShowCommonDialog(delegate(int i)
			{
				GUIManager.CloseAllCommonDialog(null);
			}, "CMD_Alert") as CMD_Alert;
			cmd_Alert.Title = StringMaster.GetString("QuestEventTitle");
			cmd_Alert.Info = StringMaster.GetString("QuestEventInfo");
			cmd_Alert.SetDisplayButton(CMD_Alert.DisplayButton.CLOSE);
		}
	}

	private void CountDown()
	{
		this.totalSeconds = GUIBannerParts.GetRestTimeSeconds(this.restTimeDate);
		if (this.WorldStageData.worldStageM.worldAreaId == "2")
		{
			if (this.totalSeconds >= 99999999)
			{
				if (this.IsMatchDayOfWeek(int.Parse(this.WorldStageData.worldStageM.worldStageId)))
				{
					this.totalSeconds = GUIBannerParts.GetRestTimeOneDaySeconds(this.restTimeDate);
				}
				else
				{
					this.totalSeconds = 0;
				}
			}
			GUIBannerParts.SetTimeTextForDayOfWeek(this.timeLabel, this.totalSeconds, this.restTimeDate, true);
		}
		else if (this.totalSeconds < 99999999)
		{
			GUIBannerParts.SetTimeText(this.timeLabel, this.totalSeconds, this.restTimeDate);
		}
		else
		{
			this.timeLabel.text = string.Empty;
		}
		if (this.totalSeconds <= 0)
		{
			base.CancelInvoke("CountDown");
		}
	}

	private bool IsMatchDayOfWeek(int world_stage_id)
	{
		DayOfWeek dayOfWeek = ServerDateTime.Now.DayOfWeek;
		int num = (int)dayOfWeek;
		int num2 = world_stage_id % 3000 - 1;
		return num == num2;
	}

	private void GetCampaignData()
	{
		DataMng dataMng = DataMng.Instance();
		dataMng.OnCampaignUpdate = (Action<GameWebAPI.RespDataCP_Campaign, bool>)Delegate.Combine(dataMng.OnCampaignUpdate, new Action<GameWebAPI.RespDataCP_Campaign, bool>(this.OnCampaignUpdate));
		this.OnCampaignUpdate(DataMng.Instance().RespDataCP_Campaign, DataMng.Instance().CampaignForceHide);
	}

	private void OnCampaignUpdate(GameWebAPI.RespDataCP_Campaign cmpList, bool forceHide)
	{
		if (cmpList != null && !forceHide)
		{
			List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> underwayCampaignList = this.GetUnderwayCampaignList(cmpList);
			if (underwayCampaignList.Count > 0)
			{
				this.SetCampaignData(underwayCampaignList);
			}
		}
	}

	private List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> GetUnderwayCampaignList(GameWebAPI.RespDataCP_Campaign campaign)
	{
		List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> list = new List<GameWebAPI.RespDataCP_Campaign.CampaignInfo>();
		DateTime now = ServerDateTime.Now;
		for (int i = 0; i < campaign.campaignInfo.Length; i++)
		{
			if (this.ExistCampaign(campaign.campaignInfo[i].GetCmpIdByEnum()) && campaign.campaignInfo[i].targetValue == this.worldStageData.worldStageM.worldStageId && campaign.campaignInfo[i].IsUnderway(now))
			{
				list.Add(campaign.campaignInfo[i]);
			}
		}
		return list;
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

	private void SetCampaignData(List<GameWebAPI.RespDataCP_Campaign.CampaignInfo> infos)
	{
		if (infos.Count > 0)
		{
			if (infos.Count > 1)
			{
				this.campaignTextList.Add(this.GetMultipleHoldingCampaignDescription());
			}
			else if (infos.Count == 1)
			{
				GameWebAPI.RespDataCP_Campaign.CampaignInfo campaignInfo = infos[0];
				GameWebAPI.RespDataCP_Campaign.CampaignType cmpIdByEnum = campaignInfo.GetCmpIdByEnum();
				float num = float.Parse(campaignInfo.rate);
				if (cmpIdByEnum == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDown || cmpIdByEnum == GameWebAPI.RespDataCP_Campaign.CampaignType.QuestStmDownMul)
				{
					num = Mathf.Ceil(1f / num);
				}
				this.campaignTextList.Add(this.GetDescription(cmpIdByEnum, num));
			}
			this.stateLabelColor.Add(new Color(0.78f, 0f, 0f));
			this.stateEffectColor.Add(new Color(1f, 0.94f, 0f));
		}
	}

	private string GetDescription(GameWebAPI.RespDataCP_Campaign.CampaignType cpmType, float rate)
	{
		return CampaignUtil.GetDescription(cpmType, rate, this.useLongDescription);
	}

	private string GetMultipleHoldingCampaignDescription()
	{
		return StringMaster.GetString("Campaign");
	}

	private void SetClearIcon()
	{
		this.ngSPR_NEW.spriteName = this.clearMark;
		this.ngSPR_NEW.MakePixelPerfect();
		this.ngSPR_NEW.GetComponent<UITweener>().enabled = false;
		this.ngSPR_NEW.alpha = 1f;
	}
}
