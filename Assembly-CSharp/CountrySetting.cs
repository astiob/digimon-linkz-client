using Master;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public static class CountrySetting
{
	public static readonly Dictionary<int, string> CountryPrefix = new Dictionary<int, string>
	{
		{
			1,
			"jp"
		},
		{
			2,
			"en"
		},
		{
			3,
			"cn"
		},
		{
			4,
			"kr"
		}
	};

	public static readonly Dictionary<SystemLanguage, CountrySetting.CountryCode> SystemLangCountryCode = new Dictionary<SystemLanguage, CountrySetting.CountryCode>
	{
		{
			SystemLanguage.Chinese,
			CountrySetting.CountryCode.CN
		},
		{
			SystemLanguage.ChineseSimplified,
			CountrySetting.CountryCode.CN
		},
		{
			SystemLanguage.ChineseTraditional,
			CountrySetting.CountryCode.CN
		},
		{
			SystemLanguage.Korean,
			CountrySetting.CountryCode.KR
		}
	};

	public static void SetCountryCode(string countryCode, CountrySetting.CountryCode defaultCountryCode = CountrySetting.CountryCode.EN)
	{
		countryCode = 1.ToString();
		PlayerPrefs.SetString("PlayerCountryCode", countryCode);
		PlayerPrefs.Save();
		if (DataMng.Instance().RespDataUS_PlayerInfo != null)
		{
			DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.countryCode = countryCode;
		}
	}

	public static string GetCountryCode(CountrySetting.CountryCode defaultCountryCode = CountrySetting.CountryCode.EN)
	{
		return 1.ToString();
	}

	public static string GetCountryPrefix(CountrySetting.CountryCode defaultCountryCode = CountrySetting.CountryCode.EN)
	{
		string countryCode = CountrySetting.GetCountryCode(defaultCountryCode);
		return CountrySetting.CountryPrefix[int.Parse(countryCode)];
	}

	public static bool IsReloadRequired(string countryCode)
	{
		return !countryCode.Equals(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
	}

	public static string GetSystemCountryCode(CountrySetting.CountryCode defaultCountryCode = CountrySetting.CountryCode.EN)
	{
		SystemLanguage systemLanguage = Application.systemLanguage;
		if (CountrySetting.SystemLangCountryCode.ContainsKey(systemLanguage))
		{
			return ((int)CountrySetting.SystemLangCountryCode[systemLanguage]).ToString();
		}
		int num = (int)defaultCountryCode;
		return num.ToString();
	}

	public static void ReloadMaster()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
		foreach (FileInfo fileInfo in directoryInfo.GetFiles())
		{
			string name = fileInfo.Name;
			if (name.StartsWith("MA_"))
			{
				File.Delete(Application.persistentDataPath + "/" + name);
			}
		}
		MasterDataMng.Instance().ClearCache();
		StringMaster.Reload();
		AlertMaster.Reload();
	}

	public static void ConvertTMProText(ref TextMeshPro textMeshPro)
	{
	}

	public enum CountryCode
	{
		JP = 1,
		EN,
		CN,
		KR
	}
}
