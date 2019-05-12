using System;
using WebAPIRequest;

namespace FarmData
{
	public sealed class RequestFA_FacilityStock : RequestTypeBase<FacilityStock, FacilityStockResult>
	{
		public RequestFA_FacilityStock()
		{
			this.apiId = "140015";
		}
	}
}
