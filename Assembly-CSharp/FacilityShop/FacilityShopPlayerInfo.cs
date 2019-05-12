using Master;
using System;
using UnityEngine;

namespace FacilityShop
{
	public sealed class FacilityShopPlayerInfo : MonoBehaviour
	{
		[SerializeField]
		private UILabel stoneNum;

		[SerializeField]
		private UILabel clusterNum;

		[SerializeField]
		private UILabel buildCostNum;

		public void SetPlayerInfo()
		{
			GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
			this.stoneNum.text = playerInfo.point.ToString();
			this.clusterNum.text = StringFormat.Cluster(playerInfo.gamemoney);
			this.buildCostNum.text = string.Format(StringMaster.GetString("SystemFraction"), FarmUtility.GetBuildFacilityCount(), 2);
		}
	}
}
