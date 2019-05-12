using Enemy.DropItem;
using System;

[Serializable]
public class ItemDropResult
{
	private bool _isDropped;

	private DropBoxType _dropBoxType;

	private DropAssetType _dropAssetType;

	private int _dropNumber;

	public ItemDropResult(DropBoxType dropBoxType, DropAssetType dropAssetType, int dropNumber)
	{
		this._isDropped = true;
		this._dropBoxType = dropBoxType;
		this._dropAssetType = dropAssetType;
		this._dropNumber = dropNumber;
	}

	public ItemDropResult(DropAssetPattern dropAssetPattern)
	{
		this._dropBoxType = dropAssetPattern.dropBoxType;
		this._dropAssetType = dropAssetPattern.dropAssetType;
		this._dropNumber = dropAssetPattern.dropNumber;
		this._isDropped = true;
	}

	public ItemDropResult(bool isDropped)
	{
		this._dropBoxType = DropBoxType.Normal;
		this._dropAssetType = DropAssetType.Monster;
		this._dropNumber = 0;
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

	public DropAssetType dropAssetType
	{
		get
		{
			return this._dropAssetType;
		}
	}

	public int dropNumber
	{
		get
		{
			return this._dropNumber;
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
