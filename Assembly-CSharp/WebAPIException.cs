using System;

public sealed class WebAPIException : Exception
{
	public WWWResponse.LocalErrorStatus localErrorStatus;

	public WebAPI.ResponseDataErr responseDataError;

	public string apiId;

	public WebAPIException()
	{
	}

	public WebAPIException(string message) : base(message)
	{
	}

	public WebAPIException(string message, Exception inner) : base(message, inner)
	{
	}

	public WebAPIException(WebAPI.ResponseDataErr response)
	{
		this.responseDataError = response;
		this.localErrorStatus = WWWResponse.LocalErrorStatus.NONE;
	}

	public WebAPIException(WebAPI.ResponseDataErr response, string message) : base(message)
	{
		this.responseDataError = response;
		this.localErrorStatus = WWWResponse.LocalErrorStatus.NONE;
	}

	public WebAPIException(WWWResponse.LocalErrorStatus status)
	{
		this.responseDataError = null;
		this.localErrorStatus = status;
	}

	public WebAPIException(WWWResponse.LocalErrorStatus status, string message) : base(message)
	{
		this.responseDataError = null;
		this.localErrorStatus = status;
	}

	public bool IsBackTopScreenError()
	{
		bool result = true;
		if (this.responseDataError == null)
		{
			if (this.localErrorStatus != WWWResponse.LocalErrorStatus.LOCAL_ERROR_JSONPARSE)
			{
				result = false;
			}
		}
		else
		{
			WWWResponse.VenusStatus venus_status = (WWWResponse.VenusStatus)this.responseDataError.venus_status;
			if (venus_status == WWWResponse.VenusStatus.RESPONSE_ERROR || venus_status == WWWResponse.VenusStatus.RESPONSE_SERVER_LOAD_HIGH || venus_status == WWWResponse.VenusStatus.RESPONSE_PENALTY)
			{
				result = false;
			}
		}
		return result;
	}
}
