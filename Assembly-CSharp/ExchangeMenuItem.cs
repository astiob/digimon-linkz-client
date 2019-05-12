using ExchangeData;
using Master;
using System;
using System.Collections;
using UnityEngine;

public class ExchangeMenuItem : GUIListPartBS
{
	[SerializeField]
	private UISprite newSprite;

	[SerializeField]
	private UISprite availableMarkSprite;

	[SerializeField]
	private UILabel fukidashiLabel;

	[SerializeField]
	private UITexture bannerTex;

	private GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result bannerInfo;

	public void Init(GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result info)
	{
		this.bannerInfo = info;
		this.ShowGUI();
	}

	public void ReloadInfo(GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result info)
	{
		this.bannerInfo = info;
	}

	private bool IsExchange(GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result.Detail[] details)
	{
		foreach (GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result.Detail detail in details)
		{
			if (int.Parse(detail.needNum) <= detail.item.count)
			{
				return true;
			}
		}
		return false;
	}

	public override void ShowGUI()
	{
		this.Init();
		base.ShowGUI();
	}

	private void Init()
	{
		this.newSprite.enabled = GUIExchangeMenu.instance.IsNewExchange(this.bannerInfo.eventExchangeId);
		this.availableMarkSprite.enabled = this.IsExchange(this.bannerInfo.detail);
		if (ClassSingleton<ExchangeWebAPI>.Instance.IsExistAlwaysExchangeInfo(this.bannerInfo))
		{
			this.fukidashiLabel.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			base.InvokeRepeating("TimeSetting", 1f, 1f);
			this.TimeSetting();
		}
		base.StartCoroutine(this.DownloadBannerTexture(this.bannerInfo.img));
	}

	private void TimeSetting()
	{
		int restTimeSeconds = GUIBannerParts.GetRestTimeSeconds(DateTime.Parse(this.bannerInfo.endTime));
		int secondToDays = GUIBannerParts.GetSecondToDays(restTimeSeconds);
		int secondToHours = GUIBannerParts.GetSecondToHours(restTimeSeconds);
		int secondToMinutes = GUIBannerParts.GetSecondToMinutes(restTimeSeconds);
		if (secondToDays > 0)
		{
			this.fukidashiLabel.text = StringMaster.GetString("ExchangeRemaining") + string.Format(StringMaster.GetString("SystemTimeD"), secondToDays);
		}
		else if (secondToHours > 0)
		{
			this.fukidashiLabel.text = StringMaster.GetString("ExchangeRemaining") + string.Format(StringMaster.GetString("SystemTimeH"), secondToHours);
		}
		else if (secondToMinutes > 0)
		{
			this.fukidashiLabel.text = StringMaster.GetString("ExchangeRemaining") + string.Format(StringMaster.GetString("SystemTimeM"), secondToMinutes);
		}
		else if (restTimeSeconds > 0)
		{
			this.fukidashiLabel.text = StringMaster.GetString("ExchangeRemaining") + string.Format(StringMaster.GetString("SystemTimeM"), 1);
		}
		else
		{
			this.fukidashiLabel.text = StringMaster.GetString("ExchangeCloseTitle");
			base.CancelInvoke("TimeSetting");
		}
	}

	private IEnumerator DownloadBannerTexture(string path)
	{
		Action<Texture2D> callback = delegate(Texture2D texture)
		{
			if (texture != null)
			{
				this.bannerTex.mainTexture = texture;
			}
		};
		string downloadURL = ConstValue.APP_ASSET_DOMAIN + "/asset/img/" + path;
		yield return TextureManager.instance.Load(downloadURL, callback, 30f, true);
		yield break;
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
				if (this.fukidashiLabel.text == StringMaster.GetString("ExchangeCloseTitle"))
				{
					CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
					cmd_ModalMessage.Title = StringMaster.GetString("ExchangeCloseTitle");
					cmd_ModalMessage.Info = StringMaster.GetString("ExchangeCloseInfo");
					return;
				}
				this.newSprite.enabled = false;
				GUIExchangeMenu.instance.VisitExchange(this.bannerInfo.eventExchangeId);
				CMD_ClearingHouse.ExchangeResultInfo = this.bannerInfo;
				GUIMain.ShowCommonDialog(null, "CMD_ClearingHouse");
			}
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.bannerTex.mainTexture != null)
		{
			this.bannerTex.mainTexture = null;
		}
	}

	public void OffAvailableMark()
	{
		this.availableMarkSprite.enabled = false;
	}
}
