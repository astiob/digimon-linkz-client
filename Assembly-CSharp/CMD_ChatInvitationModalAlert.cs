using Master;
using System;
using UnityEngine;

public class CMD_ChatInvitationModalAlert : CMD_ModalMessageBtn2
{
	[SerializeField]
	private GUIListChatMemberParts chatMemberParts;

	protected override void Awake()
	{
		base.Awake();
		base.SetTitle(StringMaster.GetString("ChatLeaveTitle"));
		base.SetExp(StringMaster.GetString("ChatLeaveText"));
		base.SetBtnText_YES(StringMaster.GetString("SystemButtonYes"));
		base.SetBtnText_NO(StringMaster.GetString("SystemButtonNo"));
	}

	public void SetUserData(GameWebAPI.ResponseData_ChatUserList.respUserList userData)
	{
		this.chatMemberParts.Data = userData;
	}

	public void PushedExpulsionDecision()
	{
		CMD_ChatInvitation.instance.PushedExpulsionDecision(this.chatMemberParts.Data);
		this.ClosePanel(true);
	}
}
