using System;
using UI.Common;

namespace UI.SaleCheck
{
	public sealed class UI_SaleBonusCheckAssetIcon : GUICollider
	{
		private string assetCategoryId;

		private string assetValue;

		private void OnPushIcon()
		{
			FactoryAssetCategoryDetailPopup.Create(this.assetCategoryId, this.assetValue);
		}

		public void SetAssetCategory(string assetCategoryId, string assetValue)
		{
			this.assetCategoryId = assetCategoryId;
			this.assetValue = assetValue;
		}
	}
}
