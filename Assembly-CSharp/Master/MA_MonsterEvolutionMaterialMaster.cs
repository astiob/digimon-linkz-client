using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterEvolutionMaterialMaster : MasterBaseData<GameWebAPI.MonsterEvolutionMaterialMaster>
	{
		public MA_MonsterEvolutionMaterialMaster()
		{
			base.ID = MasterId.MONSTER_EVOLUTION_MATERIAL;
		}

		public override string GetTableName()
		{
			return "monster_evolution_material_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMonsterEvolutionMaterialMaster
			{
				OnReceived = new Action<GameWebAPI.MonsterEvolutionMaterialMaster>(base.SetResponse)
			};
		}
	}
}
