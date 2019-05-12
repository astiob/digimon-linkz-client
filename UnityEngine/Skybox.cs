using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A script interface for the.</para>
	/// </summary>
	public sealed class Skybox : Behaviour
	{
		/// <summary>
		///   <para>The material used by the skybox.</para>
		/// </summary>
		public extern Material material { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
