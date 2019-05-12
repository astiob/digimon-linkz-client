using Master;
using System;
using UnityEngine;

public class GUIListPartsPackItem : GUIListPartBS
{
	[Header("アイコン")]
	[SerializeField]
	private UITexture ItemIcon;

	[Header("アイテムネーム")]
	[SerializeField]
	private UILabel lbTX_ItemName;

	[Header("アイテム個数")]
	[SerializeField]
	private UILabel lbTX_ItemNum;

	[Header("オマケフラグ")]
	[SerializeField]
	private bool isOmake;

	[SerializeField]
	private PresentBoxItem rewards;

	public static GameWebAPI.RespDataSH_Info.AcquireList Data { get; set; }

	public override void SetData()
	{
		CMD_ModalItemPackDetail cmd_ModalItemPackDetail = (CMD_ModalItemPackDetail)base.GetInstanceCMD();
		if (!this.isOmake)
		{
			GUIListPartsPackItem.Data = cmd_ModalItemPackDetail.GetItemDataByIDX(base.IDX);
		}
		else
		{
			GUIListPartsPackItem.Data = cmd_ModalItemPackDetail.GetOmakeDataByIDX(base.IDX);
		}
	}

	public override void InitParts()
	{
		this.ShowGUI();
	}

	public override void RefreshParts()
	{
		this.ShowGUI();
	}

	protected override void Awake()
	{
		base.Awake();
	}

	public override void ShowGUI()
	{
		this.ShowData();
		base.ShowGUI();
	}

	private void ShowData()
	{
		if (this.rewards != null)
		{
			this.rewards.SetItem(GUIListPartsPackItem.Data.assetCategoryId, GUIListPartsPackItem.Data.assetValue, "1", false, null);
		}
		GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(GUIListPartsPackItem.Data.assetCategoryId);
		this.lbTX_ItemName.text = this.rewards.GetPresentName(assetCategory, GUIListPartsPackItem.Data.assetValue, true);
		this.lbTX_ItemNum.text = StringMaster.GetString("MissionRewardKakeru") + GUIListPartsPackItem.Data.assetNum;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
