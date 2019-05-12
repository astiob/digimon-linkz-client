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

	private StoreUtil.StoneStoreData data;

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
		ShopStoneIconList component = this.ngICON.GetComponent<ShopStoneIconList>();
		this.ngICON.spriteName = component.GetSpriteName(this.data.spriteType);
		this.ngICON.MakePixelPerfect();
		this.ngTX_NAME.text = this.data.productTitle;
		this.ngTX_PRICE.text = StringFormat.Yen(this.data.price);
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
		return apirequestTask.Run(new Action(RestrictionInput.EndLoad), null, null);
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
