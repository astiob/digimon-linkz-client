using System;
using UnityEngine;

namespace Enemy.DropItem
{
	[Serializable]
	public struct DropAssetPattern
	{
		[SerializeField]
		private float _minRange;

		[SerializeField]
		private float _maxRange;

		[SerializeField]
		private DropBoxType _dropBoxType;

		[SerializeField]
		private DropAssetType _dropAssetType;

		[SerializeField]
		private int _dropNumber;

		public DropAssetPattern(float MinRange, float MaxRange, DropBoxType DropBoxType, DropAssetType DropAssetType, int DropNumber)
		{
			this._minRange = MinRange;
			this._maxRange = MaxRange;
			this._dropBoxType = DropBoxType;
			this._dropAssetType = DropAssetType;
			this._dropNumber = DropNumber;
		}

		public static DropAssetPattern GetUnDropped()
		{
			return new DropAssetPattern(0f, 0f, DropBoxType.Normal, DropAssetType.Monster, 0);
		}

		public float minRange
		{
			get
			{
				return this._minRange;
			}
		}

		public float maxRange
		{
			get
			{
				return this._maxRange;
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
	}
}
