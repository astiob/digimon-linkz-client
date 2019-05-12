using System;

namespace Neptune.Setting
{
	public class NpSetting
	{
		public static void OpenAppSettingView(string package)
		{
			NpSettingAndroid.OpenAppSettingView(package);
		}

		public static void OpenSettingView(SettingViewAction action)
		{
			NpSettingAndroid.OpenSettingView(action);
		}
	}
}
