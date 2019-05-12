using Master;
using System;
using UnityEngine;

public class CMD_HelpCategory : CMD
{
	[SerializeField]
	private GameObject partsParent;

	[SerializeField]
	private GameObject listParts;

	private GUISelectHelpPanel csPartParent;

	public static GameWebAPI.RespDataMA_GetHelpCategoryM Data { get; set; }

	protected override void Awake()
	{
		base.Awake();
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

	protected override void Update()
	{
		base.Update();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("SystemHelp"));
		this.csPartParent = this.partsParent.GetComponent<GUISelectHelpPanel>();
		this.csPartParent.AllBuildCategory(CMD_HelpCategory.Data);
		this.listParts.SetActive(false);
		base.Show(f, sizeX, sizeY, aT);
	}

	private void FakeMethod()
	{
	}

	public override void ClosePanel(bool animation = true)
	{
		this.CloseAndFarmCamOn(animation);
		if (this.csPartParent != null)
		{
			this.csPartParent.FadeOutAllListParts(null, false);
			this.csPartParent.SetHideScrollBarAllWays(true);
		}
	}
}
