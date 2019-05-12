using System;
using UnityEngine;

public class Clipboard
{
	public static string Text
	{
		set
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.clipboard.ClipboardPlugin"))
			{
				androidJavaClass.CallStatic("SetText", new object[]
				{
					value
				});
			}
		}
	}
}
