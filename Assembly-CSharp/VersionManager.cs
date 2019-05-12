using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

public class VersionManager
{
	private const string textPath = "/BuildNumber/Version.txt";

	private static VersionManager _instance;

	private string _version = string.Empty;

	public static VersionManager instance
	{
		get
		{
			VersionManager.CreateInstance();
			return VersionManager._instance;
		}
	}

	public static string version
	{
		get
		{
			return VersionManager.instance._version;
		}
	}

	public static void Save(string WriteText)
	{
		VersionManager.instance.Save_(WriteText);
	}

	private void Save_(string WriteText)
	{
		File.WriteAllText(Application.streamingAssetsPath + "/BuildNumber/Version.txt", WriteText, Encoding.UTF8);
	}

	public static Coroutine Load(Action<bool, string> OnLoaded)
	{
		return AppCoroutine.Start(VersionManager.instance.Load_(OnLoaded), false);
	}

	public static void Delete()
	{
		string path = Application.streamingAssetsPath + "/BuildNumber/Version.txt";
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	private static void CreateInstance()
	{
		if (VersionManager._instance == null)
		{
			VersionManager._instance = new VersionManager();
		}
	}

	private IEnumerator Load_(Action<bool, string> OnLoaded)
	{
		string fullPath = Application.streamingAssetsPath + "/BuildNumber/Version.txt";
		WWW www = new WWW(fullPath);
		yield return www;
		if (!string.IsNullOrEmpty(www.error))
		{
			global::Debug.LogError(www.error);
			OnLoaded(false, string.Empty);
			yield break;
		}
		this._version = www.text.Trim();
		OnLoaded(true, this._version);
		yield break;
	}
}
