using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterEvolutionRouteMaster : MasterBaseData<GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM>
	{
		public MA_MonsterEvolutionRouteMaster()
		{
			base.ID = MasterId.MONSTER_EVOLUTION_ROUTE;
		}

		public override string GetTableName()
		{
			return "monster_evolution_route_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_MonsterEvolutionRouteMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM>(base.SetResponse)
			};
		}
	}
}
