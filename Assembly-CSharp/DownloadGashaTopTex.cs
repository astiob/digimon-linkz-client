using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadGashaTopTex
{
	private readonly float timeoutSeconds = 30f;

	private static DownloadGashaTopTex instance;

	private Texture topTex;

	private Action actCallBackDownload;

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

	public Coroutine DownloadTexture(List<GameWebAPI.RespDataGA_GetGachaInfo.Result> gashaDataList, Action act = null)
	{
		this.actCallBackDownload = act;
		return AppCoroutine.Start(this.DownloadTexture_(gashaDataList), false);
	}

	private IEnumerator DownloadTexture_(List<GameWebAPI.RespDataGA_GetGachaInfo.Result> gashaDataList)
	{
		for (int i = 0; i < gashaDataList.Count; i++)
		{
			GameWebAPI.RespDataGA_GetGachaInfo.Result gashaData = gashaDataList[i];
			if (gashaData != null)
			{
				yield return AppCoroutine.Start(this.DownloadTex(gashaData.mainImagePath), false);
				gashaData.tex = this.topTex;
			}
		}
		if (this.actCallBackDownload != null)
		{
			this.actCallBackDownload();
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
