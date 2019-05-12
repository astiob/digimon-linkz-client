using System;
using System.Collections.Generic;
using UnityEngine.UI.Collections;

namespace UnityEngine.UI
{
	public class GraphicRegistry
	{
		private static GraphicRegistry s_Instance;

		private readonly Dictionary<Canvas, IndexedSet<Graphic>> m_Graphics = new Dictionary<Canvas, IndexedSet<Graphic>>();

		private static readonly List<Graphic> s_EmptyList = new List<Graphic>();

		protected GraphicRegistry()
		{
		}

		public static GraphicRegistry instance
		{
			get
			{
				if (GraphicRegistry.s_Instance == null)
				{
					GraphicRegistry.s_Instance = new GraphicRegistry();
				}
				return GraphicRegistry.s_Instance;
			}
		}

		public static void RegisterGraphicForCanvas(Canvas c, Graphic graphic)
		{
			if (c == null)
			{
				return;
			}
			IndexedSet<Graphic> indexedSet;
			GraphicRegistry.instance.m_Graphics.TryGetValue(c, out indexedSet);
			if (indexedSet != null)
			{
				indexedSet.Add(graphic);
				return;
			}
			indexedSet = new IndexedSet<Graphic>();
			indexedSet.Add(graphic);
			GraphicRegistry.instance.m_Graphics.Add(c, indexedSet);
		}

		public static void UnregisterGraphicForCanvas(Canvas c, Graphic graphic)
		{
			if (c == null)
			{
				return;
			}
			IndexedSet<Graphic> indexedSet;
			if (GraphicRegistry.instance.m_Graphics.TryGetValue(c, out indexedSet))
			{
				indexedSet.Remove(graphic);
			}
		}

		public static IList<Graphic> GetGraphicsForCanvas(Canvas canvas)
		{
			IndexedSet<Graphic> result;
			if (GraphicRegistry.instance.m_Graphics.TryGetValue(canvas, out result))
			{
				return result;
			}
			return GraphicRegistry.s_EmptyList;
		}
	}
}
