using System;
using System.Collections.Generic;
using UnityEngine.Cloud.Service;

namespace UnityEngine.Analytics
{
	internal class DeviceInfoEvent : AnalyticsEvent
	{
		public const string kEventDeviceInfo = "deviceInfo";

		public const string kAppVer = "app_ver";

		public const string kOSVer = "os_ver";

		public const string kDeviceModel = "model";

		public const string kManufacturer = "manufacturer";

		public const string kDeviceMake = "make";

		public const string kGameEngineVer = "engine_ver";

		public const string kProcessorType = "processor_type";

		public const string kSystemMemorySize = "system_memory_size";

		public const string kDeviceId = "deviceid";

		public const string kTimezone = "timezone";

		public const string kChanged = "changed";

		public const string kAppName = "app_name";

		public const string kAppInstallMode = "app_install_mode";

		public const string kRootedJailbroken = "rooted_jailbroken";

		public const string kAdvertisingId = "adsid";

		public const string kAdvertisingTracking = "ads_tracking";

		public const string kDebugBuild = "debug_build";

		public const string kLicenseType = "license_type";

		public DeviceInfoEvent() : base("deviceInfo", CloudEventFlags.CacheImmediately)
		{
		}

		public void SetDeviceMake(string deviceMake)
		{
			base.SetParameter("make", deviceMake);
		}

		public void SetDeviceModel(string deviceModel)
		{
			base.SetParameter("model", deviceModel);
		}

		public void SetOSVersion(string osVer)
		{
			base.SetParameter("os_ver", osVer);
		}

		public void SetGameEngineVersion(string engineVer)
		{
			base.SetParameter("engine_ver", engineVer);
		}

		public void SetProcessorType(string processorType)
		{
			base.SetParameter("processor_type", processorType);
		}

		public void SetSystemMemorySize(string systemMemorySize)
		{
			base.SetParameter("system_memory_size", systemMemorySize);
		}

		public void SetDeviceId(string devId)
		{
			base.SetParameter("deviceid", devId);
		}

		public void SetAdvertisingId(string advertisingId)
		{
			if (!string.IsNullOrEmpty(advertisingId))
			{
				base.SetParameter("adsid", advertisingId);
			}
		}

		public void SetAdvertisingTracking(bool trackingEnabled)
		{
			base.SetParameter("ads_tracking", trackingEnabled);
		}

		public void SetAppVersion(string ver)
		{
			if (!string.IsNullOrEmpty(ver))
			{
				base.SetParameter("app_ver", ver);
			}
		}

		public void SetAppName(string appName)
		{
			if (!string.IsNullOrEmpty(appName))
			{
				base.SetParameter("app_name", appName);
			}
		}

		public void SetAppInstallMode(string installMode)
		{
			if (!string.IsNullOrEmpty(installMode))
			{
				base.SetParameter("app_install_mode", installMode);
			}
		}

		public void SetIsRootedOrJailbroken(bool rootedJailBroken)
		{
			if (rootedJailBroken)
			{
				base.SetParameter("rooted_jailbroken", rootedJailBroken);
			}
		}

		public void SetIsDebugBuild(bool debugBuild)
		{
			base.SetParameter("debug_build", debugBuild);
		}

		public void SetLicenseType(string licenseType)
		{
			if (!string.IsNullOrEmpty(licenseType))
			{
				base.SetParameter("license_type", licenseType);
			}
		}

		public void SetTimezone(string timezone)
		{
			if (!string.IsNullOrEmpty(timezone))
			{
				base.SetParameter("timezone", timezone);
			}
		}

		public void SetChanged(List<string> changed)
		{
			base.SetParameter("changed", changed);
		}
	}
}
