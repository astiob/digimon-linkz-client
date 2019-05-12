using Master;
using System;
using UnityEngine;

public class BattleUIMultiX2PlayButton : BattleUISingleX2PlayButton
{
	[SerializeField]
	private GameObject ownerOnlyBaloon;

	[SerializeField]
	private UILabel message;

	protected override void Start()
	{
		base.Start();
		this.message.text = StringMaster.GetString("BattleUI-26");
	}

	public void SetActiveSpeedAlert(bool value)
	{
		this.ownerOnlyBaloon.SetActive(value);
	}
}
