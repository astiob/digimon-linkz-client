using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DownloadGashaTopTex
{
	private readonly float timeoutSeconds = 30f;

	private static DownloadGashaTopTex instance;

	private Texture topTex;

	public static DownloadGashaTopTex Instance
	{
		get
		{
			if (DownloadGashaTopTex.instance == null)
			{
				DownloadGashaTopTex.instance = new DownloadGashaTopTex();
			}
			return DownloadGashaTopTex.instance;
		}
	}

	public float TimeoutSeconds
	{
		get
		{
			return this.timeoutSeconds;
		}
	}

	public void DownloadTexture(List<GameWebAPI.RespDataGA_GetGachaInfo.Result> gashaInfoList, Action<Texture[]> action)
	{
		AppCoroutine.Start(this.DownloadTexture_(gashaInfoList, action), false);
	}

	private IEnumerator DownloadTexture_(List<GameWebAPI.RespDataGA_GetGachaInfo.Result> gashaInfoList, Action<Texture[]> action)
	{
		Texture[] textureList = new Texture[gashaInfoList.Count];
		for (int i = 0; i < gashaInfoList.Count; i++)
		{
			GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo = gashaInfoList[i];
			if (gashaInfo != null)
			{
				yield return AppCoroutine.Start(this.DownloadTex(gashaInfo.mainImagePath), false);
				textureList[i] = this.topTex;
			}
		}
		if (action != null)
		{
			action(textureList);
		}
		yield break;
	}

	private IEnumerator DownloadTex(string url)
	{
		Action<Texture2D> callback = delegate(Texture2D texture)
		{
			this.topTex = texture;
		};
		string downloadURL = AssetDataMng.GetWebAssetImagePath() + "/gasha/" + url;
		yield return TextureManager.instance.Load(downloadURL, callback, this.TimeoutSeconds, true);
		yield break;
	}
}
