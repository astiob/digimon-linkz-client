using System;

public class NpAuMarketReceiptData
{
	private string itemId = string.Empty;

	private string receipt = string.Empty;

	private string signature = string.Empty;

	public NpAuMarketReceiptData()
	{
	}

	public NpAuMarketReceiptData(string _itemId, string _receipt, string _signature)
	{
		this.ItemId = _itemId;
		this.Receipt = _receipt;
		this.Signature = _signature;
	}

	public string ItemId
	{
		get
		{
			return this.itemId;
		}
		set
		{
			this.itemId = value;
		}
	}

	public string Receipt
	{
		get
		{
			return this.receipt;
		}
		set
		{
			this.receipt = value;
		}
	}

	public string Signature
	{
		get
		{
			return this.signature;
		}
		set
		{
			this.signature = value;
		}
	}
}
