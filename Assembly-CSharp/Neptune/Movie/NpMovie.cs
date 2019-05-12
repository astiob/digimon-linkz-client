using System;
using UnityEngine;

namespace Neptune.Movie
{
	public class NpMovie
	{
		public static readonly string NPMOVIE_ERR_FILE_PATH_NONE = "NPMOVIE_ERR_002";

		private static NpMovie.INpMovieListener mListener;

		private static bool mIsTouchFinish = true;

		private static bool mIsSoundEnable = true;

		private static bool mIsControllerEnabled;

		public static bool TouchFinish
		{
			get
			{
				return NpMovie.mIsTouchFinish;
			}
			set
			{
				NpMovie.mIsTouchFinish = value;
			}
		}

		public static bool SoundEnable
		{
			get
			{
				return NpMovie.mIsSoundEnable;
			}
			set
			{
				NpMovie.mIsSoundEnable = value;
			}
		}

		public static bool ControllerEnabled
		{
			get
			{
				return NpMovie.mIsControllerEnabled;
			}
			set
			{
				NpMovie.mIsControllerEnabled = value;
			}
		}

		public static void Play(string path, GameObject gameObject, NpMovie.INpMovieListener listener)
		{
			NpMovie.mListener = listener;
			if (string.IsNullOrEmpty(path))
			{
				global::Debug.LogError("NpMovie : path IsNullOrEmpty.");
				if (NpMovie.mListener != null)
				{
					NpMovie.mListener.OnMovieErr(NpMovie.NPMOVIE_ERR_FILE_PATH_NONE);
				}
				return;
			}
			NpMovie.Destroy();
			string objectName = (!(gameObject == null)) ? gameObject.name : string.Empty;
			NpMovieAndroid.Instance.Play(path, NpMovie.TouchFinish, NpMovie.SoundEnable, false, NpMovie.ControllerEnabled, objectName);
		}

		public static void PlayStreaming(string path, GameObject gameObject, NpMovie.INpMovieListener listener)
		{
			NpMovie.mListener = listener;
			if (string.IsNullOrEmpty(path))
			{
				global::Debug.LogError("NpMovie : path IsNullOrEmpty.");
				if (NpMovie.mListener != null)
				{
					NpMovie.mListener.OnMovieErr(NpMovie.NPMOVIE_ERR_FILE_PATH_NONE);
				}
				return;
			}
			NpMovie.Destroy();
			string objectName = (!(gameObject == null)) ? gameObject.name : string.Empty;
			NpMovieAndroid.Instance.Play(path, NpMovie.TouchFinish, NpMovie.SoundEnable, true, NpMovie.ControllerEnabled, objectName);
		}

		private static void Destroy()
		{
			NpMovieAndroid.Instance.Destroy();
		}

		public interface INpMovieListener
		{
			void OnMovieFinish();

			void OnMovieErr(string errCode);
		}
	}
}
