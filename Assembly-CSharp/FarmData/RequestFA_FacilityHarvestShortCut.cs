using System;
using WebAPIRequest;

namespace FarmData
{
	public sealed class RequestFA_FacilityHarvestShortCut : RequestTypeBase<FacilityHarvestShortCut, FacilityHarvestShortCutResult>
	{
		public RequestFA_FacilityHarvestShortCut()
		{
			this.apiId = "140016";
		}
	}
}
