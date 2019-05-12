using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WebAPIRequest
{
	public sealed class RequestList
	{
		private List<RequestBase> requestList = new List<RequestBase>();

		public bool IsEmpty()
		{
			return this.requestList.Count == 0;
		}

		public void AddRequest(RequestBase addRequest)
		{
			addRequest.SetRequestHeader(APIUtil.Instance().GetRequestID());
			APIUtil.Instance().UpdateRequestID();
			addRequest.param.requestStatus = 0;
			this.requestList.Add(addRequest);
		}

		public void SetParam()
		{
			for (int i = 0; i < this.requestList.Count; i++)
			{
				this.requestList[i].SetParam();
			}
		}

		public IEnumerator Exceution(WebAPI webApi)
		{
			string sendJson = WebAPIJsonParse.CreateJsonData(this.requestList);
			WWWResponse response = new WWWResponse();
			yield return webApi.StartCoroutine(GameWebAPI.Instance().StartAPIRequest("API List", sendJson, response));
			try
			{
				string responseJson = GameWebAPI.Instance().GetResponseJson(response);
				try
				{
					Dictionary<string, string> responseList = WebAPIJsonParse.GetResponseList(this.requestList, responseJson);
					for (int i = 0; i < this.requestList.Count; i++)
					{
						if (responseList.ContainsKey(this.requestList[i].apiId))
						{
							this.requestList[i].ToObject(responseList[this.requestList[i].apiId], false);
						}
					}
				}
				catch (JsonException ex)
				{
					throw new WebAPIException(WWWResponse.LocalErrorStatus.LOCAL_ERROR_JSONPARSE, ex.Message);
				}
			}
			catch (WebAPIException ex2)
			{
				throw ex2;
			}
			yield break;
		}
	}
}
