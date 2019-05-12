using Master;
using System;
using UnityEngine;

public class CMD_BlockList : CMD
{
	[SerializeField]
	private GUISelectPanelFriend guiSelectPanelFriend;

	[SerializeField]
	private GameObject listParts;

	[SerializeField]
	private GameObject cautionText;

	[SerializeField]
	private UILabel cautionLabel;

	[SerializeField]
	private UILabel buttonLabel;

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

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_BlockList");
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		APIRequestTask task = BlockManager.instance().RequestBlockList(false);
		base.StartCoroutine(task.Run(delegate
		{
			RestrictionInput.EndLoad();
			this.ShowDLG();
			this.Initialize(f, sizeX, sizeY, aT);
		}, delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(false);
			GUICollider.EnableAllCollider("CMD_BlockList");
		}, null));
		this.cautionLabel.text = StringMaster.GetString("BlockListNone");
		this.buttonLabel.text = StringMaster.GetString("SystemButtonReturn");
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
	}

	public override void ClosePanel(bool animation = true)
	{
		this.guiSelectPanelFriend.SetHideScrollBarAllWays(true);
		this.guiSelectPanelFriend.FadeOutAllListParts(null, false);
		CMD_BlockList.instance = null;
		base.ClosePanel(animation);
	}

	private void Initialize(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
		if (base.PartsTitle != null)
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
		this.cautionText.SetActive(BlockManager.instance().blockList.Count == 0);
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
