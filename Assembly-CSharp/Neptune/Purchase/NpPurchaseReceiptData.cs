using System;

namespace Neptune.Purchase
{
	public class NpPurchaseReceiptData
	{
		public NpPurchaseReceiptData(string productId, string receipt, string signature, string transactionIdentifier)
		{
			this.ProductId = productId;
			this.Receipt = receipt;
			this.Signature = signature;
			this.TransactionIdentifier = transactionIdentifier;
		}

		public string ProductId { get; set; }

		public string Receipt { get; set; }

		public string Signature { get; set; }

		public string TransactionIdentifier { get; set; }

		public void PrintLog()
		{
			Debug.Log(string.Format("NpPurchaseReceiptData -> ProductId={0}, Receipt={1}, Signature={2}", this.ProductId, this.Receipt, this.Signature));
		}
	}
}
