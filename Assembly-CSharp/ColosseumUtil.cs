using System;
using System.Linq;

public sealed class ColosseumUtil
{
	public GameWebAPI.RespData_ColosseumInfoLogic colosseumInfo;

	public GameWebAPI.RespDataMA_ColosseumM.Colosseum colosseumM;

	public GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule[] colosseumTimeScheduleM;

	public ColosseumUtil()
	{
		this.colosseumInfo = DataMng.Instance().RespData_ColosseumInfo;
		if (this.colosseumInfo != null)
		{
			this.colosseumM = new GameWebAPI.RespDataMA_ColosseumM.Colosseum();
			GameWebAPI.RespDataMA_ColosseumM respDataMA_ColosseumMaster = MasterDataMng.Instance().RespDataMA_ColosseumMaster;
			if (respDataMA_ColosseumMaster != null)
			{
				this.colosseumM = respDataMA_ColosseumMaster.colosseumM.SingleOrDefault((GameWebAPI.RespDataMA_ColosseumM.Colosseum item) => item.colosseumId == this.colosseumInfo.colosseumId.ToString());
			}
			this.colosseumTimeScheduleM = new GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule[0];
			GameWebAPI.RespDataMA_ColosseumTimeScheduleM respDataMA_ColosseumTimeScheduleMaster = MasterDataMng.Instance().RespDataMA_ColosseumTimeScheduleMaster;
			if (respDataMA_ColosseumTimeScheduleMaster != null)
			{
				this.colosseumTimeScheduleM = respDataMA_ColosseumTimeScheduleMaster.colosseumTimeScheduleM.Where((GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule item) => item.colosseumId == this.colosseumInfo.colosseumId.ToString()).ToArray<GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule>();
			}
		}
	}

	public bool isOpen
	{
		get
		{
			if (this.colosseumInfo == null || this.colosseumM == null || this.colosseumTimeScheduleM == null)
			{
				return false;
			}
			if (this.isOpenAllDay)
			{
				return true;
			}
			if (DateTime.Parse(this.colosseumM.openTime) < ServerDateTime.Now && ServerDateTime.Now < DateTime.Parse(this.colosseumM.closeTime))
			{
				foreach (GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule colosseumTimeSchedule in this.colosseumTimeScheduleM)
				{
					if (colosseumTimeSchedule.colosseumId == this.colosseumInfo.colosseumId.ToString())
					{
						DateTime t = DateTime.Now;
						DateTime t2 = DateTime.Now;
						t = DateTime.Parse(colosseumTimeSchedule.startHour);
						t2 = DateTime.Parse(colosseumTimeSchedule.endHour);
						if (t < ServerDateTime.Now && ServerDateTime.Now < t2)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}

	public bool isOpenAllDay
	{
		get
		{
			return this.colosseumInfo != null && this.colosseumInfo.openAllDay > 0;
		}
	}
}
