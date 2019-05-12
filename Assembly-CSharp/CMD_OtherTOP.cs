using Master;
using System;
using UnityEngine;

public sealed class CMD_OtherTOP : CMD
{
	[SerializeField]
	private GameObject jpButtonRoot;

	[SerializeField]
	private GameObject wwButtonRoot;

	[SerializeField]
	private GameObject wwButtonListLeft;

	[SerializeField]
	private GameObject wwButtonListRight;

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		if (this.jpButtonRoot.activeSelf)
		{
			this.jpButtonRoot.SetActive(false);
		}
		if (!this.wwButtonRoot.activeSelf)
		{
			this.wwButtonRoot.SetActive(true);
		}
		this.goEFC_LEFT = this.wwButtonListLeft;
		this.goEFC_RIGHT = this.wwButtonListRight;
		SoundMng.Instance().PlayGameBGM("bgm_102");
		base.PartsTitle.SetTitle(StringMaster.GetString("InfomationOther"));
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	public override void ClosePanel(bool animation = true)
	{
		SoundMng.Instance().PlayGameBGM("bgm_201");
		this.CloseAndFarmCamOn(animation);
	}

	private void OnClickOfficialSite()
	{
		Application.OpenURL(ConstValue.OFFICIAL_SITE_URL);
	}

	private void OnClickedTermsOfUse()
	{
		Application.OpenURL(WebAddress.EXT_ADR_AGREE);
	}

	private void OnClickedRightsExpression()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow", null) as CMDWebWindow;
		if (null != cmdwebWindow)
		{
			cmdwebWindow.TitleText = StringMaster.GetString("OtherRight");
			cmdwebWindow.Url = WebAddress.EXT_ADR_COPY;
		}
	}

	private void OnClickedPurchaseHistory()
	{
		GUIMain.ShowCommonDialog(null, "CMD_History", null);
	}

	private void OnClickedTakeover()
	{
		CMD_Takeover.currentMode = CMD_Takeover.MODE.ISSUE;
		GUIMain.ShowCommonDialog(null, "CMD_TakeoverIssue", null);
	}

	private void OnClickedContact()
	{
		GUIMain.ShowCommonDialog(null, "CMD_inquiry", null);
	}

	private void OnClickedPrivacyOfUse()
	{
		Application.OpenURL(WebAddress.EXT_ADR_PRIVACY_POLICY);
	}

	private void OnClickedGDPR()
	{
		Application.OpenURL(ConstValue.GDPR_OPT_OUT_SITE_URL);
	}
}
