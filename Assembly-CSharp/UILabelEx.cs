using Master;
using System;
using UnityEngine;

public class UILabelEx : UILabel
{
	[SerializeField]
	private string stringMasterKey;

	private string masterString;

	protected override void Awake()
	{
		base.Awake();
		this.masterString = StringMaster.GetString(this.stringMasterKey);
		base.text = this.masterString;
	}

	public string GetMasterString()
	{
		return this.masterString;
	}

	public void SetStringKeyText(string key)
	{
		this.stringMasterKey = key;
		this.masterString = StringMaster.GetString(key);
		base.text = this.masterString;
	}
}
