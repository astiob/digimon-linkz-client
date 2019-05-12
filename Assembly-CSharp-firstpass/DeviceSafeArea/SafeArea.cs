using System;
using UnityEngine;

namespace DeviceSafeArea
{
	public static class SafeArea
	{
		private static Rect tempSafeArea = new Rect(0f, 0f, 0f, 0f);

		private static Vector2 tempDeviceScreenSize = new Vector2(0f, 0f);

		public static Rect GetSafeArea()
		{
			float x = 0f;
			float y = 0f;
			float width = (float)Screen.width;
			float height = (float)Screen.height;
			SafeArea.tempSafeArea.Set(x, y, width, height);
			return SafeArea.tempSafeArea;
		}

		public static Vector2 GetDeviceScreenSize()
		{
			float new_x = (float)Screen.width;
			float new_y = (float)Screen.height;
			SafeArea.tempDeviceScreenSize.Set(new_x, new_y);
			return SafeArea.tempDeviceScreenSize;
		}
	}
}
