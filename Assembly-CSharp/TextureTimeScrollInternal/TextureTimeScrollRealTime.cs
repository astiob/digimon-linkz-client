using System;
using UnityEngine;

namespace TextureTimeScrollInternal
{
	[DisallowMultipleComponent]
	public class TextureTimeScrollRealTime : MonoBehaviour
	{
		private static TextureTimeScrollRealTime instanced;

		private static float _time;

		private float currentTime;

		[RuntimeInitializeOnLoadMethod]
		public static void InstanceRealTime()
		{
			if (TextureTimeScrollRealTime.instanced != null)
			{
				return;
			}
			TextureTimeScrollRealTime.instanced = new GameObject("TextureScrollRealTime").AddComponent<TextureTimeScrollRealTime>();
			UnityEngine.Object.DontDestroyOnLoad(TextureTimeScrollRealTime.instanced.gameObject);
		}

		public static float time
		{
			get
			{
				return TextureTimeScrollRealTime._time;
			}
		}

		public static void TimeReset()
		{
			TextureTimeScrollRealTime._time = 0f;
		}

		private void Update()
		{
			if (TextureTimeScroll.objectCount > 0)
			{
				this.currentTime %= float.MaxValue;
				this.currentTime += Time.deltaTime;
			}
			else
			{
				this.currentTime = 0f;
			}
			TextureTimeScrollRealTime._time = this.currentTime;
		}
	}
}
