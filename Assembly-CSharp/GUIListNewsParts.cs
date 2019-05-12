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

	private bool isTouchEndFromChild;

	[SerializeField]
	private GameObject newMarker;

	private GameWebAPI.RespDataIN_InfoList.InfoList data;

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
		switch (groupType)
		{
		case "1":
			arg = StringMaster.GetString("InfomationTitle");
			goto IL_C2;
		case "2":
			arg = StringMaster.GetString("InfomationUpdate");
			goto IL_C2;
		case "3":
			arg = StringMaster.GetString("InfomationEvent");
			goto IL_C2;
		}
		arg = StringMaster.GetString("InfomationOther");
		IL_C2:
		this.newsTitle.text = string.Format(StringMaster.GetString("InfomationSubTitle"), arg);
		this.newsDetails.text = this.Data.title;
		this.newsDate.text = this.Data.startDateTime;
		this.newsDate.text = this.newsDate.text.Remove(16);
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
		CommonDialog commonDialog = GUIMain.ShowCommonDialog(new Action<int>(this.ClosedInfoWindow), "CMDWebWindow");
		((CMDWebWindow)commonDialog).TitleText = this.Data.title;
		((CMDWebWindow)commonDialog).callbackAction = new Action(this.CallbackClosePanel);
		((CMDWebWindow)commonDialog).Url = ConstValue.APP_WEB_DOMAIN + ConstValue.WEB_INFO_ADR + this.Data.userInfoId;
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
	}
}
