using Master;
using System;
using UnityEngine;

public sealed class CMD_ModalItemPackDetail : CMD
{
	[SerializeField]
	private UILabel lbTX_TITLE;

	[SerializeField]
	private UILabel lbTX_AVAILABLE_NUM;

	[SerializeField]
	private GUISelectPanelViewPartsUD csSelectPanel_Item;

	[SerializeField]
	private GUISelectPanelViewPartsUD csSelectPanel_Omake;

	public static StoreUtil.StoneStoreData Data { get; set; }

	public GameWebAPI.RespDataSH_Info.AcquireList GetItemDataByIDX(int idx)
	{
		return CMD_ModalItemPackDetail.Data.itemList[idx];
	}

	public GameWebAPI.RespDataSH_Info.AcquireList GetOmakeDataByIDX(int idx)
	{
		return CMD_ModalItemPackDetail.Data.omakeList[idx];
	}

	protected override void Awake()
	{
		base.Awake();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.ShowDetail();
		this.ShowList();
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	private void OnCloseButton()
	{
		this.ClosePanel(true);
	}

	private void ShowDetail()
	{
		this.lbTX_TITLE.text = CMD_ModalItemPackDetail.Data.productTitle;
		if (CMD_ModalItemPackDetail.Data.limitCount > 0)
		{
			this.lbTX_AVAILABLE_NUM.gameObject.SetActive(true);
			this.lbTX_AVAILABLE_NUM.text = string.Format(StringMaster.GetString("StorePackItemBuyLimit"), CMD_ModalItemPackDetail.Data.limitCount - CMD_ModalItemPackDetail.Data.purchasedCount, CMD_ModalItemPackDetail.Data.limitCount);
		}
		else
		{
			this.lbTX_AVAILABLE_NUM.gameObject.SetActive(false);
		}
	}

	private void ShowList()
	{
		this.csSelectPanel_Item.ZPos = -15f;
		this.csSelectPanel_Omake.ZPos = -40f;
		this.csSelectPanel_Item.AllBuild(CMD_ModalItemPackDetail.Data.itemList.Count, true, 1f, 1f, null, this);
		this.csSelectPanel_Omake.AllBuild(CMD_ModalItemPackDetail.Data.omakeList.Count, true, 1f, 1f, null, this);
	}
}
