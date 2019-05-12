using System;
using WebAPIRequest;

namespace TutorialRequestHeader
{
	public sealed class TutorialMonsterEvolution : RequestTypeBase<WebAPI.SendBaseData, WebAPI.ResponseData>
	{
		public TutorialMonsterEvolution()
		{
			this.apiId = "910007";
		}
	}
}
