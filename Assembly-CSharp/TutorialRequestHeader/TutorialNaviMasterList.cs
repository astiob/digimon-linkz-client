using System;
using WebAPIRequest;

namespace TutorialRequestHeader
{
	public sealed class TutorialNaviMasterList : RequestTypeBase<WebAPI.SendBaseData, TutorialNaviMasterResponse>
	{
		public TutorialNaviMasterList()
		{
			this.apiId = "999101";
		}
	}
}
