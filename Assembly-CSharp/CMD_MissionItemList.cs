using System;
using UnityEngine;

public class CMD_MissionItemList : CMD
{
	[SerializeField]
	[Header("タイトルラベル")]
	private UILabel lbTitle;

	[Header("リストパーツ")]
	[SerializeField]
	private GameObject partListParent;

	[SerializeField]
	private GameObject partRewardList;

	private GUISelectRewardListPanel csPartRewardListParent;

	private static CMD_MissionItemList instance;

	public static GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission missionInfo { private get; set; }

	public static CMD_MissionItemList Instance
	{
		get
		{
			return CMD_MissionItemList.instance;
		}
	}

	protected override void Awake()
	{
		CMD_MissionItemList.instance = this;
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.HideDLG();
		this.SetCommonUI();
		this.InitRewardList(f, sizeX, sizeY, aT);
	}

	private void SetCommonUI()
	{
		this.csPartRewardListParent = this.partListParent.GetComponent<GUISelectRewardListPanel>();
	}

	private void InitRewardList(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.csPartRewardListParent.selectParts = this.partRewardList;
		this.csPartRewardListParent.initLocation = true;
		this.csPartRewardListParent.AllBuild(CMD_MissionItemList.missionInfo);
		this.partRewardList.SetActive(false);
		base.ShowDLG();
		base.Show(f, sizeX, sizeY, aT);
	}
}
