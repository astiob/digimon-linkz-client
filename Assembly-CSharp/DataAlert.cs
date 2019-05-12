using Master;
using Save;
using System;
using System.Collections;

public class DataAlert
{
	private bool isOpen;

	public IEnumerator OpenAlert(PersistentFile.ErrorType error)
	{
		string errorCode = AlertManager.GetErrorCode(error);
		this.isOpen = AlertManager.ShowAlertDialog(new Action<int>(this.OnClose), errorCode);
		while (this.isOpen)
		{
			yield return null;
		}
		yield break;
	}

	public IEnumerator OpenAlertJsonParseError()
	{
		this.isOpen = AlertManager.ShowAlertDialog(new Action<int>(this.OnClose), StringMaster.GetString("AlertDataErrorTitle"), StringMaster.GetString("AlertJsonErrorInfo"), AlertManager.ButtonActionType.Title, false);
		while (this.isOpen)
		{
			yield return null;
		}
		yield break;
	}

	private void OnClose(int selectButtonIndex)
	{
		if (selectButtonIndex != 3)
		{
			this.isOpen = false;
		}
		else
		{
			if (TipsLoading.Instance.IsShow)
			{
				TipsLoading.Instance.StopTipsLoad(true);
			}
			GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
		}
	}
}
