using Master;
using System;
using System.Collections;
using UnityEngine;

public class GUIListPartsStone : GUIListPartBS
{
	[SerializeField]
	private UISprite ngBASE;

	[SerializeField]
	private UISprite ngICON;

	[SerializeField]
	private UILabel ngTX_NAME;

	[SerializeField]
	private UILabel ngTX_PRICE;

	[SerializeField]
	private UILabel ngTX_BUY_BTN;

	[SerializeField]
	private UITexture ngTEX_BANNER;

	[SerializeField]
	private UILabel ngTX_TIME_LIMIT;

	[SerializeField]
	private UISprite spBUY_BTN;

	[SerializeField]
	private UISprite spEXP_BTN;

	[SerializeField]
	private GUICollider colBUY_BTN;

	[SerializeField]
	private GUICollider colEXP_BTN;

	private StoreUtil.StoneStoreData data;

	private DateTime restTimeDate;

	private int totalSeconds;

	private CMD_AgeConfirmation ageConfDialog;

	public Action Callback { private get; set; }

	public StoreUtil.StoneStoreData Data
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
		}
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		this.ngTX_BUY_BTN.text = StringMaster.GetString("ShopBuyButton");
		if (this.data.packFlg)
		{
			this.SetTimeStatus();
			AppCoroutine.Start(this.LoadBannerTexture(this.data.imgPath), false);
		}
		else
		{
			this.SetDetail();
		}
	}

	private IEnumerator DownloadBannerTexture(string path)
	{
		Action<Texture2D> callback = delegate(Texture2D texture)
		{
			if (texture != null)
			{
				this.ngTEX_BANNER.mainTexture = texture;
				this.ngICON.gameObject.SetActive(false);
				this.ngTX_NAME.gameObject.SetActive(false);
				this.ngTX_PRICE.gameObject.SetActive(false);
			}
			else
			{
				this.ngTEX_BANNER.gameObject.SetActive(false);
				this.SetDetail();
				this.ngICON.gameObject.SetActive(false);
			}
		};
		string downloadURL = ConstValue.APP_ASSET_DOMAIN + "/asset/img/" + path;
		yield return TextureManager.instance.Load(downloadURL, callback, 30f, true);
		yield break;
	}

	private IEnumerator LoadBannerTexture(string path)
	{
		yield return AssetDataMng.Instance().LoadObject(path, delegate(UnityEngine.Object obj)
		{
			if (obj != null)
			{
				Texture2D mainTexture = obj as Texture2D;
				this.ngTEX_BANNER.mainTexture = mainTexture;
				this.ngICON.gameObject.SetActive(false);
				this.ngTX_NAME.gameObject.SetActive(false);
				this.ngTX_PRICE.gameObject.SetActive(false);
			}
			else
			{
				this.ngTEX_BANNER.gameObject.SetActive(false);
				this.SetDetail();
				this.ngICON.gameObject.SetActive(false);
			}
		}, true);
		yield break;
	}

	private void SetDetail()
	{
		ShopStoneIconList component = this.ngICON.GetComponent<ShopStoneIconList>();
		this.ngICON.spriteName = component.GetSpriteName(this.data.spriteType);
		this.ngICON.MakePixelPerfect();
		this.ngTX_NAME.text = this.data.productTitle;
		this.ngTX_PRICE.text = StringFormat.Yen(this.data.price);
	}

	private void SetTimeStatus()
	{
		if (this.data.closeTime == null)
		{
			this.ngTX_TIME_LIMIT.gameObject.SetActive(false);
			return;
		}
		this.restTimeDate = DateTime.Parse(this.data.closeTime);
		this.totalSeconds = GUIBannerParts.GetRestTimeSeconds(this.restTimeDate);
		if (this.data.countDownDispFlg)
		{
			if (this.totalSeconds >= 99999999)
			{
				this.totalSeconds = GUIBannerParts.GetRestTimeOneDaySeconds(this.restTimeDate);
			}
			GUIBannerParts.SetTimeTextForDayOfWeek(this.ngTX_TIME_LIMIT, this.totalSeconds, this.restTimeDate, false);
		}
		else if (this.totalSeconds < 99999999)
		{
			GUIBannerParts.SetTimeText(this.ngTX_TIME_LIMIT, this.totalSeconds, this.restTimeDate);
		}
		else
		{
			this.ngTX_TIME_LIMIT.text = string.Empty;
		}
		if (this.data.countDownDispFlg || this.ngTX_TIME_LIMIT.text != string.Empty)
		{
			if (0 < this.totalSeconds)
			{
				base.InvokeRepeating("CountDown", 1f, 1f);
			}
			else
			{
				this.ngTX_TIME_LIMIT.text = StringMaster.GetString("ExchangeCloseTitle");
				this.DisableBuyButtons();
			}
		}
	}

	private void CountDown()
	{
		this.totalSeconds = GUIBannerParts.GetRestTimeSeconds(this.restTimeDate);
		if (this.totalSeconds >= 99999999)
		{
			this.totalSeconds = GUIBannerParts.GetRestTimeOneDaySeconds(this.restTimeDate);
		}
		if (this.data.countDownDispFlg)
		{
			GUIBannerParts.SetTimeTextForDayOfWeek(this.ngTX_TIME_LIMIT, this.totalSeconds, this.restTimeDate, false);
		}
		else
		{
			GUIBannerParts.SetTimeText(this.ngTX_TIME_LIMIT, this.totalSeconds, this.restTimeDate);
		}
		if (this.totalSeconds <= 0)
		{
			this.ngTX_TIME_LIMIT.text = StringMaster.GetString("ExchangeCloseTitle");
			base.CancelInvoke("CountDown");
			this.DisableBuyButtons();
		}
	}

	private void DisableBuyButtons()
	{
		this.spBUY_BTN.color = new Color(0.5f, 0.5f, 0.5f, 1f);
		this.spEXP_BTN.color = new Color(0.5f, 0.5f, 0.5f, 1f);
		this.colBUY_BTN.activeCollider = false;
		this.colEXP_BTN.activeCollider = false;
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
		base.OnDestroy();
	}

	private void OnClickedDetail()
	{
		CMD_ModalItemPackDetail.Data = this.Data;
		GUIMain.ShowCommonDialog(null, "CMD_ModalItemPackDetail");
	}

	private void OnClickedPurchase()
	{
		if (this.AgeConfirmd())
		{
			return;
		}
		if (this.CheckMaxDigistoneCount())
		{
			return;
		}
		string birthday = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.birthday;
		DateTime dateTime = DateTime.Parse(birthday);
		if (!string.IsNullOrEmpty(birthday))
		{
			if (ServerDateTime.Now < dateTime.AddYears(20))
			{
				this.AgreementConfirmation();
			}
			else
			{
				this.BranchHowBuy();
			}
		}
	}

	private void AgreementConfirmation()
	{
		CMD_AgreementConfirmation cmd_AgreementConfirmation = GUIMain.ShowCommonDialog(null, "CMD_AgreementConfirmation") as CMD_AgreementConfirmation;
		cmd_AgreementConfirmation.SetActionAgreementPopupClosed(delegate(bool result)
		{
			if (result)
			{
				this.BranchHowBuy();
			}
		});
	}

	private void BranchHowBuy()
	{
		base.StartCoroutine(this.BuyRealStone(this.Data.productId, new Action<bool>(CMD_Shop.instance.OnEndConsume)));
	}

	private IEnumerator BuyRealStone(string productId, Action<bool> onCompleted)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		APIRequestTask apirequestTask = DataMng.Instance().RequestAgeCheck(productId, "3", false);
		apirequestTask.Add(new NormalTask(delegate()
		{
			if (DataMng.Instance().RespDataSH_AgeCheck.isShopMaintenance == 1)
			{
				StoreInit.Instance().SetStatusToDoneInit();
				AlertManager.ShowAlertDialog(delegate(int nop)
				{
					if (CMD_Shop.instance != null)
					{
						CMD_Shop.instance.ClosePanel(true);
					}
				}, "C-SH05");
			}
			else if (DataMng.Instance().RespDataSH_AgeCheck.isOverDigiStone == 1)
			{
				AlertManager.ShowAlertDialog(null, "C-SH06");
			}
			else
			{
				if (DataMng.Instance().RespDataSH_AgeCheck.purchaseEnabled == 1)
				{
					return this.StartPurchaseItem(productId, onCompleted);
				}
				AlertManager.ShowAlertDialog(null, "C-SH04");
			}
			return null;
		}));
		return apirequestTask.Run(delegate
		{
			RestrictionInput.EndLoad();
		}, delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
		}, null);
	}

	private IEnumerator StartPurchaseItem(string productId, Action<bool> onCompleted)
	{
		bool isFinished = false;
		Action<bool> onFnished = null;
		if (CMD_Shop.instance != null)
		{
			onFnished = delegate(bool isSuccess)
			{
				if (onCompleted != null)
				{
					if (isSuccess && this.data.limitCount > 0)
					{
						this.data.purchasedCount++;
						if (this.data.purchasedCount >= this.data.limitCount)
						{
							if (CMD_Shop.instance != null)
							{
								CMD_Shop.instance.DeleteListParts(base.IDX);
							}
						}
					}
					onCompleted(isSuccess);
				}
				isFinished = true;
			};
		}
		else
		{
			onFnished = delegate(bool nop)
			{
				isFinished = true;
			};
		}
		StoreUtil.Instance().StartPurchaseItem(productId, onFnished);
		while (!isFinished)
		{
			yield return null;
		}
		yield break;
	}

	private bool AgeConfirmd()
	{
		if (string.IsNullOrEmpty(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.birthday))
		{
			this.ageConfDialog = (CMD_AgeConfirmation)GUIMain.ShowCommonDialog(new Action<int>(this.CompleteAgeConfirmd), "CMD_AgeConfirmation");
			return true;
		}
		return false;
	}

	private bool CheckMaxDigistoneCount()
	{
		bool flag = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point + this.Data.num > ConstValue.MAX_DIGISTONE_COUNT;
		if (flag && AlertManager.CheckDialogMessage("C-SH06"))
		{
			AlertManager.ShowAlertDialog(null, "C-SH06");
		}
		return flag;
	}

	private void CompleteAgeConfirmd(int idx)
	{
		if (idx == this.ageConfDialog.GetCloseButtonIndex() || idx == -1)
		{
			return;
		}
		this.OnClickedPurchase();
	}
}
