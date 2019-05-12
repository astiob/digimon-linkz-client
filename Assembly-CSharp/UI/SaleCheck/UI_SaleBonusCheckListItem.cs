using Master;
using System;
using UI.Common;
using UnityEngine;

namespace UI.SaleCheck
{
	public sealed class UI_SaleBonusCheckListItem : GUIListPartsWrapper
	{
		private Func<int, GameWebAPI.ResponseAssetSalesBonusMaster.SalesBonus> getSaleBonus;

		private Func<int, int> getBonusAssetNum;

		[SerializeField]
		private UIAssetsIcon saleAssetsIcon;

		[SerializeField]
		private UIAssetsIcon bonusAssetsIcon;

		[SerializeField]
		private UI_SaleBonusCheckAssetIcon uiIcon;

		[SerializeField]
		private UILabel bonusNumLabel;

		private void SetAssetIcon(string assetCategoryId, string assetValue, UIAssetsIcon icon)
		{
			int num;
			if (int.TryParse(assetCategoryId, out num))
			{
				MasterDataMng.AssetCategory category = (MasterDataMng.AssetCategory)num;
				icon.SetAssetsCategory(category, assetValue);
				icon.SetIcon();
			}
		}

		protected override void OnUpdatedParts(int listPartsIndex)
		{
			if (this.getSaleBonus != null)
			{
				GameWebAPI.ResponseAssetSalesBonusMaster.SalesBonus salesBonus = this.getSaleBonus(listPartsIndex);
				if (salesBonus != null)
				{
					this.SetAssetIcon(salesBonus.baseAssetCategoryId, salesBonus.baseAssetValue, this.saleAssetsIcon);
					this.SetAssetIcon(salesBonus.bonusAssetCategoryId, salesBonus.bonusAssetValue, this.bonusAssetsIcon);
					this.uiIcon.SetAssetCategory(salesBonus.bonusAssetCategoryId, salesBonus.bonusAssetValue);
				}
			}
			if (this.getBonusAssetNum != null)
			{
				int num = this.getBonusAssetNum(listPartsIndex);
				this.bonusNumLabel.text = string.Format(StringMaster.GetString("AssetSalesBonusCount"), num);
			}
		}

		public override void ReceiveOriginalParts(GUIListPartBS originalParts)
		{
			UI_SaleBonusCheckListItem ui_SaleBonusCheckListItem = originalParts as UI_SaleBonusCheckListItem;
			if (null != ui_SaleBonusCheckListItem)
			{
				this.getSaleBonus = ui_SaleBonusCheckListItem.getSaleBonus;
				this.getBonusAssetNum = ui_SaleBonusCheckListItem.getBonusAssetNum;
			}
		}

		public override void InitParts()
		{
			this.playSelectSE = true;
			this.touchBehavior = GUICollider.TouchBehavior.None;
		}

		public void SetFuncSaleBonusAsset(Func<int, GameWebAPI.ResponseAssetSalesBonusMaster.SalesBonus> func)
		{
			this.getSaleBonus = func;
		}

		public void SetFuncBonusAssetNum(Func<int, int> func)
		{
			this.getBonusAssetNum = func;
		}
	}
}
