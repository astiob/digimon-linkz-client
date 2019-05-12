using Master;
using Save;
using System;
using UnityEngine;

public static class AlertManager
{
	public const string LocalErrorCodeTimeOut = "LOCAL_ERROR_TIMEOUT";

	public const string LocalErrorCodeWWW = "LOCAL_ERROR_WWW";

	public const string LocalErrorCodeJsonParse = "LOCAL_ERROR_JSONPARSE";

	public const string LocalErrorAssetData = "LOCAL_ERROR_ASSET_DATA";

	public const string LocalErrorSaveDataIO = "LOCAL_ERROR_SAVE_DATA_IO";

	public const string LocalErrorSaveDataOther = "LOCAL_ERROR_SAVE_DATA_OTHER";

	public const string LocalErrorSaveDataSecurity = "LOCAL_ERROR_SAVE_DATA_SECURITY";

	public const string LocalErrorTakeOver1 = "E-AL09";

	public const string LocalErrorTakeOver2 = "E-AL10";

	public const string NameInputOverError = "E-US08";

	public const string NameInputRuleError = "E-US17";

	public const string BannerDownloadError = "C-AL01";

	public const string DeviceAccountError = "C-NP200";

	public const string StoreMaintenance = "C-SH05";

	public const string StoreBuyError = "C-SH02";

	public const string AgeRegistError = "C-SH03";

	public const string AgeCheckError = "C-SH04";

	public const string DigistoneOverError = "C-SH06";

	public const string FriendSearchError = "C-US02";

	public const string CaptureFindError = "C-GA01";

	public const string NameInvalidError = "E-US17";

	public const string CommentInputOverError = "E-US18";

	public const string NameEmptyError = "E-US07";

	public const string ProfileGetError = "C-US01";

	public const string InMaintenance = "E-AL15";

	public static Action alertOpenedAction;

	public static Action<bool, CMD_Alert> onCreateAlert;

	private static string lastErrorCode;

	public static bool CheckDialogMessage(string errorCode)
	{
		if (string.IsNullOrEmpty(errorCode))
		{
			return false;
		}
		if (errorCode == "LOCAL_ERROR_TIMEOUT" || errorCode == "LOCAL_ERROR_WWW" || errorCode == "LOCAL_ERROR_JSONPARSE" || errorCode == "LOCAL_ERROR_ASSET_DATA" || errorCode == "LOCAL_ERROR_SAVE_DATA_IO" || errorCode == "LOCAL_ERROR_SAVE_DATA_OTHER" || errorCode == "LOCAL_ERROR_SAVE_DATA_SECURITY" || errorCode == "E-AL09" || errorCode == "E-AL10")
		{
			return true;
		}
		if (!string.IsNullOrEmpty(AlertManager.GetNeptuneErrorString(errorCode)))
		{
			return true;
		}
		GameWebAPI.RespDataMA_MessageM.MessageM alert = AlertMaster.GetAlert(errorCode);
		return null != alert;
	}

