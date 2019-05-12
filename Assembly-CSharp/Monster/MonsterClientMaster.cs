using System;

namespace Monster
{
	public sealed class MonsterClientMaster
	{
		private GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterSimpleMaster;

		private GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMaster;

		public MonsterClientMaster(GameWebAPI.RespDataMA_GetMonsterMS.MonsterM simple, GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group)
		{
			this.monsterSimpleMaster = simple;
			this.monsterGroupMaster = group;
		}

		public GameWebAPI.RespDataMA_GetMonsterMS.MonsterM Simple
		{
			get
			{
				return this.monsterSimpleMaster;
			}
		}

		public GameWebAPI.RespDataMA_GetMonsterMG.MonsterM Group
		{
			get
			{
				return this.monsterGroupMaster;
			}
		}
	}
}
