using System;
using WebAPIRequest;

namespace FarmData
{
	public sealed class RequestFA_FacilityBuild : RequestTypeBase<FacilityBuild, FacilityBuildResult>
	{
		public RequestFA_FacilityBuild()
		{
			this.apiId = "140001";
		}
	}
}
