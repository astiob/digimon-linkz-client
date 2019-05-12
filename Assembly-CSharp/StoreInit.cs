using System;
using System.Collections;
using UnityEngine;

public class StoreInit : MonoBehaviour
{
	private static StoreInit instance;

	private StoreInit.STATUS init_status;

	private bool getProductsSucceed;

	public static StoreInit Instance()
	{
		return StoreInit.instance;
	}

	protected virtual void Awake()
	{
		StoreInit.instance = this;
	}

	protected virtual void OnDestroy()
	{
		StoreInit.instance = null;
	}

	public StoreInit.STATUS GetStatus()
	{
		return this.init_status;
	}

	public void SetStatusToReconsume()
	{
		this.init_status = StoreInit.STATUS.DONE_REQUEST_PRODUCT;
	}

	public void SetStatusToDoneInit()
	{
		this.init_status = StoreInit.STATUS.DONE_INIT;
	}

	public IEnumerator InitStore()
	{
		if (this.init_status > StoreInit.STATUS.DONE_NOTHING)
		{
			yield break;
		}
		bool result = false;
		bool initialized = false;
		StoreUtil.Instance().InitStore(delegate(bool r)
		{
			result = r;
			initialized = true;
		});
		while (!initialized)
		{
			yield return null;
		}
		if (!result)
		{
			bool isShow = Loading.IsShow();
			bool isBarrier = GUIMain.IsBarrierON();
			if (isShow)
			{
				Loading.Invisible();
			}
			if (isBarrier)
			{
				GUIMain.BarrierOFF();
			}
			bool isClosed = false;
			AlertManager.ShowAlertDialog(delegate(int i)
			{
				isClosed = true;
			}, "C-NP200");
			while (!isClosed)
			{
				yield return null;
			}
			if (isShow)
			{
				Loading.ResumeDisplay();
			}
			if (isBarrier)
			{
				GUIMain.BarrierON(null);
			}
		}
		else
		{
			this.init_status = StoreInit.STATUS.DONE_INIT;
		}
		global::Debug.Log("================================================= STORE INIT isSuccess --> " + result);
		yield break;
	}

	private TaskBase GetProductsID(Action<string[]> onCompleted)
	{
		if (this.init_status == StoreInit.STATUS.DONE_NOTHING)
		{
			return new NormalTask();
		}
		string[] productIDS = new string[]
		{
			"digi_stone_006",
			"digi_stone_024",
			"digi_stone_060",
			"digi_stone_160",
			"digi_stone_240",
			"digi_stone_480",
			"2_digistone_6_1_120",
			"2_digistone_24_1_480",
			"2_digistone_60_1_1200",
			"2_digistone_150_1_3000",
			"2_digistone_240_1_4800",
			"2_digistone_490_1_9800"
		};
		GameWebAPI.RequestSH_ShopList requestSH_ShopList = new GameWebAPI.RequestSH_ShopList();
		requestSH_ShopList.SetSendData = delegate(GameWebAPI.SendDataSH_ShopList requestParam)
		{
			int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
			requestParam.countryCode = countryCode;
		};
		requestSH_ShopList.OnReceived = delegate(GameWebAPI.RespDataSH_Info response)
		{
			DataMng.Instance().RespDataSH_Info = response;
			if (response.shopList != null && response.shopList.Length > 0)
			{
				GameWebAPI.RespDataSH_Info.ProductList[] productList = response.shopList[0].productList;
				if (productList.Length > 0)
				{
					productIDS = new string[productList.Length];
					for (int i = 0; i < productList.Length; i++)
					{
						productIDS[i] = productList[i].productId;
					}
				}
			}
			else
			{
				productIDS = new string[0];
				DataMng.Instance().RespDataSH_Info.shopList = new GameWebAPI.RespDataSH_Info.ShopList[]
				{
					new GameWebAPI.RespDataSH_Info.ShopList()
				};
				DataMng.Instance().RespDataSH_Info.shopList[0].productList = new GameWebAPI.RespDataSH_Info.ProductList[0];
				this.init_status = StoreInit.STATUS.DONE_RECONSUME;
			}
			onCompleted(productIDS);
		};
		GameWebAPI.RequestSH_ShopList request = requestSH_ShopList;
		return new APIRequestTask(request, true);
	}

