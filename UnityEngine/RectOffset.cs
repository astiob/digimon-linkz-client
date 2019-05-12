using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Offsets for rectangles, borders, etc.</para>
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class RectOffset
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		private readonly GUIStyle m_SourceStyle;

		/// <summary>
		///   <para>Creates a new rectangle with offsets.</para>
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <param name="top"></param>
		/// <param name="bottom"></param>
		public RectOffset()
		{
			this.Init();
		}

		internal RectOffset(GUIStyle sourceStyle, IntPtr source)
		{
			this.m_SourceStyle = sourceStyle;
			this.m_Ptr = source;
		}

		/// <summary>
		///   <para>Creates a new rectangle with offsets.</para>
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <param name="top"></param>
		/// <param name="bottom"></param>
		public RectOffset(int left, int right, int top, int bottom)
		{
			this.Init();
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
		}

		~RectOffset()
		{
			if (this.m_SourceStyle == null)
			{
				this.Cleanup();
			}
		}

		public override string ToString()
		{
			return UnityString.Format("RectOffset (l:{0} r:{1} t:{2} b:{3})", new object[]
			{
				this.left,
				this.right,
				this.top,
				this.bottom
			});
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Init();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Cleanup();

		/// <summary>
		///   <para>Left edge size.</para>
		/// </summary>
		public extern int left { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Right edge size.</para>
		/// </summary>
		public extern int right { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Top edge size.</para>
		/// </summary>
		public extern int top { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Bottom edge size.</para>
		/// </summary>
		public extern int bottom { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Shortcut for left + right. (Read Only)</para>
		/// </summary>
		public extern int horizontal { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Shortcut for top + bottom. (Read Only)</para>
		/// </summary>
		public extern int vertical { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Add the border offsets to a rect.</para>
		/// </summary>
		/// <param name="rect"></param>
		public Rect Add(Rect rect)
		{
			return RectOffset.INTERNAL_CALL_Add(this, ref rect);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Rect INTERNAL_CALL_Add(RectOffset self, ref Rect rect);

		/// <summary>
		///   <para>Remove the border offsets from a rect.</para>
		/// </summary>
		/// <param name="rect"></param>
		public Rect Remove(Rect rect)
		{
			return RectOffset.INTERNAL_CALL_Remove(this, ref rect);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Rect INTERNAL_CALL_Remove(RectOffset self, ref Rect rect);
	}
}
