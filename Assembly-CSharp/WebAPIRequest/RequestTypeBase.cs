using LitJson;
using System;

namespace WebAPIRequest
{
	public abstract class RequestTypeBase<SendDataT, RecvDataT> : RequestBase where SendDataT : WebAPI.SendBaseData, new() where RecvDataT : WebAPI.RecvBaseData, new()
	{
		private bool isParamSetted;

		public Action<SendDataT> SetSendData { get; set; }

		public Action<RecvDataT> OnReceived { get; set; }

		public override void SetRequestHeader(int requestId)
		{
			if (this.param == null)
			{
				this.param = Activator.CreateInstance<SendDataT>();
			}
			this.param.uniqueRequestId = requestId;
		}

		public override void SetParam()
		{
			if (!this.isParamSetted)
			{
				this.isParamSetted = true;
				if (this.SetSendData != null)
				{
					this.SetSendData(this.param as SendDataT);
				}
			}
		}

		public override void ToObject(string json, bool isResData)
		{
			RecvDataT obj;
			try
			{
				if (isResData)
				{
					WebAPIJsonParse.GetResponseData<RecvDataT>(WebAPIJsonParse.GetBody(json), out obj);
				}
				else
				{
					WebAPIJsonParse.GetResponseData<RecvDataT>(json, out obj);
				}
			}
			catch (JsonException ex)
			{
				throw new WebAPIException(WWWResponse.LocalErrorStatus.LOCAL_ERROR_JSONPARSE, ex.Message);
			}
			if (this.OnReceived != null)
			{
				this.OnReceived(obj);
			}
			this.SetHeader(json);
		}

		protected virtual void SetHeader(string json)
		{
		}
	}
}
