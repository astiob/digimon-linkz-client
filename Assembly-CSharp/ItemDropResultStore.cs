using Enemy.DropItem;
using System;

public class ItemDropResultStore
{
	public bool isDropped { get; set; }

	public DropBoxType dropBoxType { get; set; }

	public MasterDataMng.AssetCategory dropAssetType { get; set; }

	public int dropNumber { get; set; }

	public bool isRare { get; set; }
}
