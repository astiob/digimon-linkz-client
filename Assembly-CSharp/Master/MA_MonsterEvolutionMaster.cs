using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterEvolutionMaster : MasterBaseData<GameWebAPI.RespDataMA_GetMonsterEvolutionM>
	{
		public MA_MonsterEvolutionMaster()
		{
			base.ID = MasterId.MONSTER_EVOLUTION;
		}

		public override string GetTableName()
		{
			return "monster_evolution_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_MonsterEvolutionMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetMonsterEvolutionM>(base.SetResponse)
			};
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_GetMonsterEvolutionM src)
		{
			src.Initialize();
			this.data = src;
		}
	}
}
