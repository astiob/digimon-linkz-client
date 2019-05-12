using System;
using UnityEngine;

public class MultiLanguageResources
{
	public static UnityEngine.Object Load(string path, Type systemTypeInstance)
	{
		string countryPrefix = CountrySetting.GetCountryPrefix(CountrySetting.CountryCode.EN);
		string path2 = (!string.IsNullOrEmpty(countryPrefix)) ? string.Format("{0}/{1}", countryPrefix, path) : path;
		UnityEngine.Object @object = Resources.Load(path2, systemTypeInstance);
		if (@object == null)
		{
			@object = Resources.Load(path, systemTypeInstance);
		}
		return @object;
	}
}
