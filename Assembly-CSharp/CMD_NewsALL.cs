using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_NewsALL : CMD
{
	public static CMD_NewsALL instance;

	private static readonly int DEFAULT_TAB = 1;

	[SerializeField]
	private GUISelectNewsPanel infoList;

	[SerializeField]
	private GameObject infoListItem;

	[SerializeField]
	private UILabel noneDiscription;

	protected override void Awake()
	{
		this.noneDiscription.text = "現在お知らせはありません。";
		base.Awake();
		CMD_NewsALL.instance = this;
	}

	protected override void OnDestroy()
	{
		CMD_NewsALL.instance = null;
		base.OnDestroy();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		APIRequestTask task = DataMng.Instance().RequestNews(false);
		base.StartCoroutine(task.Run(delegate
		{
			RestrictionInput.EndLoad();
			this.ShowDLG();
			this.InitNews(f, sizeX, sizeY, aT);
		}, delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(false);
		}, null));
	}

	private void InitNews(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.SetCommonUI();
		base.Show(f, sizeX, sizeY, aT);
		base.PartsTitle.SetTitle(StringMaster.GetString("InfomationTitle"));
		this.CreateInfoList(CMD_NewsALL.DEFAULT_TAB);
		base.MultiTab.InitMultiTab(new List<Action<int>>
		{
			new Action<int>(this.CreateInfoList),
			new Action<int>(this.CreateInfoList),
			new Action<int>(this.CreateInfoList)
		}, new List<string>
		{
			StringMaster.GetString("InfomationAll"),
			StringMaster.GetString("InfomationEvent"),
			StringMaster.GetString("InformationOperation")
		});
		base.MultiTab.SetOnOffColor(ConstValue.TAB_YELLOW, Color.white);
		base.MultiTab.SetFocus(CMD_NewsALL.DEFAULT_TAB);
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	public override void ClosePanel(bool animation = true)
	{
		ClassSingleton<PartsMenuNewsIconAccessor>.Instance.artsMenuNewsIcon.NewsCheck();
		this.CloseAndFarmCamOn(animation);
		if (this.infoList != null)
		{
			this.infoList.FadeOutAllListParts(null, false);
			this.infoList.SetHideScrollBarAllWays(true);
		}
	}

	private void SetCommonUI()
	{
		this.infoList.selectParts = this.infoListItem;
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -445f;
		listWindowViewRect.xMax = 445f;
		listWindowViewRect.yMin = -240f - GUIMain.VerticalSpaceSize;
		listWindowViewRect.yMax = 255f + GUIMain.VerticalSpaceSize;
		this.infoList.ListWindowViewRect = listWindowViewRect;
	}

	private void SetActiveNoneDiscription(int countInfo)
	{
		if (countInfo == 0)
		{
			this.noneDiscription.gameObject.SetActive(true);
		}
		else
		{
			this.noneDiscription.gameObject.SetActive(false);
		}
	}

	private void CreateInfoList(int tabNumber)
	{
		this.infoListItem.SetActive(true);
		this.infoList.initLocation = true;
		int activeNoneDiscription = this.infoList.AllBuild(DataMng.Instance().RespDataIN_InfoList, tabNumber);
		this.infoListItem.SetActive(false);
		this.SetActiveNoneDiscription(activeNoneDiscription);
	}
}
