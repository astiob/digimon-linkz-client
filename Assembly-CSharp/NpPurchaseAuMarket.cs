using System;

public class NpPurchaseAuMarket
{
	private bool mIsDebug;

	private string mItemId = string.Empty;

	private Action<NpAuMarketReceiptData> mPurchaseSuccessCB;

	private Action<string> mPurchaseFailureCB;

	private Action mTransactionFinishSuccessCB;

	private Action<string> mTransactionFinishFailureCB;

	private Action<bool, NpAuMarketReceiptData> mRetryTransactionSuccessCB;

	private Action<string> mRetryTransactionFailureCB;

	public void SetDebug(bool isDebug)
	{
		this.mIsDebug = isDebug;
	}

	private void DebugLog(string msg)
	{
		if (this.mIsDebug)
		{
			Debug.Log(msg);
		}
	}

	public void Init(Action successCB)
	{
		NpSingleton<NpAuMarketHelper>.Instance.Init(delegate
		{
			this.DebugLog("#=#=# init success.");
			successCB();
		});
	}

	public void Purchase(string itemId, Action<NpAuMarketReceiptData> successCB, Action<string> failureCB)
	{
		this.mItemId = itemId;
		this.mPurchaseSuccessCB = successCB;
		this.mPurchaseFailureCB = failureCB;
		NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperBind(delegate
		{
			this.DebugLog("#=#=# Purchase : BindSuccess");
			NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperConfirmReceipt(this.mItemId, new Action<bool, NpAuMarketReceiptData>(this.PurchaseConfirmReceiptSuccessCB), new Action<string>(this.PurchaseConfirmReceiptFailureCB));
		}, delegate(string resultCode)
		{
			this.DebugLog("#=#=# Purchase : BindFailure");
			if (this.mPurchaseFailureCB != null)
			{
				this.mPurchaseFailureCB(resultCode);
			}
		});
	}

	private void PurchaseConfirmReceiptSuccessCB(bool isReceiptExist, NpAuMarketReceiptData receiptData)
	{
		this.DebugLog("#=#=# Purchase : ConfirmReceiptSuccess");
		if (isReceiptExist)
		{
			this.mPurchaseFailureCB("104");
		}
		else
		{
			NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperIssueReceipt(this.mItemId, new Action<NpAuMarketReceiptData>(this.PurchaseIssueReceiptSuccessCB), new Action<string>(this.PurchaseIssueReceiptFailureCB));
		}
	}

	private void PurchaseConfirmReceiptFailureCB(string resultCode)
	{
		this.DebugLog("#=#=# Purchase : ConfirmReceiptFailure resultCode = " + resultCode);
		NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperUnbind();
		if (this.mPurchaseFailureCB != null)
		{
			this.mPurchaseFailureCB(resultCode);
		}
	}

	private void PurchaseIssueReceiptSuccessCB(NpAuMarketReceiptData receiptData)
	{
		this.DebugLog("#=#=# Purchase : IssueReceiptSuccess itemId = " + receiptData.ItemId);
		this.DebugLog("#=#=# Purchase : IssueReceiptSuccess receipt = " + receiptData.Receipt);
		this.DebugLog("#=#=# Purchase : IssueReceiptSuccess signature = " + receiptData.Signature);
		NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperUnbind();
		if (this.mPurchaseSuccessCB != null)
		{
			this.mPurchaseSuccessCB(receiptData);
		}
	}

	private void PurchaseIssueReceiptFailureCB(string resultCode)
	{
		this.DebugLog("#=#=# Purchase : IssueReceiptFailure resultCode = " + resultCode);
		NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperUnbind();
		if (this.mPurchaseFailureCB != null)
		{
			this.mPurchaseFailureCB(resultCode);
		}
	}

	public void SuccessPurchase(Action successCB, Action<string> failureCB)
	{
		this.mTransactionFinishSuccessCB = successCB;
		this.mTransactionFinishFailureCB = failureCB;
		NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperBind(delegate
		{
			this.DebugLog("#=#=# SuccessPurchase : BindSuccess.");
			NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperConfirmReceipt(string.Empty, new Action<bool, NpAuMarketReceiptData>(this.SuccessPurchaseConfirmReceiptSuccessCB), new Action<string>(this.SuccessPurchaseConfirmReceiptFailureCB));
		}, delegate(string resultCode)
		{
			this.DebugLog("#=#=# SuccessPurchase : BindFailure.");
			if (this.mTransactionFinishFailureCB != null)
			{
				this.mTransactionFinishFailureCB(resultCode);
			}
		});
	}

