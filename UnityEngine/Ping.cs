using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Ping any given IP address (given in dot notation).</para>
	/// </summary>
	public sealed class Ping
	{
		private IntPtr pingWrapper;

		/// <summary>
		///   <para>Perform a ping to the supplied target IP address.</para>
		/// </summary>
		/// <param name="address"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Ping(string address);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DestroyPing();

		~Ping()
		{
			this.DestroyPing();
		}

		/// <summary>
		///   <para>Has the ping function completed?</para>
		/// </summary>
		public extern bool isDone { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>This property contains the ping time result after isDone returns true.</para>
		/// </summary>
		public extern int time { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The IP target of the ping.</para>
		/// </summary>
		public extern string ip { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
