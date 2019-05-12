using Master;
using System;
using UnityEngine;

public class CMD_MockBattleMenu : CMD
{
	[Header("アイコン及びボタンのテキスト")]
	[SerializeField]
	private UILabel lbBTN_Friend;

	[SerializeField]
	private UILabel lbBTN_InputCode;

	[SerializeField]
	private UILabel lbBTN_Close;

	private static CMD_MockBattleMenu instance;

	public static CMD_MockBattleMenu Instance
	{
		get
		{
			return CMD_MockBattleMenu.instance;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		base.DontLookParent = true;
		CMD_MockBattleMenu.instance = this;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.lbBTN_Friend.text = StringMaster.GetString("FriendTitle");
		this.lbBTN_InputCode.text = StringMaster.GetString("ColosseumMockInputCode");
		this.lbBTN_Close.text = StringMaster.GetString("SystemButtonClose");
		base.Show(f, sizeX, sizeY, aT);
	}

	private void OnClickedFriend()
	{
		GUIMain.ShowCommonDialog(null, "CMD_PvPFriend");
	}

	private void OnClickedInputCode()
	{
		GUIMain.ShowCommonDialog(null, "CMD_PvPMockTargetInputModal");
	}

	private void OnClickedClose()
	{
		base.ClosePanel(true);
	}
}
