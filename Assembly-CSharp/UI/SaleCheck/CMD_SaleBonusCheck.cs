using System;
using UI.Common;
using UnityEngine;

namespace UI.SaleCheck
{
	public sealed class CMD_SaleBonusCheck : CMD
	{
		private SaleAsset saleAsset = new SaleAsset();

		[SerializeField]
		private UILabel titleLabel;

		[SerializeField]
		private UILabel infoLabel;

		[SerializeField]
		private UIAssetsNumber clusterNumber;

		[SerializeField]
		private UI_SaleBonusCheckList bonusList;

		private Action<CommonDialog> actionPushYesButton;

		private void OnPushYesButton()
		{
			if (this.actionPushYesButton != null)
			{
				this.actionPushYesButton(this);
			}
		}

		public void SetText(string title, string info, int saleValue)
		{
			this.titleLabel.text = title;
			this.infoLabel.text = info;
			this.clusterNumber.SetNumber(4, saleValue);
		}

		public void SetActionYesButton(Action<CommonDialog> action)
		{
			this.actionPushYesButton = action;
		}

		public SaleAsset GetSaleAssetStorage()
		{
			return this.saleAsset;
		}

		public void CreateBonusList()
		{
			if (this.saleAsset.ExistSaleAsset())
			{
				SaleBonus salesBonus = this.saleAsset.GetSalesBonus();
				this.bonusList.Initialize(salesBonus);
			}
		}
	}
}
