using System;
using System.Collections.Generic;
using UnityEngine.Cloud.Service;

namespace UnityEngine.Analytics
{
	internal class TransactionEvent : AnalyticsEvent
	{
		public const string kEventTransaction = "transaction";

		public const string kTransactionReceipt = "receipt";

		public const string kTransactionReceiptData = "data";

		public const string kTransactionSignature = "signature";

		public const string kTransactionAmount = "amount";

		public const string kTransactionCurrency = "currency";

		public const string kTransactionId = "transactionid";

		public const string kTransactionProductId = "productid";

		public TransactionEvent(string productId, decimal amount, string currency, string receiptPurchaseData, string signature, long transactionId) : base("transaction", CloudEventFlags.HighPriority)
		{
			if (receiptPurchaseData != null || signature != null)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				if (receiptPurchaseData != null)
				{
					dictionary.Add("data", receiptPurchaseData);
				}
				if (signature != null)
				{
					dictionary.Add("signature", signature);
				}
				base.SetParameter("receipt", dictionary);
			}
			base.SetParameter("productid", productId);
			base.SetParameter("amount", amount);
			base.SetParameter("currency", currency);
			base.SetParameter("transactionid", transactionId);
		}
	}
}
