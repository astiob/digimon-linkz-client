using Master;
using System;
using UnityEngine;

public sealed class CMD_BlockList : CMD
{
	[SerializeField]
	private GUISelectPanelFriend guiSelectPanelFriend;

	[SerializeField]
	private GameObject listParts;

	[SerializeField]
	private GameObject cautionText;

	private static CMD_BlockList instance;

	public static CMD_BlockList Instance
	{
		get
		{
			return CMD_BlockList.instance;
		}
	}

	protected override void Awake()
	{
		CMD_BlockList.instance = this;
		base.Awake();
	}

	public override void Show(Action<int> closeEvent, float sizeX, float sizeY, float showAnimationTime)
	{
		GUICollider.DisableAllCollider("CMD_BlockList");
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		APIRequestTask task = BlockManager.instance().RequestBlockList(false);
		base.StartCoroutine(task.Run(delegate
		{
			RestrictionInput.EndLoad();
			this.ShowDLG();
			this.Initialize(closeEvent, sizeX, sizeY, showAnimationTime);
		}, delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(false);
			GUICollider.EnableAllCollider("CMD_BlockList");
		}, null));
	}

	public override void ClosePanel(bool animation = true)
	{
		this.guiSelectPanelFriend.SetHideScrollBarAllWays(true);
		this.guiSelectPanelFriend.FadeOutAllListParts(null, false);
		CMD_BlockList.instance = null;
		base.ClosePanel(animation);
	}

	private void Initialize(Action<int> closeEvent, float sizeX, float sizeY, float showAnimationTime)
	{
		base.Show(closeEvent, sizeX, sizeY, showAnimationTime);
		if (null != base.PartsTitle)
		{
			base.PartsTitle.SetTitle(StringMaster.GetString("BlockListTitle"));
		}
		this.BuildBlockList();
		GUICollider.EnableAllCollider("CMD_BlockList");
	}

	public void BuildBlockList()
	{
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -568f;
		listWindowViewRect.xMax = 568f;
		listWindowViewRect.yMin = -238f - GUIMain.VerticalSpaceSize;
		listWindowViewRect.yMax = 260f + GUIMain.VerticalSpaceSize;
		this.guiSelectPanelFriend.ListWindowViewRect = listWindowViewRect;
		this.guiSelectPanelFriend.selectParts = this.listParts;
		this.cautionText.SetActive(0 == BlockManager.instance().blockList.Count);
		this.listParts.SetActive(true);
		this.guiSelectPanelFriend.initLocation = true;
		GUISelectPanelFriend guiselectPanelFriend = this.guiSelectPanelFriend;
		Action cb = delegate()
		{
			this.listParts.SetActive(false);
		};
		base.StartCoroutine(guiselectPanelFriend.AllBuild(BlockManager.instance().blockList, 0f, 0f, cb));
	}

	public void OnClickReturn()
	{
		this.ClosePanel(true);
	}
}
