﻿using Master;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Common;
using UnityEngine;
using User;

public sealed class GUIListPartsA_StageL_Banner : GUIListPartBS
{
	[Header("選択してないときの背景色")]
	[SerializeField]
	private Color normalBGColor = new Color32(180, 0, 0, byte.MaxValue);

	[Header("選択時の背景色")]
	[SerializeField]
	private Color selectedBGColor = new Color32(180, 0, 0, byte.MaxValue);

	[Header("選択してないときの外枠色")]
	[SerializeField]
	private Color normalFrameColor = Color.white;

	[Header("選択時の外枠色")]
	[SerializeField]
	private Color selectedFrameColor = new Color32(150, 0, 0, byte.MaxValue);

	[Header("残り時間のラベル")]
	[SerializeField]
	private UILabel timeLabel;

	[Header("バナー読み込み失敗時のテキスト")]
	[SerializeField]
	private UILabel failedTextLabel;

	[Header("背景のスプライト")]
	[SerializeField]
	private UISprite bgSprite;

	[Header("外枠のスプライト")]
	[SerializeField]
	private UISprite frameSprite;

	[Header("バナーのテクスチャ")]
	[SerializeField]
	public UITexture bannerTex;

	[Header("オープンの時の色")]
	[SerializeField]
	private Color openBannerCol;

	[Header("クローズの時の色")]
	[SerializeField]
	private Color closeBannerCol;

	[Header("オープンしてないを示す鍵")]
	[SerializeField]
	private GameObject goCloseKey;

	[Header("オープンしてないを示す鍵(本体)")]
	[SerializeField]
	private UISprite ngCloseKey;

	[Header("NEWとCLEARのアイコン")]
	[SerializeField]
	private UISprite ngSPR_NEW;

	[Header("集計中のラベル")]
	[SerializeField]
	private UILabel aggregatingLabel;

	[Header("開催終了時のテキスト")]
	[SerializeField]
	private UILabel closedTextLabel;

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

	private QuestData.WorldStageData worldStageData;

	private bool isTouchEndFromChild;

	private DateTime restTimeDate;

	private bool animationMoving;

	private int totalSeconds;

	private Shader shader;

	public QuestData.WorldStageData WorldStageData
	{
		get
		{
			return this.worldStageData;
		}
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

	public void SetData(QuestData.WorldStageData worldStageData, bool animationMoving)
	{
		this.worldStageData = worldStageData;
		this.animationMoving = animationMoving;
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
		if (this.worldStageData.wdi.timeLeft == 0 && (this.worldStageData.wdi.dungeons == null || this.worldStageData.wdi.dungeons.Length == 0))
		{
			this.timeLabel.gameObject.SetActive(false);
			this.bannerTex.material.SetFloat("_Rate", 1f);
			this.bannerTex.color = this.closeBannerCol;
			this.goCloseKey.SetActive(true);
			this.ngCloseKey.enabled = false;
			this.closedTextLabel.transform.parent.gameObject.SetActive(true);
			this.closedTextLabel.text = StringMaster.GetString("QuestPointRankingClosed");
			DkLog.W("worldStageData.isViewRanking : " + this.worldStageData.isViewRanking, false);
			DkLog.W("worldStageData.isCounting : " + this.worldStageData.isCounting, false);
			bool active = this.worldStageData.isViewRanking && this.worldStageData.isCounting;
			this.aggregatingLabel.gameObject.SetActive(active);
			this.aggregatingLabel.text = StringMaster.GetString("QuestPointRankingCounting");
		}
		else if (this.worldStageData.wdi.isOpen == 1)
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
		else if (this.worldStageData.isViewRanking && this.worldStageData.isCounting)
		{
			this.timeLabel.text = StringMaster.GetString("QuestPointRankingCounting");
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
		int status = this.WorldStageData.status;
		if (status != 2)
		{
			if (status != 3)
			{
				if (status == 4)
				{
					this.SetClearIcon();
				}
			}
			else
			{
				this.ngSPR_NEW.gameObject.SetActive(false);
			}
		}
		else
		{
			this.ngSPR_NEW.MakePixelPerfect();
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
		if (flag && !this.animationMoving)
		{
			base.OnTouchEnded(touch, pos, flag);
			float magnitude = (this.beganPostion - pos).magnitude;
			if (magnitude < 40f && !this.isTouchEndFromChild)
			{
				if (this.worldStageData.wdi.isOpen != 1)
				{
					this.OpenConfirmForceOpen();
				}
				else
				{
					this.ExecuteTouch(false);
				}
			}
		}
	}

	private void OpenConfirmForceOpen()
	{
		GameWebAPI.ResponseWorldStageForceOpenMaster.ForceOpen questForceOpen = QuestData.GetQuestForceOpen(int.Parse(this.worldStageData.worldStageM.worldStageId));
		if (questForceOpen != null)
		{
			IPayConfirmNotice payConfirmNotice = FactoryPayConfirmNotice.CreateDialog(questForceOpen.assetCategoryId);
			payConfirmNotice.SetAssets(questForceOpen.assetCategoryId, questForceOpen.assetValue, questForceOpen.assetNum);
			string assetName = UIAssetName.GetAssetName(questForceOpen.assetCategoryId.ToString(), questForceOpen.assetValue.ToString());
			string info = string.Format(StringMaster.GetString("QuestForceOpenConfirmInfo"), assetName, questForceOpen.forceOpenMinute);
			payConfirmNotice.SetMessage(StringMaster.GetString("QuestUnlockTitle"), info);
			payConfirmNotice.SetPushActionYesButton(new Action<UnityEngine.Object>(this.OnConfirmPushYesButton));
		}
	}

	private void OnConfirmPushYesButton(UnityEngine.Object popup)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		CMD cmd = popup as CMD;
		if (null != cmd)
		{
			cmd.SetWindowClosedAction(delegate
			{
				AppCoroutine.Start(this.BuyEventDungeon(), false);
			});
			cmd.ClosePanel(true);
		}
	}

	private IEnumerator BuyEventDungeon()
	{
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
		GameWebAPI.ResponseWorldStageForceOpenMaster.ForceOpen questForceOpen = QuestData.GetQuestForceOpen(int.Parse(this.worldStageData.worldStageM.worldStageId));
		UserInventory.CalculateNumber((MasterDataMng.AssetCategory)questForceOpen.assetCategoryId, questForceOpen.assetValue.ToString(), questForceOpen.assetNum);
		GUIPlayerStatus.RefreshParams_S(false);
		this.worldStageData.wdi.isOpen = 1;
		this.SetOpenStatus();
		this.ExecuteTouch(true);
	}

	private void ExecuteTouch(bool forceChange)
	{
		if (0 >= this.totalSeconds && !this.worldStageData.isViewRanking)
		{
			CMD_Alert cmd_Alert = GUIMain.ShowCommonDialog(delegate(int i)
			{
				GUIManager.CloseAllCommonDialog(null);
			}, "CMD_Alert", null) as CMD_Alert;
			cmd_Alert.Title = StringMaster.GetString("QuestEventTitle");
			cmd_Alert.Info = StringMaster.GetString("QuestEventInfo");
			cmd_Alert.SetDisplayButton(CMD_Alert.DisplayButton.CLOSE);
		}
		else
		{
			CMD_QuestTOP.ChangeSelectA_StageL_S(base.IDX, forceChange);
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
		else if (this.worldStageData.isViewRanking && this.worldStageData.isCounting)
		{
			this.timeLabel.text = StringMaster.GetString("QuestPointRankingCounting");
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
