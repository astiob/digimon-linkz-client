using System;

public class TipsLoading
{
	private static TipsLoading instance;

	private CMD_Tips cmdTips;

	private bool isLock;

	private bool isShow;

	private TipsLoading()
	{
	}

	public static TipsLoading Instance
	{
		get
		{
			if (TipsLoading.instance == null)
			{
				TipsLoading.instance = new TipsLoading();
			}
			return TipsLoading.instance;
		}
	}

	public bool IsShow
	{
		get
		{
			return this.isShow;
		}
	}

	public void StartTipsLoad(CMD_Tips.DISPLAY_PLACE DisplayPlace = CMD_Tips.DISPLAY_PLACE.TitleToFarm, bool loadPrefabPop = true)
	{
		if (!this.Lock())
		{
			return;
		}
		this.isShow = true;
		CMD_Tips.DisPlayPlace = DisplayPlace;
		this.cmdTips = (GUIMain.ShowCommonDialog(null, "CMD_Tips", null) as CMD_Tips);
	}

	public void StopTipsLoad(bool loadPrefabClose = true)
	{
		if (this.cmdTips == null)
		{
			return;
		}
		this.isShow = false;
		this.cmdTips.ClosePanel(false);
		this.cmdTips = null;
		this.UnLock();
	}

	private bool Lock()
	{
		if (this.isLock)
		{
			return false;
		}
		this.isLock = true;
		return true;
	}

	private void UnLock()
	{
		this.isLock = false;
	}
}
