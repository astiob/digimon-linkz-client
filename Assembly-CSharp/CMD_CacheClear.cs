using Master;
using System;
using UnityEngine;

public class CMD_CacheClear : CMD
{
	[SerializeField]
	private BoxCollider yesButton;

	[SerializeField]
	private BoxCollider noButton;

	private int frameCount;

	private Func<bool> actionCasheClear;

	public Action onSuccessCacheClear;

	protected override void Update()
	{
		base.Update();
		if (this.actionCasheClear != null)
		{
			this.frameCount--;
			if (0 >= this.frameCount)
			{
				bool result = this.actionCasheClear();
				this.actionCasheClear = null;
				this.OpenResultMessage(result);
			}
		}
	}

	private void SetButtonEnable(bool enable)
	{
		this.yesButton.enabled = enable;
		this.noButton.enabled = enable;
	}

	private bool CacheClear()
	{
		bool result = false;
		try
		{
			string[] ignoreFileNameList = new string[]
			{
				"SAVE_DT.binary",
				"SUB_SAVE_DT.binary"
			};
			GameCache.ClearCache(ignoreFileNameList);
			Caching.CleanCache();
			result = true;
		}
		catch
		{
			result = false;
		}
		RestrictionInput.EndLoad();
		return result;
	}

	private void OpenResultMessage(bool result)
	{
		if (result)
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(new Action<int>(this.OnClosedSuccessModalMessage), "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("CacheClearTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("CacheClearSuccess");
		}
		else
		{
			CMD_ModalMessage cmd_ModalMessage2 = GUIMain.ShowCommonDialog(new Action<int>(this.OnClosedFailedModalMessage), "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage2.Title = StringMaster.GetString("CacheClearTitle");
			cmd_ModalMessage2.Info = StringMaster.GetString("CacheClearFailed");
		}
	}

	private void StartCachelear()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		this.frameCount = 5;
		this.actionCasheClear = new Func<bool>(this.CacheClear);
	}

	private void OnClosedSuccessModalMessage(int noop)
	{
		base.ClosePanel(true);
		if (this.onSuccessCacheClear != null)
		{
			this.onSuccessCacheClear();
			this.onSuccessCacheClear = null;
		}
	}

	private void OnClosedFailedModalMessage(int noop)
	{
		this.SetButtonEnable(true);
	}

	public void OnClickYesCacheClear()
	{
		this.SetButtonEnable(false);
		this.StartCachelear();
	}

	public void OnClickNoCacheClear()
	{
		this.SetButtonEnable(false);
		base.ClosePanel(true);
	}
}
