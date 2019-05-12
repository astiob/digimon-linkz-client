using Master;
using System;
using UnityEngine;

public sealed class CMD_Alert : CMD
{
	public const int BUTTON_INDEX_RETRY = 1;

	public const int BUTTON_INDEX_CLOSE = 2;

	public const int BUTTON_INDEX_TITLE = 3;

	[SerializeField]
	private UILabel textTitle;

	[SerializeField]
	private UILabel textInfo;

	[SerializeField]
	private int MaxCharaOfLine;

	[SerializeField]
	private UISprite warningIcon;

	[SerializeField]
	private GameObject closeButton;

	[SerializeField]
	private UILabel closeLabel;

	[SerializeField]
	private GameObject retryButton;

	[SerializeField]
	private UILabel retryLabel;

	[SerializeField]
	private GameObject toTitleButton;

	[SerializeField]
	private UILabel toTitleLabel;

	private Func<CMD_Alert.ExtraFunctionReturnValue> actionOnButtonExtraFunction;

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
			this.textInfo.text = TextUtil.GetWinTextSkipColorCode(value, this.MaxCharaOfLine);
		}
	}

	public bool IsWarning
	{
		set
		{
			this.warningIcon.enabled = value;
		}
	}

	public void SetDisplayButton(CMD_Alert.DisplayButton displayButton)
	{
		Transform transform = null;
		switch (displayButton)
		{
		default:
			transform = this.closeButton.transform;
			this.retryButton.SetActive(false);
			this.toTitleButton.SetActive(false);
			break;
		case CMD_Alert.DisplayButton.RETRY:
			transform = this.retryButton.transform;
			this.closeButton.SetActive(false);
			this.toTitleButton.SetActive(false);
			break;
		case CMD_Alert.DisplayButton.TITLE:
			transform = this.toTitleButton.transform;
			this.closeButton.SetActive(false);
			this.retryButton.SetActive(false);
			break;
		case CMD_Alert.DisplayButton.TITLE_AND_RETRY:
			this.closeButton.SetActive(false);
			break;
		}
		if (null != transform)
		{
			Vector3 localPosition = transform.localPosition;
			localPosition.x = 0f;
			transform.localPosition = localPosition;
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
		if (this.closeButton != null)
		{
			GUICollider component = this.closeButton.GetComponent<GUICollider>();
			if (component != null)
			{
				component.AvoidDisableAllCollider = true;
			}
		}
		if (this.retryButton != null)
		{
			GUICollider component2 = this.retryButton.GetComponent<GUICollider>();
			if (component2 != null)
			{
				component2.AvoidDisableAllCollider = true;
			}
		}
		if (this.toTitleButton != null)
		{
			GUICollider component3 = this.toTitleButton.GetComponent<GUICollider>();
			if (component3 != null)
			{
				component3.AvoidDisableAllCollider = true;
			}
		}
		Vector3 localPosition = Vector3.zero;
		localPosition = base.gameObject.transform.localPosition;
		localPosition.z = -12000f;
		base.gameObject.transform.localPosition = localPosition;
		this.closeLabel.text = StringMaster.GetString("SystemButtonClose");
		this.retryLabel.text = StringMaster.GetString("SystemButtonRetry");
		this.toTitleLabel.text = StringMaster.GetString("SystemButtonGoTitle");
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(KeyCode.Escape) && !this.permanentMode && base.GetActionStatus() == CommonDialog.ACT_STATUS.OPEN && !GUICollider.IsAllColliderDisable())
		{
			this.OnCloseButton();
		}
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.actionOnButtonExtraFunction != null)
		{
			CMD_Alert.ExtraFunctionReturnValue extraFunctionReturnValue = this.actionOnButtonExtraFunction();
			CMD_Alert.ExtraFunctionReturnValue extraFunctionReturnValue2 = extraFunctionReturnValue;
			if (extraFunctionReturnValue2 == CMD_Alert.ExtraFunctionReturnValue.NOT_CLOSE)
			{
				return;
			}
		}
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

	private void OnCloseButton()
	{
		if (this.closeButton.activeSelf)
		{
			base.SetForceReturnValue(2);
			this.ClosePanel(true);
		}
	}

	private void OnRetryButton()
	{
		base.SetForceReturnValue(1);
		this.ClosePanel(true);
	}

	private void OnTitleButton()
	{
		base.SetForceReturnValue(3);
		this.ClosePanel(true);
	}

	public void SetActionButtonExtraFunction(Func<CMD_Alert.ExtraFunctionReturnValue> action)
	{
		this.actionOnButtonExtraFunction = action;
	}

	public enum DisplayButton
	{
		CLOSE,
		RETRY,
		TITLE,
		TITLE_AND_RETRY
	}

	public enum ExtraFunctionReturnValue
	{
		NONE,
		NOT_CLOSE
	}
}
