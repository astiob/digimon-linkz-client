using Master;
using Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUIListPartsA_StageL_Ticket : GUIListPartBS
{
	[Header("選択時の背景色")]
	[SerializeField]
	private Color backgroundColor = Util.convertColor(255f, 200f, 0f, 30f);

	[Header("非選択時の背景色 (有償) ")]
	[SerializeField]
	private Color backgroundColorOff_Paid = Util.convertColor(180f, 0f, 0f, 230f);

	[Header("非選択時の背景色 (無償・有効期限なし) ")]
	[SerializeField]
	private Color backgroundColorOff_NoLimit = Util.convertColor(0f, 0f, 180f, 230f);

	[Header("非選択時の背景色 (無償・有効期限有り) ")]
	[SerializeField]
	private Color backgroundColorOff_Limit = Util.convertColor(0f, 180f, 0f, 230f);

	[Header("サムネイル")]
	[SerializeField]
	private UITexture ngTICKET_THUMBNAIL;

	[Header("背景のスプライト")]
	[SerializeField]
	private UISprite background;

	[Header("チケット名ラベル")]
	[SerializeField]
	private UILabel ngTXT_TICKET_NAME;

	[Header("チケット数ラベル")]
	[SerializeField]
	private UILabel ngTXT_TICKET_NUM;

	[Header("チケット有効期限")]
	[SerializeField]
	private UILabel ngTXT_TICKET_EXPIRE_TIME;

	[Header("チケット更新日")]
	[SerializeField]
	private UILabel ngTXT_TICKET_UPDATE_TIME;

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

	private bool isLimit;

	private bool isFree;

	[SerializeField]
	private bool useLongDescription;

	private QuestData.WorldStageData data;

	private bool isTouchEndFromChild;

	public GUISelectPanelA_StageL selectPanelA;

	private List<string> campaignTextList = new List<string>();

	[SerializeField]
	private GameWebAPI.RespDataCP_Campaign.CampaignType[] refCampaignType;

	public QuestData.WorldStageData Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
			this.ShowGUI();
		}
	}

	protected override void Awake()
	{
		base.Awake();
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

	public void SetProgress()
	{
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM dungeonTicketM = MasterDataMng.Instance().RespDataMA_DungeonTicketMaster.dungeonTicketM.FirstOrDefault((GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM x) => this.data.wdi.dungeons[0].dungeonTicketId == x.dungeonTicketId);
		if (dungeonTicketM != null)
		{
			Texture2D texture2D = NGUIUtil.LoadTexture(dungeonTicketM.img);
			if (texture2D != null)
			{
				NGUIUtil.ChangeUITexture(this.ngTICKET_THUMBNAIL, texture2D, false);
			}
			if (!string.IsNullOrEmpty(this.data.wdi.expireTime))
			{
				this.ngTXT_TICKET_EXPIRE_TIME.text = string.Format(StringMaster.GetString("ExchangeTimeLimit"), this.data.wdi.expireTime);
				this.isLimit = true;
			}
			else
			{
				this.ngTXT_TICKET_EXPIRE_TIME.text = string.Format(StringMaster.GetString("ExchangeTimeLimit"), StringMaster.GetString("SystemNone"));
				this.isLimit = false;
			}
			this.isFree = (dungeonTicketM.freeFlg == "1");
			if (this.ngTXT_TICKET_UPDATE_TIME != null)
			{
				string arg = string.Empty;
				string updateTime = this.data.wdi.updateTime;
				int num = updateTime.IndexOf(' ', 0);
				if (num != -1)
				{
					arg = updateTime.Substring(0, num);
				}
				else
				{
					arg = updateTime;
				}
				this.ngTXT_TICKET_UPDATE_TIME.text = string.Format(StringMaster.GetString("TicketQuestUpdateTime2"), arg);
			}
			this.SetBGColor(false);
		}
		if (this.ngTXT_TICKET_NAME != null)
		{
			this.ngTXT_TICKET_NAME.text = this.data.worldStageM.name;
		}
		if (this.ngTXT_TICKET_NUM != null)
		{
			this.ngTXT_TICKET_NUM.text = this.Data.wdi.totalTicketNum.ToString();
		}
		this.SetStageGimmick();
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable() && !base.AvoidDisableAllCollider)
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
		if (GUICollider.IsAllColliderDisable() && !base.AvoidDisableAllCollider)
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
	}

	public void SetBGColor(bool isActive)
	{
		if (isActive)
		{
			this.background.color = this.backgroundColor;
		}
		else if (this.isFree)
		{
			if (this.isLimit)
			{
				this.background.color = this.backgroundColorOff_Limit;
			}
			else
			{
				this.background.color = this.backgroundColorOff_NoLimit;
			}
		}
		else
		{
			this.background.color = this.backgroundColorOff_Paid;
		}
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable() && !base.AvoidDisableAllCollider)
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
				CMD_QuestTOP.ChangeSelectA_StageL_S(base.IDX, false);
			}
		}
	}

	private void OnClickedBtnSelect()
	{
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void OnDestroy()
	{
		DataMng dataMng = DataMng.Instance();
		dataMng.OnCampaignUpdate = (Action<GameWebAPI.RespDataCP_Campaign, bool>)Delegate.Remove(dataMng.OnCampaignUpdate, new Action<GameWebAPI.RespDataCP_Campaign, bool>(this.OnCampaignUpdate));
		base.OnDestroy();
	}

	private void SetStageGimmick()
	{
		if (DataMng.Instance().StageGimmick.ContainsStage(this.data.worldStageM.worldStageId))
		{
			List<QuestData.WorldDungeonData> wddL = this.Data.wddL;
			IEnumerable<QuestData.WorldDungeonData> source = wddL.SelectMany((QuestData.WorldDungeonData xx) => DataMng.Instance().StageGimmick.DataDic[this.data.worldStageM.worldStageId], (QuestData.WorldDungeonData xx, KeyValuePair<string, List<string>> yy) => new
			{
				xx,
				yy
			}).Where(<>__TranspIdent4 => <>__TranspIdent4.xx.worldDungeonM.worldDungeonId == <>__TranspIdent4.yy.Key && <>__TranspIdent4.xx.status != 1).Select(<>__TranspIdent4 => <>__TranspIdent4.xx);
			if (source.Count<QuestData.WorldDungeonData>() != 0)
			{
				this.stateLabelColor.Add(new Color(1f, 0.94f, 0f));
				this.stateEffectColor.Add(new Color(0f, 0.51f, 0f));
				this.campaignTextList.Add(StringMaster.GetString("QuestGimmick"));
			}
		}
		this.GetCampaignData();
		bool flag = this.campaignTextList.Count > 0;
		this.stageGimmickObj.SetActive(flag);
		if (flag && this.labelToggle != null)
		{
			if (this.campaignTextList.Count > 1)
			{
				this.labelToggle.InitToggleData(this.stageStateLabel, this.campaignTextList, this.stateLabelColor, this.stateEffectColor, this.gimmickFadeTime, this.gimmickViewTime, true);
			}
			else
			{
				this.stageStateLabel.text = this.campaignTextList[0];
				this.stageStateLabel.color = this.stateLabelColor[0];
				this.stageStateLabel.effectColor = this.stateEffectColor[0];
			}
		}
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
			if (this.ExistCampaign(campaign.campaignInfo[i].GetCmpIdByEnum()) && campaign.campaignInfo[i].targetValue == this.data.worldStageM.worldStageId && campaign.campaignInfo[i].IsUnderway(now))
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

	protected string GetDescription(GameWebAPI.RespDataCP_Campaign.CampaignType cpmType, float rate)
	{
		return CampaignUtil.GetDescription(cpmType, rate, this.useLongDescription);
	}

	protected string GetMultipleHoldingCampaignDescription()
	{
		return StringMaster.GetString("Campaign");
	}
}
