using Master;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class APIAlert
{
	public Action ClosedAction;

	private bool isOpen;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	public bool IsOpen()
	{
		return this.isOpen;
	}

	public IEnumerator WaitClose()
	{
		while (this.isOpen)
		{
			yield return null;
		}
		yield break;
	}

	public bool SelectedRetryButton { get; private set; }

	public void NetworkAPIError(WebAPIException error, bool isRetry)
	{
		string errorCode = string.Empty;
		string message = string.Empty;
		if (error.localErrorStatus != WWWResponse.LocalErrorStatus.NONE)
		{
			this.SetJsonParseExceptionMessage(error);
			errorCode = AlertManager.GetErrorCode(error.localErrorStatus);
		}
		else
		{
			errorCode = error.responseDataError.subject;
			message = error.responseDataError.message;
		}
		this.SelectedRetryButton = false;
		this.Open(errorCode, StringMaster.GetString("AlertDataErrorTitle"), message, isRetry, new Action<int>(this.OnClosed), null);
	}

	public void NetworkAPIException(WebAPIException error)
	{
		if (Loading.IsShow())
		{
			Loading.Invisible();
		}
		if (error.responseDataError == null)
		{
			this.SetJsonParseExceptionMessage(error);
			this.ShowJsonParseError();
		}
		else
		{
			WWWResponse.VenusStatus venus_status = (WWWResponse.VenusStatus)error.responseDataError.venus_status;
			if (venus_status != WWWResponse.VenusStatus.RESPONSE_MAINTENANCE)
			{
				if (venus_status != WWWResponse.VenusStatus.RESPONSE_OLDVERSION)
				{
					this.SetJsonParseExceptionMessage(error);
					this.ShowJsonParseError();
				}
				else
				{
					this.ShowVersionError(error.responseDataError);
				}
			}
			else
			{
				this.ShowServerMaintenance(error.responseDataError.message);
			}
		}
	}

	private void ShowServerMaintenance(string maintenanceMessage)
	{
		CMD_maintenance cmd_maintenance = (CMD_maintenance)GUIManager.CheckTopDialog("CMD_maintenance", null);
		if (cmd_maintenance != null)
		{
			RestrictionInput.EndLoad();
			cmd_maintenance.Info = maintenanceMessage;
			if (!AlertManager.ShowAlertDialog(null, "E-AL15"))
			{
				AlertManager.ShowAlertDialog(null, StringMaster.GetString("Maintenance-01"), StringMaster.GetString("Maintenance-02"), AlertManager.ButtonActionType.Close, false);
			}
		}
		else
		{
			cmd_maintenance = (GUIMain.ShowCommonDialog(null, "CMD_maintenance", null) as CMD_maintenance);
			cmd_maintenance.Info = maintenanceMessage;
		}
	}

	private void ShowVersionError(WebAPI.ResponseDataErr errorData)
	{
		Action<int> onClosed = null;
		Func<CMD_Alert.ExtraFunctionReturnValue> onAlertButton = null;
		if (errorData.GetAPPVer() != null)
		{
			onAlertButton = new Func<CMD_Alert.ExtraFunctionReturnValue>(this.NewApplication);
		}
		else if (errorData.GetDATAVer() != null || errorData.GetASSETVer() != null)
		{
			onClosed = new Action<int>(this.BackToTop);
		}
		else if (!string.IsNullOrEmpty(errorData.GetPolicyVersion()))
		{
			onClosed = new Action<int>(this.BackToTop);
		}
		else
		{
			global::Debug.LogError("VenusStatus.RESPONSE_OLDVERSION なのにバージョン情報が無い");
			onClosed = new Action<int>(this.BackToTop);
		}
		this.Open(errorData.subject, string.Empty, errorData.message, false, onClosed, onAlertButton);
	}

	private void ShowJsonParseError()
	{
		string errorCode = AlertManager.GetErrorCode(WWWResponse.LocalErrorStatus.LOCAL_ERROR_JSONPARSE);
		this.Open(errorCode, string.Empty, string.Empty, false, new Action<int>(this.BackToTop), null);
	}

	private void Open(string errorCode, string title, string message, bool isRetry, Action<int> onClosed, Func<CMD_Alert.ExtraFunctionReturnValue> onAlertButton)
	{
		if (Loading.IsShow())
		{
			Loading.Invisible();
			if (APIAlert.<>f__mg$cache0 == null)
			{
				APIAlert.<>f__mg$cache0 = new Action(Loading.ResumeDisplay);
			}
			this.ClosedAction = APIAlert.<>f__mg$cache0;
		}
		AlertManager.onCreateAlert = delegate(bool isCreate, CMD_Alert alert)
		{
			if (isCreate)
			{
				this.isOpen = true;
				if (null != alert && onAlertButton != null)
				{
					alert.SetActionButtonExtraFunction(onAlertButton);
				}
			}
		};
		if (AlertManager.CheckDialogMessage(errorCode))
		{
			AlertManager.ShowAlertDialog(onClosed, errorCode);
		}
		else
		{
			AlertManager.ButtonActionType actionType = AlertManager.ButtonActionType.Close;
			if (isRetry)
			{
				actionType = AlertManager.ButtonActionType.Retry;
			}
			AlertManager.ShowAlertDialog(onClosed, title, message, actionType, false);
		}
	}

	private void OnClosed(int selectButtonIndex)
	{
		this.isOpen = false;
		if (selectButtonIndex != 2)
		{
			if (selectButtonIndex != 1)
			{
				if (selectButtonIndex == 3)
				{
					this.ToTop();
				}
			}
			else
			{
				this.SelectedRetryButton = true;
				if (this.ClosedAction != null)
				{
					this.ClosedAction();
				}
			}
		}
		else if (this.ClosedAction != null)
		{
			this.ClosedAction();
		}
	}

	private void BackToTop(int selectButtonIndex)
	{
		this.ClosedAction = null;
		this.OnClosed(selectButtonIndex);
	}

	private void ToTop()
	{
		if (TipsLoading.Instance.IsShow)
		{
			TipsLoading.Instance.StopTipsLoad(true);
		}
		GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
	}

	private CMD_Alert.ExtraFunctionReturnValue NewApplication()
	{
		Application.OpenURL(ConstValue.STORE_SITE_URL);
		return CMD_Alert.ExtraFunctionReturnValue.NOT_CLOSE;
	}

	private void SetJsonParseExceptionMessage(WebAPIException error)
	{
	}
}
