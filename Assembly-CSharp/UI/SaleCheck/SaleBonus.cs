using System;
using System.Collections.Generic;

namespace UI.SaleCheck
{
	public sealed class SaleBonus
	{
		private List<SaleBonus.Info> saleBonuses = new List<SaleBonus.Info>();

		public void AddBonus(GameWebAPI.ResponseAssetSalesBonusMaster.SalesBonus bonusMaster, int saleNum)
		{
			int num;
			if (int.TryParse(bonusMaster.bonusAssetNum, out num))
			{
				SaleBonus.Info item = new SaleBonus.Info
				{
					master = bonusMaster,
					num = num * saleNum
				};
				this.saleBonuses.Add(item);
			}
		}

		public int Count()
		{
			int result = 0;
			if (this.saleBonuses != null)
			{
				result = this.saleBonuses.Count;
			}
			return result;
		}

		public GameWebAPI.ResponseAssetSalesBonusMaster.SalesBonus GetMaster(int index)
		{
			GameWebAPI.ResponseAssetSalesBonusMaster.SalesBonus result = null;
			if (this.saleBonuses != null && index < this.saleBonuses.Count)
			{
				result = this.saleBonuses[index].master;
			}
			return result;
		}

		public int GetNum(int index)
		{
			int result = 0;
			if (this.saleBonuses != null && index < this.saleBonuses.Count)
			{
				result = this.saleBonuses[index].num;
			}
			return result;
		}

		private sealed class Info
		{
			public GameWebAPI.ResponseAssetSalesBonusMaster.SalesBonus master;

			public int num;
		}
	}
}
