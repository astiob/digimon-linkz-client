using System;
using UnityEngine;

namespace Neptune.Movie
{
	public class NpMovieAndroid
	{
		private static NpMovieAndroid mInstance = new NpMovieAndroid();

		private NpMovieAndroid()
		{
		}

		public static NpMovieAndroid Instance
		{
			get
			{
				return NpMovieAndroid.mInstance;
			}
		}

		public void Play(string filePath, bool isTouchFinish, bool isSoundEnable, bool isStreaming, bool isControllerEnabled, string objectName)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.Movie.NPVideoView"))
			{
				androidJavaClass.CallStatic("playVideo", new object[]
				{
					filePath,
					isTouchFinish,
					isSoundEnable,
					isStreaming,
					isControllerEnabled,
					objectName
				});
			}
		}

		public void Destroy()
		{
		}
	}
}
