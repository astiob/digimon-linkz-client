using Secure;
using System;

public static class WebAPIPlatformValue
{
	private static readonly string COUNSUMER_KEY = Secure.ConstValue.AND_COUNSUMER_KEY;

	private static readonly string COUNSUMER_SECRET = Secure.ConstValue.AND_COUNSUMER_SECRET;

	private static readonly string HTTP_ADR_AUTH = global::ConstValue.APP_SITE_DOMAIN + "/appApi/AuthUserByUuid";

	private static readonly string HTTP_ADR_ACTIVE_CONTROLLER = global::ConstValue.APP_SITE_DOMAIN + "/app/ActiveController";

	public static string GetAppVersion()
	{
		return VersionManager.version;
	}

	public static string GetCounsumerKey()
	{
		return WebAPIPlatformValue.COUNSUMER_KEY;
	}

	public static string GetCounsumerSecret()
	{
		return WebAPIPlatformValue.COUNSUMER_SECRET;
	}

	public static string GetHttpAddressAuth()
	{
		return WebAPIPlatformValue.HTTP_ADR_AUTH;
	}

	public static string GetHttpActiveController()
	{
		return WebAPIPlatformValue.HTTP_ADR_ACTIVE_CONTROLLER;
	}
}
