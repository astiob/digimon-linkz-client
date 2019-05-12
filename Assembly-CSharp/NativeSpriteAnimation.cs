using System;
using UnityEngine;

public class NativeSpriteAnimation
{
	private const string androidPluginName = "com.trc.android.plugin.spriteanimation.SpriteAnimation";

	private static bool isAnimating;

	public static int GetScreenWidth
	{
		get
		{
			return Screen.width;
		}
	}

	public static int GetScreenHeight
	{
		get
		{
			return Screen.height;
		}
	}

	public static void Start()
	{
		if (NativeSpriteAnimation.isAnimating)
		{
			return;
		}
		NativeSpriteAnimation.isAnimating = true;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.spriteanimation.SpriteAnimation"))
		{
			androidJavaClass.CallStatic("Start", new object[0]);
		}
	}

	public static void Stop()
	{
		if (!NativeSpriteAnimation.isAnimating)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.spriteanimation.SpriteAnimation"))
		{
			androidJavaClass.CallStatic("Stop", new object[0]);
		}
		NativeSpriteAnimation.isAnimating = false;
	}

	public static void SetAnimeIntervalMiliSec(long MiliTime)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.spriteanimation.SpriteAnimation"))
		{
			androidJavaClass.CallStatic("SetAnimeIntervalMiliSec", new object[]
			{
				MiliTime
			});
		}
	}

	public static void SetImageWidth(int ImageWitch)
	{
		if (NativeSpriteAnimation.isAnimating)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.spriteanimation.SpriteAnimation"))
		{
			androidJavaClass.CallStatic("SetImageWidth", new object[]
			{
				ImageWitch
			});
		}
	}

	public static void SetImageHeight(int ImageHeight)
	{
		if (NativeSpriteAnimation.isAnimating)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.spriteanimation.SpriteAnimation"))
		{
			androidJavaClass.CallStatic("SetImageHeight", new object[]
			{
				ImageHeight
			});
		}
	}

	public static void SetImage(string FilePath)
	{
		if (NativeSpriteAnimation.isAnimating)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.spriteanimation.SpriteAnimation"))
		{
			androidJavaClass.CallStatic("SetImage", new object[]
			{
				FilePath
			});
		}
	}

	public static void ClearImage()
	{
		if (NativeSpriteAnimation.isAnimating)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.spriteanimation.SpriteAnimation"))
		{
			androidJavaClass.CallStatic("ClearImage", new object[0]);
		}
	}

	public static void ResizeImage()
	{
		if (NativeSpriteAnimation.isAnimating)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.spriteanimation.SpriteAnimation"))
		{
			androidJavaClass.CallStatic("ResizeImage", new object[0]);
		}
	}

	public static void SetPaddingLeft(int Value)
	{
		if (NativeSpriteAnimation.isAnimating)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.spriteanimation.SpriteAnimation"))
		{
			androidJavaClass.CallStatic("SetPaddingLeft", new object[]
			{
				Value
			});
		}
	}

	public static void SetPaddingTop(int Value)
	{
		if (NativeSpriteAnimation.isAnimating)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.spriteanimation.SpriteAnimation"))
		{
			androidJavaClass.CallStatic("SetPaddingTop", new object[]
			{
				Value
			});
		}
	}

	public static void SetPaddingRight(int Value)
	{
		if (NativeSpriteAnimation.isAnimating)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.spriteanimation.SpriteAnimation"))
		{
			androidJavaClass.CallStatic("SetPaddingRight", new object[]
			{
				Value
			});
		}
	}

	public static void SetPaddingBottom(int Value)
	{
		if (NativeSpriteAnimation.isAnimating)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.spriteanimation.SpriteAnimation"))
		{
			androidJavaClass.CallStatic("SetPaddingBottom", new object[]
			{
				Value
			});
		}
	}

	public static void SetGravity(string ConstantString)
	{
		if (NativeSpriteAnimation.isAnimating)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.spriteanimation.SpriteAnimation"))
		{
			androidJavaClass.CallStatic("SetGravity", new object[]
			{
				ConstantString
			});
		}
	}

	public static void ResetGravity()
	{
		if (NativeSpriteAnimation.isAnimating)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.spriteanimation.SpriteAnimation"))
		{
			androidJavaClass.CallStatic("ResetGravity", new object[0]);
		}
	}
}
