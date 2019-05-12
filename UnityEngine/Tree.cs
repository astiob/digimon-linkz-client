using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Tree Component for the tree creator.</para>
	/// </summary>
	public sealed class Tree : Component
	{
		/// <summary>
		///   <para>Data asociated to the Tree.</para>
		/// </summary>
		public extern ScriptableObject data { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Tells if there is wind data exported from SpeedTree are saved on this component.</para>
		/// </summary>
		public extern bool hasSpeedTreeWind { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
