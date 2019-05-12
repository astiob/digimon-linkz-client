using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class GUIStyleState
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		private readonly GUIStyle m_SourceStyle;

		[NonSerialized]
		private Texture2D m_Background;

		public GUIStyleState()
		{
			this.Init();
		}

		private GUIStyleState(GUIStyle sourceStyle, IntPtr source)
		{
			this.m_SourceStyle = sourceStyle;
			this.m_Ptr = source;
		}

		[GeneratedByOldBindingsGenerator]
		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Init();

		[GeneratedByOldBindingsGenerator]
		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Cleanup();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetBackgroundInternal(Texture2D value);

		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Texture2D GetBackgroundInternalFromDeserialization();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Texture2D GetBackgroundInternal();

		public Color textColor
		{
			get
			{
				Color result;
				this.INTERNAL_get_textColor(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_textColor(ref value);
			}
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_textColor(out Color value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_textColor(ref Color value);

		internal static GUIStyleState ProduceGUIStyleStateFromDeserialization(GUIStyle sourceStyle, IntPtr source)
		{
			GUIStyleState guistyleState = new GUIStyleState(sourceStyle, source);
			guistyleState.m_Background = guistyleState.GetBackgroundInternalFromDeserialization();
			return guistyleState;
		}

		internal static GUIStyleState GetGUIStyleState(GUIStyle sourceStyle, IntPtr source)
		{
			GUIStyleState guistyleState = new GUIStyleState(sourceStyle, source);
			guistyleState.m_Background = guistyleState.GetBackgroundInternal();
			return guistyleState;
		}

		~GUIStyleState()
		{
			if (this.m_SourceStyle == null)
			{
				this.Cleanup();
			}
		}

		public Texture2D background
		{
			get
			{
				return this.GetBackgroundInternal();
			}
			set
			{
				this.SetBackgroundInternal(value);
				this.m_Background = value;
			}
		}
	}
}
