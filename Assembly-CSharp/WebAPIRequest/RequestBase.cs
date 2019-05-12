using System;
using System.Collections;

namespace WebAPIRequest
{
	public abstract class RequestBase
	{
		public string apiId;

		public WebAPI.SendBaseData param;

		public abstract void SetRequestHeader(int requestId);

		public abstract void SetParam();

		public IEnumerator Exceution(WebAPI webApi, WebAPI.RequestStatus requestStatus)
		{
			this.param.requestStatus = (int)requestStatus;
			string sendJson = WebAPIJsonParse.CreateJsonData(this.apiId, this.param);
			WWWResponse response = new WWWResponse();
			yield return webApi.StartCoroutine(webApi.StartAPIRequest(this.apiId, sendJson, response));
			try
			{
				string responseJson = webApi.GetResponseJson(response);
				this.ToObject(responseJson, true);
			}
			catch (WebAPIException ex)
			{
				if (ex != null)
				{
					ex.apiId = this.apiId;
				}
				throw ex;
			}
			yield break;
		}

		public abstract void ToObject(string json, bool isResData);
	}
}
