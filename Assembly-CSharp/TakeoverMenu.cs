using Master;
using System;
using System.Collections;
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
		this.InitializedFacebook();
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
		if (this.currentMode == TakeoverMenu.MODE.Input)
		{
			this.TakeoverFacebook();
		}
		else if (this.currentMode == TakeoverMenu.MODE.Issue)
		{
			this.CooperateFacebook();
		}
	}

	private void OpenMessageBtn2(string title, string text, Action callback = null)
	{
		CMD_ModalMessageBtn2 cmd_ModalMessageBtn = GUIMain.ShowCommonDialog(delegate(int x)
		{
			if (x == 0 && callback != null)
			{
				callback();
			}
		}, "CMD_ModalMessageBtn2", null) as CMD_ModalMessageBtn2;
		cmd_ModalMessageBtn.SetTitle(StringMaster.GetString(title));
		cmd_ModalMessageBtn.SetExp(StringMaster.GetString(text));
		cmd_ModalMessageBtn.SetBtnText_YES(StringMaster.GetString("SystemButtonYes"));
		cmd_ModalMessageBtn.SetBtnText_NO(StringMaster.GetString("SystemButtonNo"));
	}

	private void InitializedFacebook()
	{
		this.userStatusCheck = null;
		if (this.facebookBackupButtonObj != null)
		{
			this.facebookBackupButtonObj.SetActive(true);
		}
		if (!FacebookWrapper.Instance.IsInitialized)
		{
			FacebookWrapper.Instance.Init(delegate(bool r)
			{
				if (this.facebookBackupButtonObj != null)
				{
					this.facebookBackupButtonObj.SetActive(r);
				}
				if (r)
				{
					this.CheckSocialStatus();
				}
				if (this.uiTable != null && this.currentMode == TakeoverMenu.MODE.Issue)
				{
					this.uiTable.Reposition();
				}
			});
		}
		else
		{
			if (this.facebookBackupButtonObj != null)
			{
				this.facebookBackupButtonObj.SetActive(true);
			}
			this.CheckSocialStatus();
		}
	}

	private void UpdateFacebookButton()
	{
		if (this.currentMode == TakeoverMenu.MODE.Issue)
		{
			if (FacebookWrapper.Instance.IsLoggedIn)
			{
				if (this.facebookLoginText != null)
				{
					if (this.userStatusCheck != null)
					{
						if (this.userStatusCheck.Authenticated())
						{
							this.facebookLoginText.text = StringMaster.GetString("Facebook_LogoutBtn");
						}
						else
						{
							this.facebookLoginText.text = StringMaster.GetString("Facebook_LoginBtn");
							this.facebookUserId.text = string.Empty;
						}
					}
					else
					{
						this.facebookLoginText.text = StringMaster.GetString("Facebook_LogoutBtn");
					}
				}
			}
			else
			{
				if (this.facebookLoginText != null)
				{
					this.facebookLoginText.text = StringMaster.GetString("Facebook_LoginBtn");
				}
				if (this.facebookUserId != null)
				{
					this.facebookUserId.text = string.Empty;
				}
			}
		}
	}

	private void CooperateFacebook()
	{
		if (FacebookWrapper.Instance.IsLoggedIn)
		{
			if (this.userStatusCheck != null)
			{
				if (this.userStatusCheck.Authenticated())
				{
					this.OpenMessageBtn2("Facebook_ReleaseTitle", "Facebook_ReleaseText", new Action(this.LogOutFacebook));
				}
				else
				{
					this.OpenMessageBtn2("Facebook_ConfirmTitle", "Facebook_ConfirmExit", delegate
					{
						this.ReceiveModelChangeData();
					});
				}
			}
			else
			{
				this.OpenMessageBtn2("Facebook_ReleaseTitle", "Facebook_ReleaseText", new Action(this.LogOutFacebook));
			}
		}
		else
		{
			this.OpenMessageBtn2("Facebook_ConfirmTitle", "Facebook_ConfirmExit", delegate
			{
				this.LogInFacebook(new Action(this.ReceiveModelChangeData));
			});
		}
	}

	private IEnumerator WaitLogOutFacebook(bool isSuccess)
	{
		if (isSuccess)
		{
			FacebookWrapper.Instance.LogOut();
			yield return new WaitForSeconds(2f);
		}
		else
		{
			CMD_ModalMessage cd = CMD_ModalMessage.Create(StringMaster.GetString("SystemConfirm"), StringMaster.GetString("Facebook_TimeOut"), null);
			cd.BtnText = StringMaster.GetString("SystemButtonClose");
		}
		RestrictionInput.EndLoad();
		yield break;
	}

	private void LogOutFacebook()
	{
		bool isSuccess = false;
		GameWebAPI.RequestTL_DeleteUserSocialStatus requestTL_DeleteUserSocialStatus = new GameWebAPI.RequestTL_DeleteUserSocialStatus();
		requestTL_DeleteUserSocialStatus.SetSendData = delegate(GameWebAPI.SendDataTL_DeleteUserSocialStatus param)
		{
			param.accessToken = FacebookWrapper.Instance.GetAccessToken();
		};
		requestTL_DeleteUserSocialStatus.OnReceived = delegate(GameWebAPI.RespDataTL_DeleteUserSocialStatus response)
		{
			isSuccess = response.IsSuccess();
		};
		GameWebAPI.RequestTL_DeleteUserSocialStatus request = requestTL_DeleteUserSocialStatus;
		TaskBase task = new APIRequestTask(request, false).Add(new NormalTask(() => this.WaitLogOutFacebook(isSuccess)));
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		AppCoroutine.Start(task.Run(delegate
		{
			this.userStatusCheck = null;
			this.UpdateFacebookButton();
		}, null, null), false);
	}

	private void CheckSocialStatus()
	{
		string accessToken = FacebookWrapper.Instance.GetAccessToken();
		if (FacebookWrapper.Instance.IsLoggedIn && !string.IsNullOrEmpty(accessToken))
		{
			GameWebAPI.RequestTL_UserSocialStatusCheckLogic request = new GameWebAPI.RequestTL_UserSocialStatusCheckLogic
			{
				SetSendData = delegate(GameWebAPI.SendDataTL_UserSocialStatusCheckLogic param)
				{
					param.accessToken = accessToken;
				},
				OnReceived = delegate(GameWebAPI.RespDataTL_UserSocialStatusCheckLogic response)
				{
					this.userStatusCheck = response;
				}
			};
			AppCoroutine.Start(request.Run(delegate()
			{
				if (FacebookWrapper.Instance.IsLoggedIn)
				{
					FacebookWrapper.Instance.GetProfileName(new Action(this.FacebookUserName));
				}
				this.UpdateFacebookButton();
			}, null, null), false);
		}
		else
		{
			if (FacebookWrapper.Instance.IsLoggedIn)
			{
				FacebookWrapper.Instance.GetProfileName(new Action(this.FacebookUserName));
			}
			this.UpdateFacebookButton();
		}
	}

	private void LogInFacebook(Action callback)
	{
		FacebookWrapper.Instance.LogIn(delegate(bool r)
		{
			if (r)
			{
				if (callback != null)
				{
					callback();
				}
				else
				{
					this.UpdateFacebookButton();
				}
			}
			else
			{
				CMD_ModalMessage cmd_ModalMessage = CMD_ModalMessage.Create(StringMaster.GetString("Facebook_ConfirmTitle"), StringMaster.GetString("Facebook_ConfirmText"), delegate(int x)
				{
					this.UpdateFacebookButton();
				});
				cmd_ModalMessage.BtnText = StringMaster.GetString("SystemButtonClose");
			}
		}, new Action(this.FacebookUserName));
	}

	private void ReceiveModelChangeData()
	{
		string accessToken = FacebookWrapper.Instance.GetAccessToken();
		if (accessToken != null)
		{
			GameWebAPI.RequestTL_ModelChangeRequest requestTL_ModelChangeRequest = new GameWebAPI.RequestTL_ModelChangeRequest();
			requestTL_ModelChangeRequest.SetSendData = delegate(GameWebAPI.SendDataTL_ModelChangeRequest param)
			{
				param.accessToken = accessToken;
			};
			requestTL_ModelChangeRequest.OnReceived = delegate(GameWebAPI.RespDataTL_ModelChangeRequest response)
			{
			};
			GameWebAPI.RequestTL_ModelChangeRequest request = requestTL_ModelChangeRequest;
			AppCoroutine.Start(request.Run(delegate()
			{
				this.userStatusCheck = null;
				this.UpdateFacebookButton();
			}, null, null), false);
		}
		else
		{
			global::Debug.Log("Token:null");
		}
	}

	private void TakeoverFacebook()
	{
		if (FacebookWrapper.Instance.IsLoggedIn)
		{
			this.OpenMessageBtn2("Facebook_ConfirmTitle", "Facebook_ConfirmExit", new Action(this.ReceiveSocialStatus));
		}
		else
		{
			this.OpenMessageBtn2("Facebook_ConfirmTitle", "Facebook_ConfirmExit", delegate
			{
				this.LogInFacebook(new Action(this.ReceiveSocialStatus));
			});
		}
	}

	private void ReceiveSocialStatus()
	{
		GameWebAPI.RequestTL_UserSocialStatusInfo requestTL_UserSocialStatusInfo = new GameWebAPI.RequestTL_UserSocialStatusInfo();
		requestTL_UserSocialStatusInfo.SetSendData = delegate(GameWebAPI.SendDataTL_UserSocialStatusInfo param)
		{
			param.accessToken = FacebookWrapper.Instance.GetAccessToken();
		};
		requestTL_UserSocialStatusInfo.OnReceived = delegate(GameWebAPI.RespDataTL_UserSocialStatusInfo response)
		{
			this.userSocialStatus = response;
		};
		GameWebAPI.RequestTL_UserSocialStatusInfo request = requestTL_UserSocialStatusInfo;
		AppCoroutine.Start(request.Run(delegate()
		{
			CMD_FacebookConfirm cmd_FacebookConfirm = GUIMain.ShowCommonDialog(delegate(int x)
			{
				if (x == 1)
				{
					this.ClickedTakeover();
				}
				else
				{
					GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
				}
			}, "CMD_FacebookConfirm", null) as CMD_FacebookConfirm;
			cmd_FacebookConfirm.SetUserData(this.userSocialStatus);
		}, null, null), false);
	}

	public void ClickedTakeover()
	{
		string accessToken = FacebookWrapper.Instance.GetAccessToken();
		if (!string.IsNullOrEmpty(accessToken))
		{
			GameWebAPI.RespDataTL_ModelChangeAuthLogic respData = null;
			GameWebAPI.RequestTL_ModelChangeAuthLogic request = new GameWebAPI.RequestTL_ModelChangeAuthLogic
			{
				SetSendData = delegate(GameWebAPI.SendDataTL_ModelChangeAuthLogic param)
				{
					param.accessToken = accessToken;
					param.transferCode = this.userSocialStatus.transferCode;
					param.transferUserCode = this.userSocialStatus.userCode.ToInt32();
				},
				OnReceived = delegate(GameWebAPI.RespDataTL_ModelChangeAuthLogic response)
				{
					respData = response;
				}
			};
			APIRequestTask task = new APIRequestTask(request, false);
			task.Add(Singleton<UserDataMng>.Instance.RequestPlayerInfo(true));
			CMD_ModalMessageBtn2 cmd_ModalMessageBtn = GUIMain.ShowCommonDialog(delegate(int x)
			{
				if (x == 1)
				{
					RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
					this.StartCoroutine(task.Run(delegate
					{
						int result = 0;
						if (respData != null)
						{
							result = respData.transferStatus;
						}
						this.TryResult(result);
					}, delegate(Exception nop)
					{
						int result = 0;
						if (respData != null)
						{
							result = respData.transferStatus;
						}
						this.TryResult(result);
					}, null));
				}
				else
				{
					GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
				}
			}, "CMD_FacebookConfirm2", null) as CMD_ModalMessageBtn2;
		}
	}

	private void TryResult(int result)
	{
		RestrictionInput.EndLoad();
		string @string = StringMaster.GetString("TakeOver-10");
		string string2 = StringMaster.GetString("TakeOver-11");
		Action<int> onCloseAction;
		if (result == 1)
		{
			onCloseAction = delegate(int x)
			{
				PlayerPrefsExtentions.DeleteAllGameParams();
				GameCache.ClearCache(null);
				GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
			};
		}
		else
		{
			onCloseAction = delegate(int x)
			{
				GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
			};
			@string = StringMaster.GetString("TakeOver-12");
			string2 = StringMaster.GetString("AlertNetworkErrorRetry");
		}
		CMD_ModalMessage cmd_ModalMessage = CMD_ModalMessage.Create(@string, string2, onCloseAction);
		cmd_ModalMessage.BtnText = StringMaster.GetString("SystemButtonGoTitle");
	}

	private void FacebookUserName()
	{
		if (this.facebookUserId != null)
		{
			bool flag = true;
			if (this.userStatusCheck == null || (this.userStatusCheck != null && !this.userStatusCheck.Authenticated()))
			{
				flag = false;
			}
			if (FacebookWrapper.Instance.UserName.Length > 0 && flag)
			{
				this.facebookUserId.text = string.Format(StringMaster.GetString("Facebook_UserId"), FacebookWrapper.Instance.UserName);
			}
			else
			{
				this.facebookUserId.text = string.Empty;
			}
		}
	}

	public enum MODE
	{
		Input,
		Issue
	}
}
