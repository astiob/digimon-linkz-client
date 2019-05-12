using Master;
using System;
using UnityEngine;

public class TakeoverMenu : MonoBehaviour
{
	[SerializeField]
	private UILabel takeoverButtonLabel;

	[SerializeField]
	private GameObject iCloudbackupButtonObj;

	[SerializeField]
	private GameObject googleBackupButtonObj;

	[SerializeField]
	private GameObject facebookBackupButtonObj;

	[SerializeField]
	private TakeoverInput input;

	[SerializeField]
	private TakeoverIssue issue;

	[SerializeField]
	private UILabel titleText;

	[SerializeField]
	private UILabel googleLoginText;

	[SerializeField]
	private UILabel iCloudLoginText;

	[SerializeField]
	private UILabel facebookLoginText;

	[SerializeField]
	private UILabel facebookUserId;

	[SerializeField]
	private UITable uiTable;

	private TakeoverMenu.MODE currentMode;

	private GameWebAPI.RespDataTL_UserSocialStatusInfo userSocialStatus;

	private GameWebAPI.RespDataTL_UserSocialStatusCheckLogic userStatusCheck;

	public void Initialize(TakeoverMenu.MODE MenuMode)
	{
		if (this.titleText != null)
		{
			this.titleText.text = StringMaster.GetString("TakeOverTitle");
		}
		this.googleLoginText.text = StringMaster.GetString("TakeOver-14");
		this.iCloudLoginText.text = StringMaster.GetString("TakeOver-15");
		this.currentMode = MenuMode;
		this.googleBackupButtonObj.SetActive(true);
		this.iCloudbackupButtonObj.SetActive(false);
		if (this.facebookBackupButtonObj != null)
		{
			this.facebookBackupButtonObj.SetActive(false);
		}
		if (this.uiTable != null && this.currentMode == TakeoverMenu.MODE.Issue)
		{
			this.uiTable.Reposition();
		}
		if (MenuMode == TakeoverMenu.MODE.Input)
		{
			this.takeoverButtonLabel.text = StringMaster.GetString("TakeOver-16");
		}
		else if (MenuMode == TakeoverMenu.MODE.Issue)
		{
			this.takeoverButtonLabel.text = StringMaster.GetString("TakeOver-17");
		}
	}

	private void OnClickedTakeover()
	{
		base.gameObject.SetActive(false);
		if (this.currentMode == TakeoverMenu.MODE.Input)
		{
			this.input.gameObject.SetActive(true);
			this.input.Initialize();
		}
		else if (this.currentMode == TakeoverMenu.MODE.Issue)
		{
			this.issue.gameObject.SetActive(true);
			this.issue.Initialize();
		}
	}

	private void OnClickedBackup()
	{
		CMD_BackupModal cmd_BackupModal = GUIMain.ShowCommonDialog(delegate(int index)
		{
			CloudBackup cloudBackup = new CloudBackup();
			if (this.currentMode == TakeoverMenu.MODE.Input)
			{
				cloudBackup.LoadBackupData();
			}
			else if (this.currentMode == TakeoverMenu.MODE.Issue)
			{
				cloudBackup.SaveBackupData();
			}
		}, "CMD_BackupModal", null) as CMD_BackupModal;
		cmd_BackupModal.ChangeButtonToBackupFromTwo();
		if (this.currentMode == TakeoverMenu.MODE.Input)
		{
			cmd_BackupModal.Title = StringMaster.GetString("TakeOver-18");
			cmd_BackupModal.Info = StringMaster.GetString("TakeOver-19");
		}
		else if (this.currentMode == TakeoverMenu.MODE.Issue)
		{
			cmd_BackupModal.Title = StringMaster.GetString("TakeOver-20");
			cmd_BackupModal.Info = StringMaster.GetString("TakeOver-21");
		}
	}

	private void OnClickedFacebook()
	{
	}

	public enum MODE
	{
		Input,
		Issue
	}
}