	private void SuccessPurchaseConfirmReceiptSuccessCB(bool isReceiptExist, NpAuMarketReceiptData receiptData)
	{
		this.DebugLog("#=#=# SuccessPurchase : ConfirmReceiptSuccess itemId = " + receiptData.ItemId);
		if (!isReceiptExist)
		{
			if (this.mTransactionFinishSuccessCB != null)
			{
				this.mTransactionFinishSuccessCB();
			}
			return;
		}
		NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperInvalidateItem(receiptData.ItemId, new Action(this.SuccessPurchaseInvalidateItemSuccessCB), new Action<string>(this.SuccessPurchaseInvalidateItemFailureCB));
	}

	private void SuccessPurchaseConfirmReceiptFailureCB(string resultCode)
	{
		this.DebugLog("#=#=# SuccessPurchase : ConfirmReceiptFailure resultCode = " + resultCode);
		NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperUnbind();
		if (this.mTransactionFinishFailureCB != null)
		{
			this.mTransactionFinishFailureCB(resultCode);
		}
	}

	private void SuccessPurchaseInvalidateItemSuccessCB()
	{
		this.DebugLog("#=#=# SuccessPurchase : InvalidateItemSuccess.");
		NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperUnbind();
		if (this.mTransactionFinishSuccessCB != null)
		{
			this.mTransactionFinishSuccessCB();
		}
	}

	private void SuccessPurchaseInvalidateItemFailureCB(string resultCode)
	{
		this.DebugLog("#=#=# SuccessPurchase : InvalidateItemFailure resultCode = " + resultCode);
		NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperUnbind();
		if (this.mTransactionFinishFailureCB != null)
		{
			this.mTransactionFinishFailureCB(resultCode);
		}
	}

	public void RetryTransaction(Action<bool, NpAuMarketReceiptData> successCB, Action<string> failureCB)
	{
		this.mRetryTransactionSuccessCB = successCB;
		this.mRetryTransactionFailureCB = failureCB;
		NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperBind(delegate
		{
			this.DebugLog("#=#=# RetryTransaction : BindSuccess.");
			NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperConfirmReceipt(string.Empty, new Action<bool, NpAuMarketReceiptData>(this.RetryConfirmReceiptSuccessCB), new Action<string>(this.RetryConfirmReceiptFailureCB));
		}, delegate(string resultCode)
		{
			this.DebugLog("#=#=# RetryTransaction : BindFailure.");
			if (this.mRetryTransactionFailureCB != null)
			{
				this.mRetryTransactionFailureCB(resultCode);
			}
		});
	}

	private void RetryConfirmReceiptSuccessCB(bool isReceiptExist, NpAuMarketReceiptData receiptData)
	{
		this.DebugLog("#=#=# RetryTransaction : ConfirmReceiptSuccess isReceiptExist = " + isReceiptExist);
		this.DebugLog("#=#=# RetryTransaction : ConfirmReceiptSuccess itemId = " + receiptData.ItemId);
		this.DebugLog("#=#=# RetryTransaction : ConfirmReceiptSuccess receipt = " + receiptData.Receipt);
		this.DebugLog("#=#=# RetryTransaction : ConfirmReceiptSuccess signature = " + receiptData.Signature);
		NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperUnbind();
		if (this.mRetryTransactionSuccessCB != null)
		{
			this.mRetryTransactionSuccessCB(isReceiptExist, receiptData);
		}
	}

	private void RetryConfirmReceiptFailureCB(string resultCode)
	{
		this.DebugLog("#=#=# RetryTransaction : ConfirmReceiptFailure resultCode = " + resultCode);
		NpSingleton<NpAuMarketHelper>.Instance.CallAuMarketHelperUnbind();
		if (this.mRetryTransactionFailureCB != null)
		{
			this.mRetryTransactionFailureCB(resultCode);
		}
	}
}
