using System;

namespace UnityEngine.UI
{
	internal static class Misc
	{
		public static void Destroy(Object obj)
		{
			if (obj != null)
			{
				if (Application.isPlaying)
				{
					if (obj is GameObject)
					{
						GameObject gameObject = obj as GameObject;
						gameObject.transform.parent = null;
					}
					Object.Destroy(obj);
				}
				else
				{
					Object.DestroyImmediate(obj);
				}
			}
		}

		public static void DestroyImmediate(Object obj)
		{
			if (obj != null)
			{
				if (Application.isEditor)
				{
					Object.DestroyImmediate(obj);
				}
				else
				{
					Object.Destroy(obj);
				}
			}
		}
	}
}
