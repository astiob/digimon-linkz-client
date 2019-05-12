using Master;
using System;
using System.Collections;
using UnityEngine;

public sealed class APIAlert
{
	public Action ClosedAction;

	private bool isOpen;

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
			this.ShowJsonParseError();
		}
		else
		{
			switch (error.responseDataError.venus_status)
			{
			case 3:
				this.ShowServerMaintenance(error.responseDataError.message);
				return;
			case 5:
				this.ShowVersionError(error.responseDataError);
				return;
			}
			this.ShowJsonParseError();
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
			cmd_maintenance = (GUIMain.ShowCommonDialog(null, "CMD_maintenance") as CMD_maintenance);
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
			this.ClosedAction = new Action(Loading.ResumeDisplay);
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
		switch (selectButtonIndex)
		{
		case 1:
			this.SelectedRetryButton = true;
			if (this.ClosedAction != null)
			{
				this.ClosedAction();
			}
			break;
		case 2:
			if (this.ClosedAction != null)
			{
				this.ClosedAction();
			}
			break;
		case 3:
			this.ToTop();
			break;
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
}
