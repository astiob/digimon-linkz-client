using System;
using WebAPIRequest;

namespace FarmData
{
	public sealed class RequestFA_FacilityHarvest : RequestTypeBase<FacilityHarvest, FacilityHarvestResult>
	{
		public RequestFA_FacilityHarvest()
		{
			this.apiId = "140009";
		}
	}
}
