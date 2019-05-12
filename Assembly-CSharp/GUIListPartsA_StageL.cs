using Master;
using Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUIListPartsA_StageL : GUIListPartBS
{
	[SerializeField]
	[Header("デフォルトの進捗の画像")]
	private string normalProgress = "Common02_ProgressG";

	[Header("クリア進捗の画像")]
	[SerializeField]
	private string clearProgress = "Common02_Progress";

	[SerializeField]
	[Header("進捗の四角")]
	private List<GameObject> goPROGRESS_LIST;

	[SerializeField]
	[Header("NEWのGameObject")]
	private GameObject goNEW;

	[SerializeField]
	[Header("[の画像のGameObject")]
	private GameObject goFRAME_L;

	[SerializeField]
	[Header("]の画像のGameObject")]
	private GameObject goFRAME_R;

	[SerializeField]
	private GameObject goTXT_AREA;

	[SerializeField]
	private GameObject goTXT_AREA_NAME;

	[SerializeField]
	[Header("選択時の背景色")]
	private Color backgroundColor = Util.convertColor(0f, 100f, 0f, 200f);

	[SerializeField]
	[Header("非選択時の背景色")]
	private Color backgroundColorOff = Util.convertColor(0f, 200f, 0f, 70f);

	private Color defalutBackground2Color;

	[Header("背景のスプライト")]
	[SerializeField]
	private UISprite background;

	private UILabel ngTXT_AREA;

	private UILabel ngTXT_AREA_NAME;

	[Header("進捗マークのスプライト")]
	private List<UISprite> progressSprites;

	[Header("NEWとCLEARのアイコン")]
	[SerializeField]
	private UISprite ngSPR_NEW;

	[Header("クリアのマークの画像")]
	[SerializeField]
	private string clearMark = "Common02_text_Clear";

	[SerializeField]
	[Header("ステージギミック表記Obj")]
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
		this.ngSPR_NEW = this.goNEW.GetComponent<UISprite>();
		this.ngTXT_AREA = this.goTXT_AREA.GetComponent<UILabel>();
		this.ngTXT_AREA_NAME = this.goTXT_AREA_NAME.GetComponent<UILabel>();
		this.progressSprites = new List<UISprite>();
		foreach (GameObject gameObject in this.goPROGRESS_LIST)
		{
			this.progressSprites.Add(gameObject.GetComponent<UISprite>());
		}
		this.background.color = this.backgroundColorOff;
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
		int dngCount = this.Data.dngCount;
		int dngClearCount = this.Data.dngClearCount;
		for (int i = 0; i < this.goPROGRESS_LIST.Count; i++)
		{
			if (i < dngClearCount)
			{
				this.progressSprites[i].spriteName = this.clearProgress;
			}
			else
			{
				this.progressSprites[i].spriteName = this.normalProgress;
			}
			if (i < dngCount)
			{
				this.goPROGRESS_LIST[i].SetActive(true);
			}
			else
			{
				this.goPROGRESS_LIST[i].SetActive(false);
			}
		}
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		switch (this.data.status)
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
		if (this.ngTXT_AREA != null)
		{
			if (CMD_QuestTOP.instance.IsSpecialDungeon())
			{
				this.ngTXT_AREA.text = StringMaster.GetString("QuestSpecial");
			}
			else
			{
				this.ngTXT_AREA.text = string.Format(StringMaster.GetString("GUIListPartsA_txt"), int.Parse(this.data.worldStageM.worldStageId));
			}
		}
		if (this.ngTXT_AREA_NAME != null)
		{
			this.ngTXT_AREA_NAME.text = this.data.worldStageM.name;
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
		else
		{
			this.background.color = this.backgroundColorOff;
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
			}).Where(<>__TranspIdent2 => <>__TranspIdent2.xx.worldDungeonM.worldDungeonId == <>__TranspIdent2.yy.Key && <>__TranspIdent2.xx.status != 1).Select(<>__TranspIdent2 => <>__TranspIdent2.xx);
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

	private void SetClearIcon()
	{
		this.ngSPR_NEW.spriteName = this.clearMark;
		this.ngSPR_NEW.MakePixelPerfect();
		this.ngSPR_NEW.GetComponent<UITweener>().enabled = false;
		this.ngSPR_NEW.alpha = 1f;
	}
}
