using System;
using UnityEngine;

namespace Enemy.DropItem
{
	[Serializable]
	public class EnemyDropItemPattern
	{
		[SerializeField]
		private DropAssetPattern[] _dropAssetPattern = new DropAssetPattern[0];

		public EnemyDropItemPattern(params DropAssetPattern[] dropAssetPattern)
		{
			this._dropAssetPattern = dropAssetPattern;
			this.NormalizeAssetPattern();
		}

		public DropAssetPattern[] dropAssetPattern
		{
			get
			{
				return this._dropAssetPattern;
			}
		}

		public bool TryGetRandomDropAssetPattern(out DropAssetPattern outDropAsset)
		{
			if (this._dropAssetPattern.Length > 0)
			{
				float num = UnityEngine.Random.Range(0f, 1f);
				for (int i = 0; i < this._dropAssetPattern.Length; i++)
				{
					if (this._dropAssetPattern[i].minRange > 0f || this._dropAssetPattern[i].maxRange > 0f)
					{
						if (num >= this._dropAssetPattern[i].minRange && num <= this._dropAssetPattern[i].maxRange)
						{
							outDropAsset = this._dropAssetPattern[i];
							return true;
						}
					}
				}
			}
			outDropAsset = DropAssetPattern.GetUnDropped();
			return false;
		}

		private void NormalizeAssetPattern()
		{
			float value = 0f;
			for (int i = 0; i < this._dropAssetPattern.Length; i++)
			{
				this._dropAssetPattern[i] = new DropAssetPattern(Mathf.Clamp01(value), Mathf.Clamp01(this._dropAssetPattern[i].maxRange), this._dropAssetPattern[i].dropBoxType, this._dropAssetPattern[i].dropAssetType, this._dropAssetPattern[i].dropNumber);
				value = this._dropAssetPattern[i].maxRange;
			}
		}
	}
}
