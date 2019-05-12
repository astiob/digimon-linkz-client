using System;
using System.Collections;
using WebAPIRequest;

public sealed class APIRequestTask : TaskBase
{
	private RequestBase request;

	private RequestList requestList;

	private WebAPI.RequestStatus requestStatus;

	private bool isRequestRetry;

	private bool isSelectedRetryButton;

	public APIRequestTask()
	{
	}

	public APIRequestTask(RequestBase request, bool requestRetry = true)
	{
		request.SetRequestHeader(APIUtil.Instance().GetRequestID());
		APIUtil.Instance().UpdateRequestID();
		this.request = request;
		this.isRequestRetry = requestRetry;
	}

	public APIRequestTask(RequestList requestList, bool requestRetry = true)
	{
		this.requestList = requestList;
		this.isRequestRetry = requestRetry;
	}

	public override IEnumerator Execution()
	{
		if (this.request != null)
		{
			this.request.SetParam();
			return this.request.Exceution(GameWebAPI.Instance(), this.requestStatus);
		}
		if (this.requestList != null && !this.requestList.IsEmpty())
		{
			this.requestList.SetParam();
			return this.requestList.Exceution(GameWebAPI.Instance());
		}
		return null;
	}

	public override bool IsBackTopScreen(Exception ex)
	{
		WebAPIException ex2 = ex as WebAPIException;
		return ex2.IsBackTopScreenError();
	}

	public override IEnumerator OnAlert(Exception ex)
	{
		WebAPIException exception = ex as WebAPIException;
		APIAlert apiAlert = new APIAlert();
		if (!exception.IsBackTopScreenError())
		{
			bool isDisplayRetryButton = this.isRequestRetry;
			if (APIUtil.Instance().alertOnlyCloseButton)
			{
				isDisplayRetryButton = false;
			}
			apiAlert.NetworkAPIError(exception, isDisplayRetryButton);
			while (apiAlert.IsOpen())
			{
				yield return null;
			}
			if (apiAlert.SelectedRetryButton)
			{
				this.requestStatus = WebAPI.RequestStatus.RETRY;
				this.afterAlertClosedBehavior = TaskBase.AfterAlertClosed.RETRY;
			}
			else
			{
				this.afterAlertClosedBehavior = TaskBase.AfterAlertClosed.RETURN;
			}
			yield break;
		}
		apiAlert.NetworkAPIException(exception);
		for (;;)
		{
			yield return null;
		}
	}
}
