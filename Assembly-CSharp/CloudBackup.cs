using Master;
using Neptune.GameService;
using System;
using System.Collections;
using UnityEngine;

public class CloudBackup
{
	private readonly string USER_CODE_KEY = "CLOUD_USER_CODE";

	private readonly string USER_ID_KEY = "CLOUD_USER_ID";

	private readonly string INQUIRY_CODE_KEY = "CLOUD_INQUIRY_CODE";

	private readonly int BACKUP_MISSION_ID = 251;

	private string inquiryCode = string.Empty;

	private string cloudUserCode = string.Empty;

	private string cloudInquiryCode = string.Empty;

	private string cloudUserID = string.Empty;

	private bool? clearBackupMissionFlag;

	private bool ClearBackupMissionFlag
	{
		get
		{
			bool? flag = this.clearBackupMissionFlag;
			if (flag == null)
			{
				this.clearBackupMissionFlag = new bool?(bool.Parse(PlayerPrefs.GetString("ClearBackupMission", "false")));
			}
			bool? flag2 = this.clearBackupMissionFlag;
			return flag2.Value;
		}
		set
		{
			this.clearBackupMissionFlag = new bool?(value);
			PlayerPrefs.SetString("ClearBackupMission", this.clearBackupMissionFlag.ToString());
		}
	}

	public Coroutine SaveBackupData()
	{
		return AppCoroutine.Start(this.SaveBackupDataForGoogle(), false);
	}

