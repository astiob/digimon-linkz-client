using System;
using WebAPIRequest;

namespace TutorialRequestHeader
{
	public sealed class TutorialStatusSave : RequestTypeBase<TutorialStatusSaveQuery, WebAPI.ResponseData>
	{
		public TutorialStatusSave()
		{
			this.apiId = "910001";
		}
	}
}
