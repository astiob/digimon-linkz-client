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
}
