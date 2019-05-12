using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Renders a billboard.</para>
	/// </summary>
	public sealed class BillboardRenderer : Renderer
	{
		/// <summary>
		///   <para>The BillboardAsset to render.</para>
		/// </summary>
		public extern BillboardAsset billboard { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
