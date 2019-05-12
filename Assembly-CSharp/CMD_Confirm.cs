using Master;
using System;
using UnityEngine;

public class CMD_Confirm : CMD
{
	[SerializeField]
	private UILabel textTitle;

	[SerializeField]
	private UILabel textInfo;

	[SerializeField]
	private UILabel ngTX_BTN_YES;

	[SerializeField]
	private UILabel ngTX_BTN_NO;

	[SerializeField]
	private GUICollider cancelCollider;

	[SerializeField]
	private GUICollider yesCollider;

	public string Title
	{
		get
		{
			return this.textTitle.text;
		}
		set
		{
			this.textTitle.text = value;
		}
	}

	public string Info
	{
		get
		{
			return this.textInfo.text;
		}
		set
		{
			this.textInfo.text = TextUtil.GetWinTextSkipColorCode(value, 40);
		}
	}

	public string BtnTextYes
	{
		get
		{
			return this.ngTX_BTN_YES.text;
		}
		set
		{
			this.ngTX_BTN_YES.text = value;
		}
	}

	public string BtnTextNo
	{
		get
		{
			return this.ngTX_BTN_NO.text;
		}
		set
		{
			this.ngTX_BTN_NO.text = value;
		}
	}

	protected override void WindowOpened()
	{
		Singleton<GUIManager>.Instance.UseOutsideTouchControl = false;
		base.WindowOpened();
	}

	protected override void Awake()
	{
		base.Awake();
		this.ngTX_BTN_YES.text = StringMaster.GetString("SystemButtonYes");
		this.ngTX_BTN_NO.text = StringMaster.GetString("SystemButtonNo");
		if (this.cancelCollider != null)
		{
			this.cancelCollider.AvoidDisableAllCollider = true;
		}
		if (this.yesCollider != null)
		{
			this.yesCollider.AvoidDisableAllCollider = true;
		}
		Vector3 localPosition = Vector3.zero;
		localPosition = base.gameObject.transform.localPosition;
		localPosition.z = -11000f;
		base.gameObject.transform.localPosition = localPosition;
	}

	public override void ClosePanel(bool animation = true)
	{
		this.cancelCollider.enabled = false;
		this.yesCollider.enabled = false;
		base.ClosePanel(animation);
		BoxCollider[] componentsInChildren = base.GetComponentsInChildren<BoxCollider>(true);
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}
	}

	public void SetMaxLine(int num)
	{
		this.textInfo.maxLineCount = num;
	}
}
