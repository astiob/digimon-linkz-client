using System;
using System.Collections;

namespace UnityEngine.Advertisements
{
	internal static class AsyncExec
	{
		private static GameObject s_GameObject;

		private static MonoBehaviour s_CoroutineHost;

		private static MonoBehaviour coroutineHost
		{
			get
			{
				if (AsyncExec.s_CoroutineHost == null)
				{
					AsyncExec.s_GameObject = new GameObject("Unity Ads Coroutine Host")
					{
						hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset)
					};
					AsyncExec.s_CoroutineHost = AsyncExec.s_GameObject.AddComponent<MonoBehaviour>();
					Object.DontDestroyOnLoad(AsyncExec.s_GameObject);
				}
				return AsyncExec.s_CoroutineHost;
			}
		}

		public static Coroutine StartCoroutine(IEnumerator enumerator)
		{
			return AsyncExec.coroutineHost.StartCoroutine(enumerator);
		}
	}
}
