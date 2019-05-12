using System;
using WebAPIRequest;

namespace FarmData
{
	public sealed class RequestFA_UserNewFacility : RequestTypeBase<UserNewFacilityRequest, UserNewFacilityResponse>
	{
		public RequestFA_UserNewFacility()
		{
			this.apiId = "140013";
		}
	}
}
