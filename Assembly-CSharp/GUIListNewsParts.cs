using Master;
using System;
using UnityEngine;

public class GUIListNewsParts : GUIListPartBS
{
	[SerializeField]
	private UILabel newsTitle;

	[SerializeField]
	private UILabel newsDetails;

	[SerializeField]
	private UILabel newsDate;

	[SerializeField]
	private UISprite presentIcon;

	private bool isTouchEndFromChild;

	[SerializeField]
	private GameObject newMarker;

	private GameWebAPI.RespDataIN_InfoList.InfoList data;

	public Action callbackClose;

	public GameWebAPI.RespDataIN_InfoList.InfoList Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
			this.ShowGUI();
		}
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		string arg = string.Empty;
		string groupType = this.Data.groupType;
		if (groupType != null)
		{
			if (groupType == "1")
			{
				arg = StringMaster.GetString("InfomationTitle");
				goto IL_93;
			}
			if (groupType == "2")
			{
				arg = StringMaster.GetString("InfomationUpdate");
				goto IL_93;
			}
			if (groupType == "3")
			{
				arg = StringMaster.GetString("InfomationEvent");
				goto IL_93;
			}
		}
		arg = StringMaster.GetString("InfomationOther");
		IL_93:
		this.newsTitle.text = string.Format(StringMaster.GetString("InfomationSubTitle"), arg);
		this.newsDetails.text = this.Data.title;
		string text = string.Empty;
		DateTime jpDateTime = DateTime.Parse(this.Data.startDateTime);
		text = TimeUtility.ToJPLocalDateTime(jpDateTime).ToString("yyyy-MM-dd HH:mm:ss");
		this.newsDate.text = text;
		this.newsDate.text = this.newsDate.text.Remove(16);
		this.presentIcon.gameObject.SetActive(this.Data.prizeStatus != 0);
		this.presentIcon.color = ((this.Data.prizeStatus != 2) ? Color.white : Color.gray);
		if (this.Data.confirmationFlg == 0)
		{
			this.newMarker.SetActive(true);
		}
		else
		{
			this.newMarker.SetActive(false);
		}
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchBegan(touch, pos);
		this.isTouchEndFromChild = false;
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchEnded(touch, pos, flag);
		if (flag)
		{
			float magnitude = (this.beganPostion - pos).magnitude;
			if (magnitude < 40f && !this.isTouchEndFromChild)
			{
				CMD_QuestTOP.ChangeSelectA_StageL_S(base.IDX, false);
			}
		}
	}

	private void OnClickedBtnSelect()
	{
	}

	private void OnCrickedInfo()
	{
		CommonDialog commonDialog = GUIMain.ShowCommonDialog(new Action<int>(this.ClosedInfoWindow), "CMDWebWindow", null);
		((CMDWebWindow)commonDialog).TitleText = this.Data.title;
		((CMDWebWindow)commonDialog).callbackAction = new Action(this.CallbackClosePanel);
		((CMDWebWindow)commonDialog).Url = ConstValue.APP_WEB_DOMAIN + ConstValue.WEB_INFO_ADR + this.Data.userInfoId;
		CMDWebWindow cmdwebWindow = (CMDWebWindow)commonDialog;
		cmdwebWindow.Url = cmdwebWindow.Url + "&countryCode=" + CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN);
	}

	private void CallbackClosePanel()
	{
		if (!GUIManager.IsCloseAllMode())
		{
			CMD_NewsALL.instance.ClosePanel(true);
		}
	}

	private void ClosedInfoWindow(int id)
	{
		if (this.data.confirmationFlg == 0)
		{
			this.newMarker.SetActive(false);
			this.data.confirmationFlg = 1;
		}
		if (this.callbackClose != null)
		{
			this.callbackClose();
		}
	}
}
