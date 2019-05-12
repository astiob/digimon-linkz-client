using Master;
using System;
using UnityEngine;

public class CMD_MultiBattleParticipateMenu : CMD
{
	public static CMD_MultiBattleParticipateMenu instance;

	[Header("GameObject：参加")]
	[SerializeField]
	private GameObject goBtnParticipate;

	[Header("GameObject：募集")]
	[SerializeField]
	private GameObject goBtnRecruit;

	[Header("Label：参加")]
	[SerializeField]
	private UILabel lbBtnParticipate;

	[Header("Label：募集")]
	[SerializeField]
	private UILabel lbBtnRecruit;

	protected override void Awake()
	{
		base.Awake();
		CMD_MultiBattleParticipateMenu.instance = this;
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.SetCommonUI();
		base.Show(f, sizeX, sizeY, aT);
	}

	private void SetCommonUI()
	{
		this.lbBtnParticipate.text = StringMaster.GetString("PartyParticipate");
		this.lbBtnRecruit.text = StringMaster.GetString("PartyRecruit");
	}
}
