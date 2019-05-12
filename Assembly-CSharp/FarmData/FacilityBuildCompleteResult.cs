using System;

namespace FarmData
{
	public sealed class FacilityBuildCompleteResult : WebAPI.ResponseData
	{
		public int result;

		public enum resultCode
		{
			FAILED,
			SUCCESS
		}
	}
}
