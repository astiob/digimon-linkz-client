using System;
using UnityEngine;

public class CMD_ModalMessageBtn2 : CMD
{
	public GameObject goTX_TITLE;

	public GameObject goTX_EXP;

	public GameObject goTX_BTN_YES;

	public GameObject goTX_BTN_NO;

	private UILabel ngTX_TITLE;

	private UILabel ngTX_EXP;

	private UILabel ngTX_BTN_YES;

	private UILabel ngTX_BTN_NO;

	public void SetTitle(string str)
	{
		this.ngTX_TITLE.text = str;
	}

	public void SetExp(string str)
	{
		this.ngTX_EXP.text = str;
	}

	public void SetBtnText_YES(string str)
	{
		this.ngTX_BTN_YES.text = str;
	}

	public void SetBtnText_NO(string str)
	{
		this.ngTX_BTN_NO.text = str;
	}

	protected override void Awake()
	{
		base.Awake();
		this.ngTX_TITLE = this.goTX_TITLE.GetComponent<UILabel>();
		this.ngTX_EXP = this.goTX_EXP.GetComponent<UILabel>();
		this.ngTX_BTN_YES = this.goTX_BTN_YES.GetComponent<UILabel>();
		this.ngTX_BTN_NO = this.goTX_BTN_NO.GetComponent<UILabel>();
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
}
