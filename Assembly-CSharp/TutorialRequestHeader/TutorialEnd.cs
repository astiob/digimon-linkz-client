using System;
using WebAPIRequest;

namespace TutorialRequestHeader
{
	public sealed class TutorialEnd : RequestTypeBase<WebAPI.SendBaseData, WebAPI.ResponseData>
	{
		public TutorialEnd()
		{
			this.apiId = "910002";
		}
	}
}
