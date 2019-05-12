using System;
using System.Collections;
using UnityEngine;

public sealed class GUIListPartsMissionSelect : GUIListPartBS
{
	[SerializeField]
	[Header("NEWの スプライト")]
	private UISprite newSprite;

	[SerializeField]
	[Header("回数表示")]
	private UILabel lbAbleCount;

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

	[SerializeField]
	[Header("バナー読み込み失敗時のテキスト")]
	private UILabel failedTextLabel;

	[Header("背景のスプライト")]
	[SerializeField]
	private UISprite bgSprite;

	[SerializeField]
	[Header("外枠のスプライト")]
	private UISprite frameSprite;

	[SerializeField]
	[Header("バナーのテクスチャ")]
	public UITexture bannerTex;

	[Header("バッジスプライト")]
	[SerializeField]
	private UISprite spBadge;

	private bool isTouchEndFromChild;

	public GUISelectPanelMissionSelect selectPanelA;

	private GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission data;

	protected override void Awake()
	{
		base.Awake();
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

	public override void ShowGUI()
	{
		base.ShowGUI();
		this.bgSprite.color = this.normalBGColor;
		this.SetNew();
		int num = int.Parse(this.data.displayGroup);
		CMD_Mission.MissionType missionType = (CMD_Mission.MissionType)num;
		CMD_Mission cmd_Mission = (CMD_Mission)base.GetInstanceCMD();
		string title = cmd_Mission.GetTitle(missionType);
		AppCoroutine.Start(this.DownloadBannerTexture(title), false);
	}

	private IEnumerator DownloadBannerTexture(string name)
	{
		this.failedTextLabel.gameObject.SetActive(true);
		this.failedTextLabel.text = name;
		int _type = int.Parse(this.data.displayGroup);
		CMD_Mission.MissionType type = (CMD_Mission.MissionType)_type;
		string path = string.Empty;
		switch (type)
		{
		case CMD_Mission.MissionType.Daily:
			path = "mission_daily";
			break;
		case CMD_Mission.MissionType.Total:
			path = "mission_total";
			break;
		case CMD_Mission.MissionType.Beginner:
			path = "mission_beginner";
			break;
		case CMD_Mission.MissionType.Midrange:
			path = "mission_midrange";
			break;
		}
		path = "MissionBanner/" + path;
		yield return AssetDataMng.Instance().LoadObject(path, delegate(UnityEngine.Object obj)
		{
			if (obj != null)
			{
				Texture2D mainTexture = obj as Texture2D;
				this.failedTextLabel.text = string.Empty;
				this.failedTextLabel.gameObject.SetActive(false);
				this.bannerTex.mainTexture = mainTexture;
			}
		}, true);
		yield break;
	}

	public void RefreshBadge()
	{
		if (this.spBadge != null)
		{
			int num = int.Parse(this.data.displayGroup);
			CMD_Mission.MissionType type = (CMD_Mission.MissionType)num;
			CMD_Mission cmd_Mission = (CMD_Mission)base.GetInstanceCMD();
			if (cmd_Mission.AnyDataNotReceived(type))
			{
				this.spBadge.gameObject.SetActive(true);
			}
			else
			{
				this.spBadge.gameObject.SetActive(false);
			}
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
				this.selectPanelA.SetCellAnim(base.IDX, true);
				int num = int.Parse(this.data.displayGroup);
				CMD_Mission.MissionType type = (CMD_Mission.MissionType)num;
				CMD_Mission cmd_Mission = (CMD_Mission)base.GetInstanceCMD();
				cmd_Mission.OnTouchedMission(type);
			}
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.bannerTex.mainTexture = null;
	}

	private string GetPrefsID()
	{
		return "GASHA_BANNER_IS_SHOWED_" + base.IDX.ToString();
	}

	private void SetNew()
	{
		string prefsID = this.GetPrefsID();
		if (PlayerPrefs.HasKey(prefsID))
		{
			if (this.newSprite != null)
			{
				this.newSprite.gameObject.SetActive(false);
			}
		}
		else if (this.newSprite != null)
		{
			this.newSprite.gameObject.SetActive(true);
		}
	}

	public void ResetNew()
	{
		if (this.newSprite != null)
		{
			this.newSprite.gameObject.SetActive(false);
		}
		string prefsID = this.GetPrefsID();
		PlayerPrefs.SetString(prefsID, DateTime.Now.ToString());
	}

	public override void SetData()
	{
		this.selectPanelA = base.transform.parent.gameObject.GetComponent<GUISelectPanelMissionSelect>();
		CMD_Mission cmd_Mission = (CMD_Mission)base.GetInstanceCMD();
		if (cmd_Mission != null)
		{
			GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[] misssionDataByIDX = cmd_Mission.GetMisssionDataByIDX(base.IDX);
			this.data = misssionDataByIDX[0];
		}
	}

	public override void InitParts()
	{
	}

	public override void RefreshParts()
	{
	}

	public override void InactiveParts()
	{
	}
}
