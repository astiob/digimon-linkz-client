using Master;
using Neptune.Purchase;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StoreUtil : MonoBehaviour
{
	private static StoreUtil instance;

	private List<StoreUtil.StoneStoreData> stoneStoreDataList;

	private Action<bool> actCallBackReConsume;

	private bool isFromStart;

	private string _productId;

	private int svRetryCt;

	private List<string> reqList;

	private Action<string> actCallBack_RequestProducts;

	public static StoreUtil Instance()
	{
		return StoreUtil.instance;
	}

	protected virtual void Awake()
	{
		StoreUtil.instance = this;
	}

	protected virtual void OnDestroy()
	{
		StoreUtil.instance = null;
	}

	public bool IsDebug()
	{
		return false;
	}

	public List<StoreUtil.StoneStoreData> GetStoneStoreDataList()
	{
		this.stoneStoreDataList = new List<StoreUtil.StoneStoreData>();
		GameWebAPI.RespDataSH_Info respDataSH_Info = DataMng.Instance().RespDataSH_Info;
		if (respDataSH_Info.shopList.Length > 0)
		{
			GameWebAPI.RespDataSH_Info.ProductList[] productList = respDataSH_Info.shopList[0].productList;
			if (productList.Length > 0)
			{
				for (int i = 0; i < productList.Length; i++)
				{
					StoreUtil.StoneStoreData stoneStoreData = new StoreUtil.StoneStoreData();
					stoneStoreData.productId = productList[i].productId;
					stoneStoreData.num = int.Parse(productList[i].acquireList[0].assetNum);
					stoneStoreData.price = int.Parse(productList[i].price);
					stoneStoreData.productTitle = productList[i].productTitle;
					stoneStoreData.priority = int.Parse(productList[i].priority);
					stoneStoreData.limitCount = int.Parse(productList[i].limitCount);
					if (!string.IsNullOrEmpty(productList[i].purchasedCount))
					{
						stoneStoreData.purchasedCount = int.Parse(productList[i].purchasedCount);
					}
					else
					{
						stoneStoreData.purchasedCount = 0;
					}
					if (productList[i].countDownDispFlg == "1")
					{
						stoneStoreData.countDownDispFlg = true;
					}
					else
					{
						stoneStoreData.countDownDispFlg = false;
					}
					if (productList[i].packFlg == "1")
					{
						stoneStoreData.packFlg = true;
					}
					else
					{
						stoneStoreData.packFlg = false;
					}
					stoneStoreData.closeTime = productList[i].closeTime;
					if (stoneStoreData.packFlg)
					{
						stoneStoreData.imgPath = productList[i].img;
						stoneStoreData.spriteType = StoreUtil.StoneSpriteType.STONE_SHOPLIST_1;
					}
					else
					{
						stoneStoreData.spriteType = (StoreUtil.StoneSpriteType)int.Parse(productList[i].img);
					}
					stoneStoreData.itemList = new List<GameWebAPI.RespDataSH_Info.AcquireList>();
					stoneStoreData.omakeList = new List<GameWebAPI.RespDataSH_Info.AcquireList>();
					for (int j = 0; j < productList[i].acquireList.Length; j++)
					{
						if (productList[i].acquireList[j].omakeFlg == "1")
						{
							stoneStoreData.omakeList.Add(productList[i].acquireList[j]);
						}
						else if (productList[i].acquireList[j].omakeFlg == "0")
						{
							stoneStoreData.itemList.Add(productList[i].acquireList[j]);
						}
					}
					this.stoneStoreDataList.Add(stoneStoreData);
				}
				this.stoneStoreDataList.Sort(new Comparison<StoreUtil.StoneStoreData>(this.ComparePriority));
			}
		}
		return this.stoneStoreDataList;
	}

	private int ComparePriority(StoreUtil.StoneStoreData A, StoreUtil.StoneStoreData B)
	{
		int priority = A.priority;
		int priority2 = B.priority;
		if (priority < priority2)
		{
			return -1;
		}
		if (priority > priority2)
		{
			return 1;
		}
		return 0;
	}

	public int GetPriceFromProductId(string productId)
	{
		List<StoreUtil.StoneStoreData> list = this.GetStoneStoreDataList();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].productId == productId)
			{
				return list[i].price;
			}
		}
		return 0;
	}

	public int GetNumFromProductId(string productId)
	{
		List<StoreUtil.StoneStoreData> list = this.GetStoneStoreDataList();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].productId == productId)
			{
				return list[i].num;
			}
		}
		return 0;
	}

	public StoreUtil.StoneStoreData GetStoneStoreDataFromProductId(string productId)
	{
		List<StoreUtil.StoneStoreData> list = this.GetStoneStoreDataList();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].productId == productId)
			{
				return list[i];
			}
		}
		return null;
	}

	public void ReConsumeNonConsumedItems(Action<bool> act = null)
	{
		this.actCallBackReConsume = act;
		global::Debug.Log("================================================= STORE RECONSUME!!");
		this.isFromStart = false;
		GUICollider.DisableAllCollider("StoreUtil");
		this.svRetryCt = 0;
		NpSingleton<NpPurchase>.Instance.RetryTransaction(new Action<NpPurchaseReceiptData>(this.PurchaseProductCallBackSccess), new Action<bool>(this.PurchaseProductCallBackNone));
	}

	private void PurchaseProductCallBackSccess(NpPurchaseReceiptData data)
	{
		this.reqList = new List<string>();
		this.reqList.Add(data.Signature);
		this.reqList.Add(data.Receipt);
		this._productId = data.ProductId;
		this.RequestPurchaseAndroid();
	}

	private void PurchaseProductCallBackNone(bool success)
	{
		global::Debug.Log("================================================= STORE PP_CALLBACK --> NONE !!");
		if (this.actCallBackReConsume != null)
		{
			this.actCallBackReConsume(success);
		}
		GUICollider.EnableAllCollider("StoreUtil");
	}

	private void PurchaseProductCallBackFail(string err)
	{
		global::Debug.Log("================================================= STORE PP_CALLBACK --> FAILED !!");
		GUICollider.EnableAllCollider("StoreUtil");
		this.ShowErrorDialog(err);
		Singleton<DebugLogScreen>.Instance.Print(err);
		int num = int.Parse(err);
		if (num == 405)
		{
			StoreInit.Instance().SetStatusToReconsume();
		}
	}

	private void RequestPurchaseAndroid()
	{
		global::Debug.Log("================================================= STORE PP_CALLBACK --> TO SV VERIFY !!");
		GameWebAPI.RequestSH_PurchaseAndroid requestSH_PurchaseAndroid = new GameWebAPI.RequestSH_PurchaseAndroid();
		requestSH_PurchaseAndroid.SetSendData = delegate(GameWebAPI.SH_Req_Verify_AND param)
		{
			param.productId = this._productId;
			param.osVersion = DataMng.Instance().RespDataCM_Login.playerInfo.osType;
			param.currencyCode = "JPY";
			param.priceNumber = this.GetPriceFromProductId(this._productId).ToString();
			param.signature = this.reqList[0];
			param.signedData = this.reqList[1];
		};
		requestSH_PurchaseAndroid.OnReceived = delegate(GameWebAPI.RespDataSH_ReqVerify response)
		{
			DataMng.Instance().RespDataSH_ReqVerify = response;
		};
		GameWebAPI.RequestSH_PurchaseAndroid request = requestSH_PurchaseAndroid;
		base.StartCoroutine(request.RunOneTime(new Action(this.VerifyReceiptSuccess), new Action<Exception>(this.VerifyReceiptFailed), null));
	}

	public void StartPurchaseItem(string productId, Action<bool> act = null)
	{
		this.actCallBackReConsume = act;
		this.svRetryCt = 0;
		GUICollider.DisableAllCollider("StoreUtil");
		NpSingleton<NpPurchase>.Instance.Purchase(productId, true, new Action<NpPurchaseReceiptData>(this.PurchaseProductCallBackSccess), new Action<string>(this.PurchaseProductCallBackFail));
		this.isFromStart = true;
		global::Debug.Log("================================================= STORE START PURCHASE !!");
	}

	private void VerifyReceiptSuccess()
	{
		GameWebAPI.RespDataSH_ReqVerify respDataSH_ReqVerify = DataMng.Instance().RespDataSH_ReqVerify;
		if (respDataSH_ReqVerify.status == 1 || respDataSH_ReqVerify.status == 2)
		{
			global::Debug.Log("================================================= STORE VERIFY SUCCEED!!");
			this.svRetryCt = 0;
			NpSingleton<NpPurchase>.Instance.SuccessPurchase(new Action(this.consumeProductCallBackSuccess), new Action<string>(this.consumeProductCallBackFailed));
		}
		else
		{
			this.VerifyReceiptFailed(null);
		}
	}

	private void VerifyReceiptFailed(Exception noop)
	{
		string noop2 = (noop == null) ? string.Empty : noop.Message;
		Singleton<DebugLogScreen>.Instance.Print(noop2);
		if (++this.svRetryCt < 3)
		{
			this.RequestPurchaseAndroid();
		}
		else
		{
			global::Debug.Log("================================================= STORE VERIFY FAIL!!");
			StoreInit.Instance().SetStatusToReconsume();
			if (this.actCallBackReConsume != null)
			{
				this.actCallBackReConsume(false);
			}
			GUICollider.EnableAllCollider("StoreUtil");
		}
	}

	private void consumeProductCallBackSuccess()
	{
		int numFromProductId = this.GetNumFromProductId(this._productId);
		if (Loading.IsShow())
		{
			RestrictionInput.EndLoad();
		}
		if (GUIMain.IsBarrierON())
		{
			GUIMain.BarrierOFF();
		}
		CMD_ModalMessage cmd_ModalMessage = (CMD_ModalMessage)GUIMain.ShowCommonDialog(delegate(int x)
		{
			if (this.actCallBackReConsume != null)
			{
				this.actCallBackReConsume(true);
			}
			if (Loading.IsShow())
			{
				Loading.ResumeDisplay();
			}
			if (GUIMain.IsBarrierON())
			{
				GUIMain.BarrierON(null);
			}
		}, "CMD_ModalMessage");
		StoreUtil.StoneStoreData stoneStoreDataFromProductId = this.GetStoneStoreDataFromProductId(this._productId);
		if (!this.isFromStart)
		{
			cmd_ModalMessage.Title = StringMaster.GetString("ShopRestoreTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("ShopRestoreInfo");
		}
		else
		{
			if (!stoneStoreDataFromProductId.packFlg)
			{
				cmd_ModalMessage.Title = StringMaster.GetString("ShopConfirmTitle");
			}
			else
			{
				cmd_ModalMessage.Title = stoneStoreDataFromProductId.productTitle;
			}
			cmd_ModalMessage.Info = string.Format(StringMaster.GetString("ShopCompleted"), numFromProductId.ToString());
		}
		if (this.isFromStart)
		{
			DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point += numFromProductId;
			GUIPlayerStatus.RefreshParams_S(false);
		}
		else
		{
			if (DataMng.Instance().RespDataUS_PlayerInfo != null)
			{
				DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point += numFromProductId;
			}
			GUIPlayerStatus.RefreshParams_S(false);
		}
		Partytrack.sendPayment(this._productId, 1, "JPY", (double)this.GetPriceFromProductId(this._productId));
		GUICollider.EnableAllCollider("StoreUtil");
	}

	private void consumeProductCallBackFailed(string err)
	{
		Singleton<DebugLogScreen>.Instance.Print(err);
		if (++this.svRetryCt >= 3)
		{
			int numFromProductId = this.GetNumFromProductId(this._productId);
			this.ShowErrorDialog(err);
			GUICollider.EnableAllCollider("StoreUtil");
			StoreInit.Instance().SetStatusToReconsume();
			return;
		}
		NpSingleton<NpPurchase>.Instance.SuccessPurchase(new Action(this.consumeProductCallBackSuccess), new Action<string>(this.consumeProductCallBackFailed));
	}

	private void ShowErrorDialog(string errorCode)
	{
		Singleton<DebugLogScreen>.Instance.Print(errorCode);
		if (Loading.IsShow())
		{
			Loading.Invisible();
		}
		if (GUIMain.IsBarrierON())
		{
			GUIMain.BarrierOFF();
		}
		AlertManager.ShowAlertDialog(delegate(int x)
		{
			if (Loading.IsShow())
			{
				Loading.ResumeDisplay();
			}
			if (GUIMain.IsBarrierON())
			{
				GUIMain.BarrierON(null);
			}
			if (this.actCallBackReConsume != null)
			{
				this.actCallBackReConsume(false);
			}
		}, errorCode);
	}

	public void InitStore(Action<bool> actCallBack)
	{
		string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvSUCi37AxjkuDVpgc77DP9Z2zDl7SFQTCPS8TMOWobpj4CrxqKA+cLP1bjEAwXB/uKC2v61J0XKh16X1Ql+A7Nm44YnKRtsB+eqjkoWxyqAQrlTGAmPmknlpCdZHLO9jFkbCstz3i9dlNMxlwXUDqwjmOzCz5PVk4shqU48LBT1rbo04cUueNlCu/YjseEffeDFVkvyv72bBDqYjQje54TD1fhQylCAJpI74fZC5F71PIZT2NgTJug1LzQ8t4EBJqhnru81MfdxOH61rlgAtDrqanfusj5dVo2vKa0A7prw7DySs3mliMzvJ/ZCFxg+bpqVI8VAja9f26pmJ1E/rLQIDAQAB";
		NpSingleton<NpPurchase>.Instance.Init(publicKey, actCallBack);
	}

	public void RequestProducts(string[] productIDs, Action<string> actCallBack)
	{
		this.actCallBack_RequestProducts = actCallBack;
		NpSingleton<NpPurchase>.Instance.RequestProducts(productIDs, new Action<string>(this.StoreInitCallBackSuccess), new Action<string>(this.StoreInitCallBackFailed));
	}

	private void StoreInitCallBackSuccess(string productInfo)
	{
		this.actCallBack_RequestProducts(string.Empty);
	}

	private void StoreInitCallBackFailed(string err)
	{
		Singleton<DebugLogScreen>.Instance.Print(err);
		if (string.IsNullOrEmpty(err))
		{
			this.actCallBack_RequestProducts(StringMaster.GetString("ShopInitializeErrorInfo"));
		}
		else
		{
			this.actCallBack_RequestProducts(err);
		}
	}

	public enum StoneSpriteType
	{
		NONE,
		STONE_SHOPLIST_1,
		STONE_SHOPLIST_2,
		STONE_SHOPLIST_3,
		STONE_SHOPLIST_4,
		STONE_SHOPLIST_5,
		STONE_SHOPLIST_6,
		STONE_SHOPLIST_7,
		STONE_SHOPLIST_8
	}

	public class StoneStoreData
	{
		public string productId;

		public int num;

		public int price;

		public string productTitle;

		public StoreUtil.StoneSpriteType spriteType;

		public string imgPath;

		public int priority;

		public int limitCount;

		public int purchasedCount;

		public bool countDownDispFlg;

		public bool packFlg;

		public string closeTime;

		public List<GameWebAPI.RespDataSH_Info.AcquireList> itemList;

		public List<GameWebAPI.RespDataSH_Info.AcquireList> omakeList;
	}
}
