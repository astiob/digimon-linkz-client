using Master;
using System;
using UI.Gasha;
using UnityEngine;
using User;

public sealed class GUIListPartsGashaMain : GUIListPartBS
{
	[Header("NEWの スプライト")]
	[SerializeField]
	private UISprite newSprite;

	[SerializeField]
	[Header("回数表示")]
	private UILabel lbAbleCount;

	[SerializeField]
	[Header("選択してないときの背景色")]
	private Color normalBGColor = new Color32(180, 0, 0, byte.MaxValue);

	[Header("選択時の背景色")]
	[SerializeField]
	private Color selectedBGColor = new Color32(180, 0, 0, byte.MaxValue);

	[Header("選択してないときの外枠色")]
	[SerializeField]
	private Color normalFrameColor = Color.white;

	[SerializeField]
	[Header("選択時の外枠色")]
	private Color selectedFrameColor = new Color32(150, 0, 0, byte.MaxValue);

	[SerializeField]
	[Header("残り時間のラベル")]
	private UILabel timeLabel;

	[Header("バナー読み込み失敗時のテキスト")]
	[SerializeField]
	private UILabel failedTextLabel;

	[Header("背景のスプライト")]
	[SerializeField]
	private UISprite bgSprite;

	[SerializeField]
	[Header("外枠のスプライト")]
	private UISprite frameSprite;

	[SerializeField]
	[Header("バナーのテクスチャ")]
	private UITexture bannerTex;

	private GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo;

	private bool isTouchEndFromChild;

	private DateTime restTimeDate;

	public GUISelectPanelGashaMain selectPanelA;

	private int totalSeconds;

	private Action<int> pushedAction;

	public GameWebAPI.RespDataGA_GetGachaInfo.Result GashaInfo
	{
		get
		{
			return this.gashaInfo;
		}
		set
		{
			this.gashaInfo = value;
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
		if (99 < num)
		{
			num = 99;
		}
		this.lbAbleCount.text = num.ToString();
	}

	private int CalcAbleCount()
	{
		int result = 0;
		int numberExceptProtectedAssets = UserInventory.GetNumberExceptProtectedAssets(this.gashaInfo.priceType.GetCostAssetsCategory(), this.gashaInfo.priceType.GetCostAssetsValue());
		GameWebAPI.RespDataGA_GetGachaInfo.Detail detail = this.gashaInfo.details.GetDetail(1);
		if (detail != null)
		{
			result = detail.GetOnceRemainingPlayCount(numberExceptProtectedAssets);
		}
		return result;
	}

	public void ShowGUI(Texture buttonImage)
	{
		base.ShowGUI();
		this.bgSprite.color = this.normalBGColor;
		this.restTimeDate = DateTime.Parse(this.gashaInfo.endTime);
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
		this.bannerTex.mainTexture = buttonImage;
		this.ShowAbleCount();
		this.SetNew();
	}

	private void CountDown()
	{
		this.restTimeDate = DateTime.Parse(this.gashaInfo.endTime);
		this.totalSeconds = GUIBannerParts.GetRestTimeSeconds(this.restTimeDate);
		if (this.totalSeconds < 99999999)
		{
			GUIBannerParts.SetTimeText(this.timeLabel, this.totalSeconds, this.restTimeDate);
		}
		else
		{
			this.timeLabel.text = StringMaster.GetString("GashaRegular");
		}
		if (0 >= this.totalSeconds)
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
			Transform transform = this.selectPanelA.transform;
			float z = transform.localPosition.z;
			base.OnTouchEnded(touch, pos, flag);
			Vector3 localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
			transform.localPosition = localPosition;
			if (0 < this.totalSeconds)
			{
				float magnitude = (this.beganPostion - pos).magnitude;
				if (40f > magnitude && !this.isTouchEndFromChild && this.pushedAction != null)
				{
					this.pushedAction(base.IDX);
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
		return "GASHA_BANNER_IS_SHOWED_" + this.GashaInfo.gachaId;
	}

	private void SetNew()
	{
		if (null != this.newSprite)
		{
			if (PlayerPrefs.HasKey(this.GetPrefsID()))
			{
				this.newSprite.gameObject.SetActive(false);
			}
			else
			{
				this.newSprite.gameObject.SetActive(true);
			}
		}
	}

	public void ResetNew()
	{
		if (null != this.newSprite)
		{
			this.newSprite.gameObject.SetActive(false);
		}
		PlayerPrefs.SetString(this.GetPrefsID(), DateTime.Now.ToString());
	}

	public void SetPushedAction(Action<int> action)
	{
		this.pushedAction = action;
	}
}
