using System;
using UnityEngine;

public class AuMarketHelper
{
	private AndroidJavaObject auMarketHelper;

	public void Init(string name)
	{
		this.auMarketHelper = new AndroidJavaObject("com.kddi.aumarkethelper.unitywrapper.AuMarketHelper", new object[0]);
		this.auMarketHelper.Call("Init", new object[]
		{
			name
		});
	}

	public void Bind()
	{
		if (this.auMarketHelper == null)
		{
			global::Debug.Log("auMarketHelper is null");
			return;
		}
		this.auMarketHelper.Call("Bind", new object[0]);
	}

	public void Unbind()
	{
		if (this.auMarketHelper == null)
		{
			global::Debug.Log("auMarketHelper is null");
			return;
		}
		this.auMarketHelper.Call("Unbind", new object[0]);
	}

	public void ConfirmReceipt(string itemId)
	{
		if (this.auMarketHelper == null)
		{
			global::Debug.Log("auMarketHelper is null");
			return;
		}
		this.auMarketHelper.Call("confirmReceipt", new object[]
		{
			itemId
		});
	}

	public void IssueReceipt(string itemId)
	{
		if (this.auMarketHelper == null)
		{
			global::Debug.Log("auMarketHelper is null");
			return;
		}
		this.auMarketHelper.Call("issueReceipt", new object[]
		{
			itemId
		});
	}

	public void InvalidateItem(string itemId)
	{
		if (this.auMarketHelper == null)
		{
			global::Debug.Log("auMarketHelper is null");
			return;
		}
		this.auMarketHelper.Call("invalidateItem", new object[]
		{
			itemId
		});
	}
}
