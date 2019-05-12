using Master;
using System;
using UnityEngine;

public class CMD_BackupModal : CMD
{
	[SerializeField]
	private UILabel textTitle;

	[SerializeField]
	private UILabel textInfo;

	[SerializeField]
	private GUICollider cancelCollider;

	[SerializeField]
	private GUICollider yesCollider;

	[SerializeField]
	private GameObject twoButtonObj;

	[SerializeField]
	private GameObject backupButtonObj;

	[SerializeField]
	private GameObject googleButtonObj;

	[SerializeField]
	private GameObject iCloudButtonObj;

	[SerializeField]
	private UILabel googleBackUpButtonText;

	[SerializeField]
	private UILabel iCloudBackUpButtonText;

	private string title = string.Empty;

	private string info = string.Empty;

	private Action actionYesButton;

	private Action actionNoButton;

	public string Title
	{
		get
		{
			return this.title;
		}
		set
		{
			this.title = value;
			this.textTitle.text = this.title;
		}
	}

	public string Info
	{
		get
		{
			return this.info;
		}
		set
		{
			this.info = TextUtil.GetWinTextSkipColorCode(value, 40);
			this.textInfo.text = this.info;
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

	public void ChangeButtonToBackupFromTwo()
	{
		this.twoButtonObj.SetActive(false);
		this.backupButtonObj.SetActive(true);
		this.googleButtonObj.SetActive(true);
		this.iCloudButtonObj.SetActive(false);
		this.googleBackUpButtonText.text = StringMaster.GetString("BackupModalGoogle");
	}

	private void OnPushYesButton()
	{
		if (this.actionYesButton != null)
		{
			this.actionYesButton();
		}
		this.ClosePanel(true);
	}

	private void OnPushNoButton()
	{
		if (this.actionNoButton != null)
		{
			this.actionNoButton();
		}
		this.ClosePanel(true);
	}

	public void SetYesButtonAction(Action action)
	{
		this.actionYesButton = action;
	}

	public void SetNoButtonAction(Action action)
	{
		this.actionNoButton = action;
	}
}
