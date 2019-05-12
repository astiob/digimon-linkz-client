using System;
using UnityEngine;

public class NativeMessageDialog : MonoBehaviour
{
	private const string androidPluginName = "com.trc.android.plugin.messagedialog.MessageDialog";

	public static void Show(string MessageString)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.messagedialog.MessageDialog"))
		{
			androidJavaClass.CallStatic("Show", new object[]
			{
				MessageString
			});
		}
	}
}
