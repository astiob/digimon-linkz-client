using System;
using System.Collections.Generic;

namespace FacebookGames
{
	public class PayResponse : PipePacketResponse
	{
		public string PaymentId { get; set; }

		public string Amount { get; set; }

		public string Currency { get; set; }

		public string Quantity { get; set; }

		public string RequestId { get; set; }

		public string Status { get; set; }

		public string SignedRequest { get; set; }

		public PayResponse()
		{
		}

		public PayResponse(string paymentId, string amount, string currency, string quantity, string requestId, string status, string signedRequest, string error = null, bool cancelled = false) : base(error, cancelled)
		{
			this.PaymentId = paymentId;
			this.Amount = amount;
			this.Currency = currency;
			this.Quantity = quantity;
			this.RequestId = requestId;
			this.Status = status;
			this.SignedRequest = signedRequest;
		}

		public override IDictionary<string, object> ToDictionary()
		{
			IDictionary<string, object> dictionary = base.ToDictionary();
			dictionary.Add("payment_id", this.PaymentId);
			dictionary.Add("amount", this.Amount);
			dictionary.Add("currency", this.Currency);
			dictionary.Add("quantity", this.Quantity);
			dictionary.Add("request_id", this.RequestId);
			dictionary.Add("status", this.Status);
			dictionary.Add("signed_request", this.SignedRequest);
			return dictionary;
		}
	}
}
