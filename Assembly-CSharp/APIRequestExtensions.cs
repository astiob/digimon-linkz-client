using System;
using System.Collections;
using WebAPIRequest;

public static class APIRequestExtensions
{
	public static IEnumerator RunOneTime(this RequestBase request, Action onSuccess = null, Action<Exception> onFailed = null, Func<Exception, IEnumerator> onAlert = null)
	{
		return new APIRequestTask(request, false).Run(onSuccess, onFailed, onAlert);
	}

	public static IEnumerator Run(this RequestBase request, Action onSuccess = null, Action<Exception> onFailed = null, Func<Exception, IEnumerator> onAlert = null)
	{
		return new APIRequestTask(request, true).Run(onSuccess, onFailed, onAlert);
	}

	public static IEnumerator RunOneTime(this RequestList requestList, Action onSuccess = null, Action<Exception> onFailed = null, Func<Exception, IEnumerator> onAlert = null)
	{
		return new APIRequestTask(requestList, false).Run(onSuccess, onFailed, onAlert);
	}

	public static IEnumerator Run(this RequestList requestList, Action onSuccess = null, Action<Exception> onFailed = null, Func<Exception, IEnumerator> onAlert = null)
	{
		return new APIRequestTask(requestList, true).Run(onSuccess, onFailed, onAlert);
	}
}
