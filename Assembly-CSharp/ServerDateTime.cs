using System;

public class ServerDateTime
{
	private static TimeSpan differentialTimeSpan = default(TimeSpan);

	private static bool isInitialized = false;

	public static bool isUpdateServerDateTime = false;

	private ServerDateTime()
	{
	}

	public static DateTime Now
	{
		get
		{
			if (ServerDateTime.isInitialized)
			{
				return DateTime.Now - ServerDateTime.differentialTimeSpan;
			}
			return DateTime.Now;
		}
	}

	public static DateTime UtcNow
	{
		get
		{
			return ServerDateTime.Now.ToUniversalTime();
		}
	}

	public static void Initialize(DateTime ServerTime)
	{
		ServerDateTime.differentialTimeSpan = DateTime.Now - ServerTime;
		ServerDateTime.isInitialized = true;
	}

	public static void Initialize(string ServerTimeString)
	{
		ServerDateTime.differentialTimeSpan = DateTime.Now - DateTime.Parse(ServerTimeString);
		ServerDateTime.isInitialized = true;
	}

	public static void UpdateServerDateTime()
	{
		if (!ServerDateTime.isUpdateServerDateTime)
		{
			return;
		}
		GameWebAPI.Request_CM_GetSystemDateTime request_CM_GetSystemDateTime = new GameWebAPI.Request_CM_GetSystemDateTime();
		request_CM_GetSystemDateTime.OnReceived = delegate(GameWebAPI.RespDataCM_GetSystemDateTime response)
		{
			ServerDateTime.Initialize(response.nowDateTime);
		};
		GameWebAPI.Request_CM_GetSystemDateTime request = request_CM_GetSystemDateTime;
		AppCoroutine.Start(request.Run(null, null, null), false);
	}
}
