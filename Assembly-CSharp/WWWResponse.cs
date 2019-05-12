using System;

public sealed class WWWResponse
{
	public string responseJson;

	public string errorText;

	public WWWResponse.LocalErrorStatus errorStatus;

	public enum VenusStatus
	{
		NONE,
		RESPONSE_SUCCESS,
		RESPONSE_ERROR,
		RESPONSE_MAINTENANCE,
		RESPONSE_PULTIPLE,
		RESPONSE_OLDVERSION,
		RESPONSE_SERVER_LOAD_HIGH,
		RESPONSE_APPLICATION_ERROR,
		RESPONSE_CSRF_ERROR,
		RESPONSE_NOT_ENOUGH_BP_ERROR,
		RESPONSE_NOT_ENOUGH_AP_ERROR,
		RESPONSE_PENALTY,
		RESPONSE_TIMESTAMP
	}

	public enum LocalErrorStatus
	{
		NONE,
		LOCAL_ERROR_TIMEOUT,
		LOCAL_ERROR_WWW,
		LOCAL_ERROR_JSONPARSE
	}
}