	private IEnumerator SaveBackupDataForGoogle()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		if (string.IsNullOrEmpty(this.inquiryCode))
		{
			yield return AppCoroutine.Start(this.GetInquiryCode(), false);
		}
		if (string.IsNullOrEmpty(this.inquiryCode))
		{
			RestrictionInput.EndLoad();
			yield break;
		}
		if (!GooglePlayGamesTool.Instance.IsSignIn)
		{
			yield return AppCoroutine.Start(this.SignInGoogle(), false);
		}
		if (!GooglePlayGamesTool.Instance.IsSignIn)
		{
			RestrictionInput.EndLoad();
			yield break;
		}
		yield return AppCoroutine.Start(this.LoadUserData(delegate(bool result)
		{
		}), false);
		if (!string.IsNullOrEmpty(this.cloudUserCode) && !string.IsNullOrEmpty(this.cloudInquiryCode))
		{
			if (this.cloudUserCode == DataMng.Instance().RespDataCM_Login.playerInfo.userCode && this.cloudInquiryCode == this.inquiryCode)
			{
				RestrictionInput.EndLoad();
				string title = StringMaster.GetString("CloudSynchronizedTitle");
				string info = StringMaster.GetString("CloudSynchronizedInfo");
				yield return AppCoroutine.Start(this.ShowCMD_ModalMessage(title, info), false);
				yield break;
			}
			string userName = string.Empty;
			yield return AppCoroutine.Start(this.GetUserNameByUserID(this.cloudUserID, delegate(string result)
			{
				userName = result;
			}), false);
			RestrictionInput.EndLoad();
			bool isPermit = false;
			string title2 = StringMaster.GetString("CloudOverwriteTitle");
			string format = StringMaster.GetString("CloudOverwriteInfo");
			string info2 = string.Format(format, this.cloudUserCode, userName);
			yield return AppCoroutine.Start(this.ShowCMD_BackupModal(title2, info2, delegate(bool result)
			{
				isPermit = result;
			}), false);
			if (!isPermit)
			{
				yield break;
			}
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		}
		bool isSuccess = false;
		yield return AppCoroutine.Start(this.SaveUserData(delegate(bool result)
		{
			isSuccess = result;
		}), false);
		RestrictionInput.EndLoad();
		if (isSuccess)
		{
			if (!this.ClearBackupMissionFlag)
			{
				yield return AppCoroutine.Start(this.SendClearBackupMission(), false);
			}
			string successTitle = StringMaster.GetString("CloudSyncSuccessTitle");
			string successInfo = StringMaster.GetString("CloudSyncSuccessInfo");
			yield return AppCoroutine.Start(this.ShowCMD_ModalMessage(successTitle, successInfo), false);
		}
		else
		{
			string errorTitle = StringMaster.GetString("CloudSyncFailedTitle");
			string errorInfo = StringMaster.GetString("CloudSyncFailedInfo");
			yield return AppCoroutine.Start(this.ShowCMD_ModalMessage(errorTitle, errorInfo), false);
		}
		yield break;
	}

	private IEnumerator SaveBackupDataForiCloud()
	{
		if (this.ClearBackupMissionFlag)
		{
			yield break;
		}
		if (string.IsNullOrEmpty(this.inquiryCode))
		{
			yield return AppCoroutine.Start(this.GetInquiryCode(), false);
		}
		if (string.IsNullOrEmpty(this.inquiryCode))
		{
			yield break;
		}
		if (!NpSingleton<NpGameService>.Instance.IsSignedIn())
		{
			yield break;
		}
		yield return AppCoroutine.Start(this.LoadUserData(delegate(bool result)
		{
		}), false);
		if (!string.IsNullOrEmpty(this.cloudUserCode) && !string.IsNullOrEmpty(this.cloudInquiryCode) && this.cloudUserCode == DataMng.Instance().RespDataCM_Login.playerInfo.userCode && this.cloudInquiryCode == this.inquiryCode)
		{
			yield break;
		}
		bool isSuccess = false;
		yield return AppCoroutine.Start(this.SaveUserData(delegate(bool result)
		{
			isSuccess = result;
		}), false);
		if (isSuccess && !this.ClearBackupMissionFlag)
		{
			yield return AppCoroutine.Start(this.SendClearBackupMission(), false);
		}
		yield break;
	}

	public Coroutine LoadBackupData()
	{
		return AppCoroutine.Start(this.LoadBackupDataForGoogle(), false);
	}

	private IEnumerator LoadBackupDataForGoogle()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		if (string.IsNullOrEmpty(this.inquiryCode))
		{
			yield return AppCoroutine.Start(this.GetInquiryCode(), false);
		}
		if (string.IsNullOrEmpty(this.inquiryCode))
		{
			RestrictionInput.EndLoad();
			yield break;
		}
		if (!GooglePlayGamesTool.Instance.IsSignIn)
		{
			yield return AppCoroutine.Start(this.SignInGoogle(), false);
		}
		if (!GooglePlayGamesTool.Instance.IsSignIn)
		{
			RestrictionInput.EndLoad();
			string title = StringMaster.GetString("GoogleLoginFailedTitle");
			string info = StringMaster.GetString("GoogleLoginFailedInfo");
			yield return AppCoroutine.Start(this.ShowCMD_ModalMessage(title, info), false);
			yield break;
		}
		yield return AppCoroutine.Start(this.LoadUserData(delegate(bool result)
		{
		}), false);
		if (string.IsNullOrEmpty(this.cloudUserCode) && string.IsNullOrEmpty(this.cloudInquiryCode))
		{
			RestrictionInput.EndLoad();
			string title2 = StringMaster.GetString("CloudTakeOverTitle");
			string info2 = StringMaster.GetString("CloudTakeOverNotFound");
			yield return AppCoroutine.Start(this.ShowCMD_ModalMessage(title2, info2), false);
			yield break;
		}
		if (this.cloudUserCode == DataMng.Instance().RespDataCM_Login.playerInfo.userCode && this.cloudInquiryCode == this.inquiryCode)
		{
			RestrictionInput.EndLoad();
			string title3 = StringMaster.GetString("CloudSynchronizedTitle");
			string info3 = StringMaster.GetString("CloudSynchronizedInfo");
			yield return AppCoroutine.Start(this.ShowCMD_ModalMessage(title3, info3), false);
			yield break;
		}
		string userName = string.Empty;
		yield return AppCoroutine.Start(this.GetUserNameByUserID(this.cloudUserID, delegate(string result)
		{
			userName = result;
		}), false);
		RestrictionInput.EndLoad();
		bool isPermit = false;
		string title4 = StringMaster.GetString("CloudTakeOverTitle");
		string format = StringMaster.GetString("CloudTakeOverInfo");
		string info4 = string.Format(format, this.cloudUserCode, userName);
		yield return AppCoroutine.Start(this.ShowCMD_BackupModal(title4, info4, delegate(bool result)
		{
			isPermit = result;
		}), false);
		if (!isPermit)
		{
			yield break;
		}
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		bool isSuccess = false;
		yield return AppCoroutine.Start(this.TakeOver(delegate(bool result)
		{
			isSuccess = result;
		}), false);
		RestrictionInput.EndLoad();
		title4 = StringMaster.GetString("TakeOverTitle");
		if (isSuccess)
		{
			info4 = StringMaster.GetString("TakeOver-11");
			yield return AppCoroutine.Start(this.ShowCMD_ModalMessage(title4, info4), false);
			GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
			yield break;
		}
		info4 = StringMaster.GetString("CloudTakeOverFailed");
		yield return AppCoroutine.Start(this.ShowCMD_ModalMessage(title4, info4), false);
		yield break;
	}

	private IEnumerator LoadBackupDataForiCloud()
	{
		if (this.ClearBackupMissionFlag)
		{
			yield break;
		}
		if (string.IsNullOrEmpty(this.inquiryCode))
		{
			yield return AppCoroutine.Start(this.GetInquiryCode(), false);
		}
		if (string.IsNullOrEmpty(this.inquiryCode))
		{
			yield break;
		}
		if (!NpSingleton<NpGameService>.Instance.IsSignedIn())
		{
			yield break;
		}
		yield return AppCoroutine.Start(this.LoadUserData(delegate(bool result)
		{
		}), false);
		if (string.IsNullOrEmpty(this.cloudUserCode) && string.IsNullOrEmpty(this.cloudInquiryCode))
		{
			yield break;
		}
		yield return AppCoroutine.Start(this.SendClearBackupMission(), false);
		if ((this.cloudUserCode == DataMng.Instance().RespDataCM_Login.playerInfo.userCode && this.cloudInquiryCode == this.inquiryCode) || (string.IsNullOrEmpty(this.cloudUserCode) && string.IsNullOrEmpty(this.cloudInquiryCode)))
		{
			yield break;
		}
		string userName = string.Empty;
		yield return AppCoroutine.Start(this.GetUserNameByUserID(this.cloudUserID, delegate(string result)
		{
			userName = result;
		}), false);
		RestrictionInput.EndLoad();
		bool isPermit = false;
		string title = StringMaster.GetString("CloudTakeOverTitle");
		string format = StringMaster.GetString("CloudTakeOverInfo");
		string info = string.Format(format, this.cloudUserCode, userName);
		yield return AppCoroutine.Start(this.ShowCMD_BackupModal_2(title, info, delegate(bool result)
		{
			isPermit = result;
		}), false);
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		if (!isPermit)
		{
			yield return AppCoroutine.Start(this.ClearUserData(), false);
			this.ClearBackupMissionFlag = false;
			yield break;
		}
		bool isSuccess = false;
		yield return AppCoroutine.Start(this.TakeOver(delegate(bool result)
		{
			isSuccess = result;
		}), false);
		RestrictionInput.EndLoad();
		title = StringMaster.GetString("TakeOverTitle");
		if (isSuccess)
		{
			info = StringMaster.GetString("CloudTakeOverSuccess");
			yield return AppCoroutine.Start(this.ShowCMD_ModalMessage(title, info), false);
			GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
		}
		else
		{
			info = StringMaster.GetString("CloudTakeOverFailed");
			yield return AppCoroutine.Start(this.ShowCMD_ModalMessage(title, info), false);
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		}
		yield break;
	}

	public Coroutine CheckClearMissionForGoogle()
	{
		return AppCoroutine.Start(this.CheckClearMissionForGoogle_(), false);
	}

	public IEnumerator CheckClearMissionForGoogle_()
	{
		if (this.ClearBackupMissionFlag)
		{
			yield break;
		}
		if (string.IsNullOrEmpty(this.inquiryCode))
		{
			yield return AppCoroutine.Start(this.GetInquiryCode(), false);
		}
		if (!GooglePlayGamesTool.Instance.IsSignIn)
		{
			yield return AppCoroutine.Start(this.SignInGoogle(), false);
		}
		if (!GooglePlayGamesTool.Instance.IsSignIn)
		{
			RestrictionInput.EndLoad();
			yield break;
		}
		yield return AppCoroutine.Start(this.LoadUserData(delegate(bool result)
		{
		}), false);
		if (!string.IsNullOrEmpty(this.cloudUserCode) && !string.IsNullOrEmpty(this.cloudInquiryCode) && this.cloudUserCode == DataMng.Instance().RespDataCM_Login.playerInfo.userCode && this.cloudInquiryCode == this.inquiryCode)
		{
			yield return AppCoroutine.Start(this.SendClearBackupMission(), false);
		}
		yield break;
	}

	private IEnumerator GetInquiryCode()
	{
		GameWebAPI.RequestCM_InquiryCodeRequest request = new GameWebAPI.RequestCM_InquiryCodeRequest
		{
			OnReceived = delegate(GameWebAPI.InquiryCodeRequest resData)
			{
				this.inquiryCode = resData.inquiryCode;
			}
		};
		return request.Run(null, null, null);
	}

	private IEnumerator SignInGoogle()
	{
		bool endFlag = false;
		GooglePlayGamesTool.Instance.SignIn(delegate(bool result)
		{
			endFlag = true;
		});
		while (!endFlag)
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator SaveUserData(Action<bool> OnResult)
	{
		bool isSuccess = false;
		yield return AppCoroutine.Start(this.SaveCloudData(this.USER_CODE_KEY, DataMng.Instance().RespDataCM_Login.playerInfo.userCode, delegate(bool result)
		{
			isSuccess = result;
		}), false);
		if (!isSuccess)
		{
			OnResult(false);
			yield break;
		}
		isSuccess = false;
		yield return AppCoroutine.Start(this.SaveCloudData(this.INQUIRY_CODE_KEY, this.inquiryCode, delegate(bool result)
		{
			isSuccess = result;
		}), false);
		if (!isSuccess)
		{
			OnResult(false);
			yield break;
		}
		isSuccess = false;
		yield return AppCoroutine.Start(this.SaveCloudData(this.USER_ID_KEY, DataMng.Instance().RespDataCM_Login.playerInfo.userId, delegate(bool result)
		{
			isSuccess = result;
		}), false);
		if (!isSuccess)
		{
			OnResult(false);
			yield break;
		}
		OnResult(true);
		yield break;
	}

	private IEnumerator ClearUserData()
	{
		yield return AppCoroutine.Start(this.SaveCloudData(this.USER_CODE_KEY, string.Empty, delegate(bool result)
		{
		}), false);
		yield return AppCoroutine.Start(this.SaveCloudData(this.INQUIRY_CODE_KEY, string.Empty, delegate(bool result)
		{
		}), false);
		yield return AppCoroutine.Start(this.SaveCloudData(this.USER_ID_KEY, string.Empty, delegate(bool result)
		{
		}), false);
		yield break;
	}

	private IEnumerator SaveCloudData(string KeyString, string SaveString, Action<bool> OnResult)
	{
		bool endFlag = false;
		bool isSuccess = false;
		NpSingleton<NpGameService>.Instance.DataSave(KeyString, SaveString, delegate
		{
			endFlag = true;
			isSuccess = true;
		}, delegate(string errorText)
		{
			endFlag = true;
			global::Debug.LogWarning("backup : " + errorText);
		});
		while (!endFlag)
		{
			yield return null;
		}
		OnResult(isSuccess);
		yield break;
	}

	private IEnumerator LoadUserData(Action<bool> OnResult)
	{
		bool isSuccess = false;
		yield return AppCoroutine.Start(this.LoadCloudData(this.USER_CODE_KEY, delegate(bool result, string loadData)
		{
			isSuccess = result;
			this.cloudUserCode = loadData;
		}), false);
		if (!isSuccess)
		{
			OnResult(false);
			yield break;
		}
		isSuccess = false;
		yield return AppCoroutine.Start(this.LoadCloudData(this.INQUIRY_CODE_KEY, delegate(bool result, string loadData)
		{
			isSuccess = result;
			this.cloudInquiryCode = loadData;
		}), false);
		if (!isSuccess)
		{
			OnResult(false);
			yield break;
		}
		isSuccess = false;
		yield return AppCoroutine.Start(this.LoadCloudData(this.USER_ID_KEY, delegate(bool result, string loadData)
		{
			isSuccess = result;
			this.cloudUserID = loadData;
		}), false);
		if (!isSuccess)
		{
			OnResult(false);
			yield break;
		}
		OnResult(true);
		yield break;
	}

	private IEnumerator LoadCloudData(string KeyString, Action<bool, string> OnResult)
	{
		bool endFlag = false;
		bool isSuccess = false;
		string loadText = string.Empty;
		NpSingleton<NpGameService>.Instance.DataLoad(KeyString, delegate(string loadData)
		{
			endFlag = true;
			isSuccess = true;
			loadText = loadData;
		}, delegate(string errorText)
		{
			endFlag = true;
		}, delegate(string keys)
		{
			endFlag = true;
		});
		while (!endFlag)
		{
			yield return null;
		}
		OnResult(isSuccess, loadText);
		yield break;
	}

	private IEnumerator TakeOver(Action<bool> OnResult)
	{
		bool isSuccess = false;
		GameWebAPI.RequestCM_TakeOverUseInquiryCode request = new GameWebAPI.RequestCM_TakeOverUseInquiryCode
		{
			SetSendData = delegate(GameWebAPI.ReqDataCM_TakeOverUseInquiryCode param)
			{
				param.inquiryCode = this.cloudInquiryCode;
				param.userCode = this.cloudUserCode;
			},
			OnReceived = delegate(GameWebAPI.RespDataCM_TakeOverUseInquiryCode resData)
			{
				isSuccess = (resData.transferStatus == 1);
			}
		};
		yield return AppCoroutine.Start(request.Run(null, null, null), false);
		OnResult(isSuccess);
		yield break;
	}

	private IEnumerator GetUserNameByUserID(string UserID, Action<string> OnResult)
	{
		int userID = int.Parse(UserID);
		string userName = string.Empty;
		GameWebAPI.RequestUS_UserProfile request = new GameWebAPI.RequestUS_UserProfile
		{
			SetSendData = delegate(GameWebAPI.PRF_Req_ProfileData param)
			{
				param.targetUserId = userID;
			},
			OnReceived = delegate(GameWebAPI.RespDataPRF_Profile resData)
			{
				userName = resData.userData.nickname;
			}
		};
		yield return AppCoroutine.Start(request.Run(null, null, null), false);
		OnResult(userName);
		yield break;
	}

	private IEnumerator SendClearBackupMission()
	{
		GameWebAPI.MissionClear request = new GameWebAPI.MissionClear
		{
			SetSendData = delegate(GameWebAPI.ReqDataUS_MissionClear param)
			{
				param.missionCategoryId = this.BACKUP_MISSION_ID;
			},
			OnReceived = delegate(GameWebAPI.RespDataMS_MissionClear resData)
			{
				if (resData.result == 1)
				{
					this.ClearBackupMissionFlag = true;
				}
			}
		};
		return request.Run(null, null, null);
	}

	private IEnumerator ShowCMD_ModalMessage(string TitleText, string InfoText)
	{
		bool isClose = false;
		CMD_ModalMessage cd = GUIMain.ShowCommonDialog(delegate(int index)
		{
			isClose = true;
		}, "CMD_ModalMessage") as CMD_ModalMessage;
		cd.Title = TitleText;
		cd.Info = InfoText;
		while (!isClose)
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator ShowCMD_BackupModal(string TitleText, string InfoText, Action<bool> OnClose)
	{
		bool isClose = false;
		bool pushYes = false;
		CMD_BackupModal cd = GUIMain.ShowCommonDialog(delegate(int index)
		{
			isClose = true;
			if (index == 1)
			{
				pushYes = true;
			}
		}, "CMD_BackupModal") as CMD_BackupModal;
		cd.Title = TitleText;
		cd.Info = InfoText;
		while (!isClose)
		{
			yield return null;
		}
		OnClose(pushYes);
		yield break;
	}

	private IEnumerator ShowCMD_BackupModal_2(string TitleText, string InfoText, Action<bool> OnClose)
	{
		bool isClose = false;
		bool pushYes = false;
		CMD_BackupModal cd = GUIMain.ShowCommonDialog(delegate(int index)
		{
			isClose = true;
			if (index != 1)
			{
				pushYes = true;
			}
		}, "CMD_BackupModal") as CMD_BackupModal;
		cd.Title = TitleText;
		cd.Info = InfoText;
		while (!isClose)
		{
			yield return null;
		}
		OnClose(pushYes);
		yield break;
	}
}
