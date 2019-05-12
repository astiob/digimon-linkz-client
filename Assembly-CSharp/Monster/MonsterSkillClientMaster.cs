using System;
using System.Collections.Generic;

namespace Monster
{
	public sealed class MonsterSkillClientMaster
	{
		private GameWebAPI.RespDataMA_GetSkillM.SkillM skillSimpleMaster;

		private List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM> skillDetailMasterList;

		private GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM viewSkillDetailMaster;

		public MonsterSkillClientMaster(GameWebAPI.RespDataMA_GetSkillM.SkillM simple, List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM> detailList)
		{
			this.skillSimpleMaster = simple;
			this.skillDetailMasterList = detailList;
			this.GetSkillDetailViewData(out this.viewSkillDetailMaster, detailList);
		}

		private void GetSkillDetailViewData(out GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM dest, List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM> source)
		{
			dest = null;
			for (int i = 0; i < source.Count; i++)
			{
				if (dest == null || source[i].effectType == 1)
				{
					dest = source[i];
				}
			}
		}

		public GameWebAPI.RespDataMA_GetSkillM.SkillM Simple
		{
			get
			{
				return this.skillSimpleMaster;
			}
		}

		public List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM> DetailList
		{
			get
			{
				return this.skillDetailMasterList;
			}
		}

		public GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ViewSkillDetail
		{
			get
			{
				return this.viewSkillDetailMaster;
			}
		}
	}
}
