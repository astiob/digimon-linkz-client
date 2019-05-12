using System;
using UnityEngine;
using User;

namespace UI.Common
{
	public class UIAssetsNumber : MonoBehaviour
	{
		[SerializeField]
		protected UILabel numberLabel;

		[SerializeField]
		protected bool countProtectedAssets;

		protected virtual string ConvertFormat(string number)
		{
			return number;
		}

		public void SetNumber(string number)
		{
			if (string.IsNullOrEmpty(number))
			{
				this.numberLabel.text = "0";
			}
			else
			{
				this.numberLabel.text = this.ConvertFormat(number);
			}
		}

		public void SetUserInventoryNumber(MasterDataMng.AssetCategory assetsCategory)
		{
			this.SetUserInventoryNumber(assetsCategory, string.Empty);
		}

		public void SetUserInventoryNumber(MasterDataMng.AssetCategory assetsCategory, string assetsValue)
		{
			global::Debug.Assert(assetsCategory != (MasterDataMng.AssetCategory)0, "アセットカテゴリーIDが設定されていません");
			int num;
			if (string.IsNullOrEmpty(assetsValue))
			{
				num = UserInventory.GetNumber(assetsCategory);
			}
			else if (this.countProtectedAssets)
			{
				num = UserInventory.GetNumber(assetsCategory, assetsValue);
			}
			else
			{
				num = UserInventory.GetNumberExceptProtectedAssets(assetsCategory, assetsValue);
			}
			this.SetNumber(num.ToString());
		}
	}
}
