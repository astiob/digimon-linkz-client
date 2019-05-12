using System;
using WebAPIRequest;

namespace TutorialRequestHeader
{
	public sealed class TutorialNaviMasterList : RequestTypeBase<TutorialNaviMasterListQuery, TutorialNaviMasterResponse>
	{
		public TutorialNaviMasterList()
		{
			this.apiId = "999101";
		}
	}
}
