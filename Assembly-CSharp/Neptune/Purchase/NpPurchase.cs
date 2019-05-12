using System;
using System.Collections.Generic;

namespace Neptune.Purchase
{
	public class NpPurchase : NpSingleton<NpPurchase>
	{
		private static readonly int REQUEST_PRODUCTS_NOMAL;

		private static readonly int REQUEST_PRODUCTS_COMPARE = 1;

		private NpPurchaseAuMarket mNpPurchaseAuMarket;

		private Action<bool> initFinishAction;

		private Action<string> purchaseFailedAction;

		private Action<NpPurchaseReceiptData> purchaseSuccessAction;

		private Action<bool> retryRequestFinishAction;

		private Action<string> restoreSuccessAction;

		private Action<string> restoreFailedAction;

		private Action<string> requestProductSuccessAction;

		private Action<string> requestProductFailedAction;

		private Action<string> comparisonSuccessAction;

		private Action<string> comparisonFailedAction;

		private Action mTransactionEndSuccessAction;

		private Action<string> mTransactionEndFailedAction;

		private Action mOnCatchNull;

		protected override void Constructor()
		{
		}

		public void Init(string publicKey, Action<bool> _initFinishAction)
		{
			this.initFinishAction = _initFinishAction;
			NpPurchaseAndroid.Init(publicKey, base.name);
		}

		public void InitCallBackRemove()
		{
			this.initFinishAction = null;
		}

		public void Purchase(string productId, bool isConsumable, Action<NpPurchaseReceiptData> _purchaseSuccessAction, Action<string> _purchaseFailedAction)
		{
			this.purchaseSuccessAction = _purchaseSuccessAction;
			this.purchaseFailedAction = _purchaseFailedAction;
			NpPurchaseAndroid.purchase(productId, isConsumable);
		}

		public void Purchase(string productId, Action<NpPurchaseReceiptData> _purchaseSuccessAction, Action<string> _purchaseFailedAction)
		{
			this.Purchase(productId, true, _purchaseSuccessAction, _purchaseFailedAction);
		}

		public void SuccessPurchase(Action transactionEndSuccessAction, Action<string> transactionEndFailedAction)
		{
			this.mTransactionEndSuccessAction = transactionEndSuccessAction;
			this.mTransactionEndFailedAction = transactionEndFailedAction;
			NpPurchaseAndroid.SuccessPurchase();
		}

		public void RetryTransaction(Action<NpPurchaseReceiptData> _purchaseSuccessAction, Action<bool> _retryRequestFinishAction)
		{
			this.purchaseSuccessAction = _purchaseSuccessAction;
			this.retryRequestFinishAction = _retryRequestFinishAction;
			NpPurchaseAndroid.RetryConsume();
		}

		public void RestoreStart(Action<string> _restoreSuccessAction, Action<string> _restoreFailedAction)
		{
			this.restoreSuccessAction = _restoreSuccessAction;
			this.restoreFailedAction = _restoreFailedAction;
			NpPurchaseAndroid.RestoreStart();
		}

		public bool GetIsPurchaseSupport()
		{
			return NpPurchaseAndroid.GetIsPurchaseSupport();
		}

		public void RequestProducts(string[] productId, Action<string> _requestProductSuccessAction, Action<string> _requestProductFailedAction)
		{
			this.requestProductSuccessAction = _requestProductSuccessAction;
			this.requestProductFailedAction = _requestProductFailedAction;
			int num = productId.Length;
			for (int i = 0; i < num; i++)
			{
				string requestProduct = productId[i];
				this.SetRequestProduct(requestProduct);
			}
			NpPurchaseAndroid.RequestProducts(NpPurchase.REQUEST_PRODUCTS_NOMAL);
		}

		public void ComparisonRequest(Dictionary<string, string> items, Action<string> _comparisonSuccessAction, Action<string> _comparisonFailedAction)
		{
			this.comparisonSuccessAction = _comparisonSuccessAction;
			this.comparisonFailedAction = _comparisonFailedAction;
			foreach (KeyValuePair<string, string> keyValuePair in items)
			{
				this.SetRequestProduct(keyValuePair.Key);
				this.SetRequestProductPrice(keyValuePair.Key + "," + keyValuePair.Value);
			}
			NpPurchaseAndroid.RequestProducts(NpPurchase.REQUEST_PRODUCTS_COMPARE);
		}

