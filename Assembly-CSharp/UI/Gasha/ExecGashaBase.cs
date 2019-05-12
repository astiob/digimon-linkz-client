using System;
using System.Collections;
using User;

namespace UI.Gasha
{
	public abstract class ExecGashaBase
	{
		protected GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo;

		protected void UpdateGashaInfo(int playCount)
		{
			this.gashaInfo.UpdatePlayCount(playCount);
		}

		protected void UpdateUserAssetsInventory(int playCount)
		{
			GameWebAPI.RespDataGA_GetGachaInfo.Detail detail = this.gashaInfo.details.GetDetail(playCount);
			int value = 0;
			if (int.TryParse(detail.GetPrice(), out value))
			{
				UserInventory.CalculateNumber(this.gashaInfo.priceType.GetCostAssetsCategory(), this.gashaInfo.priceType.GetCostAssetsValue(), value);
			}
		}

		protected virtual void OnShowedGashaResultDialog(int noop)
		{
			RestrictionInput.EndLoad();
		}

		public void SetGashaInfo(GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo)
		{
			this.gashaInfo = gashaInfo;
		}

		public abstract IEnumerator Exec(GameWebAPI.GA_Req_ExecGacha playGashaRequestParam, bool isTutorial);
	}
}
