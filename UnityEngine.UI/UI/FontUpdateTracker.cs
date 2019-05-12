using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	public static class FontUpdateTracker
	{
		private static Dictionary<Font, List<Text>> m_Tracked = new Dictionary<Font, List<Text>>();

		public static void TrackText(Text t)
		{
			if (t.font == null)
			{
				return;
			}
			List<Text> list;
			FontUpdateTracker.m_Tracked.TryGetValue(t.font, out list);
			if (list == null)
			{
				list = new List<Text>();
				FontUpdateTracker.m_Tracked.Add(t.font, list);
				Font.textureRebuilt += FontUpdateTracker.RebuildForFont;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == t)
				{
					return;
				}
			}
			list.Add(t);
		}

		private static void RebuildForFont(Font f)
		{
			List<Text> list;
			FontUpdateTracker.m_Tracked.TryGetValue(f, out list);
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].FontTextureChanged();
			}
		}

		public static void UntrackText(Text t)
		{
			if (t.font == null)
			{
				return;
			}
			List<Text> list;
			FontUpdateTracker.m_Tracked.TryGetValue(t.font, out list);
			if (list == null)
			{
				return;
			}
			list.Remove(t);
			if (list.Count == 0)
			{
				FontUpdateTracker.m_Tracked.Remove(t.font);
			}
		}
	}
}
