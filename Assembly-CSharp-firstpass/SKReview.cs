using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SKReview
{
	[DllImport("__Internal")]
	private static extern void SKReviewNativePopUp();

	public static void SKReviewPopUp()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			SKReview.SKReviewNativePopUp();
		}
	}
}
