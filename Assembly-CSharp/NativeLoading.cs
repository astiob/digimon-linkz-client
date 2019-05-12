using System;
using UnityEngine;

public class NativeLoading : MonoBehaviour
{
	private readonly string[] longImageFilePaths = new string[]
	{
		"NativeLoading/Common02_Loading_1.png",
		"NativeLoading/Common02_Loading_2.png",
		"NativeLoading/Common02_Loading_3.png",
		"NativeLoading/Common02_Loading_4.png",
		"NativeLoading/Common02_Loading_5.png",
		"NativeLoading/Common02_Loading_6.png"
	};

	private readonly string[] shortImageFilePaths = new string[]
	{
		"NativeLoading/Common02_Loading1_1.png",
		"NativeLoading/Common02_Loading1_2.png",
		"NativeLoading/Common02_Loading1_3.png"
	};

	private readonly int longImageHeight = 70;

	private readonly int longImageWitdh = 262;

	private static NativeLoading instance;

	private bool isLoading;

	private NativeLoading.LoadingType currentType = NativeLoading.LoadingType.None;

	private NativeLoading()
	{
	}

	public static NativeLoading Instance
	{
		get
		{
			if (NativeLoading.instance == null)
			{
				NativeLoading.instance = new GameObject("NativeLoading").AddComponent<NativeLoading>();
			}
			return NativeLoading.instance;
		}
	}

	public bool IsLoading
	{
		get
		{
			return this.isLoading;
		}
	}

	private void InitializeLongLoading()
	{
		long animeIntervalMiliSec = 500L;
		int num = NativeSpriteAnimation.GetScreenHeight / 10;
		int imageWidth = (int)((float)num * 3.74f);
		int num2 = num / 2;
		NativeSpriteAnimation.ClearImage();
		NativeSpriteAnimation.SetAnimeIntervalMiliSec(animeIntervalMiliSec);
		NativeSpriteAnimation.SetImageWidth(imageWidth);
		NativeSpriteAnimation.SetImageHeight(num);
		foreach (string image in this.longImageFilePaths)
		{
			NativeSpriteAnimation.SetImage(image);
		}
		NativeSpriteAnimation.ResetGravity();
		NativeSpriteAnimation.SetGravity("RIGHT");
		NativeSpriteAnimation.SetGravity("BOTTOM");
		NativeSpriteAnimation.SetPaddingRight(num2);
		NativeSpriteAnimation.SetPaddingBottom(num2);
		this.currentType = NativeLoading.LoadingType.Long;
	}

	private void InitializeShortLoading()
	{
		long animeIntervalMiliSec = 500L;
		int num = NativeSpriteAnimation.GetScreenHeight / 10;
		NativeSpriteAnimation.ClearImage();
		NativeSpriteAnimation.SetAnimeIntervalMiliSec(animeIntervalMiliSec);
		NativeSpriteAnimation.SetImageWidth(num);
		NativeSpriteAnimation.SetImageHeight(num);
		foreach (string image in this.shortImageFilePaths)
		{
			NativeSpriteAnimation.SetImage(image);
		}
		NativeSpriteAnimation.ResetGravity();
		NativeSpriteAnimation.SetGravity("RIGHT");
		NativeSpriteAnimation.SetGravity("BOTTOM");
		NativeSpriteAnimation.SetPaddingRight(num);
		NativeSpriteAnimation.SetPaddingBottom(num);
		this.currentType = NativeLoading.LoadingType.Short;
	}

	public void StartAnimation(NativeLoading.LoadingType Type)
	{
		if (this.isLoading)
		{
			return;
		}
		this.isLoading = true;
		if (this.currentType != Type)
		{
			if (Type == NativeLoading.LoadingType.Long)
			{
				this.InitializeLongLoading();
			}
			else if (Type == NativeLoading.LoadingType.Short)
			{
				this.InitializeShortLoading();
			}
		}
		NativeSpriteAnimation.Start();
	}

	public void StopAnimation()
	{
		NativeSpriteAnimation.Stop();
		this.isLoading = false;
	}

	public enum LoadingType
	{
		None = -1,
		Short,
		Long
	}
}
