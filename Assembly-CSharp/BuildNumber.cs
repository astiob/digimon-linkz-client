using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

public class BuildNumber
{
	private readonly string textPath = "/BuildNumber/BuildNumber.txt";

	private static BuildNumber instance = null;

	private static string loadedText = string.Empty;

	private BuildNumber()
	{
	}

	public static BuildNumber Instance
	{
		get
		{
			if (BuildNumber.instance == null)
			{
				BuildNumber.instance = new BuildNumber();
			}
			return BuildNumber.instance;
		}
	}

	public static string LoadedText
	{
		get
		{
			return BuildNumber.loadedText;
		}
	}

	public Coroutine LoadBuildNumber(Action<string> OnLoaded)
	{
		return AppCoroutine.Start(this.LoadBuildNumber_(OnLoaded), false);
	}

	private IEnumerator LoadBuildNumber_(Action<string> OnLoaded)
	{
		WWW www = new WWW(Application.streamingAssetsPath + this.textPath);
		yield return www;
		if (!string.IsNullOrEmpty(www.error))
		{
			global::Debug.LogError(www.error);
		}
		else
		{
			BuildNumber.loadedText = www.text;
		}
		OnLoaded(BuildNumber.loadedText.Trim());
		yield break;
	}

	public void SaveBuildNumber(string WriteText)
	{
		File.WriteAllText(Application.streamingAssetsPath + this.textPath, WriteText, Encoding.UTF8);
	}
}
