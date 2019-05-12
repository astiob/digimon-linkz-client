using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Behaviours are Components that can be enabled or disabled.</para>
	/// </summary>
	public class Behaviour : Component
	{
		/// <summary>
		///   <para>Enabled Behaviours are Updated, disabled Behaviours are not.</para>
		/// </summary>
		public extern bool enabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Has the Behaviour had enabled called.</para>
		/// </summary>
		public extern bool isActiveAndEnabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
