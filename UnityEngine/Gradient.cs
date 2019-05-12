using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Gradient used for animating colors.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class Gradient
	{
		internal IntPtr m_Ptr;

		/// <summary>
		///   <para>Create a new Gradient object.</para>
		/// </summary>
		public Gradient()
		{
			this.Init();
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Init();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Cleanup();

		~Gradient()
		{
			this.Cleanup();
		}

		/// <summary>
		///   <para>Calculate color at a given time.</para>
		/// </summary>
		/// <param name="time">Time of the key (0 - 1).</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color Evaluate(float time);

		/// <summary>
		///   <para>All color keys defined in the gradient.</para>
		/// </summary>
		public extern GradientColorKey[] colorKeys { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>All alpha keys defined in the gradient.</para>
		/// </summary>
		public extern GradientAlphaKey[] alphaKeys { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Setup Gradient with an array of color keys and alpha keys.</para>
		/// </summary>
		/// <param name="colorKeys">Color keys of the gradient (maximum 8 color keys).</param>
		/// <param name="alphaKeys">Alpha keys of the gradient (maximum 8 alpha keys).</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetKeys(GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys);
	}
}
