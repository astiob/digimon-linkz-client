using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_WorldDungeonExtraEffectMaster : MasterBaseData<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM>
	{
		public MA_WorldDungeonExtraEffectMaster()
		{
			base.ID = MasterId.WORLD_DUNGEON_EXTRA_EFFECT;
		}

		public override string GetTableName()
		{
			return "world_dungeon_extra_effect_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_WorldDungeonExtraEffectMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM>(base.SetResponse)
			};
		}
	}
}