	public static bool ShowAlertDialog(Action<int> action, string errorCode)
	{
		AlertManager.lastErrorCode = errorCode;
		if (errorCode == "LOCAL_ERROR_TIMEOUT")
		{
			string @string = StringMaster.GetString("AlertNetworkErrorTitle");
			string string2 = StringMaster.GetString("AlertNetworkErrorTimeOut");
			return AlertManager.ShowAlertDialog(action, @string, string2, AlertManager.ButtonActionType.Retry, true);
		}
		if (errorCode == "LOCAL_ERROR_WWW")
		{
			string string3 = StringMaster.GetString("AlertDataErrorTitle");
			string string4 = StringMaster.GetString("AlertDataErrorInfo");
			return AlertManager.ShowAlertDialog(action, string3, string4, AlertManager.ButtonActionType.Retry, true);
		}
		if (errorCode == "LOCAL_ERROR_JSONPARSE")
		{
			string string5 = StringMaster.GetString("SaveFailedTitle");
			string string6 = StringMaster.GetString("AlertJsonErrorInfo");
			return AlertManager.ShowAlertDialog(action, string5, string6, AlertManager.ButtonActionType.Title, true);
		}
		if (errorCode == "LOCAL_ERROR_ASSET_DATA")
		{
			string string7 = StringMaster.GetString("AlertNetworkErrorTitle");
			string string8 = StringMaster.GetString("AlertNetworkErrorRetry");
			return AlertManager.ShowAlertDialog(action, string7, string8, AlertManager.ButtonActionType.Retry, true);
		}
		if (errorCode == "LOCAL_ERROR_SAVE_DATA_IO")
		{
			string string9 = StringMaster.GetString("SaveFailedTitle");
			string string10 = StringMaster.GetString("SaveFailed-02");
			return AlertManager.ShowAlertDialog(action, string9, string10, AlertManager.ButtonActionType.Close, true);
		}
		if (errorCode == "LOCAL_ERROR_SAVE_DATA_OTHER")
		{
			string string11 = StringMaster.GetString("SaveFailedTitle");
			string string12 = StringMaster.GetString("SaveFailed-01");
			return AlertManager.ShowAlertDialog(action, string11, string12, AlertManager.ButtonActionType.Close, true);
		}
		if (errorCode == "LOCAL_ERROR_SAVE_DATA_SECURITY")
		{
			string string13 = StringMaster.GetString("SaveFailedTitle");
			string string14 = StringMaster.GetString("SaveFailed-03");
			return AlertManager.ShowAlertDialog(action, string13, string14, AlertManager.ButtonActionType.Close, true);
		}
		if (errorCode == "E-AL09")
		{
			string string15 = StringMaster.GetString("TakeOver-12");
			string string16 = StringMaster.GetString("TakeOver-13");
			return AlertManager.ShowAlertDialog(action, string15, string16, AlertManager.ButtonActionType.Close, true);
		}
		if (errorCode == "E-AL10")
		{
			string string17 = StringMaster.GetString("TakeOver-12");
			string string18 = StringMaster.GetString("TakeOver-13");
			return AlertManager.ShowAlertDialog(action, string17, string18, AlertManager.ButtonActionType.Close, true);
		}
		if (!string.IsNullOrEmpty(AlertManager.GetNeptuneErrorString(errorCode)))
		{
			return AlertManager.ShowAlertDialog(action, AlertManager.GetNeptuneErrorTitle(errorCode), AlertManager.GetNeptuneErrorString(errorCode), AlertManager.ButtonActionType.Close, true);
		}
		GameWebAPI.RespDataMA_MessageM.MessageM alert = AlertMaster.GetAlert(errorCode);
		if (alert == null)
		{
			alert = AlertMaster.GetAlert("E-GP01");
		}
		if (alert == null)
		{
			return AlertManager.ErrorCallback(action, errorCode);
		}
		AlertManager.ButtonActionType actionType = (AlertManager.ButtonActionType)int.Parse(alert.actionType);
		AlertManager.DialogType dialogType = (AlertManager.DialogType)int.Parse(alert.actionValue);
		AlertManager.DialogType dialogType2 = dialogType;
		if (dialogType2 == AlertManager.DialogType.Alert)
		{
			return AlertManager.ShowAlertDialog(action, alert.messageTitle, alert.messageText, actionType, true);
		}
		if (dialogType2 != AlertManager.DialogType.Modal)
		{
			return AlertManager.ShowAlertDialog(action, alert.messageTitle, alert.messageText, actionType, true);
		}
		return AlertManager.ShowModalMessage(action, alert.messageTitle, alert.messageText, actionType, true);
	}

	public static bool ShowAlertDialog(Action<int> action, string title, string message, AlertManager.ButtonActionType actionType = AlertManager.ButtonActionType.Close, bool useErrorCode = false)
	{
		return AlertManager.ShowDialog<CMD_Alert>(action, title, message, "CMD_Alert", actionType, true, useErrorCode);
	}

	public static bool ShowNoWarningIconAlertDialog(Action<int> action, string title, string message, AlertManager.ButtonActionType actionType = AlertManager.ButtonActionType.Close)
	{
		return AlertManager.ShowDialog<CMD_Alert>(action, title, message, "CMD_Alert", actionType, false, false);
	}

	public static bool ShowMaintenanceDialog(Action<int> action, string message)
	{
		return AlertManager.ShowDialog<CMD_maintenance>(action, string.Empty, message, "CMD_maintenance", AlertManager.ButtonActionType.Close, true, false);
	}

	public static bool ShowModalMessage(Action<int> action, string title, string message, AlertManager.ButtonActionType actionType = AlertManager.ButtonActionType.Close, bool useErrorCode = false)
	{
		return AlertManager.ShowDialog<CMD_ModalMessage>(action, title, message, "CMD_ModalMessage", actionType, true, useErrorCode);
	}

	private static bool ShowDialog<CMD_Type>(Action<int> action, string title, string message, string dialogName = "", AlertManager.ButtonActionType actionType = AlertManager.ButtonActionType.Close, bool isWarningIcon = true, bool useErrorCode = false) where CMD_Type : CMD
	{
		Action<int> action2 = delegate(int i)
		{
			if (action != null)
			{
				action(i);
			}
		};
		if (AlertManager.alertOpenedAction != null)
		{
			AlertManager.alertOpenedAction();
		}
		if (!useErrorCode)
		{
			AlertManager.lastErrorCode = string.Empty;
		}
		PlayerPrefs.SetString("LastErrorInfo", ServerDateTime.Now + ":" + AlertManager.lastErrorCode);
		CMD_Type cmd_Type = GUIManager.ShowCommonDialog(action2, true, dialogName, null, null, 0.2f, 0f, 0f, -1f, -1f) as CMD_Type;
		if (cmd_Type != null)
		{
			string text = typeof(CMD_Type).ToString();
			switch (text)
			{
			case "CMD_Alert":
			{
				CMD_Alert cmd_Alert = cmd_Type as CMD_Alert;
				cmd_Alert.Title = title;
				cmd_Alert.Info = message;
				cmd_Alert.IsWarning = isWarningIcon;
				switch (actionType)
				{
				case AlertManager.ButtonActionType.Close:
					cmd_Alert.SetDisplayButton(CMD_Alert.DisplayButton.CLOSE);
					break;
				case AlertManager.ButtonActionType.Retry:
					cmd_Alert.SetDisplayButton(CMD_Alert.DisplayButton.RETRY);
					break;
				case AlertManager.ButtonActionType.TitleAndRetry:
					cmd_Alert.SetDisplayButton(CMD_Alert.DisplayButton.TITLE_AND_RETRY);
					break;
				case AlertManager.ButtonActionType.Title:
					cmd_Alert.SetDisplayButton(CMD_Alert.DisplayButton.TITLE);
					break;
				}
				AlertManager.ExecuteOnCreateAlert(true, cmd_Alert);
				break;
			}
			case "CMD_maintenance":
			{
				CMD_maintenance cmd_maintenance = cmd_Type as CMD_maintenance;
				cmd_maintenance.Info = message;
				break;
			}
			case "CMD_ModalMessage":
			{
				CMD_ModalMessage cmd_ModalMessage = cmd_Type as CMD_ModalMessage;
				cmd_ModalMessage.Title = title;
				cmd_ModalMessage.Info = message;
				AlertManager.ExecuteOnCreateAlert(true, null);
				break;
			}
			}
			return true;
		}
		if (action != null)
		{
			action(0);
		}
		UnityEngine.Debug.Log("Create Failed CMD : " + title + ", " + message);
		return false;
	}

