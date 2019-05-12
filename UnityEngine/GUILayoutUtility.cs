using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngineInternal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Utility functions for implementing and extending the GUILayout class.</para>
	/// </summary>
	public class GUILayoutUtility
	{
		private static readonly Dictionary<int, GUILayoutUtility.LayoutCache> s_StoredLayouts = new Dictionary<int, GUILayoutUtility.LayoutCache>();

		private static readonly Dictionary<int, GUILayoutUtility.LayoutCache> s_StoredWindows = new Dictionary<int, GUILayoutUtility.LayoutCache>();

		internal static GUILayoutUtility.LayoutCache current = new GUILayoutUtility.LayoutCache();

		private static readonly Rect kDummyRect = new Rect(0f, 0f, 1f, 1f);

		private static GUIStyle s_SpaceStyle;

		internal static GUILayoutUtility.LayoutCache SelectIDList(int instanceID, bool isWindow)
		{
			Dictionary<int, GUILayoutUtility.LayoutCache> dictionary = (!isWindow) ? GUILayoutUtility.s_StoredLayouts : GUILayoutUtility.s_StoredWindows;
			GUILayoutUtility.LayoutCache layoutCache;
			if (!dictionary.TryGetValue(instanceID, out layoutCache))
			{
				layoutCache = new GUILayoutUtility.LayoutCache();
				dictionary[instanceID] = layoutCache;
			}
			GUILayoutUtility.current.topLevel = layoutCache.topLevel;
			GUILayoutUtility.current.layoutGroups = layoutCache.layoutGroups;
			GUILayoutUtility.current.windows = layoutCache.windows;
			return layoutCache;
		}

		internal static void Begin(int instanceID)
		{
			GUILayoutUtility.LayoutCache layoutCache = GUILayoutUtility.SelectIDList(instanceID, false);
			if (Event.current.type == EventType.Layout)
			{
				GUILayoutUtility.current.topLevel = (layoutCache.topLevel = new GUILayoutGroup());
				GUILayoutUtility.current.layoutGroups.Clear();
				GUILayoutUtility.current.layoutGroups.Push(GUILayoutUtility.current.topLevel);
				GUILayoutUtility.current.windows = (layoutCache.windows = new GUILayoutGroup());
			}
			else
			{
				GUILayoutUtility.current.topLevel = layoutCache.topLevel;
				GUILayoutUtility.current.layoutGroups = layoutCache.layoutGroups;
				GUILayoutUtility.current.windows = layoutCache.windows;
			}
		}

		internal static void BeginWindow(int windowID, GUIStyle style, GUILayoutOption[] options)
		{
			GUILayoutUtility.LayoutCache layoutCache = GUILayoutUtility.SelectIDList(windowID, true);
			if (Event.current.type == EventType.Layout)
			{
				GUILayoutUtility.current.topLevel = (layoutCache.topLevel = new GUILayoutGroup());
				GUILayoutUtility.current.topLevel.style = style;
				GUILayoutUtility.current.topLevel.windowID = windowID;
				if (options != null)
				{
					GUILayoutUtility.current.topLevel.ApplyOptions(options);
				}
				GUILayoutUtility.current.layoutGroups.Clear();
				GUILayoutUtility.current.layoutGroups.Push(GUILayoutUtility.current.topLevel);
				GUILayoutUtility.current.windows = (layoutCache.windows = new GUILayoutGroup());
			}
			else
			{
				GUILayoutUtility.current.topLevel = layoutCache.topLevel;
				GUILayoutUtility.current.layoutGroups = layoutCache.layoutGroups;
				GUILayoutUtility.current.windows = layoutCache.windows;
			}
		}

		public static void BeginGroup(string GroupName)
		{
		}

		public static void EndGroup(string groupName)
		{
		}

		internal static void Layout()
		{
			if (GUILayoutUtility.current.topLevel.windowID == -1)
			{
				GUILayoutUtility.current.topLevel.CalcWidth();
				GUILayoutUtility.current.topLevel.SetHorizontal(0f, Mathf.Min((float)Screen.width / GUIUtility.pixelsPerPoint, GUILayoutUtility.current.topLevel.maxWidth));
				GUILayoutUtility.current.topLevel.CalcHeight();
				GUILayoutUtility.current.topLevel.SetVertical(0f, Mathf.Min((float)Screen.height / GUIUtility.pixelsPerPoint, GUILayoutUtility.current.topLevel.maxHeight));
				GUILayoutUtility.LayoutFreeGroup(GUILayoutUtility.current.windows);
			}
			else
			{
				GUILayoutUtility.LayoutSingleGroup(GUILayoutUtility.current.topLevel);
				GUILayoutUtility.LayoutFreeGroup(GUILayoutUtility.current.windows);
			}
		}

		internal static void LayoutFromEditorWindow()
		{
			GUILayoutUtility.current.topLevel.CalcWidth();
			GUILayoutUtility.current.topLevel.SetHorizontal(0f, (float)Screen.width / GUIUtility.pixelsPerPoint);
			GUILayoutUtility.current.topLevel.CalcHeight();
			GUILayoutUtility.current.topLevel.SetVertical(0f, (float)Screen.height / GUIUtility.pixelsPerPoint);
			GUILayoutUtility.LayoutFreeGroup(GUILayoutUtility.current.windows);
		}

		internal static float LayoutFromInspector(float width)
		{
			if (GUILayoutUtility.current.topLevel != null && GUILayoutUtility.current.topLevel.windowID == -1)
			{
				GUILayoutUtility.current.topLevel.CalcWidth();
				GUILayoutUtility.current.topLevel.SetHorizontal(0f, width);
				GUILayoutUtility.current.topLevel.CalcHeight();
				GUILayoutUtility.current.topLevel.SetVertical(0f, Mathf.Min((float)Screen.height / GUIUtility.pixelsPerPoint, GUILayoutUtility.current.topLevel.maxHeight));
				float minHeight = GUILayoutUtility.current.topLevel.minHeight;
				GUILayoutUtility.LayoutFreeGroup(GUILayoutUtility.current.windows);
				return minHeight;
			}
			if (GUILayoutUtility.current.topLevel != null)
			{
				GUILayoutUtility.LayoutSingleGroup(GUILayoutUtility.current.topLevel);
			}
			return 0f;
		}

		internal static void LayoutFreeGroup(GUILayoutGroup toplevel)
		{
			foreach (GUILayoutEntry guilayoutEntry in toplevel.entries)
			{
				GUILayoutGroup i = (GUILayoutGroup)guilayoutEntry;
				GUILayoutUtility.LayoutSingleGroup(i);
			}
			toplevel.ResetCursor();
		}

		private static void LayoutSingleGroup(GUILayoutGroup i)
		{
			if (!i.isWindow)
			{
				float minWidth = i.minWidth;
				float maxWidth = i.maxWidth;
				i.CalcWidth();
				i.SetHorizontal(i.rect.x, Mathf.Clamp(i.maxWidth, minWidth, maxWidth));
				float minHeight = i.minHeight;
				float maxHeight = i.maxHeight;
				i.CalcHeight();
				i.SetVertical(i.rect.y, Mathf.Clamp(i.maxHeight, minHeight, maxHeight));
			}
			else
			{
				i.CalcWidth();
				Rect rect = GUILayoutUtility.Internal_GetWindowRect(i.windowID);
				i.SetHorizontal(rect.x, Mathf.Clamp(rect.width, i.minWidth, i.maxWidth));
				i.CalcHeight();
				i.SetVertical(rect.y, Mathf.Clamp(rect.height, i.minHeight, i.maxHeight));
				GUILayoutUtility.Internal_MoveWindow(i.windowID, i.rect);
			}
		}

		[SecuritySafeCritical]
		private static GUILayoutGroup CreateGUILayoutGroupInstanceOfType(Type LayoutType)
		{
			if (!typeof(GUILayoutGroup).IsAssignableFrom(LayoutType))
			{
				throw new ArgumentException("LayoutType needs to be of type GUILayoutGroup");
			}
			return (GUILayoutGroup)Activator.CreateInstance(LayoutType);
		}

		internal static GUILayoutGroup BeginLayoutGroup(GUIStyle style, GUILayoutOption[] options, Type layoutType)
		{
			EventType type = Event.current.type;
			GUILayoutGroup guilayoutGroup;
			if (type != EventType.Layout && type != EventType.Used)
			{
				guilayoutGroup = (GUILayoutUtility.current.topLevel.GetNext() as GUILayoutGroup);
				if (guilayoutGroup == null)
				{
					throw new ArgumentException("GUILayout: Mismatched LayoutGroup." + Event.current.type);
				}
				guilayoutGroup.ResetCursor();
			}
			else
			{
				guilayoutGroup = GUILayoutUtility.CreateGUILayoutGroupInstanceOfType(layoutType);
				guilayoutGroup.style = style;
				if (options != null)
				{
					guilayoutGroup.ApplyOptions(options);
				}
				GUILayoutUtility.current.topLevel.Add(guilayoutGroup);
			}
			GUILayoutUtility.current.layoutGroups.Push(guilayoutGroup);
			GUILayoutUtility.current.topLevel = guilayoutGroup;
			return guilayoutGroup;
		}

		internal static void EndLayoutGroup()
		{
			EventType type = Event.current.type;
			GUILayoutUtility.current.layoutGroups.Pop();
			GUILayoutUtility.current.topLevel = (GUILayoutGroup)GUILayoutUtility.current.layoutGroups.Peek();
		}

		internal static GUILayoutGroup BeginLayoutArea(GUIStyle style, Type layoutType)
		{
			EventType type = Event.current.type;
			GUILayoutGroup guilayoutGroup;
			if (type != EventType.Layout && type != EventType.Used)
			{
				guilayoutGroup = (GUILayoutUtility.current.windows.GetNext() as GUILayoutGroup);
				if (guilayoutGroup == null)
				{
					throw new ArgumentException("GUILayout: Mismatched LayoutGroup." + Event.current.type);
				}
				guilayoutGroup.ResetCursor();
			}
			else
			{
				guilayoutGroup = GUILayoutUtility.CreateGUILayoutGroupInstanceOfType(layoutType);
				guilayoutGroup.style = style;
				GUILayoutUtility.current.windows.Add(guilayoutGroup);
			}
			GUILayoutUtility.current.layoutGroups.Push(guilayoutGroup);
			GUILayoutUtility.current.topLevel = guilayoutGroup;
			return guilayoutGroup;
		}

		internal static GUILayoutGroup DoBeginLayoutArea(GUIStyle style, Type layoutType)
		{
			return GUILayoutUtility.BeginLayoutArea(style, layoutType);
		}

		internal static GUILayoutGroup topLevel
		{
			get
			{
				return GUILayoutUtility.current.topLevel;
			}
		}

		/// <summary>
		///   <para>Reserve layout space for a rectangle for displaying some contents with a specific style.</para>
		/// </summary>
		/// <param name="content">The content to make room for displaying.</param>
		/// <param name="style">The GUIStyle to layout for.</param>
		/// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
		/// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, 
		/// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
		/// <returns>
		///   <para>A rectangle that is large enough to contain content when rendered in style.</para>
		/// </returns>
		public static Rect GetRect(GUIContent content, GUIStyle style)
		{
			return GUILayoutUtility.DoGetRect(content, style, null);
		}

		/// <summary>
		///   <para>Reserve layout space for a rectangle for displaying some contents with a specific style.</para>
		/// </summary>
		/// <param name="content">The content to make room for displaying.</param>
		/// <param name="style">The GUIStyle to layout for.</param>
		/// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
		/// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, 
		/// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
		/// <returns>
		///   <para>A rectangle that is large enough to contain content when rendered in style.</para>
		/// </returns>
		public static Rect GetRect(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayoutUtility.DoGetRect(content, style, options);
		}

		private static Rect DoGetRect(GUIContent content, GUIStyle style, GUILayoutOption[] options)
		{
			GUIUtility.CheckOnGUI();
			EventType type = Event.current.type;
			if (type == EventType.Layout)
			{
				if (style.isHeightDependantOnWidth)
				{
					GUILayoutUtility.current.topLevel.Add(new GUIWordWrapSizer(style, content, options));
				}
				else
				{
					Vector2 vector = style.CalcSize(content);
					GUILayoutUtility.current.topLevel.Add(new GUILayoutEntry(vector.x, vector.x, vector.y, vector.y, style, options));
				}
				return GUILayoutUtility.kDummyRect;
			}
			if (type != EventType.Used)
			{
				return GUILayoutUtility.current.topLevel.GetNext().rect;
			}
			return GUILayoutUtility.kDummyRect;
		}

		/// <summary>
		///   <para>Reserve layout space for a rectangle with a fixed content area.</para>
		/// </summary>
		/// <param name="width">The width of the area you want.</param>
		/// <param name="height">The height of the area you want.</param>
		/// <param name="style">An optional GUIStyle to layout for. If specified, the style's padding value will be added to your sizes &amp; its margin value will be used for spacing.</param>
		/// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
		/// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, 
		/// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
		/// <returns>
		///   <para>The rectanlge to put your control in.</para>
		/// </returns>
		public static Rect GetRect(float width, float height)
		{
			return GUILayoutUtility.DoGetRect(width, width, height, height, GUIStyle.none, null);
		}

		/// <summary>
		///   <para>Reserve layout space for a rectangle with a fixed content area.</para>
		/// </summary>
		/// <param name="width">The width of the area you want.</param>
		/// <param name="height">The height of the area you want.</param>
		/// <param name="style">An optional GUIStyle to layout for. If specified, the style's padding value will be added to your sizes &amp; its margin value will be used for spacing.</param>
		/// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
		/// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, 
		/// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
		/// <returns>
		///   <para>The rectanlge to put your control in.</para>
		/// </returns>
		public static Rect GetRect(float width, float height, GUIStyle style)
		{
			return GUILayoutUtility.DoGetRect(width, width, height, height, style, null);
		}

		/// <summary>
		///   <para>Reserve layout space for a rectangle with a fixed content area.</para>
		/// </summary>
		/// <param name="width">The width of the area you want.</param>
		/// <param name="height">The height of the area you want.</param>
		/// <param name="style">An optional GUIStyle to layout for. If specified, the style's padding value will be added to your sizes &amp; its margin value will be used for spacing.</param>
		/// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
		/// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, 
		/// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
		/// <returns>
		///   <para>The rectanlge to put your control in.</para>
		/// </returns>
		public static Rect GetRect(float width, float height, params GUILayoutOption[] options)
		{
			return GUILayoutUtility.DoGetRect(width, width, height, height, GUIStyle.none, options);
		}

		/// <summary>
		///   <para>Reserve layout space for a rectangle with a fixed content area.</para>
		/// </summary>
		/// <param name="width">The width of the area you want.</param>
		/// <param name="height">The height of the area you want.</param>
		/// <param name="style">An optional GUIStyle to layout for. If specified, the style's padding value will be added to your sizes &amp; its margin value will be used for spacing.</param>
		/// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
		/// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, 
		/// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
		/// <returns>
		///   <para>The rectanlge to put your control in.</para>
		/// </returns>
		public static Rect GetRect(float width, float height, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayoutUtility.DoGetRect(width, width, height, height, style, options);
		}

		/// <summary>
		///   <para>Reserve layout space for a flexible rect.</para>
		/// </summary>
		/// <param name="minWidth">The minimum width of the area passed back.</param>
		/// <param name="maxWidth">The maximum width of the area passed back.</param>
		/// <param name="minHeight">The minimum width of the area passed back.</param>
		/// <param name="maxHeight">The maximum width of the area passed back.</param>
		/// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes requested &amp; the style's margin values will be used for spacing.</param>
		/// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
		/// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, 
		/// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
		/// <returns>
		///   <para>A rectangle with size between minWidth &amp; maxWidth on both axes.</para>
		/// </returns>
		public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight)
		{
			return GUILayoutUtility.DoGetRect(minWidth, maxWidth, minHeight, maxHeight, GUIStyle.none, null);
		}

		/// <summary>
		///   <para>Reserve layout space for a flexible rect.</para>
		/// </summary>
		/// <param name="minWidth">The minimum width of the area passed back.</param>
		/// <param name="maxWidth">The maximum width of the area passed back.</param>
		/// <param name="minHeight">The minimum width of the area passed back.</param>
		/// <param name="maxHeight">The maximum width of the area passed back.</param>
		/// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes requested &amp; the style's margin values will be used for spacing.</param>
		/// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
		/// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, 
		/// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
		/// <returns>
		///   <para>A rectangle with size between minWidth &amp; maxWidth on both axes.</para>
		/// </returns>
		public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style)
		{
			return GUILayoutUtility.DoGetRect(minWidth, maxWidth, minHeight, maxHeight, style, null);
		}

		/// <summary>
		///   <para>Reserve layout space for a flexible rect.</para>
		/// </summary>
		/// <param name="minWidth">The minimum width of the area passed back.</param>
		/// <param name="maxWidth">The maximum width of the area passed back.</param>
		/// <param name="minHeight">The minimum width of the area passed back.</param>
		/// <param name="maxHeight">The maximum width of the area passed back.</param>
		/// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes requested &amp; the style's margin values will be used for spacing.</param>
		/// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
		/// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, 
		/// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
		/// <returns>
		///   <para>A rectangle with size between minWidth &amp; maxWidth on both axes.</para>
		/// </returns>
		public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, params GUILayoutOption[] options)
		{
			return GUILayoutUtility.DoGetRect(minWidth, maxWidth, minHeight, maxHeight, GUIStyle.none, options);
		}

		/// <summary>
		///   <para>Reserve layout space for a flexible rect.</para>
		/// </summary>
		/// <param name="minWidth">The minimum width of the area passed back.</param>
		/// <param name="maxWidth">The maximum width of the area passed back.</param>
		/// <param name="minHeight">The minimum width of the area passed back.</param>
		/// <param name="maxHeight">The maximum width of the area passed back.</param>
		/// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes requested &amp; the style's margin values will be used for spacing.</param>
		/// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
		/// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, 
		/// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
		/// <returns>
		///   <para>A rectangle with size between minWidth &amp; maxWidth on both axes.</para>
		/// </returns>
		public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayoutUtility.DoGetRect(minWidth, maxWidth, minHeight, maxHeight, style, options);
		}

		private static Rect DoGetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style, GUILayoutOption[] options)
		{
			EventType type = Event.current.type;
			if (type == EventType.Layout)
			{
				GUILayoutUtility.current.topLevel.Add(new GUILayoutEntry(minWidth, maxWidth, minHeight, maxHeight, style, options));
				return GUILayoutUtility.kDummyRect;
			}
			if (type != EventType.Used)
			{
				return GUILayoutUtility.current.topLevel.GetNext().rect;
			}
			return GUILayoutUtility.kDummyRect;
		}

		/// <summary>
		///   <para>Get the rectangle last used by GUILayout for a control.</para>
		/// </summary>
		/// <returns>
		///   <para>The last used rectangle.</para>
		/// </returns>
		public static Rect GetLastRect()
		{
			EventType type = Event.current.type;
			if (type == EventType.Layout)
			{
				return GUILayoutUtility.kDummyRect;
			}
			if (type != EventType.Used)
			{
				return GUILayoutUtility.current.topLevel.GetLast();
			}
			return GUILayoutUtility.kDummyRect;
		}

		/// <summary>
		///   <para>Reserve layout space for a rectangle with a specific aspect ratio.</para>
		/// </summary>
		/// <param name="aspect">The aspect ratio of the element (width / height).</param>
		/// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes of the returned rectangle &amp; the style's margin values will be used for spacing.</param>
		/// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
		/// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, 
		/// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
		/// <returns>
		///   <para>The rect for the control.</para>
		/// </returns>
		public static Rect GetAspectRect(float aspect)
		{
			return GUILayoutUtility.DoGetAspectRect(aspect, GUIStyle.none, null);
		}

		/// <summary>
		///   <para>Reserve layout space for a rectangle with a specific aspect ratio.</para>
		/// </summary>
		/// <param name="aspect">The aspect ratio of the element (width / height).</param>
		/// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes of the returned rectangle &amp; the style's margin values will be used for spacing.</param>
		/// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
		/// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, 
		/// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
		/// <returns>
		///   <para>The rect for the control.</para>
		/// </returns>
		public static Rect GetAspectRect(float aspect, GUIStyle style)
		{
			return GUILayoutUtility.DoGetAspectRect(aspect, style, null);
		}

		/// <summary>
		///   <para>Reserve layout space for a rectangle with a specific aspect ratio.</para>
		/// </summary>
		/// <param name="aspect">The aspect ratio of the element (width / height).</param>
		/// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes of the returned rectangle &amp; the style's margin values will be used for spacing.</param>
		/// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
		/// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, 
		/// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
		/// <returns>
		///   <para>The rect for the control.</para>
		/// </returns>
		public static Rect GetAspectRect(float aspect, params GUILayoutOption[] options)
		{
			return GUILayoutUtility.DoGetAspectRect(aspect, GUIStyle.none, options);
		}

		/// <summary>
		///   <para>Reserve layout space for a rectangle with a specific aspect ratio.</para>
		/// </summary>
		/// <param name="aspect">The aspect ratio of the element (width / height).</param>
		/// <param name="style">An optional style. If specified, the style's padding value will be added to the sizes of the returned rectangle &amp; the style's margin values will be used for spacing.</param>
		/// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
		/// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, 
		/// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
		/// <returns>
		///   <para>The rect for the control.</para>
		/// </returns>
		public static Rect GetAspectRect(float aspect, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayoutUtility.DoGetAspectRect(aspect, GUIStyle.none, options);
		}

		private static Rect DoGetAspectRect(float aspect, GUIStyle style, GUILayoutOption[] options)
		{
			EventType type = Event.current.type;
			if (type == EventType.Layout)
			{
				GUILayoutUtility.current.topLevel.Add(new GUIAspectSizer(aspect, options));
				return GUILayoutUtility.kDummyRect;
			}
			if (type != EventType.Used)
			{
				return GUILayoutUtility.current.topLevel.GetNext().rect;
			}
			return GUILayoutUtility.kDummyRect;
		}

		internal static GUIStyle spaceStyle
		{
			get
			{
				if (GUILayoutUtility.s_SpaceStyle == null)
				{
					GUILayoutUtility.s_SpaceStyle = new GUIStyle();
				}
				GUILayoutUtility.s_SpaceStyle.stretchWidth = false;
				return GUILayoutUtility.s_SpaceStyle;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Rect Internal_GetWindowRect(int windowID);

		private static void Internal_MoveWindow(int windowID, Rect r)
		{
			GUILayoutUtility.INTERNAL_CALL_Internal_MoveWindow(windowID, ref r);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_MoveWindow(int windowID, ref Rect r);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Rect GetWindowsBounds();

		internal sealed class LayoutCache
		{
			internal GUILayoutGroup topLevel = new GUILayoutGroup();

			internal GenericStack layoutGroups = new GenericStack();

			internal GUILayoutGroup windows = new GUILayoutGroup();

			internal LayoutCache()
			{
				this.layoutGroups.Push(this.topLevel);
			}

			internal LayoutCache(GUILayoutUtility.LayoutCache other)
			{
				this.topLevel = other.topLevel;
				this.layoutGroups = other.layoutGroups;
				this.windows = other.windows;
			}
		}
	}
}
