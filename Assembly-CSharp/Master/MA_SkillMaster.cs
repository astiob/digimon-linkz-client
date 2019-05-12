using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_SkillMaster : MasterBaseData<GameWebAPI.RespDataMA_GetSkillM>
	{
		public MA_SkillMaster()
		{
			base.ID = MasterId.SKILL;
		}

		public override string GetTableName()
		{
			return "skill_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_SkillMaster requestMA_SkillMaster = new GameWebAPI.RequestMA_SkillMaster();
			requestMA_SkillMaster.SetSendData = delegate(GameWebAPI.RequestMA_SkillM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_SkillMaster.OnReceived = new Action<GameWebAPI.RespDataMA_GetSkillM>(base.SetResponse);
			return requestMA_SkillMaster;
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_GetSkillM src)
		{
			this.data = src;
			GameWebAPI.RespDataMA_GetSkillM.SkillM[] skillM = this.data.skillM;
			for (int i = 0; i < skillM.Length; i++)
			{
				string getActionSkill = skillM[i].GetActionSkill;
				if (getActionSkill != null)
				{
					GameWebAPI.RespDataMA_GetSkillM.SkillM.ActionSkill = getActionSkill;
					break;
				}
			}
		}
	}
}
