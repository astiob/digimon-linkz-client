using System;

namespace FacebookGames
{
	public class PayRequest : PipePacketRequest
	{
		public string Method { get; set; }

		public string Action { get; set; }

		public string Product { get; set; }

		public string ProductId { get; set; }

		public string Quantity { get; set; }

		public string QuantityMin { get; set; }

		public string QuantityMax { get; set; }

		public string RequestId { get; set; }

		public string PricepointId { get; set; }

		public string TestCurrency { get; set; }

		public PayRequest()
		{
		}

		public PayRequest(string appId, string method, string action, string product, string productId, string quantity, string quantityMin, string quantityMax, string requestId, string pricepointId, string testCurrency) : base(appId)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (action == "purchaseitem" && product == null)
			{
				throw new ArgumentNullException("product");
			}
			if (action == "purchaseiap" && productId == null)
			{
				throw new ArgumentNullException("productId");
			}
			this.Method = method;
			this.Action = action;
			this.Product = product;
			this.ProductId = productId;
			this.Quantity = quantity;
			this.QuantityMin = quantityMin;
			this.QuantityMax = quantityMax;
			this.RequestId = requestId;
			this.PricepointId = pricepointId;
			this.TestCurrency = testCurrency;
		}
	}
}
