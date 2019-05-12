using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIListPartsQuestSelect : GUIListPartBS
{
	[SerializeField]
	[Header("バナーのテクスチャ")]
	public UITexture bannerTex;

	[Header("バナー読み込み失敗時のテキスト")]
	[SerializeField]
	private UILabel failedTextLabel;

	[SerializeField]
	[Header("NEW スプライト")]
	private UISprite spNew;

	private QuestData.WorldAreaData areaData;

	private Shader shader;

	public override void SetData()
	{
		this.areaData = CMD_QuestSelect.instance.GetData(base.IDX);
	}

	public override void InitParts()
	{
		this.ShowGUI();
	}

	public override void RefreshParts()
	{
		this.ShowGUI();
	}

	protected override void Awake()
	{
		base.Awake();
		this.InitializeBannerTex();
	}

	public override void ShowGUI()
	{
		this.ShowData();
		base.ShowGUI();
		AppCoroutine.Start(this.DownloadBannerTexture(), false);
	}

	private void ShowData()
	{
		List<QuestData.WorldStageData> worldStageData_ByAreaID = ClassSingleton<QuestData>.Instance.GetWorldStageData_ByAreaID(this.areaData.data.worldAreaId, false);
		QuestData.WORLD_STATUS world_STATUS = ClassSingleton<QuestData>.Instance.CheckAreaStatus(worldStageData_ByAreaID, this.areaData.data.worldAreaId);
		if (world_STATUS == QuestData.WORLD_STATUS.UNLOCK_NEW)
		{
			this.spNew.gameObject.SetActive(true);
		}
		else
		{
			this.spNew.gameObject.SetActive(false);
		}
		CampaignLabelArea component = base.gameObject.GetComponent<CampaignLabelArea>();
		component.StageDataList = worldStageData_ByAreaID;
		component.InitUI();
		component.Refresh();
	}

	private IEnumerator DownloadBannerTexture()
	{
		this.failedTextLabel.text = this.areaData.data.name;
		this.failedTextLabel.gameObject.SetActive(true);
		string path = ConstValue.APP_ASSET_DOMAIN + "/asset/img/events/" + this.areaData.data.img;
		yield return TextureManager.instance.Load(path, delegate(Texture2D tex)
		{
			if (tex != null)
			{
				this.failedTextLabel.text = string.Empty;
				this.failedTextLabel.gameObject.SetActive(false);
				this.bannerTex.mainTexture = tex;
				this.bannerTex.material.mainTexture = tex;
			}
		}, 30f, true);
		yield break;
	}

	private void InitializeBannerTex()
	{
		if (this.shader == null)
		{
			this.shader = Shader.Find("Effect/TransparentColored_GrayScaleControl");
			this.bannerTex.material = new Material(this.shader);
			this.bannerTex.material.SetFloat("_Rate", 0f);
			this.bannerTex.color = Color.white;
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
		this.beganPostion = pos;
		base.OnTouchBegan(touch, pos);
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
			if (magnitude < 40f)
			{
				this.OnTouchEndedProcess();
			}
		}
	}

	private void OnTouchEndedProcess()
	{
		if (this.areaData.data.worldAreaId == "5")
		{
			GUIMain.ShowCommonDialog(null, "CMD_PvPTop");
		}
		else
		{
			CMD_QuestTOP.AreaData = this.areaData;
			if (GUIMain.GetNowGUIName() == "UIResult")
			{
				CMD cmd = GUIMain.ShowCommonDialog(delegate(int idx)
				{
					CMD_BattleNextChoice.OnCloseQuestTOP(idx);
				}, "CMD_QuestTOP") as CMD;
				cmd.SetForceReturnValue(1);
				PartsTitleBase partsTitle = cmd.PartsTitle;
				if (partsTitle != null)
				{
					partsTitle.SetReturnAct(delegate(int i)
					{
						cmd.SetCloseAction(null);
						cmd.ClosePanel(true);
					});
				}
			}
			else
			{
				GUIMain.ShowCommonDialog(null, "CMD_QuestTOP");
			}
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.ReleaseBannerTexture();
	}
}
