using System;

namespace UI.SaleCheck
{
	public sealed class UI_SaleBonusCheckList : CMDRecycleViewUDWrapper
	{
		private SaleBonus saleBonus;

		public void Initialize(SaleBonus bonus)
		{
			this.saleBonus = bonus;
			UI_SaleBonusCheckListItem ui_SaleBonusCheckListItem = this.listParts as UI_SaleBonusCheckListItem;
			if (null != ui_SaleBonusCheckListItem)
			{
				ui_SaleBonusCheckListItem.SetFuncSaleBonusAsset(new Func<int, GameWebAPI.ResponseAssetSalesBonusMaster.SalesBonus>(this.GetSalesBonus));
				ui_SaleBonusCheckListItem.SetFuncBonusAssetNum(new Func<int, int>(this.GetBonusAssetNum));
			}
			base.InitializeView(2);
			if (this.saleBonus != null)
			{
				base.CreateList(this.saleBonus.Count());
			}
		}

		public GameWebAPI.ResponseAssetSalesBonusMaster.SalesBonus GetSalesBonus(int index)
		{
			GameWebAPI.ResponseAssetSalesBonusMaster.SalesBonus result = null;
			if (this.saleBonus != null && index < this.saleBonus.Count())
			{
				result = this.saleBonus.GetMaster(index);
			}
			return result;
		}

		public int GetBonusAssetNum(int index)
		{
			int result = 0;
			if (this.saleBonus != null && index < this.saleBonus.Count())
			{
				result = this.saleBonus.GetNum(index);
			}
			return result;
		}
	}
}
