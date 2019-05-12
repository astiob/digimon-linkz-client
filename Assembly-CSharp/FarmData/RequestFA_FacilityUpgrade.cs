using System;
using WebAPIRequest;

namespace FarmData
{
	public sealed class RequestFA_FacilityUpgrade : RequestTypeBase<FacilityUpgrade, WebAPI.ResponseData>
	{
		public RequestFA_FacilityUpgrade()
		{
			this.apiId = "140003";
		}
	}
}
