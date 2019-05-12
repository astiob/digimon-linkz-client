using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Specialized values for the given states used by GUIStyle objects.</para>
	/// </summary>
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

		internal GUIStyleState(GUIStyle sourceStyle, IntPtr source)
		{
			this.m_SourceStyle = sourceStyle;
			this.m_Ptr = source;
			this.m_Background = this.GetBackgroundInternal();
		}

		~GUIStyleState()
		{
			if (this.m_SourceStyle == null)
			{
				this.Cleanup();
			}
		}

		/// <summary>
		///   <para>The background image used by GUI elements in this given state.</para>
		/// </summary>
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

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Init();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Cleanup();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetBackgroundInternal(Texture2D value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Texture2D GetBackgroundInternal();

		/// <summary>
		///   <para>The text color used by GUI elements in this state.</para>
		/// </summary>
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

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_textColor(out Color value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_textColor(ref Color value);
	}
}
