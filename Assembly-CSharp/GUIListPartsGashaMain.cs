using Master;
using System;
using UnityEngine;

public sealed class GUIListPartsGashaMain : GUIListPartBS
{
	[SerializeField]
	[Header("NEWの スプライト")]
	private UISprite newSprite;

	[SerializeField]
	[Header("回数表示")]
	private UILabel lbAbleCount;

	[SerializeField]
	[Header("選択してないときの背景色")]
	private Color normalBGColor = new Color32(180, 0, 0, byte.MaxValue);

	[SerializeField]
	[Header("選択時の背景色")]
	private Color selectedBGColor = new Color32(180, 0, 0, byte.MaxValue);

	[Header("選択してないときの外枠色")]
	[SerializeField]
	private Color normalFrameColor = Color.white;

	[SerializeField]
	[Header("選択時の外枠色")]
	private Color selectedFrameColor = new Color32(150, 0, 0, byte.MaxValue);

	[Header("残り時間のラベル")]
	[SerializeField]
	private UILabel timeLabel;

	[Header("バナー読み込み失敗時のテキスト")]
	[SerializeField]
	private UILabel failedTextLabel;

	[SerializeField]
	[Header("背景のスプライト")]
	private UISprite bgSprite;

	[SerializeField]
	[Header("外枠のスプライト")]
	private UISprite frameSprite;

	[Header("バナーのテクスチャ")]
	[SerializeField]
	public UITexture bannerTex;

	private GameWebAPI.RespDataGA_GetGachaInfo.Result data;

	private bool isTouchEndFromChild;

	private DateTime restTimeDate;

	public GUISelectPanelGashaMain selectPanelA;

	private int totalSeconds;

	public GameWebAPI.RespDataGA_GetGachaInfo.Result Data
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

	public void ShowAbleCount()
	{
		int num = this.CalcAbleCount();
		if (num > 99)
		{
			num = 99;
		}
		this.lbAbleCount.text = num.ToString();
	}

	private int CalcAbleCount()
	{
		int point = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
		int num = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.friendPoint);
		int num2 = 0;
		int num3;
		if (this.data.IsRare() || this.data.IsRareChip() || this.data.IsRareTicket())
		{
			num3 = point;
		}
		else if (this.data.IsLink() || this.data.IsLinkChip() || this.data.IsLinkTicket())
		{
			num3 = num;
		}
		else
		{
			num3 = 0;
		}
		int num4 = 0;
		if (this.data.isFirstGacha1.total == 1)
		{
			num4 = int.Parse(this.data.priceFirst1);
			if (num4 <= num3)
			{
				num2++;
				num3 -= num4;
			}
		}
		if (this.data.isFirstGacha1.today == 1)
		{
			string dailyresetFirst = this.data.dailyresetFirst1;
			if (dailyresetFirst == "1")
			{
				num4 = int.Parse(this.data.priceFirst1);
			}
			else if (dailyresetFirst == "0")
			{
				num4 = int.Parse(this.data.price);
			}
			else
			{
				global::Debug.LogError("dailyresetFirst1がおかしいです。");
			}
			if (num4 <= num3)
			{
				num2++;
				num3 -= num4;
			}
		}
		num4 = int.Parse(this.data.price);
		return num2 + num3 / num4;
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		this.bgSprite.color = this.normalBGColor;
		this.restTimeDate = DateTime.Parse(this.data.endTime);
		this.totalSeconds = GUIBannerParts.GetRestTimeSeconds(this.restTimeDate);
		if (this.totalSeconds < 99999999)
		{
			GUIBannerParts.SetTimeText(this.timeLabel, this.totalSeconds, this.restTimeDate);
		}
		else
		{
			this.timeLabel.text = StringMaster.GetString("GashaRegular");
		}
		if (0 < this.totalSeconds)
		{
			base.InvokeRepeating("CountDown", 1f, 1f);
		}
		this.bannerTex.mainTexture = this.data.tex;
		this.ShowAbleCount();
		this.SetNew();
	}

	private void CountDown()
	{
		this.restTimeDate = DateTime.Parse(this.data.endTime);
		this.totalSeconds = GUIBannerParts.GetRestTimeSeconds(this.restTimeDate);
		if (this.totalSeconds < 99999999)
		{
			GUIBannerParts.SetTimeText(this.timeLabel, this.totalSeconds, this.restTimeDate);
		}
		else
		{
			this.timeLabel.text = StringMaster.GetString("GashaRegular");
		}
		if (this.totalSeconds <= 0)
		{
			base.CancelInvoke("CountDown");
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
			float z = this.selectPanelA.transform.localPosition.z;
			base.OnTouchEnded(touch, pos, flag);
			this.selectPanelA.transform.SetLocalZ(z);
			if (0 < this.totalSeconds)
			{
				float magnitude = (this.beganPostion - pos).magnitude;
				if (magnitude < 40f && !this.isTouchEndFromChild && CMD_GashaTOP.instance != null && CMD_GashaTOP.instance.csSelectPanelGashaMain.SetCellAnim(base.IDX))
				{
					CMD_GashaTOP.instance.ChangeSelection(base.IDX);
				}
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
		return "GASHA_BANNER_IS_SHOWED_" + this.Data.gachaId;
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
}