	private IEnumerator GetProducts(string[] productIDS, bool stateChange)
	{
		string err = "err";
		bool isFinished = false;
		StoreUtil.Instance().RequestProducts(productIDS, delegate(string result)
		{
			err = result;
			isFinished = true;
		});
		while (!isFinished)
		{
			yield return null;
		}
		if (err != string.Empty)
		{
			bool isShow = Loading.IsShow();
			bool isBarrier = GUIMain.IsBarrierON();
			if (isShow)
			{
				Loading.Invisible();
			}
			if (isBarrier)
			{
				GUIMain.BarrierOFF();
			}
			bool isClosed = false;
			AlertManager.ShowAlertDialog(delegate(int i)
			{
				isClosed = true;
			}, AlertManager.GetNeptuneErrorCode(err));
			while (!isClosed)
			{
				yield return null;
			}
			if (isShow)
			{
				Loading.ResumeDisplay();
			}
			if (isBarrier)
			{
				GUIMain.BarrierON(null);
			}
			this.getProductsSucceed = false;
		}
		else
		{
			if (stateChange)
			{
				this.init_status = StoreInit.STATUS.DONE_REQUEST_PRODUCT;
			}
			this.getProductsSucceed = true;
		}
		yield break;
	}

	private IEnumerator ReConsume()
	{
		if (this.init_status != StoreInit.STATUS.DONE_REQUEST_PRODUCT)
		{
			yield break;
		}
		bool isSuccess = false;
		bool isFinished = false;
		StoreUtil.Instance().ReConsumeNonConsumedItems(delegate(bool result)
		{
			isFinished = true;
			isSuccess = result;
		});
		while (!isFinished)
		{
			yield return null;
		}
		global::Debug.Log("================================================= STORE ReConsume isSuccess --> " + isSuccess);
		if (!isSuccess)
		{
			bool isShow = Loading.IsShow();
			bool isBarrier = GUIMain.IsBarrierON();
			if (isShow)
			{
				Loading.Invisible();
			}
			if (isBarrier)
			{
				GUIMain.BarrierOFF();
			}
			bool isClosed = false;
			AlertManager.ShowAlertDialog(delegate(int i)
			{
				isClosed = true;
			}, "C-SH02");
			while (!isClosed)
			{
				yield return null;
			}
			if (isShow)
			{
				Loading.ResumeDisplay();
			}
			if (isBarrier)
			{
				GUIMain.BarrierON(null);
			}
		}
		else
		{
			this.init_status = StoreInit.STATUS.DONE_RECONSUME;
		}
		yield break;
	}

	public IEnumerator InitRestoreOperation()
	{
		string[] productsID = null;
		TaskBase productsID2 = StoreInit.Instance().GetProductsID(delegate(string[] IDs)
		{
			productsID = IDs;
		});
		productsID2.Add(new NormalTask(delegate()
		{
			if (productsID != null)
			{
				return StoreInit.Instance().GetProducts(productsID, true);
			}
			return null;
		})).Add(new NormalTask(StoreInit.Instance().ReConsume()));
		return productsID2.Run(null, null, null);
	}

	public bool IsSuccessReceiveProducts()
	{
		return this.getProductsSucceed;
	}

	public IEnumerator GetProductsOperation()
	{
		string[] productsID = null;
		TaskBase productsID2 = StoreInit.Instance().GetProductsID(delegate(string[] IDs)
		{
			productsID = IDs;
		});
		productsID2.Add(new NormalTask(delegate()
		{
			if (productsID != null)
			{
				return StoreInit.Instance().GetProducts(productsID, false);
			}
			return StoreInit.Instance().GetProducts(productsID, false);
		}));
		return productsID2.Run(null, delegate(Exception nop)
		{
			this.getProductsSucceed = false;
		}, null);
	}

	public enum STATUS
	{
		DONE_NOTHING,
		DONE_INIT,
		DONE_REQUEST_PRODUCT,
		DONE_RECONSUME
	}
}
