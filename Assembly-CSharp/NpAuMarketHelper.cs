using JsonFx.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class NpAuMarketHelper : NpSingleton<NpAuMarketHelper>
{
	private AuMarketHelper mAuMarketHelper;

	private Action mInitSuccessCB;

	private Action mBindSuccessCB;

	private Action<string> mBindFailureCB;

	private Action<bool, NpAuMarketReceiptData> mConfirmReceiptSuccessCB;

	private Action<string> mConfirmReceiptFailureCB;

	private Action<NpAuMarketReceiptData> mIssueReceiptSuccessCB;

	private Action<string> mIssueReceiptFailureCB;

	private Action mInvalidateItemSuccessCB;

	private Action<string> mInvalidateItemFailureCB;

	public void Init(Action successCB)
	{
		this.mInitSuccessCB = successCB;
		if (this.mAuMarketHelper == null)
		{
			this.mAuMarketHelper = new AuMarketHelper();
			this.mAuMarketHelper.Init(base.gameObject.name);
		}
		else
		{
			this.mInitSuccessCB();
		}
	}

	public void CallAuMarketHelperBind(Action successCB, Action<string> failureCB)
	{
		this.mBindSuccessCB = successCB;
		this.mBindFailureCB = failureCB;
		this.mAuMarketHelper.Bind();
	}

	public void CallAuMarketHelperUnbind()
	{
		this.mAuMarketHelper.Unbind();
		Debug.Log("#=#=# CallAuMarketHelperUnbind Unbind");
	}

	public void CallAuMarketHelperConfirmReceipt(string itemId, Action<bool, NpAuMarketReceiptData> successCB, Action<string> failureCB)
	{
		this.mConfirmReceiptSuccessCB = successCB;
		this.mConfirmReceiptFailureCB = failureCB;
		this.mAuMarketHelper.ConfirmReceipt(itemId);
	}

	public void CallAuMarketHelperIssueReceipt(string itemId, Action<NpAuMarketReceiptData> successCB, Action<string> failureCB)
	{
		this.mIssueReceiptSuccessCB = successCB;
		this.mIssueReceiptFailureCB = failureCB;
		this.mAuMarketHelper.IssueReceipt(itemId);
	}

	public void CallAuMarketHelperInvalidateItem(string itemId, Action successCB, Action<string> failureCB)
	{
		this.mInvalidateItemSuccessCB = successCB;
		this.mInvalidateItemFailureCB = failureCB;
		this.mAuMarketHelper.InvalidateItem(itemId);
	}

	public void CallbackInit()
	{
		if (this.mInitSuccessCB != null)
		{
			this.mInitSuccessCB();
		}
	}

	public void CallbackBind(string message)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary = JsonReader.Deserialize<Dictionary<string, object>>(message);
		int num = (int)dictionary["resultCode"];
		if (num == 0)
		{
			if (this.mBindSuccessCB != null)
			{
				this.mBindSuccessCB();
			}
		}
		else if (this.mBindFailureCB != null)
		{
			this.mBindFailureCB(num.ToString());
		}
	}

	public void CallbackConfirmReceipt(string message)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary = JsonReader.Deserialize<Dictionary<string, object>>(message);
		int num = (int)dictionary["resultCode"];
		if (num == 0)
		{
			if (this.mConfirmReceiptSuccessCB != null)
			{
				string text = dictionary["receipt"] as string;
				string signature = dictionary["signature"] as string;
				bool arg = true;
				string itemId = this.GetItemId(text);
				NpAuMarketReceiptData arg2 = new NpAuMarketReceiptData(itemId, text, signature);
				if (this.mConfirmReceiptSuccessCB != null)
				{
					this.mConfirmReceiptSuccessCB(arg, arg2);
				}
			}
		}
		else if (num == -24)
		{
			if (this.mConfirmReceiptSuccessCB != null)
			{
				bool arg3 = false;
				NpAuMarketReceiptData arg4 = new NpAuMarketReceiptData();
				this.mConfirmReceiptSuccessCB(arg3, arg4);
			}
		}
		else if (this.mConfirmReceiptFailureCB != null)
		{
			this.mConfirmReceiptFailureCB(this.AuErrToNpErrConversion(num));
		}
	}

	public void CallbackIssueReceipt(string message)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary = JsonReader.Deserialize<Dictionary<string, object>>(message);
		int num = (int)dictionary["resultCode"];
		if (num == 0)
		{
			if (this.mIssueReceiptSuccessCB != null)
			{
				string text = dictionary["receipt"] as string;
				string signature = dictionary["signature"] as string;
				string itemId = this.GetItemId(text);
				NpAuMarketReceiptData obj = new NpAuMarketReceiptData(itemId, text, signature);
				this.mIssueReceiptSuccessCB(obj);
			}
		}
		else if (this.mIssueReceiptFailureCB != null)
		{
			this.mIssueReceiptFailureCB(this.AuErrToNpErrConversion(num));
		}
	}

	public void CallbackInvalidateItem(string message)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary = JsonReader.Deserialize<Dictionary<string, object>>(message);
		int num = (int)dictionary["resultCode"];
		if (num == 0)
		{
			if (this.mInvalidateItemSuccessCB != null)
			{
				this.mInvalidateItemSuccessCB();
			}
		}
		else if (this.mInvalidateItemFailureCB != null)
		{
			this.mInvalidateItemFailureCB(this.AuErrToNpErrConversion(num));
		}
	}

	private string GetItemId(string receiptXml)
	{
		if (string.IsNullOrEmpty(receiptXml))
		{
			return string.Empty;
		}
		string result = string.Empty;
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.Load(new StringReader(receiptXml));
			XmlElement documentElement = xmlDocument.DocumentElement;
			if (!documentElement.HasChildNodes)
			{
				return string.Empty;
			}
			XmlNode firstChild = documentElement.FirstChild;
			if (!firstChild.HasChildNodes)
			{
				return string.Empty;
			}
			for (int i = 0; i < firstChild.ChildNodes.Count; i++)
			{
				XmlNode xmlNode = firstChild.ChildNodes[i];
				if (xmlNode.HasChildNodes)
				{
					for (int j = 0; j < xmlNode.ChildNodes.Count; j++)
					{
						XmlNode xmlNode2 = xmlNode.ChildNodes[j];
						if (xmlNode2.HasChildNodes && xmlNode2.Name.Equals("item_id"))
						{
							result = xmlNode2.FirstChild.Value;
							return result;
						}
					}
				}
			}
		}
		catch (XmlException ex)
		{
			Debug.Log(ex.ToString());
		}
		return result;
	}

	private string AuErrToNpErrConversion(int resultCode)
	{
		string result = resultCode.ToString();
		if (resultCode == -20)
		{
			result = "101";
		}
		else if (resultCode == -21)
		{
			result = "104";
		}
		else if (resultCode == -24)
		{
			result = "100";
		}
		return result;
	}
}
