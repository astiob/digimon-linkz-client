using System;

namespace TutorialRequestHeader
{
	[Serializable]
	public sealed class TutorialNaviMasterResponse : WebAPI.ResponseData
	{
		public TutorialNaviMaster[] tutorialNaviM;
	}
}
