using System;
using UI.Common;
using UnityEngine;

namespace UI.Gasha
{
	public sealed class GashaUserAssetsInventory : MonoBehaviour
	{
		[SerializeField]
		private GameObject leftAssets;

		[SerializeField]
		private GameObject rightAssets;

		private UIAssetsIcon leftAssetsIcon;

		private UIAssetsNumber leftAssetsNumber;

		private UIAssetsIcon rightAssetsIcon;

		private UIAssetsNumber rightAssetsNumber;

		private void Awake()
		{
			this.leftAssetsIcon = this.leftAssets.GetComponent<UIAssetsIcon>();
			this.leftAssetsNumber = this.leftAssets.GetComponent<UIAssetsNumber>();
			global::Debug.Assert(null != this.leftAssetsIcon, "左側のアセットアイコンがMissing");
			global::Debug.Assert(null != this.leftAssetsNumber, "左側のアセットラベルがMissing");
			this.rightAssetsIcon = this.rightAssets.GetComponent<UIAssetsIcon>();
			this.rightAssetsNumber = this.rightAssets.GetComponent<UIAssetsNumber>();
			global::Debug.Assert(null != this.rightAssetsIcon, "右側のアセットアイコンがMissing");
			global::Debug.Assert(null != this.rightAssetsNumber, "右側のアセットラベルがMissing");
		}

		private void SetAssets(UIAssetsIcon icon, UIAssetsNumber number, MasterDataMng.AssetCategory category, string assetsValue)
		{
			icon.SetAssetsCategory(category, assetsValue);
			icon.SetIcon();
			number.SetUserInventoryNumber(category, assetsValue);
		}

		public void SetGashaPriceType(GameWebAPI.RespDataGA_GetGachaInfo.PriceType priceType)
		{
			MasterDataMng.AssetCategory costAssetsCategory = priceType.GetCostAssetsCategory();
			string costAssetsValue = priceType.GetCostAssetsValue();
			this.SetAssets(this.leftAssetsIcon, this.leftAssetsNumber, costAssetsCategory, costAssetsValue);
			this.rightAssets.SetActive(false);
		}
	}
}