	private static void ExecuteOnCreateAlert(bool isCreate, CMD_Alert alert)
	{
		if (AlertManager.onCreateAlert != null)
		{
			AlertManager.onCreateAlert(isCreate, alert);
			AlertManager.onCreateAlert = null;
		}
	}

	private static bool ErrorCallback(Action<int> callback, string errorCode)
	{
		if (callback != null)
		{
			callback(0);
		}
		UnityEngine.Debug.Log("Not Find Error Code : " + errorCode);
		return false;
	}

	public static string GetErrorCode(WWWResponse.LocalErrorStatus localErrorStatus)
	{
		switch (localErrorStatus)
		{
		case WWWResponse.LocalErrorStatus.LOCAL_ERROR_TIMEOUT:
			return "LOCAL_ERROR_TIMEOUT";
		case WWWResponse.LocalErrorStatus.LOCAL_ERROR_WWW:
			return "LOCAL_ERROR_WWW";
		case WWWResponse.LocalErrorStatus.LOCAL_ERROR_JSONPARSE:
			return "LOCAL_ERROR_JSONPARSE";
		default:
			return string.Empty;
		}
	}

	public static string GetErrorCode(PersistentFile.ErrorType errorType)
	{
		switch (errorType)
		{
		case PersistentFile.ErrorType.IO:
			return "LOCAL_ERROR_SAVE_DATA_IO";
		case PersistentFile.ErrorType.SECURITY:
			return "LOCAL_ERROR_SAVE_DATA_SECURITY";
		case PersistentFile.ErrorType.OTHER:
			return "LOCAL_ERROR_SAVE_DATA_OTHER";
		default:
			return string.Empty;
		}
	}

	public static string GetNeptuneErrorCode(string neptuneError)
	{
		return "C-NP" + neptuneError;
	}

	private static string GetNeptuneErrorString(string errorCode)
	{
		string result = string.Empty;
		GameWebAPI.RespDataMA_MessageM.MessageM alert = AlertMaster.GetAlert(errorCode);
		if (alert != null)
		{
			return result;
		}
		string text = errorCode.Replace("C-NP", string.Empty);
		switch (text)
		{
		case "100":
			result = StringMaster.GetString("AlertJsonErrorInfo");
			break;
		case "101":
			result = StringMaster.GetString("ShopFailed-01");
			break;
		case "102":
			result = StringMaster.GetString("ShopFailed-06");
			break;
		case "103":
			result = StringMaster.GetString("ShopFailed-07");
			break;
		case "104":
			result = StringMaster.GetString("ShopFailed-05");
			break;
		case "105":
			result = StringMaster.GetString("ShopFailed-08");
			break;
		case "106":
			result = StringMaster.GetString("ShopFailed-10");
			break;
		case "107":
			result = StringMaster.GetString("ShopFailed-02");
			break;
		case "120":
			result = StringMaster.GetString("ShopFailed-11");
			break;
		case "199":
			result = StringMaster.GetString("ShopFailed-04");
			break;
		case "200":
			result = StringMaster.GetString("ShopFailed-03");
			break;
		case "201":
			result = StringMaster.GetString("ShopFailed-13");
			break;
		case "203":
			result = StringMaster.GetString("ShopFailed-12");
			break;
		}
		return result;
	}

	private static string GetNeptuneErrorTitle(string errorCode)
	{
		string empty = string.Empty;
		GameWebAPI.RespDataMA_MessageM.MessageM alert = AlertMaster.GetAlert(errorCode);
		if (alert != null)
		{
			return empty;
		}
		return StringMaster.GetString("ShopFailedTitle");
	}

	private enum DialogType
	{
		Alert = 1,
		Modal
	}

	public enum ButtonActionType
	{
		Close = 1,
		Retry,
		TitleAndRetry,
		Title
	}
}
