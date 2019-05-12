using System;
using WebAPIRequest;

namespace FarmData
{
	public sealed class RequestFA_FacilityBuildComplete : RequestTypeBase<FacilityBuildComplete, FacilityBuildCompleteResult>
	{
		public RequestFA_FacilityBuildComplete()
		{
			this.apiId = "140010";
		}
	}
}
