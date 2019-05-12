using Master;
using System;
using UnityEngine;

public class CMD_FacebookConfirm : CMD_ModalMessageBtn2
{
	[SerializeField]
	private UILabel userName;

	[SerializeField]
	private UILabel userCode;

	protected override void Awake()
	{
		base.Awake();
		base.SetBtnText_YES(StringMaster.GetString("SystemButtonYes"));
		base.SetBtnText_NO(StringMaster.GetString("SystemButtonNo"));
	}

	public void SetUserData(GameWebAPI.RespDataTL_UserSocialStatusInfo statusInfo)
	{
		this.userName.text = statusInfo.nickname;
		this.userCode.text = statusInfo.userCode;
	}
}
