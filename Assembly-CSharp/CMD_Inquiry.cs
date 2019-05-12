using Master;
using System;
using UnityEngine;

public sealed class CMD_Inquiry : CMD
{
	[SerializeField]
	private UILabel title;

	[SerializeField]
	private UILabel description;

	[SerializeField]
	private UILabel contactCode;

	[SerializeField]
	private UILabel warningMessage;

	[SerializeField]
	private UILabel buttonText;

	private string responseContactCode;

	private string responseClipbordText;

	protected override void Awake()
	{
		base.Awake();
		this.title.text = StringMaster.GetString("InquiryTitle");
		this.description.text = StringMaster.GetString("InquiryInfo");
		this.contactCode.text = string.Empty;
		this.warningMessage.text = StringMaster.GetString("InquiryCaution");
		this.buttonText.text = StringMaster.GetString("InquiryButtonText");
	}

	public override void Show(Action<int> closeEvent, float sizeX, float sizeY, float showTime)
	{
		base.HideDLG();
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		this.responseContactCode = PlayerPrefs.GetString("InquiryCode", string.Empty);
		this.contactCode.text = this.GetMoldedContactCode(this.responseContactCode);
		RestrictionInput.EndLoad();
		base.ShowDLG();
		base.Show(closeEvent, sizeX, sizeY, showTime);
	}

	private string GetMoldedContactCode(string contactCode)
	{
		string result = contactCode;
		if (16 <= contactCode.Length)
		{
			result = string.Format("{0} {1} {2} {3}", new object[]
			{
				contactCode.Substring(0, 4),
				contactCode.Substring(4, 4),
				contactCode.Substring(8, 4),
				contactCode.Substring(12, 4)
			});
		}
		return result;
	}

	private void OnPushedContactButton()
	{
		string key = "LastErrorInfo";
		if (PlayerPrefs.HasKey(key))
		{
			Clipboard.Text = this.GetClipbordText();
		}
		else
		{
			Clipboard.Text = this.GetClipbordText() + "\n(" + PlayerPrefs.GetString(key) + ")";
		}
		Application.OpenURL(ConstValue.CONTACT_SITE_URL);
	}

	private void OnPushedCloseButton()
	{
		this.ClosePanel(true);
	}

	private string GetClipbordText()
	{
		string @string = StringMaster.GetString("Inquiry_MaintenanceText");
		if (string.IsNullOrEmpty(@string))
		{
			return string.Empty;
		}
		return string.Format(@string, new object[]
		{
			this.responseContactCode,
			SystemInfo.deviceModel,
			SystemInfo.operatingSystem,
			Application.version
		});
	}
}
