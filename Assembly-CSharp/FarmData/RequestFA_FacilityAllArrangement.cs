using System;
using WebAPIRequest;

namespace FarmData
{
	public class RequestFA_FacilityAllArrangement : RequestTypeBase<FacilityAllArrangement, WebAPI.ResponseData>
	{
		public RequestFA_FacilityAllArrangement()
		{
			this.apiId = "140008";
		}
	}
}
