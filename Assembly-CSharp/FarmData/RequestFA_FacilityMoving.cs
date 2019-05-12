using System;
using WebAPIRequest;

namespace FarmData
{
	public sealed class RequestFA_FacilityMoving : RequestTypeBase<FacilityMoving, WebAPI.ResponseData>
	{
		public RequestFA_FacilityMoving()
		{
			this.apiId = "140005";
		}
	}
}
