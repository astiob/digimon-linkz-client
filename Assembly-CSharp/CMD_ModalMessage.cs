using Master;
using System;
using UnityEngine;

public sealed class CMD_ModalMessage : CMD
{
	[SerializeField]
	private UILabel ngTX_TITLE;

	[SerializeField]
	private UILabel ngTX_EXP;

	[SerializeField]
	private UILabel ngTX_BTN;

	public static CMD_ModalMessage Create(string title, string info, Action<int> onCloseAction = null)
	{
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(onCloseAction, "CMD_ModalMessage") as CMD_ModalMessage;
		cmd_ModalMessage.Title = title;
		cmd_ModalMessage.Info = info;
		cmd_ModalMessage.BtnText = StringMaster.GetString("SystemButtonClose");
		return cmd_ModalMessage;
	}

	public string Title
	{
		get
		{
			return this.ngTX_TITLE.text;
		}
		set
		{
			this.ngTX_TITLE.text = value;
		}
	}

	public string Info
	{
		get
		{
			return this.ngTX_EXP.text;
		}
		set
		{
			this.ngTX_EXP.text = TextUtil.GetWinTextSkipColorCode(value, 40);
		}
	}

	public string InfoWithNoReturn
	{
		get
		{
			return this.ngTX_EXP.text;
		}
		set
		{
			this.ngTX_EXP.text = value;
		}
	}

	public string BtnText
	{
		get
		{
			return this.ngTX_BTN.text;
		}
		set
		{
			this.ngTX_BTN.text = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}

	private void Start()
	{
		this.ngTX_BTN.text = StringMaster.GetString("SystemButtonClose");
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public void AdjustSize()
	{
		UILabel component = this.ngTX_EXP.GetComponent<UILabel>();
		int num = component.text.Split(new char[]
		{
			'\n'
		}).Length;
		global::Debug.Log(num);
		if (num >= 5)
		{
			int num2 = (component.fontSize + component.spacingY) * (num - 4);
			base.GetComponent<UIWidget>().height += num2;
		}
	}
}
