using System;
using UnityEngine;

public class CMD_ChatModal : CMD
{
	public static CMD_ChatModal instance;

	[SerializeField]
	private GameObject goTX_TITLE;

	[SerializeField]
	private GameObject goTX_BTN_CLOSE;

	[SerializeField]
	private GameObject goTX_BTN_YES;

	[SerializeField]
	private GameObject goTX_BTN_NO;

	[SerializeField]
	private UILabel ngTX_BTN_CLOSE;

	[SerializeField]
	private UILabel ngTX_BTN_YES;

	[SerializeField]
	private UILabel ngTX_BTN_NO;

	private UILabel ngTX_TITLE;

	private Action CloseBtnCallbackAction;

	private Action YesBtnCallbackAction;

	private Action NoBtnCallbackAction;

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

	public void SetTitle(string str)
	{
		this.ngTX_TITLE.text = str;
	}

	public void SetBtn_CLOSE(bool enable, string text = "", Action action = null)
	{
		this.ngTX_BTN_CLOSE.text = text;
		this.goTX_BTN_CLOSE.SetActive(enable);
		this.CloseBtnCallbackAction = action;
	}

	public void SetBtn_YES(bool enable, string text = "", Action action = null)
	{
		this.ngTX_BTN_YES.text = text;
		this.goTX_BTN_YES.SetActive(enable);
		this.YesBtnCallbackAction = action;
	}

	public void SetBtn_NO(bool enable, string text = "", Action action = null)
	{
		this.ngTX_BTN_NO.text = text;
		this.goTX_BTN_NO.SetActive(enable);
		this.NoBtnCallbackAction = action;
	}

	public void ButtonAdjust()
	{
		if (this.goTX_BTN_CLOSE.activeSelf && !this.goTX_BTN_YES.activeSelf && !this.goTX_BTN_NO.activeSelf)
		{
			this.goTX_BTN_CLOSE.transform.SetLocalX(0f);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		CMD_ChatModal.instance = this;
		this.ngTX_TITLE = this.goTX_TITLE.GetComponent<UILabel>();
	}

	private void ClickYesBtn()
	{
		if (this.YesBtnCallbackAction != null)
		{
			this.YesBtnCallbackAction();
		}
	}

	private void ClickNoBtn()
	{
		if (this.NoBtnCallbackAction != null)
		{
			this.NoBtnCallbackAction();
		}
	}

	private void ClickCloseBtn()
	{
		this.ClosePanel(true);
		if (this.CloseBtnCallbackAction != null)
		{
			this.CloseBtnCallbackAction();
		}
	}
}
