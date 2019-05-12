using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_AbilityUpgradeM : MasterBaseData<GameWebAPI.RespDataMA_AbilityUpgradeM>
	{
		public MA_AbilityUpgradeM()
		{
			base.ID = MasterId.ABILITY_MEDAL_UPGRADE;
		}

		public override string GetTableName()
		{
			return "ability_upgrade_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_AbilityUpgradeMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_AbilityUpgradeM>(base.SetResponse)
			};
		}
	}
}