		public void AllConsumption()
		{
			NpPurchaseAndroid.AllConsumption();
		}

		public void SetCatchNullListener(Action onCatchNull)
		{
			this.mOnCatchNull = onCatchNull;
		}

		private void RetryTransactionStartUp()
		{
			NpPurchaseAndroid.RetryTransactionStartUp();
		}

		private void SetRequestProductPrice(string productPrice)
		{
			NpPurchaseAndroid.SetComparisonProduct(productPrice);
		}

		private void SetRequestProduct(string productId)
		{
			NpPurchaseAndroid.SetRequestProduct(productId);
		}

		public void initFinish(string boolst)
		{
			bool obj = boolst.Equals("true");
			if (this.initFinishAction != null)
			{
				this.initFinishAction(obj);
			}
		}

		public void postReceiptData(string nullData)
		{
			NpPurchaseReceiptData obj = new NpPurchaseReceiptData(NpPurchaseAndroid.GetProductId(), NpPurchaseAndroid.GetReceipt(), NpPurchaseAndroid.GetSignature(), string.Empty);
			if (this.purchaseSuccessAction != null)
			{
				this.purchaseSuccessAction(obj);
			}
		}

		public void purchaseFailed(string erroCode)
		{
			if (this.purchaseFailedAction != null)
			{
				this.purchaseFailedAction(erroCode);
			}
		}

		public void TransactionEndSuccess()
		{
			if (this.mTransactionEndSuccessAction != null)
			{
				this.mTransactionEndSuccessAction();
			}
		}

		public void TransactionEndFailed(string errMsg)
		{
			if (this.mTransactionEndFailedAction != null)
			{
				this.mTransactionEndFailedAction(errMsg);
			}
		}

		public void retryRequestFinish(string statusCode)
		{
			bool obj = false;
			if (string.IsNullOrEmpty(statusCode) && this.retryRequestFinishAction != null)
			{
				this.retryRequestFinishAction(obj);
				return;
			}
			int num = int.Parse(statusCode);
			if (num == 201)
			{
				obj = true;
			}
			if (this.retryRequestFinishAction != null)
			{
				this.retryRequestFinishAction(obj);
			}
		}

		public void restoreFailed(string errorCode)
		{
			if (this.restoreFailedAction != null)
			{
				this.restoreFailedAction(errorCode);
			}
		}

		public void restoreSuccess(string successCode)
		{
			if (this.restoreSuccessAction != null)
			{
				this.restoreSuccessAction(successCode);
			}
		}

		public void requestProductSuccess(string jsonData)
		{
			if (this.requestProductSuccessAction != null)
			{
				this.requestProductSuccessAction(jsonData);
			}
		}

		public void requestProductFailed(string errorCode)
		{
			if (this.requestProductFailedAction != null)
			{
				this.requestProductFailedAction(errorCode);
			}
		}

		public void comparisonFailed(string errorCode)
		{
			if (this.comparisonFailedAction != null)
			{
				this.comparisonFailedAction(errorCode);
			}
		}

		public void comparisonSuccess(string jsonData)
		{
			if (this.comparisonSuccessAction != null)
			{
				this.comparisonSuccessAction(jsonData);
			}
		}

		private void OnCatchNull()
		{
			Debug.Log("Unity OnCatchNull Call");
			if (this.mOnCatchNull != null)
			{
				this.mOnCatchNull();
			}
		}

		public enum NpPurchaseStatusCodeKind
		{
			PurchaseSuccess = 100,
			NoItem = 201,
			NotAllowed = 400,
			IncorrectId,
			PurchaseCancel,
			AppleServerError,
			NotFinishedTransaction = 405,
			JsonError,
			ItemAlreadyOwned,
			TransactionEndFailed,
			ErrorOther = 443
		}
	}
}
