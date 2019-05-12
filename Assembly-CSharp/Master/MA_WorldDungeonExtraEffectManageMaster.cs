using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_WorldDungeonExtraEffectManageMaster : MasterBaseData<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectManageM>
	{
		public MA_WorldDungeonExtraEffectManageMaster()
		{
			base.ID = MasterId.WORLD_DUNGEON_EXTRA_EFFECT_MANAGE;
		}

		public override string GetTableName()
		{
			return "world_dungeon_extra_effect_manage_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_WorldDungeonExtraEffectManageMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectManageM>(base.SetResponse)
			};
		}
	}
}
