using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_FacilityExtraEffectLevelMaster : MasterBaseData<GameWebAPI.RespDataMA_FacilityExtraEffectLevelM>
	{
		public MA_FacilityExtraEffectLevelMaster()
		{
			base.ID = MasterId.FACILITY_EXTRA_EFFECT_LEVEL;
		}

		public override string GetTableName()
		{
			return "facility_extra_effect_level_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_FacilityExtraEffectLevelM
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_FacilityExtraEffectLevelM>(base.SetResponse)
			};
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_FacilityExtraEffectLevelM src)
		{
			FarmDataManager.SetResponseData(src);
			this.data = src;
		}
	}
}
