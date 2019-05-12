using System;
using WebAPIRequest;

namespace FarmData
{
	public sealed class RequestFA_FacilityBuildShortening : RequestTypeBase<FacilityBuildShortening, FacilityBuildShorteningResult>
	{
		public RequestFA_FacilityBuildShortening()
		{
			this.apiId = "140002";
		}
	}
}
