using Master;
using System;
using UnityEngine;
using WebAPIRequest;

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
		GameWebAPI.RequestCM_InquiryCodeRequest request = new GameWebAPI.RequestCM_InquiryCodeRequest
		{
			OnReceived = delegate(GameWebAPI.InquiryCodeRequest response)
			{
				this.responseContactCode = response.inquiryCode;
				this.responseClipbordText = response.result;
			}
		};
		APIRequestTask apirequestTask = new APIRequestTask(request, true);
		string apiId = string.Empty;
		if (string.IsNullOrEmpty(ConstValue.CONTACT_SITE_URL))
		{
			MasterBase master = MasterDataMng.Instance().GetMaster(MasterId.CODE_MASTER);
			RequestBase requestBase = master.CreateRequest();
			apirequestTask.Add(new APIRequestTask(requestBase, true));
			apiId = requestBase.apiId;
			GameWebAPI.Instance().AddDisableVersionCheckApiId(apiId);
		}
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.StartCoroutine(apirequestTask.Run(delegate
		{
			if (!string.IsNullOrEmpty(apiId))
			{
				GameWebAPI.Instance().RemoveDisableVersionCheckApiId(apiId);
			}
			RestrictionInput.EndLoad();
			this.contactCode.text = this.GetMoldedContactCode(this.responseContactCode);
			this.ShowDLG();
			this.Show(closeEvent, sizeX, sizeY, showTime);
		}, delegate(Exception nop)
		{
			if (!string.IsNullOrEmpty(apiId))
			{
				GameWebAPI.Instance().RemoveDisableVersionCheckApiId(apiId);
			}
			RestrictionInput.EndLoad();
			this.ClosePanel(false);
		}, null));
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
			Clipboard.Text = this.responseClipbordText;
		}
		else
		{
			Clipboard.Text = this.responseClipbordText + "\n(" + PlayerPrefs.GetString(key) + ")";
		}
		Application.OpenURL(ConstValue.CONTACT_SITE_URL);
	}

	private void OnPushedCloseButton()
	{
		this.ClosePanel(true);
	}
}
