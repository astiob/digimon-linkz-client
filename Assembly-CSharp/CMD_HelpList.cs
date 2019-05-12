using Master;
using System;
using UnityEngine;

public class CMD_HelpList : CMD
{
	[SerializeField]
	private GameObject partsParent;

	[SerializeField]
	private GameObject listParts;

	public static string helpCategoryId;

	private GUISelectHelpPanel csPartParent;

	public static GameWebAPI.RespDataMA_GetHelpM Data { get; set; }

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
		base.Show(f, sizeX, sizeY, aT);
		base.PartsTitle.SetTitle(StringMaster.GetString("SystemHelp"));
		this.csPartParent = this.partsParent.GetComponent<GUISelectHelpPanel>();
		this.csPartParent.AllBuildList(CMD_HelpList.Data, CMD_HelpList.helpCategoryId);
		this.listParts.SetActive(false);
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
