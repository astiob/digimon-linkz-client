using System;
using WebAPIRequest;

namespace FarmData
{
	public sealed class RequestFA_FacilityUpgradeShortening : RequestTypeBase<FacilityUpgradeShortening, FacilityUpgradeShorteningResult>
	{
		public RequestFA_FacilityUpgradeShortening()
		{
			this.apiId = "140004";
		}
	}
}
