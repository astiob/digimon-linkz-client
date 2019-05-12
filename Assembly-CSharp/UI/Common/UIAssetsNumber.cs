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
			global::Debug.Assert(assetsCategory != MasterDataMng.AssetCategory.NONE, "アセットカテゴリーIDが設定されていません");
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

		public int GetUserInventoryNumber(int assetsCategory)
		{
			return this.GetUserInventoryNumber(assetsCategory, string.Empty);
		}

		public int GetUserInventoryNumber(int assetsCategory, string assetsValue)
		{
			global::Debug.Assert(assetsCategory != 0, "アセットカテゴリーIDが設定されていません");
			int result;
			if (string.IsNullOrEmpty(assetsValue))
			{
				result = UserInventory.GetNumber((MasterDataMng.AssetCategory)assetsCategory);
			}
			else if (this.countProtectedAssets)
			{
				result = UserInventory.GetNumber((MasterDataMng.AssetCategory)assetsCategory, assetsValue);
			}
			else
			{
				result = UserInventory.GetNumberExceptProtectedAssets((MasterDataMng.AssetCategory)assetsCategory, assetsValue);
			}
			return result;
		}

		public void SetNumber(int category, int number)
		{
			string text = string.Empty;
			if (category != 4)
			{
				text = number.ToString();
			}
			else
			{
				text = number.ToString("#,0");
			}
			if (string.IsNullOrEmpty(text))
			{
				this.numberLabel.text = "0";
			}
			else
			{
				this.numberLabel.text = text;
			}
		}
	}
}
