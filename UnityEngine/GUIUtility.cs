using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Utility class for making new GUI controls.</para>
	/// </summary>
	public class GUIUtility
	{
		internal static int s_SkinMode;

		internal static int s_OriginalID;

		internal static Vector2 s_EditorScreenPointOffset = Vector2.zero;

		internal static bool s_HasKeyboardFocus = false;

		internal static float pixelsPerPoint
		{
			get
			{
				return GUIUtility.Internal_GetPixelsPerPoint();
			}
		}

		/// <summary>
		///   <para>Get a unique ID for a control.</para>
		/// </summary>
		/// <param name="focus"></param>
		/// <param name="position"></param>
		public static int GetControlID(FocusType focus)
		{
			return GUIUtility.GetControlID(0, focus);
		}

		/// <summary>
		///   <para>Get a unique ID for a control, using a the label content as a hint to help ensure correct matching of IDs to controls.</para>
		/// </summary>
		/// <param name="contents"></param>
		/// <param name="focus"></param>
		/// <param name="position"></param>
		public static int GetControlID(GUIContent contents, FocusType focus)
		{
			return GUIUtility.GetControlID(contents.hash, focus);
		}

		/// <summary>
		///   <para>Get a unique ID for a control.</para>
		/// </summary>
		/// <param name="focus"></param>
		/// <param name="position"></param>
		public static int GetControlID(FocusType focus, Rect position)
		{
			return GUIUtility.Internal_GetNextControlID2(0, focus, position);
		}

		/// <summary>
		///   <para>Get a unique ID for a control, using an integer as a hint to help ensure correct matching of IDs to controls.</para>
		/// </summary>
		/// <param name="hint"></param>
		/// <param name="focus"></param>
		/// <param name="position"></param>
		public static int GetControlID(int hint, FocusType focus, Rect position)
		{
			return GUIUtility.Internal_GetNextControlID2(hint, focus, position);
		}

		/// <summary>
		///   <para>Get a unique ID for a control, using a the label content as a hint to help ensure correct matching of IDs to controls.</para>
		/// </summary>
		/// <param name="contents"></param>
		/// <param name="focus"></param>
		/// <param name="position"></param>
		public static int GetControlID(GUIContent contents, FocusType focus, Rect position)
		{
			return GUIUtility.Internal_GetNextControlID2(contents.hash, focus, position);
		}

		/// <summary>
		///   <para>Get a state object from a controlID.</para>
		/// </summary>
		/// <param name="t"></param>
		/// <param name="controlID"></param>
		public static object GetStateObject(Type t, int controlID)
		{
			return GUIStateObjects.GetStateObject(t, controlID);
		}

		/// <summary>
		///   <para>Get an existing state object from a controlID.</para>
		/// </summary>
		/// <param name="t"></param>
		/// <param name="controlID"></param>
		public static object QueryStateObject(Type t, int controlID)
		{
			return GUIStateObjects.QueryStateObject(t, controlID);
		}

		/// <summary>
		///   <para>The controlID of the current hot control.</para>
		/// </summary>
		public static int hotControl
		{
			get
			{
				return GUIUtility.Internal_GetHotControl();
			}
			set
			{
				GUIUtility.Internal_SetHotControl(value);
			}
		}

		public static void ExitGUI()
		{
			throw new ExitGUIException();
		}

		internal static GUISkin GetDefaultSkin()
		{
			return GUIUtility.Internal_GetDefaultSkin(GUIUtility.s_SkinMode);
		}

		internal static GUISkin GetBuiltinSkin(int skin)
		{
			return GUIUtility.Internal_GetBuiltinSkin(skin) as GUISkin;
		}

		internal static void BeginGUI(int skinMode, int instanceID, int useGUILayout)
		{
			GUIUtility.s_SkinMode = skinMode;
			GUIUtility.s_OriginalID = instanceID;
			GUI.skin = null;
			if (useGUILayout != 0)
			{
				GUILayoutUtility.SelectIDList(instanceID, false);
				GUILayoutUtility.Begin(instanceID);
			}
			GUI.changed = false;
		}

		internal static void EndGUI(int layoutType)
		{
			try
			{
				if (Event.current.type == EventType.Layout)
				{
					switch (layoutType)
					{
					case 1:
						GUILayoutUtility.Layout();
						break;
					case 2:
						GUILayoutUtility.LayoutFromEditorWindow();
						break;
					}
				}
				GUILayoutUtility.SelectIDList(GUIUtility.s_OriginalID, false);
				GUIContent.ClearStaticCache();
			}
			finally
			{
				GUIUtility.Internal_ExitGUI();
			}
		}

		internal static bool EndGUIFromException(Exception exception)
		{
			if (exception == null)
			{
				return false;
			}
			if (!(exception is ExitGUIException) && !(exception.InnerException is ExitGUIException))
			{
				return false;
			}
			GUIUtility.Internal_ExitGUI();
			return true;
		}

		internal static void CheckOnGUI()
		{
			if (GUIUtility.Internal_GetGUIDepth() <= 0)
			{
				throw new ArgumentException("You can only call GUI functions from inside OnGUI.");
			}
		}

		/// <summary>
		///   <para>Convert a point from GUI position to screen space.</para>
		/// </summary>
		/// <param name="guiPoint"></param>
		public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return GUIClip.Unclip(guiPoint) + GUIUtility.s_EditorScreenPointOffset;
		}

		internal static Rect GUIToScreenRect(Rect guiRect)
		{
			Vector2 vector = GUIUtility.GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
			guiRect.x = vector.x;
			guiRect.y = vector.y;
			return guiRect;
		}

		/// <summary>
		///   <para>Convert a point from screen space to GUI position.</para>
		/// </summary>
		/// <param name="screenPoint"></param>
		public static Vector2 ScreenToGUIPoint(Vector2 screenPoint)
		{
			return GUIClip.Clip(screenPoint) - GUIUtility.s_EditorScreenPointOffset;
		}

		public static Rect ScreenToGUIRect(Rect screenRect)
		{
			Vector2 vector = GUIUtility.ScreenToGUIPoint(new Vector2(screenRect.x, screenRect.y));
			screenRect.x = vector.x;
			screenRect.y = vector.y;
			return screenRect;
		}

		/// <summary>
		///   <para>Helper function to rotate the GUI around a point.</para>
		/// </summary>
		/// <param name="angle"></param>
		/// <param name="pivotPoint"></param>
		public static void RotateAroundPivot(float angle, Vector2 pivotPoint)
		{
			Matrix4x4 matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.identity;
			Vector2 vector = GUIClip.Unclip(pivotPoint);
			Matrix4x4 lhs = Matrix4x4.TRS(vector, Quaternion.Euler(0f, 0f, angle), Vector3.one) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
			GUI.matrix = lhs * matrix;
		}

		/// <summary>
		///   <para>Helper function to scale the GUI around a point.</para>
		/// </summary>
		/// <param name="scale"></param>
		/// <param name="pivotPoint"></param>
		public static void ScaleAroundPivot(Vector2 scale, Vector2 pivotPoint)
		{
			Matrix4x4 matrix = GUI.matrix;
			Vector2 vector = GUIClip.Unclip(pivotPoint);
			Matrix4x4 lhs = Matrix4x4.TRS(vector, Quaternion.identity, new Vector3(scale.x, scale.y, 1f)) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
			GUI.matrix = lhs * matrix;
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float Internal_GetPixelsPerPoint();

		/// <summary>
		///   <para>Get a unique ID for a control, using an integer as a hint to help ensure correct matching of IDs to controls.</para>
		/// </summary>
		/// <param name="hint"></param>
		/// <param name="focus"></param>
		/// <param name="position"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetControlID(int hint, FocusType focus);

		private static int Internal_GetNextControlID2(int hint, FocusType focusType, Rect rect)
		{
			return GUIUtility.INTERNAL_CALL_Internal_GetNextControlID2(hint, focusType, ref rect);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_Internal_GetNextControlID2(int hint, FocusType focusType, ref Rect rect);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetPermanentControlID();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_GetHotControl();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetHotControl(int value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void UpdateUndoName();

		/// <summary>
		///   <para>The controlID of the control that has keyboard focus.</para>
		/// </summary>
		public static extern int keyboardControl { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetDidGUIWindowsEatLastEvent(bool value);

		/// <summary>
		///   <para>Get access to the system-wide pasteboard.</para>
		/// </summary>
		public static extern string systemCopyBuffer { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern GUISkin Internal_GetDefaultSkin(int skinMode);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object Internal_GetBuiltinSkin(int skin);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_ExitGUI();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int Internal_GetGUIDepth();

		internal static extern bool mouseUsed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>A global property, which is true if a ModalWindow is being displayed, false otherwise.</para>
		/// </summary>
		public static extern bool hasModalWindow { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal static extern bool textFieldInput { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
