using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_FacilityExtraEffectMaster : MasterBaseData<GameWebAPI.RespDataMA_FacilityExtraEffectM>
	{
		public MA_FacilityExtraEffectMaster()
		{
			base.ID = MasterId.FACILITY_EXTRA_EFFECT;
		}

		public override string GetTableName()
		{
			return "facility_extra_effect_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_FacilityExtraEffectMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_FacilityExtraEffectM>(base.SetResponse)
			};
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_FacilityExtraEffectM src)
		{
			FarmDataManager.SetResponseData(src);
			this.data = src;
		}
	}
}
