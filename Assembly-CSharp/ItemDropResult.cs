using Enemy.DropItem;
using System;

[Serializable]
public class ItemDropResult
{
	private bool _isDropped;

	private DropBoxType _dropBoxType;

	private MasterDataMng.AssetCategory _dropAssetType;

	private int _dropNumber;

	public ItemDropResult(DropBoxType dropBoxType)
	{
		this._isDropped = true;
		this._dropBoxType = dropBoxType;
	}

	public ItemDropResult(bool isDropped)
	{
		this._dropBoxType = DropBoxType.Normal;
		this._isDropped = isDropped;
	}

	public bool isDropped
	{
		get
		{
			return this._isDropped;
		}
	}

	public DropBoxType dropBoxType
	{
		get
		{
			return this._dropBoxType;
		}
	}

	public bool isRare
	{
		get
		{
			return this._dropBoxType != DropBoxType.Normal;
		}
	}
}
