using System;
using UnityEngine;

public static class PlayerPrefsExtentions
{
	public static void DeleteAllGameParams()
	{
		string @string = PlayerPrefs.GetString("uuid");
		bool flag = PlayerPrefs.HasKey("GooglePlaySignState");
		int value = 0;
		if (flag)
		{
			value = PlayerPrefs.GetInt("GooglePlaySignState");
		}
		PlayerPrefs.DeleteAll();
		PlayerPrefs.SetString("uuid", @string);
		if (flag)
		{
			PlayerPrefs.SetInt("GooglePlaySignState", value);
		}
		PlayerPrefs.Save();
	}
}
